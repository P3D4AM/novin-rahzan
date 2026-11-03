using System;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace PacketParser.Entities;

public class WebLottery : ILottery, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	[JsonIgnore]
	[XmlIgnore]
	bool IHasStatus.Status => Status == LotteryStatus.Deleted;

	public Guid TafsilGuid { get; set; }

	public Guid CashierGuid { get; set; }

	public Guid? PardakhtGuid { get; set; }

	public DateTime DateM { get; set; }

	public LotteryStatus Status { get; set; }

	public string Description { get; set; }
}
