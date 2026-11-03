using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models.LkService;

public class StatusLockForDisplay
{
	public StatusLock StatusLock { get; set; }

	public string StatusLkForDisplay => StatusLock.GetDisplay().GetTranslate();

	public StatusLockForDisplay(StatusLock statusLock)
	{
		StatusLock = statusLock;
	}
}
