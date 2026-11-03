using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar.Controls;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
public class SideNavItem : ButtonItem
{
	private SideNavPanel __1371;

	private Rectangle __1379 = Rectangle.Empty;

	private Rectangle __137A = Rectangle.Empty;

	private Color[] __137D;

	private Color[] _ய;

	private int __1374;

	private string __20C2 = "";

	private bool __20C3;

	internal Rectangle _136A
	{
		get
		{
			return __1379;
		}
		set
		{
			__1379 = value;
		}
	}

	internal Rectangle _136B
	{
		get
		{
			return __137A;
		}
		set
		{
			__137A = value;
		}
	}

	protected override bool IsFadeEnabled => false;

	[Browsable(false)]
	[DefaultValue(null)]
	public SideNavPanel Panel
	{
		get
		{
			return __1371;
		}
		set
		{
			__1371 = value;
			ᎃ();
		}
	}

	private bool Մ => __1371 != null;

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DevCoBrowsable(false)]
	[DefaultValue("")]
	public override string OptionGroup
	{
		get
		{
			return base.OptionGroup;
		}
		set
		{
			base.OptionGroup = value;
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public override SubItemsCollection SubItems => base.SubItems;

	public override bool UseParentSubItemsImageSize => false;

	[Description("Indicates the array of colors that when set are used to draw the background of the item.")]
	[TypeConverter(typeof(ArrayConverter))]
	[Category("Appearance")]
	[DefaultValue(null)]
	public Color[] BackColors
	{
		get
		{
			return __137D;
		}
		set
		{
			if (__137D != value)
			{
				__137D = value;
				Refresh();
			}
		}
	}

	[Category("Appearance")]
	[Description("Indicates the array of colors that when set are used to draw the border of the item.")]
	[TypeConverter(typeof(ArrayConverter))]
	[DefaultValue(null)]
	public Color[] BorderColors
	{
		get
		{
			return _ய;
		}
		set
		{
			if (_ய != value)
			{
				_ய = value;
				Refresh();
			}
		}
	}

	[DefaultValue(0)]
	[Category("Layout")]
	[Browsable(true)]
	[Description("Indicates additional padding added around the tab item in pixels.")]
	public int PaddingHorizontal
	{
		get
		{
			return __1374;
		}
		set
		{
			__1374 = value;
			_20C5();
		}
	}

	[Description("Indicates an optional title for the associated panel. If not set item text is used.")]
	[DefaultValue("")]
	[Category("Appearance")]
	public string Title
	{
		get
		{
			return __20C2;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			if (__20C2 != value)
			{
				string oldValue = __20C2;
				__20C2 = value;
				OnTitleChanged(value, oldValue);
			}
		}
	}

	[Category("Behavior")]
	[DefaultValue(false)]
	[Description("Indicates whether this item acts as the SideNav control system menu which collapses and expands the SideNav items.")]
	public bool IsSystemMenu
	{
		get
		{
			return __20C3;
		}
		set
		{
			if (__20C3 != value)
			{
				bool oldValue = __20C3;
				__20C3 = value;
				OnIsSystemMenuChanged(value, oldValue);
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Category("Behavior")]
	[DefaultValue(true)]
	[DevCoBrowsable(false)]
	[Description("Indicates whether the item will auto-collapse (fold) when clicked.")]
	[Browsable(false)]
	public override bool AutoCollapseOnClick
	{
		get
		{
			return base.AutoCollapseOnClick;
		}
		set
		{
			base.AutoCollapseOnClick = value;
		}
	}

	[DevCoBrowsable(false)]
	[Category("Behavior")]
	[Description("Indicates whether the item will auto-collapse (fold) when clicked.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DefaultValue(false)]
	public override bool AutoExpandOnClick
	{
		get
		{
			return base.AutoExpandOnClick;
		}
		set
		{
			base.AutoExpandOnClick = value;
		}
	}

	[Description("Indicates whether item can be customized by user.")]
	[Category("Behavior")]
	[DefaultValue(true)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DevCoBrowsable(false)]
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

	[DefaultValue(false)]
	[Category("Appearance")]
	[Description("Indicates whether item is checked or not.")]
	[DevCoBrowsable(false)]
	[Browsable(false)]
	public override bool Checked
	{
		get
		{
			return base.Checked;
		}
		set
		{
			base.Checked = value;
		}
	}

	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Gets or sets whether Click event will be auto repeated when mouse button is kept pressed over the item.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
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

	[EditorBrowsable(EditorBrowsableState.Never)]
	[DevCoBrowsable(false)]
	[Category("Behavior")]
	[DefaultValue(600)]
	[Browsable(false)]
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
	[Description("Indicates whether is item enabled.")]
	[Category("Behavior")]
	[DevCoBrowsable(false)]
	[DefaultValue(true)]
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

	[Category("Appearance")]
	[DevCoBrowsable(false)]
	[DefaultValue(eMenuVisibility.VisibleAlways)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[Description("Indicates item's visiblity when on pop-up menu.")]
	public override eMenuVisibility MenuVisibility
	{
		get
		{
			return base.MenuVisibility;
		}
		set
		{
			base.MenuVisibility = value;
		}
	}

	[DevCoBrowsable(false)]
	[Category("Appearance")]
	[Description("Indicates when menu items are displayed when MenuVisiblity is set to VisibleIfRecentlyUsed and RecentlyUsed is true.")]
	[DefaultValue(ePersonalizedMenus.Disabled)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public override ePersonalizedMenus PersonalizedMenus
	{
		get
		{
			return base.PersonalizedMenus;
		}
		set
		{
			base.PersonalizedMenus = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Description("Indicates Animation type for Popups.")]
	[Browsable(false)]
	[Category("Behavior")]
	[DevCoBrowsable(false)]
	[DefaultValue(ePopupAnimation.ManagerControlled)]
	public override ePopupAnimation PopupAnimation
	{
		get
		{
			return base.PopupAnimation;
		}
		set
		{
			base.PopupAnimation = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Category("Appearance")]
	[DevCoBrowsable(false)]
	[DefaultValue(null)]
	[Description("Indicates the font that will be used on the popup window.")]
	[Browsable(false)]
	public override Font PopupFont
	{
		get
		{
			return base.PopupFont;
		}
		set
		{
			base.PopupFont = value;
		}
	}

	[Category("Appearance")]
	[Description("Indicates whether sub-items are shown on popup Bar or popup menu.")]
	[Browsable(false)]
	[DefaultValue(ePopupType.Menu)]
	[DevCoBrowsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override ePopupType PopupType
	{
		get
		{
			return base.PopupType;
		}
		set
		{
			base.PopupType = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Description("Specifies the inital width for the Bar that hosts pop-up items. Applies to PopupType.Toolbar only.")]
	[DevCoBrowsable(false)]
	[DefaultValue(200)]
	[Browsable(false)]
	[Category("Layout")]
	public override int PopupWidth
	{
		get
		{
			return base.PopupWidth;
		}
		set
		{
			base.PopupWidth = value;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DefaultValue(true)]
	[Category("Behavior")]
	[DevCoBrowsable(false)]
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

	[EditorBrowsable(EditorBrowsableState.Never)]
	[DefaultValue(false)]
	[Description("Indicates whether item will stretch to consume empty space. Items on stretchable, no-wrap Bars only.")]
	[Category("Appearance")]
	[DevCoBrowsable(false)]
	[Browsable(false)]
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

	[DevCoBrowsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Description("Indicates the width of the expand part of the button item.")]
	[Browsable(false)]
	[Category("Behavior")]
	[DefaultValue(12)]
	public override int SubItemsExpandWidth
	{
		get
		{
			return base.SubItemsExpandWidth;
		}
		set
		{
			base.SubItemsExpandWidth = value;
		}
	}

	[DevCoBrowsable(false)]
	[Category("Design")]
	[DefaultValue("")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[Description("Gets or set the alternative Shortcut Text.  This text appears next to the Text instead of any shortcuts")]
	public override string AlternateShortCutText
	{
		get
		{
			return base.AlternateShortCutText;
		}
		set
		{
			base.AlternateShortCutText = value;
		}
	}

	[Category("Appearance")]
	[Description("Indicates whether this item is beginning of the group.")]
	[DefaultValue(false)]
	[DevCoBrowsable(false)]
	[Browsable(false)]
	public override bool BeginGroup
	{
		get
		{
			return base.BeginGroup;
		}
		set
		{
			base.BeginGroup = value;
		}
	}

	[Browsable(false)]
	[Category("Design")]
	[DevCoBrowsable(false)]
	[DefaultValue("")]
	[Description("Indicates item category used to group similar items at design-time.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
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

	[DevCoBrowsable(false)]
	[Description("The foreground color used to display text when mouse is over the item.")]
	[Category("Appearance")]
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override Color HotForeColor
	{
		get
		{
			return base.HotForeColor;
		}
		set
		{
			base.HotForeColor = value;
		}
	}

	[DefaultValue(eHotTrackingStyle.Default)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Description("Indicates the way item is painting the picture when mouse is over it. Setting the value to Color will render the image in gray-scale when mouse is not over the item.")]
	[DevCoBrowsable(false)]
	[Browsable(false)]
	[Category("Appearance")]
	public override eHotTrackingStyle HotTrackingStyle
	{
		get
		{
			return base.HotTrackingStyle;
		}
		set
		{
			base.HotTrackingStyle = value;
		}
	}

	[Description("Determines the style of the button.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DefaultValue(eButtonStyle.ImageAndText)]
	[Category("Appearance")]
	[Browsable(false)]
	public override eButtonStyle ButtonStyle
	{
		get
		{
			return base.ButtonStyle;
		}
		set
		{
			base.ButtonStyle = value;
		}
	}

	public SideNavItem()
	{
		ButtonStyle = eButtonStyle.ImageAndText;
	}

	public override void Paint(ItemPaintArgs p)
	{
		BaseRenderer ໝ = p.ໝ;
		if (ໝ != null)
		{
			SideNavItemRendererEventArgs sideNavItemRendererEventArgs = new SideNavItemRendererEventArgs(this, p.Graphics);
			sideNavItemRendererEventArgs.က = p;
			ໝ.DrawSideNavItem(sideNavItemRendererEventArgs);
		}
		if (!string.IsNullOrEmpty(base.NotificationMarkText))
		{
			_1A6E._139A(p.Graphics, Bounds, base.NotificationMarkPosition, base.NotificationMarkText, new Size(base.NotificationMarkSize, base.NotificationMarkSize), base.NotificationMarkOffset, base.NotificationMarkColor);
		}
		if (DesignMode && Focused)
		{
			Rectangle bounds = Bounds;
			bounds.Inflate(-1, -1);
			_1A6B._1A6C(p.Graphics, bounds, p.Colors.ItemDesignTimeBorder);
		}
		DrawInsertMarker(p.Graphics);
	}

	public override void RecalcSize()
	{
		__1379 = Rectangle.Empty;
		__137A = Rectangle.Empty;
		base.RecalcSize();
	}

	public void Select()
	{
		if (Մ)
		{
			Checked = true;
		}
	}

	private void ᎃ()
	{
		ᎄ();
	}

	protected override void OnCheckedChanged()
	{
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		if (Checked && Parent != null)
		{
			ᎄ();
			foreach (BaseItem subItem in Parent.SubItems)
			{
				if (subItem != this && subItem is SideNavItem { Checked: not false } sideNavItem)
				{
					if (DesignMode)
					{
						TypeDescriptor.GetProperties(sideNavItem)["Checked"].SetValue(sideNavItem, false);
					}
					else
					{
						sideNavItem.Checked = false;
					}
				}
			}
		}
		if (BarFunctions.IsOffice2007Style(EffectiveStyle) && ContainerControl is Control)
		{
			((Control)ContainerControl).Invalidate();
		}
		if (!Checked)
		{
			ᎄ();
		}
		InvokeCheckedChanged();
	}

	private void ᎄ()
	{
		if (Checked && __1371 != null)
		{
			if (DesignMode)
			{
				if (!((Control)__1371).Visible)
				{
					((Control)__1371).Visible = true;
				}
				TypeDescriptor.GetProperties(__1371)["Visible"].SetValue(__1371, true);
				((Control)__1371).BringToFront();
			}
			else if (!((Control)__1371).IsDisposed)
			{
				((Control)__1371).Enabled = true;
				((Control)__1371).Visible = true;
				((Control)__1371).BringToFront();
			}
		}
		else if (!Checked && __1371 != null)
		{
			if (DesignMode)
			{
				TypeDescriptor.GetProperties(__1371)["Visible"].SetValue(__1371, false);
				return;
			}
			((Control)__1371).Visible = false;
			((Control)__1371).Enabled = false;
		}
	}

	protected override void OnClick()
	{
		base.OnClick();
		if (__20C3)
		{
			if (ContainerControl is SideNavStrip sideNavStrip && ((Control)sideNavStrip).Parent is SideNav)
			{
				((SideNav)(object)((Control)sideNavStrip).Parent).IsMenuExpanded = !((SideNav)(object)((Control)sideNavStrip).Parent).IsMenuExpanded;
			}
		}
		else
		{
			if (Checked || !Մ)
			{
				return;
			}
			if (DesignMode)
			{
				TypeDescriptor.GetProperties(this)["Checked"].SetValue(this, true);
				return;
			}
			SideNav sideNav = _20C4();
			if (sideNav == null || ((ContainerControl)sideNav).ValidateChildren())
			{
				Checked = true;
			}
		}
	}

	private SideNav _20C4()
	{
		if (!(ContainerControl is SideNavStrip sideNavStrip))
		{
			return null;
		}
		return ((Control)sideNavStrip).Parent as SideNav;
	}

	protected internal override void OnVisibleChanged(bool bVisible)
	{
		base.OnVisibleChanged(bVisible);
		if (bVisible || !Checked)
		{
			return;
		}
		TypeDescriptor.GetProperties(this)["Checked"].SetValue(this, false);
		if (Parent == null)
		{
			return;
		}
		foreach (BaseItem subItem in Parent.SubItems)
		{
			if (subItem != this && subItem.GetEnabled() && subItem.Visible && subItem is SideNavItem component)
			{
				TypeDescriptor.GetProperties(component)["Checked"].SetValue(this, true);
				break;
			}
		}
	}

	internal override void DoAccesibleDefaultAction()
	{
		if (Մ)
		{
			Checked = true;
		}
	}

	protected override void Invalidate(Control containerControl)
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		Rectangle r = m_Rect;
		r.Width++;
		r.Height++;
		if (containerControl.InvokeRequired)
		{
			containerControl.BeginInvoke((Delegate)(MethodInvoker)delegate
			{
				containerControl.Invalidate(r, true);
			});
		}
		else
		{
			containerControl.Invalidate(r, true);
		}
	}

	protected override void OnStyleChanged()
	{
		base.OnStyleChanged();
		_20C5();
	}

	private void _20C5()
	{
		base._0E62 = 6;
		base._0E63 = 14 + __1374;
		NeedRecalcSize = true;
		OnAppearanceChanged();
	}

	protected virtual void OnTitleChanged(string newValue, string oldValue)
	{
		if (Checked && ContainerControl is SideNavStrip sideNavStrip && ((Control)sideNavStrip).Parent is SideNav)
		{
			((SideNav)(object)((Control)sideNavStrip).Parent)._20E5();
		}
	}

	protected virtual void OnIsSystemMenuChanged(bool newValue, bool oldValue)
	{
		if (newValue && DesignMode)
		{
			if (string.IsNullOrEmpty(base.Symbol))
			{
				base.Symbol = "\uf0c9";
			}
			if (string.IsNullOrEmpty(Text))
			{
				Text = "Menu";
			}
		}
	}
}
