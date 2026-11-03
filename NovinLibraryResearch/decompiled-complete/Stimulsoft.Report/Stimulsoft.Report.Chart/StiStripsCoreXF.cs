using System;
using System.Drawing;
using System.Globalization;
using System.Threading;
using Stimulsoft.Base;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Chart;

public class StiStripsCoreXF : IStiApplyStyle, ICloneable
{
	private IStiStrips strips;

	public IStiStrips Strips
	{
		get
		{
			return strips;
		}
		set
		{
			strips = value;
		}
	}

	public object Clone()
	{
		return MemberwiseClone();
	}

	public void ApplyStyle(IStiChartStyle style)
	{
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		if (Strips.AllowApplyStyle)
		{
			Strips.TitleColor = style.Core.StyleColors[style.Core.StyleColors.Length - 1];
			Strips.StripBrush = (StiBrush)new StiSolidBrush(Color.FromArgb(150, style.Core.StyleColors[style.Core.StyleColors.Length - 1]));
		}
	}

	public void RenderXStrips(StiContext context, StiAxisAreaGeom geom, RectangleF rect)
	{
		if (Strips.Chart.Area is IStiAxisArea stiAxisArea)
		{
			float num = CalculateXValue(Strips.MinValue, stiAxisArea, rect);
			float num2 = CalculateXValue(Strips.MaxValue, stiAxisArea, rect);
			if (stiAxisArea.ReverseHor)
			{
				num = rect.Width - num;
				num2 = rect.Width - num2;
			}
			if (num > num2)
			{
				float num3 = num;
				num = num2;
				num2 = num3;
			}
			StiStripsXGeom item = new StiStripsXGeom(clientRectangle: new RectangleF(num, 0f, num2 - num, rect.Height), strip: Strips);
			geom.CreateChildGeoms();
			geom.ChildGeoms.Add(item);
		}
	}

	private float CalculateXValue(string xValue, IStiAxisArea area, RectangleF rect)
	{
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
		xValue = StiReportParser.Parse(xValue, (IStiReportComponent)(object)(area.Chart as StiChart));
		float result = 0f;
		float.TryParse(xValue.Replace(",", "."), out result);
		Thread.CurrentThread.CurrentCulture = currentCulture;
		float num = 0f;
		if (area.XAxis.LogarithmicScale && area.XAxis.Info.StripLines.Count > 0)
		{
			int count = area.XAxis.Info.StripLines.Count;
			int index = (area.ReverseHor ? (count - 1) : 0);
			int index2 = ((!area.ReverseHor) ? (count - 1) : 0);
			double value = area.XAxis.Info.StripLines[index].Value;
			double value2 = area.XAxis.Info.StripLines[index2].Value;
			double num2 = Math.Abs((double)rect.Width / (Math.Log10(value2) - Math.Log10(value)));
			return (float)Math.Abs(Math.Log10(result) * num2 - Math.Log10(area.XAxis.Info.StripLines[index].Value) * num2);
		}
		return area.AxisCore.GetDividerX() + result * (float)area.XAxis.Info.Dpi;
	}

	public void RenderYStrips(StiContext context, StiAxisAreaGeom geom, RectangleF rect)
	{
		if (Strips.Chart.Area is IStiAxisArea area)
		{
			float num = CalculateYValue(Strips.MinValue, area, rect);
			float num2 = CalculateYValue(Strips.MaxValue, area, rect);
			if (num > num2)
			{
				float num3 = num;
				num = num2;
				num2 = num3;
			}
			StiStripsYGeom item = new StiStripsYGeom(clientRectangle: new RectangleF(0f, num, rect.Width, num2 - num), strip: Strips);
			geom.CreateChildGeoms();
			geom.ChildGeoms.Add(item);
		}
	}

	public void Render(StiContext context, StiAxisAreaGeom geom, RectangleF rect)
	{
		if (Strips.Visible)
		{
			if (Strips.Orientation == StiStrips.StiOrientation.Vertical)
			{
				RenderXStrips(context, geom, rect);
			}
			if (Strips.Orientation == StiStrips.StiOrientation.Horizontal || Strips.Orientation == StiStrips.StiOrientation.HorizontalRight)
			{
				RenderYStrips(context, geom, rect);
			}
		}
	}

	private float CalculateYValue(string yValue, IStiAxisArea area, RectangleF rect)
	{
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
		yValue = StiReportParser.Parse(yValue, (IStiReportComponent)(object)(area.Chart as StiChart));
		float result = 0f;
		float.TryParse(yValue.Replace(",", "."), out result);
		Thread.CurrentThread.CurrentCulture = currentCulture;
		if (area.ReverseVert)
		{
			result = 0f - result;
		}
		if ((!area.YAxis.LogarithmicScale && Strips.Orientation == StiStrips.StiOrientation.Horizontal) || (!area.YRightAxis.LogarithmicScale && Strips.Orientation == StiStrips.StiOrientation.HorizontalRight))
		{
			if (Strips.Orientation == StiStrips.StiOrientation.Horizontal)
			{
				return area.AxisCore.GetDividerY() - result * (float)area.YAxis.Info.Dpi;
			}
			return area.AxisCore.GetDividerRightY() - result * (float)area.YRightAxis.Info.Dpi;
		}
		IStiYAxis stiYAxis = ((Strips.Orientation == StiStrips.StiOrientation.Horizontal) ? area.YAxis : area.YRightAxis);
		int count = stiYAxis.Info.StripLines.Count;
		int index = 0;
		int index2 = count - 1;
		double value = stiYAxis.Info.StripLines[index].Value;
		double value2 = stiYAxis.Info.StripLines[index2].Value;
		double num = Math.Abs((double)rect.Height / (Math.Log10(value2) - Math.Log10(value)));
		return (float)Math.Abs(Math.Log10(stiYAxis.Info.StripLines[index].Value) * num - Math.Log10(result) * num);
	}

	public StiStripsCoreXF(IStiStrips strips)
	{
		this.strips = strips;
	}
}
