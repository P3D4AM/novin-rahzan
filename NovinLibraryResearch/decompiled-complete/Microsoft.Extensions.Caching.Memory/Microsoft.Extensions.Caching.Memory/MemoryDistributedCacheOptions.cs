namespace Microsoft.Extensions.Caching.Memory;

public class MemoryDistributedCacheOptions : MemoryCacheOptions
{
	public MemoryDistributedCacheOptions()
	{
		base.SizeLimit = 209715200L;
	}
}
