namespace System.Reflection.Metadata
{
	public readonly struct ConstantHandle : IEquatable<ConstantHandle>
	{
		private const uint tokenType = 184549376u;

		private const byte tokenTypeSmall = 11;

		private readonly int _rowId;

		/// <returns></returns>
		public bool IsNil => RowId == 0;

		internal int RowId => _rowId;

		private ConstantHandle(int rowId)
		{
			_rowId = rowId;
		}

		internal static ConstantHandle FromRowId(int rowId)
		{
			return new ConstantHandle(rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator Handle(ConstantHandle handle)
		{
			return new Handle(11, handle._rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator EntityHandle(ConstantHandle handle)
		{
			return new EntityHandle((uint)(184549376L | (long)handle._rowId));
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator ConstantHandle(Handle handle)
		{
			if (handle.VType != 11)
			{
				Throw.InvalidCast();
			}
			return new ConstantHandle(handle.RowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator ConstantHandle(EntityHandle handle)
		{
			if (handle.VType != 184549376)
			{
				Throw.InvalidCast();
			}
			return new ConstantHandle(handle.RowId);
		}

		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator ==(ConstantHandle left, ConstantHandle right)
		{
			return left._rowId == right._rowId;
		}

		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is ConstantHandle)
			{
				return ((ConstantHandle)obj)._rowId == _rowId;
			}
			return false;
		}

		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(ConstantHandle other)
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
		public static bool operator !=(ConstantHandle left, ConstantHandle right)
		{
			return left._rowId != right._rowId;
		}
	}
}
