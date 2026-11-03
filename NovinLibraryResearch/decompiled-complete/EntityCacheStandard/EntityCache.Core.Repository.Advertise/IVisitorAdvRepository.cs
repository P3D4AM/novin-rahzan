using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EntityCache.Business.Advertise;
using PacketParser.BusinessLayer;

namespace EntityCache.Core.Repository.Advertise;

public interface IVisitorAdvRepository : IRepository<VisitorAdvBusiness>, INoGuidRepository<VisitorAdvBusiness>, IDisposable
{
	Task<ReturnedSaveFuncInfo> RemoveByVisitorGuidAsync(Guid visitorGuid, short tryCount, string tranName);

	Task<IEnumerable<VisitorAdvBusiness>> GetAllAsync(Guid visitorGuid, string tranName);
}
