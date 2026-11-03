using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;

namespace DevComponents.DotNetBar.Controls;

[Description("Color Data Class")]
[TypeConverter(typeof(ExpandableObjectConverter))]
public class ColorData : INotifyPropertyChanged
{
	private Color _Ꮃ;

	private float __2A23;

	private float __2A24;

	private float __2A25;

	private float __2A26;

	private eBrushTypes __2A27;

	private Color __1363;

	private Color __1364;

	private PropertyChangedEventHandler চ;

	[Description("The border color for this item.")]
	[Category("Appearance")]
	[DefaultValue(typeof(Color), "255, 255, 255")]
	public Color BorderColor
	{
		get
		{
			return _Ꮃ;
		}
		set
		{
			if (value != _Ꮃ)
			{
				_Ꮃ = value;
				OnPropertyChanged(new PropertyChangedEventArgs("BorderColor"));
			}
		}
	}

	[Category("Appearance")]
	[DefaultValue(0f)]
	[Description("The border width for this item.")]
	public float BorderWidth
	{
		get
		{
			return __2A23;
		}
		set
		{
			if (value != __2A23)
			{
				__2A23 = value;
				OnPropertyChanged(new PropertyChangedEventArgs("BorderWidth"));
			}
		}
	}

	[Category("Appearance")]
	[DefaultValue(0f)]
	[Description("The brush angle for this item. Only applies to Linear and Reflected brush types.")]
	public float BrushAngle
	{
		get
		{
			return __2A24;
		}
		set
		{
			if (value != __2A24)
			{
				__2A24 = value;
				OnPropertyChanged(new PropertyChangedEventArgs("BrushAngle"));
			}
		}
	}

	[DefaultValue(0.5f)]
	[Category("Appearance")]
	[Description("The brush SigmaBellShape focus for this item. Only applies to Linear and Reflected brush types.")]
	public float BrushSBSFocus
	{
		get
		{
			return __2A25;
		}
		set
		{
			if (value != __2A25)
			{
				__2A25 = value;
				OnPropertyChanged(new PropertyChangedEventArgs("BrushSBSFocus"));
			}
		}
	}

	[Description("The brush SigmaBellShape scale for this item. Only applies to Linear and Reflected brush types.")]
	[DefaultValue(0.5f)]
	[Category("Appearance")]
	public float BrushSBSScale
	{
		get
		{
			return __2A26;
		}
		set
		{
			if (value != __2A26)
			{
				__2A26 = value;
				OnPropertyChanged(new PropertyChangedEventArgs("BrushSBSScale"));
			}
		}
	}

	[Description("The brush type for this item.")]
	[DefaultValue(eBrushTypes.Solid)]
	[Category("Appearance")]
	public eBrushTypes BrushType
	{
		get
		{
			return __2A27;
		}
		set
		{
			if (value != __2A27)
			{
				__2A27 = value;
				OnPropertyChanged(new PropertyChangedEventArgs("BrushType"));
			}
		}
	}

	[DefaultValue(typeof(Color), "255, 255, 255")]
	[Category("Appearance")]
	[Description("The first color for this item.")]
	public Color Color1
	{
		get
		{
			return __1363;
		}
		set
		{
			if (value != __1363)
			{
				__1363 = value;
				OnPropertyChanged(new PropertyChangedEventArgs("Color1"));
			}
		}
	}

	[Description("The second color for this item.")]
	[Category("Appearance")]
	[DefaultValue(typeof(Color), "255, 255, 255")]
	public Color Color2
	{
		get
		{
			return __1364;
		}
		set
		{
			if (value != __1364)
			{
				__1364 = value;
				OnPropertyChanged(new PropertyChangedEventArgs("Color2"));
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged
	{
		add
		{
			PropertyChangedEventHandler propertyChangedEventHandler = চ;
			PropertyChangedEventHandler propertyChangedEventHandler2;
			do
			{
				propertyChangedEventHandler2 = propertyChangedEventHandler;
				PropertyChangedEventHandler value2 = (PropertyChangedEventHandler)Delegate.Combine(propertyChangedEventHandler2, value);
				propertyChangedEventHandler = Interlocked.CompareExchange(ref চ, value2, propertyChangedEventHandler2);
			}
			while ((object)propertyChangedEventHandler != propertyChangedEventHandler2);
		}
		remove
		{
			PropertyChangedEventHandler propertyChangedEventHandler = চ;
			PropertyChangedEventHandler propertyChangedEventHandler2;
			do
			{
				propertyChangedEventHandler2 = propertyChangedEventHandler;
				PropertyChangedEventHandler value2 = (PropertyChangedEventHandler)Delegate.Remove(propertyChangedEventHandler2, value);
				propertyChangedEventHandler = Interlocked.CompareExchange(ref চ, value2, propertyChangedEventHandler2);
			}
			while ((object)propertyChangedEventHandler != propertyChangedEventHandler2);
		}
	}

	public ColorData()
	{
		LoadData(eBrushTypes.Solid, Color.White, Color.White, Color.White, 0f, 0f, 0.5f, 1f);
	}

	public ColorData(eBrushTypes brushType, Color color1, Color color2)
	{
		LoadData(brushType, color1, color2, Color.White, 0f, 0f, 0.5f, 1f);
	}

	public ColorData(eBrushTypes brushType, Color color1, Color color2, Color borderColor, float borderWidth)
	{
		LoadData(brushType, color1, color2, borderColor, borderWidth, 0f, 0.5f, 1f);
	}

	public ColorData(eBrushTypes brushType, Color color1, Color color2, Color borderColor, float borderWidth, float brushAngle)
	{
		LoadData(brushType, color1, color2, borderColor, borderWidth, brushAngle, 0.5f, 1f);
	}

	public ColorData(eBrushTypes brushType, Color color1, Color color2, Color borderColor, float borderWidth, float brushSBSFocus, float brushSBSScale)
	{
		LoadData(brushType, color1, color2, borderColor, borderWidth, 0f, brushSBSFocus, brushSBSScale);
	}

	protected void LoadData(eBrushTypes brushType, Color color1, Color color2, Color borderColor, float borderWidth, float brushAngle, float brushSBSFocus, float brushSBSScale)
	{
		_Ꮃ = borderColor;
		__2A23 = borderWidth;
		__1363 = color1;
		__1364 = color2;
		__2A27 = brushType;
		__2A24 = brushAngle;
		__2A25 = brushSBSFocus;
		__2A26 = brushSBSScale;
	}

	public Pen GetBorderPen(float scaleFactor, PenAlignment penAlignment)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		Pen val = new Pen(_Ꮃ, (float)Math.Round(__2A23 * scaleFactor, 0));
		val.Alignment = penAlignment;
		return val;
	}

	public Brush GetBrush(GraphicsPath path)
	{
		return GetBrush(path, new PointF(0.5f, 0.5f), __2A24);
	}

	public Brush GetBrush(GraphicsPath path, float angle)
	{
		return GetBrush(path, new PointF(0.5f, 0.5f), angle);
	}

	public Brush GetBrush(GraphicsPath path, PointF center)
	{
		return GetBrush(path, center, __2A24);
	}

	public Brush GetBrush(GraphicsPath path, PointF center, float angle)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Expected O, but got Unknown
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Expected O, but got Unknown
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		switch (__2A27)
		{
		case eBrushTypes.Solid:
			return (Brush)new SolidBrush(__1363);
		case eBrushTypes.Linear:
		{
			path.Flatten();
			RectangleF bounds = path.GetBounds();
			if (bounds.Width > 0f && bounds.Height > 0f)
			{
				return (Brush)new LinearGradientBrush(path.GetBounds(), __1363, __1364, angle, false);
			}
			return null;
		}
		case eBrushTypes.Reflected:
		{
			LinearGradientBrush val2 = new LinearGradientBrush(path.GetBounds(), __1363, __1364, angle, false);
			val2.SetSigmaBellShape(__2A25, __2A26);
			return (Brush)(object)val2;
		}
		case eBrushTypes.Centered:
		{
			PointF centerPoint = default(PointF);
			RectangleF bounds = path.GetBounds();
			PathGradientBrush val = new PathGradientBrush(path);
			centerPoint.X = bounds.X + bounds.Width * center.X;
			centerPoint.Y = bounds.Y + bounds.Height * center.Y;
			val.CenterPoint = centerPoint;
			val.CenterColor = __1363;
			val.SurroundColors = new Color[1] { __1364 };
			return (Brush)(object)val;
		}
		default:
			return (Brush)new SolidBrush(__1363);
		}
	}

	public Brush GetSolidBrush()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		return (Brush)new SolidBrush(__1363);
	}

	public LinearGradientBrush GetLinearBrush(GraphicsPath path, int angle)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		return new LinearGradientBrush(path.GetBounds(), __1363, __1364, (float)angle, false);
	}

	public PathGradientBrush GetCenteredBrush(GraphicsPath path, PointF center)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		PathGradientBrush val = new PathGradientBrush(path);
		val.CenterPoint = center;
		val.CenterColor = __1363;
		val.SurroundColors = new Color[1] { __1364 };
		return val;
	}

	public LinearGradientBrush GetReflectedBrush(GraphicsPath path, int angle)
	{
		return GetReflectedBrush(path, angle, 0.5f, 1f);
	}

	public LinearGradientBrush GetReflectedBrush(GraphicsPath path, int angle, float focus)
	{
		return GetReflectedBrush(path, angle, focus, 1f);
	}

	public LinearGradientBrush GetReflectedBrush(GraphicsPath path, int angle, float focus, float scale)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		LinearGradientBrush val = new LinearGradientBrush(path.GetBounds(), __1363, __1364, (float)angle, false);
		val.SetSigmaBellShape(focus, scale);
		return val;
	}

	protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
	{
		if (চ != null)
		{
			চ(this, e);
		}
	}
}
