using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Scale.Provider;

public interface ISadrSerialCommandProvider : IScaleCommandProvider, INpDisposable, IDisposable
{
}
