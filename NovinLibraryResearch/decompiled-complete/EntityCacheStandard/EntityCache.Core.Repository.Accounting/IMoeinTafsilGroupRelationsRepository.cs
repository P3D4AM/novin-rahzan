using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.BusinessLayer;

namespace EntityCache.Core.Repository.Accounting;

public interface IMoeinTafsilGroupRelationsRepository : INoGuidRepository<MoeinTafsilGroupRelationsBusiness>, IDisposable
{
	Task<IEnumerable<MoeinTafsilGroupRelationsBusiness>> GetAllAsync(Guid? moeinGuid, Guid? tafsilLevelGuid, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> RemoveAsync(Guid? moeinGuid, Guid? tafsilGroupGuid, short tryCount, CancellationToken token, string tranName);
}
