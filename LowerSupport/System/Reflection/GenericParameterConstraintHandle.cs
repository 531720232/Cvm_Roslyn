namespace System.Reflection.Metadata
{
	public readonly struct GenericParameterConstraintHandle : IEquatable<GenericParameterConstraintHandle>
	{
		private const uint tokenType = 738197504u;

		private const byte tokenTypeSmall = 44;

		private readonly int _rowId;

		/// <returns></returns>
		public bool IsNil => RowId == 0;

		internal int RowId => _rowId;

		private GenericParameterConstraintHandle(int rowId)
		{
			_rowId = rowId;
		}

		internal static GenericParameterConstraintHandle FromRowId(int rowId)
		{
			return new GenericParameterConstraintHandle(rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator Handle(GenericParameterConstraintHandle handle)
		{
			return new Handle(44, handle._rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator EntityHandle(GenericParameterConstraintHandle handle)
		{
			return new EntityHandle((uint)(738197504L | (long)handle._rowId));
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator GenericParameterConstraintHandle(Handle handle)
		{
			if (handle.VType != 44)
			{
				Throw.InvalidCast();
			}
			return new GenericParameterConstraintHandle(handle.RowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator GenericParameterConstraintHandle(EntityHandle handle)
		{
			if (handle.VType != 738197504)
			{
				Throw.InvalidCast();
			}
			return new GenericParameterConstraintHandle(handle.RowId);
		}

		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator ==(GenericParameterConstraintHandle left, GenericParameterConstraintHandle right)
		{
			return left._rowId == right._rowId;
		}

		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is GenericParameterConstraintHandle)
			{
				return ((GenericParameterConstraintHandle)obj)._rowId == _rowId;
			}
			return false;
		}

		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(GenericParameterConstraintHandle other)
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
		public static bool operator !=(GenericParameterConstraintHandle left, GenericParameterConstraintHandle right)
		{
			return left._rowId != right._rowId;
		}
	}
}
