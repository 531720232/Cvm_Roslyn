namespace System.Reflection.Metadata.Ecma335
{
	public readonly struct CustomAttributeArrayTypeEncoder
	{
		/// <returns></returns>
		public BlobBuilder Builder
		{
			get;
		}

		/// <param name="builder"></param>
		public CustomAttributeArrayTypeEncoder(BlobBuilder builder)
		{
			Builder = builder;
		}

		public void ObjectArray()
		{
			Builder.WriteByte(29);
			Builder.WriteByte(81);
		}

		/// <returns></returns>
		public CustomAttributeElementTypeEncoder ElementType()
		{
			Builder.WriteByte(29);
			return new CustomAttributeElementTypeEncoder(Builder);
		}
	}
}
