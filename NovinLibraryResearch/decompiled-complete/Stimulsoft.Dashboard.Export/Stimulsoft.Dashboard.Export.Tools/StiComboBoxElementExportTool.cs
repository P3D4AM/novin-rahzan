using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Dashboard.Components.ComboBox;
using Stimulsoft.Dashboard.Drawing.Helpers;
using Stimulsoft.Dashboard.Export.Settings;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;

namespace Stimulsoft.Dashboard.Export.Tools;

public class StiComboBoxElementExportTool : StiElementExportTool
{
	public override void Render(IStiElement element, StiPanel destination, RectangleD rect, StiDashboardExportSettings settings)
	{
		if (element.GetMeters().Count == 0)
		{
			return;
		}
		StiDataTable stiDataTable = StiElementDataCache.TryToGetOrCreate(element);
		if (stiDataTable == null)
		{
			return;
		}
		Hashtable hashtable = new Hashtable();
		if (stiDataTable.Meters.Count > 1)
		{
			foreach (object[] row in stiDataTable.Rows)
			{
				string text = row[1]?.ToString();
				if (!string.IsNullOrWhiteSpace(text))
				{
					hashtable[text] = row[0];
				}
			}
		}
		IStiItemElement stiItemElement = (IStiItemElement)element;
		IStiUserFilters stiUserFilters = (IStiUserFilters)element;
		string value = string.Empty;
		if (stiItemElement.SelectionMode == StiItemSelectionMode.One)
		{
			if (stiUserFilters.UserFilters.Any())
			{
				value = stiUserFilters.UserFilters[0].Value;
				value = Format(element as IStiControlElement, hashtable[value] ?? value);
			}
			else
			{
				object obj;
				if (!stiItemElement.ShowAllValue)
				{
					object[]? array = stiDataTable.Rows.FirstOrDefault();
					obj = ((array == null) ? null : array[0]?.ToString()) ?? string.Empty;
				}
				else
				{
					obj = StiLocHelper.LocAll;
				}
				value = (string)obj;
			}
		}
		RenderElement(destination, element, rect, value);
		if (stiItemElement.SelectionMode != StiItemSelectionMode.Multi)
		{
			return;
		}
		RectangleD rect2 = rect;
		if (stiUserFilters.UserFilters.Any())
		{
			List<StiDataFilterRule> list = stiUserFilters.UserFilters;
			if (list.All((StiDataFilterRule f) => f.Condition == StiDataFilterCondition.NotEqualTo))
			{
				list = new List<StiDataFilterRule>();
				foreach (object[] row2 in stiDataTable.Rows)
				{
					string filterValue = row2[1]?.ToString();
					if (!stiUserFilters.UserFilters.Any((StiDataFilterRule f) => f.Value == filterValue))
					{
						list.Add(new StiDataFilterRule(null, StiDataFilterCondition.EqualTo, filterValue));
					}
				}
			}
			{
				foreach (StiDataFilterRule item in list)
				{
					if (rect2.X + rect2.Width <= rect.X + rect.Width)
					{
						if (item.Value != null)
						{
							rect2 = RenderItem(destination, element, rect2, Format(element as IStiControlElement, hashtable[item.Value] ?? item.Value));
							rect2.X += rect2.Width + 4.0;
						}
						continue;
					}
					while (destination.Components.Count > 1)
					{
						destination.Components.RemoveAt(1);
					}
					RenderItem(destination, element, rect, string.Format(Loc.Get("Dashboard", "NSelected"), stiUserFilters.UserFilters.Count()));
					break;
				}
				return;
			}
		}
		if (stiItemElement.ShowAllValue)
		{
			RenderItem(destination, element, rect, StiLocHelper.LocAll);
			return;
		}
		int valuesCount = GetValuesCount(element, stiDataTable);
		RenderItem(destination, element, rect, string.Format(Loc.Get("Dashboard", "NSelected"), valuesCount));
	}

	protected virtual int GetValuesCount(IStiElement element, StiDataTable dataTable)
	{
		StiComboBoxElement stiComboBoxElement = element as StiComboBoxElement;
		return StiComboBoxHelper.FetchItems(stiComboBoxElement, dataTable, stiComboBoxElement.ShowBlanks)?.Count() ?? 0;
	}

	private void RenderElement(StiPanel destination, IStiElement element, RectangleD rect, string value)
	{
		Color foreColor = StiDashboardStyleHelper.GetForeColor(element);
		Color backColor = StiDashboardStyleHelper.GetBackColor(element);
		StiText stiText = new StiText(rect);
		stiText.Name = element.Name + "_ComboBox";
		stiText.AllowHtmlTags = true;
		stiText.VertAlignment = StiVertAlignment.Center;
		stiText.HorAlignment = StiTextHorAlignment.Left;
		stiText.TextBrush = new StiSolidBrush(foreColor);
		stiText.Brush = new StiSolidBrush(backColor);
		stiText.Text = Format(element as IStiControlElement, value);
		StiText component = stiText;
		destination.Components.Add(component);
	}

	private RectangleD RenderItem(StiPanel destination, IStiElement element, RectangleD rect, string value)
	{
		StiControlElementStyle controlStyle = StiDashboardStyleHelper.GetControlStyle((IStiControlElement)element);
		value = Format(element as IStiControlElement, value);
		StiText stiText = new StiText
		{
			Name = element.Name + "_ComboBoxItem",
			AllowHtmlTags = true,
			VertAlignment = StiVertAlignment.Center,
			HorAlignment = StiTextHorAlignment.Center,
			TextBrush = new StiSolidBrush(controlStyle.SelectedForeColor),
			Brush = new StiSolidBrush(controlStyle.SelectedBackColor),
			Text = value
		};
		destination.Components.Add(stiText);
		SizeF sizeF = destination.GetMeasureGraphics().MeasureString(value, stiText.Font);
		return stiText.ClientRectangle = new RectangleD(rect.X, rect.Y, sizeF.Width + 4f, rect.Height);
	}
}
