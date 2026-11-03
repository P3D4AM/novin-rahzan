using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Interfaces;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;
using PacketParser.Records.Customers;
using PacketParser.Records.OccasionCalendar;
using PacketParser.Records.Products;
using PacketParser.Records.TaxService;
using PacketParser.Settings.Accounting;
using PacketParser.Settings.Attendance;
using PacketParser.Settings.Global;
using PacketParser.TransferLayer.Waiters;

namespace EntityCache.Business;

public class LocalSettingBusiness : ILocalSetting, IHasContext
{
	private static Guid dbGuid = Guid.Empty;

	private static SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

	private static SemaphoreSlim _semaphoreSend2Server = new SemaphoreSlim(1, 1);

	private static ConcurrentDictionary<string, AccGlobalSettings> _dic;

	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private static ConcurrentDictionary<Guid, Guid> _lastProductUnit = new ConcurrentDictionary<Guid, Guid>();

	private static ConcurrentDictionary<Guid, ProductSearchSettingRecord> _productSearchDic = new ConcurrentDictionary<Guid, ProductSearchSettingRecord>();

	private static ConcurrentDictionary<Guid, CustomerSearchSettingRecord> _customerSearchDic = new ConcurrentDictionary<Guid, CustomerSearchSettingRecord>();

	private static ConcurrentDictionary<string, TaxEconomicCodeRecord> _taxEcoCodeDic = new ConcurrentDictionary<string, TaxEconomicCodeRecord>();

	private static ConcurrentDictionary<string, ActivationAbonmanSettings> _abonmanSettingDic = new ConcurrentDictionary<string, ActivationAbonmanSettings>();

	private static ConcurrentDictionary<string, DeviceServiceSettings> _deviceSettingDic = new ConcurrentDictionary<string, DeviceServiceSettings>();

	private static ConcurrentDictionary<string, NPSMS_WebServiceSettings> _NPSMSsettings { get; set; } = new ConcurrentDictionary<string, NPSMS_WebServiceSettings>();


	private static ConcurrentDictionary<string, ScaleSettings> _scaleSettings { get; set; } = new ConcurrentDictionary<string, ScaleSettings>();


	private static ConcurrentDictionary<string, VersionUpdateSettings> _versionUpdateSettings { get; set; } = new ConcurrentDictionary<string, VersionUpdateSettings>();


	private static ConcurrentDictionary<string, TransferSettings> _transferSettings { get; set; } = new ConcurrentDictionary<string, TransferSettings>();


	private static ConcurrentDictionary<string, BazsaziResultModel> _bazsaziResultModel { get; set; } = new ConcurrentDictionary<string, BazsaziResultModel>();


	private static ConcurrentDictionary<string, KioskSettings> _kioskSettinhs { get; set; } = new ConcurrentDictionary<string, KioskSettings>();


	private static ConcurrentDictionary<string, TaxUnits> _taxUnits { get; set; } = new ConcurrentDictionary<string, TaxUnits>();


	private static ConcurrentDictionary<string, TaxErrors> _taxErrors { get; set; } = new ConcurrentDictionary<string, TaxErrors>();


	private static ConcurrentDictionary<string, TaxCurrency> _taxCurrency { get; set; } = new ConcurrentDictionary<string, TaxCurrency>();


	private static ConcurrentDictionary<string, TaxCustomInfo> _taxCustomInfo { get; set; } = new ConcurrentDictionary<string, TaxCustomInfo>();


	private static ConcurrentDictionary<string, ChequeQrSetting> _chequeQrList { get; set; } = new ConcurrentDictionary<string, ChequeQrSetting>();


	private static ConcurrentDictionary<string, ShabaNumberSetting> _shabaList { get; set; } = new ConcurrentDictionary<string, ShabaNumberSetting>();


	public string Key { get; set; }

	public string Value { get; set; }

	public string ContextGuid { get; set; }

	public LocalSettingBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
		_dic = new ConcurrentDictionary<string, AccGlobalSettings>();
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<LocalSettingBusiness> items, bool awaitOnSemaphore = true, short tryCount = 10, string tranName = "", CancellationToken token = default(CancellationToken), Action<PercenteCompletedEventArgs> percentCompleted = null)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		CancellationTokenSource invokeToken2 = new CancellationTokenSource();
		try
		{
			if (!(items?.Any() ?? false))
			{
				return ret;
			}
			int current = 0;
			foreach (LocalSettingBusiness item in items)
			{
				current = checked(current + 1);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(token, awaitOnSemaphore, tryCount, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 73);
				if (percentCompleted != null)
				{
					invokeToken2?.Cancel();
					invokeToken2 = new CancellationTokenSource(60000);
					CancellationToken token2 = invokeToken2.Token;
					percentCompleted(new PercenteCompletedEventArgs(items.Count(), current, item.Key, token2));
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 85);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 86);
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
					percentCompleted(new PercenteCompletedEventArgs(items?.Count() ?? 0, items?.Count() ?? 0, "", invokeTkn));
				});
			}
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(CancellationToken token = default(CancellationToken), bool awaitOnSemaphore = true, short tryCount = 10, string tranName = "")
	{
		int line = 0;
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
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "LocalSettingBusiness/SaveAsync", 60000, token))
				{
					if (!dis.Token.IsCancellationRequested)
					{
						line = 1;
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 112), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 112);
						}
						transactionHasBeenCreated = true;
						line = 2;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ILocalSetting.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 115);
						goto end_IL_0122;
					}
					result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 110);
					goto end_IL_0084;
					end_IL_0122:;
				}
				goto end_IL_0073;
				end_IL_0084:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"1111//Line:{line}//ClsCache.GetCache(contextGuid).DatabaseManager is null:{ClsCache.GetCache(ContextGuid)?.DatabaseManager == null}//SaveAsync(short tryCount = 10, string tranName)//ClsCache.GetCache(contextGuid).DatabaseManager is null:{ClsCache.GetCache(ContextGuid)?.DatabaseManager == null}", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 119);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 120);
				goto end_IL_0073;
			}
			return result;
			end_IL_0073:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 125), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 125);
			}
		}
		return ret;
	}

	public static async Task<string> GetAsync(string contextGuid, string key, bool awaitOnSemaphore = true, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (ClsCache.GetCache(contextGuid) == null)
		{
			throw new NullReferenceException("cache for context(" + contextGuid + ") is null!!\r\nHistory:" + ClsCache.GetCacheHistory(contextGuid));
		}
		if (ClsCache.GetCache(contextGuid).IsDisposed)
		{
			throw new NullReferenceException("cache for context(" + contextGuid + ") has been disposed!!\r\nHistory:" + ClsCache.GetCacheHistory(contextGuid));
		}
		if (ClsCache.GetCache(contextGuid)?.CacheRepository?.ILocalSetting == null)
		{
			return null;
		}
		bool semaphoreHasBeenTaken = false;
		try
		{
			if (awaitOnSemaphore)
			{
				await _semaphore.WaitAsync();
				semaphoreHasBeenTaken = true;
			}
			return await ClsCache.GetCache(contextGuid).CacheRepository.ILocalSetting.GetAsync(key, token, tranName);
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 148);
			throw;
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_semaphore.Release();
			}
		}
	}

	public static string Get(string contextGuid, string key, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (ClsCache.GetCache(contextGuid)?.CacheRepository?.ILocalSetting == null)
		{
			throw new NullReferenceException("ILocalSetting is null");
		}
		return ClsCache.GetCache(contextGuid).CacheRepository.ILocalSetting.Get(key, token, tranName);
	}

	public static async Task<Guid> GetDbGuidAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (ClsCache.GetCache(contextGuid)?.CacheRepository?.ILocalSetting == null)
		{
			throw new NullReferenceException("settingRepo");
		}
		using (new JaegerMonitor("GetDbGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs"))
		{
			int line = 0;
			bool semaphoreHasBeenTaken = false;
			try
			{
				await _semaphore.WaitAsync();
				semaphoreHasBeenTaken = true;
				line = 1;
				if (dbGuid != Guid.Empty)
				{
					return dbGuid;
				}
				line = 5;
				string text = (await ClsCache.GetCache(contextGuid).CacheRepository.ILocalSetting.GetAsync("DbGuid", token, tranName)) ?? "";
				line = 6;
				if (string.IsNullOrEmpty(text))
				{
					line = 7;
					Guid guid = Guid.NewGuid();
					line = 8;
					ReturnedSaveFuncInfo obj = await SaveAsync(contextGuid, "DbGuid", guid.ToString(), awaitOnSemaphore: true, 10);
					line = 9;
					if (obj.HasError)
					{
						return Guid.NewGuid();
					}
					line = 10;
					dbGuid = guid;
					line = 11;
					return guid;
				}
				line = 12;
				return Guid.Parse(text);
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"Line:{line}//ClsCache.GetCache(contextGuid).CacheRepository.ILocalSetting is null:{ClsCache.GetCache(contextGuid)?.CacheRepository?.ILocalSetting == null}//isDisposed:{ClsCache.GetCache(contextGuid)?.CacheRepository?.IsDisposed}//just for send the error always:{Guid.NewGuid()}", "GetDbGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 199);
				throw;
			}
			finally
			{
				if (semaphoreHasBeenTaken)
				{
					_semaphore.Release();
				}
			}
		}
	}

	public static AccGlobalSettings GetSetting(string contextGuid, CancellationToken token = default(CancellationToken))
	{
		ClsCache obj = ClsCache.GetCache(contextGuid) ?? throw new NullReferenceException("Cache is null!!");
		if (obj.IsDisposed)
		{
			throw new ObjectDisposedException("Cache is disposed!!");
		}
		if (obj.CacheRepository?.ILocalSetting == null)
		{
			throw new NullReferenceException("cache.CacheRepository?.ILocalSetting is null!!");
		}
		AccGlobalSettings value = null;
		try
		{
			_dic.TryGetValue(AccGlobalSettings.GetKeyName(contextGuid), out value);
			if (value == null || value.IsDisposed)
			{
				value = AccGlobalSettings.FromString(Get(contextGuid, AccGlobalSettings.GetKeyName(""), token)) ?? new AccGlobalSettings();
				_dic.TryRemove(AccGlobalSettings.GetKeyName(contextGuid), out var _);
				_dic.TryAdd(AccGlobalSettings.GetKeyName(contextGuid), value);
			}
			if (value == null)
			{
				throw new NullReferenceException("setting is null!! توقع ندارم این شرط اتفاق بیفته");
			}
			return value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetSetting", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 227);
			throw;
		}
	}

	public static async Task<AccGlobalSettings> GetSettingAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		AccGlobalSettings value = null;
		try
		{
			if (_dic == null)
			{
				_dic = new ConcurrentDictionary<string, AccGlobalSettings>();
			}
			if (_dic.TryGetValue(AccGlobalSettings.GetKeyName(contextGuid), out value) && value != null)
			{
				return value;
			}
			_dic.TryGetValue(AccGlobalSettings.GetKeyName(contextGuid), out value);
			if (value == null || value.IsDisposed)
			{
				value = AccGlobalSettings.FromString(await GetAsync(contextGuid, AccGlobalSettings.GetKeyName(""), awaitOnSemaphore: false, token, tranName)) ?? new AccGlobalSettings();
				_dic.TryRemove(AccGlobalSettings.GetKeyName(contextGuid), out var _);
				_dic.TryAdd(AccGlobalSettings.GetKeyName(contextGuid), value);
			}
			if (value == null)
			{
				throw new NullReferenceException("setting is null!! توقع ندارم این شرط اتفاق بیفته".GetTranslate());
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetSettingAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 257);
			throw;
		}
		return value;
	}

	public static async Task<TransferSettings> GetTransferSettingAsync(string contextGuid, CancellationToken token = default(CancellationToken))
	{
		bool semaphoreHasBeenTaken = false;
		try
		{
			await _semaphore.WaitAsync();
			semaphoreHasBeenTaken = true;
			_transferSettings.TryGetValue(contextGuid, out var value);
			if (value == null)
			{
				value = TransferSettings.FromString(await GetAsync(contextGuid, "Acc_TranferSettings", awaitOnSemaphore: false, token)) ?? new TransferSettings();
				_transferSettings.TryRemove("Acc_TranferSettings", out var _);
				_transferSettings.TryAdd("Acc_TranferSettings", value);
			}
			return value ?? throw new NullReferenceException("نباید نال بشه.".GetTranslate());
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetTransferSettingAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 283);
			throw;
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_semaphore.Release();
			}
		}
	}

	public static async Task<NPSMS_WebServiceSettings> GetWebServiceSettingsAsync(string contextGuid, CancellationToken token = default(CancellationToken))
	{
		bool semaphoreHasBeenTaken = false;
		try
		{
			await _semaphore.WaitAsync();
			semaphoreHasBeenTaken = true;
			_NPSMSsettings.TryGetValue(contextGuid, out var value);
			if (value == null)
			{
				value = NPSMS_WebServiceSettings.FromString(await GetAsync(contextGuid, "AccNPSMS_WebServiceSettings", awaitOnSemaphore: false, token)) ?? new NPSMS_WebServiceSettings();
				if (!(value.SignalRUrlList?.Any() ?? false))
				{
					value.SignalRUrlList = new List<string> { "http://www.NovinRank.ir:8040/HubCore" };
				}
				_NPSMSsettings.TryRemove("AccNPSMS_WebServiceSettings", out var _);
				_NPSMSsettings.TryAdd("AccNPSMS_WebServiceSettings", value);
			}
			return value ?? throw new NullReferenceException("توقع ندارم پیش بیاد".GetTranslate());
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetWebServiceSettingsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 315);
			throw;
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_semaphore.Release();
			}
		}
	}

	public static async Task<KioskSettings> GetKioskSettingsAsync(string contextGuid, CancellationToken token = default(CancellationToken))
	{
		bool semaphoreHasBeenTaken = false;
		try
		{
			await _semaphore.WaitAsync();
			semaphoreHasBeenTaken = true;
			_kioskSettinhs.TryGetValue(contextGuid, out var value);
			if (value == null)
			{
				value = KioskSettings.FromString(await GetAsync(contextGuid, "Acc_KioskSettings", awaitOnSemaphore: false, token)) ?? new KioskSettings();
				_kioskSettinhs.TryRemove("Acc_KioskSettings", out var _);
				_kioskSettinhs.TryAdd("Acc_KioskSettings", value);
			}
			return value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetKioskSettingsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 343);
			throw;
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_semaphore.Release();
			}
		}
	}

	public static async Task<ScaleSettings> GetScaleSettingsAsync(string contextGuid, CancellationToken token = default(CancellationToken), bool ignoreEmptySetting = false)
	{
		bool semaphoreHasBeenTaken = false;
		try
		{
			await _semaphore.WaitAsync();
			semaphoreHasBeenTaken = true;
			_scaleSettings.TryGetValue(contextGuid, out var value);
			if (value == null)
			{
				string text = await GetAsync(contextGuid, "Acc_ScaleSettings", awaitOnSemaphore: false, token);
				if (string.IsNullOrEmpty(text) && ignoreEmptySetting)
				{
					return new ScaleSettings();
				}
				value = ScaleSettings.FromString(text) ?? new ScaleSettings();
			}
			value = value ?? throw new NullReferenceException("توقع میره پیش نیاد".GetTranslate());
			_scaleSettings.TryRemove("Acc_ScaleSettings", out var _);
			_scaleSettings.TryAdd("Acc_ScaleSettings", value);
			return value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetScaleSettingsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 369);
			throw;
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_semaphore.Release();
			}
		}
	}

	public static async Task<BazsaziResultModel> GetBazsaziResultAsync(string contextGuid, CancellationToken token = default(CancellationToken))
	{
		bool semaphoreHasBeenTaken = false;
		try
		{
			await _semaphore.WaitAsync();
			semaphoreHasBeenTaken = true;
			_bazsaziResultModel.TryGetValue(contextGuid, out var value);
			if (value == null)
			{
				value = BazsaziResultModel.FromString(await GetAsync(contextGuid, BazsaziResultModel.KeyName, awaitOnSemaphore: false, token)) ?? new BazsaziResultModel();
				_bazsaziResultModel.TryRemove(BazsaziResultModel.KeyName, out var _);
				_bazsaziResultModel.TryAdd(BazsaziResultModel.KeyName, value);
			}
			return value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetBazsaziResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 397);
			throw;
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_semaphore.Release();
			}
		}
	}

	public static async Task<TaxUnits> GetTaxUnitsAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool semaphoreHasBeenTaken = false;
		try
		{
			await _semaphore.WaitAsync();
			semaphoreHasBeenTaken = true;
			_taxUnits.TryGetValue(contextGuid, out var value);
			if (value == null)
			{
				value = TaxUnits.FromString(await GetAsync(contextGuid, "Acc_TaxUnits", awaitOnSemaphore: false, token, tranName)) ?? new TaxUnits();
				_taxUnits.TryRemove("Acc_TaxUnits", out var _);
				_taxUnits.TryAdd("Acc_TaxUnits", value);
			}
			return value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetTaxUnitsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 425);
			throw;
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_semaphore.Release();
			}
		}
	}

	public static async Task<TaxErrors> GetTaxErrorsAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool semaphoreHasBeenTaken = false;
		try
		{
			await _semaphore.WaitAsync();
			semaphoreHasBeenTaken = true;
			_taxErrors.TryGetValue(contextGuid, out var value);
			if (value == null)
			{
				value = TaxErrors.FromString(await GetAsync(contextGuid, "Acc_TaxErrors", awaitOnSemaphore: false, token, tranName)) ?? new TaxErrors();
				_taxErrors.TryRemove("Acc_TaxErrors", out var _);
				_taxErrors.TryAdd("Acc_TaxErrors", value);
			}
			return value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetTaxErrorsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 453);
			throw;
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_semaphore.Release();
			}
		}
	}

	public static async Task<TaxCurrency> GetTaxCurrencyAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool semaphoreHasBeenTaken = false;
		try
		{
			await _semaphore.WaitAsync();
			semaphoreHasBeenTaken = true;
			_taxCurrency.TryGetValue(contextGuid, out var value);
			if (value == null)
			{
				value = TaxCurrency.FromString(await GetAsync(contextGuid, "Acc_TaxCurrency", awaitOnSemaphore: false, token, tranName)) ?? new TaxCurrency();
				_taxCurrency.TryRemove("Acc_TaxCurrency", out var _);
				_taxCurrency.TryAdd("Acc_TaxCurrency", value);
			}
			return value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetTaxCurrencyAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 481);
			throw;
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_semaphore.Release();
			}
		}
	}

	public static async Task<TaxCustomInfo> GetTaxCustomInfoAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool semaphoreHasBeenTaken = false;
		try
		{
			await _semaphore.WaitAsync();
			semaphoreHasBeenTaken = true;
			_taxCustomInfo.TryGetValue(contextGuid, out var value);
			if (value == null)
			{
				value = TaxCustomInfo.FromString(await GetAsync(contextGuid, "Acc_TaxCustomInfo", awaitOnSemaphore: false, token, tranName)) ?? new TaxCustomInfo();
				_taxCustomInfo.TryRemove("Acc_TaxCustomInfo", out var _);
				_taxCustomInfo.TryAdd("Acc_TaxCustomInfo", value);
			}
			return value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetTaxCustomInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 509);
			throw;
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_semaphore.Release();
			}
		}
	}

	public static async Task<ChequeQrSetting> GetChequeQrListAsync(string contextGuid, bool loadDefaults, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool semaphoreHasBeenTaken = false;
		try
		{
			await _semaphore.WaitAsync();
			semaphoreHasBeenTaken = true;
			_chequeQrList.TryGetValue(contextGuid, out var ret);
			if (ret == null)
			{
				string text = await GetAsync(contextGuid, "Acc_ChequeQrSetting", awaitOnSemaphore: false, token, tranName);
				if (!string.IsNullOrEmpty(text))
				{
					ret = ChequeQrSetting.FromString(text);
				}
				if (!(ret?.Templates?.Any()).GetValueOrDefault() && loadDefaults)
				{
					ret = ChequeQrSetting.GetDefaults();
				}
				_chequeQrList.TryRemove("Acc_ChequeQrSetting", out var _);
				_chequeQrList.TryAdd("Acc_ChequeQrSetting", ret);
			}
			return ret;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetChequeQrListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 539);
			throw;
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_semaphore.Release();
			}
		}
	}

	public static async Task<ShabaNumberSetting> GetShabaListAsync(string contextGuid, bool loadDefaults, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool semaphoreHasBeenTaken = false;
		try
		{
			await _semaphore.WaitAsync();
			semaphoreHasBeenTaken = true;
			_shabaList.TryGetValue(contextGuid, out var ret);
			if (ret == null)
			{
				string text = await GetAsync(contextGuid, "Acc_ShabaNumberSetting", awaitOnSemaphore: false, token, tranName);
				if (!string.IsNullOrEmpty(text))
				{
					ret = ShabaNumberSetting.FromString(text);
				}
				if (!(ret?.Templates?.Any()).GetValueOrDefault() && loadDefaults)
				{
					ret = ShabaNumberSetting.GetDefaults();
				}
				_shabaList.TryRemove("Acc_ShabaNumberSetting", out var _);
				_shabaList.TryAdd("Acc_ShabaNumberSetting", ret);
			}
			return ret;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetShabaListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 569);
			throw;
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_semaphore.Release();
			}
		}
	}

	public static async Task<List<OccasionCalendarSmsRecord>> GetOccasionCalendarSmsAsync(string contextGuid, DateTime date, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			string text = await GetAsync(contextGuid, "OccasionCalendar_" + date.MiladiToCalture(), awaitOnSemaphore: false, token, tranName);
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}
			return text.FromJson<List<OccasionCalendarSmsRecord>>("GetOccasionCalendarSmsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 584);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetOccasionCalendarSmsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 588);
			throw;
		}
	}

	public static async Task<List<CouponDiscountSmsRecord>> GetCouponsSmsSettingAsync(string contextGuid, DateTime date, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			string text = await GetAsync(contextGuid, "CouponDiscount_" + date.MiladiToCalture(), awaitOnSemaphore: false, token, tranName);
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}
			return text.FromJson<List<CouponDiscountSmsRecord>>("GetCouponsSmsSettingAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 598);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetCouponsSmsSettingAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 602);
			throw;
		}
	}

	public static async Task<List<CouponDiscountReminderSmsRecord>> GetCouponsReminderSmsSettingAsync(string contextGuid, DateTime date, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			string text = await GetAsync(contextGuid, "CouponDiscountReminder_" + date.MiladiToCalture(), awaitOnSemaphore: false, token, tranName);
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}
			return text.FromJson<List<CouponDiscountReminderSmsRecord>>("GetCouponsReminderSmsSettingAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 612);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetCouponsReminderSmsSettingAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 616);
			throw;
		}
	}

	public static async Task<List<CouponDiscountSmsRecord>> GetCouponsSmsSettingAsync(string contextGuid, DateTime date, Guid couponGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			string text = await GetAsync(contextGuid, $"CouponDiscount_{date.MiladiToCalture()}_{couponGuid}", awaitOnSemaphore: false, token, tranName);
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}
			return text.FromJson<List<CouponDiscountSmsRecord>>("GetCouponsSmsSettingAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 626);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetCouponsSmsSettingAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 630);
			throw;
		}
	}

	public static async Task<ActivationAbonmanSettings> GetAbonmanSettingsAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			_abonmanSettingDic.TryGetValue(contextGuid, out var value);
			if (value == null)
			{
				value = ActivationAbonmanSettings.FromString(await GetAsync(contextGuid, "Acc_AbonmanSettings", awaitOnSemaphore: false, token, tranName));
				_abonmanSettingDic.TryRemove("Acc_AbonmanSettings", out var _);
				_abonmanSettingDic.TryAdd("Acc_AbonmanSettings", value);
			}
			return value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAbonmanSettingsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 653);
			throw;
		}
	}

	public static async Task<VersionUpdateSettings> GetVersionUpdateSettingsAsync(string contextGuid, CancellationToken token = default(CancellationToken), bool ignoreEmptySetting = false)
	{
		bool semaphoreHasBeenTaken = false;
		try
		{
			await _semaphore.WaitAsync();
			semaphoreHasBeenTaken = true;
			_versionUpdateSettings.TryGetValue(contextGuid, out var value);
			if (value == null)
			{
				value = VersionUpdateSettings.FromString(await GetAsync(contextGuid, "Acc_VersionUpdateSettings", awaitOnSemaphore: false, token));
			}
			value = value ?? throw new NullReferenceException("توقع میره پیش نیاد".GetTranslate());
			_versionUpdateSettings.TryRemove("Acc_VersionUpdateSettings", out var _);
			_versionUpdateSettings.TryAdd("Acc_VersionUpdateSettings", value);
			return value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetVersionUpdateSettingsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 682);
			throw;
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_semaphore.Release();
			}
		}
	}

	public static async Task<DeviceServiceSettings> GetDeviceServiceSettingsAsync(string contextGuid, CancellationToken token = default(CancellationToken), bool ignoreEmptySetting = false)
	{
		bool semaphoreHasBeenTaken = false;
		try
		{
			await _semaphore.WaitAsync();
			semaphoreHasBeenTaken = true;
			_deviceSettingDic.TryGetValue(contextGuid, out var value);
			if (value == null)
			{
				string text = await GetAsync(contextGuid, "Att_DeviceServiceSettings", awaitOnSemaphore: false, token);
				value = ((!(string.IsNullOrEmpty(text) && ignoreEmptySetting)) ? DeviceServiceSettings.FromString(text) : new DeviceServiceSettings());
			}
			value = value ?? throw new NullReferenceException("توقع میره پیش نیاد".GetTranslate());
			_deviceSettingDic.TryRemove("Att_DeviceServiceSettings", out var _);
			_deviceSettingDic.TryAdd("Att_DeviceServiceSettings", value);
			return value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetDeviceServiceSettingsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 712);
			throw;
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_semaphore.Release();
			}
		}
	}

	public static async Task<ReturnedSaveFuncInfo> SetAbonmanSettingsAsync(string contextGuid, ActivationAbonmanSettings settings, bool sendToServer = false)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		if (settings == null)
		{
			throw new NullReferenceException("settings");
		}
		bool semaphoreHasBeenTaken = false;
		try
		{
			await _semaphore.WaitAsync();
			semaphoreHasBeenTaken = true;
			_abonmanSettingDic.TryRemove(contextGuid, out var value);
			_abonmanSettingDic.TryAdd(contextGuid, settings);
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			string value2 = settings.ToString();
			bool sendToServer2 = sendToServer;
			returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(contextGuid, "Acc_AbonmanSettings", value2, awaitOnSemaphore: true, 10, default(CancellationToken), "", sendToServer2), ReturnedState.None, "SetAbonmanSettingsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 734);
			if (!ret.HasError)
			{
				_abonmanSettingDic.TryRemove(contextGuid, out value);
				_abonmanSettingDic.TryAdd(contextGuid, settings);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetAbonmanSettingsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 743);
			ret.AddReturnedValue(ex, "", "SetAbonmanSettingsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 744);
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_semaphore.Release();
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> SetOccasionCalendarSmsAsync(string contextGuid, DateTime date, List<OccasionCalendarSmsRecord> list)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (!(list?.Any() ?? false))
			{
				return res;
			}
			List<OccasionCalendarSmsRecord> list2 = await GetOccasionCalendarSmsAsync(contextGuid, date);
			if (list2 != null && list2.Any())
			{
				list.AddRange(list2);
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(contextGuid, "OccasionCalendar_" + date.MiladiToCalture(), Json.ToStringJson(list), awaitOnSemaphore: true, 10), ReturnedState.None, "SetOccasionCalendarSmsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 757);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetOccasionCalendarSmsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 761);
			res.AddReturnedValue(ex, "", "SetOccasionCalendarSmsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 762);
		}
		return res;
	}

	public static async Task<ReturnedSaveFuncInfo> SetCouponDiscountSmsSettingAsync(string contextGuid, DateTime date, List<CouponDiscountSmsRecord> list)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (!(list?.Any() ?? false))
			{
				return res;
			}
			List<CouponDiscountSmsRecord> list2 = await GetCouponsSmsSettingAsync(contextGuid, date);
			if (list2 != null && list2.Any())
			{
				list.AddRange(list2);
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(contextGuid, "CouponDiscount_" + date.MiladiToCalture(), Json.ToStringJson(list), awaitOnSemaphore: true, 10), ReturnedState.None, "SetCouponDiscountSmsSettingAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 774);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetCouponDiscountSmsSettingAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 778);
			res.AddReturnedValue(ex, "", "SetCouponDiscountSmsSettingAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 779);
		}
		return res;
	}

	public static async Task<ReturnedSaveFuncInfo> SetCouponDiscountReminderSmsSettingAsync(string contextGuid, DateTime date, List<CouponDiscountReminderSmsRecord> list)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (!(list?.Any() ?? false))
			{
				return res;
			}
			List<CouponDiscountReminderSmsRecord> list2 = await GetCouponsReminderSmsSettingAsync(contextGuid, date);
			if (list2 != null && list2.Any())
			{
				list.AddRange(list2);
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(contextGuid, "CouponDiscountReminder_" + date.MiladiToCalture(), Json.ToStringJson(list), awaitOnSemaphore: true, 10), ReturnedState.None, "SetCouponDiscountReminderSmsSettingAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 791);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetCouponDiscountReminderSmsSettingAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 795);
			res.AddReturnedValue(ex, "", "SetCouponDiscountReminderSmsSettingAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 796);
		}
		return res;
	}

	public static async Task<ReturnedSaveFuncInfo> SetCouponDiscountSmsSettingAsync(string contextGuid, DateTime date, Guid couponGuid, List<CouponDiscountSmsRecord> list)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (!(list?.Any() ?? false))
			{
				return res;
			}
			List<CouponDiscountSmsRecord> list2 = await GetCouponsSmsSettingAsync(contextGuid, date, couponGuid);
			if (list2 != null && list2.Any())
			{
				list.AddRange(list2);
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(contextGuid, $"CouponDiscount_{date.MiladiToCalture()}_{couponGuid}", Json.ToStringJson(list), awaitOnSemaphore: true, 10), ReturnedState.None, "SetCouponDiscountSmsSettingAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 808);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetCouponDiscountSmsSettingAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 812);
			res.AddReturnedValue(ex, "", "SetCouponDiscountSmsSettingAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 813);
		}
		return res;
	}

	public static async Task<ReturnedSaveFuncInfo> SetKioskSettingsAsync(string contextGuid, KioskSettings settings)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		if (settings == null)
		{
			throw new NullReferenceException("settings");
		}
		bool semaphoreHasBeenTaken = false;
		try
		{
			await _semaphore.WaitAsync();
			semaphoreHasBeenTaken = true;
			_kioskSettinhs.TryRemove(contextGuid, out var _);
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(contextGuid, "Acc_KioskSettings", settings.ToString(), awaitOnSemaphore: true, 10), ReturnedState.None, "SetKioskSettingsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 827);
			if (!ret.HasError)
			{
				_kioskSettinhs.TryAdd(contextGuid, settings);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetKioskSettingsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 832);
			ret.AddReturnedValue(ex, "", "SetKioskSettingsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 833);
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_semaphore.Release();
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> SetScaleSettingsAsync(string contextGuid, ScaleSettings settings)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		if (settings == null)
		{
			throw new NullReferenceException("settings");
		}
		bool semaphoreHasBeenTaken = false;
		checked
		{
			try
			{
				await _semaphore.WaitAsync();
				semaphoreHasBeenTaken = true;
				_scaleSettings.TryRemove(contextGuid, out var value);
				if (value != null && value.Version > settings.Version)
				{
					throw new InvalidOperationException($"settings are rooling back!!! from version({value.Version})=>({settings.Version})");
				}
				string text = await ClsCache.GetCache(contextGuid).CacheRepository.ILocalSetting.GetAsync("Acc_ScaleSettings", default(CancellationToken), "");
				if (!string.IsNullOrEmpty(text))
				{
					value = text.FromJson<ScaleSettings>(sendErrorIfException: true, "SetScaleSettingsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 857);
					if (value != null && value.Version > settings.Version)
					{
						throw new InvalidOperationException("setting on database has been loaded!\r\n" + $"settings are rooling back!!! from version({value.Version})=>({settings.Version})");
					}
				}
				settings.Version++;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(contextGuid, "Acc_ScaleSettings", settings.ToString(), awaitOnSemaphore: true, 10), ReturnedState.None, "SetScaleSettingsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 864);
				if (!ret.HasError)
				{
					_scaleSettings.TryAdd(contextGuid, settings);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetScaleSettingsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 870);
				ret.AddReturnedValue(ex, "", "SetScaleSettingsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 871);
			}
			finally
			{
				if (semaphoreHasBeenTaken)
				{
					_semaphore.Release();
				}
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> SetBazsaziResultAsync(string contextGuid, BazsaziResultModel settings)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		if (settings == null)
		{
			throw new NullReferenceException("settings");
		}
		bool semaphoreHasBeenTaken = false;
		try
		{
			await _semaphore.WaitAsync();
			semaphoreHasBeenTaken = true;
			_bazsaziResultModel.TryRemove(contextGuid, out var _);
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(contextGuid, BazsaziResultModel.KeyName, settings.ToString(), awaitOnSemaphore: true, 10), ReturnedState.None, "SetBazsaziResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 886);
			if (!ret.HasError)
			{
				_bazsaziResultModel.TryAdd(contextGuid, settings);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetBazsaziResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 892);
			ret.AddReturnedValue(ex, "", "SetBazsaziResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 893);
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_semaphore.Release();
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> SetTaxUnitsAsync(string contextGuid, TaxUnits settings)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		if (settings == null)
		{
			throw new NullReferenceException("settings");
		}
		bool semaphoreHasBeenTaken = false;
		try
		{
			await _semaphore.WaitAsync();
			semaphoreHasBeenTaken = true;
			_taxUnits.TryRemove(contextGuid, out var _);
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(contextGuid, "Acc_TaxUnits", settings.ToString(), awaitOnSemaphore: true, 10), ReturnedState.None, "SetTaxUnitsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 908);
			if (!ret.HasError)
			{
				_taxUnits.TryAdd(contextGuid, settings);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetTaxUnitsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 914);
			ret.AddReturnedValue(ex, "", "SetTaxUnitsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 915);
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_semaphore.Release();
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> SetTaxErrorsAsync(string contextGuid, TaxErrors settings)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		if (settings == null)
		{
			throw new NullReferenceException("settings");
		}
		bool semaphoreHasBeenTaken = false;
		try
		{
			await _semaphore.WaitAsync();
			semaphoreHasBeenTaken = true;
			_taxErrors.TryRemove(contextGuid, out var _);
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(contextGuid, "Acc_TaxErrors", settings.ToString(), awaitOnSemaphore: true, 10), ReturnedState.None, "SetTaxErrorsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 930);
			if (!ret.HasError)
			{
				_taxErrors.TryAdd(contextGuid, settings);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetTaxErrorsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 936);
			ret.AddReturnedValue(ex, "", "SetTaxErrorsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 937);
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_semaphore.Release();
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> SetTaxCurrencySettingsAsync(string contextGuid, TaxCurrency settings)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		if (settings == null)
		{
			throw new NullReferenceException("settings");
		}
		bool semaphoreHasBeenTaken = false;
		try
		{
			await _semaphore.WaitAsync();
			semaphoreHasBeenTaken = true;
			_taxCurrency.TryRemove(contextGuid, out var _);
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(contextGuid, "Acc_TaxCurrency", settings.ToString(), awaitOnSemaphore: true, 10), ReturnedState.None, "SetTaxCurrencySettingsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 952);
			if (!ret.HasError)
			{
				_taxCurrency.TryAdd(contextGuid, settings);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetTaxCurrencySettingsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 957);
			ret.AddReturnedValue(ex, "", "SetTaxCurrencySettingsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 958);
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_semaphore.Release();
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> SetTaxCustomInfoSettingsAsync(string contextGuid, TaxCustomInfo settings)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		if (settings == null)
		{
			throw new NullReferenceException("settings");
		}
		bool semaphoreHasBeenTaken = false;
		try
		{
			await _semaphore.WaitAsync();
			semaphoreHasBeenTaken = true;
			_taxCustomInfo.TryRemove(contextGuid, out var _);
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(contextGuid, "Acc_TaxCustomInfo", settings.ToString(), awaitOnSemaphore: true, 10), ReturnedState.None, "SetTaxCustomInfoSettingsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 977);
			if (!ret.HasError)
			{
				_taxCustomInfo.TryAdd(contextGuid, settings);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetTaxCustomInfoSettingsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 982);
			ret.AddReturnedValue(ex, "", "SetTaxCustomInfoSettingsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 983);
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_semaphore.Release();
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> SetChequeQrSettingsAsync(string contextGuid, ChequeQrSetting settings)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		if (settings == null)
		{
			throw new NullReferenceException("settings");
		}
		bool semaphoreHasBeenTaken = false;
		try
		{
			await _semaphore.WaitAsync();
			semaphoreHasBeenTaken = true;
			_chequeQrList.TryRemove(contextGuid, out var _);
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(contextGuid, "Acc_ChequeQrSetting", settings.ToString(), awaitOnSemaphore: true, 10), ReturnedState.None, "SetChequeQrSettingsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1002);
			if (!ret.HasError)
			{
				_chequeQrList.TryAdd(contextGuid, settings);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetChequeQrSettingsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1007);
			ret.AddReturnedValue(ex, "", "SetChequeQrSettingsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1008);
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_semaphore.Release();
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> SetShabaSettingsAsync(string contextGuid, ShabaNumberSetting settings)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		if (settings == null)
		{
			throw new NullReferenceException("settings");
		}
		bool semaphoreHasBeenTaken = false;
		try
		{
			await _semaphore.WaitAsync();
			semaphoreHasBeenTaken = true;
			_shabaList.TryRemove(contextGuid, out var _);
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(contextGuid, "Acc_ShabaNumberSetting", settings.ToString(), awaitOnSemaphore: true, 10), ReturnedState.None, "SetShabaSettingsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1027);
			if (!ret.HasError)
			{
				_shabaList.TryAdd(contextGuid, settings);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetShabaSettingsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1032);
			ret.AddReturnedValue(ex, "", "SetShabaSettingsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1033);
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_semaphore.Release();
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> SetSettingAsync(string contextGuid, AccGlobalSettings value, bool send2Server = true, CancellationToken token = default(CancellationToken))
	{
		if (value == null)
		{
			throw new NullReferenceException("value");
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool semaphoreHasBeenTaken = false;
		try
		{
			await _semaphore.WaitAsync();
			semaphoreHasBeenTaken = true;
			LocalSettingBusiness obj = new LocalSettingBusiness(ClsCache.GetCache(contextGuid).ContextGuid)
			{
				Key = AccGlobalSettings.GetKeyName(""),
				Value = value.ToString()
			};
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await obj.SaveAsync(token, awaitOnSemaphore: true, 10), ReturnedState.None, "SetSettingAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1056);
			_dic.TryRemove(AccGlobalSettings.GetKeyName(contextGuid), out var _);
			_dic.TryAdd(AccGlobalSettings.GetKeyName(contextGuid), value);
			if (!ret.HasError)
			{
				await ClsCache.GetCache(contextGuid).SetMultiLanguageSettingsAsync();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetSettingAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1063);
			ret.AddReturnedValue(ex, "", "SetSettingAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1064);
		}
		finally
		{
			if (!ret.HasError && send2Server)
			{
				Send2ServerAsync(contextGuid, value, token);
			}
			if (semaphoreHasBeenTaken)
			{
				_semaphore.Release();
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> SetVersionUpdateSettingsAsync(string contextGuid, VersionUpdateSettings settings)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		if (settings == null)
		{
			throw new NullReferenceException("settings");
		}
		bool semaphoreHasBeenTaken = false;
		try
		{
			await _semaphore.WaitAsync();
			semaphoreHasBeenTaken = true;
			_versionUpdateSettings.TryRemove(contextGuid, out var _);
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(contextGuid, "Acc_VersionUpdateSettings", settings.ToString(), awaitOnSemaphore: true, 10), ReturnedState.None, "SetVersionUpdateSettingsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1084);
			if (!ret.HasError)
			{
				_versionUpdateSettings.TryAdd(contextGuid, settings);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetVersionUpdateSettingsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1090);
			ret.AddReturnedValue(ex, "", "SetVersionUpdateSettingsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1091);
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_semaphore.Release();
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> SetDeviceServiceSettingsAsync(string contextGuid, DeviceServiceSettings settings)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		if (settings == null)
		{
			throw new NullReferenceException("settings");
		}
		bool semaphoreHasBeenTaken = false;
		try
		{
			await _semaphore.WaitAsync();
			semaphoreHasBeenTaken = true;
			_deviceSettingDic.TryRemove(contextGuid, out var _);
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(contextGuid, "Att_DeviceServiceSettings", settings.ToString(), awaitOnSemaphore: true, 10), ReturnedState.None, "SetDeviceServiceSettingsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1106);
			if (!ret.HasError)
			{
				_deviceSettingDic.TryAdd(contextGuid, settings);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetDeviceServiceSettingsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1112);
			ret.AddReturnedValue(ex, "", "SetDeviceServiceSettingsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1113);
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_semaphore.Release();
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> SetWebServiceSettingsAsync(string contextGuid, NPSMS_WebServiceSettings value, CancellationToken token = default(CancellationToken))
	{
		if (value == null)
		{
			throw new NullReferenceException("value");
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool semaphoreHasBeenTaken = false;
		try
		{
			await _semaphore.WaitAsync();
			semaphoreHasBeenTaken = true;
			_NPSMSsettings.TryRemove(contextGuid, out var _);
			_NPSMSsettings.TryAdd(contextGuid, value);
			LocalSettingBusiness obj = new LocalSettingBusiness(contextGuid)
			{
				Key = "AccNPSMS_WebServiceSettings",
				Value = value.ToString()
			};
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await obj.SaveAsync(token, awaitOnSemaphore: true, 10), ReturnedState.None, "SetWebServiceSettingsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1138);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetWebServiceSettingsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1142);
			ret.AddReturnedValue(ex, "", "SetWebServiceSettingsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1143);
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_semaphore.Release();
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> RemoveAsync(string contextGuid, string key, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool semaphoreHasBeenTaken = false;
		bool transactionHasBeenCreated = false;
		try
		{
			_ = 2;
			try
			{
				await _semaphore.WaitAsync();
				semaphoreHasBeenTaken = true;
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1159), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1159);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.ILocalSetting.RemoveAsync(key, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1161);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1165);
				ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1166);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1171), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1171);
			}
			if (semaphoreHasBeenTaken)
			{
				_semaphore.Release();
			}
		}
		return ret;
	}

	public static Task<IEnumerable<LocalSettingBusiness>> GetAllAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ILocalSetting.GetAllAsync(token, tranName);
	}

	public static async Task<bool> GetBoolValueAsync(string contextGuid, string propertyName, bool def = true, CancellationToken token = default(CancellationToken))
	{
		try
		{
			string text = await GetAsync(contextGuid, propertyName, awaitOnSemaphore: true, token);
			if (text == null || text.Trim() == "")
			{
				return def;
			}
			return text.ParseToBoolean();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetBoolValueAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1188);
			throw;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> SaveAsync(string contextGuid, string name, string value, bool awaitOnSemaphore = true, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "", bool sendToServer = false)
	{
		int line = 0;
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
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "LocalSettingBusiness/SaveAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1202);
					}
					else
					{
						line = 100;
						ClsCache clsCache = ClsCache.GetCache(contextGuid) ?? throw new NullReferenceException("Cache is null!!");
						line = 101;
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await clsCache.GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1206), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1206);
						}
						transactionHasBeenCreated = true;
						line = 2;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.ILocalSetting.SaveAsync(name, value, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1209);
						line = 3;
						if (!ret.HasError)
						{
							goto end_IL_012a;
						}
						result = ret;
					}
					goto end_IL_008c;
					end_IL_012a:;
				}
				goto end_IL_007b;
				end_IL_008c:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}//ClsCache.GetCache(contextGuid).DatabaseManager is null:{ClsCache.GetCache(contextGuid)?.DatabaseManager == null}//SaveAsync(string name, string value, short tryCount = 10, string tranName)", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1215);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1216);
				goto end_IL_007b;
			}
			return result;
			end_IL_007b:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1221), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1221);
			}
			if (sendToServer)
			{
				Send2ServerAsync(contextGuid, await GetSettingAsync(contextGuid), token);
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> Send2ServerAsync(string contextGuid, AccGlobalSettings setting, CancellationToken token)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool semaphoreHasBeenTaken = false;
		try
		{
			await _semaphoreSend2Server.WaitAsync();
			semaphoreHasBeenTaken = true;
			ClsCache cache = ClsCache.GetCache(contextGuid);
			if (cache == null || !cache.BufferEnable || setting.Date_Synced > setting.Date_Modified || token.IsCancellationRequested)
			{
				return ret;
			}
			if (cache.Source == ENSource.Accounting)
			{
				await Send2ServerAccountingAsync(contextGuid, setting, cache, token);
			}
			if (cache.Source == ENSource.Baje)
			{
				await Send2ServerDepartmentAsync(contextGuid, cache, token);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1249);
			ret.AddReturnedValue(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1250);
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_semaphoreSend2Server.Release();
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> Send2ServerAccountingAsync(string contextGuid, AccGlobalSettings setting, ClsCache cache, CancellationToken token)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			List<Packet> lstPacket = WebApplicationSetting.GenerateSaveRequest(new List<WebApplicationSetting>
			{
				new WebApplicationSetting
				{
					Name = AccGlobalSettings.GetKeyName(""),
					Val = setting.ToString(),
					Modified = setting.Date_Modified
				}
			});
			if (token.IsCancellationRequested)
			{
				return res.AddErrorTokenIsCancelled("", "", "Send2ServerAccountingAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1269);
			}
			using SettingWaiter waiter = new SettingWaiter(contextGuid, AccGlobalSettings.GetKeyName(""));
			cache.SendData(lstPacket, Priority.High);
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await waiter.WaitForSaveResponse(), ReturnedState.None, "Send2ServerAccountingAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1272);
			if (token.IsCancellationRequested || res.HasError)
			{
				return res;
			}
			setting.Date_Synced = DateTime.Now;
			await setting.SaveAsync(contextGuid, awaitOnSemaphore: true, 10, token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Send2ServerAccountingAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1278);
		}
		return res;
	}

	public static async Task<ReturnedSaveFuncInfo> Send2ServerDepartmentAsync(string contextGuid, ClsCache cache, CancellationToken token)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			ActivationAbonmanSettings activationAbonmanSettings = await GetAbonmanSettingsAsync(contextGuid);
			if (activationAbonmanSettings == null)
			{
				return res;
			}
			List<Packet> lstPacket = WebApplicationSetting.GenerateSaveRequest(new List<WebApplicationSetting>
			{
				new WebApplicationSetting
				{
					Name = "Acc_AbonmanSettings",
					Val = activationAbonmanSettings.ToString(),
					Modified = activationAbonmanSettings.Modified
				}
			});
			if (token.IsCancellationRequested)
			{
				return res.AddErrorTokenIsCancelled("", "", "Send2ServerDepartmentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1297);
			}
			using SettingWaiter waiter = new SettingWaiter(contextGuid, "Acc_AbonmanSettings");
			cache.SendData(lstPacket, Priority.High);
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await waiter.WaitForSaveResponse(), ReturnedState.None, "Send2ServerDepartmentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1300);
		}
		catch (Exception ex)
		{
			res.AddReturnedValue(ex, "", "Send2ServerDepartmentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1304);
			WebErrorLog.Instance.StartLog(ex, "", "Send2ServerDepartmentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1305);
		}
		return res;
	}

	private static string GenerateLastUnitKey(Guid cashierGuid, Document type)
	{
		return $"{cashierGuid}_OrderType{(short)type}";
	}

	private static string GenerateProductSearchKey(Guid cashierGuid)
	{
		return $"{cashierGuid}_PRD002U3SF";
	}

	private static string GenerateCustomerSearchKey(Guid cashierGuid)
	{
		return $"{cashierGuid}_PRD002U3SS";
	}

	public static async Task<Guid> GetLastUnitGuidAsync(string contextGuid, Guid cashierGuid, Document type, Guid productGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("GetLastUnitGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs"))
		{
			try
			{
				if (cashierGuid == Guid.Empty || productGuid == Guid.Empty)
				{
					return Guid.Empty;
				}
				string key = GenerateLastUnitKey(cashierGuid, type);
				if (!(_lastProductUnit?.Any() ?? false))
				{
					string text = await GetAsync(contextGuid, key, awaitOnSemaphore: false, token, tranName);
					if (string.IsNullOrEmpty(text))
					{
						_lastProductUnit = new ConcurrentDictionary<Guid, Guid>();
					}
					else
					{
						_lastProductUnit = text.FromJson<ConcurrentDictionary<Guid, Guid>>("GetLastUnitGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1323);
					}
				}
				if (_lastProductUnit.TryGetValue(productGuid, out var value))
				{
					return value;
				}
				return Guid.Empty;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetLastUnitGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1330);
				return Guid.Empty;
			}
		}
	}

	public static async Task<ReturnedSaveFuncInfo> SetLastUnitGuidAsync(string contextGuid, Guid cashierGuid, Document type, Dictionary<Guid, Guid> dic, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("SetLastUnitGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			try
			{
				if (cashierGuid == Guid.Empty || !(dic?.Any() ?? false))
				{
					return res;
				}
				string name = GenerateLastUnitKey(cashierGuid, type);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(contextGuid, name, Json.ToStringJson(dic), awaitOnSemaphore: true, 10, token), ReturnedState.None, "SetLastUnitGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1342);
				_lastProductUnit = null;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetLastUnitGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1347);
				res.AddReturnedValue(ex, "", "SetLastUnitGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1348);
			}
			return res;
		}
	}

	public static async Task<ProductSearchSettingRecord> GetProductSerachOptionAsync(string contextGuid, Guid cashierGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("GetProductSerachOptionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs"))
		{
			try
			{
				if (cashierGuid == Guid.Empty)
				{
					return null;
				}
				string key = GenerateProductSearchKey(cashierGuid);
				if (!(_productSearchDic?.Any() ?? false))
				{
					string text = await GetAsync(contextGuid, key, awaitOnSemaphore: false, token, tranName);
					_productSearchDic = new ConcurrentDictionary<Guid, ProductSearchSettingRecord>();
					if (!string.IsNullOrEmpty(text))
					{
						ProductSearchSettingRecord value = text.FromJson<ProductSearchSettingRecord>("GetProductSerachOptionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1365);
						_productSearchDic.TryAdd(cashierGuid, value);
					}
				}
				if (_productSearchDic.TryGetValue(cashierGuid, out var value2))
				{
					return value2;
				}
				return null;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetProductSerachOptionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1374);
				return null;
			}
		}
	}

	public static async Task<ReturnedSaveFuncInfo> SetProductSerachOptionAsync(string contextGuid, Guid cashierGuid, ProductSearchSettingRecord item, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("SetProductSerachOptionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			try
			{
				if (cashierGuid == Guid.Empty || item == null)
				{
					return res;
				}
				string name = GenerateProductSearchKey(cashierGuid);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(contextGuid, name, Json.ToStringJson(item), awaitOnSemaphore: true, 10, token), ReturnedState.None, "SetProductSerachOptionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1386);
				_productSearchDic = null;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetProductSerachOptionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1391);
				res.AddReturnedValue(ex, "", "SetProductSerachOptionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1392);
			}
			return res;
		}
	}

	public static async Task<CustomerSearchSettingRecord> GetCustomerSerachOptionAsync(string contextGuid, Guid cashierGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("GetCustomerSerachOptionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs"))
		{
			try
			{
				if (cashierGuid == Guid.Empty)
				{
					return null;
				}
				string key = GenerateCustomerSearchKey(cashierGuid);
				if (!(_customerSearchDic?.Any() ?? false))
				{
					string text = await GetAsync(contextGuid, key, awaitOnSemaphore: false, token, tranName);
					_customerSearchDic = new ConcurrentDictionary<Guid, CustomerSearchSettingRecord>();
					if (!string.IsNullOrEmpty(text))
					{
						CustomerSearchSettingRecord value = text.FromJson<CustomerSearchSettingRecord>("GetCustomerSerachOptionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1409);
						_customerSearchDic.TryAdd(cashierGuid, value);
					}
				}
				if (_customerSearchDic.TryGetValue(cashierGuid, out var value2))
				{
					return value2;
				}
				return null;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetCustomerSerachOptionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1418);
				return null;
			}
		}
	}

	public static async Task<ReturnedSaveFuncInfo> SetCustomerSerachOptionAsync(string contextGuid, Guid cashierGuid, CustomerSearchSettingRecord item, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("SetCustomerSerachOptionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			try
			{
				if (cashierGuid == Guid.Empty || item == null)
				{
					return res;
				}
				string name = GenerateCustomerSearchKey(cashierGuid);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(contextGuid, name, Json.ToStringJson(item), awaitOnSemaphore: true, 10, token), ReturnedState.None, "SetCustomerSerachOptionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1430);
				_customerSearchDic = null;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetCustomerSerachOptionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1435);
				res.AddReturnedValue(ex, "", "SetCustomerSerachOptionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1436);
			}
			return res;
		}
	}

	public static async Task<TaxEconomicCodeRecord> GetTaxEconomicCodeRecordAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("GetTaxEconomicCodeRecordAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs"))
		{
			try
			{
				if (!(_taxEcoCodeDic?.Any() ?? false))
				{
					string text = await GetAsync(contextGuid, TaxEconomicCodeRecord.KeyName, awaitOnSemaphore: false, token, tranName);
					_taxEcoCodeDic = new ConcurrentDictionary<string, TaxEconomicCodeRecord>();
					if (!string.IsNullOrEmpty(text))
					{
						TaxEconomicCodeRecord value = text.FromJson<TaxEconomicCodeRecord>("GetTaxEconomicCodeRecordAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1451);
						_taxEcoCodeDic.TryAdd(contextGuid, value);
					}
				}
				if (_taxEcoCodeDic.TryGetValue(contextGuid, out var value2))
				{
					return value2;
				}
				return null;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetTaxEconomicCodeRecordAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1460);
				return null;
			}
		}
	}

	public static async Task<ReturnedSaveFuncInfo> SetTaxEconomicCodeRecordAsync(string contextGuid, TaxEconomicCodeRecord item, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("SetTaxEconomicCodeRecordAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(contextGuid, TaxEconomicCodeRecord.KeyName, Json.ToStringJson(item), awaitOnSemaphore: true, 10, token), ReturnedState.None, "SetTaxEconomicCodeRecordAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1470);
				_taxEcoCodeDic = null;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetTaxEconomicCodeRecordAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1475);
				res.AddReturnedValue(ex, "", "SetTaxEconomicCodeRecordAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LocalSettingBusiness.cs", 1476);
			}
			return res;
		}
	}
}
