using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Evaluation;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting;
using EntityCache.Mapping.BusinessMappers.Accounting;
using PacketParser.BusinessLayer.Models.Evaluation;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Filters;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting;

public class EvaluationCacheRepository : CacheRepository<EvaluationBusiness>, IEvaluationRepository, IRepository<EvaluationBusiness>, INoGuidRepository<EvaluationBusiness>, IDisposable
{
	private IEvaluationRepository _dbRepository;

	public override IItemAsyncMapper<EvaluationBusiness, EvaluationBusiness> Mapper => EvaluationMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	protected override TimingCache<Guid, EvaluationBusiness> TimingCache { get; set; }

	public EvaluationCacheRepository(IEvaluationRepository dbRepository, int CachingTime, bool cachingEnable = true)
		: base((IRepository<EvaluationBusiness>)dbRepository, CachingTime, cachingEnable)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, EvaluationBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public override void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				_dbRepository = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\EvaluationCacheRepository.cs", 42);
		}
	}

	public Task<IEnumerable<EvaluationModel>> GetAllAsync(EvaluationFilter filter, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllAsync(filter, token, tranName);
	}

	public Task<List<EvaluationDetailModel>> GetAllDetailsAsync(Guid? guid, DateTime dateM, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllDetailsAsync(guid, dateM, token, tranName);
	}

	public Task<int> GetNextId2Async(CancellationToken token, string tranName)
	{
		return _dbRepository.GetNextId2Async(token, tranName);
	}

	public Task<EvaluationBusiness> GetAsync(int number, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAsync(number, token, tranName);
	}

	public Task<EvaluationBusiness> GetByAnbargardaniAsync(Guid anbarGardaniGuid, CancellationToken token, string tranName)
	{
		return _dbRepository.GetByAnbargardaniAsync(anbarGardaniGuid, token, tranName);
	}
}
