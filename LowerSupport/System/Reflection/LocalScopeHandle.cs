namespace System.Reflection.Metadata
{
	public readonly struct LocalScopeHandle : IEquatable<LocalScopeHandle>
	{
		private const uint tokenType = 838860800u;

		private const byte tokenTypeSmall = 50;

		private readonly int _rowId;

		/// <returns></returns>
		public bool IsNil => RowId == 0;

		internal int RowId => _rowId;

		private LocalScopeHandle(int rowId)
		{
			_rowId = rowId;
		}

		internal static LocalScopeHandle FromRowId(int rowId)
		{
			return new LocalScopeHandle(rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator Handle(LocalScopeHandle handle)
		{
			return new Handle(50, handle._rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator EntityHandle(LocalScopeHandle handle)
		{
			return new EntityHandle((uint)(838860800L | (long)handle._rowId));
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator LocalScopeHandle(Handle handle)
		{
			if (handle.VType != 50)
			{
				Throw.InvalidCast();
			}
			return new LocalScopeHandle(handle.RowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator LocalScopeHandle(EntityHandle handle)
		{
			if (handle.VType != 838860800)
			{
				Throw.InvalidCast();
			}
			return new LocalScopeHandle(handle.RowId);
		}

		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator ==(LocalScopeHandle left, LocalScopeHandle right)
		{
			return left._rowId == right._rowId;
		}

		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is LocalScopeHandle)
			{
				return ((LocalScopeHandle)obj)._rowId == _rowId;
			}
			return false;
		}

		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(LocalScopeHandle other)
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
		public static bool operator !=(LocalScopeHandle left, LocalScopeHandle right)
		{
			return left._rowId != right._rowId;
		}
	}
}
