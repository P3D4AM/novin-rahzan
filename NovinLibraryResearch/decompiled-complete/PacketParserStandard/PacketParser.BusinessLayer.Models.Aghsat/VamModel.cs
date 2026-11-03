using System;

namespace PacketParser.BusinessLayer.Models.Aghsat;

public class VamModel
{
	public Guid Guid { get; set; }

	public int Id2 { get; set; }

	public DateTime DateM { get; set; }

	public string DateSh => DateM.MiladiToCalture();

	public string TafsilName { get; set; }

	public string Description { get; set; }

	public decimal Price { get; set; }
}
