using System.CodeDom;

namespace Stimulsoft.Report.CodeDom;

public class StiCodeRegionEnd : CodeCommentStatement
{
	public string Text { get; set; }

	public StiCodeRegionEnd()
		: this(string.Empty)
	{
	}

	public StiCodeRegionEnd(string text)
	{
		Text = text;
	}
}
