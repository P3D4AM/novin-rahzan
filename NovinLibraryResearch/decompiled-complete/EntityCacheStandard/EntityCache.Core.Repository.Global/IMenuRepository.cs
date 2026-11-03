using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Global;
using PacketParser.BusinessLayer;

namespace EntityCache.Core.Repository.Global;

public interface IMenuRepository : IRepository<MenuBusiness>, INoGuidRepository<MenuBusiness>, IDisposable
{
	Task<ReturnedSaveFuncInfo> UpdateAsync(Guid masterGuid, List<Guid> guids, CancellationToken token, string tranname);
}
