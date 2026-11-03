using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Note;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;

namespace EntityCache.Core.Repository.Accounting.Note;

public interface INoteRepository : ISyncRepository<NoteBusiness>, IEditableDelivery, IRepository<NoteBusiness>, INoGuidRepository<NoteBusiness>, IDisposable
{
	Task<int> CountNumbersInGroupAsync(Guid groupGuid, CancellationToken token, string tranName);

	Task<IEnumerable<NoteBusiness>> GetAllAsync(Guid groupGuid, NoteStatus? status, CancellationToken token, string tranName);

	Task<List<NoteBusiness>> GetAllAsync(CancellationToken token, NoteFilter filter, string tranName);
}
