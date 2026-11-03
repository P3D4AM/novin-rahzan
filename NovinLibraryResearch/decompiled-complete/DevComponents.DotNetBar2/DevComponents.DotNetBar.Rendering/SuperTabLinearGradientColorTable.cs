using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;

namespace DevComponents.DotNetBar.Rendering;

[TypeConverter(typeof(SuperTabLinearGradientColorTableConvertor))]
public class SuperTabLinearGradientColorTable
{
	private EventHandler<EventArgs> ቇ;

	private Color[] _ቿ;

	private float[] _ኀ;

	private int _ቴ = 90;

	private bool? _ኍ;

	[NotifyParentProperty(true)]
	[Description("Indicates the Gradient Colors.")]
	[Browsable(true)]
	[DefaultValue(null)]
	public Color[] Colors
	{
		get
		{
			return _ቿ;
		}
		set
		{
			if (_ቿ != value)
			{
				_ቿ = value;
				OnColorTableChanged();
			}
		}
	}

	[DefaultValue(null)]
	[NotifyParentProperty(true)]
	[Description("Indicates the Gradient Color Positions.")]
	[Browsable(true)]
	public float[] Positions
	{
		get
		{
			return _ኀ;
		}
		set
		{
			if (_ኀ != value)
			{
				_ኀ = value;
				OnColorTableChanged();
			}
		}
	}

	[NotifyParentProperty(true)]
	[Browsable(true)]
	[DefaultValue(90)]
	[Description("Indicates the Gradient angle.")]
	public int GradientAngle
	{
		get
		{
			return _ቴ;
		}
		set
		{
			if (_ቴ != value)
			{
				_ቴ = value;
				OnColorTableChanged();
			}
		}
	}

	[Description("Indicates whether the Gradient will adapt to changes in the TabAlignment.")]
	[NotifyParentProperty(true)]
	[DefaultValue(null)]
	[Browsable(true)]
	public bool? AdaptiveGradient
	{
		get
		{
			return _ኍ;
		}
		set
		{
			if (_ኍ != value)
			{
				_ኍ = value;
				OnColorTableChanged();
			}
		}
	}

	[Browsable(false)]
	public bool IsEmpty
	{
		get
		{
			if ((_ቿ == null || _ቿ.Length == 0) && (_ኀ == null || _ኀ.Length == 0) && _ቴ == 90)
			{
				return !_ኍ.HasValue;
			}
			return false;
		}
	}

	[Description("Event raised when the SuperTabLinearGradientColorTable is changed")]
	public event EventHandler<EventArgs> ColorTableChanged
	{
		add
		{
			EventHandler<EventArgs> eventHandler = ቇ;
			EventHandler<EventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<EventArgs> value2 = (EventHandler<EventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ቇ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<EventArgs> eventHandler = ቇ;
			EventHandler<EventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<EventArgs> value2 = (EventHandler<EventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ቇ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public SuperTabLinearGradientColorTable()
	{
	}

	public SuperTabLinearGradientColorTable(Color color)
	{
		_ቿ = new Color[1];
		_ቿ[0] = color;
	}

	public SuperTabLinearGradientColorTable(Color start, Color end)
	{
		_ቿ = new Color[2];
		_ቿ[0] = start;
		_ቿ[1] = end;
	}

	public SuperTabLinearGradientColorTable(Color start, Color end, int gradient)
	{
		_ቿ = new Color[2];
		_ቿ[0] = start;
		_ቿ[1] = end;
		_ቴ = gradient;
	}

	public SuperTabLinearGradientColorTable(Color[] colors, float[] positions)
	{
		_ቿ = colors;
		_ኀ = positions;
	}

	protected void OnColorTableChanged()
	{
		if (ቇ != null)
		{
			ቇ(this, EventArgs.Empty);
		}
	}

	public Brush GetBrush(Rectangle r)
	{
		return GetBrush(r, _ቴ);
	}

	public Brush GetBrush(Rectangle r, int angle)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		if (_ቿ != null && _ቿ.Length > 0)
		{
			if (_ቿ.Length == 1)
			{
				return (Brush)new SolidBrush(Colors[0]);
			}
			LinearGradientBrush val = new LinearGradientBrush(r, Color.White, Color.White, (float)angle);
			val.InterpolationColors = _128E();
			return (Brush)(object)val;
		}
		return null;
	}

	private ColorBlend _128E()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Expected O, but got Unknown
		ColorBlend val = new ColorBlend(_ቿ.Length);
		float[] array = _ኀ;
		if (array == null || array.Length != _ቿ.Length)
		{
			array = new float[_ቿ.Length];
			float num = 1 / _ቿ.Length;
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = (float)i * num;
			}
			array[_ቿ.Length - 1] = 1f;
		}
		for (int j = 0; j < _ቿ.Length; j++)
		{
			ref Color reference = ref val.Colors[j];
			reference = _ቿ[j];
		}
		val.Positions = array;
		return val;
	}

	public BackgroundColorBlendCollection GetColorBlendCollection()
	{
		BackgroundColorBlendCollection backgroundColorBlendCollection = new BackgroundColorBlendCollection();
		float[] array = _ኀ;
		if (array == null || array.Length != _ቿ.Length)
		{
			array = new float[_ቿ.Length];
			float num = 1 / _ቿ.Length;
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = (float)i * num;
			}
			array[_ቿ.Length - 1] = 1f;
		}
		for (int j = 0; j < _ቿ.Length; j++)
		{
			backgroundColorBlendCollection.Add(new BackgroundColorBlend(_ቿ[j], array[j]));
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
