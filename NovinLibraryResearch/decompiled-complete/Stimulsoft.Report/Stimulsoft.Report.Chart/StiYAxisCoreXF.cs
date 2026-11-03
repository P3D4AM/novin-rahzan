using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Threading;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components.TextFormats;

namespace Stimulsoft.Report.Chart;

public abstract class StiYAxisCoreXF : StiAxisCoreXF
{
	[Browsable(false)]
	public abstract StiYAxisDock Dock { get; }

	[Browsable(false)]
	public bool IsLeftSide
	{
		get
		{
			if (Dock != 0 || base.Axis.Area.ReverseHor)
			{
				if (Dock == StiYAxisDock.Right)
				{
					return base.Axis.Area.ReverseHor;
				}
				return false;
			}
			return true;
		}
	}

	[Browsable(false)]
	public bool IsRightSide
	{
		get
		{
			if (Dock != StiYAxisDock.Right || base.Axis.Area.ReverseHor)
			{
				if (Dock == StiYAxisDock.Left)
				{
					return base.Axis.Area.ReverseHor;
				}
				return false;
			}
			return true;
		}
	}

	public override StiCellGeom Render(StiContext context, RectangleF rect)
	{
		if (!base.Axis.Visible)
		{
			return null;
		}
		RectangleF axisRect = GetAxisRect(context, rect, includeAxisArrow: false, includeLabelsHeight: false, isDrawing: true, includeScrollBar: false);
		axisRect.X = 0f;
		axisRect.Y = 0f;
		if (base.Axis.Interaction.ShowScrollBar && ((base.Axis is IStiYRightAxis && !base.Axis.Area.ReverseHor) || (base.Axis is IStiYLeftAxis && base.Axis.Area.ReverseHor)))
		{
			axisRect.X += StiAxisCoreXF.DefaultScrollBarSize * context.Options.Zoom;
		}
		StiYAxisGeom stiYAxisGeom = new StiYAxisGeom(base.Axis as IStiYAxis, axisRect, isCenterAxis: false);
		RenderLabels(context, axisRect, stiYAxisGeom);
		if (!base.Axis.Range.Auto)
		{
			if (base.Axis.Area.ReverseVert)
			{
				axisRect.Y = (float)((base.Axis.Info.Minimum - base.Axis.Range.Minimum) * base.Axis.Info.Dpi);
			}
			else
			{
				axisRect.Y = (float)((base.Axis.Info.Maximum - base.Axis.Range.Maximum) * base.Axis.Info.Dpi);
			}
			axisRect.Height = (float)((base.Axis.Range.Maximum - base.Axis.Range.Minimum) * base.Axis.Info.Dpi);
		}
		RenderTitle(context, axisRect, stiYAxisGeom);
		return stiYAxisGeom;
	}

	public override StiCellGeom RenderView(StiContext context, RectangleF rect)
	{
		if (!base.Axis.Visible)
		{
			return null;
		}
		RectangleF axisRect = GetAxisRect(context, rect, includeAxisArrow: false, includeLabelsHeight: false, isDrawing: true, includeScrollBar: true);
		return new StiYAxisViewGeom(base.Axis as IStiYAxis, axisRect, isCenterAxis: false);
	}

	public void RenderScrollBar(StiContext context, RectangleF axisRect, StiYAxisViewGeom axisGeom)
	{
		if (base.Axis.Interaction.ShowScrollBar)
		{
			RectangleF rectangleF = axisRect;
			rectangleF.Y = 0f;
			rectangleF.Width = StiAxisCoreXF.DefaultScrollBarSize * context.Options.Zoom;
			if ((base.Axis is IStiYLeftAxis && !base.Axis.Area.ReverseHor) || (base.Axis is IStiYRightAxis && base.Axis.Area.ReverseHor))
			{
				rectangleF.X = axisRect.Width - rectangleF.Width;
			}
			else
			{
				rectangleF.X = 0f;
			}
			StiVertScrollBarGeom stiVertScrollBarGeom = new StiVertScrollBarGeom(base.Axis as IStiYAxis, rectangleF);
			axisGeom.CreateChildGeoms();
			axisGeom.ChildGeoms.Add(stiVertScrollBarGeom);
			rectangleF.X = 0f;
			rectangleF.Y = 0f;
			rectangleF.Inflate(-2f, -2f);
			if (base.Axis.Interaction.RangeScrollEnabled)
			{
				RectangleF clientRectangle = rectangleF;
				clientRectangle.Height = rectangleF.Width;
				StiUpButtonGeom item = new StiUpButtonGeom(base.Axis as IStiYAxis, clientRectangle);
				stiVertScrollBarGeom.CreateChildGeoms();
				stiVertScrollBarGeom.ChildGeoms.Add(item);
				RectangleF clientRectangle2 = rectangleF;
				clientRectangle2.Height = rectangleF.Width;
				clientRectangle2.Y = rectangleF.Bottom - clientRectangle2.Height;
				StiDownButtonGeom item2 = new StiDownButtonGeom(base.Axis as IStiYAxis, clientRectangle2);
				stiVertScrollBarGeom.CreateChildGeoms();
				stiVertScrollBarGeom.ChildGeoms.Add(item2);
				rectangleF.Inflate(0f, 0f - rectangleF.Width - 2f);
			}
			RectangleF clientRectangle3 = rectangleF;
			IStiYAxis yAxis = base.Axis.Area.YAxis;
			StiAxisAreaCoreXF stiAxisAreaCoreXF = base.Axis.Area.Core as StiAxisAreaCoreXF;
			if (!yAxis.Range.Auto)
			{
				float num = (float)((double)rectangleF.Height / stiAxisAreaCoreXF.ScrollRangeY);
				float num2 = (float)stiAxisAreaCoreXF.ScrollValueY * num;
				float height = (float)stiAxisAreaCoreXF.ScrollViewY * num;
				clientRectangle3 = new RectangleF(rectangleF.X, num2 + rectangleF.Y, rectangleF.Width, height);
			}
			StiVertTrackBarGeom item3 = new StiVertTrackBarGeom(base.Axis as IStiYAxis, clientRectangle3, stiVertScrollBarGeom);
			stiVertScrollBarGeom.CreateChildGeoms();
			stiVertScrollBarGeom.ChildGeoms.Add(item3);
		}
	}

	public StiCellGeom RenderCenter(StiContext context, RectangleF rect)
	{
		if (!base.Axis.Visible)
		{
			return null;
		}
		if (base.Axis.Area.XAxis.Info.Minimum >= 0.0)
		{
			return null;
		}
		RectangleF centerAxisRect = GetCenterAxisRect(context, rect, includeAxisArrow: false, includeLabelsHeight: false, isDrawing: true);
		centerAxisRect.X = 0f;
		centerAxisRect.Y = 0f;
		return new StiYAxisGeom(base.Axis as IStiYAxis, centerAxisRect, isCenterAxis: true);
	}

	public StiCellGeom RenderCenterView(StiContext context, RectangleF rect)
	{
		if (!base.Axis.Visible)
		{
			return null;
		}
		if (base.Axis.Area.XAxis.Info.Minimum >= 0.0)
		{
			return null;
		}
		RectangleF centerAxisRect = GetCenterAxisRect(context, rect, includeAxisArrow: false, includeLabelsHeight: false, isDrawing: true);
		return new StiYAxisViewGeom(base.Axis as IStiYAxis, centerAxisRect, isCenterAxis: true);
	}

	private void SetTotalNumberCapacity()
	{
		if (base.Axis.Labels.FormatService is StiNumberFormatService stiNumberFormatService)
		{
			float calculatedStep = base.Axis.Labels.CalculatedStep;
			int num = ((!(calculatedStep < 1f)) ? 1 : (-1));
			stiNumberFormatService.TotalNumberCapacity = (int)Math.Floor((double)num * Math.Log10(calculatedStep) + 1.0);
		}
	}

	internal string GetLabelText(StiStripLineXF line)
	{
		object obj = line.ValueObject;
		CultureInfo cultureInfo = null;
		try
		{
			if (base.Axis.Labels.FormatService != null && !(base.Axis.Labels.FormatService is StiGeneralFormatService))
			{
				SetTotalNumberCapacity();
				if (obj is DateTime)
				{
					if (base.Axis.Labels.FormatService is StiDateFormatService || base.Axis.Labels.FormatService is StiTimeFormatService)
					{
						return base.Axis.Labels.TextBefore + base.Axis.Labels.FormatService.Format(obj) + base.Axis.Labels.TextAfter;
					}
					return $"{base.Axis.Labels.TextBefore}{obj}{base.Axis.Labels.TextAfter}";
				}
				return base.Axis.Labels.TextBefore + base.Axis.Labels.FormatService.Format(line.Value) + base.Axis.Labels.TextAfter;
			}
			if (base.Axis.Labels.Format != null && base.Axis.Labels.Format.Trim().Length != 0)
			{
				string name = ((StiChart)base.Axis.Area.Chart)?.Report?.Culture;
				if (!string.IsNullOrEmpty(((StiChart)base.Axis.Area.Chart)?.Report?.Culture))
				{
					cultureInfo = Thread.CurrentThread.CurrentCulture;
					Thread.CurrentThread.CurrentCulture = new CultureInfo(name);
				}
				try
				{
					if (obj is string && decimal.TryParse(obj.ToString().Replace(".", ",").Replace(",", Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator), out var result))
					{
						obj = result;
					}
					if (!StiChartOptions.OldChartPercentMode && base.Axis.Labels.Format.StartsWith("P", StringComparison.InvariantCulture))
					{
						int result2 = 0;
						if (base.Axis.Labels.Format.Length > 1)
						{
							int.TryParse(base.Axis.Labels.Format.Remove(0, 1), out result2);
						}
						return string.Format("{0}{1:N" + result2 + "}{2}{3}", base.Axis.Labels.TextBefore, obj, "%", base.Axis.Labels.TextAfter);
					}
					return string.Format("{0}{1:" + base.Axis.Labels.Format + "}{2}", base.Axis.Labels.TextBefore, obj, base.Axis.Labels.TextAfter);
				}
				catch
				{
				}
			}
			return $"{base.Axis.Labels.TextBefore}{obj}{base.Axis.Labels.TextAfter}";
		}
		catch
		{
		}
		finally
		{
			if (cultureInfo != null)
			{
				Thread.CurrentThread.CurrentCulture = cultureInfo;
			}
		}
		return obj.ToString();
	}

	private List<StiAxisLabelInfoXF> MeasureStripLines(StiContext context, RectangleF rect)
	{
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Invalid comparison between Unknown and I4
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Invalid comparison between Unknown and I4
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Invalid comparison between Unknown and I4
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Invalid comparison between Unknown and I4
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_037b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0311: Unknown result type (might be due to invalid IL or missing references)
		//IL_0314: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Invalid comparison between Unknown and I4
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_031b: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Invalid comparison between Unknown and I4
		List<StiAxisLabelInfoXF> list = new List<StiAxisLabelInfoXF>();
		if (base.Axis.Info.LabelsCollection != null && base.Axis.Labels.Placement != 0)
		{
			SizeF axisTitleSize = GetAxisTitleSize(context);
			int num = 0;
			foreach (StiStripPositionXF item2 in base.Axis.Info.LabelsCollection)
			{
				bool flag = (base.Axis.Area.ReverseHor ? IsLeftSide : IsRightSide);
				if (flag && base.Axis.Area.XTopAxis.ArrowStyle != 0 && num == 0)
				{
					list.Add(new StiAxisLabelInfoXF());
					num++;
					continue;
				}
				if (flag && base.Axis.Area.XAxis.ArrowStyle != 0 && num == base.Axis.Info.StripLines.Count - 1)
				{
					list.Add(new StiAxisLabelInfoXF());
					num++;
					continue;
				}
				string labelText = GetLabelText(item2.StripLine);
				float position = item2.Position;
				StiStringFormatGeom genericStringFormat = context.GetGenericStringFormat();
				StiFontGeom font = StiFontGeom.ChangeFontSize(base.Axis.Labels.Font, base.Axis.Labels.Font.Size * context.Options.Zoom);
				StiHorAlignment textAlignment = GetTextAlignment();
				float angle = 0f - base.Axis.Labels.Angle;
				PointF empty = PointF.Empty;
				StiRotationMode val;
				if (IsLeftSide)
				{
					if ((int)textAlignment == 1)
					{
						val = (StiRotationMode)1;
						empty = new PointF(axisTitleSize.Width, position);
						angle = 0f;
					}
					else if ((int)textAlignment == 2)
					{
						val = (StiRotationMode)4;
						empty = new PointF((rect.Width - axisTitleSize.Width) / 2f, position);
						angle = 0f;
					}
					else
					{
						val = (StiRotationMode)7;
						empty = new PointF(rect.Width - GetTicksMaxLength(context) - GetLabelsSpaceAxis(context), position);
					}
				}
				else if ((int)textAlignment == 1)
				{
					val = (StiRotationMode)1;
					empty = new PointF(GetTicksMaxLength(context) + GetLabelsSpaceAxis(context), position);
				}
				else if ((int)textAlignment == 2)
				{
					val = (StiRotationMode)4;
					empty = new PointF((rect.Width - axisTitleSize.Width) / 2f, position);
					angle = 0f;
				}
				else
				{
					val = (StiRotationMode)7;
					empty = new PointF(rect.Width - axisTitleSize.Width, position);
					angle = 0f;
				}
				if (base.Axis.Labels.Placement == StiLabelsPlacement.TwoLines && ((uint)num & (true ? 1u : 0u)) != 0)
				{
					if (IsLeftSide)
					{
						empty.X -= GetLabelsTwoLinesDestination(context);
					}
					if (IsRightSide)
					{
						empty.X += GetLabelsTwoLinesDestination(context);
					}
				}
				RectangleF clientRectangle = context.MeasureRotatedString(labelText, font, empty, genericStringFormat, val, angle, (int)(base.Axis.Labels.Width * context.Options.Zoom));
				int num2 = (int)(base.Axis.Labels.Width * context.Options.Zoom);
				if (num2 != 0)
				{
					if (val - 3 > 2)
					{
						if (val - 6 <= 2)
						{
							clientRectangle.X -= (float)num2 - clientRectangle.Width;
						}
					}
					else
					{
						clientRectangle.X -= ((float)num2 - clientRectangle.Width) / 2f;
					}
					clientRectangle.Width = num2;
				}
				StiAxisLabelInfoXF item = new StiAxisLabelInfoXF
				{
					Angle = angle,
					ClientRectangle = clientRectangle,
					RotationMode = val,
					TextPoint = empty,
					Text = labelText,
					StripLine = item2.StripLine
				};
				list.Add(item);
				num++;
			}
		}
		if (base.Axis.Range.Auto)
		{
			float num3 = (float)base.Axis.Labels.Font.Height * context.Options.Zoom;
			float num4 = rect.Height / (float)base.Axis.Info.StripLines.Count * 1.5f;
			double num5 = Math.Ceiling(num3 / num4);
			int num6 = 0;
			List<StiAxisLabelInfoXF> list2 = new List<StiAxisLabelInfoXF>();
			list.Reverse();
			{
				foreach (StiAxisLabelInfoXF item3 in list)
				{
					if (num6 == 0)
					{
						list2.Add(item3);
					}
					num6++;
					if ((double)num6 == num5)
					{
						num6 = 0;
					}
				}
				return list2;
			}
		}
		return list;
	}

	public RectangleF GetCenterAxisRect(StiContext context, RectangleF rect, bool includeAxisArrow, bool includeLabelsHeight, bool isDrawing)
	{
		if (base.Axis.Area.XAxis.Info.Minimum >= 0.0)
		{
			return RectangleF.Empty;
		}
		float x = 0f - GetTicksMaxLength(context);
		return new RectangleF(x, 0f, GetTicksMaxLength(context), rect.Height);
	}

	public virtual RectangleF GetAxisRect(StiContext context, RectangleF rect, bool includeAxisArrow, bool includeLabelsHeight, bool isDrawing, bool includeScrollBar)
	{
		RectangleF rectangleF = RectangleF.Empty;
		if (!base.Axis.Visible)
		{
			return rectangleF;
		}
		List<StiAxisLabelInfoXF> list = MeasureStripLines(context, rect);
		if (list.Count == 0)
		{
			rectangleF = ((!IsLeftSide) ? new RectangleF(rect.Width, 0f, GetTicksMaxLength(context), rect.Height) : new RectangleF(0f - GetTicksMaxLength(context), 0f, GetTicksMaxLength(context), rect.Height));
		}
		else
		{
			foreach (StiAxisLabelInfoXF item in list)
			{
				if (!item.ClientRectangle.IsEmpty)
				{
					rectangleF = ((!rectangleF.IsEmpty) ? RectangleF.Union(rectangleF, item.ClientRectangle) : item.ClientRectangle);
				}
			}
			if (base.Axis.Interaction.ShowScrollBar && includeScrollBar)
			{
				rectangleF.Width += StiAxisCoreXF.DefaultScrollBarSize * context.Options.Zoom;
			}
			rectangleF.Width += GetTicksMaxLength(context) + GetLabelsSpaceAxis(context);
			RectangleF rectangleF2 = rectangleF;
			if (IsLeftSide)
			{
				rectangleF = new RectangleF(0f - rectangleF.Width, 0f, rectangleF.Width, rect.Height);
			}
			if (IsRightSide)
			{
				rectangleF = new RectangleF(rect.Width, 0f, rectangleF.Width, rect.Height);
			}
			if (includeLabelsHeight)
			{
				rectangleF.Y = rectangleF2.Y;
				rectangleF.Height = rectangleF2.Height;
			}
		}
		if (base.Axis.ArrowStyle != StiArrowStyle.None && includeAxisArrow)
		{
			float num = GetArrowHeight(context);
			if (base.Axis.Area.ReverseVert)
			{
				num = 0f - num;
			}
			rectangleF = RectangleF.Union(rectangleF, new RectangleF(rectangleF.X, 0f - num, 1f, num));
		}
		SizeF axisTitleSize = GetAxisTitleSize(context);
		if (!axisTitleSize.IsEmpty && base.Axis.Title.Position == StiTitlePosition.Outside)
		{
			rectangleF.Width += axisTitleSize.Width;
			if (IsLeftSide)
			{
				rectangleF.X -= axisTitleSize.Width;
			}
		}
		return rectangleF;
	}

	private void RenderLabels(StiContext context, RectangleF rect, StiYAxisGeom geom)
	{
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		if (base.Axis.Labels.Placement == StiLabelsPlacement.None)
		{
			return;
		}
		List<StiAxisLabelInfoXF> list = MeasureStripLines(context, rect);
		geom.CreateChildGeoms();
		foreach (StiAxisLabelInfoXF item2 in list)
		{
			if (!item2.ClientRectangle.IsEmpty)
			{
				StiAxisLabelGeom item = new StiAxisLabelGeom(base.Axis, item2.ClientRectangle, item2.TextPoint, item2.Text, item2.StripLine, item2.Angle, base.Axis.Labels.Width, item2.RotationMode, base.Axis.Labels.WordWrap);
				geom.ChildGeoms.Add(item);
			}
		}
	}

	private void RenderTitle(StiContext context, RectangleF axisRect, StiYAxisGeom geom)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected I4, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Expected I4, but got Unknown
		if (string.IsNullOrEmpty(base.Axis.Title.Text))
		{
			return;
		}
		SizeF axisTitleSize = GetAxisTitleSize(context);
		RectangleF rectangleF = RectangleF.Empty;
		if (IsLeftSide)
		{
			StringAlignment alignment = base.Axis.Title.Alignment;
			switch ((int)alignment)
			{
			case 0:
				rectangleF = new RectangleF(0f, axisRect.Height - axisTitleSize.Height + axisRect.Y, axisTitleSize.Width, axisTitleSize.Height);
				break;
			case 1:
				rectangleF = new RectangleF(0f, (axisRect.Height - axisTitleSize.Height) / 2f + axisRect.Y, axisTitleSize.Width, axisTitleSize.Height);
				break;
			case 2:
				rectangleF = new RectangleF(0f, axisRect.Y, axisTitleSize.Width, axisTitleSize.Height);
				break;
			}
			if (base.Axis.Title.Position == StiTitlePosition.Inside)
			{
				rectangleF.X += axisRect.Width;
			}
		}
		if (IsRightSide)
		{
			StringAlignment alignment2 = base.Axis.Title.Alignment;
			switch ((int)alignment2)
			{
			case 0:
				rectangleF = new RectangleF(axisRect.Width - axisTitleSize.Width, axisRect.Height - axisTitleSize.Height + axisRect.Y, axisTitleSize.Width, axisTitleSize.Height);
				break;
			case 1:
				rectangleF = new RectangleF(axisRect.Width - axisTitleSize.Width, (axisRect.Height - axisTitleSize.Height) / 2f + axisRect.Y, axisTitleSize.Width, axisTitleSize.Height);
				break;
			case 2:
				rectangleF = new RectangleF(axisRect.Width - axisTitleSize.Width, axisRect.Y, axisTitleSize.Width, axisTitleSize.Height);
				break;
			}
			if (base.Axis.Title.Position == StiTitlePosition.Inside)
			{
				rectangleF.X -= axisRect.Width;
			}
		}
		float num = base.Axis.Title.Font.Size * context.Options.Zoom;
		float maxWidth = 0f;
		if (((StiChart)base.Axis.Area.Chart).IsDashboard && CheckUseMaxWidth(axisRect, rectangleF, out maxWidth))
		{
			num = GetCorrectionFontSize(axisRect, rectangleF, num);
		}
		StiFontGeom font = StiFontGeom.ChangeFontSize(base.Axis.Title.Font, num);
		float angleTitle = GetAngleTitle();
		StiAxisTitleGeom item = new StiAxisTitleGeom(base.Axis, rectangleF, angleTitle, font);
		geom.CreateChildGeoms();
		geom.ChildGeoms.Add(item);
	}

	public StiYAxisCoreXF(IStiAxis axis)
		: base(axis)
	{
	}
}
