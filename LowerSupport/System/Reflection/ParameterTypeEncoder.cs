namespace System.Reflection.Metadata.Ecma335
{
	public readonly struct ParameterTypeEncoder
	{
		/// <returns></returns>
		public BlobBuilder Builder
		{
			get;
		}

		/// <param name="builder"></param>
		public ParameterTypeEncoder(BlobBuilder builder)
		{
			Builder = builder;
		}

		/// <returns></returns>
		public CustomModifiersEncoder CustomModifiers()
		{
			return new CustomModifiersEncoder(Builder);
		}

		/// <param name="isByRef"></param>
		/// <returns></returns>
		public SignatureTypeEncoder Type(bool isByRef = false)
		{
			if (isByRef)
			{
				Builder.WriteByte(16);
			}
			return new SignatureTypeEncoder(Builder);
		}

		public void TypedReference()
		{
			Builder.WriteByte(22);
		}
	}
}
