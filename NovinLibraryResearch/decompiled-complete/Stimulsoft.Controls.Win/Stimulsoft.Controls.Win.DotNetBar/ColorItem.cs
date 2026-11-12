using System.ComponentModel;
using System.Drawing;
using System.Xml;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class ColorItem : BaseItem
{
	private Color m_Color = Color.Black;

	private Size m_DesiredSize = new Size(13, 13);

	private bool m_MouseOver;

	private eColorItemBorder m_Border = eColorItemBorder.All;

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DevCoSerialize]
	[Category("Appearance")]
	[Description("Indicates the color represented by this item.")]
	public Color Color
	{
		get
		{
			return m_Color;
		}
		set
		{
			m_Color = value;
			OnAppearanceChanged();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DevCoSerialize]
	[Category("Appearance")]
	[Description("Indicates the size of the item when displayed.")]
	public Size DesiredSize
	{
		get
		{
			return m_DesiredSize;
		}
		set
		{
			if (value.Width > 0 && value.Height > 0)
			{
				m_DesiredSize = value;
				NeedRecalcSize = true;
				OnAppearanceChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(eColorItemBorder.All)]
	[Description("Indicate border drawn around the item")]
	[DevCoSerialize]
	public eColorItemBorder Border
	{
		get
		{
			return m_Border;
		}
		set
		{
			m_Border = value;
			OnAppearanceChanged();
		}
	}

	public bool IsMouseOver => m_MouseOver;

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
		m_Color = color;
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
	}

	public override void Paint(ItemPaintArgs p)
	{
		BaseRenderer renderer = p.Renderer;
		if (renderer != null)
		{
			ColorItemRendererEventArgs e = new ColorItemRendererEventArgs(p.Graphics, this);
			renderer.DrawColorItem(e);
			return;
		}
		ColorItemPainter colorItemPainter = PainterFactory.CreateColorItemPainter(this);
		if (colorItemPainter != null)
		{
			ColorItemRendererEventArgs e2 = new ColorItemRendererEventArgs(p.Graphics, this);
			colorItemPainter.PaintColorItem(e2);
		}
	}

	public override void RecalcSize()
	{
		m_Rect.Size = m_DesiredSize;
		base.RecalcSize();
	}

	private bool ShouldSerializeColor()
	{
		return m_Color != Color.Black;
	}

	private bool ShouldSerializeDesiredSize()
	{
		if (m_DesiredSize.Width == 13)
		{
			return m_DesiredSize.Height != 13;
		}
		return true;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void InternalMouseEnter()
	{
		base.InternalMouseEnter();
		SetMouseOver(value: true);
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

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void InternalMouseLeave()
	{
		base.InternalMouseLeave();
		SetMouseOver(value: false);
	}

	private void SetMouseOver(bool value)
	{
		if (value != m_MouseOver)
		{
			m_MouseOver = value;
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
