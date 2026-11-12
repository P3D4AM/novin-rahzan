using System;
using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Viewer.Images;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Helpers;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Design;

namespace Stimulsoft.Dashboard.Drawing.Painters;

public static class StiSortElementGdiPainter
{
	public static void PaintSortButton(Graphics g, RectangleD rect, IStiElement element)
	{
		if (!StiSortMenuHelper.IsAllowUserSorting(element) || !(element is IStiUserSorts))
		{
			return;
		}
		StiComponent stiComponent = element as StiComponent;
		if (!stiComponent.IsSelected || stiComponent.Report.Info.IsComponentsMoving || !element.IsDesigning || !StiInplaceEditorTitleElement.IsComponentFocused(stiComponent))
		{
			return;
		}
		StiControlElementStyle controlStyle = StiDashboardStyleHelper.GetControlStyle(element);
		Rectangle rect2 = StiInplaceEditorTitleElement.GetSimpleSortRect(rect, stiComponent).ToRectangle();
		if (rect2.IsEmpty)
		{
			return;
		}
		bool isDark = StiDashboardStyleHelper.IsDarkStyle(element);
		if (StiInplaceEditorTitleElement.IsSortFocused(stiComponent))
		{
			StiDrawing.FillRectangle(g, controlStyle.HotBackColor, rect2);
		}
		Image val = StiDashboardDrawingImages.Actions.Sort(isDark);
		try
		{
			Rectangle rectangle = new RectangleD(rect2.X + (rect2.Width - val.Width) / 2, rect2.Y + (rect2.Height - val.Height) / 2, val.Width, val.Height).ToRectangle();
			g.DrawImage(val, rectangle);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}
}
