using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;
using PacketParser.BusinessLayer;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;
using PacketParser.EventArgs;

namespace PacketParser.Entities;

public class WebDepEnteghal : Serializable<WebDepEnteghal>, IDepEnteghal, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IReceiver, INpDisposable, IDisposable
{
	public List<WebDepEnteghalDet> Items { get; set; }

	public Guid Guid { get; set; }

	public Guid UserGuid { get; set; }

	public int No { get; set; }

	public string Description { get; set; }

	public DateTime DateM { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public string DateSh { get; }

	public DateTime ModifiedM { get; set; }

	public string MashinNo { get; set; }

	public string GirandeName { get; set; }

	public string RanandehName { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public bool? DeliveryStatus { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public string LkSerial { get; set; }

	public string HddSerial { get; set; }

	public string CpuSerial { get; set; }

	public string AndroidIme { get; set; }

	public ENSource Source { get; set; }

	public string IP { get; set; }

	public short DepID { get; set; }

	public string SerializedData { get; set; }

	public short StatusId { get; set; }

	public DateTime DateSabt { get; set; }

	public Guid? DebitMoeinGuid { get; set; }

	public Guid? DebitTafsilGuid { get; set; }

	public Guid? CreditMoeinGuid { get; set; }

	public Guid? CreditTafsilGuid { get; set; }

	public int SanadNo { get; set; }

	[JsonIgnore]
	[XmlIgnore]
	public string DeliveryStatusFa
	{
		get
		{
			if (DeliveryStatus.HasValue)
			{
				if (!DeliveryStatus.GetValueOrDefault())
				{
					return "خطا در تحویل";
				}
				return "تحویل شده";
			}
			return "ارسال شده";
		}
	}

	[XmlIgnore]
	[JsonIgnore]
	public new bool IsDisposed { get; private set; }

	public event Func<object, ConnectionEventArgs<WebDepEnteghal>, Task> OnSaveRequest;

	public event Func<object, SaveResponseEventArgs, Task> OnSaveResponse;

	public event Func<object, ReadOperationEventArgs, Task> OnReadRequest;

	public event Func<object, ConnectionEventArgs<WebDepEnteghal>, Task> OnReadResponse;

	public static List<Packet> GenerateSaveRequest(IEnumerable<WebDepEnteghal> items)
	{
		using (new JaegerMonitor("GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDepEnteghal.cs"))
		{
			try
			{
				if (items == null || !items.Any())
				{
					throw new NullReferenceException();
				}
				return items?.GeneratePackets(PacketType.DepEnteghal, 1, null, isSync: false, null, 0, deliveryRequst: false, "GenerateSaveRequest");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDepEnteghal.cs", 70);
				throw;
			}
		}
	}

	public static List<Packet> GenerateSaveResponse(IEnumerable<SaveResponse> lst)
	{
		using (new JaegerMonitor("GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDepEnteghal.cs"))
		{
			try
			{
				if (lst == null || !lst.Any())
				{
					throw new NullReferenceException();
				}
				return lst?.GeneratePackets(PacketType.DepEnteghal, 2, null, isSync: false, null, 0, deliveryRequst: false, "GenerateSaveResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDepEnteghal.cs", 84);
				throw;
			}
		}
	}

	public static List<Packet> GenerateSaveResponse(int hostID, Guid guid, DateTime date, string shortmessage, string message, bool saveResultStatus)
	{
		using (new JaegerMonitor("GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDepEnteghal.cs"))
		{
			try
			{
				return GenerateSaveResponse(new _003C_003Ez__ReadOnlySingleElementList<SaveResponse>(new SaveResponse
				{
					ReferencedId = hostID,
					Guid = guid,
					Date = date,
					Shortmessage = shortmessage,
					Message = message,
					SaveResultStatus = saveResultStatus
				}));
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDepEnteghal.cs", 106);
				throw;
			}
		}
	}

	public static Packet GenerateReadRequest(string connectionUniqueId, List<ReadStatus> status, bool changeReadST, short top, ENSource source, short depId, int uId, long lKSerial)
	{
		using (new JaegerMonitor("GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDepEnteghal.cs"))
		{
			try
			{
				List<string> list = new List<string>
				{
					((short)5).ToString(),
					new ReadOperationEventArgs
					{
						ChangeReadST = changeReadST,
						Top = top,
						Status = status
					}.ToString()
				};
				return new Packet(PacketType.DepEnteghal, 5, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDepEnteghal.cs", 130);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReadResponse(IEnumerable<WebDepEnteghal> items)
	{
		using (new JaegerMonitor("GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDepEnteghal.cs"))
		{
			try
			{
				if (items == null || !items.Any())
				{
					throw new NullReferenceException();
				}
				return items?.GeneratePackets(PacketType.DepEnteghal, 6, null, isSync: false, null, 0, deliveryRequst: true, "GenerateReadResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDepEnteghal.cs", 144);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReset(IEnumerable<WebDepEnteghal> depEnteghals)
	{
		try
		{
			if (depEnteghals == null || !depEnteghals.Any())
			{
				throw new NullReferenceException();
			}
			return depEnteghals.GeneratePackets(PacketType.DepEnteghal, 7, null, isSync: false, null, 0, deliveryRequst: true, "GenerateReset");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDepEnteghal.cs", 157);
			throw;
		}
	}

	public ReturnedSaveFuncInfo CheckValidation()
	{
		using (new JaegerMonitor("CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDepEnteghal.cs"))
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
			try
			{
				if (DateM < new DateTime(2010, 1, 1) || DateM > new DateTime(2099, 1, 1))
				{
					returnedSaveFuncInfo.AddError("تاریخ وارد شده جهت برگه دریافت معتبر نمی باشد", "DateM", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDepEnteghal.cs", 168);
				}
				_ = Guid;
				if (Guid == Guid.Empty)
				{
					Guid = Guid.NewGuid();
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDepEnteghal.cs", 174);
				returnedSaveFuncInfo.AddReturnedValue(ex, "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDepEnteghal.cs", 175);
			}
			return returnedSaveFuncInfo;
		}
	}

	public string GetPacketActionEnumName(short action)
	{
		DepEnteghalActions depEnteghalActions = (DepEnteghalActions)action;
		return depEnteghalActions.ToString();
	}

	public string GetPacketActionName(short action)
	{
		return ((DepEnteghalActions)action).GetDisplay();
	}

	public async Task DataReceived(Packet pack, string connectionUniqueId, ConnectionData connection)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDepEnteghal.cs"))
		{
			_ = 3;
			try
			{
				switch ((DepEnteghalActions)pack.PacketActions)
				{
				case DepEnteghalActions.ReadResponse:
				{
					ConnectionEventArgs<WebDepEnteghal> args2 = new ConnectionEventArgs<WebDepEnteghal>(pack, connectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnReadResponse, this, args2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDepEnteghal.cs");
					break;
				}
				case DepEnteghalActions.ReadRequest:
				{
					ReadOperationEventArgs readOperationEventArgs = pack.Args[1].FromJson<ReadOperationEventArgs>("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDepEnteghal.cs", 194);
					readOperationEventArgs.ConnectionUniqueId = connectionUniqueId;
					readOperationEventArgs.Connection = connection;
					await Utilities.RaiseEvent(this.OnReadRequest, this, readOperationEventArgs, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDepEnteghal.cs");
					break;
				}
				case DepEnteghalActions.SaveRequest:
					Json.FromJson<List<WebDepEnteghal>>(pack.PacketContents, sendErrorIfException: true, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDepEnteghal.cs", 200);
					await Utilities.RaiseEvent(this.OnSaveRequest, this, new ConnectionEventArgs<WebDepEnteghal>(pack, connectionUniqueId, connection), connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDepEnteghal.cs");
					break;
				case DepEnteghalActions.SaveResponse:
				{
					SaveResponseEventArgs args = new SaveResponseEventArgs(pack, connectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnSaveResponse, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDepEnteghal.cs");
					break;
				}
				case (DepEnteghalActions)3:
				case (DepEnteghalActions)4:
				case DepEnteghalActions.Reset:
					break;
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDepEnteghal.cs", 213);
			}
		}
	}

	public byte[] ToBytes()
	{
		using (new JaegerMonitor("ToBytes", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDepEnteghal.cs"))
		{
			try
			{
				return Json.ToBytesJson(this);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ToBytes", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDepEnteghal.cs", 224);
				throw;
			}
		}
	}

	public override void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
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
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDepEnteghal.cs", 240);
		}
	}
}
