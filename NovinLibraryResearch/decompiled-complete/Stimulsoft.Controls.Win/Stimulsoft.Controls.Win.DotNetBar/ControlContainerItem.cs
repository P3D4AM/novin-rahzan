using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
public class ControlContainerItem : ImageItem, IPersonalizedMenuItem
{
	public delegate void ControlContainerSerializationEventHandler(object sender, ControlContainerSerializationEventArgs e);

	private eMenuVisibility m_MenuVisibility;

	private bool m_RecentlyUsed;

	private Control m_Control;

	private bool m_MouseOver;

	private bool m_AllowItemResize = true;

	private bool m_CustomizeChangedExecuting;

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Indicates item's visiblity when on pop-up menu.")]
	public eMenuVisibility MenuVisibility
	{
		get
		{
			return m_MenuVisibility;
		}
		set
		{
			if (m_MenuVisibility != value)
			{
				m_MenuVisibility = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.SyncProperty(this, "MenuVisibility");
				}
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool RecentlyUsed
	{
		get
		{
			return m_RecentlyUsed;
		}
		set
		{
			if (m_RecentlyUsed != value)
			{
				m_RecentlyUsed = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.SyncProperty(this, "RecentlyUsed");
				}
			}
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DefaultValue("")]
	[Category("Appearance")]
	[Description("Indicates the text that is displayed when mouse hovers over the item.")]
	[Localizable(true)]
	public override string Tooltip
	{
		get
		{
			return base.Tooltip;
		}
		set
		{
			base.Tooltip = value;
		}
	}

	[Category("Data")]
	[DefaultValue(null)]
	[Description("Indicates the control that is managed by the item.")]
	public Control Control
	{
		get
		{
			return m_Control;
		}
		set
		{
			if (m_Control != null)
			{
				if (m_Control.Parent != null)
				{
					m_Control.Parent.Controls.Remove(m_Control);
				}
				m_Control.Enter -= HostedControlEnter;
				m_Control.GotFocus -= HostedControlGotFocus;
				m_Control.Resize -= HostedControlResized;
			}
			m_Control = value;
			if (m_Control == null)
			{
				return;
			}
			m_Control.Enter += HostedControlEnter;
			m_Control.GotFocus += HostedControlGotFocus;
			m_Control.Resize += HostedControlResized;
			if (m_Control.Parent != null)
			{
				m_Control.Parent.Controls.Remove(m_Control);
			}
			m_Control.Dock = (DockStyle)0;
			Control val = null;
			if (ContainerControl != null)
			{
				object containerControl = ContainerControl;
				val = (Control)((containerControl is Control) ? containerControl : null);
				if (val != null)
				{
					val.Controls.Add(m_Control);
					m_Control.Refresh();
				}
			}
			if (!Displayed)
			{
				m_Control.Visible = false;
			}
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override Rectangle Bounds
	{
		get
		{
			return base.Bounds;
		}
		set
		{
			if (base.Bounds != value)
			{
				bool flag = base.Bounds.Top != value.Top;
				bool flag2 = base.Bounds.Left != value.Left;
				base.Bounds = value;
				if (flag || flag2)
				{
					UpdateControlLocation();
				}
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Description("Specifies whether contained control can be automatically resized to fill the item container.")]
	public bool AllowItemResize
	{
		get
		{
			return m_AllowItemResize;
		}
		set
		{
			m_AllowItemResize = value;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override bool IsWindowed => true;

	public event EventHandler ContainerLoadControl;

	public event ControlContainerSerializationEventHandler ContainerControlSerialize;

	public event ControlContainerSerializationEventHandler ContainerControlDeserialize;

	public ControlContainerItem()
		: this("", "")
	{
	}

	public ControlContainerItem(string sName)
		: this(sName, "")
	{
	}

	public ControlContainerItem(string sName, string ItemText)
		: base(sName, ItemText)
	{
		m_SupportedOrientation = eSupportedOrientation.Horizontal;
	}

	internal void InitControl()
	{
		EventArgs e = new EventArgs();
		if (this.ContainerLoadControl != null)
		{
			this.ContainerLoadControl(this, e);
		}
		GetIOwnerItemEvents()?.InvokeContainerLoadControl(this, e);
		CustomizeChanged();
	}

	public override BaseItem Copy()
	{
		ControlContainerItem controlContainerItem = new ControlContainerItem(Name);
		controlContainerItem.AllowItemResize = AllowItemResize;
		CopyToItem(controlContainerItem);
		return controlContainerItem;
	}

	protected override void CopyToItem(BaseItem copy)
	{
		ControlContainerItem controlContainerItem = copy as ControlContainerItem;
		base.CopyToItem(controlContainerItem);
		controlContainerItem.ContainerLoadControl = this.ContainerLoadControl;
		controlContainerItem.InitControl();
	}

	protected override void Dispose(bool disposing)
	{
		if (m_Control != null)
		{
			m_Control.Enter -= HostedControlEnter;
			m_Control.GotFocus -= HostedControlGotFocus;
			m_Control.Resize -= HostedControlResized;
			m_Control = null;
		}
		base.Dispose(disposing);
	}

	protected internal override void Serialize(ItemSerializationContext context)
	{
		base.Serialize(context);
		XmlElement itemXmlElement = context.ItemXmlElement;
		itemXmlElement.SetAttribute("AllowResize", XmlConvert.ToString(m_AllowItemResize));
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
		m_AllowItemResize = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("AllowResize"));
		InitControl();
		if (this.ContainerControlDeserialize != null)
		{
			this.ContainerControlDeserialize(this, new ControlContainerSerializationEventArgs(itemXmlElement));
		}
		GetIOwnerItemEvents()?.InvokeContainerControlDeserialize(this, new ControlContainerSerializationEventArgs(itemXmlElement));
	}

	private void HostedControlResized(object sender, EventArgs e)
	{
		NeedRecalcSize = true;
		if (ContainerControl is MenuPanel)
		{
			((MenuPanel)ContainerControl).RecalcLayout();
		}
	}

	private void HostedControlGotFocus(object sender, EventArgs e)
	{
		SetMenuHotSubItem();
	}

	private void SetMenuHotSubItem()
	{
		if (base.IsOnMenu && ContainerControl is MenuPanel menuPanel && menuPanel.HotSubItem != this)
		{
			menuPanel.HotSubItem = this;
		}
	}

	private void HostedControlEnter(object sender, EventArgs e)
	{
		SetMenuHotSubItem();
	}

	public override void Paint(ItemPaintArgs pa)
	{
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Expected O, but got Unknown
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Expected O, but got Unknown
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
		bool flag = base.IsOnMenu;
		if (flag && Parent is ItemContainer)
		{
			flag = false;
		}
		if (Orientation == eOrientation.Horizontal)
		{
			if (flag && !Stretch)
			{
				maxImageSize.Width += 7;
				displayRectangle.Width -= maxImageSize.Width;
				displayRectangle.X += maxImageSize.Width;
				if (base.IsOnCustomizeMenu)
				{
					maxImageSize.Width += maxImageSize.Height + 8;
				}
				Rectangle rectangle = new Rectangle(m_Rect.Left, m_Rect.Top, maxImageSize.Width, m_Rect.Height);
				if (MenuVisibility == eMenuVisibility.VisibleIfRecentlyUsed && !RecentlyUsed)
				{
					if (!pa.Colors.MenuUnusedSide2.IsEmpty)
					{
						LinearGradientBrush val = BarFunctions.CreateLinearGradientBrush(rectangle, pa.Colors.MenuUnusedSide, pa.Colors.MenuUnusedSide2, pa.Colors.MenuUnusedSideGradientAngle);
						graphics.FillRectangle((Brush)(object)val, rectangle);
						((Brush)val).Dispose();
					}
					else
					{
						graphics.FillRectangle((Brush)new SolidBrush(pa.Colors.MenuUnusedSide), rectangle);
					}
				}
				else if (!pa.Colors.MenuSide2.IsEmpty)
				{
					LinearGradientBrush val2 = BarFunctions.CreateLinearGradientBrush(rectangle, pa.Colors.MenuSide, pa.Colors.MenuSide2, pa.Colors.MenuSideGradientAngle);
					graphics.FillRectangle((Brush)(object)val2, rectangle);
					((Brush)val2).Dispose();
				}
				else
				{
					graphics.FillRectangle((Brush)new SolidBrush(pa.Colors.MenuSide), rectangle);
				}
				if (BarFunctions.IsOffice2007Style(EffectiveStyle) && GlobalManager.Renderer is Office2007Renderer)
				{
					Office2007MenuColorTable menu = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.Menu;
					if (menu != null && !menu.SideBorder.IsEmpty)
					{
						if (pa.RightToLeft)
						{
							DisplayHelp.DrawGradientLine(graphics, rectangle.X + 1, rectangle.Y, rectangle.X + 1, rectangle.Bottom - 1, menu.SideBorder, 1);
						}
						else
						{
							DisplayHelp.DrawGradientLine(graphics, rectangle.Right - 2, rectangle.Y, rectangle.Right - 2, rectangle.Bottom - 1, menu.SideBorder, 1);
						}
					}
					if (menu != null && !menu.SideBorderLight.IsEmpty)
					{
						if (pa.RightToLeft)
						{
							DisplayHelp.DrawGradientLine(graphics, rectangle.X, rectangle.Y, rectangle.X, rectangle.Bottom - 1, menu.SideBorder, 1);
						}
						else
						{
							DisplayHelp.DrawGradientLine(graphics, rectangle.Right - 1, rectangle.Y, rectangle.Right - 1, rectangle.Bottom - 1, menu.SideBorderLight, 1);
						}
					}
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
			if (flag)
			{
				_ = EffectiveStyle;
				_ = 1;
			}
			if (m_Control == null || GetDesignMode() || base.IsOnCustomizeMenu)
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
				displayRectangle.Inflate(-2, -2);
				if (m_AllowItemResize)
				{
					m_Control.Width = displayRectangle.Width;
				}
				Point location = displayRectangle.Location;
				location.Offset((displayRectangle.Width - m_Control.Width) / 2, (displayRectangle.Height - m_Control.Height) / 2);
				Control containerControl = pa.ContainerControl;
				ScrollableControl val3 = (ScrollableControl)(object)((containerControl is ScrollableControl) ? containerControl : null);
				if (val3 != null && val3.AutoScroll)
				{
					location.Offset(val3.AutoScrollPosition.X, val3.AutoScrollPosition.Y);
				}
				if (m_Control.Location != location)
				{
					m_Control.Location = location;
				}
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
		}
		if (Focused && (GetDesignMode() || (m_Control == null && DesignMode)))
		{
			displayRectangle = DisplayRectangle;
			displayRectangle.Inflate(-1, -1);
			DesignTime.DrawDesignTimeSelection(graphics, displayRectangle, pa.Colors.ItemDesignTimeBorder);
		}
	}

	private void UpdateControlLocation()
	{
		if (m_Control != null)
		{
			Rectangle displayRectangle = DisplayRectangle;
			displayRectangle.Inflate(-2, -2);
			m_Control.Location = displayRectangle.Location;
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
		if (Parent != null && Parent is ImageItem)
		{
			m_Rect.Height = ((ImageItem)Parent).SubItemsImageSize.Height + 4;
		}
		else
		{
			m_Rect.Height = SubItemsImageSize.Height + 4;
		}
		if (EffectiveStyle == eDotNetBarStyle.OfficeXP || EffectiveStyle == eDotNetBarStyle.Office2003 || EffectiveStyle == eDotNetBarStyle.VS2005)
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
		if (m_Control != null)
		{
			if (Stretch)
			{
				m_Rect.Width = 32;
			}
			else
			{
				m_Rect.Width = m_Control.Width + 4;
			}
		}
		else
		{
			m_Rect.Width = 68;
		}
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
					m_Rect.Width = size.Width + 8;
				}
				finally
				{
					val2.Dispose();
				}
			}
		}
		Size maxImageSize = GetMaxImageSize();
		if (base.IsOnMenu && EffectiveStyle != eDotNetBarStyle.Office2000 && !Stretch && !(Parent is ItemContainer))
		{
			m_Rect.Width += maxImageSize.Width + 7;
		}
		if (base.IsOnCustomizeMenu)
		{
			m_Rect.Width += maxImageSize.Height + 2;
		}
		base.RecalcSize();
	}

	protected override void OnExternalSizeChange()
	{
		base.OnExternalSizeChange();
		if (m_AllowItemResize)
		{
			NeedRecalcSize = true;
		}
	}

	protected internal override void OnContainerChanged(object objOldContainer)
	{
		base.OnContainerChanged(objOldContainer);
		if (m_Control != null)
		{
			object containerControl = ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (m_Control.Parent != null && val != m_Control.Parent)
			{
				m_Control.Parent.Controls.Remove(m_Control);
			}
			if (val != null && m_Control.Parent == null)
			{
				val.Controls.Add(m_Control);
				m_Control.Refresh();
			}
		}
	}

	protected internal override void OnVisibleChanged(bool newValue)
	{
		if (m_Control != null && !newValue)
		{
			m_Control.Visible = newValue;
		}
		base.OnVisibleChanged(newValue);
	}

	protected override void OnDisplayedChanged()
	{
		if (!Displayed && m_Control != null && !base.IsOnCustomizeMenu && !base.IsOnCustomizeDialog && !GetDesignMode())
		{
			m_Control.Visible = Displayed;
		}
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
		if (m_Control == null || m_CustomizeChangedExecuting)
		{
			return;
		}
		m_CustomizeChangedExecuting = true;
		try
		{
			if (base.IsOnCustomizeMenu || base.IsOnCustomizeDialog || GetDesignMode())
			{
				m_Control.Visible = false;
			}
			else
			{
				m_Control.Visible = Displayed;
			}
		}
		finally
		{
			m_CustomizeChangedExecuting = false;
		}
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
		if (m_Control != null)
		{
			m_Control.Enabled = Enabled;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void InternalMouseEnter()
	{
		base.InternalMouseEnter();
		if (!m_MouseOver)
		{
			m_MouseOver = true;
			if (base.IsOnCustomizeMenu && Visible)
			{
				Refresh();
			}
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void InternalMouseLeave()
	{
		base.InternalMouseLeave();
		if (m_MouseOver)
		{
			m_MouseOver = false;
			if (base.IsOnCustomizeMenu && Visible)
			{
				Refresh();
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void OnGotFocus()
	{
		base.OnGotFocus();
		if (m_Control != null && !m_Control.Focused && !base.IsOnCustomizeMenu && !base.IsOnCustomizeDialog && !GetDesignMode())
		{
			m_Control.Focus();
		}
	}

	private bool GetDesignMode()
	{
		if (ContainerControl is IBarDesignerServices && ((IBarDesignerServices)ContainerControl).Designer != null)
		{
			return false;
		}
		return base.DesignMode;
	}
}
