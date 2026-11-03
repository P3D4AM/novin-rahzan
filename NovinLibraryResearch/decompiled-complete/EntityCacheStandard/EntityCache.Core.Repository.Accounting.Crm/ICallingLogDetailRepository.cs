using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Crm.Calling;
using PacketParser.Filters.Crm;

namespace EntityCache.Core.Repository.Accounting.Crm;

public interface ICallingLogDetailRepository : INoGuidRepository<CallingLogDetailBusiness>, IDisposable
{
	Task<CallingLogDetailBusiness> GetAsync(string uuid, CancellationToken token, string tranName);

	Task<IEnumerable<CallingLogDetailBusiness>> GetAllAsync(CallingLogDetailFilter filter, CancellationToken token, string tranName);
}
