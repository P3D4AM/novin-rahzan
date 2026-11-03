using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Business;

public class ErrorLogBusiness : WebErrorLog, IHasContext
{
	public sealed override string ContextGuid { get; set; }

	public ErrorLogBusiness(string context)
	{
		ContextGuid = context;
	}
}
