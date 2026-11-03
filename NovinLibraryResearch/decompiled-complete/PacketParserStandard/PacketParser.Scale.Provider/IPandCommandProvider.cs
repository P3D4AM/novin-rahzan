using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Scale.Provider;

public interface IPandCommandProvider : IScaleCommandProvider, INpDisposable, IDisposable
{
}
