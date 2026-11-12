using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stimulsoft.Base.Dashboard;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.TreeView;
using Stimulsoft.Dashboard.Drawing.Helpers;
using Stimulsoft.Dashboard.Export.Settings;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.ShapeTypes;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Export.Tools;

public class StiTreeViewElementExportTool : StiListBoxElementExportTool
{
	public override void Render(IStiElement element, StiPanel destination, RectangleD rect, StiDashboardExportSettings settings)
	{
		base.Render(element, destination, rect, settings);
	}

	private StiCheckStyle GetCheckStyle(IStiElement element, StiTreeItem item)
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
		if (item.Items != null)
		{
			List<StiCheckStyle> source = item.Items.Select((StiTreeItem i) => GetCheckStyle(element, i)).ToList();
			if (source.All((StiCheckStyle c) => c == StiCheckStyle.CheckRectangle))
			{
				return StiCheckStyle.CheckRectangle;
			}
			if (source.All((StiCheckStyle c) => c == StiCheckStyle.NoneRectangle))
			{
				return StiCheckStyle.NoneRectangle;
			}
			return StiCheckStyle.DotRectangle;
		}
		if (stiUserFilters.UserFilters.Any((StiDataFilterRule f) => f.Condition == StiDataFilterCondition.EqualTo && f.Value == item.ToString()))
		{
			return StiCheckStyle.CheckRectangle;
		}
		return StiCheckStyle.NoneRectangle;
	}

	protected override void RenderItems(StiPanel destination, RectangleD rect, IStiElement element)
	{
		StiDataTable stiDataTable = StiElementDataCache.TryToGetOrCreate(element);
		if (stiDataTable != null)
		{
			IStiTreeViewElement stiTreeViewElement = element as IStiTreeViewElement;
			List<StiTreeItem> list = StiTreeViewHelper.FetchItems(stiDataTable, stiTreeViewElement.ShowBlanks);
			List<string> values = list?.Select((StiTreeItem i) => (i != null) ? i.ToString() : string.Empty)?.ToList();
			List<StiCheckStyle> checks = list?.Select((StiTreeItem i) => GetCheckStyle(element, i))?.ToList();
			RenderItems(destination, rect, element, values, checks);
		}
	}

	protected override void RenderItem(StiPanel destination, RectangleD rect, IStiElement element, string value, StiCheckStyle checkStyle, StiBorderSides lineSide, bool drawCheckBox)
	{
		if (value != StiLocHelper.LocAll)
		{
			RenderExpander(destination, rect, element);
			rect.X += StiElementConsts.ListBox.CheckBoxWidth / 2;
			rect.Width -= StiElementConsts.ListBox.CheckBoxWidth / 2;
		}
		base.RenderItem(destination, rect, element, value, checkStyle, lineSide, drawCheckBox);
	}

	private static void RenderExpander(StiPanel destination, RectangleD rect, IStiElement element)
	{
		rect = new RectangleD(rect.X, rect.Y + rect.Height / 3.0, StiElementConsts.ListBox.CheckBoxWidth / 3, rect.Height / 3.0);
		destination.Components.Add(new StiShape(rect)
		{
			Name = element.Name + "_ItemExpander",
			ShapeType = new StiTriangleShapeType(StiShapeDirection.Right),
			Brush = new StiSolidBrush(Color.DimGray),
			Style = StiPenStyle.None
		});
	}
}
