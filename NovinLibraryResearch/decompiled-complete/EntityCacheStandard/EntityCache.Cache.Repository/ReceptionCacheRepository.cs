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

public class ReceptionCacheRepository : CacheEntityRepository<ReceptionBusiness, ReceptionReportBusiness>, IReceptionRepository, IEntityRepository<ReceptionBusiness, ReceptionReportBusiness>, IEditableDelivery, IDisposable
{
	protected IReceptionRepository DbRepository { get; set; }

	public override IItemAsyncMapper<ReceptionBusiness, ReceptionBusiness> Mapper => ReceptionMapper.Instance;

	public ReceptionCacheRepository(IReceptionRepository dbRepository, int CachingTimeSeconds)
		: base((IEntityRepository<ReceptionBusiness, ReceptionReportBusiness>)dbRepository, CachingTimeSeconds)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		DbRepository = dbRepository;
	}

	public Task<List<ReceptionReportBusiness>> GetAllAsync(KhazaneFilter filters, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return DbRepository.GetAllAsync(filters, token, tranName);
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(ReceptionBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo obj = await base.SaveAsync(item, tryCount, token, tranName);
		if (!obj.HasError)
		{
			ClsCache.GetCache(base.ContextGuid).CacheRepository.IViewCheckShRepository.ResetCache();
			ClsCache.GetCache(base.ContextGuid).CacheRepository.IViewCheckMRepository.ResetCache();
			ClsCache.GetCache(base.ContextGuid).CacheRepository.IReception.ResetCache();
		}
		return obj;
	}

	public async Task<ReceptionBusiness> GetAsync(Guid guid, OrderBusiness order, EnSystem system, CancellationToken token, string tranName)
	{
		_ = 1;
		try
		{
			if (base.IsDisposed)
			{
				throw new ObjectDisposedException(GetType().Name);
			}
			if (base.TimingCache.TryGetValue(guid, out var ret) && ret != null)
			{
				return ret;
			}
			ret = await DbRepository.GetAsync(guid, order, system, token, tranName);
			await base.TimingCache.UpdateTimeAsync(guid, ret, tranName);
			return ret;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Khazane\\Reception\\ReceptionCacheRepository.cs", 59);
			throw;
		}
	}

	public async Task<int> NextId2Async(int? statusId, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("NextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Khazane\\Reception\\ReceptionCacheRepository.cs"))
		{
			_ = 1;
			int num;
			try
			{
				num = await DbRepository.NextId2Async(statusId, token, tranName);
				if (num == 1 && ClsCache.GetCache(base.ContextGuid).Source != ENSource.Accounting)
				{
					num = checked(((await UserInformationBusiness.GetAsync(base.ContextGuid))?.LastSaved?.LastReceptionId2).GetValueOrDefault() + 1);
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "NextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Khazane\\Reception\\ReceptionCacheRepository.cs", 80);
				throw;
			}
			return num;
		}
	}

	public Task<Guid> Id2ToGuidAsync(int id2, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return DbRepository.Id2ToGuidAsync(id2, token, tranName);
	}

	public Task<ReceptionBusiness> GetFromDbAsync(Guid guid, OrderBusiness order, EnSystem system, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return DbRepository.GetAsync(guid, order, system, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> SetReceptionTypeBySellAsync(List<Guid> lst1, List<Guid> lst2, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return DbRepository.SetReceptionTypeBySellAsync(lst1, lst2, token, tranName);
	}

	public Task<List<Guid>> GetNotTransferedAsync(CancellationToken token, string tranName)
	{
		return DbRepository.GetNotTransferedAsync(token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> UpdateTransferDateAsync(OperationTransfer filter, CancellationToken token, string tranName)
	{
		return DbRepository.UpdateTransferDateAsync(filter, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> UpdateCustomerFullNameAsync(Guid tafsilGuid, string name, CancellationToken token, string tranName)
	{
		return DbRepository.UpdateCustomerFullNameAsync(tafsilGuid, name, token, tranName);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Khazane\\Reception\\ReceptionCacheRepository.cs", 114);
		}
	}

	public Task<ReturnedSaveFuncInfo> UpdateDateMoaserAsync(Guid? guid, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
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
