using System;

namespace PacketParser.Entities.Department;

public class ActivationTBL
{
	public int ID { get; set; }

	public string InputMsg { get; set; }

	public string OutputMsg { get; set; } = "";


	public string Number { get; set; } = "";


	public DateTime DateM { get; set; } = DateTime.Now;


	public string DateSH
	{
		get
		{
			return DateM.MiladiToShamsi();
		}
		set
		{
		}
	}

	public long LKSerial { get; set; }

	public string SoftSerial { get; set; }

	public string ExpiredDateSH
	{
		get
		{
			return ExpiredDateM.MiladiToShamsi();
		}
		set
		{
		}
	}

	public DateTime ExpiredDateM { get; set; }

	public long RefrencedID { get; set; }

	public short ST { get; set; }
}
