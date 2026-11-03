using System.Collections.Generic;
using PacketParser.Entities;

namespace PacketParser.SiteManagementApiModel;

public class ContactUsGetAllModel
{
	public short Top { get; set; }

	public List<TypeContact> Types { get; set; }
}
