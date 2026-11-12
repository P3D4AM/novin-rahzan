using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Serilog.Data;
using Serilog.Events;

namespace Serilog.Formatting.Json;

public class JsonValueFormatter : LogEventPropertyValueVisitor<TextWriter, bool>
{
	private readonly string? _typeTagName;

	private const string DefaultTypeTagName = "_typeTag";

	public JsonValueFormatter(string? typeTagName = "_typeTag")
	{
		_typeTagName = typeTagName;
	}

	public void Format(LogEventPropertyValue value, TextWriter output)
	{
		Visit(output, value);
	}

	protected override bool VisitScalarValue(TextWriter state, ScalarValue scalar)
	{
		Guard.AgainstNull(scalar, "scalar");
		FormatLiteralValue(scalar.Value, state);
		return false;
	}

	protected override bool VisitSequenceValue(TextWriter state, SequenceValue sequence)
	{
		Guard.AgainstNull(sequence, "sequence");
		state.Write('[');
		char? c = null;
		for (int i = 0; i < sequence.Elements.Count; i++)
		{
			if (c.HasValue)
			{
				state.Write(c.Value);
			}
			c = ',';
			Visit(state, sequence.Elements[i]);
		}
		state.Write(']');
		return false;
	}

	protected override bool VisitStructureValue(TextWriter state, StructureValue structure)
	{
		state.Write('{');
		char? c = null;
		for (int i = 0; i < structure.Properties.Count; i++)
		{
			if (c.HasValue)
			{
				state.Write(c.Value);
			}
			c = ',';
			LogEventProperty logEventProperty = structure.Properties[i];
			WriteQuotedJsonString(logEventProperty.Name, state);
			state.Write(':');
			Visit(state, logEventProperty.Value);
		}
		if (_typeTagName != null && structure.TypeTag != null)
		{
			state.Write(c);
			WriteQuotedJsonString(_typeTagName, state);
			state.Write(':');
			WriteQuotedJsonString(structure.TypeTag, state);
		}
		state.Write('}');
		return false;
	}

	protected override bool VisitDictionaryValue(TextWriter state, DictionaryValue dictionary)
	{
		state.Write('{');
		char? c = null;
		foreach (KeyValuePair<ScalarValue, LogEventPropertyValue> element in dictionary.Elements)
		{
			if (c.HasValue)
			{
				state.Write(c.Value);
			}
			c = ',';
			WriteQuotedJsonString((element.Key.Value ?? "null").ToString(), state);
			state.Write(':');
			Visit(state, element.Value);
		}
		state.Write('}');
		return false;
	}

	protected virtual void FormatLiteralValue(object? value, TextWriter output)
	{
		if (value == null)
		{
			FormatNullValue(output);
			return;
		}
		if (value is string str)
		{
			FormatStringValue(str, output);
			return;
		}
		if (value is ValueType)
		{
			if (value is int value2)
			{
				FormatExactNumericValue(value2, output);
				return;
			}
			if (value is uint value3)
			{
				FormatExactNumericValue(value3, output);
				return;
			}
			if (value is long value4)
			{
				FormatExactNumericValue(value4, output);
				return;
			}
			if (value is ulong value5)
			{
				FormatExactNumericValue(value5, output);
				return;
			}
			if (value is decimal value6)
			{
				FormatExactNumericValue(value6, output);
				return;
			}
			if (value is byte value7)
			{
				FormatExactNumericValue(value7, output);
				return;
			}
			if (value is sbyte value8)
			{
				FormatExactNumericValue(value8, output);
				return;
			}
			if (value is short value9)
			{
				FormatExactNumericValue(value9, output);
				return;
			}
			if (value is ushort value10)
			{
				FormatExactNumericValue(value10, output);
				return;
			}
			if (value is double value11)
			{
				FormatDoubleValue(value11, output);
				return;
			}
			if (value is float value12)
			{
				FormatFloatValue(value12, output);
				return;
			}
			if (value is bool value13)
			{
				FormatBooleanValue(value13, output);
				return;
			}
			if (value is char)
			{
				FormatStringValue(value.ToString(), output);
				return;
			}
			if (value is DateTime value14)
			{
				FormatDateTimeValue(value14, output);
				return;
			}
			if (value is DateTimeOffset value15)
			{
				FormatDateTimeOffsetValue(value15, output);
				return;
			}
			if (value is TimeSpan value16)
			{
				FormatTimeSpanValue(value16, output);
				return;
			}
		}
		FormatLiteralObjectValue(value, output);
	}

	private static void FormatBooleanValue(bool value, TextWriter output)
	{
		output.Write(value ? "true" : "false");
	}

	private static void FormatFloatValue(float value, TextWriter output)
	{
		if (float.IsNaN(value) || float.IsInfinity(value))
		{
			FormatStringValue(value.ToString(CultureInfo.InvariantCulture), output);
		}
		else
		{
			output.Write(value.ToString("R", CultureInfo.InvariantCulture));
		}
	}

	private static void FormatDoubleValue(double value, TextWriter output)
	{
		if (double.IsNaN(value) || double.IsInfinity(value))
		{
			FormatStringValue(value.ToString(CultureInfo.InvariantCulture), output);
		}
		else
		{
			output.Write(value.ToString("R", CultureInfo.InvariantCulture));
		}
	}

	private static void FormatExactNumericValue(int value, TextWriter output)
	{
		output.Write(value.ToString(null, CultureInfo.InvariantCulture));
	}

	private static void FormatExactNumericValue(uint value, TextWriter output)
	{
		output.Write(value.ToString(null, CultureInfo.InvariantCulture));
	}

	private static void FormatExactNumericValue(long value, TextWriter output)
	{
		output.Write(value.ToString(null, CultureInfo.InvariantCulture));
	}

	private static void FormatExactNumericValue(ulong value, TextWriter output)
	{
		output.Write(value.ToString(null, CultureInfo.InvariantCulture));
	}

	private static void FormatExactNumericValue(decimal value, TextWriter output)
	{
		output.Write(value.ToString(null, CultureInfo.InvariantCulture));
	}

	private static void FormatExactNumericValue(byte value, TextWriter output)
	{
		output.Write(value.ToString(null, CultureInfo.InvariantCulture));
	}

	private static void FormatExactNumericValue(sbyte value, TextWriter output)
	{
		output.Write(value.ToString(null, CultureInfo.InvariantCulture));
	}

	private static void FormatExactNumericValue(short value, TextWriter output)
	{
		output.Write(value.ToString(null, CultureInfo.InvariantCulture));
	}

	private static void FormatExactNumericValue(ushort value, TextWriter output)
	{
		output.Write(value.ToString(null, CultureInfo.InvariantCulture));
	}

	private static void FormatDateTimeValue(DateTime value, TextWriter output)
	{
		output.Write('"');
		output.Write(value.ToString("O", CultureInfo.InvariantCulture));
		output.Write('"');
	}

	private static void FormatDateTimeOffsetValue(DateTimeOffset value, TextWriter output)
	{
		output.Write('"');
		output.Write(value.ToString("O", CultureInfo.InvariantCulture));
		output.Write('"');
	}

	private static void FormatTimeSpanValue(TimeSpan value, TextWriter output)
	{
		output.Write('"');
		output.Write(value.ToString());
		output.Write('"');
	}

	private static void FormatLiteralObjectValue(object value, TextWriter output)
	{
		Guard.AgainstNull(value, "value");
		FormatStringValue(value.ToString() ?? "", output);
	}

	private static void FormatStringValue(string str, TextWriter output)
	{
		WriteQuotedJsonString(str, output);
	}

	private static void FormatNullValue(TextWriter output)
	{
		output.Write("null");
	}

	public static void WriteQuotedJsonString(string str, TextWriter output)
	{
		output.Write('"');
		int num = 0;
		bool flag = false;
		for (int i = 0; i < str.Length; i++)
		{
			char c = str[i];
			if ((c < ' ' || c == '"' || c == '\\') ? true : false)
			{
				flag = true;
				output.Write(str.Substring(num, i - num));
				num = i + 1;
				switch (c)
				{
				case '"':
					output.Write("\\\"");
					continue;
				case '\\':
					output.Write("\\\\");
					continue;
				case '\n':
					output.Write("\\n");
					continue;
				case '\r':
					output.Write("\\r");
					continue;
				case '\f':
					output.Write("\\f");
					continue;
				case '\t':
					output.Write("\\t");
					continue;
				}
				output.Write("\\u");
				int num2 = c;
				output.Write(num2.ToString("X4"));
			}
		}
		if (flag)
		{
			if (num != str.Length)
			{
				output.Write(str.Substring(num));
			}
		}
		else
		{
			output.Write(str);
		}
		output.Write('"');
	}
}
