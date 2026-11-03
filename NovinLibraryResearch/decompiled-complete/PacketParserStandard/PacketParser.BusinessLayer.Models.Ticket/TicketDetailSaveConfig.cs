using System.Collections.Generic;
using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models.Ticket;

public class TicketDetailSaveConfig
{
	public EnSystem System { get; set; }

	public ITicket MasterTicket { get; set; }

	public ITicketDet TicketDetail { get; set; }

	public IEnumerable<IFileInfo> AddedFiles { get; set; }

	public IEnumerable<IFileInfo> RemovedFiles { get; set; }

	public bool SendToServer { get; set; }

	public bool AwaitOnSemaphore { get; set; } = true;


	public bool AwaitForSendToServer { get; set; }

	public Priority PriorityOfSendToServer { get; set; }

	public string CustomerUserName { get; set; } = "";


	public string CustomerPassword { get; set; } = "";


	public int Timeout { get; set; }
}
