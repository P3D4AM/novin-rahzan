using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Khazane.Pardakht;

namespace EntityCache.Core.Repository.Accounting.Khazane.Pardakht;

public interface IPardakhtChequeEntezamiRepository : INoGuidRepository<PardakhtChequeEntezamiBusiness>, IDisposable
{
	Task<PardakhtChequeEntezamiBusiness> GetAsync(Guid guid, CancellationToken token, string tranName);
}
