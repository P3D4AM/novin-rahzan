using System.Collections.Generic;
using PacketParser.Entities;

namespace WebSmsStandard.RestEntities;

public class NpsmsGetDeliveryResponse
{
	public List<int> Result { get; set; }

	public ResponseResult Message { get; set; }
}
