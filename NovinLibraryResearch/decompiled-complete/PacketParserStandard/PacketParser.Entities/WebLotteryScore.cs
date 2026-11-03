using System;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace PacketParser.Entities;

public class WebLotteryScore : ILotteryScore, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	private bool _status;

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	[JsonIgnore]
	[XmlIgnore]
	bool IHasStatus.Status => Status == LotteryStatus.Deleted;

	public DateTime ExpirationDate { get; set; }

	public Guid OrderGuid { get; set; }

	public DateTime DateM { get; set; }

	public Guid TafsilGuid { get; set; }

	public Guid CashierGuid { get; set; }

	public decimal Score { get; set; }

	public string Description { get; set; }

	public LotteryStatus Status { get; set; }
}
