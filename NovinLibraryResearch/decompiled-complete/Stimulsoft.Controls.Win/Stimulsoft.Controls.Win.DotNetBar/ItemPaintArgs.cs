using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Animation;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class ItemPaintArgs
{
	public Graphics Graphics;

	public ColorScheme Colors;

	public Control ContainerControl;

	public eTextFormat ButtonStringFormat;

	public bool IsOnMenu;

	public bool IsOnPopupBar;

	public bool IsOnMenuBar;

	public bool IsOnRibbonBar;

	public bool IsOnNavigationBar;

	public Font Font;

	public IOwner Owner;

	public bool RightToLeft;

	private ThemeWindow m_ThemeWindow;

	private ThemeRebar m_ThemeRebar;

	private ThemeToolbar m_ThemeToolbar;

	private ThemeHeader m_ThemeHeader;

	private ThemeScrollBar m_ThemeScrollBar;

	private ThemeExplorerBar m_ThemeExplorerBar;

	private ThemeProgress m_ThemeProgress;

	private ThemeButton m_ThemeButton;

	public bool DesignerSelection;

	public bool GlassEnabled;

	private BaseRenderer m_Renderer;

	public ButtonItemRendererEventArgs ButtonItemRendererEventArgs = new ButtonItemRendererEventArgs();

	public Rectangle ClipRectangle = Rectangle.Empty;

	public bool ControlExpanded = true;

	internal bool CachedPaint;

	internal bool IsDefaultButton;

	public bool IsFlatOffice2007Representation;

	public PaintEventArgs PaintEventArgs;

	internal bool AnimationEnabled;

	internal List<AnimationRequest> AnimationRequests;

	public bool DragInProgress;

	public BaseRenderer Renderer
	{
		get
		{
			return m_Renderer;
		}
		set
		{
			m_Renderer = value;
		}
	}

	internal ThemeWindow ThemeWindow
	{
		get
		{
			if (m_ThemeWindow == null)
			{
				if (ContainerControl is Bar)
				{
					m_ThemeWindow = ((Bar)(object)ContainerControl).ThemeWindow;
				}
				else if (ContainerControl is IThemeCache)
				{
					m_ThemeWindow = ((IThemeCache)ContainerControl).ThemeWindow;
				}
			}
			return m_ThemeWindow;
		}
	}

	internal ThemeRebar ThemeRebar
	{
		get
		{
			if (m_ThemeRebar == null)
			{
				if (ContainerControl is Bar)
				{
					m_ThemeRebar = ((Bar)(object)ContainerControl).ThemeRebar;
				}
				else if (ContainerControl is IThemeCache)
				{
					m_ThemeRebar = ((IThemeCache)ContainerControl).ThemeRebar;
				}
			}
			return m_ThemeRebar;
		}
	}

	internal ThemeToolbar ThemeToolbar
	{
		get
		{
			if (m_ThemeToolbar == null)
			{
				if (ContainerControl is Bar)
				{
					m_ThemeToolbar = ((Bar)(object)ContainerControl).ThemeToolbar;
				}
				else if (ContainerControl is IThemeCache)
				{
					m_ThemeToolbar = ((IThemeCache)ContainerControl).ThemeToolbar;
				}
			}
			return m_ThemeToolbar;
		}
	}

	internal ThemeHeader ThemeHeader
	{
		get
		{
			if (m_ThemeHeader == null)
			{
				if (ContainerControl is Bar)
				{
					m_ThemeHeader = ((Bar)(object)ContainerControl).ThemeHeader;
				}
				else if (ContainerControl is IThemeCache)
				{
					m_ThemeHeader = ((IThemeCache)ContainerControl).ThemeHeader;
				}
			}
			return m_ThemeHeader;
		}
	}

	internal ThemeScrollBar ThemeScrollBar
	{
		get
		{
			if (m_ThemeScrollBar == null)
			{
				if (ContainerControl is Bar)
				{
					m_ThemeScrollBar = ((Bar)(object)ContainerControl).ThemeScrollBar;
				}
				else if (ContainerControl is IThemeCache)
				{
					m_ThemeScrollBar = ((IThemeCache)ContainerControl).ThemeScrollBar;
				}
			}
			return m_ThemeScrollBar;
		}
	}

	internal ThemeExplorerBar ThemeExplorerBar
	{
		get
		{
			if (m_ThemeExplorerBar == null && ContainerControl is IThemeCache)
			{
				m_ThemeExplorerBar = ((IThemeCache)ContainerControl).ThemeExplorerBar;
			}
			return m_ThemeExplorerBar;
		}
	}

	internal ThemeProgress ThemeProgress
	{
		get
		{
			if (m_ThemeProgress == null)
			{
				if (ContainerControl is Bar)
				{
					m_ThemeProgress = ((Bar)(object)ContainerControl).ThemeProgress;
				}
				else if (ContainerControl is IThemeCache)
				{
					m_ThemeProgress = ((IThemeCache)ContainerControl).ThemeProgress;
				}
			}
			return m_ThemeProgress;
		}
	}

	internal ThemeButton ThemeButton
	{
		get
		{
			if (m_ThemeButton == null && ContainerControl is IThemeCache)
			{
				m_ThemeButton = ((IThemeCache)ContainerControl).ThemeButton;
			}
			return m_ThemeButton;
		}
	}

	public ItemPaintArgs(IOwner owner, Control control, Graphics g, ColorScheme scheme)
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Invalid comparison between Unknown and I4
		Graphics = g;
		Colors = scheme;
		ContainerControl = control;
		Owner = owner;
		if (control != null)
		{
			RightToLeft = (int)control.RightToLeft == 1;
		}
		if (control is MenuPanel || ContainerControl is ItemsListBox)
		{
			IsOnMenu = true;
		}
		else if (control is Bar && ((Bar)(object)control).MenuBar)
		{
			IsOnMenuBar = true;
		}
		else if (control is Bar && ((Bar)(object)control).BarState == eBarState.Popup)
		{
			IsOnPopupBar = true;
		}
		else if (control is RibbonBar)
		{
			IsOnRibbonBar = true;
		}
		else if (control is NavigationBar)
		{
			IsOnNavigationBar = true;
		}
		if (control != null)
		{
			Font = control.Font;
		}
		CreateStringFormat();
	}

	private void CreateStringFormat()
	{
		eTextFormat eTextFormat2 = eTextFormat.Default;
		if (ContainerControl is ItemControl && (Owner == null || !Owner.AlwaysDisplayKeyAccelerators))
		{
			eTextFormat2 |= eTextFormat.HidePrefix;
		}
		else if ((Owner == null || !Owner.AlwaysDisplayKeyAccelerators) && !NativeFunctions.ShowKeyboardCues && !IsOnMenu)
		{
			Bar bar = ContainerControl as Bar;
			if ((ContainerControl == null || !ContainerControl.Focused) && (bar == null || !bar.MenuFocus) && (bar == null || !bar.AlwaysDisplayKeyAccelerators) && !(ContainerControl is NavigationBar))
			{
				eTextFormat2 |= eTextFormat.HidePrefix;
			}
		}
		eTextFormat2 = ((!(ContainerControl is RibbonBar)) ? (eTextFormat2 | (eTextFormat.SingleLine | eTextFormat.VerticalCenter)) : (eTextFormat2 | eTextFormat.VerticalCenter));
		if (!TextDrawing.UseTextRenderer)
		{
			eTextFormat2 |= eTextFormat.EndEllipsis;
		}
		ButtonStringFormat = eTextFormat2;
	}
}
