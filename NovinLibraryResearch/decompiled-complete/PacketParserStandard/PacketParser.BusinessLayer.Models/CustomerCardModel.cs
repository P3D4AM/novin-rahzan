using System;

namespace PacketParser.BusinessLayer.Models;

public class CustomerCardModel
{
	public Guid Guid { get; set; }

	public bool Status { get; set; } = true;


	public Guid CustomerGuid { get; set; }

	public string CustomerName { get; set; }

	public long CustomerCode { get; set; }

	public string CardName { get; set; }

	public string CardNo { get; set; }

	public string Pass { get; set; }

	public string Description { get; set; }
}
