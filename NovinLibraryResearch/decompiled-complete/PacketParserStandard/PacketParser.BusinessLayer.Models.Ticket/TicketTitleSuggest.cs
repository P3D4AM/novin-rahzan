using Newtonsoft.Json;

namespace PacketParser.BusinessLayer.Models.Ticket;

public class TicketTitleSuggest
{
	public string Titel { get; set; }

	[JsonIgnore]
	public bool IsChecked { get; set; }
}
