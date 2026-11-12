using System;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class EventData
{
	private readonly EventDefinitionBase _eventDefinition;

	private readonly Func<EventDefinitionBase, EventData, string> _messageGenerator;

	public virtual EventId EventId => _eventDefinition.EventId;

	public virtual LogLevel LogLevel => _eventDefinition.Level;

	public EventData([JetBrains.Annotations.NotNull] EventDefinitionBase eventDefinition, [JetBrains.Annotations.NotNull] Func<EventDefinitionBase, EventData, string> messageGenerator)
	{
		_eventDefinition = eventDefinition;
		_messageGenerator = messageGenerator;
	}

	public override string ToString()
	{
		return _messageGenerator(_eventDefinition, this);
	}
}
