using System;
using System.Collections.Generic;
using PacketParser.Access;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public interface IUserAcc : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string Description { get; set; }

	string Name { get; set; }

	AccessLevel UserAccess { get; set; }

	List<Tuple<DateTime, DateTime>> UserTime { get; set; }
}
