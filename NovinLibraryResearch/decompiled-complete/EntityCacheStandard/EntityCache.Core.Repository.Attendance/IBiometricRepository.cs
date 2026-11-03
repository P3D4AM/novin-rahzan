using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Attendance;
using PacketParser.Entities;

namespace EntityCache.Core.Repository.Attendance;

public interface IBiometricRepository : IRepository<BiometricBusiness>, INoGuidRepository<BiometricBusiness>, IDisposable
{
	Task<BiometricBusiness> GetAsync(Guid personGuid, EnDeviceGroup deviceGroup, CancellationToken token, string tranName);
}
