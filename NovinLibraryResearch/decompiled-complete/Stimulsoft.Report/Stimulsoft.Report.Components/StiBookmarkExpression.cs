using System.ComponentModel;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Components;

[TypeConverter(typeof(StiBookmarkExpressionConverter))]
public class StiBookmarkExpression : StiUnifiedExpression
{
	[Browsable(false)]
	public override bool ApplyFormat => false;

	public override StiEvent GetDefaultEvent()
	{
		return new StiGetBookmarkEvent();
	}

	public StiBookmarkExpression()
		: this(string.Empty)
	{
	}

	public StiBookmarkExpression(string val)
		: base(val)
	{
	}

	public StiBookmarkExpression(StiComponent parent, string propertyName)
		: base(parent, propertyName)
	{
	}
}
