using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Business.Accounting.Cheque;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.DefaultsCoding;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;

namespace EntityCache.Business;

public class PardakhtCheckMBusiness : BaseKhazaneDetail, IPardakhtCheckM, IParDarDet, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, INotifyPropertyChanged
{
	private string _desc;

	private Guid? _decimalGuid;

	private Guid? creditTafsilGuid;

	private Guid creditMoeinGuid;

	private CancellationTokenSource _updateDescToken = new CancellationTokenSource();

	public override KhazaneType Type => KhazaneType.ChequeM;

	public override string DescriptionForUi
	{
		get
		{
			if (!string.IsNullOrEmpty(_desc))
			{
				return _desc;
			}
			_updateDescToken?.Cancel();
			_updateDescToken = new CancellationTokenSource();
			CancellationToken token = _updateDescToken.Token;
			Task.Run(() => UpdateDescForDisplayAsync(token));
			return _desc;
		}
	}

	public string RowDesc { get; set; }

	public Guid GuidCheck { get; set; }

	public Guid? DecimalGuid
	{
		get
		{
			return _decimalGuid;
		}
		set
		{
			if ((value ?? Guid.Empty) == Guid.Empty)
			{
				_decimalGuid = null;
			}
			else
			{
				_decimalGuid = value;
			}
		}
	}

	public new Guid MasterGuid { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now;


	public bool Status { get; set; } = true;


	public string SayadiNationalCode { get; set; }

	public string SayadiName { get; set; }

	public bool SayadiRegistered { get; set; }

	public override FileManagement_Parts Part => FileManagement_Parts.Cheque;

	public new event PropertyChangedEventHandler PropertyChanged;

	public PardakhtCheckMBusiness(string contextGuid)
	{
		base.ContextGuid = contextGuid;
	}

	public async Task<Guid> GetCreditMoeinGuidAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (creditMoeinGuid == Guid.Empty)
			{
				creditMoeinGuid = (await LocalSettingBusiness.GetSettingAsync(base.ContextGuid, token, tranName)).CodeSetting.CheckMCoding.Debit_MoeinGuid_Reception;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetCreditMoeinGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtCheckMBusiness.cs", 65);
			throw;
		}
		return creditMoeinGuid;
	}

	public void SetCreditMoeinGuid(Guid value)
	{
		creditMoeinGuid = value;
	}

	public async Task<Guid?> GetCreditTafsilGuidAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if ((creditTafsilGuid ?? Guid.Empty) == Guid.Empty)
			{
				creditTafsilGuid = (await LocalSettingBusiness.GetSettingAsync(base.ContextGuid, token, tranName)).CodeSetting.CheckMCoding.Debit_TafsilGuid_Reception;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetCreditTafsilGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtCheckMBusiness.cs", 83);
			throw;
		}
		return creditTafsilGuid;
	}

	public void SetCreditTafsilGuid(Guid? value)
	{
		creditTafsilGuid = value;
	}

	public static Task<IEnumerable<PardakhtCheckMBusiness>> GetAllAsync(string contextGuid, Guid chequeGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IPardakhtCheckM.GetAllWithChequeGuidAsync(chequeGuid, token, tranName);
	}

	private async Task UpdateDescForDisplayAsync(CancellationToken token)
	{
		_ = 3;
		try
		{
			if (token.IsCancellationRequested || GuidCheck == Guid.Empty)
			{
				return;
			}
			ReceptionCheckBusiness str = await ReceptionCheckBusiness.GetAsync(base.ContextGuid, GuidCheck, token);
			ReceptionBusiness receptionBusiness = await str.GetReceptionMasterAsync(EnSystem.None, token);
			string text = "";
			if (receptionBusiness != null && !str.Aval2re)
			{
				text = await (receptionBusiness?.GetFullNameAsync(EnSystem.None, token));
			}
			else if ((str?.A2re_PardazandeTafsilGuid ?? Guid.Empty) != Guid.Empty)
			{
				TafsilBusiness tafsilBusiness = await TafsilBusiness.GetAsync(base.ContextGuid, EnSystem.None, str.A2re_PardazandeTafsilGuid.Value, "", token, "UpdateDescForDisplayAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtCheckMBusiness.cs");
				text = ((!str.IsEnteghali) ? (tafsilBusiness?.Name + " (ابتدای دوره)") : (tafsilBusiness?.Name + " (ابتدای دوره انتقالی)"));
			}
			_desc = "سررسید : " + str?.DateSh + " | بانک : " + str?.BankName + "  | شماره : " + str?.Number + "  | پردازنده : " + text;
			if (!string.IsNullOrEmpty(RowDesc))
			{
				_desc = _desc + " ** شرح سطر: " + RowDesc;
			}
			if (this.PropertyChanged == null || UiInvokerHolder.Instance == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread(delegate
			{
				try
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DescriptionForUi"));
				}
				catch (Exception ex5)
				{
					WebErrorLog.Instance.StartLog(ex5, "", "UpdateDescForDisplayAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtCheckMBusiness.cs", 113);
				}
			}, "UpdateDescForDisplayAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtCheckMBusiness.cs");
			UiInvokerHolder.Instance.RunOnUiThread(delegate
			{
				try
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DescriptionForUi"));
				}
				catch (Exception ex4)
				{
					WebErrorLog.Instance.StartLog(ex4, "", "UpdateDescForDisplayAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtCheckMBusiness.cs", 114);
				}
			}, "UpdateDescForDisplayAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtCheckMBusiness.cs");
		}
		catch (OperationCanceledException)
		{
		}
		catch (ThreadAbortException)
		{
		}
		catch (Exception ex3)
		{
			WebErrorLog.Instance.StartLog(ex3, "", "UpdateDescForDisplayAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtCheckMBusiness.cs", 121);
			throw;
		}
	}

	public override void Dispose(bool disposing)
	{
		Task.Run((Func<Task?>)DisposeAsync);
	}

	private async Task DisposeAsync()
	{
		base.Dispose(disposing: true);
	}

	public async Task<ReturnedSaveFuncInfo> CheckValidationAsync(EnSystem system, PardakhtBusiness master, bool isDeleteAction, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (system == EnSystem.Transfer || !isDeleteAction)
			{
				return res;
			}
			ChequeMOperationFilter filter = new ChequeMOperationFilter
			{
				ChequeGuid = GuidCheck
			};
			ChequeMOperationBusiness chequeMOperationBusiness = (await ChequeMOperationBusiness.GetAllAsync(base.ContextGuid, filter, token))?.OrderByDescending((ChequeMOperationBusiness q) => q.Ordinal)?.FirstOrDefault();
			if (chequeMOperationBusiness != null && chequeMOperationBusiness.AsGuid != master.Guid && master.ChangeStatus == EntityChangeSt.Edited)
			{
				res.AddError(EnSystem.UserInterface, "به دلیل وجود عملیات دیگر پس از عملیات جاری، این عملیات قابل انجام نمی باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtCheckMBusiness.cs", 138);
				return res;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtCheckMBusiness.cs", 144);
			res.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtCheckMBusiness.cs", 145);
		}
		return res;
	}

	public static Task<IEnumerable<PardakhtCheckMBusiness>> GetAllAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IPardakhtCheckM.GetAllAsync(token, tranName);
	}

	public static async Task<ReturnedSaveFuncInfo> FixDataAsync(string contextGuid, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			IEnumerable<PardakhtCheckMBusiness> enumerable = (await GetAllAsync(contextGuid, token))?.Where((PardakhtCheckMBusiness q) => q.creditTafsilGuid == ParentDefaults.TafsilCoding.CLSTafsil29766);
			if (enumerable == null || !enumerable.Any())
			{
				return res;
			}
			List<IGrouping<Guid, PardakhtCheckMBusiness>> list = (from q in enumerable
				group q by q.MasterGuid).ToList();
			foreach (IGrouping<Guid, PardakhtCheckMBusiness> payment in list)
			{
				foreach (PardakhtCheckMBusiness paymentCheque in payment)
				{
					ReceptionCheckBusiness cheque = await ReceptionCheckBusiness.GetAsync(contextGuid, paymentCheque.GuidCheck, token);
					if (cheque != null)
					{
						bool hasChange = false;
						if (await paymentCheque.GetCreditMoeinGuidAsync(token) != cheque.DebitMoeinGuid)
						{
							paymentCheque.SetCreditMoeinGuid(cheque.DebitMoeinGuid);
							hasChange = true;
						}
						if (await paymentCheque.GetCreditTafsilGuidAsync(token) != cheque.DebitTafsilGuid)
						{
							paymentCheque.SetCreditTafsilGuid(cheque.DebitTafsilGuid);
							hasChange = true;
						}
						if (hasChange)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await paymentCheque.SaveAsync(EnSystem.None, 10, token), ReturnedState.None, "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtCheckMBusiness.cs", 180);
						}
					}
				}
				PardakhtBusiness pardakhtBusiness = await PardakhtBusiness.GetAsync(contextGuid, payment.Key, fromDatabase: true, token);
				if (pardakhtBusiness != null)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await pardakhtBusiness.SaveAsync(EnSystem.None, sendToServer: false, Priority.Low, token, awaitOnSemaphore: true, 10), ReturnedState.None, "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtCheckMBusiness.cs", 184);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtCheckMBusiness.cs", 189);
			res.AddReturnedValue(ex, "", "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtCheckMBusiness.cs", 190);
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(base.ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtCheckMBusiness.cs", 202), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtCheckMBusiness.cs", 202);
				}
				if (ret.HasError)
				{
					return ret;
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(base.ContextGuid).CacheRepository.IPardakhtCheckM.SaveAsync(new List<PardakhtCheckMBusiness> { this }, MasterGuid, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtCheckMBusiness.cs", 205);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtCheckMBusiness.cs", 209);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtCheckMBusiness.cs", 210);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(base.ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtCheckMBusiness.cs", 215), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtCheckMBusiness.cs", 215);
			}
			if (!ret.HasError && system == EnSystem.UserInterface)
			{
				ClsCache.GetCache(base.ContextGuid).BusinessAggregator.Events.ChequM.RaiseOperationSaved(this, EventArgs.Empty);
			}
		}
		return ret;
	}

	public override Task<string> GetShortDescriptionAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		throw new NotImplementedException();
	}
}
