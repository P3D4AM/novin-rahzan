using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Selection;
using PacketParser.BusinessLayer;

namespace EntityCache.Core.Repository.Accounting.Selection;

public interface IProductGroupRelationRepository : INoGuidRepository<ProductGroupRelationBusiness>, IDisposable
{
	Task<IEnumerable<ProductGroupRelationBusiness>> GetAllAsync(Guid? productGuid, Guid? groupGuid, CancellationToken token, string tranName);

	Task<IEnumerable<ProductGroupRelationBusiness>> GetAllAsync(List<Guid> groupGuidList, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> RemoveRangeAsync(Guid productGuid, CancellationToken token, string tranName);

	Task<ConcurrentDictionary<Guid, List<Guid>>> GetDictionaryAsync(List<Guid> productGuidList, CancellationToken token, string tranName);
}
