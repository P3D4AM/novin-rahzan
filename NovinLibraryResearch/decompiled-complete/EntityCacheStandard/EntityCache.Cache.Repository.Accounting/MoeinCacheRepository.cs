using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Core.Repository.Accounting;
using EntityCache.Mapping.BusinessMappers.Accounting;
using PacketParser.BusinessLayer;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting;

public class MoeinCacheRepository : SyncCacheRepository<MoeinBusiness>, IMoeinRepository, ISyncRepository<MoeinBusiness>, IEditableDelivery, IRepository<MoeinBusiness>, INoGuidRepository<MoeinBusiness>, IDisposable
{
	private IMoeinRepository dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public override IItemAsyncMapper<MoeinBusiness, MoeinBusiness> Mapper => MoeinMapper.Instance;

	protected override TimingCache<Guid, MoeinBusiness> TimingCache { get; set; }

	public MoeinCacheRepository(IMoeinRepository dbRepository, int CachingTime)
		: base((ISyncRepository<MoeinBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		this.dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, MoeinBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public Task<byte[]> GetGardeshHesabAsync(Guid guid, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetGardeshHesabAsync(guid, token, tranName);
	}

	public async Task<MoeinBusiness> GetAsync(long moeinCode, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		MoeinBusiness ret2 = TimingCache.GetAll()?.SingleOrDefault((MoeinBusiness p) => p.Code == moeinCode);
		if (ret2 != null)
		{
			return ret2;
		}
		ret2 = await dbRepository.GetAsync(moeinCode, token, tranName);
		if (ret2 != null)
		{
			await TimingCache.UpdateTimeAsync(ret2.Guid, ret2);
		}
		return ret2;
	}

	public async Task<IEnumerable<MoeinBusiness>> GetAllBetweenCodesAsync(long? code1, long? code2, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return await dbRepository.GetAllBetweenCodesAsync(code1, code2, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> SaveMoeinRelationTafsilLevelAsync(Guid moeinGuid, List<TafsilGroupBusiness> lst, short tryCount, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return SaveMoeinRelationTafsilLevelAsync(moeinGuid, lst, tryCount, token, tranName);
	}

	public Task<List<TafsilGroupBusiness>> GetAllMoeinRelationTafsilLevelAsync(Guid moeinGuid)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return GetAllMoeinRelationTafsilLevelAsync(moeinGuid);
	}

	public async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<MoeinBusiness> lstHesabsListMoein, bool updateAccount, short tryCount, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		if (lstHesabsListMoein == null || !lstHesabsListMoein.Any())
		{
			return new ReturnedSaveFuncInfo();
		}
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Coding\\MoeinCacheRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await dbRepository.SaveRangeAsync(lstHesabsListMoein, updateAccount, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Coding\\MoeinCacheRepository.cs", 73);
				if (updateAccount)
				{
					foreach (MoeinBusiness item in lstHesabsListMoein)
					{
						await TimingCache.UpdateTimeAsync(item.Guid, item, tranName);
					}
				}
				else
				{
					TimingCache.ClearAll();
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Coding\\MoeinCacheRepository.cs", 83);
				ret.AddReturnedValue(ex2, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Coding\\MoeinCacheRepository.cs", 84);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> UpdateAccountAsync(Guid guid, decimal p0, short tryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("UpdateAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Coding\\MoeinCacheRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				if (base.IsDisposed)
				{
					throw new ObjectDisposedException(GetType().Name);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await dbRepository.UpdateAccountAsync(guid, p0, tryCount, token, tranName), ReturnedState.None, "UpdateAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Coding\\MoeinCacheRepository.cs", 96);
				MoeinBusiness moeinBusiness = await base.DbRepository.GetAsync(guid, token, tranName);
				if (moeinBusiness != null)
				{
					await TimingCache.UpdateTimeAsync(moeinBusiness.Guid, moeinBusiness, tranName);
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "UpdateAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Coding\\MoeinCacheRepository.cs", 104);
				ret.AddReturnedValue(ex2, "", "UpdateAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Coding\\MoeinCacheRepository.cs", 105);
			}
			return ret;
		}
	}

	public Task<IEnumerable<MoeinBusiness>> GetAllAsync(string search, Guid? kolGuid, Guid? tafsilLevelGuid, CancellationToken token, bool? status, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAllAsync(search, kolGuid, tafsilLevelGuid, token, status, tranName);
	}

	public Task<long> NextCodeInGroupAsync(Guid KolGuid, long startCode, long endCode, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.NextCodeInGroupAsync(KolGuid, startCode, endCode, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(MoeinBusiness moein, bool updateAccount, short tryCount, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Coding\\MoeinCacheRepository.cs"))
		{
			_ = 1;
			try
			{
				if (base.IsDisposed)
				{
					throw new ObjectDisposedException(GetType().Name);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await dbRepository.SaveAsync(moein, updateAccount, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Coding\\MoeinCacheRepository.cs", 126);
				if (moein != null && !ret.HasError)
				{
					await TimingCache.UpdateTimeAsync(moein.Guid, moein, tranName);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"IsDisposed:{base.IsDisposed}", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Coding\\MoeinCacheRepository.cs", 132);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Coding\\MoeinCacheRepository.cs", 133);
			}
			return ret;
		}
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Coding\\MoeinCacheRepository.cs", 145);
		}
	}

	public Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<WebMoein> items, CancellationToken token, string tranName)
	{
		return dbRepository.SaveRangeAsync(items, token, tranName);
	}
}
