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

public class AdvGroupBusiness : IAdvGroup, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public string GroupName { get; set; }

	public bool Status { get; set; }

	public Guid ParentGuid { get; set; }

	public bool IsChecked { get; set; }

	public string ContextGuid { get; set; }

	public async Task<string> GetParentNameAsync()
	{
		if (ParentGuid == Guid.Empty)
		{
			return "";
		}
		AdvGroupBusiness advGroupBusiness = await GetByParentAsync(ContextGuid, ParentGuid);
		return (advGroupBusiness == null) ? "" : advGroupBusiness.GroupName;
	}

	public AdvGroupBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static Task<IEnumerable<AdvGroupBusiness>> GetAllAsync(string contextGuid, bool? status = true, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IAdvGroupRepository.GetAllAsync(status, token, tranName);
	}

	public static async Task<List<AdvGroupBusiness>> GetAllAsync(string contextGuid, Guid parentGuid, string tranName = "")
	{
		return await ClsCache.GetCache(contextGuid).CacheRepository.IAdvGroupRepository.GetAllAsync(parentGuid, tranName);
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AdvDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvGroupBusiness.cs", 51), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvGroupBusiness.cs", 51);
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IAdvGroupRepository.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvGroupBusiness.cs", 52);
				transactionHasBeenCreated = true;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvGroupBusiness.cs", 57);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvGroupBusiness.cs", 58);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvGroupBusiness.cs", 63), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvGroupBusiness.cs", 63);
			}
		}
		return ret;
	}

	public static async Task<AdvGroupBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return await ClsCache.GetCache(contextGuid).CacheRepository.IAdvGroupRepository.GetAsync(guid, token, tranName);
	}

	public static async Task<AdvGroupBusiness> GetByParentAsync(string contextGuid, Guid parentGuid, string tranName = "")
	{
		return await ClsCache.GetCache(contextGuid).CacheRepository.IAdvGroupRepository.GetAsyncByParent(parentGuid, tranName);
	}

	public static async Task<AdvGroupBusiness> GetAsync(string contextGuid, string name, string tranName = "")
	{
		return await ClsCache.GetCache(contextGuid).CacheRepository.IAdvGroupRepository.GetAsync(name, tranName);
	}

	public static Task<bool> CheckNameAsync(string contextGuid, string name, Guid guid, string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IAdvGroupRepository.CheckNameAsync(name, guid, tranName);
	}

	public static async Task<ReturnedSaveFuncInfo> ChangeStatusAsync(string contextGuid, EnSystem system, Guid guid, bool status, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
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
			_ = 2;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AdvDatabaseManager.BeginTransactionAsync(tranName, 60000, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvGroupBusiness.cs", 88), ReturnedState.None, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvGroupBusiness.cs", 88);
				}
				transactionHasBeenCreated = true;
				AdvGroupBusiness advGroupBusiness = await GetAsync(contextGuid, guid, token);
				if (advGroupBusiness == null)
				{
					ret.AddError(system, "Object is Null", "obj", "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvGroupBusiness.cs", 93);
					return ret;
				}
				advGroupBusiness.Status = status;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IAdvGroupRepository.SaveAsync(advGroupBusiness, tryCount, token, tranName), ReturnedState.None, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvGroupBusiness.cs", 98);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvGroupBusiness.cs", 102);
				ret.AddReturnedValue(ex, "", "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvGroupBusiness.cs", 103);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvGroupBusiness.cs", 108), ReturnedState.None, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvGroupBusiness.cs", 108);
			}
		}
		return ret;
	}

	public static async Task<List<AdvGroupBusiness>> GetAllAsync(string contextGuid, string search, CancellationToken token, bool? status = true)
	{
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvGroupBusiness.cs"))
		{
			_ = 1;
			try
			{
				if (string.IsNullOrEmpty(search))
				{
					search = "";
				}
				IEnumerable<AdvGroupBusiness> enumerable = await GetAllAsync(contextGuid, token: token, status: true);
				foreach (string searchString in search?.RemoveCharactersForLinkSearch()?.SplitString()?.Distinct() ?? new List<string>())
				{
					if (!string.IsNullOrEmpty(searchString))
					{
						enumerable = (await Task.WhenAll(enumerable.Select((AdvGroupBusiness item) => MacheSearchedWordAsync(searchString, item))))?.Where((AdvGroupBusiness p) => p != null);
					}
				}
				enumerable = enumerable?.OrderBy((AdvGroupBusiness o) => o.GroupName)?.ToList();
				return token.IsCancellationRequested ? null : enumerable.ToList();
			}
			catch (OperationCanceledException)
			{
				return null;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvGroupBusiness.cs", 131);
				return null;
			}
		}
	}

	private static async Task<AdvGroupBusiness> MacheSearchedWordAsync(string searchString, AdvGroupBusiness item)
	{
		try
		{
			bool flag = item.GroupName.Contains(searchString);
			if (!flag)
			{
				flag = (await item.GetParentNameAsync()).Contains(searchString);
			}
			if (flag)
			{
				return item;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MacheSearchedWordAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvGroupBusiness.cs", 145);
			return null;
		}
		return null;
	}
}
