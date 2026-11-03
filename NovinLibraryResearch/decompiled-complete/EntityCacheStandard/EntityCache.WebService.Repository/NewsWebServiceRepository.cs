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

public class NewsWebServiceRepository : WebServiceRepository<NewsBusiness>, IDisposable
{
	public NewsWebServiceRepository(ClsCache cache)
		: base((INoGuidRepository<NewsBusiness>)cache.CacheRepository.INews, cache)
	{
		try
		{
			Cache.Context.WebNews.OnReadResponse += NewsInstance_OnReadResponse;
			Cache.Context.WebNews.OnSync += NewsInstance_OnSync;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\NewsWebServiceRepository.cs", 24);
		}
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				Cache.Context.WebNews.OnReadResponse -= NewsInstance_OnReadResponse;
				Cache.Context.WebNews.OnSync -= NewsInstance_OnSync;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\NewsWebServiceRepository.cs", 36);
		}
	}

	private async Task NewsInstance_OnSync(object sender, ConnectionEventArgs<WebNews> e)
	{
		using (new JaegerMonitor("NewsInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\NewsWebServiceRepository.cs"))
		{
			_ = 2;
			try
			{
				if (e.ConnectionData == null)
				{
					WebErrorLog.Instance.Warning("News sync received but data is null!", "", "NewsInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\NewsWebServiceRepository.cs", 46);
					return;
				}
				List<WebNews> connectionData = e.ConnectionData;
				if (connectionData != null && !connectionData.Any())
				{
					WebErrorLog.Instance.Warning("News sync received but data is empty!", "", "NewsInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\NewsWebServiceRepository.cs", 51);
					return;
				}
				CancellationToken cancellationToken = e.Connection.cancellationToken;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = await NewsBusiness.SaveRangeAsync(cancellationToken, await NewsMapper.Instance.MapListAsync(e.Connection.Context.Guid, e.ConnectionData), awaitOnSemaphore: true, 10);
				returnedSaveFuncInfo.throwExceptionIfError();
				if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId))
				{
					Packet packetData = Sync.GenerateSyncDone(EntityPart.News, e.PartitionId, returnedSaveFuncInfo);
					await Cache.SendData(packetData, Priority.High);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "NewsInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\NewsWebServiceRepository.cs", 66);
			}
		}
	}

	private async Task NewsInstance_OnReadResponse(object sender, ConnectionEventArgs<WebNews> e)
	{
		using (new JaegerMonitor("NewsInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\NewsWebServiceRepository.cs"))
		{
			_ = 1;
			try
			{
				if (e.ConnectionData == null)
				{
					WebErrorLog.Instance.Warning("News received but data is null!", "", "NewsInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\NewsWebServiceRepository.cs", 76);
					return;
				}
				List<WebNews> connectionData = e.ConnectionData;
				if (connectionData != null && !connectionData.Any())
				{
					WebErrorLog.Instance.Warning("News received but data is empty!", "", "NewsInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\NewsWebServiceRepository.cs", 81);
					return;
				}
				CancellationToken cancellationToken = e.Connection.cancellationToken;
				await NewsBusiness.SaveRangeAsync(cancellationToken, await NewsMapper.Instance.MapListAsync(e.Connection.Context.Guid, e.ConnectionData), awaitOnSemaphore: true, 10);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "NewsInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\NewsWebServiceRepository.cs", 88);
			}
		}
	}
}
