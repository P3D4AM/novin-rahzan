using System;
using System.Collections.Generic;
using System.IO;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog.Sinks.SystemConsole.Themes;

namespace Serilog.Sinks.SystemConsole.Formatting;

internal class ThemedDisplayValueFormatter : ThemedValueFormatter
{
	private readonly IFormatProvider? _formatProvider;

	public ThemedDisplayValueFormatter(ConsoleTheme theme, IFormatProvider? formatProvider)
		: base(theme)
	{
		_formatProvider = formatProvider;
	}

	public override ThemedValueFormatter SwitchTheme(ConsoleTheme theme)
	{
		return new ThemedDisplayValueFormatter(theme, _formatProvider);
	}

	protected override int VisitScalarValue(ThemedValueFormatterState state, ScalarValue scalar)
	{
		if (scalar == null)
		{
			throw new ArgumentNullException("scalar");
		}
		return FormatLiteralValue(scalar, state.Output, state.Format);
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
			Visit(state, sequence.Elements[i]);
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
		if (structure.TypeTag != null)
		{
			using (ApplyStyle(state.Output, ConsoleThemeStyle.Name, ref invisibleCharacterCount))
			{
				state.Output.Write(structure.TypeTag);
			}
			state.Output.Write(' ');
		}
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
				state.Output.Write(logEventProperty.Name);
			}
			using (ApplyStyle(state.Output, ConsoleThemeStyle.TertiaryText, ref invisibleCharacterCount))
			{
				state.Output.Write('=');
			}
			invisibleCharacterCount += Visit(state.Nest(), logEventProperty.Value);
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
			using (ApplyStyle(state.Output, ConsoleThemeStyle.TertiaryText, ref invisibleCharacterCount))
			{
				state.Output.Write('[');
			}
			using (ApplyStyle(state.Output, ConsoleThemeStyle.String, ref invisibleCharacterCount))
			{
				invisibleCharacterCount += Visit(state.Nest(), element.Key);
			}
			using (ApplyStyle(state.Output, ConsoleThemeStyle.TertiaryText, ref invisibleCharacterCount))
			{
				state.Output.Write("]=");
			}
			invisibleCharacterCount += Visit(state.Nest(), element.Value);
		}
		using (ApplyStyle(state.Output, ConsoleThemeStyle.TertiaryText, ref invisibleCharacterCount))
		{
			state.Output.Write('}');
			return invisibleCharacterCount;
		}
	}

	public int FormatLiteralValue(ScalarValue scalar, TextWriter output, string? format)
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
		if (value is string text)
		{
			using (ApplyStyle(output, ConsoleThemeStyle.String, ref invisibleCharacterCount))
			{
				if (format != "l")
				{
					JsonValueFormatter.WriteQuotedJsonString(text, output);
				}
				else
				{
					output.Write(text);
				}
			}
			return invisibleCharacterCount;
		}
		if (value is ValueType)
		{
			if (value is int || value is uint || value is long || value is ulong || value is decimal || value is byte || value is sbyte || value is short || value is ushort || value is float || value is double)
			{
				using (ApplyStyle(output, ConsoleThemeStyle.Number, ref invisibleCharacterCount))
				{
					scalar.Render(output, format, _formatProvider);
					return invisibleCharacterCount;
				}
			}
			if (value is bool value2)
			{
				using (ApplyStyle(output, ConsoleThemeStyle.Boolean, ref invisibleCharacterCount))
				{
					output.Write(value2);
					return invisibleCharacterCount;
				}
			}
			if (value is char value3)
			{
				using (ApplyStyle(output, ConsoleThemeStyle.Scalar, ref invisibleCharacterCount))
				{
					output.Write('\'');
					output.Write(value3);
					output.Write('\'');
					return invisibleCharacterCount;
				}
			}
		}
		using (ApplyStyle(output, ConsoleThemeStyle.Scalar, ref invisibleCharacterCount))
		{
			scalar.Render(output, format, _formatProvider);
			return invisibleCharacterCount;
		}
	}
}
