using System;

namespace PacketParser.Entities.SerializeData;

public class SandoghData
{
	[Obsolete("Use NegativeAmount", false)]
	public bool SandoghManfi { get; set; }

	public ReturnedState? NegativeAmount { get; set; }
}
