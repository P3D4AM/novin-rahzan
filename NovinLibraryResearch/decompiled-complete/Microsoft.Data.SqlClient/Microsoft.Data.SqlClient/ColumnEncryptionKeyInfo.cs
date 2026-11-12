using System;

namespace Microsoft.Data.SqlClient;

internal class ColumnEncryptionKeyInfo
{
	internal readonly int KeyId;

	internal readonly int DatabaseId;

	internal readonly byte[] DecryptedKeyBytes;

	internal readonly byte[] KeyIdBytes;

	internal readonly byte[] DatabaseIdBytes;

	internal readonly byte[] KeyMetadataVersionBytes;

	private static readonly string _decryptedKeyName = "DecryptedKey";

	private static readonly string _keyMetadataVersionName = "KeyMetadataVersion";

	private static readonly string _className = "ColumnEncryptionKeyInfo";

	private static readonly string _bytePackageName = "BytePackage";

	private static readonly string _serializeToBufferMethodName = "SerializeToBuffer";

	private static readonly string _startOffsetName = "StartOffset";

	internal ColumnEncryptionKeyInfo(byte[] decryptedKey, int databaseId, byte[] keyMetadataVersion, int keyid)
	{
		if (decryptedKey == null)
		{
			throw SQL.NullArgumentInConstructorInternal(_decryptedKeyName, _className);
		}
		if (decryptedKey.Length == 0)
		{
			throw SQL.EmptyArgumentInConstructorInternal(_decryptedKeyName, _className);
		}
		if (keyMetadataVersion == null)
		{
			throw SQL.NullArgumentInConstructorInternal(_keyMetadataVersionName, _className);
		}
		if (keyMetadataVersion.Length == 0)
		{
			throw SQL.EmptyArgumentInConstructorInternal(_keyMetadataVersionName, _className);
		}
		KeyId = keyid;
		DatabaseId = databaseId;
		DecryptedKeyBytes = decryptedKey;
		KeyMetadataVersionBytes = keyMetadataVersion;
		ushort value;
		try
		{
			value = (ushort)keyid;
		}
		catch (Exception innerException)
		{
			throw SQL.InvalidKeyIdUnableToCastToUnsignedShort(keyid, innerException);
		}
		KeyIdBytes = BitConverter.GetBytes(value);
		try
		{
		}
		catch (Exception innerException2)
		{
			throw SQL.InvalidDatabaseIdUnableToCastToUnsignedInt(databaseId, innerException2);
		}
		DatabaseIdBytes = BitConverter.GetBytes((uint)databaseId);
	}

	internal int GetLengthForSerialization()
	{
		int num = 0;
		num += DecryptedKeyBytes.Length;
		num += KeyIdBytes.Length;
		num += DatabaseIdBytes.Length;
		return num + KeyMetadataVersionBytes.Length;
	}

	internal int SerializeToBuffer(byte[] bytePackage, int startOffset)
	{
		if (bytePackage == null)
		{
			throw SQL.NullArgumentInternal(_bytePackageName, _className, _serializeToBufferMethodName);
		}
		if (bytePackage.Length == 0)
		{
			throw SQL.EmptyArgumentInternal(_bytePackageName, _className, _serializeToBufferMethodName);
		}
		if (startOffset >= bytePackage.Length)
		{
			throw SQL.OffsetOutOfBounds(_startOffsetName, _className, _serializeToBufferMethodName);
		}
		if (bytePackage.Length - startOffset < GetLengthForSerialization())
		{
			throw SQL.InsufficientBuffer(_bytePackageName, _className, _serializeToBufferMethodName);
		}
		Buffer.BlockCopy(DatabaseIdBytes, 0, bytePackage, startOffset, DatabaseIdBytes.Length);
		startOffset += DatabaseIdBytes.Length;
		Buffer.BlockCopy(KeyMetadataVersionBytes, 0, bytePackage, startOffset, KeyMetadataVersionBytes.Length);
		startOffset += KeyMetadataVersionBytes.Length;
		Buffer.BlockCopy(KeyIdBytes, 0, bytePackage, startOffset, KeyIdBytes.Length);
		startOffset += KeyIdBytes.Length;
		Buffer.BlockCopy(DecryptedKeyBytes, 0, bytePackage, startOffset, DecryptedKeyBytes.Length);
		startOffset += DecryptedKeyBytes.Length;
		return startOffset;
	}
}
