using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Note;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Core.Repository.Accounting.Note;
using EntityCache.Mapping.BusinessMappers.Accounting.Note;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting.Note;

public class NoteCacheRepository : SyncCacheRepository<NoteBusiness>, INoteRepository, ISyncRepository<NoteBusiness>, IEditableDelivery, IRepository<NoteBusiness>, INoGuidRepository<NoteBusiness>, IDisposable
{
	private INoteRepository dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public override IItemAsyncMapper<NoteBusiness, NoteBusiness> Mapper => NoteMapper.Instance;

	protected override TimingCache<Guid, NoteBusiness> TimingCache { get; set; }

	public NoteCacheRepository(INoteRepository dbRepository, int CachingTime)
		: base((ISyncRepository<NoteBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		this.dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, NoteBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public Task<int> CountNumbersInGroupAsync(Guid groupGuid, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.CountNumbersInGroupAsync(groupGuid, token, tranName);
	}

	public Task<IEnumerable<NoteBusiness>> GetAllAsync(Guid groupGuid, NoteStatus? status, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAllAsync(groupGuid, status, token, tranName);
	}

	public Task<List<NoteBusiness>> GetAllAsync(CancellationToken token, NoteFilter filter, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAllAsync(token, filter, tranName);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Note\\NoteCacheRepository.cs", 55);
		}
	}
}
