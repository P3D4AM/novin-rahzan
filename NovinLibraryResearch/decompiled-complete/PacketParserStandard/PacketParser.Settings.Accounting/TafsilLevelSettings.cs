using System;
using PacketParser.DefaultsCoding;

namespace PacketParser.Settings.Accounting;

public class TafsilLevelSettings
{
	public Guid CustomerMoeinGuid
	{
		get
		{
			if (CustomerMoeinGuid == Guid.Empty)
			{
				CustomerMoeinGuid = ParentDefaults.MoeinCoding.CLSMoein9;
			}
			return CustomerMoeinGuid;
		}
		set
		{
			CustomerMoeinGuid = value;
		}
	}

	public Guid SandoghMoeinGuid
	{
		get
		{
			if (SandoghMoeinGuid == Guid.Empty)
			{
				SandoghMoeinGuid = ParentDefaults.MoeinCoding.CLSMoein7;
			}
			return SandoghMoeinGuid;
		}
		set
		{
			SandoghMoeinGuid = value;
		}
	}

	public Guid BankMoeinGuid
	{
		get
		{
			if (BankMoeinGuid == Guid.Empty)
			{
				BankMoeinGuid = ParentDefaults.MoeinCoding.CLSMoein4;
			}
			return BankMoeinGuid;
		}
		set
		{
			BankMoeinGuid = value;
		}
	}

	public Guid HazineMoeinGuid
	{
		get
		{
			if (HazineMoeinGuid == Guid.Empty)
			{
				HazineMoeinGuid = ParentDefaults.MoeinCoding.CLSMoein11;
			}
			return HazineMoeinGuid;
		}
		set
		{
			HazineMoeinGuid = value;
		}
	}

	public Guid BardashtMoeinGuid
	{
		get
		{
			if (BardashtMoeinGuid == Guid.Empty)
			{
				BardashtMoeinGuid = ParentDefaults.MoeinCoding.CLSMoein3;
			}
			return BardashtMoeinGuid;
		}
		set
		{
			BardashtMoeinGuid = value;
		}
	}

	public Guid SayerMoeinGuid
	{
		get
		{
			if (SayerMoeinGuid == Guid.Empty)
			{
				SayerMoeinGuid = ParentDefaults.MoeinCoding.CLSMoein9;
			}
			return SayerMoeinGuid;
		}
		set
		{
			SayerMoeinGuid = value;
		}
	}
}
