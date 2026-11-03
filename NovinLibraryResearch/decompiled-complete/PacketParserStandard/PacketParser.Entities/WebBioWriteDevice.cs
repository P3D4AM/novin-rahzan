using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models.Attendance;
using PacketParser.Entities.Attendance;
using PacketParser.Entities.Attendance.Biometric;
using PacketParser.Entities.CompositeKeys;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Attendance.Biometric;
using PacketParser.EventArgs;
using PacketParser.EventArgs.Attendance;

namespace PacketParser.Entities;

public class WebBioWriteDevice : Serializable<WebBioWriteDevice>, IBioWriteDevice, IReceiver, INpDisposable, IDisposable
{
	public Guid BioGuid { get; set; }

	public Guid DeviceGuid { get; set; }

	public string DeviceIp { get; set; }

	public int DevicePort { get; set; }

	public int DeviceNumber { get; set; }

	public int DevicePassword { get; set; }

	public int HardwareCode { get; set; }

	public string HarwareName { get; set; } = "";


	public DateTime InsertedDateM { get; set; }

	public short TryCount { get; set; }

	public DateTime NextTry { get; set; }

	public Guid UserGuid { get; set; }

	public string Message { get; set; }

	public EnAttendancePrivilage Privilage { get; set; }

	public WebBiometricData BiometricData { get; set; }

	public EnDeviceMessageType MessageType { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public EnDeviceGroup DeviceGroup => BiometricData?.DeviceGroup ?? EnDeviceGroup.None;

	[XmlIgnore]
	[JsonIgnore]
	public new bool IsDisposed { get; private set; }

	public event Func<object, BioMetricReadEventArgs, Task> OnReadRequest;

	public event Func<object, ConnectionEventArgs<WebBiometricReadMaster>, Task> OnReadResponse;

	public event Func<object, ConnectionEventArgs<WebBioWriteDevice>, Task> OnSaveRequest;

	public event Func<object, SaveResponseEventArgs<BioWriteDeviceCompositeKey>, Task> OnSaveResponse;

	public event Func<object, BioMetricDisconnectEventArgs, Task> OnDisconnectRequest;

	public event Func<object, BioMetricDisconnectEventArgs, Task> OnDisconnectResponse;

	public string GetPacketActionEnumName(short action)
	{
		BioMetricActions bioMetricActions = (BioMetricActions)action;
		return bioMetricActions.ToString();
	}

	public string GetPacketActionName(short action)
	{
		BioMetricActions bioMetricActions = (BioMetricActions)action;
		return bioMetricActions.ToString();
	}

	public async Task DataReceived(Packet pack, string ConnectionUniqueId, ConnectionData connection)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\Biometric\\WebBioWriteDevice.cs"))
		{
			_ = 5;
			try
			{
				switch ((BioMetricActions)pack.PacketActions)
				{
				case BioMetricActions.SaveRequest:
					if (this.OnSaveRequest != null)
					{
						ConnectionEventArgs<WebBioWriteDevice> args = new ConnectionEventArgs<WebBioWriteDevice>(pack, ConnectionUniqueId, connection);
						await Utilities.RaiseEvent(this.OnSaveRequest, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\Biometric\\WebBioWriteDevice.cs");
					}
					break;
				case BioMetricActions.SaveResponse:
					if (this.OnSaveResponse != null)
					{
						SaveResponseEventArgs<BioWriteDeviceCompositeKey> args3 = new SaveResponseEventArgs<BioWriteDeviceCompositeKey>(pack, ConnectionUniqueId, connection);
						await Utilities.RaiseEvent(this.OnSaveResponse, this, args3, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\Biometric\\WebBioWriteDevice.cs");
					}
					break;
				case BioMetricActions.ReadRequest:
					if (this.OnReadRequest != null)
					{
						BioMetricReadEventArgs bioMetricReadEventArgs = pack.Args[1].FromJson<BioMetricReadEventArgs>("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\Biometric\\WebBioWriteDevice.cs", 70);
						bioMetricReadEventArgs.ConnectionUniqueId = ConnectionUniqueId;
						bioMetricReadEventArgs.Connection = connection;
						await Utilities.RaiseEvent(this.OnReadRequest, this, bioMetricReadEventArgs, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\Biometric\\WebBioWriteDevice.cs");
					}
					break;
				case BioMetricActions.ReadResponse:
					if (this.OnReadResponse != null)
					{
						ConnectionEventArgs<WebBiometricReadMaster> args2 = new ConnectionEventArgs<WebBiometricReadMaster>(pack, ConnectionUniqueId, connection);
						await Utilities.RaiseEvent(this.OnReadResponse, this, args2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\Biometric\\WebBioWriteDevice.cs");
					}
					break;
				case BioMetricActions.DisconnectRequest:
					if (this.OnDisconnectRequest != null)
					{
						BioMetricDisconnectEventArgs bioMetricDisconnectEventArgs2 = pack.Args[1].FromJson<BioMetricDisconnectEventArgs>("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\Biometric\\WebBioWriteDevice.cs", 82);
						bioMetricDisconnectEventArgs2.ConnectionUniqueId = ConnectionUniqueId;
						bioMetricDisconnectEventArgs2.Connection = connection;
						await Utilities.RaiseEvent(this.OnDisconnectRequest, this, bioMetricDisconnectEventArgs2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\Biometric\\WebBioWriteDevice.cs");
					}
					break;
				case BioMetricActions.DisconnectResponse:
					if (this.OnDisconnectResponse != null)
					{
						BioMetricDisconnectEventArgs bioMetricDisconnectEventArgs = pack.Args[1].FromJson<BioMetricDisconnectEventArgs>("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\Biometric\\WebBioWriteDevice.cs", 89);
						bioMetricDisconnectEventArgs.ConnectionUniqueId = ConnectionUniqueId;
						bioMetricDisconnectEventArgs.Connection = connection;
						await Utilities.RaiseEvent(this.OnDisconnectResponse, this, bioMetricDisconnectEventArgs, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\Biometric\\WebBioWriteDevice.cs");
					}
					break;
				default:
					throw new NotImplementedException();
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\Biometric\\WebBioWriteDevice.cs", 98);
			}
		}
	}

	public static List<Packet> GenerateSaveRequest(IEnumerable<WebBioWriteDevice> bioMetrics, int partitionId = 0, string debuggerInfo = "")
	{
		using (new JaegerMonitor("GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\Biometric\\WebBioWriteDevice.cs"))
		{
			try
			{
				if (bioMetrics == null || !bioMetrics.Any())
				{
					throw new NullReferenceException();
				}
				object result;
				if (bioMetrics == null)
				{
					result = null;
				}
				else
				{
					result = bioMetrics.GeneratePackets(PacketType.BioMetric, 1, null, isSync: false, null, partitionId, deliveryRequst: false, debuggerInfo);
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\Biometric\\WebBioWriteDevice.cs", 110);
				throw;
			}
		}
	}

	public static List<Packet> GenerateSaveResponse(IEnumerable<SaveResponse<BioWriteDeviceCompositeKey>> responses, Guid? packetRefrenceId)
	{
		using (new JaegerMonitor("GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\Biometric\\WebBioWriteDevice.cs"))
		{
			try
			{
				if (responses == null || !responses.Any())
				{
					throw new NullReferenceException();
				}
				return responses?.GeneratePackets(PacketType.BioMetric, 2, null, isSync: false, packetRefrenceId, 0, deliveryRequst: false, "GenerateSaveResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\Biometric\\WebBioWriteDevice.cs", 124);
				throw;
			}
		}
	}

	public static Packet GenerateReadRequest(MicroDeviceInfo deviceInfo, DateTime modified)
	{
		using (new JaegerMonitor("GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\Biometric\\WebBioWriteDevice.cs"))
		{
			try
			{
				List<string> list = new List<string> { ((short)5).ToString() };
				BioMetricReadEventArgs bioMetricReadEventArgs = new BioMetricReadEventArgs(modified, deviceInfo);
				list.Add(bioMetricReadEventArgs.ToString());
				return new Packet(PacketType.BioMetric, 5, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\Biometric\\WebBioWriteDevice.cs", 140);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReadResponse(IEnumerable<WebBiometricReadMaster> data, int partitionId = 0)
	{
		using (new JaegerMonitor("GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\Biometric\\WebBioWriteDevice.cs"))
		{
			try
			{
				object result;
				if (data == null)
				{
					result = null;
				}
				else
				{
					result = data.GeneratePackets(PacketType.BioMetric, 6, null, isSync: false, null, partitionId, deliveryRequst: false, "GenerateReadResponse");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\Biometric\\WebBioWriteDevice.cs", 153);
				throw;
			}
		}
	}

	public static Packet GenerateDisconnectRequest(MicroDeviceInfo deviceInfo)
	{
		using (new JaegerMonitor("GenerateDisconnectRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\Biometric\\WebBioWriteDevice.cs"))
		{
			try
			{
				if (deviceInfo == null)
				{
					throw new NullReferenceException();
				}
				List<string> list = new List<string> { ((short)9).ToString() };
				BioMetricDisconnectEventArgs bioMetricDisconnectEventArgs = new BioMetricDisconnectEventArgs(deviceInfo, null);
				list.Add(bioMetricDisconnectEventArgs.ToString());
				return new Packet(PacketType.BioMetric, 9, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateDisconnectRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\Biometric\\WebBioWriteDevice.cs", 170);
				throw;
			}
		}
	}

	public static Packet GenerateDisconnectResponse(MicroDeviceInfo deviceInfo, ReturnedSaveFuncInfo result)
	{
		using (new JaegerMonitor("GenerateDisconnectResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\Biometric\\WebBioWriteDevice.cs"))
		{
			try
			{
				if (deviceInfo == null || result == null)
				{
					throw new NullReferenceException();
				}
				List<string> list = new List<string> { ((short)9).ToString() };
				BioMetricDisconnectEventArgs bioMetricDisconnectEventArgs = new BioMetricDisconnectEventArgs(deviceInfo, result);
				list.Add(bioMetricDisconnectEventArgs.ToString());
				return new Packet(PacketType.BioMetric, 10, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateDisconnectResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\Biometric\\WebBioWriteDevice.cs", 187);
				throw;
			}
		}
	}

	public MicroDeviceInfo CastToMicro()
	{
		return new MicroDeviceInfo(DeviceGuid, DeviceIp, DevicePort, DeviceNumber, DevicePassword, DeviceGroup);
	}

	public override void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				this.OnReadRequest = null;
				this.OnReadResponse = null;
				this.OnSaveRequest = null;
				this.OnSaveResponse = null;
				this.OnDisconnectRequest = null;
				this.OnDisconnectResponse = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\Biometric\\WebBioWriteDevice.cs", 208);
		}
	}
}
