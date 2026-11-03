using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.BusinessLayer;

namespace EntityCache.Core.Repository;

public interface IRasGiriDetRepository : IRepository<RasGiriDetBusiness>, INoGuidRepository<RasGiriDetBusiness>, IDisposable
{
	Task<IEnumerable<RasGiriDetBusiness>> GetAllByMasterGuidAsync(Guid masterGuid, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> RemoveAllByMasterGuidAsync(Guid masterGuid, short tryCount, CancellationToken token, string tranName);
}
