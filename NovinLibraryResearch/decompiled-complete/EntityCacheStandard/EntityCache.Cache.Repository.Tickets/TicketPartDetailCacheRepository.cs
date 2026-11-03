using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Global.Tickets;
using EntityCache.Core;
using EntityCache.Core.Repository.Global.Ticket;
using EntityCache.Mapping.Global.Ticket;
using PacketParser.BusinessLayer;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Tickets;

public class TicketPartDetailCacheRepository : CacheRepositoryNoGuiId<TicketPartDetailBusiness, string>, ITicketPartDetailRepository, INoGuidRepository<TicketPartDetailBusiness>, IDisposable
{
	private ITicketPartDetailRepository _dbRepository;

	public override bool HasStatus => false;

	public override bool HasModified => false;

	protected override TimingCache<string, TicketPartDetailBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<TicketPartDetailBusiness, TicketPartDetailBusiness> Mapper => TicketPartDetailMapper.Instance;

	public TicketPartDetailCacheRepository(ITicketPartDetailRepository dbRepository, int CachingTimeSeconds)
		: base((INoGuidRepository<TicketPartDetailBusiness>)dbRepository, CachingTimeSeconds)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<string, TicketPartDetailBusiness>(base.ContextGuid, Mapper, CachingTimeSeconds);
	}

	public new void Dispose()
	{
		try
		{
			_dbRepository = null;
			base.Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Global\\Tickets\\TicketPartDetailCacheRepository.cs", 38);
		}
	}

	public Task<ReturnedSaveFuncInfo> SaveAsync(IEnumerable<TicketPartDetailBusiness> items, Guid MasterGuid, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> RemoveAsync(Guid MasterGuid, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<List<TicketPartDetailBusiness>> GetAllAsync(Guid MasterGuid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}
}
