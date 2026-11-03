using System;
using PacketParser.Entities;

namespace PacketParser.Issabel;

public class IsabelleQueue
{
	public Guid Guid { get; set; }

	public string Title { get; set; }

	public int Number { get; set; }

	public string EndPoint { get; set; }

	public WebTelegramConfig TelegramBot_Abandon { get; set; }

	public WebTelegramConfig TelegramBot_QueueMonitor { get; set; }

	public WebTelegramConfig TelegramBot_Alarm { get; set; }

	public QueueSemaphoreParam WarningConfig { get; set; }

	public QueueSemaphoreParam ErrorConfig { get; set; }
}
