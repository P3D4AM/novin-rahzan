using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Mapping.BusinessMappers;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace EntityCache.WebService.Repository;

public class HyperLinkWebServiceRepository : WebServiceRepository<HyperLinkBusiness>
{
	private bool _isDisposed;

	public new bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return base.IsDisposed;
			}
			return true;
		}
	}

	public HyperLinkWebServiceRepository(ClsCache cache)
		: base((INoGuidRepository<HyperLinkBusiness>)cache.CacheRepository.IHyperLink, cache)
	{
		try
		{
			Cache = cache;
			Cache.Context.WebHyperLink.OnReadResponse += HyperLinkInstance_OnReadResponse;
			Cache.Context.WebHyperLink.OnSync += HyperLinkInstance_OnSync;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\HyperLinkWebServiceRepository.cs", 27);
		}
	}

	public override void Dispose()
	{
		Dispose(disposing: true);
	}

	public override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				Cache.Context.WebHyperLink.OnReadResponse -= HyperLinkInstance_OnReadResponse;
				Cache.Context.WebHyperLink.OnSync -= HyperLinkInstance_OnSync;
				base.Dispose(disposing: true);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\HyperLinkWebServiceRepository.cs", 43);
		}
	}

	private async Task HyperLinkInstance_OnSync(object sender, ConnectionEventArgs<WebHyperLink> e)
	{
		using (new JaegerMonitor("HyperLinkInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\HyperLinkWebServiceRepository.cs"))
		{
			_ = 2;
			try
			{
				if (e.ConnectionData == null)
				{
					WebErrorLog.Instance.Warning("Hyperlink sync received but data is null!", "", "HyperLinkInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\HyperLinkWebServiceRepository.cs", 53);
					return;
				}
				List<WebHyperLink> connectionData = e.ConnectionData;
				if (connectionData != null && !connectionData.Any())
				{
					WebErrorLog.Instance.Warning("Hyperlink sync received but data is empty!", "", "HyperLinkInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\HyperLinkWebServiceRepository.cs", 58);
					return;
				}
				CancellationToken cancellationToken = e.Connection.cancellationToken;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = await HyperLinkBusiness.SaveRangeAsync(cancellationToken, await HyperLinkMapper.Instance.MapListAsync(e.Connection.Context.Guid, e.ConnectionData), awaitOnSemaphore: true, 10);
				returnedSaveFuncInfo.throwExceptionIfError();
				if (!e.Connection.cancellationToken.IsCancellationRequested && InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId))
				{
					Packet packetData = Sync.GenerateSyncDone(EntityPart.HyperLink, e.PartitionId, returnedSaveFuncInfo);
					await Cache.SendData(packetData, Priority.High);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "HyperLinkInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\HyperLinkWebServiceRepository.cs", 74);
			}
		}
	}

	private async Task HyperLinkInstance_OnReadResponse(object sender, ConnectionEventArgs<WebHyperLink> e)
	{
		using (new JaegerMonitor("HyperLinkInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\HyperLinkWebServiceRepository.cs"))
		{
			_ = 1;
			try
			{
				if (e.ConnectionData == null)
				{
					WebErrorLog.Instance.Warning("Hyperlink received but data is null!", "", "HyperLinkInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\HyperLinkWebServiceRepository.cs", 85);
					return;
				}
				List<WebHyperLink> connectionData = e.ConnectionData;
				if (connectionData != null && !connectionData.Any())
				{
					WebErrorLog.Instance.Warning("Hyperlink received but data is empty!", "", "HyperLinkInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\HyperLinkWebServiceRepository.cs", 90);
					return;
				}
				CancellationToken cancellationToken = e.Connection.cancellationToken;
				await HyperLinkBusiness.SaveRangeAsync(cancellationToken, await HyperLinkMapper.Instance.MapListAsync(e.Connection.Context.Guid, e.ConnectionData), awaitOnSemaphore: true, 10);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "HyperLinkInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\HyperLinkWebServiceRepository.cs", 97);
			}
		}
	}
}
