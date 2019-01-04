namespace System.Reflection.PortableExecutable
{
	[Flags]
	public enum SectionCharacteristics : uint
	{
		/// <returns></returns>
		TypeReg = 0x0,
		/// <returns></returns>
		TypeDSect = 0x1,
		/// <returns></returns>
		TypeNoLoad = 0x2,
		/// <returns></returns>
		TypeGroup = 0x4,
		/// <returns></returns>
		TypeNoPad = 0x8,
		/// <returns></returns>
		TypeCopy = 0x10,
		/// <returns></returns>
		ContainsCode = 0x20,
		/// <returns></returns>
		ContainsInitializedData = 0x40,
		/// <returns></returns>
		ContainsUninitializedData = 0x80,
		/// <returns></returns>
		LinkerOther = 0x100,
		/// <returns></returns>
		LinkerInfo = 0x200,
		/// <returns></returns>
		TypeOver = 0x400,
		/// <returns></returns>
		LinkerRemove = 0x800,
		/// <returns></returns>
		LinkerComdat = 0x1000,
		/// <returns></returns>
		MemProtected = 0x4000,
		/// <returns></returns>
		NoDeferSpecExc = 0x4000,
		/// <returns></returns>
		GPRel = 0x8000,
		/// <returns></returns>
		MemFardata = 0x8000,
		/// <returns></returns>
		MemSysheap = 0x10000,
		/// <returns></returns>
		MemPurgeable = 0x20000,
		/// <returns></returns>
		Mem16Bit = 0x20000,
		/// <returns></returns>
		MemLocked = 0x40000,
		/// <returns></returns>
		MemPreload = 0x80000,
		/// <returns></returns>
		Align1Bytes = 0x100000,
		/// <returns></returns>
		Align2Bytes = 0x200000,
		/// <returns></returns>
		Align4Bytes = 0x300000,
		/// <returns></returns>
		Align8Bytes = 0x400000,
		/// <returns></returns>
		Align16Bytes = 0x500000,
		/// <returns></returns>
		Align32Bytes = 0x600000,
		/// <returns></returns>
		Align64Bytes = 0x700000,
		/// <returns></returns>
		Align128Bytes = 0x800000,
		/// <returns></returns>
		Align256Bytes = 0x900000,
		/// <returns></returns>
		Align512Bytes = 0xA00000,
		/// <returns></returns>
		Align1024Bytes = 0xB00000,
		/// <returns></returns>
		Align2048Bytes = 0xC00000,
		/// <returns></returns>
		Align4096Bytes = 0xD00000,
		/// <returns></returns>
		Align8192Bytes = 0xE00000,
		/// <returns></returns>
		AlignMask = 0xF00000,
		/// <returns></returns>
		LinkerNRelocOvfl = 0x1000000,
		/// <returns></returns>
		MemDiscardable = 0x2000000,
		/// <returns></returns>
		MemNotCached = 0x4000000,
		/// <returns></returns>
		MemNotPaged = 0x8000000,
		/// <returns></returns>
		MemShared = 0x10000000,
		/// <returns></returns>
		MemExecute = 0x20000000,
		/// <returns></returns>
		MemRead = 0x40000000,
		/// <returns></returns>
		MemWrite = 0x80000000
	}
}
