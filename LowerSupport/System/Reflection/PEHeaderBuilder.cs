using System.Reflection.Internal;

namespace System.Reflection.PortableExecutable
{
	public sealed class PEHeaderBuilder
	{
		/// <returns></returns>
		public Machine Machine
		{
			get;
		}

		/// <returns></returns>
		public Characteristics ImageCharacteristics
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

		internal bool Is32Bit
		{
			get
			{
				if (Machine != Machine.Amd64 && Machine != Machine.IA64)
				{
					return Machine != Machine.Arm64;
				}
				return false;
			}
		}

		/// <param name="machine"></param>
		/// <param name="sectionAlignment"></param>
		/// <param name="fileAlignment"></param>
		/// <param name="imageBase"></param>
		/// <param name="majorLinkerVersion"></param>
		/// <param name="minorLinkerVersion"></param>
		/// <param name="majorOperatingSystemVersion"></param>
		/// <param name="minorOperatingSystemVersion"></param>
		/// <param name="majorImageVersion"></param>
		/// <param name="minorImageVersion"></param>
		/// <param name="majorSubsystemVersion"></param>
		/// <param name="minorSubsystemVersion"></param>
		/// <param name="subsystem"></param>
		/// <param name="dllCharacteristics"></param>
		/// <param name="imageCharacteristics"></param>
		/// <param name="sizeOfStackReserve"></param>
		/// <param name="sizeOfStackCommit"></param>
		/// <param name="sizeOfHeapReserve"></param>
		/// <param name="sizeOfHeapCommit"></param>
		public PEHeaderBuilder(Machine machine = Machine.Unknown, int sectionAlignment = 8192, int fileAlignment = 512, ulong imageBase = 4194304uL, byte majorLinkerVersion = 48, byte minorLinkerVersion = 0, ushort majorOperatingSystemVersion = 4, ushort minorOperatingSystemVersion = 0, ushort majorImageVersion = 0, ushort minorImageVersion = 0, ushort majorSubsystemVersion = 4, ushort minorSubsystemVersion = 0, Subsystem subsystem = Subsystem.WindowsCui, DllCharacteristics dllCharacteristics = DllCharacteristics.DynamicBase | DllCharacteristics.NxCompatible | DllCharacteristics.NoSeh | DllCharacteristics.TerminalServerAware, Characteristics imageCharacteristics = Characteristics.Dll, ulong sizeOfStackReserve = 1048576uL, ulong sizeOfStackCommit = 4096uL, ulong sizeOfHeapReserve = 1048576uL, ulong sizeOfHeapCommit = 4096uL)
		{
			if (fileAlignment < 512 || fileAlignment > 65536 || BitArithmetic.CountBits(fileAlignment) != 1)
			{
				Throw.ArgumentOutOfRange("fileAlignment");
			}
			if (sectionAlignment < fileAlignment || BitArithmetic.CountBits(sectionAlignment) != 1)
			{
				Throw.ArgumentOutOfRange("sectionAlignment");
			}
			Machine = machine;
			SectionAlignment = sectionAlignment;
			FileAlignment = fileAlignment;
			ImageBase = imageBase;
			MajorLinkerVersion = majorLinkerVersion;
			MinorLinkerVersion = minorLinkerVersion;
			MajorOperatingSystemVersion = majorOperatingSystemVersion;
			MinorOperatingSystemVersion = minorOperatingSystemVersion;
			MajorImageVersion = majorImageVersion;
			MinorImageVersion = minorImageVersion;
			MajorSubsystemVersion = majorSubsystemVersion;
			MinorSubsystemVersion = minorSubsystemVersion;
			Subsystem = subsystem;
			DllCharacteristics = dllCharacteristics;
			ImageCharacteristics = imageCharacteristics;
			SizeOfStackReserve = sizeOfStackReserve;
			SizeOfStackCommit = sizeOfStackCommit;
			SizeOfHeapReserve = sizeOfHeapReserve;
			SizeOfHeapCommit = sizeOfHeapCommit;
		}

		/// <returns></returns>
		public static PEHeaderBuilder CreateExecutableHeader()
		{
			return new PEHeaderBuilder(Machine.Unknown, 8192, 512, 4194304uL, 48, 0, 4, 0, 0, 0, 4, 0, Subsystem.WindowsCui, DllCharacteristics.DynamicBase | DllCharacteristics.NxCompatible | DllCharacteristics.NoSeh | DllCharacteristics.TerminalServerAware, Characteristics.ExecutableImage, 1048576uL, 4096uL, 1048576uL, 4096uL);
		}

		/// <returns></returns>
		public static PEHeaderBuilder CreateLibraryHeader()
		{
			return new PEHeaderBuilder(Machine.Unknown, 8192, 512, 4194304uL, 48, 0, 4, 0, 0, 0, 4, 0, Subsystem.WindowsCui, DllCharacteristics.DynamicBase | DllCharacteristics.NxCompatible | DllCharacteristics.NoSeh | DllCharacteristics.TerminalServerAware, Characteristics.Dll, 1048576uL, 4096uL, 1048576uL, 4096uL);
		}

		internal int ComputeSizeOfPEHeaders(int sectionCount)
		{
			return PEBuilder.DosHeaderSize + 4 + 20 + PEHeader.Size(Is32Bit) + 40 * sectionCount;
		}
	}
}
