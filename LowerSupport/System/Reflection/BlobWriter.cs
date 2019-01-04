using System.Collections.Immutable;
using System.IO;
using System.Reflection.Internal;
using System.Runtime.InteropServices;

namespace System.Reflection.Metadata
{
	public struct BlobWriter
	{
		private readonly byte[] _buffer;

		private readonly int _start;

		private readonly int _end;

		private int _position;

		internal bool IsDefault => _buffer == null;

		/// <returns></returns>
		public int Offset
		{
			get
			{
				return _position - _start;
			}
			set
			{
				if (value < 0 || _start > _end - value)
				{
					Throw.ValueArgumentOutOfRange();
				}
				_position = _start + value;
			}
		}

		/// <returns></returns>
		public int Length => _end - _start;

		/// <returns></returns>
		public int RemainingBytes => _end - _position;

		/// <returns></returns>
		public Blob Blob => new Blob(_buffer, _start, Length);

		/// <param name="size"></param>
		public BlobWriter(int size)
		{
			this = new BlobWriter(new byte[size]);
		}

		/// <param name="buffer"></param>
		public BlobWriter(byte[] buffer)
		{
			this = new BlobWriter(buffer, 0, buffer.Length);
		}

		/// <param name="blob"></param>
		public BlobWriter(Blob blob)
		{
			this = new BlobWriter(blob.Buffer, blob.Start, blob.Length);
		}

		/// <param name="buffer"></param>
		/// <param name="start"></param>
		/// <param name="count"></param>
		public BlobWriter(byte[] buffer, int start, int count)
		{
			_buffer = buffer;
			_start = start;
			_position = start;
			_end = start + count;
		}

		/// <param name="other"></param>
		/// <returns></returns>
		public bool ContentEquals(BlobWriter other)
		{
			if (Length == other.Length)
			{
				return ByteSequenceComparer.Equals(_buffer, _start, other._buffer, other._start, Length);
			}
			return false;
		}

		/// <returns></returns>
		public byte[] ToArray()
		{
			return ToArray(0, Offset);
		}

		/// <param name="start"></param>
		/// <param name="byteCount"></param>
		/// <returns></returns>
		public byte[] ToArray(int start, int byteCount)
		{
			BlobUtilities.ValidateRange(Length, start, byteCount, "byteCount");
			byte[] array = new byte[byteCount];
			Array.Copy(_buffer, _start + start, array, 0, byteCount);
			return array;
		}

		/// <returns></returns>
		public ImmutableArray<byte> ToImmutableArray()
		{
			return ToImmutableArray(0, Offset);
		}

		/// <param name="start"></param>
		/// <param name="byteCount"></param>
		/// <returns></returns>
		public ImmutableArray<byte> ToImmutableArray(int start, int byteCount)
		{
			byte[] array = ToArray(start, byteCount);
			return ImmutableByteArrayInterop.DangerousCreateFromUnderlyingArray(ref array);
		}

		private int Advance(int value)
		{
			int position = _position;
			if (position > _end - value)
			{
				Throw.OutOfBounds();
			}
			_position = position + value;
			return position;
		}

		/// <param name="value"></param>
		/// <param name="byteCount"></param>
		public unsafe void WriteBytes(byte value, int byteCount)
		{
			if (byteCount < 0)
			{
				Throw.ArgumentOutOfRange("byteCount");
			}
			int num = Advance(byteCount);
			fixed (byte* ptr = _buffer)
			{
				byte* ptr2 = ptr + num;
				for (int i = 0; i < byteCount; i++)
				{
					ptr2[i] = value;
				}
			}
		}

		/// <param name="buffer"></param>
		/// <param name="byteCount"></param>
		public unsafe void WriteBytes(byte* buffer, int byteCount)
		{
			if (buffer == null)
			{
				Throw.ArgumentNull("buffer");
			}
			if (byteCount < 0)
			{
				Throw.ArgumentOutOfRange("byteCount");
			}
			WriteBytesUnchecked(buffer, byteCount);
		}

		private unsafe void WriteBytesUnchecked(byte* buffer, int byteCount)
		{
			int startIndex = Advance(byteCount);
			Marshal.Copy((IntPtr)(void*)buffer, _buffer, startIndex, byteCount);
		}

		/// <param name="source"></param>
		public void WriteBytes(BlobBuilder source)
		{
			if (source == null)
			{
				Throw.ArgumentNull("source");
			}
			source.WriteContentTo(ref this);
		}

		/// <param name="source"></param>
		/// <param name="byteCount"></param>
		/// <returns></returns>
		public int WriteBytes(Stream source, int byteCount)
		{
			if (source == null)
			{
				Throw.ArgumentNull("source");
			}
			if (byteCount < 0)
			{
				Throw.ArgumentOutOfRange("byteCount");
			}
			int num = Advance(byteCount);
			int num2 = source.TryReadAll(_buffer, num, byteCount);
			_position = num + num2;
			return num2;
		}

		/// <param name="buffer"></param>
		public void WriteBytes(ImmutableArray<byte> buffer)
		{
			WriteBytes(buffer, 0, (!buffer.IsDefault) ? buffer.Length : 0);
		}

		/// <param name="buffer"></param>
		/// <param name="start"></param>
		/// <param name="byteCount"></param>
		public void WriteBytes(ImmutableArray<byte> buffer, int start, int byteCount)
		{
			WriteBytes(ImmutableByteArrayInterop.DangerousGetUnderlyingArray(buffer), start, byteCount);
		}

		/// <param name="buffer"></param>
		public void WriteBytes(byte[] buffer)
		{
			WriteBytes(buffer, 0, (buffer != null) ? buffer.Length : 0);
		}

		/// <param name="buffer"></param>
		/// <param name="start"></param>
		/// <param name="byteCount"></param>
		public unsafe void WriteBytes(byte[] buffer, int start, int byteCount)
		{
			if (buffer == null)
			{
				Throw.ArgumentNull("buffer");
			}
			BlobUtilities.ValidateRange(buffer.Length, start, byteCount, "byteCount");
			if (buffer.Length != 0)
			{
				fixed (byte* ptr = &buffer[0])
				{
					WriteBytes(ptr + start, byteCount);
				}
			}
		}

		/// <param name="offset"></param>
		public void PadTo(int offset)
		{
			WriteBytes(0, offset - Offset);
		}

		/// <param name="alignment"></param>
		public void Align(int alignment)
		{
			int offset = Offset;
			WriteBytes(0, BitArithmetic.Align(offset, alignment) - offset);
		}

		/// <param name="value"></param>
		public void WriteBoolean(bool value)
		{
			WriteByte((byte)(value ? 1 : 0));
		}

		/// <param name="value"></param>
		public void WriteByte(byte value)
		{
			int num = Advance(1);
			_buffer[num] = value;
		}

		/// <param name="value"></param>
		public void WriteSByte(sbyte value)
		{
			WriteByte((byte)value);
		}

		/// <param name="value"></param>
		public void WriteDouble(double value)
		{
			int start = Advance(8);
			_buffer.WriteDouble(start, value);
		}

		/// <param name="value"></param>
		public void WriteSingle(float value)
		{
			int start = Advance(4);
			_buffer.WriteSingle(start, value);
		}

		/// <param name="value"></param>
		public void WriteInt16(short value)
		{
			WriteUInt16((ushort)value);
		}

		/// <param name="value"></param>
		public void WriteUInt16(ushort value)
		{
			int start = Advance(2);
			_buffer.WriteUInt16(start, value);
		}

		/// <param name="value"></param>
		public void WriteInt16BE(short value)
		{
			WriteUInt16BE((ushort)value);
		}

		/// <param name="value"></param>
		public void WriteUInt16BE(ushort value)
		{
			int start = Advance(2);
			_buffer.WriteUInt16BE(start, value);
		}

		/// <param name="value"></param>
		public void WriteInt32BE(int value)
		{
			WriteUInt32BE((uint)value);
		}

		/// <param name="value"></param>
		public void WriteUInt32BE(uint value)
		{
			int start = Advance(4);
			_buffer.WriteUInt32BE(start, value);
		}

		/// <param name="value"></param>
		public void WriteInt32(int value)
		{
			WriteUInt32((uint)value);
		}

		/// <param name="value"></param>
		public void WriteUInt32(uint value)
		{
			int start = Advance(4);
			_buffer.WriteUInt32(start, value);
		}

		/// <param name="value"></param>
		public void WriteInt64(long value)
		{
			WriteUInt64((ulong)value);
		}

		/// <param name="value"></param>
		public void WriteUInt64(ulong value)
		{
			int start = Advance(8);
			_buffer.WriteUInt64(start, value);
		}

		/// <param name="value"></param>
		public void WriteDecimal(decimal value)
		{
			int start = Advance(13);
			_buffer.WriteDecimal(start, value);
		}

		/// <param name="value"></param>
		public void WriteGuid(Guid value)
		{
			int start = Advance(16);
			_buffer.WriteGuid(start, value);
		}

		/// <param name="value"></param>
		public void WriteDateTime(DateTime value)
		{
			WriteInt64(value.Ticks);
		}

		/// <param name="reference"></param>
		/// <param name="isSmall"></param>
		public void WriteReference(int reference, bool isSmall)
		{
			if (isSmall)
			{
				WriteUInt16((ushort)reference);
			}
			else
			{
				WriteInt32(reference);
			}
		}

		/// <param name="value"></param>
		public unsafe void WriteUTF16(char[] value)
		{
			if (value == null)
			{
				Throw.ArgumentNull("value");
			}
			if (value.Length != 0)
			{
				fixed (char* buffer = &value[0])
				{
					WriteBytesUnchecked((byte*)buffer, value.Length * 2);
				}
			}
		}

		/// <param name="value"></param>
		public unsafe void WriteUTF16(string value)
		{
			if (value == null)
			{
				Throw.ArgumentNull("value");
			}
			fixed (char* buffer = value)
			{
				WriteBytesUnchecked((byte*)buffer, value.Length * 2);
			}
		}

		/// <param name="str"></param>
		public void WriteSerializedString(string str)
		{
			if (str == null)
			{
				WriteByte(byte.MaxValue);
			}
			else
			{
				WriteUTF8(str, 0, str.Length, true, true);
			}
		}

		/// <param name="value"></param>
		public void WriteUserString(string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			WriteCompressedInteger(BlobUtilities.GetUserStringByteLength(value.Length));
			WriteUTF16(value);
			WriteByte(BlobUtilities.GetUserStringTrailingByte(value));
		}

		/// <param name="value"></param>
		/// <param name="allowUnpairedSurrogates"></param>
		public void WriteUTF8(string value, bool allowUnpairedSurrogates)
		{
			if (value == null)
			{
				Throw.ArgumentNull("value");
			}
			WriteUTF8(value, 0, value.Length, allowUnpairedSurrogates, false);
		}

		private unsafe void WriteUTF8(string str, int start, int length, bool allowUnpairedSurrogates, bool prependSize)
		{
			fixed (char* ptr = str)
			{
				char* ptr2 = ptr + start;
				int uTF8ByteCount = BlobUtilities.GetUTF8ByteCount(ptr2, length);
				if (prependSize)
				{
					WriteCompressedInteger(uTF8ByteCount);
				}
				int start2 = Advance(uTF8ByteCount);
				_buffer.WriteUTF8(start2, ptr2, length, uTF8ByteCount, allowUnpairedSurrogates);
			}
		}

		/// <param name="value"></param>
		public void WriteCompressedSignedInteger(int value)
		{
			BlobWriterImpl.WriteCompressedSignedInteger(ref this, value);
		}

		/// <param name="value"></param>
		public void WriteCompressedInteger(int value)
		{
			BlobWriterImpl.WriteCompressedInteger(ref this, (uint)value);
		}

		/// <param name="value"></param>
		public void WriteConstant(object value)
		{
			BlobWriterImpl.WriteConstant(ref this, value);
		}

		public void Clear()
		{
			_position = _start;
		}
	}
}
