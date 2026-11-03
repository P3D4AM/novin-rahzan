using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Crm;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Notification;

namespace EntityCache.Business.Accounting;

public class CrmNotificationBusiness : Serializable<CrmNotificationBusiness>, ICrmNotification, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IDisposable, IHasContext
{
	private bool _isDisposed;

	private short _ordinal;

	private CancellationTokenSource _disposedToken = new CancellationTokenSource();

	private DateTime _endTime;

	private int _maxAliveMiliseconds;

	private bool _isPaused;

	public new bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return base.IsDisposed;
			}
			return true;
		}
	}

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; } = true;


	public string Duration { get; set; }

	public EnumNotification Type { get; set; }

	public DateTime EventTime { get; set; }

	public string DialedNumber { get; set; }

	public bool IsIncoming { get; set; }

	public string CallerIdNumber { get; set; }

	public string Number
	{
		get
		{
			if (!IsIncoming)
			{
				return DialedNumber;
			}
			return CallerIdNumber;
		}
	}

	[JsonIgnore]
	public string NumberForDisplay => new WebTafsilNumbers(CallerIdNumber).CalcedNumber.ToString();

	public Guid? CoGuid { get; set; }

	public string Uuid { get; set; }

	public bool is_Pause
	{
		get
		{
			return _isPaused;
		}
		set
		{
			_isPaused = value;
			if (is_Pause)
			{
				_disposedToken?.Cancel();
				return;
			}
			_disposedToken?.Cancel();
			_disposedToken = new CancellationTokenSource(_maxAliveMiliseconds);
			_endTime = DateTime.Now.AddMilliseconds(_maxAliveMiliseconds);
			Task.Run(() => AutoNotificationDispose(_disposedToken?.Token ?? new CancellationTokenSource(_maxAliveMiliseconds).Token));
		}
	}

	public short Ordinal
	{
		get
		{
			return _ordinal;
		}
		set
		{
			_ordinal = value;
			RaiseOnOridinalChangedEvent();
		}
	}

	public short Ring { get; set; }

	public string ContextGuid { get; set; }

	public event Func<object, EventArgs, Task> OnDispose;

	public event Func<object, EventArgs, Task> OnOridinalChanged;

	public event Func<object, EventArgs, Task> OnChange;

	public event Func<object, DisposeRemainEventArgs, Task> OnDisposeRemain;

	public Task<CallingLineBusiness> GetCoAsync(CancellationToken token = default(CancellationToken))
	{
		return CallingLineBusiness.GetAsync(ContextGuid, CoGuid ?? Guid.Empty, token);
	}

	public CrmNotificationBusiness()
	{
	}

	public CrmNotificationBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public CrmNotificationBusiness(string contextGuid, int maxAliveMiliseconds)
	{
		try
		{
			ContextGuid = contextGuid;
			_maxAliveMiliseconds = maxAliveMiliseconds;
			_disposedToken?.Cancel();
			_disposedToken = new CancellationTokenSource(maxAliveMiliseconds);
			_endTime = DateTime.Now.AddMilliseconds(maxAliveMiliseconds);
			Task.Run(() => AutoNotificationDispose(_disposedToken.Token));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CrmNotificationBusiness.cs", 105);
		}
	}

	public async Task<IEnumerable<TafsilNumberBusiness>> GetTafsilNumberAsync(string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return string.IsNullOrEmpty(CallerIdNumber) ? null : (await TafsilNumberBusiness.GetInfoAsync(ContextGuid, CallerIdNumber.ParseToLong(0L), null, tranName, token));
	}

	public async Task<string> GetOperatorNameAsync(CancellationToken token = default(CancellationToken))
	{
		string contextGuid = ContextGuid;
		return (await CashierBusiness.GetAsync(contextGuid, (await GetCoAsync())?.CashierGuid ?? Guid.Empty, token))?.Name ?? "";
	}

	public async Task<string> GetBodyAsync(EnSystem system, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 7;
		try
		{
			string name = await GetNameAsync(system, tranName, token);
			switch (Type)
			{
			case EnumNotification.ConversationStarted:
			{
				string result;
				if (IsIncoming)
				{
					string translate2 = "تماس ورودی".GetTranslate();
					string text = (await GetCoAsync())?.Number;
					string text2 = await GetOperatorNameAsync();
					result = translate2 + " " + text + " " + text2;
				}
				else
				{
					string text = "تماس خروجی با".GetTranslate();
					string translate2 = DialedNumber;
					string text3 = name;
					string translate = "از".GetTranslate();
					string text4 = (await GetCoAsync())?.Number;
					string text2 = await GetOperatorNameAsync();
					result = text + "  " + translate2 + " " + text3 + "  " + translate + "  " + text4 + " " + text2;
				}
				return result;
			}
			case EnumNotification.ConversationFinished:
				return Number + " " + name + "مدت مکالمه".GetTranslate() + " :" + Duration;
			case EnumNotification.IncommingCall:
			{
				string text4 = "تماس ورودی به خط".GetTranslate();
				string translate = (await GetCoAsync())?.Number;
				string result = await GetOperatorNameAsync();
				return text4 + "  " + translate + " " + result + " از شماره " + DialedNumber + " " + name;
			}
			case EnumNotification.MissedCall:
				return "تماس از دست رفته".GetTranslate();
			case EnumNotification.Connected:
				return "برقراری ارتباط با دستگاه".GetTranslate();
			case EnumNotification.Dnd:
				return "لطفا مزاحم نشوید".GetTranslate();
			case EnumNotification.Disconnected:
				return "قطع ارتباط با دستگاه".GetTranslate();
			case EnumNotification.LineDisconnect:
			{
				string translate = "قطع ارتباط خط متصل شماره ".GetTranslate();
				return translate + " " + (await GetCoAsync())?.Number;
			}
			case EnumNotification.Busy:
				return "مشغول".GetTranslate();
			default:
				return "";
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetBodyAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CrmNotificationBusiness.cs", 135);
			return "";
		}
	}

	public async Task<string> GetNameAsync(EnSystem system, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		string ret = "";
		try
		{
			IEnumerable<TafsilNumberBusiness> enumerable = await GetTafsilNumberAsync(tranName, token);
			if (enumerable != null && enumerable.Any())
			{
				ret = (await CusBusiness.GetAsync(ContextGuid, system, enumerable.FirstOrDefault().TafsilGuid, token))?.FullName ?? "";
			}
			if (string.IsNullOrEmpty(ret) && !string.IsNullOrEmpty(Number))
			{
				CallingLineBusiness callingLineBusiness = await CallingLineBusiness.GetAsync(ContextGuid, Number, token, tranName);
				if (callingLineBusiness != null)
				{
					ret = (await callingLineBusiness.GetCashierNameAsync(token, tranName)) ?? "";
				}
			}
			if (string.IsNullOrEmpty(ret) && CallerIdNumber.ParseToInt() > 0)
			{
				CashierBusiness cashierBusiness = await CashierBusiness.GetByInternalNumberAsync(ContextGuid, CallerIdNumber.ParseToInt(), null, token, tranName);
				if (cashierBusiness != null)
				{
					ret = cashierBusiness.Name;
				}
			}
			if (string.IsNullOrEmpty(ret) && !string.IsNullOrEmpty(CallerIdNumber))
			{
				enumerable = await TafsilNumberBusiness.GetInfoAsync(ContextGuid, new WebTafsilNumbers(CallerIdNumber).CalcedNumber, null, tranName, token);
				if (enumerable != null && enumerable.Any())
				{
					ret = (await CusBusiness.GetAsync(ContextGuid, system, enumerable.FirstOrDefault().TafsilGuid, token))?.FullName ?? "";
				}
			}
			if (string.IsNullOrEmpty(ret))
			{
				ret = "نــاشــنــــــاس".GetTranslate();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CrmNotificationBusiness.cs", 165);
		}
		return ret;
	}

	public async Task<Guid> GetTafsilGuidAsync(string tranName = "", CancellationToken token = default(CancellationToken))
	{
		Guid tfGuid = Guid.Empty;
		try
		{
			IEnumerable<TafsilNumberBusiness> enumerable = await GetTafsilNumberAsync(tranName, token);
			if (enumerable != null && enumerable.Any())
			{
				tfGuid = (await CusBusiness.GetAsync(ContextGuid, EnSystem.UserInterface, enumerable.FirstOrDefault().TafsilGuid, token))?.Guid ?? Guid.Empty;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetTafsilGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CrmNotificationBusiness.cs", 177);
		}
		return tfGuid;
	}

	private async Task AutoNotificationDispose(CancellationToken token)
	{
		_ = 1;
		checked
		{
			try
			{
				while (!token.IsCancellationRequested && !IsDisposed && !token.IsCancellationRequested)
				{
					if (is_Pause)
					{
						await Task.Delay(1000, token);
						continue;
					}
					if (this.OnDisposeRemain != null)
					{
						PacketParser.Utilities.RaiseEvent(this.OnDisposeRemain, this, new DisposeRemainEventArgs
						{
							RemainSecondsNotif = unchecked(_maxAliveMiliseconds / 1000) - (int)_endTime.Subtract(DateTime.Now).TotalSeconds
						}, default(CancellationToken), 60, "AutoNotificationDispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CrmNotificationBusiness.cs");
					}
					await Task.Delay(1000);
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "AutoNotificationDispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CrmNotificationBusiness.cs", 199);
			}
			finally
			{
				Dispose();
			}
		}
	}

	public static string SetNotificationTimerLabel(DateTime time)
	{
		StringBuilder stringBuilder = new StringBuilder(100000);
		try
		{
			TimeSpan timeOfDay = time.TimeOfDay;
			TimeSpan timeSpan = DateTime.Now.TimeOfDay - timeOfDay;
			int hours = timeSpan.Hours;
			int minutes = timeSpan.Minutes;
			int seconds = timeSpan.Seconds;
			if (hours != 0)
			{
				stringBuilder.AppendLine($"{hours}" + " ساعت".GetTranslate());
				if (minutes != 0 || seconds != 0)
				{
					stringBuilder.AppendLine(" و ");
				}
			}
			if (minutes != 0)
			{
				stringBuilder.AppendLine($"{minutes} " + "دقیقه".GetTranslate());
				if (seconds != 0)
				{
					stringBuilder.AppendLine(" و ");
				}
			}
			if (seconds != 0)
			{
				stringBuilder.AppendLine($"{seconds}" + "ثانیه".GetTranslate());
			}
			stringBuilder.AppendLine(" پیش ");
			if (hours == 0 && minutes == 0 && seconds < 3)
			{
				stringBuilder.AppendLine("هم اکنون".GetTranslate());
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetNotificationTimerLabel", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CrmNotificationBusiness.cs", 235);
		}
		return stringBuilder.ToString();
	}

	public void RaiseOnNotificationChange(CallingLogBusiness lastEvent)
	{
		try
		{
			if (lastEvent != null)
			{
				IsIncoming = lastEvent.IsIncoming;
				DialedNumber = lastEvent.DialedNumber;
				CallerIdNumber = lastEvent.CallerIdNumber;
				Type = lastEvent.Status;
				Duration = lastEvent.str_Duration;
				_disposedToken?.Cancel();
				_disposedToken = new CancellationTokenSource(_maxAliveMiliseconds);
				_endTime = DateTime.Now.AddMilliseconds(_maxAliveMiliseconds);
				Task.Run(() => AutoNotificationDispose(_disposedToken.Token));
				if (this.OnChange != null)
				{
					PacketParser.Utilities.RaiseEvent(this.OnChange, this, new EventArgs(), default(CancellationToken), 60, "RaiseOnNotificationChange", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CrmNotificationBusiness.cs");
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RaiseOnNotificationChange", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CrmNotificationBusiness.cs", 258);
		}
	}

	private async Task RaiseOnOridinalChangedEvent()
	{
		if (this.OnOridinalChanged != null)
		{
			await PacketParser.Utilities.RaiseEvent(this.OnOridinalChanged, this, new EventArgs(), default(CancellationToken), 60, "RaiseOnOridinalChangedEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CrmNotificationBusiness.cs");
		}
	}

	public override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_disposedToken?.Cancel();
				_disposedToken?.Dispose();
				_disposedToken = null;
				if (this.OnDispose != null)
				{
					PacketParser.Utilities.RaiseEvent(this.OnDispose, this, new EventArgs(), default(CancellationToken), 60, "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CrmNotificationBusiness.cs");
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CrmNotificationBusiness.cs", 279);
		}
	}
}
