namespace System.Reflection.Metadata.Ecma335
{
	public readonly struct ScalarEncoder
	{
		/// <returns></returns>
		public BlobBuilder Builder
		{
			get;
		}

		/// <param name="builder"></param>
		public ScalarEncoder(BlobBuilder builder)
		{
			Builder = builder;
		}

		public void NullArray()
		{
			Builder.WriteInt32(-1);
		}

		/// <param name="value"></param>
		public void Constant(object value)
		{
			string text = value as string;
			if (text != null || value == null)
			{
				String(text);
			}
			else
			{
				Builder.WriteConstant(value);
			}
		}

		/// <param name="serializedTypeName"></param>
		public void SystemType(string serializedTypeName)
		{
			if (serializedTypeName != null && serializedTypeName.Length == 0)
			{
				Throw.ArgumentEmptyString("serializedTypeName");
			}
			String(serializedTypeName);
		}

		private void String(string value)
		{
			Builder.WriteSerializedString(value);
		}
	}
}
