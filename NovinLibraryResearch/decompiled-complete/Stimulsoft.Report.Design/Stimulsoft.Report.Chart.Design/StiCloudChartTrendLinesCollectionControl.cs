using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Design.Images;

namespace Stimulsoft.Report.Chart.Design;

[ToolboxItem(false)]
public class StiCloudChartTrendLinesCollectionControl : UserControl
{
	private StiTrendLinesCollection trendLines;

	private IContainer components;

	private StiZoneContainerControl panelTrendLines;

	private Button buttonAdd;

	private StiToolBar toolBar;

	public StiCloudChartTrendLinesCollectionControl()
	{
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			Localize();
			panelTrendLines.EmptyImage = StiReportDesignImages.Tasks.ChartTrendLine(StiImageSize.Double, allowCache: false);
			panelTrendLines.EmptyText = StiLocalization.Get("PropertyMain", "NoElements");
		}
	}

	public void Localize()
	{
		((Control)buttonAdd).Text = StiLocalization.Get("Chart", "AddTrendLine");
		foreach (StiChartTrendLinesZoneControl item in (ArrangedElementCollection)((Control)panelTrendLines).Controls)
		{
			item.Localize();
		}
	}

	internal void Init(StiTrendLinesCollection trendLines, StiSeries series)
	{
		this.trendLines = trendLines;
		Build();
	}

	internal void Clean()
	{
		((Control)panelTrendLines).Controls.Clear();
	}

	private void AddLevel(StiTrendLine trendLine)
	{
		((Control)panelTrendLines).Controls.Add((Control)(object)new StiChartTrendLinesZoneControl(trendLine));
	}

	private void Build()
	{
		((Control)panelTrendLines).Controls.Clear();
		foreach (StiTrendLine trendLine in trendLines)
		{
			AddLevel(trendLine);
		}
	}

	internal StiTrendLinesCollection GetTrendLinesCollection()
	{
		AddTrendLines();
		return trendLines;
	}

	internal void AddTrendLines()
	{
		trendLines.Clear();
		foreach (StiChartTrendLinesZoneControl item in (ArrangedElementCollection)((Control)panelTrendLines).Controls)
		{
			StiTrendLine trendLine = item.GetTrendLine();
			trendLines.Insert(0, trendLine);
		}
	}

	private void buttonAdd_Click(object sender, EventArgs e)
	{
		AddLevel(new StiTrendLineLinear());
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
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		buttonAdd = new Button();
		panelTrendLines = new StiZoneContainerControl();
		toolBar = new StiToolBar();
		((Control)toolBar).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)buttonAdd).AutoSize = true;
		((Control)buttonAdd).Location = new Point(11, 11);
		((Control)buttonAdd).Name = "buttonAdd";
		((Control)buttonAdd).Size = new Size(100, 24);
		((Control)buttonAdd).TabIndex = 76;
		((Control)buttonAdd).Text = "Add";
		((ButtonBase)buttonAdd).UseVisualStyleBackColor = true;
		((Control)buttonAdd).Click += buttonAdd_Click;
		((Control)panelTrendLines).AllowDrop = true;
		((Control)panelTrendLines).Anchor = (AnchorStyles)15;
		((ScrollableControl)panelTrendLines).AutoScroll = true;
		((Control)panelTrendLines).BackColor = Color.White;
		panelTrendLines.EmptyImage = null;
		panelTrendLines.EmptyText = null;
		((Control)panelTrendLines).Location = new Point(4, 52);
		((Control)panelTrendLines).Margin = new Padding(2, 2, 2, 2);
		((Control)panelTrendLines).Name = "panelTrendLines";
		((Control)panelTrendLines).Padding = new Padding(2);
		panelTrendLines.ShowBorder = false;
		((Control)panelTrendLines).Size = new Size(494, 346);
		((Control)panelTrendLines).TabIndex = 62;
		((Control)toolBar).Controls.Add((Control)(object)buttonAdd);
		toolBar.ControlStyle = StiControlStyle.Office2013Blue;
		((Control)toolBar).Dock = (DockStyle)1;
		toolBar.LineStyle = StiToolBar.StiToolBarLineStyle.Bottom;
		((Control)toolBar).Location = new Point(0, 0);
		((Control)toolBar).Name = "toolBar";
		((Control)toolBar).Padding = new Padding(6, 3, 3, 4);
		((Control)toolBar).Size = new Size(500, 47);
		((Control)toolBar).TabIndex = 77;
		((Control)toolBar).Text = "stiToolBar1";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Control)this).Controls.Add((Control)(object)toolBar);
		((Control)this).Controls.Add((Control)(object)panelTrendLines);
		((Control)this).Name = "StiCloudChartTrendLinesCollectionControl";
		((Control)this).Size = new Size(500, 400);
		((Control)toolBar).ResumeLayout(false);
		((Control)toolBar).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
