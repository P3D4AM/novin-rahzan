using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Global.Tasks;
using PacketParser.BusinessLayer;

namespace EntityCache.Core.Repository.Global.Tasks;

public interface ISarNakhNumbersRepository : INoGuidRepository<SarNakhNumbersBusiness>, IDisposable
{
	Task<ReturnedSaveFuncInfo> RemoveRangeAsync(Guid masterGuid, CancellationToken token, string tranName);

	Task GetAllAsync(SarNakhBusiness master, CancellationToken token, string tranName);
}
