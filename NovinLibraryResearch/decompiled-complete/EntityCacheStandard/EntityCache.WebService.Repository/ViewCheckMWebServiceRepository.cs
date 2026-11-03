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

public class ViewCheckMWebServiceRepository : WebServiceRepository<ViewCheckMBusiness>, IDisposable
{
	public static event EventHandler ClientSyncDoneResponse;

	public ViewCheckMWebServiceRepository(ClsCache cache)
		: base((INoGuidRepository<ViewCheckMBusiness>)cache.CacheRepository.IViewCheckMRepository, cache)
	{
		try
		{
			Cache = cache;
			Cache.Context.WebViewCheckM.OnReadResponse += ViewCheckMInstance_OnReadResponse;
			Cache.Context.WebViewCheckM.OnSync += ViewCheckMInstance_OnSync;
			Cache.Context.WebViewCheckM.OnDeletedRequest += ViewCheckMInstance_OnDeletedRequest;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ViewCheckMWebServiceRepository.cs", 28);
		}
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				Cache.Context.WebViewCheckM.OnReadResponse -= ViewCheckMInstance_OnReadResponse;
				Cache.Context.WebViewCheckM.OnSync -= ViewCheckMInstance_OnSync;
				Cache.Context.WebViewCheckM.OnDeletedRequest -= ViewCheckMInstance_OnDeletedRequest;
				ViewCheckMWebServiceRepository.ClientSyncDoneResponse = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ViewCheckMWebServiceRepository.cs", 42);
		}
	}

	private async Task ViewCheckMInstance_OnDeletedRequest(object arg1, ConnectionEventArgs<Guid> e)
	{
		using (new JaegerMonitor("ViewCheckMInstance_OnDeletedRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ViewCheckMWebServiceRepository.cs"))
		{
			_ = 1;
			try
			{
				if (e.ConnectionData == null)
				{
					WebErrorLog.Instance.Warning("ViewCheckMInstance Ondelete received but data is null!", "", "ViewCheckMInstance_OnDeletedRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ViewCheckMWebServiceRepository.cs", 51);
					return;
				}
				List<Guid> connectionData = e.ConnectionData;
				if (connectionData != null && !connectionData.Any())
				{
					WebErrorLog.Instance.Warning("ViewCheckMInstance Ondelete received but data is empty!", "", "ViewCheckMInstance_OnDeletedRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ViewCheckMWebServiceRepository.cs", 56);
					return;
				}
				(await ViewCheckMBusiness.DeleteRangeAsync(e.Connection.ContextGuid, e.ConnectionData)).throwExceptionIfError();
				if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId))
				{
					Packet packetData = Sync.GenerateDeleteResponse(EntityPart.ViewCheckM, e.PartitionId);
					await Cache.SendData(packetData, Priority.High);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ViewCheckMInstance_OnDeletedRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ViewCheckMWebServiceRepository.cs", 69);
			}
		}
	}

	private async Task ViewCheckMInstance_OnSync(object arg1, ConnectionEventArgs<WebViewCheckM> e)
	{
		using (new JaegerMonitor("ViewCheckMInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ViewCheckMWebServiceRepository.cs"))
		{
			_ = 2;
			try
			{
				if (!(e.ConnectionData?.Any() ?? false))
				{
					WebErrorLog.Instance.Warning("ViewCheckMInstance sync received but data is empty!", "", "ViewCheckMInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ViewCheckMWebServiceRepository.cs", 78);
					return;
				}
				CancellationToken cancellationToken = e.Connection.cancellationToken;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = await ViewCheckMBusiness.SaveRangeAsync(cancellationToken, await ViewChechMMapper.Instance.MapListAsync(e.Connection.Context.Guid, e.ConnectionData), EnSystem.SignalR, Cache.Source == ENSource.Accounting, 10);
				returnedSaveFuncInfo.throwExceptionIfError();
				if (!e.Connection.cancellationToken.IsCancellationRequested && InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId))
				{
					Packet packetData = Sync.GenerateSyncDone(EntityPart.ViewCheckM, e.PartitionId, returnedSaveFuncInfo);
					await Cache.SendData(packetData, Priority.High);
					ViewCheckMWebServiceRepository.ClientSyncDoneResponse?.Invoke(this, EventArgs.Empty);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ViewCheckMInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ViewCheckMWebServiceRepository.cs", 94);
			}
		}
	}

	private async Task ViewCheckMInstance_OnReadResponse(object arg1, ConnectionEventArgs<WebViewCheckM> e)
	{
		using (new JaegerMonitor("ViewCheckMInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ViewCheckMWebServiceRepository.cs"))
		{
			_ = 1;
			try
			{
				if (Cache.Source == ENSource.Accounting)
				{
					throw new NotImplementedException("برای حسابداری مجاز نیست.".GetTranslate());
				}
				if (e.ConnectionData == null)
				{
					WebErrorLog.Instance.Warning("ViewCheckM received but data is null!", "", "ViewCheckMInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ViewCheckMWebServiceRepository.cs", 104);
					return;
				}
				List<WebViewCheckM> connectionData = e.ConnectionData;
				if (connectionData != null && !connectionData.Any())
				{
					WebErrorLog.Instance.Warning("ViewCheckM received but data is empty!", "", "ViewCheckMInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ViewCheckMWebServiceRepository.cs", 109);
					return;
				}
				CancellationToken cancellationToken = e.Connection.cancellationToken;
				await ViewCheckMBusiness.SaveRangeAsync(cancellationToken, await ViewChechMMapper.Instance.MapListAsync(e.Connection.Context.Guid, e.ConnectionData), EnSystem.SignalR, Cache.Source == ENSource.Accounting, 10);
				ViewCheckMWebServiceRepository.ClientSyncDoneResponse?.Invoke(this, EventArgs.Empty);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ViewCheckMInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ViewCheckMWebServiceRepository.cs", 115);
			}
		}
	}
}
