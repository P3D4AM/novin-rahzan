using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

[Serializable]
public class WebOrderDetail : Serializable<WebOrderDetail>, IOrderDetail, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	private decimal? _V2Qt = default(decimal);

	private string _name;

	[JsonIgnore]
	[XmlIgnore]
	public bool IsEdit { get; set; }

	[Obsolete]
	public int ID { get; set; }

	public WebOrderAbad AbadStr { get; set; }

	public List<WebPrdSerial> SerialStr { get; set; }

	public string TaxCode { get; set; }

	public string TaxName { get; set; }

	public int TaxUnitCode { get; set; }

	public int CodeProduct { get; set; }

	public string Vahed { get; set; }

	public string TaxUnitName { get; set; }

	public decimal TaxPercent { get; set; }

	public decimal AvarezPercent { get; set; }

	public int Ordinal { get; set; }

	public int ProductId { get; set; }

	public string ProductName
	{
		get
		{
			return _name;
		}
		set
		{
			_name = value.FixString();
		}
	}

	public Guid ProductGuid { get; set; } = Guid.Empty;


	public decimal Quantity { get; set; }

	public decimal Price { get; set; }

	public decimal? Price2 { get; set; }

	public short DepotId { get; set; }

	public string Description { get; set; }

	public decimal Discount { get; set; }

	public decimal? V2Qt
	{
		get
		{
			if (_V2Qt.HasValue)
			{
				return _V2Qt.Value;
			}
			return null;
		}
		set
		{
			_V2Qt = value;
		}
	}

	public string Serial { get; set; }

	public decimal VisitorPrice1 { get; set; }

	public decimal VisitorPrice2 { get; set; }

	public decimal VisitorPrice3 { get; set; }

	public decimal VisitorPrice4 { get; set; }

	public decimal TotalVisitorPrice { get; }

	public decimal TotalHazinePrice { get; set; }

	public decimal Tax { get; set; }

	public decimal Avarez { get; set; }

	public decimal RowSum => Math.Floor(Quantity * Price) - Discount;

	[JsonIgnore]
	public decimal RowSumWithoutDiscount => Math.Floor(Quantity * Price);

	[JsonIgnore]
	public string PriceForDisplay => Price.NpToString("N0", 0).Replace(".", "/");

	[JsonIgnore]
	public string DiscountForDisplay => ("%" + DiscountPercent).Replace(".", "/");

	[JsonIgnore]
	public string TaxForDisplay => ("%" + TaxPercent).Replace(".", "/");

	[JsonIgnore]
	public string AvarezForDisplay => ("%" + AvarezPercent).Replace(".", "/");

	[JsonIgnore]
	public string RowSumForDisplay => RowSum.NpToString("N0", 0).Replace(".", "/");

	[JsonIgnore]
	public bool HasDiscount => DiscountPercent != 0m;

	[JsonIgnore]
	public bool HasAvarez => AvarezPercent != 0m;

	[JsonIgnore]
	public bool HasTax => TaxPercent != 0m;

	[JsonIgnore]
	public bool HasDescription => !string.IsNullOrEmpty(Description);

	public decimal V2Price { get; set; }

	public decimal Hazine1Price { get; set; }

	public decimal Hazine2Price { get; set; }

	public decimal Hazine3Price { get; set; }

	public decimal Hazine4Price { get; set; }

	public decimal Hazine5Price { get; set; }

	public decimal DecimalQt { get; set; }

	public decimal DecimalPrice { get; set; }

	public decimal DecimalSum { get; set; }

	[JsonIgnore]
	public decimal DecimalRowSum => DecimalPrice * DecimalQt;

	[Obsolete("", true)]
	public EnPriceType PriceType { get; set; }

	public Guid? PriceTypeGuid { get; set; }

	public Guid DepotGuid { get; set; }

	[JsonIgnore]
	[XmlIgnore]
	public decimal DiscountPercent
	{
		get
		{
			if (Quantity * Price <= 0m)
			{
				return 0m;
			}
			return Math.Round(Discount / (Quantity * Price) * 100m, 3);
		}
	}

	[JsonIgnore]
	[XmlIgnore]
	public float VisitorPrice1Percent
	{
		get
		{
			if (RowSum <= 0m)
			{
				return 0f;
			}
			return (float)Math.Round(VisitorPrice1 / RowSum * 100m, 3);
		}
	}

	[JsonIgnore]
	[XmlIgnore]
	public float VisitorPrice2Percent
	{
		get
		{
			if (RowSum <= 0m)
			{
				return 0f;
			}
			return (float)Math.Round(VisitorPrice2 / RowSum * 100m, 3);
		}
	}

	[JsonIgnore]
	[XmlIgnore]
	public float VisitorPrice3Percent
	{
		get
		{
			if (RowSum <= 0m)
			{
				return 0f;
			}
			return (float)Math.Round(VisitorPrice3 / RowSum * 100m, 3);
		}
	}

	[JsonIgnore]
	[XmlIgnore]
	public float VisitorPrice4Percent
	{
		get
		{
			if (RowSum <= 0m)
			{
				return 0f;
			}
			return (float)Math.Round(VisitorPrice4 / RowSum * 100m, 3);
		}
	}

	public Guid? VGuid { get; set; } = Guid.Empty;


	public Guid? DecimalGuid { get; set; } = Guid.Empty;


	public Guid Guid { get; set; } = Guid.NewGuid();


	public DateTime Modified { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	public EntityChangeSt OrderDetState { get; set; }

	public OrderDetailSerializedData SerialzedData { get; set; }

	public string DiscountPercentString { get; set; }

	public Guid? DicountCouponDetailGuid { get; set; }

	public Guid? ReferenceGuid { get; set; }

	public decimal FeePrice { get; set; }

	public decimal ProfitPrice { get; set; }

	public Guid? GrouppingGuid { get; set; }

	public decimal GoldPrice { get; set; }

	public decimal DecimalTotalPrice { get; set; }

	public decimal TotalTaxPrice { get; set; }

	public decimal MetaSellPrice { get; set; }

	public string DecimalSign { get; set; }

	public bool IsTaxImpactOnCost { get; set; }

	public Guid? ProductTafsilGuid { get; set; }

	public decimal CostPrice { get; set; }

	public decimal TotalCostPrice { get; set; }

	public string SteppedDiscount { get; set; }

	public WebOrderDetail()
	{
	}

	public WebOrderDetail(WebGardeshTafsili item)
	{
		try
		{
			if (item.AsType != 9300 && item.AsType != 9400 && item.AsType != 9700 && item.AsType != 9800)
			{
				throw new ArgumentException("SanadType is Out of range" + item.ToString());
			}
			Quantity = item.QT;
			V2Qt = item.QT2;
			ProductGuid = item.PrdGuid.Value;
			Price = item.Price;
			Discount = item.Discount;
			Description = item.RowDesc;
			DepotId = item.DepotId;
			VisitorPrice1 = item.Bazaryab;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrderDetail.cs", 34);
		}
	}

	public async Task<ReturnedSaveFuncInfo> CheckTaxValidationAsync(EnTaxInvoiceTemplate? template)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (!string.IsNullOrEmpty(TaxName) && TaxName.Length > 400)
			{
				await res.AddErrorAsync("عنوان کالا باید حداکثر 400 کاراکتر باشد", "", "CheckTaxValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrderDetail.cs", 180);
			}
			if (string.IsNullOrEmpty(TaxCode))
			{
				await res.AddErrorAsync("وارد کردن شناسه مالیاتی کالا اجباری است", "", "CheckTaxValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrderDetail.cs", 181);
			}
			else if (TaxCode.Length > 13)
			{
				await res.AddErrorAsync("شناسه مالیاتی کالا باید حداکثر 13 کاراکتر باشد", "", "CheckTaxValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrderDetail.cs", 182);
			}
			if (TaxUnitCode <= 0)
			{
				await res.AddErrorAsync("واحد اندازه گیری کالا معتبر نمی باشد", "", "CheckTaxValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrderDetail.cs", 184);
			}
			else if (TaxUnitCode.ToString().Length > 8)
			{
				await res.AddErrorAsync("واحد اندازه گیری کالا باید حداکثر 8 کاراکتر باشد", "", "CheckTaxValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrderDetail.cs", 185);
			}
			if (template.GetValueOrDefault() == EnTaxInvoiceTemplate.Export)
			{
				if ((DecimalGuid ?? Guid.Empty) == Guid.Empty)
				{
					await res.AddErrorAsync("در الگوی صادرات، وارد کردن نوع ارز اجباری است", "", "CheckTaxValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrderDetail.cs", 189);
				}
				if (string.IsNullOrEmpty(DecimalSign))
				{
					await res.AddErrorAsync("نماد ارز انتخاب شده وارد نشده است", "", "CheckTaxValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrderDetail.cs", 190);
				}
				else if (DecimalSign.Length != 3)
				{
					await res.AddErrorAsync("نماد ارز انتخاب شده معتبر نمی باشد و باید 3 کاراکتر باشد", "", "CheckTaxValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrderDetail.cs", 191);
				}
				if ((SerialzedData?.NetWeight ?? 0m) <= 0m)
				{
					await res.AddErrorAsync("در الگوی صادرات، وارد کردن وزن خالص کالا اجباری است", "", "CheckTaxValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrderDetail.cs", 192);
				}
				if (DecimalQt <= 0m)
				{
					await res.AddErrorAsync("در الگوی صاردات، وارد کردن میزان ارز اجباری است", "", "CheckTaxValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrderDetail.cs", 193);
				}
				if (DecimalPrice <= 0m)
				{
					await res.AddErrorAsync("در الگوی صاردات، وارد کردن نرخ برابری ارز و ریال اجباری است", "", "CheckTaxValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrderDetail.cs", 194);
				}
				if (DecimalSum <= 0m)
				{
					await res.AddErrorAsync("در الگوی صاردات، ارزش ارزی کالا باید بزرگتر از صفر باشد", "", "CheckTaxValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrderDetail.cs", 195);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckTaxValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrderDetail.cs", 200);
			res.AddReturnedValue(ex, "", "CheckTaxValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrderDetail.cs", 201);
		}
		return res;
	}
}
