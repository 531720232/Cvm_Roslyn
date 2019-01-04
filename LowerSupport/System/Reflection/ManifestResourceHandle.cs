namespace System.Reflection.Metadata
{
	public readonly struct ManifestResourceHandle : IEquatable<ManifestResourceHandle>
	{
		private const uint tokenType = 671088640u;

		private const byte tokenTypeSmall = 40;

		private readonly int _rowId;

		/// <returns></returns>
		public bool IsNil => RowId == 0;

		internal int RowId => _rowId;

		private ManifestResourceHandle(int rowId)
		{
			_rowId = rowId;
		}

		internal static ManifestResourceHandle FromRowId(int rowId)
		{
			return new ManifestResourceHandle(rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator Handle(ManifestResourceHandle handle)
		{
			return new Handle(40, handle._rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator EntityHandle(ManifestResourceHandle handle)
		{
			return new EntityHandle((uint)(671088640L | (long)handle._rowId));
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator ManifestResourceHandle(Handle handle)
		{
			if (handle.VType != 40)
			{
				Throw.InvalidCast();
			}
			return new ManifestResourceHandle(handle.RowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator ManifestResourceHandle(EntityHandle handle)
		{
			if (handle.VType != 671088640)
			{
				Throw.InvalidCast();
			}
			return new ManifestResourceHandle(handle.RowId);
		}

		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator ==(ManifestResourceHandle left, ManifestResourceHandle right)
		{
			return left._rowId == right._rowId;
		}

		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is ManifestResourceHandle)
			{
				return ((ManifestResourceHandle)obj)._rowId == _rowId;
			}
			return false;
		}

		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(ManifestResourceHandle other)
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
		public static bool operator !=(ManifestResourceHandle left, ManifestResourceHandle right)
		{
			return left._rowId != right._rowId;
		}
	}
}
