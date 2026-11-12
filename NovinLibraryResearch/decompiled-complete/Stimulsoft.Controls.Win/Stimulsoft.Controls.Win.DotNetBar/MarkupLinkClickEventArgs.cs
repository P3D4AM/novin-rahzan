using System;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class MarkupLinkClickEventArgs : EventArgs
{
	public readonly string HRef = "";

	public readonly string Name = "";

	public MarkupLinkClickEventArgs(string name, string href)
	{
		HRef = href;
		Name = name;
	}
}
