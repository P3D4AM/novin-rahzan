using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace DevComponents.Editors.DateTimeAdv;

public class CalendarBase : ImageItem, IDesignTimeProvider
{
	private ElementStyle _ᠩ = new ElementStyle();

	private _249A __25A7;

	[Browsable(true)]
	[Description("Gets or sets container background style.")]
	[Category("Style")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ElementStyle BackgroundStyle => _ᠩ;

	[Category("Design")]
	[Description("Indicates the name used to identify item.")]
	[DefaultValue("")]
	public override string Name
	{
		get
		{
			return base.Name;
		}
		set
		{
			base.Name = value;
		}
	}

	[Description("Gets or sets the accessible role of the item.")]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Category("Accessibility")]
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

	[Description("Indicates the Key Tips access key or keys for the item when on Ribbon Control or Ribbon Bar.")]
	[Category("Appearance")]
	[DefaultValue("")]
	[Browsable(false)]
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
	[Browsable(false)]
	[Category("Behavior")]
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

	[Description("Indicates whether item can be customized by user.")]
	[Category("Behavior")]
	[Browsable(false)]
	[DefaultValue(true)]
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
	[DefaultValue("")]
	[Category("Design")]
	[Browsable(false)]
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

	[DevCoBrowsable(false)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Browsable(false)]
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

	[Description("Gets or sets the auto-repeat interval for the click event when mouse button is kept pressed over the item.")]
	[Category("Behavior")]
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

	[Description("Specifies the mouse cursor displayed when mouse is over the item.")]
	[DevCoBrowsable(false)]
	[Browsable(false)]
	[Category("Appearance")]
	[DefaultValue(null)]
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

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[Category("Design")]
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

	[DefaultValue(true)]
	[Browsable(false)]
	[DevCoBrowsable(false)]
	[Description("Indicates whether certain global properties are propagated to all items with the same name when changed.")]
	[Category("Behavior")]
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

	[DevCoBrowsable(false)]
	[DefaultValue(DevComponents.DotNetBar.eItemAlignment.Near)]
	[Browsable(false)]
	[Description("Determines alignment of the item inside the container.")]
	[Category("Appearance")]
	public override DevComponents.DotNetBar.eItemAlignment ItemAlignment
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

	[DevCoBrowsable(false)]
	[Category("Design")]
	[Browsable(false)]
	[TypeConverter("DevComponents.DotNetBar.Design.ShortcutsConverter, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
	[Editor("DevComponents.DotNetBar.Design.ShortcutsDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Indicates list of shortcut keys for this item.")]
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
	[DevCoBrowsable(false)]
	[DefaultValue(true)]
	[Browsable(false)]
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

	[DevCoBrowsable(false)]
	[Category("Appearance")]
	[DefaultValue(false)]
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

	[DefaultValue(false)]
	[Description("Specifies whether item is drawn using Themes when running on OS that supports themes like Windows XP.")]
	[DevCoBrowsable(false)]
	[Browsable(false)]
	[Category("Appearance")]
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

	[Localizable(true)]
	[Browsable(false)]
	[DefaultValue("")]
	[Description("Indicates the text that is displayed when mouse hovers over the item.")]
	[Category("Appearance")]
	[DevCoBrowsable(false)]
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

	[Browsable(false)]
	public override string Text
	{
		get
		{
			return base.Text;
		}
		set
		{
			base.Text = value;
		}
	}

	public CalendarBase()
	{
		m_IsContainer = true;
		AutoCollapseOnClick = true;
		AccessibleRole = (AccessibleRole)20;
		_ᠩ.StyleChanged += ᅻ;
	}

	protected override void Dispose(bool disposing)
	{
		_ᠩ.StyleChanged -= ᅻ;
		_ᠩ.Dispose();
		base.Dispose(disposing);
	}

	public override void Paint(ItemPaintArgs p)
	{
		Graphics graphics = p.Graphics;
		Region val = null;
		bool flag = false;
		PaintBackground(p);
		Rectangle clipRectangle = GetClipRectangle();
		val = graphics.Clip;
		graphics.SetClip(clipRectangle, (CombineMode)1);
		flag = true;
		_249A _249A = ᆆ();
		_249A._139A(this, p);
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
		DrawInsertMarker(p.Graphics);
	}

	protected virtual Rectangle GetClipRectangle()
	{
		Rectangle displayRectangle = DisplayRectangle;
		bool ല = false;
		ElementStyle elementStyle = ElementStyleDisplay.റ(_ᠩ, out ല);
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

	protected virtual void PaintBackground(ItemPaintArgs p)
	{
		_ᠩ.SetColorScheme(p.Colors);
		bool disposeStyle = false;
		ElementStyle renderBackgroundStyle = GetRenderBackgroundStyle(out disposeStyle);
		Graphics graphics = p.Graphics;
		ElementStyleDisplay.Paint(new ElementStyleDisplayInfo(renderBackgroundStyle, graphics, DisplayRectangle));
		if (disposeStyle)
		{
			renderBackgroundStyle.Dispose();
		}
	}

	protected virtual ElementStyle GetRenderBackgroundStyle(out bool disposeStyle)
	{
		disposeStyle = false;
		return ElementStyleDisplay.റ(_ᠩ, out disposeStyle);
	}

	internal _249A ᆆ()
	{
		if (__25A7 == null)
		{
			__25A7 = new _249A();
		}
		return __25A7;
	}

	private void ᅻ(object _٠, EventArgs ؿ)
	{
		OnAppearanceChanged();
	}

	public override BaseItem Copy()
	{
		CalendarBase calendarBase = new CalendarBase();
		CopyToItem(calendarBase);
		return calendarBase;
	}

	protected override void CopyToItem(BaseItem c)
	{
		CalendarBase calendarBase = c as CalendarBase;
		calendarBase.BackgroundStyle.ApplyStyle(_ᠩ);
		base.CopyToItem(calendarBase);
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
}
