namespace System.Reflection.Metadata.Ecma335
{
	public readonly struct CustomAttributeNamedArgumentsEncoder
	{
		/// <returns></returns>
		public BlobBuilder Builder
		{
			get;
		}

		/// <param name="builder"></param>
		public CustomAttributeNamedArgumentsEncoder(BlobBuilder builder)
		{
			Builder = builder;
		}

		/// <param name="count"></param>
		/// <returns></returns>
		public NamedArgumentsEncoder Count(int count)
		{
			if ((uint)count > 65535u)
			{
				Throw.ArgumentOutOfRange("count");
			}
			Builder.WriteUInt16((ushort)count);
			return new NamedArgumentsEncoder(Builder);
		}
	}
}
