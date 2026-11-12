namespace Microsoft.IdentityModel.Json;

internal abstract class JsonNameTable
{
	public abstract string Get(char[] key, int start, int length);
}
