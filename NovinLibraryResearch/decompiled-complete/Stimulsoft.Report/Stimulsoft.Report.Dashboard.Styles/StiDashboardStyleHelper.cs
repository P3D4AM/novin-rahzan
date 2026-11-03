using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using Stimulsoft.Base.Dashboard;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.App;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard.Helpers;
using Stimulsoft.Report.Dashboard.Styles.Cards;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Maps;
using Stimulsoft.Report.Surface;

namespace Stimulsoft.Report.Dashboard.Styles;

public static class StiDashboardStyleHelper
{
	private static FontFamily iconFontFamily;

	private static Color[] CloneColors(Color[] value)
	{
		if (value == null)
		{
			return null;
		}
		Color[] array = new Color[value.Length];
		for (int i = 0; i < value.Length; i++)
		{
			array[i] = value[i];
		}
		return array;
	}

	public static StiChartStyle GetCopyChartStyle(IStiChartStyle chartStyle, IStiChartElement element)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Expected O, but got Unknown
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Expected O, but got Unknown
		StiStyleCoreXF core = chartStyle.Core;
		return new StiChartStyle
		{
			Brush = (StiBrush)new StiSolidBrush(GetBackColor(element, null, allowOpacity: true)),
			ChartAreaBrush = (StiBrush)core.ChartAreaBrush.Clone(),
			ChartAreaBorderColor = core.ChartAreaBorderColor,
			ChartAreaShowShadow = core.ChartAreaShowShadow,
			SeriesLighting = core.SeriesLighting,
			SeriesShowShadow = core.SeriesShowShadow,
			SeriesShowBorder = core.SeriesShowBorder,
			SeriesLabelsBrush = (StiBrush)core.SeriesLabelsBrush.Clone(),
			SeriesLabelsColor = core.SeriesLabelsColor,
			SeriesLabelsBorderColor = core.SeriesLabelsBorderColor,
			SeriesLabelsLineColor = core.SeriesLabelsLineColor,
			TrendLineColor = core.TrendLineColor,
			TrendLineShowShadow = core.TrendLineShowShadow,
			LegendBrush = core.LegendBrush,
			LegendLabelsColor = core.LegendLabelsColor,
			LegendBorderColor = core.LegendBorderColor,
			LegendTitleColor = core.LegendTitleColor,
			AxisTitleColor = core.AxisTitleColor,
			AxisLineColor = core.AxisLineColor,
			AxisLabelsColor = core.AxisLabelsColor,
			MarkerVisible = core.MarkerVisible,
			InterlacingHorBrush = (StiBrush)core.InterlacingHorBrush.Clone(),
			InterlacingVertBrush = (StiBrush)core.InterlacingVertBrush.Clone(),
			GridLinesHorColor = core.GridLinesHorColor,
			GridLinesVertColor = core.GridLinesVertColor,
			StyleColors = CloneColors(core.StyleColors),
			BasicStyleColor = core.BasicStyleColor
		};
	}

	public static StiTableStyle GetCopyTableStyle(IStiTableElement element, StiTableElementStyle tableStyle)
	{
		return new StiTableStyle
		{
			BackColor = GetBackColor(element, null, allowOpacity: true),
			AlternatingDataColor = tableStyle.AlternatingCellBackColor,
			AlternatingDataForeground = tableStyle.AlternatingCellForeColor,
			DataColor = tableStyle.CellBackColor,
			DataForeground = tableStyle.CellForeColor,
			GridColor = tableStyle.LineColor,
			HeaderColor = tableStyle.HeaderBackColor,
			HeaderForeground = tableStyle.HeaderForeColor,
			HotHeaderColor = tableStyle.HotHeaderBackColor,
			SelectedDataColor = tableStyle.SelectedCellBackColor,
			SelectedDataForeground = tableStyle.SelectedCellForeColor,
			FooterColor = tableStyle.FooterBackColor,
			FooterForeground = tableStyle.FooterForeColor
		};
	}

	public static StiGaugeStyle ConvertToReportGaugeStyle(IStiGaugeElement element)
	{
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Expected O, but got Unknown
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Expected O, but got Unknown
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected O, but got Unknown
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Expected O, but got Unknown
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Expected O, but got Unknown
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Expected O, but got Unknown
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Expected O, but got Unknown
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Expected O, but got Unknown
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Expected O, but got Unknown
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Expected O, but got Unknown
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Expected O, but got Unknown
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Expected O, but got Unknown
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Expected O, but got Unknown
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Expected O, but got Unknown
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Expected O, but got Unknown
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Expected O, but got Unknown
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Expected O, but got Unknown
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Expected O, but got Unknown
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Expected O, but got Unknown
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Expected O, but got Unknown
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Expected O, but got Unknown
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Expected O, but got Unknown
		StiGaugeStyleXF stiGaugeStyleXF = null;
		if (element.Style == StiElementStyleIdent.Custom && !string.IsNullOrEmpty(element.CustomStyleName))
		{
			if (element.Report.Styles.ToList().FirstOrDefault((StiBaseStyle x) => x.Name == element.CustomStyleName) is StiGaugeStyle style)
			{
				stiGaugeStyleXF = new StiCustomGaugeStyle(style);
			}
		}
		else
		{
			StiElementStyleIdent style2 = ((element.Style == StiElementStyleIdent.Auto) ? ((IStiDashboard)element.Page).Style : element.Style);
			stiGaugeStyleXF = GetGaugeStyle(style2);
		}
		if (stiGaugeStyleXF == null)
		{
			stiGaugeStyleXF = new StiGaugeStyleXF29();
		}
		StiGaugeStyleCoreXF core = stiGaugeStyleXF.Core;
		return new StiGaugeStyle
		{
			Brush = (StiBrush)core.Brush.Clone(),
			BorderColor = core.BorderColor,
			ForeColor = core.ForeColor,
			TargetColor = core.TargetColor,
			BorderWidth = core.BorderWidth,
			TickMarkMajorBrush = (StiBrush)new StiEmptyBrush(),
			TickMarkMajorBorder = (StiBrush)core.TickMarkMajorBorder.Clone(),
			TickMarkMinorBrush = (StiBrush)core.TickMarkMinorBrush.Clone(),
			TickMarkMinorBorder = (StiBrush)core.TickMarkMinorBorder.Clone(),
			TickLabelMajorTextBrush = (StiBrush)core.TickLabelMajorTextBrush.Clone(),
			TickLabelMajorFont = (Font)core.TickLabelMajorFont.Clone(),
			TickLabelMinorTextBrush = (StiBrush)core.TickLabelMinorTextBrush.Clone(),
			TickLabelMinorFont = (Font)core.TickLabelMinorFont.Clone(),
			MarkerBrush = (StiBrush)core.MarkerBrush.Clone(),
			LinearBarBrush = (StiBrush)core.LinearBarBrush.Clone(),
			LinearBarBorderBrush = (StiBrush)core.LinearBarBorderBrush.Clone(),
			LinearBarEmptyBrush = (StiBrush)core.LinearBarEmptyBrush.Clone(),
			LinearBarEmptyBorderBrush = (StiBrush)core.LinearBarEmptyBorderBrush.Clone(),
			RadialBarBrush = (StiBrush)core.RadialBarBrush.Clone(),
			RadialBarBorderBrush = (StiBrush)core.RadialBarBorderBrush.Clone(),
			RadialBarEmptyBrush = (StiBrush)core.RadialBarEmptyBrush.Clone(),
			RadialBarEmptyBorderBrush = (StiBrush)core.RadialBarEmptyBorderBrush.Clone(),
			NeedleBrush = (StiBrush)core.NeedleBrush.Clone(),
			NeedleBorderBrush = (StiBrush)core.NeedleBorderBrush.Clone(),
			NeedleCapBrush = (StiBrush)core.NeedleCapBrush.Clone(),
			NeedleCapBorderBrush = (StiBrush)core.NeedleCapBorderBrush.Clone()
		};
	}

	public static StiCrossTabStyle ConvertToReportPivotTableStyle(IStiPivotTableElement element)
	{
		StiPivotElementStyle pivotTableStyle = GetPivotTableStyle(element);
		return new StiCrossTabStyle
		{
			BackColor = pivotTableStyle.BackColor,
			AlternatingCellForeColor = pivotTableStyle.AlternatingCellForeColor,
			AlternatingCellBackColor = pivotTableStyle.AlternatingCellBackColor,
			CellBackColor = pivotTableStyle.CellBackColor,
			CellForeColor = pivotTableStyle.CellForeColor,
			LineColor = pivotTableStyle.LineColor,
			ColumnHeaderBackColor = pivotTableStyle.ColumnHeaderBackColor,
			ColumnHeaderForeColor = pivotTableStyle.ColumnHeaderForeColor,
			HotColumnHeaderBackColor = pivotTableStyle.HotColumnHeaderBackColor,
			HotRowHeaderBackColor = pivotTableStyle.HotRowHeaderBackColor,
			RowHeaderBackColor = pivotTableStyle.RowHeaderBackColor,
			RowHeaderForeColor = pivotTableStyle.RowHeaderForeColor,
			SelectedCellBackColor = pivotTableStyle.SelectedCellBackColor,
			SelectedCellForeColor = pivotTableStyle.SelectedCellForeColor
		};
	}

	public static StiCardsStyle ConvertToReportCardsStyle(IStiCardsElement element)
	{
		StiCardsElementStyle cardsStyle = GetCardsStyle(element);
		return new StiCardsStyle
		{
			BackColor = GetBackColor(element, null, allowOpacity: true),
			ForeColor = cardsStyle.CellForeColor,
			LineColor = cardsStyle.LineColor
		};
	}

	public static StiIndicatorStyle ConvertToReportIndicatorStyle(IStiIndicatorElement element)
	{
		StiIndicatorElementStyle indicatorStyle = GetIndicatorStyle(element);
		return new StiIndicatorStyle
		{
			BackColor = GetBackColor(element, null, allowOpacity: true),
			ForeColor = GetForeColor(element),
			HotBackColor = GetHotBackColor(element),
			GlyphColor = indicatorStyle.GlyphColor,
			PositiveColor = indicatorStyle.PositiveColor,
			NegativeColor = indicatorStyle.NegativeColor
		};
	}

	public static StiProgressStyle ConvertToReportProgressStyle(IStiProgressElement element)
	{
		StiProgressElementStyle progressStyle = GetProgressStyle(element);
		return new StiProgressStyle
		{
			BackColor = progressStyle.BackColor,
			TrackColor = progressStyle.TrackColor,
			BandColor = progressStyle.BandColor,
			ForeColor = progressStyle.ForeColor,
			SeriesColors = progressStyle.SeriesColors
		};
	}

	public static StiMapStyle ConvertToReportRegionMapStyle(IStiRegionMapElement element)
	{
		StiMapStyle stiMapStyle;
		if (element.Style == StiElementStyleIdent.Custom && !string.IsNullOrEmpty(element.CustomStyleName))
		{
			stiMapStyle = element.Report.Styles.ToList().FirstOrDefault((StiBaseStyle x) => x.Name == element.CustomStyleName) as StiMapStyle;
		}
		else
		{
			StiElementStyleIdent style = ((element.Style == StiElementStyleIdent.Auto) ? ((IStiDashboard)element.Page).Style : element.Style);
			stiMapStyle = GetMapStyle(style);
		}
		if (stiMapStyle == null)
		{
			stiMapStyle = new StiMap29StyleFX();
		}
		StiMapStyle stiMapStyle2 = new StiMapStyle
		{
			BackColor = stiMapStyle.BackColor,
			BorderColor = stiMapStyle.BorderColor,
			BorderSize = stiMapStyle.BorderSize,
			Colors = stiMapStyle.Colors,
			DefaultColor = stiMapStyle.DefaultColor,
			IndividualColor = stiMapStyle.IndividualColor,
			LabelForeground = stiMapStyle.LabelForeground,
			LabelShadowForeground = stiMapStyle.LabelShadowForeground,
			BubbleBackColor = stiMapStyle.BubbleBackColor,
			BubbleBorderColor = stiMapStyle.BubbleBorderColor
		};
		stiMapStyle2.Heatmap.Color = stiMapStyle.Heatmap.Color;
		stiMapStyle2.Heatmap.ZeroColor = stiMapStyle.Heatmap.ZeroColor;
		stiMapStyle2.Heatmap.Mode = stiMapStyle.Heatmap.Mode;
		stiMapStyle2.HeatmapWithGroup.Colors = stiMapStyle.HeatmapWithGroup.Colors;
		stiMapStyle2.HeatmapWithGroup.ZeroColor = stiMapStyle.HeatmapWithGroup.ZeroColor;
		stiMapStyle2.HeatmapWithGroup.Mode = stiMapStyle.HeatmapWithGroup.Mode;
		return stiMapStyle2;
	}

	public static StiDialogStyle ConvertToReportControlStyle(IStiControlElement element)
	{
		StiControlElementStyle controlStyle = GetControlStyle(element);
		return new StiDialogStyle
		{
			BackColor = controlStyle.BackColor,
			ForeColor = controlStyle.ForeColor,
			GlyphColor = controlStyle.GlyphColor,
			HotBackColor = controlStyle.HotBackColor,
			HotForeColor = controlStyle.HotForeColor,
			HotGlyphColor = controlStyle.HotGlyphColor,
			HotSelectedBackColor = controlStyle.HotSelectedBackColor,
			HotSelectedForeColor = controlStyle.HotSelectedForeColor,
			HotSelectedGlyphColor = controlStyle.HotSelectedGlyphColor,
			SelectedBackColor = controlStyle.SelectedBackColor,
			SelectedForeColor = controlStyle.SelectedForeColor,
			SelectedGlyphColor = controlStyle.SelectedGlyphColor,
			SeparatorColor = controlStyle.SeparatorColor
		};
	}

	public static Color GetDashboardBackColor(IStiSurface surface, bool isViewer)
	{
		if (surface == null)
		{
			return Color.White;
		}
		if (surface is IStiBackColor stiBackColor)
		{
			Color backColor = StiDashboardExpressionHelper.GetBackColor(surface, stiBackColor.BackColor);
			if (backColor != Color.Transparent)
			{
				return backColor;
			}
		}
		if (!(surface is IStiDashboardElementStyle stiDashboardElementStyle))
		{
			return Color.White;
		}
		switch (stiDashboardElementStyle.Style)
		{
		case StiElementStyleIdent.SlateGray:
		case StiElementStyleIdent.DarkBlue:
		case StiElementStyleIdent.DarkGray:
			return StiColor.Get("333333");
		case StiElementStyleIdent.DarkGreen:
			return StiColor.Get("144b45");
		case StiElementStyleIdent.DarkTurquoise:
			return StiColor.Get("1e4a61");
		case StiElementStyleIdent.Silver:
			return StiColor.Get("43545e");
		case StiElementStyleIdent.AliceBlue:
			return StiColor.Get("40508d");
		case StiElementStyleIdent.Sienna:
			return StiColor.Get("ede5d8");
		default:
			if (!isViewer)
			{
				return Color.White;
			}
			return StiColor.Get("#f3f3f3");
		}
	}

	public static bool IsDarkStyle(IStiDashboard dashboard)
	{
		if (dashboard == null)
		{
			return false;
		}
		return IsDarkStyle(dashboard.Style);
	}

	public static bool IsDarkStyle(IStiElement element)
	{
		StiElementStyleIdent style = GetStyle(element);
		if (style == StiElementStyleIdent.Custom || style == StiElementStyleIdent.Auto)
		{
			style = GetStyle(element?.Page as IStiDashboard);
		}
		if (style == StiElementStyleIdent.AliceBlue && !(element is IStiDashboard))
		{
			return false;
		}
		return IsDarkStyle(style);
	}

	public static bool IsDarkStyle(StiElementStyleIdent ident)
	{
		if ((uint)(ident - 6) <= 6u)
		{
			return true;
		}
		return false;
	}

	public static Font GetFont(object element)
	{
		StiElementStyleIdent style = GetStyle(element);
		if (style == StiElementStyleIdent.Custom)
		{
			StiReport report = (element as StiComponent).Report;
			if (element is IStiDashboardElementStyle stiDashboardElementStyle && report.Styles[stiDashboardElementStyle.CustomStyleName] is StiDialogStyle { AllowUseFont: not false, Font: not null } stiDialogStyle)
			{
				return stiDialogStyle.Font;
			}
		}
		return (element as IStiFont).Font;
	}

	public static Color GetDataEmptyColor(IStiElement element)
	{
		if (!IsDarkStyle(element))
		{
			return Color.FromArgb(255, 240, 240, 240);
		}
		return Color.FromArgb(255, 196, 196, 196);
	}

	public static Color GetDataEmptyForeColor(IStiElement element)
	{
		if (!IsDarkStyle(element))
		{
			return Color.DimGray;
		}
		return Color.FromArgb(255, 196, 196, 196);
	}

	public static Color GetForeColor(object element, Color? defaultColor = null)
	{
		if (element is IStiForeColor stiForeColor)
		{
			Color foreColor = StiDashboardExpressionHelper.GetForeColor(element, stiForeColor.ForeColor);
			if (foreColor != Color.Transparent)
			{
				return foreColor;
			}
		}
		Color styleForeColor = GetStyleForeColor(element);
		if (styleForeColor != Color.Transparent)
		{
			return styleForeColor;
		}
		if (element is IStiControlElement element2 && GetControlStyle(element2).ForeColor != Color.Transparent)
		{
			return GetControlStyle(element2).ForeColor;
		}
		if (defaultColor.HasValue)
		{
			return defaultColor.Value;
		}
		return GetNativeForeColor(element);
	}

	private static Color GetStyleForeColor(object element)
	{
		StiElementStyleIdent style = GetStyle(element);
		if (style != StiElementStyleIdent.Custom)
		{
			return Color.Transparent;
		}
		string text = (element as IStiDashboardElementStyle)?.CustomStyleName;
		StiReport report = (element as StiComponent).Report;
		if (element is IStiIndicatorElement)
		{
			if (GetIndicatorStyle(element as IStiIndicatorElement) is StiCustomIndicatorElementStyle stiCustomIndicatorElementStyle)
			{
				return stiCustomIndicatorElementStyle.ForeColor;
			}
			return Color.Transparent;
		}
		if (element is IStiProgressElement)
		{
			if (GetProgressStyle(element as IStiProgressElement) is StiCustomProgressElementStyle stiCustomProgressElementStyle)
			{
				return stiCustomProgressElementStyle.ForeColor;
			}
			return Color.Transparent;
		}
		if (element is IStiGaugeElement)
		{
			StiCustomGaugeStyle customGaugeStyle = report.Styles.GetCustomGaugeStyle(text);
			if (customGaugeStyle != null && (customGaugeStyle.Core?.ForeColor).HasValue)
			{
				return customGaugeStyle.Core.ForeColor;
			}
			return Color.Transparent;
		}
		if (element is IStiRegionMapElement)
		{
			if (report.Styles[text] is StiMapStyle stiMapStyle)
			{
				return stiMapStyle.LabelForeground;
			}
			return Color.Transparent;
		}
		if (element is IStiControlElement)
		{
			if (report.Styles[text] is StiDialogStyle stiDialogStyle)
			{
				return stiDialogStyle.ForeColor;
			}
			return Color.Transparent;
		}
		return Color.Transparent;
	}

	public static Color GetForeColor(StiElementStyleIdent ident)
	{
		switch (ident)
		{
		case StiElementStyleIdent.Silver:
			return StiColor.Get("e9f4fc");
		case StiElementStyleIdent.AliceBlue:
			return StiColor.Get("2e2e2e");
		case StiElementStyleIdent.Sienna:
			return StiColor.Get("4c453d");
		default:
			if (!IsDarkStyle(ident))
			{
				return StiElementConsts.ForegroundColor;
			}
			return StiColor.Get("dddddd");
		}
	}

	public static Color GetNativeForeColor(object element = null)
	{
		return GetForeColor(GetStyle(element));
	}

	public static Color GetSelectedForeColor(object element)
	{
		StiElementStyleIdent style = GetStyle(element);
		if (style == StiElementStyleIdent.Custom)
		{
			StiReport report = (element as StiComponent).Report;
			if (element is IStiDashboardElementStyle stiDashboardElementStyle && report.Styles[stiDashboardElementStyle.CustomStyleName] is StiDialogStyle stiDialogStyle && stiDialogStyle.SelectedForeColor != Color.Transparent)
			{
				return stiDialogStyle.SelectedForeColor;
			}
		}
		return GetControlStyle(element).ForeColor;
	}

	public static Color GetSelectedBackColor(object element)
	{
		StiElementStyleIdent style = GetStyle(element);
		if (style == StiElementStyleIdent.Custom)
		{
			StiReport report = (element as StiComponent).Report;
			if (element is IStiDashboardElementStyle stiDashboardElementStyle && report.Styles[stiDashboardElementStyle.CustomStyleName] is StiDialogStyle stiDialogStyle && stiDialogStyle.SelectedBackColor != Color.Transparent)
			{
				return stiDialogStyle.SelectedBackColor;
			}
		}
		return GetControlStyle(element).BackColor;
	}

	public static Color GetGlyphColor(object element)
	{
		StiElementStyleIdent style = GetStyle(element);
		if (style == StiElementStyleIdent.Custom)
		{
			StiReport report = (element as StiComponent).Report;
			if (element is IStiDashboardElementStyle stiDashboardElementStyle && report.Styles[stiDashboardElementStyle.CustomStyleName] is StiDialogStyle stiDialogStyle && stiDialogStyle.GlyphColor != Color.Transparent)
			{
				return stiDialogStyle.GlyphColor;
			}
		}
		return GetControlStyle(element).GlyphColor;
	}

	public static Color GetGlyphColor(IStiIndicatorElement element)
	{
		if (element.GlyphColor != Color.Transparent)
		{
			return element.GlyphColor;
		}
		StiElementStyleIdent style = GetStyle(element);
		if (style == StiElementStyleIdent.Custom && element.Report.Styles[element.CustomStyleName] is StiIndicatorStyle stiIndicatorStyle && stiIndicatorStyle.GlyphColor != Color.Transparent)
		{
			return stiIndicatorStyle.GlyphColor;
		}
		StiIndicatorElementStyle indicatorStyle = GetIndicatorStyle(element);
		return indicatorStyle.GlyphColor;
	}

	public static Color GetSeparatorColor(object element)
	{
		StiElementStyleIdent style = GetStyle(element);
		if (style == StiElementStyleIdent.Custom)
		{
			StiReport report = (element as StiComponent).Report;
			if (element is IStiDashboardElementStyle stiDashboardElementStyle && report.Styles[stiDashboardElementStyle.CustomStyleName] is StiDialogStyle stiDialogStyle && stiDialogStyle.SeparatorColor != Color.Transparent)
			{
				return stiDialogStyle.SeparatorColor;
			}
		}
		return GetControlStyle(element).SeparatorColor;
	}

	public static Color GetBackColor(object element, Color? defaultColor = null, bool allowOpacity = false)
	{
		if (element is IStiBackColor stiBackColor)
		{
			Color backColor = StiDashboardExpressionHelper.GetBackColor(element, stiBackColor.BackColor);
			if (Color.Transparent.ToArgb() != backColor.ToArgb())
			{
				if (backColor.A == byte.MaxValue)
				{
					return backColor;
				}
				if (!allowOpacity)
				{
					return Color.FromArgb(255, backColor);
				}
				return backColor;
			}
		}
		Color styleBackColor = GetStyleBackColor(element);
		if (allowOpacity && GetStyle(element) == StiElementStyleIdent.Custom)
		{
			return styleBackColor;
		}
		if (styleBackColor != Color.Transparent)
		{
			return styleBackColor;
		}
		if (element is IStiControlElement element2 && GetControlStyle(element2).BackColor != Color.Transparent)
		{
			return GetControlStyle(element2).BackColor;
		}
		if (defaultColor.HasValue)
		{
			return defaultColor.Value;
		}
		return GetBackColor(GetStyle(element));
	}

	private static Color GetStyleBackColor(object element)
	{
		StiElementStyleIdent style = GetStyle(element);
		if (style != StiElementStyleIdent.Custom)
		{
			return Color.Transparent;
		}
		string text = (element as IStiDashboardElementStyle)?.CustomStyleName;
		StiReport report = (element as StiComponent).Report;
		if (element is IStiIndicatorElement)
		{
			if (GetIndicatorStyle(element as IStiIndicatorElement) is StiCustomIndicatorElementStyle stiCustomIndicatorElementStyle)
			{
				return stiCustomIndicatorElementStyle.BackColor;
			}
			return Color.Transparent;
		}
		if (element is IStiProgressElement)
		{
			if (GetProgressStyle(element as IStiProgressElement) is StiCustomProgressElementStyle stiCustomProgressElementStyle)
			{
				return stiCustomProgressElementStyle.BackColor;
			}
			return Color.Transparent;
		}
		if (element is IStiGaugeElement)
		{
			StiCustomGaugeStyle customGaugeStyle = report.Styles.GetCustomGaugeStyle(text);
			if (customGaugeStyle?.Core?.Brush != null)
			{
				return StiBrush.ToColor(customGaugeStyle.Core.Brush);
			}
			return Color.Transparent;
		}
		if (element is IStiChartElement)
		{
			StiChartStyle stiChartStyle = report.Styles[text] as StiChartStyle;
			if (stiChartStyle?.Brush != null)
			{
				return StiBrush.ToColor(stiChartStyle.Brush);
			}
			return Color.Transparent;
		}
		if (element is IStiRegionMapElement)
		{
			if (report.Styles[text] is StiMapStyle stiMapStyle)
			{
				return stiMapStyle.BackColor;
			}
			return Color.Transparent;
		}
		if (element is IStiTableElement)
		{
			if (report.Styles[text] is StiTableStyle stiTableStyle)
			{
				return stiTableStyle.BackColor;
			}
			return Color.Transparent;
		}
		if (element is IStiPivotTableElement)
		{
			if (report.Styles[text] is StiCrossTabStyle stiCrossTabStyle)
			{
				return stiCrossTabStyle.BackColor;
			}
			return Color.Transparent;
		}
		if (element is IStiControlElement || element is IStiComponentUI)
		{
			if (report.Styles[text] is StiDialogStyle stiDialogStyle)
			{
				return stiDialogStyle.BackColor;
			}
			return Color.Transparent;
		}
		return Color.Transparent;
	}

	private static Color GetStyleHotBackColor(IStiElement element)
	{
		StiElementStyleIdent style = GetStyle(element);
		if (style != StiElementStyleIdent.Custom)
		{
			return Color.Transparent;
		}
		string name = (element as IStiDashboardElementStyle)?.CustomStyleName;
		if (element is IStiIndicatorElement)
		{
			if (GetIndicatorStyle(element as IStiIndicatorElement) is StiCustomIndicatorElementStyle stiCustomIndicatorElementStyle)
			{
				return stiCustomIndicatorElementStyle.HotBackColor;
			}
			return Color.Transparent;
		}
		if (element is IStiControlElement)
		{
			if (element.Report.Styles[name] is StiDialogStyle stiDialogStyle)
			{
				return stiDialogStyle.HotBackColor;
			}
			return Color.Transparent;
		}
		return Color.Transparent;
	}

	public static Color GetHotBackColor(IStiElement element)
	{
		StiElementStyleIdent style = GetStyle(element);
		if (style == StiElementStyleIdent.Custom)
		{
			return GetStyleHotBackColor(element);
		}
		if (IsDarkStyle(style))
		{
			return StiColorUtils.Light(GetBackColor(element), (byte)15);
		}
		return StiColorUtils.Dark(GetBackColor(element), (byte)15);
	}

	public static Color GetBackColor(StiElementStyleIdent style)
	{
		return style switch
		{
			StiElementStyleIdent.DarkBlue => StiColor.Get("#0a325a"), 
			StiElementStyleIdent.SlateGray => StiColor.Get("#33475b"), 
			StiElementStyleIdent.DarkGray => StiColor.Get("#595b65"), 
			StiElementStyleIdent.DarkGreen => StiColor.Get("#3f745e"), 
			StiElementStyleIdent.DarkTurquoise => StiColor.Get("#235e6d"), 
			StiElementStyleIdent.Silver => StiColor.Get("#6d7e8b"), 
			StiElementStyleIdent.AliceBlue => StiColor.Get("#f2f5fc"), 
			StiElementStyleIdent.Sienna => StiColor.Get("#fefefe"), 
			_ => StiElementConsts.BackgroundColor, 
		};
	}

	public static Color GetTitleForeColor(IStiElement element)
	{
		StiElementStyleIdent style = GetStyle(element);
		switch (style)
		{
		case StiElementStyleIdent.Silver:
			return StiColor.Get("e9f4fc");
		case StiElementStyleIdent.AliceBlue:
			return StiColor.Get("1f377f");
		default:
			if (!IsDarkStyle(style))
			{
				return StiElementConsts.TitleFont.Color;
			}
			return StiColor.Get("dddddd");
		}
	}

	public static StiGaugeStyleXF GetGaugeStyle(IStiGaugeElement element)
	{
		return GetGaugeStyle(GetStyle(element));
	}

	public static StiGaugeStyleXF GetGaugeStyle(StiElementStyleIdent style)
	{
		return style switch
		{
			StiElementStyleIdent.Orange => new StiGaugeStyleXF24(), 
			StiElementStyleIdent.Green => new StiGaugeStyleXF25(), 
			StiElementStyleIdent.Turquoise => new StiGaugeStyleXF26(), 
			StiElementStyleIdent.SlateGray => new StiGaugeStyleXF27(), 
			StiElementStyleIdent.DarkBlue => new StiGaugeStyleXF28(), 
			StiElementStyleIdent.Blue => new StiGaugeStyleXF29(), 
			StiElementStyleIdent.DarkGray => new StiGaugeStyleXF30(), 
			StiElementStyleIdent.DarkTurquoise => new StiGaugeStyleXF31(), 
			StiElementStyleIdent.Silver => new StiGaugeStyleXF32(), 
			StiElementStyleIdent.AliceBlue => new StiGaugeStyleXF33(), 
			StiElementStyleIdent.DarkGreen => new StiGaugeStyleXF34(), 
			StiElementStyleIdent.Sienna => new StiGaugeStyleXF35(), 
			_ => new StiGaugeStyleXF29(), 
		};
	}

	public static IStiChartStyle GetChartStyle(IStiChartElement element)
	{
		return GetChartStyle(GetStyle(element));
	}

	public static IStiChartStyle GetChartStyle(StiElementStyleIdent style)
	{
		switch (style)
		{
		case StiElementStyleIdent.Orange:
		{
			StiStyle24 stiStyle12 = new StiStyle24();
			stiStyle12.Core.MarkerVisible = false;
			return stiStyle12;
		}
		case StiElementStyleIdent.Green:
		{
			StiStyle25 stiStyle11 = new StiStyle25();
			stiStyle11.Core.MarkerVisible = false;
			return stiStyle11;
		}
		case StiElementStyleIdent.Turquoise:
		{
			StiStyle26 stiStyle10 = new StiStyle26();
			stiStyle10.Core.MarkerVisible = false;
			return stiStyle10;
		}
		case StiElementStyleIdent.SlateGray:
		{
			StiStyle27 stiStyle9 = new StiStyle27();
			stiStyle9.Core.MarkerVisible = false;
			return stiStyle9;
		}
		case StiElementStyleIdent.DarkBlue:
		{
			StiStyle28 stiStyle8 = new StiStyle28();
			stiStyle8.Core.MarkerVisible = false;
			return stiStyle8;
		}
		case StiElementStyleIdent.Blue:
		{
			StiStyle29 stiStyle7 = new StiStyle29();
			stiStyle7.Core.MarkerVisible = false;
			return stiStyle7;
		}
		case StiElementStyleIdent.DarkGray:
		{
			StiStyle30 stiStyle6 = new StiStyle30();
			stiStyle6.Core.MarkerVisible = false;
			return stiStyle6;
		}
		case StiElementStyleIdent.DarkTurquoise:
		{
			StiStyle31 stiStyle5 = new StiStyle31();
			stiStyle5.Core.MarkerVisible = false;
			return stiStyle5;
		}
		case StiElementStyleIdent.Silver:
		{
			StiStyle32 stiStyle4 = new StiStyle32();
			stiStyle4.Core.MarkerVisible = false;
			return stiStyle4;
		}
		case StiElementStyleIdent.AliceBlue:
		{
			StiStyle33 stiStyle3 = new StiStyle33();
			stiStyle3.Core.MarkerVisible = false;
			return stiStyle3;
		}
		case StiElementStyleIdent.DarkGreen:
		{
			StiStyle34 stiStyle2 = new StiStyle34();
			stiStyle2.Core.MarkerVisible = false;
			return stiStyle2;
		}
		case StiElementStyleIdent.Sienna:
		{
			StiStyle35 stiStyle = new StiStyle35();
			stiStyle.Core.MarkerVisible = false;
			return stiStyle;
		}
		default:
			return new StiStyle29();
		}
	}

	public static StiMapStyleIdent GetMapStyleIdent(IStiRegionMapElement element)
	{
		return GetStyle(element) switch
		{
			StiElementStyleIdent.Orange => StiMapStyleIdent.Style24, 
			StiElementStyleIdent.Green => StiMapStyleIdent.Style25, 
			StiElementStyleIdent.Turquoise => StiMapStyleIdent.Style26, 
			StiElementStyleIdent.SlateGray => StiMapStyleIdent.Style27, 
			StiElementStyleIdent.DarkBlue => StiMapStyleIdent.Style28, 
			StiElementStyleIdent.Blue => StiMapStyleIdent.Style29, 
			StiElementStyleIdent.DarkGray => StiMapStyleIdent.Style30, 
			StiElementStyleIdent.DarkTurquoise => StiMapStyleIdent.Style31, 
			StiElementStyleIdent.Silver => StiMapStyleIdent.Style32, 
			StiElementStyleIdent.AliceBlue => StiMapStyleIdent.Style33, 
			StiElementStyleIdent.DarkGreen => StiMapStyleIdent.Style34, 
			StiElementStyleIdent.Sienna => StiMapStyleIdent.Style35, 
			_ => StiMapStyleIdent.Style29, 
		};
	}

	public static StiMapStyleFX GetMapStyle(IStiRegionMapElement element)
	{
		return GetMapStyle(GetStyle(element));
	}

	public static StiMapStyleFX GetMapStyle(StiElementStyleIdent style)
	{
		return style switch
		{
			StiElementStyleIdent.Orange => new StiMap24StyleFX(), 
			StiElementStyleIdent.Green => new StiMap25StyleFX(), 
			StiElementStyleIdent.Turquoise => new StiMap26StyleFX(), 
			StiElementStyleIdent.SlateGray => new StiMap27StyleFX(), 
			StiElementStyleIdent.DarkBlue => new StiMap28StyleFX(), 
			StiElementStyleIdent.Blue => new StiMap29StyleFX(), 
			StiElementStyleIdent.DarkGray => new StiMap30StyleFX(), 
			StiElementStyleIdent.DarkTurquoise => new StiMap31StyleFX(), 
			StiElementStyleIdent.Silver => new StiMap32StyleFX(), 
			StiElementStyleIdent.AliceBlue => new StiMap33StyleFX(), 
			StiElementStyleIdent.DarkGreen => new StiMap34StyleFX(), 
			StiElementStyleIdent.Sienna => new StiMap35StyleFX(), 
			_ => new StiMap29StyleFX(), 
		};
	}

	public static StiControlElementStyle GetControlStyle(object element)
	{
		switch (GetStyle(element))
		{
		case StiElementStyleIdent.Orange:
			return new StiOrangeControlElementStyle();
		case StiElementStyleIdent.Green:
			return new StiGreenControlElementStyle();
		case StiElementStyleIdent.Turquoise:
			return new StiTurquoiseControlElementStyle();
		case StiElementStyleIdent.SlateGray:
			return new StiSlateGrayControlElementStyle();
		case StiElementStyleIdent.DarkBlue:
			return new StiDarkBlueControlElementStyle();
		case StiElementStyleIdent.Blue:
			return new StiBlueControlElementStyle();
		case StiElementStyleIdent.DarkGray:
			return new StiDarkGrayControlElementStyle();
		case StiElementStyleIdent.DarkTurquoise:
			return new StiDarkTurquoiseControlElementStyle();
		case StiElementStyleIdent.Silver:
			return new StiSilverControlElementStyle();
		case StiElementStyleIdent.AliceBlue:
			return new StiAliceBlueControlElementStyle();
		case StiElementStyleIdent.DarkGreen:
			return new StiDarkGreenControlElementStyle();
		case StiElementStyleIdent.Sienna:
			return new StiSiennaControlElementStyle();
		case StiElementStyleIdent.Custom:
			if (element is IStiDashboardElementStyle stiDashboardElementStyle && !string.IsNullOrWhiteSpace(stiDashboardElementStyle.CustomStyleName))
			{
				StiReport report = (element as StiComponent).Report;
				string styleName = stiDashboardElementStyle.CustomStyleName.ToLowerInvariant();
				StiDialogStyle stiDialogStyle = (from s in report.Styles.ToList()
					where s is StiDialogStyle && s.Name != null
					select s).Cast<StiDialogStyle>().FirstOrDefault((StiDialogStyle s) => s.Name.ToLowerInvariant() == styleName);
				if (stiDialogStyle != null)
				{
					return new StiCustomControlElementStyle(stiDialogStyle);
				}
			}
			break;
		}
		return new StiDarkBlueControlElementStyle();
	}

	public static StiIndicatorElementStyle GetIndicatorStyle(IStiIndicatorElement element)
	{
		switch (GetStyle(element))
		{
		case StiElementStyleIdent.Orange:
			return new StiOrangeIndicatorElementStyle();
		case StiElementStyleIdent.Green:
			return new StiGreenIndicatorElementStyle();
		case StiElementStyleIdent.Turquoise:
			return new StiTurquoiseIndicatorElementStyle();
		case StiElementStyleIdent.SlateGray:
			return new StiSlateGrayIndicatorElementStyle();
		case StiElementStyleIdent.DarkBlue:
			return new StiDarkBlueIndicatorElementStyle();
		case StiElementStyleIdent.Blue:
			return new StiBlueIndicatorElementStyle();
		case StiElementStyleIdent.DarkGray:
			return new StiDarkGrayIndicatorElementStyle();
		case StiElementStyleIdent.DarkTurquoise:
			return new StiDarkTurquoiseIndicatorElementStyle();
		case StiElementStyleIdent.Silver:
			return new StiSilverIndicatorElementStyle();
		case StiElementStyleIdent.AliceBlue:
			return new StiAliceBlueIndicatorElementStyle();
		case StiElementStyleIdent.DarkGreen:
			return new StiDarkGreenIndicatorElementStyle();
		case StiElementStyleIdent.Sienna:
			return new StiSiennaIndicatorElementStyle();
		case StiElementStyleIdent.Custom:
			if (!string.IsNullOrWhiteSpace(element.CustomStyleName))
			{
				string styleName = element.CustomStyleName.ToLowerInvariant();
				StiIndicatorStyle stiIndicatorStyle = (from s in element.Report.Styles.ToList()
					where s is StiIndicatorStyle && s.Name != null
					select s).Cast<StiIndicatorStyle>().FirstOrDefault((StiIndicatorStyle s) => s.Name.ToLowerInvariant() == styleName);
				if (stiIndicatorStyle != null)
				{
					return new StiCustomIndicatorElementStyle(stiIndicatorStyle);
				}
			}
			break;
		}
		return new StiOrangeIndicatorElementStyle();
	}

	public static StiProgressElementStyle GetProgressStyle(IStiProgressElement element)
	{
		switch (GetStyle(element))
		{
		case StiElementStyleIdent.Blue:
			return new StiBlueProgressElementStyle();
		case StiElementStyleIdent.Green:
			return new StiGreenProgressElementStyle();
		case StiElementStyleIdent.Turquoise:
			return new StiTurquoiseProgressElementStyle();
		case StiElementStyleIdent.SlateGray:
			return new StiSlateGrayProgressElementStyle();
		case StiElementStyleIdent.DarkBlue:
			return new StiDarkBlueProgressElementStyle();
		case StiElementStyleIdent.DarkGray:
			return new StiDarkGrayProgressElementStyle();
		case StiElementStyleIdent.DarkTurquoise:
			return new StiDarkTurquoiseProgressElementStyle();
		case StiElementStyleIdent.Silver:
			return new StiSilverProgressElementStyle();
		case StiElementStyleIdent.AliceBlue:
			return new StiAliceBlueProgressElementStyle();
		case StiElementStyleIdent.DarkGreen:
			return new StiDarkGreenProgressElementStyle();
		case StiElementStyleIdent.Sienna:
			return new StiSiennaProgressElementStyle();
		case StiElementStyleIdent.Custom:
			if (!string.IsNullOrWhiteSpace(element.CustomStyleName))
			{
				string styleName = element.CustomStyleName.ToLowerInvariant();
				StiProgressStyle stiProgressStyle = (from s in element.Report.Styles.ToList()
					where s is StiProgressStyle && s.Name != null
					select s).Cast<StiProgressStyle>().FirstOrDefault((StiProgressStyle s) => s.Name.ToLowerInvariant() == styleName);
				if (stiProgressStyle != null)
				{
					return new StiCustomProgressElementStyle(stiProgressStyle);
				}
			}
			break;
		}
		return new StiOrangeProgressElementStyle();
	}

	public static StiTableElementStyle GetTableStyle(IStiTableElement element)
	{
		StiElementStyleIdent style = GetStyle(element);
		if (style == StiElementStyleIdent.Custom)
		{
			if (!string.IsNullOrWhiteSpace(element.CustomStyleName))
			{
				string styleName = element.CustomStyleName.ToLowerInvariant();
				StiTableStyle stiTableStyle = (from s in element.Report.Styles.ToList()
					where s is StiTableStyle && s.Name != null
					select s).Cast<StiTableStyle>().FirstOrDefault((StiTableStyle s) => s.Name.ToLowerInvariant() == styleName);
				if (stiTableStyle != null)
				{
					return new StiCustomTableElementStyle(stiTableStyle);
				}
			}
			return new StiBlueTableElementStyle();
		}
		return GetTableStyle(style);
	}

	public static StiTableElementStyle GetTableStyle(StiElementStyleIdent style)
	{
		return style switch
		{
			StiElementStyleIdent.Orange => new StiOrangeTableElementStyle(), 
			StiElementStyleIdent.Green => new StiGreenTableElementStyle(), 
			StiElementStyleIdent.Turquoise => new StiTurquoiseTableElementStyle(), 
			StiElementStyleIdent.SlateGray => new StiSlateGrayTableElementStyle(), 
			StiElementStyleIdent.DarkBlue => new StiDarkBlueTableElementStyle(), 
			StiElementStyleIdent.Blue => new StiBlueTableElementStyle(), 
			StiElementStyleIdent.DarkGray => new StiDarkGrayTableElementStyle(), 
			StiElementStyleIdent.DarkTurquoise => new StiDarkTurquoiseTableElementStyle(), 
			StiElementStyleIdent.Silver => new StiSilverTableElementStyle(), 
			StiElementStyleIdent.AliceBlue => new StiAliceBlueTableElementStyle(), 
			StiElementStyleIdent.DarkGreen => new StiDarkGreenTableElementStyle(), 
			StiElementStyleIdent.Sienna => new StiSiennaTableElementStyle(), 
			_ => new StiBlueTableElementStyle(), 
		};
	}

	public static StiCardsElementStyle GetCardsStyle(IStiCardsElement element)
	{
		StiElementStyleIdent style = GetStyle(element);
		if (style == StiElementStyleIdent.Custom)
		{
			if (!string.IsNullOrWhiteSpace(element.CustomStyleName))
			{
				string styleName = element.CustomStyleName.ToLowerInvariant();
				StiCardsStyle stiCardsStyle = (from s in element.Report.Styles.ToList()
					where s is StiCardsStyle && s.Name != null
					select s).Cast<StiCardsStyle>().FirstOrDefault((StiCardsStyle s) => s.Name.ToLowerInvariant() == styleName);
				if (stiCardsStyle != null)
				{
					return new StiCustomCardsElementStyle(stiCardsStyle);
				}
			}
			return new StiBlueCardsElementStyle();
		}
		return GetCardsStyle(style);
	}

	public static StiCardsElementStyle GetCardsStyle(StiElementStyleIdent style)
	{
		return style switch
		{
			StiElementStyleIdent.Orange => new StiOrangeCardsElementStyle(), 
			StiElementStyleIdent.Green => new StiGreenCardsElementStyle(), 
			StiElementStyleIdent.Turquoise => new StiTurquoiseCardsElementStyle(), 
			StiElementStyleIdent.SlateGray => new StiSlateGrayCardsElementStyle(), 
			StiElementStyleIdent.DarkBlue => new StiDarkBlueCardsElementStyle(), 
			StiElementStyleIdent.Blue => new StiBlueCardsElementStyle(), 
			StiElementStyleIdent.DarkGray => new StiDarkGrayCardsElementStyle(), 
			StiElementStyleIdent.DarkTurquoise => new StiDarkTurquoiseCardsElementStyle(), 
			StiElementStyleIdent.Silver => new StiSilverCardsElementStyle(), 
			StiElementStyleIdent.AliceBlue => new StiAliceBlueCardsElementStyle(), 
			StiElementStyleIdent.DarkGreen => new StiDarkGreenCardsElementStyle(), 
			StiElementStyleIdent.Sienna => new StiSiennaCardsElementStyle(), 
			_ => new StiBlueCardsElementStyle(), 
		};
	}

	public static StiPivotElementStyle GetPivotTableStyle(IStiPivotTableElement element)
	{
		switch (GetStyle(element))
		{
		case StiElementStyleIdent.Orange:
			return new StiOrangePivotElementStyle();
		case StiElementStyleIdent.Green:
			return new StiGreenPivotElementStyle();
		case StiElementStyleIdent.Turquoise:
			return new StiTurquoisePivotElementStyle();
		case StiElementStyleIdent.SlateGray:
			return new StiSlateGrayPivotElementStyle();
		case StiElementStyleIdent.DarkBlue:
			return new StiDarkBluePivotElementStyle();
		case StiElementStyleIdent.Blue:
			return new StiBluePivotElementStyle();
		case StiElementStyleIdent.DarkGray:
			return new StiDarkGrayPivotElementStyle();
		case StiElementStyleIdent.DarkTurquoise:
			return new StiDarkTurquoisePivotElementStyle();
		case StiElementStyleIdent.Silver:
			return new StiSilverPivotElementStyle();
		case StiElementStyleIdent.AliceBlue:
			return new StiAliceBluePivotElementStyle();
		case StiElementStyleIdent.DarkGreen:
			return new StiDarkGreenPivotElementStyle();
		case StiElementStyleIdent.Sienna:
			return new StiSiennaPivotElementStyle();
		case StiElementStyleIdent.Custom:
			if (!string.IsNullOrWhiteSpace(element.CustomStyleName))
			{
				string styleName = element.CustomStyleName.ToLowerInvariant();
				StiCrossTabStyle stiCrossTabStyle = (from s in element.Report.Styles.ToList()
					where s is StiCrossTabStyle && s.Name != null
					select s).Cast<StiCrossTabStyle>().FirstOrDefault((StiCrossTabStyle s) => s.Name.ToLowerInvariant() == styleName);
				if (stiCrossTabStyle != null)
				{
					return new StiCustomPivotElementStyle(stiCrossTabStyle);
				}
			}
			break;
		}
		return new StiOrangePivotElementStyle();
	}

	public static StiElementStyleIdent GetStyle(object element)
	{
		IStiDashboardElementStyle stiDashboardElementStyle = element as IStiDashboardElementStyle;
		IStiDashboardElementStyle stiDashboardElementStyle2 = (element as StiComponent)?.Page as IStiDashboardElementStyle;
		if (stiDashboardElementStyle == null && stiDashboardElementStyle2 == null)
		{
			return StiElementStyleIdent.Blue;
		}
		if (stiDashboardElementStyle == null)
		{
			return stiDashboardElementStyle2.Style;
		}
		if (stiDashboardElementStyle2 == null)
		{
			return stiDashboardElementStyle.Style;
		}
		if (stiDashboardElementStyle.Style != StiElementStyleIdent.Auto)
		{
			return stiDashboardElementStyle.Style;
		}
		return stiDashboardElementStyle2.Style;
	}

	internal static FontFamily GetIconFontFamily()
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		if (iconFontFamily == null)
		{
			Type type = Type.GetType("Stimulsoft.Dashboard.Helpers.StiFontIconsHelper, Stimulsoft.Dashboard");
			if (type != null)
			{
				MethodInfo method = type.GetMethod("GetFontFamaliIcons", BindingFlags.Static | BindingFlags.NonPublic);
				if (method != null)
				{
					object? obj = method.Invoke(null, null);
					iconFontFamily = (FontFamily)((obj is FontFamily) ? obj : null);
				}
			}
		}
		if (iconFontFamily == null)
		{
			iconFontFamily = new FontFamily("Arial");
		}
		return iconFontFamily;
	}
}
