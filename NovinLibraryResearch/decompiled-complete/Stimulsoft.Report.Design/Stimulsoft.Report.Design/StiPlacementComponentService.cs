using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Controls;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Table;
using Stimulsoft.Report.Design.Wizards;

namespace Stimulsoft.Report.Design;

internal class StiPlacementComponentService
{
	private StiDesignerControl designer;

	private StiBand container;

	private StiComponent component;

	private StiComponent labelComponent;

	private StiPlacementComponentControl specialDockControl;

	private StiPopupForm popupWindow;

	public bool IsOpen { get; private set; }

	public void Activate(StiComponent comp, StiComponent draggingLabelComponent)
	{
		labelComponent = draggingLabelComponent;
		component = comp;
		container = comp.Parent as StiBand;
		if (container is StiTable || comp is StiContainer || comp is StiPrimitive)
		{
			return;
		}
		popupWindow = new StiPopupForm
		{
			IsDrawShadowInPopupWindow = false
		};
		popupWindow.ClosePopup += PopupWindow_ClosePopup;
		specialDockControl = new StiPlacementComponentControl();
		specialDockControl.DockStateChanged += SpecialDockControl_DockStateChanged;
		specialDockControl.PreChangeDockState += SpecialDockControl_PreChangeDockState;
		((Control)specialDockControl.btClose).Click += ButtonClose_Click;
		specialDockControl.DockStyle = StiPlacementComponentDockStyle.None;
		((Control)popupWindow).Width = ((Control)specialDockControl).Width;
		((Control)popupWindow).Height = ((Control)specialDockControl).Height;
		((Control)popupWindow).Controls.Add((Control)(object)specialDockControl);
		StiPage page = comp.Page;
		RectangleD paintRectangle = comp.GetPaintRectangle();
		Rectangle rectangle = ((Control)designer.PageView.GetCurrentPageView().View).RectangleToScreen(new Rectangle((int)paintRectangle.X, (int)paintRectangle.Y, (int)paintRectangle.Width, (int)paintRectangle.Height));
		int num = StiScaleUI.I((int)(page.Unit.ConvertToHInches(page.Margins.Top) * page.Zoom));
		int num2 = StiScaleUI.I((int)(page.Unit.ConvertToHInches(page.Margins.Left) * page.Zoom));
		int x;
		int y;
		if ((double)(rectangle.Y + StiScaleUI.I(44)) + paintRectangle.Height > (double)Screen.PrimaryScreen.WorkingArea.Height)
		{
			if (rectangle.Y - ((Control)popupWindow).Height - StiScaleUI.I5 < 0)
			{
				x = Cursor.Position.X;
				y = Cursor.Position.Y;
			}
			else
			{
				x = rectangle.X + (int)((paintRectangle.Width - (double)((Control)popupWindow).Width) / 2.0) + num2;
				y = rectangle.Y - StiScaleUI.I(44) + num;
			}
		}
		else
		{
			x = rectangle.X + (int)((paintRectangle.Width - (double)((Control)popupWindow).Width) / 2.0) + num2;
			y = rectangle.Y + ((int)paintRectangle.Height + StiScaleUI.I5 + num);
		}
		popupWindow.SetLocation(x, y, ((Control)specialDockControl).Width, ((Control)specialDockControl).Height);
		popupWindow.ShowPopupForm();
		IsOpen = true;
	}

	public void ClosePopup()
	{
		popupWindow.ClosePopupForm();
	}

	private RectangleD CalculateHorizontalFreeSpace(RectangleD? rect)
	{
		if (component == null || container == null)
		{
			return RectangleD.Empty;
		}
		RectangleD rect2;
		if (!rect.HasValue)
		{
			rect2 = new RectangleD(0.0, 0.0, container.Width, container.Height);
			rect2.Y += component.Top;
			rect2.Height = component.Height;
		}
		else
		{
			rect2 = rect.Value;
		}
		foreach (StiComponent component in container.Components)
		{
			if (component != this.component && component != labelComponent && !(component is StiPrimitive))
			{
				CutHorizontalRegion(ref rect2, component.ClientRectangle);
			}
		}
		return rect2;
	}

	private RectangleD CalculateVerticalFreeSpace(RectangleD? rect)
	{
		if (component == null || container == null)
		{
			return RectangleD.Empty;
		}
		RectangleD rect2;
		if (!rect.HasValue)
		{
			rect2 = new RectangleD(0.0, 0.0, container.Width, container.Height);
			rect2.X += component.Left;
			rect2.Width = component.Width;
		}
		else
		{
			rect2 = rect.Value;
		}
		foreach (StiComponent component in container.Components)
		{
			if (component != this.component && component != labelComponent && !(component is StiPrimitive))
			{
				CutVerticalRegion(ref rect2, component.ClientRectangle);
			}
		}
		return rect2;
	}

	private void CutHorizontalRegion(ref RectangleD rect, RectangleD cutRect)
	{
		if (!rect.IntersectsWith(cutRect) || !((cutRect.Left < rect.Right) & (cutRect.Right >= rect.Left)))
		{
			return;
		}
		double num = ((cutRect.Left < rect.Left) ? rect.Left : cutRect.Left);
		double num2 = ((cutRect.Right > rect.Right) ? rect.Right : cutRect.Right);
		if (num - rect.Left > rect.Right - num2)
		{
			double num3 = num - rect.Left;
			if (num3 < 0.0)
			{
				num3 = 0.0;
			}
			rect = new RectangleD(rect.X, rect.Y, num3, rect.Height);
		}
		else
		{
			double num3 = rect.Right - num2;
			if (num3 < 0.0)
			{
				num3 = 0.0;
			}
			rect = new RectangleD(num2, rect.Y, num3, rect.Height);
		}
	}

	private void CutVerticalRegion(ref RectangleD rect, RectangleD cutRect)
	{
		if (rect.IntersectsWith(cutRect))
		{
			RectangleD rectangleD = RectangleD.Intersect(rect, cutRect);
			if (rectangleD.Top <= rect.Top && rectangleD.Bottom >= rect.Top)
			{
				rect = ((rectangleD.Bottom > rect.Bottom) ? RectangleD.Empty : new RectangleD(rect.X, rectangleD.Bottom, rect.Width, rect.Bottom - rectangleD.Bottom));
			}
			else
			{
				rect = new RectangleD(rect.X, rect.Y, rect.Width, rectangleD.Top - rect.Top);
			}
		}
	}

	private RectangleD SetMoveLeftFreeSpace(bool isApplySize)
	{
		RectangleD rectangleD = CalculateHorizontalFreeSpace(null);
		if (!(rectangleD.Width > 0.0) || !(rectangleD.Height > 0.0))
		{
			return RectangleD.Empty;
		}
		rectangleD = CalculateVerticalFreeSpace(new RectangleD(rectangleD.Left, 0.0, component.Width, container.Height));
		double num = ((rectangleD.Width < component.Width) ? rectangleD.Width : component.Width);
		if (isApplySize)
		{
			if (labelComponent == null)
			{
				component.ClientRectangle = new RectangleD(rectangleD.Left, rectangleD.Top, num, rectangleD.Height);
			}
			else
			{
				labelComponent.ClientRectangle = new RectangleD(rectangleD.Left, rectangleD.Top, num / 2.0, rectangleD.Height);
				component.ClientRectangle = new RectangleD(rectangleD.Left + num / 2.0, rectangleD.Top, num / 2.0, rectangleD.Height);
			}
			return RectangleD.Empty;
		}
		return new RectangleD(rectangleD.Left, rectangleD.Top, num, rectangleD.Height);
	}

	private RectangleD SetMoveRightFreeSpace(bool isApplySize)
	{
		RectangleD rectangleD = CalculateHorizontalFreeSpace(null);
		if (!(rectangleD.Width > 0.0) || !(rectangleD.Height > 0.0))
		{
			return RectangleD.Empty;
		}
		rectangleD = CalculateVerticalFreeSpace(new RectangleD(rectangleD.Right - component.Width, 0.0, component.Width, container.Height));
		double num = ((rectangleD.Width < component.Width) ? rectangleD.Width : component.Width);
		if (isApplySize)
		{
			if (labelComponent == null)
			{
				component.ClientRectangle = new RectangleD(rectangleD.Right - num, rectangleD.Top, num, rectangleD.Height);
			}
			else
			{
				labelComponent.ClientRectangle = new RectangleD(rectangleD.Right - num, rectangleD.Top, num / 2.0, rectangleD.Height);
				component.ClientRectangle = new RectangleD(rectangleD.Right - num / 2.0, rectangleD.Top, num / 2.0, rectangleD.Height);
			}
			return RectangleD.Empty;
		}
		return new RectangleD(rectangleD.Right - num, rectangleD.Top, num, rectangleD.Height);
	}

	private void PopupWindow_ClosePopup(object sender, EventArgs e)
	{
		IsOpen = false;
		popupWindow.ClosePopup -= PopupWindow_ClosePopup;
		ClearPlacementMarks();
	}

	private void ClearPlacementMarks()
	{
		(from c in designer.Report.GetComponents().ToList()
			where c is StiBand
			select c).Cast<StiBand>().ToList().ForEach(delegate(StiBand c)
		{
			c.RectangleMoveComponent = RectangleD.Empty;
		});
	}

	private void ButtonClose_Click(object sender, EventArgs e)
	{
		IsOpen = false;
		popupWindow.ClosePopup -= PopupWindow_ClosePopup;
		popupWindow.ClosePopupForm();
		ClearPlacementMarks();
	}

	private void SpecialDockControl_PreChangeDockState(object sender, StiPreChangePlacementComponentStateEventArgs e)
	{
		switch (e.DockStyle)
		{
		case StiPlacementComponentDockStyle.MoveLeftFreeSpace:
			container.RectangleMoveComponent = SetMoveLeftFreeSpace(isApplySize: false);
			break;
		case StiPlacementComponentDockStyle.MoveRightFreeSpace:
			container.RectangleMoveComponent = SetMoveRightFreeSpace(isApplySize: false);
			break;
		case StiPlacementComponentDockStyle.None:
			container.RectangleMoveComponent = null;
			break;
		}
		((Control)designer).Refresh();
	}

	private void SpecialDockControl_DockStateChanged(object sender, EventArgs e)
	{
		if (component != null)
		{
			switch (specialDockControl.DockStyle)
			{
			case StiPlacementComponentDockStyle.MoveLeftFreeSpace:
				SetMoveLeftFreeSpace(isApplySize: true);
				break;
			case StiPlacementComponentDockStyle.MoveRightFreeSpace:
				SetMoveRightFreeSpace(isApplySize: true);
				break;
			}
		}
		if (popupWindow != null)
		{
			popupWindow.ClosePopup -= PopupWindow_ClosePopup;
			IsOpen = false;
			popupWindow.ClosePopupForm();
			popupWindow = null;
		}
		if (specialDockControl != null)
		{
			specialDockControl.DockStateChanged -= SpecialDockControl_DockStateChanged;
			specialDockControl.PreChangeDockState -= SpecialDockControl_PreChangeDockState;
			specialDockControl = null;
		}
		ClearPlacementMarks();
		((Control)designer).Refresh();
	}

	public StiPlacementComponentService(StiDesignerControl designer)
	{
		this.designer = designer;
	}
}
