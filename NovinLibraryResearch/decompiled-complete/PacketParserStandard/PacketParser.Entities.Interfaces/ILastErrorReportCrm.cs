namespace PacketParser.Entities.Interfaces;

public interface ILastErrorReportCrm
{
	int Counter { get; set; }

	string Version { get; set; }

	string ClassName { get; set; }

	string FuncName { get; set; }

	short Source { get; set; }

	long LKSerial { get; set; }
}
