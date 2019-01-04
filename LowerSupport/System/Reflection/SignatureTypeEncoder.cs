
namespace System.Reflection.Metadata.Ecma335
{
	public readonly struct SignatureTypeEncoder
	{
		/// <returns></returns>
		public BlobBuilder Builder
		{
			get;
		}

		/// <param name="builder"></param>
		public SignatureTypeEncoder(BlobBuilder builder)
		{
			Builder = builder;
		}

		private void WriteTypeCode(SignatureTypeCode value)
		{
			Builder.WriteByte((byte)value);
		}

		private void ClassOrValue(bool isValueType)
		{
			Builder.WriteByte((byte)(isValueType ? 17 : 18));
		}

		public void Boolean()
		{
			WriteTypeCode(SignatureTypeCode.Boolean);
		}

		public void Char()
		{
			WriteTypeCode(SignatureTypeCode.Char);
		}

		public void SByte()
		{
			WriteTypeCode(SignatureTypeCode.SByte);
		}

		public void Byte()
		{
			WriteTypeCode(SignatureTypeCode.Byte);
		}

		public void Int16()
		{
			WriteTypeCode(SignatureTypeCode.Int16);
		}

		public void UInt16()
		{
			WriteTypeCode(SignatureTypeCode.UInt16);
		}

		public void Int32()
		{
			WriteTypeCode(SignatureTypeCode.Int32);
		}

		public void UInt32()
		{
			WriteTypeCode(SignatureTypeCode.UInt32);
		}

		public void Int64()
		{
			WriteTypeCode(SignatureTypeCode.Int64);
		}

		public void UInt64()
		{
			WriteTypeCode(SignatureTypeCode.UInt64);
		}

		public void Single()
		{
			WriteTypeCode(SignatureTypeCode.Single);
		}

		public void Double()
		{
			WriteTypeCode(SignatureTypeCode.Double);
		}

		public void String()
		{
			WriteTypeCode(SignatureTypeCode.String);
		}

		public void IntPtr()
		{
			WriteTypeCode(SignatureTypeCode.IntPtr);
		}

		public void UIntPtr()
		{
			WriteTypeCode(SignatureTypeCode.UIntPtr);
		}

		public void Object()
		{
			WriteTypeCode(SignatureTypeCode.Object);
		}

		/// <param name="type"></param>
		public void PrimitiveType(PrimitiveTypeCode type)
		{
			switch (type)
			{
			case PrimitiveTypeCode.Boolean:
			case PrimitiveTypeCode.Char:
			case PrimitiveTypeCode.SByte:
			case PrimitiveTypeCode.Byte:
			case PrimitiveTypeCode.Int16:
			case PrimitiveTypeCode.UInt16:
			case PrimitiveTypeCode.Int32:
			case PrimitiveTypeCode.UInt32:
			case PrimitiveTypeCode.Int64:
			case PrimitiveTypeCode.UInt64:
			case PrimitiveTypeCode.Single:
			case PrimitiveTypeCode.Double:
			case PrimitiveTypeCode.String:
			case PrimitiveTypeCode.IntPtr:
			case PrimitiveTypeCode.UIntPtr:
			case PrimitiveTypeCode.Object:
				Builder.WriteByte((byte)type);
				break;
			default:
				Throw.ArgumentOutOfRange("type");
				break;
			}
		}

		/// <param name="elementType"></param>
		/// <param name="arrayShape"></param>
		public void Array(out SignatureTypeEncoder elementType, out ArrayShapeEncoder arrayShape)
		{
			Builder.WriteByte(20);
			elementType = this;
			arrayShape = new ArrayShapeEncoder(Builder);
		}

		/// <param name="elementType"></param>
		/// <param name="arrayShape"></param>
		public void Array(Action<SignatureTypeEncoder> elementType, Action<ArrayShapeEncoder> arrayShape)
		{
			if (elementType == null)
			{
				Throw.ArgumentNull("elementType");
			}
			if (arrayShape == null)
			{
				Throw.ArgumentNull("arrayShape");
			}
			Array(out SignatureTypeEncoder elementType2, out ArrayShapeEncoder arrayShape2);
			elementType(elementType2);
			arrayShape(arrayShape2);
		}

		/// <param name="type"></param>
		/// <param name="isValueType"></param>
		public void Type(EntityHandle type, bool isValueType)
		{
			int value = CodedIndex.TypeDefOrRef(type);
			ClassOrValue(isValueType);
			Builder.WriteCompressedInteger(value);
		}

		/// <param name="convention"></param>
		/// <param name="attributes"></param>
		/// <param name="genericParameterCount"></param>
		/// <returns></returns>
		public MethodSignatureEncoder FunctionPointer(SignatureCallingConvention convention = SignatureCallingConvention.Default, FunctionPointerAttributes attributes = FunctionPointerAttributes.None, int genericParameterCount = 0)
		{
			if (attributes != 0 && attributes != FunctionPointerAttributes.HasThis && attributes != FunctionPointerAttributes.HasExplicitThis)
			{
				throw new ArgumentException( "attributes");
			}
			if ((uint)genericParameterCount > 65535u)
			{
				Throw.ArgumentOutOfRange("genericParameterCount");
			}
			Builder.WriteByte(27);
			Builder.WriteByte(new SignatureHeader(SignatureKind.Method, convention, (SignatureAttributes)attributes).RawValue);
			if (genericParameterCount != 0)
			{
				Builder.WriteCompressedInteger(genericParameterCount);
			}
			return new MethodSignatureEncoder(Builder, convention == SignatureCallingConvention.VarArgs);
		}

		/// <param name="genericType"></param>
		/// <param name="genericArgumentCount"></param>
		/// <param name="isValueType"></param>
		/// <returns></returns>
		public GenericTypeArgumentsEncoder GenericInstantiation(EntityHandle genericType, int genericArgumentCount, bool isValueType)
		{
			if ((uint)(genericArgumentCount - 1) > 65534u)
			{
				Throw.ArgumentOutOfRange("genericArgumentCount");
			}
			int value = CodedIndex.TypeDefOrRef(genericType);
			Builder.WriteByte(21);
			ClassOrValue(isValueType);
			Builder.WriteCompressedInteger(value);
			Builder.WriteCompressedInteger(genericArgumentCount);
			return new GenericTypeArgumentsEncoder(Builder);
		}

		/// <param name="parameterIndex"></param>
		public void GenericMethodTypeParameter(int parameterIndex)
		{
			if ((uint)parameterIndex > 65535u)
			{
				Throw.ArgumentOutOfRange("parameterIndex");
			}
			Builder.WriteByte(30);
			Builder.WriteCompressedInteger(parameterIndex);
		}

		/// <param name="parameterIndex"></param>
		public void GenericTypeParameter(int parameterIndex)
		{
			if ((uint)parameterIndex > 65535u)
			{
				Throw.ArgumentOutOfRange("parameterIndex");
			}
			Builder.WriteByte(19);
			Builder.WriteCompressedInteger(parameterIndex);
		}

		/// <returns></returns>
		public SignatureTypeEncoder Pointer()
		{
			Builder.WriteByte(15);
			return this;
		}

		public void VoidPointer()
		{
			Builder.WriteByte(15);
			Builder.WriteByte(1);
		}

		/// <returns></returns>
		public SignatureTypeEncoder SZArray()
		{
			Builder.WriteByte(29);
			return this;
		}

		/// <returns></returns>
		public CustomModifiersEncoder CustomModifiers()
		{
			return new CustomModifiersEncoder(Builder);
		}
	}
}
