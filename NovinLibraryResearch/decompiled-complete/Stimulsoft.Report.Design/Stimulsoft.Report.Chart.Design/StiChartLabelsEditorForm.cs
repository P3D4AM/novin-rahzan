using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Inplace;
using Stimulsoft.Report.Design.Panels;

namespace Stimulsoft.Report.Chart.Design;

public class StiChartLabelsEditorForm : StiInplaceToolForm
{
	private class StiLabelPicture : RadioButton
	{
		public StiChartLabelsEditorForm Editor { get; set; }

		public bool IsMouseOver { get; set; }

		protected override void OnPaint(PaintEventArgs e)
		{
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Expected O, but got Unknown
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Expected O, but got Unknown
			((ButtonBase)this).OnPaint(e);
			Graphics graphics = e.Graphics;
			Rectangle clientRectangle = ((Control)this).ClientRectangle;
			if (IsMouseOver)
			{
				StiDrawing.FillRectangle(graphics, Color.FromArgb(212, 224, 241), clientRectangle);
				clientRectangle.Width--;
				clientRectangle.Height--;
				if (((RadioButton)this).Checked)
				{
					StiDrawing.DrawRectangle(graphics, Color.FromArgb(43, 87, 154), clientRectangle);
				}
			}
			else if (((RadioButton)this).Checked)
			{
				StiDrawing.FillRectangle(graphics, Color.FromArgb(158, 186, 230), clientRectangle);
			}
			else
			{
				StiDrawing.FillRectangle(graphics, Brushes.White, clientRectangle);
			}
			clientRectangle = ((Control)this).ClientRectangle;
			int num = (clientRectangle.Width - ((ButtonBase)this).Image.Width / 2) / 2;
			Rectangle rectangle = new Rectangle(num, num, ((ButtonBase)this).Image.Width / 2, ((ButtonBase)this).Image.Height / 2);
			graphics.DrawImage(((ButtonBase)this).Image, rectangle);
			graphics.DrawRectangle(Pens.LightGray, rectangle);
			Rectangle rectangle2 = new Rectangle(clientRectangle.X, clientRectangle.Bottom - StiScaleUI.I(20), clientRectangle.Width, StiScaleUI.I(24));
			SolidBrush val = new SolidBrush(((Control)this).ForeColor);
			try
			{
				StringFormat val2 = new StringFormat();
				try
				{
					val2.Alignment = (StringAlignment)1;
					val2.LineAlignment = (StringAlignment)1;
					graphics.DrawString(((Control)this).Text, ((Control)this).Font, (Brush)(object)val, (RectangleF)rectangle2, val2);
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

		protected override void OnClick(EventArgs e)
		{
			((RadioButton)this).OnClick(e);
			((RadioButton)this).Checked = true;
			Editor.SelectedLabels = ((Control)this).Tag as StiSeriesLabels;
			Editor.ApplyLabels(Editor.SelectedLabels);
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			((ButtonBase)this).OnMouseEnter(e);
			IsMouseOver = true;
			StiSeriesLabels labels = (((Control)this).Tag as StiSeriesLabels).Clone() as StiSeriesLabels;
			Editor.ApplyLabels(labels);
			((Control)this).Invalidate();
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			((ButtonBase)this).OnMouseLeave(e);
			IsMouseOver = false;
			Editor.ApplyLabels(Editor.SelectedLabels);
			((Control)this).Invalidate();
		}
	}

	private IContainer components;

	private StiGroupLine stiGroupLine1;

	private Button buttonCancel;

	private Button buttonOk;

	private ScrollableControl panelItems;

	private StiChart Chart { get; set; }

	private StiSeries Series { get; set; }

	internal StiSeriesLabels Labels
	{
		get
		{
			return ((Series != null) ? Series.SeriesLabels : Chart.Labels) as StiSeriesLabels;
		}
		set
		{
			if (Series != null)
			{
				Series.SeriesLabels = value;
			}
			else
			{
				Chart.Labels = value;
			}
		}
	}

	private StiSeriesLabels SavedLabels { get; set; }

	private StiSeriesLabels SelectedLabels { get; set; }

	private StiShowSeriesLabels SavedShowSeriesLabels { get; set; }

	private StiReport Report => Chart?.Report;

	private StiDesignerControl Designer { get; set; }

	private StiPropertyGrid PropertyGrid { get; set; }

	private int ButtonWidth => ((Control)panelItems).Width - SystemInformation.VerticalScrollBarWidth - StiScaleUI.I8;

	private int ButtonHeight => ButtonWidth - StiScaleUI.I(70);

	private int ImageWidth => ButtonWidth - StiScaleUI.I12;

	private int ImageHeight => ButtonWidth - StiScaleUI.I(95);

	public StiChartLabelsEditorForm(StiChart chart, StiSeries series)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			Chart = chart;
			Series = series;
			Designer = Report?.Designer?.DesignerControl as StiDesignerControl;
			PropertyGrid = StiPropertiesPanelHelper.GetPropertyGrid(Designer);
			Build();
			Localize();
			SelectedLabels = ((series != null) ? ((StiSeriesLabels)series.SeriesLabels) : ((StiSeriesLabels)chart.SeriesLabels));
			SavedLabels = Labels;
			if (Series != null)
			{
				SavedShowSeriesLabels = Series.ShowSeriesLabels;
				series.ShowSeriesLabels = StiShowSeriesLabels.FromSeries;
			}
			((Form)this).ShowIcon = false;
		}
	}

	public override void Localize()
	{
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		((Control)this).Text = Loc.GetMain("SeriesLabels");
		((Control)buttonOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)buttonCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		foreach (RadioButton item in (ArrangedElementCollection)((Control)panelItems).Controls)
		{
			RadioButton val = item;
			((Control)val).Text = (((Control)val).Tag as StiSeriesLabels).ServiceName;
		}
	}

	private void ApplyLabels(StiSeriesLabels labels)
	{
		JObject jObject = Labels.SaveToJsonObject(StiJsonSaveMode.Report);
		Chart.SeriesLabelsConditions.Clear();
		labels.Chart = Chart;
		labels.LoadFromJsonObject(jObject);
		Labels = labels;
		Chart.Series.ApplyStyle(Chart.Style);
		UpdateSample();
	}

	internal void Build()
	{
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Expected O, but got Unknown
		StiChart stiChart = (StiChart)Chart.Clone();
		stiChart.ConstantLines.Clear();
		stiChart.Strips.Clear();
		if (stiChart.Area is StiAxisArea stiAxisArea)
		{
			stiAxisArea.XAxis.Visible = false;
			stiAxisArea.YAxis.Visible = false;
			stiAxisArea.YRightAxis.Visible = false;
			stiAxisArea.XTopAxis.Visible = false;
		}
		stiChart.Legend.Visible = false;
		stiChart.SeriesLabels.Visible = false;
		stiChart.Title.Visible = false;
		stiChart.Table.Visible = false;
		stiChart.Brush = new StiEmptyBrush();
		stiChart.Area.Brush = new StiEmptyBrush();
		AddDefaultSeries(stiChart);
		List<StiSeriesLabels> list = new List<StiSeriesLabels>();
		foreach (StiSeriesLabels item in StiOptions.Services.ChartSerieLabels.Where((StiSeriesLabels s) => s.ServiceEnabled))
		{
			if (stiChart.Area.Core.CheckInLabelsTypes(item.GetType()))
			{
				list.Add(item);
			}
		}
		int i = StiScale.I4;
		int num = StiScale.I2;
		StiLabelPicture stiLabelPicture = null;
		Type type = Labels.GetType();
		int num2 = 0;
		foreach (StiSeriesLabels item2 in list)
		{
			if (stiChart.Area.Core.CheckInLabelsTypes(item2.GetType()))
			{
				stiChart.SeriesLabels = (StiSeriesLabels)item2.Clone();
				stiChart.Core.ApplyStyle(stiChart.Style);
				RectangleF rect = new RectangleF(0f, 0f, ImageWidth * 2, ImageHeight * 2);
				Bitmap val = new Bitmap(ImageWidth * 2, ImageHeight * 2);
				Graphics val2 = Graphics.FromImage((Image)(object)val);
				try
				{
					double zoom = stiChart.Report.Info.Zoom;
					stiChart.Report.Info.Zoom = 1.0;
					stiChart.DrawChart(val2, rect, useMargins: false, useBackground: false);
					stiChart.Report.Info.Zoom = zoom;
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
				StiLabelPicture obj = new StiLabelPicture
				{
					Editor = this
				};
				((Control)obj).Tag = item2;
				((Control)obj).Text = item2.ServiceName;
				((Control)obj).Left = i;
				((Control)obj).Top = num;
				((Control)obj).Width = ButtonWidth;
				((Control)obj).Height = ButtonHeight;
				((ButtonBase)obj).Image = (Image)(object)val;
				StiLabelPicture stiLabelPicture2 = obj;
				((Control)panelItems).Controls.Add((Control)(object)stiLabelPicture2);
				if (type == item2.GetType())
				{
					stiLabelPicture = stiLabelPicture2;
				}
				num += ButtonHeight + StiScaleUI.I4;
				num2++;
			}
		}
		if (stiLabelPicture != null)
		{
			((RadioButton)stiLabelPicture).Checked = true;
		}
		((Control)panelItems).Invalidate();
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

	private void UpdateSample()
	{
		Report.Designer.Invalidate(Chart);
	}

	private void PanelItems_MouseLeave(object sender, EventArgs e)
	{
		ApplyLabels(SelectedLabels);
	}

	protected override void OnFormClosed(FormClosedEventArgs e)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Invalid comparison between Unknown and I4
		base.OnFormClosed(e);
		lock (StiInplaceEditorForm.FormClosingObject)
		{
			if (!base.IsFormClosed)
			{
				base.IsFormClosed = true;
				if ((int)((Form)this).DialogResult != 1)
				{
					Labels = SavedLabels;
				}
				if (Series != null)
				{
					Series.ShowSeriesLabels = SavedShowSeriesLabels;
				}
				((PropertyGrid)PropertyGrid).SelectedObject = ((object)Series) ?? ((object)Chart);
			}
		}
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
		Labels = SavedLabels;
		Designer.UndoRedoSave(Loc.GetMain("Labels"), onlyCurrentPage: true);
		Labels = SelectedLabels;
		((Form)this).Close();
	}

	private void This_CancelClick(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void This_Load(object sender, EventArgs e)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		Timer timer = new Timer
		{
			Interval = 200
		};
		timer.Tick += delegate
		{
			timer.Stop();
			((Component)(object)timer).Dispose();
			Control val = ((IEnumerable)((Control)panelItems).Controls).Cast<Control>().FirstOrDefault((Func<Control, bool>)((Control c) => c is StiLabelPicture && ((RadioButton)(StiLabelPicture)(object)c).Checked));
			if (val != null)
			{
				panelItems.ScrollControlIntoView(val);
			}
		};
		timer.Start();
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
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0315: Unknown result type (might be due to invalid IL or missing references)
		//IL_0364: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Expected O, but got Unknown
		buttonOk = new Button();
		buttonCancel = new Button();
		stiGroupLine1 = new StiGroupLine();
		panelItems = new ScrollableControl();
		((Control)stiGroupLine1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)buttonOk).Anchor = (AnchorStyles)10;
		buttonOk.DialogResult = (DialogResult)1;
		((Control)buttonOk).Location = new Point(57, 12);
		((Control)buttonOk).Name = "buttonOk";
		((Control)buttonOk).Size = new Size(75, 24);
		((Control)buttonOk).TabIndex = 0;
		((Control)buttonOk).Text = "Ok";
		((ButtonBase)buttonOk).UseVisualStyleBackColor = true;
		((Control)buttonOk).Click += This_OkClick;
		((Control)buttonCancel).Anchor = (AnchorStyles)10;
		buttonCancel.DialogResult = (DialogResult)2;
		((Control)buttonCancel).Location = new Point(138, 12);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(75, 24);
		((Control)buttonCancel).TabIndex = 1;
		((Control)buttonCancel).Text = "Cancel";
		((ButtonBase)buttonCancel).UseVisualStyleBackColor = true;
		((Control)buttonCancel).Click += This_CancelClick;
		((Control)stiGroupLine1).BackColor = Color.White;
		((Control)stiGroupLine1).Controls.Add((Control)(object)buttonCancel);
		((Control)stiGroupLine1).Controls.Add((Control)(object)buttonOk);
		((Control)stiGroupLine1).Dock = (DockStyle)2;
		stiGroupLine1.LinePosition = StiLinePosition.Top;
		stiGroupLine1.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine1).Location = new Point(0, 399);
		((Control)stiGroupLine1).Name = "stiGroupLine1";
		((Control)stiGroupLine1).Size = new Size(225, 46);
		((Control)stiGroupLine1).TabIndex = 2;
		((Control)panelItems).AllowDrop = true;
		((Control)panelItems).Anchor = (AnchorStyles)15;
		panelItems.AutoScroll = true;
		((Control)panelItems).Location = new Point(12, 13);
		((Control)panelItems).Margin = new Padding(3, 3, 3, 0);
		((Control)panelItems).Name = "panelItems";
		((Control)panelItems).Size = new Size(200, 395);
		((Control)panelItems).TabIndex = 1;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((Control)this).AllowDrop = true;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(225, 445);
		((Control)this).Controls.Add((Control)(object)panelItems);
		((Control)this).Controls.Add((Control)(object)stiGroupLine1);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).Margin = new Padding(3, 3, 3, 3);
		((Control)this).MinimumSize = new Size(211, 371);
		((Control)this).Name = "StiChartLabelsEditorForm";
		((Control)this).Text = "Labels";
		((Form)this).Load += This_Load;
		((Control)this).KeyDown += new KeyEventHandler(This_KeyDown);
		((Control)stiGroupLine1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
