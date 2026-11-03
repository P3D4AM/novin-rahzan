using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiSeriesElementGeom : StiCellGeom, IStiSeriesElement
{
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

	public bool IsSelected
	{
		get
		{
			if (!((StiChart)Series.Chart).IsDashboard)
			{
				return false;
			}
			string hashKey = GetHashKey();
			if (hashKey == null)
			{
				return false;
			}
			return ((StiChart)Series.Chart).Core.GetIsSelectedSeriesElement(hashKey);
		}
		set
		{
			string hashKey = GetHashKey();
			if (hashKey != null)
			{
				((StiChart)Series.Chart).Core.SetIsSelectedSeriesElement(hashKey, value);
			}
		}
	}

	public StiBrush SeriesBrush { get; }

	public double Value { get; }

	public int Index { get; }

	public IStiSeries Series { get; }

	public virtual StiSeriesInteractionData Interaction { get; set; }

	public StiAreaGeom AreaGeom { get; set; }

	public string ElementIndex { get; set; }

	public override void InvokeMouseEnter(StiInteractionOptions options)
	{
		if (AllowMouseOver)
		{
			if (!IsMouseOver)
			{
				IsMouseOver = true;
				options.UpdateContext = Series.Interaction.DrillDownEnabled;
			}
			int valueIndex = GetValueIndex();
			options.SeriesInteractionData = Interaction;
			options.InteractionToolTip = GetToolTip(valueIndex);
			options.InteractionHyperlink = GetHyperlink(valueIndex);
		}
	}

	public override void InvokeMouseLeave(StiInteractionOptions options)
	{
		if (AllowMouseOver && IsMouseOver)
		{
			IsMouseOver = false;
			options.UpdateContext = Series.Interaction.DrillDownEnabled;
			options.SeriesInteractionData = Interaction;
		}
	}

	public override void InvokeClick(StiInteractionOptions options)
	{
		int valueIndex = GetValueIndex();
		if (Series.Hyperlinks != null && valueIndex < Series.Hyperlinks.Length)
		{
			options.InteractionHyperlink = Series.Hyperlinks[valueIndex];
		}
		if (Series.Interaction.DrillDownEnabled)
		{
			options.SeriesInteractionData = Interaction;
			IsMouseOver = false;
			options.UpdateContext = Series.Interaction.DrillDownEnabled;
		}
		if (options.SelectOnClick)
		{
			IsSelected = !IsSelected;
		}
	}

	private int GetValueIndex()
	{
		int num = Index;
		if ((Series is IStiClusteredBarSeries && !(Series is IStiGanttSeries)) || Series is IStiStackedBarSeries || Series is IStiFullStackedBarSeries)
		{
			if (Series.Chart.Area is IStiAxisArea && !((IStiAxisArea)Series.Chart.Area).ReverseVert)
			{
				num = Series.Values.Length - num - 1;
			}
		}
		else if (Series.Chart.Area is IStiAxisArea && ((IStiAxisArea)Series.Chart.Area).ReverseHor)
		{
			num = Series.Values.Length - num - 1;
		}
		return num;
	}

	internal string GetHyperlink()
	{
		return GetHyperlink(GetValueIndex());
	}

	private string GetHyperlink(int valueIndex)
	{
		if (Series.Hyperlinks != null && valueIndex < Series.Hyperlinks.Length)
		{
			return Series.Hyperlinks[valueIndex];
		}
		return null;
	}

	internal string GetToolTip()
	{
		int valueIndex = GetValueIndex();
		return GetToolTip(valueIndex);
	}

	private string GetToolTip(int valueIndex)
	{
		string text = null;
		if (Series.ToolTips != null && valueIndex < Series.ToolTips.Length)
		{
			text = Series.ToolTips[valueIndex];
		}
		if (!string.IsNullOrEmpty(text) && text.Contains("\"StiColor\":\"#ffffff\""))
		{
			Color c = StiBrush.ToColor(SeriesBrush);
			text = text.Replace("\"StiColor\":\"#ffffff\"", "\"StiColor\":\"" + ColorTranslator.ToHtml(c) + "\"");
		}
		return text;
	}

	internal string GetHashKey()
	{
		if (Interaction == null)
		{
			return null;
		}
		return $"{Interaction.Argument}-{Interaction.Value.GetValueOrDefault()}-{Series.CoreTitle}";
	}

	public override void Draw(StiContext context)
	{
	}

	internal StiInteractionDataGeom GetInteractionData()
	{
		StiChart stiChart = Series.Chart as StiChart;
		return new StiInteractionDataGeom
		{
			ComponentName = stiChart.Name,
			ComponentIndex = ((stiChart.Page != null) ? stiChart.Page.Components.IndexOf(stiChart).ToString() : ""),
			PageGuid = ((StiSeries)Series).DrillDownPageGuid,
			PageIndex = ((stiChart.Page != null) ? stiChart.Page.Report.RenderedPages.IndexOf(stiChart.Page).ToString() : ""),
			ElementIndex = ElementIndex,
			InteractionData = Interaction
		};
	}

	internal float GetSeriesBorderThickness(float zoom)
	{
		if (Series is IStiSeriesBorderThickness stiSeriesBorderThickness)
		{
			return (float)stiSeriesBorderThickness.BorderThickness * zoom;
		}
		return 1f;
	}

	public StiSeriesElementGeom(StiAreaGeom areaGeom, double value, int index, IStiSeries series, RectangleF clientRectangle, StiBrush brush)
		: base(clientRectangle)
	{
		SeriesBrush = brush;
		AreaGeom = areaGeom;
		Series = series;
		Value = value;
		Index = index;
	}
}
