using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Business.Accounting;
using EntityCache.EventArguments;
using EntityCache.Interfaces;
using EntityCache.Interfaces.Sync;
using EntityCache.Mapping.BusinessMappers.OrderDepot;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;
using PacketParser.Settings.Accounting;
using Serilog;

namespace EntityCache.WebService.Repository;

public class OrderWebServiceRepository : IDisposable
{
	private ClsCache Cache;

	private bool _isDisposed;

	private TaskCompletionSource<bool> _newOrderAvailable = new TaskCompletionSource<bool>();

	public string ContextGuid => Cache.Context.Guid;

	public bool IsDisposed => _isDisposed;

	public event EventHandler<CountChangedEventArgs> PishfactorCountChanged;

	public event EventHandler<CountChangedEventArgs> SellCountChanged;

	public event EventHandler<CountChangedEventArgs> BackSellCountChanged;

	public event EventHandler<CountChangedEventArgs> BuyCountChanged;

	public event EventHandler<CountChangedEventArgs> BackBuyCountChanged;

	public OrderWebServiceRepository(ClsCache cache)
	{
		OrderWebServiceRepository orderWebServiceRepository = this;
		try
		{
			Cache = cache;
			cache.BufferLoggedIn += Cache_BufferLoggedIn;
			Cache.Context.WebOrder.OnSaveResponse += FactorInstance_OnSaveResponse;
			Cache.Context.WebOrder.OnSaveRequest += WebOrder_OnSaveRequest;
			if (cache.Source != ENSource.Accounting)
			{
				Task.Run(() => orderWebServiceRepository.SendToServerAsync(cache.BufferCancellationToken));
				return;
			}
			ISendData2Server sendData2Server = ClsCache.GetCache(ContextGuid)?.SendData2ServerInstance?.Order;
			if (sendData2Server != null)
			{
				sendData2Server.ShortMessage = "با موفقیت انجام شد.".GetTranslate();
				sendData2Server.Status = SyncStatus.SyncedOk;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\OrderWebServiceRepository.cs", 56);
		}
	}

	private Task Cache_BufferLoggedIn(object arg1, LoginResponseEventArgs arg2)
	{
		try
		{
			ForceToSendDataToServer();
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return Task.CompletedTask;
	}

	public void ForceToSendDataToServer()
	{
		_newOrderAvailable?.TrySetResult(result: true);
		_newOrderAvailable = new TaskCompletionSource<bool>();
	}

	private async Task SendToServerAsync(CancellationToken token)
	{
		while (!IsDisposed && !token.IsCancellationRequested)
		{
			try
			{
				try
				{
					if (IsDisposed || token.IsCancellationRequested)
					{
						goto end_IL_0032;
					}
					await OrderBusiness.ResendNotSentsAsync(ContextGuid, token, "", 5);
					goto end_IL_002c;
					end_IL_0032:;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "SendToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\OrderWebServiceRepository.cs", 81);
					goto end_IL_002c;
				}
				break;
				end_IL_002c:;
			}
			finally
			{
				if (!IsDisposed && !token.IsCancellationRequested)
				{
					await Task.WhenAny(new Task[2]
					{
						Task.Delay(60000),
						_newOrderAvailable.Task
					});
				}
				_newOrderAvailable = new TaskCompletionSource<bool>();
			}
		}
	}

	private Task WebOrder_OnSaveRequest(object sender, ConnectionEventArgs<WebOrder> e)
	{
		return SaveOrder(sender, Cache, e.ConnectionData, e.Connection.cancellationToken);
	}

	public static async Task SaveOrder(object sender, ClsCache Cache, List<WebOrder> orders, CancellationToken token)
	{
		string debugInfo = "";
		try
		{
			if (!(orders?.Any() ?? false))
			{
				return;
			}
			IUiNotificationFactory notif = Cache?.NotificationAggregator?.Notification;
			CancellationTokenSource displayMessageToken = CancellationTokenSource.CreateLinkedTokenSource(new CancellationToken[1] { token });
			NPSMS_WebServiceSettings setting = await LocalSettingBusiness.GetWebServiceSettingsAsync(Cache.ContextGuid, token);
			foreach (WebOrder order in orders)
			{
				token.ThrowIfCancellationRequested();
				if (order.Reception != null)
				{
					if ((order.ReceptionGuid ?? Guid.Empty) != Guid.Empty && order.Reception.Guid == Guid.Empty)
					{
						order.Reception.Guid = order.ReceptionGuid.Value;
					}
					if (order.Reception.Guid == Guid.Empty)
					{
						order.Reception.Guid = Guid.NewGuid();
					}
					if ((order.ReceptionGuid ?? Guid.Empty) == Guid.Empty && order.Reception.Guid != Guid.Empty)
					{
						order.ReceptionGuid = order.Reception.Guid;
					}
				}
				if (order.Pardakht != null)
				{
					if ((order.PardakhtGuid ?? Guid.Empty) != Guid.Empty && order.Pardakht.Guid == Guid.Empty)
					{
						order.Pardakht.Guid = order.PardakhtGuid.Value;
					}
					if (order.Pardakht.Guid == Guid.Empty)
					{
						order.Pardakht.Guid = Guid.NewGuid();
					}
					if ((order.PardakhtGuid ?? Guid.Empty) == Guid.Empty && order.Pardakht.Guid != Guid.Empty)
					{
						order.PardakhtGuid = order.Pardakht.Guid;
					}
				}
				if (order == null)
				{
					continue;
				}
				if (setting?.ShowSignalRNotif ?? false)
				{
					notif?.DisplayAsync("در حال ذخیره".GetTranslate() + "  " + order.FactorTypeFa, order.FactorType.GetDisplay().GetTranslate() + " \r\n" + "جمع کل".GetTranslate() + ":" + $" {order.Sum:N0} \r\n" + "جمع دریافت".GetTranslate() + ":" + $" {(order?.Reception?.Sum).GetValueOrDefault(-1m)}", ReturnedState.Information, displayMessageToken.Token);
				}
				debugInfo = $"orderCashierGuid:{order.UserAccGuid}\r\n" + $"OrderGuid:{order.Guid}\r\n" + $"orderType:{order.FactorType}\r\n" + $"OrderPrice:{order.Sum}\r\n" + $"ReceptionSum:{(order?.Reception?.Sum).GetValueOrDefault(-1m)}\r\n" + $"ReceptionCashierGuid:{order?.Reception?.UserGuid}";
				OrderBusiness oldOrder = await OrderBusiness.GetAsync(Cache.ContextGuid, order.Guid, order.FactorType, fromDatabase: true);
				if (oldOrder != null)
				{
					List<WebDelivery> items = new List<WebDelivery>
					{
						new WebDelivery
						{
							DeliveryDate = DateTime.Now,
							DeliveryId = 10000,
							DeliveryStatus = true,
							Guid = order.Guid,
							Message = "",
							ShortMessage = "",
							Modified = DateTime.Now,
							Part = EntityPart.Order,
							Status = true
						}
					};
					Cache.SendData(WebDelivery.GenerateReadResponse(items, Guid.NewGuid()), Priority.High);
					return;
				}
				OrderBusiness destination = new OrderBusiness(Cache.ContextGuid);
				OrderBusiness orderBusiness = await OrderMapper.Instance.MapAsync(Cache.ContextGuid, order, destination, "", token);
				destination.ChangeStatus = ((oldOrder == null) ? EntityChangeSt.New : EntityChangeSt.Edited);
				if (order.Reception != null)
				{
					ReceptionBusiness receptionBusiness = await destination.GetReceptionAsync(EnSystem.SignalR, "", fromDatabase: true);
					receptionBusiness.SanadNo = 0;
					if ((receptionBusiness.ListBank?.Any() ?? false) && (setting.DefaultBankGuid ?? Guid.Empty) != Guid.Empty)
					{
						foreach (ReceptionHavaleBusiness item in receptionBusiness.ListBank)
						{
							if (!((item?.TafsilGuid ?? Guid.Empty) != Guid.Empty))
							{
								item.TafsilGuid = setting.DefaultBankGuid.Value;
							}
						}
					}
					ReceptionBusiness receptionBusiness2 = await ReceptionBusiness.GetAsync(Cache.ContextGuid, receptionBusiness.Guid, EnSystem.SignalR, null, fromDatabase: true);
					if (receptionBusiness2 != null)
					{
						if (receptionBusiness2.ListBank?.Any() ?? false)
						{
							foreach (ReceptionHavaleBusiness havale2 in receptionBusiness2.ListBank)
							{
								ReceptionHavaleBusiness receptionHavaleBusiness = receptionBusiness.ListBank?.SingleOrDefault((ReceptionHavaleBusiness q) => q.Guid == havale2.Guid);
								if (receptionHavaleBusiness != null && receptionHavaleBusiness.MetaData != null && havale2.MetaData == null)
								{
									havale2.MetaData = receptionHavaleBusiness.MetaData;
								}
							}
						}
						if (receptionBusiness2.ListCheck?.Any() ?? false)
						{
							foreach (ReceptionCheckBusiness cheque2 in receptionBusiness2.ListCheck)
							{
								ReceptionCheckBusiness receptionCheckBusiness = receptionBusiness.ListCheck?.SingleOrDefault((ReceptionCheckBusiness q) => q.Guid == cheque2.Guid);
								if (receptionCheckBusiness != null && receptionCheckBusiness.MetaData != null && cheque2.MetaData == null)
								{
									cheque2.MetaData = receptionCheckBusiness.MetaData;
								}
							}
						}
					}
				}
				if (order.Pardakht != null)
				{
					PardakhtBusiness pardakhtBusiness = await destination.GetPardakhtAsync(EnSystem.SignalR);
					pardakhtBusiness.SanadNo = 0;
					PardakhtBusiness pardakhtBusiness2 = await PardakhtBusiness.GetAsync(Cache.ContextGuid, pardakhtBusiness.Guid, fromDatabase: true);
					if (pardakhtBusiness2 != null)
					{
						if (pardakhtBusiness2.Bank?.Any() ?? false)
						{
							foreach (PardakhtBankBusiness havale in pardakhtBusiness2.Bank)
							{
								PardakhtBankBusiness pardakhtBankBusiness = pardakhtBusiness.Bank?.SingleOrDefault((PardakhtBankBusiness q) => q.Guid == havale.Guid);
								if (pardakhtBankBusiness != null && pardakhtBankBusiness.MetaData != null && havale.MetaData == null)
								{
									havale.MetaData = pardakhtBankBusiness.MetaData;
								}
							}
						}
						if (pardakhtBusiness2.CheckSh?.Any() ?? false)
						{
							foreach (PardakhtCheckShBusiness cheque in pardakhtBusiness2.CheckSh)
							{
								PardakhtCheckShBusiness pardakhtCheckShBusiness = pardakhtBusiness.CheckSh?.SingleOrDefault((PardakhtCheckShBusiness q) => q.Guid == cheque.Guid);
								if (pardakhtCheckShBusiness != null && pardakhtCheckShBusiness.MetaData != null && cheque.MetaData == null)
								{
									cheque.MetaData = pardakhtCheckShBusiness.MetaData;
								}
							}
						}
					}
				}
				orderBusiness.SanadNo = 0;
				if (await CashierBusiness.GetAsync(Cache.ContextGuid, orderBusiness.UserAccGuid) == null)
				{
					OrderBusiness orderBusiness2 = orderBusiness;
					orderBusiness2.UserAccGuid = (await CashierBusiness.GetAllAsync(Cache.ContextGuid, true))?.FirstOrDefault()?.Guid ?? orderBusiness.UserAccGuid;
				}
				ReturnedSaveFuncInfo saveOrderResult = await orderBusiness.SaveAsync(EnSystem.SignalR, awaitOnSemaphore: true, sendToServer: false, isTemp: false, 10);
				if (saveOrderResult.HasError)
				{
					await Task.Delay(new Random().Next(5000));
					saveOrderResult = await orderBusiness.SaveAsync(EnSystem.SignalR, awaitOnSemaphore: true, sendToServer: false, isTemp: false, 10);
				}
				await Task.Delay(100);
				displayMessageToken?.Cancel();
				displayMessageToken = CancellationTokenSource.CreateLinkedTokenSource(new CancellationToken[1] { token });
				if (saveOrderResult.HasError && (setting?.ShowSignalRNotif ?? false))
				{
					notif?.DisplayAsync("خطا در ثبت".GetTranslate() + "  " + order.FactorTypeFa, order.FactorType.GetDisplay().GetTranslate() + " \r\n" + saveOrderResult.GetErrorMessage(), ReturnedState.Error, displayMessageToken.Token);
				}
				else
				{
					CusCompactBusiness cusCompactBusiness = await orderBusiness.GetCusInfoAsync(EnSystem.SignalR);
					if (setting?.ShowSignalRNotif ?? false)
					{
						notif?.DisplayAsync(order.FactorTypeFa + " " + "با موفقیت ثبت شد".GetTranslate(), order.FactorType.GetDisplay().GetTranslate() + " \r\n" + cusCompactBusiness?.FullName, ReturnedState.Success, displayMessageToken.Token);
					}
				}
				List<WebDelivery> items2 = new List<WebDelivery>
				{
					new WebDelivery
					{
						DeliveryDate = DateTime.Now,
						DeliveryId = 10000,
						DeliveryStatus = !saveOrderResult.HasError,
						Guid = order.Guid,
						Message = saveOrderResult.GetErrorMessage(),
						ShortMessage = saveOrderResult.GetErrorMessage(),
						Modified = DateTime.Now,
						Part = EntityPart.Order,
						Status = true
					}
				};
				Cache.SendData(WebDelivery.GenerateReadResponse(items2, Guid.NewGuid()), Priority.High);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, debugInfo, "SaveOrder", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\OrderWebServiceRepository.cs", 274);
		}
	}

	public void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				if (Cache != null)
				{
					Cache.BufferLoggedIn -= Cache_BufferLoggedIn;
				}
				_newOrderAvailable?.TrySetCanceled();
				_newOrderAvailable = null;
				Cache.Context.WebOrder.OnSaveResponse -= FactorInstance_OnSaveResponse;
				Cache.Context.WebOrder.OnSaveRequest -= WebOrder_OnSaveRequest;
				Cache = null;
				this.PishfactorCountChanged = null;
				this.SellCountChanged = null;
				this.BackSellCountChanged = null;
				this.BuyCountChanged = null;
				this.BackBuyCountChanged = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\OrderWebServiceRepository.cs", 298);
		}
	}

	private async Task FactorInstance_OnSaveResponse(object sender, SaveResponseEventArgs e)
	{
		int line = 0;
		try
		{
			line = 1;
			if (e.SaveResponses == null)
			{
				return;
			}
			line = 2;
			if (e.SaveResponses.Count == 0)
			{
				return;
			}
			line = 3;
			line = 4;
			if (e?.SaveResponses != null)
			{
				foreach (SaveResponse item in e.SaveResponses)
				{
					if (item != null)
					{
						line = 5;
						OrderBusiness orderBusiness = await Cache.CacheRepository.IOrder.GetAsync(item.Guid, e.Connection.cancellationToken, "");
						line = 6;
						if (orderBusiness != null)
						{
							await orderBusiness.SetSaveResult(EnSystem.SignalR, item, e.Connection.cancellationToken, 10);
						}
						line = 7;
					}
				}
			}
			line = 8;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "FactorInstance_OnSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\OrderWebServiceRepository.cs", 326);
		}
	}

	public async Task<ReturnedSaveFuncInfo> SendTempByCustomer(WebOrder item, string customerUsername, string customerPassword, CancellationToken token)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("SendTempByCustomer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\OrderWebServiceRepository.cs"))
		{
			try
			{
				List<Packet> packets = WebOrder.GenerateSaveTempRequest(new List<WebOrder> { item });
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendTempByCustomer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\OrderWebServiceRepository.cs", 335);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await CustomerBuffer.Send(packets, customerUsername, Cache.Context, customerPassword, Cache.BufferConnectionType, token), ReturnedState.None, "SendTempByCustomer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\OrderWebServiceRepository.cs", 337);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SendTempByCustomer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\OrderWebServiceRepository.cs", 339);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SendTempByCustomer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\OrderWebServiceRepository.cs", 340);
				ret.AddReturnedValue(ex, "", "SendTempByCustomer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\OrderWebServiceRepository.cs", 340);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SendByCustomer(WebOrder item, WebContextHolder context, string customerUsername, string customerPassword, CancellationToken token)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("SendByCustomer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\OrderWebServiceRepository.cs"))
		{
			try
			{
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendByCustomer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\OrderWebServiceRepository.cs", 349);
				}
				List<Packet> packets = WebOrder.GenerateSaveRequest(new List<WebOrder> { item });
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendByCustomer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\OrderWebServiceRepository.cs", 351);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await CustomerBuffer.Send(packets, customerUsername, context, customerPassword, Cache.BufferConnectionType, token), ReturnedState.None, "SendByCustomer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\OrderWebServiceRepository.cs", 352);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SendByCustomer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\OrderWebServiceRepository.cs", 354);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SendByCustomer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\OrderWebServiceRepository.cs", 355);
				ret.AddReturnedValue(ex, "", "SendByCustomer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\OrderWebServiceRepository.cs", 355);
			}
			return ret;
		}
	}

	public async Task Send(WebOrder item)
	{
		using (new JaegerMonitor("Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\OrderWebServiceRepository.cs"))
		{
			try
			{
				List<Packet> lstPacket = WebOrder.GenerateSaveRequest(new List<WebOrder> { item });
				await Cache.SendData(lstPacket, Priority.High);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\OrderWebServiceRepository.cs", 366);
			}
		}
	}
}
