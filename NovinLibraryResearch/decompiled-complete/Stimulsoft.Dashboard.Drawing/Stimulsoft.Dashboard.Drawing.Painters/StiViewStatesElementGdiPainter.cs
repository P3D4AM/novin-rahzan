using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Viewer.Images;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Helpers;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Design;

namespace Stimulsoft.Dashboard.Drawing.Painters;

public static class StiViewStatesElementGdiPainter
{
	public static void PaintViewStatesButtons(Graphics g, RectangleD rect, IStiElement element)
	{
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Expected O, but got Unknown
		if (!StiUserViewStatesMenuHelper.IsAllowUserViewStatesChanging(element))
		{
			return;
		}
		bool allowUserSorting = StiSortMenuHelper.IsAllowUserSorting(element);
		if (!(element is IStiUserViewStates stiUserViewStates))
		{
			return;
		}
		StiComponent stiComponent = element as StiComponent;
		if (!stiComponent.IsSelected || stiComponent.Report.Info.IsComponentsMoving || !element.IsDesigning || !StiInplaceEditorTitleElement.IsComponentFocused(stiComponent) || stiUserViewStates.UserViewStates == null || stiUserViewStates.UserViewStates.Count < 2)
		{
			return;
		}
		List<StiUserViewState> list = stiUserViewStates.UserViewStates.ToList();
		list.Reverse();
		StiControlElementStyle controlStyle = StiDashboardStyleHelper.GetControlStyle(element);
		int num = 0;
		foreach (StiUserViewState item in list)
		{
			Rectangle rectangle = StiInplaceEditorTitleElement.GetSimpleViewStateRect(rect, stiComponent, allowUserSorting, num).ToRectangle();
			if (!rectangle.IsEmpty)
			{
				if (StiInplaceEditorTitleElement.IsViewStateFocused(stiComponent, num))
				{
					StiDrawing.FillRectangle(g, controlStyle.HotBackColor, rectangle);
				}
				string text = item.SeriesType.ToString();
				if (StiDashboardStyleHelper.IsDarkStyle(element))
				{
					text = "White" + text;
				}
				Image designImage = StiDashboardDrawingImages.GetDesignImage("Meters.Charts." + text);
				try
				{
					Rectangle rectangle2 = new RectangleD(rectangle.X + (rectangle.Width - designImage.Width) / 2, rectangle.Y + (rectangle.Height - designImage.Height) / 2, designImage.Width, designImage.Height).ToRectangle();
					g.DrawImage(designImage, rectangle2);
				}
				finally
				{
					((IDisposable)designImage)?.Dispose();
				}
				if (item.Key == stiUserViewStates.SelectedViewStateKey)
				{
					rectangle.Width--;
					rectangle.Height--;
					Color color = (StiInplaceEditorTitleElement.IsViewStateFocused(stiComponent, num) ? controlStyle.HotBackColor : controlStyle.SelectedBackColor);
					Pen val = new Pen(color, (float)StiScale.I1);
					try
					{
						val.Alignment = (PenAlignment)1;
						g.DrawRectangle(val, rectangle);
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
				}
			}
			num++;
		}
	}
}
