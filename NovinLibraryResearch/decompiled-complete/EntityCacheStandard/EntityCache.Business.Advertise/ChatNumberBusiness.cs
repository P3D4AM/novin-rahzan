using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Advertise;

namespace EntityCache.Business.Advertise;

public class ChatNumberBusiness : Serializable<ChatNumberBusiness>, IChatNumber, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; } = true;


	public string Number { get; set; }

	public DateTime DateM { get; set; }

	public string City { get; set; }

	public string Category { get; set; }

	public bool IsUsed { get; set; }

	public string DateSh => DateM.MiladiToCalture();

	public string ContextGuid { get; set; }

	public ChatNumberBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static Task<bool> CheckNumber(string contextGuid, string number, string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ChatNumberRepository.CheckNumber(number, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AdvDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\ChatNumberBusiness.cs", 44), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\ChatNumberBusiness.cs", 44);
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ChatNumberRepository.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\ChatNumberBusiness.cs", 45);
				transactionHasBeenCreated = true;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\ChatNumberBusiness.cs", 50);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\ChatNumberBusiness.cs", 51);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\ChatNumberBusiness.cs", 56), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\ChatNumberBusiness.cs", 56);
			}
		}
		return ret;
	}

	public static Task<IEnumerable<ChatNumberBusiness>> GetAllAsync(string contextGuid, bool? status = true, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ChatNumberRepository.GetAllAsync(status, token, tranName);
	}

	public static async Task<List<string>> GetAllCategories(string contextGuid, string tranName = "")
	{
		return await ClsCache.GetCache(contextGuid).CacheRepository.ChatNumberRepository.GetAllCategories(tranName);
	}

	public static async Task<List<string>> GetAllCities(string contextGuid, string tranName = "")
	{
		return await ClsCache.GetCache(contextGuid).CacheRepository.ChatNumberRepository.GetAllCities(tranName);
	}

	public static async Task<ChatNumberBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return await ClsCache.GetCache(contextGuid).CacheRepository.ChatNumberRepository.GetAsync(guid, token, tranName);
	}

	public static async Task<List<ChatNumberBusiness>> GetAllAsync(string contextGuid, string search, string cat, string city, bool isUnUsed, bool? status = true, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\ChatNumberBusiness.cs"))
		{
			try
			{
				if (string.IsNullOrEmpty(search))
				{
					search = "";
				}
				IEnumerable<ChatNumberBusiness> enumerable = await GetAllAsync(contextGuid, status, token);
				if (enumerable == null || !enumerable.Any())
				{
					return null;
				}
				if (isUnUsed)
				{
					enumerable = enumerable?.Where((ChatNumberBusiness q) => !q.IsUsed);
				}
				if (!string.IsNullOrEmpty(cat) && !cat.Contains("همه دسته بندی ها"))
				{
					enumerable = enumerable.Where((ChatNumberBusiness q) => !string.IsNullOrEmpty(q.Category) && q.Category.Trim() == cat.Trim());
				}
				if (!string.IsNullOrEmpty(city) && !city.Contains("همه شهرها"))
				{
					enumerable = enumerable.Where((ChatNumberBusiness q) => !string.IsNullOrEmpty(q.City) && q.City.Trim() == city.Trim());
				}
				List<string> list = search.SplitString();
				if (list != null && list.Count > 0)
				{
					foreach (string item in list)
					{
						if (!string.IsNullOrEmpty(item) && item.Trim() != "")
						{
							enumerable = enumerable?.Where((ChatNumberBusiness x) => x.Number.ToString().Contains(item) || x.Category.Contains(item) || x.City.Contains(item));
						}
					}
				}
				enumerable = enumerable?.OrderByDescending((ChatNumberBusiness o) => o.DateM);
				return enumerable.ToList();
			}
			catch (OperationCanceledException)
			{
				return null;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\ChatNumberBusiness.cs", 111);
				return new List<ChatNumberBusiness>();
			}
		}
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(string contextGuid, List<ChatNumberBusiness> lstNumbers, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\ChatNumberBusiness.cs", 125), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\ChatNumberBusiness.cs", 125);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.ChatNumberRepository.SaveRangeAsync(lstNumbers, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\ChatNumberBusiness.cs", 127);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\ChatNumberBusiness.cs", 132);
				ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\ChatNumberBusiness.cs", 133);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\ChatNumberBusiness.cs", 138), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\ChatNumberBusiness.cs", 138);
			}
		}
		return ret;
	}
}
