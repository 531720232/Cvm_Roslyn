namespace System.Reflection.Metadata
{
	public readonly struct Blob
	{
		internal readonly byte[] Buffer;

		internal readonly int Start;

		/// <returns></returns>
		public int Length
		{
			get;
		}

		/// <returns></returns>
		public bool IsDefault => Buffer == null;

		internal Blob(byte[] buffer, int start, int length)
		{
			Buffer = buffer;
			Start = start;
			Length = length;
		}

		/// <returns></returns>
		public ArraySegment<byte> GetBytes()
		{
			return new ArraySegment<byte>(Buffer, Start, Length);
		}
	}
}
