namespace System.Reflection.Metadata.Ecma335
{
	public readonly struct ParametersEncoder
	{
		/// <returns></returns>
		public BlobBuilder Builder
		{
			get;
		}

		/// <returns></returns>
		public bool HasVarArgs
		{
			get;
		}

		/// <param name="builder"></param>
		/// <param name="hasVarArgs"></param>
		public ParametersEncoder(BlobBuilder builder, bool hasVarArgs = false)
		{
			Builder = builder;
			HasVarArgs = hasVarArgs;
		}

		/// <returns></returns>
		public ParameterTypeEncoder AddParameter()
		{
			return new ParameterTypeEncoder(Builder);
		}

		/// <returns></returns>
		public ParametersEncoder StartVarArgs()
		{
			if (!HasVarArgs)
			{
				Throw.SignatureNotVarArg();
			}
			Builder.WriteByte(65);
			return new ParametersEncoder(Builder, false);
		}
	}
}
