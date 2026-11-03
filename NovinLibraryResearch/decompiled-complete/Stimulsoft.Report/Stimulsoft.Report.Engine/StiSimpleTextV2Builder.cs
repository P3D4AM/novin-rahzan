using System.Collections;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.CrossTab;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Engine;

public class StiSimpleTextV2Builder : StiComponentV2Builder
{
	public override void Prepare(StiComponent masterComp)
	{
		base.Prepare(masterComp);
		if (StiOptions.Engine.AllowResetValuesAtComponent)
		{
			((StiSimpleText)masterComp).TextValue = null;
		}
	}

	public override StiComponent InternalRender(StiComponent masterComp)
	{
		StiSimpleText stiSimpleText = masterComp as StiSimpleText;
		StiSimpleText stiSimpleText2 = (StiSimpleText)stiSimpleText.Clone(cloneProperties: false);
		if (stiSimpleText.Conditions != null && stiSimpleText.Conditions.Count > 0 && stiSimpleText2 is StiText)
		{
			foreach (StiBaseCondition condition in stiSimpleText.Conditions)
			{
				if (condition is IStiIndicatorCondition stiIndicatorCondition)
				{
					StiBrush val = null;
					if (stiIndicatorCondition is StiColorScaleCondition)
					{
						val = ((StiText)stiSimpleText).Brush;
					}
					StiIndicator stiIndicator = stiIndicatorCondition.CreateIndicator(stiSimpleText as StiText);
					if (stiIndicator != null)
					{
						((StiText)stiSimpleText2).Indicator = stiIndicator;
						break;
					}
					if (((StiText)stiSimpleText).Brush != val)
					{
						((StiText)stiSimpleText2).Brush = ((StiText)stiSimpleText).Brush;
						((StiText)stiSimpleText).Brush = val;
					}
				}
			}
		}
		if (masterComp.Report.CalculationMode == StiCalculationMode.Interpretation && stiSimpleText.ProcessAt == StiProcessAt.EndOfPage)
		{
			return stiSimpleText2;
		}
		StiGetValueEventArgs stiGetValueEventArgs = new StiGetValueEventArgs();
		if (string.IsNullOrEmpty(stiSimpleText.TextValue))
		{
			stiSimpleText.InvokeGetValue(stiSimpleText2, stiGetValueEventArgs);
			if (masterComp is StiRichText && !string.IsNullOrEmpty(stiGetValueEventArgs.Value) && StiHyperlinkProcessor.IsServerHyperlink(stiGetValueEventArgs.Value))
			{
				string richText = StiStimulsoftServerResource.GetRichText(masterComp as StiRichText, StiHyperlinkProcessor.GetServerNameFromHyperlink(stiGetValueEventArgs.Value));
				stiGetValueEventArgs.Value = StiRichText.PackRtf(richText);
			}
			stiSimpleText2.InvokeEvents();
			stiSimpleText.CheckDuplicates(stiSimpleText2, stiGetValueEventArgs);
			StiValueEventArgs e = new StiValueEventArgs(stiGetValueEventArgs.Value);
			stiSimpleText2.InvokeTextProcess(stiSimpleText2, e);
			string text = stiGetValueEventArgs.Value;
			if (StiOptions.Engine.AllowAssignEmptyStringInCondition)
			{
				if (stiSimpleText2.TextValue != null)
				{
					text = stiSimpleText2.TextValue;
				}
				if (stiSimpleText2.TextValue == "" && stiGetValueEventArgs.Value != null && stiGetValueEventArgs.Value.StartsWith("#%#") && stiSimpleText2.CanShrink)
				{
					text = "#";
				}
			}
			else if (!string.IsNullOrEmpty(stiSimpleText2.TextValue))
			{
				text = stiSimpleText2.TextValue;
			}
			stiSimpleText2.SetTextInternal(stiSimpleText.ProcessText(stiSimpleText.GetTextWithoutZero(text)));
			stiSimpleText.InvokeRenderTo(stiSimpleText2);
			if (stiSimpleText is StiCrossField)
			{
				(stiSimpleText as StiCrossField).DisabledByCondition = !stiSimpleText2.Enabled;
			}
		}
		else
		{
			stiSimpleText2.InvokeEvents();
			StiValueEventArgs stiValueEventArgs = new StiValueEventArgs(stiSimpleText.TextValue);
			stiSimpleText2.InvokeTextProcess(stiSimpleText2, stiValueEventArgs);
			stiSimpleText2.SetTextInternal(stiSimpleText.GetTextWithoutZero(stiValueEventArgs.Value as string));
		}
		if (stiGetValueEventArgs.StoreToPrinted)
		{
			ArrayList arrayList = stiSimpleText.Report.Totals[stiSimpleText.Name] as ArrayList;
			if (arrayList == null)
			{
				arrayList = new ArrayList();
				stiSimpleText.Report.Totals[stiSimpleText.Name] = arrayList;
			}
			StiRuntimeVariables stiRuntimeVariables = new StiRuntimeVariables(stiSimpleText.Report)
			{
				PageIndex = stiSimpleText.Report.RenderedPages.Count,
				TextBox = stiSimpleText2
			};
			arrayList.Add(stiRuntimeVariables);
			stiSimpleText.Report.Totals["#%#" + stiSimpleText.Name] = stiSimpleText.Text.Value;
			StiContainer parent = masterComp.Parent;
			while (parent != null && !(parent is StiPage) && !(parent is StiOverlayBand))
			{
				parent = parent.Parent;
			}
			if (parent != null && parent is StiOverlayBand)
			{
				stiRuntimeVariables.PageIndex++;
				stiRuntimeVariables.CurrentPrintPage++;
			}
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
					StiRuntimeVariables stiRuntimeVariables2 = new StiRuntimeVariables(stiSimpleText.Report)
					{
						PageIndex = stiSimpleText.Report.RenderedPages.Count,
						TextBox = stiSimpleText2
					};
					arrayList2.Add(stiRuntimeVariables2);
					StiContainer parent2 = masterComp.Parent;
					while (parent2 != null && !(parent2 is StiPage) && !(parent2 is StiOverlayBand))
					{
						parent2 = parent2.Parent;
					}
					if (parent2 != null && parent2 is StiOverlayBand)
					{
						stiRuntimeVariables2.PageIndex++;
						stiRuntimeVariables2.CurrentPrintPage++;
					}
				}
			}
			else
			{
				((StiText)stiSimpleText).ExcelDataValue = ((StiText)stiSimpleText).ExcelDataValue;
			}
		}
		return stiSimpleText2;
	}
}
