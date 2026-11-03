using System;
using System.ComponentModel;
using Newtonsoft.Json;
using PacketParser.CoreChangeLogger;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

[Serializable]
public class WebSanad_Det : ISanad_Det, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	[PersianNameAttribute.PersianName("مبلغ بدهکاری")]
	public decimal Debit { get; set; }

	[PersianNameAttribute.PersianName("مبلغ بستانکاری")]
	public decimal Credit { get; set; }

	[PersianNameAttribute.PersianName("شرح سطر")]
	public string RowDesc { get; set; }

	[Obsolete("please use rowDesc field instead")]
	public string Description
	{
		get
		{
			return RowDesc;
		}
		set
		{
			if (!string.IsNullOrEmpty(value))
			{
				RowDesc = value;
			}
		}
	}

	[JsonIgnore]
	public string DescriptionForUi => RowDesc;

	public DateTime Modified { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	public Guid Guid { get; set; }

	[PersianNameAttribute.PersianName("تعداد")]
	public decimal Qt { get; set; }

	public int SanadNo { get; set; }

	[PersianNameAttribute.PersianName("سریال")]
	public string Serial { get; set; }

	[PersianNameAttribute.PersianName("کد معین")]
	public string CodeMoein { get; set; }

	[PersianNameAttribute.PersianName("کد تفصیلی")]
	public string CodeTafsil { get; set; }

	public Guid SanadGuid { get; set; }

	public Guid MoeinGuid { get; set; }

	[PersianNameAttribute.PersianName("کد تفصیلی")]
	public Guid? TafsilGuid { get; set; }

	[PersianNameAttribute.PersianName("کد تفصیلی 2")]
	public Guid? TafsilGuid2 { get; set; }

	[PersianNameAttribute.PersianName("کد تفصیلی 3")]
	public Guid? TafsilGuid3 { get; set; }

	[PersianNameAttribute.PersianName("کد تفصیلی 4")]
	public Guid? TafsilGuid4 { get; set; }

	public Guid? DecimalGuid { get; set; }

	public Document AsType { get; set; }

	public Guid AsGuid { get; set; }

	[JsonIgnore]
	[Obsolete("", true)]
	public int SanadID { get; set; }

	[JsonIgnore]
	[Obsolete]
	public int ID { get; set; }

	[JsonIgnore]
	[Obsolete]
	public DateTime DateM { get; set; }

	[JsonIgnore]
	[Obsolete]
	public SanadSt SanadST { get; set; }

	public Guid CashierGuid { get; set; }

	[JsonIgnore]
	[Obsolete("", true)]
	public Guid? DebitTafsilGuid { get; set; }

	[JsonIgnore]
	[Obsolete("", true)]
	public Guid DebitMoeinGuid { get; set; }

	[JsonIgnore]
	[Obsolete("", true)]
	public Guid? CreditTafsilGuid { get; set; }

	[JsonIgnore]
	[Obsolete("", true)]
	public Guid CreditMoeinGuid { get; set; }

	public DateTime? DateSarresid { get; set; }

	[PersianNameAttribute.PersianName("تاریخ سررسید")]
	[JsonIgnore]
	public string DateSarresidSh => DateSarresid.MiladiToCalture();

	[PersianNameAttribute.PersianName("ردیف")]
	[ObjectComparer.GetValueordinal("ردیف")]
	public int Ordinal { get; set; }
}
