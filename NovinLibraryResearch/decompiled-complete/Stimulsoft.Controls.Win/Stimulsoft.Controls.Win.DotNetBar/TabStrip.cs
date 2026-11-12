using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Controls.Win.UI.ContentManager;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[ComVisible(false)]
[DefaultEvent("SelectedTabChanged")]
public class TabStrip : Control
{
	public delegate void SelectedTabChangedEventHandler(object sender, TabStripTabChangedEventArgs e);

	public delegate void SelectedTabChangingEventHandler(object sender, TabStripTabChangingEventArgs e);

	public delegate void TabMovedEventHandler(object sender, TabStripTabMovedEventArgs e);

	public delegate void UserActionEventHandler(object sender, TabStripActionEventArgs e);

	private eTabStripAlignment m_Alignment = eTabStripAlignment.Bottom;

	private TabsCollection m_Tabs;

	private ImageList m_ImageList;

	private TabItem m_SelectedTab;

	private bool m_NeedRecalcSize = true;

	private bool m_TabDrag;

	private Bar m_DragBar;

	private bool m_IsDragging;

	private bool m_CanReorderTabs = true;

	private bool m_VariableTabWidth = true;

	private int m_ScrollOffset;

	private TabSystemBox m_TabSystemBox;

	private bool m_Animate = true;

	private bool m_MdiTabbedDocuments;

	private Form m_MdiForm;

	private bool m_MdiInitialized;

	private int m_MaxMdiCaptionLength = 32;

	private bool m_ShowMdiChildIcon = true;

	private bool m_MdiAutoHide = true;

	private bool m_MdiNoFormActivateFlicker = true;

	private Font m_SelectedTabFont;

	private bool m_SelectedTabFontCustom;

	private bool m_DisplaySelectedTextOnly;

	private ThemeTab m_ThemeTab;

	private TabItem m_HotTab;

	private eTabStripStyle m_Style;

	private Color m_SelectedTabTextColor = Color.Empty;

	private Color m_HotTextColor = Color.Empty;

	private Color m_SeparatorColor = Color.Empty;

	private Color m_SeparatorShadeColor = Color.Empty;

	private bool m_TabScrollAutoRepeat;

	private int m_TabScrollRepeatInterval = 300;

	private TabColorScheme m_ColorScheme;

	private TabItem m_DesignTimeSelection;

	private bool m_ShowFocusRectangle = true;

	private bool m_AutoHideSystemBox;

	private int MIN_TEXT_WIDTH = 4;

	private int m_MultiLineSpacing;

	private eTabLayoutType m_TabLayoutType;

	private ToolTip m_ToolTip;

	private TabItem m_TooltipTab;

	private bool m_ThemeAware;

	private Rectangle m_TabItemsBounds = Rectangle.Empty;

	private Size m_FixedTabSize = Size.Empty;

	private bool m_CloseButtonOnTabs;

	private eTabCloseButtonPosition m_CloseButtonPosition;

	private static Size default_close_button_size = new Size(11, 11);

	private Size m_CloseButtonSize = default_close_button_size;

	private Image m_TabCloseButtonNormal;

	private Image m_TabCloseButtonHot;

	private bool m_CloseButtonOnTabsAlwaysDisplayed = true;

	private bool m_AntiAlias;

	private bool m_AutoSelectAttachedControl = true;

	private int m_TabHeight = -1;

	private bool _KeyboardNavigationEnabled = true;

	private Point _MouseDownPoint = Point.Empty;

	private bool m_MovingTab;

	private Cursor _OldCursor;

	private Cursor _TabDragCursor;

	internal bool _IgnoreBeforeTabDisplayEvent;

	[DefaultValue(false)]
	[Browsable(true)]
	[Category("Appearance")]
	[Description("Gets or sets whether anti-aliasing is used while painting.")]
	public bool AntiAlias
	{
		get
		{
			return m_AntiAlias;
		}
		set
		{
			if (m_AntiAlias != value)
			{
				m_AntiAlias = value;
				((Control)this).Invalidate();
			}
		}
	}

	[Browsable(false)]
	public int MinTabStripHeight
	{
		get
		{
			if (m_Style == eTabStripStyle.SimulatedTheme || m_Style == eTabStripStyle.Office2007Document || m_Style == eTabStripStyle.Office2007Dock)
			{
				if (m_TabItemsBounds.IsEmpty || m_NeedRecalcSize)
				{
					if (Tabs.Count == 0 || !m_NeedRecalcSize)
					{
						return TabHeight + 6;
					}
					RecalcSize();
				}
				if (m_Alignment == eTabStripAlignment.Top || m_Alignment == eTabStripAlignment.Bottom)
				{
					return m_TabItemsBounds.Height + 3;
				}
				return m_TabItemsBounds.Width + 3;
			}
			if (m_Style == eTabStripStyle.OneNote || m_Style == eTabStripStyle.VS2005Document)
			{
				return TabHeight + 6;
			}
			return TabHeight + 5;
		}
	}

	private int TabHeight
	{
		get
		{
			if (m_TabHeight <= 0)
			{
				Graphics val = null;
				if (BarFunctions.IsHandleValid((Control)(object)this))
				{
					val = ((Control)this).CreateGraphics();
				}
				else
				{
					try
					{
						val = Graphics.FromHwnd(IntPtr.Zero);
					}
					catch
					{
					}
				}
				if (val != null)
				{
					try
					{
						m_TabHeight = GetTabHeight(val);
					}
					finally
					{
						val.Dispose();
					}
				}
			}
			return m_TabHeight;
		}
	}

	private bool IsVertical
	{
		get
		{
			if ((m_Alignment == eTabStripAlignment.Left || m_Alignment == eTabStripAlignment.Right) && Style != eTabStripStyle.SimulatedTheme)
			{
				return true;
			}
			return false;
		}
	}

	internal Rectangle TabItemsBounds => m_TabItemsBounds;

	private bool IsRightToLeft => (int)((Control)this).RightToLeft == 1;

	private bool IsContentManagedStyle
	{
		get
		{
			if (((Style == eTabStripStyle.RoundHeader || Style == eTabStripStyle.VS2005Dock) && !IsThemed) || Style == eTabStripStyle.SimulatedTheme || Style == eTabStripStyle.Office2007Document || Style == eTabStripStyle.Office2007Dock)
			{
				return true;
			}
			return false;
		}
	}

	internal bool HasMeasureTabItem => this.MeasureTabItem != null;

	internal bool HasPreRenderTabItem => this.PreRenderTabItem != null;

	internal bool HasPostRenderTabItem => this.PostRenderTabItem != null;

	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public TabItem DesignTimeSelection
	{
		get
		{
			return m_DesignTimeSelection;
		}
		set
		{
			m_DesignTimeSelection = value;
			((Control)this).Invalidate();
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int SelectedTabIndex
	{
		get
		{
			if (m_SelectedTab == null)
			{
				return -1;
			}
			return m_Tabs.IndexOf(m_SelectedTab);
		}
		set
		{
			if (value >= 0 && value < m_Tabs.Count && m_Tabs.Count != 0)
			{
				SelectedTab = m_Tabs[value];
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Description("Indicates selected tab.")]
	public TabItem SelectedTab
	{
		get
		{
			if (m_SelectedTab == null && m_Tabs.Count > 0)
			{
				m_SelectedTab = m_Tabs[0];
			}
			return m_SelectedTab;
		}
		set
		{
			SelectTab(value, eEventSource.Code);
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates whether tabs are scrolled continuously while mouse is pressed over the scroll tab button.")]
	public virtual bool TabScrollAutoRepeat
	{
		get
		{
			return m_TabScrollAutoRepeat;
		}
		set
		{
			m_TabScrollAutoRepeat = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(300)]
	[Category("Behavior")]
	[Description("Indicates auto-repeat interval for the tab scrolling while mouse button is kept pressed over the scroll tab button.")]
	public virtual int TabScrollRepeatInterval
	{
		get
		{
			return m_TabScrollRepeatInterval;
		}
		set
		{
			m_TabScrollRepeatInterval = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Close Button")]
	[Description("Indicates whether the Close button that closes the active tab is visible.")]
	public bool CloseButtonVisible
	{
		get
		{
			if (m_TabSystemBox != null)
			{
				return m_TabSystemBox.CloseVisible;
			}
			return true;
		}
		set
		{
			if (m_TabSystemBox != null)
			{
				m_TabSystemBox.CloseVisible = value;
			}
			((Control)this).Invalidate();
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[DevCoBrowsable(true)]
	[Category("Close Button")]
	[Description("Indicates whether close button is visible on each tab instead of in system box.")]
	public bool CloseButtonOnTabsVisible
	{
		get
		{
			return m_CloseButtonOnTabs;
		}
		set
		{
			m_CloseButtonOnTabs = value;
			m_NeedRecalcSize = true;
			((Control)this).Invalidate();
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[DevCoBrowsable(true)]
	[Category("Close Button")]
	[Description("Indicates whether close button on tabs when visible is displayed for every tab state.")]
	public bool CloseButtonOnTabsAlwaysDisplayed
	{
		get
		{
			return m_CloseButtonOnTabsAlwaysDisplayed;
		}
		set
		{
			m_CloseButtonOnTabsAlwaysDisplayed = value;
			((Control)this).Invalidate();
		}
	}

	[Browsable(true)]
	[DefaultValue(eTabCloseButtonPosition.Left)]
	[DevCoBrowsable(true)]
	[Category("Close Button")]
	[Description("Indicates position of the close button displayed on each tab.")]
	public eTabCloseButtonPosition CloseButtonPosition
	{
		get
		{
			return m_CloseButtonPosition;
		}
		set
		{
			m_CloseButtonPosition = value;
			m_NeedRecalcSize = true;
			((Control)this).Invalidate();
		}
	}

	[Browsable(true)]
	[DefaultValue(null)]
	[Category("Close Button")]
	[Description("Indicates custom image that is used on tabs as Close button that allows user to close the tab.")]
	public Image TabCloseButtonNormal
	{
		get
		{
			return m_TabCloseButtonNormal;
		}
		set
		{
			m_TabCloseButtonNormal = value;
			if (m_TabCloseButtonNormal != null)
			{
				m_CloseButtonSize = m_TabCloseButtonNormal.Size;
			}
			else
			{
				m_CloseButtonSize = default_close_button_size;
			}
			m_NeedRecalcSize = true;
			((Control)this).Invalidate();
		}
	}

	[Browsable(true)]
	[DefaultValue(null)]
	[Category("Close Button")]
	[Description("Indicates custom image that is used on tabs as Close button whem mouse is over the close button.")]
	public Image TabCloseButtonHot
	{
		get
		{
			return m_TabCloseButtonHot;
		}
		set
		{
			m_TabCloseButtonHot = value;
		}
	}

	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.TabStripTabsEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Data")]
	[Description("Returns the collection of Tabs.")]
	public TabsCollection Tabs => m_Tabs;

	[Browsable(false)]
	public bool AutoSelectAttachedControl
	{
		get
		{
			return m_AutoSelectAttachedControl;
		}
		set
		{
			m_AutoSelectAttachedControl = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[DefaultValue(null)]
	public ImageList ImageList
	{
		get
		{
			return m_ImageList;
		}
		set
		{
			m_ImageList = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(eTabLayoutType.FitContainer)]
	[Category("Appearance")]
	[Description("Indicates the type of the tab layout.")]
	public eTabLayoutType TabLayoutType
	{
		get
		{
			return m_TabLayoutType;
		}
		set
		{
			m_TabLayoutType = value;
			m_NeedRecalcSize = true;
			m_TabSystemBox.DisplayRectangle = Rectangle.Empty;
			OnTabStyleChanged();
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Obsolete("Please use TabLayoutType property instead.", true)]
	public bool VariableTabWidth
	{
		get
		{
			return m_VariableTabWidth;
		}
		set
		{
			if (value)
			{
				m_TabLayoutType = eTabLayoutType.FitContainer;
			}
			else
			{
				m_TabLayoutType = eTabLayoutType.FixedWithNavigationBox;
			}
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DefaultValue(0)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int ScrollOffset
	{
		get
		{
			return m_ScrollOffset;
		}
		set
		{
			if (m_ScrollOffset != value)
			{
				m_ScrollOffset = value;
				m_NeedRecalcSize = true;
				((Control)this).Invalidate();
			}
		}
	}

	internal bool IsDesignMode
	{
		get
		{
			if (((Control)this).Parent != null && ((Control)this).Parent is TabControl && ((Component)(object)((Control)this).Parent).Site != null)
			{
				return ((Component)(object)((Control)this).Parent).Site.DesignMode;
			}
			return ((Component)this).DesignMode;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	internal TabSystemBox _TabSystemBox => m_TabSystemBox;

	[Category("Behavior")]
	[DefaultValue(true)]
	[Description("Indicates whether keyboard navigation using Left and Right arrow keys to select tabs is enabled.")]
	public bool KeyboardNavigationEnabled
	{
		get
		{
			return _KeyboardNavigationEnabled;
		}
		set
		{
			_KeyboardNavigationEnabled = value;
		}
	}

	internal bool HasNavigationBox
	{
		get
		{
			if (m_TabLayoutType != eTabLayoutType.FixedWithNavigationBox)
			{
				return m_TabLayoutType == eTabLayoutType.MultilineWithNavigationBox;
			}
			return true;
		}
	}

	internal bool IsMultiLine
	{
		get
		{
			if (m_TabLayoutType != eTabLayoutType.MultilineNoNavigationBox)
			{
				return m_TabLayoutType == eTabLayoutType.MultilineWithNavigationBox;
			}
			return true;
		}
	}

	private bool TabDrag
	{
		get
		{
			return m_TabDrag;
		}
		set
		{
			if (m_TabDrag == value)
			{
				return;
			}
			m_TabDrag = value;
			if (m_TabDrag)
			{
				if (!IsDockTab)
				{
					_OldCursor = ((Control)this).Cursor;
					((Control)this).Cursor = GetTabDragCursor();
				}
			}
			else if (!IsDockTab)
			{
				((Control)this).Cursor = _OldCursor;
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(null)]
	[Category("Appearance")]
	[Description("Specifies the mouse cursor that is displayed when tab is dragged.")]
	public Cursor TabDragCursor
	{
		get
		{
			return _TabDragCursor;
		}
		set
		{
			_TabDragCursor = value;
		}
	}

	private bool IsDockTab => ((Control)this).Parent is Bar;

	internal TabItem MouseOverTab => m_HotTab;

	internal int VisibleTabCount
	{
		get
		{
			int num = 0;
			foreach (TabItem tab in m_Tabs)
			{
				if (tab.Visible)
				{
					num++;
				}
			}
			return num;
		}
	}

	internal bool IsDraggingBar => m_IsDragging;

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Style")]
	[DefaultValue(null)]
	[Description("Gets or sets the selected tab Font")]
	public Font SelectedTabFont
	{
		get
		{
			return m_SelectedTabFont;
		}
		set
		{
			m_SelectedTabFont = value;
			if (m_SelectedTabFont == null)
			{
				m_SelectedTabFontCustom = false;
			}
			else
			{
				m_SelectedTabFontCustom = true;
			}
			m_NeedRecalcSize = true;
			((Control)this).Invalidate();
		}
	}

	[DefaultValue(eTabStripAlignment.Bottom)]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Indicates the tab alignment within the Tab-Strip control.")]
	public eTabStripAlignment TabAlignment
	{
		get
		{
			return m_Alignment;
		}
		set
		{
			if (m_Alignment != value)
			{
				m_Alignment = value;
				m_NeedRecalcSize = true;
				((Control)this).Invalidate();
			}
		}
	}

	internal bool NeedRecalcSize
	{
		get
		{
			return m_NeedRecalcSize;
		}
		set
		{
			m_NeedRecalcSize = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Description("Specifes whether end-user can reorder the tabs.")]
	public bool CanReorderTabs
	{
		get
		{
			return m_CanReorderTabs;
		}
		set
		{
			if (m_CanReorderTabs != value)
			{
				m_CanReorderTabs = value;
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates whether system box that enables scrolling and closing of the tabs is automatically hidden when tab items size does not exceed the size of the control.")]
	public bool AutoHideSystemBox
	{
		get
		{
			return m_AutoHideSystemBox;
		}
		set
		{
			if (m_AutoHideSystemBox != value)
			{
				m_AutoHideSystemBox = value;
				m_NeedRecalcSize = true;
				if (IsDesignMode)
				{
					((Control)this).Invalidate();
				}
			}
		}
	}

	[Browsable(false)]
	[Description("Indicates the background color.")]
	[Category("Style")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override Color BackColor
	{
		get
		{
			return ((Control)this).BackColor;
		}
		set
		{
			((Control)this).BackColor = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Description("Specifes whether only selected tab is displaying it's text.")]
	[DefaultValue(false)]
	public bool DisplaySelectedTextOnly
	{
		get
		{
			return m_DisplaySelectedTextOnly;
		}
		set
		{
			m_DisplaySelectedTextOnly = value;
			m_NeedRecalcSize = true;
			((Control)this).Invalidate();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Specifes the TabStrip style.")]
	[DefaultValue(eTabStripStyle.Flat)]
	public eTabStripStyle Style
	{
		get
		{
			return m_Style;
		}
		set
		{
			m_Style = value;
			OnTabStyleChanged();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(false)]
	[Category("Appearance")]
	[Description("Specifies whether tab is drawn using Themes when running on OS that supports themes like Windows XP.")]
	public virtual bool ThemeAware
	{
		get
		{
			return m_ThemeAware;
		}
		set
		{
			m_ThemeAware = value;
			m_NeedRecalcSize = true;
			m_ColorScheme.Themed = IsThemed;
			((Control)this).Invalidate();
		}
	}

	internal bool IsThemed
	{
		get
		{
			if (m_ThemeAware && BarFunctions.ThemedOS && Themes.ThemesActive && m_Style != eTabStripStyle.SimulatedTheme && m_Style != eTabStripStyle.Office2007Dock && m_Style != eTabStripStyle.Office2007Document && m_Style != eTabStripStyle.Metro)
			{
				return true;
			}
			return false;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Style")]
	[Description("Gets or sets Tab Color Scheme.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public TabColorScheme ColorScheme
	{
		get
		{
			return m_ColorScheme;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentException("NULL is not a valid value for this property.");
			}
			if (m_ColorScheme != null)
			{
				m_ColorScheme.ColorChanged -= ColorSchemeChanged;
			}
			m_ColorScheme = value;
			m_ColorScheme.ColorChanged += ColorSchemeChanged;
			if (((Control)this).Visible)
			{
				((Control)this).Invalidate();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Description("Indicates whether the tab scrolling is animanted.")]
	[Category("Behavior")]
	[DefaultValue(true)]
	public bool Animate
	{
		get
		{
			return m_Animate;
		}
		set
		{
			m_Animate = value;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Gets or sets the fixed tab size in pixels. Either Height or Width can be set or both.")]
	public Size FixedTabSize
	{
		get
		{
			return m_FixedTabSize;
		}
		set
		{
			m_FixedTabSize = value;
			m_NeedRecalcSize = true;
			if (((Component)this).DesignMode)
			{
				((Control)this).Invalidate();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether focus rectangle is displayed when tab has input focus.")]
	public bool ShowFocusRectangle
	{
		get
		{
			return m_ShowFocusRectangle;
		}
		set
		{
			m_ShowFocusRectangle = value;
		}
	}

	[Browsable(true)]
	[Description("Indicates whether Tab-Strip control provides Tabbed MDI Child form support.")]
	[Category("Mdi Support")]
	[DefaultValue(false)]
	public bool MdiTabbedDocuments
	{
		get
		{
			return m_MdiTabbedDocuments;
		}
		set
		{
			if (m_MdiTabbedDocuments == value)
			{
				return;
			}
			m_MdiTabbedDocuments = value;
			if (m_MdiForm == null && m_MdiTabbedDocuments)
			{
				Form val = ((Control)this).FindForm();
				if (val != null && val.IsMdiContainer)
				{
					MdiForm = val;
				}
			}
			else if (!m_MdiTabbedDocuments)
			{
				MdiForm = null;
				m_Tabs.Clear();
				((Control)this).Invalidate();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Description("Indicates the maximum number of characters that will be used as Tab text from Mdi Child caption.")]
	[Category("Mdi Support")]
	[DefaultValue(32)]
	public int MaxMdiCaptionLength
	{
		get
		{
			return m_MaxMdiCaptionLength;
		}
		set
		{
			if (m_MaxMdiCaptionLength != value)
			{
				m_MaxMdiCaptionLength = value;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Description("Indicates whether the Mdi Child Icon is displayed on Tab.")]
	[Category("Mdi Support")]
	[DefaultValue(true)]
	public bool ShowMdiChildIcon
	{
		get
		{
			return m_ShowMdiChildIcon;
		}
		set
		{
			if (m_ShowMdiChildIcon != value)
			{
				m_ShowMdiChildIcon = value;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Description("Indicates whether the Tab-strip is automatically hidden when there are not Mdi Child forms open.")]
	[Category("Mdi Support")]
	[DefaultValue(true)]
	public bool MdiAutoHide
	{
		get
		{
			return m_MdiAutoHide;
		}
		set
		{
			if (m_MdiAutoHide != value)
			{
				m_MdiAutoHide = value;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Description("Indicates whether flicker associated with switching maximized Mdi child forms is attempted to eliminate.")]
	[Category("Mdi Support")]
	[DefaultValue(true)]
	public bool MdiNoFormActivateFlicker
	{
		get
		{
			return m_MdiNoFormActivateFlicker;
		}
		set
		{
			if (m_MdiNoFormActivateFlicker != value)
			{
				m_MdiNoFormActivateFlicker = value;
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(null)]
	[Description("Indicates Mdi Container form for which Tab-Strip is providing Tabbed MDI Child support.")]
	public Form MdiForm
	{
		get
		{
			return m_MdiForm;
		}
		set
		{
			if (m_MdiForm != null)
			{
				ReleaseMdiForm();
			}
			m_MdiForm = value;
			if (m_MdiForm != null && !((Component)this).DesignMode)
			{
				InitializeMdiForm();
				RefreshMdiTabItems();
			}
		}
	}

	public event SelectedTabChangedEventHandler SelectedTabChanged;

	public event SelectedTabChangingEventHandler SelectedTabChanging;

	public event TabMovedEventHandler TabMoved;

	public event UserActionEventHandler NavigateBack;

	public event UserActionEventHandler NavigateForward;

	public event UserActionEventHandler TabItemClose;

	public event EventHandler TabItemOpen;

	public event EventHandler BeforeTabDisplay;

	public event EventHandler TabRemoved;

	public event EventHandler TabsCleared;

	internal event EventHandler SizeRecalculated;

	public event MeasureTabItemEventHandler MeasureTabItem;

	public event RenderTabItemEventHandler PreRenderTabItem;

	public event RenderTabItemEventHandler PostRenderTabItem;

	public TabStrip()
	{
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		if (!ColorFunctions.ColorsLoaded)
		{
			NativeFunctions.RefreshSettings();
			NativeFunctions.OnDisplayChange();
			ColorFunctions.LoadColors();
		}
		m_Tabs = new TabsCollection(this);
		((Control)this).SetStyle((ControlStyles)512, true);
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)4, true);
		((Control)this).SetStyle((ControlStyles)2048, true);
		((Control)this).SetStyle(DisplayHelp.DoubleBufferFlag, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		m_TabSystemBox = new TabSystemBox(this);
		m_TabSystemBox.Back += OnTabBack;
		m_TabSystemBox.Close += OnTabClose;
		m_TabSystemBox.Forward += OnTabForward;
		m_ColorScheme = new TabColorScheme(m_Style);
		m_ColorScheme.ColorChanged += ColorSchemeChanged;
		StyleManager.Register((Control)(object)this);
	}

	protected override void OnResize(EventArgs e)
	{
		((Control)this).OnResize(e);
		if (((Control)this).Width != 0 && ((Control)this).Height != 0)
		{
			m_ScrollOffset = 0;
			RecalcSize();
			EnsureVisible(SelectedTab);
		}
	}

	private Rectangle GetFocusRectangle(Rectangle rText)
	{
		rText.Inflate(-1, -1);
		return rText;
	}

	internal void PaintTabSystemBox(Graphics g)
	{
		if (HasNavigationBox && m_TabSystemBox.Visible)
		{
			m_TabSystemBox.Paint(g);
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		SmoothingMode smoothingMode = e.Graphics.SmoothingMode;
		TextRenderingHint textRenderingHint = e.Graphics.TextRenderingHint;
		if (m_AntiAlias)
		{
			e.Graphics.SmoothingMode = (SmoothingMode)4;
			if (!SystemInformation.IsFontSmoothingEnabled)
			{
				e.Graphics.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
			}
		}
		if (((Control)this).BackColor == Color.Transparent || ColorScheme.TabBackground == Color.Transparent || ((Control)this).BackgroundImage != null)
		{
			((Control)this).OnPaintBackground(e);
		}
		if (m_Style == eTabStripStyle.SimulatedTheme)
		{
			if (m_NeedRecalcSize)
			{
				RecalcSize(e.Graphics, GetTabClientArea(((Control)this).DisplayRectangle, TabLayoutType == eTabLayoutType.MultilineWithNavigationBox, bPaintArea: false));
			}
			TabStripSimulatedThemeDisplay tabStripSimulatedThemeDisplay = new TabStripSimulatedThemeDisplay();
			tabStripSimulatedThemeDisplay.CloseButtonOnTabs = m_CloseButtonOnTabs;
			tabStripSimulatedThemeDisplay.Paint(e.Graphics, this);
		}
		else if (m_Style == eTabStripStyle.Metro)
		{
			PaintFlat(e.Graphics);
		}
		else if (IsThemed)
		{
			PaintThemed(e.Graphics);
		}
		else if (m_Style == eTabStripStyle.OneNote || m_Style == eTabStripStyle.VS2005Document)
		{
			PaintOneNote(e.Graphics);
		}
		else if (m_Style == eTabStripStyle.VS2005)
		{
			PaintVS2005(e.Graphics);
		}
		else if (m_Style == eTabStripStyle.RoundHeader)
		{
			if (m_NeedRecalcSize)
			{
				RecalcSize(e.Graphics, GetTabClientArea(((Control)this).DisplayRectangle, TabLayoutType == eTabLayoutType.MultilineWithNavigationBox, bPaintArea: false));
			}
			TabStripRoundHeaderDisplay tabStripRoundHeaderDisplay = new TabStripRoundHeaderDisplay();
			tabStripRoundHeaderDisplay.CloseButtonOnTabs = m_CloseButtonOnTabs;
			tabStripRoundHeaderDisplay.Paint(e.Graphics, this);
		}
		else if (m_Style == eTabStripStyle.Office2007Dock)
		{
			if (m_NeedRecalcSize)
			{
				RecalcSize(e.Graphics, GetTabClientArea(((Control)this).DisplayRectangle, TabLayoutType == eTabLayoutType.MultilineWithNavigationBox, bPaintArea: false));
			}
			TabStripOffice2007DockDisplay tabStripOffice2007DockDisplay = new TabStripOffice2007DockDisplay();
			tabStripOffice2007DockDisplay.CloseButtonOnTabs = m_CloseButtonOnTabs;
			tabStripOffice2007DockDisplay.Paint(e.Graphics, this);
		}
		else if (m_Style == eTabStripStyle.VS2005Dock)
		{
			if (m_NeedRecalcSize)
			{
				RecalcSize(e.Graphics, GetTabClientArea(((Control)this).DisplayRectangle, TabLayoutType == eTabLayoutType.MultilineWithNavigationBox, bPaintArea: false));
			}
			TabStripVS2005DockDisplay tabStripVS2005DockDisplay = new TabStripVS2005DockDisplay();
			tabStripVS2005DockDisplay.CloseButtonOnTabs = m_CloseButtonOnTabs;
			tabStripVS2005DockDisplay.Paint(e.Graphics, this);
		}
		else if (m_Style == eTabStripStyle.Office2007Document)
		{
			if (m_NeedRecalcSize)
			{
				RecalcSize(e.Graphics, GetTabClientArea(((Control)this).DisplayRectangle, TabLayoutType == eTabLayoutType.MultilineWithNavigationBox, bPaintArea: false));
			}
			TabStripOffice2007DocumentDisplay tabStripOffice2007DocumentDisplay = new TabStripOffice2007DocumentDisplay();
			tabStripOffice2007DocumentDisplay.CloseButtonOnTabs = m_CloseButtonOnTabs;
			tabStripOffice2007DocumentDisplay.Paint(e.Graphics, this);
		}
		else
		{
			PaintFlat(e.Graphics);
		}
		e.Graphics.SmoothingMode = smoothingMode;
		e.Graphics.TextRenderingHint = textRenderingHint;
	}

	private void PaintFlat(Graphics g)
	{
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0315: Expected O, but got Unknown
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Expected O, but got Unknown
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Expected O, but got Unknown
		//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b3: Expected O, but got Unknown
		//IL_037f: Unknown result type (might be due to invalid IL or missing references)
		//IL_039f: Expected O, but got Unknown
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Expected O, but got Unknown
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Expected O, but got Unknown
		//IL_040d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0433: Expected O, but got Unknown
		//IL_1b12: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b19: Expected O, but got Unknown
		//IL_1bc5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bcc: Expected O, but got Unknown
		//IL_0a92: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a99: Expected O, but got Unknown
		//IL_0811: Unknown result type (might be due to invalid IL or missing references)
		//IL_0818: Expected O, but got Unknown
		//IL_0633: Unknown result type (might be due to invalid IL or missing references)
		//IL_063a: Expected O, but got Unknown
		//IL_1c6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c76: Expected O, but got Unknown
		//IL_0b43: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b4a: Expected O, but got Unknown
		//IL_0ada: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae1: Expected O, but got Unknown
		//IL_1528: Unknown result type (might be due to invalid IL or missing references)
		//IL_152f: Expected O, but got Unknown
		//IL_1254: Unknown result type (might be due to invalid IL or missing references)
		//IL_125b: Expected O, but got Unknown
		//IL_106a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1071: Expected O, but got Unknown
		//IL_1d22: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d29: Expected O, but got Unknown
		//IL_0c40: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c47: Expected O, but got Unknown
		//IL_0bcd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd4: Expected O, but got Unknown
		//IL_15da: Unknown result type (might be due to invalid IL or missing references)
		//IL_15e1: Expected O, but got Unknown
		//IL_1571: Unknown result type (might be due to invalid IL or missing references)
		//IL_1578: Expected O, but got Unknown
		//IL_0991: Unknown result type (might be due to invalid IL or missing references)
		//IL_0998: Expected O, but got Unknown
		//IL_09f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fe: Expected O, but got Unknown
		//IL_08dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e3: Expected O, but got Unknown
		//IL_0755: Unknown result type (might be due to invalid IL or missing references)
		//IL_075c: Expected O, but got Unknown
		//IL_07b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07be: Expected O, but got Unknown
		//IL_06a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a9: Expected O, but got Unknown
		//IL_16cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_16d3: Expected O, but got Unknown
		//IL_1661: Unknown result type (might be due to invalid IL or missing references)
		//IL_1668: Expected O, but got Unknown
		//IL_13ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_13d5: Expected O, but got Unknown
		//IL_131f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1326: Expected O, but got Unknown
		//IL_118e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1195: Expected O, but got Unknown
		//IL_10d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_10e0: Expected O, but got Unknown
		//IL_0948: Unknown result type (might be due to invalid IL or missing references)
		//IL_094f: Expected O, but got Unknown
		//IL_070e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0715: Expected O, but got Unknown
		//IL_1437: Unknown result type (might be due to invalid IL or missing references)
		//IL_143e: Expected O, but got Unknown
		//IL_1387: Unknown result type (might be due to invalid IL or missing references)
		//IL_138e: Expected O, but got Unknown
		//IL_11d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_11da: Expected O, but got Unknown
		//IL_1145: Unknown result type (might be due to invalid IL or missing references)
		//IL_114c: Expected O, but got Unknown
		//IL_0f6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb8: Expected O, but got Unknown
		//IL_0fd8: Unknown result type (might be due to invalid IL or missing references)
		//IL_102e: Expected O, but got Unknown
		//IL_19ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_19f4: Expected O, but got Unknown
		//IL_1a46: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a4d: Expected O, but got Unknown
		TabColorScheme colorScheme = m_ColorScheme;
		if (!colorScheme.TabBackground2.IsEmpty && ((Control)this).Height > 0 && ((Control)this).Width > 0)
		{
			LinearGradientBrush val = BarFunctions.CreateLinearGradientBrush(((Control)this).ClientRectangle, colorScheme.TabBackground, colorScheme.TabBackground2, colorScheme.TabBackgroundGradientAngle);
			try
			{
				g.FillRectangle((Brush)(object)val, ((Control)this).ClientRectangle);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		else
		{
			SolidBrush val2 = new SolidBrush(colorScheme.TabBackground);
			try
			{
				g.FillRectangle((Brush)(object)val2, ((Control)this).DisplayRectangle);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		if (m_Style != eTabStripStyle.Metro && !colorScheme.TabBorder.IsEmpty)
		{
			Pen val3 = new Pen(colorScheme.TabBorder, 1f);
			try
			{
				g.DrawRectangle(val3, new Rectangle(((Control)this).ClientRectangle.X, ((Control)this).ClientRectangle.Y, ((Control)this).ClientRectangle.Width - 1, ((Control)this).ClientRectangle.Height - 1));
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		Rectangle displayRectangle = ((Control)this).DisplayRectangle;
		switch (m_Alignment)
		{
		case eTabStripAlignment.Bottom:
			displayRectangle.Height -= 3;
			break;
		case eTabStripAlignment.Top:
			displayRectangle.Y++;
			displayRectangle.Height -= 3;
			break;
		}
		TabItem selectedTab = SelectedTab;
		eTextFormat format = eTextFormat.EndEllipsis | eTextFormat.HorizontalCenter | eTextFormat.SingleLine | eTextFormat.VerticalCenter;
		int num = ((!colorScheme.TabBorder.IsEmpty) ? 1 : 0);
		if (m_Style != eTabStripStyle.Metro)
		{
			switch (m_Alignment)
			{
			case eTabStripAlignment.Top:
				if (!(((Control)this).Parent is TabControl))
				{
					g.FillRectangle((Brush)new SolidBrush(colorScheme.TabItemSelectedBackground2.IsEmpty ? colorScheme.TabItemSelectedBackground : colorScheme.TabItemSelectedBackground2), displayRectangle.X + num, displayRectangle.Bottom, displayRectangle.Width - num * 2, ((Control)this).Height - displayRectangle.Bottom);
				}
				else
				{
					Pen val5 = new Pen(colorScheme.TabItemSelectedBorder, 1f);
					try
					{
						g.DrawLine(val5, displayRectangle.X, displayRectangle.Bottom, displayRectangle.X, ((Control)this).ClientRectangle.Bottom);
						g.DrawLine(val5, displayRectangle.Right - 1, displayRectangle.Bottom, displayRectangle.Right - 1, ((Control)this).ClientRectangle.Bottom);
					}
					finally
					{
						((IDisposable)val5)?.Dispose();
					}
				}
				g.DrawLine(new Pen(colorScheme.TabItemSelectedBorderLight, 1f), displayRectangle.X + num, displayRectangle.Bottom, displayRectangle.Right - 1 - num, displayRectangle.Bottom);
				displayRectangle.Inflate(-4, 0);
				break;
			case eTabStripAlignment.Left:
				g.DrawLine(new Pen(colorScheme.TabItemSelectedBorderLight, 1f), displayRectangle.Right - 1, displayRectangle.Y + num, displayRectangle.Right - 1, displayRectangle.Bottom - num);
				displayRectangle.Inflate(0, -4);
				break;
			case eTabStripAlignment.Right:
				g.DrawLine(new Pen(colorScheme.TabItemSelectedBorderLight, 1f), displayRectangle.X, displayRectangle.Y + num, displayRectangle.X, displayRectangle.Bottom - num);
				displayRectangle.Inflate(0, -4);
				break;
			default:
				if (!(((Control)this).Parent is TabControl))
				{
					g.FillRectangle((Brush)new SolidBrush(colorScheme.TabItemSelectedBackground), displayRectangle.X, 0, displayRectangle.Width, displayRectangle.Y);
				}
				else
				{
					Pen val4 = new Pen(colorScheme.TabItemSelectedBorder, 1f);
					try
					{
						g.DrawLine(val4, displayRectangle.X, displayRectangle.Y, displayRectangle.X, 0);
						g.DrawLine(val4, displayRectangle.Right - 1, displayRectangle.Y, displayRectangle.Right - 1, 0);
					}
					finally
					{
						((IDisposable)val4)?.Dispose();
					}
				}
				g.DrawLine(new Pen(colorScheme.TabItemSelectedBorderLight, 1f), displayRectangle.X, displayRectangle.Y, displayRectangle.Right, displayRectangle.Y);
				displayRectangle.Inflate(-4, 0);
				break;
			}
		}
		if (HasNavigationBox && m_TabSystemBox.Visible)
		{
			if (m_Alignment == eTabStripAlignment.Right || m_Alignment == eTabStripAlignment.Left)
			{
				if (m_TabSystemBox.DisplayRectangle.Height > 0)
				{
					displayRectangle.Height -= m_TabSystemBox.DisplayRectangle.Height;
				}
				else
				{
					displayRectangle.Height -= m_TabSystemBox.DefaultWidth;
				}
			}
			else
			{
				if (m_TabSystemBox.DisplayRectangle.Width > 0)
				{
					displayRectangle.Width -= m_TabSystemBox.DisplayRectangle.Width;
				}
				else
				{
					displayRectangle.Width -= m_TabSystemBox.DefaultWidth;
				}
				if (IsRightToLeft)
				{
					displayRectangle.X += m_TabSystemBox.DefaultWidth;
				}
			}
		}
		if (m_NeedRecalcSize)
		{
			RecalcSize(g, GetTabClientArea(((Control)this).DisplayRectangle, bExcludeSystemBox: false, bPaintArea: false));
		}
		if (m_Alignment == eTabStripAlignment.Top)
		{
			displayRectangle.Height++;
		}
		Rectangle rectangle = displayRectangle;
		g.SetClip(displayRectangle);
		Rectangle rectangle2 = Rectangle.Empty;
		foreach (TabItem tab in m_Tabs)
		{
			if (!tab.Visible || !displayRectangle.IntersectsWith(tab.DisplayRectangle))
			{
				continue;
			}
			if (HasPreRenderTabItem)
			{
				RenderTabItemEventArgs renderTabItemEventArgs = new RenderTabItemEventArgs(tab, g);
				InvokePreRenderTabItem(renderTabItemEventArgs);
				if (renderTabItemEventArgs.Cancel)
				{
					continue;
				}
			}
			Rectangle tabRect = tab.DisplayRectangle;
			TabColors tabColors = GetTabColors(tab);
			if (m_Alignment == eTabStripAlignment.Left || m_Alignment == eTabStripAlignment.Right)
			{
				if (tab == selectedTab)
				{
					rectangle2 = tabRect;
					if (m_Alignment == eTabStripAlignment.Left)
					{
						if (tabColors.BackColor2.IsEmpty)
						{
							SolidBrush val6 = new SolidBrush(g.GetNearestColor(tabColors.BackColor));
							try
							{
								g.FillRectangle((Brush)(object)val6, tabRect);
							}
							finally
							{
								((IDisposable)val6)?.Dispose();
							}
						}
						else
						{
							LinearGradientBrush val7 = BarFunctions.CreateLinearGradientBrush(tabRect, tabColors.BackColor, tabColors.BackColor2, tabColors.BackColorGradientAngle);
							try
							{
								g.FillRectangle((Brush)(object)val7, tabRect);
							}
							finally
							{
								((IDisposable)val7)?.Dispose();
							}
						}
						if (m_Style == eTabStripStyle.Metro)
						{
							Pen val8 = new Pen(tabColors.BorderColor, 1f);
							try
							{
								g.DrawLine(val8, tabRect.X, tabRect.Y, tabRect.Right - 1, tabRect.Y);
								g.DrawLine(val8, tabRect.X, tabRect.Bottom - 1, tabRect.Right - 1, tabRect.Bottom - 1);
							}
							finally
							{
								((IDisposable)val8)?.Dispose();
							}
							SolidBrush val9 = new SolidBrush(tabColors.TextColor);
							try
							{
								g.FillRectangle((Brush)(object)val9, new Rectangle(tabRect.X, tabRect.Y, 2, tabRect.Height));
							}
							finally
							{
								((IDisposable)val9)?.Dispose();
							}
						}
						else
						{
							Pen val10 = new Pen(tabColors.LightBorderColor, 1f);
							g.DrawLine(val10, tabRect.X, tabRect.Y, tabRect.Right, tabRect.Y);
							g.DrawLine(val10, tabRect.X, tabRect.Y, tabRect.X, tabRect.Bottom);
							val10.Dispose();
							val10 = new Pen(tabColors.BorderColor, 1f);
							g.DrawLine(val10, tabRect.X + 1, tabRect.Bottom, tabRect.Right - 2, tabRect.Bottom);
							val10.Dispose();
						}
					}
					else
					{
						if (tabColors.BackColor2.IsEmpty)
						{
							SolidBrush val11 = new SolidBrush(g.GetNearestColor(tabColors.BackColor));
							try
							{
								g.FillRectangle((Brush)(object)val11, tabRect.X, tabRect.Y, tabRect.Width, tabRect.Height + 1);
							}
							finally
							{
								((IDisposable)val11)?.Dispose();
							}
						}
						else
						{
							LinearGradientBrush val12 = BarFunctions.CreateLinearGradientBrush(new Rectangle(tabRect.X, tabRect.Y, tabRect.Width, tabRect.Height + 1), tabColors.BackColor, tabColors.BackColor2, tabColors.BackColorGradientAngle);
							try
							{
								g.FillRectangle((Brush)(object)val12, tabRect.X, tabRect.Y, tabRect.Width, tabRect.Height + 1);
							}
							finally
							{
								((IDisposable)val12)?.Dispose();
							}
						}
						if (m_Style == eTabStripStyle.Metro)
						{
							Pen val13 = new Pen(tabColors.BorderColor, 1f);
							try
							{
								g.DrawLine(val13, tabRect.X, tabRect.Y, tabRect.Right - 1, tabRect.Y);
								g.DrawLine(val13, tabRect.X, tabRect.Bottom - 1, tabRect.Right - 1, tabRect.Bottom - 1);
							}
							finally
							{
								((IDisposable)val13)?.Dispose();
							}
							SolidBrush val14 = new SolidBrush(tabColors.TextColor);
							try
							{
								g.FillRectangle((Brush)(object)val14, new Rectangle(tabRect.Right - 2, tabRect.Y, 2, tabRect.Height));
							}
							finally
							{
								((IDisposable)val14)?.Dispose();
							}
						}
						else
						{
							Pen val15 = new Pen(tabColors.LightBorderColor, 1f);
							g.DrawLine(val15, tabRect.X, tabRect.Y, tabRect.Right, tabRect.Y);
							g.DrawLine(val15, tabRect.Right - 1, tabRect.Y, tabRect.Right - 1, tabRect.Bottom);
							val15.Dispose();
							val15 = new Pen(tabColors.BorderColor, 1f);
							g.DrawLine(val15, tabRect.X, tabRect.Bottom, tabRect.Right - 3, tabRect.Bottom);
							val15.Dispose();
						}
					}
				}
				else
				{
					if (!tabColors.BackColor.IsEmpty)
					{
						if (!tabColors.BackColor2.IsEmpty)
						{
							LinearGradientBrush val16 = BarFunctions.CreateLinearGradientBrush(tabRect, tabColors.BackColor, tabColors.BackColor2, tabColors.BackColorGradientAngle);
							try
							{
								g.FillRectangle((Brush)(object)val16, tabRect);
							}
							finally
							{
								((IDisposable)val16)?.Dispose();
							}
						}
						else
						{
							SolidBrush val17 = new SolidBrush(tabColors.BackColor);
							try
							{
								g.FillRectangle((Brush)(object)val17, tabRect);
							}
							finally
							{
								((IDisposable)val17)?.Dispose();
							}
						}
					}
					if (!tabColors.LightBorderColor.IsEmpty)
					{
						if (m_Alignment == eTabStripAlignment.Left)
						{
							Pen val18 = new Pen(tabColors.LightBorderColor, 1f);
							try
							{
								g.DrawLine(val18, tabRect.X, tabRect.Y, tabRect.Right, tabRect.Y);
								g.DrawLine(val18, tabRect.X, tabRect.Y, tabRect.X, tabRect.Bottom);
							}
							finally
							{
								((IDisposable)val18)?.Dispose();
							}
						}
						else
						{
							Pen val19 = new Pen(tabColors.LightBorderColor, 1f);
							try
							{
								g.DrawLine(val19, tabRect.X, tabRect.Y, tabRect.Right, tabRect.Y);
								g.DrawLine(val19, tabRect.Right - 1, tabRect.Y, tabRect.Right - 1, tabRect.Bottom);
							}
							finally
							{
								((IDisposable)val19)?.Dispose();
							}
						}
					}
					if (!tabColors.BorderColor.IsEmpty)
					{
						if (m_Alignment == eTabStripAlignment.Left)
						{
							Pen val20 = new Pen(tabColors.BorderColor, 1f);
							try
							{
								g.DrawLine(val20, tabRect.X + 1, tabRect.Bottom, tabRect.Right - 2, tabRect.Bottom);
								g.DrawLine(val20, tabRect.Right - 1, tabRect.Bottom, tabRect.Right - 1, tabRect.Y + 1);
							}
							finally
							{
								((IDisposable)val20)?.Dispose();
							}
						}
						else
						{
							Pen val21 = new Pen(tabColors.BorderColor, 1f);
							try
							{
								g.DrawLine(val21, tabRect.X, tabRect.Y + 1, tabRect.X, tabRect.Bottom);
								g.DrawLine(val21, tabRect.X + 1, tabRect.Bottom, tabRect.Right - 2, tabRect.Bottom);
							}
							finally
							{
								((IDisposable)val21)?.Dispose();
							}
						}
					}
				}
				if (m_CloseButtonOnTabs && tab.CloseButtonVisible)
				{
					tab.CloseButtonBounds = PaintTabItemCloseButton(g, vertical: true, tab.CloseButtonMouseOver, tab == m_HotTab || tab == SelectedTab, ref tabRect);
				}
				else
				{
					tab.CloseButtonBounds = Rectangle.Empty;
				}
				Image image = tab.GetImage();
				Icon icon = tab.Icon;
				if ((image != null && image.Width + 4 <= tabRect.Width) || (icon != null && tab.IconSize.Width + 4 <= tabRect.Width))
				{
					if (icon != null)
					{
						Rectangle rectangle3 = new Rectangle(tabRect.X + (tabRect.Width - tab.IconSize.Width) / 2, tabRect.Y + 4, tab.IconSize.Width, tab.IconSize.Height);
						if (rectangle.Contains(rectangle3))
						{
							g.DrawIcon(icon, rectangle3);
						}
						tabRect.Y += tab.IconSize.Height + 2;
						tabRect.Height -= tab.IconSize.Height + 2;
					}
					else if (image != null)
					{
						g.DrawImage(image, tabRect.X + (tabRect.Width - image.Width) / 2, tabRect.Y + 4, image.Width, image.Height);
						tabRect.Y += image.Height + 2;
						tabRect.Height -= image.Height + 2;
					}
					tabRect.Inflate(0, -1);
					tabRect.Height -= 4;
					tabRect.Y += 3;
				}
				else
				{
					tabRect.Y += 2;
					tabRect.Height -= 2;
				}
				g.RotateTransform(90f);
				if (!m_DisplaySelectedTextOnly || tab == m_SelectedTab)
				{
					Font font = ((Control)this).Font;
					if (tab == selectedTab && m_SelectedTabFont != null)
					{
						font = m_SelectedTabFont;
					}
					Rectangle rectangle4 = new Rectangle(tabRect.Top, -tabRect.Right, tabRect.Height, tabRect.Width);
					if (rectangle4.Height > MIN_TEXT_WIDTH)
					{
						TextDrawing.DrawStringLegacy(g, tab.Text, font, tabColors.TextColor, rectangle4, format);
					}
					if (m_ShowFocusRectangle && ((Control)this).Focused && tab == m_SelectedTab)
					{
						ControlPaint.DrawFocusRectangle(g, GetFocusRectangle(rectangle4));
					}
					g.ResetTransform();
				}
				if (tab != selectedTab)
				{
					g.DrawLine(new Pen(colorScheme.TabItemSeparator, 1f), tab.DisplayRectangle.X + 1, tab.DisplayRectangle.Bottom, tab.DisplayRectangle.Right - 4, tab.DisplayRectangle.Bottom);
					if (!colorScheme.TabItemSeparatorShade.IsEmpty)
					{
						g.DrawLine(new Pen(colorScheme.TabItemSeparatorShade, 1f), tab.DisplayRectangle.X + 1 + 1, tab.DisplayRectangle.Bottom + 1, tab.DisplayRectangle.Right - 4 + 1, tab.DisplayRectangle.Bottom + 1);
					}
				}
			}
			else
			{
				if (tab == selectedTab)
				{
					rectangle2 = tabRect;
					if (m_Alignment == eTabStripAlignment.Bottom)
					{
						if (tabColors.BackColor2.IsEmpty)
						{
							SolidBrush val22 = new SolidBrush(g.GetNearestColor(tabColors.BackColor));
							try
							{
								g.FillRectangle((Brush)(object)val22, tabRect);
							}
							finally
							{
								((IDisposable)val22)?.Dispose();
							}
						}
						else
						{
							LinearGradientBrush val23 = BarFunctions.CreateLinearGradientBrush(tabRect, tabColors.BackColor, tabColors.BackColor2, tabColors.BackColorGradientAngle);
							try
							{
								g.FillRectangle((Brush)(object)val23, tabRect);
							}
							finally
							{
								((IDisposable)val23)?.Dispose();
							}
						}
						if (m_Style == eTabStripStyle.Metro)
						{
							Pen val24 = new Pen(tabColors.BorderColor, 1f);
							try
							{
								g.DrawLine(val24, tabRect.X, tabRect.Y, tabRect.X, tabRect.Bottom - 1);
								g.DrawLine(val24, tabRect.Right - 1, tabRect.Y, tabRect.Right - 1, tabRect.Bottom - 1);
							}
							finally
							{
								((IDisposable)val24)?.Dispose();
							}
							SolidBrush val25 = new SolidBrush(tabColors.TextColor);
							try
							{
								g.FillRectangle((Brush)(object)val25, new Rectangle(tabRect.X, tabRect.Bottom - 2, tabRect.Width, 2));
							}
							finally
							{
								((IDisposable)val25)?.Dispose();
							}
						}
						else
						{
							Pen val26 = new Pen(tabColors.LightBorderColor, 1f);
							try
							{
								g.DrawLine(val26, tabRect.X, tabRect.Y, tabRect.X, tabRect.Bottom);
							}
							finally
							{
								((IDisposable)val26)?.Dispose();
							}
							Pen val27 = new Pen(tabColors.BorderColor, 1f);
							try
							{
								g.DrawLine(val27, tabRect.X + 1, tabRect.Bottom, tabRect.Right, tabRect.Bottom);
								g.DrawLine(val27, tabRect.Right, tabRect.Y, tabRect.Right, tabRect.Bottom);
							}
							finally
							{
								((IDisposable)val27)?.Dispose();
							}
						}
					}
					else
					{
						if (tabColors.BackColor2.IsEmpty)
						{
							SolidBrush val28 = new SolidBrush(g.GetNearestColor(tabColors.BackColor));
							try
							{
								g.FillRectangle((Brush)(object)val28, tabRect.X, tabRect.Y, tabRect.Width, tabRect.Height + 1);
							}
							finally
							{
								((IDisposable)val28)?.Dispose();
							}
						}
						else
						{
							LinearGradientBrush val29 = BarFunctions.CreateLinearGradientBrush(new Rectangle(tabRect.X, tabRect.Y, tabRect.Width, tabRect.Height + 1), tabColors.BackColor, tabColors.BackColor2, tabColors.BackColorGradientAngle);
							try
							{
								g.FillRectangle((Brush)(object)val29, tabRect.X, tabRect.Y, tabRect.Width, tabRect.Height + 1);
							}
							finally
							{
								((IDisposable)val29)?.Dispose();
							}
						}
						if (m_Style == eTabStripStyle.Metro)
						{
							Pen val30 = new Pen(tabColors.BorderColor, 1f);
							try
							{
								g.DrawLine(val30, tabRect.X, tabRect.Y, tabRect.X, tabRect.Bottom);
								g.DrawLine(val30, tabRect.Right - 1, tabRect.Y, tabRect.Right - 1, tabRect.Bottom);
							}
							finally
							{
								((IDisposable)val30)?.Dispose();
							}
							SolidBrush val31 = new SolidBrush(tabColors.TextColor);
							try
							{
								g.FillRectangle((Brush)(object)val31, new Rectangle(tabRect.X, tabRect.Y, tabRect.Width, 2));
							}
							finally
							{
								((IDisposable)val31)?.Dispose();
							}
						}
						else
						{
							Pen val32 = new Pen(tabColors.LightBorderColor, 1f);
							try
							{
								g.DrawLine(val32, tabRect.X, tabRect.Y, tabRect.X, tabRect.Bottom);
								g.DrawLine(val32, tabRect.X, tabRect.Y, tabRect.Right, tabRect.Y);
							}
							finally
							{
								((IDisposable)val32)?.Dispose();
							}
							Pen val33 = new Pen(tabColors.BorderColor, 1f);
							try
							{
								g.DrawLine(val33, tabRect.Right, tabRect.Y, tabRect.Right, tabRect.Bottom);
							}
							finally
							{
								((IDisposable)val33)?.Dispose();
							}
						}
					}
				}
				else
				{
					if (!tabColors.BackColor.IsEmpty)
					{
						Rectangle rectangle5 = tabRect;
						rectangle5.Width--;
						rectangle5.X++;
						rectangle5.Height--;
						rectangle5.Y++;
						if (!tabColors.BackColor2.IsEmpty)
						{
							LinearGradientBrush val34 = BarFunctions.CreateLinearGradientBrush(tabRect, tabColors.BackColor, tabColors.BackColor2, tabColors.BackColorGradientAngle);
							try
							{
								g.FillRectangle((Brush)(object)val34, rectangle5);
							}
							finally
							{
								((IDisposable)val34)?.Dispose();
							}
						}
						else
						{
							SolidBrush val35 = new SolidBrush(tabColors.BackColor);
							try
							{
								g.FillRectangle((Brush)(object)val35, rectangle5);
							}
							finally
							{
								((IDisposable)val35)?.Dispose();
							}
						}
					}
					if (!tabColors.LightBorderColor.IsEmpty)
					{
						if (m_Alignment == eTabStripAlignment.Bottom)
						{
							Pen val36 = new Pen(tabColors.LightBorderColor, 1f);
							try
							{
								g.DrawLine(val36, tabRect.X, tabRect.Y, tabRect.Right, tabRect.Y);
								g.DrawLine(val36, tabRect.X, tabRect.Y, tabRect.X, tabRect.Bottom);
							}
							finally
							{
								((IDisposable)val36)?.Dispose();
							}
						}
						else
						{
							Pen val37 = new Pen(tabColors.LightBorderColor, 1f);
							try
							{
								g.DrawLine(val37, tabRect.X, tabRect.Y, tabRect.X, tabRect.Bottom);
								g.DrawLine(val37, tabRect.X, tabRect.Y, tabRect.Right, tabRect.Y);
							}
							finally
							{
								((IDisposable)val37)?.Dispose();
							}
						}
					}
					if (!tabColors.BorderColor.IsEmpty)
					{
						if (m_Alignment == eTabStripAlignment.Bottom)
						{
							Pen val38 = new Pen(tabColors.BorderColor, 1f);
							try
							{
								g.DrawLine(val38, tabRect.X + 1, tabRect.Bottom, tabRect.Right, tabRect.Bottom);
								g.DrawLine(val38, tabRect.Right, tabRect.Y, tabRect.Right, tabRect.Bottom);
							}
							finally
							{
								((IDisposable)val38)?.Dispose();
							}
						}
						else
						{
							Pen val39 = new Pen(tabColors.BorderColor, 1f);
							try
							{
								g.DrawLine(val39, tabRect.Right - 1, tabRect.Y, tabRect.Right - 1, tabRect.Bottom);
								g.DrawLine(val39, tabRect.X, tabRect.Bottom, tabRect.Right, tabRect.Bottom);
							}
							finally
							{
								((IDisposable)val39)?.Dispose();
							}
						}
					}
				}
				if (m_Alignment == eTabStripAlignment.Top)
				{
					tabRect.Offset(0, 1);
				}
				if (m_CloseButtonOnTabs && tab.CloseButtonVisible)
				{
					tab.CloseButtonBounds = PaintTabItemCloseButton(g, vertical: false, tab.CloseButtonMouseOver, tab == m_HotTab || tab == SelectedTab, ref tabRect);
				}
				else
				{
					tab.CloseButtonBounds = Rectangle.Empty;
				}
				Image image2 = tab.GetImage();
				Icon icon2 = tab.Icon;
				if ((image2 != null && image2.Width + 4 < tabRect.Width) || (icon2 != null && tab.IconSize.Width + 4 < tabRect.Width))
				{
					if (icon2 != null)
					{
						Rectangle rectangle6 = new Rectangle(tabRect.X + 4, tabRect.Y + (tabRect.Height - tab.IconSize.Height) / 2, tab.IconSize.Width, tab.IconSize.Height);
						if (rectangle.Contains(rectangle6))
						{
							g.DrawIcon(icon2, rectangle6);
						}
						tabRect.X += tab.IconSize.Width + 2;
						tabRect.Width -= tab.IconSize.Width + 2;
					}
					else if (image2 != null)
					{
						g.DrawImage(image2, tabRect.X + 4, tabRect.Y + (tabRect.Height - image2.Height) / 2, image2.Width, image2.Height);
						tabRect.X += image2.Width + 2;
						tabRect.Width -= image2.Width + 2;
					}
					tabRect.Inflate(0, -1);
					tabRect.Width -= 4;
					tabRect.X += 3;
				}
				else
				{
					tabRect.X += 2;
					tabRect.Width -= 2;
				}
				if (!m_DisplaySelectedTextOnly || tab == m_SelectedTab)
				{
					Font font2 = ((Control)this).Font;
					if (tab == selectedTab && m_SelectedTabFont != null)
					{
						font2 = m_SelectedTabFont;
					}
					if (tabRect.Width > MIN_TEXT_WIDTH)
					{
						TextDrawing.DrawString(g, tab.Text, font2, tabColors.TextColor, tabRect, format);
					}
					if (m_ShowFocusRectangle && ((Control)this).Focused && tab == m_SelectedTab)
					{
						ControlPaint.DrawFocusRectangle(g, GetFocusRectangle(tabRect));
					}
				}
				if (tab != selectedTab)
				{
					Rectangle displayRectangle2 = tab.DisplayRectangle;
					if (IsRightToLeft)
					{
						displayRectangle2.Width -= 2;
					}
					Pen val40 = new Pen(colorScheme.TabItemSeparator, 1f);
					try
					{
						g.DrawLine(val40, displayRectangle2.Right, displayRectangle2.Y + 2, displayRectangle2.Right, displayRectangle2.Bottom - 4);
					}
					finally
					{
						((IDisposable)val40)?.Dispose();
					}
					if (!colorScheme.TabItemSeparatorShade.IsEmpty)
					{
						Pen val41 = new Pen(colorScheme.TabItemSeparatorShade, 1f);
						try
						{
							g.DrawLine(val41, displayRectangle2.Right + 1, displayRectangle2.Y + 2 + 1, displayRectangle2.Right + 1, displayRectangle2.Bottom - 4 + 1);
						}
						finally
						{
							((IDisposable)val41)?.Dispose();
						}
					}
				}
			}
			if (HasPostRenderTabItem)
			{
				RenderTabItemEventArgs e = new RenderTabItemEventArgs(tab, g);
				InvokePostRenderTabItem(e);
			}
		}
		g.ResetClip();
		if (m_Style == eTabStripStyle.Metro && !rectangle2.IsEmpty && !colorScheme.TabBorder.IsEmpty)
		{
			if (m_Alignment == eTabStripAlignment.Top)
			{
				Pen val42 = new Pen(colorScheme.TabBorder, 1f);
				try
				{
					g.DrawLine(val42, ((Control)this).ClientRectangle.X, ((Control)this).ClientRectangle.Bottom - 1, rectangle2.X, ((Control)this).ClientRectangle.Bottom - 1);
					g.DrawLine(val42, rectangle2.Right - 1, ((Control)this).ClientRectangle.Bottom - 1, ((Control)this).ClientRectangle.Right - 1, ((Control)this).ClientRectangle.Bottom - 1);
				}
				finally
				{
					((IDisposable)val42)?.Dispose();
				}
			}
			else if (m_Alignment == eTabStripAlignment.Bottom)
			{
				Pen val43 = new Pen(colorScheme.TabBorder, 1f);
				try
				{
					g.DrawLine(val43, ((Control)this).ClientRectangle.X, ((Control)this).ClientRectangle.Y, rectangle2.X, ((Control)this).ClientRectangle.Y);
					g.DrawLine(val43, rectangle2.Right - 1, ((Control)this).ClientRectangle.Y, ((Control)this).ClientRectangle.Right - 1, ((Control)this).ClientRectangle.Y);
				}
				finally
				{
					((IDisposable)val43)?.Dispose();
				}
			}
			else if (m_Alignment == eTabStripAlignment.Left)
			{
				Pen val44 = new Pen(colorScheme.TabBorder, 1f);
				try
				{
					g.DrawLine(val44, ((Control)this).ClientRectangle.Right - 1, ((Control)this).ClientRectangle.Y, ((Control)this).ClientRectangle.Right - 1, rectangle2.Y);
					g.DrawLine(val44, ((Control)this).ClientRectangle.Right - 1, rectangle2.Bottom - 1, ((Control)this).ClientRectangle.Right - 1, ((Control)this).ClientRectangle.Bottom - 1);
				}
				finally
				{
					((IDisposable)val44)?.Dispose();
				}
			}
			else if (m_Alignment == eTabStripAlignment.Right)
			{
				Pen val45 = new Pen(colorScheme.TabBorder, 1f);
				try
				{
					g.DrawLine(val45, ((Control)this).ClientRectangle.X, ((Control)this).ClientRectangle.Y, ((Control)this).ClientRectangle.X, rectangle2.Y);
					g.DrawLine(val45, ((Control)this).ClientRectangle.X, rectangle2.Bottom - 1, ((Control)this).ClientRectangle.X, ((Control)this).ClientRectangle.Bottom - 1);
				}
				finally
				{
					((IDisposable)val45)?.Dispose();
				}
			}
		}
		if (HasNavigationBox && m_TabSystemBox.Visible)
		{
			g.SetClip(m_TabSystemBox.DisplayRectangle);
			m_TabSystemBox.Paint(g);
			g.ResetClip();
		}
	}

	private Rectangle GetBackgroundArea(Rectangle r)
	{
		switch (m_Alignment)
		{
		case eTabStripAlignment.Bottom:
			r.Y += 3;
			r.Height -= 5;
			break;
		case eTabStripAlignment.Top:
			r.Y++;
			r.Height -= 5;
			break;
		case eTabStripAlignment.Left:
			r.Width -= 5;
			r.X++;
			break;
		case eTabStripAlignment.Right:
			r.Width -= 5;
			r.X += 3;
			break;
		}
		return r;
	}

	internal Rectangle GetTabClientArea(Rectangle r, bool bExcludeSystemBox, bool bPaintArea)
	{
		int singleTabHeight = GetSingleTabHeight();
		if ((m_Style == eTabStripStyle.OneNote || m_Style == eTabStripStyle.VS2005Document) && !IsThemed)
		{
			r = GetBackgroundArea(r);
			switch (m_Alignment)
			{
			case eTabStripAlignment.Top:
				r.Inflate(-4, 0);
				break;
			case eTabStripAlignment.Left:
				r.Inflate(0, -4);
				break;
			case eTabStripAlignment.Right:
				r.Inflate(0, -4);
				break;
			default:
				r.Inflate(-4, 0);
				break;
			}
			if (bExcludeSystemBox && HasNavigationBox)
			{
				if (m_Alignment == eTabStripAlignment.Right || m_Alignment == eTabStripAlignment.Left)
				{
					r.Height -= m_TabSystemBox.DefaultWidth + 3;
				}
				else if (IsRightToLeft)
				{
					r.X += m_TabSystemBox.DefaultWidth + 3;
					r.Width -= m_TabSystemBox.DefaultWidth + 3;
				}
				else
				{
					r.Width -= m_TabSystemBox.DefaultWidth + 3;
				}
			}
			if (!bPaintArea && (m_Style == eTabStripStyle.OneNote || m_Style == eTabStripStyle.VS2005Document) && !IsThemed)
			{
				if (m_Alignment == eTabStripAlignment.Right || m_Alignment == eTabStripAlignment.Left)
				{
					if (m_Alignment == eTabStripAlignment.Right)
					{
						r.Height -= singleTabHeight;
						r.Y += singleTabHeight;
					}
					else
					{
						r.Height -= singleTabHeight;
					}
				}
				else
				{
					r.Width -= singleTabHeight;
					r.X += singleTabHeight;
				}
			}
		}
		else
		{
			switch (m_Alignment)
			{
			case eTabStripAlignment.Bottom:
				if (IsThemed && ((Control)this).Parent is TabControl)
				{
					r.Y += 2;
				}
				r.Height -= 3;
				break;
			case eTabStripAlignment.Top:
				r.Y++;
				r.Height -= 3;
				break;
			case eTabStripAlignment.Left:
				if (IsThemed && ((Control)this).Parent is TabControl)
				{
					r.Width -= 2;
				}
				break;
			case eTabStripAlignment.Right:
				if (IsThemed && ((Control)this).Parent is TabControl)
				{
					r.X += 2;
					r.Width -= 2;
				}
				break;
			}
			if ((m_Style != eTabStripStyle.VS2005 && m_Style != eTabStripStyle.Office2007Document) || IsThemed)
			{
				switch (m_Alignment)
				{
				case eTabStripAlignment.Top:
					r.Inflate(-4, 0);
					break;
				case eTabStripAlignment.Left:
					r.Inflate(0, -4);
					break;
				case eTabStripAlignment.Right:
					r.Inflate(0, -4);
					break;
				default:
					r.Inflate(-4, 0);
					break;
				}
			}
			if (HasNavigationBox && bExcludeSystemBox)
			{
				if (m_Alignment == eTabStripAlignment.Right || m_Alignment == eTabStripAlignment.Left)
				{
					r.Height -= m_TabSystemBox.DefaultWidth;
				}
				else
				{
					r.Width -= m_TabSystemBox.DefaultWidth;
				}
			}
		}
		return r;
	}

	internal void ClipExcludeSystemBox(Graphics g)
	{
		Rectangle systemBoxRectangle = GetSystemBoxRectangle();
		if (!systemBoxRectangle.IsEmpty)
		{
			g.SetClip(systemBoxRectangle, (CombineMode)4);
		}
	}

	public Rectangle GetSystemBoxRectangle()
	{
		if (!m_TabSystemBox.Visible || !HasNavigationBox)
		{
			return Rectangle.Empty;
		}
		Rectangle result = Rectangle.Empty;
		result = ((m_Alignment == eTabStripAlignment.Right || m_Alignment == eTabStripAlignment.Left) ? new Rectangle(0, m_TabSystemBox.DisplayRectangle.Y, ((Control)this).ClientRectangle.Width, ((Control)this).ClientRectangle.Bottom - m_TabSystemBox.DisplayRectangle.Y) : ((!IsRightToLeft) ? new Rectangle(m_TabSystemBox.DisplayRectangle.X, 0, ((Control)this).ClientRectangle.Right - m_TabSystemBox.DisplayRectangle.X, ((Control)this).ClientRectangle.Height) : m_TabSystemBox.DisplayRectangle));
		if (m_Alignment == eTabStripAlignment.Bottom)
		{
			result.Y++;
		}
		else if (m_Alignment == eTabStripAlignment.Top)
		{
			result.Height -= 2;
		}
		return result;
	}

	private void PaintOneNote(Graphics g)
	{
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Expected O, but got Unknown
		//IL_038b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0392: Expected O, but got Unknown
		//IL_0489: Unknown result type (might be due to invalid IL or missing references)
		//IL_0490: Expected O, but got Unknown
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Expected O, but got Unknown
		//IL_0557: Unknown result type (might be due to invalid IL or missing references)
		//IL_055e: Expected O, but got Unknown
		//IL_03da: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e1: Expected O, but got Unknown
		//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d3: Expected O, but got Unknown
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Expected O, but got Unknown
		//IL_059a: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a1: Expected O, but got Unknown
		//IL_08de: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e5: Expected O, but got Unknown
		//IL_11f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_11fa: Expected O, but got Unknown
		//IL_096e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0975: Expected O, but got Unknown
		//IL_127f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1286: Expected O, but got Unknown
		//IL_0ae9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af0: Expected O, but got Unknown
		//IL_09cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d6: Expected O, but got Unknown
		//IL_0a05: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0c: Expected O, but got Unknown
		//IL_13fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_1401: Expected O, but got Unknown
		//IL_12e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_12e7: Expected O, but got Unknown
		//IL_1316: Unknown result type (might be due to invalid IL or missing references)
		//IL_131d: Expected O, but got Unknown
		//IL_0bd1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd8: Expected O, but got Unknown
		//IL_1535: Unknown result type (might be due to invalid IL or missing references)
		//IL_153c: Expected O, but got Unknown
		//IL_15cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_15d4: Expected O, but got Unknown
		//IL_0c73: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c7a: Expected O, but got Unknown
		//IL_0fb2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1000: Expected O, but got Unknown
		//IL_101c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1072: Expected O, but got Unknown
		Rectangle tabClientArea = GetTabClientArea(((Control)this).DisplayRectangle, bExcludeSystemBox: false, bPaintArea: false);
		if (m_NeedRecalcSize)
		{
			RecalcSize(g, GetTabClientArea(((Control)this).DisplayRectangle, bExcludeSystemBox: false, bPaintArea: false));
		}
		if (!m_ColorScheme.TabBackground2.IsEmpty && ((Control)this).Height > 0 && ((Control)this).Width > 0)
		{
			int num = m_ColorScheme.TabBackgroundGradientAngle;
			if (m_Alignment == eTabStripAlignment.Bottom)
			{
				num -= 180;
			}
			else if (m_Alignment == eTabStripAlignment.Left)
			{
				num -= 90;
			}
			else if (m_Alignment == eTabStripAlignment.Right)
			{
				num += 90;
			}
			LinearGradientBrush val = BarFunctions.CreateLinearGradientBrush(((Control)this).DisplayRectangle, m_ColorScheme.TabBackground, m_ColorScheme.TabBackground2, num);
			try
			{
				g.FillRectangle((Brush)(object)val, ((Control)this).DisplayRectangle);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		else
		{
			SolidBrush val2 = new SolidBrush(m_ColorScheme.TabBackground);
			try
			{
				g.FillRectangle((Brush)(object)val2, ((Control)this).DisplayRectangle);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		if (!m_ColorScheme.TabBorder.IsEmpty)
		{
			Pen val3 = new Pen(m_ColorScheme.TabBorder, 1f);
			try
			{
				g.DrawRectangle(val3, ((Control)this).DisplayRectangle);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		TabItem selectedTab = SelectedTab;
		eTextFormat format = eTextFormat.EndEllipsis | eTextFormat.HorizontalCenter | eTextFormat.SingleLine | eTextFormat.VerticalCenter;
		Rectangle backgroundArea = GetBackgroundArea(((Control)this).DisplayRectangle);
		Color color = (m_ColorScheme.TabItemSelectedBackground2.IsEmpty ? m_ColorScheme.TabItemSelectedBackground : m_ColorScheme.TabItemSelectedBackground2);
		if (selectedTab != null && selectedTab.PredefinedColor != 0 && !selectedTab.BackColor2.IsEmpty)
		{
			color = selectedTab.BackColor2;
		}
		switch (m_Alignment)
		{
		case eTabStripAlignment.Top:
		{
			SolidBrush val10 = new SolidBrush(color);
			try
			{
				g.FillRectangle((Brush)(object)val10, backgroundArea.X, backgroundArea.Bottom, backgroundArea.Width, ((Control)this).Height - tabClientArea.Bottom);
			}
			finally
			{
				((IDisposable)val10)?.Dispose();
			}
			if (selectedTab == null)
			{
				break;
			}
			Pen val11 = new Pen(m_ColorScheme.TabItemSelectedBorder, 1f);
			try
			{
				Rectangle displayRectangle = selectedTab.DisplayRectangle;
				displayRectangle.Width += displayRectangle.Height;
				displayRectangle.X -= displayRectangle.Height - 1;
				if (HasNavigationBox && displayRectangle.Right > m_TabSystemBox.DisplayRectangle.X)
				{
					displayRectangle.Width -= displayRectangle.Right - m_TabSystemBox.DisplayRectangle.X + 2;
				}
				g.DrawLine(val11, backgroundArea.X, backgroundArea.Bottom, displayRectangle.X - 1, backgroundArea.Bottom);
				g.DrawLine(val11, displayRectangle.Right + 1, backgroundArea.Bottom, backgroundArea.Right, backgroundArea.Bottom);
				if (((Control)this).Parent is TabControl)
				{
					g.DrawLine(val11, backgroundArea.X, backgroundArea.Bottom, backgroundArea.X, ((Control)this).ClientRectangle.Bottom);
					g.DrawLine(val11, backgroundArea.Right - 1, backgroundArea.Bottom, backgroundArea.Right - 1, ((Control)this).ClientRectangle.Bottom);
				}
			}
			finally
			{
				((IDisposable)val11)?.Dispose();
			}
			break;
		}
		case eTabStripAlignment.Left:
		{
			SolidBrush val6 = new SolidBrush(color);
			try
			{
				g.FillRectangle((Brush)(object)val6, backgroundArea.Right, backgroundArea.Y, ((Control)this).Width - backgroundArea.Width, ((Control)this).Height);
			}
			finally
			{
				((IDisposable)val6)?.Dispose();
			}
			Pen val7 = new Pen(m_ColorScheme.TabItemSelectedBorder, 1f);
			try
			{
				g.DrawLine(val7, backgroundArea.Right - 1, backgroundArea.Y, backgroundArea.Right - 1, backgroundArea.Bottom);
				if (((Control)this).Parent is TabControl)
				{
					g.DrawLine(val7, backgroundArea.Right - 1, backgroundArea.Y, ((Control)this).ClientRectangle.Right, backgroundArea.Y);
					g.DrawLine(val7, backgroundArea.Right - 1, backgroundArea.Bottom - 1, ((Control)this).ClientRectangle.Right, backgroundArea.Bottom - 1);
				}
			}
			finally
			{
				((IDisposable)val7)?.Dispose();
			}
			break;
		}
		case eTabStripAlignment.Right:
		{
			SolidBrush val8 = new SolidBrush(color);
			try
			{
				g.FillRectangle((Brush)(object)val8, 0, 0, ((Control)this).Width - backgroundArea.Width, ((Control)this).Height);
			}
			finally
			{
				((IDisposable)val8)?.Dispose();
			}
			Pen val9 = new Pen(m_ColorScheme.TabItemSelectedBorder, 1f);
			try
			{
				g.DrawLine(val9, backgroundArea.X, backgroundArea.Y, backgroundArea.X, backgroundArea.Bottom);
				if (((Control)this).Parent is TabControl)
				{
					g.DrawLine(val9, backgroundArea.X, backgroundArea.Y, 0, backgroundArea.Y);
					g.DrawLine(val9, backgroundArea.X, backgroundArea.Bottom - 1, 0, backgroundArea.Bottom - 1);
				}
			}
			finally
			{
				((IDisposable)val9)?.Dispose();
			}
			break;
		}
		default:
		{
			SolidBrush val4 = new SolidBrush(color);
			try
			{
				g.FillRectangle((Brush)(object)val4, backgroundArea.X, 0, backgroundArea.Width, backgroundArea.Y);
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
			Pen val5 = new Pen(m_ColorScheme.TabItemSelectedBorder, 1f);
			try
			{
				g.DrawLine(val5, backgroundArea.X, backgroundArea.Y, backgroundArea.Right, backgroundArea.Y);
				if (((Control)this).Parent is TabControl)
				{
					g.DrawLine(val5, backgroundArea.X, backgroundArea.Y, backgroundArea.X, 0);
					g.DrawLine(val5, backgroundArea.Right - 1, backgroundArea.Y, backgroundArea.Right - 1, 0);
				}
			}
			finally
			{
				((IDisposable)val5)?.Dispose();
			}
			break;
		}
		}
		tabClientArea = GetTabClientArea(((Control)this).DisplayRectangle, m_TabSystemBox.Visible, bPaintArea: true);
		Rectangle rectangle = tabClientArea;
		g.SetClip(tabClientArea);
		bool flag = true;
		bool flag2 = m_Tabs.Count > 0;
		int num2 = 0;
		bool flag3 = false;
		if (m_Alignment == eTabStripAlignment.Left || (IsRightToLeft && (m_Alignment == eTabStripAlignment.Top || m_Alignment == eTabStripAlignment.Bottom)))
		{
			num2 = m_Tabs.Count - 1;
			flag3 = true;
		}
		int num3 = -1;
		while (flag2)
		{
			TabItem tabItem = m_Tabs[num2];
			if (flag3)
			{
				num2--;
				if (num2 < 0)
				{
					flag2 = false;
				}
			}
			else
			{
				num2++;
				if (num2 >= m_Tabs.Count)
				{
					flag2 = false;
				}
			}
			if (!tabItem.Visible || !tabClientArea.IntersectsWith(tabItem.DisplayRectangle))
			{
				continue;
			}
			if (HasPreRenderTabItem)
			{
				RenderTabItemEventArgs renderTabItemEventArgs = new RenderTabItemEventArgs(tabItem, g);
				InvokePreRenderTabItem(renderTabItemEventArgs);
				if (renderTabItemEventArgs.Cancel)
				{
					continue;
				}
			}
			TabColors tabColors = GetTabColors(tabItem);
			Rectangle tabRect = tabItem.DisplayRectangle;
			if (m_Alignment == eTabStripAlignment.Right)
			{
				tabRect.Height--;
			}
			if (m_Alignment == eTabStripAlignment.Left || m_Alignment == eTabStripAlignment.Right)
			{
				if (num3 == -1)
				{
					num3 = tabItem.DisplayRectangle.X;
				}
				if (num3 != tabItem.DisplayRectangle.X)
				{
					num3 = tabItem.DisplayRectangle.X;
					flag = true;
				}
				GraphicsPath tabPath = GetTabPath(tabRect, 2, m_Alignment, bCloseFigure: true);
				Region clip = g.Clip;
				if (flag || tabItem == m_SelectedTab)
				{
					RectangleF bounds = tabPath.GetBounds();
					bounds.Width++;
					if (m_Alignment == eTabStripAlignment.Left)
					{
						if (flag && tabItem != m_SelectedTab)
						{
							bounds.Width--;
						}
					}
					else
					{
						bounds.Height++;
						if (flag && tabItem != m_SelectedTab)
						{
							bounds.X++;
						}
					}
					g.SetClip(bounds);
				}
				else if (m_Alignment == eTabStripAlignment.Right)
				{
					g.SetClip(new Rectangle(tabRect.X + 1, tabRect.Y + 1, tabRect.Width + 1, tabRect.Height + 1));
				}
				else
				{
					g.SetClip(new Rectangle(tabRect.X, tabRect.Y, tabRect.Width - 1, tabRect.Height));
				}
				ClipExcludeSystemBox(g);
				if (tabColors.BackColor2.IsEmpty)
				{
					SolidBrush val12 = new SolidBrush(tabColors.BackColor);
					try
					{
						g.FillPath((Brush)(object)val12, tabPath);
					}
					finally
					{
						((IDisposable)val12)?.Dispose();
					}
				}
				else
				{
					LinearGradientBrush val13 = BarFunctions.CreateLinearGradientBrush(tabPath.GetBounds(), tabColors.BackColor, tabColors.BackColor2, (m_Alignment == eTabStripAlignment.Left) ? (tabColors.BackColorGradientAngle - 90) : (tabColors.BackColorGradientAngle + 90));
					try
					{
						g.FillPath((Brush)(object)val13, tabPath);
					}
					finally
					{
						((IDisposable)val13)?.Dispose();
					}
				}
				tabPath = GetTabPath(tabRect, 2, m_Alignment, bCloseFigure: false);
				Pen val14 = new Pen(tabColors.BorderColor, 1f);
				try
				{
					g.DrawPath(val14, tabPath);
				}
				finally
				{
					((IDisposable)val14)?.Dispose();
				}
				if (m_TabSystemBox.Visible && HasNavigationBox && tabPath.GetBounds().IntersectsWith(m_TabSystemBox.DisplayRectangle))
				{
					Region val15 = new Region(tabPath);
					val15.Intersect(m_TabSystemBox.DisplayRectangle);
					RectangleF bounds2 = val15.GetBounds(g);
					val15.Dispose();
					Pen val16 = new Pen(tabColors.BorderColor, 1f);
					try
					{
						val16.DashPattern = new float[2] { 2f, 2f };
						g.DrawLine(val16, bounds2.X, bounds2.Y - 1f, bounds2.Right, bounds2.Y - 1f);
						g.DrawLine(val16, bounds2.X + 2f, bounds2.Y - 2f, bounds2.Right, bounds2.Y - 2f);
					}
					finally
					{
						((IDisposable)val16)?.Dispose();
					}
				}
				else if (tabPath.GetBounds().Y < -1f && tabPath.GetBounds().Bottom > 0f)
				{
					RectangleF bounds3 = tabPath.GetBounds();
					Pen val17 = new Pen(tabColors.BorderColor, 1f);
					try
					{
						val17.DashPattern = new float[2] { 2f, 2f };
						g.DrawLine(val17, bounds3.X, 0f, bounds3.Right, 0f);
						g.DrawLine(val17, bounds3.X + 2f, 1f, bounds3.Right, 1f);
					}
					finally
					{
						((IDisposable)val17)?.Dispose();
					}
				}
				Rectangle r = tabRect;
				if (m_Alignment == eTabStripAlignment.Right)
				{
					r.Offset(-1, 0);
					r.Height--;
				}
				else
				{
					r.Offset(1, 0);
					r.Height--;
					r.Y++;
				}
				tabPath = GetTabPath(r, 1, m_Alignment, bCloseFigure: false);
				Pen val18 = new Pen(tabColors.LightBorderColor, 1f);
				try
				{
					g.DrawPath(val18, tabPath);
				}
				finally
				{
					((IDisposable)val18)?.Dispose();
				}
				if (!tabColors.DarkBorderColor.IsEmpty)
				{
					RectangleF bounds4 = tabPath.GetBounds();
					if (m_Alignment == eTabStripAlignment.Right)
					{
						bounds4.Y = bounds4.Bottom - 1f;
						bounds4.Height = 2f;
					}
					else
					{
						bounds4.Height = 2f;
						bounds4.Width -= 2f;
					}
					g.SetClip(bounds4);
					ClipExcludeSystemBox(g);
					Pen val19 = new Pen(tabColors.DarkBorderColor, 1f);
					try
					{
						g.DrawPath(val19, tabPath);
					}
					finally
					{
						((IDisposable)val19)?.Dispose();
					}
				}
				g.SetClip(clip, (CombineMode)0);
				if (m_CloseButtonOnTabs && tabItem.CloseButtonVisible)
				{
					tabRect.Y += 3;
					tabRect.Height -= 3;
					tabItem.CloseButtonBounds = PaintTabItemCloseButton(g, vertical: true, tabItem.CloseButtonMouseOver, tabItem == m_HotTab || tabItem == SelectedTab, ref tabRect);
				}
				else
				{
					tabItem.CloseButtonBounds = Rectangle.Empty;
				}
				Image image = tabItem.GetImage();
				Icon icon = tabItem.Icon;
				if (icon != null)
				{
					Rectangle rectangle2 = new Rectangle(tabRect.X + (tabRect.Width - tabItem.IconSize.Width) / 2, tabRect.Y + 6, tabItem.IconSize.Width, tabItem.IconSize.Height);
					if (rectangle.Contains(rectangle2))
					{
						g.DrawIcon(icon, rectangle2);
					}
					tabRect.Y += tabItem.IconSize.Height + 2;
					tabRect.Height -= tabItem.IconSize.Height + 2;
				}
				else if (image != null)
				{
					g.DrawImage(image, tabRect.X + (tabRect.Width - image.Width) / 2, tabRect.Y + 6, image.Width, image.Height);
					tabRect.Y += image.Height + 2;
					tabRect.Height -= image.Height + 2;
				}
				tabRect.Inflate(0, -1);
				tabRect.Height -= 4;
				if (m_Style == eTabStripStyle.OneNote)
				{
					tabRect.Y += 3;
				}
				g.RotateTransform(90f);
				if (!m_DisplaySelectedTextOnly || tabItem == m_SelectedTab)
				{
					Font font = ((Control)this).Font;
					if (tabItem == selectedTab && m_SelectedTabFont != null)
					{
						font = m_SelectedTabFont;
					}
					if (tabItem != selectedTab)
					{
						Rectangle rectangle3 = new Rectangle(tabRect.Top, -tabRect.Right, tabRect.Height, tabRect.Width);
						if (rectangle3.Height > MIN_TEXT_WIDTH)
						{
							TextDrawing.DrawStringLegacy(g, tabItem.Text, font, tabColors.TextColor, rectangle3, format);
						}
						if (m_ShowFocusRectangle && ((Control)this).Focused && tabItem == m_SelectedTab)
						{
							ControlPaint.DrawFocusRectangle(g, GetFocusRectangle(rectangle3));
						}
					}
					else
					{
						Rectangle rectangle4 = new Rectangle(tabRect.Top, -tabRect.Right, tabRect.Height, tabRect.Width);
						if (rectangle4.Height > MIN_TEXT_WIDTH)
						{
							TextDrawing.DrawStringLegacy(g, tabItem.Text, font, tabColors.TextColor, rectangle4, format);
						}
						if (m_ShowFocusRectangle && ((Control)this).Focused && tabItem == m_SelectedTab)
						{
							ControlPaint.DrawFocusRectangle(g, GetFocusRectangle(rectangle4));
						}
					}
					g.ResetTransform();
				}
				if (tabItem != selectedTab)
				{
					g.DrawLine(new Pen(m_SeparatorColor, 1f), tabItem.DisplayRectangle.X + 1, tabItem.DisplayRectangle.Bottom, tabItem.DisplayRectangle.Right - 4, tabItem.DisplayRectangle.Bottom);
					if (!m_SeparatorShadeColor.IsEmpty)
					{
						g.DrawLine(new Pen(m_SeparatorShadeColor, 1f), tabItem.DisplayRectangle.X + 1 + 1, tabItem.DisplayRectangle.Bottom + 1, tabItem.DisplayRectangle.Right - 4 + 1, tabItem.DisplayRectangle.Bottom + 1);
					}
				}
			}
			else
			{
				if (num3 == -1)
				{
					num3 = tabItem.DisplayRectangle.Y;
				}
				if (num3 != tabItem.DisplayRectangle.Y)
				{
					num3 = tabItem.DisplayRectangle.Y;
					flag = true;
				}
				GraphicsPath tabPath2 = GetTabPath(tabRect, 2, m_Alignment, bCloseFigure: true);
				Region clip2 = g.Clip;
				if (flag || tabItem == m_SelectedTab)
				{
					Rectangle rectangle5 = tabRect;
					rectangle5.Width += rectangle5.Height;
					rectangle5.X -= rectangle5.Height;
					rectangle5.Width++;
					rectangle5.Height++;
					if (m_Alignment == eTabStripAlignment.Bottom && flag && tabItem != m_SelectedTab)
					{
						rectangle5.Y++;
					}
					g.SetClip(rectangle5, (CombineMode)0);
				}
				else if (m_Alignment == eTabStripAlignment.Top)
				{
					g.SetClip(new Rectangle(tabRect.X + 1, tabRect.Y, tabRect.Width + 1, tabRect.Height), (CombineMode)0);
				}
				else
				{
					g.SetClip(new Rectangle(tabRect.X + 1, tabRect.Y + 1, tabRect.Width + 1, tabRect.Height + 1), (CombineMode)0);
				}
				ClipExcludeSystemBox(g);
				if (tabColors.BackColor2.IsEmpty)
				{
					SolidBrush val20 = new SolidBrush(tabColors.BackColor);
					try
					{
						g.FillPath((Brush)(object)val20, tabPath2);
					}
					finally
					{
						((IDisposable)val20)?.Dispose();
					}
				}
				else
				{
					LinearGradientBrush val21 = BarFunctions.CreateLinearGradientBrush(tabPath2.GetBounds(), tabColors.BackColor, tabColors.BackColor2, (m_Alignment == eTabStripAlignment.Top) ? tabColors.BackColorGradientAngle : (-tabColors.BackColorGradientAngle));
					try
					{
						g.FillPath((Brush)(object)val21, tabPath2);
					}
					finally
					{
						((IDisposable)val21)?.Dispose();
					}
				}
				tabPath2 = GetTabPath(tabRect, 2, m_Alignment, bCloseFigure: false);
				Pen val22 = new Pen(tabColors.BorderColor, 1f);
				try
				{
					g.DrawPath(val22, tabPath2);
				}
				finally
				{
					((IDisposable)val22)?.Dispose();
				}
				if (m_TabSystemBox.Visible && HasNavigationBox && tabPath2.GetBounds().IntersectsWith(m_TabSystemBox.DisplayRectangle))
				{
					Region val23 = new Region(tabPath2);
					val23.Intersect(m_TabSystemBox.DisplayRectangle);
					RectangleF bounds5 = val23.GetBounds(g);
					val23.Dispose();
					Pen val24 = new Pen(tabColors.BorderColor, 1f);
					try
					{
						val24.DashPattern = new float[2] { 2f, 2f };
						g.DrawLine(val24, bounds5.X - 1f, bounds5.Y, bounds5.X - 1f, bounds5.Bottom);
						g.DrawLine(val24, bounds5.X - 2f, bounds5.Y + 2f, bounds5.X - 2f, bounds5.Bottom);
					}
					finally
					{
						((IDisposable)val24)?.Dispose();
					}
				}
				else if (tabPath2.GetBounds().X < -1f && tabPath2.GetBounds().Right > 0f)
				{
					RectangleF bounds6 = tabPath2.GetBounds();
					Pen val25 = new Pen(tabColors.BorderColor, 1f);
					try
					{
						val25.DashPattern = new float[2] { 2f, 2f };
						g.DrawLine(val25, 0f, bounds6.Y, 0f, bounds6.Bottom);
						g.DrawLine(val25, 1f, bounds6.Y + 2f, 1f, bounds6.Bottom);
					}
					finally
					{
						((IDisposable)val25)?.Dispose();
					}
				}
				Rectangle r2 = tabRect;
				if (m_Alignment == eTabStripAlignment.Top)
				{
					r2.Offset(1, 1);
					r2.X--;
					r2.Width--;
				}
				else
				{
					r2.Offset(1, -1);
					r2.X--;
					r2.Width--;
				}
				tabPath2 = GetTabPath(r2, 1, m_Alignment, bCloseFigure: false);
				if (!tabColors.LightBorderColor.IsEmpty)
				{
					RectangleF bounds7 = tabPath2.GetBounds();
					bounds7.Height--;
					Region val26 = g.Clip.Clone();
					g.SetClip(bounds7, (CombineMode)1);
					Pen val27 = new Pen(tabColors.LightBorderColor, 1f);
					try
					{
						g.DrawPath(val27, tabPath2);
					}
					finally
					{
						((IDisposable)val27)?.Dispose();
					}
					g.SetClip(val26, (CombineMode)0);
				}
				if (!tabColors.DarkBorderColor.IsEmpty)
				{
					RectangleF bounds8 = tabPath2.GetBounds();
					bounds8.X = bounds8.Right - 1f;
					bounds8.Width = 2f;
					bounds8.Height--;
					g.SetClip(clip2, (CombineMode)0);
					g.SetClip(bounds8, (CombineMode)1);
					Pen val28 = new Pen(tabColors.DarkBorderColor, 1f);
					try
					{
						g.DrawPath(val28, tabPath2);
					}
					finally
					{
						((IDisposable)val28)?.Dispose();
					}
				}
				g.SetClip(clip2, (CombineMode)0);
				tabRect.Offset(0, 1);
				if (m_CloseButtonOnTabs && tabItem.CloseButtonVisible)
				{
					tabItem.CloseButtonBounds = PaintTabItemCloseButton(g, vertical: false, tabItem.CloseButtonMouseOver, tabItem == m_HotTab || tabItem == SelectedTab, ref tabRect);
				}
				else
				{
					tabItem.CloseButtonBounds = Rectangle.Empty;
				}
				Image image2 = tabItem.GetImage();
				Icon icon2 = tabItem.Icon;
				if (icon2 != null)
				{
					Rectangle rectangle6 = new Rectangle(tabRect.X + tabRect.Height / 3 + ((m_Alignment == eTabStripAlignment.Top) ? (-2) : 0), tabRect.Y + (tabRect.Height - tabItem.IconSize.Height) / 2, tabItem.IconSize.Width, tabItem.IconSize.Height);
					if (rectangle.Contains(rectangle6))
					{
						g.DrawIcon(icon2, rectangle6);
					}
					tabRect.X += tabItem.IconSize.Width + 2;
					tabRect.Width -= tabItem.IconSize.Width + 2;
				}
				else if (image2 != null)
				{
					g.DrawImage(image2, tabRect.X + tabRect.Height / 3 + ((m_Alignment == eTabStripAlignment.Top) ? (-2) : 0), tabRect.Y + (tabRect.Height - image2.Height) / 2, image2.Width, image2.Height);
					tabRect.X += image2.Width + 2;
					tabRect.Width -= image2.Width + 2;
				}
				tabRect.Inflate(0, -1);
				tabRect.Width -= 4;
				if (m_Style == eTabStripStyle.OneNote)
				{
					tabRect.X += 3;
				}
				if (!m_DisplaySelectedTextOnly || tabItem == m_SelectedTab)
				{
					Font font2 = ((Control)this).Font;
					if (tabItem == selectedTab && m_SelectedTabFont != null)
					{
						font2 = m_SelectedTabFont;
					}
					if (tabItem != selectedTab)
					{
						if (tabRect.Width > MIN_TEXT_WIDTH)
						{
							TextDrawing.DrawString(g, tabItem.Text, font2, tabColors.TextColor, tabRect, format);
						}
						if (m_ShowFocusRectangle && ((Control)this).Focused && tabItem == m_SelectedTab)
						{
							ControlPaint.DrawFocusRectangle(g, GetFocusRectangle(tabRect));
						}
					}
					else
					{
						if (tabRect.Width > MIN_TEXT_WIDTH)
						{
							TextDrawing.DrawString(g, tabItem.Text, font2, tabColors.TextColor, tabRect, format);
						}
						if (m_ShowFocusRectangle && ((Control)this).Focused && tabItem == m_SelectedTab)
						{
							ControlPaint.DrawFocusRectangle(g, GetFocusRectangle(tabRect));
						}
					}
				}
			}
			if (HasPostRenderTabItem)
			{
				RenderTabItemEventArgs e = new RenderTabItemEventArgs(tabItem, g);
				InvokePostRenderTabItem(e);
			}
			flag = false;
		}
		g.ResetClip();
		if (HasNavigationBox && m_TabSystemBox.Visible)
		{
			m_TabSystemBox.Paint(g);
		}
	}

	private Rectangle PaintTabItemCloseButton(Graphics g, bool vertical, bool mouseOver, bool isTabHotOrSelected, ref Rectangle tabRect)
	{
		Size closeButtonSize = m_CloseButtonSize;
		bool flag = m_CloseButtonPosition == eTabCloseButtonPosition.Left;
		int num = (flag ? 2 : 4);
		Rectangle empty = Rectangle.Empty;
		empty = (flag ? ((!vertical) ? new Rectangle(tabRect.X + num, tabRect.Y + (tabRect.Height - closeButtonSize.Height) / 2, closeButtonSize.Width, closeButtonSize.Height) : new Rectangle(tabRect.X + (tabRect.Width - closeButtonSize.Width) / 2, tabRect.Y + num, closeButtonSize.Width, closeButtonSize.Height)) : ((!vertical) ? new Rectangle(tabRect.Right - num - closeButtonSize.Width, tabRect.Y + (tabRect.Height - closeButtonSize.Height) / 2, closeButtonSize.Width, closeButtonSize.Height) : new Rectangle(tabRect.X + (tabRect.Width - closeButtonSize.Width) / 2, tabRect.Bottom - num - closeButtonSize.Height, closeButtonSize.Width, closeButtonSize.Height)));
		if (isTabHotOrSelected || m_CloseButtonOnTabsAlwaysDisplayed)
		{
			TabStripBaseDisplay.PaintTabItemCloseButton(g, empty, mouseOver, this);
		}
		if (vertical)
		{
			if (flag)
			{
				tabRect.Y += empty.Height + num;
			}
			tabRect.Height -= empty.Height + num;
		}
		else
		{
			if (flag)
			{
				tabRect.X += empty.Width + num;
			}
			tabRect.Width -= empty.Width + num;
		}
		return empty;
	}

	private GraphicsPath GetTabPath(Rectangle r, int rightCornerSize, eTabStripAlignment align, bool bCloseFigure)
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Expected O, but got Unknown
		//IL_0393: Unknown result type (might be due to invalid IL or missing references)
		//IL_0399: Expected O, but got Unknown
		//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ec: Expected O, but got Unknown
		Rectangle rectangle = r;
		switch (align)
		{
		case eTabStripAlignment.Left:
			rectangle = new Rectangle(r.X, r.Y, r.Height, r.Width);
			break;
		case eTabStripAlignment.Right:
			rectangle = new Rectangle(r.Right - r.Height, r.Y, r.Height, r.Width);
			break;
		}
		GraphicsPath val = new GraphicsPath();
		Point[] array = new Point[4];
		array[0].X = rectangle.X + 2 - (rectangle.Height + 1);
		array[0].Y = rectangle.Bottom - 1;
		array[1].X = array[0].X + 3;
		array[1].Y = array[0].Y - 2;
		array[2].X = array[1].X + rectangle.Height - 6;
		array[2].Y = rectangle.Y + 3;
		array[3].X = array[2].X + 4;
		array[3].Y = rectangle.Y + 1;
		val.AddCurve(array, 0, 3, 0.5f);
		val.AddLine(array[3].X + 1, rectangle.Y, rectangle.Right - rightCornerSize, rectangle.Y);
		val.AddLine(rectangle.Right - rightCornerSize, rectangle.Y, rectangle.Right, rectangle.Y + rightCornerSize);
		val.AddLine(rectangle.Right, rectangle.Y + rightCornerSize, rectangle.Right, rectangle.Bottom - 1);
		if (bCloseFigure)
		{
			val.AddLine(array[0].X, rectangle.Bottom, rectangle.Right, rectangle.Bottom);
			val.CloseAllFigures();
		}
		switch (align)
		{
		case eTabStripAlignment.Bottom:
			val.Dispose();
			val = new GraphicsPath();
			array = new Point[4];
			array[0].X = rectangle.X + 2 - (rectangle.Height + 1);
			array[0].Y = rectangle.Top + 1;
			array[1].X = array[0].X + 3;
			array[1].Y = array[0].Y + 2;
			array[2].X = array[1].X + rectangle.Height - 6;
			array[2].Y = rectangle.Bottom - 3;
			array[3].X = array[2].X + 4;
			array[3].Y = rectangle.Bottom - 1;
			val.AddCurve(array, 0, 3, 0.5f);
			val.AddLine(array[3].X + 1, rectangle.Bottom, rectangle.Right - rightCornerSize, rectangle.Bottom);
			val.AddLine(rectangle.Right - rightCornerSize, rectangle.Bottom, rectangle.Right, rectangle.Bottom - rightCornerSize);
			val.AddLine(rectangle.Right, rectangle.Bottom - rightCornerSize, rectangle.Right, rectangle.Y + 1);
			if (bCloseFigure)
			{
				val.AddLine(array[0].X, rectangle.Y, rectangle.Right, rectangle.Y);
				val.CloseAllFigures();
			}
			break;
		case eTabStripAlignment.Left:
		{
			Matrix val3 = new Matrix();
			val3.RotateAt(-90f, new PointF(rectangle.X, rectangle.Bottom));
			val3.Translate((float)rectangle.Height, (float)(rectangle.Width - rectangle.Height), (MatrixOrder)1);
			val.Transform(val3);
			break;
		}
		case eTabStripAlignment.Right:
		{
			Matrix val2 = new Matrix();
			val2.RotateAt(90f, new PointF(rectangle.Right, rectangle.Bottom));
			val2.Translate((float)(-rectangle.Height), (float)(rectangle.Width - (rectangle.Height - 1)), (MatrixOrder)1);
			val.Transform(val2);
			break;
		}
		}
		return val;
	}

	private void PaintThemed(Graphics g)
	{
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Expected O, but got Unknown
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Expected O, but got Unknown
		//IL_060d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0614: Expected O, but got Unknown
		//IL_04d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d8: Expected O, but got Unknown
		//IL_04e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ed: Expected O, but got Unknown
		if (m_ThemeTab == null)
		{
			RefreshThemes();
		}
		if (m_ColorScheme.TabBackground.IsEmpty)
		{
			m_ThemeTab.DrawBackground(g, ThemeTabParts.Body, ThemeTabStates.BodyNormal, ((Control)this).ClientRectangle);
		}
		else if (!m_ColorScheme.TabBackground2.IsEmpty && ((Control)this).Width > 0 && ((Control)this).Height > 0)
		{
			LinearGradientBrush val = BarFunctions.CreateLinearGradientBrush(((Control)this).ClientRectangle, m_ColorScheme.TabBackground, m_ColorScheme.TabBackground2, m_ColorScheme.TabBackgroundGradientAngle);
			try
			{
				g.FillRectangle((Brush)(object)val, ((Control)this).ClientRectangle);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		else
		{
			SolidBrush val2 = new SolidBrush(m_ColorScheme.TabBackground);
			try
			{
				g.FillRectangle((Brush)(object)val2, ((Control)this).DisplayRectangle);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		Rectangle tabClientArea = GetTabClientArea(((Control)this).DisplayRectangle, bExcludeSystemBox: false, bPaintArea: false);
		TabItem selectedTab = SelectedTab;
		eTextFormat strFormat = eTextFormat.EndEllipsis | eTextFormat.HorizontalCenter | eTextFormat.SingleLine | eTextFormat.VerticalCenter;
		if (m_NeedRecalcSize)
		{
			RecalcSize(g, tabClientArea);
		}
		if (((Control)this).Parent is TabControl)
		{
			if (m_Alignment == eTabStripAlignment.Top)
			{
				m_ThemeTab.DrawBackground(g, ThemeTabParts.Pane, ThemeTabStates.PaneNormal, new Rectangle(0, tabClientArea.Bottom, ((Control)this).ClientRectangle.Width, ((Control)this).ClientRectangle.Height));
			}
			else
			{
				Size size = new Size(((Control)this).Width, 5);
				if (m_Alignment == eTabStripAlignment.Left || m_Alignment == eTabStripAlignment.Right)
				{
					size = new Size(((Control)this).Height, 5);
				}
				Bitmap val3 = new Bitmap(size.Width, size.Height, g);
				try
				{
					Graphics val4 = Graphics.FromImage((Image)(object)val3);
					try
					{
						m_ThemeTab.DrawBackground(val4, ThemeTabParts.Pane, ThemeTabStates.PaneNormal, new Rectangle(0, 0, size.Width, size.Height));
					}
					finally
					{
						val4.Dispose();
					}
					if (m_Alignment == eTabStripAlignment.Left)
					{
						((Image)val3).RotateFlip((RotateFlipType)3);
						g.DrawImageUnscaled((Image)(object)val3, tabClientArea.Right, 0);
					}
					else if (m_Alignment == eTabStripAlignment.Right)
					{
						((Image)val3).RotateFlip((RotateFlipType)1);
						g.DrawImageUnscaled((Image)(object)val3, tabClientArea.X - ((Image)val3).Width, 0);
					}
					else if (m_Alignment == eTabStripAlignment.Bottom)
					{
						((Image)val3).RotateFlip((RotateFlipType)2);
						g.DrawImageUnscaled((Image)(object)val3, 0, tabClientArea.Y - ((Image)val3).Height);
					}
				}
				finally
				{
					((Image)val3).Dispose();
				}
			}
		}
		if (m_Alignment == eTabStripAlignment.Top)
		{
			tabClientArea.Height++;
		}
		Rectangle clip = tabClientArea;
		if (m_Alignment == eTabStripAlignment.Bottom)
		{
			clip.Y--;
			clip.Height++;
		}
		else if (m_Alignment == eTabStripAlignment.Left)
		{
			clip.Width++;
		}
		else if (m_Alignment == eTabStripAlignment.Right)
		{
			clip.X--;
			clip.Width++;
		}
		if (HasNavigationBox)
		{
			if (m_Alignment == eTabStripAlignment.Right || m_Alignment == eTabStripAlignment.Left)
			{
				clip.Height -= m_TabSystemBox.DisplayRectangle.Height;
			}
			else
			{
				clip.Width -= m_TabSystemBox.DisplayRectangle.Width;
			}
		}
		g.SetClip(clip);
		foreach (TabItem tab in m_Tabs)
		{
			if (!tab.Visible || !tabClientArea.IntersectsWith(tab.DisplayRectangle))
			{
				continue;
			}
			if (HasPreRenderTabItem)
			{
				RenderTabItemEventArgs renderTabItemEventArgs = new RenderTabItemEventArgs(tab, g);
				InvokePreRenderTabItem(renderTabItemEventArgs);
				if (renderTabItemEventArgs.Cancel)
				{
					continue;
				}
			}
			Rectangle displayRectangle = tab.DisplayRectangle;
			Rectangle rectangle = new Rectangle(0, 0, displayRectangle.Width, displayRectangle.Height);
			if (m_Alignment == eTabStripAlignment.Left || m_Alignment == eTabStripAlignment.Right)
			{
				rectangle = new Rectangle(0, 0, displayRectangle.Height, displayRectangle.Width);
			}
			if (tab == selectedTab)
			{
				if (m_Alignment == eTabStripAlignment.Top)
				{
					rectangle.Height += 2;
				}
				else if (m_Alignment == eTabStripAlignment.Bottom)
				{
					rectangle.Height += 2;
					displayRectangle.Y--;
				}
				else if (m_Alignment == eTabStripAlignment.Left)
				{
					rectangle.Height += 2;
					displayRectangle.X--;
				}
				else if (m_Alignment == eTabStripAlignment.Right)
				{
					rectangle.Height += 2;
					displayRectangle.X--;
				}
			}
			Bitmap val5 = new Bitmap(rectangle.Width, rectangle.Height, g);
			try
			{
				Graphics val6 = Graphics.FromImage((Image)(object)val5);
				try
				{
					SolidBrush val7 = new SolidBrush(Color.Transparent);
					try
					{
						val6.FillRectangle((Brush)(object)val7, 0, 0, ((Image)val5).Width, ((Image)val5).Height);
					}
					finally
					{
						((IDisposable)val7)?.Dispose();
					}
					if (m_Alignment == eTabStripAlignment.Bottom)
					{
						DrawThemedTab(val6, rectangle, tab, strFormat, rectangle, drawBack: true, drawInner: false);
						((Image)val5).RotateFlip((RotateFlipType)2);
						DrawThemedTab(val6, rectangle, tab, strFormat, rectangle, drawBack: false, drawInner: true);
					}
					else
					{
						DrawThemedTab(val6, rectangle, tab, strFormat, rectangle, drawBack: true, drawInner: true);
					}
				}
				finally
				{
					val6.Dispose();
				}
			}
			finally
			{
				if (m_Alignment == eTabStripAlignment.Left)
				{
					((Image)val5).RotateFlip((RotateFlipType)3);
				}
				else if (m_Alignment == eTabStripAlignment.Right)
				{
					((Image)val5).RotateFlip((RotateFlipType)1);
				}
				g.DrawImageUnscaled((Image)(object)val5, displayRectangle);
				((Image)val5).Dispose();
			}
			if (HasPostRenderTabItem)
			{
				RenderTabItemEventArgs e = new RenderTabItemEventArgs(tab, g);
				InvokePostRenderTabItem(e);
			}
		}
		g.ResetClip();
		if (HasNavigationBox && m_TabSystemBox.Visible)
		{
			g.SetClip(m_TabSystemBox.DisplayRectangle);
			SolidBrush val8 = new SolidBrush(m_ColorScheme.TabBackground);
			try
			{
				g.FillRectangle((Brush)(object)val8, m_TabSystemBox.DisplayRectangle);
			}
			finally
			{
				((IDisposable)val8)?.Dispose();
			}
			m_TabSystemBox.Paint(g);
			g.ResetClip();
		}
	}

	private void DrawThemedTab(Graphics g, Rectangle tabRect, TabItem tab, eTextFormat strFormat, Rectangle rClip, bool drawBack, bool drawInner)
	{
		TabColors tabColors = GetTabColors(tab);
		ThemeTabStates state = ThemeTabStates.Normal;
		if (m_HotTab == tab)
		{
			state = ThemeTabStates.Hot;
		}
		else if (tab == m_SelectedTab)
		{
			state = ThemeTabStates.Selected;
		}
		if (drawBack)
		{
			if (tab == m_SelectedTab)
			{
				m_ThemeTab.DrawBackground(g, ThemeTabParts.TabItem, state, new Rectangle(tabRect.X, tabRect.Y, tabRect.Width, tabRect.Height + 1));
			}
			else
			{
				m_ThemeTab.DrawBackground(g, ThemeTabParts.TabItem, state, new Rectangle(tabRect.X, tabRect.Y, tabRect.Width, tabRect.Height + 1));
			}
		}
		tabRect.Offset(0, 1);
		if (!drawInner)
		{
			return;
		}
		if (m_CloseButtonOnTabs && tab.CloseButtonVisible)
		{
			Rectangle closeButtonBounds = PaintTabItemCloseButton(g, m_Alignment == eTabStripAlignment.Left || m_Alignment == eTabStripAlignment.Right, tab.CloseButtonMouseOver, tab == m_HotTab || tab == SelectedTab, ref tabRect);
			closeButtonBounds.Offset(tab.DisplayRectangle.Location);
			tab.CloseButtonBounds = closeButtonBounds;
		}
		else
		{
			tab.CloseButtonBounds = Rectangle.Empty;
		}
		Image image = tab.GetImage();
		Icon icon = tab.Icon;
		if ((image != null && image.Width + 4 <= tabRect.Width) || (icon != null && tab.IconSize.Width + 4 <= tabRect.Width))
		{
			if (icon != null)
			{
				Rectangle rectangle = new Rectangle(tabRect.X + 4, tabRect.Y + (tabRect.Height - tab.IconSize.Height) / 2, tab.IconSize.Width, tab.IconSize.Height);
				if (rClip.Contains(rectangle))
				{
					g.DrawIcon(icon, rectangle);
				}
				tabRect.X += tab.IconSize.Width + 2;
				tabRect.Width -= tab.IconSize.Width + 2;
			}
			else if (image != null)
			{
				g.DrawImage(image, tabRect.X + 4, tabRect.Y + (tabRect.Height - image.Height) / 2, image.Width, image.Height);
				tabRect.X += image.Width + 2;
				tabRect.Width -= image.Width + 2;
			}
			tabRect.Inflate(0, -1);
			tabRect.Width -= 4;
			tabRect.X += 3;
		}
		if (!m_DisplaySelectedTextOnly || tab == m_SelectedTab)
		{
			Font font = ((Control)this).Font;
			if (tab == m_SelectedTab && m_SelectedTabFont != null)
			{
				font = m_SelectedTabFont;
			}
			if (tabRect.Width > MIN_TEXT_WIDTH)
			{
				TextDrawing.DrawString(g, tab.Text, font, tabColors.TextColor, tabRect, strFormat);
			}
			if (m_ShowFocusRectangle && ((Control)this).Focused && tab == m_SelectedTab)
			{
				ControlPaint.DrawFocusRectangle(g, GetFocusRectangle(tabRect));
			}
		}
	}

	private void PaintVS2005(Graphics g)
	{
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Expected O, but got Unknown
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Expected O, but got Unknown
		//IL_034c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0353: Expected O, but got Unknown
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Expected O, but got Unknown
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Expected O, but got Unknown
		//IL_052c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0533: Expected O, but got Unknown
		//IL_0d55: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d5c: Expected O, but got Unknown
		//IL_0814: Unknown result type (might be due to invalid IL or missing references)
		//IL_081b: Expected O, but got Unknown
		//IL_0888: Unknown result type (might be due to invalid IL or missing references)
		//IL_088f: Expected O, but got Unknown
		if (!m_ColorScheme.TabBackground2.IsEmpty && ((Control)this).Width > 0 && ((Control)this).Height > 0)
		{
			LinearGradientBrush val = BarFunctions.CreateLinearGradientBrush(((Control)this).ClientRectangle, m_ColorScheme.TabBackground, m_ColorScheme.TabBackground2, m_ColorScheme.TabBackgroundGradientAngle);
			try
			{
				g.FillRectangle((Brush)(object)val, ((Control)this).ClientRectangle);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		else
		{
			SolidBrush val2 = new SolidBrush(m_ColorScheme.TabBackground);
			try
			{
				g.FillRectangle((Brush)(object)val2, ((Control)this).DisplayRectangle);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		Rectangle tabClientArea = GetTabClientArea(((Control)this).DisplayRectangle, bExcludeSystemBox: false, bPaintArea: false);
		Rectangle displayRectangle = ((Control)this).DisplayRectangle;
		if (m_NeedRecalcSize)
		{
			RecalcSize(g, tabClientArea);
		}
		TabItem selectedTab = SelectedTab;
		Rectangle rectangle = Rectangle.Empty;
		if (selectedTab != null)
		{
			rectangle = selectedTab.DisplayRectangle;
		}
		eTextFormat format = eTextFormat.EndEllipsis | eTextFormat.HorizontalCenter | eTextFormat.SingleLine | eTextFormat.VerticalCenter;
		switch (m_Alignment)
		{
		case eTabStripAlignment.Top:
		{
			Pen val6 = new Pen(m_ColorScheme.TabItemSelectedBorder, 1f);
			try
			{
				if (!rectangle.IsEmpty)
				{
					g.DrawLine(val6, displayRectangle.X, tabClientArea.Bottom, rectangle.X, tabClientArea.Bottom);
					g.DrawLine(val6, rectangle.Right - 1, tabClientArea.Bottom, displayRectangle.Right, tabClientArea.Bottom);
				}
				else
				{
					g.DrawLine(val6, displayRectangle.X, displayRectangle.Bottom - 1, displayRectangle.Right, displayRectangle.Bottom - 1);
				}
			}
			finally
			{
				((IDisposable)val6)?.Dispose();
			}
			break;
		}
		case eTabStripAlignment.Bottom:
		{
			Pen val4 = new Pen(m_ColorScheme.TabItemSelectedBorder, 1f);
			try
			{
				if (!rectangle.IsEmpty)
				{
					g.DrawLine(val4, displayRectangle.X, displayRectangle.Y, rectangle.X, displayRectangle.Y);
					g.DrawLine(val4, rectangle.Right - 1, displayRectangle.Y, displayRectangle.Right, displayRectangle.Y);
				}
				else
				{
					g.DrawLine(val4, displayRectangle.X, displayRectangle.Y, displayRectangle.Right, displayRectangle.Y);
				}
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
			tabClientArea.Y++;
			tabClientArea.Height--;
			break;
		}
		case eTabStripAlignment.Left:
		{
			Pen val5 = new Pen(m_ColorScheme.TabItemSelectedBorder, 1f);
			try
			{
				if (!rectangle.IsEmpty)
				{
					g.DrawLine(val5, displayRectangle.Right - 1, displayRectangle.Y, displayRectangle.Right - 1, rectangle.Y);
					g.DrawLine(val5, displayRectangle.Right - 1, rectangle.Bottom - 1, displayRectangle.Right - 1, displayRectangle.Bottom);
				}
				else
				{
					g.DrawLine(val5, displayRectangle.Right - 1, displayRectangle.Y, displayRectangle.Right - 1, displayRectangle.Bottom);
				}
			}
			finally
			{
				((IDisposable)val5)?.Dispose();
			}
			tabClientArea.Width--;
			break;
		}
		case eTabStripAlignment.Right:
		{
			Pen val3 = new Pen(m_ColorScheme.TabItemSelectedBorder, 1f);
			try
			{
				if (!rectangle.IsEmpty)
				{
					g.DrawLine(val3, displayRectangle.X, displayRectangle.Y, displayRectangle.X, rectangle.Y);
					g.DrawLine(val3, displayRectangle.X, rectangle.Bottom - 1, displayRectangle.X, displayRectangle.Bottom);
				}
				else
				{
					g.DrawLine(val3, displayRectangle.X, displayRectangle.Y, displayRectangle.X, displayRectangle.Bottom);
				}
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
			tabClientArea.Width--;
			tabClientArea.X++;
			break;
		}
		}
		if (HasNavigationBox && m_TabSystemBox.Visible)
		{
			if (m_Alignment == eTabStripAlignment.Right || m_Alignment == eTabStripAlignment.Left)
			{
				tabClientArea.Height -= m_TabSystemBox.DisplayRectangle.Height;
			}
			else
			{
				tabClientArea.Width -= m_TabSystemBox.DisplayRectangle.Width;
				if (IsRightToLeft)
				{
					tabClientArea.X += m_TabSystemBox.DisplayRectangle.Width;
				}
			}
		}
		Rectangle clip = tabClientArea;
		if (m_Alignment == eTabStripAlignment.Right)
		{
			clip.Width++;
		}
		g.SetClip(clip);
		foreach (TabItem tab in m_Tabs)
		{
			if (!tab.Visible || !tabClientArea.IntersectsWith(tab.DisplayRectangle))
			{
				continue;
			}
			if (HasPreRenderTabItem)
			{
				RenderTabItemEventArgs renderTabItemEventArgs = new RenderTabItemEventArgs(tab, g);
				InvokePreRenderTabItem(renderTabItemEventArgs);
				if (renderTabItemEventArgs.Cancel)
				{
					continue;
				}
			}
			Rectangle tabRect = tab.DisplayRectangle;
			TabColors tabColors = GetTabColors(tab);
			GraphicsPath val7 = new GraphicsPath();
			switch (m_Alignment)
			{
			case eTabStripAlignment.Top:
				tabRect.Width--;
				val7.AddLine(tabRect.X + 2, tabRect.Y, tabRect.Right - 2, tabRect.Y);
				val7.AddLine(tabRect.Right, tabRect.Y + 2, tabRect.Right, tabRect.Bottom);
				val7.AddLine(tabRect.Right, tabRect.Bottom, tabRect.X, tabRect.Bottom);
				val7.AddLine(tabRect.X, tabRect.Y + 2, tabRect.X + 2, tabRect.Y);
				val7.CloseAllFigures();
				break;
			case eTabStripAlignment.Bottom:
				tabRect.Width--;
				val7.AddLine(tabRect.X, tabRect.Y, tabRect.Right, tabRect.Y);
				val7.AddLine(tabRect.Right, tabRect.Y, tabRect.Right, tabRect.Bottom - 2);
				val7.AddLine(tabRect.Right - 2, tabRect.Bottom, tabRect.X + 2, tabRect.Bottom);
				val7.AddLine(tabRect.X, tabRect.Bottom - 2, tabRect.X, tabRect.Y);
				val7.CloseAllFigures();
				break;
			case eTabStripAlignment.Left:
				tabRect.Height--;
				val7.AddLine(tabRect.X + 2, tabRect.Y, tabRect.Right, tabRect.Y);
				val7.AddLine(tabRect.Right, tabRect.Bottom, tabRect.X + 2, tabRect.Bottom);
				val7.AddLine(tabRect.X, tabRect.Bottom - 2, tabRect.X, tabRect.Y + 2);
				val7.CloseAllFigures();
				break;
			case eTabStripAlignment.Right:
				tabRect.Height--;
				tabRect.Width -= 2;
				val7.AddLine(tabRect.X, tabRect.Y, tabRect.Right - 2, tabRect.Y);
				val7.AddLine(tabRect.Right, tabRect.Y + 2, tabRect.Right, tabRect.Bottom - 2);
				val7.AddLine(tabRect.Right - 2, tabRect.Bottom, tabRect.X, tabRect.Bottom);
				val7.CloseAllFigures();
				break;
			}
			if (tabColors.BackColor2.IsEmpty)
			{
				if (!tabColors.BackColor.IsEmpty)
				{
					SolidBrush val8 = new SolidBrush(tabColors.BackColor);
					try
					{
						g.FillPath((Brush)(object)val8, val7);
					}
					finally
					{
						((IDisposable)val8)?.Dispose();
					}
				}
			}
			else
			{
				LinearGradientBrush val9 = BarFunctions.CreateLinearGradientBrush(tabRect, tabColors.BackColor, tabColors.BackColor2, tabColors.BackColorGradientAngle);
				try
				{
					g.FillPath((Brush)(object)val9, val7);
				}
				finally
				{
					((IDisposable)val9)?.Dispose();
				}
			}
			if (!tabColors.BorderColor.IsEmpty)
			{
				Pen val10 = new Pen(tabColors.BorderColor, 1f);
				try
				{
					g.DrawPath(val10, val7);
				}
				finally
				{
					((IDisposable)val10)?.Dispose();
				}
			}
			if (m_CloseButtonOnTabs && tab.CloseButtonVisible)
			{
				tab.CloseButtonBounds = PaintTabItemCloseButton(g, m_Alignment == eTabStripAlignment.Left || m_Alignment == eTabStripAlignment.Right, tab.CloseButtonMouseOver, tab == m_HotTab || tab == SelectedTab, ref tabRect);
			}
			else
			{
				tab.CloseButtonBounds = Rectangle.Empty;
			}
			CompositeImage tabImage = GetTabImage(tab);
			if (tabImage != null && tabImage.Width + 4 <= tabRect.Width)
			{
				if (m_Alignment == eTabStripAlignment.Top || m_Alignment == eTabStripAlignment.Bottom)
				{
					tabImage.DrawImage(g, new Rectangle(tabRect.X + 3, tabRect.Y + (tabRect.Height - tabImage.Height) / 2, tabImage.Width, tabImage.Height));
					int num = tabImage.Width + 2;
					tabRect.X += num;
					tabRect.Width -= num;
				}
				else
				{
					tabImage.DrawImage(g, new Rectangle(tabRect.X + (tabRect.Width - tabImage.Width) / 2, tabRect.Y + 3, tabImage.Width, tabImage.Height));
					int num2 = tabImage.Height + 2;
					tabRect.Y += num2;
					tabRect.Height -= num2;
				}
			}
			if (!m_DisplaySelectedTextOnly || tab == m_SelectedTab)
			{
				Font font = ((Control)this).Font;
				if (tab == selectedTab && m_SelectedTabFont != null)
				{
					font = m_SelectedTabFont;
				}
				Rectangle bounds = tabRect;
				if (m_Alignment == eTabStripAlignment.Left || m_Alignment == eTabStripAlignment.Right)
				{
					g.RotateTransform(90f);
					bounds = new Rectangle(bounds.Top, -bounds.Right, bounds.Height, bounds.Width);
				}
				if (bounds.Width > MIN_TEXT_WIDTH)
				{
					if (m_Alignment == eTabStripAlignment.Left || m_Alignment == eTabStripAlignment.Right)
					{
						TextDrawing.DrawStringLegacy(g, tab.Text, font, tabColors.TextColor, bounds, format);
					}
					else
					{
						TextDrawing.DrawString(g, tab.Text, font, tabColors.TextColor, bounds, format);
					}
				}
				if (m_Alignment == eTabStripAlignment.Left || m_Alignment == eTabStripAlignment.Right)
				{
					g.ResetTransform();
				}
				if (m_ShowFocusRectangle && ((Control)this).Focused && tab == selectedTab)
				{
					ControlPaint.DrawFocusRectangle(g, GetFocusRectangle(tabRect));
				}
			}
			if (!tab.IsSelected && m_Tabs.IndexOf(tab) < m_Tabs.Count - 1 && m_Tabs.IndexOf(tab) + 1 != m_Tabs.IndexOf(m_SelectedTab))
			{
				if (m_Alignment == eTabStripAlignment.Left || m_Alignment == eTabStripAlignment.Right)
				{
					DisplayHelp.DrawLine(g, tab.DisplayRectangle.X + 3, tab.DisplayRectangle.Bottom - 1, tab.DisplayRectangle.Right - 3, tab.DisplayRectangle.Bottom - 1, m_ColorScheme.TabItemSeparator, 1);
					DisplayHelp.DrawLine(g, tab.DisplayRectangle.X + 3, tab.DisplayRectangle.Bottom, tab.DisplayRectangle.Right - 3, tab.DisplayRectangle.Bottom, m_ColorScheme.TabItemSeparatorShade, 1);
				}
				else
				{
					DisplayHelp.DrawLine(g, tab.DisplayRectangle.Right - 1, tab.DisplayRectangle.Y + 3, tab.DisplayRectangle.Right - 1, tab.DisplayRectangle.Bottom - 3, m_ColorScheme.TabItemSeparator, 1);
					DisplayHelp.DrawLine(g, tab.DisplayRectangle.Right, tab.DisplayRectangle.Y + 3, tab.DisplayRectangle.Right, tab.DisplayRectangle.Bottom - 3, m_ColorScheme.TabItemSeparatorShade, 1);
				}
			}
			if (HasPostRenderTabItem)
			{
				RenderTabItemEventArgs e = new RenderTabItemEventArgs(tab, g);
				InvokePostRenderTabItem(e);
			}
		}
		g.ResetClip();
		if (HasNavigationBox && m_TabSystemBox.Visible)
		{
			g.SetClip(m_TabSystemBox.DisplayRectangle);
			SolidBrush val11 = new SolidBrush(m_ColorScheme.TabBackground);
			try
			{
				g.FillRectangle((Brush)(object)val11, m_TabSystemBox.DisplayRectangle);
			}
			finally
			{
				((IDisposable)val11)?.Dispose();
			}
			m_TabSystemBox.Paint(g);
			g.ResetClip();
		}
	}

	private CompositeImage GetTabImage(TabItem tab)
	{
		Image image = tab.GetImage();
		if (image != null)
		{
			return new CompositeImage(image, dispose: false);
		}
		Icon icon = tab.Icon;
		if (icon != null)
		{
			return new CompositeImage(icon, dispose: false, tab.IconSize);
		}
		return null;
	}

	internal void ResetTabHeight()
	{
		m_TabHeight = 0;
	}

	private int GetSingleTabHeight()
	{
		if (IsMultiLine)
		{
			int result = 0;
			if (BarFunctions.IsHandleValid((Control)(object)this))
			{
				Graphics val = ((Control)this).CreateGraphics();
				try
				{
					result = GetTabHeight(val);
				}
				finally
				{
					val.Dispose();
				}
			}
			return result;
		}
		return TabHeight;
	}

	private int GetTabHeight(Graphics g)
	{
		double y = StiScale2.Y;
		if (m_FixedTabSize.Height > 0)
		{
			return m_FixedTabSize.Height;
		}
		int num = (int)Math.Ceiling(16.0 * y);
		if ((m_Alignment == eTabStripAlignment.Left || m_Alignment == eTabStripAlignment.Right) && m_Style != eTabStripStyle.OneNote && m_Style != eTabStripStyle.VS2005Document && m_Style != eTabStripStyle.Office2007Document)
		{
			num = (int)Math.Ceiling(20.0 * y);
		}
		if (m_ImageList != null)
		{
			num = m_ImageList.ImageSize.Height;
		}
		else
		{
			Font font = ((Control)this).Font;
			if (m_SelectedTabFont != null)
			{
				font = m_SelectedTabFont;
			}
			foreach (TabItem tab in m_Tabs)
			{
				if (tab.Icon != null && tab.Icon.Height > num)
				{
					num = tab.IconSize.Height;
				}
				else if (tab.Image != null && tab.Image.Height > num)
				{
					num = tab.Image.Height;
				}
				string text = tab.Text;
				if (text != "")
				{
					Size empty = Size.Empty;
					empty = ((!IsVertical) ? TextDrawing.MeasureString(g, tab.Text, font) : TextDrawing.MeasureStringLegacy(g, tab.Text, font, Size.Empty, eTextFormat.Default));
					if (empty.Height > num)
					{
						num = empty.Height;
					}
				}
			}
		}
		if (((Control)this).Font.Height > num)
		{
			num = ((Control)this).Font.Height;
		}
		num += 4;
		if (m_Style == eTabStripStyle.Metro)
		{
			num += 2;
		}
		if (IsThemed)
		{
			num++;
		}
		return num;
	}

	public void RecalcSize()
	{
		if (!BarFunctions.IsHandleValid((Control)(object)this))
		{
			m_NeedRecalcSize = true;
			return;
		}
		Graphics val = ((Control)this).CreateGraphics();
		try
		{
			RecalcSize(val, GetTabClientArea(((Control)this).DisplayRectangle, TabLayoutType == eTabLayoutType.MultilineWithNavigationBox, bPaintArea: false));
		}
		finally
		{
			val.Dispose();
		}
	}

	private void RecalcSizeContentManager(Graphics g, Rectangle dispRect)
	{
		if (Tabs.Count == 0)
		{
			if (IsRightToLeft)
			{
				RecalcSizeTabSystemBox(dispRect, Rectangle.Empty);
			}
			else
			{
				RecalcSizeTabSystemBox(dispRect, Rectangle.Empty);
			}
			return;
		}
		SerialContentLayoutManager serialContentLayoutManager = new SerialContentLayoutManager();
		TabItemLayoutManager tabItemLayoutManager = new TabItemLayoutManager(this);
		tabItemLayoutManager.FixedTabSize = m_FixedTabSize;
		tabItemLayoutManager.CloseButtonOnTabs = m_CloseButtonOnTabs;
		tabItemLayoutManager.CloseButtonSize = m_CloseButtonSize;
		serialContentLayoutManager.RightToLeft = IsRightToLeft;
		if (m_Style == eTabStripStyle.RoundHeader)
		{
			serialContentLayoutManager.ContentAlignment = eContentAlignment.Center;
			serialContentLayoutManager.EvenHeight = true;
			serialContentLayoutManager.FitContainerOversize = true;
		}
		else if (m_Style == eTabStripStyle.Office2007Document)
		{
			serialContentLayoutManager.EvenHeight = true;
			if (m_Alignment == eTabStripAlignment.Bottom)
			{
				serialContentLayoutManager.ContentAlignment = eContentAlignment.Left;
				serialContentLayoutManager.ContentVerticalAlignment = eContentVerticalAlignment.Top;
				serialContentLayoutManager.BlockLineAlignment = eContentVerticalAlignment.Top;
				serialContentLayoutManager.ContentOrientation = eContentOrientation.Horizontal;
			}
			else if (m_Alignment == eTabStripAlignment.Top)
			{
				serialContentLayoutManager.ContentAlignment = eContentAlignment.Left;
				serialContentLayoutManager.ContentVerticalAlignment = eContentVerticalAlignment.Bottom;
				serialContentLayoutManager.BlockLineAlignment = eContentVerticalAlignment.Bottom;
				serialContentLayoutManager.ContentOrientation = eContentOrientation.Horizontal;
			}
			else if (m_Alignment == eTabStripAlignment.Left)
			{
				serialContentLayoutManager.ContentAlignment = eContentAlignment.Right;
				serialContentLayoutManager.ContentVerticalAlignment = eContentVerticalAlignment.Top;
				serialContentLayoutManager.BlockLineAlignment = eContentVerticalAlignment.Top;
				serialContentLayoutManager.ContentOrientation = eContentOrientation.Vertical;
			}
			else if (m_Alignment == eTabStripAlignment.Right)
			{
				serialContentLayoutManager.ContentAlignment = eContentAlignment.Left;
				serialContentLayoutManager.ContentVerticalAlignment = eContentVerticalAlignment.Top;
				serialContentLayoutManager.BlockLineAlignment = eContentVerticalAlignment.Top;
				serialContentLayoutManager.ContentOrientation = eContentOrientation.Vertical;
			}
			serialContentLayoutManager.BlockSpacing = 0;
			if (m_Alignment == eTabStripAlignment.Top || m_Alignment == eTabStripAlignment.Bottom)
			{
				dispRect.Width -= 2;
			}
			else
			{
				dispRect.Height -= 2;
			}
			tabItemLayoutManager.PaddingHeight = 6;
		}
		else if (m_Style == eTabStripStyle.Office2007Dock)
		{
			serialContentLayoutManager.EvenHeight = true;
			if (m_Alignment == eTabStripAlignment.Bottom)
			{
				serialContentLayoutManager.ContentAlignment = eContentAlignment.Left;
				serialContentLayoutManager.ContentVerticalAlignment = eContentVerticalAlignment.Top;
				serialContentLayoutManager.BlockLineAlignment = eContentVerticalAlignment.Top;
				serialContentLayoutManager.ContentOrientation = eContentOrientation.Horizontal;
			}
			else if (m_Alignment == eTabStripAlignment.Top)
			{
				serialContentLayoutManager.ContentAlignment = eContentAlignment.Left;
				serialContentLayoutManager.ContentVerticalAlignment = eContentVerticalAlignment.Bottom;
				serialContentLayoutManager.BlockLineAlignment = eContentVerticalAlignment.Bottom;
				serialContentLayoutManager.ContentOrientation = eContentOrientation.Horizontal;
			}
			else if (m_Alignment == eTabStripAlignment.Left)
			{
				serialContentLayoutManager.ContentAlignment = eContentAlignment.Right;
				serialContentLayoutManager.ContentVerticalAlignment = eContentVerticalAlignment.Top;
				serialContentLayoutManager.BlockLineAlignment = eContentVerticalAlignment.Top;
				serialContentLayoutManager.ContentOrientation = eContentOrientation.Vertical;
			}
			else if (m_Alignment == eTabStripAlignment.Right)
			{
				serialContentLayoutManager.ContentAlignment = eContentAlignment.Left;
				serialContentLayoutManager.ContentVerticalAlignment = eContentVerticalAlignment.Top;
				serialContentLayoutManager.BlockLineAlignment = eContentVerticalAlignment.Top;
				serialContentLayoutManager.ContentOrientation = eContentOrientation.Vertical;
			}
			serialContentLayoutManager.BlockSpacing = 2;
			if (m_Alignment == eTabStripAlignment.Top || m_Alignment == eTabStripAlignment.Bottom)
			{
				dispRect.X += 2;
				dispRect.Width -= 2;
			}
			else
			{
				dispRect.Y += 2;
				dispRect.Height -= 2;
			}
			tabItemLayoutManager.PaddingHeight = 6;
		}
		else if (m_Style == eTabStripStyle.VS2005Dock)
		{
			serialContentLayoutManager.EvenHeight = true;
			if (m_Alignment == eTabStripAlignment.Bottom)
			{
				serialContentLayoutManager.ContentAlignment = eContentAlignment.Left;
				serialContentLayoutManager.ContentVerticalAlignment = eContentVerticalAlignment.Top;
				serialContentLayoutManager.BlockLineAlignment = eContentVerticalAlignment.Top;
				serialContentLayoutManager.ContentOrientation = eContentOrientation.Horizontal;
			}
			else if (m_Alignment == eTabStripAlignment.Top)
			{
				serialContentLayoutManager.ContentAlignment = eContentAlignment.Left;
				serialContentLayoutManager.ContentVerticalAlignment = eContentVerticalAlignment.Bottom;
				serialContentLayoutManager.BlockLineAlignment = eContentVerticalAlignment.Bottom;
				serialContentLayoutManager.ContentOrientation = eContentOrientation.Horizontal;
			}
			else if (m_Alignment == eTabStripAlignment.Left)
			{
				serialContentLayoutManager.ContentAlignment = eContentAlignment.Right;
				serialContentLayoutManager.ContentVerticalAlignment = eContentVerticalAlignment.Top;
				serialContentLayoutManager.BlockLineAlignment = eContentVerticalAlignment.Top;
				serialContentLayoutManager.ContentOrientation = eContentOrientation.Vertical;
			}
			else if (m_Alignment == eTabStripAlignment.Right)
			{
				serialContentLayoutManager.ContentAlignment = eContentAlignment.Left;
				serialContentLayoutManager.ContentVerticalAlignment = eContentVerticalAlignment.Top;
				serialContentLayoutManager.BlockLineAlignment = eContentVerticalAlignment.Top;
				serialContentLayoutManager.ContentOrientation = eContentOrientation.Vertical;
			}
			serialContentLayoutManager.BlockSpacing = 8;
			if (m_Alignment == eTabStripAlignment.Top || m_Alignment == eTabStripAlignment.Bottom)
			{
				dispRect.X += 4;
				dispRect.Width -= 8;
			}
			else
			{
				dispRect.Y += 4;
				dispRect.Height -= 8;
			}
		}
		else if (m_Style == eTabStripStyle.SimulatedTheme)
		{
			if (m_Alignment == eTabStripAlignment.Top || m_Alignment == eTabStripAlignment.Bottom)
			{
				dispRect.Width -= 3;
			}
			else
			{
				dispRect.Height -= 3;
			}
			if (m_Alignment == eTabStripAlignment.Bottom)
			{
				serialContentLayoutManager.ContentAlignment = eContentAlignment.Left;
				serialContentLayoutManager.ContentVerticalAlignment = eContentVerticalAlignment.Top;
				serialContentLayoutManager.BlockLineAlignment = eContentVerticalAlignment.Top;
				serialContentLayoutManager.ContentOrientation = eContentOrientation.Horizontal;
				serialContentLayoutManager.EvenHeight = true;
			}
			else if (m_Alignment == eTabStripAlignment.Top)
			{
				serialContentLayoutManager.ContentAlignment = eContentAlignment.Left;
				serialContentLayoutManager.ContentVerticalAlignment = eContentVerticalAlignment.Bottom;
				serialContentLayoutManager.BlockLineAlignment = eContentVerticalAlignment.Bottom;
				serialContentLayoutManager.ContentOrientation = eContentOrientation.Horizontal;
				serialContentLayoutManager.EvenHeight = true;
			}
			else if (m_Alignment == eTabStripAlignment.Left)
			{
				serialContentLayoutManager.ContentAlignment = eContentAlignment.Right;
				serialContentLayoutManager.ContentVerticalAlignment = eContentVerticalAlignment.Top;
				serialContentLayoutManager.BlockLineAlignment = eContentVerticalAlignment.Top;
				serialContentLayoutManager.ContentOrientation = eContentOrientation.Vertical;
				serialContentLayoutManager.EvenHeight = true;
			}
			else if (m_Alignment == eTabStripAlignment.Right)
			{
				serialContentLayoutManager.ContentAlignment = eContentAlignment.Left;
				serialContentLayoutManager.ContentVerticalAlignment = eContentVerticalAlignment.Top;
				serialContentLayoutManager.BlockLineAlignment = eContentVerticalAlignment.Top;
				serialContentLayoutManager.ContentOrientation = eContentOrientation.Vertical;
				serialContentLayoutManager.EvenHeight = true;
			}
			tabItemLayoutManager.HorizontalText = true;
			tabItemLayoutManager.PaddingHeight = 10;
			tabItemLayoutManager.PaddingWidth = 2;
		}
		if (m_Alignment == eTabStripAlignment.Left || m_Alignment == eTabStripAlignment.Right)
		{
			serialContentLayoutManager.ContentOrientation = eContentOrientation.Vertical;
		}
		else
		{
			serialContentLayoutManager.ContentOrientation = eContentOrientation.Horizontal;
		}
		if (m_Style != eTabStripStyle.RoundHeader)
		{
			if (m_TabLayoutType == eTabLayoutType.FitContainer)
			{
				serialContentLayoutManager.FitContainerOversize = true;
				serialContentLayoutManager.OversizeDistribute = true;
				serialContentLayoutManager.MultiLine = false;
			}
			else if (m_TabLayoutType == eTabLayoutType.FixedWithNavigationBox)
			{
				serialContentLayoutManager.FitContainerOversize = false;
				serialContentLayoutManager.MultiLine = false;
			}
			else if (m_TabLayoutType == eTabLayoutType.MultilineNoNavigationBox || m_TabLayoutType == eTabLayoutType.MultilineWithNavigationBox)
			{
				serialContentLayoutManager.FitContainerOversize = false;
				serialContentLayoutManager.MultiLine = true;
			}
		}
		tabItemLayoutManager.Graphics = g;
		TabItem[] array = new TabItem[Tabs.Count];
		Tabs.CopyTo(array, 0);
		Rectangle rectangle = dispRect;
		if (m_ScrollOffset != 0)
		{
			if (m_Alignment == eTabStripAlignment.Left || m_Alignment == eTabStripAlignment.Right)
			{
				rectangle.Y -= m_ScrollOffset;
			}
			else
			{
				rectangle.X -= m_ScrollOffset;
			}
		}
		if (IsRightToLeft)
		{
			if (HasNavigationBox && m_TabSystemBox.Visible)
			{
				rectangle.X += m_TabSystemBox.DefaultWidth;
			}
			Rectangle containerBounds = rectangle;
			IBlock[] contentBlocks = array;
			m_TabItemsBounds = serialContentLayoutManager.Layout(containerBounds, contentBlocks, tabItemLayoutManager);
			RecalcSizeTabSystemBox(dispRect, rectangle);
		}
		else
		{
			Rectangle containerBounds2 = rectangle;
			IBlock[] contentBlocks = array;
			m_TabItemsBounds = serialContentLayoutManager.Layout(containerBounds2, contentBlocks, tabItemLayoutManager);
			RecalcSizeTabSystemBox(dispRect, m_TabItemsBounds);
		}
	}

	private void RecalcSize(Graphics g, Rectangle dispRect)
	{
		if (IsContentManagedStyle)
		{
			RecalcSizeContentManager(g, dispRect);
		}
		else
		{
			RecalcSizeLegacy(g, dispRect);
		}
		m_NeedRecalcSize = false;
		InvokeSizeRecalculated();
	}

	private void RecalcSizeLegacy(Graphics g, Rectangle dispRect)
	{
		int num = dispRect.Width;
		int num2 = 0;
		int num3 = 0;
		int num4 = -1;
		eTextFormat format = eTextFormat.Default;
		if (m_Alignment == eTabStripAlignment.Left || m_Alignment == eTabStripAlignment.Right)
		{
			num = dispRect.Height;
		}
		int num5 = (m_TabHeight = GetTabHeight(g));
		int num6 = dispRect.X;
		int num7 = dispRect.Y;
		if (m_Alignment == eTabStripAlignment.Top)
		{
			num7 = dispRect.Bottom - num5;
		}
		else if (m_Alignment == eTabStripAlignment.Left)
		{
			num6 = dispRect.Right - num5;
		}
		if (m_Alignment == eTabStripAlignment.Left || m_Alignment == eTabStripAlignment.Right)
		{
			num7 -= m_ScrollOffset;
		}
		else
		{
			num6 -= m_ScrollOffset;
		}
		int num8 = num6;
		int num9 = num7;
		bool flag = false;
		if (HasNavigationBox)
		{
			num -= m_TabSystemBox.DefaultWidth;
			num -= 4;
		}
		ArrayList arrayList = new ArrayList();
		foreach (TabItem tab in m_Tabs)
		{
			int num10 = 4;
			if (!tab.Visible)
			{
				continue;
			}
			if (num4 < 0)
			{
				num4 = m_Tabs.IndexOf(tab);
			}
			num3++;
			if (m_FixedTabSize.Width == 0)
			{
				Image image = tab.GetImage();
				if (tab.Icon != null)
				{
					num10 += tab.IconSize.Width;
					if (m_Style != eTabStripStyle.OneNote || IsThemed)
					{
						num10 += 4;
					}
				}
				else if (image != null)
				{
					num10 += image.Width;
					if (m_Style != eTabStripStyle.OneNote || IsThemed)
					{
						num10 += 4;
					}
				}
				if (m_Style == eTabStripStyle.OneNote && !IsThemed)
				{
					num10 += num5 - 6;
				}
				else if (m_Style == eTabStripStyle.VS2005Document && !IsThemed)
				{
					num10 += 4;
				}
				if (tab.Text != "" && (!m_DisplaySelectedTextOnly || tab == m_SelectedTab))
				{
					Font font = ((Control)this).Font;
					if (m_SelectedTabFont != null && (tab == m_SelectedTab || (m_CloseButtonPosition == eTabCloseButtonPosition.Right && m_CloseButtonOnTabsAlwaysDisplayed)))
					{
						font = m_SelectedTabFont;
					}
					Size empty = Size.Empty;
					num10 += ((!IsVertical) ? TextDrawing.MeasureString(g, tab.Text, font, 0, format) : TextDrawing.MeasureStringLegacy(g, tab.Text, font, Size.Empty, format)).Width;
					if ((m_Style != eTabStripStyle.OneNote && m_Style != eTabStripStyle.VS2005Document) || IsThemed || m_Alignment == eTabStripAlignment.Top || m_Alignment == eTabStripAlignment.Bottom)
					{
						num10 += 4;
					}
					if (IsThemed)
					{
						num10 += 10;
					}
				}
				if (m_CloseButtonOnTabs && tab.CloseButtonVisible && m_Style != eTabStripStyle.OneNote)
				{
					num10 += m_CloseButtonSize.Width;
				}
			}
			else
			{
				num10 = m_FixedTabSize.Width;
			}
			if (IsMultiLine && num2 + num10 > num && num3 > 1)
			{
				flag = true;
				switch (m_Alignment)
				{
				case eTabStripAlignment.Left:
				case eTabStripAlignment.Right:
					if (arrayList.Count == 0)
					{
						arrayList.Add(num6);
					}
					num6 += num5 + m_MultiLineSpacing;
					num7 = num9;
					arrayList.Add(num6);
					break;
				case eTabStripAlignment.Top:
				case eTabStripAlignment.Bottom:
					if (arrayList.Count == 0)
					{
						arrayList.Add(num7);
					}
					num7 += num5 + m_MultiLineSpacing;
					num6 = num8;
					arrayList.Add(num7);
					break;
				}
				m_TabHeight += num5 + m_MultiLineSpacing;
				num2 = 0;
			}
			if (m_Alignment == eTabStripAlignment.Left || m_Alignment == eTabStripAlignment.Right)
			{
				tab._DisplayRectangle = new Rectangle(num6, num7, num5, num10);
			}
			else
			{
				tab._DisplayRectangle = new Rectangle(num6, num7, num10, num5);
			}
			if (this.MeasureTabItem != null)
			{
				MeasureTabItemEventArgs measureTabItemEventArgs = new MeasureTabItemEventArgs(tab, tab.DisplayRectangle.Size);
				this.MeasureTabItem(this, measureTabItemEventArgs);
				tab._DisplayRectangle = new Rectangle(tab._DisplayRectangle.Location, measureTabItemEventArgs.Size);
				num10 = ((m_Alignment != 0 && m_Alignment != eTabStripAlignment.Right) ? measureTabItemEventArgs.Size.Width : measureTabItemEventArgs.Size.Height);
			}
			if (m_Alignment == eTabStripAlignment.Left || m_Alignment == eTabStripAlignment.Right)
			{
				num7 += num10;
			}
			else
			{
				num6 += num10;
			}
			num2 += num10;
		}
		if (flag)
		{
			if (m_Alignment == eTabStripAlignment.Top)
			{
				int num11 = num9 - num7;
				foreach (TabItem tab2 in m_Tabs)
				{
					tab2._DisplayRectangle = new Rectangle(tab2.DisplayRectangle.X, tab2.DisplayRectangle.Y + num11, tab2.DisplayRectangle.Width, tab2.DisplayRectangle.Height);
				}
				for (int i = 0; i < arrayList.Count; i++)
				{
					arrayList[i] = (int)arrayList[i] + num11;
				}
			}
			else if (m_Alignment == eTabStripAlignment.Left)
			{
				int num12 = num8 - num6;
				foreach (TabItem tab3 in m_Tabs)
				{
					tab3._DisplayRectangle = new Rectangle(tab3.DisplayRectangle.X + num12, tab3.DisplayRectangle.Y, tab3.DisplayRectangle.Width, tab3.DisplayRectangle.Height);
				}
				for (int j = 0; j < arrayList.Count; j++)
				{
					arrayList[j] = (int)arrayList[j] + num12;
				}
			}
			switch (m_Alignment)
			{
			case eTabStripAlignment.Left:
			{
				if (m_SelectedTab.DisplayRectangle.X == (int)arrayList[arrayList.Count - 1])
				{
					break;
				}
				int x2 = m_SelectedTab.DisplayRectangle.X;
				int num16 = (int)arrayList[arrayList.Count - 1];
				foreach (TabItem tab4 in m_Tabs)
				{
					if (tab4.DisplayRectangle.X == num16)
					{
						tab4._DisplayRectangle = new Rectangle(x2, tab4.DisplayRectangle.Y, tab4.DisplayRectangle.Width, tab4.DisplayRectangle.Height);
					}
					else if (tab4.DisplayRectangle.X == x2)
					{
						tab4._DisplayRectangle = new Rectangle(num16, tab4.DisplayRectangle.Y, tab4.DisplayRectangle.Width, tab4.DisplayRectangle.Height);
					}
				}
				break;
			}
			case eTabStripAlignment.Right:
			{
				if (m_SelectedTab.DisplayRectangle.X == (int)arrayList[0])
				{
					break;
				}
				int x = m_SelectedTab.DisplayRectangle.X;
				int num14 = (int)arrayList[0];
				foreach (TabItem tab5 in m_Tabs)
				{
					if (tab5.DisplayRectangle.X == num14)
					{
						tab5._DisplayRectangle = new Rectangle(x, tab5.DisplayRectangle.Y, tab5.DisplayRectangle.Width, tab5.DisplayRectangle.Height);
					}
					else if (tab5.DisplayRectangle.X == x)
					{
						tab5._DisplayRectangle = new Rectangle(num14, tab5.DisplayRectangle.Y, tab5.DisplayRectangle.Width, tab5.DisplayRectangle.Height);
					}
				}
				break;
			}
			case eTabStripAlignment.Top:
			{
				if (m_SelectedTab.DisplayRectangle.Y == (int)arrayList[arrayList.Count - 1])
				{
					break;
				}
				int y2 = m_SelectedTab.DisplayRectangle.Y;
				int num15 = (int)arrayList[arrayList.Count - 1];
				foreach (TabItem tab6 in m_Tabs)
				{
					if (tab6.DisplayRectangle.Y == num15)
					{
						tab6._DisplayRectangle = new Rectangle(tab6.DisplayRectangle.X, y2, tab6.DisplayRectangle.Width, tab6.DisplayRectangle.Height);
					}
					else if (tab6.DisplayRectangle.Y == y2)
					{
						tab6._DisplayRectangle = new Rectangle(tab6.DisplayRectangle.X, num15, tab6.DisplayRectangle.Width, tab6.DisplayRectangle.Height);
					}
				}
				break;
			}
			case eTabStripAlignment.Bottom:
			{
				if (m_SelectedTab.DisplayRectangle.Y == (int)arrayList[0])
				{
					break;
				}
				int y = m_SelectedTab.DisplayRectangle.Y;
				int num13 = (int)arrayList[0];
				foreach (TabItem tab7 in m_Tabs)
				{
					if (tab7.DisplayRectangle.Y == num13)
					{
						tab7._DisplayRectangle = new Rectangle(tab7.DisplayRectangle.X, y, tab7.DisplayRectangle.Width, tab7.DisplayRectangle.Height);
					}
					else if (tab7.DisplayRectangle.Y == y)
					{
						tab7._DisplayRectangle = new Rectangle(tab7.DisplayRectangle.X, num13, tab7.DisplayRectangle.Width, tab7.DisplayRectangle.Height);
					}
				}
				break;
			}
			}
		}
		if (HasNavigationBox)
		{
			if (m_ScrollOffset != 0)
			{
				m_TabSystemBox.BackEnabled = true;
			}
			else
			{
				m_TabSystemBox.BackEnabled = false;
			}
			if (num2 - m_ScrollOffset > num)
			{
				m_TabSystemBox.ForwardEnabled = true;
			}
			else
			{
				m_TabSystemBox.ForwardEnabled = false;
			}
			if (m_AutoHideSystemBox && !IsMultiLine)
			{
				if (num2 > num)
				{
					m_TabSystemBox.Visible = true;
				}
				else
				{
					m_TabSystemBox.Visible = false;
				}
			}
			else
			{
				m_TabSystemBox.Visible = true;
			}
			Rectangle displayRectangle = Rectangle.Empty;
			switch (m_Alignment)
			{
			case eTabStripAlignment.Left:
				displayRectangle = new Rectangle((num4 < 0) ? dispRect.X : m_Tabs[num4].DisplayRectangle.X, ((Control)this).ClientRectangle.Bottom - m_TabSystemBox.DefaultWidth - 3, (num4 < 0) ? dispRect.Width : m_Tabs[num4].DisplayRectangle.Width, m_TabSystemBox.DefaultWidth);
				if (m_Style == eTabStripStyle.VS2005 && !IsThemed)
				{
					displayRectangle.X--;
				}
				break;
			case eTabStripAlignment.Right:
				displayRectangle = new Rectangle(dispRect.X, ((Control)this).ClientRectangle.Bottom - m_TabSystemBox.DefaultWidth - 3, (num4 < 0) ? dispRect.Width : m_Tabs[num4].DisplayRectangle.Width, m_TabSystemBox.DefaultWidth);
				if (m_Style == eTabStripStyle.VS2005 && !IsThemed)
				{
					displayRectangle.X++;
				}
				break;
			case eTabStripAlignment.Top:
				displayRectangle = ((!IsRightToLeft) ? new Rectangle(((Control)this).ClientRectangle.Right - m_TabSystemBox.DefaultWidth - 3, (num4 < 0) ? dispRect.Y : m_Tabs[num4].DisplayRectangle.Y, m_TabSystemBox.DefaultWidth, (num4 < 0) ? dispRect.Height : m_Tabs[num4].DisplayRectangle.Height) : new Rectangle(((Control)this).ClientRectangle.Left, (num4 < 0) ? dispRect.Y : m_Tabs[num4].DisplayRectangle.Y, m_TabSystemBox.DefaultWidth, (num4 < 0) ? dispRect.Height : m_Tabs[num4].DisplayRectangle.Height));
				if (m_Style == eTabStripStyle.VS2005 && !IsThemed)
				{
					displayRectangle.Y--;
				}
				break;
			default:
				displayRectangle = ((!IsRightToLeft) ? new Rectangle(((Control)this).ClientRectangle.Right - m_TabSystemBox.DefaultWidth - 2, dispRect.Y, m_TabSystemBox.DefaultWidth + 2, (num4 < 0) ? dispRect.Height : m_Tabs[num4].DisplayRectangle.Height) : new Rectangle(((Control)this).ClientRectangle.Left, dispRect.Y, m_TabSystemBox.DefaultWidth + 2, (num4 < 0) ? dispRect.Height : m_Tabs[num4].DisplayRectangle.Height));
				if (m_Style == eTabStripStyle.VS2005 && !IsThemed)
				{
					displayRectangle.Y++;
				}
				break;
			}
			m_TabSystemBox.DisplayRectangle = displayRectangle;
		}
		if (num2 > num && m_TabLayoutType == eTabLayoutType.FitContainer && !IsMultiLine)
		{
			if (num3 <= 0)
			{
				return;
			}
			if (m_Alignment == eTabStripAlignment.Left || m_Alignment == eTabStripAlignment.Right)
			{
				num6 = dispRect.X;
				num7 = dispRect.Y;
				if (m_Alignment == eTabStripAlignment.Left)
				{
					num6 = dispRect.Right - num5;
				}
				int num17 = num / num3;
				int num18 = 0;
				foreach (TabItem tab8 in m_Tabs)
				{
					if (tab8.Visible)
					{
						if (tab8.DisplayRectangle.Height > num17)
						{
							num18++;
							continue;
						}
						num -= tab8.DisplayRectangle.Height;
						num2 -= tab8.DisplayRectangle.Height;
					}
				}
				float num19 = (float)num / (float)num2;
				{
					foreach (TabItem tab9 in m_Tabs)
					{
						if (!tab9.Visible)
						{
							continue;
						}
						if (tab9.DisplayRectangle.Height > num17)
						{
							if (num18 == num3)
							{
								tab9._DisplayRectangle = new Rectangle(num6, num7, tab9.DisplayRectangle.Width, num17);
							}
							else
							{
								tab9._DisplayRectangle = new Rectangle(num6, num7, tab9.DisplayRectangle.Width, (int)((float)tab9.DisplayRectangle.Height * num19));
							}
						}
						else
						{
							tab9._DisplayRectangle = new Rectangle(num6, num7, tab9.DisplayRectangle.Width, tab9.DisplayRectangle.Height);
						}
						num7 += tab9.DisplayRectangle.Height;
					}
					return;
				}
			}
			num6 = dispRect.X;
			int num20 = num / num3;
			int num21 = 0;
			foreach (TabItem tab10 in m_Tabs)
			{
				if (tab10.Visible)
				{
					if (tab10.DisplayRectangle.Width > num20)
					{
						num21++;
						continue;
					}
					num -= tab10.DisplayRectangle.Width;
					num2 -= tab10.DisplayRectangle.Width;
				}
			}
			float num22 = (float)num / (float)num2;
			{
				foreach (TabItem tab11 in m_Tabs)
				{
					if (!tab11.Visible)
					{
						continue;
					}
					if (tab11.DisplayRectangle.Width > num20)
					{
						if (num21 == num3)
						{
							tab11._DisplayRectangle = new Rectangle(num6, num7, num20, tab11.DisplayRectangle.Height);
						}
						else
						{
							tab11._DisplayRectangle = new Rectangle(num6, num7, (int)((float)tab11.DisplayRectangle.Width * num22), tab11.DisplayRectangle.Height);
						}
					}
					else
					{
						tab11._DisplayRectangle = new Rectangle(num6, num7, tab11.DisplayRectangle.Width, tab11.DisplayRectangle.Height);
					}
					num6 += tab11.DisplayRectangle.Width;
				}
				return;
			}
		}
		if (!IsRightToLeft || m_TabLayoutType == eTabLayoutType.FitContainer || (m_Alignment != eTabStripAlignment.Top && m_Alignment != eTabStripAlignment.Bottom))
		{
			return;
		}
		if (num2 > num)
		{
			for (int num23 = m_Tabs.Count - 1; num23 >= 0; num23--)
			{
				if (m_Tabs[num23].Visible)
				{
					num6 = m_Tabs[num23].DisplayRectangle.Right;
					break;
				}
			}
			if (m_TabSystemBox.Visible)
			{
				num6 += m_TabSystemBox.DisplayRectangle.Right;
			}
		}
		else
		{
			num6 = dispRect.Right;
		}
		foreach (TabItem tab12 in m_Tabs)
		{
			if (tab12.Visible)
			{
				num6 -= tab12.DisplayRectangle.Width;
				tab12._DisplayRectangle = new Rectangle(num6, tab12.DisplayRectangle.Y, tab12.DisplayRectangle.Width, tab12.DisplayRectangle.Height);
			}
		}
	}

	internal void InvokeMeasureTabItem(MeasureTabItemEventArgs e)
	{
		if (this.MeasureTabItem != null)
		{
			this.MeasureTabItem(this, e);
		}
	}

	internal void InvokePreRenderTabItem(RenderTabItemEventArgs e)
	{
		if (this.PreRenderTabItem != null)
		{
			this.PreRenderTabItem(this, e);
		}
	}

	internal void InvokePostRenderTabItem(RenderTabItemEventArgs e)
	{
		if (this.PostRenderTabItem != null)
		{
			this.PostRenderTabItem(this, e);
		}
	}

	private TabItem GetFirstVisibleTabItem()
	{
		foreach (TabItem tab in m_Tabs)
		{
			if (tab.Visible)
			{
				return tab;
			}
		}
		return null;
	}

	internal void RecalcSizeTabSystemBox(Rectangle displayRect, Rectangle tabsBounds)
	{
		if (!HasNavigationBox)
		{
			return;
		}
		Rectangle rectangle = tabsBounds;
		if (m_Alignment == eTabStripAlignment.Top || m_Alignment == eTabStripAlignment.Bottom)
		{
			rectangle.Offset(m_ScrollOffset, 0);
		}
		else
		{
			rectangle.Offset(0, m_ScrollOffset);
		}
		bool visible = false;
		if ((rectangle.Right > displayRect.Right - ((!m_AutoHideSystemBox) ? m_TabSystemBox.DefaultWidth : 0) && (m_Alignment == eTabStripAlignment.Top || m_Alignment == eTabStripAlignment.Bottom)) || (rectangle.Bottom > displayRect.Bottom - ((!m_AutoHideSystemBox) ? m_TabSystemBox.DefaultWidth : 0) && (m_Alignment == eTabStripAlignment.Right || m_Alignment == eTabStripAlignment.Left)))
		{
			visible = true;
		}
		if (m_ScrollOffset != 0)
		{
			m_TabSystemBox.BackEnabled = true;
		}
		else
		{
			m_TabSystemBox.BackEnabled = false;
		}
		if (m_AutoHideSystemBox && !IsMultiLine)
		{
			m_TabSystemBox.Visible = visible;
		}
		else
		{
			m_TabSystemBox.Visible = true;
		}
		if ((tabsBounds.Right + m_ScrollOffset > displayRect.Right - (m_TabSystemBox.Visible ? m_TabSystemBox.DefaultWidth : 0) && (m_Alignment == eTabStripAlignment.Top || m_Alignment == eTabStripAlignment.Bottom)) || (tabsBounds.Bottom + m_ScrollOffset > displayRect.Bottom - (m_TabSystemBox.Visible ? m_TabSystemBox.DefaultWidth : 0) && (m_Alignment == eTabStripAlignment.Right || m_Alignment == eTabStripAlignment.Left)))
		{
			m_TabSystemBox.ForwardEnabled = true;
		}
		else
		{
			m_TabSystemBox.ForwardEnabled = false;
		}
		TabItem firstVisibleTabItem = GetFirstVisibleTabItem();
		Rectangle displayRectangle = Rectangle.Empty;
		switch (m_Alignment)
		{
		case eTabStripAlignment.Left:
			displayRectangle = new Rectangle(firstVisibleTabItem?.DisplayRectangle.X ?? displayRect.X, ((Control)this).ClientRectangle.Bottom - m_TabSystemBox.DefaultWidth - 3, firstVisibleTabItem?.DisplayRectangle.Width ?? displayRect.Width, m_TabSystemBox.DefaultWidth);
			if (m_Style == eTabStripStyle.VS2005 && !IsThemed)
			{
				displayRectangle.X--;
			}
			break;
		case eTabStripAlignment.Right:
			displayRectangle = new Rectangle(displayRect.X, ((Control)this).ClientRectangle.Bottom - m_TabSystemBox.DefaultWidth - 3, firstVisibleTabItem?.DisplayRectangle.Width ?? displayRect.Width, m_TabSystemBox.DefaultWidth);
			if (m_Style == eTabStripStyle.VS2005 && !IsThemed)
			{
				displayRectangle.X++;
			}
			break;
		case eTabStripAlignment.Top:
			displayRectangle = ((!IsRightToLeft) ? new Rectangle(((Control)this).ClientRectangle.Right - m_TabSystemBox.DefaultWidth - 3, firstVisibleTabItem?.DisplayRectangle.Y ?? displayRect.Y, m_TabSystemBox.DefaultWidth, firstVisibleTabItem?.DisplayRectangle.Height ?? displayRect.Height) : new Rectangle(displayRect.X, firstVisibleTabItem?.DisplayRectangle.Y ?? displayRect.Y, m_TabSystemBox.DefaultWidth, firstVisibleTabItem?.DisplayRectangle.Height ?? displayRect.Height));
			if (m_Style == eTabStripStyle.VS2005 && !IsThemed)
			{
				displayRectangle.Y--;
			}
			break;
		default:
			displayRectangle = ((!IsRightToLeft) ? new Rectangle(((Control)this).ClientRectangle.Right - m_TabSystemBox.DefaultWidth - 2, displayRect.Y, m_TabSystemBox.DefaultWidth + 2, firstVisibleTabItem?.DisplayRectangle.Height ?? displayRect.Height) : new Rectangle(displayRect.X, displayRect.Y, m_TabSystemBox.DefaultWidth + 2, firstVisibleTabItem?.DisplayRectangle.Height ?? displayRect.Height));
			if (m_Style == eTabStripStyle.VS2005 && !IsThemed)
			{
				displayRectangle.Y++;
			}
			break;
		}
		m_TabSystemBox.DisplayRectangle = displayRectangle;
	}

	private void InvokeSizeRecalculated()
	{
		if (this.SizeRecalculated != null)
		{
			this.SizeRecalculated(this, new EventArgs());
		}
		if (IsMultiLine && !(((Control)this).Parent is TabControl))
		{
			eTabStripAlignment tabAlignment = TabAlignment;
			if ((uint)tabAlignment <= 1u)
			{
				((Control)this).Width = MinTabStripHeight;
			}
			else
			{
				((Control)this).Height = MinTabStripHeight;
			}
		}
	}

	private void RefreshThemes()
	{
		if (m_ThemeTab != null)
		{
			m_ThemeTab.Dispose();
		}
		m_ThemeTab = new ThemeTab((Control)(object)this);
	}

	private void DisposeThemes()
	{
		if (m_ThemeTab != null)
		{
			m_ThemeTab.Dispose();
			m_ThemeTab = null;
		}
	}

	protected override void WndProc(ref Message m)
	{
		if (((Message)(ref m)).Msg == 794)
		{
			Themes.RefreshIsThemeActive();
			RefreshThemes();
		}
		((Control)this).WndProc(ref m);
	}

	protected override void OnHandleDestroyed(EventArgs e)
	{
		DisposeThemes();
		((Control)this).OnHandleDestroyed(e);
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		((Control)this).OnHandleCreated(e);
		if (m_NeedRecalcSize)
		{
			RecalcSize();
			if (((Control)this).Parent is TabControl)
			{
				((TabControl)(object)((Control)this).Parent).SyncTabStripSize();
			}
		}
		RefreshMdiTabItems();
	}

	internal void OnSelectedTabRemoved(int index, TabItem item)
	{
		if (index >= Tabs.Count)
		{
			index = Tabs.Count - 1;
		}
		int num = -1;
		for (int i = index; i < Tabs.Count; i++)
		{
			if (Tabs[i].Visible)
			{
				num = i;
				break;
			}
		}
		if (num == -1)
		{
			for (int j = 0; j < Tabs.Count; j++)
			{
				if (Tabs[j].Visible)
				{
					num = j;
					break;
				}
			}
		}
		if (num >= 0)
		{
			SelectedTab = Tabs[num];
		}
		else
		{
			m_SelectedTab = null;
		}
	}

	internal void InvokeTabRemoved(TabItem tab)
	{
		if (!m_MovingTab)
		{
			HideToolTip();
			if (this.TabRemoved != null)
			{
				this.TabRemoved(tab, new EventArgs());
			}
			if (VisibleTabCount == 0)
			{
				SelectedTab = null;
			}
		}
	}

	private void SelectTab(TabItem value, eEventSource eventSource)
	{
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Invalid comparison between Unknown and I4
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Invalid comparison between Unknown and I4
		if (m_SelectedTab == value)
		{
			return;
		}
		TabItem selectedTab = m_SelectedTab;
		if (this.SelectedTabChanging != null)
		{
			TabStripTabChangingEventArgs tabStripTabChangingEventArgs = new TabStripTabChangingEventArgs(m_SelectedTab, value, eventSource);
			this.SelectedTabChanging(this, tabStripTabChangingEventArgs);
			if (tabStripTabChangingEventArgs.Cancel)
			{
				return;
			}
		}
		InvokeBeforeTabDisplay(value);
		Control val = null;
		if (m_SelectedTab != null && m_SelectedTab.AttachedItem != null)
		{
			m_SelectedTab.AttachedItem.Displayed = false;
		}
		else if (m_SelectedTab != null && m_SelectedTab.AttachedControl != null && !m_MdiTabbedDocuments)
		{
			val = m_SelectedTab.AttachedControl;
		}
		m_SelectedTab = value;
		m_NeedRecalcSize = true;
		EnsureVisible(m_SelectedTab);
		if (m_SelectedTab != null && m_SelectedTab.AttachedItem != null)
		{
			m_SelectedTab.AttachedItem.Displayed = true;
			if (m_SelectedTab.AttachedItem is DockContainerItem && ((DockContainerItem)m_SelectedTab.AttachedItem).Control != null && m_AutoSelectAttachedControl)
			{
				((DockContainerItem)m_SelectedTab.AttachedItem).Control.Select();
			}
		}
		else if (m_SelectedTab != null && m_SelectedTab.AttachedControl != null)
		{
			if (!m_MdiTabbedDocuments)
			{
				m_SelectedTab.AttachedControl.Visible = true;
				if (m_AutoSelectAttachedControl && (!((Control)this).TabStop || !(((Control)this).Parent is TabControl) || !((Control)this).Focused || eventSource != 0))
				{
					m_SelectedTab.AttachedControl.Select();
				}
				if (val == m_SelectedTab.AttachedControl)
				{
					val = null;
				}
			}
			else if (m_SelectedTab.AttachedControl is Form)
			{
				MdiClient mdiClient = GetMdiClient(m_MdiForm);
				bool flag = false;
				Form activeMdiChild = m_MdiForm.ActiveMdiChild;
				if (mdiClient != null && m_MdiNoFormActivateFlicker && m_SelectedTab.AttachedControl is Form && ((int)((Form)m_SelectedTab.AttachedControl).WindowState == 2 || (activeMdiChild != null && (int)activeMdiChild.WindowState == 2)))
				{
					NativeFunctions.SendMessage(((Control)mdiClient).Handle, 11, 0, 0);
					flag = true;
				}
				((Form)m_SelectedTab.AttachedControl).Activate();
				if (flag)
				{
					NativeFunctions.SendMessage(((Control)mdiClient).Handle, 11, 1, 0);
					((Control)mdiClient).Refresh();
				}
				if (activeMdiChild != null && (int)activeMdiChild.WindowState == 0 && BarFunctions.ThemedOS)
				{
					string text = ((Control)activeMdiChild).Text;
					((Control)activeMdiChild).Text = text + " ";
					((Control)activeMdiChild).Text = text;
				}
			}
		}
		if (val != null)
		{
			val.Visible = false;
		}
		((Control)this).Invalidate();
		if (this.SelectedTabChanged != null)
		{
			TabStripTabChangedEventArgs e = new TabStripTabChangedEventArgs(selectedTab, m_SelectedTab, eventSource);
			this.SelectedTabChanged(this, e);
		}
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Invalid comparison between Unknown and I4
		if (_KeyboardNavigationEnabled)
		{
			if ((int)keyData == 37)
			{
				if (SelectPreviousTab(eEventSource.Keyboard, cycle: false))
				{
					return true;
				}
			}
			else if ((int)keyData == 39 && SelectNextTab(eEventSource.Keyboard, cycle: false))
			{
				return true;
			}
		}
		return ((Control)this).ProcessCmdKey(ref msg, keyData);
	}

	public bool SelectPreviousTab()
	{
		return SelectPreviousTab(eEventSource.Code, cycle: false);
	}

	internal bool SelectPreviousTab(eEventSource eventSource, bool cycle)
	{
		if (SelectedTab != null && Tabs.IndexOf(SelectedTab) > 0)
		{
			for (int num = Tabs.IndexOf(SelectedTab) - 1; num >= 0; num--)
			{
				if (Tabs[num].Visible)
				{
					SelectTab(Tabs[num], eventSource);
					return true;
				}
			}
		}
		if (cycle)
		{
			for (int num2 = Tabs.Count - 1; num2 >= 0; num2--)
			{
				if (Tabs[num2].Visible)
				{
					SelectTab(Tabs[num2], eventSource);
					return true;
				}
			}
		}
		return false;
	}

	public bool SelectNextTab()
	{
		return SelectNextTab(eEventSource.Code, cycle: false);
	}

	internal bool SelectNextTab(eEventSource eventSource, bool cycle)
	{
		if (SelectedTab != null && Tabs.IndexOf(SelectedTab) < Tabs.Count)
		{
			for (int i = Tabs.IndexOf(SelectedTab) + 1; i < Tabs.Count; i++)
			{
				if (Tabs[i].Visible)
				{
					SelectTab(Tabs[i], eventSource);
					return true;
				}
			}
		}
		if (cycle)
		{
			for (int j = 0; j < Tabs.Count; j++)
			{
				if (Tabs[j].Visible)
				{
					SelectTab(Tabs[j], eventSource);
					return true;
				}
			}
		}
		return false;
	}

	protected override void OnClick(EventArgs e)
	{
		((Control)this).OnClick(e);
		Point point = ((Control)this).PointToClient(Control.MousePosition);
		if (!HasNavigationBox || !m_TabSystemBox.Visible || !m_TabSystemBox.DisplayRectangle.Contains(point.X, point.Y))
		{
			HitTest(point.X, point.Y)?.InvokeClick(e);
		}
	}

	protected override void OnDoubleClick(EventArgs e)
	{
		((Control)this).OnDoubleClick(e);
		Point point = ((Control)this).PointToClient(Control.MousePosition);
		if (!HasNavigationBox || !m_TabSystemBox.Visible || !m_TabSystemBox.DisplayRectangle.Contains(point.X, point.Y))
		{
			HitTest(point.X, point.Y)?.InvokeDoubleClick(e);
		}
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		((Control)this).OnMouseDown(e);
		InternalOnMouseDown(e);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void InternalOnMouseDown(MouseEventArgs e)
	{
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Invalid comparison between Unknown and I4
		//IL_034a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0376: Unknown result type (might be due to invalid IL or missing references)
		//IL_0310: Unknown result type (might be due to invalid IL or missing references)
		//IL_0315: Unknown result type (might be due to invalid IL or missing references)
		//IL_031b: Invalid comparison between Unknown and I4
		//IL_0323: Unknown result type (might be due to invalid IL or missing references)
		//IL_0329: Invalid comparison between Unknown and I4
		HideToolTip();
		if (HasNavigationBox && m_TabSystemBox.Visible && m_TabSystemBox.DisplayRectangle.Contains(e.X, e.Y))
		{
			m_TabSystemBox.MouseDown(e);
			return;
		}
		_MouseDownPoint = new Point(e.X, e.Y);
		if ((int)e.Button == 1048576)
		{
			Rectangle selectedTabRectangle = GetSelectedTabRectangle();
			foreach (TabItem tab in m_Tabs)
			{
				if ((!tab.DisplayRectangle.Contains(new Point(e.X, e.Y)) && !selectedTabRectangle.Contains(e.X, e.Y)) || !tab.Visible)
				{
					continue;
				}
				TabItem tabItem2 = tab;
				if (selectedTabRectangle.Contains(e.X, e.Y))
				{
					tabItem2 = m_SelectedTab;
				}
				if (m_SelectedTab != tabItem2)
				{
					TabItem selectedTab = m_SelectedTab;
					if (this.SelectedTabChanging != null)
					{
						TabStripTabChangingEventArgs tabStripTabChangingEventArgs = new TabStripTabChangingEventArgs(m_SelectedTab, tabItem2, eEventSource.Mouse);
						this.SelectedTabChanging(this, tabStripTabChangingEventArgs);
						if (tabStripTabChangingEventArgs.Cancel)
						{
							break;
						}
					}
					InvokeBeforeTabDisplay(tabItem2);
					if (m_SelectedTab.AttachedItem is DockContainerItem && ((DockContainerItem)m_SelectedTab.AttachedItem).Control != null && ((DockContainerItem)m_SelectedTab.AttachedItem).Control.Focused)
					{
						((Control)this).Focus();
					}
					if (m_SelectedTab.AttachedItem != null)
					{
						m_SelectedTab.AttachedItem.Displayed = false;
					}
					else if (m_SelectedTab.AttachedControl != null && !m_MdiTabbedDocuments)
					{
						m_SelectedTab.AttachedControl.Visible = false;
					}
					m_SelectedTab = tabItem2;
					m_NeedRecalcSize = true;
					EnsureVisible(m_SelectedTab);
					if (m_SelectedTab.AttachedItem != null)
					{
						m_SelectedTab.AttachedItem.Displayed = true;
						if (m_SelectedTab.AttachedItem is DockContainerItem && ((DockContainerItem)m_SelectedTab.AttachedItem).Control != null && m_AutoSelectAttachedControl)
						{
							((DockContainerItem)m_SelectedTab.AttachedItem).Control.Select();
						}
					}
					else if (m_SelectedTab.AttachedControl != null)
					{
						if (!m_MdiTabbedDocuments)
						{
							m_SelectedTab.AttachedControl.Visible = true;
							if (m_AutoSelectAttachedControl)
							{
								m_SelectedTab.AttachedControl.Select();
							}
						}
						else if (m_SelectedTab.AttachedControl is Form)
						{
							MdiClient mdiClient = GetMdiClient(m_MdiForm);
							bool flag = false;
							Form activeMdiChild = m_MdiForm.ActiveMdiChild;
							if (mdiClient != null && m_MdiNoFormActivateFlicker && m_SelectedTab.AttachedControl is Form && ((int)((Form)m_SelectedTab.AttachedControl).WindowState == 2 || (activeMdiChild != null && (int)activeMdiChild.WindowState == 2)))
							{
								NativeFunctions.SendMessage(((Control)mdiClient).Handle, 11, 0, 0);
								flag = true;
							}
							((Form)m_SelectedTab.AttachedControl).Activate();
							if (flag)
							{
								NativeFunctions.SendMessage(((Control)mdiClient).Handle, 11, 1, 0);
								((Control)mdiClient).Refresh();
							}
							if (activeMdiChild != null && (int)activeMdiChild.WindowState == 0 && BarFunctions.ThemedOS)
							{
								string text = ((Control)activeMdiChild).Text;
								((Control)activeMdiChild).Text = text + " ";
								((Control)activeMdiChild).Text = text;
							}
						}
					}
					((Control)this).Invalidate();
					if (this.SelectedTabChanged != null)
					{
						TabStripTabChangedEventArgs e2 = new TabStripTabChangedEventArgs(selectedTab, m_SelectedTab, eEventSource.Mouse);
						this.SelectedTabChanged(this, e2);
					}
				}
				else if (m_SelectedTab != null && m_SelectedTab.AttachedItem is DockContainerItem && m_SelectedTab.AttachedItem.ContainerControl is Bar && ((Bar)m_SelectedTab.AttachedItem.ContainerControl).DockSide == eDockSide.Document && ((DockContainerItem)m_SelectedTab.AttachedItem).Control != null && m_AutoSelectAttachedControl)
				{
					((DockContainerItem)m_SelectedTab.AttachedItem).Control.Select();
				}
				break;
			}
		}
		HitTest(e.X, e.Y)?.InvokeMouseDown(e);
	}

	private void InvalidateHotTab()
	{
		if (m_HotTab == null)
		{
			return;
		}
		if (m_Style == eTabStripStyle.OneNote || m_Style == eTabStripStyle.VS2005Document)
		{
			Rectangle displayRectangle = m_HotTab.DisplayRectangle;
			if (m_Alignment == eTabStripAlignment.Top || m_Alignment == eTabStripAlignment.Bottom)
			{
				displayRectangle.X -= displayRectangle.Height;
				displayRectangle.Width += displayRectangle.Height * 2;
				displayRectangle.Height += 2;
			}
			else
			{
				displayRectangle.Y -= displayRectangle.Width;
				displayRectangle.Height += displayRectangle.Width * 2;
				displayRectangle.Width += 2;
			}
			((Control)this).Invalidate(displayRectangle);
		}
		else if (m_Style == eTabStripStyle.Office2007Document)
		{
			Rectangle displayRectangle2 = m_HotTab.DisplayRectangle;
			if (m_Alignment == eTabStripAlignment.Top || m_Alignment == eTabStripAlignment.Bottom)
			{
				displayRectangle2.Width += displayRectangle2.Height * 2;
				displayRectangle2.Height += 2;
			}
			else
			{
				displayRectangle2.Height += displayRectangle2.Width * 2;
				displayRectangle2.Width += 2;
				displayRectangle2.X--;
			}
			((Control)this).Invalidate(displayRectangle2);
		}
		else if (m_Style == eTabStripStyle.VS2005Dock)
		{
			Rectangle displayRectangle3 = m_HotTab.DisplayRectangle;
			displayRectangle3.Inflate(6, 0);
			((Control)this).Invalidate(displayRectangle3);
		}
		else
		{
			Rectangle displayRectangle4 = m_HotTab.DisplayRectangle;
			displayRectangle4.Inflate(2, 2);
			((Control)this).Invalidate(displayRectangle4);
		}
	}

	private Rectangle GetSelectedTabRectangle()
	{
		if (SelectedTab == null)
		{
			return Rectangle.Empty;
		}
		Rectangle displayRectangle = SelectedTab.DisplayRectangle;
		if (m_Style == eTabStripStyle.OneNote || m_Style == eTabStripStyle.VS2005Document)
		{
			switch (m_Alignment)
			{
			case eTabStripAlignment.Top:
				displayRectangle.X -= displayRectangle.Height - 6;
				displayRectangle.Width += displayRectangle.Height - 6;
				break;
			case eTabStripAlignment.Bottom:
				displayRectangle.X -= displayRectangle.Height - 6;
				displayRectangle.Width += displayRectangle.Height - 6;
				break;
			case eTabStripAlignment.Left:
				displayRectangle.Height += displayRectangle.Width - 6;
				break;
			case eTabStripAlignment.Right:
				displayRectangle.Y -= displayRectangle.Width - 6;
				displayRectangle.Height += displayRectangle.Width - 6;
				break;
			}
		}
		return displayRectangle;
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
		InternalOnMouseMove(e);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void InternalOnMouseMove(MouseEventArgs e)
	{
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Invalid comparison between Unknown and I4
		//IL_0480: Unknown result type (might be due to invalid IL or missing references)
		if (m_TooltipTab != null && !m_TooltipTab.DisplayRectangle.Contains(e.X, e.Y))
		{
			HideToolTip();
			ResetHover();
		}
		if (m_HotTab != null)
		{
			if (m_HotTab.DisplayRectangle.Contains(e.X, e.Y))
			{
				m_HotTab.InvokeMouseMove(e);
			}
			else
			{
				m_HotTab.InvokeMouseLeave((EventArgs)(object)e);
				TabItem tabItem = HitTest(e.X, e.Y);
				if (tabItem != null)
				{
					tabItem.InvokeMouseEnter((EventArgs)(object)e);
					ResetHover();
				}
			}
		}
		else
		{
			TabItem tabItem2 = HitTest(e.X, e.Y);
			if (tabItem2 != null)
			{
				tabItem2.InvokeMouseEnter((EventArgs)(object)e);
				ResetHover();
			}
		}
		if (HasNavigationBox && !TabDrag)
		{
			m_TabSystemBox.MouseMove(e);
			if (m_TabSystemBox.Visible && m_TabSystemBox.DisplayRectangle.Contains(e.X, e.Y))
			{
				if (m_HotTab != null)
				{
					if (m_Tabs.Contains(m_HotTab))
					{
						InvalidateHotTab();
						m_HotTab = null;
						((Control)this).Update();
					}
					else
					{
						m_HotTab = null;
					}
				}
				return;
			}
		}
		if (m_DragBar != null)
		{
			if (!((Control)m_DragBar).IsDisposed)
			{
				m_DragBar.DragMouseMove();
			}
		}
		else
		{
			if (m_IsDragging)
			{
				return;
			}
			if ((int)e.Button == 1048576)
			{
				if (m_HotTab != null && m_HotTab.CloseButtonMouseOver)
				{
					m_HotTab.CloseButtonMouseOver = false;
					InvalidateHotTab();
				}
				if (TabDrag)
				{
					if (((Control)this).DisplayRectangle.Contains(e.X, e.Y))
					{
						if (!m_CanReorderTabs && !GetDesignMode())
						{
							return;
						}
						{
							foreach (TabItem tab in m_Tabs)
							{
								Rectangle displayRectangle = tab.DisplayRectangle;
								if (e.X < displayRectangle.Right && (m_Alignment == eTabStripAlignment.Bottom || m_Alignment == eTabStripAlignment.Top))
								{
									displayRectangle.Width = m_SelectedTab.DisplayRectangle.Width;
								}
								else if (e.Y < displayRectangle.Bottom && (m_Alignment == eTabStripAlignment.Left || m_Alignment == eTabStripAlignment.Right))
								{
									displayRectangle.Height = m_SelectedTab.DisplayRectangle.Height;
								}
								if (!displayRectangle.Contains(e.X, e.Y))
								{
									continue;
								}
								if (m_SelectedTab == tab)
								{
									break;
								}
								int num = m_Tabs.IndexOf(tab);
								if (this.TabMoved != null)
								{
									TabStripTabMovedEventArgs tabStripTabMovedEventArgs = new TabStripTabMovedEventArgs(m_SelectedTab, m_Tabs.IndexOf(m_SelectedTab), num);
									this.TabMoved(this, tabStripTabMovedEventArgs);
									if (tabStripTabMovedEventArgs.Cancel)
									{
										break;
									}
								}
								TabItem selectedTab = m_SelectedTab;
								m_MovingTab = true;
								m_Tabs._Remove(selectedTab);
								m_Tabs._Insert(num, selectedTab);
								SelectedTab = selectedTab;
								m_MovingTab = false;
								((Control)this).Refresh();
								break;
							}
							return;
						}
					}
					Rectangle displayRectangle2 = ((Control)this).DisplayRectangle;
					displayRectangle2.Inflate(16, 16);
					if (displayRectangle2.Contains(e.X, e.Y))
					{
						return;
					}
					int visibleTabCount = VisibleTabCount;
					if (((Control)this).Parent is Bar && ((Bar)(object)((Control)this).Parent).CanTearOffTabs && (visibleTabCount > 1 || (visibleTabCount == 1 && ((Bar)(object)((Control)this).Parent).GrabHandleStyle == eGrabHandleStyle.None)))
					{
						TabDrag = false;
						m_IsDragging = true;
						m_DragBar = ((Bar)(object)((Control)this).Parent).StartTabDrag();
						if (m_DragBar != null)
						{
							((Control)this).Capture = true;
							((Control)this).Refresh();
						}
						else
						{
							m_IsDragging = false;
						}
					}
				}
				else if ((Math.Abs(e.X - _MouseDownPoint.X) >= 4 || Math.Abs(e.Y - _MouseDownPoint.Y) >= 4) && m_Tabs.Count > 0 && !IsMultiLine && CanReorderTabs)
				{
					TabDrag = true;
				}
			}
			else
			{
				if ((int)e.Button != 0)
				{
					return;
				}
				if (GetSelectedTabRectangle().Contains(e.X, e.Y))
				{
					SetHotTab(SelectedTab, e.X, e.Y);
					return;
				}
				foreach (TabItem tab2 in m_Tabs)
				{
					if (tab2.DisplayRectangle.Contains(e.X, e.Y))
					{
						SetHotTab(tab2, e.X, e.Y);
						return;
					}
				}
				SetHotTab(null, e.X, e.Y);
			}
		}
	}

	private Cursor GetTabDragCursor()
	{
		if (_TabDragCursor != (Cursor)null)
		{
			return _TabDragCursor;
		}
		return Cursors.SizeAll;
	}

	private bool GetDesignMode()
	{
		if (((Control)this).Parent is TabControl)
		{
			TabControl tabControl = ((Control)this).Parent as TabControl;
			if (((Component)(object)tabControl).Site != null)
			{
				return ((Component)(object)tabControl).Site.DesignMode;
			}
		}
		return ((Component)this).DesignMode;
	}

	private void SetHotTab(TabItem tab, int x, int y)
	{
		if (m_HotTab != tab)
		{
			if (m_Tabs.Contains(m_HotTab))
			{
				InvalidateHotTab();
			}
			if (m_HotTab != null)
			{
				m_HotTab.CloseButtonMouseOver = false;
			}
			m_HotTab = tab;
			if (m_HotTab != null)
			{
				if (!m_HotTab.CloseButtonBounds.IsEmpty && m_HotTab.CloseButtonBounds.Contains(x, y))
				{
					m_HotTab.CloseButtonMouseOver = true;
				}
				InvalidateHotTab();
			}
			((Control)this).Update();
			ResetHover();
		}
		else
		{
			if (m_HotTab == null || m_HotTab.CloseButtonBounds.IsEmpty)
			{
				return;
			}
			if (m_HotTab.CloseButtonBounds.Contains(x, y))
			{
				if (!m_HotTab.CloseButtonMouseOver)
				{
					m_HotTab.CloseButtonMouseOver = true;
					InvalidateHotTab();
					((Control)this).Update();
				}
			}
			else if (m_HotTab.CloseButtonMouseOver)
			{
				m_HotTab.CloseButtonMouseOver = false;
				InvalidateHotTab();
				((Control)this).Update();
			}
		}
	}

	protected override void OnMouseWheel(MouseEventArgs e)
	{
		((Control)this).OnMouseWheel(e);
		m_TabSystemBox.MouseWheel(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		((Control)this).OnMouseUp(e);
		InternalOnMouseUp(e);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void InternalOnMouseUp(MouseEventArgs e)
	{
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Invalid comparison between Unknown and I4
		if (HasNavigationBox)
		{
			m_TabSystemBox.MouseUp(e);
			if (m_TabSystemBox.Visible && m_TabSystemBox.DisplayRectangle.Contains(e.X, e.Y))
			{
				return;
			}
		}
		Point mouseDownPoint = _MouseDownPoint;
		_MouseDownPoint = Point.Empty;
		if (m_IsDragging)
		{
			((Control)this).Capture = false;
		}
		if (m_DragBar != null)
		{
			if (!((Control)m_DragBar).IsDisposed)
			{
				m_DragBar.DragMouseUp();
			}
			m_DragBar = null;
		}
		m_IsDragging = false;
		TabDrag = false;
		TabItem tabItem = HitTest(e.X, e.Y);
		tabItem?.InvokeMouseUp(e);
		if (SelectedTab != null && SelectedTab == tabItem && !SelectedTab.CloseButtonBounds.IsEmpty && (int)e.Button == 1048576)
		{
			Rectangle closeButtonBounds = SelectedTab.CloseButtonBounds;
			closeButtonBounds.Width += 2;
			if (closeButtonBounds.Contains(e.X, e.Y) && closeButtonBounds.Contains(mouseDownPoint))
			{
				CloseSelectedTab();
			}
		}
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		InternalOnMouseLeave(e);
		((Control)this).OnMouseLeave(e);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void InternalOnMouseLeave(EventArgs e)
	{
		HideToolTip();
		if (HasNavigationBox)
		{
			m_TabSystemBox.MouseLeave();
		}
		if (m_HotTab != null)
		{
			m_HotTab.InvokeMouseLeave(e);
			if (m_Tabs.Contains(m_HotTab))
			{
				m_HotTab.CloseButtonMouseOver = false;
				InvalidateHotTab();
				m_HotTab = null;
				((Control)this).Update();
			}
			else
			{
				m_HotTab = null;
			}
		}
	}

	protected override void OnMouseHover(EventArgs e)
	{
		((Control)this).OnMouseHover(e);
		if (HasNavigationBox)
		{
			m_TabSystemBox.MouseHover();
		}
		InternalMouseHover();
	}

	internal void InternalTabsCleared()
	{
		HideToolTip();
		OnTabsCleared(new EventArgs());
		if (m_SelectedTab != null)
		{
			if (this.SelectedTabChanged != null)
			{
				TabStripTabChangedEventArgs e = new TabStripTabChangedEventArgs(m_SelectedTab, null, eEventSource.Code);
				this.SelectedTabChanged(this, e);
			}
			m_SelectedTab = null;
		}
	}

	protected virtual void OnTabsCleared(EventArgs e)
	{
		if (this.TabsCleared != null)
		{
			this.TabsCleared(this, e);
		}
	}

	public void EnsureVisible(TabItem tab)
	{
		if (tab == null || !m_Tabs.Contains(tab) || m_TabLayoutType == eTabLayoutType.FitContainer || IsMultiLine || !BarFunctions.IsHandleValid((Control)(object)this))
		{
			return;
		}
		if (tab.DisplayRectangle.IsEmpty)
		{
			((Control)this).Invalidate();
			RecalcSize();
		}
		Rectangle displayRectangle = tab.DisplayRectangle;
		if (m_Style == eTabStripStyle.OneNote || m_Style == eTabStripStyle.VS2005Document || m_Style == eTabStripStyle.Office2007Document)
		{
			if (m_Alignment == eTabStripAlignment.Top || m_Alignment == eTabStripAlignment.Bottom)
			{
				if (m_Style != eTabStripStyle.Office2007Document)
				{
					displayRectangle.X -= TabHeight - 6;
				}
				displayRectangle.Width += TabHeight - 6;
			}
			else if (m_Alignment == eTabStripAlignment.Right)
			{
				if (m_Style != eTabStripStyle.Office2007Document)
				{
					displayRectangle.Y -= TabHeight - 6;
				}
				displayRectangle.Height += TabHeight - 6;
			}
			else if (m_Alignment == eTabStripAlignment.Left)
			{
				displayRectangle.Height += TabHeight - 6;
			}
		}
		int scrollOffset = ScrollOffset;
		if (m_Alignment == eTabStripAlignment.Top || m_Alignment == eTabStripAlignment.Bottom)
		{
			if (displayRectangle.X < 0 || (IsRightToLeft && displayRectangle.X < m_TabSystemBox.DisplayRectangle.Right && m_TabSystemBox.Visible))
			{
				scrollOffset = m_TabItemsBounds.Width - ((Control)this).ClientRectangle.Width;
				if (IsRightToLeft && (m_Style == eTabStripStyle.OneNote || m_Style == eTabStripStyle.VS2005Document || m_Style == eTabStripStyle.Office2007Document))
				{
					scrollOffset -= displayRectangle.Height;
				}
				if (scrollOffset < 0 || (IsRightToLeft && scrollOffset < m_TabSystemBox.DisplayRectangle.Right))
				{
					scrollOffset = 0;
				}
				ScrollOffset = scrollOffset;
			}
			else if (!IsRightToLeft && displayRectangle.Right > m_TabSystemBox.DisplayRectangle.Left && m_TabSystemBox.Visible && !m_TabSystemBox.DisplayRectangle.IsEmpty)
			{
				scrollOffset = scrollOffset + displayRectangle.Right - m_TabSystemBox.DisplayRectangle.Left + 2;
				ScrollOffset = scrollOffset;
			}
			else if (IsRightToLeft && displayRectangle.Right > ((Control)this).DisplayRectangle.Right)
			{
				scrollOffset = scrollOffset + displayRectangle.Right - ((Control)this).DisplayRectangle.Right + 2;
				ScrollOffset = scrollOffset;
			}
			else if (tab.IsSelected && IsLastVisibleTab(tab) && scrollOffset > 0 && !m_TabItemsBounds.IsEmpty)
			{
				scrollOffset = m_TabItemsBounds.Width - (((Control)this).ClientRectangle.Width - m_TabSystemBox.DisplayRectangle.Width - TabHeight);
				if (scrollOffset < 0 || (IsRightToLeft && scrollOffset < m_TabSystemBox.DisplayRectangle.Right))
				{
					scrollOffset = 0;
				}
				ScrollOffset = scrollOffset;
			}
		}
		else if (displayRectangle.Y < 0)
		{
			scrollOffset += displayRectangle.Y - 4;
			if (scrollOffset < 0)
			{
				scrollOffset = 0;
			}
			ScrollOffset = scrollOffset;
		}
		else if (displayRectangle.Bottom > m_TabSystemBox.DisplayRectangle.Top && m_TabSystemBox.Visible && !m_TabSystemBox.DisplayRectangle.IsEmpty)
		{
			scrollOffset = scrollOffset + displayRectangle.Bottom - m_TabSystemBox.DisplayRectangle.Top + 4;
			ScrollOffset = scrollOffset;
		}
	}

	private bool IsLastVisibleTab(TabItem tab)
	{
		int num = Tabs.Count - 1;
		for (int num2 = num; num2 >= 0; num2--)
		{
			if (Tabs[num2].Visible)
			{
				if (Tabs[num2] != tab)
				{
					break;
				}
				return true;
			}
		}
		return false;
	}

	internal void InvokeBeforeTabDisplay(TabItem item)
	{
		if (!_IgnoreBeforeTabDisplayEvent && this.BeforeTabDisplay != null)
		{
			this.BeforeTabDisplay(item, new EventArgs());
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			StyleManager.Unregister((Control)(object)this);
		}
		if (BarUtilities.DisposeItemImages && !((Component)this).DesignMode)
		{
			BarUtilities.DisposeImage(ref m_TabCloseButtonNormal);
			BarUtilities.DisposeImage(ref m_TabCloseButtonHot);
		}
		((Control)this).Dispose(disposing);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void StyleManagerStyleChanged(eDotNetBarStyle newStyle)
	{
		OnTabStyleChanged();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSelectedTabFont()
	{
		m_SelectedTabFont = null;
	}

	public void RefreshColorScheme()
	{
		m_ColorScheme.Style = m_Style;
		m_ColorScheme.Themed = IsThemed;
		m_ColorScheme.Refresh();
	}

	public void SetStyle()
	{
		OnTabStyleChanged();
	}

	private void OnTabStyleChanged()
	{
		m_NeedRecalcSize = true;
		RefreshColorScheme();
		m_ScrollOffset = 0;
		if ((m_Style == eTabStripStyle.OneNote || m_Style == eTabStripStyle.VS2005Document || m_Style == eTabStripStyle.Office2007Document) && !IsThemed && m_TabLayoutType == eTabLayoutType.FitContainer)
		{
			m_TabLayoutType = eTabLayoutType.FixedWithNavigationBox;
		}
		((Control)this).Invalidate();
		EnsureVisible(SelectedTab);
	}

	protected override void OnSystemColorsChanged(EventArgs e)
	{
		((Control)this).OnSystemColorsChanged(e);
		Application.DoEvents();
		OnTabStyleChanged();
	}

	protected override void OnFontChanged(EventArgs e)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		((Control)this).OnFontChanged(e);
		m_TabHeight = 0;
		m_NeedRecalcSize = true;
		if (IsDesignMode)
		{
			if ((m_Style == eTabStripStyle.OneNote || m_Style == eTabStripStyle.VS2005Document || m_Style == eTabStripStyle.Office2007Document) && !m_SelectedTabFontCustom)
			{
				m_SelectedTabFont = new Font(((Control)this).Font, (FontStyle)1);
			}
			((Control)this).Invalidate();
		}
	}

	protected override void OnGotFocus(EventArgs e)
	{
		((Control)this).OnGotFocus(e);
		if (ShowFocusRectangle)
		{
			((Control)this).Invalidate();
		}
	}

	protected override void OnLostFocus(EventArgs e)
	{
		((Control)this).OnLostFocus(e);
		if (ShowFocusRectangle)
		{
			((Control)this).Invalidate();
		}
	}

	internal void OnTabVisibleChanged(TabItem tab)
	{
		m_NeedRecalcSize = true;
		if (tab == m_TooltipTab)
		{
			HideToolTip();
		}
		if (!tab.Visible && m_SelectedTab == tab)
		{
			int num = Tabs.IndexOf(tab) - 1;
			TabItem tabItem = null;
			while (num >= 0)
			{
				if (Tabs[num].Visible)
				{
					tabItem = Tabs[num];
					break;
				}
				num--;
			}
			if (tabItem == null)
			{
				if (num < 0)
				{
					num = 0;
				}
				for (int i = num; i < Tabs.Count; i++)
				{
					if (Tabs[i].Visible)
					{
						tabItem = Tabs[i];
						break;
					}
				}
			}
			SelectedTab = tabItem;
		}
		else if (VisibleTabCount == 1 && tab.Visible)
		{
			SelectedTab = tab;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeColorScheme()
	{
		return m_ColorScheme.SchemeChanged;
	}

	public void ResetColorScheme()
	{
		m_ColorScheme.ResetChangedFlag();
		m_ColorScheme.Refresh();
		if (IsDesignMode)
		{
			((Control)this).Invalidate();
		}
	}

	private void ColorSchemeChanged(object sender, EventArgs e)
	{
		if (IsDesignMode || ((Control)this).Parent is TabControl)
		{
			((Control)this).Invalidate();
		}
	}

	protected override bool ProcessMnemonic(char charCode)
	{
		string text = "&" + charCode;
		text = text.ToLower();
		foreach (TabItem tab in Tabs)
		{
			if (!string.IsNullOrEmpty(tab.Text))
			{
				string text2 = tab.Text.ToLower();
				if (text2.IndexOf(text) >= 0)
				{
					SelectTab(tab, eEventSource.Keyboard);
					return true;
				}
			}
		}
		return ((Control)this).ProcessMnemonic(charCode);
	}

	internal TabColors GetTabColors(TabItem tab)
	{
		TabColors tabColors = new TabColors();
		if (tab == m_HotTab && !tab.IsSelected)
		{
			tabColors.BackColor = m_ColorScheme.TabItemHotBackground;
			tabColors.BackColor2 = m_ColorScheme.TabItemHotBackground2;
			tabColors.BackColorGradientAngle = m_ColorScheme.TabItemHotBackgroundGradientAngle;
			tabColors.TextColor = m_ColorScheme.TabItemHotText;
			tabColors.LightBorderColor = m_ColorScheme.TabItemHotBorderLight;
			tabColors.DarkBorderColor = m_ColorScheme.TabItemHotBorderDark;
			tabColors.BorderColor = m_ColorScheme.TabItemHotBorder;
			tabColors.BackgroundColorBlend.CopyFrom(m_ColorScheme.TabItemHotBackgroundColorBlend);
		}
		if (SelectedTab == tab)
		{
			if (tab.PredefinedColor != 0)
			{
				tabColors.BackColor = tab.BackColor;
				tabColors.BackColor2 = tab.BackColor2;
				tabColors.BackColorGradientAngle = tab.BackColorGradientAngle;
				tabColors.TextColor = tab.TextColor;
			}
			if (tabColors.BackColor.IsEmpty)
			{
				tabColors.BackColor = m_ColorScheme.TabItemSelectedBackground;
				tabColors.BackColor2 = m_ColorScheme.TabItemSelectedBackground2;
				tabColors.BackColorGradientAngle = m_ColorScheme.TabItemSelectedBackgroundGradientAngle;
				tabColors.BackgroundColorBlend.CopyFrom(m_ColorScheme.TabItemSelectedBackgroundColorBlend);
			}
			if (tabColors.TextColor.IsEmpty)
			{
				if (m_ColorScheme.TabItemSelectedText.IsEmpty)
				{
					tabColors.TextColor = m_ColorScheme.TabItemText;
				}
				else
				{
					tabColors.TextColor = m_ColorScheme.TabItemSelectedText;
				}
			}
			if (tabColors.BorderColor.IsEmpty)
			{
				tabColors.LightBorderColor = m_ColorScheme.TabItemSelectedBorderLight;
				tabColors.DarkBorderColor = m_ColorScheme.TabItemSelectedBorderDark;
				tabColors.BorderColor = m_ColorScheme.TabItemSelectedBorder;
			}
		}
		if (tabColors.BackColor.IsEmpty)
		{
			if (tab.BackColor.IsEmpty && tab.BackColor2.IsEmpty)
			{
				tabColors.BackColor = m_ColorScheme.TabItemBackground;
				tabColors.BackColor2 = m_ColorScheme.TabItemBackground2;
				tabColors.BackColorGradientAngle = m_ColorScheme.TabItemBackgroundGradientAngle;
				tabColors.BackgroundColorBlend.CopyFrom(m_ColorScheme.TabItemBackgroundColorBlend);
			}
			else
			{
				tabColors.BackColor = tab.BackColor;
				tabColors.BackColor2 = tab.BackColor2;
				tabColors.BackColorGradientAngle = tab.BackColorGradientAngle;
				tabColors.TextColor = tab.TextColor;
			}
		}
		if (tabColors.TextColor.IsEmpty)
		{
			if (tab.TextColor.IsEmpty)
			{
				tabColors.TextColor = m_ColorScheme.TabItemText;
			}
			else
			{
				tabColors.TextColor = tab.TextColor;
			}
		}
		if (tabColors.BorderColor.IsEmpty)
		{
			if (tab.BorderColor.IsEmpty && tab.LightBorderColor.IsEmpty && tab.DarkBorderColor.IsEmpty)
			{
				tabColors.DarkBorderColor = m_ColorScheme.TabItemBorderDark;
				tabColors.LightBorderColor = m_ColorScheme.TabItemBorderLight;
				tabColors.BorderColor = m_ColorScheme.TabItemBorder;
			}
			else
			{
				tabColors.LightBorderColor = tab.LightBorderColor;
				tabColors.DarkBorderColor = tab.DarkBorderColor;
				tabColors.BorderColor = tab.BorderColor;
			}
		}
		return tabColors;
	}

	private void OnTabBack(object sender, EventArgs e)
	{
		ScrollBackwards();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ScrollBackwards()
	{
		if (this.NavigateBack != null)
		{
			TabStripActionEventArgs tabStripActionEventArgs = new TabStripActionEventArgs();
			this.NavigateBack(this, tabStripActionEventArgs);
			if (tabStripActionEventArgs.Cancel)
			{
				return;
			}
		}
		int num = 0;
		for (int num2 = m_Tabs.Count - 1; num2 >= 0; num2--)
		{
			TabItem tabItem = m_Tabs[num2];
			if (tabItem.Visible)
			{
				if (m_Alignment == eTabStripAlignment.Top || m_Alignment == eTabStripAlignment.Bottom)
				{
					if (tabItem.DisplayRectangle.X < 4)
					{
						num = -tabItem.DisplayRectangle.Width;
						if (m_Style == eTabStripStyle.OneNote || m_Style == eTabStripStyle.VS2005Document)
						{
							num -= tabItem.DisplayRectangle.Height;
						}
						break;
					}
				}
				else if (tabItem.DisplayRectangle.Y < 4)
				{
					num = -tabItem.DisplayRectangle.Height;
					if (m_Style == eTabStripStyle.OneNote || m_Style == eTabStripStyle.VS2005Document)
					{
						num -= tabItem.DisplayRectangle.Height;
					}
					break;
				}
			}
		}
		if (num == 0 && m_ScrollOffset > 0)
		{
			num = -m_ScrollOffset;
		}
		int scrollOffset = m_ScrollOffset;
		int num3 = m_ScrollOffset + num;
		if (num3 < 0)
		{
			num3 = 0;
		}
		if (m_Animate)
		{
			DateTime now = DateTime.Now;
			for (int num4 = scrollOffset; num4 > num3; num4--)
			{
				ScrollOffset = num4;
				if (DateTime.Now.Subtract(now).TotalMilliseconds > 200.0)
				{
					break;
				}
			}
		}
		ScrollOffset = num3;
	}

	private void OnTabForward(object sender, EventArgs e)
	{
		ScrollForward();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ScrollForward()
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Invalid comparison between Unknown and I4
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Invalid comparison between Unknown and I4
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Invalid comparison between Unknown and I4
		if (this.NavigateForward != null)
		{
			TabStripActionEventArgs tabStripActionEventArgs = new TabStripActionEventArgs();
			this.NavigateForward(this, tabStripActionEventArgs);
			if (tabStripActionEventArgs.Cancel)
			{
				return;
			}
		}
		int num = 0;
		TabItem tabItem = null;
		int num2 = 0;
		int num3 = m_Tabs.Count;
		int num4 = 1;
		if ((int)((Control)this).RightToLeft == 1)
		{
			num2 = m_Tabs.Count - 1;
			num3 = -1;
			num4 = -1;
		}
		for (int i = num2; i != num3; i += num4)
		{
			TabItem tabItem2 = m_Tabs[i];
			if (!tabItem2.Visible)
			{
				continue;
			}
			if (m_Alignment == eTabStripAlignment.Top || m_Alignment == eTabStripAlignment.Bottom)
			{
				if ((tabItem2.DisplayRectangle.X >= 4 || (int)((Control)this).RightToLeft != 0) && (tabItem2.DisplayRectangle.Right > ((Control)this).Width || (int)((Control)this).RightToLeft != 1))
				{
					tabItem = tabItem2;
					num = tabItem2.DisplayRectangle.Width;
					if (tabItem2.DisplayRectangle.Right > ((Control)this).Width - m_TabSystemBox.DefaultWidth)
					{
						break;
					}
				}
			}
			else if (tabItem2.DisplayRectangle.Y >= 4)
			{
				tabItem = tabItem2;
				num = tabItem2.DisplayRectangle.Height;
				if (tabItem2.DisplayRectangle.Bottom > ((Control)this).Height - m_TabSystemBox.DefaultWidth)
				{
					break;
				}
			}
		}
		if (tabItem != null)
		{
			if (m_Alignment == eTabStripAlignment.Top || m_Alignment == eTabStripAlignment.Bottom)
			{
				if (tabItem.DisplayRectangle.Right + 2 < ((Control)this).Width - m_TabSystemBox.DefaultWidth)
				{
					return;
				}
			}
			else if (tabItem.DisplayRectangle.Bottom + 2 < ((Control)this).Height - m_TabSystemBox.DefaultWidth)
			{
				return;
			}
		}
		int scrollOffset = m_ScrollOffset;
		int num5 = m_ScrollOffset + num;
		if (m_Alignment == eTabStripAlignment.Top || m_Alignment == eTabStripAlignment.Bottom)
		{
			if (num5 > m_Tabs[m_Tabs.Count - 1].DisplayRectangle.Right + m_ScrollOffset && (int)((Control)this).RightToLeft == 0)
			{
				num5 = m_Tabs[m_Tabs.Count - 1].DisplayRectangle.Right + m_ScrollOffset + m_TabSystemBox.DefaultWidth;
			}
			else if (num5 < 0 && (int)((Control)this).RightToLeft == 1)
			{
				num5 = 0;
			}
		}
		else if (num5 > m_Tabs[m_Tabs.Count - 1].DisplayRectangle.Bottom + m_ScrollOffset)
		{
			num5 = m_Tabs[m_Tabs.Count - 1].DisplayRectangle.Bottom + m_ScrollOffset + m_TabSystemBox.DefaultWidth;
		}
		if (m_Animate)
		{
			DateTime now = DateTime.Now;
			for (int j = scrollOffset; j < num5; j++)
			{
				ScrollOffset = j;
				if (DateTime.Now.Subtract(now).TotalMilliseconds > 200.0)
				{
					break;
				}
			}
		}
		ScrollOffset = num5;
	}

	private void OnTabClose(object sender, EventArgs e)
	{
		CloseSelectedTab();
	}

	private void CloseSelectedTab()
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		if (this.TabItemClose != null)
		{
			TabStripActionEventArgs tabStripActionEventArgs = new TabStripActionEventArgs();
			this.TabItemClose(this, tabStripActionEventArgs);
			if (tabStripActionEventArgs.Cancel)
			{
				return;
			}
		}
		if (SelectedTab == null)
		{
			return;
		}
		if (m_MdiTabbedDocuments && SelectedTab != null && SelectedTab.AttachedControl is Form)
		{
			((Form)SelectedTab.AttachedControl).Close();
		}
		else if (SelectedTab != null && SelectedTab.AttachedItem != null && SelectedTab.AttachedItem is DockContainerItem)
		{
			if (SelectedTab.AttachedItem.ContainerControl is Bar bar)
			{
				bar.CloseDockTab((DockContainerItem)SelectedTab.AttachedItem, eEventSource.Mouse);
			}
			else
			{
				SelectedTab.AttachedItem.Visible = false;
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeFixedTabSize()
	{
		return !m_FixedTabSize.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetFixedTabSize()
	{
		TypeDescriptor.GetProperties(this)["FixedTabSize"].SetValue(this, Size.Empty);
	}

	public TabItem HitTest(int x, int y)
	{
		if ((m_Style == eTabStripStyle.OneNote || m_Style == eTabStripStyle.VS2005Document || m_Style == eTabStripStyle.Office2007Document) && GetSelectedTabRectangle().Contains(x, y))
		{
			return SelectedTab;
		}
		Rectangle systemBoxRectangle = GetSystemBoxRectangle();
		foreach (TabItem tab in Tabs)
		{
			if (tab.Visible && tab.DisplayRectangle.Contains(x, y))
			{
				return tab;
			}
		}
		return null;
	}

	protected override void OnParentChanged(EventArgs e)
	{
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		((Control)this).OnParentChanged(e);
		RefreshColorScheme();
		if (m_MdiTabbedDocuments && m_MdiForm == null)
		{
			Form val = ((Control)this).FindForm();
			if (val != null && val.IsMdiContainer)
			{
				MdiForm = val;
			}
		}
		if ((m_Style == eTabStripStyle.OneNote || m_Style == eTabStripStyle.VS2005Document || m_Style == eTabStripStyle.Office2007Document) && m_SelectedTabFont == null)
		{
			m_SelectedTabFont = new Font(((Control)this).Font, (FontStyle)1);
		}
	}

	private void InitializeMdiForm()
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected O, but got Unknown
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		MdiClient mdiClient = GetMdiClient(m_MdiForm);
		if (mdiClient == null)
		{
			m_MdiForm.Load += ParentFormLoaded;
			return;
		}
		((Control)mdiClient).ControlAdded += new ControlEventHandler(MdiFormAdded);
		((Control)mdiClient).ControlRemoved += new ControlEventHandler(MdiFormRemoved);
		m_MdiForm.MdiChildActivate += MdiFormActivated;
		m_MdiInitialized = true;
	}

	private void ReleaseMdiForm()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		if (m_MdiInitialized)
		{
			MdiClient mdiClient = GetMdiClient(m_MdiForm);
			if (mdiClient != null)
			{
				((Control)mdiClient).ControlAdded -= new ControlEventHandler(MdiFormAdded);
				((Control)mdiClient).ControlRemoved -= new ControlEventHandler(MdiFormRemoved);
				m_MdiForm.MdiChildActivate -= MdiFormActivated;
				m_MdiInitialized = false;
			}
		}
	}

	private void ParentFormLoaded(object sender, EventArgs e)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		if (m_MdiForm != null)
		{
			m_MdiForm.Load -= ParentFormLoaded;
			if (!m_MdiInitialized)
			{
				InitializeMdiForm();
				if (m_MdiForm.MdiChildren.Length != 0)
				{
					RefreshMdiTabItems();
				}
			}
		}
		else if (sender is Form)
		{
			((Form)sender).Load -= ParentFormLoaded;
		}
	}

	internal void OnTabAdded(TabItem item)
	{
		HideToolTip();
		if (!m_MovingTab)
		{
			if (this.TabItemOpen != null)
			{
				this.TabItemOpen(item, new EventArgs());
			}
			if (VisibleTabCount == 1 && item.Visible && m_SelectedTab == null)
			{
				SelectedTab = item;
			}
		}
	}

	private string GetFormText(Form form)
	{
		if (form == null)
		{
			return "";
		}
		string text = ((Control)form).Text.Replace("&", "&&");
		if (text.Length > m_MaxMdiCaptionLength)
		{
			int num = m_MaxMdiCaptionLength / 2;
			return text.Substring(0, num) + "..." + text.Substring(text.Length - (m_MaxMdiCaptionLength - num));
		}
		return text;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void MdiFormAdded(object sender, ControlEventArgs e)
	{
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		if (m_MdiForm == null || !m_MdiInitialized)
		{
			return;
		}
		Control control = e.Control;
		Form val = (Form)(object)((control is Form) ? control : null);
		if (val == null)
		{
			RefreshMdiTabItems();
			return;
		}
		string formText = GetFormText(val);
		TabItem tabItem = new TabItem();
		tabItem.Text = formText;
		tabItem.AttachedControl = (Control)(object)val;
		if (m_ShowMdiChildIcon && val.Icon != null)
		{
			tabItem.Icon = new Icon(val.Icon, tabItem.IconSize);
		}
		m_Tabs.Add(tabItem);
		((Control)val).TextChanged += FormTextChanged;
		((Control)val).VisibleChanged += FormVisibleChanged;
		if (m_MdiForm.ActiveMdiChild == val)
		{
			SelectedTab = tabItem;
		}
		if (m_MdiAutoHide && m_MdiForm != null && !((Control)this).Visible && !IsDesignMode)
		{
			BarFunctions.SetControlVisible((Control)(object)this, visible: true);
		}
		((Control)this).Invalidate();
	}

	private void RefreshMdiTabItems()
	{
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		if (m_MdiForm == null || !m_MdiTabbedDocuments || !BarFunctions.IsHandleValid((Control)(object)this))
		{
			return;
		}
		if (m_Tabs.Count > 0)
		{
			foreach (TabItem tab in m_Tabs)
			{
				if (tab.AttachedControl is Form)
				{
					Form val = (Form)tab.AttachedControl;
					((Control)val).TextChanged -= FormTextChanged;
					((Control)val).VisibleChanged -= FormVisibleChanged;
				}
				tab.Dispose();
			}
		}
		m_Tabs.Clear();
		Form[] mdiChildren = m_MdiForm.MdiChildren;
		foreach (Form val2 in mdiChildren)
		{
			string formText = GetFormText(val2);
			TabItem tabItem2 = new TabItem();
			tabItem2.Text = formText;
			tabItem2.AttachedControl = (Control)(object)val2;
			if (m_ShowMdiChildIcon && val2.Icon != null)
			{
				tabItem2.Icon = new Icon(val2.Icon, tabItem2.IconSize);
			}
			m_Tabs.Add(tabItem2);
			tabItem2.Visible = ((Control)val2).Visible;
			((Control)val2).TextChanged += FormTextChanged;
			((Control)val2).VisibleChanged += FormVisibleChanged;
			if (m_MdiForm.ActiveMdiChild == val2)
			{
				SelectedTab = tabItem2;
			}
		}
		if (m_MdiAutoHide && m_MdiForm != null)
		{
			if (m_MdiForm.MdiChildren.Length == 0 && ((Control)this).Visible && !IsDesignMode)
			{
				((Control)this).Visible = false;
			}
			else if (m_MdiForm.MdiChildren.Length != 0 && !((Control)this).Visible && !IsDesignMode)
			{
				BarFunctions.SetControlVisible((Control)(object)this, visible: true);
			}
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void MdiFormRemoved(object sender, ControlEventArgs e)
	{
		Control control = e.Control;
		Form val = (Form)(object)((control is Form) ? control : null);
		if (val == null)
		{
			if (this.TabItemClose != null)
			{
				TabStripActionEventArgs e2 = new TabStripActionEventArgs();
				this.TabItemClose(this, e2);
			}
			RefreshMdiTabItems();
			return;
		}
		foreach (TabItem tab in m_Tabs)
		{
			if (tab.AttachedControl == val)
			{
				if (this.TabItemClose != null)
				{
					TabStripActionEventArgs e3 = new TabStripActionEventArgs();
					this.TabItemClose(tab, e3);
				}
				m_Tabs.Remove(tab);
				tab.AttachedControl = null;
				tab.Dispose();
				((Control)val).TextChanged -= FormTextChanged;
				((Control)val).VisibleChanged -= FormVisibleChanged;
				((Control)this).Invalidate();
				break;
			}
		}
		if (m_MdiAutoHide && m_MdiForm != null && m_MdiForm.MdiChildren.Length == 0 && !IsDesignMode)
		{
			((Control)this).Visible = false;
		}
	}

	private void MdiFormActivated(object sender, EventArgs e)
	{
		Form activeMdiChild = m_MdiForm.ActiveMdiChild;
		foreach (TabItem tab in m_Tabs)
		{
			if (tab.AttachedControl == activeMdiChild)
			{
				if (SelectedTab != tab)
				{
					SelectedTab = tab;
				}
				break;
			}
		}
	}

	private void FormTextChanged(object sender, EventArgs e)
	{
		Form val = (Form)((sender is Form) ? sender : null);
		if (val == null)
		{
			return;
		}
		foreach (TabItem tab in m_Tabs)
		{
			if (tab.AttachedControl == val)
			{
				tab.Text = GetFormText(val);
				break;
			}
		}
	}

	private void FormVisibleChanged(object sender, EventArgs e)
	{
		Form val = (Form)((sender is Form) ? sender : null);
		if (val == null)
		{
			return;
		}
		foreach (TabItem tab in m_Tabs)
		{
			if (tab.AttachedControl == val)
			{
				tab.Visible = ((Control)val).Visible;
				break;
			}
		}
	}

	private MdiClient GetMdiClient(Form MdiForm)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		if (!MdiForm.IsMdiContainer)
		{
			return null;
		}
		foreach (Control item in (ArrangedElementCollection)((Control)MdiForm).Controls)
		{
			Control val = item;
			if (val is MdiClient)
			{
				return (MdiClient)(object)((val is MdiClient) ? val : null);
			}
		}
		return null;
	}

	internal Color GetBackColorFlat()
	{
		Color result = ControlPaint.Light(SystemColors.Control);
		if (BarFunctions.ThemedOS && NativeFunctions.ColorDepth >= 16)
		{
			if (SystemColors.Control.ToArgb() == Color.FromArgb(236, 233, 216).ToArgb() && SystemColors.Highlight.ToArgb() == Color.FromArgb(49, 106, 197).ToArgb())
			{
				result = Color.FromArgb(255, 251, 233);
			}
			else if (SystemColors.Control.ToArgb() == Color.FromArgb(224, 223, 227).ToArgb() && SystemColors.Highlight.ToArgb() == Color.FromArgb(178, 180, 191).ToArgb())
			{
				result = Color.FromArgb(251, 250, 255);
			}
			else if (SystemColors.Control.ToArgb() == Color.FromArgb(236, 233, 216).ToArgb() && SystemColors.Highlight.ToArgb() == Color.FromArgb(147, 160, 112).ToArgb())
			{
				result = Color.FromArgb(255, 251, 233);
			}
		}
		return result;
	}

	public void HideToolTip()
	{
		if (m_ToolTip != null)
		{
			((Control)m_ToolTip).Hide();
			((Component)(object)m_ToolTip).Dispose();
			m_ToolTip = null;
		}
		m_TooltipTab = null;
	}

	private void ShowToolTip(TabItem tab)
	{
		if (m_ToolTip != null)
		{
			HideToolTip();
		}
		if (tab.Tooltip != null && !(tab.Tooltip == "") && ((Control)this).Visible)
		{
			if (m_ToolTip == null)
			{
				m_ToolTip = new ToolTip();
			}
			m_ToolTip.Text = tab.Tooltip;
			m_ToolTip.ShowToolTip();
			m_TooltipTab = tab;
		}
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

	private void InternalMouseHover()
	{
		Point mousePosition = Control.MousePosition;
		mousePosition = ((Control)this).PointToClient(mousePosition);
		TabItem tabItem = HitTest(mousePosition.X, mousePosition.Y);
		tabItem?.InvokeMouseHover(new EventArgs());
		if (tabItem == null)
		{
			HideToolTip();
		}
		else
		{
			if (m_TooltipTab == tabItem)
			{
				return;
			}
			HideToolTip();
			if (Style == eTabStripStyle.Office2007Document)
			{
				Rectangle systemBoxRectangle = GetSystemBoxRectangle();
				if (!systemBoxRectangle.IsEmpty && systemBoxRectangle.IntersectsWith(tabItem.DisplayRectangle))
				{
					return;
				}
			}
			ShowToolTip(tabItem);
		}
	}
}
