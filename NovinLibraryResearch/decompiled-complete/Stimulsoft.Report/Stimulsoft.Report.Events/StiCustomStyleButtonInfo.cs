using System;
using System.Drawing;

namespace Stimulsoft.Report.Events;

public sealed class StiCustomStyleButtonInfo
{
	public object IconWpf16 { get; private set; }

	public object IconWpf32 { get; private set; }

	public Image Icon16 { get; private set; }

	public Image Icon32 { get; private set; }

	public string Text { get; private set; }

	public Type StyleType { get; private set; }

	public StiCustomStyleButtonInfo(Image icon16, Image icon32, string text, Type styleType)
	{
		Icon16 = icon16;
		Icon32 = icon32;
		Text = text;
		StyleType = styleType;
	}

	public StiCustomStyleButtonInfo(object iconWpf16, object iconWpf32, string text, Type styleType)
	{
		IconWpf16 = iconWpf16;
		IconWpf32 = iconWpf32;
		Text = text;
		StyleType = styleType;
	}
}
