namespace PacketParser.Scale.Entities.Interfaces.Reports;

public interface IReportTajmi
{
	string PrdCode { get; set; }

	string PrdName { get; set; }

	int ScaleId { get; set; }

	double TotalQt { get; set; }

	decimal TotalSum { get; set; }
}
