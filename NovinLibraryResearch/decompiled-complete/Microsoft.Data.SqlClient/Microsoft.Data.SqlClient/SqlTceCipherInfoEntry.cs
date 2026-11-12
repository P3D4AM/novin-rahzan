using System.Collections.Generic;

namespace Microsoft.Data.SqlClient;

internal class SqlTceCipherInfoEntry
{
	private readonly List<SqlEncryptionKeyInfo> _columnEncryptionKeyValues;

	private readonly int _ordinal;

	private int _databaseId;

	private int _cekId;

	private int _cekVersion;

	private byte[] _cekMdVersion;

	internal int Ordinal => _ordinal;

	internal int DatabaseId => _databaseId;

	internal int CekId => _cekId;

	internal int CekVersion => _cekVersion;

	internal byte[] CekMdVersion => _cekMdVersion;

	internal List<SqlEncryptionKeyInfo> ColumnEncryptionKeyValues => _columnEncryptionKeyValues;

	internal void Add(byte[] encryptedKey, int databaseId, int cekId, int cekVersion, byte[] cekMdVersion, string keyPath, string keyStoreName, string algorithmName)
	{
		SqlEncryptionKeyInfo sqlEncryptionKeyInfo = new SqlEncryptionKeyInfo();
		sqlEncryptionKeyInfo.encryptedKey = encryptedKey;
		sqlEncryptionKeyInfo.databaseId = databaseId;
		sqlEncryptionKeyInfo.cekId = cekId;
		sqlEncryptionKeyInfo.cekVersion = cekVersion;
		sqlEncryptionKeyInfo.cekMdVersion = cekMdVersion;
		sqlEncryptionKeyInfo.keyPath = keyPath;
		sqlEncryptionKeyInfo.keyStoreName = keyStoreName;
		sqlEncryptionKeyInfo.algorithmName = algorithmName;
		_columnEncryptionKeyValues.Add(sqlEncryptionKeyInfo);
		if (_databaseId == 0)
		{
			_databaseId = databaseId;
			_cekId = cekId;
			_cekVersion = cekVersion;
			_cekMdVersion = cekMdVersion;
		}
	}

	internal SqlTceCipherInfoEntry(int ordinal = 0)
	{
		_ordinal = ordinal;
		_databaseId = 0;
		_cekId = 0;
		_cekVersion = 0;
		_cekMdVersion = null;
		_columnEncryptionKeyValues = new List<SqlEncryptionKeyInfo>();
	}
}
