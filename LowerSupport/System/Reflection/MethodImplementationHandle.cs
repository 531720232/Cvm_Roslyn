namespace System.Reflection.Metadata
{
	public readonly struct MethodImplementationHandle : IEquatable<MethodImplementationHandle>
	{
		private const uint tokenType = 419430400u;

		private const byte tokenTypeSmall = 25;

		private readonly int _rowId;

		/// <returns></returns>
		public bool IsNil => RowId == 0;

		internal int RowId => _rowId;

		private MethodImplementationHandle(int rowId)
		{
			_rowId = rowId;
		}

		internal static MethodImplementationHandle FromRowId(int rowId)
		{
			return new MethodImplementationHandle(rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator Handle(MethodImplementationHandle handle)
		{
			return new Handle(25, handle._rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator EntityHandle(MethodImplementationHandle handle)
		{
			return new EntityHandle((uint)(419430400L | (long)handle._rowId));
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator MethodImplementationHandle(Handle handle)
		{
			if (handle.VType != 25)
			{
				Throw.InvalidCast();
			}
			return new MethodImplementationHandle(handle.RowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator MethodImplementationHandle(EntityHandle handle)
		{
			if (handle.VType != 419430400)
			{
				Throw.InvalidCast();
			}
			return new MethodImplementationHandle(handle.RowId);
		}

		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator ==(MethodImplementationHandle left, MethodImplementationHandle right)
		{
			return left._rowId == right._rowId;
		}

		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is MethodImplementationHandle)
			{
				return ((MethodImplementationHandle)obj)._rowId == _rowId;
			}
			return false;
		}

		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(MethodImplementationHandle other)
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
		public static bool operator !=(MethodImplementationHandle left, MethodImplementationHandle right)
		{
			return left._rowId != right._rowId;
		}
	}
}
