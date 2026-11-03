using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Core.Repository;

public interface ICustomerGroupRepository : ISyncRepository<CusGroupBusiness>, IEditableDelivery, IRepository<CusGroupBusiness>, INoGuidRepository<CusGroupBusiness>, IDisposable
{
	Task<CusGroupBusiness> GetAsync(string name, CancellationToken token, string tranName);

	Task<int> CountInGroupActiveAsync(Guid selectedGroupGuid, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<WebCusGroup> items, CancellationToken token, string tranName);
}
