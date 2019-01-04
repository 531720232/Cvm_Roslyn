namespace System.Reflection.Metadata
{
	public readonly struct AssemblyDefinitionHandle : IEquatable<AssemblyDefinitionHandle>
	{
		private const uint tokenType = 536870912u;

		private const byte tokenTypeSmall = 32;

		private readonly int _rowId;

		/// <returns></returns>
		public bool IsNil => RowId == 0;

		internal int RowId => _rowId;

		internal AssemblyDefinitionHandle(int rowId)
		{
			_rowId = rowId;
		}

		internal static AssemblyDefinitionHandle FromRowId(int rowId)
		{
			return new AssemblyDefinitionHandle(rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator Handle(AssemblyDefinitionHandle handle)
		{
			return new Handle(32, handle._rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator EntityHandle(AssemblyDefinitionHandle handle)
		{
			return new EntityHandle((uint)(536870912L | (long)handle._rowId));
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator AssemblyDefinitionHandle(Handle handle)
		{
			if (handle.VType != 32)
			{
				Throw.InvalidCast();
			}
			return new AssemblyDefinitionHandle(handle.RowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator AssemblyDefinitionHandle(EntityHandle handle)
		{
			if (handle.VType != 536870912)
			{
				Throw.InvalidCast();
			}
			return new AssemblyDefinitionHandle(handle.RowId);
		}

		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator ==(AssemblyDefinitionHandle left, AssemblyDefinitionHandle right)
		{
			return left._rowId == right._rowId;
		}

		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is AssemblyDefinitionHandle)
			{
				return ((AssemblyDefinitionHandle)obj)._rowId == _rowId;
			}
			return false;
		}

		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(AssemblyDefinitionHandle other)
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
		public static bool operator !=(AssemblyDefinitionHandle left, AssemblyDefinitionHandle right)
		{
			return left._rowId != right._rowId;
		}
	}
}
