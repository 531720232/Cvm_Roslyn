namespace System.Reflection.Metadata
{
	public readonly struct PropertyDefinitionHandle : IEquatable<PropertyDefinitionHandle>
	{
		private const uint tokenType = 385875968u;

		private const byte tokenTypeSmall = 23;

		private readonly int _rowId;

		/// <returns></returns>
		public bool IsNil => RowId == 0;

		internal int RowId => _rowId;

		private PropertyDefinitionHandle(int rowId)
		{
			_rowId = rowId;
		}

		internal static PropertyDefinitionHandle FromRowId(int rowId)
		{
			return new PropertyDefinitionHandle(rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator Handle(PropertyDefinitionHandle handle)
		{
			return new Handle(23, handle._rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator EntityHandle(PropertyDefinitionHandle handle)
		{
			return new EntityHandle((uint)(385875968L | (long)handle._rowId));
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator PropertyDefinitionHandle(Handle handle)
		{
			if (handle.VType != 23)
			{
				Throw.InvalidCast();
			}
			return new PropertyDefinitionHandle(handle.RowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator PropertyDefinitionHandle(EntityHandle handle)
		{
			if (handle.VType != 385875968)
			{
				Throw.InvalidCast();
			}
			return new PropertyDefinitionHandle(handle.RowId);
		}

		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator ==(PropertyDefinitionHandle left, PropertyDefinitionHandle right)
		{
			return left._rowId == right._rowId;
		}

		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is PropertyDefinitionHandle)
			{
				return ((PropertyDefinitionHandle)obj)._rowId == _rowId;
			}
			return false;
		}

		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(PropertyDefinitionHandle other)
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
		public static bool operator !=(PropertyDefinitionHandle left, PropertyDefinitionHandle right)
		{
			return left._rowId != right._rowId;
		}
	}
}
