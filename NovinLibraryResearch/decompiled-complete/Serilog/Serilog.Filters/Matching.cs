using System;
using Serilog.Events;

namespace Serilog.Filters;

public static class Matching
{
	public static Func<LogEvent, bool> FromSource<TSource>()
	{
		return WithProperty("SourceContext", typeof(TSource).FullName);
	}

	public static Func<LogEvent, bool> FromSource(string source)
	{
		string source2 = source;
		Guard.AgainstNull(source2, "source");
		return WithProperty("SourceContext", (string s) => s != null && s.StartsWith(source2) && (s.Length == source2.Length || s[source2.Length] == '.'));
	}

	public static Func<LogEvent, bool> WithProperty(string propertyName)
	{
		string propertyName2 = propertyName;
		Guard.AgainstNull(propertyName2, "propertyName");
		return (LogEvent e) => e.Properties.ContainsKey(propertyName2);
	}

	public static Func<LogEvent, bool> WithProperty(string propertyName, object scalarValue)
	{
		string propertyName2 = propertyName;
		Guard.AgainstNull(propertyName2, "propertyName");
		ScalarValue scalar = new ScalarValue(scalarValue);
		LogEventPropertyValue value;
		return (LogEvent e) => e.Properties.TryGetValue(propertyName2, out value) && scalar.Equals(value);
	}

	public static Func<LogEvent, bool> WithProperty<TScalar>(string propertyName, Func<TScalar, bool> predicate)
	{
		string propertyName2 = propertyName;
		Func<TScalar, bool> predicate2 = predicate;
		Guard.AgainstNull(propertyName2, "propertyName");
		Guard.AgainstNull(predicate2, "predicate");
		return delegate(LogEvent e)
		{
			if (!e.Properties.TryGetValue(propertyName2, out LogEventPropertyValue value))
			{
				return false;
			}
			return value is ScalarValue scalarValue && scalarValue.Value is TScalar arg && predicate2(arg);
		};
	}
}
