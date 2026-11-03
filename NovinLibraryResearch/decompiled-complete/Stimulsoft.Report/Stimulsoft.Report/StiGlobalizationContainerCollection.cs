using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report;

public class StiGlobalizationContainerCollection : CollectionBase, IStiJsonReportObject
{
	private StiReport report;

	public StiGlobalizationContainer this[int index]
	{
		get
		{
			return (StiGlobalizationContainer)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	public StiGlobalizationContainer this[string name]
	{
		get
		{
			name = name.ToLowerInvariant();
			foreach (StiGlobalizationContainer item in base.List)
			{
				if (item.CultureName.ToLowerInvariant() == name)
				{
					return item;
				}
			}
			return null;
		}
		set
		{
			name = name.ToLowerInvariant();
			for (int i = 0; i < base.List.Count; i++)
			{
				StiGlobalizationContainer stiGlobalizationContainer = base.List[i] as StiGlobalizationContainer;
				if (stiGlobalizationContainer.CultureName.ToLowerInvariant() == name)
				{
					base.List[i] = value;
					return;
				}
			}
			Add(value);
		}
	}

	internal bool SkipException { get; set; }

	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		if (base.List.Count == 0)
		{
			return null;
		}
		JObject val = new JObject();
		int num = 0;
		foreach (StiGlobalizationContainer item in base.List)
		{
			JsonExtensions.AddPropertyJObject(val, num.ToString(), item.SaveToJsonObject(mode));
			num++;
		}
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		foreach (JProperty item in jObject.Properties())
		{
			StiGlobalizationContainer stiGlobalizationContainer = new StiGlobalizationContainer();
			stiGlobalizationContainer.LoadFromJsonObject((JObject)item.Value);
			base.List.Add(stiGlobalizationContainer);
		}
	}

	public void SaveToFile(string file)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Expected O, but got Unknown
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		StiReport stiReport = new StiReport();
		stiReport.ReportUnit = StiReportUnitType.HundredthsOfInch;
		stiReport.IsRendered = true;
		StiPage stiPage = new StiPage();
		stiPage.UnlimitedHeight = true;
		stiReport.RenderedPages.Add(stiPage);
		double num = 0.0;
		int num2 = 50;
		int num3 = 400;
		int num4 = 20;
		StiSolidBrush brush = new StiSolidBrush(StiColor.Get("ddd"));
		stiPage.Components.Add(new StiText(new RectangleD(0.0, num, (double)num2, (double)num4), "Culture")
		{
			Brush = (StiBrush)(object)brush,
			VertAlignment = (StiVertAlignment)1
		});
		stiPage.Components.Add(new StiText(new RectangleD((double)num2, num, (double)num3, (double)num4), "Property")
		{
			Brush = (StiBrush)(object)brush,
			VertAlignment = (StiVertAlignment)1
		});
		stiPage.Components.Add(new StiText(new RectangleD((double)(num2 + num3), num, (double)num3, (double)num4), "Value")
		{
			Brush = (StiBrush)(object)brush,
			VertAlignment = (StiVertAlignment)1
		});
		num += (double)num4;
		int num5 = 255;
		foreach (StiGlobalizationContainer globalizationString in report.GlobalizationStrings)
		{
			Color color = Color.FromArgb(255, num5, num5, num5);
			brush = new StiSolidBrush(color);
			foreach (StiGlobalizationItem item in globalizationString.Items)
			{
				stiPage.Components.Add(new StiText(new RectangleD(0.0, num, (double)num2, (double)num4), globalizationString.CultureName)
				{
					Brush = (StiBrush)(object)brush
				});
				stiPage.Components.Add(new StiText(new RectangleD((double)num2, num, (double)num3, (double)num4), item.PropertyName)
				{
					Brush = (StiBrush)(object)brush
				});
				stiPage.Components.Add(new StiText(new RectangleD((double)(num2 + num3), num, (double)num3, (double)num4), item.Text)
				{
					Brush = (StiBrush)(object)brush
				});
				num += (double)num4;
			}
			num5 -= 5;
			if (num5 < 200)
			{
				num5 = 200;
			}
		}
		stiPage.Height = num;
		stiReport.ExportDocument(StiExportFormat.Excel2007, file);
	}

	public void LoadFromFile(string file)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		DataSet dataSet = ((StiFileDataConnector)StiExcelConnector.Get()).GetDataSet((StiFileDataOptions)new StiExcelOptions(File.ReadAllBytes(file), true));
		DataTable dataTable = dataSet.Tables.Cast<DataTable>().FirstOrDefault();
		if (dataTable == null)
		{
			MessageBox.Show("An incorrect format of the structure of globalization data!");
			return;
		}
		if (!dataTable.Columns.Contains("Culture"))
		{
			MessageBox.Show("A 'Culture' column is found in the file!");
			return;
		}
		if (!dataTable.Columns.Contains("Property"))
		{
			MessageBox.Show("A 'Property' column is found in the file!");
			return;
		}
		if (!dataTable.Columns.Contains("Value"))
		{
			MessageBox.Show("A 'Value' column is found in the file!");
			return;
		}
		report.GlobalizationStrings.Clear();
		foreach (DataRow row in dataTable.Rows)
		{
			string text = row["Culture"] as string;
			string text2 = row["Property"] as string;
			string text3 = row["Value"] as string;
			text = text ?? "";
			text2 = text2 ?? "";
			text3 = text3 ?? "";
			StiGlobalizationContainer stiGlobalizationContainer = report.GlobalizationStrings[text];
			if (stiGlobalizationContainer == null)
			{
				stiGlobalizationContainer = new StiGlobalizationContainer(text);
				report.GlobalizationStrings.Add(stiGlobalizationContainer);
			}
			stiGlobalizationContainer.Items.Add(new StiGlobalizationItem(text2, text3));
		}
	}

	public List<StiGlobalizationContainer> ToList()
	{
		return this.Cast<object>().Cast<StiGlobalizationContainer>().ToList();
	}

	public void Add(StiGlobalizationContainer data)
	{
		base.List.Add(data);
	}

	public void AddRange(StiGlobalizationContainer[] data)
	{
		base.InnerList.AddRange(data);
	}

	public bool Contains(StiGlobalizationContainer data)
	{
		return base.List.Contains(data);
	}

	public int IndexOf(StiGlobalizationContainer data)
	{
		return base.List.IndexOf(data);
	}

	public void Insert(int index, StiGlobalizationContainer data)
	{
		lock (this)
		{
			base.List.Insert(index, data);
		}
	}

	public void Remove(StiGlobalizationContainer data)
	{
		lock (this)
		{
			base.List.Remove(data);
		}
	}

	private string GetShortName(string cultureName)
	{
		int num = cultureName.IndexOf("-", StringComparison.InvariantCulture);
		if (num > -1)
		{
			return cultureName.Substring(0, num);
		}
		return cultureName;
	}

	public void LocalizeReport(string cultureName)
	{
		StiGlobalizationContainer stiGlobalizationContainer = this[cultureName];
		if (stiGlobalizationContainer == null)
		{
			stiGlobalizationContainer = this[GetShortName(cultureName)];
		}
		if (stiGlobalizationContainer == null && !SkipException)
		{
			throw new Exception("Can't find globalized strings for culture " + cultureName);
		}
		if (stiGlobalizationContainer != null)
		{
			FillItemsFromReport();
			stiGlobalizationContainer.LocalizeReport(report);
			RemoveUnlocalizedItemsFromReport();
		}
	}

	public void LocalizeReport(CultureInfo info)
	{
		LocalizeReport(info.Name);
	}

	public void FillItemsFromReport()
	{
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				StiGlobalizationContainer stiGlobalizationContainer = (StiGlobalizationContainer)enumerator.Current;
				stiGlobalizationContainer.FillItemsFromReport(report);
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
	}

	public void RemoveUnlocalizedItemsFromReport()
	{
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				StiGlobalizationContainer stiGlobalizationContainer = (StiGlobalizationContainer)enumerator.Current;
				stiGlobalizationContainer.RemoveUnlocalizedItemsFromReport(report);
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
	}

	public void RemoveComponent(StiComponent comp)
	{
		if (!(comp is IStiGlobalizationProvider stiGlobalizationProvider))
		{
			return;
		}
		string[] allStrings = stiGlobalizationProvider.GetAllStrings();
		string[] array = allStrings;
		foreach (string text in array)
		{
			string text2 = comp.Name + "." + text;
			{
				IEnumerator enumerator = GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						StiGlobalizationContainer stiGlobalizationContainer = (StiGlobalizationContainer)enumerator.Current;
						int num = 0;
						while (num < stiGlobalizationContainer.Items.Count)
						{
							StiGlobalizationItem stiGlobalizationItem = stiGlobalizationContainer.Items[num];
							if (stiGlobalizationItem.PropertyName == text2)
							{
								stiGlobalizationContainer.Items.RemoveAt(num);
							}
							else
							{
								num++;
							}
						}
					}
				}
				finally
				{
					IDisposable disposable = enumerator as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
			}
		}
	}

	public void RenameComponent(StiComponent comp, string oldName, string newName)
	{
		if (!(comp is IStiGlobalizationProvider stiGlobalizationProvider))
		{
			return;
		}
		string[] allStrings = stiGlobalizationProvider.GetAllStrings();
		string[] array = allStrings;
		foreach (string text in array)
		{
			string text2 = oldName + "." + text;
			string propertyName = newName + "." + text;
			{
				IEnumerator enumerator = GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						StiGlobalizationContainer stiGlobalizationContainer = (StiGlobalizationContainer)enumerator.Current;
						foreach (StiGlobalizationItem item in stiGlobalizationContainer.Items)
						{
							if (item.PropertyName == text2)
							{
								item.PropertyName = propertyName;
							}
						}
					}
				}
				finally
				{
					IDisposable disposable = enumerator as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
			}
		}
	}

	public StiGlobalizationContainerCollection(StiReport report)
	{
		this.report = report;
	}
}
