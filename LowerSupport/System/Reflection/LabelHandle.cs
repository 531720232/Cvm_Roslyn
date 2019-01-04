namespace System.Reflection.Metadata.Ecma335
{
	public readonly struct LabelHandle : IEquatable<LabelHandle>
	{
		/// <returns></returns>
		public int Id
		{
			get;
		}

		/// <returns></returns>
		public bool IsNil => Id == 0;

		internal LabelHandle(int id)
		{
			Id = id;
		}

		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(LabelHandle other)
		{
			return Id == other.Id;
		}

		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is LabelHandle)
			{
				return Equals((LabelHandle)obj);
			}
			return false;
		}

		/// <returns></returns>
		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}

		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator ==(LabelHandle left, LabelHandle right)
		{
			return left.Equals(right);
		}

		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator !=(LabelHandle left, LabelHandle right)
		{
			return !left.Equals(right);
		}
	}
}
