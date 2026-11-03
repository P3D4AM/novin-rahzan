using System;

namespace PacketParser.EventArgs;

public class AccountChangedEventArgs : System.EventArgs
{
	public Guid TafsilGuid { get; set; }

	public string TranName { get; set; }
}
