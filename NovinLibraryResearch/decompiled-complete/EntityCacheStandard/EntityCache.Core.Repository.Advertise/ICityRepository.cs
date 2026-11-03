using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Advertise;
using PacketParser.Entities;

namespace EntityCache.Core.Repository.Advertise;

public interface ICityRepository : IRepository<CityBusiness>, INoGuidRepository<CityBusiness>, IDisposable
{
	Task<IEnumerable<CityBusiness>> GetAllAsync(Guid stateGuid, bool? status, string tranName);

	Task<IEnumerable<CityBusiness>> GetAllAsync(string search, Guid stateGuid, CancellationToken token, bool? status, string tranName);

	Task<CityBusiness> GetAsync(string Name, string tranName);

	Task UpdateAllCityChecked(AdvertiseType type, short tryCount, string tranName);

	Task<bool> CheckNameAsync(string name, Guid guid, string tranName);
}
