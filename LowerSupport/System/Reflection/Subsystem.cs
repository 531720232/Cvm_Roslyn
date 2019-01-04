namespace System.Reflection.PortableExecutable
{
	public enum Subsystem : ushort
	{
		/// <returns></returns>
		Unknown = 0,
		/// <returns></returns>
		Native = 1,
		/// <returns></returns>
		WindowsGui = 2,
		/// <returns></returns>
		WindowsCui = 3,
		/// <returns></returns>
		OS2Cui = 5,
		/// <returns></returns>
		PosixCui = 7,
		/// <returns></returns>
		NativeWindows = 8,
		/// <returns></returns>
		WindowsCEGui = 9,
		/// <returns></returns>
		EfiApplication = 10,
		/// <returns></returns>
		EfiBootServiceDriver = 11,
		/// <returns></returns>
		EfiRuntimeDriver = 12,
		/// <returns></returns>
		EfiRom = 13,
		/// <returns></returns>
		Xbox = 14,
		/// <returns></returns>
		WindowsBootApplication = 0x10
	}
}
