namespace System.Reflection.Metadata
{
	public readonly struct GuidHandle : IEquatable<GuidHandle>
	{
		private readonly int _index;

		/// <returns></returns>
		public bool IsNil => _index == 0;

		internal int Index => _index;

		private GuidHandle(int index)
		{
			_index = index;
		}

		internal static GuidHandle FromIndex(int heapIndex)
		{
			return new GuidHandle(heapIndex);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator Handle(GuidHandle handle)
		{
			return new Handle(114, handle._index);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator GuidHandle(Handle handle)
		{
			if (handle.VType != 114)
			{
				Throw.InvalidCast();
			}
			return new GuidHandle(handle.Offset);
		}

		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is GuidHandle)
			{
				return Equals((GuidHandle)obj);
			}
			return false;
		}

		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(GuidHandle other)
		{
			return _index == other._index;
		}

		/// <returns></returns>
		public override int GetHashCode()
		{
			return _index;
		}

		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator ==(GuidHandle left, GuidHandle right)
		{
			return left.Equals(right);
		}

		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator !=(GuidHandle left, GuidHandle right)
		{
			return !left.Equals(right);
		}
	}
}
