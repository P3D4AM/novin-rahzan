using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Aghsat;
using PacketParser.EventArgs;

namespace PacketParser.Entities;

[Serializable]
public class WebAghsatMGroup : Serializable<WebAghsatMGroup>, IAghsatM_Group, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IReceiver, INpDisposable, IDisposable
{
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	public Guid TafsilGuid { get; set; }

	public Guid MoeinGuid { get; set; }

	public Guid CashierGuid { get; set; }

	public decimal Sum { get; set; }

	public int Count { get; set; }

	public int? SanadNo { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public new bool IsDisposed { get; private set; }

	public decimal ProfitSum { get; set; }

	public Guid? CreditMoeinGuid { get; set; }

	public Guid? CreditTafsilGuid { get; set; }

	public event Func<object, AghsatAshkhasReadEventArgs, Task> OnReadRequest;

	public event Func<object, ConnectionEventArgs<WebAghsatMGroup>, Task> OnReadResponse;

	public event Func<object, ConnectionEventArgs<WebAghsatMGroup>, Task> OnSaveRequest;

	public event Func<object, SaveResponseEventArgs, Task> OnSaveResponse;

	public event Func<object, ConnectionEventArgs<WebAghsatMGroup>, Task> OnSync;

	public static Packet GenerateReadRequest(int requestedUId, DateTime modified)
	{
		using (new JaegerMonitor("GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatMGroup.cs"))
		{
			try
			{
				List<string> list = new List<string> { ((short)5).ToString() };
				AghsatAshkhasReadEventArgs aghsatAshkhasReadEventArgs = new AghsatAshkhasReadEventArgs
				{
					Modified = modified
				};
				list.Add(aghsatAshkhasReadEventArgs.ToString());
				return new Packet(PacketType.AghsatMGroup, 5, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatMGroup.cs", 58);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReadResponse(IEnumerable<WebAghsatMGroup> items, int partitionId, bool isSync = false)
	{
		if (items == null || !items.Any())
		{
			throw new NullReferenceException();
		}
		using (new JaegerMonitor("GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatMGroup.cs"))
		{
			try
			{
				object result;
				if (items == null)
				{
					result = null;
				}
				else
				{
					result = items.GeneratePackets(PacketType.AghsatMGroup, 6, null, isSync, null, partitionId, deliveryRequst: true, "GenerateReadResponse");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatMGroup.cs", 72);
				return null;
			}
		}
	}

	public static List<Packet> GenerateSaveRequest(IEnumerable<WebAghsatMGroup> aghsats, int partitionId = 0, string debuggerInfo = "")
	{
		using (new JaegerMonitor("GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatMGroup.cs"))
		{
			try
			{
				if (aghsats == null || !aghsats.Any())
				{
					throw new NullReferenceException();
				}
				object result;
				if (aghsats == null)
				{
					result = null;
				}
				else
				{
					result = aghsats.GeneratePackets(PacketType.AghsatMGroup, 1, null, isSync: false, null, partitionId, deliveryRequst: false, debuggerInfo);
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatMGroup.cs", 86);
				throw;
			}
		}
	}

	public static List<Packet> GenerateSaveResponse(IEnumerable<SaveResponse> responses)
	{
		using (new JaegerMonitor("GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatMGroup.cs"))
		{
			try
			{
				if (responses == null || responses.Count() <= 0)
				{
					throw new NullReferenceException();
				}
				return responses?.GeneratePackets(PacketType.AghsatMGroup, 2, null, isSync: false, null, 0, deliveryRequst: false, "GenerateSaveResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatMGroup.cs", 100);
				throw;
			}
		}
	}

	public string GetPacketActionEnumName(short action)
	{
		AghsatMGroupActions aghsatMGroupActions = (AghsatMGroupActions)action;
		return aghsatMGroupActions.ToString();
	}

	public string GetPacketActionName(short action)
	{
		return ((AghsatMGroupActions)action).GetDisplay();
	}

	public async Task DataReceived(Packet pack, string connectionUniqueId, ConnectionData connection)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatMGroup.cs"))
		{
			_ = 4;
			try
			{
				switch ((AghsatMGroupActions)pack.PacketActions)
				{
				case AghsatMGroupActions.SaveRequest:
					if (this.OnSaveRequest != null)
					{
						pack.ExteraDataForDebug += "/ Received Packet !!!!";
						ConnectionEventArgs<WebAghsatMGroup> args3 = new ConnectionEventArgs<WebAghsatMGroup>(pack, connectionUniqueId, connection);
						await Utilities.RaiseEvent(this.OnSaveRequest, this, args3, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatMGroup.cs");
					}
					break;
				case AghsatMGroupActions.SaveResponse:
					if (this.OnSaveResponse != null)
					{
						SaveResponseEventArgs args2 = new SaveResponseEventArgs(pack, connectionUniqueId, connection);
						await Utilities.RaiseEvent(this.OnSaveResponse, this, args2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatMGroup.cs");
					}
					break;
				case AghsatMGroupActions.ReadRequest:
					if (this.OnReadRequest != null)
					{
						AghsatAshkhasReadEventArgs args4 = new AghsatAshkhasReadEventArgs
						{
							ConnectionUniqueId = connectionUniqueId,
							Connection = connection
						};
						await Utilities.RaiseEvent(this.OnReadRequest, this, args4, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatMGroup.cs");
					}
					break;
				case AghsatMGroupActions.ReadResponse:
				{
					if (this.OnReadResponse == null && this.OnSync == null)
					{
						break;
					}
					ConnectionEventArgs<WebAghsatMGroup> args = new ConnectionEventArgs<WebAghsatMGroup>(pack, connectionUniqueId, connection);
					try
					{
						if (pack.IsSynced)
						{
							await Utilities.RaiseEvent(this.OnSync, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatMGroup.cs");
						}
						else
						{
							await Utilities.RaiseEvent(this.OnReadResponse, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatMGroup.cs");
						}
						break;
					}
					catch (Exception ex)
					{
						WebErrorLog.Instance.StartLog(ex, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatMGroup.cs", 144);
						break;
					}
				}
				case (AghsatMGroupActions)3:
				case (AghsatMGroupActions)4:
					break;
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex3)
			{
				WebErrorLog.Instance.StartLog(ex3, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatMGroup.cs", 150);
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
				if (this.OnSync != null)
				{
					this.OnSync = null;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatMGroup.cs", 165);
		}
	}
}
