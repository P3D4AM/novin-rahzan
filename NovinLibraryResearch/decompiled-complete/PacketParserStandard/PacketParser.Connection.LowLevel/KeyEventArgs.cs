using System;

namespace PacketParser.Connection.LowLevel;

public class KeyEventArgs : System.EventArgs
{
	public byte[] SymmetricKey { get; set; }

	public byte[] SymmetricIv { get; set; }
}
