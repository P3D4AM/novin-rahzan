using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.DepartmentApiModel.RequestModel;
using PacketParser.Entities;

namespace Server.Core;

public interface IDalOrder
{
	Task<ReturnedSaveFuncInfo> SaveAsync(WebContextHolder context, ConnectionData connection, WebOrder Item, bool DeleteTempOrder);

	Task<ReturnedSaveFuncInfo> SaveTempAsync(ConnectionData connection, Guid CusGuid, WebOrder Item);

	Task<WebOrder> LoadDataAsync(short depId, int Id);

	Task FixAsync(short depId, WebOrder item, int hId = 0, short HType = 0);

	Task<List<WebOrder>> GetNotDeliveredAsync(short depId, ReadStatus status, bool changeReadST, short top, CancellationToken token);

	Task<WebOrder> GetTempAsync(short depId, Guid orderGuid);

	Task<WebOrder> LoadDataAsync(short depId, Guid guid);

	Task<WebOrder> LoadDataAsync(short depId, Document orderType, string WpRefrencedId);

	Task<WebOrder> GetAsync(short DepID, Guid OrderGuid);

	Task<List<WebOrder>> GetAllAsync(short depId, List<ReadStatus> status, bool changeReadST, DateTime modified, CancellationToken token, int top);

	Task<List<WebOrder>> GetAllAsync(short depId, OrderRequestModelcs filter, CancellationToken token);

	Task<List<WebOrder>> GetAllByTafsilGuidAsync(short depId, Guid tafsilGuid);
}
