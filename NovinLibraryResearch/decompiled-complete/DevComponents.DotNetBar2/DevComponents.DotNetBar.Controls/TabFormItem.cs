using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar.Controls;

public class TabFormItem : TabFormItemBase
{
	private TabFormPanel __1371;

	private string __1372 = "Default";

	private bool __1373;

	private int __1374;

	private bool __1375;

	private InsertPosition __1376;

	private eButtonState __1377 = eButtonState.Hidden;

	private Rectangle __1378 = Rectangle.Empty;

	private Rectangle __1379 = Rectangle.Empty;

	private Rectangle __137A = Rectangle.Empty;

	private bool __137B = true;

	private eTabFormItemColor __137C;

	private Color[] __137D;

	private TabFormItemColorTable __137E;

	public static readonly string DefaultColorTableKey = Enum.GetName(typeof(eTabFormItemColor), eTabFormItemColor.Default);

	public static readonly string GreenColorTableKey = Enum.GetName(typeof(eTabFormItemColor), eTabFormItemColor.Green);

	public static readonly string MagentaColorTableKey = Enum.GetName(typeof(eTabFormItemColor), eTabFormItemColor.Magenta);

	public static readonly string OrangeColorTableKey = Enum.GetName(typeof(eTabFormItemColor), eTabFormItemColor.Orange);

	public static readonly string RedColorTableKey = Enum.GetName(typeof(eTabFormItemColor), eTabFormItemColor.Red);

	public static readonly string BlueColorTableKey = Enum.GetName(typeof(eTabFormItemColor), eTabFormItemColor.Blue);

	public static readonly string YellowColorTableKey = Enum.GetName(typeof(eTabFormItemColor), eTabFormItemColor.Yellow);

	public static readonly string PurpleColorTableKey = Enum.GetName(typeof(eTabFormItemColor), eTabFormItemColor.Purple);

	public static readonly string CyanColorTableKey = Enum.GetName(typeof(eTabFormItemColor), eTabFormItemColor.Cyan);

	public static readonly string BlueMistColorTableKey = Enum.GetName(typeof(eTabFormItemColor), eTabFormItemColor.BlueMist);

	public static readonly string PurpleMistColorTableKey = Enum.GetName(typeof(eTabFormItemColor), eTabFormItemColor.PurpleMist);

	public static readonly string TanColorTableKey = Enum.GetName(typeof(eTabFormItemColor), eTabFormItemColor.Tan);

	public static readonly string LemonLimeColorTableKey = Enum.GetName(typeof(eTabFormItemColor), eTabFormItemColor.LemonLime);

	public static readonly string AppleColorTableKey = Enum.GetName(typeof(eTabFormItemColor), eTabFormItemColor.Apple);

	public static readonly string TealColorTableKey = Enum.GetName(typeof(eTabFormItemColor), eTabFormItemColor.Teal);

	public static readonly string RedChalkColorTableKey = Enum.GetName(typeof(eTabFormItemColor), eTabFormItemColor.RedChalk);

	public static readonly string SilverColorTableKey = Enum.GetName(typeof(eTabFormItemColor), eTabFormItemColor.Silver);

	private bool _1367
	{
		get
		{
			if (!(ContainerControl is TabFormStripControl tabFormStripControl))
			{
				return false;
			}
			return tabFormStripControl.IsTabDragEnabled;
		}
	}

	internal eButtonState _1368
	{
		get
		{
			return __1377;
		}
		set
		{
			if (value != __1377)
			{
				__1377 = value;
				if (!__1378.IsEmpty)
				{
					Invalidate();
				}
			}
		}
	}

	internal Rectangle _1369
	{
		get
		{
			return __1378;
		}
		set
		{
			__1378 = value;
		}
	}

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

	[Category("Layout")]
	[DefaultValue(0)]
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
			ᎆ();
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether close button is visible on the tabs which when clicked closes the tab.")]
	[Category("Appearance")]
	public bool CloseButtonVisible
	{
		get
		{
			return __137B;
		}
		set
		{
			__137B = value;
			if (!value)
			{
				__1377 = eButtonState.Hidden;
			}
			NeedRecalcSize = true;
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

	[Browsable(true)]
	[Description("Indicates predefined color of the tab.")]
	[DefaultValue(eTabFormItemColor.Default)]
	[Category("Appearance")]
	public new eTabFormItemColor ColorTable
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
				__1372 = Enum.GetName(typeof(eTabFormItemColor), __137C);
				Refresh();
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(null)]
	public TabFormPanel Panel
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

	[DevCoBrowsable(false)]
	[DefaultValue("")]
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

	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(false)]
	public override SubItemsCollection SubItems => base.SubItems;

	public override bool UseParentSubItemsImageSize => false;

	[EditorBrowsable(EditorBrowsableState.Never)]
	[DefaultValue(true)]
	[Description("Indicates whether the item will auto-collapse (fold) when clicked.")]
	[DevCoBrowsable(false)]
	[Category("Behavior")]
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

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DefaultValue(true)]
	[Description("Indicates whether item can be customized by user.")]
	[Category("Behavior")]
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
	[Browsable(false)]
	[Description("Gets or sets whether Click event will be auto repeated when mouse button is kept pressed over the item.")]
	[Category("Behavior")]
	[DevCoBrowsable(false)]
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
	[Category("Behavior")]
	[Description("Gets or sets the auto-repeat interval for the click event when mouse button is kept pressed over the item.")]
	[DefaultValue(600)]
	[DevCoBrowsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
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
	[Category("Behavior")]
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

	[DefaultValue(eMenuVisibility.VisibleAlways)]
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DevCoBrowsable(false)]
	[Category("Appearance")]
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

	[Browsable(false)]
	[Description("Indicates Animation type for Popups.")]
	[DefaultValue(ePopupAnimation.ManagerControlled)]
	[Category("Behavior")]
	[DevCoBrowsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
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

	[Description("Indicates the font that will be used on the popup window.")]
	[DevCoBrowsable(false)]
	[DefaultValue(null)]
	[Browsable(false)]
	[Category("Appearance")]
	[EditorBrowsable(EditorBrowsableState.Never)]
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

	[Description("Indicates whether sub-items are shown on popup Bar or popup menu.")]
	[DevCoBrowsable(false)]
	[Category("Appearance")]
	[Browsable(false)]
	[DefaultValue(ePopupType.Menu)]
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

	[Category("Layout")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Description("Specifies the inital width for the Bar that hosts pop-up items. Applies to PopupType.Toolbar only.")]
	[DevCoBrowsable(false)]
	[DefaultValue(200)]
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
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DefaultValue(true)]
	[Category("Behavior")]
	[DevCoBrowsable(false)]
	[Browsable(false)]
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
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DevCoBrowsable(false)]
	[Category("Appearance")]
	[Description("Indicates whether item will stretch to consume empty space. Items on stretchable, no-wrap Bars only.")]
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

	[DefaultValue(12)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Category("Behavior")]
	[DevCoBrowsable(false)]
	[Description("Indicates the width of the expand part of the button item.")]
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

	[DevCoBrowsable(false)]
	[Category("Design")]
	[Description("Gets or set the alternative Shortcut Text.  This text appears next to the Text instead of any shortcuts")]
	[DefaultValue("")]
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
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
	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DefaultValue(false)]
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

	[DefaultValue("")]
	[Category("Design")]
	[Description("Indicates item category used to group similar items at design-time.")]
	[DevCoBrowsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
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

	[Browsable(false)]
	[Category("Appearance")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Description("The foreground color used to display text when mouse is over the item.")]
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
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DefaultValue(eHotTrackingStyle.Default)]
	[Browsable(false)]
	[DevCoBrowsable(false)]
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
	[DefaultValue(eButtonStyle.ImageAndText)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[Category("Appearance")]
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

	public eTabFormStripControlDock TabAlignment
	{
		get
		{
			if (!(ContainerControl is TabFormStripControl tabFormStripControl))
			{
				return eTabFormStripControlDock.Top;
			}
			return tabFormStripControl.TabAlignment;
		}
	}

	[Description("Indicates the array of colors that when set are used to draw the background of the item.")]
	[Category("Appearance")]
	[DefaultValue(null)]
	[TypeConverter(typeof(ArrayConverter))]
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

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public TabFormItemColorTable CustomColorTable
	{
		get
		{
			return __137E;
		}
		set
		{
			__137E = value;
		}
	}

	internal static int _136D => Dpi.Width8;

	internal static int _136E => _136D * 2;

	internal static int ܭ => Dpi.Width2;

	internal static int _136F => Dpi.Width25;

	internal static int _1370 => 13;

	public TabFormItem()
	{
		ButtonStyle = eButtonStyle.ImageAndText;
	}

	public override void Paint(ItemPaintArgs p)
	{
		if (!_137F(p))
		{
			BaseRenderer ໝ = p.ໝ;
			if (ໝ != null)
			{
				p.ButtonItemRendererEventArgs.Graphics = p.Graphics;
				p.ButtonItemRendererEventArgs.ButtonItem = this;
				p.ButtonItemRendererEventArgs.က = p;
				ໝ.DrawTabFormItem(p.ButtonItemRendererEventArgs);
			}
		}
		if (!string.IsNullOrEmpty(base.NotificationMarkText))
		{
			_1A6E._139A(p.Graphics, Bounds, base.NotificationMarkPosition, base.NotificationMarkText, new Size(base.NotificationMarkSize, base.NotificationMarkSize), base.NotificationMarkOffset, base.NotificationMarkColor);
		}
		DrawInsertMarker(p.Graphics);
	}

	private bool _137F(ItemPaintArgs ᎀ)
	{
		TabFormControl tabFormControl = ᎅ();
		if (tabFormControl == null)
		{
			return false;
		}
		return !tabFormControl.Უ(this, ᎀ);
	}

	public override void RecalcSize()
	{
		__1379 = Rectangle.Empty;
		__137A = Rectangle.Empty;
		base.RecalcSize();
	}

	public override void InternalMouseMove(MouseEventArgs objArg)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)objArg.Button == 1048576 && (_1368 == eButtonState.Normal || _1368 == eButtonState.Disabled || _1368 == eButtonState.Hidden) && (Math.Abs(objArg.X - base.MouseDownPt.X) > Dpi.Width6 || Math.Abs(objArg.Y - base.MouseDownPt.Y) > Dpi.Width6) && _1367)
		{
			if (ContainerControl is TabFormStripControl tabFormStripControl)
			{
				tabFormStripControl.StartItemDrag(this);
			}
		}
		else if (__137B && !__1378.IsEmpty)
		{
			if (__1378.Contains(objArg.Location))
			{
				_1368 = eButtonState.MouseOver;
			}
			else
			{
				_1368 = eButtonState.Normal;
			}
		}
		base.InternalMouseMove(objArg);
	}

	public override void InternalMouseDown(MouseEventArgs objArg)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		if (__1377 == eButtonState.MouseOver && (int)objArg.Button == 1048576)
		{
			_1368 = eButtonState.MouseDownLeft;
		}
		base.InternalMouseDown(objArg);
	}

	public override void InternalMouseUp(MouseEventArgs objArg)
	{
		if (__1377 == eButtonState.MouseDownLeft)
		{
			_1368 = eButtonState.Normal;
			ᎁ(eEventSource.Mouse);
		}
		base.InternalMouseUp(objArg);
	}

	private void ᎁ(eEventSource ᎂ)
	{
		if (ContainerControl is TabFormStripControl tabFormStripControl)
		{
			tabFormStripControl.CloseTab(this, ᎂ);
		}
	}

	public override void InternalMouseLeave()
	{
		if (__137B)
		{
			_1368 = eButtonState.Normal;
		}
		base.InternalMouseLeave();
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
				if (subItem != this && subItem is TabFormItem { Checked: not false } tabFormItem)
				{
					if (DesignMode)
					{
						TypeDescriptor.GetProperties(tabFormItem)["Checked"].SetValue(tabFormItem, false);
					}
					else
					{
						tabFormItem.Checked = false;
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
				if (!((Control)__1371).Visible)
				{
					((Control)__1371).Visible = true;
				}
				TypeDescriptor.GetProperties(__1371)["Visible"].SetValue(__1371, true);
				((Control)__1371).BringToFront();
			}
			else if (!((Control)__1371).IsDisposed)
			{
				DockStyle dock = ((Control)__1371).Dock;
				((Control)__1371).Dock = (DockStyle)0;
				((Control)__1371).Location = new Point(-32000, 32000);
				((Control)__1371).Enabled = true;
				((Control)__1371).Visible = true;
				((Control)__1371).BringToFront();
				if (((Control)__1371).Dock != dock)
				{
					((Control)__1371).Dock = dock;
				}
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
		if (Checked)
		{
			return;
		}
		if (DesignMode)
		{
			TypeDescriptor.GetProperties(this)["Checked"].SetValue(this, true);
			return;
		}
		TabFormControl tabFormControl = ᎅ();
		if (tabFormControl == null || ((ContainerControl)tabFormControl).ValidateChildren())
		{
			Checked = true;
		}
	}

	private TabFormControl ᎅ()
	{
		if (!(ContainerControl is TabFormStripControl tabFormStripControl))
		{
			return null;
		}
		return ((Control)tabFormStripControl).Parent as TabFormControl;
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
			if (subItem != this && subItem.GetEnabled() && subItem.Visible && subItem is TabFormItem component)
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
		base._0E62 = 2;
		base._0E63 = 28 + __1374 + (__137B ? _1370 : 0);
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

	protected override bool ShouldDrawInsertMarker()
	{
		GetOwner();
		if (base.DesignInsertMarker != 0 && Visible && Displayed)
		{
			return !DesignMode;
		}
		return false;
	}
}
