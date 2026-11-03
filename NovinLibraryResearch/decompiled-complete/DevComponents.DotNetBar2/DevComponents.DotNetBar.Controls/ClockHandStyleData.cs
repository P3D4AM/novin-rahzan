using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;

namespace DevComponents.DotNetBar.Controls;

[TypeConverter(typeof(ExpandableObjectConverter))]
[Description("Clock Hand Style")]
public class ClockHandStyleData : INotifyPropertyChanged
{
	private bool __2A28;

	private ColorData __2A29;

	private eHandStyles __2A2A;

	private float __2A2B;

	private float _ች;

	private PropertyChangedEventHandler চ;

	[Description("Indicates whether the hand is drawn over the cap.")]
	[DefaultValue(false)]
	[Category("Appearance")]
	public bool DrawOverCap
	{
		get
		{
			return __2A28;
		}
		set
		{
			__2A28 = value;
		}
	}

	[Category("Appearance")]
	[Description("The hand color data for this hand.")]
	public ColorData HandColor
	{
		get
		{
			return __2A29;
		}
		set
		{
			if (value != __2A29)
			{
				if (__2A29 != null)
				{
					__2A29.PropertyChanged -= _2A21;
				}
				__2A29 = value;
				if (__2A29 != null)
				{
					__2A29.PropertyChanged += _2A21;
				}
				OnPropertyChanged(new PropertyChangedEventArgs("HandColor"));
			}
		}
	}

	[DefaultValue(eHandStyles.Style1)]
	[Description("The hand style for this clock hand.")]
	[Category("Appearance")]
	public eHandStyles HandStyle
	{
		get
		{
			return __2A2A;
		}
		set
		{
			if (value != __2A2A)
			{
				__2A2A = value;
				OnPropertyChanged(new PropertyChangedEventArgs("HandStyle"));
			}
		}
	}

	[Description("The length of this clock hand as a percentage value ranging from 0.0 to 1.0, with 1.0 being half the width/height of the bounding rectangle.")]
	[DefaultValue(1f)]
	[Category("Appearance")]
	public float Length
	{
		get
		{
			return __2A2B;
		}
		set
		{
			if (value != __2A2B)
			{
				__2A2B = value;
				OnPropertyChanged(new PropertyChangedEventArgs("Length"));
			}
		}
	}

	[Category("Appearance")]
	[DefaultValue(0.1f)]
	[Description("The width of this clock hand as a percentage value ranging from 0.0 to 1.0, with 1.0 being half the width/height of the bounding rectangle.")]
	public float Width
	{
		get
		{
			return _ች;
		}
		set
		{
			if (value != _ች)
			{
				_ች = value;
				OnPropertyChanged(new PropertyChangedEventArgs("Width"));
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

	private void _2A21(object _٠, PropertyChangedEventArgs ؿ)
	{
		OnPropertyChanged(ؿ);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetHandColor()
	{
		HandColor = new ColorData(eBrushTypes.Solid, Color.FromArgb(109, 127, 138), Color.FromArgb(109, 127, 138), Color.FromArgb(128, 109, 127, 138), 0.01f);
	}

	public ClockHandStyleData()
	{
		__2A28 = false;
		__2A29 = new ColorData(eBrushTypes.Solid, Color.FromArgb(109, 127, 138), Color.FromArgb(109, 127, 138), Color.FromArgb(128, 109, 127, 138), 0.01f);
		__2A2A = eHandStyles.Style1;
		__2A2B = 1f;
		_ች = 0.1f;
	}

	public ClockHandStyleData(eHandStyles handStyle, float length, float width)
	{
		__2A28 = false;
		__2A29 = new ColorData(eBrushTypes.Solid, Color.FromArgb(109, 127, 138), Color.FromArgb(109, 127, 138), Color.FromArgb(128, 109, 127, 138), 0.01f);
		__2A2A = handStyle;
		__2A2B = length;
		_ች = width;
	}

	public virtual bool ContainsPoint(RectangleF boundingRect, float angle, Point pt)
	{
		GraphicsPath val = GenerateHandPath(boundingRect, angle);
		bool result = val.IsVisible(pt);
		val.Dispose();
		return result;
	}

	public GraphicsPath GenerateHandPath(RectangleF boundingRect, float angle)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		switch (__2A2A)
		{
		case eHandStyles.Style1:
		{
			PointF[] array = new PointF[4];
			array[0].X = -0.5f;
			array[0].Y = 0f;
			array[1].X = -0.5f;
			array[1].Y = -1f;
			array[2].X = 0.5f;
			array[2].Y = -1f;
			array[3].X = 0.5f;
			array[3].Y = 0f;
			val.AddPolygon(array);
			break;
		}
		case eHandStyles.Style2:
		{
			PointF[] array = new PointF[4];
			array[0].X = -0.4f;
			array[0].Y = 0.25f;
			array[1].X = -0.4f;
			array[1].Y = -1f;
			array[2].X = 0.4f;
			array[2].Y = -1f;
			array[3].X = 0.4f;
			array[3].Y = 0.25f;
			val.AddPolygon(array);
			break;
		}
		case eHandStyles.Style3:
		{
			PointF[] array = new PointF[4];
			array[0].X = -0.5f;
			array[0].Y = 0f;
			array[1].X = -0.0125f;
			array[1].Y = -1f;
			array[2].X = 0.0125f;
			array[2].Y = -1f;
			array[3].X = 0.5f;
			array[3].Y = 0f;
			val.AddPolygon(array);
			break;
		}
		case eHandStyles.Style4:
		{
			val.FillMode = (FillMode)1;
			PointF[] array = new PointF[4];
			array[0].X = -0.5f;
			array[0].Y = -0.05f;
			array[1].X = -0.5f;
			array[1].Y = -1f;
			array[2].X = 0.5f;
			array[2].Y = -1f;
			array[3].X = 0.5f;
			array[3].Y = -0.05f;
			val.AddPolygon(array);
			RectangleF rectangleF = new RectangleF(-5f, -0.06f, 10f, 0.12f);
			val.AddEllipse(rectangleF);
			break;
		}
		}
		float num = Math.Min(boundingRect.Width, boundingRect.Height) / 2f;
		Matrix val2 = new Matrix();
		val2.Translate(boundingRect.X + boundingRect.Width * 0.5f, boundingRect.Y + boundingRect.Width * 0.5f);
		val2.Rotate(angle);
		val2.Scale(num * Width, num * Length);
		val.Transform(val2);
		val2.Dispose();
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
