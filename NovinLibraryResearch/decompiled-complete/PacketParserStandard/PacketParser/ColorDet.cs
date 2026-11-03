using PacketParser.Attendance;

namespace PacketParser;

public class ColorDet
{
	public string AdamGharardadColor { get; set; } = "#A52A2A";


	public string AdamShiftColor { get; set; } = "#00008B";


	public string DefaultColor { get; set; } = "#000000\t";


	public string GheybatColor { get; set; } = "#FF0000";


	public string GheybatOnHolidayColor { get; set; } = "#db7093";


	public string VacationColor { get; set; } = "#6a0dad";


	public string TatilColor { get; set; } = "#008000";


	public string TatilRasmiColor { get; set; } = "#9ACD32";


	public string VokhKamelColor { get; set; } = "#0000FF";


	public string VokhNaghesColor { get; set; } = "#de991b";


	public string RealTaradodColor { get; set; } = "#000000";


	public string EditTaradodColor { get; set; } = "#d91cd9";


	public string AddTaradodColor { get; set; } = "#4040d6";


	public string VirtualTaradodColor { get; set; } = "#004000";


	public string UnCorrectTaradodColor { get; set; } = "#A52A2A";


	public string ColorMorSaatiColor { get; set; } = "#004000";


	public string MissionColor { get; set; } = "#32CD32";


	public string HalidayWorkColor { get; set; } = "#8A2BE2";


	public string TatilWorkColor { get; set; } = "#8B4513";


	public string ShiftWorkColor1 { get; set; } = "#1E90FF";


	public string ShiftWorkColor2 { get; set; } = "#ff7f50";


	public string ShiftWorkColor3 { get; set; } = "#414141";


	public string NightWorkColor { get; set; } = "#8b0000";


	public string SurceaseColor { get; set; } = "#00FFFF";


	public string FridayColor { get; set; } = "#FF0000";


	public string LeavingWorkColor { get; set; } = "#FF1493";


	public string TatilRasmiAndLeavingWorkColor { get; set; } = "#FF1493";


	public string TatilAndLeavingWorkColor { get; set; } = "#FF1493";


	public string BackColorVacation => "#99ff9b";

	public string BackColorMission => "#cfff82";

	public string BackColorFriDayWork => "#ffd6d6";

	public string BackColorTatilWork => "#cf8f76";

	public string BackColorShiftWork1 => "#bfe9ff";

	public string BackColorShiftWork2 => "#fffdc7";

	public string BackColorShiftWork3 => "#d6d6d6";

	public string BackColorNightWork => "#ababab";

	public string BackColorNormal => "#ffffff";

	public string GetColor(StatuseTaradodEnumType swStatus)
	{
		switch (swStatus)
		{
		case StatuseTaradodEnumType.None:
			return DefaultColor;
		case StatuseTaradodEnumType.Morakhsi:
			return VacationColor;
		case StatuseTaradodEnumType.Mission:
			return MissionColor;
		case StatuseTaradodEnumType.Gheybat:
			return GheybatColor;
		case StatuseTaradodEnumType.TatilRasmiAndGheybat:
		case StatuseTaradodEnumType.TatilAndGheybat:
			return GheybatOnHolidayColor;
		case StatuseTaradodEnumType.AdamGharardad:
			return AdamGharardadColor;
		case StatuseTaradodEnumType.Tatil:
			return TatilColor;
		case StatuseTaradodEnumType.TatilRasmi:
			return TatilRasmiColor;
		case StatuseTaradodEnumType.VOKHKamel:
			return VokhKamelColor;
		case StatuseTaradodEnumType.VOKHNaghes:
			return VokhNaghesColor;
		case StatuseTaradodEnumType.AdamShift:
			return AdamShiftColor;
		case StatuseTaradodEnumType.Surcease:
			return SurceaseColor;
		case StatuseTaradodEnumType.LeavingWork:
			return LeavingWorkColor;
		case StatuseTaradodEnumType.TatilRasmiAndLeavingWork:
			return TatilRasmiAndLeavingWorkColor;
		case StatuseTaradodEnumType.TatilAndLeavingWork:
			return TatilAndLeavingWorkColor;
		default:
			return DefaultColor;
		}
	}
}
