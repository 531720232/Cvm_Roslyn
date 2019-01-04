using System.Text;

namespace System.Reflection.Metadata
{
	/// <summary>Represents the signature characteristics specified by the leading byte of signature blobs.</summary>
	public struct SignatureHeader : IEquatable<SignatureHeader>
	{
		private byte _rawValue;

		/// <summary>Gets the mask value for the calling convention or signature kind. The default <see cref="F:System.Reflection.Metadata.SignatureHeader.CallingConventionOrKindMask"></see> value is 15 (0x0F).</summary>
		/// <returns></returns>
		public const byte CallingConventionOrKindMask = 15;

		private const byte maxCallingConvention = 5;

		/// <summary>Gets the raw value of the header byte.</summary>
		/// <returns>The raw value of the header byte.</returns>
		public byte RawValue => _rawValue;

		/// <summary>Gets the calling convention.</summary>
		/// <returns>The calling convention.</returns>
		public SignatureCallingConvention CallingConvention
		{
			get
			{
				int num = _rawValue & 0xF;
				if (num > 5)
				{
					return SignatureCallingConvention.Default;
				}
				return (SignatureCallingConvention)num;
			}
		}

		/// <summary>Gets the signature kind.</summary>
		/// <returns>The signature kind.</returns>
		public SignatureKind Kind
		{
			get
			{
				int num = _rawValue & 0xF;
				if (num <= 5)
				{
					return SignatureKind.Method;
				}
				return (SignatureKind)num;
			}
		}

		/// <summary>Gets the signature attributes.</summary>
		/// <returns>The attributes.</returns>
		public SignatureAttributes Attributes => (SignatureAttributes)(_rawValue & -16);

		/// <summary>Gets a value that indicates whether this <see cref="T:System.Reflection.Metadata.SignatureHeader"></see> structure has the <see cref="F:System.Reflection.Metadata.SignatureAttributes.ExplicitThis"></see> signature attribute.</summary>
		/// <returns>true if the <see cref="System.Reflection.Metadata.SignatureAttributes.ExplicitThis"></see> attribute is present; otherwise, false.</returns>
		public bool HasExplicitThis => (_rawValue & 0x40) != 0;

		/// <summary>Gets a value that indicates whether this <see cref="T:System.Reflection.Metadata.SignatureHeader"></see> structure has the <see cref="F:System.Reflection.Metadata.SignatureAttributes.Instance"></see> signature attribute.</summary>
		/// <returns>true if the <see cref="System.Reflection.Metadata.SignatureAttributes.Instance"></see> attribute is present; otherwise, false.</returns>
		public bool IsInstance => (_rawValue & 0x20) != 0;

		/// <summary>Gets a value that indicates whether this <see cref="T:System.Reflection.Metadata.SignatureHeader"></see> structure has the <see cref="F:System.Reflection.Metadata.SignatureAttributes.Generic"></see> signature attribute.</summary>
		/// <returns>true if the <see cref="System.Reflection.Metadata.SignatureAttributes.Generic"></see> attribute is present; otherwise, false.</returns>
		public bool IsGeneric => (_rawValue & 0x10) != 0;

		/// <summary>Initializes a new instance of the <see cref="T:System.Reflection.Metadata.SignatureHeader"></see> structure using the specified byte value.</summary>
		/// <param name="rawValue">The byte.</param>
		public SignatureHeader(byte rawValue)
		{
			_rawValue = rawValue;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Reflection.Metadata.SignatureHeader"></see> structure using the specified signature kind, calling convention and signature attributes.</summary>
		/// <param name="kind">The signature kind.</param>
		/// <param name="convention">The calling convention.</param>
		/// <param name="attributes">The signature attributes.</param>
		public SignatureHeader(SignatureKind kind, SignatureCallingConvention convention, SignatureAttributes attributes)
		{
			this = new SignatureHeader((byte)((int)kind | (int)convention | (int)attributes));
		}

		/// <summary>Compares the specified object with this <see cref="T:System.Reflection.Metadata.SignatureHeader"></see> for equality.</summary>
		/// <param name="obj">The object to compare.</param>
		/// <returns>true if the objects are equal; otherwise, false.</returns>
		public override bool Equals(object obj)
		{
			if (obj is SignatureHeader)
			{
				return Equals((SignatureHeader)obj);
			}
			return false;
		}

		/// <summary>Compares two <see cref="T:System.Reflection.Metadata.SignatureHeader"></see> values for equality.</summary>
		/// <param name="other">The value to compare.</param>
		/// <returns>true if the values are equal; otherwise, false.</returns>
		public bool Equals(SignatureHeader other)
		{
			return _rawValue == other._rawValue;
		}

		/// <summary>Gets a hash code for the current object.</summary>
		/// <returns>A hash code for the current object.</returns>
		public override int GetHashCode()
		{
			return _rawValue;
		}

		/// <summary>Compares two <see cref="T:System.Reflection.Metadata.SignatureHeader"></see> values for equality.</summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns>true if the values are equal; otherwise, false.</returns>
		public static bool operator ==(SignatureHeader left, SignatureHeader right)
		{
			return left._rawValue == right._rawValue;
		}

		/// <summary>Determines whether two <see cref="T:System.Reflection.Metadata.SignatureHeader"></see> values are unequal.</summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns>true if the values are unequal; otherwise, false.</returns>
		public static bool operator !=(SignatureHeader left, SignatureHeader right)
		{
			return left._rawValue != right._rawValue;
		}

		/// <summary>Returns a string that represents the current object.</summary>
		/// <returns>A string that represents the current object.</returns>
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(Kind.ToString());
			if (Kind == SignatureKind.Method)
			{
				stringBuilder.Append(',');
				stringBuilder.Append(CallingConvention.ToString());
			}
			if (Attributes != 0)
			{
				stringBuilder.Append(',');
				stringBuilder.Append(Attributes.ToString());
			}
			return stringBuilder.ToString();
		}
	}
}
