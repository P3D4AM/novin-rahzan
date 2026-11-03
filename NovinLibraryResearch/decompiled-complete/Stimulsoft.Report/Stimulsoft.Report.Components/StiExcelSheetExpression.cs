using System.ComponentModel;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Components;

[TypeConverter(typeof(StiExcelSheetExpressionConverter))]
public class StiExcelSheetExpression : StiUnifiedExpression
{
	[Browsable(false)]
	public override bool ApplyFormat => false;

	public override bool FullConvert => true;

	public override StiEvent GetDefaultEvent()
	{
		return new StiGetExcelSheetEvent();
	}

	public StiExcelSheetExpression()
		: this(string.Empty)
	{
	}

	public StiExcelSheetExpression(string value)
		: base(value)
	{
	}

	public StiExcelSheetExpression(StiComponent parent, string propertyName)
		: base(parent, propertyName)
	{
	}
}
