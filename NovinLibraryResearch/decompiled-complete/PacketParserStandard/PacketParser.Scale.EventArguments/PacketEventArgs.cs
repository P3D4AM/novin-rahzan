using System;

namespace PacketParser.Scale.EventArguments;

public class PacketEventArgs : System.EventArgs
{
	public string MessageBody { get; set; }
}
