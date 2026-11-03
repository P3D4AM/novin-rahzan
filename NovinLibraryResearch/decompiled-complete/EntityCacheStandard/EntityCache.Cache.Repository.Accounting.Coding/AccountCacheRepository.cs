using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Coding;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Core.Repository.Accounting.Coding;
using EntityCache.Mapping.BusinessMappers.Accounting.Coding;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting.Coding;

public class AccountCacheRepository : SyncCacheRepository<AccountBusiness>, IAccountRepository, ISyncRepository<AccountBusiness>, IEditableDelivery, IRepository<AccountBusiness>, INoGuidRepository<AccountBusiness>, IDisposable
{
	private IAccountRepository _dbRepository;

	public override IItemAsyncMapper<AccountBusiness, AccountBusiness> Mapper => AccountMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, AccountBusiness> TimingCache { get; set; }

	public AccountCacheRepository(IAccountRepository dbRepository, int cachingTime)
		: base((ISyncRepository<AccountBusiness>)dbRepository, cachingTime, cachingEnable: true)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, AccountBusiness>(base.ContextGuid, Mapper, cachingTime);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Coding\\AccountCacheRepository.cs", 40);
		}
	}

	public Task<ReturnedSaveFuncInfo> RemoveAsync(Guid moeinGuid, Guid tafsilGuid, decimal account, string tranName, CancellationToken token)
	{
		return _dbRepository.RemoveAsync(moeinGuid, tafsilGuid, account, tranName, token);
	}

	public Task<IEnumerable<AccountTableViewModel>> GetAllAsync(Guid tafsilGuid, bool? status, string tranName, CancellationToken token)
	{
		return _dbRepository.GetAllAsync(tafsilGuid, status, tranName, token);
	}

	public Task<decimal> GetAccountSumAsync(List<Guid> moeinGuidList, List<Guid> tafsilGuidList, bool? status, string tranName, CancellationToken token)
	{
		return _dbRepository.GetAccountSumAsync(moeinGuidList, tafsilGuidList, status, tranName, token);
	}

	public Task<decimal> GetAccountSumAsync(Guid tafsilGuid, List<Guid> moeinGuids, string tranName, CancellationToken token)
	{
		return _dbRepository.GetAccountSumAsync(tafsilGuid, moeinGuids, tranName, token);
	}

	public Task<ReturnedSaveFuncInfo> RemoveNotRelatedAccountsAsync(List<Tuple<Guid, List<Guid>>> data, string tranName, CancellationToken token)
	{
		return _dbRepository.RemoveNotRelatedAccountsAsync(data, tranName, token);
	}

	public Task<ReturnedSaveFuncInfo> SetAccountAsync(AccountBusiness item, short tryCount, string tranName, CancellationToken token)
	{
		return _dbRepository.SetAccountAsync(item, tryCount, tranName, token);
	}

	public Task<ReturnedSaveFuncInfo> BallanceAccountAsync(AccountBusiness item, short tryCount, string tranName, CancellationToken token)
	{
		return _dbRepository.BallanceAccountAsync(item, tryCount, tranName, token);
	}

	public Task<ReturnedSaveFuncInfo> SaveBallanceAsync(Guid moeinGuid, Guid tafsilGuid, byte[] gardesh, string tranName, CancellationToken token)
	{
		return _dbRepository.SaveBallanceAsync(moeinGuid, tafsilGuid, gardesh, tranName, token);
	}

	public Task<ReturnedSaveFuncInfoWithValue<byte[]>> GetBallanceAsync(Guid moeinGuid, Guid tafsilGuid, string tranName, CancellationToken token)
	{
		return _dbRepository.GetBallanceAsync(moeinGuid, tafsilGuid, tranName, token);
	}

	public Task<ReturnedSaveFuncInfo> SaveRangeAsync(List<WebAccount> accounts, string tranName, CancellationToken token)
	{
		return _dbRepository.SaveRangeAsync(accounts, tranName, token);
	}

	public Task<IEnumerable<AccountBusiness>> GetAllAsync(Guid? moeinGuid, Guid? tafsilGuid, bool? status, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllAsync(moeinGuid, tafsilGuid, status, token, tranName);
	}

	public Task<IEnumerable<AccountBusiness>> GetAllAsync(List<Guid> tafsilGuidList, bool? status, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllAsync(tafsilGuidList, status, token, tranName);
	}
}
