namespace System.Reflection.Metadata
{
	public readonly struct TypeReferenceHandle : IEquatable<TypeReferenceHandle>
	{
		private const uint tokenType = 16777216u;

		private const byte tokenTypeSmall = 1;

		private readonly int _rowId;

		/// <returns></returns>
		public bool IsNil => RowId == 0;

		internal int RowId => _rowId;

		private TypeReferenceHandle(int rowId)
		{
			_rowId = rowId;
		}

		internal static TypeReferenceHandle FromRowId(int rowId)
		{
			return new TypeReferenceHandle(rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator Handle(TypeReferenceHandle handle)
		{
			return new Handle(1, handle._rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator EntityHandle(TypeReferenceHandle handle)
		{
			return new EntityHandle((uint)(16777216L | (long)handle._rowId));
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator TypeReferenceHandle(Handle handle)
		{
			if (handle.VType != 1)
			{
				Throw.InvalidCast();
			}
			return new TypeReferenceHandle(handle.RowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator TypeReferenceHandle(EntityHandle handle)
		{
			if (handle.VType != 16777216)
			{
				Throw.InvalidCast();
			}
			return new TypeReferenceHandle(handle.RowId);
		}

		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator ==(TypeReferenceHandle left, TypeReferenceHandle right)
		{
			return left._rowId == right._rowId;
		}

		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is TypeReferenceHandle)
			{
				return ((TypeReferenceHandle)obj)._rowId == _rowId;
			}
			return false;
		}

		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(TypeReferenceHandle other)
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
		public static bool operator !=(TypeReferenceHandle left, TypeReferenceHandle right)
		{
			return left._rowId != right._rowId;
		}
	}
}
