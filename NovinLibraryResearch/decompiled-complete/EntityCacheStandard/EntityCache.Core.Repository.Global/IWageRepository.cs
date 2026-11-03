using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Global;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Filters.Khazane;

namespace EntityCache.Core.Repository.Global;

public interface IWageRepository : IRepository<WageBusiness>, INoGuidRepository<WageBusiness>, IDisposable
{
	Task<IEnumerable<WageBusiness>> GetAllAsync(WageFilter filter, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfoWithValue<List<WageBusiness>>> IsDuplicateCodeAsync(EnKhazaneMaster master, EnWageType typeEnteghal, decimal minPrice, decimal maxPrice, Guid guid, EnWageCalculationMethod calcMethod, CancellationToken token, string tranName);

	Task<WageBusiness> GetByServerGuidAsync(Guid serverGuid, CancellationToken token, string tranName);
}
