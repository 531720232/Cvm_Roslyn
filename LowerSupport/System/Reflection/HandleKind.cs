namespace System.Reflection.Metadata
{
	public enum HandleKind : byte
	{
		/// <returns></returns>
		ModuleDefinition = 0,
		/// <returns></returns>
		TypeReference = 1,
		/// <returns></returns>
		TypeDefinition = 2,
		/// <returns></returns>
		FieldDefinition = 4,
		/// <returns></returns>
		MethodDefinition = 6,
		/// <returns></returns>
		Parameter = 8,
		/// <returns></returns>
		InterfaceImplementation = 9,
		/// <returns></returns>
		MemberReference = 10,
		/// <returns></returns>
		Constant = 11,
		/// <returns></returns>
		CustomAttribute = 12,
		/// <returns></returns>
		DeclarativeSecurityAttribute = 14,
		/// <returns></returns>
		StandaloneSignature = 17,
		/// <returns></returns>
		EventDefinition = 20,
		/// <returns></returns>
		PropertyDefinition = 23,
		/// <returns></returns>
		MethodImplementation = 25,
		/// <returns></returns>
		ModuleReference = 26,
		/// <returns></returns>
		TypeSpecification = 27,
		/// <returns></returns>
		AssemblyDefinition = 0x20,
		/// <returns></returns>
		AssemblyFile = 38,
		/// <returns></returns>
		AssemblyReference = 35,
		/// <returns></returns>
		ExportedType = 39,
		/// <returns></returns>
		GenericParameter = 42,
		/// <returns></returns>
		MethodSpecification = 43,
		/// <returns></returns>
		GenericParameterConstraint = 44,
		/// <returns></returns>
		ManifestResource = 40,
		/// <returns></returns>
		Document = 48,
		/// <returns></returns>
		MethodDebugInformation = 49,
		/// <returns></returns>
		LocalScope = 50,
		/// <returns></returns>
		LocalVariable = 51,
		/// <returns></returns>
		LocalConstant = 52,
		/// <returns></returns>
		ImportScope = 53,
		/// <returns></returns>
		CustomDebugInformation = 55,
		/// <returns></returns>
		NamespaceDefinition = 124,
		/// <returns></returns>
		UserString = 112,
		/// <returns></returns>
		String = 120,
		/// <returns></returns>
		Blob = 113,
		/// <returns></returns>
		Guid = 114
	}
}
