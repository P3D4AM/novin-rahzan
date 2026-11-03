using System;
using EntityCache.Business.Advertise;

namespace EntityCache.Core.Repository.Advertise;

public interface ICategoryGroupRepository : IRepository<CategoryGroupBusiness>, INoGuidRepository<CategoryGroupBusiness>, IDisposable
{
}
