using System;
using System.Threading.Tasks;
using PacketParser.Entities;

namespace PacketParser.Assistance;

public class EventLogger
{
	public static EventLogger Instance = new EventLogger();

	public Guid Guid { get; set; } = Guid.NewGuid();


	public string ReferencedId { get; set; } = Guid.NewGuid().ToString();


	public EventLoggerType EventLoggerType { get; set; }

	public ReturnedState State { get; set; }

	public string Title { get; set; }

	public string Message { get; set; }

	public string Description { get; set; }

	public DateTime Date { get; set; } = DateTime.Now;


	public object Sender { get; set; }

	public event Func<object, EventLogger, Task> OnHappened;

	protected EventLogger()
	{
	}

	public EventLogger(EventLoggerType eventLoggerType, string message, ReturnedState state = ReturnedState.Success, string title = "", string referencedId = "", string description = "", DateTime? date = null)
	{
		EventLoggerType = eventLoggerType;
		State = state;
		Message = message;
		Description = description;
		Title = title;
		if (!string.IsNullOrEmpty(referencedId))
		{
			ReferencedId = referencedId;
		}
		if (date.HasValue)
		{
			Date = date.Value;
		}
		Task.Run(() => Instance.OnHappened?.Invoke(Sender, this));
	}

	public static EventLogger NewInstance(EventLoggerType eventLoggerType, string message, ReturnedState state = ReturnedState.Success, string title = "", string referencedId = "", string description = "", DateTime? date = null)
	{
		return new EventLogger(eventLoggerType, message, state, title, referencedId, description, date);
	}
}
