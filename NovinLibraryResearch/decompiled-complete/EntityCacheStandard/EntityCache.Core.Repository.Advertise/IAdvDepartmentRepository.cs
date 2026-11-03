using System;
using System.Threading.Tasks;
using EntityCache.Business.Advertise;

namespace EntityCache.Core.Repository.Advertise;

public interface IAdvDepartmentRepository : IRepository<AdvDepartmentBusiness>, INoGuidRepository<AdvDepartmentBusiness>, IDisposable
{
	Task<bool> CheckNameAsync(string name, Guid guid, string tranName);

	Task<AdvDepartmentBusiness> GetAsync(string depName, string tranName);
}
