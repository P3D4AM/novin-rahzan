using System;
using Serilog.Events;

namespace Serilog.Sinks.EventLog;

internal sealed class EventIdHashProvider : IEventIdProvider
{
	public ushort ComputeEventId(LogEvent logEvent)
	{
		return (ushort)Compute(logEvent.MessageTemplate.Text);
	}

	private static int Compute(string messageTemplate)
	{
		if (messageTemplate == null)
		{
			throw new ArgumentNullException("messageTemplate");
		}
		uint num = 0u;
		for (int i = 0; i < messageTemplate.Length; i++)
		{
			num += messageTemplate[i];
			num += num << 10;
			num ^= num >> 6;
		}
		num += num << 3;
		num ^= num >> 11;
		num += num << 15;
		return (ushort)num;
	}
}
