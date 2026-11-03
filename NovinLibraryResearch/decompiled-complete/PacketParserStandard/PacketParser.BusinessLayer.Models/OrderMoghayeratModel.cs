using System;

namespace PacketParser.BusinessLayer.Models;

public class OrderMoghayeratModel
{
	public Guid TafsilGuid { get; set; }

	public string ProductName { get; set; }

	public string TafsilName { get; set; }

	public decimal SellSum { get; set; }

	public int SellCount { get; set; }

	public decimal SellQuantity { get; set; }

	public DateTime? FirstSellDate { get; set; }

	public string FirstSellDateSh => FirstSellDate.MiladiToCalture();

	public DateTime? LastSellDate { get; set; }

	public string LastSellDateSh => LastSellDate.MiladiToCalture();

	public decimal BackSellSum { get; set; }

	public int BackSellCount { get; set; }

	public decimal BackSellQuantity { get; set; }

	public DateTime? FirstBackSell { get; set; }

	public string FirstBackSellSh => FirstBackSell.MiladiToCalture();

	public DateTime? LastBackSell { get; set; }

	public string LastBackSellSh => LastBackSell.MiladiToCalture();
}
