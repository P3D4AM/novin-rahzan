using System.Collections.Generic;
using PacketParser.Entities;

namespace WebSmsStandard.RestEntities;

public class ActiveSendJsonResponse
{
	public List<long> Result { get; set; }

	public ResponseResult Message { get; set; }
}
