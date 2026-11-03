using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar.MicroCharts;

namespace DevComponents.DotNetBar;

[DefaultEvent("Click")]
[ToolboxBitmap(typeof(MicroChart), "MicroCharts.MicroChart.ico")]
[ComVisible(false)]
[Designer("DevComponents.DotNetBar.Design.MicroChartDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[ToolboxItem(true)]
public class MicroChart : BaseItemControl, ICommandSource
{
	private MicroChartItem _ⴓ;

	private EventHandler ⴔ;

	private ICommand _૦;

	private object _૧;

	protected override Size DefaultSize => new Size(96, 24);

	[DefaultValue(true)]
	[Description("Indicates whether mouse over tooltip for data point is enabled")]
	[Category("Behavior")]
	public bool MouseOverDataPointTooltipEnabled
	{
		get
		{
			return _ⴓ.MouseOverDataPointTooltipEnabled;
		}
		set
		{
			_ⴓ.MouseOverDataPointTooltipEnabled = value;
		}
	}

	[Browsable(false)]
	public int MouseOverDataPointIndex => _ⴓ.MouseOverDataPointIndex;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public List<string> DataPointTooltips
	{
		get
		{
			return _ⴓ.DataPointTooltips;
		}
		set
		{
			_ⴓ.DataPointTooltips = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<double> DataPoints
	{
		get
		{
			return _ⴓ.DataPoints;
		}
		set
		{
			if (value != _ⴓ.DataPoints)
			{
				List<double> dataPoints = _ⴓ.DataPoints;
				_ⴓ.DataPoints = value;
				ⴖ(dataPoints, value);
			}
		}
	}

	[Category("Appearance")]
	[DefaultValue("")]
	[Description("Indicates format string for the value when it is displayed as tool-tip for data point.")]
	public string TooltipValueFormatString
	{
		get
		{
			return _ⴓ.TooltipValueFormatString;
		}
		set
		{
			_ⴓ.TooltipValueFormatString = value;
		}
	}

	[Category("Behavior")]
	[Description("Indicates whether chart is tracking mouse movement to show data-point and its value on tooltip.")]
	[DefaultValue(true)]
	public bool TrackChartPoints
	{
		get
		{
			return _ⴓ.TrackChartPoints;
		}
		set
		{
			_ⴓ.TrackChartPoints = value;
		}
	}

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates  whether transition animation between same chart with different data-points is enabled.")]
	public bool AnimationEnabled
	{
		get
		{
			return _ⴓ.AnimationEnabled;
		}
		set
		{
			_ⴓ.AnimationEnabled = value;
		}
	}

	internal bool ᥧ
	{
		get
		{
			if (((Component)(object)this).DesignMode || _089F.ष())
			{
				return false;
			}
			return true;
		}
	}

	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates whether button like mouse over tracking is enabled for whole control. When enabled control looks like a button when mouse is over it.")]
	public bool MouseOverEnabled
	{
		get
		{
			return _ⴓ.MouseOverEnabled;
		}
		set
		{
			_ⴓ.MouseOverEnabled = value;
		}
	}

	[Category("Style")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Identifies style used to customize appearance of Line micro-chart.")]
	public LineMicroChartStyle LineChartStyle => _ⴓ.LineChartStyle;

	[Description("Identifies style used to customize appearance of Plot micro-chart.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Style")]
	public PlotMicroChartStyle PlotChartStyle => _ⴓ.PlotChartStyle;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Style")]
	[Description("Identifies style used to customize appearance of Column micro-chart.")]
	public BarMicroChartStyle ColumnChartStyle => _ⴓ.ColumnChartStyle;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Identifies style used to customize appearance of Bar micro-chart.")]
	[Category("Style")]
	public BarMicroChartStyle BarChartStyle => _ⴓ.BarChartStyle;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Style")]
	[Description("Identifies style used to customize appearance of Win-Lose micro-chart.")]
	public BarMicroChartStyle WinLoseChartStyle => _ⴓ.WinLoseChartStyle;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Style")]
	[Description("Identifies style used to customize appearance of Pie micro-chart.")]
	public PieMicroChartStyle PieChartStyle => _ⴓ.PieChartStyle;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Identifies style used to customize appearance of Area micro-chart.")]
	[Category("Style")]
	public AreaMicroChartStyle AreaChartStyle => _ⴓ.AreaChartStyle;

	[Description("Identifies style used to customize appearance of 100% micro-chart.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Style")]
	public HundredPctMicroChartStyle HundredPctChartStyle => _ⴓ.HundredPctChartStyle;

	[DefaultValue(eMicroChartType.Line)]
	[Description("Indicates type of the chart rendered.")]
	[Category("Appearance")]
	public eMicroChartType ChartType
	{
		get
		{
			return _ⴓ.ChartType;
		}
		set
		{
			_ⴓ.ChartType = value;
		}
	}

	[Category("Behavior")]
	[Description("Indicates maximum value for data points. By default maximum data point is calculated based on that data displayed by the chart, but when two charts need to be scaled the same setting maximum and minimum values for them will ensure that scales are visually the same.")]
	[DefaultValue(double.NaN)]
	public double DataMaxValue
	{
		get
		{
			return _ⴓ.DataMaxValue;
		}
		set
		{
			_ⴓ.DataMaxValue = value;
		}
	}

	[DefaultValue(double.NaN)]
	[Category("Behavior")]
	[Description("Indicates minimum value for data points. By default minimum data point is calculated based on that data displayed by the chart, but when two charts need to be scaled the same setting maximum and minimum values for them will ensure that scales are visually the same.")]
	public double DataMinValue
	{
		get
		{
			return _ⴓ.DataMinValue;
		}
		set
		{
			_ⴓ.DataMinValue = value;
		}
	}

	protected virtual bool ExecuteCommandOnClick => true;

	[DefaultValue(null)]
	[Category("Commands")]
	[Description("Indicates the command assigned to the item.")]
	public Command Command
	{
		get
		{
			return (Command)((ICommandSource)this).Command;
		}
		set
		{
			((ICommandSource)this).Command = value;
		}
	}

	ICommand ICommandSource.ᒍ
	{
		get
		{
			return _૦;
		}
		set
		{
			bool flag = false;
			if (_૦ != value)
			{
				flag = true;
			}
			if (_૦ != null)
			{
				CommandManager.UnRegisterCommandSource(this, _૦);
			}
			_૦ = value;
			if (value != null)
			{
				CommandManager.RegisterCommand(this, value);
			}
			if (flag)
			{
				OnCommandChanged();
			}
		}
	}

	[TypeConverter(typeof(StringConverter))]
	[Localizable(true)]
	[DefaultValue(null)]
	[Browsable(true)]
	[Description("Indicates user defined data value that can be passed to the command when it is executed.")]
	[Category("Commands")]
	public object CommandParameter
	{
		get
		{
			return _૧;
		}
		set
		{
			_૧ = value;
		}
	}

	public event EventHandler MouseOverDataPointChanged
	{
		add
		{
			EventHandler eventHandler = ⴔ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ⴔ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ⴔ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ⴔ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public MicroChart()
	{
		((Control)this).SetStyle((ControlStyles)512, false);
		_ⴓ = new MicroChartItem();
		_ⴓ.TextVisible = false;
		_ⴓ.MouseOverDataPointChanged += ⴕ;
		HostItem = _ⴓ;
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		RecalcLayout();
		((Control)this).OnHandleCreated(e);
	}

	public override void RecalcLayout()
	{
		if (_ⴓ != null)
		{
			Rectangle clientRectangle = ((Control)this).ClientRectangle;
			clientRectangle.Inflate(-4, -4);
			if (clientRectangle.Width < 4)
			{
				clientRectangle.Width = 4;
			}
			if (clientRectangle.Height < 4)
			{
				clientRectangle.Height = 4;
			}
			_ⴓ.Bounds = clientRectangle;
			_ⴓ.ChartHeight = clientRectangle.Height;
			_ⴓ.ChartWidth = clientRectangle.Width;
			base.RecalcLayout();
		}
	}

	private void ⴕ(object _٠, EventArgs ؿ)
	{
		OnMouseOverDataPointChanged(ؿ);
	}

	protected virtual void OnMouseOverDataPointChanged(EventArgs e)
	{
		ⴔ?.Invoke(this, e);
	}

	private void ⴖ(List<double> ك, List<double> ل)
	{
	}

	public void InvalidateChart(bool animateTransition)
	{
		_ⴓ.InvalidateChart(animateTransition);
	}

	protected virtual void ExecuteCommand()
	{
		if (_૦ != null)
		{
			CommandManager._૭(this);
		}
	}

	protected virtual void OnCommandChanged()
	{
	}
}
