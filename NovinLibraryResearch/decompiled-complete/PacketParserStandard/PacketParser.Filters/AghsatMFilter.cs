using System;
using System.Collections.Generic;
using PacketParser.Entities;

namespace PacketParser.Filters;

public class AghsatMFilter
{
	public short PardakhtType { get; set; }

	public Guid? TafsilGuid { get; set; }

	public Guid? GroupGuid { get; set; }

	public EnAghsatMStatus? Status { get; set; }

	public DateTime? DateFrom { get; set; }

	public DateTime? DateTo { get; set; }

	public Guid? CusGroupGuid { get; set; }

	public Guid? ReceptionGuid { get; set; }

	public List<Guid> TafsilGuidList { get; set; }
}
