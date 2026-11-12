namespace Serilog.Sinks.Http.TextFormatters;

public class CompactRenderedTextFormatter : CompactTextFormatter
{
	public CompactRenderedTextFormatter()
	{
		base.IsRenderingMessage = true;
	}
}
