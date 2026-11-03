using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Mapping.BusinessMappers;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository;

public class PardakhtCacheRepository : CacheEntityRepository<PardakhtBusiness, PardakhtReportBusiness>, IPardakhtRepository, IEntityRepository<PardakhtBusiness, PardakhtReportBusiness>, IEditableDelivery, IDisposable
{
	private IPardakhtRepository DbRepository { get; set; }

	public override IItemAsyncMapper<PardakhtBusiness, PardakhtBusiness> Mapper => PardakhtMapper.Instance;

	public PardakhtCacheRepository(IPardakhtRepository dbRepository, int CachingTimeSeconds)
		: base((IEntityRepository<PardakhtBusiness, PardakhtReportBusiness>)dbRepository, CachingTimeSeconds)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		DbRepository = dbRepository;
	}

	public Task<IEnumerable<PardakhtReportBusiness>> GetAllAsync(KhazaneFilter filter, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return DbRepository.GetAllAsync(filter, token, tranName);
	}

	public Task<Guid> Id2ToGuidAsync(int id2, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return DbRepository.Id2ToGuidAsync(id2, token, tranName);
	}

	public async Task<int> NextId2Async(int? statusId, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("NextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Khazane\\Pardakht\\PardakhtCacheRepository.cs"))
		{
			_ = 1;
			try
			{
				int num = await DbRepository.NextId2Async(statusId, token, tranName);
				if (num == 1 && ClsCache.GetCache(base.ContextGuid).Source != ENSource.Accounting)
				{
					num = checked(((await UserInformationBusiness.GetAsync(base.ContextGuid))?.LastSaved?.LastPardakhtId2).GetValueOrDefault() + 1);
				}
				return num;
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "NextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Khazane\\Pardakht\\PardakhtCacheRepository.cs", 53);
				throw;
			}
		}
	}

	public Task<PardakhtBusiness> GetFromDbAsync(Guid guid, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return DbRepository.GetAsync(guid, token, tranName);
	}

	public Task<List<Guid>> GetNotTransferedAsync(CancellationToken token, string tranName)
	{
		return DbRepository.GetNotTransferedAsync(token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> UpdateTransferDateAsync(OperationTransfer filter, CancellationToken token, string tranName)
	{
		return DbRepository.UpdateTransferDateAsync(filter, token, tranName);
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				DbRepository = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Khazane\\Pardakht\\PardakhtCacheRepository.cs", 73);
		}
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(PardakhtBusiness item, EnSystem system, short tryCount, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo obj = await DbRepository.SaveAsync(item, system, tryCount, token, tranName);
		if (!obj.HasError)
		{
			ClsCache.GetCache(base.ContextGuid).CacheRepository.IViewCheckShRepository.ResetCache();
			ClsCache.GetCache(base.ContextGuid).CacheRepository.IViewCheckMRepository.ResetCache();
			ClsCache.GetCache(base.ContextGuid).CacheRepository.IPardakht.ResetCache();
		}
		return obj;
	}

	public Task<ReturnedSaveFuncInfo> UpdateCustomerFullNameAsync(Guid tafsilGuid, string name, CancellationToken token, string tranName)
	{
		return DbRepository.UpdateCustomerFullNameAsync(tafsilGuid, name, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> UpdateDateMoaserAsync(Guid? guid, CancellationToken token, string tranName)
	{
		return DbRepository.UpdateDateMoaserAsync(guid, token, tranName);
	}

	public Task<List<ENSource>> GetAllExistingSourceAsync(CancellationToken token, string tranName)
	{
		return DbRepository.GetAllExistingSourceAsync(token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> ChangeStatusIdAsync(Guid guid, int statusId, CancellationToken token, string tranName)
	{
		return DbRepository.ChangeStatusIdAsync(guid, statusId, token, tranName);
	}
}
