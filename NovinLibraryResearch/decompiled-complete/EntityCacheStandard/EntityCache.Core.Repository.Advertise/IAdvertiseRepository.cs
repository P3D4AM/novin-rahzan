using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EntityCache.Business.Advertise;

namespace EntityCache.Core.Repository.Advertise;

public interface IAdvertiseRepository : IRepository<AdvertiseBusiness>, INoGuidRepository<AdvertiseBusiness>, IDisposable
{
	Task<IEnumerable<AdvertiseBusiness>> GetAllAsync(Guid groupGuid, bool? status, string tranName);
}
