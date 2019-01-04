namespace System.Reflection.Metadata.Ecma335
{
	public readonly struct FixedArgumentsEncoder
	{
		/// <returns></returns>
		public BlobBuilder Builder
		{
			get;
		}

		/// <param name="builder"></param>
		public FixedArgumentsEncoder(BlobBuilder builder)
		{
			Builder = builder;
		}

		/// <returns></returns>
		public LiteralEncoder AddArgument()
		{
			return new LiteralEncoder(Builder);
		}
	}
}
