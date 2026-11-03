using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiLegendItemGeom : StiCellGeom
{
	private IStiLegend legend;

	private StiLegendItemCoreXF item;

	private int colorIndex;

	private int legendItemsCount;

	private int legendItemIndex;

	public virtual bool AllowMouseOver
	{
		get
		{
			if (Item != null && Item.Series != null && Item.Series.Interaction != null && Item.Series.Interaction.AllowSeries)
			{
				return Item.Series.Interaction.DrillDownEnabled;
			}
			return false;
		}
	}

	private bool IsColorEach => Item.Series.Chart.Area.ColorEach;

	public virtual bool IsMouseOver
	{
		get
		{
			if (IsColorEach)
			{
				if (Item.Index == -1)
				{
					return false;
				}
				return Item.Series.Core.GetIsMouseOverSeriesElement(Item.Index);
			}
			return Item.Series.Core.IsMouseOver;
		}
		set
		{
			if (IsColorEach)
			{
				if (Item.Index != -1)
				{
					Item.Series.Core.SetIsMouseOverSeriesElement(Item.Index, value);
				}
			}
			else
			{
				Item.Series.Core.IsMouseOver = value;
			}
		}
	}

	internal bool IsSelected
	{
		get
		{
			if (!((StiChart)Item.Series.Chart).IsDashboard)
			{
				return false;
			}
			return ((StiChart)Item.Series.Chart).Core.GetIsSelectedSeriesElement($"legend{Item.Index}");
		}
		set
		{
			((StiChart)Item.Series.Chart).Core.SetIsSelectedSeriesElement($"legend{Item.Index}", value);
		}
	}

	public IStiLegend Legend => legend;

	public StiLegendItemCoreXF Item => item;

	public int ColorIndex => colorIndex;

	public int LegendItemsCount => legendItemsCount;

	public override void InvokeMouseEnter(StiInteractionOptions options)
	{
		if (AllowMouseOver && !IsMouseOver)
		{
			IsMouseOver = true;
			options.UpdateContext = true;
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
		if (Item.Series.Interaction != null && Item.Series.Interaction.DrillDownEnabled && Item.Series.Interaction.AllowSeries)
		{
			StiSeriesInteractionData stiSeriesInteractionData = new StiSeriesInteractionData();
			if (IsColorEach)
			{
				stiSeriesInteractionData.Fill(Item.Series.Chart.Area, Item.Series, Item.Index);
				stiSeriesInteractionData.IsElements = true;
			}
			else
			{
				stiSeriesInteractionData.Series = Item.Series;
				stiSeriesInteractionData.IsElements = false;
			}
			IsMouseOver = false;
			options.UpdateContext = true;
			options.SeriesInteractionData = stiSeriesInteractionData;
			if (IsColorEach && options.SelectOnClick)
			{
				IsSelected = !IsSelected;
			}
		}
	}

	public override void Draw(StiContext context)
	{
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Expected O, but got Unknown
		RectangleF rect = base.ClientRectangle;
		if (legend.MarkerVisible)
		{
			RectangleF rect2 = new RectangleF(base.ClientRectangle.X, base.ClientRectangle.Y + (base.ClientRectangle.Height - (float)legend.MarkerSize.Height * context.Options.Zoom) / 2f, (float)legend.MarkerSize.Width * context.Options.Zoom, (float)legend.MarkerSize.Height * context.Options.Zoom);
			if (legend.MarkerAlignment == StiMarkerAlignment.Right)
			{
				rect2.X = base.ClientRectangle.Right - rect2.Width;
			}
			IStiLegendMarker stiLegendMarker = StiMarkerLegendFactory.CreateMarker(Item.Series);
			if (rect2.Width > 0f && rect2.Height > 0f)
			{
				stiLegendMarker.Draw(context, Item.Series, rect2, ColorIndex, LegendItemsCount, legendItemIndex);
			}
			if (legend.MarkerAlignment == StiMarkerAlignment.Left)
			{
				rect.X += (float)legend.MarkerSize.Width * context.Options.Zoom;
			}
			rect.Width -= (float)legend.MarkerSize.Width * context.Options.Zoom;
		}
		rect.X += 2f;
		rect.Width += 4f;
		StiSolidBrush brush = new StiSolidBrush(Legend.LabelsColor);
		StiFontGeom font = StiFontGeom.ChangeFontSize(Legend.Font, Legend.Font.Size * context.Options.Zoom);
		StiStringFormatGeom defaultStringFormat = context.GetDefaultStringFormat();
		defaultStringFormat.Alignment = (StringAlignment)0;
		defaultStringFormat.LineAlignment = (StringAlignment)1;
		defaultStringFormat.FormatFlags = (StringFormatFlags)0;
		if (legend.MarkerAlignment == StiMarkerAlignment.Right)
		{
			defaultStringFormat.FormatFlags = (StringFormatFlags)4096;
		}
		context.DrawString(Item.Text, font, brush, rect, defaultStringFormat, legendItemIndex);
	}

	public StiLegendItemGeom(IStiLegend legend, StiLegendItemCoreXF item, RectangleF clientRectangle, int colorIndex, int legendItemsCount, int legendItemIndex)
		: base(clientRectangle)
	{
		this.legend = legend;
		this.item = item;
		this.colorIndex = colorIndex;
		this.legendItemsCount = legendItemsCount;
		this.legendItemIndex = legendItemIndex;
	}
}
