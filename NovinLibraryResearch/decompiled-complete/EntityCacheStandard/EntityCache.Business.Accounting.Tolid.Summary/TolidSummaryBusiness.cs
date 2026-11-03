using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Tolid.Summary;
using PacketParser.Filters;

namespace EntityCache.Business.Accounting.Tolid.Summary;

public class TolidSummaryBusiness : ITolidSummary, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	private List<TolidSummaryDetailBusiness> _listTolidDet;

	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now;


	public int Id2 { get; set; }

	public string Description { get; set; } = "";


	public Guid CashierGuid { get; set; }

	public DateTime DateM { get; set; } = DateTime.Now;


	public string DateSh => DateM.MiladiToCalture();

	public bool Status { get; set; } = true;


	public string ContextGuid { get; set; }

	public IReadOnlyList<TolidSummaryDetailBusiness> ListTolidDet
	{
		get
		{
			_listTolidDet = _listTolidDet?.Where((TolidSummaryDetailBusiness p) => p != null)?.ToList() ?? new List<TolidSummaryDetailBusiness>();
			return _listTolidDet;
		}
	}

	public TolidSummaryBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public Task<int> NextNumberAsync(CancellationToken token, string tranName)
	{
		return ClsCache.GetCache(ContextGuid).CacheRepository.TolidSummary.NextNumberAsync(token, tranName);
	}

	public static Task<IEnumerable<TolidSummaryReportBusiness>> GetAllAsync(string contextGuid, TolidSummaryFilter filter, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.TolidSummary.GetAllAsync(filter, token, tranName);
	}

	public static Task<TolidSummaryBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.TolidSummary.GetAsync(guid, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(EnSystem system, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool autoCommit = string.IsNullOrEmpty(tranName);
		bool transactionHasBeenCreated = false;
		if (autoCommit)
		{
			tranName = Guid.NewGuid().ToString();
		}
		try
		{
			_ = 3;
			ReturnedSaveFuncInfo result;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoCommit)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Summary\\TolidSummaryBusiness.cs", 54), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Summary\\TolidSummaryBusiness.cs", 54);
				}
				transactionHasBeenCreated = true;
				foreach (TolidSummaryDetailBusiness item in ListTolidDet)
				{
					if (!((item?.TolidGuid ?? Guid.Empty) != Guid.Empty))
					{
						continue;
					}
					TolidBusiness tolidBusiness = await TolidBusiness.GetAsync(ContextGuid, item.TolidGuid.Value, token, tranName);
					if (tolidBusiness != null)
					{
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await tolidBusiness.RemoveAsync(system, 10, token), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Summary\\TolidSummaryBusiness.cs", 62);
					}
					if (!ret.HasError)
					{
						continue;
					}
					result = ret;
					goto end_IL_0087;
				}
				if (ret.HasError)
				{
					result = ret;
					goto end_IL_0087;
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.TolidSummary.RemoveAsync(Guid, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Summary\\TolidSummaryBusiness.cs", 67);
				if (ret.HasError)
				{
					result = ret;
					goto end_IL_0087;
				}
				goto end_IL_006c;
				end_IL_0087:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Summary\\TolidSummaryBusiness.cs", 72);
				ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Summary\\TolidSummaryBusiness.cs", 73);
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
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Summary\\TolidSummaryBusiness.cs", 78), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Summary\\TolidSummaryBusiness.cs", 78);
			}
		}
		return ret;
	}

	public void AddRangeToDetList(List<TolidSummaryDetailBusiness> detList, bool clearList)
	{
		using (new JaegerMonitor("AddRangeToDetList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Summary\\TolidSummaryBusiness.cs"))
		{
			try
			{
				if (detList != null && detList.Count > 0)
				{
					if (clearList)
					{
						_listTolidDet?.Clear();
					}
					if (_listTolidDet == null)
					{
						_listTolidDet = new List<TolidSummaryDetailBusiness>();
					}
					_listTolidDet.AddRange(detList);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AddRangeToDetList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Summary\\TolidSummaryBusiness.cs", 95);
				throw;
			}
		}
	}

	public void AddToDetList(TolidSummaryDetailBusiness item)
	{
		using (new JaegerMonitor("AddToDetList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Summary\\TolidSummaryBusiness.cs"))
		{
			try
			{
				if (item != null)
				{
					if (_listTolidDet == null)
					{
						_listTolidDet = new List<TolidSummaryDetailBusiness>();
					}
					if (_listTolidDet.All((TolidSummaryDetailBusiness p) => p.Guid != item.Guid))
					{
						_listTolidDet.Add(item);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AddToDetList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Summary\\TolidSummaryBusiness.cs", 111);
				throw;
			}
		}
	}

	public void RemoveFromListDet(Guid detGuid)
	{
		using (new JaegerMonitor("RemoveFromListDet", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Summary\\TolidSummaryBusiness.cs"))
		{
			try
			{
				if (_listTolidDet == null)
				{
					_listTolidDet = new List<TolidSummaryDetailBusiness>();
				}
				_listTolidDet.FirstOrDefault((TolidSummaryDetailBusiness p) => p.Guid == detGuid)?.Dispose();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveFromListDet", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Summary\\TolidSummaryBusiness.cs", 126);
				throw;
			}
		}
	}

	public Task<List<TolidSummaryDetailBusiness>> GetAllDetListAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.TolidSummary.GetAllAsync(this, DateM, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> CheckValidationAsync()
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (!(ListTolidDet?.Any() ?? false))
			{
				res.AddError("هیچ داده ای جهت ثبت وجود ندارد", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Summary\\TolidSummaryBusiness.cs", 138);
			}
			if (string.IsNullOrEmpty(Description))
			{
				res.AddError("لطفا شرح را وارد نمایید", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Summary\\TolidSummaryBusiness.cs", 141);
			}
			foreach (TolidSummaryDetailBusiness item in ListTolidDet)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await item.CheckValidationAsync(), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Summary\\TolidSummaryBusiness.cs", 144);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Summary\\TolidSummaryBusiness.cs", 148);
			res.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Summary\\TolidSummaryBusiness.cs", 149);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, CancellationToken token = default(CancellationToken), bool awaitOnSemaphore = true, short tryCount = 10, string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		try
		{
			_ = 5;
			ReturnedSaveFuncInfo result;
			try
			{
				using SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "TolidSummaryBusiness/SaveAsync", 60000, token);
				if (dis.Token.IsCancellationRequested)
				{
					result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Summary\\TolidSummaryBusiness.cs", 162);
					goto end_IL_0129;
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 300000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Summary\\TolidSummaryBusiness.cs", 163), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Summary\\TolidSummaryBusiness.cs", 163);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Summary\\TolidSummaryBusiness.cs", 165);
				if (ret.HasError)
				{
					result = ret;
					goto end_IL_0129;
				}
				foreach (TolidSummaryDetailBusiness item in ListTolidDet)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await GenerateTolidAsync(item, DateM, system, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Summary\\TolidSummaryBusiness.cs", 170);
					if (!ret.HasError)
					{
						continue;
					}
					result = ret;
					goto end_IL_0129;
				}
				if (Id2 <= 0)
				{
					Id2 = await NextNumberAsync(token, tranName);
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.TolidSummary.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Summary\\TolidSummaryBusiness.cs", 175);
				goto end_IL_006c;
				end_IL_0129:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Summary\\TolidSummaryBusiness.cs", 179);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Summary\\TolidSummaryBusiness.cs", 180);
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
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Summary\\TolidSummaryBusiness.cs", 185), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Summary\\TolidSummaryBusiness.cs", 185);
			}
		}
		return ret;
	}

	private async Task<ReturnedSaveFuncInfo> GenerateTolidAsync(TolidSummaryDetailBusiness item, DateTime masterDate, EnSystem system, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			ProductBusiness productBusiness = await ProductBusiness.GetAsync(ContextGuid, item.ProductGuid, token);
			if ((productBusiness?.TolidFormulaGuid ?? Guid.Empty) == Guid.Empty)
			{
				return res;
			}
			TolidFormulaBusiness tolidFormulaBusiness = await TolidFormulaBusiness.GetAsync(ContextGuid, productBusiness.TolidFormulaGuid.Value, token, tranName);
			if (tolidFormulaBusiness == null)
			{
				return res;
			}
			TolidBusiness tolid = await TolidBusiness.GenerateToliBusiness(ContextGuid, system, tolidFormulaBusiness, item.ProducedAmount, checkMovazene: false, token, tranName, Guid.NewGuid(), Description, masterDate);
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await tolid.SetTolidBalanceAsync(item.ProducedAmount, system, token, tranName), ReturnedState.None, "GenerateTolidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Summary\\TolidSummaryBusiness.cs", 199);
			if (res.HasError)
			{
				return res;
			}
			decimal sumMavad = await tolid.GetMavadSumAsync(system, token, tranName);
			decimal num = await tolid.GetExportSumAsync(system, token, tranName);
			if (tolid.HazineSum + sumMavad != num)
			{
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await tolid.SetTolidAdjustAsync(system, token, tranName), ReturnedState.None, "GenerateTolidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Summary\\TolidSummaryBusiness.cs", 205);
			}
			returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await tolid.SaveAsync(system, token, awaitOnSemaphore: true, 10), ReturnedState.None, "GenerateTolidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Summary\\TolidSummaryBusiness.cs", 207);
			if (res.HasError)
			{
				return res;
			}
			if ((item?.TolidGuid ?? Guid.Empty) != Guid.Empty)
			{
				TolidBusiness tolidBusiness = await TolidBusiness.GetAsync(ContextGuid, item.TolidGuid.Value, token, tranName);
				if (tolidBusiness != null)
				{
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await tolidBusiness.RemoveAsync(system, 10, token), ReturnedState.None, "GenerateTolidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Summary\\TolidSummaryBusiness.cs", 213);
				}
				if (res.HasError)
				{
					return res;
				}
			}
			item.TolidGuid = tolid.Guid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateTolidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Summary\\TolidSummaryBusiness.cs", 220);
			res.AddReturnedValue(ex, "", "GenerateTolidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Summary\\TolidSummaryBusiness.cs", 221);
		}
		return res;
	}
}
