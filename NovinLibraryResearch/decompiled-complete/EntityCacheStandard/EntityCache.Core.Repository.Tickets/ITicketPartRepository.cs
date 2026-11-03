using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Tickets;

namespace EntityCache.Core.Repository.Tickets;

public interface ITicketPartRepository : IRepository<TicketPartBusiness>, INoGuidRepository<TicketPartBusiness>, IDisposable
{
	Task<TicketPartBusiness> GetAsync(string name, CancellationToken token, string tranName);

	Task<IEnumerable<TicketPartBusiness>> GetSubItemsAsync(Guid masterGuid, CancellationToken token, string tranName);

	Task<short> GetNextOrdinalAsync(CancellationToken token, string tranName);

	Task<List<Guid>> GetAllRelatedPartsAsync(Guid targetGuid, CancellationToken token, string tranName);
}
