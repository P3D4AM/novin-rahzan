using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Product;
using PacketParser.EventArgs;

namespace PacketParser.Entities;

public class WebProductPriceLevel : Serializable<WebProductPriceLevel>, IProductPriceLevel, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IDeliverableEntity
{
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public short Ordinal { get; set; }

	public EnPriceLevelType Type { get; set; }

	public decimal DiscountCoefficient { get; set; }

	public bool IsSystem { get; set; }

	public bool IsDefault { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public new bool IsDisposed { get; private set; }

	public event Func<object, ProductPriceLevelReadEventArgs, Task> OnReadRequest;

	public event Func<object, ConnectionEventArgs<WebProductPriceLevel>, Task> OnReadResponse;

	public event Func<object, ConnectionEventArgs<WebProductPriceLevel>, Task> OnSaveRequest;

	public event Func<object, SaveResponseEventArgs, Task> OnSaveResponse;

	public event Func<object, ConnectionEventArgs<WebProductPriceLevel>, Task> OnSync;

	public string GetPacketActionName(short action)
	{
		return ((ProductPriceLevelActions)action).GetDisplay();
	}

	public string GetPacketActionEnumName(short action)
	{
		ProductPriceLevelActions productPriceLevelActions = (ProductPriceLevelActions)action;
		return productPriceLevelActions.ToString();
	}

	public static Packet GenerateReadRequest(DateTime modified)
	{
		using (new JaegerMonitor("GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebProductPriceLevel.cs"))
		{
			try
			{
				List<string> list = new List<string> { ((short)5).ToString() };
				ProductPriceLevelReadEventArgs productPriceLevelReadEventArgs = new ProductPriceLevelReadEventArgs
				{
					Modified = modified
				};
				list.Add(productPriceLevelReadEventArgs.ToString());
				return new Packet(PacketType.ProductPriceLevel, 5, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebProductPriceLevel.cs", 54);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReadResponse(IEnumerable<WebProductPriceLevel> items, bool isSync, int partitionId)
	{
		if (items == null || !items.Any())
		{
			throw new NullReferenceException();
		}
		using (new JaegerMonitor("GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebProductPriceLevel.cs"))
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
					result = items.GeneratePackets(PacketType.ProductPriceLevel, 6, null, isSync, null, partitionId, deliveryRequst: false, "GenerateReadResponse");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebProductPriceLevel.cs", 68);
				throw;
			}
		}
	}

	public static List<Packet> GenerateSaveRequest(IEnumerable<WebProductPriceLevel> titles, int partitionId)
	{
		using (new JaegerMonitor("GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebProductPriceLevel.cs"))
		{
			try
			{
				if (titles == null || !titles.Any())
				{
					throw new NullReferenceException();
				}
				string text = titles.Select((WebProductPriceLevel p) => $"{p.Guid}/{p.Name}").GenerateDebugDescription(5);
				object result;
				if (titles == null)
				{
					result = null;
				}
				else
				{
					string extraDataForDebug = text;
					result = titles.GeneratePackets(PacketType.ProductPriceLevel, 1, null, isSync: false, null, partitionId, deliveryRequst: false, extraDataForDebug);
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebProductPriceLevel.cs", 83);
				throw;
			}
		}
	}

	public static List<Packet> GenerateSaveResponse(IEnumerable<SaveResponse> responses)
	{
		using (new JaegerMonitor("GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebProductPriceLevel.cs"))
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
					result = responses.GeneratePackets(PacketType.ProductPriceLevel, 2, null, isSync: false, null, 0, deliveryRequst: false, extraDataForDebug);
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebProductPriceLevel.cs", 98);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReset(IEnumerable<WebProductPriceLevel> titles)
	{
		using (new JaegerMonitor("GenerateReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebProductPriceLevel.cs"))
		{
			try
			{
				return titles?.GeneratePackets(PacketType.ProductPriceLevel, 7, null, isSync: false, null, 0, deliveryRequst: true, "GenerateReset");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebProductPriceLevel.cs", 111);
				throw;
			}
		}
	}

	public async Task DataReceived(Packet pack, string ConnectionUniqueId, ConnectionData connection)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebProductPriceLevel.cs"))
		{
			_ = 4;
			try
			{
				switch ((ProductPriceLevelActions)pack.PacketActions)
				{
				case ProductPriceLevelActions.SaveRequest:
				{
					ConnectionEventArgs<WebProductPriceLevel> args3 = new ConnectionEventArgs<WebProductPriceLevel>(pack, ConnectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnSaveRequest, this, args3, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebProductPriceLevel.cs");
					break;
				}
				case ProductPriceLevelActions.SaveResponse:
				{
					SaveResponseEventArgs args2 = new SaveResponseEventArgs(pack, ConnectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnSaveResponse, this, args2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebProductPriceLevel.cs");
					break;
				}
				case ProductPriceLevelActions.ReadRequest:
				{
					ProductPriceLevelReadEventArgs productPriceLevelReadEventArgs = pack.Args[1].FromJson<ProductPriceLevelReadEventArgs>("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebProductPriceLevel.cs", 132);
					productPriceLevelReadEventArgs.ConnectionUniqueId = ConnectionUniqueId;
					productPriceLevelReadEventArgs.Connection = connection;
					await Utilities.RaiseEvent(this.OnReadRequest, this, productPriceLevelReadEventArgs, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebProductPriceLevel.cs");
					break;
				}
				case ProductPriceLevelActions.ReadResponse:
				{
					ConnectionEventArgs<WebProductPriceLevel> args = new ConnectionEventArgs<WebProductPriceLevel>(pack, ConnectionUniqueId, connection);
					if (pack.IsSynced)
					{
						await Utilities.RaiseEvent(this.OnSync, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebProductPriceLevel.cs");
					}
					else
					{
						await Utilities.RaiseEvent(this.OnReadResponse, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebProductPriceLevel.cs");
					}
					break;
				}
				case (ProductPriceLevelActions)3:
				case (ProductPriceLevelActions)4:
				case ProductPriceLevelActions.Reset:
					break;
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebProductPriceLevel.cs", 149);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebProductPriceLevel.cs", 164);
		}
	}
}
