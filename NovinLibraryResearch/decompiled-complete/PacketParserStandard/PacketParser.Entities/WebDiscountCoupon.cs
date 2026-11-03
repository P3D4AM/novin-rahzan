using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.Entities.Accounting.DiscountCoupon;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.DiscountCoupon;
using PacketParser.EventArgs;

namespace PacketParser.Entities;

public class WebDiscountCoupon : ICouponDiscount, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IDeliverableEntity, INpDisposable, IDisposable
{
	public Guid Guid { get; set; }

	public string Title { get; set; }

	public DateTime? DateFrom { get; set; }

	public DateTime? DateTo { get; set; }

	public bool SingleUse { get; set; }

	public int CountCouponUse { get; set; }

	public int CountCouponUseForUser { get; set; }

	public Guid CashierGuid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public List<WebDiscountCouponDetail> DetailList { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public EnCouponType Type { get; set; }

	public EnCouponMode Mode { get; set; }

	public int DeadLineForUse { get; set; }

	public CouponDiscountMetaData MetaData { get; set; }

	public bool IsImmediateUseAllowed { get; set; }

	public int Version { get; set; }

	public int UsedCount { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public bool IsDisposed { get; set; }

	public event Func<object, HesabMoeinReadEventArgs, Task> OnReadRequest;

	public event Func<object, ConnectionEventArgs<WebDiscountCoupon>, Task> OnReadResponse;

	public event Func<object, ConnectionEventArgs<WebDiscountCoupon>, Task> OnSaveRequest;

	public event Func<object, SaveResponseEventArgs, Task> OnSaveResponse;

	public event Func<object, ConnectionEventArgs<WebDiscountCoupon>, Task> OnSync;

	public static List<Packet> GenerateSaveRequest(IEnumerable<WebDiscountCoupon> webCoupon, int partitionId)
	{
		using (new JaegerMonitor("GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDiscountCoupon.cs"))
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
					result = webCoupon.GeneratePackets(PacketType.DiscountCoupon, 1, null, isSync: false, null, partitionId, deliveryRequst: false, "GenerateSaveRequest");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDiscountCoupon.cs", 59);
				throw;
			}
		}
	}

	public static List<Packet> GenerateSaveResponse(IEnumerable<SaveResponse> responses)
	{
		using (new JaegerMonitor("GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDiscountCoupon.cs"))
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
					result = responses.GeneratePackets(PacketType.DiscountCoupon, 2, null, isSync: false, null, 0, deliveryRequst: false, extraDataForDebug);
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDiscountCoupon.cs", 74);
				throw;
			}
		}
	}

	public static Packet GenerateReadRequest(DateTime modified)
	{
		using (new JaegerMonitor("GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDiscountCoupon.cs"))
		{
			try
			{
				List<string> list = new List<string> { ((short)5).ToString() };
				HesabMoeinReadEventArgs hesabMoeinReadEventArgs = new HesabMoeinReadEventArgs
				{
					Modified = modified
				};
				list.Add(hesabMoeinReadEventArgs.ToString());
				return new Packet(PacketType.DiscountCoupon, 5, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDiscountCoupon.cs", 90);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReadResponse(IEnumerable<WebDiscountCoupon> webCoupon, bool isSync, int partitionId)
	{
		using (new JaegerMonitor("GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDiscountCoupon.cs"))
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
					result = webCoupon.GeneratePackets(PacketType.DiscountCoupon, 6, null, isSync, null, partitionId, deliveryRequst: false, "GenerateReadResponse");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDiscountCoupon.cs", 104);
				return null;
			}
		}
	}

	public static List<Packet> GenerateDeleteRequest(IEnumerable<Guid> coupons)
	{
		using (new JaegerMonitor("GenerateDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDiscountCoupon.cs"))
		{
			try
			{
				if (coupons == null || !coupons.Any())
				{
					throw new NullReferenceException();
				}
				return coupons?.GeneratePackets(PacketType.DiscountCoupon, 8, null, isSync: false, null, 0, deliveryRequst: true, "GenerateDeleteRequest");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDiscountCoupon.cs", 118);
				return null;
			}
		}
	}

	public string GetPacketActionEnumName(short action)
	{
		CouponActions couponActions = (CouponActions)action;
		return couponActions.ToString();
	}

	public string GetPacketActionName(short action)
	{
		return ((CouponActions)action).GetDisplay();
	}

	public async Task DataReceived(Packet pack, string ConnectionUniqueId, ConnectionData connection)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDiscountCoupon.cs"))
		{
			_ = 4;
			try
			{
				switch ((CouponActions)pack.PacketActions)
				{
				case CouponActions.SaveRequest:
					if (this.OnSaveRequest != null)
					{
						ConnectionEventArgs<WebDiscountCoupon> args2 = new ConnectionEventArgs<WebDiscountCoupon>(pack, ConnectionUniqueId, connection);
						await Utilities.RaiseEvent(this.OnSaveRequest, this, args2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDiscountCoupon.cs");
					}
					break;
				case CouponActions.SaveResponse:
					if (this.OnSaveResponse != null)
					{
						SaveResponseEventArgs args3 = new SaveResponseEventArgs(pack, ConnectionUniqueId, connection);
						await Utilities.RaiseEvent(this.OnSaveResponse, this, args3, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDiscountCoupon.cs");
					}
					break;
				case CouponActions.ReadRequest:
					if (this.OnReadRequest != null)
					{
						HesabMoeinReadEventArgs hesabMoeinReadEventArgs = pack.Args[1].FromJson<HesabMoeinReadEventArgs>("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDiscountCoupon.cs", 144);
						hesabMoeinReadEventArgs.ConnectionUniqueId = ConnectionUniqueId;
						hesabMoeinReadEventArgs.Connection = connection;
						await Utilities.RaiseEvent(this.OnReadRequest, this, hesabMoeinReadEventArgs, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDiscountCoupon.cs");
					}
					break;
				case CouponActions.ReadResponse:
				{
					if (this.OnReadResponse == null && this.OnSync == null)
					{
						break;
					}
					ConnectionEventArgs<WebDiscountCoupon> args = new ConnectionEventArgs<WebDiscountCoupon>(pack, ConnectionUniqueId, connection);
					if (pack.IsSynced)
					{
						if (this.OnSync != null)
						{
							await Utilities.RaiseEvent(this.OnSync, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDiscountCoupon.cs");
						}
						else if (this.OnReadResponse != null)
						{
							await Utilities.RaiseEvent(this.OnReadResponse, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDiscountCoupon.cs");
						}
					}
					break;
				}
				case (CouponActions)3:
				case (CouponActions)4:
					break;
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDiscountCoupon.cs", 162);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDiscountCoupon.cs", 176);
		}
	}
}
