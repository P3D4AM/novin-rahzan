using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using DevComponents.DotNetBar.Rendering;
using DevComponents.DotNetBar.Ribbon;

namespace DevComponents.DotNetBar.Controls;

[Designer("DevComponents.DotNetBar.Design.TabFormControlDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[ToolboxBitmap(typeof(TabFormControl), "TabFormControl.ico")]
[ComVisible(false)]
[ToolboxItem(true)]
public class TabFormControl : ContainerControl
{
	private PaintTabFormItemEventHandler Რ;

	private DotNetBarManager.LocalizeStringEventHandler _09D4;

	private EventHandler _09BC;

	private EventHandler Ꮷ;

	private MarkupLinkClickEventHandler _0D83;

	private TabFormItemDetachEventHandler Ლ;

	private TabFormItemDetachEventHandler Მ;

	private TabFormStripControl _Ꮺ;

	private bool _09FB;

	private ShadowPaintInfo Ꮻ;

	private bool _Ꮼ = true;

	private int Ꮿ;

	private Ყ Ᏸ;

	private RibbonLocalization Ᏺ = new RibbonLocalization();

	private bool Ᏼ = true;

	private ContextMenuBar แ;

	private EventHandler Ს;

	private TabFormColorTable __137C;

	private bool __13F6;

	private bool __13F7 = true;

	private eTabFormStripControlDock _ᏹ;

	[CompilerGenerated]
	private bool Ტ;

	internal bool Ꮬ => ((Component)this).DesignMode;

	[Category("Appearance")]
	[Browsable(true)]
	[Description("Gets or sets whether anti-aliasing is used while painting.")]
	[DefaultValue(true)]
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

	[Browsable(false)]
	public int TabsCount => _Ꮺ.TabsCount;

	[Category("Appearance")]
	[DefaultValue(true)]
	[Description("Indicates whether Form.Icon is shown in top-left corner.")]
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

	[Description("Indicates text displayed on Title bar instead of the Form.Text property.")]
	[EditorBrowsable(EditorBrowsableState.Always)]
	[DefaultValue("")]
	[Browsable(true)]
	[Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Category("Appearance")]
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

	[Category("Data")]
	[Description("Indicates Context menu bar associated with the TabFormControl control which is used as part of Global Items feature.")]
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
	[Browsable(true)]
	[Description("Indicates whether custom caption and quick access toolbar provided by the control is visible.")]
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

	[Browsable(true)]
	[DefaultValue(null)]
	[Description("Indicates font for the form caption text when CaptionVisible=true.")]
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

	[Browsable(true)]
	[Category("Caption")]
	[DefaultValue(0)]
	[Description("Indicates explicit height of the caption provided by control")]
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

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Gets or sets bar background style.")]
	[Category("Style")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	public ElementStyle BackgroundStyle => _Ꮺ.BackgroundStyle;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public TabFormItem SelectedTab
	{
		get
		{
			return _Ꮺ.SelectedTab;
		}
		set
		{
			if (value != null)
			{
				if (!value.Checked)
				{
					value.Checked = true;
				}
				else if (value.Panel != null && !((Control)value.Panel).Visible)
				{
					((Control)value.Panel).Visible = true;
				}
			}
		}
	}

	[DefaultValue(false)]
	[Description("Indicates whether new tab item which allows creation of new tab when clicked is visible. When visible you need to handle CreateNewTab event and create your new tab in event handler.")]
	[Category("Appearance")]
	public bool NewTabItemVisible
	{
		get
		{
			return _Ꮺ.NewTabItemVisible;
		}
		set
		{
			_Ꮺ.NewTabItemVisible = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public TabFormStripControl TabStrip => _Ꮺ;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public SubItemsCollection Items => _Ꮺ.Items;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(false)]
	public SubItemsCollection CaptionItems => _Ꮺ.Ბ.Ჸ;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public eDotNetBarStyle EffectiveStyle => _Ꮺ._൨;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public TabFormColorTable ColorTable
	{
		get
		{
			return __137C;
		}
		set
		{
			__137C = value;
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether mouse wheel scrolls through the tabs.")]
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

	[Category("Data")]
	[DefaultValue(null)]
	[Browsable(true)]
	[Description("ImageList for images used on Items. Images specified here will always be used on menu-items and are by default used on all Bars.")]
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

	[Category("Data")]
	[DefaultValue(null)]
	[Description("ImageList for medium-sized images used on Items.")]
	[Browsable(true)]
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

	[DefaultValue(null)]
	[Category("Data")]
	[Browsable(true)]
	[Description("ImageList for large-sized images used on Items.")]
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

	[Description("Indicates whether the user can give the focus to this control using the TAB key.")]
	[Category("Behavior")]
	[Browsable(true)]
	[DefaultValue(false)]
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

	protected override Size DefaultSize => new Size(200, 100);

	[Category("Appearance")]
	[DefaultValue(eTabFormStripControlDock.Top)]
	[Description("Indicates side tab-strip is docked to.")]
	public eTabFormStripControlDock TabStripDock
	{
		get
		{
			return _ᏹ;
		}
		set
		{
			_ᏹ = value;
			if (_ᏹ == eTabFormStripControlDock.Top)
			{
				((Control)_Ꮺ).Dock = (DockStyle)1;
			}
			else if (_ᏹ == eTabFormStripControlDock.Bottom)
			{
				((Control)_Ꮺ).Dock = (DockStyle)2;
			}
		}
	}

	[AmbientValue(null)]
	[Description("Indicates the font tab items are displayed with.")]
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

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool IsAutoCreated
	{
		[CompilerGenerated]
		get
		{
			return Ტ;
		}
		[CompilerGenerated]
		set
		{
			Ტ = value;
		}
	}

	[Description("Occurs ")]
	public event PaintTabFormItemEventHandler PaintTabFormItem
	{
		add
		{
			PaintTabFormItemEventHandler paintTabFormItemEventHandler = Რ;
			PaintTabFormItemEventHandler paintTabFormItemEventHandler2;
			do
			{
				paintTabFormItemEventHandler2 = paintTabFormItemEventHandler;
				PaintTabFormItemEventHandler value2 = (PaintTabFormItemEventHandler)Delegate.Combine(paintTabFormItemEventHandler2, value);
				paintTabFormItemEventHandler = Interlocked.CompareExchange(ref Რ, value2, paintTabFormItemEventHandler2);
			}
			while ((object)paintTabFormItemEventHandler != paintTabFormItemEventHandler2);
		}
		remove
		{
			PaintTabFormItemEventHandler paintTabFormItemEventHandler = Რ;
			PaintTabFormItemEventHandler paintTabFormItemEventHandler2;
			do
			{
				paintTabFormItemEventHandler2 = paintTabFormItemEventHandler;
				PaintTabFormItemEventHandler value2 = (PaintTabFormItemEventHandler)Delegate.Remove(paintTabFormItemEventHandler2, value);
				paintTabFormItemEventHandler = Interlocked.CompareExchange(ref Რ, value2, paintTabFormItemEventHandler2);
			}
			while ((object)paintTabFormItemEventHandler != paintTabFormItemEventHandler2);
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

	[Description("Occurs when Item on control is clicked.")]
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

	[Description("Occurs before TabFormItem is detached and gives you opportunity to cancel the action or provide your own new TabParentForm and TabFormControl.")]
	public event TabFormItemDetachEventHandler BeforeTabFormItemDetach
	{
		add
		{
			TabFormItemDetachEventHandler tabFormItemDetachEventHandler = Ლ;
			TabFormItemDetachEventHandler tabFormItemDetachEventHandler2;
			do
			{
				tabFormItemDetachEventHandler2 = tabFormItemDetachEventHandler;
				TabFormItemDetachEventHandler value2 = (TabFormItemDetachEventHandler)Delegate.Combine(tabFormItemDetachEventHandler2, value);
				tabFormItemDetachEventHandler = Interlocked.CompareExchange(ref Ლ, value2, tabFormItemDetachEventHandler2);
			}
			while ((object)tabFormItemDetachEventHandler != tabFormItemDetachEventHandler2);
		}
		remove
		{
			TabFormItemDetachEventHandler tabFormItemDetachEventHandler = Ლ;
			TabFormItemDetachEventHandler tabFormItemDetachEventHandler2;
			do
			{
				tabFormItemDetachEventHandler2 = tabFormItemDetachEventHandler;
				TabFormItemDetachEventHandler value2 = (TabFormItemDetachEventHandler)Delegate.Remove(tabFormItemDetachEventHandler2, value);
				tabFormItemDetachEventHandler = Interlocked.CompareExchange(ref Ლ, value2, tabFormItemDetachEventHandler2);
			}
			while ((object)tabFormItemDetachEventHandler != tabFormItemDetachEventHandler2);
		}
	}

	[Description("Occurs after TabFormItem has been detached and is added to the new form and tab control.")]
	public event TabFormItemDetachEventHandler TabFormItemDetach
	{
		add
		{
			TabFormItemDetachEventHandler tabFormItemDetachEventHandler = Მ;
			TabFormItemDetachEventHandler tabFormItemDetachEventHandler2;
			do
			{
				tabFormItemDetachEventHandler2 = tabFormItemDetachEventHandler;
				TabFormItemDetachEventHandler value2 = (TabFormItemDetachEventHandler)Delegate.Combine(tabFormItemDetachEventHandler2, value);
				tabFormItemDetachEventHandler = Interlocked.CompareExchange(ref Მ, value2, tabFormItemDetachEventHandler2);
			}
			while ((object)tabFormItemDetachEventHandler != tabFormItemDetachEventHandler2);
		}
		remove
		{
			TabFormItemDetachEventHandler tabFormItemDetachEventHandler = Მ;
			TabFormItemDetachEventHandler tabFormItemDetachEventHandler2;
			do
			{
				tabFormItemDetachEventHandler2 = tabFormItemDetachEventHandler;
				TabFormItemDetachEventHandler value2 = (TabFormItemDetachEventHandler)Delegate.Remove(tabFormItemDetachEventHandler2, value);
				tabFormItemDetachEventHandler = Interlocked.CompareExchange(ref Მ, value2, tabFormItemDetachEventHandler2);
			}
			while ((object)tabFormItemDetachEventHandler != tabFormItemDetachEventHandler2);
		}
	}

	[Description("Occurs when new tab item is clicked by end user and allows you to create and add new tab to the control.")]
	public event EventHandler CreateNewTab
	{
		add
		{
			EventHandler eventHandler = Ს;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ს, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = Ს;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ს, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	protected virtual void OnPaintTabFormItem(PaintTabFormItemEventArgs e)
	{
		Რ?.Invoke(this, e);
	}

	protected virtual void OnBeforeTabFormItemDetach(TabFormItemDetachEventArgs e)
	{
		Ლ?.Invoke(this, e);
	}

	protected virtual void OnTabFormItemDetach(TabFormItemDetachEventArgs e)
	{
		Მ?.Invoke(this, e);
	}

	public TabFormControl()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		_ = GlobalManager.Renderer;
		((Control)this).SetStyle((ControlStyles)(0x2010 | DisplayHelp.DoubleBufferFlag | 2 | 4), true);
		_Ꮺ = new TabFormStripControl();
		((Control)_Ꮺ).Dock = (DockStyle)1;
		((Control)_Ꮺ).Height = 40;
		_Ꮺ.ItemAdded += _13FF;
		_Ꮺ.LocalizeString += _13FE;
		_Ꮺ.ItemClick += ᏽ;
		_Ꮺ.ButtonCheckedChanged += ᏼ;
		_Ꮺ.TitleTextMarkupLinkClick += ᐁ;
		_Ꮺ.BeforeTabFormItemDetach += Ქ;
		_Ꮺ.TabFormItemDetach += Ფ;
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

	internal bool Უ(TabFormItem ᆰ, ItemPaintArgs ᎀ)
	{
		PaintTabFormItemEventArgs paintTabFormItemEventArgs = new PaintTabFormItemEventArgs(ᆰ, ᎀ.Graphics);
		OnPaintTabFormItem(paintTabFormItemEventArgs);
		return paintTabFormItemEventArgs.PaintDefault;
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
		if (_٠ is TabFormItem && ((TabFormItem)_٠).Checked)
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

	public void SetTabPanelStyle(TabFormPanel panel)
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
		panel.Style.Class = ElementStyleClassKeys.TabFormPanelKey;
	}

	public TabFormItem CreateTab(string text, string name, int insertPosition)
	{
		TabFormItem tabFormItem = new TabFormItem();
		tabFormItem.Text = text;
		tabFormItem.Name = name;
		TabFormPanel tabFormPanel = new TabFormPanel();
		((Control)tabFormPanel).Dock = (DockStyle)5;
		SetTabPanelStyle(tabFormPanel);
		((Control)this).Controls.Add((Control)(object)tabFormPanel);
		((Control)tabFormPanel).SendToBack();
		tabFormItem.Panel = tabFormPanel;
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
				Items.Add(tabFormItem);
			}
			else
			{
				Items.Insert(insertPosition, tabFormItem);
			}
		}
		else if (insertPosition > Items.Count - 1)
		{
			Items.Add(tabFormItem);
		}
		else
		{
			Items.Insert(insertPosition, tabFormItem);
		}
		return tabFormItem;
	}

	public TabFormItem CreateTab(string text, string name)
	{
		return CreateTab(text, name, -1);
	}

	public void RecalcLayout()
	{
		_Ꮺ.RecalcLayout();
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
			if (!(val is TabFormPanel))
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
		if (((Component)this).DesignMode || !(e.Control is TabFormPanel tabFormPanel))
		{
			return;
		}
		if (tabFormPanel.TabFormItem != null)
		{
			if (Items.Contains(tabFormPanel.TabFormItem) && SelectedTab == tabFormPanel.TabFormItem)
			{
				((Control)tabFormPanel).Visible = true;
				((Control)tabFormPanel).BringToFront();
			}
			else
			{
				((Control)tabFormPanel).Visible = false;
			}
		}
		else
		{
			((Control)tabFormPanel).Visible = false;
		}
	}

	private void _13FF(object _٠, EventArgs ؿ)
	{
		if (((Component)this).DesignMode || !(_٠ is TabFormItem))
		{
			return;
		}
		TabFormItem tabFormItem = _٠ as TabFormItem;
		if (tabFormItem.Panel != null)
		{
			if (((Control)this).Controls.Contains((Control)(object)tabFormItem.Panel) && tabFormItem.Checked)
			{
				((Control)tabFormItem.Panel).Visible = true;
				((Control)tabFormItem.Panel).BringToFront();
			}
			else
			{
				((Control)tabFormItem.Panel).Visible = false;
			}
		}
	}

	protected override void OnControlRemoved(ControlEventArgs e)
	{
		((Control)this).OnControlRemoved(e);
		if (e.Control is TabFormPanel { TabFormItem: not null } tabFormPanel && Items.Contains(tabFormPanel.TabFormItem))
		{
			Items.Remove(tabFormPanel.TabFormItem);
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
		if (((Message)(ref m)).Msg == 132 && !((Component)this).DesignMode)
		{
			int x = _0EFE._0F0B(((Message)(ref m)).LParam);
			int y = _0EFE._0F0D(((Message)(ref m)).LParam);
			Point pt = ((Control)this).PointToClient(new Point(x, y));
			if (CaptionVisible && _Ꮺ != null && !_Ꮺ._൩)
			{
				if (!(((Control)this).FindForm() is TabParentForm { Sizable: false }))
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
			}
			else if (_Ꮺ != null)
			{
				Point point = ((Control)_Ꮺ).PointToClient(new Point(x, y));
				if (_Ꮺ.HitTest(point.X, point.Y) == null)
				{
					((Message)(ref m)).Result = new IntPtr(-1);
					return;
				}
			}
		}
		((ContainerControl)this).WndProc(ref m);
	}

	private void Ფ(object _٠, TabFormItemDetachEventArgs ؿ)
	{
		OnTabFormItemDetach(ؿ);
	}

	private void Ქ(object _٠, TabFormItemDetachEventArgs ؿ)
	{
		OnBeforeTabFormItemDetach(ؿ);
	}

	private void ᐁ(object _٠, MarkupLinkClickEventArgs ؿ)
	{
		if (_0D83 != null)
		{
			_0D83(this, new MarkupLinkClickEventArgs(ؿ.Name, ؿ.HRef));
		}
	}

	protected internal virtual void OnCreateNewTab(EventArgs e)
	{
		Ს?.Invoke(this, e);
	}

	internal void Ღ(EventArgs ؿ)
	{
		OnCreateNewTab(ؿ);
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

	protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (Dpi.RecordScalePerControl)
		{
			Dpi.SetScaling(factor);
		}
		((ScrollableControl)this).ScaleControl(factor, specified);
	}
}
