using System;
using PacketParser.DefaultsCoding;

namespace PacketParser.Settings.Accounting.AccCoding;

public class AmaniKhorojCodingSettings
{
	private Guid credit_MoeinGuid_Product = Guid.Empty;

	public Guid Credit_MoeinGuid_Product
	{
		get
		{
			if (credit_MoeinGuid_Product == Guid.Empty || credit_MoeinGuid_Product == ParentDefaults.MoeinCoding.CLSMoein50)
			{
				credit_MoeinGuid_Product = ParentDefaults.MoeinCoding.CLSMoein_HesabEntezami_Ma;
			}
			return credit_MoeinGuid_Product;
		}
		set
		{
			credit_MoeinGuid_Product = value;
		}
	}

	public Guid? Credit_TafsilGuid_Product => null;
}
