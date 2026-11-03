using System;
using System.Collections.Generic;

namespace PacketParser.Entities;

public class UserAccCompact
{
	public Guid Guid { get; set; }

	public string UserName { get; set; }

	public string Name { get; set; }

	[Obsolete("This property is obsolete. Use HashedPass instead.", false)]
	public string HashedPass { get; set; }

	public byte[]? SafePassword { get; set; }

	public List<WebCashier> Cashiers { get; set; }
}
