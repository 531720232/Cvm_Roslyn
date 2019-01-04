namespace System.Reflection.Metadata
{
	public readonly struct DocumentHandle : IEquatable<DocumentHandle>
	{
		private const uint tokenType = 805306368u;

		private const byte tokenTypeSmall = 48;

		private readonly int _rowId;

		/// <returns></returns>
		public bool IsNil => RowId == 0;

		internal int RowId => _rowId;

		private DocumentHandle(int rowId)
		{
			_rowId = rowId;
		}

		internal static DocumentHandle FromRowId(int rowId)
		{
			return new DocumentHandle(rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator Handle(DocumentHandle handle)
		{
			return new Handle(48, handle._rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator EntityHandle(DocumentHandle handle)
		{
			return new EntityHandle((uint)(805306368L | (long)handle._rowId));
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator DocumentHandle(Handle handle)
		{
			if (handle.VType != 48)
			{
				Throw.InvalidCast();
			}
			return new DocumentHandle(handle.RowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator DocumentHandle(EntityHandle handle)
		{
			if (handle.VType != 805306368)
			{
				Throw.InvalidCast();
			}
			return new DocumentHandle(handle.RowId);
		}

		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator ==(DocumentHandle left, DocumentHandle right)
		{
			return left._rowId == right._rowId;
		}

		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is DocumentHandle)
			{
				return ((DocumentHandle)obj)._rowId == _rowId;
			}
			return false;
		}

		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(DocumentHandle other)
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
		public static bool operator !=(DocumentHandle left, DocumentHandle right)
		{
			return left._rowId != right._rowId;
		}
	}
}
