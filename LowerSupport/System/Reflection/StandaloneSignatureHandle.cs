namespace System.Reflection.Metadata
{
	public readonly struct StandaloneSignatureHandle : IEquatable<StandaloneSignatureHandle>
	{
		private const uint tokenType = 285212672u;

		private const byte tokenTypeSmall = 17;

		private readonly int _rowId;

		/// <returns></returns>
		public bool IsNil => RowId == 0;

		internal int RowId => _rowId;

		private StandaloneSignatureHandle(int rowId)
		{
			_rowId = rowId;
		}

		internal static StandaloneSignatureHandle FromRowId(int rowId)
		{
			return new StandaloneSignatureHandle(rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator Handle(StandaloneSignatureHandle handle)
		{
			return new Handle(17, handle._rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator EntityHandle(StandaloneSignatureHandle handle)
		{
			return new EntityHandle((uint)(285212672L | (long)handle._rowId));
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator StandaloneSignatureHandle(Handle handle)
		{
			if (handle.VType != 17)
			{
				Throw.InvalidCast();
			}
			return new StandaloneSignatureHandle(handle.RowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator StandaloneSignatureHandle(EntityHandle handle)
		{
			if (handle.VType != 285212672)
			{
				Throw.InvalidCast();
			}
			return new StandaloneSignatureHandle(handle.RowId);
		}

		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator ==(StandaloneSignatureHandle left, StandaloneSignatureHandle right)
		{
			return left._rowId == right._rowId;
		}

		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is StandaloneSignatureHandle)
			{
				return ((StandaloneSignatureHandle)obj)._rowId == _rowId;
			}
			return false;
		}

		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(StandaloneSignatureHandle other)
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
		public static bool operator !=(StandaloneSignatureHandle left, StandaloneSignatureHandle right)
		{
			return left._rowId != right._rowId;
		}
	}
}
