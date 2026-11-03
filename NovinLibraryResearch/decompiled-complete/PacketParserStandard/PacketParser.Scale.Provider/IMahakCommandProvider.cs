using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Scale.Provider;

public interface IMahakCommandProvider : IScaleCommandProvider, INpDisposable, IDisposable
{
}
