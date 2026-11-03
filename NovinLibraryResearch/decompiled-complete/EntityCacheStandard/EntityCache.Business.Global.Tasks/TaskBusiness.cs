using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Global.Tasks;
using PacketParser.Entities.MetaData;

namespace EntityCache.Business.Global.Tasks;

public class TaskBusiness : Serializable<TaskBusiness>, ITask, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	private List<TaskCashierRelationBusiness> _cashierlst;

	private bool _isDisposed;

	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	public Guid Guid { get; set; }

	public Guid CashierGuid { get; set; }

	public DateTime DateM { get; set; }

	public DateTime? StartDate { get; set; }

	public DateTime? EndDate { get; set; }

	public string Title { get; set; }

	public int StatusId { get; set; }

	public string Description { get; set; }

	public Guid? MasterGuid { get; set; }

	public int Id2 { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public TaskMetaData MetaData { get; set; }

	public string ContextGuid { get; set; }

	public EntityChangeSt ChangeStatus { get; set; }

	public IReadOnlyCollection<TaskCashierRelationBusiness> CashierList
	{
		get
		{
			if (_cashierlst == null)
			{
				_cashierlst = new List<TaskCashierRelationBusiness>();
			}
			_cashierlst.RemoveAll((TaskCashierRelationBusiness q) => (q == null) | q.IsDisposed);
			return _cashierlst;
		}
	}

	public TaskBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
		Guid = Guid.NewGuid();
		ChangeStatus = EntityChangeSt.New;
	}

	public Task AddDetailCashierRelationListAsync(TaskCashierRelationBusiness cashier)
	{
		using (new JaegerMonitor("AddDetailCashierRelationListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tasks\\Tasks\\TaskBusiness.cs"))
		{
			if (cashier == null)
			{
				return Task.CompletedTask;
			}
			try
			{
				if (_cashierlst == null)
				{
					_cashierlst = new List<TaskCashierRelationBusiness>();
				}
				cashier.TaskGuid = Guid;
				_cashierlst.Add(cashier);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AddDetailCashierRelationListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tasks\\Tasks\\TaskBusiness.cs", 64);
			}
			return Task.CompletedTask;
		}
	}

	public Task RemoveCashierRelationDetailListAsync(TaskCashierRelationBusiness taskCashier)
	{
		using (new JaegerMonitor("RemoveCashierRelationDetailListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tasks\\Tasks\\TaskBusiness.cs"))
		{
			if (taskCashier == null)
			{
				return Task.CompletedTask;
			}
			try
			{
				TaskCashierRelationBusiness taskCashierRelationBusiness = _cashierlst?.FirstOrDefault((TaskCashierRelationBusiness q) => q.CashierGuid == taskCashier.TaskGuid && q.TaskGuid == taskCashier.CashierGuid);
				if (taskCashierRelationBusiness == null)
				{
					return Task.CompletedTask;
				}
				_cashierlst.Remove(taskCashierRelationBusiness);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveCashierRelationDetailListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tasks\\Tasks\\TaskBusiness.cs", 77);
			}
			return Task.CompletedTask;
		}
	}

	public override void Dispose()
	{
		try
		{
			if (base.IsDisposed)
			{
				return;
			}
			_isDisposed = true;
			if (_cashierlst != null)
			{
				foreach (TaskCashierRelationBusiness item in _cashierlst)
				{
					item?.Dispose();
				}
			}
			base.Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tasks\\Tasks\\TaskBusiness.cs", 93);
		}
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, bool awaitOnSemaphore = true, string CustomerUserName = "", CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool autotran = string.IsNullOrEmpty(tranName);
		if (autotran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionBeenCreate = false;
		try
		{
			_ = 3;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil semaphore = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "TaskBusiness/SaveAsync", 6000, token))
				{
					if (semaphore.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tasks\\Tasks\\TaskBusiness.cs", 104);
					}
					else
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autotran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tasks\\Tasks\\TaskBusiness.cs", 105), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tasks\\Tasks\\TaskBusiness.cs", 105);
						}
						transactionBeenCreate = true;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tasks\\Tasks\\TaskBusiness.cs", 107);
						if (!ret.HasError)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ITasks.SaveAsync(this, 1, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tasks\\Tasks\\TaskBusiness.cs", 109);
							goto end_IL_0121;
						}
						result = ret;
					}
					goto end_IL_007d;
					end_IL_0121:;
				}
				goto end_IL_006c;
				end_IL_007d:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tasks\\Tasks\\TaskBusiness.cs", 113);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tasks\\Tasks\\TaskBusiness.cs", 114);
				goto end_IL_006c;
			}
			return result;
			end_IL_006c:;
		}
		finally
		{
			if (transactionBeenCreate || autotran)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tasks\\Tasks\\TaskBusiness.cs", 119), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tasks\\Tasks\\TaskBusiness.cs", 119);
			}
		}
		return ret;
	}

	public Task<ReturnedSaveFuncInfo> CheckValidationAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			if (Title == null)
			{
				returnedSaveFuncInfo.AddError("عنوان وظیفه را پر کنید.".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tasks\\Tasks\\TaskBusiness.cs", 128);
			}
			if (StatusId == 0)
			{
				returnedSaveFuncInfo.AddError("وضعیت وظیفه را مشخص کنید.".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tasks\\Tasks\\TaskBusiness.cs", 129);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tasks\\Tasks\\TaskBusiness.cs", 133);
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tasks\\Tasks\\TaskBusiness.cs", 134);
		}
		return Task.FromResult(returnedSaveFuncInfo);
	}

	public async Task<int> GetNextId2Async(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			return await ClsCache.GetCache(ContextGuid).CacheRepository.ITasks.GetNextId2Async(token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetNextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tasks\\Tasks\\TaskBusiness.cs", 146);
			throw;
		}
	}
}
