using System;
using System.Collections.Generic;

namespace PacketParser.BusinessLayer.Models.Crm;

public class CashierNumber
{
	public Guid Guid { get; set; }

	public string Name { get; set; }

	public string UserName { get; set; }

	public List<int> InternalNumber { get; set; }
}
