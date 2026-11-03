using System;

namespace PacketParser.Entities.Interfaces.Accounting;

public interface ICustomerRole : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string WordPressName { get; set; }

	string Name { get; set; }

	string Description { get; set; }

	Guid? PriceTypeGuid { get; set; }
}
