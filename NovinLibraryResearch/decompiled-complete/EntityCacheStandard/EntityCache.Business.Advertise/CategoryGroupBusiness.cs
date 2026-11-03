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

public class CategoryGroupBusiness : IAdvCategoryGroup, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	private string _divarCategoryListData;

	private string _sheypoorCategoryListData;

	private List<DivarCategory> _divarCategoryListInfo;

	private List<SheypoorCategory> _sheypoorCategoryListInfo;

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now;


	public bool Status { get; set; } = true;


	public string Name { get; set; }

	public string DivarCategoryList
	{
		get
		{
			return _divarCategoryListData;
		}
		set
		{
			_divarCategoryListData = value.FixString();
		}
	}

	public string SheypoorCategoryList
	{
		get
		{
			return _sheypoorCategoryListData;
		}
		set
		{
			_sheypoorCategoryListData = value.FixString();
		}
	}

	public bool IsChecked { get; set; }

	public List<DivarCategory> DivarCategories
	{
		get
		{
			if (string.IsNullOrEmpty(DivarCategoryList))
			{
				return null;
			}
			if (_divarCategoryListInfo != null && _divarCategoryListInfo.Count > 0)
			{
				return _divarCategoryListInfo;
			}
			return _divarCategoryListInfo = DivarCategoryList.FromJson<List<DivarCategory>>("DivarCategories", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\CategoryGroupBusiness.cs", 45);
		}
		set
		{
			DivarCategoryList = Json.ToStringJson(value);
		}
	}

	public List<SheypoorCategory> SheypoorCategories
	{
		get
		{
			if (string.IsNullOrEmpty(SheypoorCategoryList))
			{
				return null;
			}
			if (_sheypoorCategoryListInfo != null && _sheypoorCategoryListInfo.Count > 0)
			{
				return _sheypoorCategoryListInfo;
			}
			return _sheypoorCategoryListInfo = SheypoorCategoryList.FromJson<List<SheypoorCategory>>("SheypoorCategories", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\CategoryGroupBusiness.cs", 55);
		}
		set
		{
			SheypoorCategoryList = Json.ToStringJson(value);
		}
	}

	public string ContextGuid { get; set; }

	public CategoryGroupBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static Task<IEnumerable<CategoryGroupBusiness>> GetAllAsync(string contextGuid, bool? status = true, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.CategoryGroupRepository.GetAllAsync(status, token, tranName);
	}

	public static Task<CategoryGroupBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.CategoryGroupRepository.GetAsync(guid, token, tranName);
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AdvDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\CategoryGroupBusiness.cs", 80), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\CategoryGroupBusiness.cs", 80);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.CategoryGroupRepository.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\CategoryGroupBusiness.cs", 82);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\CategoryGroupBusiness.cs", 88);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\CategoryGroupBusiness.cs", 89);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\CategoryGroupBusiness.cs", 94), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\CategoryGroupBusiness.cs", 94);
			}
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AdvDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\CategoryGroupBusiness.cs", 107), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\CategoryGroupBusiness.cs", 107);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.CategoryGroupRepository.RemoveAsync(Guid, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\CategoryGroupBusiness.cs", 109);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\CategoryGroupBusiness.cs", 115);
				ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\CategoryGroupBusiness.cs", 116);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\CategoryGroupBusiness.cs", 121), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\CategoryGroupBusiness.cs", 121);
			}
		}
		return ret;
	}

	public static async Task<List<CategoryGroupBusiness>> GetAllAsync(string contextGuid, string search, CancellationToken token, bool? status = true)
	{
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\CategoryGroupBusiness.cs"))
		{
			try
			{
				if (string.IsNullOrEmpty(search))
				{
					search = "";
				}
				IEnumerable<CategoryGroupBusiness> enumerable = await GetAllAsync(contextGuid, status, token);
				List<string> list = search.SplitString();
				if (list != null && list.Count > 0)
				{
					foreach (string item in list)
					{
						if (!string.IsNullOrEmpty(item) && item.Trim() != "")
						{
							enumerable = enumerable.Where((CategoryGroupBusiness x) => x.Name.Contains(item));
						}
					}
				}
				enumerable = enumerable?.OrderBy((CategoryGroupBusiness o) => o.Name)?.ToList();
				return token.IsCancellationRequested ? null : enumerable.ToList();
			}
			catch (OperationCanceledException)
			{
				return null;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\CategoryGroupBusiness.cs", 146);
				throw;
			}
		}
	}
}
