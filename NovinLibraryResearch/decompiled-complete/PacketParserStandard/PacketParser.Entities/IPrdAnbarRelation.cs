using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public interface IPrdAnbarRelation : IHasModified
{
	decimal Amount { get; set; }

	Guid AnbarGuid { get; set; }

	Guid PrdGuid { get; set; }

	decimal Min { get; set; }

	decimal Max { get; set; }
}
