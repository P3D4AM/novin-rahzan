using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Global.Tasks;
using EntityCache.Business.Global.Tasks.SarNakh;
using PacketParser.Filters.Global;

namespace EntityCache.Core.Repository.Global.Tasks;

public interface ISarNakhRepository : IRepository<SarNakhBusiness>, INoGuidRepository<SarNakhBusiness>, IDisposable
{
	Task<IEnumerable<SarNakhReportBusiness>> GetAllAsync(SarNakhFilter filter, CancellationToken token, string tranName);

	Task<List<string>> GetStateListAsync(CancellationToken token, string tranName);

	Task<List<string>> GetCityListAsync(CancellationToken token, string tranName);
}
