namespace System.Reflection.Metadata
{
	public readonly struct ExportedTypeHandle : IEquatable<ExportedTypeHandle>
	{
		private const uint tokenType = 654311424u;

		private const byte tokenTypeSmall = 39;

		private readonly int _rowId;

		/// <returns></returns>
		public bool IsNil => RowId == 0;

		internal int RowId => _rowId;

		private ExportedTypeHandle(int rowId)
		{
			_rowId = rowId;
		}

		internal static ExportedTypeHandle FromRowId(int rowId)
		{
			return new ExportedTypeHandle(rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator Handle(ExportedTypeHandle handle)
		{
			return new Handle(39, handle._rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator EntityHandle(ExportedTypeHandle handle)
		{
			return new EntityHandle((uint)(654311424L | (long)handle._rowId));
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator ExportedTypeHandle(Handle handle)
		{
			if (handle.VType != 39)
			{
				Throw.InvalidCast();
			}
			return new ExportedTypeHandle(handle.RowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator ExportedTypeHandle(EntityHandle handle)
		{
			if (handle.VType != 654311424)
			{
				Throw.InvalidCast();
			}
			return new ExportedTypeHandle(handle.RowId);
		}

		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator ==(ExportedTypeHandle left, ExportedTypeHandle right)
		{
			return left._rowId == right._rowId;
		}

		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is ExportedTypeHandle)
			{
				return ((ExportedTypeHandle)obj)._rowId == _rowId;
			}
			return false;
		}

		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(ExportedTypeHandle other)
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
		public static bool operator !=(ExportedTypeHandle left, ExportedTypeHandle right)
		{
			return left._rowId != right._rowId;
		}
	}
}
