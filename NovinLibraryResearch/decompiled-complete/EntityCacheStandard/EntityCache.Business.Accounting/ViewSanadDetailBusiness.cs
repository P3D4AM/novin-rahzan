using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;
using PacketParser.EventArgs;
using PacketParser.Settings.Accounting;

namespace EntityCache.Business.Accounting;

public class ViewSanadDetailBusiness : IViewSanadDetail, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, ISyncFromClient2Server<ViewSanadDetailBusiness>, IDeliverableEntity, IEditableDelivery, IHasContext
{
	public Guid Guid { get; set; }

	public DateTime DateM { get; set; }

	public string DateSh => DateM.MiladiToCalture();

	public string Description { get; set; }

	public Guid? AsGuid { get; set; }

	public Document AsType { get; set; }

	public decimal Debit { get; set; }

	public decimal Credit { get; set; }

	public int SanadNo { get; set; }

	public SanadSt SanadSt { get; set; }

	public decimal Qt { get; set; }

	public Guid? DecimalGuid { get; set; }

	public string RowDesc { get; set; }

	public Guid KolGuid { get; set; }

	public Guid MoeinGuid { get; set; }

	public Guid? TafsilGuid { get; set; }

	public Guid? TafsilGuid2 { get; set; }

	public Guid? TafsilGuid3 { get; set; }

	public Guid? TafsilGuid4 { get; set; }

	public Guid CashierGuid { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now;


	public bool Status { get; set; } = true;


	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public string ContextGuid { get; set; }

	public string Serial { get; set; }

	public ViewSanadDetailBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token, string tranName, short maxparalelTask)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> SetDeliveryAsync(WebDelivery del, CancellationToken token, short tryCount = 10, string tranName = "")
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token, short tryCount, string tranName)
	{
		throw new NotImplementedException();
	}

	public static async Task<IEnumerable<ViewSanadDetailBusiness>> GetAllSanadOutOfFinancialPeriodAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			AccGlobalSettings obj = await LocalSettingBusiness.GetSettingAsync(contextGuid, token);
			DateTime maliyearStart = obj.MaliYearSettings.MaliyearStart;
			DateTime maliyearEnd = obj.MaliYearSettings.MaliyearEnd;
			return await ClsCache.GetCache(contextGuid).CacheRepository.ViewSanadDetail.GetAllSanadOutOfFinancialPeriodAsync(maliyearStart, maliyearEnd, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllSanadOutOfFinancialPeriodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\ViewSanadDetailBusiness.cs", 64);
			return null;
		}
	}

	public static Task<IEnumerable<ViewSanadDetailBusiness>> GetAllAsync(string contextGuid, int sanadNo, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ViewSanadDetail.GetAllAsync(sanadNo, token, tranName);
	}

	public static Task<int> GetSanadNoAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ViewSanadDetail.GetSanadNoAsync(guid, token, tranName);
	}

	public static Task<int> GetFutureSanadCountAsync(string contextGuid, DateTime lastDate, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ViewSanadDetail.GetFutureSanadCountAsync(lastDate, token, tranName);
	}
}
