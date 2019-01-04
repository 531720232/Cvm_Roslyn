using System.IO;
using System.Runtime.InteropServices;

namespace System.Reflection.Internal
{
	internal static class StreamExtensions
	{
		internal const int StreamCopyBufferSize = 81920;

		internal unsafe static void CopyTo(this Stream source, byte* destination, int size)
		{
			byte[] array = new byte[Math.Min(81920, size)];
			while (true)
			{
				if (size <= 0)
				{
					return;
				}
				int num = Math.Min(size, array.Length);
				int num2 = source.Read(array, 0, num);
				if (num2 <= 0 || num2 > num)
				{
					break;
				}
				Marshal.Copy(array, 0, (IntPtr)(void*)destination, num2);
				destination += num2;
				size -= num2;
			}
			throw new IOException();
		}

		internal static int TryReadAll(this Stream stream, byte[] buffer, int offset, int count)
		{
			int num = 0;
			int i;
			for (i = 0; i < count; i += num)
			{
				num = stream.Read(buffer, offset + i, count - i);
				if (num == 0)
				{
					break;
				}
			}
			return i;
		}

		internal static int GetAndValidateSize(Stream stream, int size, string streamParameterName)
		{
			long num = stream.Length - stream.Position;
			if (size < 0 || size > num)
			{
				throw new ArgumentOutOfRangeException("size");
			}
			if (size != 0)
			{
				return size;
			}
			if (num > 2147483647)
			{
				throw new ArgumentException();
			}
			return (int)num;
		}
	}
}
