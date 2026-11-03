using System;

namespace PacketParser.Entities.Interfaces;

public interface INpAsyncDisposable : IAsyncDisposable
{
	bool IsDisposed { get; }
}
