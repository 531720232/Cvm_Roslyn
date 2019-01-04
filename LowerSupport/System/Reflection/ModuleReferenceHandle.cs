namespace System.Reflection.Metadata
{
	public readonly struct ModuleReferenceHandle : IEquatable<ModuleReferenceHandle>
	{
		private const uint tokenType = 436207616u;

		private const byte tokenTypeSmall = 26;

		private readonly int _rowId;

		/// <returns></returns>
		public bool IsNil => RowId == 0;

		internal int RowId => _rowId;

		private ModuleReferenceHandle(int rowId)
		{
			_rowId = rowId;
		}

		internal static ModuleReferenceHandle FromRowId(int rowId)
		{
			return new ModuleReferenceHandle(rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator Handle(ModuleReferenceHandle handle)
		{
			return new Handle(26, handle._rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator EntityHandle(ModuleReferenceHandle handle)
		{
			return new EntityHandle((uint)(436207616L | (long)handle._rowId));
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator ModuleReferenceHandle(Handle handle)
		{
			if (handle.VType != 26)
			{
				Throw.InvalidCast();
			}
			return new ModuleReferenceHandle(handle.RowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator ModuleReferenceHandle(EntityHandle handle)
		{
			if (handle.VType != 436207616)
			{
				Throw.InvalidCast();
			}
			return new ModuleReferenceHandle(handle.RowId);
		}

		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator ==(ModuleReferenceHandle left, ModuleReferenceHandle right)
		{
			return left._rowId == right._rowId;
		}

		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is ModuleReferenceHandle)
			{
				return ((ModuleReferenceHandle)obj)._rowId == _rowId;
			}
			return false;
		}

		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(ModuleReferenceHandle other)
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
		public static bool operator !=(ModuleReferenceHandle left, ModuleReferenceHandle right)
		{
			return left._rowId != right._rowId;
		}
	}
}
