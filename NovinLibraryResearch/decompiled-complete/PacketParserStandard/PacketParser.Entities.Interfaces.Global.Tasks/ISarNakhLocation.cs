using System;

namespace PacketParser.Entities.Interfaces.Global.Tasks;

public interface ISarNakhLocation
{
	Guid SarNakhGuid { get; set; }

	int Ordinal { get; set; }

	string Email { get; set; }

	string WebSite { get; set; }

	string Address { get; set; }

	string State { get; set; }

	string City { get; set; }
}
