using System;
using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models;

public class TafsilCurrencyGardeshModel
{
	public int SanadNo { get; set; }

	public DateTime DateM { get; set; }

	public string DateSh => DateM.MiladiToCalture();

	public string DecimalName { get; set; }

	public Guid DecimalGuid { get; set; }

	public Guid TafsilGuid { get; set; }

	public Guid AsGuid { get; set; }

	public Document AsType { get; set; }

	public string Description { get; set; }

	public decimal Debit { get; set; }

	public decimal Credit { get; set; }

	public decimal Rem { get; set; }
}
