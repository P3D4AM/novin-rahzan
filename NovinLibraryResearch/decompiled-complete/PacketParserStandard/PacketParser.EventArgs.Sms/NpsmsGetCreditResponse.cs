using PacketParser.Entities;

namespace PacketParser.EventArgs.Sms;

public class NpsmsGetCreditResponse
{
	public string Credit { get; set; }

	public ResponseResult Message { get; set; }
}
