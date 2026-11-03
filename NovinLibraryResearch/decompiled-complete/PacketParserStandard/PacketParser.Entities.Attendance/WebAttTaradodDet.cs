using System;
using PacketParser.Entities.Interfaces.Attendance;

namespace PacketParser.Entities.Attendance;

public class WebAttTaradodDet : IAttTaradodDet
{
	public DateTime DateM { get; set; }

	public int VacationTime { get; set; }

	public DateTime? VacationStart { get; set; }

	public DateTime? VacationEnd { get; set; }

	public string VacationDesc { get; set; }

	public int EKTime { get; set; }

	public DateTime? EKStart { get; set; }

	public DateTime? EKEnd { get; set; }

	public string EKDesc { get; set; }

	public int KKTime { get; set; }

	public DateTime? KKStart { get; set; }

	public DateTime? KKEnd { get; set; }

	public string KKDesc { get; set; }

	public int MissionTime { get; set; }

	public DateTime? MissionStart { get; set; }

	public DateTime? MissionEnd { get; set; }

	public string MissionDesc { get; set; }

	public int FridayTime { get; set; }

	public int TatilTime { get; set; }

	public DateTime? FridayStart { get; set; }

	public DateTime? FridayEnd { get; set; }

	public string FridayDesc { get; set; }

	public int ShiftTime1 { get; set; }

	public int ShiftTime2 { get; set; }

	public int ShiftTime3 { get; set; }

	public DateTime? ShiftStart1 { get; set; }

	public DateTime? ShiftStart2 { get; set; }

	public DateTime? ShiftStart3 { get; set; }

	public DateTime? ShiftEnd1 { get; set; }

	public DateTime? ShiftEnd2 { get; set; }

	public DateTime? ShiftEnd3 { get; set; }

	public string ShiftDesc1 { get; set; }

	public string ShiftDesc2 { get; set; }

	public string ShiftDesc3 { get; set; }

	public int AbsentTime { get; set; }

	public DateTime? AbsendStart { get; set; }

	public DateTime? AbsentEnd { get; set; }

	public string AbsentDesc { get; set; }

	public int NightTime { get; set; }

	public DateTime? NightStart { get; set; }

	public DateTime? NightEnd { get; set; }

	public string NightDesc { get; set; }
}
