using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using DevComponents.UI.ContentManager;

namespace DevComponents.DotNetBar;

[ComVisible(false)]
[ToolboxItem(true)]
[DefaultEvent("SelectedTabChanged")]
[Designer("DevComponents.DotNetBar.Design.TabStripDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
public class TabStrip : Control
{
	public delegate void SelectedTabChangedEventHandler(object sender, TabStripTabChangedEventArgs e);

	public delegate void SelectedTabChangingEventHandler(object sender, TabStripTabChangingEventArgs e);

	public delegate void UserActionEventHandler(object sender, TabStripActionEventArgs e);

	public delegate void TabMovedEventHandler(object sender, TabStripTabMovedEventArgs e);

	private SelectedTabChangedEventHandler Ꮷ;

	private SelectedTabChangingEventHandler ᓩ;

	private TabMovedEventHandler ᓪ;

	private UserActionEventHandler ᓫ;

	private UserActionEventHandler ᓬ;

	private UserActionEventHandler ᓭ;

	private EventHandler ᓮ;

	private EventHandler _0C55;

	private EventHandler ᓯ;

	private EventHandler ᓰ;

	private MeasureTabItemEventHandler ᓱ;

	private RenderTabItemEventHandler ᓲ;

	private RenderTabItemEventHandler ᓳ;

	private EventHandler ᓴ;

	private EventHandler ᓵ;

	private MouseEventHandler ᓶ;

	private MouseEventHandler ᓷ;

	private EventHandler ᓸ;

	private eTabStripAlignment ᓹ = eTabStripAlignment.Bottom;

	private TabsCollection ᓺ;

	private ImageList գ;

	private TabItem ᓻ;

	private bool ᓼ = true;

	private bool ᓽ;

	private Bar ᓾ;

	private bool ᓿ;

	private bool _0B79 = true;

	private bool ᔀ = true;

	private int ᔁ;

	private TabSystemBox ᔂ;

	private bool ᔃ = true;

	private bool ᔄ;

	private Form ᔅ;

	private bool ᔆ;

	private int ᔇ = 32;

	private bool ᔈ = true;

	private bool ᔉ = true;

	private bool ᔊ = true;

	private Font ᔋ;

	private bool ᔌ;

	private bool ᔍ;

	private ᑧ ᑬ;

	private TabItem ᔎ;

	private eTabStripStyle Ӷ;

	private Color ᔏ = Color.Empty;

	private Color ᔐ = Color.Empty;

	private Color ᔑ = Color.Empty;

	private Color ᔒ = Color.Empty;

	private bool ᔓ;

	private int ᔔ = 300;

	private TabColorScheme լ;

	private TabItem ᔕ;

	private bool ᑫ = true;

	private bool ᔖ;

	private int ᔗ = 4;

	private int ᔘ;

	private eTabLayoutType ᔙ;

	private ToolTip _0CA9;

	private TabItem ᔚ;

	private bool _09F8;

	private Rectangle ᔛ = Rectangle.Empty;

	private Size ᔜ = Size.Empty;

	private bool ᔝ;

	private eTabCloseButtonPosition ᔞ;

	private static Size ᔟ = new Size(11, 11);

	private Size ᔠ = ᔟ;

	private Image ᔡ;

	private Image ᔢ;

	private bool ᔣ = true;

	private bool ե;

	private bool ᔤ = true;

	private int ᔥ = -1;

	private bool _ᔦ = true;

	private bool _ᅸ = true;

	private Point _ᐨ = Point.Empty;

	private bool ᔧ;

	private Cursor _נ;

	private Cursor _ᔨ;

	internal bool _ᔩ;

	[DefaultValue(false)]
	[Browsable(true)]
	[Category("Appearance")]
	[Description("Gets or sets whether anti-aliasing is used while painting.")]
	public bool AntiAlias
	{
		get
		{
			return ե;
		}
		set
		{
			if (ե != value)
			{
				ե = value;
				((Control)this).Invalidate();
			}
		}
	}

	[Browsable(false)]
	public int MinTabStripHeight
	{
		get
		{
			if (Ӷ == eTabStripStyle.SimulatedTheme || Ӷ == eTabStripStyle.Office2007Document || Ӷ == eTabStripStyle.Office2007Dock)
			{
				if (ᔛ.IsEmpty || ᓼ)
				{
					if (Tabs.Count == 0 || !ᓼ)
					{
						return ᓘ + 6;
					}
					RecalcSize();
				}
				if (ᓹ == eTabStripAlignment.Top || ᓹ == eTabStripAlignment.Bottom)
				{
					return ᔛ.Height + 3;
				}
				return ᔛ.Width + 3;
			}
			if (Ӷ == eTabStripStyle.OneNote || Ӷ == eTabStripStyle.VS2005Document)
			{
				return ᓘ + 6;
			}
			return ᓘ + 5;
		}
	}

	private int ᓘ
	{
		get
		{
			if (ᔥ <= 0)
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
						ᔥ = ᕄ(val);
					}
					finally
					{
						val.Dispose();
					}
				}
			}
			return ᔥ;
		}
	}

	private bool ᓙ
	{
		get
		{
			if ((ᓹ == eTabStripAlignment.Left || ᓹ == eTabStripAlignment.Right) && Style != eTabStripStyle.SimulatedTheme)
			{
				return true;
			}
			return false;
		}
	}

	internal Rectangle ᓚ => ᔛ;

	private bool ᓛ => (int)((Control)this).RightToLeft == 1;

	private bool ᓜ
	{
		get
		{
			if (((Style == eTabStripStyle.RoundHeader || Style == eTabStripStyle.VS2005Dock) && !ᓨ) || Style == eTabStripStyle.SimulatedTheme || Style == eTabStripStyle.Office2007Document || Style == eTabStripStyle.Office2007Dock)
			{
				return true;
			}
			return false;
		}
	}

	internal bool ᓝ => ᓱ != null;

	internal bool ᓞ => ᓲ != null;

	internal bool ᓟ => ᓳ != null;

	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public TabItem DesignTimeSelection
	{
		get
		{
			return ᔕ;
		}
		set
		{
			ᔕ = value;
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
			if (ᓻ == null)
			{
				return -1;
			}
			return ᓺ.IndexOf(ᓻ);
		}
		set
		{
			if (value >= 0 && value < ᓺ.Count && ᓺ.Count != 0)
			{
				SelectedTab = ᓺ[value];
			}
		}
	}

	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Browsable(true)]
	[Description("Indicates selected tab.")]
	public TabItem SelectedTab
	{
		get
		{
			if (ᓻ == null && ᓺ.Count > 0)
			{
				ᓻ = ᓺ[0];
			}
			return ᓻ;
		}
		set
		{
			ᕒ(value, eEventSource.Code);
		}
	}

	[Browsable(true)]
	[Description("Indicates whether tabs are scrolled continuously while mouse is pressed over the scroll tab button.")]
	[Category("Behavior")]
	[DefaultValue(false)]
	[DevCoBrowsable(true)]
	public virtual bool TabScrollAutoRepeat
	{
		get
		{
			return ᔓ;
		}
		set
		{
			ᔓ = value;
		}
	}

	[Category("Behavior")]
	[Description("Indicates auto-repeat interval for the tab scrolling while mouse button is kept pressed over the scroll tab button.")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	[DefaultValue(300)]
	public virtual int TabScrollRepeatInterval
	{
		get
		{
			return ᔔ;
		}
		set
		{
			ᔔ = value;
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
			if (ᔂ != null)
			{
				return ᔂ.CloseVisible;
			}
			return true;
		}
		set
		{
			if (ᔂ != null)
			{
				ᔂ.CloseVisible = value;
			}
			((Control)this).Invalidate();
		}
	}

	[DevCoBrowsable(true)]
	[Category("Close Button")]
	[DefaultValue(false)]
	[Description("Indicates whether close button is visible on each tab instead of in system box.")]
	[Browsable(true)]
	public bool CloseButtonOnTabsVisible
	{
		get
		{
			return ᔝ;
		}
		set
		{
			ᔝ = value;
			ᓼ = true;
			((Control)this).Invalidate();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Close Button")]
	[Description("Indicates whether close button on tabs when visible is displayed for every tab state.")]
	[DefaultValue(true)]
	public bool CloseButtonOnTabsAlwaysDisplayed
	{
		get
		{
			return ᔣ;
		}
		set
		{
			ᔣ = value;
			((Control)this).Invalidate();
		}
	}

	[Category("Close Button")]
	[DefaultValue(eTabCloseButtonPosition.Left)]
	[DevCoBrowsable(true)]
	[Description("Indicates position of the close button displayed on each tab.")]
	[Browsable(true)]
	public eTabCloseButtonPosition CloseButtonPosition
	{
		get
		{
			return ᔞ;
		}
		set
		{
			ᔞ = value;
			ᓼ = true;
			((Control)this).Invalidate();
		}
	}

	[Description("Indicates custom image that is used on tabs as Close button that allows user to close the tab.")]
	[DefaultValue(null)]
	[Category("Close Button")]
	[Browsable(true)]
	public Image TabCloseButtonNormal
	{
		get
		{
			return ᔡ;
		}
		set
		{
			ᔡ = value;
			if (ᔡ != null)
			{
				ᔠ = ᔡ.Size;
			}
			else
			{
				ᔠ = ᔟ;
			}
			ᓼ = true;
			((Control)this).Invalidate();
		}
	}

	[Description("Indicates custom image that is used on tabs as Close button whem mouse is over the close button.")]
	[Category("Close Button")]
	[Browsable(true)]
	[DefaultValue(null)]
	public Image TabCloseButtonHot
	{
		get
		{
			return ᔢ;
		}
		set
		{
			ᔢ = value;
		}
	}

	[Editor("DevComponents.DotNetBar.Design.TabStripTabsEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Category("Data")]
	[Description("Returns the collection of Tabs.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public TabsCollection Tabs => ᓺ;

	[Browsable(false)]
	public bool AutoSelectAttachedControl
	{
		get
		{
			return ᔤ;
		}
		set
		{
			ᔤ = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(null)]
	[Category("Appearance")]
	public ImageList ImageList
	{
		get
		{
			return գ;
		}
		set
		{
			գ = value;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates whether selected tab is aligned with the panel in multiple lines use case.")]
	[DefaultValue(true)]
	public bool MultiLinePanelAlignSelectedTab
	{
		get
		{
			return _ᔦ;
		}
		set
		{
			_ᔦ = value;
			ᓼ = true;
		}
	}

	[Description("Indicates the type of the tab layout.")]
	[DefaultValue(eTabLayoutType.FitContainer)]
	[Category("Appearance")]
	[Browsable(true)]
	public eTabLayoutType TabLayoutType
	{
		get
		{
			return ᔙ;
		}
		set
		{
			ᔙ = value;
			ᓼ = true;
			ᔂ.DisplayRectangle = Rectangle.Empty;
			ᕚ();
		}
	}

	[Obsolete("Please use TabLayoutType property instead.", true)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DevCoBrowsable(false)]
	public bool VariableTabWidth
	{
		get
		{
			return ᔀ;
		}
		set
		{
			if (value)
			{
				ᔙ = eTabLayoutType.FitContainer;
			}
			else
			{
				ᔙ = eTabLayoutType.FixedWithNavigationBox;
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DefaultValue(0)]
	[Browsable(false)]
	[DevCoBrowsable(false)]
	public int ScrollOffset
	{
		get
		{
			return ᔁ;
		}
		set
		{
			if (ᔁ != value)
			{
				ᔁ = value;
				ᓼ = true;
				((Control)this).Invalidate();
			}
		}
	}

	internal bool Ꮬ
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

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public TabSystemBox _TabSystemBox => ᔂ;

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether keyboard navigation using Left and Right arrow keys to select tabs is enabled.")]
	public bool KeyboardNavigationEnabled
	{
		get
		{
			return _ᅸ;
		}
		set
		{
			_ᅸ = value;
		}
	}

	internal bool ᓠ
	{
		get
		{
			if (ᔙ != eTabLayoutType.FixedWithNavigationBox)
			{
				return ᔙ == eTabLayoutType.MultilineWithNavigationBox;
			}
			return true;
		}
	}

	internal bool ᓡ
	{
		get
		{
			if (ᔙ != eTabLayoutType.MultilineNoNavigationBox)
			{
				return ᔙ == eTabLayoutType.MultilineWithNavigationBox;
			}
			return true;
		}
	}

	private bool ᓢ
	{
		get
		{
			return ᓽ;
		}
		set
		{
			if (ᓽ == value)
			{
				return;
			}
			ᓽ = value;
			if (ᓽ)
			{
				if (!ᓣ)
				{
					_נ = ((Control)this).Cursor;
					((Control)this).Cursor = ᕕ();
				}
			}
			else if (!ᓣ)
			{
				((Control)this).Cursor = _נ;
			}
		}
	}

	[Browsable(true)]
	[Description("Specifies the mouse cursor that is displayed when tab is dragged.")]
	[DefaultValue(null)]
	[Category("Appearance")]
	public Cursor TabDragCursor
	{
		get
		{
			return _ᔨ;
		}
		set
		{
			_ᔨ = value;
		}
	}

	private bool ᓣ => ((Control)this).Parent is Bar;

	internal TabItem ᓤ => ᔎ;

	internal int ᓥ
	{
		get
		{
			int num = 0;
			foreach (TabItem item in ᓺ)
			{
				if (item.Visible)
				{
					num++;
				}
			}
			return num;
		}
	}

	internal bool ᓦ => ᓿ;

	[Description("Gets or sets the selected tab Font")]
	[Browsable(true)]
	[DefaultValue(null)]
	[Category("Style")]
	[DevCoBrowsable(true)]
	public Font SelectedTabFont
	{
		get
		{
			return ᔋ;
		}
		set
		{
			ᔋ = value;
			if (ᔋ == null)
			{
				ᔌ = false;
			}
			else
			{
				ᔌ = true;
			}
			ᓼ = true;
			((Control)this).Invalidate();
		}
	}

	[Category("Appearance")]
	[Description("Indicates the tab alignment within the Tab-Strip control.")]
	[DevCoBrowsable(true)]
	[DefaultValue(eTabStripAlignment.Bottom)]
	[Browsable(true)]
	public eTabStripAlignment TabAlignment
	{
		get
		{
			return ᓹ;
		}
		set
		{
			if (ᓹ != value)
			{
				ᓹ = value;
				ᓼ = true;
				((Control)this).Invalidate();
			}
		}
	}

	internal bool ᓧ
	{
		get
		{
			return ᓼ;
		}
		set
		{
			ᓼ = value;
		}
	}

	[Category("Behavior")]
	[Description("Specifes whether end-user can reorder the tabs.")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	public bool CanReorderTabs
	{
		get
		{
			return _0B79;
		}
		set
		{
			if (_0B79 != value)
			{
				_0B79 = value;
			}
		}
	}

	[DefaultValue(false)]
	[Description("Indicates whether system box that enables scrolling and closing of the tabs is automatically hidden when tab items size does not exceed the size of the control.")]
	[Category("Behavior")]
	[Browsable(true)]
	public bool AutoHideSystemBox
	{
		get
		{
			return ᔖ;
		}
		set
		{
			if (ᔖ != value)
			{
				ᔖ = value;
				ᓼ = true;
				if (Ꮬ)
				{
					((Control)this).Invalidate();
				}
			}
		}
	}

	[Category("Style")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Obsolete("This property is obsolete. Please use ColorScheme property to change all tab colors.")]
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Description("Indicates the background color.")]
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

	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Browsable(true)]
	[Description("Specifes whether only selected tab is displaying it's text.")]
	[DefaultValue(false)]
	public bool DisplaySelectedTextOnly
	{
		get
		{
			return ᔍ;
		}
		set
		{
			ᔍ = value;
			ᓼ = true;
			((Control)this).Invalidate();
		}
	}

	[Category("Appearance")]
	[Description("Specifes the TabStrip style.")]
	[DevCoBrowsable(true)]
	[DefaultValue(eTabStripStyle.Flat)]
	[Browsable(true)]
	public eTabStripStyle Style
	{
		get
		{
			return Ӷ;
		}
		set
		{
			Ӷ = value;
			ᕚ();
		}
	}

	[DefaultValue(false)]
	[Description("Specifies whether tab is drawn using Themes when running on OS that supports themes like Windows XP.")]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Browsable(true)]
	public virtual bool ThemeAware
	{
		get
		{
			return _09F8;
		}
		set
		{
			_09F8 = value;
			ᓼ = true;
			լ._1CFB = ᓨ;
			((Control)this).Invalidate();
		}
	}

	internal bool ᓨ
	{
		get
		{
			if (_09F8 && BarFunctions.ThemedOS && _0EEE._0EF0 && Ӷ != eTabStripStyle.SimulatedTheme && Ӷ != eTabStripStyle.Office2007Dock && Ӷ != eTabStripStyle.Office2007Document && Ӷ != eTabStripStyle.Metro)
			{
				return true;
			}
			return false;
		}
	}

	[Category("Style")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[DevCoBrowsable(true)]
	[Description("Gets or sets Tab Color Scheme.")]
	[Browsable(true)]
	public TabColorScheme ColorScheme
	{
		get
		{
			return լ;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentException("NULL is not a valid value for this property.");
			}
			if (լ != null)
			{
				լ.ColorChanged -= ᕜ;
			}
			լ = value;
			լ.ColorChanged += ᕜ;
			if (((Control)this).Visible)
			{
				((Control)this).Invalidate();
			}
		}
	}

	[Description("Indicates whether the tab scrolling is animanted.")]
	[DevCoBrowsable(true)]
	[DefaultValue(true)]
	[Browsable(true)]
	[Category("Behavior")]
	public bool Animate
	{
		get
		{
			return ᔃ;
		}
		set
		{
			ᔃ = value;
		}
	}

	[Category("Appearance")]
	[Browsable(true)]
	[Description("Gets or sets the fixed tab size in pixels. Either Height or Width can be set or both.")]
	public Size FixedTabSize
	{
		get
		{
			return ᔜ;
		}
		set
		{
			ᔜ = value;
			ᓼ = true;
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
			return ᑫ;
		}
		set
		{
			ᑫ = value;
		}
	}

	[DefaultValue(false)]
	[Category("Mdi Support")]
	[Description("Indicates whether Tab-Strip control provides Tabbed MDI Child form support.")]
	[Browsable(true)]
	public bool MdiTabbedDocuments
	{
		get
		{
			return ᔄ;
		}
		set
		{
			if (ᔄ == value)
			{
				return;
			}
			ᔄ = value;
			if (ᔅ == null && ᔄ)
			{
				Form val = ((Control)this).FindForm();
				if (val != null && val.IsMdiContainer)
				{
					MdiForm = val;
				}
			}
			else if (!ᔄ)
			{
				MdiForm = null;
				ᓺ.Clear();
				((Control)this).Invalidate();
			}
		}
	}

	[Description("Indicates the maximum number of characters that will be used as Tab text from Mdi Child caption.")]
	[Category("Mdi Support")]
	[DevCoBrowsable(true)]
	[DefaultValue(32)]
	[Browsable(true)]
	public int MaxMdiCaptionLength
	{
		get
		{
			return ᔇ;
		}
		set
		{
			if (ᔇ != value)
			{
				ᔇ = value;
			}
		}
	}

	[DefaultValue(true)]
	[Category("Mdi Support")]
	[Description("Indicates whether the Mdi Child Icon is displayed on Tab.")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	public bool ShowMdiChildIcon
	{
		get
		{
			return ᔈ;
		}
		set
		{
			if (ᔈ != value)
			{
				ᔈ = value;
			}
		}
	}

	[Description("Indicates whether the Tab-strip is automatically hidden when there are not Mdi Child forms open.")]
	[Category("Mdi Support")]
	[DevCoBrowsable(true)]
	[DefaultValue(true)]
	[Browsable(true)]
	public bool MdiAutoHide
	{
		get
		{
			return ᔉ;
		}
		set
		{
			if (ᔉ != value)
			{
				ᔉ = value;
			}
		}
	}

	[Description("Indicates whether flicker associated with switching maximized Mdi child forms is attempted to eliminate.")]
	[Category("Mdi Support")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	[DefaultValue(true)]
	public bool MdiNoFormActivateFlicker
	{
		get
		{
			return ᔊ;
		}
		set
		{
			if (ᔊ != value)
			{
				ᔊ = value;
			}
		}
	}

	[Browsable(true)]
	[Description("Indicates Mdi Container form for which Tab-Strip is providing Tabbed MDI Child support.")]
	[DefaultValue(null)]
	public Form MdiForm
	{
		get
		{
			return ᔅ;
		}
		set
		{
			if (ᔅ != null)
			{
				ᕣ();
			}
			ᔅ = value;
			if (ᔅ != null && !((Component)this).DesignMode)
			{
				ᕢ();
				ᕨ();
			}
		}
	}

	public event SelectedTabChangedEventHandler SelectedTabChanged
	{
		add
		{
			SelectedTabChangedEventHandler selectedTabChangedEventHandler = Ꮷ;
			SelectedTabChangedEventHandler selectedTabChangedEventHandler2;
			do
			{
				selectedTabChangedEventHandler2 = selectedTabChangedEventHandler;
				SelectedTabChangedEventHandler value2 = (SelectedTabChangedEventHandler)Delegate.Combine(selectedTabChangedEventHandler2, value);
				selectedTabChangedEventHandler = Interlocked.CompareExchange(ref Ꮷ, value2, selectedTabChangedEventHandler2);
			}
			while ((object)selectedTabChangedEventHandler != selectedTabChangedEventHandler2);
		}
		remove
		{
			SelectedTabChangedEventHandler selectedTabChangedEventHandler = Ꮷ;
			SelectedTabChangedEventHandler selectedTabChangedEventHandler2;
			do
			{
				selectedTabChangedEventHandler2 = selectedTabChangedEventHandler;
				SelectedTabChangedEventHandler value2 = (SelectedTabChangedEventHandler)Delegate.Remove(selectedTabChangedEventHandler2, value);
				selectedTabChangedEventHandler = Interlocked.CompareExchange(ref Ꮷ, value2, selectedTabChangedEventHandler2);
			}
			while ((object)selectedTabChangedEventHandler != selectedTabChangedEventHandler2);
		}
	}

	public event SelectedTabChangingEventHandler SelectedTabChanging
	{
		add
		{
			SelectedTabChangingEventHandler selectedTabChangingEventHandler = ᓩ;
			SelectedTabChangingEventHandler selectedTabChangingEventHandler2;
			do
			{
				selectedTabChangingEventHandler2 = selectedTabChangingEventHandler;
				SelectedTabChangingEventHandler value2 = (SelectedTabChangingEventHandler)Delegate.Combine(selectedTabChangingEventHandler2, value);
				selectedTabChangingEventHandler = Interlocked.CompareExchange(ref ᓩ, value2, selectedTabChangingEventHandler2);
			}
			while ((object)selectedTabChangingEventHandler != selectedTabChangingEventHandler2);
		}
		remove
		{
			SelectedTabChangingEventHandler selectedTabChangingEventHandler = ᓩ;
			SelectedTabChangingEventHandler selectedTabChangingEventHandler2;
			do
			{
				selectedTabChangingEventHandler2 = selectedTabChangingEventHandler;
				SelectedTabChangingEventHandler value2 = (SelectedTabChangingEventHandler)Delegate.Remove(selectedTabChangingEventHandler2, value);
				selectedTabChangingEventHandler = Interlocked.CompareExchange(ref ᓩ, value2, selectedTabChangingEventHandler2);
			}
			while ((object)selectedTabChangingEventHandler != selectedTabChangingEventHandler2);
		}
	}

	public event TabMovedEventHandler TabMoved
	{
		add
		{
			TabMovedEventHandler tabMovedEventHandler = ᓪ;
			TabMovedEventHandler tabMovedEventHandler2;
			do
			{
				tabMovedEventHandler2 = tabMovedEventHandler;
				TabMovedEventHandler value2 = (TabMovedEventHandler)Delegate.Combine(tabMovedEventHandler2, value);
				tabMovedEventHandler = Interlocked.CompareExchange(ref ᓪ, value2, tabMovedEventHandler2);
			}
			while ((object)tabMovedEventHandler != tabMovedEventHandler2);
		}
		remove
		{
			TabMovedEventHandler tabMovedEventHandler = ᓪ;
			TabMovedEventHandler tabMovedEventHandler2;
			do
			{
				tabMovedEventHandler2 = tabMovedEventHandler;
				TabMovedEventHandler value2 = (TabMovedEventHandler)Delegate.Remove(tabMovedEventHandler2, value);
				tabMovedEventHandler = Interlocked.CompareExchange(ref ᓪ, value2, tabMovedEventHandler2);
			}
			while ((object)tabMovedEventHandler != tabMovedEventHandler2);
		}
	}

	public event UserActionEventHandler NavigateBack
	{
		add
		{
			UserActionEventHandler userActionEventHandler = ᓫ;
			UserActionEventHandler userActionEventHandler2;
			do
			{
				userActionEventHandler2 = userActionEventHandler;
				UserActionEventHandler value2 = (UserActionEventHandler)Delegate.Combine(userActionEventHandler2, value);
				userActionEventHandler = Interlocked.CompareExchange(ref ᓫ, value2, userActionEventHandler2);
			}
			while ((object)userActionEventHandler != userActionEventHandler2);
		}
		remove
		{
			UserActionEventHandler userActionEventHandler = ᓫ;
			UserActionEventHandler userActionEventHandler2;
			do
			{
				userActionEventHandler2 = userActionEventHandler;
				UserActionEventHandler value2 = (UserActionEventHandler)Delegate.Remove(userActionEventHandler2, value);
				userActionEventHandler = Interlocked.CompareExchange(ref ᓫ, value2, userActionEventHandler2);
			}
			while ((object)userActionEventHandler != userActionEventHandler2);
		}
	}

	public event UserActionEventHandler NavigateForward
	{
		add
		{
			UserActionEventHandler userActionEventHandler = ᓬ;
			UserActionEventHandler userActionEventHandler2;
			do
			{
				userActionEventHandler2 = userActionEventHandler;
				UserActionEventHandler value2 = (UserActionEventHandler)Delegate.Combine(userActionEventHandler2, value);
				userActionEventHandler = Interlocked.CompareExchange(ref ᓬ, value2, userActionEventHandler2);
			}
			while ((object)userActionEventHandler != userActionEventHandler2);
		}
		remove
		{
			UserActionEventHandler userActionEventHandler = ᓬ;
			UserActionEventHandler userActionEventHandler2;
			do
			{
				userActionEventHandler2 = userActionEventHandler;
				UserActionEventHandler value2 = (UserActionEventHandler)Delegate.Remove(userActionEventHandler2, value);
				userActionEventHandler = Interlocked.CompareExchange(ref ᓬ, value2, userActionEventHandler2);
			}
			while ((object)userActionEventHandler != userActionEventHandler2);
		}
	}

	public event UserActionEventHandler TabItemClose
	{
		add
		{
			UserActionEventHandler userActionEventHandler = ᓭ;
			UserActionEventHandler userActionEventHandler2;
			do
			{
				userActionEventHandler2 = userActionEventHandler;
				UserActionEventHandler value2 = (UserActionEventHandler)Delegate.Combine(userActionEventHandler2, value);
				userActionEventHandler = Interlocked.CompareExchange(ref ᓭ, value2, userActionEventHandler2);
			}
			while ((object)userActionEventHandler != userActionEventHandler2);
		}
		remove
		{
			UserActionEventHandler userActionEventHandler = ᓭ;
			UserActionEventHandler userActionEventHandler2;
			do
			{
				userActionEventHandler2 = userActionEventHandler;
				UserActionEventHandler value2 = (UserActionEventHandler)Delegate.Remove(userActionEventHandler2, value);
				userActionEventHandler = Interlocked.CompareExchange(ref ᓭ, value2, userActionEventHandler2);
			}
			while ((object)userActionEventHandler != userActionEventHandler2);
		}
	}

	public event EventHandler TabItemOpen
	{
		add
		{
			EventHandler eventHandler = ᓮ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᓮ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ᓮ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᓮ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler BeforeTabDisplay
	{
		add
		{
			EventHandler eventHandler = _0C55;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0C55, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _0C55;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0C55, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler TabRemoved
	{
		add
		{
			EventHandler eventHandler = ᓯ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᓯ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ᓯ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᓯ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler TabsCleared
	{
		add
		{
			EventHandler eventHandler = ᓰ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᓰ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ᓰ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᓰ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	internal event EventHandler ᓗ;

	public event MeasureTabItemEventHandler MeasureTabItem
	{
		add
		{
			MeasureTabItemEventHandler measureTabItemEventHandler = ᓱ;
			MeasureTabItemEventHandler measureTabItemEventHandler2;
			do
			{
				measureTabItemEventHandler2 = measureTabItemEventHandler;
				MeasureTabItemEventHandler value2 = (MeasureTabItemEventHandler)Delegate.Combine(measureTabItemEventHandler2, value);
				measureTabItemEventHandler = Interlocked.CompareExchange(ref ᓱ, value2, measureTabItemEventHandler2);
			}
			while ((object)measureTabItemEventHandler != measureTabItemEventHandler2);
		}
		remove
		{
			MeasureTabItemEventHandler measureTabItemEventHandler = ᓱ;
			MeasureTabItemEventHandler measureTabItemEventHandler2;
			do
			{
				measureTabItemEventHandler2 = measureTabItemEventHandler;
				MeasureTabItemEventHandler value2 = (MeasureTabItemEventHandler)Delegate.Remove(measureTabItemEventHandler2, value);
				measureTabItemEventHandler = Interlocked.CompareExchange(ref ᓱ, value2, measureTabItemEventHandler2);
			}
			while ((object)measureTabItemEventHandler != measureTabItemEventHandler2);
		}
	}

	public event RenderTabItemEventHandler PreRenderTabItem
	{
		add
		{
			RenderTabItemEventHandler renderTabItemEventHandler = ᓲ;
			RenderTabItemEventHandler renderTabItemEventHandler2;
			do
			{
				renderTabItemEventHandler2 = renderTabItemEventHandler;
				RenderTabItemEventHandler value2 = (RenderTabItemEventHandler)Delegate.Combine(renderTabItemEventHandler2, value);
				renderTabItemEventHandler = Interlocked.CompareExchange(ref ᓲ, value2, renderTabItemEventHandler2);
			}
			while ((object)renderTabItemEventHandler != renderTabItemEventHandler2);
		}
		remove
		{
			RenderTabItemEventHandler renderTabItemEventHandler = ᓲ;
			RenderTabItemEventHandler renderTabItemEventHandler2;
			do
			{
				renderTabItemEventHandler2 = renderTabItemEventHandler;
				RenderTabItemEventHandler value2 = (RenderTabItemEventHandler)Delegate.Remove(renderTabItemEventHandler2, value);
				renderTabItemEventHandler = Interlocked.CompareExchange(ref ᓲ, value2, renderTabItemEventHandler2);
			}
			while ((object)renderTabItemEventHandler != renderTabItemEventHandler2);
		}
	}

	public event RenderTabItemEventHandler PostRenderTabItem
	{
		add
		{
			RenderTabItemEventHandler renderTabItemEventHandler = ᓳ;
			RenderTabItemEventHandler renderTabItemEventHandler2;
			do
			{
				renderTabItemEventHandler2 = renderTabItemEventHandler;
				RenderTabItemEventHandler value2 = (RenderTabItemEventHandler)Delegate.Combine(renderTabItemEventHandler2, value);
				renderTabItemEventHandler = Interlocked.CompareExchange(ref ᓳ, value2, renderTabItemEventHandler2);
			}
			while ((object)renderTabItemEventHandler != renderTabItemEventHandler2);
		}
		remove
		{
			RenderTabItemEventHandler renderTabItemEventHandler = ᓳ;
			RenderTabItemEventHandler renderTabItemEventHandler2;
			do
			{
				renderTabItemEventHandler2 = renderTabItemEventHandler;
				RenderTabItemEventHandler value2 = (RenderTabItemEventHandler)Delegate.Remove(renderTabItemEventHandler2, value);
				renderTabItemEventHandler = Interlocked.CompareExchange(ref ᓳ, value2, renderTabItemEventHandler2);
			}
			while ((object)renderTabItemEventHandler != renderTabItemEventHandler2);
		}
	}

	[Description("Occurs when mouse enters a tab.")]
	public event EventHandler TabMouseEnter
	{
		add
		{
			EventHandler eventHandler = ᓴ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᓴ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ᓴ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᓴ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when mouse leaves a tab.")]
	public event EventHandler TabMouseLeave
	{
		add
		{
			EventHandler eventHandler = ᓵ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᓵ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ᓵ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᓵ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when mouse moves over the tab.")]
	public event MouseEventHandler TabMouseMove
	{
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = ᓶ;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref ᓶ, value2, val2);
			}
			while (val != val2);
		}
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = ᓶ;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref ᓶ, value2, val2);
			}
			while (val != val2);
		}
	}

	[Description("Occurs when mouse clicks the tab.")]
	public event MouseEventHandler TabMouseClick
	{
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = ᓷ;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref ᓷ, value2, val2);
			}
			while (val != val2);
		}
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = ᓷ;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref ᓷ, value2, val2);
			}
			while (val != val2);
		}
	}

	[Description("Occurs when mouse hovers over the tab.")]
	public event EventHandler TabMouseHover
	{
		add
		{
			EventHandler eventHandler = ᓸ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᓸ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ᓸ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᓸ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	protected internal virtual void OnTabMouseEnter(TabItem sender, EventArgs e)
	{
		ᓴ?.Invoke(sender, e);
	}

	protected internal virtual void OnTabMouseLeave(TabItem sender, EventArgs e)
	{
		ᓵ?.Invoke(sender, e);
	}

	protected internal virtual void OnTabMouseMove(TabItem sender, MouseEventArgs e)
	{
		MouseEventHandler val = ᓶ;
		if (val != null)
		{
			val.Invoke((object)sender, e);
		}
	}

	protected internal virtual void OnTabMouseClick(TabItem sender, MouseEventArgs e)
	{
		MouseEventHandler val = ᓷ;
		if (val != null)
		{
			val.Invoke((object)sender, e);
		}
	}

	protected internal virtual void OnTabMouseHover(TabItem sender, EventArgs e)
	{
		ᓸ?.Invoke(sender, e);
	}

	public TabStrip()
	{
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		if (!ColorFunctions._0893)
		{
			_089F.ळ();
			_089F.ऴ();
			ColorFunctions.LoadColors();
		}
		ᓺ = new TabsCollection(this);
		((Control)this).SetStyle((ControlStyles)512, true);
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)4, true);
		((Control)this).SetStyle((ControlStyles)2048, true);
		((Control)this).SetStyle(DisplayHelp.DoubleBufferFlag, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		ᔂ = new TabSystemBox(this);
		ᔂ.Back += ᕞ;
		ᔂ.Close += ᕠ;
		ᔂ.Forward += ᕟ;
		լ = new TabColorScheme(Ӷ);
		լ.ColorChanged += ᕜ;
		StyleManager.Register((Control)(object)this);
	}

	protected override AccessibleObject CreateAccessibilityInstance()
	{
		return (AccessibleObject)(object)new TabStripAccessibleObject(this);
	}

	protected override void OnResize(EventArgs e)
	{
		((Control)this).OnResize(e);
		if (((Control)this).Width != 0 && ((Control)this).Height != 0)
		{
			ᔁ = 0;
			RecalcSize();
			EnsureVisible(SelectedTab);
		}
	}

	private Rectangle ᔪ(Rectangle ᔫ)
	{
		ᔫ.Inflate(-1, -1);
		return ᔫ;
	}

	internal void ᔬ(Graphics _٥)
	{
		if (ᓠ && ᔂ.Visible)
		{
			ᔂ.Paint(_٥);
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
		if (ե)
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
		if (Ӷ == eTabStripStyle.SimulatedTheme)
		{
			if (ᓼ)
			{
				_0BE5(e.Graphics, ᔯ(((Control)this).DisplayRectangle, TabLayoutType == eTabLayoutType.MultilineWithNavigationBox, ᔱ: false));
			}
			_2841 _28412 = new _2841();
			_28412.ῷ = ᔝ;
			_28412.Paint(e.Graphics, this);
		}
		else if (Ӷ == eTabStripStyle.Metro)
		{
			ᔭ(e.Graphics);
		}
		else if (ᓨ)
		{
			_0DF4(e.Graphics);
		}
		else if (Ӷ == eTabStripStyle.OneNote || Ӷ == eTabStripStyle.VS2005Document)
		{
			ᔳ(e.Graphics);
		}
		else if (Ӷ == eTabStripStyle.VS2005)
		{
			ᕁ(e.Graphics);
		}
		else if (Ӷ == eTabStripStyle.RoundHeader)
		{
			if (ᓼ)
			{
				_0BE5(e.Graphics, ᔯ(((Control)this).DisplayRectangle, TabLayoutType == eTabLayoutType.MultilineWithNavigationBox, ᔱ: false));
			}
			_2850 _28502 = new _2850();
			_28502.ῷ = ᔝ;
			_28502.Paint(e.Graphics, this);
		}
		else if (Ӷ == eTabStripStyle.Office2007Dock)
		{
			if (ᓼ)
			{
				_0BE5(e.Graphics, ᔯ(((Control)this).DisplayRectangle, TabLayoutType == eTabLayoutType.MultilineWithNavigationBox, ᔱ: false));
			}
			_2851 _28512 = new _2851();
			_28512.ῷ = ᔝ;
			_28512.Paint(e.Graphics, this);
		}
		else if (Ӷ == eTabStripStyle.VS2005Dock)
		{
			if (ᓼ)
			{
				_0BE5(e.Graphics, ᔯ(((Control)this).DisplayRectangle, TabLayoutType == eTabLayoutType.MultilineWithNavigationBox, ᔱ: false));
			}
			_2853 _28532 = new _2853();
			_28532.ῷ = ᔝ;
			_28532.Paint(e.Graphics, this);
		}
		else if (Ӷ == eTabStripStyle.Office2007Document)
		{
			if (ᓼ)
			{
				_0BE5(e.Graphics, ᔯ(((Control)this).DisplayRectangle, TabLayoutType == eTabLayoutType.MultilineWithNavigationBox, ᔱ: false));
			}
			_2857 _28572 = new _2857();
			_28572.ῷ = ᔝ;
			_28572.Paint(e.Graphics, this);
		}
		else
		{
			ᔭ(e.Graphics);
		}
		e.Graphics.SmoothingMode = smoothingMode;
		e.Graphics.TextRenderingHint = textRenderingHint;
	}

	private void ᔭ(Graphics _٥)
	{
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_030e: Expected O, but got Unknown
		//IL_0329: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Expected O, but got Unknown
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Expected O, but got Unknown
		//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ac: Expected O, but got Unknown
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Expected O, but got Unknown
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Expected O, but got Unknown
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Expected O, but got Unknown
		//IL_0406: Unknown result type (might be due to invalid IL or missing references)
		//IL_042c: Expected O, but got Unknown
		//IL_0ac6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0acd: Expected O, but got Unknown
		//IL_0827: Unknown result type (might be due to invalid IL or missing references)
		//IL_082e: Expected O, but got Unknown
		//IL_062b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0632: Expected O, but got Unknown
		//IL_1d7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d84: Expected O, but got Unknown
		//IL_1bff: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c06: Expected O, but got Unknown
		//IL_0b7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b81: Expected O, but got Unknown
		//IL_0b0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b15: Expected O, but got Unknown
		//IL_12e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_12ef: Expected O, but got Unknown
		//IL_10c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_10c8: Expected O, but got Unknown
		//IL_1e2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e31: Expected O, but got Unknown
		//IL_0c7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c81: Expected O, but got Unknown
		//IL_0c04: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c0b: Expected O, but got Unknown
		//IL_1686: Unknown result type (might be due to invalid IL or missing references)
		//IL_168d: Expected O, but got Unknown
		//IL_161d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1624: Expected O, but got Unknown
		//IL_15d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_15db: Expected O, but got Unknown
		//IL_1ece: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ed5: Expected O, but got Unknown
		//IL_09c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09cc: Expected O, but got Unknown
		//IL_0a2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a32: Expected O, but got Unknown
		//IL_076b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0772: Expected O, but got Unknown
		//IL_07cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d4: Expected O, but got Unknown
		//IL_177b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1782: Expected O, but got Unknown
		//IL_170d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1714: Expected O, but got Unknown
		//IL_1f7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f82: Expected O, but got Unknown
		//IL_0910: Unknown result type (might be due to invalid IL or missing references)
		//IL_0917: Expected O, but got Unknown
		//IL_06b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bf: Expected O, but got Unknown
		//IL_1480: Unknown result type (might be due to invalid IL or missing references)
		//IL_1487: Expected O, but got Unknown
		//IL_1222: Unknown result type (might be due to invalid IL or missing references)
		//IL_1229: Expected O, but got Unknown
		//IL_13d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_13d8: Expected O, but got Unknown
		//IL_115c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1163: Expected O, but got Unknown
		//IL_097c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0983: Expected O, but got Unknown
		//IL_0724: Unknown result type (might be due to invalid IL or missing references)
		//IL_072b: Expected O, but got Unknown
		//IL_14e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_14f0: Expected O, but got Unknown
		//IL_1267: Unknown result type (might be due to invalid IL or missing references)
		//IL_126e: Expected O, but got Unknown
		//IL_1439: Unknown result type (might be due to invalid IL or missing references)
		//IL_1440: Expected O, but got Unknown
		//IL_11d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_11e0: Expected O, but got Unknown
		//IL_0fc0: Unknown result type (might be due to invalid IL or missing references)
		//IL_100e: Expected O, but got Unknown
		//IL_102e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1084: Expected O, but got Unknown
		//IL_1ab7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1abe: Expected O, but got Unknown
		//IL_1b10: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b17: Expected O, but got Unknown
		TabColorScheme tabColorScheme = լ;
		if (!tabColorScheme.TabBackground2.IsEmpty && ((Control)this).Height > 0 && ((Control)this).Width > 0)
		{
			LinearGradientBrush val = BarFunctions.ॴ(((Control)this).ClientRectangle, tabColorScheme.TabBackground, tabColorScheme.TabBackground2, tabColorScheme.TabBackgroundGradientAngle);
			try
			{
				_٥.FillRectangle((Brush)(object)val, ((Control)this).ClientRectangle);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		else
		{
			SolidBrush val2 = new SolidBrush(tabColorScheme.TabBackground);
			try
			{
				_٥.FillRectangle((Brush)(object)val2, ((Control)this).DisplayRectangle);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		Color tabBorder = tabColorScheme.TabBorder;
		if (Ӷ != eTabStripStyle.Metro && !tabBorder.IsEmpty)
		{
			Pen val3 = new Pen(tabBorder, 1f);
			try
			{
				_٥.DrawRectangle(val3, new Rectangle(((Control)this).ClientRectangle.X, ((Control)this).ClientRectangle.Y, ((Control)this).ClientRectangle.Width - 1, ((Control)this).ClientRectangle.Height - 1));
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		Rectangle displayRectangle = ((Control)this).DisplayRectangle;
		switch (ᓹ)
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
		int num = ((!tabBorder.IsEmpty) ? 1 : 0);
		if (Ӷ != eTabStripStyle.Metro)
		{
			switch (ᓹ)
			{
			case eTabStripAlignment.Top:
				if (!(((Control)this).Parent is TabControl))
				{
					_٥.FillRectangle((Brush)new SolidBrush(tabColorScheme.TabItemSelectedBackground2.IsEmpty ? tabColorScheme.TabItemSelectedBackground : tabColorScheme.TabItemSelectedBackground2), displayRectangle.X + num, displayRectangle.Bottom, displayRectangle.Width - num * 2, ((Control)this).Height - displayRectangle.Bottom);
				}
				else
				{
					Pen val5 = new Pen(tabColorScheme.TabItemSelectedBorder, 1f);
					try
					{
						_٥.DrawLine(val5, displayRectangle.X, displayRectangle.Bottom, displayRectangle.X, ((Control)this).ClientRectangle.Bottom);
						_٥.DrawLine(val5, displayRectangle.Right - 1, displayRectangle.Bottom, displayRectangle.Right - 1, ((Control)this).ClientRectangle.Bottom);
					}
					finally
					{
						((IDisposable)val5)?.Dispose();
					}
				}
				_٥.DrawLine(new Pen(tabColorScheme.TabItemSelectedBorderLight, 1f), displayRectangle.X + num, displayRectangle.Bottom, displayRectangle.Right - 1 - num, displayRectangle.Bottom);
				displayRectangle.Inflate(-4, 0);
				break;
			case eTabStripAlignment.Left:
				_٥.DrawLine(new Pen(tabColorScheme.TabItemSelectedBorderLight, 1f), displayRectangle.Right - 1, displayRectangle.Y + num, displayRectangle.Right - 1, displayRectangle.Bottom - num);
				displayRectangle.Inflate(0, -4);
				break;
			case eTabStripAlignment.Right:
				_٥.DrawLine(new Pen(tabColorScheme.TabItemSelectedBorderLight, 1f), displayRectangle.X, displayRectangle.Y + num, displayRectangle.X, displayRectangle.Bottom - num);
				displayRectangle.Inflate(0, -4);
				break;
			default:
				if (!(((Control)this).Parent is TabControl))
				{
					_٥.FillRectangle((Brush)new SolidBrush(tabColorScheme.TabItemSelectedBackground), displayRectangle.X, 0, displayRectangle.Width, displayRectangle.Y);
				}
				else
				{
					Pen val4 = new Pen(tabColorScheme.TabItemSelectedBorder, 1f);
					try
					{
						_٥.DrawLine(val4, displayRectangle.X, displayRectangle.Y, displayRectangle.X, 0);
						_٥.DrawLine(val4, displayRectangle.Right - 1, displayRectangle.Y, displayRectangle.Right - 1, 0);
					}
					finally
					{
						((IDisposable)val4)?.Dispose();
					}
				}
				_٥.DrawLine(new Pen(tabColorScheme.TabItemSelectedBorderLight, 1f), displayRectangle.X, displayRectangle.Y, displayRectangle.Right, displayRectangle.Y);
				displayRectangle.Inflate(-4, 0);
				break;
			}
		}
		if (ᓠ && ᔂ.Visible)
		{
			if (ᓹ == eTabStripAlignment.Right || ᓹ == eTabStripAlignment.Left)
			{
				if (ᔂ.DisplayRectangle.Height > 0)
				{
					displayRectangle.Height -= ᔂ.DisplayRectangle.Height;
				}
				else
				{
					displayRectangle.Height -= ᔂ.DefaultWidth;
				}
			}
			else
			{
				if (ᔂ.DisplayRectangle.Width > 0)
				{
					displayRectangle.Width -= ᔂ.DisplayRectangle.Width;
				}
				else
				{
					displayRectangle.Width -= ᔂ.DefaultWidth;
				}
				if (ᓛ)
				{
					displayRectangle.X += ᔂ.DefaultWidth;
				}
			}
		}
		if (ᓼ)
		{
			_0BE5(_٥, ᔯ(((Control)this).DisplayRectangle, ᔰ: false, ᔱ: false));
		}
		if (ᓹ == eTabStripAlignment.Top)
		{
			displayRectangle.Height++;
		}
		Rectangle rectangle = displayRectangle;
		_٥.SetClip(displayRectangle);
		Rectangle rectangle2 = Rectangle.Empty;
		foreach (TabItem item in ᓺ)
		{
			if (!item.Visible || !displayRectangle.IntersectsWith(item.DisplayRectangle))
			{
				continue;
			}
			if (ᓞ)
			{
				RenderTabItemEventArgs renderTabItemEventArgs = new RenderTabItemEventArgs(item, _٥);
				ᕉ(renderTabItemEventArgs);
				if (renderTabItemEventArgs.Cancel)
				{
					continue;
				}
			}
			Rectangle ᔸ = item.DisplayRectangle;
			TabColors tabColors = ᕝ(item);
			if (ᓹ == eTabStripAlignment.Left || ᓹ == eTabStripAlignment.Right)
			{
				if (item == selectedTab)
				{
					rectangle2 = ᔸ;
					if (ᓹ == eTabStripAlignment.Left)
					{
						if (tabColors.BackColor2.IsEmpty)
						{
							SolidBrush val6 = new SolidBrush(_٥.GetNearestColor(tabColors.BackColor));
							try
							{
								_٥.FillRectangle((Brush)(object)val6, ᔸ);
							}
							finally
							{
								((IDisposable)val6)?.Dispose();
							}
						}
						else
						{
							LinearGradientBrush val7 = BarFunctions.ॴ(ᔸ, tabColors.BackColor, tabColors.BackColor2, tabColors.BackColorGradientAngle);
							try
							{
								_٥.FillRectangle((Brush)(object)val7, ᔸ);
							}
							finally
							{
								((IDisposable)val7)?.Dispose();
							}
						}
						if (Ӷ == eTabStripStyle.Metro)
						{
							if (StyleManager.Style == eStyle.Metro || StyleManager.Style == eStyle.OfficeMobile2014 || StyleManager.Style == eStyle.Office2016)
							{
								Pen val8 = new Pen(tabColors.BorderColor, 1f);
								try
								{
									_٥.DrawLine(val8, ᔸ.X, ᔸ.Y, ᔸ.Right - 1, ᔸ.Y);
									_٥.DrawLine(val8, ᔸ.X, ᔸ.Bottom - 1, ᔸ.Right - 1, ᔸ.Bottom - 1);
								}
								finally
								{
									((IDisposable)val8)?.Dispose();
								}
								SolidBrush val9 = new SolidBrush(tabColors.TextColor);
								try
								{
									_٥.FillRectangle((Brush)(object)val9, new Rectangle(ᔸ.X, ᔸ.Y, 2, ᔸ.Height));
								}
								finally
								{
									((IDisposable)val9)?.Dispose();
								}
							}
						}
						else
						{
							Pen val10 = new Pen(tabColors.LightBorderColor, 1f);
							_٥.DrawLine(val10, ᔸ.X, ᔸ.Y, ᔸ.Right, ᔸ.Y);
							_٥.DrawLine(val10, ᔸ.X, ᔸ.Y, ᔸ.X, ᔸ.Bottom);
							val10.Dispose();
							val10 = new Pen(tabColors.BorderColor, 1f);
							_٥.DrawLine(val10, ᔸ.X + 1, ᔸ.Bottom, ᔸ.Right - 2, ᔸ.Bottom);
							val10.Dispose();
						}
					}
					else
					{
						if (tabColors.BackColor2.IsEmpty)
						{
							SolidBrush val11 = new SolidBrush(_٥.GetNearestColor(tabColors.BackColor));
							try
							{
								_٥.FillRectangle((Brush)(object)val11, ᔸ.X, ᔸ.Y, ᔸ.Width, ᔸ.Height + 1);
							}
							finally
							{
								((IDisposable)val11)?.Dispose();
							}
						}
						else
						{
							LinearGradientBrush val12 = BarFunctions.ॴ(new Rectangle(ᔸ.X, ᔸ.Y, ᔸ.Width, ᔸ.Height + 1), tabColors.BackColor, tabColors.BackColor2, tabColors.BackColorGradientAngle);
							try
							{
								_٥.FillRectangle((Brush)(object)val12, ᔸ.X, ᔸ.Y, ᔸ.Width, ᔸ.Height + 1);
							}
							finally
							{
								((IDisposable)val12)?.Dispose();
							}
						}
						if (Ӷ == eTabStripStyle.Metro)
						{
							if (StyleManager.Style == eStyle.Metro || StyleManager.Style == eStyle.OfficeMobile2014 || StyleManager.Style == eStyle.Office2016)
							{
								Pen val13 = new Pen(tabColors.BorderColor, 1f);
								try
								{
									_٥.DrawLine(val13, ᔸ.X, ᔸ.Y, ᔸ.Right - 1, ᔸ.Y);
									_٥.DrawLine(val13, ᔸ.X, ᔸ.Bottom - 1, ᔸ.Right - 1, ᔸ.Bottom - 1);
								}
								finally
								{
									((IDisposable)val13)?.Dispose();
								}
								SolidBrush val14 = new SolidBrush(tabColors.TextColor);
								try
								{
									_٥.FillRectangle((Brush)(object)val14, new Rectangle(ᔸ.Right - 2, ᔸ.Y, 2, ᔸ.Height));
								}
								finally
								{
									((IDisposable)val14)?.Dispose();
								}
							}
						}
						else
						{
							Pen val15 = new Pen(tabColors.LightBorderColor, 1f);
							_٥.DrawLine(val15, ᔸ.X, ᔸ.Y, ᔸ.Right, ᔸ.Y);
							_٥.DrawLine(val15, ᔸ.Right - 1, ᔸ.Y, ᔸ.Right - 1, ᔸ.Bottom);
							val15.Dispose();
							val15 = new Pen(tabColors.BorderColor, 1f);
							_٥.DrawLine(val15, ᔸ.X, ᔸ.Bottom, ᔸ.Right - 3, ᔸ.Bottom);
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
							LinearGradientBrush val16 = BarFunctions.ॴ(ᔸ, tabColors.BackColor, tabColors.BackColor2, tabColors.BackColorGradientAngle);
							try
							{
								_٥.FillRectangle((Brush)(object)val16, ᔸ);
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
								_٥.FillRectangle((Brush)(object)val17, ᔸ);
							}
							finally
							{
								((IDisposable)val17)?.Dispose();
							}
						}
					}
					if (!tabColors.LightBorderColor.IsEmpty)
					{
						if (ᓹ == eTabStripAlignment.Left)
						{
							Pen val18 = new Pen(tabColors.LightBorderColor, 1f);
							try
							{
								_٥.DrawLine(val18, ᔸ.X, ᔸ.Y, ᔸ.Right, ᔸ.Y);
								_٥.DrawLine(val18, ᔸ.X, ᔸ.Y, ᔸ.X, ᔸ.Bottom);
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
								_٥.DrawLine(val19, ᔸ.X, ᔸ.Y, ᔸ.Right, ᔸ.Y);
								_٥.DrawLine(val19, ᔸ.Right - 1, ᔸ.Y, ᔸ.Right - 1, ᔸ.Bottom);
							}
							finally
							{
								((IDisposable)val19)?.Dispose();
							}
						}
					}
					if (!tabColors.BorderColor.IsEmpty)
					{
						if (ᓹ == eTabStripAlignment.Left)
						{
							Pen val20 = new Pen(tabColors.BorderColor, 1f);
							try
							{
								_٥.DrawLine(val20, ᔸ.X + 1, ᔸ.Bottom, ᔸ.Right - 2, ᔸ.Bottom);
								_٥.DrawLine(val20, ᔸ.Right - 1, ᔸ.Bottom, ᔸ.Right - 1, ᔸ.Y + 1);
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
								_٥.DrawLine(val21, ᔸ.X, ᔸ.Y + 1, ᔸ.X, ᔸ.Bottom);
								_٥.DrawLine(val21, ᔸ.X + 1, ᔸ.Bottom, ᔸ.Right - 2, ᔸ.Bottom);
							}
							finally
							{
								((IDisposable)val21)?.Dispose();
							}
						}
					}
				}
				if (ᔝ && item.CloseButtonVisible)
				{
					item.CloseButtonBounds = ᔴ(_٥, ᔵ: true, item.CloseButtonMouseOver, item == ᔎ || item == SelectedTab, ref ᔸ);
				}
				else
				{
					item.CloseButtonBounds = Rectangle.Empty;
				}
				Image image = item.GetImage();
				Icon icon = item.Icon;
				Size size = ((image != null) ? Dpi.ImageSize(image.Size) : Size.Empty);
				if ((image != null && image.Width + 4 <= ᔸ.Width) || (icon != null && item.IconSize.Width + 4 <= ᔸ.Width))
				{
					if (icon != null)
					{
						Rectangle rectangle3 = new Rectangle(ᔸ.X + (ᔸ.Width - item.IconSize.Width) / 2, ᔸ.Y + 4, item.IconSize.Width, item.IconSize.Height);
						if (rectangle.Contains(rectangle3))
						{
							_٥.DrawIcon(icon, rectangle3);
						}
						ᔸ.Y += item.IconSize.Height + 2;
						ᔸ.Height -= item.IconSize.Height + 2;
					}
					else if (image != null)
					{
						_٥.DrawImage(image, ᔸ.X + (ᔸ.Width - size.Width) / 2, ᔸ.Y + 4, size.Width, size.Height);
						ᔸ.Y += size.Height + 2;
						ᔸ.Height -= size.Height + 2;
					}
					ᔸ.Inflate(0, -1);
					ᔸ.Height -= 4;
					ᔸ.Y += 3;
				}
				else
				{
					ᔸ.Y += 2;
					ᔸ.Height -= 2;
				}
				_٥.RotateTransform(90f);
				if (!ᔍ || item == ᓻ)
				{
					Font font = ((Control)this).Font;
					if (item == selectedTab && ᔋ != null)
					{
						font = ᔋ;
					}
					Rectangle rectangle4 = new Rectangle(ᔸ.Top, -ᔸ.Right, ᔸ.Height, ᔸ.Width);
					if (rectangle4.Height > ᔗ)
					{
						TextDrawing.DrawStringLegacy(_٥, item.Text, font, tabColors.TextColor, rectangle4, format);
					}
					if (ᑫ && ((Control)this).Focused && item == ᓻ)
					{
						ControlPaint.DrawFocusRectangle(_٥, ᔪ(rectangle4));
					}
					_٥.ResetTransform();
				}
				if (item != selectedTab)
				{
					_٥.DrawLine(new Pen(tabColorScheme.TabItemSeparator, 1f), item.DisplayRectangle.X + 1, item.DisplayRectangle.Bottom, item.DisplayRectangle.Right - 4, item.DisplayRectangle.Bottom);
					if (!tabColorScheme.TabItemSeparatorShade.IsEmpty)
					{
						_٥.DrawLine(new Pen(tabColorScheme.TabItemSeparatorShade, 1f), item.DisplayRectangle.X + 1 + 1, item.DisplayRectangle.Bottom + 1, item.DisplayRectangle.Right - 4 + 1, item.DisplayRectangle.Bottom + 1);
					}
				}
			}
			else
			{
				if (item == selectedTab)
				{
					rectangle2 = ᔸ;
					if (ᓹ == eTabStripAlignment.Bottom)
					{
						if (tabColors.BackColor2.IsEmpty)
						{
							SolidBrush val22 = new SolidBrush(_٥.GetNearestColor(tabColors.BackColor));
							try
							{
								_٥.FillRectangle((Brush)(object)val22, ᔸ);
							}
							finally
							{
								((IDisposable)val22)?.Dispose();
							}
						}
						else
						{
							LinearGradientBrush val23 = BarFunctions.ॴ(ᔸ, tabColors.BackColor, tabColors.BackColor2, tabColors.BackColorGradientAngle);
							try
							{
								_٥.FillRectangle((Brush)(object)val23, ᔸ);
							}
							finally
							{
								((IDisposable)val23)?.Dispose();
							}
						}
						if (Ӷ == eTabStripStyle.Metro)
						{
							if (StyleManager.Style == eStyle.Metro || StyleManager.Style == eStyle.OfficeMobile2014 || StyleManager.Style == eStyle.Office2016 || լ.ᳺ == ᵋ.ᵌ)
							{
								Pen val24 = new Pen(tabColors.BorderColor, 1f);
								try
								{
									_٥.DrawLine(val24, ᔸ.X, ᔸ.Y, ᔸ.X, ᔸ.Bottom - 1);
									_٥.DrawLine(val24, ᔸ.Right - 1, ᔸ.Y, ᔸ.Right - 1, ᔸ.Bottom - 1);
								}
								finally
								{
									((IDisposable)val24)?.Dispose();
								}
								if (լ.ᳺ != ᵋ.ᵌ)
								{
									SolidBrush val25 = new SolidBrush(tabColors.TextColor);
									try
									{
										_٥.FillRectangle((Brush)(object)val25, new Rectangle(ᔸ.X, ᔸ.Bottom - 2, ᔸ.Width, 2));
									}
									finally
									{
										((IDisposable)val25)?.Dispose();
									}
								}
							}
						}
						else
						{
							Pen val26 = new Pen(tabColors.LightBorderColor, 1f);
							try
							{
								_٥.DrawLine(val26, ᔸ.X, ᔸ.Y, ᔸ.X, ᔸ.Bottom);
							}
							finally
							{
								((IDisposable)val26)?.Dispose();
							}
							Pen val27 = new Pen(tabColors.BorderColor, 1f);
							try
							{
								_٥.DrawLine(val27, ᔸ.X + 1, ᔸ.Bottom, ᔸ.Right, ᔸ.Bottom);
								_٥.DrawLine(val27, ᔸ.Right, ᔸ.Y, ᔸ.Right, ᔸ.Bottom);
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
							SolidBrush val28 = new SolidBrush(_٥.GetNearestColor(tabColors.BackColor));
							try
							{
								_٥.FillRectangle((Brush)(object)val28, ᔸ.X, ᔸ.Y, ᔸ.Width, ᔸ.Height + 1);
							}
							finally
							{
								((IDisposable)val28)?.Dispose();
							}
						}
						else
						{
							LinearGradientBrush val29 = BarFunctions.ॴ(new Rectangle(ᔸ.X, ᔸ.Y, ᔸ.Width, ᔸ.Height + 1), tabColors.BackColor, tabColors.BackColor2, tabColors.BackColorGradientAngle);
							try
							{
								_٥.FillRectangle((Brush)(object)val29, ᔸ.X, ᔸ.Y, ᔸ.Width, ᔸ.Height + 1);
							}
							finally
							{
								((IDisposable)val29)?.Dispose();
							}
						}
						if (Ӷ == eTabStripStyle.Metro)
						{
							if (StyleManager.Style == eStyle.Metro || StyleManager.Style == eStyle.OfficeMobile2014 || StyleManager.Style == eStyle.Office2016)
							{
								Pen val30 = new Pen(tabColors.BorderColor, 1f);
								try
								{
									_٥.DrawLine(val30, ᔸ.X, ᔸ.Y, ᔸ.X, ᔸ.Bottom);
									_٥.DrawLine(val30, ᔸ.Right - 1, ᔸ.Y, ᔸ.Right - 1, ᔸ.Bottom);
								}
								finally
								{
									((IDisposable)val30)?.Dispose();
								}
								SolidBrush val31 = new SolidBrush(tabColors.TextColor);
								try
								{
									_٥.FillRectangle((Brush)(object)val31, new Rectangle(ᔸ.X, ᔸ.Y, ᔸ.Width, 2));
								}
								finally
								{
									((IDisposable)val31)?.Dispose();
								}
							}
						}
						else
						{
							Pen val32 = new Pen(tabColors.LightBorderColor, 1f);
							try
							{
								_٥.DrawLine(val32, ᔸ.X, ᔸ.Y, ᔸ.X, ᔸ.Bottom);
								_٥.DrawLine(val32, ᔸ.X, ᔸ.Y, ᔸ.Right, ᔸ.Y);
							}
							finally
							{
								((IDisposable)val32)?.Dispose();
							}
							Pen val33 = new Pen(tabColors.BorderColor, 1f);
							try
							{
								_٥.DrawLine(val33, ᔸ.Right, ᔸ.Y, ᔸ.Right, ᔸ.Bottom);
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
						Rectangle rectangle5 = ᔸ;
						if (Ӷ != eTabStripStyle.Metro)
						{
							rectangle5.Width--;
							rectangle5.X++;
							rectangle5.Height--;
							rectangle5.Y++;
						}
						if (!tabColors.BackColor2.IsEmpty)
						{
							LinearGradientBrush val34 = BarFunctions.ॴ(ᔸ, tabColors.BackColor, tabColors.BackColor2, tabColors.BackColorGradientAngle);
							try
							{
								_٥.FillRectangle((Brush)(object)val34, rectangle5);
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
								_٥.FillRectangle((Brush)(object)val35, rectangle5);
							}
							finally
							{
								((IDisposable)val35)?.Dispose();
							}
						}
					}
					if (!tabColors.LightBorderColor.IsEmpty)
					{
						if (ᓹ == eTabStripAlignment.Bottom)
						{
							Pen val36 = new Pen(tabColors.LightBorderColor, 1f);
							try
							{
								_٥.DrawLine(val36, ᔸ.X, ᔸ.Y, ᔸ.Right, ᔸ.Y);
								_٥.DrawLine(val36, ᔸ.X, ᔸ.Y, ᔸ.X, ᔸ.Bottom);
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
								_٥.DrawLine(val37, ᔸ.X, ᔸ.Y, ᔸ.X, ᔸ.Bottom);
								_٥.DrawLine(val37, ᔸ.X, ᔸ.Y, ᔸ.Right, ᔸ.Y);
							}
							finally
							{
								((IDisposable)val37)?.Dispose();
							}
						}
					}
					if (!tabColors.BorderColor.IsEmpty)
					{
						if (ᓹ == eTabStripAlignment.Bottom)
						{
							Pen val38 = new Pen(tabColors.BorderColor, 1f);
							try
							{
								_٥.DrawLine(val38, ᔸ.X + 1, ᔸ.Bottom, ᔸ.Right, ᔸ.Bottom);
								_٥.DrawLine(val38, ᔸ.Right, ᔸ.Y, ᔸ.Right, ᔸ.Bottom);
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
								_٥.DrawLine(val39, ᔸ.Right - 1, ᔸ.Y, ᔸ.Right - 1, ᔸ.Bottom);
								_٥.DrawLine(val39, ᔸ.X, ᔸ.Bottom, ᔸ.Right, ᔸ.Bottom);
							}
							finally
							{
								((IDisposable)val39)?.Dispose();
							}
						}
					}
				}
				if (ᓹ == eTabStripAlignment.Top)
				{
					ᔸ.Offset(0, 1);
				}
				if (ᔝ && item.CloseButtonVisible)
				{
					item.CloseButtonBounds = ᔴ(_٥, ᔵ: false, item.CloseButtonMouseOver, item == ᔎ || item == SelectedTab, ref ᔸ);
				}
				else
				{
					item.CloseButtonBounds = Rectangle.Empty;
				}
				Image image2 = item.GetImage();
				Icon icon2 = item.Icon;
				Size size2 = Size.Empty;
				if (image2 != null)
				{
					size2 = Dpi.ImageSize(image2.Size);
				}
				if ((image2 != null && size2.Width + 4 < ᔸ.Width) || (icon2 != null && item.IconSize.Width + 4 < ᔸ.Width))
				{
					if (icon2 != null)
					{
						Rectangle rectangle6 = new Rectangle(ᔸ.X + 4, ᔸ.Y + (ᔸ.Height - item.IconSize.Height) / 2, item.IconSize.Width, item.IconSize.Height);
						if (rectangle.Contains(rectangle6))
						{
							_٥.DrawIcon(icon2, rectangle6);
						}
						ᔸ.X += item.IconSize.Width + 2;
						ᔸ.Width -= item.IconSize.Width + 2;
					}
					else if (image2 != null)
					{
						_٥.DrawImage(image2, ᔸ.X + 4, ᔸ.Y + (ᔸ.Height - size2.Height) / 2, size2.Width, size2.Height);
						ᔸ.X += size2.Width + 2;
						ᔸ.Width -= size2.Width + 2;
					}
					ᔸ.Inflate(0, -1);
					ᔸ.Width -= 4;
					ᔸ.X += 3;
				}
				else
				{
					ᔸ.Inflate(-1, 0);
				}
				if (!ᔍ || item == ᓻ)
				{
					Font font2 = ((Control)this).Font;
					if (item == selectedTab && ᔋ != null)
					{
						font2 = ᔋ;
					}
					if (ᔸ.Width > ᔗ)
					{
						TextDrawing.DrawString(_٥, item.Text, font2, tabColors.TextColor, ᔸ, format);
					}
					if (ᑫ && ((Control)this).Focused && item == ᓻ)
					{
						ControlPaint.DrawFocusRectangle(_٥, ᔪ(ᔸ));
					}
				}
				if (item != selectedTab)
				{
					Rectangle displayRectangle2 = item.DisplayRectangle;
					if (ᓛ)
					{
						displayRectangle2.Width -= 2;
					}
					if (!tabColorScheme.TabItemSeparator.IsEmpty)
					{
						Pen val40 = new Pen(tabColorScheme.TabItemSeparator, 1f);
						try
						{
							_٥.DrawLine(val40, displayRectangle2.Right, displayRectangle2.Y + 2, displayRectangle2.Right, displayRectangle2.Bottom - 4);
						}
						finally
						{
							((IDisposable)val40)?.Dispose();
						}
					}
					if (!tabColorScheme.TabItemSeparatorShade.IsEmpty)
					{
						Pen val41 = new Pen(tabColorScheme.TabItemSeparatorShade, 1f);
						try
						{
							_٥.DrawLine(val41, displayRectangle2.Right + 1, displayRectangle2.Y + 2 + 1, displayRectangle2.Right + 1, displayRectangle2.Bottom - 4 + 1);
						}
						finally
						{
							((IDisposable)val41)?.Dispose();
						}
					}
				}
			}
			if (ᓟ)
			{
				RenderTabItemEventArgs ؿ = new RenderTabItemEventArgs(item, _٥);
				ᕊ(ؿ);
			}
		}
		_٥.ResetClip();
		Color color = tabBorder;
		int num2 = 1;
		if (StyleManager.IsVisualStudio2012(StyleManager.Style))
		{
			num2 = 2;
			color = tabColorScheme.TabItemSelectedBackground;
		}
		if (Ӷ == eTabStripStyle.Metro && !rectangle2.IsEmpty && !color.IsEmpty)
		{
			if (StyleManager.IsVisualStudio2012(StyleManager.Style))
			{
				if (լ.ᳺ != ᵋ.ᵌ)
				{
					Pen val42 = new Pen(color, (float)num2);
					try
					{
						if (ᓹ == eTabStripAlignment.Top)
						{
							_٥.DrawLine(val42, ((Control)this).ClientRectangle.X, ((Control)this).ClientRectangle.Bottom - num2, ((Control)this).ClientRectangle.Right, ((Control)this).ClientRectangle.Bottom - num2);
						}
						else if (ᓹ == eTabStripAlignment.Bottom)
						{
							_٥.DrawLine(val42, ((Control)this).ClientRectangle.X, ((Control)this).ClientRectangle.Y + num2 / 2, ((Control)this).ClientRectangle.Right, ((Control)this).ClientRectangle.Y + num2 / 2);
						}
						else if (ᓹ == eTabStripAlignment.Left)
						{
							_٥.DrawLine(val42, ((Control)this).ClientRectangle.Right - num2, ((Control)this).ClientRectangle.Y, ((Control)this).ClientRectangle.Right - num2, ((Control)this).ClientRectangle.Bottom);
						}
						else if (ᓹ == eTabStripAlignment.Right)
						{
							_٥.DrawLine(val42, ((Control)this).ClientRectangle.X, ((Control)this).ClientRectangle.Y, ((Control)this).ClientRectangle.X, ((Control)this).ClientRectangle.Bottom);
						}
					}
					finally
					{
						((IDisposable)val42)?.Dispose();
					}
				}
			}
			else if (ᓹ == eTabStripAlignment.Top)
			{
				Pen val43 = new Pen(color, (float)num2);
				try
				{
					_٥.DrawLine(val43, ((Control)this).ClientRectangle.X, ((Control)this).ClientRectangle.Bottom - 1, rectangle2.X, ((Control)this).ClientRectangle.Bottom - 1);
					_٥.DrawLine(val43, rectangle2.Right - 1, ((Control)this).ClientRectangle.Bottom - 1, ((Control)this).ClientRectangle.Right - 1, ((Control)this).ClientRectangle.Bottom - 1);
				}
				finally
				{
					((IDisposable)val43)?.Dispose();
				}
			}
			else if (ᓹ == eTabStripAlignment.Bottom)
			{
				Pen val44 = new Pen(color, (float)num2);
				try
				{
					_٥.DrawLine(val44, ((Control)this).ClientRectangle.X, ((Control)this).ClientRectangle.Y, rectangle2.X, ((Control)this).ClientRectangle.Y);
					_٥.DrawLine(val44, rectangle2.Right - 1, ((Control)this).ClientRectangle.Y, ((Control)this).ClientRectangle.Right - 1, ((Control)this).ClientRectangle.Y);
				}
				finally
				{
					((IDisposable)val44)?.Dispose();
				}
			}
			else if (ᓹ == eTabStripAlignment.Left)
			{
				Pen val45 = new Pen(color, (float)num2);
				try
				{
					_٥.DrawLine(val45, ((Control)this).ClientRectangle.Right - 1, ((Control)this).ClientRectangle.Y, ((Control)this).ClientRectangle.Right - 1, rectangle2.Y);
					_٥.DrawLine(val45, ((Control)this).ClientRectangle.Right - 1, rectangle2.Bottom - 1, ((Control)this).ClientRectangle.Right - 1, ((Control)this).ClientRectangle.Bottom - 1);
				}
				finally
				{
					((IDisposable)val45)?.Dispose();
				}
			}
			else if (ᓹ == eTabStripAlignment.Right)
			{
				Pen val46 = new Pen(color, (float)num2);
				try
				{
					_٥.DrawLine(val46, ((Control)this).ClientRectangle.X, ((Control)this).ClientRectangle.Y, ((Control)this).ClientRectangle.X, rectangle2.Y);
					_٥.DrawLine(val46, ((Control)this).ClientRectangle.X, rectangle2.Bottom - 1, ((Control)this).ClientRectangle.X, ((Control)this).ClientRectangle.Bottom - 1);
				}
				finally
				{
					((IDisposable)val46)?.Dispose();
				}
			}
		}
		if (ᓠ && ᔂ.Visible)
		{
			_٥.SetClip(ᔂ.DisplayRectangle);
			ᔂ.Paint(_٥);
			_٥.ResetClip();
		}
	}

	private Rectangle ᔮ(Rectangle ल)
	{
		switch (ᓹ)
		{
		case eTabStripAlignment.Bottom:
			ल.Y += 3;
			ल.Height -= 5;
			break;
		case eTabStripAlignment.Top:
			ल.Y++;
			ल.Height -= 5;
			break;
		case eTabStripAlignment.Left:
			ल.Width -= 5;
			ल.X++;
			break;
		case eTabStripAlignment.Right:
			ल.Width -= 5;
			ल.X += 3;
			break;
		}
		return ल;
	}

	internal Rectangle ᔯ(Rectangle ल, bool ᔰ, bool ᔱ)
	{
		int num = ᕃ();
		if ((Ӷ == eTabStripStyle.OneNote || Ӷ == eTabStripStyle.VS2005Document) && !ᓨ)
		{
			ल = ᔮ(ल);
			switch (ᓹ)
			{
			case eTabStripAlignment.Top:
				ल.Inflate(-4, 0);
				break;
			case eTabStripAlignment.Left:
				ल.Inflate(0, -4);
				break;
			case eTabStripAlignment.Right:
				ल.Inflate(0, -4);
				break;
			default:
				ल.Inflate(-4, 0);
				break;
			}
			if (ᔰ && ᓠ)
			{
				if (ᓹ == eTabStripAlignment.Right || ᓹ == eTabStripAlignment.Left)
				{
					ल.Height -= ᔂ.DefaultWidth + 3;
				}
				else if (ᓛ)
				{
					ल.X += ᔂ.DefaultWidth + 3;
					ल.Width -= ᔂ.DefaultWidth + 3;
				}
				else
				{
					ल.Width -= ᔂ.DefaultWidth + 3;
				}
			}
			if (!ᔱ && (Ӷ == eTabStripStyle.OneNote || Ӷ == eTabStripStyle.VS2005Document) && !ᓨ)
			{
				if (ᓹ == eTabStripAlignment.Right || ᓹ == eTabStripAlignment.Left)
				{
					if (ᓹ == eTabStripAlignment.Right)
					{
						ल.Height -= num;
						ल.Y += num;
					}
					else
					{
						ल.Height -= num;
					}
				}
				else
				{
					ल.Width -= num;
					ल.X += num;
				}
			}
		}
		else
		{
			switch (ᓹ)
			{
			case eTabStripAlignment.Bottom:
				if (ᓨ && ((Control)this).Parent is TabControl)
				{
					ल.Y += 2;
				}
				ल.Height -= 3;
				break;
			case eTabStripAlignment.Top:
				ल.Y++;
				ल.Height -= 3;
				break;
			case eTabStripAlignment.Left:
				if (ᓨ && ((Control)this).Parent is TabControl)
				{
					ल.Width -= 2;
				}
				break;
			case eTabStripAlignment.Right:
				if (ᓨ && ((Control)this).Parent is TabControl)
				{
					ल.X += 2;
					ल.Width -= 2;
				}
				break;
			}
			if ((Ӷ != eTabStripStyle.VS2005 && Ӷ != eTabStripStyle.Office2007Document) || ᓨ)
			{
				switch (ᓹ)
				{
				case eTabStripAlignment.Top:
					ल.Inflate(-4, 0);
					break;
				case eTabStripAlignment.Left:
					ल.Inflate(0, -4);
					break;
				case eTabStripAlignment.Right:
					ल.Inflate(0, -4);
					break;
				default:
					ल.Inflate(-4, 0);
					break;
				}
			}
			if (ᓠ && ᔰ)
			{
				if (ᓹ == eTabStripAlignment.Right || ᓹ == eTabStripAlignment.Left)
				{
					ल.Height -= ᔂ.DefaultWidth;
				}
				else
				{
					ल.Width -= ᔂ.DefaultWidth;
				}
			}
		}
		return ल;
	}

	internal void ᔲ(Graphics _٥)
	{
		Rectangle systemBoxRectangle = GetSystemBoxRectangle();
		if (!systemBoxRectangle.IsEmpty)
		{
			_٥.SetClip(systemBoxRectangle, (CombineMode)4);
		}
	}

	public Rectangle GetSystemBoxRectangle()
	{
		if (!ᔂ.Visible || !ᓠ)
		{
			return Rectangle.Empty;
		}
		Rectangle result = Rectangle.Empty;
		result = ((ᓹ == eTabStripAlignment.Right || ᓹ == eTabStripAlignment.Left) ? new Rectangle(0, ᔂ.DisplayRectangle.Y, ((Control)this).ClientRectangle.Width, ((Control)this).ClientRectangle.Bottom - ᔂ.DisplayRectangle.Y) : ((!ᓛ) ? new Rectangle(ᔂ.DisplayRectangle.X, 0, ((Control)this).ClientRectangle.Right - ᔂ.DisplayRectangle.X, ((Control)this).ClientRectangle.Height) : ᔂ.DisplayRectangle));
		if (ᓹ == eTabStripAlignment.Bottom)
		{
			result.Y++;
		}
		else if (ᓹ == eTabStripAlignment.Top)
		{
			result.Height -= 2;
		}
		return result;
	}

	private void ᔳ(Graphics _٥)
	{
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Expected O, but got Unknown
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Expected O, but got Unknown
		//IL_0485: Unknown result type (might be due to invalid IL or missing references)
		//IL_048c: Expected O, but got Unknown
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Expected O, but got Unknown
		//IL_0554: Unknown result type (might be due to invalid IL or missing references)
		//IL_055b: Expected O, but got Unknown
		//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dd: Expected O, but got Unknown
		//IL_04c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d0: Expected O, but got Unknown
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Expected O, but got Unknown
		//IL_0598: Unknown result type (might be due to invalid IL or missing references)
		//IL_059f: Expected O, but got Unknown
		//IL_08dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e3: Expected O, but got Unknown
		//IL_12b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_12bc: Expected O, but got Unknown
		//IL_0974: Unknown result type (might be due to invalid IL or missing references)
		//IL_097b: Expected O, but got Unknown
		//IL_1349: Unknown result type (might be due to invalid IL or missing references)
		//IL_1350: Expected O, but got Unknown
		//IL_0afa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b01: Expected O, but got Unknown
		//IL_09d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09dc: Expected O, but got Unknown
		//IL_0a0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a12: Expected O, but got Unknown
		//IL_14d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_14d8: Expected O, but got Unknown
		//IL_13aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_13b1: Expected O, but got Unknown
		//IL_13e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_13e8: Expected O, but got Unknown
		//IL_0c00: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c07: Expected O, but got Unknown
		//IL_163a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1641: Expected O, but got Unknown
		//IL_16d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_16dc: Expected O, but got Unknown
		//IL_0ca7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cae: Expected O, but got Unknown
		//IL_104b: Unknown result type (might be due to invalid IL or missing references)
		//IL_10a1: Expected O, but got Unknown
		//IL_10be: Unknown result type (might be due to invalid IL or missing references)
		//IL_1126: Expected O, but got Unknown
		Rectangle rectangle = ᔯ(((Control)this).DisplayRectangle, ᔰ: false, ᔱ: false);
		if (ᓼ)
		{
			_0BE5(_٥, ᔯ(((Control)this).DisplayRectangle, ᔰ: false, ᔱ: false));
		}
		if (!լ.TabBackground2.IsEmpty && ((Control)this).Height > 0 && ((Control)this).Width > 0)
		{
			int num = լ.TabBackgroundGradientAngle;
			if (ᓹ == eTabStripAlignment.Bottom)
			{
				num -= 180;
			}
			else if (ᓹ == eTabStripAlignment.Left)
			{
				num -= 90;
			}
			else if (ᓹ == eTabStripAlignment.Right)
			{
				num += 90;
			}
			LinearGradientBrush val = BarFunctions.ॴ(((Control)this).DisplayRectangle, լ.TabBackground, լ.TabBackground2, num);
			try
			{
				_٥.FillRectangle((Brush)(object)val, ((Control)this).DisplayRectangle);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		else
		{
			SolidBrush val2 = new SolidBrush(լ.TabBackground);
			try
			{
				_٥.FillRectangle((Brush)(object)val2, ((Control)this).DisplayRectangle);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		if (!լ.TabBorder.IsEmpty)
		{
			Pen val3 = new Pen(լ.TabBorder, (float)Dpi.Width1);
			try
			{
				_٥.DrawRectangle(val3, ((Control)this).DisplayRectangle);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		TabItem selectedTab = SelectedTab;
		eTextFormat format = eTextFormat.EndEllipsis | eTextFormat.HorizontalCenter | eTextFormat.SingleLine | eTextFormat.VerticalCenter;
		Rectangle rectangle2 = ᔮ(((Control)this).DisplayRectangle);
		Color color = (լ.TabItemSelectedBackground2.IsEmpty ? լ.TabItemSelectedBackground : լ.TabItemSelectedBackground2);
		if (selectedTab != null && selectedTab.PredefinedColor != 0 && !selectedTab.BackColor2.IsEmpty)
		{
			color = selectedTab.BackColor2;
		}
		switch (ᓹ)
		{
		case eTabStripAlignment.Top:
		{
			SolidBrush val10 = new SolidBrush(color);
			try
			{
				_٥.FillRectangle((Brush)(object)val10, rectangle2.X, rectangle2.Bottom, rectangle2.Width, ((Control)this).Height - rectangle.Bottom);
			}
			finally
			{
				((IDisposable)val10)?.Dispose();
			}
			if (selectedTab == null)
			{
				break;
			}
			Pen val11 = new Pen(լ.TabItemSelectedBorder, (float)Dpi.Width1);
			try
			{
				Rectangle displayRectangle = selectedTab.DisplayRectangle;
				displayRectangle.Width += displayRectangle.Height;
				displayRectangle.X -= displayRectangle.Height - 1;
				if (ᓠ && displayRectangle.Right > ᔂ.DisplayRectangle.X)
				{
					displayRectangle.Width -= displayRectangle.Right - ᔂ.DisplayRectangle.X + 2;
				}
				_٥.DrawLine(val11, rectangle2.X, rectangle2.Bottom, displayRectangle.X - 1, rectangle2.Bottom);
				_٥.DrawLine(val11, displayRectangle.Right + 1, rectangle2.Bottom, rectangle2.Right, rectangle2.Bottom);
				if (((Control)this).Parent is TabControl)
				{
					_٥.DrawLine(val11, rectangle2.X, rectangle2.Bottom, rectangle2.X, ((Control)this).ClientRectangle.Bottom);
					_٥.DrawLine(val11, rectangle2.Right - 1, rectangle2.Bottom, rectangle2.Right - 1, ((Control)this).ClientRectangle.Bottom);
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
				_٥.FillRectangle((Brush)(object)val6, rectangle2.Right, rectangle2.Y, ((Control)this).Width - rectangle2.Width, ((Control)this).Height);
			}
			finally
			{
				((IDisposable)val6)?.Dispose();
			}
			Pen val7 = new Pen(լ.TabItemSelectedBorder, (float)Dpi.Width1);
			try
			{
				_٥.DrawLine(val7, rectangle2.Right - 1, rectangle2.Y, rectangle2.Right - 1, rectangle2.Bottom);
				if (((Control)this).Parent is TabControl)
				{
					_٥.DrawLine(val7, rectangle2.Right - 1, rectangle2.Y, ((Control)this).ClientRectangle.Right, rectangle2.Y);
					_٥.DrawLine(val7, rectangle2.Right - 1, rectangle2.Bottom - 1, ((Control)this).ClientRectangle.Right, rectangle2.Bottom - 1);
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
				_٥.FillRectangle((Brush)(object)val8, 0, 0, ((Control)this).Width - rectangle2.Width, ((Control)this).Height);
			}
			finally
			{
				((IDisposable)val8)?.Dispose();
			}
			Pen val9 = new Pen(լ.TabItemSelectedBorder, (float)Dpi.Width1);
			try
			{
				_٥.DrawLine(val9, rectangle2.X, rectangle2.Y, rectangle2.X, rectangle2.Bottom);
				if (((Control)this).Parent is TabControl)
				{
					_٥.DrawLine(val9, rectangle2.X, rectangle2.Y, 0, rectangle2.Y);
					_٥.DrawLine(val9, rectangle2.X, rectangle2.Bottom - 1, 0, rectangle2.Bottom - 1);
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
				_٥.FillRectangle((Brush)(object)val4, rectangle2.X, 0, rectangle2.Width, rectangle2.Y);
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
			Pen val5 = new Pen(լ.TabItemSelectedBorder, (float)Dpi.Width1);
			try
			{
				_٥.DrawLine(val5, rectangle2.X, rectangle2.Y, rectangle2.Right, rectangle2.Y);
				if (((Control)this).Parent is TabControl)
				{
					_٥.DrawLine(val5, rectangle2.X, rectangle2.Y, rectangle2.X, 0);
					_٥.DrawLine(val5, rectangle2.Right - 1, rectangle2.Y, rectangle2.Right - 1, 0);
				}
			}
			finally
			{
				((IDisposable)val5)?.Dispose();
			}
			break;
		}
		}
		rectangle = ᔯ(((Control)this).DisplayRectangle, ᔂ.Visible, ᔱ: true);
		Rectangle rectangle3 = rectangle;
		_٥.SetClip(rectangle);
		bool flag = true;
		bool flag2 = ᓺ.Count > 0;
		int num2 = 0;
		bool flag3 = false;
		if (ᓹ == eTabStripAlignment.Left || (ᓛ && (ᓹ == eTabStripAlignment.Top || ᓹ == eTabStripAlignment.Bottom)))
		{
			num2 = ᓺ.Count - 1;
			flag3 = true;
		}
		int num3 = -1;
		while (flag2)
		{
			TabItem tabItem = ᓺ[num2];
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
				if (num2 >= ᓺ.Count)
				{
					flag2 = false;
				}
			}
			if (!tabItem.Visible || !rectangle.IntersectsWith(tabItem.DisplayRectangle))
			{
				continue;
			}
			if (ᓞ)
			{
				RenderTabItemEventArgs renderTabItemEventArgs = new RenderTabItemEventArgs(tabItem, _٥);
				ᕉ(renderTabItemEventArgs);
				if (renderTabItemEventArgs.Cancel)
				{
					continue;
				}
			}
			TabColors tabColors = ᕝ(tabItem);
			Rectangle ᔸ = tabItem.DisplayRectangle;
			if (ᓹ == eTabStripAlignment.Right)
			{
				ᔸ.Height--;
			}
			if (ᓹ == eTabStripAlignment.Left || ᓹ == eTabStripAlignment.Right)
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
				GraphicsPath val12 = ᔹ(ᔸ, 2, ᓹ, ᔻ: true);
				Region clip = _٥.Clip;
				if (flag || tabItem == ᓻ)
				{
					RectangleF bounds = val12.GetBounds();
					bounds.Width += 1f;
					if (ᓹ == eTabStripAlignment.Left)
					{
						if (flag && tabItem != ᓻ)
						{
							bounds.Width -= 1f;
						}
					}
					else
					{
						bounds.Height += 1f;
						if (flag && tabItem != ᓻ)
						{
							bounds.X += 1f;
						}
					}
					_٥.SetClip(bounds);
				}
				else if (ᓹ == eTabStripAlignment.Right)
				{
					_٥.SetClip(new Rectangle(ᔸ.X + Dpi.Width1, ᔸ.Y + Dpi.Height1, ᔸ.Width + Dpi.Width1, ᔸ.Height + Dpi.Height1));
				}
				else
				{
					_٥.SetClip(new Rectangle(ᔸ.X, ᔸ.Y, ᔸ.Width - Dpi.Width1, ᔸ.Height));
				}
				ᔲ(_٥);
				if (tabColors.BackColor2.IsEmpty)
				{
					SolidBrush val13 = new SolidBrush(tabColors.BackColor);
					try
					{
						_٥.FillPath((Brush)(object)val13, val12);
					}
					finally
					{
						((IDisposable)val13)?.Dispose();
					}
				}
				else
				{
					LinearGradientBrush val14 = BarFunctions.ॴ(val12.GetBounds(), tabColors.BackColor, tabColors.BackColor2, (ᓹ == eTabStripAlignment.Left) ? (tabColors.BackColorGradientAngle - 90) : (tabColors.BackColorGradientAngle + 90));
					try
					{
						_٥.FillPath((Brush)(object)val14, val12);
					}
					finally
					{
						((IDisposable)val14)?.Dispose();
					}
				}
				val12.Dispose();
				val12 = ᔹ(ᔸ, 2, ᓹ, ᔻ: false);
				Pen val15 = new Pen(tabColors.BorderColor, (float)Dpi.Width1);
				try
				{
					_٥.DrawPath(val15, val12);
				}
				finally
				{
					((IDisposable)val15)?.Dispose();
				}
				if (ᔂ.Visible && ᓠ && val12.GetBounds().IntersectsWith(ᔂ.DisplayRectangle))
				{
					Region val16 = new Region(val12);
					val16.Intersect(ᔂ.DisplayRectangle);
					RectangleF bounds2 = val16.GetBounds(_٥);
					val16.Dispose();
					Pen val17 = new Pen(tabColors.BorderColor, 1f);
					try
					{
						val17.DashPattern = new float[2] { 2f, 2f };
						_٥.DrawLine(val17, bounds2.X, bounds2.Y - (float)Dpi.Width1, bounds2.Right, bounds2.Y - (float)Dpi.Height1);
						_٥.DrawLine(val17, bounds2.X + (float)Dpi.Width2, bounds2.Y - (float)Dpi.Height2, bounds2.Right, bounds2.Y - (float)Dpi.Height2);
					}
					finally
					{
						((IDisposable)val17)?.Dispose();
					}
				}
				else if (val12.GetBounds().Y < -1f && val12.GetBounds().Bottom > 0f)
				{
					RectangleF bounds3 = val12.GetBounds();
					Pen val18 = new Pen(tabColors.BorderColor, 1f);
					try
					{
						val18.DashPattern = new float[2] { 2f, 2f };
						_٥.DrawLine(val18, bounds3.X, 0f, bounds3.Right, 0f);
						_٥.DrawLine(val18, bounds3.X + (float)Dpi.Width2, (float)Dpi.Height1, bounds3.Right, (float)Dpi.Height1);
					}
					finally
					{
						((IDisposable)val18)?.Dispose();
					}
				}
				Rectangle ल = ᔸ;
				if (ᓹ == eTabStripAlignment.Right)
				{
					ल.Offset(-Dpi.Width1, 0);
					ल.Height -= Dpi.Width1;
				}
				else
				{
					ल.Offset(Dpi.Width1, 0);
					ल.Height -= Dpi.Width1;
					ल.Y++;
				}
				val12.Dispose();
				val12 = ᔹ(ल, 1, ᓹ, ᔻ: false);
				Pen val19 = new Pen(tabColors.LightBorderColor, (float)Dpi.Width1);
				try
				{
					_٥.DrawPath(val19, val12);
				}
				finally
				{
					((IDisposable)val19)?.Dispose();
				}
				if (!tabColors.DarkBorderColor.IsEmpty)
				{
					RectangleF bounds4 = val12.GetBounds();
					if (ᓹ == eTabStripAlignment.Right)
					{
						bounds4.Y = bounds4.Bottom - (float)Dpi.Height1;
						bounds4.Height = Dpi.Height2;
					}
					else
					{
						bounds4.Height = Dpi.Height2;
						bounds4.Width -= Dpi.Width2;
					}
					_٥.SetClip(bounds4);
					ᔲ(_٥);
					Pen val20 = new Pen(tabColors.DarkBorderColor, (float)Dpi.Width1);
					try
					{
						_٥.DrawPath(val20, val12);
					}
					finally
					{
						((IDisposable)val20)?.Dispose();
					}
				}
				val12.Dispose();
				val12 = null;
				_٥.SetClip(clip, (CombineMode)0);
				if (clip != null)
				{
					clip.Dispose();
					clip = null;
				}
				if (ᔝ && tabItem.CloseButtonVisible)
				{
					ᔸ.Y += Dpi.Height3;
					ᔸ.Height -= Dpi.Height3;
					tabItem.CloseButtonBounds = ᔴ(_٥, ᔵ: true, tabItem.CloseButtonMouseOver, tabItem == ᔎ || tabItem == SelectedTab, ref ᔸ);
				}
				else
				{
					tabItem.CloseButtonBounds = Rectangle.Empty;
				}
				Image image = tabItem.GetImage();
				Icon icon = tabItem.Icon;
				if (icon != null)
				{
					Rectangle rectangle4 = new Rectangle(ᔸ.X + (ᔸ.Width - tabItem.IconSize.Width) / 2, ᔸ.Y + Dpi.Height6, tabItem.IconSize.Width, tabItem.IconSize.Height);
					if (rectangle3.Contains(rectangle4))
					{
						_٥.DrawIcon(icon, rectangle4);
					}
					ᔸ.Y += tabItem.IconSize.Height + Dpi.Height2;
					ᔸ.Height -= tabItem.IconSize.Height + Dpi.Height2;
				}
				else if (image != null)
				{
					Size size = Dpi.ImageSize(image.Size);
					_٥.DrawImage(image, ᔸ.X + (ᔸ.Width - size.Width) / 2, ᔸ.Y + Dpi.Height6, size.Width, size.Height);
					ᔸ.Y += size.Height + Dpi.Height2;
					ᔸ.Height -= size.Height + Dpi.Height2;
				}
				ᔸ.Inflate(0, -Dpi.Height1);
				ᔸ.Height -= Dpi.Height4;
				if (Ӷ == eTabStripStyle.OneNote)
				{
					ᔸ.Y += Dpi.Height3;
				}
				_٥.RotateTransform(90f);
				if (!ᔍ || tabItem == ᓻ)
				{
					Font font = ((Control)this).Font;
					if (tabItem == selectedTab && ᔋ != null)
					{
						font = ᔋ;
					}
					if (tabItem != selectedTab)
					{
						Rectangle rectangle5 = new Rectangle(ᔸ.Top, -ᔸ.Right, ᔸ.Height, ᔸ.Width);
						if (rectangle5.Height > ᔗ)
						{
							TextDrawing.DrawStringLegacy(_٥, tabItem.Text, font, tabColors.TextColor, rectangle5, format);
						}
						if (ᑫ && ((Control)this).Focused && tabItem == ᓻ)
						{
							ControlPaint.DrawFocusRectangle(_٥, ᔪ(rectangle5));
						}
					}
					else
					{
						Rectangle rectangle6 = new Rectangle(ᔸ.Top, -ᔸ.Right, ᔸ.Height, ᔸ.Width);
						if (rectangle6.Height > ᔗ)
						{
							TextDrawing.DrawStringLegacy(_٥, tabItem.Text, font, tabColors.TextColor, rectangle6, format);
						}
						if (ᑫ && ((Control)this).Focused && tabItem == ᓻ)
						{
							ControlPaint.DrawFocusRectangle(_٥, ᔪ(rectangle6));
						}
					}
					_٥.ResetTransform();
				}
				if (tabItem != selectedTab)
				{
					_٥.DrawLine(new Pen(ᔑ, (float)Dpi.Width1), tabItem.DisplayRectangle.X + Dpi.Width1, tabItem.DisplayRectangle.Bottom, tabItem.DisplayRectangle.Right - Dpi.Width4, tabItem.DisplayRectangle.Bottom);
					if (!ᔒ.IsEmpty)
					{
						_٥.DrawLine(new Pen(ᔒ, (float)Dpi.Width1), tabItem.DisplayRectangle.X + Dpi.Width2, tabItem.DisplayRectangle.Bottom + Dpi.Height1, tabItem.DisplayRectangle.Right - Dpi.Width4 + Dpi.Width1, tabItem.DisplayRectangle.Bottom + Dpi.Height1);
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
				GraphicsPath val21 = ᔹ(ᔸ, 2, ᓹ, ᔻ: true);
				Region clip2 = _٥.Clip;
				if (flag || tabItem == ᓻ)
				{
					Rectangle rectangle7 = ᔸ;
					rectangle7.Width += rectangle7.Height;
					rectangle7.X -= rectangle7.Height;
					rectangle7.Width++;
					rectangle7.Height++;
					if (ᓹ == eTabStripAlignment.Bottom && flag && tabItem != ᓻ)
					{
						rectangle7.Y++;
					}
					_٥.SetClip(rectangle7, (CombineMode)0);
				}
				else if (ᓹ == eTabStripAlignment.Top)
				{
					_٥.SetClip(new Rectangle(ᔸ.X + Dpi.Width1, ᔸ.Y, ᔸ.Width + Dpi.Width1, ᔸ.Height), (CombineMode)0);
				}
				else
				{
					_٥.SetClip(new Rectangle(ᔸ.X + Dpi.Width1, ᔸ.Y + Dpi.Height1, ᔸ.Width + Dpi.Width1, ᔸ.Height + Dpi.Height1), (CombineMode)0);
				}
				ᔲ(_٥);
				if (tabColors.BackColor2.IsEmpty)
				{
					SolidBrush val22 = new SolidBrush(tabColors.BackColor);
					try
					{
						_٥.FillPath((Brush)(object)val22, val21);
					}
					finally
					{
						((IDisposable)val22)?.Dispose();
					}
				}
				else
				{
					LinearGradientBrush val23 = BarFunctions.ॴ(val21.GetBounds(), tabColors.BackColor, tabColors.BackColor2, (ᓹ == eTabStripAlignment.Top) ? tabColors.BackColorGradientAngle : (-tabColors.BackColorGradientAngle));
					try
					{
						_٥.FillPath((Brush)(object)val23, val21);
					}
					finally
					{
						((IDisposable)val23)?.Dispose();
					}
				}
				val21.Dispose();
				val21 = ᔹ(ᔸ, 2, ᓹ, ᔻ: false);
				Pen val24 = new Pen(tabColors.BorderColor, (float)Dpi.Width1);
				try
				{
					_٥.DrawPath(val24, val21);
				}
				finally
				{
					((IDisposable)val24)?.Dispose();
				}
				if (ᔂ.Visible && ᓠ && val21.GetBounds().IntersectsWith(ᔂ.DisplayRectangle))
				{
					Region val25 = new Region(val21);
					val25.Intersect(ᔂ.DisplayRectangle);
					RectangleF bounds5 = val25.GetBounds(_٥);
					val25.Dispose();
					Pen val26 = new Pen(tabColors.BorderColor, (float)Dpi.Width1);
					try
					{
						val26.DashPattern = new float[2] { 2f, 2f };
						_٥.DrawLine(val26, bounds5.X - (float)Dpi.Width1, bounds5.Y, bounds5.X - (float)Dpi.Width1, bounds5.Bottom);
						_٥.DrawLine(val26, bounds5.X - (float)Dpi.Width2, bounds5.Y + (float)Dpi.Height2, bounds5.X - (float)Dpi.Width2, bounds5.Bottom);
					}
					finally
					{
						((IDisposable)val26)?.Dispose();
					}
				}
				else if (val21.GetBounds().X < -1f && val21.GetBounds().Right > 0f)
				{
					RectangleF bounds6 = val21.GetBounds();
					Pen val27 = new Pen(tabColors.BorderColor, (float)Dpi.Width1);
					try
					{
						val27.DashPattern = new float[2] { 2f, 2f };
						_٥.DrawLine(val27, 0f, bounds6.Y, 0f, bounds6.Bottom);
						_٥.DrawLine(val27, (float)Dpi.Width1, bounds6.Y + (float)Dpi.Height2, (float)Dpi.Width1, bounds6.Bottom);
					}
					finally
					{
						((IDisposable)val27)?.Dispose();
					}
				}
				Rectangle ल2 = ᔸ;
				if (ᓹ == eTabStripAlignment.Top)
				{
					ल2.Offset(Dpi.Width1, Dpi.Height1);
					ल2.X -= Dpi.Width1;
					ल2.Width -= Dpi.Width1;
				}
				else
				{
					ल2.Offset(Dpi.Width1, -Dpi.Height1);
					ल2.X -= Dpi.Width1;
					ल2.Width -= Dpi.Width1;
				}
				val21.Dispose();
				val21 = ᔹ(ल2, 1, ᓹ, ᔻ: false);
				if (!tabColors.LightBorderColor.IsEmpty)
				{
					RectangleF bounds7 = val21.GetBounds();
					bounds7.Height -= 1f;
					Region val28 = _٥.Clip.Clone();
					_٥.SetClip(bounds7, (CombineMode)1);
					Pen val29 = new Pen(tabColors.LightBorderColor, (float)Dpi.Width1);
					try
					{
						_٥.DrawPath(val29, val21);
					}
					finally
					{
						((IDisposable)val29)?.Dispose();
					}
					_٥.SetClip(val28, (CombineMode)0);
				}
				if (!tabColors.DarkBorderColor.IsEmpty)
				{
					RectangleF bounds8 = val21.GetBounds();
					bounds8.X = bounds8.Right - (float)Dpi.Width1;
					bounds8.Width = Dpi.Width2;
					bounds8.Height -= Dpi.Height1;
					_٥.SetClip(clip2, (CombineMode)0);
					_٥.SetClip(bounds8, (CombineMode)1);
					Pen val30 = new Pen(tabColors.DarkBorderColor, (float)Dpi.Width1);
					try
					{
						_٥.DrawPath(val30, val21);
					}
					finally
					{
						((IDisposable)val30)?.Dispose();
					}
				}
				val21.Dispose();
				val21 = null;
				_٥.SetClip(clip2, (CombineMode)0);
				if (clip2 != null)
				{
					clip2.Dispose();
					clip2 = null;
				}
				ᔸ.Offset(0, Dpi.Width1);
				if (ᔝ && tabItem.CloseButtonVisible)
				{
					tabItem.CloseButtonBounds = ᔴ(_٥, ᔵ: false, tabItem.CloseButtonMouseOver, tabItem == ᔎ || tabItem == SelectedTab, ref ᔸ);
				}
				else
				{
					tabItem.CloseButtonBounds = Rectangle.Empty;
				}
				Image image2 = tabItem.GetImage();
				Icon icon2 = tabItem.Icon;
				if (icon2 != null)
				{
					Rectangle rectangle8 = new Rectangle(ᔸ.X + ᔸ.Height / 3 + ((ᓹ == eTabStripAlignment.Top) ? (-Dpi.Width2) : 0), ᔸ.Y + (ᔸ.Height - tabItem.IconSize.Height) / 2, tabItem.IconSize.Width, tabItem.IconSize.Height);
					if (rectangle3.Contains(rectangle8))
					{
						_٥.DrawIcon(icon2, rectangle8);
					}
					ᔸ.X += tabItem.IconSize.Width + Dpi.Width2;
					ᔸ.Width -= tabItem.IconSize.Width + Dpi.Width2;
				}
				else if (image2 != null)
				{
					Size size2 = Dpi.ImageSize(image2.Size);
					_٥.DrawImage(image2, ᔸ.X + ᔸ.Height / 3 + ((ᓹ == eTabStripAlignment.Top) ? (-Dpi.Width2) : 0), ᔸ.Y + (ᔸ.Height - size2.Height) / 2, size2.Width, size2.Height);
					ᔸ.X += size2.Width + Dpi.Width2;
					ᔸ.Width -= size2.Width + Dpi.Width2;
				}
				ᔸ.Inflate(0, -Dpi.Height1);
				ᔸ.Width -= Dpi.Width4;
				if (Ӷ == eTabStripStyle.OneNote)
				{
					ᔸ.X += Dpi.Width3;
				}
				if (!ᔍ || tabItem == ᓻ)
				{
					Font font2 = ((Control)this).Font;
					if (tabItem == selectedTab && ᔋ != null)
					{
						font2 = ᔋ;
					}
					if (tabItem != selectedTab)
					{
						if (ᔸ.Width > ᔗ)
						{
							TextDrawing.DrawString(_٥, tabItem.Text, font2, tabColors.TextColor, ᔸ, format);
						}
						if (ᑫ && ((Control)this).Focused && tabItem == ᓻ)
						{
							ControlPaint.DrawFocusRectangle(_٥, ᔪ(ᔸ));
						}
					}
					else
					{
						if (ᔸ.Width > ᔗ)
						{
							TextDrawing.DrawString(_٥, tabItem.Text, font2, tabColors.TextColor, ᔸ, format);
						}
						if (ᑫ && ((Control)this).Focused && tabItem == ᓻ)
						{
							ControlPaint.DrawFocusRectangle(_٥, ᔪ(ᔸ));
						}
					}
				}
			}
			if (ᓟ)
			{
				RenderTabItemEventArgs ؿ = new RenderTabItemEventArgs(tabItem, _٥);
				ᕊ(ؿ);
			}
			flag = false;
		}
		_٥.ResetClip();
		if (ᓠ && ᔂ.Visible)
		{
			ᔂ.Paint(_٥);
		}
	}

	private Rectangle ᔴ(Graphics _٥, bool ᔵ, bool ᔶ, bool ᔷ, ref Rectangle ᔸ)
	{
		Size size = Dpi.Size(ᔠ);
		bool flag = ᔞ == eTabCloseButtonPosition.Left;
		int num = (flag ? Dpi.Width2 : Dpi.Width4);
		Rectangle empty = Rectangle.Empty;
		empty = (flag ? ((!ᔵ) ? new Rectangle(ᔸ.X + num, ᔸ.Y + (ᔸ.Height - size.Height) / 2, size.Width, size.Height) : new Rectangle(ᔸ.X + (ᔸ.Width - size.Width) / 2, ᔸ.Y + num, size.Width, size.Height)) : ((!ᔵ) ? new Rectangle(ᔸ.Right - num - size.Width, ᔸ.Y + (ᔸ.Height - size.Height) / 2, size.Width, size.Height) : new Rectangle(ᔸ.X + (ᔸ.Width - size.Width) / 2, ᔸ.Bottom - num - size.Height, size.Width, size.Height)));
		if (ᔷ || ᔣ)
		{
			_2848.ᔴ(_٥, empty, ᔶ, ᔷ, this);
		}
		if (ᔵ)
		{
			if (flag)
			{
				ᔸ.Y += empty.Height + num;
			}
			ᔸ.Height -= empty.Height + num;
		}
		else
		{
			if (flag)
			{
				ᔸ.X += empty.Width + num;
			}
			ᔸ.Width -= empty.Width + num;
		}
		return empty;
	}

	private GraphicsPath ᔹ(Rectangle ल, int ᔺ, eTabStripAlignment _0736, bool ᔻ)
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Expected O, but got Unknown
		//IL_03db: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e1: Expected O, but got Unknown
		//IL_042d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0434: Expected O, but got Unknown
		Rectangle rectangle = ल;
		switch (_0736)
		{
		case eTabStripAlignment.Left:
			rectangle = new Rectangle(ल.X, ल.Y, ल.Height, ल.Width);
			break;
		case eTabStripAlignment.Right:
			rectangle = new Rectangle(ल.Right - ल.Height, ल.Y, ल.Height, ल.Width);
			break;
		}
		GraphicsPath val = new GraphicsPath();
		Point[] array = new Point[4];
		array[0].X = rectangle.X + 2 - (rectangle.Height + Dpi.Width1);
		array[0].Y = rectangle.Bottom - 1;
		array[1].X = array[0].X + Dpi.Width3;
		array[1].Y = array[0].Y - Dpi.Height2;
		array[2].X = array[1].X + rectangle.Height - Dpi.Width6;
		array[2].Y = rectangle.Y + Dpi.Height3;
		array[3].X = array[2].X + Dpi.Width4;
		array[3].Y = rectangle.Y + Dpi.Height1;
		val.AddCurve(array, 0, 3, 0.5f);
		val.AddLine(array[3].X + 1, rectangle.Y, rectangle.Right - ᔺ, rectangle.Y);
		val.AddLine(rectangle.Right - ᔺ, rectangle.Y, rectangle.Right, rectangle.Y + ᔺ);
		val.AddLine(rectangle.Right, rectangle.Y + ᔺ, rectangle.Right, rectangle.Bottom - 1);
		if (ᔻ)
		{
			val.AddLine(array[0].X, rectangle.Bottom, rectangle.Right, rectangle.Bottom);
			val.CloseAllFigures();
		}
		switch (_0736)
		{
		case eTabStripAlignment.Bottom:
			val.Dispose();
			val = new GraphicsPath();
			array = new Point[4];
			array[0].X = rectangle.X + Dpi.Width2 - (rectangle.Height + Dpi.Width1);
			array[0].Y = rectangle.Top + Dpi.Height1;
			array[1].X = array[0].X + Dpi.Width3;
			array[1].Y = array[0].Y + Dpi.Height2;
			array[2].X = array[1].X + rectangle.Height - Dpi.Width6;
			array[2].Y = rectangle.Bottom - Dpi.Height3;
			array[3].X = array[2].X + Dpi.Width4;
			array[3].Y = rectangle.Bottom - Dpi.Height1;
			val.AddCurve(array, 0, 3, 0.5f);
			val.AddLine(array[3].X + Dpi.Width1, rectangle.Bottom, rectangle.Right - ᔺ, rectangle.Bottom);
			val.AddLine(rectangle.Right - ᔺ, rectangle.Bottom, rectangle.Right, rectangle.Bottom - ᔺ);
			val.AddLine(rectangle.Right, rectangle.Bottom - ᔺ, rectangle.Right, rectangle.Y + Dpi.Height1);
			if (ᔻ)
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

	private void _0DF4(Graphics _٥)
	{
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Expected O, but got Unknown
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Expected O, but got Unknown
		//IL_05e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f0: Expected O, but got Unknown
		//IL_04a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04af: Expected O, but got Unknown
		//IL_04bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c4: Expected O, but got Unknown
		if (ᑬ == null)
		{
			చ();
		}
		if (լ.TabBackground.IsEmpty)
		{
			ᑬ._0EE5(_٥, ᑷ.ᒁ, ᒂ.ᒃ, ((Control)this).ClientRectangle);
		}
		else if (!լ.TabBackground2.IsEmpty && ((Control)this).Width > 0 && ((Control)this).Height > 0)
		{
			LinearGradientBrush val = BarFunctions.ॴ(((Control)this).ClientRectangle, լ.TabBackground, լ.TabBackground2, լ.TabBackgroundGradientAngle);
			try
			{
				_٥.FillRectangle((Brush)(object)val, ((Control)this).ClientRectangle);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		else
		{
			SolidBrush val2 = new SolidBrush(լ.TabBackground);
			try
			{
				_٥.FillRectangle((Brush)(object)val2, ((Control)this).DisplayRectangle);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		Rectangle rectangle = ᔯ(((Control)this).DisplayRectangle, ᔰ: false, ᔱ: false);
		TabItem selectedTab = SelectedTab;
		eTextFormat ᔽ = eTextFormat.EndEllipsis | eTextFormat.HorizontalCenter | eTextFormat.SingleLine | eTextFormat.VerticalCenter;
		if (ᓼ)
		{
			_0BE5(_٥, rectangle);
		}
		if (((Control)this).Parent is TabControl)
		{
			if (ᓹ == eTabStripAlignment.Top)
			{
				ᑬ._0EE5(_٥, ᑷ.ᒀ, ᒂ.ᒄ, new Rectangle(0, rectangle.Bottom, ((Control)this).ClientRectangle.Width, ((Control)this).ClientRectangle.Height));
			}
			else
			{
				Size size = new Size(((Control)this).Width, 5);
				if (ᓹ == eTabStripAlignment.Left || ᓹ == eTabStripAlignment.Right)
				{
					size = new Size(((Control)this).Height, 5);
				}
				Bitmap val3 = new Bitmap(size.Width, size.Height, _٥);
				try
				{
					Graphics val4 = Graphics.FromImage((Image)(object)val3);
					try
					{
						ᑬ._0EE5(val4, ᑷ.ᒀ, ᒂ.ᒄ, new Rectangle(0, 0, size.Width, size.Height));
					}
					finally
					{
						val4.Dispose();
					}
					if (ᓹ == eTabStripAlignment.Left)
					{
						((Image)val3).RotateFlip((RotateFlipType)3);
						_٥.DrawImageUnscaled((Image)(object)val3, rectangle.Right, 0);
					}
					else if (ᓹ == eTabStripAlignment.Right)
					{
						((Image)val3).RotateFlip((RotateFlipType)1);
						_٥.DrawImageUnscaled((Image)(object)val3, rectangle.X - ((Image)val3).Width, 0);
					}
					else if (ᓹ == eTabStripAlignment.Bottom)
					{
						((Image)val3).RotateFlip((RotateFlipType)2);
						_٥.DrawImageUnscaled((Image)(object)val3, 0, rectangle.Y - ((Image)val3).Height);
					}
				}
				finally
				{
					((Image)val3).Dispose();
				}
			}
		}
		if (ᓹ == eTabStripAlignment.Top)
		{
			rectangle.Height++;
		}
		Rectangle clip = rectangle;
		if (ᓹ == eTabStripAlignment.Bottom)
		{
			clip.Y--;
			clip.Height++;
		}
		else if (ᓹ == eTabStripAlignment.Left)
		{
			clip.Width++;
		}
		else if (ᓹ == eTabStripAlignment.Right)
		{
			clip.X--;
			clip.Width++;
		}
		if (ᓠ)
		{
			if (ᓹ == eTabStripAlignment.Right || ᓹ == eTabStripAlignment.Left)
			{
				clip.Height -= ᔂ.DisplayRectangle.Height;
			}
			else
			{
				clip.Width -= ᔂ.DisplayRectangle.Width;
			}
		}
		_٥.SetClip(clip);
		foreach (TabItem item in ᓺ)
		{
			if (!item.Visible || !rectangle.IntersectsWith(item.DisplayRectangle))
			{
				continue;
			}
			if (ᓞ)
			{
				RenderTabItemEventArgs renderTabItemEventArgs = new RenderTabItemEventArgs(item, _٥);
				ᕉ(renderTabItemEventArgs);
				if (renderTabItemEventArgs.Cancel)
				{
					continue;
				}
			}
			Rectangle displayRectangle = item.DisplayRectangle;
			Rectangle rectangle2 = new Rectangle(0, 0, displayRectangle.Width, displayRectangle.Height);
			if (ᓹ == eTabStripAlignment.Left || ᓹ == eTabStripAlignment.Right)
			{
				rectangle2 = new Rectangle(0, 0, displayRectangle.Height, displayRectangle.Width);
			}
			if (item == selectedTab)
			{
				if (ᓹ == eTabStripAlignment.Top)
				{
					rectangle2.Height += 2;
				}
				else if (ᓹ == eTabStripAlignment.Bottom)
				{
					rectangle2.Height += 2;
					displayRectangle.Y--;
				}
				else if (ᓹ == eTabStripAlignment.Left)
				{
					rectangle2.Height += 2;
					displayRectangle.X--;
				}
				else if (ᓹ == eTabStripAlignment.Right)
				{
					rectangle2.Height += 2;
					displayRectangle.X--;
				}
			}
			Bitmap val5 = new Bitmap(rectangle2.Width, rectangle2.Height, _٥);
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
					if (ᓹ == eTabStripAlignment.Bottom)
					{
						ᔼ(val6, rectangle2, item, ᔽ, rectangle2, ᔿ: true, ᕀ: false);
						((Image)val5).RotateFlip((RotateFlipType)2);
						ᔼ(val6, rectangle2, item, ᔽ, rectangle2, ᔿ: false, ᕀ: true);
					}
					else
					{
						ᔼ(val6, rectangle2, item, ᔽ, rectangle2, ᔿ: true, ᕀ: true);
					}
				}
				finally
				{
					val6.Dispose();
				}
			}
			finally
			{
				if (ᓹ == eTabStripAlignment.Left)
				{
					((Image)val5).RotateFlip((RotateFlipType)3);
				}
				else if (ᓹ == eTabStripAlignment.Right)
				{
					((Image)val5).RotateFlip((RotateFlipType)1);
				}
				_٥.DrawImageUnscaled((Image)(object)val5, displayRectangle);
				((Image)val5).Dispose();
			}
			if (ᓟ)
			{
				RenderTabItemEventArgs ؿ = new RenderTabItemEventArgs(item, _٥);
				ᕊ(ؿ);
			}
		}
		_٥.ResetClip();
		if (ᓠ && ᔂ.Visible)
		{
			_٥.SetClip(ᔂ.DisplayRectangle);
			SolidBrush val8 = new SolidBrush(լ.TabBackground);
			try
			{
				_٥.FillRectangle((Brush)(object)val8, ᔂ.DisplayRectangle);
			}
			finally
			{
				((IDisposable)val8)?.Dispose();
			}
			ᔂ.Paint(_٥);
			_٥.ResetClip();
		}
	}

	private void ᔼ(Graphics _٥, Rectangle ᔸ, TabItem ᆰ, eTextFormat ᔽ, Rectangle ᔾ, bool ᔿ, bool ᕀ)
	{
		TabColors tabColors = ᕝ(ᆰ);
		ᒂ య = ᒂ.ཊ;
		if (ᔎ == ᆰ)
		{
			య = ᒂ._0F77;
		}
		else if (ᆰ == ᓻ)
		{
			య = ᒂ.ᒅ;
		}
		if (ᔿ)
		{
			if (ᆰ == ᓻ)
			{
				ᑬ._0EE5(_٥, ᑷ.ᑸ, య, new Rectangle(ᔸ.X, ᔸ.Y, ᔸ.Width, ᔸ.Height + 1));
			}
			else
			{
				ᑬ._0EE5(_٥, ᑷ.ᑸ, య, new Rectangle(ᔸ.X, ᔸ.Y, ᔸ.Width, ᔸ.Height + 1));
			}
		}
		ᔸ.Offset(0, 1);
		if (!ᕀ)
		{
			return;
		}
		if (ᔝ && ᆰ.CloseButtonVisible)
		{
			Rectangle closeButtonBounds = ᔴ(_٥, ᓹ == eTabStripAlignment.Left || ᓹ == eTabStripAlignment.Right, ᆰ.CloseButtonMouseOver, ᆰ == ᔎ || ᆰ == SelectedTab, ref ᔸ);
			closeButtonBounds.Offset(ᆰ.DisplayRectangle.Location);
			ᆰ.CloseButtonBounds = closeButtonBounds;
		}
		else
		{
			ᆰ.CloseButtonBounds = Rectangle.Empty;
		}
		Image image = ᆰ.GetImage();
		Icon icon = ᆰ.Icon;
		Size size = ((image != null) ? Dpi.ImageSize(image.Size) : Size.Empty);
		if ((image != null && image.Width + 4 <= ᔸ.Width) || (icon != null && ᆰ.IconSize.Width + 4 <= ᔸ.Width))
		{
			if (icon != null)
			{
				Rectangle rectangle = new Rectangle(ᔸ.X + 4, ᔸ.Y + (ᔸ.Height - ᆰ.IconSize.Height) / 2, ᆰ.IconSize.Width, ᆰ.IconSize.Height);
				if (ᔾ.Contains(rectangle))
				{
					_٥.DrawIcon(icon, rectangle);
				}
				ᔸ.X += ᆰ.IconSize.Width + 2;
				ᔸ.Width -= ᆰ.IconSize.Width + 2;
			}
			else if (image != null)
			{
				_٥.DrawImage(image, ᔸ.X + 4, ᔸ.Y + (ᔸ.Height - size.Height) / 2, size.Width, size.Height);
				ᔸ.X += size.Width + 2;
				ᔸ.Width -= size.Width + 2;
			}
			ᔸ.Inflate(0, -1);
			ᔸ.Width -= 4;
			ᔸ.X += 3;
		}
		if (!ᔍ || ᆰ == ᓻ)
		{
			Font font = ((Control)this).Font;
			if (ᆰ == ᓻ && ᔋ != null)
			{
				font = ᔋ;
			}
			if (ᔸ.Width > ᔗ)
			{
				TextDrawing.DrawString(_٥, ᆰ.Text, font, tabColors.TextColor, ᔸ, ᔽ);
			}
			if (ᑫ && ((Control)this).Focused && ᆰ == ᓻ)
			{
				ControlPaint.DrawFocusRectangle(_٥, ᔪ(ᔸ));
			}
		}
	}

	private void ᕁ(Graphics _٥)
	{
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Expected O, but got Unknown
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Expected O, but got Unknown
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0341: Expected O, but got Unknown
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Expected O, but got Unknown
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Expected O, but got Unknown
		//IL_0511: Unknown result type (might be due to invalid IL or missing references)
		//IL_0518: Expected O, but got Unknown
		//IL_0d2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d33: Expected O, but got Unknown
		//IL_07e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f0: Expected O, but got Unknown
		//IL_085d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0864: Expected O, but got Unknown
		if (!լ.TabBackground2.IsEmpty && ((Control)this).Width > 0 && ((Control)this).Height > 0)
		{
			LinearGradientBrush val = BarFunctions.ॴ(((Control)this).ClientRectangle, լ.TabBackground, լ.TabBackground2, լ.TabBackgroundGradientAngle);
			try
			{
				_٥.FillRectangle((Brush)(object)val, ((Control)this).ClientRectangle);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		else
		{
			SolidBrush val2 = new SolidBrush(լ.TabBackground);
			try
			{
				_٥.FillRectangle((Brush)(object)val2, ((Control)this).DisplayRectangle);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		Rectangle rectangle = ᔯ(((Control)this).DisplayRectangle, ᔰ: false, ᔱ: false);
		Rectangle displayRectangle = ((Control)this).DisplayRectangle;
		if (ᓼ)
		{
			_0BE5(_٥, rectangle);
		}
		TabItem selectedTab = SelectedTab;
		Rectangle rectangle2 = Rectangle.Empty;
		if (selectedTab != null)
		{
			rectangle2 = selectedTab.DisplayRectangle;
		}
		eTextFormat format = eTextFormat.EndEllipsis | eTextFormat.HorizontalCenter | eTextFormat.SingleLine | eTextFormat.VerticalCenter;
		switch (ᓹ)
		{
		case eTabStripAlignment.Top:
		{
			Pen val6 = new Pen(լ.TabItemSelectedBorder, 1f);
			try
			{
				if (!rectangle2.IsEmpty)
				{
					_٥.DrawLine(val6, displayRectangle.X, rectangle.Bottom, rectangle2.X, rectangle.Bottom);
					_٥.DrawLine(val6, rectangle2.Right - 1, rectangle.Bottom, displayRectangle.Right, rectangle.Bottom);
				}
				else
				{
					_٥.DrawLine(val6, displayRectangle.X, displayRectangle.Bottom - 1, displayRectangle.Right, displayRectangle.Bottom - 1);
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
			Pen val4 = new Pen(լ.TabItemSelectedBorder, 1f);
			try
			{
				if (!rectangle2.IsEmpty)
				{
					_٥.DrawLine(val4, displayRectangle.X, displayRectangle.Y, rectangle2.X, displayRectangle.Y);
					_٥.DrawLine(val4, rectangle2.Right - 1, displayRectangle.Y, displayRectangle.Right, displayRectangle.Y);
				}
				else
				{
					_٥.DrawLine(val4, displayRectangle.X, displayRectangle.Y, displayRectangle.Right, displayRectangle.Y);
				}
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
			rectangle.Y++;
			rectangle.Height--;
			break;
		}
		case eTabStripAlignment.Left:
		{
			Pen val5 = new Pen(լ.TabItemSelectedBorder, 1f);
			try
			{
				if (!rectangle2.IsEmpty)
				{
					_٥.DrawLine(val5, displayRectangle.Right - 1, displayRectangle.Y, displayRectangle.Right - 1, rectangle2.Y);
					_٥.DrawLine(val5, displayRectangle.Right - 1, rectangle2.Bottom - 1, displayRectangle.Right - 1, displayRectangle.Bottom);
				}
				else
				{
					_٥.DrawLine(val5, displayRectangle.Right - 1, displayRectangle.Y, displayRectangle.Right - 1, displayRectangle.Bottom);
				}
			}
			finally
			{
				((IDisposable)val5)?.Dispose();
			}
			rectangle.Width--;
			break;
		}
		case eTabStripAlignment.Right:
		{
			Pen val3 = new Pen(լ.TabItemSelectedBorder, 1f);
			try
			{
				if (!rectangle2.IsEmpty)
				{
					_٥.DrawLine(val3, displayRectangle.X, displayRectangle.Y, displayRectangle.X, rectangle2.Y);
					_٥.DrawLine(val3, displayRectangle.X, rectangle2.Bottom - 1, displayRectangle.X, displayRectangle.Bottom);
				}
				else
				{
					_٥.DrawLine(val3, displayRectangle.X, displayRectangle.Y, displayRectangle.X, displayRectangle.Bottom);
				}
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
			rectangle.Width--;
			rectangle.X++;
			break;
		}
		}
		if (ᓠ && ᔂ.Visible)
		{
			if (ᓹ == eTabStripAlignment.Right || ᓹ == eTabStripAlignment.Left)
			{
				rectangle.Height -= ᔂ.DisplayRectangle.Height;
			}
			else
			{
				rectangle.Width -= ᔂ.DisplayRectangle.Width;
				if (ᓛ)
				{
					rectangle.X += ᔂ.DisplayRectangle.Width;
				}
			}
		}
		Rectangle clip = rectangle;
		if (ᓹ == eTabStripAlignment.Right)
		{
			clip.Width++;
		}
		_٥.SetClip(clip);
		foreach (TabItem item in ᓺ)
		{
			if (!item.Visible || !rectangle.IntersectsWith(item.DisplayRectangle))
			{
				continue;
			}
			if (ᓞ)
			{
				RenderTabItemEventArgs renderTabItemEventArgs = new RenderTabItemEventArgs(item, _٥);
				ᕉ(renderTabItemEventArgs);
				if (renderTabItemEventArgs.Cancel)
				{
					continue;
				}
			}
			Rectangle ᔸ = item.DisplayRectangle;
			TabColors tabColors = ᕝ(item);
			GraphicsPath val7 = new GraphicsPath();
			switch (ᓹ)
			{
			case eTabStripAlignment.Top:
				ᔸ.Width--;
				val7.AddLine(ᔸ.X + 2, ᔸ.Y, ᔸ.Right - 2, ᔸ.Y);
				val7.AddLine(ᔸ.Right, ᔸ.Y + 2, ᔸ.Right, ᔸ.Bottom);
				val7.AddLine(ᔸ.Right, ᔸ.Bottom, ᔸ.X, ᔸ.Bottom);
				val7.AddLine(ᔸ.X, ᔸ.Y + 2, ᔸ.X + 2, ᔸ.Y);
				val7.CloseAllFigures();
				break;
			case eTabStripAlignment.Bottom:
				ᔸ.Width--;
				val7.AddLine(ᔸ.X, ᔸ.Y, ᔸ.Right, ᔸ.Y);
				val7.AddLine(ᔸ.Right, ᔸ.Y, ᔸ.Right, ᔸ.Bottom - 2);
				val7.AddLine(ᔸ.Right - 2, ᔸ.Bottom, ᔸ.X + 2, ᔸ.Bottom);
				val7.AddLine(ᔸ.X, ᔸ.Bottom - 2, ᔸ.X, ᔸ.Y);
				val7.CloseAllFigures();
				break;
			case eTabStripAlignment.Left:
				ᔸ.Height--;
				val7.AddLine(ᔸ.X + 2, ᔸ.Y, ᔸ.Right, ᔸ.Y);
				val7.AddLine(ᔸ.Right, ᔸ.Bottom, ᔸ.X + 2, ᔸ.Bottom);
				val7.AddLine(ᔸ.X, ᔸ.Bottom - 2, ᔸ.X, ᔸ.Y + 2);
				val7.CloseAllFigures();
				break;
			case eTabStripAlignment.Right:
				ᔸ.Height--;
				ᔸ.Width -= 2;
				val7.AddLine(ᔸ.X, ᔸ.Y, ᔸ.Right - 2, ᔸ.Y);
				val7.AddLine(ᔸ.Right, ᔸ.Y + 2, ᔸ.Right, ᔸ.Bottom - 2);
				val7.AddLine(ᔸ.Right - 2, ᔸ.Bottom, ᔸ.X, ᔸ.Bottom);
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
						_٥.FillPath((Brush)(object)val8, val7);
					}
					finally
					{
						((IDisposable)val8)?.Dispose();
					}
				}
			}
			else
			{
				LinearGradientBrush val9 = BarFunctions.ॴ(ᔸ, tabColors.BackColor, tabColors.BackColor2, tabColors.BackColorGradientAngle);
				try
				{
					_٥.FillPath((Brush)(object)val9, val7);
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
					_٥.DrawPath(val10, val7);
				}
				finally
				{
					((IDisposable)val10)?.Dispose();
				}
			}
			if (ᔝ && item.CloseButtonVisible)
			{
				item.CloseButtonBounds = ᔴ(_٥, ᓹ == eTabStripAlignment.Left || ᓹ == eTabStripAlignment.Right, item.CloseButtonMouseOver, item == ᔎ || item == SelectedTab, ref ᔸ);
			}
			else
			{
				item.CloseButtonBounds = Rectangle.Empty;
			}
			ᇎ ᇎ2 = ᕂ(item);
			if (ᇎ2 != null && ᇎ2.ܐ + 4 <= ᔸ.Width)
			{
				if (ᓹ == eTabStripAlignment.Top || ᓹ == eTabStripAlignment.Bottom)
				{
					ᇎ2.ᇗ(_٥, new Rectangle(ᔸ.X + 3, ᔸ.Y + (ᔸ.Height - ᇎ2._0711) / 2, ᇎ2.ܐ, ᇎ2._0711));
					int num = ᇎ2.ܐ + 2;
					ᔸ.X += num;
					ᔸ.Width -= num;
				}
				else
				{
					ᇎ2.ᇗ(_٥, new Rectangle(ᔸ.X + (ᔸ.Width - ᇎ2.ܐ) / 2, ᔸ.Y + 3, ᇎ2.ܐ, ᇎ2._0711));
					int num2 = ᇎ2._0711 + 2;
					ᔸ.Y += num2;
					ᔸ.Height -= num2;
				}
			}
			if (!ᔍ || item == ᓻ)
			{
				Font font = ((Control)this).Font;
				if (item == selectedTab && ᔋ != null)
				{
					font = ᔋ;
				}
				Rectangle bounds = ᔸ;
				if (ᓹ == eTabStripAlignment.Left || ᓹ == eTabStripAlignment.Right)
				{
					_٥.RotateTransform(90f);
					bounds = new Rectangle(bounds.Top, -bounds.Right, bounds.Height, bounds.Width);
				}
				if (bounds.Width > ᔗ)
				{
					if (ᓹ == eTabStripAlignment.Left || ᓹ == eTabStripAlignment.Right)
					{
						TextDrawing.DrawStringLegacy(_٥, item.Text, font, tabColors.TextColor, bounds, format);
					}
					else
					{
						TextDrawing.DrawString(_٥, item.Text, font, tabColors.TextColor, bounds, format);
					}
				}
				if (ᓹ == eTabStripAlignment.Left || ᓹ == eTabStripAlignment.Right)
				{
					_٥.ResetTransform();
				}
				if (ᑫ && ((Control)this).Focused && item == selectedTab)
				{
					ControlPaint.DrawFocusRectangle(_٥, ᔪ(ᔸ));
				}
			}
			if (!item.IsSelected && ᓺ.IndexOf(item) < ᓺ.Count - 1 && ᓺ.IndexOf(item) + 1 != ᓺ.IndexOf(ᓻ))
			{
				if (ᓹ == eTabStripAlignment.Left || ᓹ == eTabStripAlignment.Right)
				{
					DisplayHelp.DrawLine(_٥, item.DisplayRectangle.X + 3, item.DisplayRectangle.Bottom - 1, item.DisplayRectangle.Right - 3, item.DisplayRectangle.Bottom - 1, լ.TabItemSeparator, 1);
					DisplayHelp.DrawLine(_٥, item.DisplayRectangle.X + 3, item.DisplayRectangle.Bottom, item.DisplayRectangle.Right - 3, item.DisplayRectangle.Bottom, լ.TabItemSeparatorShade, 1);
				}
				else
				{
					DisplayHelp.DrawLine(_٥, item.DisplayRectangle.Right - 1, item.DisplayRectangle.Y + 3, item.DisplayRectangle.Right - 1, item.DisplayRectangle.Bottom - 3, լ.TabItemSeparator, 1);
					DisplayHelp.DrawLine(_٥, item.DisplayRectangle.Right, item.DisplayRectangle.Y + 3, item.DisplayRectangle.Right, item.DisplayRectangle.Bottom - 3, լ.TabItemSeparatorShade, 1);
				}
			}
			if (ᓟ)
			{
				RenderTabItemEventArgs ؿ = new RenderTabItemEventArgs(item, _٥);
				ᕊ(ؿ);
			}
		}
		_٥.ResetClip();
		if (ᓠ && ᔂ.Visible)
		{
			_٥.SetClip(ᔂ.DisplayRectangle);
			SolidBrush val11 = new SolidBrush(լ.TabBackground);
			try
			{
				_٥.FillRectangle((Brush)(object)val11, ᔂ.DisplayRectangle);
			}
			finally
			{
				((IDisposable)val11)?.Dispose();
			}
			ᔂ.Paint(_٥);
			_٥.ResetClip();
		}
	}

	private ᇎ ᕂ(TabItem ᆰ)
	{
		Image image = ᆰ.GetImage();
		if (image != null)
		{
			return new ᇎ(image, dispose: false);
		}
		Icon icon = ᆰ.Icon;
		if (icon != null)
		{
			return new ᇎ(icon, dispose: false, ᆰ.IconSize);
		}
		return null;
	}

	internal void ResetTabHeight()
	{
		ᔥ = 0;
	}

	private int ᕃ()
	{
		if (ᓡ)
		{
			int result = 0;
			if (BarFunctions.IsHandleValid((Control)(object)this))
			{
				Graphics val = ((Control)this).CreateGraphics();
				try
				{
					result = ᕄ(val);
				}
				finally
				{
					val.Dispose();
				}
			}
			return result;
		}
		return ᓘ;
	}

	private int ᕄ(Graphics _٥)
	{
		if (ᔜ.Height > 0)
		{
			return Dpi.Height(ᔜ.Height);
		}
		int num = Dpi.Height16;
		if ((ᓹ == eTabStripAlignment.Left || ᓹ == eTabStripAlignment.Right) && Ӷ != eTabStripStyle.OneNote && Ӷ != eTabStripStyle.VS2005Document && Ӷ != eTabStripStyle.Office2007Document)
		{
			num = Dpi.Height20;
		}
		if (գ != null)
		{
			num = Dpi.Height(գ.ImageSize.Height);
		}
		else
		{
			Font font = ((Control)this).Font;
			if (ᔋ != null)
			{
				font = ᔋ;
			}
			foreach (TabItem item in ᓺ)
			{
				if (item.Icon != null && item.Icon.Height > num)
				{
					num = Dpi.Height(item.IconSize.Height);
				}
				else if (item.Image != null && item.Image.Height > num)
				{
					num = Dpi.Height(item.Image.Height);
				}
				string text = item.Text;
				if (text != "")
				{
					Size empty = Size.Empty;
					empty = ((!ᓙ) ? TextDrawing.MeasureString(_٥, item.Text, font) : TextDrawing.MeasureStringLegacy(_٥, item.Text, font, Size.Empty, eTextFormat.Default));
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
		num += Dpi.Height4;
		if (Ӷ == eTabStripStyle.Metro)
		{
			num += Dpi.Height2;
		}
		if (ᓨ)
		{
			num += Dpi.Width1;
		}
		return num;
	}

	public void RecalcSize()
	{
		if (!BarFunctions.IsHandleValid((Control)(object)this))
		{
			ᓼ = true;
			return;
		}
		Graphics val = ((Control)this).CreateGraphics();
		try
		{
			_0BE5(val, ᔯ(((Control)this).DisplayRectangle, TabLayoutType == eTabLayoutType.MultilineWithNavigationBox, ᔱ: false));
		}
		finally
		{
			val.Dispose();
		}
	}

	private void ᕅ(Graphics _٥, Rectangle ᕆ)
	{
		if (Tabs.Count == 0)
		{
			if (ᓛ)
			{
				ᕌ(ᕆ, Rectangle.Empty);
			}
			else
			{
				ᕌ(ᕆ, Rectangle.Empty);
			}
			return;
		}
		SerialContentLayoutManager serialContentLayoutManager = new SerialContentLayoutManager();
		TabItemLayoutManager tabItemLayoutManager = new TabItemLayoutManager(this);
		tabItemLayoutManager.FixedTabSize = ᔜ;
		tabItemLayoutManager.CloseButtonOnTabs = ᔝ;
		Size closeButtonSize = Dpi.Size(ᔠ);
		tabItemLayoutManager.CloseButtonSize = closeButtonSize;
		serialContentLayoutManager.RightToLeft = ᓛ;
		if (Ӷ == eTabStripStyle.RoundHeader)
		{
			serialContentLayoutManager.ContentAlignment = eContentAlignment.Center;
			serialContentLayoutManager.EvenHeight = true;
			serialContentLayoutManager.FitContainerOversize = true;
		}
		else if (Ӷ == eTabStripStyle.Office2007Document)
		{
			serialContentLayoutManager.EvenHeight = true;
			if (ᓹ == eTabStripAlignment.Bottom)
			{
				serialContentLayoutManager.ContentAlignment = eContentAlignment.Left;
				serialContentLayoutManager.ContentVerticalAlignment = eContentVerticalAlignment.Top;
				serialContentLayoutManager.BlockLineAlignment = eContentVerticalAlignment.Top;
				serialContentLayoutManager.ContentOrientation = eContentOrientation.Horizontal;
			}
			else if (ᓹ == eTabStripAlignment.Top)
			{
				serialContentLayoutManager.ContentAlignment = eContentAlignment.Left;
				serialContentLayoutManager.ContentVerticalAlignment = eContentVerticalAlignment.Bottom;
				serialContentLayoutManager.BlockLineAlignment = eContentVerticalAlignment.Bottom;
				serialContentLayoutManager.ContentOrientation = eContentOrientation.Horizontal;
			}
			else if (ᓹ == eTabStripAlignment.Left)
			{
				serialContentLayoutManager.ContentAlignment = eContentAlignment.Right;
				serialContentLayoutManager.ContentVerticalAlignment = eContentVerticalAlignment.Top;
				serialContentLayoutManager.BlockLineAlignment = eContentVerticalAlignment.Top;
				serialContentLayoutManager.ContentOrientation = eContentOrientation.Vertical;
			}
			else if (ᓹ == eTabStripAlignment.Right)
			{
				serialContentLayoutManager.ContentAlignment = eContentAlignment.Left;
				serialContentLayoutManager.ContentVerticalAlignment = eContentVerticalAlignment.Top;
				serialContentLayoutManager.BlockLineAlignment = eContentVerticalAlignment.Top;
				serialContentLayoutManager.ContentOrientation = eContentOrientation.Vertical;
			}
			serialContentLayoutManager.BlockSpacing = 0;
			if (ᓹ == eTabStripAlignment.Top || ᓹ == eTabStripAlignment.Bottom)
			{
				ᕆ.Width -= Dpi.Width2;
			}
			else
			{
				ᕆ.Height -= Dpi.Height2;
			}
			tabItemLayoutManager.PaddingHeight = 6;
		}
		else if (Ӷ == eTabStripStyle.Office2007Dock)
		{
			serialContentLayoutManager.EvenHeight = true;
			if (ᓹ == eTabStripAlignment.Bottom)
			{
				serialContentLayoutManager.ContentAlignment = eContentAlignment.Left;
				serialContentLayoutManager.ContentVerticalAlignment = eContentVerticalAlignment.Top;
				serialContentLayoutManager.BlockLineAlignment = eContentVerticalAlignment.Top;
				serialContentLayoutManager.ContentOrientation = eContentOrientation.Horizontal;
			}
			else if (ᓹ == eTabStripAlignment.Top)
			{
				serialContentLayoutManager.ContentAlignment = eContentAlignment.Left;
				serialContentLayoutManager.ContentVerticalAlignment = eContentVerticalAlignment.Bottom;
				serialContentLayoutManager.BlockLineAlignment = eContentVerticalAlignment.Bottom;
				serialContentLayoutManager.ContentOrientation = eContentOrientation.Horizontal;
			}
			else if (ᓹ == eTabStripAlignment.Left)
			{
				serialContentLayoutManager.ContentAlignment = eContentAlignment.Right;
				serialContentLayoutManager.ContentVerticalAlignment = eContentVerticalAlignment.Top;
				serialContentLayoutManager.BlockLineAlignment = eContentVerticalAlignment.Top;
				serialContentLayoutManager.ContentOrientation = eContentOrientation.Vertical;
			}
			else if (ᓹ == eTabStripAlignment.Right)
			{
				serialContentLayoutManager.ContentAlignment = eContentAlignment.Left;
				serialContentLayoutManager.ContentVerticalAlignment = eContentVerticalAlignment.Top;
				serialContentLayoutManager.BlockLineAlignment = eContentVerticalAlignment.Top;
				serialContentLayoutManager.ContentOrientation = eContentOrientation.Vertical;
			}
			serialContentLayoutManager.BlockSpacing = Dpi.Width2;
			if (ᓹ == eTabStripAlignment.Top || ᓹ == eTabStripAlignment.Bottom)
			{
				ᕆ.X += Dpi.Width2;
				ᕆ.Width -= Dpi.Width2;
			}
			else
			{
				ᕆ.Y += Dpi.Height2;
				ᕆ.Height -= Dpi.Height2;
			}
			tabItemLayoutManager.PaddingHeight = 6;
		}
		else if (Ӷ == eTabStripStyle.VS2005Dock)
		{
			serialContentLayoutManager.EvenHeight = true;
			if (ᓹ == eTabStripAlignment.Bottom)
			{
				serialContentLayoutManager.ContentAlignment = eContentAlignment.Left;
				serialContentLayoutManager.ContentVerticalAlignment = eContentVerticalAlignment.Top;
				serialContentLayoutManager.BlockLineAlignment = eContentVerticalAlignment.Top;
				serialContentLayoutManager.ContentOrientation = eContentOrientation.Horizontal;
			}
			else if (ᓹ == eTabStripAlignment.Top)
			{
				serialContentLayoutManager.ContentAlignment = eContentAlignment.Left;
				serialContentLayoutManager.ContentVerticalAlignment = eContentVerticalAlignment.Bottom;
				serialContentLayoutManager.BlockLineAlignment = eContentVerticalAlignment.Bottom;
				serialContentLayoutManager.ContentOrientation = eContentOrientation.Horizontal;
			}
			else if (ᓹ == eTabStripAlignment.Left)
			{
				serialContentLayoutManager.ContentAlignment = eContentAlignment.Right;
				serialContentLayoutManager.ContentVerticalAlignment = eContentVerticalAlignment.Top;
				serialContentLayoutManager.BlockLineAlignment = eContentVerticalAlignment.Top;
				serialContentLayoutManager.ContentOrientation = eContentOrientation.Vertical;
			}
			else if (ᓹ == eTabStripAlignment.Right)
			{
				serialContentLayoutManager.ContentAlignment = eContentAlignment.Left;
				serialContentLayoutManager.ContentVerticalAlignment = eContentVerticalAlignment.Top;
				serialContentLayoutManager.BlockLineAlignment = eContentVerticalAlignment.Top;
				serialContentLayoutManager.ContentOrientation = eContentOrientation.Vertical;
			}
			serialContentLayoutManager.BlockSpacing = Dpi.Width8;
			if (ᓹ == eTabStripAlignment.Top || ᓹ == eTabStripAlignment.Bottom)
			{
				ᕆ.X += Dpi.Width4;
				ᕆ.Width -= Dpi.Width8;
			}
			else
			{
				ᕆ.Y += Dpi.Height4;
				ᕆ.Height -= Dpi.Height8;
			}
		}
		else if (Ӷ == eTabStripStyle.SimulatedTheme)
		{
			if (ᓹ == eTabStripAlignment.Top || ᓹ == eTabStripAlignment.Bottom)
			{
				ᕆ.Width -= Dpi.Width3;
			}
			else
			{
				ᕆ.Height -= Dpi.Height3;
			}
			if (ᓹ == eTabStripAlignment.Bottom)
			{
				serialContentLayoutManager.ContentAlignment = eContentAlignment.Left;
				serialContentLayoutManager.ContentVerticalAlignment = eContentVerticalAlignment.Top;
				serialContentLayoutManager.BlockLineAlignment = eContentVerticalAlignment.Top;
				serialContentLayoutManager.ContentOrientation = eContentOrientation.Horizontal;
				serialContentLayoutManager.EvenHeight = true;
			}
			else if (ᓹ == eTabStripAlignment.Top)
			{
				serialContentLayoutManager.ContentAlignment = eContentAlignment.Left;
				serialContentLayoutManager.ContentVerticalAlignment = eContentVerticalAlignment.Bottom;
				serialContentLayoutManager.BlockLineAlignment = eContentVerticalAlignment.Bottom;
				serialContentLayoutManager.ContentOrientation = eContentOrientation.Horizontal;
				serialContentLayoutManager.EvenHeight = true;
			}
			else if (ᓹ == eTabStripAlignment.Left)
			{
				serialContentLayoutManager.ContentAlignment = eContentAlignment.Right;
				serialContentLayoutManager.ContentVerticalAlignment = eContentVerticalAlignment.Top;
				serialContentLayoutManager.BlockLineAlignment = eContentVerticalAlignment.Top;
				serialContentLayoutManager.ContentOrientation = eContentOrientation.Vertical;
				serialContentLayoutManager.EvenHeight = true;
			}
			else if (ᓹ == eTabStripAlignment.Right)
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
		if (ᓹ == eTabStripAlignment.Left || ᓹ == eTabStripAlignment.Right)
		{
			serialContentLayoutManager.ContentOrientation = eContentOrientation.Vertical;
		}
		else
		{
			serialContentLayoutManager.ContentOrientation = eContentOrientation.Horizontal;
		}
		if (Ӷ != eTabStripStyle.RoundHeader)
		{
			if (ᔙ == eTabLayoutType.FitContainer)
			{
				serialContentLayoutManager.FitContainerOversize = true;
				serialContentLayoutManager.OversizeDistribute = true;
				serialContentLayoutManager.MultiLine = false;
			}
			else if (ᔙ == eTabLayoutType.FixedWithNavigationBox)
			{
				serialContentLayoutManager.FitContainerOversize = false;
				serialContentLayoutManager.MultiLine = false;
			}
			else if (ᔙ == eTabLayoutType.MultilineNoNavigationBox || ᔙ == eTabLayoutType.MultilineWithNavigationBox)
			{
				serialContentLayoutManager.FitContainerOversize = false;
				serialContentLayoutManager.MultiLine = true;
			}
		}
		tabItemLayoutManager.Graphics = _٥;
		TabItem[] array = new TabItem[Tabs.Count];
		Tabs.CopyTo(array, 0);
		Rectangle rectangle = ᕆ;
		if (ᔁ != 0)
		{
			if (ᓹ == eTabStripAlignment.Left || ᓹ == eTabStripAlignment.Right)
			{
				rectangle.Y -= ᔁ;
			}
			else
			{
				rectangle.X -= ᔁ;
			}
		}
		if (ᓛ)
		{
			if (ᓠ && ᔂ.Visible)
			{
				rectangle.X += ᔂ.DefaultWidth;
			}
			ᔛ = serialContentLayoutManager.Layout(rectangle, array, tabItemLayoutManager);
			ᕌ(ᕆ, rectangle);
		}
		else
		{
			ᔛ = serialContentLayoutManager.Layout(rectangle, array, tabItemLayoutManager);
			ᕌ(ᕆ, ᔛ);
		}
	}

	private void _0BE5(Graphics _٥, Rectangle ᕆ)
	{
		if (ᓜ)
		{
			ᕅ(_٥, ᕆ);
		}
		else
		{
			ᕇ(_٥, ᕆ);
		}
		ᓼ = false;
		ᕏ();
	}

	private void ᕇ(Graphics _٥, Rectangle ᕆ)
	{
		int num = ᕆ.Width;
		int num2 = 0;
		int num3 = 0;
		int num4 = -1;
		eTextFormat format = eTextFormat.Default;
		if (ᓹ == eTabStripAlignment.Left || ᓹ == eTabStripAlignment.Right)
		{
			num = ᕆ.Height;
		}
		int num5 = (ᔥ = ᕄ(_٥));
		int num6 = ᕆ.X;
		int num7 = ᕆ.Y;
		if (ᓹ == eTabStripAlignment.Top)
		{
			num7 = Math.Max(0, ᕆ.Bottom - num5);
		}
		else if (ᓹ == eTabStripAlignment.Left)
		{
			num6 = Math.Max(0, ᕆ.Right - num5);
		}
		if (ᓹ == eTabStripAlignment.Left || ᓹ == eTabStripAlignment.Right)
		{
			num7 -= ᔁ;
		}
		else
		{
			num6 -= ᔁ;
		}
		int num8 = num6;
		int num9 = num7;
		bool flag = false;
		int num10 = Dpi.Width(ᔂ.DefaultWidth);
		if (ᓠ)
		{
			num -= num10;
			num -= Dpi.Width4;
		}
		ArrayList arrayList = new ArrayList();
		foreach (TabItem item in ᓺ)
		{
			int num11 = Dpi.Width4;
			if (!item.Visible)
			{
				continue;
			}
			if (num4 < 0)
			{
				num4 = ᓺ.IndexOf(item);
			}
			num3++;
			if (ᔜ.Width == 0)
			{
				Image image = item.GetImage();
				if (item.Icon != null)
				{
					num11 += item.IconSize.Width;
					if (Ӷ != eTabStripStyle.OneNote || ᓨ)
					{
						num11 += Dpi.Width4;
					}
				}
				else if (image != null)
				{
					num11 += Dpi.Width(image.Width);
					if (Ӷ != eTabStripStyle.OneNote || ᓨ)
					{
						num11 += Dpi.Width4;
					}
				}
				if (Ӷ == eTabStripStyle.OneNote && !ᓨ)
				{
					num11 += num5 - Dpi.Height6;
				}
				else if (Ӷ == eTabStripStyle.VS2005Document && !ᓨ)
				{
					num11 += Dpi.Width4;
				}
				if (item.Text != "" && (!ᔍ || item == ᓻ))
				{
					Font font = ((Control)this).Font;
					if (ᔋ != null && (item == ᓻ || (ᔞ == eTabCloseButtonPosition.Right && ᔣ)))
					{
						font = ᔋ;
					}
					Size empty = Size.Empty;
					num11 += ((!ᓙ) ? TextDrawing.MeasureString(_٥, item.Text, font, 0, format) : TextDrawing.MeasureStringLegacy(_٥, item.Text, font, Size.Empty, format)).Width;
					if ((Ӷ != eTabStripStyle.OneNote && Ӷ != eTabStripStyle.VS2005Document) || ᓨ || ᓹ == eTabStripAlignment.Top || ᓹ == eTabStripAlignment.Bottom)
					{
						num11 += Dpi.Width4;
					}
					if (ᓨ)
					{
						num11 += Dpi.Width10;
					}
				}
				if (ᔝ && item.CloseButtonVisible && Ӷ != eTabStripStyle.OneNote)
				{
					num11 += Dpi.Width(ᔠ.Width);
				}
			}
			else
			{
				num11 = ᔜ.Width;
			}
			if (ᓡ && num2 + num11 > num && num3 > 1)
			{
				flag = true;
				switch (ᓹ)
				{
				case eTabStripAlignment.Left:
				case eTabStripAlignment.Right:
					if (arrayList.Count == 0)
					{
						arrayList.Add(num6);
					}
					num6 += num5 + ᔘ;
					num7 = num9;
					arrayList.Add(num6);
					break;
				case eTabStripAlignment.Top:
				case eTabStripAlignment.Bottom:
					if (arrayList.Count == 0)
					{
						arrayList.Add(num7);
					}
					num7 += num5 + ᔘ;
					num6 = num8;
					arrayList.Add(num7);
					break;
				}
				ᔥ += num5 + ᔘ;
				num2 = 0;
			}
			if (ᓹ == eTabStripAlignment.Left || ᓹ == eTabStripAlignment.Right)
			{
				item._DisplayRectangle = new Rectangle(num6, num7, num5, num11);
			}
			else
			{
				item._DisplayRectangle = new Rectangle(num6, num7, num11, num5);
			}
			if (ᓱ != null)
			{
				MeasureTabItemEventArgs measureTabItemEventArgs = new MeasureTabItemEventArgs(item, item.DisplayRectangle.Size);
				ᓱ(this, measureTabItemEventArgs);
				item._DisplayRectangle = new Rectangle(item._DisplayRectangle.Location, measureTabItemEventArgs.Size);
				num11 = ((ᓹ != 0 && ᓹ != eTabStripAlignment.Right) ? measureTabItemEventArgs.Size.Width : measureTabItemEventArgs.Size.Height);
			}
			if (ᓹ == eTabStripAlignment.Left || ᓹ == eTabStripAlignment.Right)
			{
				num7 += num11;
			}
			else
			{
				num6 += num11;
			}
			num2 += num11;
		}
		if (flag)
		{
			if (ᓹ == eTabStripAlignment.Top)
			{
				int num12 = num9 - num7;
				foreach (TabItem item2 in ᓺ)
				{
					item2._DisplayRectangle = new Rectangle(item2.DisplayRectangle.X, item2.DisplayRectangle.Y + num12, item2.DisplayRectangle.Width, item2.DisplayRectangle.Height);
				}
				for (int i = 0; i < arrayList.Count; i++)
				{
					arrayList[i] = (int)arrayList[i] + num12;
				}
			}
			else if (ᓹ == eTabStripAlignment.Left)
			{
				int num13 = num8 - num6;
				foreach (TabItem item3 in ᓺ)
				{
					item3._DisplayRectangle = new Rectangle(item3.DisplayRectangle.X + num13, item3.DisplayRectangle.Y, item3.DisplayRectangle.Width, item3.DisplayRectangle.Height);
				}
				for (int j = 0; j < arrayList.Count; j++)
				{
					arrayList[j] = (int)arrayList[j] + num13;
				}
			}
			if (_ᔦ)
			{
				switch (ᓹ)
				{
				case eTabStripAlignment.Left:
				{
					if (ᓻ.DisplayRectangle.X == (int)arrayList[arrayList.Count - 1])
					{
						break;
					}
					int x2 = ᓻ.DisplayRectangle.X;
					int num17 = (int)arrayList[arrayList.Count - 1];
					foreach (TabItem item4 in ᓺ)
					{
						if (item4.DisplayRectangle.X == num17)
						{
							item4._DisplayRectangle = new Rectangle(x2, item4.DisplayRectangle.Y, item4.DisplayRectangle.Width, item4.DisplayRectangle.Height);
						}
						else if (item4.DisplayRectangle.X == x2)
						{
							item4._DisplayRectangle = new Rectangle(num17, item4.DisplayRectangle.Y, item4.DisplayRectangle.Width, item4.DisplayRectangle.Height);
						}
					}
					break;
				}
				case eTabStripAlignment.Right:
				{
					if (ᓻ.DisplayRectangle.X == (int)arrayList[0])
					{
						break;
					}
					int x = ᓻ.DisplayRectangle.X;
					int num15 = (int)arrayList[0];
					foreach (TabItem item5 in ᓺ)
					{
						if (item5.DisplayRectangle.X == num15)
						{
							item5._DisplayRectangle = new Rectangle(x, item5.DisplayRectangle.Y, item5.DisplayRectangle.Width, item5.DisplayRectangle.Height);
						}
						else if (item5.DisplayRectangle.X == x)
						{
							item5._DisplayRectangle = new Rectangle(num15, item5.DisplayRectangle.Y, item5.DisplayRectangle.Width, item5.DisplayRectangle.Height);
						}
					}
					break;
				}
				case eTabStripAlignment.Top:
				{
					if (ᓻ.DisplayRectangle.Y == (int)arrayList[arrayList.Count - 1])
					{
						break;
					}
					int y2 = ᓻ.DisplayRectangle.Y;
					int num16 = (int)arrayList[arrayList.Count - 1];
					foreach (TabItem item6 in ᓺ)
					{
						if (item6.DisplayRectangle.Y == num16)
						{
							item6._DisplayRectangle = new Rectangle(item6.DisplayRectangle.X, y2, item6.DisplayRectangle.Width, item6.DisplayRectangle.Height);
						}
						else if (item6.DisplayRectangle.Y == y2)
						{
							item6._DisplayRectangle = new Rectangle(item6.DisplayRectangle.X, num16, item6.DisplayRectangle.Width, item6.DisplayRectangle.Height);
						}
					}
					break;
				}
				case eTabStripAlignment.Bottom:
				{
					if (ᓻ.DisplayRectangle.Y == (int)arrayList[0])
					{
						break;
					}
					int y = ᓻ.DisplayRectangle.Y;
					int num14 = (int)arrayList[0];
					foreach (TabItem item7 in ᓺ)
					{
						if (item7.DisplayRectangle.Y == num14)
						{
							item7._DisplayRectangle = new Rectangle(item7.DisplayRectangle.X, y, item7.DisplayRectangle.Width, item7.DisplayRectangle.Height);
						}
						else if (item7.DisplayRectangle.Y == y)
						{
							item7._DisplayRectangle = new Rectangle(item7.DisplayRectangle.X, num14, item7.DisplayRectangle.Width, item7.DisplayRectangle.Height);
						}
					}
					break;
				}
				}
			}
		}
		if (ᓠ)
		{
			if (ᔁ != 0)
			{
				ᔂ.BackEnabled = true;
			}
			else
			{
				ᔂ.BackEnabled = false;
			}
			if (num2 - ᔁ > num)
			{
				ᔂ.ForwardEnabled = true;
			}
			else
			{
				ᔂ.ForwardEnabled = false;
			}
			if (ᔖ && !ᓡ)
			{
				if (num2 > num)
				{
					ᔂ.Visible = true;
				}
				else
				{
					ᔂ.Visible = false;
				}
			}
			else
			{
				ᔂ.Visible = true;
			}
			Rectangle displayRectangle = Rectangle.Empty;
			switch (ᓹ)
			{
			case eTabStripAlignment.Left:
				displayRectangle = new Rectangle((num4 < 0) ? ᕆ.X : ᓺ[num4].DisplayRectangle.X, ((Control)this).ClientRectangle.Bottom - num10 - 3, (num4 < 0) ? ᕆ.Width : ᓺ[num4].DisplayRectangle.Width, num10);
				if (Ӷ == eTabStripStyle.VS2005 && !ᓨ)
				{
					displayRectangle.X--;
				}
				break;
			case eTabStripAlignment.Right:
				displayRectangle = new Rectangle(ᕆ.X, ((Control)this).ClientRectangle.Bottom - num10 - 3, (num4 < 0) ? ᕆ.Width : ᓺ[num4].DisplayRectangle.Width, num10);
				if (Ӷ == eTabStripStyle.VS2005 && !ᓨ)
				{
					displayRectangle.X++;
				}
				break;
			case eTabStripAlignment.Top:
				displayRectangle = ((!ᓛ) ? new Rectangle(((Control)this).ClientRectangle.Right - num10 - 3, (num4 < 0) ? ᕆ.Y : ᓺ[num4].DisplayRectangle.Y, num10, (num4 < 0) ? ᕆ.Height : ᓺ[num4].DisplayRectangle.Height) : new Rectangle(((Control)this).ClientRectangle.Left, (num4 < 0) ? ᕆ.Y : ᓺ[num4].DisplayRectangle.Y, num10, (num4 < 0) ? ᕆ.Height : ᓺ[num4].DisplayRectangle.Height));
				if (Ӷ == eTabStripStyle.VS2005 && !ᓨ)
				{
					displayRectangle.Y--;
				}
				break;
			default:
				displayRectangle = ((!ᓛ) ? new Rectangle(((Control)this).ClientRectangle.Right - num10 - 2, ᕆ.Y, num10 + 2, (num4 < 0) ? ᕆ.Height : ᓺ[num4].DisplayRectangle.Height) : new Rectangle(((Control)this).ClientRectangle.Left, ᕆ.Y, num10 + 2, (num4 < 0) ? ᕆ.Height : ᓺ[num4].DisplayRectangle.Height));
				if (Ӷ == eTabStripStyle.VS2005 && !ᓨ)
				{
					displayRectangle.Y++;
				}
				break;
			}
			ᔂ.DisplayRectangle = displayRectangle;
		}
		if (num2 > num && ᔙ == eTabLayoutType.FitContainer && !ᓡ)
		{
			if (num3 <= 0)
			{
				return;
			}
			if (ᓹ == eTabStripAlignment.Left || ᓹ == eTabStripAlignment.Right)
			{
				num6 = ᕆ.X;
				num7 = ᕆ.Y;
				if (ᓹ == eTabStripAlignment.Left)
				{
					num6 = ᕆ.Right - num5;
				}
				int num18 = num / num3;
				int num19 = 0;
				foreach (TabItem item8 in ᓺ)
				{
					if (item8.Visible)
					{
						if (item8.DisplayRectangle.Height > num18)
						{
							num19++;
							continue;
						}
						num -= item8.DisplayRectangle.Height;
						num2 -= item8.DisplayRectangle.Height;
					}
				}
				float num20 = (float)num / (float)num2;
				{
					foreach (TabItem item9 in ᓺ)
					{
						if (!item9.Visible)
						{
							continue;
						}
						if (item9.DisplayRectangle.Height > num18)
						{
							if (num19 == num3)
							{
								item9._DisplayRectangle = new Rectangle(num6, num7, item9.DisplayRectangle.Width, num18);
							}
							else
							{
								item9._DisplayRectangle = new Rectangle(num6, num7, item9.DisplayRectangle.Width, (int)((float)item9.DisplayRectangle.Height * num20));
							}
						}
						else
						{
							item9._DisplayRectangle = new Rectangle(num6, num7, item9.DisplayRectangle.Width, item9.DisplayRectangle.Height);
						}
						num7 += item9.DisplayRectangle.Height;
					}
					return;
				}
			}
			num6 = ᕆ.X;
			int num21 = num / num3;
			int num22 = 0;
			foreach (TabItem item10 in ᓺ)
			{
				if (item10.Visible)
				{
					if (item10.DisplayRectangle.Width > num21)
					{
						num22++;
						continue;
					}
					num -= item10.DisplayRectangle.Width;
					num2 -= item10.DisplayRectangle.Width;
				}
			}
			float num23 = (float)num / (float)num2;
			{
				foreach (TabItem item11 in ᓺ)
				{
					if (!item11.Visible)
					{
						continue;
					}
					if (item11.DisplayRectangle.Width > num21)
					{
						if (num22 == num3)
						{
							item11._DisplayRectangle = new Rectangle(num6, num7, num21, item11.DisplayRectangle.Height);
						}
						else
						{
							item11._DisplayRectangle = new Rectangle(num6, num7, (int)((float)item11.DisplayRectangle.Width * num23), item11.DisplayRectangle.Height);
						}
					}
					else
					{
						item11._DisplayRectangle = new Rectangle(num6, num7, item11.DisplayRectangle.Width, item11.DisplayRectangle.Height);
					}
					num6 += item11.DisplayRectangle.Width;
				}
				return;
			}
		}
		if (!ᓛ || ᔙ == eTabLayoutType.FitContainer || (ᓹ != eTabStripAlignment.Top && ᓹ != eTabStripAlignment.Bottom))
		{
			return;
		}
		if (num2 > num)
		{
			for (int num24 = ᓺ.Count - 1; num24 >= 0; num24--)
			{
				if (ᓺ[num24].Visible)
				{
					num6 = ᓺ[num24].DisplayRectangle.Right;
					break;
				}
			}
			if (ᔂ.Visible)
			{
				num6 += ᔂ.DisplayRectangle.Right;
			}
		}
		else
		{
			num6 = ᕆ.Right;
		}
		foreach (TabItem item12 in ᓺ)
		{
			if (item12.Visible)
			{
				num6 -= item12.DisplayRectangle.Width;
				item12._DisplayRectangle = new Rectangle(num6, item12.DisplayRectangle.Y, item12.DisplayRectangle.Width, item12.DisplayRectangle.Height);
			}
		}
	}

	internal void ᕈ(MeasureTabItemEventArgs ؿ)
	{
		if (ᓱ != null)
		{
			ᓱ(this, ؿ);
		}
	}

	internal void ᕉ(RenderTabItemEventArgs ؿ)
	{
		if (ᓲ != null)
		{
			ᓲ(this, ؿ);
		}
	}

	internal void ᕊ(RenderTabItemEventArgs ؿ)
	{
		if (ᓳ != null)
		{
			ᓳ(this, ؿ);
		}
	}

	private TabItem ᕋ()
	{
		foreach (TabItem item in ᓺ)
		{
			if (item.Visible)
			{
				return item;
			}
		}
		return null;
	}

	internal void ᕌ(Rectangle ᕍ, Rectangle ᕎ)
	{
		if (!ᓠ)
		{
			return;
		}
		Rectangle rectangle = ᕎ;
		if (ᓹ == eTabStripAlignment.Top || ᓹ == eTabStripAlignment.Bottom)
		{
			rectangle.Offset(ᔁ, 0);
		}
		else
		{
			rectangle.Offset(0, ᔁ);
		}
		bool visible = false;
		int num = Dpi.Width(ᔂ.DefaultWidth);
		if ((rectangle.Right > ᕍ.Right - ((!ᔖ) ? num : 0) && (ᓹ == eTabStripAlignment.Top || ᓹ == eTabStripAlignment.Bottom)) || (rectangle.Bottom > ᕍ.Bottom - ((!ᔖ) ? num : 0) && (ᓹ == eTabStripAlignment.Right || ᓹ == eTabStripAlignment.Left)))
		{
			visible = true;
		}
		if (ᔁ != 0)
		{
			ᔂ.BackEnabled = true;
		}
		else
		{
			ᔂ.BackEnabled = false;
		}
		if (ᔖ && !ᓡ)
		{
			ᔂ.Visible = visible;
		}
		else
		{
			ᔂ.Visible = true;
		}
		if ((ᕎ.Right + ᔁ > ᕍ.Right - (ᔂ.Visible ? num : 0) && (ᓹ == eTabStripAlignment.Top || ᓹ == eTabStripAlignment.Bottom)) || (ᕎ.Bottom + ᔁ > ᕍ.Bottom - (ᔂ.Visible ? num : 0) && (ᓹ == eTabStripAlignment.Right || ᓹ == eTabStripAlignment.Left)))
		{
			ᔂ.ForwardEnabled = true;
		}
		else
		{
			ᔂ.ForwardEnabled = false;
		}
		TabItem tabItem = ᕋ();
		Rectangle displayRectangle = Rectangle.Empty;
		switch (ᓹ)
		{
		case eTabStripAlignment.Left:
			displayRectangle = new Rectangle(tabItem?.DisplayRectangle.X ?? ᕍ.X, ((Control)this).ClientRectangle.Bottom - num - 3, tabItem?.DisplayRectangle.Width ?? ᕍ.Width, num);
			if (Ӷ == eTabStripStyle.VS2005 && !ᓨ)
			{
				displayRectangle.X--;
			}
			break;
		case eTabStripAlignment.Right:
			displayRectangle = new Rectangle(ᕍ.X, ((Control)this).ClientRectangle.Bottom - num - 3, tabItem?.DisplayRectangle.Width ?? ᕍ.Width, num);
			if (Ӷ == eTabStripStyle.VS2005 && !ᓨ)
			{
				displayRectangle.X++;
			}
			break;
		case eTabStripAlignment.Top:
			displayRectangle = ((!ᓛ) ? new Rectangle(((Control)this).ClientRectangle.Right - num - 3, tabItem?.DisplayRectangle.Y ?? ᕍ.Y, num, tabItem?.DisplayRectangle.Height ?? ᕍ.Height) : new Rectangle(ᕍ.X, tabItem?.DisplayRectangle.Y ?? ᕍ.Y, num, tabItem?.DisplayRectangle.Height ?? ᕍ.Height));
			if (Ӷ == eTabStripStyle.VS2005 && !ᓨ)
			{
				displayRectangle.Y--;
			}
			break;
		default:
			displayRectangle = ((!ᓛ) ? new Rectangle(((Control)this).ClientRectangle.Right - num - 2, ᕍ.Y, num + 2, tabItem?.DisplayRectangle.Height ?? ᕍ.Height) : new Rectangle(ᕍ.X, ᕍ.Y, num + 2, tabItem?.DisplayRectangle.Height ?? ᕍ.Height));
			if (Ӷ == eTabStripStyle.VS2005 && !ᓨ)
			{
				displayRectangle.Y++;
			}
			break;
		}
		ᔂ.DisplayRectangle = displayRectangle;
	}

	private void ᕏ()
	{
		if (this.ᓗ != null)
		{
			this.ᓗ(this, new EventArgs());
		}
		if (ᓡ && !(((Control)this).Parent is TabControl))
		{
			switch (TabAlignment)
			{
			case eTabStripAlignment.Left:
			case eTabStripAlignment.Right:
				((Control)this).Width = MinTabStripHeight;
				break;
			default:
				((Control)this).Height = MinTabStripHeight;
				break;
			}
		}
	}

	private void చ()
	{
		if (ᑬ != null)
		{
			ᑬ.Dispose();
		}
		ᑬ = new ᑧ((Control)(object)this);
	}

	private void _0A78()
	{
		if (ᑬ != null)
		{
			ᑬ.Dispose();
			ᑬ = null;
		}
	}

	protected override void WndProc(ref Message m)
	{
		if (((Message)(ref m)).Msg == 794)
		{
			_0EEE._0EF6();
			చ();
		}
		((Control)this).WndProc(ref m);
	}

	protected override void OnHandleDestroyed(EventArgs e)
	{
		_0A78();
		((Control)this).OnHandleDestroyed(e);
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		((Control)this).OnHandleCreated(e);
		if (ᓼ)
		{
			RecalcSize();
			if (((Control)this).Parent is TabControl)
			{
				((TabControl)(object)((Control)this).Parent).SyncTabStripSize();
			}
		}
		ᕨ();
	}

	internal void ᕐ(int ԡ, TabItem _06EC)
	{
		if (ԡ >= Tabs.Count)
		{
			ԡ = Tabs.Count - 1;
		}
		int num = -1;
		for (int i = ԡ; i < Tabs.Count; i++)
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
			ᓻ = null;
		}
	}

	internal void ᕑ(TabItem ᆰ)
	{
		if (!ᔧ)
		{
			HideToolTip();
			if (ᓯ != null)
			{
				ᓯ(ᆰ, new EventArgs());
			}
			if (ᓥ == 0)
			{
				SelectedTab = null;
			}
		}
	}

	private void ᕒ(TabItem ԝ, eEventSource ಥ)
	{
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Invalid comparison between Unknown and I4
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Invalid comparison between Unknown and I4
		if (ᓻ == ԝ)
		{
			return;
		}
		TabItem oldtab = ᓻ;
		if (ᓩ != null)
		{
			TabStripTabChangingEventArgs tabStripTabChangingEventArgs = new TabStripTabChangingEventArgs(ᓻ, ԝ, ಥ);
			ᓩ(this, tabStripTabChangingEventArgs);
			if (tabStripTabChangingEventArgs.Cancel)
			{
				return;
			}
		}
		ᕙ(ԝ);
		Control val = null;
		if (ᓻ != null && ᓻ.AttachedItem != null)
		{
			ᓻ.AttachedItem.Displayed = false;
		}
		else if (ᓻ != null && ᓻ.AttachedControl != null && !ᔄ)
		{
			val = ᓻ.AttachedControl;
		}
		ᓻ = ԝ;
		ᓼ = true;
		EnsureVisible(ᓻ);
		if (ᓻ != null && ᓻ.AttachedItem != null)
		{
			ᓻ.AttachedItem.Displayed = true;
			if (ᓻ.AttachedItem is DockContainerItem && ((DockContainerItem)ᓻ.AttachedItem).Control != null && ᔤ)
			{
				((DockContainerItem)ᓻ.AttachedItem).Control.Select();
			}
		}
		else if (ᓻ != null && ᓻ.AttachedControl != null)
		{
			if (!ᔄ)
			{
				ᓻ.AttachedControl.Visible = true;
				if (ᔤ && (!((Control)this).TabStop || !(((Control)this).Parent is TabControl) || !((Control)this).Focused || ಥ != 0))
				{
					ᓻ.AttachedControl.Select();
				}
				if (val == ᓻ.AttachedControl)
				{
					val = null;
				}
			}
			else if (ᓻ.AttachedControl is Form)
			{
				MdiClient val2 = ᕬ(ᔅ);
				bool flag = false;
				Form activeMdiChild = ᔅ.ActiveMdiChild;
				if (val2 != null && ᔊ && ᓻ.AttachedControl is Form && ((int)((Form)ᓻ.AttachedControl).WindowState == 2 || (activeMdiChild != null && (int)activeMdiChild.WindowState == 2)))
				{
					_089F.SendMessage(((Control)val2).Handle, 11, 0, 0);
					flag = true;
				}
				((Form)ᓻ.AttachedControl).Activate();
				if (flag)
				{
					_089F.SendMessage(((Control)val2).Handle, 11, 1, 0);
					((Control)val2).Refresh();
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
		if (Ꮷ != null)
		{
			TabStripTabChangedEventArgs e = new TabStripTabChangedEventArgs(oldtab, ᓻ, ಥ);
			Ꮷ(this, e);
		}
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Invalid comparison between Unknown and I4
		if (_ᅸ)
		{
			if ((int)keyData == 37)
			{
				if (_౯(eEventSource.Keyboard, _0C70: false))
				{
					return true;
				}
			}
			else if ((int)keyData == 39 && _0C71(eEventSource.Keyboard, _0C70: false))
			{
				return true;
			}
		}
		return ((Control)this).ProcessCmdKey(ref msg, keyData);
	}

	public bool SelectPreviousTab()
	{
		return _౯(eEventSource.Code, _0C70: false);
	}

	internal bool _౯(eEventSource ಥ, bool _0C70)
	{
		if (SelectedTab != null && Tabs.IndexOf(SelectedTab) > 0)
		{
			for (int num = Tabs.IndexOf(SelectedTab) - 1; num >= 0; num--)
			{
				if (Tabs[num].Visible)
				{
					ᕒ(Tabs[num], ಥ);
					return true;
				}
			}
		}
		if (_0C70)
		{
			for (int num2 = Tabs.Count - 1; num2 >= 0; num2--)
			{
				if (Tabs[num2].Visible)
				{
					ᕒ(Tabs[num2], ಥ);
					return true;
				}
			}
		}
		return false;
	}

	public bool SelectNextTab()
	{
		return _0C71(eEventSource.Code, _0C70: false);
	}

	internal bool _0C71(eEventSource ಥ, bool _0C70)
	{
		if (SelectedTab != null && Tabs.IndexOf(SelectedTab) < Tabs.Count)
		{
			for (int i = Tabs.IndexOf(SelectedTab) + 1; i < Tabs.Count; i++)
			{
				if (Tabs[i].Visible)
				{
					ᕒ(Tabs[i], ಥ);
					return true;
				}
			}
		}
		if (_0C70)
		{
			for (int j = 0; j < Tabs.Count; j++)
			{
				if (Tabs[j].Visible)
				{
					ᕒ(Tabs[j], ಥ);
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
		if (!ᓠ || !ᔂ.Visible || !ᔂ.DisplayRectangle.Contains(point.X, point.Y))
		{
			HitTest(point.X, point.Y)?.InvokeClick(e);
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
		//IL_0387: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_034d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_0358: Invalid comparison between Unknown and I4
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_0366: Invalid comparison between Unknown and I4
		HideToolTip();
		if (ᓠ && ᔂ.Visible && ᔂ.DisplayRectangle.Contains(e.X, e.Y))
		{
			ᔂ.MouseDown(e);
			return;
		}
		_ᐨ = new Point(e.X, e.Y);
		if ((int)e.Button == 1048576)
		{
			Rectangle rectangle = ᕔ();
			foreach (TabItem item in ᓺ)
			{
				if ((!item.DisplayRectangle.Contains(new Point(e.X, e.Y)) && !rectangle.Contains(e.X, e.Y)) || !item.Visible)
				{
					continue;
				}
				if (!item.CloseButtonBounds.IsEmpty && item.CloseButtonBounds.Contains(e.X, e.Y) && (ᔙ == eTabLayoutType.MultilineNoNavigationBox || ᔙ == eTabLayoutType.MultilineWithNavigationBox))
				{
					return;
				}
				TabItem tabItem2 = item;
				if (rectangle.Contains(e.X, e.Y))
				{
					tabItem2 = ᓻ;
				}
				if (ᓻ != tabItem2)
				{
					TabItem oldtab = ᓻ;
					if (ᓩ != null)
					{
						TabStripTabChangingEventArgs tabStripTabChangingEventArgs = new TabStripTabChangingEventArgs(ᓻ, tabItem2, eEventSource.Mouse);
						ᓩ(this, tabStripTabChangingEventArgs);
						if (tabStripTabChangingEventArgs.Cancel)
						{
							break;
						}
					}
					ᕙ(tabItem2);
					if (ᓻ.AttachedItem is DockContainerItem && ((DockContainerItem)ᓻ.AttachedItem).Control != null && ((DockContainerItem)ᓻ.AttachedItem).Control.Focused)
					{
						((Control)this).Focus();
					}
					Control val = null;
					if (ᓻ.AttachedItem != null)
					{
						ᓻ.AttachedItem.Displayed = false;
					}
					else if (ᓻ.AttachedControl != null && !ᔄ)
					{
						val = ᓻ.AttachedControl;
					}
					ᓻ = tabItem2;
					ᓼ = true;
					EnsureVisible(ᓻ);
					if (ᓻ.AttachedItem != null)
					{
						ᓻ.AttachedItem.Displayed = true;
						if (ᓻ.AttachedItem is DockContainerItem && ((DockContainerItem)ᓻ.AttachedItem).Control != null && ᔤ)
						{
							((DockContainerItem)ᓻ.AttachedItem).Control.Select();
						}
					}
					else if (ᓻ.AttachedControl != null)
					{
						if (!ᔄ)
						{
							ᓻ.AttachedControl.Visible = true;
							if (ᔤ)
							{
								ᓻ.AttachedControl.Select();
							}
						}
						else if (ᓻ.AttachedControl is Form)
						{
							MdiClient val2 = ᕬ(ᔅ);
							bool flag = false;
							Form activeMdiChild = ᔅ.ActiveMdiChild;
							if (val2 != null && ᔊ && ᓻ.AttachedControl is Form && ((int)((Form)ᓻ.AttachedControl).WindowState == 2 || (activeMdiChild != null && (int)activeMdiChild.WindowState == 2)))
							{
								_089F.SendMessage(((Control)val2).Handle, 11, 0, 0);
								flag = true;
							}
							((Form)ᓻ.AttachedControl).Activate();
							if (flag)
							{
								_089F.SendMessage(((Control)val2).Handle, 11, 1, 0);
								((Control)val2).Refresh();
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
					if (Ꮷ != null)
					{
						TabStripTabChangedEventArgs e2 = new TabStripTabChangedEventArgs(oldtab, ᓻ, eEventSource.Mouse);
						Ꮷ(this, e2);
					}
				}
				else if (ᓻ != null && ᓻ.AttachedItem is DockContainerItem && ᓻ.AttachedItem.ContainerControl is Bar && ((Bar)ᓻ.AttachedItem.ContainerControl).DockSide == eDockSide.Document && ((DockContainerItem)ᓻ.AttachedItem).Control != null && ᔤ)
				{
					((DockContainerItem)ᓻ.AttachedItem).Control.Select();
				}
				break;
			}
		}
		HitTest(e.X, e.Y)?.InvokeMouseDown(e);
	}

	private void ᕓ()
	{
		if (ᔎ == null)
		{
			return;
		}
		if (Ӷ == eTabStripStyle.OneNote || Ӷ == eTabStripStyle.VS2005Document)
		{
			Rectangle displayRectangle = ᔎ.DisplayRectangle;
			if (ᓹ == eTabStripAlignment.Top || ᓹ == eTabStripAlignment.Bottom)
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
		else if (Ӷ == eTabStripStyle.Office2007Document)
		{
			Rectangle displayRectangle2 = ᔎ.DisplayRectangle;
			if (ᓹ == eTabStripAlignment.Top || ᓹ == eTabStripAlignment.Bottom)
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
		else if (Ӷ == eTabStripStyle.VS2005Dock)
		{
			Rectangle displayRectangle3 = ᔎ.DisplayRectangle;
			displayRectangle3.Inflate(6, 0);
			((Control)this).Invalidate(displayRectangle3);
		}
		else
		{
			Rectangle displayRectangle4 = ᔎ.DisplayRectangle;
			displayRectangle4.Inflate(2, 2);
			((Control)this).Invalidate(displayRectangle4);
		}
	}

	private Rectangle ᕔ()
	{
		if (SelectedTab == null)
		{
			return Rectangle.Empty;
		}
		Rectangle displayRectangle = SelectedTab.DisplayRectangle;
		if (Ӷ == eTabStripStyle.OneNote || Ӷ == eTabStripStyle.VS2005Document)
		{
			switch (ᓹ)
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
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Invalid comparison between Unknown and I4
		//IL_0487: Unknown result type (might be due to invalid IL or missing references)
		if (ᔚ != null && !ᔚ.DisplayRectangle.Contains(e.X, e.Y))
		{
			HideToolTip();
			ఘ();
		}
		if (ᔎ != null)
		{
			if (ᔎ.DisplayRectangle.Contains(e.X, e.Y))
			{
				ᔎ.InvokeMouseMove(e);
			}
			else
			{
				ᔎ.InvokeMouseLeave((EventArgs)(object)e);
				TabItem tabItem = HitTest(e.X, e.Y);
				if (tabItem != null)
				{
					tabItem.InvokeMouseEnter((EventArgs)(object)e);
					ఘ();
				}
			}
		}
		else
		{
			TabItem tabItem2 = HitTest(e.X, e.Y);
			if (tabItem2 != null)
			{
				tabItem2.InvokeMouseEnter((EventArgs)(object)e);
				ఘ();
			}
		}
		if (ᓠ && !ᓢ)
		{
			ᔂ.MouseMove(e);
			if (ᔂ.Visible && ᔂ.DisplayRectangle.Contains(e.X, e.Y))
			{
				if (ᔎ != null)
				{
					if (ᓺ.Contains(ᔎ))
					{
						ᕓ();
						ᔎ = null;
						((Control)this).Update();
					}
					else
					{
						ᔎ = null;
					}
				}
				return;
			}
		}
		if (ᓾ != null)
		{
			if (!((Control)ᓾ).IsDisposed)
			{
				ᓾ._0BFB();
			}
		}
		else
		{
			if (ᓿ)
			{
				return;
			}
			if ((int)e.Button == 1048576)
			{
				if (ᔎ != null && ᔎ.CloseButtonMouseOver)
				{
					ᔎ.CloseButtonMouseOver = false;
					ᕓ();
				}
				if (ᓢ)
				{
					if (((Control)this).DisplayRectangle.Contains(e.X, e.Y))
					{
						if (!_0B79 && !ఛ())
						{
							return;
						}
						{
							foreach (TabItem item in ᓺ)
							{
								Rectangle displayRectangle = item.DisplayRectangle;
								if (e.X < displayRectangle.Right && (ᓹ == eTabStripAlignment.Bottom || ᓹ == eTabStripAlignment.Top))
								{
									displayRectangle.Width = ᓻ.DisplayRectangle.Width;
								}
								else if (e.Y < displayRectangle.Bottom && (ᓹ == eTabStripAlignment.Left || ᓹ == eTabStripAlignment.Right))
								{
									displayRectangle.Height = ᓻ.DisplayRectangle.Height;
								}
								if (!displayRectangle.Contains(e.X, e.Y))
								{
									continue;
								}
								if (ᓻ == item)
								{
									break;
								}
								int num = ᓺ.IndexOf(item);
								if (ᓪ != null)
								{
									TabStripTabMovedEventArgs tabStripTabMovedEventArgs = new TabStripTabMovedEventArgs(ᓻ, ᓺ.IndexOf(ᓻ), num);
									ᓪ(this, tabStripTabMovedEventArgs);
									if (tabStripTabMovedEventArgs.Cancel)
									{
										break;
									}
								}
								TabItem tabItem4 = ᓻ;
								ᔧ = true;
								ᓺ._ᳩ(tabItem4);
								ᓺ.__1CE8(num, tabItem4);
								SelectedTab = tabItem4;
								ᔧ = false;
								((Control)this).Refresh();
								break;
							}
							return;
						}
					}
					Rectangle displayRectangle2 = ((Control)this).DisplayRectangle;
					displayRectangle2.Inflate(16, 16);
					if (displayRectangle2.Contains(e.X, e.Y) || !DotNetBarManager.MouseDockingEnabled)
					{
						return;
					}
					int num2 = ᓥ;
					if (((Control)this).Parent is Bar && ((Bar)(object)((Control)this).Parent).CanTearOffTabs && (num2 > 1 || (num2 == 1 && ((Bar)(object)((Control)this).Parent).GrabHandleStyle == eGrabHandleStyle.None)))
					{
						ᓢ = false;
						ᓿ = true;
						ᓾ = ((Bar)(object)((Control)this).Parent)._0C0D();
						if (ᓾ != null)
						{
							((Control)this).Capture = true;
							((Control)this).Refresh();
						}
						else
						{
							ᓿ = false;
						}
					}
				}
				else if ((Math.Abs(e.X - _ᐨ.X) >= 4 || Math.Abs(e.Y - _ᐨ.Y) >= 4) && ᓺ.Count > 0 && !ᓡ && CanReorderTabs)
				{
					ᓢ = true;
				}
			}
			else
			{
				if ((int)e.Button != 0)
				{
					return;
				}
				if (ᕔ().Contains(e.X, e.Y))
				{
					ᕖ(SelectedTab, e.X, e.Y);
					return;
				}
				foreach (TabItem item2 in ᓺ)
				{
					if (item2.DisplayRectangle.Contains(e.X, e.Y))
					{
						ᕖ(item2, e.X, e.Y);
						return;
					}
				}
				ᕖ(null, e.X, e.Y);
			}
		}
	}

	private Cursor ᕕ()
	{
		if (_ᔨ != (Cursor)null)
		{
			return _ᔨ;
		}
		return Cursors.SizeAll;
	}

	private bool ఛ()
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

	private void ᕖ(TabItem ᆰ, int و, int ى)
	{
		if (ᔎ != ᆰ)
		{
			if (ᓺ.Contains(ᔎ))
			{
				ᕓ();
			}
			if (ᔎ != null)
			{
				ᔎ.CloseButtonMouseOver = false;
			}
			ᔎ = ᆰ;
			if (ᔎ != null)
			{
				if (!ᔎ.CloseButtonBounds.IsEmpty && ᔎ.CloseButtonBounds.Contains(و, ى))
				{
					ᔎ.CloseButtonMouseOver = true;
				}
				ᕓ();
			}
			((Control)this).Update();
			ఘ();
		}
		else
		{
			if (ᔎ == null || ᔎ.CloseButtonBounds.IsEmpty)
			{
				return;
			}
			if (ᔎ.CloseButtonBounds.Contains(و, ى))
			{
				if (!ᔎ.CloseButtonMouseOver)
				{
					ᔎ.CloseButtonMouseOver = true;
					ᕓ();
					((Control)this).Update();
				}
			}
			else if (ᔎ.CloseButtonMouseOver)
			{
				ᔎ.CloseButtonMouseOver = false;
				ᕓ();
				((Control)this).Update();
			}
		}
	}

	protected override void OnMouseWheel(MouseEventArgs e)
	{
		((Control)this).OnMouseWheel(e);
		ᔂ.MouseWheel(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		((Control)this).OnMouseUp(e);
		InternalOnMouseUp(e);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void InternalOnMouseUp(MouseEventArgs e)
	{
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Invalid comparison between Unknown and I4
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Invalid comparison between Unknown and I4
		if (ᓠ)
		{
			ᔂ.MouseUp(e);
			if (ᔂ.Visible && ᔂ.DisplayRectangle.Contains(e.X, e.Y))
			{
				return;
			}
		}
		Point ᐨ = _ᐨ;
		_ᐨ = Point.Empty;
		if (ᓿ)
		{
			((Control)this).Capture = false;
		}
		if (ᓾ != null)
		{
			if (!((Control)ᓾ).IsDisposed)
			{
				ᓾ._0BFC();
			}
			ᓾ = null;
		}
		ᓿ = false;
		ᓢ = false;
		TabItem tabItem = HitTest(e.X, e.Y);
		tabItem?.InvokeMouseUp(e);
		if (SelectedTab != null && SelectedTab == tabItem && !SelectedTab.CloseButtonBounds.IsEmpty && (int)e.Button == 1048576)
		{
			Rectangle closeButtonBounds = SelectedTab.CloseButtonBounds;
			closeButtonBounds.Width += 2;
			if (closeButtonBounds.Contains(e.X, e.Y) && closeButtonBounds.Contains(ᐨ))
			{
				ᕡ();
			}
		}
		else if ((int)e.Button == 1048576 && tabItem != null && !tabItem.CloseButtonBounds.IsEmpty && (ᔙ == eTabLayoutType.MultilineNoNavigationBox || ᔙ == eTabLayoutType.MultilineWithNavigationBox) && tabItem.CloseButtonBounds.Contains(e.X, e.Y) && tabItem.CloseButtonBounds.Contains(ᐨ))
		{
			ᎁ(tabItem);
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
		if (ᓠ)
		{
			ᔂ.MouseLeave();
		}
		if (ᔎ != null)
		{
			ᔎ.InvokeMouseLeave(e);
			if (ᓺ.Contains(ᔎ))
			{
				ᔎ.CloseButtonMouseOver = false;
				ᕓ();
				ᔎ = null;
				((Control)this).Update();
			}
			else
			{
				ᔎ = null;
			}
		}
	}

	protected override void OnMouseClick(MouseEventArgs e)
	{
		if (ᔎ != null)
		{
			OnTabMouseClick(ᔎ, e);
		}
		((Control)this).OnMouseClick(e);
	}

	protected override void OnMouseHover(EventArgs e)
	{
		((Control)this).OnMouseHover(e);
		if (ᓠ)
		{
			ᔂ.MouseHover();
		}
		ఆ();
	}

	internal void ᕗ()
	{
		HideToolTip();
		OnTabsCleared(new EventArgs());
		if (ᓻ != null)
		{
			if (Ꮷ != null)
			{
				TabStripTabChangedEventArgs e = new TabStripTabChangedEventArgs(ᓻ, null, eEventSource.Code);
				Ꮷ(this, e);
			}
			ᓻ = null;
		}
	}

	protected virtual void OnTabsCleared(EventArgs e)
	{
		if (ᓰ != null)
		{
			ᓰ(this, e);
		}
	}

	public void EnsureVisible(TabItem tab)
	{
		if (tab == null || !ᓺ.Contains(tab) || ᔙ == eTabLayoutType.FitContainer || ᓡ || !BarFunctions.IsHandleValid((Control)(object)this))
		{
			return;
		}
		if (tab.DisplayRectangle.IsEmpty)
		{
			((Control)this).Invalidate();
			RecalcSize();
		}
		Rectangle displayRectangle = tab.DisplayRectangle;
		if (Ӷ == eTabStripStyle.OneNote || Ӷ == eTabStripStyle.VS2005Document || Ӷ == eTabStripStyle.Office2007Document)
		{
			if (ᓹ == eTabStripAlignment.Top || ᓹ == eTabStripAlignment.Bottom)
			{
				if (Ӷ != eTabStripStyle.Office2007Document)
				{
					displayRectangle.X -= ᓘ - 6;
				}
				displayRectangle.Width += ᓘ - 6;
			}
			else if (ᓹ == eTabStripAlignment.Right)
			{
				if (Ӷ != eTabStripStyle.Office2007Document)
				{
					displayRectangle.Y -= ᓘ - 6;
				}
				displayRectangle.Height += ᓘ - 6;
			}
			else if (ᓹ == eTabStripAlignment.Left)
			{
				displayRectangle.Height += ᓘ - 6;
			}
		}
		int scrollOffset = ScrollOffset;
		if (ᓹ == eTabStripAlignment.Top || ᓹ == eTabStripAlignment.Bottom)
		{
			if (displayRectangle.X < 0 || (ᓛ && displayRectangle.X < ᔂ.DisplayRectangle.Right && ᔂ.Visible))
			{
				scrollOffset += displayRectangle.X;
				if (ᓛ && (Ӷ == eTabStripStyle.OneNote || Ӷ == eTabStripStyle.VS2005Document || Ӷ == eTabStripStyle.Office2007Document))
				{
					scrollOffset -= displayRectangle.Height;
				}
				if (scrollOffset < 0 || (ᓛ && scrollOffset < ᔂ.DisplayRectangle.Right))
				{
					scrollOffset = 0;
				}
				ScrollOffset = scrollOffset;
			}
			else if (!ᓛ && displayRectangle.Right > ᔂ.DisplayRectangle.Left && ᔂ.Visible && !ᔂ.DisplayRectangle.IsEmpty)
			{
				scrollOffset = scrollOffset + displayRectangle.Right - ᔂ.DisplayRectangle.Left + 2;
				ScrollOffset = scrollOffset;
			}
			else if (ᓛ && displayRectangle.Right > ((Control)this).DisplayRectangle.Right)
			{
				scrollOffset = scrollOffset + displayRectangle.Right - ((Control)this).DisplayRectangle.Right + 2;
				ScrollOffset = scrollOffset;
			}
			else if (tab.IsSelected && ᕘ(tab) && scrollOffset > 0 && !ᔛ.IsEmpty)
			{
				scrollOffset = ᔛ.Width - (((Control)this).ClientRectangle.Width - ᔂ.DisplayRectangle.Width - ᓘ);
				if (scrollOffset < 0 || (ᓛ && scrollOffset < ᔂ.DisplayRectangle.Right))
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
		else if (displayRectangle.Bottom > ᔂ.DisplayRectangle.Top && ᔂ.Visible && !ᔂ.DisplayRectangle.IsEmpty)
		{
			scrollOffset = scrollOffset + displayRectangle.Bottom - ᔂ.DisplayRectangle.Top + 4;
			ScrollOffset = scrollOffset;
		}
	}

	private bool ᕘ(TabItem ᆰ)
	{
		int num = Tabs.Count - 1;
		for (int num2 = num; num2 >= 0; num2--)
		{
			if (Tabs[num2].Visible)
			{
				if (Tabs[num2] != ᆰ)
				{
					break;
				}
				return true;
			}
		}
		return false;
	}

	internal void ᕙ(TabItem _06EC)
	{
		if (!_ᔩ && _0C55 != null)
		{
			_0C55(_06EC, new EventArgs());
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
			BarUtilities.ᆟ(ref ᔡ);
			BarUtilities.ᆟ(ref ᔢ);
		}
		((Control)this).Dispose(disposing);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void StyleManagerStyleChanged(eDotNetBarStyle newStyle)
	{
		ᕚ();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSelectedTabFont()
	{
		ᔋ = null;
	}

	public void RefreshColorScheme()
	{
		լ.Style = Ӷ;
		լ._1CFB = ᓨ;
		if (((Control)this).Parent is Bar && ((Bar)(object)((Control)this).Parent).DockSide != eDockSide.Document)
		{
			լ.ᳺ = ᵋ.ᵌ;
		}
		else
		{
			լ.ᳺ = ᵋ.ሬ;
		}
		լ.Refresh();
	}

	private void ᕚ()
	{
		ᓼ = true;
		RefreshColorScheme();
		ᔁ = 0;
		ᔥ = 0;
		if ((Ӷ == eTabStripStyle.OneNote || Ӷ == eTabStripStyle.VS2005Document || Ӷ == eTabStripStyle.Office2007Document) && !ᓨ && ᔙ == eTabLayoutType.FitContainer)
		{
			ᔙ = eTabLayoutType.FixedWithNavigationBox;
		}
		((Control)this).Invalidate();
		EnsureVisible(SelectedTab);
	}

	protected override void OnSystemColorsChanged(EventArgs e)
	{
		((Control)this).OnSystemColorsChanged(e);
		Application.DoEvents();
		ᕚ();
	}

	protected override void OnFontChanged(EventArgs e)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		((Control)this).OnFontChanged(e);
		ᔥ = 0;
		ᓼ = true;
		if (Ꮬ)
		{
			if ((Ӷ == eTabStripStyle.OneNote || Ӷ == eTabStripStyle.VS2005Document || Ӷ == eTabStripStyle.Office2007Document) && !ᔌ)
			{
				ᔋ = new Font(((Control)this).Font, (FontStyle)1);
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

	internal void ᕛ(TabItem ᆰ)
	{
		ᓼ = true;
		if (ᆰ == ᔚ)
		{
			HideToolTip();
		}
		if (!ᆰ.Visible && ᓻ == ᆰ)
		{
			int num = Tabs.IndexOf(ᆰ) - 1;
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
		else if (ᓥ == 1 && ᆰ.Visible)
		{
			SelectedTab = ᆰ;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeColorScheme()
	{
		return լ.SchemeChanged;
	}

	public void ResetColorScheme()
	{
		լ.ResetChangedFlag();
		լ.Refresh();
		if (Ꮬ)
		{
			((Control)this).Invalidate();
		}
	}

	private void ᕜ(object _٠, EventArgs ؿ)
	{
		if (Ꮬ || ((Control)this).Parent is TabControl)
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
					ᕒ(tab, eEventSource.Keyboard);
					return true;
				}
			}
		}
		return ((Control)this).ProcessMnemonic(charCode);
	}

	internal TabColors ᕝ(TabItem ᆰ)
	{
		TabColors tabColors = new TabColors();
		if (ᆰ == ᔎ && !ᆰ.IsSelected)
		{
			tabColors.BackColor = լ.TabItemHotBackground;
			tabColors.BackColor2 = լ.TabItemHotBackground2;
			tabColors.BackColorGradientAngle = լ.TabItemHotBackgroundGradientAngle;
			tabColors.TextColor = լ.TabItemHotText;
			tabColors.LightBorderColor = լ.TabItemHotBorderLight;
			tabColors.DarkBorderColor = լ.TabItemHotBorderDark;
			tabColors.BorderColor = լ.TabItemHotBorder;
			tabColors.BackgroundColorBlend.CopyFrom(լ.TabItemHotBackgroundColorBlend);
		}
		if (SelectedTab == ᆰ)
		{
			if (ᆰ.PredefinedColor != 0)
			{
				tabColors.BackColor = ᆰ.BackColor;
				tabColors.BackColor2 = ᆰ.BackColor2;
				tabColors.BackColorGradientAngle = ᆰ.BackColorGradientAngle;
				tabColors.TextColor = ᆰ.TextColor;
			}
			if (tabColors.BackColor.IsEmpty)
			{
				tabColors.BackColor = լ.TabItemSelectedBackground;
				tabColors.BackColor2 = լ.TabItemSelectedBackground2;
				tabColors.BackColorGradientAngle = լ.TabItemSelectedBackgroundGradientAngle;
				tabColors.BackgroundColorBlend.CopyFrom(լ.TabItemSelectedBackgroundColorBlend);
			}
			if (tabColors.TextColor.IsEmpty)
			{
				if (լ.TabItemSelectedText.IsEmpty)
				{
					tabColors.TextColor = լ.TabItemText;
				}
				else
				{
					tabColors.TextColor = լ.TabItemSelectedText;
				}
			}
			if (tabColors.BorderColor.IsEmpty)
			{
				tabColors.LightBorderColor = լ.TabItemSelectedBorderLight;
				tabColors.DarkBorderColor = լ.TabItemSelectedBorderDark;
				tabColors.BorderColor = լ.TabItemSelectedBorder;
			}
		}
		if (tabColors.BackColor.IsEmpty)
		{
			if (ᆰ.BackColor.IsEmpty && ᆰ.BackColor2.IsEmpty)
			{
				tabColors.BackColor = լ.TabItemBackground;
				tabColors.BackColor2 = լ.TabItemBackground2;
				tabColors.BackColorGradientAngle = լ.TabItemBackgroundGradientAngle;
				tabColors.BackgroundColorBlend.CopyFrom(լ.TabItemBackgroundColorBlend);
			}
			else
			{
				tabColors.BackColor = ᆰ.BackColor;
				tabColors.BackColor2 = ᆰ.BackColor2;
				tabColors.BackColorGradientAngle = ᆰ.BackColorGradientAngle;
				tabColors.TextColor = ᆰ.TextColor;
			}
		}
		if (tabColors.TextColor.IsEmpty)
		{
			if (ᆰ.TextColor.IsEmpty)
			{
				tabColors.TextColor = լ.TabItemText;
			}
			else
			{
				tabColors.TextColor = ᆰ.TextColor;
			}
		}
		if (tabColors.BorderColor.IsEmpty)
		{
			if (ᆰ.BorderColor.IsEmpty && ᆰ.LightBorderColor.IsEmpty && ᆰ.DarkBorderColor.IsEmpty)
			{
				tabColors.DarkBorderColor = լ.TabItemBorderDark;
				tabColors.LightBorderColor = լ.TabItemBorderLight;
				tabColors.BorderColor = լ.TabItemBorder;
			}
			else
			{
				tabColors.LightBorderColor = ᆰ.LightBorderColor;
				tabColors.DarkBorderColor = ᆰ.DarkBorderColor;
				tabColors.BorderColor = ᆰ.BorderColor;
			}
		}
		return tabColors;
	}

	private void ᕞ(object _٠, EventArgs ؿ)
	{
		ScrollBackwards();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ScrollBackwards()
	{
		if (ᓫ != null)
		{
			TabStripActionEventArgs tabStripActionEventArgs = new TabStripActionEventArgs();
			ᓫ(this, tabStripActionEventArgs);
			if (tabStripActionEventArgs.Cancel)
			{
				return;
			}
		}
		int num = 0;
		for (int num2 = ᓺ.Count - 1; num2 >= 0; num2--)
		{
			TabItem tabItem = ᓺ[num2];
			if (tabItem.Visible)
			{
				if (ᓹ == eTabStripAlignment.Top || ᓹ == eTabStripAlignment.Bottom)
				{
					if (tabItem.DisplayRectangle.X < 4)
					{
						num = -tabItem.DisplayRectangle.Width;
						if (Ӷ == eTabStripStyle.OneNote || Ӷ == eTabStripStyle.VS2005Document)
						{
							num -= tabItem.DisplayRectangle.Height;
						}
						break;
					}
				}
				else if (tabItem.DisplayRectangle.Y < 4)
				{
					num = -tabItem.DisplayRectangle.Height;
					if (Ӷ == eTabStripStyle.OneNote || Ӷ == eTabStripStyle.VS2005Document)
					{
						num -= tabItem.DisplayRectangle.Height;
					}
					break;
				}
			}
		}
		if (num == 0 && ᔁ > 0)
		{
			num = -ᔁ;
		}
		int num3 = ᔁ;
		int num4 = ᔁ + num;
		if (num4 < 0)
		{
			num4 = 0;
		}
		if (ᔃ)
		{
			DateTime now = DateTime.Now;
			for (int num5 = num3; num5 > num4; num5--)
			{
				ScrollOffset = num5;
				if (DateTime.Now.Subtract(now).TotalMilliseconds > 200.0)
				{
					break;
				}
			}
		}
		ScrollOffset = num4;
	}

	private void ᕟ(object _٠, EventArgs ؿ)
	{
		ScrollForward();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ScrollForward()
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Invalid comparison between Unknown and I4
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Invalid comparison between Unknown and I4
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Invalid comparison between Unknown and I4
		if (ᓬ != null)
		{
			TabStripActionEventArgs tabStripActionEventArgs = new TabStripActionEventArgs();
			ᓬ(this, tabStripActionEventArgs);
			if (tabStripActionEventArgs.Cancel)
			{
				return;
			}
		}
		int num = 0;
		TabItem tabItem = null;
		int num2 = 0;
		int num3 = ᓺ.Count;
		int num4 = 1;
		if ((int)((Control)this).RightToLeft == 1)
		{
			num2 = ᓺ.Count - 1;
			num3 = -1;
			num4 = -1;
		}
		for (int i = num2; i != num3; i += num4)
		{
			TabItem tabItem2 = ᓺ[i];
			if (!tabItem2.Visible)
			{
				continue;
			}
			if (ᓹ == eTabStripAlignment.Top || ᓹ == eTabStripAlignment.Bottom)
			{
				if ((tabItem2.DisplayRectangle.X >= 4 || (int)((Control)this).RightToLeft != 0) && (tabItem2.DisplayRectangle.Right > ((Control)this).Width || (int)((Control)this).RightToLeft != 1))
				{
					tabItem = tabItem2;
					num = tabItem2.DisplayRectangle.Width;
					if (tabItem2.DisplayRectangle.Right > ((Control)this).Width - ᔂ.DefaultWidth)
					{
						break;
					}
				}
			}
			else if (tabItem2.DisplayRectangle.Y >= 4)
			{
				tabItem = tabItem2;
				num = tabItem2.DisplayRectangle.Height;
				if (tabItem2.DisplayRectangle.Bottom > ((Control)this).Height - ᔂ.DefaultWidth)
				{
					break;
				}
			}
		}
		if (tabItem != null)
		{
			if (ᓹ == eTabStripAlignment.Top || ᓹ == eTabStripAlignment.Bottom)
			{
				if (tabItem.DisplayRectangle.Right + 2 < ((Control)this).Width - ᔂ.DefaultWidth)
				{
					return;
				}
			}
			else if (tabItem.DisplayRectangle.Bottom + 2 < ((Control)this).Height - ᔂ.DefaultWidth)
			{
				return;
			}
		}
		int num5 = ᔁ;
		int num6 = ᔁ + num;
		if (ᓹ == eTabStripAlignment.Top || ᓹ == eTabStripAlignment.Bottom)
		{
			if (num6 > ᓺ[ᓺ.Count - 1].DisplayRectangle.Right + ᔁ && (int)((Control)this).RightToLeft == 0)
			{
				num6 = ᓺ[ᓺ.Count - 1].DisplayRectangle.Right + ᔁ + ᔂ.DefaultWidth;
			}
			else if (num6 < 0 && (int)((Control)this).RightToLeft == 1)
			{
				num6 = 0;
			}
		}
		else if (num6 > ᓺ[ᓺ.Count - 1].DisplayRectangle.Bottom + ᔁ)
		{
			num6 = ᓺ[ᓺ.Count - 1].DisplayRectangle.Bottom + ᔁ + ᔂ.DefaultWidth;
		}
		if (ᔃ)
		{
			DateTime now = DateTime.Now;
			for (int j = num5; j < num6; j++)
			{
				ScrollOffset = j;
				if (DateTime.Now.Subtract(now).TotalMilliseconds > 200.0)
				{
					break;
				}
			}
		}
		ScrollOffset = num6;
	}

	private void ᕠ(object _٠, EventArgs ؿ)
	{
		ᕡ();
	}

	private void ᕡ()
	{
		ᎁ(SelectedTab);
	}

	private void ᎁ(TabItem ᆰ)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		if (ᆰ == null)
		{
			return;
		}
		if (ᓭ != null)
		{
			TabStripActionEventArgs tabStripActionEventArgs = new TabStripActionEventArgs();
			tabStripActionEventArgs.TabItem = ᆰ;
			ᓭ(this, tabStripActionEventArgs);
			if (tabStripActionEventArgs.Cancel)
			{
				return;
			}
		}
		if (ᔄ && ᆰ != null && ᆰ.AttachedControl is Form)
		{
			((Form)ᆰ.AttachedControl).Close();
		}
		else if (ᆰ != null && ᆰ.AttachedItem != null && ᆰ.AttachedItem is DockContainerItem)
		{
			if (ᆰ.AttachedItem.ContainerControl is Bar bar)
			{
				bar.CloseDockTab((DockContainerItem)ᆰ.AttachedItem, eEventSource.Mouse);
			}
			else
			{
				ᆰ.AttachedItem.Visible = false;
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeFixedTabSize()
	{
		return !ᔜ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetFixedTabSize()
	{
		TypeDescriptor.GetProperties(this)["FixedTabSize"].SetValue(this, Size.Empty);
	}

	public TabItem HitTest(int x, int y)
	{
		if ((Ӷ == eTabStripStyle.OneNote || Ӷ == eTabStripStyle.VS2005Document || Ӷ == eTabStripStyle.Office2007Document) && ᕔ().Contains(x, y))
		{
			return SelectedTab;
		}
		GetSystemBoxRectangle();
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
		if (ᔄ && ᔅ == null)
		{
			Form val = ((Control)this).FindForm();
			if (val != null && val.IsMdiContainer)
			{
				MdiForm = val;
			}
		}
		if ((Ӷ == eTabStripStyle.OneNote || Ӷ == eTabStripStyle.VS2005Document || Ӷ == eTabStripStyle.Office2007Document) && ᔋ == null)
		{
			ᔋ = new Font(((Control)this).Font, (FontStyle)1);
		}
	}

	private void ᕢ()
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected O, but got Unknown
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		MdiClient val = ᕬ(ᔅ);
		if (val == null)
		{
			ᔅ.Load += ᕤ;
			return;
		}
		((Control)val).ControlAdded += new ControlEventHandler(MdiFormAdded);
		((Control)val).ControlRemoved += new ControlEventHandler(MdiFormRemoved);
		ᔅ.MdiChildActivate += ᕩ;
		ᔆ = true;
	}

	private void ᕣ()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		if (ᔆ)
		{
			MdiClient val = ᕬ(ᔅ);
			if (val != null)
			{
				((Control)val).ControlAdded -= new ControlEventHandler(MdiFormAdded);
				((Control)val).ControlRemoved -= new ControlEventHandler(MdiFormRemoved);
				ᔅ.MdiChildActivate -= ᕩ;
				ᔆ = false;
			}
		}
	}

	private void ᕤ(object _٠, EventArgs ؿ)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		if (ᔅ != null)
		{
			ᔅ.Load -= ᕤ;
			if (!ᔆ)
			{
				ᕢ();
				if (ᔅ.MdiChildren.Length > 0)
				{
					ᕨ();
				}
			}
		}
		else if (_٠ is Form)
		{
			((Form)_٠).Load -= ᕤ;
		}
	}

	internal void ᕥ(TabItem _06EC)
	{
		HideToolTip();
		if (!ᔧ)
		{
			if (ᓮ != null)
			{
				ᓮ(_06EC, new EventArgs());
			}
			if (ᓥ == 1 && _06EC.Visible && ᓻ == null)
			{
				SelectedTab = _06EC;
			}
		}
	}

	private string ᕦ(Form ᕧ)
	{
		if (ᕧ == null)
		{
			return "";
		}
		string text = ((Control)ᕧ).Text.Replace("&", "&&");
		if (text.Length > ᔇ)
		{
			int num = ᔇ / 2;
			return text.Substring(0, num) + "..." + text.Substring(text.Length - (ᔇ - num));
		}
		return text;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public void MdiFormAdded(object sender, ControlEventArgs e)
	{
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		if (ᔅ == null || !ᔆ)
		{
			return;
		}
		Control control = e.Control;
		Form val = (Form)(object)((control is Form) ? control : null);
		if (val == null)
		{
			ᕨ();
			return;
		}
		string text = ᕦ(val);
		TabItem tabItem = new TabItem();
		tabItem.Text = text;
		tabItem.AttachedControl = (Control)(object)val;
		if (ᔈ && val.Icon != null)
		{
			tabItem.Icon = new Icon(val.Icon, tabItem.IconSize);
		}
		ᓺ.Add(tabItem);
		((Control)val).TextChanged += ᕪ;
		((Control)val).VisibleChanged += ᕫ;
		if (ᔅ.ActiveMdiChild == val)
		{
			SelectedTab = tabItem;
		}
		if (ᔉ && ᔅ != null && !((Control)this).Visible && !Ꮬ)
		{
			BarFunctions.SetControlVisible((Control)(object)this, visible: true);
		}
		((Control)this).Invalidate();
	}

	private void ᕨ()
	{
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		if (ᔅ == null || !ᔄ || !BarFunctions.IsHandleValid((Control)(object)this))
		{
			return;
		}
		if (ᓺ.Count > 0)
		{
			foreach (TabItem item in ᓺ)
			{
				if (item.AttachedControl is Form)
				{
					Form val = (Form)item.AttachedControl;
					((Control)val).TextChanged -= ᕪ;
					((Control)val).VisibleChanged -= ᕫ;
				}
				item.Dispose();
			}
		}
		ᓺ.Clear();
		Form[] mdiChildren = ᔅ.MdiChildren;
		foreach (Form val2 in mdiChildren)
		{
			string text = ᕦ(val2);
			TabItem tabItem2 = new TabItem();
			tabItem2.Text = text;
			tabItem2.AttachedControl = (Control)(object)val2;
			if (ᔈ && val2.Icon != null)
			{
				tabItem2.Icon = new Icon(val2.Icon, tabItem2.IconSize);
			}
			ᓺ.Add(tabItem2);
			tabItem2.Visible = ((Control)val2).Visible;
			((Control)val2).TextChanged += ᕪ;
			((Control)val2).VisibleChanged += ᕫ;
			if (ᔅ.ActiveMdiChild == val2)
			{
				SelectedTab = tabItem2;
			}
		}
		if (ᔉ && ᔅ != null)
		{
			if (ᔅ.MdiChildren.Length == 0 && ((Control)this).Visible && !Ꮬ)
			{
				((Control)this).Visible = false;
			}
			else if (ᔅ.MdiChildren.Length > 0 && !((Control)this).Visible && !Ꮬ)
			{
				BarFunctions.SetControlVisible((Control)(object)this, visible: true);
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public void MdiFormRemoved(object sender, ControlEventArgs e)
	{
		Control control = e.Control;
		Form val = (Form)(object)((control is Form) ? control : null);
		if (val == null)
		{
			if (ᓭ != null)
			{
				TabStripActionEventArgs e2 = new TabStripActionEventArgs();
				ᓭ(this, e2);
			}
			ᕨ();
			return;
		}
		foreach (TabItem item in ᓺ)
		{
			if (item.AttachedControl == val)
			{
				if (ᓭ != null)
				{
					TabStripActionEventArgs e3 = new TabStripActionEventArgs();
					ᓭ(item, e3);
				}
				ᓺ.Remove(item);
				item.AttachedControl = null;
				item.Dispose();
				((Control)val).TextChanged -= ᕪ;
				((Control)val).VisibleChanged -= ᕫ;
				((Control)this).Invalidate();
				break;
			}
		}
		if (ᔉ && ᔅ != null && ᔅ.MdiChildren.Length == 0 && !Ꮬ)
		{
			((Control)this).Visible = false;
		}
	}

	private void ᕩ(object _٠, EventArgs ؿ)
	{
		Form activeMdiChild = ᔅ.ActiveMdiChild;
		foreach (TabItem item in ᓺ)
		{
			if (item.AttachedControl == activeMdiChild)
			{
				if (SelectedTab != item)
				{
					SelectedTab = item;
				}
				break;
			}
		}
	}

	private void ᕪ(object _٠, EventArgs ؿ)
	{
		Form val = (Form)((_٠ is Form) ? _٠ : null);
		if (val == null)
		{
			return;
		}
		foreach (TabItem item in ᓺ)
		{
			if (item.AttachedControl == val)
			{
				item.Text = ᕦ(val);
				break;
			}
		}
	}

	private void ᕫ(object _٠, EventArgs ؿ)
	{
		Form val = (Form)((_٠ is Form) ? _٠ : null);
		if (val == null)
		{
			return;
		}
		foreach (TabItem item in ᓺ)
		{
			if (item.AttachedControl == val)
			{
				item.Visible = ((Control)val).Visible;
				break;
			}
		}
	}

	private MdiClient ᕬ(Form ਸ਼)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		if (!ਸ਼.IsMdiContainer)
		{
			return null;
		}
		foreach (Control item in (ArrangedElementCollection)((Control)ਸ਼).Controls)
		{
			Control val = item;
			if (val is MdiClient)
			{
				return (MdiClient)(object)((val is MdiClient) ? val : null);
			}
		}
		return null;
	}

	internal Color ᕭ()
	{
		Color result = ControlPaint.Light(SystemColors.Control);
		if (BarFunctions.ThemedOS && _089F.न >= 16)
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
		if (_0CA9 != null)
		{
			((Control)_0CA9).Hide();
			((Component)(object)_0CA9).Dispose();
			_0CA9 = null;
		}
		ᔚ = null;
	}

	private void _0C4E(TabItem ᆰ)
	{
		if (_0CA9 != null)
		{
			HideToolTip();
		}
		if (ᆰ.Tooltip != null && !(ᆰ.Tooltip == "") && ((Control)this).Visible)
		{
			if (_0CA9 == null)
			{
				_0CA9 = new ToolTip();
			}
			_0CA9.Text = ᆰ.Tooltip;
			_0CA9.ShowToolTip();
			ᔚ = ᆰ;
		}
	}

	private void ఘ()
	{
		_089F.TRACKMOUSEEVENT tme = default(_089F.TRACKMOUSEEVENT);
		tme.dwFlags = 1073741824u;
		tme.hwndTrack = ((Control)this).Handle;
		tme.cbSize = Marshal.SizeOf((object)tme);
		_089F.TrackMouseEvent(ref tme);
		tme.dwFlags |= 1u;
		_089F.TrackMouseEvent(ref tme);
	}

	private void ఆ()
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
			if (ᔚ == tabItem)
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
			_0C4E(tabItem);
		}
	}
}
