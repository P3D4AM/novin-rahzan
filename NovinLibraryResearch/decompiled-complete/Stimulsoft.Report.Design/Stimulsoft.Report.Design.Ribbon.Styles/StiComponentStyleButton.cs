using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Components.Gauge;
using Stimulsoft.Report.Components.Table;
using Stimulsoft.Report.CrossTab;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Gauge.Helpers;
using Stimulsoft.Report.Maps;
using Stimulsoft.Report.Maps.Helpers;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Design.Ribbon.Styles;

[ToolboxItem(false)]
internal sealed class StiComponentStyleButton : Control
{
	public enum StiItemType
	{
		None,
		Chart,
		Gauge,
		Map,
		CrossTab,
		Table,
		DashboardControl,
		DashboardIndicator,
		DashboardProgress,
		DashboardTable,
		DashboardPage,
		DashboardPivot
	}

	private static StiReport globalReport;

	private static StiGauge globalDefaultGauge;

	private static StiGauge globalFullCircularGauge;

	private static StiGauge globalHalfCircularGauge;

	private static StiGauge globalLinearGauge;

	private static StiCrossTab globalCrossTab;

	private StiComponentStyleItem item;

	internal static StiChart GlobalChart { get; set; }

	internal bool AddGaugeRange { get; set; }

	public StiBaseStyle SelectedStyle
	{
		get
		{
			if (item == null)
			{
				return null;
			}
			return item.SelectedStyle;
		}
	}

	public event EventHandler ButtonClick;

	public void Collect()
	{
		item = null;
	}

	private static void AddDefaultSeries(StiChart chart)
	{
		chart.Series.Clear();
		Type defaultSeriesType = chart.Area.GetDefaultSeriesType();
		StiSeries stiSeries = (StiSeries)StiActivator.CreateObject(defaultSeriesType);
		stiSeries.ShowShadow = false;
		chart.Series.Add(stiSeries);
		if (!(stiSeries is StiFunnelSeries))
		{
			stiSeries = StiActivator.CreateObject(defaultSeriesType) as StiSeries;
			chart.Series.Add(stiSeries);
			stiSeries.ShowShadow = false;
			chart.Series.ApplyStyle(chart.Style);
		}
	}

	private StiChart GetChart()
	{
		if (globalReport == null)
		{
			globalReport = new StiReport();
		}
		globalReport.Styles.Clear();
		if (item.Report != null && item.Report.Styles.Count > 0)
		{
			globalReport.Styles.AddRange(item.Report.Styles);
		}
		if (GlobalChart == null)
		{
			GlobalChart = new StiChart();
			GlobalChart.Legend.Visible = false;
			GlobalChart.SeriesLabels.Visible = false;
			if (GlobalChart.Area is StiAxisArea stiAxisArea)
			{
				stiAxisArea.AxisCore?.SwitchOff();
			}
			AddDefaultSeries(GlobalChart);
			GlobalChart.ConstantLines.Clear();
			GlobalChart.Brush = new StiEmptyBrush();
			GlobalChart.Area.Brush = new StiEmptyBrush();
			globalReport.Pages[0].Components.Add(GlobalChart);
		}
		return GlobalChart;
	}

	private StiGauge GetGauge()
	{
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0327: Expected O, but got Unknown
		//IL_03f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fe: Expected O, but got Unknown
		if (globalReport == null)
		{
			globalReport = new StiReport();
		}
		globalReport.Styles.Clear();
		if (item.Report != null && item.Report.Styles.Count > 0)
		{
			globalReport.Styles.AddRange(item.Report.Styles);
		}
		if (item.GaugeType.HasValue)
		{
			switch (item.GaugeType.GetValueOrDefault())
			{
			case StiGaugeType.FullCircular:
				if (globalFullCircularGauge == null)
				{
					globalFullCircularGauge = new StiGauge();
					StiGaugeV2InitHelper.Init(globalFullCircularGauge, StiGaugeType.FullCircular, AddGaugeRange, skipText: true);
				}
				return globalFullCircularGauge;
			case StiGaugeType.HalfCircular:
				if (globalHalfCircularGauge == null)
				{
					globalHalfCircularGauge = new StiGauge();
					StiGaugeV2InitHelper.Init(globalHalfCircularGauge, StiGaugeType.HalfCircular, AddGaugeRange, skipText: true);
				}
				return globalHalfCircularGauge;
			case StiGaugeType.Linear:
				if (globalLinearGauge == null)
				{
					globalLinearGauge = new StiGauge();
					StiGaugeV2InitHelper.Init(globalLinearGauge, StiGaugeType.Linear, AddGaugeRange, skipText: true);
				}
				return globalLinearGauge;
			case StiGaugeType.HorizontalLinear:
				if (globalLinearGauge == null)
				{
					globalLinearGauge = new StiGauge();
					StiGaugeV2InitHelper.Init(globalLinearGauge, StiGaugeType.HorizontalLinear, AddGaugeRange, skipText: true);
				}
				return globalLinearGauge;
			case StiGaugeType.Bullet:
				if (globalLinearGauge == null)
				{
					globalLinearGauge = new StiGauge();
					StiGaugeV2InitHelper.Init(globalLinearGauge, StiGaugeType.Bullet, AddGaugeRange, skipText: true);
				}
				return globalLinearGauge;
			}
		}
		if (globalDefaultGauge == null)
		{
			globalDefaultGauge = new StiGauge();
			StiRadialScale stiRadialScale = new StiRadialScale
			{
				Center = new PointF(0.5f, 0.5f),
				Radius = 0.55f,
				EndWidth = 0.05f,
				Maximum = 100f,
				MinorInterval = 5f,
				StartAngle = 0f,
				StartWidth = 0.05f,
				SweepAngle = 340f,
				BorderBrush = new StiSolidBrush(Color.FromArgb(150, 255, 255, 255)),
				Brush = new StiSolidBrush(Color.FromArgb(50, 255, 255, 255))
			};
			globalDefaultGauge.Scales.Add(stiRadialScale);
			StiNeedle element = new StiNeedle
			{
				Scale = stiRadialScale,
				CapBorderWidth = 2f,
				CenterPoint = new PointF(0f, 0f),
				RelativeHeight = 0.06f,
				RelativeWidth = 0.45f,
				StartWidth = 0.1f,
				BorderBrush = new StiEmptyBrush(),
				Brush = new StiSolidBrush(Color.FromArgb(255, 244, 152, 78)),
				CapBorderBrush = new StiSolidBrush(Color.FromArgb(255, 244, 152, 78)),
				CapBrush = new StiSolidBrush(Color.FromArgb(255, 255, 255, 255)),
				CustomSkin = null,
				Font = new Font("Arial", 7f),
				TextBrush = new StiSolidBrush(Color.DimGray)
			};
			StiRadialTickMarkMajor element2 = new StiRadialTickMarkMajor
			{
				BorderWidth = 0f,
				OffsetAngle = -2f,
				Placement = StiPlacement.Overlay,
				RelativeHeight = 0.03f,
				RelativeWidth = 0.06f,
				Skin = StiTickMarkSkin.TriangleRight,
				BorderBrush = new StiSolidBrush(Color.FromArgb(255, 38, 68, 120)),
				Brush = new StiSolidBrush(Color.FromArgb(255, 38, 68, 120)),
				CustomSkin = null,
				MaximumValue = null,
				MinimumValue = null,
				SkipIndicesObj = null,
				SkipValuesObj = null
			};
			StiRadialTickLabelMajor element3 = new StiRadialTickLabelMajor
			{
				Font = new Font("Arial", 8f),
				MaximumValue = null,
				MinimumValue = null,
				SkipIndicesObj = null,
				SkipValuesObj = null,
				TextBrush = new StiSolidBrush(Color.FromArgb(255, 51, 71, 91)),
				TextFormat = null
			};
			stiRadialScale.Items.Add(element2);
			stiRadialScale.Items.Add(element3);
			stiRadialScale.Items.Add(element);
			globalReport.Pages[0].Components.Add(globalDefaultGauge);
		}
		return globalDefaultGauge;
	}

	private static StiCrossTab GetCrossTabStyles()
	{
		if (globalReport == null)
		{
			globalReport = new StiReport();
		}
		if (globalCrossTab == null)
		{
			globalCrossTab = new StiCrossTab();
			globalReport.Pages[0].Components.Add(globalCrossTab);
		}
		return globalCrossTab;
	}

	private void InitImageSample(Rectangle rect, StiItemType itemType)
	{
		//IL_0d5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d69: Expected O, but got Unknown
		//IL_0faf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb9: Expected O, but got Unknown
		//IL_115d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1167: Expected O, but got Unknown
		//IL_1481: Unknown result type (might be due to invalid IL or missing references)
		//IL_148b: Expected O, but got Unknown
		//IL_1782: Unknown result type (might be due to invalid IL or missing references)
		//IL_178c: Expected O, but got Unknown
		//IL_1d7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d86: Expected O, but got Unknown
		//IL_1a7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a89: Expected O, but got Unknown
		//IL_045b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0465: Expected O, but got Unknown
		//IL_1e16: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e1d: Expected O, but got Unknown
		//IL_0b9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba9: Expected O, but got Unknown
		//IL_1061: Unknown result type (might be due to invalid IL or missing references)
		//IL_1068: Expected O, but got Unknown
		//IL_121c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1223: Expected O, but got Unknown
		//IL_153e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1545: Expected O, but got Unknown
		//IL_1834: Unknown result type (might be due to invalid IL or missing references)
		//IL_183b: Expected O, but got Unknown
		//IL_1b31: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b38: Expected O, but got Unknown
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Expected O, but got Unknown
		//IL_1334: Unknown result type (might be due to invalid IL or missing references)
		//IL_133b: Expected O, but got Unknown
		//IL_1635: Unknown result type (might be due to invalid IL or missing references)
		//IL_163c: Expected O, but got Unknown
		//IL_1932: Unknown result type (might be due to invalid IL or missing references)
		//IL_1939: Expected O, but got Unknown
		//IL_1c2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c36: Expected O, but got Unknown
		//IL_0c11: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c18: Expected O, but got Unknown
		//IL_0e64: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e6b: Expected O, but got Unknown
		//IL_133b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1342: Expected O, but got Unknown
		//IL_163c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1643: Expected O, but got Unknown
		//IL_1939: Unknown result type (might be due to invalid IL or missing references)
		//IL_1940: Expected O, but got Unknown
		//IL_1c36: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c3d: Expected O, but got Unknown
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Expected O, but got Unknown
		//IL_0c18: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c1f: Expected O, but got Unknown
		//IL_0e6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e72: Expected O, but got Unknown
		//IL_13ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_13f4: Expected O, but got Unknown
		//IL_16ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_16f5: Expected O, but got Unknown
		//IL_19eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_19f2: Expected O, but got Unknown
		//IL_1ce8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cef: Expected O, but got Unknown
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Expected O, but got Unknown
		//IL_05bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c2: Expected O, but got Unknown
		//IL_0554: Unknown result type (might be due to invalid IL or missing references)
		//IL_055b: Expected O, but got Unknown
		//IL_094f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0956: Expected O, but got Unknown
		//IL_08cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d2: Expected O, but got Unknown
		//IL_0cc8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ccf: Expected O, but got Unknown
		//IL_0f1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f22: Expected O, but got Unknown
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Expected O, but got Unknown
		//IL_05c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c9: Expected O, but got Unknown
		//IL_0956: Unknown result type (might be due to invalid IL or missing references)
		//IL_095d: Expected O, but got Unknown
		//IL_0672: Unknown result type (might be due to invalid IL or missing references)
		//IL_0679: Expected O, but got Unknown
		//IL_0a1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a21: Expected O, but got Unknown
		switch (itemType)
		{
		case StiItemType.Chart:
		{
			StiChart chart = GetChart();
			if (item.SelectedStyle is StiChartStyle)
			{
				StiCustomStyle stiCustomStyle = new StiCustomStyle(item.SelectedStyle.Name);
				stiCustomStyle.CustomCore.ReportChartStyle = item.SelectedStyle as StiChartStyle;
				chart.Style = stiCustomStyle;
				chart.CustomStyleName = stiCustomStyle.CustomCore.ReportChartStyle.Name;
				chart.Core.ApplyStyle(chart.Style);
			}
			else
			{
				chart.ComponentStyle = null;
				chart.Style = (Stimulsoft.Report.Chart.StiChartStyle)item.SelectedStyle;
				chart.Core.ApplyStyle(chart.Style);
			}
			string text7 = null;
			if (item.IsDashboardPage)
			{
				text7 = item.SelectedStyle.DashboardName;
			}
			if (text7 == null)
			{
				text7 = (string.IsNullOrEmpty(item.SelectedStyle.Name) ? item.SelectedStyle.ServiceName : item.SelectedStyle.Name);
			}
			if (item.IsDashboardAutoStyle)
			{
				text7 = StiLocalization.Get("PropertyEnum", "StiGaugeCalculationModeAuto");
			}
			RectangleF rect2 = new RectangleF(0f, 0f, rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0));
			item.Bitmap = (Image)new Bitmap(rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0));
			Graphics val34 = Graphics.FromImage(item.Bitmap);
			try
			{
				double zoom = chart.Report.Info.Zoom;
				chart.Report.Info.Zoom = 1.0;
				chart.DrawChart(val34, rect2, useMargins: false, useBackground: false);
				chart.Report.Info.Zoom = zoom;
				if (string.IsNullOrEmpty(text7))
				{
					break;
				}
				Font val35 = new Font("Arial", 8f);
				try
				{
					StringFormat val36 = new StringFormat();
					try
					{
						SizeF sizeF7 = val34.MeasureString(text7, val35);
						val36.Alignment = (StringAlignment)1;
						val36.LineAlignment = (StringAlignment)1;
						RectangleF rectangleF8 = new RectangleF(rect2.Right - sizeF7.Width - (float)StiScale.XXI(7.0), rect2.Bottom - sizeF7.Height - (float)StiScale.YYI(7.0), sizeF7.Width + (float)StiScale.XXI(6.0), sizeF7.Height + (float)StiScale.YYI(6.0));
						SolidBrush val37 = new SolidBrush(Color.FromArgb(200, 255, 255, 255));
						try
						{
							val34.FillRectangle((Brush)(object)val37, rectangleF8);
						}
						finally
						{
							((IDisposable)val37)?.Dispose();
						}
						val34.DrawString(text7, val35, Brushes.Black, rectangleF8, val36);
						break;
					}
					finally
					{
						((IDisposable)val36)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val35)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val34)?.Dispose();
			}
		}
		case StiItemType.Gauge:
		{
			StiGauge gauge = GetGauge();
			if (item.SelectedStyle is StiGaugeStyleXF)
			{
				gauge.CustomStyleName = null;
				gauge.Style = (IStiGaugeStyle)item.SelectedStyle;
				gauge.ApplyStyle(gauge.Style);
			}
			else if (item.SelectedStyle is StiGaugeStyle)
			{
				StiCustomGaugeStyle stiCustomGaugeStyle = new StiCustomGaugeStyle((StiGaugeStyle)item.SelectedStyle);
				gauge.CustomStyleName = stiCustomGaugeStyle.Name;
				gauge.Style = stiCustomGaugeStyle;
				gauge.ApplyStyle(stiCustomGaugeStyle);
			}
			RectangleF rect3 = new RectangleF(0f, 0f, rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0));
			item.Bitmap = (Image)new Bitmap(rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0));
			string text9 = null;
			if (item.IsDashboardPage)
			{
				text9 = item.SelectedStyle.DashboardName;
			}
			if (text9 == null)
			{
				text9 = (string.IsNullOrEmpty(item.SelectedStyle.Name) ? item.SelectedStyle.ServiceName : item.SelectedStyle.Name);
			}
			if (item.IsDashboardAutoStyle)
			{
				text9 = StiLocalization.Get("PropertyEnum", "StiGaugeCalculationModeAuto");
			}
			Graphics val42 = Graphics.FromImage(item.Bitmap);
			try
			{
				StiGaugeGdiPainter stiGaugeGdiPainter = new StiGaugeGdiPainter();
				stiGaugeGdiPainter.DrawGauge(val42, gauge, rect3, 0.5f);
				if (item.IsSelected && !(item.SelectedStyle is StiGaugeStyle))
				{
					Pen val43 = new Pen(Color.FromArgb(255, 43, 86, 154), (float)(1.0 * StiScale.Factor));
					try
					{
						val42.DrawRectangle(val43, new Rectangle(0, 0, rect.Width - StiScale.XXI(7.0), rect.Height - StiScale.YYI(7.0)));
					}
					finally
					{
						((IDisposable)val43)?.Dispose();
					}
				}
				if (string.IsNullOrEmpty(text9))
				{
					break;
				}
				Font val44 = new Font("Arial", 8f);
				try
				{
					StringFormat val45 = new StringFormat();
					try
					{
						SizeF sizeF9 = val42.MeasureString(text9, val44);
						val45.Alignment = (StringAlignment)1;
						val45.LineAlignment = (StringAlignment)1;
						RectangleF rectangleF11 = new RectangleF(rect3.Right - sizeF9.Width - (float)StiScale.XXI(7.0), rect3.Bottom - sizeF9.Height - (float)StiScale.YYI(7.0), sizeF9.Width + (float)StiScale.XXI(6.0), sizeF9.Height + (float)StiScale.YYI(6.0));
						SolidBrush val46 = new SolidBrush(Color.FromArgb(200, 255, 255, 255));
						try
						{
							val42.FillRectangle((Brush)(object)val46, rectangleF11);
						}
						finally
						{
							((IDisposable)val46)?.Dispose();
						}
						val42.DrawString(text9, val44, Brushes.Black, rectangleF11, val45);
						break;
					}
					finally
					{
						((IDisposable)val45)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val44)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val42)?.Dispose();
			}
		}
		case StiItemType.Map:
		{
			StiMap mapSample = StiMapHelper.GetMapSample(item.Report, item.MapIdent);
			mapSample.ClientRectangle = mapSample.Report.Unit.ConvertFromHInches(new RectangleD(0.0, 0.0, rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0)));
			string text6 = null;
			if (item.IsDashboardPage)
			{
				text6 = item.SelectedStyle.DashboardName;
			}
			if (text6 == null)
			{
				text6 = ((!(item.SelectedStyle is StiMapStyleFX)) ? item.SelectedStyle.Name : (string.IsNullOrEmpty(item.SelectedStyle.Name) ? ((StiMapStyleFX)item.SelectedStyle).LocalizeName : item.SelectedStyle.Name));
			}
			if (item.IsDashboardAutoStyle)
			{
				text6 = StiLocalization.Get("PropertyEnum", "StiGaugeCalculationModeAuto");
			}
			if (item.SelectedStyle is StiMapStyleFX)
			{
				mapSample.ComponentStyle = null;
				mapSample.MapStyle = ((StiMapStyleFX)item.SelectedStyle).StyleId;
			}
			else
			{
				mapSample.ComponentStyle = item.SelectedStyle.Name;
			}
			StiMapGdiPainter stiMapGdiPainter = new StiMapGdiPainter();
			item.Bitmap = (Image)(object)stiMapGdiPainter.GetScaleImage(mapSample);
			Graphics val29 = Graphics.FromImage(item.Bitmap);
			try
			{
				if (item.IsSelected && item.SelectedStyle.GetType() != typeof(StiMapStyle))
				{
					Pen val30 = new Pen(Color.FromArgb(255, 43, 86, 154), (float)(1.0 * StiScale.Factor));
					try
					{
						val29.DrawRectangle(val30, new Rectangle(0, 0, item.Bitmap.Width - StiScale.XXI(2.0), item.Bitmap.Height - StiScale.YYI(2.0)));
					}
					finally
					{
						((IDisposable)val30)?.Dispose();
					}
				}
				if (string.IsNullOrEmpty(text6))
				{
					break;
				}
				Font val31 = new Font("Arial", (float)(17.0 * StiScale.Factor));
				try
				{
					StringFormat val32 = new StringFormat();
					try
					{
						SizeF sizeF6 = val29.MeasureString(text6, val31);
						val32.Alignment = (StringAlignment)1;
						val32.LineAlignment = (StringAlignment)1;
						RectangleF rectangleF7 = new RectangleF((float)item.Bitmap.Width - sizeF6.Width - (float)StiScale.XXI(8.0), (float)item.Bitmap.Height - sizeF6.Height - (float)StiScale.YYI(8.0), sizeF6.Width + (float)StiScale.XXI(6.0), sizeF6.Height + (float)StiScale.YYI(6.0));
						SolidBrush val33 = new SolidBrush(Color.FromArgb(200, 255, 255, 255));
						try
						{
							val29.FillRectangle((Brush)(object)val33, rectangleF7);
						}
						finally
						{
							((IDisposable)val33)?.Dispose();
						}
						val29.DrawString(text6, val31, Brushes.Black, rectangleF7, val32);
						break;
					}
					finally
					{
						((IDisposable)val32)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val31)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val29)?.Dispose();
			}
		}
		case StiItemType.CrossTab:
		{
			StiCrossTab crossTabStyles = GetCrossTabStyles();
			crossTabStyles.ClientRectangle = crossTabStyles.Report.Unit.ConvertFromHInches(new RectangleD(0.0, 0.0, rect.Width, rect.Height));
			string text8;
			if (string.IsNullOrEmpty(item.SelectedStyle.Name))
			{
				List<Color> list = new List<Color>(StiOptions.Designer.CrossTab.StyleColors);
				Color color = ((StiCrossTabStyle)item.SelectedStyle).Color;
				int num = list.IndexOf(color);
				if (num == -1)
				{
					num = 0;
				}
				text8 = color.Name;
				crossTabStyles.CrossTabStyleIndex = num;
			}
			else
			{
				text8 = item.SelectedStyle.Name;
				crossTabStyles.CrossTabStyle = item.SelectedStyle.Name;
			}
			if (item.IsDashboardAutoStyle)
			{
				text8 = StiLocalization.Get("PropertyEnum", "StiGaugeCalculationModeAuto");
			}
			RectangleF rectangleF9 = new RectangleF(0f, 0f, rect.Width, rect.Height);
			item.Bitmap = (Image)new Bitmap(rect.Width, rect.Height);
			Graphics val38 = Graphics.FromImage(item.Bitmap);
			try
			{
				StiCrossTabStyle stiCrossTabStyle = (StiCrossTabStyle)item.SelectedStyle;
				stiCrossTabStyle.DrawStyleForGallery(val38, new Rectangle(0, 0, rect.Width, rect.Height));
				if (item.IsDashboardPage || string.IsNullOrEmpty(text8))
				{
					break;
				}
				Font val39 = new Font("Arial", 8f);
				try
				{
					StringFormat val40 = new StringFormat();
					try
					{
						SizeF sizeF8 = val38.MeasureString(text8, val39);
						val40.Alignment = (StringAlignment)1;
						val40.LineAlignment = (StringAlignment)1;
						RectangleF rectangleF10 = new RectangleF(rectangleF9.Right - sizeF8.Width - (float)StiScale.XXI(11.0), rectangleF9.Bottom - sizeF8.Height - (float)StiScale.YYI(12.0), sizeF8.Width + (float)StiScale.XXI(6.0), sizeF8.Height + (float)StiScale.YYI(6.0));
						SolidBrush val41 = new SolidBrush(Color.FromArgb(200, 255, 255, 255));
						try
						{
							val38.FillRectangle((Brush)(object)val41, rectangleF10);
						}
						finally
						{
							((IDisposable)val41)?.Dispose();
						}
						val38.DrawString(text8, val39, Brushes.Black, rectangleF10, val40);
						break;
					}
					finally
					{
						((IDisposable)val40)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val39)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val38)?.Dispose();
			}
		}
		case StiItemType.Table:
		{
			RectangleF rectangleF5 = new RectangleF(0f, 0f, rect.Width, rect.Height);
			item.Bitmap = (Image)new Bitmap(rect.Width, rect.Height);
			string text5 = ((!(item.SelectedStyle is StiTableStyleFX)) ? (string.IsNullOrEmpty(item.SelectedStyle.Name) ? item.SelectedStyle.ServiceName : item.SelectedStyle.Name) : ((StiTableStyleFX)item.SelectedStyle).StyleId.ToString());
			if (item.IsDashboardAutoStyle)
			{
				text5 = StiLocalization.Get("PropertyEnum", "StiGaugeCalculationModeAuto");
			}
			Graphics val25 = Graphics.FromImage(item.Bitmap);
			try
			{
				StiTableStyle stiTableStyle = (StiTableStyle)item.SelectedStyle;
				stiTableStyle.DrawStyleForGallery(val25, new Rectangle(0, 0, rect.Width, rect.Height));
				if (item.IsDashboardPage || string.IsNullOrEmpty(text5))
				{
					break;
				}
				Font val26 = new Font("Arial", 8f);
				try
				{
					StringFormat val27 = new StringFormat();
					try
					{
						SizeF sizeF5 = val25.MeasureString(text5, val26);
						val27.Alignment = (StringAlignment)1;
						val27.LineAlignment = (StringAlignment)1;
						RectangleF rectangleF6 = new RectangleF(rectangleF5.Right - sizeF5.Width - (float)StiScale.XXI(11.0), rectangleF5.Bottom - sizeF5.Height - (float)StiScale.YYI(12.0), sizeF5.Width + (float)StiScale.XXI(6.0), sizeF5.Height + (float)StiScale.YYI(6.0));
						SolidBrush val28 = new SolidBrush(Color.FromArgb(200, 255, 255, 255));
						try
						{
							val25.FillRectangle((Brush)(object)val28, rectangleF6);
						}
						finally
						{
							((IDisposable)val28)?.Dispose();
						}
						val25.DrawString(text5, val26, Brushes.Black, rectangleF6, val27);
						break;
					}
					finally
					{
						((IDisposable)val27)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val26)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val25)?.Dispose();
			}
		}
		case StiItemType.DashboardControl:
		{
			item.Bitmap = (Image)new Bitmap(rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0));
			Graphics val23 = Graphics.FromImage(item.Bitmap);
			try
			{
				if (item.SelectedStyle is StiControlElementStyle)
				{
					StiControlElementStyle stiControlElementStyle = (StiControlElementStyle)item.SelectedStyle;
					stiControlElementStyle.DrawStyleForGallery(val23, new Rectangle(0, 0, rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0)));
					if (item.IsSelected)
					{
						Pen val24 = new Pen(Color.FromArgb(255, 43, 86, 154), (float)(1.0 * StiScale.Factor));
						try
						{
							val23.DrawRectangle(val24, new Rectangle(0, 0, rect.Width - StiScale.XXI(7.0), rect.Height - StiScale.YYI(7.0)));
							break;
						}
						finally
						{
							((IDisposable)val24)?.Dispose();
						}
					}
					break;
				}
				if (item.SelectedStyle is StiDialogStyle)
				{
					StiCustomControlElementStyle stiCustomControlElementStyle = new StiCustomControlElementStyle((StiDialogStyle)item.SelectedStyle);
					stiCustomControlElementStyle.DrawStyleForGallery(val23, new Rectangle(0, 0, rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0)));
				}
				break;
			}
			finally
			{
				((IDisposable)val23)?.Dispose();
			}
		}
		case StiItemType.DashboardIndicator:
		{
			item.Bitmap = (Image)new Bitmap(rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0));
			Graphics val18 = Graphics.FromImage(item.Bitmap);
			try
			{
				string text4 = null;
				if (item.SelectedStyle is StiIndicatorElementStyle)
				{
					StiIndicatorElementStyle stiIndicatorElementStyle = (StiIndicatorElementStyle)item.SelectedStyle;
					stiIndicatorElementStyle.DrawStyleForGallery(val18, new Rectangle(0, 0, rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0)), item.IndicatorFontIcons, isDashboard: true, isFormUI: false);
					if (item.IsSelected)
					{
						Pen val19 = new Pen(Color.FromArgb(255, 43, 86, 154), (float)(1.0 * StiScale.Factor));
						try
						{
							val18.DrawRectangle(val19, new Rectangle(0, 0, rect.Width - StiScale.XXI(7.0), rect.Height - StiScale.YYI(7.0)));
						}
						finally
						{
							((IDisposable)val19)?.Dispose();
						}
					}
					text4 = stiIndicatorElementStyle.LocalizedName;
				}
				else if (item.SelectedStyle is StiIndicatorStyle)
				{
					StiCustomIndicatorElementStyle stiCustomIndicatorElementStyle = new StiCustomIndicatorElementStyle((StiIndicatorStyle)item.SelectedStyle);
					stiCustomIndicatorElementStyle.DrawStyleForGallery(val18, new Rectangle(0, 0, rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0)), item.IndicatorFontIcons, item.IsDashboardPage, isFormUI: false);
					text4 = stiCustomIndicatorElementStyle.LocalizedName;
				}
				if (item.IsDashboardAutoStyle)
				{
					text4 = StiLocalization.Get("PropertyEnum", "StiGaugeCalculationModeAuto");
				}
				if (string.IsNullOrEmpty(text4))
				{
					break;
				}
				Font val20 = new Font("Arial", 8f);
				try
				{
					StringFormat val21 = new StringFormat();
					try
					{
						SizeF sizeF4 = val18.MeasureString(text4, val20);
						val21.Alignment = (StringAlignment)1;
						val21.LineAlignment = (StringAlignment)1;
						RectangleF rectangleF4 = new RectangleF((float)rect.Width - sizeF4.Width - (float)StiScale.XXI(12.0), (float)rect.Height - sizeF4.Height - (float)StiScale.YYI(12.0), sizeF4.Width + (float)StiScale.XXI(6.0), sizeF4.Height + (float)StiScale.YYI(6.0));
						SolidBrush val22 = new SolidBrush(Color.FromArgb(200, 255, 255, 255));
						try
						{
							val18.FillRectangle((Brush)(object)val22, rectangleF4);
						}
						finally
						{
							((IDisposable)val22)?.Dispose();
						}
						val18.DrawString(text4, val20, Brushes.Black, rectangleF4, val21);
						break;
					}
					finally
					{
						((IDisposable)val21)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val20)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val18)?.Dispose();
			}
		}
		case StiItemType.DashboardProgress:
		{
			item.Bitmap = (Image)new Bitmap(rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0));
			Graphics val13 = Graphics.FromImage(item.Bitmap);
			try
			{
				string text3 = null;
				if (item.SelectedStyle is StiProgressElementStyle)
				{
					StiProgressElementStyle stiProgressElementStyle = (StiProgressElementStyle)item.SelectedStyle;
					stiProgressElementStyle.DrawStyleForGallery(val13, new Rectangle(0, 0, rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0)), item.ProgressMode);
					if (item.IsSelected)
					{
						Pen val14 = new Pen(Color.FromArgb(255, 43, 86, 154), (float)(1.0 * StiScale.Factor));
						try
						{
							val13.DrawRectangle(val14, new Rectangle(0, 0, rect.Width - StiScale.XXI(7.0), rect.Height - StiScale.YYI(7.0)));
						}
						finally
						{
							((IDisposable)val14)?.Dispose();
						}
					}
					text3 = stiProgressElementStyle.LocalizedName;
				}
				else
				{
					StiCustomProgressElementStyle stiCustomProgressElementStyle = new StiCustomProgressElementStyle((StiProgressStyle)item.SelectedStyle);
					stiCustomProgressElementStyle.DrawStyleForGallery(val13, new Rectangle(0, 0, rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0)), item.ProgressMode);
					text3 = stiCustomProgressElementStyle.LocalizedName;
				}
				if (item.IsDashboardAutoStyle)
				{
					text3 = StiLocalization.Get("PropertyEnum", "StiGaugeCalculationModeAuto");
				}
				if (string.IsNullOrEmpty(text3))
				{
					break;
				}
				Font val15 = new Font("Arial", 8f);
				try
				{
					StringFormat val16 = new StringFormat();
					try
					{
						SizeF sizeF3 = val13.MeasureString(text3, val15);
						val16.Alignment = (StringAlignment)1;
						val16.LineAlignment = (StringAlignment)1;
						RectangleF rectangleF3 = new RectangleF((float)rect.Width - sizeF3.Width - (float)StiScale.XXI(14.0), (float)rect.Height - sizeF3.Height - (float)StiScale.YYI(14.0), sizeF3.Width + (float)StiScale.XXI(6.0), sizeF3.Height + (float)StiScale.YYI(6.0));
						SolidBrush val17 = new SolidBrush(Color.FromArgb(200, 255, 255, 255));
						try
						{
							val13.FillRectangle((Brush)(object)val17, rectangleF3);
						}
						finally
						{
							((IDisposable)val17)?.Dispose();
						}
						val13.DrawString(text3, val15, Brushes.Black, rectangleF3, val16);
						break;
					}
					finally
					{
						((IDisposable)val16)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val15)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val13)?.Dispose();
			}
		}
		case StiItemType.DashboardTable:
		{
			item.Bitmap = (Image)new Bitmap(rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0));
			Graphics val8 = Graphics.FromImage(item.Bitmap);
			try
			{
				string text2 = null;
				if (item.SelectedStyle is StiTableElementStyle)
				{
					StiTableElementStyle stiTableElementStyle = (StiTableElementStyle)item.SelectedStyle;
					stiTableElementStyle.DrawStyleForGallery(val8, new Rectangle(0, 0, rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0)));
					if (item.IsSelected)
					{
						Pen val9 = new Pen(Color.FromArgb(255, 43, 86, 154), (float)(1.0 * StiScale.Factor));
						try
						{
							val8.DrawRectangle(val9, new Rectangle(0, 0, rect.Width - StiScale.XXI(7.0), rect.Height - StiScale.YYI(7.0)));
						}
						finally
						{
							((IDisposable)val9)?.Dispose();
						}
					}
					text2 = stiTableElementStyle.LocalizedName;
				}
				else if (item.SelectedStyle is StiTableStyle)
				{
					StiCustomTableElementStyle stiCustomTableElementStyle = new StiCustomTableElementStyle((StiTableStyle)item.SelectedStyle);
					stiCustomTableElementStyle.DrawStyleForGallery(val8, new Rectangle(0, 0, rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0)));
					text2 = stiCustomTableElementStyle.LocalizedName;
				}
				if (item.IsDashboardAutoStyle)
				{
					text2 = StiLocalization.Get("PropertyEnum", "StiGaugeCalculationModeAuto");
				}
				if (string.IsNullOrEmpty(text2))
				{
					break;
				}
				Font val10 = new Font("Arial", 8f);
				try
				{
					StringFormat val11 = new StringFormat();
					try
					{
						SizeF sizeF2 = val8.MeasureString(text2, val10);
						val11.Alignment = (StringAlignment)1;
						val11.LineAlignment = (StringAlignment)1;
						RectangleF rectangleF2 = new RectangleF((float)rect.Width - sizeF2.Width - (float)StiScale.XXI(27.0), (float)rect.Height - sizeF2.Height - (float)StiScale.YYI(16.0), sizeF2.Width + (float)StiScale.XXI(6.0), sizeF2.Height + (float)StiScale.YYI(6.0));
						SolidBrush val12 = new SolidBrush(Color.FromArgb(200, 255, 255, 255));
						try
						{
							val8.FillRectangle((Brush)(object)val12, rectangleF2);
						}
						finally
						{
							((IDisposable)val12)?.Dispose();
						}
						val8.DrawString(text2, val10, Brushes.Black, rectangleF2, val11);
						break;
					}
					finally
					{
						((IDisposable)val11)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val10)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val8)?.Dispose();
			}
		}
		case StiItemType.DashboardPivot:
		{
			item.Bitmap = (Image)new Bitmap(rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0));
			Graphics val3 = Graphics.FromImage(item.Bitmap);
			try
			{
				string text = null;
				if (item.SelectedStyle is StiPivotElementStyle)
				{
					StiPivotElementStyle stiPivotElementStyle = (StiPivotElementStyle)item.SelectedStyle;
					stiPivotElementStyle.DrawStyleForGallery(val3, new Rectangle(0, 0, rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0)));
					if (item.IsSelected)
					{
						Pen val4 = new Pen(Color.FromArgb(255, 43, 86, 154), (float)(1.0 * StiScale.Factor));
						try
						{
							val3.DrawRectangle(val4, new Rectangle(0, 0, rect.Width - StiScale.XXI(7.0), rect.Height - StiScale.YYI(7.0)));
						}
						finally
						{
							((IDisposable)val4)?.Dispose();
						}
					}
					text = stiPivotElementStyle.LocalizedName;
				}
				else if (item.SelectedStyle is StiCrossTabStyle)
				{
					StiCustomPivotElementStyle stiCustomPivotElementStyle = new StiCustomPivotElementStyle((StiCrossTabStyle)item.SelectedStyle);
					stiCustomPivotElementStyle.DrawStyleForGallery(val3, new Rectangle(0, 0, rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0)));
					text = stiCustomPivotElementStyle.LocalizedName;
				}
				if (item.IsDashboardAutoStyle)
				{
					text = StiLocalization.Get("PropertyEnum", "StiGaugeCalculationModeAuto");
				}
				if (string.IsNullOrEmpty(text))
				{
					break;
				}
				Font val5 = new Font("Arial", 8f);
				try
				{
					StringFormat val6 = new StringFormat();
					try
					{
						SizeF sizeF = val3.MeasureString(text, val5);
						val6.Alignment = (StringAlignment)1;
						val6.LineAlignment = (StringAlignment)1;
						RectangleF rectangleF = new RectangleF((float)rect.Width - sizeF.Width - (float)StiScale.XXI(27.0), (float)rect.Height - sizeF.Height - (float)StiScale.YYI(16.0), sizeF.Width + (float)StiScale.XXI(6.0), sizeF.Height + (float)StiScale.YYI(6.0));
						SolidBrush val7 = new SolidBrush(Color.FromArgb(200, 255, 255, 255));
						try
						{
							val3.FillRectangle((Brush)(object)val7, rectangleF);
						}
						finally
						{
							((IDisposable)val7)?.Dispose();
						}
						val3.DrawString(text, val5, Brushes.Black, rectangleF, val6);
						break;
					}
					finally
					{
						((IDisposable)val6)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val5)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		case StiItemType.DashboardPage:
		{
			item.Bitmap = (Image)new Bitmap(rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0));
			Graphics val = Graphics.FromImage(item.Bitmap);
			try
			{
				StiDashboardStyle stiDashboardStyle = (StiDashboardStyle)item.SelectedStyle;
				stiDashboardStyle.DrawStyleForGallery(val, new Rectangle(0, 0, rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0)));
				if (item.IsSelected)
				{
					Pen val2 = new Pen(Color.FromArgb(255, 43, 86, 154), (float)(1.0 * StiScale.Factor));
					try
					{
						val.DrawRectangle(val2, new Rectangle(0, 0, rect.Width - StiScale.XXI(7.0), rect.Height - StiScale.YYI(7.0)));
						break;
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
				}
				break;
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		}
	}

	public void InitImage()
	{
		if (item.SelectedStyle != null)
		{
			Rectangle clientRectangle = ((Control)this).ClientRectangle;
			clientRectangle.Height++;
			if (item.SelectedStyle is Stimulsoft.Report.Chart.StiChartStyle || item.SelectedStyle is StiChartStyle)
			{
				InitImageSample(clientRectangle, StiItemType.Chart);
			}
			else if (item.SelectedStyle is StiGaugeStyleXF || item.SelectedStyle is StiGaugeStyle)
			{
				InitImageSample(clientRectangle, StiItemType.Gauge);
			}
			else if (item.SelectedStyle is StiMapStyle)
			{
				InitImageSample(clientRectangle, StiItemType.Map);
			}
			else if (item.SelectedStyle is StiControlElementStyle || item.SelectedStyle is StiDialogStyle)
			{
				InitImageSample(clientRectangle, StiItemType.DashboardControl);
			}
			else if (item.SelectedStyle is StiIndicatorElementStyle || (item.IsDashboardPage && item.SelectedStyle is StiIndicatorStyle))
			{
				InitImageSample(clientRectangle, StiItemType.DashboardIndicator);
			}
			else if (item.SelectedStyle is StiProgressElementStyle || (item.IsDashboardPage && item.SelectedStyle is StiProgressStyle))
			{
				InitImageSample(clientRectangle, StiItemType.DashboardProgress);
			}
			else if (item.SelectedStyle is StiTableElementStyle || (item.IsDashboardPage && item.SelectedStyle is StiTableStyle))
			{
				InitImageSample(clientRectangle, StiItemType.DashboardTable);
			}
			else if (item.SelectedStyle is StiPivotElementStyle || (item.IsDashboardPage && item.SelectedStyle is StiCrossTabStyle))
			{
				InitImageSample(clientRectangle, StiItemType.DashboardPivot);
			}
			else if (item.SelectedStyle is StiTableStyle)
			{
				InitImageSample(clientRectangle, StiItemType.Table);
			}
			else if (item.SelectedStyle is StiCrossTabStyle)
			{
				InitImageSample(clientRectangle, StiItemType.CrossTab);
			}
			else if (item.SelectedStyle is StiDashboardStyle)
			{
				InitImageSample(clientRectangle, StiItemType.DashboardPage);
			}
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Expected O, but got Unknown
		//IL_06ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f5: Expected O, but got Unknown
		//IL_06f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fc: Expected O, but got Unknown
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Expected O, but got Unknown
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Expected O, but got Unknown
		//IL_0770: Unknown result type (might be due to invalid IL or missing references)
		//IL_0777: Expected O, but got Unknown
		((Control)this).OnPaint(e);
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		if (item.IsMouseOver)
		{
			SolidBrush val = new SolidBrush(StiCollectionNameStyleButton.MouseOverColor);
			try
			{
				e.Graphics.FillRectangle((Brush)(object)val, clientRectangle);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		if (item.IsSelected)
		{
			Pen val2 = new Pen(StiCollectionNameStyleButton.SelectedColor);
			try
			{
				e.Graphics.DrawRectangle(val2, clientRectangle.X, clientRectangle.Y, clientRectangle.Width - 1, clientRectangle.Height - 1);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		if (item.IsDashboardAutoStyle)
		{
			clientRectangle.X += StiScale.XXI(3.0);
			clientRectangle.Y += StiScale.YYI(3.0);
			clientRectangle.Width -= StiScale.XXI(6.0);
			clientRectangle.Height -= StiScale.YYI(6.0);
			e.Graphics.FillRectangle(Brushes.White, clientRectangle);
			Pen val3 = new Pen(StiCollectionNameStyleButton.StrokeColor);
			try
			{
				e.Graphics.DrawRectangle(val3, clientRectangle);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
			Font val4 = new Font("Arial", 8f);
			try
			{
				StringFormat val5 = new StringFormat();
				try
				{
					val5.Alignment = (StringAlignment)1;
					val5.LineAlignment = (StringAlignment)1;
					val5.Trimming = (StringTrimming)0;
					e.Graphics.DrawString(StiLocalization.Get("PropertyEnum", "StiGaugeCalculationModeAuto"), val4, Brushes.Black, (RectangleF)clientRectangle, val5);
					return;
				}
				finally
				{
					((IDisposable)val5)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
		}
		if (item.SelectedStyle != null)
		{
			clientRectangle.Height++;
			if (item.SelectedStyle is Stimulsoft.Report.Chart.StiChartStyle || item.SelectedStyle is StiChartStyle)
			{
				e.Graphics.DrawImage(item.Bitmap, new Rectangle(clientRectangle.X + StiScale.I3, clientRectangle.Y + StiScale.I3, clientRectangle.Width - StiScale.I6, clientRectangle.Height - StiScale.I6));
			}
			else if (item.SelectedStyle is StiGaugeStyleXF || item.SelectedStyle is StiGaugeStyle)
			{
				e.Graphics.DrawImage(item.Bitmap, new Rectangle(clientRectangle.X + StiScale.I3, clientRectangle.Y + StiScale.I3, clientRectangle.Width - StiScale.I6, clientRectangle.Height - StiScale.I6));
			}
			else if (item.SelectedStyle is StiMapStyle)
			{
				e.Graphics.DrawImage(item.Bitmap, new Rectangle(clientRectangle.X + StiScale.I3, clientRectangle.Y + StiScale.I3, clientRectangle.Width - StiScale.I6, clientRectangle.Height - StiScale.I6));
			}
			else if (item.SelectedStyle is StiControlElementStyle || item.SelectedStyle is StiDialogStyle)
			{
				e.Graphics.DrawImage(item.Bitmap, new Rectangle(clientRectangle.X + StiScale.I3, clientRectangle.Y + StiScale.I3, clientRectangle.Width - StiScale.I6, clientRectangle.Height - StiScale.I6));
			}
			else if (item.SelectedStyle is StiIndicatorElementStyle || item.SelectedStyle is StiIndicatorStyle)
			{
				e.Graphics.DrawImage(item.Bitmap, new Rectangle(clientRectangle.X + StiScale.I3, clientRectangle.Y + StiScale.I3, clientRectangle.Width - StiScale.I6, clientRectangle.Height - StiScale.I6));
			}
			else if (item.SelectedStyle is StiProgressElementStyle || (item.IsDashboardPage && item.SelectedStyle is StiProgressStyle))
			{
				e.Graphics.DrawImage(item.Bitmap, new Rectangle(clientRectangle.X + StiScale.I3, clientRectangle.Y + StiScale.I3, clientRectangle.Width - StiScale.I6, clientRectangle.Height - StiScale.I6));
			}
			else if (item.SelectedStyle is StiTableElementStyle || (item.IsDashboardPage && item.SelectedStyle is StiTableStyle))
			{
				e.Graphics.DrawImage(item.Bitmap, new Rectangle(clientRectangle.X + StiScale.I3, clientRectangle.Y + StiScale.I3, clientRectangle.Width - StiScale.I6, clientRectangle.Height - StiScale.I6));
			}
			else if (item.SelectedStyle is StiCardsElementStyle || (item.IsDashboardPage && item.SelectedStyle is StiCardsStyle))
			{
				e.Graphics.DrawImage(item.Bitmap, new Rectangle(clientRectangle.X + StiScale.I3, clientRectangle.Y + StiScale.I3, clientRectangle.Width - StiScale.I6, clientRectangle.Height - StiScale.I6));
			}
			else if (item.SelectedStyle is StiPivotElementStyle || (item.IsDashboardPage && item.SelectedStyle is StiCrossTabStyle))
			{
				e.Graphics.DrawImage(item.Bitmap, new Rectangle(clientRectangle.X + StiScale.I3, clientRectangle.Y + StiScale.I3, clientRectangle.Width - StiScale.I6, clientRectangle.Height - StiScale.I6));
			}
			else if (item.SelectedStyle is StiTableStyle)
			{
				e.Graphics.DrawImage(item.Bitmap, clientRectangle);
			}
			else if (item.SelectedStyle is StiCrossTabStyle)
			{
				e.Graphics.DrawImage(item.Bitmap, clientRectangle);
			}
			else if (item.SelectedStyle is StiDashboardStyle)
			{
				e.Graphics.DrawImage(item.Bitmap, new Rectangle(clientRectangle.X + StiScale.I3, clientRectangle.Y + StiScale.I3, clientRectangle.Width - StiScale.I6, clientRectangle.Height - StiScale.I6));
			}
			else
			{
				StiBaseStyle.DrawStyle(e.Graphics, clientRectangle, item.SelectedStyle, (DrawItemState)0, fillBackground: false, paintImage: false);
			}
			return;
		}
		Font val6 = new Font("Arial", 8f);
		try
		{
			StringFormat val7 = new StringFormat();
			try
			{
				val7.Alignment = (StringAlignment)1;
				val7.LineAlignment = (StringAlignment)1;
				val7.Trimming = (StringTrimming)0;
				e.Graphics.DrawString(StiLocalization.Get("Report", "No"), val6, Brushes.Black, (RectangleF)clientRectangle, val7);
			}
			finally
			{
				((IDisposable)val7)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val6)?.Dispose();
		}
		Pen val8 = new Pen(Color.FromArgb(255, 198, 198, 198));
		try
		{
			val8.DashStyle = (DashStyle)1;
			Rectangle rectangle = new Rectangle(clientRectangle.X + StiScale.XXI(1.0), clientRectangle.Y + StiScale.YYI(1.0), clientRectangle.Width - StiScale.XXI(2.0), clientRectangle.Height - StiScale.YYI(2.0));
			e.Graphics.DrawRectangle(val8, rectangle);
		}
		finally
		{
			((IDisposable)val8)?.Dispose();
		}
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((Control)this).OnMouseLeave(e);
		item.IsMouseOver = false;
		((Control)this).Invalidate();
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		((Control)this).OnMouseEnter(e);
		item.IsMouseOver = true;
		((Control)this).Invalidate();
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		((Control)this).OnMouseDown(e);
		this.ButtonClick?.Invoke(this, EventArgs.Empty);
	}

	public StiComponentStyleButton(StiComponentStyleItem item)
	{
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)131072, true);
		this.item = item;
	}
}
