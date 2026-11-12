using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
internal class KeyTipsCanvasControl : Control
{
	private Control m_ParentControl;

	private IKeyTipsRenderer m_Renderer;

	protected override CreateParams CreateParams
	{
		get
		{
			CreateParams createParams = ((Control)this).CreateParams;
			createParams.ExStyle |= 0x20;
			return createParams;
		}
	}

	public KeyTipsCanvasControl(IKeyTipsRenderer renderer)
	{
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)4, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)2048, true);
		((Control)this).SetStyle((ControlStyles)1, false);
		((Control)this).SetStyle((ControlStyles)512, false);
		((Control)this).BackColor = Color.Transparent;
		m_Renderer = renderer;
	}

	protected override void Dispose(bool disposing)
	{
		if (m_ParentControl != null)
		{
			m_ParentControl.Resize -= ParentControlResize;
			m_ParentControl = null;
		}
		((Control)this).Dispose(disposing);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		if (m_Renderer != null)
		{
			m_Renderer.PaintKeyTips(e.Graphics);
		}
		((Control)this).OnPaint(e);
	}

	protected override void OnParentChanged(EventArgs e)
	{
		if (m_ParentControl != null)
		{
			m_ParentControl.Resize -= ParentControlResize;
		}
		m_ParentControl = ((Control)this).Parent;
		if (m_ParentControl != null)
		{
			m_ParentControl.Resize += ParentControlResize;
		}
		((Control)this).OnParentChanged(e);
	}

	protected override void OnPaintBackground(PaintEventArgs e)
	{
	}

	private void ParentControlResize(object sender, EventArgs e)
	{
		if (m_ParentControl != null)
		{
			((Control)this).Bounds = new Rectangle(0, 0, m_ParentControl.Width, m_ParentControl.Height);
		}
	}

	protected override void WndProc(ref Message m)
	{
		if (((Message)(ref m)).Msg == 132)
		{
			((Message)(ref m)).Result = new IntPtr(-1);
		}
		else
		{
			((Control)this).WndProc(ref m);
		}
	}
}
