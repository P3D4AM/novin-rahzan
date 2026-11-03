using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Attendance;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Core.Repository.Attendance;

public interface IAttDepartmentRepository : ISyncRepository<AttDepartmentBusiness>, IEditableDelivery, IRepository<AttDepartmentBusiness>, INoGuidRepository<AttDepartmentBusiness>, IDisposable
{
	Task<AttDepartmentBusiness> GetAsync(string name, CancellationToken token, string tranName);
}
