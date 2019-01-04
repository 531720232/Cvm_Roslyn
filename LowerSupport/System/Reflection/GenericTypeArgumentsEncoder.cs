namespace System.Reflection.Metadata.Ecma335
{
	public readonly struct GenericTypeArgumentsEncoder
	{
		/// <returns></returns>
		public BlobBuilder Builder
		{
			get;
		}

		/// <param name="builder"></param>
		public GenericTypeArgumentsEncoder(BlobBuilder builder)
		{
			Builder = builder;
		}

		/// <returns></returns>
		public SignatureTypeEncoder AddArgument()
		{
			return new SignatureTypeEncoder(Builder);
		}
	}
}
