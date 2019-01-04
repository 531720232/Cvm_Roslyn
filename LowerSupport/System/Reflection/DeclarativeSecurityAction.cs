namespace System.Reflection
{
	public enum DeclarativeSecurityAction : short
	{
		/// <returns></returns>
		None = 0,
		/// <returns></returns>
		Demand = 2,
		/// <returns></returns>
		Assert = 3,
		/// <returns></returns>
		Deny = 4,
		/// <returns></returns>
		PermitOnly = 5,
		/// <returns></returns>
		LinkDemand = 6,
		/// <returns></returns>
		InheritanceDemand = 7,
		/// <returns></returns>
		RequestMinimum = 8,
		/// <returns></returns>
		RequestOptional = 9,
		/// <returns></returns>
		RequestRefuse = 10
	}
}
