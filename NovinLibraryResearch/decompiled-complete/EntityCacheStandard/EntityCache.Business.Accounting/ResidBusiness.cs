using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Selection;
using EntityCache.Business.Report;
using PacketParser;
using PacketParser.Access.SanadStatusAccess;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Interfaces;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;
using PacketParser.Filters;
using PacketParser.Settings.Accounting;

namespace EntityCache.Business.Accounting;

public class ResidBusiness : IResid, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext, IHasSanadStatusReport
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private Guid? _prdGuid;

	private Guid? _tafsilGuid;

	private Guid? _vahedGuid;

	private decimal _qt;

	private ReceptionBusiness _reception;

	private OrderBusiness _order;

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; } = true;


	public long ID2 { get; set; }

	public DateTime DateM { get; set; } = DateTime.Now;


	public string Description { get; set; }

	public DateTime Date1 { get; set; } = DateTime.Now;


	public DateTime Date2 { get; set; } = DateTime.Now;


	public PrdVahedBusiness ProductVahed { get; private set; }

	public Guid? GuidVahed => _vahedGuid;

	public string VahedName => ProductVahed?.Name ?? "";

	public decimal Price { get; set; }

	public decimal Qt
	{
		get
		{
			return _qt.ToString().ParseToDecimal(0m, 3);
		}
		set
		{
			_qt = value;
		}
	}

	public decimal Mandeh { get; set; }

	public ProductCompactBusiness Prd { get; private set; }

	public string PrdModel { get; set; }

	public string PrdColor { get; set; }

	public string PrdSerial { get; set; }

	public TafsilBusiness Tafsil { get; private set; }

	public Guid? TafsilGuid => _tafsilGuid;

	public string TafsilName => Tafsil?.Name ?? "";

	public string Problems { get; set; }

	public string Darkhast { get; set; }

	public string Date1Sh => Date1.MiladiToCalture();

	public string Time1 => Date1.Hour + ":" + Date1.Minute;

	public string PrdName => Prd?.Name ?? "";

	public string PrdCode => Prd?.Code.ToString() ?? "";

	public string ContextGuid { get; set; }

	public bool IsChecked { get; set; } = true;


	public short StatusId { get; set; }

	public Guid? CashierGuid { get; set; }

	public Guid? ReceptionGuid { get; set; }

	public Guid? OrderGuid { get; set; }

	public string Reserve { get; set; }

	public Document AsType => Document.Resid;

	public EntityChangeSt ChangeStatus { get; set; }

	Guid IHasSanadStatusReport.CashierGuid => CashierGuid ?? Guid.Empty;

	internal ResidBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
		ChangeStatus = EntityChangeSt.New;
	}

	public static async Task<ResidBusiness> GetFactoryAsync(string contextGuid)
	{
		try
		{
			ResidBusiness item = new ResidBusiness(contextGuid)
			{
				CashierGuid = CashierBusiness.GetCurrentCashierGuid(contextGuid)
			};
			await SanadStatusReportBusiness.ApplySanadStatusIdAsync(contextGuid, item, EnSystem.None);
			return item;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetFactoryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs", 84);
			throw;
		}
	}

	public async Task<bool> CanInsertAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("CanInsertAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs"))
		{
			_ = 1;
			try
			{
				CashierBusiness cashierBusiness = await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token);
				UserAccBusiness userAccBusiness = ((cashierBusiness != null) ? (await cashierBusiness.GetUserAsync(token, tranName)) : null);
				bool flag = userAccBusiness.UserAccess.Resid.SanadStatusAccess.SingleOrDefault((ResidSanadStatusAccess q) => q.StatusId == StatusId)?.Add ?? true;
				return userAccBusiness.UserAccess.Resid.Resid_Insert && flag;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CanInsertAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs", 101);
				throw;
			}
		}
	}

	public async Task<bool> CanDeleteAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("CanDeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs"))
		{
			_ = 1;
			try
			{
				CashierBusiness cashier = await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token);
				UserAccBusiness userAccBusiness = ((cashier != null) ? (await cashier.GetUserAsync(token, tranName)) : null);
				UserAccBusiness userAccBusiness2 = userAccBusiness;
				bool flag = userAccBusiness2.UserAccess.Resid.SanadStatusAccess.SingleOrDefault((ResidSanadStatusAccess q) => q.StatusId == StatusId)?.Delete ?? true;
				return (userAccBusiness2.UserAccess.Resid.Resid_Delete || (userAccBusiness2.UserAccess.Resid.Resid_Delete_Self && CashierGuid == cashier.Guid)) && flag;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CanDeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs", 118);
				throw;
			}
		}
	}

	public async Task<bool> CanUpdateAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("CanUpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs"))
		{
			_ = 1;
			try
			{
				CashierBusiness cashier = await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token, tranName);
				UserAccBusiness userAccBusiness = ((cashier != null) ? (await cashier.GetUserAsync(token, tranName)) : null);
				UserAccBusiness userAccBusiness2 = userAccBusiness;
				bool flag = userAccBusiness2.UserAccess.Resid.SanadStatusAccess.SingleOrDefault((ResidSanadStatusAccess q) => q.StatusId == StatusId)?.Edit ?? true;
				int num;
				if (!userAccBusiness2.UserAccess.Resid.Resid_Update)
				{
					if (userAccBusiness2.UserAccess.Resid.Resid_Update_Self)
					{
						Guid guid = cashier.Guid;
						Guid? cashierGuid = CashierGuid;
						num = ((guid == cashierGuid) ? 1 : 0);
					}
					else
					{
						num = 0;
					}
				}
				else
				{
					num = 1;
				}
				return (byte)((uint)num & (flag ? 1u : 0u)) != 0;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CanUpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs", 135);
				throw;
			}
		}
	}

	public async Task SetGuidVahedAsync(Guid? value)
	{
		try
		{
			_vahedGuid = value;
			if (_vahedGuid == Guid.Empty)
			{
				_vahedGuid = null;
			}
			if ((_vahedGuid ?? Guid.Empty) != Guid.Empty)
			{
				ProductVahed = await PrdVahedBusiness.GetAsync(ContextGuid, GuidVahed.Value);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetGuidVahedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs", 149);
		}
	}

	public async Task SetTafsilGuidAsync(Guid? value, EnSystem system)
	{
		try
		{
			_tafsilGuid = value;
			if (_tafsilGuid == Guid.Empty)
			{
				_tafsilGuid = null;
			}
			if ((_tafsilGuid ?? Guid.Empty) != Guid.Empty)
			{
				Tafsil = await TafsilBusiness.GetAsync(ContextGuid, system, TafsilGuid.Value, "", default(CancellationToken), "SetTafsilGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetTafsilGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs", 160);
		}
	}

	public Guid? GetPrdGuid()
	{
		return _prdGuid;
	}

	public async Task SetPrdGuidAsync(Guid? value, EnSystem system, CancellationToken token = default(CancellationToken))
	{
		try
		{
			_prdGuid = value;
			if (_prdGuid == Guid.Empty)
			{
				_prdGuid = null;
			}
			if (_prdGuid.HasValue)
			{
				Prd = await ProductCompactBusiness.GetAsync(ContextGuid, _prdGuid.Value, system, null, "", token, true);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetPrdGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs", 172);
		}
	}

	public async Task<ReceptionBusiness> GetReceptionAsync(EnSystem system = EnSystem.UserInterface, string tranName = "", bool fromDatabase = false, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("GetReceptionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs"))
		{
			if (string.IsNullOrEmpty(ContextGuid))
			{
				throw new ArgumentException("ContextGuid is null or empty!");
			}
			try
			{
				if (system == EnSystem.Transfer || (TafsilGuid ?? Guid.Empty) == Guid.Empty)
				{
					return null;
				}
				if (ClsCache.GetCache(ContextGuid).Source == ENSource.Accounting)
				{
					fromDatabase = true;
				}
				if (_reception == null && (ReceptionGuid ?? Guid.Empty) != Guid.Empty)
				{
					_reception = await ReceptionBusiness.GetAsync(ContextGuid, ReceptionGuid.Value, system, null, fromDatabase, token, tranName);
				}
				if (_reception == null)
				{
					if ((CashierGuid ?? Guid.Empty) == Guid.Empty)
					{
						CashierGuid = CashierBusiness.GetCurrentCashierGuid(ContextGuid);
					}
					if (ID2 <= 0)
					{
						ID2 = await NextId2Async(ContextGuid, StatusId, token, tranName);
					}
					_reception = new ReceptionBusiness(ContextGuid)
					{
						UserGuid = CashierGuid.Value,
						IsTemp = false,
						Date = DateM,
						Description = "پیش دریافت بابت رسید شماره".GetTranslate() + $" ({ID2})",
						Status = Status,
						ChangeStatus = EntityChangeSt.New
					};
					await _reception.SetTafsilGuidAsync(TafsilGuid.Value, system, token, tranName);
					ReceptionGuid = _reception.Guid;
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "GetReceptionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs", 204);
			}
			return _reception;
		}
	}

	public async Task<OrderBusiness> GetOrderAsync(EnSystem system = EnSystem.UserInterface, string tranName = "", bool fromDatabase = false, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("GetOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs"))
		{
			if (string.IsNullOrEmpty(ContextGuid))
			{
				throw new ArgumentException("ContextGuid is null or empty!");
			}
			try
			{
				if (system == EnSystem.Transfer || (TafsilGuid ?? Guid.Empty) == Guid.Empty)
				{
					return null;
				}
				ClsCache cache = ClsCache.GetCache(ContextGuid);
				if (cache.Source == ENSource.Accounting)
				{
					fromDatabase = true;
				}
				if (_order == null && (OrderGuid ?? Guid.Empty) != Guid.Empty)
				{
					_order = await OrderBusiness.GetAsync(ContextGuid, OrderGuid.Value, Document.OrderSell, fromDatabase, token, tranName);
				}
				if (_order == null)
				{
					if ((CashierGuid ?? Guid.Empty) == Guid.Empty)
					{
						CashierGuid = CashierBusiness.GetCurrentCashierGuid(ContextGuid);
					}
					if (ID2 <= 0)
					{
						ID2 = await NextId2Async(ContextGuid, StatusId, token, tranName);
					}
					_order = await OrderBusiness.GenerateNewWithDefaultsAsync(ContextGuid, system, Document.OrderSell, cache.Source, TafsilGuid, "", null, tranName, token);
					if (_reception != null)
					{
						_order.SetReception(_reception);
					}
					if ((_prdGuid ?? Guid.Empty) != Guid.Empty)
					{
						OrderDetailBusiness orderDetail = await OrderDetailBusiness.OrderDetailFactoryAsync(ContextGuid, _order, token, tranName);
						await orderDetail.SetProductGuidAsync(system, _prdGuid.Value, isLoadOriginalPrice: true, tranName, token);
						await orderDetail.SetQuantityAsync(system, Qt, token, tranName);
						await orderDetail.SetPriceAsync(system, Price, token, tranName);
						await _order.AddDetAsync(orderDetail, system, awaitOnSemaphor: true, token);
					}
					OrderGuid = _order.Guid;
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "GetOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs", 237);
			}
			return _order;
		}
	}

	public static Task<long> NextId2Async(string contextGuid, int? statusId, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ResidRepository.GetNextId2Async(statusId, token, tranName);
	}

	public async Task<int> NextId2Async(short? statusId, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return checked((int)(await NextId2Async(ContextGuid, statusId, token, tranName)));
	}

	public static Task<ResidBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ResidRepository.GetAsync(guid, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(short autoFixTryCount = 0, IMessageSender messageSender = null, CancellationToken token = default(CancellationToken), EnSystem system = EnSystem.UserInterface, bool awaitOnSemaphore = true, short tryCount = 10, string tranName = "")
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
			_ = 12;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "ResiedBusiness/SaveAsync", 60000, token))
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo;
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs", 254);
					}
					else
					{
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs", 255), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs", 255);
						}
						transactionHasBeenCreated = true;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(system, isDeleteAction: false, autoFixTryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs", 257);
						if (ret.HasError)
						{
							result = ret;
						}
						else
						{
							if (system == EnSystem.Transfer)
							{
								goto IL_0a4a;
							}
							if (!((_reception?.Sum ?? 0m) <= 0m))
							{
								_reception.SetId2(0, token);
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await _reception.SaveAsync(system, sendToServer: true, Priority.Low, awaitOnSemaphore: true, "", "", 60000, await4send: false, tryCount, tranName, token), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs", 278);
								goto IL_06d5;
							}
							ReceptionBusiness receptionOnDb = null;
							if ((ReceptionGuid ?? Guid.Empty) != Guid.Empty)
							{
								receptionOnDb = await ReceptionBusiness.GetAsync(ContextGuid, ReceptionGuid.Value, system, null, fromDatabase: true, token, tranName);
							}
							ReceptionGuid = null;
							_reception = null;
							if (receptionOnDb == null)
							{
								goto IL_06d5;
							}
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ResidRepository.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs", 270);
							if (!ret.HasError)
							{
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await receptionOnDb.RemoveAsync(system, awaitOnSemaphore: false, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs", 272);
								goto IL_06d5;
							}
							result = ret;
						}
					}
					goto end_IL_00b6;
					IL_06d5:
					if (!((_order?.Sum ?? 0m) <= 0m))
					{
						_order.SetId2(0);
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await _order.SaveAsync(system, awaitOnSemaphore: true, sendToServer: true, isTemp: false, 10), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs", 298);
						goto IL_0a4a;
					}
					OrderBusiness orderOnDb = null;
					if ((OrderGuid ?? Guid.Empty) != Guid.Empty)
					{
						orderOnDb = await OrderBusiness.GetAsync(ContextGuid, OrderGuid.Value, Document.OrderSell, fromDatabase: true, token, tranName);
					}
					OrderGuid = null;
					_order = null;
					if (orderOnDb == null)
					{
						goto IL_0a4a;
					}
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ResidRepository.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs", 290);
					if (!ret.HasError)
					{
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await orderOnDb.RemoveAsync(system, new OrderRemoveFilter(config: true), tryCount, tranName, token), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs", 292);
						goto IL_0a4a;
					}
					result = ret;
					goto end_IL_00b6;
					IL_0a4a:
					if (!ret.HasError)
					{
						await SanadStatusReportBusiness.ApplySanadStatusIdAsync(ContextGuid, this, system, token, tranName);
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ResidRepository.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs", 303);
						goto end_IL_017e;
					}
					result = ret;
					goto end_IL_00b6;
					end_IL_017e:;
				}
				goto end_IL_00a3;
				end_IL_00b6:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs", 307);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs", 308);
				goto end_IL_00a3;
			}
			return result;
			end_IL_00a3:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs", 313), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs", 313);
			}
			if (!ret.HasError && messageSender != null)
			{
				await SendMessageAsync(messageSender);
			}
		}
		return ret;
	}

	private async Task SendMessageAsync(IMessageSender messageSender)
	{
		try
		{
			if (StatusId <= 0 || (TafsilGuid ?? Guid.Empty) == Guid.Empty)
			{
				return;
			}
			SanadStatusReportBusiness status = await SanadStatusReportBusiness.GetAsync(ContextGuid, StatusId);
			if (!(status?.MetaData?.SmsManager?.IsSendSms).GetValueOrDefault())
			{
				return;
			}
			AccGlobalSettings settings = await LocalSettingBusiness.GetSettingAsync(ContextGuid);
			string message = status.MetaData.SmsManager.MessagePattern;
			message = await FillSsmsParametersAsync(message, status.Name);
			if (!string.IsNullOrEmpty(message) && status.MetaData.SmsManager.SendStatus != PayamakSendStatus.Disable && settings.Payamak.RunAfterInitializeProgram)
			{
				string mobiles = string.Join(",", (await TafsilNumberBusiness.GetAsync(ContextGuid, TafsilGuid.Value))?.Select((TafsilNumberBusiness p) => p.NumberForDisplay()));
				Task.Run(() => messageSender.SendSmsAsync(status.MetaData.SmsManager.SendStatus, settings.Payamak.Account_ID, message, mobiles));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SendMessageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs", 337);
			throw;
		}
	}

	private async Task<string> FillSsmsParametersAsync(string message, string statusName)
	{
		try
		{
			AccGlobalSettings accGlobalSettings = await LocalSettingBusiness.GetSettingAsync(ContextGuid);
			if (message.Contains("[نام مشتری]"))
			{
				message = message.Replace("[نام مشتری]", Tafsil?.Name ?? "");
			}
			if (message.Contains("[شماره رسید]"))
			{
				message = message.Replace("[شماره رسید]", ID2.ToString());
			}
			if (message.Contains("[نام وضعیت]"))
			{
				message = message.Replace("[نام وضعیت]", statusName);
			}
			if (message.Contains("[نام شرکت]"))
			{
				message = message.Replace("[نام شرکت]", accGlobalSettings.CompanyInfo.CompanyTitle);
			}
			if (message.Contains("[تلفن]"))
			{
				message = message.Replace("[تلفن]", accGlobalSettings.CompanyInfo.Telephone);
			}
			if (message.Contains("[همراه]"))
			{
				message = message.Replace("[همراه]", accGlobalSettings.CompanyInfo.Mobile);
			}
			return message;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FillSsmsParametersAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs", 356);
			throw;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> RemoveAsync(string contextGuid, Guid guid, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs", 368), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs", 368);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.ResidRepository.RemoveAsync(guid, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs", 370);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs", 374);
				ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs", 375);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs", 380), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs", 380);
			}
		}
		return ret;
	}

	public static Task<ResidBusiness> GetAsync(string contextGuid, long Id2, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ResidRepository.GetAsync(Id2, token, tranName);
	}

	public static async Task<List<ResidCompactBusiness>> GetAllAsync(string contextGuid, ResidFilter filter, CancellationToken token, string tranName = "")
	{
		_ = 2;
		try
		{
			IEnumerable<ResidCompactBusiness> list = await ClsCache.GetCache(contextGuid).CacheRepository.ResidRepository.GetAllCompactAsync(filter, token, tranName);
			if ((filter?.GroupGuid ?? Guid.Empty) != Guid.Empty)
			{
				List<Guid> validProductGuids = (await ProductGroupRelationBusiness.GetAllAsync(contextGuid, null, filter.GroupGuid.Value, token, tranName))?.Select((ProductGroupRelationBusiness q) => q.ProductGuid)?.ToList();
				if (validProductGuids?.Any() ?? false)
				{
					list = list?.Where((ResidCompactBusiness q) => validProductGuids.Contains(q.ProductGuid));
				}
			}
			return await ApplyAccessAsync(contextGuid, list, token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs", 403);
			throw;
		}
	}

	private static async Task<List<ResidCompactBusiness>> ApplyAccessAsync(string contextGuid, IEnumerable<ResidCompactBusiness> items, CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			IEnumerable<int> illegalStatus = (await (await CashierBusiness.GetCurrentCashierAsync(contextGuid, token)).GetUserAsync(token)).UserAccess.Resid.SanadStatusAccess?.Where((ResidSanadStatusAccess q) => !q.Show)?.Select((ResidSanadStatusAccess q) => q.StatusId);
			if ((items?.Any() ?? false) && (illegalStatus?.Any() ?? false))
			{
				items = items?.Where((ResidCompactBusiness q) => !illegalStatus.Contains(q.StatusId) || q.StatusId <= 0);
			}
			return items?.ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs", 420);
			throw;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(EnSystem system, IEnumerable<ResidBusiness> list, short autoFixTryCount, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (!(list?.Any() ?? false))
			{
				return ret;
			}
			foreach (ResidBusiness item in list)
			{
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs", 432);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(autoFixTryCount, null, token, system, awaitOnSemaphore: true, 10), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs", 433);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs", 438);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs", 439);
		}
		return ret;
	}

	public static Task<ResidBusiness> GetByReceptionGuidAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ResidRepository.GetByReceptionGuidAsync(guid, token, tranName);
	}

	public static Task<ResidBusiness> GetByOrderGuidAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ResidRepository.GetByOrderGuidAsync(guid, token, tranName);
	}

	private async Task<ReturnedSaveFuncInfo> CheckValidationAsync(EnSystem system, bool isDeleteAction, short autoFixTryCount, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		checked
		{
			try
			{
				ResidBusiness residBusiness = await GetAsync(ContextGuid, ID2, token, tranName);
				if (residBusiness != null && residBusiness.Guid != Guid)
				{
					if (autoFixTryCount > 0)
					{
						ID2 = await NextId2Async(ContextGuid, StatusId, token, tranName);
						autoFixTryCount--;
						ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(system, isDeleteAction, autoFixTryCount, token, tranName), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs", 459);
					}
					else
					{
						res.AddError("شماره رسید".GetTranslate() + $"{ID2}" + " " + " تکراری می باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs", 461);
					}
				}
				if (system == EnSystem.UserInterface && !isDeleteAction)
				{
					AccGlobalSettings accGlobalSettings = await LocalSettingBusiness.GetSettingAsync(ContextGuid, token);
					DateTime maliyearStart = accGlobalSettings.MaliYearSettings.MaliyearStart;
					DateTime maliyearEnd = accGlobalSettings.MaliYearSettings.MaliyearEnd;
					DateTime dateTime = DateM.AddDays(accGlobalSettings.DateAndTimePriod.ResidMaxDays);
					dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59);
					if (DateM < maliyearStart || DateM > maliyearEnd)
					{
						res.AddError(system, "تاریخ صدور رسید".GetTranslate() + "(" + DateM.MiladiToCalture() + ")\r\n" + " در بازه مجاز".GetTranslate() + "(" + maliyearStart.MiladiToCalture() + " تا " + maliyearEnd.MiladiToCalture() + ")" + " نیست.".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs", 474);
					}
					if ((Date1.Date < DateM.Date && Date1 < dateTime) || (Date1.Date > DateM.Date && Date1 > dateTime))
					{
						res.AddError(system, "حداکثر روز مجاز جهت تحویل کالا".GetTranslate() + $" {accGlobalSettings.DateAndTimePriod.ResidMaxDays}".GetTranslate() + " روز پس از تاریخ صدور".GetTranslate() + " " + DateM.MiladiToCalture() + " می باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs", 476);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs", 481);
				res.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs", 482);
			}
			return res;
		}
	}

	public static Task<List<string>> GetSuggestionListAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ResidRepository.GetSuggestionListAsync(token, tranName);
	}

	public void SetId2(int value, CancellationToken token)
	{
		ID2 = value;
	}

	public static async Task<ReturnedSaveFuncInfo> ChangeStatusIdAsync(string contextGuid, Guid guid, int statusId, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			bool autoCommit = string.IsNullOrEmpty(tranName);
			if (autoCommit)
			{
				tranName = Guid.NewGuid().ToString();
			}
			bool transactionHasBeenCreated = false;
			try
			{
				_ = 2;
				ReturnedSaveFuncInfo result;
				try
				{
					using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(true, "ResidBusiness/ChangeStatusIdAsync", 60000, token))
					{
						if (!dis.Token.IsCancellationRequested)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo;
							if (autoCommit)
							{
								returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs", 500), ReturnedState.None, "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs", 500);
							}
							transactionHasBeenCreated = true;
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.ResidRepository.ChangeStatusIdAsync(guid, statusId, token, tranName), ReturnedState.None, "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs", 502);
							goto end_IL_012c;
						}
						result = res.AddErrorTokenIsCancelled("", "", "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs", 499);
						goto end_IL_0095;
						end_IL_012c:;
					}
					goto end_IL_0084;
					end_IL_0095:;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs", 506);
					res.AddReturnedValue(ex, "", "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs", 507);
					goto end_IL_0084;
				}
				return result;
				end_IL_0084:;
			}
			finally
			{
				if (autoCommit && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs", 512), ReturnedState.None, "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ResidBusiness.cs", 512);
				}
			}
			return res;
		}
	}
}
