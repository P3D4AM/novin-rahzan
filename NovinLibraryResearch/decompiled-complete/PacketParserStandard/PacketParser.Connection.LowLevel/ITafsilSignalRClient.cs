using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;

namespace PacketParser.Connection.LowLevel;

public interface ITafsilSignalRClient : IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	Task<ReturnedSaveFuncInfoWithValue<WebTafsil>> GetAsync(Guid guid, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfoWithValue<List<WebTafsil>>> GetAllAsync(EnSystem system, TafsilFilter filter, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfoWithValue<long>> NextCodeInGroupAsync(Guid tfGuid, EnSuggestionType suggestionMode, CancellationToken token, string tanName);

	Task<ReturnedSaveFuncInfoWithValue<WebTafsil>> GetByTypeAndCodeAsync(HesabType hesabType, long code, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfoWithValue<WebTafsil>> GetByCodeAsync(long tafsilCode, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> SaveAsync(WebTafsil tafsil, bool updateAccount, short tryCount, CancellationToken token, string tranName);
}
