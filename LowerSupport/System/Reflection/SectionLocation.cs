namespace System.Reflection.PortableExecutable
{
	public readonly struct SectionLocation
	{
		/// <returns></returns>
		public int RelativeVirtualAddress
		{
			get;
		}

		/// <returns></returns>
		public int PointerToRawData
		{
			get;
		}

		/// <param name="relativeVirtualAddress"></param>
		/// <param name="pointerToRawData"></param>
		public SectionLocation(int relativeVirtualAddress, int pointerToRawData)
		{
			RelativeVirtualAddress = relativeVirtualAddress;
			PointerToRawData = pointerToRawData;
		}
	}
}
