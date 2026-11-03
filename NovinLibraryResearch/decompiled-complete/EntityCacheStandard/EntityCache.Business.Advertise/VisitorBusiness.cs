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

public class VisitorBusiness : Serializable<VisitorBusiness>, IVisitor, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	private IEnumerable<VisitorAdvBusiness> _advList;

	private IEnumerable<VisitorCityBusiness> _cityList;

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public short MasterCount { get; set; }

	public short SlaveCount { get; set; }

	public bool IsAgent { get; set; }

	public string AdvReplacement { get; set; }

	public string FullName { get; set; }

	public bool Status { get; set; }

	public bool IsChecked { get; set; }

	public string Enable_Status { get; set; }

	public string Phone { get; set; }

	public Guid DepartmentGuid { get; set; }

	public string ContextGuid { get; set; }

	public async Task<IEnumerable<VisitorAdvBusiness>> GetAdvListAsync()
	{
		if (_advList?.Any() ?? false)
		{
			return _advList;
		}
		return await VisitorAdvBusiness.GetAllAsync(ContextGuid, Guid);
	}

	public void SetAdvList(IEnumerable<VisitorAdvBusiness> value)
	{
		_advList = value;
	}

	public async Task<IEnumerable<VisitorCityBusiness>> GetCityListAsync()
	{
		if (_cityList?.Any() ?? false)
		{
			return _cityList;
		}
		return await VisitorCityBusiness.GetAllAsync(ContextGuid, Guid);
	}

	public void SetCityList(IEnumerable<VisitorCityBusiness> value)
	{
		_cityList = value;
	}

	public VisitorBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(bool alaki, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool transactionHasBeenCreated = false;
		ReturnedSaveFuncInfo result;
		try
		{
			_ = 7;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AdvDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorBusiness.cs", 58), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorBusiness.cs", 58);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await VisitorCityBusiness.RemoveByVisitorGuidAsync(ContextGuid, Guid, tryCount, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorBusiness.cs", 61);
				if (ret.HasError)
				{
					result = ret;
					goto IL_0759;
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await VisitorAdvBusiness.RemoveByVisitorGuidAsync(ContextGuid, Guid, tryCount, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorBusiness.cs", 64);
				if (ret.HasError)
				{
					result = ret;
					goto IL_0759;
				}
				returnedSaveFuncInfo = ret;
				string contextGuid = ContextGuid;
				returnedSaveFuncInfo.AddReturnedValue(await VisitorCityBusiness.SaveRangeAsync(contextGuid, await GetCityListAsync(), tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorBusiness.cs", 67);
				if (ret.HasError)
				{
					result = ret;
					goto IL_0759;
				}
				returnedSaveFuncInfo = ret;
				contextGuid = ContextGuid;
				returnedSaveFuncInfo.AddReturnedValue(await VisitorAdvBusiness.SaveRangeAsync(contextGuid, await GetAdvListAsync(), tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorBusiness.cs", 70);
				if (ret.HasError)
				{
					result = ret;
					goto IL_0759;
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IVisitor.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorBusiness.cs", 73);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorBusiness.cs", 77);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorBusiness.cs", 78);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorBusiness.cs", 83), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorBusiness.cs", 83);
			}
		}
		return ret;
		IL_0759:
		return result;
	}

	public static Task<IEnumerable<VisitorBusiness>> GetAllAsync(string contextGuid, bool? status = true, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IVisitor.GetAllAsync(status, token, tranName);
	}

	public static Task<MasterSlaveAdvReplacementsModel> GetMasterSlaveAdvReplacementsAsync(string contextGuid, long simCardNumber, string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IVisitor.GetMasterSlaveAdvReplacementsAsync(simCardNumber, tranName);
	}

	public static Task<VisitorBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IVisitor.GetAsync(guid, token, tranName);
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
		ReturnedSaveFuncInfo result;
		try
		{
			_ = 4;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AdvDatabaseManager.BeginTransactionAsync(tranName, 60000, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorBusiness.cs", 108), ReturnedState.None, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorBusiness.cs", 108);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await VisitorCityBusiness.RemoveByVisitorGuidAsync(contextGuid, guid, tryCount, tranName), ReturnedState.None, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorBusiness.cs", 111);
				if (ret.HasError)
				{
					result = ret;
					goto IL_05a0;
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await VisitorAdvBusiness.RemoveByVisitorGuidAsync(contextGuid, guid, tryCount, tranName), ReturnedState.None, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorBusiness.cs", 114);
				if (ret.HasError)
				{
					result = ret;
					goto IL_05a0;
				}
				VisitorBusiness visitorBusiness = await GetAsync(contextGuid, guid, token);
				if (visitorBusiness == null)
				{
					ret.AddError(system, "Object is Null", "obj", "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorBusiness.cs", 120);
					result = ret;
					goto IL_05a0;
				}
				visitorBusiness.Status = status;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IVisitor.SaveAsync(visitorBusiness, tryCount, token, tranName), ReturnedState.None, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorBusiness.cs", 125);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorBusiness.cs", 129);
				ret.AddReturnedValue(ex, "", "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorBusiness.cs", 130);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorBusiness.cs", 135), ReturnedState.None, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorBusiness.cs", 135);
			}
		}
		return ret;
		IL_05a0:
		return result;
	}

	public static async Task<List<VisitorBusiness>> GetAllAsync(string contextGuid, string search, CancellationToken token, bool? status = true)
	{
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorBusiness.cs"))
		{
			try
			{
				if (string.IsNullOrEmpty(search))
				{
					search = "";
				}
				IEnumerable<VisitorBusiness> source = await GetAllAsync(contextGuid, status, token);
				List<string> list = search.SplitString();
				if (list != null && list.Count > 0)
				{
					foreach (string item in list)
					{
						if (!string.IsNullOrEmpty(item) && item.Trim() != "")
						{
							source = source.Where((VisitorBusiness x) => x.FullName.Contains(item) || x.AdvReplacement.Contains(item));
						}
					}
				}
				source = source.OrderBy((VisitorBusiness o) => o.FullName);
				return token.IsCancellationRequested ? null : source.ToList();
			}
			catch (OperationCanceledException)
			{
				return null;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorBusiness.cs", 160);
				throw;
			}
		}
	}

	public static async Task<ReturnedSaveFuncInfo> ChangeEnableStatusAsync(string contextGuid, Guid guid, string status, short tryCount = 10, string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (string.IsNullOrEmpty(tranName))
			{
				tranName = Guid.NewGuid().ToString();
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IVisitor.ChangeEnableStatusAsync(guid, status, tryCount, tranName), ReturnedState.None, "ChangeEnableStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorBusiness.cs", 172);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ChangeEnableStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorBusiness.cs", 176);
			ret.AddReturnedValue(ex, "", "ChangeEnableStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorBusiness.cs", 177);
		}
		return ret;
	}

	public static Task<int> DeleteCountAsync(string contextGuid, short tryCount = 10, string tranName = "")
	{
		if (string.IsNullOrEmpty(tranName))
		{
			tranName = Guid.NewGuid().ToString();
		}
		return ClsCache.GetCache(contextGuid).CacheRepository.IVisitor.DeleteCountAsync(tryCount, tranName);
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AdvDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorBusiness.cs", 197), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorBusiness.cs", 197);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IVisitor.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorBusiness.cs", 199);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorBusiness.cs", 203);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorBusiness.cs", 204);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorBusiness.cs", 209), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorBusiness.cs", 209);
			}
		}
		return ret;
	}

	public static async Task<List<VisitorBusiness>> GetAllAsync(string contextGuid, Guid depGuid, string tranName = "")
	{
		return await ClsCache.GetCache(contextGuid).CacheRepository.IVisitor.GetAllAsync(depGuid, tranName);
	}

	public static async Task<ReturnedSaveFuncInfo> RemoveAsync(string contextGuid, Guid visitorGuid, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		ReturnedSaveFuncInfo result;
		try
		{
			_ = 3;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AdvDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorBusiness.cs", 228), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorBusiness.cs", 228);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IVisitorCity.RemoveByVisitorGuidAsync(visitorGuid, tryCount, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorBusiness.cs", 230);
				if (ret.HasError)
				{
					result = ret;
					goto IL_0506;
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IVisitorAdv.RemoveByVisitorGuidAsync(visitorGuid, tryCount, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorBusiness.cs", 232);
				if (ret.HasError)
				{
					result = ret;
					goto IL_0506;
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IVisitor.RemoveAsync(visitorGuid, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorBusiness.cs", 234);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorBusiness.cs", 240);
				ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorBusiness.cs", 241);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorBusiness.cs", 246), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorBusiness.cs", 246);
			}
		}
		return ret;
		IL_0506:
		return result;
	}
}
