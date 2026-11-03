using PacketParser.Entities;

namespace EntityCache.WebService.Repository.Accounting;

public record TaxServiceResponse
{
	public EnNotification Type { get; set; }

	public string Message { get; set; }

	public string Title { get; set; }
}
