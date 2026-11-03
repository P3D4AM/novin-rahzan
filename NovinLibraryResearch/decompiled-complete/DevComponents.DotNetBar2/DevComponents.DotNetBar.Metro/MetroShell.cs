using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using DevComponents.DotNetBar.Rendering;
using DevComponents.DotNetBar.Ribbon;
using DevComponents.DotNetBar.TextMarkup;

namespace DevComponents.DotNetBar.Metro;

[ComVisible(false)]
[ToolboxItem(true)]
[Designer("DevComponents.DotNetBar.Design.MetroShellDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[ToolboxBitmap(typeof(MetroShell), "MetroShell.ico")]
public class MetroShell : ContainerControl
{
	private const string Ꮰ = "syscustomizepopupmenu";

	private CustomizeMenuPopupEventHandler Ꮱ;

	private CustomizeMenuPopupEventHandler Ꮲ;

	private CustomizeMenuPopupEventHandler Ꮳ;

	private DotNetBarManager.LocalizeStringEventHandler _09D4;

	private EventHandler _09BC;

	private QatCustomizeDialogEventHandler Ꮴ;

	private QatCustomizeDialogEventHandler Ꮵ;

	private EventHandler Ꮶ;

	private EventHandler Ꮷ;

	private MarkupLinkClickEventHandler _0D83;

	private EventHandler Ꮸ;

	private EventHandler Ꮹ;

	private MetroTabStrip _Ꮺ;

	private bool _09FB;

	private ShadowPaintInfo Ꮻ;

	private bool _Ꮼ = true;

	private eMetroCategorizeMode Ꮽ;

	private Ꮮ Ꮾ;

	private int Ꮿ;

	private Ყ Ᏸ;

	private bool Ᏹ;

	private RibbonLocalization Ᏺ = new RibbonLocalization();

	private bool _Ᏻ;

	private bool Ᏼ = true;

	private ContextMenuBar แ;

	private SubItemsCollection _Ᏽ = new SubItemsCollection(null);

	public static readonly string SysQatCustomizeItemName = "sysCustomizeQuickAccessToolbar";

	public static readonly string SysQatAddToItemName = "sysAddToQuickAccessToolbar";

	public static readonly string SysQatRemoveFromItemName = "sysRemoveFromQuickAccessToolbar";

	public static readonly string SysQatPlaceItemName = "sysPlaceQuickAccessToolbar";

	public static readonly string SysQatCustomizeLabelName = "sysCustomizeQuickAccessToolbarLabel";

	public static readonly string SysFrequentlyQatNamePart = "sysQatFrequent_";

	private bool __13F6;

	private bool __13F7 = true;

	private List<WeakReference> _ᏸ = new List<WeakReference>();

	private eMetroTabStripDock _ᏹ;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public SubItemsCollection QatFrequentCommands => _Ᏽ;

	[DefaultValue(true)]
	[Category("Behavior")]
	[Browsable(true)]
	[Description("Indicates whether KeyTips functionality is enabled.")]
	public bool KeyTipsEnabled
	{
		get
		{
			return _Ꮺ.KeyTipsEnabled;
		}
		set
		{
			_Ꮺ.KeyTipsEnabled = value;
		}
	}

	internal bool Ꮬ => ((Component)this).DesignMode;

	[Category("Appearance")]
	[Description("Gets or sets whether anti-aliasing is used while painting.")]
	[DefaultValue(true)]
	[Browsable(true)]
	public bool AntiAlias
	{
		get
		{
			return _Ꮺ.AntiAlias;
		}
		set
		{
			_Ꮺ.AntiAlias = value;
			((Control)this).Invalidate();
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether Form.Icon is shown in top-left corner.")]
	[Category("Appearance")]
	public bool ShowIcon
	{
		get
		{
			return _Ꮺ.ShowIcon;
		}
		set
		{
			_Ꮺ.ShowIcon = value;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[DefaultValue("")]
	[EditorBrowsable(EditorBrowsableState.Always)]
	[Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Description("Indicates text displayed on Title bar instead of the Form.Text property.")]
	public string TitleText
	{
		get
		{
			return _Ꮺ.TitleText;
		}
		set
		{
			_Ꮺ.TitleText = value;
		}
	}

	[Description("Indicates Context menu bar associated with the MetroShell control which is used as part of Global Items feature.")]
	[Category("Data")]
	[DefaultValue(null)]
	public ContextMenuBar GlobalContextMenuBar
	{
		get
		{
			return แ;
		}
		set
		{
			if (แ != null)
			{
				แ.GlobalParentComponent = null;
			}
			แ = value;
			if (แ != null)
			{
				แ.GlobalParentComponent = (Component)(object)this;
			}
		}
	}

	[DefaultValue(false)]
	[Category("Caption")]
	[Description("Indicates whether custom caption and quick access toolbar provided by the control is visible.")]
	[Browsable(true)]
	public bool CaptionVisible
	{
		get
		{
			return _Ꮺ.CaptionVisible;
		}
		set
		{
			_Ꮺ.CaptionVisible = value;
		}
	}

	[Description("Indicates font for the form caption text when CaptionVisible=true.")]
	[DefaultValue(null)]
	[Browsable(true)]
	[Category("Caption")]
	public Font CaptionFont
	{
		get
		{
			return _Ꮺ.CaptionFont;
		}
		set
		{
			_Ꮺ.CaptionFont = value;
		}
	}

	[DefaultValue(0)]
	[Browsable(true)]
	[Description("Indicates explicit height of the caption provided by control")]
	[Category("Caption")]
	public int CaptionHeight
	{
		get
		{
			return _Ꮺ.CaptionHeight;
		}
		set
		{
			_Ꮺ.CaptionHeight = value;
			RecalcLayout();
		}
	}

	[Browsable(true)]
	[Description("Indicates font that is used to display Key Tips (accelerator keys) when they are displayed.")]
	[Category("Appearance")]
	[DefaultValue(null)]
	public virtual Font KeyTipsFont
	{
		get
		{
			return _Ꮺ.KeyTipsFont;
		}
		set
		{
			_Ꮺ.KeyTipsFont = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Description("Gets or sets bar background style.")]
	[Category("Style")]
	public ElementStyle BackgroundStyle => _Ꮺ.BackgroundStyle;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public MetroTabItem SelectedTab
	{
		get
		{
			return _Ꮺ.SelectedTab;
		}
		set
		{
			if (value != null)
			{
				value.Checked = true;
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public MetroTabStrip MetroTabStrip => _Ꮺ;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public SubItemsCollection Items => _Ꮺ.Items;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(false)]
	public SubItemsCollection QuickToolbarItems => ᐂ();

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public eDotNetBarStyle EffectiveStyle => _Ꮺ._൨;

	[Description("Indicates whether mouse wheel scrolls through the Metro tabs.")]
	[DefaultValue(true)]
	[Category("Behavior")]
	public bool MouseWheelTabScrollEnabled
	{
		get
		{
			return __13F7;
		}
		set
		{
			__13F7 = value;
		}
	}

	[Description("ImageList for images used on Items. Images specified here will always be used on menu-items and are by default used on all Bars.")]
	[DefaultValue(null)]
	[Browsable(true)]
	[Category("Data")]
	public ImageList Images
	{
		get
		{
			return _Ꮺ.Images;
		}
		set
		{
			_Ꮺ.Images = value;
		}
	}

	[DefaultValue(null)]
	[Description("ImageList for medium-sized images used on Items.")]
	[Browsable(true)]
	[Category("Data")]
	public ImageList ImagesMedium
	{
		get
		{
			return _Ꮺ.ImagesMedium;
		}
		set
		{
			_Ꮺ.ImagesMedium = value;
		}
	}

	[Description("ImageList for large-sized images used on Items.")]
	[Browsable(true)]
	[Category("Data")]
	[DefaultValue(null)]
	public ImageList ImagesLarge
	{
		get
		{
			return _Ꮺ.ImagesLarge;
		}
		set
		{
			_Ꮺ.ImagesLarge = value;
		}
	}

	[DefaultValue(false)]
	[Category("Behavior")]
	[Browsable(true)]
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

	private bool Ꮭ
	{
		get
		{
			if (CanCustomize)
			{
				if (!CaptionVisible)
				{
					return _Ᏻ;
				}
				return true;
			}
			return false;
		}
	}

	internal Ꮮ Ꮮ => Ꮾ;

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Quick Access Toolbar")]
	[Description("Indicates whether control can be customized. Caption must be visible for customization to be fully enabled.")]
	public bool CanCustomize
	{
		get
		{
			return _Ꮺ.CanCustomize;
		}
		set
		{
			_Ꮺ.CanCustomize = value;
		}
	}

	[Description("Indicates whether external implementation for MetroToolbar and menu item customization will be used for customizing the MetroShell control.")]
	[DefaultValue(false)]
	[Browsable(true)]
	[Category("Quick Access Toolbar")]
	public bool UseExternalCustomization
	{
		get
		{
			return _Ᏻ;
		}
		set
		{
			_Ᏻ = value;
		}
	}

	[Category("Quick Access Toolbar")]
	[Description("Indicates whether customize dialog is used to customize the quick access toolbar.")]
	[DefaultValue(true)]
	[Browsable(true)]
	public bool UseCustomizeDialog
	{
		get
		{
			return _Ꮼ;
		}
		set
		{
			_Ꮼ = value;
		}
	}

	[Description("Indicates categorization mode for the items on Quick Access Toolbar customize dialog box.")]
	[Browsable(true)]
	[Category("Quick Access Toolbar")]
	[DefaultValue(eMetroCategorizeMode.Toolbar)]
	public eMetroCategorizeMode CategorizeMode
	{
		get
		{
			return Ꮽ;
		}
		set
		{
			Ꮽ = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public string QatLayout
	{
		get
		{
			return ᐓ();
		}
		set
		{
			ᐔ(value);
		}
	}

	internal List<IOwner> Ꮯ
	{
		get
		{
			List<IOwner> list = new List<IOwner>();
			foreach (WeakReference item in _ᏸ)
			{
				if (item.IsAlive)
				{
					list.Add((IOwner)item.Target);
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
			return Ᏹ;
		}
		set
		{
			Ᏹ = value;
		}
	}

	[Browsable(true)]
	[Category("Localization")]
	[Description("Gets system text used by the component..")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[DevCoBrowsable(true)]
	[NotifyParentProperty(true)]
	public RibbonLocalization SystemText => Ᏺ;

	protected override Size DefaultSize => new Size(200, 100);

	[DefaultValue(eMetroTabStripDock.Top)]
	[Description("Indicates side tab-strip is docked to.")]
	[Category("Appearance")]
	public eMetroTabStripDock TabStripDock
	{
		get
		{
			return _ᏹ;
		}
		set
		{
			_ᏹ = value;
			if (_ᏹ == eMetroTabStripDock.Top)
			{
				((Control)_Ꮺ).Dock = (DockStyle)1;
			}
			else if (_ᏹ == eMetroTabStripDock.Bottom)
			{
				((Control)_Ꮺ).Dock = (DockStyle)2;
			}
		}
	}

	[Category("Appearance")]
	[Description("Indicates whether SETTINGS button is visible.")]
	[DefaultValue(true)]
	public bool SettingsButtonVisible
	{
		get
		{
			return _Ꮺ.ᐙ.ᐯ;
		}
		set
		{
			if (value != _Ꮺ.ᐙ.ᐯ)
			{
				_Ꮺ.ᐙ.ᐯ = value;
			}
		}
	}

	[Description("Indicates whether HELP button is visible.")]
	[Category("Appearance")]
	[DefaultValue(true)]
	public bool HelpButtonVisible
	{
		get
		{
			return _Ꮺ.ᐙ.ᐰ;
		}
		set
		{
			if (value != _Ꮺ.ᐙ.ᐰ)
			{
				_Ꮺ.ᐙ.ᐰ = value;
			}
		}
	}

	[Description("Indicates SETTINGS button text")]
	[Category("Appearance")]
	[DefaultValue("")]
	public string SettingsButtonText
	{
		get
		{
			return _Ꮺ.ᐙ.ᐱ;
		}
		set
		{
			_Ꮺ.ᐙ.ᐱ = value;
		}
	}

	[Description("Indicates HELP button text")]
	[DefaultValue("")]
	[Category("Appearance")]
	public string HelpButtonText
	{
		get
		{
			return _Ꮺ.ᐙ.ᐲ;
		}
		set
		{
			_Ꮺ.ᐙ.ᐲ = value;
		}
	}

	[Description("Indicates the font tab items are displayed with.")]
	[AmbientValue(null)]
	[Localizable(true)]
	[Category("Appearance")]
	public Font TabStripFont
	{
		get
		{
			return ((Control)_Ꮺ).Font;
		}
		set
		{
			((Control)_Ꮺ).Font = value;
		}
	}

	public event CustomizeMenuPopupEventHandler BeforeCustomizeMenuPopup
	{
		add
		{
			CustomizeMenuPopupEventHandler customizeMenuPopupEventHandler = Ꮱ;
			CustomizeMenuPopupEventHandler customizeMenuPopupEventHandler2;
			do
			{
				customizeMenuPopupEventHandler2 = customizeMenuPopupEventHandler;
				CustomizeMenuPopupEventHandler value2 = (CustomizeMenuPopupEventHandler)Delegate.Combine(customizeMenuPopupEventHandler2, value);
				customizeMenuPopupEventHandler = Interlocked.CompareExchange(ref Ꮱ, value2, customizeMenuPopupEventHandler2);
			}
			while ((object)customizeMenuPopupEventHandler != customizeMenuPopupEventHandler2);
		}
		remove
		{
			CustomizeMenuPopupEventHandler customizeMenuPopupEventHandler = Ꮱ;
			CustomizeMenuPopupEventHandler customizeMenuPopupEventHandler2;
			do
			{
				customizeMenuPopupEventHandler2 = customizeMenuPopupEventHandler;
				CustomizeMenuPopupEventHandler value2 = (CustomizeMenuPopupEventHandler)Delegate.Remove(customizeMenuPopupEventHandler2, value);
				customizeMenuPopupEventHandler = Interlocked.CompareExchange(ref Ꮱ, value2, customizeMenuPopupEventHandler2);
			}
			while ((object)customizeMenuPopupEventHandler != customizeMenuPopupEventHandler2);
		}
	}

	public event CustomizeMenuPopupEventHandler BeforeAddItemToQuickAccessToolbar
	{
		add
		{
			CustomizeMenuPopupEventHandler customizeMenuPopupEventHandler = Ꮲ;
			CustomizeMenuPopupEventHandler customizeMenuPopupEventHandler2;
			do
			{
				customizeMenuPopupEventHandler2 = customizeMenuPopupEventHandler;
				CustomizeMenuPopupEventHandler value2 = (CustomizeMenuPopupEventHandler)Delegate.Combine(customizeMenuPopupEventHandler2, value);
				customizeMenuPopupEventHandler = Interlocked.CompareExchange(ref Ꮲ, value2, customizeMenuPopupEventHandler2);
			}
			while ((object)customizeMenuPopupEventHandler != customizeMenuPopupEventHandler2);
		}
		remove
		{
			CustomizeMenuPopupEventHandler customizeMenuPopupEventHandler = Ꮲ;
			CustomizeMenuPopupEventHandler customizeMenuPopupEventHandler2;
			do
			{
				customizeMenuPopupEventHandler2 = customizeMenuPopupEventHandler;
				CustomizeMenuPopupEventHandler value2 = (CustomizeMenuPopupEventHandler)Delegate.Remove(customizeMenuPopupEventHandler2, value);
				customizeMenuPopupEventHandler = Interlocked.CompareExchange(ref Ꮲ, value2, customizeMenuPopupEventHandler2);
			}
			while ((object)customizeMenuPopupEventHandler != customizeMenuPopupEventHandler2);
		}
	}

	public event CustomizeMenuPopupEventHandler BeforeRemoveItemFromQuickAccessToolbar
	{
		add
		{
			CustomizeMenuPopupEventHandler customizeMenuPopupEventHandler = Ꮳ;
			CustomizeMenuPopupEventHandler customizeMenuPopupEventHandler2;
			do
			{
				customizeMenuPopupEventHandler2 = customizeMenuPopupEventHandler;
				CustomizeMenuPopupEventHandler value2 = (CustomizeMenuPopupEventHandler)Delegate.Combine(customizeMenuPopupEventHandler2, value);
				customizeMenuPopupEventHandler = Interlocked.CompareExchange(ref Ꮳ, value2, customizeMenuPopupEventHandler2);
			}
			while ((object)customizeMenuPopupEventHandler != customizeMenuPopupEventHandler2);
		}
		remove
		{
			CustomizeMenuPopupEventHandler customizeMenuPopupEventHandler = Ꮳ;
			CustomizeMenuPopupEventHandler customizeMenuPopupEventHandler2;
			do
			{
				customizeMenuPopupEventHandler2 = customizeMenuPopupEventHandler;
				CustomizeMenuPopupEventHandler value2 = (CustomizeMenuPopupEventHandler)Delegate.Remove(customizeMenuPopupEventHandler2, value);
				customizeMenuPopupEventHandler = Interlocked.CompareExchange(ref Ꮳ, value2, customizeMenuPopupEventHandler2);
			}
			while ((object)customizeMenuPopupEventHandler != customizeMenuPopupEventHandler2);
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

	[Description("Occurs when Item on metro tab strip or quick access toolbar is clicked.")]
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

	public event QatCustomizeDialogEventHandler BeforeQatCustomizeDialog
	{
		add
		{
			QatCustomizeDialogEventHandler qatCustomizeDialogEventHandler = Ꮴ;
			QatCustomizeDialogEventHandler qatCustomizeDialogEventHandler2;
			do
			{
				qatCustomizeDialogEventHandler2 = qatCustomizeDialogEventHandler;
				QatCustomizeDialogEventHandler value2 = (QatCustomizeDialogEventHandler)Delegate.Combine(qatCustomizeDialogEventHandler2, value);
				qatCustomizeDialogEventHandler = Interlocked.CompareExchange(ref Ꮴ, value2, qatCustomizeDialogEventHandler2);
			}
			while ((object)qatCustomizeDialogEventHandler != qatCustomizeDialogEventHandler2);
		}
		remove
		{
			QatCustomizeDialogEventHandler qatCustomizeDialogEventHandler = Ꮴ;
			QatCustomizeDialogEventHandler qatCustomizeDialogEventHandler2;
			do
			{
				qatCustomizeDialogEventHandler2 = qatCustomizeDialogEventHandler;
				QatCustomizeDialogEventHandler value2 = (QatCustomizeDialogEventHandler)Delegate.Remove(qatCustomizeDialogEventHandler2, value);
				qatCustomizeDialogEventHandler = Interlocked.CompareExchange(ref Ꮴ, value2, qatCustomizeDialogEventHandler2);
			}
			while ((object)qatCustomizeDialogEventHandler != qatCustomizeDialogEventHandler2);
		}
	}

	public event QatCustomizeDialogEventHandler AfterQatCustomizeDialog
	{
		add
		{
			QatCustomizeDialogEventHandler qatCustomizeDialogEventHandler = Ꮵ;
			QatCustomizeDialogEventHandler qatCustomizeDialogEventHandler2;
			do
			{
				qatCustomizeDialogEventHandler2 = qatCustomizeDialogEventHandler;
				QatCustomizeDialogEventHandler value2 = (QatCustomizeDialogEventHandler)Delegate.Combine(qatCustomizeDialogEventHandler2, value);
				qatCustomizeDialogEventHandler = Interlocked.CompareExchange(ref Ꮵ, value2, qatCustomizeDialogEventHandler2);
			}
			while ((object)qatCustomizeDialogEventHandler != qatCustomizeDialogEventHandler2);
		}
		remove
		{
			QatCustomizeDialogEventHandler qatCustomizeDialogEventHandler = Ꮵ;
			QatCustomizeDialogEventHandler qatCustomizeDialogEventHandler2;
			do
			{
				qatCustomizeDialogEventHandler2 = qatCustomizeDialogEventHandler;
				QatCustomizeDialogEventHandler value2 = (QatCustomizeDialogEventHandler)Delegate.Remove(qatCustomizeDialogEventHandler2, value);
				qatCustomizeDialogEventHandler = Interlocked.CompareExchange(ref Ꮵ, value2, qatCustomizeDialogEventHandler2);
			}
			while ((object)qatCustomizeDialogEventHandler != qatCustomizeDialogEventHandler2);
		}
	}

	public event EventHandler AfterQatDialogChangesApplied
	{
		add
		{
			EventHandler eventHandler = Ꮶ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ꮶ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = Ꮶ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ꮶ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler SelectedTabChanged
	{
		add
		{
			EventHandler eventHandler = Ꮷ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ꮷ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = Ꮷ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ꮷ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when text markup link from TitleText markup is clicked.")]
	public event MarkupLinkClickEventHandler TitleTextMarkupLinkClick
	{
		add
		{
			MarkupLinkClickEventHandler markupLinkClickEventHandler = _0D83;
			MarkupLinkClickEventHandler markupLinkClickEventHandler2;
			do
			{
				markupLinkClickEventHandler2 = markupLinkClickEventHandler;
				MarkupLinkClickEventHandler value2 = (MarkupLinkClickEventHandler)Delegate.Combine(markupLinkClickEventHandler2, value);
				markupLinkClickEventHandler = Interlocked.CompareExchange(ref _0D83, value2, markupLinkClickEventHandler2);
			}
			while ((object)markupLinkClickEventHandler != markupLinkClickEventHandler2);
		}
		remove
		{
			MarkupLinkClickEventHandler markupLinkClickEventHandler = _0D83;
			MarkupLinkClickEventHandler markupLinkClickEventHandler2;
			do
			{
				markupLinkClickEventHandler2 = markupLinkClickEventHandler;
				MarkupLinkClickEventHandler value2 = (MarkupLinkClickEventHandler)Delegate.Remove(markupLinkClickEventHandler2, value);
				markupLinkClickEventHandler = Interlocked.CompareExchange(ref _0D83, value2, markupLinkClickEventHandler2);
			}
			while ((object)markupLinkClickEventHandler != markupLinkClickEventHandler2);
		}
	}

	[Description("Occurs when SETTINGS button, if displayed, is clicked.")]
	public event EventHandler SettingsButtonClick
	{
		add
		{
			EventHandler eventHandler = Ꮸ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ꮸ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = Ꮸ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ꮸ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when HELP button, if displayed, is clicked.")]
	public event EventHandler HelpButtonClick
	{
		add
		{
			EventHandler eventHandler = Ꮹ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ꮹ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = Ꮹ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ꮹ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	protected virtual void OnSettingsButtonClick(EventArgs e)
	{
		Ꮸ?.Invoke(this, e);
	}

	internal void ᏺ(EventArgs ؿ)
	{
		OnSettingsButtonClick(ؿ);
	}

	protected virtual void OnHelpButtonClick(EventArgs e)
	{
		Ꮹ?.Invoke(this, e);
	}

	internal void ᏻ(EventArgs ؿ)
	{
		OnHelpButtonClick(ؿ);
	}

	public MetroShell()
	{
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		_ = GlobalManager.Renderer;
		_Ᏽ._0AF0 = true;
		_Ᏽ.AllowParentRemove = false;
		((Control)this).SetStyle((ControlStyles)(0x2010 | DisplayHelp.DoubleBufferFlag | 2 | 4), true);
		_Ꮺ = new MetroTabStrip();
		((Control)_Ꮺ).Dock = (DockStyle)1;
		((Control)_Ꮺ).Height = 32;
		_Ꮺ.ItemAdded += _13FF;
		_Ꮺ.LocalizeString += _13FE;
		_Ꮺ.ItemClick += ᏽ;
		_Ꮺ.ButtonCheckedChanged += ᏼ;
		_Ꮺ.TitleTextMarkupLinkClick += ᐁ;
		((Control)this).Controls.Add((Control)(object)_Ꮺ);
		TabStop = false;
		((ScrollableControl)this).DockPadding.Bottom = Ꮿ;
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
		((ScrollableControl)this).DockPadding.Left = 0;
	}

	protected virtual void OnSelectedTabChanged(EventArgs e)
	{
		if (Ꮷ != null)
		{
			Ꮷ(this, e);
		}
	}

	private void ᏼ(object _٠, EventArgs ؿ)
	{
		if (_٠ is MetroTabItem && ((MetroTabItem)_٠).Checked)
		{
			OnSelectedTabChanged(new EventArgs());
		}
	}

	private void ᏽ(object _٠, EventArgs ؿ)
	{
		if (_09BC != null)
		{
			_09BC(_٠, ؿ);
		}
	}

	private void _13FE(object _٠, LocalizeEventArgs ؿ)
	{
		if (_09D4 != null)
		{
			_09D4(this, ؿ);
		}
	}

	internal Office2007ColorTable ጪ()
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
		_Ꮺ.RecalcLayout();
		if (Ꮾ != null && ((Control)Ꮾ).Visible)
		{
			Ꮾ.RecalcLayout();
		}
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		((Control)this).OnHandleCreated(e);
		foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control val = item;
			_ = val.Handle;
			if (!(val is MetroTabPanel))
			{
				continue;
			}
			foreach (Control item2 in (ArrangedElementCollection)val.Controls)
			{
				Control val2 = item2;
				_ = val2.Handle;
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

	private void _13FF(object _٠, EventArgs ؿ)
	{
		if (((Component)this).DesignMode || !(_٠ is MetroTabItem))
		{
			return;
		}
		MetroTabItem metroTabItem = _٠ as MetroTabItem;
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
		_Ꮺ.SetDesignMode(mode: true);
	}

	private ElementStyle _1400()
	{
		return _Ꮺ.ඖ();
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected O, but got Unknown
		ElementStyle elementStyle = _1400();
		if ((elementStyle.BackColor.A < byte.MaxValue && !elementStyle.BackColor.IsEmpty) || ((Control)this).BackColor == Color.Transparent || ((Control)this).BackgroundImage != null)
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
		ElementStyleDisplayInfo e2 = new ElementStyleDisplayInfo(elementStyle, e.Graphics, ((Control)this).ClientRectangle);
		ElementStyleDisplay.PaintBackground(e2);
	}

	protected override void WndProc(ref Message m)
	{
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Invalid comparison between Unknown and I4
		if (((Message)(ref m)).Msg == 132)
		{
			int x = _0EFE._0F0B(((Message)(ref m)).LParam);
			int y = _0EFE._0F0D(((Message)(ref m)).LParam);
			Point pt = ((Control)this).PointToClient(new Point(x, y));
			if (CaptionVisible && _Ꮺ != null && !_Ꮺ._൩ && !(((Control)this).FindForm() is MetroAppForm { Sizable: false }))
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
			if (BarFunctions.IsWindows7 && _Ꮺ != null && _Ꮺ._൩ && _Ꮺ._0D65.Contains(pt))
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

	private void ᐁ(object _٠, MarkupLinkClickEventArgs ؿ)
	{
		if (_0D83 != null)
		{
			_0D83(this, new MarkupLinkClickEventArgs(ؿ.Name, ؿ.HRef));
		}
	}

	private SubItemsCollection ᐂ()
	{
		return _Ꮺ.QuickToolbarItems;
	}

	private static BaseItem ᐃ(MetroShell ᆰ)
	{
		BaseItem result = null;
		for (int i = 0; i < ᆰ.QuickToolbarItems.Count; i++)
		{
			if (ᆰ.QuickToolbarItems[i] is MetroAppButton)
			{
				result = ᆰ.QuickToolbarItems[i];
				break;
			}
		}
		return result;
	}

	protected override void OnBackColorChanged(EventArgs e)
	{
		((Control)this).OnBackColorChanged(e);
	}

	internal void ට(BaseItem _06EC)
	{
		_Ꮺ.ට(_06EC);
		if (_06EC != null && !(_06EC is MetroTabItem) && !_06EC.IsContainer && !_06EC.Name.StartsWith("sysgallery") && !(_06EC is CheckBoxItem) && _06EC.AutoCollapseOnClick && _06EC is PopupItem)
		{
			_ = _06EC.Expanded;
		}
	}

	protected override void OnVisibleChanged(EventArgs e)
	{
		if (!((Component)this).DesignMode && !__13F6)
		{
			Ӕ ӕ = new Ӕ();
			ӕ.Ḕ();
			((Component)(object)ӕ).Dispose();
			__13F6 = true;
		}
		((ScrollableControl)this).OnVisibleChanged(e);
	}

	protected override void OnTabStopChanged(EventArgs e)
	{
		((Control)this).OnTabStopChanged(e);
		((Control)_Ꮺ).TabStop = TabStop;
	}

	internal void ᐄ(MetroTabStrip ᐅ, int و, int ى)
	{
		if (Ꮭ)
		{
			BaseItem baseItem = ᐅ.HitTest(و, ى);
			if (baseItem != null && baseItem.CanCustomize && !baseItem.SystemItem)
			{
				ᐆ(baseItem, ᐇ: true);
			}
		}
	}

	internal virtual void ᐆ(object ໄ, bool ᐇ)
	{
		if (ໄ == null || !_Ꮼ)
		{
			return;
		}
		_Ꮺ._0A4A("syscustomizepopupmenu");
		ButtonItem buttonItem = new ButtonItem("syscustomizepopupmenu");
		buttonItem.Style = eDotNetBarStyle.StyleManagerControlled;
		buttonItem.SetOwner(_Ꮺ);
		if (ᐌ(ໄ as BaseItem) && !_Ᏻ)
		{
			if (ໄ is BaseItem && QuickToolbarItems.Contains((BaseItem)ໄ))
			{
				ButtonItem buttonItem2 = new ButtonItem(SysQatRemoveFromItemName);
				buttonItem2.Text = SystemText.QatRemoveItemText;
				buttonItem2.Click += ᐎ;
				buttonItem2.Tag = ໄ;
				buttonItem.SubItems.Add(buttonItem2);
			}
			else
			{
				BaseItem baseItem = ໄ as BaseItem;
				ButtonItem buttonItem3 = new ButtonItem(SysQatAddToItemName);
				buttonItem3.Text = SystemText.QatAddItemText;
				buttonItem3.Click += ᐏ;
				buttonItem3.Tag = ໄ;
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
						val.VisibleChanged += ᐈ;
					}
				}
			}
		}
		if (_Ꮼ)
		{
			ButtonItem buttonItem4 = new ButtonItem(SysQatCustomizeItemName);
			buttonItem4.Text = SystemText.QatCustomizeText;
			buttonItem4.BeginGroup = true;
			buttonItem4.Click += ᐋ;
			buttonItem.SubItems.Add(buttonItem4);
		}
		RibbonCustomizeEventArgs ribbonCustomizeEventArgs = new RibbonCustomizeEventArgs(ໄ, buttonItem);
		OnBeforeCustomizeMenuPopup(ribbonCustomizeEventArgs);
		if (ribbonCustomizeEventArgs.Cancel)
		{
			buttonItem.Dispose();
			return;
		}
		((IOwnerMenuSupport)_Ꮺ).RegisterPopup((PopupItem)buttonItem);
		buttonItem.Popup(Control.MousePosition);
	}

	private void ᐈ(object _٠, EventArgs ؿ)
	{
		Control val = (Control)((_٠ is Control) ? _٠ : null);
		if (val != null)
		{
			val.VisibleChanged -= ᐈ;
			_Ꮺ._0A4A("syscustomizepopupmenu");
		}
	}

	private ArrayList ᐉ(bool ᐊ)
	{
		ArrayList arrayList = new ArrayList();
		foreach (BaseItem quickToolbarItem in QuickToolbarItems)
		{
			if (ᐒ(quickToolbarItem) || (quickToolbarItem is ButtonItem && ((ButtonItem)quickToolbarItem).HotTrackingStyle == eHotTrackingStyle.Image && QuickToolbarItems.IndexOf(quickToolbarItem) == 0))
			{
				if (ᐊ && quickToolbarItem is CustomizeItem)
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

	private void ᐋ(object _٠, EventArgs ؿ)
	{
		ShowQatCustomizeDialog();
	}

	private bool ᐌ(BaseItem _06EC)
	{
		if (_06EC == null)
		{
			return false;
		}
		if (!_06EC.CanCustomize || _06EC.SystemItem)
		{
			return false;
		}
		if (_06EC is ItemContainer)
		{
			return false;
		}
		if (_06EC is ButtonItem && QuickToolbarItems.IndexOf(_06EC) == 0)
		{
			return false;
		}
		return true;
	}

	private string ᐍ(RibbonBar ච)
	{
		return "qt_" + ((Control)ච).Name;
	}

	private void ᐎ(object _٠, EventArgs ؿ)
	{
		ButtonItem buttonItem = _٠ as ButtonItem;
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
			Ᏹ = true;
		}
	}

	private void ᐏ(object _٠, EventArgs ؿ)
	{
		ButtonItem buttonItem = _٠ as ButtonItem;
		AddItemToQuickAccessToolbar(buttonItem.Tag);
		buttonItem.Tag = null;
	}

	public void AddItemToQuickAccessToolbar(object originalItem)
	{
		BaseItem baseItem = ᐐ(originalItem);
		if (baseItem != null)
		{
			RibbonCustomizeEventArgs ribbonCustomizeEventArgs = new RibbonCustomizeEventArgs(baseItem, null);
			OnBeforeAddItemToQuickAccessToolbar(ribbonCustomizeEventArgs);
			if (!ribbonCustomizeEventArgs.Cancel)
			{
				QuickToolbarItems.Add(baseItem);
				RecalcLayout();
				Ᏹ = true;
			}
		}
	}

	private BaseItem ᐐ(object ໄ)
	{
		BaseItem baseItem = null;
		if (ໄ is ButtonItem)
		{
			ButtonItem buttonItem = ((ButtonItem)ໄ).Copy() as ButtonItem;
			buttonItem.KeyTips = "";
			buttonItem.ImageFixedSize = Size.Empty;
			buttonItem.FixedSize = Size.Empty;
			buttonItem.ButtonStyle = eButtonStyle.Default;
			buttonItem.ImagePosition = eImagePosition.Left;
			buttonItem.BeginGroup = false;
			buttonItem.UseSmallImage = true;
			buttonItem.ImagePaddingHorizontal = 8;
			buttonItem.ImagePaddingVertical = 6;
			if (buttonItem.TextMarkupBody != null && buttonItem.TextMarkupBody._0CBA)
			{
				buttonItem.Text = ሒ.ሙ(buttonItem.Text);
			}
			if (buttonItem.Image == null && buttonItem.ImageIndex == -1 && buttonItem.Icon == null)
			{
				buttonItem.Image = (Image)(object)BarFunctions.ই("SystemImages.GreenLight.png");
			}
			else
			{
				buttonItem.ImageFixedSize = new Size(16, 16);
			}
			baseItem = buttonItem;
		}
		else if (ໄ is BaseItem)
		{
			baseItem = ((BaseItem)ໄ).Copy();
			baseItem.KeyTips = "";
		}
		return baseItem;
	}

	protected virtual void OnBeforeCustomizeMenuPopup(RibbonCustomizeEventArgs e)
	{
		if (Ꮱ != null)
		{
			Ꮱ(this, e);
		}
	}

	protected virtual void OnBeforeAddItemToQuickAccessToolbar(RibbonCustomizeEventArgs e)
	{
		if (Ꮲ != null)
		{
			Ꮲ(this, e);
		}
	}

	protected virtual void OnBeforeRemoveItemFromQuickAccessToolbar(RibbonCustomizeEventArgs e)
	{
		if (Ꮳ != null)
		{
			Ꮳ(this, e);
		}
	}

	public virtual void ShowQatCustomizeDialog()
	{
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Invalid comparison between Unknown and I4
		Form val = (Form)(object)ᐑ();
		if (Ꮴ != null)
		{
			QatCustomizeDialogEventArgs qatCustomizeDialogEventArgs = new QatCustomizeDialogEventArgs(val);
			Ꮴ(this, qatCustomizeDialogEventArgs);
			if (qatCustomizeDialogEventArgs.Cancel || qatCustomizeDialogEventArgs.Dialog == null)
			{
				((Component)(object)val).Dispose();
				return;
			}
			val = qatCustomizeDialogEventArgs.Dialog;
		}
		if (val is _244B)
		{
			((_244B)(object)val)._244D(this);
		}
		Form val2 = ((Control)this).FindForm();
		val.StartPosition = (FormStartPosition)4;
		DialogResult val3 = val.ShowDialog((IWin32Window)(object)val2);
		if (Ꮵ != null)
		{
			QatCustomizeDialogEventArgs qatCustomizeDialogEventArgs2 = new QatCustomizeDialogEventArgs(val);
			Ꮵ(this, qatCustomizeDialogEventArgs2);
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
		else if (val is _244B obj && obj._244C.DataChanged)
		{
			ApplyQatCustomizePanelChanges(obj._244C);
			((Component)(object)val).Dispose();
		}
	}

	public void ApplyQatCustomizePanelChanges(QatCustomizePanel customizePanel)
	{
		if (customizePanel == null || !customizePanel.DataChanged)
		{
			return;
		}
		_Ꮺ.BeginUpdate();
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
				if (!ᐒ(baseItem))
				{
					if (!itemPanelQat.Items.Contains(baseItem.Name))
					{
						arrayList.Add(baseItem);
					}
					else
					{
						subItemsCollection.__0AF3(baseItem);
					}
				}
			}
			foreach (BaseItem item4 in arrayList)
			{
				QuickToolbarItems.Remove(item4);
			}
			foreach (BaseItem item5 in subItemsCollection)
			{
				QuickToolbarItems.Remove(item5);
			}
			foreach (BaseItem item6 in itemPanelQat.Items)
			{
				if (item6.Tag != null)
				{
					BaseItem item3 = ᐐ(item6.Tag as BaseItem);
					QuickToolbarItems.Add(item3);
					continue;
				}
				BaseItem baseItem3 = subItemsCollection[item6.Name];
				if (baseItem3 != null)
				{
					QuickToolbarItems.Add(baseItem3);
				}
			}
			Ᏹ = true;
		}
		finally
		{
			_Ꮺ.EndUpdate();
		}
		OnAfterQatDialogChangesApplied();
	}

	protected virtual void OnAfterQatDialogChangesApplied()
	{
		if (Ꮶ != null)
		{
			Ꮶ(this, new EventArgs());
		}
	}

	private _244B ᐑ()
	{
		_244B obj = new _244B();
		((Control)obj).Text = SystemText.QatDialogCaption;
		((Control)obj.ẘ).Text = SystemText.QatDialogCancelButton;
		((Control)obj.ẗ).Text = SystemText.QatDialogOkButton;
		((Control)obj._244C.buttonAddToQat).Text = SystemText.QatDialogAddButton;
		((Control)obj._244C.buttonRemoveFromQat).Text = SystemText.QatDialogRemoveButton;
		((Control)obj._244C.labelCategories).Text = SystemText.QatDialogCategoriesLabel;
		((Control)obj._244C.checkQatBelowRibbon).Text = SystemText.QatDialogPlacementCheckbox;
		return obj;
	}

	private bool ᐒ(BaseItem _06EC)
	{
		if (_06EC.SystemItem || _06EC is ItemContainer || _06EC is CustomizeItem || _06EC is SystemCaptionItem)
		{
			return true;
		}
		return false;
	}

	public BaseItem GetApplicationButton()
	{
		return _Ꮺ.ඩ();
	}

	private string ᐓ()
	{
		ArrayList arrayList = ᐉ(ᐊ: false);
		StringBuilder stringBuilder = new StringBuilder();
		foreach (BaseItem item in arrayList)
		{
			if (item.Name != "")
			{
				stringBuilder.Append("," + item.Name);
			}
		}
		return stringBuilder.ToString();
	}

	private void ᐔ(string ᐕ)
	{
		if (ᐕ == "" || ᐕ == null)
		{
			return;
		}
		string[] array = ᐕ.Split(new char[1] { ',' });
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
			BaseItem baseItem = ᐖ(array[i]);
			if (baseItem != null && ᐌ(baseItem))
			{
				arrayList.Add(ᐐ(baseItem));
			}
		}
		ArrayList arrayList2 = ᐉ(ᐊ: false);
		foreach (BaseItem item3 in arrayList2)
		{
			QuickToolbarItems.Remove(item3);
		}
		foreach (BaseItem item4 in arrayList)
		{
			QuickToolbarItems.Add(item4);
		}
		Ᏹ = false;
	}

	private BaseItem ᐖ(string _0956)
	{
		foreach (WeakReference item2 in _ᏸ)
		{
			if (!item2.IsAlive)
			{
				continue;
			}
			IOwner owner = (IOwner)item2.Target;
			if (owner != null)
			{
				BaseItem item = owner.GetItem(_0956);
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
		if (ᐗ(toolbar))
		{
			throw new ArgumentException("toolbar is already registered");
		}
		WeakReference item = new WeakReference(toolbar);
		_ᏸ.Add(item);
	}

	public void UnregisterToolbar(IOwner toolbar)
	{
		if (toolbar == null)
		{
			throw new ArgumentNullException("toolbar parameter cannot be null");
		}
		foreach (WeakReference item in _ᏸ)
		{
			if (item.IsAlive && item.Target == toolbar)
			{
				_ᏸ.Remove(item);
				break;
			}
		}
	}

	private bool ᐗ(IOwner ᐘ)
	{
		foreach (WeakReference item in _ᏸ)
		{
			if (item.IsAlive && item.Target == ᐘ)
			{
				return true;
			}
		}
		return false;
	}
}
