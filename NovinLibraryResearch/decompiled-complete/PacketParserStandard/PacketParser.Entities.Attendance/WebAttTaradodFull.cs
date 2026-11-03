using System;
using System.Collections.Generic;
using PacketParser.Attendance.InterFaces;

namespace PacketParser.Entities.Attendance;

public class WebAttTaradodFull : ITaradodFull
{
	public List<WebAttTaradodFullDet> ListTaradodDets { get; set; } = new List<WebAttTaradodFullDet>();


	public List<WebAttFish> Fishes { get; set; }

	public string Year { get; set; } = "---";


	public string Month { get; set; } = "---";


	public int SumKarkerd { get; set; }

	public int SumMoazzafi { get; set; }

	public string SumMovazafiForDisplay { get; set; }

	public string SumKarkerdForDisplay { get; set; }

	public short CountKarkerd { get; set; }

	public int SumKasrkar { get; set; }

	public int SumKasrkarBegin { get; set; }

	public int SumKasrkarMid { get; set; }

	public int SumKasrkarEnd { get; set; }

	public int SumEzafekar { get; set; }

	public string SumEzafekarForDisplay { get; set; }

	public string SumKasrkarForDisplay { get; set; }

	public int SumEzafekarBegin { get; set; }

	public int SumEzafekarMid { get; set; }

	public int SumEzafekarEnd { get; set; }

	public short CountMorakhasiNotSalary { get; set; }

	public short CountMorakhasiWithSalary { get; set; }

	public short CountMorakhasi { get; set; }

	public short CountMorakhasiEstelaji { get; set; }

	public short CountMorakhasiEstehghaghi { get; set; }

	public short MissionDaily { get; set; }

	public short JarimeRoz { get; set; }

	public short CountDelayBeginShift { get; set; }

	public short CountGheybat { get; set; }

	public int GheybatMin { get; set; }

	public int SumGheybatMin { get; set; }

	public short CountTatil { get; set; }

	public short CountNaghes { get; set; }

	public short Countshift { get; set; }

	public int Sumshift { get; set; }

	public string SumShiftTime { get; set; }

	public DateTime DateMin { get; set; }

	public DateTime DateMax { get; set; }

	public int PersonId { get; set; }

	public long CodeTafsil { get; set; }

	public string FullName { get; set; }

	public short CountWorkdayHoghogh { get; set; }

	public short CountWorkdayBime { get; set; }

	public short CountWorkdayMaliyat { get; set; }

	public int SumWorkVacation { get; set; }

	public int SumWorkMission { get; set; }

	public int SumWorkNightWork { get; set; }

	public int SumNobatKari1 { get; set; }

	public int SumNobatKari2 { get; set; }

	public int SumNobatKari3 { get; set; }

	public int SumFriDayWork { get; set; }

	public int SumTatilDayWork { get; set; }

	public int SumJarimeRoz { get; set; }

	public string SumKarkerdTime { get; set; }

	public string SumMoazzafiTime { get; set; }

	public string SumKasrkarTime { get; set; }

	public string SumKasrkarBeginTime { get; set; }

	public string SumKasrkarMidTime { get; set; }

	public string SumKasrkarEndTime { get; set; }

	public string SumEzafekarTime { get; set; }

	public string SumEzafekarBeginTime { get; set; }

	public string SumEzafekarMidTime { get; set; }

	public string SumEzafekarEndTime { get; set; }

	public string SumEzafekarBeginForDisplay { get; set; }

	public string SumEzafekarMidForDisplay { get; set; }

	public string SumEzafekarEndDisplay { get; set; }

	public string SumKasrkarBeginForDisplay { get; set; }

	public string SumKasrkarMidForDisplay { get; set; }

	public string SumKasrkarEndDisplay { get; set; }

	public string SumWorkVacationTime { get; set; }

	public string SumWorkMissionTime { get; set; }

	public string SumWorkNightWorkTime { get; set; }

	public string SumNobatKari1Time { get; set; }

	public string SumNobatKari2Time { get; set; }

	public string SumNobatKari3Time { get; set; }

	public string SumWorkHalioDayWorkTime { get; set; }

	public string SumWorkTatilWorkTime { get; set; }

	short ITaradodFull.CountKarkerd { get; set; }
}
