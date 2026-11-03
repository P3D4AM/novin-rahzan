using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Scale.Provider;

public interface IRadinCommandProvider : IScaleCommandProvider, INpDisposable, IDisposable
{
}
