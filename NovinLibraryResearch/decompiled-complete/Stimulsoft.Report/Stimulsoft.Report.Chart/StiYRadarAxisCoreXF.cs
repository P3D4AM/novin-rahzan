using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components.TextFormats;

namespace Stimulsoft.Report.Chart;

public class StiYRadarAxisCoreXF : StiRadarAxisCoreXF
{
	public IStiYRadarAxis YAxis => base.Axis as IStiYRadarAxis;

	public StiAxisInfoXF Info
	{
		get
		{
			return YAxis.Info;
		}
		set
		{
			YAxis.Info = value;
		}
	}

	internal float TicksMaxLength
	{
		get
		{
			if (base.Axis.Visible)
			{
				return Math.Max(YAxis.Ticks.Visible ? YAxis.Ticks.Length : 0f, YAxis.Ticks.MinorVisible ? YAxis.Ticks.MinorLength : 0f);
			}
			return 0f;
		}
	}

	public override void ApplyStyle(IStiChartStyle style)
	{
		if (base.Axis.AllowApplyStyle)
		{
			YAxis.LineColor = style.Core.AxisLineColor;
			YAxis.Labels.Core.ApplyStyle(style);
		}
	}

	public StiCellGeom Render(StiContext context, RectangleF rect)
	{
		if (!base.Axis.Visible)
		{
			return null;
		}
		RectangleF axisRect = GetAxisRect(context, rect, includeAxisArrow: false, includeLabelsHeight: false, isDrawing: true);
		StiRadarAxisGeom stiRadarAxisGeom = new StiRadarAxisGeom(base.Axis as IStiYRadarAxis, axisRect);
		RenderLabels(context, axisRect, stiRadarAxisGeom);
		return stiRadarAxisGeom;
	}

	private List<StiAxisLabelInfoXF> MeasureStripLines(StiContext context, RectangleF rect)
	{
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Invalid comparison between Unknown and I4
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		List<StiAxisLabelInfoXF> list = new List<StiAxisLabelInfoXF>();
		if (YAxis.Info.LabelsCollection != null && YAxis.Labels.Placement != 0)
		{
			int num = 0;
			foreach (StiStripPositionXF item in YAxis.Info.LabelsCollection)
			{
				string labelText = GetLabelText(item.StripLine, null);
				float position = item.Position;
				StiStringFormatGeom genericStringFormat = context.GetGenericStringFormat();
				StiFontGeom font = StiFontGeom.ChangeFontSize(YAxis.Labels.Font, YAxis.Labels.Font.Size * context.Options.Zoom);
				StiHorAlignment textAlignment = GetTextAlignment();
				float angle = 0f - YAxis.Labels.Angle;
				PointF empty = PointF.Empty;
				StiRotationMode val;
				if ((int)textAlignment == 1)
				{
					val = (StiRotationMode)1;
					empty = new PointF(0f, position);
					angle = 0f;
				}
				else
				{
					val = (StiRotationMode)7;
					empty = new PointF(rect.Width - GetTicksMaxLength(context) - GetLabelsSpaceAxis(context), position);
				}
				if (YAxis.Labels.Placement == StiLabelsPlacement.TwoLines && ((uint)num & (true ? 1u : 0u)) != 0)
				{
					empty.X -= GetLabelsTwoLinesDestination(context);
				}
				RectangleF clientRectangle = context.MeasureRotatedString(labelText, font, empty, genericStringFormat, val, angle, (int)(YAxis.Labels.Width * context.Options.Zoom));
				StiAxisLabelInfoXF stiAxisLabelInfoXF = new StiAxisLabelInfoXF();
				stiAxisLabelInfoXF.Angle = angle;
				stiAxisLabelInfoXF.ClientRectangle = clientRectangle;
				stiAxisLabelInfoXF.RotationMode = val;
				stiAxisLabelInfoXF.TextPoint = empty;
				stiAxisLabelInfoXF.Text = labelText;
				stiAxisLabelInfoXF.StripLine = item.StripLine;
				list.Add(stiAxisLabelInfoXF);
				num++;
			}
		}
		return list;
	}

	private void RenderLabels(StiContext context, RectangleF rect, StiRadarAxisGeom geom)
	{
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		if (YAxis.Labels.Placement == StiLabelsPlacement.None)
		{
			return;
		}
		List<StiAxisLabelGeom> list = new List<StiAxisLabelGeom>();
		List<StiAxisLabelInfoXF> list2 = MeasureStripLines(context, rect);
		geom.CreateChildGeoms();
		foreach (StiAxisLabelInfoXF item2 in list2)
		{
			if (!item2.ClientRectangle.IsEmpty)
			{
				StiYRadarAxisLabelGeom item = new StiYRadarAxisLabelGeom(YAxis, item2.ClientRectangle, item2.TextPoint, item2.Text, item2.StripLine, item2.Angle, item2.RotationMode);
				geom.ChildGeoms.Add(item);
			}
		}
	}

	protected internal void CalculateStripPositions(float topPosition, float bottomPosition)
	{
		bottomPosition -= topPosition;
		topPosition = 0f;
		if (YAxis.Info.StripLines == null || YAxis.Info.StripLines.Count < 2)
		{
			YAxis.Info.StripPositions = new float[0];
			return;
		}
		YAxis.Info.StripPositions = new float[YAxis.Info.StripLines.Count];
		YAxis.Info.StripPositions[0] = topPosition;
		YAxis.Info.StripPositions[YAxis.Info.StripPositions.Length - 1] = bottomPosition;
		for (int i = 1; i < YAxis.Info.StripPositions.Length - 1; i++)
		{
			YAxis.Info.StripPositions[i] = topPosition + (float)i * YAxis.Info.Step;
		}
	}

	public RectangleF GetAxisRect(StiContext context, RectangleF rect, bool includeAxisArrow, bool includeLabelsHeight, bool isDrawing)
	{
		return new RectangleF(rect.Width / 2f - GetTicksMaxLength(context), 0f, GetTicksMaxLength(context), rect.Height / 2f);
	}

	protected internal float GetTicksMaxLength(StiContext context)
	{
		return YAxis.YCore.TicksMaxLength * context.Options.Zoom;
	}

	protected internal float GetLabelsSpaceAxis(StiContext context)
	{
		return 5f * context.Options.Zoom;
	}

	protected internal float GetLabelsTwoLinesDestination(StiContext context)
	{
		return YAxis.Labels.Font.SizeInPoints * 2f * context.Options.Zoom;
	}

	protected internal StiHorAlignment GetTextAlignment()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		if (YAxis.Labels.Placement == StiLabelsPlacement.TwoLines)
		{
			return (StiHorAlignment)3;
		}
		return YAxis.Labels.TextAlignment;
	}

	private void SetTotalNumberCapacity()
	{
		if (YAxis.Labels.FormatService is StiNumberFormatService stiNumberFormatService)
		{
			float calculatedStep = YAxis.Labels.CalculatedStep;
			int num = ((!(calculatedStep < 1f)) ? 1 : (-1));
			stiNumberFormatService.TotalNumberCapacity = (int)Math.Floor((double)num * Math.Log10(calculatedStep) + 1.0);
		}
	}

	internal string GetLabelText(StiStripLineXF line, IStiSeries series)
	{
		object obj = line?.ValueObject ?? "";
		try
		{
			if (YAxis.Labels.FormatService != null && !(YAxis.Labels.FormatService is StiGeneralFormatService))
			{
				SetTotalNumberCapacity();
				return YAxis.Labels.TextBefore + YAxis.Labels.FormatService.Format(line.Value) + YAxis.Labels.TextAfter;
			}
			if (YAxis.Labels.Format != null && YAxis.Labels.Format.Trim().Length != 0)
			{
				try
				{
					if (obj is string && decimal.TryParse(obj.ToString().Replace(".", ",").Replace(",", Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator), out var result))
					{
						obj = result;
					}
					if (!StiChartOptions.OldChartPercentMode && YAxis.Labels.Format.StartsWith("P", StringComparison.InvariantCulture))
					{
						int result2 = 0;
						if (YAxis.Labels.Format.Length > 1)
						{
							int.TryParse(YAxis.Labels.Format.Remove(0, 1), out result2);
						}
						return string.Format("{0}{1:N" + result2 + "}{2}{3}", YAxis.Labels.TextBefore, obj, "%", YAxis.Labels.TextAfter);
					}
					return string.Format("{0}{1:" + YAxis.Labels.Format + "}{2}", YAxis.Labels.TextBefore, obj, YAxis.Labels.TextAfter);
				}
				catch
				{
				}
			}
			return $"{YAxis.Labels.TextBefore}{obj}{YAxis.Labels.TextAfter}";
		}
		catch
		{
		}
		return obj.ToString();
	}

	protected internal StiStringFormatGeom GetStringFormatGeom(StiContext context)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Invalid comparison between Unknown and I4
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Invalid comparison between Unknown and I4
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Invalid comparison between Unknown and I4
		StiStringFormatGeom genericStringFormat = context.GetGenericStringFormat();
		genericStringFormat.Trimming = (StringTrimming)0;
		genericStringFormat.FormatFlags = (StringFormatFlags)6144;
		if (YAxis.Labels.Width > 0f)
		{
			StiHorAlignment textAlignment = GetTextAlignment();
			if ((int)textAlignment == 1)
			{
				genericStringFormat.Alignment = (StringAlignment)0;
			}
			else if ((int)textAlignment == 3)
			{
				genericStringFormat.Alignment = (StringAlignment)2;
			}
			else if ((int)textAlignment == 2)
			{
				genericStringFormat.Alignment = (StringAlignment)1;
			}
		}
		return genericStringFormat;
	}

	protected internal StiFontGeom GetFontGeom(StiContext context)
	{
		return StiFontGeom.ChangeFontSize(YAxis.Labels.Font, YAxis.Labels.Font.Size * context.Options.Zoom);
	}

	public StiYRadarAxisCoreXF(IStiRadarAxis axis)
		: base(axis)
	{
	}
}
