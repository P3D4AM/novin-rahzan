namespace Microsoft.Data.SqlClient;

internal class EnclavePublicKey
{
	public byte[] PublicKey { get; set; }

	public EnclavePublicKey(byte[] payload)
	{
		PublicKey = payload;
	}
}
