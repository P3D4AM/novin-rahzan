using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiMarkerGeom : StiCellGeom, IStiSeriesElement
{
	private StiSeriesInteractionData interaction;

	private int index;

	private PointF point;

	private IStiMarker marker;

	private double value;

	private bool showShadow;

	private bool isTooltipMode;

	private IStiSeries series;

	private string elementIndex;

	public virtual bool AllowMouseOver
	{
		get
		{
			int valueIndex = GetValueIndex();
			if (GetHyperlink(GetValueIndex()) == null && (Series.ToolTips == null || valueIndex >= Series.ToolTips.Length))
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
			if (Series == null)
			{
				return false;
			}
			return Series.Core.GetIsMouseOverSeriesElement(Index);
		}
		set
		{
			if (Series != null)
			{
				Series.Core.SetIsMouseOverSeriesElement(Index, value);
			}
		}
	}

	public virtual bool IsSelected
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

	public int Index => index;

	public PointF Point => point;

	public IStiMarker Marker => marker;

	public double Value => value;

	public bool ShowShadow => showShadow;

	public bool IsTooltipMode => isTooltipMode;

	public IStiSeries Series => series;

	public string ElementIndex
	{
		get
		{
			return elementIndex;
		}
		set
		{
			elementIndex = value;
		}
	}

	public override void InvokeMouseEnter(StiInteractionOptions options)
	{
		if (AllowMouseOver)
		{
			if (!IsMouseOver)
			{
				IsMouseOver = true;
				options.UpdateContext = true;
			}
			int valueIndex = GetValueIndex();
			options.InteractionToolTip = GetToolTip(valueIndex);
			options.InteractionHyperlink = GetHyperlink(valueIndex);
		}
	}

	public override void InvokeMouseLeave(StiInteractionOptions options)
	{
		if (AllowMouseOver && IsMouseOver)
		{
			IsMouseOver = false;
			options.UpdateContext = true;
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
			options.UpdateContext = true;
		}
		if (options.SelectOnClick)
		{
			IsSelected = !IsSelected;
		}
	}

	private int GetValueIndex()
	{
		int num = Index;
		if (Series is IStiClusteredBarSeries || Series is IStiStackedBarSeries || Series is IStiFullStackedBarSeries)
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
			return series.Hyperlinks[valueIndex];
		}
		return null;
	}

	internal string GetToolTip()
	{
		return GetToolTip(GetValueIndex());
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
			Color c = StiBrush.ToColor(series.ProcessSeriesBrushes(index, Marker.Brush));
			text = text.Replace("\"StiColor\":\"#ffffff\"", "\"StiColor\":\"" + ColorTranslator.ToHtml(c) + "\"");
		}
		return text;
	}

	private string GetHashKey()
	{
		if (Interaction == null)
		{
			return null;
		}
		return $"{Interaction.Argument}-{Interaction.Value}-{Series.CoreTitle}";
	}

	public override bool Contains(float x, float y)
	{
		if (Invisible)
		{
			return false;
		}
		return GetMouseOverRect().Contains(x, y);
	}

	internal RectangleF GetMouseOverRect()
	{
		RectangleF result = base.ClientRectangle;
		result.Inflate(result.Width / 2f, result.Height / 2f);
		return result;
	}

	public override void Draw(StiContext context)
	{
		if (!(Marker.Size <= 0f))
		{
			context.PushSmoothingModeToAntiAlias();
			float zoom = context.Options.Zoom;
			if (IsMouseOver)
			{
				context.FillEllipse(StiMouseOverHelper.GetLineMouseOverColor(Marker.Brush), GetMouseOverRect());
			}
			StiChart stiChart = Series.Chart as StiChart;
			IStiMarker stiMarker = (IStiMarker)Marker.Clone();
			bool flag = series.ProcessSeriesMarkerVisible(index) || Marker.Visible;
			if (!IsTooltipMode || flag)
			{
				stiMarker.Brush = series.ProcessSeriesBrushes(index, Marker.Brush);
				stiMarker.Angle = series.ProcessSeriesMarkerAngle(index, Marker.Angle);
				stiMarker.Type = series.ProcessSeriesMarkerType(index, Marker.Type);
				stiMarker.Visible = flag;
			}
			if (IsTooltipMode && !flag)
			{
				stiMarker.Brush = null;
				stiMarker.BorderColor = Color.Transparent;
			}
			StiInteractionDataGeom stiInteractionDataGeom = (stiChart.IsAnimation ? new StiInteractionDataGeom
			{
				ComponentName = stiChart.Name,
				ComponentIndex = ((stiChart.Page != null) ? stiChart.Page.Components.IndexOf(stiChart).ToString() : ""),
				PageGuid = ((StiSeries)Series).DrillDownPageGuid,
				PageIndex = ((stiChart.Page != null) ? stiChart.Page.Report.RenderedPages.IndexOf(stiChart.Page).ToString() : ""),
				ElementIndex = ElementIndex.ToString(),
				InteractionData = Interaction
			} : null);
			Marker.Core.Draw(context, stiMarker, Point, zoom, ShowShadow, IsMouseOver || Series.Core.IsMouseOver, isTooltipMode, stiChart.IsAnimation, GetToolTip(), this, stiInteractionDataGeom);
			context.PopSmoothingMode();
		}
	}

	public StiMarkerGeom(IStiSeries series, int index, double value, PointF point, IStiMarker marker, bool showShadow, float zoom, bool isTooltipMode)
		: base(StiMarkerCoreXF.GetMarkerRect(point, marker.Size, zoom))
	{
		this.series = series;
		this.index = index;
		this.value = value;
		this.point = point;
		this.marker = marker;
		this.showShadow = showShadow;
		this.isTooltipMode = isTooltipMode;
	}
}
