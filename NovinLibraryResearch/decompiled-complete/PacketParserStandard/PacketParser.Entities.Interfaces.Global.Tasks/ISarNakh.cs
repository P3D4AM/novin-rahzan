using System;

namespace PacketParser.Entities.Interfaces.Global.Tasks;

public interface ISarNakh : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string Company { get; set; }

	string FullName { get; set; }

	Guid CashierGuid { get; set; }

	string Description { get; set; }

	short StatusId { get; set; }

	Guid? IntroductionGuid { get; set; }

	DateTime CreationDate { get; set; }
}
