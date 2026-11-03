using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Business.Operations;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;
using PacketParser.Settings.Accounting;

namespace EntityCache.WebService.Repository;

public class HesabTreeWebServiceRepository : IDisposable
{
	private ClsCache Cache;

	public HesabTreeWebServiceRepository(ClsCache cache)
	{
		try
		{
			Cache = cache;
			Cache.Context.WebHesabTree.OnReadResponse += HesabTreeInstance_OnReadResponse;
			Cache.Context.WebHesabTree.OnSync += HesabTreeInstance_OnSync;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\HesabTreeWebServiceRepository.cs", 31);
		}
	}

	private async Task HesabTreeInstance_OnSync(object sender, ConnectionEventArgs<WebHesabTree> e)
	{
		using (new JaegerMonitor("HesabTreeInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\HesabTreeWebServiceRepository.cs"))
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			int num = 0;
			new ReturnedSaveFuncInfo();
			long num2 = 0L;
			long num3 = 0L;
			long num4 = 0L;
			long num5 = 0L;
			try
			{
				throw new Exception("سرور نباید این نوع داده را ارسال کند.".GetTranslate());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.Error(ex, $"Function Line is : {num}", "HesabTreeInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\HesabTreeWebServiceRepository.cs", 103);
			}
			WebErrorLog.Instance.Trace($"**********HesabTreeSyncSaveTime:{stopwatch.ElapsedMilliseconds}//t1:{num2}//t2:{num3}//t3:{num4}//t4:{num5}//Count:{(e?.ConnectionData?.Count).GetValueOrDefault()}**********", "", "HesabTreeInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\HesabTreeWebServiceRepository.cs", 104);
		}
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(string contextGuid, EnSystem system, short autoFixValidationErrorsCount, IEnumerable<WebHesabTree> items, CancellationToken token, bool updateAccount, bool send2server, TransferSettings setting, TransferSqquestAggregator transferwaiter = null, short maxParalelTasks = 1)
	{
		int line = 0;
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			line = 4;
			HesabTreesSeparation lstHesabs = await HesabTreesSeparation.GetFactoryAsync(contextGuid, items);
			line = 51;
			if (setting.Kol)
			{
				transferwaiter?.Kol?.TaskStatus?.TrySetResult(result: true);
			}
			else
			{
				if (transferwaiter != null)
				{
					transferwaiter.AddAccountInformation("در حال انتظار حساب های کل برای گروه حساب ها".GetTranslate());
					await (transferwaiter?.HesabGroup?.TaskStatus?.Task);
				}
				if (lstHesabs?.ListKol != null && lstHesabs.ListKol.Any())
				{
					transferwaiter?.AddAccountInformation("شروع عملیات ذخیره سازی حساب های کل".GetTranslate());
					ReturnedSaveFuncInfo returnedSaveFuncInfo = await KolBusiness.SaveRangeAsync(system, lstHesabs.ListKol, send2server, Priority.High, updateAccount, autoFixValidationErrorsCount, awaitOnSemaphore: true, token, wait4Send: false, 10, "", transferwaiter?.Kol?.PercentageAction);
					transferwaiter?.Kol?.TaskStatus?.TrySetResult(!returnedSaveFuncInfo.HasError);
					ret.AddReturnedValue(returnedSaveFuncInfo, ReturnedState.Information, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\HesabTreeWebServiceRepository.cs", 132);
					if (returnedSaveFuncInfo.HasError)
					{
						transferwaiter?.AddAccountError("خطا در ذخیره سازی حساب ها کل".GetTranslate() + "  " + returnedSaveFuncInfo.GetErrorMessage());
						return ret;
					}
					transferwaiter?.AddAccountSuccess("پایان عملیات ذخیره سازی حساب های کل با ثبت ".GetTranslate() + " " + $"{lstHesabs.ListKol.Count}" + " " + " حساب کل".GetTranslate());
				}
				else
				{
					transferwaiter?.Kol?.TaskStatus?.TrySetResult(result: true);
				}
				setting.Kol = true;
			}
			if (setting.Moein)
			{
				transferwaiter?.Moein?.TaskStatus?.TrySetResult(result: true);
			}
			else
			{
				line = 52;
				if (lstHesabs?.ListMoein != null && lstHesabs.ListMoein.Any())
				{
					transferwaiter?.AddAccountInformation("شروع عملیات ذخیره سازی حساب های معین".GetTranslate());
					foreach (MoeinBusiness moein in lstHesabs.ListMoein)
					{
						if (moein.KolGuid == Guid.Empty)
						{
							MoeinBusiness moeinBusiness = moein;
							moeinBusiness.KolGuid = (await KolBusiness.GetAllAsync(contextGuid, "", token, true))?.FirstOrDefault()?.Guid ?? Guid.Empty;
							if (!(moein.KolGuid != Guid.Empty))
							{
								ReturnedSaveFuncInfoWithValue<KolBusiness> returnedSaveFuncInfoWithValue = await KolBusiness.SaveAsnadDaryaftaniDefaultsync(contextGuid, system);
								moein.KolGuid = returnedSaveFuncInfoWithValue.value.Guid;
								ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.Information, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\HesabTreeWebServiceRepository.cs", 162);
								ret.throwExceptionIfError();
							}
						}
					}
					ReturnedSaveFuncInfo returnedSaveFuncInfo2 = await MoeinBusiness.SaveRangeAsync(system, lstHesabs.ListMoein, updateAccount, autoFixValidationErrorsCount, send2server, Priority.High, 10, "", token, transferwaiter?.Moein?.PercentageAction);
					transferwaiter?.Moein?.TaskStatus?.TrySetResult(!returnedSaveFuncInfo2.HasError);
					setting.Moein = !returnedSaveFuncInfo2.HasError;
					ret.AddReturnedValue(returnedSaveFuncInfo2, ReturnedState.Information, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\HesabTreeWebServiceRepository.cs", 169);
					if (returnedSaveFuncInfo2.HasError)
					{
						transferwaiter?.AddAccountError("خطا در ذخیره سازی حساب های معین".GetTranslate() + "  " + returnedSaveFuncInfo2.GetErrorMessage());
						return ret;
					}
					transferwaiter?.AddAccountSuccess("پایان عملیات ذخیره سازی حساب های معین با ثبت ".GetTranslate() + " " + $"{lstHesabs.ListMoein.Count} " + " " + "حساب معین".GetTranslate());
				}
			}
			line = 53;
			if (setting.Tafsil)
			{
				transferwaiter?.Tafsil?.TaskStatus?.TrySetResult(result: true);
			}
			else if (lstHesabs?.ListTafsil != null && lstHesabs.ListTafsil.Any())
			{
				if (transferwaiter != null)
				{
					transferwaiter.AddAccountInformation("درحال انتظار حساب های تفصیلی برای گروه حساب های تفصیلی".GetTranslate());
					await (transferwaiter?.TafsilGroup?.TaskStatus?.Task);
				}
				transferwaiter?.AddAccountInformation("شروع عملیات ذخیره سازی حساب های تفصیلی".GetTranslate());
				new ReturnedSaveFuncInfo();
				ReturnedSaveFuncInfo returnedSaveFuncInfo3 = await TafsilBusiness.SaveRangeAsync(contextGuid, system, keepCurrentCodeAndChangedOtherTafsilCodesIfWasDuplicate: true, lstHesabs.ListTafsil, updateAccount, autoFixValidationErrorsCount, token, 1, transferwaiter?.Tafsil?.PercentageAction);
				transferwaiter?.Tafsil?.TaskStatus?.TrySetResult(!returnedSaveFuncInfo3.HasError);
				ret.AddReturnedValue(returnedSaveFuncInfo3, ReturnedState.Information, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\HesabTreeWebServiceRepository.cs", 198);
				if (returnedSaveFuncInfo3.HasError)
				{
					transferwaiter?.AddAccountError("خطا در ذخیره سازی حساب های تفصیلی".GetTranslate() + "  " + returnedSaveFuncInfo3.GetErrorMessage());
					return ret;
				}
				transferwaiter?.AddAccountSuccess("پایان عملیات ذخیره سازی حساب های تفصیلی با ثبت ".GetTranslate() + " " + $"{lstHesabs.ListTafsil.Count}" + " " + "حساب تفصیلی".GetTranslate());
				ReturnedSaveFuncInfo returnedSaveFuncInfo4 = ret;
				returnedSaveFuncInfo4.AddReturnedValue(await BankBusiness.SaveFromTafsil(contextGuid, system, token, lstHesabs.ListTafsil, awaitOnSemaphore: true, updateAccount, 10), ReturnedState.Information, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\HesabTreeWebServiceRepository.cs", 206);
				returnedSaveFuncInfo4 = ret;
				returnedSaveFuncInfo4.AddReturnedValue(await SandoghBusiness.SaveFromTafsil(contextGuid, system, token, lstHesabs.ListTafsil, awaitOnSemaphore: true, updateAccount, send2server, 10), ReturnedState.Information, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\HesabTreeWebServiceRepository.cs", 207);
				setting.Tafsil = true;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\HesabTreeWebServiceRepository.cs", 219);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\HesabTreeWebServiceRepository.cs", 220);
		}
		finally
		{
			ret?.AddReturnedValue(ret, ReturnedState.Information, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\HesabTreeWebServiceRepository.cs", 222);
		}
		return ret;
	}

	private async Task HesabTreeInstance_OnReadResponse(object sender, ConnectionEventArgs<WebHesabTree> e)
	{
		using (new JaegerMonitor("HesabTreeInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\HesabTreeWebServiceRepository.cs"))
		{
			int line = 0;
			string tranName = Guid.NewGuid().ToString();
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				_ = 6;
				try
				{
					if (Cache.Source == ENSource.Accounting)
					{
						throw new Exception("سرور نباید این نوع داده را ارسال کند.".GetTranslate());
					}
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(e.Connection.ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "HesabTreeInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\HesabTreeWebServiceRepository.cs", 235), ReturnedState.None, "HesabTreeInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\HesabTreeWebServiceRepository.cs", 235);
					line = 1;
					if (e.ConnectionData == null)
					{
						line = 3;
						WebErrorLog.Instance.Warning("Hesabtree received but data is null!", "", "HesabTreeInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\HesabTreeWebServiceRepository.cs", 240);
						line = 4;
						return;
					}
					line = 5;
					List<WebHesabTree> connectionData = e.ConnectionData;
					if (connectionData != null && !connectionData.Any())
					{
						line = 6;
						WebErrorLog.Instance.Warning("Hesabtree received but data is empty!", "", "HesabTreeInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\HesabTreeWebServiceRepository.cs", 248);
						return;
					}
					line = 10;
					WebErrorLog.Instance.Debug("Hesabtree received ....", "", "HesabTreeInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\HesabTreeWebServiceRepository.cs", 252);
					line = 11;
					List<WebHesabTree> lstWebHesabtree = e.ConnectionData.ToList();
					line = 12;
					HesabTreesSeparation lstHesabs = await HesabTreesSeparation.GetFactoryAsync(Cache.Context.Guid, lstWebHesabtree);
					line = 14;
					bool updateAccount = ClsCache.GetCache(e.Connection.ContextGuid).Source != ENSource.Accounting;
					bool sendToServer = Cache.Source == ENSource.Accounting;
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await KolBusiness.SaveRangeAsync(EnSystem.SignalR, lstHesabs.ListKol, sendToServer, Priority.High, updateAccount, 10, awaitOnSemaphore: true, e.Connection.cancellationToken, wait4Send: false, 10, tranName), ReturnedState.None, "HesabTreeInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\HesabTreeWebServiceRepository.cs", 260);
					line = 15;
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await MoeinBusiness.SaveRangeAsync(EnSystem.SignalR, lstHesabs.ListMoein, updateAccount, 10, send2Server: false, Priority.High, 10, "", e.Connection.cancellationToken), ReturnedState.None, "HesabTreeInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\HesabTreeWebServiceRepository.cs", 262);
					line = 16;
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await TafsilBusiness.SaveRangeAsync(e.Connection.ContextGuid, EnSystem.SignalR, keepCurrentCodeAndChangedOtherTafsilCodesIfWasDuplicate: true, lstHesabs.ListTafsil, updateAccount, 10, e.Connection.cancellationToken, 20), ReturnedState.None, "HesabTreeInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\HesabTreeWebServiceRepository.cs", 264);
					line = 17;
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await BankBusiness.SaveFromTafsil(e.Connection.ContextGuid, EnSystem.SignalR, e.Connection.cancellationToken, lstHesabs.ListTafsil, updateAccount, updateAccount: true, 1, tranName), ReturnedState.None, "HesabTreeInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\HesabTreeWebServiceRepository.cs", 266);
					line = 18;
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SandoghBusiness.SaveFromTafsil(e.Connection.ContextGuid, EnSystem.SignalR, e.Connection.cancellationToken, lstHesabs.ListTafsil, awaitOnSemaphore: true, updateAccount, send2server: false, 1, tranName), ReturnedState.None, "HesabTreeInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\HesabTreeWebServiceRepository.cs", 268);
					line = 19;
					line = 20;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, $"The Line IS Error :{line}", "HesabTreeInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\HesabTreeWebServiceRepository.cs", 279);
					ret.AddReturnedValue(ex, "", "HesabTreeInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\HesabTreeWebServiceRepository.cs", 280);
				}
			}
			finally
			{
				if (ret.HasError)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(e.Connection.ContextGuid, tranName, ret.HasError, disposeContext: true, "HesabTreeInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\HesabTreeWebServiceRepository.cs", 285), ReturnedState.None, "HesabTreeInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\HesabTreeWebServiceRepository.cs", 285);
				}
			}
		}
	}

	public void Dispose()
	{
		try
		{
			Cache.Context.WebHesabTree.OnReadResponse -= HesabTreeInstance_OnReadResponse;
			Cache.Context.WebHesabTree.OnSync -= HesabTreeInstance_OnSync;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\HesabTreeWebServiceRepository.cs", 301);
		}
	}
}
