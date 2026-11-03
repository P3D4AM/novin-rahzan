using System;
using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models;

public class GardeshDifferenceModel
{
	public Guid AsGuid { get; set; }

	public Document AsType { get; set; }

	public string Description { get; set; }

	public decimal Debit { get; set; }

	public decimal Credit { get; set; }
}
