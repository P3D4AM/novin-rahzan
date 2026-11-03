using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace EntityCache.Core.Repository.Accounting;

public interface ITableRelationShipRepository : IRepository<TableRelationShipBusiness>, INoGuidRepository<TableRelationShipBusiness>, IDisposable
{
	Task<IEnumerable<TableRelationShipBusiness>> GetAllRelatedSanadAsync(Guid sanadGuid, Document sanadType, CancellationToken token, string tranName);

	Task<IEnumerable<TableRelationShipBusiness>> GetAllInRelationSanadAsync(Guid sanadGuid, Document sanadType, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> RemoveRangeAsync(Guid sanadGuid, Document sanadType, CancellationToken token, string tranName);
}
