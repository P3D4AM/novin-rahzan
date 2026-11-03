using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Advertise;

namespace EntityCache.Core.Repository.Advertise;

public interface ISheypoorCityRepository : IRepository<SheypoorCityBusiness>, INoGuidRepository<SheypoorCityBusiness>, IDisposable
{
	Task<List<SheypoorCityBusiness>> GetAllAsync(string search, CancellationToken token, bool? status, string tranName);
}
