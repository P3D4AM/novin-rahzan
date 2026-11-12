using System.IO;
using System.Web.UI;

namespace Stimulsoft.Report.Web;

internal class StiJavaScript : LiteralControl
{
	public string ScriptUrl { get; set; } = string.Empty;


	protected override void Render(HtmlTextWriter output)
	{
		if (!string.IsNullOrEmpty(ScriptUrl))
		{
			((TextWriter)(object)output).Write($"<script type=\"text/javascript\" src=\"{StiUrlHelper.EscapeUrlQuotes(ScriptUrl)}\">");
		}
		else
		{
			((TextWriter)(object)output).Write("<script type=\"text/javascript\">");
		}
		((LiteralControl)this).Render(output);
		((TextWriter)(object)output).Write("</script>");
	}

	public StiJavaScript()
	{
		((LiteralControl)this).Text = string.Empty;
	}
}
