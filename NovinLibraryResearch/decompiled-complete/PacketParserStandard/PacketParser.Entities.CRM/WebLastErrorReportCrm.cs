using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities.CRM;

public class WebLastErrorReportCrm : Serializable<WebLastErrorReportCrm>, ILastErrorReportCrm
{
	public int Counter { get; set; }

	public string Version { get; set; }

	public string ClassName { get; set; }

	public string FuncName { get; set; }

	public short Source { get; set; }

	public long LKSerial { get; set; }

	public int HashAll { get; set; }
}
