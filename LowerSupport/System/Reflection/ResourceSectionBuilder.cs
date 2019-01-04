using System.Reflection.Metadata;

namespace System.Reflection.PortableExecutable
{
	public abstract class ResourceSectionBuilder
	{
		/// <param name="builder"></param>
		/// <param name="location"></param>
		protected internal abstract void Serialize(BlobBuilder builder, SectionLocation location);
	}
}
