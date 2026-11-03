using PacketParser.Entities.Interfaces;

namespace EntityCache.Business.Advertise.NoBisunessModels;

public class AdvCountReport : IHasContext
{
	public long SimCardNumber { get; set; }

	public int DivarCount { get; set; }

	public int SheyPorCount { get; set; }

	public int DivarCountToday { get; set; }

	public int SheyPorCountToday { get; set; }

	public int DivarCountTodayIp { get; set; }

	public int SheyPorCountTodayIp { get; set; }

	public string ContextGuid { get; set; }

	public AdvCountReport(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}
