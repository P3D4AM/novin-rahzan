using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EntityCache.Business.Advertise;

namespace EntityCache.Core.Repository.Advertise;

public interface IDepartmentCategoryRepository : IRepository<DepartmentCategoryBusiness>, INoGuidRepository<DepartmentCategoryBusiness>, IDisposable
{
	Task<IEnumerable<DepartmentCategoryBusiness>> GetAllAsync(Guid depGuid, bool? status, string tranName);
}
