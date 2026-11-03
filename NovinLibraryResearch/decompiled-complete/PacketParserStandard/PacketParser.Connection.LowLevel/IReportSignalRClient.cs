using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs.Crm.CallCenter;
using PacketParser.Issabel;

namespace PacketParser.Connection.LowLevel;

public interface IReportSignalRClient : INpAsyncDisposable, IAsyncDisposable
{
	event Func<WebCallingLog, Task> OnNotifToClient;

	Task<ReturnedSaveFuncInfo> LoginAsync();

	Task<ReturnedSaveFuncInfoWithValue<List<IsabelleCallCenterBase>>> GetCallCenterSetting(CancellationToken token = default(CancellationToken));

	Task<ReturnedSaveFuncInfo> MakeInternalCall(MakeInternalCallEventArgs callInfo1, CancellationToken token = default(CancellationToken));

	Task<ReturnedSaveFuncInfo> MakeOutGoinglCall(MakeOutGoingCallEventArgs callInfo, CancellationToken token = default(CancellationToken));
}
