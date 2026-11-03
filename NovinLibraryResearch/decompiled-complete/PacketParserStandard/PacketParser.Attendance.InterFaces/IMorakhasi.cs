namespace PacketParser.Attendance.InterFaces;

public interface IMorakhasi
{
	int GharardadId { get; set; }

	int SumVacationUsed { get; set; }

	int SumVacationSaati { get; set; }

	double SumVacationLeft { get; set; }

	string SumVacationSaatiEquivalentDay { get; set; }
}
