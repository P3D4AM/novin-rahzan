using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;

namespace EntityCache.Core.Repository.Accounting;

public interface ITafsilRepository : ISyncRepository<TafsilBusiness>, IEditableDelivery, IRepository<TafsilBusiness>, INoGuidRepository<TafsilBusiness>, IDisposable
{
	Task<TafsilBusiness> GetAsync(long tafsilCode, CancellationToken token, string tranName);

	Task<string> NextTafsilSayerAsync(Guid ParentGuid, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> UpdateAccountAsync(Guid guid, decimal accountChange, short tryCount, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<TafsilBusiness> list, CancellationToken token, string tranName);

	Task<TafsilBusiness> GetByTypeAndCodeAsync(HesabType hesabType, long code, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> UpdateSumAghsatAsync(Guid guid, decimal accountChange, short tryCount, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfoWithValue<long>> NextCodeInGroupAsync(Guid tfGuid, EnSuggestionType suggestionMode, CancellationToken token, string tanName);

	Task<IEnumerable<TafsilBusiness>> GetAllAsync(EnSystem system, TafsilFilter filter, CancellationToken token, string tranName);

	Task<ConcurrentDictionary<Guid, decimal>> GetAccountsAsync(CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> SaveAsync(TafsilBusiness tafsil, bool updateAccount, short tryCount, CancellationToken token, string tranName);

	Task<List<Guid>> CheckExistAsync(List<Guid> lst, CancellationToken token, string tranName);
}
