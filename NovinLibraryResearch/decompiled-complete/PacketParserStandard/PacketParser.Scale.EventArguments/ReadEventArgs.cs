using System;

namespace PacketParser.Scale.EventArguments;

public class ReadEventArgs : System.EventArgs
{
	public string Data { get; set; }

	public byte[] Bytes { get; internal set; }
}
