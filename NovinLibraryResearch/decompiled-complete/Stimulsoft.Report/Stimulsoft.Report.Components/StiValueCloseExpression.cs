using System.ComponentModel;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Components;

[TypeConverter(typeof(StiValueCloseExpressionConverter))]
public class StiValueCloseExpression : StiExpression
{
	[Browsable(false)]
	public override bool ApplyFormat => false;

	public override bool FullConvert => true;

	public override StiEvent GetDefaultEvent()
	{
		return new StiGetValueCloseEvent();
	}

	public StiValueCloseExpression()
		: this(string.Empty)
	{
	}

	public StiValueCloseExpression(string value)
		: base(value)
	{
	}
}
