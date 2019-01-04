using System.Collections.Generic;
using System.Collections.Immutable;

namespace System.Reflection.Metadata.Ecma335
{
	public sealed class PortablePdbBuilder
	{
		private Blob _pdbIdBlob;

		private readonly MethodDefinitionHandle _entryPoint;

		private readonly MetadataBuilder _builder;

		private readonly SerializedMetadata _serializedMetadata;

		/// <returns></returns>
		public string MetadataVersion => "PDB v1.0";

		/// <returns></returns>
		public ushort FormatVersion => 256;

		/// <returns></returns>
		public Func<IEnumerable<Blob>, BlobContentId> IdProvider
		{
			get;
		}

		/// <param name="tablesAndHeaps"></param>
		/// <param name="typeSystemRowCounts"></param>
		/// <param name="entryPoint"></param>
		/// <param name="idProvider"></param>
		public PortablePdbBuilder(MetadataBuilder tablesAndHeaps, ImmutableArray<int> typeSystemRowCounts, MethodDefinitionHandle entryPoint, Func<IEnumerable<Blob>, BlobContentId> idProvider = null)
		{
			if (tablesAndHeaps == null)
			{
				Throw.ArgumentNull("tablesAndHeaps");
			}
			ValidateTypeSystemRowCounts(typeSystemRowCounts);
			_builder = tablesAndHeaps;
			_entryPoint = entryPoint;
			_serializedMetadata = tablesAndHeaps.GetSerializedMetadata(typeSystemRowCounts, MetadataVersion.Length, true);
			IdProvider = (idProvider ?? BlobContentId.GetTimeBasedProvider());
		}

		private static void ValidateTypeSystemRowCounts(ImmutableArray<int> typeSystemRowCounts)
		{
			if (typeSystemRowCounts.IsDefault)
			{
				Throw.ArgumentNull("typeSystemRowCounts");
			}
			if (typeSystemRowCounts.Length != MetadataTokens.TableCount)
			{
				throw new ArgumentException("typeSystemRowCounts");
			}
			int num = 0;
			while (true)
			{
				if (num >= typeSystemRowCounts.Length)
				{
					return;
				}
				if (typeSystemRowCounts[num] != 0)
				{
					if ((typeSystemRowCounts[num] & -16777216) != 0)
					{
						throw new ArgumentOutOfRangeException("typeSystemRowCounts");
					}
					if (((1L << num) & 0x1FC93FB7FF57) == 0L)
					{
						break;
					}
				}
				num++;
			}
			throw new ArgumentException("typeSystemRowCounts");
		}

		private void SerializeStandalonePdbStream(BlobBuilder builder)
		{
			int count = builder.Count;
			_pdbIdBlob = builder.ReserveBytes(20);
			builder.WriteInt32((!_entryPoint.IsNil) ? MetadataTokens.GetToken(_entryPoint) : 0);
			builder.WriteUInt64(_serializedMetadata.Sizes.ExternalTablesMask);
			MetadataWriterUtilities.SerializeRowCounts(builder, _serializedMetadata.Sizes.ExternalRowCounts);
			int count2 = builder.Count;
		}

		/// <param name="builder"></param>
		/// <returns></returns>
		public BlobContentId Serialize(BlobBuilder builder)
		{
			if (builder == null)
			{
				Throw.ArgumentNull("builder");
			}
			MetadataBuilder.SerializeMetadataHeader(builder, MetadataVersion, _serializedMetadata.Sizes);
			SerializeStandalonePdbStream(builder);
			_builder.SerializeMetadataTables(builder, _serializedMetadata.Sizes, _serializedMetadata.StringMap, 0, 0);
			_builder.WriteHeapsTo(builder, _serializedMetadata.StringHeap);
			BlobContentId result = IdProvider(builder.GetBlobs());
			BlobWriter blobWriter = new BlobWriter(_pdbIdBlob);
			blobWriter.WriteGuid(result.Guid);
			blobWriter.WriteUInt32(result.Stamp);
			return result;
		}
	}
}
