namespace System.Reflection.Metadata.Ecma335
{
	public readonly struct MethodSignatureEncoder
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
		public MethodSignatureEncoder(BlobBuilder builder, bool hasVarArgs)
		{
			Builder = builder;
			HasVarArgs = hasVarArgs;
		}

		/// <param name="parameterCount"></param>
		/// <param name="returnType"></param>
		/// <param name="parameters"></param>
		public void Parameters(int parameterCount, out ReturnTypeEncoder returnType, out ParametersEncoder parameters)
		{
			if ((uint)parameterCount > 536870911u)
			{
				Throw.ArgumentOutOfRange("parameterCount");
			}
			Builder.WriteCompressedInteger(parameterCount);
			returnType = new ReturnTypeEncoder(Builder);
			parameters = new ParametersEncoder(Builder, HasVarArgs);
		}

		/// <param name="parameterCount"></param>
		/// <param name="returnType"></param>
		/// <param name="parameters"></param>
		public void Parameters(int parameterCount, Action<ReturnTypeEncoder> returnType, Action<ParametersEncoder> parameters)
		{
			if (returnType == null)
			{
				Throw.ArgumentNull("returnType");
			}
			if (parameters == null)
			{
				Throw.ArgumentNull("parameters");
			}
			Parameters(parameterCount, out ReturnTypeEncoder returnType2, out ParametersEncoder parameters2);
			returnType(returnType2);
			parameters(parameters2);
		}
	}
}
