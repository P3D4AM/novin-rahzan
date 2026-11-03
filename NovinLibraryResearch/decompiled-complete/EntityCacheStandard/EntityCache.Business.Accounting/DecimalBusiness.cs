using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ClientHub.AccountingHubs;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.GoldAndCurrency;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;
using PacketParser.EventArgs;

namespace EntityCache.Business.Accounting;

public class DecimalBusiness : IDecimal, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private CancellationTokenSource _saveToken;

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now;


	public string Name { get; set; }

	public string Country { get; set; }

	public string NameForDisplay
	{
		get
		{
			if (string.IsNullOrEmpty(Country))
			{
				return Name;
			}
			return Name + " (" + Country + ")";
		}
	}

	public string Description { get; set; }

	public bool Status { get; set; }

	public string Abstract { get; set; }

	public DateTime LastUpdate { get; set; }

	public string LastUpdateSh => LastUpdate.MiladiToCalture();

	public string Time => LastUpdate.Hour + ":" + LastUpdate.Minute + ":" + ((LastUpdate.Second < 10) ? $"0{LastUpdate.Second}" : ((object)LastUpdate.Second));

	public decimal LastPrice { get; set; }

	public short IconIndex { get; set; }

	public string NameCountry => Name ?? ("-" + Country);

	public string ContextGuid { get; set; }

	public string Symbol { get; set; }

	public bool IsChecked { get; set; }

	[JsonIgnore]
	[XmlIgnore]
	public DecimalBusiness EventManager => ClsCache.GetCache(ContextGuid).BusinessAggregator.DecimalBusiness;

	public event Func<object, EventArgs, Task> OnItemSaved;

	public DecimalBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, CancellationToken token, bool awaitOnSemaphore = true, short tryCount = 10, string tranName = "", short autoFixTryCount = 0)
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
			_ = 5;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "DecimalBusiness/SaveAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DecimalBusiness.cs", 58);
					}
					else
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DecimalBusiness.cs", 59), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DecimalBusiness.cs", 59);
						}
						transactionHasBeenCreated = true;
						Modified = DateTime.Now;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(system, autoFixTryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DecimalBusiness.cs", 62);
						ret.throwExceptionIfError();
						DecimalBusiness oldData = await ClsCache.GetCache(ContextGuid).CacheRepository.DecimalRepository.GetFromDatabaseAsync(Guid, token, tranName);
						if (oldData != null && (oldData.Name != Name || oldData.Country != Country || oldData.LastPrice != LastPrice || oldData.Abstract != Abstract))
						{
							LastUpdate = DateTime.Now;
						}
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.DecimalRepository.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DecimalBusiness.cs", 71);
						if (ret.HasError || system == EnSystem.Transfer)
						{
							result = ret;
						}
						else
						{
							decimal? num = oldData?.LastPrice;
							decimal lastPrice = LastPrice;
							if (!((num.GetValueOrDefault() == lastPrice) & num.HasValue))
							{
								DecimalUpdatesBusiness obj = new DecimalUpdatesBusiness(ContextGuid)
								{
									DateM = DateTime.Now,
									DecimalGuid = Guid,
									Guid = Guid.NewGuid(),
									Modified = DateTime.Now,
									Price = LastPrice,
									Status = true
								};
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await obj.SaveAsync(token, awaitOnSemaphore, tryCount, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DecimalBusiness.cs", 83);
								goto end_IL_0129;
							}
							result = ret;
						}
					}
					goto end_IL_007d;
					end_IL_0129:;
				}
				goto end_IL_006c;
				end_IL_007d:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DecimalBusiness.cs", 87);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DecimalBusiness.cs", 88);
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
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DecimalBusiness.cs", 93), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DecimalBusiness.cs", 93);
			}
		}
		return ret;
	}

	public static Task<DecimalBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.DecimalRepository.GetAsync(guid, token, tranName);
	}

	public static Task<IEnumerable<DecimalBusiness>> GetAllAsync(string contextGuid, bool? status = true, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.DecimalRepository.GetAllAsync(status, token, tranName);
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(EnSystem system, CancellationToken token, IEnumerable<DecimalBusiness> lst, short tryCount = 10, string tranName = "", Action<PercenteCompletedEventArgs> percentCompleted = null)
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
			foreach (DecimalBusiness item in lst)
			{
				current = checked(current + 1);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DecimalBusiness.cs", 112);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(system, token, awaitOnSemaphore: true, tryCount, tranName, 10), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DecimalBusiness.cs", 113);
				if (percentCompleted != null)
				{
					invokeToken2?.Cancel();
					invokeToken2 = new CancellationTokenSource(60000);
					CancellationToken token2 = invokeToken2.Token;
					percentCompleted(new PercenteCompletedEventArgs(lst.Count(), current, item.Name, token2));
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DecimalBusiness.cs", 125);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DecimalBusiness.cs", 126);
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

	private async Task<ReturnedSaveFuncInfo> CheckValidationAsync(EnSystem system, short autoFixTryCount, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		checked
		{
			try
			{
				if (string.IsNullOrEmpty(Name))
				{
					ret.AddError(system, "نام ارز پر نشده".GetTranslate(), "Name", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DecimalBusiness.cs", 145);
				}
				if (Modified > DateTime.Now)
				{
					ret.AddError(system, "زمان آخرین تغییر".GetTranslate() + $"({Modified})" + " از زمان فعلی".GetTranslate() + $"({DateTime.Now})" + " بزرگتر است".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DecimalBusiness.cs", 146);
				}
				if (Modified.Year < DateTime.Now.Year - 10)
				{
					ret.AddError(system, "زمان آخرین تغییر".GetTranslate() + $"({Modified})" + " نامعتبر است".GetTranslate(), "Modified", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DecimalBusiness.cs", 147);
				}
				if (autoFixTryCount > 0 && system != EnSystem.UserInterface)
				{
					DecimalBusiness decimalBusiness = await GetAsync(ContextGuid, Name, Country, token, tranName);
					if (decimalBusiness != null && decimalBusiness.Guid != Guid)
					{
						Name += "_";
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						DecimalBusiness decimalBusiness2 = this;
						short num = (short)(autoFixTryCount - 1);
						autoFixTryCount = num;
						returnedSaveFuncInfo.AddReturnedValue(await decimalBusiness2.CheckValidationAsync(system, num, token, tranName), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DecimalBusiness.cs", 155);
						return ret;
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DecimalBusiness.cs", 162);
				ret.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DecimalBusiness.cs", 163);
			}
			return ret;
		}
	}

	public static Task<DecimalBusiness> GetAsync(string contextGuid, string name, string country, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.DecimalRepository.GetAsync(name, country, token, tranName);
	}

	public static Task<DecimalBusiness> GetAsync(string contextGuid, string slug, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.DecimalRepository.GetAsync(slug, token, tranName);
	}

	public static Task<DecimalBusiness> GenerateNewWithDefaultsAsync(string contextGuid, string description = "", Guid? guid = null)
	{
		try
		{
			DecimalBusiness decimalBusiness = new DecimalBusiness(contextGuid);
			if (!string.IsNullOrEmpty(description))
			{
				decimalBusiness.Description = description.FixString();
			}
			if ((guid ?? Guid.Empty) != Guid.Empty)
			{
				decimalBusiness.Guid = guid.Value;
			}
			else
			{
				decimalBusiness.Guid = Guid.NewGuid();
			}
			return Task.FromResult(decimalBusiness);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateNewWithDefaultsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DecimalBusiness.cs", 183);
			throw;
		}
	}

	public void HandleServerEvent()
	{
		try
		{
			GoldAndCurrencyHubInstances instance = GoldAndCurrencyHubInstances.GetInstance(ContextGuid);
			if (instance?.GoldAndCurrency != null)
			{
				instance.GoldAndCurrency.OnLogin += GoldAndCurrency_OnLogin;
				instance.GoldAndCurrency.DataRecieved += GoldAndCurrency_DataRecieved;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "HandleServerEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DecimalBusiness.cs", 196);
		}
	}

	private Task GoldAndCurrency_DataRecieved(List<WebGoldAndCurrencyDetail> e)
	{
		return SaveFromServerAsync(e?.Select((WebGoldAndCurrencyDetail q) => new GoldAndCurrencyRecord
		{
			Namad = (q?.Namad ?? ""),
			Price = (q?.Price ?? 0m),
			DateM = (q?.InsertDate ?? DateTime.Now)
		}));
	}

	private Task GoldAndCurrency_OnLogin(List<WebGoldAndCurrencyReport> e)
	{
		return SaveFromServerAsync(e?.Select((WebGoldAndCurrencyReport q) => new GoldAndCurrencyRecord
		{
			Namad = (q?.Namad ?? ""),
			Price = (q?.Price ?? 0m),
			DateM = (q?.InsertDate ?? DateTime.Now)
		}));
	}

	private async Task<ReturnedSaveFuncInfo> SaveFromServerAsync(IEnumerable<GoldAndCurrencyRecord> data)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			_saveToken?.Cancel();
			_saveToken = new CancellationTokenSource(60000);
			CancellationToken token = _saveToken.Token;
			await Task.Delay(5000);
			if (token.IsCancellationRequested)
			{
				return res;
			}
			if (!(data?.Any() ?? false) || ClsCache.GetCache(ContextGuid)?.CacheRepository?.DecimalRepository == null || token.IsCancellationRequested)
			{
				return res;
			}
			IEnumerable<DecimalBusiness> enumerable = (await GetAllAsync(ContextGuid, true))?.Where((DecimalBusiness q) => !string.IsNullOrEmpty(q.Symbol));
			if (enumerable == null || !enumerable.Any() || token.IsCancellationRequested)
			{
				return res;
			}
			foreach (DecimalBusiness dbItem in enumerable)
			{
				if (token.IsCancellationRequested)
				{
					return res;
				}
				GoldAndCurrencyRecord goldAndCurrencyRecord = data.FirstOrDefault((GoldAndCurrencyRecord q) => q.Namad == dbItem.Symbol);
				if (!(goldAndCurrencyRecord == null) && !(goldAndCurrencyRecord.Price == dbItem.LastPrice))
				{
					dbItem.LastPrice = goldAndCurrencyRecord.Price;
					dbItem.LastUpdate = goldAndCurrencyRecord.DateM;
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await dbItem.SaveAsync(EnSystem.SignalR, default(CancellationToken), awaitOnSemaphore: true, 10, "", 0), ReturnedState.None, "SaveFromServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DecimalBusiness.cs", 233);
				}
			}
			if (EventManager != null)
			{
				PacketParser.Utilities.RaiseEvent<EventArgs>(EventManager.OnItemSaved, this, EventArgs.Empty, default(CancellationToken), 60, "SaveFromServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DecimalBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveFromServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DecimalBusiness.cs", 240);
			res.AddReturnedValue(ex, "", "SaveFromServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DecimalBusiness.cs", 241);
		}
		return res;
	}
}
