namespace System.Reflection.Metadata
{
	public readonly struct AssemblyReferenceHandle : IEquatable<AssemblyReferenceHandle>
	{
		internal enum VirtualIndex
		{
			System_Runtime,
			System_Runtime_InteropServices_WindowsRuntime,
			System_ObjectModel,
			System_Runtime_WindowsRuntime,
			System_Runtime_WindowsRuntime_UI_Xaml,
			System_Numerics_Vectors,
			Count
		}

		private const uint tokenType = 587202560u;

		private const byte tokenTypeSmall = 35;

		private readonly uint _value;

		internal uint Value => _value;

		private uint VToken => _value | 0x23000000;

		/// <returns></returns>
		public bool IsNil => _value == 0;

		internal bool IsVirtual => ((int)_value & -2147483648) != 0;

		internal int RowId => (int)(_value & 0xFFFFFF);

		private AssemblyReferenceHandle(uint value)
		{
			_value = value;
		}

		internal static AssemblyReferenceHandle FromRowId(int rowId)
		{
			return new AssemblyReferenceHandle((uint)rowId);
		}

		internal static AssemblyReferenceHandle FromVirtualIndex(VirtualIndex virtualIndex)
		{
			return new AssemblyReferenceHandle((uint)((VirtualIndex)(-2147483648) | virtualIndex));
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator Handle(AssemblyReferenceHandle handle)
		{
			return Handle.FromVToken(handle.VToken);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator EntityHandle(AssemblyReferenceHandle handle)
		{
			return new EntityHandle(handle.VToken);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator AssemblyReferenceHandle(Handle handle)
		{
			if (handle.Type != 35)
			{
				Throw.InvalidCast();
			}
			return new AssemblyReferenceHandle(handle.SpecificEntityHandleValue);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator AssemblyReferenceHandle(EntityHandle handle)
		{
			if (handle.Type != 587202560)
			{
				Throw.InvalidCast();
			}
			return new AssemblyReferenceHandle(handle.SpecificHandleValue);
		}

		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator ==(AssemblyReferenceHandle left, AssemblyReferenceHandle right)
		{
			return left._value == right._value;
		}

		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is AssemblyReferenceHandle)
			{
				return ((AssemblyReferenceHandle)obj)._value == _value;
			}
			return false;
		}

		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(AssemblyReferenceHandle other)
		{
			return _value == other._value;
		}

		/// <returns></returns>
		public override int GetHashCode()
		{
			return _value.GetHashCode();
		}

		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator !=(AssemblyReferenceHandle left, AssemblyReferenceHandle right)
		{
			return left._value != right._value;
		}
	}
}
