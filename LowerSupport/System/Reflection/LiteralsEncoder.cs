namespace System.Reflection.Metadata.Ecma335
{
	public readonly struct LiteralsEncoder
	{
		/// <returns></returns>
		public BlobBuilder Builder
		{
			get;
		}

		/// <param name="builder"></param>
		public LiteralsEncoder(BlobBuilder builder)
		{
			Builder = builder;
		}

		/// <returns></returns>
		public LiteralEncoder AddLiteral()
		{
			return new LiteralEncoder(Builder);
		}
	}
}
