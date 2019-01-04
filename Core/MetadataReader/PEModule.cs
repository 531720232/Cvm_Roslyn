// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading;
using Microsoft.CodeAnalysis.PooledObjects;
using Roslyn.Utilities;

namespace Microsoft.CodeAnalysis
{
    /// <summary>
    /// A set of helpers for extracting elements from metadata.
    /// This type is not responsible for managing the underlying storage
    /// backing the PE image.
    /// </summary>
    internal sealed class PEModule : IDisposable
    {
        internal const string ByRefLikeMarker = "Types with embedded references are not supported in this version of your compiler.";

        /// <summary>
        /// We need to store reference to the module metadata to keep the metadata alive while 
        /// symbols have reference to PEModule.
        /// </summary>

        // Either we have PEReader or we have pointer and size of the metadata blob:
        private readonly IntPtr _metadataPointerOpt;
        private readonly int _metadataSizeOpt;


        private ImmutableArray<AssemblyIdentity> _lazyAssemblyReferences;

        /// <summary>
        /// This is a tuple for optimization purposes. In valid cases, we need to store
        /// only one assembly index per type. However, if we found more than one, we
        /// keep a second one as well to use it for error reporting.
        /// We use -1 in case there was no forward.
        /// </summary>
        private Dictionary<string, (int FirstIndex, int SecondIndex)> _lazyForwardedTypesToAssemblyIndexMap;

        private readonly Lazy<IdentifierCollection> _lazyTypeNameCollection;
        private readonly Lazy<IdentifierCollection> _lazyNamespaceNameCollection;

        private string _lazyName;
        private bool _isDisposed;

        /// <summary>
        /// Using <see cref="ThreeState"/> as a type for atomicity.
        /// </summary>
        private ThreeState _lazyContainsNoPiaLocalTypes;

        /// <summary>
        /// If bitmap is not null, each bit indicates whether a TypeDef 
        /// with corresponding RowId has been checked if it is a NoPia 
        /// local type. If the bit is 1, local type will have an entry 
        /// in m_lazyTypeDefToTypeIdentifierMap.
        /// </summary>
        private int[] _lazyNoPiaLocalTypeCheckBitMap;

        /// <summary>
        /// For each TypeDef that has 1 in m_lazyNoPiaLocalTypeCheckBitMap,
        /// this map stores corresponding TypeIdentifier AttributeInfo. 
        /// </summary>

        // The module can be used by different compilations or different versions of the "same"
        // compilation, which use different hash algorithms. Let's cache result for each distinct 
        // algorithm.



        // 'ignoreAssemblyRefs' is used by the EE only, when debugging
        // .NET Native, where the corlib may have assembly references
        // (see https://github.com/dotnet/roslyn/issues/13275).
        internal PEModule(int metadataSizeOpt, bool includeEmbeddedInteropTypes, bool ignoreAssemblyRefs)
        {
            // shall not throw



            _metadataSizeOpt = metadataSizeOpt;
            _lazyTypeNameCollection = new Lazy<IdentifierCollection>(ComputeTypeNameCollection);
            _lazyNamespaceNameCollection = new Lazy<IdentifierCollection>(ComputeNamespaceNameCollection);
            _lazyContainsNoPiaLocalTypes = includeEmbeddedInteropTypes ? ThreeState.False : ThreeState.Unknown;

            if (ignoreAssemblyRefs)
            {
                _lazyAssemblyReferences = ImmutableArray<AssemblyIdentity>.Empty;
            }
        }
        private IdentifierCollection ComputeNamespaceNameCollection()
        {
            try
            {

                var ts = CVM.GlobalDefine.Instance.clr_types;
                var full = new List<string>();
                foreach (var t in ts)
                {

                    if (full.Contains(t.Namespace) || string.IsNullOrWhiteSpace(t.Namespace))
                        continue;
                    full.Add(t.Namespace);
                }


                var namespaceNames =
                    from fullName in full.Distinct()
                    from name in fullName.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries)
                    select name;

                return new IdentifierCollection(namespaceNames);
            }
            catch
            {
                return new IdentifierCollection();
            }
        }

        private IdentifierCollection ComputeTypeNameCollection()
        {
            try
            {
                var allTypeDefs = CVM.GlobalDefine.Instance.clr_types;
                var typeNames =
                    from typeDef in allTypeDefs
                    let metadataName = typeDef.Name
                    let backtickIndex = metadataName.IndexOf('`')
                    select backtickIndex < 0 ? metadataName : metadataName.Substring(0, backtickIndex);

                return new IdentifierCollection(typeNames);
            }
            catch
            {
                return new IdentifierCollection();
            }
        }

        internal bool IsDisposed
        {
            get
            {
                return _isDisposed;
            }
        }

        public void Dispose()
        {
            _isDisposed = true;

        }





        private unsafe void InitializeMetadataReader()
        {


        }

        private static void ThrowMetadataDisposed()
        {
            throw new ObjectDisposedException("");
        }


   



        /// <exception cref="BadImageFormatException">An exception from metadata reader.</exception>
        public TypeDefinitionHandle GetContainingTypeOrThrow(TypeDefinitionHandle typeDef)
        {
            return default;
        }

        /// <exception cref="BadImageFormatException">An exception from metadata reader.</exception>
        public string GetTypeDefNameOrThrow(TypeDefinitionHandle typeDef)
        {


            return "";
        }

        /// <exception cref="BadImageFormatException">An exception from metadata reader.</exception>
        public string GetTypeDefNamespaceOrThrow(TypeDefinitionHandle typeDef)
        {
            return default;
        }

        /// <exception cref="BadImageFormatException">An exception from metadata reader.</exception>
        public EntityHandle GetTypeDefExtendsOrThrow(TypeDefinitionHandle typeDef)
        {
            return default;
        }

        /// <exception cref="BadImageFormatException">An exception from metadata reader.</exception>
        public TypeAttributes GetTypeDefFlagsOrThrow(TypeDefinitionHandle typeDef)
        {
            return default;
        }

        /// <exception cref="BadImageFormatException">An exception from metadata reader.</exception>


        /// <exception cref="BadImageFormatException">An exception from metadata reader.</exception>

        /// <exception cref="BadImageFormatException">An exception from metadata reader.</exception>
        

        /// <summary>
        /// The function groups types defined in the module by their fully-qualified namespace name.
        /// The case-sensitivity of the grouping depends upon the provided StringComparer.
        /// 
        /// The sequence is sorted by name by using provided comparer. Therefore, if there are multiple 
        /// groups for a namespace name (e.g. because they differ in case), the groups are going to be 
        /// adjacent to each other. 
        /// 
        /// Empty string is used as namespace name for types in the Global namespace. Therefore, all types 
        /// in the Global namespace, if any, should be in the first group (assuming a reasonable StringComparer).
        /// </summary>
        /// Comparer to sort the groups.
        /// <param name="nameComparer">
        /// </param>
        /// <returns>A sorted list of TypeDef row ids, grouped by fully-qualified namespace name.</returns>
        /// <exception cref="BadImageFormatException">An exception from metadata reader.</exception>
     
        internal class TypesByNamespaceSortComparer : IComparer<IGrouping<string, Type>>
        {
            private readonly StringComparer _nameComparer;

            public TypesByNamespaceSortComparer(StringComparer nameComparer)
            {
                _nameComparer = nameComparer;
            }

            public int Compare(IGrouping<string, Type> left, IGrouping<string, Type> right)
            {
                if (left == right)
                {
                    return 0;
                }

                int result = _nameComparer.Compare(left.Key, right.Key);

                if (result == 0)
                {
                    var fLeft = left.FirstOrDefault();
                    var fRight = right.FirstOrDefault();

                    if (fLeft==null ^ fRight==null)
                    {
                        result = fLeft==null ? +1 : -1;
                    }
                    //else
                    //{
                    //    result =// HandleComparer.Default.Compare(fLeft, fRight);
                    //}

                    if (result == 0)
                    {
                        // This can only happen when both are for forwarded types.
                        Debug.Assert(left.IsEmpty() && right.IsEmpty());
                        result = string.CompareOrdinal(left.Key, right.Key);
                    }
                }

                Debug.Assert(result != 0);
                return result;
            }
        }

        /// <summary>
        /// Groups together the RowIds of types in a given namespaces.  The types considered are
        /// those defined in this module.
        /// </summary>
        /// <exception cref="BadImageFormatException">An exception from metadata reader.</exception>

  

        /// <summary>
        /// Supplements the namespace-to-RowIDs map with the namespaces of forwarded types.
        /// These types will not have associated row IDs (represented as null, for efficiency).
        /// These namespaces are important because we want lookups of missing forwarded types
        /// to succeed far enough that we can actually find the type forwarder and provide
        /// information about the target assembly.
        /// 
        /// For example, consider the following forwarded type:
        /// 
        /// .class extern forwarder Namespace.Type {}
        /// 
        /// If this type is referenced in source as "Namespace.Type", then dev10 reports
        /// 
        /// error CS1070: The type name 'Namespace.Name' could not be found. This type has been 
        /// forwarded to assembly 'pe2, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'. 
        /// Consider adding a reference to that assembly.
        /// 
        /// If we did not include "Namespace" as a child of the global namespace of this module
        /// (the forwarding module), then Roslyn would report that the type "Namespace" was not
        /// found and say nothing about "Name" (because of the diagnostic already attached to 
        /// the qualifier).
        /// </summary>
        /// <exception cref="BadImageFormatException">An exception from metadata reader.</exception>
  



        internal static bool IsNested(TypeAttributes flags)
        {
            return (flags & ((TypeAttributes)0x00000006)) != 0;
        }

        /// <summary>
        /// Returns true if method IL can be retrieved from the module.
        /// </summary>
        internal bool HasIL
        {
            get { return true; }
        }



        /// <exception cref="BadImageFormatException">Invalid metadata.</exception>

        // TODO: remove, API should be provided by MetadataReader


        // Provides a UTF8 decoder to the MetadataReader that reuses strings from the string table
        // rather than allocating on each call to MetadataReader.GetString(handle).

        internal bool IsCOFFOnly
        {
            get
            {
              

                return false;
            }
        }

        public ICollection<string> TypeNames => _lazyTypeNameCollection?.Value.AsCaseInsensitiveCollection();

        public ICollection<string> NamespaceNames => _lazyNamespaceNameCollection?.Value.AsCaseInsensitiveCollection();

        internal bool HasNullableAttribute(Type handle, out byte transformFlag, out object _)
        {
            throw new NotImplementedException();
        }

        internal void HasDefaultMemberAttribute(ICustomAttributeProvider handle, out string defaultMemberName)
        {
            foreach (DefaultMemberAttribute VARIABLE in handle.GetCustomAttributes(typeof(System.Reflection.DefaultMemberAttribute), false))
            {
                defaultMemberName = VARIABLE.MemberName;

                return;
            }
            defaultMemberName = null;
            return;
        }
        internal ConstantValue GetConstantValue(object handle)
        {
            switch (handle)
            {
                case bool b:
                    return ConstantValue.Create(b);
                case char c:
                    return ConstantValue.Create(c);
                case sbyte s:
                    return ConstantValue.Create(s);
                case Int16 it16:
                    return ConstantValue.Create(it16);
                case Int32 i32:
                    return ConstantValue.Create(i32);
                case Int64 i64:
                    return ConstantValue.Create(i64);
                case byte be:
                    return ConstantValue.Create(be);
                case UInt16 u16:
                    return ConstantValue.Create(u16);
                case UInt32 u32:
                    return ConstantValue.Create(u32);
                case UInt64 u64:
                    return ConstantValue.Create(u64);
                case Single sg:
                    return ConstantValue.Create(sg);
                case Double de:
                    return ConstantValue.Create(de);
                case String sr:
                    return ConstantValue.Create(sr);
                case null:
                    return ConstantValue.Null;

                default:

                    return ConstantValue.Bad;
            }
        }

        internal ConstantValue GetConstantFieldValue(FieldInfo handle)
        {
            switch (handle.GetRawConstantValue())
            {
                case bool b:
                    return ConstantValue.Create(b);
                case char c:
                    return ConstantValue.Create(c);
                case sbyte s:
                    return ConstantValue.Create(s);
                case Int16 it16:
                    return ConstantValue.Create(it16);
                case Int32 i32:
                    return ConstantValue.Create(i32);
                case Int64 i64:
                    return ConstantValue.Create(i64);
                case byte be:
                    return ConstantValue.Create(be);
                case UInt16 u16:
                    return ConstantValue.Create(u16);
                case UInt32 u32:
                    return ConstantValue.Create(u32);
                case UInt64 u64:
                    return ConstantValue.Create(u64);
                case Single sg:
                    return ConstantValue.Create(sg);
                case Double de:
                    return ConstantValue.Create(de);
                case String sr:
                    return ConstantValue.Create(sr);
                case null:
                    return ConstantValue.Null;

                default:

                    return ConstantValue.Bad;
            }
        }

        internal bool HasDecimalConstantAttribute(ICustomAttributeProvider handle, out ConstantValue defaultValue)
        {
            defaultValue = ConstantValue.Bad;
            try
            {
                var objs = handle.GetCustomAttributes(typeof(System.Runtime.CompilerServices.DecimalConstantAttribute), false);

                foreach (var obj in objs)
                {
                    if (obj is System.Runtime.CompilerServices.DecimalConstantAttribute d)
                    {
                        defaultValue = ConstantValue.Create(d.Value);
                        return true;
                    }
                }


            }
            catch
            {

            }
            return false;
        }
        internal ImmutableArray<string> GetConditionalAttributeValues(ICustomAttributeProvider handle)
        {
            List<string> str = new List<string>();
            foreach (ConditionalAttribute VARIABLE in handle.GetCustomAttributes(typeof(ConditionalAttribute), false))
            {
              str.Add( VARIABLE.ConditionString);
            }
            return str.ToImmutableArray();
          
        }
        private struct TypeDefToNamespace
        {
            internal readonly Type TypeDef;
            internal readonly string NamespaceHandle;

            internal TypeDefToNamespace(Type typeDef, string namespaceHandle)
            {
                TypeDef = typeDef;
                NamespaceHandle = namespaceHandle;
            }
        }
        private IEnumerable<TypeDefToNamespace> GetTypeDefsOrThrow(bool topLevelOnly)
        {
            foreach (var typeDef in CVM.GlobalDefine.Instance.clr_types)
            {


                if (topLevelOnly && typeDef.IsNested)
                {
                    continue;
                }

                yield return new TypeDefToNamespace(typeDef, typeDef.Namespace);
            }
        }
        private void GetTypeNamespaceNamesOrThrow(Dictionary<string, ArrayBuilder<Type>> namespaces)
        {
            // PERF: Group by namespace handle so we only have to allocate one string for every namespace
            var namespaceHandles = new Dictionary<string, ArrayBuilder<Type>>();//(NamespaceHandleEqualityComparer.Singleton);
            foreach (TypeDefToNamespace pair in GetTypeDefsOrThrow(topLevelOnly: true))
            {
                var nsHandle = pair.NamespaceHandle;
                var typeDef = pair.TypeDef;

                ArrayBuilder<Type> builder;
                if (nsHandle == null)
                {
                    nsHandle = "";
                }
                if (namespaceHandles.TryGetValue(nsHandle, out builder))
                {
                    builder.Add(typeDef);
                }
                else
                {
                    namespaceHandles.Add(nsHandle, new ArrayBuilder<Type> { typeDef });
                }
            }

            foreach (var kvp in namespaceHandles)
            {
                string @namespace = kvp.Key;

                ArrayBuilder<Type> builder;

                if (namespaces.TryGetValue(@namespace, out builder))
                {
                    builder.AddRange(kvp.Value);
                }
                else
                {
                    namespaces.Add(@namespace, kvp.Value);
                }
            }
        }

        internal bool HasDateTimeConstantAttribute(ICustomAttributeProvider handle, out ConstantValue value)
        {
            value = ConstantValue.Bad;
            foreach (System.Runtime.CompilerServices.DateTimeConstantAttribute a in handle.GetCustomAttributes(typeof(System.Runtime.CompilerServices.DateTimeConstantAttribute), false))
            {
                try
                {
                    DateTime w = new DateTime((long)a.Value);
                    value = ConstantValue.Create(w);
                    return true;

                }
                catch
                {

                }

            }
            return false;
        }

        internal IEnumerable<IGrouping<string, Type>> GroupTypesByNamespaceOrThrow(StringComparer nameComparer)
        {
            // TODO: Consider if we should cache the result (not the IEnumerable, but the actual values).

            // NOTE:  Rather than use a sorted dictionary, we accumulate the groupings in a normal dictionary
            // and then sort the list.  We do this so that namespaces with distinct names are not
            // merged, even if they are equal according to the provided comparer.  This improves the error
            // experience because types retain their exact namespaces.

            Dictionary<string, ArrayBuilder<Type>> namespaces = new Dictionary<string, ArrayBuilder<Type>>();

            GetTypeNamespaceNamesOrThrow(namespaces);


            var result = new ArrayBuilder<IGrouping<string, Type>>(namespaces.Count);

            foreach (var pair in namespaces)
            {
                result.Add(new Grouping<string, Type>(pair.Key, pair.Value ?? SpecializedCollections.EmptyEnumerable<Type>()));
            }

            result.Sort(new TypesByNamespaceSortComparer(nameComparer));
            return result;
        }

        internal bool HasExtensionAttribute(MethodBase handle, bool ignoreCase)
        {
            var ms = handle.GetCustomAttributes(typeof(System.Runtime.CompilerServices.ExtensionAttribute), false);
            {
                foreach(var v in ms)
                {
                    return true;
                }
            }
            return false;
        }
    }
}

