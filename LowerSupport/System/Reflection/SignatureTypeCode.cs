namespace System.Reflection.Metadata
{
	/// <summary>Specifies constants that define type codes used in signature encoding.</summary>
	public enum SignatureTypeCode : byte
	{
		/// <summary>Represents an invalid or uninitialized type code. It will not appear in valid signatures.</summary>
		/// <returns></returns>
		Invalid = 0,
		/// <summary>Represents <see cref="T:System.Void"></see> in signatures.</summary>
		/// <returns></returns>
		Void = 1,
		/// <summary>Represents a <see cref="T:System.Boolean"></see> in signatures.</summary>
		/// <returns></returns>
		Boolean = 2,
		/// <summary>Represents a <see cref="T:System.Char"></see> in signatures.</summary>
		/// <returns></returns>
		Char = 3,
		/// <summary>Represents an <see cref="T:System.SByte"></see> in signatures.</summary>
		/// <returns></returns>
		SByte = 4,
		/// <summary>Represents a <see cref="T:System.Byte"></see> in signatures.</summary>
		/// <returns></returns>
		Byte = 5,
		/// <summary>Represents an <see cref="T:System.Int16"></see> in signatures.</summary>
		/// <returns></returns>
		Int16 = 6,
		/// <summary>Represents a <see cref="T:System.UInt16"></see> in signatures.</summary>
		/// <returns></returns>
		UInt16 = 7,
		/// <summary>Represents an <see cref="T:System.Int32"></see> in signatures.</summary>
		/// <returns></returns>
		Int32 = 8,
		/// <summary>Represents a <see cref="T:System.UInt32"></see> in signatures.</summary>
		/// <returns></returns>
		UInt32 = 9,
		/// <summary>Represents an <see cref="T:System.Int64"></see> in signatures.</summary>
		/// <returns></returns>
		Int64 = 10,
		/// <summary>Represents a <see cref="T:System.UInt64"></see> in signatures.</summary>
		/// <returns></returns>
		UInt64 = 11,
		/// <summary>Represents a <see cref="T:System.Single"></see> in signatures.</summary>
		/// <returns></returns>
		Single = 12,
		/// <summary>Represents a <see cref="T:System.Double"></see> in signatures.</summary>
		/// <returns></returns>
		Double = 13,
		/// <summary>Represents a <see cref="T:System.String"></see> in signatures.</summary>
		/// <returns></returns>
		String = 14,
		/// <summary>Represents an unmanaged pointer in signatures. It is followed in the blob by the signature encoding of the underlying type.</summary>
		/// <returns></returns>
		Pointer = 0xF,
		/// <summary>Represents managed pointers (byref return values and parameters) in signatures. It is followed in the blob by the signature encoding of the underlying type.</summary>
		/// <returns></returns>
		ByReference = 0x10,
		/// <summary>Represents a generic type parameter used within a signature.</summary>
		/// <returns></returns>
		GenericTypeParameter = 19,
		/// <summary>Represents a generalized <see cref="T:System.Array"></see> in signatures.</summary>
		/// <returns></returns>
		Array = 20,
		/// <summary>Represents the instantiation of a generic type in signatures.</summary>
		/// <returns></returns>
		GenericTypeInstance = 21,
		/// <summary>Represents a typed reference in signatures.</summary>
		/// <returns></returns>
		TypedReference = 22,
		/// <summary>Represents an <see cref="T:System.IntPtr"></see> in signatures.</summary>
		/// <returns></returns>
		IntPtr = 24,
		/// <summary>Represents a <see cref="T:System.UIntPtr"></see> in signatures.</summary>
		/// <returns></returns>
		UIntPtr = 25,
		/// <summary>Represents function pointer types in signatures.</summary>
		/// <returns></returns>
		FunctionPointer = 27,
		/// <summary>Represents an <see cref="T:System.Object"></see> in signatures.</summary>
		/// <returns></returns>
		Object = 28,
		/// <summary>Represents a single dimensional <see cref="T:System.Array"></see> with a lower bound of 0.</summary>
		/// <returns></returns>
		SZArray = 29,
		/// <summary>Represents a generic method parameter used within a signature.</summary>
		/// <returns></returns>
		GenericMethodParameter = 30,
		/// <summary>Represents a custom modifier applied to a type within a signature that the caller must understand.</summary>
		/// <returns></returns>
		RequiredModifier = 0x1F,
		/// <summary>Represents a custom modifier applied to a type within a signature that the caller can ignore.</summary>
		/// <returns></returns>
		OptionalModifier = 0x20,
		/// <summary>Precedes a type <see cref="EntityHandle"></see> in signatures.</summary>
		/// <returns></returns>
		TypeHandle = 0x40,
		/// <summary>Represents a marker to indicate the end of fixed arguments and the beginning of variable arguments.</summary>
		/// <returns></returns>
		Sentinel = 65,
		/// <summary>Represents a local variable that is pinned by garbage collector.</summary>
		/// <returns></returns>
		Pinned = 69
	}
}
