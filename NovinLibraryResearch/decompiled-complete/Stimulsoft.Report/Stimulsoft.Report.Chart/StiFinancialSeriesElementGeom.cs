using System.Drawing;
using Stimulsoft.Base.Context;

namespace Stimulsoft.Report.Chart;

public class StiFinancialSeriesElementGeom : StiCellGeom
{
	private IStiSeries series;

	private StiSeriesInteractionData interaction;

	private float open;

	private float close;

	private float high;

	private float low;

	private float positionX;

	private StiAreaGeom areaGeom;

	private int index;

	public virtual bool AllowMouseOver
	{
		get
		{
			int valueIndex = GetValueIndex();
			if ((Series.Hyperlinks == null || valueIndex >= Series.Hyperlinks.Length) && (Series.ToolTips == null || valueIndex >= Series.ToolTips.Length))
			{
				if (Series.Interaction.DrillDownEnabled)
				{
					return Series.Interaction.AllowSeriesElements;
				}
				return false;
			}
			return true;
		}
	}

	public virtual bool IsMouseOver
	{
		get
		{
			return Series.Core.GetIsMouseOverSeriesElement(Index);
		}
		set
		{
			Series.Core.SetIsMouseOverSeriesElement(Index, value);
		}
	}

	public IStiSeries Series => series;

	public StiSeriesInteractionData Interaction
	{
		get
		{
			return interaction;
		}
		set
		{
			interaction = value;
		}
	}

	public float Open => open;

	public float Close => close;

	public float High => high;

	public float Low => low;

	public float PositionX => positionX;

	public StiAreaGeom AreaGeom => areaGeom;

	public int Index => index;

	public override void InvokeMouseEnter(StiInteractionOptions options)
	{
		if (AllowMouseOver)
		{
			if (!IsMouseOver)
			{
				IsMouseOver = true;
				options.UpdateContext = series.Interaction.DrillDownEnabled;
			}
			int valueIndex = GetValueIndex();
			options.InteractionToolTip = GetToolTip(valueIndex);
			options.InteractionHyperlink = GetHyperlink(valueIndex);
		}
	}

	public override void InvokeClick(StiInteractionOptions options)
	{
		int valueIndex = GetValueIndex();
		if (Series.Hyperlinks != null && valueIndex < Series.Hyperlinks.Length)
		{
			options.InteractionHyperlink = series.Hyperlinks[valueIndex];
		}
		if (Series.Interaction.DrillDownEnabled)
		{
			options.SeriesInteractionData = Interaction;
			IsMouseOver = false;
			options.UpdateContext = series.Interaction.DrillDownEnabled;
		}
	}

	private int GetValueIndex()
	{
		int num = Index;
		if (Series.Chart.Area is IStiAxisArea && ((IStiAxisArea)Series.Chart.Area).ReverseHor)
		{
			num = Series.Arguments.Length - num - 1;
		}
		return num;
	}

	private string GetHyperlink(int valueIndex)
	{
		if (Series.Hyperlinks != null && valueIndex < Series.Hyperlinks.Length)
		{
			return series.Hyperlinks[valueIndex];
		}
		return null;
	}

	private string GetToolTip(int valueIndex)
	{
		if (Series.ToolTips != null && valueIndex < Series.ToolTips.Length)
		{
			return series.ToolTips[valueIndex];
		}
		return null;
	}

	public override void Draw(StiContext context)
	{
	}

	public StiFinancialSeriesElementGeom(StiAreaGeom areaGeom, IStiSeries series, RectangleF clientRectangle, float open, float close, float high, float low, float positionX, int index)
		: base(clientRectangle)
	{
		this.areaGeom = areaGeom;
		this.series = series;
		this.open = open;
		this.close = close;
		this.high = high;
		this.low = low;
		this.positionX = positionX;
		this.index = index;
	}
}
