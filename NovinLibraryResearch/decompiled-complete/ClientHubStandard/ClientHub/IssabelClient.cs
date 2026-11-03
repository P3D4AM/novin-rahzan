using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Connection.LowLevel;
using PacketParser.Entities;
using PacketParser.Entities.CRM.CallCenter.Asterisk;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs.Connection;
using PacketParser.EventArgs.Crm.CallCenter;
using PacketParser.Issabel;
using Serilog;

namespace ClientHub;

public class IssabelClient : RetriableHubConnection, IIssabelSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	private CancellationTokenSource _connectinToken;

	private bool _isDisposed;

	private List<IsabelleCallCenterBase> _asternetList;

	private TaskCompletionSource<List<IsabelleCallCenterBase>> _setting;

	public string ContextGuid { get; private set; }

	public IReadOnlyList<IsabelleCallCenterBase> AsternetList
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public event Func<object, ReturnedSaveFuncInfo, Task> OnSetSettingIssabel;

	public event Func<object, StateChange, Task> StateChanged;

	public event Func<object, QueueAssingedToAgentEventArgs, Task> OnQueueAssingedToAgent;

	public event Func<object, QueueAbandonEventArgs, Task> OnQueueAbandon;

	public event Func<object, List<IsabelleCallCenterBase>, Task> OnAsterNetConfigs;

	public event Func<object, QueueJoinEventArgs, Task> OnQueueJoinToAgent;

	public event Func<object, AsteriskHangupEventArgs, Task> OnAsteriskHangup;

	public event Func<object, AsteriskNewExtenEventArgs, Task> OnAsteriskNewExten;

	public event Func<object, AsteriskNewStateEventArgs, Task> OnAsteriskNewState;

	public event Func<object, AsteriskQueueCallerJoinEventArgs, Task> OnAsteriskQueueCallerJoin;

	public event Func<object, AsteriskQueueCallerLeaveEventArgs, Task> OnAsteriskQueueCallerLeave;

	public event Func<object, AsteriskQueueCallerAbandonEventArgs, Task> OnAsteriskQueueCallerAbandon;

	public event Func<object, ReturnedSaveFuncInfo, Task> OnStatus;

	public event Func<object, ConnectionStateEventArgs, Task> ConnectionStateChanged;

	public event Func<object, AsteriskChangeQueueStatusEventArgs, Task> OnQueueStateChanged;

	public event Func<object, AsteriskAgentConnectEventArgs, Task> OnAgentConnectEvent;

	public IssabelClient(string contextGuid, string endPointUrl, ILogger logger)
		: base(endPointUrl, logger)
	{
		try
		{
			ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\IssabelClient.cs", 51);
		}
	}

	public async Task<ReturnedSaveFuncInfo> SetCallCenterSettings(List<IsabelleCallCenterBase> loginInfo, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await CheckConnectionAsync(), ReturnedState.None, "SetCallCenterSettings", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\IssabelClient.cs", 59);
			if (res.HasError)
			{
				return res;
			}
			returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await HubConnectionExtensions.InvokeAsync<ReturnedSaveFuncInfo>(base.Connection, "SetCallCenterSettings", (object)loginInfo, default(CancellationToken)), ReturnedState.None, "SetCallCenterSettings", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\IssabelClient.cs", 61);
			if (this.OnSetSettingIssabel != null)
			{
				Utilities.RaiseEvent(this.OnSetSettingIssabel, this, res, default(CancellationToken), 60, "SetCallCenterSettings", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\IssabelClient.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetCallCenterSettings", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\IssabelClient.cs", 67);
			res.AddReturnedValue(ex, "", "SetCallCenterSettings", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\IssabelClient.cs", 68);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<List<IsabelleCallCenterBase>>> GetCallCenterSettings(CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfoWithValue<List<IsabelleCallCenterBase>> res = new ReturnedSaveFuncInfoWithValue<List<IsabelleCallCenterBase>>();
		try
		{
			ReturnedSaveFuncInfoWithValue<List<IsabelleCallCenterBase>> returnedSaveFuncInfoWithValue = res;
			returnedSaveFuncInfoWithValue.AddReturnedValue(await CheckConnectionAsync(), ReturnedState.None, "GetCallCenterSettings", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\IssabelClient.cs", 77);
			if (res.HasError)
			{
				return res;
			}
			if ((int)base.Connection.State == 0)
			{
				await base.Connection.StopAsync(default(CancellationToken));
				return res.AddError("ارتباط با سرویس برقرار نمیباشد.", "GetCallCenterSettings", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\IssabelClient.cs", 82);
			}
			await HubConnectionExtensions.SendAsync(base.Connection, "GetCallCenterSettings", default(CancellationToken));
			_setting = new TaskCompletionSource<List<IsabelleCallCenterBase>>();
			returnedSaveFuncInfoWithValue = res;
			returnedSaveFuncInfoWithValue.value = await _setting.Task;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetCallCenterSettings", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\IssabelClient.cs", 91);
			res.AddReturnedValue(ex, "", "GetCallCenterSettings", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\IssabelClient.cs", 92);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> MakeInternalCall(MakeInternalCallEventArgs callInfo1, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await CheckConnectionAsync(), ReturnedState.None, "MakeInternalCall", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\IssabelClient.cs", 101);
			if (res.HasError)
			{
				return res;
			}
			await HubConnectionExtensions.SendAsync(base.Connection, "MakeInternalCall", (object)callInfo1, token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MakeInternalCall", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\IssabelClient.cs", 107);
			res.AddReturnedValue(ex, "", "MakeInternalCall", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\IssabelClient.cs", 108);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> MakeOutGoinglCall(MakeOutGoingCallEventArgs callInfo, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await CheckConnectionAsync(), ReturnedState.None, "MakeOutGoinglCall", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\IssabelClient.cs", 117);
			if (res.HasError)
			{
				return res;
			}
			await HubConnectionExtensions.SendAsync(base.Connection, "MakeOutGoinglCall", (object)callInfo, token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MakeOutGoinglCall", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\IssabelClient.cs", 123);
			res.AddReturnedValue(ex, "", "MakeOutGoinglCall", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\IssabelClient.cs", 124);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> SetSaveResponseAsync(WebCallingLog callingLog)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await CheckConnectionAsync(), ReturnedState.None, "SetSaveResponseAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\IssabelClient.cs", 133);
			if (ret.HasError)
			{
				return ret;
			}
			if (!string.IsNullOrEmpty(callingLog.CallerIdNumber) && !string.IsNullOrEmpty(callingLog.DialedNumber))
			{
				await HubConnectionExtensions.SendAsync(base.Connection, "SetSaveResponseAsync", (object)callingLog, default(CancellationToken));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetSaveResponseAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\IssabelClient.cs", 140);
			ret.AddReturnedValue(ex, "", "SetSaveResponseAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\IssabelClient.cs", 141);
		}
		return ret;
	}

	public override Task InitializeConnectionAsync()
	{
		try
		{
			HubConnectionExtensions.On<QueueAssingedToAgentEventArgs>(base.Connection, "SendQueueAssingedToAgent", (Action<QueueAssingedToAgentEventArgs>)delegate(QueueAssingedToAgentEventArgs e)
			{
				new ReturnedSaveFuncInfo();
				try
				{
					if (this.OnQueueAssingedToAgent != null)
					{
						Utilities.RaiseEvent(this.OnQueueAssingedToAgent, this, e, default(CancellationToken), 60, "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\IssabelClient.cs");
					}
				}
				catch (Exception ex13)
				{
					WebErrorLog.Instance.StartLog(ex13, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\IssabelClient.cs", 157);
				}
			});
			HubConnectionExtensions.On<QueueAbandonEventArgs>(base.Connection, "SendOnQueueAbandonToAgent", (Action<QueueAbandonEventArgs>)delegate(QueueAbandonEventArgs e)
			{
				new ReturnedSaveFuncInfo();
				try
				{
					if (this.OnQueueAbandon != null)
					{
						Utilities.RaiseEvent(this.OnQueueAbandon, this, e, default(CancellationToken), 60, "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\IssabelClient.cs");
					}
				}
				catch (Exception ex12)
				{
					WebErrorLog.Instance.StartLog(ex12, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\IssabelClient.cs", 167);
				}
			});
			HubConnectionExtensions.On<List<IsabelleCallCenterBase>>(base.Connection, "OnGetSettings", (Action<List<IsabelleCallCenterBase>>)delegate(List<IsabelleCallCenterBase> e)
			{
				new ReturnedSaveFuncInfo();
				try
				{
					_setting?.TrySetResult(e);
					if (this.OnAsterNetConfigs != null)
					{
						Utilities.RaiseEvent(this.OnAsterNetConfigs, this, e, default(CancellationToken), 60, "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\IssabelClient.cs");
					}
					_asternetList = e;
				}
				catch (Exception ex11)
				{
					WebErrorLog.Instance.StartLog(ex11, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\IssabelClient.cs", 179);
				}
			});
			HubConnectionExtensions.On<QueueJoinEventArgs>(base.Connection, "SendOnQueueJoinToAgent", (Action<QueueJoinEventArgs>)delegate(QueueJoinEventArgs e)
			{
				new ReturnedSaveFuncInfo();
				try
				{
					if (this.OnQueueJoinToAgent != null)
					{
						Utilities.RaiseEvent(this.OnQueueJoinToAgent, this, e, default(CancellationToken), 60, "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\IssabelClient.cs");
					}
				}
				catch (Exception ex10)
				{
					WebErrorLog.Instance.StartLog(ex10, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\IssabelClient.cs", 189);
				}
			});
			HubConnectionExtensions.On<AsteriskHangupEventArgs>(base.Connection, "OnIssableEvent_HangUp", (Action<AsteriskHangupEventArgs>)delegate(AsteriskHangupEventArgs e)
			{
				try
				{
					if (this.OnAsteriskHangup != null)
					{
						Utilities.RaiseEvent(this.OnAsteriskHangup, this, e, default(CancellationToken), 60, "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\IssabelClient.cs");
					}
				}
				catch (Exception ex9)
				{
					WebErrorLog.Instance.StartLog(ex9, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\IssabelClient.cs", 198);
				}
			});
			HubConnectionExtensions.On<AsteriskNewExtenEventArgs>(base.Connection, "OnIssableEvent_NewExten", (Action<AsteriskNewExtenEventArgs>)delegate(AsteriskNewExtenEventArgs e)
			{
				try
				{
					if (this.OnAsteriskNewExten != null)
					{
						Utilities.RaiseEvent(this.OnAsteriskNewExten, this, e, default(CancellationToken), 60, "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\IssabelClient.cs");
					}
				}
				catch (Exception ex8)
				{
					WebErrorLog.Instance.StartLog(ex8, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\IssabelClient.cs", 207);
				}
			});
			HubConnectionExtensions.On<AsteriskQueueCallerAbandonEventArgs>(base.Connection, "OnIssableEvent_QueueCallerAbandon", (Action<AsteriskQueueCallerAbandonEventArgs>)delegate(AsteriskQueueCallerAbandonEventArgs e)
			{
				try
				{
					if (this.OnAsteriskQueueCallerAbandon != null)
					{
						Utilities.RaiseEvent(this.OnAsteriskQueueCallerAbandon, this, e, default(CancellationToken), 60, "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\IssabelClient.cs");
					}
				}
				catch (Exception ex7)
				{
					WebErrorLog.Instance.StartLog(ex7, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\IssabelClient.cs", 216);
				}
			});
			HubConnectionExtensions.On<AsteriskQueueCallerLeaveEventArgs>(base.Connection, "OnIssableEvent_QueueCallerLeave", (Action<AsteriskQueueCallerLeaveEventArgs>)delegate(AsteriskQueueCallerLeaveEventArgs e)
			{
				try
				{
					if (this.OnAsteriskQueueCallerLeave != null)
					{
						Utilities.RaiseEvent(this.OnAsteriskQueueCallerLeave, this, e, default(CancellationToken), 60, "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\IssabelClient.cs");
					}
				}
				catch (Exception ex6)
				{
					WebErrorLog.Instance.StartLog(ex6, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\IssabelClient.cs", 225);
				}
			});
			HubConnectionExtensions.On<AsteriskQueueCallerJoinEventArgs>(base.Connection, "OnIssableEvent_QueueCallerJoin", (Action<AsteriskQueueCallerJoinEventArgs>)delegate(AsteriskQueueCallerJoinEventArgs e)
			{
				try
				{
					if (this.OnAsteriskQueueCallerJoin != null)
					{
						Utilities.RaiseEvent(this.OnAsteriskQueueCallerJoin, this, e, default(CancellationToken), 60, "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\IssabelClient.cs");
					}
				}
				catch (Exception ex5)
				{
					WebErrorLog.Instance.StartLog(ex5, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\IssabelClient.cs", 234);
				}
			});
			HubConnectionExtensions.On<Guid, CancellationToken, string>(base.Connection, "OnGetTafsil", (Func<Guid, CancellationToken, string, Task>)async delegate
			{
			});
			HubConnectionExtensions.On<object, ConnectionStateEventArgs>(base.Connection, "OnStatus", (Action<object, ConnectionStateEventArgs>)delegate(object sender, ConnectionStateEventArgs e)
			{
				try
				{
					if (this.ConnectionStateChanged != null)
					{
						Utilities.RaiseEvent(this.ConnectionStateChanged, this, e, default(CancellationToken), 60, "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\IssabelClient.cs");
					}
				}
				catch (Exception ex4)
				{
					WebErrorLog.Instance.StartLog(ex4, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\IssabelClient.cs", 250);
				}
			});
			HubConnectionExtensions.On<AsteriskNewStateEventArgs>(base.Connection, "OnIssableEvent_NewState", (Action<AsteriskNewStateEventArgs>)delegate(AsteriskNewStateEventArgs e)
			{
				try
				{
					if (this.OnAsteriskNewState != null)
					{
						Utilities.RaiseEvent(this.OnAsteriskNewState, this, e, default(CancellationToken), 60, "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\IssabelClient.cs");
					}
				}
				catch (Exception ex3)
				{
					WebErrorLog.Instance.StartLog(ex3, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\IssabelClient.cs", 259);
				}
			});
			HubConnectionExtensions.On<AsteriskAgentConnectEventArgs>(base.Connection, "OnAgentConnectEvent", (Action<AsteriskAgentConnectEventArgs>)delegate(AsteriskAgentConnectEventArgs e)
			{
				try
				{
					if (this.OnAgentConnectEvent != null)
					{
						Utilities.RaiseEvent(this.OnAgentConnectEvent, this, e, default(CancellationToken), 60, "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\IssabelClient.cs");
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\IssabelClient.cs", 268);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\IssabelClient.cs", 271);
		}
		return Task.CompletedTask;
	}

	public override async ValueTask DisposeAsync()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				this.StateChanged = null;
				this.OnQueueAssingedToAgent = null;
				this.OnQueueAbandon = null;
				this.OnAsterNetConfigs = null;
				this.OnQueueJoinToAgent = null;
				this.OnAsteriskHangup = null;
				this.OnAsteriskNewExten = null;
				this.OnAsteriskNewState = null;
				this.OnAsteriskQueueCallerJoin = null;
				this.OnAsteriskQueueCallerLeave = null;
				this.OnAsteriskQueueCallerAbandon = null;
				this.OnAgentConnectEvent = null;
				this.ConnectionStateChanged = null;
				await base.DisposeAsync();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DisposeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\IssabelClient.cs", 297);
		}
	}
}
