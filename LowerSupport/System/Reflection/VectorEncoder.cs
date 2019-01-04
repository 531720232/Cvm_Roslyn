namespace System.Reflection.Metadata.Ecma335
{
	public readonly struct VectorEncoder
	{
		/// <returns></returns>
		public BlobBuilder Builder
		{
			get;
		}

		/// <param name="builder"></param>
		public VectorEncoder(BlobBuilder builder)
		{
			Builder = builder;
		}

		/// <param name="count"></param>
		/// <returns></returns>
		public LiteralsEncoder Count(int count)
		{
			if (count < 0)
			{
				Throw.ArgumentOutOfRange("count");
			}
			Builder.WriteUInt32((uint)count);
			return new LiteralsEncoder(Builder);
		}
	}
}
