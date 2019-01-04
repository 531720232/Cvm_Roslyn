namespace System.Reflection.Metadata
{
	/// <summary>Specifies how arguments in a given signature are passed from the caller to the callee. The underlying values of the fields in this type correspond to the representation in the leading signature byte represented by a <see cref="T:System.Reflection.Metadata.SignatureHeader"></see> structure.</summary>
	public enum SignatureCallingConvention : byte
	{
		/// <summary>A managed calling convention with a fixed-length argument list.</summary>
		/// <returns></returns>
		Default,
		/// <summary>An unmanaged C/C++ style calling convention where the call stack is cleaned by the caller.</summary>
		/// <returns></returns>
		CDecl,
		/// <summary>An unmanaged calling convention where the call stack is cleaned up by the callee.</summary>
		/// <returns></returns>
		StdCall,
		/// <summary>An unmanaged C++ style calling convention for calling instance member functions with a fixed argument list.</summary>
		/// <returns></returns>
		ThisCall,
		/// <summary>An unmanaged calling convention where arguments are passed in registers when possible.</summary>
		/// <returns></returns>
		FastCall,
		/// <summary>A managed calling convention for passing extra arguments.</summary>
		/// <returns></returns>
		VarArgs
	}
}
