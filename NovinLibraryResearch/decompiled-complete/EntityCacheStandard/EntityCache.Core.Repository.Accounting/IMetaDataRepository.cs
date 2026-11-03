using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Filters;

namespace EntityCache.Core.Repository.Accounting;

public interface IMetaDataRepository : INoGuidRepository<MetaDataBusiness>, IDisposable
{
	Task<IEnumerable<MetaDataBusiness>> GetAllAsync(MetaDataFilter filter, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> RemoveRangeAsync(Guid entityGuid, EntityPart part, CancellationToken token, string tranName);

	Task<List<Guid>> GetGuidListForFixAsync(CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> SaveRangeAsync<T>(Guid entityGuid, EntityPart part, List<T> data, CancellationToken token, string tranName);
}
