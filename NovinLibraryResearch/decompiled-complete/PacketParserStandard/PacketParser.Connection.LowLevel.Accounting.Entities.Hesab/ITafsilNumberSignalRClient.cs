using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Connection.LowLevel.Accounting.Entities.Hesab;

public interface ITafsilNumberSignalRClient : IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	Task<ReturnedSaveFuncInfoWithValue<IEnumerable<WebTafsilNumbers>>> GetInfoAsync(long number, bool? status, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfoWithValue<List<WebTafsilNumbers>>> GetAllAsync(Guid tafsilGuid, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfoWithValue<List<string>>> GetTitleListAsync(CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> RemoveAsync(Guid tafsilGuid, short tryCount, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfoWithValue<List<WebTafsilNumberReport>>> GetAllReportAsync(CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfoWithValue<List<WebTafsilNumbers>>> GetInfoByCalcNumberAsync(long number, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<WebTafsilNumbers> items, short tryCount, CancellationToken token, string tranName);
}
