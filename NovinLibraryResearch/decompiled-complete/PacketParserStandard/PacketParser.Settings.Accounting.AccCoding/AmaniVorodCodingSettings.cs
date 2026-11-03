using System;
using PacketParser.DefaultsCoding;

namespace PacketParser.Settings.Accounting.AccCoding;

public class AmaniVorodCodingSettings
{
	private Guid debit_MoeinGuid_Product = Guid.Empty;

	public Guid Debit_MoeinGuid_Product
	{
		get
		{
			if (debit_MoeinGuid_Product == Guid.Empty || debit_MoeinGuid_Product == ParentDefaults.MoeinCoding.CLSMoein50)
			{
				debit_MoeinGuid_Product = ParentDefaults.MoeinCoding.CLSMoein_HesabEntezami_Digaran;
			}
			return debit_MoeinGuid_Product;
		}
		set
		{
			debit_MoeinGuid_Product = value;
		}
	}

	public Guid? Debit_TafsilGuid_Product => null;
}
