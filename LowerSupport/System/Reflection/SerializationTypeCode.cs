namespace System.Reflection.Metadata
{
	/// <summary>Specifies type codes used to encode the types of values in a <see cref="T:System.Reflection.Metadata.CustomAttributeValue`1"></see> blob.</summary>
	public enum SerializationTypeCode : byte
	{
		/// <summary>A value equivalent to <see cref="SignatureTypeCode.Invalid"></see>.</summary>
		/// <returns></returns>
		Invalid = 0,
		/// <summary>A value equivalent to <see cref="SignatureTypeCode.Boolean"></see>.</summary>
		/// <returns></returns>
		Boolean = 2,
		/// <summary>A value equivalent to <see cref="SignatureTypeCode.Char"></see>.</summary>
		/// <returns></returns>
		Char = 3,
		/// <summary>A value equivalent to <see cref="SignatureTypeCode.SByte"></see>.</summary>
		/// <returns></returns>
		SByte = 4,
		/// <summary>A value equivalent to <see cref="SignatureTypeCode.Byte"></see>.</summary>
		/// <returns></returns>
		Byte = 5,
		/// <summary>A value equivalent to <see cref="SignatureTypeCode.Int16"></see>.</summary>
		/// <returns></returns>
		Int16 = 6,
		/// <summary>A value equivalent to <see cref="SignatureTypeCode.UInt16"></see>.</summary>
		/// <returns></returns>
		UInt16 = 7,
		/// <summary>A value equivalent to <see cref="SignatureTypeCode.Int32"></see>.</summary>
		/// <returns></returns>
		Int32 = 8,
		/// <summary>A value equivalent to <see cref="SignatureTypeCode.UInt32"></see>.</summary>
		/// <returns></returns>
		UInt32 = 9,
		/// <summary>A value equivalent to <see cref="SignatureTypeCode.Int64"></see>.</summary>
		/// <returns></returns>
		Int64 = 10,
		/// <summary>A value equivalent to <see cref="SignatureTypeCode.UInt64"></see>.</summary>
		/// <returns></returns>
		UInt64 = 11,
		/// <summary>A value equivalent to <see cref="SignatureTypeCode.Single"></see>.</summary>
		/// <returns></returns>
		Single = 12,
		/// <summary>A value equivalent to <see cref="SignatureTypeCode.Double"></see>.</summary>
		/// <returns></returns>
		Double = 13,
		/// <summary>A value equivalent to <see cref="SignatureTypeCode.String"></see>.</summary>
		/// <returns></returns>
		String = 14,
		/// <summary>A value equivalent to <see cref="SignatureTypeCode.SZArray"></see>.</summary>
		/// <returns></returns>
		SZArray = 29,
		/// <summary>The attribute argument is a <see cref="T:System.Type"></see> instance.</summary>
		/// <returns></returns>
		Type = 80,
		/// <summary>The attribute argument is &amp;quot;boxed&amp;quot; (passed to a parameter, field, or property of type object) and carries type information in the attribute blob.</summary>
		/// <returns></returns>
		TaggedObject = 81,
		/// <summary>The attribute argument is an Enum instance.</summary>
		/// <returns></returns>
		Enum = 85
	}
}
