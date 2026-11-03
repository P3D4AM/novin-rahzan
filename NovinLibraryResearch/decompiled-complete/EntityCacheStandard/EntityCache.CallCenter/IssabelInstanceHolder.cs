using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ClientHub;
using EntityCache.Business.Accounting;
using EntityCache.Business.Crm;
using EntityCache.Business.Crm.Calling;
using EntityCache.Business.Crm.Report;
using EntityCache.Mapping.BusinessMappers.Crm.Calling;
using Microsoft.AspNetCore.SignalR.Client;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Connection;
using PacketParser.Connection.LowLevel;
using PacketParser.Entities;
using PacketParser.Entities.CRM.CallCenter.Asterisk;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;
using PacketParser.EventArgs.Connection;
using PacketParser.EventArgs.Crm.CallCenter;
using PacketParser.Issabel;
using Serilog;

namespace EntityCache.CallCenter;

public class IssabelInstanceHolder : INpAsyncDisposable, IAsyncDisposable
{
	private static SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

	private static Dictionary<string, IssabelInstanceHolder> _dic = new Dictionary<string, IssabelInstanceHolder>();

	private bool _isDisposed;

	public static string ContextGuid { get; private set; }

	public bool IsDisposed => _isDisposed;

	public IIssabelSignalRClient IssabelSignalRClient { get; private set; }

	public IReportSignalRClient ReportSignalRClient { get; private set; }

	public static event Func<object, ConnectionStateEventArgs, Task> ConnectionStateChanged;

	public static event Func<object, ReturnedSaveFuncInfo, Task> OnSetSettingIssabel;

	public static event Func<object, ConnectionEventArgs<HubConnectionState>, Task> OnHubChanged;

	public static event Func<object, ReturnedSaveFuncInfo, Task> OnStatus;

	public static event Func<object, StateChange, Task> StateChanged;

	public static event Func<object, AsteriskNewStateEventArgs, Task> OnAsteriskNewState;

	public static event Func<object, AsteriskAgentConnectEventArgs, Task> OnAgentConnectEvent;

	public static event Func<object, AsteriskHangupEventArgs, Task> OnAsteriskHangup;

	public static event Func<object, AsteriskQueueCallerJoinEventArgs, Task> OnAsteriskQueueCallerJoin;

	public static event Func<object, AsteriskQueueCallerLeaveEventArgs, Task> OnAsteriskQueueCallerLeave;

	public static event Func<object, AsteriskQueueCallerAbandonEventArgs, Task> OnAsteriskQueueCallerAbandon;

	public static event Func<object, List<IsabelleCallCenterBase>, Task> OnAsterNetConfigs;

	public static event Func<object, QueueJoinEventArgs, Task> OnQueueJoinToAgent;

	public static event Func<object, AsteriskNewExtenEventArgs, Task> OnAsteriskNewExten;

	public static event Func<object, AsteriskChangeQueueStatusEventArgs, Task> OnQueueStateChanged;

	public static event Func<object, QueueAssingedToAgentEventArgs, Task> OnQueueAssingedToAgent;

	public static event Func<object, QueueAbandonEventArgs, Task> OnQueueAbandon;

	public static event Func<WebCallingLog, Task> OnNotifToClient;

	public void HandleSignalRHubs(IsabelleClientSettings settings)
	{
		try
		{
			IssabelSignalRClient = new IssabelClient(ContextGuid, $"http://{settings.Ip}:{settings.Port}/IssableHub", Log.Logger);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "HandleSignalRHubs", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\CallCenter\\IssabelInstanceHolder.cs", 68);
		}
	}

	public async Task<ReturnedSaveFuncInfo> HandleReportHubs(IsabelleClientSettings settings, LoginCallCenterToIssabel extention)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			ReportSignalRClient = new ReportClientHub(ContextGuid, $"http://{settings.Ip}:{settings.Port}/ReportHub", null, extention);
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await ReportSignalRClient.LoginAsync(), ReturnedState.None, "HandleReportHubs", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\CallCenter\\IssabelInstanceHolder.cs", 76);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "HandleReportHubs", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\CallCenter\\IssabelInstanceHolder.cs", 78);
		}
		return res;
	}

	public static IssabelInstanceHolder GetInstance(string contextGuid)
	{
		_dic.TryGetValue(contextGuid, out var value);
		return value;
	}

	public static void SetInstance(string contextGuid, IssabelInstanceHolder instance, bool handleClient, bool handleReport)
	{
		try
		{
			ContextGuid = contextGuid;
			_dic.TryGetValue(contextGuid, out var value);
			if (value != null && !value.IsDisposed)
			{
				throw new DuplicateNameException("Context with Guid:" + contextGuid + " is already Exist");
			}
			if (value != null && value.IsDisposed)
			{
				_dic.Remove(contextGuid);
			}
			_dic.Add(contextGuid, instance);
			if (handleClient)
			{
				instance.IssabelSignalRClient.ConnectionStateChanged += Issabel_ConnectionStateChanged;
				instance.IssabelSignalRClient.OnAsteriskNewState += Issabel_OnAsteriskNewState;
				instance.IssabelSignalRClient.OnAgentConnectEvent += Issabel_OnAgentConnectEvent;
				instance.IssabelSignalRClient.OnAsteriskHangup += Issabel_OnAsteriskHangup;
				instance.IssabelSignalRClient.OnAsteriskQueueCallerJoin += Issabel_OnAsteriskQueueCallerJoin;
				instance.IssabelSignalRClient.OnAsteriskQueueCallerLeave += Issabel_OnAsteriskQueueCallerLeave;
				instance.IssabelSignalRClient.OnAsteriskQueueCallerAbandon += Issabel_OnAsteriskQueueCallerAbandon;
				instance.IssabelSignalRClient.OnQueueJoinToAgent += IssabelSignalRClient_OnQueueJoinToAgent;
				instance.IssabelSignalRClient.StateChanged += IssabelSignalRClient_StateChanged;
				instance.IssabelSignalRClient.OnAsterNetConfigs += IssabelSignalRClient_OnAsterNetConfigs;
				instance.IssabelSignalRClient.OnAsteriskNewExten += IssabelSignalRClient_OnAsteriskNewExten;
				instance.IssabelSignalRClient.OnQueueAssingedToAgent += IssabelSignalRClient_OnQueueAssingedToAgent;
				instance.IssabelSignalRClient.OnQueueAbandon += IssabelSignalRClient_OnQueueAbandon;
				instance.IssabelSignalRClient.OnSetSettingIssabel += IssabelSignalRClient_OnSetSettingIssabel;
			}
			if (handleReport)
			{
				instance.ReportSignalRClient.OnNotifToClient += ReportSignalRClient_OnNotifToClient;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetInstance", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\CallCenter\\IssabelInstanceHolder.cs", 118);
		}
	}

	private static Task IssabelSignalRClient_OnSetSettingIssabel(object sender, ReturnedSaveFuncInfo e)
	{
		return IssabelInstanceHolder.OnSetSettingIssabel?.Invoke(sender, e);
	}

	public static Task ReportSignalRClient_OnNotifToClient(WebCallingLog e)
	{
		return IssabelInstanceHolder.OnNotifToClient?.Invoke(e);
	}

	public static Task IssabelSignalRClient_OnQueueAbandon(object sender, QueueAbandonEventArgs e)
	{
		return IssabelInstanceHolder.OnQueueAbandon?.Invoke(sender, e);
	}

	public static Task IssabelSignalRClient_OnQueueAssingedToAgent(object sender, QueueAssingedToAgentEventArgs e)
	{
		return IssabelInstanceHolder.OnQueueAssingedToAgent?.Invoke(sender, e);
	}

	public static Task IssabelSignalRClient_OnAsteriskNewExten(object sender, AsteriskNewExtenEventArgs e)
	{
		return IssabelInstanceHolder.OnAsteriskNewExten?.Invoke(sender, e);
	}

	public static Task IssabelSignalRClient_OnAsterNetConfigs(object sender, List<IsabelleCallCenterBase> e)
	{
		return IssabelInstanceHolder.OnAsterNetConfigs?.Invoke(sender, e);
	}

	public static Task IssabelSignalRClient_StateChanged(object sender, StateChange e)
	{
		return IssabelInstanceHolder.StateChanged?.Invoke(sender, e);
	}

	public static Task IssabelSignalRClient_OnQueueJoinToAgent(object sender, QueueJoinEventArgs e)
	{
		return IssabelInstanceHolder.OnQueueJoinToAgent?.Invoke(sender, e);
	}

	public async ValueTask DisposeAsync()
	{
		_ = 1;
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				if (IssabelSignalRClient != null)
				{
					IssabelSignalRClient.ConnectionStateChanged -= Issabel_ConnectionStateChanged;
					IssabelSignalRClient.OnAsteriskNewState -= Issabel_OnAsteriskNewState;
					IssabelSignalRClient.OnAgentConnectEvent -= Issabel_OnAgentConnectEvent;
					IssabelSignalRClient.OnAsteriskHangup -= Issabel_OnAsteriskHangup;
					IssabelSignalRClient.OnAsteriskQueueCallerJoin -= Issabel_OnAsteriskQueueCallerJoin;
					IssabelSignalRClient.OnAsteriskQueueCallerLeave -= Issabel_OnAsteriskQueueCallerLeave;
					IssabelSignalRClient.OnAsteriskQueueCallerAbandon -= Issabel_OnAsteriskQueueCallerAbandon;
					IssabelSignalRClient.OnQueueJoinToAgent -= IssabelSignalRClient_OnQueueJoinToAgent;
					IssabelSignalRClient.StateChanged -= IssabelSignalRClient_StateChanged;
					IssabelSignalRClient.OnAsterNetConfigs -= IssabelSignalRClient_OnAsterNetConfigs;
					IssabelSignalRClient.OnAsteriskNewExten -= IssabelSignalRClient_OnAsteriskNewExten;
					IssabelSignalRClient.OnQueueAssingedToAgent -= IssabelSignalRClient_OnQueueAssingedToAgent;
					IssabelSignalRClient.OnQueueAbandon -= IssabelSignalRClient_OnQueueAbandon;
					await IssabelSignalRClient.DisposeAsync();
					IssabelSignalRClient = null;
				}
				if (ReportSignalRClient != null)
				{
					ReportSignalRClient.OnNotifToClient -= ReportSignalRClient_OnNotifToClient;
					await ReportSignalRClient.DisposeAsync();
					ReportSignalRClient = null;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DisposeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\CallCenter\\IssabelInstanceHolder.cs", 161);
		}
	}

	public static async Task Issabel_OnAsteriskNewState(object arg1, AsteriskNewStateEventArgs e)
	{
		_ = 3;
		try
		{
			if (e != null || _semaphore != null)
			{
				await _semaphore.WaitAsync();
				CallingLogBusiness masterCall = await SaveMasterLogAsync(e);
				if (masterCall != null)
				{
					await SaveDetCallAsync(e, masterCall);
					await SetSaveResponseAsync(masterCall);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Issabel_OnAsteriskNewState", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\CallCenter\\IssabelInstanceHolder.cs", 175);
		}
		finally
		{
			_semaphore.Release();
		}
	}

	public static Task Issabel_ConnectionStateChanged(object sender, ConnectionStateEventArgs e)
	{
		return IssabelInstanceHolder.ConnectionStateChanged?.Invoke(sender, e);
	}

	public static async Task Issabel_OnAgentConnectEvent(object arg1, AsteriskAgentConnectEventArgs e)
	{
		_ = 4;
		try
		{
			if (e != null || _semaphore != null)
			{
				await _semaphore.WaitAsync();
				CallingLogReportBusiness callingLogReportBusiness = await CallingLogBusiness.GetReportAsync(ContextGuid, e.LinkedId);
				CallingLogBusiness masterCall = new CallingLogBusiness(ContextGuid)
				{
					Uuid = e.LinkedId,
					DialedNumber = e.Member,
					CallerIdNumber = (callingLogReportBusiness?.CallerIdNumber ?? e.CallerIdNumber),
					PickUpDateM = (callingLogReportBusiness?.PickUpDateM ?? e.DateReceived),
					HangUpDateM = (callingLogReportBusiness?.HangUpDateM ?? null),
					DateM = (callingLogReportBusiness?.DateM ?? DateTime.Now),
					CoGuid = null,
					ContextGuid = ContextGuid,
					Modified = DateTime.Now,
					Description = Json.ToStringJson(e),
					ExGuid = null,
					Version = (callingLogReportBusiness?.Version ?? 0),
					IsIncoming = (callingLogReportBusiness?.IsIncoming ?? false),
					Rings = (callingLogReportBusiness?.Rings ?? 0),
					DeliveryDate = null,
					DeliveryId = 0,
					DeliveryMessage = "",
					DeliveryShortMessage = "",
					HostRefrenced = "",
					ServerStatus = ServerStatusForClient.None,
					Status = EnumNotification.ConversationStarted
				};
				if (!(await masterCall.SaveAsync(EnSystem.None, sendToServer: true, isFromSync: false, awaitOnSemaphore: true, Priority.Low, await4Send: false, 10)).HasError)
				{
					await new CallingLogDetailBusiness(ContextGuid)
					{
						LinkedId = masterCall.Uuid,
						Uuid = e.UniqueId,
						CallFrom_Number = masterCall.CallerIdNumber,
						CallTo_Number = masterCall.DialedNumber,
						DateInsert = e.DateReceived,
						Description = Json.ToStringJson(new List<AsteriskAgentConnectEventArgs> { e }),
						PickUpDateM = e.DateReceived,
						HangUpDateM = (masterCall?.HangUpDateM ?? null)
					}.SaveAsync(EnSystem.None);
					await SetSaveResponseAsync(masterCall);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Issabel_OnAgentConnectEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\CallCenter\\IssabelInstanceHolder.cs", 227);
		}
		finally
		{
			_semaphore.Release();
		}
	}

	public static async Task Issabel_OnAsteriskQueueCallerJoin(object arg1, AsteriskQueueCallerJoinEventArgs e)
	{
		try
		{
			if (e != null || _semaphore != null)
			{
				await _semaphore.WaitAsync();
				if (DateTime.Now.Year > 2025)
				{
					throw new NotImplementedException();
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Issabel_OnAsteriskQueueCallerJoin", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\CallCenter\\IssabelInstanceHolder.cs", 239);
		}
		finally
		{
			_semaphore.Release();
		}
	}

	public static async Task Issabel_OnAsteriskQueueCallerAbandon(object arg1, AsteriskQueueCallerAbandonEventArgs e)
	{
		try
		{
			if (e != null || _semaphore != null)
			{
				await _semaphore.WaitAsync();
				if (DateTime.Now.Year > 2025)
				{
					throw new NotImplementedException();
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Issabel_OnAsteriskQueueCallerAbandon", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\CallCenter\\IssabelInstanceHolder.cs", 251);
		}
		finally
		{
			_semaphore.Release();
		}
	}

	public static async Task Issabel_OnAsteriskQueueCallerLeave(object arg1, AsteriskQueueCallerLeaveEventArgs e)
	{
		try
		{
			if (e != null || _semaphore != null)
			{
				await _semaphore.WaitAsync();
				if (DateTime.Now.Year > 2025)
				{
					throw new NotImplementedException();
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Issabel_OnAsteriskQueueCallerLeave", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\CallCenter\\IssabelInstanceHolder.cs", 263);
		}
		finally
		{
			_semaphore.Release();
		}
	}

	public static async Task Issabel_OnAsteriskHangup(object arg1, AsteriskHangupEventArgs e)
	{
		_ = 3;
		try
		{
			if (e == null && _semaphore == null)
			{
				return;
			}
			await _semaphore.WaitAsync();
			CallingLogReportBusiness callingLogReportBusiness = await CallingLogBusiness.GetReportAsync(ContextGuid, e.LinkedUuid);
			CallingLogBusiness callingLogBusiness = new CallingLogBusiness(ContextGuid)
			{
				Uuid = e.LinkedUuid,
				DialedNumber = (callingLogReportBusiness?.DialedNumber ?? e.Connectedlinenum),
				CallerIdNumber = (callingLogReportBusiness?.CallerIdNumber ?? e.CallerIdNum),
				PickUpDateM = callingLogReportBusiness?.PickUpDateM,
				HangUpDateM = e.DateReceived,
				DateM = (callingLogReportBusiness?.DateM ?? DateTime.Now),
				CoGuid = null,
				ContextGuid = ContextGuid,
				Modified = DateTime.Now,
				Description = Json.ToStringJson(e),
				ExGuid = null,
				Version = (callingLogReportBusiness?.Version ?? 0),
				IsIncoming = (callingLogReportBusiness?.IsIncoming ?? true),
				Rings = (callingLogReportBusiness?.Rings ?? 0),
				IsCompleteCall = true
			};
			if (callingLogReportBusiness != null && callingLogReportBusiness.Status == EnumNotification.ConversationStarted)
			{
				callingLogBusiness.Status = EnumNotification.ConversationFinished;
			}
			else if (callingLogReportBusiness == null || !callingLogReportBusiness.PickUpDateM.HasValue)
			{
				callingLogBusiness.Status = EnumNotification.MissedCall;
			}
			CallingLogDetailBusiness callDet = new CallingLogDetailBusiness(ContextGuid)
			{
				Uuid = e.Uuid,
				LinkedId = e.LinkedUuid,
				CallFrom_Number = callingLogBusiness.CallerIdNumber,
				CallTo_Number = callingLogBusiness.DialedNumber,
				DateInsert = e.DateReceived,
				Description = Json.ToStringJson(new List<AsteriskHangupEventArgs> { e }),
				PickUpDateM = callingLogBusiness.PickUpDateM,
				HangUpDateM = callingLogBusiness.HangUpDateM,
				CallTo_Name = e.CallerIdName,
				CallFrom_Name = e.ConnectedLineName
			};
			if (!callingLogBusiness.PickUpDateM.HasValue && callingLogBusiness.HangUpDateM.HasValue)
			{
				callingLogBusiness.Status = EnumNotification.MissedCall;
				callDet.Status = EnumNotification.MissedCall;
			}
			if (callingLogBusiness.PickUpDateM.HasValue && callingLogBusiness.HangUpDateM.HasValue)
			{
				callingLogBusiness.Status = EnumNotification.ConversationFinished;
				callDet.Status = EnumNotification.ConversationFinished;
			}
			if ((callingLogReportBusiness == null || !callingLogReportBusiness.PickUpDateM.HasValue) && e.ChannelStateDesc.ToLower() != "up")
			{
				callingLogBusiness.Status = EnumNotification.MissedCall;
				callingLogBusiness.HangUpDateM = null;
				callDet.HangUpDateM = null;
			}
			if (GetValue("context", e.Attributes) == "app-blackhole")
			{
				callingLogBusiness.HangUpDateM = null;
				callingLogBusiness.Status = EnumNotification.MissedCall;
				callDet.HangUpDateM = null;
				callDet.Status = EnumNotification.MissedCall;
			}
			ReturnedSaveFuncInfo obj = await callingLogBusiness.SaveAsync(EnSystem.None, sendToServer: true, isFromSync: false, awaitOnSemaphore: true, Priority.Low, await4Send: false, 10);
			obj.throwExceptionIfError();
			if (obj.HasError)
			{
				return;
			}
			if (!string.IsNullOrEmpty(GetValue("exten", e.Attributes)))
			{
				if (callDet != null && (callDet.CallFrom_Number?.Length).GetValueOrDefault() == 3)
				{
					if (callDet != null && (callDet.CallTo_Number?.Length).GetValueOrDefault() == 3)
					{
						return;
					}
				}
			}
			(await callDet.SaveAsync(EnSystem.None)).throwExceptionIfError();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Issabel_OnAsteriskHangup", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\CallCenter\\IssabelInstanceHolder.cs", 343);
		}
		finally
		{
			_semaphore.Release();
		}
	}

	public static async Task SetSaveResponseAsync(CallingLogBusiness callingLog)
	{
		using (new JaegerMonitor("SetSaveResponseAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\CallCenter\\IssabelInstanceHolder.cs"))
		{
			_ = 1;
			try
			{
				WebCallingLog saveResponseAsync = await CallingLogMapper.Instance.MapAsync(ContextGuid, callingLog, new WebCallingLog());
				await GetInstance(ContextGuid).IssabelSignalRClient.SetSaveResponseAsync(saveResponseAsync);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetSaveResponseAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\CallCenter\\IssabelInstanceHolder.cs", 354);
			}
		}
	}

	private static async Task<CallingLogBusiness> SaveMasterLogAsync(AsteriskNewStateEventArgs e)
	{
		_ = 1;
		checked
		{
			try
			{
				GetValue("context", e.Attributes);
				CallingLogReportBusiness callingLogReportBusiness = await CallingLogBusiness.GetReportAsync(ContextGuid, e.LinkedUuid);
				CallingLogBusiness masterCall = new CallingLogBusiness(ContextGuid)
				{
					Uuid = e.LinkedUuid,
					CallerIdNumber = (callingLogReportBusiness?.CallerIdNumber ?? e.CallerIdNumber),
					DialedNumber = (callingLogReportBusiness?.DialedNumber ?? e.ConnectedLineNumber),
					PickUpDateM = null,
					HangUpDateM = null,
					CoGuid = Guid.Empty,
					DateM = (callingLogReportBusiness?.DateM ?? DateTime.Now),
					ContextGuid = ContextGuid,
					Modified = DateTime.Now,
					Description = Json.ToStringJson(e),
					ExGuid = Guid.Empty,
					Version = (callingLogReportBusiness?.Version ?? 0),
					IsIncoming = (callingLogReportBusiness?.IsIncoming ?? e.IsIncoming),
					Rings = (callingLogReportBusiness?.Rings ?? 0),
					ServerStatus = ServerStatusForClient.None
				};
				if (e.ChannelStateDesc.ToLower() == "up")
				{
					masterCall.PickUpDateM = e.DateReceived;
				}
				if (e.CallerIdNumber.Length == 3 && e.ConnectedLineNumber.Length == 3 && e.ChannelStateDesc.ToLower() == "up")
				{
					masterCall.PickUpDateM = e.DateReceived;
				}
				if (masterCall.CallerIdNumber == masterCall.DialedNumber)
				{
					masterCall.CallerIdNumber = e.CallerIdNumber;
				}
				masterCall.Status = CallingLogBusiness.GetStatus(e.ChannelStateDesc, masterCall.Status, masterCall.PickUpDateM, masterCall.HangUpDateM);
				if (string.IsNullOrEmpty(masterCall.CallerIdNumber))
				{
					masterCall.CallerIdNumber = e.CallerIdNumber;
				}
				if (string.IsNullOrEmpty(masterCall.DialedNumber))
				{
					masterCall.DialedNumber = e.CallerIdNumber;
				}
				if (string.IsNullOrEmpty(masterCall.DialedNumber))
				{
					masterCall.DialedNumber = GetValue("exten", e.Attributes);
				}
				if (e.ChannelStateDesc.ToLower() == "ringing".ToLower())
				{
					masterCall.Rings++;
				}
				(await masterCall.SaveAsync(EnSystem.None, sendToServer: true, isFromSync: false, awaitOnSemaphore: true, Priority.Low, await4Send: false, 10)).throwExceptionIfError();
				return masterCall;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveMasterLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\CallCenter\\IssabelInstanceHolder.cs", 396);
				throw;
			}
		}
	}

	private static async Task SaveDetCallAsync(AsteriskNewStateEventArgs e, CallingLogBusiness masterCall)
	{
		_ = 4;
		try
		{
			if (string.IsNullOrEmpty(e.LinkedUuid) || masterCall.Uuid == e.Uuid)
			{
				return;
			}
			int num = e.CallerIdNumber?.ParseToInt() ?? 0;
			Guid value = Guid.Empty;
			if (num > 0)
			{
				value = await CashierBusiness.GetCashierGuidByIssabelExtentionNumberAsync(ContextGuid, num, null);
			}
			CallingLogDetailBusiness callDet = new CallingLogDetailBusiness(ContextGuid)
			{
				Uuid = e.Uuid,
				LinkedId = e.LinkedUuid,
				CallFrom_Number = (masterCall?.CallerIdNumber ?? e.ConnectedLineNumber),
				CallFrom_Name = e.ConnectedLineName,
				CallTo_Number = (masterCall?.DialedNumber ?? e.CallerIdNumber),
				CallTo_Name = e.CallerIdName,
				DateInsert = e.DateReceived,
				CashierGuid = value,
				Description = Json.ToStringJson(new List<AsteriskNewStateEventArgs> { e })
			};
			List<AsteriskNewStateEventArgs> list = callDet.Description.FromJson<List<AsteriskNewStateEventArgs>>("SaveDetCallAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\CallCenter\\IssabelInstanceHolder.cs", 422) ?? new List<AsteriskNewStateEventArgs>();
			list.Add(e);
			callDet.Description = Json.ToStringJson(list);
			if (string.IsNullOrEmpty(callDet.CallFrom_Number))
			{
				callDet.CallFrom_Number = e.CallerIdNumber;
			}
			if (string.IsNullOrEmpty(callDet.CallFrom_Name))
			{
				callDet.CallFrom_Name = e.CallerIdName;
			}
			if (string.IsNullOrEmpty(callDet.CallTo_Number))
			{
				callDet.CallTo_Number = e.ConnectedLineNumber;
			}
			if (string.IsNullOrEmpty(callDet.CallTo_Name))
			{
				callDet.CallTo_Name = e.ConnectedLineName;
			}
			string value2 = GetValue("context", e.Attributes);
			if (value2 == "macro-dial-one" && e.ChannelStateDesc == "Up")
			{
				callDet.PickUpDateM = e.DateReceived;
			}
			if (value2 == "macro-dial-trunk" && e.ChannelStateDesc == "Up")
			{
				callDet.PickUpDateM = e.DateReceived;
			}
			if (value2 == "macro-dial" && e.ChannelStateDesc == "Up")
			{
				callDet.PickUpDateM = e.DateReceived;
			}
			EnumNotification status = CallingLogBusiness.GetStatus(e.ChannelStateDesc, callDet.Status, callDet.PickUpDateM, callDet.HangUpDateM);
			switch (status)
			{
			case EnumNotification.IncommingCall:
				callDet.Status = status;
				break;
			case EnumNotification.ConversationStarted:
				callDet.Status = status;
				callDet.PickUpDateM = masterCall.PickUpDateM;
				if (!masterCall.PickUpDateM.HasValue)
				{
					masterCall.PickUpDateM = DateTime.Now;
					masterCall.Status = EnumNotification.ConversationStarted;
					(await masterCall.SaveAsync(EnSystem.None, sendToServer: true, isFromSync: false, awaitOnSemaphore: true, Priority.Low, await4Send: false, 10)).throwExceptionIfError();
				}
				break;
			case EnumNotification.ConversationFinished:
				callDet.Status = status;
				callDet.HangUpDateM = masterCall.HangUpDateM;
				break;
			case EnumNotification.Busy:
				callDet.Status = status;
				break;
			default:
				callDet.Status = status;
				break;
			}
			(await callDet.SaveAsync(EnSystem.SignalR)).throwExceptionIfError();
			WebCallingLog saveResponseAsync = await CallingLogMapper.Instance.MapAsync(ContextGuid, masterCall, new WebCallingLog());
			await GetInstance(ContextGuid).IssabelSignalRClient.SetSaveResponseAsync(saveResponseAsync);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveDetCallAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\CallCenter\\IssabelInstanceHolder.cs", 482);
			throw;
		}
	}

	private static Task Issabel_OnDisposed1(object sender, EventArgs arg2)
	{
		try
		{
			_dic.Remove(((IssabelClient)sender).ContextGuid);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Issabel_OnDisposed1", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\CallCenter\\IssabelInstanceHolder.cs", 492);
		}
		return Task.CompletedTask;
	}

	private static string GetValue(string key, Dictionary<string, string> dic)
	{
		try
		{
			if (dic.TryGetValue(key, out var value))
			{
				return value;
			}
			return "";
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetValue", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\CallCenter\\IssabelInstanceHolder.cs", 504);
			return "";
		}
	}

	private static string ParseFxo(string value)
	{
		try
		{
			return value switch
			{
				"FXO1" => "05137597590", 
				"FXO2" => "05137597592", 
				"FXO3" => "05137597593", 
				"FXO4" => "05137597594", 
				"FXO5" => "05137597595", 
				"FXO6" => "05137597596", 
				"FXO7" => "05137597597", 
				"FXO8" => "05137597591", 
				_ => value, 
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ParseFxo", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\CallCenter\\IssabelInstanceHolder.cs", 523);
			throw;
		}
	}

	private static async Task SaveDetCallAsync(AsteriskNewExtenEventArgs e, CallingLogBusiness masterCall)
	{
		_ = 2;
		try
		{
			throw new NotImplementedException();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveDetCallAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\CallCenter\\IssabelInstanceHolder.cs", 655);
			throw;
		}
	}

	private static async Task<CallingLogBusiness> SaveMasterLogAsync(AsteriskNewExtenEventArgs e)
	{
		_ = 1;
		try
		{
			throw new NotImplementedException();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveMasterLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\CallCenter\\IssabelInstanceHolder.cs", 778);
			throw;
		}
	}

	private static async Task<string> GetTafsilNameAsync(string number)
	{
		string res = "";
		try
		{
			TafsilNumberBusiness tafsilNumberBusiness = (await TafsilNumberBusiness.GetInfoAsync(ContextGuid, number.ParseToLong(0L), true)).FirstOrDefault();
			if (tafsilNumberBusiness == null)
			{
				return res;
			}
			res = (await TafsilBusiness.GetAsync(ContextGuid, EnSystem.SignalR, tafsilNumberBusiness.TafsilGuid, "", default(CancellationToken), "GetTafsilNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\CallCenter\\IssabelInstanceHolder.cs"))?.Name ?? "";
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetTafsilNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\CallCenter\\IssabelInstanceHolder.cs", 792);
		}
		return res;
	}
}
