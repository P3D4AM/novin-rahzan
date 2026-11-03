using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EntityCache.Business.Advertise;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace EntityCache.Core.Repository.Advertise;

public interface IRegionRepository : IRepository<RegionBusiness>, INoGuidRepository<RegionBusiness>, IDisposable
{
	Task<IEnumerable<RegionBusiness>> GetAllAsync(Guid cityGuid, AdvertiseType type, bool? status, string tranName);

	Task<ReturnedSaveFuncInfo> RemoveByTypeAsync(AdvertiseType type, short tryCount, string tranName);
}
