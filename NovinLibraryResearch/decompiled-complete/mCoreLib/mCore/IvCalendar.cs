using System;

namespace mCore;

public interface IvCalendar
{
	string Destination { get; set; }

	DateTime StartDateTime { get; set; }

	DateTime EndDateTime { get; set; }

	string EventDuration { get; set; }

	string Description { get; set; }

	string Summary { get; set; }

	string Location { get; set; }

	string Send();

	string SendToQueue();

	string SendToQueue(QueuePriority Priority);

	string SendFile(string vCalendarFile);

	string SendFileToQueue(string vCalendarFile);

	string SendFileToQueue(string vCalendarFile, QueuePriority Priority);
}
