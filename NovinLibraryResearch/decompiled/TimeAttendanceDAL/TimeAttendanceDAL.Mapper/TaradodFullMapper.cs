using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities.Attendance;
using PacketParser.Mapping;
using Serilog;
using TimeAttendanceDAL.Dal.Taradod;

namespace TimeAttendanceDAL.Mapper;

public class TaradodFullMapper : IItemAsyncMapper<TaradodFull, WebAttTaradodFull>
{
	public static TaradodFullMapper Instance { get; } = new TaradodFullMapper();


	public async Task<WebAttTaradodFull> MapAsync(string contextGuid, TaradodFull source, WebAttTaradodFull destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return null;
			}
			if (destination == null)
			{
				destination = new WebAttTaradodFull();
			}
			destination.SumMoazzafi = source.SumMoazzafi;
			destination.CodeTafsil = source.CodeTafsil;
			destination.CountDelayBeginShift = source.CountDelayBeginShift;
			destination.CountGheybat = source.CountGheybat;
			destination.CountMorakhasi = source.CountMorakhasi;
			destination.CountMorakhasiEstehghaghi = source.CountMorakhasiEstehghaghi;
			destination.SumMoazzafi = destination.SumMoazzafi;
			destination.Sumshift = source.SumMoazzafi;
			destination.Countshift = destination.Countshift;
			destination.SumEzafekarMid = source.SumEzafekarMid;
			destination.CountMorakhasiEstelaji = source.CountMorakhasiEstelaji;
			destination.CountMorakhasiEstehghaghi = source.CountMorakhasiEstehghaghi;
			destination.CountMorakhasiNotSalary = source.CountMorakhasiNotSalary;
			destination.CountMorakhasiWithSalary = source.CountMorakhasiWithSalary;
			destination.CountNaghes = source.CountNaghes;
			destination.CountTatil = source.CountTatil;
			destination.CountWorkdayBime = source.CountWorkdayBime;
			destination.CountWorkdayHoghogh = source.CountWorkdayHoghogh;
			destination.CountWorkdayMaliyat = source.CountWorkdayMaliyat;
			destination.Year = source.Year;
			destination.Month = source.Month;
			destination.GheybatMin = source.GheybatMin;
			destination.Sumshift = source.Sumshift;
			destination.SumFriDayWork = source.SumFriDayWork;
			destination.SumEzafekarEndTime = source.SumEzafekarEndTime;
			destination.SumEzafekarMid = source.SumEzafekarMid;
			destination.SumEzafekarMidTime = source.SumEzafekarMidTime;
			destination.SumEzafekarTime = source.SumMoazzafiTime;
			destination.SumMoazzafiTime = source.SumMoazzafiTime;
			destination.SumNobatKari1 = source.SumNobatKari1;
			destination.SumNobatKari1Time = source.SumNobatKari1Time;
			destination.SumNobatKari2 = source.SumNobatKari2;
			destination.SumNobatKari2Time = source.SumNobatKari2Time;
			destination.SumNobatKari3 = source.SumNobatKari3;
			destination.SumNobatKari3Time = source.SumNobatKari3Time;
			destination.SumShiftTime = source.SumShiftTime;
			destination.SumTatilDayWork = source.SumTatilDayWork;
			destination.SumWorkHalioDayWorkTime = source.SumWorkHalioDayWorkTime;
			destination.SumWorkMission = source.SumWorkMission;
			destination.SumWorkMissionTime = source.SumWorkMissionTime;
			destination.SumWorkNightWork = source.SumWorkNightWork;
			destination.SumWorkNightWorkTime = source.SumWorkNightWorkTime;
			destination.SumWorkVacation = source.SumWorkVacation;
			destination.SumWorkVacationTime = source.SumWorkVacationTime;
			destination.SumKasrkar = source.SumKasrkar;
			destination.SumKasrkarBegin = source.SumKasrkarBegin;
			destination.SumKasrkarBeginTime = source.SumKasrkarBeginTime;
			destination.SumKasrkarEnd = source.SumKasrkarEnd;
			destination.SumKasrkarEndTime = source.SumKasrkarEndTime;
			destination.SumKasrkarMid = source.SumKasrkarMid;
			destination.SumKasrkarMidTime = source.SumKasrkarMidTime;
			destination.SumKasrkarTime = source.SumKasrkarTime;
			destination.SumEzafekar = source.SumEzafekar;
			destination.SumEzafekarBegin = source.SumMoazzafi;
			destination.SumEzafekarBeginTime = source.SumEzafekarBeginTime;
			destination.SumEzafekarEnd = source.SumEzafekarEnd;
			destination.MissionDaily = source.MissionDaily;
			destination.SumWorkMission = source.SumWorkMission;
			destination.SumWorkMissionTime = source.SumWorkMissionTime;
			destination.SumJarimeRoz = source.SumJarimeRoz;
			destination.JarimeRoz = source.JarimeRoz;
			destination.CountKarkerd = source.CountKarkerd;
			destination.SumKarkerd = source.SumKasrkar;
			destination.SumGheybatMin = source.SumGheybatMin;
			destination.DateMin = source.DateMin;
			destination.DateMax = source.DateMax;
			destination.PersonId = source.PersonId;
			destination.FullName = source.FullName;
			destination.SumKarkerdTime = source.SumKarkerdTime;
			destination.SumWorkTatilWorkTime = source.SumWorkTatilWorkTime;
			destination.SumMovazafiForDisplay = source.SumMovazafiForDisplay;
			destination.SumKarkerdForDisplay = source.SumKarkerdForDisplay;
			destination.SumEzafekarForDisplay = source.SumEzafekarForDisplay;
			destination.SumKasrkarForDisplay = source.SumKasrkarForDisplay;
			destination.SumEzafekarBeginForDisplay = source.SumEzafekarBeginForDisplay;
			destination.SumEzafekarMidForDisplay = source.SumEzafekarMidForDisplay;
			destination.SumEzafekarEndDisplay = source.SumEzafekarEndDisplay;
			destination.SumKasrkarBeginForDisplay = source.SumKasrkarBeginForDisplay;
			destination.SumKasrkarMidForDisplay = source.SumKasrkarMidForDisplay;
			destination.SumKasrkarEndDisplay = source.SumKasrkarEndDisplay;
			WebAttTaradodFull val = destination;
			val.ListTaradodDets = await TaraododFullDetMapper.Instance.MapListAsync(contextGuid, source.ListTaradodDets);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
		return destination;
	}

	public Task<List<WebAttTaradodFull>> MapListAsync(string contextGuid, IEnumerable<TaradodFull> source, List<WebAttTaradodFull> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return MapperUtilities.MapListAsync<TaradodFull, WebAttTaradodFull>(source, contextGuid, (IItemAsyncMapper<TaradodFull, WebAttTaradodFull>)this, destination, tranName, (short)20);
	}
}
