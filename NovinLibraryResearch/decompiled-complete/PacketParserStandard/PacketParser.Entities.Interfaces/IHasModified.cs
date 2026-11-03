using System;

namespace PacketParser.Entities.Interfaces;

public interface IHasModified
{
	DateTime Modified { get; set; }
}
