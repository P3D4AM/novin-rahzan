using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Tolid.Base;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces.Accounting.Tolid.Base;
using PacketParser.EventArgs;

namespace EntityCache.Business.Accounting.Tolid;

public class TolidFormulaBusiness : BaseTolidBusiness
{
	private SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private OrderBusiness _listExport;

	private OrderBusiness _listMavad;

	private List<TolidFormulaHazineBusiness> _listHazine;

	public string Title { get; set; } = "";


	public string Description { get; set; } = "";


	public override IReadOnlyList<IBaseHazineBusiness> HazineList
	{
		get
		{
			if (_listHazine == null)
			{
				_listHazine = new List<TolidFormulaHazineBusiness>();
			}
			_listHazine.RemoveAll((TolidFormulaHazineBusiness p) => p == null);
			return _listHazine;
		}
	}

	public override async Task<OrderBusiness> GetExportListAsync(EnSystem system, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (_listExport != null)
			{
				return _listExport;
			}
			_listExport = await OrderBusiness.GenerateNewWithDefaultsAsync(base.ContextGuid, system, Document.Tolid_Mahsul, ENSource.Accounting, null, "", null, tranName, token);
			return _listExport;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetExportListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidFormulaBusiness.cs", 36);
			throw;
		}
	}

	public override async Task<OrderBusiness> GetMavadListAsync(EnSystem system, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (_listMavad != null)
			{
				return _listMavad;
			}
			_listMavad = await OrderBusiness.GenerateNewWithDefaultsAsync(base.ContextGuid, system, Document.Tolid_Mavad, ENSource.Accounting, null, "", null, tranName, token);
			return _listMavad;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetMavadListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidFormulaBusiness.cs", 50);
			throw;
		}
	}

	public TolidFormulaBusiness(string contextGuid)
		: base(contextGuid)
	{
	}

	public static Task<IEnumerable<TolidFormulaBusiness>> GetAllAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.TolidFormula.GetAllAsync(token, tranName);
	}

	public static Task<TolidFormulaBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.TolidFormula.GetAsync(guid, token, tranName);
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(base.ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidFormulaBusiness.cs", 78), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidFormulaBusiness.cs", 78);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(base.ContextGuid).CacheRepository.TolidFormula.RemoveAsync(base.Guid, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidFormulaBusiness.cs", 80);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidFormulaBusiness.cs", 84);
				ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidFormulaBusiness.cs", 85);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(base.ContextGuid, tranName, ret.HasError, disposeContext: true, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidFormulaBusiness.cs", 90), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidFormulaBusiness.cs", 90);
			}
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, CancellationToken token = default(CancellationToken), bool awaitOnSemaphore = true, short tryCount = 10, string tranName = "")
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
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "TolidFormulaBusiness/SaveAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidFormulaBusiness.cs", 103);
					}
					else
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(base.ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidFormulaBusiness.cs", 104), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidFormulaBusiness.cs", 104);
						}
						transactionHasBeenCreated = true;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(system, isDeleteAction: false, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidFormulaBusiness.cs", 106);
						if (!ret.HasError)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(base.ContextGuid).CacheRepository.TolidFormula.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidFormulaBusiness.cs", 108);
							goto end_IL_0122;
						}
						result = ret;
					}
					goto end_IL_007d;
					end_IL_0122:;
				}
				goto end_IL_006c;
				end_IL_007d:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidFormulaBusiness.cs", 112);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidFormulaBusiness.cs", 113);
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
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(base.ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidFormulaBusiness.cs", 118), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidFormulaBusiness.cs", 118);
			}
		}
		return ret;
	}

	public override async Task<ReturnedSaveFuncInfo> CheckValidationAsync(EnSystem system, bool isDeleteAction, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (system == EnSystem.Transfer)
			{
				return res;
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await base.CheckValidationAsync(system, isDeleteAction, token, tranName), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidFormulaBusiness.cs", 128);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidFormulaBusiness.cs", 132);
			res.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidFormulaBusiness.cs", 133);
		}
		return res;
	}

	public override async Task AddExportListAsync(EnSystem system, IEnumerable<OrderDetailBusiness> lstObjects, bool clearDets = false, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			if (lstObjects != null)
			{
				await (await GetExportListAsync(system, tranName, token)).AddDetListAsync(lstObjects.ToList(), clearDets, system, token);
			}
		}
		catch (ThreadAbortException)
		{
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "AddExportListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidFormulaBusiness.cs", 145);
		}
	}

	public override async Task AddMavadListAsync(EnSystem system, IEnumerable<OrderDetailBusiness> lstObjects, bool clearDets = false, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			if (lstObjects?.Any() ?? false)
			{
				await (await GetMavadListAsync(system, tranName, token)).AddDetListAsync(lstObjects.ToList(), clearDets, system, token);
			}
		}
		catch (ThreadAbortException)
		{
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "AddMavadListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidFormulaBusiness.cs", 157);
			throw;
		}
	}

	public override Task AddHazineListAsync(IEnumerable<IBaseHazineBusiness> lstObjects, EnSystem system, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (lstObjects == null || !lstObjects.Any())
			{
				return Task.CompletedTask;
			}
			foreach (IBaseHazineBusiness obj in lstObjects)
			{
				if (obj != null)
				{
					obj.MasterGuid = base.Guid;
					if (_listHazine == null)
					{
						_listHazine = new List<TolidFormulaHazineBusiness>();
					}
					TolidFormulaHazineBusiness tolidFormulaHazineBusiness = _listHazine.FirstOrDefault((TolidFormulaHazineBusiness q) => q.Guid == obj.Guid);
					if (tolidFormulaHazineBusiness != null)
					{
						_listHazine.Remove(tolidFormulaHazineBusiness);
					}
					_listHazine.Add((TolidFormulaHazineBusiness)obj);
					if (system == EnSystem.UserInterface)
					{
						RaiseEventAsync(token);
					}
				}
			}
		}
		catch (ThreadAbortException)
		{
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "AddHazineListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidFormulaBusiness.cs", 180);
			throw;
		}
		return Task.CompletedTask;
	}

	public override async Task RemoveFromListMavadAsync(EnSystem system, Guid guid, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("RemoveFromListMavadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidFormulaBusiness.cs"))
		{
			_ = 1;
			try
			{
				if (_listMavad == null)
				{
					_listMavad = await OrderBusiness.GenerateNewWithDefaultsAsync(base.ContextGuid, system, Document.Tolid_Mavad, ENSource.Accounting, null, "", null, "", token);
				}
				OrderDetailBusiness orderDetailBusiness = _listMavad.ListKala.FirstOrDefault((OrderDetailBusiness p) => p.Guid == guid);
				if (orderDetailBusiness != null)
				{
					orderDetailBusiness.Dispose();
					await RaiseEventAsync(token);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveFromListMavadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidFormulaBusiness.cs", 198);
				throw;
			}
		}
	}

	public override async Task RemoveFromListHazineAsync(Guid guid, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("RemoveFromListHazineAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidFormulaBusiness.cs"))
		{
			try
			{
				if (_listHazine == null)
				{
					_listHazine = new List<TolidFormulaHazineBusiness>();
				}
				TolidFormulaHazineBusiness tolidFormulaHazineBusiness = _listHazine.FirstOrDefault((TolidFormulaHazineBusiness p) => p.Guid == guid);
				if (tolidFormulaHazineBusiness != null)
				{
					_listHazine.Remove(tolidFormulaHazineBusiness);
					await RaiseEventAsync(token);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveFromListHazineAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidFormulaBusiness.cs", 215);
				throw;
			}
		}
	}

	public void ClearExportList()
	{
		try
		{
			foreach (OrderDetailBusiness item in _listExport?.ListKala?.ToList())
			{
				item.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ClearExportList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidFormulaBusiness.cs", 228);
			throw;
		}
	}

	private Task _listExport_OnSumChanged(object arg1, EventArgs arg2)
	{
		return RaiseEventAsync();
	}

	public static Task<List<TolidFormulaBusiness>> GetAllByExportPrdGuidAsync(string contextGuid, Guid prdGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.TolidFormula.GetAllByProductAsync(prdGuid, token, tranName);
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(EnSystem system, CancellationToken token, IEnumerable<TolidFormulaBusiness> lst, Action<PercenteCompletedEventArgs> percentCompleted = null)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		CancellationTokenSource invokeToken2 = new CancellationTokenSource();
		try
		{
			if (!(lst?.Any() ?? false))
			{
				return ret;
			}
			int current = 0;
			foreach (TolidFormulaBusiness item in lst)
			{
				current = checked(current + 1);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidFormulaBusiness.cs", 246);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(system, token, awaitOnSemaphore: true, 10), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidFormulaBusiness.cs", 247);
				if (percentCompleted != null)
				{
					invokeToken2?.Cancel();
					invokeToken2 = new CancellationTokenSource(60000);
					CancellationToken token2 = invokeToken2.Token;
					percentCompleted(new PercenteCompletedEventArgs(lst.Count(), current, item.Title, token2));
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidFormulaBusiness.cs", 259);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidFormulaBusiness.cs", 260);
		}
		finally
		{
			if (percentCompleted != null)
			{
				invokeToken2?.Cancel();
				invokeToken2 = new CancellationTokenSource(60000);
				CancellationToken invokeTkn = invokeToken2.Token;
				Task.Run(delegate
				{
					percentCompleted(new PercenteCompletedEventArgs(lst?.Count() ?? 0, lst?.Count() ?? 0, "", invokeTkn));
				});
			}
		}
		return ret;
	}
}
