namespace System.Reflection.Metadata.Ecma335
{
	public readonly struct LocalVariableTypeEncoder
	{
		/// <returns></returns>
		public BlobBuilder Builder
		{
			get;
		}

		/// <param name="builder"></param>
		public LocalVariableTypeEncoder(BlobBuilder builder)
		{
			Builder = builder;
		}

		/// <returns></returns>
		public CustomModifiersEncoder CustomModifiers()
		{
			return new CustomModifiersEncoder(Builder);
		}

		/// <param name="isByRef"></param>
		/// <param name="isPinned"></param>
		/// <returns></returns>
		public SignatureTypeEncoder Type(bool isByRef = false, bool isPinned = false)
		{
			if (isPinned)
			{
				Builder.WriteByte(69);
			}
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
