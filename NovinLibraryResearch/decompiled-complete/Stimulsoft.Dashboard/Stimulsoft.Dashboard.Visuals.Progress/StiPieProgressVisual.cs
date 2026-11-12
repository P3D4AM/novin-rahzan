using System;
using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.Progress;
using Stimulsoft.Dashboard.Helpers;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Helpers;

namespace Stimulsoft.Dashboard.Visuals.Progress;

public class StiPieProgressVisual : StiProgressVisual
{
	public override void Draw(StiContext context, RectangleF rectMain)
	{
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Expected O, but got Unknown
		if (base.Iterations.Count == 0)
		{
			return;
		}
		List<RectangleD> locationRectangles = StiElementLocationHelper.GetLocationRectangles(RectangleD.CreateFromRectangle(rectMain), base.Iterations.Count, base.Element);
		if (locationRectangles.Count < 1)
		{
			return;
		}
		RectangleF rectangleF = locationRectangles[0].ToRectangleF();
		float num = ((rectangleF.Width < rectangleF.Height) ? rectangleF.Width : rectangleF.Height);
		Font font = base.Element.Font;
		float num2 = GetTitleHeight(num, font.Size);
		float width = rectangleF.Width;
		float? num3 = null;
		Font val = new Font(font.FontFamily.Name, 1f, font.Style, font.Unit);
		try
		{
			foreach (StiProgressIteration iteration2 in base.Iterations)
			{
				if (!string.IsNullOrEmpty(iteration2.Series))
				{
					float fontSize = StiStringFormatHelper.GetFontSize(context, new RectangleF(0f, 0f, width, num2), iteration2.Series, val);
					if (!num3.HasValue)
					{
						num3 = fontSize;
					}
					else if (num3 > fontSize)
					{
						num3 = fontSize;
					}
				}
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		if (num3.HasValue && num3.GetValueOrDefault() < font.Size)
		{
			num2 = GetTitleHeight(num, num3.GetValueOrDefault());
		}
		float num4 = (num - num2) / 2f * 0.8f;
		if (num3.GetValueOrDefault() < (float)(int)base.MinFontSize)
		{
			num3 = 0f;
			num2 = 0f;
			num4 = num / 2f * 0.8f;
		}
		float num5 = 2f * num4;
		List<decimal> targetValues = GetTargetValues();
		bool flag = false;
		context.PushClip(rectMain);
		for (int i = 0; i < base.Iterations.Count; i++)
		{
			decimal taget = targetValues[i];
			StiProgressIteration iteration = base.Iterations[i];
			RectangleF rectangleF2 = locationRectangles[i].ToRectangleF();
			if (!flag && (rectangleF2.Right - rectMain.Right > 1f || rectangleF2.Bottom - rectMain.Bottom > 1f))
			{
				flag = true;
			}
			float x = rectangleF2.X + rectangleF2.Width / 2f;
			float y = rectangleF2.Y + num2 + (rectangleF.Height - num2) / 2f;
			PointF pointF = new PointF(x, y);
			RectangleF pieRect = new RectangleF(pointF.X - num4, pointF.Y - num4, num5, num5);
			RectangleF elementRectText = new RectangleF(pieRect.X + pieRect.Width / 2f - width / 2f, pieRect.Top - num2, width, num2);
			Color color = ProcessColor(GetColor(i), base.Iterations[i]);
			DrawPieProgress(context, pieRect, elementRectText, taget, iteration, num3, color);
		}
		if (flag)
		{
			float factor = (float)(StiElementScale.Factor((IStiElement)base.Element) * StiScale.Factor);
			DrawArrowMore(context, base.Element, rectMain, factor);
		}
		context.PopClip();
	}

	private void DrawPieProgress(StiContext context, RectangleF pieRect, RectangleF elementRectText, decimal taget, StiProgressIteration iteration, float? fontSize, Color color)
	{
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Expected O, but got Unknown
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Expected O, but got Unknown
		Color color2 = ProcessTrackColor(base.Style.TrackColor, iteration);
		context.FillEllipse(color2, pieRect);
		decimal num = ((taget != 0m) ? (360m / taget * Math.Abs(iteration.Value)) : taget);
		if (num > 360m)
		{
			num = 360m;
		}
		List<StiSegmentGeom> path = new List<StiSegmentGeom>
		{
			new StiPieSegmentGeom(pieRect, -90f, (float)num, null)
		};
		context.FillPath(color, path, pieRect);
		if (fontSize.GetValueOrDefault() >= (float)(int)base.MinFontSize)
		{
			string text = ProcessFontName(base.Element.Font.FontFamily.Name, iteration);
			FontStyle val = ProcessFontStyle(base.Element.Font, iteration);
			Font val2 = new Font(text, fontSize.GetValueOrDefault(), val);
			StiFontGeom font = new StiFontGeom(new Font(val2.FontFamily.Name, fontSize.GetValueOrDefault(), val2.Style, val2.Unit));
			StiRotationMode rotationMode = StiRotationMode.CenterCenter;
			PointF point = default(PointF);
			StiStringFormatHelper.MeasureAlignmentParameters(StiHorAlignment.Center, elementRectText, ref rotationMode, ref point);
			Color color3 = ProcessForeColor(iteration);
			context.DrawRotatedString(iteration.Series, font, color3, point, StiStringFormatHelper.GetStringFormatGeom(context), rotationMode, 0f, antialiasing: true);
		}
	}

	private float GetTitleHeight(float elementSide, float size)
	{
		float num = size / 13f;
		if (num > 4f)
		{
			num = 4f;
		}
		return elementSide / 4f * num;
	}

	public StiPieProgressVisual(StiProgressElement element, List<StiProgressIteration> iterations)
		: base(element, iterations)
	{
	}
}
