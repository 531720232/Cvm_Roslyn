namespace System.Reflection.Metadata.Ecma335
{
	public readonly struct NamedArgumentTypeEncoder
	{
		/// <returns></returns>
		public BlobBuilder Builder
		{
			get;
		}

		/// <param name="builder"></param>
		public NamedArgumentTypeEncoder(BlobBuilder builder)
		{
			Builder = builder;
		}

		/// <returns></returns>
		public CustomAttributeElementTypeEncoder ScalarType()
		{
			return new CustomAttributeElementTypeEncoder(Builder);
		}

		public void Object()
		{
			Builder.WriteByte(81);
		}

		/// <returns></returns>
		public CustomAttributeArrayTypeEncoder SZArray()
		{
			return new CustomAttributeArrayTypeEncoder(Builder);
		}
	}
}
