namespace Microsoft.Data.SqlClient;

internal class EnclavePackage
{
	internal SqlEnclaveSession EnclaveSession { get; }

	internal byte[] EnclavePackageBytes { get; }

	internal EnclavePackage(byte[] enclavePackageBytes, SqlEnclaveSession enclaveSession)
	{
		EnclavePackageBytes = enclavePackageBytes;
		EnclaveSession = enclaveSession;
	}
}
