using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using System.Xml;

namespace DevComponents.DotNetBar;

[Designer("DevComponents.DotNetBar.Design.NavigationPaneDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[ToolboxItem(true)]
public class NavigationPane : UserControl
{
	private PanelExTitle ℳ;

	private NavigationBar ℴ;

	private Container Պ;

	private int ℵ;

	private bool ℶ;

	private bool Ӫ = true;

	private bool _1C3B = true;

	private Image ℷ;

	private Image ℸ;

	private Image ℹ;

	private Image _213A;

	private Image _213B;

	private Image ℼ;

	private Image ℽ;

	private Image ℾ;

	private int ℿ;

	private int _2140;

	private PanelEx _2141;

	private int ᇰ = 100;

	private bool _2142;

	private PanelEx _2143;

	private bool ஹ;

	private EventHandler _09BC;

	private PanelChangingEventHandler _2144;

	private DotNetBarManager.LocalizeStringEventHandler _09D4;

	private PanelPopupEventHandler ⅅ;

	private EventHandler ⅆ;

	private EventHandler ⅇ;

	private ExpandChangeEventHandler ℏ;

	private ExpandChangeEventHandler _1C2B;

	private EventHandler ᯞ;

	private bool ⅈ;

	[Browsable(false)]
	public int ExpandedSize => ℿ;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(false)]
	public SubItemsCollection Items => ℴ.Items;

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(32)]
	[Category("Layout")]
	public int NavigationBarHeight
	{
		get
		{
			if (!((Component)this).DesignMode && ℴ != null && ℴ.Items.Count > 0)
			{
				return ((Control)ℴ).Height;
			}
			return ℵ;
		}
		set
		{
			ℵ = value;
			((Control)ℴ).Height = ℵ;
		}
	}

	[DevCoBrowsable(false)]
	[Browsable(false)]
	public NavigationBar NavigationBar => ℴ;

	[Description("Indicates size of the image that will be use to resize images to when button button is on the bottom summary line of navigation bar and AutoSizeButtonImage=true.")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	public Size ImageSizeSummaryLine
	{
		get
		{
			return ℴ.ImageSizeSummaryLine;
		}
		set
		{
			ℴ.ImageSizeSummaryLine = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[DefaultValue(true)]
	[Description("Indicates Configure Buttons button is visible.")]
	public bool ConfigureItemVisible
	{
		get
		{
			return ℴ.ConfigureItemVisible;
		}
		set
		{
			ℴ.ConfigureItemVisible = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Description("Indicates whether Show More Buttons and Show Fewer Buttons menu items are visible on Configure buttons menu.")]
	[Category("Behavior")]
	[DefaultValue(true)]
	public bool ConfigureShowHideVisible
	{
		get
		{
			return ℴ.ConfigureShowHideVisible;
		}
		set
		{
			ℴ.ConfigureShowHideVisible = value;
		}
	}

	[Description("Gets or sets whether Navigation Pane Options menu item is visible on Configure buttons menu.")]
	[DefaultValue(true)]
	[Category("Behavior")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	public bool ConfigureNavOptionsVisible
	{
		get
		{
			return ℴ.ConfigureNavOptionsVisible;
		}
		set
		{
			ℴ.ConfigureNavOptionsVisible = value;
		}
	}

	[DevCoBrowsable(true)]
	[Description("Indicates whether Navigation Pane Add/Remove Buttons menu item is visible on Configure buttons menu.")]
	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Behavior")]
	public bool ConfigureAddRemoveVisible
	{
		get
		{
			return ℴ.ConfigureAddRemoveVisible;
		}
		set
		{
			ℴ.ConfigureAddRemoveVisible = value;
		}
	}

	public ButtonItem CheckedButton => ℴ.CheckedButton;

	public NavigationPanePanel SelectedPanel => GetPanel(CheckedButton);

	[Category("Data")]
	[DefaultValue(null)]
	[Description("ImageList for images used on Items.")]
	[Browsable(true)]
	public ImageList Images
	{
		get
		{
			return ℴ.Images;
		}
		set
		{
			ℴ.Images = value;
		}
	}

	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[DevCoBrowsable(true)]
	[Description("PanelEx control that is used to display title.")]
	[Category("Appearance")]
	public PanelEx TitlePanel => ℳ;

	[Description("Indicates the padding in pixels at the top portion of the item.")]
	[DefaultValue(4)]
	[Category("Layout")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	public int ItemPaddingTop
	{
		get
		{
			return ℴ.ItemPaddingTop;
		}
		set
		{
			ℴ.ItemPaddingTop = value;
			if (((Component)this).DesignMode)
			{
				RecalcLayout();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(4)]
	[Category("Layout")]
	[DevCoBrowsable(true)]
	[Description("Indicates the padding in pixels at the bottom of the item.")]
	public int ItemPaddingBottom
	{
		get
		{
			return ℴ.ItemPaddingBottom;
		}
		set
		{
			ℴ.ItemPaddingBottom = value;
			if (((Component)this).DesignMode)
			{
				RecalcLayout();
			}
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[DevCoBrowsable(true)]
	[DefaultValue(true)]
	[Description("Indicates whether images are automatically resized to size specified in ImageSizeSummaryLine when button is on the bottom summary line of navigation bar.")]
	public bool AutoSizeButtonImage
	{
		get
		{
			return ℴ.AutoSizeButtonImage;
		}
		set
		{
			ℴ.AutoSizeButtonImage = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string LayoutDefinition
	{
		get
		{
			return ℴ.LayoutDefinition;
		}
		set
		{
			ℴ.LayoutDefinition = value;
		}
	}

	[Description("Gets or sets whether anti-aliasing is used while painting.")]
	[Browsable(true)]
	[Category("Appearance")]
	[DefaultValue(false)]
	public bool AntiAlias
	{
		get
		{
			return ℴ.AntiAlias;
		}
		set
		{
			ℴ.AntiAlias = value;
			if (ℳ != null)
			{
				ℳ.AntiAlias = value;
			}
			if (_2141 != null)
			{
				_2141.AntiAlias = value;
			}
			if (_2143 != null)
			{
				_2143.AntiAlias = value;
			}
		}
	}

	[DefaultValue(100)]
	[Browsable(true)]
	[Description("Indicates animation time in milliseconds when control is expanded or collapsed.")]
	[Category("Expand")]
	public int AnimationTime
	{
		get
		{
			return ᇰ;
		}
		set
		{
			if (ᇰ >= 0)
			{
				ᇰ = value;
			}
		}
	}

	[DefaultValue(false)]
	[Description("Indicates whether navigation pane can be collapsed.")]
	[Browsable(true)]
	[Category("Title")]
	public bool CanCollapse
	{
		get
		{
			return ℶ;
		}
		set
		{
			ℶ = value;
			_2159();
			ℳ.ExpandButtonVisible = ℶ;
		}
	}

	[Category("Title")]
	[DefaultValue(eTitleButtonAlignment.Right)]
	[Browsable(true)]
	[Description("Indicates the alignment of expand button inside title bar.")]
	public eTitleButtonAlignment TitleButtonAlignment
	{
		get
		{
			return ℳ.ButtonAlignment;
		}
		set
		{
			ℳ.ButtonAlignment = value;
			_215E();
		}
	}

	[Category("Title")]
	[Description("Indicates whether navigation pane can be collapsed.")]
	[Browsable(true)]
	[DefaultValue(true)]
	public bool Expanded
	{
		get
		{
			return Ӫ;
		}
		set
		{
			if (ஹ)
			{
				_1C3B = value;
			}
			else if (Ӫ != value)
			{
				_211E(value, eEventSource.Code);
			}
		}
	}

	[Browsable(false)]
	public PanelEx CollapsedInnerPanel => _2141;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool PopupSelectedPaneVisible
	{
		get
		{
			return _2142;
		}
		set
		{
			if (_2142 != value)
			{
				if (value)
				{
					_2154();
				}
				else
				{
					_2156();
				}
			}
		}
	}

	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Browsable(true)]
	[Description("Specifies the visual style of the control.")]
	[DefaultValue(eDotNetBarStyle.Office2003)]
	public eDotNetBarStyle Style
	{
		get
		{
			return ℴ.Style;
		}
		set
		{
			ℴ.Style = value;
			ColorSchemeStyleChanged();
		}
	}

	[Description("Occurs when Item is clicked.")]
	[Category("Navigation Pane Events")]
	public event EventHandler ItemClick
	{
		add
		{
			EventHandler eventHandler = _09BC;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09BC, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _09BC;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09BC, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when Item is clicked.")]
	[Category("Navigation Pane Events")]
	public event PanelChangingEventHandler PanelChanging
	{
		add
		{
			PanelChangingEventHandler panelChangingEventHandler = _2144;
			PanelChangingEventHandler panelChangingEventHandler2;
			do
			{
				panelChangingEventHandler2 = panelChangingEventHandler;
				PanelChangingEventHandler value2 = (PanelChangingEventHandler)Delegate.Combine(panelChangingEventHandler2, value);
				panelChangingEventHandler = Interlocked.CompareExchange(ref _2144, value2, panelChangingEventHandler2);
			}
			while ((object)panelChangingEventHandler != panelChangingEventHandler2);
		}
		remove
		{
			PanelChangingEventHandler panelChangingEventHandler = _2144;
			PanelChangingEventHandler panelChangingEventHandler2;
			do
			{
				panelChangingEventHandler2 = panelChangingEventHandler;
				PanelChangingEventHandler value2 = (PanelChangingEventHandler)Delegate.Remove(panelChangingEventHandler2, value);
				panelChangingEventHandler = Interlocked.CompareExchange(ref _2144, value2, panelChangingEventHandler2);
			}
			while ((object)panelChangingEventHandler != panelChangingEventHandler2);
		}
	}

	public event DotNetBarManager.LocalizeStringEventHandler LocalizeString
	{
		add
		{
			DotNetBarManager.LocalizeStringEventHandler localizeStringEventHandler = _09D4;
			DotNetBarManager.LocalizeStringEventHandler localizeStringEventHandler2;
			do
			{
				localizeStringEventHandler2 = localizeStringEventHandler;
				DotNetBarManager.LocalizeStringEventHandler value2 = (DotNetBarManager.LocalizeStringEventHandler)Delegate.Combine(localizeStringEventHandler2, value);
				localizeStringEventHandler = Interlocked.CompareExchange(ref _09D4, value2, localizeStringEventHandler2);
			}
			while ((object)localizeStringEventHandler != localizeStringEventHandler2);
		}
		remove
		{
			DotNetBarManager.LocalizeStringEventHandler localizeStringEventHandler = _09D4;
			DotNetBarManager.LocalizeStringEventHandler localizeStringEventHandler2;
			do
			{
				localizeStringEventHandler2 = localizeStringEventHandler;
				DotNetBarManager.LocalizeStringEventHandler value2 = (DotNetBarManager.LocalizeStringEventHandler)Delegate.Remove(localizeStringEventHandler2, value);
				localizeStringEventHandler = Interlocked.CompareExchange(ref _09D4, value2, localizeStringEventHandler2);
			}
			while ((object)localizeStringEventHandler != localizeStringEventHandler2);
		}
	}

	public event PanelPopupEventHandler BeforePanelPopup
	{
		add
		{
			PanelPopupEventHandler panelPopupEventHandler = ⅅ;
			PanelPopupEventHandler panelPopupEventHandler2;
			do
			{
				panelPopupEventHandler2 = panelPopupEventHandler;
				PanelPopupEventHandler value2 = (PanelPopupEventHandler)Delegate.Combine(panelPopupEventHandler2, value);
				panelPopupEventHandler = Interlocked.CompareExchange(ref ⅅ, value2, panelPopupEventHandler2);
			}
			while ((object)panelPopupEventHandler != panelPopupEventHandler2);
		}
		remove
		{
			PanelPopupEventHandler panelPopupEventHandler = ⅅ;
			PanelPopupEventHandler panelPopupEventHandler2;
			do
			{
				panelPopupEventHandler2 = panelPopupEventHandler;
				PanelPopupEventHandler value2 = (PanelPopupEventHandler)Delegate.Remove(panelPopupEventHandler2, value);
				panelPopupEventHandler = Interlocked.CompareExchange(ref ⅅ, value2, panelPopupEventHandler2);
			}
			while ((object)panelPopupEventHandler != panelPopupEventHandler2);
		}
	}

	public event EventHandler AfterPanelPopup
	{
		add
		{
			EventHandler eventHandler = ⅆ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ⅆ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ⅆ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ⅆ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler PanelPopupClosed
	{
		add
		{
			EventHandler eventHandler = ⅇ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ⅇ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ⅇ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ⅇ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event ExpandChangeEventHandler ExpandedChanging
	{
		add
		{
			ExpandChangeEventHandler expandChangeEventHandler = ℏ;
			ExpandChangeEventHandler expandChangeEventHandler2;
			do
			{
				expandChangeEventHandler2 = expandChangeEventHandler;
				ExpandChangeEventHandler value2 = (ExpandChangeEventHandler)Delegate.Combine(expandChangeEventHandler2, value);
				expandChangeEventHandler = Interlocked.CompareExchange(ref ℏ, value2, expandChangeEventHandler2);
			}
			while ((object)expandChangeEventHandler != expandChangeEventHandler2);
		}
		remove
		{
			ExpandChangeEventHandler expandChangeEventHandler = ℏ;
			ExpandChangeEventHandler expandChangeEventHandler2;
			do
			{
				expandChangeEventHandler2 = expandChangeEventHandler;
				ExpandChangeEventHandler value2 = (ExpandChangeEventHandler)Delegate.Remove(expandChangeEventHandler2, value);
				expandChangeEventHandler = Interlocked.CompareExchange(ref ℏ, value2, expandChangeEventHandler2);
			}
			while ((object)expandChangeEventHandler != expandChangeEventHandler2);
		}
	}

	public event ExpandChangeEventHandler ExpandedChanged
	{
		add
		{
			ExpandChangeEventHandler expandChangeEventHandler = _1C2B;
			ExpandChangeEventHandler expandChangeEventHandler2;
			do
			{
				expandChangeEventHandler2 = expandChangeEventHandler;
				ExpandChangeEventHandler value2 = (ExpandChangeEventHandler)Delegate.Combine(expandChangeEventHandler2, value);
				expandChangeEventHandler = Interlocked.CompareExchange(ref _1C2B, value2, expandChangeEventHandler2);
			}
			while ((object)expandChangeEventHandler != expandChangeEventHandler2);
		}
		remove
		{
			ExpandChangeEventHandler expandChangeEventHandler = _1C2B;
			ExpandChangeEventHandler expandChangeEventHandler2;
			do
			{
				expandChangeEventHandler2 = expandChangeEventHandler;
				ExpandChangeEventHandler value2 = (ExpandChangeEventHandler)Delegate.Remove(expandChangeEventHandler2, value);
				expandChangeEventHandler = Interlocked.CompareExchange(ref _1C2B, value2, expandChangeEventHandler2);
			}
			while ((object)expandChangeEventHandler != expandChangeEventHandler2);
		}
	}

	public event EventHandler OptionsDialogClosed
	{
		add
		{
			EventHandler eventHandler = ᯞ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᯞ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ᯞ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᯞ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public NavigationPane()
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		ـ();
		((Control)this).SetStyle((ControlStyles)4, true);
		((Control)this).SetStyle(DisplayHelp.DoubleBufferFlag, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		ℴ.ItemAdded += _214B;
		ℴ.ItemRemoved += _214D;
		ℴ.ᯢ += ᯢ;
		ℴ.LocalizeString += _214A;
		ℴ.OptionsDialogClosed += ⅉ;
		ℴ.SplitterVisible = true;
		ℵ = ((Control)ℴ).Height;
		ℳ.ExpandedClick += _2151;
		ℳ.ExpandChangeButton.ImagePaddingHorizontal = 2;
		ℳ.ExpandChangeButton.ImagePaddingVertical = 2;
		StyleManager.Register((Control)(object)this);
	}

	private void ⅉ(object _٠, EventArgs ؿ)
	{
		OnOptionsDialogClosed(ؿ);
	}

	private void _214A(object _٠, LocalizeEventArgs ؿ)
	{
		if (_09D4 != null)
		{
			_09D4(this, ؿ);
		}
	}

	protected virtual void OnOptionsDialogClosed(EventArgs e)
	{
		ᯞ?.Invoke(this, e);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			if (ℷ != null)
			{
				ℷ.Dispose();
			}
			if (ℸ != null)
			{
				ℸ.Dispose();
			}
			if (Պ != null)
			{
				Պ.Dispose();
			}
		}
		StyleManager.Unregister((Control)(object)this);
		((ContainerControl)this).Dispose(disposing);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Expected O, but got Unknown
		((Control)this).OnPaint(e);
		if (ℴ.ItemsContainer.EffectiveStyle == eDotNetBarStyle.Office2010)
		{
			ℴ.GetRenderer()?.DrawNavPaneButtonBackground(new NavPaneRenderEventArgs(e.Graphics, ((Control)this).ClientRectangle));
			return;
		}
		SolidBrush val = new SolidBrush(NavigationBar.GetColorScheme().BarBackground);
		try
		{
			e.Graphics.FillRectangle((Brush)(object)val, ((Control)this).ClientRectangle);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		Pen val2 = new Pen(NavigationBar.GetColorScheme().PanelBorder, 1f);
		try
		{
			e.Graphics.DrawRectangle(val2, new Rectangle(0, 0, ((Control)this).Width - 1, ((Control)this).Height - 1));
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void SetDesignMode()
	{
		ℴ.ᒶ(((Component)this).DesignMode);
	}

	public virtual void RecalcLayout()
	{
		ℴ.RecalcLayout();
	}

	private void _214B(object _٠, EventArgs ؿ)
	{
		if (_٠ is ButtonItem { IsOnCustomizeMenu: false } buttonItem)
		{
			buttonItem.CheckedChanged += ⅎ;
			buttonItem.TextChanged += _09D0;
			buttonItem.VisibleChanged += _214C;
			if (buttonItem.Checked)
			{
				buttonItem._0EDB(_0BCF: false);
				buttonItem.Checked = true;
			}
		}
	}

	private void _214C(object _٠, EventArgs ؿ)
	{
		if (!(_٠ is ButtonItem { IsOnCustomizeMenu: false, Checked: not false, Visible: false }))
		{
			return;
		}
		foreach (BaseItem item in Items)
		{
			if (item is ButtonItem { Visible: not false } buttonItem2)
			{
				buttonItem2.Checked = true;
				break;
			}
		}
	}

	private void _214D(object _٠, ItemRemovedEventArgs ؿ)
	{
		if (_٠ is ButtonItem buttonItem)
		{
			try
			{
				buttonItem.CheckedChanged -= ⅎ;
				buttonItem.TextChanged -= _09D0;
				buttonItem.VisibleChanged -= _214C;
			}
			catch
			{
			}
		}
	}

	private void ⅎ(object _٠, EventArgs ؿ)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		if (!(_٠ is ButtonItem { IsOnCustomizeMenu: false } buttonItem))
		{
			return;
		}
		foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control val = item;
			if (!(val is NavigationPanePanel))
			{
				continue;
			}
			NavigationPanePanel navigationPanePanel = val as NavigationPanePanel;
			if (navigationPanePanel.ParentItem != buttonItem)
			{
				continue;
			}
			if (buttonItem.Checked)
			{
				if (Ӫ)
				{
					if (!((Component)this).DesignMode)
					{
						((Control)navigationPanePanel).Visible = true;
					}
					((Control)navigationPanePanel).BringToFront();
				}
				((Control)ℳ).Text = buttonItem.Text;
				if (_2141 != null)
				{
					((Control)_2141).Text = ((Control)ℳ).Text;
				}
			}
			else if (Ӫ)
			{
				if (!((Component)this).DesignMode)
				{
					((Control)navigationPanePanel).Visible = false;
				}
				else
				{
					((Control)navigationPanePanel).SendToBack();
				}
			}
			break;
		}
	}

	private void _09D0(object _٠, EventArgs ؿ)
	{
		if (_٠ is ButtonItem { Checked: not false } buttonItem)
		{
			((Control)ℳ).Text = buttonItem.Text;
			if (_2141 != null)
			{
				((Control)_2141).Text = buttonItem.Text;
			}
		}
	}

	protected override void OnControlAdded(ControlEventArgs e)
	{
		((Control)this).OnControlAdded(e);
		if (((Component)this).DesignMode || !(e.Control is NavigationPanePanel navigationPanePanel))
		{
			return;
		}
		if (navigationPanePanel.ParentItem != null && Ӫ)
		{
			if (Items.Contains(navigationPanePanel.ParentItem) && navigationPanePanel.ParentItem is ButtonItem)
			{
				((Control)navigationPanePanel).Visible = ((ButtonItem)navigationPanePanel.ParentItem).Checked;
				if (((Control)navigationPanePanel).Visible)
				{
					((Control)navigationPanePanel).BringToFront();
				}
			}
			else
			{
				((Control)navigationPanePanel).Visible = false;
			}
		}
		else
		{
			((Control)navigationPanePanel).Visible = false;
		}
	}

	protected override void OnSystemColorsChanged(EventArgs e)
	{
		((Control)this).OnSystemColorsChanged(e);
		Application.DoEvents();
		if (ℶ)
		{
			_215F();
			_215E();
		}
	}

	public NavigationPanePanel GetPanel(ButtonItem item)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		if (item == null)
		{
			return null;
		}
		foreach (Control item2 in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control val = item2;
			if (val is NavigationPanePanel navigationPanePanel && navigationPanePanel.ParentItem == item)
			{
				return navigationPanePanel;
			}
		}
		return null;
	}

	private void _214F(object _٠, EventArgs ؿ)
	{
		if (((Component)this).DesignMode)
		{
			ℵ = ((Control)ℴ).Height;
		}
	}

	protected override void OnLayout(LayoutEventArgs e)
	{
		((ContainerControl)this).OnLayout(e);
		_2157();
	}

	public void SuspendLayout()
	{
		ஹ = true;
		((Control)this).SuspendLayout();
	}

	public void ResumeLayout(bool performLayout)
	{
		ஹ = false;
		((Control)ℴ).Height = ℵ;
		((Control)ℴ).SendToBack();
		((Control)this).ResumeLayout(true);
		Expanded = _1C3B;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeImageSizeSummaryLine()
	{
		return ℴ.ShouldSerializeImageSizeSummaryLine();
	}

	public void ShowMoreButtons()
	{
		ℴ.ShowMoreButtons();
	}

	public void ShowFewerButtons()
	{
		ℴ.ShowFewerButtons();
	}

	public void SaveLayout(string fileName)
	{
		ℴ.SaveLayout(fileName);
	}

	public void SaveLayout(XmlElement xmlParent)
	{
		ℴ.SaveLayout(xmlParent);
	}

	private void _211E(bool _211F, eEventSource ᎂ)
	{
		if (ℏ != null)
		{
			ExpandedChangeEventArgs expandedChangeEventArgs = new ExpandedChangeEventArgs(ᎂ, _211F);
			ℏ(this, expandedChangeEventArgs);
			if (expandedChangeEventArgs.Cancel)
			{
				return;
			}
		}
		Ӫ = _211F;
		ᱞ();
		if (_1C2B != null)
		{
			ExpandedChangeEventArgs e = new ExpandedChangeEventArgs(ᎂ, _211F);
			_1C2B(this, e);
		}
	}

	private void ᱞ()
	{
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Invalid comparison between Unknown and I4
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Invalid comparison between Unknown and I4
		if (Ӫ)
		{
			foreach (BaseItem item in ℴ.Items)
			{
				item._0AD4 = true;
			}
			ℳ.DrawText = true;
			PopupSelectedPaneVisible = false;
			_2158();
			if (AnimationTime == 0 || ((Component)this).DesignMode)
			{
				TypeDescriptor.GetProperties(this)["Width"].SetValue(this, ℿ);
			}
			else
			{
				Rectangle rectEnd = new Rectangle(((Control)this).Location, new Size(ℿ, ((Control)this).Height));
				if ((int)((Control)this).Dock == 4)
				{
					rectEnd.Offset(-ℿ, 0);
				}
				BarFunctions.AnimateControl((Control)(object)this, show: true, ᇰ, ((Control)this).Bounds, rectEnd);
			}
		}
		else
		{
			ℳ.DrawText = false;
			ℿ = ((Control)this).Width;
			if (AnimationTime == 0 || ((Component)this).DesignMode)
			{
				TypeDescriptor.GetProperties(this)["Width"].SetValue(this, _2150());
			}
			else
			{
				Rectangle rectEnd2 = new Rectangle(((Control)this).Location, new Size(_2150(), ((Control)this).Height));
				if ((int)((Control)this).Dock == 4)
				{
					rectEnd2.Offset(((Control)this).Width - rectEnd2.Width, 0);
				}
				BarFunctions.AnimateControl((Control)(object)this, show: true, ᇰ, ((Control)this).Bounds, rectEnd2);
			}
			_2152();
			foreach (BaseItem item2 in ℴ.Items)
			{
				item2._0AD4 = false;
			}
			((Control)ℴ).Invalidate();
		}
		_215E();
	}

	private int _2150()
	{
		int num = Math.Max(((Control)ℳ).Font.Height + 10, 34);
		if (_2140 > 0)
		{
			return _2140;
		}
		if (!(ℴ.ItemsContainer.ఽ() is ButtonItem { ImageSize: var imageSize } buttonItem))
		{
			return num;
		}
		if (imageSize.IsEmpty)
		{
			return num;
		}
		ᇎ ᇎ2 = buttonItem._0EC5(_1937.ሬ, Color.Black);
		if (ᇎ2 != null)
		{
			return Math.Max(num, ᇎ2.ᇓ + 10);
		}
		return num;
	}

	private void _2151(object _٠, EventArgs ؿ)
	{
		_211E(!Expanded, eEventSource.Mouse);
	}

	private void ᯢ(object _٠, _218C ؿ)
	{
		if (!PopupSelectedPaneVisible)
		{
			return;
		}
		string text = _089F.स(ؿ._0A65);
		text = text.ToLower();
		if (text.IndexOf("combolbox") >= 0)
		{
			return;
		}
		for (Control val = Control.FromChildHandle(ؿ._0A65); val != null; val = val.Parent)
		{
			if (val == _2143 || val == _2141)
			{
				return;
			}
		}
		PopupSelectedPaneVisible = false;
	}

	private void _2152()
	{
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Expected O, but got Unknown
		_2141 = new PanelEx();
		_2141.ColorSchemeStyle = ℴ.Style;
		_2141.ColorScheme = ℴ.GetColorScheme();
		_2141.ApplyButtonStyle();
		_2141.Style.VerticalText = true;
		_2141.Style.ForeColor.ColorSchemePart = eColorSchemePart.PanelText;
		_2141.Style.BackColor1.ColorSchemePart = eColorSchemePart.PanelBackground;
		_2141.Style.ResetBackColor2();
		_2141.StyleMouseOver.VerticalText = true;
		_2141.StyleMouseDown.VerticalText = true;
		((Control)_2141).Font = ((Control)ℳ).Font;
		_2141.AntiAlias = AntiAlias;
		_2157();
		if (CheckedButton != null)
		{
			((Control)_2141).Text = CheckedButton.Text;
		}
		else
		{
			((Control)_2141).Text = "Navigation Pane";
		}
		foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control val = item;
			if (val is NavigationPanePanel)
			{
				val.Visible = false;
			}
		}
		((Control)this).Controls.Add((Control)(object)_2141);
		((Control)_2141).Click += _2153;
	}

	private void _2153(object _٠, EventArgs ؿ)
	{
		PopupSelectedPaneVisible = !PopupSelectedPaneVisible;
	}

	private void _2154()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Invalid comparison between Unknown and I4
		if (_2142)
		{
			return;
		}
		Control val = null;
		foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control val2 = item;
			if (val2 is NavigationPanePanel && ((NavigationPanePanel)(object)val2).ParentItem == CheckedButton)
			{
				val = val2;
				break;
			}
		}
		if (val == null)
		{
			return;
		}
		int height = ((Control)ℴ).Top - ((Control)ℳ).Bottom;
		Rectangle rectangle = new Rectangle(((Control)this).Width, ((Control)ℳ).Bottom, ℿ, height);
		Rectangle rectStart = new Rectangle(((Control)this).Width, ((Control)ℳ).Bottom, 16, height);
		if (ⅅ != null)
		{
			PanelPopupEventArgs panelPopupEventArgs = new PanelPopupEventArgs(rectangle);
			ⅅ(this, panelPopupEventArgs);
			if (panelPopupEventArgs.Cancel)
			{
				return;
			}
			rectangle = panelPopupEventArgs.PopupBounds;
		}
		Control val3 = (Control)(object)((Control)this).FindForm();
		if (val3 == null)
		{
			val3 = ((Control)this).TopLevelControl;
		}
		if (val3 == null)
		{
			val3 = ((Control)this).Parent;
		}
		_2143 = new PanelEx();
		((Control)_2143).Visible = false;
		_2143.ApplyLabelStyle();
		_2143.AntiAlias = AntiAlias;
		val3.Controls.Add((Control)(object)_2143);
		((Control)_2143).BringToFront();
		((Control)this).Controls.Remove(val);
		if (val is NavigationPanePanel)
		{
			NavigationPanePanel navigationPanePanel = (NavigationPanePanel)(object)val;
			((ScrollableControl)navigationPanePanel).DockPadding.Bottom = 1;
			if (navigationPanePanel.Style.Border == eBorderType.None)
			{
				_2143.Style.BorderSide = eBorderSide.All;
				_2143.Style.Border = eBorderType.SingleLine;
				_2143.Style.BorderWidth = 1;
				_2143.Style.BorderColor.Color = ℳ.Style.BorderColor.Color;
				((ScrollableControl)_2143).DockPadding.All = 1;
			}
		}
		((Control)_2143).Controls.Add(val);
		val.Visible = true;
		if ((int)((Control)this).Dock == 4)
		{
			rectangle.Offset(-(((Control)this).Width + rectangle.Width), 0);
			rectStart.Offset(-(((Control)this).Width + rectStart.Width), 0);
		}
		Point point = ((Control)this).PointToScreen(rectangle.Location);
		rectangle.Location = val3.PointToClient(point);
		point = ((Control)this).PointToScreen(rectStart.Location);
		rectStart.Location = val3.PointToClient(point);
		if (AnimationTime == 0)
		{
			((Control)_2143).Bounds = rectangle;
			((Control)_2143).Visible = true;
		}
		else
		{
			BarFunctions.AnimateControl((Control)(object)_2143, show: true, AnimationTime, rectStart, rectangle);
		}
		((Control)_2143).Focus();
		((Control)_2143).Leave += _2155;
		_2142 = true;
		if (ⅆ != null)
		{
			ⅆ(this, new EventArgs());
		}
	}

	private void _2155(object _٠, EventArgs ؿ)
	{
		if (PopupSelectedPaneVisible)
		{
			PopupSelectedPaneVisible = false;
		}
	}

	private void _2156()
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Invalid comparison between Unknown and I4
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		if (!_2142 || ⅈ)
		{
			return;
		}
		ⅈ = true;
		try
		{
			if (AnimationTime == 0)
			{
				((Control)_2143).Hide();
			}
			else
			{
				Rectangle rectEnd = new Rectangle(((Control)this).Width, ((Control)this).Top, 16, ((Control)this).Height);
				if ((int)((Control)this).Dock == 4)
				{
					rectEnd.Offset(-(((Control)this).Width + rectEnd.Width), 0);
				}
				Point point = ((Control)this).PointToScreen(rectEnd.Location);
				rectEnd.Location = ((Control)_2143).Parent.PointToClient(point);
				BarFunctions.AnimateControl((Control)(object)_2143, show: false, AnimationTime, ((Control)_2143).Bounds, rectEnd);
			}
			Control val = null;
			foreach (Control item in (ArrangedElementCollection)((Control)_2143).Controls)
			{
				Control val2 = item;
				if (val2 is NavigationPanePanel)
				{
					val2.Visible = false;
					((Control)_2143).Controls.Remove(val2);
					val = val2;
					break;
				}
			}
			if (val is NavigationPanePanel)
			{
				((ScrollableControl)(NavigationPanePanel)(object)val).DockPadding.Bottom = 0;
			}
			((Control)this).Controls.Add(val);
			((Control)_2143).Leave -= _2155;
			((Component)(object)_2143).Dispose();
			_2143 = null;
			_2142 = false;
			OnPanelPopupClosed(EventArgs.Empty);
		}
		finally
		{
			ⅈ = false;
		}
	}

	protected virtual void OnPanelPopupClosed(EventArgs e)
	{
		ⅇ?.Invoke(this, e);
	}

	private void _2157()
	{
		if (_2141 != null)
		{
			int num = 3;
			((Control)_2141).Bounds = new Rectangle(new Point(((Control)this).ClientRectangle.X + num, ((Control)ℳ).Bottom + num), new Size(((Control)this).ClientRectangle.Width - num * 2, ((Control)ℴ).Top - ((Control)ℳ).Bottom - num * 2));
		}
	}

	private void _2158()
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		if (_2141 != null)
		{
			((Control)this).Controls.Remove((Control)(object)_2141);
			((Component)(object)_2141).Dispose();
			_2141 = null;
		}
		foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control val = item;
			if (val is NavigationPanePanel && ((NavigationPanePanel)(object)val).ParentItem == CheckedButton)
			{
				val.Visible = true;
				if (((Component)this).DesignMode)
				{
					TypeDescriptor.GetProperties(val)["Visible"].SetValue(val, true);
				}
				val.BringToFront();
				break;
			}
		}
	}

	private void _2159()
	{
		if (ℶ)
		{
			_215F();
		}
		_215E();
	}

	private Image _215A(bool _215B)
	{
		if (_215B)
		{
			if (ℽ != null)
			{
				return ℽ;
			}
			return ℹ;
		}
		if (_213B != null)
		{
			return _213B;
		}
		return ℷ;
	}

	private Image _215C(bool _215B)
	{
		if (_215B)
		{
			if (ℾ != null)
			{
				return ℾ;
			}
			return _213A;
		}
		if (ℼ != null)
		{
			return ℼ;
		}
		return ℸ;
	}

	internal void _215D()
	{
		ColorSchemeStyleChanged();
	}

	private void _215E()
	{
		LocalizationManager localizationManager = new LocalizationManager(ℴ);
		if (Ӫ)
		{
			if (ℳ.ButtonAlignment == eTitleButtonAlignment.Right || _213B != null)
			{
				ℳ.ExpandChangeButton.Image = _215A(_215B: false);
				ℳ.ExpandChangeButton.HoverImage = _215A(_215B: true);
			}
			else
			{
				ℳ.ExpandChangeButton.Image = _215C(_215B: false);
				ℳ.ExpandChangeButton.HoverImage = _215C(_215B: true);
			}
			ℳ.ExpandChangeButton.Tooltip = localizationManager.GetDefaultLocalizedString(LocalizationKeys.NavPaneCollapseButtonTooltip);
		}
		else
		{
			if (ℳ.ButtonAlignment == eTitleButtonAlignment.Right || ℼ != null)
			{
				ℳ.ExpandChangeButton.Image = _215C(_215B: false);
				ℳ.ExpandChangeButton.HoverImage = _215C(_215B: true);
			}
			else
			{
				ℳ.ExpandChangeButton.Image = _215A(_215B: false);
				ℳ.ExpandChangeButton.HoverImage = _215A(_215B: true);
			}
			ℳ.ExpandChangeButton.Tooltip = localizationManager.GetDefaultLocalizedString(LocalizationKeys.NavPaneExpandButtonTooltip);
		}
		ℳ.UpdateButtonPosition();
	}

	private void _215F()
	{
		if (ℷ != null)
		{
			ℷ.Dispose();
		}
		if (ℸ != null)
		{
			ℸ.Dispose();
		}
		if (ℹ != null)
		{
			ℹ.Dispose();
		}
		if (_213A != null)
		{
			_213A.Dispose();
		}
		ℷ = ឱ.ឳ(_17B4: true, ℳ.ColorScheme.PanelText, _17B5: false);
		ℹ = ឱ.ឳ(_17B4: true, ℳ.ColorScheme.ItemHotText, _17B5: false);
		ℸ = ឱ.ឳ(_17B4: false, ℳ.ColorScheme.PanelText, _17B5: false);
		_213A = ឱ.ឳ(_17B4: false, ℳ.ColorScheme.ItemHotText, _17B5: false);
	}

	public void LoadLayout(string fileName)
	{
		ℴ.LoadLayout(fileName);
	}

	public void LoadLayout(XmlElement xmlParent)
	{
		ℴ.LoadLayout(xmlParent);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void StyleManagerStyleChanged(eDotNetBarStyle newStyle)
	{
		ColorSchemeStyleChanged();
		if (((Control)this).IsHandleCreated && ((Control)this).Visible)
		{
			((Control)this).Invalidate(true);
		}
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		ColorSchemeStyleChanged();
		((Control)this).OnHandleCreated(e);
	}

	public void ColorSchemeStyleChanged()
	{
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		ColorScheme colorScheme = null;
		eDotNetBarStyle effectiveStyle = ℴ.ItemsContainer.EffectiveStyle;
		if (BarFunctions.IsOffice2007Style(effectiveStyle) || (ℴ.ColorScheme != null && ℴ.ColorScheme.PredefinedColorScheme == ePredefinedColorScheme.SystemColors))
		{
			colorScheme = ℴ.GetColorScheme();
		}
		foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control val = item;
			if (val is PanelEx)
			{
				PanelEx component = val as PanelEx;
				TypeDescriptor.GetProperties(component)["ColorSchemeStyle"].SetValue(component, effectiveStyle);
				if (colorScheme != null)
				{
					((PanelEx)(object)val).ColorScheme = colorScheme;
				}
			}
		}
		if (effectiveStyle == eDotNetBarStyle.Office2010)
		{
			ℳ.Style.Border = eBorderType.None;
		}
		else if (BarFunctions.IsOffice2007Style(effectiveStyle))
		{
			ℳ.Style.Border = eBorderType.RaisedInner;
		}
		else
		{
			ℳ.Style.Border = eBorderType.SingleLine;
		}
		_215F();
		_215E();
		if (effectiveStyle == eDotNetBarStyle.Office2010)
		{
			((ScrollableControl)this).DockPadding.All = 0;
			ℴ.BackgroundStyle.BorderWidth = 0;
			ℴ.BackgroundStyle.BorderColor.Color = Color.Transparent;
		}
		else
		{
			((ScrollableControl)this).DockPadding.All = 1;
			ℴ.BackgroundStyle.BorderWidth = 1;
			ℴ.BackgroundStyle.BorderColor.ColorSchemePart = eColorSchemePart.PanelBorder;
		}
	}

	private void _2160(object _٠, EventArgs ؿ)
	{
		if (_09BC != null)
		{
			_09BC(_٠, ؿ);
		}
	}

	private void _2161(object _٠, OptionGroupChangingEventArgs ؿ)
	{
		NavigationPanePanel oldpanel = null;
		NavigationPanePanel navigationPanePanel = null;
		if (ؿ.OldChecked != null)
		{
			oldpanel = GetPanel((ButtonItem)ؿ.OldChecked);
		}
		if (ؿ.NewChecked != null)
		{
			navigationPanePanel = GetPanel((ButtonItem)ؿ.NewChecked);
		}
		if (navigationPanePanel != null)
		{
			PanelChangingEventArgs panelChangingEventArgs = new PanelChangingEventArgs(oldpanel, navigationPanePanel);
			InvokePanelChanging(panelChangingEventArgs);
			ؿ.Cancel = panelChangingEventArgs.Cancel;
		}
	}

	protected virtual void InvokePanelChanging(PanelChangingEventArgs e)
	{
		if (_2144 != null)
		{
			_2144(this, e);
		}
	}

	private void ـ()
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Expected O, but got Unknown
		ℳ = new PanelExTitle();
		ℴ = new NavigationBar();
		((ISupportInitialize)ℴ).BeginInit();
		((Control)ℳ).Dock = (DockStyle)1;
		((Control)ℳ).Font = new Font("Tahoma", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)ℳ).Name = "panelTitle";
		((Control)ℳ).Size = new Size(150, 24);
		ℳ.Style.Alignment = (StringAlignment)0;
		ℳ.Style.BackColor1.ColorSchemePart = eColorSchemePart.PanelBackground;
		ℳ.Style.BackColor2.ColorSchemePart = eColorSchemePart.PanelBackground2;
		ℳ.Style.Border = eBorderType.SingleLine;
		ℳ.Style.BorderSide = eBorderSide.Bottom;
		ℳ.Style.BorderColor.ColorSchemePart = eColorSchemePart.PanelBorder;
		ℳ.Style.ForeColor.ColorSchemePart = eColorSchemePart.PanelText;
		ℳ.Style.GradientAngle = 90;
		ℳ.Style.MarginLeft = 4;
		ℳ.StyleMouseDown.Alignment = (StringAlignment)0;
		ℳ.StyleMouseOver.Alignment = (StringAlignment)0;
		((Control)ℳ).TabIndex = 0;
		ℳ.ExpandButtonVisible = false;
		((Control)ℳ).Text = "Title";
		ℴ.BackgroundStyle.BackColor1.ColorSchemePart = eColorSchemePart.PanelBackground;
		ℴ.BackgroundStyle.Border = eBorderType.SingleLine;
		ℴ.BackgroundStyle.BorderSide = eBorderSide.Top;
		ℴ.BackgroundStyle.BorderColor.ColorSchemePart = eColorSchemePart.PanelBorder;
		((Control)ℴ).Dock = (DockStyle)2;
		((Control)ℴ).Location = new Point(0, 128);
		((Control)ℴ).Name = "navigationBar1";
		((Control)ℴ).Size = new Size(150, 32);
		((Control)ℴ).TabIndex = 1;
		((Control)ℴ).Text = "navBar";
		((Control)ℴ).Resize += _214F;
		ℴ.ItemClick += _2160;
		ℴ.OptionGroupChanging += _2161;
		((Control)ℴ).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)this).Controls.AddRange((Control[])(object)new Control[2]
		{
			ℴ,
			(Control)ℳ
		});
		((Control)this).Name = "NavigationPane";
		((Control)this).Size = new Size(150, 192);
		((ScrollableControl)this).DockPadding.All = 1;
		((ISupportInitialize)ℴ).EndInit();
	}
}
