using System.CodeDom;

namespace Stimulsoft.Report.CodeDom;

public class StiCodeRegionStart : CodeCommentStatement
{
	public string Text { get; set; }

	public StiCodeRegionStart(string text)
	{
		Text = text;
	}
}
