using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Global.Tickets;
using EntityCache.Business.Tickets;
using EntityCache.Core;
using EntityCache.Core.Repository.Tickets;
using EntityCache.Mapping.BusinessMappers.Crm.Ticket;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Filters;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Tickets;

public class TicketDetCacheRepository : CacheRepository<TicketDetBusiness>, ITicketDetRepository, IRepository<TicketDetBusiness>, INoGuidRepository<TicketDetBusiness>, IDisposable
{
	private ITicketDetRepository _dbRepository;

	public override bool HasStatus => false;

	public override bool HasModified => false;

	protected override TimingCache<Guid, TicketDetBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<TicketDetBusiness, TicketDetBusiness> Mapper => TicketDetMapper.Instance;

	public TicketDetCacheRepository(ITicketDetRepository dbRepository, int CachingTimeSeconds)
		: base((IRepository<TicketDetBusiness>)dbRepository, CachingTimeSeconds, cachingEnable: true)
	{
		try
		{
			if (base.IsDisposed)
			{
				throw new ObjectDisposedException(GetType().Name);
			}
			_dbRepository = dbRepository;
			TimingCache = new TimingCache<Guid, TicketDetBusiness>(base.ContextGuid, Mapper, base.CachingTime);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Global\\Tickets\\TicketDetCacheRepository.cs", 33);
		}
	}

	public override void Dispose()
	{
		try
		{
			_dbRepository = null;
			base.Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Global\\Tickets\\TicketDetCacheRepository.cs", 43);
		}
	}

	public Task<IEnumerable<TicketReportBusiness>> GetAllAsync(TicketDetFilter filter, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllAsync(filter, token, tranName);
	}
}
