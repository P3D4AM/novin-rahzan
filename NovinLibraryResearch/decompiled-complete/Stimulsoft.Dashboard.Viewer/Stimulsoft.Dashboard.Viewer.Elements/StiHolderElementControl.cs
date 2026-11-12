using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Drawing.Helpers;
using Stimulsoft.Dashboard.Drawing.Painters;
using Stimulsoft.Dashboard.Viewer.Controls;
using Stimulsoft.Dashboard.Viewer.Controls.Helpers;
using Stimulsoft.Dashboard.Viewer.Images;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Dashboard.Viewer.Elements;

[ToolboxItem(false)]
public class StiHolderElementControl : Panel, IStiActionButtons, IStiRefreshControl, IStiElementControl
{
	public StiViewerMenuActionButton MenuButton { get; set; }

	public StiSaveActionButton SaveButton { get; set; }

	public StiViewDataActionButton ViewDataButton { get; set; }

	public StiDrillUpActionButton DrillUpButton { get; set; }

	public StiDrillDownActionButton DrillDownButton { get; set; }

	public StiDrillDownSelectedActionButton DrillDownSelectedButton { get; set; }

	public StiDrillDownCancelActionButton DrillDownCancelButton { get; set; }

	public StiFilterMultiOptionActionCheckBox FilterMultiOptionButton { get; set; }

	public StiRemoveFilterActionButton RemoveFilterButton { get; set; }

	public StiOpenFullScreenActionButton OpenFullScreenButton { get; set; }

	public StiCloseFullScreenActionButton CloseFullScreenButton { get; set; }

	public StiSortActionButton SortButton { get; set; }

	public StiColumnSelectionActionButton SelectColumnsButton { get; set; }

	public List<StiViewStateActionButton> ViewStateButtons { get; set; }

	public IStiElement Element { get; set; }

	public bool IsDrillSelectionActivated { get; set; }

	private bool IsMouseOver => new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height).Contains(((Control)this).PointToClient(Cursor.Position));

	private bool IsFilterPresent
	{
		get
		{
			if (StiUserFiltersHelper.IsFilterPresent(Element) && Title != null)
			{
				return Title.Visible;
			}
			return false;
		}
	}

	public StiSimpleBorder Border
	{
		get
		{
			if (!(Element is IStiSimpleBorder stiSimpleBorder))
			{
				return null;
			}
			return stiSimpleBorder.Border;
		}
	}

	public StiCornerRadius CornerRadius
	{
		get
		{
			if (!(Element is IStiCornerRadius stiCornerRadius))
			{
				return null;
			}
			return stiCornerRadius.CornerRadius;
		}
	}

	public StiTitle Title
	{
		get
		{
			if (!(Element is IStiTitleElement stiTitleElement))
			{
				return null;
			}
			return stiTitleElement.Title as StiTitle;
		}
	}

	public Control Child { get; set; }

	public StiElementControl ElementChild => Child as StiElementControl;

	public void RefreshControl(object sender, StiReportControlUpdateEventArgs e)
	{
		if (sender == Element)
		{
			RefreshChildLocation();
			(Child as IStiRefreshControl)?.RefreshControl(sender, e);
			((Control)this).Invalidate();
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		try
		{
			Graphics graphics = e.Graphics;
			Rectangle rectangle = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
			Color backColor = StiDashboardStyleHelper.GetBackColor(Element, null, allowOpacity: true);
			StiComponent stiComponent = Element as StiComponent;
			StiRoundedBackgroundGdiPainter.PaintBackground(graphics, rectangle, Border, CornerRadius, backColor, StiButtonShapeType.Rectangle, isEnabled: true, 1f);
			Rectangle borderContentRect = StiElementControlPainter.GetBorderContentRect(rectangle, Border);
			StiElementControlPainter.PaintTitle(graphics, Element as StiElement, rectangle, Title);
			RectangleD paintTitleRect = StiElementGdiPainter.GetPaintTitleRect(graphics, RectangleD.CreateFromRectangle(borderContentRect), Element as IStiTitleElement, Title?.Text);
			if (!IsMouseOver && IsFilterPresent)
			{
				PaintFilter(e, graphics, ((Title.HorAlignment != StiHorAlignment.Right) ? new RectangleD(paintTitleRect.Right, paintTitleRect.Top, (double)borderContentRect.Right - paintTitleRect.Right, paintTitleRect.Height) : new RectangleD(borderContentRect.Left, paintTitleRect.Top, paintTitleRect.Left - (double)borderContentRect.Left, paintTitleRect.Height)).ToRectangle(), Title);
			}
			rectangle.Width--;
			rectangle.Height--;
			if (CornerRadius != null && !CornerRadius.IsDefault)
			{
				StiRoundedBorderGdiPainter.PaintBorder(graphics, rectangle, Border, CornerRadius, StiButtonShapeType.Rectangle, 1f, stiComponent.IsDesigning);
			}
			else
			{
				StiElementControlPainter.PaintBorder(graphics, rectangle, Border, ((Control)this).BackColor);
			}
		}
		catch
		{
		}
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		((Control)this).OnMouseEnter(e);
		(Child as IStiMouseEventsControl)?.InvokeMouseEnter();
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
		RefreshActionButtons();
		if (IsFilterPresent)
		{
			((Control)this).Invalidate();
		}
		(Child as IStiMouseEventsControl)?.InvokeMouseMove(e);
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((Control)this).OnMouseLeave(e);
		RefreshActionButtons();
		if (IsFilterPresent)
		{
			((Control)this).Invalidate();
		}
		(Child as IStiMouseEventsControl)?.InvokeMouseLeave();
	}

	protected override void OnResize(EventArgs e)
	{
		((Panel)this).OnResize(e);
		RefreshChildLocation();
	}

	private void PaintFilter(PaintEventArgs e, Graphics g, Rectangle filterRect, StiTitle title)
	{
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Expected O, but got Unknown
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		filterRect.X += StiScale.XXI(20.0);
		filterRect.Width -= StiScale.XXI(40.0);
		filterRect.Inflate(-StiScale.I4, -StiScale.I4);
		if (filterRect.Width < StiScale.XXI(50.0))
		{
			return;
		}
		string dataTableFilterQueryStringRepresentation = GetDataTableFilterQueryStringRepresentation(Element);
		if (string.IsNullOrWhiteSpace(dataTableFilterQueryStringRepresentation))
		{
			return;
		}
		Color titleForeColor = StiDashboardStyleHelper.GetTitleForeColor(Element);
		Font val = new Font("Arial", 8f);
		try
		{
			StringFormat val2 = new StringFormat();
			try
			{
				TextRenderingHint textRenderingHint = g.TextRenderingHint;
				g.TextRenderingHint = (TextRenderingHint)4;
				val2.Alignment = (StringAlignment)((title.HorAlignment != StiHorAlignment.Right) ? 2 : 0);
				val2.FormatFlags = (StringFormatFlags)4096;
				val2.LineAlignment = (StringAlignment)1;
				val2.Trimming = (StringTrimming)3;
				StiTextDrawing.DrawString(e.Graphics, dataTableFilterQueryStringRepresentation, val, titleForeColor, RectangleD.CreateFromRectangle(filterRect), val2);
				g.TextRenderingHint = textRenderingHint;
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
		Rectangle rectangle = ((title.HorAlignment == StiHorAlignment.Right) ? new Rectangle(filterRect.X - StiScale.XXI(20.0), filterRect.Top + (filterRect.Height - StiScale.I16) / 2 - StiScale.I1, StiScale.XXI(16.0), StiScale.YYI(16.0)) : new Rectangle(filterRect.Right + StiScale.I4, filterRect.Top + (filterRect.Height - StiScale.I16) / 2 - StiScale.I1, StiScale.XXI(16.0), StiScale.YYI(16.0)));
		Image val3 = StiDashboardViewerImages.Actions.Filter(StiDashboardStyleHelper.IsDarkStyle(Element));
		try
		{
			g.DrawImage(val3, rectangle);
		}
		finally
		{
			((IDisposable)val3)?.Dispose();
		}
	}

	internal static string GetDataTableFilterQueryStringRepresentation(IStiElement element)
	{
		if (!(element is IStiUserFilters stiUserFilters))
		{
			return "";
		}
		List<StiDataFilterRule> list = stiUserFilters.UserFilters.Select((StiDataFilterRule f) => f.Clone()).Cast<StiDataFilterRule>().ToList();
		list.ForEach(delegate(StiDataFilterRule f)
		{
			int num = ((f.Path != null) ? f.Path.IndexOfInvariant(".") : (-1));
			if (num != -1)
			{
				f.Path = f.Path.Substring(num + 1);
			}
			int num2 = ((f.Path2 != null) ? f.Path2.IndexOfInvariant(".") : (-1));
			if (num2 != -1)
			{
				f.Path2 = f.Path2.Substring(num2 + 1);
			}
			DateTime? dateTime = StiValueHelper.TryToNullableDateTime(f.Value);
			if (dateTime.HasValue)
			{
				f.Value = dateTime.Value.ToShortDateString();
			}
			DateTime? dateTime2 = StiValueHelper.TryToNullableDateTime(f.Value2);
			if (dateTime2.HasValue)
			{
				f.Value2 = dateTime2.Value.ToShortDateString();
			}
		});
		return StiDataFilterRuleHelper.GetDataTableFilterQuery(list, null, null, null);
	}

	public void RefreshActionButtons()
	{
		StiActionButtonsHelper.RefreshButtonsVisibility(this, Element);
	}

	public void RefreshStatus(bool checkProgressStatus)
	{
		ElementChild?.RefreshStatus(checkProgressStatus);
	}

	private void RefreshChildLocation()
	{
		if (((Control)this).Width > 0 && ((Control)this).Height > 0)
		{
			Rectangle rectangle = StiElementControlPainter.GetContentRect(rect: new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height), element: Element);
			Child.SetBounds(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
		}
	}

	public void RemoveFilter()
	{
		ElementChild?.RemoveFilter();
	}

	public void SwitchFilterMultiOption()
	{
		ElementChild?.SwitchFilterMultiOption();
	}

	public StiHolderElementControl(IStiElement element, Control child, bool showActionButtons = true)
	{
		((Control)this).SetStyle((ControlStyles)131072, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		Element = element;
		Child = child;
		((Control)this).Controls.Add(Child);
		if (showActionButtons)
		{
			StiActionButtonsHelper.CreateActionButtons(this, element);
		}
		RefreshChildLocation();
	}
}
