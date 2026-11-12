using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
[ComVisible(false)]
public class PopupContainer : Control
{
	private const int WM_MOUSEACTIVATE = 33;

	private const int MA_NOACTIVATE = 3;

	private const int MA_NOACTIVATEANDEAT = 4;

	private const uint WS_POPUP = 2147483648u;

	private const uint WS_CLIPSIBLINGS = 67108864u;

	private const uint WS_CLIPCHILDREN = 33554432u;

	private const uint WS_EX_TOPMOST = 8u;

	private const uint WS_EX_TOOLWINDOW = 128u;

	private PopupShadow m_DropShadow;

	private bool m_ShowDropShadow;

	private const int SHADOW_OFFSET = 5;

	private MenuPanel HostedMenu
	{
		get
		{
			if (((ArrangedElementCollection)((Control)this).Controls).Count > 0 && ((Control)this).Controls[0] is MenuPanel)
			{
				return ((Control)this).Controls[0] as MenuPanel;
			}
			return null;
		}
	}

	private Bar HostedBar
	{
		get
		{
			if (((ArrangedElementCollection)((Control)this).Controls).Count > 0 && ((Control)this).Controls[0] is Bar)
			{
				return ((Control)this).Controls[0] as Bar;
			}
			return null;
		}
	}

	protected override CreateParams CreateParams
	{
		get
		{
			CreateParams createParams = ((Control)this).CreateParams;
			createParams.Style = -2046820352;
			createParams.ExStyle = 136;
			createParams.Caption = "";
			return createParams;
		}
	}

	public bool ShowDropShadow
	{
		get
		{
			return m_ShowDropShadow;
		}
		set
		{
			m_ShowDropShadow = value;
		}
	}

	public PopupContainer()
	{
		((Control)this).IsAccessible = false;
		((Control)this).SetStyle((ControlStyles)512, false);
	}

	protected override void Dispose(bool disposing)
	{
		if (m_DropShadow != null)
		{
			((Control)m_DropShadow).Hide();
			((Component)(object)m_DropShadow).Dispose();
			m_DropShadow = null;
		}
		((Control)this).Dispose(disposing);
	}

	protected override void OnVisibleChanged(EventArgs e)
	{
		((Control)this).OnVisibleChanged(e);
		if (!((Control)this).Visible && m_DropShadow != null)
		{
			((Control)m_DropShadow).Hide();
			((Component)(object)m_DropShadow).Dispose();
			m_DropShadow = null;
		}
	}

	protected override void WndProc(ref Message m)
	{
		if (((Message)(ref m)).Msg == 33)
		{
			((Message)(ref m)).Result = new IntPtr(3);
		}
		else
		{
			((Control)this).WndProc(ref m);
		}
	}

	public void ShowShadow()
	{
		if (m_ShowDropShadow)
		{
			if (m_DropShadow != null)
			{
				((Control)m_DropShadow).Hide();
				((Component)(object)m_DropShadow).Dispose();
				m_DropShadow = null;
			}
			if (((Control)this).Width > 5 && ((Control)this).Height > 5)
			{
				m_DropShadow = new PopupShadow(bAlphaShadow: true);
				((Control)m_DropShadow).CreateControl();
				NativeFunctions.SetWindowPos(((Control)m_DropShadow).Handle, new IntPtr(-2), ((Control)this).Left + 5, ((Control)this).Top + 5, ((Control)this).Width - 2, ((Control)this).Height - 2, 80);
				m_DropShadow.UpdateShadow();
			}
		}
	}

	protected override void OnResize(EventArgs e)
	{
		if (m_DropShadow != null)
		{
			NativeFunctions.SetWindowPos(((Control)m_DropShadow).Handle, new IntPtr(-2), ((Control)this).Left + 5, ((Control)this).Top + 5, ((Control)this).Width - 2, ((Control)this).Height - 2, 80);
			m_DropShadow.UpdateShadow();
		}
		((Control)this).OnResize(e);
	}
}
