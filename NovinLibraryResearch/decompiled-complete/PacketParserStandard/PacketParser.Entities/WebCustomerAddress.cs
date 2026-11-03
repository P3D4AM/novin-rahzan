using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs.Customer;

namespace PacketParser.Entities;

public class WebCustomerAddress : Serializable<WebCustomerAddress>, ICustomerAddress, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, INpDisposable, IDisposable, IReceiver
{
	public short Id { get; set; }

	public short State { get; set; }

	public short City { get; set; }

	public string Address { get; set; }

	public string PostalCode { get; set; }

	public string FullName { get; set; }

	public string Phone { get; set; }

	public string CellPhone { get; set; }

	public string NationalCode { get; set; }

	public Guid Guid { get; set; } = Guid.NewGuid();


	public DateTime Modified { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	[XmlIgnore]
	[JsonIgnore]
	public new bool IsDisposed { get; private set; }

	public event Func<object, CustomerAddressSaveResponseEventArgs, Task> OnSaveResponse;

	public event Func<object, CustomerAddressSaveRequestEventArgs, Task> OnSaveRequest;

	public static Packet GenerateSaveRequest(WebCustomerAddress address)
	{
		using (new JaegerMonitor("GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCustomerAddress.cs"))
		{
			try
			{
				if (address == null)
				{
					throw new ArgumentNullException("address");
				}
				List<string> list = new List<string> { ((short)0).ToString() };
				return new Packet(PacketType.CustomerAddress, 0, Json.ToBytesJson(address), list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCustomerAddress.cs", 72);
				throw;
			}
		}
	}

	public static Packet GenerateSaveResponse(bool success)
	{
		using (new JaegerMonitor("GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCustomerAddress.cs"))
		{
			try
			{
				List<string> list = new List<string>
				{
					((short)0).ToString(),
					new CustomerAddressSaveResponseEventArgs
					{
						Success = success
					}.ToString()
				};
				return new Packet(PacketType.CustomerAddress, 0, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCustomerAddress.cs", 91);
				throw;
			}
		}
	}

	public async Task DataReceived(Packet pack, string connectionUniqueId, ConnectionData connection)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCustomerAddress.cs"))
		{
			_ = 1;
			try
			{
				switch ((CusAddressAction)pack.PacketActions)
				{
				case CusAddressAction.SaveRequest:
				{
					WebCustomerAddress customerAddress = Json.FromJson<WebCustomerAddress>(pack.PacketContents, sendErrorIfException: true, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCustomerAddress.cs", 107);
					CustomerAddressSaveRequestEventArgs args = new CustomerAddressSaveRequestEventArgs
					{
						ConnectionUniqueId = connectionUniqueId,
						Connection = connection,
						CustomerAddress = customerAddress,
						PacketReferenceId = Guid.NewGuid(),
						Part = pack.Part,
						TotalParts = pack.TotalParts
					};
					await Utilities.RaiseEvent(this.OnSaveRequest, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCustomerAddress.cs");
					break;
				}
				case CusAddressAction.SaveResponse:
				{
					CustomerAddressSaveResponseEventArgs customerAddressSaveResponseEventArgs = pack.Args[1].FromJson<CustomerAddressSaveResponseEventArgs>("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCustomerAddress.cs", 120);
					customerAddressSaveResponseEventArgs.ConnectionUniqueId = connectionUniqueId;
					customerAddressSaveResponseEventArgs.Connection = connection;
					customerAddressSaveResponseEventArgs.Part = pack.Part;
					customerAddressSaveResponseEventArgs.TotalParts = pack.TotalParts;
					customerAddressSaveResponseEventArgs.PacketReferenceId = pack.PacketReferenceId;
					await Utilities.RaiseEvent(this.OnSaveResponse, this, customerAddressSaveResponseEventArgs, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCustomerAddress.cs");
					break;
				}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCustomerAddress.cs", 132);
			}
		}
	}

	public string GetPacketActionEnumName(short action)
	{
		throw new NotImplementedException();
	}

	public string GetPacketActionName(short action)
	{
		throw new NotImplementedException();
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCustomerAddress.cs", 162);
		}
	}
}
