using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Controls;

[TypeConverter(typeof(ExpandableObjectConverter))]
[Description("Clock Style")]
public class ClockStyleData : IDisposable, INotifyPropertyChanged
{
	private eClockStyles _ᎌ;

	private eClockShapes __2A0E;

	private ColorData __2A0F;

	private float __2A10;

	private ColorData __2A11;

	private Image __2A12;

	private ClockHandStyleData __2A13;

	private ClockHandStyleData __2A14;

	private ClockHandStyleData __2A15;

	private ColorData __2A16;

	private float __2A17;

	private Color __2A18;

	private Font __2A19;

	private ColorData __2A1A;

	private float __2A1B;

	private float __2A1C;

	private ColorData __2A1D;

	private float __2A1E;

	private float __2A1F;

	private int __2A20;

	private PropertyChangedEventHandler চ;

	private AnalogClockControl _ᗚ;

	[Browsable(false)]
	public eClockStyles Style
	{
		get
		{
			return _ᎌ;
		}
		set
		{
			if (value != _ᎌ)
			{
				_ᎌ = value;
				OnPropertyChanged(new PropertyChangedEventArgs("Style"));
			}
		}
	}

	[DefaultValue(eClockShapes.Round)]
	[Description("The clock shape for this style.")]
	[Category("Appearance")]
	public eClockShapes ClockShape
	{
		get
		{
			return __2A0E;
		}
		set
		{
			if (value != __2A0E)
			{
				__2A0E = value;
				OnPropertyChanged(new PropertyChangedEventArgs("ClockShape"));
			}
		}
	}

	[Category("Appearance")]
	[Description("The bezel color data for this style.")]
	public ColorData BezelColor
	{
		get
		{
			return __2A0F;
		}
		set
		{
			if (value != __2A0F)
			{
				if (__2A0F != null)
				{
					__2A0F.PropertyChanged -= _2A21;
				}
				__2A0F = value;
				if (__2A0F != null)
				{
					__2A0F.PropertyChanged += _2A21;
				}
				OnPropertyChanged(new PropertyChangedEventArgs("BezelColor"));
			}
		}
	}

	[Category("Appearance")]
	[Description("The width of clock bezel as a percentage value ranging from 0.0 to 1.0.")]
	[DefaultValue(0.03f)]
	public float BezelWidth
	{
		get
		{
			return __2A10;
		}
		set
		{
			if (value != __2A10)
			{
				__2A10 = value;
				OnPropertyChanged(new PropertyChangedEventArgs("BezelWidth"));
			}
		}
	}

	[Description("The face color data for this style.")]
	[Category("Appearance")]
	public ColorData FaceColor
	{
		get
		{
			return __2A11;
		}
		set
		{
			if (value != __2A11)
			{
				if (__2A11 != null)
				{
					__2A11.PropertyChanged -= _2A21;
				}
				__2A11 = value;
				if (__2A11 != null)
				{
					__2A11.PropertyChanged += _2A21;
				}
				OnPropertyChanged(new PropertyChangedEventArgs("FaceColor"));
			}
		}
	}

	[Category("Appearance")]
	[DefaultValue(null)]
	[Description("The face background image for this style.")]
	public Image FaceBackgroundImage
	{
		get
		{
			return __2A12;
		}
		set
		{
			if (value != __2A12)
			{
				__2A12 = value;
				OnPropertyChanged(new PropertyChangedEventArgs("FaceBackgroundImage"));
			}
		}
	}

	[Description("The hour hand style for this style.")]
	[Category("Appearance")]
	public ClockHandStyleData HourHandStyle
	{
		get
		{
			return __2A13;
		}
		set
		{
			if (value != __2A13)
			{
				if (__2A13 != null)
				{
					__2A13.PropertyChanged -= _2A21;
				}
				__2A13 = value;
				if (__2A13 != null)
				{
					__2A13.PropertyChanged += _2A21;
				}
				OnPropertyChanged(new PropertyChangedEventArgs("HourHandStyle"));
			}
		}
	}

	[Category("Appearance")]
	[Description("The minute hand style for this style.")]
	public ClockHandStyleData MinuteHandStyle
	{
		get
		{
			return __2A14;
		}
		set
		{
			if (value != __2A14)
			{
				if (__2A14 != null)
				{
					__2A14.PropertyChanged -= _2A21;
				}
				__2A14 = value;
				if (__2A14 != null)
				{
					__2A14.PropertyChanged += _2A21;
				}
				OnPropertyChanged(new PropertyChangedEventArgs("MinuteHandStyle"));
			}
		}
	}

	[Description("The second hand style for this style.")]
	[Category("Appearance")]
	public ClockHandStyleData SecondHandStyle
	{
		get
		{
			return __2A15;
		}
		set
		{
			if (value != __2A15)
			{
				if (__2A15 != null)
				{
					__2A15.PropertyChanged -= _2A21;
				}
				__2A15 = value;
				if (__2A15 != null)
				{
					__2A15.PropertyChanged += _2A21;
				}
				OnPropertyChanged(new PropertyChangedEventArgs("SecondHandStyle"));
			}
		}
	}

	[Category("Appearance")]
	[Description("The center cap color data for this style.")]
	public ColorData CapColor
	{
		get
		{
			return __2A16;
		}
		set
		{
			if (value != __2A16)
			{
				if (__2A16 != null)
				{
					__2A16.PropertyChanged -= _2A21;
				}
				__2A16 = value;
				if (__2A16 != null)
				{
					__2A16.PropertyChanged += _2A21;
				}
				OnPropertyChanged(new PropertyChangedEventArgs("CapColor"));
			}
		}
	}

	[Category("Appearance")]
	[Description("The center cap diameter as a percentage value ranging from 0.0 to 1.0.")]
	[DefaultValue(0.03f)]
	public float CapSize
	{
		get
		{
			return __2A17;
		}
		set
		{
			if (value != __2A17)
			{
				__2A17 = value;
				OnPropertyChanged(new PropertyChangedEventArgs("CapSize"));
			}
		}
	}

	[Description("The face number color for this style.")]
	[DefaultValue(typeof(Color), "139, 158, 168")]
	[Category("Appearance")]
	public Color NumberColor
	{
		get
		{
			return __2A18;
		}
		set
		{
			if (value != __2A18)
			{
				__2A18 = value;
				OnPropertyChanged(new PropertyChangedEventArgs("NumberColor"));
			}
		}
	}

	[Description("The face number font for this style.")]
	[Category("Appearance")]
	public Font NumberFont
	{
		get
		{
			return __2A19;
		}
		set
		{
			if (value != __2A19)
			{
				__2A19 = value;
				OnPropertyChanged(new PropertyChangedEventArgs("NumberFont"));
			}
		}
	}

	[Category("Appearance")]
	[Description("The large tick color data for this style.")]
	public ColorData LargeTickColor
	{
		get
		{
			return __2A1A;
		}
		set
		{
			if (value != __2A1A)
			{
				if (__2A1A != null)
				{
					__2A1A.PropertyChanged -= _2A21;
				}
				__2A1A = value;
				if (__2A1A != null)
				{
					__2A1A.PropertyChanged += _2A21;
				}
				OnPropertyChanged(new PropertyChangedEventArgs("LargeTickColor"));
			}
		}
	}

	[DefaultValue(0.06f)]
	[Description("The large tick length as a percentage value ranging from 0.0 to 1.0.")]
	[Category("Appearance")]
	public float LargeTickLength
	{
		get
		{
			return __2A1B;
		}
		set
		{
			if (value != __2A1B)
			{
				__2A1B = value;
				OnPropertyChanged(new PropertyChangedEventArgs("LargeTickLength"));
			}
		}
	}

	[Category("Appearance")]
	[DefaultValue(0.02f)]
	[Description("The large tick width as a percentage value ranging from 0.0 to 1.0.")]
	public float LargeTickWidth
	{
		get
		{
			return __2A1C;
		}
		set
		{
			if (value != __2A1C)
			{
				__2A1C = value;
				OnPropertyChanged(new PropertyChangedEventArgs("LargeTickWidth"));
			}
		}
	}

	[Category("Appearance")]
	[Description("The small tick color data for this style.")]
	public ColorData SmallTickColor
	{
		get
		{
			return __2A1D;
		}
		set
		{
			if (value != __2A1D)
			{
				if (__2A1D != null)
				{
					__2A1D.PropertyChanged -= _2A21;
				}
				__2A1D = value;
				if (__2A1D != null)
				{
					__2A1D.PropertyChanged += _2A21;
				}
				OnPropertyChanged(new PropertyChangedEventArgs("SmallTickColor"));
			}
		}
	}

	[Category("Appearance")]
	[Description("The small tick length as a percentage value ranging from 0.0 to 1.0.")]
	[DefaultValue(0.02f)]
	public float SmallTickLength
	{
		get
		{
			return __2A1E;
		}
		set
		{
			if (value != __2A1E)
			{
				__2A1E = value;
				OnPropertyChanged(new PropertyChangedEventArgs("SmallTickLength"));
			}
		}
	}

	[Category("Appearance")]
	[Description("The small tick width as a percentage value ranging from 0.0 to 1.0.")]
	[DefaultValue(0.02f)]
	public float SmallTickWidth
	{
		get
		{
			return __2A1F;
		}
		set
		{
			if (value != __2A1F)
			{
				__2A1F = value;
				OnPropertyChanged(new PropertyChangedEventArgs("SmallTickWidth"));
			}
		}
	}

	[DefaultValue(20)]
	[Category("Appearance")]
	[Description("The overlay angle, in degrees for this style.")]
	public int GlassAngle
	{
		get
		{
			return __2A20;
		}
		set
		{
			if (value != __2A20)
			{
				__2A20 = value;
				OnPropertyChanged(new PropertyChangedEventArgs("_GlassAngle"));
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public AnalogClockControl Parent
	{
		get
		{
			return _ᗚ;
		}
		internal set
		{
			_ᗚ = value;
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

	private void _2A21(object _٠, PropertyChangedEventArgs ؿ)
	{
		if (_ᗚ != null)
		{
			((Control)_ᗚ).Invalidate();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBezelColor()
	{
		BezelColor = new ColorData(eBrushTypes.Linear, Color.FromArgb(255, 255, 255), Color.FromArgb(152, 152, 152), Color.FromArgb(120, 120, 120), 1f, 45f);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetFaceColor()
	{
		FaceColor = new ColorData(eBrushTypes.Linear, Color.FromArgb(191, 204, 213), Color.FromArgb(255, 255, 255), Color.FromArgb(135, 145, 161), 1f, 45f);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetHourHandStyle()
	{
		HourHandStyle = new ClockHandStyleData(eHandStyles.Style1, 0.55f, 0.01f);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetMinuteHandStyle()
	{
		MinuteHandStyle = new ClockHandStyleData(eHandStyles.Style1, 0.8f, 0.01f);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSecondHandStyle()
	{
		SecondHandStyle = new ClockHandStyleData(eHandStyles.Style2, 0.8f, 0.005f);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetCapColor()
	{
		CapColor = new ColorData(eBrushTypes.Solid, Color.FromArgb(109, 127, 138), Color.FromArgb(109, 127, 138), Color.FromArgb(128, 109, 127, 138), 0.01f);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetNumberFont()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		__2A19 = new Font("Microsoft Sans Serif", 8f, (FontStyle)0, (GraphicsUnit)2);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetLargeTickColor()
	{
		LargeTickColor = new ColorData(eBrushTypes.Centered, Color.FromArgb(122, 142, 154), Color.FromArgb(122, 142, 154), Color.FromArgb(255, 255, 255), 1f);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSmallTickColor()
	{
		SmallTickColor = new ColorData(eBrushTypes.Centered, Color.FromArgb(122, 142, 154), Color.FromArgb(122, 142, 154), Color.FromArgb(255, 255, 255), 1f);
	}

	public ClockStyleData()
	{
		_2A22(eClockStyles.Style1);
	}

	public ClockStyleData(eClockStyles style)
	{
		_2A22(style);
	}

	public ClockStyleData(eClockStyles style, AnalogClockControl parent)
	{
		_2A22(style);
		_ᗚ = parent;
	}

	public void Dispose()
	{
		if (__2A12 != null)
		{
			__2A12.Dispose();
		}
		if (__2A19 != null)
		{
			__2A19.Dispose();
		}
	}

	private void _2A22(eClockStyles _0657)
	{
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Expected O, but got Unknown
		//IL_044f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0459: Expected O, but got Unknown
		_ᎌ = _0657;
		switch (_0657)
		{
		case eClockStyles.Style1:
		case eClockStyles.Custom:
			__2A0E = eClockShapes.Round;
			BezelColor = new ColorData(eBrushTypes.Linear, Color.FromArgb(255, 255, 255), Color.FromArgb(152, 152, 152), Color.FromArgb(120, 120, 120), 0.01f);
			__2A10 = 0.03f;
			FaceColor = new ColorData(eBrushTypes.Linear, Color.FromArgb(191, 204, 213), Color.FromArgb(255, 255, 255), Color.FromArgb(135, 145, 161), 0.01f, 45f);
			__2A12 = null;
			HourHandStyle = new ClockHandStyleData(eHandStyles.Style1, 0.55f, 0.015f);
			MinuteHandStyle = new ClockHandStyleData(eHandStyles.Style1, 0.8f, 0.01f);
			SecondHandStyle = new ClockHandStyleData(eHandStyles.Style2, 0.8f, 0.005f);
			CapColor = new ColorData(eBrushTypes.Solid, Color.FromArgb(109, 127, 138), Color.FromArgb(109, 127, 138), Color.FromArgb(128, 109, 127, 138), 0.01f);
			__2A17 = 0.03f;
			__2A18 = Color.FromArgb(139, 158, 168);
			__2A19 = new Font("Microsoft Sans Serif", 12f, (FontStyle)0, (GraphicsUnit)2);
			LargeTickColor = new ColorData(eBrushTypes.Linear, Color.FromArgb(122, 142, 154), Color.FromArgb(122, 142, 154), Color.FromArgb(128, 255, 255, 255), 0.01f);
			__2A1B = 0.06f;
			__2A1C = 0.02f;
			SmallTickColor = new ColorData(eBrushTypes.Linear, Color.FromArgb(122, 142, 154), Color.FromArgb(122, 142, 154), Color.FromArgb(128, 255, 255, 255), 0.01f);
			__2A1E = 0.02f;
			__2A1F = 0.02f;
			__2A20 = -20;
			break;
		case eClockStyles.Style2:
			__2A0E = eClockShapes.Round;
			BezelColor = new ColorData(eBrushTypes.Linear, Color.FromArgb(80, 80, 80), Color.FromArgb(0, 0, 0), Color.FromArgb(0, 0, 0), 0f, 90f);
			__2A10 = 0.03f;
			FaceColor = new ColorData(eBrushTypes.Linear, Color.FromArgb(225, 225, 225), Color.FromArgb(240, 240, 240), Color.FromArgb(0, 0, 0), 0f, 90f);
			__2A12 = null;
			HourHandStyle = new ClockHandStyleData(eHandStyles.Style3, 0.45f, 0.175f);
			HourHandStyle.HandColor = new ColorData(eBrushTypes.Linear, Color.FromArgb(0, 0, 0), Color.FromArgb(80, 80, 80), Color.FromArgb(64, 0, 0, 0), 0.01f, 90f);
			MinuteHandStyle = new ClockHandStyleData(eHandStyles.Style3, 0.75f, 0.175f);
			MinuteHandStyle.HandColor = new ColorData(eBrushTypes.Linear, Color.FromArgb(0, 0, 0), Color.FromArgb(80, 80, 80), Color.FromArgb(64, 0, 0, 0), 0.01f, 90f);
			SecondHandStyle = new ClockHandStyleData(eHandStyles.Style4, 0.9f, 0.01f);
			SecondHandStyle.HandColor = new ColorData(eBrushTypes.Solid, Color.FromArgb(255, 0, 0), Color.FromArgb(255, 0, 0), Color.FromArgb(128, 192, 0, 0), 0.01f);
			__2A15.DrawOverCap = true;
			CapColor = new ColorData(eBrushTypes.Solid, Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(223, 0, 0, 0), 0.01f);
			__2A17 = 0.1f;
			__2A18 = Color.FromArgb(0, 0, 0);
			__2A19 = new Font("Trebuchet MS", 12f, (FontStyle)0, (GraphicsUnit)2);
			LargeTickColor = new ColorData(eBrushTypes.Solid, Color.FromArgb(0, 0, 0), Color.FromArgb(0, 0, 0), Color.FromArgb(64, 0, 0, 0), 0.01f);
			__2A1B = 0.06f;
			__2A1C = 0.01f;
			SmallTickColor = new ColorData(eBrushTypes.Solid, Color.FromArgb(0, 0, 0), Color.FromArgb(0, 0, 0), Color.FromArgb(64, 0, 0, 0), 0.01f);
			__2A1E = 0.01f;
			__2A1F = 0.01f;
			__2A20 = 0;
			break;
		}
	}

	protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
	{
		if (চ != null)
		{
			চ(this, e);
		}
		if (_ᗚ != null)
		{
			((Control)_ᗚ).Invalidate();
		}
	}
}
