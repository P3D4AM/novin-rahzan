using System;
using PacketParser.DefaultsCoding;

namespace PacketParser.Settings.Accounting.AccCoding;

public class BankActivityCodingSetting
{
	private Guid debit_MoeinGuid_KasrKarmozd = Guid.Empty;

	private Guid debit_TafsilGuid_KasrKarmozd = Guid.Empty;

	private Guid credit_MoeinGuid_VarizSood = Guid.Empty;

	private Guid credit_TafsilGuid_VarizSood = Guid.Empty;

	public Guid Debit_MoeinGuid_KasrKarmozd
	{
		get
		{
			if (debit_MoeinGuid_KasrKarmozd == Guid.Empty)
			{
				debit_MoeinGuid_KasrKarmozd = ParentDefaults.MoeinCoding.CLSMoein34;
			}
			return debit_MoeinGuid_KasrKarmozd;
		}
		set
		{
			debit_MoeinGuid_KasrKarmozd = value;
		}
	}

	public Guid Debit_TafsilGuid_KasrKarmozd
	{
		get
		{
			if (debit_TafsilGuid_KasrKarmozd == Guid.Empty)
			{
				debit_TafsilGuid_KasrKarmozd = ParentDefaults.TafsilCoding.CLSTafsil4400;
			}
			return debit_TafsilGuid_KasrKarmozd;
		}
		set
		{
			debit_TafsilGuid_KasrKarmozd = value;
		}
	}

	public Guid Credit_MoeinGuid_VarizSood
	{
		get
		{
			if (credit_MoeinGuid_VarizSood == Guid.Empty)
			{
				credit_MoeinGuid_VarizSood = ParentDefaults.MoeinCoding.CLSMoein29;
			}
			return credit_MoeinGuid_VarizSood;
		}
		set
		{
			credit_MoeinGuid_VarizSood = value;
		}
	}

	public Guid Credit_TafsilGuid_VarizSood
	{
		get
		{
			if (credit_TafsilGuid_VarizSood == Guid.Empty)
			{
				credit_TafsilGuid_VarizSood = ParentDefaults.TafsilCoding.CLSTafsil440;
			}
			return credit_TafsilGuid_VarizSood;
		}
		set
		{
			credit_TafsilGuid_VarizSood = value;
		}
	}
}
