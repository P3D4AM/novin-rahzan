using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
internal class FloatingContainer : Form
{
	private const int WM_MOUSEACTIVATE = 33;

	private const int MA_NOACTIVATE = 3;

	private Bar m_Bar;

	private Size m_OldSize = Size.Empty;

	private bool m_CodeClose;

	public FloatingContainer(Bar objBar)
	{
		m_Bar = objBar;
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).ControlBox = false;
		((Form)this).ShowInTaskbar = false;
		if (objBar.Owner is IOwner owner)
		{
			((Form)this).Owner = owner.ParentForm;
		}
		((Form)this).StartPosition = (FormStartPosition)0;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).SetStyle((ControlStyles)512, false);
		((Control)this).SetStyle((ControlStyles)4, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).Font = ((Control)objBar).Font;
	}

	public void Close()
	{
		m_CodeClose = true;
		((Form)this).Close();
	}

	protected override void OnClosing(CancelEventArgs e)
	{
		if (((Control)this).Visible && !m_CodeClose)
		{
			e.Cancel = true;
			if (m_Bar != null)
			{
				m_Bar.CloseBar();
			}
		}
		((Form)this).OnClosing(e);
	}

	public void RefreshRegion(Size sz)
	{
		if (m_Bar == null || m_OldSize == sz || !m_Bar.IsThemed)
		{
			return;
		}
		m_OldSize = sz;
		ThemeWindow themeWindow = m_Bar.ThemeWindow;
		Graphics val = ((Control)this).CreateGraphics();
		try
		{
			IntPtr themeBackgroundRegion = themeWindow.GetThemeBackgroundRegion(val, ThemeWindowParts.SmallCaption, ThemeWindowStates.CaptionActive, new Rectangle(0, 0, sz.Width, sz.Height));
			if (themeBackgroundRegion != IntPtr.Zero)
			{
				NativeFunctions.SetWindowRgn(((Control)this).Handle, themeBackgroundRegion, bRedraw: true);
			}
		}
		finally
		{
			val.Dispose();
		}
	}

	protected override void WndProc(ref Message m)
	{
		if (((Message)(ref m)).Msg == 33 && m_Bar.LayoutType != eLayoutType.DockContainer)
		{
			((Message)(ref m)).Result = new IntPtr(3);
		}
		else
		{
			((Form)this).WndProc(ref m);
		}
	}

	protected override void OnActivated(EventArgs e)
	{
		((Form)this).OnActivated(e);
		if (m_Bar != null && m_Bar.LayoutType == eLayoutType.DockContainer)
		{
			m_Bar.SetHasFocus(b: true);
		}
	}

	protected override void OnDeactivate(EventArgs e)
	{
		((Form)this).OnDeactivate(e);
		if (m_Bar != null && m_Bar.LayoutType == eLayoutType.DockContainer)
		{
			m_Bar.SetHasFocus(b: false);
		}
	}
}
