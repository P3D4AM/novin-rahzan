using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Business;

public class RasGiriDetBusiness : Serializable<RasGiriDetBusiness>, IRasGiriDet, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext, INotifyPropertyChanged
{
	private bool _isDisposed;

	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private DateTime enherafBaseDate;

	private DateTime enherafRasDate;

	public new bool IsDisposed { get; set; }

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; } = true;


	public Guid MasterGuid { get; set; }

	public DateTime DateM { get; set; }

	public DateTime EnherafBaseDate
	{
		get
		{
			return enherafBaseDate;
		}
		internal set
		{
			enherafBaseDate = value;
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Enheraf"));
		}
	}

	public DateTime EnherafRasDate
	{
		get
		{
			return enherafRasDate;
		}
		internal set
		{
			enherafRasDate = value;
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("EnherafAsRas"));
		}
	}

	public string DateSh => DateM.MiladiToCalture();

	public decimal Price { get; set; }

	public decimal Enheraf => checked((int)DateM.Subtract(EnherafBaseDate).TotalDays);

	public decimal EnherafAsRas => checked((int)DateM.Subtract(EnherafRasDate).TotalDays);

	public decimal EnherafForCalculation { get; internal set; }

	public decimal EnherafForCalculationRas { get; internal set; }

	public string Description { get; set; }

	public string ContextGuid { get; set; }

	public event PropertyChangedEventHandler PropertyChanged;

	public event Func<object, DateTime, Task> OnBaseDateChanged;

	public event Func<object, DateTime, Task> OnDateChanged;

	public event Func<object, decimal, Task> OnPriceChanged;

	public event Func<object, EventArgs, Task> OnDisposed;

	public RasGiriDetBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public RasGiriDetBusiness(string contextGuid, DateTime enherafBaseDate)
	{
		ContextGuid = contextGuid;
		EnherafBaseDate = enherafBaseDate;
	}

	public static Task<RasGiriDetBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IRasGiriDet.GetAsync(guid, token, tranName);
	}

	public static Task<IEnumerable<RasGiriDetBusiness>> GetAllByMasterGuidAsync(string contextGuid, Guid masterGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IRasGiriDet.GetAllByMasterGuidAsync(masterGuid, token, tranName);
	}

	public static Task<IEnumerable<RasGiriDetBusiness>> GetAllAsync(string contextGuid, bool? status = true, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IRasGiriDet.GetAllAsync(status, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(CancellationToken token, bool awaitOnSemaphore = true, short tryCount = 10, string tranName = "")
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
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "RasGiriDetBusiness/SaveAsync", 60000, token))
				{
					if (!dis.Token.IsCancellationRequested)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\RasGiri\\RasGiriDetBusiness.cs", 96), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\RasGiri\\RasGiriDetBusiness.cs", 96);
						}
						transactionHasBeenCreated = true;
						Modified = DateTime.Now;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IRasGiriDet.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\RasGiri\\RasGiriDetBusiness.cs", 100);
						goto end_IL_011b;
					}
					result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\RasGiri\\RasGiriDetBusiness.cs", 94);
					goto end_IL_007d;
					end_IL_011b:;
				}
				goto end_IL_006c;
				end_IL_007d:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\RasGiri\\RasGiriDetBusiness.cs", 105);
				throw;
			}
			return result;
			end_IL_006c:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\RasGiri\\RasGiriDetBusiness.cs", 111), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\RasGiri\\RasGiriDetBusiness.cs", 111);
			}
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> SaveRangeAsync(CancellationToken token, List<RasGiriDetBusiness> lst, short tryCount = 10, string tranName = null)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (lst == null || !lst.Any())
			{
				return ret;
			}
			foreach (RasGiriDetBusiness item in lst)
			{
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\RasGiri\\RasGiriDetBusiness.cs", 123);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(token, awaitOnSemaphore: true, tryCount, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\RasGiri\\RasGiriDetBusiness.cs", 124);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\RasGiri\\RasGiriDetBusiness.cs", 129);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\RasGiri\\RasGiriDetBusiness.cs", 130);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAllByMasterGuidAsync(Guid masterGuid, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = null)
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAllByMasterGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\RasGiri\\RasGiriDetBusiness.cs", 142), ReturnedState.None, "RemoveAllByMasterGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\RasGiri\\RasGiriDetBusiness.cs", 142);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IRasGiriDet.RemoveAllByMasterGuidAsync(masterGuid, tryCount, token, tranName), ReturnedState.None, "RemoveAllByMasterGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\RasGiri\\RasGiriDetBusiness.cs", 144);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAllByMasterGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\RasGiri\\RasGiriDetBusiness.cs", 148);
				ret.AddReturnedValue(ex, "", "RemoveAllByMasterGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\RasGiri\\RasGiriDetBusiness.cs", 149);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "RemoveAllByMasterGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\RasGiri\\RasGiriDetBusiness.cs", 154), ReturnedState.None, "RemoveAllByMasterGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\RasGiri\\RasGiriDetBusiness.cs", 154);
			}
		}
		return ret;
	}

	public override void Dispose()
	{
		Dispose(disposing: true);
	}

	public override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				if (this.OnDisposed != null)
				{
					PacketParser.Utilities.RaiseEvent(this.OnDisposed, this, new EventArgs(), default(CancellationToken), 60, "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\RasGiri\\RasGiriDetBusiness.cs");
				}
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\RasGiri\\RasGiriDetBusiness.cs", 171);
		}
	}
}
