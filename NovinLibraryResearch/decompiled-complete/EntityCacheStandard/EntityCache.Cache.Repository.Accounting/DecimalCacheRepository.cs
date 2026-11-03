using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting;
using EntityCache.Mapping.BusinessMappers.Accounting;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting;

public class DecimalCacheRepository : CacheRepository<DecimalBusiness>, IDecimalRepository, IRepository<DecimalBusiness>, INoGuidRepository<DecimalBusiness>, IDisposable
{
	private IDecimalRepository _dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, DecimalBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<DecimalBusiness, DecimalBusiness> Mapper => DecimalMapper.Instance;

	public DecimalCacheRepository(IDecimalRepository dbRepository, int CachingTime)
		: base((IRepository<DecimalBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, DecimalBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				_dbRepository = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\DecimalCacheRepository.cs", 38);
		}
	}

	public async Task<DecimalBusiness> GetAsync(string name, string country, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\DecimalCacheRepository.cs"))
		{
			_ = 2;
			try
			{
				DecimalBusiness decimalBusiness = (await GetAllAsync(token, tranName)).FirstOrDefault((DecimalBusiness x) => x.Name == name && x.Country == country);
				if (decimalBusiness != null)
				{
					return decimalBusiness;
				}
				DecimalBusiness res = await ((IDecimalRepository)base.DbRepository).GetAsync(name, country, token, tranName);
				if (res != null)
				{
					await TimingCache.UpdateTimeAsync(res.Guid, res);
				}
				return res;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\DecimalCacheRepository.cs", 56);
				throw;
			}
		}
	}

	public Task<DecimalBusiness> GetAsync(string slug, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAsync(slug, token, tranName);
	}
}
