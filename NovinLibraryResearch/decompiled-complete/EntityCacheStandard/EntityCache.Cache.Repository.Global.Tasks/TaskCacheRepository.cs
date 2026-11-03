using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Global.Tasks;
using EntityCache.Core;
using EntityCache.Core.Repository.Global.Tasks;
using EntityCache.Mapping.Global.Tasks;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Global.Tasks;

public class TaskCacheRepository : CacheRepository<TaskBusiness>, ITaskRepository, IRepository<TaskBusiness>, INoGuidRepository<TaskBusiness>, IDisposable
{
	private ITaskRepository _dbRepository;

	private bool _isDisposed;

	public override IItemAsyncMapper<TaskBusiness, TaskBusiness> Mapper => TaskMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, TaskBusiness> TimingCache { get; set; }

	public TaskCacheRepository(ITaskRepository dbRepository, int CachingTime, bool cachingEnable = true)
		: base((IRepository<TaskBusiness>)dbRepository, CachingTime, cachingEnable)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, TaskBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public override void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_dbRepository?.Dispose();
				_dbRepository = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Global\\Tasks\\Tasks\\TaskCacheRepository.cs", 39);
		}
	}

	public Task<int> GetNextId2Async(CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		try
		{
			return _dbRepository.GetNextId2Async(token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetNextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Global\\Tasks\\Tasks\\TaskCacheRepository.cs", 52);
			throw;
		}
	}
}
