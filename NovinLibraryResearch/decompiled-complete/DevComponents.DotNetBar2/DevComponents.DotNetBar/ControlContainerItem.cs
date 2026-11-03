using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar;

[ToolboxItem(false)]
[Designer("DevComponents.DotNetBar.Design.SimpleBaseItemDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[DesignTimeVisible(false)]
public class ControlContainerItem : ImageItem, IPersonalizedMenuItem
{
	public delegate void ControlContainerSerializationEventHandler(object sender, ControlContainerSerializationEventArgs e);

	private EventHandler _09CF;

	private ControlContainerSerializationEventHandler _09D1;

	private ControlContainerSerializationEventHandler _09D2;

	private eMenuVisibility _0E7F;

	private bool _0E80;

	private Control _10CF;

	private bool ݡ;

	private bool ს = true;

	private bool _ტ;

	private bool უ;

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Indicates item's visiblity when on pop-up menu.")]
	public eMenuVisibility MenuVisibility
	{
		get
		{
			return _0E7F;
		}
		set
		{
			if (_0E7F != value)
			{
				_0E7F = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.ढ़(this, "MenuVisibility");
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
			return _0E80;
		}
		set
		{
			if (_0E80 != value)
			{
				_0E80 = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.ढ़(this, "RecentlyUsed");
				}
			}
		}
	}

	[Browsable(false)]
	[Localizable(true)]
	[Description("Indicates the text that is displayed when mouse hovers over the item.")]
	[DevCoBrowsable(false)]
	[DefaultValue("")]
	[Category("Appearance")]
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
	[Description("Indicates the control that is managed by the item.")]
	[DefaultValue(null)]
	public Control Control
	{
		get
		{
			return _10CF;
		}
		set
		{
			if (_10CF != null)
			{
				if (_10CF.Parent != null)
				{
					_10CF.Parent.Controls.Remove(_10CF);
				}
				_10CF.Enter -= ყ;
				_10CF.GotFocus -= ქ;
				_10CF.Resize -= ფ;
			}
			_10CF = value;
			if (_10CF == null)
			{
				return;
			}
			_10CF.Enter += ყ;
			_10CF.GotFocus += ქ;
			_10CF.Resize += ფ;
			if (_10CF.Parent != null)
			{
				_10CF.Parent.Controls.Remove(_10CF);
			}
			_10CF.Dock = (DockStyle)0;
			Control val = null;
			if (ContainerControl != null)
			{
				object containerControl = ContainerControl;
				val = (Control)((containerControl is Control) ? containerControl : null);
				if (val != null)
				{
					val.Controls.Add(_10CF);
					_10CF.Refresh();
				}
			}
			if (!Displayed)
			{
				_10CF.Visible = false;
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
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
					შ();
				}
			}
		}
	}

	[DevCoBrowsable(true)]
	[Description("Specifies whether contained control can be automatically resized to fill the item container.")]
	[Category("Behavior")]
	[Browsable(true)]
	public bool AllowItemResize
	{
		get
		{
			return ს;
		}
		set
		{
			ს = value;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override bool IsWindowed => true;

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

	public event ControlContainerSerializationEventHandler ContainerControlSerialize
	{
		add
		{
			ControlContainerSerializationEventHandler controlContainerSerializationEventHandler = _09D1;
			ControlContainerSerializationEventHandler controlContainerSerializationEventHandler2;
			do
			{
				controlContainerSerializationEventHandler2 = controlContainerSerializationEventHandler;
				ControlContainerSerializationEventHandler value2 = (ControlContainerSerializationEventHandler)Delegate.Combine(controlContainerSerializationEventHandler2, value);
				controlContainerSerializationEventHandler = Interlocked.CompareExchange(ref _09D1, value2, controlContainerSerializationEventHandler2);
			}
			while ((object)controlContainerSerializationEventHandler != controlContainerSerializationEventHandler2);
		}
		remove
		{
			ControlContainerSerializationEventHandler controlContainerSerializationEventHandler = _09D1;
			ControlContainerSerializationEventHandler controlContainerSerializationEventHandler2;
			do
			{
				controlContainerSerializationEventHandler2 = controlContainerSerializationEventHandler;
				ControlContainerSerializationEventHandler value2 = (ControlContainerSerializationEventHandler)Delegate.Remove(controlContainerSerializationEventHandler2, value);
				controlContainerSerializationEventHandler = Interlocked.CompareExchange(ref _09D1, value2, controlContainerSerializationEventHandler2);
			}
			while ((object)controlContainerSerializationEventHandler != controlContainerSerializationEventHandler2);
		}
	}

	public event ControlContainerSerializationEventHandler ContainerControlDeserialize
	{
		add
		{
			ControlContainerSerializationEventHandler controlContainerSerializationEventHandler = _09D2;
			ControlContainerSerializationEventHandler controlContainerSerializationEventHandler2;
			do
			{
				controlContainerSerializationEventHandler2 = controlContainerSerializationEventHandler;
				ControlContainerSerializationEventHandler value2 = (ControlContainerSerializationEventHandler)Delegate.Combine(controlContainerSerializationEventHandler2, value);
				controlContainerSerializationEventHandler = Interlocked.CompareExchange(ref _09D2, value2, controlContainerSerializationEventHandler2);
			}
			while ((object)controlContainerSerializationEventHandler != controlContainerSerializationEventHandler2);
		}
		remove
		{
			ControlContainerSerializationEventHandler controlContainerSerializationEventHandler = _09D2;
			ControlContainerSerializationEventHandler controlContainerSerializationEventHandler2;
			do
			{
				controlContainerSerializationEventHandler2 = controlContainerSerializationEventHandler;
				ControlContainerSerializationEventHandler value2 = (ControlContainerSerializationEventHandler)Delegate.Remove(controlContainerSerializationEventHandler2, value);
				controlContainerSerializationEventHandler = Interlocked.CompareExchange(ref _09D2, value2, controlContainerSerializationEventHandler2);
			}
			while ((object)controlContainerSerializationEventHandler != controlContainerSerializationEventHandler2);
		}
	}

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

	internal void კ()
	{
		EventArgs e = new EventArgs();
		if (_09CF != null)
		{
			_09CF(this, e);
		}
		GetIOwnerItemEvents()?.InvokeContainerLoadControl(this, e);
		ნ();
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
		controlContainerItem._09CF = _09CF;
		controlContainerItem.კ();
	}

	protected override void Dispose(bool disposing)
	{
		if (_10CF != null)
		{
			_10CF.Enter -= ყ;
			_10CF.GotFocus -= ქ;
			_10CF.Resize -= ფ;
			_10CF = null;
		}
		base.Dispose(disposing);
	}

	protected internal override void Serialize(ItemSerializationContext context)
	{
		base.Serialize(context);
		XmlElement itemXmlElement = context.ItemXmlElement;
		itemXmlElement.SetAttribute("AllowResize", XmlConvert.ToString(ს));
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
		ს = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("AllowResize"));
		კ();
		if (_09D2 != null)
		{
			_09D2(this, new ControlContainerSerializationEventArgs(itemXmlElement));
		}
		GetIOwnerItemEvents()?.InvokeContainerControlDeserialize(this, new ControlContainerSerializationEventArgs(itemXmlElement));
	}

	private void ფ(object _٠, EventArgs ؿ)
	{
		NeedRecalcSize = true;
		if (ContainerControl is MenuPanel)
		{
			((MenuPanel)ContainerControl).RecalcLayout();
		}
	}

	private void ქ(object _٠, EventArgs ؿ)
	{
		ღ();
	}

	private void ღ()
	{
		if (base.IsOnMenu && ContainerControl is MenuPanel menuPanel && menuPanel.HotSubItem != this)
		{
			menuPanel.HotSubItem = this;
		}
	}

	private void ყ(object _٠, EventArgs ؿ)
	{
		ღ();
	}

	public override void Paint(ItemPaintArgs pa)
	{
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Expected O, but got Unknown
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Expected O, but got Unknown
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
		bool flag = base.IsOnMenu;
		if (flag && Parent is ItemContainer)
		{
			flag = false;
		}
		if (Orientation == eOrientation.Horizontal)
		{
			if (flag && !Stretch)
			{
				size.Width += 7;
				displayRectangle.Width -= size.Width;
				displayRectangle.X += size.Width;
				if (base.IsOnCustomizeMenu)
				{
					size.Width += size.Height + 8;
				}
				Rectangle rectangle = new Rectangle(m_Rect.Left, m_Rect.Top, size.Width, m_Rect.Height);
				if (MenuVisibility == eMenuVisibility.VisibleIfRecentlyUsed && !RecentlyUsed)
				{
					if (!pa.Colors.MenuUnusedSide2.IsEmpty)
					{
						LinearGradientBrush val = BarFunctions.ॴ(rectangle, pa.Colors.MenuUnusedSide, pa.Colors.MenuUnusedSide2, pa.Colors.MenuUnusedSideGradientAngle);
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
					LinearGradientBrush val2 = BarFunctions.ॴ(rectangle, pa.Colors.MenuSide, pa.Colors.MenuSide2, pa.Colors.MenuSideGradientAngle);
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
					displayRectangle.X += size.Height + 8;
					displayRectangle.Width -= size.Height + 8;
				}
				else
				{
					displayRectangle.X += size.Height + 4;
					displayRectangle.Width -= size.Height + 4;
				}
			}
			if (flag)
			{
				_ = EffectiveStyle;
				_ = 1;
			}
			if (_10CF == null || ఛ() || base.IsOnCustomizeMenu)
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
				displayRectangle.Inflate(-2, -2);
				if (ს)
				{
					_10CF.Width = displayRectangle.Width;
				}
				Point location = displayRectangle.Location;
				location.Offset((displayRectangle.Width - _10CF.Width) / 2, (displayRectangle.Height - _10CF.Height) / 2);
				Control containerControl = pa.ContainerControl;
				ScrollableControl val3 = (ScrollableControl)(object)((containerControl is ScrollableControl) ? containerControl : null);
				if (val3 != null && val3.AutoScroll)
				{
					location.Offset(val3.AutoScrollPosition.X, val3.AutoScrollPosition.Y);
				}
				if ((_10CF.Location != location && (!(_10CF is ComboBoxEx) || !(ContainerControl is RibbonBar) || BaseItem.IsOnPopup(this))) || _ტ)
				{
					_10CF.Location = location;
					_ტ = false;
				}
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
		}
		if (Focused && (ఛ() || (_10CF == null && DesignMode)))
		{
			displayRectangle = DisplayRectangle;
			displayRectangle.Inflate(-1, -1);
			_1A6B._1A6C(graphics, displayRectangle, pa.Colors.ItemDesignTimeBorder);
		}
	}

	private void შ()
	{
		if (_10CF != null)
		{
			if (base.IsLayoutPassInProgress)
			{
				_ტ = true;
				return;
			}
			Rectangle displayRectangle = DisplayRectangle;
			displayRectangle.Inflate(-2, -2);
			_10CF.Location = displayRectangle.Location;
			_ტ = false;
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
			if (_10CF != null && m_Rect.Height < _10CF.Height + 2)
			{
				m_Rect.Height = _10CF.Height + 2;
			}
		}
		else if (_10CF != null && m_Rect.Height < _10CF.Height + 2)
		{
			m_Rect.Height = _10CF.Height + 2;
		}
		if (_10CF != null)
		{
			if (Stretch)
			{
				m_Rect.Width = 32;
			}
			else
			{
				m_Rect.Width = _10CF.Width + 4;
			}
		}
		else
		{
			m_Rect.Width = 68;
		}
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
					m_Rect.Width = size.Width + 8;
				}
				finally
				{
					val2.Dispose();
				}
			}
		}
		Size size2 = ო();
		if (base.IsOnMenu && EffectiveStyle != eDotNetBarStyle.Office2000 && !Stretch && !(Parent is ItemContainer))
		{
			m_Rect.Width += size2.Width + 7;
		}
		if (base.IsOnCustomizeMenu)
		{
			m_Rect.Width += size2.Height + 2;
		}
		base.RecalcSize();
	}

	protected override void OnExternalSizeChange()
	{
		base.OnExternalSizeChange();
		if (ს)
		{
			NeedRecalcSize = true;
		}
	}

	protected internal override void OnContainerChanged(object objOldContainer)
	{
		base.OnContainerChanged(objOldContainer);
		if (_10CF != null)
		{
			object containerControl = ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (_10CF.Parent != null && val != _10CF.Parent)
			{
				_10CF.Parent.Controls.Remove(_10CF);
			}
			if (val != null && _10CF.Parent == null)
			{
				val.Controls.Add(_10CF);
				_10CF.Refresh();
			}
		}
	}

	protected internal override void OnVisibleChanged(bool newValue)
	{
		if (_10CF != null && !newValue)
		{
			_10CF.Visible = newValue;
		}
		base.OnVisibleChanged(newValue);
	}

	protected override void OnDisplayedChanged()
	{
		if (!Displayed && _10CF != null && !base.IsOnCustomizeMenu && !base.IsOnCustomizeDialog && !ఛ())
		{
			_10CF.Visible = Displayed;
		}
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
		if (_10CF == null || უ)
		{
			return;
		}
		უ = true;
		try
		{
			if (base.IsOnCustomizeMenu || base.IsOnCustomizeDialog || ఛ())
			{
				_10CF.Visible = false;
			}
			else
			{
				_10CF.Visible = Displayed;
			}
		}
		finally
		{
			უ = false;
		}
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
		if (_10CF != null)
		{
			_10CF.Enabled = Enabled;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void InternalMouseEnter()
	{
		base.InternalMouseEnter();
		if (!ݡ)
		{
			ݡ = true;
			if (base.IsOnCustomizeMenu && Visible)
			{
				Refresh();
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public override void InternalMouseLeave()
	{
		base.InternalMouseLeave();
		if (ݡ)
		{
			ݡ = false;
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
		if (_10CF != null && !_10CF.Focused && !base.IsOnCustomizeMenu && !base.IsOnCustomizeDialog && !ఛ())
		{
			_10CF.Focus();
		}
	}

	private bool ఛ()
	{
		if (ContainerControl is IBarDesignerServices && ((IBarDesignerServices)ContainerControl).Designer != null)
		{
			return false;
		}
		return base.DesignMode;
	}
}
