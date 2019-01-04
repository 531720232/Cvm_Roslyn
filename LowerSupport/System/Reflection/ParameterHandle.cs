namespace System.Reflection.Metadata
{
	public readonly struct ParameterHandle : IEquatable<ParameterHandle>
	{
		private const uint tokenType = 134217728u;

		private const byte tokenTypeSmall = 8;

		private readonly int _rowId;

		/// <returns></returns>
		public bool IsNil => RowId == 0;

		internal int RowId => _rowId;

		private ParameterHandle(int rowId)
		{
			_rowId = rowId;
		}

		internal static ParameterHandle FromRowId(int rowId)
		{
			return new ParameterHandle(rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator Handle(ParameterHandle handle)
		{
			return new Handle(8, handle._rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator EntityHandle(ParameterHandle handle)
		{
			return new EntityHandle((uint)(134217728L | (long)handle._rowId));
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator ParameterHandle(Handle handle)
		{
			if (handle.VType != 8)
			{
				Throw.InvalidCast();
			}
			return new ParameterHandle(handle.RowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator ParameterHandle(EntityHandle handle)
		{
			if (handle.VType != 134217728)
			{
				Throw.InvalidCast();
			}
			return new ParameterHandle(handle.RowId);
		}

		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator ==(ParameterHandle left, ParameterHandle right)
		{
			return left._rowId == right._rowId;
		}

		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is ParameterHandle)
			{
				return ((ParameterHandle)obj)._rowId == _rowId;
			}
			return false;
		}

		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(ParameterHandle other)
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
		public static bool operator !=(ParameterHandle left, ParameterHandle right)
		{
			return left._rowId != right._rowId;
		}
	}
}
