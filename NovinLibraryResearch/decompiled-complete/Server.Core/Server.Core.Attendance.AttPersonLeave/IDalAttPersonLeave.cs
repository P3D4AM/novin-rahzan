using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using PacketParser.EventArgs;
using PacketParser.Records.Attendance;

namespace Server.Core.Attendance.AttPersonLeave;

public interface IDalAttPersonLeave
{
	Task<List<WebAttPersonLeave>> GetAllAsync(short depId, Guid? personGuid, EnPersonLeaveStatus? status, Guid? adminGuid);

	Task<List<SaveResponse>> SaveAsync(short DepId, long hashId, List<WebAttPersonLeave> lst, CancellationToken token, ENSource source);

	Task<List<short>> GetPartitionsAsync(short depId, long hashId, CancellationToken cancellationToken);

	Task<List<WebAttPersonLeave>> GetAllAsync(short depId, long hashId, int partitionId, CancellationToken cancellationToken, bool loadGardeshHesab);

	Task<WebAttPersonLeave> GetAsync(short depId, Guid guid);

	Task<List<AttPersonLeaveRemindingSmsRecord>> GetAllForSendRemindingSmsAsync(DateTime dateM);

	Task<ReturnedSaveFuncInfo> SaveRemindingSmsSettingAsync(DateTime dateM);

	Task<DateTime?> GetRemindingSmsSettingAsync();
}
