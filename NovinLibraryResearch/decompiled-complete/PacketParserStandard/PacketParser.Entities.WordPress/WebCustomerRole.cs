using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;
using PacketParser.EventArgs;

namespace PacketParser.Entities.WordPress;

public class WebCustomerRole : Serializable<WebCustomerRole>, ICustomerRole, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IReceiver, INpDisposable, IDisposable, IDeliverableEntity
{
	[XmlIgnore]
	[JsonIgnore]
	public new bool IsDisposed { get; private set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public Guid Guid { get; set; }

	public string WordPressName { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public Dictionary<string, string> MetaData { get; set; }

	[Obsolete]
	public EnPriceType PriceType { get; set; }

	public Guid? PriceTypeGuid { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public event Func<object, CusTitleReadEventArgs, Task> OnReadRequest;

	public event Func<object, ConnectionEventArgs<WebCustomerRole>, Task> OnReadResponse;

	public event Func<object, ConnectionEventArgs<WebCustomerRole>, Task> OnSaveRequest;

	public event Func<object, SaveResponseEventArgs, Task> OnSaveResponse;

	public event Func<object, ConnectionEventArgs<WebCustomerRole>, Task> OnSync;

	public async Task DataReceived(Packet pack, string ConnectionUniqueId, ConnectionData connection)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WordPress\\WebCustomerRole.cs"))
		{
			_ = 4;
			try
			{
				switch ((CustomerRoleActions)pack.PacketActions)
				{
				case CustomerRoleActions.SaveRequest:
				{
					ConnectionEventArgs<WebCustomerRole> args3 = new ConnectionEventArgs<WebCustomerRole>(pack, ConnectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnSaveRequest, this, args3, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WordPress\\WebCustomerRole.cs");
					break;
				}
				case CustomerRoleActions.SaveResponse:
				{
					SaveResponseEventArgs args2 = new SaveResponseEventArgs(pack, ConnectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnSaveResponse, this, args2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WordPress\\WebCustomerRole.cs");
					break;
				}
				case CustomerRoleActions.ReadRequest:
				{
					CusTitleReadEventArgs cusTitleReadEventArgs = pack.Args[1].FromJson<CusTitleReadEventArgs>("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WordPress\\WebCustomerRole.cs", 56);
					cusTitleReadEventArgs.ConnectionUniqueId = ConnectionUniqueId;
					cusTitleReadEventArgs.Connection = connection;
					await Utilities.RaiseEvent(this.OnReadRequest, this, cusTitleReadEventArgs, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WordPress\\WebCustomerRole.cs");
					break;
				}
				case CustomerRoleActions.ReadResponse:
				{
					ConnectionEventArgs<WebCustomerRole> args = new ConnectionEventArgs<WebCustomerRole>(pack, ConnectionUniqueId, connection);
					if (pack.IsSynced)
					{
						await Utilities.RaiseEvent(this.OnSync, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WordPress\\WebCustomerRole.cs");
					}
					else
					{
						await Utilities.RaiseEvent(this.OnReadResponse, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WordPress\\WebCustomerRole.cs");
					}
					break;
				}
				case (CustomerRoleActions)3:
				case (CustomerRoleActions)4:
				case CustomerRoleActions.Reset:
					break;
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WordPress\\WebCustomerRole.cs", 80);
			}
		}
	}

	public string GetPacketActionEnumName(short action)
	{
		CustomerRoleActions customerRoleActions = (CustomerRoleActions)action;
		return customerRoleActions.ToString();
	}

	public string GetPacketActionName(short action)
	{
		return ((CustomerRoleActions)action).GetDisplay();
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
				if (this.OnSync != null)
				{
					this.OnSync = null;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WordPress\\WebCustomerRole.cs", 98);
		}
	}

	public static Packet GenerateReadRequest(DateTime modified)
	{
		using (new JaegerMonitor("GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WordPress\\WebCustomerRole.cs"))
		{
			try
			{
				List<string> list = new List<string> { ((short)5).ToString() };
				CusTitleReadEventArgs cusTitleReadEventArgs = new CusTitleReadEventArgs
				{
					Modified = modified
				};
				list.Add(cusTitleReadEventArgs.ToString());
				return new Packet(PacketType.CustomerRole, 5, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WordPress\\WebCustomerRole.cs", 112);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReadResponse(IEnumerable<WebCustomerRole> items, bool isSync, int partitionId = 0)
	{
		if (items == null || items.Count() <= 0)
		{
			throw new NullReferenceException();
		}
		using (new JaegerMonitor("GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WordPress\\WebCustomerRole.cs"))
		{
			try
			{
				if (items == null || items.Count() <= 0)
				{
					throw new NullReferenceException();
				}
				object result;
				if (items == null)
				{
					result = null;
				}
				else
				{
					result = items.GeneratePackets(PacketType.CustomerRole, 6, null, isSync, null, partitionId, deliveryRequst: true, "GenerateReadResponse");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WordPress\\WebCustomerRole.cs", 127);
				return null;
			}
		}
	}

	public static List<Packet> GenerateSaveRequest(IEnumerable<WebCustomerRole> titles, int partitionId = 0)
	{
		using (new JaegerMonitor("GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WordPress\\WebCustomerRole.cs"))
		{
			try
			{
				if (titles == null || !titles.Any())
				{
					throw new NullReferenceException();
				}
				string text = titles.Select((WebCustomerRole p) => $"{p.Guid}/{p.Name}").GenerateDebugDescription(5);
				object result;
				if (titles == null)
				{
					result = null;
				}
				else
				{
					string extraDataForDebug = text;
					result = titles.GeneratePackets(PacketType.CustomerRole, 1, null, isSync: false, null, partitionId, deliveryRequst: false, extraDataForDebug);
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WordPress\\WebCustomerRole.cs", 142);
				throw;
			}
		}
	}

	public static List<Packet> GenerateSaveResponse(IEnumerable<SaveResponse> responses)
	{
		using (new JaegerMonitor("GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WordPress\\WebCustomerRole.cs"))
		{
			try
			{
				if (responses == null || responses.Count() <= 0)
				{
					throw new NullReferenceException();
				}
				string text = responses.Select((SaveResponse p) => $"Guid:{p.Guid}/Success:{p.SaveResultStatus}/{p.Message}").GenerateDebugDescription(5);
				object result;
				if (responses == null)
				{
					result = null;
				}
				else
				{
					string extraDataForDebug = text;
					result = responses.GeneratePackets(PacketType.CustomerRole, 2, null, isSync: false, null, 0, deliveryRequst: false, extraDataForDebug);
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WordPress\\WebCustomerRole.cs", 157);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReset(IEnumerable<WebCustomerRole> titles)
	{
		using (new JaegerMonitor("GenerateReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WordPress\\WebCustomerRole.cs"))
		{
			try
			{
				return titles?.GeneratePackets(PacketType.CustomerRole, 7, null, isSync: false, null, 0, deliveryRequst: true, "GenerateReset");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WordPress\\WebCustomerRole.cs", 170);
				throw;
			}
		}
	}
}
