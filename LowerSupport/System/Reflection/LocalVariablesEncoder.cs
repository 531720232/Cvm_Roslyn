namespace System.Reflection.Metadata.Ecma335
{
	public readonly struct LocalVariablesEncoder
	{
		/// <returns></returns>
		public BlobBuilder Builder
		{
			get;
		}

		/// <param name="builder"></param>
		public LocalVariablesEncoder(BlobBuilder builder)
		{
			Builder = builder;
		}

		/// <returns></returns>
		public LocalVariableTypeEncoder AddVariable()
		{
			return new LocalVariableTypeEncoder(Builder);
		}
	}
}
