using System;
using System.Linq;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace EntityCache.WebService.Repository;

public class SyncWebServiceRepository : IDisposable
{
	private ClsCache Cache;

	public SyncWebServiceRepository(ClsCache cache)
	{
		try
		{
			Cache = cache;
			cache.Context.Sync.OnSyncReset += SyncInstance_OnSyncReset;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\SyncWebServiceRepository.cs", 22);
		}
	}

	public void Dispose()
	{
		try
		{
			if (Cache?.Context?.Sync != null)
			{
				Cache.Context.Sync.OnSyncReset -= SyncInstance_OnSyncReset;
			}
			Cache = null;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\SyncWebServiceRepository.cs", 32);
		}
	}

	private async Task SyncInstance_OnSyncReset(object sender, SyncResetEventArgs e)
	{
		using (new JaegerMonitor("SyncInstance_OnSyncReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\SyncWebServiceRepository.cs"))
		{
			_ = 1;
			try
			{
				switch (e.EntityPart)
				{
				case EntityPart.Customer:
					if (Cache.Source != ENSource.CrmPortal && Cache.Source != ENSource.Baje)
					{
					}
					break;
				case EntityPart.Depot:
					await DepotBusiness.KeepRangeAsync(e.Connection.ContextGuid, e.SyncResetItems.Select((SyncResetItem x) => x.Guid), 10);
					break;
				case EntityPart.HesabTree:
					e.SyncResetItems.Select((SyncResetItem x) => x.Guid);
					break;
				}
				await Cache.SendData(Sync.GenerateSyncResetDone(e.EntityPart), Priority.High);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"e.EntityPart:{e?.EntityPart ?? EntityPart.None}", "SyncInstance_OnSyncReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\SyncWebServiceRepository.cs", 107);
			}
		}
	}
}
