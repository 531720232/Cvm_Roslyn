﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using Microsoft.CodeAnalysis.PooledObjects;
using Roslyn.Utilities;

namespace Microsoft.CodeAnalysis
{
    [StructLayout(LayoutKind.Auto)]
    internal struct ModifierInfo<TypeSymbol>
        where TypeSymbol : class
    {
        internal readonly bool IsOptional;
        internal readonly TypeSymbol Modifier;

        public ModifierInfo(bool isOptional, TypeSymbol modifier)
        {
            IsOptional = isOptional;
            Modifier = modifier;
        }
    }

    [StructLayout(LayoutKind.Auto)]
    internal struct ParamInfo<TypeSymbol>
        where TypeSymbol : class
    {
        internal bool IsByRef;
        internal TypeSymbol Type;
        internal ParameterInfo Handle; // may be nil
        internal ImmutableArray<ModifierInfo<TypeSymbol>> RefCustomModifiers;
        internal ImmutableArray<ModifierInfo<TypeSymbol>> CustomModifiers;
    }

    [StructLayout(LayoutKind.Auto)]
    internal struct LocalInfo<TypeSymbol>
        where TypeSymbol : class
    {
        internal readonly byte[] SignatureOpt;
        internal readonly TypeSymbol Type;
        internal readonly ImmutableArray<ModifierInfo<TypeSymbol>> CustomModifiers;
        internal readonly LocalSlotConstraints Constraints;

        internal LocalInfo(TypeSymbol type, ImmutableArray<ModifierInfo<TypeSymbol>> customModifiers, LocalSlotConstraints constraints, byte[] signatureOpt)
        {
            Debug.Assert(type != null);

            this.Type = type;
            this.CustomModifiers = customModifiers;
            this.Constraints = constraints;
            this.SignatureOpt = signatureOpt;
        }

        internal LocalInfo<TypeSymbol> WithSignature(byte[] signature)
        {
            return new LocalInfo<TypeSymbol>(this.Type, this.CustomModifiers, this.Constraints, signature);
        }

        public bool IsByRef => (Constraints & LocalSlotConstraints.ByRef) != 0;

        public bool IsPinned => (Constraints & LocalSlotConstraints.Pinned) != 0;
    }

    internal abstract class MetadataDecoder<ModuleSymbol, TypeSymbol, MethodSymbol, FieldSymbol, Symbol> :
        TypeNameDecoder<ModuleSymbol, TypeSymbol>
        where ModuleSymbol : class
        where TypeSymbol : class, Symbol, ITypeSymbol
        where MethodSymbol : class, Symbol, IMethodSymbol
        where FieldSymbol : class, Symbol, IFieldSymbol
        where Symbol : class, ISymbol
    {
        public readonly PEModule Module;

        // Identity of an assembly containing the module, or null if the module is a standalone module
        private readonly AssemblyIdentity _containingAssemblyIdentity;

        internal MetadataDecoder(PEModule module, AssemblyIdentity containingAssemblyIdentity, SymbolFactory<ModuleSymbol, TypeSymbol> factory, ModuleSymbol moduleSymbol) :
            base(factory, moduleSymbol)
        {
            Debug.Assert(module != null);
            this.Module = module;
            _containingAssemblyIdentity = containingAssemblyIdentity;
        }

        internal TypeSymbol GetTypeOfToken(Type token)
        {
            return GetType1(token);
        }

        internal TypeSymbol GetType1(Type token)
        {
            Debug.Assert(token != null);

            TypeSymbol type;



            type = GetTypeOfTypeDef(token, isContainingType: false);
           
            Debug.Assert(type != null);
            return type;
        }
        private TypeSymbol GetTypeOfTypeDef(Type typeDef, bool isContainingType)
        {
            try
            {
                // This is a cache similar to one used in MetaImport::GetTypeOfToken by native compiler.
                // TypeDef tokens are unique within Module.
                // This cache makes lookup of top level types about twice as fast, about three times as fast if 
                // EmittedNameToTypeMap in LookupTopLevelType doesn't contain the name. 
                // It is likely that gain for nested types will be bigger because we don't cache names of nested types.

                ConcurrentDictionary<Type, TypeSymbol> cache = GetTypeHandleToTypeMap();

                TypeSymbol result;

                if (cache != null && cache.TryGetValue(typeDef, out result))
                {
                    //if (!(typeDef).IsNested )
                    //{
                    //    isNoPiaLocalType = true;
                    //}
                    //else
                    //{
                  
                    //}

                    return result;
                }

                MetadataTypeName mdName;
                string name = (typeDef).Name;
                Debug.Assert(MetadataHelpers.IsValidMetadataIdentifier(name));

                if (typeDef.IsNested)
                {
                    // first resolve nesting type 
                    Type containerTypeDef = typeDef.DeclaringType;

                    // invalid metadata?
                    if (containerTypeDef == null)
                    {
                       
                        return GetUnsupportedMetadataTypeSymbol();
                    }

                    TypeSymbol container = GetTypeOfTypeDef(containerTypeDef,  isContainingType: true);

                 

                    mdName = MetadataTypeName.FromTypeName(name);
                    return LookupNestedTypeDefSymbol(container, ref mdName);
                }

                string namespaceName = typeDef.Namespace;

                mdName = namespaceName.Length > 0
                    ? MetadataTypeName.FromNamespaceAndTypeName(namespaceName, name)
                    : MetadataTypeName.FromTypeName(name);
                // It is extremely difficult to hit the last branch because it is executed 
                // only for types in the Global namespace and they are getting loaded 
                // as soon as we start traversing Symbol Table, therefore, their TypeDef
                // handle is getting cached and lookup in the cache succeeds. 
                // Probably we can hit it if the first thing we do is to interrogate 
                // Module/Assembly level attributes, which refer to a TypeDef in the 
                // Global namespace.

                // Check if this is NoPia local type which should be substituted 
                // with corresponding canonical type
                string interfaceGuid;
                string scope;
                string identifier;


            
                result = LookupTopLevelTypeDefSymbol(ref mdName, out bool isNoPiaLocalType);
             
                return result;
            }
            catch 
            {
           
                return GetUnsupportedMetadataTypeSymbol(); // an exception from metadata reader.
            }
        }
        //internal TypeSymbol GetTypeOfToken(EntityHandle token, out bool isNoPiaLocalType)
        //{
        //    Debug.Assert(!token.IsNil);

        //    TypeSymbol type;
        //    HandleKind tokenType = token.Kind;

        //    switch (tokenType)
        //    {
        //        case HandleKind.TypeDefinition:
        //            type = GetTypeOfTypeDef((TypeDefinitionHandle)token, out isNoPiaLocalType, isContainingType: false);
        //            break;

        //        case HandleKind.TypeSpecification:
        //            isNoPiaLocalType = false;
        //            type = GetTypeOfTypeSpec((TypeSpecificationHandle)token);
        //            break;

        //        case HandleKind.TypeReference:
        //            type = GetTypeOfTypeRef((TypeReferenceHandle)token, out isNoPiaLocalType);
        //            break;

        //        default:
        //            isNoPiaLocalType = false;
        //            type = GetUnsupportedMetadataTypeSymbol();
        //            break;
        //    }

        //    Debug.Assert(type != null);
        //    return type;
        //}

        //private TypeSymbol GetTypeOfTypeSpec(TypeSpecificationHandle typeSpec)
        //{
        //    TypeSymbol ptype;

        //    try
        //    {
        //        BlobReader memoryReader = this.Module.GetTypeSpecificationSignatureReaderOrThrow(typeSpec);

        //        bool refersToNoPiaLocalType;
        //        ptype = DecodeTypeOrThrow(ref memoryReader, out refersToNoPiaLocalType);
        //    }
        //    catch (BadImageFormatException mrEx)
        //    {
        //        ptype = GetUnsupportedMetadataTypeSymbol(mrEx); // an exception from metadata reader.
        //    }
        //    catch (UnsupportedSignatureContent)
        //    {
        //        ptype = GetUnsupportedMetadataTypeSymbol(); // unsupported signature content
        //    }

        //    return ptype;
        //}

        /// <exception cref="UnsupportedSignatureContent">If the encoded type is invalid.</exception>
        /// <exception cref="BadImageFormatException">An exception from metadata reader.</exception>
        internal static SpecialType ToSpecialType(TypeCode typeCode)
        {

            switch (typeCode)
            {


                case TypeCode.Boolean:
                    return SpecialType.System_Boolean;

                case TypeCode.SByte:
                    return SpecialType.System_SByte;

                case TypeCode.Byte:
                    return SpecialType.System_Byte;

                case TypeCode.Int16:
                    return SpecialType.System_Int16;

                case TypeCode.UInt16:
                    return SpecialType.System_UInt16;

                case TypeCode.Int32:
                    return SpecialType.System_Int32;

                case TypeCode.UInt32:
                    return SpecialType.System_UInt32;

                case TypeCode.Int64:
                    return SpecialType.System_Int64;

                case TypeCode.UInt64:
                    return SpecialType.System_UInt64;

                case TypeCode.Single:
                    return SpecialType.System_Single;

                case TypeCode.Double:
                    return SpecialType.System_Double;

                case TypeCode.Char:
                    return SpecialType.System_Char;

                case TypeCode.String:
                    return SpecialType.System_String;



                case TypeCode.Object:
                    return SpecialType.System_Object;

                default:
                    throw ExceptionUtilities.UnexpectedValue(typeCode);
            }
        }

        /// <exception cref="UnsupportedSignatureContent">If the encoded type is invalid.</exception>
        /// <exception cref="BadImageFormatException">An exception from metadata reader.</exception>
        private TypeSymbol DecodeTypeOrThrow(Type typeCode, out bool refersToNoPiaLocalType)
        {

            TypeSymbol typeSymbol = default;
            int paramPosition;
            ImmutableArray<ModifierInfo<TypeSymbol>> modifiers;

            refersToNoPiaLocalType = false;
            var tc = Type.GetTypeCode(typeCode);
            // Switch on the type.

            switch (tc)
            {
                case TypeCode.Empty:
                case TypeCode.DBNull:
                case TypeCode.Boolean:
                case TypeCode.Char:
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                case TypeCode.DateTime:
                case TypeCode.String:
                    typeSymbol = GetSpecialType(ToSpecialType(tc));
                    return typeSymbol;
            }

            if (typeCode.FullName == "System.Void")
            {
                typeSymbol = GetSpecialType(SpecialType.System_Void);
                return typeSymbol;
            }
            if (typeCode.FullName == "System.Object")
            {
                typeSymbol = GetSpecialType(SpecialType.System_Object);
                return typeSymbol;
            }
       

            if (typeCode.IsArray)
            {
                typeSymbol = DecodeTypeOrThrow(typeCode.GetElementType(), out refersToNoPiaLocalType);
                var rank = typeCode.GetArrayRank();
                if (rank == 1)//SZArray
                {
                    typeSymbol = GetSZArrayTypeSymbol(typeSymbol, ImmutableArray<ModifierInfo<TypeSymbol>>.Empty);
                }
                else
                {
                    typeSymbol = GetCSArrayTypeSymbol(rank, typeSymbol, ImmutableArray<ModifierInfo<TypeSymbol>>.Empty);


                }
                return typeSymbol;
            }
            if (typeCode.IsPointer)
            {
                typeSymbol = DecodeTypeOrThrow(typeCode.GetElementType(), out refersToNoPiaLocalType);
                typeSymbol = MakePointerTypeSymbol(typeSymbol, ImmutableArray<ModifierInfo<TypeSymbol>>.Empty);
                return typeSymbol;
            }
            if (typeCode.GetGenericArguments().Length > 0)
            {
                typeSymbol = DecodeTypeOrThrow(typeCode.GetElementType(), out refersToNoPiaLocalType);
                typeSymbol = MakePointerTypeSymbol(typeSymbol, ImmutableArray<ModifierInfo<TypeSymbol>>.Empty);
                return typeSymbol;
            }



            return typeSymbol;
        }

        /// <exception cref="UnsupportedSignatureContent">If the encoded type is invalid.</exception>
        /// <exception cref="BadImageFormatException">An exception from metadata reader.</exception>
        internal TypeSymbol GetSymbolForTypeHandleOrThrow(Type handle, out bool isNoPiaLocalType, bool allowTypeSpec, bool requireShortForm)
        {
            if (handle==null)
            {
                throw new UnsupportedSignatureContent();
            }

            TypeSymbol typeSymbol;
            isNoPiaLocalType = false;
            typeSymbol = GetTypeOfTypeDef(handle, isContainingType: false);
            //switch (handle.Kind)
            //{
            //    case HandleKind.TypeDefinition:
            //        typeSymbol = GetTypeOfTypeDef((TypeDefinitionHandle)handle, out isNoPiaLocalType, isContainingType: false);
            //        break;

            //    case HandleKind.TypeReference:
            //        typeSymbol = GetTypeOfTypeRef((TypeReferenceHandle)handle, out isNoPiaLocalType);
            //        break;

            //    case HandleKind.TypeSpecification:
            //        if (!allowTypeSpec)
            //        {
            //            throw new UnsupportedSignatureContent();
            //        }

            //        isNoPiaLocalType = false;
            //        typeSymbol = GetTypeOfTypeSpec((TypeSpecificationHandle)handle);
            //        break;

            //    default:
            //        throw ExceptionUtilities.UnexpectedValue(handle.Kind);
            //}

            // tomat: Breaking change
            // Metadata spec II.23.2.16 (Short form signatures) requires primitive types to be encoded using a short form:
            // 
            //  "The general specification for signatures leaves some leeway in how to encode certain items. For
            //   example, it appears valid to encode a String as either 
            //     long-form: (ELEMENT_TYPE_CLASS, TypeRef-to-System.String )
            //     short-form: ELEMENT_TYPE_STRING
            //   Only the short form is valid."
            // 
            // Native compilers accept long form signatures (actually IMetadataImport does).
            // When a MemberRef is emitted the signature blob is copied from the metadata reference to the resulting assembly. 
            // Such assembly doesn't PEVerify but the CLR type loader matches the MemberRef with the original signature 
            // (since they are identical copies).
            // 
            // Roslyn doesn't copy signature blobs to the resulting assembly, it encodes the MemberRef using the 
            // correct short type codes. If we allowed long forms in a signature we would produce IL that PEVerifies but
            // the type loader isn't able to load it since the MemberRef signature wouldn't match the original signature.
            // 
            // Rather then producing broken code we report an error at compile time.

            if (requireShortForm && typeSymbol.SpecialType.HasShortFormSignatureEncoding())
            {
                throw new UnsupportedSignatureContent();
            }

            return typeSymbol;
        }

        // MetaImport::GetTypeOfTypeRef
        private TypeSymbol GetTypeOfTypeRef(Type typeRef, out bool isNoPiaLocalType)
        {
            // This is a cache similar to one used by native compiler in MetaImport::GetTypeOfTypeRef.
            // TypeRef tokens are unique only within a Module
            ConcurrentDictionary<Type, TypeSymbol> cache = GetTypeRefHandleToTypeMap();
            TypeSymbol result;

            if (cache != null && cache.TryGetValue(typeRef, out result))
            {
                isNoPiaLocalType = false; // We do not cache otherwise.
                return result;
            }

            try
            {
                string name, @namespace;
                //Type resolutionScope;
                //  System.Reflection.Module w;
                name = typeRef.Name;
                @namespace = typeRef.Namespace;

                //  Module.GetTypeRefPropsOrThrow(typeRef, out name, out @namespace, out resolutionScope);
                Debug.Assert(MetadataHelpers.IsValidMetadataIdentifier(name));

                MetadataTypeName mdName = @namespace.Length > 0
                    ? MetadataTypeName.FromNamespaceAndTypeName(@namespace, name)
                    : MetadataTypeName.FromTypeName(name);

                result = GetTypeByNameOrThrow(ref mdName, typeRef, out isNoPiaLocalType);
            }
            catch (BadImageFormatException mrEx)
            {
                result = GetUnsupportedMetadataTypeSymbol(mrEx); // an exception from metadata reader.
                isNoPiaLocalType = false;
            }

            Debug.Assert(result != null);

            // Cache the result, but only if it is not a local type because the cache doesn't retain this information.
            if (cache != null && !isNoPiaLocalType)
            {
                TypeSymbol result1 = cache.GetOrAdd(typeRef, result);
                Debug.Assert(result1.Equals(result));
            }

            return result;
        }


        // MetaImport::GetTypeByName
        /// <exception cref="BadImageFormatException">An exception from metadata reader.</exception>
        private TypeSymbol GetTypeByNameOrThrow(
            ref MetadataTypeName fullName,
            Type tokenResolutionScope,
            out bool isNoPiaLocalType)
        {
            var tokenType = tokenResolutionScope.GetType();
            var t1 = typeof(System.TypedReference);
            // TODO: I believe refs can be parented by a def tokens too, not common, but.
            //       Should also do NoPia related checks.

            // The resolution scope should be either a type ref, an assembly or a module.

            if (tokenResolutionScope == null)
            {
                throw new BadImageFormatException();
            }
            TypeSymbol psymContainer = GetType1(tokenResolutionScope);
            Debug.Assert(fullName.NamespaceName.Length == 0);
            isNoPiaLocalType = false;
            //    return LookupNestedTypeDefSymbol(psymContainer, ref fullName);

            isNoPiaLocalType = false;
            return GetUnsupportedMetadataTypeSymbol();
        }

        private TypeSymbol GetTypeOfTypeDef(Type typeDef)
        {
            bool isNoPiaLocalType;
            return GetTypeOfTypeDef(typeDef, out isNoPiaLocalType, isContainingType: false);
        }

        private TypeSymbol GetTypeOfTypeDef(Type typeDef, out bool isNoPiaLocalType, bool isContainingType)
        {
            try
            {
                // This is a cache similar to one used in MetaImport::GetTypeOfToken by native compiler.
                // TypeDef tokens are unique within Module.
                // This cache makes lookup of top level types about twice as fast, about three times as fast if 
                // EmittedNameToTypeMap in LookupTopLevelType doesn't contain the name. 
                // It is likely that gain for nested types will be bigger because we don't cache names of nested types.

                ConcurrentDictionary<Type, TypeSymbol> cache = GetTypeHandleToTypeMap();

                TypeSymbol result;

                if (cache != null && cache.TryGetValue(typeDef, out result))
                {
                    //if (!(typeDef).IsNested )
                    //{
                    //    isNoPiaLocalType = true;
                    //}
                    //else
                    //{
                    isNoPiaLocalType = false;
                    //}

                    return result;
                }

                MetadataTypeName mdName;
                string name = (typeDef).Name;
                Debug.Assert(MetadataHelpers.IsValidMetadataIdentifier(name));

                if (typeDef.IsNested)
                {
                    // first resolve nesting type 
                    Type containerTypeDef = typeDef.DeclaringType;

                    // invalid metadata?
                    if (containerTypeDef == null)
                    {
                        isNoPiaLocalType = false;
                        return GetUnsupportedMetadataTypeSymbol();
                    }

                    TypeSymbol container = GetTypeOfTypeDef(containerTypeDef, out isNoPiaLocalType, isContainingType: true);

                    if (isNoPiaLocalType)
                    {
                        // Types nested into local types are not supported.
                        if (!isContainingType)
                        {
                            isNoPiaLocalType = false;
                        }

                        return GetUnsupportedMetadataTypeSymbol();
                    }

                    mdName = MetadataTypeName.FromTypeName(name);
                    return LookupNestedTypeDefSymbol(container, ref mdName);
                }

                string namespaceName = typeDef.Namespace;

                mdName = namespaceName.Length > 0
                    ? MetadataTypeName.FromNamespaceAndTypeName(namespaceName, name)
                    : MetadataTypeName.FromTypeName(name);
                // It is extremely difficult to hit the last branch because it is executed 
                // only for types in the Global namespace and they are getting loaded 
                // as soon as we start traversing Symbol Table, therefore, their TypeDef
                // handle is getting cached and lookup in the cache succeeds. 
                // Probably we can hit it if the first thing we do is to interrogate 
                // Module/Assembly level attributes, which refer to a TypeDef in the 
                // Global namespace.

                // Check if this is NoPia local type which should be substituted 
                // with corresponding canonical type
                string interfaceGuid;
                string scope;
                string identifier;


                isNoPiaLocalType = false;
                result = LookupTopLevelTypeDefSymbol(ref mdName, out isNoPiaLocalType);
                Debug.Assert(!isNoPiaLocalType);
                return result;
            }
            catch (BadImageFormatException mrEx)
            {
                isNoPiaLocalType = false;
                return GetUnsupportedMetadataTypeSymbol(mrEx); // an exception from metadata reader.
            }
        }

        /// <exception cref="UnsupportedSignatureContent">If the encoded type is invalid.</exception>
        /// <exception cref="BadImageFormatException">An exception from metadata reader.</exception>
        private ImmutableArray<ModifierInfo<TypeSymbol>> DecodeModifiersOrThrow(
              System.Reflection.ParameterInfo signatureReader,
            AllowedRequiredModifierType allowedRequiredModifierType,
            
            out bool requiredModifierFound)
        {
            requiredModifierFound = false;
            ArrayBuilder<ModifierInfo<TypeSymbol>> modifiers = null;

            modifiers = ArrayBuilder<ModifierInfo<TypeSymbol>>.GetInstance();
            var req = signatureReader.GetRequiredCustomModifiers();
            foreach(var v in req)
            {
                TypeSymbol type = GetType1(v);
                bool isAllowed = false;
                switch (allowedRequiredModifierType)
                {
                    case AllowedRequiredModifierType.System_Runtime_InteropServices_InAttribute:
                        isAllowed = IsAcceptedInAttributeModifierType(type);
                        break;
                    case AllowedRequiredModifierType.System_Runtime_CompilerServices_Volatile:
                        isAllowed = IsAcceptedVolatileModifierType(type);
                        break;
                    case AllowedRequiredModifierType.System_Runtime_InteropServices_UnmanagedType:
                        isAllowed = IsAcceptedUnmanagedTypeModifierType(type);
                        break;
                }

                if (isAllowed)
                {
                    requiredModifierFound = true;
                }
                else
                {
                    throw new UnsupportedSignatureContent();
                }
                ModifierInfo<TypeSymbol> modifier = new ModifierInfo<TypeSymbol>(false, type);
                modifiers.Add(modifier);
            }
            var op = signatureReader.GetOptionalCustomModifiers();

            foreach (var v in op)
            {
                TypeSymbol type = GetType1(v);
              

               
                ModifierInfo<TypeSymbol> modifier = new ModifierInfo<TypeSymbol>(true, type);
                modifiers.Add(modifier);
            }


            return modifiers?.ToImmutableAndFree() ?? default(ImmutableArray<ModifierInfo<TypeSymbol>>);
        }


        /// <exception cref="UnsupportedSignatureContent">If the encoded local variable type is invalid.</exception>
        /// <exception cref="BadImageFormatException">An exception from metadata reader.</exception>


        /// <exception cref="UnsupportedSignatureContent">If the encoded local variable type is invalid.</exception>
        /// <exception cref="BadImageFormatException">An exception from metadata reader.</exception>




        /// <summary>
        /// Used to decode signatures of local constants returned by SymReader.
        /// </summary>

        /// <summary>
        /// Returns the local info for all locals indexed by slot.
        /// </summary>

        /// <exception cref="UnsupportedSignatureContent">If the encoded parameter type is invalid.</exception>
        internal void DecodeParameterOrThrow(System.Reflection.ParameterInfo p,/*out*/ ref ParamInfo<TypeSymbol> info)
        {


            info.CustomModifiers = ImmutableArray<ModifierInfo<TypeSymbol>>.Empty;
            //if (typeCode == SignatureTypeCode.ByReference)
            //{
            //    info.IsByRef = true;
            //    info.RefCustomModifiers = info.CustomModifiers;
            //    info.CustomModifiers = DecodeModifiersOrThrow(ref signatureReader, AllowedRequiredModifierType.None, out typeCode, out _);
            //}
            //else if (inAttributeFound)
            //{
            //    // This cannot be placed on CustomModifiers, just RefCustomModifiers
            //    throw new UnsupportedSignatureContent();
            //}
            info.Type = DecodeTypeOrThrow(p.ParameterType, out _);
        }

        // MetaImport::DecodeMethodSignature

        /// <exception cref="BadImageFormatException">An exception from metadata reader.</exception>



        #region Custom Attributes

        /// <summary>
        /// Decodes attribute parameter type from method signature.
        /// </summary>
        /// <exception cref="UnsupportedSignatureContent">If the encoded parameter type is invalid.</exception>
        /// <exception cref="BadImageFormatException">An exception from metadata reader.</exception>
        private void DecodeCustomAttributeParameterTypeOrThrow(Type sigReader, out SerializationTypeCode typeCode, out TypeSymbol type, out SerializationTypeCode elementTypeCode, out TypeSymbol elementType, bool isElementType)
        {


            if (sigReader.GetArrayRank() == 1)
            {
                if (isElementType)
                {
                    // nested arrays not allowed
                    throw new System.Exception();
                }

                SerializationTypeCode unusedElementTypeCode;
                TypeSymbol unusedElementType;
                DecodeCustomAttributeParameterTypeOrThrow(sigReader.GetElementType(), out elementTypeCode, out elementType, out unusedElementTypeCode, out unusedElementType, isElementType: true);
                type = GetSZArrayTypeSymbol(elementType, customModifiers: default(ImmutableArray<ModifierInfo<TypeSymbol>>));
                typeCode = SerializationTypeCode.SZArray;
                return;
            }

            elementTypeCode = SerializationTypeCode.Invalid;
            elementType = null;
            var tc = Type.GetTypeCode(sigReader);
            switch (tc)
            {
                case TypeCode.Object:
                    if (sigReader.DeclaringType.FullName == "System.Object")
                    {
                        type = GetSpecialType(SpecialType.System_Object);
                        typeCode = SerializationTypeCode.TaggedObject;
                        return;
                    }
                    break;
                default:

                    type = GetSpecialType(ToSpecialType(tc));
                    typeCode = (SerializationTypeCode)tc;

                    break;

            }

            //switch (sigReader)
            //{
            //    case typeof(object) a:
            //        type = GetSpecialType(SpecialType.System_Object);
            //        typeCode = SerializationTypeCode.TaggedObject;
            //        return;

            //    case SignatureTypeCode.String:
            //    case SignatureTypeCode.Boolean:
            //    case SignatureTypeCode.Char:
            //    case SignatureTypeCode.SByte:
            //    case SignatureTypeCode.Byte:
            //    case SignatureTypeCode.Int16:
            //    case SignatureTypeCode.UInt16:
            //    case SignatureTypeCode.Int32:
            //    case SignatureTypeCode.UInt32:
            //    case SignatureTypeCode.Int64:
            //    case SignatureTypeCode.UInt64:
            //    case SignatureTypeCode.Single:
            //    case SignatureTypeCode.Double:
            //        type = GetSpecialType(paramTypeCode.ToSpecialType());
            //        typeCode = (SerializationTypeCode)paramTypeCode;
            //        return;

            //    case SignatureTypeCode.TypeHandle:
            //        // The type of the parameter can either be an enum type or System.Type.
            //        bool isNoPiaLocalType;
            //        type = GetSymbolForTypeHandleOrThrow(sigReader.ReadTypeHandle(), out isNoPiaLocalType, allowTypeSpec: true, requireShortForm: true);

            //        var underlyingEnumType = GetEnumUnderlyingType(type);

            //        // Spec: If the parameter kind is an enum -- simply store the value of the enum's underlying integer type.
            //        if ((object)underlyingEnumType != null)
            //        {
            //            Debug.Assert(!isNoPiaLocalType);

            //            // GetEnumUnderlyingType always returns a valid enum underlying type
            //            typeCode = underlyingEnumType.SpecialType.ToSerializationType();
            //            return;
            //        }

            //        if ((object)type == SystemTypeSymbol)
            //        {
            //            Debug.Assert(!isNoPiaLocalType);
            //            typeCode = SerializationTypeCode.Type;
            //            return;
            //        }

            //        break;
            //}

            throw new System.Exception();
        }
        /// <summary>
        /// Decodes attribute argument type from attribute blob (called FieldOrPropType in the spec).
        /// </summary>
        /// <exception cref="UnsupportedSignatureContent">If the encoded argument type is invalid.</exception>
        /// <exception cref="BadImageFormatException">An exception from metadata reader.</exception>
        //private void DecodeCustomAttributeFieldOrPropTypeOrThrow(ref BlobReader argReader, out SerializationTypeCode typeCode, out TypeSymbol type, out SerializationTypeCode elementTypeCode, out TypeSymbol elementType, bool isElementType)
        //{
        //    typeCode = argReader.ReadSerializationTypeCode();

        //    // Spec:
        //    // The FieldOrPropType (typeCode) shall be exactly one of: ELEMENT_TYPE_BOOLEAN,
        //    // ELEMENT_TYPE_CHAR, ELEMENT_TYPE_I1, ELEMENT_TYPE_U1, ELEMENT_TYPE_I2,
        //    // ELEMENT_TYPE_U2, ELEMENT_TYPE_I4, ELEMENT_TYPE_U4, ELEMENT_TYPE_I8,
        //    // ELEMENT_TYPE_U8, ELEMENT_TYPE_R4, ELEMENT_TYPE_R8, ELEMENT_TYPE_STRING.
        //    // 
        //    // A single-dimensional, zero-based array is specified as a single byte 0x1D followed
        //    // by the FieldOrPropType of the element type. (See §II.23.1.16) An enum is
        //    // specified as a single byte 0x55 followed by a SerString.
        //    // 
        //    // tomat: The spec is missing ELEMENT_TYPE_TYPE.

        //    if (typeCode == SerializationTypeCode.SZArray)
        //    {
        //        if (isElementType)
        //        {
        //            // nested array not allowed:
        //            throw new UnsupportedSignatureContent();
        //        }

        //        SerializationTypeCode unusedElementTypeCode;
        //        TypeSymbol unusedElementType;
        //        DecodeCustomAttributeFieldOrPropTypeOrThrow(ref argReader, out elementTypeCode, out elementType, out unusedElementTypeCode, out unusedElementType, isElementType: true);
        //        type = GetSZArrayTypeSymbol(elementType, customModifiers: default(ImmutableArray<ModifierInfo<TypeSymbol>>));
        //        return;
        //    }

        //    elementTypeCode = SerializationTypeCode.Invalid;
        //    elementType = null;

        //    switch (typeCode)
        //    {
        //        case SerializationTypeCode.TaggedObject:
        //            type = GetSpecialType(SpecialType.System_Object);
        //            return;

        //        case SerializationTypeCode.Enum:
        //            string enumTypeName;
        //            if (!PEModule.CrackStringInAttributeValue(out enumTypeName, ref argReader))
        //            {
        //                throw new UnsupportedSignatureContent();
        //            }

        //            type = GetTypeSymbolForSerializedType(enumTypeName);
        //            var underlyingType = GetEnumUnderlyingType(type);
        //            if ((object)underlyingType == null)
        //            {
        //                throw new UnsupportedSignatureContent();
        //            }

        //            // GetEnumUnderlyingType always returns a valid enum underlying type
        //            typeCode = underlyingType.SpecialType.ToSerializationType();
        //            return;

        //        case SerializationTypeCode.Type:
        //            type = SystemTypeSymbol;
        //            return;

        //        case SerializationTypeCode.String:
        //        case SerializationTypeCode.Boolean:
        //        case SerializationTypeCode.Char:
        //        case SerializationTypeCode.SByte:
        //        case SerializationTypeCode.Byte:
        //        case SerializationTypeCode.Int16:
        //        case SerializationTypeCode.UInt16:
        //        case SerializationTypeCode.Int32:
        //        case SerializationTypeCode.UInt32:
        //        case SerializationTypeCode.Int64:
        //        case SerializationTypeCode.UInt64:
        //        case SerializationTypeCode.Single:
        //        case SerializationTypeCode.Double:
        //            type = GetSpecialType(((SignatureTypeCode)typeCode).ToSpecialType());
        //            return;
        //    }

        //    throw new UnsupportedSignatureContent();
        //}

        /// <exception cref="UnsupportedSignatureContent">If the encoded attribute argument is invalid.</exception>
        /// <exception cref="BadImageFormatException">An exception from metadata reader.</exception>
        //private TypedConstant DecodeCustomAttributeFixedArgumentOrThrow(ref BlobReader sigReader, ref BlobReader argReader)
        //{
        //    SerializationTypeCode typeCode, elementTypeCode;
        //    TypeSymbol type, elementType;
        //    DecodeCustomAttributeParameterTypeOrThrow(ref sigReader, out typeCode, out type, out elementTypeCode, out elementType, isElementType: false);

        //    // arrays are allowed only on top-level:
        //    if (typeCode == SerializationTypeCode.SZArray)
        //    {
        //        return DecodeCustomAttributeElementArrayOrThrow(ref argReader, elementTypeCode, elementType, type);
        //    }

        //    return DecodeCustomAttributeElementOrThrow(ref argReader, typeCode, type);
        //}

        /// <exception cref="UnsupportedSignatureContent">If the encoded attribute argument is invalid.</exception>
        /// <exception cref="BadImageFormatException">An exception from metadata reader.</exception>
        //private TypedConstant DecodeCustomAttributeElementOrThrow(ref BlobReader argReader, SerializationTypeCode typeCode, TypeSymbol type)
        //{
        //    if (typeCode == SerializationTypeCode.TaggedObject)
        //    {
        //        // Spec: If the parameter kind is System.Object, the value stored represents the "boxed" instance of that value-type.
        //        SerializationTypeCode elementTypeCode;
        //        TypeSymbol elementType;
        //        DecodeCustomAttributeFieldOrPropTypeOrThrow(ref argReader, out typeCode, out type, out elementTypeCode, out elementType, isElementType: false);

        //        if (typeCode == SerializationTypeCode.SZArray)
        //        {
        //            return DecodeCustomAttributeElementArrayOrThrow(ref argReader, elementTypeCode, elementType, type);
        //        }
        //    }

        //    return DecodeCustomAttributePrimitiveElementOrThrow(ref argReader, typeCode, type);
        //}

        /// <exception cref="UnsupportedSignatureContent">If the encoded attribute argument is invalid.</exception>
        /// <exception cref="BadImageFormatException">An exception from metadata reader.</exception>
        //private TypedConstant DecodeCustomAttributeElementArrayOrThrow(ref BlobReader argReader, SerializationTypeCode elementTypeCode, TypeSymbol elementType, TypeSymbol arrayType)
        //{
        //    int count = argReader.ReadInt32();
        //    TypedConstant[] values;

        //    if (count == -1)
        //    {
        //        values = null;
        //    }
        //    else if (count == 0)
        //    {
        //        values = Array.Empty<TypedConstant>();
        //    }
        //    else
        //    {
        //        values = new TypedConstant[count];
        //        for (int i = 0; i < count; i++)
        //        {
        //            values[i] = DecodeCustomAttributeElementOrThrow(ref argReader, elementTypeCode, elementType);
        //        }
        //    }

        //    return CreateArrayTypedConstant(arrayType, values.AsImmutableOrNull());
        //}

        /// <exception cref="UnsupportedSignatureContent">If the given <paramref name="typeCode"/> is invalid.</exception>
        /// <exception cref="BadImageFormatException">An exception from metadata reader.</exception>
        //private TypedConstant DecodeCustomAttributePrimitiveElementOrThrow(ref BlobReader argReader, SerializationTypeCode typeCode, TypeSymbol type)
        //{
        //    Debug.Assert(type != null);

        //    switch (typeCode)
        //    {
        //        case SerializationTypeCode.Boolean:
        //            return CreateTypedConstant(type, GetPrimitiveOrEnumTypedConstantKind(type), argReader.ReadSByte() != 0);

        //        case SerializationTypeCode.SByte:
        //            return CreateTypedConstant(type, GetPrimitiveOrEnumTypedConstantKind(type), argReader.ReadSByte());

        //        case SerializationTypeCode.Byte:
        //            return CreateTypedConstant(type, GetPrimitiveOrEnumTypedConstantKind(type), argReader.ReadByte());

        //        case SerializationTypeCode.Int16:
        //            return CreateTypedConstant(type, GetPrimitiveOrEnumTypedConstantKind(type), argReader.ReadInt16());

        //        case SerializationTypeCode.UInt16:
        //            return CreateTypedConstant(type, GetPrimitiveOrEnumTypedConstantKind(type), argReader.ReadUInt16());

        //        case SerializationTypeCode.Int32:
        //            return CreateTypedConstant(type, GetPrimitiveOrEnumTypedConstantKind(type), argReader.ReadInt32());

        //        case SerializationTypeCode.UInt32:
        //            return CreateTypedConstant(type, GetPrimitiveOrEnumTypedConstantKind(type), argReader.ReadUInt32());

        //        case SerializationTypeCode.Int64:
        //            return CreateTypedConstant(type, GetPrimitiveOrEnumTypedConstantKind(type), argReader.ReadInt64());

        //        case SerializationTypeCode.UInt64:
        //            return CreateTypedConstant(type, GetPrimitiveOrEnumTypedConstantKind(type), argReader.ReadUInt64());

        //        case SerializationTypeCode.Single:
        //            return CreateTypedConstant(type, GetPrimitiveOrEnumTypedConstantKind(type), argReader.ReadSingle());

        //        case SerializationTypeCode.Double:
        //            return CreateTypedConstant(type, GetPrimitiveOrEnumTypedConstantKind(type), argReader.ReadDouble());

        //        case SerializationTypeCode.Char:
        //            return CreateTypedConstant(type, GetPrimitiveOrEnumTypedConstantKind(type), argReader.ReadChar());

        //        case SerializationTypeCode.String:
        //            string s;
        //            TypedConstantKind kind = PEModule.CrackStringInAttributeValue(out s, ref argReader) ?
        //                TypedConstantKind.Primitive :
        //                TypedConstantKind.Error;

        //            return CreateTypedConstant(type, kind, s);

        //        case SerializationTypeCode.Type:
        //            string typeName;
        //            TypeSymbol serializedType = PEModule.CrackStringInAttributeValue(out typeName, ref argReader) ?
        //                (typeName != null ? GetTypeSymbolForSerializedType(typeName) : null) :
        //                GetUnsupportedMetadataTypeSymbol();

        //            return CreateTypedConstant(type, TypedConstantKind.Type, serializedType);

        //        default:
        //            throw new UnsupportedSignatureContent();
        //    }
        //}

        private static TypedConstantKind GetPrimitiveOrEnumTypedConstantKind(TypeSymbol type)
        {
            return (type.TypeKind == TypeKind.Enum) ? TypedConstantKind.Enum : TypedConstantKind.Primitive;
        }

        /// <exception cref="UnsupportedSignatureContent">If the encoded named argument is invalid.</exception>
        /// <exception cref="BadImageFormatException">An exception from metadata reader.</exception>
        //private KeyValuePair<string, TypedConstant> DecodeCustomAttributeNamedArgumentOrThrow(ref BlobReader argReader)
        //{
        //    // Ecma-335 23.3 - A NamedArg is simply a FixedArg preceded by information to identify which field or
        //    // property it represents. [Note: Recall that the CLI allows fields and properties to have the same name; so
        //    // we require a means to disambiguate such situations. end note] FIELD is the single byte 0x53. PROPERTY is
        //    // the single byte 0x54.

        //    var kind = (CustomAttributeNamedArgumentKind)argReader.ReadCompressedInteger();
        //    if (kind != CustomAttributeNamedArgumentKind.Field && kind != CustomAttributeNamedArgumentKind.Property)
        //    {
        //        throw new UnsupportedSignatureContent();
        //    }

        //    SerializationTypeCode typeCode, elementTypeCode;
        //    TypeSymbol type, elementType;
        //    DecodeCustomAttributeFieldOrPropTypeOrThrow(ref argReader, out typeCode, out type, out elementTypeCode, out elementType, isElementType: false);

        //    string name;
        //    if (!PEModule.CrackStringInAttributeValue(out name, ref argReader))
        //    {
        //        throw new UnsupportedSignatureContent();
        //    }

        //    TypedConstant value = typeCode == SerializationTypeCode.SZArray
        //        ? DecodeCustomAttributeElementArrayOrThrow(ref argReader, elementTypeCode, elementType, type)
        //        : DecodeCustomAttributeElementOrThrow(ref argReader, typeCode, type);

        //    return new KeyValuePair<string, TypedConstant>(name, value);
        //}

        //internal bool IsTargetAttribute(
        //    CustomAttributeHandle customAttribute,
        //    string namespaceName,
        //    string typeName,
        //    bool ignoreCase = false)
        //{
        //    try
        //    {
        //        EntityHandle ctor;

        //        return Module.IsTargetAttribute(
        //            customAttribute,
        //            namespaceName,
        //            typeName,
        //            out ctor,
        //            ignoreCase);
        //    }
        //    catch (BadImageFormatException)
        //    {
        //        return false;
        //    }
        //}

        //internal int GetTargetAttributeSignatureIndex(CustomAttributeHandle customAttribute, AttributeDescription description)
        //{
        //    try
        //    {
        //        return Module.GetTargetAttributeSignatureIndex(customAttribute, description);
        //    }
        //    catch (BadImageFormatException)
        //    {
        //        return -1;
        //    }
        //}

        //internal bool GetCustomAttribute(
        //    CustomAttributeHandle handle,
        //    out TypedConstant[] positionalArgs,
        //    out KeyValuePair<string, TypedConstant>[] namedArgs)
        //{
        //    try
        //    {
        //        positionalArgs = Array.Empty<TypedConstant>();
        //        namedArgs = Array.Empty<KeyValuePair<String, TypedConstant>>();

        //        // We could call decoder.GetSignature and use that to decode the arguments. However, materializing the
        //        // constructor signature is more work. We try to decode the arguments directly from the metadata bytes.
        //        EntityHandle attributeType;
        //        EntityHandle ctor;

        //        if (Module.GetTypeAndConstructor(handle, out attributeType, out ctor))
        //        {
        //            BlobReader argsReader = Module.GetMemoryReaderOrThrow(Module.GetCustomAttributeValueOrThrow(handle));
        //            BlobReader sigReader = Module.GetMemoryReaderOrThrow(Module.GetMethodSignatureOrThrow(ctor));

        //            uint prolog = argsReader.ReadUInt16();
        //            if (prolog != 1)
        //            {
        //                return false;
        //            }

        //            // Read the signature header.
        //            SignatureHeader signatureHeader = sigReader.ReadSignatureHeader();

        //            // Get the type parameter count.
        //            if (signatureHeader.IsGeneric && sigReader.ReadCompressedInteger() != 0)
        //            {
        //                return false;
        //            }

        //            // Get the parameter count
        //            int paramCount = sigReader.ReadCompressedInteger();

        //            // Get the type return type.
        //            var returnTypeCode = sigReader.ReadSignatureTypeCode();
        //            if (returnTypeCode != SignatureTypeCode.Void)
        //            {
        //                return false;
        //            }

        //            if (paramCount > 0)
        //            {
        //                positionalArgs = new TypedConstant[paramCount];

        //                for (int i = 0; i < positionalArgs.Length; i++)
        //                {
        //                    positionalArgs[i] = DecodeCustomAttributeFixedArgumentOrThrow(ref sigReader, ref argsReader);
        //                }
        //            }

        //            short namedParamCount = argsReader.ReadInt16();

        //            if (namedParamCount > 0)
        //            {
        //                namedArgs = new KeyValuePair<string, TypedConstant>[namedParamCount];

        //                for (int i = 0; i < namedArgs.Length; i++)
        //                {
        //                    namedArgs[i] = DecodeCustomAttributeNamedArgumentOrThrow(ref argsReader);
        //                }
        //            }

        //            return true;
        //        }
        //    }
        //    catch (Exception e) when (e is UnsupportedSignatureContent || e is BadImageFormatException)
        //    {
        //        positionalArgs = Array.Empty<TypedConstant>();
        //        namedArgs = Array.Empty<KeyValuePair<String, TypedConstant>>();
        //    }

        //    return false;
        //}

        //internal bool GetCustomAttribute(CustomAttributeHandle handle, out TypeSymbol attributeClass, out MethodSymbol attributeCtor)
        //{
        //    EntityHandle attributeType;
        //    EntityHandle ctor;

        //    try
        //    {
        //        if (!Module.GetTypeAndConstructor(handle, out attributeType, out ctor))
        //        {
        //            attributeClass = null;
        //            attributeCtor = null;
        //            return false;
        //        }
        //    }
        //    catch (BadImageFormatException)
        //    {
        //        attributeClass = null;
        //        attributeCtor = null;
        //        return false;
        //    }

        //    attributeClass = GetTypeOfToken(attributeType);
        //    attributeCtor = GetMethodSymbolForMethodDefOrMemberRef(ctor, attributeClass);
        //    return true;
        //}

        //internal bool GetCustomAttributeWellKnownType(CustomAttributeHandle handle, out WellKnownType wellKnownAttribute)
        //{
        //    wellKnownAttribute = WellKnownType.Unknown;

        //    try
        //    {
        //        EntityHandle attributeType;
        //        EntityHandle ctor;

        //        if (!Module.GetTypeAndConstructor(handle, out attributeType, out ctor))
        //        {
        //            return false;
        //        }

        //        StringHandle namespaceHandle;
        //        StringHandle nameHandle;
        //        if (!Module.GetAttributeNamespaceAndName(attributeType, out namespaceHandle, out nameHandle))
        //        {
        //            return false;
        //        }

        //        string fullName = Module.GetFullNameOrThrow(namespaceHandle, nameHandle);
        //        wellKnownAttribute = WellKnownTypes.GetTypeFromMetadataName(fullName);
        //        return true;
        //    }
        //    catch (BadImageFormatException)
        //    {
        //        return false;
        //    }
        //}

        //#endregion

        ///// <exception cref="BadImageFormatException">An exception from metadata reader.</exception>
        //private TypeSymbol[] DecodeMethodSpecTypeArgumentsOrThrow(BlobHandle signature)
        //{
        //    SignatureHeader signatureHeader;
        //    var signatureReader = DecodeSignatureHeaderOrThrow(signature, out signatureHeader);
        //    if (signatureHeader.Kind != SignatureKind.MethodSpecification)
        //    {
        //        throw new BadImageFormatException();
        //    }

        //    int argumentCount = signatureReader.ReadCompressedInteger();
        //    if (argumentCount == 0)
        //    {
        //        throw new BadImageFormatException();
        //    }

        //    var result = new TypeSymbol[argumentCount];
        //    for (int i = 0; i < result.Length; i++)
        //    {
        //        bool refersToNoPiaLocalType;
        //        result[i] = DecodeTypeOrThrow(ref signatureReader, out refersToNoPiaLocalType);
        //    }

        //    return result;
        //}

        ///// <exception cref="BadImageFormatException">An exception from metadata reader.</exception>
        //internal BlobReader DecodeSignatureHeaderOrThrow(BlobHandle signature, out SignatureHeader signatureHeader)
        //{
        //    return DecodeSignatureHeaderOrThrow(Module, signature, out signatureHeader);
        //}

        ///// <exception cref="BadImageFormatException">An exception from metadata reader.</exception>
        //internal static BlobReader DecodeSignatureHeaderOrThrow(PEModule module, BlobHandle signature, out SignatureHeader signatureHeader)
        //{
        //    BlobReader reader = module.GetMemoryReaderOrThrow(signature);
        //    signatureHeader = reader.ReadSignatureHeader();
        //    return reader;
        //}

        ///// <exception cref="BadImageFormatException">An exception from metadata reader.</exception>
        //protected ParamInfo<TypeSymbol>[] DecodeSignatureParametersOrThrow(ref BlobReader signatureReader, SignatureHeader signatureHeader, out int typeParameterCount)
        //{
        //    int paramCount;
        //    GetSignatureCountsOrThrow(ref signatureReader, signatureHeader, out paramCount, out typeParameterCount);

        //    ParamInfo<TypeSymbol>[] paramInfo = new ParamInfo<TypeSymbol>[paramCount + 1];

        //    uint paramIndex = 0;

        //    try
        //    {
        //        // get the return type
        //        DecodeParameterOrThrow(ref signatureReader, ref paramInfo[0]);

        //        // Get all of the parameters.
        //        for (paramIndex = 1; paramIndex <= paramCount; paramIndex++)
        //        {
        //            // Figure out the type.
        //            DecodeParameterOrThrow(ref signatureReader, ref paramInfo[paramIndex]);
        //        }

        //        if (signatureReader.RemainingBytes > 0)
        //        {
        //            throw new UnsupportedSignatureContent();
        //        }
        //    }
        //    catch (Exception e) when (e is UnsupportedSignatureContent || e is BadImageFormatException)
        //    {
        //        for (; paramIndex <= paramCount; paramIndex++)
        //        {
        //            paramInfo[paramIndex].Type = GetUnsupportedMetadataTypeSymbol(e as BadImageFormatException);
        //        }
        //    }

        //    return paramInfo;
        //}

        ///// <exception cref="BadImageFormatException">An exception from metadata reader.</exception>
        //private static void GetSignatureCountsOrThrow(ref BlobReader signatureReader, SignatureHeader signatureHeader, out int parameterCount, out int typeParameterCount)
        //{
        //    // Get the type parameter count.
        //    typeParameterCount = signatureHeader.IsGeneric ? signatureReader.ReadCompressedInteger() : 0;

        //    // Get the parameter count
        //    parameterCount = signatureReader.ReadCompressedInteger();
        //}

        //internal TypeSymbol DecodeFieldSignature(FieldDefinitionHandle fieldHandle, out bool isVolatile, out ImmutableArray<ModifierInfo<TypeSymbol>> customModifiers)
        //{
        //    try
        //    {
        //        BlobHandle signature = Module.GetFieldSignatureOrThrow(fieldHandle);

        //        SignatureHeader signatureHeader;
        //        BlobReader signatureReader = DecodeSignatureHeaderOrThrow(signature, out signatureHeader);

        //        if (signatureHeader.Kind != SignatureKind.Field)
        //        {
        //            isVolatile = false;
        //            customModifiers = default(ImmutableArray<ModifierInfo<TypeSymbol>>);
        //            return GetUnsupportedMetadataTypeSymbol(); // unsupported signature content
        //        }

        //        return DecodeFieldSignature(ref signatureReader, out isVolatile, out customModifiers);
        //    }
        //    catch (BadImageFormatException mrEx)
        //    {
        //        isVolatile = false;
        //        customModifiers = default(ImmutableArray<ModifierInfo<TypeSymbol>>);
        //        return GetUnsupportedMetadataTypeSymbol(mrEx);
        //    }
        //}

        //// MetaImport::DecodeFieldSignature
        //protected TypeSymbol DecodeFieldSignature(ref BlobReader signatureReader, out bool isVolatile, out ImmutableArray<ModifierInfo<TypeSymbol>> customModifiers)
        //{
        //    isVolatile = false;
        //    customModifiers = default(ImmutableArray<ModifierInfo<TypeSymbol>>);

        //    try
        //    {
             
        //        SignatureTypeCode typeCode;
        //        customModifiers = DecodeModifiersOrThrow(
        //            ref signatureReader,
        //            AllowedRequiredModifierType.System_Runtime_CompilerServices_Volatile,
        //            out typeCode,
        //            out isVolatile);

        //        return DecodeTypeOrThrow(ref signatureReader, typeCode, out _);
        //    }
        //    catch (UnsupportedSignatureContent)
        //    {
        //        return GetUnsupportedMetadataTypeSymbol(); // unsupported signature content
        //    }
        //    catch (BadImageFormatException mrEx)
        //    {
        //        return GetUnsupportedMetadataTypeSymbol(mrEx);
        //    }
        //}

        ///// <summary>
        ///// Find the methods that a given method explicitly overrides.
        ///// </summary>
        ///// <remarks>
        ///// Methods may be on class or interfaces.
        ///// Containing classes/interfaces will be supertypes of the implementing type.
        ///// </remarks>
        ///// <param name="implementingTypeDef">TypeDef handle of the implementing type.</param>
        ///// <param name="implementingMethodDef">MethodDef handle of the implementing method.</param>
        ///// <param name="implementingTypeSymbol">The type symbol for the implementing type.</param>
        ///// <returns>Array of implemented methods.</returns>
        //internal ImmutableArray<MethodSymbol> GetExplicitlyOverriddenMethods(TypeDefinitionHandle implementingTypeDef, MethodDefinitionHandle implementingMethodDef, TypeSymbol implementingTypeSymbol)
        //{
        //    ArrayBuilder<MethodSymbol> resultBuilder = ArrayBuilder<MethodSymbol>.GetInstance();

        //    try
        //    {
        //        foreach (var methodImpl in Module.GetMethodImplementationsOrThrow(implementingTypeDef))
        //        {
        //            EntityHandle methodDebugHandle;
        //            EntityHandle implementedMethodHandle;
        //            Module.GetMethodImplPropsOrThrow(methodImpl, out methodDebugHandle, out implementedMethodHandle);

        //            // Though it is rare in practice, the spec allows the MethodImpl table to represent
        //            // methods defined in the current module as MemberRefs rather than MethodDefs.
        //            if (methodDebugHandle.Kind == HandleKind.MemberReference)
        //            {
        //                MethodSymbol methodBodySymbol = GetMethodSymbolForMemberRef((MemberReferenceHandle)methodDebugHandle, implementingTypeSymbol);
        //                if (methodBodySymbol != null)
        //                {
        //                    // Note: this might have a nil row ID, but that won't cause a problem
        //                    // since it will simply fail to be equal to the implementingMethodToken.
        //                    methodDebugHandle = GetMethodHandle(methodBodySymbol);
        //                }
        //            }

        //            if (methodDebugHandle == implementingMethodDef)
        //            {
        //                if (!implementedMethodHandle.IsNil)
        //                {
        //                    HandleKind implementedMethodTokenType = implementedMethodHandle.Kind;

        //                    MethodSymbol methodSymbol = null;

        //                    if (implementedMethodTokenType == HandleKind.MethodDefinition)
        //                    {
        //                        methodSymbol = FindMethodSymbolInSuperType(implementingTypeDef, (MethodDefinitionHandle)implementedMethodHandle);
        //                    }
        //                    else if (implementedMethodTokenType == HandleKind.MemberReference)
        //                    {
        //                        methodSymbol = GetMethodSymbolForMemberRef((MemberReferenceHandle)implementedMethodHandle, implementingTypeSymbol);
        //                    }

        //                    if (methodSymbol != null)
        //                    {
        //                        resultBuilder.Add(methodSymbol);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (BadImageFormatException)
        //    { }

        //    return resultBuilder.ToImmutableAndFree();
        //}

        ///// <summary>
        ///// Search for the <typeparamref name="MethodSymbol"/> corresponding to the given MethodDef token. Search amongst
        ///// the supertypes (classes and interfaces) of a designated type.
        ///// </summary>
        ///// <remarks>
        ///// Generally, the type will be a type that explicitly implements an interface and the method will be the
        ///// implemented method (i.e. on the interface).
        ///// </remarks>
        ///// <param name="searchTypeDef">TypeDef token of the type from which the search should begin.</param>
        ///// <param name="targetMethodDef">MethodDef token of the target method.</param>
        ///// <returns>Corresponding <typeparamref name="MethodSymbol"/> or null, if none is found.</returns>
        //private MethodSymbol FindMethodSymbolInSuperType(TypeDefinitionHandle searchTypeDef, MethodDefinitionHandle targetMethodDef)
        //{
        //    try
        //    {
        //        // We're using queues (i.e. BFS), rather than stacks (i.e. DFS), because we expect the common case
        //        // to be implementing a method on an immediate supertype, rather than a remote ancestor.
        //        // We're using more than one queue for two reasons: 1) some of our TypeDef tokens come directly from the
        //        // metadata tables and we'd prefer not to manipulate the corresponding symbol objects; 2) we bump TypeDefs
        //        // to the front of the search order (i.e. ahead of symbols) because a MethodDef can correspond to a TypeDef
        //        // but not to a type ref (i.e. symbol).
        //        Queue<TypeDefinitionHandle> typeDefsToSearch = new Queue<TypeDefinitionHandle>();
        //        Queue<TypeSymbol> typeSymbolsToSearch = new Queue<TypeSymbol>();

        //        // A method def represents a method defined in this module, so we can
        //        // just search the method defs of this module.
        //        EnqueueTypeDefInterfacesAndBaseTypeOrThrow(typeDefsToSearch, typeSymbolsToSearch, searchTypeDef);

        //        //catch both cycles and duplicate interfaces
        //        HashSet<TypeDefinitionHandle> visitedTypeDefTokens = new HashSet<TypeDefinitionHandle>();
        //        HashSet<TypeSymbol> visitedTypeSymbols = new HashSet<TypeSymbol>();

        //        bool hasMoreTypeDefs;
        //        while ((hasMoreTypeDefs = (typeDefsToSearch.Count > 0)) || typeSymbolsToSearch.Count > 0)
        //        {
        //            if (hasMoreTypeDefs)
        //            {
        //                TypeDefinitionHandle typeDef = typeDefsToSearch.Dequeue();
        //                Debug.Assert(!typeDef.IsNil);

        //                if (!visitedTypeDefTokens.Contains(typeDef))
        //                {
        //                    visitedTypeDefTokens.Add(typeDef);

        //                    foreach (MethodDefinitionHandle methodDef in Module.GetMethodsOfTypeOrThrow(typeDef))
        //                    {
        //                        if (methodDef == targetMethodDef)
        //                        {
        //                            TypeSymbol typeSymbol = this.GetTypeOfToken(typeDef);
        //                            return FindMethodSymbolInType(typeSymbol, targetMethodDef);
        //                        }
        //                    }

        //                    EnqueueTypeDefInterfacesAndBaseTypeOrThrow(typeDefsToSearch, typeSymbolsToSearch, typeDef);
        //                }
        //            }
        //            else //has more type symbols
        //            {
        //                TypeSymbol typeSymbol = typeSymbolsToSearch.Dequeue();
        //                Debug.Assert(typeSymbol != null);

        //                if (!visitedTypeSymbols.Contains(typeSymbol))
        //                {
        //                    visitedTypeSymbols.Add(typeSymbol);

        //                    //we're looking for a method def but we're currently on a type *ref*, so just enqueue supertypes

        //                    EnqueueTypeSymbolInterfacesAndBaseTypes(typeDefsToSearch, typeSymbolsToSearch, typeSymbol);
        //                }
        //            }
        //        }
        //    }
        //    catch (BadImageFormatException)
        //    { }

        //    return null;
        //}

        ///// <summary>
        ///// Enqueue the interfaces implemented and the type extended by a given TypeDef.
        ///// </summary>
        ///// <param name="typeDefsToSearch">Queue of TypeDefs to search.</param>
        ///// <param name="typeSymbolsToSearch">Queue of TypeSymbols (representing typeRefs to search).</param>
        ///// <param name="searchTypeDef">Handle of the TypeDef for which we want to enqueue supertypes.</param>
        ///// <exception cref="BadImageFormatException">An exception from metadata reader.</exception>
        //private void EnqueueTypeDefInterfacesAndBaseTypeOrThrow(Queue<TypeDefinitionHandle> typeDefsToSearch, Queue<TypeSymbol> typeSymbolsToSearch, TypeDefinitionHandle searchTypeDef)
        //{
        //    foreach (var interfaceImplHandle in Module.GetInterfaceImplementationsOrThrow(searchTypeDef))
        //    {
        //        var interfaceImpl = Module.MetadataReader.GetInterfaceImplementation(interfaceImplHandle);
        //        EnqueueTypeToken(typeDefsToSearch, typeSymbolsToSearch, interfaceImpl.Interface);
        //    }

        //    EnqueueTypeToken(typeDefsToSearch, typeSymbolsToSearch, Module.GetBaseTypeOfTypeOrThrow(searchTypeDef));
        //}

        ///// <summary>
        ///// Helper method for enqueuing a type token in the right queue.
        ///// Def -> typeDefsToSearch
        ///// Ref -> typeSymbolsToSearch
        ///// null -> neither
        ///// </summary>
        //private void EnqueueTypeToken(Queue<TypeDefinitionHandle> typeDefsToSearch, Queue<TypeSymbol> typeSymbolsToSearch, EntityHandle typeToken)
        //{
        //    if (!typeToken.IsNil)
        //    {
        //        if (typeToken.Kind == HandleKind.TypeDefinition)
        //        {
        //            typeDefsToSearch.Enqueue((TypeDefinitionHandle)typeToken);
        //        }
        //        else
        //        {
        //            EnqueueTypeSymbol(typeDefsToSearch, typeSymbolsToSearch, this.GetTypeOfToken(typeToken));
        //        }
        //    }
        //}

        /// <summary>
        /// Enqueue the interfaces implemented and the type extended by a given TypeDef.
        /// </summary>
        /// <param name="typeDefsToSearch">Queue of TypeDefs to search.</param>
        /// <param name="typeSymbolsToSearch">Queue of TypeSymbols (representing typeRefs to search).</param>
        /// <param name="typeSymbol">Symbol for which we want to enqueue supertypes.</param>
        protected abstract void EnqueueTypeSymbolInterfacesAndBaseTypes(Queue<Type> typeDefsToSearch, Queue<TypeSymbol> typeSymbolsToSearch, TypeSymbol typeSymbol);

        /// <summary>
        /// Enqueue the given type as either a def or a ref.
        /// </summary>
        /// <param name="typeDefsToSearch">Queue of TypeDefs to search.</param>
        /// <param name="typeSymbolsToSearch">Queue of TypeSymbols (representing typeRefs to search).</param>
        /// <param name="typeSymbol">Symbol to enqueue.</param>
        protected abstract void EnqueueTypeSymbol(Queue<Type> typeDefsToSearch, Queue<TypeSymbol> typeSymbolsToSearch, TypeSymbol typeSymbol);

        /// <summary>
        /// Search the members of a TypeSymbol to find the one that matches a given MethodDef token.
        /// </summary>
        /// <param name="type">Type to search for method.</param>
        /// <param name="methodDef">MethodDef handle of the method to find.</param>
        /// <returns>The corresponding MethodSymbol or null.</returns>
        protected abstract MethodSymbol FindMethodSymbolInType(TypeSymbol type, MethodBase methodDef);

        /// <summary>
        /// Search the members of a TypeSymbol to find the one that matches a given FieldDef token.
        /// </summary>
        /// <param name="type">Type to search for field.</param>
        /// <param name="fieldDef">FieldDef handle of the field to find.</param>
        /// <returns>The corresponding FieldSymbol or null.</returns>
        protected abstract FieldSymbol FindFieldSymbolInType(TypeSymbol type, FieldInfo fieldDef);

        /// <summary>
        /// Given a MemberRef token for a method, we can find a corresponding MethodSymbol by
        /// searching for the name and signature.
        /// </summary>
        /// <param name="memberRef">A MemberRef token for a method.</param>
        /// <param name="implementingTypeSymbol">Scope the search to supertypes of the implementing type.</param>
        /// <param name="methodsOnly">True to only return method symbols, null if the token resolves to a field.</param>
        /// <returns>The corresponding MethodSymbol or null.</returns>
        internal abstract Symbol GetSymbolForMemberRef(MemberInfo memberRef, TypeSymbol implementingTypeSymbol = null, bool methodsOnly = false);

        internal MethodSymbol GetMethodSymbolForMemberRef(MemberInfo methodRef, TypeSymbol implementingTypeSymbol)
        {
            return (MethodSymbol)GetSymbolForMemberRef(methodRef, implementingTypeSymbol, methodsOnly: true);
        }

        internal FieldSymbol GetFieldSymbolForMemberRef(MemberInfo methodRef, TypeSymbol implementingTypeSymbol)
        {
            return (FieldSymbol)GetSymbolForMemberRef(methodRef, implementingTypeSymbol, methodsOnly: true);
        }

        protected override bool IsContainingAssembly(AssemblyIdentity identity)
        {
            return _containingAssemblyIdentity != null && _containingAssemblyIdentity.Equals(identity);
        }

        /// <summary>
        /// Given a method symbol, return the MethodDef token, if it is defined in
        /// this module, or a nil token, otherwise.
        /// </summary>
        /// <param name="method">The method symbol for which to return a MethodDef token.</param>
        /// <returns>A MethodDef token or nil.</returns>
        protected abstract MethodBase GetMethodHandle(MethodSymbol method);

        protected abstract ConcurrentDictionary<Type, TypeSymbol> GetTypeHandleToTypeMap();
        protected abstract ConcurrentDictionary<Type, TypeSymbol> GetTypeRefHandleToTypeMap();

        protected abstract TypeSymbol SubstituteNoPiaLocalType(Type typeDef, ref MetadataTypeName name, string interfaceGuid, string scope, string identifier);

        protected abstract TypeSymbol LookupTopLevelTypeDefSymbol(string moduleName, ref MetadataTypeName emittedName, out bool isNoPiaLocalType);

        protected abstract TypeSymbol GetGenericTypeParamSymbol(int position);
        protected abstract TypeSymbol GetGenericMethodTypeParamSymbol(int position);

        private static TypedConstant CreateArrayTypedConstant(TypeSymbol type, ImmutableArray<TypedConstant> array)
        {
            if (type.TypeKind == TypeKind.Error)
            {
                return new TypedConstant(type, TypedConstantKind.Error, null);
            }

            Debug.Assert(type.TypeKind == TypeKind.Array);
            return new TypedConstant(type, array);
        }

        private static TypedConstant CreateTypedConstant(TypeSymbol type, TypedConstantKind kind, object value)
        {
            if (type.TypeKind == TypeKind.Error)
            {
                return new TypedConstant(type, TypedConstantKind.Error, null);
            }

            return new TypedConstant(type, kind, value);
        }

        private static TypedConstant CreateTypedConstant(TypeSymbol type, TypedConstantKind kind, bool value)
        {
            return CreateTypedConstant(type, kind, Boxes.Box(value));
        }

        /// <summary>
        /// Returns a symbol that given token resolves to or null of the token represents an entity that isn't represented by a symbol,
        /// such as vararg MemberRef.
        /// </summary>

        /// <summary>
        /// Given a MemberRef token, return the TypeSymbol for its Class field.
        /// </summary>
        internal TypeSymbol GetMemberRefTypeSymbol(MemberInfo memberRef)
        {
            try
            {
                var container = memberRef.DeclaringType;
                return this.GetTypeOfToken(container);
            }
            catch (BadImageFormatException)
            {
                return null;
            }
        }

        //internal MethodSymbol GetMethodSymbolForMethodDefOrMemberRef(EntityHandle memberToken, TypeSymbol container)
        //{
        //    HandleKind type = memberToken.Kind;
        //    Debug.Assert(type == HandleKind.MethodDefinition || type == HandleKind.MemberReference);

        //    return type == HandleKind.MethodDefinition
        //        ? FindMethodSymbolInType(container, (MethodDefinitionHandle)memberToken)
        //        : GetMethodSymbolForMemberRef((MemberReferenceHandle)memberToken, container);
        //}

        //internal FieldSymbol GetFieldSymbolForFieldDefOrMemberRef(EntityHandle memberToken, TypeSymbol container)
        //{
        //    HandleKind type = memberToken.Kind;
        //    Debug.Assert(type == HandleKind.FieldDefinition ||
        //                    type == HandleKind.MemberReference);

        //    return type == HandleKind.FieldDefinition
        //        ? FindFieldSymbolInType(container,(FieldInfo) memberToken)
        //        : GetFieldSymbolForMemberRef((MemberReferenceHandle)memberToken, container);
        //}

        /// <summary>
        /// Checks whether signatures match where the signatures are either from a property
        /// and an accessor or two accessors. When comparing a property or getter to setter, the
        /// setter signature must be the second argument and 'comparingToSetter' must be true.
        /// </summary>
        /// <param name="signature1">
        /// Signature of the property containing the accessor, or the getter (type, then parameters).
        /// </param>
        /// <param name="signature2">
        /// Signature of the accessor when comparing property and accessor,
        /// or the setter when comparing getter and setter (return type and then parameters).
        /// </param>
        /// <param name="comparingToSetter">
        /// True when comparing a property or getter to a setter, false otherwise.
        /// </param>
        /// <param name="compareParamByRef">
        /// True if differences in IsByRef for parameters should be treated as significant.
        /// </param>
        /// <param name="compareReturnType">
        /// True if differences in return type (or value parameter for setter) should be treated as significant.
        /// </param>
        /// <returns>True if the accessor signature is appropriate for the containing property.</returns>
        internal bool DoPropertySignaturesMatch(ParamInfo<TypeSymbol>[] signature1, ParamInfo<TypeSymbol>[] signature2, bool comparingToSetter, bool compareParamByRef, bool compareReturnType)
        {
            int additionalParamCount = (comparingToSetter ? 1 : 0);

            // Check the number of parameters.
            if ((signature2.Length - additionalParamCount) != signature1.Length)
            {
                return false;
            }

            // Check the setter has a void type.
            if (comparingToSetter &&
                (GetPrimitiveTypeCode(signature2[0].Type) != Cci.PrimitiveTypeCode.Void))
            {
                return false;
            }

            // Check the type of each parameter.
            for (int paramIndex1 = compareReturnType ? 0 : 1; paramIndex1 < signature1.Length; paramIndex1++)
            {
                int paramIndex2 =
                    ((paramIndex1 == 0) && comparingToSetter) ?
                    signature1.Length :
                    paramIndex1;
                var param1 = signature1[paramIndex1];
                var param2 = signature2[paramIndex2];
                if (compareParamByRef && (param2.IsByRef != param1.IsByRef))
                {
                    return false;
                }
                if (!param2.Type.Equals(param1.Type))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Check whether an event accessor has an appropriate signature.
        /// </summary>
        /// <param name="eventType">Type of the event containing the accessor.</param>
        /// <param name="methodParams">Signature of the accessor (return type and then parameters).</param>
        /// <returns>True if the accessor signature is appropriate for the containing event.</returns>
        internal bool DoesSignatureMatchEvent(TypeSymbol eventType, ParamInfo<TypeSymbol>[] methodParams)
        {
            // Check the number of parameters.
            if (methodParams.Length != 2)
            {
                return false;
            }

            // Check the accessor has a void type.
            if (GetPrimitiveTypeCode(methodParams[0].Type) != Cci.PrimitiveTypeCode.Void)
            {
                return false;
            }

            var methodParam = methodParams[1];
            return !methodParam.IsByRef && methodParam.Type.Equals(eventType);
        }

        private enum AllowedRequiredModifierType
        {
            None,
            System_Runtime_CompilerServices_Volatile,
            System_Runtime_InteropServices_InAttribute,
            System_Runtime_InteropServices_UnmanagedType,
        }
    }
}
#endregion