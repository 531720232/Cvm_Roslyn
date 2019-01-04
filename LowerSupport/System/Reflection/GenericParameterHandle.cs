namespace System.Reflection.Metadata
{
	public readonly struct GenericParameterHandle : IEquatable<GenericParameterHandle>
	{
		private const uint tokenType = 704643072u;

		private const byte tokenTypeSmall = 42;

		private readonly int _rowId;

		/// <returns></returns>
		public bool IsNil => RowId == 0;

		internal int RowId => _rowId;

		private GenericParameterHandle(int rowId)
		{
			_rowId = rowId;
		}

		internal static GenericParameterHandle FromRowId(int rowId)
		{
			return new GenericParameterHandle(rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator Handle(GenericParameterHandle handle)
		{
			return new Handle(42, handle._rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator EntityHandle(GenericParameterHandle handle)
		{
			return new EntityHandle((uint)(704643072L | (long)handle._rowId));
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator GenericParameterHandle(Handle handle)
		{
			if (handle.VType != 42)
			{
				Throw.InvalidCast();
			}
			return new GenericParameterHandle(handle.RowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator GenericParameterHandle(EntityHandle handle)
		{
			if (handle.VType != 704643072)
			{
				Throw.InvalidCast();
			}
			return new GenericParameterHandle(handle.RowId);
		}

		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator ==(GenericParameterHandle left, GenericParameterHandle right)
		{
			return left._rowId == right._rowId;
		}

		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is GenericParameterHandle)
			{
				return ((GenericParameterHandle)obj)._rowId == _rowId;
			}
			return false;
		}

		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(GenericParameterHandle other)
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
		public static bool operator !=(GenericParameterHandle left, GenericParameterHandle right)
		{
			return left._rowId != right._rowId;
		}
	}
}
