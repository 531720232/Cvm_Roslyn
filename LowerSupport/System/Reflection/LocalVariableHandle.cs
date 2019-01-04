namespace System.Reflection.Metadata
{
	public readonly struct LocalVariableHandle : IEquatable<LocalVariableHandle>
	{
		private const uint tokenType = 855638016u;

		private const byte tokenTypeSmall = 51;

		private readonly int _rowId;

		/// <returns></returns>
		public bool IsNil => RowId == 0;

		internal int RowId => _rowId;

		private LocalVariableHandle(int rowId)
		{
			_rowId = rowId;
		}

		internal static LocalVariableHandle FromRowId(int rowId)
		{
			return new LocalVariableHandle(rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator Handle(LocalVariableHandle handle)
		{
			return new Handle(51, handle._rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator EntityHandle(LocalVariableHandle handle)
		{
			return new EntityHandle((uint)(855638016L | (long)handle._rowId));
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator LocalVariableHandle(Handle handle)
		{
			if (handle.VType != 51)
			{
				Throw.InvalidCast();
			}
			return new LocalVariableHandle(handle.RowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator LocalVariableHandle(EntityHandle handle)
		{
			if (handle.VType != 855638016)
			{
				Throw.InvalidCast();
			}
			return new LocalVariableHandle(handle.RowId);
		}

		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator ==(LocalVariableHandle left, LocalVariableHandle right)
		{
			return left._rowId == right._rowId;
		}

		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is LocalVariableHandle)
			{
				return ((LocalVariableHandle)obj)._rowId == _rowId;
			}
			return false;
		}

		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(LocalVariableHandle other)
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
		public static bool operator !=(LocalVariableHandle left, LocalVariableHandle right)
		{
			return left._rowId != right._rowId;
		}
	}
}
