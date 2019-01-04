namespace System.Reflection.Metadata
{
	public readonly struct MethodSpecificationHandle : IEquatable<MethodSpecificationHandle>
	{
		private const uint tokenType = 721420288u;

		private const byte tokenTypeSmall = 43;

		private readonly int _rowId;

		/// <returns></returns>
		public bool IsNil => RowId == 0;

		internal int RowId => _rowId;

		private MethodSpecificationHandle(int rowId)
		{
			_rowId = rowId;
		}

		internal static MethodSpecificationHandle FromRowId(int rowId)
		{
			return new MethodSpecificationHandle(rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator Handle(MethodSpecificationHandle handle)
		{
			return new Handle(43, handle._rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator EntityHandle(MethodSpecificationHandle handle)
		{
			return new EntityHandle((uint)(721420288L | (long)handle._rowId));
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator MethodSpecificationHandle(Handle handle)
		{
			if (handle.VType != 43)
			{
				Throw.InvalidCast();
			}
			return new MethodSpecificationHandle(handle.RowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator MethodSpecificationHandle(EntityHandle handle)
		{
			if (handle.VType != 721420288)
			{
				Throw.InvalidCast();
			}
			return new MethodSpecificationHandle(handle.RowId);
		}

		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator ==(MethodSpecificationHandle left, MethodSpecificationHandle right)
		{
			return left._rowId == right._rowId;
		}

		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is MethodSpecificationHandle)
			{
				return ((MethodSpecificationHandle)obj)._rowId == _rowId;
			}
			return false;
		}

		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(MethodSpecificationHandle other)
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
		public static bool operator !=(MethodSpecificationHandle left, MethodSpecificationHandle right)
		{
			return left._rowId != right._rowId;
		}
	}
}
