using System.ComponentModel;
using System.Drawing;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar.Controls;

public class NewTabFormItem : TabFormItemBase
{
	private string __1372 = "Default";

	private Color[] __137D;

	[Description("Indicates the array of colors that when set are used to draw the background of the item.")]
	[TypeConverter(typeof(ArrayConverter))]
	[Category("Appearance")]
	[DefaultValue(null)]
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

	public override void Paint(ItemPaintArgs p)
	{
		BaseRenderer ໝ = p.ໝ;
		if (ໝ != null)
		{
			p.ButtonItemRendererEventArgs.Graphics = p.Graphics;
			p.ButtonItemRendererEventArgs.ButtonItem = this;
			p.ButtonItemRendererEventArgs.က = p;
			ໝ.DrawNewTabFormItem(p.ButtonItemRendererEventArgs);
		}
		if (!string.IsNullOrEmpty(base.NotificationMarkText))
		{
			_1A6E._139A(p.Graphics, Bounds, base.NotificationMarkPosition, base.NotificationMarkText, new Size(base.NotificationMarkSize, base.NotificationMarkSize), base.NotificationMarkOffset, base.NotificationMarkColor);
		}
		DrawInsertMarker(p.Graphics);
	}

	public override void RecalcSize()
	{
		m_Rect.Width = Dpi.Width32 + TabFormItem._136F / 2;
		m_Rect.Height = Dpi.Height16;
		m_NeedRecalcSize = false;
	}

	internal override string ຽ()
	{
		if (!(CustomColorName != ""))
		{
			return __1372;
		}
		return CustomColorName;
	}
}
