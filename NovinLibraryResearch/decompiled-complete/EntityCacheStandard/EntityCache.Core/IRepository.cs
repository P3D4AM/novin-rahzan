using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Core;

public interface IRepository<T> : INoGuidRepository<T>, IDisposable where T : class, IHasJustGuid, IHasContext
{
	Task<ReturnedSaveFuncInfo> KeepRangeAsync(IEnumerable<Guid> items, short tryCount, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> DeleteRangeAsync(IEnumerable<Guid> items, short tryCount, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> RemoveAsync(Guid guid, short tryCount, CancellationToken token, string tranName);

	Task<T> GetAsync(Guid guid, CancellationToken token, string tranName);

	T Get(Guid guid, CancellationToken token, string tranName);

	Task<T> GetFromDatabaseAsync(Guid guid, CancellationToken token, string tranName);

	Task<IEnumerable<T>> GetAllAsync(bool? status, CancellationToken token, string tranName);
}
