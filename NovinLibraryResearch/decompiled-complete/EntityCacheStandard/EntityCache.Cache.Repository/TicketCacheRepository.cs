using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Global.Tickets;
using EntityCache.Business.Tickets;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Mapping.BusinessMappers.Crm.Ticket;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models.Ticket;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Filters;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository;

public class TicketCacheRepository : CacheRepository<TicketBusiness>, ITicketRepository, IRepository<TicketBusiness>, INoGuidRepository<TicketBusiness>, IDisposable
{
	private ITicketRepository _dbRepository;

	public override bool HasStatus => false;

	public override bool HasModified => false;

	protected override TimingCache<Guid, TicketBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<TicketBusiness, TicketBusiness> Mapper => TicketMapper.Instance;

	public TicketCacheRepository(ITicketRepository dbRepository, int CachingTime)
		: base((IRepository<TicketBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		try
		{
			if (base.IsDisposed)
			{
				throw new ObjectDisposedException(GetType().Name);
			}
			_dbRepository = dbRepository;
			TimingCache = new TimingCache<Guid, TicketBusiness>(base.ContextGuid, Mapper, CachingTime);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Global\\Tickets\\TicketCacheRepository.cs", 37);
			throw;
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Global\\Tickets\\TicketCacheRepository.cs", 48);
		}
	}

	public Task<IEnumerable<TicketReportBusiness>> GetAllAsync(TicketFilter filter, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllAsync(filter, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> ChangeStatusAsync(Guid guid, short statusId, CancellationToken token, string tranName)
	{
		return _dbRepository.ChangeStatusAsync(guid, statusId, token, tranName);
	}

	public Task<List<TicketTitleSuggest>> GetSuggestTitelsAsync(CancellationToken token, string tranName)
	{
		return _dbRepository.GetSuggestTitelsAsync(token, tranName);
	}

	public Task<int> NextNumberAsync(CancellationToken token, string tranName)
	{
		return _dbRepository.NextNumberAsync(token, tranName);
	}

	public Task<TicketBusiness> GetByNumberAsync(int number, CancellationToken token, string tranName)
	{
		return _dbRepository.GetByNumberAsync(number, token, tranName);
	}
}
