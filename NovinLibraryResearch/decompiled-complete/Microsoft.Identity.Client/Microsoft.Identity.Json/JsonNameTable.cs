namespace Microsoft.Identity.Json;

internal abstract class JsonNameTable
{
	public abstract string Get(char[] key, int start, int length);
}
