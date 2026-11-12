using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
[ComVisible(false)]
public class SideBarPanelControlHost : Control, IThemeCache
{
	private SideBarPanelItem m_ParentPanel;

	private BaseItem m_HotSubItem;

	private Point m_MouseDownPt = Point.Empty;

	private ThemeWindow m_ThemeWindow;

	private ThemeRebar m_ThemeRebar;

	private ThemeToolbar m_ThemeToolbar;

	private ThemeHeader m_ThemeHeader;

	private ThemeScrollBar m_ThemeScrollBar;

	private ThemeExplorerBar m_ThemeExplorerBar;

	private ThemeProgress m_ThemeProgress;

	private ThemeButton m_ThemeButton;

	private ScrollButton m_TopScrollButton;

	private ScrollButton m_BottomScrollButton;

	ThemeWindow IThemeCache.ThemeWindow
	{
		get
		{
			if (m_ThemeWindow == null)
			{
				m_ThemeWindow = new ThemeWindow((Control)(object)this);
			}
			return m_ThemeWindow;
		}
	}

	ThemeRebar IThemeCache.ThemeRebar
	{
		get
		{
			if (m_ThemeRebar == null)
			{
				m_ThemeRebar = new ThemeRebar((Control)(object)this);
			}
			return m_ThemeRebar;
		}
	}

	ThemeToolbar IThemeCache.ThemeToolbar
	{
		get
		{
			if (m_ThemeToolbar == null)
			{
				m_ThemeToolbar = new ThemeToolbar((Control)(object)this);
			}
			return m_ThemeToolbar;
		}
	}

	ThemeHeader IThemeCache.ThemeHeader
	{
		get
		{
			if (m_ThemeHeader == null)
			{
				m_ThemeHeader = new ThemeHeader((Control)(object)this);
			}
			return m_ThemeHeader;
		}
	}

	ThemeScrollBar IThemeCache.ThemeScrollBar
	{
		get
		{
			if (m_ThemeScrollBar == null)
			{
				m_ThemeScrollBar = new ThemeScrollBar((Control)(object)this);
			}
			return m_ThemeScrollBar;
		}
	}

	ThemeExplorerBar IThemeCache.ThemeExplorerBar
	{
		get
		{
			if (m_ThemeExplorerBar == null)
			{
				m_ThemeExplorerBar = new ThemeExplorerBar((Control)(object)this);
			}
			return m_ThemeExplorerBar;
		}
	}

	ThemeProgress IThemeCache.ThemeProgress
	{
		get
		{
			if (m_ThemeProgress == null)
			{
				m_ThemeProgress = new ThemeProgress((Control)(object)this);
			}
			return m_ThemeProgress;
		}
	}

	ThemeButton IThemeCache.ThemeButton
	{
		get
		{
			if (m_ThemeButton == null)
			{
				m_ThemeButton = new ThemeButton((Control)(object)this);
			}
			return m_ThemeButton;
		}
	}

	public SideBarPanelControlHost(SideBarPanelItem parentPanel)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Expected O, but got Unknown
		//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Expected O, but got Unknown
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)4, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle(DisplayHelp.DoubleBufferFlag, true);
		((Control)this).TabStop = false;
		m_TopScrollButton = new ScrollButton();
		m_TopScrollButton.UseTimer = false;
		m_TopScrollButton.StandardButton = true;
		((Control)m_TopScrollButton).Visible = false;
		m_TopScrollButton.Orientation = eOrientation.Vertical;
		m_TopScrollButton.ButtonAlignment = eItemAlignment.Near;
		ColorScheme colorScheme = null;
		if (parentPanel.Appearance == eSideBarAppearance.Flat)
		{
			((Control)m_TopScrollButton).Size = new Size(14, 14);
			if (parentPanel.HeaderStyle != null)
			{
				colorScheme = new ColorScheme();
				colorScheme.ItemBackground = Color.Empty;
				colorScheme.ItemBackground = Color.Empty;
				colorScheme.ItemBackground = parentPanel.HeaderStyle.BackColor1.Color;
				colorScheme.ItemHotBackground = parentPanel.HeaderStyle.BackColor1.Color;
				colorScheme.ItemHotBackground2 = parentPanel.HeaderStyle.BackColor2.Color;
				colorScheme.ItemHotBorder = parentPanel.HeaderStyle.BorderColor.Color;
				colorScheme.ItemPressedBorder = parentPanel.HeaderStyle.BorderColor.Color;
				colorScheme.ItemText = parentPanel.HeaderStyle.ForeColor.Color;
				colorScheme.ItemHotText = parentPanel.HeaderStyle.ForeColor.Color;
				colorScheme.ItemPressedText = parentPanel.HeaderStyle.ForeColor.Color;
				colorScheme.ItemPressedBackground = ControlPaint.Light(parentPanel.HeaderStyle.BackColor1.Color);
				colorScheme.ItemPressedBackground2 = ControlPaint.Light(parentPanel.HeaderStyle.BackColor2.Color);
				m_TopScrollButton._Scheme = colorScheme;
				m_TopScrollButton.StandardButton = false;
			}
		}
		else
		{
			((Control)m_TopScrollButton).Size = new Size(16, 16);
		}
		((Control)this).Controls.Add((Control)(object)m_TopScrollButton);
		m_BottomScrollButton = new ScrollButton();
		m_BottomScrollButton.UseTimer = false;
		m_BottomScrollButton.StandardButton = true;
		((Control)m_BottomScrollButton).Visible = false;
		m_BottomScrollButton.Orientation = eOrientation.Vertical;
		m_BottomScrollButton.ButtonAlignment = eItemAlignment.Far;
		if (parentPanel.Appearance == eSideBarAppearance.Flat)
		{
			((Control)m_BottomScrollButton).Size = new Size(14, 14);
			if (colorScheme != null)
			{
				m_BottomScrollButton._Scheme = colorScheme;
				m_BottomScrollButton.StandardButton = false;
			}
		}
		else
		{
			((Control)m_BottomScrollButton).Size = new Size(16, 16);
		}
		((Control)this).Controls.Add((Control)(object)m_BottomScrollButton);
		((Control)m_BottomScrollButton).MouseDown += new MouseEventHandler(ScrollButtonClick);
		((Control)m_TopScrollButton).MouseDown += new MouseEventHandler(ScrollButtonClick);
		m_ParentPanel = parentPanel;
		foreach (BaseItem subItem in m_ParentPanel.SubItems)
		{
			subItem.ContainerControl = this;
		}
	}

	public void SetupScrollButtons()
	{
		if (m_ParentPanel.TopItemIndex > 0 && m_ParentPanel.EnableScrollButtons)
		{
			((Control)m_TopScrollButton).Location = new Point(((Control)this).Width - (((Control)m_TopScrollButton).Width + 2), 4);
			m_TopScrollButton.UseThemes = m_ParentPanel.UseThemes;
			((Control)m_TopScrollButton).BringToFront();
			if (!((Control)m_TopScrollButton).Visible)
			{
				((Control)m_TopScrollButton).Visible = true;
			}
		}
		else
		{
			((Control)m_TopScrollButton).Visible = false;
		}
		if (m_ParentPanel.ScrollDownButton && m_ParentPanel.EnableScrollButtons)
		{
			((Control)m_BottomScrollButton).Location = new Point(((Control)this).Width - (((Control)m_BottomScrollButton).Width + 2), ((Control)this).Height - (((Control)m_BottomScrollButton).Height + 2));
			m_BottomScrollButton.UseThemes = m_ParentPanel.UseThemes;
			((Control)m_BottomScrollButton).BringToFront();
			if (!((Control)m_BottomScrollButton).Visible)
			{
				((Control)m_BottomScrollButton).Visible = true;
			}
		}
		else
		{
			((Control)m_BottomScrollButton).Visible = false;
		}
	}

	private void ScrollButtonClick(object sender, MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)e.Button == 1048576)
		{
			if (sender == m_TopScrollButton)
			{
				m_ParentPanel.ScrollButtonClick(up: true);
			}
			else
			{
				m_ParentPanel.ScrollButtonClick(up: false);
			}
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		if (m_ParentPanel == null || ((Control)this).IsDisposed)
		{
			return;
		}
		ItemPaintArgs itemPaintArgs = null;
		SideBar sideBar = m_ParentPanel.ContainerControl as SideBar;
		itemPaintArgs = ((sideBar == null) ? new ItemPaintArgs(m_ParentPanel.GetOwner() as IOwner, (Control)(object)this, e.Graphics, new ColorScheme(e.Graphics)) : new ItemPaintArgs(m_ParentPanel.GetOwner() as IOwner, (Control)(object)this, e.Graphics, sideBar.ColorScheme));
		itemPaintArgs.Renderer = sideBar.GetRenderer();
		if (m_ParentPanel.UseThemes)
		{
			((IThemeCache)this).ThemeRebar.DrawBackground(e.Graphics, ThemeRebarParts.Background, ThemeRebarStates.Normal, ((Control)this).DisplayRectangle);
		}
		else if (!m_ParentPanel.BackgroundStyle.BackColor1.IsEmpty)
		{
			if (m_ParentPanel.Appearance == eSideBarAppearance.Flat)
			{
				eBorderSide borderSide = m_ParentPanel.BackgroundStyle.BorderSide;
				m_ParentPanel.BackgroundStyle.BorderSide = ~(borderSide & eBorderSide.Top) & borderSide;
				m_ParentPanel.BackgroundStyle.Paint(e.Graphics, ((Control)this).ClientRectangle);
				m_ParentPanel.BackgroundStyle.BorderSide = borderSide;
			}
			else
			{
				m_ParentPanel.BackgroundStyle.Paint(e.Graphics, ((Control)this).ClientRectangle);
			}
		}
		else
		{
			Color color = SystemColors.Control;
			if (sideBar != null && BarFunctions.IsOffice2007Style(sideBar.Style) && ((Control)sideBar).BackColor == SystemColors.Control)
			{
				if (itemPaintArgs.Renderer is Office2007Renderer)
				{
					DisplayHelp.FillRectangle(e.Graphics, ((Control)this).ClientRectangle, ((Office2007Renderer)itemPaintArgs.Renderer).ColorTable.SideBar.Background);
					color = Color.Empty;
				}
			}
			else if (((Control)this).Parent != null && ((Control)this).Parent.BackColor != Color.Transparent)
			{
				color = ((Control)this).Parent.BackColor;
			}
			DisplayHelp.FillRectangle(e.Graphics, ((Control)this).ClientRectangle, color, Color.Empty);
		}
		for (int i = m_ParentPanel.TopItemIndex; i < m_ParentPanel.SubItems.Count; i++)
		{
			BaseItem baseItem = m_ParentPanel.SubItems[i];
			if (baseItem.Displayed && baseItem.Visible)
			{
				baseItem.Paint(itemPaintArgs);
			}
		}
	}

	private void InternalClick(MouseButtons mb, Point mpos)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (m_HotSubItem != null)
		{
			m_HotSubItem.InternalClick(mb, mpos);
		}
	}

	private void InternalDoubleClick(MouseButtons mb, Point mpos)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (m_HotSubItem != null)
		{
			m_HotSubItem.InternalDoubleClick(mb, mpos);
		}
	}

	private void InternalKeyDown(KeyEventArgs objArg)
	{
		if (m_ParentPanel.DesignMode)
		{
			objArg.Handled = true;
		}
		else if (m_HotSubItem != null)
		{
			m_HotSubItem.InternalKeyDown(objArg);
		}
	}

	private void InternalMouseDown(MouseEventArgs objArg)
	{
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Invalid comparison between Unknown and I4
		IOwner owner = null;
		m_MouseDownPt = new Point(objArg.X, objArg.Y);
		BaseItem baseItem = m_ParentPanel.ExpandedItem();
		if (baseItem != null && baseItem != m_HotSubItem && !m_ParentPanel.DesignMode)
		{
			baseItem.Expanded = false;
			m_ParentPanel.AutoExpand = false;
		}
		if (m_ParentPanel.DesignMode && m_ParentPanel.CanCustomize)
		{
			if (m_ParentPanel.IsContainer && m_ParentPanel.SubItems.Count > 0)
			{
				BaseItem baseItem2 = ItemAtLocation(objArg.X, objArg.Y);
				if (baseItem2 != null && baseItem2.CanCustomize && m_ParentPanel.GetOwner() is IOwner owner2)
				{
					owner2.SetFocusItem(baseItem2);
				}
				baseItem2?.InternalMouseDown(objArg);
			}
			if ((int)objArg.Button == 2097152 && !m_ParentPanel.IsContainer && m_ParentPanel.GetOwner() is IOwner owner3)
			{
				owner3.DesignTimeContextMenu(m_ParentPanel);
			}
		}
		if (!m_ParentPanel.DesignMode && m_ParentPanel.GetOwner() is IOwner owner4 && owner4.GetFocusItem() != null)
		{
			BaseItem baseItem3 = ItemAtLocation(objArg.X, objArg.Y);
			if (baseItem3 != owner4.GetFocusItem())
			{
				owner4.GetFocusItem().ReleaseFocus();
			}
		}
		if (m_HotSubItem != null)
		{
			m_HotSubItem.InternalMouseDown(objArg);
		}
	}

	private void InternalMouseHover()
	{
		if (m_ParentPanel.DesignMode || m_HotSubItem == null)
		{
			return;
		}
		if (!m_ParentPanel.AutoExpand)
		{
			BaseItem baseItem = m_ParentPanel.ExpandedItem();
			if (baseItem != null && m_HotSubItem != baseItem && (m_ParentPanel.IsOnMenu || m_ParentPanel.ContainerControl is Bar))
			{
				baseItem.Expanded = false;
			}
		}
		if (m_HotSubItem != null)
		{
			m_HotSubItem.InternalMouseHover();
		}
	}

	public virtual void InternalMouseLeave()
	{
		if (!m_ParentPanel.DesignMode && m_HotSubItem != null)
		{
			m_HotSubItem.InternalMouseLeave();
			m_HotSubItem = null;
		}
	}

	private void InternalMouseMove(MouseEventArgs objArg)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Invalid comparison between Unknown and I4
		if (m_ParentPanel.DesignMode && (int)objArg.Button == 1048576 && (Math.Abs(objArg.X - m_MouseDownPt.X) >= 2 || Math.Abs(objArg.Y - m_MouseDownPt.Y) >= 2))
		{
			BaseItem baseItem = m_ParentPanel.FocusedItem();
			if (baseItem != null && baseItem.CanCustomize)
			{
				if (!baseItem.SystemItem && m_ParentPanel.GetOwner() is IOwner owner)
				{
					owner.StartItemDrag(baseItem);
				}
			}
			else if (!m_ParentPanel.IsContainer && !m_ParentPanel.SystemItem && m_ParentPanel.CanCustomize && m_ParentPanel.GetOwner() is IOwner owner2)
			{
				owner2.StartItemDrag(m_ParentPanel);
			}
		}
		else
		{
			if (!m_ParentPanel.IsContainer || m_ParentPanel.DesignMode)
			{
				return;
			}
			BaseItem baseItem2 = ItemAtLocation(objArg.X, objArg.Y);
			if (baseItem2 != m_HotSubItem)
			{
				if (m_HotSubItem != null)
				{
					m_HotSubItem.InternalMouseLeave();
					if (baseItem2 != null && m_HotSubItem.Expanded && (m_ParentPanel.IsOnMenu || m_ParentPanel.ContainerControl is Bar))
					{
						m_HotSubItem.Expanded = false;
					}
				}
				if (baseItem2 != null)
				{
					if (m_ParentPanel.AutoExpand)
					{
						BaseItem baseItem3 = m_ParentPanel.ExpandedItem();
						if (baseItem3 != null && baseItem3 != baseItem2)
						{
							baseItem3.Expanded = false;
						}
					}
					baseItem2.InternalMouseEnter();
					baseItem2.InternalMouseMove(objArg);
					if (m_ParentPanel.AutoExpand && baseItem2.Enabled && baseItem2.ShowSubItems)
					{
						if (baseItem2 is PopupItem)
						{
							PopupItem popupItem = baseItem2 as PopupItem;
							ePopupAnimation popupAnimation = popupItem.PopupAnimation;
							popupItem.PopupAnimation = ePopupAnimation.None;
							if (baseItem2.SubItems.Count > 0)
							{
								baseItem2.Expanded = true;
							}
							popupItem.PopupAnimation = popupAnimation;
						}
						else
						{
							baseItem2.Expanded = true;
						}
					}
					m_HotSubItem = baseItem2;
					ResetHover();
				}
				else
				{
					m_HotSubItem = null;
				}
			}
			else if (m_HotSubItem != null)
			{
				m_HotSubItem.InternalMouseMove(objArg);
			}
		}
	}

	private BaseItem ItemAtLocation(int x, int y)
	{
		foreach (BaseItem subItem in m_ParentPanel.SubItems)
		{
			if ((subItem.Visible || m_ParentPanel.IsOnCustomizeMenu) && subItem.Displayed && subItem.DisplayRectangle.Contains(x, y))
			{
				return subItem;
			}
		}
		return null;
	}

	private void ResetHover()
	{
		NativeFunctions.TRACKMOUSEEVENT tme = default(NativeFunctions.TRACKMOUSEEVENT);
		tme.dwFlags = 1073741824u;
		tme.hwndTrack = ((Control)this).Handle;
		tme.cbSize = Marshal.SizeOf(tme);
		NativeFunctions.TrackMouseEvent(ref tme);
		tme.dwFlags |= 1u;
		NativeFunctions.TrackMouseEvent(ref tme);
	}

	protected override void OnClick(EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		InternalClick(Control.MouseButtons, Control.MousePosition);
		((Control)this).OnClick(e);
	}

	protected override void OnDoubleClick(EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		InternalDoubleClick(Control.MouseButtons, Control.MousePosition);
		((Control)this).OnDoubleClick(e);
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		InternalKeyDown(e);
		((Control)this).OnKeyDown(e);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		InternalMouseDown(e);
		((Control)this).OnMouseDown(e);
	}

	protected override void OnMouseHover(EventArgs e)
	{
		((Control)this).OnMouseHover(e);
		InternalMouseHover();
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		if (((Control)this).Cursor != Cursors.Arrow)
		{
			((Control)this).Cursor = Cursors.Arrow;
		}
		InternalMouseLeave();
		((Control)this).OnMouseLeave(e);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
		InternalMouseMove(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		((Control)this).OnMouseUp(e);
		if (m_HotSubItem != null)
		{
			m_HotSubItem.InternalMouseUp(e);
		}
	}

	protected override void OnHandleDestroyed(EventArgs e)
	{
		DisposeThemes();
		((Control)this).OnHandleDestroyed(e);
	}

	private void DisposeThemes()
	{
		if (m_ThemeWindow != null)
		{
			m_ThemeWindow.Dispose();
			m_ThemeWindow = null;
		}
		if (m_ThemeRebar != null)
		{
			m_ThemeRebar.Dispose();
			m_ThemeRebar = null;
		}
		if (m_ThemeToolbar != null)
		{
			m_ThemeToolbar.Dispose();
			m_ThemeToolbar = null;
		}
		if (m_ThemeHeader != null)
		{
			m_ThemeHeader.Dispose();
			m_ThemeHeader = null;
		}
		if (m_ThemeScrollBar != null)
		{
			m_ThemeScrollBar.Dispose();
			m_ThemeScrollBar = null;
		}
		if (m_ThemeProgress != null)
		{
			m_ThemeProgress.Dispose();
			m_ThemeProgress = null;
		}
		if (m_ThemeExplorerBar != null)
		{
			m_ThemeExplorerBar.Dispose();
			m_ThemeExplorerBar = null;
		}
		if (m_ThemeButton != null)
		{
			m_ThemeButton.Dispose();
			m_ThemeButton = null;
		}
	}

	protected override void WndProc(ref Message m)
	{
		if (((Message)(ref m)).Msg == 794)
		{
			RefreshThemes();
		}
		((Control)this).WndProc(ref m);
	}

	private void RefreshThemes()
	{
		if (m_ThemeWindow != null)
		{
			m_ThemeWindow.Dispose();
			m_ThemeWindow = new ThemeWindow((Control)(object)this);
		}
		if (m_ThemeRebar != null)
		{
			m_ThemeRebar.Dispose();
			m_ThemeRebar = new ThemeRebar((Control)(object)this);
		}
		if (m_ThemeToolbar != null)
		{
			m_ThemeToolbar.Dispose();
			m_ThemeToolbar = new ThemeToolbar((Control)(object)this);
		}
		if (m_ThemeHeader != null)
		{
			m_ThemeHeader.Dispose();
			m_ThemeHeader = new ThemeHeader((Control)(object)this);
		}
		if (m_ThemeScrollBar != null)
		{
			m_ThemeScrollBar.Dispose();
			m_ThemeScrollBar = new ThemeScrollBar((Control)(object)this);
		}
		if (m_ThemeProgress != null)
		{
			m_ThemeProgress.Dispose();
			m_ThemeProgress = new ThemeProgress((Control)(object)this);
		}
		if (m_ThemeExplorerBar != null)
		{
			m_ThemeExplorerBar.Dispose();
			m_ThemeExplorerBar = new ThemeExplorerBar((Control)(object)this);
		}
	}
}
