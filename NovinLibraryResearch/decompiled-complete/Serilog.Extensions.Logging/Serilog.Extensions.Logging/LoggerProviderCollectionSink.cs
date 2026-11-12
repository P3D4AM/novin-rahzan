using System;
using Microsoft.Extensions.Logging;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Extensions.Logging;

internal class LoggerProviderCollectionSink : ILogEventSink, IDisposable
{
	private readonly LoggerProviderCollection _providers;

	public LoggerProviderCollectionSink(LoggerProviderCollection providers)
	{
		_providers = providers ?? throw new ArgumentNullException("providers");
	}

	public void Emit(LogEvent logEvent)
	{
		string categoryName = "None";
		EventId eventId = default(EventId);
		if (logEvent.Properties.TryGetValue("SourceContext", out LogEventPropertyValue value) && value is ScalarValue { Value: string value2 })
		{
			categoryName = value2;
		}
		if (logEvent.Properties.TryGetValue("EventId", out LogEventPropertyValue value3) && value3 is StructureValue structureValue)
		{
			string name = null;
			int id = 0;
			foreach (LogEventProperty property in structureValue.Properties)
			{
				if (property.Name == "Id" && property.Value is ScalarValue scalarValue2 && scalarValue2.Value is int num)
				{
					id = num;
				}
				if (property.Name == "Name" && property.Value is ScalarValue { Value: string value4 })
				{
					name = value4;
				}
			}
			eventId = new EventId(id, name);
		}
		LogLevel logLevel = LevelConvert.ToExtensionsLevel(logEvent.Level);
		SerilogLogValues state = new SerilogLogValues(logEvent.MessageTemplate, logEvent.Properties);
		foreach (ILoggerProvider provider in _providers.Providers)
		{
			provider.CreateLogger(categoryName).Log(logLevel, eventId, state, logEvent.Exception, (SerilogLogValues s, Exception? e) => s.ToString());
		}
	}

	public void Dispose()
	{
		_providers.Dispose();
	}
}
