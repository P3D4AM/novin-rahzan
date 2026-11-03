using System.ComponentModel;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Components;

[TypeConverter(typeof(StiListOfValuesCloseExpressionConverter))]
public class StiListOfValuesCloseExpression : StiExpression
{
	[Browsable(false)]
	public override bool ApplyFormat => false;

	public override bool FullConvert => true;

	public override StiEvent GetDefaultEvent()
	{
		return new StiGetListOfValuesCloseEvent();
	}

	public StiListOfValuesCloseExpression()
		: this(string.Empty)
	{
	}

	public StiListOfValuesCloseExpression(string value)
		: base(value)
	{
	}
}
