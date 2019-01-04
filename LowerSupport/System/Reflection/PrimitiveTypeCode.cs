namespace System.Reflection.Metadata
{
	/// <summary>Specifies constants that define primitive types found in metadata signatures.</summary>
	public enum PrimitiveTypeCode : byte
	{
		/// <summary>A <see cref="T:System.Boolean"></see> type.</summary>
		/// <returns></returns>
		Boolean = 2,
		/// <summary>A <see cref="T:System.Byte"></see> type.</summary>
		/// <returns></returns>
		Byte = 5,
		/// <summary>An <see cref="T:System.SByte"></see> type.</summary>
		/// <returns></returns>
		SByte = 4,
		/// <summary>A <see cref="T:System.Char"></see> type.</summary>
		/// <returns></returns>
		Char = 3,
		/// <summary>A <see cref="T:System.Int16"></see> type.</summary>
		/// <returns></returns>
		Int16 = 6,
		/// <summary>A <see cref="T:System.UInt16"></see> type.</summary>
		/// <returns></returns>
		UInt16 = 7,
		/// <summary>A <see cref="T:System.Int32"></see> type.</summary>
		/// <returns></returns>
		Int32 = 8,
		/// <summary>A <see cref="T:System.UInt32"></see> type.</summary>
		/// <returns></returns>
		UInt32 = 9,
		/// <summary>A <see cref="T:System.Int64"></see> type.</summary>
		/// <returns></returns>
		Int64 = 10,
		/// <summary>A <see cref="T:System.UInt64"></see> type.</summary>
		/// <returns></returns>
		UInt64 = 11,
		/// <summary>A <see cref="T:System.Single"></see> type.</summary>
		/// <returns></returns>
		Single = 12,
		/// <summary>A <see cref="T:System.Double"></see> type.</summary>
		/// <returns></returns>
		Double = 13,
		/// <summary>A <see cref="T:System.IntPtr"></see> type.</summary>
		/// <returns></returns>
		IntPtr = 24,
		/// <summary>A <see cref="T:System.UIntPtr"></see> type.</summary>
		/// <returns></returns>
		UIntPtr = 25,
		/// <summary>An <see cref="T:System.Object"></see> type.</summary>
		/// <returns></returns>
		Object = 28,
		/// <summary>An <see cref="T:System.Single"></see> type.</summary>
		/// <returns></returns>
		String = 14,
		/// <summary>A typed reference.</summary>
		/// <returns></returns>
		TypedReference = 22,
		/// <summary>A <see cref="T:System.Void"></see> type.</summary>
		/// <returns></returns>
		Void = 1
	}
}
