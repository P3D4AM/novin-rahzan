using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Chart;

public abstract class StiXAxisCoreXF : StiAxisCoreXF
{
	[Browsable(false)]
	private bool IsLabelsAngleByWidth
	{
		get
		{
			if (!(base.Axis.Labels.Angle < 30f))
			{
				return base.Axis.Labels.Angle > 150f;
			}
			return true;
		}
	}

	[Browsable(false)]
	public abstract StiXAxisDock Dock { get; }

	[Browsable(false)]
	public bool IsTopSide
	{
		get
		{
			if (Dock != 0 || base.Axis.Area.ReverseVert)
			{
				if (Dock == StiXAxisDock.Bottom)
				{
					return base.Axis.Area.ReverseVert;
				}
				return false;
			}
			return true;
		}
	}

	[Browsable(false)]
	public bool IsBottomSide
	{
		get
		{
			if (Dock != StiXAxisDock.Bottom || base.Axis.Area.ReverseVert)
			{
				if (Dock == StiXAxisDock.Top)
				{
					return base.Axis.Area.ReverseVert;
				}
				return false;
			}
			return true;
		}
	}

	public override bool GetStartFromZero()
	{
		if (base.Axis.Info.LabelsCollection != null && IsArgumentDateTime(base.Axis.Info.LabelsCollection) && (base.Axis.Area is StiScatterArea || base.Axis.Area is StiGanttArea))
		{
			return false;
		}
		return base.GetStartFromZero();
	}

	public override StiCellGeom Render(StiContext context, RectangleF rect)
	{
		if (!base.Axis.Visible)
		{
			return null;
		}
		RectangleF axisRect = GetAxisRect(context, rect, includeAxisArrow: false, includeLabelsWidth: false, isDrawing: true, includeScrollBar: false);
		axisRect.X = 0f;
		axisRect.Y = 0f;
		if (base.Axis.Interaction.ShowScrollBar && ((base.Axis is IStiXBottomAxis && !base.Axis.Area.ReverseVert) || (base.Axis is IStiXTopAxis && base.Axis.Area.ReverseVert)))
		{
			axisRect.Y += StiAxisCoreXF.DefaultScrollBarSize * context.Options.Zoom;
		}
		StiXAxisGeom stiXAxisGeom = new StiXAxisGeom(base.Axis as IStiXAxis, axisRect, isCenterAxis: false);
		RenderLabels(context, axisRect, stiXAxisGeom);
		RenderTitle(context, axisRect, stiXAxisGeom);
		return stiXAxisGeom;
	}

	public override StiCellGeom RenderView(StiContext context, RectangleF rect)
	{
		if (!base.Axis.Visible)
		{
			return null;
		}
		RectangleF axisRect = GetAxisRect(context, rect, includeAxisArrow: false, includeLabelsWidth: false, isDrawing: true, includeScrollBar: true);
		return new StiXAxisViewGeom(base.Axis as IStiXAxis, axisRect, isCenterAxis: false);
	}

	public void RenderScrollBar(StiContext context, RectangleF axisRect, StiXAxisViewGeom axisGeom)
	{
		if (base.Axis.Interaction.ShowScrollBar)
		{
			RectangleF rectangleF = axisRect;
			rectangleF.X = 0f;
			rectangleF.Height = StiAxisCoreXF.DefaultScrollBarSize * context.Options.Zoom;
			if ((base.Axis is IStiXTopAxis && !base.Axis.Area.ReverseVert) || (base.Axis is IStiXBottomAxis && base.Axis.Area.ReverseVert))
			{
				rectangleF.Y = axisRect.Height - rectangleF.Height;
			}
			else
			{
				rectangleF.Y = 0f;
			}
			StiHorzScrollBarGeom stiHorzScrollBarGeom = new StiHorzScrollBarGeom(base.Axis as IStiXAxis, rectangleF);
			axisGeom.CreateChildGeoms();
			axisGeom.ChildGeoms.Add(stiHorzScrollBarGeom);
			rectangleF.X = 0f;
			rectangleF.Y = 0f;
			rectangleF.Inflate(-2f, -2f);
			if (base.Axis.Interaction.RangeScrollEnabled)
			{
				RectangleF clientRectangle = rectangleF;
				clientRectangle.Width = rectangleF.Height;
				StiLeftButtonGeom item = new StiLeftButtonGeom(base.Axis as IStiXAxis, clientRectangle);
				stiHorzScrollBarGeom.CreateChildGeoms();
				stiHorzScrollBarGeom.ChildGeoms.Add(item);
				RectangleF clientRectangle2 = rectangleF;
				clientRectangle2.Width = rectangleF.Height;
				clientRectangle2.X = rectangleF.Right - clientRectangle2.Width;
				StiRightButtonGeom item2 = new StiRightButtonGeom(base.Axis as IStiXAxis, clientRectangle2);
				stiHorzScrollBarGeom.CreateChildGeoms();
				stiHorzScrollBarGeom.ChildGeoms.Add(item2);
				rectangleF.Inflate(0f - rectangleF.Height - 2f, 0f);
			}
			RectangleF clientRectangle3 = rectangleF;
			IStiXAxis xAxis = base.Axis.Area.XAxis;
			StiAxisAreaCoreXF stiAxisAreaCoreXF = base.Axis.Area.Core as StiAxisAreaCoreXF;
			if (!xAxis.Range.Auto)
			{
				float num = (float)((double)rectangleF.Width / stiAxisAreaCoreXF.ScrollRangeX);
				float num2 = (float)stiAxisAreaCoreXF.ScrollValueX * num;
				float width = ((stiAxisAreaCoreXF.ScrollViewX < stiAxisAreaCoreXF.ScrollRangeX) ? ((float)stiAxisAreaCoreXF.ScrollViewX * num) : ((float)stiAxisAreaCoreXF.ScrollRangeX * num));
				clientRectangle3 = new RectangleF(num2 + rectangleF.X, rectangleF.Y, width, rectangleF.Height);
			}
			StiHorzTrackBarGeom item3 = new StiHorzTrackBarGeom(base.Axis as IStiXAxis, clientRectangle3, stiHorzScrollBarGeom);
			stiHorzScrollBarGeom.CreateChildGeoms();
			stiHorzScrollBarGeom.ChildGeoms.Add(item3);
		}
	}

	public StiCellGeom RenderCenter(StiContext context, RectangleF rect)
	{
		if (!base.Axis.Visible)
		{
			return null;
		}
		if (base.Axis.Area.YAxis.Info.Minimum >= 0.0)
		{
			return null;
		}
		RectangleF centerAxisRect = GetCenterAxisRect(context, rect, includeAxisArrow: false, includeLabelsHeight: false, isDrawing: true);
		centerAxisRect.X = 0f;
		centerAxisRect.Y = 0f;
		return new StiXAxisGeom(base.Axis as IStiXAxis, centerAxisRect, isCenterAxis: true);
	}

	public StiCellGeom RenderCenterView(StiContext context, RectangleF rect)
	{
		if (!base.Axis.Visible)
		{
			return null;
		}
		if (base.Axis.Area.YAxis.Info.Minimum >= 0.0)
		{
			return null;
		}
		RectangleF centerAxisRect = GetCenterAxisRect(context, rect, includeAxisArrow: false, includeLabelsHeight: false, isDrawing: true);
		return new StiXAxisViewGeom(base.Axis as IStiXAxis, centerAxisRect, isCenterAxis: true);
	}

	internal string GetLabelText(StiStripLineXF line, IStiSeries series)
	{
		return GetLabelText(line.ValueObject, line.Value, series);
	}

	internal string GetLabelText(object objectValue, double value, IStiSeries series)
	{
		CultureInfo cultureInfo = null;
		try
		{
			if (base.Axis.Labels.FormatService != null && !(base.Axis.Labels.FormatService is StiGeneralFormatService))
			{
				if (objectValue is DateTime)
				{
					if (base.Axis.Labels.FormatService is StiDateFormatService || base.Axis.Labels.FormatService is StiTimeFormatService)
					{
						return base.Axis.Labels.TextBefore + base.Axis.Labels.FormatService.Format(objectValue) + base.Axis.Labels.TextAfter;
					}
					return $"{base.Axis.Labels.TextBefore}{objectValue}{base.Axis.Labels.TextAfter}";
				}
				return base.Axis.Labels.TextBefore + base.Axis.Labels.FormatService.Format(value) + base.Axis.Labels.TextAfter;
			}
			string text = ((StiChart)base.Axis.Area.Chart)?.Report?.Culture;
			if (!string.IsNullOrEmpty(((StiChart)base.Axis.Area.Chart)?.Report?.Culture))
			{
				cultureInfo = Thread.CurrentThread.CurrentCulture;
				Thread.CurrentThread.CurrentCulture = new CultureInfo(text);
			}
			string text2 = null;
			if (series != null)
			{
				text2 = series.Format;
			}
			if (text2 == null || text2.Trim().Length == 0)
			{
				text2 = base.Axis.Labels.Format;
			}
			if (text2 != null && text2.Trim().Length != 0)
			{
				if (objectValue is string)
				{
					string s = objectValue.ToString().Replace(".", ",").Replace(",", Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
					if (decimal.TryParse(s, out var result))
					{
						objectValue = result;
					}
					else
					{
						string name = (string.IsNullOrEmpty(text) ? "en-US" : text);
						if (DateTime.TryParse(objectValue.ToString(), new CultureInfo(name, useUserOverride: true), DateTimeStyles.None, out var result2))
						{
							objectValue = result2;
						}
						else if (DateTime.TryParse(objectValue.ToString(), out result2))
						{
							objectValue = result2;
						}
					}
				}
				else if (objectValue == null)
				{
					return string.Empty;
				}
				if (!StiChartOptions.OldChartPercentMode && text2.StartsWith("P", StringComparison.InvariantCulture))
				{
					int result3 = 0;
					if (text2.Length > 1)
					{
						int.TryParse(text2.Remove(0, 1), out result3);
					}
					return string.Format("{0}{1:N" + result3 + "}{2}{3}", base.Axis.Labels.TextBefore, objectValue, "%", base.Axis.Labels.TextAfter);
				}
				StiErrorFormatEntity stiErrorFormatEntity = default(StiErrorFormatEntity);
				stiErrorFormatEntity.Format = text2;
				stiErrorFormatEntity.Value = objectValue;
				StiErrorFormatEntity item = stiErrorFormatEntity;
				try
				{
					if (!StiErrorFormatEntity.Hash.Contains(item))
					{
						return string.Format("{0}{1:" + text2 + "}{2}", base.Axis.Labels.TextBefore, objectValue, base.Axis.Labels.TextAfter);
					}
				}
				catch
				{
					StiErrorFormatEntity.Hash.Add(item);
				}
			}
			return $"{base.Axis.Labels.TextBefore}{objectValue}{base.Axis.Labels.TextAfter}";
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
		return objectValue.ToString();
	}

	private List<StiAxisLabelInfoXF> MeasureAxisLabelInfos(RectangleF rect, int start, int end, StiContext context)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		List<StiAxisLabelInfoXF> list = new List<StiAxisLabelInfoXF>();
		StiContextPainter stiContextPainter = context.ContextPainter.Clone();
		StiStringFormatGeom stringFormatGeom = GetStringFormatGeom(context, base.Axis.Labels.WordWrap);
		StiFontGeom fontGeom = GetFontGeom(context);
		StiRotationMode val = (StiRotationMode)4;
		float angle = 0f - base.Axis.Labels.Angle;
		PointF empty = PointF.Empty;
		val = ((!IsTopSide) ? ((StiRotationMode)3) : ((StiRotationMode)5));
		bool flag = (base.Axis.Area.ReverseVert ? IsBottomSide : IsTopSide);
		for (int i = start; i < end && i < base.Axis.Info.LabelsCollection.Count; i++)
		{
			if (flag && base.Axis.Area.YRightAxis.ArrowStyle != 0 && i == 0)
			{
				list.Add(new StiAxisLabelInfoXF());
				i++;
				continue;
			}
			if (flag && base.Axis.Area.YAxis.ArrowStyle != 0 && i == base.Axis.Info.StripLines.Count - 1)
			{
				list.Add(new StiAxisLabelInfoXF());
				i++;
				continue;
			}
			StiStripPositionXF stiStripPositionXF = base.Axis.Info.LabelsCollection[i];
			string labelText = GetLabelText(stiStripPositionXF.StripLine, null);
			float position = stiStripPositionXF.Position;
			empty = ((!IsTopSide) ? new PointF(position, GetTicksMaxLength(context) + GetLabelsSpaceAxis(context)) : new PointF(position, rect.Height - (GetTicksMaxLength(context) + GetLabelsSpaceAxis(context))));
			if (base.Axis.Labels.Placement != 0)
			{
				RectangleF value = stiContextPainter.MeasureRotatedString(labelText, fontGeom, empty, stringFormatGeom, val, angle, (int)(base.Axis.Labels.Width * context.Options.Zoom));
				StiAxisLabelInfoXF item = new StiAxisLabelInfoXF
				{
					Angle = angle,
					ClientRectangle = Rectangle.Round(value),
					RotationMode = val,
					TextPoint = empty,
					Text = labelText,
					StripLine = stiStripPositionXF.StripLine
				};
				list.Add(item);
			}
			else
			{
				list.Add(new StiAxisLabelInfoXF());
			}
		}
		stiContextPainter.Dispose();
		return list;
	}

	internal static string GetMeasureText(StiContext context, string text, Font font, double maxWidth)
	{
		for (int i = 0; text.Length > i; i++)
		{
			string text2 = text.Substring(0, i);
			if (maxWidth < (double)context.MeasureString(text2, new StiFontGeom(font)).Width)
			{
				if (text2.Length > 4)
				{
					return text2.Substring(0, text2.Length - 3) + "...";
				}
				if (text2.Length > 1)
				{
					return text2.Substring(0, 1) + "...";
				}
			}
		}
		return text;
	}

	private bool CheckAutoAngleLabels(StiContext context, RectangleF rect)
	{
		if (base.Axis.Labels.Placement != StiLabelsPlacement.AutoRotation)
		{
			return false;
		}
		List<StiAxisLabelInfoXF> list = new List<StiAxisLabelInfoXF>();
		if (base.Axis.Info.LabelsCollection != null && base.Axis.Labels.Placement != 0)
		{
			if (base.Axis.Info.LabelsCollection.Count < 400)
			{
				list = MeasureAxisLabelInfos(rect, 0, base.Axis.Info.LabelsCollection.Count, context);
			}
			else
			{
				Func<object, object[]> function = delegate(object obj)
				{
					object[] array = (object[])obj;
					int start = (int)array[0];
					int end = (int)array[1];
					StiContext context2 = (StiContext)array[2];
					List<StiAxisLabelInfoXF> list3 = MeasureAxisLabelInfos(rect, start, end, context2);
					return new object[1] { list3 };
				};
				List<Task<object[]>> list2 = new List<Task<object[]>>();
				int num = 5;
				int num2 = base.Axis.Info.LabelsCollection.Count / num;
				for (int i = 0; i < num; i++)
				{
					int num3 = (i + 1) * num2;
					if (i == num - 1)
					{
						num3 = base.Axis.Info.LabelsCollection.Count;
					}
					object[] state = new object[3]
					{
						i * num2,
						num3,
						context
					};
					list2.Add(Task<object[]>.Factory.StartNew(function, state));
				}
				Task[] tasks = list2.ToArray();
				Task.WaitAll(tasks);
				foreach (Task<object[]> item in list2)
				{
					list.AddRange((List<StiAxisLabelInfoXF>)item.Result[0]);
				}
			}
			for (int j = 1; j < list.Count; j++)
			{
				RectangleF clientRectangle = list[j - 1].ClientRectangle;
				RectangleF clientRectangle2 = list[j].ClientRectangle;
				if (RectangleF.Intersect(clientRectangle, clientRectangle2).Width > 0f)
				{
					return true;
				}
			}
		}
		return false;
	}

	private List<StiAxisLabelInfoXF> MeasureAxisLabelInfos(RectangleF rect, float angle, int start, int end, StiContext context, float labelsWidth, StiRotationMode? rotation, bool wordWrap, out float maxLabelWidth)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Invalid comparison between Unknown and I4
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Invalid comparison between Unknown and I4
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Invalid comparison between Unknown and I4
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		List<StiAxisLabelInfoXF> list = new List<StiAxisLabelInfoXF>();
		maxLabelWidth = 0f;
		StiContextPainter stiContextPainter = context.ContextPainter.Clone();
		StiStringFormatGeom stringFormatGeom = GetStringFormatGeom(context, wordWrap);
		StiFontGeom fontGeom = GetFontGeom(context);
		StiHorAlignment textAlignment = GetTextAlignment();
		StiRotationMode val = (StiRotationMode)4;
		PointF empty = PointF.Empty;
		val = (StiRotationMode)(((_003F?)rotation) ?? (IsTopSide ? ((angle == 0f) ? 5 : (((int)base.Axis.Labels.TextAlignment != 2) ? 1 : 4)) : ((angle == 0f) ? 3 : (((int)base.Axis.Labels.TextAlignment != 2) ? 7 : 4))));
		bool flag = (base.Axis.Area.ReverseVert ? IsBottomSide : IsTopSide);
		for (int i = start; i < end && i < base.Axis.Info.LabelsCollection.Count; i++)
		{
			if (flag && base.Axis.Area.YRightAxis.ArrowStyle != 0 && i == 0)
			{
				list.Add(new StiAxisLabelInfoXF());
				i++;
				continue;
			}
			if (flag && base.Axis.Area.YAxis.ArrowStyle != 0 && i == base.Axis.Info.StripLines.Count - 1)
			{
				list.Add(new StiAxisLabelInfoXF());
				i++;
				continue;
			}
			StiStripPositionXF stiStripPositionXF = base.Axis.Info.LabelsCollection[i];
			string text = GetLabelText(stiStripPositionXF.StripLine, null);
			if (!wordWrap && labelsWidth > 0f)
			{
				text = GetMeasureText(context, text, base.Axis.Labels.Font, labelsWidth);
			}
			float position = stiStripPositionXF.Position;
			empty = ((!IsTopSide) ? new PointF(position, GetTicksMaxLength(context) + GetLabelsSpaceAxis(context)) : new PointF(position, rect.Height - (GetTicksMaxLength(context) + GetLabelsSpaceAxis(context))));
			if (base.Axis.Labels.Placement != 0)
			{
				if (base.Axis.Labels.Placement == StiLabelsPlacement.TwoLines && ((uint)i & (true ? 1u : 0u)) != 0)
				{
					if (IsTopSide)
					{
						empty.Y -= GetLabelsTwoLinesDestination(context);
					}
					if (IsBottomSide)
					{
						empty.Y += GetLabelsTwoLinesDestination(context);
					}
				}
				RectangleF value = stiContextPainter.MeasureRotatedString(text, fontGeom, empty, stringFormatGeom, val, angle, (int)(labelsWidth * context.Options.Zoom));
				PointF empty2 = PointF.Empty;
				if ((int)val == 4)
				{
					empty.Y = (IsTopSide ? (empty.Y - value.Height / 2f) : (empty.Y + value.Height / 2f));
				}
				StiAxisLabelInfoXF item = new StiAxisLabelInfoXF
				{
					Angle = angle,
					Width = labelsWidth,
					WordWrap = wordWrap,
					ClientRectangle = Rectangle.Round(value),
					RotationMode = val,
					TextPoint = empty,
					Text = text,
					StripLine = stiStripPositionXF.StripLine
				};
				list.Add(item);
				if (IsLabelsAngleByWidth)
				{
					maxLabelWidth = Math.Max(maxLabelWidth, value.Width);
				}
			}
			else
			{
				list.Add(new StiAxisLabelInfoXF());
			}
		}
		stiContextPainter.Dispose();
		return list;
	}

	private float GetMaxHeightLabels(List<StiAxisLabelInfoXF> infos)
	{
		RectangleF a = RectangleF.Empty;
		foreach (StiAxisLabelInfoXF info in infos)
		{
			if (!info.ClientRectangle.IsEmpty)
			{
				a = ((!a.IsEmpty) ? RectangleF.Union(a, info.ClientRectangle) : info.ClientRectangle);
			}
		}
		return a.Height;
	}

	private List<StiAxisLabelInfoXF> MeasureStripLines(StiContext context, RectangleF rect)
	{
		float labelsWidth = base.Axis.Labels.Width;
		StiRotationMode? rotationMode = null;
		bool wordWrap = base.Axis.Labels.WordWrap;
		float angle = 0f - base.Axis.Labels.Angle;
		bool flag = CheckAutoAngleLabels(context, rect);
		if (flag)
		{
			angle = -45f;
			rotationMode = (StiRotationMode)((base.Axis is IStiXTopAxis) ? 2 : 6);
			wordWrap = false;
			float height = ((StiChart)base.Axis.Area.Chart).Core.FullRectangle.Height;
			labelsWidth = height / 4f;
		}
		List<StiAxisLabelInfoXF> list = new List<StiAxisLabelInfoXF>();
		if (base.Axis.Info.LabelsCollection != null && base.Axis.Labels.Placement != 0)
		{
			float maxLabelWidth = 0f;
			if (base.Axis.Info.LabelsCollection.Count < 400)
			{
				list = MeasureAxisLabelInfos(rect, angle, 0, base.Axis.Info.LabelsCollection.Count, context, labelsWidth, rotationMode, wordWrap, out maxLabelWidth);
			}
			else
			{
				List<Task<object[]>> list2 = new List<Task<object[]>>();
				Func<object, object[]> function = delegate(object obj)
				{
					object[] array = (object[])obj;
					int start = (int)array[0];
					int end = (int)array[1];
					StiContext context2 = (StiContext)array[2];
					float maxLabelWidth2 = 0f;
					List<StiAxisLabelInfoXF> list4 = MeasureAxisLabelInfos(rect, angle, start, end, context2, labelsWidth, rotationMode, wordWrap, out maxLabelWidth2);
					return new object[2] { list4, maxLabelWidth };
				};
				int num = 5;
				int num2 = base.Axis.Info.LabelsCollection.Count / num;
				for (int i = 0; i < num; i++)
				{
					int num3 = (i + 1) * num2;
					if (i == num - 1)
					{
						num3 = base.Axis.Info.LabelsCollection.Count;
					}
					object[] state = new object[3]
					{
						i * num2,
						num3,
						context
					};
					list2.Add(Task<object[]>.Factory.StartNew(function, state));
				}
				Task[] tasks = list2.ToArray();
				Task.WaitAll(tasks);
				foreach (Task<object[]> item in list2)
				{
					list.AddRange((List<StiAxisLabelInfoXF>)item.Result[0]);
					maxLabelWidth = Math.Max(maxLabelWidth, (float)item.Result[1]);
				}
			}
			int num4 = 1;
			if (base.Axis.Range.Auto)
			{
				if (flag)
				{
					maxLabelWidth = (float)base.Axis.Labels.Font.Height * 1.2f * context.Options.Zoom;
					float num5 = rect.Width / (float)base.Axis.Info.StripLines.Count;
					num4 = (int)Math.Ceiling(maxLabelWidth / num5);
				}
				else
				{
					if (!IsLabelsAngleByWidth)
					{
						maxLabelWidth = (float)base.Axis.Labels.Font.Height * 1.5f * context.Options.Zoom;
					}
					float num6 = rect.Width / (float)base.Axis.Info.StripLines.Count;
					float num7 = maxLabelWidth / num6;
					if (num7 > 1f)
					{
						num4 = (int)StiStripLineCalculatorXF.GetInterval(0.0, base.Axis.Info.LabelsCollection.Count, 6);
						if (base.Axis.LogarithmicScale)
						{
							num4 = 1;
						}
					}
				}
			}
			else
			{
				num4 = (int)Math.Max(1f, base.Axis.Step);
			}
			bool flag2 = false;
			float y = 0f;
			float y2 = 0f;
			if ((flag || base.Axis.Labels.Placement == StiLabelsPlacement.TwoLines) && list.Count > 1)
			{
				y2 = list[0].TextPoint.Y;
				y = list[1].TextPoint.Y;
			}
			if (num4 == 1 || base.Axis.Labels.Step != 0f)
			{
				return list;
			}
			int num8 = 0;
			List<StiAxisLabelInfoXF> list3 = new List<StiAxisLabelInfoXF>();
			{
				foreach (StiAxisLabelInfoXF item2 in list)
				{
					if (num8 == 0)
					{
						if (base.Axis.Labels.Placement == StiLabelsPlacement.TwoLines && list.Count > 1)
						{
							if (flag2)
							{
								item2.TextPoint.Y = y;
								flag2 = false;
							}
							else
							{
								item2.TextPoint.Y = y2;
								flag2 = true;
							}
						}
						list3.Add(item2);
					}
					num8++;
					if (num8 == num4)
					{
						num8 = 0;
					}
				}
				return list3;
			}
		}
		return list;
	}

	public RectangleF GetCenterAxisRect(StiContext context, RectangleF rect, bool includeAxisArrow, bool includeLabelsHeight, bool isDrawing)
	{
		if (base.Axis.Area.YAxis.Info.Minimum >= 0.0)
		{
			return RectangleF.Empty;
		}
		float y = 0f - GetTicksMaxLength(context);
		return new RectangleF(0f, y, rect.Width, GetTicksMaxLength(context));
	}

	public RectangleF GetAxisRect(StiContext context, RectangleF rect, bool includeAxisArrow, bool includeLabelsWidth, bool isDrawing, bool includeScrollBar)
	{
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Invalid comparison between Unknown and I4
		RectangleF rectangleF = RectangleF.Empty;
		if (!base.Axis.Visible)
		{
			return rectangleF;
		}
		List<StiAxisLabelInfoXF> list = MeasureStripLines(context, rect);
		if (list.Count == 0)
		{
			rectangleF = ((!IsTopSide) ? new RectangleF(0f, rect.Height, rect.Width, GetTicksMaxLength(context)) : new RectangleF(0f, 0f - GetTicksMaxLength(context), rect.Width, GetTicksMaxLength(context)));
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
			bool flag = context.ContextPainter is StiGdiContextPainter;
			if (rectangleF.Y < 0f && !IsTopSide && (int)base.Axis.Labels.TextAlignment == 3 && flag)
			{
				rectangleF.Height += rectangleF.Y;
				rectangleF.Y = 0f;
			}
			if (base.Axis.Interaction.ShowScrollBar && includeScrollBar)
			{
				rectangleF.Height += StiAxisCoreXF.DefaultScrollBarSize * context.Options.Zoom;
			}
			rectangleF.Height += GetTicksMaxLength(context) + GetLabelsSpaceAxis(context);
			RectangleF rectangleF2 = rectangleF;
			if (IsTopSide)
			{
				rectangleF = new RectangleF(0f, 0f - rectangleF.Height, rect.Width, rectangleF.Height);
			}
			if (IsBottomSide)
			{
				rectangleF = new RectangleF(0f, rect.Height, rect.Width, rectangleF.Height);
			}
			if (includeLabelsWidth)
			{
				rectangleF.X = rectangleF2.X;
				rectangleF.Width = rectangleF2.Width;
			}
		}
		if (base.Axis.ArrowStyle != StiArrowStyle.None && includeAxisArrow)
		{
			float num = GetArrowHeight(context);
			if (base.Axis.Area.ReverseHor)
			{
				num = 0f - num;
			}
			rectangleF = RectangleF.Union(rectangleF, new RectangleF(rect.Width, rectangleF.Y + rectangleF.Height / 2f, num, 1f));
		}
		SizeF axisTitleSize = GetAxisTitleSize(context);
		if (!axisTitleSize.IsEmpty && base.Axis.Title.Position == StiTitlePosition.Outside)
		{
			rectangleF.Height += axisTitleSize.Height;
			if (rectangleF.Width == 0f)
			{
				rectangleF.Width = axisTitleSize.Width;
			}
			if (IsTopSide)
			{
				rectangleF.Y -= axisTitleSize.Height;
			}
		}
		return rectangleF;
	}

	private void RenderLabels(StiContext context, RectangleF rect, StiXAxisGeom geom)
	{
		//IL_0858: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b5: Unknown result type (might be due to invalid IL or missing references)
		if (base.Axis.Info.StripLines == null || base.Axis.Labels.Placement == StiLabelsPlacement.None)
		{
			return;
		}
		List<StiAxisLabelInfoXF> list = MeasureStripLines(context, rect);
		geom.CreateChildGeoms();
		StiXBottomAxis stiXBottomAxis = base.Axis.Area.XAxis as StiXBottomAxis;
		if (IsArgumentDateTime(list) && stiXBottomAxis != null && stiXBottomAxis.DateTimeStep.Step != 0)
		{
			DateTime dateTime = default(DateTime);
			DateTime dateTime2 = default(DateTime);
			List<StiStripPositionXF> labelsCollection = stiXBottomAxis.Info.LabelsCollection;
			List<StiAxisLabelInfoXF> list2 = new List<StiAxisLabelInfoXF>();
			string text = string.Empty;
			bool flag = true;
			float num = 0f;
			for (int i = 0; i < labelsCollection.Count; i++)
			{
				if (labelsCollection[i].StripLine.ValueObject == null || !(labelsCollection[i].StripLine.ValueObject is DateTime))
				{
					continue;
				}
				dateTime = (DateTime)labelsCollection[i].StripLine.ValueObject;
				dateTime2 = ((i >= labelsCollection.Count - 2 || stiXBottomAxis.Area is StiScatterArea) ? dateTime : ((DateTime)labelsCollection[i + 1].StripLine.ValueObject));
				if (flag)
				{
					num = labelsCollection[i].Position;
					flag = false;
				}
				switch (((StiXBottomAxis)base.Axis.Area.XAxis).DateTimeStep.Step)
				{
				case StiTimeDateStep.Day:
					if (dateTime.ToString("yyyyMMMMdd") != dateTime2.ToString("yyyyMMMMdd") || i == labelsCollection.Count - 2)
					{
						text = dateTime.ToString("dd");
					}
					break;
				case StiTimeDateStep.Hour:
					if (dateTime.ToString("yyyyMMMMddHH") != dateTime2.ToString("yyyyMMMMddHH") || i == labelsCollection.Count - 2)
					{
						text = dateTime.ToString("HH");
					}
					break;
				case StiTimeDateStep.Minute:
					if (dateTime.ToString("yyyyMMMMddHHmm") != dateTime2.ToString("yyyyMMMMddHHmm") || i == labelsCollection.Count - 2)
					{
						text = dateTime.ToString("mm");
					}
					break;
				case StiTimeDateStep.Month:
					if (dateTime.ToString("yyyyMMMM") != dateTime2.ToString("yyyyMMMM") || i == labelsCollection.Count - 2)
					{
						text = StiLocalization.Get("A_WebViewer", "Month" + dateTime.ToString("MMMM", CultureInfo.GetCultureInfo("en-US")));
					}
					break;
				case StiTimeDateStep.Second:
					if (dateTime.ToString("yyyyMMMMddHHmmss") != dateTime2.ToString("yyyyMMMMddHHmmss") || i == labelsCollection.Count - 2)
					{
						text = dateTime.ToString("ss");
					}
					break;
				case StiTimeDateStep.Year:
					if (dateTime.ToString("yyyy") != dateTime2.ToString("yyyy") || i == labelsCollection.Count - 2)
					{
						text = dateTime.ToString("yyyy");
					}
					break;
				}
				if (text != string.Empty)
				{
					PointF pointF = new PointF((labelsCollection[i].Position + num) / 2f, GetTicksMaxLength(context) + GetLabelsSpaceAxis(context));
					float angle = 0f;
					StiAxisLabelInfoXF stiAxisLabelInfoXF = new StiAxisLabelInfoXF();
					stiAxisLabelInfoXF.Angle = angle;
					stiAxisLabelInfoXF.ClientRectangle = context.MeasureRotatedString(text, GetFontGeom(context), pointF, GetStringFormatGeom(context, stiAxisLabelInfoXF.WordWrap), (StiRotationMode)4, angle, (int)(base.Axis.Labels.Width * context.Options.Zoom));
					stiAxisLabelInfoXF.RotationMode = (StiRotationMode)4;
					stiAxisLabelInfoXF.StripLine = labelsCollection[i].StripLine;
					stiAxisLabelInfoXF.Text = text;
					stiAxisLabelInfoXF.TextPoint = pointF;
					list2.Add(stiAxisLabelInfoXF);
					flag = true;
					text = string.Empty;
				}
			}
			flag = true;
			List<StiAxisLabelInfoXF> list3 = new List<StiAxisLabelInfoXF>();
			for (int j = 0; j < list2.Count - 1; j++)
			{
				dateTime = (DateTime)list2[j].StripLine.ValueObject;
				dateTime2 = (DateTime)list2[j + 1].StripLine.ValueObject;
				if (flag)
				{
					num = list2[j].TextPoint.X;
					flag = false;
				}
				switch (((StiXBottomAxis)base.Axis.Area.XAxis).DateTimeStep.Step)
				{
				case StiTimeDateStep.Day:
					if (dateTime.ToString("yyyyMMMM") != dateTime2.ToString("yyyyMMMM") || j == list2.Count - 2)
					{
						text = StiLocalization.Get("A_WebViewer", "Month" + dateTime.ToString("MMMM", CultureInfo.GetCultureInfo("en-US")));
					}
					break;
				case StiTimeDateStep.Hour:
					if (dateTime.ToString("yyyyMMMMdd") != dateTime2.ToString("yyyyMMMMdd") || j == list2.Count - 2)
					{
						text = dateTime.ToString("dd");
					}
					break;
				case StiTimeDateStep.Minute:
					if (dateTime.ToString("yyyyMMMMddhh") != dateTime2.ToString("yyyyMMMMddhh") || j == list2.Count - 2)
					{
						text = dateTime.ToString("hh");
					}
					break;
				case StiTimeDateStep.Month:
					if (dateTime.ToString("yyyy") != dateTime2.ToString("yyyy") || j == list2.Count - 2)
					{
						text = dateTime.ToString("yyyy");
					}
					break;
				case StiTimeDateStep.Second:
					if (dateTime.ToString("yyyyMMMMddhhmm") != dateTime2.ToString("yyyyMMMMddhhmm") || j == list2.Count - 2)
					{
						text = dateTime.ToString("mm");
					}
					break;
				case StiTimeDateStep.Year:
					if (dateTime.ToString("yyyy") != dateTime2.ToString("yyyy") || j == list2.Count - 2)
					{
						continue;
					}
					break;
				}
				if (text != string.Empty)
				{
					PointF pointF2 = new PointF((list2[j + 1].TextPoint.X + num) / 2f, GetTicksMaxLength(context) + 2f * GetLabelsSpaceAxis(context));
					float angle2 = 0f;
					StiAxisLabelInfoXF stiAxisLabelInfoXF2 = new StiAxisLabelInfoXF();
					stiAxisLabelInfoXF2.Angle = angle2;
					stiAxisLabelInfoXF2.ClientRectangle = context.MeasureRotatedString(text, GetFontGeom(context), pointF2, GetStringFormatGeom(context, stiAxisLabelInfoXF2.WordWrap), (StiRotationMode)4, angle2, (int)(base.Axis.Labels.Width * context.Options.Zoom));
					stiAxisLabelInfoXF2.RotationMode = (StiRotationMode)3;
					stiAxisLabelInfoXF2.StripLine = list2[j].StripLine;
					stiAxisLabelInfoXF2.Text = text;
					stiAxisLabelInfoXF2.TextPoint = pointF2;
					list3.Add(stiAxisLabelInfoXF2);
					flag = true;
					text = string.Empty;
				}
			}
			list2.AddRange(list3);
			foreach (StiAxisLabelInfoXF item3 in list2)
			{
				if (!item3.ClientRectangle.IsEmpty)
				{
					StiAxisLabelGeom item = new StiAxisLabelGeom(base.Axis, item3.ClientRectangle, item3.TextPoint, item3.Text, item3.StripLine, item3.Angle, item3.Width, item3.RotationMode, item3.WordWrap);
					geom.ChildGeoms.Add(item);
				}
			}
			base.Axis.Info.LabelInfoCollection = list2;
			return;
		}
		foreach (StiAxisLabelInfoXF item4 in list)
		{
			if (!item4.ClientRectangle.IsEmpty)
			{
				StiAxisLabelGeom item2 = new StiAxisLabelGeom(base.Axis, item4.ClientRectangle, item4.TextPoint, item4.Text, item4.StripLine, item4.Angle, item4.Width, item4.RotationMode, item4.WordWrap);
				geom.ChildGeoms.Add(item2);
			}
		}
		base.Axis.Info.LabelInfoCollection = list;
	}

	private void RenderTitle(StiContext context, RectangleF axisRect, StiXAxisGeom geom)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected I4, but got Unknown
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Expected I4, but got Unknown
		if (string.IsNullOrEmpty(base.Axis.Title.Text))
		{
			return;
		}
		SizeF axisTitleSize = GetAxisTitleSize(context);
		RectangleF rectangleF = RectangleF.Empty;
		if (IsTopSide)
		{
			StringAlignment alignment = base.Axis.Title.Alignment;
			switch ((int)alignment)
			{
			case 0:
				rectangleF = new RectangleF(0f, 0f, axisTitleSize.Width, axisTitleSize.Height);
				break;
			case 1:
				rectangleF = new RectangleF((axisRect.Width - axisTitleSize.Width) / 2f, 0f, axisTitleSize.Width, axisTitleSize.Height);
				break;
			case 2:
				rectangleF = new RectangleF(axisRect.Width - axisTitleSize.Width, 0f, axisTitleSize.Width, axisTitleSize.Height);
				break;
			}
			if (base.Axis.Title.Position == StiTitlePosition.Inside)
			{
				rectangleF.Y += axisRect.Height;
			}
		}
		if (IsBottomSide)
		{
			StringAlignment alignment2 = base.Axis.Title.Alignment;
			switch ((int)alignment2)
			{
			case 0:
				rectangleF = new RectangleF(0f, axisRect.Height - axisTitleSize.Height, axisTitleSize.Width, axisTitleSize.Height);
				break;
			case 1:
				rectangleF = new RectangleF((axisRect.Width - axisTitleSize.Width) / 2f, axisRect.Height - axisTitleSize.Height, axisTitleSize.Width, axisTitleSize.Height);
				break;
			case 2:
				rectangleF = new RectangleF(axisRect.Width - axisTitleSize.Width, axisRect.Height - axisTitleSize.Height, axisTitleSize.Width, axisTitleSize.Height);
				break;
			}
			if (base.Axis.Title.Position == StiTitlePosition.Inside)
			{
				rectangleF.Y -= axisRect.Height;
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

	private bool IsArgumentDateTime(List<StiStripPositionXF> positions)
	{
		foreach (StiStripPositionXF position in positions)
		{
			if (position.StripLine.ValueObject is DateTime)
			{
				return true;
			}
		}
		return false;
	}

	private bool IsArgumentDateTime(List<StiAxisLabelInfoXF> infos)
	{
		foreach (StiAxisLabelInfoXF info in infos)
		{
			if (info.StripLine != null && info.StripLine.ValueObject is DateTime)
			{
				return true;
			}
		}
		return false;
	}

	public StiXAxisCoreXF(IStiAxis axis)
		: base(axis)
	{
	}
}
