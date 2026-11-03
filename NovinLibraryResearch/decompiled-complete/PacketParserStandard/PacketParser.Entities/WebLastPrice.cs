using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.Entities.CompositeKeys;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;

namespace PacketParser.Entities;

public class WebLastPrice : Serializable<WebLastPrice>, ILastPrice, IHasModified, IHasJustGuid, IHasStatus, IReceiver
{
	private class NestedWebLastPrice
	{
		internal static readonly WebLastPrice Instance;

		static NestedWebLastPrice()
		{
			Instance = new WebLastPrice();
		}
	}

	public enum LastPriceActions
	{
		[PersianNameAttribute.PersianName("نامشخص")]
		Unknown = 0,
		[PersianNameAttribute.PersianName("درخواست ذخیره")]
		SaveRequest = 1,
		[PersianNameAttribute.PersianName("پاسخ به درخواست ذخیره")]
		SaveResponse = 2,
		[PersianNameAttribute.PersianName("درخواست خواندن")]
		ReadRequest = 5,
		[PersianNameAttribute.PersianName("سینک")]
		ReadResponse = 6,
		[PersianNameAttribute.PersianName("ریست")]
		Reset = 7
	}

	[JsonIgnore]
	[XmlIgnore]
	public new bool IsDisposed { get; set; }

	public int PartitionId { get; set; }

	public DateTime Date { get; set; }

	public decimal Price { get; set; }

	public Guid PrdGuid { get; set; }

	public Guid TafsilGuid { get; set; }

	public Guid Guid { get; set; }

	public Guid AsGuid { get; set; }

	public Document AsType { get; set; }

	public DateTime Modified { get; set; }

	public bool Invalidated { get; set; }

	public bool Status { get; set; }

	public static WebLastPrice WebLastPriceInstance => NestedWebLastPrice.Instance;

	public ServerStatusForClient ServerStatus { get; set; } = ServerStatusForClient.Delivered;


	public event Func<object, ConnectionEventArgs<WebLastPrice>, Task> OnReadResponse;

	public event Func<object, ConnectionEventArgs<WebLastPrice>, Task> OnSaveRequest;

	public event Func<object, SaveResponseEventArgs<LastPriceCompositeKey>, Task> OnSaveResponse;

	public event Func<object, ConnectionEventArgs<WebLastPrice>, Task> OnSync;

	public string GetPacketActionEnumName(short action)
	{
		try
		{
			LastPriceActions lastPriceActions = (LastPriceActions)action;
			return lastPriceActions.ToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionEnumName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebLastPrice.cs", 42);
			return $"Error({action})";
		}
	}

	public string GetPacketActionName(short action)
	{
		try
		{
			return ((LastPriceActions)action).GetDisplay();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebLastPrice.cs", 55);
			return $"Error({action})";
		}
	}

	public async Task DataReceived(Packet pack, string ConnectionUniqueId, ConnectionData Connection)
	{
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebLastPrice.cs"))
		{
			_ = 3;
			try
			{
				switch ((LastPriceActions)pack.PacketActions)
				{
				case LastPriceActions.SaveRequest:
				{
					ConnectionEventArgs<WebLastPrice> args2 = new ConnectionEventArgs<WebLastPrice>(pack, ConnectionUniqueId, Connection);
					await Utilities.RaiseEvent(this.OnSaveRequest, this, args2, Connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebLastPrice.cs");
					break;
				}
				case LastPriceActions.SaveResponse:
				{
					SaveResponseEventArgs<LastPriceCompositeKey> args3 = new SaveResponseEventArgs<LastPriceCompositeKey>(pack, ConnectionUniqueId, Connection);
					await Utilities.RaiseEvent(this.OnSaveResponse, this, args3, Connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebLastPrice.cs");
					break;
				}
				case LastPriceActions.ReadResponse:
				{
					if (this.OnReadResponse == null && this.OnSync == null)
					{
						break;
					}
					ConnectionEventArgs<WebLastPrice> args = new ConnectionEventArgs<WebLastPrice>(pack, ConnectionUniqueId, Connection);
					try
					{
						if (pack.IsSynced)
						{
							await Utilities.RaiseEvent(this.OnSync, this, args, Connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebLastPrice.cs");
						}
						else
						{
							await Utilities.RaiseEvent(this.OnReadResponse, this, args, Connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebLastPrice.cs");
						}
						break;
					}
					catch (Exception ex)
					{
						WebErrorLog.Instance.StartLog(ex, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebLastPrice.cs", 86);
						break;
					}
				}
				case (LastPriceActions)3:
				case (LastPriceActions)4:
				case LastPriceActions.ReadRequest:
				case LastPriceActions.Reset:
					break;
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex3)
			{
				WebErrorLog.Instance.StartLog(ex3, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebLastPrice.cs", 97);
			}
		}
	}

	public static List<Packet> GenerateReadResponse(IEnumerable<WebLastPrice> items, int partitionId, bool isSync = false)
	{
		if (items == null || items.Count() <= 0)
		{
			throw new NullReferenceException();
		}
		using (new JaegerMonitor("GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebLastPrice.cs"))
		{
			try
			{
				if (items == null || (items != null && items.Count() == 0))
				{
					throw new NullReferenceException("جنریت لیست تهی در  LastPriceActions readresponse");
				}
				object result;
				if (items == null)
				{
					result = null;
				}
				else
				{
					result = items.GeneratePackets(PacketType.LastPrice, 6, null, isSync, null, partitionId, deliveryRequst: true, "GenerateReadResponse");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebLastPrice.cs", 122);
				return null;
			}
		}
	}

	public static List<Packet> GenerateSaveRequest(IEnumerable<WebLastPrice> prices, int partitionId = 0)
	{
		using (new JaegerMonitor("GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebLastPrice.cs"))
		{
			try
			{
				if (prices == null || !prices.Any())
				{
					throw new NullReferenceException();
				}
				return prices.GeneratePackets(PacketType.LastPrice, 1, null, isSync: false, null, partitionId, deliveryRequst: false, "GenerateSaveRequest");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebLastPrice.cs", 140);
				throw;
			}
		}
	}

	public static List<Packet> GenerateSaveResponse(IEnumerable<SaveResponse<LastPriceCompositeKey>> responses, Guid RequestId)
	{
		if (responses == null)
		{
			throw new NullReferenceException();
		}
		using (new JaegerMonitor("GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebLastPrice.cs"))
		{
			try
			{
				if (responses == null || (responses != null && responses.Count() == 0))
				{
					throw new NullReferenceException();
				}
				return responses?.GeneratePackets(PacketType.LastPrice, 2, null, isSync: false, RequestId, 0, deliveryRequst: false, "GenerateSaveResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebLastPrice.cs", 156);
				return null;
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebLastPrice.cs", 173);
		}
	}
}
