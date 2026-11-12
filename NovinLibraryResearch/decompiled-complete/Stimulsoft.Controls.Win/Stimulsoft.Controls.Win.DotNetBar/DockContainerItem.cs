using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
public class DockContainerItem : ImageItem
{
	private Control m_Control;

	private bool m_MouseOver;

	private bool m_InternalResize;

	private Size m_MinimumSize = new Size(32, 32);

	private Size m_DefaultFloatingSize = new Size(128, 128);

	private Image m_Image;

	private int m_ImageIndex = -1;

	private Image m_TabImage;

	private Icon m_Icon;

	private int m_MinFormClientSize = 64;

	private int m_DelayedWidth = -1;

	private int m_DelayedHeight = -1;

	private eTabItemColor m_PredefinedTabColor;

	private eDockContainerClose _CanClose;

	[Browsable(true)]
	[DevCoBrowsable(false)]
	[DefaultValue(null)]
	[Description("Indicates the control hosted on dockable window")]
	[Category("Docking")]
	public Control Control
	{
		get
		{
			return m_Control;
		}
		set
		{
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			if (m_Control != null && m_Control.Parent != null)
			{
				if (!DesignMode || Site == null)
				{
					m_Control.Visible = false;
				}
				m_Control.Parent.Controls.Remove(m_Control);
			}
			m_Control = value;
			if (m_Control == null)
			{
				return;
			}
			if (m_Control is PanelDockContainer)
			{
				((PanelDockContainer)(object)m_Control).DockContainerItem = this;
			}
			if (m_Control is ListBox)
			{
				((ListBox)m_Control).IntegralHeight = false;
			}
			if (m_Control.Parent != null)
			{
				m_Control.Parent.Controls.Remove(m_Control);
			}
			m_Control.Dock = (DockStyle)0;
			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(m_Control);
			if (properties.Find("AutoSize", ignoreCase: true) != null)
			{
				properties.Find("AutoSize", ignoreCase: true).SetValue(m_Control, false);
			}
			if (!DesignMode || Site == null)
			{
				m_Control.Visible = false;
			}
			Control val = null;
			if (ContainerControl != null)
			{
				object containerControl = ContainerControl;
				val = (Control)((containerControl is Control) ? containerControl : null);
				if (val != null)
				{
					val.Controls.Add(m_Control);
					if (DesignMode && Site != null && m_Control.Visible && !Selected)
					{
						m_Control.SendToBack();
					}
				}
			}
			OnDisplayedChanged();
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DefaultValue("")]
	[Category("Design")]
	[Description("Indicates item category used to group similar items at design-time.")]
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

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Gets or sets whether Click event will be auto repeated when mouse button is kept pressed over the item.")]
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

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DefaultValue(600)]
	[Category("Behavior")]
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

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DefaultValue("")]
	[Category("Design")]
	[Description("Indicates description of the item that is displayed during design.")]
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

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DefaultValue(eItemAlignment.Near)]
	[Category("Appearance")]
	[Description("Determines alignment of the item inside the container.")]
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

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[Category("Design")]
	[Description("Indicates list of shortcut keys for this item.")]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.ShortcutsDesigner, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter("Stimulsoft.Controls.Win.DotNetBar.Design.ShortcutsConverter, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Determines whether sub-items are displayed.")]
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

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
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

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(null)]
	[Category("Appearance")]
	[Description("Specifies the Tab image. Image specified here is used only on Tab when there are multiple dock containers on Bar.")]
	public Image Image
	{
		get
		{
			return m_Image;
		}
		set
		{
			m_Image = value;
			m_TabImage = null;
			if (ContainerControl is Bar bar)
			{
				bar.RefreshDockContainerItem(this);
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(-1)]
	[Category("Appearance")]
	[Description("Specifies the index of the Tab image if ImageList is used. Image specified here is used only on Tab when there are multiple dock containers on Bar.")]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.ImageIndexEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter(typeof(ImageIndexConverter))]
	public int ImageIndex
	{
		get
		{
			return m_ImageIndex;
		}
		set
		{
			if (m_ImageIndex != value)
			{
				m_ImageIndex = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.SyncProperty(this, "ImageIndex");
				}
				m_TabImage = null;
				if (ContainerControl is Bar bar)
				{
					bar.RefreshDockContainerItem(this);
				}
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Specifies the Button icon. Icons support multiple image sizes and alpha blending.")]
	[DefaultValue(null)]
	public Icon Icon
	{
		get
		{
			return m_Icon;
		}
		set
		{
			NeedRecalcSize = true;
			m_Icon = value;
			m_TabImage = null;
			if (ContainerControl is Bar bar)
			{
				bar.RefreshDockContainerItem(this);
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(eTabItemColor.Default)]
	[Category("Style")]
	[Description("Applies predefined color to tab.")]
	public eTabItemColor PredefinedTabColor
	{
		get
		{
			return m_PredefinedTabColor;
		}
		set
		{
			if (m_PredefinedTabColor == value)
			{
				return;
			}
			m_PredefinedTabColor = value;
			if (ContainerControl is Bar { DockTabControl: not null } bar && Parent != null && Parent.SubItems.IndexOf(this) < bar.DockTabControl.Tabs.Count && bar.DockTabControl.Tabs[Parent.SubItems.IndexOf(this)].AttachedItem == this)
			{
				bar.DockTabControl.Tabs[Parent.SubItems.IndexOf(this)].PredefinedColor = m_PredefinedTabColor;
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

	internal Image TabImage
	{
		get
		{
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Expected O, but got Unknown
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Expected O, but got Unknown
			if (m_TabImage == null)
			{
				Image image = GetImage();
				if (image != null)
				{
					double factor = StiScale2.Factor;
					int num = (int)Math.Ceiling(16.0 * factor);
					if (image.Width == num && image.Height == num)
					{
						m_TabImage = (Image)image.Clone();
					}
					else
					{
						m_TabImage = (Image)new Bitmap(image, num, num);
						Graphics val = Graphics.FromImage(m_TabImage);
						val.DrawImage(image, new Rectangle(0, 0, num, num), 0, 0, num, num, (GraphicsUnit)2);
						val.Dispose();
					}
				}
			}
			return m_TabImage;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DefaultValue(0)]
	[Category("Layout")]
	[Description("Indicates the width of the item in pixels.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int Width
	{
		get
		{
			return DisplayRectangle.Width;
		}
		set
		{
			if (value < m_MinimumSize.Width)
			{
				value = m_MinimumSize.Width;
			}
			if (value >= m_MinimumSize.Width)
			{
				Bar bar = ContainerControl as Bar;
				if (Parent is GenericItemContainer { SystemContainer: not false } && bar != null && bar.LayoutType == eLayoutType.DockContainer && bar.Stretch)
				{
					bar.OnDockContainerWidthChanged(this, value);
					m_DelayedWidth = -1;
				}
				else
				{
					m_DelayedWidth = value;
				}
			}
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(true)]
	[DefaultValue(0)]
	[Category("Layout")]
	[Description("Indicates height of the item in pixels.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int Height
	{
		get
		{
			return DisplayRectangle.Height;
		}
		set
		{
			if (value < m_MinimumSize.Height)
			{
				value = m_MinimumSize.Height;
			}
			if (value >= m_MinimumSize.Height)
			{
				Bar bar = ContainerControl as Bar;
				if (Parent is GenericItemContainer { SystemContainer: not false } && bar != null && bar.LayoutType == eLayoutType.DockContainer && bar.Stretch)
				{
					bar.OnDockContainerHeightChanged(this, value);
					m_DelayedHeight = -1;
				}
				else
				{
					m_DelayedHeight = value;
				}
			}
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[Category("Layout")]
	[Description("Gets or sets the minimum size of the item.")]
	public Size MinimumSize
	{
		get
		{
			return m_MinimumSize;
		}
		set
		{
			m_MinimumSize = value;
			if (m_MinimumSize.Width > m_DefaultFloatingSize.Width)
			{
				m_DefaultFloatingSize.Width = m_MinimumSize.Width;
			}
			if (m_MinimumSize.Height > m_DefaultFloatingSize.Height)
			{
				m_DefaultFloatingSize.Height = m_MinimumSize.Height;
			}
			if (ContainerControl is Bar bar)
			{
				bar.OnDockContainerMinimumSizeChanged(this);
			}
			if (m_MinimumSize.Width > Width)
			{
				Width = m_MinimumSize.Width;
			}
			if (m_MinimumSize.Height > Height)
			{
				Height = m_MinimumSize.Height;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Layout")]
	[Description("Gets or sets the default floating size of the Bar that is containing this item.")]
	public Size DefaultFloatingSize
	{
		get
		{
			return m_DefaultFloatingSize;
		}
		set
		{
			if (value.Width < m_MinimumSize.Width)
			{
				value.Width = m_MinimumSize.Width;
			}
			if (value.Height < m_MinimumSize.Height)
			{
				value.Height = m_MinimumSize.Height;
			}
			m_DefaultFloatingSize = value;
		}
	}

	[Obsolete("This property is obsolete and it will be removed in next version")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DefaultValue(64)]
	[Category("Layout")]
	[Description("Gets or sets the minimum size of the form client area that is tried to maintain when dockable window is resized.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int MinFormClientSize
	{
		get
		{
			return m_MinFormClientSize;
		}
		set
		{
			m_MinFormClientSize = value;
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Category("Behavior")]
	[Description("Indicates whether item can be customized by user.")]
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

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates whether certain global properties are propagated to all items with the same name when changed.")]
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

	[Category("Behavior")]
	[DefaultValue(eDockContainerClose.Inherit)]
	[Description("Specifies close button behavior on the host Bar. Default value is eDockContainerClose.Inherit which means that Bar.CanHide will control whether DockContainerItem can be closed.")]
	public eDockContainerClose CanClose
	{
		get
		{
			return _CanClose;
		}
		set
		{
			if (value != _CanClose)
			{
				eDockContainerClose canClose = _CanClose;
				_CanClose = value;
				OnCanCloseChanged(canClose, value);
			}
		}
	}

	public event EventHandler ContainerLoadControl;

	public event ControlContainerItem.ControlContainerSerializationEventHandler ContainerControlSerialize;

	public event ControlContainerItem.ControlContainerSerializationEventHandler ContainerControlDeserialize;

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

	internal void InitControl()
	{
		EventArgs e = new EventArgs();
		if (this.ContainerLoadControl != null)
		{
			this.ContainerLoadControl(this, e);
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
		dockContainerItem.SetImageIndex(m_ImageIndex);
		if (m_Icon != null)
		{
			object obj = m_Icon.Clone();
			dockContainerItem.Icon = (Icon)((obj is Icon) ? obj : null);
		}
		if (dockContainerItem.Image != null)
		{
			object obj2 = m_Image.Clone();
			dockContainerItem.Image = (Image)((obj2 is Image) ? obj2 : null);
		}
		dockContainerItem.ContainerLoadControl = this.ContainerLoadControl;
		dockContainerItem.InitControl();
	}

	protected override void Dispose(bool disposing)
	{
		if (BarUtilities.DisposeItemImages && !DesignMode)
		{
			BarUtilities.DisposeImage(ref m_Image);
			BarUtilities.DisposeImage(ref m_Icon);
		}
		m_Control = null;
		base.Dispose(disposing);
	}

	protected internal override void Serialize(ItemSerializationContext context)
	{
		base.Serialize(context);
		XmlElement itemXmlElement = context.ItemXmlElement;
		XmlElement xmlElement = null;
		XmlElement xmlElement2 = null;
		if (m_Image != null || m_ImageIndex >= 0)
		{
			xmlElement = itemXmlElement.OwnerDocument.CreateElement("images");
			itemXmlElement.AppendChild(xmlElement);
			if (m_ImageIndex >= 0)
			{
				xmlElement.SetAttribute("imageindex", XmlConvert.ToString(m_ImageIndex));
			}
			if (m_Image != null)
			{
				xmlElement2 = itemXmlElement.OwnerDocument.CreateElement("image");
				xmlElement2.SetAttribute("type", "default");
				xmlElement.AppendChild(xmlElement2);
				BarFunctions.SerializeImage(m_Image, xmlElement2);
			}
		}
		else if (m_Icon != null)
		{
			xmlElement = itemXmlElement.OwnerDocument.CreateElement("images");
			itemXmlElement.AppendChild(xmlElement);
			xmlElement2 = itemXmlElement.OwnerDocument.CreateElement("image");
			xmlElement2.SetAttribute("type", "icon");
			xmlElement.AppendChild(xmlElement2);
			BarFunctions.SerializeIcon(m_Icon, xmlElement2);
		}
		if (m_MinimumSize.Width != 32 || m_MinimumSize.Height != 32)
		{
			itemXmlElement.SetAttribute("minw", m_MinimumSize.Width.ToString());
			itemXmlElement.SetAttribute("minh", m_MinimumSize.Height.ToString());
		}
		if (m_DefaultFloatingSize.Width != 128 || m_DefaultFloatingSize.Height != 128)
		{
			itemXmlElement.SetAttribute("defw", m_DefaultFloatingSize.Width.ToString());
			itemXmlElement.SetAttribute("defh", m_DefaultFloatingSize.Height.ToString());
		}
		if (m_MinFormClientSize != 64)
		{
			itemXmlElement.SetAttribute("csize", m_MinFormClientSize.ToString());
		}
		if (m_PredefinedTabColor != 0)
		{
			itemXmlElement.SetAttribute("PredefinedTabColor", XmlConvert.ToString((int)m_PredefinedTabColor));
		}
		if (this.ContainerControlSerialize != null)
		{
			this.ContainerControlSerialize(this, new ControlContainerSerializationEventArgs(itemXmlElement));
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
				m_ImageIndex = XmlConvert.ToInt32(childNode.GetAttribute("imageindex"));
			}
			foreach (XmlElement childNode2 in childNode.ChildNodes)
			{
				if (childNode2.GetAttribute("type") == "default")
				{
					m_Image = BarFunctions.DeserializeImage(childNode2);
					m_ImageIndex = -1;
				}
				else if (childNode2.GetAttribute("type") == "icon")
				{
					m_Icon = BarFunctions.DeserializeIcon(childNode2);
					m_ImageIndex = -1;
				}
			}
			break;
		}
		if (itemXmlElement.HasAttribute("minw"))
		{
			m_MinimumSize = new Size(XmlConvert.ToInt32(itemXmlElement.GetAttribute("minw")), XmlConvert.ToInt32(itemXmlElement.GetAttribute("minh")));
		}
		if (itemXmlElement.HasAttribute("defw"))
		{
			m_DefaultFloatingSize = new Size(XmlConvert.ToInt32(itemXmlElement.GetAttribute("defw")), XmlConvert.ToInt32(itemXmlElement.GetAttribute("defh")));
		}
		if (itemXmlElement.HasAttribute("csize"))
		{
			m_MinFormClientSize = XmlConvert.ToInt32(itemXmlElement.GetAttribute("csize"));
		}
		else
		{
			m_MinFormClientSize = 64;
		}
		if (itemXmlElement.HasAttribute("PredefinedTabColor"))
		{
			m_PredefinedTabColor = (eTabItemColor)XmlConvert.ToInt32(itemXmlElement.GetAttribute("PredefinedTabColor"));
		}
		else
		{
			m_PredefinedTabColor = eTabItemColor.Default;
		}
		InitControl();
		if (m_Control == null && context.DockControls != null && context.DockControls.ContainsKey(Name))
		{
			object? obj = context.DockControls[Name];
			Control = (Control)((obj is Control) ? obj : null);
			context.DockControls.Remove(Name);
		}
		if (this.ContainerControlDeserialize != null)
		{
			this.ContainerControlDeserialize(this, new ControlContainerSerializationEventArgs(itemXmlElement));
		}
		GetIOwnerItemEvents()?.InvokeContainerControlDeserialize(this, new ControlContainerSerializationEventArgs(itemXmlElement));
	}

	protected override void OnExternalSizeChange()
	{
		ResizeControl();
		base.OnExternalSizeChange();
	}

	private void ResizeControl()
	{
		if (m_InternalResize)
		{
			return;
		}
		m_InternalResize = true;
		try
		{
			Rectangle displayRectangle = DisplayRectangle;
			Size maxImageSize = GetMaxImageSize();
			if (base.IsOnMenu && EffectiveStyle != eDotNetBarStyle.Office2000)
			{
				maxImageSize.Width += 7;
				displayRectangle.Width -= maxImageSize.Width;
				displayRectangle.X += maxImageSize.Width;
			}
			if (base.IsOnCustomizeMenu)
			{
				if (EffectiveStyle != eDotNetBarStyle.Office2000)
				{
					displayRectangle.X += maxImageSize.Height + 8;
					displayRectangle.Width -= maxImageSize.Height + 8;
				}
				else
				{
					displayRectangle.X += maxImageSize.Height + 4;
					displayRectangle.Width -= maxImageSize.Height + 4;
				}
			}
			if (m_Control == null)
			{
				return;
			}
			displayRectangle.Inflate(-2, -2);
			if (displayRectangle.Width > 0 && displayRectangle.Height > 0)
			{
				if (!m_Control.Size.Equals((object?)displayRectangle.Size))
				{
					m_Control.SuspendLayout();
					m_Control.Size = displayRectangle.Size;
					m_Control.ResumeLayout();
				}
				Point location = displayRectangle.Location;
				location.Offset((displayRectangle.Width - m_Control.Width) / 2, (displayRectangle.Height - m_Control.Height) / 2);
				if (location.X != m_Control.Location.X || location.Y != m_Control.Location.Y)
				{
					m_Control.Location = location;
				}
			}
		}
		finally
		{
			m_InternalResize = false;
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
		if (m_Control != null && m_Control.Visible != Displayed && !m_Control.Visible)
		{
			CustomizeChanged();
		}
		Rectangle displayRectangle = DisplayRectangle;
		Size maxImageSize = GetMaxImageSize();
		if (base.IsOnMenu && EffectiveStyle != eDotNetBarStyle.Office2000)
		{
			maxImageSize.Width += 7;
			displayRectangle.Width -= maxImageSize.Width;
			displayRectangle.X += maxImageSize.Width;
			if (base.IsOnCustomizeMenu)
			{
				maxImageSize.Width += maxImageSize.Height + 8;
			}
			if (!pa.Colors.MenuSide2.IsEmpty)
			{
				LinearGradientBrush val = BarFunctions.CreateLinearGradientBrush(new Rectangle(m_Rect.Left, m_Rect.Top, maxImageSize.Width, m_Rect.Height), pa.Colors.MenuSide, pa.Colors.MenuSide2, pa.Colors.MenuSideGradientAngle);
				graphics.FillRectangle((Brush)(object)val, m_Rect.Left, m_Rect.Top, maxImageSize.Width, m_Rect.Height);
				((Brush)val).Dispose();
			}
			else
			{
				graphics.FillRectangle((Brush)new SolidBrush(pa.Colors.MenuSide), m_Rect.Left, m_Rect.Top, maxImageSize.Width, m_Rect.Height);
			}
		}
		if (base.IsOnCustomizeMenu)
		{
			if (EffectiveStyle != eDotNetBarStyle.Office2000)
			{
				displayRectangle.X += maxImageSize.Height + 8;
				displayRectangle.Width -= maxImageSize.Height + 8;
			}
			else
			{
				displayRectangle.X += maxImageSize.Height + 4;
				displayRectangle.Width -= maxImageSize.Height + 4;
			}
		}
		if (m_Control == null)
		{
			string text = m_Text;
			if (text == "")
			{
				text = "Container";
			}
			eTextFormat stringFormat = GetStringFormat();
			Font font = GetFont();
			Rectangle bounds = new Rectangle(displayRectangle.X + 8, displayRectangle.Y, displayRectangle.Width, displayRectangle.Height);
			if (EffectiveStyle == eDotNetBarStyle.Office2000)
			{
				TextDrawing.DrawString(graphics, text, font, SystemColors.ControlText, bounds, stringFormat);
			}
			else
			{
				TextDrawing.DrawString(graphics, text, font, SystemColors.ControlText, bounds, stringFormat);
			}
			Size size = TextDrawing.MeasureString(graphics, text, font, 0, stringFormat);
			displayRectangle.X += size.Width + 8;
			displayRectangle.Width -= size.Width + 8;
		}
		else if (m_Control != null)
		{
			ResizeControl();
		}
		if (base.IsOnCustomizeMenu && Visible)
		{
			Rectangle r = new Rectangle(m_Rect.Left, m_Rect.Top, m_Rect.Height, m_Rect.Height);
			if (EffectiveStyle != eDotNetBarStyle.Office2000)
			{
				r.Inflate(-1, -1);
			}
			BarFunctions.DrawMenuCheckBox(pa, r, EffectiveStyle, m_MouseOver);
		}
		if (Focused && DesignMode)
		{
			displayRectangle = DisplayRectangle;
			displayRectangle.Inflate(-1, -1);
			DesignTime.DrawDesignTimeSelection(graphics, displayRectangle, pa.Colors.ItemDesignTimeBorder);
		}
	}

	public override void RecalcSize()
	{
		if (SuspendLayout)
		{
			return;
		}
		bool isOnMenu = base.IsOnMenu;
		if (m_Control == null && !DesignMode)
		{
			InitControl();
		}
		if (m_Control == null)
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
				if (m_Control != null && m_Rect.Height < m_Control.Height + 2)
				{
					m_Rect.Height = m_Control.Height + 2;
				}
			}
			else if (m_Control != null && m_Rect.Height < m_Control.Height + 2)
			{
				m_Rect.Height = m_Control.Height + 2;
			}
		}
		else
		{
			m_Rect.Height = m_MinimumSize.Height + 4;
		}
		m_Rect.Width = m_MinimumSize.Width + 4;
		if (m_Control == null)
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
					Size size = TextDrawing.MeasureString(val2, text, GetFont(), 0, GetStringFormat());
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
		Size maxImageSize = GetMaxImageSize();
		if (base.IsOnMenu && EffectiveStyle != eDotNetBarStyle.Office2000)
		{
			m_Rect.Width += maxImageSize.Width + 7;
		}
		if (base.IsOnCustomizeMenu)
		{
			m_Rect.Width += maxImageSize.Height + 2;
		}
		base.RecalcSize();
	}

	protected internal override void OnContainerChanged(object objOldContainer)
	{
		base.OnContainerChanged(objOldContainer);
		ParentControlChanged();
	}

	private void ParentControlChanged()
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		if (m_Control == null)
		{
			return;
		}
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val == m_Control.Parent)
		{
			return;
		}
		bool flag = false;
		if (m_Control.Parent != null)
		{
			if (m_Control is TextBox && ((TextBoxBase)(TextBox)m_Control).Multiline)
			{
				flag = true;
				((TextBoxBase)(TextBox)m_Control).Multiline = false;
			}
			m_Control.Parent.Controls.Remove(m_Control);
		}
		if (val != null)
		{
			val.Controls.Add(m_Control);
			if (m_Control is TextBox && flag)
			{
				((TextBoxBase)(TextBox)m_Control).Multiline = true;
			}
			if (m_Control.Visible)
			{
				m_Control.Refresh();
			}
		}
	}

	protected internal override void OnProcessDelayedCommands()
	{
		if (m_DelayedWidth >= 0)
		{
			int delayedWidth = m_DelayedWidth;
			m_DelayedWidth = -1;
			Width = delayedWidth;
		}
		if (m_DelayedHeight > 0)
		{
			int delayedHeight = m_DelayedHeight;
			m_DelayedHeight = -1;
			Height = delayedHeight;
		}
	}

	protected internal override void OnVisibleChanged(bool newValue)
	{
		if (m_Control != null && !newValue)
		{
			if (!DesignMode || Site == null)
			{
				m_Control.Visible = newValue;
			}
			else if (newValue)
			{
				m_Control.SendToBack();
			}
			else
			{
				m_Control.BringToFront();
			}
		}
		if (ContainerControl is Bar { LayoutType: eLayoutType.DockContainer } bar)
		{
			bar.OnDockContainerVisibleChanged(this);
			if (bar.AutoHide)
			{
				bar.RefreshAutoHidePanel();
				bar.RefreshDockTab(bFireEvents: true);
			}
			else
			{
				bar.RefreshDockTab(bFireEvents: true);
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
				bar.SyncBarCaption();
			}
		}
		if (m_Control != null && !base.IsOnCustomizeMenu && !base.IsOnCustomizeDialog)
		{
			ResizeControl();
			if (!DesignMode || Site == null)
			{
				m_Control.Visible = Displayed;
			}
			else
			{
				if (Displayed)
				{
					m_Control.BringToFront();
				}
				else
				{
					m_Control.SendToBack();
				}
				ResizeControl();
			}
		}
		base.OnDisplayedChanged();
	}

	protected override void OnIsOnCustomizeDialogChanged()
	{
		base.OnIsOnCustomizeDialogChanged();
		CustomizeChanged();
	}

	protected override void OnDesignModeChanged()
	{
		base.OnDesignModeChanged();
		CustomizeChanged();
	}

	protected override void OnIsOnCustomizeMenuChanged()
	{
		base.OnIsOnCustomizeMenuChanged();
		CustomizeChanged();
	}

	private void CustomizeChanged()
	{
		if (m_Control == null || (DesignMode && Site != null))
		{
			return;
		}
		if (base.IsOnCustomizeMenu || base.IsOnCustomizeDialog || DesignMode)
		{
			m_Control.Enabled = false;
			return;
		}
		if (!DesignMode || Site == null)
		{
			m_Control.Visible = Displayed;
		}
		m_Control.Enabled = true;
	}

	private Size GetMaxImageSize()
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

	private eTextFormat GetStringFormat()
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
		if (!flag && m_Control != null && m_Control.Handle == iHandle)
		{
			flag = true;
		}
		return flag;
	}

	protected override void OnEnabledChanged()
	{
		base.OnEnabledChanged();
		if ((!DesignMode || Site == null) && m_Control != null)
		{
			m_Control.Enabled = Enabled;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void OnGotFocus()
	{
		base.OnGotFocus();
		if (m_Control != null && !m_Control.Focused && !base.IsOnCustomizeMenu && !base.IsOnCustomizeDialog && !DesignMode)
		{
			m_Control.Focus();
		}
	}

	private Image GetImage()
	{
		if (m_Image != null)
		{
			return m_Image;
		}
		return GetImageFromImageList(m_ImageIndex);
	}

	internal void SetImageIndex(int iImageIndex)
	{
		m_ImageIndex = iImageIndex;
	}

	private Image GetImageFromImageList(int ImageIndex)
	{
		if (ImageIndex >= 0 && GetOwner() is IOwner { Images: not null } owner && owner.Images.Images.Count > 0 && ImageIndex < owner.Images.Images.Count)
		{
			return owner.Images.Images[ImageIndex];
		}
		return null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeMinimumSize()
	{
		if (m_MinimumSize.Width != 32 || m_MinimumSize.Height != 32)
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
		if (m_DefaultFloatingSize.Width != 128 || m_DefaultFloatingSize.Height != 128)
		{
			return true;
		}
		return false;
	}

	protected internal override void OnAfterItemRemoved(BaseItem item)
	{
		base.OnAfterItemRemoved(item);
		ParentControlChanged();
	}

	protected override void OnTextChanged()
	{
		base.OnTextChanged();
		if (ContainerControl is Bar)
		{
			((Bar)ContainerControl).RefreshDockContainerItem(this);
		}
	}

	protected override void OnTooltipChanged()
	{
		base.OnTooltipChanged();
		if (ContainerControl is Bar)
		{
			((Bar)ContainerControl).RefreshDockContainerItem(this);
		}
	}

	protected override void OnStyleChanged()
	{
		base.OnStyleChanged();
		if (m_Control is PanelDockContainer && !((PanelDockContainer)(object)m_Control).UseCustomStyle)
		{
			((PanelDockContainer)(object)m_Control).ColorSchemeStyle = Style;
		}
	}

	private void OnCanCloseChanged(eDockContainerClose oldValue, eDockContainerClose newValue)
	{
		if (ContainerControl is Bar bar)
		{
			bar.DockContainerItemCanCloseChanged(this, oldValue, newValue);
		}
	}
}
