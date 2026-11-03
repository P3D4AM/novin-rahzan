using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Business.Report;
using EntityCache.Mapping.BusinessMappers;
using EntityCache.Mapping.BusinessMappers.Accounting;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Compression;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace EntityCache.WebService.Repository;

public class SyncEntityWebServiceRepository : IDisposable
{
	public ClsCache Cache { get; private set; }

	public bool IsDisposed { get; private set; }

	public SyncEntityWebServiceRepository(ClsCache cache)
	{
		try
		{
			Cache = cache;
			Cache.Context.WebEntitySync.OnReadResponse += DescInstance_OnReadResponse;
			Cache.Context.WebEntitySync.OnSaveRequest += WebEntitySync_OnSaveRequest;
			Cache.Context.WebEntitySync.OnSync += Instance_OnSync;
			cache.Context.Sync.OnSyncReset += SyncInstance_OnSyncReset;
			Cache.Context.WebEntitySync.OnSaveResponse += DescInstance_OnSaveResponse;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\SyncEntityWebServiceRepository.cs", 34);
		}
	}

	private async Task WebEntitySync_OnSaveRequest(object sender, ConnectionEventArgs<WebEntitySync> e)
	{
		using (new JaegerMonitor("WebEntitySync_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\SyncEntityWebServiceRepository.cs"))
		{
			try
			{
				ReturnedSaveFuncInfo saveResult = await SaveAsync(e);
				if (!e.Connection.cancellationToken.IsCancellationRequested)
				{
					List<Packet> lstPacket = WebEntitySync.GenerateSaveResponse(e.ConnectionData.Select((WebEntitySync p) => new SaveResponse
					{
						Date = DateTime.Now,
						Guid = p.Guid,
						SaveResultStatus = !saveResult.HasError,
						Shortmessage = saveResult.ErrorMessage,
						Message = saveResult.ErrorMessage,
						EntityPart = p.Part
					}));
					ClsCache.GetCache(e.Connection.ContextGuid).SendData(lstPacket, Priority.High);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "WebEntitySync_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\SyncEntityWebServiceRepository.cs", 58);
			}
		}
	}

	public void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				Cache.Context.WebEntitySync.OnReadResponse -= DescInstance_OnReadResponse;
				Cache.Context.WebEntitySync.OnSaveRequest -= WebEntitySync_OnSaveRequest;
				Cache.Context.WebEntitySync.OnSync -= Instance_OnSync;
				Cache.Context.Sync.OnSyncReset -= SyncInstance_OnSyncReset;
				Cache.Context.WebEntitySync.OnSaveResponse -= DescInstance_OnSaveResponse;
				Cache = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\SyncEntityWebServiceRepository.cs", 73);
		}
	}

	private async Task SyncInstance_OnSyncReset(object arg1, SyncResetEventArgs e)
	{
	}

	private async Task Instance_OnSync(object sender, ConnectionEventArgs<WebEntitySync> e)
	{
		using (new JaegerMonitor("Instance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\SyncEntityWebServiceRepository.cs"))
		{
			_ = 5;
			try
			{
				if (!(e?.ConnectionData?.Any()).GetValueOrDefault())
				{
					WebErrorLog.Instance.Warning("WebEntitySync received but data is null!", "", "Instance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\SyncEntityWebServiceRepository.cs", 131);
					return;
				}
				List<WebEntitySync> list = e.ConnectionData.ToList();
				ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
				foreach (WebEntitySync item in list)
				{
					if (e.Connection.cancellationToken.IsCancellationRequested)
					{
						return;
					}
					byte[] bytes = await GZip.Decompress(item.SerializedData);
					switch (item.Type)
					{
					case PacketType.SanadStatusReport:
					{
						WebSanadStatusReport source2 = Json.FromJson<WebSanadStatusReport>(bytes, sendErrorIfException: true, "Instance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\SyncEntityWebServiceRepository.cs", 143);
						SanadStatusReportBusiness sanadStatusReportBusiness = await SanadStatusReportMapper.Instance.MapAsync(e.Connection.ContextGuid, source2, new SanadStatusReportBusiness(e.Connection.ContextGuid));
						sanadStatusReportBusiness.ServerStatus = item.ServerStatus;
						if (sanadStatusReportBusiness.ServerStatus != ServerStatusForClient.Delivered)
						{
							throw new InvalidOperationException($"case PacketType.SanadStatusReport=>{item.Type} received from server with not delivered ststus({sanadStatusReportBusiness.ServerStatus}/{item.ServerStatus})!");
						}
						ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await sanadStatusReportBusiness.SaveAsync(EnSystem.SignalR, sendToServer: false, null, await4send: false, default(CancellationToken), 10), ReturnedState.None, "Instance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\SyncEntityWebServiceRepository.cs", 148);
						break;
					}
					case PacketType.VisitorArea:
					{
						WebVisitorArea source = Json.FromJson<WebVisitorArea>(bytes, sendErrorIfException: true, "Instance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\SyncEntityWebServiceRepository.cs", 151);
						VisitorAreaBusiness visitorAreaBusiness = await VisitorAreaMapper.Instance.MapAsync(e.Connection.ContextGuid, source, new VisitorAreaBusiness(e.Connection.ContextGuid));
						visitorAreaBusiness.ServerStatus = item.ServerStatus;
						if (visitorAreaBusiness.ServerStatus != ServerStatusForClient.Delivered)
						{
							throw new InvalidOperationException($"PacketType.VisitorArea=>{item.Type} received from server with not delivered ststus({visitorAreaBusiness.ServerStatus}/{item.ServerStatus})!\"");
						}
						ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await visitorAreaBusiness.SaveAsync(EnSystem.SignalR, default(CancellationToken), 0, send2Server: false, Priority.Low, awaitOnSemaphore: true, 10), ReturnedState.None, "Instance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\SyncEntityWebServiceRepository.cs", 156);
						break;
					}
					default:
						throw new NotImplementedException($"type({item.Type}) not defined!");
					}
				}
				if (e.Connection.cancellationToken.IsCancellationRequested)
				{
					return;
				}
				res.throwExceptionIfError();
				if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId))
				{
					Packet packet = Sync.GenerateSyncDone(EntityPart.EntitySync, e.PartitionId, res);
					if (packet == null)
					{
						throw new NullReferenceException("pack");
					}
					if (!IsDisposed && !(Cache?.IsDisposed ?? true) && Cache.BufferEnable)
					{
						await Cache.SendData(packet, Priority.High);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Instance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\SyncEntityWebServiceRepository.cs", 172);
			}
		}
	}

	private async Task DescInstance_OnReadResponse(object sender, ConnectionEventArgs<WebEntitySync> e)
	{
		using (new JaegerMonitor("DescInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\SyncEntityWebServiceRepository.cs"))
		{
			try
			{
				await SaveAsync(e);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "DescInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\SyncEntityWebServiceRepository.cs", 182);
			}
		}
	}

	private async Task<ReturnedSaveFuncInfo> SaveAsync(ConnectionEventArgs<WebEntitySync> e)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (!(e?.ConnectionData?.Any()).GetValueOrDefault())
			{
				WebErrorLog.Instance.Warning("WebEntitySync received but data is null!", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\SyncEntityWebServiceRepository.cs", 192);
				return ret;
			}
			List<WebEntitySync> list = e.ConnectionData.ToList();
			_ = Cache.Source;
			foreach (WebEntitySync item in list)
			{
				if (e.Connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\SyncEntityWebServiceRepository.cs", 199);
				}
				byte[] bytes = await GZip.Decompress(item.SerializedData);
				switch (item.Type)
				{
				case PacketType.SanadStatusReport:
				{
					WebSanadStatusReport source2 = Json.FromJson<WebSanadStatusReport>(bytes, sendErrorIfException: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\SyncEntityWebServiceRepository.cs", 204);
					SanadStatusReportBusiness obj2 = await SanadStatusReportMapper.Instance.MapAsync(e.Connection.ContextGuid, source2, new SanadStatusReportBusiness(e.Connection.ContextGuid));
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await obj2.SaveAsync(EnSystem.SignalR, sendToServer: false, null, await4send: false, default(CancellationToken), 10), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\SyncEntityWebServiceRepository.cs", 206);
					break;
				}
				case PacketType.VisitorArea:
				{
					WebVisitorArea source = Json.FromJson<WebVisitorArea>(bytes, sendErrorIfException: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\SyncEntityWebServiceRepository.cs", 209);
					VisitorAreaBusiness obj = await VisitorAreaMapper.Instance.MapAsync(e.Connection.ContextGuid, source, new VisitorAreaBusiness(e.Connection.ContextGuid));
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await obj.SaveAsync(EnSystem.SignalR, default(CancellationToken), 0, send2Server: false, Priority.Low, awaitOnSemaphore: true, 10), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\SyncEntityWebServiceRepository.cs", 211);
					break;
				}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\SyncEntityWebServiceRepository.cs", 218);
			ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\SyncEntityWebServiceRepository.cs", 219);
		}
		return ret;
	}

	private async Task DescInstance_OnSaveResponse(object sender, SaveResponseEventArgs e)
	{
		using (new JaegerMonitor("DescInstance_OnSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\SyncEntityWebServiceRepository.cs"))
		{
			try
			{
				if (e == null || !(e.SaveResponses?.Count > 0))
				{
					return;
				}
				foreach (SaveResponse saveResponse in e.SaveResponses)
				{
					await SetSaveResponseAsync(saveResponse, e.Connection.cancellationToken);
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "DescInstance_OnSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\SyncEntityWebServiceRepository.cs", 234);
			}
		}
	}

	private async Task SetSaveResponseAsync(SaveResponse resopnse, CancellationToken token)
	{
		_ = 7;
		try
		{
			if (resopnse == null)
			{
				return;
			}
			switch (resopnse.EntityPart)
			{
			case EntityPart.SanadStatusReport:
				await (await SanadStatusReportBusiness.GetAsync(Cache.ContextGuid, resopnse.Guid)).SetSaveResultAsync(resopnse, token, 10);
				break;
			case EntityPart.VisitorArea:
				await (await VisitorAreaBusiness.GetAsync(Cache.ContextGuid, resopnse.Guid)).SetSaveResultAsync(resopnse, token, 10);
				break;
			case EntityPart.None:
			{
				SanadStatusReportBusiness sanadStatusReportBusiness = await SanadStatusReportBusiness.GetAsync(Cache.ContextGuid, resopnse.Guid);
				if (sanadStatusReportBusiness != null)
				{
					await sanadStatusReportBusiness.SetSaveResultAsync(resopnse, token, 10);
					break;
				}
				VisitorAreaBusiness visitorAreaBusiness = await VisitorAreaBusiness.GetAsync(Cache.ContextGuid, resopnse.Guid);
				if (visitorAreaBusiness != null)
				{
					await visitorAreaBusiness.SetSaveResultAsync(resopnse, token, 10);
				}
				break;
			}
			default:
				throw new NotImplementedException($"EntityPart({resopnse.EntityPart}) is not valid!");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetSaveResponseAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\SyncEntityWebServiceRepository.cs", 268);
		}
	}
}
