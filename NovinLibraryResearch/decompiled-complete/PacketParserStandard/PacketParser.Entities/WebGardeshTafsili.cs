using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;

namespace PacketParser.Entities;

[Serializable]
public class WebGardeshTafsili : Serializable<WebGardeshTafsili>, IReceiver, IGardeshTafsil, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	private class NestedWebGardeshTafsili
	{
		internal static readonly WebGardeshTafsili instance;

		static NestedWebGardeshTafsili()
		{
			instance = new WebGardeshTafsili();
		}
	}

	public enum GardeshTafsiliActions
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
		ReadResponse = 6
	}

	private const int ChunckSizeKB = 100;

	private DateTime _Date = DateTime.Now;

	private DateTime _DateMoaser = DateTime.Now;

	public DateTime Date
	{
		get
		{
			if (_Date < new DateTime(1900, 1, 1))
			{
				_Date = new DateTime(2000, 1, 1);
			}
			else if (_Date > new DateTime(2100, 1, 1))
			{
				_Date = DateTime.Now;
			}
			return _Date;
		}
		set
		{
			_Date = value;
		}
	}

	[JsonIgnore]
	public string DateSh
	{
		get
		{
			return Date.MiladiToShamsi();
		}
		set
		{
		}
	}

	public short DepotId { get; set; }

	public Guid? DepotGuid { get; set; }

	public string Description { get; set; }

	public Guid? AsGuid { get; set; }

	public int ASID { get; set; }

	public short AsType { get; set; }

	[JsonIgnore]
	[XmlIgnore]
	public Document EnAsType
	{
		get
		{
			try
			{
				short num = AsType;
				if (AsType > 99)
				{
					num = short.Parse(AsType.ToString().Substring(0, 2));
				}
				if (num > 90)
				{
					num = short.Parse(AsType.ToString().Substring(1, 1));
				}
				return (Document)num;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "EnAsType", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebGardeshTafsili.cs", 65);
				return Document.None;
			}
		}
		set
		{
		}
	}

	public bool Ischecked { get; set; }

	public DocumentDet AsTypeEnum { get; set; }

	public decimal Debit { get; set; }

	public decimal Credit { get; set; }

	public short UId { get; set; }

	public int SanadNO { get; set; }

	public int SanadSt { get; set; }

	public int KolId { get; set; }

	public int MoeinId { get; set; }

	public int TafsilId { get; set; }

	public string Serial { get; set; }

	public decimal Price { get; set; }

	public decimal Discount { get; set; }

	[JsonIgnore]
	public decimal DiscountPercent
	{
		get
		{
			if (QT * Price <= 0m)
			{
				return 0m;
			}
			return Discount / (QT * Price);
		}
		set
		{
		}
	}

	public int Id2 { get; set; }

	public decimal QT { get; set; }

	public decimal QT2 { get; set; }

	public decimal Bazaryab { get; set; }

	public Guid? VGuid { get; set; }

	public short VId { get; set; }

	public int PrdId { get; set; }

	public Guid? PrdGuid { get; set; }

	public short TypeId { get; set; }

	public string SanadDesc { get; set; }

	public string y
	{
		get
		{
			return SanadDesc;
		}
		set
		{
			SanadDesc = value;
		}
	}

	public DateTime DateMoaser
	{
		get
		{
			if (_DateMoaser < new DateTime(1900, 1, 1))
			{
				_DateMoaser = new DateTime(2000, 1, 1);
			}
			else if (_DateMoaser > new DateTime(2100, 1, 1))
			{
				_DateMoaser = DateTime.Now;
			}
			return _DateMoaser;
		}
		set
		{
			_DateMoaser = value;
		}
	}

	public string DateMoaserSh => DateMoaser.MiladiToCalture();

	public decimal Rem { get; set; }

	public int RowIndex { get; set; }

	public Guid? DecimalGuid { get; set; }

	public int DecimalId { get; set; }

	public string RowDesc { get; set; }

	public Guid? TafsilGuid { get; set; }

	public Guid MoeinGuid { get; set; }

	public Guid KolGuid { get; set; }

	public Guid UGuid { get; set; }

	public decimal DecimalPrice { get; set; }

	public decimal DecimalQt { get; set; }

	public Guid DetailGuid { get; set; }

	[JsonIgnore]
	[XmlIgnore]
	public string RemForDisplay
	{
		get
		{
			decimal rem = Rem;
			if (rem < 0m)
			{
				rem *= -1m;
			}
			return rem.NpToString("N0", 0);
		}
	}

	[JsonIgnore]
	[XmlIgnore]
	public string CreditForDisplay => Credit.NpToString("N0", 0);

	[JsonIgnore]
	[XmlIgnore]
	public string DebitForDisplay => Debit.NpToString("N0", 0);

	[JsonIgnore]
	[XmlIgnore]
	public string DiscountForDisplay => Discount.NpToString("N0", 0);

	[JsonIgnore]
	[XmlIgnore]
	public string PriceForDisplay => Price.NpToString("N0", 0);

	public string DescForDisplay { get; set; }

	[JsonIgnore]
	[XmlIgnore]
	public string Tashkhis
	{
		get
		{
			if (Credit - Debit > 0m)
			{
				return "بس";
			}
			if (Credit - Debit < 0m)
			{
				return "بد";
			}
			return "";
		}
	}

	public Guid Guid { get; set; } = Guid.NewGuid();


	public DateTime Modified { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	public event Func<object, GardeshTafsiliReadEventArgs, Task> OnReadRequest;

	public event Func<object, ConnectionEventArgs<WebGardeshTafsili>, Task> OnReadResponse;

	public event Func<object, ConnectionEventArgs<WebGardeshTafsili>, Task> OnSaveRequest;

	public event Func<object, SaveResponseEventArgs, Task> OnSaveResponse;

	public event Func<object, ConnectionEventArgs<WebGardeshTafsili>, Task> OnSync;

	public static Packet GenerateReadRequest(DateTime modified)
	{
		using (new JaegerMonitor("GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebGardeshTafsili.cs"))
		{
			try
			{
				List<string> list = new List<string> { ((short)5).ToString() };
				GardeshTafsiliReadEventArgs gardeshTafsiliReadEventArgs = new GardeshTafsiliReadEventArgs
				{
					Modified = modified
				};
				list.Add(gardeshTafsiliReadEventArgs.ToString());
				return new Packet(PacketType.GardeshTafsili, 5, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebGardeshTafsili.cs", 213);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReadResponse(IEnumerable<WebGardeshTafsili> items, bool isSync = false)
	{
		if (items == null || items.Count() <= 0)
		{
			throw new NullReferenceException();
		}
		using (new JaegerMonitor("GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebGardeshTafsili.cs"))
		{
			try
			{
				return items?.GeneratePackets(PacketType.GardeshTafsili, 6, null, isSync, null, 0, deliveryRequst: true, "GenerateReadResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebGardeshTafsili.cs", 229);
				return null;
			}
		}
	}

	public static List<Packet> GenerateSaveRequest(IEnumerable<WebGardeshTafsili> gardeshes)
	{
		using (new JaegerMonitor("GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebGardeshTafsili.cs"))
		{
			try
			{
				return gardeshes?.GeneratePackets(PacketType.GardeshTafsili, 1, null, isSync: false, null, 0, deliveryRequst: false, "GenerateSaveRequest");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebGardeshTafsili.cs", 247);
				throw;
			}
		}
	}

	public static List<Packet> GenerateSaveResponse(IEnumerable<SaveResponse> responses)
	{
		using (new JaegerMonitor("GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebGardeshTafsili.cs"))
		{
			try
			{
				return responses?.GeneratePackets(PacketType.GardeshTafsili, 2, null, isSync: false, null, 0, deliveryRequst: false, "GenerateSaveResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebGardeshTafsili.cs", 266);
				throw;
			}
		}
	}

	public string GetPacketActionEnumName(short action)
	{
		try
		{
			GardeshTafsiliActions gardeshTafsiliActions = (GardeshTafsiliActions)action;
			return gardeshTafsiliActions.ToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionEnumName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebGardeshTafsili.cs", 285);
			return $"Error({action})";
		}
	}

	public string GetPacketActionName(short action)
	{
		try
		{
			return ((GardeshTafsiliActions)action).GetDisplay();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebGardeshTafsili.cs", 298);
			return $"Error({action})";
		}
	}

	public async Task DataReceived(Packet pack, string ConnectionUniqueId, ConnectionData connection)
	{
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebGardeshTafsili.cs"))
		{
			_ = 4;
			try
			{
				switch ((GardeshTafsiliActions)pack.PacketActions)
				{
				case GardeshTafsiliActions.SaveRequest:
					if (this.OnSaveRequest != null)
					{
						ConnectionEventArgs<WebGardeshTafsili> args2 = new ConnectionEventArgs<WebGardeshTafsili>(pack, ConnectionUniqueId, connection);
						await Utilities.RaiseEvent(this.OnSaveRequest, this, args2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebGardeshTafsili.cs");
					}
					break;
				case GardeshTafsiliActions.SaveResponse:
					if (this.OnSaveResponse != null)
					{
						SaveResponseEventArgs args3 = new SaveResponseEventArgs(pack, ConnectionUniqueId, connection);
						await Utilities.RaiseEvent(this.OnSaveResponse, this, args3, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebGardeshTafsili.cs");
					}
					break;
				case GardeshTafsiliActions.ReadRequest:
					if (this.OnReadRequest != null)
					{
						GardeshTafsiliReadEventArgs gardeshTafsiliReadEventArgs = pack.Args[1].FromJson<GardeshTafsiliReadEventArgs>("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebGardeshTafsili.cs", 325);
						gardeshTafsiliReadEventArgs.ConnectionUniqueId = ConnectionUniqueId;
						gardeshTafsiliReadEventArgs.Connection = connection;
						await Utilities.RaiseEvent(this.OnReadRequest, this, gardeshTafsiliReadEventArgs, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebGardeshTafsili.cs");
					}
					break;
				case GardeshTafsiliActions.ReadResponse:
				{
					if (this.OnReadResponse == null && this.OnSync == null)
					{
						break;
					}
					ConnectionEventArgs<WebGardeshTafsili> args = new ConnectionEventArgs<WebGardeshTafsili>(pack, ConnectionUniqueId, connection);
					if (pack.IsSynced)
					{
						if (this.OnSync != null)
						{
							await Utilities.RaiseEvent(this.OnSync, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebGardeshTafsili.cs");
						}
						else if (this.OnReadResponse != null)
						{
							await Utilities.RaiseEvent(this.OnReadResponse, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebGardeshTafsili.cs");
						}
					}
					break;
				}
				case (GardeshTafsiliActions)3:
				case (GardeshTafsiliActions)4:
					break;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebGardeshTafsili.cs", 347);
			}
		}
	}
}
