using System;
using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models;

public class ProductTolidReportModel
{
	public decimal Quantity { get; set; }

	public Guid ProductGuid { get; set; }

	public Guid TolidFormulaGuid { get; set; }

	public DateTime DateM { get; set; }

	public string DateSh => DateM.MiladiToCalture();

	public string Title { get; set; }

	public EnProductTolidType Type { get; set; }

	public string TypeForDisplay => Type.GetDisplay().GetTranslate();
}
