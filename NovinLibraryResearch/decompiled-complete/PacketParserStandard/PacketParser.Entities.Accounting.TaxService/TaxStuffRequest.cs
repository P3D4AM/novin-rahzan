namespace PacketParser.Entities.Accounting.TaxService;

public class TaxStuffRequest
{
	public string Name { get; set; }

	public int? Page { get; set; }

	public int? Count { get; set; }

	public bool IsWebSite { get; set; }
}
