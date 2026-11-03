using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;

namespace DevComponents.DotNetBar.MicroCharts;

[DesignTimeVisible(false)]
[TypeConverter(typeof(ExpandableObjectConverter))]
[ToolboxItem(false)]
public class AreaMicroChartStyle
{
	private EventHandler ވ;

	private double _ⳳ;

	private Color __2CF4 = ColorScheme.GetColor(9668947);

	private Color __2CF5 = Color.Empty;

	private Color __2CF6 = Color.Empty;

	private Color __2CF7 = Color.Empty;

	private Color __2CF8 = Color.Empty;

	[Description("Indicates value of the zero line, i.e. where zero line is drawn.")]
	[DefaultValue(0.0)]
	[Category("Appearance")]
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

	[Description("Indicates chart area color.")]
	[Category("Columns")]
	public Color AreaColor
	{
		get
		{
			return __2CF4;
		}
		set
		{
			__2CF4 = value;
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

	[Description("Indicates color of low point dot on chart.")]
	[Category("Columns")]
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
	public bool ShouldSerializeAreaColor()
	{
		return __2CF4 != ColorScheme.GetColor(9668947);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetAreaColor()
	{
		AreaColor = ColorScheme.GetColor(9668947);
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
}
