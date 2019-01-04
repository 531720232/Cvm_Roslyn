namespace System.Reflection.Metadata.Ecma335
{
	public readonly struct LiteralEncoder
	{
		/// <returns></returns>
		public BlobBuilder Builder
		{
			get;
		}

		/// <param name="builder"></param>
		public LiteralEncoder(BlobBuilder builder)
		{
			Builder = builder;
		}

		/// <returns></returns>
		public VectorEncoder Vector()
		{
			return new VectorEncoder(Builder);
		}

		/// <param name="arrayType"></param>
		/// <param name="vector"></param>
		public void TaggedVector(out CustomAttributeArrayTypeEncoder arrayType, out VectorEncoder vector)
		{
			arrayType = new CustomAttributeArrayTypeEncoder(Builder);
			vector = new VectorEncoder(Builder);
		}

		/// <param name="arrayType"></param>
		/// <param name="vector"></param>
		public void TaggedVector(Action<CustomAttributeArrayTypeEncoder> arrayType, Action<VectorEncoder> vector)
		{
			if (arrayType == null)
			{
				Throw.ArgumentNull("arrayType");
			}
			if (vector == null)
			{
				Throw.ArgumentNull("vector");
			}
			TaggedVector(out CustomAttributeArrayTypeEncoder arrayType2, out VectorEncoder vector2);
			arrayType(arrayType2);
			vector(vector2);
		}

		/// <returns></returns>
		public ScalarEncoder Scalar()
		{
			return new ScalarEncoder(Builder);
		}

		/// <param name="type"></param>
		/// <param name="scalar"></param>
		public void TaggedScalar(out CustomAttributeElementTypeEncoder type, out ScalarEncoder scalar)
		{
			type = new CustomAttributeElementTypeEncoder(Builder);
			scalar = new ScalarEncoder(Builder);
		}

		/// <param name="type"></param>
		/// <param name="scalar"></param>
		public void TaggedScalar(Action<CustomAttributeElementTypeEncoder> type, Action<ScalarEncoder> scalar)
		{
			if (type == null)
			{
				Throw.ArgumentNull("type");
			}
			if (scalar == null)
			{
				Throw.ArgumentNull("scalar");
			}
			TaggedScalar(out CustomAttributeElementTypeEncoder type2, out ScalarEncoder scalar2);
			type(type2);
			scalar(scalar2);
		}
	}
}
