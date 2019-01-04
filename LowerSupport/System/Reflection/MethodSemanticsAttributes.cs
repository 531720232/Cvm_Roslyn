namespace System.Reflection
{
	[Flags]
	public enum MethodSemanticsAttributes
	{
		/// <returns></returns>
		Setter = 0x1,
		/// <returns></returns>
		Getter = 0x2,
		/// <returns></returns>
		Other = 0x4,
		/// <returns></returns>
		Adder = 0x8,
		/// <returns></returns>
		Remover = 0x10,
		/// <returns></returns>
		Raiser = 0x20
	}
}
