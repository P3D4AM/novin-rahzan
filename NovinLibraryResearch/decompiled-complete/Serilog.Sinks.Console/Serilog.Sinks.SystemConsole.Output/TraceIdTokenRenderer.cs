using System.Diagnostics;
using System.IO;
using Serilog.Events;
using Serilog.Parsing;
using Serilog.Sinks.SystemConsole.Rendering;
using Serilog.Sinks.SystemConsole.Themes;

namespace Serilog.Sinks.SystemConsole.Output;

internal class TraceIdTokenRenderer : OutputTemplateTokenRenderer
{
	private readonly ConsoleTheme _theme;

	private readonly Alignment? _alignment;

	public TraceIdTokenRenderer(ConsoleTheme theme, PropertyToken traceIdToken)
	{
		_theme = theme;
		_alignment = traceIdToken.Alignment;
	}

	public override void Render(LogEvent logEvent, TextWriter output)
	{
		ActivityTraceId? traceId = logEvent.TraceId;
		if (!traceId.HasValue)
		{
			return;
		}
		ActivityTraceId valueOrDefault = traceId.GetValueOrDefault();
		int invisibleCharacterCount = 0;
		using (_theme.Apply(output, ConsoleThemeStyle.Text, ref invisibleCharacterCount))
		{
			Alignment? alignment = _alignment;
			if (alignment.HasValue)
			{
				Alignment valueOrDefault2 = alignment.GetValueOrDefault();
				Padding.Apply(output, valueOrDefault.ToString(), valueOrDefault2);
			}
			else
			{
				output.Write(valueOrDefault);
			}
		}
	}
}
