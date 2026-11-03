using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public interface IPardakhtCheckM : IParDarDet, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	Guid GuidCheck { get; set; }

	Guid MasterGuid { get; set; }

	string SayadiNationalCode { get; set; }

	string SayadiName { get; set; }

	bool SayadiRegistered { get; set; }
}
