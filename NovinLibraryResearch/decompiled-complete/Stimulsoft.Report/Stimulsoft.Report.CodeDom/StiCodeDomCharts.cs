using System.CodeDom;
using System.Collections;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.CodeDom;

internal class StiCodeDomCharts
{
	internal static void Serialize(StiCodeDomSerializator serializator, StiReport report)
	{
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Expected O, but got Unknown
		if (report.CalculationMode == StiCalculationMode.Interpretation)
		{
			return;
		}
		StiComponentsCollection components = report.GetComponents();
		foreach (StiComponent item in components)
		{
			if (!(item is StiChart stiChart))
			{
				continue;
			}
			int num = 0;
			foreach (IStiSeries item2 in stiChart.Series)
			{
				int num2 = 0;
				foreach (IStiChartFilter filter in item2.Filters)
				{
					if (filter.Item == StiFilterItem.Expression)
					{
						string methodName = $"{stiChart.Name}Filters_{num}_{num2}";
						AddFilterMethod(serializator, report, methodName, filter.Value);
						if (((CollectionBase)(object)serializator.Members).Count > 0)
						{
							CodeTypeMember obj = serializator.Members[((CollectionBase)(object)serializator.Members).Count - 1];
							CodeMemberMethod val = (CodeMemberMethod)(object)((obj is CodeMemberMethod) ? obj : null);
							if (val != null)
							{
								val.Statements.Insert(0, (CodeStatement)new CodeCommentStatement(StiCodeDomSerializator.GetCheckerInfoString(item.Name, "Filters")));
							}
						}
					}
					num2++;
				}
				num++;
			}
		}
	}

	internal static void AddFilterMethod(StiCodeDomSerializator serializator, StiReport report, string methodName, string expression)
	{
		expression = StiCodeDomFunctions.ParseFunctions(serializator, expression);
		serializator.GenReturnMethodForExpresion(methodName, expression, typeof(bool));
	}
}
