using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace DevComponents.DotNetBar;

[DesignTimeVisible(false)]
[Designer("DevComponents.DotNetBar.Design.RibbonTabItemDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[ToolboxItem(false)]
public class RibbonTabItem : ButtonItem
{
	private RibbonPanel ᰉ;

	private RibbonTabItemGroup ᰊ;

	private eRibbonTabColor ຊ;

	private string ᰋ = "Default";

	private bool ᰌ;

	private int ᰍ;

	private bool _ᎈ = true;

	private DateTime ᰎ = DateTime.MinValue;

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

	protected override bool IsFadeEnabled
	{
		get
		{
			if ((EffectiveStyle == eDotNetBarStyle.Office2010 || StyleManager.IsMetro(EffectiveStyle)) && _0EFE.ষ)
			{
				return false;
			}
			return base.IsFadeEnabled;
		}
	}

	[Browsable(true)]
	[Category("Layout")]
	[DefaultValue(0)]
	[Description("Indicates additional padding added around the tab item in pixels.")]
	public int PaddingHorizontal
	{
		get
		{
			return ᰍ;
		}
		set
		{
			ᰍ = value;
			ᎆ();
		}
	}

	internal bool _136C
	{
		get
		{
			return ᰌ;
		}
		set
		{
			ᰌ = value;
		}
	}

	[Category("Appearance")]
	[DefaultValue(eRibbonTabColor.Default)]
	[Browsable(true)]
	[Description("Indicates predefined color of item when Office 2007 style is used.")]
	public new eRibbonTabColor ColorTable
	{
		get
		{
			return ຊ;
		}
		set
		{
			if (ຊ != value)
			{
				ຊ = value;
				ᰋ = Enum.GetName(typeof(eRibbonTabColor), ຊ);
				Refresh();
			}
		}
	}

	[Category("Tab Group")]
	[Editor("DevComponents.DotNetBar.Design.RibbonTabItemGroupTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[DevCoBrowsable(true)]
	[Description("Indicates the group tab belongs to.")]
	[Browsable(true)]
	[DefaultValue(null)]
	public RibbonTabItemGroup Group
	{
		get
		{
			return ᰊ;
		}
		set
		{
			ᰊ = value;
			if (DesignMode && ContainerControl is ItemControl itemControl)
			{
				itemControl.RecalcLayout();
			}
		}
	}

	[DefaultValue(null)]
	[Browsable(false)]
	public RibbonPanel Panel
	{
		get
		{
			return ᰉ;
		}
		set
		{
			if (ᰉ != null)
			{
				ᰉ.RibbonTabItem = null;
			}
			ᰉ = value;
			if (ᰉ != null)
			{
				ᰉ.RibbonTabItem = this;
			}
			ᎃ();
		}
	}

	[DefaultValue("")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DevCoBrowsable(false)]
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

	[DevCoBrowsable(false)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public override SubItemsCollection SubItems => base.SubItems;

	[Category("Behavior")]
	[DevCoBrowsable(false)]
	[DefaultValue(true)]
	[Description("Indicates whether the item will auto-collapse (fold) when clicked.")]
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
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

	[Browsable(false)]
	[Category("Behavior")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DefaultValue(false)]
	[DevCoBrowsable(false)]
	[Description("Indicates whether the item will auto-collapse (fold) when clicked.")]
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

	[DevCoBrowsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Description("Indicates whether item can be customized by user.")]
	[Category("Behavior")]
	[DefaultValue(true)]
	[Browsable(false)]
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

	[Category("Appearance")]
	[Description("Indicates whether item is checked or not.")]
	[Browsable(false)]
	[DefaultValue(false)]
	[DevCoBrowsable(false)]
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

	[EditorBrowsable(EditorBrowsableState.Never)]
	[DefaultValue(false)]
	[DevCoBrowsable(false)]
	[Category("Behavior")]
	[Description("Gets or sets whether Click event will be auto repeated when mouse button is kept pressed over the item.")]
	[Browsable(false)]
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

	[Category("Behavior")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Description("Gets or sets the auto-repeat interval for the click event when mouse button is kept pressed over the item.")]
	[Browsable(false)]
	[DevCoBrowsable(false)]
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

	[Description("Indicates whether is item enabled.")]
	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DefaultValue(true)]
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

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[Category("Appearance")]
	[DefaultValue(eMenuVisibility.VisibleAlways)]
	[Description("Indicates item's visiblity when on pop-up menu.")]
	[DevCoBrowsable(false)]
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

	[Description("Indicates when menu items are displayed when MenuVisiblity is set to VisibleIfRecentlyUsed and RecentlyUsed is true.")]
	[DevCoBrowsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DefaultValue(ePersonalizedMenus.Disabled)]
	[Browsable(false)]
	[Category("Appearance")]
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
	[DefaultValue(ePopupAnimation.ManagerControlled)]
	[DevCoBrowsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
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

	[DefaultValue(null)]
	[DevCoBrowsable(false)]
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Category("Appearance")]
	[Description("Indicates the font that will be used on the popup window.")]
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
	[DevCoBrowsable(false)]
	[DefaultValue(ePopupType.Menu)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
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
	[DefaultValue(200)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DevCoBrowsable(false)]
	[Browsable(false)]
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

	[Description("Determines whether sub-items are displayed.")]
	[DefaultValue(true)]
	[Category("Behavior")]
	[DevCoBrowsable(false)]
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
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
	[Category("Appearance")]
	[Description("Indicates whether item will stretch to consume empty space. Items on stretchable, no-wrap Bars only.")]
	[Browsable(false)]
	[DefaultValue(false)]
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
	[DefaultValue(12)]
	[DevCoBrowsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
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

	[Browsable(false)]
	[Description("Gets or set the alternative Shortcut Text.  This text appears next to the Text instead of any shortcuts")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Category("Design")]
	[DevCoBrowsable(false)]
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

	[Category("Appearance")]
	[Description("Indicates whether this item is beginning of the group.")]
	[DefaultValue(false)]
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

	[DevCoBrowsable(false)]
	[DefaultValue("")]
	[Category("Design")]
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
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

	[Description("The foreground color used to display text when mouse is over the item.")]
	[DevCoBrowsable(false)]
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Category("Appearance")]
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

	[Description("Indicates the way item is painting the picture when mouse is over it. Setting the value to Color will render the image in gray-scale when mouse is not over the item.")]
	[DevCoBrowsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DefaultValue(eHotTrackingStyle.Default)]
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

	[Category("Appearance")]
	[DevCoBrowsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
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

	public void Select()
	{
		Checked = true;
	}

	internal override string ຽ()
	{
		if (!(CustomColorName != ""))
		{
			return ᰋ;
		}
		return CustomColorName;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetGroup()
	{
		TypeDescriptor.GetProperties(this)["Group"].SetValue(this, null);
	}

	private void ᎃ()
	{
		ᎄ();
	}

	protected override void OnCheckedChanged()
	{
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		if (Checked && Parent != null)
		{
			ᎄ();
			foreach (BaseItem subItem in Parent.SubItems)
			{
				if (subItem != this && subItem is RibbonTabItem { Checked: not false } ribbonTabItem)
				{
					if (DesignMode)
					{
						TypeDescriptor.GetProperties(ribbonTabItem)["Checked"].SetValue(ribbonTabItem, false);
					}
					else
					{
						ribbonTabItem.Checked = false;
					}
				}
			}
		}
		ᰎ = DateTime.MinValue;
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
		if (Checked && ᰉ != null)
		{
			if (DesignMode)
			{
				if (!ᰉ.Visible)
				{
					ᰉ.Visible = true;
				}
				TypeDescriptor.GetProperties(ᰉ)["Visible"].SetValue(ᰉ, true);
				((Control)ᰉ).BringToFront();
			}
			else if (!((Control)ᰉ).IsDisposed)
			{
				((Control)ᰉ).SuspendLayout();
				ᰉ.Visible = true;
				((Control)ᰉ).ResumeLayout(true);
				((Control)ᰉ).BringToFront();
			}
		}
		else if (!Checked && ᰉ != null && !ᰉ.ᑗ)
		{
			if (DesignMode)
			{
				TypeDescriptor.GetProperties(ᰉ)["Visible"].SetValue(ᰉ, false);
			}
			else
			{
				ᰉ.Visible = false;
			}
		}
	}

	protected override void OnClick()
	{
		base.OnClick();
		if (!Checked)
		{
			if (DesignMode)
			{
				TypeDescriptor.GetProperties(this)["Checked"].SetValue(this, true);
			}
			else
			{
				Checked = true;
			}
		}
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (!(val is RibbonStrip) || !((RibbonStrip)(object)val)._൫ || !(val.Parent is RibbonControl))
		{
			return;
		}
		RibbonControl ribbonControl = (RibbonControl)(object)val.Parent;
		if (!ribbonControl.Expanded)
		{
			TimeSpan timeSpan = DateTime.Now.Subtract(ᰎ);
			if (ᰎ == DateTime.MinValue || Math.Abs(timeSpan.TotalMilliseconds) > (double)SystemInformation.DoubleClickTime)
			{
				ribbonControl.ᱚ(this);
				ᰎ = DateTime.MinValue;
			}
		}
	}

	protected override void InvokeDoubleClick()
	{
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val is RibbonStrip && ((RibbonStrip)(object)val)._൫ && val.Parent is RibbonControl)
		{
			((RibbonControl)(object)val.Parent).ᱜ(this);
			ᰎ = DateTime.Now;
		}
		base.InvokeDoubleClick();
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
			if (subItem != this && subItem.GetEnabled() && subItem.Visible && subItem is RibbonTabItem component)
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
		eDotNetBarStyle effectiveStyle = EffectiveStyle;
		if (effectiveStyle == eDotNetBarStyle.Office2007)
		{
			base._0E62 = 0;
			base._0E63 = 12 + ᰍ;
		}
		else if (effectiveStyle == eDotNetBarStyle.Office2010 || StyleManager.IsMetro(effectiveStyle))
		{
			base._0E62 = 1;
			base._0E63 = 16 + ᰍ;
		}
		else
		{
			switch (effectiveStyle)
			{
			case eDotNetBarStyle.Windows7:
				base._0E62 = 0;
				base._0E63 = 16 + ᰍ;
				break;
			case eDotNetBarStyle.Office2003:
				base._0E62 = -1;
				base._0E63 = 2 + ᰍ;
				break;
			default:
				base._0E62 = 0;
				base._0E63 = ᰍ;
				break;
			}
		}
		NeedRecalcSize = true;
		OnAppearanceChanged();
	}

	protected override void ScaleItem(SizeF factor)
	{
		ᎆ();
		base.ScaleItem(factor);
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
