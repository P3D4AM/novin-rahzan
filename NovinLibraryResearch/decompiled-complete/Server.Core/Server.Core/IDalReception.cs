using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.DepartmentApiModel.RequestModel;
using PacketParser.Entities;

namespace Server.Core;

public interface IDalReception
{
	Task<ReturnedSaveFuncInfo> SaveAsync(short depId, int depUserId, long hashId, ENSource _source, WebReception item);

	Task<WebReception> LoadDataAsync(short DepId, int Id);

	Task<List<WebReception>> GetAllAsync(short DepId, List<ReadStatus> ST, bool ChangeReadST, DateTime Modified, CancellationToken cancellationToken, int top);

	Task<List<WebReception>> GetAllAsync(short DepId, RecptionRequestModelcs filter, CancellationToken cancellationToken);

	Task FixAsync(ConnectionData connection, WebReception item);

	Task<WebReception> GetAsync(Guid guid, short depId);
}
