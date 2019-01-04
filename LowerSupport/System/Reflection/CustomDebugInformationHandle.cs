namespace System.Reflection.Metadata
{
	public readonly struct CustomDebugInformationHandle : IEquatable<CustomDebugInformationHandle>
	{
		private const uint tokenType = 922746880u;

		private const byte tokenTypeSmall = 55;

		private readonly int _rowId;

		/// <returns></returns>
		public bool IsNil => RowId == 0;

		internal int RowId => _rowId;

		private CustomDebugInformationHandle(int rowId)
		{
			_rowId = rowId;
		}

		internal static CustomDebugInformationHandle FromRowId(int rowId)
		{
			return new CustomDebugInformationHandle(rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator Handle(CustomDebugInformationHandle handle)
		{
			return new Handle(55, handle._rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator EntityHandle(CustomDebugInformationHandle handle)
		{
			return new EntityHandle((uint)(922746880L | (long)handle._rowId));
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator CustomDebugInformationHandle(Handle handle)
		{
			if (handle.VType != 55)
			{
				Throw.InvalidCast();
			}
			return new CustomDebugInformationHandle(handle.RowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator CustomDebugInformationHandle(EntityHandle handle)
		{
			if (handle.VType != 922746880)
			{
				Throw.InvalidCast();
			}
			return new CustomDebugInformationHandle(handle.RowId);
		}

		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator ==(CustomDebugInformationHandle left, CustomDebugInformationHandle right)
		{
			return left._rowId == right._rowId;
		}

		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is CustomDebugInformationHandle)
			{
				return ((CustomDebugInformationHandle)obj)._rowId == _rowId;
			}
			return false;
		}

		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(CustomDebugInformationHandle other)
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
		public static bool operator !=(CustomDebugInformationHandle left, CustomDebugInformationHandle right)
		{
			return left._rowId != right._rowId;
		}
	}
}
