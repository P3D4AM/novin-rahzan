using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Payamak.Entities;

namespace PacketParser.Connection.LowLevel.Payamak;

public interface ISmsOutBoxSignalRClient : IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	Task<ReturnedSaveFuncInfoWithValue<BaseSmsOutBox>> NextMessageGsmAsync(string senderNumber, EnSmsSenderType _type);

	Task<ReturnedSaveFuncInfoWithValue<List<int>>> SaveAsync(BaseSmsOutBox item, CancellationToken token = default(CancellationToken));

	Task<ReturnedSaveFuncInfo> SaveRangeAsync(List<BaseSmsOutBox> items, CancellationToken token = default(CancellationToken));

	Task<ReturnedSaveFuncInfoWithValue<int>> CountAsync(CancellationToken token);

	Task<ReturnedSaveFuncInfo> DeleteListAsync(List<int> list, CancellationToken token);

	Task<ReturnedSaveFuncInfo> UpdateDeliveryStatusAsync(int deliverySt, long refrencedId, CancellationToken token);

	Task<ReturnedSaveFuncInfoWithValue<List<BaseSmsOutBox>>> GetAllAsync(CancellationToken token, DateTime date1, DateTime date2, bool st);

	Task<ReturnedSaveFuncInfoWithValue<List<string>>> GetAllScadulingDateShAsync();

	Task<ReturnedSaveFuncInfoWithValue<int>> SelectAccountIdAsync(CancellationToken token);
}
