using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.BusinessLayer;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Crm;
using PacketParser.EventArgs;

namespace PacketParser.Entities;

public class WebCallingLog : Serializable<WebCallingLog>, IReceiver, ICallingLog, IHasModified, IHasStatus, INpDisposable, IDisposable
{
	private bool _isDisposed;

	private WebTafsilNumbers _webTafsilNumbers;

	public string FullName { get; set; }

	public DateTime Modified { get; set; }

	public string? MasterUuid { get; set; }

	public DateTime? DateUtc { get; set; }

	[JsonIgnore]
	[XmlIgnore]
	bool IHasStatus.Status => Status == EnumNotification.Deleted;

	public Guid? TicketGuid { get; set; }

	public string CallerIdNumber { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public string NumberForDisplay
	{
		get
		{
			if (!string.IsNullOrEmpty(DialedNumber))
			{
				if (DialedNumber.Length == 10)
				{
					return "0" + DialedNumber;
				}
				return DialedNumber;
			}
			if (!string.IsNullOrEmpty(CallerIdNumber))
			{
				if (CallerIdNumber.Length == 10)
				{
					return "0" + CallerIdNumber;
				}
				return CallerIdNumber;
			}
			return "";
		}
	}

	public short Rings { get; set; }

	public short StatusId { get; set; }

	public string Message { get; set; }

	public string DialedNumber { get; set; }

	public DateTime? PickUpDateM { get; set; }

	[JsonIgnore]
	[XmlIgnore]
	public double DurationSeconds
	{
		get
		{
			if (!HangUpDateM.HasValue || !PickUpDateM.HasValue)
			{
				return 0.0;
			}
			return HangUpDateM?.Subtract(PickUpDateM.Value).TotalSeconds ?? 0.0;
		}
	}

	[JsonIgnore]
	[XmlIgnore]
	public string DurationSecondsString
	{
		get
		{
			if (!(DurationSeconds > 0.0))
			{
				return "hh:mm:ss";
			}
			return Time.IntToTime(DurationSeconds);
		}
	}

	public DateTime? HangUpDateM { get; set; }

	public DateTime DateM { get; set; }

	public Guid? CoGuid { get; set; }

	public Guid? ExGuid { get; set; }

	public EnumNotification Status { get; set; }

	public bool IsIncoming { get; set; }

	public bool IsCompleteCall { get; set; }

	public string Description { get; set; }

	public string DateSh { get; set; }

	public DateTime ResetTime { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public int Version { get; set; }

	public WebCallingLine Co { get; set; }

	public WebCallingLine Extension { get; set; }

	public List<WebCallingLogDetail> ListCallingLogDetail { get; set; } = new List<WebCallingLogDetail>();


	public string HostRefrenced { get; set; }

	[JsonIgnore]
	public WebTafsilNumbers TafsilNumber
	{
		get
		{
			if (_webTafsilNumbers != null)
			{
				return _webTafsilNumbers;
			}
			if (CallerIdNumber.Length == DialedNumber.ToString().Length)
			{
				_webTafsilNumbers = new WebTafsilNumbers(CallerIdNumber);
			}
			else
			{
				_webTafsilNumbers = new WebTafsilNumbers(DialedNumber);
			}
			return _webTafsilNumbers;
		}
	}

	[XmlIgnore]
	[JsonIgnore]
	public new bool IsDisposed
	{
		get
		{
			if (!base.IsDisposed)
			{
				return _isDisposed;
			}
			return true;
		}
	}

	public long CalcedNumber { get; set; }

	public string Uuid { get; set; }

	public event Func<object, CallingLogReadEventArgs, Task> OnReadRequest;

	public event Func<object, ConnectionEventArgs<WebCallingLog>, Task> OnReadResponse;

	public event Func<object, ConnectionEventArgs<WebCallingLog>, Task> OnSaveRequest;

	public event Func<object, SaveResponseEventArgs, Task> OnSaveResponse;

	public event Func<object, ConnectionEventArgs<WebCallingLog>, Task> OnSync;

	public event Func<object, ConnectionEventArgs<Guid>, Task> OnDeletedRequest;

	public async Task<WebTelegramMessage> ToWebTelegramAsync(short depId, CancellationToken token)
	{
		try
		{
			string text = await GenerateMessageForTelegramAsync(depId, token);
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}
			return new WebTelegramMessage
			{
				Guid = Guid.NewGuid(),
				ApiKey = "1169869009:AAH2wBO6N0zsWr45bDCdprVUCI2L-YVINS8",
				Message = text,
				contactType = EnTelegramType.Chanel,
				ContactInfo = "@NovinPardazCallings",
				UrlCaption = "مشاهده جزئیات"
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ToWebTelegramAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCallingLog.cs", 119);
			return null;
		}
	}

	public static List<Packet> GenerateDeleteRequest(IEnumerable<Guid> callings, int partitionId)
	{
		using (new JaegerMonitor("GenerateDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCallingLog.cs"))
		{
			try
			{
				object result;
				if (callings == null)
				{
					result = null;
				}
				else
				{
					result = callings.GeneratePackets(PacketType.CallingLog, 8, null, isSync: false, null, partitionId, deliveryRequst: true, "GenerateDeleteRequest");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCallingLog.cs", 156);
				return null;
			}
		}
	}

	public async Task<string> GenerateMessageForTelegramAsync(short depId, CancellationToken token)
	{
		StringBuilder sb = new StringBuilder(100000);
		int line = 0;
		try
		{
			if ((string.IsNullOrEmpty(CallerIdNumber) || CallerIdNumber.Length <= 3) && (string.IsNullOrEmpty(DialedNumber) || DialedNumber.Length <= 3))
			{
				return "";
			}
			line = 1;
			switch (Status)
			{
			case EnumNotification.MissedCall:
				sb.AppendLine("#تماس_ناموفق");
				sb.AppendLine("\ud83d\udd15⛔\ufe0f☎\ufe0f⛔\ufe0f\ud83d\udd15");
				break;
			case EnumNotification.ConversationFinished:
				if (IsIncoming)
				{
					sb.AppendLine("☎\ufe0f☎\ufe0f☎\ufe0f☎\ufe0f☎\ufe0f");
					sb.AppendLine("#پایان_مکالمه_تماس_ورودی");
				}
				else
				{
					sb.AppendLine("☎\ufe0f☎\ufe0f☎\ufe0f☎\ufe0f☎\ufe0f");
					sb.AppendLine("#پایان_مکالمه_تماس_خروجی");
				}
				break;
			case EnumNotification.ConversationStarted:
				if (IsIncoming)
				{
					sb.AppendLine("\ud83d\udde3\ud83d\udde3\ud83d\udde3\ud83d\udde3");
					sb.AppendLine("#در_حال_مکالمه_تماس_ورودی");
				}
				else
				{
					sb.AppendLine("\ud83d\udde3\ud83d\udde3\ud83d\udde3\ud83d\udde3");
					sb.AppendLine("#در_حال_مکالمه_تماس_خروجی");
				}
				break;
			case EnumNotification.IncommingCall:
				sb.AppendLine("\ud83d\udd14\ud83d\udd14☎\ufe0f\ud83d\udd14\ud83d\udd14");
				sb.AppendLine("#در_حال_زنگ_خوردن_تلفن");
				break;
			case EnumNotification.Busy:
				sb.AppendLine("⛔\ufe0f⛔\ufe0f☎\ufe0f⛔\ufe0f⛔\ufe0f");
				sb.AppendLine("#مشغول");
				break;
			case EnumNotification.Dnd:
				sb.AppendLine("⛔\ufe0f⛔\ufe0f☎\ufe0f⛔\ufe0f⛔\ufe0f");
				sb.AppendLine("#مزاحم_نشوید");
				break;
			default:
				if (!string.IsNullOrEmpty(CallerIdNumber))
				{
					sb.AppendLine("#تماس_ورودی2");
				}
				else if (!string.IsNullOrEmpty(DialedNumber))
				{
					sb.AppendLine("#تماس_خروجی2");
				}
				return "";
			}
			line = 2;
			if (!string.IsNullOrEmpty(NumberForDisplay))
			{
				sb.AppendLine(NumberForDisplay);
				line = 3;
				sb.AppendLine(NumberForDisplay.fixNumberAndPutSharp());
				line = 4;
			}
			string text = "\ud83d\udcc5";
			line = 5;
			sb.AppendLine(text + " تاریخ : " + DateSh + " " + DateM.ToString("H:mm:ss"));
			line = 6;
			if (PickUpDateM.HasValue)
			{
				sb.AppendLine("شروع مکالمه : " + (PickUpDateM?.ToString("H:mm:ss") ?? "-"));
			}
			if (HangUpDateM.HasValue)
			{
				sb.AppendLine("پایان مکالمه : " + (HangUpDateM?.ToString("H:mm:ss") ?? "-"));
			}
			if (DurationSeconds > 0.0 && !string.IsNullOrEmpty(DurationSecondsString))
			{
				if (DurationSeconds < 120.0)
				{
					sb.AppendLine("\ud83d\udfe2مدت مکالمه : " + DurationSecondsString);
				}
				else if (DurationSeconds < 420.0)
				{
					sb.AppendLine("\ud83d\udfe1مدت مکالمه : " + DurationSecondsString);
				}
				else if (DurationSeconds < 900.0)
				{
					sb.AppendLine("\ud83d\udfe0مدت مکالمه : " + DurationSecondsString);
				}
				else
				{
					sb.AppendLine("#مکالمه_طولانی");
					sb.AppendLine("\ud83d\udd34مدت مکالمه : " + DurationSecondsString);
				}
			}
			line = 7;
			if (Rings > 0)
			{
				sb.AppendLine($"تعداد زنگ: \ud83d\udd14{Rings}\ud83d\udd14");
			}
			line = 8;
			if (!string.IsNullOrEmpty(Description))
			{
				sb.AppendLine("Desc:" + Description);
			}
			ReturnedSaveFuncInfoWithValue<List<WebCus>> returnedSaveFuncInfoWithValue = await WebCus.GetByNumberAsync(NumberForDisplay.ParseToLong(0L), token);
			if (returnedSaveFuncInfoWithValue.HasError)
			{
				return "خطا در فراخوانی api بیرونی جهت دریافت مشخصات مشتری\r\n❌❌❌Errro: " + returnedSaveFuncInfoWithValue.ErrorMessage;
			}
			IEnumerable<WebCus> enumerable = returnedSaveFuncInfoWithValue.value?.Where((WebCus p) => p.Status);
			line = 901;
			if ((enumerable == null || !enumerable.Any()) && NumberForDisplay.ParseToLong(0L) > 99999999)
			{
				line = 91;
				returnedSaveFuncInfoWithValue = await WebCus.GetByNumberAsync(NumberForDisplay.Substring(checked(NumberForDisplay.Length - 8), 8).ParseToLong(0L), token);
				if (returnedSaveFuncInfoWithValue.HasError)
				{
					return "خطا در فراخوانی api بیرونی جهت دریافت مشخصات مشتری\r\n❌❌❌Errro: " + returnedSaveFuncInfoWithValue.ErrorMessage;
				}
				enumerable = returnedSaveFuncInfoWithValue.value;
				line = 92;
			}
			line = 93;
			if (enumerable != null && enumerable.Any())
			{
				line = 94;
				if (enumerable.Count() > 1)
				{
					sb.AppendLine($"{enumerable.Count()} نفر با شماره فوق در سیستم یافت شد.");
				}
				line = 95;
				foreach (WebCus item in enumerable)
				{
					if (item != null)
					{
						line = 96;
						sb.AppendLine(item.FullName.FixForTelegram() ?? "");
						line = 97;
						sb.AppendLine($"✅\ud83d\udc49{ResourceManagement.TextBrandResourceManagement.CRMBaseURL}/singlecall/{Uuid}/{item.Guid}");
						line = 11;
						if (item.Account == 0m)
						{
							sb.AppendLine("⚫\ufe0fمانده حساب: تسویه حساب⚫\ufe0f");
						}
						else if (item.Account > 0m)
						{
							sb.AppendLine($"\ud83d\udfe2{Math.Round(item.Account, 0):N0} بستانکار\ud83d\udfe2");
						}
						else
						{
							sb.AppendLine($"❌❌ {Math.Round(-item.Account, 0):N0} بدهکار❌❌");
						}
						line = 12;
					}
				}
			}
			else
			{
				sb.AppendLine("#شماره_ناشناس");
				sb.AppendLine($"‼\ufe0f\ud83d\udc49{ResourceManagement.TextBrandResourceManagement.CRMBaseURL}/singlecall/{Uuid}/{Guid.Empty}");
			}
		}
		catch (WebException ex)
		{
			return ex.Message;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, string.Format("Line:{0}//Disposed:{1}//NumberForDisplay{2}", line, IsDisposed, NumberForDisplay ?? "Null!!"), "GenerateMessageForTelegramAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCallingLog.cs", 318);
			return ex2.Message;
		}
		return sb.ToString();
	}

	public static List<Packet> GenerateDeleteRequest(IEnumerable<WebCallingLog> callings, int partitionId)
	{
		using (new JaegerMonitor("GenerateDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCallingLog.cs"))
		{
			try
			{
				object result;
				if (callings == null)
				{
					result = null;
				}
				else
				{
					result = callings.GeneratePackets(PacketType.CallingLog, 8, null, isSync: false, null, partitionId, deliveryRequst: true, "GenerateDeleteRequest");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCallingLog.cs", 333);
				throw;
			}
		}
	}

	public static Packet GenerateReadRequest(int requestedUId, DateTime modified)
	{
		using (new JaegerMonitor("GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCallingLog.cs"))
		{
			try
			{
				List<string> list = new List<string> { ((short)5).ToString() };
				CallingLogReadEventArgs callingLogReadEventArgs = new CallingLogReadEventArgs
				{
					Modified = modified
				};
				list.Add(callingLogReadEventArgs.ToString());
				return new Packet(PacketType.CallingLog, 5, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCallingLog.cs", 356);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReadResponse(IEnumerable<WebCallingLog> callings, int partitionId, bool isSync = false)
	{
		if (callings == null || !callings.Any())
		{
			throw new NullReferenceException("CallingLog List is null or empty!!!");
		}
		using (new JaegerMonitor("GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCallingLog.cs"))
		{
			try
			{
				object result;
				if (callings == null)
				{
					result = null;
				}
				else
				{
					result = callings.GeneratePackets(PacketType.CallingLog, 6, null, isSync, null, partitionId, deliveryRequst: true, "GenerateReadResponse");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCallingLog.cs", 371);
				return null;
			}
		}
	}

	public static List<Packet> GenerateSaveRequest(IEnumerable<WebCallingLog> callings, int partitionId)
	{
		using (new JaegerMonitor("GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCallingLog.cs"))
		{
			try
			{
				object result;
				if (callings == null)
				{
					result = null;
				}
				else
				{
					result = callings.GeneratePackets(PacketType.CallingLog, 1, null, isSync: false, null, partitionId, deliveryRequst: false, "GenerateSaveRequest");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCallingLog.cs", 385);
				throw;
			}
		}
	}

	public static List<Packet> GenerateSaveResponse(IEnumerable<SaveResponse> responses)
	{
		using (new JaegerMonitor("GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCallingLog.cs"))
		{
			try
			{
				return responses?.GeneratePackets(PacketType.CallingLog, 2, null, isSync: false, null, 0, deliveryRequst: false, "GenerateSaveResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCallingLog.cs", 399);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReset(IEnumerable<WebCallingLog> callings)
	{
		using (new JaegerMonitor("GenerateReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCallingLog.cs"))
		{
			try
			{
				return callings.GeneratePackets(PacketType.CallingLog, 7, null, isSync: false, null, 0, deliveryRequst: true, "GenerateReset");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCallingLog.cs", 412);
				return null;
			}
		}
	}

	public string GetPacketActionEnumName(short action)
	{
		try
		{
			CallingLogActions callingLogActions = (CallingLogActions)action;
			return callingLogActions.ToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionEnumName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCallingLog.cs", 427);
			return $"Error({action})";
		}
	}

	public string GetPacketActionName(short action)
	{
		try
		{
			return ((CallingLogActions)action).GetDisplay();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCallingLog.cs", 440);
			return $"Error({action})";
		}
	}

	public async Task DataReceived(Packet pack, string connectionUniqueId, ConnectionData connection)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCallingLog.cs"))
		{
			_ = 5;
			try
			{
				switch ((CallingLogActions)pack.PacketActions)
				{
				case CallingLogActions.SaveRequest:
					if (this.OnSaveRequest != null)
					{
						ConnectionEventArgs<WebCallingLog> args5 = new ConnectionEventArgs<WebCallingLog>(pack, connectionUniqueId, connection);
						await Utilities.RaiseEvent(this.OnSaveRequest, this, args5, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCallingLog.cs");
					}
					break;
				case CallingLogActions.SaveResponse:
					if (this.OnSaveResponse != null)
					{
						SaveResponseEventArgs args4 = new SaveResponseEventArgs(pack, connectionUniqueId, connection);
						await Utilities.RaiseEvent(this.OnSaveResponse, this, args4, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCallingLog.cs");
					}
					break;
				case CallingLogActions.ReadRequest:
					if (this.OnReadRequest != null)
					{
						CallingLogReadEventArgs args2 = new CallingLogReadEventArgs
						{
							ConnectionUniqueId = connectionUniqueId,
							Connection = connection
						};
						await Utilities.RaiseEvent(this.OnReadRequest, this, args2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCallingLog.cs");
					}
					break;
				case CallingLogActions.ReadResponse:
				{
					if (this.OnReadResponse == null && this.OnSync == null)
					{
						break;
					}
					ConnectionEventArgs<WebCallingLog> args3 = new ConnectionEventArgs<WebCallingLog>(pack, connectionUniqueId, connection);
					try
					{
						if (pack.IsSynced)
						{
							await Utilities.RaiseEvent(this.OnSync, this, args3, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCallingLog.cs");
						}
						else
						{
							await Utilities.RaiseEvent(this.OnReadResponse, this, args3, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCallingLog.cs");
						}
						break;
					}
					catch (Exception ex2)
					{
						WebErrorLog.Instance.StartLog(ex2, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCallingLog.cs", 483);
						break;
					}
				}
				case CallingLogActions.DeleteRequest:
				{
					if (this.OnDeletedRequest == null)
					{
						break;
					}
					ConnectionEventArgs<Guid> args = new ConnectionEventArgs<Guid>(pack, connectionUniqueId, connection);
					try
					{
						await Utilities.RaiseEvent(this.OnDeletedRequest, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCallingLog.cs");
						break;
					}
					catch (Exception ex)
					{
						WebErrorLog.Instance.StartLog(ex, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCallingLog.cs", 497);
						break;
					}
				}
				case (CallingLogActions)3:
				case (CallingLogActions)4:
				case CallingLogActions.Reset:
					break;
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex4)
			{
				WebErrorLog.Instance.StartLog(ex4, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCallingLog.cs", 509);
			}
		}
	}

	public Task<ReturnedSaveFuncInfo> SetDeliveryAsync(WebDelivery del, CancellationToken token, short tryCount, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token, short tryCount, string tranName)
	{
		throw new NotImplementedException();
	}

	public override void Dispose()
	{
		Dispose(disposing: true);
	}

	public override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				if (this.OnReadRequest != null)
				{
					this.OnReadRequest = null;
				}
				if (this.OnReadResponse != null)
				{
					this.OnReadResponse = null;
				}
				if (this.OnSaveRequest != null)
				{
					this.OnSaveRequest = null;
				}
				if (this.OnSaveResponse != null)
				{
					this.OnSaveResponse = null;
				}
				if (this.OnSync != null)
				{
					this.OnSync = null;
				}
				if (this.OnDeletedRequest != null)
				{
					this.OnDeletedRequest = null;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCallingLog.cs", 540);
		}
	}

	public Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token, string tranName, short maxParalelTask)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> Send2ServerAsync(CancellationToken token, EnSystem system, Priority priority)
	{
		throw new NotImplementedException();
	}
}
