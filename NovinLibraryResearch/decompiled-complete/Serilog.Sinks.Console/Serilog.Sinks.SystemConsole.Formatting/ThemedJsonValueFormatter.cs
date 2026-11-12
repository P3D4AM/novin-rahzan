using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog.Sinks.SystemConsole.Themes;

namespace Serilog.Sinks.SystemConsole.Formatting;

internal class ThemedJsonValueFormatter : ThemedValueFormatter
{
	private readonly ThemedDisplayValueFormatter _displayFormatter;

	private readonly IFormatProvider? _formatProvider;

	public ThemedJsonValueFormatter(ConsoleTheme theme, IFormatProvider? formatProvider)
		: base(theme)
	{
		_displayFormatter = new ThemedDisplayValueFormatter(theme, formatProvider);
		_formatProvider = formatProvider;
	}

	public override ThemedValueFormatter SwitchTheme(ConsoleTheme theme)
	{
		return new ThemedJsonValueFormatter(theme, _formatProvider);
	}

	protected override int VisitScalarValue(ThemedValueFormatterState state, ScalarValue scalar)
	{
		if (scalar == null)
		{
			throw new ArgumentNullException("scalar");
		}
		if (state.IsTopLevel)
		{
			return _displayFormatter.FormatLiteralValue(scalar, state.Output, state.Format);
		}
		return FormatLiteralValue(scalar, state.Output);
	}

	protected override int VisitSequenceValue(ThemedValueFormatterState state, SequenceValue sequence)
	{
		if (sequence == null)
		{
			throw new ArgumentNullException("sequence");
		}
		int invisibleCharacterCount = 0;
		using (ApplyStyle(state.Output, ConsoleThemeStyle.TertiaryText, ref invisibleCharacterCount))
		{
			state.Output.Write('[');
		}
		string text = string.Empty;
		for (int i = 0; i < sequence.Elements.Count; i++)
		{
			if (text.Length != 0)
			{
				using (ApplyStyle(state.Output, ConsoleThemeStyle.TertiaryText, ref invisibleCharacterCount))
				{
					state.Output.Write(text);
				}
			}
			text = ", ";
			Visit(state.Nest(), sequence.Elements[i]);
		}
		using (ApplyStyle(state.Output, ConsoleThemeStyle.TertiaryText, ref invisibleCharacterCount))
		{
			state.Output.Write(']');
			return invisibleCharacterCount;
		}
	}

	protected override int VisitStructureValue(ThemedValueFormatterState state, StructureValue structure)
	{
		int invisibleCharacterCount = 0;
		using (ApplyStyle(state.Output, ConsoleThemeStyle.TertiaryText, ref invisibleCharacterCount))
		{
			state.Output.Write('{');
		}
		string text = string.Empty;
		for (int i = 0; i < structure.Properties.Count; i++)
		{
			if (text.Length != 0)
			{
				using (ApplyStyle(state.Output, ConsoleThemeStyle.TertiaryText, ref invisibleCharacterCount))
				{
					state.Output.Write(text);
				}
			}
			text = ", ";
			LogEventProperty logEventProperty = structure.Properties[i];
			using (ApplyStyle(state.Output, ConsoleThemeStyle.Name, ref invisibleCharacterCount))
			{
				JsonValueFormatter.WriteQuotedJsonString(logEventProperty.Name, state.Output);
			}
			using (ApplyStyle(state.Output, ConsoleThemeStyle.TertiaryText, ref invisibleCharacterCount))
			{
				state.Output.Write(": ");
			}
			invisibleCharacterCount += Visit(state.Nest(), logEventProperty.Value);
		}
		if (structure.TypeTag != null)
		{
			using (ApplyStyle(state.Output, ConsoleThemeStyle.TertiaryText, ref invisibleCharacterCount))
			{
				state.Output.Write(text);
			}
			using (ApplyStyle(state.Output, ConsoleThemeStyle.Name, ref invisibleCharacterCount))
			{
				JsonValueFormatter.WriteQuotedJsonString("$type", state.Output);
			}
			using (ApplyStyle(state.Output, ConsoleThemeStyle.TertiaryText, ref invisibleCharacterCount))
			{
				state.Output.Write(": ");
			}
			using (ApplyStyle(state.Output, ConsoleThemeStyle.String, ref invisibleCharacterCount))
			{
				JsonValueFormatter.WriteQuotedJsonString(structure.TypeTag, state.Output);
			}
		}
		using (ApplyStyle(state.Output, ConsoleThemeStyle.TertiaryText, ref invisibleCharacterCount))
		{
			state.Output.Write('}');
			return invisibleCharacterCount;
		}
	}

	protected override int VisitDictionaryValue(ThemedValueFormatterState state, DictionaryValue dictionary)
	{
		int invisibleCharacterCount = 0;
		using (ApplyStyle(state.Output, ConsoleThemeStyle.TertiaryText, ref invisibleCharacterCount))
		{
			state.Output.Write('{');
		}
		string text = string.Empty;
		foreach (KeyValuePair<ScalarValue, LogEventPropertyValue> element in dictionary.Elements)
		{
			if (text.Length != 0)
			{
				using (ApplyStyle(state.Output, ConsoleThemeStyle.TertiaryText, ref invisibleCharacterCount))
				{
					state.Output.Write(text);
				}
			}
			text = ", ";
			ConsoleThemeStyle style = ((element.Key.Value == null) ? ConsoleThemeStyle.Null : ((element.Key.Value is string) ? ConsoleThemeStyle.String : ConsoleThemeStyle.Scalar));
			using (ApplyStyle(state.Output, style, ref invisibleCharacterCount))
			{
				JsonValueFormatter.WriteQuotedJsonString((element.Key.Value ?? "null").ToString() ?? "", state.Output);
			}
			using (ApplyStyle(state.Output, ConsoleThemeStyle.TertiaryText, ref invisibleCharacterCount))
			{
				state.Output.Write(": ");
			}
			invisibleCharacterCount += Visit(state.Nest(), element.Value);
		}
		using (ApplyStyle(state.Output, ConsoleThemeStyle.TertiaryText, ref invisibleCharacterCount))
		{
			state.Output.Write('}');
			return invisibleCharacterCount;
		}
	}

	private int FormatLiteralValue(ScalarValue scalar, TextWriter output)
	{
		object value = scalar.Value;
		int invisibleCharacterCount = 0;
		if (value == null)
		{
			using (ApplyStyle(output, ConsoleThemeStyle.Null, ref invisibleCharacterCount))
			{
				output.Write("null");
				return invisibleCharacterCount;
			}
		}
		if (value is string str)
		{
			using (ApplyStyle(output, ConsoleThemeStyle.String, ref invisibleCharacterCount))
			{
				JsonValueFormatter.WriteQuotedJsonString(str, output);
				return invisibleCharacterCount;
			}
		}
		if (value is ValueType)
		{
			if (value is int || value is uint || value is long || value is ulong || value is decimal || value is byte || value is sbyte || value is short || value is ushort)
			{
				using (ApplyStyle(output, ConsoleThemeStyle.Number, ref invisibleCharacterCount))
				{
					output.Write(((IFormattable)value).ToString(null, CultureInfo.InvariantCulture));
					return invisibleCharacterCount;
				}
			}
			if (value is double d)
			{
				using (ApplyStyle(output, ConsoleThemeStyle.Number, ref invisibleCharacterCount))
				{
					if (double.IsNaN(d) || double.IsInfinity(d))
					{
						JsonValueFormatter.WriteQuotedJsonString(d.ToString(CultureInfo.InvariantCulture), output);
					}
					else
					{
						output.Write(d.ToString("R", CultureInfo.InvariantCulture));
					}
				}
				return invisibleCharacterCount;
			}
			if (value is float num)
			{
				using (ApplyStyle(output, ConsoleThemeStyle.Number, ref invisibleCharacterCount))
				{
					if (double.IsNaN(num) || double.IsInfinity(num))
					{
						JsonValueFormatter.WriteQuotedJsonString(num.ToString(CultureInfo.InvariantCulture), output);
					}
					else
					{
						output.Write(num.ToString("R", CultureInfo.InvariantCulture));
					}
				}
				return invisibleCharacterCount;
			}
			if (value is bool flag)
			{
				using (ApplyStyle(output, ConsoleThemeStyle.Boolean, ref invisibleCharacterCount))
				{
					output.Write(flag ? "true" : "false");
					return invisibleCharacterCount;
				}
			}
			if (value is char c)
			{
				using (ApplyStyle(output, ConsoleThemeStyle.Scalar, ref invisibleCharacterCount))
				{
					JsonValueFormatter.WriteQuotedJsonString(c.ToString(), output);
					return invisibleCharacterCount;
				}
			}
			if (value is DateTime || value is DateTimeOffset)
			{
				using (ApplyStyle(output, ConsoleThemeStyle.Scalar, ref invisibleCharacterCount))
				{
					output.Write('"');
					output.Write(((IFormattable)value).ToString("O", CultureInfo.InvariantCulture));
					output.Write('"');
					return invisibleCharacterCount;
				}
			}
		}
		using (ApplyStyle(output, ConsoleThemeStyle.Scalar, ref invisibleCharacterCount))
		{
			JsonValueFormatter.WriteQuotedJsonString(value.ToString() ?? "", output);
			return invisibleCharacterCount;
		}
	}
}
