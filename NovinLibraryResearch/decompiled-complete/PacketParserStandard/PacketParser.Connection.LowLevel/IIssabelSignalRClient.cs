using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.CRM.CallCenter.Asterisk;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs.Connection;
using PacketParser.EventArgs.Crm.CallCenter;
using PacketParser.Issabel;

namespace PacketParser.Connection.LowLevel;

public interface IIssabelSignalRClient : INpAsyncDisposable, IAsyncDisposable
{
	IReadOnlyList<IsabelleCallCenterBase> AsternetList { get; }

	event Func<object, ReturnedSaveFuncInfo, Task> OnSetSettingIssabel;

	event Func<object, StateChange, Task> StateChanged;

	event Func<object, QueueAssingedToAgentEventArgs, Task> OnQueueAssingedToAgent;

	event Func<object, QueueAbandonEventArgs, Task> OnQueueAbandon;

	event Func<object, QueueJoinEventArgs, Task> OnQueueJoinToAgent;

	event Func<object, List<IsabelleCallCenterBase>, Task> OnAsterNetConfigs;

	event Func<object, AsteriskHangupEventArgs, Task> OnAsteriskHangup;

	event Func<object, AsteriskNewExtenEventArgs, Task> OnAsteriskNewExten;

	event Func<object, AsteriskQueueCallerJoinEventArgs, Task> OnAsteriskQueueCallerJoin;

	event Func<object, AsteriskQueueCallerLeaveEventArgs, Task> OnAsteriskQueueCallerLeave;

	event Func<object, AsteriskQueueCallerAbandonEventArgs, Task> OnAsteriskQueueCallerAbandon;

	event Func<object, ConnectionStateEventArgs, Task> ConnectionStateChanged;

	event Func<object, AsteriskNewStateEventArgs, Task> OnAsteriskNewState;

	event Func<object, AsteriskAgentConnectEventArgs, Task> OnAgentConnectEvent;

	Task<ReturnedSaveFuncInfoWithValue<List<IsabelleCallCenterBase>>> GetCallCenterSettings(CancellationToken token = default(CancellationToken));

	Task<ReturnedSaveFuncInfo> SetCallCenterSettings(List<IsabelleCallCenterBase> loginInfo, CancellationToken token = default(CancellationToken));

	Task<ReturnedSaveFuncInfo> MakeInternalCall(MakeInternalCallEventArgs callInfo, CancellationToken token = default(CancellationToken));

	Task<ReturnedSaveFuncInfo> MakeOutGoinglCall(MakeOutGoingCallEventArgs callInfo, CancellationToken token = default(CancellationToken));

	Task<ReturnedSaveFuncInfo> SetSaveResponseAsync(WebCallingLog callingLog);
}
