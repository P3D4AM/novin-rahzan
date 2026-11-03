using System;

namespace PacketParser.Entities;

[Serializable]
public class WebBazaryabCust : IBazaryabCust
{
	public int ID { get; set; }

	public int BazaryabID { get; set; }

	public int CustID { get; set; }

	public int MantagheID { get; set; }

	public string Description { get; set; } = "";


	public Guid BazaryabGuid { get; set; }

	public Guid CustGuid { get; set; }

	public Guid Guid { get; set; }

	public Guid MantagheGuid { get; set; }
}
