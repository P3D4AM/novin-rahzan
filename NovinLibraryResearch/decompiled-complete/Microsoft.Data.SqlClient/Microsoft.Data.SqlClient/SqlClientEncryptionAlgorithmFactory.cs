namespace Microsoft.Data.SqlClient;

internal abstract class SqlClientEncryptionAlgorithmFactory
{
	internal abstract SqlClientEncryptionAlgorithm Create(SqlClientSymmetricKey encryptionKey, SqlClientEncryptionType encryptionType, string encryptionAlgorithm);
}
