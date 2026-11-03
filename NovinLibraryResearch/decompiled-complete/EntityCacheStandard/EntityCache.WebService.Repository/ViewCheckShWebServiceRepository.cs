using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Mapping.BusinessMappers.Accounting;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace EntityCache.WebService.Repository;

public class ViewCheckShWebServiceRepository : WebServiceRepository<ViewCheckShBusiness>, IDisposable
{
	public static event EventHandler ClientSyncDoneResponse;

	public ViewCheckShWebServiceRepository(ClsCache cache)
		: base((INoGuidRepository<ViewCheckShBusiness>)cache.CacheRepository.IViewCheckShRepository, cache)
	{
		try
		{
			Cache = cache;
			Cache.Context.WebViewCheckSh.OnReadResponse += ViewCheckShInstance_OnReadResponse;
			Cache.Context.WebViewCheckSh.OnSync += ViewCheckShInstance_OnSync;
			Cache.Context.WebViewCheckSh.OnDeletedRequest += ViewCheckShInstance_OnDeletedRequest;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ViewCheckShWebServiceRepository.cs", 29);
		}
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				Cache.Context.WebViewCheckSh.OnReadResponse -= ViewCheckShInstance_OnReadResponse;
				Cache.Context.WebViewCheckSh.OnSync -= ViewCheckShInstance_OnSync;
				Cache.Context.WebViewCheckSh.OnDeletedRequest -= ViewCheckShInstance_OnDeletedRequest;
				ViewCheckShWebServiceRepository.ClientSyncDoneResponse = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ViewCheckShWebServiceRepository.cs", 43);
		}
	}

	private async Task ViewCheckShInstance_OnDeletedRequest(object arg1, ConnectionEventArgs<Guid> e)
	{
		using (new JaegerMonitor("ViewCheckShInstance_OnDeletedRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ViewCheckShWebServiceRepository.cs"))
		{
			_ = 1;
			try
			{
				if (e.ConnectionData == null)
				{
					WebErrorLog.Instance.Warning("ViewCheckShInstance Ondelete received but data is null!", "", "ViewCheckShInstance_OnDeletedRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ViewCheckShWebServiceRepository.cs", 52);
					return;
				}
				List<Guid> connectionData = e.ConnectionData;
				if (connectionData != null && !connectionData.Any())
				{
					WebErrorLog.Instance.Warning("ViewCheckShInstance Ondelete received but data is empty!", "", "ViewCheckShInstance_OnDeletedRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ViewCheckShWebServiceRepository.cs", 57);
					return;
				}
				(await ViewCheckShBusiness.DeleteRangeAsync(Cache.ContextGuid, e.ConnectionData)).throwExceptionIfError();
				if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId))
				{
					Packet packetData = Sync.GenerateDeleteResponse(EntityPart.ViewCheckSh, e.PartitionId);
					await Cache.SendData(packetData, Priority.High);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ViewCheckShInstance_OnDeletedRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ViewCheckShWebServiceRepository.cs", 70);
			}
		}
	}

	private async Task ViewCheckShInstance_OnSync(object sender, ConnectionEventArgs<WebViewCheckSh> e)
	{
		using (new JaegerMonitor("ViewCheckShInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ViewCheckShWebServiceRepository.cs"))
		{
			int line = 0;
			try
			{
				if (e.ConnectionData == null)
				{
					line = 3;
					WebErrorLog.Instance.Warning("ViewCheckShInstance sync received but data is null!", "", "ViewCheckShInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ViewCheckShWebServiceRepository.cs", 82);
					return;
				}
				List<WebViewCheckSh> connectionData = e.ConnectionData;
				if (connectionData != null && !connectionData.Any())
				{
					line = 4;
					WebErrorLog.Instance.Warning("ViewCheckShInstance sync received but data is empty!", "", "ViewCheckShInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ViewCheckShWebServiceRepository.cs", 88);
					return;
				}
				CancellationToken cancellationToken = e.Connection.cancellationToken;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = await ViewCheckShBusiness.SaveRangeAsync(cancellationToken, await ViewCheckShMapper.Instance.MapListAsync(e.Connection.Context.Guid, e.ConnectionData), EnSystem.SignalR, ClsCache.GetCache(e.Connection.Context.Guid).Source == ENSource.Accounting, 10);
				line = 5;
				returnedSaveFuncInfo.throwExceptionIfError();
				line = 6;
				if (!e.Connection.cancellationToken.IsCancellationRequested && InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId))
				{
					line = 7;
					Packet packetData = Sync.GenerateSyncDone(EntityPart.ViewCheckSh, e.PartitionId, returnedSaveFuncInfo);
					line = 8;
					await Cache.SendData(packetData, Priority.High);
					ViewCheckShWebServiceRepository.ClientSyncDoneResponse?.Invoke(this, EventArgs.Empty);
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"#Line_{line}", "ViewCheckShInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ViewCheckShWebServiceRepository.cs", 109);
			}
		}
	}

	private async Task ViewCheckShInstance_OnReadResponse(object arg1, ConnectionEventArgs<WebViewCheckSh> e)
	{
		using (new JaegerMonitor("ViewCheckShInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ViewCheckShWebServiceRepository.cs"))
		{
			_ = 1;
			try
			{
				if (e.ConnectionData == null)
				{
					WebErrorLog.Instance.Warning("ViewCheckSh received but data is null!", "", "ViewCheckShInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ViewCheckShWebServiceRepository.cs", 119);
					return;
				}
				List<WebViewCheckSh> connectionData = e.ConnectionData;
				if (connectionData != null && !connectionData.Any())
				{
					WebErrorLog.Instance.Warning("ViewCheckSh received but data is empty!", "", "ViewCheckShInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ViewCheckShWebServiceRepository.cs", 124);
					return;
				}
				CancellationToken cancellationToken = e.Connection.cancellationToken;
				await ViewCheckShBusiness.SaveRangeAsync(cancellationToken, await ViewCheckShMapper.Instance.MapListAsync(e.Connection.Context.Guid, e.ConnectionData), EnSystem.SignalR, ClsCache.GetCache(e.Connection.Context.Guid).Source == ENSource.Accounting, 10);
				ViewCheckShWebServiceRepository.ClientSyncDoneResponse?.Invoke(this, EventArgs.Empty);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ViewCheckShInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ViewCheckShWebServiceRepository.cs", 133);
			}
		}
	}
}
