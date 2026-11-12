using System;
using ExcelDataReader.Core.OfficeCrypto;

namespace ExcelDataReader.Core.BinaryFormat;

internal class XlsBiffFilePass : XlsBiffRecord
{
	public EncryptionInfo EncryptionInfo { get; }

	internal XlsBiffFilePass(byte[] bytes, uint offset, int biffVersion)
		: base(bytes, offset)
	{
		if (biffVersion >= 2 && biffVersion <= 5)
		{
			ushort xorEncryptionKey = ReadUInt16(0);
			ushort xorHashValue = ReadUInt16(2);
			EncryptionInfo = EncryptionInfo.Create(xorEncryptionKey, xorHashValue);
			return;
		}
		ushort num = ReadUInt16(0);
		switch (num)
		{
		case 0:
		{
			ushort xorEncryptionKey2 = ReadUInt16(2);
			ushort xorHashValue2 = ReadUInt16(4);
			EncryptionInfo = EncryptionInfo.Create(xorEncryptionKey2, xorHashValue2);
			break;
		}
		case 1:
		{
			byte[] array = new byte[bytes.Length - 6];
			Array.Copy(bytes, 6, array, 0, bytes.Length - 6);
			EncryptionInfo = EncryptionInfo.Create(array);
			break;
		}
		default:
			throw new NotSupportedException("Unknown encryption type: " + num);
		}
	}
}
