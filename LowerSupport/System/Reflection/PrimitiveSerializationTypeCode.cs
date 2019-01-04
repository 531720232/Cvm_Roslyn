namespace System.Reflection.Metadata
{
	/// <summary>Specifies constants that define the type codes used to encode types of primitive values in a <see cref="T:System.Reflection.Metadata.CustomAttribute"></see> value blob.</summary>
	public enum PrimitiveSerializationTypeCode : byte
	{
		/// <summary>A <see cref="T:System.Boolean"></see> type.</summary>
		/// <returns></returns>
		Boolean = 2,
		/// <summary>An unsigned 1-byte integer type.</summary>
		/// <returns></returns>
		Byte = 5,
		/// <summary>A signed 1-byte integer type.</summary>
		/// <returns></returns>
		SByte = 4,
		/// <summary>A <see cref="T:System.Char"></see> type.</summary>
		/// <returns></returns>
		Char = 3,
		/// <summary>A signed 2-byte integer type.</summary>
		/// <returns></returns>
		Int16 = 6,
		/// <summary>An unsigned 2-byte integer type.</summary>
		/// <returns></returns>
		UInt16 = 7,
		/// <summary>A signed 4-byte integer type.</summary>
		/// <returns></returns>
		Int32 = 8,
		/// <summary>An unsigned 4-byte integer type.</summary>
		/// <returns></returns>
		UInt32 = 9,
		/// <summary>A signed 8-byte integer type.</summary>
		/// <returns></returns>
		Int64 = 10,
		/// <summary>An unsigned 8-byte integer type.</summary>
		/// <returns></returns>
		UInt64 = 11,
		/// <summary>A 4-byte floating point type.</summary>
		/// <returns></returns>
		Single = 12,
		/// <summary>An 8-byte floating point type.</summary>
		/// <returns></returns>
		Double = 13,
		/// <summary>A <see cref="T:System.String"></see> type.</summary>
		/// <returns></returns>
		String = 14
	}
}
