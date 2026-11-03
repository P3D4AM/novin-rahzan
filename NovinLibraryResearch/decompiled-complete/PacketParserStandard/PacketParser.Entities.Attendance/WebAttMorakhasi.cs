using Newtonsoft.Json;
using PacketParser.Attendance.InterFaces;

namespace PacketParser.Entities.Attendance;

public class WebAttMorakhasi : IMorakhasi
{
	public int GharardadId { get; set; }

	public int SumVacationUsed { get; set; }

	public int SumVacationSaati { get; set; }

	public double SumVacationLeft { get; set; }

	public string SumVacationSaatiEquivalentDay { get; set; }

	[JsonIgnore]
	public double SumVacationSaatiEquivalentDayDouble => SumVacationSaatiEquivalentDay.ParseToDouble(0.0, 3);

	public int TotalVacationDaysLeft { get; set; }

	public string TotalVacationHourLeft { get; set; }

	public double VacationDaysSavedFromGharardad { get; set; }

	public double VacationDaysFromGharardad { get; set; }

	public int RemindVacationToNow { get; set; }

	[JsonIgnore]
	public double UsedVacationSum => SumVacationSaatiEquivalentDayDouble + (double)SumVacationUsed;

	[JsonIgnore]
	public double EntitlementLeave => VacationDaysFromGharardad + VacationDaysSavedFromGharardad;
}
