using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.DalInterfaces;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;

namespace PacketParser.Entities.Accounting;

public class GardeshTafsil : IGardeshTafsil, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IItemAsyncMapper<GardeshTafsil, GardeshTafsil>
{
	private IProduct _PrdInfo;

	private IPrdVahed _prdVahed;

	private string _TafsilItemName = "";

	public int RowIndex { get; set; }

	public DateTime Date { get; set; }

	public string DateSh
	{
		get
		{
			return Date.MiladiToCalture();
		}
		set
		{
		}
	}

	public Guid? DepotGuid { get; set; }

	public string Description { get; set; }

	public Guid? AsGuid { get; set; }

	public short AsType { get; set; }

	public bool IsEnteghalHesab { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public IDalTurnover DalTurnover { get; set; }

	public Document EnAsType
	{
		get
		{
			using (new JaegerMonitor("EnAsType", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Accounting\\GardeshTafsil.cs"))
			{
				try
				{
					short num = AsType;
					if (AsType == 206)
					{
						return Document.OrderPreBuy;
					}
					if (AsType == 211)
					{
						return Document.AghsatM;
					}
					if (AsType == 212)
					{
						return Document.AghsatSh;
					}
					if (AsType == 215)
					{
						return Document.AghsatMProfit;
					}
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
					WebErrorLog.Instance.StartLog(ex, "", "EnAsType", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Accounting\\GardeshTafsil.cs", 47);
					throw;
				}
			}
		}
		set
		{
		}
	}

	public DocumentDet AsTypeEnum { get; set; }

	public decimal Debit { get; set; }

	public decimal Credit { get; set; }

	public int SanadNO { get; set; }

	public int SanadSt { get; set; }

	public string Serial { get; set; }

	public decimal Price { get; set; }

	public decimal Discount { get; set; }

	public decimal DiscountPercent
	{
		get
		{
			decimal num = Math.Round(Price * QT, 0);
			if (num <= 0m)
			{
				return 0m;
			}
			return Math.Round((100m * num - Discount) / num, 0);
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

	public Guid? PrdGuid { get; set; }

	public string SanadDesc { get; set; }

	public DateTime DateMoaser { get; set; }

	public string DateMoaserSh => DateMoaser.MiladiToCalture();

	public decimal Rem { get; set; }

	public decimal RemWithQest { get; set; }

	public Guid? DecimalGuid { get; set; }

	public string RowDesc { get; set; }

	public Guid MoeinGuid { get; set; }

	public Guid KolGuid { get; set; }

	public Guid UGuid { get; set; }

	public decimal DecimalPrice { get; set; }

	public decimal DecimalQt { get; set; }

	public Guid DetailGuid { get; set; }

	public string Tashkhis
	{
		get
		{
			if (Rem < 0m)
			{
				return "بد".GetTranslate();
			}
			if (Rem > 0m)
			{
				return "بس".GetTranslate();
			}
			return "بی حساب".GetTranslate();
		}
		set
		{
		}
	}

	public Guid Guid { get; set; } = Guid.NewGuid();


	public DateTime Modified { get; set; }

	public bool Status { get; set; } = true;


	public bool IsChecked { get; set; }

	public decimal Sood { get; set; }

	public short DepId { get; set; }

	[JsonIgnore]
	[XmlIgnore]
	public IProduct PrdInfo
	{
		get
		{
			throw new NotImplementedException();
		}
		set
		{
			_PrdInfo = value;
		}
	}

	[JsonIgnore]
	[XmlIgnore]
	public IPrdVahed PrdVahed
	{
		get
		{
			if (_prdVahed == null)
			{
				_prdVahed = DalTurnover.GetPrdVahed(DepId, PrdInfo?.GuidVahed ?? Guid.Empty);
			}
			return _prdVahed;
		}
		set
		{
			_prdVahed = value;
		}
	}

	[JsonIgnore]
	public string TafsilItemName
	{
		get
		{
			if (!TafsilGuid.HasValue || TafsilGuid.Value == Guid.Empty)
			{
				return null;
			}
			if (string.IsNullOrEmpty(_TafsilItemName) && TafsilGuid != Guid.Empty)
			{
				_TafsilItemName = DalTurnover?.GetTafsil(DepId, TafsilGuid.Value)?.Name ?? "";
			}
			return _TafsilItemName;
		}
		set
		{
			_TafsilItemName = value;
		}
	}

	public Guid? TafsilGuid { get; set; }

	public string RowSumForDisplay
	{
		get
		{
			return Math.Abs(Credit - Debit).NpToString("N0", 0);
		}
		set
		{
		}
	}

	public string vahedForDisplay
	{
		get
		{
			return PrdVahed?.Name ?? "";
		}
		set
		{
		}
	}

	public string DescForDisplay
	{
		get
		{
			try
			{
				if (PrdInfo != null)
				{
					string text = PrdInfo.Name ?? "";
					string text2 = PrdInfo.Code.ToString() ?? "";
					return " ( " + text + " - " + text2 + " ) " + RowDesc;
				}
				if (EnAsType == Document.None)
				{
					return "مانده از قبل".GetTranslate() + " (-)";
				}
				return (RowDesc ?? "") ?? "";
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "DescForDisplay", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Accounting\\GardeshTafsil.cs", 178);
				return null;
			}
		}
		set
		{
		}
	}

	public string CreditForDisplay
	{
		get
		{
			return Credit.NpToString("N0", 0);
		}
		set
		{
		}
	}

	public string DebitForDisplay
	{
		get
		{
			return Debit.NpToString("N0", 0);
		}
		set
		{
		}
	}

	public string DiscountForDisplay
	{
		get
		{
			return Discount.NpToString("N0", 0);
		}
		set
		{
		}
	}

	public string PriceForDisplay
	{
		get
		{
			return Price.NpToString("N0", 0);
		}
		set
		{
		}
	}

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
		set
		{
		}
	}

	public GardeshTafsil()
	{
	}

	public GardeshTafsil(IDalTurnover dalTurnover)
	{
		DalTurnover = dalTurnover;
	}

	public Task<GardeshTafsil> MapAsync(string contextGuid, GardeshTafsil source, GardeshTafsil destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return Task.FromResult(destination);
			}
			if (destination == null)
			{
				destination = new GardeshTafsil();
			}
			destination.Date = source.Date;
			destination.DateSh = source.DateSh;
			destination.DepotGuid = source.DepotGuid;
			destination.Description = source.Description;
			destination.AsGuid = source.AsGuid;
			destination.AsType = source.AsType;
			destination.EnAsType = source.EnAsType;
			destination.AsTypeEnum = source.AsTypeEnum;
			destination.Debit = source.Debit;
			destination.Credit = source.Credit;
			destination.SanadNO = source.SanadNO;
			destination.SanadSt = source.SanadSt;
			destination.Serial = source.Serial;
			destination.Price = source.Price;
			destination.Discount = source.Discount;
			destination.DiscountPercent = source.DiscountPercent;
			destination.Id2 = source.Id2;
			destination.QT = source.QT;
			destination.QT2 = source.QT2;
			destination.Bazaryab = source.Bazaryab;
			destination.VGuid = source.VGuid;
			destination.PrdGuid = source.PrdGuid;
			destination.SanadDesc = source.SanadDesc;
			destination.DateMoaser = source.DateMoaser;
			destination.Rem = source.Rem;
			destination.RowIndex = source.RowIndex;
			destination.DecimalGuid = source.DecimalGuid;
			destination.RowDesc = source.RowDesc;
			destination.MoeinGuid = source.MoeinGuid;
			destination.KolGuid = source.KolGuid;
			destination.UGuid = source.UGuid;
			destination.Tashkhis = source.Tashkhis;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.IsChecked = source.IsChecked;
			destination.Sood = source.Sood;
			destination.DepId = source.DepId;
			destination._TafsilItemName = source._TafsilItemName;
			destination.TafsilItemName = source.TafsilItemName;
			destination.TafsilGuid = source.TafsilGuid;
			destination.RowSumForDisplay = source.RowSumForDisplay;
			destination.vahedForDisplay = source.vahedForDisplay;
			destination.DescForDisplay = source.DescForDisplay;
			destination.CreditForDisplay = source.CreditForDisplay;
			destination.DebitForDisplay = source.DebitForDisplay;
			destination.DiscountForDisplay = source.DiscountForDisplay;
			destination.PriceForDisplay = source.PriceForDisplay;
			destination.RemForDisplay = source.RemForDisplay;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Accounting\\GardeshTafsil.cs", 262);
		}
		return Task.FromResult(destination);
	}

	public Task<List<GardeshTafsil>> MapListAsync(string contextGuid, IEnumerable<GardeshTafsil> source, List<GardeshTafsil> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
