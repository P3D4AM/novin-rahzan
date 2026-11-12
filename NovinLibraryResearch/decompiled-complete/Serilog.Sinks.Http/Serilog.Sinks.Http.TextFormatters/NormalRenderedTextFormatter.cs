namespace Serilog.Sinks.Http.TextFormatters;

public class NormalRenderedTextFormatter : NormalTextFormatter
{
	public NormalRenderedTextFormatter()
	{
		base.IsRenderingMessage = true;
	}
}
