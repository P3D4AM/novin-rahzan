using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Metro.Rendering;
using Stimulsoft.Controls.Win.Events;

namespace Stimulsoft.Controls.Win.DotNetBar.Metro;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
public class MetroTabItem : ButtonItem
{
	private MetroTabPanel _Panel;

	private eMetroTabColor _ColorTable;

	private string _CashedColorTableName = "Default";

	private bool _ReducedSize;

	private int _PaddingHorizontal;

	private Rectangle _ImageRenderBounds = Rectangle.Empty;

	private Rectangle _TextRenderBounds = Rectangle.Empty;

	private bool _RenderTabState = true;

	internal Rectangle ImageRenderBounds
	{
		get
		{
			return _ImageRenderBounds;
		}
		set
		{
			_ImageRenderBounds = value;
		}
	}

	internal Rectangle TextRenderBounds
	{
		get
		{
			return _TextRenderBounds;
		}
		set
		{
			_TextRenderBounds = value;
		}
	}

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

	protected override bool IsFadeEnabled => false;

	[Browsable(true)]
	[DefaultValue(0)]
	[Category("Layout")]
	[Description("Indicates additional padding added around the tab item in pixels.")]
	public int PaddingHorizontal
	{
		get
		{
			return _PaddingHorizontal;
		}
		set
		{
			_PaddingHorizontal = value;
			UpdateTabAppearance();
		}
	}

	internal bool ReducedSize
	{
		get
		{
			return _ReducedSize;
		}
		set
		{
			_ReducedSize = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(eMetroTabColor.Default)]
	[Category("Appearance")]
	[Description("Indicates predefined color of item when Office 2007 style is used.")]
	public new eMetroTabColor ColorTable
	{
		get
		{
			return _ColorTable;
		}
		set
		{
			if (_ColorTable != value)
			{
				_ColorTable = value;
				_CashedColorTableName = Enum.GetName(typeof(eMetroTabColor), _ColorTable);
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
			return _Panel;
		}
		set
		{
			_Panel = value;
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

	[Browsable(false)]
	[Category("Appearance")]
	[Description("Determines the style of the button.")]
	[DefaultValue(eButtonStyle.ImageAndText)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
	}

	public override void RecalcSize()
	{
		_ImageRenderBounds = Rectangle.Empty;
		_TextRenderBounds = Rectangle.Empty;
		base.RecalcSize();
	}

	public void Select()
	{
		Checked = true;
	}

	internal override string GetColorTableName()
	{
		if (!(CustomColorName != ""))
		{
			return _CashedColorTableName;
		}
		return CustomColorName;
	}

	private void OnPanelChanged()
	{
		ChangePanelVisibility();
	}

	protected override void OnCheckedChanged()
	{
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		ButtonItem oldItem = null;
		if (Checked && Parent != null)
		{
			ChangePanelVisibility();
			foreach (BaseItem subItem in Parent.SubItems)
			{
				if (subItem != this && subItem is MetroTabItem { Checked: not false } metroTabItem)
				{
					oldItem = metroTabItem;
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
			ChangePanelVisibility();
		}
		InvokeCheckedChanged(new CheckedChangedEventArgs(oldItem));
	}

	private void ChangePanelVisibility()
	{
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		if (Checked && _Panel != null)
		{
			if (DesignMode)
			{
				if (!_Panel.Visible)
				{
					_Panel.Visible = true;
				}
				TypeDescriptor.GetProperties(_Panel)["Visible"].SetValue(_Panel, true);
				((Control)_Panel).BringToFront();
			}
			else if (!((Control)_Panel).IsDisposed)
			{
				DockStyle dock = ((Control)_Panel).Dock;
				((Control)_Panel).Dock = (DockStyle)0;
				_Panel.Location = new Point(-32000, 32000);
				_Panel.Visible = true;
				((Control)_Panel).BringToFront();
				if (((Control)_Panel).Dock != dock)
				{
					((Control)_Panel).Dock = dock;
				}
			}
		}
		else if (!Checked && _Panel != null && !_Panel.IsPopupMode)
		{
			if (DesignMode)
			{
				TypeDescriptor.GetProperties(_Panel)["Visible"].SetValue(_Panel, false);
			}
			else
			{
				_Panel.Visible = false;
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
		UpdateTabAppearance();
	}

	private void UpdateTabAppearance()
	{
		base.VerticalPadding = 1;
		base.HorizontalPadding = 16 + _PaddingHorizontal;
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
