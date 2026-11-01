using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities.Attendance;
using PacketParser.Mapping;
using Serilog;
using TimeAttendanceDAL.Dal.Salary;

namespace TimeAttendanceDAL.Mapper;

public class FishMapper : IItemAsyncMapper<Fish, WebAttFish>
{
	public static FishMapper Instance { get; } = new FishMapper();


	public async Task<WebAttFish> MapAsync(string contextGuid, Fish source, WebAttFish destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new WebAttFish();
			}
			destination.Id = source.Id;
			destination.PersonId = source.PersonId;
			destination.FishNo = source.FishNo;
			destination.DateStartM = source.DateStartM;
			destination.DateEndM = source.DateEndM;
			destination.Description = source.Description;
			destination.St = source.St;
			destination.VacationST = source.VacationST;
			destination.KarkerdMin = source.KarkerdMin;
			destination.KarkerdPrice = source.KarkerdPrice;
			destination.EzafeMin = source.EzafeMin;
			destination.EzafeMin1 = source.EzafeMin1;
			destination.EzafeMin2 = source.EzafeMin2;
			destination.EzafeMin3 = source.EzafeMin3;
			destination.EzafePrice = source.EzafePrice;
			destination.EzafePrice1 = source.EzafePrice1;
			destination.EzafePrice2 = source.EzafePrice2;
			destination.EzafePrice3 = source.EzafePrice3;
			destination.ShabMin = source.ShabMin;
			destination.ShabPrice = source.ShabPrice;
			destination.NobatMin1 = source.NobatMin1;
			destination.NobatMin2 = source.NobatMin2;
			destination.NobatMin3 = source.NobatMin3;
			destination.NobatPrice1 = source.NobatPrice1;
			destination.NobatPrice2 = source.NobatPrice2;
			destination.NobatPrice3 = source.NobatPrice3;
			destination.JomeMin = source.JomeMin;
			destination.TatilMin = source.TatilMin;
			destination.Mozafimin = source.MoazafiMin;
			destination.JarimeRoz = source.JarimeRoz;
			destination.JarimePrice = source.JarimePrice;
			destination.TatilPrice = source.TatilPrice;
			destination.JarimePrice = source.JarimePrice;
			destination.VacationMin = source.VacationMin;
			destination.VacationPrice = source.VacationPrice;
			destination.VacationMinUsed = source.VacationMinUsed;
			destination.GheybatMin = source.GheybatMin;
			destination.GheybatPrice = source.GheybatPrice;
			destination.KasrMin = source.KasrMin;
			destination.KasrMin1 = source.KasrMin1;
			destination.KasrMin2 = source.KasrMin2;
			destination.KasrMin3 = source.KasrMin3;
			destination.KasrPrice = source.KasrPrice;
			destination.KasrPrice1 = source.KasrPrice1;
			destination.KasrPrice2 = source.KasrPrice2;
			destination.KasrPrice3 = source.KasrPrice3;
			destination.IsPrint = source.IsPrint;
			destination.SumKarkerd = source.SumKarkerd;
			destination.SumMazaya = source.SumMazaya;
			destination.SumKosorat = source.SumKosorat;
			destination.SumKhales = source.SumKhales;
			destination.CountKarKerd = source.CountKarKerd;
			destination.CountKarKerdBime = source.CountWorkdayBime;
			destination.CountKarKerdMaliat = source.CountWorkdayMaliyat;
			destination.CountDaysOfMonth = source.CountDaysOfMonth;
			destination.CountShift = source.Countshift;
			destination.CountVacationWithHoghogh = source.CountMorakhasiWithSalary;
			destination.CountVacationNotHoghogh = source.CountMorakhasiNotSalary;
			destination.CountMorakhasi = source.CountMorakhasi;
			destination.CountGheybat = source.CountGheybat;
			destination.CountNaghes = source.CountNaghes;
			destination.CountTatil = source.CountTatil;
			destination.MissionPrice = source.MissionPrice;
			destination.MissionRoz = source.MissionRoz;
			WebAttFish val = destination;
			val.VacationRemind = await source.GetVacationRemindAaync(token);
			destination.DateEndSh = source.DateEndSh;
			destination.DateStartSh = source.DateStartSh;
			destination.GheybatRoze = source.GheybatRoze;
			destination.SumVacation = source.SumVacation;
			destination.SumGheybat = source.SumGheybat;
			destination.SumMission = source.SumMission;
			destination.SumKasrKar1 = source.SumKasrKar1;
			destination.SumKasrKar2 = source.SumKasrKar2;
			destination.SumKasrKar3 = source.SumKasrKar3;
			destination.SumTotalKarkard = source.GetSumKolKarkerd();
			destination.SumEzafehKar1 = source.SumEzafeKar1;
			destination.SumEzafehKar2 = source.SumEzafeKar2;
			destination.SumEzafehKar3 = source.SumEzafeKar3;
			destination.SumShabKar = source.SumShabKar;
			destination.SumNobatKar = source.SumNobatKar;
			destination.AVGNobatKary = source.AVGNobatKary;
			destination.SumJomeKary = source.SumJomeKary;
			val = destination;
			val.ListFishDetails = await FishDetailMapper.Instance.MapListAsync(contextGuid, source.ListFishDet, null, "", token);
			destination.SodourDate = source.DateM;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
		return destination;
	}

	public Task<List<WebAttFish>> MapListAsync(string contextGuid, IEnumerable<Fish> source, List<WebAttFish> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return MapperUtilities.MapListAsync<Fish, WebAttFish>(source, contextGuid, (IItemAsyncMapper<Fish, WebAttFish>)this, destination, tranName, (short)20);
	}
}
