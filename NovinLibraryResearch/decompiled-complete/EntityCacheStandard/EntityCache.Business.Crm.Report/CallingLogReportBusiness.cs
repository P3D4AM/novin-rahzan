using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EntityCache.Utilities;
using PacketParser;
using PacketParser.Entities;

namespace EntityCache.Business.Crm.Report;

public class CallingLogReportBusiness
{
	public static Func<WebCallingLog, Task> OnSetMessage;

	private DateTime? _pickUpDateM;

	private DateTime? _hangUpDateM;

	public string Uuid { get; set; }

	public string? MasterUuid { get; set; }

	public Guid Guid { get; set; }

	public string CallerIdNumber { get; set; }

	public string NumberForDisplay { get; set; }

	public string DialedNumber { get; set; }

	public Guid? TicketGuid { get; set; }

	public TimeSpan? PickUpTime { get; set; }

	public TimeSpan? HangUpTime { get; set; }

	public DateTime DateM { get; set; }

	public int Version { get; set; }

	public short Rings { get; set; }

	public short StatusId { get; set; }

	public string Message { get; set; }

	public DateTime? PickUpDateM
	{
		get
		{
			if (!_pickUpDateM.HasValue)
			{
				return null;
			}
			checked
			{
				if (_pickUpDateM.Value.Year > DateTime.Now.Year + 10)
				{
					return null;
				}
				if (_pickUpDateM.Value.Year < DateTime.Now.Year - 10)
				{
					return null;
				}
				return _pickUpDateM;
			}
		}
		set
		{
			_pickUpDateM = value;
		}
	}

	public DateTime? HangUpDateM
	{
		get
		{
			if (!_hangUpDateM.HasValue)
			{
				return null;
			}
			checked
			{
				if (_hangUpDateM.Value.Year > DateTime.Now.Year + 10)
				{
					return null;
				}
				if (_hangUpDateM.Value.Year < DateTime.Now.Year - 10)
				{
					return null;
				}
				return _hangUpDateM;
			}
		}
		set
		{
			_hangUpDateM = value;
		}
	}

	public string DateSh => DateM.MiladiToCalture();

	public string Time => $"{DateM.Hour}:{DateM.Minute}:{DateM.Second}";

	public TimeSpan? Duration
	{
		get
		{
			if (HangUpTime.HasValue && PickUpTime.HasValue)
			{
				return HangUpTime.Value.Subtract(PickUpTime.Value);
			}
			return null;
		}
	}

	public TimeSpan? WaitingTime
	{
		get
		{
			if (PickUpTime.HasValue)
			{
				return checked(new TimeSpan(0, PickUpTime.Value.Hours - DateM.Hour, PickUpTime.Value.Minutes - DateM.Minute, PickUpTime.Value.Seconds - DateM.Second));
			}
			return null;
		}
	}

	public EnumNotification Status { get; set; }

	public bool IsIncoming { get; set; }

	public bool IsCompleteCall { get; set; }

	public long CalcedNumber { get; set; }

	public string StatusName { get; set; }

	public EnConversation StatusConversation
	{
		get
		{
			try
			{
				if (IsIncoming && !IsCompleteCall && Status == EnumNotification.ConversationStarted)
				{
					return EnConversation.IncomingCallAnswered;
				}
				if (IsIncoming && Status == EnumNotification.ConversationFinished)
				{
					return EnConversation.IncomingCallCompleted;
				}
				if (!IsIncoming && !IsCompleteCall && Status == EnumNotification.ConversationStarted)
				{
					return EnConversation.OutCominigCallAnswered;
				}
				if (!IsIncoming && Status == EnumNotification.ConversationFinished)
				{
					return EnConversation.OutCominigCallCompleted;
				}
				if (!IsIncoming && !IsCompleteCall && Status == EnumNotification.IncommingCall)
				{
					return EnConversation.MissedCall;
				}
				if (IsIncoming && !IsCompleteCall && Status == EnumNotification.IncommingCall)
				{
					return EnConversation.MissedCall;
				}
				if (Status == EnumNotification.MissedCall)
				{
					return EnConversation.Ringing;
				}
				WebErrorLog.Instance.StartLog($"Call Data: Status:{Status} - IsIncoming:{IsIncoming}", "", "StatusConversation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Report\\CallingLogReportBusiness.cs", 73);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "StatusConversation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Report\\CallingLogReportBusiness.cs", 75);
			}
			return EnConversation.None;
		}
	}

	public byte[] SourceImage
	{
		get
		{
			try
			{
				switch (StatusConversation)
				{
				case EnConversation.MissedCall:
					return ImageResourceManager.MissedCall;
				case EnConversation.IncomingCallAnswered:
					return ImageResourceManager.IncomingCallAnswered;
				case EnConversation.IncomingCallCompleted:
					return ImageResourceManager.IncomingCallCompleted;
				case EnConversation.OutCominigCallAnswered:
					return ImageResourceManager.OutCominigCallAnswered;
				case EnConversation.OutCominigCallCompleted:
					return ImageResourceManager.OutCominigCallCompleted;
				case EnConversation.Ringing:
					return ImageResourceManager.Ringing;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SourceImage", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Report\\CallingLogReportBusiness.cs", 95);
			}
			return ImageResourceManager.ServerStatus_None;
		}
	}

	public List<Guid?> CashierList { get; set; }

	public string CallToName { get; set; }

	public string CallFromName { get; set; }
}
