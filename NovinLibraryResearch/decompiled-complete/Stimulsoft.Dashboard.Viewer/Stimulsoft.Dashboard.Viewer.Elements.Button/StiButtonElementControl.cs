using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.Button;
using Stimulsoft.Dashboard.Drawing.Painters;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Dashboard.Viewer.Elements.Button;

[ToolboxItem(false)]
public class StiButtonElementControl : Button, IStiRefreshControl, IStiElementControl
{
	private bool isPressed;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsMouseOver => ((Control)this).ClientRectangle.Contains(((Control)this).PointToClient(Cursor.Position));

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsPressed
	{
		get
		{
			return isPressed;
		}
		set
		{
			if (isPressed != value)
			{
				isPressed = value;
				((Control)this).Invalidate();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiButtonElement ButtonElement => Element as StiButtonElement;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public IStiElement Element { get; set; }

	public void RefreshControl(object sender, StiReportControlUpdateEventArgs e)
	{
		if (sender == Element)
		{
			((Control)this).Invalidate();
		}
	}

	protected override void OnMouseDown(MouseEventArgs mevent)
	{
		((ButtonBase)this).OnMouseDown(mevent);
		IsPressed = true;
	}

	protected override void OnMouseUp(MouseEventArgs mevent)
	{
		((Button)this).OnMouseUp(mevent);
		IsPressed = false;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		((ButtonBase)this).OnPaint(e);
		Graphics graphics = e.Graphics;
		RectangleD rect = new RectangleD(0.0, 0.0, ((Control)this).Width - 1, ((Control)this).Height - 1);
		StiButtonVisualState visualState = GetVisualState();
		StiButtonElementGdiPainter.PaintButton(graphics, rect, Element, visualState, isChecked: false);
	}

	protected override void OnPaintBackground(PaintEventArgs e)
	{
	}

	protected override void OnClick(EventArgs e)
	{
		((Button)this).OnClick(e);
		try
		{
			StiDashboardViewerHelper.EnableRefreshViewerProcessing(StiDashboardViewerHelper.FindViewer((Control)(object)this));
			ButtonElement?.InvokeClick(ButtonElement, e);
		}
		finally
		{
			StiDashboardViewerHelper.DisableRefreshViewerProcessing(StiDashboardViewerHelper.FindViewer((Control)(object)this));
		}
	}

	private StiButtonVisualState GetVisualState()
	{
		if (IsPressed)
		{
			return StiButtonVisualStatesHelper.GetPressed(ButtonElement);
		}
		if (IsMouseOver)
		{
			return StiButtonVisualStatesHelper.GetHovered(ButtonElement);
		}
		return StiButtonVisualStatesHelper.GetDefault(ButtonElement);
	}

	public StiButtonElementControl(IStiElement element = null)
	{
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)131072, true);
		Element = element;
		((Control)this).BackColor = Color.Transparent;
		((ButtonBase)this).FlatStyle = (FlatStyle)0;
		((ButtonBase)this).FlatAppearance.BorderSize = 0;
		((ButtonBase)this).FlatAppearance.MouseDownBackColor = Color.Transparent;
		((ButtonBase)this).FlatAppearance.CheckedBackColor = Color.Transparent;
		((ButtonBase)this).FlatAppearance.MouseOverBackColor = Color.Transparent;
	}
}
