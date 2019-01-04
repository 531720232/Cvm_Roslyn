namespace System.Reflection.Metadata
{
	public readonly struct UserStringHandle : IEquatable<UserStringHandle>
	{
		private readonly int _offset;

		/// <returns></returns>
		public bool IsNil => _offset == 0;

		private UserStringHandle(int offset)
		{
			_offset = offset;
		}

		internal static UserStringHandle FromOffset(int heapOffset)
		{
			return new UserStringHandle(heapOffset);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator Handle(UserStringHandle handle)
		{
			return new Handle(112, handle._offset);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator UserStringHandle(Handle handle)
		{
			if (handle.VType != 112)
			{
				Throw.InvalidCast();
			}
			return new UserStringHandle(handle.Offset);
		}

		internal int GetHeapOffset()
		{
			return _offset;
		}

		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator ==(UserStringHandle left, UserStringHandle right)
		{
			return left._offset == right._offset;
		}

		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is UserStringHandle)
			{
				return ((UserStringHandle)obj)._offset == _offset;
			}
			return false;
		}

		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(UserStringHandle other)
		{
			return _offset == other._offset;
		}

		/// <returns></returns>
		public override int GetHashCode()
		{
			return _offset.GetHashCode();
		}

		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator !=(UserStringHandle left, UserStringHandle right)
		{
			return left._offset != right._offset;
		}
	}
}
