using System.Text;

namespace Microsoft.Data.SqlClient;

internal class SqlAeadAes256CbcHmac256EncryptionKey : SqlClientSymmetricKey
{
	internal const int KeySize = 256;

	private const string _encryptionKeySaltFormat = "Microsoft SQL Server cell encryption key with encryption algorithm:{0} and key length:{1}";

	private const string _macKeySaltFormat = "Microsoft SQL Server cell MAC key with encryption algorithm:{0} and key length:{1}";

	private const string _ivKeySaltFormat = "Microsoft SQL Server cell IV key with encryption algorithm:{0} and key length:{1}";

	private readonly SqlClientSymmetricKey _encryptionKey;

	private readonly SqlClientSymmetricKey _macKey;

	private readonly SqlClientSymmetricKey _ivKey;

	private readonly string _algorithmName;

	internal byte[] EncryptionKey => _encryptionKey.RootKey;

	internal byte[] MACKey => _macKey.RootKey;

	internal byte[] IVKey => _ivKey.RootKey;

	internal SqlAeadAes256CbcHmac256EncryptionKey(byte[] rootKey, string algorithmName)
		: base(rootKey)
	{
		_algorithmName = algorithmName;
		int num = 32;
		if (rootKey.Length != num)
		{
			throw SQL.InvalidKeySize(_algorithmName, rootKey.Length, num);
		}
		string s = $"Microsoft SQL Server cell encryption key with encryption algorithm:{_algorithmName} and key length:{256}";
		byte[] array = new byte[num];
		SqlSecurityUtility.GetHMACWithSHA256(Encoding.Unicode.GetBytes(s), RootKey, array);
		_encryptionKey = new SqlClientSymmetricKey(array);
		string s2 = $"Microsoft SQL Server cell MAC key with encryption algorithm:{_algorithmName} and key length:{256}";
		byte[] array2 = new byte[num];
		SqlSecurityUtility.GetHMACWithSHA256(Encoding.Unicode.GetBytes(s2), RootKey, array2);
		_macKey = new SqlClientSymmetricKey(array2);
		string s3 = $"Microsoft SQL Server cell IV key with encryption algorithm:{_algorithmName} and key length:{256}";
		byte[] array3 = new byte[num];
		SqlSecurityUtility.GetHMACWithSHA256(Encoding.Unicode.GetBytes(s3), RootKey, array3);
		_ivKey = new SqlClientSymmetricKey(array3);
	}
}
