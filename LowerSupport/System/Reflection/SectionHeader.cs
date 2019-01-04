namespace System.Reflection.PortableExecutable
{
	public readonly struct SectionHeader
	{
		internal const int NameSize = 8;

		internal const int Size = 40;

		/// <returns></returns>
		public string Name
		{
			get;
		}

		/// <returns></returns>
		public int VirtualSize
		{
			get;
		}

		/// <returns></returns>
		public int VirtualAddress
		{
			get;
		}

		/// <returns></returns>
		public int SizeOfRawData
		{
			get;
		}

		/// <returns></returns>
		public int PointerToRawData
		{
			get;
		}

		/// <returns></returns>
		public int PointerToRelocations
		{
			get;
		}

		/// <returns></returns>
		public int PointerToLineNumbers
		{
			get;
		}

		/// <returns></returns>
		public ushort NumberOfRelocations
		{
			get;
		}

		/// <returns></returns>
		public ushort NumberOfLineNumbers
		{
			get;
		}

		/// <returns></returns>
		public SectionCharacteristics SectionCharacteristics
		{
			get;
		}

	
	}
}
