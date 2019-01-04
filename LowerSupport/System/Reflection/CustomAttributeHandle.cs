namespace System.Reflection.Metadata
{
	public readonly struct CustomAttributeHandle : IEquatable<CustomAttributeHandle>
	{
		private const uint tokenType = 201326592u;

		private const byte tokenTypeSmall = 12;

		private readonly int _rowId;

		/// <returns></returns>
		public bool IsNil => _rowId == 0;

		internal int RowId => _rowId;

		private CustomAttributeHandle(int rowId)
		{
			_rowId = rowId;
		}

		internal static CustomAttributeHandle FromRowId(int rowId)
		{
			return new CustomAttributeHandle(rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator Handle(CustomAttributeHandle handle)
		{
			return new Handle(12, handle._rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator EntityHandle(CustomAttributeHandle handle)
		{
			return new EntityHandle((uint)(201326592L | (long)handle._rowId));
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator CustomAttributeHandle(Handle handle)
		{
			if (handle.VType != 12)
			{
				Throw.InvalidCast();
			}
			return new CustomAttributeHandle(handle.RowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator CustomAttributeHandle(EntityHandle handle)
		{
			if (handle.VType != 201326592)
			{
				Throw.InvalidCast();
			}
			return new CustomAttributeHandle(handle.RowId);
		}

		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator ==(CustomAttributeHandle left, CustomAttributeHandle right)
		{
			return left._rowId == right._rowId;
		}

		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is CustomAttributeHandle)
			{
				return ((CustomAttributeHandle)obj)._rowId == _rowId;
			}
			return false;
		}

		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(CustomAttributeHandle other)
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
		public static bool operator !=(CustomAttributeHandle left, CustomAttributeHandle right)
		{
			return left._rowId != right._rowId;
		}
	}
}
