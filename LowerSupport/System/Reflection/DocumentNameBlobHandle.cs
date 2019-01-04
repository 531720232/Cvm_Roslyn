namespace System.Reflection.Metadata
{
	public readonly struct DocumentNameBlobHandle : IEquatable<DocumentNameBlobHandle>
	{
		private readonly int _heapOffset;

		/// <returns></returns>
		public bool IsNil => _heapOffset == 0;

		private DocumentNameBlobHandle(int heapOffset)
		{
			_heapOffset = heapOffset;
		}

		internal static DocumentNameBlobHandle FromOffset(int heapOffset)
		{
			return new DocumentNameBlobHandle(heapOffset);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator BlobHandle(DocumentNameBlobHandle handle)
		{
			return BlobHandle.FromOffset(handle._heapOffset);
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator DocumentNameBlobHandle(BlobHandle handle)
		{
			if (handle.IsVirtual)
			{
				Throw.InvalidCast();
			}
			return FromOffset(handle.GetHeapOffset());
		}

		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is DocumentNameBlobHandle)
			{
				return Equals((DocumentNameBlobHandle)obj);
			}
			return false;
		}

		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(DocumentNameBlobHandle other)
		{
			return _heapOffset == other._heapOffset;
		}

		/// <returns></returns>
		public override int GetHashCode()
		{
			return _heapOffset;
		}

		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator ==(DocumentNameBlobHandle left, DocumentNameBlobHandle right)
		{
			return left.Equals(right);
		}

		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator !=(DocumentNameBlobHandle left, DocumentNameBlobHandle right)
		{
			return !left.Equals(right);
		}
	}
}
