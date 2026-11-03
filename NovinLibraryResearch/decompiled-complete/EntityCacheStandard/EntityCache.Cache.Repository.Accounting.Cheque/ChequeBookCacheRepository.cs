using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Cheque;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Cheque;
using EntityCache.Mapping.BusinessMappers.Accounting.Cheque;
using PacketParser.BusinessLayer;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting.Cheque;

public class ChequeBookCacheRepository : CacheRepository<ChequeBookBusiness>, IChequeBookRepository, IRepository<ChequeBookBusiness>, INoGuidRepository<ChequeBookBusiness>, IDisposable
{
	private IChequeBookRepository dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public override IItemAsyncMapper<ChequeBookBusiness, ChequeBookBusiness> Mapper => ChequeBookMapper.Instance;

	protected override TimingCache<Guid, ChequeBookBusiness> TimingCache { get; set; }

	public new bool IsDisposed { get; private set; }

	public ChequeBookCacheRepository(IChequeBookRepository dbRepository, int CachingTime)
		: base((IRepository<ChequeBookBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		this.dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, ChequeBookBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public Task<List<ChequeBookBusiness>> GetAllAsync(Guid? bankGuid, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAllAsync(bankGuid, token, tranName);
	}

	public Task<ChequeBookBusiness> GetAsync(string serialNumber, Guid bankGuid, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAsync(serialNumber, bankGuid, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> RemoveAllAsync(Guid? bankGuid, short tryCount, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.RemoveAllAsync(bankGuid, tryCount, token, tranName);
	}

	public Task<string> NextSerialAsync(Guid BankGuid, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.NextSerialAsync(BankGuid, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> SaveRangeAsync(long number1, long number2, Guid bankGuid, bool isSadershode, short tryCount, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.SaveRangeAsync(number1, number2, bankGuid, isSadershode, tryCount, token, tranName);
	}

	public override void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				dbRepository = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Cheque\\ChequeBookCacheRepository.cs", 73);
		}
	}
}
