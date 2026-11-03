using System.ComponentModel;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Components;

[TypeConverter(typeof(StiListOfArgumentsExpressionConverter))]
public class StiListOfArgumentsExpression : StiExpression
{
	[Browsable(false)]
	public override bool ApplyFormat => false;

	public override bool FullConvert => true;

	public override StiEvent GetDefaultEvent()
	{
		return new StiGetListOfArgumentsEvent();
	}

	public StiListOfArgumentsExpression()
		: this(string.Empty)
	{
	}

	public StiListOfArgumentsExpression(string value)
		: base(value)
	{
	}
}
