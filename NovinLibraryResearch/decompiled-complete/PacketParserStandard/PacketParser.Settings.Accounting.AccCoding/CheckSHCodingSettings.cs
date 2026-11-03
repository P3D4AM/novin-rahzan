using System;
using System.ComponentModel;
using PacketParser.DefaultsCoding;

namespace PacketParser.Settings.Accounting.AccCoding;

public class CheckSHCodingSettings
{
	private Guid debit_MoeinGuid_Pardakht;

	private Guid debit_TafsilGuid_Pardakht;

	[DefaultValue(29765)]
	public int Debit_Tafsil_Pardakht { get; set; } = 29765;


	[DefaultValue(10)]
	public int Debit_Moein_Pardakht { get; set; } = 10;


	[DefaultValue(29765)]
	public int a
	{
		get
		{
			return Debit_Tafsil_Pardakht;
		}
		set
		{
			Debit_Tafsil_Pardakht = value;
		}
	}

	[DefaultValue(10)]
	public int b
	{
		get
		{
			return Debit_Moein_Pardakht;
		}
		set
		{
			Debit_Moein_Pardakht = value;
		}
	}

	public Guid Debit_MoeinGuid_Pardakht
	{
		get
		{
			if (debit_MoeinGuid_Pardakht == Guid.Empty)
			{
				debit_MoeinGuid_Pardakht = ParentDefaults.MoeinCoding.CLSMoein10;
			}
			return debit_MoeinGuid_Pardakht;
		}
		set
		{
			debit_MoeinGuid_Pardakht = value;
		}
	}

	public Guid Debit_TafsilGuid_Pardakht
	{
		get
		{
			if (debit_TafsilGuid_Pardakht == Guid.Empty)
			{
				debit_TafsilGuid_Pardakht = ParentDefaults.TafsilCoding.CLSTafsil29765;
			}
			return debit_TafsilGuid_Pardakht;
		}
		set
		{
			debit_TafsilGuid_Pardakht = value;
		}
	}
}
