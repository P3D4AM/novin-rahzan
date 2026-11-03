using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;

namespace EntityCache.Core;

public interface IEntityRepository<T, K> : IEditableDelivery, IDisposable where T : class, IEntityCache, IHasContext
{
	WebContextHolder Context { get; }

	Task<int> DbCountAsync(bool? status, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> RemoveAsync(Guid item, short tryCount, CancellationToken token, string tranName);

	Task<T> GetAsync(Guid guid, CancellationToken token, string tranName);

	Task<K> GetReportAsync(Guid guid, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> SaveAsync(T item, short tryCount, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token, short tryCount, string tranName);

	Task<List<T>> GetAllNotSentAsync(CancellationToken token, string tranName);

	Task Add2CacheAsync(T item, CancellationToken token, string tranName);

	Task ClearCachedItemAsync(Guid guid, CancellationToken token, string tranName);

	Task ClearCachedAsync(CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> SetSanadNoAsync(Guid guid, int sanadNo, CancellationToken token, string tranName);
}
