using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PacketParser.Entities.DepartmentEntitySummery;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;
using Serilog;

namespace PacketParser.Entities;

public class LoginExtention : Serializable<LoginExtention>, ILoginExtention, INpDisposable, IDisposable, IReceiver
{
	private class NestedLogin
	{
		internal static readonly LoginExtention instance;

		static NestedLogin()
		{
			instance = new LoginExtention();
		}
	}

	public ENSource Source { get; set; }

	public Guid DbGuid { get; set; } = Guid.Empty;


	public string CpuSerial { get; set; } = "";


	public Guid CashierGuid { get; set; } = Guid.Empty;


	[Obsolete("لطفا از CpuSerial استفاده کنید", true)]
	public string HardSerial { get; set; } = "";


	public long AndroidIme { get; set; }

	public long LkSerial { get; set; }

	public WebUserInformation UserInfo { get; set; }

	public string Username { get; set; }

	public string Password { get; set; }

	public string Version { get; set; }

	public string CustomerUsername { get; set; } = "";


	public string CustomerPassword { get; set; } = "";


	public bool SyncRequested { get; set; } = true;


	[DefaultValue(0)]
	public ENCompressedMode CompressedMode { get; set; }

	public short ClientVersion { get; set; }

	public DateTime? ClientTime { get; set; } = DateTime.Now;


	public string LoginResultMessage { get; set; }

	public EnLoginResult Status { get; set; }

	public LogLevel MinLogLevel { get; set; }

	public WebDepartmentEntitySummery DepartmentEntitySummery { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public new bool IsDisposed { get; private set; }

	public string ClientContextGuid { get; set; } = "";


	[DefaultValue(EncryptionType.Custom)]
	public EncryptionType Encryption { get; set; } = EncryptionType.Custom;


	public event Func<object, LoginRequestEventArgs, Task> OnLoginRequest;

	public event Func<object, LoginResponseEventArgs, Task> OnLoginResponse;

	public event Func<object, LoginResponseProcessedEventArgs, Task> OnLoginResponseProcessed;

	public LoginExtention(EnLoginResult status, string loginResultMessage, WebUserInformation userInfo, string customerUsername, string customerPassword, Guid cashierGuid, string clientContextGuid)
	{
		Status = status;
		LoginResultMessage = loginResultMessage;
		UserInfo = userInfo;
		CustomerUsername = customerUsername;
		CustomerPassword = customerPassword;
		CashierGuid = cashierGuid;
		ClientContextGuid = clientContextGuid;
	}

	public LoginExtention()
	{
	}

	public Packet GenerateLoginStatusPacket(WebContextHolder context)
	{
		using (new JaegerMonitor("GenerateLoginStatusPacket", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\LoginExtention.cs"))
		{
			Packet result = null;
			try
			{
				List<string> list = new List<string> { ((short)2).ToString() };
				LoginResponseEventArgs item = new LoginResponseEventArgs
				{
					Status = Status,
					LoginResultMessage = LoginResultMessage,
					UserInfo = UserInfo,
					CustomerUserName = CustomerUsername,
					CustomerPassword = CustomerPassword,
					Context = context
				};
				result = new Packet(PacketType.Login, 2, Json.ToBytesJson(item), list.ToArray(), 0, 1, Guid.NewGuid());
				return result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateLoginStatusPacket", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\LoginExtention.cs", 80);
				return result;
			}
		}
	}

	public static Packet GenerateLoginPacket(LoginExtention item)
	{
		using (new JaegerMonitor("GenerateLoginPacket", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\LoginExtention.cs"))
		{
			try
			{
				List<string> list = new List<string> { ((short)1).ToString() };
				return new Packet(PacketType.Login, 1, Json.ToBytesJson(item), list.ToArray(), 0, 1, Guid.NewGuid())
				{
					DeliveryRequested = false
				};
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateLoginPacket", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\LoginExtention.cs", 95);
				throw;
			}
		}
	}

	public static Packet GenerateOnLoginResponseProcessedPacket()
	{
		using (new JaegerMonitor("GenerateOnLoginResponseProcessedPacket", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\LoginExtention.cs"))
		{
			try
			{
				List<string> list = new List<string> { ((short)3).ToString() };
				return new Packet(PacketType.Login, 3, null, list.ToArray(), 0, 1, Guid.NewGuid())
				{
					DeliveryRequested = false
				};
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateOnLoginResponseProcessedPacket", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\LoginExtention.cs", 110);
				throw;
			}
		}
	}

	public async Task DataReceived(Packet pack, WebContextHolder context, string ConnectionUniqueId, ConnectionData connection)
	{
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\LoginExtention.cs"))
		{
			_ = 2;
			try
			{
				connection.Context = context;
				if (pack == null || pack.Args == null)
				{
					return;
				}
				switch (GetAction(pack.Args))
				{
				case LoginActions.LoginRequest:
				{
					LoginRequestEventArgs args2 = new LoginRequestEventArgs
					{
						ConnectionData = Json.FromJson<LoginExtention>(pack.PacketContents, sendErrorIfException: true, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\LoginExtention.cs", 126),
						ConnectionUniqueId = ConnectionUniqueId,
						Context = context,
						Ip = connection.Ip
					};
					await Utilities.RaiseEvent(this.OnLoginRequest, this, args2, connection?.cancellationToken ?? default(CancellationToken), 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\LoginExtention.cs");
					break;
				}
				case LoginActions.LoginResponse:
				{
					LoginResponseEventArgs loginResponseEventArgs = Json.FromJson<LoginResponseEventArgs>(pack.PacketContents, sendErrorIfException: true, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\LoginExtention.cs", 134);
					if (loginResponseEventArgs == null)
					{
						throw new Exception("Arg2 is null");
					}
					loginResponseEventArgs.connection = connection;
					loginResponseEventArgs.Context = context;
					loginResponseEventArgs.ConnectionUniqueId = ConnectionUniqueId;
					Log.Debug("Login data received!");
					await Utilities.RaiseEvent(this.OnLoginResponse, this, loginResponseEventArgs, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\LoginExtention.cs");
					break;
				}
				case LoginActions.LoginResponseProcessed:
				{
					LoginResponseProcessedEventArgs args = new LoginResponseProcessedEventArgs
					{
						ConnectionUniqueId = ConnectionUniqueId,
						connection = connection
					};
					Log.Debug("Login data processed!");
					await Utilities.RaiseEvent(this.OnLoginResponseProcessed, this, args, connection?.cancellationToken ?? default(CancellationToken), 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\LoginExtention.cs");
					break;
				}
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "{@line},packet content is null:{@isnull}" + $"{pack.PacketContents == null}", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\LoginExtention.cs", 154);
			}
		}
	}

	private static LoginActions GetAction(string[] args)
	{
		using (new JaegerMonitor("GetAction", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\LoginExtention.cs"))
		{
			try
			{
				if (args == null || args.Length == 0 || string.IsNullOrEmpty(args[0]))
				{
					return LoginActions.Unknown;
				}
				return (LoginActions)short.Parse(args[0]);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAction", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\LoginExtention.cs", 167);
				return LoginActions.Unknown;
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
				if (this.OnLoginRequest != null)
				{
					this.OnLoginRequest = null;
				}
				if (this.OnLoginResponse != null)
				{
					this.OnLoginResponse = null;
				}
				if (this.OnLoginResponseProcessed != null)
				{
					this.OnLoginResponseProcessed = null;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\LoginExtention.cs", 182);
		}
	}

	public Task DataReceived(Packet pack, string ConnectionUniqueId, ConnectionData Connection)
	{
		throw new NotImplementedException();
	}

	public string GetPacketActionEnumName(short action)
	{
		LoginActions loginActions = (LoginActions)action;
		return loginActions.ToString();
	}

	public string GetPacketActionName(short action)
	{
		return ((LoginActions)action).GetDisplay();
	}
}
