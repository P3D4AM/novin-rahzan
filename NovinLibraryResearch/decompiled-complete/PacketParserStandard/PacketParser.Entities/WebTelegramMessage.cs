using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.Assistance;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;

namespace PacketParser.Entities;

[Serializable]
public class WebTelegramMessage : Serializable<WebTelegramMessage>, IReceiver, INpDisposable, IDisposable, ITelegramConfig
{
	private static ConcurrentDictionary<Guid, CancellationTokenSource> _tokens = new ConcurrentDictionary<Guid, CancellationTokenSource>();

	private static BaseLocks _sendLock = new BaseLocks();

	private static BufferBlock<WebTelegramMessage> _bufferTelegramMessages = new BufferBlock<WebTelegramMessage>();

	private static Task _sendData2TelegramServer = null;

	private int MaxLen = 4000;

	private const int ChunckSizeKB = 100;

	private string message;

	private string _contactInfo;

	private static int _index_GetNp_NovinFestival = 0;

	private static int _index_GetNp_WordpressLogInforamtions = 0;

	private static short _index_GetNp_ErrorHandler2_bot = 0;

	public string Message
	{
		get
		{
			if (!string.IsNullOrEmpty(Url))
			{
				message = message + "\r\n" + Url;
				Url = "";
			}
			return message;
		}
		set
		{
			if (!string.IsNullOrEmpty(value) && value.Length > MaxLen)
			{
				value = value.Substring(0, MaxLen);
			}
			message = value;
		}
	}

	public static IWindowsPlatform WindowsPlatform { get; set; }

	public static string UserInfo { get; set; }

	public string ApiKey { get; set; }

	public long ContactId { get; set; }

	public string ContactInfo
	{
		get
		{
			return _contactInfo;
		}
		set
		{
			if (value.First() != '@')
			{
				WebErrorLog.Instance.StartLog("نام کانال بدون @ وارد شده است " + value, "", "ContactInfo", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTelegramMessage.cs", 94);
			}
			_contactInfo = value;
		}
	}

	public EnTelegramType contactType { get; set; }

	public Guid Guid { get; set; } = Guid.NewGuid();


	public string ImageUrl { get; set; }

	public string ImageName { get; set; }

	public string VideoUrl { get; set; }

	public string VideoName { get; set; }

	public string FileUrl { get; set; }

	public string FileName { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now;


	public string Url { get; set; }

	public string UrlCaption { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public new bool IsDisposed { get; private set; }

	public event Func<object, SaveResponseEventArgs, Task> OnSaveResponse;

	public event Func<object, ConnectionEventArgs<WebTelegramMessage>, Task> OnSaveRequest;

	public WebTelegramMessage()
	{
	}

	public WebTelegramMessage(WebTelegramConfig config)
	{
		if (!string.IsNullOrEmpty(config?.ApiKey))
		{
			ApiKey = config.ApiKey;
			contactType = config.contactType;
			ContactInfo = config.ContactInfo;
		}
	}

	public static List<Packet> GenerateSaveRequest(IEnumerable<WebTelegramMessage> Messages)
	{
		using (new JaegerMonitor("GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTelegramMessage.cs"))
		{
			try
			{
				return Messages?.GeneratePackets(PacketType.TelegramMessage, 1, null, isSync: false, null, 0, deliveryRequst: true, "GenerateSaveRequest");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTelegramMessage.cs", 140);
				throw;
			}
		}
	}

	public static List<Packet> GenerateSaveResponse(IEnumerable<SaveResponse> responses)
	{
		using (new JaegerMonitor("GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTelegramMessage.cs"))
		{
			try
			{
				return responses?.GeneratePackets(PacketType.TelegramMessage, 2, null, isSync: false, null, 0, deliveryRequst: false, "GenerateSaveResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTelegramMessage.cs", 154);
				throw;
			}
		}
	}

	public static WebTelegramMessage GetNp_TaxApiServiceExceptions_bot()
	{
		return new WebTelegramMessage
		{
			ApiKey = "6956306684:AAGpJhNtZBhRGJvZGGtjt0pVjqnuYug4B8I",
			ContactInfo = "@TaxApiServiceExceptions",
			contactType = EnTelegramType.Chanel
		};
	}

	public static WebTelegramMessage GetNp_TaxApiServiceTrassitions_bot()
	{
		return new WebTelegramMessage
		{
			ApiKey = "6956306684:AAGpJhNtZBhRGJvZGGtjt0pVjqnuYug4B8I",
			ContactInfo = "@TaxApiServiceTrassitions",
			contactType = EnTelegramType.Chanel
		};
	}

	public static WebTelegramMessage GetNp_CallerIdService_bot()
	{
		return new WebTelegramMessage
		{
			ApiKey = "1792355228:AAE7f4iUQzHhLxzvNkq6Cbk90Pq7PVf0R9A",
			ContactInfo = "@CallerIdWindowsService",
			contactType = EnTelegramType.Chanel
		};
	}

	public static WebTelegramMessage GetWebServiceMonitoring_bot()
	{
		return new WebTelegramMessage
		{
			ApiKey = "1224264757:AAH4stzTZD3ay__VjLxsfXEgJnXIcX1kjOs",
			ContactInfo = "@WebSereviceMonitoring",
			contactType = EnTelegramType.Chanel
		};
	}

	public static WebTelegramMessage GetNp_AccountingImportFromHost_bot()
	{
		return new WebTelegramMessage
		{
			ApiKey = "1333465918:AAGx3GVMKVuZ-x72nDdNk_oOlaNiKtbDSCE",
			ContactInfo = "@NpAccountingImport",
			contactType = EnTelegramType.Chanel
		};
	}

	public static WebTelegramMessage Get_QueryTimedOut_bot()
	{
		return new WebTelegramMessage
		{
			ApiKey = "1322379690:AAFQI1NbJ1Oq4AXI5FEZnS4h3WTrxfgWkCM",
			ContactInfo = "@NovinPardazQueryTimeOut",
			contactType = EnTelegramType.Chanel
		};
	}

	public static WebTelegramMessage Get_LkOnlineoperations_bot()
	{
		return new WebTelegramMessage
		{
			ApiKey = "938348701:AAFE7JnZ3LrVG-n0sXf3E4YRvoDNj1QLitA",
			ContactInfo = "@LkOnlineoperations",
			contactType = EnTelegramType.Chanel
		};
	}

	public static WebTelegramMessage GetAttendanceRealTimeMonitoring_bot()
	{
		return new WebTelegramMessage
		{
			ApiKey = "1494333751:AAFuwUurVlHqU_00BAPyD17AgKFKlupJxQU",
			ContactInfo = "@AttendanceRealTimeMonitoring",
			contactType = EnTelegramType.Chanel
		};
	}

	public static WebTelegramMessage GetNovinFestival_bot()
	{
		return new WebTelegramMessage
		{
			ApiKey = "6770112059:AAG4fndow6ovuN-7pJ2zlaqFpYcDak0fCcM",
			ContactInfo = "@NovinFestival",
			contactType = EnTelegramType.Chanel
		};
	}

	public static WebTelegramMessage SupportBugTracker()
	{
		return new WebTelegramMessage
		{
			ApiKey = "8453358913:AAEW4-C6IqvTHWL10Z0KK8Vm8zfrfeCy2eQ",
			ContactInfo = "@NPSupportBugTracker",
			contactType = EnTelegramType.Chanel
		};
	}

	public static WebTelegramMessage ContactUs_bot()
	{
		return new WebTelegramMessage
		{
			ApiKey = "938348701:AAFE7JnZ3LrVG-n0sXf3E4YRvoDNj1QLitA",
			ContactInfo = "@NpAdvGrp",
			contactType = EnTelegramType.Chanel
		};
	}

	public static WebTelegramMessage GetNp_ErrorHanlerUnhandled_bot()
	{
		return new WebTelegramMessage
		{
			ApiKey = "7083686310:AAHDa-J1QeZHELxdfNYLrK5oSEBEeXfrl8w",
			ContactInfo = "@Np_ErrorHanlerUnhandled",
			contactType = EnTelegramType.Chanel
		};
	}

	public static WebTelegramMessage GetNp_Advertise_bot()
	{
		return new WebTelegramMessage
		{
			ApiKey = "1347185373:AAFEDsnuiDNfvIrC9bCkIZzfTGDd5OigzLY",
			ContactInfo = "@Np_Advertise",
			contactType = EnTelegramType.Chanel
		};
	}

	public static WebTelegramMessage GetNp_WordpressSupportDepartment_bot()
	{
		return new WebTelegramMessage
		{
			ApiKey = "1347185373:AAFEDsnuiDNfvIrC9bCkIZzfTGDd5OigzLY",
			ContactInfo = "@NPWordPressErrorReporting",
			contactType = EnTelegramType.Chanel
		};
	}

	public static WebTelegramMessage GetNp_QueueAbandonCallings_bot()
	{
		return new WebTelegramMessage
		{
			ApiKey = "1169869009:AAH2wBO6N0zsWr45bDCdprVUCI2L-YVINS8",
			ContactInfo = "@Novin_CanceledCalls",
			contactType = EnTelegramType.Chanel
		};
	}

	public static WebTelegramMessage GetNp_SupportCallings_bot()
	{
		return new WebTelegramMessage
		{
			ApiKey = "1169869009:AAH2wBO6N0zsWr45bDCdprVUCI2L-YVINS8",
			ContactInfo = "@NovinPardazCallings",
			contactType = EnTelegramType.Chanel
		};
	}

	public static WebTelegramMessage GetNp_AdministratorCallCenterAlarm_bot()
	{
		return new WebTelegramMessage
		{
			ApiKey = "1169869009:AAH2wBO6N0zsWr45bDCdprVUCI2L-YVINS8",
			ContactInfo = "@NovinPardaz_IssabelStatus",
			contactType = EnTelegramType.Chanel
		};
	}

	public static WebTelegramMessage GetNp_SupportCallCenterAlarm_bot()
	{
		return new WebTelegramMessage
		{
			ApiKey = "1169869009:AAH2wBO6N0zsWr45bDCdprVUCI2L-YVINS8",
			ContactInfo = "@Novin_CallCenter_Alarm",
			contactType = EnTelegramType.Chanel
		};
	}

	public static WebTelegramMessage GetNp_CallCenter_bot()
	{
		return new WebTelegramMessage
		{
			ApiKey = "1347185373:AAFEDsnuiDNfvIrC9bCkIZzfTGDd5OigzLY",
			ContactInfo = "@NPCallCenter",
			contactType = EnTelegramType.Chanel
		};
	}

	public static WebTelegramMessage GetNp_WordpressLogInforamtions()
	{
		checked
		{
			_index_GetNp_WordpressLogInforamtions++;
			if (_index_GetNp_WordpressLogInforamtions == 1)
			{
				return new WebTelegramMessage
				{
					ApiKey = "6963853358:AAEJP5sPLW3eKaCX0HoUZ_FeiZaMtlcAPSc",
					ContactInfo = "@WordPressException",
					contactType = EnTelegramType.Chanel
				};
			}
			if (_index_GetNp_WordpressLogInforamtions == 2)
			{
				return new WebTelegramMessage
				{
					ApiKey = "6476629030:AAF_xObF6wtlGQwjyTFyP0gBAPKJnfe6LCk",
					ContactInfo = "@WordPressException",
					contactType = EnTelegramType.Chanel
				};
			}
			if (_index_GetNp_WordpressLogInforamtions == 3)
			{
				return new WebTelegramMessage
				{
					ApiKey = "6861366526:AAFiLnmdaR6FxyubRZL7oixMyL14lTJvWro",
					ContactInfo = "@WordPressException",
					contactType = EnTelegramType.Chanel
				};
			}
			if (_index_GetNp_WordpressLogInforamtions >= 4)
			{
				_index_GetNp_WordpressLogInforamtions = 0;
			}
			return new WebTelegramMessage
			{
				ApiKey = "6403936776:AAF0iRepxJ2yXVHmCqlYSN0hRFIw4TUYYZw",
				ContactInfo = "@WordPressException",
				contactType = EnTelegramType.Chanel
			};
		}
	}

	public static WebTelegramMessage GetHotelParnian_bot()
	{
		return new WebTelegramMessage
		{
			ApiKey = "1347185373:AAFEDsnuiDNfvIrC9bCkIZzfTGDd5OigzLY",
			ContactInfo = "@abrsaDevice",
			contactType = EnTelegramType.Chanel
		};
	}

	public static WebTelegramMessage GetHotelAsa_bot()
	{
		return new WebTelegramMessage
		{
			ApiKey = "1347185373:AAFEDsnuiDNfvIrC9bCkIZzfTGDd5OigzLY",
			ContactInfo = "@AsaRealTimeAttendace",
			contactType = EnTelegramType.Chanel
		};
	}

	public static WebTelegramMessage GetNp_Ticket_bot()
	{
		return new WebTelegramMessage
		{
			ApiKey = "1287161382:AAENDZpMMcNC2EIRZlHYdnd2U2KRFcvgFqo",
			ContactInfo = "@NpTicketReporter",
			contactType = EnTelegramType.Chanel
		};
	}

	public static WebTelegramMessage GetNp_RestApi_bot(string telegramUrl = "", string telegramUrlCaption = "")
	{
		return new WebTelegramMessage
		{
			ApiKey = "1389482892:AAGFIbG66qmWZzm7SHnNbD88eyyh_945IVY",
			ContactInfo = "@NPSupporterErrorReport",
			contactType = EnTelegramType.Chanel,
			Url = telegramUrl,
			UrlCaption = telegramUrlCaption
		};
	}

	public static WebTelegramMessage GetNp_Supporter_bot(string telegramUrl = "", string telegramUrlCaption = "")
	{
		return new WebTelegramMessage
		{
			ApiKey = "1389482892:AAGFIbG66qmWZzm7SHnNbD88eyyh_945IVY",
			ContactInfo = "@NPSupporterErrorReport",
			contactType = EnTelegramType.Chanel,
			Url = telegramUrl,
			UrlCaption = telegramUrlCaption
		};
	}

	public static WebTelegramMessage GetNp_ServerMessaging_bot(string telegramUrl = "", string telegramUrlCaption = "")
	{
		return new WebTelegramMessage
		{
			ApiKey = "6854371186:AAFJMMXPv9iME-FKNgsY9h80BpFktemiKPo",
			ContactInfo = "@NPServerMessagingReport",
			contactType = EnTelegramType.Chanel,
			Url = telegramUrl,
			UrlCaption = telegramUrlCaption
		};
	}

	public static WebTelegramMessage GetNp_GpsLog_bot(string telegramUrl = "", string telegramUrlCaption = "")
	{
		return new WebTelegramMessage
		{
			ApiKey = "",
			ContactInfo = "@NPGpsLogReport",
			contactType = EnTelegramType.Chanel,
			Url = telegramUrl,
			UrlCaption = telegramUrlCaption
		};
	}

	public static WebTelegramMessage GetNp_ErrorHandler_bot()
	{
		return new WebTelegramMessage
		{
			ApiKey = "6768557103:AAEiWqSEjN-FraUYQRuMKZUCYQJlpyg02VA",
			ContactInfo = "@NPErrorHandler",
			contactType = EnTelegramType.Chanel
		};
	}

	public static WebTelegramMessage GetNp_ErrorHandler2_bot()
	{
		checked
		{
			_index_GetNp_ErrorHandler2_bot++;
			if (_index_GetNp_WordpressLogInforamtions == 1)
			{
				return new WebTelegramMessage
				{
					ApiKey = "6768557103:AAEiWqSEjN-FraUYQRuMKZUCYQJlpyg02VA",
					ContactInfo = "@NPErrorHandler2",
					contactType = EnTelegramType.Chanel
				};
			}
			if (_index_GetNp_ErrorHandler2_bot == 2)
			{
				return new WebTelegramMessage
				{
					ApiKey = "6890150387:AAEmXrLmFMjgdoj4-f3OGowwdzcGv7Or-uw",
					ContactInfo = "@NPErrorHandler2",
					contactType = EnTelegramType.Chanel
				};
			}
			if (_index_GetNp_ErrorHandler2_bot == 3)
			{
				return new WebTelegramMessage
				{
					ApiKey = "6857900868:AAGI-To06iBGakdRaRnUv9i7nWlEYWZ7u6k",
					ContactInfo = "@NPErrorHandler2",
					contactType = EnTelegramType.Chanel
				};
			}
			if (_index_GetNp_ErrorHandler2_bot == 4)
			{
				return new WebTelegramMessage
				{
					ApiKey = "6353329970:AAHS60YgV_fC5nfkUWrkPL0fJv5z5-q8Cq0",
					ContactInfo = "@NPErrorHandler2",
					contactType = EnTelegramType.Chanel
				};
			}
			if (_index_GetNp_ErrorHandler2_bot == 5)
			{
				return new WebTelegramMessage
				{
					ApiKey = "6924437999:AAHbW3Rc6HGmZczaaeuEGQk129cf73VxP3E",
					ContactInfo = "@NPErrorHandler2",
					contactType = EnTelegramType.Chanel
				};
			}
			if (_index_GetNp_ErrorHandler2_bot == 6)
			{
				return new WebTelegramMessage
				{
					ApiKey = "6845082708:AAHRU1BZ_GpErRJpV-Kndmqccm4XNSwV8j4",
					ContactInfo = "@NPErrorHandler2",
					contactType = EnTelegramType.Chanel
				};
			}
			if (_index_GetNp_ErrorHandler2_bot == 7)
			{
				return new WebTelegramMessage
				{
					ApiKey = "6873125829:AAGInn6NRdjRpOrBYjZ_dc6kBGDVxWrHwd0",
					ContactInfo = "@NPErrorHandler2",
					contactType = EnTelegramType.Chanel
				};
			}
			if (_index_GetNp_ErrorHandler2_bot >= 7)
			{
				_index_GetNp_ErrorHandler2_bot = 0;
			}
			return new WebTelegramMessage
			{
				ApiKey = "6522808510:AAGlXyxidXYeemIMhxz5Gy3Vl4PfjSKL55M",
				ContactInfo = "@NPErrorHandler2",
				contactType = EnTelegramType.Chanel
			};
		}
	}

	public static WebTelegramMessage GetNp_TimedOutTraces_bot()
	{
		return new WebTelegramMessage
		{
			ApiKey = "1620071708:AAF4HCm2fhpZncvC2YfqZjka3uMvKymNd3w",
			ContactInfo = "@NPPerfMonitor",
			contactType = EnTelegramType.Chanel
		};
	}

	public static WebTelegramMessage GetNp_SelfLog_bot()
	{
		return new WebTelegramMessage
		{
			ApiKey = "6438134642:AAFcXsFcmrIgbtVcfjkuDj-QEK4ZZzGD5sc",
			ContactInfo = "@NPSelfLog_chanel",
			contactType = EnTelegramType.Chanel
		};
	}

	public static WebTelegramMessage GetNp_UnseenErrors_bot()
	{
		return new WebTelegramMessage
		{
			ApiKey = "6898065300:AAFJ8F0FjLVAGQz0d0ihSFViyIxqMQyRs7A",
			ContactInfo = "@NPUnSeenErrors",
			contactType = EnTelegramType.Chanel
		};
	}

	public static WebTelegramMessage GetNp_WordpressAggregate_bot()
	{
		return new WebTelegramMessage
		{
			ApiKey = "1389482892:AAGFIbG66qmWZzm7SHnNbD88eyyh_945IVY",
			ContactInfo = "@NPWordPressAggregateMessage",
			contactType = EnTelegramType.Chanel
		};
	}

	public static WebTelegramMessage GetNp_NovinPardaz_Android_sms_Parser_bot()
	{
		return new WebTelegramMessage
		{
			ApiKey = "938348701:AAFE7JnZ3LrVG-n0sXf3E4YRvoDNj1QLitA",
			ContactInfo = "@MobileSmsParser",
			contactType = EnTelegramType.Chanel
		};
	}

	public static WebTelegramMessage GetNP_AutorizationReport()
	{
		return new WebTelegramMessage
		{
			ApiKey = "6964041747:AAGZ8aQ1U38FpymuJsBhhJ_Tm0EqVdtS_iE",
			ContactInfo = "@NovinRestAPI",
			contactType = EnTelegramType.Chanel
		};
	}

	public static WebTelegramMessage GetNP_CSR_Channel()
	{
		return new WebTelegramMessage
		{
			ApiKey = "6469468902:AAHcJ8inpAcIQvUt3j6_WVqPzrk-_1Afy5g",
			ContactInfo = "@NP_CSR_Channel",
			contactType = EnTelegramType.Chanel
		};
	}

	public static WebTelegramMessage GetNp_novinPardaz_error_rsfi_bot()
	{
		return new WebTelegramMessage
		{
			ApiKey = "938348701:AAFE7JnZ3LrVG-n0sXf3E4YRvoDNj1QLitA",
			ContactInfo = "@novinPardaz_error_rsfi",
			contactType = EnTelegramType.Chanel
		};
	}

	public static WebTelegramMessage NpSmsReporter1()
	{
		return new WebTelegramMessage
		{
			ApiKey = "938348701:AAFE7JnZ3LrVG-n0sXf3E4YRvoDNj1QLitA",
			ContactInfo = "@NpSmsReporter",
			contactType = EnTelegramType.Chanel
		};
	}

	public static WebTelegramMessage NpProductAmountReport()
	{
		return new WebTelegramMessage
		{
			ApiKey = "1620071708:AAF4HCm2fhpZncvC2YfqZjka3uMvKymNd3w",
			ContactInfo = "@NpProductAmountReport",
			contactType = EnTelegramType.Chanel
		};
	}

	public static WebTelegramMessage NpVersionReport()
	{
		return new WebTelegramMessage
		{
			ApiKey = "6914623289:AAGI3x2PLLK1YyK2LEUwAm_3MYYkrGx7lEo",
			ContactInfo = "@PendingTasks",
			contactType = EnTelegramType.Chanel
		};
	}

	public static WebTelegramMessage NpVersionUsageReport()
	{
		return new WebTelegramMessage
		{
			ApiKey = "938348701:AAFE7JnZ3LrVG-n0sXf3E4YRvoDNj1QLitA",
			ContactInfo = "@NpVersionUsage",
			contactType = EnTelegramType.Chanel
		};
	}

	private static async Task SendData2ServerAsync(short maxParalelTasks = 50, CancellationToken tkn1 = default(CancellationToken))
	{
		int line = 0;
		try
		{
			line = 1;
			line = 2;
			while (true)
			{
				List<Task<string>> taskList = new List<Task<string>>();
				line = 3;
				line = 4;
				WebTelegramMessage webTelegramMessage = await _bufferTelegramMessages.ReceiveAsync();
				line = 5;
				_tokens.TryRemove(webTelegramMessage.Guid, out var value);
				line = 6;
				value?.Cancel();
				line = 7;
				value = CancellationTokenSource.CreateLinkedTokenSource(tkn1, new CancellationTokenSource(60000).Token);
				CancellationToken token = value.Token;
				line = 8;
				if (!token.IsCancellationRequested)
				{
					_tokens.TryAdd(webTelegramMessage.Guid, value);
					line = 9;
					taskList.Add(webTelegramMessage.Sent2webAsync_(token));
					line = 10;
					if (taskList.Count > maxParalelTasks)
					{
						line = 11;
						await Task.WhenAll(taskList);
						line = 12;
						taskList.Clear();
						line = 16;
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "SendData2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTelegramMessage.cs", 769);
		}
	}

	public void Sent2web(string message = "", Guid? guid = null, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		try
		{
			if (string.IsNullOrEmpty(ApiKey))
			{
				return;
			}
			if (_sendData2TelegramServer == null)
			{
				_sendData2TelegramServer = Task.Run(() => SendData2ServerAsync(50));
			}
			if (!string.IsNullOrEmpty(message))
			{
				Message = CreateMessage(message, sourceFilePath, memberName, sourceLineNumber);
			}
			if (guid.HasValue)
			{
				Guid = guid.Value;
			}
			Message = Message.UnicodeToCorrectString();
			if (_bufferTelegramMessages.Count > 5000)
			{
				int count = _bufferTelegramMessages.Count;
				_bufferTelegramMessages.TryReceiveAll(out IList<WebTelegramMessage> _);
				throw new Exception($"Count({count}) of telegram messages exceeded!");
			}
			if (_tokens.Count > 1000)
			{
				_tokens.Clear();
			}
			_bufferTelegramMessages.Post(this);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Sent2web", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTelegramMessage.cs", 799);
		}
	}

	public string CreateMessage(string message, string sourceFilePath, string memberName, int sourceLineNumber)
	{
		try
		{
			return string.Concat(string.Concat(string.Concat("className:#" + GetClassName(sourceFilePath) + "\r\n", "FuncName:#", memberName, "\r\n"), $"LineNumber:{sourceLineNumber}\r\n"), "Message:", message);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CreateMessage", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTelegramMessage.cs", 835);
			return "error in parse Data check error logger";
		}
	}

	private string GetClassName(string memberName)
	{
		try
		{
			if (string.IsNullOrEmpty(memberName))
			{
				return "";
			}
			FileInfo fileInfo = new FileInfo(memberName);
			if (string.IsNullOrEmpty(fileInfo.Extension))
			{
				return fileInfo.Name;
			}
			return fileInfo.Name.Replace(fileInfo.Extension, "");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, memberName, "GetClassName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTelegramMessage.cs", 852);
			return memberName;
		}
	}

	[DebuggerHidden]
	private async Task<string> Sent2webAsync_(CancellationToken token, int timeOut = 60000)
	{
		await Task.Delay(100);
		if (token.IsCancellationRequested)
		{
			return "token cancelled!";
		}
		try
		{
			if (string.IsNullOrEmpty(Message))
			{
				return "no data for send!";
			}
			if (Environment.OSVersion.Platform == PlatformID.Win32NT && WindowsPlatform != null)
			{
				message = message + "\r\nTelegramSenderHostName: " + WindowsPlatform.GetUsername();
			}
			if (!string.IsNullOrEmpty(UserInfo))
			{
				message = message + "\r\nUserInfo:#" + UserInfo;
			}
			SemaphoreSlim semaphore = await _sendLock.GetSemaphoreAsync($"{Guid}");
			bool semaphorHasbeenTaken = false;
			try
			{
				using HttpClient client = new HttpClient();
				Dictionary<string, string> nameValueCollection = new Dictionary<string, string> { 
				{
					"message",
					ToString().ToBase64()
				} };
				FormUrlEncodedContent content = new FormUrlEncodedContent(nameValueCollection);
				Message = $"{Message}\r\n{Guid}";
				if (token.IsCancellationRequested)
				{
					return "token cancelled!";
				}
				semaphorHasbeenTaken = await semaphore.WaitAsync(new TimeSpan(0, 0, 0, 0, timeOut), token);
				if (token.IsCancellationRequested)
				{
					return "token cancelled!";
				}
				HttpResponseMessage httpResponseMessage = await client.PostAsync("http://www.WebService.npmail.ir/Telegram/SendMessage/", content, new CancellationTokenSource(60000).Token);
				if (token.IsCancellationRequested)
				{
					return "token cancelled!";
				}
				return await httpResponseMessage.Content.ReadAsStringAsync();
			}
			finally
			{
				if (semaphorHasbeenTaken)
				{
					semaphore?.Release();
				}
			}
		}
		catch (HttpRequestException)
		{
			return "error!";
		}
		catch (OperationCanceledException)
		{
			return "Task Cancelled!";
		}
		catch (Exception ex3)
		{
			if (!ex3.Message.ToLower().Contains("An error occurred while sending the request".ToLower()))
			{
				WebErrorLog.Instance.StartLog(ex3, "", "Sent2webAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTelegramMessage.cs", 910);
			}
			return ex3.Message;
		}
	}

	public string GetPacketActionEnumName(short action)
	{
		try
		{
			TelegramMessageActions telegramMessageActions = (TelegramMessageActions)action;
			return telegramMessageActions.ToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionEnumName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTelegramMessage.cs", 923);
			return $"Error({action})";
		}
	}

	public string GetPacketActionName(short action)
	{
		try
		{
			return ((TelegramMessageActions)action).GetDisplay();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTelegramMessage.cs", 936);
			return $"Error({action})";
		}
	}

	public async Task DataReceived(Packet pack, string ConnectionUniqueId, ConnectionData connection)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTelegramMessage.cs"))
		{
			try
			{
				switch ((TelegramMessageActions)pack.PacketActions)
				{
				case TelegramMessageActions.SaveRequest:
					if (this.OnSaveRequest != null)
					{
						new ConnectionEventArgs<WebTelegramMessage>(pack, ConnectionUniqueId, connection);
					}
					break;
				case TelegramMessageActions.SaveResponse:
					if (this.OnSaveResponse != null)
					{
						SaveResponseEventArgs args = new SaveResponseEventArgs(pack, ConnectionUniqueId, connection);
						await Utilities.RaiseEvent(this.OnSaveResponse, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTelegramMessage.cs");
					}
					break;
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTelegramMessage.cs", 967);
			}
		}
	}

	public new void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				if (this.OnSaveRequest != null)
				{
					this.OnSaveRequest = null;
				}
				if (this.OnSaveResponse != null)
				{
					this.OnSaveResponse = null;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTelegramMessage.cs", 981);
		}
	}
}
