using System;

namespace Microsoft.Data.SqlClient;

public abstract class SqlColumnEncryptionKeyStoreProvider
{
	public virtual TimeSpan? ColumnEncryptionKeyCacheTtl { get; set; } = new TimeSpan(0L);


	public abstract byte[] DecryptColumnEncryptionKey(string masterKeyPath, string encryptionAlgorithm, byte[] encryptedColumnEncryptionKey);

	public abstract byte[] EncryptColumnEncryptionKey(string masterKeyPath, string encryptionAlgorithm, byte[] columnEncryptionKey);

	public virtual byte[] SignColumnMasterKeyMetadata(string masterKeyPath, bool allowEnclaveComputations)
	{
		throw new NotImplementedException();
	}

	public virtual bool VerifyColumnMasterKeyMetadata(string masterKeyPath, bool allowEnclaveComputations, byte[] signature)
	{
		throw new NotImplementedException();
	}
}
