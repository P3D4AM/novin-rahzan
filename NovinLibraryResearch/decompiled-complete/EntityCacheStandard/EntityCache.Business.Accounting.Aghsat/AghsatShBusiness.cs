using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Coding;
using EntityCache.Business.Core;
using EntityCache.Core;
using EntityCache.Interfaces.Sync;
using EntityCache.Mapping.BusinessMappers.Accounting.Aghsat;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.MetaData;
using PacketParser.EventArgs;
using PacketParser.Filters;
using PacketParser.Filters.Khazane;
using PacketParser.Records;
using PacketParser.Settings.Accounting;
using PacketParser.TransferLayer.Waiters.Accounting.Hesab;

namespace EntityCache.Business.Accounting.Aghsat;

public class AghsatShBusiness : BaseBusiness<AghsatShBusiness>, IAghsatShakhsi, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, ISyncFromClient2Server<AghsatShBusiness>, IDeliverableEntity, IEditableDelivery, IHasContext, INotifyPropertyChanged
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private SemaphoreSlim _sendToServerSemaphore = new SemaphoreSlim(1, 1);

	private CancellationTokenSource _sendToken = new CancellationTokenSource();

	private CancellationTokenSource _pardakhtToken = new CancellationTokenSource();

	private Guid? pardakhtGuid;

	private PardakhtReportBusiness _payment;

	public override string ContextGuid { get; set; }

	[JsonIgnore]
	public override IDatabaseManager DatabaseManager => ClsCache.GetCache(ContextGuid).AccDatabaseManager;

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now;


	public bool Status { get; set; } = true;


	public EnAghsatShStatus St { get; set; }

	public int ID2 { get; set; }

	public Guid TafsilGuid { get; set; }

	public Guid MoeinGuid { get; set; }

	public string Description { get; set; }

	public DateTime DateSarResid { get; set; } = DateTime.Now.AddMonths(1);


	public DateTime DateM { get; set; } = DateTime.Now;


	[JsonIgnore]
	public string DateSh => DateM.MiladiToCalture();

	public Guid UserGuid { get; set; }

	public Guid? PardakhtGuid
	{
		get
		{
			return pardakhtGuid;
		}
		set
		{
			try
			{
				if ((pardakhtGuid ?? Guid.Empty) == (value ?? Guid.Empty))
				{
					return;
				}
				if ((value ?? Guid.Empty) == Guid.Empty)
				{
					pardakhtGuid = null;
					_payment = null;
					if (this.PropertyChanged != null && UiInvokerHolder.Instance != null)
					{
						UiInvokerHolder.Instance.RunOnUiThread(delegate
						{
							this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PardakhtDateSh"));
							this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PardakhtSum"));
						}, "PardakhtGuid", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs");
					}
				}
				else
				{
					pardakhtGuid = value.Value;
					_pardakhtToken?.Cancel();
					_pardakhtToken = new CancellationTokenSource();
					CancellationToken token = _pardakhtToken.Token;
					Task.Run(() => GetPardakhtAsync(token));
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "PardakhtGuid", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 80);
			}
		}
	}

	public decimal Price { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public ENSource Source { get; set; }

	[JsonIgnore]
	public string PardakhtDateSh => PardakhtDateM.MiladiToCalture();

	public DateTime? PardakhtDateM { get; set; }

	public decimal PardakhtSum { get; set; }

	public string StatusName => St.GetDisplay();

	public string DateSarresidSh => DateSarResid.MiladiToCalture();

	public string FullName { get; set; }

	public PardakhtChequeShMetaData MetaData { get; set; }

	public DateTime? DateUtc { get; set; }

	[JsonIgnore]
	public bool IsChecked { get; set; }

	public event PropertyChangedEventHandler PropertyChanged;

	public AghsatShBusiness(string contextGuid)
	{
		try
		{
			ContextGuid = contextGuid;
			ChangeStatus = EntityChangeSt.New;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 109);
		}
	}

	public async Task<PardakhtReportBusiness> GetPardakhtAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (token.IsCancellationRequested)
			{
				return null;
			}
			if (PardakhtGuid.HasValue)
			{
				_payment = await PardakhtBusiness.GetReportAsync(ContextGuid, PardakhtGuid ?? Guid.Empty, token, tranName);
			}
			if (this.PropertyChanged != null && UiInvokerHolder.Instance != null)
			{
				UiInvokerHolder.Instance.RunOnUiThread(delegate
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PardakhtDateSh"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PardakhtSum"));
				}, "GetPardakhtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs");
			}
			return _payment;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPardakhtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 129);
			return null;
		}
	}

	public static Task<AghsatShBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.AghsatShRepository.GetAsync(guid, token, tranName);
	}

	public static Task<AghsatShBusiness> GetAsync(string contextGuid, int id2, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.AghsatShRepository.GetAsync(id2, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> CheckValidationAsync(EnSystem system, bool isDeleteAction, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (Price <= 0m)
			{
				ret.AddError(system, "مبلغ قسط پرداختنی معتبر نمی باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 142);
			}
			if (TafsilGuid == Guid.Empty)
			{
				return ret.AddError(system, "طرف حساب قسط پرداختنی معتبر نمی باشد!".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 143);
			}
			if (await TafsilBusiness.GetAsync(ContextGuid, system, TafsilGuid, tranName, token, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs") == null)
			{
				ret.AddError(system, "طرف حساب قسط دریافتنی یافت نشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 145);
			}
			TafsilBusiness tafsil = await TafsilBusiness.GetAsync(ContextGuid, system, TafsilGuid, tranName, token, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs");
			MoeinBusiness moein = await MoeinBusiness.GetAsync(ContextGuid, MoeinGuid, tranName, token);
			CashierBusiness cashierBusiness = await CashierBusiness.GetAsync(ContextGuid, UserGuid, token, tranName);
			if (tafsil == null)
			{
				ret.AddError("حساب تفصیلی با شناسه".GetTranslate() + "(" + $"{TafsilGuid}".GetTranslate() + ")" + "در سیستم یافت نشد.".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 151);
			}
			if (moein == null)
			{
				ret.AddError("حساب معین با شناسه".GetTranslate() + "(" + $"{MoeinGuid}".GetTranslate() + ")" + " در سیستم یافت نشد.".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 152);
			}
			if (cashierBusiness == null)
			{
				ret.AddError("کاربر با شناسه".GetTranslate() + "(" + $"{UserGuid}" + ")" + " در سیستم یافت نشد.".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 153);
			}
			if (system == EnSystem.UserInterface && !isDeleteAction)
			{
				AccGlobalSettings obj = await LocalSettingBusiness.GetSettingAsync(ContextGuid, token);
				DateTime chequeAndInstallmentStartDate = obj.DateAndTimePriod.ChequeAndInstallmentStartDate;
				DateTime chequeAndInstallmentEndDate = obj.DateAndTimePriod.ChequeAndInstallmentEndDate;
				if (DateSarResid < chequeAndInstallmentStartDate || DateSarResid > chequeAndInstallmentEndDate)
				{
					ret.AddError(system, "شماره".GetTranslate() + " " + $"{ID2}" + "\r\n" + "تاریخ سررسید".GetTranslate() + "(" + DateSarResid.MiladiToCalture() + ")\r\n" + " در بازه مجاز".GetTranslate() + "(" + chequeAndInstallmentStartDate.MiladiToCalture() + " " + " تا".GetTranslate() + " " + chequeAndInstallmentEndDate.MiladiToCalture() + ") " + " نیست.".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 162);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 167);
			ret.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 168);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, CancellationToken token = default(CancellationToken), Priority priority = Priority.Low, bool sendToServer = true, bool awaitOnSemaphore = true, bool await4send = false, short tryCount = 10, string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo ret = await CheckValidationAsync(system, isDeleteAction: false, autoTran ? "" : tranName, token);
		bool transactionHasBeenCreated = false;
		ClsCache cache = ClsCache.GetCache(ContextGuid);
		try
		{
			int num;
			_ = num - 1;
			_ = 7;
			ReturnedSaveFuncInfo result;
			try
			{
				if (system == EnSystem.SignalR)
				{
					ret.throwExceptionIfError();
				}
				sendToServer = sendToServer && cache.BufferEnable;
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "AghsatShBusiness/SaveAsync", 6000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 185);
					}
					else
					{
						if (autoTran)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 186), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 186);
						}
						transactionHasBeenCreated = true;
						if (ChangeStatus == EntityChangeSt.Edited && system != EnSystem.Transfer && cache.Source == ENSource.Accounting)
						{
							AghsatShBusiness aghsatShBusiness = await GetFromDatabaseAsync(ContextGuid, Guid, token, tranName);
							if (aghsatShBusiness != null)
							{
								ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await TafsilBusiness.UpdateSumAghsatAsync(ContextGuid, TafsilGuid, aghsatShBusiness.Price, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 192);
							}
						}
						if (ret.HasError)
						{
							result = ret;
						}
						else
						{
							if (system == EnSystem.SignalR && cache.Source == ENSource.Accounting)
							{
								ServerStatus = ServerStatusForClient.None;
							}
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await TafsilBusiness.UpdateServerStatusAsync(ContextGuid, TafsilGuid, ServerStatusForClient.None, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 196);
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.Account.UpdateServerStatusAsync(TafsilGuid, ServerStatusForClient.None, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 197);
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.AghsatShRepository.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 198);
							if (!ret.HasError)
							{
								if (system != EnSystem.Transfer && !PardakhtGuid.HasValue && cache.Source == ENSource.Accounting)
								{
									returnedSaveFuncInfo = ret;
									returnedSaveFuncInfo.AddReturnedValue(await TafsilBusiness.UpdateSumAghsatAsync(ContextGuid, TafsilGuid, -Price, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 201);
								}
								goto end_IL_0281;
							}
							result = ret;
						}
					}
					goto end_IL_0193;
					end_IL_0281:;
				}
				goto end_IL_017c;
				end_IL_0193:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 205);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 206);
				goto end_IL_017c;
			}
			return result;
			end_IL_017c:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 211), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 211);
			}
			if (ret.HasError)
			{
				LogError(ret);
			}
			if (!ret.HasError && sendToServer)
			{
				if (await4send)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Send2ServerAsync(token, system, priority), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 217);
				}
				else
				{
					Task.Run(() => Send2ServerAsync(token, system, priority));
				}
			}
			if (ClsCache.GetCache(ContextGuid).BufferEnable && cache.Source == ENSource.Accounting && system != EnSystem.Transfer)
			{
				Task.Run(() => SendEntitiesToServerAsync(token));
			}
			if (!ret.HasError && system == EnSystem.UserInterface)
			{
				ClsCache.GetCache(ContextGuid).BusinessAggregator.Events.AghsatSh.RaiseGhestSaved(this, EventArgs.Empty);
			}
		}
		return ret;
	}

	private async Task SendEntitiesToServerAsync(CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("SendEntitiesToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs"))
		{
			_ = 1;
			try
			{
				await Task.Delay(500);
				await Task.WhenAll<ReturnedSaveFuncInfo>(KolBusiness.ResendNotSents(ContextGuid, token, "", 5), MoeinBusiness.ResendNotSentsAsync(ContextGuid, token, "", 20), TafsilBusiness.ResendNotSentsAsync(ContextGuid, token, "", 20), AccountBusiness.ResendNotSentsAsync(ContextGuid, EnSystem.None, token, "", 10));
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SendEntitiesToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 239);
			}
		}
	}

	private void LogError(ReturnedSaveFuncInfo ret)
	{
		try
		{
			string text = Json.ToStringJson(this);
			WebErrorLog.Instance.StartLog("خطا در ذخیره قسط شخصی".GetTranslate() + "\r\n" + ret.ErrorMessage + "\r\nJsonFata=>\r\n" + text, "", "LogError", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 246);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LogError", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 253);
		}
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(EnSystem system, bool removePayment, CancellationToken token = default(CancellationToken), string tranName = "")
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
			_ = 4;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 264), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 264);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.AghsatShRepository.RemoveAsync(Guid, 1, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 266);
				if (ret.HasError)
				{
					return ret;
				}
				if (removePayment)
				{
					PardakhtReportBusiness pardakhtReportBusiness = await GetPardakhtAsync(token, tranName);
					if (pardakhtReportBusiness != null)
					{
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await pardakhtReportBusiness.RemoveAsync(system, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 271);
					}
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await TafsilBusiness.UpdateSumAghsatAsync(ContextGuid, TafsilGuid, Price, 1, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 273);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 277);
				ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 278);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 283), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 283);
			}
		}
		return ret;
	}

	public static Task<int> NextId2Async(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.AghsatShRepository.NextId2Async(token, tranName);
	}

	public static async Task<List<AghsatShBusiness>> GetAllAsync(string contextGuid, AghsatShFilter filter, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs"))
		{
			try
			{
				if (string.IsNullOrEmpty(filter.SearchString))
				{
					filter.SearchString = "";
				}
				IEnumerable<AghsatShBusiness> enumerable = await ClsCache.GetCache(contextGuid).CacheRepository.AghsatShRepository.GetAllAsync(filter, token, tranName);
				List<string> list = filter.SearchString.SplitString();
				if (list != null && list.Count > 0)
				{
					foreach (string item in list)
					{
						if (!string.IsNullOrEmpty(item?.Trim()))
						{
							enumerable = enumerable?.Where((AghsatShBusiness x) => x.DateSarresidSh.StringComparisons(item) || x.ID2.StringComparisons(item) || (!string.IsNullOrEmpty(x.FullName) && x.FullName.StringComparisons(item)) || (!string.IsNullOrEmpty(x.Description) && x.Description.StringComparisons(item)) || x.Price.StringComparisons(item));
						}
					}
				}
				return (enumerable?.OrderBy((AghsatShBusiness o) => o.DateSarresidSh))?.ToList();
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 312);
				throw;
			}
		}
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(EnSystem system, CancellationToken token, Priority priority, List<AghsatShBusiness> lst, bool sendToServer, bool awaitOnSend = false, short tryCount = 10, string tranName = "", Action<PercenteCompletedEventArgs> percentCompleted = null)
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
			foreach (AghsatShBusiness item in lst)
			{
				current = checked(current + 1);
				if (item == null)
				{
					throw new NullReferenceException("list contains null items!");
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 328);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(system, token, priority, sendToServer, awaitOnSemaphore: true, awaitOnSend, tryCount, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 330);
				if (percentCompleted != null)
				{
					invokeToken2?.Cancel();
					invokeToken2 = new CancellationTokenSource(60000);
					CancellationToken token2 = invokeToken2.Token;
					percentCompleted(new PercenteCompletedEventArgs(lst.Count(), current, item.ID2.ToString(), token2));
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 343);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 344);
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

	public override IEditableDelivery GetRepository()
	{
		return ClsCache.GetCache(ContextGuid).CacheRepository.AghsatShRepository;
	}

	public async Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token = default(CancellationToken), short tryCount = 10, string tranName = "")
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 367), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 367);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.AghsatShRepository.SetSaveResult(item, token, tryCount, tranName), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 369);
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 374);
				ret.AddReturnedValue(ex2, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 375);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 380), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 380);
			}
		}
		return ret;
	}

	public Task<WebAghsatShakhsi> ToWebItemAsync(CancellationToken token = default(CancellationToken))
	{
		return AghsatShMapper.Instance.MapAsync(ContextGuid, this, new WebAghsatShakhsi(), "", token);
	}

	public static Task<ReturnedSaveFuncInfo> DeleteRangeAsync(string contextGuid, List<Guid> eConnectionData, CancellationToken token = default(CancellationToken))
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.AghsatShRepository.DeleteRangeAsync(eConnectionData, 3, token, "");
	}

	public async Task<ReturnedSaveFuncInfo> Send2ServerAsync(CancellationToken token, EnSystem system, Priority priority = Priority.Low)
	{
		using (new JaegerMonitor("Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				_sendToken?.Cancel();
				_sendToken = new CancellationTokenSource();
				CancellationToken token2 = _sendToken.Token;
				using SemaphoreUtil dis = await _sendToServerSemaphore.GaurdAsync(true, "AghsatMBusiness/Send2ServerAsync", 60000, token, token2);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 399);
				}
				List<WebAghsatShakhsi> list = new List<WebAghsatShakhsi>();
				List<WebAghsatShakhsi> list2 = list;
				list2.Add(await AghsatShMapper.Instance.MapAsync(ContextGuid, this, new WebAghsatShakhsi(), "", token));
				List<Packet> paks = WebAghsatShakhsi.GenerateSaveRequest(list, 0);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 403);
				}
				using AghsatShWaiter waiter = new AghsatShWaiter(WebContextHolder.GetWebContextHolder(ContextGuid), Guid, dis.Token);
				Task.Run(() => ClsCache.GetCache(ContextGuid).SendData(paks, priority), token);
				ReturnedSaveFuncInfoWithValue<SaveResponse> returnedSaveFuncInfoWithValue = await waiter.WaitForSaveResponse("AghsatShBusinessWaiter");
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 407);
				if (!ret.HasError && returnedSaveFuncInfoWithValue?.value != null)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SetSaveResultAsync(returnedSaveFuncInfoWithValue.value, token, 10), ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 409);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 411);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 414);
				ret.AddReturnedValue(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 415);
			}
			return ret;
		}
	}

	public static Task<AghsatShBusiness> GetFromDatabaseAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (!(guid == Guid.Empty))
		{
			return ClsCache.GetCache(contextGuid).CacheRepository.AghsatShRepository.GetFromDatabaseAsync(guid, token, tranName);
		}
		return Task.FromResult<AghsatShBusiness>(null);
	}

	public Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token, string tranName, short maxParalelTask)
	{
		return ResendNotSentsAsync(contextGuid, token, tranName, maxParalelTask);
	}

	internal Task<string> GetUiDescriptionAsync()
	{
		try
		{
			return St switch
			{
				EnAghsatShStatus.Payed => Task.FromResult("اقساط شخصی پرداخت شده".GetTranslate() + (string.IsNullOrEmpty(Description) ? "" : ("\r\n" + Description))), 
				EnAghsatShStatus.NotPayed => Task.FromResult("اقساط شخصی پرداخت نشده".GetTranslate() + (string.IsNullOrEmpty(Description) ? "" : ("\r\n" + Description))), 
				EnAghsatShStatus.Removed => Task.FromResult("اقساط شخصی ابطال شده".GetTranslate() + (string.IsNullOrEmpty(Description) ? "" : ("\r\n" + Description))), 
				_ => throw new NotImplementedException($"chequeSh status is not valid. {Status}"), 
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetUiDescriptionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 438);
			throw;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> ResendNotSentsAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "", short maxParalelTask = 20)
	{
		using (new JaegerMonitor("ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs"))
		{
			int line = 0;
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			ISendData2Server uiUpdater = ClsCache.GetCache(contextGuid)?.SendData2ServerInstance?.GhestSh;
			try
			{
				if (uiUpdater != null)
				{
					uiUpdater.ShortMessage = "در حال ارسال اقساط شخصی".GetTranslate();
					uiUpdater.Status = SyncStatus.Syncing;
					uiUpdater.FinalStatus = ret;
					uiUpdater.Group = SyncPart.Operations;
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 458);
				}
				ClsCache cache = ClsCache.GetCache(contextGuid);
				int num;
				if (cache == null)
				{
					num = 1;
				}
				else
				{
					_ = cache.BufferCancellationToken;
					num = 0;
				}
				if (num != 0)
				{
					return ret.AddError(EnSystem.SignalR, "کش آنلاین نیست!!!!".GetTranslate(), "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 460);
				}
				line = 50;
				if (ClsCache.GetCache(contextGuid).CacheRepository.AghsatShRepository == null)
				{
					throw new NullReferenceException("ClsCache.GetCache(contextGuid) .CacheRepository.AghsatShRepository is null");
				}
				line = 51;
				List<Guid> list = (await ClsCache.GetCache(contextGuid).CacheRepository.AghsatShRepository.GetAllNotSentAsync(token, tranName)) ?? new List<Guid>();
				line = 52;
				if (uiUpdater != null)
				{
					uiUpdater.TotalCount = list?.Count ?? 0;
				}
				line = 6;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 470);
				}
				if (list == null || !list.Any())
				{
					return ret.AddInformation("اقساط شخصی سینک هستند تعداد اقساط شخصی سینک شده".GetTranslate() + ":" + $"{list?.Count ?? 0}" + ")", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 471);
				}
				List<Task<ReturnedSaveFuncInfo>> tasks = new List<Task<ReturnedSaveFuncInfo>>();
				int current = 0;
				_ = (decimal)(list?.Count ?? 0);
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (list != null && list.Any())
				{
					foreach (Guid item in list)
					{
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 478);
						}
						current = checked(current + 1);
						AghsatShBusiness obj = await GetFromDatabaseAsync(contextGuid, item, token, tranName);
						if (uiUpdater != null)
						{
							uiUpdater.ShortMessage = "اقساط شخصی".GetTranslate() + (obj.FullName ?? "").GetTranslate() + $"-{obj.Price:N0}";
							uiUpdater.Count = current;
						}
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 486);
						}
						if (obj.ServerStatus == ServerStatusForClient.Delivered || obj.ServerStatus == ServerStatusForClient.DirectDelivery)
						{
							ret.AddInformation("current status on database is delivered!\r\nEntityType: AghsatSh \r\n" + $"EntityGuid:{obj?.Guid ?? Guid.Empty}", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 490);
							continue;
						}
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 495);
						}
						tasks.Add(Task.Run(() => obj.Send2ServerAsync(token, EnSystem.SignalR), token));
						if (tasks.Count >= maxParalelTask)
						{
							Task<ReturnedSaveFuncInfo> t = await Task.WhenAny(tasks);
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await t, ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 500);
							tasks.Remove(t);
						}
					}
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 504);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 505);
				}
				line = 21;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}//ClsCache.GetCache(contextGuid).CacheRepository.AghsatShRepository null:{ClsCache.GetCache(contextGuid)?.CacheRepository?.AghsatShRepository == null}", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 510);
				ret.AddReturnedValue(ex, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 511);
			}
			finally
			{
				if (uiUpdater != null)
				{
					if (ret.HasError)
					{
						uiUpdater.ShortMessage = "با شکست مواجه شد".GetTranslate();
						uiUpdater.Status = SyncStatus.SyncFailed;
					}
					else
					{
						uiUpdater.ShortMessage = "با موفقیت انجام شد.".GetTranslate();
						uiUpdater.Status = SyncStatus.SyncedOk;
					}
				}
			}
			return ret;
		}
	}

	public static async Task<AghsatShBusiness> GenerateNewWithDefaultsAsync(string contextGuid, EnSystem system, Guid? tafsilGuid = null, Guid? guid = null, decimal? price = null, string description = "", CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			AghsatShBusiness ret = new AghsatShBusiness(contextGuid)
			{
				ChangeStatus = EntityChangeSt.New,
				Source = ClsCache.GetCache(contextGuid).Source,
				UserGuid = CashierBusiness.GetCurrentCashierGuid(contextGuid),
				Price = price.GetValueOrDefault()
			};
			if ((tafsilGuid ?? Guid.Empty) != Guid.Empty)
			{
				ret.TafsilGuid = tafsilGuid.Value;
				TafsilBusiness obj = await TafsilBusiness.GetAsync(contextGuid, system, tafsilGuid.Value, "", default(CancellationToken), "GenerateNewWithDefaultsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs");
				AghsatShBusiness aghsatShBusiness = ret;
				aghsatShBusiness.MoeinGuid = (await obj.GetTafsilGroupAsync(system, token, tranName)).MoeinPardakhtGuid;
			}
			if (!string.IsNullOrEmpty(description))
			{
				ret.Description = description.FixString();
			}
			if ((guid ?? Guid.Empty) != Guid.Empty)
			{
				ret.Guid = guid.Value;
			}
			else
			{
				ret.Guid = Guid.NewGuid();
			}
			return ret;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateNewWithDefaultsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 556);
			throw;
		}
	}

	public static Task<IEnumerable<BedehiTalabModel>> GetAllReportAsync(string contextGuid, BedehiTalabReportFilter filter, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.AghsatShRepository.GetAllReportAsync(filter, token, tranName);
	}

	public static Task<List<AghsatSHSummaryViewModel>> GetSummaryAsync(string contextGuid, DateTime? date1 = null, DateTime? date2 = null, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.AghsatShRepository.GetSummaryAsync(date1, date2, token, tranName);
	}

	public static async Task<ReturnedSaveFuncInfo> CalculatePardakhtAsync(string contextGuid, Guid pardakhtGuid, Guid qestGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			AghsatShBusiness qest = await GetAsync(contextGuid, qestGuid, token, tranName);
			if (qest == null)
			{
				return res;
			}
			decimal qestPrice = qest.Price;
			PardakhtBusiness pardakht = await PardakhtBusiness.GetAsync(contextGuid, pardakhtGuid, fromDatabase: true, token, tranName);
			if (pardakht == null)
			{
				return res;
			}
			decimal pardakhtSum = pardakht.Sum;
			AghsatShFilter filter = new AghsatShFilter
			{
				TafsilGuid = qest.TafsilGuid,
				Status = EnAghsatShStatus.NotPayed
			};
			List<AghsatShBusiness> lstAghsat = (await GetAllAsync(contextGuid, filter, token, tranName))?.Where((AghsatShBusiness q) => q.Guid != qestGuid)?.ToList();
			Guid guid = qest.Guid;
			decimal num = default(decimal);
			if (pardakhtSum - qestPrice > 0m)
			{
				while (pardakhtSum > num)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ElamPardakhtAsync(contextGuid, guid, pardakht.Guid, token, tranName), ReturnedState.None, "CalculatePardakhtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 588);
					decimal num2 = pardakhtSum;
					pardakhtSum = num2 - (await GetAsync(contextGuid, guid)).Price;
					AghsatShBusiness aghsatShBusiness = lstAghsat?.Take(1).FirstOrDefault();
					if (aghsatShBusiness == null)
					{
						guid = Guid.Empty;
						break;
					}
					lstAghsat.Remove(aghsatShBusiness);
					num = aghsatShBusiness.Price;
					guid = aghsatShBusiness.Guid;
				}
				if ((pardakhtSum > 0m) & (guid != Guid.Empty))
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ElamPardakhtGuidAsync(contextGuid, guid, pardakht.Guid, pardakhtSum, token, tranName), ReturnedState.None, "CalculatePardakhtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 604);
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await AddNewGhestAsync(contextGuid, guid, qestPrice - pardakhtSum, token, tranName), ReturnedState.None, "CalculatePardakhtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 605);
				}
			}
			else if (pardakhtSum - qestPrice < 0m)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ElamPardakhtGuidAsync(contextGuid, qest.Guid, pardakht.Guid, pardakhtSum, token, tranName), ReturnedState.None, "CalculatePardakhtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 610);
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await AddNewGhestAsync(contextGuid, qest.Guid, qestPrice - pardakhtSum, token, tranName), ReturnedState.None, "CalculatePardakhtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 611);
			}
			else
			{
				qest.PardakhtGuid = pardakht.Guid;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await qest.SaveAsync(EnSystem.UserInterface, default(CancellationToken), Priority.Low, sendToServer: true, awaitOnSemaphore: true, await4send: false, 10), ReturnedState.None, "CalculatePardakhtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 616);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CalculatePardakhtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 621);
			res.AddReturnedValue(ex, "", "CalculatePardakhtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 622);
		}
		return res;
	}

	private static async Task<ReturnedSaveFuncInfo> ElamPardakhtAsync(string contextGuid, Guid guid, Guid pardakhtGuid, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			AghsatShBusiness cls = await GetAsync(contextGuid, guid, token, tranName);
			cls.PardakhtGuid = (await PardakhtBusiness.GetAsync(contextGuid, pardakhtGuid, fromDatabase: true, token, tranName)).Guid;
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await cls.SaveAsync(EnSystem.UserInterface, token, Priority.Low, sendToServer: true, awaitOnSemaphore: true, await4send: false, 10, tranName), ReturnedState.None, "ElamPardakhtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 634);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ElamPardakhtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 638);
			res.AddReturnedValue(ex, "", "ElamPardakhtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 639);
		}
		return res;
	}

	public static async Task<ReturnedSaveFuncInfo> ElamPardakhtGuidAsync(string contextGuid, Guid guid, Guid pardakhtGuid, decimal r, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			AghsatShBusiness obj = await GetAsync(contextGuid, guid, token, tranName);
			obj.PardakhtGuid = pardakhtGuid;
			obj.Price = r;
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await obj.SaveAsync(EnSystem.UserInterface, token, Priority.Low, sendToServer: true, awaitOnSemaphore: true, await4send: false, 10, tranName), ReturnedState.None, "ElamPardakhtGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 651);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ElamPardakhtGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 655);
			res.AddReturnedValue(ex, "", "ElamPardakhtGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 656);
		}
		return res;
	}

	private static async Task<ReturnedSaveFuncInfo> AddNewGhestAsync(string contextGuid, Guid guid, decimal p, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			AghsatShBusiness cls = await GetAsync(contextGuid, guid, token, tranName);
			AghsatShBusiness aghsatShBusiness = new AghsatShBusiness(contextGuid)
			{
				DateM = DateTime.Now.Date,
				DateSarResid = cls.DateSarResid,
				Description = (cls.Description ?? "").GetTranslate() + "باقیمانده قسط شماره ی ".GetTranslate() + $"{cls.ID2}",
				TafsilGuid = cls.TafsilGuid
			};
			AghsatShBusiness aghsatShBusiness2 = aghsatShBusiness;
			aghsatShBusiness2.ID2 = await NextId2Async(contextGuid, token, tranName);
			aghsatShBusiness.Price = p;
			aghsatShBusiness.Guid = Guid.NewGuid();
			aghsatShBusiness.UserGuid = cls.UserGuid;
			AghsatShBusiness cls2 = aghsatShBusiness;
			cls2.MoeinGuid = (await (await TafsilBusiness.GetAsync(contextGuid, EnSystem.UserInterface, cls.TafsilGuid, "", default(CancellationToken), "AddNewGhestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs")).GetTafsilGroupAsync(EnSystem.UserInterface, token, tranName)).MoeinPardakhtGuid;
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await cls2.SaveAsync(EnSystem.UserInterface, token, Priority.Low, sendToServer: true, awaitOnSemaphore: true, await4send: false, 10, tranName), ReturnedState.None, "AddNewGhestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 680);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddNewGhestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 684);
			res.AddReturnedValue(ex, "", "AddNewGhestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 685);
		}
		return res;
	}

	public static List<SmsTemplateRecord> GetSmsTemplateList()
	{
		return new List<SmsTemplateRecord>
		{
			new SmsTemplateRecord("[گیرنده]".GetTranslate()),
			new SmsTemplateRecord("[شماره قسط]".GetTranslate()),
			new SmsTemplateRecord("[تاریخ قسط]".GetTranslate()),
			new SmsTemplateRecord("[مبلغ قسط]".GetTranslate())
		};
	}

	public static async Task<ReturnedSaveFuncInfo> UpdatePardakhtInfoAsync(string contextGuid, Guid? pardakhtGuid, CancellationToken token = default(CancellationToken), string tranName = "")
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
			_ = 2;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(true, "AghsatShBusiness/UpdatePardakhtInfoAsync", 60000, token))
				{
					if (!dis.Token.IsCancellationRequested)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "UpdatePardakhtInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 707), ReturnedState.None, "UpdatePardakhtInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 707);
						}
						transactionHasBeenCreated = true;
						returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.AghsatShRepository.UpdatePardakhtInfoAsync(pardakhtGuid, token, tranName), ReturnedState.None, "UpdatePardakhtInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 709);
						goto end_IL_010d;
					}
					result = res.AddErrorTokenIsCancelled("", "", "UpdatePardakhtInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 705);
					goto end_IL_0076;
					end_IL_010d:;
				}
				goto end_IL_0065;
				end_IL_0076:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UpdatePardakhtInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 713);
				res.AddReturnedValue(ex, "", "UpdatePardakhtInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 714);
				goto end_IL_0065;
			}
			return result;
			end_IL_0065:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "UpdatePardakhtInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 719), ReturnedState.None, "UpdatePardakhtInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatShBusiness.cs", 719);
			}
		}
		return res;
	}
}
