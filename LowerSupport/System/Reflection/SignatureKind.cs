namespace System.Reflection.Metadata
{
	/// <summary>Specifies the signature kind. The underlying values of the fields in this type correspond to the representation in the leading signature byte represented by a <see cref="T:System.Reflection.Metadata.SignatureHeader"></see> structure.</summary>
	public enum SignatureKind : byte
	{
		/// <summary>A method reference, method definition, or standalone method signature.</summary>
		/// <returns></returns>
		Method = 0,
		/// <summary>A field signature.</summary>
		/// <returns></returns>
		Field = 6,
		/// <summary>A local variables signature.</summary>
		/// <returns></returns>
		LocalVariables = 7,
		/// <summary>A property signature.</summary>
		/// <returns></returns>
		Property = 8,
		/// <summary>A method specification signature.</summary>
		/// <returns></returns>
		MethodSpecification = 10
	}
}
