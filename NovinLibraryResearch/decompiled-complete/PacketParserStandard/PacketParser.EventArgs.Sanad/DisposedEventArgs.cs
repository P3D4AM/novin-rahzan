using System;
using PacketParser.Entities;

namespace PacketParser.EventArgs.Sanad;

public class DisposedEventArgs : global::System.EventArgs
{
	public EnSystem System { get; private set; }

	public DisposedEventArgs(EnSystem system)
	{
		System = system;
	}
}
