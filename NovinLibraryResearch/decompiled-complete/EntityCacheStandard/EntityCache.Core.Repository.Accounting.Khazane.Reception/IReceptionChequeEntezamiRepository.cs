using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Khazane.Reception;

namespace EntityCache.Core.Repository.Accounting.Khazane.Reception;

public interface IReceptionChequeEntezamiRepository : INoGuidRepository<ReceptionChequeEntezamiBusiness>, IDisposable
{
	Task<ReceptionChequeEntezamiBusiness> GetAsync(Guid guid, CancellationToken token, string tranName);
}
