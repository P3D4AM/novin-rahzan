using System;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.TozieList;

namespace PacketParser.Entities;

public class WebTozieListDetail : ITozieListDetail, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid
{
	public Guid Guid { get; set; }

	public Guid MasterGuid { get; set; }

	public Guid AsGuid { get; set; }

	public Document AsType { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }
}
