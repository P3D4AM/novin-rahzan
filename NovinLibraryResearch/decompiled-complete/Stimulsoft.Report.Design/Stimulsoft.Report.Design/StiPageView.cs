using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design.Controls;

namespace Stimulsoft.Report.Design;

public sealed class StiPageView
{
	public StiDesignerControl Designer { get; private set; }

	public StiComponent DraggingComponent
	{
		get
		{
			return Designer?.Info?.DraggingComponent;
		}
		set
		{
			if (Designer?.Info != null)
			{
				Designer.Info.DraggingComponent = value;
			}
		}
	}

	public StiComponent DraggingLabelComponent
	{
		get
		{
			return Designer?.Info?.DraggingLabelComponent;
		}
		set
		{
			if (Designer?.Info != null)
			{
				Designer.Info.DraggingLabelComponent = value;
			}
		}
	}

	public StiReport Report => Designer.Report;

	public Point StartPos { get; set; }

	public Rectangle SelectedRectangle { get; set; }

	public bool IsDrawSelection { get; set; }

	public bool IsAlignSelection { get; set; } = true;


	public void ResetDragging()
	{
		DraggingComponent = null;
		DraggingLabelComponent = null;
		Designer.PageView.GetCurrentPageView().Page.OffsetRectangle = RectangleD.Empty;
		Designer.SelectTool.Action = StiAction.None;
		Designer.SelectTool.IsDragDropComponent = false;
		Designer.Report.Info.IsComponentsMoving = false;
	}

	public PointD GetCurrentCursorPos()
	{
		if (Designer.TabControl.SelectedTabIndex >= Designer.Report.Pages.Count)
		{
			return PointD.Empty;
		}
		Point point = ((Control)Designer.PageView.GetCurrentPageView().View).PointToClient(Cursor.Position);
		double value = Designer.XToPage(point.X);
		double value2 = Designer.YToPage(point.Y);
		value = StiAlignValue.AlignToGrid(value, Designer.Report.GetCurrentPage().GridSize, Designer.Report.Info.AlignToGrid);
		value2 = StiAlignValue.AlignToGrid(value2, Designer.Report.GetCurrentPage().GridSize, Designer.Report.Info.AlignToGrid);
		return new PointD(value, value2);
	}

	public StiPageViewControl GetCurrentPageView()
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		int selectedTabIndex = Designer.TabControl.SelectedTabIndex;
		if (selectedTabIndex == -1)
		{
			return null;
		}
		ControlCollection controls = Designer.TabControl.Tabs[selectedTabIndex].AttachedControl.Controls;
		foreach (Control item in (ArrangedElementCollection)controls)
		{
			Control val = item;
			if (val is StiPageViewControl)
			{
				return val as StiPageViewControl;
			}
		}
		return null;
	}

	public void Dispose()
	{
		Designer = null;
		DraggingComponent = null;
		DraggingLabelComponent = null;
	}

	public StiPageView(StiDesignerControl designer)
	{
		Designer = designer;
	}
}
