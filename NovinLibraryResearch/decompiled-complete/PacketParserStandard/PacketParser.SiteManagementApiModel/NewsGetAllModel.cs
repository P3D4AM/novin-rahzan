using System.Collections.Generic;
using PacketParser.Entities;

namespace PacketParser.SiteManagementApiModel;

public class NewsGetAllModel
{
	public short Top { get; set; }

	public List<NewsType> Types { get; set; }
}
