namespace System.Reflection.Metadata
{
	public enum ExceptionRegionKind : ushort
	{
		/// <returns></returns>
		Catch = 0,
		/// <returns></returns>
		Filter = 1,
		/// <returns></returns>
		Finally = 2,
		/// <returns></returns>
		Fault = 4
	}
}
