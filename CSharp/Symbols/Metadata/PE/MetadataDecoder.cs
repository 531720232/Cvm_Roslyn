// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Metadata;
using Microsoft.CodeAnalysis.PooledObjects;
using Roslyn.Utilities;

namespace Microsoft.CodeAnalysis.CSharp.Symbols.Metadata.PE
{
    /// <summary>
    /// Helper class to resolve metadata tokens and signatures.
    /// </summary>
    internal class MetadataDecoder : MetadataDecoder<PEModuleSymbol, TypeSymbol, MethodSymbol, FieldSymbol, Symbol>
    {
        /// <summary>
        /// Type context for resolving generic type arguments.
        /// </summary>
        private readonly PENamedTypeSymbol _typeContextOpt;

        /// <summary>
        /// Method context for resolving generic method type arguments.
        /// </summary>
        private readonly PEMethodSymbol _methodContextOpt;

        public MetadataDecoder(
            PEModuleSymbol moduleSymbol,
            PENamedTypeSymbol context) :
            this(moduleSymbol, context, null)
        {
        }

        public MetadataDecoder(
            PEModuleSymbol moduleSymbol,
            PEMethodSymbol context) :
            this(moduleSymbol, (PENamedTypeSymbol)context.ContainingType, context)
        {
        }

        public MetadataDecoder(
            PEModuleSymbol moduleSymbol) :
            this(moduleSymbol, null, null)
        {
        }

        private MetadataDecoder(PEModuleSymbol moduleSymbol, PENamedTypeSymbol typeContextOpt, PEMethodSymbol methodContextOpt)
            // TODO (tomat): if the containing assembly is a source assembly and we are about to decode assembly level attributes, we run into a cycle,
            // so for now ignore the assembly identity.
            : base(moduleSymbol.Module, (moduleSymbol.ContainingAssembly is PEAssemblySymbol) ? moduleSymbol.ContainingAssembly.Identity : null, SymbolFactory.Instance, moduleSymbol)
        {
            Debug.Assert((object)moduleSymbol != null);

            _typeContextOpt = typeContextOpt;
            _methodContextOpt = methodContextOpt;
        }

        internal PEModuleSymbol ModuleSymbol
        {
            get { return moduleSymbol; }
        }
        private void EnqueueTypeDefInterfacesAndBaseTypeOrThrow(Queue<Type> typeDefsToSearch, Queue<TypeSymbol> typeSymbolsToSearch, Type searchTypeDef)
        {
            foreach (var interfaceImplHandle in (searchTypeDef).GetInterfaces())
            {
               
                EnqueueTypeToken(typeDefsToSearch, typeSymbolsToSearch, interfaceImplHandle);
            }

            EnqueueTypeToken(typeDefsToSearch, typeSymbolsToSearch, (searchTypeDef).BaseType);
        }

        private void EnqueueTypeToken(Queue<Type> typeDefsToSearch, Queue<TypeSymbol> typeSymbolsToSearch, Type typeToken)
        {
           
                    typeDefsToSearch.Enqueue(typeToken);
              
        }
        internal ImmutableArray<MethodSymbol> GetExplicitlyOverriddenMethods(Type implementingTypeDef, MethodBase implementingMethodDef, TypeSymbol implementingTypeSymbol)
        {
            ArrayBuilder<MethodSymbol> resultBuilder = ArrayBuilder<MethodSymbol>.GetInstance();

            try
            {
                var mi = implementingTypeDef.GetInterfaces();

                foreach (var m in mi)
                {

                    var map = implementingTypeDef.GetInterfaceMap(m);

                    var m2 = map.TargetMethods;

                    foreach (var m3 in m2)
                    {
                        if (m3 == implementingMethodDef)
                        {
                        var    methodSymbol = FindMethodSymbolInSuperType(implementingTypeDef, m3);
                            if (methodSymbol != null)
                            {
                                resultBuilder.Add(methodSymbol);
                            }
                        }
                      
                    }
                }
            }
            catch 
            { }

            return resultBuilder.ToImmutableAndFree();
        }
        private MethodSymbol FindMethodSymbolInSuperType(Type searchTypeDef, MethodBase targetMethodDef)
        {
            try
            {
                // We're using queues (i.e. BFS), rather than stacks (i.e. DFS), because we expect the common case
                // to be implementing a method on an immediate supertype, rather than a remote ancestor.
                // We're using more than one queue for two reasons: 1) some of our TypeDef tokens come directly from the
                // metadata tables and we'd prefer not to manipulate the corresponding symbol objects; 2) we bump TypeDefs
                // to the front of the search order (i.e. ahead of symbols) because a MethodDef can correspond to a TypeDef
                // but not to a type ref (i.e. symbol).
                Queue<Type> typeDefsToSearch = new Queue<Type>();
                Queue<TypeSymbol> typeSymbolsToSearch = new Queue<TypeSymbol>();

                // A method def represents a method defined in this module, so we can
                // just search the method defs of this module.
                EnqueueTypeDefInterfacesAndBaseTypeOrThrow(typeDefsToSearch, typeSymbolsToSearch, searchTypeDef);

                //catch both cycles and duplicate interfaces
                HashSet<Type> visitedTypeDefTokens = new HashSet<Type>();
                HashSet<TypeSymbol> visitedTypeSymbols = new HashSet<TypeSymbol>();

                bool hasMoreTypeDefs;
                while ((hasMoreTypeDefs = (typeDefsToSearch.Count > 0)) || typeSymbolsToSearch.Count > 0)
                {
                    if (hasMoreTypeDefs)
                    {
                        System.Type typeDef = typeDefsToSearch.Dequeue();

                        if (!visitedTypeDefTokens.Contains(typeDef))
                        {
                            visitedTypeDefTokens.Add(typeDef);

                            foreach (var methodDef in (typeDef.GetMethods()))
                            {
                                if (methodDef == targetMethodDef)
                                {
                                    TypeSymbol typeSymbol = this.GetTypeOfToken(typeDef);
                                    return FindMethodSymbolInType(typeSymbol, targetMethodDef);
                                }
                            }

                            EnqueueTypeDefInterfacesAndBaseTypeOrThrow(typeDefsToSearch, typeSymbolsToSearch, typeDef);
                        }
                    }
                    else //has more type symbols
                    {
                        TypeSymbol typeSymbol = typeSymbolsToSearch.Dequeue();
                        Debug.Assert(typeSymbol.Equals(null));

                        if (!visitedTypeSymbols.Contains(typeSymbol))
                        {
                            visitedTypeSymbols.Add(typeSymbol);

                            //we're looking for a method def but we're currently on a type *ref*, so just enqueue supertypes

                            EnqueueTypeSymbolInterfacesAndBaseTypes(typeDefsToSearch, typeSymbolsToSearch, typeSymbol);
                        }
                    }
                }
            }
            catch (BadImageFormatException)
            { }

            return null;
        }

        protected override TypeSymbol GetGenericMethodTypeParamSymbol(int position)
        {
            if ((object)_methodContextOpt == null)
            {
                return new UnsupportedMetadataTypeSymbol(); // type parameter not associated with a method
            }

            var typeParameters = _methodContextOpt.TypeParameters;

            if (typeParameters.Length <= position)
            {
                return new UnsupportedMetadataTypeSymbol(); // type parameter position too large
            }

            return typeParameters[position];
        }

        protected override TypeSymbol GetGenericTypeParamSymbol(int position)
        {
            PENamedTypeSymbol type = _typeContextOpt;

            while ((object)type != null && (type.MetadataArity - type.Arity) > position)
            {
                type = type.ContainingSymbol as PENamedTypeSymbol;
            }

            if ((object)type == null || type.MetadataArity <= position)
            {
                return new UnsupportedMetadataTypeSymbol(); // position of type parameter too large
            }

            position -= type.MetadataArity - type.Arity;
            Debug.Assert(position >= 0 && position < type.Arity);

            return type.TypeParameters[position];
        }

        protected override ConcurrentDictionary<Type, TypeSymbol> GetTypeHandleToTypeMap()
        {
            return moduleSymbol.TypeHandleToTypeMap;
        }

        protected override ConcurrentDictionary<Type, TypeSymbol> GetTypeRefHandleToTypeMap()
        {
            return moduleSymbol.TypeRefHandleToTypeMap;
        }

        protected override TypeSymbol LookupNestedTypeDefSymbol(TypeSymbol container, ref MetadataTypeName emittedName)
        {
            var result = container.LookupMetadataType(ref emittedName);
            Debug.Assert((object)result != null);

            return result;
        }

        /// <summary>
        /// Lookup a type defined in referenced assembly.
        /// </summary>
        /// <param name="referencedAssemblyIndex"></param>
        /// <param name="emittedName"></param>
        protected override TypeSymbol LookupTopLevelTypeDefSymbol(
            int referencedAssemblyIndex,
            ref MetadataTypeName emittedName)
        {
            var assembly = moduleSymbol.GetReferencedAssemblySymbol(referencedAssemblyIndex);
            if ((object)assembly == null)
            {
                return new UnsupportedMetadataTypeSymbol();
            }

            try
            {
                return assembly.LookupTopLevelMetadataType(ref emittedName, digThroughForwardedTypes: true);
            }
            catch (Exception e) // Trying to get more useful Watson dumps.
            {
                throw ExceptionUtilities.Unreachable;
            }
        }

        /// <summary>
        /// Lookup a type defined in a module of a multi-module assembly.
        /// </summary>
        protected override TypeSymbol LookupTopLevelTypeDefSymbol(string moduleName, ref MetadataTypeName emittedName, out bool isNoPiaLocalType)
        {
            foreach (ModuleSymbol m in moduleSymbol.ContainingAssembly.Modules)
            {
                if (string.Equals(m.Name, moduleName, StringComparison.OrdinalIgnoreCase))
                {
                    if ((object)m == (object)moduleSymbol)
                    {
                        return moduleSymbol.LookupTopLevelMetadataType(ref emittedName, out isNoPiaLocalType);
                    }
                    else
                    {
                        isNoPiaLocalType = false;
                        return m.LookupTopLevelMetadataType(ref emittedName);
                    }
                }
            }

            isNoPiaLocalType = false;
            return new MissingMetadataTypeSymbol.TopLevel(new MissingModuleSymbolWithName(moduleSymbol.ContainingAssembly, moduleName), ref emittedName, SpecialType.None);
        }

        /// <summary>
        /// Lookup a type defined in this module.
        /// This method will be called only if the type we are
        /// looking for hasn't been loaded yet. Otherwise, MetadataDecoder
        /// would have found the type in TypeDefRowIdToTypeMap based on its 
        /// TypeDef row id. 
        /// </summary>
        protected override TypeSymbol LookupTopLevelTypeDefSymbol(ref MetadataTypeName emittedName, out bool isNoPiaLocalType)
        {
            return moduleSymbol.LookupTopLevelMetadataType(ref emittedName, out isNoPiaLocalType);
        }

        protected override int GetIndexOfReferencedAssembly(AssemblyIdentity identity)
        {
            // Go through all assemblies referenced by the current module and
            // find the one which *exactly* matches the given identity.
            // No unification will be performed
            var assemblies = this.moduleSymbol.GetReferencedAssemblies();
            for (int i = 0; i < assemblies.Length; i++)
            {
                if (identity.Equals(assemblies[i]))
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Perform a check whether the type or at least one of its generic arguments 
        /// is defined in the specified assemblies. The check is performed recursively. 
        /// </summary>
        public static bool IsOrClosedOverATypeFromAssemblies(TypeSymbol symbol, ImmutableArray<AssemblySymbol> assemblies)
        {
            switch (symbol.Kind)
            {
                case SymbolKind.TypeParameter:
                    return false;

                case SymbolKind.ArrayType:
                    return IsOrClosedOverATypeFromAssemblies(((ArrayTypeSymbol)symbol).ElementType.TypeSymbol, assemblies);

                case SymbolKind.PointerType:
                    return IsOrClosedOverATypeFromAssemblies(((PointerTypeSymbol)symbol).PointedAtType.TypeSymbol, assemblies);

                case SymbolKind.DynamicType:
                    return false;

                case SymbolKind.ErrorType:
                    goto case SymbolKind.NamedType;
                case SymbolKind.NamedType:
                    var namedType = (NamedTypeSymbol)symbol;
                    AssemblySymbol containingAssembly = symbol.OriginalDefinition.ContainingAssembly;
                    int i;

                    if ((object)containingAssembly != null)
                    {
                        for (i = 0; i < assemblies.Length; i++)
                        {
                            if (ReferenceEquals(containingAssembly, assemblies[i]))
                            {
                                return true;
                            }
                        }
                    }

                    do
                    {
                        if (namedType.IsTupleType)
                        {
                            return IsOrClosedOverATypeFromAssemblies(namedType.TupleUnderlyingType, assemblies);
                        }

                        var arguments = namedType.TypeArgumentsNoUseSiteDiagnostics;
                        int count = arguments.Length;

                        for (i = 0; i < count; i++)
                        {
                            if (IsOrClosedOverATypeFromAssemblies(arguments[i].TypeSymbol, assemblies))
                            {
                                return true;
                            }
                        }

                        namedType = namedType.ContainingType;
                    }
                    while ((object)namedType != null);

                    return false;

                default:
                    throw ExceptionUtilities.UnexpectedValue(symbol.Kind);
            }
        }

        protected override TypeSymbol SubstituteNoPiaLocalType(
            Type typeDef,
            ref MetadataTypeName name,
            string interfaceGuid,
            string scope,
            string identifier)
        {
            TypeSymbol result;

            try
            {
                bool isInterface = (typeDef).IsInterface;
                TypeSymbol baseType = null;

                if (!isInterface)
                {
                    var baseToken =(typeDef).BaseType;

                    if (baseToken!=null)
                    {
                        baseType = GetTypeOfToken(baseToken);
                    }
                }

                result = SubstituteNoPiaLocalType(
                    ref name,
                    isInterface,
                    baseType,
                    interfaceGuid,
                    scope,
                    identifier,
                    moduleSymbol.ContainingAssembly);
            }
            catch (BadImageFormatException mrEx)
            {
                result = GetUnsupportedMetadataTypeSymbol(mrEx);
            }

            Debug.Assert((object)result != null);

            ConcurrentDictionary<Type, TypeSymbol> cache = GetTypeHandleToTypeMap();
            Debug.Assert(cache != null);

            TypeSymbol newresult = cache.GetOrAdd(typeDef, result);
            Debug.Assert(ReferenceEquals(newresult, result) || (newresult.Kind == SymbolKind.ErrorType));

            return newresult;
        }

        /// <summary>
        /// Find canonical type for NoPia embedded type.
        /// </summary>
        /// <returns>
        /// Symbol for the canonical type or an ErrorTypeSymbol. Never returns null.
        /// </returns>
        internal static NamedTypeSymbol SubstituteNoPiaLocalType(
            ref MetadataTypeName name,
            bool isInterface,
            TypeSymbol baseType,
            string interfaceGuid,
            string scope,
            string identifier,
            AssemblySymbol referringAssembly)
        {
            NamedTypeSymbol result = null;

            Guid interfaceGuidValue = new Guid();
            bool haveInterfaceGuidValue = false;
            Guid scopeGuidValue = new Guid();
            bool haveScopeGuidValue = false;

            if (isInterface && interfaceGuid != null)
            {
                haveInterfaceGuidValue = Guid.TryParse(interfaceGuid, out interfaceGuidValue);

                if (haveInterfaceGuidValue)
                {
                    // To have consistent errors.
                    scope = null;
                    identifier = null;
                }
            }

            if (scope != null)
            {
                haveScopeGuidValue = Guid.TryParse(scope, out scopeGuidValue);
            }

            foreach (AssemblySymbol assembly in referringAssembly.GetNoPiaResolutionAssemblies())
            {
                Debug.Assert((object)assembly != null);
                if (ReferenceEquals(assembly, referringAssembly))
                {
                    continue;
                }

                NamedTypeSymbol candidate = assembly.LookupTopLevelMetadataType(ref name, digThroughForwardedTypes: false);
                Debug.Assert(!candidate.IsGenericType);

                // Ignore type forwarders, error symbols and non-public types
                if (candidate.Kind == SymbolKind.ErrorType ||
                    !ReferenceEquals(candidate.ContainingAssembly, assembly) ||
                    candidate.DeclaredAccessibility != Accessibility.Public)
                {
                    continue;
                }

                // Ignore NoPia local types.
                // If candidate is coming from metadata, we don't need to do any special check,
                // because we do not create symbols for local types. However, local types defined in source 
                // is another story. However, if compilation explicitly defines a local type, it should be
                // represented by a retargeting assembly, which is supposed to hide the local type.
                Debug.Assert(!(assembly is SourceAssemblySymbol) || !((SourceAssemblySymbol)assembly).SourceModule.MightContainNoPiaLocalTypes());

                string candidateGuid;
                bool haveCandidateGuidValue = false;
                Guid candidateGuidValue = new Guid();

                // The type must be of the same kind (interface, struct, delegate or enum).
                switch (candidate.TypeKind)
                {
                    case TypeKind.Interface:
                        if (!isInterface)
                        {
                            continue;
                        }

                        // Get candidate's Guid
                        if (candidate.GetGuidString(out candidateGuid) && candidateGuid != null)
                        {
                            haveCandidateGuidValue = Guid.TryParse(candidateGuid, out candidateGuidValue);
                        }

                        break;

                    case TypeKind.Delegate:
                    case TypeKind.Enum:
                    case TypeKind.Struct:

                        if (isInterface)
                        {
                            continue;
                        }

                        // Let's use a trick. To make sure the kind is the same, make sure
                        // base type is the same.
                        SpecialType baseSpecialType = (candidate.BaseTypeNoUseSiteDiagnostics?.SpecialType).GetValueOrDefault();
                        if (baseSpecialType == SpecialType.None || baseSpecialType != (baseType?.SpecialType).GetValueOrDefault())
                        {
                            continue;
                        }

                        break;

                    default:
                        continue;
                }

                if (haveInterfaceGuidValue || haveCandidateGuidValue)
                {
                    if (!haveInterfaceGuidValue || !haveCandidateGuidValue ||
                        candidateGuidValue != interfaceGuidValue)
                    {
                        continue;
                    }
                }
                else
                {
                    if (!haveScopeGuidValue || identifier == null || !identifier.Equals(name.FullName))
                    {
                        continue;
                    }

                    // Scope guid must match candidate's assembly guid.
                    haveCandidateGuidValue = false;
                    if (assembly.GetGuidString(out candidateGuid) && candidateGuid != null)
                    {
                        haveCandidateGuidValue = Guid.TryParse(candidateGuid, out candidateGuidValue);
                    }

                    if (!haveCandidateGuidValue || scopeGuidValue != candidateGuidValue)
                    {
                        continue;
                    }
                }

                // OK. It looks like we found canonical type definition.
                if ((object)result != null)
                {
                    // Ambiguity 
                    result = new NoPiaAmbiguousCanonicalTypeSymbol(referringAssembly, result, candidate);
                    break;
                }

                result = candidate;
            }

            if ((object)result == null)
            {
                result = new NoPiaMissingCanonicalTypeSymbol(
                                referringAssembly,
                                name.FullName,
                                interfaceGuid,
                                scope,
                                identifier);
            }

            return result;
        }

        internal static ObsoleteAttributeData GetOb(ICustomAttributeProvider handle)
        {
            ObsoleteAttributeData data = ObsoleteAttributeData.Uninitialized;
            var objs = handle.GetCustomAttributes(typeof(System.ObsoleteAttribute), false);
            {
                foreach (var obj in objs)
                {
                    if (obj is ObsoleteAttribute attr)
                    {
                        data = new ObsoleteAttributeData(ObsoleteAttributeKind.Obsolete, attr.Message, attr.IsError);
                        break;
                    }
                }
            }
            return data;
        }

        protected override MethodSymbol FindMethodSymbolInType(TypeSymbol typeSymbol, MethodBase targetMethodDef)
        {
            Debug.Assert(typeSymbol is PENamedTypeSymbol || typeSymbol is ErrorTypeSymbol);

            foreach (Symbol member in typeSymbol.GetMembersUnordered())
            {
                PEMethodSymbol method = member as PEMethodSymbol;
                if ((object)method != null && method.Handle == targetMethodDef)
                {
                    return method;
                }
            }

            return null;
        }

        protected override FieldSymbol FindFieldSymbolInType(TypeSymbol typeSymbol, FieldInfo fieldDef)
        {
            Debug.Assert(typeSymbol is PENamedTypeSymbol || typeSymbol is ErrorTypeSymbol);

            foreach (Symbol member in typeSymbol.GetMembersUnordered())
            {
                PEFieldSymbol field = member as PEFieldSymbol;
                if ((object)field != null && field.Handle == fieldDef)
                {
                    return field;
                }
            }

            return null;
        }

        internal override Symbol GetSymbolForMemberRef(MemberInfo memberRef, TypeSymbol scope = null, bool methodsOnly = false)
        {
            TypeSymbol targetTypeSymbol = GetMemberRefTypeSymbol(memberRef);

            if ((object)scope != null)
            {
                Debug.Assert(scope.Kind == SymbolKind.NamedType || scope.Kind == SymbolKind.ErrorType);

                // We only want to consider members that are at or above "scope" in the type hierarchy.
                HashSet<DiagnosticInfo> useSiteDiagnostics = null;
                if (!TypeSymbol.Equals(scope, targetTypeSymbol, TypeCompareKind.ConsiderEverything2) &&
                    !(targetTypeSymbol.IsInterfaceType()
                        ? scope.AllInterfacesNoUseSiteDiagnostics.IndexOf((NamedTypeSymbol)targetTypeSymbol, 0, TypeSymbol.EqualsIgnoringNullableComparer) != -1
                        : scope.IsDerivedFrom(targetTypeSymbol, TypeCompareKind.IgnoreNullableModifiersForReferenceTypes, useSiteDiagnostics: ref useSiteDiagnostics)))
                {
                    return null;
                }
            }

            // We're going to use a special decoder that can generate usable symbols for type parameters without full context.
            // (We're not just using a different type - we're also changing the type context.)
            var memberRefDecoder = new MemberRefMetadataDecoder(moduleSymbol, targetTypeSymbol);

            return memberRefDecoder.FindMember(targetTypeSymbol, memberRef, methodsOnly);
        }

        protected override void EnqueueTypeSymbolInterfacesAndBaseTypes(Queue<Type> typeDefsToSearch, Queue<TypeSymbol> typeSymbolsToSearch, TypeSymbol typeSymbol)
        {
            foreach (NamedTypeSymbol @interface in typeSymbol.InterfacesNoUseSiteDiagnostics())
            {
                EnqueueTypeSymbol(typeDefsToSearch, typeSymbolsToSearch, @interface);
            }

            EnqueueTypeSymbol(typeDefsToSearch, typeSymbolsToSearch, typeSymbol.BaseTypeNoUseSiteDiagnostics);
        }

        protected override void EnqueueTypeSymbol(Queue<Type> typeDefsToSearch, Queue<TypeSymbol> typeSymbolsToSearch, TypeSymbol typeSymbol)
        {
            if ((object)typeSymbol != null)
            {
                PENamedTypeSymbol peTypeSymbol = typeSymbol as PENamedTypeSymbol;
                if ((object)peTypeSymbol != null && ReferenceEquals(peTypeSymbol.ContainingPEModule, moduleSymbol))
                {
                    typeDefsToSearch.Enqueue(peTypeSymbol.Handle);
                }
                else
                {
                    typeSymbolsToSearch.Enqueue(typeSymbol);
                }
            }
        }

        protected override MethodBase GetMethodHandle(MethodSymbol method)
        {
            PEMethodSymbol peMethod = method as PEMethodSymbol;
            if ((object)peMethod != null && ReferenceEquals(peMethod.ContainingModule, moduleSymbol))
            {
                return peMethod.Handle;
            }

            return default(MethodBase);
        }
    }
}
