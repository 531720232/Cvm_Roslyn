namespace System.Reflection.Metadata
{
	internal static class BlobWriterImpl
	{
		internal const int SingleByteCompressedIntegerMaxValue = 127;

		internal const int TwoByteCompressedIntegerMaxValue = 16383;

		internal const int MaxCompressedIntegerValue = 536870911;

		internal const int MinSignedCompressedIntegerValue = -268435456;

		internal const int MaxSignedCompressedIntegerValue = 268435455;

		internal static int GetCompressedIntegerSize(int value)
		{
			if (value <= 127)
			{
				return 1;
			}
			if (value <= 16383)
			{
				return 2;
			}
			return 4;
		}

		internal static void WriteCompressedInteger(ref BlobWriter writer, uint value)
		{
			if (value <= 127)
			{
				writer.WriteByte((byte)value);
			}
			else if (value <= 16383)
			{
				writer.WriteUInt16BE((ushort)(0x8000 | value));
			}
			else if (value <= 536870911)
			{
				writer.WriteUInt32BE((uint)(-1073741824 | (int)value));
			}
			else
			{
				Throw.ValueArgumentOutOfRange();
			}
		}

		internal static void WriteCompressedInteger(BlobBuilder writer, uint value)
		{
			if (value <= 127)
			{
				writer.WriteByte((byte)value);
			}
			else if (value <= 16383)
			{
				writer.WriteUInt16BE((ushort)(0x8000 | value));
			}
			else if (value <= 536870911)
			{
				writer.WriteUInt32BE((uint)(-1073741824 | (int)value));
			}
			else
			{
				Throw.ValueArgumentOutOfRange();
			}
		}

		internal static void WriteCompressedSignedInteger(ref BlobWriter writer, int value)
		{
			int num = value >> 31;
			if ((value & -64) == (num & -64))
			{
				int num2 = ((value & 0x3F) << 1) | (num & 1);
				writer.WriteByte((byte)num2);
			}
			else if ((value & -8192) == (num & -8192))
			{
				int num3 = ((value & 0x1FFF) << 1) | (num & 1);
				writer.WriteUInt16BE((ushort)(0x8000 | num3));
			}
			else if ((value & -268435456) == (num & -268435456))
			{
				int num4 = ((value & 0xFFFFFFF) << 1) | (num & 1);
				writer.WriteUInt32BE((uint)(-1073741824 | num4));
			}
			else
			{
				Throw.ValueArgumentOutOfRange();
			}
		}

		internal static void WriteCompressedSignedInteger(BlobBuilder writer, int value)
		{
			int num = value >> 31;
			if ((value & -64) == (num & -64))
			{
				int num2 = ((value & 0x3F) << 1) | (num & 1);
				writer.WriteByte((byte)num2);
			}
			else if ((value & -8192) == (num & -8192))
			{
				int num3 = ((value & 0x1FFF) << 1) | (num & 1);
				writer.WriteUInt16BE((ushort)(0x8000 | num3));
			}
			else if ((value & -268435456) == (num & -268435456))
			{
				int num4 = ((value & 0xFFFFFFF) << 1) | (num & 1);
				writer.WriteUInt32BE((uint)(-1073741824 | num4));
			}
			else
			{
				Throw.ValueArgumentOutOfRange();
			}
		}

		internal static void WriteConstant(ref BlobWriter writer, object value)
		{
			if (value == null)
			{
				writer.WriteUInt32(0u);
			}
			else
			{
				Type type = value.GetType();
				if (type.IsEnum)
				{
					type = Enum.GetUnderlyingType(type);
				}
				if (type == typeof(bool))
				{
					writer.WriteBoolean((bool)value);
				}
				else if (type == typeof(int))
				{
					writer.WriteInt32((int)value);
				}
				else if (type == typeof(string))
				{
					writer.WriteUTF16((string)value);
				}
				else if (type == typeof(byte))
				{
					writer.WriteByte((byte)value);
				}
				else if (type == typeof(char))
				{
					writer.WriteUInt16((char)value);
				}
				else if (type == typeof(double))
				{
					writer.WriteDouble((double)value);
				}
				else if (type == typeof(short))
				{
					writer.WriteInt16((short)value);
				}
				else if (type == typeof(long))
				{
					writer.WriteInt64((long)value);
				}
				else if (type == typeof(sbyte))
				{
					writer.WriteSByte((sbyte)value);
				}
				else if (type == typeof(float))
				{
					writer.WriteSingle((float)value);
				}
				else if (type == typeof(ushort))
				{
					writer.WriteUInt16((ushort)value);
				}
				else if (type == typeof(uint))
				{
					writer.WriteUInt32((uint)value);
				}
				else
				{
					if (!(type == typeof(ulong)))
					{
						throw new ArgumentException();
					}
					writer.WriteUInt64((ulong)value);
				}
			}
		}

		internal static void WriteConstant(BlobBuilder writer, object value)
		{
			if (value == null)
			{
				writer.WriteUInt32(0u);
			}
			else
			{
				Type type = value.GetType();
				if (type.IsEnum)
				{
					type = Enum.GetUnderlyingType(type);
				}
				if (type == typeof(bool))
				{
					writer.WriteBoolean((bool)value);
				}
				else if (type == typeof(int))
				{
					writer.WriteInt32((int)value);
				}
				else if (type == typeof(string))
				{
					writer.WriteUTF16((string)value);
				}
				else if (type == typeof(byte))
				{
					writer.WriteByte((byte)value);
				}
				else if (type == typeof(char))
				{
					writer.WriteUInt16((char)value);
				}
				else if (type == typeof(double))
				{
					writer.WriteDouble((double)value);
				}
				else if (type == typeof(short))
				{
					writer.WriteInt16((short)value);
				}
				else if (type == typeof(long))
				{
					writer.WriteInt64((long)value);
				}
				else if (type == typeof(sbyte))
				{
					writer.WriteSByte((sbyte)value);
				}
				else if (type == typeof(float))
				{
					writer.WriteSingle((float)value);
				}
				else if (type == typeof(ushort))
				{
					writer.WriteUInt16((ushort)value);
				}
				else if (type == typeof(uint))
				{
					writer.WriteUInt32((uint)value);
				}
				else
				{
					if (!(type == typeof(ulong)))
					{
						throw new ArgumentException();
					}
					writer.WriteUInt64((ulong)value);
				}
			}
		}
	}
}
