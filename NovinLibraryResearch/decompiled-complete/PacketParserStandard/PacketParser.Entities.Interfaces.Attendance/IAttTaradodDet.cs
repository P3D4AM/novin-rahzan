using System;

namespace PacketParser.Entities.Interfaces.Attendance;

public interface IAttTaradodDet
{
	DateTime DateM { get; set; }

	int VacationTime { get; set; }

	DateTime? VacationStart { get; set; }

	DateTime? VacationEnd { get; set; }

	string VacationDesc { get; set; }

	int EKTime { get; set; }

	DateTime? EKStart { get; set; }

	DateTime? EKEnd { get; set; }

	string EKDesc { get; set; }

	int KKTime { get; set; }

	DateTime? KKStart { get; set; }

	DateTime? KKEnd { get; set; }

	string KKDesc { get; set; }

	int MissionTime { get; set; }

	DateTime? MissionStart { get; set; }

	DateTime? MissionEnd { get; set; }

	string MissionDesc { get; set; }

	int FridayTime { get; set; }

	int TatilTime { get; set; }

	DateTime? FridayStart { get; set; }

	DateTime? FridayEnd { get; set; }

	string FridayDesc { get; set; }

	int ShiftTime1 { get; set; }

	int ShiftTime2 { get; set; }

	int ShiftTime3 { get; set; }

	DateTime? ShiftStart1 { get; set; }

	DateTime? ShiftStart2 { get; set; }

	DateTime? ShiftStart3 { get; set; }

	DateTime? ShiftEnd1 { get; set; }

	DateTime? ShiftEnd2 { get; set; }

	DateTime? ShiftEnd3 { get; set; }

	string ShiftDesc1 { get; set; }

	string ShiftDesc2 { get; set; }

	string ShiftDesc3 { get; set; }

	int AbsentTime { get; set; }

	DateTime? AbsendStart { get; set; }

	DateTime? AbsentEnd { get; set; }

	string AbsentDesc { get; set; }

	int NightTime { get; set; }

	DateTime? NightStart { get; set; }

	DateTime? NightEnd { get; set; }

	string NightDesc { get; set; }
}
