using System;
using Microsoft.Data.SqlClient.Server;

namespace Microsoft.Data.SqlClient;

internal sealed class SqlColumnEncryptionInputParameterInfo
{
	private readonly SmiParameterMetaData _smiParameterMetadata;

	private readonly SqlCipherMetadata _cipherMetadata;

	private readonly byte[] _serializedWireFormat;

	internal SmiParameterMetaData ParameterMetadata => _smiParameterMetadata;

	internal byte[] SerializedWireFormat => _serializedWireFormat;

	internal SqlColumnEncryptionInputParameterInfo(SmiParameterMetaData smiParameterMetadata, SqlCipherMetadata cipherMetadata)
	{
		_smiParameterMetadata = smiParameterMetadata;
		_cipherMetadata = cipherMetadata;
		_serializedWireFormat = SerializeToWriteFormat();
	}

	private byte[] SerializeToWriteFormat()
	{
		int num = 0;
		num++;
		num++;
		num += 4;
		num += 4;
		num += 4;
		num += _cipherMetadata.EncryptionKeyInfo.cekMdVersion.Length;
		num++;
		byte[] array = new byte[num];
		int offset = 0;
		array[offset++] = _cipherMetadata.CipherAlgorithmId;
		array[offset++] = _cipherMetadata.EncryptionType;
		SerializeIntIntoBuffer(_cipherMetadata.EncryptionKeyInfo.databaseId, array, ref offset);
		SerializeIntIntoBuffer(_cipherMetadata.EncryptionKeyInfo.cekId, array, ref offset);
		SerializeIntIntoBuffer(_cipherMetadata.EncryptionKeyInfo.cekVersion, array, ref offset);
		Buffer.BlockCopy(_cipherMetadata.EncryptionKeyInfo.cekMdVersion, 0, array, offset, _cipherMetadata.EncryptionKeyInfo.cekMdVersion.Length);
		offset += _cipherMetadata.EncryptionKeyInfo.cekMdVersion.Length;
		array[offset++] = _cipherMetadata.NormalizationRuleVersion;
		return array;
	}

	private void SerializeIntIntoBuffer(int value, byte[] buffer, ref int offset)
	{
		buffer[offset++] = (byte)((uint)value & 0xFFu);
		buffer[offset++] = (byte)((uint)(value >> 8) & 0xFFu);
		buffer[offset++] = (byte)((uint)(value >> 16) & 0xFFu);
		buffer[offset++] = (byte)((uint)(value >> 24) & 0xFFu);
	}
}
