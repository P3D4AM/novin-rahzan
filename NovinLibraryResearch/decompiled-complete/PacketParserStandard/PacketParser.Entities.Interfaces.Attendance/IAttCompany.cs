namespace PacketParser.Entities.Interfaces.Attendance;

public interface IAttCompany
{
	short Id { get; }

	string Code { get; set; }

	string Name { get; set; }

	string NameKarfarma { get; set; }

	string Address { get; set; }

	int NerkhBimeh { get; set; }

	string NoPeyman { get; set; }

	string InsuranceBranch { get; set; }

	string NameWorkShop { get; set; }

	string AddressWorkShop { get; set; }
}
