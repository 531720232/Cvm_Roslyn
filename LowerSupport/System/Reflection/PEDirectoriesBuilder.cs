namespace System.Reflection.PortableExecutable
{
	public sealed class PEDirectoriesBuilder
	{
		/// <returns></returns>
		public int AddressOfEntryPoint
		{
			get;
			set;
		}

		/// <returns></returns>
		public DirectoryEntry ExportTable
		{
			get;
			set;
		}

		/// <returns></returns>
		public DirectoryEntry ImportTable
		{
			get;
			set;
		}

		/// <returns></returns>
		public DirectoryEntry ResourceTable
		{
			get;
			set;
		}

		/// <returns></returns>
		public DirectoryEntry ExceptionTable
		{
			get;
			set;
		}

		/// <returns></returns>
		public DirectoryEntry BaseRelocationTable
		{
			get;
			set;
		}

		/// <returns></returns>
		public DirectoryEntry DebugTable
		{
			get;
			set;
		}

		/// <returns></returns>
		public DirectoryEntry CopyrightTable
		{
			get;
			set;
		}

		/// <returns></returns>
		public DirectoryEntry GlobalPointerTable
		{
			get;
			set;
		}

		/// <returns></returns>
		public DirectoryEntry ThreadLocalStorageTable
		{
			get;
			set;
		}

		/// <returns></returns>
		public DirectoryEntry LoadConfigTable
		{
			get;
			set;
		}

		/// <returns></returns>
		public DirectoryEntry BoundImportTable
		{
			get;
			set;
		}

		/// <returns></returns>
		public DirectoryEntry ImportAddressTable
		{
			get;
			set;
		}

		/// <returns></returns>
		public DirectoryEntry DelayImportTable
		{
			get;
			set;
		}

		/// <returns></returns>
		public DirectoryEntry CorHeaderTable
		{
			get;
			set;
		}
	}
}
