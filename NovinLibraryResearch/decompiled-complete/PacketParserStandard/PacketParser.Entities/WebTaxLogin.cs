using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.EventArgs;
using PacketParser.EventArgs.TaxService;

namespace PacketParser.Entities;

public class WebTaxLogin : Serializable<WebTaxLogin>, IReceiver
{
	[PersianNameAttribute.PersianName("شناسه کلاینت")]
	public string ClientId { get; set; }

	[PersianNameAttribute.PersianName("ادرس پایه")]
	public string BaseUrl { get; set; }

	[PersianNameAttribute.PersianName("ورژن Api")]
	public string ApiVersion { get; set; }

	[PersianNameAttribute.PersianName("کلید عمومی")]
	public string PublicKey { get; set; }

	[PersianNameAttribute.PersianName("کلید خصوصی")]
	public string PrivateKey { get; set; }

	[PersianNameAttribute.PersianName("نوع توکن")]
	public EnTaxTokenType TokenType { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public new bool IsDisposed { get; private set; }

	public event Func<object, ConnectionEventArgs<WebTaxLogin>, Task> OnLoginRequest;

	public event Func<object, TaxLoginEventArgs, Task> OnLoginResponse;

	public static Packet GenerateLoginRequest(WebTaxLogin config)
	{
		using (new JaegerMonitor("GenerateLoginRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Accounting\\TaxService\\WebTaxLogin.cs"))
		{
			try
			{
				if (config == null)
				{
					throw new NullReferenceException();
				}
				return config.GeneratePacket(PacketType.TaxLogin, 1, null, isSync: false, null, 0, deliveryRequst: false, "GenerateLoginRequest");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateLoginRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Accounting\\TaxService\\WebTaxLogin.cs", 35);
				throw;
			}
		}
	}

	public static Packet GenerateLoginResponse(TaxLoginEventArgs response)
	{
		using (new JaegerMonitor("GenerateLoginResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Accounting\\TaxService\\WebTaxLogin.cs"))
		{
			try
			{
				if (response == null)
				{
					throw new NullReferenceException();
				}
				return response.GeneratePacket(PacketType.TaxLogin, 2, null, isSync: false, null, 0, deliveryRequst: false, "GenerateLoginResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateLoginResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Accounting\\TaxService\\WebTaxLogin.cs", 49);
				throw;
			}
		}
	}

	public string GetPacketActionEnumName(short action)
	{
		TaxLoginActions taxLoginActions = (TaxLoginActions)action;
		return taxLoginActions.ToString();
	}

	public string GetPacketActionName(short action)
	{
		return ((TaxLoginActions)action).GetDisplay();
	}

	public async Task DataReceived(Packet pack, string ConnectionUniqueId, ConnectionData Connection)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Accounting\\TaxService\\WebTaxLogin.cs"))
		{
			_ = 1;
			try
			{
				switch ((TaxLoginActions)pack.PacketActions)
				{
				case TaxLoginActions.LoginRequest:
				{
					ConnectionEventArgs<WebTaxLogin> args = new ConnectionEventArgs<WebTaxLogin>(pack, ConnectionUniqueId, Connection);
					await Utilities.RaiseEvent(this.OnLoginRequest, this, args, Connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Accounting\\TaxService\\WebTaxLogin.cs");
					break;
				}
				case TaxLoginActions.LoginResponse:
				{
					List<TaxLoginEventArgs> list = Json.FromJson<List<TaxLoginEventArgs>>(pack.PacketContents, sendErrorIfException: true, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Accounting\\TaxService\\WebTaxLogin.cs", 68);
					if (list != null && list.Any())
					{
						await Utilities.RaiseEvent<TaxLoginEventArgs>(this.OnLoginResponse, this, list.FirstOrDefault(), Connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Accounting\\TaxService\\WebTaxLogin.cs");
					}
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
				WebErrorLog.Instance.StartLog(ex2, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Accounting\\TaxService\\WebTaxLogin.cs", 75);
			}
		}
	}

	public WebTaxLogin Clone()
	{
		return (WebTaxLogin)MemberwiseClone();
	}

	public override void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				if (this.OnLoginResponse != null)
				{
					this.OnLoginResponse = null;
				}
				if (this.OnLoginRequest != null)
				{
					this.OnLoginRequest = null;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Accounting\\TaxService\\WebTaxLogin.cs", 88);
		}
	}
}
