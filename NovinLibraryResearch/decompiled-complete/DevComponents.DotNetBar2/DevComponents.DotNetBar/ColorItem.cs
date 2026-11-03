using System.ComponentModel;
using System.Drawing;
using System.Xml;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar;

public class ColorItem : BaseItem
{
	private Color ൠ = Color.Black;

	private Size ẑ = new Size(13, 13);

	private bool ݡ;

	private eColorItemBorder ᖾ = eColorItemBorder.All;

	[DevCoSerialize]
	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates the color represented by this item.")]
	[DevCoBrowsable(true)]
	public Color Color
	{
		get
		{
			return ൠ;
		}
		set
		{
			ൠ = value;
			OnAppearanceChanged();
		}
	}

	[DevCoSerialize]
	[Category("Appearance")]
	[DevCoBrowsable(true)]
	[Description("Indicates the size of the item when displayed.")]
	[Browsable(true)]
	public Size DesiredSize
	{
		get
		{
			return ẑ;
		}
		set
		{
			if (value.Width > 0 && value.Height > 0)
			{
				ẑ = value;
				NeedRecalcSize = true;
				OnAppearanceChanged();
			}
		}
	}

	[DevCoBrowsable(true)]
	[Description("Indicate border drawn around the item")]
	[Browsable(true)]
	[DevCoSerialize]
	[DefaultValue(eColorItemBorder.All)]
	public eColorItemBorder Border
	{
		get
		{
			return ᖾ;
		}
		set
		{
			ᖾ = value;
			OnAppearanceChanged();
		}
	}

	public bool IsMouseOver => ݡ;

	public ColorItem()
		: this("", "")
	{
	}

	public ColorItem(string sName)
		: this(sName, "")
	{
	}

	public ColorItem(string sName, string ItemText)
		: base(sName, ItemText)
	{
		IsAccessible = false;
		CanCustomize = false;
	}

	public ColorItem(string sName, string ItemText, Color color)
		: base(sName, ItemText)
	{
		IsAccessible = false;
		CanCustomize = false;
		ൠ = color;
	}

	public override BaseItem Copy()
	{
		ColorItem colorItem = new ColorItem(m_Name);
		CopyToItem(colorItem);
		return colorItem;
	}

	protected override void CopyToItem(BaseItem copy)
	{
		ColorItem colorItem = copy as ColorItem;
		base.CopyToItem(colorItem);
		colorItem.DesiredSize = DesiredSize;
		colorItem.Color = Color;
		colorItem.Border = Border;
		if (ShouldSerializeDesiredSize())
		{
			colorItem.DesiredSize = DesiredSize;
		}
	}

	public override void Paint(ItemPaintArgs p)
	{
		BaseRenderer ໝ = p.ໝ;
		if (ໝ != null)
		{
			ColorItemRendererEventArgs e = new ColorItemRendererEventArgs(p.Graphics, this);
			ໝ.DrawColorItem(e);
			return;
		}
		Ẓ ẓ = ᣓ.ᤕ(this);
		if (ẓ != null)
		{
			ColorItemRendererEventArgs e2 = new ColorItemRendererEventArgs(p.Graphics, this);
			ẓ.PaintColorItem(e2);
		}
	}

	public override void RecalcSize()
	{
		m_Rect.Size = Dpi.Size(ẑ);
		base.RecalcSize();
	}

	private bool ShouldSerializeColor()
	{
		return ൠ != Color.Black;
	}

	private bool ShouldSerializeDesiredSize()
	{
		if (ẑ.Width == 13)
		{
			return ẑ.Height != 13;
		}
		return true;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void InternalMouseEnter()
	{
		base.InternalMouseEnter();
		_0ECC(ԝ: true);
		BaseItem parent = Parent;
		while (parent != null && !(parent is ColorPickerDropDown))
		{
			parent = parent.Parent;
		}
		if (parent != null && parent is ColorPickerDropDown)
		{
			((ColorPickerDropDown)parent).InvokeColorPreview(new ColorPreviewEventArgs(Color, this));
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public override void InternalMouseLeave()
	{
		base.InternalMouseLeave();
		_0ECC(ԝ: false);
	}

	private void _0ECC(bool ԝ)
	{
		if (ԝ != ݡ)
		{
			ݡ = ԝ;
			Refresh();
		}
	}

	protected internal override void Serialize(ItemSerializationContext context)
	{
		base.Serialize(context);
		XmlElement itemXmlElement = context.ItemXmlElement;
		ElementSerializer.Serialize(this, itemXmlElement);
	}

	public override void Deserialize(ItemSerializationContext context)
	{
		base.Deserialize(context);
		XmlElement itemXmlElement = context.ItemXmlElement;
		ElementSerializer.Deserialize(this, itemXmlElement);
	}
}
