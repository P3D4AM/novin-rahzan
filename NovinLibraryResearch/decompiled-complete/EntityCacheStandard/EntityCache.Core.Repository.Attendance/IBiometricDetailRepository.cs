using System;
using EntityCache.Business.Attendance;

namespace EntityCache.Core.Repository.Attendance;

public interface IBiometricDetailRepository : IPersistenceMasterRepository<BiometricDetailBusiness>, IDisposable
{
}
