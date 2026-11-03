using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Coding;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Core.Repository.Accounting.Coding;

public interface IAccountRepository : ISyncRepository<AccountBusiness>, IEditableDelivery, IRepository<AccountBusiness>, INoGuidRepository<AccountBusiness>, IDisposable
{
	Task<ReturnedSaveFuncInfo> RemoveAsync(Guid moeinGuid, Guid tafsilGuid, decimal account, string tranName, CancellationToken token);

	Task<IEnumerable<AccountTableViewModel>> GetAllAsync(Guid tafsilGuid, bool? status, string tranName, CancellationToken token);

	Task<decimal> GetAccountSumAsync(List<Guid> moeinGuidList, List<Guid> tafsilGuidList, bool? status, string tranName, CancellationToken token);

	Task<decimal> GetAccountSumAsync(Guid tafsilGuid, List<Guid> moeinGuids, string tranName, CancellationToken token);

	Task<ReturnedSaveFuncInfo> RemoveNotRelatedAccountsAsync(List<Tuple<Guid, List<Guid>>> data, string tranName, CancellationToken token);

	Task<ReturnedSaveFuncInfo> SetAccountAsync(AccountBusiness item, short tryCount, string tranName, CancellationToken token);

	Task<ReturnedSaveFuncInfo> BallanceAccountAsync(AccountBusiness item, short tryCount, string tranName, CancellationToken token);

	Task<ReturnedSaveFuncInfo> SaveBallanceAsync(Guid moeinGuid, Guid tafsilGuid, byte[]? gardesh, string tranName, CancellationToken token);

	Task<ReturnedSaveFuncInfoWithValue<byte[]>> GetBallanceAsync(Guid moeinGuid, Guid tafsilGuid, string tranName, CancellationToken token);

	Task<ReturnedSaveFuncInfo> SaveRangeAsync(List<WebAccount> accounts, string tranName, CancellationToken token);

	Task<IEnumerable<AccountBusiness>> GetAllAsync(Guid? moeinGuid, Guid? tafsilGuid, bool? status, CancellationToken token, string tranName);

	Task<IEnumerable<AccountBusiness>> GetAllAsync(List<Guid> tafsilGuidList, bool? status, CancellationToken token, string tranName);
}
