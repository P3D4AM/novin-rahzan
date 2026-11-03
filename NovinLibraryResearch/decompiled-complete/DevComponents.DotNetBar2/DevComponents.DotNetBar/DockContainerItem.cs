using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace DevComponents.DotNetBar;

[Designer("DevComponents.DotNetBar.Design.DockContainerItemDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[ToolboxItem(false)]
[DesignTimeVisible(false)]
public class DockContainerItem : ImageItem
{
	private EventHandler _09CF;

	private ControlContainerItem.ControlContainerSerializationEventHandler _09D1;

	private ControlContainerItem.ControlContainerSerializationEventHandler _09D2;

	private Control _10CF;

	private bool ݡ;

	private bool ა;

	private Size Ⴓ = new Size(32, 32);

	private Size ბ = new Size(128, 128);

	private Image _0E66;

	private int դ = -1;

	private Image გ;

	private Icon ຈ;

	private int დ = 64;

	private int ე = -1;

	private int ვ = -1;

	private eTabItemColor ზ;

	private DockItemAccessibleObject _თ;

	private eDockContainerClose _ი;

	[Category("Docking")]
	[Description("Indicates the control hosted on dockable window")]
	[DefaultValue(null)]
	[DevCoBrowsable(false)]
	[Browsable(true)]
	public Control Control
	{
		get
		{
			return _10CF;
		}
		set
		{
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			if (_10CF != null && _10CF.Parent != null)
			{
				if (!DesignMode || Site == null)
				{
					_10CF.Visible = false;
				}
				_10CF.Parent.Controls.Remove(_10CF);
			}
			_10CF = value;
			if (_10CF == null)
			{
				return;
			}
			if (_10CF is PanelDockContainer)
			{
				((PanelDockContainer)(object)_10CF).DockContainerItem = this;
			}
			if (_10CF is ListBox)
			{
				((ListBox)_10CF).IntegralHeight = false;
			}
			if (_10CF.Parent != null)
			{
				_10CF.Parent.Controls.Remove(_10CF);
			}
			_10CF.Dock = (DockStyle)0;
			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(_10CF);
			if (properties.Find("AutoSize", ignoreCase: true) != null)
			{
				properties.Find("AutoSize", ignoreCase: true).SetValue(_10CF, false);
			}
			if (!DesignMode || Site == null)
			{
				_10CF.Visible = false;
			}
			Control val = null;
			if (ContainerControl != null)
			{
				object containerControl = ContainerControl;
				val = (Control)((containerControl is Control) ? containerControl : null);
				if (val != null)
				{
					val.Controls.Add(_10CF);
					if (DesignMode && Site != null && _10CF.Visible && !Selected)
					{
						_10CF.SendToBack();
					}
				}
			}
			OnDisplayedChanged();
		}
	}

	[DevCoBrowsable(false)]
	[Browsable(false)]
	[Description("Indicates item category used to group similar items at design-time.")]
	[Category("Design")]
	[DefaultValue("")]
	public override string Category
	{
		get
		{
			return base.Category;
		}
		set
		{
			base.Category = value;
		}
	}

	[Description("Gets or sets whether Click event will be auto repeated when mouse button is kept pressed over the item.")]
	[Category("Behavior")]
	[DefaultValue(false)]
	[Browsable(false)]
	[DevCoBrowsable(false)]
	public override bool ClickAutoRepeat
	{
		get
		{
			return base.ClickAutoRepeat;
		}
		set
		{
			base.ClickAutoRepeat = value;
		}
	}

	[DefaultValue(600)]
	[Category("Behavior")]
	[Browsable(false)]
	[DevCoBrowsable(false)]
	[Description("Gets or sets the auto-repeat interval for the click event when mouse button is kept pressed over the item.")]
	public override int ClickRepeatInterval
	{
		get
		{
			return base.ClickRepeatInterval;
		}
		set
		{
			base.ClickRepeatInterval = value;
		}
	}

	[DefaultValue("")]
	[DevCoBrowsable(false)]
	[Description("Indicates description of the item that is displayed during design.")]
	[Browsable(false)]
	[Category("Design")]
	public override string Description
	{
		get
		{
			return base.Description;
		}
		set
		{
			base.Description = value;
		}
	}

	[Description("Determines alignment of the item inside the container.")]
	[DevCoBrowsable(false)]
	[DefaultValue(eItemAlignment.Near)]
	[Browsable(false)]
	[Category("Appearance")]
	public override eItemAlignment ItemAlignment
	{
		get
		{
			return base.ItemAlignment;
		}
		set
		{
			base.ItemAlignment = value;
		}
	}

	[Description("Indicates list of shortcut keys for this item.")]
	[TypeConverter("DevComponents.DotNetBar.Design.ShortcutsConverter, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
	[DevCoBrowsable(false)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Editor("DevComponents.DotNetBar.Design.ShortcutsDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Category("Design")]
	public override ShortcutsCollection Shortcuts
	{
		get
		{
			return base.Shortcuts;
		}
		set
		{
			base.Shortcuts = value;
		}
	}

	[Category("Behavior")]
	[DevCoBrowsable(false)]
	[Browsable(false)]
	[Description("Determines whether sub-items are displayed.")]
	[DefaultValue(true)]
	public override bool ShowSubItems
	{
		get
		{
			return base.ShowSubItems;
		}
		set
		{
			base.ShowSubItems = value;
		}
	}

	public override bool Visible
	{
		get
		{
			if (base.IsOnMenu)
			{
				return false;
			}
			if (ContainerControl is Bar bar)
			{
				if (bar.BarState == eBarState.Docked || bar.BarState == eBarState.Floating || bar.BarState == eBarState.AutoHide)
				{
					return base.Visible;
				}
				return false;
			}
			return base.Visible;
		}
		set
		{
			base.Visible = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public ImageList ImageList
	{
		get
		{
			if (GetOwner() is IOwner owner)
			{
				return owner.Images;
			}
			return null;
		}
	}

	[DevCoBrowsable(true)]
	[Browsable(true)]
	[DefaultValue(null)]
	[Category("Appearance")]
	[Description("Specifies the Tab image. Image specified here is used only on Tab when there are multiple dock containers on Bar.")]
	public Image Image
	{
		get
		{
			return _0E66;
		}
		set
		{
			_0E66 = value;
			გ = null;
			if (ContainerControl is Bar bar)
			{
				bar._0C62(this);
			}
		}
	}

	[Category("Appearance")]
	[Description("Specifies the index of the Tab image if ImageList is used. Image specified here is used only on Tab when there are multiple dock containers on Bar.")]
	[Editor("DevComponents.DotNetBar.Design.ImageIndexEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[TypeConverter(typeof(ImageIndexConverter))]
	[DefaultValue(-1)]
	public int ImageIndex
	{
		get
		{
			return դ;
		}
		set
		{
			if (դ != value)
			{
				դ = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.ढ़(this, "ImageIndex");
				}
				გ = null;
				if (ContainerControl is Bar bar)
				{
					bar._0C62(this);
				}
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(null)]
	[Category("Appearance")]
	[DevCoBrowsable(true)]
	[Description("Specifies the Button icon. Icons support multiple image sizes and alpha blending.")]
	public Icon Icon
	{
		get
		{
			return ຈ;
		}
		set
		{
			NeedRecalcSize = true;
			ຈ = value;
			გ = null;
			if (ContainerControl is Bar bar)
			{
				bar._0C62(this);
			}
		}
	}

	[Category("Style")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Description("Applies predefined color to tab.")]
	[DefaultValue(eTabItemColor.Default)]
	public eTabItemColor PredefinedTabColor
	{
		get
		{
			return ზ;
		}
		set
		{
			if (ზ == value)
			{
				return;
			}
			ზ = value;
			if (ContainerControl is Bar { DockTabControl: not null } bar)
			{
				TabItem tabItem = პ(bar.DockTabControl);
				if (tabItem != null)
				{
					tabItem.PredefinedColor = ზ;
				}
			}
			if (DesignMode)
			{
				object containerControl = ContainerControl;
				Control val = (Control)((containerControl is Control) ? containerControl : null);
				if (val != null)
				{
					val.Refresh();
				}
			}
		}
	}

	internal Image _10CE
	{
		get
		{
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Expected O, but got Unknown
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Expected O, but got Unknown
			if (გ == null)
			{
				Image val = _0EC5();
				if (val != null)
				{
					if (val.Width == 16 && val.Height == 16)
					{
						გ = (Image)val.Clone();
					}
					else
					{
						გ = (Image)new Bitmap(val, 16, 16);
						Graphics val2 = Graphics.FromImage(გ);
						val2.DrawImage(val, new Rectangle(0, 0, 16, 16), 0, 0, 16, 16, (GraphicsUnit)2);
						val2.Dispose();
					}
				}
			}
			return გ;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool Selected
	{
		get
		{
			if (Visible && ContainerControl is Bar bar)
			{
				return bar.SelectedDockContainerItem == this;
			}
			return false;
		}
		set
		{
			if (Visible && ContainerControl is Bar bar)
			{
				bar.SelectedDockContainerItem = this;
			}
		}
	}

	[Category("Layout")]
	[DevCoBrowsable(false)]
	[DefaultValue(0)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[Description("Indicates the width of the item in pixels.")]
	public int Width
	{
		get
		{
			return DisplayRectangle.Width;
		}
		set
		{
			if (value < Ⴓ.Width)
			{
				value = Ⴓ.Width;
			}
			if (value >= Ⴓ.Width)
			{
				Bar bar = ContainerControl as Bar;
				if (Parent is GenericItemContainer { SystemContainer: not false } && bar != null && bar.LayoutType == eLayoutType.DockContainer && bar.Stretch)
				{
					bar._0BDF(this, value);
					ე = -1;
				}
				else
				{
					ე = value;
				}
			}
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Description("Indicates height of the item in pixels.")]
	[DefaultValue(0)]
	[Category("Layout")]
	public int Height
	{
		get
		{
			return DisplayRectangle.Height;
		}
		set
		{
			if (value < Ⴓ.Height)
			{
				value = Ⴓ.Height;
			}
			if (value >= Ⴓ.Height)
			{
				Bar bar = ContainerControl as Bar;
				if (Parent is GenericItemContainer { SystemContainer: not false } && bar != null && bar.LayoutType == eLayoutType.DockContainer && bar.Stretch)
				{
					bar._0BE0(this, value);
					ვ = -1;
				}
				else
				{
					ვ = value;
				}
			}
		}
	}

	[Category("Layout")]
	[Browsable(false)]
	[Description("Gets or sets the minimum size of the item.")]
	[DevCoBrowsable(false)]
	public Size MinimumSize
	{
		get
		{
			return Ⴓ;
		}
		set
		{
			Ⴓ = value;
			if (Ⴓ.Width > ბ.Width)
			{
				ბ.Width = Ⴓ.Width;
			}
			if (Ⴓ.Height > ბ.Height)
			{
				ბ.Height = Ⴓ.Height;
			}
			if (ContainerControl is Bar bar)
			{
				bar._0BE1(this);
			}
			if (Ⴓ.Width > Width)
			{
				Width = Ⴓ.Width;
			}
			if (Ⴓ.Height > Height)
			{
				Height = Ⴓ.Height;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Description("Gets or sets the default floating size of the Bar that is containing this item.")]
	[Category("Layout")]
	public Size DefaultFloatingSize
	{
		get
		{
			return ბ;
		}
		set
		{
			if (value.Width < Ⴓ.Width)
			{
				value.Width = Ⴓ.Width;
			}
			if (value.Height < Ⴓ.Height)
			{
				value.Height = Ⴓ.Height;
			}
			ბ = value;
		}
	}

	[Category("Layout")]
	[Obsolete("This property is obsolete and it will be removed in next version")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Description("Gets or sets the minimum size of the form client area that is tried to maintain when dockable window is resized.")]
	[DefaultValue(64)]
	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int MinFormClientSize
	{
		get
		{
			return დ;
		}
		set
		{
			დ = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[DevCoBrowsable(false)]
	public override bool Stretch
	{
		get
		{
			return base.Stretch;
		}
		set
		{
			base.Stretch = value;
		}
	}

	[Category("Behavior")]
	[DevCoBrowsable(false)]
	[Browsable(false)]
	[Description("Indicates whether item can be customized by user.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override bool CanCustomize
	{
		get
		{
			return base.CanCustomize;
		}
		set
		{
			base.CanCustomize = value;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override bool IsWindowed => true;

	[Browsable(false)]
	[DevCoBrowsable(false)]
	public override bool DesignMode
	{
		get
		{
			if (!base.DesignMode)
			{
				if (Site != null)
				{
					return Site.DesignMode;
				}
				return false;
			}
			return true;
		}
	}

	[DevCoBrowsable(true)]
	[DefaultValue(false)]
	[Description("Indicates whether certain global properties are propagated to all items with the same name when changed.")]
	[Browsable(true)]
	[Category("Behavior")]
	public override bool GlobalItem
	{
		get
		{
			return base.GlobalItem;
		}
		set
		{
			base.GlobalItem = value;
		}
	}

	[DefaultValue(eDockContainerClose.Inherit)]
	[Description("Specifies close button behavior on the host Bar. Default value is eDockContainerClose.Inherit which means that Bar.CanHide will control whether DockContainerItem can be closed.")]
	[Category("Behavior")]
	public eDockContainerClose CanClose
	{
		get
		{
			return _ი;
		}
		set
		{
			if (value != _ი)
			{
				eDockContainerClose ი = _ი;
				_ი = value;
				რ(ი, value);
			}
		}
	}

	[DevCoBrowsable(false)]
	[DefaultValue(true)]
	[Browsable(false)]
	[Description("Indicates whether is item enabled.")]
	[Category("Behavior")]
	public override bool Enabled
	{
		get
		{
			return base.Enabled;
		}
		set
		{
			base.Enabled = value;
		}
	}

	public event EventHandler ContainerLoadControl
	{
		add
		{
			EventHandler eventHandler = _09CF;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09CF, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _09CF;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09CF, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event ControlContainerItem.ControlContainerSerializationEventHandler ContainerControlSerialize
	{
		add
		{
			ControlContainerItem.ControlContainerSerializationEventHandler controlContainerSerializationEventHandler = _09D1;
			ControlContainerItem.ControlContainerSerializationEventHandler controlContainerSerializationEventHandler2;
			do
			{
				controlContainerSerializationEventHandler2 = controlContainerSerializationEventHandler;
				ControlContainerItem.ControlContainerSerializationEventHandler value2 = (ControlContainerItem.ControlContainerSerializationEventHandler)Delegate.Combine(controlContainerSerializationEventHandler2, value);
				controlContainerSerializationEventHandler = Interlocked.CompareExchange(ref _09D1, value2, controlContainerSerializationEventHandler2);
			}
			while ((object)controlContainerSerializationEventHandler != controlContainerSerializationEventHandler2);
		}
		remove
		{
			ControlContainerItem.ControlContainerSerializationEventHandler controlContainerSerializationEventHandler = _09D1;
			ControlContainerItem.ControlContainerSerializationEventHandler controlContainerSerializationEventHandler2;
			do
			{
				controlContainerSerializationEventHandler2 = controlContainerSerializationEventHandler;
				ControlContainerItem.ControlContainerSerializationEventHandler value2 = (ControlContainerItem.ControlContainerSerializationEventHandler)Delegate.Remove(controlContainerSerializationEventHandler2, value);
				controlContainerSerializationEventHandler = Interlocked.CompareExchange(ref _09D1, value2, controlContainerSerializationEventHandler2);
			}
			while ((object)controlContainerSerializationEventHandler != controlContainerSerializationEventHandler2);
		}
	}

	public event ControlContainerItem.ControlContainerSerializationEventHandler ContainerControlDeserialize
	{
		add
		{
			ControlContainerItem.ControlContainerSerializationEventHandler controlContainerSerializationEventHandler = _09D2;
			ControlContainerItem.ControlContainerSerializationEventHandler controlContainerSerializationEventHandler2;
			do
			{
				controlContainerSerializationEventHandler2 = controlContainerSerializationEventHandler;
				ControlContainerItem.ControlContainerSerializationEventHandler value2 = (ControlContainerItem.ControlContainerSerializationEventHandler)Delegate.Combine(controlContainerSerializationEventHandler2, value);
				controlContainerSerializationEventHandler = Interlocked.CompareExchange(ref _09D2, value2, controlContainerSerializationEventHandler2);
			}
			while ((object)controlContainerSerializationEventHandler != controlContainerSerializationEventHandler2);
		}
		remove
		{
			ControlContainerItem.ControlContainerSerializationEventHandler controlContainerSerializationEventHandler = _09D2;
			ControlContainerItem.ControlContainerSerializationEventHandler controlContainerSerializationEventHandler2;
			do
			{
				controlContainerSerializationEventHandler2 = controlContainerSerializationEventHandler;
				ControlContainerItem.ControlContainerSerializationEventHandler value2 = (ControlContainerItem.ControlContainerSerializationEventHandler)Delegate.Remove(controlContainerSerializationEventHandler2, value);
				controlContainerSerializationEventHandler = Interlocked.CompareExchange(ref _09D2, value2, controlContainerSerializationEventHandler2);
			}
			while ((object)controlContainerSerializationEventHandler != controlContainerSerializationEventHandler2);
		}
	}

	public DockContainerItem()
		: this("", "")
	{
	}

	public DockContainerItem(string sName)
		: this(sName, "")
	{
	}

	public DockContainerItem(string sName, string ItemText)
		: base(sName, ItemText)
	{
		m_SupportedOrientation = eSupportedOrientation.Both;
		Stretch = true;
		CanCustomize = false;
		m_Displayed = true;
		GlobalItem = false;
	}

	internal void კ()
	{
		EventArgs e = new EventArgs();
		if (_09CF != null)
		{
			_09CF(this, e);
		}
		GetIOwnerItemEvents()?.InvokeContainerLoadControl(this, e);
	}

	public override BaseItem Copy()
	{
		DockContainerItem dockContainerItem = new DockContainerItem(Name);
		CopyToItem(dockContainerItem);
		return dockContainerItem;
	}

	protected override void CopyToItem(BaseItem copy)
	{
		DockContainerItem dockContainerItem = copy as DockContainerItem;
		base.CopyToItem(dockContainerItem);
		dockContainerItem._0EBE(դ);
		if (ຈ != null)
		{
			object obj = ຈ.Clone();
			dockContainerItem.Icon = (Icon)((obj is Icon) ? obj : null);
		}
		if (dockContainerItem.Image != null)
		{
			object obj2 = _0E66.Clone();
			dockContainerItem.Image = (Image)((obj2 is Image) ? obj2 : null);
		}
		dockContainerItem._09CF = _09CF;
		dockContainerItem.კ();
	}

	protected override void Dispose(bool disposing)
	{
		if (BarUtilities.DisposeItemImages && !DesignMode)
		{
			BarUtilities.ᆟ(ref _0E66);
			BarUtilities.ᆟ(ref ຈ);
		}
		_10CF = null;
		base.Dispose(disposing);
	}

	protected internal override void Serialize(ItemSerializationContext context)
	{
		base.Serialize(context);
		XmlElement itemXmlElement = context.ItemXmlElement;
		XmlElement xmlElement = null;
		XmlElement xmlElement2 = null;
		if (_0E66 != null || դ >= 0)
		{
			xmlElement = itemXmlElement.OwnerDocument.CreateElement("images");
			itemXmlElement.AppendChild(xmlElement);
			if (դ >= 0)
			{
				xmlElement.SetAttribute("imageindex", XmlConvert.ToString(դ));
			}
			if (_0E66 != null)
			{
				xmlElement2 = itemXmlElement.OwnerDocument.CreateElement("image");
				xmlElement2.SetAttribute("type", "default");
				xmlElement.AppendChild(xmlElement2);
				BarFunctions.SerializeImage(_0E66, xmlElement2);
			}
		}
		else if (ຈ != null)
		{
			xmlElement = itemXmlElement.OwnerDocument.CreateElement("images");
			itemXmlElement.AppendChild(xmlElement);
			xmlElement2 = itemXmlElement.OwnerDocument.CreateElement("image");
			xmlElement2.SetAttribute("type", "icon");
			xmlElement.AppendChild(xmlElement2);
			BarFunctions.SerializeIcon(ຈ, xmlElement2);
		}
		if (Ⴓ.Width != 32 || Ⴓ.Height != 32)
		{
			itemXmlElement.SetAttribute("minw", Ⴓ.Width.ToString());
			itemXmlElement.SetAttribute("minh", Ⴓ.Height.ToString());
		}
		if (ბ.Width != 128 || ბ.Height != 128)
		{
			itemXmlElement.SetAttribute("defw", ბ.Width.ToString());
			itemXmlElement.SetAttribute("defh", ბ.Height.ToString());
		}
		if (დ != 64)
		{
			itemXmlElement.SetAttribute("csize", დ.ToString());
		}
		if (ზ != 0)
		{
			itemXmlElement.SetAttribute("PredefinedTabColor", XmlConvert.ToString((int)ზ));
		}
		if (_09D1 != null)
		{
			_09D1(this, new ControlContainerSerializationEventArgs(itemXmlElement));
		}
		GetIOwnerItemEvents()?.InvokeContainerControlSerialize(this, new ControlContainerSerializationEventArgs(itemXmlElement));
	}

	public override void Deserialize(ItemSerializationContext context)
	{
		base.Deserialize(context);
		XmlElement itemXmlElement = context.ItemXmlElement;
		foreach (XmlElement childNode in itemXmlElement.ChildNodes)
		{
			if (!(childNode.Name == "images"))
			{
				continue;
			}
			if (childNode.HasAttribute("imageindex"))
			{
				դ = XmlConvert.ToInt32(childNode.GetAttribute("imageindex"));
			}
			foreach (XmlElement childNode2 in childNode.ChildNodes)
			{
				if (childNode2.GetAttribute("type") == "default")
				{
					_0E66 = BarFunctions.DeserializeImage(childNode2);
					դ = -1;
				}
				else if (childNode2.GetAttribute("type") == "icon")
				{
					ຈ = BarFunctions.DeserializeIcon(childNode2);
					դ = -1;
				}
			}
			break;
		}
		if (itemXmlElement.HasAttribute("minw"))
		{
			Ⴓ = new Size(XmlConvert.ToInt32(itemXmlElement.GetAttribute("minw")), XmlConvert.ToInt32(itemXmlElement.GetAttribute("minh")));
		}
		if (itemXmlElement.HasAttribute("defw"))
		{
			ბ = new Size(XmlConvert.ToInt32(itemXmlElement.GetAttribute("defw")), XmlConvert.ToInt32(itemXmlElement.GetAttribute("defh")));
		}
		if (itemXmlElement.HasAttribute("csize"))
		{
			დ = XmlConvert.ToInt32(itemXmlElement.GetAttribute("csize"));
		}
		else
		{
			დ = 64;
		}
		if (itemXmlElement.HasAttribute("PredefinedTabColor"))
		{
			ზ = (eTabItemColor)XmlConvert.ToInt32(itemXmlElement.GetAttribute("PredefinedTabColor"));
		}
		else
		{
			ზ = eTabItemColor.Default;
		}
		კ();
		if (_10CF == null && context.ᵠ != null && context.ᵠ.ContainsKey(Name))
		{
			object? obj = context.ᵠ[Name];
			Control = (Control)((obj is Control) ? obj : null);
			context.ᵠ.Remove(Name);
		}
		if (_09D2 != null)
		{
			_09D2(this, new ControlContainerSerializationEventArgs(itemXmlElement));
		}
		GetIOwnerItemEvents()?.InvokeContainerControlDeserialize(this, new ControlContainerSerializationEventArgs(itemXmlElement));
	}

	protected override AccessibleObject CreateAccessibilityInstance()
	{
		if (_თ == null)
		{
			_თ = new DockItemAccessibleObject(this);
		}
		return (AccessibleObject)(object)_თ;
	}

	protected override void OnExternalSizeChange()
	{
		ლ();
		base.OnExternalSizeChange();
	}

	private void ლ()
	{
		if (ა)
		{
			return;
		}
		ა = true;
		try
		{
			Rectangle displayRectangle = DisplayRectangle;
			Size size = ო();
			if (base.IsOnMenu && EffectiveStyle != eDotNetBarStyle.Office2000)
			{
				size.Width += 7;
				displayRectangle.Width -= size.Width;
				displayRectangle.X += size.Width;
			}
			if (base.IsOnCustomizeMenu)
			{
				if (EffectiveStyle != eDotNetBarStyle.Office2000)
				{
					displayRectangle.X += size.Height + 8;
					displayRectangle.Width -= size.Height + 8;
				}
				else
				{
					displayRectangle.X += size.Height + 4;
					displayRectangle.Width -= size.Height + 4;
				}
			}
			if (_10CF == null)
			{
				return;
			}
			displayRectangle.Inflate(-2, -2);
			if (displayRectangle.Width > 0 && displayRectangle.Height > 0)
			{
				if (!_10CF.Size.Equals((object?)displayRectangle.Size))
				{
					_10CF.SuspendLayout();
					_10CF.Size = displayRectangle.Size;
					_10CF.ResumeLayout();
				}
				Point location = displayRectangle.Location;
				location.Offset((displayRectangle.Width - _10CF.Width) / 2, (displayRectangle.Height - _10CF.Height) / 2);
				if (location.X != _10CF.Location.X || location.Y != _10CF.Location.Y)
				{
					_10CF.Location = location;
				}
			}
		}
		finally
		{
			ა = false;
		}
	}

	public override void Paint(ItemPaintArgs pa)
	{
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Expected O, but got Unknown
		if (SuspendLayout)
		{
			return;
		}
		Graphics graphics = pa.Graphics;
		if (_10CF != null && _10CF.Visible != Displayed && !_10CF.Visible)
		{
			ნ();
		}
		Rectangle displayRectangle = DisplayRectangle;
		Size size = ო();
		if (base.IsOnMenu && EffectiveStyle != eDotNetBarStyle.Office2000)
		{
			size.Width += 7;
			displayRectangle.Width -= size.Width;
			displayRectangle.X += size.Width;
			if (base.IsOnCustomizeMenu)
			{
				size.Width += size.Height + 8;
			}
			if (!pa.Colors.MenuSide2.IsEmpty)
			{
				LinearGradientBrush val = BarFunctions.ॴ(new Rectangle(m_Rect.Left, m_Rect.Top, size.Width, m_Rect.Height), pa.Colors.MenuSide, pa.Colors.MenuSide2, pa.Colors.MenuSideGradientAngle);
				graphics.FillRectangle((Brush)(object)val, m_Rect.Left, m_Rect.Top, size.Width, m_Rect.Height);
				((Brush)val).Dispose();
			}
			else
			{
				graphics.FillRectangle((Brush)new SolidBrush(pa.Colors.MenuSide), m_Rect.Left, m_Rect.Top, size.Width, m_Rect.Height);
			}
		}
		if (base.IsOnCustomizeMenu)
		{
			if (EffectiveStyle != eDotNetBarStyle.Office2000)
			{
				displayRectangle.X += size.Height + 8;
				displayRectangle.Width -= size.Height + 8;
			}
			else
			{
				displayRectangle.X += size.Height + 4;
				displayRectangle.Width -= size.Height + 4;
			}
		}
		if (_10CF == null)
		{
			string text = m_Text;
			if (text == "")
			{
				text = "Container";
			}
			eTextFormat format = ಲ();
			Font font = GetFont();
			Rectangle bounds = new Rectangle(displayRectangle.X + 8, displayRectangle.Y, displayRectangle.Width, displayRectangle.Height);
			if (EffectiveStyle == eDotNetBarStyle.Office2000)
			{
				TextDrawing.DrawString(graphics, text, font, SystemColors.ControlText, bounds, format);
			}
			else
			{
				TextDrawing.DrawString(graphics, text, font, SystemColors.ControlText, bounds, format);
			}
			Size size2 = TextDrawing.MeasureString(graphics, text, font, 0, format);
			displayRectangle.X += size2.Width + 8;
			displayRectangle.Width -= size2.Width + 8;
		}
		else if (_10CF != null)
		{
			ლ();
		}
		if (base.IsOnCustomizeMenu && Visible)
		{
			Rectangle r = new Rectangle(m_Rect.Left, m_Rect.Top, m_Rect.Height, m_Rect.Height);
			if (EffectiveStyle != eDotNetBarStyle.Office2000)
			{
				r.Inflate(-1, -1);
			}
			BarFunctions.DrawMenuCheckBox(pa, r, EffectiveStyle, ݡ);
		}
		if (Focused && DesignMode)
		{
			displayRectangle = DisplayRectangle;
			displayRectangle.Inflate(-1, -1);
			_1A6B._1A6C(graphics, displayRectangle, pa.Colors.ItemDesignTimeBorder);
		}
	}

	public override void RecalcSize()
	{
		if (SuspendLayout)
		{
			return;
		}
		_ = base.IsOnMenu;
		if (_10CF == null && !DesignMode)
		{
			კ();
		}
		if (_10CF == null)
		{
			if (Parent != null && Parent is ImageItem)
			{
				m_Rect.Height = ((ImageItem)Parent).SubItemsImageSize.Height + 4;
			}
			else
			{
				m_Rect.Height = SubItemsImageSize.Height + 4;
			}
			if (EffectiveStyle != eDotNetBarStyle.Office2000)
			{
				if (_10CF != null && m_Rect.Height < _10CF.Height + 2)
				{
					m_Rect.Height = _10CF.Height + 2;
				}
			}
			else if (_10CF != null && m_Rect.Height < _10CF.Height + 2)
			{
				m_Rect.Height = _10CF.Height + 2;
			}
		}
		else
		{
			m_Rect.Height = Ⴓ.Height + 4;
		}
		m_Rect.Width = Ⴓ.Width + 4;
		if (_10CF == null)
		{
			string text = m_Text;
			if (text == "")
			{
				text = "Container";
			}
			object containerControl = ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (val != null && BaseItem.IsHandleValid(val))
			{
				Graphics val2 = BarFunctions.CreateGraphics(val);
				try
				{
					Size size = TextDrawing.MeasureString(val2, text, GetFont(), 0, ಲ());
					if (size.Height > SubItemsImageSize.Height && size.Height > m_Rect.Height)
					{
						m_Rect.Height = size.Height + 4;
					}
				}
				finally
				{
					val2.Dispose();
				}
			}
		}
		Size size2 = ო();
		if (base.IsOnMenu && EffectiveStyle != eDotNetBarStyle.Office2000)
		{
			m_Rect.Width += size2.Width + 7;
		}
		if (base.IsOnCustomizeMenu)
		{
			m_Rect.Width += size2.Height + 2;
		}
		base.RecalcSize();
	}

	protected internal override void OnContainerChanged(object objOldContainer)
	{
		base.OnContainerChanged(objOldContainer);
		მ();
	}

	private void მ()
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		if (_10CF == null)
		{
			return;
		}
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val == _10CF.Parent)
		{
			return;
		}
		bool flag = false;
		if (_10CF.Parent != null)
		{
			if (_10CF is TextBox && ((TextBoxBase)(TextBox)_10CF).Multiline)
			{
				flag = true;
				((TextBoxBase)(TextBox)_10CF).Multiline = false;
			}
			_10CF.Parent.Controls.Remove(_10CF);
		}
		if (val != null)
		{
			val.Controls.Add(_10CF);
			if (_10CF is TextBox && flag)
			{
				((TextBoxBase)(TextBox)_10CF).Multiline = true;
			}
			if (_10CF.Visible)
			{
				_10CF.Refresh();
			}
		}
	}

	protected internal override void OnProcessDelayedCommands()
	{
		if (ე >= 0)
		{
			int width = ე;
			ე = -1;
			Width = width;
		}
		if (ვ > 0)
		{
			int height = ვ;
			ვ = -1;
			Height = height;
		}
	}

	protected internal override void OnVisibleChanged(bool newValue)
	{
		if (_10CF != null && !newValue)
		{
			if (!DesignMode || Site == null)
			{
				_10CF.Visible = newValue;
			}
			else if (newValue)
			{
				_10CF.SendToBack();
			}
			else
			{
				_10CF.BringToFront();
			}
		}
		if (ContainerControl is Bar { LayoutType: eLayoutType.DockContainer } bar)
		{
			bar._0C5B(this);
			if (bar.AutoHide)
			{
				bar.ౚ();
				bar.ౘ(ౙ: true);
			}
			else
			{
				bar.ౘ(ౙ: true);
			}
		}
		base.OnVisibleChanged(newValue);
	}

	protected override void OnDisplayedChanged()
	{
		if (Displayed)
		{
			Refresh();
			if (ContainerControl is Bar bar)
			{
				bar._౧();
			}
		}
		if (_10CF != null && !base.IsOnCustomizeMenu && !base.IsOnCustomizeDialog)
		{
			ლ();
			if (!DesignMode || Site == null)
			{
				_10CF.Visible = Displayed;
			}
			else
			{
				if (Displayed)
				{
					_10CF.BringToFront();
				}
				else
				{
					_10CF.SendToBack();
				}
				ლ();
			}
		}
		base.OnDisplayedChanged();
	}

	protected override void OnIsOnCustomizeDialogChanged()
	{
		base.OnIsOnCustomizeDialogChanged();
		ნ();
	}

	protected override void OnDesignModeChanged()
	{
		base.OnDesignModeChanged();
		ნ();
	}

	protected override void OnIsOnCustomizeMenuChanged()
	{
		base.OnIsOnCustomizeMenuChanged();
		ნ();
	}

	private void ნ()
	{
		if (_10CF == null || (DesignMode && Site != null))
		{
			return;
		}
		if (base.IsOnCustomizeMenu || base.IsOnCustomizeDialog || DesignMode)
		{
			_10CF.Enabled = false;
			return;
		}
		if (!DesignMode || Site == null)
		{
			_10CF.Visible = Displayed;
		}
		_10CF.Enabled = true;
	}

	private Size ო()
	{
		if (m_Parent != null)
		{
			if (m_Parent is ImageItem imageItem)
			{
				return imageItem.SubItemsImageSize;
			}
			return ImageSize;
		}
		return ImageSize;
	}

	private eTextFormat ಲ()
	{
		eTextFormat eTextFormat2 = eTextFormat.Default;
		eTextFormat2 |= eTextFormat.SingleLine;
		eTextFormat2 |= eTextFormat.EndEllipsis;
		return eTextFormat2 | eTextFormat.VerticalCenter;
	}

	protected virtual Font GetFont()
	{
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val != null)
		{
			return val.Font;
		}
		return SystemFonts.DefaultFont;
	}

	protected internal override bool IsAnyOnHandle(IntPtr iHandle)
	{
		bool flag = base.IsAnyOnHandle(iHandle);
		if (!flag && _10CF != null && _10CF.Handle == iHandle)
		{
			flag = true;
		}
		return flag;
	}

	protected override void OnEnabledChanged()
	{
		base.OnEnabledChanged();
		if ((!DesignMode || Site == null) && _10CF != null)
		{
			_10CF.Enabled = Enabled;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void OnGotFocus()
	{
		base.OnGotFocus();
		if (_10CF != null && !_10CF.Focused && !base.IsOnCustomizeMenu && !base.IsOnCustomizeDialog && !DesignMode)
		{
			_10CF.Focus();
		}
	}

	private TabItem პ(TabStrip ჟ)
	{
		foreach (TabItem tab in ჟ.Tabs)
		{
			if (tab.AttachedItem == this)
			{
				return tab;
			}
		}
		return null;
	}

	private Image _0EC5()
	{
		if (_0E66 != null)
		{
			return _0E66;
		}
		return _0EC9(դ);
	}

	internal void _0EBE(int _0EBF)
	{
		դ = _0EBF;
	}

	private Image _0EC9(int _0ECA)
	{
		if (_0ECA >= 0 && GetOwner() is IOwner { Images: not null } owner && owner.Images.Images.Count > 0 && _0ECA < owner.Images.Images.Count)
		{
			return owner.Images.Images[_0ECA];
		}
		return null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeMinimumSize()
	{
		if (Ⴓ.Width != 32 || Ⴓ.Height != 32)
		{
			return true;
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetMinimumSize()
	{
		MinimumSize = new Size(32, 32);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeDefaultFloatingSize()
	{
		if (ბ.Width != 128 || ბ.Height != 128)
		{
			return true;
		}
		return false;
	}

	protected internal override void OnAfterItemRemoved(BaseItem item, int itemIndex)
	{
		base.OnAfterItemRemoved(item, itemIndex);
		მ();
	}

	protected override void OnTextChanged()
	{
		base.OnTextChanged();
		if (ContainerControl is Bar)
		{
			((Bar)ContainerControl)._0C62(this);
		}
	}

	protected override void OnTooltipChanged()
	{
		base.OnTooltipChanged();
		if (ContainerControl is Bar)
		{
			((Bar)ContainerControl)._0C62(this);
		}
	}

	protected override void OnStyleChanged()
	{
		base.OnStyleChanged();
		if (_10CF is PanelDockContainer && !((PanelDockContainer)(object)_10CF).UseCustomStyle)
		{
			((PanelDockContainer)(object)_10CF).ColorSchemeStyle = Style;
		}
	}

	private void რ(eDockContainerClose ك, eDockContainerClose ل)
	{
		if (ContainerControl is Bar bar)
		{
			bar._0C3B(this, ك, ل);
		}
	}
}
