using System;
using PacketParser.DefaultsCoding;

namespace PacketParser.Settings.Accounting;

public class VamSettings
{
	private Guid baseMoeinGuid = Guid.Empty;

	private Guid loanMoeinGuid = Guid.Empty;

	public Guid BaseMoeinGuid
	{
		get
		{
			if (baseMoeinGuid == Guid.Empty)
			{
				baseMoeinGuid = ParentDefaults.MoeinCoding.CLSMoein9;
			}
			return baseMoeinGuid;
		}
		set
		{
			baseMoeinGuid = value;
		}
	}

	public Guid LoanMoeinGuid
	{
		get
		{
			if (loanMoeinGuid == Guid.Empty)
			{
				loanMoeinGuid = ParentDefaults.MoeinCoding.CLSMoein29815;
			}
			return loanMoeinGuid;
		}
		set
		{
			loanMoeinGuid = value;
		}
	}
}
