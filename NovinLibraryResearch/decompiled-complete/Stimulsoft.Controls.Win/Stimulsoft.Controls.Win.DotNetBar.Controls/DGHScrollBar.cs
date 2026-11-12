using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

[ToolboxItem(false)]
internal class DGHScrollBar : HScrollBar, ScrollBarReplacement.IScrollBarExtender
{
	private ScrollBarReplacement m_ScrollBarImpl;

	private bool m_EnableStyling = true;

	protected override CreateParams CreateParams
	{
		get
		{
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Invalid comparison between Unknown and I4
			CreateParams createParams = ((HScrollBar)this).CreateParams;
			if (m_EnableStyling)
			{
				createParams.ClassName = null;
				createParams.Style = 33554432;
				createParams.Style |= 0x44000000;
				if (((Control)this).Visible)
				{
					createParams.Style |= 0x10000000;
				}
				if (!((Control)this).Enabled)
				{
					createParams.Style |= 0x8000000;
				}
				if ((int)((Control)this).RightToLeft == 1)
				{
					createParams.ExStyle |= 0x2000;
					createParams.ExStyle |= 0x1000;
					createParams.ExStyle |= 0x4000;
				}
			}
			return createParams;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether custom styling (Office 2007 style) is enabled.")]
	public bool EnableStyling
	{
		get
		{
			return m_EnableStyling;
		}
		set
		{
			if (m_EnableStyling != value)
			{
				m_EnableStyling = value;
				if (((Control)this).IsHandleCreated)
				{
					((Control)this).RecreateHandle();
				}
			}
		}
	}

	public bool AppStyleScrollBar
	{
		get
		{
			return m_ScrollBarImpl.AppStyleScrollBar;
		}
		set
		{
			m_ScrollBarImpl.AppStyleScrollBar = value;
		}
	}

	public DGHScrollBar()
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		m_ScrollBarImpl = new ScrollBarReplacement((ScrollBar)(object)this);
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)4, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle(DisplayHelp.DoubleBufferFlag, true);
		((Control)this).SetStyle((ControlStyles)2048, false);
	}

	protected override void Dispose(bool disposing)
	{
		m_ScrollBarImpl.Dispose();
		((Control)this).Dispose(disposing);
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		m_ScrollBarImpl.OnHandleCreated();
		((ScrollBar)this).OnHandleCreated(e);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		if (m_EnableStyling)
		{
			m_ScrollBarImpl.OnMouseDown(e);
		}
		((Control)this).OnMouseDown(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		if (m_EnableStyling)
		{
			m_ScrollBarImpl.OnMouseUp(e);
		}
		((Control)this).OnMouseUp(e);
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		if (m_EnableStyling)
		{
			m_ScrollBarImpl.OnMouseEnter(e);
		}
		((Control)this).OnMouseEnter(e);
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		if (m_EnableStyling)
		{
			m_ScrollBarImpl.OnMouseLeave(e);
		}
		((Control)this).OnMouseLeave(e);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		if (m_EnableStyling)
		{
			m_ScrollBarImpl.OnMouseMove(e);
		}
		((Control)this).OnMouseMove(e);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		if (m_EnableStyling)
		{
			m_ScrollBarImpl.OnPaint(e);
		}
		((Control)this).OnPaint(e);
	}

	protected override void NotifyInvalidate(Rectangle invalidatedArea)
	{
		if (m_EnableStyling)
		{
			m_ScrollBarImpl.NotifyInvalidate(invalidatedArea);
		}
		((Control)this).NotifyInvalidate(invalidatedArea);
	}

	public void UpdateScrollValues()
	{
		m_ScrollBarImpl.UpdateScrollValues();
	}

	protected override void OnParentChanged(EventArgs e)
	{
		if (((Control)this).Parent is DataGridView)
		{
			m_ScrollBarImpl.ScrollBarCore.SideBorderOnly = true;
			m_ScrollBarImpl.ScrollBarCore.HasBorder = false;
		}
		else
		{
			m_ScrollBarImpl.ScrollBarCore.SideBorderOnly = false;
			m_ScrollBarImpl.ScrollBarCore.HasBorder = true;
		}
		((Control)this).OnParentChanged(e);
	}

	protected override void OnResize(EventArgs e)
	{
		((Control)this).OnResize(e);
		UpdateScrollValues();
	}

	void ScrollBarReplacement.IScrollBarExtender.SetValue(int newValue, ScrollEventType type)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		if (newValue != ((ScrollBar)this).Value)
		{
			int value = ((ScrollBar)this).Value;
			((ScrollBar)this).Value = newValue;
			ScrollEventArgs val = new ScrollEventArgs(type, value, newValue, (ScrollOrientation)0);
			((ScrollBar)this).OnScroll(val);
		}
	}
}
