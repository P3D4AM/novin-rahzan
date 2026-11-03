using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;

namespace PacketParser.Connection.LowLevel.Accounting.Entities.Note;

public interface INoteSignalRClient : IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	Task<ReturnedSaveFuncInfoWithValue<List<WebNote>>> GetAllAsync(NoteFilter filter, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfoWithValue<WebNote>> GetAsync(Guid guid, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfoWithValue<int>> CountNumbersInGroupAsync(Guid groupGuid, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> SaveAsync(WebNote item, short tryCount, CancellationToken token, string tranName);
}
