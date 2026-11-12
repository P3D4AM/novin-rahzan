using System;
using System.Linq;

namespace Microsoft.Data.SqlClient;

internal class EnclaveReportModule
{
	private static readonly int ImageEnclaveLongIdLength = 32;

	private static readonly int ImageEnclaveShortIdLength = 16;

	public byte[] UniqueId = new byte[ImageEnclaveLongIdLength];

	public byte[] AuthorId = new byte[ImageEnclaveLongIdLength];

	public byte[] FamilyId = new byte[ImageEnclaveShortIdLength];

	public byte[] ImageId = new byte[ImageEnclaveShortIdLength];

	public EnclaveReportModuleHeader Header { get; set; }

	public uint Svn { get; set; }

	public string ModuleName { get; set; }

	public EnclaveReportModule(byte[] payload)
	{
		int num = 0;
		Header = new EnclaveReportModuleHeader(payload);
		num += Convert.ToInt32(Header.GetSizeInPayload());
		int imageEnclaveLongIdLength = ImageEnclaveLongIdLength;
		UniqueId = payload.Skip(num).Take(imageEnclaveLongIdLength).ToArray();
		num += imageEnclaveLongIdLength;
		int imageEnclaveLongIdLength2 = ImageEnclaveLongIdLength;
		AuthorId = payload.Skip(num).Take(imageEnclaveLongIdLength2).ToArray();
		num += imageEnclaveLongIdLength2;
		int imageEnclaveShortIdLength = ImageEnclaveShortIdLength;
		FamilyId = payload.Skip(num).Take(imageEnclaveShortIdLength).ToArray();
		num += imageEnclaveShortIdLength;
		int imageEnclaveShortIdLength2 = ImageEnclaveShortIdLength;
		ImageId = payload.Skip(num).Take(imageEnclaveShortIdLength).ToArray();
		num += imageEnclaveShortIdLength2;
		Svn = BitConverter.ToUInt32(payload, num);
		num += 4;
		int num2 = Convert.ToInt32(Header.ModuleSize) - num;
		ModuleName = BitConverter.ToString(payload, num, 1);
		num += 2;
	}

	public int GetSizeInPayload()
	{
		return Header.GetSizeInPayload() + Convert.ToInt32(Header.ModuleSize);
	}
}
