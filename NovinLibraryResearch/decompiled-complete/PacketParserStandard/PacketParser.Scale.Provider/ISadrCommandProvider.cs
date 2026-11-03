using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Scale.Provider;

public interface ISadrCommandProvider : IScaleCommandProvider, INpDisposable, IDisposable
{
}
