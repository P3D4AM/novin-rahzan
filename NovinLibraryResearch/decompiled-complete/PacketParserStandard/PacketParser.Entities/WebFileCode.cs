using System;

namespace PacketParser.Entities;

[Serializable]
public class WebFileCode : IFileCode
{
	public int ID { get; set; }

	public string Name { get; set; }

	public DateTime Modified { get; set; }
}
