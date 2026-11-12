using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Inplace;
using Stimulsoft.Report.Design.Panels;
using Stimulsoft.Report.Help;

namespace Stimulsoft.Report.Chart.Design;

public class StiCloudChartForm : StiInplaceToolForm
{
	private StiCloudChartControl chartControl;

	private StiCloudSeriesControl seriesControl;

	private StiCloudLabelsControl labelsControl;

	private StiCloudStylesControl stylesControl;

	private StiCloudAreaControl areaControl;

	private Bitmap sampleBitmap;

	private IContainer components;

	private StiGroupLine stiGroupLine1;

	private Button btOk;

	private Button btCancel;

	private StiTabulator tabControl;

	private StiTabulatorPage pageChart;

	private StiTabulatorPage pageSeries;

	private StiTabulatorPage pageArea;

	private StiTabulatorPage pageLabels;

	private StiTabulatorPage pageStyles;

	private Panel pageChartContent;

	private Panel pageSeriesContent;

	private Panel pageAreaContent;

	private Panel pageLabelsContent;

	private Panel pageStylesContent;

	private PictureBox chartSampleImage;

	private Button buttonRunChartWizard;

	private Button buttonLessOptions;

	internal StiChart Chart { get; set; }

	public override bool AllowResize { get; }

	internal StiPropertyGrid PropertyGrid { get; set; }

	internal StiDesignerControl Designer { get; set; }

	public StiCloudChartForm()
		: this(null)
	{
	}

	public StiCloudChartForm(StiChart chart)
	{
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected O, but got Unknown
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			if (StiOptions.Appearance.ShowFormsHelp)
			{
				((Form)this).HelpButton = true;
			}
			Chart = chart;
			Designer = chart?.Report?.Designer?.DesignerControl as StiDesignerControl;
			((Form)this).ShowIcon = false;
			Localize();
			PropertyGrid = StiPropertiesPanelHelper.GetPropertyGrid(Designer);
			StiPropertiesPanelHelper.ActivatePanel(Designer, null);
			((PropertyGrid)PropertyGrid).PropertyValueChanged += new PropertyValueChangedEventHandler(PropertyGrid_PropertyValueChanged);
			UpdateSample();
			UpdateSamplePosition(0, 0, 2, 2, showRightSeparator: true, showSample: true);
			tabControl.SelectedIndex = 1;
			TabControl_SelectedIndexChanged(null, null);
			((Control)buttonRunChartWizard).Visible = StiOptions.Designer.Editors.ChartEditor.ShowRunChartWizard;
		}
	}

	public override void Localize()
	{
		((Control)this).Text = StiLocalization.Get("Chart", "ChartEditorForm");
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok").Replace("&", string.Empty);
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel").Replace("&", string.Empty);
		((Control)buttonLessOptions).Text = StiLocalization.Get("Buttons", "LessOptions");
		((Control)buttonRunChartWizard).Text = StiLocalization.Get("Chart", "RunChartWizard").Replace("&", string.Empty);
		((Control)buttonLessOptions).Width = TextRenderer.MeasureText(((Control)buttonLessOptions).Text, ((Control)buttonLessOptions).Font).Width + StiScale.I16;
		((Control)buttonRunChartWizard).Width = TextRenderer.MeasureText(((Control)buttonRunChartWizard).Text, ((Control)buttonRunChartWizard).Font).Width + StiScale.I16;
		((Control)buttonRunChartWizard).Left = ((Control)buttonLessOptions).Right + StiScale.I6;
		((Control)pageChart).Text = StiLocalization.Get("Components", "StiChart");
		((Control)pageSeries).Text = StiLocalization.Get("PropertyMain", "Series");
		((Control)pageLabels).Text = StiLocalization.Get("Chart", "Labels");
		((Control)pageArea).Text = StiLocalization.Get("Chart", "Area");
		((Control)pageStyles).Text = StiLocalization.Get("PropertyMain", "Styles");
		((Control)buttonRunChartWizard).Width = TextRenderer.MeasureText(((Control)buttonRunChartWizard).Text, ((Control)buttonRunChartWizard).Font).Width + StiScale.I(20);
	}

	public override void CalculateWindowPosition()
	{
	}

	protected override void CalculateWindowHeight()
	{
	}

	internal void UpdateSample()
	{
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Expected O, but got Unknown
		try
		{
			if (chartSampleImage != null)
			{
				chartSampleImage.Image = null;
			}
			Bitmap obj = sampleBitmap;
			if (obj != null)
			{
				((Image)obj).Dispose();
			}
			sampleBitmap = null;
			RectangleF rect = new RectangleF(0f, 0f, StiScale.I(380), StiScale.I(380));
			sampleBitmap = new Bitmap(StiScale.I(380), StiScale.I(380));
			Graphics val = Graphics.FromImage((Image)(object)sampleBitmap);
			try
			{
				val.Clear(Color.White);
				if (Chart?.Report?.Info != null)
				{
					double zoom = Chart.Report.Info.Zoom;
					Chart.Report.Info.Zoom = 1.0;
					Chart.DrawChart(val, rect, useMargins: true, useBackground: false);
					Chart.Report.Info.Zoom = zoom;
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			chartSampleImage.Image = (Image)(object)sampleBitmap;
		}
		catch
		{
			if (chartSampleImage != null)
			{
				chartSampleImage.Image = null;
			}
		}
	}

	internal void UpdateSamplePosition(int row, int col, int rowSpan, int colSpan, bool showRightSeparator, bool showSample, bool skipFooter = false, int marginTop = 0)
	{
		int[] array = new int[4]
		{
			StiScale.I(200),
			((Control)tabControl).Width - StiScale.I(326),
			StiScale.I1,
			StiScale.I(95)
		};
		int[] array2 = new int[2]
		{
			StiScale.I(44),
			((Control)tabControl).Height - StiScale.I(30)
		};
		int num = StiScale.I4;
		int num2 = 0;
		if (row > 0)
		{
			for (int i = 0; i < row; i++)
			{
				num2 += array2[i];
			}
		}
		if (col > 0)
		{
			for (int j = 0; j < col; j++)
			{
				num += array[j];
			}
		}
		int num3 = array2[row];
		for (int k = row + 1; k < row + rowSpan; k++)
		{
			num3 += array2[k];
		}
		if (skipFooter)
		{
			num3 -= StiScale.I(80);
		}
		int num4 = array[col];
		for (int l = col + 1; l < col + colSpan; l++)
		{
			num4 += array[l];
		}
		((Control)chartSampleImage).Left = num + (num4 - StiScale.I(380)) / 2;
		((Control)chartSampleImage).Top = num2 + (num3 - StiScale.I(380)) / 2 + marginTop;
		((Control)chartSampleImage).Visible = showSample;
	}

	private void This_RunChartWizardClick(object sender, EventArgs e)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Invalid comparison between Unknown and I4
		StiChartEditorForm stiChartEditorForm = new StiChartEditorForm(Chart);
		try
		{
			if (Designer != null && Chart.Report.Designer.DesignerControl != null)
			{
				Designer?.DesignerControl.AddOwnedForm((Form)(object)stiChartEditorForm);
			}
			if ((int)((Form)stiChartEditorForm).ShowDialog() == 1)
			{
				((Form)this).DialogResult = (DialogResult)1;
			}
		}
		finally
		{
			((IDisposable)stiChartEditorForm)?.Dispose();
		}
	}

	private void This_LessOptionsClick(object sender, EventArgs e)
	{
		Chart.EditorType = StiChartEditorType.Simple;
		((Form)this).DialogResult = (DialogResult)1;
		This_OkClick(sender, e);
		new StiChartDesigner(Designer).OnDoubleClick(Chart);
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/index.html?reports_internals_charts_editor.htm");
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		if (seriesControl != null)
		{
			seriesControl.SaveFilters();
			seriesControl.SaveConditions();
			seriesControl.SaveTrendLines();
		}
		if (labelsControl != null)
		{
			labelsControl.Save();
		}
		((Form)this).Close();
	}

	private void This_CancelClick(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void This_FormClosed(object sender, FormClosedEventArgs e)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Invalid comparison between Unknown and I4
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Expected O, but got Unknown
		lock (StiInplaceEditorForm.FormClosingObject)
		{
			if (!base.IsFormClosed)
			{
				base.IsFormClosed = true;
				((PropertyGrid)PropertyGrid).SelectedObject = Chart;
				if ((int)((Form)this).DialogResult != 1)
				{
					StiActions.Clipboard.InvokeUndo(Designer, closeAllInplaceDesigners: false);
					Designer.UndoRedo.RedoStack.Clear();
					((Control)Designer).Refresh();
				}
				else
				{
					seriesControl?.Collect();
					labelsControl?.Collect();
					stylesControl?.Collect();
				}
				((PropertyGrid)PropertyGrid).PropertyValueChanged -= new PropertyValueChangedEventHandler(PropertyGrid_PropertyValueChanged);
			}
		}
	}

	private void This_Load(object sender, EventArgs e)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		Timer timer = new Timer
		{
			Interval = 300
		};
		timer.Tick += delegate
		{
			timer.Stop();
			((Component)(object)timer).Dispose();
			StiPropertyGrid propertyGrid = PropertyGrid;
			TreeNode selectedNode = ((TreeView)seriesControl.tvChartSeries).SelectedNode;
			((PropertyGrid)propertyGrid).SelectedObject = ((selectedNode != null) ? selectedNode.Tag : null);
		};
		timer.Start();
	}

	private void PropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
	{
		UpdateSample();
		switch (e.ChangedItem.PropertyDescriptor.Name)
		{
		case "AllowApplyStyle":
		case "AllowApplyBrushNegative":
		case "AllowApplyColorNegative":
		case "AllowApplyBorderColor":
		case "AllowApplyBrush":
		case "AllowApplyLineColor":
		{
			object selectedObject = ((PropertyGrid)PropertyGrid).SelectedObject;
			((PropertyGrid)PropertyGrid).SelectedObject = null;
			((PropertyGrid)PropertyGrid).SelectedObject = selectedObject;
			break;
		}
		}
	}

	private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl.SelectedTab == pageChart)
		{
			if (chartControl == null)
			{
				StiCloudChartControl stiCloudChartControl = new StiCloudChartControl(this);
				((Control)stiCloudChartControl).Dock = (DockStyle)5;
				chartControl = stiCloudChartControl;
				((Control)pageChartContent).Controls.Add((Control)(object)chartControl);
				UpdateSamplePosition(0, 0, 2, 2, showRightSeparator: true, showSample: true);
			}
			else
			{
				chartControl.CheckSamplePosition();
			}
			chartControl.CheckStateTabItem();
		}
		else if (tabControl.SelectedTab == pageSeries)
		{
			if (seriesControl == null)
			{
				StiCloudSeriesControl stiCloudSeriesControl = new StiCloudSeriesControl(this);
				((Control)stiCloudSeriesControl).Dock = (DockStyle)5;
				seriesControl = stiCloudSeriesControl;
				((Control)pageSeriesContent).Controls.Add((Control)(object)seriesControl);
			}
			else
			{
				seriesControl.CheckSamplePosition();
			}
			((Control)seriesControl.tvChartSeries).Focus();
		}
		else if (tabControl.SelectedTab == pageArea)
		{
			if (areaControl == null)
			{
				StiCloudAreaControl stiCloudAreaControl = new StiCloudAreaControl(this);
				((Control)stiCloudAreaControl).Dock = (DockStyle)5;
				areaControl = stiCloudAreaControl;
				((Control)pageAreaContent).Controls.Add((Control)(object)areaControl);
			}
			else
			{
				areaControl.BuildArea();
				areaControl.CheckPropertyGridValue();
			}
			UpdateSamplePosition(0, 0, 2, 2, showRightSeparator: true, showSample: true);
		}
		else if (tabControl.SelectedTab == pageLabels)
		{
			if (labelsControl == null)
			{
				UpdateSamplePosition(0, 1, 2, 1, showRightSeparator: true, showSample: true);
				StiCloudLabelsControl stiCloudLabelsControl = new StiCloudLabelsControl(this);
				((Control)stiCloudLabelsControl).Dock = (DockStyle)5;
				labelsControl = stiCloudLabelsControl;
				((Control)pageLabelsContent).Controls.Add((Control)(object)labelsControl);
			}
			else
			{
				labelsControl.UpdateSamplePosition();
				labelsControl.BuildLabels();
			}
			labelsControl.CheckPropertyGridValue();
		}
		else if (tabControl.SelectedTab == pageStyles)
		{
			UpdateSamplePosition(1, 1, 1, 3, showRightSeparator: false, showSample: true);
			if (stylesControl == null)
			{
				StiCloudStylesControl stiCloudStylesControl = new StiCloudStylesControl(this);
				((Control)stiCloudStylesControl).Dock = (DockStyle)5;
				stylesControl = stiCloudStylesControl;
				((Control)pageStylesContent).Controls.Add((Control)(object)stylesControl);
			}
			else
			{
				stylesControl.BuildStyles();
			}
			stylesControl.CheckPropertyGridValue();
		}
		else
		{
			UpdateSamplePosition(0, 0, 2, 2, showRightSeparator: true, showSample: true);
		}
	}

	private void ChartSampleImage_Paint(object sender, PaintEventArgs e)
	{
		e.Graphics.DrawRectangle(Pens.LightGray, 0, 0, ((Control)chartSampleImage).Width - 1, ((Control)chartSampleImage).Height - 1);
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
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_039a: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06be: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a90: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a9a: Expected O, but got Unknown
		//IL_0b23: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b75: Expected O, but got Unknown
		btOk = new Button();
		btCancel = new Button();
		tabControl = new StiTabulator();
		pageChart = new StiTabulatorPage();
		pageChartContent = new Panel();
		pageSeries = new StiTabulatorPage();
		pageSeriesContent = new Panel();
		pageArea = new StiTabulatorPage();
		pageAreaContent = new Panel();
		pageLabels = new StiTabulatorPage();
		pageLabelsContent = new Panel();
		pageStyles = new StiTabulatorPage();
		pageStylesContent = new Panel();
		stiGroupLine1 = new StiGroupLine();
		buttonLessOptions = new Button();
		buttonRunChartWizard = new Button();
		chartSampleImage = new PictureBox();
		((Control)tabControl).SuspendLayout();
		((Control)pageChart).SuspendLayout();
		((Control)pageSeries).SuspendLayout();
		((Control)pageArea).SuspendLayout();
		((Control)pageLabels).SuspendLayout();
		((Control)pageStyles).SuspendLayout();
		((Control)stiGroupLine1).SuspendLayout();
		((ISupportInitialize)chartSampleImage).BeginInit();
		((Control)this).SuspendLayout();
		((Control)btOk).Anchor = (AnchorStyles)10;
		btOk.DialogResult = (DialogResult)1;
		((Control)btOk).Location = new Point(619, 12);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 24);
		((Control)btOk).TabIndex = 2;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += This_OkClick;
		((Control)btCancel).Anchor = (AnchorStyles)10;
		btCancel.DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(700, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 24);
		((Control)btCancel).TabIndex = 3;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)btCancel).Click += This_CancelClick;
		((Control)tabControl).Anchor = (AnchorStyles)7;
		((Control)tabControl).Controls.Add((Control)(object)pageChart);
		((Control)tabControl).Controls.Add((Control)(object)pageSeries);
		((Control)tabControl).Controls.Add((Control)(object)pageArea);
		((Control)tabControl).Controls.Add((Control)(object)pageLabels);
		((Control)tabControl).Controls.Add((Control)(object)pageStyles);
		((Control)tabControl).Location = new Point(12, 12);
		((Control)tabControl).Name = "tabControl";
		((Control)tabControl).Padding = new Padding(0, 22, 0, 0);
		((Control)tabControl).Size = new Size(762, 491);
		((Control)tabControl).TabIndex = 0;
		tabControl.TitleBackColor = Color.White;
		tabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;
		((Control)pageChart).Controls.Add((Control)(object)pageChartContent);
		pageChart.Dock = (DockStyle)5;
		pageChart.InvisibleVisible = true;
		((Control)pageChart).Location = new Point(0, 22);
		((Control)pageChart).Name = "pageChart";
		((Control)pageChart).Padding = new Padding(0, 0, 1, 0);
		((Control)pageChart).Size = new Size(762, 469);
		((Control)pageChart).TabIndex = 0;
		((Control)pageChart).Text = "Chart";
		((Control)pageChart).Visible = false;
		((Control)pageChartContent).Anchor = (AnchorStyles)15;
		((Control)pageChartContent).BackColor = Color.White;
		((Control)pageChartContent).Location = new Point(3, 3);
		((Control)pageChartContent).Name = "pageChartContent";
		((Control)pageChartContent).Size = new Size(756, 463);
		((Control)pageChartContent).TabIndex = 0;
		((Control)pageSeries).BackColor = Color.White;
		((Control)pageSeries).Controls.Add((Control)(object)pageSeriesContent);
		pageSeries.Dock = (DockStyle)5;
		pageSeries.InvisibleVisible = false;
		((Control)pageSeries).Location = new Point(0, 22);
		((Control)pageSeries).Name = "pageSeries";
		((Control)pageSeries).Padding = new Padding(0, 0, 1, 2);
		((Control)pageSeries).Size = new Size(762, 469);
		((Control)pageSeries).TabIndex = 1;
		((Control)pageSeries).Text = "Series";
		((Control)pageSeries).Visible = false;
		((Control)pageSeriesContent).Anchor = (AnchorStyles)15;
		((Control)pageSeriesContent).BackColor = Color.White;
		((Control)pageSeriesContent).Location = new Point(3, 3);
		((Control)pageSeriesContent).Name = "pageSeriesContent";
		((Control)pageSeriesContent).Size = new Size(756, 464);
		((Control)pageSeriesContent).TabIndex = 0;
		((Control)pageArea).Controls.Add((Control)(object)pageAreaContent);
		pageArea.Dock = (DockStyle)5;
		pageArea.InvisibleVisible = false;
		((Control)pageArea).Location = new Point(0, 22);
		((Control)pageArea).Name = "pageArea";
		((Control)pageArea).Padding = new Padding(0, 0, 1, 0);
		((Control)pageArea).Size = new Size(762, 469);
		((Control)pageArea).TabIndex = 1;
		((Control)pageArea).Text = "Area";
		((Control)pageArea).Visible = false;
		((Control)pageAreaContent).Anchor = (AnchorStyles)15;
		((Control)pageAreaContent).Location = new Point(3, 3);
		((Control)pageAreaContent).Name = "pageAreaContent";
		((Control)pageAreaContent).Size = new Size(756, 463);
		((Control)pageAreaContent).TabIndex = 0;
		((Control)pageLabels).Controls.Add((Control)(object)pageLabelsContent);
		pageLabels.Dock = (DockStyle)5;
		pageLabels.InvisibleVisible = false;
		((Control)pageLabels).Location = new Point(0, 22);
		((Control)pageLabels).Name = "pageLabels";
		((Control)pageLabels).Padding = new Padding(0, 0, 1, 0);
		((Control)pageLabels).Size = new Size(762, 469);
		((Control)pageLabels).TabIndex = 1;
		((Control)pageLabels).Text = "Labels";
		((Control)pageLabels).Visible = false;
		((Control)pageLabelsContent).Anchor = (AnchorStyles)15;
		((Control)pageLabelsContent).Location = new Point(3, 3);
		((Control)pageLabelsContent).Name = "pageLabelsContent";
		((Control)pageLabelsContent).Size = new Size(756, 463);
		((Control)pageLabelsContent).TabIndex = 0;
		((Control)pageStyles).Controls.Add((Control)(object)pageStylesContent);
		pageStyles.Dock = (DockStyle)5;
		pageStyles.InvisibleVisible = false;
		((Control)pageStyles).Location = new Point(0, 22);
		((Control)pageStyles).Name = "pageStyles";
		((Control)pageStyles).Padding = new Padding(0, 0, 1, 0);
		((Control)pageStyles).Size = new Size(762, 469);
		((Control)pageStyles).TabIndex = 1;
		((Control)pageStyles).Text = "Styles";
		((Control)pageStyles).Visible = false;
		((Control)pageStylesContent).Anchor = (AnchorStyles)15;
		((Control)pageStylesContent).Location = new Point(3, 3);
		((Control)pageStylesContent).Name = "pageStylesContent";
		((Control)pageStylesContent).Size = new Size(756, 463);
		((Control)pageStylesContent).TabIndex = 0;
		((Control)stiGroupLine1).BackColor = Color.White;
		((Control)stiGroupLine1).Controls.Add((Control)(object)buttonLessOptions);
		((Control)stiGroupLine1).Controls.Add((Control)(object)buttonRunChartWizard);
		((Control)stiGroupLine1).Controls.Add((Control)(object)btOk);
		((Control)stiGroupLine1).Controls.Add((Control)(object)btCancel);
		((Control)stiGroupLine1).Dock = (DockStyle)2;
		stiGroupLine1.LinePosition = StiLinePosition.Top;
		stiGroupLine1.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine1).Location = new Point(0, 518);
		((Control)stiGroupLine1).Name = "stiGroupLine1";
		((Control)stiGroupLine1).Size = new Size(787, 46);
		((Control)stiGroupLine1).TabIndex = 2;
		((Control)buttonLessOptions).Location = new Point(12, 12);
		((Control)buttonLessOptions).Name = "buttonLessOptions";
		((Control)buttonLessOptions).Size = new Size(97, 24);
		((Control)buttonLessOptions).TabIndex = 0;
		((Control)buttonLessOptions).Text = "Less Options";
		((ButtonBase)buttonLessOptions).UseVisualStyleBackColor = true;
		((Control)buttonLessOptions).Click += This_LessOptionsClick;
		((Control)buttonRunChartWizard).AutoSize = true;
		((Control)buttonRunChartWizard).Location = new Point(115, 12);
		((Control)buttonRunChartWizard).Name = "buttonRunChartWizard";
		((Control)buttonRunChartWizard).Size = new Size(146, 24);
		((Control)buttonRunChartWizard).TabIndex = 1;
		((Control)buttonRunChartWizard).Text = "Run Chart Wizard";
		((ButtonBase)buttonRunChartWizard).UseVisualStyleBackColor = true;
		((Control)buttonRunChartWizard).Click += This_RunChartWizardClick;
		((Control)chartSampleImage).Location = new Point(50, 50);
		((Control)chartSampleImage).Name = "chartSampleImage";
		((Control)chartSampleImage).Size = new Size(400, 400);
		chartSampleImage.TabIndex = 0;
		chartSampleImage.TabStop = false;
		((Control)chartSampleImage).Paint += new PaintEventHandler(ChartSampleImage_Paint);
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(787, 564);
		((Control)this).Controls.Add((Control)(object)chartSampleImage);
		((Control)this).Controls.Add((Control)(object)tabControl);
		((Control)this).Controls.Add((Control)(object)stiGroupLine1);
		((Form)this).Margin = new Padding(3);
		((Control)this).Name = "StiCloudChartForm";
		((Form)this).SizeGripStyle = (SizeGripStyle)2;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "StiCloudChartForm";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Form)this).FormClosed += new FormClosedEventHandler(This_FormClosed);
		((Form)this).Load += This_Load;
		((Control)tabControl).ResumeLayout(false);
		((Control)pageChart).ResumeLayout(false);
		((Control)pageSeries).ResumeLayout(false);
		((Control)pageArea).ResumeLayout(false);
		((Control)pageLabels).ResumeLayout(false);
		((Control)pageStyles).ResumeLayout(false);
		((Control)stiGroupLine1).ResumeLayout(false);
		((Control)stiGroupLine1).PerformLayout();
		((ISupportInitialize)chartSampleImage).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
