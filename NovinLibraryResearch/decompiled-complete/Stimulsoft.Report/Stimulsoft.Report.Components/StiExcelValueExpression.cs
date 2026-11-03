using System.ComponentModel;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Components;

[TypeConverter(typeof(StiExcelValueExpressionConverter))]
public class StiExcelValueExpression : StiUnifiedExpression
{
	[Browsable(false)]
	public override bool ApplyFormat => false;

	public override bool FullConvert => true;

	public override StiEvent GetDefaultEvent()
	{
		return new StiGetExcelValueEvent();
	}

	public StiExcelValueExpression()
		: this(string.Empty)
	{
	}

	public StiExcelValueExpression(string value)
		: base(value)
	{
	}

	public StiExcelValueExpression(StiComponent parent, string propertyName)
		: base(parent, propertyName)
	{
	}
}
