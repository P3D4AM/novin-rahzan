using System;
using PacketParser.Payamak.Interfaces.Entities;

namespace PacketParser.Payamak.Entities;

public class BaseSms : IBaseSms
{
	private string _number = "";

	private string _customerName = "";

	public int ID { get; set; }

	public string Number
	{
		get
		{
			return _number.Replace("+98", "0");
		}
		set
		{
			_number = value.FixString();
		}
	}

	public string Message { get; set; } = "";


	public DateTime DateM { get; set; } = DateTime.Now;


	public string DateSH => DateM.MiladiToCalture();

	public int CustomerId { get; set; }

	public string CustomerName => _customerName.FixString();

	public string FixedMessage => Message.FixString();

	public bool ST { get; set; } = true;

}
