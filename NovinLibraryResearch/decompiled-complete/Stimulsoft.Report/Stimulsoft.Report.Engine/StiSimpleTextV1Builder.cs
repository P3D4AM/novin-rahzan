using System.Collections;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Engine;

public class StiSimpleTextV1Builder : StiComponentV1Builder
{
	public override bool InternalRender(StiComponent masterComp, ref StiComponent renderedComponent, StiContainer outContainer)
	{
		StiSimpleText stiSimpleText = masterComp as StiSimpleText;
		StiSimpleText stiSimpleText2 = (StiSimpleText)stiSimpleText.Clone(cloneProperties: false);
		StiGetValueEventArgs stiGetValueEventArgs = new StiGetValueEventArgs();
		if (string.IsNullOrEmpty(stiSimpleText.TextValue))
		{
			stiSimpleText.InvokeGetValue(stiSimpleText2, stiGetValueEventArgs);
			stiSimpleText2.InvokeEvents();
			stiSimpleText.CheckDuplicates(stiSimpleText2, stiGetValueEventArgs);
			StiValueEventArgs stiValueEventArgs = new StiValueEventArgs(stiGetValueEventArgs.Value);
			stiSimpleText2.InvokeTextProcess(stiSimpleText2, stiValueEventArgs);
			stiSimpleText2.SetTextInternal(stiSimpleText.ProcessText(stiSimpleText.GetTextWithoutZero(stiValueEventArgs.Value as string)));
			stiSimpleText.InvokeRenderTo(stiSimpleText2);
		}
		else
		{
			stiSimpleText2.InvokeEvents();
			StiValueEventArgs stiValueEventArgs2 = new StiValueEventArgs(stiSimpleText.TextValue);
			stiSimpleText2.InvokeTextProcess(stiSimpleText2, stiValueEventArgs2);
			stiSimpleText2.SetTextInternal(stiSimpleText.GetTextWithoutZero(stiValueEventArgs2.Value as string));
		}
		outContainer.Components.Add(stiSimpleText2);
		if (stiGetValueEventArgs.StoreToPrinted)
		{
			ArrayList arrayList = stiSimpleText.Report.Totals[stiSimpleText.Name] as ArrayList;
			if (arrayList == null)
			{
				arrayList = new ArrayList();
				stiSimpleText.Report.Totals[stiSimpleText.Name] = arrayList;
			}
			StiRuntimeVariables stiRuntimeVariables = new StiRuntimeVariables(stiSimpleText.Report);
			stiRuntimeVariables.PageIndex = stiSimpleText.Report.RenderedPages.Count;
			stiRuntimeVariables.TextBox = stiSimpleText2;
			arrayList.Add(stiRuntimeVariables);
			stiSimpleText.Report.Totals["#%#" + stiSimpleText.Name] = stiSimpleText.Text.Value;
		}
		if (stiSimpleText is StiText)
		{
			if (string.IsNullOrEmpty(((StiText)stiSimpleText).ExcelDataValue) || ((StiText)stiSimpleText).ExcelDataValue == "-")
			{
				StiGetExcelValueEventArgs stiGetExcelValueEventArgs = new StiGetExcelValueEventArgs();
				((StiText)stiSimpleText).InvokeGetExcelValue(stiSimpleText2, stiGetExcelValueEventArgs);
				if (stiGetExcelValueEventArgs.StoreToPrinted)
				{
					ArrayList arrayList2 = stiSimpleText.Report.Totals[stiSimpleText.Name + "Excel"] as ArrayList;
					if (arrayList2 == null)
					{
						arrayList2 = new ArrayList();
						stiSimpleText.Report.Totals[stiSimpleText.Name + "Excel"] = arrayList2;
					}
					StiRuntimeVariables stiRuntimeVariables2 = new StiRuntimeVariables(stiSimpleText.Report);
					stiRuntimeVariables2.PageIndex = stiSimpleText.Report.RenderedPages.Count;
					stiRuntimeVariables2.TextBox = stiSimpleText2;
					arrayList2.Add(stiRuntimeVariables2);
				}
			}
			else
			{
				((StiText)stiSimpleText).ExcelDataValue = ((StiText)stiSimpleText).ExcelDataValue;
			}
		}
		renderedComponent = stiSimpleText2;
		return true;
	}
}
