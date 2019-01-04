namespace System.Reflection.Metadata.Ecma335
{
	public readonly struct NameEncoder
	{
		/// <returns></returns>
		public BlobBuilder Builder
		{
			get;
		}

		/// <param name="builder"></param>
		public NameEncoder(BlobBuilder builder)
		{
			Builder = builder;
		}

		/// <param name="name"></param>
		public void Name(string name)
		{
			if (name == null)
			{
				Throw.ArgumentNull("name");
			}
			if (name.Length == 0)
			{
				Throw.ArgumentEmptyString("name");
			}
			Builder.WriteSerializedString(name);
		}
	}
}
