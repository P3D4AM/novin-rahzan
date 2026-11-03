using System;

namespace PacketParser.Entities.Interfaces.Global.Tasks;

public interface ISarNakhNumbers
{
	Guid SarNakhGuid { get; set; }

	string Number { get; set; }

	string Title { get; set; }
}
