using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EntityCache.Business.Advertise;

namespace EntityCache.Core.Repository.Advertise;

public interface IAdvGroupRepository : IRepository<AdvGroupBusiness>, INoGuidRepository<AdvGroupBusiness>, IDisposable
{
	Task<bool> CheckNameAsync(string name, Guid guid, string tranName);

	Task<AdvGroupBusiness> GetAsyncByParent(Guid parentGuid, string tranName);

	Task<List<AdvGroupBusiness>> GetAllAsync(Guid parentGuid, string tranName);

	Task<AdvGroupBusiness> GetAsync(string name, string tranName);
}
