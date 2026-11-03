using EntityCache.Business.Crm;
using PacketParser.Entities;

namespace EntityCache.EventArguments;

public class CallingLogEventArgs
{
	public CallingLogBusiness Call { get; set; }

	public EnSystem System { get; set; }

	public bool IsFromSync { get; set; }
}
