namespace System.Reflection.PortableExecutable
{
	public enum Characteristics : ushort
	{
		/// <returns></returns>
		RelocsStripped = 1,
		/// <returns></returns>
		ExecutableImage = 2,
		/// <returns></returns>
		LineNumsStripped = 4,
		/// <returns></returns>
		LocalSymsStripped = 8,
		/// <returns></returns>
		AggressiveWSTrim = 0x10,
		/// <returns></returns>
		LargeAddressAware = 0x20,
		/// <returns></returns>
		BytesReversedLo = 0x80,
		/// <returns></returns>
		Bit32Machine = 0x100,
		/// <returns></returns>
		DebugStripped = 0x200,
		/// <returns></returns>
		RemovableRunFromSwap = 0x400,
		/// <returns></returns>
		NetRunFromSwap = 0x800,
		/// <returns></returns>
		System = 0x1000,
		/// <returns></returns>
		Dll = 0x2000,
		/// <returns></returns>
		UpSystemOnly = 0x4000,
		/// <returns></returns>
		BytesReversedHi = 0x8000
	}
}
