using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Tolid.Base;

namespace EntityCache.Business.Accounting.Tolid.Base;

public abstract class BaseTolidBusiness : IHasContext, IHasJustGuid, IHasStatus
{
	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	public string ContextGuid { get; set; }

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now;


	public DateTime DateM { get; set; } = DateTime.Now;


	public string DateSh => DateM.MiladiToCalture();

	public decimal HazineSum => Math.Round(HazineList?.Sum((IBaseHazineBusiness q) => q.Price) ?? 0m, DigitAfterPoint);

	[JsonIgnore]
	public short DigitForRound { get; }

	[JsonIgnore]
	public short DigitAfterPoint { get; }

	public abstract IReadOnlyList<IBaseHazineBusiness> HazineList { get; }

	public BaseTolidBusiness(string contextGuid)
	{
		try
		{
			ContextGuid = contextGuid;
			DigitForRound = (ClsCache.GetCache(ContextGuid)?.MultiLanguageConfig?.DigitForRound).GetValueOrDefault(3);
			DigitAfterPoint = (ClsCache.GetCache(ContextGuid)?.MultiLanguageConfig?.DigitAfterPoint).GetValueOrDefault();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Base\\BaseTolidBusiness.cs", 38);
		}
	}

	public async Task<decimal> GetMavadSumAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return Math.Round((await GetMavadListAsync(system, tranName, token))?.SumKalaKhadamat ?? 0m, DigitAfterPoint);
	}

	public async Task<decimal> GetExportSumAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return Math.Round((await GetExportListAsync(system, tranName, token))?.SumKalaKhadamat ?? 0m, DigitAfterPoint);
	}

	public abstract Task<OrderBusiness> GetExportListAsync(EnSystem enSystem, string tranName, CancellationToken token = default(CancellationToken));

	public abstract Task<OrderBusiness> GetMavadListAsync(EnSystem system, string tranName, CancellationToken token = default(CancellationToken));

	public abstract Task AddHazineListAsync(IEnumerable<IBaseHazineBusiness> lstObjects, EnSystem system, CancellationToken token = default(CancellationToken));

	public abstract Task AddMavadListAsync(EnSystem system, IEnumerable<OrderDetailBusiness> objects, bool clearDets = false, string tranName = "", CancellationToken token = default(CancellationToken));

	public abstract Task AddExportListAsync(EnSystem system, IEnumerable<OrderDetailBusiness> objects, bool clearDets = false, string tranName = "", CancellationToken token = default(CancellationToken));

	public Task AddHazineAsync(IBaseHazineBusiness objects, EnSystem system)
	{
		return AddHazineListAsync(new List<IBaseHazineBusiness> { objects }, system);
	}

	public Task AddExportAsync(EnSystem system, OrderDetailBusiness obj, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return AddExportListAsync(system, new List<OrderDetailBusiness> { obj }, clearDets: false, tranName, token);
	}

	public abstract Task RemoveFromListMavadAsync(EnSystem system, Guid guid, CancellationToken token = default(CancellationToken));

	public abstract Task RemoveFromListHazineAsync(Guid guid, CancellationToken token = default(CancellationToken));

	public virtual async Task<ReturnedSaveFuncInfo> CheckValidationAsync(EnSystem system, bool isDeleteAction, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			IReadOnlyList<OrderDetailBusiness> exportList = (await GetExportListAsync(system, tranName, token))?.ListKala;
			IReadOnlyList<OrderDetailBusiness> readOnlyList = (await GetMavadListAsync(system, tranName, token))?.ListKala;
			if (!(exportList?.Any() ?? false) && !isDeleteAction)
			{
				res.AddError(system, "هیچ کالایی برای تولید انتخاب نشده است".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Base\\BaseTolidBusiness.cs", 65);
				return res;
			}
			foreach (OrderDetailBusiness item in exportList)
			{
				if (item.ProductGuid == Guid.Empty)
				{
					res.AddError("کالای تولید شده انتخاب نشده است".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Base\\BaseTolidBusiness.cs", 72);
				}
				if (item.Quantity <= 0m)
				{
					res.AddError("".GetTranslate() + "برای کالای".GetTranslate() + " " + (" " + item.ProductName).GetTranslate() + " در ردیف ".GetTranslate() + " " + $"{item.Ordinal}" + " " + " کالای تولید شده، مقدار وارد نشده است".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Base\\BaseTolidBusiness.cs", 75);
				}
			}
			if ((readOnlyList == null || !readOnlyList.Any()) && !isDeleteAction)
			{
				res.AddError(system, "هیچ مواد مصرفی وارد نشده است".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Base\\BaseTolidBusiness.cs", 80);
				return res;
			}
			foreach (OrderDetailBusiness item2 in readOnlyList)
			{
				if (item2.Quantity <= 0m)
				{
					res.AddError("برای کالای".GetTranslate() + " " + (" " + item2.ProductName).GetTranslate() + " " + " در ردیف ".GetTranslate() + " " + $"{item2.Ordinal}".GetTranslate() + " " + " مواد مصرفی، مقدار وارد نشده است".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Base\\BaseTolidBusiness.cs", 86);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Base\\BaseTolidBusiness.cs", 90);
			res.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Base\\BaseTolidBusiness.cs", 91);
		}
		return res;
	}

	public virtual Task RaiseEventAsync(CancellationToken token = default(CancellationToken))
	{
		try
		{
			ClsCache.GetCache(ContextGuid).BusinessAggregator.OrderBusiness.RaiseOnSumChangedAsync(token);
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "RaiseEventAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Base\\BaseTolidBusiness.cs", 102);
		}
		return Task.CompletedTask;
	}
}
