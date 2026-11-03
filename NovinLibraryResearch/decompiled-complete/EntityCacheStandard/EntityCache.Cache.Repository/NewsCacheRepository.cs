using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Mapping.BusinessMappers;
using PacketParser;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository;

public class NewsCacheRepository : CacheRepository<NewsBusiness>, INewsRepository, IRepository<NewsBusiness>, INoGuidRepository<NewsBusiness>, IDisposable
{
	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, NewsBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<NewsBusiness, NewsBusiness> Mapper => NewsMapper.Instance;

	public NewsCacheRepository(IRepository<NewsBusiness> dbRepository, int CachingTime)
		: base(dbRepository, CachingTime, cachingEnable: true)
	{
		TimingCache = new TimingCache<Guid, NewsBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public Task<IEnumerable<NewsBusiness>> GetAllByTypeCategoryAsync(NewsType type, string category, bool? status, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return ((INewsRepository)base.DbRepository).GetAllByTypeCategoryAsync(type, category?.FixString() ?? "", status, token, tranName);
	}

	public async Task<IEnumerable<NewsBusiness>> GetAllByTypeAsync(NewsType type, bool? status, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("GetAllByTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\NewsCacheRepository.cs"))
		{
			_ = 2;
			try
			{
				List<NewsBusiness> list = (await GetAllAsync(token, tranName)).Where((NewsBusiness x) => x.Type == type).ToList();
				if (list.Any())
				{
					return list;
				}
				IEnumerable<NewsBusiness> res = await ((INewsRepository)base.DbRepository).GetAllByTypeAsync(type, status, token, tranName);
				if (res != null)
				{
					foreach (NewsBusiness item in res)
					{
						await TimingCache.UpdateTimeAsync(item.Guid, item, tranName);
					}
				}
				return res;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllByTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\NewsCacheRepository.cs", 49);
				throw;
			}
		}
	}

	public Task<IEnumerable<NewsBusiness>> GetByCategoryAsync(string category, bool? status, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return ((INewsRepository)base.DbRepository).GetByCategoryAsync(category?.FixString() ?? "", status, token, tranName);
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\NewsCacheRepository.cs", 70);
		}
	}
}
