using System;
using System.Collections;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Web;

internal class StiEditableFieldsHelper
{
	public static bool CheckEditableReport(StiReport report)
	{
		if ((report.PreviewSettings & 0x10000) == 0)
		{
			return false;
		}
		StiComponentsCollection renderedComponents = report.GetRenderedComponents();
		foreach (StiComponent item in renderedComponents)
		{
			if (item is StiText && ((StiText)item).Editable)
			{
				return true;
			}
			if (item is StiCheckBox && ((StiCheckBox)item).Editable)
			{
				return true;
			}
			if (item is StiRichText && ((StiRichText)item).Editable)
			{
				return true;
			}
		}
		return false;
	}

	public static void ApplyEditableFieldsToReport(StiReport report, object parameters)
	{
		if (parameters == null)
		{
			return;
		}
		try
		{
			Hashtable hashtable = (Hashtable)parameters;
			foreach (DictionaryEntry item in hashtable)
			{
				int num = Convert.ToInt32(item.Key);
				Hashtable hashtable2 = (Hashtable)item.Value;
				foreach (DictionaryEntry item2 in hashtable2)
				{
					int num2 = Convert.ToInt32(item2.Key);
					Hashtable hashtable3 = (Hashtable)item2.Value;
					if (num >= report.RenderedPages.Count)
					{
						continue;
					}
					StiPage stiPage = report.RenderedPages[num];
					if (num2 >= stiPage.Components.Count)
					{
						continue;
					}
					StiComponent stiComponent = stiPage.Components[num2];
					if ((string)hashtable3["type"] == "CheckBox" && stiComponent is StiCheckBox)
					{
						((StiCheckBox)stiComponent).CheckedValue = (((bool)hashtable3["checked"]) ? "true" : "false");
					}
					else if ((string)hashtable3["type"] == "Text" && stiComponent is StiText)
					{
						string text = (string)hashtable3["text"];
						if (((StiText)stiComponent).AllowHtmlTags)
						{
							text = text.Replace("\n", "<br>");
						}
						((StiText)stiComponent).Text.Value = text;
					}
				}
			}
		}
		catch (Exception ex)
		{
			Console.Write(ex.Message);
		}
	}
}
