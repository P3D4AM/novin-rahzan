using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Business.Accounting;
using EntityCache.Interfaces.Sync;
using EntityCache.Mapping.BusinessMappers;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;
using PacketParser.Filters;

namespace EntityCache.WebService.Repository;

public class PardakhtWebServiceRepository : INpDisposable, IDisposable
{
	private bool _isDisposed;

	private ClsCache Cache;

	private TaskCompletionSource<bool> _newPaymentAvailable = new TaskCompletionSource<bool>();

	public string ContextGuid => Cache.ContextGuid;

	public WebContextHolder Context => Cache.Context;

	public bool IsDisposed => _isDisposed;

	public PardakhtWebServiceRepository(ClsCache cache)
	{
		PardakhtWebServiceRepository pardakhtWebServiceRepository = this;
		try
		{
			Cache = cache;
			cache.BufferLoggedIn += Cache_BufferLoggedIn;
			Cache.Context.WebPardakht.OnSaveResponse += PardakhtInstance_OnSaveResponse;
			Cache.Context.WebPardakht.OnSaveRequest += WebPardakht_OnSaveRequest;
			Context.LoginExtention.OnLoginResponse += LoginExtention_OnLoginResponse;
			_ = ClsCache.GetCache(ContextGuid).Source;
			if (cache.Source != ENSource.Accounting)
			{
				Task.Run(() => pardakhtWebServiceRepository.SendToServerAsync(cache.BufferCancellationToken));
				return;
			}
			ISendData2Server sendData2Server = ClsCache.GetCache(ContextGuid)?.SendData2ServerInstance?.Pardakht;
			if (sendData2Server != null)
			{
				sendData2Server.ShortMessage = "با موفقیت انجام شد.";
				sendData2Server.Status = SyncStatus.SyncedOk;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\PardakhtWebServiceRepository.cs", 49);
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
			WebErrorLog.Instance.StartLog(ex, "", "Cache_BufferLoggedIn", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\PardakhtWebServiceRepository.cs", 58);
		}
		return Task.CompletedTask;
	}

	public void ForceToSendDataToServer()
	{
		_newPaymentAvailable?.TrySetResult(result: true);
		_newPaymentAvailable = new TaskCompletionSource<bool>();
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
			WebErrorLog.Instance.StartLog(ex, "", "LoginExtention_OnLoginResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\PardakhtWebServiceRepository.cs", 77);
		}
		return Task.CompletedTask;
	}

	private async Task SendToServerAsync(CancellationToken token)
	{
		while (!IsDisposed && !token.IsCancellationRequested)
		{
			try
			{
				try
				{
					if (IsDisposed || IsDisposed || token.IsCancellationRequested)
					{
						goto end_IL_0032;
					}
					await PardakhtBusiness.ResendNotSentsAsync(ContextGuid, token, "", 20);
					goto end_IL_002c;
					end_IL_0032:;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "SendToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\PardakhtWebServiceRepository.cs", 91);
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
						_newPaymentAvailable.Task
					});
				}
				_newPaymentAvailable = new TaskCompletionSource<bool>();
			}
		}
	}

	private async Task WebPardakht_OnSaveRequest(object sender, ConnectionEventArgs<WebPardakht> e)
	{
		_ = 6;
		try
		{
			if (!(e?.ConnectionData?.Any()).GetValueOrDefault())
			{
				return;
			}
			foreach (WebPardakht pardakht in e.ConnectionData)
			{
				if (pardakht == null)
				{
					continue;
				}
				if ((e?.Connection?.IsDisposed).GetValueOrDefault(true))
				{
					break;
				}
				PardakhtBusiness pardakhtBusiness = await PardakhtMapper.Instance.MapAsync(Cache.ContextGuid, pardakht, new PardakhtBusiness(Cache.ContextGuid), "", e.Connection.cancellationToken);
				PardakhtBusiness pardakhtBusiness2 = await PardakhtBusiness.GetAsync(Cache.ContextGuid, pardakhtBusiness.Guid, fromDatabase: true);
				if (pardakhtBusiness2 != null)
				{
					List<WebDelivery> items = new List<WebDelivery>
					{
						new WebDelivery
						{
							DeliveryDate = DateTime.Now,
							DeliveryId = 10000,
							DeliveryStatus = true,
							Guid = pardakhtBusiness2.Guid,
							Message = "",
							ShortMessage = "",
							Modified = DateTime.Now,
							Part = EntityPart.Pardakht,
							Status = true
						}
					};
					Cache.SendData(WebDelivery.GenerateReadResponse(items, Guid.NewGuid()), Priority.High);
					break;
				}
				if (await CashierBusiness.GetAsync(Cache.ContextGuid, pardakhtBusiness.UserGuid) == null)
				{
					PardakhtBusiness pardakhtBusiness3 = pardakhtBusiness;
					pardakhtBusiness3.UserGuid = (await CashierBusiness.GetAllAsync(Cache.ContextGuid, true))?.FirstOrDefault()?.Guid ?? pardakhtBusiness.UserGuid;
				}
				if (string.IsNullOrEmpty(pardakhtBusiness.Description))
				{
					pardakhtBusiness.Description = "دریافت شده از سرور";
				}
				if ((pardakhtBusiness?.Naghd?.Any()).GetValueOrDefault())
				{
					foreach (PardakhtNaghdBusiness item in pardakhtBusiness.Naghd)
					{
						item.DateM = ((item.DateM < DateTime.Now.AddYears(-20)) ? pardakhtBusiness.DateM : item.DateM);
					}
				}
				pardakhtBusiness.SanadNo = 0;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = await pardakhtBusiness.SaveAsync(EnSystem.SignalR, sendToServer: false, Priority.Low, e.Connection.cancellationToken, awaitOnSemaphore: true, 10);
				if (returnedSaveFuncInfo.HasError)
				{
					await Task.Delay(new Random().Next(5000));
					returnedSaveFuncInfo = await pardakhtBusiness.SaveAsync(EnSystem.SignalR, sendToServer: false, Priority.Low, e.Connection.cancellationToken, awaitOnSemaphore: true, 10);
				}
				List<WebDelivery> items2 = new List<WebDelivery>
				{
					new WebDelivery
					{
						DeliveryDate = DateTime.Now,
						DeliveryId = 10000,
						DeliveryStatus = !returnedSaveFuncInfo.HasError,
						Guid = pardakht.Guid,
						Message = returnedSaveFuncInfo.GetErrorMessage(),
						ShortMessage = returnedSaveFuncInfo.GetErrorMessage(),
						Modified = DateTime.Now,
						Part = EntityPart.Pardakht,
						Status = true
					}
				};
				Cache.SendData(WebDelivery.GenerateReadResponse(items2, Guid.NewGuid()), Priority.High);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "WebPardakht_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\PardakhtWebServiceRepository.cs", 183);
		}
	}

	private async Task PardakhtInstance_OnSaveResponse(object sender, SaveResponseEventArgs e)
	{
		int line = 0;
		using (new JaegerMonitor("PardakhtInstance_OnSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\PardakhtWebServiceRepository.cs"))
		{
			try
			{
				line = 1;
				foreach (SaveResponse item in e.SaveResponses)
				{
					line = 3;
					WebErrorLog.Instance.Debug("Pardakht Save Response ....", "", "PardakhtInstance_OnSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\PardakhtWebServiceRepository.cs", 196);
					line = 4;
					PardakhtBusiness obj = await Cache.CacheRepository.IPardakht.GetAsync(item.Guid, e.Connection.cancellationToken, "");
					line = 5;
					obj?.SetSaveResultAsync(item, e.Connection.cancellationToken, 10);
					line = 6;
					WebErrorLog.Instance.Information("Pardakht Save Response.", "", "PardakhtInstance_OnSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\PardakhtWebServiceRepository.cs", 202);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, string.Format("The line Number Error Is{0} And IsDispose is{1}", line, IsDisposed.ToString() ?? "Null"), "PardakhtInstance_OnSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\PardakhtWebServiceRepository.cs", 205);
			}
		}
	}

	public async Task ResendAllNotSent(CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("ResendAllNotSent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\PardakhtWebServiceRepository.cs"))
		{
			_ = 3;
			try
			{
				IEnumerable<PardakhtReportBusiness> enumerable = await Cache.CacheRepository.IPardakht.GetAllAsync(new KhazaneFilter(), token, tranName);
				List<Task> tasks = new List<Task>();
				if (enumerable.Any())
				{
					foreach (PardakhtReportBusiness item in enumerable)
					{
						List<WebPardakht> list = new List<WebPardakht>();
						List<WebPardakht> list2 = list;
						PardakhtMapper instance = PardakhtMapper.Instance;
						string guid = Cache.Context.Guid;
						list2.Add(await instance.MapAsync(guid, await PardakhtBusiness.GetAsync(Cache.ContextGuid, item.Guid, fromDatabase: false), new WebPardakht(), "", token));
						List<Packet> lstPacket = WebPardakht.GenerateSaveRequest(list);
						tasks.Add(Cache.SendData(lstPacket, Priority.High));
					}
				}
				await Task.WhenAll(tasks);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ResendAllNotSent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\PardakhtWebServiceRepository.cs", 225);
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
				if (Cache != null)
				{
					Cache.BufferLoggedIn -= Cache_BufferLoggedIn;
				}
				_newPaymentAvailable?.TrySetCanceled();
				_newPaymentAvailable = null;
				Context.LoginExtention.OnLoginResponse -= LoginExtention_OnLoginResponse;
				if (Cache.Context.WebPardakht != null)
				{
					Cache.Context.WebPardakht.OnSaveResponse -= PardakhtInstance_OnSaveResponse;
					Cache.Context.WebPardakht.OnSaveRequest -= WebPardakht_OnSaveRequest;
				}
				Cache?.CacheRepository?.IPardakht.Dispose();
				if (Cache?.CacheRepository != null)
				{
					Cache.CacheRepository.IPardakht = null;
				}
				Cache = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\PardakhtWebServiceRepository.cs", 254);
		}
	}
}
