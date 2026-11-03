using PacketParser.Entities;

namespace EntityCache.Business;

public class PrintTemplateMapingBusiness : WebPrintTemplateMaping
{
	private ClsCache _cache;

	public PrintTemplateMapingBusiness(ClsCache cache)
	{
		_cache = cache;
	}

	public PrintTemplateMapingBusiness()
	{
	}
}
