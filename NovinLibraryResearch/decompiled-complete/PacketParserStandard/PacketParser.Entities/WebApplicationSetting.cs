using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;

namespace PacketParser.Entities;

[Serializable]
public class WebApplicationSetting : IApplicationSetting, IReceiver, INpDisposable, IDisposable
{
	public string Name { get; set; } = "";


	public string Val { get; set; } = "";


	public DateTime Modified { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public bool IsDisposed { get; private set; }

	public ServerStatusForClient ServerStatus { get; set; } = ServerStatusForClient.Delivered;


	public event Func<object, AppSettingsReadEventArgs, Task> OnReadRequest;

	public event Func<object, ConnectionEventArgs<WebApplicationSetting>, Task> OnReadResponse;

	public event Func<object, ConnectionEventArgs<WebApplicationSetting>, Task> OnSaveRequest;

	public event Func<object, SaveResponseEventArgs, Task> OnSaveResponse;

	public event Func<object, ConnectionEventArgs<WebApplicationSetting>, Task> OnSync;

	public static Packet GenerateReadRequest(int requestedUId, DateTime modified)
	{
		using (new JaegerMonitor("GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebApplicationSetting.cs"))
		{
			try
			{
				List<string> list = new List<string> { ((short)5).ToString() };
				AppSettingsReadEventArgs appSettingsReadEventArgs = new AppSettingsReadEventArgs
				{
					Modified = modified,
					Part = 0,
					TotalParts = 1,
					PacketReferenceId = Guid.Empty
				};
				list.Add(appSettingsReadEventArgs.ToString());
				return new Packet(PacketType.ApplicationSetting, 5, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebApplicationSetting.cs", 46);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReadResponse(IEnumerable<WebApplicationSetting> items, bool isSync = false)
	{
		if (items == null || items.Count() <= 0)
		{
			throw new NullReferenceException();
		}
		using (new JaegerMonitor("GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebApplicationSetting.cs"))
		{
			try
			{
				return items?.GeneratePackets(PacketType.ApplicationSetting, 6, null, isSync, null, 0, deliveryRequst: true, "GenerateReadResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebApplicationSetting.cs", 60);
				return null;
			}
		}
	}

	public static List<Packet> GenerateSaveRequest(IEnumerable<WebApplicationSetting> settings)
	{
		using (new JaegerMonitor("GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebApplicationSetting.cs"))
		{
			try
			{
				return settings?.GeneratePackets(PacketType.ApplicationSetting, 1, null, isSync: false, null, 0, deliveryRequst: false, "GenerateSaveRequest");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebApplicationSetting.cs", 73);
				throw;
			}
		}
	}

	public static List<Packet> GenerateSaveResponse(IEnumerable<SaveResponse> responses)
	{
		using (new JaegerMonitor("GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebApplicationSetting.cs"))
		{
			try
			{
				return responses?.GeneratePackets(PacketType.ApplicationSetting, 2, null, isSync: false, null, 0, deliveryRequst: false, "GenerateSaveResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebApplicationSetting.cs", 86);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReset(IEnumerable<WebApplicationSetting> settings)
	{
		using (new JaegerMonitor("GenerateReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebApplicationSetting.cs"))
		{
			try
			{
				return settings?.GeneratePackets(PacketType.ApplicationSetting, 7, null, isSync: false, null, 0, deliveryRequst: true, "GenerateReset");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebApplicationSetting.cs", 99);
				throw;
			}
		}
	}

	public string GetPacketActionName(short action)
	{
		return ((AppSettingsActions)action).GetDisplay();
	}

	public string GetPacketActionEnumName(short action)
	{
		AppSettingsActions appSettingsActions = (AppSettingsActions)action;
		return appSettingsActions.ToString();
	}

	public async Task DataReceived(Packet pack, string ConnectionUniqueId, ConnectionData connection)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebApplicationSetting.cs"))
		{
			_ = 4;
			try
			{
				switch ((AppSettingsActions)pack.PacketActions)
				{
				case AppSettingsActions.SaveRequest:
				{
					ConnectionEventArgs<WebApplicationSetting> args3 = new ConnectionEventArgs<WebApplicationSetting>(pack, ConnectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnSaveRequest, this, args3, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebApplicationSetting.cs");
					break;
				}
				case AppSettingsActions.SaveResponse:
				{
					SaveResponseEventArgs args2 = new SaveResponseEventArgs(pack, ConnectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnSaveResponse, this, args2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebApplicationSetting.cs");
					break;
				}
				case AppSettingsActions.ReadRequest:
				{
					AppSettingsReadEventArgs appSettingsReadEventArgs = pack.Args[1].FromJson<AppSettingsReadEventArgs>("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebApplicationSetting.cs", 123);
					appSettingsReadEventArgs.ConnectionUniqueId = ConnectionUniqueId;
					appSettingsReadEventArgs.Connection = connection;
					await Utilities.RaiseEvent(this.OnReadRequest, this, appSettingsReadEventArgs, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebApplicationSetting.cs");
					break;
				}
				case AppSettingsActions.ReadResponse:
				{
					ConnectionEventArgs<WebApplicationSetting> args = new ConnectionEventArgs<WebApplicationSetting>(pack, ConnectionUniqueId, connection);
					if (pack.IsSynced)
					{
						await Utilities.RaiseEvent(this.OnSync, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebApplicationSetting.cs");
					}
					else
					{
						await Utilities.RaiseEvent(this.OnReadResponse, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebApplicationSetting.cs");
					}
					break;
				}
				case (AppSettingsActions)3:
				case (AppSettingsActions)4:
				case AppSettingsActions.Reset:
					break;
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebApplicationSetting.cs", 140);
			}
		}
	}

	public void Dispose()
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
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebApplicationSetting.cs", 155);
		}
	}
}
