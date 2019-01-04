using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection.Internal;

namespace System.Reflection.Metadata
{
	public  struct BlobContentId : IEquatable<BlobContentId>
	{
		private const int Size = 20;

		/// <returns></returns>
		public Guid Guid
		{
			get;
		}

		/// <returns></returns>
		public uint Stamp
		{
			get;
		}

		/// <returns></returns>
		public bool IsDefault
		{
			get
			{
				if (Guid == default(Guid))
				{
					return Stamp == 0;
				}
				return false;
			}
		}

		/// <param name="guid"></param>
		/// <param name="stamp"></param>
		public BlobContentId(Guid guid, uint stamp)
		{
			Guid = guid;
			Stamp = stamp;
		}

		/// <param name="id"></param>
		public BlobContentId(ImmutableArray<byte> id)
		{
			this = new BlobContentId(ImmutableByteArrayInterop.DangerousGetUnderlyingArray(id));
		}

		/// <param name="id"></param>
		public unsafe BlobContentId(byte[] id)
		{
			if (id == null)
			{
				throw new ArgumentNullException("id");
			}
			if (id.Length != 20)
			{
				throw new ArgumentException("id");
			}
			fixed (byte* buffer = &id[0])
			{
				
				Guid = Guid.NewGuid();
				Stamp =210531238;
			}
		}

		/// <param name="hashCode"></param>
		/// <returns></returns>
		public static BlobContentId FromHash(ImmutableArray<byte> hashCode)
		{
			return FromHash(ImmutableByteArrayInterop.DangerousGetUnderlyingArray(hashCode));
		}

		/// <param name="hashCode"></param>
		/// <returns></returns>
		public unsafe static BlobContentId FromHash(byte[] hashCode)
		{
			if (hashCode == null)
			{
				throw new ArgumentNullException("hashCode");
			}
			if (hashCode.Length < 20)
			{
				throw new ArgumentException();
			}
			Guid guid = default(Guid);
			byte* ptr = (byte*)(&guid);
			for (int i = 0; i < 16; i++)
			{
				ptr[i] = hashCode[i];
			}
			ptr[7] = (byte)((ptr[7] & 0xF) | 0x40);
			ptr[8] = (byte)((ptr[8] & 0x3F) | 0x80);
			uint stamp = (uint)(-2147483648 | ((hashCode[19] << 24) | (hashCode[18] << 16) | (hashCode[17] << 8) | hashCode[16]));
			return new BlobContentId(guid, stamp);
		}

		/// <returns></returns>
		public static Func<IEnumerable<Blob>, BlobContentId> GetTimeBasedProvider()
		{
			uint timestamp = (uint)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
			return (IEnumerable<Blob> content) => new BlobContentId(Guid.NewGuid(), timestamp);
		}

		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(BlobContentId other)
		{
			if (Guid == other.Guid)
			{
				return Stamp == other.Stamp;
			}
			return false;
		}

		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is BlobContentId)
			{
				return Equals((BlobContentId)obj);
			}
			return false;
		}

		/// <returns></returns>
		public override int GetHashCode()
		{
			return Hash.Combine(Stamp, Guid.GetHashCode());
		}

		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator ==(BlobContentId left, BlobContentId right)
		{
			return left.Equals(right);
		}

		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator !=(BlobContentId left, BlobContentId right)
		{
			return !left.Equals(right);
		}
	}
}
