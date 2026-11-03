using System;

namespace PacketParser.Entities;

public class WebTafsilNumberReport
{
	public Guid TafsilGuid { get; set; }

	public Guid Guid { get; set; }

	public string Title { get; set; }

	public long OriginalNumber { get; set; }

	public string TitleWithNumber => "(" + Title + " : " + OriginalNumber + ")";

	public string Name { get; set; }
}
