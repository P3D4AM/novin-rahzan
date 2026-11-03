using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;
using DevComponents.UI.ContentManager;

namespace DevComponents.DotNetBar;

[Designer("DevComponents.DotNetBar.Design.ItemContainerDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[DesignTimeVisible(false)]
[ToolboxItem(false)]
public class ItemContainer : ImageItem, IBlock, IBlockExtended, IDesignTimeProvider
{
	private eOrientation ᅡ;

	private Size ᅢ = new Size(24, 24);

	private bool ෆ;

	private int ᅣ = 1;

	private Rectangle ᅤ = Rectangle.Empty;

	private bool ᅥ = true;

	private eHorizontalItemsAlignment ᅦ;

	private eVerticalItemsAlignment ᅧ;

	private Size Ⴓ = Size.Empty;

	private ElementStyle _0599 = new ElementStyle();

	private int ᅨ;

	private bool ೠ;

	private bool _ᅩ;

	private Rectangle _ᅪ = Rectangle.Empty;

	private Size _ᅫ = Size.Empty;

	private bool _ᅬ;

	private Size _ᅭ = Size.Empty;

	private Rectangle _ᅮ = Rectangle.Empty;

	private bool _ᅯ;

	private bool _ᅰ;

	private bool __0CE3;

	private SerialContentLayoutManager ᅱ;

	private bool __0CE4;

	private _249A ᅲ;

	private ElementStyle _ᅳ;

	private ElementStyle _ᅴ;

	private bool _ᅵ = true;

	private string _ᅶ = "";

	private eTitlePosition _ᅷ;

	private bool _ᅸ = true;

	public Size MinimumSize
	{
		get
		{
			return Ⴓ;
		}
		set
		{
			Ⴓ = value;
			NeedRecalcSize = true;
			OnAppearanceChanged();
		}
	}

	[Description("Gets or sets container background style.")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Style")]
	public ElementStyle BackgroundStyle => _0599;

	[DefaultValue(eHorizontalItemsAlignment.Left)]
	[Description("Indicates item alignment when container is in horizontal layout.")]
	[Category("Layout")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	public virtual eHorizontalItemsAlignment HorizontalItemAlignment
	{
		get
		{
			return ᅦ;
		}
		set
		{
			ᅦ = value;
			NeedRecalcSize = true;
			OnAppearanceChanged();
		}
	}

	[DefaultValue(eVerticalItemsAlignment.Top)]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Layout")]
	[Description("Indicates item item vertical alignment.")]
	public virtual eVerticalItemsAlignment VerticalItemAlignment
	{
		get
		{
			return ᅧ;
		}
		set
		{
			ᅧ = value;
			NeedRecalcSize = true;
			OnAppearanceChanged();
		}
	}

	[Category("Layout")]
	[Description("Indicates whether items in horizontal layout are wrapped into the new line when they cannot fit allotted container size.")]
	[Browsable(true)]
	[DefaultValue(false)]
	public virtual bool MultiLine
	{
		get
		{
			return ೠ;
		}
		set
		{
			ೠ = value;
			NeedRecalcSize = true;
			OnAppearanceChanged();
		}
	}

	[Browsable(false)]
	public virtual bool CanExpand => false;

	[DefaultValue(1)]
	[Browsable(true)]
	[Description("Indicates spacing in pixels between items.")]
	[Category("Layout")]
	public int ItemSpacing
	{
		get
		{
			return ᅣ;
		}
		set
		{
			if (ᅣ != value)
			{
				ᅣ = value;
				NeedRecalcSize = true;
				OnAppearanceChanged();
			}
		}
	}

	private bool ᅘ
	{
		get
		{
			if (BarFunctions.IsOffice2007StyleOnly(EffectiveStyle))
			{
				return BeginGroup;
			}
			return false;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool TitleMouseOver
	{
		get
		{
			return _ᅩ;
		}
		set
		{
			if (_ᅩ != value)
			{
				_ᅩ = value;
				if (_ᅴ.Custom)
				{
					Invalidate();
				}
			}
		}
	}

	[Browsable(false)]
	public Rectangle TitleRectangle => _ᅪ;

	[Description("Indicates suggested fixed size of the container. Width or Height or both may be set to suggest fixed size for the container.")]
	[Category("Appearance")]
	public Size FixedSize
	{
		get
		{
			return _ᅫ;
		}
		set
		{
			if (value != _ᅫ)
			{
				Size ᅫ = _ᅫ;
				_ᅫ = value;
				OnFixedSizeChanged(ᅫ, value);
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool TitleWidthFill
	{
		get
		{
			return _ᅬ;
		}
		set
		{
			_ᅬ = value;
			NeedRecalcSize = true;
		}
	}

	internal Rectangle ᅙ => _ᅮ;

	internal bool ᅚ
	{
		get
		{
			return _ᅯ;
		}
		set
		{
			_ᅯ = value;
		}
	}

	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public override eOrientation Orientation
	{
		get
		{
			return eOrientation.Horizontal;
		}
		set
		{
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Layout")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	[DefaultValue(eOrientation.Horizontal)]
	public virtual eOrientation LayoutOrientation
	{
		get
		{
			return ᅡ;
		}
		set
		{
			ᅡ = value;
			Ⴞ();
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[DevCoBrowsable(true)]
	[Description("Indicates whether items contained by container are resized to fit the container bounds. When container is in horizontal layout mode then all items will have the same height. When container is in vertical layout mode then all items will have the same width.")]
	[Category("Layout")]
	public virtual bool ResizeItemsToFit
	{
		get
		{
			return ᅥ;
		}
		set
		{
			ᅥ = value;
			OnAppearanceChanged();
		}
	}

	[DevCoBrowsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override Rectangle Bounds
	{
		get
		{
			return base.Bounds;
		}
		set
		{
			Point _066A = new Point(value.X - m_Rect.X, value.Y - m_Rect.Y);
			base.Bounds = value;
			if (ᅦ != 0 || (ᅱ != null && ᅱ.RightToLeft))
			{
				LayoutItems(value);
			}
			else if (!_066A.IsEmpty)
			{
				ᆀ(_066A);
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override bool Expanded
	{
		get
		{
			return m_Expanded;
		}
		set
		{
			base.Expanded = value;
			if (!value)
			{
				BaseItem.CollapseSubItems(this);
			}
		}
	}

	[Browsable(false)]
	public bool SystemContainer => ෆ;

	[DevCoBrowsable(true)]
	[Description("Gets or sets the accessible role of the item.")]
	[Category("Accessibility")]
	[Browsable(true)]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	public override AccessibleRole AccessibleRole
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return base.AccessibleRole;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			base.AccessibleRole = value;
		}
	}

	[DefaultValue(false)]
	[Description("Indicates whether block elements inside of container (affects span or div for example) when aligned center or right will reserve the space to the left.")]
	[Category("Layout")]
	public bool ReserveLeftSpace
	{
		get
		{
			return __0CE3;
		}
		set
		{
			__0CE3 = value;
		}
	}

	[Description("Indicates whether all items are equally sized based on the size of the largest item in the list.")]
	[Category("Layout")]
	[DefaultValue(false)]
	public virtual bool EqualItemSize
	{
		get
		{
			return __0CE4;
		}
		set
		{
			__0CE4 = value;
			NeedRecalcSize = true;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(true)]
	[Category("Style")]
	[DevCoBrowsable(true)]
	[Description("Gets or sets title style.")]
	public ElementStyle TitleStyle => _ᅳ;

	[Description("Gets or sets title mouse over style.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Style")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	public ElementStyle TitleMouseOverStyle => _ᅴ;

	private bool ᅛ
	{
		get
		{
			if (_ᅵ)
			{
				return !string.IsNullOrEmpty(_ᅶ);
			}
			return false;
		}
	}

	[Description("Indicates whether TitleText is visible if set.")]
	[DefaultValue(true)]
	[Category("Appearance")]
	public bool TitleVisible
	{
		get
		{
			return _ᅵ;
		}
		set
		{
			if (value != _ᅵ)
			{
				bool ᅵ = _ᅵ;
				_ᅵ = value;
				OnTitleVisibleChanged(ᅵ, value);
			}
		}
	}

	[Category("Appearance")]
	[Description("Indicates title text for the container.")]
	[DefaultValue("")]
	public string TitleText
	{
		get
		{
			return _ᅶ;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			if (value != _ᅶ)
			{
				string ᅶ = _ᅶ;
				_ᅶ = value;
				OnTitleTextChanged(ᅶ, value);
			}
		}
	}

	[DefaultValue(eTitlePosition.Top)]
	[Category("Appearance")]
	[Description("Indicates the position of title text.")]
	public eTitlePosition TitlePosition
	{
		get
		{
			return _ᅷ;
		}
		set
		{
			if (value != _ᅷ)
			{
				eTitlePosition ᅷ = _ᅷ;
				_ᅷ = value;
				OnTitlePositionChanged(ᅷ, value);
			}
		}
	}

	[Description("Indicates the Key Tips access key or keys for the item when on Ribbon Control or Ribbon Bar.")]
	[DefaultValue("")]
	[Browsable(false)]
	[Category("Appearance")]
	public override string KeyTips
	{
		get
		{
			return base.KeyTips;
		}
		set
		{
			base.KeyTips = value;
		}
	}

	[DevCoBrowsable(false)]
	[DefaultValue(true)]
	[Description("Indicates whether the item will auto-collapse (fold) when clicked.")]
	[Browsable(false)]
	[Category("Behavior")]
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

	[Category("Behavior")]
	[DefaultValue(true)]
	[Description("Indicates whether item can be customized by user.")]
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

	[DevCoBrowsable(false)]
	[Category("Design")]
	[Description("Indicates item category used to group similar items at design-time.")]
	[DefaultValue("")]
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

	[DevCoBrowsable(false)]
	[DefaultValue(false)]
	[Browsable(false)]
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

	[DefaultValue(600)]
	[Category("Behavior")]
	[Browsable(false)]
	[Description("Gets or sets the auto-repeat interval for the click event when mouse button is kept pressed over the item.")]
	[DevCoBrowsable(false)]
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

	[DevCoBrowsable(false)]
	[DefaultValue(null)]
	[Description("Specifies the mouse cursor displayed when mouse is over the item.")]
	[Category("Appearance")]
	[Browsable(false)]
	public override Cursor Cursor
	{
		get
		{
			return base.Cursor;
		}
		set
		{
			base.Cursor = value;
		}
	}

	[DevCoBrowsable(false)]
	[Category("Design")]
	[Browsable(false)]
	[Description("Indicates description of the item that is displayed during design.")]
	[DefaultValue("")]
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
	[Category("Behavior")]
	[Description("Indicates whether is item enabled.")]
	[DefaultValue(true)]
	[DevCoBrowsable(false)]
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

	[Category("Behavior")]
	[Description("Indicates whether certain global properties are propagated to all items with the same name when changed.")]
	[DevCoBrowsable(false)]
	[DefaultValue(true)]
	[Browsable(false)]
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

	[Description("Determines alignment of the item inside the container.")]
	[Category("Appearance")]
	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DefaultValue(eItemAlignment.Near)]
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
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[DevCoBrowsable(false)]
	[Browsable(false)]
	[Category("Design")]
	[Editor("DevComponents.DotNetBar.Design.ShortcutsDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[TypeConverter("DevComponents.DotNetBar.Design.ShortcutsConverter, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
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
	[Description("Determines whether sub-items are displayed.")]
	[DevCoBrowsable(false)]
	[DefaultValue(true)]
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
	[DevCoBrowsable(false)]
	[Browsable(false)]
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
	[Category("Appearance")]
	[DevCoBrowsable(false)]
	[DefaultValue(false)]
	[Description("Specifies whether item is drawn using Themes when running on OS that supports themes like Windows XP.")]
	public override bool ThemeAware
	{
		get
		{
			return base.ThemeAware;
		}
		set
		{
			base.ThemeAware = value;
		}
	}

	[DevCoBrowsable(false)]
	[Localizable(true)]
	[Description("Indicates the text that is displayed when mouse hovers over the item.")]
	[Category("Appearance")]
	[DefaultValue("")]
	[Browsable(false)]
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

	[Description("Indicates whether container responds to keyboard presses and changes the active/mouse over item.")]
	[DefaultValue(true)]
	[Category("Behavior")]
	public bool KeyboardNavigationEnabled
	{
		get
		{
			return _ᅸ;
		}
		set
		{
			_ᅸ = value;
		}
	}

	bool IBlockExtended.ᅜ => false;

	bool IBlockExtended.ᅝ => false;

	bool IBlockExtended.ᅞ => true;

	[Browsable(false)]
	public bool IsBlockContainer => true;

	Rectangle IBlock.ᅟ
	{
		get
		{
			return Bounds;
		}
		set
		{
			Bounds = value;
		}
	}

	bool IBlock.ᅠ
	{
		get
		{
			return Visible;
		}
		set
		{
			Visible = value;
		}
	}

	public ItemContainer()
	{
		m_IsContainer = true;
		AutoCollapseOnClick = true;
		AccessibleRole = (AccessibleRole)20;
		_0599.StyleChanged += ᅻ;
		_ᅳ = new ElementStyle();
		_ᅳ.StyleChanged += ᆇ;
		_ᅴ = new ElementStyle();
		_ᅴ.StyleChanged += ᆇ;
	}

	protected override void Dispose(bool disposing)
	{
		_0599.StyleChanged -= ᅻ;
		_0599.Dispose();
		_ᅳ.StyleChanged -= ᆇ;
		_ᅳ.Dispose();
		base.Dispose(disposing);
	}

	private void ᅹ(object _٠, EventArgs ؿ)
	{
		NeedRecalcSize = true;
		OnAppearanceChanged();
	}

	private void ᅺ(object _٠, EventArgs ؿ)
	{
		NeedRecalcSize = true;
		OnAppearanceChanged();
	}

	private void ᅻ(object _٠, EventArgs ؿ)
	{
		OnAppearanceChanged();
	}

	private bool ShouldSerializeMinimumSize()
	{
		return !Ⴓ.IsEmpty;
	}

	protected virtual Rectangle GetClipRectangle()
	{
		Rectangle displayRectangle = DisplayRectangle;
		bool ല = false;
		ElementStyle elementStyle = ElementStyleDisplay.റ(_0599, out ല);
		displayRectangle.X += ᝈ.ᝐ(elementStyle);
		displayRectangle.Width -= ᝈ.ᝍ(elementStyle);
		displayRectangle.Y += ᝈ._1753(elementStyle);
		displayRectangle.Height -= ᝈ.ᝏ(elementStyle);
		if (ല)
		{
			elementStyle.Dispose();
		}
		return displayRectangle;
	}

	internal void ᅼ(ItemPaintArgs _065C)
	{
		PaintBackground(_065C);
	}

	protected virtual void PaintBackground(ItemPaintArgs p)
	{
		_0599.SetColorScheme(p.Colors);
		Graphics graphics = p.Graphics;
		BaseRenderer ໝ = p.ໝ;
		if (ໝ != null)
		{
			ItemContainerRendererEventArgs itemContainerRendererEventArgs = new ItemContainerRendererEventArgs(p.Graphics, this);
			itemContainerRendererEventArgs.က = p;
			ໝ.DrawItemContainer(itemContainerRendererEventArgs);
		}
		else
		{
			ElementStyleDisplay.Paint(new ElementStyleDisplayInfo(_0599, graphics, DisplayRectangle));
		}
	}

	private void ᅽ(ItemPaintArgs _065C)
	{
		_ᅪ = Rectangle.Empty;
		if (!ᅛ)
		{
			return;
		}
		Rectangle rectangle = new Rectangle(DisplayRectangle.X, DisplayRectangle.Y, DisplayRectangle.Width, _ᅭ.Height);
		if (_ᅷ == eTitlePosition.Bottom)
		{
			rectangle.Y = DisplayRectangle.Bottom - _ᅭ.Height;
		}
		Graphics graphics = _065C.Graphics;
		Font font = _065C.Font;
		ElementStyle activeTitleStyle = GetActiveTitleStyle();
		if (activeTitleStyle.Custom)
		{
			bool ല = false;
			ElementStyle elementStyle = ElementStyleDisplay.റ(activeTitleStyle, out ല);
			ElementStyleDisplayInfo e = new ElementStyleDisplayInfo(elementStyle, graphics, rectangle);
			ElementStyleDisplay.Paint(e);
			ElementStyleDisplay.PaintText(e, _ᅶ, font);
			if (ല)
			{
				elementStyle.Dispose();
			}
		}
		else
		{
			TextDrawing.DrawString(graphics, _ᅶ, font, SystemColors.ControlText, rectangle, eTextFormat.Default);
		}
		_ᅪ = rectangle;
	}

	protected virtual ElementStyle GetActiveTitleStyle()
	{
		if (_ᅩ && _ᅴ.Custom)
		{
			return _ᅴ;
		}
		return _ᅳ;
	}

	public override void Paint(ItemPaintArgs p)
	{
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Expected O, but got Unknown
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		Graphics graphics = p.Graphics;
		Region val = null;
		bool flag = false;
		PaintBackground(p);
		if (ᅘ)
		{
			Rectangle displayRectangle = DisplayRectangle;
			displayRectangle.Inflate(-1, -1);
			val = graphics.Clip;
			graphics.SetClip(displayRectangle, (CombineMode)0);
			flag = true;
		}
		else
		{
			Rectangle clipRectangle = GetClipRectangle();
			val = graphics.Clip;
			graphics.SetClip(clipRectangle, (CombineMode)1);
			flag = true;
		}
		ᅽ(p);
		_249A _249A2 = ᆆ();
		_249A2._139A(this, p);
		if (flag)
		{
			if (val != null)
			{
				graphics.Clip = val;
			}
			else
			{
				graphics.ResetClip();
			}
		}
		if (val != null)
		{
			val.Dispose();
		}
		if (DesignMode && !SystemContainer && p.DesignerSelection)
		{
			Rectangle displayRectangle2 = DisplayRectangle;
			Pen val2 = null;
			val2 = ((!Focused) ? new Pen(Color.FromArgb(80, Color.Black), 1f) : new Pen(Color.FromArgb(190, Color.Navy), 1f));
			val2.DashStyle = (DashStyle)2;
			DisplayHelp.DrawRoundedRectangle(graphics, val2, displayRectangle2, 3);
			val2.Dispose();
			val2 = null;
			Image val3 = (Image)(object)BarFunctions.ই("SystemImages.AddMoreItemsContainer.png");
			if (Parent is ItemContainer && !((ItemContainer)Parent).SystemContainer)
			{
				ItemContainer itemContainer = Parent as ItemContainer;
				while (itemContainer != null && !itemContainer.SystemContainer)
				{
					if (new Rectangle(itemContainer.DisplayRectangle.Location, val3.Size).Contains(DisplayRectangle.Location))
					{
						if (displayRectangle2.X + val3.Width + 1 > DisplayRectangle.Right)
						{
							displayRectangle2.X = DisplayRectangle.X;
							if (displayRectangle2.Y + val3.Height >= DisplayRectangle.Bottom)
							{
								break;
							}
							displayRectangle2.Y += val3.Height + 1;
						}
						displayRectangle2.X += val3.Width + 1;
					}
					itemContainer = itemContainer.Parent as ItemContainer;
				}
			}
			ᅤ = new Rectangle(displayRectangle2.X + 1, displayRectangle2.Y + 1, val3.Width, val3.Height);
			graphics.DrawImageUnscaled(val3, ᅤ.Location);
		}
		DrawInsertMarker(p.Graphics);
	}

	protected virtual Size GetEmptyDesignTimeSize()
	{
		Size result = ᅢ;
		if (ᅡ == eOrientation.Horizontal)
		{
			result.Width += 12;
		}
		else
		{
			result.Height += 12;
		}
		return result;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeFixedSize()
	{
		return !_ᅫ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetFixedSize()
	{
		TypeDescriptor.GetProperties(this)["FixedSize"].SetValue(this, Size.Empty);
	}

	protected virtual void OnFixedSizeChanged(Size oldValue, Size newValue)
	{
		if (oldValue.Width != newValue.Width || oldValue.Height != newValue.Height)
		{
			NeedRecalcSize = true;
			Refresh();
		}
	}

	public override void RecalcSize()
	{
		if (SuspendLayout)
		{
			return;
		}
		if (SubItems.Count == 0 && !ᅛ)
		{
			if (DesignMode && !SystemContainer)
			{
				m_Rect.Size = GetEmptyDesignTimeSize();
			}
			else if (!_ᅫ.IsEmpty)
			{
				m_Rect = new Rectangle(m_Rect.X, m_Rect.Y, Dpi.Width((_ᅫ.Width > 0) ? _ᅫ.Width : Ⴓ.Width), Dpi.Height((_ᅫ.Height > 0) ? _ᅫ.Height : Ⴓ.Height));
			}
			else if (Ⴓ.IsEmpty)
			{
				m_Rect = Rectangle.Empty;
			}
			else
			{
				m_Rect = new Rectangle(m_Rect.X, m_Rect.Y, Dpi.Width(Ⴓ.Width), Dpi.Height(Ⴓ.Height));
			}
			base.RecalcSize();
		}
		else
		{
			m_Rect = LayoutItems(m_Rect);
			base.RecalcSize();
		}
	}

	private void ᅾ(Rectangle ശ)
	{
		_ᅭ = Size.Empty;
		if (!ᅛ)
		{
			return;
		}
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val == null)
		{
			return;
		}
		ElementStyle activeTitleStyle = GetActiveTitleStyle();
		if (activeTitleStyle.Custom)
		{
			bool ല = false;
			ElementStyle elementStyle = ElementStyleDisplay.റ(activeTitleStyle, out ല);
			Font font = val.Font;
			if (elementStyle.Font != null)
			{
				font = elementStyle.Font;
			}
			Size size = Size.Empty;
			Graphics val2 = BarFunctions.CreateGraphics(val);
			try
			{
				size = TextDrawing.MeasureString(val2, _ᅶ, font, ശ.Width - ᝈ.ᝍ(elementStyle), elementStyle.ݽ);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			if (ശ.Width > 0 && _ᅬ)
			{
				_ᅭ.Width = ശ.Width;
			}
			else
			{
				_ᅭ.Width = size.Width;
			}
			_ᅭ.Height = size.Height + ᝈ.ᝏ(elementStyle);
			if (ല)
			{
				elementStyle.Dispose();
			}
			return;
		}
		Font font2 = val.Font;
		Graphics val3 = BarFunctions.CreateGraphics(val);
		try
		{
			Size size2 = TextDrawing.MeasureString(val3, _ᅶ, font2, ശ.Width);
			if (ശ.Width > 0 && _ᅬ)
			{
				_ᅭ.Width = ശ.Width;
			}
			else
			{
				_ᅭ.Width = size2.Width;
			}
			_ᅭ.Height = size2.Height + 1;
		}
		finally
		{
			((IDisposable)val3)?.Dispose();
		}
	}

	protected virtual Rectangle LayoutItems(Rectangle bounds)
	{
		IContentLayout contentLayout = GetContentLayout();
		BlockLayoutManager blockLayout = ᆅ();
		ᅾ(bounds);
		Rectangle layoutRectangle = GetLayoutRectangle(bounds);
		BaseItem[] array = new BaseItem[SubItems.Count];
		if (_ᅯ)
		{
			for (int i = 0; i < SubItems.Count; i++)
			{
				if (SubItems[i] is ButtonItem buttonItem)
				{
					buttonItem._ງ = true;
					buttonItem.WidthInternal = layoutRectangle.Width;
				}
				array[i] = SubItems[i];
			}
		}
		else
		{
			SubItems.CopyTo(array, 0);
		}
		bool ല = false;
		ElementStyle elementStyle = ElementStyleDisplay.റ(_0599, out ല);
		layoutRectangle = contentLayout.Layout(layoutRectangle, array, blockLayout);
		if (!Displayed)
		{
			for (int j = 0; j < SubItems.Count; j++)
			{
				SubItems[j].Displayed = false;
			}
		}
		if (!SystemContainer)
		{
			layoutRectangle = (_ᅫ.IsEmpty ? new Rectangle(bounds.Location, layoutRectangle.Size) : new Rectangle(bounds.X, bounds.Y, (_ᅫ.Width > 0) ? Dpi.Width(_ᅫ.Width) : layoutRectangle.Width, (_ᅫ.Height > 0) ? Dpi.Height(_ᅫ.Height) : layoutRectangle.Height));
		}
		layoutRectangle.Height += _ᅭ.Height;
		if (_ᅭ.Width > layoutRectangle.Width)
		{
			layoutRectangle.Width = _ᅭ.Width;
		}
		_ᅮ = layoutRectangle;
		Size size = Dpi.Size(Ⴓ);
		if (size.Width > 0 && size.Width > layoutRectangle.Width)
		{
			layoutRectangle.Width = size.Width;
		}
		else
		{
			layoutRectangle.Width += ᝈ.ᝍ(elementStyle);
		}
		if (size.Height > 0 && size.Height > layoutRectangle.Height)
		{
			layoutRectangle.Height = size.Height;
		}
		else
		{
			layoutRectangle.Height += ᝈ.ᝏ(elementStyle);
		}
		if (ല)
		{
			elementStyle.Dispose();
		}
		return layoutRectangle;
	}

	protected virtual Rectangle GetLayoutRectangle(Rectangle bounds)
	{
		if (bounds.Width == 0)
		{
			bounds.Width = 16;
		}
		if (bounds.Height == 0)
		{
			bounds.Height = 16;
		}
		if (_ᅫ.Width > 0)
		{
			bounds.Width = Dpi.Width(_ᅫ.Width);
		}
		else if (Ⴓ.Width > 0)
		{
			bounds.Width = Dpi.Width(Ⴓ.Width);
		}
		if (_ᅫ.Height > 0)
		{
			bounds.Height = Dpi.Height(_ᅫ.Height);
		}
		else if (Ⴓ.Height > 0)
		{
			bounds.Height = Dpi.Height(Ⴓ.Height);
		}
		bool ല = false;
		ElementStyle elementStyle = ElementStyleDisplay.റ(_0599, out ല);
		bounds.X += ᝈ.ᝐ(elementStyle);
		bounds.Y += ᝈ._1753(elementStyle);
		bounds.Width -= ᝈ.ᝍ(elementStyle);
		bounds.Height -= ᝈ.ᝏ(elementStyle);
		if (ല)
		{
			elementStyle.Dispose();
		}
		if (!_ᅭ.IsEmpty)
		{
			if (_ᅷ == eTitlePosition.Top)
			{
				bounds.Y += _ᅭ.Height;
				bounds.Height -= _ᅭ.Height;
			}
			else if (_ᅷ == eTitlePosition.Bottom)
			{
				bounds.Height -= _ᅭ.Height;
			}
		}
		return bounds;
	}

	protected override void OnExternalSizeChange()
	{
		base.OnExternalSizeChange();
		if ((ᅧ != 0 || IsRightToLeft) && SubItems.Count > 0 && !SuspendLayout)
		{
			LayoutItems(m_Rect);
		}
	}

	protected internal override void OnAfterItemRemoved(BaseItem item, int itemIndex)
	{
		if (item is ButtonItem && _ᅯ)
		{
			((ButtonItem)item)._ງ = false;
		}
		base.OnAfterItemRemoved(item, itemIndex);
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void InternalMouseDown(MouseEventArgs objArg)
	{
		if (DesignMode && !SystemContainer)
		{
			BaseItem baseItem = ItemAtLocation(objArg.X, objArg.Y);
			if (baseItem == this && (!baseItem.DesignMode || baseItem.DesignTimeVisible) && GetOwner() is IOwner owner)
			{
				owner.SetFocusItem(this);
				return;
			}
		}
		else if (!DisplayRectangle.Contains(objArg.X, objArg.Y))
		{
			if (DesignMode)
			{
				if (GetOwner() is IOwner owner2)
				{
					owner2.SetFocusItem(null);
				}
				LeaveHotSubItem(null);
			}
			return;
		}
		base.InternalMouseDown(objArg);
	}

	public override void InternalMouseMove(MouseEventArgs objArg)
	{
		if (!DisplayRectangle.Contains(objArg.X, objArg.Y))
		{
			LeaveHotSubItem(null);
			if (!SystemContainer)
			{
				return;
			}
		}
		else if (TitleVisible && _ᅪ.Contains(objArg.Location))
		{
			LeaveHotSubItem(null);
			TitleMouseOver = true;
		}
		else
		{
			TitleMouseOver = false;
		}
		base.InternalMouseMove(objArg);
	}

	public override void InternalMouseLeave()
	{
		TitleMouseOver = false;
		base.InternalMouseLeave();
	}

	public override void InternalMouseUp(MouseEventArgs objArg)
	{
		if (DisplayRectangle.Contains(objArg.X, objArg.Y))
		{
			base.InternalMouseUp(objArg);
		}
	}

	public override BaseItem ItemAtLocation(int x, int y)
	{
		if (DesignMode && !SystemContainer && !ᅤ.IsEmpty && ᅤ.Contains(x, y))
		{
			return this;
		}
		if (!Visible || !DisplayRectangle.Contains(x, y))
		{
			return null;
		}
		if (m_SubItems != null)
		{
			foreach (BaseItem subItem in m_SubItems)
			{
				if (subItem.IsContainer)
				{
					BaseItem baseItem2 = subItem.ItemAtLocation(x, y);
					if (baseItem2 != null)
					{
						return baseItem2;
					}
				}
				else if ((subItem.Visible || base.IsOnCustomizeMenu) && subItem.Displayed && subItem.DisplayRectangle.Contains(x, y) && (!DesignMode || DisplayRectangle.IntersectsWith(subItem.DisplayRectangle)))
				{
					return subItem;
				}
			}
		}
		if (DisplayRectangle.Contains(x, y) && !SystemContainer)
		{
			return this;
		}
		return null;
	}

	private void ᅿ()
	{
		NeedRecalcSize = true;
		OnAppearanceChanged();
	}

	private void Ⴞ()
	{
		NeedRecalcSize = true;
		if (ᅱ != null)
		{
			ᅱ.ContentOrientation = ((LayoutOrientation != 0) ? eContentOrientation.Vertical : eContentOrientation.Horizontal);
		}
		OnAppearanceChanged();
	}

	internal void ᆀ(Point _066A)
	{
		if (_ᅰ || _066A.IsEmpty)
		{
			return;
		}
		try
		{
			_ᅰ = true;
			BaseItem[] array = new BaseItem[SubItems.Count];
			SubItems.CopyTo(array, 0);
			BaseItem[] array2 = array;
			foreach (IBlock block in array2)
			{
				Rectangle bounds = block.Bounds;
				bounds.Offset(_066A);
				block.Bounds = bounds;
			}
		}
		finally
		{
			_ᅰ = false;
		}
	}

	protected override void OnTopLocationChanged(int oldValue)
	{
		base.OnTopLocationChanged(oldValue);
		Point _066A = new Point(0, TopInternal - oldValue);
		ᆀ(_066A);
	}

	protected override void OnLeftLocationChanged(int oldValue)
	{
		base.OnLeftLocationChanged(oldValue);
		Point _066A = new Point(LeftInternal - oldValue, 0);
		ᆀ(_066A);
	}

	public override BaseItem Copy()
	{
		ItemContainer itemContainer = new ItemContainer();
		CopyToItem(itemContainer);
		return itemContainer;
	}

	protected override void CopyToItem(BaseItem c)
	{
		ItemContainer itemContainer = c as ItemContainer;
		itemContainer.BackgroundStyle.ApplyStyle(_0599);
		itemContainer.HorizontalItemAlignment = HorizontalItemAlignment;
		itemContainer.ItemSpacing = ItemSpacing;
		itemContainer.LayoutOrientation = LayoutOrientation;
		itemContainer.MinimumSize = MinimumSize;
		itemContainer.MultiLine = MultiLine;
		itemContainer.ResizeItemsToFit = ResizeItemsToFit;
		itemContainer.VerticalItemAlignment = VerticalItemAlignment;
		base.CopyToItem(itemContainer);
	}

	protected internal override void OnSubItemExpandChange(BaseItem item)
	{
		base.OnSubItemExpandChange(item);
		if (item.Expanded)
		{
			Expanded = true;
		}
	}

	protected internal override void OnSubItemsClear()
	{
		RefreshImageSize();
		base.OnSubItemsClear();
	}

	protected override void OnDisplayedChanged()
	{
		base.OnDisplayedChanged();
		foreach (BaseItem subItem in SubItems)
		{
			if (subItem.Visible)
			{
				subItem.Displayed = Displayed;
			}
		}
		if (!Displayed)
		{
			InternalMouseLeave();
		}
	}

	internal void ᆁ(bool _0BCF)
	{
		ෆ = _0BCF;
	}

	protected virtual IContentLayout GetContentLayout()
	{
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Invalid comparison between Unknown and I4
		if (ᅱ == null)
		{
			ᅱ = new SerialContentLayoutManager();
			ᅱ.BlockSpacing = 1;
			ᅱ.FitContainerOversize = false;
			ᅱ.FitContainer = false;
			ᅱ.BeforeNewBlockLayout += ᆂ;
		}
		ᅱ.EqualItemSize = __0CE4;
		ᅱ.ReserveLeftSpace = __0CE3;
		ᅱ.MultiLine = ೠ;
		ᅱ.EvenHeight = ᅥ;
		if (Ⴓ.Width > 0 && ᅡ == eOrientation.Vertical)
		{
			ᅱ.VerticalFitContainerWidth = true;
		}
		else
		{
			ᅱ.VerticalFitContainerWidth = false;
		}
		if (Ⴓ.Height > 0 && ᅡ == eOrientation.Horizontal)
		{
			ᅱ.HorizontalFitContainerHeight = true;
		}
		else
		{
			ᅱ.HorizontalFitContainerHeight = false;
		}
		ᅱ.BlockLineAlignment = eContentVerticalAlignment.Top;
		ᅱ.ContentAlignment = ᆃ();
		ᅱ.ContentVerticalAlignment = ᆄ();
		ᅱ.ContentOrientation = ((LayoutOrientation != 0) ? eContentOrientation.Vertical : eContentOrientation.Horizontal);
		ᅱ.RightToLeft = false;
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val != null && (int)val.RightToLeft == 1)
		{
			ᅱ.RightToLeft = true;
		}
		else
		{
			ᅱ.RightToLeft = false;
		}
		ᅱ.BlockSpacing = Dpi.Width(ᅣ - (ᅘ ? 1 : 0));
		return ᅱ;
	}

	private void ᆂ(object _٠, LayoutManagerLayoutEventArgs ؿ)
	{
		if (ؿ.Block is BaseItem baseItem && ᅘ && !(baseItem is ItemContainer) && ؿ.BlockVisibleIndex > 0)
		{
			if (ᅡ == eOrientation.Horizontal)
			{
				ؿ.CurrentPosition.X += ᅨ;
			}
			else
			{
				ؿ.CurrentPosition.Y += ᅨ;
			}
		}
	}

	private eContentAlignment ᆃ()
	{
		if (ᅦ == eHorizontalItemsAlignment.Left)
		{
			return eContentAlignment.Left;
		}
		if (ᅦ == eHorizontalItemsAlignment.Center)
		{
			return eContentAlignment.Center;
		}
		return eContentAlignment.Right;
	}

	private eContentVerticalAlignment ᆄ()
	{
		if (ᅧ == eVerticalItemsAlignment.Top)
		{
			return eContentVerticalAlignment.Top;
		}
		if (ᅧ == eVerticalItemsAlignment.Middle)
		{
			return eContentVerticalAlignment.Middle;
		}
		if (ᅧ == eVerticalItemsAlignment.Bottom)
		{
			return eContentVerticalAlignment.Bottom;
		}
		return eContentVerticalAlignment.Top;
	}

	private BlockLayoutManager ᆅ()
	{
		return new ItemBlockLayoutManager();
	}

	internal _249A ᆆ()
	{
		if (ᅲ == null)
		{
			ᅲ = new _249A();
		}
		return ᅲ;
	}

	private void ᆇ(object _٠, EventArgs ؿ)
	{
		OnAppearanceChanged();
	}

	private void ᆈ(object _٠, EventArgs ؿ)
	{
		OnAppearanceChanged();
	}

	protected virtual void OnTitleVisibleChanged(bool oldValue, bool newValue)
	{
		NeedRecalcSize = true;
		OnAppearanceChanged();
	}

	protected virtual void OnTitleTextChanged(string oldValue, string newValue)
	{
		NeedRecalcSize = true;
		OnAppearanceChanged();
	}

	protected virtual void OnTitlePositionChanged(eTitlePosition oldValue, eTitlePosition newValue)
	{
		NeedRecalcSize = true;
		OnAppearanceChanged();
	}

	protected virtual InsertPosition GetContainerInsertPosition(Point pScreen, BaseItem dragItem)
	{
		return DesignTimeProviderContainer.GetInsertPosition(this, pScreen, dragItem);
	}

	private InsertPosition ᆉ(Point ॺ, BaseItem ᆊ)
	{
		return GetContainerInsertPosition(ॺ, ᆊ);
	}

	InsertPosition IDesignTimeProvider.GetInsertPosition(Point ॺ, BaseItem ᆊ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᆉ
		return this.ᆉ(ॺ, ᆊ);
	}

	private void ᆋ(int ᆌ, bool ᆍ)
	{
		DesignTimeProviderContainer.DrawReversibleMarker(this, ᆌ, ᆍ);
	}

	void IDesignTimeProvider.DrawReversibleMarker(int ᆌ, bool ᆍ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᆋ
		this.ᆋ(ᆌ, ᆍ);
	}

	private void ᆎ(BaseItem ਤ, int ᆌ, bool ᆍ)
	{
		DesignTimeProviderContainer.InsertItemAt(this, ਤ, ᆌ, ᆍ);
	}

	void IDesignTimeProvider.InsertItemAt(BaseItem ਤ, int ᆌ, bool ᆍ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᆎ
		this.ᆎ(ਤ, ᆌ, ᆍ);
	}

	internal bool _1030()
	{
		foreach (BaseItem subItem in SubItems)
		{
			if (!subItem.Visible || !subItem.Displayed)
			{
				continue;
			}
			if (subItem is ItemContainer)
			{
				if (((ItemContainer)subItem)._1030())
				{
					SetHotSubItem(null);
					base.HotSubItem = subItem;
					return true;
				}
			}
			else if (_1033(subItem))
			{
				SetHotSubItem(subItem);
				return true;
			}
		}
		return false;
	}

	private bool _1033(BaseItem _06EC)
	{
		if (_06EC is LabelItem)
		{
			return false;
		}
		return true;
	}

	public void SetHotSubItem(BaseItem item)
	{
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Expected O, but got Unknown
		if (item == m_HotSubItem)
		{
			return;
		}
		if (m_HotSubItem != null)
		{
			m_HotSubItem.InternalMouseLeave();
			if (m_AutoExpand && m_HotSubItem.Expanded)
			{
				m_HotSubItem.Expanded = false;
			}
			base.HotSubItem = null;
		}
		if (item != null)
		{
			base.HotSubItem = item;
			m_HotSubItem.InternalMouseEnter();
			m_HotSubItem.InternalMouseMove(new MouseEventArgs((MouseButtons)0, 0, m_HotSubItem.LeftInternal + 1, m_HotSubItem.TopInternal + 1, 0));
			if (ContainerControl is IScrollableItemControl scrollableItemControl)
			{
				scrollableItemControl.KeyboardItemSelected(m_HotSubItem);
			}
		}
	}

	public override void InternalKeyDown(KeyEventArgs objArg)
	{
		base.InternalKeyDown(objArg);
		if (_ᅸ)
		{
			_2189.ᡟ(this, objArg);
		}
	}
}
