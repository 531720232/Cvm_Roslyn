namespace System.Reflection.Metadata
{
	public readonly struct LocalConstantHandle : IEquatable<LocalConstantHandle>
	{
		private const uint tokenType = 872415232u;

		private const byte tokenTypeSmall = 52;

		private readonly int _rowId;

		/// <returns></returns>
		public bool IsNil => RowId == 0;

		internal int RowId => _rowId;

		private LocalConstantHandle(int rowId)
		{
			_rowId = rowId;
		}

		internal static LocalConstantHandle FromRowId(int rowId)
		{
			return new LocalConstantHandle(rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator Handle(LocalConstantHandle handle)
		{
			return new Handle(52, handle._rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator EntityHandle(LocalConstantHandle handle)
		{
			return new EntityHandle((uint)(872415232L | (long)handle._rowId));
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator LocalConstantHandle(Handle handle)
		{
			if (handle.VType != 52)
			{
				Throw.InvalidCast();
			}
			return new LocalConstantHandle(handle.RowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator LocalConstantHandle(EntityHandle handle)
		{
			if (handle.VType != 872415232)
			{
				Throw.InvalidCast();
			}
			return new LocalConstantHandle(handle.RowId);
		}

		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator ==(LocalConstantHandle left, LocalConstantHandle right)
		{
			return left._rowId == right._rowId;
		}

		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is LocalConstantHandle)
			{
				return ((LocalConstantHandle)obj)._rowId == _rowId;
			}
			return false;
		}

		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(LocalConstantHandle other)
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
		public static bool operator !=(LocalConstantHandle left, LocalConstantHandle right)
		{
			return left._rowId != right._rowId;
		}
	}
}
