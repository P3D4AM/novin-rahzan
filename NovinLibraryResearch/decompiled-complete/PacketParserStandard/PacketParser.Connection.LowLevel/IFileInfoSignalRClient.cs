using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Connection.LowLevel;

public interface IFileInfoSignalRClient : IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	Task<ReturnedSaveFuncInfoWithValue<List<WebFileInfo>>> GetAllAsync(Guid entityGuid, FileManagement_Parts part, bool justFromCache, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> SaveAsync(WebFileInfo item, CancellationToken token, string tranName);
}
