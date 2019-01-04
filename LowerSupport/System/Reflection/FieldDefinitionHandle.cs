namespace System.Reflection.Metadata
{
	public readonly struct FieldDefinitionHandle : IEquatable<FieldDefinitionHandle>
	{
		private const uint tokenType = 67108864u;

		private const byte tokenTypeSmall = 4;

		private readonly int _rowId;

		/// <returns></returns>
		public bool IsNil => RowId == 0;

		internal int RowId => _rowId;

		private FieldDefinitionHandle(int rowId)
		{
			_rowId = rowId;
		}

		internal static FieldDefinitionHandle FromRowId(int rowId)
		{
			return new FieldDefinitionHandle(rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator Handle(FieldDefinitionHandle handle)
		{
			return new Handle(4, handle._rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator EntityHandle(FieldDefinitionHandle handle)
		{
			return new EntityHandle((uint)(67108864L | (long)handle._rowId));
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator FieldDefinitionHandle(Handle handle)
		{
			if (handle.VType != 4)
			{
				Throw.InvalidCast();
			}
			return new FieldDefinitionHandle(handle.RowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator FieldDefinitionHandle(EntityHandle handle)
		{
			if (handle.VType != 67108864)
			{
				Throw.InvalidCast();
			}
			return new FieldDefinitionHandle(handle.RowId);
		}

		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator ==(FieldDefinitionHandle left, FieldDefinitionHandle right)
		{
			return left._rowId == right._rowId;
		}

		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is FieldDefinitionHandle)
			{
				return ((FieldDefinitionHandle)obj)._rowId == _rowId;
			}
			return false;
		}

		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(FieldDefinitionHandle other)
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
		public static bool operator !=(FieldDefinitionHandle left, FieldDefinitionHandle right)
		{
			return left._rowId != right._rowId;
		}
	}
}
