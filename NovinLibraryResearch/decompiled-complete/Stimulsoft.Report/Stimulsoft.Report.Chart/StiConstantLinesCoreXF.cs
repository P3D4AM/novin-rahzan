using System;
using System.Drawing;
using System.Globalization;
using System.Threading;
using Stimulsoft.Base;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Chart;

public class StiConstantLinesCoreXF : IStiApplyStyle, ICloneable
{
	public IStiConstantLines ConstantLines { get; set; }

	public object Clone()
	{
		return MemberwiseClone();
	}

	public void ApplyStyle(IStiChartStyle style)
	{
		if (ConstantLines.AllowApplyStyle)
		{
			ConstantLines.LineColor = style.Core.SeriesLabelsColor;
		}
	}

	public void RenderXConstantLines(StiContext context, StiAxisAreaGeom geom, RectangleF rect)
	{
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0323: Unknown result type (might be due to invalid IL or missing references)
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		//IL_0377: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
		if (!(geom.Area is IStiAxisArea stiAxisArea))
		{
			return;
		}
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
		string text = StiReportParser.Parse(ConstantLines.AxisValue, (IStiReportComponent)(object)(stiAxisArea.Chart as StiChart));
		float result = 0f;
		if (DateTime.TryParse(text, out var result2))
		{
			result = (float)result2.ToOADate();
		}
		else
		{
			float.TryParse(text.Replace(",", "."), out result);
		}
		Thread.CurrentThread.CurrentCulture = currentCulture;
		if (stiAxisArea.ReverseVert)
		{
			result = 0f - result;
		}
		float num = 0f;
		if (stiAxisArea.XAxis.LogarithmicScale && stiAxisArea.XAxis.Info.StripLines.Count > 0)
		{
			int count = stiAxisArea.XAxis.Info.StripLines.Count;
			int index = (stiAxisArea.ReverseHor ? (count - 1) : 0);
			int index2 = ((!stiAxisArea.ReverseHor) ? (count - 1) : 0);
			double value = stiAxisArea.XAxis.Info.StripLines[index].Value;
			double value2 = stiAxisArea.XAxis.Info.StripLines[index2].Value;
			double num2 = Math.Abs((double)rect.Width / (Math.Log10(value2) - Math.Log10(value)));
			num = (float)Math.Abs(Math.Log10(result) * num2 - Math.Log10(stiAxisArea.XAxis.Info.StripLines[index].Value) * num2);
		}
		else
		{
			num = stiAxisArea.AxisCore.GetDividerX() + result * (float)stiAxisArea.XAxis.Info.Dpi;
			foreach (StiStripLineXF stripLine in stiAxisArea.XAxis.Info.StripLines)
			{
				if (stripLine.ValueObject != null && stripLine.ValueObject.ToString() == ConstantLines.AxisValue)
				{
					result = (float)stripLine.Value;
					if (stiAxisArea.ReverseHor)
					{
						result = 0f - result;
					}
					num = (float)((double)result * stiAxisArea.XAxis.Info.Dpi) + stiAxisArea.AxisCore.GetDividerX();
				}
			}
		}
		PointF point = new PointF(0f, 0f);
		StiRotationMode mode = (StiRotationMode)0;
		switch (ConstantLines.Position)
		{
		case StiConstantLines.StiTextPosition.LeftTop:
			mode = (StiRotationMode)2;
			point = new PointF(num + ConstantLines.LineWidth / 2f, 0f);
			break;
		case StiConstantLines.StiTextPosition.LeftBottom:
			mode = (StiRotationMode)0;
			point = new PointF(num - ConstantLines.LineWidth / 2f, 0f);
			break;
		case StiConstantLines.StiTextPosition.CenterTop:
			mode = (StiRotationMode)5;
			point = new PointF(num + ConstantLines.LineWidth / 2f, rect.Height / 2f);
			break;
		case StiConstantLines.StiTextPosition.CenterBottom:
			mode = (StiRotationMode)3;
			point = new PointF(num - ConstantLines.LineWidth / 2f, rect.Height / 2f);
			break;
		case StiConstantLines.StiTextPosition.RightTop:
			mode = (StiRotationMode)8;
			point = new PointF(num + ConstantLines.LineWidth / 2f, rect.Height);
			break;
		case StiConstantLines.StiTextPosition.RightBottom:
			mode = (StiRotationMode)6;
			point = new PointF(num - ConstantLines.LineWidth / 2f, rect.Height);
			break;
		}
		StiConstantLinesVerticalGeom item = new StiConstantLinesVerticalGeom(ConstantLines, new RectangleF(num, 0f, num, rect.Height), point, mode);
		geom.CreateChildGeoms();
		geom.ChildGeoms.Add(item);
	}

	public void RenderYConstantLines(StiContext context, StiAxisAreaGeom geom, RectangleF rect)
	{
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		if (geom.Area is IStiAxisArea stiAxisArea)
		{
			CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
			string text = StiReportParser.Parse(ConstantLines.AxisValue, (IStiReportComponent)(object)(stiAxisArea.Chart as StiChart));
			float result = 0f;
			float.TryParse(text.Replace(",", "."), out result);
			Thread.CurrentThread.CurrentCulture = currentCulture;
			if (stiAxisArea.ReverseVert)
			{
				result = 0f - result;
			}
			float num = 0f;
			if ((!stiAxisArea.YAxis.LogarithmicScale && ConstantLines.Orientation == StiConstantLines.StiOrientation.Horizontal) || (!stiAxisArea.YRightAxis.LogarithmicScale && ConstantLines.Orientation == StiConstantLines.StiOrientation.HorizontalRight))
			{
				num = ((ConstantLines.Orientation != 0) ? (stiAxisArea.AxisCore.GetDividerRightY() - result * (float)stiAxisArea.YRightAxis.Info.Dpi) : (stiAxisArea.AxisCore.GetDividerY() - result * (float)stiAxisArea.YAxis.Info.Dpi));
			}
			else
			{
				IStiYAxis stiYAxis = ((ConstantLines.Orientation == StiConstantLines.StiOrientation.Horizontal) ? stiAxisArea.YAxis : stiAxisArea.YRightAxis);
				int count = stiYAxis.Info.StripLines.Count;
				int index = 0;
				int index2 = count - 1;
				double value = stiYAxis.Info.StripLines[index].Value;
				double value2 = stiYAxis.Info.StripLines[index2].Value;
				double num2 = Math.Abs((double)rect.Height / (Math.Log10(value2) - Math.Log10(value)));
				num = (float)Math.Abs(Math.Log10(stiYAxis.Info.StripLines[index].Value) * num2 - Math.Log10(result) * num2);
			}
			PointF point = new PointF(0f, 0f);
			StiRotationMode mode = (StiRotationMode)0;
			switch (ConstantLines.Position)
			{
			case StiConstantLines.StiTextPosition.LeftTop:
				mode = (StiRotationMode)2;
				point = new PointF(0f - rect.X, num - ConstantLines.LineWidth / 2f);
				break;
			case StiConstantLines.StiTextPosition.LeftBottom:
				mode = (StiRotationMode)0;
				point = new PointF(0f - rect.X, num + ConstantLines.LineWidth / 2f);
				break;
			case StiConstantLines.StiTextPosition.CenterTop:
				mode = (StiRotationMode)5;
				point = new PointF(rect.Width / 2f, num - ConstantLines.LineWidth / 2f);
				break;
			case StiConstantLines.StiTextPosition.CenterBottom:
				mode = (StiRotationMode)3;
				point = new PointF(rect.Width / 2f, num + ConstantLines.LineWidth / 2f);
				break;
			case StiConstantLines.StiTextPosition.RightTop:
				mode = (StiRotationMode)8;
				point = new PointF(rect.Width, num - ConstantLines.LineWidth / 2f);
				break;
			case StiConstantLines.StiTextPosition.RightBottom:
				mode = (StiRotationMode)6;
				point = new PointF(rect.Width, num + ConstantLines.LineWidth / 2f);
				break;
			}
			StiConstantLinesYGeom item = new StiConstantLinesYGeom(ConstantLines, new RectangleF(0f, num, rect.Width, num), point, mode);
			geom.CreateChildGeoms();
			geom.ChildGeoms.Add(item);
		}
	}

	public void Render(StiContext context, StiAxisAreaGeom geom, RectangleF rect)
	{
		if (ConstantLines.Visible)
		{
			if (ConstantLines.Orientation == StiConstantLines.StiOrientation.Vertical)
			{
				RenderXConstantLines(context, geom, rect);
			}
			if (ConstantLines.Orientation == StiConstantLines.StiOrientation.Horizontal || ConstantLines.Orientation == StiConstantLines.StiOrientation.HorizontalRight)
			{
				RenderYConstantLines(context, geom, rect);
			}
		}
	}

	public StiConstantLinesCoreXF(IStiConstantLines constantLines)
	{
		ConstantLines = constantLines;
	}
}
