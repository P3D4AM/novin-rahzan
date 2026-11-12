namespace Microsoft.Identity.Client.Extensions.Msal;

internal interface ICacheAccessor
{
	void Clear();

	byte[] Read();

	void Write(byte[] data);

	ICacheAccessor CreateForPersistenceValidation();
}
