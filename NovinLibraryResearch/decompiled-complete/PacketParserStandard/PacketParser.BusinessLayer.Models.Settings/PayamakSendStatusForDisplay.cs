using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models.Settings;

public class PayamakSendStatusForDisplay
{
	public PayamakSendStatus Status { get; set; }

	public string StatusForDisplay => Status.GetDisplay().GetTranslate();

	public PayamakSendStatusForDisplay(PayamakSendStatus status)
	{
		Status = status;
	}
}
