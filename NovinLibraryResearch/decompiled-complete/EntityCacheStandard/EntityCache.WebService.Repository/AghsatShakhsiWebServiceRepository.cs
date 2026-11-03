using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Aghsat;
using EntityCache.Core;
using EntityCache.Mapping.BusinessMappers.Accounting.Aghsat;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace EntityCache.WebService.Repository;

public class AghsatShakhsiWebServiceRepository : WebServiceRepository<AghsatShBusiness>, IDisposable
{
	public static event EventHandler ClientSyncDoneResponse;

	public AghsatShakhsiWebServiceRepository(ClsCache cache)
		: base((INoGuidRepository<AghsatShBusiness>)cache.CacheRepository.AghsatShRepository, cache)
	{
		try
		{
			Cache = cache;
			Cache.Context.WebAghsatShakhsi.OnReadResponse += AghsatShakhsiInstance_OnReadResponse;
			Cache.Context.WebAghsatShakhsi.OnSync += AghsatShakhsiInstance_OnSync;
			Cache.Context.WebAghsatShakhsi.OnDeletedRequest += AghsatShakhsiInstance_OnDeletedRequest;
			Cache.Context.WebAghsatShakhsi.OnSaveRequest += WebAghsatShakhsi_OnSaveRequest;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\AghsatShakhsiWebServiceRepository.cs", 30);
		}
	}

	private async Task WebAghsatShakhsi_OnSaveRequest(object sender, ConnectionEventArgs<WebAghsatShakhsi> e)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("WebAghsatShakhsi_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\AghsatShakhsiWebServiceRepository.cs"))
		{
			_ = 2;
			try
			{
				if (!(e.ConnectionData?.Any() ?? false))
				{
					return;
				}
				List<AghsatShBusiness> list = await AghsatShMapper.Instance.MapListAsync(base.ContextGuid, e.ConnectionData, new List<AghsatShBusiness>());
				List<SaveResponse> responses = new List<SaveResponse>();
				foreach (AghsatShBusiness item in list)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = await item.SaveAsync(EnSystem.SignalR, e.Connection.cancellationToken, Priority.Low, sendToServer: false, awaitOnSemaphore: true, await4send: false, 10);
					responses.Add(new SaveResponse
					{
						Date = DateTime.Now,
						Guid = item.Guid,
						HostRefrenced = item.HostRefrenced,
						Message = returnedSaveFuncInfo.Messages,
						PartitionId = e.PartitionId,
						RequestPart = e.Part,
						RequestRefrencedId = e.PacketReferenceId,
						Shortmessage = returnedSaveFuncInfo.Messages,
						SaveResultStatus = !returnedSaveFuncInfo.HasError
					});
					ret.AddReturnedValue(returnedSaveFuncInfo, ReturnedState.None, "WebAghsatShakhsi_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\AghsatShakhsiWebServiceRepository.cs", 58);
				}
				await Cache.SendData(WebAghsatShakhsi.GenerateSaveResponse(responses), Priority.High);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "WebAghsatShakhsi_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\AghsatShakhsiWebServiceRepository.cs", 62);
			}
		}
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				if (Cache.Context.WebAghsatShakhsi != null)
				{
					Cache.Context.WebAghsatShakhsi.OnReadResponse -= AghsatShakhsiInstance_OnReadResponse;
					Cache.Context.WebAghsatShakhsi.OnSync -= AghsatShakhsiInstance_OnSync;
					Cache.Context.WebAghsatShakhsi.OnDeletedRequest -= AghsatShakhsiInstance_OnDeletedRequest;
				}
				AghsatShakhsiWebServiceRepository.ClientSyncDoneResponse = null;
				Cache = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\AghsatShakhsiWebServiceRepository.cs", 81);
		}
	}

	private async Task AghsatShakhsiInstance_OnDeletedRequest(object arg1, ConnectionEventArgs<Guid> e)
	{
		using (new JaegerMonitor("AghsatShakhsiInstance_OnDeletedRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\AghsatShakhsiWebServiceRepository.cs"))
		{
			_ = 1;
			try
			{
				if (e.ConnectionData == null)
				{
					WebErrorLog.Instance.Warning("AghsatShakhsiInstance_OnDeletedRequest received but data is null!", "", "AghsatShakhsiInstance_OnDeletedRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\AghsatShakhsiWebServiceRepository.cs", 90);
					return;
				}
				List<Guid> connectionData = e.ConnectionData;
				if (connectionData != null && !connectionData.Any())
				{
					WebErrorLog.Instance.Warning("AghsatShakhsiInstance_OnDeletedRequest received but data is empty!", "", "AghsatShakhsiInstance_OnDeletedRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\AghsatShakhsiWebServiceRepository.cs", 95);
					return;
				}
				(await AghsatShBusiness.DeleteRangeAsync(Cache.ContextGuid, e.ConnectionData)).throwExceptionIfError();
				if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId))
				{
					Packet packetData = Sync.GenerateDeleteResponse(EntityPart.AghsatShakhsi, e.PartitionId);
					await Cache.SendData(packetData, Priority.High);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AghsatShakhsiInstance_OnDeletedRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\AghsatShakhsiWebServiceRepository.cs", 108);
			}
		}
	}

	private async Task AghsatShakhsiInstance_OnSync(object sender, ConnectionEventArgs<WebAghsatShakhsi> e)
	{
		using (new JaegerMonitor("AghsatShakhsiInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\AghsatShakhsiWebServiceRepository.cs"))
		{
			_ = 2;
			try
			{
				if (e.ConnectionData == null)
				{
					WebErrorLog.Instance.Warning("AghsatShakhsi sync received but data is null!", "", "AghsatShakhsiInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\AghsatShakhsiWebServiceRepository.cs", 118);
					return;
				}
				List<WebAghsatShakhsi> connectionData = e.ConnectionData;
				if (connectionData != null && !connectionData.Any())
				{
					WebErrorLog.Instance.Warning("AghsatShakhsi sync received but data is empty!", "", "AghsatShakhsiInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\AghsatShakhsiWebServiceRepository.cs", 123);
					return;
				}
				List<WebAghsatShakhsi> source = e.ConnectionData.ToList();
				bool sendToServer = e.Connection.Source == ENSource.Accounting;
				CancellationToken cancellationToken = e.Connection.cancellationToken;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = await AghsatShBusiness.SaveRangeAsync(EnSystem.SignalR, cancellationToken, Priority.High, await AghsatShMapper.Instance.MapListAsync(e.Connection.Context.Guid, source, new List<AghsatShBusiness>()), sendToServer, awaitOnSend: false, 10);
				returnedSaveFuncInfo.throwExceptionIfError();
				if (!e.Connection.cancellationToken.IsCancellationRequested && InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId))
				{
					Packet packetData = Sync.GenerateSyncDone(EntityPart.AghsatShakhsi, e.PartitionId, returnedSaveFuncInfo);
					await Cache.SendData(packetData, Priority.High);
					AghsatShakhsiWebServiceRepository.ClientSyncDoneResponse?.Invoke(this, EventArgs.Empty);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AghsatShakhsiInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\AghsatShakhsiWebServiceRepository.cs", 143);
			}
		}
	}

	private async Task AghsatShakhsiInstance_OnReadResponse(object arg1, ConnectionEventArgs<WebAghsatShakhsi> e)
	{
		using (new JaegerMonitor("AghsatShakhsiInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\AghsatShakhsiWebServiceRepository.cs"))
		{
			_ = 1;
			try
			{
				if (e.ConnectionData == null)
				{
					WebErrorLog.Instance.Warning("AghsatShakhsi received but data is null!", "", "AghsatShakhsiInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\AghsatShakhsiWebServiceRepository.cs", 153);
					return;
				}
				List<WebAghsatShakhsi> connectionData = e.ConnectionData;
				if (connectionData != null && !connectionData.Any())
				{
					WebErrorLog.Instance.Warning("AghsatShakhsi received but data is empty!", "", "AghsatShakhsiInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\AghsatShakhsiWebServiceRepository.cs", 158);
					return;
				}
				List<WebAghsatShakhsi> source = e.ConnectionData.ToList();
				bool sendToServer = e.Connection.Source == ENSource.Accounting;
				CancellationToken cancellationToken = e.Connection.cancellationToken;
				await AghsatShBusiness.SaveRangeAsync(EnSystem.SignalR, cancellationToken, Priority.Medium, await AghsatShMapper.Instance.MapListAsync(e.Connection.Context.Guid, source, new List<AghsatShBusiness>()), sendToServer, awaitOnSend: false, 10);
				AghsatShakhsiWebServiceRepository.ClientSyncDoneResponse?.Invoke(this, EventArgs.Empty);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AghsatShakhsiInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\AghsatShakhsiWebServiceRepository.cs", 168);
			}
		}
	}
}
