using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Attendance.Business;

namespace EntityCache.Core.Repository;

public interface ICalenderRepository : IRepository<CalenderBusiness>, INoGuidRepository<CalenderBusiness>, IDisposable
{
	Task<IEnumerable<CalenderBusiness>> GetAllAsync(DateTime? d1, DateTime? d2, bool? status, CancellationToken token, string tranName);

	Task<CalenderBusiness> GetAsync(Guid guid, DateTime dateM, CancellationToken token, string tranName);

	Task<CalenderBusiness> GetAsync(DateTime dateM, CancellationToken token, string tranName);
}
