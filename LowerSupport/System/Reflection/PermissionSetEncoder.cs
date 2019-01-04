using System.Collections.Immutable;

namespace System.Reflection.Metadata.Ecma335
{
	public readonly struct PermissionSetEncoder
	{
		/// <returns></returns>
		public BlobBuilder Builder
		{
			get;
		}

		/// <param name="builder"></param>
		public PermissionSetEncoder(BlobBuilder builder)
		{
			Builder = builder;
		}

		/// <param name="typeName"></param>
		/// <param name="encodedArguments"></param>
		/// <returns></returns>
		public PermissionSetEncoder AddPermission(string typeName, ImmutableArray<byte> encodedArguments)
		{
			if (typeName == null)
			{
				Throw.ArgumentNull("typeName");
			}
			if (encodedArguments.IsDefault)
			{
				Throw.ArgumentNull("encodedArguments");
			}
			if (encodedArguments.Length > 536870911)
			{
				Throw.BlobTooLarge("encodedArguments");
			}
			Builder.WriteSerializedString(typeName);
			Builder.WriteCompressedInteger(encodedArguments.Length);
			Builder.WriteBytes(encodedArguments);
			return this;
		}

		/// <param name="typeName"></param>
		/// <param name="encodedArguments"></param>
		/// <returns></returns>
		public PermissionSetEncoder AddPermission(string typeName, BlobBuilder encodedArguments)
		{
			if (typeName == null)
			{
				Throw.ArgumentNull("typeName");
			}
			if (encodedArguments == null)
			{
				Throw.ArgumentNull("encodedArguments");
			}
			if (encodedArguments.Count > 536870911)
			{
				Throw.BlobTooLarge("encodedArguments");
			}
			Builder.WriteSerializedString(typeName);
			Builder.WriteCompressedInteger(encodedArguments.Count);
			encodedArguments.WriteContentTo(Builder);
			return this;
		}
	}
}
