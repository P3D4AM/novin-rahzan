using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar.Metro.Rendering;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar.Metro;

[Designer("DevComponents.DotNetBar.Design.MetroTabItemDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[ToolboxItem(false)]
[DesignTimeVisible(false)]
public class MetroTabItem : ButtonItem
{
	private MetroTabPanel __1371;

	private eMetroTabColor __137C;

	private string __1372 = "Default";

	private bool __1373;

	private int __1374;

	private Rectangle __1379 = Rectangle.Empty;

	private Rectangle __137A = Rectangle.Empty;

	private bool _ᎈ = true;

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

	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	internal bool ᎇ
	{
		get
		{
			return _ᎈ;
		}
		set
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			_ᎈ = value;
			if (ContainerControl is Control)
			{
				((Control)ContainerControl).Invalidate();
			}
			else
			{
				Refresh();
			}
		}
	}

	protected override bool IsFadeEnabled => false;

	[Browsable(true)]
	[DefaultValue(0)]
	[Description("Indicates additional padding added around the tab item in pixels.")]
	[Category("Layout")]
	public int PaddingHorizontal
	{
		get
		{
			return __1374;
		}
		set
		{
			__1374 = value;
			ᎆ();
		}
	}

	internal bool _136C
	{
		get
		{
			return __1373;
		}
		set
		{
			__1373 = value;
		}
	}

	[Category("Appearance")]
	[Description("Indicates predefined color of item when Office 2007 style is used.")]
	[DefaultValue(eMetroTabColor.Default)]
	[Browsable(true)]
	public new eMetroTabColor ColorTable
	{
		get
		{
			return __137C;
		}
		set
		{
			if (__137C != value)
			{
				__137C = value;
				__1372 = Enum.GetName(typeof(eMetroTabColor), __137C);
				Refresh();
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(null)]
	public MetroTabPanel Panel
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

	[DefaultValue("")]
	[DevCoBrowsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
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
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[DevCoBrowsable(false)]
	public override SubItemsCollection SubItems => base.SubItems;

	public override bool UseParentSubItemsImageSize => false;

	[DevCoBrowsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Category("Behavior")]
	[Description("Indicates whether the item will auto-collapse (fold) when clicked.")]
	[DefaultValue(true)]
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

	[DefaultValue(false)]
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Description("Indicates whether the item will auto-collapse (fold) when clicked.")]
	[DevCoBrowsable(false)]
	[Category("Behavior")]
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
	[DevCoBrowsable(false)]
	[Browsable(false)]
	[Category("Behavior")]
	[DefaultValue(true)]
	[EditorBrowsable(EditorBrowsableState.Never)]
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

	[DevCoBrowsable(false)]
	[Description("Indicates whether item is checked or not.")]
	[Browsable(false)]
	[Category("Appearance")]
	[DefaultValue(false)]
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

	[Category("Behavior")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DevCoBrowsable(false)]
	[Description("Gets or sets whether Click event will be auto repeated when mouse button is kept pressed over the item.")]
	[DefaultValue(false)]
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
	[Description("Gets or sets the auto-repeat interval for the click event when mouse button is kept pressed over the item.")]
	[DevCoBrowsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Category("Behavior")]
	[DefaultValue(600)]
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

	[DefaultValue(true)]
	[Browsable(false)]
	[DevCoBrowsable(false)]
	[Category("Behavior")]
	[Description("Indicates whether is item enabled.")]
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

	[Description("Indicates item's visiblity when on pop-up menu.")]
	[DefaultValue(eMenuVisibility.VisibleAlways)]
	[DevCoBrowsable(false)]
	[Category("Appearance")]
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
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

	[Category("Appearance")]
	[DefaultValue(ePersonalizedMenus.Disabled)]
	[DevCoBrowsable(false)]
	[Description("Indicates when menu items are displayed when MenuVisiblity is set to VisibleIfRecentlyUsed and RecentlyUsed is true.")]
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
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

	[Category("Behavior")]
	[Description("Indicates Animation type for Popups.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DefaultValue(ePopupAnimation.ManagerControlled)]
	[DevCoBrowsable(false)]
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

	[Category("Appearance")]
	[Description("Indicates the font that will be used on the popup window.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DefaultValue(null)]
	[Browsable(false)]
	[DevCoBrowsable(false)]
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
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DefaultValue(ePopupType.Menu)]
	[Browsable(false)]
	[DevCoBrowsable(false)]
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

	[Category("Layout")]
	[Description("Specifies the inital width for the Bar that hosts pop-up items. Applies to PopupType.Toolbar only.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DefaultValue(200)]
	[Browsable(false)]
	[DevCoBrowsable(false)]
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

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Determines whether sub-items are displayed.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DevCoBrowsable(false)]
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

	[DefaultValue(false)]
	[Category("Appearance")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Description("Indicates whether item will stretch to consume empty space. Items on stretchable, no-wrap Bars only.")]
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
	[Description("Indicates the width of the expand part of the button item.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DevCoBrowsable(false)]
	[DefaultValue(12)]
	[Browsable(false)]
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

	[Category("Design")]
	[Description("Gets or set the alternative Shortcut Text.  This text appears next to the Text instead of any shortcuts")]
	[DevCoBrowsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DefaultValue("")]
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

	[DefaultValue(false)]
	[Category("Appearance")]
	[Description("Indicates whether this item is beginning of the group.")]
	[Browsable(false)]
	[DevCoBrowsable(false)]
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

	[Category("Design")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DevCoBrowsable(false)]
	[DefaultValue("")]
	[Description("Indicates item category used to group similar items at design-time.")]
	[Browsable(false)]
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

	[Category("Appearance")]
	[Description("The foreground color used to display text when mouse is over the item.")]
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DevCoBrowsable(false)]
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

	[Category("Appearance")]
	[Description("Indicates the way item is painting the picture when mouse is over it. Setting the value to Color will render the image in gray-scale when mouse is not over the item.")]
	[DevCoBrowsable(false)]
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DefaultValue(eHotTrackingStyle.Default)]
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

	[DevCoBrowsable(false)]
	[Category("Appearance")]
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Description("The foreground color used to display text.")]
	public override Color ForeColor
	{
		get
		{
			return base.ForeColor;
		}
		set
		{
			base.ForeColor = value;
		}
	}

	[Category("Appearance")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[Description("Determines the style of the button.")]
	[DefaultValue(eButtonStyle.ImageAndText)]
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

	public MetroTabItem()
	{
		ButtonStyle = eButtonStyle.ImageAndText;
	}

	public override void Paint(ItemPaintArgs p)
	{
		MetroRender.Paint(this, p);
		if (!string.IsNullOrEmpty(base.NotificationMarkText))
		{
			_1A6E._139A(p.Graphics, Bounds, base.NotificationMarkPosition, base.NotificationMarkText, new Size(base.NotificationMarkSize, base.NotificationMarkSize), base.NotificationMarkOffset, base.NotificationMarkColor);
		}
	}

	public override void RecalcSize()
	{
		__1379 = Rectangle.Empty;
		__137A = Rectangle.Empty;
		base.RecalcSize();
	}

	public void Select()
	{
		Checked = true;
	}

	internal override string ຽ()
	{
		if (!(CustomColorName != ""))
		{
			return __1372;
		}
		return CustomColorName;
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
				if (subItem != this && subItem is MetroTabItem { Checked: not false } metroTabItem)
				{
					if (DesignMode)
					{
						TypeDescriptor.GetProperties(metroTabItem)["Checked"].SetValue(metroTabItem, false);
					}
					else
					{
						metroTabItem.Checked = false;
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
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		if (Checked && __1371 != null)
		{
			if (DesignMode)
			{
				if (!__1371.Visible)
				{
					__1371.Visible = true;
				}
				TypeDescriptor.GetProperties(__1371)["Visible"].SetValue(__1371, true);
				((Control)__1371).BringToFront();
			}
			else if (!((Control)__1371).IsDisposed)
			{
				DockStyle dock = ((Control)__1371).Dock;
				((Control)__1371).Dock = (DockStyle)0;
				__1371.Location = new Point(-32000, 32000);
				((Control)__1371).Enabled = true;
				__1371.Visible = true;
				((Control)__1371).BringToFront();
				if (((Control)__1371).Dock != dock)
				{
					((Control)__1371).Dock = dock;
				}
			}
		}
		else if (!Checked && __1371 != null && !__1371.ᑗ)
		{
			if (DesignMode)
			{
				TypeDescriptor.GetProperties(__1371)["Visible"].SetValue(__1371, false);
				return;
			}
			__1371.Visible = false;
			((Control)__1371).Enabled = false;
		}
	}

	protected override void OnClick()
	{
		base.OnClick();
		if (Checked)
		{
			return;
		}
		if (DesignMode)
		{
			TypeDescriptor.GetProperties(this)["Checked"].SetValue(this, true);
			return;
		}
		MetroShell metroShell = ᑖ();
		if (metroShell == null || ((ContainerControl)metroShell).ValidateChildren())
		{
			Checked = true;
		}
	}

	private MetroShell ᑖ()
	{
		if (!(ContainerControl is MetroTabStrip metroTabStrip))
		{
			return null;
		}
		return ((Control)metroTabStrip).Parent as MetroShell;
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
			if (subItem != this && subItem.GetEnabled() && subItem.Visible && subItem is MetroTabItem component)
			{
				TypeDescriptor.GetProperties(component)["Checked"].SetValue(this, true);
				break;
			}
		}
	}

	protected override void OnStyleChanged()
	{
		base.OnStyleChanged();
		ᎆ();
	}

	private void ᎆ()
	{
		if (StyleManager.Style == eStyle.OfficeMobile2014 || StyleManager.Style == eStyle.Office2016)
		{
			base._0E62 = 5;
			base._0E63 = 16 + __1374;
		}
		else
		{
			base._0E62 = 1;
			base._0E63 = 16 + __1374;
		}
		NeedRecalcSize = true;
		OnAppearanceChanged();
	}

	internal override void DoAccesibleDefaultAction()
	{
		Checked = true;
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
}
