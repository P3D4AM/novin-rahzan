using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EntityCache.Business.Advertise;
using PacketParser.BusinessLayer;

namespace EntityCache.Core.Repository.Advertise;

public interface IVisitorCityRepository : IRepository<VisitorCityBusiness>, INoGuidRepository<VisitorCityBusiness>, IDisposable
{
	Task<IEnumerable<VisitorCityBusiness>> GetAllAsync(Guid visitorGuid, string tranName);

	Task<ReturnedSaveFuncInfo> RemoveByVisitorGuidAsync(Guid visitorGuid, short tryCount, string tranName);

	Task<IEnumerable<VisitorCityBusiness>> GetAllCityAsync(Guid cityGuid, string tranName);
}
