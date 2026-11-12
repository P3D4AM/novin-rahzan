namespace Microsoft.IdentityModel.Tokens;

public interface ICryptoProvider
{
	bool IsSupportedAlgorithm(string algorithm, params object[] args);

	object Create(string algorithm, params object[] args);

	void Release(object cryptoInstance);
}
