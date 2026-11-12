namespace Microsoft.Data.SqlClient;

internal class SqlClientSymmetricKey
{
	protected readonly byte[] _rootKey;

	internal virtual byte[] RootKey => _rootKey;

	internal SqlClientSymmetricKey(byte[] rootKey)
	{
		if (rootKey == null || rootKey.Length == 0)
		{
			throw SQL.NullColumnEncryptionKeySysErr();
		}
		_rootKey = rootKey;
	}

	internal virtual string GetKeyHash()
	{
		return SqlSecurityUtility.GetSHA256Hash(RootKey);
	}

	internal virtual int Length()
	{
		return _rootKey.Length;
	}
}
