using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities.Interfaces;
using PacketParser.Payamak.Entities;

namespace PacketParser.Connection.LowLevel.Payamak;

public interface ISmsNetAccountSignalRClient : IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	Task<ReturnedSaveFuncInfoWithValue<BaseSmsNetAccount>> GetByIdAsync(int id);

	Task<ReturnedSaveFuncInfoWithValue<BaseSmsNetAccount>> GetByNumberAsync(string senderNumber);

	Task<ReturnedSaveFuncInfoWithValue<List<BaseSmsNetAccount>>> GetAllAsync(CancellationToken token);

	Task<ReturnedSaveFuncInfo> SaveAsync(BaseSmsNetAccount item, CancellationToken token);

	Task<ReturnedSaveFuncInfo> DeleteAsync(int id, CancellationToken token);

	Task<ReturnedSaveFuncInfoWithValue<decimal>> CheckAccountCreditAsync(int accountId, CancellationToken token);
}
