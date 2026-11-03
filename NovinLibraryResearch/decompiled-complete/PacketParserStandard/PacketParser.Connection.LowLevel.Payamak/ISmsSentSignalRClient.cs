using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Payamak.Entities;

namespace PacketParser.Connection.LowLevel.Payamak;

public interface ISmsSentSignalRClient : IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	Task<ReturnedSaveFuncInfoWithValue<BaseSmsSent>> GetAsync(int id);

	Task<ReturnedSaveFuncInfoWithValue<List<BaseSmsSent>>> GetAllAsync();

	Task<Dictionary<short, long>> GetAllCountDeliverStatus(CancellationToken token);

	Task<ReturnedSaveFuncInfoWithValue<List<BaseSmsSent>>> GetAllAsync(string number, DeliveryStatus deliverySt, string message, string senderNumber, DateTime? date1, DateTime? date2);

	Task<ReturnedSaveFuncInfo> SaveAsync(BaseSmsSent item, CancellationToken token);

	Task<ReturnedSaveFuncInfo> DeleteListAsync(List<int> list, CancellationToken token);

	Task<ReturnedSaveFuncInfoWithValue<int>> CountAsync(CancellationToken token);

	Task<ReturnedSaveFuncInfoWithValue<int>> CountSentTodayMessagesAsync(string senderNumber, CancellationToken token);

	Task<ReturnedSaveFuncInfoWithValue<List<long>>> GetRefrensedIdsAsync(int accid, int count);

	Task<ReturnedSaveFuncInfoWithValue<List<BaseSmsSent>>> GetAllByRefrenceGuidAsync(Guid refrenceGuid);
}
