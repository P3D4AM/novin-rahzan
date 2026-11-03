using System;

namespace PacketParser.Attendance.InterFaces;

public interface ITaradodFull
{
	string Year { get; set; }

	string Month { get; set; }

	int SumKarkerd { get; set; }

	int SumMoazzafi { get; set; }

	short CountKarkerd { get; set; }

	int SumKasrkar { get; set; }

	int SumKasrkarBegin { get; set; }

	int SumKasrkarMid { get; set; }

	int SumKasrkarEnd { get; set; }

	int SumEzafekar { get; set; }

	int SumEzafekarBegin { get; set; }

	int SumEzafekarMid { get; set; }

	int SumEzafekarEnd { get; set; }

	short CountMorakhasiNotSalary { get; set; }

	short CountMorakhasiWithSalary { get; set; }

	short CountMorakhasi { get; set; }

	short CountMorakhasiEstelaji { get; set; }

	short CountMorakhasiEstehghaghi { get; set; }

	short MissionDaily { get; set; }

	short JarimeRoz { get; set; }

	short CountDelayBeginShift { get; set; }

	short CountGheybat { get; set; }

	int GheybatMin { get; set; }

	int SumGheybatMin { get; set; }

	short CountTatil { get; set; }

	short CountNaghes { get; set; }

	short Countshift { get; set; }

	int Sumshift { get; set; }

	DateTime DateMin { get; set; }

	DateTime DateMax { get; set; }

	int PersonId { get; set; }

	long CodeTafsil { get; set; }

	string FullName { get; set; }

	short CountWorkdayHoghogh { get; set; }

	short CountWorkdayBime { get; set; }

	short CountWorkdayMaliyat { get; set; }

	int SumWorkVacation { get; set; }

	int SumWorkMission { get; }

	int SumWorkNightWork { get; set; }

	int SumNobatKari1 { get; set; }

	int SumNobatKari2 { get; set; }

	int SumNobatKari3 { get; set; }

	int SumFriDayWork { get; set; }

	int SumTatilDayWork { get; set; }

	int SumJarimeRoz { get; set; }
}
