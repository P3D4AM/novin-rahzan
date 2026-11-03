using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.BusinessLayer;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.MetaData;
using PacketParser.EventArgs;

namespace PacketParser.Entities;

[Serializable]
public class WebAghsatShakhsi : Serializable<WebAghsatShakhsi>, IAghsatShakhsi, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IReceiver, ISyncFromClient2Server<WebAghsatShakhsi>, IDeliverableEntity, IEditableDelivery, INpDisposable, IDisposable
{
	public PardakhtChequeShMetaData MetaData { get; set; }

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	public int ID2 { get; set; }

	public Guid TafsilGuid { get; set; }

	public Guid MoeinGuid { get; set; }

	public HesabType HType { get; set; }

	public string Description { get; set; }

	public DateTime DateSarResid { get; set; }

	public DateTime DateM { get; set; }

	public Guid UserGuid { get; set; }

	public Guid? PardakhtGuid { get; set; }

	public DateTime? PardakhtDateM { get; set; }

	public decimal PardakhtSum { get; set; }

	public decimal Price { get; set; }

	public DateTime ResetTime { get; set; }

	public DateTime? DateUtc { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public new bool IsDisposed { get; private set; }

	public EntityChangeSt ChangeStatus { get; set; }

	public ENSource Source { get; set; }

	public event Func<object, AghsatShakhsiReadEventArgs, Task> OnReadRequest;

	public event Func<object, ConnectionEventArgs<WebAghsatShakhsi>, Task> OnReadResponse;

	public event Func<object, ConnectionEventArgs<WebAghsatShakhsi>, Task> OnSaveRequest;

	public event Func<object, SaveResponseEventArgs, Task> OnSaveResponse;

	public event Func<object, ConnectionEventArgs<WebAghsatShakhsi>, Task> OnSync;

	public event Func<object, ConnectionEventArgs<Guid>, Task> OnDeletedRequest;

	public static List<Packet> GenerateDeleteRequest(IEnumerable<Guid> aghsats, int partitionId)
	{
		using (new JaegerMonitor("GenerateDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatShakhsi.cs"))
		{
			try
			{
				if (aghsats == null || aghsats.Count() <= 0)
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
					result = aghsats.GeneratePackets(PacketType.AghsatShakhsi, 8, null, isSync: false, null, partitionId, deliveryRequst: true, "GenerateDeleteRequest");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatShakhsi.cs", 67);
				throw;
			}
		}
	}

	public static List<Packet> GenerateDeleteRequest(IEnumerable<WebAghsatShakhsi> aghsats, int partitionId)
	{
		using (new JaegerMonitor("GenerateDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatShakhsi.cs"))
		{
			try
			{
				if (aghsats == null || aghsats.Count() <= 0)
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
					result = aghsats.GeneratePackets(PacketType.AghsatShakhsi, 8, null, isSync: false, null, partitionId, deliveryRequst: true, "GenerateDeleteRequest");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatShakhsi.cs", 81);
				throw;
			}
		}
	}

	public static Packet GenerateReadRequest(int requestedUId, DateTime modified)
	{
		using (new JaegerMonitor("GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatShakhsi.cs"))
		{
			try
			{
				List<string> list = new List<string> { ((short)5).ToString() };
				AghsatShakhsiReadEventArgs aghsatShakhsiReadEventArgs = new AghsatShakhsiReadEventArgs
				{
					Modified = modified
				};
				list.Add(aghsatShakhsiReadEventArgs.ToString());
				return new Packet(PacketType.AghsatShakhsi, 5, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatShakhsi.cs", 105);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReadResponse(IEnumerable<WebAghsatShakhsi> items, int partitionId, bool isSync = false)
	{
		if (items == null || items.Count() <= 0)
		{
			throw new NullReferenceException();
		}
		using (new JaegerMonitor("GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatShakhsi.cs"))
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
					result = items.GeneratePackets(PacketType.AghsatShakhsi, 6, null, isSync, null, partitionId, deliveryRequst: true, "GenerateReadResponse");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatShakhsi.cs", 121);
				return null;
			}
		}
	}

	public static List<Packet> GenerateSaveRequest(IEnumerable<WebAghsatShakhsi> aghsatShakhsis, int partitionId)
	{
		using (new JaegerMonitor("GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatShakhsi.cs"))
		{
			try
			{
				if (aghsatShakhsis != null && !aghsatShakhsis.Any())
				{
					throw new NullReferenceException();
				}
				object result;
				if (aghsatShakhsis == null)
				{
					result = null;
				}
				else
				{
					result = aghsatShakhsis.GeneratePackets(PacketType.AghsatShakhsi, 1, null, isSync: false, null, partitionId, deliveryRequst: false, "GenerateSaveRequest");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatShakhsi.cs", 136);
				throw;
			}
		}
	}

	public static List<Packet> GenerateSaveResponse(IEnumerable<SaveResponse> responses)
	{
		using (new JaegerMonitor("GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatShakhsi.cs"))
		{
			try
			{
				if (responses == null || responses.Count() <= 0)
				{
					throw new NullReferenceException();
				}
				return responses?.GeneratePackets(PacketType.AghsatShakhsi, 2, null, isSync: false, null, 0, deliveryRequst: false, "GenerateSaveResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatShakhsi.cs", 151);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReset(IEnumerable<WebAghsatShakhsi> aghsatShakhsis)
	{
		using (new JaegerMonitor("GenerateReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatShakhsi.cs"))
		{
			try
			{
				if (aghsatShakhsis == null || aghsatShakhsis.Count() <= 0)
				{
					throw new NullReferenceException();
				}
				return aghsatShakhsis.GeneratePackets(PacketType.AghsatShakhsi, 7, null, isSync: false, null, 0, deliveryRequst: true, "GenerateReset");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatShakhsi.cs", 167);
				return null;
			}
		}
	}

	public string GetPacketActionEnumName(short action)
	{
		try
		{
			AghsatShakhsiActions aghsatShakhsiActions = (AghsatShakhsiActions)action;
			return aghsatShakhsiActions.ToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionEnumName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatShakhsi.cs", 182);
			return $"Error({action})";
		}
	}

	public string GetPacketActionName(short action)
	{
		try
		{
			return ((AghsatShakhsiActions)action).GetDisplay();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatShakhsi.cs", 195);
			return $"Error({action})";
		}
	}

	public async Task DataReceived(Packet pack, string connectionUniqueId, ConnectionData connection)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatShakhsi.cs"))
		{
			_ = 5;
			try
			{
				switch ((AghsatShakhsiActions)pack.PacketActions)
				{
				case AghsatShakhsiActions.SaveRequest:
					if (this.OnSaveRequest != null)
					{
						ConnectionEventArgs<WebAghsatShakhsi> args5 = new ConnectionEventArgs<WebAghsatShakhsi>(pack, connectionUniqueId, connection);
						await Utilities.RaiseEvent(this.OnSaveRequest, this, args5, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatShakhsi.cs");
					}
					break;
				case AghsatShakhsiActions.SaveResponse:
					if (this.OnSaveResponse != null)
					{
						SaveResponseEventArgs args4 = new SaveResponseEventArgs(pack, connectionUniqueId, connection);
						await Utilities.RaiseEvent(this.OnSaveResponse, this, args4, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatShakhsi.cs");
					}
					break;
				case AghsatShakhsiActions.ReadRequest:
					if (this.OnReadRequest != null)
					{
						AghsatShakhsiReadEventArgs args2 = new AghsatShakhsiReadEventArgs
						{
							ConnectionUniqueId = connectionUniqueId,
							Connection = connection
						};
						await Utilities.RaiseEvent(this.OnReadRequest, this, args2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatShakhsi.cs");
					}
					break;
				case AghsatShakhsiActions.ReadResponse:
				{
					if (this.OnReadResponse == null && this.OnSync == null)
					{
						break;
					}
					ConnectionEventArgs<WebAghsatShakhsi> args3 = new ConnectionEventArgs<WebAghsatShakhsi>(pack, connectionUniqueId, connection);
					try
					{
						if (pack.IsSynced)
						{
							await Utilities.RaiseEvent(this.OnSync, this, args3, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatShakhsi.cs");
						}
						else
						{
							await Utilities.RaiseEvent(this.OnReadResponse, this, args3, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatShakhsi.cs");
						}
						break;
					}
					catch (Exception ex2)
					{
						WebErrorLog.Instance.StartLog(ex2, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatShakhsi.cs", 238);
						break;
					}
				}
				case AghsatShakhsiActions.DeleteRequest:
				{
					if (this.OnDeletedRequest == null)
					{
						break;
					}
					ConnectionEventArgs<Guid> args = new ConnectionEventArgs<Guid>(pack, connectionUniqueId, connection);
					try
					{
						await Utilities.RaiseEvent(this.OnDeletedRequest, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatShakhsi.cs");
						break;
					}
					catch (Exception ex)
					{
						WebErrorLog.Instance.StartLog(ex, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatShakhsi.cs", 250);
						break;
					}
				}
				case (AghsatShakhsiActions)3:
				case (AghsatShakhsiActions)4:
				case AghsatShakhsiActions.Reset:
					break;
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex4)
			{
				WebErrorLog.Instance.StartLog(ex4, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatShakhsi.cs", 257);
			}
		}
	}

	public Task<ReturnedSaveFuncInfo> SetDeliveryAsync(WebDelivery del, CancellationToken token, short tryCount, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token, short tryCount, string tranName)
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
				if (this.OnDeletedRequest != null)
				{
					this.OnDeletedRequest = null;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatShakhsi.cs", 298);
		}
	}

	public Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token, string tranName, short maxParalelTask)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> Send2ServerAsync(CancellationToken token, EnSystem system, Priority priority)
	{
		throw new NotImplementedException();
	}
}
