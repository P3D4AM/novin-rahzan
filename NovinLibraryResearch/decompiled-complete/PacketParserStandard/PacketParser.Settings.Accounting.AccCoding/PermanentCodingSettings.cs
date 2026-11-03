using System;
using PacketParser.DefaultsCoding;

namespace PacketParser.Settings.Accounting.AccCoding;

public class PermanentCodingSettings
{
	private Guid productMoeinGuid = Guid.Empty;

	private Guid costMoeinGuid = Guid.Empty;

	private Guid? productTafsilGuid;

	private Guid? costTafsilGuid;

	public Guid ProductMoeinGuid
	{
		get
		{
			if (productMoeinGuid == Guid.Empty)
			{
				productMoeinGuid = ParentDefaults.MoeinCoding.CLSMoein50;
			}
			return productMoeinGuid;
		}
		set
		{
			productMoeinGuid = value;
		}
	}

	public Guid CostMoeinGuid
	{
		get
		{
			if (costMoeinGuid == Guid.Empty)
			{
				costMoeinGuid = ParentDefaults.MoeinCoding.CLSMoein29780;
			}
			return costMoeinGuid;
		}
		set
		{
			costMoeinGuid = value;
		}
	}

	public Guid? ProductTafsilGuid
	{
		get
		{
			if ((productTafsilGuid ?? Guid.Empty) == Guid.Empty)
			{
				productTafsilGuid = ParentDefaults.TafsilCoding.clsTafsilInventory;
			}
			return productTafsilGuid;
		}
		set
		{
			productTafsilGuid = value;
		}
	}

	public Guid? CostTafsilGuid
	{
		get
		{
			if ((costTafsilGuid ?? Guid.Empty) == Guid.Empty)
			{
				costTafsilGuid = ParentDefaults.TafsilCoding.clsTafsilSoldCost;
			}
			return costTafsilGuid;
		}
		set
		{
			costTafsilGuid = value;
		}
	}
}
