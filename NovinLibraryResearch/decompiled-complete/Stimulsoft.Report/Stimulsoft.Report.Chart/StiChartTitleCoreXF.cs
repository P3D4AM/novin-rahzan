using System;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiChartTitleCoreXF : ICloneable, IStiApplyStyle
{
	private IStiChartTitle chartTitle;

	public IStiChartTitle ChartTitle
	{
		get
		{
			return chartTitle;
		}
		set
		{
			chartTitle = value;
		}
	}

	public object Clone()
	{
		return MemberwiseClone();
	}

	public virtual void ApplyStyle(IStiChartStyle style)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		if (ChartTitle.AllowApplyStyle)
		{
			ChartTitle.Brush = (StiBrush)new StiSolidBrush(style.Core.AxisTitleColor);
		}
	}

	public StiCellGeom Render(StiContext context, IStiChartTitle chartTitle, RectangleF rect)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Invalid comparison between Unknown and I4
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Invalid comparison between Unknown and I4
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Invalid comparison between Unknown and I4
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Invalid comparison between Unknown and I4
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Invalid comparison between Unknown and I4
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Invalid comparison between Unknown and I4
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0306: Unknown result type (might be due to invalid IL or missing references)
		//IL_030c: Invalid comparison between Unknown and I4
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Invalid comparison between Unknown and I4
		if (!chartTitle.Visible)
		{
			return null;
		}
		StiFontGeom font = new StiFontGeom(chartTitle.Font);
		StiStringFormatGeom defaultStringFormat = context.GetDefaultStringFormat();
		defaultStringFormat.Alignment = chartTitle.Alignment;
		RectangleF clientRectangle = context.MeasureRotatedString(chartTitle.Text, font, rect, defaultStringFormat, (StiRotationMode)4, (float)chartTitle.Dock);
		switch (chartTitle.Dock)
		{
		case StiChartTitleDock.Top:
			clientRectangle.Height += chartTitle.Spacing;
			clientRectangle.Height *= context.Options.Zoom;
			clientRectangle.Y = rect.Y;
			if ((int)chartTitle.Alignment != 1)
			{
				clientRectangle.Width *= context.Options.Zoom;
			}
			if ((int)chartTitle.Alignment == 0)
			{
				clientRectangle.X = rect.X;
			}
			if ((int)chartTitle.Alignment == 2)
			{
				clientRectangle.X = rect.X + rect.Width - clientRectangle.Width;
			}
			break;
		case StiChartTitleDock.Right:
			clientRectangle.Width += chartTitle.Spacing;
			clientRectangle.Width *= context.Options.Zoom;
			clientRectangle.X = rect.X + rect.Width - clientRectangle.Width;
			if ((int)chartTitle.Alignment != 1)
			{
				clientRectangle.Height *= context.Options.Zoom;
			}
			if ((int)chartTitle.Alignment == 0)
			{
				clientRectangle.Y = rect.Y;
			}
			if ((int)chartTitle.Alignment == 2)
			{
				clientRectangle.Y = rect.Y + rect.Height - clientRectangle.Height;
			}
			break;
		case StiChartTitleDock.Bottom:
			clientRectangle.Height += chartTitle.Spacing;
			clientRectangle.Height *= context.Options.Zoom;
			clientRectangle.Y = rect.Y + rect.Height - clientRectangle.Height;
			if ((int)chartTitle.Alignment != 1)
			{
				clientRectangle.Width *= context.Options.Zoom;
			}
			if ((int)chartTitle.Alignment == 0)
			{
				clientRectangle.X = rect.X + rect.Width - clientRectangle.Width;
			}
			if ((int)chartTitle.Alignment == 2)
			{
				clientRectangle.X = rect.X;
			}
			break;
		case StiChartTitleDock.Left:
			clientRectangle.Width += chartTitle.Spacing;
			clientRectangle.Width *= context.Options.Zoom;
			clientRectangle.X = rect.X;
			if ((int)chartTitle.Alignment != 1)
			{
				clientRectangle.Height *= context.Options.Zoom;
			}
			if ((int)chartTitle.Alignment == 0)
			{
				clientRectangle.Y = rect.Y + rect.Height - clientRectangle.Height;
			}
			if ((int)chartTitle.Alignment == 2)
			{
				clientRectangle.Y = rect.Y;
			}
			break;
		}
		return new StiChartTitleGeom(chartTitle, clientRectangle);
	}

	public StiChartTitleCoreXF(IStiChartTitle chartTitle)
	{
		this.chartTitle = chartTitle;
	}
}
