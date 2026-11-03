using System.ComponentModel;
using System.Drawing;

namespace DevComponents.WinForms.Drawing;

[ToolboxItem(false)]
public abstract class Border : Component
{
	internal int _ች;

	[DefaultValue(0)]
	[Description("Indicates border width.")]
	public int Width
	{
		get
		{
			return _ች;
		}
		set
		{
			_ች = value;
		}
	}

	public abstract Pen CreatePen();

	internal static Rectangle Deflate(Rectangle bounds, Border border)
	{
		if (border == null)
		{
			return bounds;
		}
		bounds.Inflate(-border.Width, -border.Width);
		return bounds;
	}
}
