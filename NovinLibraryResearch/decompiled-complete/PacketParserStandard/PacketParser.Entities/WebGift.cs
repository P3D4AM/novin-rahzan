using System;
using System.Collections.Generic;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Gift;
using PacketParser.Entities.MetaData;

namespace PacketParser.Entities;

public class WebGift : IGift, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public Guid Guid { get; set; }

	public bool Status { get; set; }

	public DateTime Modified { get; set; }

	public string Title { get; set; }

	public string Discription { get; set; }

	public EnTypeChose Type { get; set; }

	public GiftMetaData MetaData { get; set; }

	public List<WebGiftDetail> DetailList { get; set; }
}
