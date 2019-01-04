namespace System.Reflection
{
	[Flags]
	public enum AssemblyFlags
	{
		/// <returns></returns>
		PublicKey = 0x1,
		/// <returns></returns>
		Retargetable = 0x100,
		/// <returns></returns>
		WindowsRuntime = 0x200,
		/// <returns></returns>
		ContentTypeMask = 0xE00,
		/// <returns></returns>
		DisableJitCompileOptimizer = 0x4000,
		/// <returns></returns>
		EnableJitCompileTracking = 0x8000
	}
}
