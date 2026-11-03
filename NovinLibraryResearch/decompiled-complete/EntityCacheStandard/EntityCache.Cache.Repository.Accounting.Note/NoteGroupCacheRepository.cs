using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Note;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Note;
using EntityCache.Mapping.BusinessMappers.Accounting.Note;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting.Note;

internal class NoteGroupCacheRepository : CacheRepository<NoteGroupBusiness>, INoteGroupRepository, IRepository<NoteGroupBusiness>, INoGuidRepository<NoteGroupBusiness>, IDisposable
{
	private INoteGroupRepository dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public override IItemAsyncMapper<NoteGroupBusiness, NoteGroupBusiness> Mapper => NoteGroupMapper.Instance;

	protected override TimingCache<Guid, NoteGroupBusiness> TimingCache { get; set; }

	public NoteGroupCacheRepository(INoteGroupRepository dbRepository, int CachingTime)
		: base((IRepository<NoteGroupBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		this.dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, NoteGroupBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public Task<NoteGroupBusiness> CheckNameAsync(Guid guid, string Name, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.CheckNameAsync(guid, Name, token, tranName);
	}

	public Task<int> CountGroupChildsAsync(Guid groupGuid, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.CountGroupChildsAsync(groupGuid, token, tranName);
	}

	public Task<List<NoteGroupBusiness>> GetAllAsync(Guid parentGuid, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAllAsync(parentGuid, token, tranName);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Note\\NoteGroupCacheRepository.cs", 54);
		}
	}
}
