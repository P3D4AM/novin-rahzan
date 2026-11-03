using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Global;
using PacketParser.BusinessLayer;

namespace EntityCache.Core.Repository.Global;

public interface ITempBarcodePrintRepository : IRepository<TempBarcodePrintBusiness>, INoGuidRepository<TempBarcodePrintBusiness>, IDisposable
{
	Task<ReturnedSaveFuncInfo> RemoveRangeAsync(Guid masterGuid, CancellationToken token, string tranName);
}
