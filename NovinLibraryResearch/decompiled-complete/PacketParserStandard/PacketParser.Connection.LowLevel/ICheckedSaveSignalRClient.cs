using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Connection.LowLevel;

public interface ICheckedSaveSignalRClient : IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	Task<ReturnedSaveFuncInfoWithValue<List<WebCheckedSave>>> GetAllAsync(checkedTablReportType reportType, Guid hesabGuid, bool? status, CancellationToken token, string tranName);
}
