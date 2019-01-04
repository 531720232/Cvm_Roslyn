//// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

//using System;
//using System.Collections.Generic;
//using System.Collections.Immutable;
//using System.Diagnostics;
//using System.IO;
//using System.Linq;
//using System.Reflection.Metadata;
//using System.Reflection.Metadata.Ecma335;
//using Microsoft.CodeAnalysis.CodeGen;
//using Microsoft.CodeAnalysis.Symbols;
//using Roslyn.Utilities;

//namespace Microsoft.CodeAnalysis.Emit
//{
//    internal abstract class DefinitionMap
//    {
//        protected struct MappedMethod
//        {
//            public MappedMethod(IMethodSymbolInternal previousMethodOpt, Func<SyntaxNode, SyntaxNode> syntaxMap)
//            {
//                this.PreviousMethod = previousMethodOpt;
//                this.SyntaxMap = syntaxMap;
//            }

//            public readonly IMethodSymbolInternal PreviousMethod;
//            public readonly Func<SyntaxNode, SyntaxNode> SyntaxMap;
//        }

//        protected readonly PEModule module;
//        protected readonly IDictionary<IMethodSymbol, MappedMethod> mappedMethods;

//        protected DefinitionMap(PEModule module, IEnumerable<SemanticEdit> edits)
//        {
//            Debug.Assert(module != null);
//            Debug.Assert(edits != null);

//            this.module = module;
//            this.mappedMethods = GetMappedMethods(edits);
//        }

//        private static IDictionary<IMethodSymbol, MappedMethod> GetMappedMethods(IEnumerable<SemanticEdit> edits)
//        {
//            var mappedMethods = new Dictionary<IMethodSymbol, MappedMethod>();
//            foreach (var edit in edits)
//            {
//                // We should always "preserve locals" of iterator and async methods since the state machine 
//                // might be active without MoveNext method being on stack. We don't enforce this requirement here,
//                // since a method may be incorrectly marked by Iterator/AsyncStateMachine attribute by the user, 
//                // in which case we can't reliably figure out that it's an error in semantic edit set. 

//                // We should also "preserve locals" of any updated method containing lambdas. The goal is to 
//                // treat lambdas the same as method declarations. Lambdas declared in a method body that escape 
//                // the method (are assigned to a field, added to an event, e.g.) might be invoked after the method 
//                // is updated and when it no longer contains active statements. If we didn't map the lambdas of 
//                // the updated body to the original lambdas we would run the out-of-date lambda bodies, 
//                // which would not happen if the lambdas were named methods.

//                // TODO (bug https://github.com/dotnet/roslyn/issues/2504)
//                // Note that in some cases an Insert might also need to map syntax. For example, a new constructor added 
//                // to a class that has field/property initializers with lambdas. These lambdas get "copied" into the constructor
//                // (assuming it doesn't have "this" constructor initializer) and thus their generated names need to be preserved. 

//                if (edit.Kind == SemanticEditKind.Update && edit.PreserveLocalVariables)
//                {
//                    var method = edit.NewSymbol as IMethodSymbol;
//                    if (method != null)
//                    {
//                        mappedMethods.Add(method, new MappedMethod((IMethodSymbolInternal)edit.OldSymbol, edit.SyntaxMap));
//                    }
//                }
//            }

//            return mappedMethods;
//        }

//        internal abstract Cci.IDefinition MapDefinition(Cci.IDefinition definition);

//        internal bool DefinitionExists(Cci.IDefinition definition)
//        {
//            return MapDefinition(definition) != null;
//        }

//        internal abstract bool TryGetTypeHandle(Cci.ITypeDefinition def, out TypeDefinitionHandle handle);
//        internal abstract bool TryGetEventHandle(Cci.IEventDefinition def, out EventDefinitionHandle handle);
//        internal abstract bool TryGetFieldHandle(Cci.IFieldDefinition def, out FieldDefinitionHandle handle);
//        internal abstract bool TryGetMethodHandle(Cci.IMethodDefinition def, out MethodDefinitionHandle handle);
//        internal abstract bool TryGetPropertyHandle(Cci.IPropertyDefinition def, out PropertyDefinitionHandle handle);
//        internal abstract CommonMessageProvider MessageProvider { get; }
//    }

//    internal abstract class DefinitionMap<TSymbolMatcher> : DefinitionMap
//        where TSymbolMatcher : SymbolMatcher
//    {
//        protected readonly TSymbolMatcher mapToMetadata;
//        protected readonly TSymbolMatcher mapToPrevious;

//        protected DefinitionMap(PEModule module, IEnumerable<SemanticEdit> edits, TSymbolMatcher mapToMetadata, TSymbolMatcher mapToPrevious)
//            : base(module, edits)
//        {
//            Debug.Assert(mapToMetadata != null);

//            this.mapToMetadata = mapToMetadata;
//            this.mapToPrevious = mapToPrevious ?? mapToMetadata;
//        }

//        internal sealed override Cci.IDefinition MapDefinition(Cci.IDefinition definition)
//        {
//            return this.mapToPrevious.MapDefinition(definition) ??
//                   (this.mapToMetadata != this.mapToPrevious ? this.mapToMetadata.MapDefinition(definition) : null);
//        }

//        private bool TryGetMethodHandle(object baseline, Cci.IMethodDefinition def, out MethodDefinitionHandle handle)
//        {
//            if (this.TryGetMethodHandle(def, out handle))
//            {
//                return true;
//            }

//            //def = (Cci.IMethodDefinition)this.mapToPrevious.MapDefinition(def);
//            //if (def != null)
//            //{
//            //    int methodIndex;
//            //    if (baseline.MethodsAdded.TryGetValue(def, out methodIndex))
//            //    {
//            //        handle = MetadataTokens.MethodDefinitionHandle(methodIndex);
//            //        return true;
//            //    }
//            //}

//            handle = default(MethodDefinitionHandle);
//            return false;
//        }

//        protected static IDictionary<SyntaxNode, int> CreateDeclaratorToSyntaxOrdinalMap(ImmutableArray<SyntaxNode> declarators)
//        {
//            var declaratorToIndex = new Dictionary<SyntaxNode, int>();
//            for (int i = 0; i < declarators.Length; i++)
//            {
//                declaratorToIndex.Add(declarators[i], i);
//            }

//            return declaratorToIndex;
//        }

//        protected abstract void GetStateMachineFieldMapFromMetadata(
//            ITypeSymbol stateMachineType,
//            ImmutableArray<LocalSlotDebugInfo> localSlotDebugInfo,
//            out IDictionary<EncHoistedLocalInfo, int> hoistedLocalMap,
//            out IDictionary<Cci.ITypeReference, int> awaiterMap,
//            out int awaiterSlotCount);

//        protected abstract ImmutableArray<EncLocalInfo> GetLocalSlotMapFromMetadata(StandaloneSignatureHandle handle, EditAndContinueMethodDebugInformation debugInfo);
//        protected abstract ITypeSymbol TryGetStateMachineType(EntityHandle methodHandle);


//        protected abstract LambdaSyntaxFacts GetLambdaSyntaxFacts();

//        private void ReportMissingStateMachineAttribute(DiagnosticBag diagnostics, IMethodSymbolInternal method, string stateMachineAttributeFullName)
//        {
//            diagnostics.Add(MessageProvider.CreateDiagnostic(
//                MessageProvider.ERR_EncUpdateFailedMissingAttribute,
//                method.Locations.First(),
//                MessageProvider.GetErrorDisplayString(method),
//                stateMachineAttributeFullName));
//        }

//        private static void MakeLambdaAndClosureMaps(
//            ImmutableArray<LambdaDebugInfo> lambdaDebugInfo,
//            ImmutableArray<ClosureDebugInfo> closureDebugInfo,
//            out IDictionary<int, KeyValuePair<DebugId, int>> lambdaMap,
//            out IDictionary<int, DebugId> closureMap)
//        {
//            var lambdas = new Dictionary<int, KeyValuePair<DebugId, int>>(lambdaDebugInfo.Length);
//            var closures = new Dictionary<int, DebugId>(closureDebugInfo.Length);

//            for (int i = 0; i < lambdaDebugInfo.Length; i++)
//            {
//                var lambdaInfo = lambdaDebugInfo[i];
//                lambdas[lambdaInfo.SyntaxOffset] = KeyValuePairUtil.Create(lambdaInfo.LambdaId, lambdaInfo.ClosureOrdinal);
//            }

//            for (int i = 0; i < closureDebugInfo.Length; i++)
//            {
//                var closureInfo = closureDebugInfo[i];
//                closures[closureInfo.SyntaxOffset] = closureInfo.ClosureId;
//            }

//            lambdaMap = lambdas;
//            closureMap = closures;
//        }

//        private static void GetStateMachineFieldMapFromPreviousCompilation(
//            ImmutableArray<EncHoistedLocalInfo> hoistedLocalSlots,
//            ImmutableArray<Cci.ITypeReference> hoistedAwaiters,
//            out IDictionary<EncHoistedLocalInfo, int> hoistedLocalMap,
//            out IDictionary<Cci.ITypeReference, int> awaiterMap)
//        {
//            var hoistedLocals = new Dictionary<EncHoistedLocalInfo, int>();
//            var awaiters = new Dictionary<Cci.ITypeReference, int>();

//            for (int slotIndex = 0; slotIndex < hoistedLocalSlots.Length; slotIndex++)
//            {
//                var slot = hoistedLocalSlots[slotIndex];
//                if (slot.IsUnused)
//                {
//                    // Unused field.
//                    continue;
//                }

//                hoistedLocals.Add(slot, slotIndex);
//            }

//            for (int slotIndex = 0; slotIndex < hoistedAwaiters.Length; slotIndex++)
//            {
//                var slot = hoistedAwaiters[slotIndex];
//                if (slot == null)
//                {
//                    // Unused awaiter.
//                    continue;
//                }

//                awaiters.Add(slot, slotIndex);
//            }

//            hoistedLocalMap = hoistedLocals;
//            awaiterMap = awaiters;
//        }
//    }
//}
