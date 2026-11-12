using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Components.Gauge;
using Stimulsoft.Report.Components.Table;
using Stimulsoft.Report.CrossTab;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Design.Ribbon.Styles;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Gauge.Helpers;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.Maps;
using Stimulsoft.Report.Maps.Helpers;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Design.Controls;

public class StiStyleItem : StiCustomItem
{
	private static StiReport globalReport;

	internal static StiChart GlobalChart;

	private static StiCrossTab globalCrossTab;

	private StiGauge globalDefaultGauge;

	private Image bitmap;

	private StiBaseStyle selectedStyle;

	internal StiReport Report { get; set; }

	internal bool IsDashboardPage { get; set; }

	internal bool IsScreenPage { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiBaseStyle SelectedStyle
	{
		get
		{
			return selectedStyle;
		}
		set
		{
			selectedStyle = value;
			InvokeSelectedIndexChanged(this, EventArgs.Empty);
			Invalidate();
		}
	}

	public bool IsOnRibbonBar { get; set; }

	public bool IsShowMore { get; set; }

	public StiGaugeType? GaugeType { get; set; }

	public bool AddGaugeRange { get; set; }

	public bool IsDashboardAutoStyle { get; set; }

	public StiProgressElementMode ProgressMode { get; set; } = StiProgressElementMode.Circle;


	public string MapIdent { get; set; } = "USA";


	public StiFontIcons IndicatorFontIcons { get; set; } = StiFontIcons.Rating4;


	public bool IsRootItem { get; set; }

	public event EventHandler SelectedIndexChanged;

	public virtual void InvokeSelectedIndexChanged(object sender, EventArgs e)
	{
		this.SelectedIndexChanged?.Invoke(sender, e);
	}

	public void Collect()
	{
		if (bitmap != null)
		{
			bitmap.Dispose();
			bitmap = null;
		}
		Report = null;
		selectedStyle = null;
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
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Expected O, but got Unknown
		//IL_0389: Unknown result type (might be due to invalid IL or missing references)
		//IL_0393: Expected O, but got Unknown
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
		//IL_0cce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd8: Expected O, but got Unknown
		//IL_0f3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f49: Expected O, but got Unknown
		//IL_10c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_10cf: Expected O, but got Unknown
		//IL_13f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_13fb: Expected O, but got Unknown
		//IL_1702: Unknown result type (might be due to invalid IL or missing references)
		//IL_170c: Expected O, but got Unknown
		//IL_202a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2034: Expected O, but got Unknown
		//IL_1d18: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d22: Expected O, but got Unknown
		//IL_1a14: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a1e: Expected O, but got Unknown
		//IL_0459: Unknown result type (might be due to invalid IL or missing references)
		//IL_0463: Expected O, but got Unknown
		//IL_20b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_20b7: Expected O, but got Unknown
		//IL_0b17: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b21: Expected O, but got Unknown
		//IL_0fd8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fdf: Expected O, but got Unknown
		//IL_1166: Unknown result type (might be due to invalid IL or missing references)
		//IL_116d: Expected O, but got Unknown
		//IL_1490: Unknown result type (might be due to invalid IL or missing references)
		//IL_1497: Expected O, but got Unknown
		//IL_179b: Unknown result type (might be due to invalid IL or missing references)
		//IL_17a2: Expected O, but got Unknown
		//IL_1db1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1db8: Expected O, but got Unknown
		//IL_1aa4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aab: Expected O, but got Unknown
		//IL_1267: Unknown result type (might be due to invalid IL or missing references)
		//IL_126e: Expected O, but got Unknown
		//IL_1578: Unknown result type (might be due to invalid IL or missing references)
		//IL_157f: Expected O, but got Unknown
		//IL_188a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1891: Expected O, but got Unknown
		//IL_1ea0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ea7: Expected O, but got Unknown
		//IL_1b8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b95: Expected O, but got Unknown
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Expected O, but got Unknown
		//IL_126e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1275: Expected O, but got Unknown
		//IL_157f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1586: Expected O, but got Unknown
		//IL_1891: Unknown result type (might be due to invalid IL or missing references)
		//IL_1898: Expected O, but got Unknown
		//IL_1ea7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1eae: Expected O, but got Unknown
		//IL_1b95: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b9c: Expected O, but got Unknown
		//IL_0b85: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b8c: Expected O, but got Unknown
		//IL_0db1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0db8: Expected O, but got Unknown
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Expected O, but got Unknown
		//IL_0b8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b93: Expected O, but got Unknown
		//IL_0db8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dbf: Expected O, but got Unknown
		//IL_1362: Unknown result type (might be due to invalid IL or missing references)
		//IL_1369: Expected O, but got Unknown
		//IL_1673: Unknown result type (might be due to invalid IL or missing references)
		//IL_167a: Expected O, but got Unknown
		//IL_1985: Unknown result type (might be due to invalid IL or missing references)
		//IL_198c: Expected O, but got Unknown
		//IL_1f9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fa2: Expected O, but got Unknown
		//IL_1c89: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c90: Expected O, but got Unknown
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Expected O, but got Unknown
		//IL_058f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0596: Expected O, but got Unknown
		//IL_0528: Unknown result type (might be due to invalid IL or missing references)
		//IL_052f: Expected O, but got Unknown
		//IL_08c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08cb: Expected O, but got Unknown
		//IL_084a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0851: Expected O, but got Unknown
		//IL_0c3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c43: Expected O, but got Unknown
		//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f2: Expected O, but got Unknown
		//IL_0596: Unknown result type (might be due to invalid IL or missing references)
		//IL_059d: Expected O, but got Unknown
		//IL_08cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d2: Expected O, but got Unknown
		//IL_0eb0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb7: Expected O, but got Unknown
		//IL_0646: Unknown result type (might be due to invalid IL or missing references)
		//IL_064d: Expected O, but got Unknown
		//IL_0985: Unknown result type (might be due to invalid IL or missing references)
		//IL_098c: Expected O, but got Unknown
		bool flag = (Parent as StiStyleItem)?.SelectedStyle == SelectedStyle;
		switch (itemType)
		{
		case StiStyleItemType.Chart:
		{
			StiChart chart = GetChart();
			if (selectedStyle is StiChartStyle)
			{
				StiCustomStyle stiCustomStyle = new StiCustomStyle(selectedStyle.Name);
				stiCustomStyle.CustomCore.ReportChartStyle = selectedStyle as StiChartStyle;
				chart.Style = stiCustomStyle;
				chart.CustomStyleName = stiCustomStyle.CustomCore.ReportChartStyle.Name;
				chart.Core.ApplyStyle(chart.Style);
			}
			else
			{
				chart.ComponentStyle = null;
				chart.Style = (Stimulsoft.Report.Chart.StiChartStyle)selectedStyle;
				chart.Core.ApplyStyle(chart.Style);
			}
			string text10 = null;
			if (IsDashboardPage || IsScreenPage)
			{
				text10 = selectedStyle.DashboardName;
			}
			if (text10 == null)
			{
				text10 = (string.IsNullOrEmpty(selectedStyle.Name) ? selectedStyle.ServiceName : selectedStyle.Name);
			}
			if (IsDashboardAutoStyle)
			{
				text10 = StiLocalization.Get("PropertyEnum", "StiGaugeCalculationModeAuto");
			}
			RectangleF rect3 = new RectangleF(0f, 0f, rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0));
			bitmap = (Image)new Bitmap(rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0));
			Graphics val48 = Graphics.FromImage(bitmap);
			try
			{
				double zoom = chart.Report.Info.Zoom;
				chart.Report.Info.Zoom = 1.0;
				chart.DrawChart(val48, rect3, useMargins: false, useBackground: false);
				chart.Report.Info.Zoom = zoom;
				if (string.IsNullOrEmpty(text10))
				{
					break;
				}
				Font val49 = new Font("Arial", 8f);
				try
				{
					StringFormat val50 = new StringFormat();
					try
					{
						SizeF sizeF10 = val48.MeasureString(text10, val49);
						val50.Alignment = (StringAlignment)1;
						val50.LineAlignment = (StringAlignment)1;
						RectangleF rectangleF12 = new RectangleF(rect3.Right - sizeF10.Width - (float)StiScale.XXI(7.0), rect3.Bottom - sizeF10.Height - (float)StiScale.YYI(7.0), sizeF10.Width + (float)StiScale.XXI(6.0), sizeF10.Height + (float)StiScale.YYI(6.0));
						SolidBrush val51 = new SolidBrush(Color.FromArgb(200, 255, 255, 255));
						try
						{
							val48.FillRectangle((Brush)(object)val51, rectangleF12);
						}
						finally
						{
							((IDisposable)val51)?.Dispose();
						}
						val48.DrawString(text10, val49, Brushes.Black, rectangleF12, val50);
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
				((IDisposable)val48)?.Dispose();
			}
		}
		case StiStyleItemType.Gauge:
		{
			StiGauge gauge = GetGauge();
			if (selectedStyle is StiGaugeStyleXF)
			{
				gauge.CustomStyleName = null;
				gauge.Style = (IStiGaugeStyle)selectedStyle;
				gauge.ApplyStyle(gauge.Style);
			}
			else if (selectedStyle is StiGaugeStyle stiGaugeStyle)
			{
				StiGaugeStyle stiGaugeStyle2 = stiGaugeStyle.Clone() as StiGaugeStyle;
				if (stiGaugeStyle2.NeedleBorderWidth > 7f)
				{
					stiGaugeStyle2.NeedleBorderWidth = 7f;
				}
				StiCustomGaugeStyle stiCustomGaugeStyle = new StiCustomGaugeStyle(stiGaugeStyle2);
				gauge.CustomStyleName = stiCustomGaugeStyle.Name;
				gauge.Style = stiCustomGaugeStyle;
				gauge.ApplyStyle(stiCustomGaugeStyle);
			}
			RectangleF rect2 = new RectangleF(0f, 0f, rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0));
			bitmap = (Image)new Bitmap(rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0));
			string text7 = null;
			if (IsDashboardPage || IsScreenPage)
			{
				text7 = selectedStyle.DashboardName;
			}
			if (text7 == null)
			{
				text7 = (string.IsNullOrEmpty(selectedStyle.Name) ? selectedStyle.ServiceName : selectedStyle.Name);
			}
			if (IsDashboardAutoStyle)
			{
				text7 = StiLocalization.Get("PropertyEnum", "StiGaugeCalculationModeAuto");
			}
			Graphics val34 = Graphics.FromImage(bitmap);
			try
			{
				StiGaugeGdiPainter stiGaugeGdiPainter = new StiGaugeGdiPainter();
				stiGaugeGdiPainter.DrawGauge(val34, gauge, rect2, 0.5f);
				if (flag && !(selectedStyle is StiGaugeStyle))
				{
					Pen val35 = new Pen(Color.FromArgb(255, 43, 86, 154), (float)(1.0 * StiScale.Factor));
					try
					{
						val34.DrawRectangle(val35, new Rectangle(0, 0, rect.Width - StiScale.XXI(7.0), rect.Height - StiScale.YYI(7.0)));
					}
					finally
					{
						((IDisposable)val35)?.Dispose();
					}
				}
				if (string.IsNullOrEmpty(text7))
				{
					break;
				}
				Font val36 = new Font("Arial", 8f);
				try
				{
					StringFormat val37 = new StringFormat();
					try
					{
						SizeF sizeF7 = val34.MeasureString(text7, val36);
						val37.Alignment = (StringAlignment)1;
						val37.LineAlignment = (StringAlignment)1;
						RectangleF rectangleF8 = new RectangleF(rect2.Right - sizeF7.Width - (float)StiScale.XXI(7.0), rect2.Bottom - sizeF7.Height - (float)StiScale.YYI(7.0), sizeF7.Width + (float)StiScale.XXI(6.0), sizeF7.Height + (float)StiScale.YYI(6.0));
						SolidBrush val38 = new SolidBrush(Color.FromArgb(200, 255, 255, 255));
						try
						{
							val34.FillRectangle((Brush)(object)val38, rectangleF8);
						}
						finally
						{
							((IDisposable)val38)?.Dispose();
						}
						val34.DrawString(text7, val36, Brushes.Black, rectangleF8, val37);
						break;
					}
					finally
					{
						((IDisposable)val37)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val36)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val34)?.Dispose();
			}
		}
		case StiStyleItemType.Map:
		{
			StiMap mapSample = StiMapHelper.GetMapSample(Report, MapIdent);
			mapSample.ClientRectangle = mapSample.Report.Unit.ConvertFromHInches(new RectangleD(0.0, 0.0, rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0)));
			string text8 = null;
			if (IsDashboardPage || IsScreenPage)
			{
				text8 = selectedStyle.DashboardName;
			}
			if (text8 == null)
			{
				text8 = ((!(selectedStyle is StiMapStyleFX)) ? selectedStyle.Name : (string.IsNullOrEmpty(selectedStyle.Name) ? ((StiMapStyleFX)selectedStyle).LocalizeName : selectedStyle.Name));
			}
			if (IsDashboardAutoStyle)
			{
				text8 = StiLocalization.Get("PropertyEnum", "StiGaugeCalculationModeAuto");
			}
			if (selectedStyle is StiMapStyleFX)
			{
				mapSample.ComponentStyle = null;
				mapSample.MapStyle = ((StiMapStyleFX)selectedStyle).StyleId;
			}
			else
			{
				mapSample.ComponentStyle = selectedStyle.Name;
			}
			StiMapGdiPainter stiMapGdiPainter = new StiMapGdiPainter();
			bitmap = (Image)(object)stiMapGdiPainter.GetScaleImage(mapSample);
			Graphics val39 = Graphics.FromImage(bitmap);
			try
			{
				if (flag && selectedStyle.GetType() != typeof(StiMapStyle))
				{
					Pen val40 = new Pen(Color.FromArgb(255, 43, 86, 154), (float)(1.0 * StiScale.Factor));
					try
					{
						val39.DrawRectangle(val40, new Rectangle(0, 0, bitmap.Width - StiScale.XXI(2.0), bitmap.Height - StiScale.YYI(2.0)));
					}
					finally
					{
						((IDisposable)val40)?.Dispose();
					}
				}
				if (string.IsNullOrEmpty(text8))
				{
					break;
				}
				Font val41 = new Font("Arial", (float)(17.0 * StiScale.Factor));
				try
				{
					StringFormat val42 = new StringFormat();
					try
					{
						SizeF sizeF8 = val39.MeasureString(text8, val41);
						val42.Alignment = (StringAlignment)1;
						val42.LineAlignment = (StringAlignment)1;
						RectangleF rectangleF9 = new RectangleF((float)bitmap.Width - sizeF8.Width - (float)StiScale.XXI(8.0), (float)bitmap.Height - sizeF8.Height - (float)StiScale.YYI(8.0), sizeF8.Width + (float)StiScale.XXI(6.0), sizeF8.Height + (float)StiScale.YYI(6.0));
						SolidBrush val43 = new SolidBrush(Color.FromArgb(200, 255, 255, 255));
						try
						{
							val39.FillRectangle((Brush)(object)val43, rectangleF9);
						}
						finally
						{
							((IDisposable)val43)?.Dispose();
						}
						val39.DrawString(text8, val41, Brushes.Black, rectangleF9, val42);
						break;
					}
					finally
					{
						((IDisposable)val42)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val41)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val39)?.Dispose();
			}
		}
		case StiStyleItemType.CrossTab:
		{
			StiCrossTab crossTabStyles = GetCrossTabStyles();
			crossTabStyles.ClientRectangle = crossTabStyles.Report.Unit.ConvertFromHInches(new RectangleD(0.0, 0.0, rect.Width, rect.Height));
			string text9;
			if (string.IsNullOrEmpty(selectedStyle.Name))
			{
				List<Color> list = new List<Color>(StiOptions.Designer.CrossTab.StyleColors);
				Color color = ((StiCrossTabStyle)selectedStyle).Color;
				int num = list.IndexOf(color);
				if (num == -1)
				{
					num = 0;
				}
				text9 = color.Name;
				crossTabStyles.CrossTabStyleIndex = num;
				if (!string.IsNullOrEmpty(text9) && text9.Length == 8 && text9.StartsWith("ff"))
				{
					text9 = text9.Substring(2);
				}
			}
			else
			{
				text9 = selectedStyle.Name;
				crossTabStyles.CrossTabStyle = selectedStyle.Name;
			}
			if (IsDashboardAutoStyle)
			{
				text9 = StiLocalization.Get("PropertyEnum", "StiGaugeCalculationModeAuto");
			}
			RectangleF rectangleF10 = new RectangleF(0f, 0f, rect.Width, rect.Height);
			bitmap = (Image)new Bitmap(rect.Width, rect.Height);
			Graphics val44 = Graphics.FromImage(bitmap);
			try
			{
				StiCrossTabStyle stiCrossTabStyle = (StiCrossTabStyle)selectedStyle;
				stiCrossTabStyle.DrawStyleForGallery(val44, new Rectangle(0, 0, rect.Width, rect.Height));
				if (IsDashboardPage || IsScreenPage || string.IsNullOrEmpty(text9))
				{
					break;
				}
				Font val45 = new Font("Arial", 8f);
				try
				{
					StringFormat val46 = new StringFormat();
					try
					{
						SizeF sizeF9 = val44.MeasureString(text9, val45);
						val46.Alignment = (StringAlignment)1;
						val46.LineAlignment = (StringAlignment)1;
						RectangleF rectangleF11 = new RectangleF(rectangleF10.Right - sizeF9.Width - (float)StiScale.XXI(16.0), rectangleF10.Bottom - sizeF9.Height - (float)StiScale.YYI(12.0), sizeF9.Width + (float)StiScale.XXI(6.0), sizeF9.Height + (float)StiScale.YYI(6.0));
						SolidBrush val47 = new SolidBrush(Color.FromArgb(200, 255, 255, 255));
						try
						{
							val44.FillRectangle((Brush)(object)val47, rectangleF11);
						}
						finally
						{
							((IDisposable)val47)?.Dispose();
						}
						val44.DrawString(text9, val45, Brushes.Black, rectangleF11, val46);
						break;
					}
					finally
					{
						((IDisposable)val46)?.Dispose();
					}
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
		case StiStyleItemType.Table:
		{
			RectangleF rectangleF6 = new RectangleF(0f, 0f, rect.Width, rect.Height);
			bitmap = (Image)new Bitmap(rect.Width, rect.Height);
			string text6 = ((!(selectedStyle is StiTableStyleFX)) ? (string.IsNullOrEmpty(selectedStyle.Name) ? selectedStyle.ServiceName : selectedStyle.Name) : ((StiTableStyleFX)selectedStyle).StyleId.ToString());
			if (IsDashboardAutoStyle)
			{
				text6 = StiLocalization.Get("PropertyEnum", "StiGaugeCalculationModeAuto");
			}
			Graphics val30 = Graphics.FromImage(bitmap);
			try
			{
				StiTableStyle stiTableStyle = (StiTableStyle)selectedStyle;
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
						if (SubItems.Count > 0)
						{
							rectangleF7.X -= StiScale.XXI(6.0);
							rectangleF7.Y += StiScale.YYI(2.0);
						}
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
			bitmap = (Image)new Bitmap(rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0));
			Graphics val28 = Graphics.FromImage(bitmap);
			try
			{
				if (selectedStyle is StiControlElementStyle)
				{
					StiControlElementStyle stiControlElementStyle = (StiControlElementStyle)selectedStyle;
					stiControlElementStyle.DrawStyleForGallery(val28, new Rectangle(0, 0, rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0)));
					if (flag)
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
				if (selectedStyle is StiDialogStyle)
				{
					StiCustomControlElementStyle stiCustomControlElementStyle = new StiCustomControlElementStyle((StiDialogStyle)selectedStyle);
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
			bitmap = (Image)new Bitmap(rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0));
			Graphics val23 = Graphics.FromImage(bitmap);
			try
			{
				string text5 = null;
				if (selectedStyle is StiIndicatorElementStyle)
				{
					StiIndicatorElementStyle stiIndicatorElementStyle = (StiIndicatorElementStyle)selectedStyle;
					stiIndicatorElementStyle.DrawStyleForGallery(val23, new Rectangle(0, 0, rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0)), IndicatorFontIcons, isDashboard: true, isFormUI: false);
					if (flag)
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
				else if (selectedStyle is StiIndicatorStyle)
				{
					StiCustomIndicatorElementStyle stiCustomIndicatorElementStyle = new StiCustomIndicatorElementStyle((StiIndicatorStyle)selectedStyle);
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
						if (IsRootItem)
						{
							rectangleF5.X -= StiScale.XXI(1.0);
							rectangleF5.Y -= StiScale.YYI(4.0);
						}
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
			bitmap = (Image)new Bitmap(rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0));
			Graphics val18 = Graphics.FromImage(bitmap);
			try
			{
				string text4 = null;
				if (selectedStyle is StiProgressElementStyle)
				{
					StiProgressElementStyle stiProgressElementStyle = (StiProgressElementStyle)selectedStyle;
					stiProgressElementStyle.DrawStyleForGallery(val18, new Rectangle(0, 0, rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0)), ProgressMode);
					if (flag)
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
					StiCustomProgressElementStyle stiCustomProgressElementStyle = new StiCustomProgressElementStyle((StiProgressStyle)selectedStyle);
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
						if (IsRootItem)
						{
							rectangleF4.X -= StiScale.XXI(1.0);
							rectangleF4.Y -= StiScale.YYI(3.0);
						}
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
			bitmap = (Image)new Bitmap(rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0));
			Graphics val13 = Graphics.FromImage(bitmap);
			try
			{
				string text3 = null;
				if (selectedStyle is StiTableElementStyle)
				{
					StiTableElementStyle stiTableElementStyle = (StiTableElementStyle)selectedStyle;
					stiTableElementStyle.DrawStyleForGallery(val13, new Rectangle(0, 0, rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0)));
					if (flag)
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
				else if (selectedStyle is StiTableStyle)
				{
					StiCustomTableElementStyle stiCustomTableElementStyle = new StiCustomTableElementStyle((StiTableStyle)selectedStyle);
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
						if (IsRootItem)
						{
							rectangleF3.X -= StiScale.XXI(1.0);
							rectangleF3.Y -= StiScale.YYI(4.0);
						}
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
			bitmap = (Image)new Bitmap(rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0));
			Graphics val8 = Graphics.FromImage(bitmap);
			try
			{
				string text2 = null;
				if (selectedStyle is StiCardsElementStyle stiCardsElementStyle)
				{
					stiCardsElementStyle.DrawStyleForGallery(val8, new Rectangle(0, 0, rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0)));
					if (flag)
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
				else if (selectedStyle is StiCardsStyle style)
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
						RectangleF rectangleF2 = new RectangleF((float)rect.Width - sizeF2.Width - (float)StiScale.XXI(15.0), (float)rect.Height - sizeF2.Height - (float)StiScale.YYI(12.0), sizeF2.Width + (float)StiScale.XXI(6.0), sizeF2.Height + (float)StiScale.YYI(6.0));
						if (IsRootItem)
						{
							rectangleF2.X -= StiScale.XXI(1.0);
							rectangleF2.Y -= StiScale.YYI(4.0);
						}
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
			bitmap = (Image)new Bitmap(rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0));
			Graphics val3 = Graphics.FromImage(bitmap);
			try
			{
				string text = null;
				if (selectedStyle is StiPivotElementStyle)
				{
					StiPivotElementStyle stiPivotElementStyle = (StiPivotElementStyle)selectedStyle;
					stiPivotElementStyle.DrawStyleForGallery(val3, new Rectangle(0, 0, rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0)));
					if (flag)
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
				else if (selectedStyle is StiCrossTabStyle)
				{
					StiCustomPivotElementStyle stiCustomPivotElementStyle = new StiCustomPivotElementStyle((StiCrossTabStyle)selectedStyle);
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
						if (IsRootItem)
						{
							rectangleF.X -= StiScale.XXI(1.0);
							rectangleF.Y -= StiScale.YYI(4.0);
						}
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
			bitmap = (Image)new Bitmap(rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0));
			Graphics val = Graphics.FromImage(bitmap);
			try
			{
				StiDashboardStyle stiDashboardStyle = (StiDashboardStyle)selectedStyle;
				stiDashboardStyle.DrawStyleForGallery(val, new Rectangle(0, 0, rect.Width - StiScale.XXI(6.0), rect.Height - StiScale.YYI(6.0)));
				if (flag)
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

	protected override void RenderButton(ItemPaintArgs p)
	{
		if (!base.IsOnGallery)
		{
			base.RenderButton(p);
		}
	}

	public override void Paint(ItemPaintArgs e)
	{
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Expected O, but got Unknown
		//IL_0690: Unknown result type (might be due to invalid IL or missing references)
		//IL_0697: Expected O, but got Unknown
		//IL_0791: Unknown result type (might be due to invalid IL or missing references)
		//IL_0798: Expected O, but got Unknown
		//IL_0697: Unknown result type (might be due to invalid IL or missing references)
		//IL_069e: Expected O, but got Unknown
		//IL_0798: Unknown result type (might be due to invalid IL or missing references)
		//IL_079f: Expected O, but got Unknown
		//IL_0751: Unknown result type (might be due to invalid IL or missing references)
		//IL_0758: Expected O, but got Unknown
		Rectangle bounds = Bounds;
		if (IsOnRibbonBar && bounds.Height > StiScale.YYI(30.0))
		{
			bounds.Height -= StiScale.I10;
		}
		else if (!e.IsOnMenu && !base.IsOnGallery)
		{
			bounds.Width -= StiScale.I10;
		}
		bounds.Width--;
		bounds.Height--;
		bounds.Inflate(-1, -1);
		if (!e.IsOnMenu)
		{
			if (Enabled)
			{
				e.Graphics.FillRectangle(Brushes.White, bounds);
				e.Graphics.DrawRectangle(Pens.Silver, bounds);
			}
			else
			{
				e.Graphics.FillRectangle(Brushes.WhiteSmoke, bounds);
				e.Graphics.DrawRectangle(Pens.LightGray, bounds);
			}
		}
		base.Paint(e);
		if (!Enabled)
		{
			return;
		}
		if (IsShowMore)
		{
			Font val = new Font("Arial", 8f);
			try
			{
				StringFormat val2 = new StringFormat();
				try
				{
					val2.Alignment = (StringAlignment)1;
					val2.LineAlignment = (StringAlignment)1;
					val2.Trimming = (StringTrimming)0;
					e.Graphics.DrawString(Text, val, Brushes.Black, (RectangleF)bounds, val2);
					return;
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		if (SelectedStyle != null)
		{
			bounds.Height++;
			if (bounds.Width > StiScale.XXI(50.0) && bounds.Height > StiScale.YYI(50.0))
			{
				if (selectedStyle is Stimulsoft.Report.Chart.StiChartStyle || selectedStyle is StiChartStyle)
				{
					InitImageSample(bounds, StiStyleItemType.Chart);
					e.Graphics.DrawImage(bitmap, new Rectangle(bounds.X + StiScale.I3, bounds.Y + StiScale.I3, bounds.Width - StiScale.I6, bounds.Height - StiScale.I6));
					return;
				}
				if (selectedStyle is StiGaugeStyleXF || selectedStyle is StiGaugeStyle)
				{
					InitImageSample(bounds, StiStyleItemType.Gauge);
					e.Graphics.DrawImage(bitmap, new Rectangle(bounds.X + StiScale.I3, bounds.Y + StiScale.I3, bounds.Width - StiScale.I6, bounds.Height - StiScale.I6));
					return;
				}
				if (selectedStyle is StiMapStyle)
				{
					InitImageSample(bounds, StiStyleItemType.Map);
					e.Graphics.DrawImage(bitmap, new Rectangle(bounds.X + StiScale.I3, bounds.Y + StiScale.I3, bounds.Width - StiScale.I6, bounds.Height - StiScale.I6));
					return;
				}
				if (selectedStyle is StiControlElementStyle || selectedStyle is StiDialogStyle)
				{
					InitImageSample(bounds, StiStyleItemType.DashboardControl);
					e.Graphics.DrawImage(bitmap, new Rectangle(bounds.X + StiScale.I3, bounds.Y + StiScale.I3, bounds.Width - StiScale.I6, bounds.Height - StiScale.I6));
					return;
				}
				if (selectedStyle is StiIndicatorElementStyle || selectedStyle is StiIndicatorStyle)
				{
					InitImageSample(bounds, StiStyleItemType.DashboardIndicator);
					e.Graphics.DrawImage(bitmap, new Rectangle(bounds.X + StiScale.I3, bounds.Y + StiScale.I3, bounds.Width - StiScale.I6, bounds.Height - StiScale.I6));
					return;
				}
				if (selectedStyle is StiProgressElementStyle || (IsDashboardPage && selectedStyle is StiProgressStyle))
				{
					InitImageSample(bounds, StiStyleItemType.DashboardProgress);
					e.Graphics.DrawImage(bitmap, new Rectangle(bounds.X + StiScale.I3, bounds.Y + StiScale.I3, bounds.Width - StiScale.I6, bounds.Height - StiScale.I6));
					return;
				}
				if (selectedStyle is StiTableElementStyle || (IsDashboardPage && selectedStyle is StiTableStyle))
				{
					InitImageSample(bounds, StiStyleItemType.DashboardTable);
					e.Graphics.DrawImage(bitmap, new Rectangle(bounds.X + StiScale.I3, bounds.Y + StiScale.I3, bounds.Width - StiScale.I6, bounds.Height - StiScale.I6));
					return;
				}
				if (selectedStyle is StiCardsElementStyle || (IsDashboardPage && selectedStyle is StiCardsStyle))
				{
					InitImageSample(bounds, StiStyleItemType.DashboardCards);
					e.Graphics.DrawImage(bitmap, new Rectangle(bounds.X + StiScale.I3, bounds.Y + StiScale.I3, bounds.Width - StiScale.I6, bounds.Height - StiScale.I6));
					return;
				}
				if (selectedStyle is StiPivotElementStyle || (IsDashboardPage && selectedStyle is StiCrossTabStyle))
				{
					InitImageSample(bounds, StiStyleItemType.DashboardPivot);
					e.Graphics.DrawImage(bitmap, new Rectangle(bounds.X + StiScale.I3, bounds.Y + StiScale.I3, bounds.Width - StiScale.I6, bounds.Height - StiScale.I6));
					return;
				}
				if (selectedStyle is StiTableStyle)
				{
					InitImageSample(bounds, StiStyleItemType.Table);
					e.Graphics.DrawImage(bitmap, bounds);
					return;
				}
				if (selectedStyle is StiCrossTabStyle)
				{
					InitImageSample(bounds, StiStyleItemType.CrossTab);
					e.Graphics.DrawImage(bitmap, bounds);
					return;
				}
				if (selectedStyle is StiDashboardStyle)
				{
					InitImageSample(bounds, StiStyleItemType.DashboardPage);
					e.Graphics.DrawImage(bitmap, new Rectangle(bounds.X + StiScale.I3, bounds.Y + StiScale.I3, bounds.Width - StiScale.I6, bounds.Height - StiScale.I6));
					return;
				}
			}
			if (base.IsOnGallery)
			{
				StiBaseStyle.DrawStyle(e.Graphics, bounds, SelectedStyle, (DrawItemState)0, fillBackground: false, paintImage: false);
			}
			else
			{
				StiBaseStyle.DrawStyle(e.Graphics, bounds, SelectedStyle, (DrawItemState)0, fillBackground: false, paintImage: false);
			}
		}
		else if (IsDashboardAutoStyle)
		{
			Font val3 = new Font("Arial", 8f);
			try
			{
				StringFormat val4 = new StringFormat();
				try
				{
					val4.Alignment = (StringAlignment)1;
					val4.LineAlignment = (StringAlignment)1;
					val4.Trimming = (StringTrimming)0;
					e.Graphics.DrawString(StiLocalization.Get("PropertyEnum", "StiGaugeCalculationModeAuto"), val3, Brushes.Black, (RectangleF)bounds, val4);
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
			if ((Parent as StiStyleItem)?.SelectedStyle?.Name == StiLocalization.Get("PropertyEnum", "StiGaugeCalculationModeAuto"))
			{
				Pen val5 = new Pen(Color.FromArgb(255, 43, 86, 154), 1f);
				try
				{
					e.Graphics.DrawRectangle(val5, bounds);
				}
				finally
				{
					((IDisposable)val5)?.Dispose();
				}
			}
		}
		else
		{
			if (!e.IsOnMenu && !base.IsOnGallery)
			{
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
					e.Graphics.DrawString(StiLocalization.Get("Report", "No"), val6, Brushes.Black, (RectangleF)bounds, val7);
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
		}
	}

	public override BaseItem Copy()
	{
		StiStyleItem stiStyleItem = new StiStyleItem
		{
			Name = Name,
			SelectedStyle = SelectedStyle
		};
		CopyToItem(stiStyleItem);
		return stiStyleItem;
	}

	public StiStyleItem()
	{
	}

	public StiStyleItem(bool isDashboardPage, bool isFormUI)
	{
		IsDashboardPage = isDashboardPage;
		IsScreenPage = isFormUI;
	}
}
