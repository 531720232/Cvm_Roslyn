namespace System.Reflection.Metadata
{
	/// <summary>Specifies additional flags that can be applied to method signatures. The underlying values of the fields in this type correspond to the representation in the leading signature byte represented by a <see cref="T:System.Reflection.Metadata.SignatureHeader"></see> structure.</summary>
	[Flags]
	public enum SignatureAttributes : byte
	{
		/// <summary>No flags.</summary>
		/// <returns></returns>
		None = 0x0,
		/// <summary>A generic method.</summary>
		/// <returns></returns>
		Generic = 0x10,
		/// <summary>An instance method.</summary>
		/// <returns></returns>
		Instance = 0x20,
		/// <summary>Indicates the first explicitly declared parameter that represents the instance pointer.</summary>
		/// <returns></returns>
		ExplicitThis = 0x40
	}
}
