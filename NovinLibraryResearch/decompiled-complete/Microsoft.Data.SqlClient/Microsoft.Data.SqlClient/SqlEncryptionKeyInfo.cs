namespace Microsoft.Data.SqlClient;

internal class SqlEncryptionKeyInfo
{
	internal byte[] encryptedKey;

	internal int databaseId;

	internal int cekId;

	internal int cekVersion;

	internal byte[] cekMdVersion;

	internal string keyPath;

	internal string keyStoreName;

	internal string algorithmName;

	internal byte normalizationRuleVersion;
}
