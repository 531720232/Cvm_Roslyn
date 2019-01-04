

namespace System.Reflection.Metadata.Ecma335
{
	public readonly struct InstructionEncoder
	{
		/// <returns></returns>
		public BlobBuilder CodeBuilder
		{
			get;
		}

		/// <returns></returns>
		public ControlFlowBuilder ControlFlowBuilder
		{
			get;
		}

		/// <returns></returns>
		public int Offset => CodeBuilder.Count;

		/// <param name="codeBuilder"></param>
		/// <param name="controlFlowBuilder"></param>
		public InstructionEncoder(BlobBuilder codeBuilder, ControlFlowBuilder controlFlowBuilder = null)
		{
			if (codeBuilder == null)
			{
				Throw.BuilderArgumentNull();
			}
			CodeBuilder = codeBuilder;
			ControlFlowBuilder = controlFlowBuilder;
		}

		/// <param name="code"></param>
		public void OpCode(ILOpCode code)
		{
			if ((uint)(byte)code == (uint)code)
			{
				CodeBuilder.WriteByte((byte)code);
			}
			else
			{
				CodeBuilder.WriteUInt16BE((ushort)code);
			}
		}

		/// <param name="handle"></param>
		public void Token(EntityHandle handle)
		{
			Token(MetadataTokens.GetToken(handle));
		}

		/// <param name="token"></param>
		public void Token(int token)
		{
			CodeBuilder.WriteInt32(token);
		}

		/// <param name="handle"></param>
		public void LoadString(UserStringHandle handle)
		{
			OpCode(ILOpCode.Ldstr);
			Token(MetadataTokens.GetToken(handle));
		}

		/// <param name="methodHandle"></param>
		public void Call(EntityHandle methodHandle)
		{
			if (methodHandle.Kind != HandleKind.MethodDefinition && methodHandle.Kind != HandleKind.MethodSpecification && methodHandle.Kind != HandleKind.MemberReference)
			{
				Throw.InvalidArgument_Handle("methodHandle");
			}
			OpCode(ILOpCode.Call);
			Token(methodHandle);
		}

		/// <param name="methodHandle"></param>
		public void Call(MethodDefinitionHandle methodHandle)
		{
			OpCode(ILOpCode.Call);
			Token(methodHandle);
		}

		/// <param name="methodHandle"></param>
		public void Call(MethodSpecificationHandle methodHandle)
		{
			OpCode(ILOpCode.Call);
			Token(methodHandle);
		}

		/// <param name="methodHandle"></param>
		public void Call(MemberReferenceHandle methodHandle)
		{
			OpCode(ILOpCode.Call);
			Token(methodHandle);
		}

		/// <param name="signature"></param>
		public void CallIndirect(StandaloneSignatureHandle signature)
		{
			OpCode(ILOpCode.Calli);
			Token(signature);
		}

		/// <param name="value"></param>
		public void LoadConstantI4(int value)
		{
			ILOpCode code;
			switch (value)
			{
			case -1:
				code = ILOpCode.Ldc_i4_m1;
				break;
			case 0:
				code = ILOpCode.Ldc_i4_0;
				break;
			case 1:
				code = ILOpCode.Ldc_i4_1;
				break;
			case 2:
				code = ILOpCode.Ldc_i4_2;
				break;
			case 3:
				code = ILOpCode.Ldc_i4_3;
				break;
			case 4:
				code = ILOpCode.Ldc_i4_4;
				break;
			case 5:
				code = ILOpCode.Ldc_i4_5;
				break;
			case 6:
				code = ILOpCode.Ldc_i4_6;
				break;
			case 7:
				code = ILOpCode.Ldc_i4_7;
				break;
			case 8:
				code = ILOpCode.Ldc_i4_8;
				break;
			default:
				if ((sbyte)value == value)
				{
					OpCode(ILOpCode.Ldc_i4_s);
					CodeBuilder.WriteSByte((sbyte)value);
				}
				else
				{
					OpCode(ILOpCode.Ldc_i4);
					CodeBuilder.WriteInt32(value);
				}
				return;
			}
			OpCode(code);
		}

		/// <param name="value"></param>
		public void LoadConstantI8(long value)
		{
			OpCode(ILOpCode.Ldc_i8);
			CodeBuilder.WriteInt64(value);
		}

		/// <param name="value"></param>
		public void LoadConstantR4(float value)
		{
			OpCode(ILOpCode.Ldc_r4);
			CodeBuilder.WriteSingle(value);
		}

		/// <param name="value"></param>
		public void LoadConstantR8(double value)
		{
			OpCode(ILOpCode.Ldc_r8);
			CodeBuilder.WriteDouble(value);
		}

		/// <param name="slotIndex"></param>
		public void LoadLocal(int slotIndex)
		{
			switch (slotIndex)
			{
			case 0:
				OpCode(ILOpCode.Ldloc_0);
				break;
			case 1:
				OpCode(ILOpCode.Ldloc_1);
				break;
			case 2:
				OpCode(ILOpCode.Ldloc_2);
				break;
			case 3:
				OpCode(ILOpCode.Ldloc_3);
				break;
			default:
				if ((uint)slotIndex <= 255u)
				{
					OpCode(ILOpCode.Ldloc_s);
					CodeBuilder.WriteByte((byte)slotIndex);
				}
				else if (slotIndex > 0)
				{
					OpCode(ILOpCode.Ldloc);
					CodeBuilder.WriteInt32(slotIndex);
				}
				else
				{
					Throw.ArgumentOutOfRange("slotIndex");
				}
				break;
			}
		}

		/// <param name="slotIndex"></param>
		public void StoreLocal(int slotIndex)
		{
			switch (slotIndex)
			{
			case 0:
				OpCode(ILOpCode.Stloc_0);
				break;
			case 1:
				OpCode(ILOpCode.Stloc_1);
				break;
			case 2:
				OpCode(ILOpCode.Stloc_2);
				break;
			case 3:
				OpCode(ILOpCode.Stloc_3);
				break;
			default:
				if ((uint)slotIndex <= 255u)
				{
					OpCode(ILOpCode.Stloc_s);
					CodeBuilder.WriteByte((byte)slotIndex);
				}
				else if (slotIndex > 0)
				{
					OpCode(ILOpCode.Stloc);
					CodeBuilder.WriteInt32(slotIndex);
				}
				else
				{
					Throw.ArgumentOutOfRange("slotIndex");
				}
				break;
			}
		}

		/// <param name="slotIndex"></param>
		public void LoadLocalAddress(int slotIndex)
		{
			if ((uint)slotIndex <= 255u)
			{
				OpCode(ILOpCode.Ldloca_s);
				CodeBuilder.WriteByte((byte)slotIndex);
			}
			else if (slotIndex > 0)
			{
				OpCode(ILOpCode.Ldloca);
				CodeBuilder.WriteInt32(slotIndex);
			}
			else
			{
				Throw.ArgumentOutOfRange("slotIndex");
			}
		}

		/// <param name="argumentIndex"></param>
		public void LoadArgument(int argumentIndex)
		{
			switch (argumentIndex)
			{
			case 0:
				OpCode(ILOpCode.Ldarg_0);
				break;
			case 1:
				OpCode(ILOpCode.Ldarg_1);
				break;
			case 2:
				OpCode(ILOpCode.Ldarg_2);
				break;
			case 3:
				OpCode(ILOpCode.Ldarg_3);
				break;
			default:
				if ((uint)argumentIndex <= 255u)
				{
					OpCode(ILOpCode.Ldarg_s);
					CodeBuilder.WriteByte((byte)argumentIndex);
				}
				else if (argumentIndex > 0)
				{
					OpCode(ILOpCode.Ldarg);
					CodeBuilder.WriteInt32(argumentIndex);
				}
				else
				{
					Throw.ArgumentOutOfRange("argumentIndex");
				}
				break;
			}
		}

		/// <param name="argumentIndex"></param>
		public void LoadArgumentAddress(int argumentIndex)
		{
			if ((uint)argumentIndex <= 255u)
			{
				OpCode(ILOpCode.Ldarga_s);
				CodeBuilder.WriteByte((byte)argumentIndex);
			}
			else if (argumentIndex > 0)
			{
				OpCode(ILOpCode.Ldarga);
				CodeBuilder.WriteInt32(argumentIndex);
			}
			else
			{
				Throw.ArgumentOutOfRange("argumentIndex");
			}
		}

		/// <param name="argumentIndex"></param>
		public void StoreArgument(int argumentIndex)
		{
			if ((uint)argumentIndex <= 255u)
			{
				OpCode(ILOpCode.Starg_s);
				CodeBuilder.WriteByte((byte)argumentIndex);
			}
			else if (argumentIndex > 0)
			{
				OpCode(ILOpCode.Starg);
				CodeBuilder.WriteInt32(argumentIndex);
			}
			else
			{
				Throw.ArgumentOutOfRange("argumentIndex");
			}
		}

		/// <returns></returns>
		public LabelHandle DefineLabel()
		{
			return GetBranchBuilder().AddLabel();
		}

		/// <param name="code"></param>
		/// <param name="label"></param>
		public void Branch(ILOpCode code, LabelHandle label)
		{
			int branchOperandSize = code.GetBranchOperandSize();
			GetBranchBuilder().AddBranch(Offset, label, code);
			OpCode(code);
			if (branchOperandSize == 1)
			{
				CodeBuilder.WriteSByte(-1);
			}
			else
			{
				CodeBuilder.WriteInt32(-1);
			}
		}

		/// <param name="label"></param>
		public void MarkLabel(LabelHandle label)
		{
			GetBranchBuilder().MarkLabel(Offset, label);
		}

		private ControlFlowBuilder GetBranchBuilder()
		{
			if (ControlFlowBuilder == null)
			{
				Throw.ControlFlowBuilderNotAvailable();
			}
			return ControlFlowBuilder;
		}
	}
}
