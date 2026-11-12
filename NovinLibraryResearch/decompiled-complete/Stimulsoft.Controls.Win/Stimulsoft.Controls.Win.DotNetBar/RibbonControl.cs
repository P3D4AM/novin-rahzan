using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;
using Stimulsoft.Controls.Win.DotNetBar.Ribbon;
using Stimulsoft.Controls.Win.DotNetBar.TextMarkup;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxBitmap(typeof(RibbonControl), "Ribbon.RibbonControl.ico")]
[ToolboxItem(false)]
public class RibbonControl : ContainerControl
{
	private RibbonStrip m_RibbonStrip;

	private bool m_Expanded = true;

	private bool m_MdiSystemItemVisible = true;

	private Form m_ActiveMdiChild;

	private bool m_MdiChildMaximized;

	private bool m_AutoSize;

	private ShadowPaintInfo m_ShadowPaintInfo;

	private bool m_UseCustomizeDialog = true;

	private bool m_EnableQatPlacement = true;

	private eCategorizeMode m_CategorizeMode;

	private QatToolbar m_QatToolbar;

	private bool m_QatPositionedBelow;

	private int DefaultBottomDockPadding;

	private SubItemsQatCollection m_QatSubItemsCollection;

	private bool m_QatLayoutChanged;

	private RibbonLocalization m_SystemText = new RibbonLocalization();

	private Form m_PreviousMergedForm;

	private bool m_AllowMerge = true;

	private bool m_UseExternalCustomization;

	private const string SYS_CUSTOMIZE_POPUP_MENU = "syscustomizepopupmenu";

	private bool m_ExpandedQatBelowRibbon;

	private int m_ExpandedQatHeight;

	private bool m_PopupMode;

	private RibbonPanel m_PopupRibbonPanel;

	private bool m_MenuTabsEnabled = true;

	private ContextMenuBar m_GlobalContextMenuBar;

	private SubItemsCollection m_QatFrequentCommands = new SubItemsCollection(null);

	public static readonly string SysQatCustomizeItemName = "sysCustomizeQuickAccessToolbar";

	public static readonly string SysQatAddToItemName = "sysAddToQuickAccessToolbar";

	public static readonly string SysQatRemoveFromItemName = "sysRemoveFromQuickAccessToolbar";

	public static readonly string SysQatPlaceItemName = "sysPlaceQuickAccessToolbar";

	public static readonly string SysMinimizeRibbon = "sysMinimizeRibbon";

	public static readonly string SysMaximizeRibbon = "sysMaximizeRibbon";

	public static readonly string SysQatCustomizeLabelName = "sysCustomizeQuickAccessToolbarLabel";

	public static readonly string SysFrequentlyQatNamePart = "sysQatFrequent_";

	private int m_SyncingSize;

	private bool m_RibbonControlResized;

	private eOffice2007ColorScheme m_DelayedColorTableChange;

	private bool m_SetColorTableDelayed;

	private Timer m_ActiveWindowTimer;

	private IntPtr m_ForegroundWindow = IntPtr.Zero;

	private IntPtr m_ActiveWindow = IntPtr.Zero;

	private bool _MouseWheelTabScrollEnabled = true;

	private bool m_SetExpandedDelayed;

	private bool m_ExpandedDelayed;

	private int m_ExpandedHeight;

	private bool stopExpanded;

	private bool prevExpanded;

	private bool m_IsLayoutSuspended;

	private Form _ParentForm;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public SubItemsCollection QatFrequentCommands => m_QatFrequentCommands;

	[DefaultValue(true)]
	[Browsable(true)]
	[Category("Behavior")]
	[Description("Indicates whether KeyTips functionality is enabled.")]
	public bool KeyTipsEnabled
	{
		get
		{
			return m_RibbonStrip.KeyTipsEnabled;
		}
		set
		{
			m_RibbonStrip.KeyTipsEnabled = value;
		}
	}

	internal bool IsDesignMode => ((Component)this).DesignMode;

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Behaviour")]
	[Description("Indicates whether merge functionality is enabled for the control.")]
	public bool AllowMerge
	{
		get
		{
			return m_AllowMerge;
		}
		set
		{
			m_AllowMerge = value;
		}
	}

	[Browsable(false)]
	[Category("Layout")]
	[Description("Indicates whether control height is set automatically.")]
	[DefaultValue(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override bool AutoSize
	{
		get
		{
			return m_AutoSize;
		}
		set
		{
			m_AutoSize = value;
			OnAutoSizeChanged();
		}
	}

	[DefaultValue(true)]
	[Browsable(true)]
	[Category("Appearance")]
	[Description("Gets or sets whether anti-aliasing is used while painting.")]
	public bool AntiAlias
	{
		get
		{
			return m_RibbonStrip.AntiAlias;
		}
		set
		{
			m_RibbonStrip.AntiAlias = value;
			((Control)this).Invalidate();
		}
	}

	private bool IsGlassEnabled
	{
		get
		{
			if (((Component)this).DesignMode || !CanSupportGlass)
			{
				return false;
			}
			return WinApi.IsGlassEnabled;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.TextMarkupUIEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[EditorBrowsable(EditorBrowsableState.Always)]
	[Category("Appearance")]
	[Description("Indicates text displayed on Ribbon Title instead of the Form.Text property.")]
	public string TitleText
	{
		get
		{
			return m_RibbonStrip.TitleText;
		}
		set
		{
			m_RibbonStrip.TitleText = value;
		}
	}

	[DefaultValue(null)]
	[Description("Indicates Context menu bar associated with the ribbon control which is used as part of Global Items feature.")]
	[Category("Data")]
	public ContextMenuBar GlobalContextMenuBar
	{
		get
		{
			return m_GlobalContextMenuBar;
		}
		set
		{
			if (m_GlobalContextMenuBar != null)
			{
				m_GlobalContextMenuBar.GlobalParentComponent = null;
			}
			m_GlobalContextMenuBar = value;
			if (m_GlobalContextMenuBar != null)
			{
				m_GlobalContextMenuBar.GlobalParentComponent = (Component)(object)this;
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Caption")]
	[Description("Indicates whether custom caption and quick access toolbar provided by the control is visible.")]
	public bool CaptionVisible
	{
		get
		{
			return m_RibbonStrip.CaptionVisible;
		}
		set
		{
			m_RibbonStrip.CaptionVisible = value;
			if (!value && ((Control)this).FindForm() is RibbonForm ribbonForm)
			{
				ribbonForm.UpdateGlass();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(null)]
	[Category("Caption")]
	[Description("Indicates font for the form caption text when CaptionVisible=true.")]
	public Font CaptionFont
	{
		get
		{
			return m_RibbonStrip.CaptionFont;
		}
		set
		{
			m_RibbonStrip.CaptionFont = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(0)]
	[Category("Caption")]
	[Description("Indicates explicit height of the caption provided by control")]
	public int CaptionHeight
	{
		get
		{
			return m_RibbonStrip.CaptionHeight;
		}
		set
		{
			m_RibbonStrip.CaptionHeight = value;
			RecalcLayout();
		}
	}

	[Browsable(true)]
	[DefaultValue(46)]
	[Category("Layout")]
	[Description("Indicates indent of the ribbon strip.")]
	public int RibbonStripIndent
	{
		get
		{
			return m_RibbonStrip.RibbonStripIndent;
		}
		set
		{
			m_RibbonStrip.RibbonStripIndent = value;
		}
	}

	[Category("Appearance")]
	[Description("Gets or sets the font used by the ribbon strip.")]
	public Font RibbonStripFont
	{
		get
		{
			return ((Control)m_RibbonStrip).Font;
		}
		set
		{
			((Control)m_RibbonStrip).Font = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether mouse over fade effect is enabled")]
	public bool FadeEffect
	{
		get
		{
			return m_RibbonStrip.FadeEffect;
		}
		set
		{
			m_RibbonStrip.FadeEffect = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(null)]
	[Category("Appearance")]
	[Description("Indicates font that is used to display Key Tips (accelerator keys) when they are displayed.")]
	public virtual Font KeyTipsFont
	{
		get
		{
			return m_RibbonStrip.KeyTipsFont;
		}
		set
		{
			m_RibbonStrip.KeyTipsFont = value;
		}
	}

	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.RibbonTabItemGroupCollectionEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Tab Groups")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	public RibbonTabItemGroupCollection TabGroups => m_RibbonStrip.TabGroups;

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(10)]
	[Category("Tab Groups")]
	[Description("Indicates height in pixels of tab group line that is displayed above the RibbonTabItem objects that have group assigned.")]
	public int TabGroupHeight
	{
		get
		{
			return m_RibbonStrip.TabGroupHeight;
		}
		set
		{
			m_RibbonStrip.TabGroupHeight = value;
			if (((Component)this).DesignMode)
			{
				RecalcLayout();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(false)]
	[Category("Tab Groups")]
	[Description("Indicates whether tab group line that is displayed above the RibbonTabItem objects that have group assigned is visible.")]
	public bool TabGroupsVisible
	{
		get
		{
			return m_RibbonStrip.TabGroupsVisible;
		}
		set
		{
			m_RibbonStrip.TabGroupsVisible = value;
			if (((Component)this).DesignMode)
			{
				RecalcLayout();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Tab Groups")]
	[DefaultValue(null)]
	public Font DefaultGroupFont
	{
		get
		{
			return m_RibbonStrip.DefaultGroupFont;
		}
		set
		{
			m_RibbonStrip.DefaultGroupFont = value;
			if (((Component)this).DesignMode)
			{
				((Control)this).Refresh();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Style")]
	[Description("Gets or sets bar background style.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ElementStyle BackgroundStyle => m_RibbonStrip.BackgroundStyle;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public RibbonTabItem SelectedRibbonTabItem
	{
		get
		{
			return m_RibbonStrip.SelectedRibbonTabItem;
		}
		set
		{
			if (value != null)
			{
				value.Checked = true;
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public RibbonStrip RibbonStrip => m_RibbonStrip;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(false)]
	public SubItemsCollection Items => m_RibbonStrip.Items;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(false)]
	public SubItemsCollection QuickToolbarItems => GetQatSubItemsCollection();

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public eDotNetBarStyle EffectiveStyle => m_RibbonStrip.EffectiveStyle;

	[Browsable(false)]
	[Category("Appearance")]
	[Description("Specifies the visual style of the control.")]
	[DefaultValue(eDotNetBarStyle.Office2003)]
	public eDotNetBarStyle Style
	{
		get
		{
			return m_RibbonStrip.Style;
		}
		set
		{
			if (value == eDotNetBarStyle.StyleManagerControlled)
			{
				SetStyle();
			}
			else
			{
				ChangeControlStyle(value);
			}
		}
	}

	public eOffice2007ColorScheme Office2007ColorTable
	{
		get
		{
			if (m_RibbonStrip.GetRenderer() is Office2007Renderer office2007Renderer)
			{
				return office2007Renderer.ColorTable.InitialColorScheme;
			}
			return eOffice2007ColorScheme.Blue;
		}
		set
		{
		}
	}

	[Browsable(false)]
	public bool IsPopupMode => m_PopupMode;

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether mouse wheel scrolls through the ribbon tabs.")]
	public bool MouseWheelTabScrollEnabled
	{
		get
		{
			return _MouseWheelTabScrollEnabled;
		}
		set
		{
			_MouseWheelTabScrollEnabled = value;
		}
	}

	[DefaultValue(true)]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Layout")]
	[Description("Gets or sets whether control is expanded or not. When control is expanded both the tabs and the tab ribbons are visible.")]
	public bool Expanded
	{
		get
		{
			return m_Expanded;
		}
		set
		{
			if (!stopExpanded && m_Expanded != value)
			{
				if (m_IsLayoutSuspended || !BarFunctions.IsHandleValid((Control)(object)this))
				{
					m_SetExpandedDelayed = true;
					m_ExpandedDelayed = value;
				}
				else
				{
					m_SetExpandedDelayed = false;
					m_Expanded = value;
					OnExpandedChanged();
				}
			}
		}
	}

	[DefaultValue(true)]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Layout")]
	[Description("Indicates whether control is collapsed when RibbonTabItem is double clicked and expanded when RibbonTabItem is clicked.")]
	public virtual bool AutoExpand
	{
		get
		{
			return m_RibbonStrip.AutoExpand;
		}
		set
		{
			m_RibbonStrip.AutoExpand = value;
		}
	}

	[DefaultValue(true)]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Layout")]
	[Description("Indicates whether control is collapsed/expanded when Ctrl+F1 key is clicked.")]
	public virtual bool AutoKeyboardExpand
	{
		get
		{
			return m_RibbonStrip.AutoKeyboardExpand;
		}
		set
		{
			m_RibbonStrip.AutoKeyboardExpand = value;
		}
	}

	[Browsable(true)]
	[Category("Data")]
	[DefaultValue(null)]
	[Description("ImageList for images used on Items. Images specified here will always be used on menu-items and are by default used on all Bars.")]
	public ImageList Images
	{
		get
		{
			return m_RibbonStrip.Images;
		}
		set
		{
			m_RibbonStrip.Images = value;
		}
	}

	[Browsable(true)]
	[Category("Data")]
	[DefaultValue(null)]
	[Description("ImageList for medium-sized images used on Items.")]
	public ImageList ImagesMedium
	{
		get
		{
			return m_RibbonStrip.ImagesMedium;
		}
		set
		{
			m_RibbonStrip.ImagesMedium = value;
		}
	}

	[Browsable(true)]
	[Category("Data")]
	[DefaultValue(null)]
	[Description("ImageList for large-sized images used on Items.")]
	public ImageList ImagesLarge
	{
		get
		{
			return m_RibbonStrip.ImagesLarge;
		}
		set
		{
			m_RibbonStrip.ImagesLarge = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates whether the user can give the focus to this control using the TAB key.")]
	public bool TabStop
	{
		get
		{
			return ((Control)this).TabStop;
		}
		set
		{
			((Control)this).TabStop = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Run-time Behavior")]
	[Description("Specifies whether the MDI system buttons are displayed in menu bar when MDI Child window is maximized.")]
	public bool MdiSystemItemVisible
	{
		get
		{
			return m_MdiSystemItemVisible;
		}
		set
		{
			if (m_MdiSystemItemVisible == value)
			{
				return;
			}
			m_MdiSystemItemVisible = value;
			if (!((Component)this).DesignMode)
			{
				SyncMdiChildSystemItem();
				if (!m_MdiSystemItemVisible && m_ActiveMdiChild != null)
				{
					((Control)m_ActiveMdiChild).Resize -= MdiChildFormResize;
					((Control)m_ActiveMdiChild).VisibleChanged -= MdiChildVisibleChanged;
				}
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual bool CanSupportGlass
	{
		get
		{
			return m_RibbonStrip.CanSupportGlass;
		}
		set
		{
			m_RibbonStrip.CanSupportGlass = value;
		}
	}

	private bool DisplayCustomizeContextMenu
	{
		get
		{
			if (CanCustomize)
			{
				if (!CaptionVisible)
				{
					return m_UseExternalCustomization;
				}
				return true;
			}
			return false;
		}
	}

	internal QatToolbar QatToolbar => m_QatToolbar;

	[DefaultValue(true)]
	[Browsable(true)]
	[Category("Quick Access Toolbar")]
	[Description("Indicates whether control can be customized. Caption must be visible for customization to be fully enabled.")]
	public bool CanCustomize
	{
		get
		{
			return m_RibbonStrip.CanCustomize;
		}
		set
		{
			m_RibbonStrip.CanCustomize = value;
		}
	}

	[DefaultValue(false)]
	[Browsable(true)]
	[Category("Quick Access Toolbar")]
	[Description("Indicates whether external implementation for ribbon bar and menu item customization will be used for customizing the ribbon control.")]
	public bool UseExternalCustomization
	{
		get
		{
			return m_UseExternalCustomization;
		}
		set
		{
			m_UseExternalCustomization = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Quick Access Toolbar")]
	[Description("Indicates whether end-user customization of the placement of the Quick Access Toolbar is enabled.")]
	public bool EnableQatPlacement
	{
		get
		{
			return m_EnableQatPlacement;
		}
		set
		{
			m_EnableQatPlacement = value;
		}
	}

	[DefaultValue(true)]
	[Browsable(true)]
	[Category("Quick Access Toolbar")]
	[Description("Indicates whether customize dialog is used to customize the quick access toolbar.")]
	public bool UseCustomizeDialog
	{
		get
		{
			return m_UseCustomizeDialog;
		}
		set
		{
			m_UseCustomizeDialog = value;
		}
	}

	[DefaultValue(eCategorizeMode.RibbonBar)]
	[Browsable(true)]
	[Category("Quick Access Toolbar")]
	[Description("Indicates categorization mode for the items on Quick Access Toolbar customize dialog box.")]
	public eCategorizeMode CategorizeMode
	{
		get
		{
			return m_CategorizeMode;
		}
		set
		{
			m_CategorizeMode = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool QatPositionedBelowRibbon
	{
		get
		{
			return m_QatPositionedBelow;
		}
		set
		{
			if (m_QatPositionedBelow != value)
			{
				QuickAccessToolbarChangePlacement();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string QatLayout
	{
		get
		{
			return GetQatLayoutDescription();
		}
		set
		{
			SetQatLayoutDescription(value);
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool QatLayoutChanged
	{
		get
		{
			return m_QatLayoutChanged;
		}
		set
		{
			m_QatLayoutChanged = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[NotifyParentProperty(true)]
	[Category("Localization")]
	[Description("Gets system text used by the component..")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public RibbonLocalization SystemText => m_SystemText;

	[Description("Occurs when Quick Access Toolbar placement is changed, i.e. below or above the Ribbon.")]
	public event EventHandler QatPlacementChanged;

	public event CustomizeMenuPopupEventHandler BeforeCustomizeMenuPopup;

	public event CustomizeMenuPopupEventHandler BeforeAddItemToQuickAccessToolbar;

	public event CustomizeMenuPopupEventHandler BeforeRemoveItemFromQuickAccessToolbar;

	public event CustomizeMenuPopupEventHandler AfterAddItemToQuickAccessToolbar;

	public event CustomizeMenuPopupEventHandler AfterRemoveItemFromQuickAccessToolbar;

	public event DotNetBarManager.LocalizeStringEventHandler LocalizeString;

	[Description("Occurs when Item on ribbon tab strip or quick access toolbar is clicked.")]
	public event EventHandler ItemClick;

	public event QatCustomizeDialogEventHandler BeforeQatCustomizeDialog;

	public event QatCustomizeDialogEventHandler AfterQatCustomizeDialog;

	public event EventHandler AfterQatDialogChangesApplied;

	public event EventHandler SelectedRibbonTabChanged;

	[Description("Occurs before selected RibbonPanel is displayed on popup while ribbon is collapsed.")]
	public event CancelEventHandler BeforeRibbonPanelPopup;

	[Description("Occurs after selected RibbonPanel is displayed on popup while ribbon is collapsed.")]
	public event EventHandler AfterRibbonPanelPopup;

	[Description("Occurs before RibbonPanel popup is closed.")]
	public event RibbonPopupCloseEventHandler BeforeRibbonPanelPopupClose;

	[Description("Occurs after RibbonPanel popup is closed.")]
	public event EventHandler AfterRibbonPanelPopupClose;

	[Description("Occurs when text markup link from TitleText markup is clicked.")]
	public event MarkupLinkClickEventHandler TitleTextMarkupLinkClick;

	[Description("Occurs after Expanded property has changed.")]
	public event EventHandler ExpandedChanged;

	public RibbonControl()
	{
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		BaseRenderer renderer = GlobalManager.Renderer;
		m_QatFrequentCommands.IgnoreEvents = true;
		m_QatFrequentCommands.AllowParentRemove = false;
		((Control)this).SetStyle((ControlStyles)(0x2010 | DisplayHelp.DoubleBufferFlag | 2 | 4), true);
		m_RibbonStrip = new RibbonStrip();
		((Control)m_RibbonStrip).Dock = (DockStyle)1;
		((Control)m_RibbonStrip).Height = 32;
		m_RibbonStrip.ItemAdded += RibbonStripItemAdded;
		((Control)m_RibbonStrip).SizeChanged += RibbonStripSizeChanged;
		m_RibbonStrip.ItemRemoved += RibbonStripItemRemoved;
		m_RibbonStrip.LocalizeString += RibbonStripLocalizeString;
		m_RibbonStrip.ItemClick += RibbonStripItemClick;
		m_RibbonStrip.ButtonCheckedChanged += RibbonStripButtonCheckedChanged;
		m_RibbonStrip.TitleTextMarkupLinkClick += RibbonStripTitleTextMarkupLinkClick;
		((Control)this).Controls.Add((Control)(object)m_RibbonStrip);
		TabStop = false;
		((ScrollableControl)this).DockPadding.Bottom = DefaultBottomDockPadding;
		StyleManager.Register((Control)(object)this);
	}

	protected override void Dispose(bool disposing)
	{
		StyleManager.Unregister((Control)(object)this);
		ReleaseParentForm();
		((ContainerControl)this).Dispose(disposing);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void StyleManagerStyleChanged(eDotNetBarStyle newStyle)
	{
		SetStyle();
	}

	protected virtual void OnSelectedRibbonTabChanged(object sender, EventArgs e)
	{
		if (this.SelectedRibbonTabChanged != null)
		{
			this.SelectedRibbonTabChanged(sender, e);
		}
	}

	private void RibbonStripButtonCheckedChanged(object sender, EventArgs e)
	{
		if (sender is RibbonTabItem && ((RibbonTabItem)sender).Checked)
		{
			AutoSyncSize();
			OnSelectedRibbonTabChanged(sender is RibbonTabItem, new EventArgs());
		}
	}

	private void RibbonStripItemClick(object sender, EventArgs e)
	{
		if (this.ItemClick != null)
		{
			this.ItemClick(sender, e);
		}
	}

	private void RibbonStripLocalizeString(object sender, LocalizeEventArgs e)
	{
		if (this.LocalizeString != null)
		{
			this.LocalizeString(this, e);
		}
	}

	private void OnAutoSizeChanged()
	{
		AutoSyncSize();
	}

	public virtual void AutoSyncSize()
	{
		AutoSyncSize(layoutSelectedPanel: true);
	}

	private void AutoSyncSize(bool layoutSelectedPanel)
	{
		if (!m_AutoSize || m_SyncingSize > 0 || !BarFunctions.IsHandleValid((Control)(object)this))
		{
			return;
		}
		m_SyncingSize++;
		try
		{
			RecalculateAutoSize(layoutSelectedPanel);
		}
		finally
		{
			m_SyncingSize--;
		}
	}

	private int GetItemControlHeight(ItemControl c)
	{
		int num = c.GetAutoSizeHeight();
		if (!((Control)c).MinimumSize.IsEmpty && ((Control)c).MinimumSize.Height > num)
		{
			num = ((Control)c).MinimumSize.Height + ElementStyleLayout.VerticalStyleWhiteSpace(c.GetPaintBackgroundStyle());
		}
		else if (!((Control)c).MaximumSize.IsEmpty && num > ((Control)c).MaximumSize.Height)
		{
			num = ((Control)c).MaximumSize.Height + ElementStyleLayout.VerticalStyleWhiteSpace(c.GetPaintBackgroundStyle());
		}
		return num;
	}

	private void RecalculateAutoSize()
	{
		RecalculateAutoSize(layoutSelectedPanel: true);
	}

	private int GetAutoPanelHeight(RibbonPanel panel)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		int num = 0;
		if (panel == null)
		{
			return num;
		}
		int num2 = 0;
		foreach (Control item in (ArrangedElementCollection)((Control)panel).Controls)
		{
			Control val = item;
			if (!(val is ItemControl itemControl))
			{
				continue;
			}
			int autoSizeHeight = itemControl.GetAutoSizeHeight();
			if (itemControl != null && autoSizeHeight > num2)
			{
				int itemControlHeight = GetItemControlHeight(itemControl);
				if (itemControlHeight > num2)
				{
					num2 = itemControlHeight;
				}
			}
		}
		if (num2 > 0)
		{
			num += num2;
		}
		return num;
	}

	private void RecalculateAutoSize(bool layoutSelectedPanel)
	{
		int height = ((Control)m_RibbonStrip).Height;
		if (Expanded)
		{
			if (SelectedRibbonTabItem != null)
			{
				RibbonPanel panel = SelectedRibbonTabItem.Panel;
				if (panel != null)
				{
					int autoPanelHeight = GetAutoPanelHeight(panel);
					height = ((autoPanelHeight > 0) ? (height + autoPanelHeight) : 0);
				}
				else
				{
					height = 0;
				}
			}
			else
			{
				height = 0;
			}
		}
		else
		{
			height -= 4;
		}
		if (height > 0)
		{
			height += ((ScrollableControl)this).DockPadding.Bottom + ((ScrollableControl)this).DockPadding.Top;
			if (m_QatPositionedBelow)
			{
				height += ((Control)m_QatToolbar).Height;
			}
		}
		if (((Control)this).Height == height || height <= 0)
		{
			return;
		}
		RibbonPanel ribbonPanel = null;
		if (!layoutSelectedPanel)
		{
			RibbonTabItem selectedRibbonTabItem = SelectedRibbonTabItem;
			if (selectedRibbonTabItem != null && selectedRibbonTabItem.Panel != null)
			{
				ribbonPanel = selectedRibbonTabItem.Panel;
				((Control)ribbonPanel).SuspendLayout();
			}
		}
		((Control)this).Height = height;
		if (ribbonPanel != null)
		{
			((Control)ribbonPanel).ResumeLayout(false);
		}
	}

	internal Office2007ColorTable GetOffice2007ColorTable()
	{
		if (GlobalManager.Renderer is Office2007Renderer office2007Renderer)
		{
			return office2007Renderer.ColorTable;
		}
		return new Office2007ColorTable();
	}

	public void SetRibbonPanelStyle(RibbonPanel panel)
	{
		if (((Component)this).DesignMode)
		{
			TypeDescriptor.GetProperties(((ScrollableControl)panel).DockPadding)["Left"].SetValue(((ScrollableControl)panel).DockPadding, 3);
			TypeDescriptor.GetProperties(((ScrollableControl)panel).DockPadding)["Right"].SetValue(((ScrollableControl)panel).DockPadding, 3);
			TypeDescriptor.GetProperties(((ScrollableControl)panel).DockPadding)["Bottom"].SetValue(((ScrollableControl)panel).DockPadding, 3);
		}
		else
		{
			((ScrollableControl)panel).DockPadding.Left = 3;
			((ScrollableControl)panel).DockPadding.Right = 3;
			((ScrollableControl)panel).DockPadding.Bottom = 3;
		}
	}

	public RibbonTabItem CreateRibbonTab(string text, string name, int insertPosition)
	{
		RibbonTabItem ribbonTabItem = new RibbonTabItem();
		ribbonTabItem.Text = text;
		ribbonTabItem.Name = name;
		RibbonPanel ribbonPanel = new RibbonPanel();
		((Control)ribbonPanel).Dock = (DockStyle)5;
		((Control)ribbonPanel).Width = ((Control)this).Width - 4;
		SetRibbonPanelStyle(ribbonPanel);
		((Control)this).Controls.Add((Control)(object)ribbonPanel);
		((Control)ribbonPanel).SendToBack();
		ribbonTabItem.Panel = ribbonPanel;
		if (insertPosition < 0)
		{
			insertPosition = Items.Count;
			for (int i = 0; i < Items.Count; i++)
			{
				if (Items[i].ItemAlignment == eItemAlignment.Far)
				{
					insertPosition = i;
					break;
				}
			}
			if (insertPosition >= Items.Count)
			{
				Items.Add(ribbonTabItem);
			}
			else
			{
				Items.Insert(insertPosition, ribbonTabItem);
			}
		}
		else if (insertPosition > Items.Count - 1)
		{
			insertPosition = Items.Count;
			for (int j = 0; j < Items.Count; j++)
			{
				if (Items[j].ItemAlignment == eItemAlignment.Far)
				{
					insertPosition = j;
					break;
				}
			}
			if (insertPosition >= Items.Count)
			{
				Items.Add(ribbonTabItem);
			}
			else
			{
				Items.Insert(insertPosition, ribbonTabItem);
			}
		}
		else
		{
			Items.Insert(insertPosition, ribbonTabItem);
		}
		return ribbonTabItem;
	}

	public RibbonTabItem CreateRibbonTab(string text, string name)
	{
		return CreateRibbonTab(text, name, -1);
	}

	public void RecalcLayout()
	{
		m_RibbonStrip.RecalcLayout();
		if (m_QatToolbar != null && ((Control)m_QatToolbar).Visible)
		{
			m_QatToolbar.RecalcLayout();
		}
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Expected O, but got Unknown
		((Control)this).OnHandleCreated(e);
		foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control val = item;
			IntPtr handle = val.Handle;
			if (!(val is RibbonPanel))
			{
				continue;
			}
			foreach (Control item2 in (ArrangedElementCollection)val.Controls)
			{
				Control val2 = item2;
				handle = val2.Handle;
			}
		}
		UpdateRegion();
		RecalcLayout();
		if (m_SetExpandedDelayed)
		{
			Expanded = m_ExpandedDelayed;
		}
	}

	protected override void OnControlAdded(ControlEventArgs e)
	{
		((Control)this).OnControlAdded(e);
		if (((Component)this).DesignMode || !(e.Control is RibbonPanel ribbonPanel))
		{
			return;
		}
		if (ribbonPanel.RibbonTabItem != null)
		{
			if (Items.Contains(ribbonPanel.RibbonTabItem) && SelectedRibbonTabItem == ribbonPanel.RibbonTabItem)
			{
				ribbonPanel.Visible = true;
				((Control)ribbonPanel).BringToFront();
			}
			else
			{
				ribbonPanel.Visible = false;
			}
		}
		else
		{
			ribbonPanel.Visible = false;
		}
	}

	private void RibbonStripSizeChanged(object sender, EventArgs e)
	{
		if (!m_RibbonControlResized && m_AutoSize)
		{
			AutoSyncSize();
		}
		m_RibbonControlResized = false;
		if (!m_Expanded && !m_AutoSize)
		{
			((Control)this).Height = GetCollapsedHeight();
		}
	}

	private void RibbonStripItemAdded(object sender, EventArgs e)
	{
		if (((Component)this).DesignMode || !(sender is RibbonTabItem))
		{
			return;
		}
		RibbonTabItem ribbonTabItem = sender as RibbonTabItem;
		if (ribbonTabItem.Panel != null)
		{
			if (((Control)this).Controls.Contains((Control)(object)ribbonTabItem.Panel) && ribbonTabItem.Checked)
			{
				ribbonTabItem.Panel.Visible = true;
				((Control)ribbonTabItem.Panel).BringToFront();
			}
			else
			{
				ribbonTabItem.Panel.Visible = false;
			}
			ribbonTabItem.CheckedChanged += RibbonTabCheckedChanged;
		}
	}

	private void RibbonStripItemRemoved(object sender, ItemRemovedEventArgs e)
	{
		if (sender is RibbonTabItem)
		{
			((RibbonTabItem)sender).CheckedChanged -= RibbonTabCheckedChanged;
		}
	}

	private void RibbonTabCheckedChanged(object sender, EventArgs e)
	{
		if (sender is RibbonTabItem && ((RibbonTabItem)sender).Checked)
		{
			AutoSyncSize();
		}
	}

	protected override void OnControlRemoved(ControlEventArgs e)
	{
		((Control)this).OnControlRemoved(e);
		if (!m_PopupMode && e.Control is RibbonPanel { RibbonTabItem: not null } ribbonPanel && Items.Contains(ribbonPanel.RibbonTabItem))
		{
			Items.Remove(ribbonPanel.RibbonTabItem);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void SetDesignMode()
	{
		m_RibbonStrip.SetDesignMode(mode: true);
	}

	private ElementStyle GetBackgroundStyle()
	{
		return m_RibbonStrip.InternalGetBackgroundStyle();
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		ElementStyle backgroundStyle = GetBackgroundStyle();
		if ((backgroundStyle.BackColor.A < byte.MaxValue && !backgroundStyle.BackColor.IsEmpty) || ((Control)this).BackColor == Color.Transparent)
		{
			((ScrollableControl)this).OnPaintBackground(e);
		}
		else
		{
			SolidBrush val = new SolidBrush(((Control)this).BackColor);
			try
			{
				e.Graphics.FillRectangle((Brush)(object)val, ((Control)this).ClientRectangle);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		if (IsGlassEnabled && ((Control)this).FindForm() is RibbonForm ribbonForm)
		{
			e.Graphics.SetClip(new Rectangle(0, 0, ((Control)this).Width, ribbonForm.GlassHeight), (CombineMode)4);
		}
		Office2007ColorTable office2007ColorTable = GetOffice2007ColorTable();
		if (office2007ColorTable != null)
		{
			SolidBrush val2 = new SolidBrush(office2007ColorTable.Form.BackColor);
			try
			{
				e.Graphics.FillRectangle((Brush)(object)val2, ((Control)this).ClientRectangle);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		else
		{
			ElementStyleDisplayInfo e2 = new ElementStyleDisplayInfo(backgroundStyle, e.Graphics, ((Control)this).ClientRectangle);
			ElementStyleDisplay.PaintBackground(e2);
		}
		if (!m_QatPositionedBelow && m_RibbonStrip.HasVisibleTabs && EffectiveStyle != eDotNetBarStyle.Metro)
		{
			ShadowPaintInfo shadowPaintInfo = GetShadowPaintInfo();
			shadowPaintInfo.Rectangle = new Rectangle(-6, 0, ((Control)this).Bounds.Width + 12, ((Control)this).Bounds.Height);
			shadowPaintInfo.Graphics = e.Graphics;
			ShadowPainter.Paint3(shadowPaintInfo, 2);
		}
	}

	private ShadowPaintInfo GetShadowPaintInfo()
	{
		if (m_ShadowPaintInfo == null)
		{
			m_ShadowPaintInfo = new ShadowPaintInfo();
		}
		m_ShadowPaintInfo.Size = 3;
		eDotNetBarStyle effectiveStyle = EffectiveStyle;
		m_ShadowPaintInfo.IsSquare = effectiveStyle == eDotNetBarStyle.Office2010 || effectiveStyle == eDotNetBarStyle.Windows7 || effectiveStyle == eDotNetBarStyle.Metro;
		return m_ShadowPaintInfo;
	}

	protected override void OnResize(EventArgs e)
	{
		m_RibbonControlResized = true;
		UpdateRegion();
		((Control)this).OnResize(e);
		AutoSyncSize();
	}

	protected override void NotifyInvalidate(Rectangle invalidatedArea)
	{
		((Control)this).NotifyInvalidate(invalidatedArea);
		UpdateRegion();
	}

	private void UpdateRegion()
	{
		if (((Control)this).Parent is RibbonForm && m_RibbonStrip.CaptionVisible && !IsGlassEnabled)
		{
			((Control)this).Region = GetControlRegion();
		}
	}

	protected override void WndProc(ref Message m)
	{
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Invalid comparison between Unknown and I4
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Invalid comparison between Unknown and I4
		if (((Message)(ref m)).Msg == 132)
		{
			int x = WinApi.LOWORD(((Message)(ref m)).LParam);
			int y = WinApi.HIWORD(((Message)(ref m)).LParam);
			Point pt = ((Control)this).PointToClient(new Point(x, y));
			if (IsGlassEnabled && CaptionVisible)
			{
				Rectangle rectangle = new Rectangle(((Control)this).Width - SystemInformation.CaptionButtonSize.Width * 3, 0, SystemInformation.CaptionButtonSize.Width * 3, SystemInformation.CaptionButtonSize.Height);
				if (rectangle.Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(-1);
					return;
				}
				if (m_RibbonStrip != null && !m_RibbonStrip.IsMaximized && new Rectangle(0, 0, ((Control)this).Width, 4).Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(-1);
					return;
				}
				if (BarFunctions.IsWindows7 && m_RibbonStrip != null && m_RibbonStrip.IsMaximized && m_RibbonStrip.CaptionBounds.Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(-1);
					return;
				}
			}
			else if (CaptionVisible && m_RibbonStrip != null && !m_RibbonStrip.IsMaximized && new Rectangle(0, 0, ((Control)this).Width, 4).Contains(pt))
			{
				((Message)(ref m)).Result = new IntPtr(-1);
				return;
			}
			if ((EffectiveStyle == eDotNetBarStyle.Office2010 || EffectiveStyle == eDotNetBarStyle.Metro) && ((pt.X < 28 && (int)((Control)this).RightToLeft == 0) || (pt.X > ((Control)this).Width - 28 && (int)((Control)this).RightToLeft == 1)) && pt.Y < 28)
			{
				((Message)(ref m)).Result = new IntPtr(-1);
				return;
			}
			if (EffectiveStyle == eDotNetBarStyle.Windows7 && ((pt.X < 28 && (int)((Control)this).RightToLeft == 0) || (pt.X > ((Control)this).Width - 28 && (int)((Control)this).RightToLeft == 1)) && pt.Y < 28)
			{
				((Message)(ref m)).Result = new IntPtr(-1);
				return;
			}
		}
		((ContainerControl)this).WndProc(ref m);
	}

	protected virtual Region GetControlRegion()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Invalid comparison between Unknown and I4
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Expected O, but got Unknown
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Expected O, but got Unknown
		if (((Control)this).Parent is RibbonForm && (int)((Form)((Control)this).Parent).WindowState == 2)
		{
			return null;
		}
		eDotNetBarStyle effectiveStyle = EffectiveStyle;
		if (effectiveStyle == eDotNetBarStyle.Metro)
		{
			return null;
		}
		GraphicsPath val = new GraphicsPath();
		Rectangle bounds = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
		if (effectiveStyle == eDotNetBarStyle.Office2010 && !BarFunctions.IsWindows8)
		{
			bounds.Y++;
		}
		bool flag = (int)((Control)this).RightToLeft == 1;
		int num = 2;
		int num2 = 2;
		if (((Control)this).Parent is RibbonForm)
		{
			RibbonForm ribbonForm = ((Control)this).Parent as RibbonForm;
			num = ribbonForm.TopLeftCornerSize - 2;
			num2 = ribbonForm.TopRightCornerSize - 1;
			if (num <= 0)
			{
				num = 0;
			}
			if (num2 <= 0)
			{
				num = 0;
			}
			if (flag)
			{
				int num3 = num;
				num = num2;
				num2 = num3;
			}
			if (num > 0)
			{
				ArcData cornerArc = ElementStyleDisplay.GetCornerArc(bounds, num, eCornerArc.TopLeft);
				val.AddArc(cornerArc.X, cornerArc.Y, cornerArc.Width, cornerArc.Height, cornerArc.StartAngle, cornerArc.SweepAngle);
			}
			else
			{
				val.AddLine(bounds.X, bounds.Y + 2, bounds.X, bounds.Y);
				val.AddLine(bounds.X, bounds.Y, bounds.X + 2, bounds.Y);
			}
			if (num2 > 0)
			{
				ArcData cornerArc = ElementStyleDisplay.GetCornerArc(bounds, num2, eCornerArc.TopRight);
				val.AddArc(cornerArc.X, cornerArc.Y, cornerArc.Width, cornerArc.Height, cornerArc.StartAngle, cornerArc.SweepAngle);
			}
			else
			{
				val.AddLine(bounds.Right - 2, bounds.Y, bounds.Right, bounds.Y);
				val.AddLine(bounds.Right, bounds.Y, bounds.Right, bounds.Y + 2);
			}
			val.AddLine(bounds.Right, bounds.Bottom - 2, bounds.Right, bounds.Bottom);
			val.AddLine(bounds.Right, bounds.Bottom, bounds.Right - 2, bounds.Bottom);
			val.AddLine(bounds.X + 2, bounds.Bottom, bounds.X, bounds.Bottom);
			val.AddLine(bounds.X, bounds.Bottom, bounds.X, bounds.Bottom - 2);
			val.CloseAllFigures();
			Region val2 = new Region();
			val2.MakeEmpty();
			val2.Union(val);
			val.Widen(SystemPens.Control);
			Region val3 = new Region(val);
			val2.Union(val);
			return val2;
		}
		return null;
	}

	private void RibbonStripTitleTextMarkupLinkClick(object sender, MarkupLinkClickEventArgs e)
	{
		if (this.TitleTextMarkupLinkClick != null)
		{
			this.TitleTextMarkupLinkClick(this, new MarkupLinkClickEventArgs(e.Name, e.HRef));
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeRibbonStripFont()
	{
		MethodInfo method = ((object)m_RibbonStrip).GetType().GetMethod("ShouldSerializeFont", BindingFlags.Instance | BindingFlags.NonPublic);
		return (bool)method.Invoke(m_RibbonStrip, null);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetRibbonStripFont()
	{
		((Control)m_RibbonStrip).ResetFont();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetDefaultGroupFont()
	{
		TypeDescriptor.GetProperties(this)["DefaultGroupFont"].SetValue(this, null);
	}

	private SubItemsCollection GetQatSubItemsCollection()
	{
		if (m_QatPositionedBelow)
		{
			if (m_QatSubItemsCollection == null)
			{
				m_QatSubItemsCollection = new SubItemsQatCollection(m_QatToolbar);
				BaseItem applicationButton = GetApplicationButton();
				if (applicationButton != null)
				{
					m_QatSubItemsCollection._Add(applicationButton);
				}
				foreach (BaseItem item in m_QatToolbar.Items)
				{
					m_QatSubItemsCollection._Add(item);
				}
			}
			return m_QatSubItemsCollection;
		}
		return m_RibbonStrip.QuickToolbarItems;
	}

	private static BaseItem GetAppButton(RibbonControl ribbon)
	{
		BaseItem result = null;
		for (int i = 0; i < ribbon.QuickToolbarItems.Count; i++)
		{
			if (ribbon.QuickToolbarItems[i] is ApplicationButton)
			{
				result = ribbon.QuickToolbarItems[i];
				break;
			}
		}
		return result;
	}

	protected override void OnBackColorChanged(EventArgs e)
	{
		((Control)this).OnBackColorChanged(e);
	}

	public void SetStyle()
	{
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Expected O, but got Unknown
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Expected O, but got Unknown
		switch (StyleManager.GetEffectiveStyle())
		{
		case eDotNetBarStyle.Office2010:
		case eDotNetBarStyle.Windows7:
		case eDotNetBarStyle.Metro:
		{
			BaseItem appButton = GetAppButton(this);
			if (appButton != null)
			{
				if (appButton is ButtonItem && ((ButtonItem)appButton).ImageFixedSize.IsEmpty)
				{
					double factor = StiScale2.Factor;
					((ButtonItem)appButton).ImageFixedSize = new Size((int)Math.Ceiling(16.0 * StiScale2.X), (int)Math.Ceiling(16.0 * StiScale2.Y));
				}
				if (appButton is ButtonItem)
				{
					ButtonItem buttonItem = (ButtonItem)appButton;
					buttonItem.ImagePaddingHorizontal = 0;
					buttonItem.ImagePaddingVertical = 0;
				}
				if (appButton.Parent != null)
				{
					appButton.Parent.SubItems.Remove(appButton);
				}
				Items.Insert(0, appButton);
			}
			break;
		}
		case eDotNetBarStyle.Office2007:
			if (Items.Count > 0 && Items[0] is ApplicationButton)
			{
				ApplicationButton applicationButton = (ApplicationButton)Items[0];
				if (!applicationButton.ImageFixedSize.IsEmpty)
				{
					applicationButton.ImageFixedSize = Size.Empty;
				}
				applicationButton.ImagePaddingHorizontal = 2;
				applicationButton.ImagePaddingVertical = 2;
				Items.Remove(applicationButton);
				if (m_QatPositionedBelow)
				{
					m_RibbonStrip.CaptionContainerItem.SubItems.Insert(0, applicationButton);
				}
				else
				{
					QuickToolbarItems.Insert(0, applicationButton);
				}
			}
			break;
		}
		foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control val = item;
			if (!(val is RibbonPanel))
			{
				continue;
			}
			((RibbonPanel)(object)val).ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
			RibbonPanel ribbonPanel = (RibbonPanel)(object)val;
			foreach (Control item2 in (ArrangedElementCollection)val.Controls)
			{
				Control val2 = item2;
				if (val2 is RibbonBar)
				{
					RibbonBar ribbonBar = val2 as RibbonBar;
					if (ribbonBar.GalleryStretch == null && !ribbonPanel.DefaultLayout && !((Component)this).DesignMode)
					{
						((Control)ribbonBar).Width = ((Control)ribbonBar).Width + 4;
					}
					TypeDescriptor.GetProperties(ribbonBar)["Style"].SetValue(ribbonBar, eDotNetBarStyle.StyleManagerControlled);
				}
			}
		}
		m_RibbonStrip.Style = eDotNetBarStyle.StyleManagerControlled;
		RibbonStrip.InitDefaultStyles();
		((Control)this).Invalidate(true);
		if (((Control)this).IsHandleCreated && SelectedRibbonTabItem != null && SelectedRibbonTabItem.Panel != null)
		{
			((Control)SelectedRibbonTabItem.Panel).PerformLayout();
		}
		if (m_QatToolbar != null)
		{
			m_QatToolbar.Style = eDotNetBarStyle.StyleManagerControlled;
		}
		if (((Control)this).IsHandleCreated)
		{
			((Control)this).Invalidate(true);
			RecalcLayout();
		}
	}

	private void ChangeControlStyle(eDotNetBarStyle newStyle)
	{
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Expected O, but got Unknown
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Expected O, but got Unknown
		switch (newStyle)
		{
		case eDotNetBarStyle.Office2010:
			if (GlobalManager.Renderer is Office2007Renderer && !(((Office2007Renderer)GlobalManager.Renderer).ColorTable is Office2010ColorTable))
			{
				((Office2007Renderer)GlobalManager.Renderer).ColorTable = new Office2010ColorTable();
			}
			break;
		case eDotNetBarStyle.Windows7:
			if (GlobalManager.Renderer is Office2007Renderer && !(((Office2007Renderer)GlobalManager.Renderer).ColorTable is Windows7ColorTable))
			{
				((Office2007Renderer)GlobalManager.Renderer).ColorTable = new Windows7ColorTable();
			}
			break;
		case eDotNetBarStyle.Office2007:
			if (GlobalManager.Renderer is Office2007Renderer && (((Office2007Renderer)GlobalManager.Renderer).ColorTable is Office2010ColorTable || ((Office2007Renderer)GlobalManager.Renderer).ColorTable is Windows7ColorTable))
			{
				((Office2007Renderer)GlobalManager.Renderer).ColorTable = new Office2007ColorTable(Office2007ColorTable);
			}
			break;
		}
		m_RibbonStrip.Style = newStyle;
		foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control val = item;
			if (!(val is RibbonPanel))
			{
				continue;
			}
			((RibbonPanel)(object)val).ColorSchemeStyle = newStyle;
			foreach (Control item2 in (ArrangedElementCollection)val.Controls)
			{
				Control val2 = item2;
				if (val2 is RibbonBar)
				{
					RibbonBar component = val2 as RibbonBar;
					TypeDescriptor.GetProperties(component)["Style"].SetValue(component, newStyle);
				}
			}
		}
		if (m_QatToolbar != null)
		{
			m_QatToolbar.Style = newStyle;
		}
		if (((Control)this).IsHandleCreated)
		{
			((Control)this).Invalidate(true);
			RecalcLayout();
		}
		if (!((Component)this).DesignMode && ((Control)this).FindForm() is RibbonForm ribbonForm)
		{
			ribbonForm.UpdateGlass();
		}
	}

	protected virtual void OnBeforeRibbonPanelPopupClose(RibbonPopupCloseEventArgs e)
	{
		if (this.BeforeRibbonPanelPopupClose != null)
		{
			this.BeforeRibbonPanelPopupClose(this, e);
		}
	}

	protected virtual void OnAfterRibbonPanelPopupClose(EventArgs e)
	{
		if (this.AfterRibbonPanelPopupClose != null)
		{
			this.AfterRibbonPanelPopupClose(this, e);
		}
	}

	protected virtual void OnBeforeRibbonPanelPopup(CancelEventArgs ce)
	{
		if (this.BeforeRibbonPanelPopup != null)
		{
			this.BeforeRibbonPanelPopup(this, ce);
		}
	}

	protected virtual void OnAfterRibbonPanelPopup(EventArgs e)
	{
		if (this.AfterRibbonPanelPopup != null)
		{
			this.AfterRibbonPanelPopup(this, e);
		}
	}

	internal void OnChildItemClick(BaseItem item)
	{
		m_RibbonStrip.OnChildItemClick(item);
		if (m_PopupMode && item != null && !(item is RibbonTabItem) && !item.IsContainer && !item.Name.StartsWith("sysgallery") && !(item is CheckBoxItem) && item.AutoCollapseOnClick && (!(item is PopupItem) || !item.Expanded))
		{
			CloseRibbonMenu(item, eEventSource.Mouse);
		}
	}

	private void SetupActiveWindowTimer()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		if (m_ActiveWindowTimer == null)
		{
			m_ActiveWindowTimer = new Timer();
			m_ActiveWindowTimer.Interval = 100;
			m_ActiveWindowTimer.Tick += ActiveWindowTimer_Tick;
			m_ForegroundWindow = NativeFunctions.GetForegroundWindow();
			m_ActiveWindow = NativeFunctions.GetActiveWindow();
			m_ActiveWindowTimer.Start();
		}
	}

	private void ActiveWindowTimer_Tick(object sender, EventArgs e)
	{
		if (m_ActiveWindowTimer == null)
		{
			return;
		}
		IntPtr foregroundWindow = NativeFunctions.GetForegroundWindow();
		IntPtr activeWindow = NativeFunctions.GetActiveWindow();
		if (!(foregroundWindow != m_ForegroundWindow) && !(activeWindow != m_ActiveWindow))
		{
			return;
		}
		if (activeWindow != IntPtr.Zero)
		{
			Control val = Control.FromHandle(activeWindow);
			if (val is PopupContainer || val is PopupContainerControl || val is Balloon)
			{
				return;
			}
		}
		m_ActiveWindowTimer.Stop();
		OnActiveWindowChanged();
	}

	private void OnActiveWindowChanged()
	{
		if (m_PopupMode)
		{
			CloseRibbonMenu(this, eEventSource.Code);
		}
		ReleaseActiveWindowTimer();
	}

	private void ReleaseActiveWindowTimer()
	{
		if (m_ActiveWindowTimer != null)
		{
			Timer activeWindowTimer = m_ActiveWindowTimer;
			m_ActiveWindowTimer = null;
			activeWindowTimer.Stop();
			activeWindowTimer.Tick -= ActiveWindowTimer_Tick;
			((Component)(object)activeWindowTimer).Dispose();
		}
	}

	protected override void OnVisibleChanged(EventArgs e)
	{
		if (!((Control)this).Visible)
		{
			OnActiveWindowChanged();
		}
		((ScrollableControl)this).OnVisibleChanged(e);
	}

	internal void OnEscapeKeyDown()
	{
		OnActiveWindowChanged();
	}

	internal void OnSysMouseDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		if (!m_PopupMode)
		{
			return;
		}
		Control val = Control.FromChildHandle(hWnd);
		if (val == null)
		{
			string className = NativeFunctions.GetClassName(hWnd);
			className = className.ToLower();
			if (className.IndexOf("combolbox") < 0)
			{
				OnActiveWindowChanged();
			}
			return;
		}
		do
		{
			if (val is MenuPanel || val is PopupContainer || val is PopupContainerControl || val is RibbonBar || val is RibbonStrip || val is RibbonPanel || val is Balloon)
			{
				return;
			}
			val = val.Parent;
		}
		while (val != null);
		OnActiveWindowChanged();
	}

	public void PopupRibbon(object source, eEventSource eventSource)
	{
		if (stopExpanded || Expanded || SelectedRibbonTabItem == null || SelectedRibbonTabItem.Panel == null || m_PopupRibbonPanel == SelectedRibbonTabItem.Panel)
		{
			return;
		}
		Control val = (Control)(object)((Control)this).FindForm();
		if (val == null)
		{
			val = ((Control)this).Parent;
		}
		if (val == null)
		{
			return;
		}
		RibbonPanel popupRibbonPanel = m_PopupRibbonPanel;
		if (popupRibbonPanel != null)
		{
			RibbonPopupCloseEventArgs ribbonPopupCloseEventArgs = new RibbonPopupCloseEventArgs(source, eventSource);
			OnBeforeRibbonPanelPopupClose(ribbonPopupCloseEventArgs);
			if (ribbonPopupCloseEventArgs.Cancel)
			{
				return;
			}
		}
		CancelEventArgs cancelEventArgs = new CancelEventArgs();
		OnBeforeRibbonPanelPopup(cancelEventArgs);
		if (!cancelEventArgs.Cancel)
		{
			m_PopupMode = true;
			RibbonPanel panel = SelectedRibbonTabItem.Panel;
			((Control)panel).SuspendLayout();
			((Control)this).Controls.Remove((Control)(object)panel);
			panel.Visible = false;
			((Control)panel).Dock = (DockStyle)0;
			val.Controls.Add((Control)(object)panel);
			Point point = ((Control)m_RibbonStrip).PointToScreen(new Point(0, ((Control)m_RibbonStrip).Height));
			point = val.PointToClient(point);
			((Control)panel).Font = ((Control)this).Font;
			((Control)panel).Bounds = new Rectangle(point.X, point.Y - DefaultBottomDockPadding - 1, ((Control)this).Width, GetPopupRibbonPanelHeight(panel));
			panel.SetPopupMode(popupMode: true, this);
			((Control)panel).ResumeLayout();
			panel.Visible = true;
			((Control)panel).BringToFront();
			CloseRibbonMenu(popupRibbonPanel);
			SetupActiveWindowTimer();
			m_PopupRibbonPanel = panel;
			OnAfterRibbonPanelPopup(new EventArgs());
		}
	}

	private void CloseRibbonMenu(RibbonPanel panel)
	{
		if (panel == null || ((Control)panel).IsDisposed || ((Control)panel).Parent == null)
		{
			return;
		}
		SuspendLayout();
		panel.Visible = false;
		((Control)panel).Parent.Controls.Remove((Control)(object)panel);
		panel.SetPopupMode(popupMode: false, this);
		((Control)panel).Dock = (DockStyle)5;
		((Control)this).Controls.Add((Control)(object)panel);
		if (SelectedRibbonTabItem != null)
		{
			SelectedRibbonTabItem.Refresh();
			if (SelectedRibbonTabItem.Panel == panel)
			{
				panel.Visible = true;
			}
		}
		((Control)panel).BringToFront();
		ResumeLayout(performLayout: true);
		OnAfterRibbonPanelPopupClose(new EventArgs());
	}

	public void CloseRibbonMenu()
	{
		CloseRibbonMenu(null, eEventSource.Code);
	}

	public void CloseRibbonMenu(object source, eEventSource eventSource)
	{
		if (m_PopupMode && m_PopupRibbonPanel != null)
		{
			RibbonPopupCloseEventArgs ribbonPopupCloseEventArgs = new RibbonPopupCloseEventArgs(source, eventSource);
			OnBeforeRibbonPanelPopupClose(ribbonPopupCloseEventArgs);
			if (!ribbonPopupCloseEventArgs.Cancel)
			{
				CloseRibbonMenu(m_PopupRibbonPanel);
				m_PopupRibbonPanel = null;
				m_PopupMode = false;
			}
		}
	}

	private int GetPopupRibbonPanelHeight(RibbonPanel panel)
	{
		if (((Control)this).AutoSize)
		{
			return GetAutoPanelHeight(panel);
		}
		int num = m_ExpandedHeight - ((Control)m_RibbonStrip).Height;
		if (m_ExpandedQatBelowRibbon)
		{
			num -= m_ExpandedQatHeight + 1;
		}
		return num;
	}

	internal void RibbonTabItemClick(RibbonTabItem rt)
	{
		if (Expanded)
		{
			return;
		}
		if (m_MenuTabsEnabled)
		{
			if (rt.Panel == m_PopupRibbonPanel)
			{
				CloseRibbonMenu(rt, eEventSource.Mouse);
			}
			else
			{
				PopupRibbon(rt, eEventSource.Mouse);
			}
		}
		else
		{
			Expanded = true;
		}
	}

	internal void RibbonTabItemDoubleClick(RibbonTabItem ribbonTabItem)
	{
		if (m_MenuTabsEnabled && !Expanded)
		{
			CloseRibbonMenu(ribbonTabItem, eEventSource.Code);
			Expanded = true;
		}
		else
		{
			Expanded = false;
		}
	}

	private void OnExpandedChanged()
	{
		if (m_Expanded && m_QatToolbar != null)
		{
			((Control)m_QatToolbar).SendToBack();
		}
		else if (!m_Expanded && m_QatToolbar != null)
		{
			((Control)m_QatToolbar).BringToFront();
		}
		((Control)m_RibbonStrip).Invalidate();
		if (((Control)this).AutoSize)
		{
			RecalculateAutoSize();
		}
		else
		{
			int num = 0;
			if (!Expanded)
			{
				num = GetCollapsedHeight();
				m_ExpandedQatBelowRibbon = m_QatPositionedBelow;
				if (m_QatPositionedBelow)
				{
					m_ExpandedQatHeight = ((Control)m_QatToolbar).Height;
				}
			}
			if (Expanded)
			{
				num = m_ExpandedHeight;
				if (!m_QatPositionedBelow && m_ExpandedQatBelowRibbon)
				{
					num -= m_ExpandedQatHeight;
				}
				else if (m_QatPositionedBelow && !m_ExpandedQatBelowRibbon)
				{
					num += ((Control)m_QatToolbar).Height;
				}
			}
			else
			{
				m_ExpandedHeight = ((Control)this).Height;
			}
			if (((Control)this).Height != num && num > 0)
			{
				((Control)this).Height = num;
			}
		}
		if (EffectiveStyle == eDotNetBarStyle.Metro && ((Control)this).FindForm() is RibbonForm ribbonForm)
		{
			BaseItem applicationButton = GetApplicationButton();
			if (applicationButton != null)
			{
				((Control)ribbonForm).Invalidate(new Rectangle(0, applicationButton.TopInternal, ((Control)ribbonForm).Width, applicationButton.HeightInternal + 16));
			}
		}
		if (this.ExpandedChanged != null)
		{
			this.ExpandedChanged(this, new EventArgs());
		}
	}

	public void StopExpanded()
	{
		prevExpanded = Expanded;
		Expanded = false;
		stopExpanded = true;
	}

	public void ReleaseExpanded()
	{
		if (stopExpanded)
		{
			stopExpanded = false;
			Expanded = prevExpanded;
		}
	}

	private int GetCollapsedHeight()
	{
		int num = ((Control)m_RibbonStrip).Height;
		if (m_QatPositionedBelow)
		{
			num += ((Control)m_QatToolbar).Height;
		}
		return num;
	}

	public int GetQatToolbarHeight()
	{
		if (m_QatPositionedBelow)
		{
			return ((Control)m_QatToolbar).Height;
		}
		return 0;
	}

	protected override void OnTabStopChanged(EventArgs e)
	{
		((Control)this).OnTabStopChanged(e);
		((Control)m_RibbonStrip).TabStop = TabStop;
	}

	public void SuspendLayout()
	{
		((Control)this).SuspendLayout();
		m_IsLayoutSuspended = true;
	}

	public void ResumeLayout()
	{
		m_IsLayoutSuspended = false;
		if (m_SetColorTableDelayed)
		{
			m_SetColorTableDelayed = false;
			Office2007ColorTable = m_DelayedColorTableChange;
		}
		((Control)this).ResumeLayout();
	}

	public void ResumeLayout(bool performLayout)
	{
		m_IsLayoutSuspended = false;
		if (m_SetColorTableDelayed)
		{
			m_SetColorTableDelayed = false;
			Office2007ColorTable = m_DelayedColorTableChange;
		}
		((Control)this).ResumeLayout(performLayout);
	}

	private void ReleaseParentForm()
	{
		Form parentForm = _ParentForm;
		_ParentForm = null;
		if (parentForm != null)
		{
			parentForm.MdiChildActivate -= ParentFormMdiChildActivate;
		}
	}

	protected override void OnParentChanged(EventArgs e)
	{
		ReleaseParentForm();
		((ContainerControl)this).OnParentChanged(e);
		Form val = ((Control)this).FindForm();
		if (val != null)
		{
			val.MdiChildActivate += ParentFormMdiChildActivate;
			_ParentForm = val;
		}
		if (m_SetColorTableDelayed && !m_IsLayoutSuspended)
		{
			m_SetColorTableDelayed = false;
			Office2007ColorTable = m_DelayedColorTableChange;
		}
	}

	private void ParentFormMdiChildActivate(object sender, EventArgs e)
	{
		Form val = ((Control)this).FindForm();
		if (val == null)
		{
			return;
		}
		Form activeMdiChild = val.ActiveMdiChild;
		if (m_MdiSystemItemVisible)
		{
			if (activeMdiChild != null)
			{
				((Control)activeMdiChild).Resize += MdiChildFormResize;
				((Control)activeMdiChild).VisibleChanged += MdiChildVisibleChanged;
			}
			if (m_ActiveMdiChild != null)
			{
				((Control)m_ActiveMdiChild).Resize -= MdiChildFormResize;
				((Control)m_ActiveMdiChild).VisibleChanged -= MdiChildVisibleChanged;
			}
			m_MdiChildMaximized = false;
			m_ActiveMdiChild = activeMdiChild;
			SyncMdiChildSystemItem();
		}
		UpdateMerging();
	}

	private void UpdateMerging()
	{
		if (!m_AllowMerge)
		{
			return;
		}
		Form val = ((Control)this).FindForm();
		if (val == null)
		{
			return;
		}
		Form activeMdiChild = val.ActiveMdiChild;
		if (activeMdiChild == m_PreviousMergedForm)
		{
			return;
		}
		bool flag = false;
		SuspendLayout();
		m_RibbonStrip.BeginUpdate();
		if (m_PreviousMergedForm != null)
		{
			ArrayList mergeContainers = GetMergeContainers(m_PreviousMergedForm);
			foreach (RibbonBarMergeContainer item in mergeContainers)
			{
				if (item.RibbonTabItem == SelectedRibbonTabItem)
				{
					flag = true;
				}
				item.RemoveMergedRibbonBars(this);
			}
			m_PreviousMergedForm = null;
		}
		if (activeMdiChild != null)
		{
			ArrayList mergeContainers2 = GetMergeContainers(activeMdiChild);
			if (mergeContainers2.Count > 0)
			{
				bool flag2 = true;
				foreach (RibbonBarMergeContainer item2 in mergeContainers2)
				{
					item2.MergeRibbonBars(this);
					if (flag2 && item2.RibbonTabItem != null && item2.AutoActivateTab)
					{
						item2.RibbonTabItem.Checked = true;
						flag = false;
					}
				}
				m_PreviousMergedForm = activeMdiChild;
			}
		}
		if (flag)
		{
			SelectFirstVisibleRibbonTab();
		}
		m_RibbonStrip.EndUpdate(callRecalcLayout: false);
		ResumeLayout(performLayout: true);
		RecalcLayout();
	}

	public bool SelectFirstVisibleRibbonTab()
	{
		foreach (BaseItem item in Items)
		{
			if (item is RibbonTabItem && item.Visible)
			{
				((RibbonTabItem)item).Checked = true;
				return true;
			}
		}
		return false;
	}

	private ArrayList GetMergeContainers(Form form)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		ArrayList arrayList = new ArrayList();
		foreach (Control item in (ArrangedElementCollection)((Control)form).Controls)
		{
			Control val = item;
			if (val is RibbonBarMergeContainer && ((RibbonBarMergeContainer)(object)val).AllowMerge)
			{
				arrayList.Add(val);
			}
		}
		return arrayList;
	}

	private void MdiChildVisibleChanged(object sender, EventArgs e)
	{
		SyncMdiChildSystemItem();
	}

	private void MdiChildFormResize(object sender, EventArgs e)
	{
		SyncMdiChildSystemItem();
	}

	private void SyncMdiChildSystemItem()
	{
		if (m_MdiSystemItemVisible && m_ActiveMdiChild != null && IsMaximized(m_ActiveMdiChild) && ((Control)m_ActiveMdiChild).Visible)
		{
			if (!m_MdiChildMaximized)
			{
				m_RibbonStrip.ShowMDIChildSystemItems(m_ActiveMdiChild, bRecalcLayout: true);
				m_MdiChildMaximized = true;
			}
		}
		else
		{
			m_RibbonStrip.ClearMDIChildSystemItems(bRecalcLayout: true);
			m_MdiChildMaximized = false;
		}
	}

	private bool IsMaximized(Form form)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		if ((int)form.WindowState == 2)
		{
			if (form.IsMdiChild && !WinApi.IsZoomed(((Control)form).Handle))
			{
				return false;
			}
			return true;
		}
		return false;
	}

	internal void OnRibbonBarRightClick(RibbonBar ribbonBar, int x, int y)
	{
		if (!DisplayCustomizeContextMenu)
		{
			return;
		}
		if (ribbonBar.CanCustomize && (ribbonBar.TitleRectangle.Contains(x, y) || ribbonBar.OverflowState))
		{
			ShowCustomizeContextMenu(ribbonBar, ribbonStrip: false);
			return;
		}
		BaseItem baseItem = ribbonBar.HitTest(x, y);
		if (baseItem != null && baseItem.CanCustomize && !baseItem.SystemItem)
		{
			ShowCustomizeContextMenu(baseItem, ribbonStrip: false);
		}
	}

	internal void OnRibbonStripRightClick(RibbonStrip ribbonStrip, int x, int y)
	{
		if (DisplayCustomizeContextMenu)
		{
			BaseItem baseItem = ribbonStrip.HitTest(x, y);
			if (baseItem != null && baseItem.CanCustomize && !baseItem.SystemItem)
			{
				ShowCustomizeContextMenu(baseItem, ribbonStrip: true);
			}
		}
	}

	internal virtual void ShowCustomizeContextMenu(object o, bool ribbonStrip)
	{
		if (o == null || !m_UseCustomizeDialog)
		{
			return;
		}
		m_RibbonStrip.ClosePopup("syscustomizepopupmenu");
		ButtonItem buttonItem = new ButtonItem("syscustomizepopupmenu");
		buttonItem.Style = eDotNetBarStyle.Office2007;
		buttonItem.SetOwner(m_RibbonStrip);
		if ((CanCustomizeItem(o as BaseItem) || o is RibbonBar) && !m_UseExternalCustomization)
		{
			if (o is BaseItem && QuickToolbarItems.Contains((BaseItem)o))
			{
				ButtonItem buttonItem2 = new ButtonItem(SysQatRemoveFromItemName);
				buttonItem2.Text = SystemText.QatRemoveItemText;
				buttonItem2.Click += CustomizeRemoveFromQuickAccessToolbar;
				buttonItem2.Tag = o;
				buttonItem.SubItems.Add(buttonItem2);
			}
			else
			{
				BaseItem baseItem = o as BaseItem;
				ButtonItem buttonItem3 = new ButtonItem(SysQatAddToItemName);
				buttonItem3.Text = SystemText.QatAddItemText;
				buttonItem3.Click += CustomizeAddToQuickAccessToolbar;
				buttonItem3.Tag = o;
				buttonItem.SubItems.Add(buttonItem3);
				if ((baseItem != null && QuickToolbarItems.Contains(baseItem.Name)) || (o is RibbonBar && QuickToolbarItems.Contains(GetQATRibbonBarName(o as RibbonBar))))
				{
					buttonItem3.Enabled = false;
				}
				if (baseItem != null && BaseItem.IsOnPopup(baseItem) && baseItem.Parent != null)
				{
					object containerControl = baseItem.ContainerControl;
					Control val = (Control)((containerControl is Control) ? containerControl : null);
					if (val != null)
					{
						val.VisibleChanged += CustomizePopupItemParentVisibleChange;
					}
				}
			}
		}
		if (m_UseCustomizeDialog)
		{
			ButtonItem buttonItem4 = new ButtonItem(SysQatCustomizeItemName);
			buttonItem4.Text = SystemText.QatCustomizeText;
			buttonItem4.BeginGroup = true;
			buttonItem4.Click += CustomizeQuickAccessToolbarDialog;
			buttonItem.SubItems.Add(buttonItem4);
		}
		if (m_EnableQatPlacement && !m_UseExternalCustomization)
		{
			ButtonItem buttonItem5 = new ButtonItem(SysQatPlaceItemName);
			if (m_QatPositionedBelow)
			{
				buttonItem5.Text = SystemText.QatPlaceAboveRibbonText;
			}
			else
			{
				buttonItem5.Text = SystemText.QatPlaceBelowRibbonText;
			}
			buttonItem5.Click += QuickAccessToolbarChangePlacement;
			buttonItem.SubItems.Add(buttonItem5);
		}
		if (AutoExpand)
		{
			ButtonItem buttonItem6 = new ButtonItem(Expanded ? SysMinimizeRibbon : SysMaximizeRibbon, Expanded ? SystemText.MinimizeRibbonText : SystemText.MaximizeRibbonText);
			buttonItem6.Click += MinMaxRibbonClick;
			buttonItem6.BeginGroup = true;
			buttonItem.SubItems.Add(buttonItem6);
		}
		RibbonCustomizeEventArgs ribbonCustomizeEventArgs = new RibbonCustomizeEventArgs(o, buttonItem);
		OnBeforeCustomizeMenuPopup(ribbonCustomizeEventArgs);
		if (ribbonCustomizeEventArgs.Cancel)
		{
			buttonItem.Dispose();
			return;
		}
		((IOwnerMenuSupport)m_RibbonStrip).RegisterPopup((PopupItem)buttonItem);
		buttonItem.Popup(Control.MousePosition);
	}

	private void MinMaxRibbonClick(object sender, EventArgs e)
	{
		Expanded = !Expanded;
	}

	private void CustomizePopupItemParentVisibleChange(object sender, EventArgs e)
	{
		Control val = (Control)((sender is Control) ? sender : null);
		if (val != null)
		{
			val.VisibleChanged -= CustomizePopupItemParentVisibleChange;
			m_RibbonStrip.ClosePopup("syscustomizepopupmenu");
		}
	}

	private void QuickAccessToolbarChangePlacement(object sender, EventArgs e)
	{
		QuickAccessToolbarChangePlacement();
	}

	internal void QuickAccessToolbarChangePlacement()
	{
		m_RibbonStrip.ClosePopups();
		if (m_QatPositionedBelow)
		{
			m_QatToolbar.ClosePopups();
			((Control)m_QatToolbar).Visible = false;
			ArrayList arrayList = new ArrayList(m_QatToolbar.Items.Count);
			m_QatToolbar.Items.CopyTo(arrayList);
			m_QatToolbar.Items.Clear();
			foreach (BaseItem item3 in arrayList)
			{
				m_RibbonStrip.CaptionContainerItem.SubItems.Add(item3);
			}
			if (m_QatSubItemsCollection != null)
			{
				m_QatSubItemsCollection._Clear();
				m_QatSubItemsCollection = null;
			}
			((ScrollableControl)this).DockPadding.Bottom = DefaultBottomDockPadding;
			m_QatPositionedBelow = false;
			m_RibbonStrip.RecalcLayout();
			if (m_AutoSize)
			{
				AutoSyncSize();
			}
			else
			{
				((Control)this).Height = ((Control)this).Height - (((Control)m_QatToolbar).Height + ((ScrollableControl)m_QatToolbar).DockPadding.Top);
			}
			((Component)(object)m_QatToolbar).Dispose();
			m_QatToolbar = null;
		}
		else
		{
			if (m_QatToolbar == null)
			{
				CreateQatToolbar();
			}
			((ScrollableControl)this).DockPadding.Bottom = 1;
			m_QatToolbar.BeginUpdate();
			ArrayList qatItems = GetQatItems(includeCustomizeItem: true);
			foreach (BaseItem item4 in qatItems)
			{
				QuickToolbarItems.Remove(item4);
				m_QatToolbar.Items.Add(item4);
			}
			m_QatToolbar.EndUpdate();
			((Control)m_QatToolbar).Height = m_QatToolbar.GetAutoSizeHeight();
			m_QatPositionedBelow = true;
			if (m_AutoSize)
			{
				AutoSyncSize();
			}
			else
			{
				((Control)this).Height = ((Control)this).Height + (((Control)m_QatToolbar).Height + ((ScrollableControl)m_QatToolbar).DockPadding.Top);
			}
		}
		RecalcLayout();
		m_QatLayoutChanged = true;
		OnQatPlacementChanged(EventArgs.Empty);
		((Control)((Control)this).FindForm()).Invalidate();
	}

	protected virtual void OnQatPlacementChanged(EventArgs e)
	{
		this.QatPlacementChanged?.Invoke(this, e);
	}

	private ArrayList GetQatItems(bool includeCustomizeItem)
	{
		ArrayList arrayList = new ArrayList();
		foreach (BaseItem quickToolbarItem in QuickToolbarItems)
		{
			if (IsSystemItem(quickToolbarItem) || (quickToolbarItem is ButtonItem && ((ButtonItem)quickToolbarItem).HotTrackingStyle == eHotTrackingStyle.Image && QuickToolbarItems.IndexOf(quickToolbarItem) == 0))
			{
				if (includeCustomizeItem && quickToolbarItem is CustomizeItem)
				{
					arrayList.Add(quickToolbarItem);
				}
			}
			else
			{
				arrayList.Add(quickToolbarItem);
			}
		}
		return arrayList;
	}

	private void CreateQatToolbar()
	{
		m_QatToolbar = new QatToolbar();
		m_QatToolbar.FadeEffect = m_RibbonStrip.FadeEffect;
		m_QatToolbar.Style = Style;
		((Control)m_QatToolbar).Dock = (DockStyle)2;
		((Control)this).Controls.Add((Control)(object)m_QatToolbar);
		if (!Expanded)
		{
			((Control)m_QatToolbar).BringToFront();
		}
	}

	private void CustomizeQuickAccessToolbarDialog(object sender, EventArgs e)
	{
		ShowQatCustomizeDialog();
	}

	private bool CanCustomizeItem(BaseItem item)
	{
		if (item == null)
		{
			return false;
		}
		if (!item.CanCustomize || item.SystemItem)
		{
			return false;
		}
		if (item is ItemContainer)
		{
			return false;
		}
		if (item is ButtonItem && QuickToolbarItems.IndexOf(item) == 0)
		{
			return false;
		}
		return true;
	}

	private string GetQATRibbonBarName(RibbonBar bar)
	{
		return "qt_" + ((Control)bar).Name;
	}

	private void CustomizeRemoveFromQuickAccessToolbar(object sender, EventArgs e)
	{
		ButtonItem buttonItem = sender as ButtonItem;
		if (buttonItem.Tag is BaseItem)
		{
			BaseItem item = buttonItem.Tag as BaseItem;
			RemoveItemFromQuickAccessToolbar(item);
		}
		buttonItem.Tag = null;
	}

	public void RemoveItemFromQuickAccessToolbar(BaseItem item)
	{
		RibbonCustomizeEventArgs ribbonCustomizeEventArgs = new RibbonCustomizeEventArgs(item, null);
		OnBeforeRemoveItemFromQuickAccessToolbar(ribbonCustomizeEventArgs);
		if (!ribbonCustomizeEventArgs.Cancel)
		{
			QuickToolbarItems.Remove(item);
			RecalcLayout();
			m_QatLayoutChanged = true;
			OnAfterRemoveItemFromQuickAccessToolbar(ribbonCustomizeEventArgs);
		}
	}

	private void CustomizeAddToQuickAccessToolbar(object sender, EventArgs e)
	{
		ButtonItem buttonItem = sender as ButtonItem;
		AddItemToQuickAccessToolbar(buttonItem.Tag);
		buttonItem.Tag = null;
	}

	public void AddItemToQuickAccessToolbar(object originalItem)
	{
		BaseItem qatItemCopy = GetQatItemCopy(originalItem);
		if (qatItemCopy != null)
		{
			RibbonCustomizeEventArgs ribbonCustomizeEventArgs = new RibbonCustomizeEventArgs(qatItemCopy, null);
			OnBeforeAddItemToQuickAccessToolbar(ribbonCustomizeEventArgs);
			if (!ribbonCustomizeEventArgs.Cancel)
			{
				QuickToolbarItems.Add(qatItemCopy);
				RecalcLayout();
				m_QatLayoutChanged = true;
				OnAfterAddItemToQuickAccessToolbar(ribbonCustomizeEventArgs);
			}
		}
	}

	private BaseItem GetQatItemCopy(object o)
	{
		BaseItem baseItem = null;
		if (o is ButtonItem)
		{
			ButtonItem buttonItem = ((ButtonItem)o).Copy() as ButtonItem;
			buttonItem.KeyTips = "";
			buttonItem.ImageFixedSize = Size.Empty;
			buttonItem.FixedSize = Size.Empty;
			buttonItem.ButtonStyle = eButtonStyle.Default;
			buttonItem.ImagePosition = eImagePosition.Left;
			buttonItem.BeginGroup = false;
			buttonItem.UseSmallImage = true;
			if (buttonItem.TextMarkupBody != null && buttonItem.TextMarkupBody.HasExpandElement)
			{
				buttonItem.Text = MarkupParser.RemoveExpand(buttonItem.Text);
			}
			if (buttonItem.Image == null && buttonItem.ImageIndex == -1 && buttonItem.Icon == null)
			{
				buttonItem.Image = (Image)(object)BarFunctions.LoadBitmap("SystemImages.GreenLight.png");
			}
			else
			{
				buttonItem.ImageFixedSize = new Size(16, 16);
			}
			baseItem = buttonItem;
		}
		else if (o is BaseItem)
		{
			baseItem = ((BaseItem)o).Copy();
			baseItem.KeyTips = "";
		}
		else if (o is RibbonBar)
		{
			RibbonBar ribbonBar = o as RibbonBar;
			ButtonItem buttonItem2 = new ButtonItem(GetQATRibbonBarName(ribbonBar));
			buttonItem2.Image = ribbonBar.GetOverflowButtonImage();
			buttonItem2.ImageFixedSize = new Size(16, 16);
			buttonItem2.AutoExpandOnClick = true;
			buttonItem2.Tooltip = ((Control)ribbonBar).Text;
			buttonItem2.Text = ((Control)ribbonBar).Text;
			RibbonBar ribbonBar2 = null;
			bool flag = false;
			if (ribbonBar.IsItemsAutoSizeActive)
			{
				ribbonBar.RestoreAutoSizedItems();
				ribbonBar2 = ribbonBar.CreateOverflowRibbon(createHandle: true);
				flag = true;
			}
			else
			{
				ribbonBar2 = ribbonBar.CreateOverflowRibbon(createHandle: true);
			}
			ribbonBar2.IsOnQat = true;
			ribbonBar2.QatButtonParent = buttonItem2;
			foreach (BaseItem item2 in ribbonBar.Items)
			{
				BaseItem item = item2.Copy();
				ribbonBar2.Items.Add(item);
			}
			if (flag)
			{
				ribbonBar.RecalcLayout();
			}
			ItemContainer itemContainer = new ItemContainer();
			itemContainer.Stretch = true;
			ControlContainerItem controlContainerItem = new ControlContainerItem();
			controlContainerItem.AllowItemResize = false;
			itemContainer.SubItems.Add(controlContainerItem);
			controlContainerItem.Control = (Control)(object)ribbonBar2;
			buttonItem2.SubItems.Add(itemContainer);
			baseItem = buttonItem2;
		}
		return baseItem;
	}

	protected virtual void OnBeforeCustomizeMenuPopup(RibbonCustomizeEventArgs e)
	{
		if (this.BeforeCustomizeMenuPopup != null)
		{
			this.BeforeCustomizeMenuPopup(this, e);
		}
	}

	protected virtual void OnBeforeAddItemToQuickAccessToolbar(RibbonCustomizeEventArgs e)
	{
		if (this.BeforeAddItemToQuickAccessToolbar != null)
		{
			this.BeforeAddItemToQuickAccessToolbar(this, e);
		}
	}

	protected virtual void OnBeforeRemoveItemFromQuickAccessToolbar(RibbonCustomizeEventArgs e)
	{
		if (this.BeforeRemoveItemFromQuickAccessToolbar != null)
		{
			this.BeforeRemoveItemFromQuickAccessToolbar(this, e);
		}
	}

	protected virtual void OnAfterAddItemToQuickAccessToolbar(RibbonCustomizeEventArgs e)
	{
		if (this.AfterAddItemToQuickAccessToolbar != null)
		{
			this.AfterAddItemToQuickAccessToolbar(this, e);
		}
	}

	protected virtual void OnAfterRemoveItemFromQuickAccessToolbar(RibbonCustomizeEventArgs e)
	{
		if (this.AfterRemoveItemFromQuickAccessToolbar != null)
		{
			this.AfterRemoveItemFromQuickAccessToolbar(this, e);
		}
	}

	public virtual void ShowQatCustomizeDialog()
	{
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Invalid comparison between Unknown and I4
		Form val = (Form)(object)CreateQatCustomizeDialog();
		if (this.BeforeQatCustomizeDialog != null)
		{
			QatCustomizeDialogEventArgs qatCustomizeDialogEventArgs = new QatCustomizeDialogEventArgs(val);
			this.BeforeQatCustomizeDialog(this, qatCustomizeDialogEventArgs);
			if (qatCustomizeDialogEventArgs.Cancel || qatCustomizeDialogEventArgs.Dialog == null)
			{
				((Component)(object)val).Dispose();
				return;
			}
			val = qatCustomizeDialogEventArgs.Dialog;
		}
		if (val is QatCustomizeDialog)
		{
			((QatCustomizeDialog)(object)val).LoadItems(this);
		}
		Form val2 = ((Control)this).FindForm();
		val.StartPosition = (FormStartPosition)4;
		DialogResult val3 = val.ShowDialog((IWin32Window)(object)val2);
		if (this.AfterQatCustomizeDialog != null)
		{
			QatCustomizeDialogEventArgs qatCustomizeDialogEventArgs2 = new QatCustomizeDialogEventArgs(val);
			this.AfterQatCustomizeDialog(this, qatCustomizeDialogEventArgs2);
			if (qatCustomizeDialogEventArgs2.Cancel)
			{
				((Component)(object)val).Dispose();
				return;
			}
		}
		if ((int)val3 == 2)
		{
			((Component)(object)val).Dispose();
		}
		else if (val is QatCustomizeDialog qatCustomizeDialog && qatCustomizeDialog.QatCustomizePanel.DataChanged)
		{
			ApplyQatCustomizePanelChanges(qatCustomizeDialog.QatCustomizePanel);
			((Component)(object)val).Dispose();
		}
	}

	public void ApplyQatCustomizePanelChanges(QatCustomizePanel customizePanel)
	{
		if (customizePanel == null || !customizePanel.DataChanged)
		{
			return;
		}
		m_RibbonStrip.BeginUpdate();
		try
		{
			ItemPanel itemPanelQat = customizePanel.itemPanelQat;
			int num = 0;
			BaseItem applicationButton = GetApplicationButton();
			if (applicationButton != null)
			{
				num = QuickToolbarItems.IndexOf(applicationButton) + 1;
			}
			ArrayList arrayList = new ArrayList();
			SubItemsCollection subItemsCollection = new SubItemsCollection(null);
			for (int i = num; i < QuickToolbarItems.Count; i++)
			{
				BaseItem baseItem = QuickToolbarItems[i];
				if (!IsSystemItem(baseItem))
				{
					if (!itemPanelQat.Items.Contains(baseItem.Name))
					{
						arrayList.Add(baseItem);
					}
					else
					{
						subItemsCollection._Add(baseItem);
					}
				}
			}
			foreach (BaseItem item3 in arrayList)
			{
				QuickToolbarItems.Remove(item3);
			}
			foreach (BaseItem item4 in subItemsCollection)
			{
				QuickToolbarItems.Remove(item4);
			}
			foreach (BaseItem item5 in itemPanelQat.Items)
			{
				if (item5.Tag != null)
				{
					BaseItem qatItemCopy = GetQatItemCopy(item5.Tag as BaseItem);
					QuickToolbarItems.Add(qatItemCopy);
					continue;
				}
				BaseItem baseItem3 = subItemsCollection[item5.Name];
				if (baseItem3 != null)
				{
					QuickToolbarItems.Add(baseItem3);
				}
			}
			m_QatLayoutChanged = true;
		}
		finally
		{
			m_RibbonStrip.EndUpdate();
		}
		if (customizePanel.checkQatBelowRibbon.Checked != m_QatPositionedBelow)
		{
			QuickAccessToolbarChangePlacement();
		}
		OnAfterQatDialogChangesApplied();
	}

	protected virtual void OnAfterQatDialogChangesApplied()
	{
		if (this.AfterQatDialogChangesApplied != null)
		{
			this.AfterQatDialogChangesApplied(this, new EventArgs());
		}
	}

	private QatCustomizeDialog CreateQatCustomizeDialog()
	{
		QatCustomizeDialog qatCustomizeDialog = new QatCustomizeDialog();
		((Control)qatCustomizeDialog).Text = SystemText.QatDialogCaption;
		((Control)qatCustomizeDialog.buttonCancel).Text = SystemText.QatDialogCancelButton;
		((Control)qatCustomizeDialog.buttonOK).Text = SystemText.QatDialogOkButton;
		((Control)qatCustomizeDialog.QatCustomizePanel.buttonAddToQat).Text = SystemText.QatDialogAddButton;
		((Control)qatCustomizeDialog.QatCustomizePanel.buttonRemoveFromQat).Text = SystemText.QatDialogRemoveButton;
		((Control)qatCustomizeDialog.QatCustomizePanel.labelCategories).Text = SystemText.QatDialogCategoriesLabel;
		((Control)qatCustomizeDialog.QatCustomizePanel.checkQatBelowRibbon).Text = SystemText.QatDialogPlacementCheckbox;
		return qatCustomizeDialog;
	}

	private bool IsSystemItem(BaseItem item)
	{
		if (item.SystemItem || item is ItemContainer || item is CustomizeItem || item is SystemCaptionItem)
		{
			return true;
		}
		return false;
	}

	public BaseItem GetApplicationButton()
	{
		return m_RibbonStrip.GetApplicationButton();
	}

	private string GetQatLayoutDescription()
	{
		ArrayList qatItems = GetQatItems(includeCustomizeItem: false);
		StringBuilder stringBuilder = new StringBuilder();
		if (m_QatPositionedBelow)
		{
			stringBuilder.Append("1");
		}
		else
		{
			stringBuilder.Append("0");
		}
		foreach (BaseItem item in qatItems)
		{
			if (item.Name != "")
			{
				stringBuilder.Append("," + item.Name);
			}
		}
		return stringBuilder.ToString();
	}

	private void SetQatLayoutDescription(string layoutDesc)
	{
		if (layoutDesc == "" || layoutDesc == null)
		{
			return;
		}
		string[] array = layoutDesc.Split(new char[1] { ',' });
		if (array.Length == 0)
		{
			return;
		}
		bool flag = array[0] == "1";
		ArrayList arrayList = new ArrayList();
		for (int i = 1; i < array.Length; i++)
		{
			if (QuickToolbarItems.Contains(array[i]))
			{
				arrayList.Add(QuickToolbarItems[array[i]]);
			}
			else if (array[i].StartsWith("qt_"))
			{
				RibbonBar ribbonBar = GetRibbonBar(array[i].Substring(3));
				if (ribbonBar != null)
				{
					arrayList.Add(GetQatItemCopy(ribbonBar));
				}
			}
			else
			{
				BaseItem item = m_RibbonStrip.GetItem(array[i]);
				if (item != null && CanCustomizeItem(item))
				{
					arrayList.Add(GetQatItemCopy(item));
				}
			}
		}
		ArrayList qatItems = GetQatItems(includeCustomizeItem: false);
		foreach (BaseItem item4 in qatItems)
		{
			QuickToolbarItems.Remove(item4);
		}
		foreach (BaseItem item5 in arrayList)
		{
			QuickToolbarItems.Add(item5);
		}
		if (m_QatPositionedBelow != flag)
		{
			QuickAccessToolbarChangePlacement();
		}
		m_QatLayoutChanged = false;
	}

	private RibbonBar GetRibbonBar(string name)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control parent = item;
			RibbonBar ribbonBar = GetRibbonBar(parent, name);
			if (ribbonBar != null)
			{
				return ribbonBar;
			}
		}
		return null;
	}

	private RibbonBar GetRibbonBar(Control parent, string name)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		if (parent is RibbonBar && parent.Name == name)
		{
			return parent as RibbonBar;
		}
		foreach (Control item in (ArrangedElementCollection)parent.Controls)
		{
			Control parent2 = item;
			RibbonBar ribbonBar = GetRibbonBar(parent2, name);
			if (ribbonBar != null)
			{
				return ribbonBar;
			}
		}
		return null;
	}
}
