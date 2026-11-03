using System;
using PacketParser.DefaultsCoding;

namespace PacketParser.Settings.Accounting;

public class VajhEltezamSetting
{
	private Guid creditMoeinGuid = Guid.Empty;

	private Guid creditPenaltyMoeinGuid = Guid.Empty;

	private Guid? creditTafsilGuid;

	private Guid? creditPenaltyTafsilGuid;

	private string _vajhEltezamSanadDescription;

	private string _profitSanadDescription;

	public int NumberOfPenaltyDays { get; set; }

	public decimal PenaltyPercent { get; set; }

	public decimal CommitionPercent { get; set; }

	public Guid CreditMoeinGuid
	{
		get
		{
			if (creditMoeinGuid == Guid.Empty)
			{
				creditMoeinGuid = ParentDefaults.MoeinCoding.clsMoeinPenalty;
			}
			return creditMoeinGuid;
		}
		set
		{
			creditMoeinGuid = value;
		}
	}

	public Guid? CreditTafsilGuid
	{
		get
		{
			if ((creditTafsilGuid ?? Guid.Empty) == Guid.Empty)
			{
				creditTafsilGuid = ParentDefaults.TafsilCoding.CLSTafsil440;
			}
			return creditTafsilGuid;
		}
		set
		{
			creditTafsilGuid = value;
		}
	}

	public Guid CreditPenaltyMoeinGuid
	{
		get
		{
			if (creditPenaltyMoeinGuid == Guid.Empty)
			{
				creditPenaltyMoeinGuid = ParentDefaults.MoeinCoding.clsMoeinPenalty;
			}
			return creditPenaltyMoeinGuid;
		}
		set
		{
			creditPenaltyMoeinGuid = value;
		}
	}

	public Guid? CreditPenaltyTafsilGuid
	{
		get
		{
			if ((creditPenaltyTafsilGuid ?? Guid.Empty) == Guid.Empty)
			{
				creditPenaltyTafsilGuid = ParentDefaults.TafsilCoding.CLSTafsil440;
			}
			return creditPenaltyTafsilGuid;
		}
		set
		{
			creditPenaltyTafsilGuid = value;
		}
	}

	public string VajhEltezamSanadDescription
	{
		get
		{
			if (string.IsNullOrEmpty(_vajhEltezamSanadDescription))
			{
				_vajhEltezamSanadDescription = "جریمه دیرکرد قسط ([نام طرف حساب]) شماره [شماره قسط] به سررسید [تاریخ سررسید] و مبلغ [مبلغ قسط] دارای [تعداد روز تاخیر] روز تاخیر".GetTranslate();
			}
			return _vajhEltezamSanadDescription;
		}
		set
		{
			_vajhEltezamSanadDescription = value;
		}
	}

	public string ProfitSanadDescription
	{
		get
		{
			if (string.IsNullOrEmpty(_profitSanadDescription))
			{
				_profitSanadDescription = "کارمزد فروش اقساطی".GetTranslate();
			}
			return _profitSanadDescription;
		}
		set
		{
			_profitSanadDescription = value;
		}
	}
}
