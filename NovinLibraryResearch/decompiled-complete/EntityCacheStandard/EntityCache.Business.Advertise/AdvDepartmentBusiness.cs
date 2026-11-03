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
using PacketParser.Settings.Advertise;

namespace EntityCache.Business.Advertise;

public class AdvDepartmentBusiness : Serializable<AdvDepartmentBusiness>, IAdvDepartment, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	private string _chatData;

	private string _chatCityData;

	private AdvDepartmentChatList _chatInfo;

	private AdvDepartmentChatCity _chatCityInfo;

	private List<VisitorBusiness> _visitorList;

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now;


	public string Title { get; set; }

	public AdvDepartmentChatList ChatList
	{
		get
		{
			if (string.IsNullOrEmpty(ChatData))
			{
				return null;
			}
			return _chatInfo ?? (_chatInfo = ChatData.FromJson<AdvDepartmentChatList>("ChatList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvDepartmentBusiness.cs", 27));
		}
		set
		{
		}
	}

	public string ChatData
	{
		get
		{
			return _chatData;
		}
		set
		{
			_chatData = value.FixString();
		}
	}

	public bool IsSendChat { get; set; }

	public bool Status { get; set; } = true;


	public string Description { get; set; }

	public int ChatCount { get; set; }

	public AdvDepartmentChatCity ChatCity
	{
		get
		{
			if (string.IsNullOrEmpty(ChatData))
			{
				return null;
			}
			return _chatCityInfo ?? (_chatCityInfo = ChatCityData.FromJson<AdvDepartmentChatCity>("ChatCity", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvDepartmentBusiness.cs", 47));
		}
		set
		{
		}
	}

	public string ChatCityData
	{
		get
		{
			return _chatCityData;
		}
		set
		{
			_chatCityData = value.FixString();
		}
	}

	public string ContextGuid { get; set; }

	public async Task<List<VisitorBusiness>> GetVisitorListAsync(string tranName)
	{
		if (_visitorList == null || _visitorList.Count <= 0)
		{
			_visitorList = await VisitorBusiness.GetAllAsync(ContextGuid, Guid, tranName);
		}
		return _visitorList;
	}

	public void SetVisitList(List<VisitorBusiness> value)
	{
		_visitorList = value;
	}

	public AdvDepartmentBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static Task<IEnumerable<AdvDepartmentBusiness>> GetAllAsync(string contextGuid, bool? status = true, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IAdvDepartment.GetAllAsync(status, token, tranName);
	}

	public static async Task<AdvDepartmentBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return await ClsCache.GetCache(contextGuid).CacheRepository.IAdvDepartment.GetAsync(guid, token, tranName);
	}

	public static async Task<List<AdvDepartmentBusiness>> GetAllAsync(string contextGuid, string search, CancellationToken token, bool? status = true)
	{
		try
		{
			if (string.IsNullOrEmpty(search))
			{
				search = "";
			}
			IEnumerable<AdvDepartmentBusiness> enumerable = await GetAllAsync(contextGuid, status, token);
			List<string> list = search.SplitString();
			if (list != null && list.Count > 0)
			{
				foreach (string item in list)
				{
					if (!string.IsNullOrEmpty(item) && item.Trim() != "")
					{
						enumerable = enumerable.Where((AdvDepartmentBusiness x) => x.Title.Contains(item));
					}
				}
			}
			enumerable = enumerable?.OrderBy((AdvDepartmentBusiness o) => o.Title)?.ToList();
			return token.IsCancellationRequested ? null : enumerable.ToList();
		}
		catch (OperationCanceledException)
		{
			return null;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvDepartmentBusiness.cs", 99);
			throw;
		}
	}

	public static Task<bool> CheckNameAsync(string contextGuid, string name, Guid guid, string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IAdvDepartment.CheckNameAsync(name, guid, tranName);
	}

	public static async Task<ReturnedSaveFuncInfo> ChangeStatusAsync(string contextGuid, EnSystem system, Guid guid, bool status, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
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
			_ = 2;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AdvDatabaseManager.BeginTransactionAsync(tranName, 60000, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvDepartmentBusiness.cs", 115), ReturnedState.None, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvDepartmentBusiness.cs", 115);
				}
				transactionHasBeenCreated = true;
				AdvDepartmentBusiness advDepartmentBusiness = await GetAsync(contextGuid, guid, token);
				if (advDepartmentBusiness == null)
				{
					ret.AddError(system, "Object is Null", "obj", "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvDepartmentBusiness.cs", 120);
					return ret;
				}
				advDepartmentBusiness.Status = status;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IAdvDepartment.SaveAsync(advDepartmentBusiness, tryCount, token, tranName), ReturnedState.None, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvDepartmentBusiness.cs", 125);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvDepartmentBusiness.cs", 129);
				ret.AddReturnedValue(ex, "", "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvDepartmentBusiness.cs", 130);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvDepartmentBusiness.cs", 135), ReturnedState.None, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvDepartmentBusiness.cs", 135);
			}
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(List<DepartmentCategoryBusiness> categories, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AdvDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvDepartmentBusiness.cs", 148), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvDepartmentBusiness.cs", 148);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await DepartmentCategoryBusiness.SaveRangeAsync(ContextGuid, categories, Guid, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvDepartmentBusiness.cs", 150);
				if (ret.HasError)
				{
					result = ret;
					goto IL_057b;
				}
				returnedSaveFuncInfo = ret;
				string contextGuid = ContextGuid;
				returnedSaveFuncInfo.AddReturnedValue(await SaveRangeAsync(contextGuid, await GetVisitorListAsync(tranName), awit4Send: false, 10, token), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvDepartmentBusiness.cs", 154);
				if (ret.HasError)
				{
					result = ret;
					goto IL_057b;
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IAdvDepartment.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvDepartmentBusiness.cs", 156);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvDepartmentBusiness.cs", 160);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvDepartmentBusiness.cs", 161);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvDepartmentBusiness.cs", 166), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvDepartmentBusiness.cs", 166);
			}
		}
		return ret;
		IL_057b:
		return result;
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(string contextGuid, List<VisitorBusiness> Lst, bool awit4Send = false, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (Lst == null || !Lst.Any())
			{
				return ret;
			}
			foreach (VisitorBusiness item in Lst)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(alaki: true, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvDepartmentBusiness.cs", 177);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvDepartmentBusiness.cs", 181);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\AdvDepartmentBusiness.cs", 182);
		}
		return ret;
	}

	public static Task<AdvDepartmentBusiness> GetAsync(string contextGuid, string depName, string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IAdvDepartment.GetAsync(depName, tranName);
	}
}
