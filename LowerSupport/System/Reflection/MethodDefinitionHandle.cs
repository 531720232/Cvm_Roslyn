namespace System.Reflection.Metadata
{
	public readonly struct MethodDefinitionHandle : IEquatable<MethodDefinitionHandle>
	{
		private const uint tokenType = 100663296u;

		private const byte tokenTypeSmall = 6;

		private readonly int _rowId;

		/// <returns></returns>
		public bool IsNil => RowId == 0;

		internal int RowId => _rowId;

		private MethodDefinitionHandle(int rowId)
		{
			_rowId = rowId;
		}

		internal static MethodDefinitionHandle FromRowId(int rowId)
		{
			return new MethodDefinitionHandle(rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator Handle(MethodDefinitionHandle handle)
		{
			return new Handle(6, handle._rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator EntityHandle(MethodDefinitionHandle handle)
		{
			return new EntityHandle((uint)(100663296L | (long)handle._rowId));
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator MethodDefinitionHandle(Handle handle)
		{
			if (handle.VType != 6)
			{
				Throw.InvalidCast();
			}
			return new MethodDefinitionHandle(handle.RowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator MethodDefinitionHandle(EntityHandle handle)
		{
			if (handle.VType != 100663296)
			{
				Throw.InvalidCast();
			}
			return new MethodDefinitionHandle(handle.RowId);
		}

		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator ==(MethodDefinitionHandle left, MethodDefinitionHandle right)
		{
			return left._rowId == right._rowId;
		}

		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is MethodDefinitionHandle)
			{
				return ((MethodDefinitionHandle)obj)._rowId == _rowId;
			}
			return false;
		}

		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(MethodDefinitionHandle other)
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
		public static bool operator !=(MethodDefinitionHandle left, MethodDefinitionHandle right)
		{
			return left._rowId != right._rowId;
		}

		/// <returns></returns>
		public MethodDebugInformationHandle ToDebugInformationHandle()
		{
			return MethodDebugInformationHandle.FromRowId(_rowId);
		}
	}
}
