using Stimulsoft.Blockly.Blocks;
using Stimulsoft.Blockly.Model;
using Stimulsoft.Report;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Blockly.StiBlocks.Data;

public class StiSetDataSourceSqlCommand : IronBlock
{
	public override object Evaluate(Context context)
	{
		if (base.Values.Evaluate("DATA", context) is StiSqlSource stiSqlSource)
		{
			string text = base.Values.Evaluate("VALUE", context).ToString();
			StiReport report = context.Report;
			if (report != null && report.CalculationMode == StiCalculationMode.Interpretation)
			{
				context.Report.Variables["**StoredDataSourceSqlCommandForInterpretationMode**" + stiSqlSource.Name] = text;
			}
			else
			{
				stiSqlSource.SqlCommand = text;
			}
		}
		return base.Evaluate(context);
	}
}
