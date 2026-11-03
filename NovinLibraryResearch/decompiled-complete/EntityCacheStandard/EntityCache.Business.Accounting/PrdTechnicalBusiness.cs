using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models.Product;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Settings.Accounting;

namespace EntityCache.Business.Accounting;

public class PrdTechnicalBusiness : Serializable<PrdTechnicalBusiness>, IPrdTechnical, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	public Guid Guid { get; set; } = Guid.NewGuid();


	public DateTime Modified { get; set; } = DateTime.Now;


	public bool Status { get; set; } = true;


	public Guid ProductGuid { get; set; }

	public string Code { get; set; } = "";


	public string Name { get; set; }

	public string Key { get; set; }

	public short TechnicalType { get; set; }

	public string ContextGuid { get; }

	public PrdTechnicalBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(CancellationToken token, List<PrdTechnicalBusiness> Lst, short tryCount = 10, string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (!(Lst?.Any() ?? false))
			{
				return ret;
			}
			foreach (PrdTechnicalBusiness item in Lst)
			{
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdTechnicalBusiness.cs", 38);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(token, awaitOnSemaphore: true, tryCount, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdTechnicalBusiness.cs", 39);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdTechnicalBusiness.cs", 44);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdTechnicalBusiness.cs", 45);
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> RemoveRangeAsync(string contextGuid, Guid productGuid, CancellationToken token, string tranName)
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		bool transactionHasBeenCreated = false;
		try
		{
			_ = 1;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdTechnicalBusiness.cs", 57), ReturnedState.None, "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdTechnicalBusiness.cs", 57);
				}
				transactionHasBeenCreated = true;
				if (res.HasError)
				{
					return res;
				}
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IPrdTechnical.RemoveByProductGuidAsync(productGuid, token, tranName), ReturnedState.None, "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdTechnicalBusiness.cs", 60);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdTechnicalBusiness.cs", 64);
				res.AddReturnedValue(ex, "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdTechnicalBusiness.cs", 65);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdTechnicalBusiness.cs", 70), ReturnedState.None, "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdTechnicalBusiness.cs", 70);
			}
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(CancellationToken token = default(CancellationToken), bool awaitOnSemaphore = true, short tryCount = 10, string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool autoCommit = string.IsNullOrEmpty(tranName);
		if (autoCommit)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		try
		{
			_ = 2;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "PrdTechnicalBusiness/SaveAsync", 60000, token))
				{
					if (!dis.Token.IsCancellationRequested)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoCommit)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdTechnicalBusiness.cs", 84), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdTechnicalBusiness.cs", 84);
						}
						transactionHasBeenCreated = true;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IPrdTechnical.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdTechnicalBusiness.cs", 86);
						goto end_IL_011b;
					}
					result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdTechnicalBusiness.cs", 83);
					goto end_IL_007d;
					end_IL_011b:;
				}
				goto end_IL_006c;
				end_IL_007d:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdTechnicalBusiness.cs", 90);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdTechnicalBusiness.cs", 91);
				goto end_IL_006c;
			}
			return result;
			end_IL_006c:;
		}
		finally
		{
			if (autoCommit && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdTechnicalBusiness.cs", 96), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdTechnicalBusiness.cs", 96);
			}
		}
		return ret;
	}

	public static Task<IEnumerable<PrdTechnicalBusiness>> GetAllAsync(string contextGuid, short technicalType, bool? status = true, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IPrdTechnical.GetAllAsync(technicalType, status, token, tranName);
	}

	public static Task<IEnumerable<PrdTechnicalBusiness>> GetAllAsync(string contextGuid, Guid prdGuid, bool? status, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IPrdTechnical.GetAllAsync(prdGuid, status, token, tranName);
	}

	public static async Task<PrdTechnicalBusiness> GetAsync(string contextGuid, Guid guid, short technicalType, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return await ClsCache.GetCache(contextGuid).CacheRepository.IPrdTechnical.GetAsync(guid, technicalType, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(string contextGuid, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdTechnicalBusiness.cs", 114), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdTechnicalBusiness.cs", 114);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IPrdTechnical.RemoveAsync(Guid, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdTechnicalBusiness.cs", 116);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdTechnicalBusiness.cs", 120);
				ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdTechnicalBusiness.cs", 121);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdTechnicalBusiness.cs", 126), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdTechnicalBusiness.cs", 126);
			}
		}
		return ret;
	}

	public static async Task<List<ProductTechnicalInfoUpdateModel>> GetAllDefaultTechnicalInfoAsync(string contextGuid, CancellationToken token, string tranName = "")
	{
		try
		{
			List<ProductTechnicalInfoUpdateModel> list = new List<ProductTechnicalInfoUpdateModel>();
			AccGlobalSettings accGlobalSettings = await LocalSettingBusiness.GetSettingAsync(contextGuid, token, tranName);
			list.Add(new ProductTechnicalInfoUpdateModel
			{
				Ordinal = 0,
				TechnicalInfoTitle = "<... عنوان مشخصه فنی ...>".GetTranslate()
			});
			list.Add(new ProductTechnicalInfoUpdateModel
			{
				Ordinal = 1,
				TechnicalInfoTitle = accGlobalSettings.Products.TechnicalInfo1Name
			});
			list.Add(new ProductTechnicalInfoUpdateModel
			{
				Ordinal = 2,
				TechnicalInfoTitle = accGlobalSettings.Products.TechnicalInfo2Name
			});
			list.Add(new ProductTechnicalInfoUpdateModel
			{
				Ordinal = 3,
				TechnicalInfoTitle = accGlobalSettings.Products.TechnicalInfo3Name
			});
			list.Add(new ProductTechnicalInfoUpdateModel
			{
				Ordinal = 4,
				TechnicalInfoTitle = accGlobalSettings.Products.TechnicalInfo4Name
			});
			list.Add(new ProductTechnicalInfoUpdateModel
			{
				Ordinal = 5,
				TechnicalInfoTitle = accGlobalSettings.Products.TechnicalInfo5Name
			});
			list.Add(new ProductTechnicalInfoUpdateModel
			{
				Ordinal = 6,
				TechnicalInfoTitle = accGlobalSettings.Products.TechnicalInfo6Name
			});
			list.Add(new ProductTechnicalInfoUpdateModel
			{
				Ordinal = 7,
				TechnicalInfoTitle = accGlobalSettings.Products.TechnicalInfo7Name
			});
			list.Add(new ProductTechnicalInfoUpdateModel
			{
				Ordinal = 8,
				TechnicalInfoTitle = accGlobalSettings.Products.TechnicalInfo8Name
			});
			return list;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllDefaultTechnicalInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdTechnicalBusiness.cs", 187);
			throw;
		}
	}
}
