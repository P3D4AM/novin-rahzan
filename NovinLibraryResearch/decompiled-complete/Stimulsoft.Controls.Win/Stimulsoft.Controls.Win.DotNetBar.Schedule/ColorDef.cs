using System;
using System.ComponentModel;
using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar.Schedule;

[TypeConverter(typeof(ColorDefConvertor))]
public class ColorDef
{
	private Color[] _Colors;

	private float[] _Positions;

	private float _Angle;

	[Browsable(true)]
	[DefaultValue(null)]
	[NotifyParentProperty(true)]
	[Description("Indicates the Color array")]
	public Color[] Colors
	{
		get
		{
			return _Colors;
		}
		set
		{
			_Colors = value;
			OnColorDefChanged();
		}
	}

	[Browsable(true)]
	[DefaultValue(null)]
	[NotifyParentProperty(true)]
	[Description("Indicates the Color Positions.")]
	public float[] Positions
	{
		get
		{
			return _Positions;
		}
		set
		{
			_Positions = value;
			OnColorDefChanged();
		}
	}

	[Browsable(true)]
	[DefaultValue(0f)]
	[NotifyParentProperty(true)]
	[Description("Indicates the Gradient Angle.")]
	public float Angle
	{
		get
		{
			return _Angle;
		}
		set
		{
			_Angle = value;
			OnColorDefChanged();
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsEmpty
	{
		get
		{
			if (_Colors != null)
			{
				if (_Colors.Length == 1)
				{
					return _Colors[0].IsEmpty;
				}
				return false;
			}
			return true;
		}
	}

	[Description("Event raised when the ColorDef is changed")]
	public event EventHandler<EventArgs> ColorDefChanged;

	public ColorDef()
	{
	}

	public ColorDef(int rgb)
		: this(ColorScheme.GetColor(rgb))
	{
	}

	public ColorDef(Color color)
	{
		_Colors = new Color[1];
		_Colors[0] = color;
		_Positions = null;
	}

	public ColorDef(Color start, Color end)
		: this(start, end, 90f)
	{
	}

	public ColorDef(Color start, Color end, float angle)
	{
		_Colors = new Color[2] { start, end };
		_Positions = new float[2] { 0f, 1f };
		_Angle = angle;
	}

	public ColorDef(int[] rgbs, float[] cPositions)
		: this(rgbs, cPositions, 90f)
	{
	}

	public ColorDef(Color[] colors, float[] cPositions)
		: this(colors, cPositions, 90f)
	{
	}

	public ColorDef(int[] rgbs, float[] cPositions, float angle)
	{
		_Colors = new Color[rgbs.Length];
		for (int i = 0; i < rgbs.Length; i++)
		{
			_Colors[i] = ColorScheme.GetColor(rgbs[i]);
		}
		_Positions = cPositions;
		_Angle = angle;
	}

	public ColorDef(Color[] colors, float[] cPositions, float angle)
	{
		_Colors = colors;
		_Positions = cPositions;
		_Angle = angle;
	}

	private void OnColorDefChanged()
	{
		if (this.ColorDefChanged != null)
		{
			this.ColorDefChanged(this, EventArgs.Empty);
		}
	}
}
