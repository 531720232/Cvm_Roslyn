namespace System.Reflection.Metadata.Ecma335
{
	public readonly struct NamedArgumentsEncoder
	{
		/// <returns></returns>
		public BlobBuilder Builder
		{
			get;
		}

		/// <param name="builder"></param>
		public NamedArgumentsEncoder(BlobBuilder builder)
		{
			Builder = builder;
		}

		/// <param name="isField"></param>
		/// <param name="type"></param>
		/// <param name="name"></param>
		/// <param name="literal"></param>
		public void AddArgument(bool isField, out NamedArgumentTypeEncoder type, out NameEncoder name, out LiteralEncoder literal)
		{
			Builder.WriteByte((byte)(isField ? 83 : 84));
			type = new NamedArgumentTypeEncoder(Builder);
			name = new NameEncoder(Builder);
			literal = new LiteralEncoder(Builder);
		}

		/// <param name="isField"></param>
		/// <param name="type"></param>
		/// <param name="name"></param>
		/// <param name="literal"></param>
		public void AddArgument(bool isField, Action<NamedArgumentTypeEncoder> type, Action<NameEncoder> name, Action<LiteralEncoder> literal)
		{
			if (type == null)
			{
				Throw.ArgumentNull("type");
			}
			if (name == null)
			{
				Throw.ArgumentNull("name");
			}
			if (literal == null)
			{
				Throw.ArgumentNull("literal");
			}
			AddArgument(isField, out NamedArgumentTypeEncoder type2, out NameEncoder name2, out LiteralEncoder literal2);
			type(type2);
			name(name2);
			literal(literal2);
		}
	}
}
