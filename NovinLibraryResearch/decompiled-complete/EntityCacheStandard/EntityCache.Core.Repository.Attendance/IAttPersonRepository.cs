using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Attendance;

namespace EntityCache.Core.Repository.Attendance;

public interface IAttPersonRepository : IRepository<AttPersonBusiness>, INoGuidRepository<AttPersonBusiness>, IDisposable
{
	Task<int> CountInDepartmentAsync(Guid departmentGuid, CancellationToken token, string tranName);
}
