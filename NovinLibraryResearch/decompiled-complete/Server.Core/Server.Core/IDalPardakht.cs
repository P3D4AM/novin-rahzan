using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.DepartmentApiModel.RequestModel;
using PacketParser.Entities;

namespace Server.Core;

public interface IDalPardakht
{
	Task<ReturnedSaveFuncInfo> SaveAsync(short depId, long hashId, WebPardakht item, int userId, string ip, ENSource source, short tryCount = 10);

	Task<WebPardakht> LoadDataAsync(short DepId, int Id);

	Task<List<WebPardakht>> GetAllAsync(short DepId, List<ReadStatus> status, bool ChangeReadSt, short Top, DateTime modified, CancellationToken? cancellationToken);

	Task<List<WebPardakht>> GetAllAsync(short DepId, PaymentRequestModelcs filter, CancellationToken? cancellationToken = null);

	Task<ReturnedSaveFuncInfo> FixAsync(ConnectionData connection, WebPardakht item);

	Task<WebPardakht> GetAsync(Guid guid, short depId);
}
