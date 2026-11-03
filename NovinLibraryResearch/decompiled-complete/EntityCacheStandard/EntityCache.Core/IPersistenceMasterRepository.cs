using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Core;

public interface IPersistenceMasterRepository<T> : IDisposable where T : class, IHasContext
{
	Task<ReturnedSaveFuncInfo> SaveAsync(IEnumerable<T> items, Guid MasterGuid, short tryCount, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> RemoveAsync(Guid MasterGuid, short tryCount, CancellationToken token, string tranName);

	Task<List<T>> GetAllAsync(Guid MasterGuid, CancellationToken token, string tranName);
}
