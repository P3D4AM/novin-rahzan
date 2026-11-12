using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Images;

namespace Stimulsoft.Report.Chart.Design;

[ToolboxItem(false)]
public class StiCloudLabelsControl : UserControl
{
	public struct StiLabelInfo
	{
		public Bitmap Bitmap { get; set; }

		public StiSeriesLabels SeriesLabels { get; set; }

		public int ImageWidth { get; set; }

		public int ImageHeight { get; set; }

		public int X { get; set; }

		public int Y { get; set; }

		public int Width { get; set; }

		public int Height { get; set; }

		public StiLabelInfo(Bitmap bitmap, StiSeriesLabels seriesLabels, int x, int y, int width, int height, int imageWidth, int imageHeight)
		{
			Bitmap = bitmap;
			SeriesLabels = seriesLabels;
			X = x;
			Y = y;
			Width = width;
			Height = height;
			ImageWidth = imageWidth;
			ImageHeight = imageHeight;
		}
	}

	private class StiMouseScrollableControl : StiScrollableControl
	{
		protected override void OnMouseWheel(MouseEventArgs e)
		{
			if (e.Delta > 0)
			{
				base.VScrollBar.DoUp();
			}
			else
			{
				base.VScrollBar.DoDown();
			}
		}
	}

	private StiCloudChartForm chartWindow;

	private StiCloudChartConditionsCollectionControl conditionsCollectionControl;

	private Type lastAreaType;

	private Type lastStyleType;

	private IStiSeriesLabels lastSeriesLabels;

	private List<StiLabelInfo> listLabelsImages;

	private int labelsImagesSelectedIndex;

	private int labelsImagesMouseOverIndex;

	private Timer timerLabels;

	private IStiSeriesLabels lastMouseEnterSeriesLabels;

	private IContainer components;

	private StiCloudTabulator TabControl;

	private StiCloudTabulatorPage pageCommon;

	private StiCloudTabulatorPage pageConditions;

	private Panel pageCommonLeftPanel;

	private StiMouseScrollableControl panelLabels;

	private Panel pageConditionsContent;

	public StiCloudLabelsControl(StiCloudChartForm chartWindow)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		this.chartWindow = chartWindow;
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			timerLabels = new Timer
			{
				Interval = 70
			};
			timerLabels.Tick += timerLabelsTick;
			Localize();
			BuildLabels();
			TabControl.SelectedIndex = 0;
			DoScaling();
		}
	}

	private void Localize()
	{
		((Control)pageCommon).Text = StiLocalization.Get("Chart", "Common");
		((Control)pageConditions).Text = StiLocalization.Get("Chart", "ChartConditionsCollectionForm");
	}

	private void Panels_Paint(object sender, PaintEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		Panel val = (Panel)sender;
		Pen val2 = new Pen(Color.FromArgb(170, 170, 170));
		try
		{
			val2.DashPattern = new float[2] { 1f, 2f };
			Point point = new Point(((Control)val).Width - 1, 0);
			Point point2 = new Point(((Control)val).Width - 1, ((Control)val).Height);
			e.Graphics.DrawLine(val2, point, point2);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	private void DoScaling()
	{
		DockPaddingEdges dockPadding = TabControl.DockPadding;
		int right = (TabControl.TitleWidth = StiScale.XXI(110.0));
		dockPadding.Right = right;
		pageCommon.Image = StiReportDesignImages.Tasks.ChartSeriesLabelsCommon(StiImageSize.Double);
		pageConditions.Image = StiReportDesignImages.Tasks.ChartConditions(StiImageSize.Double);
	}

	public void Save()
	{
		if (chartWindow.Chart.SeriesLabels != null && conditionsCollectionControl != null)
		{
			((StiSeriesLabels)chartWindow.Chart.SeriesLabels).Conditions = conditionsCollectionControl.GetConditions();
		}
	}

	internal void Collect()
	{
		if (timerLabels != null)
		{
			timerLabels.Tick -= timerLabelsTick;
			((Component)(object)timerLabels).Dispose();
			timerLabels = null;
		}
		if (listLabelsImages == null)
		{
			return;
		}
		foreach (StiLabelInfo listLabelsImage in listLabelsImages)
		{
			((Image)listLabelsImage.Bitmap).Dispose();
		}
		listLabelsImages.Clear();
		listLabelsImages = null;
	}

	internal void BuildLabels()
	{
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Expected O, but got Unknown
		labelsImagesMouseOverIndex = -1;
		if (listLabelsImages == null)
		{
			listLabelsImages = new List<StiLabelInfo>();
		}
		if (listLabelsImages.Count > 0 && lastAreaType == chartWindow.Chart.Area.GetType() && lastStyleType == chartWindow.Chart.Style.GetType())
		{
			return;
		}
		lastAreaType = chartWindow.Chart.Area.GetType();
		lastStyleType = chartWindow.Chart.Style.GetType();
		labelsImagesSelectedIndex = -1;
		foreach (StiLabelInfo listLabelsImage in listLabelsImages)
		{
			((Image)listLabelsImage.Bitmap).Dispose();
		}
		listLabelsImages.Clear();
		StiChart simpleChart = StiSimpleChartHelper.GetSimpleChart(chartWindow.Chart);
		AddDefaultSeries(simpleChart);
		List<StiSeriesLabels> list = new List<StiSeriesLabels>();
		foreach (StiSeriesLabels item in StiOptions.Services.ChartSerieLabels.Where((StiSeriesLabels s) => s.ServiceEnabled))
		{
			if (simpleChart.Area.Core.CheckInLabelsTypes(item.GetType()))
			{
				list.Add(item);
			}
		}
		int num = StiScale.I4;
		int num2 = StiScale.I2;
		int num3 = StiScale.I(80);
		int num4 = StiScale.I(80);
		int width = StiScale.I(86);
		int height = StiScale.I(116);
		bool flag = false;
		if (listLabelsImages.Count < 6)
		{
			num3 = StiScale.I(160);
			num4 = StiScale.I(110);
			width = StiScale.I(166);
			height = StiScale.I(146);
			flag = true;
		}
		Type type = chartWindow.Chart.SeriesLabels.GetType();
		int num5 = 0;
		foreach (StiSeriesLabels item2 in list)
		{
			if (simpleChart.Area.Core.CheckInLabelsTypes(item2.GetType()))
			{
				simpleChart.SeriesLabels = (StiSeriesLabels)item2.Clone();
				simpleChart.Core.ApplyStyle(simpleChart.Style);
				RectangleF rect = new RectangleF(0f, 0f, num3, num4);
				Bitmap val = new Bitmap(num3, num4);
				Graphics val2 = Graphics.FromImage((Image)(object)val);
				try
				{
					double zoom = simpleChart.Report.Info.Zoom;
					simpleChart.Report.Info.Zoom = 1.0;
					simpleChart.DrawChart(val2, rect, useMargins: false, useBackground: false);
					simpleChart.Report.Info.Zoom = zoom;
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
				listLabelsImages.Add(new StiLabelInfo(val, item2, num, num2, width, height, num3, num4));
				if (type == item2.GetType())
				{
					labelsImagesSelectedIndex = num5;
				}
				if (flag)
				{
					num2 += StiScale.I(151);
				}
				else if (num5 % 2 == 0)
				{
					num += StiScale.I(94);
				}
				else
				{
					num = StiScale.I4;
					num2 += StiScale.I(121);
				}
				num5++;
			}
		}
		panelLabels.ScrollTop = 0;
		if (flag)
		{
			panelLabels.ScrollHeight = StiScale.I(151) * listLabelsImages.Count;
		}
		else
		{
			panelLabels.ScrollHeight = StiScale.I(121) * (listLabelsImages.Count / 2);
		}
		((Control)panelLabels).Invalidate();
	}

	private void AddDefaultSeries(StiChart chart)
	{
		Type type = ((chart.Series.Count > 0) ? chart.Series[0].GetType() : chart.Area.GetDefaultSeriesType());
		chart.Series.Clear();
		StiSeries stiSeries = (StiSeries)StiActivator.CreateObject(type);
		stiSeries.ShowShadow = false;
		chart.Series.Add(stiSeries);
		if (!(stiSeries is StiFunnelSeries))
		{
			stiSeries = (StiSeries)StiActivator.CreateObject(type);
			chart.Series.Add(stiSeries);
			stiSeries.ShowShadow = false;
		}
		chart.Series.ApplyStyle(chart.Style);
	}

	internal void CheckPropertyGridValue()
	{
		if (TabControl.SelectedTab == pageCommon)
		{
			((PropertyGrid)chartWindow.PropertyGrid).SelectedObject = chartWindow.Chart.SeriesLabels;
		}
		else
		{
			((PropertyGrid)chartWindow.PropertyGrid).SelectedObject = null;
		}
	}

	internal void UpdateSamplePosition()
	{
		if (TabControl.SelectedTab == pageCommon)
		{
			chartWindow.UpdateSamplePosition(0, 1, 2, 1, showRightSeparator: true, showSample: true);
		}
		else
		{
			chartWindow.UpdateSamplePosition(0, 0, 2, 2, showRightSeparator: true, showSample: false);
		}
	}

	private void panelSeriesLabels_Paint(object sender, PaintEventArgs e)
	{
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Expected O, but got Unknown
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Expected O, but got Unknown
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Expected O, but got Unknown
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Expected O, but got Unknown
		if (listLabelsImages == null || listLabelsImages.Count == 0)
		{
			return;
		}
		Graphics graphics = e.Graphics;
		int num = 0;
		foreach (StiLabelInfo listLabelsImage in listLabelsImages)
		{
			Rectangle rectangle = new Rectangle(listLabelsImage.X, listLabelsImage.Y - ((ScrollBar)panelLabels.VScrollBar).Value, listLabelsImage.Width, listLabelsImage.Height);
			if (num == labelsImagesSelectedIndex)
			{
				SolidBrush val = new SolidBrush(Color.FromArgb(212, 224, 241));
				try
				{
					graphics.FillRectangle((Brush)(object)val, rectangle);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
				Pen val2 = new Pen(Color.FromArgb(43, 87, 154));
				try
				{
					graphics.DrawRectangle(val2, rectangle);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			else if (num == labelsImagesMouseOverIndex)
			{
				SolidBrush val3 = new SolidBrush(Color.FromArgb(212, 224, 241));
				try
				{
					graphics.FillRectangle((Brush)(object)val3, rectangle);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
			Rectangle rectangle2 = new Rectangle(rectangle.X + StiScale.I3, rectangle.Y + StiScale.I6, listLabelsImage.ImageWidth, listLabelsImage.ImageHeight);
			graphics.DrawImage((Image)(object)listLabelsImage.Bitmap, rectangle2);
			SolidBrush val4 = new SolidBrush(((Control)panelLabels).ForeColor);
			try
			{
				StringFormat val5 = new StringFormat();
				try
				{
					val5.Alignment = (StringAlignment)1;
					val5.LineAlignment = (StringAlignment)1;
					rectangle2.Y += listLabelsImage.ImageHeight + StiScale.I1;
					rectangle2.Height = StiScale.I(28);
					graphics.DrawString(listLabelsImage.SeriesLabels.ToString(), ((Control)panelLabels).Font, (Brush)(object)val4, (RectangleF)rectangle2, val5);
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
			num++;
		}
	}

	private void panelLabels_MouseMove(object sender, MouseEventArgs e)
	{
		int num = 0;
		foreach (StiLabelInfo listLabelsImage in listLabelsImages)
		{
			if (new Rectangle(listLabelsImage.X, listLabelsImage.Y - ((ScrollBar)panelLabels.VScrollBar).Value, listLabelsImage.Width, listLabelsImage.Height).Contains(e.X, e.Y))
			{
				if (labelsImagesMouseOverIndex != num)
				{
					buttonLabelsMouseEnter(listLabelsImage);
					labelsImagesMouseOverIndex = num;
					((Control)panelLabels).Invalidate();
				}
				return;
			}
			num++;
		}
		if (labelsImagesMouseOverIndex != -1)
		{
			buttonLabelsMouseLeave();
			labelsImagesMouseOverIndex = -1;
			((Control)panelLabels).Invalidate();
		}
	}

	private void panelSeriesLabels_MouseDown(object sender, MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)e.Button != 1048576)
		{
			return;
		}
		int num = 0;
		foreach (StiLabelInfo listLabelsImage in listLabelsImages)
		{
			if (new Rectangle(listLabelsImage.X, listLabelsImage.Y - ((ScrollBar)panelLabels.VScrollBar).Value, listLabelsImage.Width, listLabelsImage.Height).Contains(e.X, e.Y))
			{
				if (labelsImagesSelectedIndex != num)
				{
					timerLabels.Stop();
					labelsImagesSelectedIndex = num;
					((Control)panelLabels).Invalidate();
					if (lastMouseEnterSeriesLabels != null)
					{
						JObject jObject = lastMouseEnterSeriesLabels.SaveToJsonObject(StiJsonSaveMode.Report);
						lastMouseEnterSeriesLabels = null;
						StiSeriesLabels stiSeriesLabels = (StiSeriesLabels)listLabelsImage.SeriesLabels.Clone();
						chartWindow.Chart.SeriesLabelsConditions.Clear();
						stiSeriesLabels.Chart = chartWindow.Chart;
						stiSeriesLabels.LoadFromJsonObject(jObject);
						chartWindow.Chart.SeriesLabels = stiSeriesLabels;
						chartWindow.Chart.SeriesLabels.Core.ApplyStyle(chartWindow.Chart.Style);
						chartWindow.UpdateSample();
						((PropertyGrid)chartWindow.PropertyGrid).SelectedObject = stiSeriesLabels;
					}
				}
				return;
			}
			num++;
		}
		if (labelsImagesSelectedIndex != -1)
		{
			buttonLabelsMouseLeave();
			labelsImagesSelectedIndex = -1;
			((Control)panelLabels).Invalidate();
		}
	}

	private void panelLabels_MouseLeave(object sender, EventArgs e)
	{
		if (labelsImagesMouseOverIndex != -1)
		{
			buttonLabelsMouseLeave();
			labelsImagesMouseOverIndex = -1;
			((Control)panelLabels).Invalidate();
		}
	}

	private void timerLabelsTick(object sender, EventArgs e)
	{
		timerLabels.Stop();
		if (lastMouseEnterSeriesLabels != null)
		{
			IStiSeriesLabels stiSeriesLabels = lastMouseEnterSeriesLabels;
			lastMouseEnterSeriesLabels = null;
			JObject jObject = chartWindow.Chart.SeriesLabels.SaveToJsonObject(StiJsonSaveMode.Report);
			chartWindow.Chart.SeriesLabelsConditions.Clear();
			stiSeriesLabels.Chart = chartWindow.Chart;
			stiSeriesLabels.LoadFromJsonObject(jObject);
			chartWindow.Chart.SeriesLabels = stiSeriesLabels;
			chartWindow.Chart.SeriesLabels.Core.ApplyStyle(chartWindow.Chart.Style);
			chartWindow.UpdateSample();
		}
	}

	private void buttonLabelsMouseLeave()
	{
		Timer obj = timerLabels;
		if (obj != null)
		{
			obj.Stop();
		}
		Timer obj2 = timerLabels;
		if (obj2 != null)
		{
			obj2.Start();
		}
	}

	private void buttonLabelsMouseEnter(StiLabelInfo info)
	{
		timerLabels.Stop();
		if (lastMouseEnterSeriesLabels == null)
		{
			lastMouseEnterSeriesLabels = chartWindow.Chart.SeriesLabels;
		}
		JObject jObject = lastMouseEnterSeriesLabels.SaveToJsonObject(StiJsonSaveMode.Report);
		StiSeriesLabels stiSeriesLabels = (StiSeriesLabels)info.SeriesLabels.Clone();
		chartWindow.Chart.SeriesLabelsConditions.Clear();
		stiSeriesLabels.Chart = chartWindow.Chart;
		stiSeriesLabels.LoadFromJsonObject(jObject);
		chartWindow.Chart.SeriesLabels = stiSeriesLabels;
		chartWindow.Chart.SeriesLabels.Core.ApplyStyle(chartWindow.Chart.Style);
		chartWindow.UpdateSample();
	}

	private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
	{
		CheckPropertyGridValue();
		if (TabControl.SelectedTab == pageConditions)
		{
			if (conditionsCollectionControl == null)
			{
				StiCloudChartConditionsCollectionControl stiCloudChartConditionsCollectionControl = new StiCloudChartConditionsCollectionControl();
				((Control)stiCloudChartConditionsCollectionControl).Dock = (DockStyle)5;
				conditionsCollectionControl = stiCloudChartConditionsCollectionControl;
				((Control)pageConditionsContent).Controls.Add((Control)(object)conditionsCollectionControl);
			}
			if (lastSeriesLabels == null)
			{
				lastSeriesLabels = chartWindow.Chart.SeriesLabels;
				StiSeriesLabels stiSeriesLabels = (StiSeriesLabels)chartWindow.Chart.SeriesLabels;
				StiSeries series = ((chartWindow.Chart.Series.Count > 0) ? ((StiSeries)chartWindow.Chart.Series[0]) : null);
				conditionsCollectionControl.Init(stiSeriesLabels.Conditions, series);
			}
		}
		else if (TabControl.SelectedTab == pageCommon)
		{
			StiChartConditionsCollection conditions = conditionsCollectionControl.GetConditions();
			if (conditions != null && lastMouseEnterSeriesLabels != null)
			{
				((StiSeriesLabels)lastMouseEnterSeriesLabels).Conditions = conditions;
			}
		}
		UpdateSamplePosition();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Expected O, but got Unknown
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Expected O, but got Unknown
		//IL_031e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Expected O, but got Unknown
		//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03af: Expected O, but got Unknown
		//IL_03bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c6: Expected O, but got Unknown
		//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f4: Expected O, but got Unknown
		//IL_0427: Unknown result type (might be due to invalid IL or missing references)
		//IL_0431: Expected O, but got Unknown
		//IL_0466: Unknown result type (might be due to invalid IL or missing references)
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiCloudLabelsControl));
		TabControl = new StiCloudTabulator();
		pageCommon = new StiCloudTabulatorPage();
		pageCommonLeftPanel = new Panel();
		panelLabels = new StiMouseScrollableControl();
		pageConditions = new StiCloudTabulatorPage();
		pageConditionsContent = new Panel();
		((Control)TabControl).SuspendLayout();
		((Control)pageCommon).SuspendLayout();
		((Control)pageCommonLeftPanel).SuspendLayout();
		((Control)pageConditions).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)TabControl).Controls.Add((Control)(object)pageCommon);
		((Control)TabControl).Controls.Add((Control)(object)pageConditions);
		((Control)TabControl).Dock = (DockStyle)5;
		((Control)TabControl).Location = new Point(0, 0);
		((Control)TabControl).Margin = new Padding(0, 3, 3, 3);
		((Control)TabControl).Name = "TabControl";
		((Control)TabControl).Padding = new Padding(0, 0, 119, 0);
		TabControl.ScrollHeight = 0;
		TabControl.ScrollWidth = 100;
		((Control)TabControl).Size = new Size(600, 400);
		((Control)TabControl).TabIndex = 59;
		TabControl.TitleBackColor = SystemColors.Control;
		TabControl.TitleInLine = false;
		TabControl.TitlePosition = StiTabTitlePosition.RightHorizontal;
		TabControl.TitleWidth = 110;
		TabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;
		((Control)pageCommon).Controls.Add((Control)(object)pageCommonLeftPanel);
		pageCommon.Dock = (DockStyle)5;
		pageCommon.Image = (Image)componentResourceManager.GetObject("pageCommon.Image");
		pageCommon.InvisibleVisible = false;
		((Control)pageCommon).Location = new Point(0, 0);
		((Control)pageCommon).Name = "pageCommon";
		((Control)pageCommon).Padding = new Padding(0, 0, 1, 0);
		((Control)pageCommon).Size = new Size(481, 400);
		((Control)pageCommon).TabIndex = 0;
		((Control)pageCommon).Text = "Common";
		((Control)pageCommon).Visible = false;
		((Control)pageCommonLeftPanel).Anchor = (AnchorStyles)7;
		((Control)pageCommonLeftPanel).BackColor = Color.White;
		((Control)pageCommonLeftPanel).Controls.Add((Control)(object)panelLabels);
		((Control)pageCommonLeftPanel).Location = new Point(1, 0);
		((Control)pageCommonLeftPanel).Name = "pageCommonLeftPanel";
		((Control)pageCommonLeftPanel).Size = new Size(205, 400);
		((Control)pageCommonLeftPanel).TabIndex = 4;
		((Control)pageCommonLeftPanel).Paint += new PaintEventHandler(Panels_Paint);
		((Control)panelLabels).Anchor = (AnchorStyles)15;
		((ScrollableControl)panelLabels).AutoScroll = true;
		((Control)panelLabels).BackColor = Color.White;
		((Control)panelLabels).Font = new Font("Arial", 8f);
		((Control)panelLabels).ForeColor = Color.Black;
		((Control)panelLabels).Location = new Point(0, 0);
		((Control)panelLabels).Name = "panelLabels";
		panelLabels.ScrollHeight = 0;
		panelLabels.ScrollWidth = 0;
		((Control)panelLabels).Size = new Size(200, 400);
		((Control)panelLabels).TabIndex = 0;
		((Control)panelLabels).Paint += new PaintEventHandler(panelSeriesLabels_Paint);
		((Control)panelLabels).MouseClick += new MouseEventHandler(panelSeriesLabels_MouseDown);
		((Control)panelLabels).MouseLeave += panelLabels_MouseLeave;
		((Control)panelLabels).MouseMove += new MouseEventHandler(panelLabels_MouseMove);
		((Control)pageConditions).Controls.Add((Control)(object)pageConditionsContent);
		pageConditions.Dock = (DockStyle)5;
		pageConditions.Image = (Image)componentResourceManager.GetObject("pageConditions.Image");
		pageConditions.InvisibleVisible = true;
		((Control)pageConditions).Location = new Point(0, 0);
		((Control)pageConditions).Name = "pageConditions";
		((Control)pageConditions).Padding = new Padding(4);
		((Control)pageConditions).Size = new Size(481, 400);
		((Control)pageConditions).TabIndex = 3;
		((Control)pageConditions).Text = "Conditions";
		((Control)pageConditions).Visible = false;
		((Control)pageConditionsContent).BackColor = Color.White;
		((Control)pageConditionsContent).Dock = (DockStyle)5;
		((Control)pageConditionsContent).Location = new Point(4, 4);
		((Control)pageConditionsContent).Name = "pageConditionsContent";
		((Control)pageConditionsContent).Size = new Size(473, 392);
		((Control)pageConditionsContent).TabIndex = 0;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)TabControl);
		((Control)this).Name = "StiCloudLabelsControl";
		((Control)this).Size = new Size(600, 400);
		((Control)TabControl).ResumeLayout(false);
		((Control)pageCommon).ResumeLayout(false);
		((Control)pageCommonLeftPanel).ResumeLayout(false);
		((Control)pageConditions).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
