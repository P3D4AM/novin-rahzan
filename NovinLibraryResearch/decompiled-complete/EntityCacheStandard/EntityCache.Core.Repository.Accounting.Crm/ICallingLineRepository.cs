using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Crm;

namespace EntityCache.Core.Repository.Accounting.Crm;

public interface ICallingLineRepository : IRepository<CallingLineBusiness>, INoGuidRepository<CallingLineBusiness>, IDisposable
{
	Task<CallingLineBusiness> GetAsync(string ComputerName, short DeviceNumber, short LineNumber, CancellationToken token, string tranName);

	Task<CallingLineBusiness> GetAsync(string number, CancellationToken token, string tranName);
}
