namespace System.Reflection.Metadata
{
	public readonly struct MemberReferenceHandle : IEquatable<MemberReferenceHandle>
	{
		private const uint tokenType = 167772160u;

		private const byte tokenTypeSmall = 10;

		private readonly int _rowId;

		/// <returns></returns>
		public bool IsNil => RowId == 0;

		internal int RowId => _rowId;

		private MemberReferenceHandle(int rowId)
		{
			_rowId = rowId;
		}

		internal static MemberReferenceHandle FromRowId(int rowId)
		{
			return new MemberReferenceHandle(rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator Handle(MemberReferenceHandle handle)
		{
			return new Handle(10, handle._rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator EntityHandle(MemberReferenceHandle handle)
		{
			return new EntityHandle((uint)(167772160L | (long)handle._rowId));
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator MemberReferenceHandle(Handle handle)
		{
			if (handle.VType != 10)
			{
				Throw.InvalidCast();
			}
			return new MemberReferenceHandle(handle.RowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator MemberReferenceHandle(EntityHandle handle)
		{
			if (handle.VType != 167772160)
			{
				Throw.InvalidCast();
			}
			return new MemberReferenceHandle(handle.RowId);
		}

		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator ==(MemberReferenceHandle left, MemberReferenceHandle right)
		{
			return left._rowId == right._rowId;
		}

		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is MemberReferenceHandle)
			{
				return ((MemberReferenceHandle)obj)._rowId == _rowId;
			}
			return false;
		}

		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(MemberReferenceHandle other)
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
		public static bool operator !=(MemberReferenceHandle left, MemberReferenceHandle right)
		{
			return left._rowId != right._rowId;
		}
	}
}
