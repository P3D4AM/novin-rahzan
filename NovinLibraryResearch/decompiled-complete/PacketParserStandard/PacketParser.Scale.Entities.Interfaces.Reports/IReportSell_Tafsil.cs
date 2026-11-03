namespace PacketParser.Scale.Entities.Interfaces.Reports;

public interface IReportSell_Tafsil
{
	string DateSh { get; set; }

	string PrdCode { get; set; }

	string PrdName { get; set; }

	bool ReadSt { get; set; }

	int SaleId { get; set; }

	string ScaleName { get; set; }

	double TotalQt { get; set; }

	decimal TotalSum { get; set; }

	decimal UnitPrice { get; set; }
}
