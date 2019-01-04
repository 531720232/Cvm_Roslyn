namespace System.Reflection.Metadata
{
	/// <typeparam name="THandle"></typeparam>
	public readonly struct ReservedBlob<THandle> where THandle : struct
	{
		/// <returns></returns>
		public THandle Handle
		{
			get;
		}

		/// <returns></returns>
		public Blob Content
		{
			get;
		}

		internal ReservedBlob(THandle handle, Blob content)
		{
			Handle = handle;
			Content = content;
		}

		/// <returns></returns>
		public BlobWriter CreateWriter()
		{
			return new BlobWriter(Content);
		}
	}
}
