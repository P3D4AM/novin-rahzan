using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Connection.LowLevel;
using PacketParser.Entities.Interfaces;
using PacketParser.Payamak.Entities;

namespace PacketParser.Payamak.Interfaces;

public interface ISmsTemplateSignalRClient : IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	Task<ReturnedSaveFuncInfoWithValue<List<BaseSmsTemplate>>> GetAllAsync(CancellationToken token);

	Task<ReturnedSaveFuncInfoWithValue<BaseSmsTemplate>> GetAsync(int id, CancellationToken token);

	Task<ReturnedSaveFuncInfo> SaveAsync(BaseSmsTemplate item, CancellationToken token);

	Task<ReturnedSaveFuncInfo> DeleteAsync(int id, CancellationToken token);
}
