using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using PacketParser.Entities.CRM;
using PacketParser.Entities.Department;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.WordPress;
using PacketParser.EventArgs;
using PacketParser.LogPacket;
using Serilog;

namespace PacketParser;

public class Packet : Serializable<Packet>, IDisposable, INpDisposable, IReceiver
{
	private enum PacketAction
	{
		Unknown,
		Response
	}

	private bool _isDisposed;

	private Guid _packetReferenceId = Guid.Empty;

	private Guid packetUniqueId = Guid.NewGuid();

	private bool _deliveryRequested = true;

	private byte[] _packetContents;

	private JaegerMonitor _monitor;

	private DateTime _Created = DateTime.Now;

	private TimeSpan _CreationLimit = new TimeSpan(0, 0, 60);

	public static ExpiredPacketLogger _logger = new ExpiredPacketLogger(300000);

	public string ExteraDataForDebug { get; set; }

	public int PartitionId { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public bool? SendResult { get; set; }

	public PacketType PacketType { get; set; }

	public byte[] PacketContents
	{
		get
		{
			return _packetContents;
		}
		set
		{
			_packetContents = value;
		}
	}

	public short Part { get; set; }

	public short TotalParts { get; set; } = 1;


	public Guid PacketReferenceId
	{
		get
		{
			if (_packetReferenceId == Guid.Empty)
			{
				_packetReferenceId = Guid.NewGuid();
			}
			return _packetReferenceId;
		}
		set
		{
			_packetReferenceId = value;
		}
	}

	public Guid PacketUniqueId
	{
		get
		{
			if (packetUniqueId == Guid.Empty)
			{
				packetUniqueId = Guid.NewGuid();
			}
			return packetUniqueId;
		}
		set
		{
			packetUniqueId = value;
		}
	}

	public short PacketActions { get; set; }

	public bool IsSynced { get; set; }

	public string[] Args { get; set; }

	[DefaultValue(true)]
	public bool DeliveryRequested
	{
		get
		{
			if (PacketType != PacketType.Packet)
			{
				return _deliveryRequested;
			}
			return false;
		}
		set
		{
			_deliveryRequested = value;
		}
	}

	[XmlIgnore]
	[JsonIgnore]
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

	public event Func<object, BaseEventArgs, Task> OnResponse;

	public Packet()
	{
	}

	private Packet(PacketType packetType, byte[] packetContents, string[] args)
		: this(packetType, short.Parse(args[0]), packetContents, args, 0, 1, Guid.NewGuid())
	{
	}

	public Packet(PacketType packetType, short packetActions, byte[] packetContents, string[] args, short part, short totalParts, Guid packetReferenceId, bool isSynced = false, int partitionId = 0, string extraDataForDebug = "", int lifespanLimit = 30000)
	{
		using (new JaegerMonitor(".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Packet.cs"))
		{
			try
			{
				PacketType = packetType;
				PacketContents = packetContents;
				Args = args;
				Part = part;
				PacketReferenceId = packetReferenceId;
				TotalParts = totalParts;
				PacketActions = packetActions;
				IsSynced = isSynced;
				PartitionId = partitionId;
				ExteraDataForDebug = extraDataForDebug;
				if (lifespanLimit < 300000)
				{
					lifespanLimit = 300000;
				}
				_CreationLimit = TimeSpan.FromMilliseconds(lifespanLimit);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Packet.cs", 155);
			}
		}
	}

	public static Packet GetPacket(byte[] bytes, StringBuilder sb)
	{
		using (new JaegerMonitor("GetPacket", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Packet.cs"))
		{
			try
			{
				sb.AppendLine("GetPacket...");
				sb.AppendLine($"DataLen:{bytes?.Length}");
				return Json.FromJson<Packet>(bytes, sendErrorIfException: false, "GetPacket", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Packet.cs", 165);
			}
			catch (Exception ex)
			{
				sb.AppendLine("❌Exception:" + ex.Message);
				WebErrorLog.Instance.StartLog(ex, "", "GetPacket", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Packet.cs", 170);
				return null;
			}
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<Packet>> ParsePacket(WebContextHolder context, byte[] bytes, string ConnectionUniqueId, ConnectionData connection, StringBuilder sb)
	{
		ReturnedSaveFuncInfoWithValue<Packet> ret = new ReturnedSaveFuncInfoWithValue<Packet>();
		Stopwatch sw = Stopwatch.StartNew();
		try
		{
			sb.AppendLine("ParsePacketFromBytes...");
			if (bytes == null || bytes.Length == 0)
			{
				Log.Warning("Received empty packet");
				return ret.AddError(EnSystem.SignalR, "bytes is empty!!", "ParsePacket", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Packet.cs", 186);
			}
			Packet packet = GetPacket(bytes, sb);
			if (packet.PacketType == PacketType.Packet && packet.PacketActions == 0)
			{
				throw new Exception($"Data is invalid packet.PacketType:{packet.PacketType}//packet.PacketActions:{packet.PacketActions}");
			}
			if (packet == null)
			{
				return ret.AddError(EnSystem.SignalR, "packet is null!!", "ParsePacket", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Packet.cs", 192);
			}
			if (connection.cancellationToken.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "ParsePacket", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Packet.cs", 193);
			}
			await ParsePacket(context, packet, ConnectionUniqueId, connection, sb);
			ret.value = packet;
			int num = 60000;
			num = checked(num * 5);
			if (sw.ElapsedMilliseconds > num)
			{
				throw new Exception($"زمان پردازش بسته {sw.ElapsedMilliseconds}(ms)\r\n" + $"PacketType:{packet.PacketType}({packet.PacketType.GetDisplay()})\r\n" + $"PacketAction:{packet.PacketActions}({packet.GetPacketActionName()})\r\n" + "بیش از حد مجاز شده است.");
			}
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			sb.AppendLine("❌Exception:" + ex2.Message);
			ret.AddReturnedValue(ex2, "", "ParsePacket", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Packet.cs", 210);
			WebErrorLog.Instance.StartLog(ex2, "", "ParsePacket", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Packet.cs", 211);
		}
		return ret;
	}

	public static async Task ParsePacket(WebContextHolder context, Packet packet, string ConnectionUniqueId, ConnectionData connection, StringBuilder sb)
	{
		int line = 0;
		using (new JaegerMonitor("ParsePacket", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Packet.cs"))
		{
			_ = 64;
			try
			{
				sb.AppendLine("ParsePacket2....");
				sb.AppendLine($"PacketType:{packet?.PacketType}\r\n" + $"PacketAction:{packet?.PacketActions}");
				_logger.GetOrAdd(packet, null, isReceivedPacket: true, packet.GetPacketActionName() + "-" + packet.GetPacketActionEnumName());
				switch (packet.PacketType)
				{
				case PacketType.Login:
					line = 1;
					Log.Information<PacketType>("login packet received. Type:{@packet_type}", packet.PacketType);
					await context.LoginExtention.DataReceived(packet, context, ConnectionUniqueId, connection);
					break;
				case PacketType.Reception:
					line = 2;
					await context.WebReception.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.Pardakht:
					line = 3;
					await context.WebPardakht.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.PishFactor:
					line = 4;
					await context.WebOrder.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.Product:
					line = 5;
					await context.WebProduct.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.ProductGroup:
					line = 6;
					await context.WebPrdGroup.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.ErrorHandleSum:
					line = 7;
					await context.WebErrorLogSum.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.File:
					line = 8;
					await context.WebFileInfo.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.Customer:
					line = 8;
					await context.WebCus.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.CustomerGroup:
					line = 9;
					await context.WebCusGroup.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.CustomerTitle:
					line = 10;
					await context.WebCusTitle.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.CustomerAddress:
					await context.WebCustomerAddress.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.HesabTree:
					line = 11;
					await context.WebHesabTree.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.Depot:
					line = 12;
					await context.WebDepot.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.GPS:
					line = 13;
					await context.WebGPS.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.ApplicationSetting:
					await context.WebApplicationSetting.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.ProductVahed:
					line = 14;
					await context.WebPrdVahed.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.Sync:
					line = 15;
					sb.AppendLine($"Time than PacketParsed:{DateTime.Now}");
					await context.Sync.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.Delivery:
					line = 16;
					await context.WebDelivery.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.News:
					line = 17;
					await context.WebNews.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.HyperLink:
					line = 18;
					await context.WebHyperLink.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.UserAcc:
					line = 19;
					await context.WebUserAcc.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.TelegramMessage:
					line = 20;
					await context.WebTelegramMessage.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.ContactUs:
					line = 21;
					await context.WebContactUs.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.Packet:
					line = 22;
					await context.Packet.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.ExecuteQuery:
					line = 23;
					await context.WebExecuteQuery.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.Desc:
					line = 27;
					await context.WebDesc.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.Visit:
					line = 28;
					await context.WebVisit.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.AghsatShakhsi:
					line = 29;
					await context.WebAghsatShakhsi.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.AghsatAshkhas:
					await context.WebAghsatAshkhas.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.ViewCheckM:
					line = 30;
					await context.WebViewCheckM.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.ViewCheckSh:
					line = 31;
					await context.WebViewCheckSh.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.TafsilGroup:
					line = 32;
					await context.WebTafsilGroup.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.CallingLog:
					line = 33;
					await context.WebCallingLog.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.Modem:
					line = 34;
					await context.WebModem.DataReceived(packet, ConnectionUniqueId, connection, isTcp: false);
					break;
				case PacketType.LastPrice:
					line = 35;
					await context.WebLastPrice.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.AttendanceLogOnPersonel:
					line = 35;
					await context.WebAttendanceLogOnPerson.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.Kol:
					line = 36;
					await context.WebKol.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.Moein:
					line = 37;
					await context.WebMoein.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.Tafsil:
					line = 38;
					await context.WebTafsil.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.UserLog:
					line = 38;
					await context.WebUserLog.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.LkInfo:
					line = 38;
					await context.WebLkInfo.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.CheckMOperation:
					line = 38;
					await context.WebChequeMOperation.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.CheckShOperation:
					line = 38;
					await context.WebChequeShOperation.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.Sanad:
					line = 38;
					await context.WebSanad.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.PrdAnbarGardani:
					line = 380;
					await context.WebPrdAnbarGardani.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.PrdSerial:
					line = 380;
					await context.WebPrdSerial.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.IntroductionMethod:
					await context.WebIntroductionMethod.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.EntitySync:
					await context.WebEntitySync.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.Account:
					line = 37;
					await context.WebAccount.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.CustomerRole:
					await context.CustomerRole.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.Note:
					await context.Note.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.TaxLogin:
					await context.TaxLogin.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.TaxSendOrder:
				case PacketType.TaxTrackingOrder:
				case PacketType.TaxEconomicCodeState:
					await context.WebOrder.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.AttPersonLeave:
					await context.AttPersonLeave.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.DiscountCoupon:
					await context.DiscoupntCoupon.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.AttDepartment:
					await context.AttDepartment.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.Tax:
					await context.Tax.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.TaxOrderLog:
					await context.TaxOrderLog.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.AghsatMGroup:
					await context.AghsatMGroup.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.ProductPriceLevel:
					await context.ProductPriceLevel.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.SanadStatusReport:
					await context.SanadStatusReport.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.SanadPrint:
					await context.SanadPrint.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.AttendanceDeviceLogOnPersonel:
					await context.DeviceLogOnPerson.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				case PacketType.BioMetric:
					await context.BioMetricWriteDevice.DataReceived(packet, ConnectionUniqueId, connection);
					break;
				default:
					line = 39;
					Log.Warning<Packet>("UnRecognized Data Recived: {@packet}", packet);
					break;
				}
				packet?.Dispose();
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"The Error Line IS : {line}//line=34 is modem Packet//context.WebModem is null:{context?.WebModem == null}", "ParsePacket", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Packet.cs", 489);
			}
			finally
			{
				if (packet == null)
				{
					Log.Information<PacketType>("PacketParsed Null!!!", packet.PacketType);
				}
			}
		}
	}

	public WebLogPacket GenerateLogPacket(int DepId, long HashId, WebContextHolder context, string ConnectionUniqueId)
	{
		return new WebLogPacket
		{
			ConnectionUniqueId = ConnectionUniqueId,
			Date = DateTime.Now,
			DepId = DepId,
			HashId = HashId,
			PacketType = checked((short)PacketType),
			PacketAction = PacketActions,
			PacketRefrenced = PacketReferenceId.ToString(),
			Context = context,
			PacketLengh = ToBytes().Length,
			ExteraDataForDebug = ExteraDataForDebug
		};
	}

	public Packet GenerateDeliveryResponse()
	{
		return new Packet(PacketType.Packet, 1, null, null, Part, TotalParts, PacketReferenceId);
	}

	public static async Task ParseLoginPacket(WebContextHolder context, byte[] bytes, string ConnectionUniqueId, ConnectionData connection, StringBuilder sb)
	{
		using (new JaegerMonitor("ParseLoginPacket", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Packet.cs"))
		{
			try
			{
				sb.AppendLine("ParseLoginPacket....");
				if (bytes == null || bytes.Length == 0)
				{
					return;
				}
				Packet temp = GetPacket(bytes, sb);
				if (temp == null)
				{
					sb.AppendLine("packet is null!!");
					return;
				}
				sb.AppendLine($"type:{temp.PacketType}\r\n" + $"Action:{temp.PacketActions}");
				if (temp.PacketType == PacketType.Login)
				{
					await context.LoginExtention.DataReceived(temp, context, ConnectionUniqueId, connection);
				}
				temp.Dispose();
				bytes = null;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ParseLoginPacket", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Packet.cs", 562);
			}
		}
	}

	public byte[] ToBytes(ENCompressedMode CompressMode = ENCompressedMode.None)
	{
		try
		{
			return Json.ToBytesJson(this);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ToBytes", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Packet.cs", 578);
			throw;
		}
	}

	public static void Initialize(object errorLogSum, object errorLog)
	{
		using (new JaegerMonitor("Initialize", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Packet.cs"))
		{
		}
	}

	public string PacketTextForDisplay()
	{
		string text = ToString();
		if (text.Length > 300)
		{
			text = text.Substring(0, 300);
		}
		return $"{DateTime.Now}>sent Size:{ToBytes().Length} type:{PacketType.ToString()} PacketContent:" + text;
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
				_monitor?.Dispose();
				_monitor = null;
				if (DateTime.Now - _Created > _CreationLimit)
				{
					WebTelegramMessage np_ErrorHanlerUnhandled_bot = WebTelegramMessage.GetNp_ErrorHanlerUnhandled_bot();
					np_ErrorHanlerUnhandled_bot.Message = $"#PacketType : {PacketType}\n" + $"#PacketAction : {PacketActions}\n" + "⏰ #ElapsedTime : " + (DateTime.Now - _Created).TotalMilliseconds.ToString("N0") + " (ms)\n" + $"#LimiTime : {_CreationLimit} (ms)\r\n" + $"Now : {DateTime.Now} \r\n" + $"CreationTime : {_Created}\r\n" + "#Description : " + ExteraDataForDebug;
					np_ErrorHanlerUnhandled_bot.Sent2web("", null, "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Packet.cs", 625);
				}
				if (this.OnResponse != null)
				{
					this.OnResponse = null;
				}
				_packetContents = null;
				Args = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Packet.cs", 634);
		}
	}

	public Task DataReceived(Packet pack, string ConnectionUniqueId, ConnectionData connection)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("DataReceived_" + (pack?.PacketType.ToString() ?? "Null"), "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Packet.cs"))
		{
			try
			{
				ConnectionData connectionData = connection;
				if (connectionData == null)
				{
					goto IL_0094;
				}
				_ = connectionData.cancellationToken;
				if (false)
				{
					goto IL_0094;
				}
				if (pack.PacketActions == 1)
				{
					Task.Run(async delegate
					{
						try
						{
							await Utilities.RaiseEvent(this.OnResponse, this, new BaseEventArgs
							{
								PacketReferenceId = pack.PacketReferenceId,
								Part = pack.Part,
								TotalParts = pack.TotalParts,
								ConnectionUniqueId = ConnectionUniqueId
							}, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Packet.cs");
						}
						catch (OperationCanceledException)
						{
						}
						catch (Exception ex4)
						{
							WebErrorLog.Instance.StartLog(ex4, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Packet.cs", 661);
						}
					});
				}
				goto end_IL_007e;
				IL_0094:
				return Task.CompletedTask;
				end_IL_007e:;
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Packet.cs", 669);
			}
			return Task.CompletedTask;
		}
	}

	public string GetPacketActionName(short action)
	{
		return GetPacketActionName(PacketType, PacketActions);
	}

	public string GetPacketActionEnumName()
	{
		return GetPacketActionEnumName(PacketType, PacketActions);
	}

	public string GetPacketActionName()
	{
		return GetPacketActionName(PacketType, PacketActions);
	}

	public string GetPacketActionEnumName(short action)
	{
		return GetPacketActionEnumName(PacketType, action);
	}

	public static string GetPacketActionEnumName(PacketType type, short action)
	{
		try
		{
			switch (type)
			{
			case PacketType.Packet:
				return action switch
				{
					0 => throw new Exception($"action zero is not valid. action:{action}"), 
					1 => PacketAction.Response.ToString(), 
					_ => throw new ArgumentOutOfRangeException(), 
				};
			case PacketType.Login:
				return new LoginExtention().GetPacketActionEnumName(action);
			case PacketType.Product:
				return new WebProduct().GetPacketActionEnumName(action);
			case PacketType.Customer:
				return new WebCus().GetPacketActionEnumName(action);
			case PacketType.PishFactor:
				return new WebOrder().GetPacketActionEnumName(action);
			case PacketType.Reception:
				return new WebReception().GetPacketActionEnumName(action);
			case PacketType.Pardakht:
				return new WebPardakht().GetPacketActionEnumName(action);
			case PacketType.ErrorHandleSum:
				return new WebErrorLogSum().GetPacketActionEnumName(action);
			case PacketType.File:
				return new WebFileInfo().GetPacketActionEnumName(action);
			case PacketType.ProductGroup:
				return new WebPrdGroup().GetPacketActionEnumName(action);
			case PacketType.CustomerGroup:
				return new WebCusGroup().GetPacketActionEnumName(action);
			case PacketType.CustomerTitle:
				return new WebCusTitle().GetPacketActionEnumName(action);
			case PacketType.HesabTree:
				return new WebHesabTree().GetPacketActionEnumName(action);
			case PacketType.Depot:
				return new WebDepot().GetPacketActionEnumName(action);
			case PacketType.GPS:
				return new WebGPS().GetPacketActionEnumName(action);
			case PacketType.PrdAnbarRelation:
				return new WebPrdAnbarRelation().GetPacketActionEnumName(action);
			case PacketType.ApplicationSetting:
				return new WebApplicationSetting().GetPacketActionEnumName(action);
			case PacketType.ProductVahed:
				return new WebPrdVahed().GetPacketActionEnumName(action);
			case PacketType.Sync:
				return new Sync().GetPacketActionEnumName(action);
			case PacketType.ContactUs:
				return new WebContactUs().GetPacketActionEnumName(action);
			case PacketType.News:
				return new WebNews().GetPacketActionEnumName(action);
			case PacketType.GardeshTafsili:
				return new WebGardeshTafsili().GetPacketActionEnumName(action);
			case PacketType.HyperLink:
				return new WebHyperLink().GetPacketActionEnumName(action);
			case PacketType.Notification:
				return new WebNotification().GetPacketActionEnumName(action);
			case PacketType.Send2Friend:
				return new WebSend2Friend().GetPacketActionEnumName(action);
			case PacketType.UserAcc:
				return new WebUserAcc().GetPacketActionEnumName(action);
			case PacketType.GardeshHesab:
				return new WebGardeshHesab().GetPacketActionEnumName(action);
			case PacketType.Delivery:
				return new WebDelivery().GetPacketActionEnumName(action);
			case PacketType.TelegramMessage:
				return new WebTelegramMessage().GetPacketActionEnumName(action);
			case PacketType.Script:
				return new WebScript().GetPacketActionEnumName(action);
			case PacketType.CustomerAddress:
				return new WebCustomerAddress().GetPacketActionEnumName(action);
			case PacketType.ExecuteQuery:
				return new WebExecuteQuery().GetPacketActionEnumName(action);
			case PacketType.Desc:
				return new WebDesc().GetPacketActionEnumName(action);
			case PacketType.AttendanceLogOnPersonel:
				return new WebAttendanceLogOnPerson().GetPacketActionEnumName(action);
			case PacketType.Visit:
				return new WebVisit().GetPacketActionEnumName(action);
			case PacketType.AghsatAshkhas:
				return new WebAghsatAshkhas().GetPacketActionEnumName(action);
			case PacketType.AghsatShakhsi:
				return new WebAghsatShakhsi().GetPacketActionEnumName(action);
			case PacketType.Modem:
				return new WebModem().GetPacketActionEnumName(action);
			case PacketType.ViewCheckM:
				return new WebViewCheckM().GetPacketActionEnumName(action);
			case PacketType.ViewCheckSh:
				return new WebViewCheckSh().GetPacketActionEnumName(action);
			case PacketType.TafsilGroup:
				return new WebTafsilGroup().GetPacketActionEnumName(action);
			case PacketType.CallingLog:
				return new WebCallingLog().GetPacketActionEnumName(action);
			case PacketType.LastPrice:
				return new WebLastPrice().GetPacketActionEnumName(action);
			case PacketType.AttendanceLogOnAdmin:
				return new WebAttendanceLogOnAdmin().GetPacketActionEnumName(action);
			case PacketType.AttendanceLogOnAlarm:
				return new WebAttendanceLogOnAlarm().GetPacketActionEnumName(action);
			case PacketType.Kol:
				return new WebKol().GetPacketActionEnumName(action);
			case PacketType.Moein:
				return new WebMoein().GetPacketActionEnumName(action);
			case PacketType.Tafsil:
				return new WebTafsil().GetPacketActionEnumName(action);
			case PacketType.UserLog:
				return new WebCashierLog().GetPacketActionEnumName(action);
			case PacketType.LkInfo:
				return new WebLkInfo().GetPacketActionEnumName(action);
			case PacketType.CheckMOperation:
				return new WebChequeMOperation().GetPacketActionName(action);
			case PacketType.CheckShOperation:
				return new WebChequeShOperation().GetPacketActionName(action);
			case PacketType.Sanad:
				return new WebSanad().GetPacketActionName(action);
			case PacketType.PrdAnbarGardani:
				return new WebPrdAnbarGardani().GetPacketActionName(action);
			case PacketType.PrdSerial:
				return new WebPrdSerial().GetPacketActionName(action);
			case PacketType.IntroductionMethod:
				return new WebIntroductionMethod().GetPacketActionName(action);
			case PacketType.EntitySync:
				return new WebEntitySync().GetPacketActionName(action);
			case PacketType.Account:
				return new WebAccount().GetPacketActionName(action);
			case PacketType.CustomerRole:
				return new WebCustomerRole().GetPacketActionName(action);
			case PacketType.Note:
				return new WebNote().GetPacketActionName(action);
			case PacketType.TaxLogin:
				return new WebTaxLogin().GetPacketActionName(action);
			case PacketType.TaxSendOrder:
			case PacketType.TaxTrackingOrder:
			case PacketType.TaxEconomicCodeState:
				return new WebOrder().GetPacketActionName(action);
			case PacketType.AttPersonLeave:
				return new WebAttPersonLeave().GetPacketActionName(action);
			case PacketType.DiscountCoupon:
				return new WebDiscountCoupon().GetPacketActionName(action);
			case PacketType.Tax:
				return new WebTax().GetPacketActionName(action);
			case PacketType.TaxOrderLog:
				return new WebTaxOrderLog().GetPacketActionName(action);
			case PacketType.AttDepartment:
				return new WebAttDepartment().GetPacketActionEnumName(action);
			case PacketType.AghsatMGroup:
				return new WebAghsatMGroup().GetPacketActionEnumName(action);
			case PacketType.ProductPriceLevel:
				return new WebProductPriceLevel().GetPacketActionEnumName(action);
			case PacketType.SanadStatusReport:
				return new WebSanadStatusReport().GetPacketActionEnumName(action);
			case PacketType.SanadPrint:
				return new WebSanadPrint().GetPacketActionEnumName(action);
			case PacketType.AttendanceDeviceLogOnPersonel:
				return new WebDeviceLogOnPerson().GetPacketActionEnumName(action);
			case PacketType.BioMetric:
				return new WebBioWriteDevice().GetPacketActionEnumName(action);
			case PacketType.WebProductAttribute:
				return "";
			default:
				throw new ArgumentOutOfRangeException($"type({checked((short)type)}):{type}//action:{action}");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionEnumName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Packet.cs", 811);
			return $"Error({action})";
		}
	}

	public static string GetPacketActionName(PacketType type, short action)
	{
		try
		{
			switch (type)
			{
			case PacketType.Packet:
				return action switch
				{
					0 => throw new Exception($"zero aztion is not valid. action:{action}"), 
					1 => PacketAction.Response.GetDisplay(), 
					_ => throw new ArgumentOutOfRangeException(), 
				};
			case PacketType.Login:
				return new LoginExtention().GetPacketActionName(action);
			case PacketType.Product:
				return new WebProduct().GetPacketActionName(action);
			case PacketType.Customer:
				return new WebCus().GetPacketActionName(action);
			case PacketType.PishFactor:
				return new WebOrder().GetPacketActionName(action);
			case PacketType.Reception:
				return new WebReception().GetPacketActionName(action);
			case PacketType.Pardakht:
				return new WebPardakht().GetPacketActionName(action);
			case PacketType.ErrorHandleSum:
				return new WebErrorLogSum().GetPacketActionName(action);
			case PacketType.File:
				return new WebFileInfo().GetPacketActionName(action);
			case PacketType.ProductGroup:
				return new WebPrdGroup().GetPacketActionName(action);
			case PacketType.CustomerGroup:
				return new WebCusGroup().GetPacketActionName(action);
			case PacketType.CustomerTitle:
				return new WebCusTitle().GetPacketActionName(action);
			case PacketType.HesabTree:
				return new WebHesabTree().GetPacketActionName(action);
			case PacketType.Depot:
				return new WebDepot().GetPacketActionName(action);
			case PacketType.GPS:
				return new WebGPS().GetPacketActionName(action);
			case PacketType.PrdAnbarRelation:
				return new WebPrdAnbarRelation().GetPacketActionName(action);
			case PacketType.ApplicationSetting:
				return new WebApplicationSetting().GetPacketActionName(action);
			case PacketType.ProductVahed:
				return new WebPrdVahed().GetPacketActionName(action);
			case PacketType.Sync:
				return new Sync().GetPacketActionName(action);
			case PacketType.ContactUs:
				return new WebContactUs().GetPacketActionName(action);
			case PacketType.News:
				return new WebNews().GetPacketActionName(action);
			case PacketType.GardeshTafsili:
				return new WebGardeshTafsili().GetPacketActionName(action);
			case PacketType.HyperLink:
				return new WebHyperLink().GetPacketActionName(action);
			case PacketType.Notification:
				return new WebNotification().GetPacketActionName(action);
			case PacketType.Send2Friend:
				return new WebSend2Friend().GetPacketActionName(action);
			case PacketType.UserAcc:
				return new WebUserAcc().GetPacketActionName(action);
			case PacketType.GardeshHesab:
				return new WebGardeshHesab().GetPacketActionName(action);
			case PacketType.Delivery:
				return new WebDelivery().GetPacketActionName(action);
			case PacketType.TelegramMessage:
				return new WebTelegramMessage().GetPacketActionName(action);
			case PacketType.Script:
				return new WebScript().GetPacketActionName(action);
			case PacketType.CustomerAddress:
				return new WebCustomerAddress().GetPacketActionName(action);
			case PacketType.ExecuteQuery:
				return new WebExecuteQuery().GetPacketActionName(action);
			case PacketType.Desc:
				return new WebDesc().GetPacketActionName(action);
			case PacketType.AttendanceLogOnPersonel:
				return new WebAttendanceLogOnPerson().GetPacketActionName(action);
			case PacketType.Visit:
				return new WebVisit().GetPacketActionName(action);
			case PacketType.AghsatAshkhas:
				return new WebAghsatAshkhas().GetPacketActionName(action);
			case PacketType.AghsatShakhsi:
				return new WebAghsatShakhsi().GetPacketActionName(action);
			case PacketType.Modem:
				return new WebModem().GetPacketActionName(action);
			case PacketType.ViewCheckM:
				return new WebViewCheckM().GetPacketActionName(action);
			case PacketType.ViewCheckSh:
				return new WebViewCheckSh().GetPacketActionName(action);
			case PacketType.TafsilGroup:
				return new WebTafsilGroup().GetPacketActionName(action);
			case PacketType.CallingLog:
				return new WebCallingLog().GetPacketActionName(action);
			case PacketType.LastPrice:
				return new WebLastPrice().GetPacketActionName(action);
			case PacketType.AttendanceLogOnAdmin:
				return new WebAttendanceLogOnAdmin().GetPacketActionName(action);
			case PacketType.AttendanceLogOnAlarm:
				return new WebAttendanceLogOnAlarm().GetPacketActionName(action);
			case PacketType.Kol:
				return new WebKol().GetPacketActionName(action);
			case PacketType.Moein:
				return new WebMoein().GetPacketActionName(action);
			case PacketType.Tafsil:
				return new WebTafsil().GetPacketActionName(action);
			case PacketType.UserLog:
				return new WebCashierLog().GetPacketActionName(action);
			case PacketType.DepEnteghal:
				return new WebDepEnteghal().GetPacketActionName(action);
			case PacketType.LkInfo:
				return new WebLkInfo().GetPacketActionName(action);
			case PacketType.CheckMOperation:
				return new WebChequeMOperation().GetPacketActionName(action);
			case PacketType.CheckShOperation:
				return new WebChequeShOperation().GetPacketActionName(action);
			case PacketType.Sanad:
				return new WebSanad().GetPacketActionName(action);
			case PacketType.PrdAnbarGardani:
				return new WebPrdAnbarGardani().GetPacketActionName(action);
			case PacketType.PrdSerial:
				return new WebPrdSerial().GetPacketActionName(action);
			case PacketType.IntroductionMethod:
				return new WebIntroductionMethod().GetPacketActionName(action);
			case PacketType.EntitySync:
				return new WebEntitySync().GetPacketActionName(action);
			case PacketType.Account:
				return new WebAccount().GetPacketActionName(action);
			case PacketType.CustomerRole:
				return new WebCustomerRole().GetPacketActionName(action);
			case PacketType.Note:
				return new WebNote().GetPacketActionName(action);
			case PacketType.TaxLogin:
				return new WebTaxLogin().GetPacketActionName(action);
			case PacketType.TaxSendOrder:
			case PacketType.TaxTrackingOrder:
			case PacketType.TaxEconomicCodeState:
				return new WebOrder().GetPacketActionName(action);
			case PacketType.AttPersonLeave:
				return new WebAttPersonLeave().GetPacketActionName(action);
			case PacketType.DiscountCoupon:
				return new WebDiscountCoupon().GetPacketActionName(action);
			case PacketType.Tax:
				return new WebTax().GetPacketActionName(action);
			case PacketType.TaxOrderLog:
				return new WebTaxOrderLog().GetPacketActionName(action);
			case PacketType.AttDepartment:
				return new WebAttDepartment().GetPacketActionName(action);
			case PacketType.AghsatMGroup:
				return new WebAghsatMGroup().GetPacketActionName(action);
			case PacketType.ProductPriceLevel:
				return new WebProductPriceLevel().GetPacketActionName(action);
			case PacketType.SanadStatusReport:
				return new WebSanadStatusReport().GetPacketActionName(action);
			case PacketType.SanadPrint:
				return new WebSanadPrint().GetPacketActionName(action);
			case PacketType.AttendanceDeviceLogOnPersonel:
				return new WebDeviceLogOnPerson().GetPacketActionName(action);
			case PacketType.BioMetric:
				return new WebBioWriteDevice().GetPacketActionName(action);
			case PacketType.none:
			case PacketType.WebProductAttribute:
				return "";
			default:
				throw new ArgumentOutOfRangeException($"type({checked((short)type)}):{type}//action:{action}");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Packet.cs", 953);
			return $"Error({action})";
		}
	}

	public string UniqueKey()
	{
		return PacketReferenceId.ToString() + Part;
	}
}
