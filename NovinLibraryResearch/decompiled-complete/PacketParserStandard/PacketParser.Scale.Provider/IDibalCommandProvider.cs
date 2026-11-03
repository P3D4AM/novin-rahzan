using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Scale.Provider;

public interface IDibalCommandProvider : IScaleCommandProvider, INpDisposable, IDisposable
{
}
