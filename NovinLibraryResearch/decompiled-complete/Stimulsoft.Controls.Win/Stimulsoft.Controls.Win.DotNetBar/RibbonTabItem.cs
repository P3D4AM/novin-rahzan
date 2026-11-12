using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.Events;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
public class RibbonTabItem : ButtonItem
{
	private RibbonPanel m_Panel;

	private RibbonTabItemGroup m_Group;

	private eRibbonTabColor m_ColorTable;

	private string m_CashedColorTableName = "Default";

	private bool m_ReducedSize;

	private int m_PaddingHorizontal;

	private bool _RenderTabState = true;

	private DateTime m_LastAutoExpandTime = DateTime.MinValue;

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	internal bool RenderTabState
	{
		get
		{
			return _RenderTabState;
		}
		set
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			_RenderTabState = value;
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
			if ((EffectiveStyle == eDotNetBarStyle.Office2010 || EffectiveStyle == eDotNetBarStyle.Metro) && WinApi.IsGlassEnabled)
			{
				return false;
			}
			return base.IsFadeEnabled;
		}
	}

	[Browsable(true)]
	[DefaultValue(0)]
	[Category("Layout")]
	[Description("Indicates additional padding added around the tab item in pixels.")]
	public int PaddingHorizontal
	{
		get
		{
			return m_PaddingHorizontal;
		}
		set
		{
			m_PaddingHorizontal = value;
			UpdateTabAppearance();
		}
	}

	internal bool ReducedSize
	{
		get
		{
			return m_ReducedSize;
		}
		set
		{
			m_ReducedSize = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(eRibbonTabColor.Default)]
	[Category("Appearance")]
	[Description("Indicates predefined color of item when Office 2007 style is used.")]
	public new eRibbonTabColor ColorTable
	{
		get
		{
			return m_ColorTable;
		}
		set
		{
			if (m_ColorTable != value)
			{
				m_ColorTable = value;
				m_CashedColorTableName = Enum.GetName(typeof(eRibbonTabColor), m_ColorTable);
				Refresh();
			}
		}
	}

	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.RibbonTabItemGroupTypeEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Browsable(true)]
	[DefaultValue(null)]
	[DevCoBrowsable(true)]
	[Category("Tab Group")]
	[Description("Indicates the group tab belongs to.")]
	public RibbonTabItemGroup Group
	{
		get
		{
			return m_Group;
		}
		set
		{
			m_Group = value;
			if (DesignMode && ContainerControl is ItemControl itemControl)
			{
				itemControl.RecalcLayout();
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(null)]
	public RibbonPanel Panel
	{
		get
		{
			return m_Panel;
		}
		set
		{
			m_Panel = value;
			OnPanelChanged();
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DefaultValue("")]
	[EditorBrowsable(EditorBrowsableState.Never)]
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

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Category("Behavior")]
	[DefaultValue(true)]
	[Description("Indicates whether the item will auto-collapse (fold) when clicked.")]
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
	[DevCoBrowsable(false)]
	[Category("Behavior")]
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

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DefaultValue(true)]
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
	[DevCoBrowsable(false)]
	[Category("Appearance")]
	[Description("Indicates whether item is checked or not.")]
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

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
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
	[EditorBrowsable(EditorBrowsableState.Never)]
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
	[DefaultValue(true)]
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

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Category("Appearance")]
	[Description("Indicates item's visiblity when on pop-up menu.")]
	[DefaultValue(eMenuVisibility.VisibleAlways)]
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

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DevCoBrowsable(false)]
	[Category("Appearance")]
	[Description("Indicates when menu items are displayed when MenuVisiblity is set to VisibleIfRecentlyUsed and RecentlyUsed is true.")]
	[DefaultValue(ePersonalizedMenus.Disabled)]
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

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Category("Behavior")]
	[Description("Indicates Animation type for Popups.")]
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

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Category("Appearance")]
	[Description("Indicates the font that will be used on the popup window.")]
	[DefaultValue(null)]
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

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Category("Appearance")]
	[Description("Indicates whether sub-items are shown on popup Bar or popup menu.")]
	[DefaultValue(ePopupType.Menu)]
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

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Category("Layout")]
	[Description("Specifies the inital width for the Bar that hosts pop-up items. Applies to PopupType.Toolbar only.")]
	[DefaultValue(200)]
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
	[DevCoBrowsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
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

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DefaultValue(false)]
	[Category("Appearance")]
	[Description("Indicates whether item will stretch to consume empty space. Items on stretchable, no-wrap Bars only.")]
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
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Category("Behavior")]
	[Description("Indicates the width of the expand part of the button item.")]
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

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DevCoBrowsable(false)]
	[Category("Design")]
	[Description("Gets or set the alternative Shortcut Text.  This text appears next to the Text instead of any shortcuts")]
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

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DefaultValue(false)]
	[Category("Appearance")]
	[Description("Indicates whether this item is beginning of the group.")]
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
	[DevCoBrowsable(false)]
	[DefaultValue("")]
	[Category("Design")]
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

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[Category("Appearance")]
	[Description("The foreground color used to display text when mouse is over the item.")]
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

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[Category("Appearance")]
	[Description("Indicates the way item is painting the picture when mouse is over it. Setting the value to Color will render the image in gray-scale when mouse is not over the item.")]
	[DefaultValue(eHotTrackingStyle.Default)]
	[EditorBrowsable(EditorBrowsableState.Never)]
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

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Category("Appearance")]
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

	internal override string GetColorTableName()
	{
		if (!(CustomColorName != ""))
		{
			return m_CashedColorTableName;
		}
		return CustomColorName;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetGroup()
	{
		TypeDescriptor.GetProperties(this)["Group"].SetValue(this, null);
	}

	private void OnPanelChanged()
	{
		ChangePanelVisibility();
	}

	protected override void OnCheckedChanged()
	{
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		ButtonItem oldItem = null;
		if (Checked && Parent != null)
		{
			ChangePanelVisibility();
			foreach (BaseItem subItem in Parent.SubItems)
			{
				if (subItem != this && subItem is RibbonTabItem { Checked: not false } ribbonTabItem)
				{
					oldItem = ribbonTabItem;
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
		m_LastAutoExpandTime = DateTime.MinValue;
		if (BarFunctions.IsOffice2007Style(EffectiveStyle) && ContainerControl is Control)
		{
			((Control)ContainerControl).Invalidate();
		}
		if (!Checked)
		{
			ChangePanelVisibility();
		}
		InvokeCheckedChanged(new CheckedChangedEventArgs(oldItem));
	}

	private void ChangePanelVisibility()
	{
		if (Checked && m_Panel != null)
		{
			if (DesignMode)
			{
				if (!m_Panel.Visible)
				{
					m_Panel.Visible = true;
				}
				TypeDescriptor.GetProperties(m_Panel)["Visible"].SetValue(m_Panel, true);
				((Control)m_Panel).BringToFront();
			}
			else if (!((Control)m_Panel).IsDisposed)
			{
				((Control)m_Panel).SuspendLayout();
				m_Panel.Visible = true;
				((Control)m_Panel).ResumeLayout(true);
				((Control)m_Panel).BringToFront();
			}
		}
		else if (!Checked && m_Panel != null && !m_Panel.IsPopupMode)
		{
			if (DesignMode)
			{
				TypeDescriptor.GetProperties(m_Panel)["Visible"].SetValue(m_Panel, false);
			}
			else
			{
				m_Panel.Visible = false;
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
		if (!(val is RibbonStrip) || !((RibbonStrip)(object)val).AutoExpand || !(val.Parent is RibbonControl))
		{
			return;
		}
		RibbonControl ribbonControl = (RibbonControl)(object)val.Parent;
		if (!ribbonControl.Expanded)
		{
			TimeSpan timeSpan = DateTime.Now.Subtract(m_LastAutoExpandTime);
			if (m_LastAutoExpandTime == DateTime.MinValue || Math.Abs(timeSpan.TotalMilliseconds) > (double)SystemInformation.DoubleClickTime)
			{
				ribbonControl.RibbonTabItemClick(this);
				m_LastAutoExpandTime = DateTime.MinValue;
			}
		}
	}

	protected override void InvokeDoubleClick()
	{
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val is RibbonStrip && ((RibbonStrip)(object)val).AutoExpand && val.Parent is RibbonControl)
		{
			((RibbonControl)(object)val.Parent).RibbonTabItemDoubleClick(this);
			m_LastAutoExpandTime = DateTime.Now;
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
		UpdateTabAppearance();
	}

	private void UpdateTabAppearance()
	{
		if (EffectiveStyle == eDotNetBarStyle.Office2007)
		{
			base.VerticalPadding = 0;
			base.HorizontalPadding = 12 + m_PaddingHorizontal;
		}
		else if (EffectiveStyle == eDotNetBarStyle.Office2010 || EffectiveStyle == eDotNetBarStyle.Metro)
		{
			base.VerticalPadding = 1;
			base.HorizontalPadding = 16 + m_PaddingHorizontal;
		}
		else if (EffectiveStyle == eDotNetBarStyle.Windows7)
		{
			base.VerticalPadding = 0;
			base.HorizontalPadding = 16 + m_PaddingHorizontal;
		}
		else if (EffectiveStyle == eDotNetBarStyle.Office2003)
		{
			base.VerticalPadding = -1;
			base.HorizontalPadding = 2 + m_PaddingHorizontal;
		}
		else
		{
			base.VerticalPadding = 0;
			base.HorizontalPadding = m_PaddingHorizontal;
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
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
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
