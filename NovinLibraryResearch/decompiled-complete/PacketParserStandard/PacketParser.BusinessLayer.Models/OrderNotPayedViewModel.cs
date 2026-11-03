using System;

namespace PacketParser.BusinessLayer.Models;

public class OrderNotPayedViewModel
{
	public Guid Guid { get; set; }

	public int Id2 { get; set; }

	public DateTime DateSabt { get; set; }

	public string DateSh => DateSabt.MiladiToCalture();

	public decimal TotalSum { get; set; }
}
