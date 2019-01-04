namespace System.Reflection.Metadata
{
	public readonly struct ModuleDefinitionHandle : IEquatable<ModuleDefinitionHandle>
	{
		private const uint tokenType = 0u;

		private const byte tokenTypeSmall = 0;

		private readonly int _rowId;

		/// <returns></returns>
		public bool IsNil => RowId == 0;

		internal int RowId => _rowId;

		internal ModuleDefinitionHandle(int rowId)
		{
			_rowId = rowId;
		}

		internal static ModuleDefinitionHandle FromRowId(int rowId)
		{
			return new ModuleDefinitionHandle(rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator Handle(ModuleDefinitionHandle handle)
		{
			return new Handle(0, handle._rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator EntityHandle(ModuleDefinitionHandle handle)
		{
			return new EntityHandle((uint)(0L | (long)handle._rowId));
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator ModuleDefinitionHandle(Handle handle)
		{
			if (handle.VType != 0)
			{
				Throw.InvalidCast();
			}
			return new ModuleDefinitionHandle(handle.RowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator ModuleDefinitionHandle(EntityHandle handle)
		{
			if (handle.VType != 0)
			{
				Throw.InvalidCast();
			}
			return new ModuleDefinitionHandle(handle.RowId);
		}

		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator ==(ModuleDefinitionHandle left, ModuleDefinitionHandle right)
		{
			return left._rowId == right._rowId;
		}

		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is ModuleDefinitionHandle)
			{
				return ((ModuleDefinitionHandle)obj)._rowId == _rowId;
			}
			return false;
		}

		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(ModuleDefinitionHandle other)
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
		public static bool operator !=(ModuleDefinitionHandle left, ModuleDefinitionHandle right)
		{
			return left._rowId != right._rowId;
		}
	}
}
