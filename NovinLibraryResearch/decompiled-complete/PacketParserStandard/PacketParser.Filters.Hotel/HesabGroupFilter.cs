using System;

namespace PacketParser.Filters.Hotel;

public class HesabGroupFilter
{
	public string Name { get; set; }

	public int Code { get; set; }

	public Guid? TypeGuid { get; set; }

	public bool? Status { get; set; }
}
