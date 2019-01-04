namespace System.Reflection
{
	[Flags]
	public enum ManifestResourceAttributes
	{
		/// <returns></returns>
		Public = 0x1,
		/// <returns></returns>
		Private = 0x2,
		/// <returns></returns>
		VisibilityMask = 0x7
	}
}
