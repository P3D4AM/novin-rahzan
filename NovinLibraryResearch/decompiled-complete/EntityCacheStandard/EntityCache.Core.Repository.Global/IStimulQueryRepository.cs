using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Global;
using PacketParser.BusinessLayer;

namespace EntityCache.Core.Repository.Global;

public interface IStimulQueryRepository : IRepository<StimulQueryBusiness>, INoGuidRepository<StimulQueryBusiness>, IDisposable
{
	Task<ReturnedSaveFuncInfoWithValue<int>> GetNextRefrenced(CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> TruncateAsync(CancellationToken token, string tranName);
}
