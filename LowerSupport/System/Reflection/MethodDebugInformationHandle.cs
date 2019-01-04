namespace System.Reflection.Metadata
{
	public readonly struct MethodDebugInformationHandle : IEquatable<MethodDebugInformationHandle>
	{
		private const uint tokenType = 822083584u;

		private const byte tokenTypeSmall = 49;

		private readonly int _rowId;

		/// <returns></returns>
		public bool IsNil => RowId == 0;

		internal int RowId => _rowId;

		private MethodDebugInformationHandle(int rowId)
		{
			_rowId = rowId;
		}

		internal static MethodDebugInformationHandle FromRowId(int rowId)
		{
			return new MethodDebugInformationHandle(rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator Handle(MethodDebugInformationHandle handle)
		{
			return new Handle(49, handle._rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator EntityHandle(MethodDebugInformationHandle handle)
		{
			return new EntityHandle((uint)(822083584L | (long)handle._rowId));
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator MethodDebugInformationHandle(Handle handle)
		{
			if (handle.VType != 49)
			{
				Throw.InvalidCast();
			}
			return new MethodDebugInformationHandle(handle.RowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator MethodDebugInformationHandle(EntityHandle handle)
		{
			if (handle.VType != 822083584)
			{
				Throw.InvalidCast();
			}
			return new MethodDebugInformationHandle(handle.RowId);
		}

		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator ==(MethodDebugInformationHandle left, MethodDebugInformationHandle right)
		{
			return left._rowId == right._rowId;
		}

		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is MethodDebugInformationHandle)
			{
				return ((MethodDebugInformationHandle)obj)._rowId == _rowId;
			}
			return false;
		}

		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(MethodDebugInformationHandle other)
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
		public static bool operator !=(MethodDebugInformationHandle left, MethodDebugInformationHandle right)
		{
			return left._rowId != right._rowId;
		}

		/// <returns></returns>
		public MethodDefinitionHandle ToDefinitionHandle()
		{
			return MethodDefinitionHandle.FromRowId(_rowId);
		}
	}
}
