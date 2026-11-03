using System;
using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Components.Table;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Maps;

namespace Stimulsoft.Report.Styles.Helpers;

public static class StiStyleBasedHelper
{
	public class StyleInfo
	{
		public StiStyleType StyleType { get; }

		public List<StiBaseStyle> Styles { get; }

		public StyleInfo(StiStyleType styleType, List<StiBaseStyle> styles)
		{
			StyleType = styleType;
			Styles = styles;
		}

		public string GetDefaultStyle()
		{
			return StyleType switch
			{
				StiStyleType.Cards => DefaultCardsStyle, 
				StiStyleType.Chart => DefaultChartStyle, 
				StiStyleType.CrossTab => DefaultCrossTabStyle, 
				StiStyleType.Table => DefaultTableStyle, 
				StiStyleType.Gauge => DefaultGaugeStyle, 
				StiStyleType.Map => DefaultMapStyle, 
				StiStyleType.Indicator => DefaultIndicatorStyle, 
				StiStyleType.Progress => DefaultProgressStyle, 
				_ => throw new NotSupportedException(), 
			};
		}

		public void SetDefaultStyle(string value)
		{
			switch (StyleType)
			{
			case StiStyleType.Cards:
				DefaultCardsStyle = value;
				break;
			case StiStyleType.Chart:
				DefaultChartStyle = value;
				break;
			case StiStyleType.CrossTab:
				DefaultCrossTabStyle = value;
				break;
			case StiStyleType.Table:
				DefaultTableStyle = value;
				break;
			case StiStyleType.Gauge:
				DefaultGaugeStyle = value;
				break;
			case StiStyleType.Map:
				DefaultMapStyle = value;
				break;
			case StiStyleType.Indicator:
				DefaultIndicatorStyle = value;
				break;
			case StiStyleType.Progress:
				DefaultProgressStyle = value;
				break;
			default:
				throw new NotSupportedException();
			}
		}

		public StiBaseStyle GenerateStyle(StiBaseStyle baseStyle)
		{
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Expected O, but got Unknown
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Expected O, but got Unknown
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Expected O, but got Unknown
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Expected O, but got Unknown
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Expected O, but got Unknown
			//IL_018a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Expected O, but got Unknown
			//IL_0315: Unknown result type (might be due to invalid IL or missing references)
			//IL_031f: Expected O, but got Unknown
			//IL_032c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0336: Expected O, but got Unknown
			//IL_0350: Unknown result type (might be due to invalid IL or missing references)
			//IL_035a: Expected O, but got Unknown
			//IL_0367: Unknown result type (might be due to invalid IL or missing references)
			//IL_0371: Expected O, but got Unknown
			//IL_038b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0395: Expected O, but got Unknown
			//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ac: Expected O, but got Unknown
			//IL_03b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c3: Expected O, but got Unknown
			//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03da: Expected O, but got Unknown
			//IL_03e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f1: Expected O, but got Unknown
			//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0408: Expected O, but got Unknown
			//IL_0415: Unknown result type (might be due to invalid IL or missing references)
			//IL_041f: Expected O, but got Unknown
			//IL_042c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0436: Expected O, but got Unknown
			//IL_0443: Unknown result type (might be due to invalid IL or missing references)
			//IL_044d: Expected O, but got Unknown
			//IL_045a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0464: Expected O, but got Unknown
			//IL_0471: Unknown result type (might be due to invalid IL or missing references)
			//IL_047b: Expected O, but got Unknown
			//IL_0488: Unknown result type (might be due to invalid IL or missing references)
			//IL_0492: Expected O, but got Unknown
			//IL_049f: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a9: Expected O, but got Unknown
			//IL_04b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c0: Expected O, but got Unknown
			//IL_04cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d7: Expected O, but got Unknown
			//IL_04e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ee: Expected O, but got Unknown
			//IL_04fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0505: Expected O, but got Unknown
			//IL_051f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0529: Expected O, but got Unknown
			switch (StyleType)
			{
			case StiStyleType.Cards:
			{
				StiCardsElementStyle stiCardsElementStyle = (StiCardsElementStyle)baseStyle;
				return new StiCardsStyle
				{
					BackColor = stiCardsElementStyle.BackColor,
					ForeColor = stiCardsElementStyle.CellForeColor,
					LineColor = stiCardsElementStyle.LineColor
				};
			}
			case StiStyleType.Chart:
			{
				StiStyleCoreXF core2 = ((Stimulsoft.Report.Chart.StiChartStyle)baseStyle).Core;
				return new StiChartStyle
				{
					Brush = (StiBrush)core2.ChartBrush.Clone(),
					ChartAreaBrush = (StiBrush)core2.ChartAreaBrush.Clone(),
					ChartAreaBorderColor = core2.ChartAreaBorderColor,
					ChartAreaShowShadow = core2.ChartAreaShowShadow,
					SeriesLabelsBrush = (StiBrush)core2.SeriesLabelsBrush.Clone(),
					SeriesLabelsColor = core2.SeriesLabelsColor,
					SeriesLabelsBorderColor = core2.SeriesLabelsBorderColor,
					SeriesLabelsLineColor = core2.SeriesLabelsLineColor,
					TrendLineColor = core2.TrendLineColor,
					TrendLineShowShadow = core2.TrendLineShowShadow,
					LegendBrush = (StiBrush)core2.LegendBrush.Clone(),
					LegendLabelsColor = core2.LegendLabelsColor,
					LegendBorderColor = core2.LegendBorderColor,
					LegendTitleColor = core2.LegendTitleColor,
					AxisTitleColor = core2.AxisTitleColor,
					AxisLineColor = core2.AxisLineColor,
					AxisLabelsColor = core2.AxisLabelsColor,
					InterlacingHorBrush = (StiBrush)core2.InterlacingHorBrush.Clone(),
					InterlacingVertBrush = (StiBrush)core2.InterlacingVertBrush.Clone(),
					GridLinesHorColor = core2.GridLinesHorColor,
					GridLinesVertColor = core2.GridLinesVertColor,
					SeriesLighting = core2.SeriesLighting,
					SeriesShowShadow = core2.SeriesShowShadow,
					SeriesShowBorder = core2.SeriesShowBorder,
					MarkerVisible = core2.MarkerVisible,
					StyleColors = CloneColors(core2.StyleColors),
					BasicStyleColor = core2.BasicStyleColor
				};
			}
			case StiStyleType.CrossTab:
				return (StiCrossTabStyle)((StiService)(StiCrossTabStyle)baseStyle).Clone();
			case StiStyleType.Table:
			{
				StiTableStyleFX stiTableStyleFX = (StiTableStyleFX)baseStyle;
				return new StiTableStyle
				{
					BackColor = stiTableStyleFX.BackColor,
					DataColor = stiTableStyleFX.DataColor,
					DataForeground = stiTableStyleFX.DataForeground,
					SelectedDataColor = stiTableStyleFX.SelectedDataColor,
					SelectedDataForeground = stiTableStyleFX.SelectedDataForeground,
					AlternatingDataColor = stiTableStyleFX.AlternatingDataColor,
					AlternatingDataForeground = stiTableStyleFX.AlternatingDataForeground,
					HeaderColor = stiTableStyleFX.HeaderColor,
					HeaderForeground = stiTableStyleFX.HeaderForeground,
					HotHeaderColor = stiTableStyleFX.HotHeaderColor,
					FooterColor = stiTableStyleFX.FooterColor,
					FooterForeground = stiTableStyleFX.FooterForeground,
					GridColor = stiTableStyleFX.GridColor
				};
			}
			case StiStyleType.Gauge:
			{
				StiGaugeStyleCoreXF core = ((StiGaugeStyleXF)baseStyle).Core;
				return new StiGaugeStyle
				{
					Brush = core.Brush,
					ForeColor = core.ForeColor,
					TargetColor = core.TargetColor,
					BorderColor = core.BorderColor,
					BorderWidth = core.BorderWidth,
					TickMarkMajorBrush = (StiBrush)core.TickMarkMajorBrush.Clone(),
					TickMarkMajorBorder = (StiBrush)core.TickMarkMajorBorder.Clone(),
					TickMarkMajorBorderWidth = core.TickMarkMajorBorderWidth,
					TickMarkMinorBrush = (StiBrush)core.TickMarkMinorBrush.Clone(),
					TickMarkMinorBorder = (StiBrush)core.TickMarkMinorBorder.Clone(),
					TickMarkMinorBorderWidth = core.TickMarkMinorBorderWidth,
					TickLabelMajorTextBrush = (StiBrush)core.TickLabelMajorTextBrush.Clone(),
					TickLabelMajorFont = (Font)core.TickLabelMajorFont.Clone(),
					TickLabelMinorTextBrush = (StiBrush)core.TickLabelMinorTextBrush.Clone(),
					TickLabelMinorFont = (Font)core.TickLabelMinorFont.Clone(),
					LinearScaleBrush = (StiBrush)core.LinearScaleBrush.Clone(),
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
					NeedleCapBorderBrush = (StiBrush)core.NeedleCapBorderBrush.Clone(),
					NeedleBorderWidth = core.NeedleBorderWidth,
					MarkerBrush = (StiBrush)core.MarkerBrush.Clone()
				};
			}
			case StiStyleType.Map:
			{
				StiMapStyleFX stiMapStyleFX = (StiMapStyleFX)baseStyle;
				StiMapStyle stiMapStyle = new StiMapStyle
				{
					IndividualColor = stiMapStyleFX.IndividualColor,
					Colors = CloneColors(stiMapStyleFX.Colors),
					DefaultColor = stiMapStyleFX.DefaultColor,
					BackColor = stiMapStyleFX.BackColor,
					BorderSize = stiMapStyleFX.BorderSize,
					BorderColor = stiMapStyleFX.BorderColor,
					LabelShadowForeground = stiMapStyleFX.LabelShadowForeground,
					LabelForeground = stiMapStyleFX.LabelForeground,
					BubbleBackColor = stiMapStyleFX.BubbleBackColor,
					BubbleBorderColor = stiMapStyleFX.BubbleBorderColor
				};
				stiMapStyle.Heatmap.Color = stiMapStyleFX.Heatmap.Color;
				stiMapStyle.Heatmap.ZeroColor = stiMapStyleFX.Heatmap.ZeroColor;
				stiMapStyle.Heatmap.Mode = stiMapStyleFX.Heatmap.Mode;
				stiMapStyle.HeatmapWithGroup.Colors = stiMapStyleFX.HeatmapWithGroup.Colors;
				stiMapStyle.HeatmapWithGroup.ZeroColor = stiMapStyleFX.HeatmapWithGroup.ZeroColor;
				stiMapStyle.HeatmapWithGroup.Mode = stiMapStyleFX.HeatmapWithGroup.Mode;
				return stiMapStyle;
			}
			case StiStyleType.Indicator:
			{
				StiIndicatorElementStyle stiIndicatorElementStyle = (StiIndicatorElementStyle)baseStyle;
				return new StiIndicatorStyle
				{
					GlyphColor = stiIndicatorElementStyle.GlyphColor,
					BackColor = stiIndicatorElementStyle.BackColor,
					ForeColor = StiDashboardStyleHelper.GetForeColor(stiIndicatorElementStyle.Ident),
					HotBackColor = stiIndicatorElementStyle.HotBackColor,
					PositiveColor = stiIndicatorElementStyle.PositiveColor,
					NegativeColor = stiIndicatorElementStyle.NegativeColor
				};
			}
			case StiStyleType.Progress:
			{
				StiProgressElementStyle stiProgressElementStyle = (StiProgressElementStyle)baseStyle;
				return new StiProgressStyle
				{
					ForeColor = stiProgressElementStyle.ForeColor,
					TrackColor = stiProgressElementStyle.TrackColor,
					BandColor = stiProgressElementStyle.BandColor,
					SeriesColors = CloneColors(stiProgressElementStyle.SeriesColors),
					BackColor = stiProgressElementStyle.BackColor
				};
			}
			default:
				return null;
			}
		}

		private Color[] CloneColors(Color[] colors)
		{
			if (colors != null)
			{
				Color[] array = new Color[colors.Length];
				for (int i = 0; i < colors.Length; i++)
				{
					array[i] = colors[i];
				}
				return array;
			}
			return null;
		}
	}

	public static string DefaultChartStyle
	{
		get
		{
			StiSettings.Load();
			return StiSettings.GetStr("StyleBasedForm", "ChartStyle", "StiStyle29");
		}
		set
		{
			StiSettings.Set("StyleBasedForm", "ChartStyle", value);
			StiSettings.Save();
		}
	}

	public static string DefaultCrossTabStyle
	{
		get
		{
			StiSettings.Load();
			return StiSettings.GetStr("StyleBasedForm", "CrossTabStyle", "ffffff");
		}
		set
		{
			StiSettings.Set("StyleBasedForm", "CrossTabStyle", value);
			StiSettings.Save();
		}
	}

	public static string DefaultTableStyle
	{
		get
		{
			StiSettings.Load();
			return StiSettings.GetStr("StyleBasedForm", "TableStyle", "StiTable21StyleFX");
		}
		set
		{
			StiSettings.Set("StyleBasedForm", "TableStyle", value);
			StiSettings.Save();
		}
	}

	public static string DefaultGaugeStyle
	{
		get
		{
			StiSettings.Load();
			return StiSettings.GetStr("StyleBasedForm", "GaugeStyle", "StiGaugeStyleXF26");
		}
		set
		{
			StiSettings.Set("StyleBasedForm", "GaugeStyle", value);
			StiSettings.Save();
		}
	}

	public static string DefaultMapStyle
	{
		get
		{
			StiSettings.Load();
			return StiSettings.GetStr("StyleBasedForm", "MapStyle", "StiMap25StyleFX");
		}
		set
		{
			StiSettings.Set("StyleBasedForm", "MapStyle", value);
			StiSettings.Save();
		}
	}

	public static string DefaultIndicatorStyle
	{
		get
		{
			StiSettings.Load();
			return StiSettings.GetStr("StyleBasedForm", "IndicatorStyle", "StiBlueIndicatorElementStyle");
		}
		set
		{
			StiSettings.Set("StyleBasedForm", "IndicatorStyle", value);
			StiSettings.Save();
		}
	}

	public static string DefaultProgressStyle
	{
		get
		{
			StiSettings.Load();
			return StiSettings.GetStr("StyleBasedForm", "ProgressStyle", "StiBlueProgressElementStyle");
		}
		set
		{
			StiSettings.Set("StyleBasedForm", "ProgressStyle", value);
			StiSettings.Save();
		}
	}

	public static string DefaultCardsStyle
	{
		get
		{
			StiSettings.Load();
			return StiSettings.GetStr("StyleBasedForm", "CardsStyle", "StiBlueCardsElementStyle");
		}
		set
		{
			StiSettings.Set("StyleBasedForm", "CardsStyle", value);
			StiSettings.Save();
		}
	}

	public static StyleInfo GetInfo(Type styleType)
	{
		if (styleType == typeof(StiChartStyle))
		{
			List<StiBaseStyle> list = new List<StiBaseStyle>(StiOptions.Services.ChartStyles);
			list.Reverse();
			if (list.Count > 0)
			{
				return new StyleInfo(StiStyleType.Chart, list);
			}
		}
		else if (styleType == typeof(StiCrossTabStyle))
		{
			List<StiBaseStyle> list2 = new List<StiBaseStyle>();
			Color[] styleColors = StiOptions.Designer.CrossTab.StyleColors;
			foreach (Color color in styleColors)
			{
				StiCrossTabStyle item = new StiCrossTabStyle
				{
					Color = color
				};
				list2.Add(item);
			}
			if (list2.Count > 0)
			{
				return new StyleInfo(StiStyleType.CrossTab, list2);
			}
		}
		else if (styleType == typeof(StiTableStyle))
		{
			List<StiBaseStyle> list3 = new List<StiBaseStyle>(StiOptions.Services.TableStyles);
			list3.Reverse();
			if (list3.Count > 0)
			{
				return new StyleInfo(StiStyleType.Table, list3);
			}
		}
		else if (styleType == typeof(StiGaugeStyle))
		{
			List<StiBaseStyle> list4 = new List<StiBaseStyle>(StiOptions.Services.GaugeStyles);
			list4.Reverse();
			if (list4.Count > 0)
			{
				return new StyleInfo(StiStyleType.Gauge, list4);
			}
		}
		else if (styleType == typeof(StiMapStyle))
		{
			List<StiBaseStyle> list5 = new List<StiBaseStyle>(StiOptions.Services.MapStyles);
			list5.Reverse();
			if (list5.Count > 0)
			{
				return new StyleInfo(StiStyleType.Map, list5);
			}
		}
		else if (styleType == typeof(StiIndicatorStyle))
		{
			List<StiBaseStyle> list6 = new List<StiBaseStyle>(StiOptions.Services.Dashboards.IndicatorStyles);
			if (list6.Count > 0)
			{
				return new StyleInfo(StiStyleType.Indicator, list6);
			}
		}
		else if (styleType == typeof(StiProgressStyle))
		{
			List<StiBaseStyle> list7 = new List<StiBaseStyle>(StiOptions.Services.Dashboards.ProgressStyles);
			if (list7.Count > 0)
			{
				return new StyleInfo(StiStyleType.Progress, list7);
			}
		}
		else if (styleType == typeof(StiCardsStyle))
		{
			List<StiBaseStyle> list8 = new List<StiBaseStyle>(StiOptions.Services.Dashboards.CardsStyles);
			if (list8.Count > 0)
			{
				return new StyleInfo(StiStyleType.Cards, list8);
			}
		}
		return null;
	}

	public static string GetStyleName(StiBaseStyle style)
	{
		if (style is StiCrossTabStyle)
		{
			return ToHex(((StiCrossTabStyle)style).Color);
		}
		return ((object)style).GetType().Name;
	}

	private static string ToHex(Color c)
	{
		return $"{c.R:X2}{c.G:X2}{c.B:X2}".ToLowerInvariant();
	}
}
