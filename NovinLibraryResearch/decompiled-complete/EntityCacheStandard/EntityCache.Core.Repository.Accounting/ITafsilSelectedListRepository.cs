using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.BusinessLayer.Models.Customer;

namespace EntityCache.Core.Repository.Accounting;

public interface ITafsilSelectedListRepository : IRepository<TafsilSelectedBusiness>, INoGuidRepository<TafsilSelectedBusiness>, IDisposable
{
	Task<IEnumerable<TafsilSelectedBusiness>> GetAllAsync(TafsilSelectedFilter filter, CancellationToken token, string tranName);
}
