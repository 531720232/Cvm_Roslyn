namespace System.Reflection.Metadata
{
	public readonly struct EventDefinitionHandle : IEquatable<EventDefinitionHandle>
	{
		private const uint tokenType = 335544320u;

		private const byte tokenTypeSmall = 20;

		private readonly int _rowId;

		/// <returns></returns>
		public bool IsNil => RowId == 0;

		internal int RowId => _rowId;

		private EventDefinitionHandle(int rowId)
		{
			_rowId = rowId;
		}

		internal static EventDefinitionHandle FromRowId(int rowId)
		{
			return new EventDefinitionHandle(rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator Handle(EventDefinitionHandle handle)
		{
			return new Handle(20, handle._rowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator EntityHandle(EventDefinitionHandle handle)
		{
			return new EntityHandle((uint)(335544320L | (long)handle._rowId));
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator EventDefinitionHandle(Handle handle)
		{
			if (handle.VType != 20)
			{
				Throw.InvalidCast();
			}
			return new EventDefinitionHandle(handle.RowId);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator EventDefinitionHandle(EntityHandle handle)
		{
			if (handle.VType != 335544320)
			{
				Throw.InvalidCast();
			}
			return new EventDefinitionHandle(handle.RowId);
		}

		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator ==(EventDefinitionHandle left, EventDefinitionHandle right)
		{
			return left._rowId == right._rowId;
		}

		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is EventDefinitionHandle)
			{
				return ((EventDefinitionHandle)obj)._rowId == _rowId;
			}
			return false;
		}

		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(EventDefinitionHandle other)
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
		public static bool operator !=(EventDefinitionHandle left, EventDefinitionHandle right)
		{
			return left._rowId != right._rowId;
		}
	}
}
