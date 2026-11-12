using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stimulsoft.Base.Dashboard;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.ListBox;
using Stimulsoft.Dashboard.Drawing.Helpers;
using Stimulsoft.Dashboard.Export.Settings;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;

namespace Stimulsoft.Dashboard.Export.Tools;

public class StiListBoxElementExportTool : StiElementExportTool
{
	public override void Render(IStiElement element, StiPanel destination, RectangleD rect, StiDashboardExportSettings settings)
	{
		if (RenderEmptyDataMessage(element, destination, rect, settings))
		{
			return;
		}
		StiReport report = destination.Report;
		StiPage page = destination.Page;
		StiListBoxElement stiListBoxElement = element as StiListBoxElement;
		StiPanel stiPanel = destination.Clone(cloneProperties: true, cloneComponents: false) as StiPanel;
		bool flag = settings.RenderSingleElement && (settings.Format == StiDashboardExportFormat.Excel || settings.Format == StiDashboardExportFormat.Image);
		if (!settings.RenderSinglePage || flag)
		{
			rect.Height = 2147483647.0;
		}
		RenderItems(stiPanel, rect, element);
		int num = 0;
		double num2 = destination.Top;
		if (stiPanel.Components.Count > 0)
		{
			num2 += stiPanel.GetComponentsList().Min((StiComponent c) => c.Top);
		}
		while (true)
		{
			List<StiComponent> componentsList = stiPanel.GetComponentsList();
			foreach (StiComponent item in componentsList)
			{
				double num3 = item.Bottom;
				double left = item.Left;
				if (item is IStiShape)
				{
					num3 += (double)(StiElementConsts.ListBox.ItemHeight / 3);
				}
				if (stiListBoxElement != null)
				{
					if ((stiListBoxElement.Orientation == StiItemOrientation.Vertical && num3 <= destination.Height) || (stiListBoxElement.Orientation == StiItemOrientation.Horizontal && left <= destination.Width))
					{
						stiPanel.Components.Remove(item);
						destination.Components.Add(item);
					}
				}
				else if (num3 <= destination.Height)
				{
					stiPanel.Components.Remove(item);
					destination.Components.Add(item);
				}
			}
			if ((settings != null && settings.RenderSinglePage) || stiPanel.Components.Count == 0)
			{
				break;
			}
			double minTop = stiPanel.GetComponentsList().Min((StiComponent c) => c.Top);
			stiPanel.GetComponentsList().ForEach(delegate(StiComponent c)
			{
				c.Top = c.Top - minTop + 1.0;
			});
			if (report.RenderedPages.Count <= ++num)
			{
				report.RenderedPages.Add((StiPage)page.Clone(cloneProperties: true, cloneComponents: false));
			}
			page = report.RenderedPages[num];
			destination = (StiPanel)destination.Clone(cloneProperties: true, cloneComponents: false);
			if (settings == null || !settings.RenderSingleElement)
			{
				destination.Top = num2;
			}
			else
			{
				destination.Top = 0.0;
				destination.Left = 0.0;
			}
			page.Components.Add(destination);
		}
		foreach (StiPage renderedPage in destination.Report.RenderedPages)
		{
			renderedPage.GetComponentsList().ForEach(delegate(StiComponent c)
			{
				c.Report = report;
				c.Page = page;
			});
		}
	}

	private StiCheckStyle GetCheckStyle(IStiElement element, StiListBoxItem item)
	{
		IStiUserFilters stiUserFilters = (IStiUserFilters)element;
		if (!stiUserFilters.UserFilters.Any())
		{
			return StiCheckStyle.CheckRectangle;
		}
		if (stiUserFilters.UserFilters.Any((StiDataFilterRule f) => f.Condition == StiDataFilterCondition.IsFalse))
		{
			return StiCheckStyle.NoneRectangle;
		}
		if (stiUserFilters.UserFilters.Any((StiDataFilterRule f) => f.Condition == StiDataFilterCondition.EqualTo && f.Value == item?.ToString()))
		{
			return StiCheckStyle.CheckRectangle;
		}
		return StiCheckStyle.NoneRectangle;
	}

	protected virtual void RenderItems(StiPanel destination, RectangleD rect, IStiElement element)
	{
		StiDataTable stiDataTable = StiElementDataCache.TryToGetOrCreate(element);
		if (stiDataTable != null)
		{
			IStiUserFilters stiUserFilters = (IStiUserFilters)element;
			StiListBoxElement stiListBoxElement = element as StiListBoxElement;
			StiListBoxItem[] array = StiListBoxHelper.FetchItems(stiListBoxElement, stiDataTable, stiListBoxElement.ShowBlanks);
			List<string> values = array?.Select((StiListBoxItem i) => (i != null) ? i.ToString() : string.Empty)?.ToList();
			List<StiCheckStyle> checks = array?.Select((StiListBoxItem i) => GetCheckStyle(element, i))?.ToList();
			RenderItems(destination, rect, element, values, checks);
		}
	}

	protected void RenderItems(StiPanel destination, RectangleD rect, IStiElement element, IList<string> values, IList<StiCheckStyle> checks)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		IStiItemElement stiItemElement = (IStiItemElement)element;
		bool drawCheckBox = stiItemElement.SelectionMode == StiItemSelectionMode.Multi;
		bool flag = element is IStiListBoxElement && ((IStiListBoxElement)element).Orientation == StiItemOrientation.Horizontal;
		Font val = (Font)(((object)(stiItemElement as IStiFont)?.Font) ?? ((object)new Font("Arial", 8f)));
		IStiUserFilters stiUserFilters = (IStiUserFilters)element;
		double num = (flag ? GetHorizontalItemAutoWidth(stiItemElement, val, values, rect.Width, drawCheckBox) : rect.Width);
		double num2 = (flag ? rect.Height : ((double)StiElementConsts.ListBox.ItemHeight));
		double num3 = rect.X;
		double num4 = rect.Y;
		if (stiItemElement.ShowAllValue)
		{
			if (flag && num == 0.0)
			{
				num = MeasureItemWidth(StiLocHelper.LocAll, val, drawCheckBox);
			}
			RectangleD rect2 = new RectangleD(num3, num4, num, num2);
			StiCheckStyle checkStyle = StiCheckStyle.CheckRectangle;
			if (stiUserFilters.UserFilters.Any())
			{
				checkStyle = (stiUserFilters.UserFilters.Any((StiDataFilterRule f) => f.Condition == StiDataFilterCondition.IsFalse) ? StiCheckStyle.NoneRectangle : StiCheckStyle.DotRectangle);
			}
			RenderItem(destination, rect2, element, StiLocHelper.LocAll, checkStyle, flag ? StiBorderSides.Right : StiBorderSides.Bottom, drawCheckBox);
			if (flag)
			{
				num3 += num + 1.0;
			}
			else
			{
				num4 += num2 + 1.0;
			}
		}
		if (values == null)
		{
			return;
		}
		for (int i = 0; i < values.Count(); i++)
		{
			if (flag && num == 0.0)
			{
				num = MeasureItemWidth(values[i], val, drawCheckBox);
			}
			RectangleD rect3 = new RectangleD(num3, num4, num, num2);
			RenderItem(destination, rect3, element, values[i], checks[i], StiBorderSides.None, drawCheckBox);
			if (flag)
			{
				num3 += num + 1.0;
			}
			else
			{
				num4 += num2 + 1.0;
			}
			if ((flag && num3 >= rect.Right) || (!flag && num4 >= rect.Bottom))
			{
				break;
			}
		}
	}

	private double GetHorizontalItemAutoWidth(IStiItemElement itemElement, Font itemsFont, IList<string> values, double elementWidth, bool drawCheckBox)
	{
		double num = 0.0;
		int num2 = 0;
		while (num < elementWidth && num2 < values.Count)
		{
			num += MeasureItemWidth(values[num2], itemsFont, drawCheckBox);
			num2++;
		}
		if (itemElement.ShowAllValue)
		{
			MeasureItemWidth(StiLocHelper.LocAll, itemsFont, drawCheckBox);
			num2++;
		}
		if (!(num < elementWidth) || num2 <= 0)
		{
			return 0.0;
		}
		return elementWidth / (double)num2;
	}

	private double MeasureItemWidth(string text, Font baseFont, bool drawCheckBox)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Expected O, but got Unknown
		Bitmap val = new Bitmap(1, 1);
		try
		{
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			try
			{
				float num = val2.MeasureString(text, baseFont).Width;
				if (drawCheckBox)
				{
					num += (float)StiElementConsts.ListBox.CheckBoxWidth;
				}
				return num;
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	protected virtual void RenderItem(StiPanel destination, RectangleD rect, IStiElement element, string value, StiCheckStyle checkStyle, StiBorderSides lineSide, bool drawCheckBox)
	{
		if (drawCheckBox)
		{
			bool flag = element is IStiListBoxElement && ((IStiListBoxElement)element).Orientation == StiItemOrientation.Horizontal;
			RenderCheckBox(destination, rect, element, checkStyle, (!flag) ? lineSide : StiBorderSides.None);
			rect.X += StiElementConsts.ListBox.CheckBoxWidth;
			rect.Width -= StiElementConsts.ListBox.CheckBoxWidth;
		}
		Color foreColor = StiDashboardStyleHelper.GetForeColor(element);
		Color backColor = StiDashboardStyleHelper.GetBackColor(element);
		value = Format(element as IStiControlElement, value);
		destination.Components.Add(new StiText(rect)
		{
			Name = element.Name + "_Item",
			AllowHtmlTags = true,
			Margins = (drawCheckBox ? new StiMargins(1.0, 0.0, 0.0, 0.0) : StiMargins.Empty),
			Border = ((lineSide != 0) ? new StiBorder(lineSide, Color.Gray, 1.0, StiPenStyle.Solid) : new StiBorder(StiBorderSides.None, Color.Transparent, 0.0, StiPenStyle.None)),
			Brush = new StiSolidBrush(backColor),
			ExcelDataValue = value,
			HorAlignment = StiTextHorAlignment.Left,
			VertAlignment = StiVertAlignment.Center,
			Text = value,
			TextBrush = new StiSolidBrush(foreColor),
			WordWrap = false
		});
	}

	private static void RenderCheckBox(StiPanel destination, RectangleD rect, IStiElement element, StiCheckStyle checkStyle, StiBorderSides lineSide)
	{
		Color foreColor = StiDashboardStyleHelper.GetForeColor(element);
		Color backColor = StiDashboardStyleHelper.GetBackColor(element);
		rect = new RectangleD(rect.X, rect.Y, StiElementConsts.ListBox.CheckBoxWidth, rect.Height);
		destination.Components.Add(new StiCheckBox(rect)
		{
			Name = element.Name + "_ItemCheckBox",
			Border = ((lineSide != 0) ? new StiBorder(lineSide, Color.Gray, 1.0, StiPenStyle.Solid) : new StiBorder(StiBorderSides.None, Color.Transparent, 0.0, StiPenStyle.None)),
			ContourColor = Color.Transparent,
			TextBrush = new StiSolidBrush(foreColor),
			Brush = new StiSolidBrush(backColor),
			CheckStyleForTrue = checkStyle,
			CheckStyleForFalse = StiCheckStyle.NoneRectangle,
			CheckedValue = (checkStyle != StiCheckStyle.NoneRectangle)
		});
	}
}
