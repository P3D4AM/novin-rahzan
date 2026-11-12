using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Inplace;

namespace Stimulsoft.Report.Chart.Design;

public class StiChartConditionsEditorForm : StiInplaceToolForm
{
	private StiCloudChartConditionsCollectionControl conditionsControl;

	private StiChart chart;

	private StiSeries series;

	private StiSeriesLabels labels;

	private IContainer components;

	private StiGroupLine stiGroupLine1;

	private Button buttonCancel;

	private Button buttonOk;

	public override bool AllowResize => false;

	private StiReport Report => chart?.Report;

	private StiDesignerControl Designer { get; set; }

	public StiChartConditionsEditorForm(StiChart chart, StiSeries series, StiSeriesLabels labels)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			this.chart = chart;
			this.series = series;
			this.labels = labels;
			Designer = Report?.Designer?.DesignerControl as StiDesignerControl;
			Localize();
			((Form)this).ShowIcon = false;
			StiCloudChartConditionsCollectionControl stiCloudChartConditionsCollectionControl = new StiCloudChartConditionsCollectionControl();
			((Control)stiCloudChartConditionsCollectionControl).Dock = (DockStyle)5;
			conditionsControl = stiCloudChartConditionsCollectionControl;
			((Control)this).Controls.Add((Control)(object)conditionsControl);
			((Control)conditionsControl).BringToFront();
			Build();
		}
	}

	private void Build()
	{
		if (labels != null)
		{
			conditionsControl.Init(labels.Conditions, chart.Series.ToList().FirstOrDefault());
		}
		if (series != null)
		{
			conditionsControl.Init(series.Conditions, series);
		}
	}

	public override void Localize()
	{
		((Control)this).Text = StiLocalization.Get("Chart", "ChartConditionsCollectionForm");
		((Control)buttonOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)buttonCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		conditionsControl?.Localize();
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
		Designer.UndoRedoSave(Loc.Get("PropertyMain", "ConstantLines"), onlyCurrentPage: true);
		conditionsControl.AddConditions();
		((Form)this).Close();
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
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Expected O, but got Unknown
		stiGroupLine1 = new StiGroupLine();
		buttonCancel = new Button();
		buttonOk = new Button();
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
		((Control)stiGroupLine1).Size = new Size(492, 46);
		((Control)stiGroupLine1).TabIndex = 2;
		((Control)buttonCancel).Anchor = (AnchorStyles)10;
		buttonCancel.DialogResult = (DialogResult)2;
		((Control)buttonCancel).Location = new Point(405, 12);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(75, 24);
		((Control)buttonCancel).TabIndex = 1;
		((Control)buttonCancel).Text = "Cancel";
		((ButtonBase)buttonCancel).UseVisualStyleBackColor = true;
		((Control)buttonCancel).Click += This_CancelClick;
		((Control)buttonOk).Anchor = (AnchorStyles)10;
		buttonOk.DialogResult = (DialogResult)1;
		((Control)buttonOk).Location = new Point(324, 12);
		((Control)buttonOk).Name = "buttonOk";
		((Control)buttonOk).Size = new Size(75, 24);
		((Control)buttonOk).TabIndex = 0;
		((Control)buttonOk).Text = "Ok";
		((ButtonBase)buttonOk).UseVisualStyleBackColor = true;
		((Control)buttonOk).Click += This_OkClick;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((Control)this).AllowDrop = true;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(492, 470);
		((Control)this).Controls.Add((Control)(object)stiGroupLine1);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).Margin = new Padding(3, 3, 3, 3);
		((Control)this).MinimumSize = new Size(508, 375);
		((Control)this).Name = "StiChartConditionsEditorForm";
		((Control)this).Text = "Conditions";
		((Control)this).KeyDown += new KeyEventHandler(This_KeyDown);
		((Control)stiGroupLine1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
