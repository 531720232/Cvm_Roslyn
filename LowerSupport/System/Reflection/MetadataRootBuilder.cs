using System.Collections.Immutable;

namespace System.Reflection.Metadata.Ecma335
{
	public sealed class MetadataRootBuilder
	{
		private const string DefaultMetadataVersionString = "v4.0.30319";

		internal static readonly ImmutableArray<int> EmptyRowCounts = ImmutableArray.Create(new int[MetadataTokens.TableCount]);

		private readonly MetadataBuilder _tablesAndHeaps;

		private readonly SerializedMetadata _serializedMetadata;

		/// <returns></returns>
		public string MetadataVersion
		{
			get;
		}

		/// <returns></returns>
		public bool SuppressValidation
		{
			get;
		}

		/// <returns></returns>
		public MetadataSizes Sizes => _serializedMetadata.Sizes;

		/// <param name="tablesAndHeaps"></param>
		/// <param name="metadataVersion"></param>
		/// <param name="suppressValidation"></param>
		public MetadataRootBuilder(MetadataBuilder tablesAndHeaps, string metadataVersion = null, bool suppressValidation = false)
		{
			if (tablesAndHeaps == null)
			{
				Throw.ArgumentNull("tablesAndHeaps");
			}
			int num = (metadataVersion != null) ? BlobUtilities.GetUTF8ByteCount(metadataVersion) : "v2.0.0".Length;
			if (num > 254)
			{
				Throw.InvalidArgument("metadataVersion","");
			}
			_tablesAndHeaps = tablesAndHeaps;
			MetadataVersion = (metadataVersion ?? "v2.0.0");
			SuppressValidation = suppressValidation;
			_serializedMetadata = tablesAndHeaps.GetSerializedMetadata(EmptyRowCounts, num, false);
		}

		/// <param name="builder"></param>
		/// <param name="methodBodyStreamRva"></param>
		/// <param name="mappedFieldDataStreamRva"></param>
		public void Serialize(BlobBuilder builder, int methodBodyStreamRva, int mappedFieldDataStreamRva)
		{
			if (builder == null)
			{
				Throw.ArgumentNull("builder");
			}
			if (methodBodyStreamRva < 0)
			{
				Throw.ArgumentOutOfRange("methodBodyStreamRva");
			}
			if (mappedFieldDataStreamRva < 0)
			{
				Throw.ArgumentOutOfRange("mappedFieldDataStreamRva");
			}
			if (!SuppressValidation)
			{
				_tablesAndHeaps.ValidateOrder();
			}
			MetadataBuilder.SerializeMetadataHeader(builder, MetadataVersion, _serializedMetadata.Sizes);
			_tablesAndHeaps.SerializeMetadataTables(builder, _serializedMetadata.Sizes, _serializedMetadata.StringMap, methodBodyStreamRva, mappedFieldDataStreamRva);
			_tablesAndHeaps.WriteHeapsTo(builder, _serializedMetadata.StringHeap);
		}
	}
}
