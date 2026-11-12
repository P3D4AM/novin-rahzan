using System;
using System.Globalization;
using System.IO;

namespace Serilog.Events;

public class ScalarValue : LogEventPropertyValue
{
	public static ScalarValue Null { get; } = new ScalarValue(null);


	public object? Value { get; }

	public ScalarValue(object? value)
	{
		Value = value;
	}

	public override void Render(TextWriter output, string? format = null, IFormatProvider? formatProvider = null)
	{
		Render(Value, output, format, formatProvider);
	}

	internal static void Render(object? value, TextWriter output, string? format = null, IFormatProvider? formatProvider = null)
	{
		Guard.AgainstNull(output, "output");
		if (value == null)
		{
			output.Write("null");
			return;
		}
		if (value is string text)
		{
			if (format != "l")
			{
				output.Write('"');
				output.Write(text.Replace("\"", "\\\""));
				output.Write('"');
			}
			else
			{
				output.Write(text);
			}
			return;
		}
		ICustomFormatter customFormatter = (ICustomFormatter)(formatProvider?.GetFormat(typeof(ICustomFormatter)));
		if (customFormatter != null)
		{
			output.Write(customFormatter.Format(format, value, formatProvider));
		}
		else if (value is IFormattable formattable)
		{
			output.Write(formattable.ToString(format, formatProvider ?? CultureInfo.InvariantCulture));
		}
		else
		{
			output.Write(value.ToString());
		}
	}

	public override bool Equals(object? obj)
	{
		if (obj is ScalarValue scalarValue)
		{
			return object.Equals(Value, scalarValue.Value);
		}
		return false;
	}

	public override int GetHashCode()
	{
		if (Value == null)
		{
			return 0;
		}
		return Value.GetHashCode();
	}
}
