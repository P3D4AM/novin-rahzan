namespace Microsoft.Data.SqlClient;

internal abstract class SqlClientEncryptionAlgorithm
{
	internal abstract byte[] EncryptData(byte[] plainText);

	internal abstract byte[] DecryptData(byte[] cipherText);
}
