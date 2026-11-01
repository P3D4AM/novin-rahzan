using System;
using SmartAssembly.Attributes;

namespace TimeAttendanceDAL.View.Taradod;

[Serializable]
[DoNotPruneType]
public class ClsForRptManage
{
	public long CodeTafsil { get; set; }

	public string FullName { get; set; }

	public string StartDate { get; set; }

	public string EndDate { get; set; }

	public string DateReport { get; set; }

	public int CountDays { get; set; }

	public int CountShifts { get; set; }

	public int CountHozor { get; set; }

	public int CountGheybat { get; set; }

	public int CountVacationWithSalary { get; set; }

	public int CountVacationWithoutSalary { get; set; }

	public int CountMission { get; set; }

	public string SumWorkMissionTime { get; set; }

	public int DayGetSalary { get; set; }

	public int TaradodNaghes { get; set; }

	public string KarkerdTime { get; set; }

	public string OverTime { get; set; }

	public string LeaveTime { get; set; }

	public string SumKasrkarBegin { get; set; }

	public string SumKasrkarMid { get; set; }

	public string SumKasrkarEnd { get; set; }

	public string SumEzafekarBegin { get; set; }

	public string SumEzafekarMid { get; set; }

	public string SumEzafekarEnd { get; set; }

	public string KarkardMozafiTime { get; set; }

	public string MozafiMonthTime { get; set; }

	public short CountDelayBeginShift { get; set; }

	public short CountJarimeDays { get; set; }

	public string SumClocklyVacation { get; set; }

	public int SumKarkardMin { get; set; }

	public int GharardadLeaveMinutes { get; set; }

	public int SumNobatKari1 { get; set; }

	public int SumNobatKari2 { get; set; }

	public int SumNobatKari3 { get; set; }

	public int SumWorkNightWork { get; set; }

	public int SumTatilDayWork { get; set; }

	public int SumFriDayWork { get; set; }

	public short CountMorakhasiEstehghaghi { get; set; }

	public short CountMorakhasiEstelaji { get; set; }
}
