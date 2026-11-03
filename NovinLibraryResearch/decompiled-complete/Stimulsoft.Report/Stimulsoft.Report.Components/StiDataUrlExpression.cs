using System.ComponentModel;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Components;

[TypeConverter(typeof(StiDataUrlExpressionConverter))]
public class StiDataUrlExpression : StiUnifiedExpression
{
	[Browsable(false)]
	public override bool ApplyFormat => false;

	public override bool FullConvert => true;

	public override StiEvent GetDefaultEvent()
	{
		return new StiGetDataUrlEvent();
	}

	public StiDataUrlExpression()
		: this(string.Empty)
	{
	}

	public StiDataUrlExpression(string value)
		: base(value)
	{
	}

	public StiDataUrlExpression(StiComponent parent, string propertyName)
		: base(parent, propertyName)
	{
	}
}
