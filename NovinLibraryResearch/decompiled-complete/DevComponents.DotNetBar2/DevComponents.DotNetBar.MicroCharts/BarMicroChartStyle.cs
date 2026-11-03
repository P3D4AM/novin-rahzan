using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;

namespace DevComponents.DotNetBar.MicroCharts;

[TypeConverter(typeof(ExpandableObjectConverter))]
[DesignTimeVisible(false)]
[ToolboxItem(false)]
public class BarMicroChartStyle
{
	private EventHandler ވ;

	private int __2CFA = 2;

	private Color __2CFB = Color.Black;

	private Color __2CFC = Color.Red;

	private bool __2CFD;

	private double _ⳳ;

	private Color __2CFE = Color.Empty;

	private Color __2CFF = Color.Empty;

	[Category("Appearance")]
	[Description("Indicates minimum single bar width.")]
	[DefaultValue(2)]
	public int MinBarSize
	{
		get
		{
			return __2CFA;
		}
		set
		{
			__2CFA = value;
			ߓ();
		}
	}

	[Description("Indicates color of positive bar value.")]
	[Category("Appearance")]
	public Color PositiveBarColor
	{
		get
		{
			return __2CFB;
		}
		set
		{
			__2CFB = value;
			ߓ();
		}
	}

	[Category("Appearance")]
	[Description("Indicates color of negative bar value.")]
	public Color NegativeBarColor
	{
		get
		{
			return __2CFC;
		}
		set
		{
			__2CFC = value;
			ߓ();
		}
	}

	[DefaultValue(false)]
	[Category("Appearance")]
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

	[DefaultValue(0.0)]
	[Description("Indicates value of the zero line, i.e. pivot point that determines negative and positive values.")]
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

	[Description("Indicates color of lowest bar on graph.")]
	[Category("Columns")]
	public Color LowPointBarColor
	{
		get
		{
			return __2CFE;
		}
		set
		{
			__2CFE = value;
			ߓ();
		}
	}

	[Category("Columns")]
	[Description("Indicates color of highest value bar on graph..")]
	public Color HighPointBarColor
	{
		get
		{
			return __2CFF;
		}
		set
		{
			__2CFF = value;
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
	public bool ShouldSerializePositiveBarColor()
	{
		return __2CFB != Color.Black;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetPositiveBarColor()
	{
		PositiveBarColor = Color.Black;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeNegativeBarColor()
	{
		return __2CFC != Color.Red;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetNegativeBarColor()
	{
		NegativeBarColor = Color.Red;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeLowPointBarColor()
	{
		return !__2CFE.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetLowPointBarColor()
	{
		LowPointBarColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeHighPointBarColor()
	{
		return !__2CFF.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetHighPointBarColor()
	{
		HighPointBarColor = Color.Empty;
	}
}
