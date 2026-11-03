using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace EntityCache.Business;

public class CheckedSaveBusiness : Serializable<CheckedSaveBusiness>, ICheckedSave, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now;


	public bool Status { get; set; } = true;


	public Guid EntityGuid { get; set; }

	public Guid DetailGuid { get; set; }

	public bool IsDebit { get; set; }

	public checkedTablReportType ReportType { get; set; }

	public DateTime DateM { get; set; } = DateTime.Now;


	public string ContextGuid { get; set; }

	public CheckedSaveBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static async Task<ReturnedSaveFuncInfo> DeleteAsync(string contextGuid, checkedTablReportType reportType, Guid TafsilGuid, DateTime d1, DateTime d2, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\CheckedSaveBusiness.cs", 38), ReturnedState.None, "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\CheckedSaveBusiness.cs", 38);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.CheckedSave.DeleteAsync(reportType, TafsilGuid, d1, d2, token, tranName), ReturnedState.None, "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\CheckedSaveBusiness.cs", 40);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\CheckedSaveBusiness.cs", 44);
				ret.AddReturnedValue(ex, "", "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\CheckedSaveBusiness.cs", 45);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\CheckedSaveBusiness.cs", 50), ReturnedState.None, "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\CheckedSaveBusiness.cs", 50);
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> DeleteAsync(string contextGuid, checkedTablReportType reportType, Guid TafsilGuid, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\CheckedSaveBusiness.cs", 62), ReturnedState.None, "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\CheckedSaveBusiness.cs", 62);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.CheckedSave.DeleteAsync(reportType, TafsilGuid, token, tranName), ReturnedState.None, "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\CheckedSaveBusiness.cs", 64);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\CheckedSaveBusiness.cs", 68);
				ret.AddReturnedValue(ex, "", "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\CheckedSaveBusiness.cs", 69);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\CheckedSaveBusiness.cs", 74), ReturnedState.None, "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\CheckedSaveBusiness.cs", 74);
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> DeleteAsync(string contextGuid, checkedTablReportType reportType, Guid tafsilGuid, Guid detailGuid, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transacrionHasBeenCreated = false;
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\CheckedSaveBusiness.cs", 86), ReturnedState.None, "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\CheckedSaveBusiness.cs", 86);
				}
				transacrionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.CheckedSave.DeleteAsync(reportType, tafsilGuid, detailGuid, tryCount, token, tranName), ReturnedState.None, "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\CheckedSaveBusiness.cs", 88);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\CheckedSaveBusiness.cs", 92);
				ret.AddReturnedValue(ex, "", "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\CheckedSaveBusiness.cs", 93);
			}
		}
		finally
		{
			if (autoTran && transacrionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\CheckedSaveBusiness.cs", 98), ReturnedState.None, "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\CheckedSaveBusiness.cs", 98);
			}
		}
		return ret;
	}

	public static Task<IEnumerable<CheckedSaveBusiness>> GetAllAsync(string contextGuid, bool? status, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.CheckedSave.GetAllAsync(status, token, tranName);
	}

	public static Task<IEnumerable<CheckedSaveBusiness>> GetAllAsync(string contextGuid, checkedTablReportType reportType, Guid hesabGuid, bool? status = true, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.CheckedSave.GetAllAsync(reportType, hesabGuid, status, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(CancellationToken token = default(CancellationToken), bool awaitOnSemaphore = true, short tryCount = 10, string tranName = "")
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
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "CheckedSavebusiness/SaveAsync", 60000, token))
				{
					if (!dis.Token.IsCancellationRequested)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\CheckedSaveBusiness.cs", 117), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\CheckedSaveBusiness.cs", 117);
						}
						transactionHasBeenCreated = true;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.CheckedSave.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\CheckedSaveBusiness.cs", 119);
						goto end_IL_011b;
					}
					result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\CheckedSaveBusiness.cs", 116);
					goto end_IL_007d;
					end_IL_011b:;
				}
				goto end_IL_006c;
				end_IL_007d:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\CheckedSaveBusiness.cs", 124);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\CheckedSaveBusiness.cs", 125);
				goto end_IL_006c;
			}
			return result;
			end_IL_006c:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\CheckedSaveBusiness.cs", 130), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\CheckedSaveBusiness.cs", 130);
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(string contextGuid, List<CheckedSaveBusiness> items, checkedTablReportType type, Guid entityGuid, CancellationToken token = default(CancellationToken), bool awaitOnSemaphore = true, short tryCount = 10, string tranName = "")
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
			_ = 3;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "CheckedSavebusiness/SaveAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\CheckedSaveBusiness.cs", 143);
					}
					else
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\CheckedSaveBusiness.cs", 144), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\CheckedSaveBusiness.cs", 144);
						}
						transactionHasBeenCreated = true;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await DeleteAsync(contextGuid, type, entityGuid, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\CheckedSaveBusiness.cs", 146);
						if (ret.HasError)
						{
							result = ret;
						}
						else
						{
							if (items != null && items.Count > 0)
							{
								foreach (CheckedSaveBusiness item in items)
								{
									returnedSaveFuncInfo = ret;
									returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(token, awaitOnSemaphore: false, tryCount, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\CheckedSaveBusiness.cs", 151);
									if (!ret.HasError)
									{
										continue;
									}
									result = ret;
									goto end_IL_0076;
								}
								goto end_IL_0118;
							}
							result = ret;
						}
					}
					goto end_IL_0076;
					end_IL_0118:;
				}
				goto end_IL_0065;
				end_IL_0076:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\CheckedSaveBusiness.cs", 157);
				ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\CheckedSaveBusiness.cs", 158);
				goto end_IL_0065;
			}
			return result;
			end_IL_0065:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\CheckedSaveBusiness.cs", 163), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\CheckedSaveBusiness.cs", 163);
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(string contextGuid, List<CheckedSaveBusiness> items, CancellationToken token = default(CancellationToken), bool awaitOnSemaphore = true, short tryCount = 10, string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (items == null || items.Count <= 0)
			{
				return ret;
			}
			foreach (CheckedSaveBusiness item in items)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(token, awaitOnSemaphore: false, tryCount, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\CheckedSaveBusiness.cs", 175);
				if (ret.HasError)
				{
					return ret;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\CheckedSaveBusiness.cs", 181);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\CheckedSaveBusiness.cs", 182);
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(string contextGuid, IEnumerable<GardeshHesabBusiness> items, checkedTablReportType type, Guid? tafsilGuid, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (!(items?.Any() ?? false))
			{
				return res;
			}
			foreach (GardeshHesabBusiness item in items)
			{
				if (item.AsGuid.HasValue)
				{
					Guid value = item.AsGuid.Value;
					if (item.Ischecked)
					{
						CheckedSaveBusiness obj = new CheckedSaveBusiness(contextGuid)
						{
							ContextGuid = contextGuid,
							Guid = Guid.NewGuid(),
							Status = true,
							DateM = DateTime.Now,
							Modified = DateTime.Now,
							ReportType = type,
							EntityGuid = (tafsilGuid ?? Guid.Empty),
							DetailGuid = value,
							IsDebit = (item.Debit > 0m)
						};
						ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await obj.SaveAsync(token, awaitOnSemaphore: true, 10), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\CheckedSaveBusiness.cs", 210);
					}
					else
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await DeleteAsync(contextGuid, type, tafsilGuid ?? Guid.Empty, value, 1, token), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\CheckedSaveBusiness.cs", 212);
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\CheckedSaveBusiness.cs", 217);
			res.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\CheckedSaveBusiness.cs", 218);
		}
		return res;
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(string contextGuid, IEnumerable<GardeshTafsilBusiness> items, checkedTablReportType type, Guid? tafsilGuid, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (!(items?.Any() ?? false))
			{
				return res;
			}
			foreach (GardeshTafsilBusiness item in items)
			{
				Guid detailGuid = item.DetailGuid;
				if (item.IsChecked)
				{
					CheckedSaveBusiness obj = new CheckedSaveBusiness(contextGuid)
					{
						ContextGuid = contextGuid,
						Guid = Guid.NewGuid(),
						Status = true,
						DateM = DateTime.Now,
						Modified = DateTime.Now,
						ReportType = type,
						EntityGuid = (tafsilGuid ?? Guid.Empty),
						DetailGuid = detailGuid,
						IsDebit = (item.Debit > 0m)
					};
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await obj.SaveAsync(token, awaitOnSemaphore: true, 10), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\CheckedSaveBusiness.cs", 245);
				}
				else
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await DeleteAsync(contextGuid, type, tafsilGuid ?? Guid.Empty, detailGuid, 1, token), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\CheckedSaveBusiness.cs", 247);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\CheckedSaveBusiness.cs", 252);
			res.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\CheckedSaveBusiness.cs", 253);
		}
		return res;
	}
}
