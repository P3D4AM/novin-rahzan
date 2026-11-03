using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public interface ICustomerAddress : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	short State { get; set; }

	short City { get; set; }

	string Address { get; set; }

	string PostalCode { get; set; }
}
