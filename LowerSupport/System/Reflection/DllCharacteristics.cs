namespace System.Reflection.PortableExecutable
{
	[Flags]
	public enum DllCharacteristics : ushort
	{
		/// <returns></returns>
		ProcessInit = 0x1,
		/// <returns></returns>
		ProcessTerm = 0x2,
		/// <returns></returns>
		ThreadInit = 0x4,
		/// <returns></returns>
		ThreadTerm = 0x8,
		/// <returns></returns>
		HighEntropyVirtualAddressSpace = 0x20,
		/// <returns></returns>
		DynamicBase = 0x40,
		/// <returns></returns>
		NxCompatible = 0x100,
		/// <returns></returns>
		NoIsolation = 0x200,
		/// <returns></returns>
		NoSeh = 0x400,
		/// <returns></returns>
		NoBind = 0x800,
		/// <returns></returns>
		AppContainer = 0x1000,
		/// <returns></returns>
		WdmDriver = 0x2000,
		/// <returns></returns>
		TerminalServerAware = 0x8000
	}
}
