namespace System.Reflection.PortableExecutable
{
	[Flags]
	public enum CorFlags
	{
		/// <returns></returns>
		ILOnly = 0x1,
		/// <returns></returns>
		Requires32Bit = 0x2,
		/// <returns></returns>
		ILLibrary = 0x4,
		/// <returns></returns>
		StrongNameSigned = 0x8,
		/// <returns></returns>
		NativeEntryPoint = 0x10,
		/// <returns></returns>
		TrackDebugData = 0x10000,
		/// <returns></returns>
		Prefers32Bit = 0x20000
	}
}
