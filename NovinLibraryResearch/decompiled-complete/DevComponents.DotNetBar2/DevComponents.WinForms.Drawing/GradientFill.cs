using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DevComponents.WinForms.Drawing;

public class GradientFill : Fill
{
	private Color __1363 = Color.Empty;

	private Color __1364 = Color.Empty;

	private ColorBlendCollection __1365 = new ColorBlendCollection();

	private float _ኁ = 90f;

	[Description("Indicates the fill color.")]
	public Color Color1
	{
		get
		{
			return __1363;
		}
		set
		{
			__1363 = value;
		}
	}

	[Description("Indicates the fill color.")]
	public Color Color2
	{
		get
		{
			return __1364;
		}
		set
		{
			__1364 = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(true)]
	[Description("Collection that defines the multicolor gradient background.")]
	public ColorBlendCollection InterpolationColors => __1365;

	[Description("Indicates gradient fill angle.")]
	[DefaultValue(90)]
	public float Angle
	{
		get
		{
			return _ኁ;
		}
		set
		{
			_ኁ = value;
		}
	}

	public GradientFill()
	{
	}

	public GradientFill(Color color1, Color color2)
	{
		__1363 = color1;
		__1364 = color2;
	}

	public GradientFill(Color color1, Color color2, float angle)
	{
		__1363 = color1;
		__1364 = color2;
		_ኁ = angle;
	}

	public GradientFill(ColorStop[] interpolationColors)
	{
		__1365.AddRange(interpolationColors);
	}

	public GradientFill(ColorStop[] interpolationColors, int angle)
	{
		__1365.AddRange(interpolationColors);
		_ኁ = angle;
	}

	public override Brush CreateBrush(Rectangle bounds)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		if ((__1363.IsEmpty && __1364.IsEmpty && __1365.Count == 0) || bounds.Width < 1 || bounds.Height < 1)
		{
			return null;
		}
		LinearGradientBrush val = new LinearGradientBrush(bounds, __1363, __1364, _ኁ);
		if (__1365.Count == 0)
		{
			return (Brush)(object)val;
		}
		val.InterpolationColors = __1365.GetColorBlend();
		return (Brush)(object)val;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeColor1()
	{
		return !__1363.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetColor1()
	{
		Color1 = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeColor2()
	{
		return !__1364.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetColor2()
	{
		Color2 = Color.Empty;
	}

	public override Pen CreatePen(int width)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		if (!__1363.IsEmpty)
		{
			return new Pen(__1363, (float)width);
		}
		if (!__1364.IsEmpty)
		{
			return new Pen(__1364, (float)width);
		}
		return null;
	}
}
