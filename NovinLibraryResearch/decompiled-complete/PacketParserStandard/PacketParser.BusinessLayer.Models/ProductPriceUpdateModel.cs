using System;

namespace PacketParser.BusinessLayer.Models;

public class ProductPriceUpdateModel
{
	public Guid Guid { get; set; }

	public Guid ProductGuid { get; set; }

	public string ProductName { get; set; }

	public string ProductCode { get; set; }

	public decimal RegularPrice { get; set; }

	public DateTime DateM { get; set; }

	public string DateSh => DateM.MiladiToCalture();

	public string Time => $"{DateM.Hour}:{DateM.Minute}";
}
