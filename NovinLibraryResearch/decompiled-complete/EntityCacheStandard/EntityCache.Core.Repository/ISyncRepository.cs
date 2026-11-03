using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;

namespace EntityCache.Core.Repository;

public interface ISyncRepository<T> : IEditableDelivery, IRepository<T>, INoGuidRepository<T>, IDisposable where T : class, IHasJustGuid, IHasContext
{
	Task<List<Guid>> GetAllNotSentAsync(CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> SetSaveResult(SaveResponse item, CancellationToken token, short tryCount, string tranName);

	Task<ReturnedSaveFuncInfo> UpdateServerStatusAsync(Guid guid, ServerStatusForClient status, CancellationToken token, string tranName);
}
