namespace System.Reflection.PortableExecutable
{
	public readonly struct DirectoryEntry
	{
		/// <returns></returns>
		public readonly int RelativeVirtualAddress;

		/// <returns></returns>
		public readonly int Size;

		/// <param name="relativeVirtualAddress"></param>
		/// <param name="size"></param>
		public DirectoryEntry(int relativeVirtualAddress, int size)
		{
			RelativeVirtualAddress = relativeVirtualAddress;
			Size = size;
		}

	
	}
}
