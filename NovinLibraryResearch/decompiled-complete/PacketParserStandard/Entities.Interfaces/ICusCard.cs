using System;
using PacketParser.Entities.Interfaces;

namespace Entities.Interfaces;

public interface ICusCard : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	Guid CusGuid { get; set; }

	string CardName { get; set; }

	string CardNo { get; set; }

	string Pass { get; set; }

	DateTime DateStart { get; set; }

	DateTime DateExpire { get; set; }

	string Description { get; set; }
}
