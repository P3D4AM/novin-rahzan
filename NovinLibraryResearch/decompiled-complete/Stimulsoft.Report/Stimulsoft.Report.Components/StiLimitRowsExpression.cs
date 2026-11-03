using System.ComponentModel;
using Stimulsoft.Report.Components.Design;

namespace Stimulsoft.Report.Components;

[TypeConverter(typeof(StiLimitRowsExpressionConverter))]
public class StiLimitRowsExpression : StiUnifiedExpression
{
	[Browsable(false)]
	public override bool ApplyFormat => false;

	public StiLimitRowsExpression()
		: this(string.Empty)
	{
	}

	public StiLimitRowsExpression(string val)
		: base(val)
	{
	}

	public StiLimitRowsExpression(StiComponent parent, string propertyName)
		: base(parent, propertyName)
	{
	}
}
