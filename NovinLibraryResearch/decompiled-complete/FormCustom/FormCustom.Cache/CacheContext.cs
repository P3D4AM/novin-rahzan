using PacketParser.Entities;

namespace FormCustom.Cache;

public class CacheContext
{
	public string ContextGuid { get; private set; }

	public WebContextHolder WebContextHolder => WebContextHolder.GetWebContextHolder(ContextGuid);

	public CacheContext(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}
