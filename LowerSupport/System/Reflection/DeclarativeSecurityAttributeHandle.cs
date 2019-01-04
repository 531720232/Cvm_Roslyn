namespace System.Reflection.Metadata
{
	public readonly struct DeclarativeSecurityAttributeHandle : IEquatable<DeclarativeSecurityAttributeHandle>
	{
		private const uint tokenType = 234881024u;

		private const byte tokenTypeSmall = 14;

		private readonly int _rowId;

		/// <returns></returns>
		public bool IsNil => _rowId == 0;

		internal int RowId => _rowId;

		private DeclarativeSecurityAttributeHandle(int rowId)
		{
			_rowId = rowId;
		}

		internal static DeclarativeSecurityAttributeHandle FromRowId(int rowId)
		{
			return new DeclarativeSecurityAttributeHandle(rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator Handle(DeclarativeSecurityAttributeHandle handle)
		{
			return new Handle(14, handle._rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator EntityHandle(DeclarativeSecurityAttributeHandle handle)
		{
			return new EntityHandle((uint)(234881024L | (long)handle._rowId));
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator DeclarativeSecurityAttributeHandle(Handle handle)
		{
			if (handle.VType != 14)
			{
				Throw.InvalidCast();
			}
			return new DeclarativeSecurityAttributeHandle(handle.RowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator DeclarativeSecurityAttributeHandle(EntityHandle handle)
		{
			if (handle.VType != 234881024)
			{
				Throw.InvalidCast();
			}
			return new DeclarativeSecurityAttributeHandle(handle.RowId);
		}

		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator ==(DeclarativeSecurityAttributeHandle left, DeclarativeSecurityAttributeHandle right)
		{
			return left._rowId == right._rowId;
		}

		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is DeclarativeSecurityAttributeHandle)
			{
				return ((DeclarativeSecurityAttributeHandle)obj)._rowId == _rowId;
			}
			return false;
		}

		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(DeclarativeSecurityAttributeHandle other)
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
		public static bool operator !=(DeclarativeSecurityAttributeHandle left, DeclarativeSecurityAttributeHandle right)
		{
			return left._rowId != right._rowId;
		}
	}
}
