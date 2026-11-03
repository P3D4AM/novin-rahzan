using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Connection.LowLevel.Accounting.Entities.NoteGroup;

public interface INoteGroupSignalRClient : IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	Task<ReturnedSaveFuncInfoWithValue<List<WebNoteGroup>>> GetAllAsync(CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfoWithValue<List<WebNoteGroup>>> GetAllAsync(Guid parentGuid, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfoWithValue<WebNoteGroup>> CheckNameAsync(Guid guid, string name, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfoWithValue<WebNoteGroup>> GetAsync(Guid guid, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfoWithValue<int>> CountGroupChildsAsync(Guid groupGuid, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> SaveAsync(WebNoteGroup item, short tryCount, CancellationToken token, string tranName);
}
