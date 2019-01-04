namespace System.Reflection.Metadata
{
	public readonly struct BlobHandle : IEquatable<BlobHandle>
	{
		internal enum VirtualIndex : byte
		{
			Nil,
			ContractPublicKeyToken,
			ContractPublicKey,
			AttributeUsage_AllowSingle,
			AttributeUsage_AllowMultiple,
			Count
		}

		private readonly uint _value;

		internal const int TemplateParameterOffset_AttributeUsageTarget = 2;

		internal uint RawValue => _value;

		/// <returns></returns>
		public bool IsNil => _value == 0;

		internal bool IsVirtual => ((int)_value & -2147483648) != 0;

		private ushort VirtualValue => (ushort)(_value >> 8);

		private BlobHandle(uint value)
		{
			_value = value;
		}

		internal static BlobHandle FromOffset(int heapOffset)
		{
			return new BlobHandle((uint)heapOffset);
		}

		internal static BlobHandle FromVirtualIndex(VirtualIndex virtualIndex, ushort virtualValue)
		{
			return new BlobHandle((uint)(-2147483648 | (virtualValue << 8) | (int)virtualIndex));
		}

		internal unsafe void SubstituteTemplateParameters(byte[] blob)
		{
			fixed (byte* ptr = &blob[2])
			{
				*(int*)ptr = VirtualValue;
			}
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator Handle(BlobHandle handle)
		{
			return new Handle((byte)(((uint)((int)handle._value & -2147483648) >> 24) | 0x71), (int)(handle._value & 0x1FFFFFFF));
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator BlobHandle(Handle handle)
		{
			if ((handle.VType & 0x7F) != 113)
			{
				Throw.InvalidCast();
			}
			return new BlobHandle((uint)(((handle.VType & 0x80) << 24) | handle.Offset));
		}

		internal int GetHeapOffset()
		{
			return (int)_value;
		}

		internal VirtualIndex GetVirtualIndex()
		{
			return (VirtualIndex)(_value & 0xFF);
		}

		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is BlobHandle)
			{
				return Equals((BlobHandle)obj);
			}
			return false;
		}

		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(BlobHandle other)
		{
			return _value == other._value;
		}

		/// <returns></returns>
		public override int GetHashCode()
		{
			return (int)_value;
		}

		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator ==(BlobHandle left, BlobHandle right)
		{
			return left.Equals(right);
		}

		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator !=(BlobHandle left, BlobHandle right)
		{
			return !left.Equals(right);
		}
	}
}
