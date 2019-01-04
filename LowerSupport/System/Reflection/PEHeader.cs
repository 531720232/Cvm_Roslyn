namespace System.Reflection.PortableExecutable
{
	public sealed class PEHeader
	{
		internal const int OffsetOfChecksum = 64;

		/// <returns></returns>
		public PEMagic Magic
		{
			get;
		}

		/// <returns></returns>
		public byte MajorLinkerVersion
		{
			get;
		}

		/// <returns></returns>
		public byte MinorLinkerVersion
		{
			get;
		}

		/// <returns></returns>
		public int SizeOfCode
		{
			get;
		}

		/// <returns></returns>
		public int SizeOfInitializedData
		{
			get;
		}

		/// <returns></returns>
		public int SizeOfUninitializedData
		{
			get;
		}

		/// <returns></returns>
		public int AddressOfEntryPoint
		{
			get;
		}

		/// <returns></returns>
		public int BaseOfCode
		{
			get;
		}

		/// <returns></returns>
		public int BaseOfData
		{
			get;
		}

		/// <returns></returns>
		public ulong ImageBase
		{
			get;
		}

		/// <returns></returns>
		public int SectionAlignment
		{
			get;
		}

		/// <returns></returns>
		public int FileAlignment
		{
			get;
		}

		/// <returns></returns>
		public ushort MajorOperatingSystemVersion
		{
			get;
		}

		/// <returns></returns>
		public ushort MinorOperatingSystemVersion
		{
			get;
		}

		/// <returns></returns>
		public ushort MajorImageVersion
		{
			get;
		}

		/// <returns></returns>
		public ushort MinorImageVersion
		{
			get;
		}

		/// <returns></returns>
		public ushort MajorSubsystemVersion
		{
			get;
		}

		/// <returns></returns>
		public ushort MinorSubsystemVersion
		{
			get;
		}

		/// <returns></returns>
		public int SizeOfImage
		{
			get;
		}

		/// <returns></returns>
		public int SizeOfHeaders
		{
			get;
		}

		/// <returns></returns>
		public uint CheckSum
		{
			get;
		}

		/// <returns></returns>
		public Subsystem Subsystem
		{
			get;
		}

		/// <returns></returns>
		public DllCharacteristics DllCharacteristics
		{
			get;
		}

		/// <returns></returns>
		public ulong SizeOfStackReserve
		{
			get;
		}

		/// <returns></returns>
		public ulong SizeOfStackCommit
		{
			get;
		}

		/// <returns></returns>
		public ulong SizeOfHeapReserve
		{
			get;
		}

		/// <returns></returns>
		public ulong SizeOfHeapCommit
		{
			get;
		}

		/// <returns></returns>
		public int NumberOfRvaAndSizes
		{
			get;
		}

		/// <returns></returns>
		public DirectoryEntry ExportTableDirectory
		{
			get;
		}

		/// <returns></returns>
		public DirectoryEntry ImportTableDirectory
		{
			get;
		}

		/// <returns></returns>
		public DirectoryEntry ResourceTableDirectory
		{
			get;
		}

		/// <returns></returns>
		public DirectoryEntry ExceptionTableDirectory
		{
			get;
		}

		/// <returns></returns>
		public DirectoryEntry CertificateTableDirectory
		{
			get;
		}

		/// <returns></returns>
		public DirectoryEntry BaseRelocationTableDirectory
		{
			get;
		}

		/// <returns></returns>
		public DirectoryEntry DebugTableDirectory
		{
			get;
		}

		/// <returns></returns>
		public DirectoryEntry CopyrightTableDirectory
		{
			get;
		}

		/// <returns></returns>
		public DirectoryEntry GlobalPointerTableDirectory
		{
			get;
		}

		/// <returns></returns>
		public DirectoryEntry ThreadLocalStorageTableDirectory
		{
			get;
		}

		/// <returns></returns>
		public DirectoryEntry LoadConfigTableDirectory
		{
			get;
		}

		/// <returns></returns>
		public DirectoryEntry BoundImportTableDirectory
		{
			get;
		}

		/// <returns></returns>
		public DirectoryEntry ImportAddressTableDirectory
		{
			get;
		}

		/// <returns></returns>
		public DirectoryEntry DelayImportTableDirectory
		{
			get;
		}

		/// <returns></returns>
		public DirectoryEntry CorHeaderTableDirectory
		{
			get;
		}

		internal static int Size(bool is32Bit)
		{
			return 72 + 4 * (is32Bit ? 4 : 8) + 4 + 4 + 128;
		}

		//internal PEHeader(ref PEBinaryReader reader)
		//{
		//	PEMagic pEMagic = (PEMagic)reader.ReadUInt16();
		//	if (pEMagic != PEMagic.PE32 && pEMagic != PEMagic.PE32Plus)
		//	{
		//		throw new BadImageFormatException(System.SR.UnknownPEMagicValue);
		//	}
		//	Magic = pEMagic;
		//	MajorLinkerVersion = reader.ReadByte();
		//	MinorLinkerVersion = reader.ReadByte();
		//	SizeOfCode = reader.ReadInt32();
		//	SizeOfInitializedData = reader.ReadInt32();
		//	SizeOfUninitializedData = reader.ReadInt32();
		//	AddressOfEntryPoint = reader.ReadInt32();
		//	BaseOfCode = reader.ReadInt32();
		//	if (pEMagic == PEMagic.PE32Plus)
		//	{
		//		BaseOfData = 0;
		//	}
		//	else
		//	{
		//		BaseOfData = reader.ReadInt32();
		//	}
		//	if (pEMagic == PEMagic.PE32Plus)
		//	{
		//		ImageBase = reader.ReadUInt64();
		//	}
		//	else
		//	{
		//		ImageBase = reader.ReadUInt32();
		//	}
		//	SectionAlignment = reader.ReadInt32();
		//	FileAlignment = reader.ReadInt32();
		//	MajorOperatingSystemVersion = reader.ReadUInt16();
		//	MinorOperatingSystemVersion = reader.ReadUInt16();
		//	MajorImageVersion = reader.ReadUInt16();
		//	MinorImageVersion = reader.ReadUInt16();
		//	MajorSubsystemVersion = reader.ReadUInt16();
		//	MinorSubsystemVersion = reader.ReadUInt16();
		//	reader.ReadUInt32();
		//	SizeOfImage = reader.ReadInt32();
		//	SizeOfHeaders = reader.ReadInt32();
		//	CheckSum = reader.ReadUInt32();
		//	Subsystem = (Subsystem)reader.ReadUInt16();
		//	DllCharacteristics = (DllCharacteristics)reader.ReadUInt16();
		//	if (pEMagic == PEMagic.PE32Plus)
		//	{
		//		SizeOfStackReserve = reader.ReadUInt64();
		//		SizeOfStackCommit = reader.ReadUInt64();
		//		SizeOfHeapReserve = reader.ReadUInt64();
		//		SizeOfHeapCommit = reader.ReadUInt64();
		//	}
		//	else
		//	{
		//		SizeOfStackReserve = reader.ReadUInt32();
		//		SizeOfStackCommit = reader.ReadUInt32();
		//		SizeOfHeapReserve = reader.ReadUInt32();
		//		SizeOfHeapCommit = reader.ReadUInt32();
		//	}
		//	reader.ReadUInt32();
		//	NumberOfRvaAndSizes = reader.ReadInt32();
		//	ExportTableDirectory = new DirectoryEntry(ref reader);
		//	ImportTableDirectory = new DirectoryEntry(ref reader);
		//	ResourceTableDirectory = new DirectoryEntry(ref reader);
		//	ExceptionTableDirectory = new DirectoryEntry(ref reader);
		//	CertificateTableDirectory = new DirectoryEntry(ref reader);
		//	BaseRelocationTableDirectory = new DirectoryEntry(ref reader);
		//	DebugTableDirectory = new DirectoryEntry(ref reader);
		//	CopyrightTableDirectory = new DirectoryEntry(ref reader);
		//	GlobalPointerTableDirectory = new DirectoryEntry(ref reader);
		//	ThreadLocalStorageTableDirectory = new DirectoryEntry(ref reader);
		//	LoadConfigTableDirectory = new DirectoryEntry(ref reader);
		//	BoundImportTableDirectory = new DirectoryEntry(ref reader);
		//	ImportAddressTableDirectory = new DirectoryEntry(ref reader);
		//	DelayImportTableDirectory = new DirectoryEntry(ref reader);
		//	CorHeaderTableDirectory = new DirectoryEntry(ref reader);
		//	new DirectoryEntry(ref reader);
		//}
	}
}
