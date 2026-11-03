using PacketParser.Entities;

namespace EntityCache.Business;

public class PrintTemplateBusiness : WebPrintTemplate
{
	private ClsCache cache;

	public PrintTemplateBusiness(ClsCache cache)
	{
		this.cache = cache;
	}

	public PrintTemplateBusiness()
	{
	}
}
