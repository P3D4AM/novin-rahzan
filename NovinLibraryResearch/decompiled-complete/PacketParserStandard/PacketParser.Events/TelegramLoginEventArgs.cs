using System;
using System.Threading.Tasks;

namespace PacketParser.Events;

public class TelegramLoginEventArgs
{
	public string SenderNumber { get; set; }

	public Func<string, Task> SetCode { get; set; }
}
