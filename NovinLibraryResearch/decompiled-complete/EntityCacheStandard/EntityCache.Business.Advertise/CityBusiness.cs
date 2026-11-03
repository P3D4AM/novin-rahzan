using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Advertise;

namespace EntityCache.Business.Advertise;

public class CityBusiness : ICity, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now;


	public bool Status { get; set; } = true;


	public string CityName { get; set; }

	public Guid StateGuid { get; set; }

	public int Weight { get; set; }

	public bool isDivarCity { get; set; }

	public bool isSheypoorCity { get; set; }

	public bool isNiazKadeCity { get; set; }

	public bool isNiazmandyHaCity { get; set; }

	public int StartPoint { get; set; }

	public int EndPoint => checked(StartPoint + Weight);

	[JsonIgnore]
	public bool IsChecked { get; set; }

	public string StateName { get; set; }

	public string ContextGuid { get; set; }

	public CityBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AdvDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\CityBusiness.cs", 45), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\CityBusiness.cs", 45);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ICity.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\CityBusiness.cs", 47);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\CityBusiness.cs", 51);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\CityBusiness.cs", 52);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\CityBusiness.cs", 57), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\CityBusiness.cs", 57);
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(List<CityBusiness> list, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			foreach (CityBusiness item in list)
			{
				if (item != null)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\CityBusiness.cs", 71);
				}
			}
			return res;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\CityBusiness.cs", 76);
			throw;
		}
	}

	public static Task<CityBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ICity.GetAsync(guid, token, tranName);
	}

	public static Task<IEnumerable<CityBusiness>> GetAllAsync(string contextGuid, bool? status = true, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ICity.GetAllAsync(status, token, tranName);
	}

	public static Task<IEnumerable<CityBusiness>> GetAllAsync(string contextGuid, Guid stateGuid, bool? status = true, string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ICity.GetAllAsync(stateGuid, status, tranName);
	}

	public static Task<CityBusiness> GetAsync(string contextGuid, string name, string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ICity.GetAsync(name, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(string contextGuid, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			_ = 1;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AdvDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\CityBusiness.cs", 101), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\CityBusiness.cs", 101);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.ICity.RemoveAsync(Guid, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\CityBusiness.cs", 103);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\CityBusiness.cs", 107);
				ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\CityBusiness.cs", 108);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\CityBusiness.cs", 113), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\CityBusiness.cs", 113);
			}
		}
		return ret;
	}

	public static Task<IEnumerable<CityBusiness>> GetAllAsync(string contextGuid, string search, Guid stateGuid, CancellationToken token, bool? status = true, string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ICity.GetAllAsync(search, stateGuid, token, status, tranName);
	}

	public static Task UpdateAllCityChecked(string contextGuid, AdvertiseType type, short tryCount = 10, string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ICity.UpdateAllCityChecked(type, tryCount, tranName);
	}

	public static async Task<CityBusiness> GetNextRandomCityAsync(string contextGuid, Guid masterVisitorGuid, AdvertiseType type, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("GetNextRandomCityAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\CityBusiness.cs"))
		{
			_ = 1;
			try
			{
				CityBusiness city = null;
				while (city == null)
				{
					List<Guid> list = (await VisitorCityBusiness.GetAllAsync(contextGuid, masterVisitorGuid)).Select((VisitorCityBusiness p) => p.CityGuid)?.ToList();
					List<CityBusiness> lstCity = new List<CityBusiness>();
					int weight = 0;
					if (list != null && list.Count() > 0)
					{
						foreach (Guid item in list)
						{
							CityBusiness cityBusiness = await GetAsync(contextGuid, item, token);
							lstCity.Add(cityBusiness);
							cityBusiness.StartPoint = weight;
							weight = checked(weight + cityBusiness.Weight);
						}
					}
					int maxValue = lstCity.Sum((CityBusiness p) => p.Weight);
					int random = new Random().Next(0, maxValue);
					switch (type)
					{
					case AdvertiseType.Divar:
						city = lstCity.FirstOrDefault((CityBusiness q) => q.StartPoint <= random && q.EndPoint > random && q.isDivarCity);
						break;
					case AdvertiseType.Sheypoor:
						city = lstCity.FirstOrDefault((CityBusiness q) => q.StartPoint <= random && q.EndPoint > random && q.isSheypoorCity);
						break;
					case AdvertiseType.NiazKade:
						city = lstCity.FirstOrDefault((CityBusiness q) => q.StartPoint <= random && q.EndPoint > random && q.isNiazKadeCity);
						break;
					case AdvertiseType.NiazmandyHa:
						city = lstCity.FirstOrDefault((CityBusiness q) => q.StartPoint <= random && q.EndPoint > random && q.isNiazmandyHaCity);
						break;
					}
				}
				return city;
			}
			catch (OperationCanceledException)
			{
				return null;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "GetNextRandomCityAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\CityBusiness.cs", 170);
				throw;
			}
		}
	}

	public static Task<bool> CheckNameAsync(string contextGuid, string name, Guid guid, string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ICity.CheckNameAsync(name, guid, tranName);
	}
}
