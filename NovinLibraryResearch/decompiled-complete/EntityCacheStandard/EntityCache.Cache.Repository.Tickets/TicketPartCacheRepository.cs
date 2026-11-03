using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Tickets;
using EntityCache.Core;
using EntityCache.Core.Repository.Tickets;
using EntityCache.Mapping.BusinessMappers.Crm.Ticket;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Tickets;

public class TicketPartCacheRepository : CacheRepository<TicketPartBusiness>, ITicketPartRepository, IRepository<TicketPartBusiness>, INoGuidRepository<TicketPartBusiness>, IDisposable
{
	private ITicketPartRepository _dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, TicketPartBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<TicketPartBusiness, TicketPartBusiness> Mapper => TicketPartMapper.Instance;

	public TicketPartCacheRepository(ITicketPartRepository dbRepository, int CachingTime)
		: base((IRepository<TicketPartBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		try
		{
			_dbRepository = dbRepository;
			TimingCache = new TimingCache<Guid, TicketPartBusiness>(base.ContextGuid, Mapper, CachingTime);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Global\\Tickets\\TicketPartCacheRepository.cs", 31);
		}
	}

	public override void Dispose()
	{
		try
		{
			_dbRepository?.Dispose();
			_dbRepository = null;
			base.Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Global\\Tickets\\TicketPartCacheRepository.cs", 42);
		}
	}

	public Task<TicketPartBusiness> GetAsync(string name, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAsync(name, token, tranName);
	}

	public override Task<TicketPartBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAsync(guid, token, tranName);
	}

	public async Task<IEnumerable<TicketPartBusiness>> GetSubItemsAsync(Guid masterGuid, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetSubItemsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Global\\Tickets\\TicketPartCacheRepository.cs"))
		{
			try
			{
				if (masterGuid == Guid.Empty)
				{
					return null;
				}
				return (await GetAllAsync(true, token, tranName))?.Where((TicketPartBusiness w) => w.ParentGuid == masterGuid);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "GetSubItemsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Global\\Tickets\\TicketPartCacheRepository.cs", 58);
				throw;
			}
		}
	}

	public Task<short> GetNextOrdinalAsync(CancellationToken token, string tranName)
	{
		return _dbRepository.GetNextOrdinalAsync(token, tranName);
	}

	public Task<List<Guid>> GetAllRelatedPartsAsync(Guid targetGuid, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllRelatedPartsAsync(targetGuid, token, tranName);
	}
}
