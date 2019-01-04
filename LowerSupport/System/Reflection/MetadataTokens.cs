namespace System.Reflection.Metadata.Ecma335
{
	public static class MetadataTokens
	{
		/// <returns></returns>
		public static readonly int TableCount = 64;

		/// <returns></returns>
		public static readonly int HeapCount = 4;

		/// <param name="reader"></param>
		/// <param name="handle"></param>
		/// <returns></returns>
		

		/// <param name="reader"></param>
		/// <param name="handle"></param>
		/// <returns></returns>

		/// <param name="reader"></param>
		/// <param name="handle"></param>
		/// <returns></returns>


		/// <param name="handle"></param>
		/// <returns></returns>
		public static int GetRowNumber(EntityHandle handle)
		{
			if (!handle.IsVirtual)
			{
				return handle.RowId;
			}
			return -1;
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static int GetHeapOffset(Handle handle)
		{
			if (!handle.IsHeapHandle)
			{
				Throw.HeapHandleRequired();
			}
			if (handle.IsVirtual)
			{
				return -1;
			}
			return handle.Offset;
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static int GetHeapOffset(BlobHandle handle)
		{
			if (!handle.IsVirtual)
			{
				return handle.GetHeapOffset();
			}
			return -1;
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static int GetHeapOffset(GuidHandle handle)
		{
			return handle.Index;
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static int GetHeapOffset(UserStringHandle handle)
		{
			return handle.GetHeapOffset();
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static int GetHeapOffset(StringHandle handle)
		{
			if (!handle.IsVirtual)
			{
				return handle.GetHeapOffset();
			}
			return -1;
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static int GetToken(Handle handle)
		{
			if (!handle.IsEntityOrUserStringHandle)
			{
				Throw.EntityOrUserStringHandleRequired();
			}
			if (handle.IsVirtual)
			{
				return 0;
			}
			return handle.Token;
		}
        public static int GetToken(Type handle)
        {
        
            return handle.MetadataToken;
        }

        /// <param name="handle"></param>
        /// <returns></returns>
        public static int GetToken(EntityHandle handle)
		{
			if (!handle.IsVirtual)
			{
				return handle.Token;
			}
			return 0;
		}

		/// <param name="type"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		public static bool TryGetTableIndex(HandleKind type, out TableIndex index)
		{
			if ((int)type < TableCount && ((1L << (int)type) & 0xFF1FC9FFFFFFFF) != 0L)
			{
				index = (TableIndex)type;
				return true;
			}
			index = TableIndex.Module;
			return false;
		}

		/// <param name="type"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		public static bool TryGetHeapIndex(HandleKind type, out HeapIndex index)
		{
			switch (type)
			{
			case HandleKind.UserString:
				index = HeapIndex.UserString;
				return true;
			case HandleKind.String:
			case HandleKind.NamespaceDefinition:
				index = HeapIndex.String;
				return true;
			case HandleKind.Blob:
				index = HeapIndex.Blob;
				return true;
			case HandleKind.Guid:
				index = HeapIndex.Guid;
				return true;
			default:
				index = HeapIndex.UserString;
				return false;
			}
		}

		/// <param name="token"></param>
		/// <returns></returns>
		public static Handle Handle(int token)
		{
			if (!TokenTypeIds.IsEntityOrUserStringToken((uint)token))
			{
				Throw.InvalidToken();
			}
			return System.Reflection.Metadata.Handle.FromVToken((uint)token);
		}

		/// <param name="token"></param>
		/// <returns></returns>
		public static EntityHandle EntityHandle(int token)
		{
			if (!TokenTypeIds.IsEntityToken((uint)token))
			{
				Throw.InvalidToken();
			}
			return new EntityHandle((uint)token);
		}

		/// <param name="tableIndex"></param>
		/// <param name="rowNumber"></param>
		/// <returns></returns>
		public static EntityHandle EntityHandle(TableIndex tableIndex, int rowNumber)
		{
			return Handle(tableIndex, rowNumber);
		}

		/// <param name="tableIndex"></param>
		/// <param name="rowNumber"></param>
		/// <returns></returns>
		public static EntityHandle Handle(TableIndex tableIndex, int rowNumber)
		{
			int vToken = (int)((uint)tableIndex << 24) | rowNumber;
			if (!TokenTypeIds.IsEntityOrUserStringToken((uint)vToken))
			{
				Throw.TableIndexOutOfRange();
			}
			return new EntityHandle((uint)vToken);
		}

		private static int ToRowId(int rowNumber)
		{
			return rowNumber & 0xFFFFFF;
		}

		/// <param name="rowNumber"></param>
		/// <returns></returns>
		public static MethodDefinitionHandle MethodDefinitionHandle(int rowNumber)
		{
			return System.Reflection.Metadata.MethodDefinitionHandle.FromRowId(ToRowId(rowNumber));
		}

		/// <param name="rowNumber"></param>
		/// <returns></returns>
		public static MethodImplementationHandle MethodImplementationHandle(int rowNumber)
		{
			return System.Reflection.Metadata.MethodImplementationHandle.FromRowId(ToRowId(rowNumber));
		}

		/// <param name="rowNumber"></param>
		/// <returns></returns>
		public static MethodSpecificationHandle MethodSpecificationHandle(int rowNumber)
		{
			return System.Reflection.Metadata.MethodSpecificationHandle.FromRowId(ToRowId(rowNumber));
		}

		/// <param name="rowNumber"></param>
		/// <returns></returns>
		public static TypeDefinitionHandle TypeDefinitionHandle(int rowNumber)
		{
			return System.Reflection.Metadata.TypeDefinitionHandle.FromRowId(ToRowId(rowNumber));
		}

		/// <param name="rowNumber"></param>
		/// <returns></returns>
		public static ExportedTypeHandle ExportedTypeHandle(int rowNumber)
		{
			return System.Reflection.Metadata.ExportedTypeHandle.FromRowId(ToRowId(rowNumber));
		}

		/// <param name="rowNumber"></param>
		/// <returns></returns>
		public static TypeReferenceHandle TypeReferenceHandle(int rowNumber)
		{
			return System.Reflection.Metadata.TypeReferenceHandle.FromRowId(ToRowId(rowNumber));
		}

		/// <param name="rowNumber"></param>
		/// <returns></returns>
		public static TypeSpecificationHandle TypeSpecificationHandle(int rowNumber)
		{
			return System.Reflection.Metadata.TypeSpecificationHandle.FromRowId(ToRowId(rowNumber));
		}

		/// <param name="rowNumber"></param>
		/// <returns></returns>
		public static InterfaceImplementationHandle InterfaceImplementationHandle(int rowNumber)
		{
			return System.Reflection.Metadata.InterfaceImplementationHandle.FromRowId(ToRowId(rowNumber));
		}

		/// <param name="rowNumber"></param>
		/// <returns></returns>
		public static MemberReferenceHandle MemberReferenceHandle(int rowNumber)
		{
			return System.Reflection.Metadata.MemberReferenceHandle.FromRowId(ToRowId(rowNumber));
		}

		/// <param name="rowNumber"></param>
		/// <returns></returns>
		public static FieldDefinitionHandle FieldDefinitionHandle(int rowNumber)
		{
			return System.Reflection.Metadata.FieldDefinitionHandle.FromRowId(ToRowId(rowNumber));
		}

		/// <param name="rowNumber"></param>
		/// <returns></returns>
		public static EventDefinitionHandle EventDefinitionHandle(int rowNumber)
		{
			return System.Reflection.Metadata.EventDefinitionHandle.FromRowId(ToRowId(rowNumber));
		}

		/// <param name="rowNumber"></param>
		/// <returns></returns>
		public static PropertyDefinitionHandle PropertyDefinitionHandle(int rowNumber)
		{
			return System.Reflection.Metadata.PropertyDefinitionHandle.FromRowId(ToRowId(rowNumber));
		}

		/// <param name="rowNumber"></param>
		/// <returns></returns>
		public static StandaloneSignatureHandle StandaloneSignatureHandle(int rowNumber)
		{
			return System.Reflection.Metadata.StandaloneSignatureHandle.FromRowId(ToRowId(rowNumber));
		}

		/// <param name="rowNumber"></param>
		/// <returns></returns>
		public static ParameterHandle ParameterHandle(int rowNumber)
		{
			return System.Reflection.Metadata.ParameterHandle.FromRowId(ToRowId(rowNumber));
		}

		/// <param name="rowNumber"></param>
		/// <returns></returns>
		public static GenericParameterHandle GenericParameterHandle(int rowNumber)
		{
			return System.Reflection.Metadata.GenericParameterHandle.FromRowId(ToRowId(rowNumber));
		}

		/// <param name="rowNumber"></param>
		/// <returns></returns>
		public static GenericParameterConstraintHandle GenericParameterConstraintHandle(int rowNumber)
		{
			return System.Reflection.Metadata.GenericParameterConstraintHandle.FromRowId(ToRowId(rowNumber));
		}

		/// <param name="rowNumber"></param>
		/// <returns></returns>
		public static ModuleReferenceHandle ModuleReferenceHandle(int rowNumber)
		{
			return System.Reflection.Metadata.ModuleReferenceHandle.FromRowId(ToRowId(rowNumber));
		}

		/// <param name="rowNumber"></param>
		/// <returns></returns>
		public static AssemblyReferenceHandle AssemblyReferenceHandle(int rowNumber)
		{
			return System.Reflection.Metadata.AssemblyReferenceHandle.FromRowId(ToRowId(rowNumber));
		}

		/// <param name="rowNumber"></param>
		/// <returns></returns>
		public static CustomAttributeHandle CustomAttributeHandle(int rowNumber)
		{
			return System.Reflection.Metadata.CustomAttributeHandle.FromRowId(ToRowId(rowNumber));
		}

		/// <param name="rowNumber"></param>
		/// <returns></returns>
		public static DeclarativeSecurityAttributeHandle DeclarativeSecurityAttributeHandle(int rowNumber)
		{
			return System.Reflection.Metadata.DeclarativeSecurityAttributeHandle.FromRowId(ToRowId(rowNumber));
		}

		/// <param name="rowNumber"></param>
		/// <returns></returns>
		public static ConstantHandle ConstantHandle(int rowNumber)
		{
			return System.Reflection.Metadata.ConstantHandle.FromRowId(ToRowId(rowNumber));
		}

		/// <param name="rowNumber"></param>
		/// <returns></returns>
		public static ManifestResourceHandle ManifestResourceHandle(int rowNumber)
		{
			return System.Reflection.Metadata.ManifestResourceHandle.FromRowId(ToRowId(rowNumber));
		}

		/// <param name="rowNumber"></param>
		/// <returns></returns>
		public static AssemblyFileHandle AssemblyFileHandle(int rowNumber)
		{
			return System.Reflection.Metadata.AssemblyFileHandle.FromRowId(ToRowId(rowNumber));
		}

		/// <param name="rowNumber"></param>
		/// <returns></returns>
		public static DocumentHandle DocumentHandle(int rowNumber)
		{
			return System.Reflection.Metadata.DocumentHandle.FromRowId(ToRowId(rowNumber));
		}

		/// <param name="rowNumber"></param>
		/// <returns></returns>
		public static MethodDebugInformationHandle MethodDebugInformationHandle(int rowNumber)
		{
			return System.Reflection.Metadata.MethodDebugInformationHandle.FromRowId(ToRowId(rowNumber));
		}

		/// <param name="rowNumber"></param>
		/// <returns></returns>
		public static LocalScopeHandle LocalScopeHandle(int rowNumber)
		{
			return System.Reflection.Metadata.LocalScopeHandle.FromRowId(ToRowId(rowNumber));
		}

		/// <param name="rowNumber"></param>
		/// <returns></returns>
		public static LocalVariableHandle LocalVariableHandle(int rowNumber)
		{
			return System.Reflection.Metadata.LocalVariableHandle.FromRowId(ToRowId(rowNumber));
		}

		/// <param name="rowNumber"></param>
		/// <returns></returns>
		public static LocalConstantHandle LocalConstantHandle(int rowNumber)
		{
			return System.Reflection.Metadata.LocalConstantHandle.FromRowId(ToRowId(rowNumber));
		}

		/// <param name="rowNumber"></param>
		/// <returns></returns>
		public static ImportScopeHandle ImportScopeHandle(int rowNumber)
		{
			return System.Reflection.Metadata.ImportScopeHandle.FromRowId(ToRowId(rowNumber));
		}

		/// <param name="rowNumber"></param>
		/// <returns></returns>
		public static CustomDebugInformationHandle CustomDebugInformationHandle(int rowNumber)
		{
			return System.Reflection.Metadata.CustomDebugInformationHandle.FromRowId(ToRowId(rowNumber));
		}

		/// <param name="offset"></param>
		/// <returns></returns>
		public static UserStringHandle UserStringHandle(int offset)
		{
			return System.Reflection.Metadata.UserStringHandle.FromOffset(offset & 0xFFFFFF);
		}

		/// <param name="offset"></param>
		/// <returns></returns>
		public static StringHandle StringHandle(int offset)
		{
			return System.Reflection.Metadata.StringHandle.FromOffset(offset);
		}

		/// <param name="offset"></param>
		/// <returns></returns>
		public static BlobHandle BlobHandle(int offset)
		{
			return System.Reflection.Metadata.BlobHandle.FromOffset(offset);
		}

		/// <param name="offset"></param>
		/// <returns></returns>
		public static GuidHandle GuidHandle(int offset)
		{
			return System.Reflection.Metadata.GuidHandle.FromIndex(offset);
		}

		/// <param name="offset"></param>
		/// <returns></returns>
		public static DocumentNameBlobHandle DocumentNameBlobHandle(int offset)
		{
			return System.Reflection.Metadata.DocumentNameBlobHandle.FromOffset(offset);
		}
	}
}
