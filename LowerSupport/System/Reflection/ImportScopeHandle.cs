namespace System.Reflection.Metadata
{
	public readonly struct ImportScopeHandle : IEquatable<ImportScopeHandle>
	{
		private const uint tokenType = 889192448u;

		private const byte tokenTypeSmall = 53;

		private readonly int _rowId;

		/// <returns></returns>
		public bool IsNil => RowId == 0;

		internal int RowId => _rowId;

		private ImportScopeHandle(int rowId)
		{
			_rowId = rowId;
		}

		internal static ImportScopeHandle FromRowId(int rowId)
		{
			return new ImportScopeHandle(rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator Handle(ImportScopeHandle handle)
		{
			return new Handle(53, handle._rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator EntityHandle(ImportScopeHandle handle)
		{
			return new EntityHandle((uint)(889192448L | (long)handle._rowId));
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator ImportScopeHandle(Handle handle)
		{
			if (handle.VType != 53)
			{
				Throw.InvalidCast();
			}
			return new ImportScopeHandle(handle.RowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator ImportScopeHandle(EntityHandle handle)
		{
			if (handle.VType != 889192448)
			{
				Throw.InvalidCast();
			}
			return new ImportScopeHandle(handle.RowId);
		}

		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator ==(ImportScopeHandle left, ImportScopeHandle right)
		{
			return left._rowId == right._rowId;
		}

		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is ImportScopeHandle)
			{
				return ((ImportScopeHandle)obj)._rowId == _rowId;
			}
			return false;
		}

		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(ImportScopeHandle other)
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
		public static bool operator !=(ImportScopeHandle left, ImportScopeHandle right)
		{
			return left._rowId != right._rowId;
		}
	}
}
