using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.CRM;
using PacketParser.LogPacket;

namespace Server.Core.Log;

public interface IDalPacketLogSum
{
	Task<List<WebLogPacketSum>> SelectAllAsync(int hashId, int maxCount);

	Task<IEnumerable<WebLastDataTransferCrm>> GetAllAsync(CancellationToken token, int maxCount = 0);

	Task<ReturnedSaveFuncInfoWithValue<List<WebLastDataTransferCrm>>> GetDataAsync(Guid tafsilGuid, int counts, DateTime PassedDate, CancellationToken cancellationToken);

	Task<ReturnedSaveFuncInfoWithValue<List<WebLastDataTransferCrm>>> Search(List<ENSource> source, DateTime dtBegin, DateTime? dtEnd, bool? OnLineState, CancellationToken token);
}
