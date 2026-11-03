using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting;
using EntityCache.Mapping.BusinessMappers.Accounting;
using PacketParser.BusinessLayer;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;
using PacketParser.Mapping;
using PacketParser.Records.AnbarGardani;

namespace EntityCache.Cache.Repository;

public class PrdAnbarGardaniCacheRepository : CacheEntityRepository<PrdAnbarGardaniBusiness, PrdAnbarGardaniReportBusiness>, IPrdAnbarGardaniRepository, IEntityRepository<PrdAnbarGardaniBusiness, PrdAnbarGardaniReportBusiness>, IEditableDelivery, IDisposable
{
	private IPrdAnbarGardaniRepository dbRepository;

	public bool HasStatus => true;

	public bool HasModified => true;

	public override IItemAsyncMapper<PrdAnbarGardaniBusiness, PrdAnbarGardaniBusiness> Mapper => PrdAnbarGardaniMapper.Instance;

	protected new TimingCache<Guid, PrdAnbarGardaniBusiness> TimingCache { get; set; }

	public PrdAnbarGardaniCacheRepository(IPrdAnbarGardaniRepository dbRepository, int CachingTime)
		: base((IEntityRepository<PrdAnbarGardaniBusiness, PrdAnbarGardaniReportBusiness>)dbRepository, CachingTime)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		this.dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, PrdAnbarGardaniBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public Task<PrdAnbarGardaniBusiness> GetByNumberAsync(int number, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetByNumberAsync(number, token, tranName);
	}

	public Task<PrdAnbarGardaniBusiness> GetAsync(int sanadNumber, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAsync(sanadNumber, token, tranName);
	}

	public Task<int> NextNumberAsync(int? statusId, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.NextNumberAsync(statusId, token, tranName);
	}

	public Task<List<PrdAnbarGardaniDetBusiness>> GetAllAsync(PrdAnbarGardaniBusiness master, AnbarGardaniFilter filter, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAllAsync(master, filter, token, tranName);
	}

	public Task<PrdAnbarGardaniBusiness> GetFromDbAsync(Guid guid, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetFromDbAsync(guid, token, tranName);
	}

	public override Task<PrdAnbarGardaniBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetFromDbAsync(guid, token, tranName);
	}

	public Task<List<PrdAnbarGardaniReportBusiness>> GetAllAsync(AnbarGardaniFilter filter, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAllAsync(filter, token, tranName);
	}

	public Task<Guid> GetDepotGuidAsync(Guid detailGuid, CancellationToken token, string tranName)
	{
		return dbRepository.GetDepotGuidAsync(detailGuid, token, tranName);
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				dbRepository = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\PrdAnbarGardaniCacheRepository.cs", 77);
		}
	}

	public Task<ReturnedSaveFuncInfo> UpdateTypeAsync(Guid guid, SanadType type, CancellationToken token, short tryCount, string tranName)
	{
		return dbRepository.UpdateTypeAsync(guid, type, token, tryCount, tranName);
	}

	public Task<ReturnedSaveFuncInfo> UpdateDateAsync(Guid guid, DateTime date, CancellationToken token, string tranName)
	{
		return dbRepository.UpdateDateAsync(guid, date, token, tranName);
	}

	public Task<List<AnbarGardaniEkhtetamRecord>> GetAllEkhtetamRecordsAsync(CancellationToken token, string tranName)
	{
		return dbRepository.GetAllEkhtetamRecordsAsync(token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> ChangeStatusIdAsync(Guid guid, int statusId, CancellationToken token, string tranName)
	{
		return dbRepository.ChangeStatusIdAsync(guid, statusId, token, tranName);
	}
}
