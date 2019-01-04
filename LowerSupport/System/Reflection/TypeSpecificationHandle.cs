namespace System.Reflection.Metadata
{
	public readonly struct TypeSpecificationHandle : IEquatable<TypeSpecificationHandle>
	{
		private const uint tokenType = 452984832u;

		private const byte tokenTypeSmall = 27;

		private readonly int _rowId;

		/// <returns></returns>
		public bool IsNil => RowId == 0;

		internal int RowId => _rowId;

		private TypeSpecificationHandle(int rowId)
		{
			_rowId = rowId;
		}

		internal static TypeSpecificationHandle FromRowId(int rowId)
		{
			return new TypeSpecificationHandle(rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator Handle(TypeSpecificationHandle handle)
		{
			return new Handle(27, handle._rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator EntityHandle(TypeSpecificationHandle handle)
		{
			return new EntityHandle((uint)(452984832L | (long)handle._rowId));
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator TypeSpecificationHandle(Handle handle)
		{
			if (handle.VType != 27)
			{
				Throw.InvalidCast();
			}
			return new TypeSpecificationHandle(handle.RowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator TypeSpecificationHandle(EntityHandle handle)
		{
			if (handle.VType != 452984832)
			{
				Throw.InvalidCast();
			}
			return new TypeSpecificationHandle(handle.RowId);
		}

		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator ==(TypeSpecificationHandle left, TypeSpecificationHandle right)
		{
			return left._rowId == right._rowId;
		}

		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is TypeSpecificationHandle)
			{
				return ((TypeSpecificationHandle)obj)._rowId == _rowId;
			}
			return false;
		}

		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(TypeSpecificationHandle other)
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
		public static bool operator !=(TypeSpecificationHandle left, TypeSpecificationHandle right)
		{
			return left._rowId != right._rowId;
		}
	}
}
