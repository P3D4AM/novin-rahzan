namespace Microsoft.IdentityModel.Tokens;

public interface ICompressionProvider
{
	string Algorithm { get; }

	bool IsSupportedAlgorithm(string algorithm);

	byte[] Decompress(byte[] value);

	byte[] Compress(byte[] value);
}
