using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Mapping.BusinessMappers.OrderDepot;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace EntityCache.WebService.Repository;

public class DepotWebServiceRepository : WebServiceRepository<DepotBusiness>
{
	public DepotWebServiceRepository(ClsCache cache)
		: base((INoGuidRepository<DepotBusiness>)cache.CacheRepository.IDepot, cache)
	{
		try
		{
			Cache = cache;
			Cache.Context.WebDepot.OnReadResponse += DepotInstance_OnReadResponse;
			Cache.Context.WebDepot.OnSync += DepotInstance_OnSync;
			Cache.Context.WebDepot.OnSaveRequest += WebDepot_OnSaveRequest;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\DepotWebServiceRepository.cs", 25);
		}
	}

	private async Task WebDepot_OnSaveRequest(object sender, ConnectionEventArgs<WebDepot> e)
	{
		using (new JaegerMonitor("WebDepot_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\DepotWebServiceRepository.cs"))
		{
			_ = 2;
			try
			{
				if (e.ConnectionData == null)
				{
					WebErrorLog.Instance.Warning("Depot WebDepot_OnSaveRequest received but data is null!", "", "WebDepot_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\DepotWebServiceRepository.cs", 35);
					return;
				}
				List<WebDepot> connectionData = e.ConnectionData;
				if (connectionData != null && !connectionData.Any())
				{
					WebErrorLog.Instance.Warning("Depot WebDepot_OnSaveRequest received but data is empty!", "", "WebDepot_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\DepotWebServiceRepository.cs", 40);
					return;
				}
				ReturnedSaveFuncInfo saveResult = await DepotBusiness.SaveRangeAsync(send2Server: e.Connection.Source == ENSource.Accounting, system: EnSystem.SignalR, Lst: await DepotMapper.Instance.MapListAsync(e.Connection.Context.Guid, e.ConnectionData), priority: Priority.High, tryCount: 10, tranName: "", token: e.Connection.cancellationToken);
				if (!e.Connection.cancellationToken.IsCancellationRequested && InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId))
				{
					List<Packet> lstPacket = WebDepot.GenerateSaveResponse(e.ConnectionData.Select((WebDepot p) => new SaveResponse(0, p.Guid, DateTime.Now, !saveResult.HasError, saveResult.ErrorMessage, saveResult.ErrorMessage)).ToList());
					await Cache.SendData(lstPacket, Priority.High);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "WebDepot_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\DepotWebServiceRepository.cs", 56);
			}
		}
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				base.IsDisposed = true;
				Cache.Context.WebDepot.OnReadResponse -= DepotInstance_OnReadResponse;
				Cache.Context.WebDepot.OnSync -= DepotInstance_OnSync;
				Cache.Context.WebDepot.OnSaveRequest -= WebDepot_OnSaveRequest;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\DepotWebServiceRepository.cs", 70);
		}
	}

	private async Task DepotInstance_OnReadResponse(object sender, ConnectionEventArgs<WebDepot> e)
	{
		using (new JaegerMonitor("DepotInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\DepotWebServiceRepository.cs"))
		{
			_ = 1;
			try
			{
				if (e.ConnectionData == null)
				{
					WebErrorLog.Instance.Warning("Depot received but data is null!", "", "DepotInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\DepotWebServiceRepository.cs", 80);
				}
				else if (!(e.ConnectionData?.Any() ?? false))
				{
					WebErrorLog.Instance.Warning("Depot received but data is empty!", "", "DepotInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\DepotWebServiceRepository.cs", 85);
				}
				else
				{
					(await DepotBusiness.SaveRangeAsync(send2Server: Cache.Source == ENSource.Accounting, system: EnSystem.SignalR, Lst: await DepotMapper.Instance.MapListAsync(e.Connection.Context.Guid, e.ConnectionData), priority: Priority.Low, tryCount: 10, tranName: "", token: e.Connection.cancellationToken)).throwExceptionIfError();
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "DepotInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\DepotWebServiceRepository.cs", 95);
			}
		}
	}

	private async Task DepotInstance_OnSync(object sender, ConnectionEventArgs<WebDepot> e)
	{
		using (new JaegerMonitor("DepotInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\DepotWebServiceRepository.cs"))
		{
			_ = 2;
			try
			{
				if (e.ConnectionData == null)
				{
					WebErrorLog.Instance.Warning("Depot sync received but data is null!", "", "DepotInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\DepotWebServiceRepository.cs", 105);
					return;
				}
				List<WebDepot> connectionData = e.ConnectionData;
				if (connectionData != null && !connectionData.Any())
				{
					WebErrorLog.Instance.Warning("Depot sync received but data is empty!", "", "DepotInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\DepotWebServiceRepository.cs", 110);
					return;
				}
				ReturnedSaveFuncInfo result = await DepotBusiness.SaveRangeAsync(send2Server: e.Connection.Source == ENSource.Accounting, system: EnSystem.SignalR, Lst: await DepotMapper.Instance.MapListAsync(e.Connection.Context.Guid, e.ConnectionData), priority: Priority.High, tryCount: 10, tranName: "", token: e.Connection.cancellationToken);
				if (!e.Connection.cancellationToken.IsCancellationRequested && InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId))
				{
					Packet packetData = Sync.GenerateSyncDone(EntityPart.Depot, e.PartitionId, result);
					await Cache.SendData(packetData, Priority.High);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "DepotInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\DepotWebServiceRepository.cs", 125);
			}
		}
	}
}
