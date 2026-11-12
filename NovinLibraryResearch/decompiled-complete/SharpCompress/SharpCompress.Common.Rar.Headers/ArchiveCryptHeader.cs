using SharpCompress.IO;

namespace SharpCompress.Common.Rar.Headers;

internal class ArchiveCryptHeader : RarHeader
{
	private const int CRYPT_VERSION = 0;

	private const int SIZE_SALT50 = 16;

	private const int SIZE_SALT30 = 8;

	private const int SIZE_INITV = 16;

	private const int SIZE_PSWCHECK = 8;

	private const int SIZE_PSWCHECK_CSUM = 4;

	private const int CRYPT5_KDF_LG2_COUNT = 15;

	private const int CRYPT5_KDF_LG2_COUNT_MAX = 24;

	private bool _usePswCheck;

	private uint _lg2Count;

	private byte[] _salt;

	private byte[] _pswCheck;

	private byte[] _pswCheckCsm;

	public ArchiveCryptHeader(RarHeader header, RarCrcBinaryReader reader)
		: base(header, reader, HeaderType.Crypt)
	{
	}

	protected override void ReadFinish(MarkingBinaryReader reader)
	{
		if (reader.ReadRarVIntUInt32() != 0)
		{
			return;
		}
		uint bitField = reader.ReadRarVIntUInt32();
		_usePswCheck = FlagUtility.HasFlag(bitField, 1u);
		_lg2Count = reader.ReadRarVIntByte(1);
		if (_lg2Count <= 24)
		{
			_salt = reader.ReadBytes(16);
			if (_usePswCheck)
			{
				_pswCheck = reader.ReadBytes(8);
				_pswCheckCsm = reader.ReadBytes(4);
			}
		}
	}
}
