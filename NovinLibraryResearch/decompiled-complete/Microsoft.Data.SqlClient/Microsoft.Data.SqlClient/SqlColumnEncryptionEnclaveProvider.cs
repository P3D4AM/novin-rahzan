using System.Security.Cryptography;

namespace Microsoft.Data.SqlClient;

internal abstract class SqlColumnEncryptionEnclaveProvider
{
	internal abstract void GetEnclaveSession(EnclaveSessionParameters enclaveSessionParameters, bool generateCustomData, out SqlEnclaveSession sqlEnclaveSession, out long counter, out byte[] customData, out int customDataLength);

	internal abstract SqlEnclaveAttestationParameters GetAttestationParameters(string attestationUrl, byte[] customData, int customDataLength);

	internal abstract void CreateEnclaveSession(byte[] enclaveAttestationInfo, ECDiffieHellman clientDiffieHellmanKey, EnclaveSessionParameters enclaveSessionParameters, byte[] customData, int customDataLength, out SqlEnclaveSession sqlEnclaveSession, out long counter);

	internal abstract void InvalidateEnclaveSession(EnclaveSessionParameters enclaveSessionParameters, SqlEnclaveSession enclaveSession);
}
