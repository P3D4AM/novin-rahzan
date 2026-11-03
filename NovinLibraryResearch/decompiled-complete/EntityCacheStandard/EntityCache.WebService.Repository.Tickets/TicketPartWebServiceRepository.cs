using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Tickets;
using EntityCache.Core;
using EntityCache.Mapping.BusinessMappers.Crm.Ticket;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace EntityCache.WebService.Repository.Tickets;

public class TicketPartWebServiceRepository : WebServiceRepository<TicketPartBusiness>, IDisposable
{
	public TicketPartWebServiceRepository(ClsCache cache)
		: base((INoGuidRepository<TicketPartBusiness>)cache.CacheRepository.TicketPart, cache)
	{
		try
		{
			Cache.Context.WebTicketPart.OnReadResponse += TicketPartInstance_OnReadResponse;
			Cache.Context.WebTicketPart.OnSync += TicketPartInstance_OnSync;
			Cache.Context.WebTicketPart.OnSaveResponse += TicketPartInstance_OnSaveResponse;
			cache.Context.Sync.OnSyncReset += SyncInstance_OnSyncReset;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Tickets\\TicketPartWebServiceRepository.cs", 27);
		}
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				Cache.Context.WebTicketPart.OnReadResponse -= TicketPartInstance_OnReadResponse;
				Cache.Context.WebTicketPart.OnSync -= TicketPartInstance_OnSync;
				Cache.Context.WebTicketPart.OnSaveResponse -= TicketPartInstance_OnSaveResponse;
				Cache.Context.Sync.OnSyncReset -= SyncInstance_OnSyncReset;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Tickets\\TicketPartWebServiceRepository.cs", 41);
		}
	}

	private async Task SyncInstance_OnSyncReset(object arg1, SyncResetEventArgs e)
	{
		using (new JaegerMonitor("SyncInstance_OnSyncReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Tickets\\TicketPartWebServiceRepository.cs"))
		{
			try
			{
				if (e.EntityPart == EntityPart.TicketPart)
				{
					Task<IEnumerable<TicketPartBusiness>> allAsync = base.CacheRepository.GetAllAsync(default(CancellationToken), "");
					HashSet<Guid> keepIds = new HashSet<Guid>(e.SyncResetItems.Select((SyncResetItem x) => x.Guid));
					(await allAsync).Where((TicketPartBusiness x) => keepIds.Contains(x.Guid));
					throw new NotImplementedException("send2Server" + "  باید دقیق تر تصمیم گیری شود".GetTranslate());
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SyncInstance_OnSyncReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Tickets\\TicketPartWebServiceRepository.cs", 66);
			}
		}
	}

	private async Task TicketPartInstance_OnSync(object sender, ConnectionEventArgs<WebTicketPart> e)
	{
		using (new JaegerMonitor("TicketPartInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Tickets\\TicketPartWebServiceRepository.cs"))
		{
			_ = 2;
			try
			{
				if (e.ConnectionData == null)
				{
					WebErrorLog.Instance.Warning("TicketPart sync received but data is null!", "", "TicketPartInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Tickets\\TicketPartWebServiceRepository.cs", 76);
					return;
				}
				List<WebTicketPart> connectionData = e.ConnectionData;
				if (connectionData != null && !connectionData.Any())
				{
					WebErrorLog.Instance.Warning("TicketPart sync received but data is empty!", "", "TicketPartInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Tickets\\TicketPartWebServiceRepository.cs", 81);
					return;
				}
				List<WebTicketPart> connectionData2 = e.ConnectionData;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = await TicketPartBusiness.SaveRangeAsync(send2Server: Cache.Source == ENSource.Accounting, system: EnSystem.SignalR, autoFixValidationCount: 10, list: await TicketPartMapper.Instance.MapListAsync(e.Connection.Context.Guid, connectionData2), token: e.Connection.cancellationToken);
				returnedSaveFuncInfo.throwExceptionIfError();
				if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId))
				{
					Packet packetData = Sync.GenerateSyncDone(EntityPart.TicketPart, e.PartitionId, returnedSaveFuncInfo);
					await Cache.SendData(packetData, Priority.High);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "TicketPartInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Tickets\\TicketPartWebServiceRepository.cs", 97);
			}
		}
	}

	private async Task TicketPartInstance_OnReadResponse(object sender, ConnectionEventArgs<WebTicketPart> e)
	{
		using (new JaegerMonitor("TicketPartInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Tickets\\TicketPartWebServiceRepository.cs"))
		{
			_ = 1;
			try
			{
				if (e.ConnectionData == null)
				{
					WebErrorLog.Instance.Warning("TicketPart received but data is null!", "", "TicketPartInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Tickets\\TicketPartWebServiceRepository.cs", 107);
					return;
				}
				List<WebTicketPart> connectionData = e.ConnectionData;
				if (connectionData != null && !connectionData.Any())
				{
					WebErrorLog.Instance.Warning("TicketPart received but data is empty!", "", "TicketPartInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Tickets\\TicketPartWebServiceRepository.cs", 112);
					return;
				}
				List<WebTicketPart> source = e.ConnectionData.ToList();
				await TicketPartBusiness.SaveRangeAsync(send2Server: Cache.Source == ENSource.Accounting, system: EnSystem.SignalR, autoFixValidationCount: 10, list: await TicketPartMapper.Instance.MapListAsync(e.Connection.Context.Guid, source), token: e.Connection.cancellationToken);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "TicketPartInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Tickets\\TicketPartWebServiceRepository.cs", 122);
			}
		}
	}

	private async Task TicketPartInstance_OnSaveResponse(object arg1, SaveResponseEventArgs e)
	{
		using (new JaegerMonitor("TicketPartInstance_OnSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Tickets\\TicketPartWebServiceRepository.cs"))
		{
			try
			{
				if (e == null || !(e.SaveResponses?.Count > 0))
				{
					return;
				}
				foreach (SaveResponse saveResponse in e.SaveResponses)
				{
					if (saveResponse != null)
					{
						await TicketPartBusiness.GetAsync(Cache.ContextGuid, saveResponse.Guid);
					}
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "TicketPartInstance_OnSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Tickets\\TicketPartWebServiceRepository.cs", 140);
			}
		}
	}
}
