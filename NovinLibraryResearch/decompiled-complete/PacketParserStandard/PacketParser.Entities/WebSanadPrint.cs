using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;
using PacketParser.EventArgs;

namespace PacketParser.Entities;

public class WebSanadPrint : ISanadPrint, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	public Guid AsGuid { get; set; }

	public Document AsType { get; set; }

	public string Name { get; set; }

	public string Tel { get; set; }

	public string Address { get; set; }

	public string Mob { get; set; }

	public DateTime DateM { get; set; }

	public long MobileLong { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public bool IsDisposed { get; set; }

	public event Func<object, HesabMoeinReadEventArgs, Task> OnReadRequest;

	public event Func<object, ConnectionEventArgs<WebSanadPrint>, Task> OnReadResponse;

	public event Func<object, ConnectionEventArgs<WebSanadPrint>, Task> OnSaveRequest;

	public event Func<object, SaveResponseEventArgs, Task> OnSaveResponse;

	public event Func<object, ConnectionEventArgs<WebSanadPrint>, Task> OnSync;

	public static List<Packet> GenerateSaveRequest(IEnumerable<WebSanadPrint> webCoupon, int partitionId)
	{
		using (new JaegerMonitor("GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSanadPrint.cs"))
		{
			try
			{
				if (webCoupon == null || !webCoupon.Any())
				{
					throw new NullReferenceException();
				}
				object result;
				if (webCoupon == null)
				{
					result = null;
				}
				else
				{
					result = webCoupon.GeneratePackets(PacketType.SanadPrint, 1, null, isSync: false, null, partitionId, deliveryRequst: false, "GenerateSaveRequest");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSanadPrint.cs", 51);
				throw;
			}
		}
	}

	public static List<Packet> GenerateSaveResponse(IEnumerable<SaveResponse> responses)
	{
		using (new JaegerMonitor("GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSanadPrint.cs"))
		{
			try
			{
				if (responses == null || !responses.Any())
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
					result = responses.GeneratePackets(PacketType.SanadPrint, 2, null, isSync: false, null, 0, deliveryRequst: false, extraDataForDebug);
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSanadPrint.cs", 66);
				throw;
			}
		}
	}

	public static Packet GenerateReadRequest(DateTime modified)
	{
		using (new JaegerMonitor("GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSanadPrint.cs"))
		{
			try
			{
				List<string> list = new List<string> { ((short)5).ToString() };
				HesabMoeinReadEventArgs hesabMoeinReadEventArgs = new HesabMoeinReadEventArgs
				{
					Modified = modified
				};
				list.Add(hesabMoeinReadEventArgs.ToString());
				return new Packet(PacketType.SanadPrint, 5, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSanadPrint.cs", 82);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReadResponse(IEnumerable<WebSanadPrint> webCoupon, bool isSync, int partitionId)
	{
		using (new JaegerMonitor("GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSanadPrint.cs"))
		{
			try
			{
				if (webCoupon == null || !webCoupon.Any())
				{
					throw new NullReferenceException();
				}
				object result;
				if (webCoupon == null)
				{
					result = null;
				}
				else
				{
					result = webCoupon.GeneratePackets(PacketType.SanadPrint, 6, null, isSync, null, partitionId, deliveryRequst: false, "GenerateReadResponse");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSanadPrint.cs", 96);
				return null;
			}
		}
	}

	public static List<Packet> GenerateDeleteRequest(IEnumerable<Guid> coupons)
	{
		using (new JaegerMonitor("GenerateDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSanadPrint.cs"))
		{
			try
			{
				if (coupons == null || !coupons.Any())
				{
					throw new NullReferenceException();
				}
				return coupons?.GeneratePackets(PacketType.SanadPrint, 8, null, isSync: false, null, 0, deliveryRequst: true, "GenerateDeleteRequest");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSanadPrint.cs", 110);
				return null;
			}
		}
	}

	public string GetPacketActionEnumName(short action)
	{
		SanadPrintActions sanadPrintActions = (SanadPrintActions)action;
		return sanadPrintActions.ToString();
	}

	public string GetPacketActionName(short action)
	{
		return ((SanadPrintActions)action).GetDisplay();
	}

	public async Task DataReceived(Packet pack, string ConnectionUniqueId, ConnectionData connection)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSanadPrint.cs"))
		{
			_ = 4;
			try
			{
				switch ((SanadPrintActions)pack.PacketActions)
				{
				case SanadPrintActions.SaveRequest:
					if (this.OnSaveRequest != null)
					{
						ConnectionEventArgs<WebSanadPrint> args2 = new ConnectionEventArgs<WebSanadPrint>(pack, ConnectionUniqueId, connection);
						await Utilities.RaiseEvent(this.OnSaveRequest, this, args2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSanadPrint.cs");
					}
					break;
				case SanadPrintActions.SaveResponse:
					if (this.OnSaveResponse != null)
					{
						SaveResponseEventArgs args3 = new SaveResponseEventArgs(pack, ConnectionUniqueId, connection);
						await Utilities.RaiseEvent(this.OnSaveResponse, this, args3, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSanadPrint.cs");
					}
					break;
				case SanadPrintActions.ReadRequest:
					if (this.OnReadRequest != null)
					{
						HesabMoeinReadEventArgs hesabMoeinReadEventArgs = pack.Args[1].FromJson<HesabMoeinReadEventArgs>("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSanadPrint.cs", 136);
						hesabMoeinReadEventArgs.ConnectionUniqueId = ConnectionUniqueId;
						hesabMoeinReadEventArgs.Connection = connection;
						await Utilities.RaiseEvent(this.OnReadRequest, this, hesabMoeinReadEventArgs, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSanadPrint.cs");
					}
					break;
				case SanadPrintActions.ReadResponse:
				{
					if (this.OnReadResponse == null && this.OnSync == null)
					{
						break;
					}
					ConnectionEventArgs<WebSanadPrint> args = new ConnectionEventArgs<WebSanadPrint>(pack, ConnectionUniqueId, connection);
					if (pack.IsSynced)
					{
						if (this.OnSync != null)
						{
							await Utilities.RaiseEvent(this.OnSync, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSanadPrint.cs");
						}
						else if (this.OnReadResponse != null)
						{
							await Utilities.RaiseEvent(this.OnReadResponse, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSanadPrint.cs");
						}
					}
					break;
				}
				case (SanadPrintActions)3:
				case (SanadPrintActions)4:
					break;
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSanadPrint.cs", 154);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSanadPrint.cs", 168);
		}
	}
}
