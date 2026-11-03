using System;

namespace PacketParser.Entities.GPS;

public class GPSLoginResultModel
{
	public short Dep_ID { get; set; }

	public string Token { get; set; }

	public DateTime ExpirationDate { get; set; }
}
