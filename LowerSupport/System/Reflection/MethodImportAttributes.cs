namespace System.Reflection
{
	[Flags]
	public enum MethodImportAttributes : short
	{
		/// <returns></returns>
		None = 0x0,
		/// <returns></returns>
		ExactSpelling = 0x1,
		/// <returns></returns>
		BestFitMappingDisable = 0x20,
		/// <returns></returns>
		BestFitMappingEnable = 0x10,
		/// <returns></returns>
		BestFitMappingMask = 0x30,
		/// <returns></returns>
		CharSetAnsi = 0x2,
		/// <returns></returns>
		CharSetUnicode = 0x4,
		/// <returns></returns>
		CharSetAuto = 0x6,
		/// <returns></returns>
		CharSetMask = 0x6,
		/// <returns></returns>
		ThrowOnUnmappableCharEnable = 0x1000,
		/// <returns></returns>
		ThrowOnUnmappableCharDisable = 0x2000,
		/// <returns></returns>
		ThrowOnUnmappableCharMask = 0x3000,
		/// <returns></returns>
		SetLastError = 0x40,
		/// <returns></returns>
		CallingConventionWinApi = 0x100,
		/// <returns></returns>
		CallingConventionCDecl = 0x200,
		/// <returns></returns>
		CallingConventionStdCall = 0x300,
		/// <returns></returns>
		CallingConventionThisCall = 0x400,
		/// <returns></returns>
		CallingConventionFastCall = 0x500,
		/// <returns></returns>
		CallingConventionMask = 0x700
	}
}
