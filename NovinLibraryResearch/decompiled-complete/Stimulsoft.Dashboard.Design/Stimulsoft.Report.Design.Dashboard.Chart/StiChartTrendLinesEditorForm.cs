using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Dashboard.Components.Chart;
using Stimulsoft.Dashboard.Design.Editors.Chart;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Design.Images;

namespace Stimulsoft.Report.Design.Dashboard.Chart;

public class StiChartTrendLinesEditorForm : Form
{
	private List<StiChartTrendLine> cloneTrendLines;

	private StiChartElement chartElement;

	private IContainer components;

	private StiGroupLine stiGroupLine1;

	private Button buttonCancel;

	private Button buttonOk;

	private StiZoneContainerControl zoneContainerTrendLines;

	private Button buttonAdd;

	public List<StiChartTrendLine> TrendLines => cloneTrendLines;

	public StiChartTrendLinesEditorForm(IStiChartElement chartElement)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			this.chartElement = (StiChartElement)chartElement;
			cloneTrendLines = this.chartElement.TrendLines.ConvertAll((StiChartTrendLine item) => (StiChartTrendLine)item.Clone());
			Localize();
			Build();
			((Form)this).ShowIcon = false;
			zoneContainerTrendLines.EmptyImage = StiReportDesignImages.Tasks.ChartTrendLine(StiImageSize.Double);
			zoneContainerTrendLines.EmptyText = StiLocalization.Get("PropertyMain", "NoElements");
		}
	}

	private void Build()
	{
		if (cloneTrendLines == null)
		{
			return;
		}
		foreach (StiChartTrendLine cloneTrendLine in cloneTrendLines)
		{
			StiChartTrendLinesZoneControl stiChartTrendLinesZoneControl = new StiChartTrendLinesZoneControl(chartElement, cloneTrendLine);
			((Control)zoneContainerTrendLines).Controls.Add((Control)(object)stiChartTrendLinesZoneControl);
			((Control)zoneContainerTrendLines).Controls.SetChildIndex((Control)(object)stiChartTrendLinesZoneControl, 0);
		}
		UpdateStateGroupLines();
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("Chart", "TrendLinesEditorForm");
		((Control)buttonOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)buttonCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		((Control)buttonAdd).Text = StiLocalization.Get("Buttons", "Add");
	}

	private void FillTrendLines()
	{
		cloneTrendLines.Clear();
		for (int num = ((ArrangedElementCollection)((Control)zoneContainerTrendLines).Controls).Count - 1; num >= 0; num--)
		{
			StiChartTrendLinesZoneControl stiChartTrendLinesZoneControl = ((Control)zoneContainerTrendLines).Controls[num] as StiChartTrendLinesZoneControl;
			cloneTrendLines.Add(stiChartTrendLinesZoneControl.GetTrendLine());
		}
	}

	private void UpdateStateGroupLines()
	{
		foreach (StiChartTrendLinesZoneControl item in (ArrangedElementCollection)((Control)zoneContainerTrendLines).Controls)
		{
			((Control)item.groupLine).Visible = ((ArrangedElementCollection)((Control)zoneContainerTrendLines).Controls).Count > 1;
		}
	}

	private void ButtonAddColor_Click(object sender, EventArgs e)
	{
		StiChartTrendLinesZoneControl stiChartTrendLinesZoneControl = new StiChartTrendLinesZoneControl(chartElement, new StiChartTrendLine());
		((Control)zoneContainerTrendLines).Controls.Add((Control)(object)stiChartTrendLinesZoneControl);
		UpdateStateGroupLines();
	}

	private void This_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 27)
		{
			e.Handled = true;
			((Form)this).Close();
		}
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		FillTrendLines();
		((Form)this).DialogResult = (DialogResult)1;
	}

	private void This_CancelClick(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Form)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_0428: Unknown result type (might be due to invalid IL or missing references)
		//IL_0432: Expected O, but got Unknown
		stiGroupLine1 = new StiGroupLine();
		buttonCancel = new Button();
		buttonOk = new Button();
		zoneContainerTrendLines = new StiZoneContainerControl();
		buttonAdd = new Button();
		((Control)stiGroupLine1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)stiGroupLine1).BackColor = Color.White;
		((Control)stiGroupLine1).Controls.Add((Control)(object)buttonCancel);
		((Control)stiGroupLine1).Controls.Add((Control)(object)buttonOk);
		((Control)stiGroupLine1).Dock = (DockStyle)2;
		stiGroupLine1.LinePosition = StiLinePosition.Top;
		stiGroupLine1.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine1).Location = new Point(0, 424);
		((Control)stiGroupLine1).Name = "stiGroupLine1";
		((Control)stiGroupLine1).Size = new Size(306, 46);
		((Control)stiGroupLine1).TabIndex = 68;
		((Control)buttonCancel).Anchor = (AnchorStyles)10;
		buttonCancel.DialogResult = (DialogResult)2;
		((Control)buttonCancel).Location = new Point(219, 12);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(75, 24);
		((Control)buttonCancel).TabIndex = 63;
		((Control)buttonCancel).Text = "Cancel";
		((ButtonBase)buttonCancel).UseVisualStyleBackColor = true;
		((Control)buttonCancel).Click += This_CancelClick;
		((Control)buttonOk).Anchor = (AnchorStyles)10;
		((Control)buttonOk).Location = new Point(138, 12);
		((Control)buttonOk).Name = "buttonOk";
		((Control)buttonOk).Size = new Size(75, 24);
		((Control)buttonOk).TabIndex = 62;
		((Control)buttonOk).Text = "Ok";
		((ButtonBase)buttonOk).UseVisualStyleBackColor = true;
		((Control)buttonOk).Click += This_OkClick;
		((Control)zoneContainerTrendLines).AllowDrop = true;
		((Control)zoneContainerTrendLines).Anchor = (AnchorStyles)15;
		((ScrollableControl)zoneContainerTrendLines).AutoScroll = true;
		((Control)zoneContainerTrendLines).BackColor = Color.White;
		((Control)zoneContainerTrendLines).Location = new Point(12, 49);
		((Control)zoneContainerTrendLines).Margin = new Padding(2, 2, 2, 2);
		((Control)zoneContainerTrendLines).Name = "zoneContainerTrendLines";
		((Control)zoneContainerTrendLines).Padding = new Padding(2);
		zoneContainerTrendLines.ShowBorder = true;
		((Control)zoneContainerTrendLines).Size = new Size(283, 364);
		((Control)zoneContainerTrendLines).TabIndex = 69;
		((Control)buttonAdd).Anchor = (AnchorStyles)10;
		((Control)buttonAdd).Location = new Point(12, 12);
		((Control)buttonAdd).Name = "buttonAdd";
		((Control)buttonAdd).Size = new Size(100, 24);
		((Control)buttonAdd).TabIndex = 75;
		((Control)buttonAdd).Text = "Add";
		((ButtonBase)buttonAdd).UseVisualStyleBackColor = true;
		((Control)buttonAdd).Click += ButtonAddColor_Click;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(306, 470);
		((Control)this).Controls.Add((Control)(object)buttonAdd);
		((Control)this).Controls.Add((Control)(object)zoneContainerTrendLines);
		((Control)this).Controls.Add((Control)(object)stiGroupLine1);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(295, 391);
		((Control)this).Name = "StiChartTrendLinesEditorForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Constants Lines";
		((Control)this).KeyDown += new KeyEventHandler(This_KeyDown);
		((Control)stiGroupLine1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
