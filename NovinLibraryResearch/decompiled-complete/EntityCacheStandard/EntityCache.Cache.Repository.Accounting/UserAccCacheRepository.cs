using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Mapping.BusinessMappers.Accounting;
using PacketParser.BusinessLayer;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting;

public class UserAccCacheRepository : SyncCacheRepository<UserAccBusiness>, IUserAccRepository, ISyncRepository<UserAccBusiness>, IEditableDelivery, IRepository<UserAccBusiness>, INoGuidRepository<UserAccBusiness>, IDisposable
{
	private IUserAccRepository DbUserAddRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public override IItemAsyncMapper<UserAccBusiness, UserAccBusiness> Mapper => UserAccMapper.Instance;

	protected override TimingCache<Guid, UserAccBusiness> TimingCache { get; set; }

	public UserAccCacheRepository(ISyncRepository<UserAccBusiness> dbRepository, int CachingTime)
		: base(dbRepository, CachingTime, cachingEnable: true)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		DbUserAddRepository = (IUserAccRepository)dbRepository;
		TimingCache = new TimingCache<Guid, UserAccBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public override async Task<IEnumerable<UserAccBusiness>> GetAllAsync(bool? status, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\UserAccCacheRepository.cs"))
		{
			_ = 1;
			try
			{
				if (base.IsDisposed)
				{
					throw new ObjectDisposedException(GetType().Name);
				}
				IEnumerable<UserAccBusiness> result = await base.DbRepository.GetAllAsync(status, token, tranName);
				if (result?.Any() ?? false)
				{
					foreach (UserAccBusiness item in result)
					{
						await TimingCache.UpdateTimeAsync(item.Guid, item, tranName);
					}
				}
				return result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\UserAccCacheRepository.cs", 47);
				return new List<UserAccBusiness>();
			}
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(UserAccBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		int line = 0;
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\UserAccCacheRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				line = 1;
				if (base.IsDisposed)
				{
					throw new ObjectDisposedException(GetType().Name);
				}
				line = 2;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await base.DbRepository.SaveAsync(item, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\UserAccCacheRepository.cs", 63);
				if (!ret.HasError)
				{
					await TimingCache.UpdateTimeAsync(item.Guid, item, tranName);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}//ClsCache.GetCache(contextGuid).IsDisposed:{ClsCache.GetCache(base.ContextGuid)?.IsDisposed ?? true}//DbRepository is null:{base.DbRepository == null}", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\UserAccCacheRepository.cs", 74);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\UserAccCacheRepository.cs", 75);
			}
			return ret;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<UserAccBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException("نباید رنجی ذخیره بشه.");
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				DbUserAddRepository = null;
				DbUserAddRepository?.Dispose();
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\UserAccCacheRepository.cs", 109);
		}
	}

	public Task<UserAccBusiness> GetAsync(string name, CancellationToken token, string tranName)
	{
		return DbUserAddRepository.GetAsync(name, token, tranName);
	}
}
