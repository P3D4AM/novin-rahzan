using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;

namespace DevComponents.DotNetBar.Schedule;

[TypeConverter(typeof(ColorDefConvertor))]
public class ColorDef
{
	private EventHandler<EventArgs> ቾ;

	private Color[] _ቿ;

	private float[] _ኀ;

	private float _ኁ;

	[NotifyParentProperty(true)]
	[Description("Indicates the Color array")]
	[DefaultValue(null)]
	[Browsable(true)]
	public Color[] Colors
	{
		get
		{
			return _ቿ;
		}
		set
		{
			_ቿ = value;
			ኂ();
		}
	}

	[NotifyParentProperty(true)]
	[Description("Indicates the Color Positions.")]
	[Browsable(true)]
	[DefaultValue(null)]
	public float[] Positions
	{
		get
		{
			return _ኀ;
		}
		set
		{
			_ኀ = value;
			ኂ();
		}
	}

	[DefaultValue(0f)]
	[NotifyParentProperty(true)]
	[Description("Indicates the Gradient Angle.")]
	[Browsable(true)]
	public float Angle
	{
		get
		{
			return _ኁ;
		}
		set
		{
			_ኁ = value;
			ኂ();
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsEmpty
	{
		get
		{
			if (_ቿ != null)
			{
				if (_ቿ.Length == 1)
				{
					return _ቿ[0].IsEmpty;
				}
				return false;
			}
			return true;
		}
	}

	[Description("Event raised when the ColorDef is changed")]
	public event EventHandler<EventArgs> ColorDefChanged
	{
		add
		{
			EventHandler<EventArgs> eventHandler = ቾ;
			EventHandler<EventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<EventArgs> value2 = (EventHandler<EventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ቾ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<EventArgs> eventHandler = ቾ;
			EventHandler<EventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<EventArgs> value2 = (EventHandler<EventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ቾ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public ColorDef()
	{
	}

	public ColorDef(int rgb)
		: this(ColorScheme.GetColor(rgb))
	{
	}

	public ColorDef(Color color)
	{
		_ቿ = new Color[1];
		_ቿ[0] = color;
		_ኀ = null;
	}

	public ColorDef(Color start, Color end)
		: this(start, end, 90f)
	{
	}

	public ColorDef(Color start, Color end, float angle)
	{
		_ቿ = new Color[2] { start, end };
		_ኀ = new float[2] { 0f, 1f };
		_ኁ = angle;
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
		_ቿ = new Color[rgbs.Length];
		for (int i = 0; i < rgbs.Length; i++)
		{
			ref Color reference = ref _ቿ[i];
			reference = ColorScheme.GetColor(rgbs[i]);
		}
		_ኀ = cPositions;
		_ኁ = angle;
	}

	public ColorDef(Color[] colors, float[] cPositions, float angle)
	{
		_ቿ = colors;
		_ኀ = cPositions;
		_ኁ = angle;
	}

	private void ኂ()
	{
		if (ቾ != null)
		{
			ቾ(this, EventArgs.Empty);
		}
	}
}
