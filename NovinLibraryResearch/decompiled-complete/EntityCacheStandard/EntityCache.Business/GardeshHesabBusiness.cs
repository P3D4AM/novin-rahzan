using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Mapping.BusinessMappers;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.DalInterfaces;
using PacketParser.Entities;
using PacketParser.Entities.Accounting;
using PacketParser.Entities.Filters;
using PacketParser.Entities.Interfaces;
using Serilog;

namespace EntityCache.Business;

public class GardeshHesabBusiness : PacketParser.Entities.Accounting.GardeshHesab, IHasContext
{
	private DateTime? _dateMoasser;

	public new List<GardeshTafsilBusiness> TafsiliItems;

	public string ContextGuid { get; set; }

	public DateTime DateMoaser
	{
		get
		{
			if (_dateMoasser.HasValue)
			{
				return _dateMoasser.Value;
			}
			return GetDateMoaser(EnRasGiriType.Standard, default(CancellationToken));
		}
	}

	public string DateMoaserSh
	{
		get
		{
			return DateMoaser.MiladiToCalture();
		}
		set
		{
		}
	}

	public GardeshHesabBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	private GardeshHesabBusiness(string contextGuid, IDalTurnover iDalTurnover, int rowIndex)
		: base(iDalTurnover, rowIndex)
	{
		ContextGuid = contextGuid;
	}

	public static async Task<Tuple<List<GardeshHesabBusiness>, List<GardeshTafsilBusiness>>> GetGardeshTajmieiFromVSanadDetAsync(string contextGuid, List<Guid> listTafsilGuid, EnSystem system, int top, DateTime? Date1 = null, DateTime? Date2 = null, List<Guid> listMoeinGuid = null, Guid? kolGuid = null, CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			IEnumerable<GardeshTafsilBusiness> source = await GardeshTafsilBusiness.GetFromVSanadDetAsync(contextGuid, system, listTafsilGuid, top, Date1, Date2, listMoeinGuid, kolGuid, true, "", token);
			await GardeshTafsilMapper.Instance.MapListAsync(contextGuid, source, new List<GardeshTafsil>(), "", token);
			return null;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetGardeshTajmieiFromVSanadDetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GardeshHesabBusiness.cs", 40);
			return new Tuple<List<GardeshHesabBusiness>, List<GardeshTafsilBusiness>>(new List<GardeshHesabBusiness>(), new List<GardeshTafsilBusiness>());
		}
	}

	public RasGiriDetBusiness GetRasGiriItem(CancellationToken token)
	{
		return new RasGiriDetBusiness(ContextGuid)
		{
			DateM = GetDateMoaser(EnRasGiriType.Standard, token),
			Description = base.DescForDisplay,
			Price = base.Credit - base.Debit
		};
	}

	public DateTime GetDateMoaser(EnRasGiriType rasGiriType, CancellationToken token)
	{
		try
		{
			List<GardeshTafsil> lstTafsiliItems = base.LstTafsiliItems;
			if (lstTafsiliItems == null || !lstTafsiliItems.Any())
			{
				return DateTime.Now;
			}
			if ((base.LstTafsiliItems?.Count ?? 0) == 1)
			{
				return base.LstTafsiliItems.Single().DateMoaser;
			}
			_dateMoasser = CalculateRasDates(base.LstTafsiliItems, rasGiriType, token);
			return _dateMoasser ?? DateTime.Now;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetDateMoaser", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GardeshHesabBusiness.cs", 65);
			return DateTime.Now;
		}
	}

	private DateTime CalculateRasDates(List<GardeshTafsil> gardeshList, EnRasGiriType rasGiriType, CancellationToken token)
	{
		try
		{
			if (token.IsCancellationRequested)
			{
				return DateTime.Now;
			}
			RasGiriBusiness rasGiriBusiness = new RasGiriBusiness(ContextGuid)
			{
				RasGiriType = rasGiriType,
				BaseDateM = DateTime.Now
			};
			if (gardeshList == null || !gardeshList.Any())
			{
				return DateTime.Now;
			}
			if (token.IsCancellationRequested)
			{
				return DateTime.Now;
			}
			foreach (GardeshTafsil gardesh in gardeshList)
			{
				if (token.IsCancellationRequested)
				{
					return DateTime.Now;
				}
				rasGiriBusiness.AddItem(new RasGiriDetBusiness(ContextGuid)
				{
					DateM = gardesh.DateMoaser,
					Price = gardesh.Debit - gardesh.Credit
				}, token);
			}
			return DateTime.Now.AddDays(rasGiriBusiness.RasgiriCalculator.CalCulateRasDaysCalculator());
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CalculateRasDates", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GardeshHesabBusiness.cs", 92);
			return DateTime.Now;
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<IEnumerable<GardeshHesabBusiness>>> GetGardeshTajmieiAsync(string contextGuid, GardeshTafsiliFilter filter, EnSystem system, string tranName = "", CancellationToken token = default(CancellationToken), int timeOutMiliSeconds = 30000)
	{
		Stopwatch sw = Stopwatch.StartNew();
		string debugInfo = "Filter:" + Json.ToStringJson(filter);
		ReturnedSaveFuncInfoWithValue<IEnumerable<GardeshHesabBusiness>> ret = new ReturnedSaveFuncInfoWithValue<IEnumerable<GardeshHesabBusiness>>();
		try
		{
			ReturnedSaveFuncInfoWithValue<List<GardeshTafsilBusiness>> tafsili = await GardeshTafsilBusiness.GetAllAsync(contextGuid, filter, system, tranName, token, timeOutMiliSeconds);
			ret.AddReturnedValue(tafsili, ReturnedState.None, "GetGardeshTajmieiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GardeshHesabBusiness.cs", 105);
			if (ret.HasError)
			{
				return ret;
			}
			if (!(tafsili?.value?.Any()).GetValueOrDefault())
			{
				return ret;
			}
			IEnumerable<CheckedSaveBusiness> checkedList = await CheckedSaveBusiness.GetAllAsync(contextGuid, checkedTablReportType.GardeshTajmiee, filter.TafsilGuid ?? Guid.Empty, true, token);
			ReturnedSaveFuncInfoWithValue<IEnumerable<GardeshHesabBusiness>> returnedSaveFuncInfoWithValue = ret;
			returnedSaveFuncInfoWithValue.value = await GetGardeshTajmieiAsync(contextGuid, filter.TafsilGuid, tafsili.value, checkedList);
			ret.value = ret.value?.OrderBy((GardeshHesabBusiness p) => p.RowIndex)?.ToList();
		}
		catch (Exception ex)
		{
			ret.AddReturnedValue(ex, "", "GetGardeshTajmieiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GardeshHesabBusiness.cs", 114);
			WebErrorLog.Instance.StartLog(ex, "DebugInfo:" + debugInfo, "GetGardeshTajmieiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GardeshHesabBusiness.cs", 115);
		}
		finally
		{
			ILogger obj = LogCache.Instance.Value.Get("UiLogger");
			if (obj != null)
			{
				obj.Debug($"GardeshHesab|Business.GetGardeshTajmieiAsync from database finiehsed on {sw.ElapsedMilliseconds}(ms) records({ret.value?.Count() ?? 0})rows");
			}
		}
		return ret;
	}

	public static async Task<List<GardeshHesabBusiness>> GetGardeshTajmieiAsync(string contextGuid, Guid? tafsilGuid, IEnumerable<GardeshTafsilBusiness> gardeshTafsili, IEnumerable<CheckedSaveBusiness> checkedList)
	{
		Stopwatch sw = Stopwatch.StartNew();
		List<GardeshHesabBusiness> ret = new List<GardeshHesabBusiness>();
		try
		{
			CancellationToken token = default(CancellationToken);
			List<GardeshTafsil> list = (await GardeshTafsilMapper.Instance.MapListAsync(contextGuid, gardeshTafsili, new List<GardeshTafsil>(), "", token))?.OrderBy((GardeshTafsil p) => p.RowIndex)?.ToList() ?? new List<GardeshTafsil>();
			DalTurnover dalTurnover = DalTurnover.Get(contextGuid, ClsCache.GetCache(contextGuid).Source);
			foreach (GardeshTafsil item in list)
			{
				if (checkedList?.Any() ?? false)
				{
					CheckedSaveBusiness checkedSaveBusiness = checkedList?.FirstOrDefault(delegate(CheckedSaveBusiness q)
					{
						if (q.ReportType == checkedTablReportType.GardeshTajmiee && item.AsGuid.HasValue)
						{
							Guid detailGuid = q.DetailGuid;
							Guid? asGuid = item.AsGuid;
							return detailGuid == asGuid;
						}
						return false;
					});
					item.IsChecked = checkedSaveBusiness != null;
				}
				item.DalTurnover = dalTurnover;
			}
			IEnumerable<PacketParser.Entities.Accounting.GardeshHesab> fromSanadDetail = PacketParser.Entities.Accounting.GardeshHesab.GetFromSanadDetail(tafsilGuid, dalTurnover, list);
			ret = await GardeshHesabMapper.Instance.MapListAsync(contextGuid, fromSanadDetail, ret, "", token);
			ret = ret?.OrderBy((GardeshHesabBusiness p) => p.RowIndex)?.ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetGardeshTajmieiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GardeshHesabBusiness.cs", 157);
			throw;
		}
		finally
		{
			ILogger obj = LogCache.Instance.Value.Get("UiLogger");
			if (obj != null)
			{
				obj.Debug($"GardeshHesab|Business.GetGardeshTajmieiAsync from list of gardeshTafsil finiehsed on {sw.ElapsedMilliseconds}(ms) records({ret?.Count() ?? 0})rows");
			}
		}
		return ret;
	}
}
