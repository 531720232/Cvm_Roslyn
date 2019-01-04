namespace System.Reflection.Metadata.Ecma335
{
	public readonly struct CustomModifiersEncoder
	{
		/// <returns></returns>
		public BlobBuilder Builder
		{
			get;
		}

		/// <param name="builder"></param>
		public CustomModifiersEncoder(BlobBuilder builder)
		{
			Builder = builder;
		}

		/// <param name="type"></param>
		/// <param name="isOptional"></param>
		/// <returns></returns>
		public CustomModifiersEncoder AddModifier(EntityHandle type, bool isOptional)
		{
			if (type.IsNil)
			{
				Throw.InvalidArgument_Handle("type");
			}
			if (isOptional)
			{
				Builder.WriteByte(32);
			}
			else
			{
				Builder.WriteByte(31);
			}
			Builder.WriteCompressedInteger(CodedIndex.TypeDefOrRefOrSpec(type));
			return this;
		}
	}
}
