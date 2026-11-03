using System.Collections.Generic;

namespace PacketParser.SiteManagementApiModel;

public class JsonResponse
{
	public bool Result { get; set; }

	public List<string> Message { get; set; } = new List<string>();

}
