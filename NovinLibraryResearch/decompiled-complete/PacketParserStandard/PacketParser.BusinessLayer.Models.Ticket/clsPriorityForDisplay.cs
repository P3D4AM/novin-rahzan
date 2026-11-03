using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models.Ticket;

public class clsPriorityForDisplay
{
	public Priority Priority { get; private set; }

	public string PriorityForDisplay => Priority.GetDisplay().GetTranslate();

	public clsPriorityForDisplay(Priority _priority)
	{
		Priority = _priority;
	}
}
