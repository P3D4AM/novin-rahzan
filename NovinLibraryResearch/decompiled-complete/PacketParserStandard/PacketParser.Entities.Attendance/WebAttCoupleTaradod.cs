using System;
using System.Collections.Generic;
using System.Drawing;
using PacketParser.Attendance;
using PacketParser.Entities.Interfaces.Attendance;

namespace PacketParser.Entities.Attendance;

public class WebAttCoupleTaradod : ICoupleTaradod
{
	public Color BackColorOfCell { get; set; }

	public Color ColorKh { get; set; }

	public Color ColorV { get; set; }

	public string ContextGuid { get; set; }

	public DateTime DateM { get; set; }

	public int DeviceId { get; set; }

	public bool IsEnteghaliTaradod { get; set; }

	public bool IsTaradodVirtual { get; set; }

	public WebAttLogOnPersonel Ktaradod { get; set; }

	public string KTime { get; set; } = "00:00:00";


	public int PersonId { get; set; }

	public int PersonNo { get; set; }

	public List<StatuseTaradodEnumType> StatuseTaradodLst { get; set; }

	public TypeTaradodEnumType TypeTaradod { get; set; }

	public WebAttLogOnPersonel Vtaradod { get; set; }

	public string VTime { get; set; } = "00:00:00";

}
