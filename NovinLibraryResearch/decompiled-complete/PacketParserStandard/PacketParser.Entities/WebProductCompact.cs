using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public class WebProductCompact : IProductCompact, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public bool Special { get; set; }

	public bool Status { get; set; }

	public long Code { get; set; }

	public string Description { get; set; }

	public decimal DiscountPercent { get; set; }

	public string IdProduct { get; set; }

	public string Name { get; set; }

	public string Pic { get; set; }

	public decimal Sell1 { get; set; }

	public decimal Sell1Qt { get; set; }

	public decimal Sell2 { get; set; }

	public decimal Sell2Qt { get; set; }

	public decimal Sell3 { get; set; }

	public decimal Sell3Qt { get; set; }

	public decimal Sell4 { get; set; }

	public decimal Sell4Qt { get; set; }

	public decimal Sell5 { get; set; }

	public decimal Sell5Qt { get; set; }

	public decimal Sell6 { get; set; }

	public decimal Sell6Qt { get; set; }

	public decimal Sell7 { get; set; }

	public decimal Sell7Qt { get; set; }

	public decimal Sell8 { get; set; }

	public decimal Sell8Qt { get; set; }

	public Guid? V2Guid { get; set; }

	public decimal V2Qt { get; set; }

	public Guid GuidGroup { get; set; }

	public Guid GuidVahed { get; set; }

	public decimal Amount { get; set; }

	public decimal OrderPoint { get; set; }

	public bool IsTax { get; set; }

	public string Tags { get; set; }

	public EnProductType Type { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }
}
