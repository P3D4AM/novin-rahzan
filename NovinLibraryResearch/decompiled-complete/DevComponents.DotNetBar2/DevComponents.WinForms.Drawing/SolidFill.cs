using System.ComponentModel;
using System.Drawing;

namespace DevComponents.WinForms.Drawing;

public class SolidFill : Fill
{
	private Color __1360 = Color.Empty;

	[Description("Indicates the fill color.")]
	public Color Color
	{
		get
		{
			return __1360;
		}
		set
		{
			__1360 = value;
		}
	}

	public SolidFill(Color color)
	{
		__1360 = color;
	}

	public SolidFill()
	{
	}

	public override Brush CreateBrush(Rectangle bounds)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		if (__1360.IsEmpty)
		{
			return null;
		}
		return (Brush)new SolidBrush(__1360);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeColor()
	{
		return !__1360.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetColor()
	{
		Color = Color.Empty;
	}

	public override Pen CreatePen(int width)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		if (!__1360.IsEmpty)
		{
			return new Pen(__1360, (float)width);
		}
		return null;
	}
}
