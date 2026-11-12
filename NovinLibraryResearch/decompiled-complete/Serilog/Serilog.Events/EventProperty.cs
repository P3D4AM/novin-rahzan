namespace Serilog.Events;

internal readonly struct EventProperty
{
	public static EventProperty None;

	public string Name { get; }

	public LogEventPropertyValue Value { get; }

	public EventProperty(string name, LogEventPropertyValue value)
	{
		Guard.AgainstNull(value, "value");
		LogEventProperty.EnsureValidName(name);
		Name = name;
		Value = value;
	}

	public void Deconstruct(out string name, out LogEventPropertyValue value)
	{
		name = Name;
		value = Value;
	}

	public override bool Equals(object? obj)
	{
		if (obj is EventProperty other)
		{
			return Equals(other);
		}
		return false;
	}

	public bool Equals(EventProperty other)
	{
		if (string.Equals(Name, other.Name))
		{
			return object.Equals(Value, other.Value);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (((Name != null) ? Name.GetHashCode() : 0) * 397) ^ ((Value != null) ? Value.GetHashCode() : 0);
	}
}
