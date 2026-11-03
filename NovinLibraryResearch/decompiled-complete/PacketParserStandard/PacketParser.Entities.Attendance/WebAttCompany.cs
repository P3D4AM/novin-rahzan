using PacketParser.Entities.Interfaces.Attendance;

namespace PacketParser.Entities.Attendance;

public class WebAttCompany : IAttCompany
{
	public short Id { get; set; }

	public string Code { get; set; }

	public string Name { get; set; }

	public string NameKarfarma { get; set; }

	public string Address { get; set; }

	public int NerkhBimeh { get; set; }

	public string NoPeyman { get; set; }

	public string InsuranceBranch { get; set; }

	public string NameWorkShop { get; set; }

	public string AddressWorkShop { get; set; }
}
