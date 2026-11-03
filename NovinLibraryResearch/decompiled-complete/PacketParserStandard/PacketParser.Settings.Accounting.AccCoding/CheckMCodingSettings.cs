using System;
using System.ComponentModel;
using PacketParser.DefaultsCoding;

namespace PacketParser.Settings.Accounting.AccCoding;

public class CheckMCodingSettings
{
	private Guid debit_MoeinGuid_Reception;

	private Guid debit_TafsilGuid_Reception;

	[DefaultValue(29766)]
	public int Debit_Tafsil_Reception { get; set; } = 29766;


	[DefaultValue(29766)]
	public int a
	{
		get
		{
			return Debit_Tafsil_Reception;
		}
		set
		{
			Debit_Tafsil_Reception = value;
		}
	}

	[DefaultValue(29976)]
	public int Debit_Moein_Reception { get; set; } = 29976;


	[DefaultValue(29976)]
	public int b
	{
		get
		{
			return Debit_Moein_Reception;
		}
		set
		{
			Debit_Moein_Reception = value;
		}
	}

	public Guid Debit_MoeinGuid_Reception
	{
		get
		{
			if (debit_MoeinGuid_Reception == Guid.Empty)
			{
				debit_MoeinGuid_Reception = ParentDefaults.MoeinCoding.CLSMoein29976;
			}
			return debit_MoeinGuid_Reception;
		}
		set
		{
			debit_MoeinGuid_Reception = value;
		}
	}

	public Guid Debit_TafsilGuid_Reception
	{
		get
		{
			if (debit_TafsilGuid_Reception == Guid.Empty)
			{
				debit_TafsilGuid_Reception = ParentDefaults.TafsilCoding.CLSTafsil29766;
			}
			return debit_TafsilGuid_Reception;
		}
		set
		{
			debit_TafsilGuid_Reception = value;
		}
	}
}
