using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Core.Repository.Accounting;

public interface ITafsilGroupRepository : ISyncRepository<TafsilGroupBusiness>, IEditableDelivery, IRepository<TafsilGroupBusiness>, INoGuidRepository<TafsilGroupBusiness>, IDisposable
{
	Task<List<TafsilGroupBusiness>> GetAllAsync(string search, CancellationToken token, bool? status, string tranName);

	Task<TafsilGroupBusiness> GetAsync(string name, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfoWithValue<List<TafsilGroupBusiness>>> IsDuplicateCodeAsync(long codeMin, long codeMax, Guid guid, CancellationToken token, string tranName);

	Task<long> NextMinCodeAsync(CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<WebTafsilGroup> items, CancellationToken token, string tranName);
}
