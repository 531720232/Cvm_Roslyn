namespace System.Reflection.Metadata
{
	public readonly struct AssemblyFileHandle : IEquatable<AssemblyFileHandle>
	{
		private const uint tokenType = 637534208u;

		private const byte tokenTypeSmall = 38;

		private readonly int _rowId;

		/// <returns></returns>
		public bool IsNil => RowId == 0;

		internal int RowId => _rowId;

		private AssemblyFileHandle(int rowId)
		{
			_rowId = rowId;
		}

		internal static AssemblyFileHandle FromRowId(int rowId)
		{
			return new AssemblyFileHandle(rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator Handle(AssemblyFileHandle handle)
		{
			return new Handle(38, handle._rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator EntityHandle(AssemblyFileHandle handle)
		{
			return new EntityHandle((uint)(637534208L | (long)handle._rowId));
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator AssemblyFileHandle(Handle handle)
		{
			if (handle.VType != 38)
			{
				Throw.InvalidCast();
			}
			return new AssemblyFileHandle(handle.RowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator AssemblyFileHandle(EntityHandle handle)
		{
			if (handle.VType != 637534208)
			{
				Throw.InvalidCast();
			}
			return new AssemblyFileHandle(handle.RowId);
		}

		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator ==(AssemblyFileHandle left, AssemblyFileHandle right)
		{
			return left._rowId == right._rowId;
		}

		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is AssemblyFileHandle)
			{
				return ((AssemblyFileHandle)obj)._rowId == _rowId;
			}
			return false;
		}

		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(AssemblyFileHandle other)
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
		public static bool operator !=(AssemblyFileHandle left, AssemblyFileHandle right)
		{
			return left._rowId != right._rowId;
		}
	}
}
