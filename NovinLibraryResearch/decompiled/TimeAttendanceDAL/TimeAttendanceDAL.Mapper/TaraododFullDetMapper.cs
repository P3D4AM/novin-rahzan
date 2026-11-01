using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities.Attendance;
using PacketParser.Mapping;
using Serilog;
using TimeAttendanceDAL.Dal.Taradod;

namespace TimeAttendanceDAL.Mapper;

public class TaraododFullDetMapper : IItemAsyncMapper<TaradodFullDet, WebAttTaradodFullDet>
{
	public static TaraododFullDetMapper Instance { get; } = new TaraododFullDetMapper();


	public async Task<WebAttTaradodFullDet> MapAsync(string contextGuid, TaradodFullDet source, WebAttTaradodFullDet destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 18;
		try
		{
			if (source == null)
			{
				destination = null;
				return null;
			}
			if (destination == null)
			{
				destination = new WebAttTaradodFullDet();
			}
			destination.DateM = source.DateM;
			WebAttTaradodFullDet val = destination;
			val.StatuseTaradod = await source.GetStatuseTaradodAsync(token);
			destination.ReloadNextRow = source.ReloadNextRow;
			val = destination;
			val.OnTatilWorkKarkard = await source.GetOnTatilWorkKarkardAsync(token);
			destination.ShiftTime = source.ShiftTime;
			val = destination;
			val.ShiftWorkKarkard1 = await source.GetShiftWorkKarkard1Aaync(token);
			val = destination;
			val.ShiftWorkKarkard2 = await source.GetShiftWorkKarkard2Async(token);
			val = destination;
			val.ShiftWorkKarkard3 = await source.GetShiftWorkKarkard3Async(token);
			destination.ColorManulaShiftTime1 = source.ColorManulaShiftTime1;
			destination.ColorManulaShiftTime2 = source.ColorManulaShiftTime2;
			destination.ColorManulaShiftTime3 = source.ColorManulaShiftTime3;
			destination.ColorManulaVacationTime = source.ColorManulaVacationTime;
			destination.DayofShMonth = source.DayofShMonth;
			destination.Dayofweek = source.Dayofweek;
			destination.DetailStatusTypeTaradod = source.DetailStatusTypeTaradod;
			val = destination;
			val.EkTime = await source.GetEkTimeAsync(token);
			val = destination;
			val.Karkard = await source.GetShiftWorkKarkard3Async(token);
			val = destination;
			val.KkTime = await source.GetKkTimeAsync(token);
			val = destination;
			val.LunchTime = await source.GetShiftWorkKarkard3Async(token);
			destination.KasreKarBeginShift = source.KasreKarBeginShift;
			destination.EzafeKarBeginShift = source.EzafeKarBeginShift;
			destination.EzafeKarEndShift = source.EzafeKarEndShift;
			destination.KasreKarEndShift = source.KasreKarEndShift;
			destination.EzafeKarMidShift = source.EzafeKarMidShift;
			destination.ManulaEzafeKarTime = source.ManulaEzafeKarTime;
			destination.ManulaKasreKarTime = source.ManulaKasreKarTime;
			destination.ManulaMissionTime = source.ManulaMissionTime;
			destination.ManulaVacationTime = source.ManulaVacationTime;
			destination.ManulaFridayTime = source.ManulaFridayTime;
			destination.ManulaShiftTime1 = source.ManulaShiftTime1;
			destination.ManulaShiftTime2 = source.ManulaShiftTime2;
			destination.ManulaShiftTime3 = source.ManulaShiftTime3;
			destination.ManulaAbsentTime = source.ManulaAbsentTime;
			destination.ManulaNightTime = source.ManulaNightTime;
			destination.ColorManulaEzafeKar = source.ColorManulaEzafeKar;
			destination.ColorManulaKasreKarTime = source.ColorManulaKasreKarTime;
			destination.ColorManulaMissionTime = source.ColorManulaMissionTime;
			destination.ColorManulaVacationTime = source.ColorManulaVacationTime;
			destination.ColorManulaFridayTime = source.ColorManulaFridayTime;
			destination.ColorManulaShiftTime1 = source.ColorManulaShiftTime1;
			destination.ColorManulaShiftTime2 = source.ColorManulaShiftTime2;
			destination.ColorManulaShiftTime3 = source.ColorManulaShiftTime3;
			destination.ColorManulaAbsentTime = source.ColorManulaAbsentTime;
			destination.ColorManulaNightTime = source.ColorManulaNightTime;
			val = destination;
			val.HozorTime = await source.GetHozorTimeAsync(token);
			destination.MorSaatiTime = source.MorSaatiTime;
			destination.MissionTime = source.MissionTime;
			destination.CodeTafsil = source.CodeTafsil;
			destination.FullName = source.FullName;
			destination.Family = source.Family;
			destination.FirstName = source.FirstName;
			val = destination;
			val.GheybatTime = await source.GetGheybatTimeAsync(token);
			destination.CanEditV = source.CanEditV;
			destination.CanEditK = source.CanEditK;
			destination.ColorEk = source.ColorEk;
			destination.DateSh = source.DateSh;
			destination.TatilRasmiToolTip = source.TatilRasmiToolTip;
			destination.MorakhasiToolTip = source.MorakhasiToolTip;
			val = destination;
			val.GheybatMin = await source.GetGheybatMinAsync(token);
			destination.ColorKk = source.GetColorKk();
			val = destination;
			val.KasrKar = await source.GetKasrKarAsync(token);
			destination.JarimeRoz = source.JarimeRoz;
			destination.KasreKarMidShift = source.KasreKarMidShift;
			val = destination;
			val.EzafeKar = await source.GetEzafeKarAsync(token);
			val = destination;
			val.OnFriDayWorkKarkard = await source.GetOnFriDayWorkKarkardAsync(token);
			val = destination;
			val.NightWorkKarkard = await source.GetNightWorkKarkardAsync(token);
			destination.SumShiftMinuts = source.SumShiftMinuts;
			val = destination;
			val.SumMoazafiMinutes = await source.GetShiftWorkKarkard2Async(token);
			val = destination;
			val.CoupleTaradods = await CoupleTaradodMapper.Instance.MapListAsync(contextGuid, source.CoupleTaradodReadOnly);
			if (source.AttTaradodDet != null)
			{
				val = destination;
				val.TaradodDet = await TaradodDetMapper.Instance.MapAsync(contextGuid, source.AttTaradodDet);
			}
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
		return destination;
	}

	public Task<List<WebAttTaradodFullDet>> MapListAsync(string contextGuid, IEnumerable<TaradodFullDet> source, List<WebAttTaradodFullDet> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return MapperUtilities.MapListAsync<TaradodFullDet, WebAttTaradodFullDet>(source, contextGuid, (IItemAsyncMapper<TaradodFullDet, WebAttTaradodFullDet>)this, destination, tranName, (short)20);
	}
}
