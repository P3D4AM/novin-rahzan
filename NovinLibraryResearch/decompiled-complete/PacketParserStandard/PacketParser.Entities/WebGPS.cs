using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;

namespace PacketParser.Entities;

[Serializable]
public class WebGPS : Serializable<WebGPS>, IGPS, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IReceiver, INpDisposable, IDisposable
{
	private const int ChunckSizeKB = 100;

	public int Id { get; set; }

	public int UId { get; set; }

	public DateTime DateM { get; set; } = DateTime.Now;


	public DateTime? DateMUtc { get; set; }

	public Guid UserAccGuid { get; set; } = Guid.Empty;


	public decimal Distance { get; set; }

	public EnGpsActivity Activity { get; set; }

	[DefaultValue(EnGpsActivity.None)]
	public EnGpsActivity A
	{
		get
		{
			return Activity;
		}
		set
		{
			Activity = value;
		}
	}

	public string Icon { get; set; } = "Null.png";


	public string PointNumber { get; set; } = ".";


	public decimal FromStartPointDistance { get; set; }

	public string Description
	{
		get
		{
			try
			{
				StringBuilder stringBuilder = new StringBuilder($"زمان {GPSTime.Hour}:{GPSTime.Minute}");
				if (PointDurationSeconds > 120m)
				{
					stringBuilder.Append(" ** مدت توقف:" + PointDurationTime);
				}
				if (Math.Round(FromLastPointDistance, 0) > 0m)
				{
					stringBuilder.Append(" ** فاصله از نقطه قبلی:" + FromLastPointDistance.NpToString("N0", 0) + "متر");
				}
				if (Math.Round(FromStartPointDistance, 0) > 0m)
				{
					stringBuilder.Append(" ** فاصله از ابتدای مسیر:" + FromStartPointDistance.NpToString("N0", 0) + "متر");
				}
				return stringBuilder.ToString();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Description", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebGPS.cs", 52);
				return "";
			}
		}
		set
		{
		}
	}

	public string ColorHex { get; set; }

	public string PointDurationTime => Calendar.Minutes2TimeFormat((int)(PointDurationSeconds / 60m));

	public DateTime GPSTime { get; set; } = DateTime.Now;


	public decimal Latitude { get; set; }

	public decimal Longitude { get; set; }

	public decimal Speed { get; set; }

	public decimal Time { get; set; }

	public decimal Accuracy { get; set; } = decimal.MaxValue;


	public decimal Altitude { get; set; }

	public decimal Bearing { get; set; }

	public decimal FromLastPointDistance { get; set; }

	public decimal PointDurationSeconds { get; set; }

	public int PreviewsID { get; set; }

	public Guid Guid { get; set; } = Guid.NewGuid();


	public DateTime Modified { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	[JsonIgnore]
	public WebUserInformation UserInfo { get; set; }

	public Guid UserGuid { get; set; } = Guid.Empty;


	public DateTime InsertTime { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public new bool IsDisposed { get; private set; }

	public ServerStatusForClient ServerStatus { get; set; } = ServerStatusForClient.Delivered;


	public event Func<object, GpsReadEventArgs, Task> OnReadRequest;

	public event Func<object, ConnectionEventArgs<WebGPS>, Task> OnReadResponse;

	public event Func<object, ConnectionEventArgs<WebGPS>, Task> OnSaveRequest;

	public event Func<object, SaveResponseEventArgs, Task> OnSaveResponse;

	public event Func<object, ConnectionEventArgs<WebGPS>, Task> OnSync;

	public static Packet GenerateReadRequest(int requestedUId, DateTime date1, DateTime date2)
	{
		using (new JaegerMonitor("GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebGPS.cs"))
		{
			try
			{
				List<string> list = new List<string> { ((short)5).ToString() };
				GpsReadEventArgs gpsReadEventArgs = new GpsReadEventArgs
				{
					RequestedUId = requestedUId,
					Date1 = date1,
					Date2 = date2
				};
				list.Add(gpsReadEventArgs.ToString());
				return new Packet(PacketType.GPS, 5, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebGPS.cs", 112);
				return null;
			}
		}
	}

	public static List<Packet> GenerateReadResponse(IEnumerable<WebGPS> items)
	{
		if (items == null || items.Count() <= 0)
		{
			throw new NullReferenceException();
		}
		using (new JaegerMonitor("GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebGPS.cs"))
		{
			try
			{
				return items?.GeneratePackets(PacketType.GPS, 6, null, isSync: false, null, 0, deliveryRequst: true, "GenerateReadResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebGPS.cs", 127);
				return null;
			}
		}
	}

	public static List<Packet> GenerateSaveRequest(IEnumerable<WebGPS> gpses)
	{
		using (new JaegerMonitor("GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebGPS.cs"))
		{
			try
			{
				return gpses?.GeneratePackets(PacketType.GPS, 1, null, isSync: false, null, 0, deliveryRequst: false, "GenerateSaveRequest");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebGPS.cs", 143);
				throw;
			}
		}
	}

	public static List<Packet> GenerateSaveResponse(IEnumerable<SaveResponse> responses, bool isSync = false)
	{
		using (new JaegerMonitor("GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebGPS.cs"))
		{
			try
			{
				return responses?.GeneratePackets(PacketType.GPS, 2, null, isSync, null, 0, deliveryRequst: false, "GenerateSaveResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebGPS.cs", 158);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReset(IEnumerable<WebGPS> gpses)
	{
		using (new JaegerMonitor("GenerateReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebGPS.cs"))
		{
			try
			{
				return gpses.GeneratePackets(PacketType.GPS, 7, null, isSync: false, null, 0, deliveryRequst: true, "GenerateReset");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebGPS.cs", 173);
				return null;
			}
		}
	}

	public async Task DataReceived(Packet pack, string connectionUniqueId, ConnectionData connection)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebGPS.cs"))
		{
			_ = 4;
			try
			{
				int num;
				if (connection == null)
				{
					num = 1;
				}
				else
				{
					_ = connection.cancellationToken;
					num = 0;
				}
				if (num != 0)
				{
					return;
				}
				connection?.cancellationToken.ThrowIfCancellationRequested();
				switch ((GpsActions)pack.PacketActions)
				{
				case GpsActions.SaveRequest:
					if (this.OnSaveRequest != null)
					{
						ConnectionEventArgs<WebGPS> args3 = new ConnectionEventArgs<WebGPS>(pack, connectionUniqueId, connection);
						await Utilities.RaiseEvent(this.OnSaveRequest, this, args3, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebGPS.cs");
					}
					break;
				case GpsActions.SaveResponse:
					if (this.OnSaveResponse != null)
					{
						SaveResponseEventArgs args2 = new SaveResponseEventArgs(pack, connectionUniqueId, connection);
						await Utilities.RaiseEvent(this.OnSaveResponse, this, args2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebGPS.cs");
					}
					break;
				case GpsActions.ReadRequest:
					if (this.OnReadRequest != null)
					{
						GpsReadEventArgs gpsReadEventArgs = pack.Args[1].FromJson<GpsReadEventArgs>("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebGPS.cs", 207);
						gpsReadEventArgs.ConnectionUniqueId = connectionUniqueId;
						gpsReadEventArgs.Connection = connection;
						await Utilities.RaiseEvent(this.OnReadRequest, this, gpsReadEventArgs, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebGPS.cs");
					}
					break;
				case GpsActions.ReadResponse:
					if (this.OnReadResponse != null || this.OnSync != null)
					{
						ConnectionEventArgs<WebGPS> args = new ConnectionEventArgs<WebGPS>(pack, connectionUniqueId, connection);
						if (pack.IsSynced)
						{
							await Utilities.RaiseEvent(this.OnSync, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebGPS.cs");
						}
						else
						{
							await Utilities.RaiseEvent(this.OnReadResponse, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebGPS.cs");
						}
					}
					break;
				case (GpsActions)3:
				case (GpsActions)4:
				case GpsActions.Reset:
					break;
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebGPS.cs", 231);
			}
		}
	}

	public string GetPacketActionEnumName(short action)
	{
		try
		{
			GpsActions gpsActions = (GpsActions)action;
			return gpsActions.ToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionEnumName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebGPS.cs", 244);
			return $"Error({action})";
		}
	}

	public string GetPacketActionName(short action)
	{
		try
		{
			return ((GpsActions)action).GetDisplay();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebGPS.cs", 257);
			return $"Error({action})";
		}
	}

	public override void Dispose()
	{
		Dispose(disposing: true);
	}

	public override void Dispose(bool disposing)
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
				if (this.OnSync != null)
				{
					this.OnSync = null;
				}
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebGPS.cs", 279);
		}
	}
}
