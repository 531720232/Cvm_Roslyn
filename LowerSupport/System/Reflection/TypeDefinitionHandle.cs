namespace System.Reflection.Metadata
{
	public readonly struct TypeDefinitionHandle : IEquatable<TypeDefinitionHandle>
	{
		private const uint tokenType = 33554432u;

		private const byte tokenTypeSmall = 2;

		private readonly int _rowId;

		/// <returns></returns>
		public bool IsNil => RowId == 0;

		internal int RowId => _rowId;

		private TypeDefinitionHandle(int rowId)
		{
			_rowId = rowId;
		}

		internal static TypeDefinitionHandle FromRowId(int rowId)
		{
			return new TypeDefinitionHandle(rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator Handle(TypeDefinitionHandle handle)
		{
			return new Handle(2, handle._rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator EntityHandle(TypeDefinitionHandle handle)
		{
			return new EntityHandle((uint)(33554432L | (long)handle._rowId));
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator TypeDefinitionHandle(Handle handle)
		{
			if (handle.VType != 2)
			{
				Throw.InvalidCast();
			}
			return new TypeDefinitionHandle(handle.RowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator TypeDefinitionHandle(EntityHandle handle)
		{
			if (handle.VType != 33554432)
			{
				Throw.InvalidCast();
			}
			return new TypeDefinitionHandle(handle.RowId);
		}

		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator ==(TypeDefinitionHandle left, TypeDefinitionHandle right)
		{
			return left._rowId == right._rowId;
		}

		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is TypeDefinitionHandle)
			{
				return ((TypeDefinitionHandle)obj)._rowId == _rowId;
			}
			return false;
		}

		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(TypeDefinitionHandle other)
		{
			return _rowId == other._rowId;
		}

		/// <returns></returns>
		public override int GetHashCode()
		{
			return _rowId.GetHashCode();
		}

		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator !=(TypeDefinitionHandle left, TypeDefinitionHandle right)
		{
			return left._rowId != right._rowId;
		}
	}
}
