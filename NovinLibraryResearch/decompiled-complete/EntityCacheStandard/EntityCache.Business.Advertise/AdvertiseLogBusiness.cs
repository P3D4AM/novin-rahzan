using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Advertise.NoBisunessModels;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Advertise;

namespace EntityCache.Business.Advertise;

public class AdvertiseLogBusiness : IAdvertiseLog, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	public List<string> ImagesPathList;

	public Guid Guid { get; set; } = Guid.NewGuid();


	public DateTime Modified { get; set; } = DateTime.Now;


	public bool Status => StatusCode == AdvStatusCode.Deleted;

	public string Title { get; set; } = "-";


	public string Content { get; set; } = "-";


	public long SimCardNumber { get; set; }

	public string State { get; set; } = "-";


	public string City { get; set; } = "-";


	public string Region { get; set; } = "-";


	public decimal Price { get; set; }

	public string Category { get; set; } = "برای کسب و کار";


	public string SubCategory1 { get; set; } = "تجهیزات و ماشین\u200cآلات";


	public string SubCategory2 { get; set; } = "فروشگاه و مغازه";


	public string URL { get; set; } = "-";


	public string IP { get; set; }

	public string UpdateDesc { get; set; } = "-";


	public int VisitCount { get; set; }

	public string ImagesPath
	{
		get
		{
			if (ImagesPathList == null || ImagesPathList.Count <= 0)
			{
				return "---";
			}
			return string.Join("\r\n", ImagesPathList);
		}
	}

	public string DateSh { get; set; } = DateTime.Now.MiladiToCalture();


	public DateTime DateM { get; set; } = DateTime.Now;


	public string Adv { get; set; } = "";


	public Guid MasterVisitorGuid { get; set; }

	public Guid SlaveVisitorGuid { get; set; }

	public AdvStatusCode StatusCode { get; set; }

	public AdvertiseType AdvType { get; set; }

	public DateTime LastUpdate { get; set; } = DateTime.Now;


	public string TypeName => AdvType.GetDisplay().GetTranslate();

	public Guid rnd => Guid.NewGuid();

	public string ContextGuid { get; set; }

	public AdvertiseLogBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static Task<AdvertiseLogBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IAdvertiseLog.GetAsync(guid, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(string contextGuid, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool transactionHasBeenCreated = false;
		try
		{
			_ = 1;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AdvDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvertiseLogBusiness.cs", 68), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvertiseLogBusiness.cs", 68);
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IAdvertiseLog.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvertiseLogBusiness.cs", 69);
				transactionHasBeenCreated = true;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvertiseLogBusiness.cs", 74);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvertiseLogBusiness.cs", 75);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvertiseLogBusiness.cs", 80), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvertiseLogBusiness.cs", 80);
			}
		}
		return ret;
	}

	public static Task<IEnumerable<AdvertiseLogBusiness>> GetAllAsync(string contextGuid, bool? status = true, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IAdvertiseLog.GetAllAsync(status, token, tranName);
	}

	public static Task<AdvertiseLogBusiness> GetLastAdvAsync(string contextGuid, string advName = "", string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IAdvertiseLog.GetLastAdvAsync(advName, tranName);
	}

	public static Task<List<int>> GetAdvCountInSpecialMounthAsync(string contextGuid, int day, AdvertiseType type, string tranName = "")
	{
		DateTime firstDate = DateTime.Now.AddDays(checked(-day));
		DateTime now = DateTime.Now;
		return ClsCache.GetCache(contextGuid).CacheRepository.IAdvertiseLog.GetAdvCountInSpecialMounthAsync(firstDate, now, type, tranName);
	}

	public static Task<List<int>> GetPublishedAdvCountInSpecialMounthAsync(string contextGuid, int day, AdvertiseType type, string tranName = "")
	{
		DateTime firstDate = DateTime.Now.AddDays(checked(-day));
		DateTime now = DateTime.Now;
		return ClsCache.GetCache(contextGuid).CacheRepository.IAdvertiseLog.GetPublishedAdvCountInSpecialMounthAsync(firstDate, now, type, tranName);
	}

	public static Task<List<AdvertiseLogBusiness>> GetAllSpecialAsync(string contextGuid, Expression<Func<IAdvertiseLog, bool>> where = null, Func<IQueryable<IAdvertiseLog>, IOrderedQueryable<IAdvertiseLog>> orderby = null, string includes = "", int takeCount = -1, string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IAdvertiseLog.GetAllSpecialAsync(where, orderby, includes, takeCount, tranName);
	}

	public static Task<IEnumerable<AdvertiseLogViewModel>> GetAllAsync(string contextGuid, long number, DateTime d1, DateTime d2, AdvStatusCode statusCode, AdvertiseType type, string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IAdvertiseLog.GetAllAsync(number, d1, d2, statusCode, type, tranName);
	}

	public static Task<AdvertiseLogBusiness> GetAsync(string contextGuid, string url, string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IAdvertiseLog.GetAsync(url, tranName);
	}

	public static Task<int> GetAllAdvInDayFromIP(string contextGuid, string ip, AdvertiseType type, string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IAdvertiseLog.GetAllAdvInDayFromIP(ip, type, tranName);
	}

	public static async Task<ReturnedSaveFuncInfo> RemoveRangeByDateAsync(string contextGuid, short tryCount = 10, string tranName = "")
	{
		return await ClsCache.GetCache(contextGuid).CacheRepository.IAdvertiseLog.RemoveRangeByDateAsync(tryCount, tranName);
	}

	public static async Task<List<long>> GetAllNotFull(string contextGuid, string tranName = "")
	{
		DateTime dateTime = DateTime.Now.AddMonths(-1);
		dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
		DateTime dateTime2 = DateTime.Now.AddDays(1.0);
		dateTime2 = new DateTime(dateTime2.Year, dateTime2.Month, dateTime2.Day, 0, 0, 0);
		List<long> retList = new List<long>();
		try
		{
			IEnumerable<AdvertiseLogBusiness> source = await ClsCache.GetCache(contextGuid).CacheRepository.IAdvertiseLog.GetAllNotFullAsync(dateTime, dateTime2, tranName);
			foreach (IGrouping<long, AdvertiseLogBusiness> item in (from q in source
				group q by q.SimCardNumber)?.ToList())
			{
				if (source.Count((AdvertiseLogBusiness q) => q.SimCardNumber == item.Key) < 5)
				{
					retList.Add(item.Key);
				}
				List<long> list = (from q in retList
					group q by q into q
					where q.Count() == 1
					select q.Key).ToList();
				retList = list;
			}
			DateTime dateTime3 = DateTime.Now.AddMonths(-1);
			dateTime3 = new DateTime(dateTime3.Year, dateTime3.Month, 30, 0, 0, 0);
			foreach (IGrouping<long, AdvertiseLogBusiness> item2 in (from q in await ClsCache.GetCache(contextGuid).CacheRepository.IAdvertiseLog.GetAllByNextUseAsync(dateTime3, tranName)
				group q by q.SimCardNumber)?.ToList())
			{
				retList.Add(item2.Key);
				List<long> list2 = (from q in retList
					group q by q into q
					where q.Count() == 1
					select q.Key)?.ToList();
				retList = list2;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllNotFull", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvertiseLogBusiness.cs", 156);
			return null;
		}
		return retList;
	}

	public static async Task<int> GetAdvCountInMount(string contextGuid, long number, AdvStatusCode status, string tranName = "")
	{
		DateTime date = DateTime.Now.AddMonths(-1);
		DateTime now = DateTime.Now;
		try
		{
			IEnumerable<AdvertiseLogBusiness> source = ((status == AdvStatusCode.Published) ? (await ClsCache.GetCache(contextGuid).CacheRepository.IAdvertiseLog.GetPublishAdvCountInMountAsync(date, now, number, tranName)) : (await ClsCache.GetCache(contextGuid).CacheRepository.IAdvertiseLog.GetAdvCountInMountAsync(date, now, number, tranName)));
			return source.Count();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAdvCountInMount", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvertiseLogBusiness.cs", 177);
			throw;
		}
	}

	public static Task<AdvertiseLogBusiness> GetLastSendAsync(string contextGuid, long number, AdvertiseType type, string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IAdvertiseLog.GetLastSendAsync(number, type, tranName);
	}

	public static Task<IEnumerable<AdvertiseLogBusiness>> GetAllEditNeededAdv(string contextGuid, string tranName = "")
	{
		try
		{
			DateTime date = DateTime.Now.AddMonths(-1);
			DateTime now = DateTime.Now;
			return ClsCache.GetCache(contextGuid).CacheRepository.IAdvertiseLog.GetAllEditNeedAsync(date, now, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllEditNeededAdv", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvertiseLogBusiness.cs", 196);
			return null;
		}
	}

	public static Task<IEnumerable<AdvCountReport>> GetAllAdvCountReportAsync(string contextGuid, string monthStart, string monthEnd, string dateSh, string ip, string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IAdvertiseLog.GetAllAdvCountReportAsync(monthStart, monthEnd, dateSh, ip, tranName);
	}
}
