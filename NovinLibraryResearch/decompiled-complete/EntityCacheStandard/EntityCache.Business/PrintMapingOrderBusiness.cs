using System;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Business;

public class PrintMapingOrderBusiness : WebPrintMapingOrder, IHasContext
{
	private ClsCache cache;

	public string ContextGuid
	{
		get
		{
			throw new NotImplementedException();
		}
		set
		{
			throw new NotImplementedException();
		}
	}

	public PrintMapingOrderBusiness(ClsCache cache)
	{
		this.cache = cache;
	}

	public PrintMapingOrderBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public PrintMapingOrderBusiness()
	{
	}
}
