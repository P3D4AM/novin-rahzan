using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;

namespace EntityCache.Core;

public interface IDetailRepository<T> : IEditableDelivery, IDisposable where T : class, IHasGuid
{
	WebContextHolder Context { get; }

	Task<int> DbCountAsync(bool? status, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> RemoveAsync(Guid item, short tryCount, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> KeepRangeAsync(IEnumerable<Guid> items, short tryCount, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> DeleteRangeAsync(IEnumerable<Guid> items, short tryCount, CancellationToken token, string tranName);

	Task<T> GetAsync(Guid guid, CancellationToken token, string tranName);

	T Get(Guid guid, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> SaveAsync(T item, short tryCount, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<T> items, short tryCount, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token, short tryCount, string tranName);

	Task<List<Guid>> GetAllNotSentAsync(CancellationToken token, string tranName);
}
