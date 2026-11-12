using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar;

namespace Stimulsoft.Controls.Win.Editors.DateTimeAdv;

public class CalendarBase : ImageItem, IDesignTimeProvider
{
	private ElementStyle _BackgroundStyle = new ElementStyle();

	private ItemDisplay _ItemDisplay;

	[Browsable(true)]
	[Category("Style")]
	[Description("Gets or sets container background style.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ElementStyle BackgroundStyle => _BackgroundStyle;

	[DefaultValue("")]
	[Category("Design")]
	[Description("Indicates the name used to identify item.")]
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

	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Category("Accessibility")]
	[Description("Gets or sets the accessible role of the item.")]
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

	[Browsable(false)]
	[Category("Appearance")]
	[DefaultValue("")]
	[Description("Indicates the Key Tips access key or keys for the item when on Ribbon Control or Ribbon Bar.")]
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

	[Browsable(false)]
	[DevCoBrowsable(false)]
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

	[Browsable(false)]
	[DevCoBrowsable(false)]
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
	[DefaultValue("")]
	[Category("Design")]
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

	[Browsable(false)]
	[DevCoBrowsable(false)]
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
	[DefaultValue(null)]
	[Category("Appearance")]
	[Description("Specifies the mouse cursor displayed when mouse is over the item.")]
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
	[DefaultValue("")]
	[Category("Design")]
	[Description("Indicates description of the item that is displayed during design.")]
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
	[DevCoBrowsable(false)]
	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether certain global properties are propagated to all items with the same name when changed.")]
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

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DefaultValue(Stimulsoft.Controls.Win.DotNetBar.eItemAlignment.Near)]
	[Category("Appearance")]
	[Description("Determines alignment of the item inside the container.")]
	public override Stimulsoft.Controls.Win.DotNetBar.eItemAlignment ItemAlignment
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

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[Category("Design")]
	[Description("Indicates list of shortcut keys for this item.")]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.ShortcutsDesigner, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter("Stimulsoft.Controls.Win.DotNetBar.Design.ShortcutsConverter, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
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

	[Browsable(false)]
	[DevCoBrowsable(false)]
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
	[DefaultValue(false)]
	[Category("Appearance")]
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
		_BackgroundStyle.StyleChanged += BackgroundStyleChanged;
	}

	protected override void Dispose(bool disposing)
	{
		_BackgroundStyle.StyleChanged -= BackgroundStyleChanged;
		_BackgroundStyle.Dispose();
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
		ItemDisplay itemDisplay = GetItemDisplay();
		itemDisplay.Paint(this, p);
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
		bool dispose = false;
		ElementStyle elementStyle = ElementStyleDisplay.GetElementStyle(_BackgroundStyle, out dispose);
		displayRectangle.X += ElementStyleLayout.LeftWhiteSpace(elementStyle);
		displayRectangle.Width -= ElementStyleLayout.HorizontalStyleWhiteSpace(elementStyle);
		displayRectangle.Y += ElementStyleLayout.TopWhiteSpace(elementStyle);
		displayRectangle.Height -= ElementStyleLayout.VerticalStyleWhiteSpace(elementStyle);
		if (dispose)
		{
			elementStyle.Dispose();
		}
		return displayRectangle;
	}

	protected virtual void PaintBackground(ItemPaintArgs p)
	{
		_BackgroundStyle.SetColorScheme(p.Colors);
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
		return ElementStyleDisplay.GetElementStyle(_BackgroundStyle, out disposeStyle);
	}

	internal ItemDisplay GetItemDisplay()
	{
		if (_ItemDisplay == null)
		{
			_ItemDisplay = new ItemDisplay();
		}
		return _ItemDisplay;
	}

	private void BackgroundStyleChanged(object sender, EventArgs e)
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
		calendarBase.BackgroundStyle.ApplyStyle(_BackgroundStyle);
		base.CopyToItem(calendarBase);
	}

	protected virtual InsertPosition GetContainerInsertPosition(Point pScreen, BaseItem dragItem)
	{
		return DesignTimeProviderContainer.GetInsertPosition(this, pScreen, dragItem);
	}

	InsertPosition IDesignTimeProvider.GetInsertPosition(Point pScreen, BaseItem dragItem)
	{
		return GetContainerInsertPosition(pScreen, dragItem);
	}

	void IDesignTimeProvider.DrawReversibleMarker(int iPos, bool Before)
	{
		DesignTimeProviderContainer.DrawReversibleMarker(this, iPos, Before);
	}

	void IDesignTimeProvider.InsertItemAt(BaseItem objItem, int iPos, bool Before)
	{
		DesignTimeProviderContainer.InsertItemAt(this, objItem, iPos, Before);
	}
}
