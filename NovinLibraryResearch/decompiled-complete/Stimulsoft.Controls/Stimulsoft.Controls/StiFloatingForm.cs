using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

public class StiFloatingForm : Form
{
	private StiDockingPanel Panel;

	protected override void Dispose(bool disposing)
	{
		((Form)this).Dispose(disposing);
	}

	public StiFloatingForm(StiDockingPanel panel)
	{
		((Control)this).SetStyle((ControlStyles)16, true);
		InitializeComponent();
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)0;
		((ScrollableControl)this).DockPadding.All = 4;
		Panel = panel;
	}

	private void InitializeComponent()
	{
		((Form)this).AutoScaleBaseSize = new Size(5, 13);
		((Form)this).ClientSize = new Size(228, 294);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Control)this).MinimumSize = new Size(100, 55);
		((Control)this).Name = "StiFloatingForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)0;
		((Control)this).Text = "Floating Form";
		((Control)this).SizeChanged += StiFloatingForm_SizeChanged;
	}

	protected override void OnActivated(EventArgs e)
	{
		((Form)this).OnActivated(e);
		Panel.InvalidateTitle();
	}

	protected override void OnDeactivate(EventArgs e)
	{
		((Form)this).OnDeactivate(e);
		Panel.InvalidateTitle();
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		((Form)this).OnPaint(e);
		ControlPaint.DrawBorder3D(e.Graphics, 0, 0, ((Control)this).Width, ((Control)this).Height, (Border3DStyle)5, (Border3DSide)2063);
	}

	protected override void OnControlAdded(ControlEventArgs e)
	{
		((Control)this).OnControlAdded(e);
		e.Control.Dock = (DockStyle)5;
		((StiDockingPanel)(object)e.Control).FloatingForm = this;
	}

	protected override void OnControlRemoved(ControlEventArgs e)
	{
		((Control)this).OnControlRemoved(e);
		((StiDockingPanel)(object)e.Control).FloatingForm = null;
		if (((ArrangedElementCollection)((Control)this).Controls).Count == 0)
		{
			((Form)this).Close();
			((Component)this).Dispose();
		}
	}

	protected override void WndProc(ref Message m)
	{
		if (((Message)(ref m)).Msg == 132)
		{
			((Message)(ref m)).Result = new IntPtr((int)GetNCHitTest(new Point(((Message)(ref m)).LParam.ToInt32())));
		}
		else
		{
			((Form)this).WndProc(ref m);
		}
	}

	private Win32.MousePosition GetNCHitTest(Point p)
	{
		Size frameBorderSize = SystemInformation.FrameBorderSize;
		p.Offset(-((Control)this).Left, -((Control)this).Top);
		if ((p.X <= 20 && p.Y <= frameBorderSize.Height) || (p.Y <= 20 && p.X <= frameBorderSize.Height))
		{
			return Win32.MousePosition.HTTOPLEFT;
		}
		if ((p.X >= ((Control)this).Width - 20 && p.Y <= frameBorderSize.Height) || (p.Y <= 20 && p.X >= ((Control)this).Width - frameBorderSize.Height))
		{
			return Win32.MousePosition.HTTOPRIGHT;
		}
		if ((p.X <= 20 && p.Y >= ((Control)this).Height - frameBorderSize.Height) || (p.Y >= ((Control)this).Height - 20 && p.X <= frameBorderSize.Height))
		{
			return Win32.MousePosition.HTBOTTOMLEFT;
		}
		if ((p.X >= ((Control)this).Width - 20 && p.Y >= ((Control)this).Height - frameBorderSize.Height) || (p.Y >= ((Control)this).Height - 20 && p.X >= ((Control)this).Width - frameBorderSize.Height))
		{
			return Win32.MousePosition.HTBOTTOMRIGHT;
		}
		if (p.X <= frameBorderSize.Height)
		{
			return Win32.MousePosition.HTLEFT;
		}
		if (p.Y <= frameBorderSize.Height)
		{
			return Win32.MousePosition.HTTOP;
		}
		if (p.X >= ((Control)this).Width - frameBorderSize.Height)
		{
			return Win32.MousePosition.HTRIGHT;
		}
		if (p.Y >= ((Control)this).Height - frameBorderSize.Height)
		{
			return Win32.MousePosition.HTBOTTOM;
		}
		return Win32.MousePosition.HTCLIENT;
	}

	private void StiFloatingForm_SizeChanged(object sender, EventArgs e)
	{
		Panel.SetLayout();
		((Control)Panel).Invalidate();
	}
}
