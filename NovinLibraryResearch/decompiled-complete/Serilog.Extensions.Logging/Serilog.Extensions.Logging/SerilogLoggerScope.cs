using System;
using System.Collections.Generic;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Extensions.Logging;

internal class SerilogLoggerScope : IDisposable
{
	private const string NoName = "None";

	private readonly SerilogLoggerProvider _provider;

	private readonly object? _state;

	private readonly IDisposable? _chainedDisposable;

	private bool _disposed;

	public SerilogLoggerScope? Parent { get; }

	public SerilogLoggerScope(SerilogLoggerProvider provider, object? state, IDisposable? chainedDisposable = null)
	{
		_provider = provider;
		_state = state;
		Parent = _provider.CurrentScope;
		_provider.CurrentScope = this;
		_chainedDisposable = chainedDisposable;
	}

	public void Dispose()
	{
		if (_disposed)
		{
			return;
		}
		_disposed = true;
		for (SerilogLoggerScope serilogLoggerScope = _provider.CurrentScope; serilogLoggerScope != null; serilogLoggerScope = serilogLoggerScope.Parent)
		{
			if (serilogLoggerScope == this)
			{
				_provider.CurrentScope = Parent;
			}
		}
		_chainedDisposable?.Dispose();
	}

	public void EnrichAndCreateScopeItem(LogEvent logEvent, ILogEventPropertyFactory propertyFactory, out LogEventPropertyValue? scopeItem)
	{
		ILogEventPropertyFactory propertyFactory2 = propertyFactory;
		LogEvent logEvent2 = logEvent;
		if (_state == null)
		{
			scopeItem = null;
			return;
		}
		if (_state is Dictionary<string, object> dictionary)
		{
			scopeItem = null;
			{
				foreach (KeyValuePair<string, object> item in dictionary)
				{
					if (item.Key == "{OriginalFormat}" && item.Value is string)
					{
						scopeItem = new ScalarValue(_state.ToString());
					}
					else
					{
						AddProperty(item);
					}
				}
				return;
			}
		}
		if (_state is IEnumerable<KeyValuePair<string, object>> enumerable)
		{
			scopeItem = null;
			{
				foreach (KeyValuePair<string, object> item2 in enumerable)
				{
					if (item2.Key == "{OriginalFormat}" && item2.Value is string)
					{
						scopeItem = new ScalarValue(_state.ToString());
					}
					else
					{
						AddProperty(item2);
					}
				}
				return;
			}
		}
		scopeItem = propertyFactory2.CreateProperty("None", _state).Value;
		void AddProperty(KeyValuePair<string, object> stateProperty)
		{
			string text = stateProperty.Key;
			bool destructureObjects = false;
			object obj = stateProperty.Value;
			if (text.StartsWith("@"))
			{
				text = SerilogLogger.GetKeyWithoutFirstSymbol(SerilogLogger.DestructureDictionary, text);
				destructureObjects = true;
			}
			else if (text.StartsWith("$"))
			{
				text = SerilogLogger.GetKeyWithoutFirstSymbol(SerilogLogger.StringifyDictionary, text);
				obj = obj?.ToString();
			}
			LogEventProperty property = propertyFactory2.CreateProperty(text, obj, destructureObjects);
			logEvent2.AddPropertyIfAbsent(property);
		}
	}
}
