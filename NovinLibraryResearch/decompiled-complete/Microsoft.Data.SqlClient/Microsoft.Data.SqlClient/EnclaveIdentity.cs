using System;
using System.Linq;

namespace Microsoft.Data.SqlClient;

internal class EnclaveIdentity
{
	private static readonly int ImageEnclaveLongIdLength = 32;

	private static readonly int ImageEnclaveShortIdLength = 16;

	public byte[] OwnerId = new byte[ImageEnclaveLongIdLength];

	public byte[] UniqueId = new byte[ImageEnclaveLongIdLength];

	public byte[] AuthorId = new byte[ImageEnclaveLongIdLength];

	public byte[] FamilyId = new byte[ImageEnclaveShortIdLength];

	public byte[] ImageId = new byte[ImageEnclaveShortIdLength];

	private int Size { get; set; }

	public uint EnclaveSvn { get; set; }

	public uint SecureKernelSvn { get; set; }

	public uint PlatformSvn { get; set; }

	public uint Flags { get; set; }

	public uint SigningLevel { get; set; }

	public uint Reserved { get; set; }

	public EnclaveIdentity()
	{
	}

	public EnclaveIdentity(byte[] payload)
	{
		Size = payload.Length;
		int num = 0;
		int imageEnclaveLongIdLength = ImageEnclaveLongIdLength;
		OwnerId = payload.Skip(num).Take(imageEnclaveLongIdLength).ToArray();
		num += imageEnclaveLongIdLength;
		int imageEnclaveLongIdLength2 = ImageEnclaveLongIdLength;
		UniqueId = payload.Skip(num).Take(imageEnclaveLongIdLength2).ToArray();
		num += imageEnclaveLongIdLength2;
		int imageEnclaveLongIdLength3 = ImageEnclaveLongIdLength;
		AuthorId = payload.Skip(num).Take(imageEnclaveLongIdLength3).ToArray();
		num += imageEnclaveLongIdLength3;
		int imageEnclaveShortIdLength = ImageEnclaveShortIdLength;
		FamilyId = payload.Skip(num).Take(imageEnclaveShortIdLength).ToArray();
		num += imageEnclaveShortIdLength;
		int imageEnclaveShortIdLength2 = ImageEnclaveShortIdLength;
		ImageId = payload.Skip(num).Take(imageEnclaveShortIdLength2).ToArray();
		num += imageEnclaveShortIdLength2;
		EnclaveSvn = BitConverter.ToUInt32(payload, num);
		num += 4;
		SecureKernelSvn = BitConverter.ToUInt32(payload, num);
		num += 4;
		PlatformSvn = BitConverter.ToUInt32(payload, num);
		num += 4;
		Flags = BitConverter.ToUInt32(payload, num);
		num += 4;
		SigningLevel = BitConverter.ToUInt32(payload, num);
		num += 4;
		Reserved = BitConverter.ToUInt32(payload, num);
		num += 4;
	}

	public int GetSizeInPayload()
	{
		return ImageEnclaveLongIdLength * 3 + ImageEnclaveShortIdLength * 2 + 24;
	}
}
