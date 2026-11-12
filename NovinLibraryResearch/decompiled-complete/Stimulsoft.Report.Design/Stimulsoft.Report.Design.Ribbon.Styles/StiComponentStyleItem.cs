using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Components.Gauge;
using Stimulsoft.Report.Components.Table;
using Stimulsoft.Report.CrossTab;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Gauge.Helpers;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.Maps;
using Stimulsoft.Report.Maps.Helpers;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Design.Ribbon.Styles;

internal sealed class StiComponentStyleItem : IDisposable
{
	private static StiReport globalReport;

	public static StiChart GlobalChart;

	private static StiGauge globalDefaultGauge;

	private static StiCrossTab globalCrossTab;

	public StiBaseStyle Style { get; set; }

	public bool IsNoItem { get; set; }

	public bool AddGaugeRange { get; set; }

	public int Width { get; set; }

	public int Height { get; set; }

	public Image Bitmap { get; set; }

	public bool IsDashboardPage { get; set; }

	public bool IsScreenPage { get; set; }

	public bool IsMouseOver { get; set; }

	public bool IsSelected { get; set; }

	public StiReport Report { get; set; }

	public StiGaugeType? GaugeType { get; set; }

	public bool IsDashboardAutoStyle { get; set; }

	public StiProgressElementMode ProgressMode { get; set; } = StiProgressElementMode.Circle;


	public string MapIdent { get; set; } = "USA";


	public StiFontIcons IndicatorFontIcons { get; set; } = StiFontIcons.Rating4;


	public StiBaseStyle SelectedStyle { get; set; }

	public string Text { get; set; }

	public StiComponentStyleItem(bool isDashboardPage = false, bool isScreenPage = false)
	{
		IsDashboardPage = isDashboardPage;
		IsScreenPage = isScreenPage;
	}

	public void Dispose()
	{
		if (Bitmap != null)
		{
			Bitmap.Dispose();
			Bitmap = null;
		}
		Report = null;
	}

	public StiComponentStyleButton CreateButton(StiRibbonStylesPopup popup)
	{
		StiComponentStyleButton stiComponentStyleButton = new StiComponentStyleButton(this);
		((Control)stiComponentStyleButton).Width = Width;
		((Control)stiComponentStyleButton).Height = Height;
		StiComponentStyleButton stiComponentStyleButton2 = stiComponentStyleButton;
		if (IsDashboardAutoStyle)
		{
			stiComponentStyleButton2.ButtonClick += popup.OnDashboardAutoStyleClick;
		}
		else
		{
			stiComponentStyleButton2.ButtonClick += popup.OnStyleClick;
		}
		return stiComponentStyleButton2;
	}

	public StiComponentStyleButton CreateButton(EventHandler method)
	{
		StiComponentStyleButton stiComponentStyleButton = new StiComponentStyleButton(this);
		((Control)stiComponentStyleButton).Width = Width;
		((Control)stiComponentStyleButton).Height = Height;
		StiComponentStyleButton stiComponentStyleButton2 = stiComponentStyleButton;
		stiComponentStyleButton2.ButtonClick += method;
		return stiComponentStyleButton2;
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
		if (Report != null && Report.Styles.Count > 0)
		{
			globalReport.Styles.AddRange(Report.Styles);
		}
		if (GlobalChart == null)
		{
			GlobalChart = new StiChart();
			GlobalChart.Legend.Visible = false;
			GlobalChart.SeriesLabels.Visible = false;
			if (GlobalChart.Area is StiAxisArea stiAxisArea)
			{
				stiAxisArea.AxisCore.SwitchOff();
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
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Expected O, but got Unknown
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_0390: Expected O, but got Unknown
		if (globalReport == null)
		{
			globalReport = new StiReport();
		}
		globalReport.Styles.Clear();
		if (Report != null && Report.Styles.Count > 0)
		{
			globalReport.Styles.AddRange(Report.Styles);
		}
		if (GaugeType.HasValue)
		{
			switch (GaugeType.GetValueOrDefault())
			{
			case StiGaugeType.FullCircular:
			{
				StiGauge stiGauge5 = new StiGauge();
				StiGaugeV2InitHelper.Init(stiGauge5, StiGaugeType.FullCircular, AddGaugeRange, skipText: true);
				return stiGauge5;
			}
			case StiGaugeType.HalfCircular:
			{
				StiGauge stiGauge4 = new StiGauge();
				StiGaugeV2InitHelper.Init(stiGauge4, StiGaugeType.HalfCircular, AddGaugeRange, skipText: true);
				return stiGauge4;
			}
			case StiGaugeType.Linear:
			{
				StiGauge stiGauge3 = new StiGauge();
				StiGaugeV2InitHelper.Init(stiGauge3, StiGaugeType.Linear, AddGaugeRange, skipText: true);
				return stiGauge3;
			}
			case StiGaugeType.HorizontalLinear:
			{
				StiGauge stiGauge2 = new StiGauge();
				StiGaugeV2InitHelper.Init(stiGauge2, StiGaugeType.HorizontalLinear, AddGaugeRange, skipText: true);
				return stiGauge2;
			}
			case StiGaugeType.Bullet:
			{
				StiGauge stiGauge = new StiGauge();
				StiGaugeV2InitHelper.Init(stiGauge, StiGaugeType.Bullet, AddGaugeRange, skipText: true);
				return stiGauge;
			}
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

	private void InitImageSample(Rectangle rect, StiStyleItemType itemType)
	{
		//IL_0ce7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf1: Expected O, but got Unknown
		//IL_0f10: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f1a: Expected O, but got Unknown
		//IL_109b: Unknown result type (might be due to invalid IL or missing references)
		//IL_10a5: Expected O, but got Unknown
		//IL_138a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1394: Expected O, but got Unknown
		//IL_165e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1668: Expected O, but got Unknown
		//IL_1ecf: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ed9: Expected O, but got Unknown
		//IL_1bfa: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c04: Expected O, but got Unknown
		//IL_1933: Unknown result type (might be due to invalid IL or missing references)
		//IL_193d: Expected O, but got Unknown
		//IL_0412: Unknown result type (might be due to invalid IL or missing references)
		//IL_041c: Expected O, but got Unknown
		//IL_1f5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f61: Expected O, but got Unknown
		//IL_0b30: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3a: Expected O, but got Unknown
		//IL_0fae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb5: Expected O, but got Unknown
		//IL_1141: Unknown result type (might be due to invalid IL or missing references)
		//IL_1148: Expected O, but got Unknown
		//IL_142e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1435: Expected O, but got Unknown
		//IL_16fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1703: Expected O, but got Unknown
		//IL_1c98: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c9f: Expected O, but got Unknown
		//IL_19c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_19cf: Expected O, but got Unknown
		//IL_1242: Unknown result type (might be due to invalid IL or missing references)
		//IL_1249: Expected O, but got Unknown
		//IL_1516: Unknown result type (might be due to invalid IL or missing references)
		//IL_151d: Expected O, but got Unknown
		//IL_17eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_17f2: Expected O, but got Unknown
		//IL_1d87: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d8e: Expected O, but got Unknown
		//IL_1ab2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ab9: Expected O, but got Unknown
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Expected O, but got Unknown
		//IL_1249: Unknown result type (might be due to invalid IL or missing references)
		//IL_1250: Expected O, but got Unknown
		//IL_151d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1524: Expected O, but got Unknown
		//IL_17f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_17f9: Expected O, but got Unknown
		//IL_1d8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d95: Expected O, but got Unknown
		//IL_1ab9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ac0: Expected O, but got Unknown
		//IL_0b9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba5: Expected O, but got Unknown
		//IL_0dca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dd1: Expected O, but got Unknown
		//IL_12fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1302: Expected O, but got Unknown
		//IL_15cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_15d6: Expected O, but got Unknown
		//IL_18a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_18ab: Expected O, but got Unknown
		//IL_1e40: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e47: Expected O, but got Unknown
		//IL_1b6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b72: Expected O, but got Unknown
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Expected O, but got Unknown
		//IL_0ba5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bac: Expected O, but got Unknown
		//IL_0dd1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dd8: Expected O, but got Unknown
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Expected O, but got Unknown
		//IL_054d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0554: Expected O, but got Unknown
		//IL_04e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ed: Expected O, but got Unknown
		//IL_0887: Unknown result type (might be due to invalid IL or missing references)
		//IL_088e: Expected O, but got Unknown
		//IL_080d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0814: Expected O, but got Unknown
		//IL_0c55: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c5c: Expected O, but got Unknown
		//IL_0e81: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e88: Expected O, but got Unknown
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Expected O, but got Unknown
		//IL_0554: Unknown result type (might be due to invalid IL or missing references)
		//IL_055b: Expected O, but got Unknown
		//IL_088e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0895: Expected O, but got Unknown
		//IL_0604: Unknown result type (might be due to invalid IL or missing references)
		//IL_060b: Expected O, but got Unknown
		//IL_0948: Unknown result type (might be due to invalid IL or missing references)
		//IL_094f: Expected O, but got Unknown
		switch (itemType)
		{
		case StiStyleItemType.Chart:
		{
			StiChart chart = GetChart();
			if (SelectedStyle is StiChartStyle)
			{
				StiCustomStyle stiCustomStyle = new StiCustomStyle(SelectedStyle.Name);
				stiCustomStyle.CustomCore.ReportChartStyle = SelectedStyle as StiChartStyle;
				chart.Style = stiCustomStyle;
				chart.CustomStyleName = stiCustomStyle.CustomCore.ReportChartStyle.Name;
				chart.Core.ApplyStyle(chart.Style);
			}
			else
			{
				chart.ComponentStyle = null;
				chart.Style = (Stimulsoft.Report.Chart.StiChartStyle)SelectedStyle;
				chart.Core.ApplyStyle(chart.Style);
			}
			string text7 = null;
			if (IsDashboardPage || IsScreenPage)
			{
				text7 = SelectedStyle.DashboardName;
			}
			if (text7 == null)
			{
				text7 = (string.IsNullOrEmpty(SelectedStyle.Name) ? SelectedStyle.ServiceName : SelectedStyle.Name);
			}
			if (IsDashboardAutoStyle)
			{
				text7 = StiLocalization.Get("PropertyEnum", "StiGaugeCalculationModeAuto");
			}
			RectangleF rect2 = new RectangleF(0f, 0f, rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0));
			Bitmap = (Image)new Bitmap(rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0));
			Graphics val34 = Graphics.FromImage(Bitmap);
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
		case StiStyleItemType.Gauge:
		{
			StiGauge gauge = GetGauge();
			if (SelectedStyle is StiGaugeStyleXF)
			{
				gauge.CustomStyleName = null;
				gauge.Style = (IStiGaugeStyle)SelectedStyle;
				gauge.ApplyStyle(gauge.Style);
			}
			else if (SelectedStyle is StiGaugeStyle)
			{
				StiCustomGaugeStyle stiCustomGaugeStyle = new StiCustomGaugeStyle((StiGaugeStyle)SelectedStyle);
				gauge.CustomStyleName = stiCustomGaugeStyle.Name;
				gauge.Style = stiCustomGaugeStyle;
				gauge.ApplyStyle(stiCustomGaugeStyle);
			}
			RectangleF rect3 = new RectangleF(0f, 0f, rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0));
			Bitmap = (Image)new Bitmap(rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0));
			string text8 = null;
			if (IsDashboardPage || IsScreenPage)
			{
				text8 = SelectedStyle.DashboardName;
			}
			if (text8 == null)
			{
				text8 = (string.IsNullOrEmpty(SelectedStyle.Name) ? SelectedStyle.ServiceName : SelectedStyle.Name);
			}
			if (IsDashboardAutoStyle)
			{
				text8 = StiLocalization.Get("PropertyEnum", "StiGaugeCalculationModeAuto");
			}
			Graphics val38 = Graphics.FromImage(Bitmap);
			try
			{
				StiGaugeGdiPainter stiGaugeGdiPainter = new StiGaugeGdiPainter();
				stiGaugeGdiPainter.DrawGauge(val38, gauge, rect3, 0.5f);
				if (IsSelected && !(SelectedStyle is StiGaugeStyle))
				{
					Pen val39 = new Pen(Color.FromArgb(255, 43, 86, 154), (float)(1.0 * StiScale.Factor));
					try
					{
						val38.DrawRectangle(val39, new Rectangle(0, 0, rect.Width - StiScale.XXI(7.0), rect.Height - StiScale.YYI(7.0)));
					}
					finally
					{
						((IDisposable)val39)?.Dispose();
					}
				}
				if (string.IsNullOrEmpty(text8))
				{
					break;
				}
				Font val40 = new Font("Arial", 8f);
				try
				{
					StringFormat val41 = new StringFormat();
					try
					{
						SizeF sizeF8 = val38.MeasureString(text8, val40);
						val41.Alignment = (StringAlignment)1;
						val41.LineAlignment = (StringAlignment)1;
						RectangleF rectangleF9 = new RectangleF(rect3.Right - sizeF8.Width - (float)StiScale.XXI(7.0), rect3.Bottom - sizeF8.Height - (float)StiScale.YYI(7.0), sizeF8.Width + (float)StiScale.XXI(6.0), sizeF8.Height + (float)StiScale.YYI(6.0));
						SolidBrush val42 = new SolidBrush(Color.FromArgb(200, 255, 255, 255));
						try
						{
							val38.FillRectangle((Brush)(object)val42, rectangleF9);
						}
						finally
						{
							((IDisposable)val42)?.Dispose();
						}
						val38.DrawString(text8, val40, Brushes.Black, rectangleF9, val41);
						break;
					}
					finally
					{
						((IDisposable)val41)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val40)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val38)?.Dispose();
			}
		}
		case StiStyleItemType.Map:
		{
			StiMap mapSample = StiMapHelper.GetMapSample(Report, MapIdent);
			mapSample.ClientRectangle = mapSample.Report.Unit.ConvertFromHInches(new RectangleD(0.0, 0.0, rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0)));
			string text10 = null;
			if (IsDashboardPage || IsScreenPage)
			{
				text10 = SelectedStyle.DashboardName;
			}
			if (text10 == null)
			{
				text10 = ((!(SelectedStyle is StiMapStyleFX)) ? SelectedStyle.Name : (string.IsNullOrEmpty(SelectedStyle.Name) ? ((StiMapStyleFX)SelectedStyle).LocalizeName : SelectedStyle.Name));
			}
			if (IsDashboardAutoStyle)
			{
				text10 = StiLocalization.Get("PropertyEnum", "StiGaugeCalculationModeAuto");
			}
			if (SelectedStyle is StiMapStyleFX)
			{
				mapSample.ComponentStyle = null;
				mapSample.MapStyle = ((StiMapStyleFX)SelectedStyle).StyleId;
			}
			else
			{
				mapSample.ComponentStyle = SelectedStyle.Name;
			}
			StiMapGdiPainter stiMapGdiPainter = new StiMapGdiPainter();
			Bitmap = (Image)(object)stiMapGdiPainter.GetScaleImage(mapSample);
			Graphics val47 = Graphics.FromImage(Bitmap);
			try
			{
				if (IsSelected && SelectedStyle.GetType() != typeof(StiMapStyle))
				{
					Pen val48 = new Pen(Color.FromArgb(255, 43, 86, 154), (float)(1.0 * StiScale.Factor));
					try
					{
						val47.DrawRectangle(val48, new Rectangle(0, 0, Bitmap.Width - StiScale.XXI(2.0), Bitmap.Height - StiScale.YYI(2.0)));
					}
					finally
					{
						((IDisposable)val48)?.Dispose();
					}
				}
				if (string.IsNullOrEmpty(text10))
				{
					break;
				}
				Font val49 = new Font("Arial", (float)(17.0 * StiScale.Factor));
				try
				{
					StringFormat val50 = new StringFormat();
					try
					{
						SizeF sizeF10 = val47.MeasureString(text10, val49);
						val50.Alignment = (StringAlignment)1;
						val50.LineAlignment = (StringAlignment)1;
						RectangleF rectangleF12 = new RectangleF((float)Bitmap.Width - sizeF10.Width - (float)StiScale.XXI(8.0), (float)Bitmap.Height - sizeF10.Height - (float)StiScale.YYI(8.0), sizeF10.Width + (float)StiScale.XXI(6.0), sizeF10.Height + (float)StiScale.YYI(6.0));
						SolidBrush val51 = new SolidBrush(Color.FromArgb(200, 255, 255, 255));
						try
						{
							val47.FillRectangle((Brush)(object)val51, rectangleF12);
						}
						finally
						{
							((IDisposable)val51)?.Dispose();
						}
						val47.DrawString(text10, val49, Brushes.Black, rectangleF12, val50);
						break;
					}
					finally
					{
						((IDisposable)val50)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val49)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val47)?.Dispose();
			}
		}
		case StiStyleItemType.CrossTab:
		{
			StiCrossTab crossTabStyles = GetCrossTabStyles();
			crossTabStyles.ClientRectangle = crossTabStyles.Report.Unit.ConvertFromHInches(new RectangleD(0.0, 0.0, rect.Width, rect.Height));
			string text9;
			if (string.IsNullOrEmpty(SelectedStyle.Name))
			{
				List<Color> list = new List<Color>(StiOptions.Designer.CrossTab.StyleColors);
				Color color = ((StiCrossTabStyle)SelectedStyle).Color;
				int num = list.IndexOf(color);
				if (num == -1)
				{
					num = 0;
				}
				text9 = color.Name switch
				{
					"ff0bac45" => "Dark Pastel Green", 
					"ffb5a1dd" => "Perfume", 
					"ffffc000" => "Amber", 
					"ffed7d31" => "Sun", 
					"ff239fd9" => "Summer Sky", 
					_ => color.Name, 
				};
				crossTabStyles.CrossTabStyleIndex = num;
			}
			else
			{
				text9 = SelectedStyle.Name;
				crossTabStyles.CrossTabStyle = SelectedStyle.Name;
			}
			if (IsDashboardAutoStyle)
			{
				text9 = StiLocalization.Get("PropertyEnum", "StiGaugeCalculationModeAuto");
			}
			RectangleF rectangleF10 = new RectangleF(0f, 0f, rect.Width, rect.Height);
			Bitmap = (Image)new Bitmap(rect.Width, rect.Height);
			Graphics val43 = Graphics.FromImage(Bitmap);
			try
			{
				StiCrossTabStyle stiCrossTabStyle = (StiCrossTabStyle)SelectedStyle;
				stiCrossTabStyle.DrawStyleForGallery(val43, new Rectangle(0, 0, rect.Width, rect.Height));
				if (IsDashboardPage || IsScreenPage || string.IsNullOrEmpty(text9))
				{
					break;
				}
				Font val44 = new Font("Arial", 8f);
				try
				{
					StringFormat val45 = new StringFormat();
					try
					{
						SizeF sizeF9 = val43.MeasureString(text9, val44);
						val45.Alignment = (StringAlignment)1;
						val45.LineAlignment = (StringAlignment)1;
						RectangleF rectangleF11 = new RectangleF(rectangleF10.Right - sizeF9.Width - (float)StiScale.XXI(16.0), rectangleF10.Bottom - sizeF9.Height - (float)StiScale.YYI(12.0), sizeF9.Width + (float)StiScale.XXI(6.0), sizeF9.Height + (float)StiScale.YYI(6.0));
						SolidBrush val46 = new SolidBrush(Color.FromArgb(200, 255, 255, 255));
						try
						{
							val43.FillRectangle((Brush)(object)val46, rectangleF11);
						}
						finally
						{
							((IDisposable)val46)?.Dispose();
						}
						val43.DrawString(text9, val44, Brushes.Black, rectangleF11, val45);
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
				((IDisposable)val43)?.Dispose();
			}
		}
		case StiStyleItemType.Table:
		{
			RectangleF rectangleF6 = new RectangleF(0f, 0f, rect.Width, rect.Height);
			Bitmap = (Image)new Bitmap(rect.Width, rect.Height);
			string text6 = ((!(SelectedStyle is StiTableStyleFX)) ? (string.IsNullOrEmpty(SelectedStyle.Name) ? SelectedStyle.ServiceName : SelectedStyle.Name) : ((StiTableStyleFX)SelectedStyle).StyleId.ToString());
			if (IsDashboardAutoStyle)
			{
				text6 = StiLocalization.Get("PropertyEnum", "StiGaugeCalculationModeAuto");
			}
			Graphics val30 = Graphics.FromImage(Bitmap);
			try
			{
				StiTableStyle stiTableStyle = (StiTableStyle)SelectedStyle;
				stiTableStyle.DrawStyleForGallery(val30, new Rectangle(0, 0, rect.Width, rect.Height));
				if (IsDashboardPage || IsScreenPage || string.IsNullOrEmpty(text6))
				{
					break;
				}
				Font val31 = new Font("Arial", 8f);
				try
				{
					StringFormat val32 = new StringFormat();
					try
					{
						SizeF sizeF6 = val30.MeasureString(text6, val31);
						val32.Alignment = (StringAlignment)1;
						val32.LineAlignment = (StringAlignment)1;
						RectangleF rectangleF7 = new RectangleF(rectangleF6.Right - sizeF6.Width - (float)StiScale.XXI(11.0), rectangleF6.Bottom - sizeF6.Height - (float)StiScale.YYI(12.0), sizeF6.Width + (float)StiScale.XXI(6.0), sizeF6.Height + (float)StiScale.YYI(6.0));
						SolidBrush val33 = new SolidBrush(Color.FromArgb(200, 255, 255, 255));
						try
						{
							val30.FillRectangle((Brush)(object)val33, rectangleF7);
						}
						finally
						{
							((IDisposable)val33)?.Dispose();
						}
						val30.DrawString(text6, val31, Brushes.Black, rectangleF7, val32);
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
				((IDisposable)val30)?.Dispose();
			}
		}
		case StiStyleItemType.DashboardControl:
		{
			Bitmap = (Image)new Bitmap(rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0));
			Graphics val28 = Graphics.FromImage(Bitmap);
			try
			{
				if (SelectedStyle is StiControlElementStyle)
				{
					StiControlElementStyle stiControlElementStyle = (StiControlElementStyle)SelectedStyle;
					stiControlElementStyle.DrawStyleForGallery(val28, new Rectangle(0, 0, rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0)));
					if (IsSelected)
					{
						Pen val29 = new Pen(Color.FromArgb(255, 43, 86, 154), (float)(1.0 * StiScale.Factor));
						try
						{
							val28.DrawRectangle(val29, new Rectangle(0, 0, rect.Width - StiScale.XXI(7.0), rect.Height - StiScale.YYI(7.0)));
							break;
						}
						finally
						{
							((IDisposable)val29)?.Dispose();
						}
					}
					break;
				}
				if (SelectedStyle is StiDialogStyle)
				{
					StiCustomControlElementStyle stiCustomControlElementStyle = new StiCustomControlElementStyle((StiDialogStyle)SelectedStyle);
					stiCustomControlElementStyle.DrawStyleForGallery(val28, new Rectangle(0, 0, rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0)));
				}
				break;
			}
			finally
			{
				((IDisposable)val28)?.Dispose();
			}
		}
		case StiStyleItemType.DashboardIndicator:
		{
			Bitmap = (Image)new Bitmap(rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0));
			Graphics val23 = Graphics.FromImage(Bitmap);
			try
			{
				string text5 = null;
				if (SelectedStyle is StiIndicatorElementStyle)
				{
					StiIndicatorElementStyle stiIndicatorElementStyle = (StiIndicatorElementStyle)SelectedStyle;
					stiIndicatorElementStyle.DrawStyleForGallery(val23, new Rectangle(0, 0, rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0)), IndicatorFontIcons, isDashboard: true, isFormUI: false);
					if (IsSelected)
					{
						Pen val24 = new Pen(Color.FromArgb(255, 43, 86, 154), (float)(1.0 * StiScale.Factor));
						try
						{
							val23.DrawRectangle(val24, new Rectangle(0, 0, rect.Width - StiScale.XXI(7.0), rect.Height - StiScale.YYI(7.0)));
						}
						finally
						{
							((IDisposable)val24)?.Dispose();
						}
					}
					text5 = stiIndicatorElementStyle.LocalizedName;
				}
				else if (SelectedStyle is StiIndicatorStyle)
				{
					StiCustomIndicatorElementStyle stiCustomIndicatorElementStyle = new StiCustomIndicatorElementStyle((StiIndicatorStyle)SelectedStyle);
					stiCustomIndicatorElementStyle.DrawStyleForGallery(val23, new Rectangle(0, 0, rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0)), IndicatorFontIcons, IsDashboardPage, IsScreenPage);
					text5 = stiCustomIndicatorElementStyle.LocalizedName;
				}
				if (IsDashboardAutoStyle)
				{
					text5 = StiLocalization.Get("PropertyEnum", "StiGaugeCalculationModeAuto");
				}
				if (string.IsNullOrEmpty(text5))
				{
					break;
				}
				Font val25 = new Font("Arial", 8f);
				try
				{
					StringFormat val26 = new StringFormat();
					try
					{
						SizeF sizeF5 = val23.MeasureString(text5, val25);
						val26.Alignment = (StringAlignment)1;
						val26.LineAlignment = (StringAlignment)1;
						RectangleF rectangleF5 = new RectangleF((float)rect.Width - sizeF5.Width - (float)StiScale.XXI(12.0), (float)rect.Height - sizeF5.Height - (float)StiScale.YYI(12.0), sizeF5.Width + (float)StiScale.XXI(6.0), sizeF5.Height + (float)StiScale.YYI(6.0));
						SolidBrush val27 = new SolidBrush(Color.FromArgb(200, 255, 255, 255));
						try
						{
							val23.FillRectangle((Brush)(object)val27, rectangleF5);
						}
						finally
						{
							((IDisposable)val27)?.Dispose();
						}
						val23.DrawString(text5, val25, Brushes.Black, rectangleF5, val26);
						break;
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
			finally
			{
				((IDisposable)val23)?.Dispose();
			}
		}
		case StiStyleItemType.DashboardProgress:
		{
			Bitmap = (Image)new Bitmap(rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0));
			Graphics val18 = Graphics.FromImage(Bitmap);
			try
			{
				string text4 = null;
				if (SelectedStyle is StiProgressElementStyle)
				{
					StiProgressElementStyle stiProgressElementStyle = (StiProgressElementStyle)SelectedStyle;
					stiProgressElementStyle.DrawStyleForGallery(val18, new Rectangle(0, 0, rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0)), ProgressMode);
					if (IsSelected)
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
					text4 = stiProgressElementStyle.LocalizedName;
				}
				else
				{
					StiCustomProgressElementStyle stiCustomProgressElementStyle = new StiCustomProgressElementStyle((StiProgressStyle)SelectedStyle);
					stiCustomProgressElementStyle.DrawStyleForGallery(val18, new Rectangle(0, 0, rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0)), ProgressMode);
					text4 = stiCustomProgressElementStyle.LocalizedName;
				}
				if (IsDashboardAutoStyle)
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
						RectangleF rectangleF4 = new RectangleF((float)rect.Width - sizeF4.Width - (float)StiScale.XXI(14.0), (float)rect.Height - sizeF4.Height - (float)StiScale.YYI(14.0), sizeF4.Width + (float)StiScale.XXI(6.0), sizeF4.Height + (float)StiScale.YYI(6.0));
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
		case StiStyleItemType.DashboardTable:
		{
			Bitmap = (Image)new Bitmap(rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0));
			Graphics val13 = Graphics.FromImage(Bitmap);
			try
			{
				string text3 = null;
				if (SelectedStyle is StiTableElementStyle)
				{
					StiTableElementStyle stiTableElementStyle = (StiTableElementStyle)SelectedStyle;
					stiTableElementStyle.DrawStyleForGallery(val13, new Rectangle(0, 0, rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0)));
					if (IsSelected)
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
					text3 = stiTableElementStyle.LocalizedName;
				}
				else if (SelectedStyle is StiTableStyle)
				{
					StiCustomTableElementStyle stiCustomTableElementStyle = new StiCustomTableElementStyle((StiTableStyle)SelectedStyle);
					stiCustomTableElementStyle.DrawStyleForGallery(val13, new Rectangle(0, 0, rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0)));
					text3 = stiCustomTableElementStyle.LocalizedName;
				}
				if (IsDashboardAutoStyle)
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
						RectangleF rectangleF3 = new RectangleF((float)rect.Width - sizeF3.Width - (float)StiScale.XXI(27.0), (float)rect.Height - sizeF3.Height - (float)StiScale.YYI(16.0), sizeF3.Width + (float)StiScale.XXI(6.0), sizeF3.Height + (float)StiScale.YYI(6.0));
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
		case StiStyleItemType.DashboardCards:
		{
			Bitmap = (Image)new Bitmap(rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0));
			Graphics val8 = Graphics.FromImage(Bitmap);
			try
			{
				string text2 = null;
				if (SelectedStyle is StiCardsElementStyle stiCardsElementStyle)
				{
					stiCardsElementStyle.DrawStyleForGallery(val8, new Rectangle(0, 0, rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0)));
					if (IsSelected)
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
					text2 = stiCardsElementStyle.LocalizedName;
				}
				else if (SelectedStyle is StiCardsStyle style)
				{
					StiCustomCardsElementStyle stiCustomCardsElementStyle = new StiCustomCardsElementStyle(style);
					stiCustomCardsElementStyle.DrawStyleForGallery(val8, new Rectangle(0, 0, rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0)));
					text2 = stiCustomCardsElementStyle.LocalizedName;
				}
				if (IsDashboardAutoStyle)
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
						RectangleF rectangleF2 = new RectangleF((float)rect.Width - sizeF2.Width - (float)StiScale.XXI(16.0), (float)rect.Height - sizeF2.Height - (float)StiScale.YYI(16.0), sizeF2.Width + (float)StiScale.XXI(6.0), sizeF2.Height + (float)StiScale.YYI(6.0));
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
		case StiStyleItemType.DashboardPivot:
		{
			Bitmap = (Image)new Bitmap(rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0));
			Graphics val3 = Graphics.FromImage(Bitmap);
			try
			{
				string text = null;
				if (SelectedStyle is StiPivotElementStyle)
				{
					StiPivotElementStyle stiPivotElementStyle = (StiPivotElementStyle)SelectedStyle;
					stiPivotElementStyle.DrawStyleForGallery(val3, new Rectangle(0, 0, rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0)));
					if (IsSelected)
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
				else if (SelectedStyle is StiCrossTabStyle)
				{
					StiCustomPivotElementStyle stiCustomPivotElementStyle = new StiCustomPivotElementStyle((StiCrossTabStyle)SelectedStyle);
					stiCustomPivotElementStyle.DrawStyleForGallery(val3, new Rectangle(0, 0, rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0)));
					text = stiCustomPivotElementStyle.LocalizedName;
				}
				if (IsDashboardAutoStyle)
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
		case StiStyleItemType.DashboardPage:
		{
			Bitmap = (Image)new Bitmap(rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0));
			Graphics val = Graphics.FromImage(Bitmap);
			try
			{
				StiDashboardStyle stiDashboardStyle = (StiDashboardStyle)SelectedStyle;
				stiDashboardStyle.DrawStyleForGallery(val, new Rectangle(0, 0, rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0)));
				if (IsSelected)
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

	public void InitImage(Rectangle rect)
	{
		if (SelectedStyle != null)
		{
			rect.Height++;
			if (SelectedStyle is Stimulsoft.Report.Chart.StiChartStyle || SelectedStyle is StiChartStyle)
			{
				InitImageSample(rect, StiStyleItemType.Chart);
			}
			else if (SelectedStyle is StiGaugeStyleXF || SelectedStyle is StiGaugeStyle)
			{
				InitImageSample(rect, StiStyleItemType.Gauge);
			}
			else if (SelectedStyle is StiMapStyle)
			{
				InitImageSample(rect, StiStyleItemType.Map);
			}
			else if (SelectedStyle is StiControlElementStyle || SelectedStyle is StiDialogStyle)
			{
				InitImageSample(rect, StiStyleItemType.DashboardControl);
			}
			else if (SelectedStyle is StiIndicatorElementStyle || SelectedStyle is StiIndicatorStyle)
			{
				InitImageSample(rect, StiStyleItemType.DashboardIndicator);
			}
			else if (SelectedStyle is StiProgressElementStyle || (IsDashboardPage && SelectedStyle is StiProgressStyle))
			{
				InitImageSample(rect, StiStyleItemType.DashboardProgress);
			}
			else if (SelectedStyle is StiTableElementStyle || (IsDashboardPage && SelectedStyle is StiTableStyle))
			{
				InitImageSample(rect, StiStyleItemType.DashboardTable);
			}
			else if (SelectedStyle is StiCardsElementStyle || (IsDashboardPage && SelectedStyle is StiCardsStyle))
			{
				InitImageSample(rect, StiStyleItemType.DashboardCards);
			}
			else if (SelectedStyle is StiPivotElementStyle || (IsDashboardPage && SelectedStyle is StiCrossTabStyle))
			{
				InitImageSample(rect, StiStyleItemType.DashboardPivot);
			}
			else if (SelectedStyle is StiTableStyle)
			{
				InitImageSample(rect, StiStyleItemType.Table);
			}
			else if (SelectedStyle is StiCrossTabStyle)
			{
				InitImageSample(rect, StiStyleItemType.CrossTab);
			}
			else if (SelectedStyle is StiDashboardStyle)
			{
				InitImageSample(rect, StiStyleItemType.DashboardPage);
			}
		}
	}
}
