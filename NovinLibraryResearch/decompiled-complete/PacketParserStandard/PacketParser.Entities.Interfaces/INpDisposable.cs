using System;

namespace PacketParser.Entities.Interfaces;

public interface INpDisposable : IDisposable
{
	bool IsDisposed { get; }
}
