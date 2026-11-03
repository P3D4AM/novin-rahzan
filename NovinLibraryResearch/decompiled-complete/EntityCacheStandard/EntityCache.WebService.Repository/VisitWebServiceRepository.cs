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

public class VisitWebServiceRepository : WebServiceRepository<VisitBusiness>, IDisposable
{
	public VisitWebServiceRepository(ClsCache cache)
		: base((INoGuidRepository<VisitBusiness>)cache.CacheRepository.IVisit, cache)
	{
		try
		{
			Cache.Context.WebVisit.OnReadResponse += VisitInstance_OnReadResponse;
			Cache.Context.WebVisit.OnSync += VisitInstance_OnSync;
			Cache.Context.WebVisit.OnSaveResponse += VisitInstance_OnSaveResponse;
			cache.Context.Sync.OnSyncReset += SyncInstance_OnSyncReset;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\VisitWebServiceRepository.cs", 26);
		}
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				Cache.Context.WebVisit.OnReadResponse -= VisitInstance_OnReadResponse;
				Cache.Context.WebVisit.OnSync -= VisitInstance_OnSync;
				Cache.Context.WebVisit.OnSaveResponse -= VisitInstance_OnSaveResponse;
				Cache.Context.Sync.OnSyncReset -= SyncInstance_OnSyncReset;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\VisitWebServiceRepository.cs", 41);
		}
	}

	private async Task SyncInstance_OnSyncReset(object sender, SyncResetEventArgs e)
	{
		using (new JaegerMonitor("SyncInstance_OnSyncReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\VisitWebServiceRepository.cs"))
		{
			_ = 2;
			try
			{
				if (e.EntityPart == EntityPart.Visit)
				{
					Task<IEnumerable<VisitBusiness>> allAsync = base.CacheRepository.GetAllAsync(default(CancellationToken), "");
					HashSet<Guid> keepIds = new HashSet<Guid>(e.SyncResetItems.Select((SyncResetItem x) => x.Guid));
					IEnumerable<VisitBusiness> list = (await allAsync).Where((VisitBusiness x) => keepIds.Contains(x.Guid)).ToList();
					bool sendToServer = Cache.Source == ENSource.Accounting;
					(await VisitBusiness.SaveRangeAsync(EnSystem.SignalR, list, 10, sendToServer, Priority.High, 10, "", e.Connection.cancellationToken)).throwExceptionIfError();
					if (!e.Connection.cancellationToken.IsCancellationRequested)
					{
						await Cache.SendData(Sync.GenerateSyncResetDone(e.EntityPart), Priority.High);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SyncInstance_OnSyncReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\VisitWebServiceRepository.cs", 64);
			}
		}
	}

	private async Task VisitInstance_OnSync(object sender, ConnectionEventArgs<WebVisit> e)
	{
		using (new JaegerMonitor("VisitInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\VisitWebServiceRepository.cs"))
		{
			int line = 0;
			try
			{
				if (e.ConnectionData == null)
				{
					line = 1;
					WebErrorLog.Instance.Warning("Visit sync received but data is null!", "", "VisitInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\VisitWebServiceRepository.cs", 76);
					return;
				}
				List<WebVisit> connectionData = e.ConnectionData;
				if (connectionData != null && !connectionData.Any())
				{
					line = 2;
					WebErrorLog.Instance.Warning("Visit sync received but data is empty!", "", "VisitInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\VisitWebServiceRepository.cs", 82);
					return;
				}
				line = 3;
				List<WebVisit> source = e.ConnectionData.ToList();
				line = 4;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = await VisitBusiness.SaveRangeAsync(sendToServer: Cache.Source == ENSource.Accounting, system: EnSystem.SignalR, list: await VisitMapper.Instance.MapListAsync(e.Connection.Context.Guid, source), autoFixValidationCount: 10, priority: Priority.High, tryCount: 10, tranName: "", token: e.Connection.cancellationToken);
				line = 5;
				returnedSaveFuncInfo.throwExceptionIfError();
				if (!e.Connection.cancellationToken.IsCancellationRequested && InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId))
				{
					Packet packetData = Sync.GenerateSyncDone(EntityPart.Visit, e.PartitionId, returnedSaveFuncInfo);
					line = 5;
					await Cache.SendData(packetData, Priority.High);
					line = 6;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"ClsCache.GetCache(contextGuid).DatabaseManager :{Cache.DatabaseManager} The Error Line IS {line} ", "VisitInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\VisitWebServiceRepository.cs", 104);
			}
		}
	}

	private async Task VisitInstance_OnReadResponse(object sender, ConnectionEventArgs<WebVisit> e)
	{
		using (new JaegerMonitor("VisitInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\VisitWebServiceRepository.cs"))
		{
			_ = 1;
			try
			{
				if (e.ConnectionData == null)
				{
					WebErrorLog.Instance.Warning("Visit received but data is null!", "", "VisitInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\VisitWebServiceRepository.cs", 114);
					return;
				}
				List<WebVisit> connectionData = e.ConnectionData;
				if (connectionData != null && !connectionData.Any())
				{
					WebErrorLog.Instance.Warning("Visit received but data is empty!", "", "VisitInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\VisitWebServiceRepository.cs", 119);
					return;
				}
				List<WebVisit> source = e.ConnectionData.ToList();
				await VisitBusiness.SaveRangeAsync(sendToServer: Cache.Source == ENSource.Accounting, system: EnSystem.SignalR, list: await VisitMapper.Instance.MapListAsync(e.Connection.Context.Guid, source), autoFixValidationCount: 10, priority: Priority.High, tryCount: 10, tranName: "", token: e.Connection.cancellationToken);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "VisitInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\VisitWebServiceRepository.cs", 129);
			}
		}
	}

	private async Task VisitInstance_OnSaveResponse(object sender, SaveResponseEventArgs e)
	{
		using (new JaegerMonitor("VisitInstance_OnSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\VisitWebServiceRepository.cs"))
		{
			_ = 1;
			try
			{
				if (e?.SaveResponses == null)
				{
					return;
				}
				if (e == null || !(e.SaveResponses?.Count > 0))
				{
					return;
				}
				foreach (SaveResponse resopnse in e.SaveResponses)
				{
					if (resopnse != null)
					{
						VisitBusiness visitBusiness = await VisitBusiness.GetAsync(e.Connection.ContextGuid, resopnse.Guid);
						if (visitBusiness != null)
						{
							await visitBusiness.SetSaveResultAsync(resopnse, e.Connection.cancellationToken, 10);
						}
					}
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "VisitInstance_OnSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\VisitWebServiceRepository.cs", 147);
			}
		}
	}
}
