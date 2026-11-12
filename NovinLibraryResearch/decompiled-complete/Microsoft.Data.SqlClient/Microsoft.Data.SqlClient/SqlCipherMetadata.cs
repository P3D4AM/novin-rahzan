namespace Microsoft.Data.SqlClient;

internal class SqlCipherMetadata
{
	private SqlTceCipherInfoEntry _sqlTceCipherInfoEntry;

	private readonly byte _cipherAlgorithmId;

	private readonly string _cipherAlgorithmName;

	private readonly byte _encryptionType;

	private readonly byte _normalizationRuleVersion;

	private SqlClientEncryptionAlgorithm _sqlClientEncryptionAlgorithm;

	private SqlEncryptionKeyInfo _sqlEncryptionKeyInfo;

	private readonly ushort _ordinal;

	internal SqlTceCipherInfoEntry EncryptionInfo
	{
		get
		{
			return _sqlTceCipherInfoEntry;
		}
		set
		{
			_sqlTceCipherInfoEntry = value;
		}
	}

	internal byte CipherAlgorithmId => _cipherAlgorithmId;

	internal string CipherAlgorithmName => _cipherAlgorithmName;

	internal byte EncryptionType => _encryptionType;

	internal byte NormalizationRuleVersion => _normalizationRuleVersion;

	internal SqlClientEncryptionAlgorithm CipherAlgorithm
	{
		get
		{
			return _sqlClientEncryptionAlgorithm;
		}
		set
		{
			_sqlClientEncryptionAlgorithm = value;
		}
	}

	internal SqlEncryptionKeyInfo EncryptionKeyInfo
	{
		get
		{
			return _sqlEncryptionKeyInfo;
		}
		set
		{
			_sqlEncryptionKeyInfo = value;
		}
	}

	internal ushort CekTableOrdinal => _ordinal;

	internal SqlCipherMetadata(SqlTceCipherInfoEntry sqlTceCipherInfoEntry, ushort ordinal, byte cipherAlgorithmId, string cipherAlgorithmName, byte encryptionType, byte normalizationRuleVersion)
	{
		_sqlTceCipherInfoEntry = sqlTceCipherInfoEntry;
		_ordinal = ordinal;
		_cipherAlgorithmId = cipherAlgorithmId;
		_cipherAlgorithmName = cipherAlgorithmName;
		_encryptionType = encryptionType;
		_normalizationRuleVersion = normalizationRuleVersion;
		_sqlEncryptionKeyInfo = null;
	}

	internal bool IsAlgorithmInitialized()
	{
		if (_sqlClientEncryptionAlgorithm == null)
		{
			return false;
		}
		return true;
	}
}
