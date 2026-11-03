using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;

namespace DevComponents.DotNetBar.MicroCharts;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
[TypeConverter(typeof(ExpandableObjectConverter))]
public class PlotMicroChartStyle
{
	private EventHandler ވ;

	private Color _ⵌ = Color.Black;

	private bool __2CFD;

	private Color _ⴈ = ColorScheme.GetColor(16433295);

	private bool _ⴉ;

	private double _ⳳ;

	private Color _ⴊ = Color.Red;

	private Color __2CF5 = Color.Empty;

	private Color __2CF6 = Color.Empty;

	private Color __2CF7 = Color.Empty;

	private Color __2CF8 = Color.Empty;

	private bool _ⴋ;

	private Color _ⴌ = Color.Empty;

	private double _ⴍ;

	private double _ⴎ;

	private bool _ⴏ;

	private Color _ⴐ = Color.Empty;

	private double _ⴑ;

	private double _ⴒ;

	[Description("Indicates color of chart line .")]
	[Category("Appearance")]
	public Color PlotColor
	{
		get
		{
			return _ⵌ;
		}
		set
		{
			_ⵌ = value;
			ߓ();
		}
	}

	[Category("Appearance")]
	[DefaultValue(false)]
	public bool DrawAverageLine
	{
		get
		{
			return __2CFD;
		}
		set
		{
			__2CFD = value;
			ߓ();
		}
	}

	[Description("Indicates color of.")]
	[Category("Columns")]
	public Color AverageLineColor
	{
		get
		{
			return _ⴈ;
		}
		set
		{
			_ⴈ = value;
			ߓ();
		}
	}

	[DefaultValue(false)]
	[Description("Gets or sets whether zero-line is drawn on chart.")]
	[Category("Appearance")]
	public bool DrawZeroLine
	{
		get
		{
			return _ⴉ;
		}
		set
		{
			_ⴉ = value;
			ߓ();
		}
	}

	[Category("Appearance")]
	[DefaultValue(0.0)]
	[Description("Indicates value of the zero line, i.e. where zero line is drawn.")]
	public double ZeroLineValue
	{
		get
		{
			return _ⳳ;
		}
		set
		{
			_ⳳ = value;
			ߓ();
		}
	}

	[Description("Indicates color of.")]
	[Category("Columns")]
	public Color ZeroLineColor
	{
		get
		{
			return _ⴊ;
		}
		set
		{
			_ⴊ = value;
			ߓ();
		}
	}

	[Description("Indicates color of high point dot on chart..")]
	[Category("Columns")]
	public Color HighPointColor
	{
		get
		{
			return __2CF5;
		}
		set
		{
			__2CF5 = value;
			ߓ();
		}
	}

	[Category("Columns")]
	[Description("Indicates color of low point dot on chart.")]
	public Color LowPointColor
	{
		get
		{
			return __2CF6;
		}
		set
		{
			__2CF6 = value;
			ߓ();
		}
	}

	[Description("Indicates color of first point dot on chart..")]
	[Category("Columns")]
	public Color FirstPointColor
	{
		get
		{
			return __2CF7;
		}
		set
		{
			__2CF7 = value;
			ߓ();
		}
	}

	[Description("Indicates color of last point dot on chart.")]
	[Category("Columns")]
	public Color LastPointColor
	{
		get
		{
			return __2CF8;
		}
		set
		{
			__2CF8 = value;
			ߓ();
		}
	}

	[DefaultValue(false)]
	[Description("Indicates whether control line is drawn. Control lines can be used to display for example low and high control bounds for the chart")]
	public bool DrawControlLine1
	{
		get
		{
			return _ⴋ;
		}
		set
		{
			_ⴋ = value;
			ߓ();
		}
	}

	[Description("Indicates color of.")]
	[Category("Columns")]
	public Color ControlLine1Color
	{
		get
		{
			return _ⴌ;
		}
		set
		{
			_ⴌ = value;
			ߓ();
		}
	}

	[Description("Indicates the starting value that is used to draw first control line.")]
	[DefaultValue(0.0)]
	public double ControlLine1StartValue
	{
		get
		{
			return _ⴍ;
		}
		set
		{
			_ⴍ = value;
			ߓ();
		}
	}

	[Description("Indicates the end value that is used to draw first control line.")]
	[DefaultValue(0.0)]
	public double ControlLine1EndValue
	{
		get
		{
			return _ⴎ;
		}
		set
		{
			_ⴎ = value;
			ߓ();
		}
	}

	[DefaultValue(false)]
	[Description("Indicates whether control line is drawn. Control lines can be used to display for example low and high control bounds for the chart")]
	public bool DrawControlLine2
	{
		get
		{
			return _ⴏ;
		}
		set
		{
			_ⴏ = value;
			ߓ();
		}
	}

	[Category("Columns")]
	[Description("Indicates color of second control line.")]
	public Color ControlLine2Color
	{
		get
		{
			return _ⴐ;
		}
		set
		{
			_ⴐ = value;
			ߓ();
		}
	}

	[DefaultValue(0.0)]
	[Description("Indicates the starting value that is used to draw first second line.")]
	public double ControlLine2StartValue
	{
		get
		{
			return _ⴑ;
		}
		set
		{
			_ⴑ = value;
			ߓ();
		}
	}

	[DefaultValue(0.0)]
	[Description("Indicates the end value that is used to draw second control line.")]
	public double ControlLine2EndValue
	{
		get
		{
			return _ⴒ;
		}
		set
		{
			_ⴒ = value;
			ߓ();
		}
	}

	public event EventHandler StyleChanged
	{
		add
		{
			EventHandler eventHandler = ވ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ވ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ވ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ވ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	protected virtual void OnStyleChanged(EventArgs e)
	{
		ވ?.Invoke(this, e);
	}

	private void ߓ()
	{
		OnStyleChanged(EventArgs.Empty);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializePlotColor()
	{
		return _ⵌ != Color.Black;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetPlotColor()
	{
		PlotColor = Color.Black;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeAverageLineColor()
	{
		return _ⴈ != ColorScheme.GetColor(16433295);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetAverageLineColor()
	{
		AverageLineColor = ColorScheme.GetColor(16433295);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeZeroLineColor()
	{
		return _ⴊ != Color.Red;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetZeroLineColor()
	{
		ZeroLineColor = Color.Red;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeHighPointColor()
	{
		return !__2CF5.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetHighPointColor()
	{
		HighPointColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeLowPointColor()
	{
		return !__2CF6.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetLowPointColor()
	{
		LowPointColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeFirstPointColor()
	{
		return !__2CF7.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetFirstPointColor()
	{
		FirstPointColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeLastPointColor()
	{
		return !__2CF8.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetLastPointColor()
	{
		LastPointColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeControlLine1Color()
	{
		return !_ⴌ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetControlLine1Color()
	{
		ControlLine1Color = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeControlLine2Color()
	{
		return !_ⴐ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetControlLine2Color()
	{
		ControlLine2Color = Color.Empty;
	}
}
