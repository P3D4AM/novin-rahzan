using System;

namespace PacketParser.Entities.Interfaces.Advertise;

public interface ISimCard : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	DateTime DivarModified { get; set; }

	DateTime SheypoorModified { get; set; }

	long Number { get; set; }

	Guid OwnerGuid { get; set; }

	int UseCount { get; set; }

	string Operator { get; set; }

	string UserName { get; set; }

	DateTime NextUseDivar { get; set; }

	DateTime NextUseSheypoor { get; set; }

	bool isSheypoorBlocked { get; set; }

	bool isSendDivarAdv { get; set; }

	bool isSendSheypoorAdv { get; set; }
}
