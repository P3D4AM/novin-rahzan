using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Scale.Provider;

public interface ITakinCommandProvider : IScaleCommandProvider, INpDisposable, IDisposable
{
}
