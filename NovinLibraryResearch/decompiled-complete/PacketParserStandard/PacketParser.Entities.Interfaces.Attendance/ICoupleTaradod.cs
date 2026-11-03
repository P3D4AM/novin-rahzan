using System;
using System.Collections.Generic;
using System.Drawing;
using PacketParser.Attendance;
using PacketParser.Entities.Attendance;

namespace PacketParser.Entities.Interfaces.Attendance;

public interface ICoupleTaradod
{
	Color BackColorOfCell { get; set; }

	Color ColorKh { get; set; }

	Color ColorV { get; set; }

	string ContextGuid { get; set; }

	DateTime DateM { get; set; }

	int DeviceId { get; set; }

	bool IsEnteghaliTaradod { get; }

	bool IsTaradodVirtual { get; }

	string KTime { get; }

	int PersonId { get; set; }

	int PersonNo { get; set; }

	List<StatuseTaradodEnumType> StatuseTaradodLst { get; set; }

	TypeTaradodEnumType TypeTaradod { get; set; }

	string VTime { get; }
}
