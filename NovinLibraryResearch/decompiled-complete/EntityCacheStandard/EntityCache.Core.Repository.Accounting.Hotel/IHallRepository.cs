using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Hotel;
using PacketParser.Filters.Hotel;

namespace EntityCache.Core.Repository.Accounting.Hotel;

public interface IHallRepository : IRepository<HallBusiness>, INoGuidRepository<HallBusiness>, IDisposable
{
	Task<HallBusiness> GetAsync(string title, CancellationToken token, string tranName);

	Task<IEnumerable<HallBusiness>> GetAllAsync(HallFilter filter, CancellationToken token, string tranName);
}
