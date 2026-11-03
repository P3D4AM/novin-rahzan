using System;
using PacketParser.DefaultsCoding;

namespace PacketParser.Settings.Accounting.AccCoding;

public class ZayeatCodingSettings
{
	private Guid credit_MoeinGuid_Product = Guid.Empty;

	private Guid? credit_TafsilGuid_Product = Guid.Empty;

	public Guid Credit_MoeinGuid_Product
	{
		get
		{
			if (credit_MoeinGuid_Product == Guid.Empty)
			{
				credit_MoeinGuid_Product = ParentDefaults.MoeinCoding.CLSMoein50;
			}
			return credit_MoeinGuid_Product;
		}
		set
		{
			credit_MoeinGuid_Product = value;
		}
	}

	public Guid? Credit_TafsilGuid_Product
	{
		get
		{
			if (credit_TafsilGuid_Product == Guid.Empty)
			{
				credit_TafsilGuid_Product = ParentDefaults.TafsilCoding.clsTafsilInventory;
			}
			return credit_TafsilGuid_Product;
		}
		set
		{
			credit_TafsilGuid_Product = value;
		}
	}
}
