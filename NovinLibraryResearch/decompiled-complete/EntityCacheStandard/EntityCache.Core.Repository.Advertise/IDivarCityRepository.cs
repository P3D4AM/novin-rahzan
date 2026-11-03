using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Advertise;

namespace EntityCache.Core.Repository.Advertise;

public interface IDivarCityRepository : IRepository<DivarCityBusiness>, INoGuidRepository<DivarCityBusiness>, IDisposable
{
	Task<List<DivarCityBusiness>> GetAllAsync(string search, CancellationToken token, bool? status, string tranName);
}
