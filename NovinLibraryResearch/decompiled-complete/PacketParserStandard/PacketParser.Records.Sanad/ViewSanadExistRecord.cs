using System;
using PacketParser.Entities;

namespace PacketParser.Records.Sanad;

public class ViewSanadExistRecord
{
	public Guid AsGuid { get; set; }

	public Document AsType { get; set; }
}
