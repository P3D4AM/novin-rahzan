namespace Microsoft.Identity.Client.Cache;

internal interface ILegacyCachePersistence
{
	byte[] LoadCache();

	void WriteCache(byte[] serializedCache);
}
