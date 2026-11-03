using System;
using Newtonsoft.Json;
using PacketParser.Activation;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Department;

namespace PacketParser.Entities.Department;

public class LKInfo : Serializable<LKInfo>, ILkInfo, IHasModified, IHasStatus
{
	public Guid Guid { get; set; }

	public AccountingPackage AccountingPackage { get; set; }

	public ENCode TypeAccounting { get; set; }

	public HozoorPackage HozoorType { get; set; }

	public string StrTypeAccounting
	{
		get
		{
			return TypeAccounting.GetDisplay();
		}
		set
		{
		}
	}

	public long LKSerial { get; set; }

	public Guid TafsilGuid { get; set; }

	public DateTime ExpireDate { get; set; } = DateTime.Now;


	public string ExpireDateSh
	{
		get
		{
			return ExpireDate.MiladiToShamsi();
		}
		set
		{
		}
	}

	public string StartDateSh
	{
		get
		{
			return StartDate.MiladiToShamsi();
		}
		set
		{
		}
	}

	public DateTime StartDate { get; set; } = DateTime.Now;


	public string Description { get; set; } = "";


	public bool Tolid { get; set; }

	public bool BarCode { get; set; }

	public bool Scale { get; set; }

	public bool SpecialAccountingPackage { get; set; }

	public bool IsOffline { get; set; }

	public TypeHozor Hozor_Hoghogh { get; set; }

	public string StrHozor_Hoghogh
	{
		get
		{
			return Hozor_Hoghogh.GetDisplay();
		}
		set
		{
		}
	}

	public string URLTamdid => "http://npsms.ir/active.aspx?LK=" + LKSerial;

	public Customer Customer_CLS { get; set; }

	public short ScaleCount { get; set; }

	public short MaxActivationDays { get; set; }

	public short Net_Accounting { get; set; }

	public short Net_Hozor { get; set; }

	public short Net_Scales { get; set; }

	public bool CallerID { get; set; }

	[Obsolete("", true)]
	public decimal TamdidPrice_Total_1year_Net { get; set; }

	public decimal TamdidPrice_Total_5year_Net { get; set; }

	public decimal TamdidPrice_Total_10year_Net { get; set; }

	public DateTime? DisableActivationCodeDate { get; set; }

	public LkInfoMeta MetaData { get; set; }

	public int RemainingDays => Calendar.RemainingDays(ExpireDate);

	public int UsedDays => Calendar.UsedDays(ExpireDate);

	public DateTime? FailedDate { get; set; }

	public string FailedDescription { get; set; } = "";


	public bool IsFailed => !string.IsNullOrEmpty(FailedDescription);

	public DateTime Modified { get; set; }

	public bool Status => true;

	bool IHasStatus.Status { get; }

	public EnLK_SpecialFacilities SpecialFacilities { get; set; }

	[JsonIgnore]
	public bool HasSpecialInstallment => (SpecialFacilities & EnLK_SpecialFacilities.Aghsat) == EnLK_SpecialFacilities.Aghsat;

	[JsonIgnore]
	public bool HasSpecialTax => (SpecialFacilities & EnLK_SpecialFacilities.Tax) == EnLK_SpecialFacilities.Tax;

	[JsonIgnore]
	public bool HasSpecialAghsat => (SpecialFacilities & EnLK_SpecialFacilities.Aghsat) == EnLK_SpecialFacilities.Aghsat;

	[JsonIgnore]
	public bool HasSpecialCheque_Khazane => (SpecialFacilities & EnLK_SpecialFacilities.Cheque) == EnLK_SpecialFacilities.Cheque;

	[JsonIgnore]
	public bool HasSpecialCustomerClub => (SpecialFacilities & EnLK_SpecialFacilities.CustomerClub) == EnLK_SpecialFacilities.CustomerClub;

	[JsonIgnore]
	public bool HasSpecialProductDepot_Pishrafteh => (SpecialFacilities & EnLK_SpecialFacilities.Depot) == EnLK_SpecialFacilities.Depot;

	[JsonIgnore]
	public bool HasSpecialService => (SpecialFacilities & EnLK_SpecialFacilities.Service) == EnLK_SpecialFacilities.Service;

	[JsonIgnore]
	public bool HasSpecialVoiceCall => (SpecialFacilities & EnLK_SpecialFacilities.VoiceCall) == EnLK_SpecialFacilities.VoiceCall;

	[JsonIgnore]
	public bool HasSpecialProductSerial => (SpecialFacilities & EnLK_SpecialFacilities.ProductSerial) == EnLK_SpecialFacilities.ProductSerial;

	[JsonIgnore]
	public bool HasSpecialMultiCurrency => (SpecialFacilities & EnLK_SpecialFacilities.MultiCurrency) == EnLK_SpecialFacilities.MultiCurrency;

	public string DescriptionForTelegram()
	{
		string text = "";
		try
		{
			switch (TypeAccounting)
			{
			case ENCode.ForoshgahiSade:
				text += "\ud83d\udcb0حسابداری ساده";
				break;
			case ENCode.ForoshgahiMotavaset:
				text += "\ud83d\udcb0حسابدرای متوسط";
				break;
			case ENCode.ForoshgahiPishrafteh:
				text += "\ud83d\udcb0حسابدرای پیشرفته";
				break;
			case ENCode.Jame:
				text += "\ud83d\udcb0حسابداری جامع";
				break;
			}
			if (text.Length > 0)
			{
				text += "-";
			}
			switch (Hozor_Hoghogh)
			{
			case TypeHozor.Level1:
				text += "\ud83d\udd70حضور سطح1";
				break;
			case TypeHozor.Level2:
				text += "\ud83d\udd70حضور سطح2";
				break;
			case TypeHozor.Level3:
				text += "\ud83d\udd70حضور سطح3";
				break;
			case TypeHozor.Level4:
				text += "\ud83d\udd70حضور سطح4";
				break;
			case TypeHozor.Level5:
				text += "\ud83d\udd70حضور سطح5";
				break;
			}
			if (Scale)
			{
				text += "-⚖\ufe0fترازو";
			}
			if (BarCode)
			{
				text += "-\ud83d\udcb3بارکد";
			}
			if (Tolid)
			{
				text += "-\ud83c\udfedتولید";
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DescriptionForTelegram", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\LkInfo.cs", 104);
		}
		return text;
	}

	public override string ToString()
	{
		string text = "";
		if (TypeAccounting != 0)
		{
			text = text + TypeAccounting.GetDisplay() + "-";
		}
		if (Scale)
		{
			text += "ترازو-";
		}
		if (BarCode)
		{
			text += "بارکد -";
		}
		if (Tolid)
		{
			text += "تولید -";
		}
		if (Hozor_Hoghogh != 0)
		{
			text += Hozor_Hoghogh.GetDisplay();
		}
		return LKSerial + Customer_CLS.FullName + " انقضاء " + ExpireDateSh + text;
	}
}
