using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Stimulsoft.Controls.Win.DotNetBar.Rendering;

[TypeConverter(typeof(SuperTabLinearGradientColorTableConvertor))]
public class SuperTabLinearGradientColorTable
{
	private Color[] _Colors;

	private float[] _Positions;

	private int _GradientAngle = 90;

	private bool? _AdaptiveGradient;

	[Browsable(true)]
	[DefaultValue(null)]
	[NotifyParentProperty(true)]
	[Description("Indicates the Gradient Colors.")]
	public Color[] Colors
	{
		get
		{
			return _Colors;
		}
		set
		{
			if (_Colors != value)
			{
				_Colors = value;
				OnColorTableChanged();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(null)]
	[NotifyParentProperty(true)]
	[Description("Indicates the Gradient Color Positions.")]
	public float[] Positions
	{
		get
		{
			return _Positions;
		}
		set
		{
			if (_Positions != value)
			{
				_Positions = value;
				OnColorTableChanged();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(90)]
	[NotifyParentProperty(true)]
	[Description("Indicates the Gradient angle.")]
	public int GradientAngle
	{
		get
		{
			return _GradientAngle;
		}
		set
		{
			if (_GradientAngle != value)
			{
				_GradientAngle = value;
				OnColorTableChanged();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(null)]
	[NotifyParentProperty(true)]
	[Description("Indicates whether the Gradient will adapt to changes in the TabAlignment.")]
	public bool? AdaptiveGradient
	{
		get
		{
			return _AdaptiveGradient;
		}
		set
		{
			if (_AdaptiveGradient != value)
			{
				_AdaptiveGradient = value;
				OnColorTableChanged();
			}
		}
	}

	[Browsable(false)]
	public bool IsEmpty
	{
		get
		{
			if ((_Colors == null || _Colors.Length == 0) && (_Positions == null || _Positions.Length == 0) && _GradientAngle == 90)
			{
				return !_AdaptiveGradient.HasValue;
			}
			return false;
		}
	}

	[Description("Event raised when the SuperTabLinearGradientColorTable is changed")]
	public event EventHandler<EventArgs> ColorTableChanged;

	public SuperTabLinearGradientColorTable()
	{
	}

	public SuperTabLinearGradientColorTable(Color color)
	{
		_Colors = new Color[1];
		_Colors[0] = color;
	}

	public SuperTabLinearGradientColorTable(Color start, Color end)
	{
		_Colors = new Color[2];
		_Colors[0] = start;
		_Colors[1] = end;
	}

	public SuperTabLinearGradientColorTable(Color start, Color end, int gradient)
	{
		_Colors = new Color[2];
		_Colors[0] = start;
		_Colors[1] = end;
		_GradientAngle = gradient;
	}

	public SuperTabLinearGradientColorTable(Color[] colors, float[] positions)
	{
		_Colors = colors;
		_Positions = positions;
	}

	protected void OnColorTableChanged()
	{
		if (this.ColorTableChanged != null)
		{
			this.ColorTableChanged(this, EventArgs.Empty);
		}
	}

	public Brush GetBrush(Rectangle r)
	{
		return GetBrush(r, _GradientAngle);
	}

	public Brush GetBrush(Rectangle r, int angle)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		if (_Colors != null && _Colors.Length != 0)
		{
			if (_Colors.Length == 1)
			{
				return (Brush)new SolidBrush(Colors[0]);
			}
			LinearGradientBrush val = new LinearGradientBrush(r, Color.White, Color.White, (float)angle);
			val.InterpolationColors = GetColorBlend();
			return (Brush)(object)val;
		}
		return null;
	}

	private ColorBlend GetColorBlend()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Expected O, but got Unknown
		ColorBlend val = new ColorBlend(_Colors.Length);
		float[] array = _Positions;
		if (array == null || array.Length != _Colors.Length)
		{
			array = new float[_Colors.Length];
			float num = 1 / _Colors.Length;
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = (float)i * num;
			}
			array[_Colors.Length - 1] = 1f;
		}
		for (int j = 0; j < _Colors.Length; j++)
		{
			val.Colors[j] = _Colors[j];
		}
		val.Positions = array;
		return val;
	}

	public BackgroundColorBlendCollection GetColorBlendCollection()
	{
		BackgroundColorBlendCollection backgroundColorBlendCollection = new BackgroundColorBlendCollection();
		float[] array = _Positions;
		if (array == null || array.Length != _Colors.Length)
		{
			array = new float[_Colors.Length];
			float num = 1 / _Colors.Length;
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = (float)i * num;
			}
			array[_Colors.Length - 1] = 1f;
		}
		for (int j = 0; j < _Colors.Length; j++)
		{
			backgroundColorBlendCollection.Add(new BackgroundColorBlend(_Colors[j], array[j]));
		}
		return backgroundColorBlendCollection;
	}

	public object Clone()
	{
		SuperTabLinearGradientColorTable superTabLinearGradientColorTable = new SuperTabLinearGradientColorTable();
		superTabLinearGradientColorTable.Colors = Colors;
		superTabLinearGradientColorTable.Positions = Positions;
		superTabLinearGradientColorTable.GradientAngle = GradientAngle;
		superTabLinearGradientColorTable.AdaptiveGradient = AdaptiveGradient;
		return superTabLinearGradientColorTable;
	}
}
