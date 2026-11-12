namespace Microsoft.Extensions.Caching.Memory;

public class PostEvictionCallbackRegistration
{
	public PostEvictionDelegate EvictionCallback { get; set; }

	public object State { get; set; }
}
