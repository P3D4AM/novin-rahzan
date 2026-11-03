using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Connection.LowLevel;

public interface IGenericSignalRClient : INpAsyncDisposable, IAsyncDisposable
{
	string ContextGuid { get; set; }
}
