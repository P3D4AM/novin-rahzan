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

public class KolCacheRepository : SyncCacheRepository<KolBusiness>, IKolRepository, ISyncRepository<KolBusiness>, IEditableDelivery, IRepository<KolBusiness>, INoGuidRepository<KolBusiness>, IDisposable
{
	private IKolRepository dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, KolBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<KolBusiness, KolBusiness> Mapper => KolMapper.Instance;

	public KolCacheRepository(IKolRepository dbRepository, int CachingTime)
		: base((ISyncRepository<KolBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		this.dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, KolBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public Task<KolBusiness> GetAsync(int codeKol, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAsync(codeKol, token, tranName);
	}

	public Task<int> CountChilderenNotZeroAsync(Guid guid, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.CountChilderenNotZeroAsync(guid, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> SaveRangeAsync(List<KolBusiness> lstHesabsListKol, bool updateAccount, short tryCount, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		if (lstHesabsListKol == null || !lstHesabsListKol.Any())
		{
			return new ReturnedSaveFuncInfo();
		}
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Coding\\KolCacheRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await dbRepository.SaveRangeAsync(lstHesabsListKol, updateAccount, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Coding\\KolCacheRepository.cs", 50);
				foreach (KolBusiness item in lstHesabsListKol)
				{
					await TimingCache.UpdateTimeAsync(item.Guid, item, tranName);
				}
				return ret;
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Coding\\KolCacheRepository.cs", 58);
				ret.AddReturnedValue(ex2, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Coding\\KolCacheRepository.cs", 59);
			}
			return ret;
		}
	}

	public Task<IEnumerable<KolBusiness>> GetAllAsync(string search, CancellationToken token, bool? status, Guid? groupGuid, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAllAsync(search, token, status, groupGuid, tranName);
	}

	public Task<int> NextCodeInGroupAsync(Guid groupGuid, int startCode, int endCode, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.NextCodeInGroupAsync(groupGuid, startCode, endCode, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> SaveAsync(KolBusiness kol, bool updateAccount, short tryCount, CancellationToken token, string tranName)
	{
		return dbRepository.SaveAsync(kol, updateAccount, tryCount, token, tranName);
	}

	public Task<IEnumerable<KolBusiness>> GetAllBetweenCodesAsync(long? code1, long? code2, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAllBetweenCodesAsync(code1, code2, token, tranName);
	}

	public Task<byte[]> GetGardeshHesabAsync(Guid guid, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetGardeshHesabAsync(guid, token, tranName);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Coding\\KolCacheRepository.cs", 94);
		}
	}

	public override KolBusiness Get(Guid guid, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		int num = 0;
		try
		{
			num = 1;
			if (guid == Guid.Empty)
			{
				return null;
			}
			num = 2;
			TimingCache.TryGetValue(guid, out var ret);
			num = 3;
			if (ret == null)
			{
				num = 4;
				ret = base.DbRepository.Get(guid, token, tranName);
				num = 5;
				if (ret != null)
				{
					TimingCache.UpdateTimeAsync(ret.Guid, ret, tranName);
				}
				num = 7;
			}
			num = 8;
			return ret;
		}
		catch (ThreadAbortException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, $"Line:{num}//TimingCache is null:{TimingCache == null}//isDisposed:{base.IsDisposed}", "Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Coding\\KolCacheRepository.cs", 123);
			throw;
		}
	}

	public Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<WebKol> items, CancellationToken token, string tranName)
	{
		return dbRepository.SaveRangeAsync(items, token, tranName);
	}
}
