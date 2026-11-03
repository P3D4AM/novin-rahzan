using System;

namespace Stimulsoft.Report;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
public sealed class StiSuperToolTipTextAttribute : Attribute
{
	public string Text { get; set; }

	public StiSuperToolTipTextAttribute(string text)
	{
		Text = text;
	}
}
