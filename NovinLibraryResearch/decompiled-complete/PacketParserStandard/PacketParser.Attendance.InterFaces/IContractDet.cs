namespace PacketParser.Attendance.InterFaces;

public interface IContractDet
{
	decimal Account { get; set; }

	string Desc { get; set; }

	string Formula { get; set; }

	int HesabId { get; set; }

	bool IsDecrease { get; set; }

	string Name { get; set; }

	short Order { get; set; }

	int Privilege { get; set; }

	short TypeAccount { get; set; }

	decimal nValue { get; set; }
}
