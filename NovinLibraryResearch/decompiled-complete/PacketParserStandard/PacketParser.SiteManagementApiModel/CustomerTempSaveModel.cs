using System;
using PacketParser.Entities;

namespace PacketParser.SiteManagementApiModel;

public class CustomerTempSaveModel
{
	public short DepId { get; set; }

	public Guid Token { get; set; }

	public ENSource ENSource { get; set; }

	public WebCus Customer { get; set; }
}
