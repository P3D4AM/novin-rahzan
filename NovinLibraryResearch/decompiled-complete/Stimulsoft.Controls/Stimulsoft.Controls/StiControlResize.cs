using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
internal class StiControlResize : Control
{
	private static StiControlResize ResizeControl;

	private static Rectangle startResizeRectangle;

	private static Rectangle lastResizeRectangle;

	private static StiDockingPanel dockingPanel;

	private static int minSize = 66;

	private static int maxSize = 400;

	private static Point lastResizePos;

	protected override void OnPaint(PaintEventArgs e)
	{
		e.Graphics.FillRectangle(SystemBrushes.ControlDark, ((Control)ResizeControl).ClientRectangle);
	}

	public static void StartResize(StiDockingPanel panel, Rectangle rect, int minimumSize, int maximumSize)
	{
		minSize = minimumSize;
		maxSize = maximumSize;
		dockingPanel = panel;
		startResizeRectangle = (lastResizeRectangle = rect);
		if (ResizeControl != null)
		{
			((Component)(object)ResizeControl).Dispose();
			ResizeControl = null;
		}
		ResizeControl = new StiControlResize();
		((Control)ResizeControl).Bounds = rect;
		((Control)dockingPanel).Parent.Controls.Add((Control)(object)ResizeControl);
		((Control)dockingPanel).Parent.Controls.SetChildIndex((Control)(object)ResizeControl, 0);
		dockingPanel.IsResizing = true;
		lastResizePos = Cursor.Position;
	}

	public static void RefreshResize()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Invalid comparison between Unknown and I4
		DockStyle dock = ((Control)dockingPanel).Dock;
		if (dock - 1 > 1)
		{
			if (dock - 3 <= 1)
			{
				Rectangle bounds = lastResizeRectangle;
				bounds.Offset(Cursor.Position.X - lastResizePos.X, 0);
				lastResizeRectangle = bounds;
				bounds.X = Math.Max(minSize, bounds.X);
				bounds.X = Math.Min(maxSize, bounds.X);
				((Control)ResizeControl).Bounds = bounds;
			}
		}
		else
		{
			Rectangle bounds = lastResizeRectangle;
			bounds.Offset(0, Cursor.Position.Y - lastResizePos.Y);
			lastResizeRectangle = bounds;
			bounds.Y = Math.Max(minSize, bounds.Y);
			bounds.Y = Math.Min(maxSize, bounds.Y);
			((Control)ResizeControl).Bounds = bounds;
		}
		lastResizePos = Cursor.Position;
	}

	public static void StopResize()
	{
		StopResize(cancel: false);
	}

	public static void StopResize(bool cancel)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected I4, but got Unknown
		if (!cancel)
		{
			Rectangle bounds = ((Control)ResizeControl).Bounds;
			DockStyle dock = ((Control)dockingPanel).Dock;
			switch (dock - 1)
			{
			case 2:
			{
				StiDockingPanel stiDockingPanel4 = dockingPanel;
				((Control)stiDockingPanel4).Width = ((Control)stiDockingPanel4).Width + (bounds.Left - startResizeRectangle.Left);
				break;
			}
			case 3:
			{
				StiDockingPanel stiDockingPanel3 = dockingPanel;
				((Control)stiDockingPanel3).Width = ((Control)stiDockingPanel3).Width - (bounds.Left - startResizeRectangle.Left);
				break;
			}
			case 0:
			{
				StiDockingPanel stiDockingPanel2 = dockingPanel;
				((Control)stiDockingPanel2).Height = ((Control)stiDockingPanel2).Height + (bounds.Top - startResizeRectangle.Top);
				break;
			}
			case 1:
			{
				StiDockingPanel stiDockingPanel = dockingPanel;
				((Control)stiDockingPanel).Height = ((Control)stiDockingPanel).Height - (bounds.Top - startResizeRectangle.Top);
				break;
			}
			}
		}
		((Control)dockingPanel).Parent.Controls.Remove((Control)(object)ResizeControl);
		((Component)(object)ResizeControl).Dispose();
		ResizeControl = null;
		dockingPanel.IsResizing = false;
	}

	public StiControlResize()
	{
		((Control)this).SetStyle((ControlStyles)65536, true);
	}
}
