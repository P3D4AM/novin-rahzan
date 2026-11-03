using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EntityCache.Business.Advertise;
using PacketParser.Entities;

namespace EntityCache.Core.Repository.Advertise;

public interface IAdvCategoryRepository : IRepository<AdvCategoryBusiness>, INoGuidRepository<AdvCategoryBusiness>, IDisposable
{
	Task<IEnumerable<AdvCategoryBusiness>> GetAllAsync(Guid parentGuid, AdvertiseType type, bool? status, string tranName);
}
