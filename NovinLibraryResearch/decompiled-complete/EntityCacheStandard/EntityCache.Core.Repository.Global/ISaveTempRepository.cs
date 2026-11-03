using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Global;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Filters;

namespace EntityCache.Core.Repository.Global;

public interface ISaveTempRepository : IRepository<SaveTempBusiness>, INoGuidRepository<SaveTempBusiness>, IDisposable
{
	Task<IEnumerable<SaveTempViewModel>> GetAllAsync(SaveTempFilter filter, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> RemoveRangeAsync(Guid asGuid, Document asType, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> RemoveRangeByDateAsync(CancellationToken token, string tranName);
}
