using System;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Khazane;

namespace PacketParser.Entities;

[Serializable]
public class WebReceptionHavale : IReceptionHavale, IParDarDet, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasWage
{
	public string Number { get; set; } = "";


	[Obsolete("Use TafsilGuid also fill MoeinGuid")]
	public int IdBank { get; set; }

	public HesabType HType { get; set; } = HesabType.Bank;


	public string Description { get; set; } = "";


	public DateTime Date { get; set; } = DateTime.Now;


	public DateTime? DateUtc { get; set; }

	public decimal Price { get; set; }

	[JsonIgnore]
	[XmlIgnore]
	public string DateSh => Date.MiladiToCalture();

	public ENHavaleType Type { get; set; }

	public Guid Guid { get; set; } = Guid.NewGuid();


	public DateTime Modified { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	[PersianNameAttribute.PersianName("پایانه")]
	public string TerminalId { get; set; } = "";


	[PersianNameAttribute.PersianName("شماره کارت")]
	public string PAN { get; set; } = "";


	[PersianNameAttribute.PersianName("پیگیری")]
	public string STAN { get; set; } = "";


	[PersianNameAttribute.PersianName("مرجع")]
	public string RRN { get; set; } = "";


	public string MerchantId { get; set; } = "";


	public string MessageId { get; set; } = "";


	public DateTime DateTasviye { get; set; }

	[JsonIgnore]
	public string FullDescription
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder(100000);
			stringBuilder.Append(Description);
			if (Number != "")
			{
				if (stringBuilder.Length == 0)
				{
					stringBuilder.AppendLine("ش.سند:" + Number);
				}
				else
				{
					stringBuilder.AppendLine(",ش.سند:" + Number);
				}
			}
			if (STAN != "")
			{
				if (stringBuilder.Length == 0)
				{
					stringBuilder.AppendLine("ش.پیگیری:" + STAN);
				}
				else
				{
					stringBuilder.AppendLine(",ش.پیگیری:" + STAN);
				}
			}
			if (RRN != "")
			{
				if (stringBuilder.Length == 0)
				{
					stringBuilder.AppendLine("ش.ارجاع:" + RRN);
				}
				else
				{
					stringBuilder.AppendLine(",ش.ارجاع:" + RRN);
				}
			}
			if (TerminalId != "")
			{
				if (stringBuilder.Length == 0)
				{
					stringBuilder.AppendLine("ش.پایانه:" + TerminalId);
				}
				else
				{
					stringBuilder.AppendLine(",ش.پایانه:" + TerminalId);
				}
			}
			if (PAN != "")
			{
				if (stringBuilder.Length == 0)
				{
					stringBuilder.AppendLine("پردازنده:" + PAN);
				}
				else
				{
					stringBuilder.AppendLine("پردازنده،:" + PAN);
				}
			}
			return stringBuilder.ToString();
		}
	}

	public Guid MoeinGuid { get; set; }

	public decimal Qt { get; set; }

	public Guid? DecimalGuid { get; set; }

	public Guid TafsilGuid { get; set; }

	[JsonIgnore]
	public string ShortDescription => $"حواله- {Price}";

	[Obsolete]
	public int Id { get; set; }

	public int Ordinal { get; set; }

	public decimal? BankRealAccount { get; set; }

	public string CustomerNationalCode { get; set; }

	public Guid? WageGuid { get; set; }

	public Guid? WageMoeinGuid { get; set; }

	public Guid? WageTafsilGuid { get; set; }

	public decimal WagePrice { get; set; }
}
