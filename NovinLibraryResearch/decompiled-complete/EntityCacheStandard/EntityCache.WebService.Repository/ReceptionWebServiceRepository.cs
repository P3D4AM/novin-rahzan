using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Business.Accounting;
using EntityCache.Core.Repository;
using EntityCache.Interfaces.Sync;
using EntityCache.Mapping.BusinessMappers;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.MetaData;
using PacketParser.EventArgs;
using PacketParser.Filters;
using PacketParser.Settings.Accounting;

namespace EntityCache.WebService.Repository;

public class ReceptionWebServiceRepository : INpDisposable, IDisposable
{
	private bool _isDisposed;

	private ClsCache Cache;

	private TaskCompletionSource<bool> _newReceptionAvailable = new TaskCompletionSource<bool>();

	public string ContextGuid => Cache.ContextGuid;

	public WebContextHolder Context => Cache.Context;

	public bool IsDisposed => _isDisposed;

	public ReceptionWebServiceRepository(ClsCache cache)
	{
		ReceptionWebServiceRepository receptionWebServiceRepository = this;
		try
		{
			Cache = cache;
			cache.BufferLoggedIn += Cache_BufferLoggedIn;
			Cache.Context.WebReception.OnSaveResponse += ReceptionInstance_OnSaveResponse;
			Cache.Context.WebReception.OnSaveRequest += WebReception_OnSaveRequest;
			Context.LoginExtention.OnLoginResponse += LoginExtention_OnLoginResponse;
			if (cache.Source != ENSource.Accounting)
			{
				Task.Run(() => receptionWebServiceRepository.SendToServerAsync(cache.BufferCancellationToken));
				return;
			}
			ISendData2Server sendData2Server = ClsCache.GetCache(ContextGuid)?.SendData2ServerInstance?.Reception;
			if (sendData2Server != null)
			{
				sendData2Server.ShortMessage = "با موفقیت انجام شد.";
				sendData2Server.Status = SyncStatus.SyncedOk;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ReceptionWebServiceRepository.cs", 47);
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
			WebErrorLog.Instance.StartLog(ex, "", "Cache_BufferLoggedIn", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ReceptionWebServiceRepository.cs", 56);
		}
		return Task.CompletedTask;
	}

	public void ForceToSendDataToServer()
	{
		try
		{
			if (!(_newReceptionAvailable?.Task?.IsCompleted).GetValueOrDefault())
			{
				_newReceptionAvailable?.TrySetResult(result: true);
			}
			_newReceptionAvailable = new TaskCompletionSource<bool>();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ForceToSendDataToServer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ReceptionWebServiceRepository.cs", 69);
		}
	}

	private Task LoginExtention_OnLoginResponse(object sender, LoginResponseEventArgs e)
	{
		try
		{
			ENSource source = ClsCache.GetCache(ContextGuid).Source;
			if (IsDisposed || source == ENSource.Accounting)
			{
				return Task.CompletedTask;
			}
			CancellationToken token = e.connection.cancellationToken;
			Task.Run(() => SendToServerAsync(token));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoginExtention_OnLoginResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ReceptionWebServiceRepository.cs", 81);
		}
		return Task.CompletedTask;
	}

	private async Task SendToServerAsync(CancellationToken token)
	{
		while (!IsDisposed && !token.IsCancellationRequested)
		{
			Stopwatch.StartNew();
			try
			{
				try
				{
					if (IsDisposed || token.IsCancellationRequested)
					{
						goto end_IL_0038;
					}
					await ReceptionBusiness.ResendNotSentsAsync(ContextGuid, token, "", 20);
					goto end_IL_0032;
					end_IL_0038:;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "SendToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ReceptionWebServiceRepository.cs", 95);
					goto end_IL_0032;
				}
				break;
				end_IL_0032:;
			}
			finally
			{
				if (!IsDisposed && !token.IsCancellationRequested)
				{
					await Task.WhenAny(new Task[2]
					{
						Task.Delay(60000),
						_newReceptionAvailable.Task
					});
					_newReceptionAvailable = new TaskCompletionSource<bool>();
				}
			}
		}
	}

	private async Task WebReception_OnSaveRequest(object sender, ConnectionEventArgs<WebReception> e)
	{
		_ = 12;
		try
		{
			if (!(e?.ConnectionData?.Any()).GetValueOrDefault())
			{
				return;
			}
			foreach (WebReception reception in e.ConnectionData)
			{
				if (reception == null)
				{
					continue;
				}
				if ((e?.Connection?.IsDisposed).GetValueOrDefault(true))
				{
					break;
				}
				ReceptionBusiness receptionBusiness = await ReceptionMapper.Instance.MapAsync(Cache.ContextGuid, reception, new ReceptionBusiness(Cache.ContextGuid));
				receptionBusiness.SanadNo = 0;
				ReceptionBusiness receptionBusiness2 = await ReceptionBusiness.GetAsync(Cache.ContextGuid, receptionBusiness.Guid, EnSystem.SignalR, null, fromDatabase: true);
				if (receptionBusiness2 != null)
				{
					List<WebDelivery> items = new List<WebDelivery>
					{
						new WebDelivery
						{
							DeliveryDate = DateTime.Now,
							DeliveryId = 10000,
							DeliveryStatus = true,
							Guid = receptionBusiness2.Guid,
							Message = "",
							ShortMessage = "",
							Modified = DateTime.Now,
							Part = EntityPart.Reception,
							Status = true
						}
					};
					Cache.SendData(WebDelivery.GenerateReadResponse(items, Guid.NewGuid()), Priority.High);
					break;
				}
				CashierBusiness cashier = await CashierBusiness.GetAsync(Cache.ContextGuid, receptionBusiness.UserGuid);
				if (cashier == null)
				{
					ReceptionBusiness receptionBusiness3 = receptionBusiness;
					receptionBusiness3.UserGuid = (await CashierBusiness.GetAllAsync(Cache.ContextGuid, true))?.FirstOrDefault()?.Guid ?? receptionBusiness.UserGuid;
					cashier = await CashierBusiness.GetAsync(Cache.ContextGuid, receptionBusiness.UserGuid);
				}
				if (receptionBusiness.ListCheck?.Any() ?? false)
				{
					foreach (ReceptionCheckBusiness item in receptionBusiness.ListCheck)
					{
						ReceptionCheckBusiness receptionCheckBusiness = await ReceptionCheckBusiness.GetAsync(Cache.ContextGuid, item.Guid);
						if (receptionCheckBusiness == null)
						{
							item.Status = ENUMCheck.Mojod;
						}
						else
						{
							item.Status = receptionCheckBusiness.Status;
						}
						ReceptionCheckBusiness receptionCheckBusiness2 = item;
						if (receptionCheckBusiness2.MetaData == null)
						{
							receptionCheckBusiness2.MetaData = new ReceptionChequeMetaData();
						}
						if (item.MetaData.ChequeType == EnChequeType.Traditional && !string.IsNullOrEmpty(item.SayadiIdentity))
						{
							item.MetaData.ChequeType = EnChequeType.Sayadi;
						}
						if ((item?.VisitorGuid ?? Guid.Empty) == Guid.Empty)
						{
							item.VisitorGuid = cashier.VisitorGuid;
						}
					}
				}
				NPSMS_WebServiceSettings nPSMS_WebServiceSettings = await LocalSettingBusiness.GetWebServiceSettingsAsync(ContextGuid, e.Connection.cancellationToken);
				if ((receptionBusiness.ListBank?.Any() ?? false) && (nPSMS_WebServiceSettings.DefaultBankGuid ?? Guid.Empty) != Guid.Empty)
				{
					foreach (ReceptionHavaleBusiness item2 in receptionBusiness.ListBank)
					{
						if (!((item2?.TafsilGuid ?? Guid.Empty) != Guid.Empty))
						{
							item2.TafsilGuid = nPSMS_WebServiceSettings.DefaultBankGuid.Value;
						}
					}
				}
				ReturnedSaveFuncInfo savereceptionResult = await receptionBusiness.SaveAsync(EnSystem.SignalR, sendToServer: false, Priority.Low, awaitOnSemaphore: true, "", "", 60000, await4send: false, 10);
				if (savereceptionResult.HasError)
				{
					await Task.Delay(new Random().Next(5000));
					savereceptionResult = await receptionBusiness.SaveAsync(EnSystem.SignalR, sendToServer: false, Priority.Low, awaitOnSemaphore: true, "", "", 60000, await4send: false, 10);
				}
				if (!savereceptionResult.HasError && (reception?.OrderGuid ?? Guid.Empty) != Guid.Empty)
				{
					OrderBusiness order = await OrderBusiness.GetAsync(ContextGuid, reception.OrderGuid.Value, reception.OrderType.Value, fromDatabase: true, e.Connection.cancellationToken);
					if (order != null)
					{
						if (!(reception.Sum <= 0m))
						{
							await order.AddToLinkedSanadsAsync(reception.Guid, Document.Reception, reception.Sum);
						}
						else
						{
							order.RemoveFromLinkedSanad(reception.Guid, Document.Reception);
						}
						ReturnedSaveFuncInfo returnedSaveFuncInfo = savereceptionResult;
						returnedSaveFuncInfo.AddReturnedValue(await order.SaveAsync(EnSystem.SignalR, awaitOnSemaphore: true, sendToServer: false, isTemp: false, 10, "", "", "", 60000, await4sent: false, e.Connection.cancellationToken), ReturnedState.None, "WebReception_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ReceptionWebServiceRepository.cs", 181);
					}
				}
				List<WebDelivery> items2 = new List<WebDelivery>
				{
					new WebDelivery
					{
						DeliveryDate = DateTime.Now,
						DeliveryId = 10000,
						DeliveryStatus = !savereceptionResult.HasError,
						Guid = reception.Guid,
						Message = savereceptionResult.GetErrorMessage(),
						ShortMessage = savereceptionResult.GetErrorMessage(),
						Modified = DateTime.Now,
						Part = EntityPart.Reception,
						Status = true
					}
				};
				Cache.SendData(WebDelivery.GenerateReadResponse(items2, e.PacketReferenceId), Priority.High);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "WebReception_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ReceptionWebServiceRepository.cs", 200);
		}
	}

	private async Task ReceptionInstance_OnSaveResponse(object sender, SaveResponseEventArgs e)
	{
		using (new JaegerMonitor("ReceptionInstance_OnSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ReceptionWebServiceRepository.cs"))
		{
			try
			{
				foreach (SaveResponse item in e.SaveResponses)
				{
					(await Cache.CacheRepository.IReception.GetAsync(item.Guid, e.Connection.cancellationToken, ""))?.SetSaveResultAsync(item, e.Connection.cancellationToken, 10);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ReceptionInstance_OnSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ReceptionWebServiceRepository.cs", 213);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> AddAndSend(string contextGuid, WebReception item, string CustomerUserName, string CustomerPassword, short tryCount, string tranName, CancellationToken token)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("AddAndSend", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ReceptionWebServiceRepository.cs"))
		{
			_ = 3;
			try
			{
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "AddAndSend", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ReceptionWebServiceRepository.cs", 221);
				}
				IReceptionRepository iReception = Cache.CacheRepository.IReception;
				await iReception.SaveAsync(await ReceptionMapper.Instance.MapAsync(contextGuid, item, new ReceptionBusiness(contextGuid), "", token), tryCount, token, tranName);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "AddAndSend", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ReceptionWebServiceRepository.cs", 223);
				}
				List<Packet> list = WebReception.GenerateSaveRequest(new List<WebReception> { item });
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "AddAndSend", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ReceptionWebServiceRepository.cs", 225);
				}
				if (string.IsNullOrEmpty(CustomerUserName))
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Cache.SendData(list, Priority.High), ReturnedState.None, "AddAndSend", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ReceptionWebServiceRepository.cs", 227);
				}
				else
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await CustomerBuffer.Send(list, CustomerUserName, Cache.Context, CustomerPassword, Cache.BufferConnectionType, token), ReturnedState.None, "AddAndSend", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ReceptionWebServiceRepository.cs", 229);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "AddAndSend", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ReceptionWebServiceRepository.cs", 231);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AddAndSend", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ReceptionWebServiceRepository.cs", 232);
				ret.AddReturnedValue(ex, "", "AddAndSend", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ReceptionWebServiceRepository.cs", 232);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> Send(WebReception item, CancellationToken token, string CustomerUserName = "", string CustomerPassword = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ReceptionWebServiceRepository.cs"))
		{
			_ = 1;
			try
			{
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ReceptionWebServiceRepository.cs", 241);
				}
				List<Packet> list = WebReception.GenerateSaveRequest(new List<WebReception> { item });
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ReceptionWebServiceRepository.cs", 243);
				}
				if (!string.IsNullOrEmpty(CustomerUserName))
				{
					await CustomerBuffer.Send(list, CustomerUserName, Cache.Context, CustomerPassword, Cache.BufferConnectionType, token);
				}
				else
				{
					await Cache.SendData(list, Priority.High);
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ReceptionWebServiceRepository.cs", 248);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ReceptionWebServiceRepository.cs", 250);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ReceptionWebServiceRepository.cs", 251);
				ret.AddReturnedValue(ex, "", "Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ReceptionWebServiceRepository.cs", 251);
			}
			return ret;
		}
	}

	public async Task ResendAllNotSent(CancellationToken token)
	{
		using (new JaegerMonitor("ResendAllNotSent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ReceptionWebServiceRepository.cs"))
		{
			_ = 3;
			try
			{
				IEnumerable<ReceptionReportBusiness> enumerable = await ReceptionBusiness.GetAllAsync(Cache.Context.Guid, EnSystem.SignalR, new KhazaneFilter());
				List<Task> Tasks = new List<Task>();
				if (enumerable != null && enumerable.Any())
				{
					foreach (ReceptionReportBusiness item in enumerable)
					{
						ReceptionBusiness source = await ReceptionBusiness.GetAsync(Cache.ContextGuid, item.Guid, EnSystem.SignalR);
						List<WebReception> list = new List<WebReception>();
						List<WebReception> list2 = list;
						list2.Add(await ReceptionMapper.Instance.MapAsync(Cache.Context.Guid, source, new WebReception(), "", token));
						List<Packet> lstPacket = WebReception.GenerateSaveRequest(list);
						Tasks.Add(Cache.SendData(lstPacket, Priority.High));
					}
				}
				await Task.WhenAll(Tasks);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ResendAllNotSent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ReceptionWebServiceRepository.cs", 270);
			}
		}
	}

	public void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_newReceptionAvailable?.TrySetCanceled();
				_newReceptionAvailable = null;
				if (Cache != null)
				{
					Cache.BufferLoggedIn -= Cache_BufferLoggedIn;
				}
				Context.LoginExtention.OnLoginResponse -= LoginExtention_OnLoginResponse;
				Cache.Context.WebReception.OnSaveResponse -= ReceptionInstance_OnSaveResponse;
				Cache.Context.WebReception.OnSaveRequest -= WebReception_OnSaveRequest;
				Cache.CacheRepository?.IReception?.Dispose();
				if (Cache.CacheRepository != null)
				{
					Cache.CacheRepository.IReception = null;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ReceptionWebServiceRepository.cs", 292);
		}
	}
}
