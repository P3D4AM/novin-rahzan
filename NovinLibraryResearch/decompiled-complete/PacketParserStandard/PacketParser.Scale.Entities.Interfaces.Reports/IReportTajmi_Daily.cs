namespace PacketParser.Scale.Entities.Interfaces.Reports;

public interface IReportTajmi_Daily
{
	string DateSh { get; set; }

	string PrdCode { get; set; }

	string PrdName { get; set; }

	decimal Price_Sum { get; set; }

	double Qt_Sum { get; set; }
}
