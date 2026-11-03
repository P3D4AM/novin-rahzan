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
using PacketParser.EventArgs;

namespace PacketParser.Entities;

[Serializable]
public class WebPrdVahed : Serializable<WebPrdVahed>, IPrdVahed, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IReceiver, INpDisposable, IDisposable, ISyncFromClient2Server<WebPrdVahed>, IDeliverableEntity, IEditableDelivery
{
	private const int ChunckSizeKB = 100;

	public short Id { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	public DateTime Modified { get; set; }

	public Guid Guid { get; set; } = Guid.NewGuid();


	public bool DecimalSales { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public new bool IsDisposed { get; private set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public Guid? TaxUnitGuid { get; set; }

	public event Func<object, PrdVahedReadEventArgs, Task> OnReadRequest;

	public event Func<object, ConnectionEventArgs<WebPrdVahed>, Task> OnReadResponse;

	public event Func<object, ConnectionEventArgs<WebPrdVahed>, Task> OnSaveRequest;

	public event Func<object, SaveResponseEventArgs, Task> OnSaveResponse;

	public event Func<object, ConnectionEventArgs<WebPrdVahed>, Task> OnSync;

	public static Packet GenerateReadRequest(int requestedUId, DateTime modified)
	{
		using (new JaegerMonitor("GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdVahed.cs"))
		{
			try
			{
				List<string> list = new List<string> { ((short)5).ToString() };
				PrdVahedReadEventArgs prdVahedReadEventArgs = new PrdVahedReadEventArgs
				{
					Modified = modified
				};
				list.Add(prdVahedReadEventArgs.ToString());
				return new Packet(PacketType.ProductVahed, 5, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdVahed.cs", 69);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReadResponse(IEnumerable<WebPrdVahed> items, bool isSync = false)
	{
		if (items == null || items.Count() <= 0)
		{
			throw new NullReferenceException();
		}
		using (new JaegerMonitor("GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdVahed.cs"))
		{
			try
			{
				if (items == null || (items != null && items.Count() == 0))
				{
					throw new NullReferenceException();
				}
				return items?.GeneratePackets(PacketType.ProductVahed, 6, null, isSync, null, 0, deliveryRequst: true, "GenerateReadResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdVahed.cs", 86);
				return null;
			}
		}
	}

	public static List<Packet> GenerateSaveRequest(IEnumerable<WebPrdVahed> vaheds, int partitionId = 0)
	{
		using (new JaegerMonitor("GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdVahed.cs"))
		{
			try
			{
				if (vaheds == null || !vaheds.Any())
				{
					throw new NullReferenceException();
				}
				object result;
				if (vaheds == null)
				{
					result = null;
				}
				else
				{
					result = vaheds.GeneratePackets(PacketType.ProductVahed, 1, null, isSync: false, null, partitionId, deliveryRequst: false, "GenerateSaveRequest");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdVahed.cs", 101);
				throw;
			}
		}
	}

	public static List<Packet> GenerateSaveResponse(IEnumerable<SaveResponse> responses)
	{
		using (new JaegerMonitor("GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdVahed.cs"))
		{
			try
			{
				if (responses == null || (responses != null && responses.Count() == 0))
				{
					throw new NullReferenceException();
				}
				return responses?.GeneratePackets(PacketType.ProductVahed, 2, null, isSync: false, null, 0, deliveryRequst: false, "GenerateSaveResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdVahed.cs", 116);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReset(IEnumerable<WebPrdVahed> vaheds)
	{
		using (new JaegerMonitor("GenerateReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdVahed.cs"))
		{
			try
			{
				if (vaheds == null || (vaheds != null && vaheds.Count() == 0))
				{
					throw new NullReferenceException();
				}
				return vaheds.GeneratePackets(PacketType.ProductVahed, 7, null, isSync: false, null, 0, deliveryRequst: true, "GenerateReset");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdVahed.cs", 131);
				return null;
			}
		}
	}

	public string GetPacketActionName(short action)
	{
		try
		{
			return ((PrdVahedActions)action).GetDisplay();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdVahed.cs", 146);
			return $"Error({action})";
		}
	}

	public string GetPacketActionEnumName(short action)
	{
		try
		{
			PrdVahedActions prdVahedActions = (PrdVahedActions)action;
			return prdVahedActions.ToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionEnumName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdVahed.cs", 159);
			return $"Error({action})";
		}
	}

	public async Task DataReceived(Packet pack, string connectionUniqueId, ConnectionData connection)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdVahed.cs"))
		{
			_ = 4;
			try
			{
				switch ((PrdVahedActions)pack.PacketActions)
				{
				case PrdVahedActions.SaveRequest:
					if (this.OnSaveRequest != null)
					{
						ConnectionEventArgs<WebPrdVahed> args3 = new ConnectionEventArgs<WebPrdVahed>(pack, connectionUniqueId, connection);
						await Utilities.RaiseEvent(this.OnSaveRequest, this, args3, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdVahed.cs");
					}
					break;
				case PrdVahedActions.SaveResponse:
					if (this.OnSaveResponse != null)
					{
						SaveResponseEventArgs args2 = new SaveResponseEventArgs(pack, connectionUniqueId, connection);
						await Utilities.RaiseEvent(this.OnSaveResponse, this, args2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdVahed.cs");
					}
					break;
				case PrdVahedActions.ReadRequest:
					if (this.OnReadRequest != null)
					{
						PrdVahedReadEventArgs args4 = new PrdVahedReadEventArgs
						{
							ConnectionUniqueId = connectionUniqueId,
							Connection = connection
						};
						await Utilities.RaiseEvent(this.OnReadRequest, this, args4, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdVahed.cs");
					}
					break;
				case PrdVahedActions.ReadResponse:
				{
					if (this.OnReadResponse == null && this.OnSync == null)
					{
						break;
					}
					ConnectionEventArgs<WebPrdVahed> args = new ConnectionEventArgs<WebPrdVahed>(pack, connectionUniqueId, connection);
					try
					{
						if (pack.IsSynced)
						{
							await Utilities.RaiseEvent(this.OnSync, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdVahed.cs");
						}
						else
						{
							await Utilities.RaiseEvent(this.OnReadResponse, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdVahed.cs");
						}
						break;
					}
					catch (Exception ex)
					{
						WebErrorLog.Instance.StartLog(ex, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdVahed.cs", 202);
						break;
					}
				}
				case (PrdVahedActions)3:
				case (PrdVahedActions)4:
					break;
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex3)
			{
				WebErrorLog.Instance.StartLog(ex3, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdVahed.cs", 223);
			}
		}
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
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdVahed.cs", 239);
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

	public Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token, string tranName, short maxParalelTask)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> Send2ServerAsync(CancellationToken token, EnSystem system, Priority priority)
	{
		throw new NotImplementedException();
	}
}
