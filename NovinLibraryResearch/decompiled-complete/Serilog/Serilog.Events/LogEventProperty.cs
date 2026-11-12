using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Serilog.Events;

public class LogEventProperty
{
	public string Name { get; }

	public LogEventPropertyValue Value { get; }

	public LogEventProperty(string name, LogEventPropertyValue value)
	{
		Guard.AgainstNull(value, "value");
		EnsureValidName(name);
		Name = name;
		Value = value;
	}

	internal LogEventProperty(EventProperty property)
	{
		if (property.Equals(EventProperty.None))
		{
			throw new ArgumentNullException("property");
		}
		Name = property.Name;
		Value = property.Value;
	}

	public static bool IsValidName([NotNullWhen(true)] string? name)
	{
		return !string.IsNullOrWhiteSpace(name);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void EnsureValidName(string name)
	{
		Guard.AgainstNull(name, "name");
		if (!IsValidName(name))
		{
			throw new ArgumentException("Property name must not be empty or whitespace.", "name");
		}
	}
}
