namespace System.Reflection.Metadata
{
	public readonly struct EntityHandle : IEquatable<EntityHandle>
	{
		private readonly uint _vToken;

		/// <returns></returns>
		public static readonly ModuleDefinitionHandle ModuleDefinition = new ModuleDefinitionHandle(1);

		/// <returns></returns>
		public static readonly AssemblyDefinitionHandle AssemblyDefinition = new AssemblyDefinitionHandle(1);

		internal uint Type => _vToken & 0x7F000000;

		internal uint VType => (uint)((int)_vToken & -16777216);

		internal bool IsVirtual => ((int)_vToken & -2147483648) != 0;

		/// <returns></returns>
		public bool IsNil => ((int)_vToken & -2130706433) == 0;

		internal int RowId => (int)(_vToken & 0xFFFFFF);

		internal uint SpecificHandleValue => (uint)((int)_vToken & -2130706433);

		/// <returns></returns>
		public HandleKind Kind => (HandleKind)(Type >> 24);

		internal int Token => (int)_vToken;

		internal EntityHandle(uint vToken)
		{
			_vToken = vToken;
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator Handle(EntityHandle handle)
		{
			return Handle.FromVToken(handle._vToken);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator EntityHandle(Handle handle)
		{
			if (handle.IsHeapHandle)
			{
				Throw.InvalidCast();
			}
			return new EntityHandle(handle.EntityHandleValue);
		}

		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is EntityHandle)
			{
				return Equals((EntityHandle)obj);
			}
			return false;
		}

		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(EntityHandle other)
		{
			return _vToken == other._vToken;
		}

		/// <returns></returns>
		public override int GetHashCode()
		{
			return (int)_vToken;
		}

		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator ==(EntityHandle left, EntityHandle right)
		{
			return left.Equals(right);
		}

		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator !=(EntityHandle left, EntityHandle right)
		{
			return !left.Equals(right);
		}

		internal static int Compare(EntityHandle left, EntityHandle right)
		{
			return left._vToken.CompareTo(right._vToken);
		}
	}
}
