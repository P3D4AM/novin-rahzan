using System;
using System.ComponentModel;

namespace PacketParser.Entities;

[Serializable]
public class WebSerialMojood : ISerialMojood
{
	public Guid PrdGuid { get; set; }

	public string Serial { get; set; } = "";


	public string GNCode { get; set; } = "";


	public DateTime StartDate { get; set; } = DateTime.Now.Date;


	public DateTime EndDate { get; set; } = DateTime.Now.Date;


	public bool SMS { get; set; }

	public string SMSMob { get; set; } = "";


	public string Description { get; set; } = "";


	[DefaultValue(true)]
	public bool IsMojod { get; set; } = true;


	public int PrdId { get; set; }

	public int DetID { get; set; }

	public int TypeID { get; set; }

	public short Type { get; set; }

	public DateTime Modified { get; set; }
}
