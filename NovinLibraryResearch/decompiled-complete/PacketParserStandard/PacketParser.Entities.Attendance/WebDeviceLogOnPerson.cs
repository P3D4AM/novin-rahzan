using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.Entities.Interfaces.Attendance;
using PacketParser.EventArgs;

namespace PacketParser.Entities.Attendance;

public class WebDeviceLogOnPerson : Serializable<WebDeviceLogOnPerson>, IDeviceLogOnPerson
{
	public Guid Guid { get; set; }

	public string DeviceIp { get; set; }

	public string DeviceSerial { get; set; }

	public string DeviceModel { get; set; }

	public string DevicePassword { get; set; }

	public string InOut { get; set; }

	public string VerifyMode { get; set; }

	public string LogPhoto { get; set; }

	public int DeviceNo { get; set; }

	public int DevicePort { get; set; }

	public int IoMode { get; set; }

	public DeviceModelEnumType TerminalType { get; set; }

	public DateTime DateTimeLogM { get; set; }

	public long IdTraffic { get; set; }

	public int PersonNo { get; set; }

	public DateTime FixedDateM { get; }

	[XmlIgnore]
	[JsonIgnore]
	public new bool IsDisposed { get; private set; }

	public event Func<object, ConnectionEventArgs<WebDeviceLogOnPerson>, Task> OnSaveRequest;

	public event Func<object, SaveResponseEventArgs, Task> OnSaveResponse;

	public event Func<object, ConnectionEventArgs<DeviceLogOnPersonReadEventArgs>, Task> OnReadRequest;

	public event Func<object, ConnectionEventArgs<WebDeviceLogOnPerson>, Task> OnReadResponse;

	public static Packet GenerateReadRequest(int requestedUId, DateTime modified)
	{
		using (new JaegerMonitor("GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebDeviceLogOnPerson.cs"))
		{
			try
			{
				List<string> list = new List<string> { ((short)5).ToString() };
				DeviceLogOnPersonReadEventArgs deviceLogOnPersonReadEventArgs = new DeviceLogOnPersonReadEventArgs
				{
					Modified = modified
				};
				list.Add(deviceLogOnPersonReadEventArgs.ToString());
				return new Packet(PacketType.AttendanceDeviceLogOnPersonel, 5, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebDeviceLogOnPerson.cs", 50);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReadResponse(IEnumerable<IDeviceLogOnPerson> items, bool isSync = false)
	{
		if (items == null || !items.Any())
		{
			throw new NullReferenceException();
		}
		using (new JaegerMonitor("GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebDeviceLogOnPerson.cs"))
		{
			try
			{
				return items?.GeneratePackets(PacketType.AttendanceDeviceLogOnPersonel, 6, null, isSync, null, 0, deliveryRequst: true, "GenerateReadResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebDeviceLogOnPerson.cs", 64);
				return null;
			}
		}
	}

	public static List<Packet> GenerateSaveRequest(IEnumerable<WebDeviceLogOnPerson> data, int partitionId = 0)
	{
		using (new JaegerMonitor("GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebDeviceLogOnPerson.cs"))
		{
			try
			{
				if (data == null || !data.Any())
				{
					throw new NullReferenceException();
				}
				object result;
				if (data == null)
				{
					result = null;
				}
				else
				{
					result = data.GeneratePackets(PacketType.AttendanceDeviceLogOnPersonel, 1, null, isSync: false, null, partitionId, deliveryRequst: false, "GenerateSaveRequest");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebDeviceLogOnPerson.cs", 78);
				throw;
			}
		}
	}

	public static List<Packet> GenerateSaveResponse(IEnumerable<SaveResponse> responses)
	{
		using (new JaegerMonitor("GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebDeviceLogOnPerson.cs"))
		{
			try
			{
				if (responses == null || !responses.Any())
				{
					throw new NullReferenceException();
				}
				return responses?.GeneratePackets(PacketType.AttendanceDeviceLogOnPersonel, 2, null, isSync: false, null, 0, deliveryRequst: false, "GenerateSaveResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebDeviceLogOnPerson.cs", 92);
				throw;
			}
		}
	}

	public string GetPacketActionEnumName(short action)
	{
		DeviceLogOnPersonActions deviceLogOnPersonActions = (DeviceLogOnPersonActions)action;
		return deviceLogOnPersonActions.ToString();
	}

	public string GetPacketActionName(short action)
	{
		return ((DeviceLogOnPersonActions)action).GetDisplay();
	}

	public async Task DataReceived(Packet pack, string connectionUniqueId, ConnectionData connection)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebDeviceLogOnPerson.cs"))
		{
			_ = 3;
			try
			{
				switch ((DeviceLogOnPersonActions)pack.PacketActions)
				{
				case DeviceLogOnPersonActions.ReadRequest:
				{
					ConnectionEventArgs<DeviceLogOnPersonReadEventArgs> args4 = new ConnectionEventArgs<DeviceLogOnPersonReadEventArgs>(pack, connectionUniqueId, connection);
					try
					{
						await Utilities.RaiseEvent(this.OnReadRequest, this, args4, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebDeviceLogOnPerson.cs");
						break;
					}
					catch (Exception ex2)
					{
						WebErrorLog.Instance.StartLog(ex2, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebDeviceLogOnPerson.cs", 112);
						break;
					}
				}
				case DeviceLogOnPersonActions.ReadResponse:
				{
					ConnectionEventArgs<WebDeviceLogOnPerson> args2 = new ConnectionEventArgs<WebDeviceLogOnPerson>(pack, connectionUniqueId, connection);
					try
					{
						await Utilities.RaiseEvent(this.OnReadResponse, this, args2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebDeviceLogOnPerson.cs");
						break;
					}
					catch (Exception ex)
					{
						WebErrorLog.Instance.StartLog(ex, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebDeviceLogOnPerson.cs", 120);
						break;
					}
				}
				case DeviceLogOnPersonActions.SaveRequest:
					if (this.OnSaveRequest != null)
					{
						ConnectionEventArgs<WebDeviceLogOnPerson> args3 = new ConnectionEventArgs<WebDeviceLogOnPerson>(pack, connectionUniqueId, connection);
						await Utilities.RaiseEvent(this.OnSaveRequest, this, args3, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebDeviceLogOnPerson.cs");
					}
					break;
				case DeviceLogOnPersonActions.SaveResponse:
					if (this.OnSaveResponse != null)
					{
						SaveResponseEventArgs args = new SaveResponseEventArgs(pack, connectionUniqueId, connection);
						await Utilities.RaiseEvent(this.OnSaveResponse, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebDeviceLogOnPerson.cs");
					}
					break;
				case (DeviceLogOnPersonActions)3:
				case (DeviceLogOnPersonActions)4:
					break;
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex4)
			{
				WebErrorLog.Instance.StartLog(ex4, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebDeviceLogOnPerson.cs", 136);
			}
		}
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
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebDeviceLogOnPerson.cs", 152);
		}
	}
}
