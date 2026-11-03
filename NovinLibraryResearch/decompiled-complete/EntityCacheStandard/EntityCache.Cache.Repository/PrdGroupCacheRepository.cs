using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Mapping.BusinessMappers;
using PacketParser.BusinessLayer;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository;

public class PrdGroupCacheRepository : SyncCacheRepository<PrdGroupBusiness>, IProductGroupRepository, ISyncRepository<PrdGroupBusiness>, IEditableDelivery, IRepository<PrdGroupBusiness>, INoGuidRepository<PrdGroupBusiness>, IDisposable
{
	private IProductGroupRepository _dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public override IItemAsyncMapper<PrdGroupBusiness, PrdGroupBusiness> Mapper => PrdGroupMapper.Instance;

	protected override TimingCache<Guid, PrdGroupBusiness> TimingCache { get; set; }

	public PrdGroupCacheRepository(IProductGroupRepository dbRepository, int CachingTime)
		: base((ISyncRepository<PrdGroupBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, PrdGroupBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public async Task<IEnumerable<PrdGroupBusiness>> GetListByGrpNameAsync(string name, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetListByGrpNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\PrdGroupCacheRepository.cs"))
		{
			_ = 2;
			try
			{
				if (base.IsDisposed)
				{
					throw new ObjectDisposedException(GetType().Name);
				}
				List<PrdGroupBusiness> list = (await GetAllAsync(token, tranName)).Where((PrdGroupBusiness x) => x.Name.Contains(name)).ToList();
				if (list.Any())
				{
					return list;
				}
				IEnumerable<PrdGroupBusiness> res = await _dbRepository.GetListByGrpNameAsync(name, token, tranName);
				if (res != null)
				{
					foreach (PrdGroupBusiness item in res)
					{
						await TimingCache.UpdateTimeAsync(item.Guid, item, tranName);
					}
				}
				return res;
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "GetListByGrpNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\PrdGroupCacheRepository.cs", 46);
			}
			return null;
		}
	}

	public async Task<PrdGroupBusiness> GetAsync(long fullCode, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\PrdGroupCacheRepository.cs"))
		{
			int line = 0;
			try
			{
				PrdGroupBusiness prdGroupBusiness = (await GetAllAsync(token, tranName)).FirstOrDefault((PrdGroupBusiness x) => x.FullCode == fullCode);
				line = 1;
				if (prdGroupBusiness != null)
				{
					return prdGroupBusiness;
				}
				line = 2;
				PrdGroupBusiness res = await _dbRepository.GetAsync(fullCode, token, tranName);
				line = 3;
				if (res != null)
				{
					await TimingCache.UpdateTimeAsync(res.Guid, res, tranName);
				}
				line = 4;
				return res;
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"#Line_{line}", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\PrdGroupCacheRepository.cs", 70);
				throw;
			}
		}
	}

	public async Task<PrdGroupBusiness> GetAsync(string name, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\PrdGroupCacheRepository.cs"))
		{
			_ = 2;
			try
			{
				PrdGroupBusiness prdGroupBusiness = (await GetAllAsync(token, tranName)).FirstOrDefault((PrdGroupBusiness x) => x.Name == name);
				if (prdGroupBusiness != null)
				{
					return prdGroupBusiness;
				}
				PrdGroupBusiness res = await _dbRepository.GetAsync(name, token, tranName);
				if (res != null)
				{
					await TimingCache.UpdateTimeAsync(res.Guid, res, tranName);
				}
				return res;
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\PrdGroupCacheRepository.cs", 90);
				throw;
			}
		}
	}

	public Task<long> GetNextCodeAsync(Guid parent, CancellationToken token, string tranName)
	{
		return _dbRepository.GetNextCodeAsync(parent, token, tranName);
	}

	public Task<IEnumerable<PrdGroupBusiness>> GetAllAsync(string search, CancellationToken token, bool? status, string tranName)
	{
		try
		{
			if (base.IsDisposed)
			{
				throw new ObjectDisposedException(GetType().Name);
			}
			return _dbRepository.GetAllAsync(search, token, status, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\PrdGroupCacheRepository.cs", 104);
			throw;
		}
	}

	public override async Task<IEnumerable<PrdGroupBusiness>> GetAllAsync(bool? status, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("GetAllAsync", $"CacheRepository.GetAll_{typeof(PrdGroupBusiness)}"))
		{
			try
			{
				await LoadCacheAsync(token, tranName);
				IEnumerable<PrdGroupBusiness> enumerable = TimingCache.GetAll();
				if (status.HasValue)
				{
					enumerable = enumerable.Where((PrdGroupBusiness w) => w.Status == status);
				}
				return enumerable;
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\PrdGroupCacheRepository.cs", 121);
			}
			return null;
		}
	}

	public async Task<ReturnedSaveFuncInfoWithValue<bool>> HasImageAsync(string fileName, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("HasImageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\PrdGroupCacheRepository.cs"))
		{
			ReturnedSaveFuncInfoWithValue<bool> ret = new ReturnedSaveFuncInfoWithValue<bool>();
			try
			{
				ret.value = (await GetAllAsync(token, tranName))?.Any((PrdGroupBusiness w) => w.Pic == fileName) ?? false;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "HasImageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\PrdGroupCacheRepository.cs", 136);
				ret.AddReturnedValue(ex, "", "HasImageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\PrdGroupCacheRepository.cs", 137);
			}
			return ret;
		}
	}

	public async Task<IEnumerable<PrdGroupBusiness>> GetChildrenAsync(Guid parentGuid, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetChildrenAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\PrdGroupCacheRepository.cs"))
		{
			try
			{
				if (parentGuid == Guid.Empty)
				{
					return null;
				}
				return (await GetAllAsync(true, token, tranName))?.Where((PrdGroupBusiness w) => w.ParentGuid == parentGuid);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "GetChildrenAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\PrdGroupCacheRepository.cs", 153);
				throw;
			}
		}
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\PrdGroupCacheRepository.cs", 164);
		}
	}

	public void ResetCache(CancellationToken token, string tranName)
	{
		TimingCache?.ClearAll();
	}

	public Task<ReturnedSaveFuncInfo> UpdateSubGroupSpecialAsync(Guid masterGuid, bool special, string technicalInfo, CancellationToken token, string tranName)
	{
		return _dbRepository.UpdateSubGroupSpecialAsync(masterGuid, special, technicalInfo, token, tranName);
	}
}
