
namespace System.Reflection.Metadata.Ecma335
{
	public  struct BlobEncoder
	{
		/// <returns></returns>
		public BlobBuilder Builder
		{
			get;
		}

		/// <param name="builder"></param>
		public BlobEncoder(BlobBuilder builder)
		{
			if (builder == null)
			{
				Throw.BuilderArgumentNull();
			}
			Builder = builder;
		}

		/// <returns></returns>
		public SignatureTypeEncoder FieldSignature()
		{
			Builder.WriteByte(6);
			return new SignatureTypeEncoder(Builder);
		}

		/// <param name="genericArgumentCount"></param>
		/// <returns></returns>
		public GenericTypeArgumentsEncoder MethodSpecificationSignature(int genericArgumentCount)
		{
			if ((uint)genericArgumentCount > 65535u)
			{
				Throw.ArgumentOutOfRange("genericArgumentCount");
			}
			Builder.WriteByte(10);
			Builder.WriteCompressedInteger(genericArgumentCount);
			return new GenericTypeArgumentsEncoder(Builder);
		}

		/// <param name="convention"></param>
		/// <param name="genericParameterCount"></param>
		/// <param name="isInstanceMethod"></param>
		/// <returns></returns>
		public MethodSignatureEncoder MethodSignature(SignatureCallingConvention convention = SignatureCallingConvention.Default, int genericParameterCount = 0, bool isInstanceMethod = false)
		{
			if ((uint)genericParameterCount > 65535u)
			{
				Throw.ArgumentOutOfRange("genericParameterCount");
			}
			SignatureAttributes attributes = (SignatureAttributes)(((genericParameterCount != 0) ? 16 : 0) | (isInstanceMethod ? 32 : 0));
			Builder.WriteByte(new SignatureHeader(SignatureKind.Method, convention, attributes).RawValue);
			if (genericParameterCount != 0)
			{
				Builder.WriteCompressedInteger(genericParameterCount);
			}
			return new MethodSignatureEncoder(Builder, convention == SignatureCallingConvention.VarArgs);
		}

		/// <param name="isInstanceProperty"></param>
		/// <returns></returns>
		public MethodSignatureEncoder PropertySignature(bool isInstanceProperty = false)
		{
			Builder.WriteByte(new SignatureHeader(SignatureKind.Property, SignatureCallingConvention.Default, isInstanceProperty ? SignatureAttributes.Instance : SignatureAttributes.None).RawValue);
			return new MethodSignatureEncoder(Builder, false);
		}

		/// <param name="fixedArguments"></param>
		/// <param name="namedArguments"></param>
		public void CustomAttributeSignature(out FixedArgumentsEncoder fixedArguments, out CustomAttributeNamedArgumentsEncoder namedArguments)
		{
			Builder.WriteUInt16(1);
			fixedArguments = new FixedArgumentsEncoder(Builder);
			namedArguments = new CustomAttributeNamedArgumentsEncoder(Builder);
		}

		/// <param name="fixedArguments"></param>
		/// <param name="namedArguments"></param>
		public void CustomAttributeSignature(Action<FixedArgumentsEncoder> fixedArguments, Action<CustomAttributeNamedArgumentsEncoder> namedArguments)
		{
			if (fixedArguments == null)
			{
				Throw.ArgumentNull("fixedArguments");
			}
			if (namedArguments == null)
			{
				Throw.ArgumentNull("namedArguments");
			}
			CustomAttributeSignature(out FixedArgumentsEncoder fixedArguments2, out CustomAttributeNamedArgumentsEncoder namedArguments2);
			fixedArguments(fixedArguments2);
			namedArguments(namedArguments2);
		}

		/// <param name="variableCount"></param>
		/// <returns></returns>
		public LocalVariablesEncoder LocalVariableSignature(int variableCount)
		{
			if ((uint)variableCount > 536870911u)
			{
				Throw.ArgumentOutOfRange("variableCount");
			}
			Builder.WriteByte(7);
			Builder.WriteCompressedInteger(variableCount);
			return new LocalVariablesEncoder(Builder);
		}

		/// <returns></returns>
		public SignatureTypeEncoder TypeSpecificationSignature()
		{
			return new SignatureTypeEncoder(Builder);
		}

		/// <param name="attributeCount"></param>
		/// <returns></returns>
		public PermissionSetEncoder PermissionSetBlob(int attributeCount)
		{
			if ((uint)attributeCount > 536870911u)
			{
				Throw.ArgumentOutOfRange("attributeCount");
			}
			Builder.WriteByte(46);
			Builder.WriteCompressedInteger(attributeCount);
			return new PermissionSetEncoder(Builder);
		}

		/// <param name="argumentCount"></param>
		/// <returns></returns>
		public NamedArgumentsEncoder PermissionSetArguments(int argumentCount)
		{
			if ((uint)argumentCount > 536870911u)
			{
				Throw.ArgumentOutOfRange("argumentCount");
			}
			Builder.WriteCompressedInteger(argumentCount);
			return new NamedArgumentsEncoder(Builder);
		}
	}
}
