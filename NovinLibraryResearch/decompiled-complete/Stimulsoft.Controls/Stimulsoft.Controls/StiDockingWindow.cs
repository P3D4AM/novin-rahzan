using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

internal class StiDockingWindow : Form
{
	private Container components;

	internal static StiDockingWindow DockingWindow;

	internal static StiDockingPanel panel;

	internal static Control ControlToDock;

	internal static DockStyle DockType;

	internal StiDockingWindow()
	{
		InitializeComponent();
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)0;
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)65536, true);
		((Control)this).SetStyle((ControlStyles)2, true);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Form)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		((Form)this).AutoScaleBaseSize = new Size(5, 13);
		((Form)this).ClientSize = new Size(296, 240);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Control)this).Name = "StiDockingWindow";
		((Form)this).Opacity = 0.5;
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "StiDockWindow";
	}

	protected override void OnPaintBackground(PaintEventArgs e)
	{
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		((Form)this).OnPaint(e);
		Graphics graphics = e.Graphics;
		Rectangle clipRectangle = e.ClipRectangle;
		clipRectangle.Width--;
		clipRectangle.Height--;
		graphics.DrawRectangle(SystemPens.ControlDark, clipRectangle);
		clipRectangle.Inflate(-1, -1);
		graphics.DrawRectangle(SystemPens.ControlDark, clipRectangle);
		clipRectangle.Inflate(-1, -1);
		clipRectangle.Width++;
		clipRectangle.Height++;
		graphics.FillRectangle(SystemBrushes.ActiveCaption, clipRectangle);
	}

	private static Form GetForm()
	{
		if (panel.LastDockForm != null)
		{
			return panel.LastDockForm;
		}
		return ((Control)panel).FindForm();
	}

	private static Rectangle GetDockRectangle(Form form)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected I4, but got Unknown
		Rectangle clientRectangle = ((Control)form).ClientRectangle;
		for (int num = ((ArrangedElementCollection)((Control)form).Controls).Count - 1; num >= 0; num--)
		{
			Control val = ((Control)form).Controls[num];
			if (val.Visible)
			{
				DockStyle dock = val.Dock;
				switch (dock - 1)
				{
				case 2:
					clientRectangle.X += val.Width;
					clientRectangle.Width -= val.Width;
					break;
				case 3:
					clientRectangle.Width -= val.Width;
					break;
				case 0:
					clientRectangle.Y += val.Height;
					clientRectangle.Height -= val.Height;
					break;
				case 1:
					clientRectangle.Height -= val.Height;
					break;
				case 4:
					return ((Control)form).RectangleToScreen(clientRectangle);
				}
			}
		}
		return ((Control)form).RectangleToScreen(clientRectangle);
	}

	private static Rectangle GetWindowDockRectangle(Rectangle rect)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected I4, but got Unknown
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		Form form = GetForm();
		Rectangle dockRectangle = GetDockRectangle(form);
		Point position = Cursor.Position;
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		foreach (Control item in (ArrangedElementCollection)((Control)form).Controls)
		{
			Control val = item;
			if (val is StiDockingPanel)
			{
				DockStyle dock = val.Dock;
				switch (dock - 1)
				{
				case 2:
					flag = true;
					break;
				case 3:
					flag2 = true;
					break;
				case 0:
					flag3 = true;
					break;
				case 1:
					flag4 = true;
					break;
				}
			}
		}
		ControlToDock = (Control)(object)form;
		if (!flag3 && new Rectangle(dockRectangle.Left, dockRectangle.Top, dockRectangle.Width, 20).Contains(position))
		{
			DockType = (DockStyle)1;
			return new Rectangle(dockRectangle.X, dockRectangle.Y, dockRectangle.Width, 150);
		}
		if (!flag4 && new Rectangle(dockRectangle.Left, dockRectangle.Bottom - 20, dockRectangle.Width, 20).Contains(position))
		{
			DockType = (DockStyle)2;
			return new Rectangle(dockRectangle.X, dockRectangle.Bottom - 150, dockRectangle.Width, 150);
		}
		if (!flag && new Rectangle(dockRectangle.Left, dockRectangle.Top, 20, dockRectangle.Height).Contains(position))
		{
			DockType = (DockStyle)3;
			return new Rectangle(dockRectangle.X, dockRectangle.Y, 200, dockRectangle.Height);
		}
		if (!flag2 && new Rectangle(dockRectangle.Right - 20, dockRectangle.Top, 20, dockRectangle.Height).Contains(position))
		{
			DockType = (DockStyle)4;
			return new Rectangle(dockRectangle.Right - 200, dockRectangle.Y, 200, dockRectangle.Height);
		}
		return Rectangle.Empty;
	}

	private static Rectangle GetPanelDockRectangle(StiDockingPanel dockPanel, Rectangle rect)
	{
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Expected I4, but got Unknown
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		Point position = Cursor.Position;
		ControlToDock = (Control)(object)dockPanel;
		if (dockPanel.Collapsed)
		{
			Rectangle result = ((Control)dockPanel).RectangleToScreen(((Control)dockPanel).ClientRectangle);
			if (result.Contains(position))
			{
				return result;
			}
		}
		Rectangle result2 = ((Control)dockPanel).RectangleToScreen(dockPanel.PanelBounds);
		if (!result2.Contains(position))
		{
			return Rectangle.Empty;
		}
		if (dockPanel == panel)
		{
			return Rectangle.Empty;
		}
		int num = 0;
		if (((ArrangedElementCollection)((Control)dockPanel).Controls).Count > 1)
		{
			num = 24;
		}
		if (new Rectangle(result2.Left, result2.Top, result2.Width, 20).Contains(position))
		{
			DockType = (DockStyle)5;
			return result2;
		}
		if (num != 0 && new Rectangle(result2.Left, result2.Bottom - 24, result2.Width, 24).Contains(position))
		{
			DockType = (DockStyle)5;
			return result2;
		}
		DockStyle dock = ((Control)dockPanel).Dock;
		switch (dock - 2)
		{
		case 1:
			if (new Rectangle(result2.Left, result2.Top, 20, result2.Height).Contains(position))
			{
				DockType = (DockStyle)5;
				return result2;
			}
			break;
		case 2:
			if (new Rectangle(result2.Right - 20, result2.Top, 20, result2.Height).Contains(position))
			{
				DockType = (DockStyle)5;
				return result2;
			}
			break;
		case 0:
			if (new Rectangle(result2.Left, result2.Bottom - 20, result2.Width, 20).Contains(position))
			{
				DockType = (DockStyle)5;
				return result2;
			}
			break;
		}
		return Rectangle.Empty;
	}

	private static Rectangle GetPanelsDockRectangle(Rectangle rect)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		Form form = GetForm();
		foreach (Control item in (ArrangedElementCollection)((Control)form).Controls)
		{
			Control val = item;
			if (val is StiDockingPanel dockPanel)
			{
				Rectangle panelDockRectangle = GetPanelDockRectangle(dockPanel, rect);
				if (!panelDockRectangle.IsEmpty)
				{
					return panelDockRectangle;
				}
			}
		}
		foreach (StiDockingPanel undockedPanel in panel.Manager.UndockedPanels)
		{
			Rectangle panelDockRectangle2 = GetPanelDockRectangle(undockedPanel, rect);
			if (!panelDockRectangle2.IsEmpty)
			{
				return panelDockRectangle2;
			}
		}
		return Rectangle.Empty;
	}

	internal static Rectangle GetDockRectangle(Rectangle rect)
	{
		Rectangle panelsDockRectangle = GetPanelsDockRectangle(rect);
		if (!panelsDockRectangle.IsEmpty)
		{
			return panelsDockRectangle;
		}
		panelsDockRectangle = GetWindowDockRectangle(rect);
		if (!panelsDockRectangle.IsEmpty)
		{
			return panelsDockRectangle;
		}
		return rect;
	}

	internal static void RefreshDockingWindow(Rectangle rect)
	{
		((Control)DockingWindow).Bounds = GetDockRectangle(rect);
	}

	internal static void StopDockingWindow()
	{
		if (DockingWindow != null)
		{
			((Form)DockingWindow).Close();
			((Component)(object)DockingWindow).Dispose();
			DockingWindow = null;
		}
	}

	internal static void StartDockingWindow(StiDockingPanel pn, Rectangle rect)
	{
		panel = pn;
		DockingWindow = new StiDockingWindow();
		Win32.SetWindowPos(((Control)DockingWindow).Handle, new IntPtr(-1), 0, 0, rect.Width, rect.Height, 83);
	}
}
