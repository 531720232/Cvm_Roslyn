using System.Collections.Generic;

namespace System.Reflection.Metadata
{
	public sealed class HandleComparer : IEqualityComparer<Handle>, IComparer<Handle>, IEqualityComparer<EntityHandle>, IComparer<EntityHandle>
	{
		private static readonly HandleComparer s_default = new HandleComparer();

		/// <returns></returns>
		public static HandleComparer Default => s_default;

		private HandleComparer()
		{
		}

		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public bool Equals(Handle x, Handle y)
		{
			return x.Equals(y);
		}

		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public bool Equals(EntityHandle x, EntityHandle y)
		{
			return x.Equals(y);
		}

		/// <param name="obj"></param>
		/// <returns></returns>
		public int GetHashCode(Handle obj)
		{
			return obj.GetHashCode();
		}

		/// <param name="obj"></param>
		/// <returns></returns>
		public int GetHashCode(EntityHandle obj)
		{
			return obj.GetHashCode();
		}

		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public int Compare(Handle x, Handle y)
		{
			return Handle.Compare(x, y);
		}

		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public int Compare(EntityHandle x, EntityHandle y)
		{
			return EntityHandle.Compare(x, y);
		}
	}
}
