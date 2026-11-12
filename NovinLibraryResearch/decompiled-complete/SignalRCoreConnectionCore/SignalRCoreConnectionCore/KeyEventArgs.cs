using System;

namespace SignalRCoreConnectionCore;

internal class KeyEventArgs : EventArgs
{
	public byte[] SymmetricKey { get; set; }

	public byte[] SymmetricIv { get; set; }
}
