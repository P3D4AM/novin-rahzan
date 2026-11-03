using System.Collections;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Export;

public class StiHtmlHyperlink
{
	public string Text;

	public string ToolTip;

	public string NavigateUrl;

	public Hashtable Attributes;

	public Hashtable Style;

	public string ImageUrl;

	public string CssClass;

	public StiHtmlUnit Width;

	public StiHtmlUnit Height;

	public string OpenLinksTarget;

	public string Id;

	public StiHtmlUnit Margin;

	public StiImage ImageComp;

	public float Zoom;

	public StiHtmlHyperlink()
	{
		Attributes = new Hashtable();
		Style = new Hashtable();
	}
}
