using System.ComponentModel;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Components;

[TypeConverter(typeof(StiImageDataExpressionConverter))]
public class StiImageDataExpression : StiUnifiedExpression
{
	[Browsable(false)]
	public override bool ApplyFormat => false;

	public override bool FullConvert => false;

	public override StiEvent GetDefaultEvent()
	{
		return new StiGetImageDataEvent();
	}

	public StiImageDataExpression()
		: this(string.Empty)
	{
	}

	public StiImageDataExpression(string value)
		: base(value)
	{
	}

	public StiImageDataExpression(StiComponent parent, string propertyName)
		: base(parent, propertyName)
	{
	}
}
