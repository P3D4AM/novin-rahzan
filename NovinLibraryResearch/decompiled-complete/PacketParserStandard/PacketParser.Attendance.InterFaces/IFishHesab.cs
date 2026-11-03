namespace PacketParser.Attendance.InterFaces;

public interface IFishHesab
{
	int Id { get; }

	string Name { get; set; }

	int HtreeId { get; set; }

	bool St { get; set; }

	decimal Account { get; set; }

	string Description { get; set; }

	short Type { get; set; }

	long CodeMoein { get; set; }

	long CodeTafsil { get; set; }

	bool Taxable { get; set; }

	bool Insurance { get; set; }

	bool PrsnTafsilUsed { get; set; }
}
