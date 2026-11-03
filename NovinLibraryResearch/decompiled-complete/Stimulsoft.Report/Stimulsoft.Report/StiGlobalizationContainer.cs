using System;
using System.Collections;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report;

public class StiGlobalizationContainer : IStiJsonReportObject
{
	[DefaultValue("")]
	[StiSerializable]
	public string CultureName { get; set; }

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	public StiGlobalizationItemCollection Items { get; set; } = new StiGlobalizationItemCollection();


	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		JObject val = new JObject();
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "CultureName", CultureName);
		if (Items.Count > 0)
		{
			JObject val2 = new JObject();
			int num = 0;
			foreach (StiGlobalizationItem item in Items)
			{
				JsonExtensions.AddPropertyJObject(val2, num.ToString(), item.SaveToJsonObject(mode));
				num++;
			}
			JsonExtensions.AddPropertyJObject(val, "Items", val2);
		}
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Expected O, but got Unknown
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (!(name == "CultureName"))
			{
				if (!(name == "Items"))
				{
					continue;
				}
				foreach (JProperty item2 in ((JObject)item.Value).Properties())
				{
					StiGlobalizationItem stiGlobalizationItem = new StiGlobalizationItem();
					stiGlobalizationItem.LoadFromJsonObject((JObject)item2.Value);
					Items.Add(stiGlobalizationItem);
				}
			}
			else
			{
				CultureName = StiJsonReportObjectHelper.DeserializeString(item);
			}
		}
	}

	public Hashtable GetAllStringsForReport(StiReport report)
	{
		Hashtable hashtable = new Hashtable();
		StiComponentsCollection components = report.GetComponents();
		foreach (StiComponent item in components)
		{
			if (!(item is IStiGlobalizationProvider stiGlobalizationProvider))
			{
				continue;
			}
			string[] allStrings = stiGlobalizationProvider.GetAllStrings();
			string[] array = allStrings;
			foreach (string text in array)
			{
				string key = item.Name + "." + text;
				string text2 = (string)stiGlobalizationProvider.GetString(text)?.Clone();
				if (text2 != null)
				{
					hashtable[key] = text2;
				}
			}
		}
		foreach (StiVariable variable in report.Dictionary.Variables)
		{
			string text3 = "Variable." + variable.Name;
			hashtable[text3] = variable.Value;
			if (!variable.RequestFromUser || !StiOptions.Engine.Globalization.AllowUseVariableAlias)
			{
				continue;
			}
			hashtable[text3 + ".Alias"] = variable.Alias ?? "";
			if (variable.DialogInfo?.Keys != null)
			{
				StiDialogInfo dialogInfo = variable.DialogInfo;
				if (dialogInfo != null && dialogInfo.Keys.Length != 0)
				{
					hashtable[text3 + ".Items.Keys"] = string.Join(Environment.NewLine, variable.DialogInfo.Keys);
				}
			}
			if (variable.DialogInfo?.Values != null)
			{
				StiDialogInfo dialogInfo2 = variable.DialogInfo;
				if (dialogInfo2 != null && dialogInfo2.Values.Length != 0)
				{
					hashtable[text3 + ".Items.Values"] = string.Join(Environment.NewLine, variable.DialogInfo.Values);
				}
			}
		}
		hashtable["ReportAuthor"] = report.ReportAuthor ?? "";
		hashtable["ReportDescription"] = report.ReportDescription ?? "";
		hashtable["ReportAlias"] = report.ReportAlias ?? "";
		return hashtable;
	}

	public void LocalizeReport(StiReport report)
	{
		Hashtable hashtable = new Hashtable();
		foreach (StiVariable variable in report.Dictionary.Variables)
		{
			string text = "Variable." + variable.Name;
			hashtable[text] = variable;
			if (!variable.RequestFromUser || !StiOptions.Engine.Globalization.AllowUseVariableAlias)
			{
				continue;
			}
			hashtable[text + ".Alias"] = variable;
			if (variable.DialogInfo?.Keys != null)
			{
				StiDialogInfo dialogInfo = variable.DialogInfo;
				if (dialogInfo != null && dialogInfo.Keys.Length != 0)
				{
					hashtable[text + ".Items.Keys"] = variable;
				}
			}
			if (variable.DialogInfo?.Values != null)
			{
				StiDialogInfo dialogInfo2 = variable.DialogInfo;
				if (dialogInfo2 != null && dialogInfo2.Values.Length != 0)
				{
					hashtable[text + ".Items.Values"] = variable;
				}
			}
		}
		foreach (StiGlobalizationItem item in Items)
		{
			string propertyName = item.PropertyName;
			switch (propertyName)
			{
			case "ReportAuthor":
				report.ReportAuthor = item.Text;
				continue;
			case "ReportDescription":
				report.ReportDescription = item.Text;
				continue;
			case "ReportAlias":
				report.ReportAlias = item.Text;
				continue;
			}
			if (hashtable[propertyName] is StiVariable)
			{
				StiVariable stiVariable2 = hashtable[propertyName] as StiVariable;
				if (propertyName.EndsWith(".Alias"))
				{
					stiVariable2.Alias = item.Text;
				}
				else if (propertyName.EndsWith(".Items.Keys"))
				{
					stiVariable2.DialogInfo.Keys = item.Text.Split(new string[2]
					{
						Environment.NewLine,
						"\n"
					}, StringSplitOptions.RemoveEmptyEntries);
				}
				else if (propertyName.EndsWith(".Items.Values"))
				{
					stiVariable2.DialogInfo.Values = item.Text.Split(new string[2]
					{
						Environment.NewLine,
						"\n"
					}, StringSplitOptions.RemoveEmptyEntries);
				}
				else
				{
					stiVariable2.Value = item.Text;
				}
				continue;
			}
			int num = StringExt.IndexOfInvariant(propertyName, ".");
			if (num != -1)
			{
				string name = propertyName.Substring(0, num);
				string propertyName2 = propertyName.Substring(num + 1);
				if (report.GetComponents()[name] is IStiGlobalizationProvider stiGlobalizationProvider)
				{
					stiGlobalizationProvider.SetString(propertyName2, item.Text);
				}
			}
		}
	}

	public void FillItemsFromReport(StiReport report)
	{
		Hashtable allStringsForReport = GetAllStringsForReport(report);
		Hashtable hashtable = new Hashtable();
		foreach (StiGlobalizationItem item in Items)
		{
			hashtable[item.PropertyName] = item;
		}
		foreach (string key in allStringsForReport.Keys)
		{
			if (hashtable[key] == null)
			{
				Items.Add(new StiGlobalizationItem(key, allStringsForReport[key] as string));
			}
		}
	}

	public void RemoveUnlocalizedItemsFromReport(StiReport report)
	{
		Hashtable allStringsForReport = GetAllStringsForReport(report);
		Hashtable hashtable = new Hashtable();
		foreach (StiGlobalizationItem item in Items)
		{
			hashtable[item.PropertyName] = item;
		}
		foreach (string key in allStringsForReport.Keys)
		{
			string text = allStringsForReport[key] as string;
			if (hashtable[key] is StiGlobalizationItem stiGlobalizationItem2 && text == stiGlobalizationItem2.Text)
			{
				Items.Remove(stiGlobalizationItem2);
			}
		}
	}

	public StiGlobalizationContainer()
		: this("en")
	{
	}

	public StiGlobalizationContainer(string cultureName)
	{
		CultureName = cultureName;
	}
}
