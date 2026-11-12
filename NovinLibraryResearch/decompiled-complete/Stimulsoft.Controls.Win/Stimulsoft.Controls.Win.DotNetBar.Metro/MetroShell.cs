using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;
using Stimulsoft.Controls.Win.DotNetBar.Ribbon;
using Stimulsoft.Controls.Win.DotNetBar.TextMarkup;

namespace Stimulsoft.Controls.Win.DotNetBar.Metro;

[ToolboxBitmap(typeof(MetroShell), "MetroShell.ico")]
[ToolboxItem(false)]
public class MetroShell : ContainerControl
{
	private MetroTabStrip _TabStrip;

	private bool _UseCustomizeDialog = true;

	private eMetroCategorizeMode m_CategorizeMode;

	private QatToolbar m_QatToolbar;

	private int DefaultBottomDockPadding;

	private bool m_QatLayoutChanged;

	private RibbonLocalization m_SystemText = new RibbonLocalization();

	private bool _UseExternalCustomization;

	private const string SYS_CUSTOMIZE_POPUP_MENU = "syscustomizepopupmenu";

	private ContextMenuBar m_GlobalContextMenuBar;

	private SubItemsCollection _QatFrequentCommands = new SubItemsCollection(null);

	public static readonly string SysQatCustomizeItemName = "sysCustomizeQuickAccessToolbar";

	public static readonly string SysQatAddToItemName = "sysAddToQuickAccessToolbar";

	public static readonly string SysQatRemoveFromItemName = "sysRemoveFromQuickAccessToolbar";

	public static readonly string SysQatPlaceItemName = "sysPlaceQuickAccessToolbar";

	public static readonly string SysQatCustomizeLabelName = "sysCustomizeQuickAccessToolbarLabel";

	public static readonly string SysFrequentlyQatNamePart = "sysQatFrequent_";

	private bool _MouseWheelTabScrollEnabled = true;

	private List<WeakReference> _Toolbars = new List<WeakReference>();

	private eMetroTabStripDock _TabStripDock;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public SubItemsCollection QatFrequentCommands => _QatFrequentCommands;

	[DefaultValue(true)]
	[Browsable(true)]
	[Category("Behavior")]
	[Description("Indicates whether KeyTips functionality is enabled.")]
	public bool KeyTipsEnabled
	{
		get
		{
			return _TabStrip.KeyTipsEnabled;
		}
		set
		{
			_TabStrip.KeyTipsEnabled = value;
		}
	}

	internal bool IsDesignMode => ((Component)this).DesignMode;

	[DefaultValue(true)]
	[Browsable(true)]
	[Category("Appearance")]
	[Description("Gets or sets whether anti-aliasing is used while painting.")]
	public bool AntiAlias
	{
		get
		{
			return _TabStrip.AntiAlias;
		}
		set
		{
			_TabStrip.AntiAlias = value;
			((Control)this).Invalidate();
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.TextMarkupUIEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[EditorBrowsable(EditorBrowsableState.Always)]
	[Category("Appearance")]
	[Description("Indicates text displayed on Title bar instead of the Form.Text property.")]
	public string TitleText
	{
		get
		{
			return _TabStrip.TitleText;
		}
		set
		{
			_TabStrip.TitleText = value;
		}
	}

	[DefaultValue(null)]
	[Description("Indicates Context menu bar associated with the MetroShell control which is used as part of Global Items feature.")]
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
			return _TabStrip.CaptionVisible;
		}
		set
		{
			_TabStrip.CaptionVisible = value;
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
			return _TabStrip.CaptionFont;
		}
		set
		{
			_TabStrip.CaptionFont = value;
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
			return _TabStrip.CaptionHeight;
		}
		set
		{
			_TabStrip.CaptionHeight = value;
			RecalcLayout();
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
			return _TabStrip.KeyTipsFont;
		}
		set
		{
			_TabStrip.KeyTipsFont = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Style")]
	[Description("Gets or sets bar background style.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ElementStyle BackgroundStyle => _TabStrip.BackgroundStyle;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public MetroTabItem SelectedTab
	{
		get
		{
			return _TabStrip.SelectedTab;
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
	public MetroTabStrip MetroTabStrip => _TabStrip;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(false)]
	public SubItemsCollection Items => _TabStrip.Items;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(false)]
	public SubItemsCollection QuickToolbarItems => GetQatSubItemsCollection();

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public eDotNetBarStyle EffectiveStyle => _TabStrip.EffectiveStyle;

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether mouse wheel scrolls through the Metro tabs.")]
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

	[Browsable(true)]
	[Category("Data")]
	[DefaultValue(null)]
	[Description("ImageList for images used on Items. Images specified here will always be used on menu-items and are by default used on all Bars.")]
	public ImageList Images
	{
		get
		{
			return _TabStrip.Images;
		}
		set
		{
			_TabStrip.Images = value;
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
			return _TabStrip.ImagesMedium;
		}
		set
		{
			_TabStrip.ImagesMedium = value;
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
			return _TabStrip.ImagesLarge;
		}
		set
		{
			_TabStrip.ImagesLarge = value;
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

	private bool DisplayCustomizeContextMenu
	{
		get
		{
			if (CanCustomize)
			{
				if (!CaptionVisible)
				{
					return _UseExternalCustomization;
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
			return _TabStrip.CanCustomize;
		}
		set
		{
			_TabStrip.CanCustomize = value;
		}
	}

	[DefaultValue(false)]
	[Browsable(true)]
	[Category("Quick Access Toolbar")]
	[Description("Indicates whether external implementation for MetroToolbar and menu item customization will be used for customizing the MetroShell control.")]
	public bool UseExternalCustomization
	{
		get
		{
			return _UseExternalCustomization;
		}
		set
		{
			_UseExternalCustomization = value;
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
			return _UseCustomizeDialog;
		}
		set
		{
			_UseCustomizeDialog = value;
		}
	}

	[DefaultValue(eMetroCategorizeMode.Toolbar)]
	[Browsable(true)]
	[Category("Quick Access Toolbar")]
	[Description("Indicates categorization mode for the items on Quick Access Toolbar customize dialog box.")]
	public eMetroCategorizeMode CategorizeMode
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

	internal List<IOwner> RegisteredToolbars
	{
		get
		{
			List<IOwner> list = new List<IOwner>();
			foreach (WeakReference toolbar in _Toolbars)
			{
				if (toolbar.IsAlive)
				{
					list.Add((IOwner)toolbar.Target);
				}
			}
			return list;
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

	protected override Size DefaultSize => new Size(200, 100);

	[DefaultValue(eMetroTabStripDock.Top)]
	[Category("Appearance")]
	[Description("Indicates side tab-strip is docked to.")]
	public eMetroTabStripDock TabStripDock
	{
		get
		{
			return _TabStripDock;
		}
		set
		{
			_TabStripDock = value;
			if (_TabStripDock == eMetroTabStripDock.Top)
			{
				((Control)_TabStrip).Dock = (DockStyle)1;
			}
			else if (_TabStripDock == eMetroTabStripDock.Bottom)
			{
				((Control)_TabStrip).Dock = (DockStyle)2;
			}
		}
	}

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether SETTINGS button is visible.")]
	public bool SettingsButtonVisible
	{
		get
		{
			return _TabStrip.MetroStripContainer.SettingsButtonVisible;
		}
		set
		{
			if (value != _TabStrip.MetroStripContainer.SettingsButtonVisible)
			{
				_TabStrip.MetroStripContainer.SettingsButtonVisible = value;
			}
		}
	}

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether HELP button is visible.")]
	public bool HelpButtonVisible
	{
		get
		{
			return _TabStrip.MetroStripContainer.HelpButtonVisible;
		}
		set
		{
			if (value != _TabStrip.MetroStripContainer.HelpButtonVisible)
			{
				_TabStrip.MetroStripContainer.HelpButtonVisible = value;
			}
		}
	}

	[DefaultValue("")]
	[Category("Appearance")]
	[Description("Indicates SETTINGS button text")]
	public string SettingsButtonText
	{
		get
		{
			return _TabStrip.MetroStripContainer.SettingsButtonText;
		}
		set
		{
			_TabStrip.MetroStripContainer.SettingsButtonText = value;
		}
	}

	[DefaultValue("")]
	[Category("Appearance")]
	[Description("Indicates HELP button text")]
	public string HelpButtonText
	{
		get
		{
			return _TabStrip.MetroStripContainer.HelpButtonText;
		}
		set
		{
			_TabStrip.MetroStripContainer.HelpButtonText = value;
		}
	}

	[AmbientValue(null)]
	[Localizable(true)]
	[Category("Appearance")]
	[Description("Indicates the font tab items are displayed with.")]
	public Font TabStripFont
	{
		get
		{
			return ((Control)_TabStrip).Font;
		}
		set
		{
			((Control)_TabStrip).Font = value;
		}
	}

	public event CustomizeMenuPopupEventHandler BeforeCustomizeMenuPopup;

	public event CustomizeMenuPopupEventHandler BeforeAddItemToQuickAccessToolbar;

	public event CustomizeMenuPopupEventHandler BeforeRemoveItemFromQuickAccessToolbar;

	public event DotNetBarManager.LocalizeStringEventHandler LocalizeString;

	[Description("Occurs when Item on metro tab strip or quick access toolbar is clicked.")]
	public event EventHandler ItemClick;

	public event QatCustomizeDialogEventHandler BeforeQatCustomizeDialog;

	public event QatCustomizeDialogEventHandler AfterQatCustomizeDialog;

	public event EventHandler AfterQatDialogChangesApplied;

	public event EventHandler SelectedTabChanged;

	[Description("Occurs when text markup link from TitleText markup is clicked.")]
	public event MarkupLinkClickEventHandler TitleTextMarkupLinkClick;

	[Description("Occurs when SETTINGS button, if displayed, is clicked.")]
	public event EventHandler SettingsButtonClick;

	[Description("Occurs when HELP button, if displayed, is clicked.")]
	public event EventHandler HelpButtonClick;

	protected virtual void OnSettingsButtonClick(EventArgs e)
	{
		this.SettingsButtonClick?.Invoke(this, e);
	}

	internal void InvokeSettingsButtonClick(EventArgs e)
	{
		OnSettingsButtonClick(e);
	}

	protected virtual void OnHelpButtonClick(EventArgs e)
	{
		this.HelpButtonClick?.Invoke(this, e);
	}

	internal void InvokeHelpButtonClick(EventArgs e)
	{
		OnHelpButtonClick(e);
	}

	public MetroShell()
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		BaseRenderer renderer = GlobalManager.Renderer;
		_QatFrequentCommands.IgnoreEvents = true;
		_QatFrequentCommands.AllowParentRemove = false;
		((Control)this).SetStyle((ControlStyles)(0x2010 | DisplayHelp.DoubleBufferFlag | 2 | 4), true);
		_TabStrip = new MetroTabStrip();
		((Control)_TabStrip).Dock = (DockStyle)1;
		((Control)_TabStrip).Height = 32;
		_TabStrip.ItemAdded += TabStripItemAdded;
		_TabStrip.LocalizeString += TabStripLocalizeString;
		_TabStrip.ItemClick += TabStripItemClick;
		_TabStrip.ButtonCheckedChanged += TabStripButtonCheckedChanged;
		_TabStrip.TitleTextMarkupLinkClick += TabStripTitleTextMarkupLinkClick;
		((Control)this).Controls.Add((Control)(object)_TabStrip);
		TabStop = false;
		((ScrollableControl)this).DockPadding.Bottom = DefaultBottomDockPadding;
		StyleManager.Register((Control)(object)this);
	}

	protected override void Dispose(bool disposing)
	{
		StyleManager.Unregister((Control)(object)this);
		((ContainerControl)this).Dispose(disposing);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void StyleManagerStyleChanged(eDotNetBarStyle newStyle)
	{
	}

	protected virtual void OnSelectedTabChanged(EventArgs e)
	{
		if (this.SelectedTabChanged != null)
		{
			this.SelectedTabChanged(this, e);
		}
	}

	private void TabStripButtonCheckedChanged(object sender, EventArgs e)
	{
		if (sender is MetroTabItem && ((MetroTabItem)sender).Checked)
		{
			OnSelectedTabChanged(new EventArgs());
		}
	}

	private void TabStripItemClick(object sender, EventArgs e)
	{
		if (this.ItemClick != null)
		{
			this.ItemClick(sender, e);
		}
	}

	private void TabStripLocalizeString(object sender, LocalizeEventArgs e)
	{
		if (this.LocalizeString != null)
		{
			this.LocalizeString(this, e);
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

	public void SetTabPanelStyle(MetroTabPanel panel)
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

	public MetroTabItem CreateTab(string text, string name, int insertPosition)
	{
		MetroTabItem metroTabItem = new MetroTabItem();
		metroTabItem.Text = text;
		metroTabItem.Name = name;
		MetroTabPanel metroTabPanel = new MetroTabPanel();
		((Control)metroTabPanel).Dock = (DockStyle)5;
		SetTabPanelStyle(metroTabPanel);
		((Control)this).Controls.Add((Control)(object)metroTabPanel);
		((Control)metroTabPanel).SendToBack();
		metroTabItem.Panel = metroTabPanel;
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
				Items.Add(metroTabItem);
			}
			else
			{
				Items.Insert(insertPosition, metroTabItem);
			}
		}
		else if (insertPosition > Items.Count - 1)
		{
			Items.Add(metroTabItem);
		}
		else
		{
			Items.Insert(insertPosition, metroTabItem);
		}
		return metroTabItem;
	}

	public MetroTabItem CreateTab(string text, string name)
	{
		return CreateTab(text, name, -1);
	}

	public void RecalcLayout()
	{
		_TabStrip.RecalcLayout();
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
			if (!(val is MetroTabPanel))
			{
				continue;
			}
			foreach (Control item2 in (ArrangedElementCollection)val.Controls)
			{
				Control val2 = item2;
				handle = val2.Handle;
			}
		}
		RecalcLayout();
	}

	protected override void OnControlAdded(ControlEventArgs e)
	{
		((Control)this).OnControlAdded(e);
		if (((Component)this).DesignMode || !(e.Control is MetroTabPanel metroTabPanel))
		{
			return;
		}
		if (metroTabPanel.MetroTabItem != null)
		{
			if (Items.Contains(metroTabPanel.MetroTabItem) && SelectedTab == metroTabPanel.MetroTabItem)
			{
				metroTabPanel.Visible = true;
				((Control)metroTabPanel).BringToFront();
			}
			else
			{
				metroTabPanel.Visible = false;
			}
		}
		else
		{
			metroTabPanel.Visible = false;
		}
	}

	private void TabStripItemAdded(object sender, EventArgs e)
	{
		if (((Component)this).DesignMode || !(sender is MetroTabItem))
		{
			return;
		}
		MetroTabItem metroTabItem = sender as MetroTabItem;
		if (metroTabItem.Panel != null)
		{
			if (((Control)this).Controls.Contains((Control)(object)metroTabItem.Panel) && metroTabItem.Checked)
			{
				metroTabItem.Panel.Visible = true;
				((Control)metroTabItem.Panel).BringToFront();
			}
			else
			{
				metroTabItem.Panel.Visible = false;
			}
		}
	}

	protected override void OnControlRemoved(ControlEventArgs e)
	{
		((Control)this).OnControlRemoved(e);
		if (e.Control is MetroTabPanel { MetroTabItem: not null } metroTabPanel && Items.Contains(metroTabPanel.MetroTabItem))
		{
			Items.Remove(metroTabPanel.MetroTabItem);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void SetDesignMode()
	{
		_TabStrip.SetDesignMode(mode: true);
	}

	private ElementStyle GetBackgroundStyle()
	{
		return _TabStrip.InternalGetBackgroundStyle();
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		ElementStyle backgroundStyle = GetBackgroundStyle();
		if ((backgroundStyle.BackColor.A < byte.MaxValue && !backgroundStyle.BackColor.IsEmpty) || ((Control)this).BackColor == Color.Transparent || ((Control)this).BackgroundImage != null)
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
		ElementStyleDisplayInfo e2 = new ElementStyleDisplayInfo(backgroundStyle, e.Graphics, ((Control)this).ClientRectangle);
		ElementStyleDisplay.PaintBackground(e2);
	}

	protected override void WndProc(ref Message m)
	{
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Invalid comparison between Unknown and I4
		if (((Message)(ref m)).Msg == 132)
		{
			int x = WinApi.LOWORD(((Message)(ref m)).LParam);
			int y = WinApi.HIWORD(((Message)(ref m)).LParam);
			Point pt = ((Control)this).PointToClient(new Point(x, y));
			if (CaptionVisible && _TabStrip != null && !_TabStrip.IsMaximized && !(((Control)this).FindForm() is MetroAppForm { Sizable: false }))
			{
				int num = 4;
				if (new Rectangle(0, 0, ((Control)this).Width, num).Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(-1);
					return;
				}
				if (new Rectangle(0, 0, num, ((Control)this).Height).Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(-1);
					return;
				}
				if (new Rectangle(((Control)this).Width - num, 0, num, ((Control)this).Height).Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(-1);
					return;
				}
			}
			if (BarFunctions.IsWindows7 && _TabStrip != null && _TabStrip.IsMaximized && _TabStrip.CaptionBounds.Contains(pt))
			{
				((Message)(ref m)).Result = new IntPtr(-1);
				return;
			}
			if (((pt.X < 28 && (int)((Control)this).RightToLeft == 0) || (pt.X > ((Control)this).Width - 28 && (int)((Control)this).RightToLeft == 1)) && pt.Y < 28)
			{
				((Message)(ref m)).Result = new IntPtr(-1);
				return;
			}
		}
		((ContainerControl)this).WndProc(ref m);
	}

	private void TabStripTitleTextMarkupLinkClick(object sender, MarkupLinkClickEventArgs e)
	{
		if (this.TitleTextMarkupLinkClick != null)
		{
			this.TitleTextMarkupLinkClick(this, new MarkupLinkClickEventArgs(e.Name, e.HRef));
		}
	}

	private SubItemsCollection GetQatSubItemsCollection()
	{
		return _TabStrip.QuickToolbarItems;
	}

	private static BaseItem GetAppButton(MetroShell tab)
	{
		BaseItem result = null;
		for (int i = 0; i < tab.QuickToolbarItems.Count; i++)
		{
			if (tab.QuickToolbarItems[i] is MetroAppButton)
			{
				result = tab.QuickToolbarItems[i];
				break;
			}
		}
		return result;
	}

	protected override void OnBackColorChanged(EventArgs e)
	{
		((Control)this).OnBackColorChanged(e);
	}

	internal void OnChildItemClick(BaseItem item)
	{
		_TabStrip.OnChildItemClick(item);
		if (item != null && !(item is MetroTabItem) && !item.IsContainer && !item.Name.StartsWith("sysgallery") && !(item is CheckBoxItem) && item.AutoCollapseOnClick && item is PopupItem)
		{
			_ = item.Expanded;
		}
	}

	protected override void OnVisibleChanged(EventArgs e)
	{
		((ScrollableControl)this).OnVisibleChanged(e);
	}

	protected override void OnTabStopChanged(EventArgs e)
	{
		((Control)this).OnTabStopChanged(e);
		((Control)_TabStrip).TabStop = TabStop;
	}

	internal void OnTabStripRightClick(MetroTabStrip metroStrip, int x, int y)
	{
		if (DisplayCustomizeContextMenu)
		{
			BaseItem baseItem = metroStrip.HitTest(x, y);
			if (baseItem != null && baseItem.CanCustomize && !baseItem.SystemItem)
			{
				ShowCustomizeContextMenu(baseItem, isTabStrip: true);
			}
		}
	}

	internal virtual void ShowCustomizeContextMenu(object o, bool isTabStrip)
	{
		if (o == null || !_UseCustomizeDialog)
		{
			return;
		}
		_TabStrip.ClosePopup("syscustomizepopupmenu");
		ButtonItem buttonItem = new ButtonItem("syscustomizepopupmenu");
		buttonItem.Style = eDotNetBarStyle.StyleManagerControlled;
		buttonItem.SetOwner(_TabStrip);
		if (CanCustomizeItem(o as BaseItem) && !_UseExternalCustomization)
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
				if (baseItem != null && QuickToolbarItems.Contains(baseItem.Name))
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
		if (_UseCustomizeDialog)
		{
			ButtonItem buttonItem4 = new ButtonItem(SysQatCustomizeItemName);
			buttonItem4.Text = SystemText.QatCustomizeText;
			buttonItem4.BeginGroup = true;
			buttonItem4.Click += CustomizeQuickAccessToolbarDialog;
			buttonItem.SubItems.Add(buttonItem4);
		}
		RibbonCustomizeEventArgs ribbonCustomizeEventArgs = new RibbonCustomizeEventArgs(o, buttonItem);
		OnBeforeCustomizeMenuPopup(ribbonCustomizeEventArgs);
		if (ribbonCustomizeEventArgs.Cancel)
		{
			buttonItem.Dispose();
			return;
		}
		((IOwnerMenuSupport)_TabStrip).RegisterPopup((PopupItem)buttonItem);
		buttonItem.Popup(Control.MousePosition);
	}

	private void CustomizePopupItemParentVisibleChange(object sender, EventArgs e)
	{
		Control val = (Control)((sender is Control) ? sender : null);
		if (val != null)
		{
			val.VisibleChanged -= CustomizePopupItemParentVisibleChange;
			_TabStrip.ClosePopup("syscustomizepopupmenu");
		}
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
			buttonItem.ImagePaddingHorizontal = 8;
			buttonItem.ImagePaddingVertical = 6;
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
		if (val is MetroQatCustomizeDialog)
		{
			((MetroQatCustomizeDialog)(object)val).LoadItems(this);
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
		else if (val is MetroQatCustomizeDialog metroQatCustomizeDialog && metroQatCustomizeDialog.QatCustomizePanel.DataChanged)
		{
			ApplyQatCustomizePanelChanges(metroQatCustomizeDialog.QatCustomizePanel);
			((Component)(object)val).Dispose();
		}
	}

	public void ApplyQatCustomizePanelChanges(QatCustomizePanel customizePanel)
	{
		if (customizePanel == null || !customizePanel.DataChanged)
		{
			return;
		}
		_TabStrip.BeginUpdate();
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
			_TabStrip.EndUpdate();
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

	private MetroQatCustomizeDialog CreateQatCustomizeDialog()
	{
		MetroQatCustomizeDialog metroQatCustomizeDialog = new MetroQatCustomizeDialog();
		((Control)metroQatCustomizeDialog).Text = SystemText.QatDialogCaption;
		((Control)metroQatCustomizeDialog.buttonCancel).Text = SystemText.QatDialogCancelButton;
		((Control)metroQatCustomizeDialog.buttonOK).Text = SystemText.QatDialogOkButton;
		((Control)metroQatCustomizeDialog.QatCustomizePanel.buttonAddToQat).Text = SystemText.QatDialogAddButton;
		((Control)metroQatCustomizeDialog.QatCustomizePanel.buttonRemoveFromQat).Text = SystemText.QatDialogRemoveButton;
		((Control)metroQatCustomizeDialog.QatCustomizePanel.labelCategories).Text = SystemText.QatDialogCategoriesLabel;
		((Control)metroQatCustomizeDialog.QatCustomizePanel.checkQatBelowRibbon).Text = SystemText.QatDialogPlacementCheckbox;
		return metroQatCustomizeDialog;
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
		return _TabStrip.GetApplicationButton();
	}

	private string GetQatLayoutDescription()
	{
		ArrayList qatItems = GetQatItems(includeCustomizeItem: false);
		StringBuilder stringBuilder = new StringBuilder();
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
		ArrayList arrayList = new ArrayList();
		for (int i = 0; i < array.Length; i++)
		{
			if (QuickToolbarItems.Contains(array[i]))
			{
				arrayList.Add(QuickToolbarItems[array[i]]);
				continue;
			}
			BaseItem baseItem = FindItem(array[i]);
			if (baseItem != null && CanCustomizeItem(baseItem))
			{
				arrayList.Add(GetQatItemCopy(baseItem));
			}
		}
		ArrayList qatItems = GetQatItems(includeCustomizeItem: false);
		foreach (BaseItem item3 in qatItems)
		{
			QuickToolbarItems.Remove(item3);
		}
		foreach (BaseItem item4 in arrayList)
		{
			QuickToolbarItems.Add(item4);
		}
		m_QatLayoutChanged = false;
	}

	private BaseItem FindItem(string name)
	{
		foreach (WeakReference toolbar in _Toolbars)
		{
			if (!toolbar.IsAlive)
			{
				continue;
			}
			IOwner owner = (IOwner)toolbar.Target;
			if (owner != null)
			{
				BaseItem item = owner.GetItem(name);
				if (item != null)
				{
					return item;
				}
			}
		}
		return null;
	}

	public void RegisterToolbar(IOwner toolbar)
	{
		if (toolbar == null)
		{
			throw new ArgumentNullException("toolbar parameter cannot be null");
		}
		if (IsToolbarRegistered(toolbar))
		{
			throw new ArgumentException("toolbar is already registered");
		}
		WeakReference item = new WeakReference(toolbar);
		_Toolbars.Add(item);
	}

	public void UnregisterToolbar(IOwner toolbar)
	{
		if (toolbar == null)
		{
			throw new ArgumentNullException("toolbar parameter cannot be null");
		}
		foreach (WeakReference toolbar2 in _Toolbars)
		{
			if (toolbar2.IsAlive && toolbar2.Target == toolbar)
			{
				_Toolbars.Remove(toolbar2);
				break;
			}
		}
	}

	private bool IsToolbarRegistered(IOwner toolbar)
	{
		foreach (WeakReference toolbar2 in _Toolbars)
		{
			if (toolbar2.IsAlive && toolbar2.Target == toolbar)
			{
				return true;
			}
		}
		return false;
	}
}
