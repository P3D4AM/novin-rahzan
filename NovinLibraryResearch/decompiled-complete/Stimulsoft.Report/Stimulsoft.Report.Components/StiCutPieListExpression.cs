using System.ComponentModel;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Components;

[TypeConverter(typeof(StiCutPieListExpressionConverter))]
public class StiCutPieListExpression : StiExpression
{
	[Browsable(false)]
	public override bool ApplyFormat => false;

	public override bool FullConvert => true;

	public override StiEvent GetDefaultEvent()
	{
		return new StiGetCutPieListEvent();
	}

	public StiCutPieListExpression()
		: this(string.Empty)
	{
	}

	public StiCutPieListExpression(string value)
		: base(value)
	{
	}
}
