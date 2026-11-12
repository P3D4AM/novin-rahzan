using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Controls;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Design.Images.Office2013;
using Stimulsoft.Report.Design.Panels;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Help;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart.Design;

public class StiChartEditorForm : Form
{
	public class StiSeriesTitle : StiSeries
	{
		private StiSeries series;

		public override string ToString()
		{
			return series.CoreTitle;
		}

		public override Type GetDefaultAreaType()
		{
			return null;
		}

		public StiSeriesTitle()
		{
		}

		public StiSeriesTitle(StiSeries series)
		{
			this.series = series;
		}

		public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
		{
			return null;
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

	private Hashtable menuItemsSeries;

	private StiChart chart;

	private StiChart tmpChartStyle;

	private StiChart tmpChartSettings;

	private StiChart tmpChartLabels;

	private StiService[] chartAreas;

	private List<StiChartStyle> chartStyles;

	private StiService[] chartLabelsTypes;

	private StiAxisSeriesLabels chartAxisLabelsSettings;

	private StiCenterPieLabels chartPieLabelsSettings;

	private StiAxisArea chartAxesSettings;

	private StiLegend chartLegendSettings;

	private int chartTypeId;

	private StiChartStyleId chartStyleId;

	private int chartLabelTypeId;

	private int lastSelectedSeries = -1;

	private IStiArea copyArea;

	private IStiArea lastArea;

	private ImageList ilData;

	private StiWizard Wizard;

	private StiWizardPage wpChartType;

	private StiWizardPage wpLabels;

	private StiMouseScrollableControl scChartType;

	private StiWizardPage wpStyle;

	private StiMouseScrollableControl scChartStyle;

	private TabControl stiTabControl1;

	private StiWizardPage wpAxes;

	private StiScrollableControl scAxes;

	private TabControl tcAxes;

	private TabControl tabControlLegend;

	private CheckBox cbxReverseX;

	private CheckBox cbxVisibleX;

	private CheckBox cbxReverseY;

	private CheckBox cbxVisibleY;

	private TabPage tpAxisX;

	private TabPage tpAxisY;

	private StiNumericUpDown udAngleX;

	private Label lbAngleX;

	private StiComboBox cbLabelsX;

	private CheckBox cbxInterlacedX;

	private StiComboBox cbGridLinesX;

	private StiComboBox cbTicksX;

	private StiComboBox cbAligmentX;

	private Label lbAligmentX;

	private StiTextBox tbTitleX;

	private StiNumericUpDown udMinorCountX;

	private StiOffice2013GroupBox gbLabelsX;

	private StiOffice2013GroupBox gbGridLinesX;

	private StiOffice2013GroupBox gbTicksX;

	private Label lbMinorCountX;

	private StiOffice2013GroupBox gbTitleX;

	private StiOffice2013GroupBox gbLabelsY;

	private StiNumericUpDown udAngleY;

	private Label lbAngleY;

	private StiComboBox cbLabelsY;

	private StiOffice2013GroupBox gbGridLinesY;

	private CheckBox cbxInterlacedY;

	private StiComboBox cbGridLinesY;

	private StiOffice2013GroupBox gbTicksY;

	private StiNumericUpDown udMinorCountY;

	private Label lbMinorCountY;

	private StiComboBox cbTicksY;

	private StiOffice2013GroupBox gbTitleY;

	private StiComboBox cbAligmentY;

	private Label lbAligmentY;

	private StiTextBox tbTitleY;

	private CheckBox cbxLegendVisible;

	private StiOffice2013GroupBox gbAligment;

	private Label lbHorizontalAligment;

	private Label lbVerticalAligment;

	private StiComboBox cbHorizontalAligment;

	private StiComboBox cbVerticalAligment;

	private StiOffice2013GroupBox gbDirection;

	private StiOffice2013GroupBox gbMarker;

	private StiOffice2013GroupBox gbSpacing;

	private StiTextBox tbLegendTitle;

	private StiComboBox cbDirection;

	private CheckBox cbxMarkerVisible;

	private Label lbWidth;

	private Label lbHeight;

	private StiNumericUpDown udWidth;

	private StiNumericUpDown udHeight;

	private Label lbVerticalSpacing;

	private Label lbHorizontalSpacing;

	private StiNumericUpDown udVerticalSpacing;

	private StiNumericUpDown udHorizontalSpacing;

	private StiScrollableControl scLegends;

	private StiComboBox cbValueType;

	private StiTextBox tbTextBefore;

	private StiTextBox tbTextAfter;

	private Label lbTextAfter;

	private Label lbAngle;

	private CheckBox cbxAutoRotation;

	private StiOffice2013GroupBox gbLabels;

	private TabPage gbLegends;

	private StiNumericUpDown udLabelsAngle;

	private ListBox lbSeries;

	private StiTextBox tbSeriesName;

	private DataGridView dgPoints;

	private DataSet dsPoints;

	private DataTable dtPoints;

	private DataColumn dcArgument;

	private DataColumn dcValue;

	private DataColumn dcEndValue;

	private DataColumn dcOpenValue;

	private DataColumn dcCloseValue;

	private DataColumn dcHighValue;

	private DataColumn dcLowValue;

	private StiTreeViewBox twBoxArguments;

	private StiTreeViewBox twBoxValues;

	private StiTreeViewBox twBoxEndValues;

	private StiTreeViewBox twBoxOpenValues;

	private StiTreeViewBox twBoxCloseValues;

	private StiTreeViewBox twBoxHighValues;

	private StiTreeViewBox twBoxLowValues;

	public StiToolBar tbTool;

	private StiToolButton tbMoveDown;

	private StiToolButton tbMoveUp;

	private StiToolButton stiToolButton3;

	private StiToolButton tbRemove;

	private StiToolButton tbAddSeries;

	private ContextMenuStrip cmSeries;

	private StiOffice2013GroupBox groupBoxTitle;

	private Label labelValueType;

	private StiGroupLine stiGroupLine1;

	private StiWizardPage wpLegend;

	private StiWizardPage wpSeriesData;

	private TabPage tabPageDataColumns;

	private Label labelValueDataColumn;

	private Label labelEndValueDataColumn;

	private Label labelOpenValueDataColumn;

	private Label labelCloseValueDataColumn;

	private Label labelHighValueDataColumn;

	private Label labelLowValueDataColumn;

	private Label labelArgumentDataColumn;

	private TabPage tabPageListOfValues;

	private Label labelSeriesName;

	private ToolTip toolTip1;

	private Label lbTextBefore;

	private StiMouseScrollableControl scrollableLabels;

	private StiToolButton tbNewDataSource;

	private StiGroupLine stiGroupLine2;

	private StiGroupLine stiGroupLine3;

	private IContainer components;

	private StiDataBuilder dataBuilder = new StiDataBuilder();

	public StiChartEditorForm()
		: this(null)
	{
	}

	public StiChartEditorForm(StiChart chart)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (((Component)this).DesignMode)
		{
			return;
		}
		((Form)this).ShowIcon = false;
		if (StiOptions.Appearance.ShowFormsHelp)
		{
			((Form)this).HelpButton = true;
		}
		this.chart = chart;
		copyArea = chart.Area;
		tmpChartStyle = chart.Clone() as StiChart;
		tmpChartSettings = chart.Clone() as StiChart;
		tmpChartSettings.Series.Clear();
		chartAreas = GetChartAreas();
		chartStyles = GetChartStyles();
		chartLabelsTypes = GetLabelTypes();
		Localize();
		((ListControl)cbValueType).SelectedIndex = 0;
		((Control)tbTextBefore).Text = "";
		((Control)tbTextAfter).Text = "";
		udLabelsAngle.Value = 0m;
		cbxAutoRotation.Checked = false;
		cbxVisibleX.Checked = true;
		cbxReverseX.Checked = false;
		((Control)tbTitleX).Text = "";
		((ListControl)cbAligmentX).SelectedIndex = 1;
		((ListControl)cbTicksX).SelectedIndex = 1;
		udMinorCountX.Value = 4m;
		((ListControl)cbGridLinesX).SelectedIndex = 1;
		cbxInterlacedX.Checked = true;
		((ListControl)cbLabelsX).SelectedIndex = 1;
		udAngleX.Value = 0m;
		cbxVisibleY.Checked = true;
		cbxReverseY.Checked = false;
		((Control)tbTitleY).Text = "";
		((ListControl)cbAligmentY).SelectedIndex = 1;
		((ListControl)cbTicksY).SelectedIndex = 1;
		udMinorCountY.Value = 4m;
		((ListControl)cbGridLinesY).SelectedIndex = 1;
		cbxInterlacedY.Checked = true;
		((ListControl)cbLabelsY).SelectedIndex = 1;
		udAngleY.Value = 0m;
		cbxLegendVisible.Checked = true;
		((Control)tbLegendTitle).Text = "";
		((ListControl)cbHorizontalAligment).SelectedIndex = 1;
		((ListControl)cbVerticalAligment).SelectedIndex = 1;
		((ListControl)cbDirection).SelectedIndex = 2;
		cbxMarkerVisible.Checked = true;
		udWidth.Value = 10m;
		udHeight.Value = 10m;
		udHorizontalSpacing.Value = 4m;
		udVerticalSpacing.Value = 2m;
		((Control)twBoxArguments).Text = "";
		((Control)twBoxValues).Text = "";
		((Control)twBoxEndValues).Text = "";
		((Control)twBoxOpenValues).Text = "";
		((Control)twBoxCloseValues).Text = "";
		((Control)twBoxHighValues).Text = "";
		((Control)twBoxLowValues).Text = "";
		FillDateTablePoints(tmpChartSettings.Area);
		foreach (StiSeries item in chart.Series)
		{
			tmpChartSettings.Series.Add(item);
			item.Core.ApplyStyle(chart.Style, chart.Style.Core.GetColorBySeries(item, item.Core?.SeriesColors));
			StiSeriesTitle stiSeriesTitle = new StiSeriesTitle(item);
			lbSeries.Items.Add((object)stiSeriesTitle);
			((ListControl)lbSeries).SelectedIndex = lbSeries.Items.Count - 1;
		}
		((Control)dgPoints).Enabled = lbSeries.Items.Count > 0;
		((Control)twBoxArguments).Enabled = lbSeries.Items.Count > 0;
		((Control)twBoxValues).Enabled = lbSeries.Items.Count > 0;
		((Control)tbNewDataSource).Text = StiLocalization.Get("FormDictionaryDesigner", "DataSourceNew");
		chartStyleId = (from s in StiOptions.Services.ChartStyles.Where((StiChartStyle s) => s.ServiceEnabled).ToList()
			orderby s.Core.StyleId descending
			select s).FirstOrDefault().Core.StyleId;
		DoScaling();
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("FormTitles", "ChartWizardForm");
		((Control)wpChartType).Text = StiLocalization.Get("Chart", "ChartType");
		((Control)wpStyle).Text = StiLocalization.Get("Chart", "Style");
		((Control)wpLegend).Text = StiLocalization.Get("Chart", "Legend");
		((Control)wpLabels).Text = StiLocalization.Get("Chart", "Labels");
		((Control)wpSeriesData).Text = StiLocalization.Get("Chart", "Serieses");
		((Control)wpAxes).Text = StiLocalization.Get("Chart", "Axes");
		toolTip1.SetToolTip((Control)(object)tbAddSeries, StiLocalization.Get("Chart", "AddSeries"));
		toolTip1.SetToolTip((Control)(object)tbRemove, StiLocalization.Get("Chart", "RemoveSeries"));
		toolTip1.SetToolTip((Control)(object)tbMoveUp, StiLocalization.Get("Chart", "MoveSeriesUp"));
		toolTip1.SetToolTip((Control)(object)tbMoveDown, StiLocalization.Get("Chart", "MoveSeriesDown"));
		Wizard.BackText = StiLocalization.Get("Wizards", "ButtonBack");
		Wizard.CancelText = StiLocalization.Get("Wizards", "ButtonCancel");
		Wizard.FinishText = StiLocalization.Get("Wizards", "ButtonFinish");
		Wizard.NextText = StiLocalization.Get("Wizards", "ButtonNext");
		((Control)tabPageDataColumns).Text = StiLocalization.Get("Chart", "DataColumns");
		((Control)tabPageListOfValues).Text = StiLocalization.Get("Chart", "ListOfValues");
		((Control)labelSeriesName).Text = Loc.GetCleaned("Chart", "LabelSeriesName");
		((Control)labelArgumentDataColumn).Text = Loc.GetCleaned("Chart", "LabelArgumentDataColumn");
		((Control)labelValueDataColumn).Text = Loc.GetCleaned("Chart", "LabelValueDataColumn");
		((Control)labelEndValueDataColumn).Text = Loc.GetCleaned("Chart", "LabelEndValueDataColumn");
		((Control)labelOpenValueDataColumn).Text = Loc.GetCleaned("Chart", "LabelOpenValueDataColumn");
		((Control)labelCloseValueDataColumn).Text = Loc.GetCleaned("Chart", "LabelCloseValueDataColumn");
		((Control)labelHighValueDataColumn).Text = Loc.GetCleaned("Chart", "LabelHighValueDataColumn");
		((Control)labelLowValueDataColumn).Text = Loc.GetCleaned("Chart", "LabelLowValueDataColumn");
		((Control)gbLabels).Text = Loc.GetCleaned("Chart", "Labels");
		((Control)labelValueType).Text = Loc.GetCleaned("Chart", "LabelValueType");
		((Control)lbTextBefore).Text = Loc.GetCleaned("Chart", "LabelTextBefore");
		((Control)lbTextAfter).Text = Loc.GetCleaned("Chart", "LabelTextAfter");
		((Control)lbAngle).Text = Loc.GetCleaned("Chart", "LabelAngle");
		((Control)cbxAutoRotation).Text = Loc.GetCleaned("Chart", "CheckBoxAutoRotation");
		((Control)tpAxisX).Text = Loc.GetCleaned("Chart", "AxisX");
		((Control)gbTitleX).Text = Loc.GetCleaned("PropertyMain", "Title");
		((Control)lbAligmentX).Text = Loc.GetCleaned("Chart", "LabelTitleAlignment");
		((Control)gbTicksX).Text = Loc.GetCleaned("PropertyMain", "Ticks");
		((Control)lbMinorCountX).Text = Loc.GetCleaned("Chart", "LabelMinorCount");
		((Control)gbGridLinesX).Text = Loc.GetCleaned("Chart", "GridLines");
		((Control)cbxInterlacedX).Text = Loc.GetCleaned("Chart", "GridInterlaced");
		((Control)gbLabelsX).Text = Loc.GetCleaned("PropertyMain", "Labels");
		((Control)lbAngleX).Text = Loc.GetCleaned("Chart", "LabelAngle");
		((Control)cbxVisibleX).Text = Loc.GetCleaned("PropertyMain", "Visible");
		((Control)cbxReverseX).Text = Loc.GetCleaned("Chart", "AxisReverse");
		((Control)tpAxisY).Text = Loc.GetCleaned("Chart", "AxisY");
		((Control)gbTitleY).Text = Loc.GetCleaned("PropertyMain", "Title");
		((Control)lbAligmentY).Text = Loc.GetCleaned("Chart", "LabelTitleAlignment");
		((Control)gbTicksY).Text = Loc.GetCleaned("PropertyMain", "Ticks");
		((Control)lbMinorCountY).Text = Loc.GetCleaned("Chart", "LabelMinorCount");
		((Control)gbGridLinesY).Text = Loc.GetCleaned("Chart", "GridLines");
		((Control)cbxInterlacedY).Text = Loc.GetCleaned("Chart", "GridInterlaced");
		((Control)gbLabelsY).Text = Loc.GetCleaned("PropertyMain", "Labels");
		((Control)lbAngleY).Text = Loc.GetCleaned("Chart", "LabelAngle");
		((Control)cbxVisibleY).Text = Loc.GetCleaned("PropertyMain", "Visible");
		((Control)cbxReverseY).Text = Loc.GetCleaned("Chart", "AxisReverse");
		((Control)gbLegends).Text = Loc.GetCleaned("Chart", "Legend");
		((Control)groupBoxTitle).Text = Loc.GetCleaned("PropertyMain", "Title");
		((Control)gbAligment).Text = Loc.GetCleaned("PropertyMain", "Alignment");
		((Control)lbHorizontalAligment).Text = Loc.GetCleaned("Chart", "LabelAlignmentHorizontal");
		((Control)lbVerticalAligment).Text = Loc.GetCleaned("Chart", "LabelAlignmentVertical");
		((Control)gbDirection).Text = Loc.GetCleaned("PropertyMain", "Direction");
		((Control)gbMarker).Text = Loc.GetCleaned("Chart", "Marker");
		((Control)cbxMarkerVisible).Text = Loc.GetCleaned("PropertyMain", "Visible");
		((Control)lbWidth).Text = Loc.GetCleaned("Wizards", "LabelWidth");
		((Control)lbHeight).Text = Loc.GetCleaned("Wizards", "LabelHeight");
		((Control)gbSpacing).Text = Loc.GetCleaned("Chart", "LegendSpacing");
		((Control)lbHorizontalSpacing).Text = Loc.GetCleaned("Chart", "LabelAlignmentHorizontal");
		((Control)lbVerticalSpacing).Text = Loc.GetCleaned("Chart", "LabelAlignmentVertical");
		((Control)cbxLegendVisible).Text = Loc.GetCleaned("PropertyMain", "Visible");
		((ComboBox)cbValueType).Items.Clear();
		((ComboBox)cbValueType).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiSeriesLabelsValueTypeValue"));
		((ComboBox)cbValueType).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiSeriesLabelsValueTypeSeriesTitle"));
		((ComboBox)cbValueType).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiSeriesLabelsValueTypeArgument"));
		((ComboBox)cbValueType).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiSeriesLabelsValueTypeValueArgument"));
		((ComboBox)cbValueType).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiSeriesLabelsValueTypeArgumentValue"));
		((ComboBox)cbValueType).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiSeriesLabelsValueTypeSeriesTitleValue"));
		((ComboBox)cbValueType).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiSeriesLabelsValueTypeSeriesTitleArgument"));
		((ComboBox)cbAligmentX).Items.Clear();
		((ComboBox)cbAligmentX).Items.Add((object)StiLocalization.Get("PropertyEnum", "StringAlignmentNear"));
		((ComboBox)cbAligmentX).Items.Add((object)StiLocalization.Get("PropertyEnum", "StringAlignmentCenter"));
		((ComboBox)cbAligmentX).Items.Add((object)StiLocalization.Get("PropertyEnum", "StringAlignmentFar"));
		((ComboBox)cbTicksX).Items.Clear();
		((ComboBox)cbTicksX).Items.Add((object)StiLocalization.Get("PropertyEnum", "ChartAxesTicksNone"));
		((ComboBox)cbTicksX).Items.Add((object)StiLocalization.Get("PropertyEnum", "ChartAxesTicksMajor"));
		((ComboBox)cbTicksX).Items.Add((object)StiLocalization.Get("PropertyEnum", "ChartAxesTicksAll"));
		((ComboBox)cbGridLinesX).Items.Clear();
		((ComboBox)cbGridLinesX).Items.Add((object)StiLocalization.Get("PropertyEnum", "ChartGridLinesNone"));
		((ComboBox)cbGridLinesX).Items.Add((object)StiLocalization.Get("PropertyEnum", "ChartGridLinesMajor"));
		((ComboBox)cbGridLinesX).Items.Add((object)StiLocalization.Get("PropertyEnum", "ChartGridLinesAll"));
		((ComboBox)cbLabelsX).Items.Clear();
		((ComboBox)cbLabelsX).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiLabelsPlacementNone"));
		((ComboBox)cbLabelsX).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiLabelsPlacementOneLine"));
		((ComboBox)cbLabelsX).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiLabelsPlacementTwoLines"));
		((ComboBox)cbAligmentY).Items.Clear();
		((ComboBox)cbAligmentY).Items.Add((object)StiLocalization.Get("PropertyEnum", "StringAlignmentNear"));
		((ComboBox)cbAligmentY).Items.Add((object)StiLocalization.Get("PropertyEnum", "StringAlignmentCenter"));
		((ComboBox)cbAligmentY).Items.Add((object)StiLocalization.Get("PropertyEnum", "StringAlignmentFar"));
		((ComboBox)cbTicksY).Items.Clear();
		((ComboBox)cbTicksY).Items.Add((object)StiLocalization.Get("PropertyEnum", "ChartAxesTicksNone"));
		((ComboBox)cbTicksY).Items.Add((object)StiLocalization.Get("PropertyEnum", "ChartAxesTicksMajor"));
		((ComboBox)cbTicksY).Items.Add((object)StiLocalization.Get("PropertyEnum", "ChartAxesTicksAll"));
		((ComboBox)cbGridLinesY).Items.Clear();
		((ComboBox)cbGridLinesY).Items.Add((object)StiLocalization.Get("PropertyEnum", "ChartGridLinesNone"));
		((ComboBox)cbGridLinesY).Items.Add((object)StiLocalization.Get("PropertyEnum", "ChartGridLinesMajor"));
		((ComboBox)cbGridLinesY).Items.Add((object)StiLocalization.Get("PropertyEnum", "ChartGridLinesAll"));
		((ComboBox)cbLabelsY).Items.Clear();
		((ComboBox)cbLabelsY).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiLabelsPlacementNone"));
		((ComboBox)cbLabelsY).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiLabelsPlacementOneLine"));
		((ComboBox)cbLabelsY).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiLabelsPlacementTwoLines"));
		((ComboBox)cbHorizontalAligment).Items.Clear();
		((ComboBox)cbHorizontalAligment).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiLegendHorAlignmentLeftOutside"));
		((ComboBox)cbHorizontalAligment).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiLegendHorAlignmentLeft"));
		((ComboBox)cbHorizontalAligment).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiLegendHorAlignmentCenter"));
		((ComboBox)cbHorizontalAligment).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiLegendHorAlignmentRight"));
		((ComboBox)cbHorizontalAligment).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiLegendHorAlignmentRightOutside"));
		((ComboBox)cbVerticalAligment).Items.Clear();
		((ComboBox)cbVerticalAligment).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiLegendVertAlignmentTopOutside"));
		((ComboBox)cbVerticalAligment).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiLegendVertAlignmentTop"));
		((ComboBox)cbVerticalAligment).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiLegendVertAlignmentCenter"));
		((ComboBox)cbVerticalAligment).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiLegendVertAlignmentBottom"));
		((ComboBox)cbVerticalAligment).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiLegendVertAlignmentBottomOutside"));
		((ComboBox)cbDirection).Items.Clear();
		((ComboBox)cbDirection).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiLegendDirectionLeftToRight"));
		((ComboBox)cbDirection).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiLegendDirectionRightToLeft"));
		((ComboBox)cbDirection).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiLegendDirectionTopToBottom"));
		((ComboBox)cbDirection).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiLegendDirectionBottomToTop"));
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
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Expected O, but got Unknown
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Expected O, but got Unknown
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Expected O, but got Unknown
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected O, but got Unknown
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Expected O, but got Unknown
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Expected O, but got Unknown
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Expected O, but got Unknown
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Expected O, but got Unknown
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Expected O, but got Unknown
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Expected O, but got Unknown
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Expected O, but got Unknown
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Expected O, but got Unknown
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Expected O, but got Unknown
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Expected O, but got Unknown
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Expected O, but got Unknown
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Expected O, but got Unknown
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Expected O, but got Unknown
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Expected O, but got Unknown
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Expected O, but got Unknown
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Expected O, but got Unknown
		//IL_0324: Unknown result type (might be due to invalid IL or missing references)
		//IL_032e: Expected O, but got Unknown
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Expected O, but got Unknown
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Expected O, but got Unknown
		//IL_0366: Unknown result type (might be due to invalid IL or missing references)
		//IL_0370: Expected O, but got Unknown
		//IL_0387: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Expected O, but got Unknown
		//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bd: Expected O, but got Unknown
		//IL_03df: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e9: Expected O, but got Unknown
		//IL_03f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ff: Expected O, but got Unknown
		//IL_0400: Unknown result type (might be due to invalid IL or missing references)
		//IL_040a: Expected O, but got Unknown
		//IL_0421: Unknown result type (might be due to invalid IL or missing references)
		//IL_042b: Expected O, but got Unknown
		//IL_042c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0436: Expected O, but got Unknown
		//IL_0458: Unknown result type (might be due to invalid IL or missing references)
		//IL_0462: Expected O, but got Unknown
		//IL_0463: Unknown result type (might be due to invalid IL or missing references)
		//IL_046d: Expected O, but got Unknown
		//IL_048f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0499: Expected O, but got Unknown
		//IL_049a: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a4: Expected O, but got Unknown
		//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04af: Expected O, but got Unknown
		//IL_04e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f1: Expected O, but got Unknown
		//IL_04f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fc: Expected O, but got Unknown
		//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0507: Expected O, but got Unknown
		//IL_057c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0586: Expected O, but got Unknown
		//IL_076b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0775: Expected O, but got Unknown
		//IL_08e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ec: Expected O, but got Unknown
		//IL_0913: Unknown result type (might be due to invalid IL or missing references)
		//IL_091d: Expected O, but got Unknown
		//IL_0968: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ed: Expected O, but got Unknown
		//IL_0aa8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb3: Expected O, but got Unknown
		//IL_0bd7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be1: Expected O, but got Unknown
		//IL_0c40: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d35: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d3f: Expected O, but got Unknown
		//IL_0d63: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d6d: Expected O, but got Unknown
		//IL_0e24: Unknown result type (might be due to invalid IL or missing references)
		//IL_1097: Unknown result type (might be due to invalid IL or missing references)
		//IL_1904: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c91: Unknown result type (might be due to invalid IL or missing references)
		//IL_217c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2272: Unknown result type (might be due to invalid IL or missing references)
		//IL_227c: Expected O, but got Unknown
		//IL_2289: Unknown result type (might be due to invalid IL or missing references)
		//IL_2293: Expected O, but got Unknown
		//IL_2475: Unknown result type (might be due to invalid IL or missing references)
		//IL_247f: Expected O, but got Unknown
		//IL_2617: Unknown result type (might be due to invalid IL or missing references)
		//IL_2621: Expected O, but got Unknown
		//IL_26b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_26bd: Expected O, but got Unknown
		//IL_272f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2739: Expected O, but got Unknown
		//IL_27de: Unknown result type (might be due to invalid IL or missing references)
		//IL_27e8: Expected O, but got Unknown
		//IL_285a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2864: Expected O, but got Unknown
		//IL_2a0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2aff: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b09: Expected O, but got Unknown
		//IL_2c7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e26: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e30: Expected O, but got Unknown
		//IL_2ea2: Unknown result type (might be due to invalid IL or missing references)
		//IL_2eac: Expected O, but got Unknown
		//IL_3090: Unknown result type (might be due to invalid IL or missing references)
		//IL_309a: Expected O, but got Unknown
		//IL_329e: Unknown result type (might be due to invalid IL or missing references)
		//IL_32a8: Expected O, but got Unknown
		//IL_331a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3324: Expected O, but got Unknown
		//IL_35af: Unknown result type (might be due to invalid IL or missing references)
		//IL_35b9: Expected O, but got Unknown
		//IL_365e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3668: Expected O, but got Unknown
		//IL_38a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a58: Expected O, but got Unknown
		//IL_3aca: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ad4: Expected O, but got Unknown
		//IL_3cb8: Unknown result type (might be due to invalid IL or missing references)
		//IL_3cc2: Expected O, but got Unknown
		//IL_3ec6: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ed0: Expected O, but got Unknown
		//IL_3f42: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f4c: Expected O, but got Unknown
		//IL_41d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_41e1: Expected O, but got Unknown
		//IL_4286: Unknown result type (might be due to invalid IL or missing references)
		//IL_4290: Expected O, but got Unknown
		//IL_445f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4562: Unknown result type (might be due to invalid IL or missing references)
		//IL_456c: Expected O, but got Unknown
		//IL_46b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_47fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_4805: Expected O, but got Unknown
		//IL_4897: Unknown result type (might be due to invalid IL or missing references)
		//IL_48a1: Expected O, but got Unknown
		//IL_4916: Unknown result type (might be due to invalid IL or missing references)
		//IL_4920: Expected O, but got Unknown
		//IL_49ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_49b5: Expected O, but got Unknown
		//IL_4b49: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b53: Expected O, but got Unknown
		//IL_4be5: Unknown result type (might be due to invalid IL or missing references)
		//IL_4bef: Expected O, but got Unknown
		//IL_4c64: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c6e: Expected O, but got Unknown
		//IL_4cf9: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d03: Expected O, but got Unknown
		//IL_4d9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4da5: Expected O, but got Unknown
		//IL_51a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_51ac: Expected O, but got Unknown
		//IL_5234: Unknown result type (might be due to invalid IL or missing references)
		//IL_523e: Expected O, but got Unknown
		//IL_5698: Unknown result type (might be due to invalid IL or missing references)
		//IL_56d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_56df: Expected O, but got Unknown
		//IL_56f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_5703: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiChartEditorForm));
		ilData = new ImageList(components);
		Wizard = new StiWizard();
		wpChartType = new StiWizardPage();
		scChartType = new StiMouseScrollableControl();
		wpStyle = new StiWizardPage();
		scChartStyle = new StiMouseScrollableControl();
		wpSeriesData = new StiWizardPage();
		stiTabControl1 = new TabControl();
		tabPageDataColumns = new TabPage();
		twBoxValues = new StiTreeViewBox();
		twBoxEndValues = new StiTreeViewBox();
		twBoxOpenValues = new StiTreeViewBox();
		twBoxCloseValues = new StiTreeViewBox();
		twBoxHighValues = new StiTreeViewBox();
		twBoxLowValues = new StiTreeViewBox();
		twBoxArguments = new StiTreeViewBox();
		labelValueDataColumn = new Label();
		labelEndValueDataColumn = new Label();
		labelOpenValueDataColumn = new Label();
		labelCloseValueDataColumn = new Label();
		labelHighValueDataColumn = new Label();
		labelLowValueDataColumn = new Label();
		labelArgumentDataColumn = new Label();
		tabPageListOfValues = new TabPage();
		dgPoints = new DataGridView();
		dtPoints = new DataTable();
		tbSeriesName = new StiTextBox();
		labelSeriesName = new Label();
		lbSeries = new ListBox();
		tbTool = new StiToolBar();
		tbNewDataSource = new StiToolButton();
		tbMoveDown = new StiToolButton();
		tbMoveUp = new StiToolButton();
		stiToolButton3 = new StiToolButton();
		tbRemove = new StiToolButton();
		tbAddSeries = new StiToolButton();
		cmSeries = new ContextMenuStrip();
		wpLabels = new StiWizardPage();
		scrollableLabels = new StiMouseScrollableControl();
		gbLabels = new StiOffice2013GroupBox();
		stiGroupLine1 = new StiGroupLine();
		labelValueType = new Label();
		cbxAutoRotation = new CheckBox();
		lbAngle = new Label();
		udLabelsAngle = new StiNumericUpDown();
		tbTextAfter = new StiTextBox();
		lbTextBefore = new Label();
		tbTextBefore = new StiTextBox();
		lbTextAfter = new Label();
		cbValueType = new StiComboBox();
		wpAxes = new StiWizardPage();
		scAxes = new StiScrollableControl();
		tcAxes = new TabControl();
		tpAxisX = new TabPage();
		stiGroupLine2 = new StiGroupLine();
		gbLabelsX = new StiOffice2013GroupBox();
		udAngleX = new StiNumericUpDown();
		lbAngleX = new Label();
		cbLabelsX = new StiComboBox();
		gbGridLinesX = new StiOffice2013GroupBox();
		cbxInterlacedX = new CheckBox();
		cbGridLinesX = new StiComboBox();
		gbTicksX = new StiOffice2013GroupBox();
		udMinorCountX = new StiNumericUpDown();
		lbMinorCountX = new Label();
		cbTicksX = new StiComboBox();
		gbTitleX = new StiOffice2013GroupBox();
		cbAligmentX = new StiComboBox();
		lbAligmentX = new Label();
		tbTitleX = new StiTextBox();
		cbxReverseX = new CheckBox();
		cbxVisibleX = new CheckBox();
		tpAxisY = new TabPage();
		stiGroupLine3 = new StiGroupLine();
		gbLabelsY = new StiOffice2013GroupBox();
		udAngleY = new StiNumericUpDown();
		lbAngleY = new Label();
		cbLabelsY = new StiComboBox();
		gbGridLinesY = new StiOffice2013GroupBox();
		cbxInterlacedY = new CheckBox();
		cbGridLinesY = new StiComboBox();
		gbTicksY = new StiOffice2013GroupBox();
		udMinorCountY = new StiNumericUpDown();
		lbMinorCountY = new Label();
		cbTicksY = new StiComboBox();
		gbTitleY = new StiOffice2013GroupBox();
		cbAligmentY = new StiComboBox();
		lbAligmentY = new Label();
		tbTitleY = new StiTextBox();
		cbxReverseY = new CheckBox();
		cbxVisibleY = new CheckBox();
		wpLegend = new StiWizardPage();
		scLegends = new StiScrollableControl();
		tabControlLegend = new TabControl();
		gbLegends = new TabPage();
		gbSpacing = new StiOffice2013GroupBox();
		udVerticalSpacing = new StiNumericUpDown();
		udHorizontalSpacing = new StiNumericUpDown();
		lbVerticalSpacing = new Label();
		lbHorizontalSpacing = new Label();
		gbMarker = new StiOffice2013GroupBox();
		udHeight = new StiNumericUpDown();
		udWidth = new StiNumericUpDown();
		lbHeight = new Label();
		lbWidth = new Label();
		cbxMarkerVisible = new CheckBox();
		gbDirection = new StiOffice2013GroupBox();
		cbDirection = new StiComboBox();
		gbAligment = new StiOffice2013GroupBox();
		cbVerticalAligment = new StiComboBox();
		cbHorizontalAligment = new StiComboBox();
		lbVerticalAligment = new Label();
		lbHorizontalAligment = new Label();
		cbxLegendVisible = new CheckBox();
		groupBoxTitle = new StiOffice2013GroupBox();
		tbLegendTitle = new StiTextBox();
		dcArgument = new DataColumn();
		dcValue = new DataColumn();
		dcEndValue = new DataColumn();
		dcOpenValue = new DataColumn();
		dcCloseValue = new DataColumn();
		dcHighValue = new DataColumn();
		dcLowValue = new DataColumn();
		dsPoints = new DataSet();
		toolTip1 = new ToolTip(components);
		((Control)Wizard).SuspendLayout();
		((Control)wpChartType).SuspendLayout();
		((Control)wpStyle).SuspendLayout();
		((Control)wpSeriesData).SuspendLayout();
		((Control)stiTabControl1).SuspendLayout();
		((Control)tabPageDataColumns).SuspendLayout();
		((Control)tabPageListOfValues).SuspendLayout();
		((ISupportInitialize)dgPoints).BeginInit();
		((ISupportInitialize)dtPoints).BeginInit();
		((Control)tbTool).SuspendLayout();
		((Control)wpLabels).SuspendLayout();
		((Control)gbLabels).SuspendLayout();
		((ISupportInitialize)udLabelsAngle).BeginInit();
		((Control)wpAxes).SuspendLayout();
		((Control)tcAxes).SuspendLayout();
		((Control)tpAxisX).SuspendLayout();
		((Control)gbLabelsX).SuspendLayout();
		((ISupportInitialize)udAngleX).BeginInit();
		((Control)gbGridLinesX).SuspendLayout();
		((Control)gbTicksX).SuspendLayout();
		((ISupportInitialize)udMinorCountX).BeginInit();
		((Control)gbTitleX).SuspendLayout();
		((Control)tpAxisY).SuspendLayout();
		((Control)gbLabelsY).SuspendLayout();
		((ISupportInitialize)udAngleY).BeginInit();
		((Control)gbGridLinesY).SuspendLayout();
		((Control)gbTicksY).SuspendLayout();
		((ISupportInitialize)udMinorCountY).BeginInit();
		((Control)gbTitleY).SuspendLayout();
		((Control)wpLegend).SuspendLayout();
		((Control)tabControlLegend).SuspendLayout();
		((Control)gbLegends).SuspendLayout();
		((Control)gbSpacing).SuspendLayout();
		((ISupportInitialize)udVerticalSpacing).BeginInit();
		((ISupportInitialize)udHorizontalSpacing).BeginInit();
		((Control)gbMarker).SuspendLayout();
		((ISupportInitialize)udHeight).BeginInit();
		((ISupportInitialize)udWidth).BeginInit();
		((Control)gbDirection).SuspendLayout();
		((Control)gbAligment).SuspendLayout();
		((Control)groupBoxTitle).SuspendLayout();
		((ISupportInitialize)dsPoints).BeginInit();
		((Control)this).SuspendLayout();
		ilData.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("ilData.ImageStream");
		ilData.TransparentColor = Color.Magenta;
		ilData.Images.SetKeyName(0, "");
		ilData.Images.SetKeyName(1, "");
		ilData.Images.SetKeyName(2, "");
		ilData.Images.SetKeyName(3, "");
		ilData.Images.SetKeyName(4, "DataSourceNew.png");
		Wizard.ContentColor = Color.Empty;
		((Control)Wizard).Controls.Add((Control)(object)wpChartType);
		((Control)Wizard).Controls.Add((Control)(object)wpStyle);
		((Control)Wizard).Controls.Add((Control)(object)wpSeriesData);
		((Control)Wizard).Controls.Add((Control)(object)wpLabels);
		((Control)Wizard).Controls.Add((Control)(object)wpAxes);
		((Control)Wizard).Controls.Add((Control)(object)wpLegend);
		Wizard.ControlStyle = StiControlStyle.Office2013Blue;
		((Control)Wizard).Dock = (DockStyle)5;
		((Control)Wizard).ForeColor = SystemColors.ControlText;
		Wizard.HeaderBackgroundImage = null;
		Wizard.HeaderColor = Color.White;
		Wizard.HeaderDescriptionFont = new Font("Times New Roman", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		Wizard.HeaderTextColor = Color.Empty;
		Wizard.HeaderTitleFont = new Font("Times New Roman", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)204);
		Wizard.HelpButtonVisible = false;
		((Control)Wizard).Location = new Point(12, 12);
		Wizard.MarkLastPage = false;
		((Control)Wizard).Name = "Wizard";
		((Control)Wizard).Padding = new Padding(200, 0, 0, 46);
		Wizard.ProgressTreeOneLevel = true;
		Wizard.ShowHeader = false;
		Wizard.ShowProgressLine = false;
		((Control)Wizard).Size = new Size(768, 611);
		((Control)Wizard).TabIndex = 3;
		((Control)Wizard).Text = "Introduction";
		Wizard.TreeProgressFont = new Font("Times New Roman", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		Wizard.CancelClick += Wizard_CancelClick;
		Wizard.BackClick += Wizard_ChangeSelected;
		Wizard.NextClick += Wizard_ChangeSelected;
		Wizard.FinishClick += Wizard_FinishClick;
		((Control)wpChartType).BackColor = Color.White;
		((Control)wpChartType).Controls.Add((Control)(object)scChartType);
		wpChartType.Dock = (DockStyle)5;
		((Control)wpChartType).Location = new Point(200, 0);
		((Control)wpChartType).Name = "wpChartType";
		((Control)wpChartType).Padding = new Padding(2);
		((Control)wpChartType).Size = new Size(568, 565);
		((Control)wpChartType).TabIndex = 1006;
		((Control)wpChartType).Text = "Chart Type";
		wpChartType.Visible = false;
		((Control)wpChartType).VisibleChanged += wpChartType_VisibleChanged;
		((ScrollableControl)scChartType).AutoScroll = true;
		((Control)scChartType).BackColor = SystemColors.Window;
		((Control)scChartType).Dock = (DockStyle)5;
		((Control)scChartType).Location = new Point(2, 2);
		((Control)scChartType).Name = "scChartType";
		scChartType.ScrollHeight = 1000;
		scChartType.ScrollWidth = 100;
		((Control)scChartType).Size = new Size(564, 561);
		((Control)scChartType).TabIndex = 0;
		((Control)scChartType).Paint += new PaintEventHandler(scChartType_Paint);
		((Control)scChartType).DoubleClick += ControlChartType_DoubleClick;
		((Control)scChartType).MouseDown += new MouseEventHandler(scChartType_MouseDown);
		((Control)wpStyle).BackColor = Color.White;
		((Control)wpStyle).Controls.Add((Control)(object)scChartStyle);
		wpStyle.Dock = (DockStyle)5;
		((Control)wpStyle).Location = new Point(200, 0);
		((Control)wpStyle).Name = "wpStyle";
		((Control)wpStyle).Padding = new Padding(2);
		((Control)wpStyle).Size = new Size(568, 565);
		((Control)wpStyle).TabIndex = 1007;
		((Control)wpStyle).Text = "Style";
		wpStyle.Visible = false;
		((Control)wpStyle).VisibleChanged += WpStyle_VisibleChanged;
		((Control)scChartStyle).BackColor = SystemColors.Window;
		((Control)scChartStyle).Dock = (DockStyle)5;
		((Control)scChartStyle).Location = new Point(2, 2);
		((Control)scChartStyle).Name = "scChartStyle";
		scChartStyle.ScrollHeight = 1000;
		scChartStyle.ScrollWidth = 100;
		((Control)scChartStyle).Size = new Size(564, 561);
		((Control)scChartStyle).TabIndex = 1;
		((Control)scChartStyle).Paint += new PaintEventHandler(ScChartStyle_Paint);
		((Control)scChartStyle).DoubleClick += ControlChartType_DoubleClick;
		((Control)scChartStyle).MouseDown += new MouseEventHandler(ScChartStyle_MouseDown);
		((Control)wpSeriesData).BackColor = Color.White;
		((Control)wpSeriesData).Controls.Add((Control)(object)stiTabControl1);
		((Control)wpSeriesData).Controls.Add((Control)(object)tbSeriesName);
		((Control)wpSeriesData).Controls.Add((Control)(object)labelSeriesName);
		((Control)wpSeriesData).Controls.Add((Control)(object)lbSeries);
		((Control)wpSeriesData).Controls.Add((Control)(object)tbTool);
		wpSeriesData.Dock = (DockStyle)5;
		((Control)wpSeriesData).Location = new Point(200, 0);
		((Control)wpSeriesData).Name = "wpSeriesData";
		((Control)wpSeriesData).Padding = new Padding(2);
		((Control)wpSeriesData).Size = new Size(568, 565);
		((Control)wpSeriesData).TabIndex = 1008;
		((Control)wpSeriesData).Text = "Series Data";
		wpSeriesData.Visible = false;
		((Control)wpSeriesData).VisibleChanged += WpDataSeries_VisibleChanged;
		((Control)stiTabControl1).Anchor = (AnchorStyles)15;
		((Control)stiTabControl1).Controls.Add((Control)(object)tabPageDataColumns);
		((Control)stiTabControl1).Controls.Add((Control)(object)tabPageListOfValues);
		((Control)stiTabControl1).Location = new Point(224, 72);
		((Control)stiTabControl1).Name = "stiTabControl1";
		stiTabControl1.SelectedIndex = 0;
		((Control)stiTabControl1).Size = new Size(334, 485);
		((Control)stiTabControl1).TabIndex = 1;
		((Control)tabPageDataColumns).BackColor = Color.White;
		((Control)tabPageDataColumns).Controls.Add((Control)(object)twBoxValues);
		((Control)tabPageDataColumns).Controls.Add((Control)(object)twBoxEndValues);
		((Control)tabPageDataColumns).Controls.Add((Control)(object)twBoxOpenValues);
		((Control)tabPageDataColumns).Controls.Add((Control)(object)twBoxCloseValues);
		((Control)tabPageDataColumns).Controls.Add((Control)(object)twBoxHighValues);
		((Control)tabPageDataColumns).Controls.Add((Control)(object)twBoxLowValues);
		((Control)tabPageDataColumns).Controls.Add((Control)(object)twBoxArguments);
		((Control)tabPageDataColumns).Controls.Add((Control)(object)labelValueDataColumn);
		((Control)tabPageDataColumns).Controls.Add((Control)(object)labelEndValueDataColumn);
		((Control)tabPageDataColumns).Controls.Add((Control)(object)labelOpenValueDataColumn);
		((Control)tabPageDataColumns).Controls.Add((Control)(object)labelCloseValueDataColumn);
		((Control)tabPageDataColumns).Controls.Add((Control)(object)labelHighValueDataColumn);
		((Control)tabPageDataColumns).Controls.Add((Control)(object)labelLowValueDataColumn);
		((Control)tabPageDataColumns).Controls.Add((Control)(object)labelArgumentDataColumn);
		((Control)tabPageDataColumns).Dock = (DockStyle)5;
		tabPageDataColumns.Location = new Point(4, 22);
		((Control)tabPageDataColumns).Name = "tabPageDataColumns";
		((Control)tabPageDataColumns).Padding = new Padding(4);
		((Control)tabPageDataColumns).Size = new Size(326, 459);
		tabPageDataColumns.TabIndex = 0;
		((Control)tabPageDataColumns).Text = "Data Columns";
		tabPageDataColumns.Visible = false;
		((Control)twBoxValues).Anchor = (AnchorStyles)13;
		((Control)twBoxValues).BackColor = SystemColors.Window;
		twBoxValues.Flat = false;
		((Control)twBoxValues).Location = new Point(152, 45);
		((Control)twBoxValues).Name = "twBoxValues";
		((Control)twBoxValues).Size = new Size(162, 21);
		((Control)twBoxValues).TabIndex = 1;
		twBoxValues.ValueChanged += TwBoxValues_ValueChanged;
		twBoxValues.TreeNodesFill += TwBoxArguments_TreeNodesFill;
		((Control)twBoxEndValues).Anchor = (AnchorStyles)13;
		((Control)twBoxEndValues).BackColor = SystemColors.Window;
		twBoxEndValues.Flat = false;
		((Control)twBoxEndValues).Location = new Point(152, 78);
		((Control)twBoxEndValues).Name = "twBoxEndValues";
		((Control)twBoxEndValues).Size = new Size(162, 21);
		((Control)twBoxEndValues).TabIndex = 2;
		twBoxEndValues.ValueChanged += TwBoxEndValues_ValueChanged;
		twBoxEndValues.TreeNodesFill += TwBoxArguments_TreeNodesFill;
		((Control)twBoxOpenValues).Anchor = (AnchorStyles)13;
		((Control)twBoxOpenValues).BackColor = SystemColors.Window;
		twBoxOpenValues.Flat = false;
		((Control)twBoxOpenValues).Location = new Point(152, 111);
		((Control)twBoxOpenValues).Name = "twBoxOpenValues";
		((Control)twBoxOpenValues).Size = new Size(162, 21);
		((Control)twBoxOpenValues).TabIndex = 3;
		twBoxOpenValues.ValueChanged += TwBoxOpenValues_ValueChanged;
		twBoxOpenValues.TreeNodesFill += TwBoxArguments_TreeNodesFill;
		((Control)twBoxCloseValues).Anchor = (AnchorStyles)13;
		((Control)twBoxCloseValues).BackColor = SystemColors.Window;
		twBoxCloseValues.Flat = false;
		((Control)twBoxCloseValues).Location = new Point(152, 144);
		((Control)twBoxCloseValues).Name = "twBoxCloseValues";
		((Control)twBoxCloseValues).Size = new Size(162, 21);
		((Control)twBoxCloseValues).TabIndex = 4;
		twBoxCloseValues.ValueChanged += TwBoxCloseValues_ValueChanged;
		twBoxCloseValues.TreeNodesFill += TwBoxArguments_TreeNodesFill;
		((Control)twBoxHighValues).Anchor = (AnchorStyles)13;
		((Control)twBoxHighValues).BackColor = SystemColors.Window;
		twBoxHighValues.Flat = false;
		((Control)twBoxHighValues).Location = new Point(152, 177);
		((Control)twBoxHighValues).Name = "twBoxHighValues";
		((Control)twBoxHighValues).Size = new Size(162, 21);
		((Control)twBoxHighValues).TabIndex = 5;
		twBoxHighValues.ValueChanged += TwBoxHighValues_ValueChanged;
		twBoxHighValues.TreeNodesFill += TwBoxArguments_TreeNodesFill;
		((Control)twBoxLowValues).Anchor = (AnchorStyles)13;
		((Control)twBoxLowValues).BackColor = SystemColors.Window;
		twBoxLowValues.Flat = false;
		((Control)twBoxLowValues).Location = new Point(152, 210);
		((Control)twBoxLowValues).Name = "twBoxLowValues";
		((Control)twBoxLowValues).Size = new Size(162, 21);
		((Control)twBoxLowValues).TabIndex = 6;
		twBoxLowValues.ValueChanged += TwBoxLowValues_ValueChanged;
		twBoxLowValues.TreeNodesFill += TwBoxArguments_TreeNodesFill;
		((Control)twBoxArguments).Anchor = (AnchorStyles)13;
		((Control)twBoxArguments).BackColor = SystemColors.Window;
		((Control)twBoxArguments).Cursor = Cursors.Default;
		twBoxArguments.Flat = false;
		((Control)twBoxArguments).ForeColor = SystemColors.ControlText;
		((Control)twBoxArguments).Location = new Point(152, 12);
		((Control)twBoxArguments).Name = "twBoxArguments";
		((Control)twBoxArguments).Size = new Size(162, 21);
		((Control)twBoxArguments).TabIndex = 0;
		twBoxArguments.ValueChanged += TwBoxArguments_ValueChanged;
		twBoxArguments.TreeNodesFill += TwBoxArguments_TreeNodesFill;
		((Control)labelValueDataColumn).ForeColor = Color.DimGray;
		((Control)labelValueDataColumn).Location = new Point(12, 45);
		((Control)labelValueDataColumn).Name = "labelValueDataColumn";
		((Control)labelValueDataColumn).Size = new Size(138, 21);
		((Control)labelValueDataColumn).TabIndex = 4;
		((Control)labelValueDataColumn).Text = "Value Data Column";
		labelValueDataColumn.TextAlign = (ContentAlignment)16;
		((Control)labelEndValueDataColumn).ForeColor = Color.DimGray;
		((Control)labelEndValueDataColumn).Location = new Point(12, 78);
		((Control)labelEndValueDataColumn).Name = "labelEndValueDataColumn";
		((Control)labelEndValueDataColumn).Size = new Size(138, 21);
		((Control)labelEndValueDataColumn).TabIndex = 4;
		((Control)labelEndValueDataColumn).Text = "End Value Data Column";
		labelEndValueDataColumn.TextAlign = (ContentAlignment)16;
		((Control)labelOpenValueDataColumn).ForeColor = Color.DimGray;
		((Control)labelOpenValueDataColumn).Location = new Point(12, 111);
		((Control)labelOpenValueDataColumn).Name = "labelOpenValueDataColumn";
		((Control)labelOpenValueDataColumn).Size = new Size(138, 21);
		((Control)labelOpenValueDataColumn).TabIndex = 4;
		((Control)labelOpenValueDataColumn).Text = "Open Value Data Column";
		labelOpenValueDataColumn.TextAlign = (ContentAlignment)16;
		((Control)labelCloseValueDataColumn).ForeColor = Color.DimGray;
		((Control)labelCloseValueDataColumn).Location = new Point(12, 144);
		((Control)labelCloseValueDataColumn).Name = "labelCloseValueDataColumn";
		((Control)labelCloseValueDataColumn).Size = new Size(138, 21);
		((Control)labelCloseValueDataColumn).TabIndex = 4;
		((Control)labelCloseValueDataColumn).Text = "Close Value Data Column";
		labelCloseValueDataColumn.TextAlign = (ContentAlignment)16;
		((Control)labelHighValueDataColumn).ForeColor = Color.DimGray;
		((Control)labelHighValueDataColumn).Location = new Point(12, 177);
		((Control)labelHighValueDataColumn).Name = "labelHighValueDataColumn";
		((Control)labelHighValueDataColumn).Size = new Size(138, 21);
		((Control)labelHighValueDataColumn).TabIndex = 4;
		((Control)labelHighValueDataColumn).Text = "High Value Data Column";
		labelHighValueDataColumn.TextAlign = (ContentAlignment)16;
		((Control)labelLowValueDataColumn).ForeColor = Color.DimGray;
		((Control)labelLowValueDataColumn).Location = new Point(12, 210);
		((Control)labelLowValueDataColumn).Name = "labelLowValueDataColumn";
		((Control)labelLowValueDataColumn).Size = new Size(138, 21);
		((Control)labelLowValueDataColumn).TabIndex = 4;
		((Control)labelLowValueDataColumn).Text = "Low Value Data Column";
		labelLowValueDataColumn.TextAlign = (ContentAlignment)16;
		((Control)labelArgumentDataColumn).ForeColor = Color.DimGray;
		((Control)labelArgumentDataColumn).Location = new Point(12, 12);
		((Control)labelArgumentDataColumn).Name = "labelArgumentDataColumn";
		((Control)labelArgumentDataColumn).Size = new Size(138, 21);
		((Control)labelArgumentDataColumn).TabIndex = 2;
		((Control)labelArgumentDataColumn).Text = "Argument Data Column";
		labelArgumentDataColumn.TextAlign = (ContentAlignment)16;
		((Control)tabPageListOfValues).BackColor = Color.White;
		((Control)tabPageListOfValues).Controls.Add((Control)(object)dgPoints);
		((Control)tabPageListOfValues).Dock = (DockStyle)5;
		tabPageListOfValues.Location = new Point(4, 22);
		((Control)tabPageListOfValues).Name = "tabPageListOfValues";
		((Control)tabPageListOfValues).Padding = new Padding(4);
		((Control)tabPageListOfValues).Size = new Size(326, 459);
		tabPageListOfValues.TabIndex = 1;
		((Control)tabPageListOfValues).Text = "List of Values";
		tabPageListOfValues.Visible = false;
		((Control)dgPoints).Anchor = (AnchorStyles)15;
		dgPoints.BorderStyle = (BorderStyle)0;
		dgPoints.DataMember = "";
		dgPoints.DataSource = dtPoints;
		((Control)dgPoints).Location = new Point(12, 12);
		((Control)dgPoints).Name = "dgPoints";
		((Control)dgPoints).Size = new Size(302, 433);
		((Control)dgPoints).TabIndex = 0;
		dgPoints.AllowUserToResizeRows = false;
		dgPoints.AllowUserToResizeColumns = false;
		dgPoints.AllowUserToOrderColumns = false;
		dgPoints.AllowUserToDeleteRows = false;
		dgPoints.AllowUserToAddRows = false;
		dgPoints.ColumnHeadersHeight = 16;
		dtPoints.CaseSensitive = true;
		dtPoints.TableName = "Points Table";
		dtPoints.RowDeleted += dtPoints_RowChanged;
		((Control)tbSeriesName).Anchor = (AnchorStyles)13;
		((Control)tbSeriesName).BackColor = Color.White;
		((Control)tbSeriesName).Location = new Point(376, 40);
		((Control)tbSeriesName).Name = "tbSeriesName";
		((Control)tbSeriesName).Size = new Size(178, 21);
		((Control)tbSeriesName).TabIndex = 0;
		((Control)tbSeriesName).TextChanged += tbSeriesName_TextChanged;
		((Control)labelSeriesName).ForeColor = Color.DimGray;
		((Control)labelSeriesName).Location = new Point(228, 40);
		((Control)labelSeriesName).Name = "labelSeriesName";
		((Control)labelSeriesName).Size = new Size(144, 20);
		((Control)labelSeriesName).TabIndex = 3;
		((Control)labelSeriesName).Text = "Series name";
		labelSeriesName.TextAlign = (ContentAlignment)16;
		lbSeries.BorderStyle = (BorderStyle)0;
		lbSeries.IntegralHeight = false;
		((Control)lbSeries).Location = new Point(6, 36);
		((Control)lbSeries).Name = "lbSeries";
		((Control)lbSeries).Size = new Size(212, 467);
		((Control)lbSeries).TabIndex = 2;
		lbSeries.SelectedIndexChanged += lbSeries_SelectedIndexChanged;
		((Control)tbTool).Controls.Add((Control)(object)tbNewDataSource);
		((Control)tbTool).Controls.Add((Control)(object)tbMoveDown);
		((Control)tbTool).Controls.Add((Control)(object)tbMoveUp);
		((Control)tbTool).Controls.Add((Control)(object)stiToolButton3);
		((Control)tbTool).Controls.Add((Control)(object)tbRemove);
		((Control)tbTool).Controls.Add((Control)(object)tbAddSeries);
		tbTool.ControlStyle = StiControlStyle.Office2013Blue;
		((Control)tbTool).Dock = (DockStyle)1;
		tbTool.LineStyle = StiToolBar.StiToolBarLineStyle.Bottom;
		((Control)tbTool).Location = new Point(2, 2);
		((Control)tbTool).Name = "tbTool";
		((Control)tbTool).Padding = new Padding(6, 3, 3, 4);
		((Control)tbTool).Size = new Size(564, 28);
		((Control)tbTool).TabIndex = 3;
		((Control)tbTool).Text = "stiToolBar1";
		((Control)tbNewDataSource).AutoSize = true;
		((Control)tbNewDataSource).Dock = (DockStyle)4;
		tbNewDataSource.Draw3DButton = false;
		tbNewDataSource.DropDownMenu = null;
		tbNewDataSource.HotDragDrop = false;
		((ButtonBase)tbNewDataSource).ImageIndex = 4;
		((ButtonBase)tbNewDataSource).ImageList = ilData;
		((Control)tbNewDataSource).Location = new Point(443, 3);
		((Control)tbNewDataSource).Name = "tbNewDataSource";
		((Control)tbNewDataSource).Size = new Size(118, 21);
		((Control)tbNewDataSource).TabIndex = 8;
		((Control)tbNewDataSource).Text = "New Data Source";
		((ButtonBase)tbNewDataSource).UseVisualStyleBackColor = false;
		((Control)tbNewDataSource).Click += ButtonNewDataSource_Click;
		((Control)tbMoveDown).Dock = (DockStyle)3;
		tbMoveDown.Draw3DButton = false;
		tbMoveDown.DropDownMenu = null;
		tbMoveDown.HotDragDrop = false;
		((ButtonBase)tbMoveDown).ImageIndex = 1;
		((ButtonBase)tbMoveDown).ImageList = ilData;
		((Control)tbMoveDown).Location = new Point(94, 3);
		((Control)tbMoveDown).Name = "tbMoveDown";
		((Control)tbMoveDown).Size = new Size(22, 21);
		((Control)tbMoveDown).TabIndex = 4;
		((ButtonBase)tbMoveDown).UseVisualStyleBackColor = false;
		((Control)tbMoveDown).Click += btMoveDown_Click;
		((Control)tbMoveUp).Dock = (DockStyle)3;
		tbMoveUp.Draw3DButton = false;
		tbMoveUp.DropDownMenu = null;
		tbMoveUp.HotDragDrop = false;
		((ButtonBase)tbMoveUp).ImageIndex = 0;
		((ButtonBase)tbMoveUp).ImageList = ilData;
		((Control)tbMoveUp).Location = new Point(72, 3);
		((Control)tbMoveUp).Name = "tbMoveUp";
		((Control)tbMoveUp).Size = new Size(22, 21);
		((Control)tbMoveUp).TabIndex = 2;
		((ButtonBase)tbMoveUp).UseVisualStyleBackColor = false;
		((Control)tbMoveUp).Click += BtMoveUp_Click;
		((Control)stiToolButton3).Dock = (DockStyle)3;
		stiToolButton3.Draw3DButton = false;
		stiToolButton3.DropDownMenu = null;
		stiToolButton3.HotDragDrop = false;
		((Control)stiToolButton3).Location = new Point(64, 3);
		((Control)stiToolButton3).Name = "stiToolButton3";
		((Control)stiToolButton3).Size = new Size(8, 21);
		stiToolButton3.Style = StiToolBarButtonStyle.Separator;
		((Control)stiToolButton3).TabIndex = 7;
		((ButtonBase)stiToolButton3).UseVisualStyleBackColor = false;
		((Control)tbRemove).Dock = (DockStyle)3;
		tbRemove.Draw3DButton = false;
		tbRemove.DropDownMenu = null;
		tbRemove.HotDragDrop = false;
		((ButtonBase)tbRemove).ImageIndex = 3;
		((ButtonBase)tbRemove).ImageList = ilData;
		((Control)tbRemove).Location = new Point(42, 3);
		((Control)tbRemove).Name = "tbRemove";
		((Control)tbRemove).Size = new Size(22, 21);
		((Control)tbRemove).TabIndex = 6;
		((ButtonBase)tbRemove).UseVisualStyleBackColor = false;
		((Control)tbRemove).Click += BtRemove_Click;
		((Control)tbAddSeries).Dock = (DockStyle)3;
		tbAddSeries.Draw3DButton = false;
		tbAddSeries.DropDownMenu = cmSeries;
		tbAddSeries.HotDragDrop = false;
		((ButtonBase)tbAddSeries).ImageAlign = (ContentAlignment)16;
		((ButtonBase)tbAddSeries).ImageIndex = 2;
		((ButtonBase)tbAddSeries).ImageList = ilData;
		((Control)tbAddSeries).Location = new Point(6, 3);
		((Control)tbAddSeries).Name = "tbAddSeries";
		((Control)tbAddSeries).Size = new Size(36, 21);
		((Control)tbAddSeries).TabIndex = 5;
		((ButtonBase)tbAddSeries).UseVisualStyleBackColor = false;
		((ToolStripDropDown)cmSeries).Opening += CmSeries_Popup;
		((Control)wpLabels).BackColor = Color.White;
		((Control)wpLabels).Controls.Add((Control)(object)scrollableLabels);
		((Control)wpLabels).Controls.Add((Control)(object)gbLabels);
		wpLabels.Dock = (DockStyle)5;
		((Control)wpLabels).Location = new Point(200, 0);
		((Control)wpLabels).Name = "wpLabels";
		((Control)wpLabels).Padding = new Padding(2);
		((Control)wpLabels).Size = new Size(568, 565);
		((Control)wpLabels).TabIndex = 1010;
		((Control)wpLabels).Text = "Labels";
		wpLabels.Visible = false;
		((Control)wpLabels).VisibleChanged += wpLabels_VisibleChanged;
		((Control)scrollableLabels).Anchor = (AnchorStyles)15;
		((Control)scrollableLabels).BackColor = SystemColors.Window;
		((Control)scrollableLabels).Location = new Point(2, 2);
		((Control)scrollableLabels).Name = "scrollableLabels";
		scrollableLabels.ScrollHeight = 1000;
		scrollableLabels.ScrollWidth = 50;
		((Control)scrollableLabels).Size = new Size(326, 561);
		((Control)scrollableLabels).TabIndex = 4;
		((Control)scrollableLabels).Paint += new PaintEventHandler(scrollableLabels_Paint);
		((Control)scrollableLabels).MouseDown += new MouseEventHandler(scrollableLabels_MouseDown);
		((Control)gbLabels).Anchor = (AnchorStyles)11;
		((Control)gbLabels).Controls.Add((Control)(object)stiGroupLine1);
		((Control)gbLabels).Controls.Add((Control)(object)labelValueType);
		((Control)gbLabels).Controls.Add((Control)(object)cbxAutoRotation);
		((Control)gbLabels).Controls.Add((Control)(object)lbAngle);
		((Control)gbLabels).Controls.Add((Control)(object)udLabelsAngle);
		((Control)gbLabels).Controls.Add((Control)(object)tbTextAfter);
		((Control)gbLabels).Controls.Add((Control)(object)lbTextBefore);
		((Control)gbLabels).Controls.Add((Control)(object)tbTextBefore);
		((Control)gbLabels).Controls.Add((Control)(object)lbTextAfter);
		((Control)gbLabels).Controls.Add((Control)(object)cbValueType);
		gbLabels.HeaderHeight = 24;
		((Control)gbLabels).Location = new Point(332, 0);
		((Control)gbLabels).Name = "gbLabels";
		((Control)gbLabels).Size = new Size(230, 559);
		((Control)gbLabels).TabIndex = 1;
		((Control)gbLabels).TabStop = false;
		((Control)gbLabels).Text = "Labels";
		((Control)stiGroupLine1).Anchor = (AnchorStyles)13;
		stiGroupLine1.LinePosition = StiLinePosition.Top;
		stiGroupLine1.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine1).Location = new Point(4, 168);
		((Control)stiGroupLine1).Name = "stiGroupLine1";
		((Control)stiGroupLine1).Size = new Size(222, 8);
		((Control)stiGroupLine1).TabIndex = 4;
		((Control)labelValueType).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)labelValueType).ForeColor = Color.DimGray;
		((Control)labelValueType).Location = new Point(12, 36);
		((Control)labelValueType).Name = "labelValueType";
		((Control)labelValueType).Size = new Size(96, 20);
		((Control)labelValueType).TabIndex = 19;
		((Control)labelValueType).Text = "Value Type";
		labelValueType.TextAlign = (ContentAlignment)16;
		((Control)cbxAutoRotation).Anchor = (AnchorStyles)13;
		((Control)cbxAutoRotation).Location = new Point(12, 182);
		((Control)cbxAutoRotation).Name = "cbxAutoRotation";
		((Control)cbxAutoRotation).Size = new Size(206, 16);
		((Control)cbxAutoRotation).TabIndex = 4;
		((Control)cbxAutoRotation).Text = "Auto rotation";
		((Control)cbxAutoRotation).Click += cbxLabelsVisible_Click;
		((Control)lbAngle).ForeColor = Color.DimGray;
		((Control)lbAngle).Location = new Point(12, 135);
		((Control)lbAngle).Name = "lbAngle";
		((Control)lbAngle).Size = new Size(90, 20);
		((Control)lbAngle).TabIndex = 4;
		((Control)lbAngle).Text = "Angle";
		lbAngle.TextAlign = (ContentAlignment)16;
		((Control)udLabelsAngle).Anchor = (AnchorStyles)13;
		((Control)udLabelsAngle).BackColor = SystemColors.Window;
		((Control)udLabelsAngle).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)udLabelsAngle).Location = new Point(112, 135);
		((Control)udLabelsAngle).Name = "udLabelsAngle";
		((Control)udLabelsAngle).Size = new Size(107, 21);
		((Control)udLabelsAngle).TabIndex = 3;
		udLabelsAngle.ValueChanged += cbxLabelsVisible_Click;
		((Control)tbTextAfter).Anchor = (AnchorStyles)13;
		((Control)tbTextAfter).BackColor = Color.White;
		((Control)tbTextAfter).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)tbTextAfter).Location = new Point(112, 102);
		((Control)tbTextAfter).Name = "tbTextAfter";
		((Control)tbTextAfter).Size = new Size(107, 21);
		((Control)tbTextAfter).TabIndex = 2;
		((Control)tbTextAfter).TextChanged += cbxLabelsVisible_Click;
		((Control)lbTextBefore).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)lbTextBefore).ForeColor = Color.DimGray;
		((Control)lbTextBefore).Location = new Point(12, 69);
		((Control)lbTextBefore).Name = "lbTextBefore";
		((Control)lbTextBefore).Size = new Size(98, 20);
		((Control)lbTextBefore).TabIndex = 1;
		((Control)lbTextBefore).Text = "Text before";
		lbTextBefore.TextAlign = (ContentAlignment)16;
		((Control)tbTextBefore).Anchor = (AnchorStyles)13;
		((Control)tbTextBefore).BackColor = Color.White;
		((Control)tbTextBefore).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)tbTextBefore).Location = new Point(112, 69);
		((Control)tbTextBefore).Name = "tbTextBefore";
		((Control)tbTextBefore).Size = new Size(107, 21);
		((Control)tbTextBefore).TabIndex = 1;
		((Control)tbTextBefore).TextChanged += cbxLabelsVisible_Click;
		((Control)lbTextAfter).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)lbTextAfter).ForeColor = Color.DimGray;
		((Control)lbTextAfter).Location = new Point(12, 102);
		((Control)lbTextAfter).Name = "lbTextAfter";
		((Control)lbTextAfter).Size = new Size(98, 20);
		((Control)lbTextAfter).TabIndex = 3;
		((Control)lbTextAfter).Text = "Text after";
		lbTextAfter.TextAlign = (ContentAlignment)16;
		((Control)cbValueType).Anchor = (AnchorStyles)13;
		cbValueType.DropDownStyle = (ComboBoxStyle)2;
		((ComboBox)cbValueType).Items.AddRange(new object[7] { "Value", "Series Title", "Argument", "Value - Argument", "Argument - Value", "SeriesTitle - Value", "SeriesTitle - Argument" });
		((Control)cbValueType).Location = new Point(112, 36);
		((Control)cbValueType).Name = "cbValueType";
		((Control)cbValueType).Size = new Size(107, 21);
		((Control)cbValueType).TabIndex = 0;
		((ComboBox)cbValueType).SelectedIndexChanged += cbxLabelsVisible_Click;
		((Control)wpAxes).BackColor = Color.White;
		((Control)wpAxes).Controls.Add((Control)(object)scAxes);
		((Control)wpAxes).Controls.Add((Control)(object)tcAxes);
		wpAxes.Dock = (DockStyle)5;
		((Control)wpAxes).Location = new Point(200, 0);
		((Control)wpAxes).Name = "wpAxes";
		((Control)wpAxes).Padding = new Padding(2);
		((Control)wpAxes).Size = new Size(568, 565);
		((Control)wpAxes).TabIndex = 1012;
		((Control)wpAxes).Text = "Axes";
		wpAxes.Visible = false;
		((Control)wpAxes).VisibleChanged += wpAxes_VisibleChanged;
		((Control)scAxes).Anchor = (AnchorStyles)15;
		((Control)scAxes).BackColor = SystemColors.Window;
		((Control)scAxes).Location = new Point(2, 2);
		((Control)scAxes).Name = "scAxes";
		scAxes.ScrollHeight = 100;
		scAxes.ScrollWidth = 100;
		((Control)scAxes).Size = new Size(326, 561);
		((Control)scAxes).TabIndex = 4;
		((Control)scAxes).Paint += new PaintEventHandler(scAxes_Paint);
		((Control)tcAxes).Anchor = (AnchorStyles)11;
		((Control)tcAxes).Controls.Add((Control)(object)tpAxisX);
		((Control)tcAxes).Controls.Add((Control)(object)tpAxisY);
		((Control)tcAxes).Location = new Point(330, 2);
		((Control)tcAxes).Name = "tcAxes";
		tcAxes.SelectedIndex = 0;
		((Control)tcAxes).Size = new Size(236, 561);
		((Control)tcAxes).TabIndex = 3;
		((Control)tpAxisX).BackColor = Color.White;
		((Control)tpAxisX).Controls.Add((Control)(object)stiGroupLine2);
		((Control)tpAxisX).Controls.Add((Control)(object)gbLabelsX);
		((Control)tpAxisX).Controls.Add((Control)(object)gbGridLinesX);
		((Control)tpAxisX).Controls.Add((Control)(object)gbTicksX);
		((Control)tpAxisX).Controls.Add((Control)(object)gbTitleX);
		((Control)tpAxisX).Controls.Add((Control)(object)cbxReverseX);
		((Control)tpAxisX).Controls.Add((Control)(object)cbxVisibleX);
		((Control)tpAxisX).Dock = (DockStyle)5;
		tpAxisX.Location = new Point(4, 22);
		((Control)tpAxisX).Name = "tpAxisX";
		((Control)tpAxisX).Padding = new Padding(4);
		((Control)tpAxisX).Size = new Size(228, 535);
		tpAxisX.TabIndex = 0;
		((Control)tpAxisX).Text = "Axis X";
		tpAxisX.Visible = false;
		((Control)stiGroupLine2).Dock = (DockStyle)1;
		stiGroupLine2.LinePosition = StiLinePosition.Top;
		stiGroupLine2.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine2).Location = new Point(4, 412);
		((Control)stiGroupLine2).Name = "stiGroupLine2";
		((Control)stiGroupLine2).Size = new Size(220, 8);
		((Control)stiGroupLine2).TabIndex = 6;
		((Control)gbLabelsX).Controls.Add((Control)(object)udAngleX);
		((Control)gbLabelsX).Controls.Add((Control)(object)lbAngleX);
		((Control)gbLabelsX).Controls.Add((Control)(object)cbLabelsX);
		((Control)gbLabelsX).Dock = (DockStyle)1;
		gbLabelsX.HeaderHeight = 24;
		((Control)gbLabelsX).Location = new Point(4, 311);
		((Control)gbLabelsX).Name = "gbLabelsX";
		((Control)gbLabelsX).Size = new Size(220, 101);
		((Control)gbLabelsX).TabIndex = 3;
		((Control)gbLabelsX).TabStop = false;
		((Control)gbLabelsX).Text = "Labels  ";
		((Control)udAngleX).Anchor = (AnchorStyles)13;
		((Control)udAngleX).BackColor = SystemColors.Window;
		((Control)udAngleX).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)udAngleX).Location = new Point(120, 69);
		((Control)udAngleX).Name = "udAngleX";
		((Control)udAngleX).Size = new Size(88, 21);
		((Control)udAngleX).TabIndex = 2;
		udAngleX.ValueChanged += cbxVisibleX_Click;
		((Control)lbAngleX).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)lbAngleX).ForeColor = Color.DimGray;
		((Control)lbAngleX).Location = new Point(12, 69);
		((Control)lbAngleX).Name = "lbAngleX";
		((Control)lbAngleX).Size = new Size(108, 21);
		((Control)lbAngleX).TabIndex = 1;
		((Control)lbAngleX).Text = "Angle";
		lbAngleX.TextAlign = (ContentAlignment)16;
		((Control)cbLabelsX).Anchor = (AnchorStyles)13;
		cbLabelsX.DropDownStyle = (ComboBoxStyle)2;
		((ComboBox)cbLabelsX).Items.AddRange(new object[3] { "None", "One Line", "Two Lines" });
		((Control)cbLabelsX).Location = new Point(12, 36);
		((Control)cbLabelsX).Name = "cbLabelsX";
		((Control)cbLabelsX).Size = new Size(196, 21);
		((Control)cbLabelsX).TabIndex = 0;
		((ComboBox)cbLabelsX).SelectedIndexChanged += cbxVisibleX_Click;
		((Control)gbGridLinesX).Controls.Add((Control)(object)cbxInterlacedX);
		((Control)gbGridLinesX).Controls.Add((Control)(object)cbGridLinesX);
		((Control)gbGridLinesX).Dock = (DockStyle)1;
		gbGridLinesX.HeaderHeight = 24;
		((Control)gbGridLinesX).Location = new Point(4, 212);
		((Control)gbGridLinesX).Name = "gbGridLinesX";
		((Control)gbGridLinesX).Size = new Size(220, 99);
		((Control)gbGridLinesX).TabIndex = 2;
		((Control)gbGridLinesX).TabStop = false;
		((Control)gbGridLinesX).Text = "Grid lines  ";
		((Control)cbxInterlacedX).Anchor = (AnchorStyles)13;
		((Control)cbxInterlacedX).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)cbxInterlacedX).Location = new Point(12, 71);
		((Control)cbxInterlacedX).Name = "cbxInterlacedX";
		((Control)cbxInterlacedX).Size = new Size(196, 16);
		((Control)cbxInterlacedX).TabIndex = 1;
		((Control)cbxInterlacedX).Text = "Interlaced";
		((Control)cbxInterlacedX).Click += cbxVisibleX_Click;
		((Control)cbGridLinesX).Anchor = (AnchorStyles)13;
		cbGridLinesX.DropDownStyle = (ComboBoxStyle)2;
		((ComboBox)cbGridLinesX).Items.AddRange(new object[3] { "None", "Major", "All" });
		((Control)cbGridLinesX).Location = new Point(12, 36);
		((Control)cbGridLinesX).Name = "cbGridLinesX";
		((Control)cbGridLinesX).Size = new Size(196, 21);
		((Control)cbGridLinesX).TabIndex = 0;
		((ComboBox)cbGridLinesX).SelectedIndexChanged += cbxVisibleX_Click;
		((Control)gbTicksX).Controls.Add((Control)(object)udMinorCountX);
		((Control)gbTicksX).Controls.Add((Control)(object)lbMinorCountX);
		((Control)gbTicksX).Controls.Add((Control)(object)cbTicksX);
		((Control)gbTicksX).Dock = (DockStyle)1;
		gbTicksX.HeaderHeight = 24;
		((Control)gbTicksX).Location = new Point(4, 108);
		((Control)gbTicksX).Name = "gbTicksX";
		((Control)gbTicksX).Size = new Size(220, 104);
		((Control)gbTicksX).TabIndex = 1;
		((Control)gbTicksX).TabStop = false;
		((Control)gbTicksX).Text = "Ticks  ";
		((Control)udMinorCountX).Anchor = (AnchorStyles)13;
		((Control)udMinorCountX).BackColor = SystemColors.Window;
		((Control)udMinorCountX).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)udMinorCountX).Location = new Point(120, 69);
		((Control)udMinorCountX).Name = "udMinorCountX";
		((Control)udMinorCountX).Size = new Size(88, 21);
		((Control)udMinorCountX).TabIndex = 2;
		udMinorCountX.ValueChanged += cbxVisibleX_Click;
		((Control)lbMinorCountX).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)lbMinorCountX).ForeColor = Color.DimGray;
		((Control)lbMinorCountX).Location = new Point(12, 69);
		((Control)lbMinorCountX).Name = "lbMinorCountX";
		((Control)lbMinorCountX).Size = new Size(108, 21);
		((Control)lbMinorCountX).TabIndex = 1;
		((Control)lbMinorCountX).Text = "Minor count";
		lbMinorCountX.TextAlign = (ContentAlignment)16;
		((Control)cbTicksX).Anchor = (AnchorStyles)13;
		cbTicksX.DropDownStyle = (ComboBoxStyle)2;
		((ComboBox)cbTicksX).Items.AddRange(new object[3] { "None", "Major", "All" });
		((Control)cbTicksX).Location = new Point(12, 36);
		((Control)cbTicksX).Name = "cbTicksX";
		((Control)cbTicksX).Size = new Size(196, 21);
		((Control)cbTicksX).TabIndex = 0;
		((ComboBox)cbTicksX).SelectedIndexChanged += cbxVisibleX_Click;
		((Control)gbTitleX).Controls.Add((Control)(object)cbAligmentX);
		((Control)gbTitleX).Controls.Add((Control)(object)lbAligmentX);
		((Control)gbTitleX).Controls.Add((Control)(object)tbTitleX);
		((Control)gbTitleX).Dock = (DockStyle)1;
		gbTitleX.HeaderHeight = 24;
		((Control)gbTitleX).Location = new Point(4, 4);
		((Control)gbTitleX).Name = "gbTitleX";
		((Control)gbTitleX).Size = new Size(220, 104);
		((Control)gbTitleX).TabIndex = 0;
		((Control)gbTitleX).TabStop = false;
		((Control)gbTitleX).Text = "Title  ";
		((Control)cbAligmentX).Anchor = (AnchorStyles)13;
		cbAligmentX.DropDownStyle = (ComboBoxStyle)2;
		((ComboBox)cbAligmentX).Items.AddRange(new object[3] { "Near", "Center", "Far" });
		((Control)cbAligmentX).Location = new Point(120, 69);
		((Control)cbAligmentX).Name = "cbAligmentX";
		((Control)cbAligmentX).Size = new Size(88, 21);
		((Control)cbAligmentX).TabIndex = 2;
		((ComboBox)cbAligmentX).SelectedIndexChanged += cbxVisibleX_Click;
		((Control)lbAligmentX).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)lbAligmentX).ForeColor = Color.DimGray;
		((Control)lbAligmentX).Location = new Point(12, 69);
		((Control)lbAligmentX).Name = "lbAligmentX";
		((Control)lbAligmentX).Size = new Size(102, 20);
		((Control)lbAligmentX).TabIndex = 1;
		((Control)lbAligmentX).Text = "Aligment";
		lbAligmentX.TextAlign = (ContentAlignment)16;
		((Control)tbTitleX).Anchor = (AnchorStyles)13;
		((Control)tbTitleX).BackColor = Color.White;
		((Control)tbTitleX).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)tbTitleX).Location = new Point(12, 36);
		((Control)tbTitleX).Name = "tbTitleX";
		((Control)tbTitleX).Size = new Size(196, 21);
		((Control)tbTitleX).TabIndex = 0;
		((Control)tbTitleX).TextChanged += cbxVisibleX_Click;
		((Control)cbxReverseX).Anchor = (AnchorStyles)13;
		((Control)cbxReverseX).Location = new Point(16, 451);
		((Control)cbxReverseX).Name = "cbxReverseX";
		((Control)cbxReverseX).Size = new Size(200, 16);
		((Control)cbxReverseX).TabIndex = 5;
		((Control)cbxReverseX).Text = "Reverse";
		((Control)cbxReverseX).Click += cbxVisibleX_Click;
		((Control)cbxVisibleX).Anchor = (AnchorStyles)13;
		((Control)cbxVisibleX).Location = new Point(16, 427);
		((Control)cbxVisibleX).Name = "cbxVisibleX";
		((Control)cbxVisibleX).Size = new Size(200, 16);
		((Control)cbxVisibleX).TabIndex = 4;
		((Control)cbxVisibleX).Text = "Visible";
		((Control)cbxVisibleX).Click += cbxVisibleX_Click;
		((Control)tpAxisY).BackColor = Color.White;
		((Control)tpAxisY).Controls.Add((Control)(object)stiGroupLine3);
		((Control)tpAxisY).Controls.Add((Control)(object)gbLabelsY);
		((Control)tpAxisY).Controls.Add((Control)(object)gbGridLinesY);
		((Control)tpAxisY).Controls.Add((Control)(object)gbTicksY);
		((Control)tpAxisY).Controls.Add((Control)(object)gbTitleY);
		((Control)tpAxisY).Controls.Add((Control)(object)cbxReverseY);
		((Control)tpAxisY).Controls.Add((Control)(object)cbxVisibleY);
		((Control)tpAxisY).Dock = (DockStyle)5;
		tpAxisY.Location = new Point(4, 22);
		((Control)tpAxisY).Name = "tpAxisY";
		((Control)tpAxisY).Padding = new Padding(4);
		((Control)tpAxisY).Size = new Size(228, 535);
		tpAxisY.TabIndex = 1;
		((Control)tpAxisY).Text = "Axis Y";
		tpAxisY.Visible = false;
		((Control)stiGroupLine3).Dock = (DockStyle)1;
		stiGroupLine3.LinePosition = StiLinePosition.Top;
		stiGroupLine3.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine3).Location = new Point(4, 412);
		((Control)stiGroupLine3).Name = "stiGroupLine3";
		((Control)stiGroupLine3).Size = new Size(220, 8);
		((Control)stiGroupLine3).TabIndex = 7;
		((Control)gbLabelsY).Controls.Add((Control)(object)udAngleY);
		((Control)gbLabelsY).Controls.Add((Control)(object)lbAngleY);
		((Control)gbLabelsY).Controls.Add((Control)(object)cbLabelsY);
		((Control)gbLabelsY).Dock = (DockStyle)1;
		gbLabelsY.HeaderHeight = 24;
		((Control)gbLabelsY).Location = new Point(4, 311);
		((Control)gbLabelsY).Name = "gbLabelsY";
		((Control)gbLabelsY).Size = new Size(220, 101);
		((Control)gbLabelsY).TabIndex = 3;
		((Control)gbLabelsY).TabStop = false;
		((Control)gbLabelsY).Text = "Labels  ";
		((Control)udAngleY).Anchor = (AnchorStyles)13;
		((Control)udAngleY).BackColor = SystemColors.Window;
		((Control)udAngleY).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)udAngleY).Location = new Point(120, 69);
		((Control)udAngleY).Name = "udAngleY";
		((Control)udAngleY).Size = new Size(88, 21);
		((Control)udAngleY).TabIndex = 2;
		udAngleY.ValueChanged += cbxVisibleY_Click;
		((Control)lbAngleY).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)lbAngleY).ForeColor = Color.DimGray;
		((Control)lbAngleY).Location = new Point(12, 69);
		((Control)lbAngleY).Name = "lbAngleY";
		((Control)lbAngleY).Size = new Size(102, 20);
		((Control)lbAngleY).TabIndex = 1;
		((Control)lbAngleY).Text = "Angle";
		lbAngleY.TextAlign = (ContentAlignment)16;
		((Control)cbLabelsY).Anchor = (AnchorStyles)13;
		cbLabelsY.DropDownStyle = (ComboBoxStyle)2;
		((ComboBox)cbLabelsY).Items.AddRange(new object[3] { "None", "One Line", "Two Lines" });
		((Control)cbLabelsY).Location = new Point(12, 36);
		((Control)cbLabelsY).Name = "cbLabelsY";
		((Control)cbLabelsY).Size = new Size(196, 21);
		((Control)cbLabelsY).TabIndex = 0;
		((ComboBox)cbLabelsY).SelectedIndexChanged += cbxVisibleY_Click;
		((Control)gbGridLinesY).Controls.Add((Control)(object)cbxInterlacedY);
		((Control)gbGridLinesY).Controls.Add((Control)(object)cbGridLinesY);
		((Control)gbGridLinesY).Dock = (DockStyle)1;
		gbGridLinesY.HeaderHeight = 24;
		((Control)gbGridLinesY).Location = new Point(4, 212);
		((Control)gbGridLinesY).Name = "gbGridLinesY";
		((Control)gbGridLinesY).Size = new Size(220, 99);
		((Control)gbGridLinesY).TabIndex = 2;
		((Control)gbGridLinesY).TabStop = false;
		((Control)gbGridLinesY).Text = "Grid lines  ";
		((Control)cbxInterlacedY).Anchor = (AnchorStyles)13;
		((Control)cbxInterlacedY).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)cbxInterlacedY).Location = new Point(12, 71);
		((Control)cbxInterlacedY).Name = "cbxInterlacedY";
		((Control)cbxInterlacedY).Size = new Size(172, 16);
		((Control)cbxInterlacedY).TabIndex = 1;
		((Control)cbxInterlacedY).Text = "Interlaced";
		((Control)cbxInterlacedY).Click += cbxVisibleY_Click;
		((Control)cbGridLinesY).Anchor = (AnchorStyles)13;
		cbGridLinesY.DropDownStyle = (ComboBoxStyle)2;
		((ComboBox)cbGridLinesY).Items.AddRange(new object[3] { "None", "Major", "All" });
		((Control)cbGridLinesY).Location = new Point(12, 36);
		((Control)cbGridLinesY).Name = "cbGridLinesY";
		((Control)cbGridLinesY).Size = new Size(196, 21);
		((Control)cbGridLinesY).TabIndex = 0;
		((ComboBox)cbGridLinesY).SelectedIndexChanged += cbxVisibleY_Click;
		((Control)gbTicksY).Controls.Add((Control)(object)udMinorCountY);
		((Control)gbTicksY).Controls.Add((Control)(object)lbMinorCountY);
		((Control)gbTicksY).Controls.Add((Control)(object)cbTicksY);
		((Control)gbTicksY).Dock = (DockStyle)1;
		gbTicksY.HeaderHeight = 24;
		((Control)gbTicksY).Location = new Point(4, 108);
		((Control)gbTicksY).Name = "gbTicksY";
		((Control)gbTicksY).Size = new Size(220, 104);
		((Control)gbTicksY).TabIndex = 1;
		((Control)gbTicksY).TabStop = false;
		((Control)gbTicksY).Text = "Ticks  ";
		((Control)udMinorCountY).Anchor = (AnchorStyles)13;
		((Control)udMinorCountY).BackColor = SystemColors.Window;
		((Control)udMinorCountY).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)udMinorCountY).Location = new Point(120, 69);
		((Control)udMinorCountY).Name = "udMinorCountY";
		((Control)udMinorCountY).Size = new Size(88, 21);
		((Control)udMinorCountY).TabIndex = 2;
		udMinorCountY.ValueChanged += cbxVisibleY_Click;
		((Control)lbMinorCountY).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)lbMinorCountY).ForeColor = Color.DimGray;
		((Control)lbMinorCountY).Location = new Point(12, 69);
		((Control)lbMinorCountY).Name = "lbMinorCountY";
		((Control)lbMinorCountY).Size = new Size(112, 20);
		((Control)lbMinorCountY).TabIndex = 1;
		((Control)lbMinorCountY).Text = "Minor count";
		lbMinorCountY.TextAlign = (ContentAlignment)16;
		((Control)cbTicksY).Anchor = (AnchorStyles)13;
		cbTicksY.DropDownStyle = (ComboBoxStyle)2;
		((ComboBox)cbTicksY).Items.AddRange(new object[3] { "None", "Major", "All" });
		((Control)cbTicksY).Location = new Point(12, 36);
		((Control)cbTicksY).Name = "cbTicksY";
		((Control)cbTicksY).Size = new Size(196, 21);
		((Control)cbTicksY).TabIndex = 0;
		((ComboBox)cbTicksY).SelectedIndexChanged += cbxVisibleY_Click;
		((Control)gbTitleY).Controls.Add((Control)(object)cbAligmentY);
		((Control)gbTitleY).Controls.Add((Control)(object)lbAligmentY);
		((Control)gbTitleY).Controls.Add((Control)(object)tbTitleY);
		((Control)gbTitleY).Dock = (DockStyle)1;
		gbTitleY.HeaderHeight = 24;
		((Control)gbTitleY).Location = new Point(4, 4);
		((Control)gbTitleY).Name = "gbTitleY";
		((Control)gbTitleY).Size = new Size(220, 104);
		((Control)gbTitleY).TabIndex = 0;
		((Control)gbTitleY).TabStop = false;
		((Control)gbTitleY).Text = "Title  ";
		((Control)cbAligmentY).Anchor = (AnchorStyles)13;
		cbAligmentY.DropDownStyle = (ComboBoxStyle)2;
		((ComboBox)cbAligmentY).Items.AddRange(new object[3] { "Near", "Center", "Far" });
		((Control)cbAligmentY).Location = new Point(120, 69);
		((Control)cbAligmentY).Name = "cbAligmentY";
		((Control)cbAligmentY).Size = new Size(88, 21);
		((Control)cbAligmentY).TabIndex = 2;
		((ComboBox)cbAligmentY).SelectedIndexChanged += cbxVisibleY_Click;
		((Control)lbAligmentY).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)lbAligmentY).ForeColor = Color.DimGray;
		((Control)lbAligmentY).Location = new Point(12, 69);
		((Control)lbAligmentY).Name = "lbAligmentY";
		((Control)lbAligmentY).Size = new Size(102, 20);
		((Control)lbAligmentY).TabIndex = 1;
		((Control)lbAligmentY).Text = "Aligment";
		lbAligmentY.TextAlign = (ContentAlignment)16;
		((Control)tbTitleY).Anchor = (AnchorStyles)13;
		((Control)tbTitleY).BackColor = Color.White;
		((Control)tbTitleY).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)tbTitleY).Location = new Point(12, 36);
		((Control)tbTitleY).Name = "tbTitleY";
		((Control)tbTitleY).Size = new Size(196, 21);
		((Control)tbTitleY).TabIndex = 0;
		((Control)tbTitleY).TextChanged += cbxVisibleY_Click;
		((Control)cbxReverseY).Anchor = (AnchorStyles)13;
		((Control)cbxReverseY).Location = new Point(16, 451);
		((Control)cbxReverseY).Name = "cbxReverseY";
		((Control)cbxReverseY).Size = new Size(200, 16);
		((Control)cbxReverseY).TabIndex = 5;
		((Control)cbxReverseY).Text = "Reverse";
		((Control)cbxReverseY).Click += cbxVisibleY_Click;
		((Control)cbxVisibleY).Anchor = (AnchorStyles)13;
		((Control)cbxVisibleY).Location = new Point(16, 427);
		((Control)cbxVisibleY).Name = "cbxVisibleY";
		((Control)cbxVisibleY).Size = new Size(200, 16);
		((Control)cbxVisibleY).TabIndex = 4;
		((Control)cbxVisibleY).Text = "Visible";
		((Control)cbxVisibleY).Click += cbxVisibleY_Click;
		((Control)wpLegend).BackColor = Color.White;
		((Control)wpLegend).Controls.Add((Control)(object)scLegends);
		((Control)wpLegend).Controls.Add((Control)(object)tabControlLegend);
		wpLegend.Dock = (DockStyle)5;
		((Control)wpLegend).Location = new Point(200, 0);
		((Control)wpLegend).Name = "wpLegend";
		((Control)wpLegend).Padding = new Padding(2);
		((Control)wpLegend).Size = new Size(568, 565);
		((Control)wpLegend).TabIndex = 1009;
		((Control)wpLegend).Text = "Legend";
		wpLegend.Visible = false;
		((Control)wpLegend).VisibleChanged += wpLegend_VisibleChanged;
		((Control)scLegends).Anchor = (AnchorStyles)15;
		((Control)scLegends).BackColor = SystemColors.Window;
		((Control)scLegends).Location = new Point(2, 2);
		((Control)scLegends).Name = "scLegends";
		scLegends.ScrollHeight = 100;
		scLegends.ScrollWidth = 100;
		((Control)scLegends).Size = new Size(327, 558);
		((Control)scLegends).TabIndex = 4;
		((Control)scLegends).Text = "stiScrollableControl1";
		((Control)scLegends).Paint += new PaintEventHandler(scLegend_Paint);
		((Control)tabControlLegend).Dock = (DockStyle)4;
		((Control)tabControlLegend).Controls.Add((Control)(object)gbLegends);
		((Control)tabControlLegend).Location = new Point(331, 2);
		((Control)tabControlLegend).Name = "tabControlLegend";
		tabControlLegend.SelectedIndex = 0;
		((Control)tabControlLegend).Size = new Size(236, 558);
		((Control)tabControlLegend).TabIndex = 3;
		((Control)gbLegends).Anchor = (AnchorStyles)11;
		((Control)gbLegends).BackColor = Color.White;
		((Control)gbLegends).Controls.Add((Control)(object)gbSpacing);
		((Control)gbLegends).Controls.Add((Control)(object)gbMarker);
		((Control)gbLegends).Controls.Add((Control)(object)gbDirection);
		((Control)gbLegends).Controls.Add((Control)(object)gbAligment);
		((Control)gbLegends).Controls.Add((Control)(object)cbxLegendVisible);
		((Control)gbLegends).Controls.Add((Control)(object)groupBoxTitle);
		gbLegends.Location = new Point(4, 22);
		((Control)gbLegends).Name = "gbLegends";
		((Control)gbLegends).Padding = new Padding(4);
		((Control)gbLegends).Size = new Size(228, 532);
		gbLegends.TabIndex = 3;
		((Control)gbLegends).Text = "Legend";
		((Control)gbSpacing).Controls.Add((Control)(object)udVerticalSpacing);
		((Control)gbSpacing).Controls.Add((Control)(object)udHorizontalSpacing);
		((Control)gbSpacing).Controls.Add((Control)(object)lbVerticalSpacing);
		((Control)gbSpacing).Controls.Add((Control)(object)lbHorizontalSpacing);
		((Control)gbSpacing).Dock = (DockStyle)1;
		gbSpacing.HeaderHeight = 24;
		((Control)gbSpacing).Location = new Point(4, 372);
		((Control)gbSpacing).Name = "gbSpacing";
		((Control)gbSpacing).Size = new Size(220, 99);
		((Control)gbSpacing).TabIndex = 4;
		((Control)gbSpacing).TabStop = false;
		((Control)gbSpacing).Text = "Spacing  ";
		((Control)udVerticalSpacing).Anchor = (AnchorStyles)13;
		((Control)udVerticalSpacing).BackColor = SystemColors.Window;
		((Control)udVerticalSpacing).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)udVerticalSpacing).Location = new Point(144, 69);
		((Control)udVerticalSpacing).Name = "udVerticalSpacing";
		((Control)udVerticalSpacing).Size = new Size(64, 21);
		((Control)udVerticalSpacing).TabIndex = 3;
		udVerticalSpacing.ValueChanged += cbxLegendVisible_Click;
		((Control)udHorizontalSpacing).Anchor = (AnchorStyles)13;
		((Control)udHorizontalSpacing).BackColor = SystemColors.Window;
		((Control)udHorizontalSpacing).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)udHorizontalSpacing).Location = new Point(144, 36);
		((Control)udHorizontalSpacing).Name = "udHorizontalSpacing";
		((Control)udHorizontalSpacing).Size = new Size(64, 21);
		((Control)udHorizontalSpacing).TabIndex = 1;
		udHorizontalSpacing.ValueChanged += cbxLegendVisible_Click;
		((Control)lbVerticalSpacing).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)lbVerticalSpacing).ForeColor = Color.DimGray;
		((Control)lbVerticalSpacing).Location = new Point(12, 69);
		((Control)lbVerticalSpacing).Name = "lbVerticalSpacing";
		((Control)lbVerticalSpacing).Size = new Size(130, 20);
		((Control)lbVerticalSpacing).TabIndex = 2;
		((Control)lbVerticalSpacing).Text = "Vertical";
		lbVerticalSpacing.TextAlign = (ContentAlignment)16;
		((Control)lbHorizontalSpacing).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)lbHorizontalSpacing).ForeColor = Color.DimGray;
		((Control)lbHorizontalSpacing).Location = new Point(12, 37);
		((Control)lbHorizontalSpacing).Name = "lbHorizontalSpacing";
		((Control)lbHorizontalSpacing).Size = new Size(128, 20);
		((Control)lbHorizontalSpacing).TabIndex = 0;
		((Control)lbHorizontalSpacing).Text = "Horizontal";
		lbHorizontalSpacing.TextAlign = (ContentAlignment)16;
		((Control)gbMarker).Controls.Add((Control)(object)udHeight);
		((Control)gbMarker).Controls.Add((Control)(object)udWidth);
		((Control)gbMarker).Controls.Add((Control)(object)lbHeight);
		((Control)gbMarker).Controls.Add((Control)(object)lbWidth);
		((Control)gbMarker).Controls.Add((Control)(object)cbxMarkerVisible);
		((Control)gbMarker).Dock = (DockStyle)1;
		gbMarker.HeaderHeight = 24;
		((Control)gbMarker).Location = new Point(4, 245);
		((Control)gbMarker).Name = "gbMarker";
		((Control)gbMarker).Size = new Size(220, 127);
		((Control)gbMarker).TabIndex = 3;
		((Control)gbMarker).TabStop = false;
		((Control)gbMarker).Text = "Marker  ";
		((Control)udHeight).Anchor = (AnchorStyles)13;
		((Control)udHeight).BackColor = SystemColors.Window;
		((Control)udHeight).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)udHeight).Location = new Point(144, 95);
		((Control)udHeight).Name = "udHeight";
		((Control)udHeight).Size = new Size(64, 21);
		((Control)udHeight).TabIndex = 4;
		udHeight.ValueChanged += cbxLegendVisible_Click;
		((Control)udWidth).Anchor = (AnchorStyles)13;
		((Control)udWidth).BackColor = SystemColors.Window;
		((Control)udWidth).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)udWidth).Location = new Point(144, 62);
		((Control)udWidth).Name = "udWidth";
		((Control)udWidth).Size = new Size(64, 21);
		((Control)udWidth).TabIndex = 2;
		udWidth.ValueChanged += cbxLegendVisible_Click;
		((Control)lbHeight).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)lbHeight).ForeColor = Color.DimGray;
		((Control)lbHeight).Location = new Point(12, 95);
		((Control)lbHeight).Name = "lbHeight";
		((Control)lbHeight).Size = new Size(130, 20);
		((Control)lbHeight).TabIndex = 3;
		((Control)lbHeight).Text = "Height";
		lbHeight.TextAlign = (ContentAlignment)16;
		((Control)lbWidth).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)lbWidth).ForeColor = Color.DimGray;
		((Control)lbWidth).Location = new Point(12, 62);
		((Control)lbWidth).Name = "lbWidth";
		((Control)lbWidth).Size = new Size(130, 20);
		((Control)lbWidth).TabIndex = 1;
		((Control)lbWidth).Text = "Width";
		lbWidth.TextAlign = (ContentAlignment)16;
		((Control)cbxMarkerVisible).Anchor = (AnchorStyles)13;
		((Control)cbxMarkerVisible).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)cbxMarkerVisible).Location = new Point(12, 36);
		((Control)cbxMarkerVisible).Name = "cbxMarkerVisible";
		((Control)cbxMarkerVisible).Size = new Size(196, 16);
		((Control)cbxMarkerVisible).TabIndex = 0;
		((Control)cbxMarkerVisible).Text = "Visible";
		((Control)cbxMarkerVisible).Click += cbxLegendVisible_Click;
		((Control)gbDirection).Controls.Add((Control)(object)cbDirection);
		((Control)gbDirection).Dock = (DockStyle)1;
		gbDirection.HeaderHeight = 24;
		((Control)gbDirection).Location = new Point(4, 176);
		((Control)gbDirection).Name = "gbDirection";
		((Control)gbDirection).Size = new Size(220, 69);
		((Control)gbDirection).TabIndex = 2;
		((Control)gbDirection).TabStop = false;
		((Control)gbDirection).Text = "Direction  ";
		((Control)cbDirection).Anchor = (AnchorStyles)13;
		cbDirection.DropDownStyle = (ComboBoxStyle)2;
		((ComboBox)cbDirection).Items.AddRange(new object[4] { "LeftToRight", "RightToLeft", "TopToBottom", "BottomToTop" });
		((Control)cbDirection).Location = new Point(12, 36);
		((Control)cbDirection).Name = "cbDirection";
		((Control)cbDirection).Size = new Size(196, 21);
		((Control)cbDirection).TabIndex = 0;
		((ComboBox)cbDirection).SelectedIndexChanged += cbxLegendVisible_Click;
		((Control)gbAligment).Controls.Add((Control)(object)cbVerticalAligment);
		((Control)gbAligment).Controls.Add((Control)(object)cbHorizontalAligment);
		((Control)gbAligment).Controls.Add((Control)(object)lbVerticalAligment);
		((Control)gbAligment).Controls.Add((Control)(object)lbHorizontalAligment);
		((Control)gbAligment).Dock = (DockStyle)1;
		gbAligment.HeaderHeight = 24;
		((Control)gbAligment).Location = new Point(4, 74);
		((Control)gbAligment).Name = "gbAligment";
		((Control)gbAligment).Size = new Size(220, 102);
		((Control)gbAligment).TabIndex = 1;
		((Control)gbAligment).TabStop = false;
		((Control)gbAligment).Text = "Aligment  ";
		((Control)cbVerticalAligment).Anchor = (AnchorStyles)13;
		cbVerticalAligment.DropDownStyle = (ComboBoxStyle)2;
		((ComboBox)cbVerticalAligment).Items.AddRange(new object[5] { "TopOutside", "Top", "Center", "Bottom", "BottomOutside" });
		((Control)cbVerticalAligment).Location = new Point(104, 69);
		((Control)cbVerticalAligment).Name = "cbVerticalAligment";
		((Control)cbVerticalAligment).Size = new Size(104, 21);
		((Control)cbVerticalAligment).TabIndex = 1;
		((ComboBox)cbVerticalAligment).SelectedIndexChanged += cbxLegendVisible_Click;
		((Control)cbHorizontalAligment).Anchor = (AnchorStyles)13;
		cbHorizontalAligment.DropDownStyle = (ComboBoxStyle)2;
		((ComboBox)cbHorizontalAligment).Items.AddRange(new object[5] { "LeftOutside", "Left", "Center", "Right", "RightOutside" });
		((Control)cbHorizontalAligment).Location = new Point(104, 36);
		((Control)cbHorizontalAligment).Name = "cbHorizontalAligment";
		((Control)cbHorizontalAligment).Size = new Size(104, 21);
		((Control)cbHorizontalAligment).TabIndex = 0;
		((ComboBox)cbHorizontalAligment).SelectedIndexChanged += cbxLegendVisible_Click;
		((Control)lbVerticalAligment).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)lbVerticalAligment).ForeColor = Color.DimGray;
		((Control)lbVerticalAligment).Location = new Point(12, 69);
		((Control)lbVerticalAligment).Name = "lbVerticalAligment";
		((Control)lbVerticalAligment).Size = new Size(88, 21);
		((Control)lbVerticalAligment).TabIndex = 2;
		((Control)lbVerticalAligment).Text = "Vertical";
		lbVerticalAligment.TextAlign = (ContentAlignment)16;
		((Control)lbHorizontalAligment).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)lbHorizontalAligment).ForeColor = Color.DimGray;
		((Control)lbHorizontalAligment).Location = new Point(12, 36);
		((Control)lbHorizontalAligment).Name = "lbHorizontalAligment";
		((Control)lbHorizontalAligment).Size = new Size(89, 21);
		((Control)lbHorizontalAligment).TabIndex = 1;
		((Control)lbHorizontalAligment).Text = "Horizontal";
		lbHorizontalAligment.TextAlign = (ContentAlignment)16;
		((Control)cbxLegendVisible).Anchor = (AnchorStyles)13;
		((Control)cbxLegendVisible).Location = new Point(20, 348);
		((Control)cbxLegendVisible).Name = "cbxLegendVisible";
		((Control)cbxLegendVisible).Size = new Size(188, 16);
		((Control)cbxLegendVisible).TabIndex = 5;
		((Control)cbxLegendVisible).Text = "Visible";
		((Control)cbxLegendVisible).Click += cbxLegendVisible_Click;
		((Control)groupBoxTitle).Controls.Add((Control)(object)tbLegendTitle);
		((Control)groupBoxTitle).Dock = (DockStyle)1;
		groupBoxTitle.HeaderHeight = 24;
		((Control)groupBoxTitle).Location = new Point(4, 4);
		((Control)groupBoxTitle).Name = "groupBoxTitle";
		((Control)groupBoxTitle).Size = new Size(220, 70);
		((Control)groupBoxTitle).TabIndex = 0;
		((Control)groupBoxTitle).TabStop = false;
		((Control)groupBoxTitle).Text = "Direction  ";
		((Control)tbLegendTitle).Anchor = (AnchorStyles)13;
		((Control)tbLegendTitle).BackColor = Color.White;
		((Control)tbLegendTitle).Location = new Point(12, 36);
		((Control)tbLegendTitle).Name = "tbLegendTitle";
		((Control)tbLegendTitle).Size = new Size(196, 21);
		((Control)tbLegendTitle).TabIndex = 0;
		((Control)tbLegendTitle).TextChanged += cbxLegendVisible_Click;
		dcArgument.AllowDBNull = false;
		dcArgument.ColumnName = "Argument";
		dcArgument.DefaultValue = "";
		dcValue.ColumnName = "Value";
		dcValue.DataType = typeof(double);
		dcValue.DefaultValue = 0.0;
		dcEndValue.ColumnName = "End Value";
		dcEndValue.DataType = typeof(double);
		dcEndValue.DefaultValue = 1.0;
		dcOpenValue.ColumnName = "Open Value";
		dcOpenValue.DataType = typeof(double);
		dcOpenValue.DefaultValue = 2.0;
		dcCloseValue.ColumnName = "Close Value";
		dcCloseValue.DataType = typeof(double);
		dcCloseValue.DefaultValue = 3.0;
		dcHighValue.ColumnName = "High Value";
		dcHighValue.DataType = typeof(double);
		dcHighValue.DefaultValue = 4.0;
		dcLowValue.ColumnName = "Low Value";
		dcLowValue.DataType = typeof(double);
		dcLowValue.DefaultValue = 5.0;
		dsPoints.DataSetName = "NewDataSet";
		dsPoints.Locale = new CultureInfo("ru-RU");
		dsPoints.Tables.AddRange(new DataTable[1] { dtPoints });
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).ClientSize = new Size(792, 623);
		((Control)this).Controls.Add((Control)(object)Wizard);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiChartEditorForm";
		((Control)this).Padding = new Padding(12, 12, 12, 0);
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Select container for clone";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Form)this).FormClosing += new FormClosingEventHandler(This_FormClosing);
		((Form)this).Shown += Wizard_ChangeSelected;
		((Control)this).KeyDown += new KeyEventHandler(This_KeyDown);
		((Control)Wizard).ResumeLayout(false);
		((Control)wpChartType).ResumeLayout(false);
		((Control)wpStyle).ResumeLayout(false);
		((Control)wpSeriesData).ResumeLayout(false);
		((Control)stiTabControl1).ResumeLayout(false);
		((Control)tabPageDataColumns).ResumeLayout(false);
		((Control)tabPageListOfValues).ResumeLayout(false);
		((ISupportInitialize)dgPoints).EndInit();
		((ISupportInitialize)dtPoints).EndInit();
		((Control)tbTool).ResumeLayout(false);
		((Control)tbTool).PerformLayout();
		((Control)wpLabels).ResumeLayout(false);
		((Control)gbLabels).ResumeLayout(false);
		((ISupportInitialize)udLabelsAngle).EndInit();
		((Control)wpAxes).ResumeLayout(false);
		((Control)tcAxes).ResumeLayout(false);
		((Control)tpAxisX).ResumeLayout(false);
		((Control)gbLabelsX).ResumeLayout(false);
		((ISupportInitialize)udAngleX).EndInit();
		((Control)gbGridLinesX).ResumeLayout(false);
		((Control)gbTicksX).ResumeLayout(false);
		((ISupportInitialize)udMinorCountX).EndInit();
		((Control)gbTitleX).ResumeLayout(false);
		((Control)tpAxisY).ResumeLayout(false);
		((Control)gbLabelsY).ResumeLayout(false);
		((ISupportInitialize)udAngleY).EndInit();
		((Control)gbGridLinesY).ResumeLayout(false);
		((Control)gbTicksY).ResumeLayout(false);
		((ISupportInitialize)udMinorCountY).EndInit();
		((Control)gbTitleY).ResumeLayout(false);
		((Control)wpLegend).ResumeLayout(false);
		((Control)tabControlLegend).ResumeLayout(false);
		((Control)gbLegends).ResumeLayout(false);
		((Control)gbSpacing).ResumeLayout(false);
		((ISupportInitialize)udVerticalSpacing).EndInit();
		((ISupportInitialize)udHorizontalSpacing).EndInit();
		((Control)gbMarker).ResumeLayout(false);
		((ISupportInitialize)udHeight).EndInit();
		((ISupportInitialize)udWidth).EndInit();
		((Control)gbDirection).ResumeLayout(false);
		((Control)gbAligment).ResumeLayout(false);
		((Control)groupBoxTitle).ResumeLayout(false);
		((ISupportInitialize)dsPoints).EndInit();
		((Control)this).ResumeLayout(false);
	}

	private void DoScaling()
	{
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		StiResourcesHelper.SetImage((ButtonBase)(object)tbAddSeries, "Stimulsoft.Report.Design.Resources.Office2013.Chart.SeriesNew.png", scale: true);
		dgPoints.ColumnHeadersHeight = StiScale.YYI(20.0);
		dgPoints.RowTemplate.Height = StiScale.YYI(20.0);
		foreach (DataGridViewColumn item in (BaseCollection)dgPoints.Columns)
		{
			DataGridViewColumn val = item;
			val.Width = StiScale.XXI(120.0);
		}
		((ButtonBase)tbRemove).Image = StiReportDesignImages.Actions.Remove();
		((ButtonBase)tbMoveUp).Image = StiReportDesignImages.Actions.MoveUp();
		((ButtonBase)tbMoveDown).Image = StiReportDesignImages.Actions.MoveDown();
		StiResourcesHelper.SetImage((ButtonBase)(object)tbNewDataSource, PanelDictionaryOffice2013Resource.DataSourceNew, scale: true);
	}

	private StiService[] GetChartAreas()
	{
		List<StiArea> list = StiOptions.Services.ChartAreas.Where((StiArea s) => s.ServiceEnabled).ToList();
		chartTypeId = 0;
		tmpChartStyle.Area = list[chartTypeId].Clone() as StiArea;
		scChartType.ScrollHeight = (list.Count - 1) / 3 * StiScale.I(185) + StiScale.I(186);
		return list.ToArray();
	}

	private List<StiChartStyle> GetChartStyles()
	{
		List<StiChartStyle> list = (from s in StiOptions.Services.ChartStyles.Where((StiChartStyle s) => s.ServiceEnabled).ToList()
			orderby s.Core.StyleId descending
			select s).ToList();
		List<StiChartStyle> list2 = new List<StiChartStyle>();
		foreach (StiBaseStyle style in chart.Report.Styles)
		{
			if (style is Stimulsoft.Report.StiChartStyle)
			{
				StiCustomStyle stiCustomStyle = new StiCustomStyle(style.Name);
				stiCustomStyle.CustomCore.ReportChartStyle = style as Stimulsoft.Report.StiChartStyle;
				list2.Add(stiCustomStyle);
			}
		}
		foreach (StiChartStyle item in list)
		{
			list2.Add(item);
		}
		scChartStyle.ScrollHeight = (list.Count - 1) / 3 * StiScale.I(185) + StiScale.I(186);
		return list2;
	}

	private StiService[] GetLabelTypes()
	{
		return StiOptions.Services.ChartSerieLabels.Where((StiSeriesLabels s) => s.ServiceEnabled).ToArray();
	}

	private void AddDefaultSeries(StiChart chart)
	{
		Type defaultSeriesType = chart.Area.GetDefaultSeriesType();
		chart.Series.Clear();
		StiSeries value = StiActivator.CreateObject(defaultSeriesType) as StiSeries;
		chart.Series.Add(value);
		value = StiActivator.CreateObject(defaultSeriesType) as StiSeries;
		chart.Series.Add(value);
	}

	private void DrawSelectRect(Graphics g, int x, int y, int x_size, int y_size)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Expected O, but got Unknown
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		Pen val = new Pen(StiColors.SelectedText, 2f);
		try
		{
			SolidBrush val2 = new SolidBrush(StiColors.Selected);
			try
			{
				g.FillRectangle((Brush)(object)val2, x, y, x_size, y_size);
				g.DrawRectangle(val, x + 1, y + 1, x_size, y_size);
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

	private void wpChartType_VisibleChanged(object sender, EventArgs e)
	{
		if (wpChartType.Visible)
		{
			tmpChartStyle.Style = chartStyles.FirstOrDefault((StiChartStyle s) => s.Core.StyleId == chartStyleId).Clone() as StiChartStyle;
			tmpChartStyle.Legend.Visible = false;
			tmpChartStyle.SeriesLabels.Visible = false;
		}
	}

	private void scChartType_Paint(object sender, PaintEventArgs e)
	{
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Expected O, but got Unknown
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Expected O, but got Unknown
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Expected O, but got Unknown
		Graphics graphics = e.Graphics;
		int num = StiScale.I5;
		int num2 = 0;
		int num3 = 0;
		double zoom = tmpChartStyle.Report.Info.Zoom;
		tmpChartStyle.Report.Info.Zoom = 1.0;
		StiService[] array = chartAreas;
		for (int i = 0; i < array.Length; i++)
		{
			StiArea stiArea = (StiArea)array[i];
			tmpChartStyle.Area = stiArea.Clone() as StiArea;
			AddDefaultSeries(tmpChartStyle);
			tmpChartStyle.Brush = new StiSolidBrush(Color.FromArgb(10, 0, 0, 0));
			if (tmpChartStyle.Area is StiAxisArea)
			{
				((StiAxisArea)tmpChartStyle.Area).AxisCore.SwitchOff();
			}
			tmpChartStyle.Area.ShowShadow = true;
			tmpChartStyle.SeriesLabels.Visible = false;
			tmpChartStyle.Core.ApplyStyle(tmpChartStyle.Style);
			if (num3 == chartTypeId)
			{
				DrawSelectRect(graphics, num, num2 - ((ScrollBar)scChartType.VScrollBar).Value, StiScale.I(170), StiScale.I(180));
			}
			Rectangle rectangle = new Rectangle(num, num2 - ((ScrollBar)scChartType.VScrollBar).Value + StiScale.I(22), StiScale.I(170), StiScale.I(160));
			if (rectangle.Top <= ((Control)scChartType).Height && rectangle.Bottom >= 0)
			{
				tmpChartStyle.DrawChart(graphics, rectangle, useMargins: true, useBackground: false);
			}
			rectangle = new Rectangle(num, num2 - ((ScrollBar)scChartType.VScrollBar).Value + StiScale.I4, StiScale.I(170), StiScale.I(24));
			rectangle.Inflate(-StiScale.I2, -StiScale.I2);
			if (rectangle.Top <= ((Control)scChartType).Height && rectangle.Bottom >= 0)
			{
				SolidBrush val = new SolidBrush(Color.Black);
				try
				{
					Font val2 = new Font("Arial", 8f);
					try
					{
						StringFormat val3 = new StringFormat();
						try
						{
							val3.Alignment = (StringAlignment)1;
							val3.LineAlignment = (StringAlignment)2;
							graphics.DrawString(stiArea.ServiceName, val2, (Brush)(object)val, (RectangleF)rectangle, val3);
						}
						finally
						{
							((IDisposable)val3)?.Dispose();
						}
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
			num += StiScale.I(190);
			if (num > StiScale.I(385))
			{
				num2 += StiScale.I(185);
				num = StiScale.I5;
			}
			num3++;
		}
		tmpChartStyle.Report.Info.Zoom = zoom;
	}

	private void scChartType_MouseDown(object sender, MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)e.Button != 1048576)
		{
			return;
		}
		int num = StiScale.I5;
		int num2 = 0;
		for (int i = 0; i < chartAreas.Length; i++)
		{
			if (new Rectangle(num, num2 - ((ScrollBar)scChartType.VScrollBar).Value, StiScale.I(170), StiScale.I(180)).Contains(e.X, e.Y))
			{
				chartTypeId = i;
				((Control)scChartType).Invalidate();
			}
			num += StiScale.I(190);
			if (num > StiScale.I(385))
			{
				num2 += StiScale.I(185);
				num = StiScale.I5;
			}
		}
	}

	private void WpStyle_VisibleChanged(object sender, EventArgs e)
	{
		if (wpStyle.Visible)
		{
			tmpChartStyle.Area = chartAreas[chartTypeId].Clone() as StiArea;
			AddDefaultSeries(tmpChartStyle);
			if (tmpChartStyle.Area is StiAxisArea)
			{
				((StiAxisArea)tmpChartStyle.Area).AxisCore.SwitchOff();
			}
			tmpChartStyle.Area.ShowShadow = true;
			tmpChartStyle.Legend.Visible = true;
			tmpChartStyle.SeriesLabels.Visible = true;
		}
	}

	private void ScChartStyle_Paint(object sender, PaintEventArgs e)
	{
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Expected O, but got Unknown
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Expected O, but got Unknown
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Expected O, but got Unknown
		int num = ((Control)scChartStyle).Width - ((Control)scChartStyle.VScrollBar).Width - StiScale.I8;
		int num2 = num / 3;
		Graphics graphics = e.Graphics;
		int num3 = StiScale.I5;
		int num4 = 0;
		StiChartStyleId stiChartStyleId = StiChartStyleId.StiStyle01;
		double zoom = tmpChartStyle.Report.Info.Zoom;
		tmpChartStyle.Report.Info.Zoom = 1.0;
		foreach (StiChartStyle chartStyle in chartStyles)
		{
			tmpChartStyle.Style = chartStyle.Clone() as StiChartStyle;
			if (chartStyle is StiCustomStyle && ((StiCustomStyle)chartStyle).CustomCore.ReportChartStyle != null)
			{
				tmpChartStyle.CustomStyleName = ((StiCustomStyle)chartStyle).CustomCore.ReportChartStyle.Name;
			}
			tmpChartStyle.Core.ApplyStyle(tmpChartStyle.Style);
			if (chartStyle.Core.StyleId == chartStyleId)
			{
				DrawSelectRect(graphics, num3, num4 - ((ScrollBar)scChartStyle.VScrollBar).Value, num2, StiScale.I(180));
			}
			Rectangle rectangle = new Rectangle(num3, num4 - ((ScrollBar)scChartStyle.VScrollBar).Value + StiScale.I(22), num2, StiScale.I(160));
			if (rectangle.Top <= ((Control)scChartStyle).Height && rectangle.Bottom >= 0)
			{
				tmpChartStyle.DrawChart(graphics, rectangle, useMargins: true, useBackground: false);
			}
			rectangle = new Rectangle(num3, num4 - ((ScrollBar)scChartStyle.VScrollBar).Value + StiScale.I4, num2, StiScale.I(24));
			if (rectangle.Top <= ((Control)scChartStyle).Height && rectangle.Bottom >= 0)
			{
				SolidBrush val = new SolidBrush(Color.Black);
				try
				{
					Font val2 = new Font("Arial", 8f);
					try
					{
						StringFormat val3 = new StringFormat();
						try
						{
							val3.Alignment = (StringAlignment)1;
							val3.LineAlignment = (StringAlignment)2;
							graphics.DrawString(chartStyle.ServiceName, val2, (Brush)(object)val, (RectangleF)rectangle, val3);
						}
						finally
						{
							((IDisposable)val3)?.Dispose();
						}
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
			num3 += num2;
			if (num3 > num2 * 3)
			{
				num4 += StiScale.I(185);
				num3 = StiScale.I5;
			}
			stiChartStyleId++;
		}
		tmpChartStyle.Report.Info.Zoom = zoom;
	}

	private void ScChartStyle_MouseDown(object sender, MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)e.Button != 1048576)
		{
			return;
		}
		int num = StiScale.I5;
		int num2 = 0;
		for (int i = 0; i < chartStyles.Count; i++)
		{
			if (new Rectangle(num, num2 - ((ScrollBar)scChartStyle.VScrollBar).Value, StiScale.I(170), StiScale.I(180)).Contains(e.X, e.Y))
			{
				chartStyleId = chartStyles[i].Core.StyleId;
				((Control)scChartStyle).Invalidate();
			}
			num += StiScale.I(190);
			if (num > StiScale.I(385))
			{
				num2 += StiScale.I(185);
				num = StiScale.I5;
			}
		}
	}

	private void ApplyStyles()
	{
		tmpChartSettings.Style = chartStyles.FirstOrDefault((StiChartStyle s) => s.Core.StyleId == chartStyleId).Clone() as StiChartStyle;
		if (chartAxisLabelsSettings != null)
		{
			chartAxisLabelsSettings.Core.ApplyStyle(tmpChartSettings.Style);
		}
		if (chartPieLabelsSettings != null)
		{
			chartPieLabelsSettings.Core.ApplyStyle(tmpChartSettings.Style);
		}
		if (chartAxesSettings != null)
		{
			chartAxesSettings.Core.ApplyStyle(tmpChartSettings.Style);
		}
		if (chartLegendSettings != null)
		{
			chartLegendSettings.Core.ApplyStyle(tmpChartSettings.Style);
		}
		tmpChartSettings.Core.ApplyStyle(tmpChartSettings.Style);
	}

	private void ApplySettings(object sender, EventArgs e)
	{
		tmpChartStyle.Area.ShowShadow = false;
		if (tmpChartStyle.Area is StiAxisArea)
		{
			chartAxesSettings = tmpChartStyle.Area.Clone() as StiAxisArea;
			chartAxesSettings.Brush = new StiSolidBrush(Color.FromArgb(10, 0, 0, 0));
		}
		chartAxisLabelsSettings = chartLabelsTypes[chartLabelTypeId].Clone() as StiAxisSeriesLabels;
		chartPieLabelsSettings = chartLabelsTypes[chartLabelTypeId].Clone() as StiCenterPieLabels;
		chartLegendSettings = tmpChartStyle.Legend.Clone() as StiLegend;
		tmpChartSettings.Area = chartAreas[chartTypeId].Clone() as StiArea;
		tmpChartSettings.SeriesLabels = chartLabelsTypes[chartLabelTypeId].Clone() as StiSeriesLabels;
		ApplyStyles();
		cbxLabelsVisible_Click(sender, e);
		cbxVisibleX_Click(sender, e);
		cbxVisibleY_Click(sender, e);
		cbxLegendVisible_Click(sender, e);
	}

	private TreeNode SelectNode(TreeNodeCollection nodes, string[] strs, int index)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		TreeNode val = null;
		foreach (TreeNode node in nodes)
		{
			TreeNode val2 = node;
			if (val2.Text == strs[index])
			{
				val = val2;
				break;
			}
		}
		if (val == null)
		{
			return null;
		}
		TreeNode val3 = null;
		if (++index < strs.Length)
		{
			val3 = SelectNode(val.Nodes, strs, index);
		}
		if (val3 != null)
		{
			return val3;
		}
		return val;
	}

	private void TwBoxArguments_TreeNodesFill(object sender, StiTreeNodesFillEventArgs e)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		((TreeView)e.TreeView).BeforeExpand += new TreeViewCancelEventHandler(TreeView_BeforeExpand);
		TreeNode val = new TreeNode(StiLocalization.Get("MainMenu", "menuViewQuickInfoNone"));
		string imageKey = (val.SelectedImageKey = "Close");
		val.ImageKey = imageKey;
		e.Nodes.Add(val);
		dataBuilder.Build(e.Nodes, tmpChartSettings.Report.Designer.Report.Dictionary, dictionaryDesign: false, includeCalcColumn: true);
		((TreeView)e.TreeView).ImageList = StiDataBuilder.Images;
		string[] strs = ((Control)(sender as StiTreeViewBox)).Text.Split(new char[1] { '.' });
		((TreeView)e.TreeView).SelectedNode = SelectNode(e.Nodes, strs, 0);
	}

	private void TreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
	{
		if (e.Node is TreeNodeEx node && e.Node.Tag != null)
		{
			dataBuilder.ExpandedNode(node, includeCalcColumn: true);
		}
	}

	private void TwBox_ValueChanged(object sender, StiValueChangedEventArgs e)
	{
		TreeNode selectedNode = ((TreeView)e.TreeView).SelectedNode;
		if (selectedNode != null && selectedNode == ((TreeView)e.TreeView).Nodes[0])
		{
			e.Text = string.Empty;
		}
		else if (selectedNode != null && selectedNode.Tag is StiDataColumn)
		{
			e.Text = StiDataBuilder.GetColumnPathFromNode(selectedNode);
		}
	}

	private void TwBoxArguments_ValueChanged(object sender, StiValueChangedEventArgs e)
	{
		TwBox_ValueChanged(sender, e);
		if (((ListControl)lbSeries).SelectedIndex >= 0)
		{
			if (tmpChartSettings.Area is StiCandlestickArea)
			{
				SetSeriesData(tmpChartSettings.Series[((ListControl)lbSeries).SelectedIndex] as StiSeries, e.Text, ((Control)twBoxOpenValues).Text, ((Control)twBoxCloseValues).Text, ((Control)twBoxHighValues).Text, ((Control)twBoxLowValues).Text);
			}
			else if (tmpChartSettings.Area is StiRangeBarArea || tmpChartSettings.Area is StiGanttArea)
			{
				SetSeriesData(tmpChartSettings.Series[((ListControl)lbSeries).SelectedIndex] as StiSeries, e.Text, ((Control)twBoxValues).Text, ((Control)twBoxEndValues).Text);
			}
			else
			{
				SetSeriesData(tmpChartSettings.Series[((ListControl)lbSeries).SelectedIndex] as StiSeries, e.Text, ((Control)twBoxValues).Text);
			}
		}
	}

	private void TwBoxValues_ValueChanged(object sender, StiValueChangedEventArgs e)
	{
		TwBox_ValueChanged(sender, e);
		if (((ListControl)lbSeries).SelectedIndex >= 0)
		{
			if (tmpChartSettings.Area is StiRangeBarArea || tmpChartSettings.Area is StiGanttArea)
			{
				SetSeriesData(tmpChartSettings.Series[((ListControl)lbSeries).SelectedIndex] as StiSeries, ((Control)twBoxArguments).Text, e.Text, ((Control)twBoxEndValues).Text);
			}
			else
			{
				SetSeriesData(tmpChartSettings.Series[((ListControl)lbSeries).SelectedIndex] as StiSeries, ((Control)twBoxArguments).Text, e.Text);
			}
		}
	}

	private void TwBoxEndValues_ValueChanged(object sender, StiValueChangedEventArgs e)
	{
		TwBox_ValueChanged(sender, e);
		if (((ListControl)lbSeries).SelectedIndex >= 0 && (tmpChartSettings.Area is StiRangeBarArea || tmpChartSettings.Area is StiGanttArea))
		{
			SetSeriesData(tmpChartSettings.Series[((ListControl)lbSeries).SelectedIndex] as StiSeries, ((Control)twBoxArguments).Text, ((Control)twBoxEndValues).Text, e.Text);
		}
	}

	private void TwBoxOpenValues_ValueChanged(object sender, StiValueChangedEventArgs e)
	{
		TwBox_ValueChanged(sender, e);
		if (((ListControl)lbSeries).SelectedIndex >= 0 && tmpChartSettings.Area is StiCandlestickArea)
		{
			SetSeriesData(tmpChartSettings.Series[((ListControl)lbSeries).SelectedIndex] as StiSeries, ((Control)twBoxArguments).Text, e.Text, ((Control)twBoxCloseValues).Text, ((Control)twBoxHighValues).Text, ((Control)twBoxLowValues).Text);
		}
	}

	private void TwBoxCloseValues_ValueChanged(object sender, StiValueChangedEventArgs e)
	{
		TwBox_ValueChanged(sender, e);
		if (((ListControl)lbSeries).SelectedIndex >= 0 && tmpChartSettings.Area is StiCandlestickArea)
		{
			SetSeriesData(tmpChartSettings.Series[((ListControl)lbSeries).SelectedIndex] as StiSeries, ((Control)twBoxArguments).Text, ((Control)twBoxOpenValues).Text, e.Text, ((Control)twBoxHighValues).Text, ((Control)twBoxLowValues).Text);
		}
	}

	private void TwBoxHighValues_ValueChanged(object sender, StiValueChangedEventArgs e)
	{
		TwBox_ValueChanged(sender, e);
		if (((ListControl)lbSeries).SelectedIndex >= 0 && tmpChartSettings.Area is StiCandlestickArea)
		{
			SetSeriesData(tmpChartSettings.Series[((ListControl)lbSeries).SelectedIndex] as StiSeries, ((Control)twBoxArguments).Text, ((Control)twBoxOpenValues).Text, ((Control)twBoxCloseValues).Text, e.Text, ((Control)twBoxLowValues).Text);
		}
	}

	private void TwBoxLowValues_ValueChanged(object sender, StiValueChangedEventArgs e)
	{
		TwBox_ValueChanged(sender, e);
		if (((ListControl)lbSeries).SelectedIndex >= 0 && tmpChartSettings.Area is StiCandlestickArea)
		{
			SetSeriesData(tmpChartSettings.Series[((ListControl)lbSeries).SelectedIndex] as StiSeries, ((Control)twBoxArguments).Text, ((Control)twBoxOpenValues).Text, ((Control)twBoxCloseValues).Text, ((Control)twBoxHighValues).Text, e.Text);
		}
	}

	private void WpDataSeries_VisibleChanged(object sender, EventArgs e)
	{
		if (wpSeriesData.Visible)
		{
			if (lbSeries.Items.Count == 0)
			{
				((Control)tbSeriesName).Text = "";
			}
			ApplySettings(sender, e);
			((Control)tbMoveUp).Enabled = lbSeries.Items.Count > 0;
			((Control)tbMoveDown).Enabled = lbSeries.Items.Count > 0;
			if (lbSeries.Items.Count == 0)
			{
				dtPoints.Rows.Clear();
			}
			((Control)dgPoints).Enabled = lbSeries.Items.Count > 0;
			((Control)twBoxArguments).Enabled = lbSeries.Items.Count > 0;
			((Control)twBoxValues).Enabled = lbSeries.Items.Count > 0;
			((Control)twBoxEndValues).Enabled = lbSeries.Items.Count > 0;
			((Control)twBoxOpenValues).Enabled = lbSeries.Items.Count > 0;
			((Control)twBoxCloseValues).Enabled = lbSeries.Items.Count > 0;
			((Control)twBoxHighValues).Enabled = lbSeries.Items.Count > 0;
			((Control)twBoxLowValues).Enabled = lbSeries.Items.Count > 0;
			if (lastArea == null || tmpChartSettings.Area.GetType() != lastArea.GetType())
			{
				dtPoints.Columns.Clear();
				lbSeries.Items.Clear();
				tmpChartSettings.Series.Clear();
				lastSelectedSeries = -1;
				FillDateTablePoints(tmpChartSettings.Area);
				lastArea = tmpChartSettings.Area;
			}
			else if (((ListControl)lbSeries).SelectedIndex >= 0)
			{
				GetSeriesData(tmpChartSettings.Series[((ListControl)lbSeries).SelectedIndex] as StiSeries);
			}
			DoScaling();
			CheckEnabledComponents();
		}
	}

	private void FillDateTablePoints(IStiArea area)
	{
		if (area is StiCandlestickArea || area is StiStockArea)
		{
			dtPoints.Columns.AddRange(new DataColumn[5] { dcArgument, dcOpenValue, dcCloseValue, dcHighValue, dcLowValue });
		}
		else if (area is StiRangeBarArea || area is StiGanttArea || area is StiRangeArea || area is StiSplineRangeArea || area is StiSteppedRangeArea)
		{
			dtPoints.Columns.AddRange(new DataColumn[3] { dcArgument, dcValue, dcEndValue });
		}
		else
		{
			dtPoints.Columns.AddRange(new DataColumn[2] { dcArgument, dcValue });
		}
	}

	private void CmSeries_Popup(object sender, EventArgs e)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		((ToolStrip)cmSeries).Items.Clear();
		menuItemsSeries = new Hashtable();
		Type[] seriesTypes = tmpChartSettings.Area.GetSeriesTypes();
		Type[] array = seriesTypes;
		foreach (Type type in array)
		{
			if (StiActivator.CreateObject(type) is StiSeries stiSeries)
			{
				ToolStripMenuItem val = new ToolStripMenuItem(stiSeries.ToString());
				((ToolStripItem)val).Click += OnSeriesAddClick;
				((ToolStrip)cmSeries).Items.Add((ToolStripItem)(object)val);
				menuItemsSeries[val] = stiSeries;
			}
		}
	}

	private void OnSeriesAddClick(object sender, EventArgs e)
	{
		StiSeries stiSeries = menuItemsSeries[sender] as StiSeries;
		if (stiSeries is StiScatterSeries || stiSeries is StiScatterLineSeries || stiSeries is StiScatterSplineSeries)
		{
			stiSeries.ListOfArguments.Value = "1";
		}
		else
		{
			stiSeries.ListOfArguments.Value = "A";
		}
		stiSeries.ListOfValues.Value = (tmpChartSettings.Series.Count + 1).ToString();
		tmpChartSettings.Series.Add(stiSeries);
		stiSeries.Core.ApplyStyle(chart.Style, chart.Style.Core.GetColorBySeries(stiSeries, stiSeries.Core?.SeriesColors));
		StiSeriesTitle stiSeriesTitle = new StiSeriesTitle(stiSeries);
		lbSeries.Items.Add((object)stiSeriesTitle);
		((ListControl)lbSeries).SelectedIndex = lbSeries.Items.Count - 1;
		((Control)dgPoints).Enabled = lbSeries.Items.Count > 0;
		CheckEnabledComponents(stiSeries);
	}

	private void CheckEnabledComponents()
	{
		((Control)twBoxArguments).Enabled = false;
		((Control)twBoxOpenValues).Enabled = false;
		((Control)twBoxCloseValues).Enabled = false;
		((Control)twBoxHighValues).Enabled = false;
		((Control)twBoxLowValues).Enabled = false;
		((Control)twBoxEndValues).Enabled = false;
		((Control)twBoxValues).Enabled = false;
		((Control)twBoxValues).Enabled = false;
		if (((ListControl)lbSeries).SelectedIndex >= 0)
		{
			CheckEnabledComponents(tmpChartSettings.Series[((ListControl)lbSeries).SelectedIndex] as StiSeries);
		}
	}

	private void CheckEnabledComponents(StiSeries series)
	{
		((Control)twBoxArguments).Enabled = true;
		((Control)twBoxOpenValues).Enabled = false;
		((Control)twBoxCloseValues).Enabled = false;
		((Control)twBoxHighValues).Enabled = false;
		((Control)twBoxLowValues).Enabled = false;
		((Control)twBoxEndValues).Enabled = false;
		((Control)twBoxValues).Enabled = false;
		((Control)twBoxValues).Enabled = false;
		if (series is StiCandlestickSeries || series is StiStockSeries)
		{
			((Control)twBoxOpenValues).Enabled = true;
			((Control)twBoxCloseValues).Enabled = true;
			((Control)twBoxHighValues).Enabled = true;
			((Control)twBoxLowValues).Enabled = true;
		}
		else if (series is StiRangeBarSeries || series is StiGanttSeries || series is StiRangeSeries || series is StiSplineRangeSeries || series is StiSteppedRangeSeries)
		{
			((Control)twBoxEndValues).Enabled = true;
			((Control)twBoxValues).Enabled = true;
		}
		else
		{
			((Control)twBoxValues).Enabled = true;
		}
	}

	private void BtRemove_Click(object sender, EventArgs e)
	{
		if (tmpChartSettings.Series.Count > 0)
		{
			tmpChartSettings.Series.RemoveAt(((ListControl)lbSeries).SelectedIndex);
			int num = ((ListControl)lbSeries).SelectedIndex;
			if (num == lbSeries.Items.Count - 1)
			{
				num--;
				lastSelectedSeries = -1;
			}
			lbSeries.Items.RemoveAt(((ListControl)lbSeries).SelectedIndex);
			if (lbSeries.Items.Count > 0)
			{
				((ListControl)lbSeries).SelectedIndex = num;
			}
			((Control)dgPoints).Enabled = lbSeries.Items.Count > 0;
			((Control)twBoxArguments).Enabled = lbSeries.Items.Count > 0;
			((Control)twBoxValues).Enabled = lbSeries.Items.Count > 0;
			((Control)twBoxEndValues).Enabled = lbSeries.Items.Count > 0;
			((Control)twBoxOpenValues).Enabled = lbSeries.Items.Count > 0;
			((Control)twBoxCloseValues).Enabled = lbSeries.Items.Count > 0;
			((Control)twBoxHighValues).Enabled = lbSeries.Items.Count > 0;
			((Control)twBoxLowValues).Enabled = lbSeries.Items.Count > 0;
			if (lbSeries.Items.Count == 0)
			{
				((Control)tbSeriesName).Text = "";
			}
		}
	}

	private void BtMoveUp_Click(object sender, EventArgs e)
	{
		int selectedIndex = ((ListControl)lbSeries).SelectedIndex;
		if (selectedIndex > 0)
		{
			StiSeries value = tmpChartSettings.Series[selectedIndex] as StiSeries;
			tmpChartSettings.Series.RemoveAt(selectedIndex);
			tmpChartSettings.Series.Insert(selectedIndex - 1, value);
			object obj = lbSeries.Items[selectedIndex];
			lastSelectedSeries = -1;
			lbSeries.Items.RemoveAt(selectedIndex);
			lbSeries.Items.Insert(selectedIndex - 1, obj);
			((ListControl)lbSeries).SelectedIndex = selectedIndex - 1;
			((Control)lbSeries).Invalidate();
		}
	}

	private void btMoveDown_Click(object sender, EventArgs e)
	{
		int selectedIndex = ((ListControl)lbSeries).SelectedIndex;
		if (selectedIndex < lbSeries.Items.Count - 1)
		{
			StiSeries value = tmpChartSettings.Series[selectedIndex] as StiSeries;
			tmpChartSettings.Series.RemoveAt(selectedIndex);
			tmpChartSettings.Series.Insert(selectedIndex + 1, value);
			object obj = lbSeries.Items[selectedIndex];
			lastSelectedSeries = -1;
			lbSeries.Items.RemoveAt(selectedIndex);
			lbSeries.Items.Insert(selectedIndex + 1, obj);
			((ListControl)lbSeries).SelectedIndex = selectedIndex + 1;
			((Control)lbSeries).Invalidate();
		}
	}

	private void tbSeriesName_TextChanged(object sender, EventArgs e)
	{
		if (((ListControl)lbSeries).SelectedIndex >= 0)
		{
			((StiSeries)tmpChartSettings.Series[((ListControl)lbSeries).SelectedIndex]).CoreTitle = ((Control)tbSeriesName).Text;
			((Control)lbSeries).Invalidate();
		}
	}

	private void lbSeries_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (lastSelectedSeries >= 0)
		{
			SetSeriesData(tmpChartSettings.Series[lastSelectedSeries] as StiSeries);
		}
		if (((ListControl)lbSeries).SelectedIndex >= 0)
		{
			((Control)tbMoveUp).Enabled = ((ListControl)lbSeries).SelectedIndex > 0;
			((Control)tbMoveDown).Enabled = ((ListControl)lbSeries).SelectedIndex + 1 < lbSeries.Items.Count;
			((Control)tbSeriesName).Text = ((StiSeries)tmpChartSettings.Series[((ListControl)lbSeries).SelectedIndex]).CoreTitle;
			GetSeriesData(tmpChartSettings.Series[((ListControl)lbSeries).SelectedIndex] as StiSeries);
		}
		lastSelectedSeries = ((ListControl)lbSeries).SelectedIndex;
		if (lastSelectedSeries >= 0)
		{
			GetSeriesData(tmpChartSettings.Series[lastSelectedSeries] as StiSeries);
		}
		else
		{
			dtPoints.Clear();
		}
	}

	private void SetSeriesData(StiSeries series)
	{
		if (series is StiCandlestickSeries)
		{
			SetSeriesData(series, ((Control)twBoxArguments).Text, ((Control)twBoxOpenValues).Text, ((Control)twBoxCloseValues).Text, ((Control)twBoxHighValues).Text, ((Control)twBoxLowValues).Text);
		}
		else if (series is StiRangeBarSeries || series is StiGanttSeries || series is StiRangeSeries || series is StiSplineRangeSeries || series is StiSteppedRangeSeries)
		{
			SetSeriesData(series, ((Control)twBoxArguments).Text, ((Control)twBoxValues).Text, ((Control)twBoxEndValues).Text);
		}
		else
		{
			SetSeriesData(series, ((Control)twBoxArguments).Text, ((Control)twBoxValues).Text);
		}
	}

	private void SetSeriesData(StiSeries series, string argumentDataColumn, string valueDataColumn)
	{
		series.ArgumentDataColumn = argumentDataColumn;
		series.ValueDataColumn = valueDataColumn;
		if (argumentDataColumn.Length == 0 && valueDataColumn.Length == 0)
		{
			if (dtPoints.Rows.Count <= 0)
			{
				return;
			}
			int num = 0;
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			if (((Control)twBoxArguments).Text.Length != 0 || ((Control)twBoxValues).Text.Length != 0)
			{
				return;
			}
			foreach (DataRow row in dtPoints.Rows)
			{
				stringBuilder.Append((string)row.ItemArray.GetValue(0));
				stringBuilder2.Append((double)row.ItemArray.GetValue(1));
				if (num + 1 < dtPoints.Rows.Count)
				{
					stringBuilder.Append(";");
					stringBuilder2.Append(";");
				}
				num++;
			}
			series.ListOfArguments.Value = stringBuilder.ToString();
			series.ListOfValues.Value = stringBuilder2.ToString();
		}
		else
		{
			dtPoints.Rows.Clear();
		}
	}

	private void SetSeriesData(StiSeries series, string argumentDataColumn, string valueDataColumn, string endValueDataColumn)
	{
		StiRangeBarSeries stiRangeBarSeries = series as StiRangeBarSeries;
		StiRangeSeries stiRangeSeries = series as StiRangeSeries;
		StiSplineRangeSeries stiSplineRangeSeries = series as StiSplineRangeSeries;
		StiSteppedRangeSeries stiSteppedRangeSeries = series as StiSteppedRangeSeries;
		StiGanttSeries stiGanttSeries = series as StiGanttSeries;
		if (stiRangeBarSeries != null)
		{
			stiRangeBarSeries.ArgumentDataColumn = argumentDataColumn;
			stiRangeBarSeries.ValueDataColumn = valueDataColumn;
			stiRangeBarSeries.ValueDataColumnEnd = endValueDataColumn;
		}
		else if (stiRangeSeries != null)
		{
			stiRangeSeries.ArgumentDataColumn = argumentDataColumn;
			stiRangeSeries.ValueDataColumn = valueDataColumn;
			stiRangeSeries.ValueDataColumnEnd = endValueDataColumn;
		}
		else if (stiSplineRangeSeries != null)
		{
			stiSplineRangeSeries.ArgumentDataColumn = argumentDataColumn;
			stiSplineRangeSeries.ValueDataColumn = valueDataColumn;
			stiSplineRangeSeries.ValueDataColumnEnd = endValueDataColumn;
		}
		else if (stiSteppedRangeSeries != null)
		{
			stiSteppedRangeSeries.ArgumentDataColumn = argumentDataColumn;
			stiSteppedRangeSeries.ValueDataColumn = valueDataColumn;
			stiSteppedRangeSeries.ValueDataColumnEnd = endValueDataColumn;
		}
		else if (stiGanttSeries != null)
		{
			stiGanttSeries.ArgumentDataColumn = argumentDataColumn;
			stiGanttSeries.ValueDataColumn = valueDataColumn;
			stiGanttSeries.ValueDataColumnEnd = endValueDataColumn;
		}
		if (argumentDataColumn.Length == 0 && valueDataColumn.Length == 0 && endValueDataColumn.Length == 0)
		{
			if (dtPoints.Rows.Count <= 0)
			{
				return;
			}
			int num = 0;
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			StringBuilder stringBuilder3 = new StringBuilder();
			foreach (DataRow row in dtPoints.Rows)
			{
				stringBuilder.Append((string)row.ItemArray.GetValue(0));
				stringBuilder2.Append((double)row.ItemArray.GetValue(1));
				stringBuilder3.Append((double)row.ItemArray.GetValue(2));
				if (num + 1 < dtPoints.Rows.Count)
				{
					stringBuilder.Append(";");
					stringBuilder2.Append(";");
					stringBuilder3.Append(";");
				}
				num++;
			}
			if (stiRangeBarSeries != null)
			{
				stiRangeBarSeries.ListOfArguments.Value = stringBuilder.ToString();
				stiRangeBarSeries.ListOfValues.Value = stringBuilder2.ToString();
				stiRangeBarSeries.ListOfValuesEnd.Value = stringBuilder3.ToString();
			}
			else if (stiRangeSeries != null)
			{
				stiRangeSeries.ListOfArguments.Value = stringBuilder.ToString();
				stiRangeSeries.ListOfValues.Value = stringBuilder2.ToString();
				stiRangeSeries.ListOfValuesEnd.Value = stringBuilder3.ToString();
			}
			else if (stiSplineRangeSeries != null)
			{
				stiSplineRangeSeries.ListOfArguments.Value = stringBuilder.ToString();
				stiSplineRangeSeries.ListOfValues.Value = stringBuilder2.ToString();
				stiSplineRangeSeries.ListOfValuesEnd.Value = stringBuilder3.ToString();
			}
			else if (stiSteppedRangeSeries != null)
			{
				stiSteppedRangeSeries.ListOfArguments.Value = stringBuilder.ToString();
				stiSteppedRangeSeries.ListOfValues.Value = stringBuilder2.ToString();
				stiSteppedRangeSeries.ListOfValuesEnd.Value = stringBuilder3.ToString();
			}
			else if (stiGanttSeries != null)
			{
				stiGanttSeries.ListOfArguments.Value = stringBuilder.ToString();
				stiGanttSeries.ListOfValues.Value = stringBuilder2.ToString();
				stiGanttSeries.ListOfValuesEnd.Value = stringBuilder3.ToString();
			}
		}
		else
		{
			dtPoints.Rows.Clear();
		}
	}

	private void SetSeriesData(StiSeries series, string argumentDataColumn, string openValueDataColumn, string closeValueDataColumn, string highValueDataColumn, string lowValueDataColumn)
	{
		StiCandlestickSeries stiCandlestickSeries = series as StiCandlestickSeries;
		stiCandlestickSeries.ArgumentDataColumn = argumentDataColumn;
		stiCandlestickSeries.ValueDataColumnOpen = openValueDataColumn;
		stiCandlestickSeries.ValueDataColumnClose = closeValueDataColumn;
		stiCandlestickSeries.ValueDataColumnHigh = highValueDataColumn;
		stiCandlestickSeries.ValueDataColumnLow = lowValueDataColumn;
		if (argumentDataColumn.Length == 0 && openValueDataColumn.Length == 0 && closeValueDataColumn.Length == 0 && highValueDataColumn.Length == 0 && lowValueDataColumn.Length == 0)
		{
			if (dtPoints.Rows.Count <= 0)
			{
				return;
			}
			int num = 0;
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			StringBuilder stringBuilder3 = new StringBuilder();
			StringBuilder stringBuilder4 = new StringBuilder();
			StringBuilder stringBuilder5 = new StringBuilder();
			if (((Control)twBoxArguments).Text.Length != 0 || ((Control)twBoxOpenValues).Text.Length != 0 || ((Control)twBoxCloseValues).Text.Length != 0 || ((Control)twBoxHighValues).Text.Length != 0 || ((Control)twBoxLowValues).Text.Length != 0)
			{
				return;
			}
			foreach (DataRow row in dtPoints.Rows)
			{
				stringBuilder.Append((string)row.ItemArray.GetValue(0));
				stringBuilder2.Append((double)row.ItemArray.GetValue(1));
				stringBuilder3.Append((double)row.ItemArray.GetValue(2));
				stringBuilder4.Append((double)row.ItemArray.GetValue(3));
				stringBuilder5.Append((double)row.ItemArray.GetValue(4));
				if (num + 1 < dtPoints.Rows.Count)
				{
					stringBuilder.Append(";");
					stringBuilder2.Append(";");
					stringBuilder3.Append(";");
					stringBuilder4.Append(";");
					stringBuilder5.Append(";");
				}
				num++;
			}
			stiCandlestickSeries.ListOfArguments.Value = stringBuilder.ToString();
			stiCandlestickSeries.ListOfValuesOpen.Value = stringBuilder2.ToString();
			stiCandlestickSeries.ListOfValuesClose.Value = stringBuilder3.ToString();
			stiCandlestickSeries.ListOfValuesHigh.Value = stringBuilder4.ToString();
			stiCandlestickSeries.ListOfValuesLow.Value = stringBuilder5.ToString();
		}
		else
		{
			dtPoints.Rows.Clear();
		}
	}

	private void GetSeriesData(StiSeries series)
	{
		if (series is StiCandlestickSeries)
		{
			StiCandlestickSeries stiCandlestickSeries = series as StiCandlestickSeries;
			((Control)twBoxArguments).Text = stiCandlestickSeries.ArgumentDataColumn;
			((Control)twBoxOpenValues).Text = stiCandlestickSeries.ValueDataColumnOpen;
			((Control)twBoxCloseValues).Text = stiCandlestickSeries.ValueDataColumnClose;
			((Control)twBoxHighValues).Text = stiCandlestickSeries.ValueDataColumnHigh;
			((Control)twBoxLowValues).Text = stiCandlestickSeries.ValueDataColumnLow;
			int num = 0;
			dtPoints.Rows.Clear();
			if (((Control)twBoxArguments).Text.Length != 0 || ((Control)twBoxOpenValues).Text.Length != 0 || ((Control)twBoxCloseValues).Text.Length != 0 || ((Control)twBoxHighValues).Text.Length != 0 || ((Control)twBoxLowValues).Text.Length != 0)
			{
				return;
			}
			object[] arguments = stiCandlestickSeries.Arguments;
			foreach (object obj in arguments)
			{
				DataRow dataRow = dtPoints.NewRow();
				dataRow["Argument"] = stiCandlestickSeries.Arguments[num];
				if (stiCandlestickSeries.ValuesOpen.Length > num)
				{
					dataRow["Open Value"] = stiCandlestickSeries.ValuesOpen[num];
				}
				if (stiCandlestickSeries.ValuesClose.Length > num)
				{
					dataRow["Close Value"] = stiCandlestickSeries.ValuesClose[num];
				}
				if (stiCandlestickSeries.ValuesHigh.Length > num)
				{
					dataRow["High Value"] = stiCandlestickSeries.ValuesHigh[num];
				}
				if (stiCandlestickSeries.ValuesLow.Length > num)
				{
					dataRow["Low Value"] = stiCandlestickSeries.ValuesLow[num];
				}
				dtPoints.Rows.Add(dataRow);
				num++;
			}
			return;
		}
		if (series is StiRangeBarSeries || series is StiGanttSeries)
		{
			StiRangeBarSeries stiRangeBarSeries = series as StiRangeBarSeries;
			StiRangeSeries stiRangeSeries = series as StiRangeSeries;
			StiSplineRangeSeries stiSplineRangeSeries = series as StiSplineRangeSeries;
			StiSteppedRangeSeries stiSteppedRangeSeries = series as StiSteppedRangeSeries;
			StiGanttSeries stiGanttSeries = series as StiGanttSeries;
			if (stiRangeBarSeries != null)
			{
				((Control)twBoxArguments).Text = stiRangeBarSeries.ArgumentDataColumn;
				((Control)twBoxOpenValues).Text = stiRangeBarSeries.ValueDataColumn;
				((Control)twBoxCloseValues).Text = stiRangeBarSeries.ValueDataColumnEnd;
			}
			else if (stiRangeSeries != null)
			{
				((Control)twBoxArguments).Text = stiRangeSeries.ArgumentDataColumn;
				((Control)twBoxOpenValues).Text = stiRangeSeries.ValueDataColumn;
				((Control)twBoxCloseValues).Text = stiRangeSeries.ValueDataColumnEnd;
			}
			else if (stiSplineRangeSeries != null)
			{
				((Control)twBoxArguments).Text = stiSplineRangeSeries.ArgumentDataColumn;
				((Control)twBoxOpenValues).Text = stiSplineRangeSeries.ValueDataColumn;
				((Control)twBoxCloseValues).Text = stiSplineRangeSeries.ValueDataColumnEnd;
			}
			else if (stiSteppedRangeSeries != null)
			{
				((Control)twBoxArguments).Text = stiSteppedRangeSeries.ArgumentDataColumn;
				((Control)twBoxOpenValues).Text = stiSteppedRangeSeries.ValueDataColumn;
				((Control)twBoxCloseValues).Text = stiSteppedRangeSeries.ValueDataColumnEnd;
			}
			else if (stiGanttSeries != null)
			{
				((Control)twBoxArguments).Text = stiGanttSeries.ArgumentDataColumn;
				((Control)twBoxOpenValues).Text = stiGanttSeries.ValueDataColumn;
				((Control)twBoxCloseValues).Text = stiGanttSeries.ValueDataColumnEnd;
			}
			int num2 = 0;
			dtPoints.Rows.Clear();
			if (((Control)twBoxArguments).Text.Length != 0 || ((Control)twBoxOpenValues).Text.Length != 0 || ((Control)twBoxCloseValues).Text.Length != 0 || ((Control)twBoxHighValues).Text.Length != 0 || ((Control)twBoxLowValues).Text.Length != 0)
			{
				return;
			}
			if (stiRangeBarSeries != null)
			{
				object[] arguments2 = stiRangeBarSeries.Arguments;
				foreach (object obj2 in arguments2)
				{
					DataRow dataRow2 = dtPoints.NewRow();
					dataRow2["Argument"] = stiRangeBarSeries.Arguments[num2];
					if (stiRangeBarSeries.Values.Length > num2)
					{
						dataRow2["Value"] = stiRangeBarSeries.Values[num2];
					}
					if (stiRangeBarSeries.ValuesEnd.Length > num2)
					{
						dataRow2["End Value"] = stiRangeBarSeries.ValuesEnd[num2];
					}
					dtPoints.Rows.Add(dataRow2);
					num2++;
				}
			}
			if (stiRangeSeries != null)
			{
				object[] arguments3 = stiRangeSeries.Arguments;
				foreach (object obj3 in arguments3)
				{
					DataRow dataRow3 = dtPoints.NewRow();
					dataRow3["Argument"] = stiRangeSeries.Arguments[num2];
					if (stiGanttSeries.Values.Length > num2)
					{
						dataRow3["Value"] = stiRangeSeries.Values[num2];
					}
					if (stiGanttSeries.ValuesEnd.Length > num2)
					{
						dataRow3["End Value"] = stiRangeSeries.ValuesEnd[num2];
					}
					dtPoints.Rows.Add(dataRow3);
					num2++;
				}
			}
			if (stiSplineRangeSeries != null)
			{
				object[] arguments4 = stiSplineRangeSeries.Arguments;
				foreach (object obj4 in arguments4)
				{
					DataRow dataRow4 = dtPoints.NewRow();
					dataRow4["Argument"] = stiSplineRangeSeries.Arguments[num2];
					if (stiGanttSeries.Values.Length > num2)
					{
						dataRow4["Value"] = stiSplineRangeSeries.Values[num2];
					}
					if (stiGanttSeries.ValuesEnd.Length > num2)
					{
						dataRow4["End Value"] = stiSplineRangeSeries.ValuesEnd[num2];
					}
					dtPoints.Rows.Add(dataRow4);
					num2++;
				}
			}
			if (stiSteppedRangeSeries != null)
			{
				object[] arguments5 = stiSteppedRangeSeries.Arguments;
				foreach (object obj5 in arguments5)
				{
					DataRow dataRow5 = dtPoints.NewRow();
					dataRow5["Argument"] = stiSteppedRangeSeries.Arguments[num2];
					if (stiGanttSeries.Values.Length > num2)
					{
						dataRow5["Value"] = stiSteppedRangeSeries.Values[num2];
					}
					if (stiGanttSeries.ValuesEnd.Length > num2)
					{
						dataRow5["End Value"] = stiSteppedRangeSeries.ValuesEnd[num2];
					}
					dtPoints.Rows.Add(dataRow5);
					num2++;
				}
			}
			if (stiGanttSeries == null)
			{
				return;
			}
			object[] arguments6 = stiGanttSeries.Arguments;
			foreach (object obj6 in arguments6)
			{
				DataRow dataRow6 = dtPoints.NewRow();
				dataRow6["Argument"] = stiGanttSeries.Arguments[num2];
				if (stiGanttSeries.Values.Length > num2)
				{
					dataRow6["Value"] = stiGanttSeries.Values[num2];
				}
				if (stiGanttSeries.ValuesEnd.Length > num2)
				{
					dataRow6["End Value"] = stiGanttSeries.ValuesEnd[num2];
				}
				dtPoints.Rows.Add(dataRow6);
				num2++;
			}
			return;
		}
		((Control)twBoxArguments).Text = series.ArgumentDataColumn;
		((Control)twBoxValues).Text = series.ValueDataColumn;
		int num3 = 0;
		dtPoints.Rows.Clear();
		if (((Control)twBoxArguments).Text.Length != 0 || ((Control)twBoxValues).Text.Length != 0)
		{
			return;
		}
		double?[] values = series.Values;
		for (int num4 = 0; num4 < values.Length; num4++)
		{
			double value = values[num4].Value;
			DataRow dataRow7 = dtPoints.NewRow();
			if (series.Arguments.Length > num3)
			{
				dataRow7["Argument"] = series.Arguments[num3];
			}
			else
			{
				dataRow7["Argument"] = value.ToString();
			}
			dataRow7["Value"] = value;
			dtPoints.Rows.Add(dataRow7);
			num3++;
		}
	}

	private void dtPoints_RowChanged(object sender, DataRowChangeEventArgs e)
	{
		dtPoints.AcceptChanges();
	}

	private void wpLabels_VisibleChanged(object sender, EventArgs e)
	{
		if (!wpLabels.Visible)
		{
			return;
		}
		if (lastSelectedSeries >= 0)
		{
			dtPoints.AcceptChanges();
			SetSeriesData(tmpChartSettings.Series[lastSelectedSeries] as StiSeries);
		}
		tmpChartLabels = tmpChartSettings.Clone() as StiChart;
		if (tmpChartStyle.Area is StiAxisArea)
		{
			((StiAxisArea)tmpChartLabels.Area).AxisCore.SwitchOff();
		}
		tmpChartLabels.Area.ShowShadow = true;
		tmpChartLabels.Legend.Visible = false;
		int num = 0;
		StiService[] array = chartLabelsTypes;
		foreach (StiService stiService in array)
		{
			if (tmpChartLabels.Area.Core.CheckInLabelsTypes(stiService.GetType()))
			{
				num++;
			}
		}
		if (!tmpChartLabels.Area.Core.CheckInLabelsTypes(chartLabelsTypes[chartLabelTypeId].GetType()))
		{
			chartLabelTypeId = 0;
		}
		((ScrollBar)scrollableLabels.VScrollBar).Value = 0;
		scrollableLabels.ScrollHeight = (num - 1) / 2 * StiScale.I(185) + StiScale.I(186);
		if (((ListControl)lbSeries).SelectedIndex >= 0)
		{
			SetSeriesData(tmpChartSettings.Series[((ListControl)lbSeries).SelectedIndex] as StiSeries);
		}
		ApplySettings(sender, e);
		tmpChartSettings.Legend.Visible = false;
		((Control)cbxAutoRotation).Enabled = chartPieLabelsSettings != null;
	}

	private void cbxLabelsVisible_Click(object sender, EventArgs e)
	{
		if (chartAxisLabelsSettings != null)
		{
			if (((ComboBox)cbValueType).SelectedItem.ToString() == StiLocalization.Get("PropertyEnum", "StiSeriesLabelsValueTypeValue"))
			{
				chartAxisLabelsSettings.ValueType = StiSeriesLabelsValueType.Value;
			}
			if (((ComboBox)cbValueType).SelectedItem.ToString() == StiLocalization.Get("PropertyEnum", "StiSeriesLabelsValueTypeSeriesTitle"))
			{
				chartAxisLabelsSettings.ValueType = StiSeriesLabelsValueType.SeriesTitle;
			}
			if (((ComboBox)cbValueType).SelectedItem.ToString() == StiLocalization.Get("PropertyEnum", "StiSeriesLabelsValueTypeArgument"))
			{
				chartAxisLabelsSettings.ValueType = StiSeriesLabelsValueType.Argument;
			}
			if (((ComboBox)cbValueType).SelectedItem.ToString() == StiLocalization.Get("PropertyEnum", "StiSeriesLabelsValueTypeValueArgument"))
			{
				chartAxisLabelsSettings.ValueType = StiSeriesLabelsValueType.ValueArgument;
			}
			if (((ComboBox)cbValueType).SelectedItem.ToString() == StiLocalization.Get("PropertyEnum", "StiSeriesLabelsValueTypeArgumentValue"))
			{
				chartAxisLabelsSettings.ValueType = StiSeriesLabelsValueType.ArgumentValue;
			}
			if (((ComboBox)cbValueType).SelectedItem.ToString() == StiLocalization.Get("PropertyEnum", "StiSeriesLabelsValueTypeSeriesTitleValue"))
			{
				chartAxisLabelsSettings.ValueType = StiSeriesLabelsValueType.SeriesTitleValue;
			}
			if (((ComboBox)cbValueType).SelectedItem.ToString() == StiLocalization.Get("PropertyEnum", "StiSeriesLabelsValueTypeSeriesTitleArgument"))
			{
				chartAxisLabelsSettings.ValueType = StiSeriesLabelsValueType.SeriesTitleArgument;
			}
			chartAxisLabelsSettings.TextBefore = ((Control)tbTextBefore).Text;
			chartAxisLabelsSettings.TextAfter = ((Control)tbTextAfter).Text;
			chartAxisLabelsSettings.Angle = (float)udLabelsAngle.Value;
			tmpChartSettings.SeriesLabels = chartAxisLabelsSettings;
			((Control)scrollableLabels).Invalidate();
		}
		if (chartPieLabelsSettings != null)
		{
			if (((ComboBox)cbValueType).SelectedItem.ToString() == StiLocalization.Get("PropertyEnum", "StiSeriesLabelsValueTypeValue"))
			{
				chartPieLabelsSettings.ValueType = StiSeriesLabelsValueType.Value;
			}
			if (((ComboBox)cbValueType).SelectedItem.ToString() == StiLocalization.Get("PropertyEnum", "StiSeriesLabelsValueTypeSeriesTitle"))
			{
				chartPieLabelsSettings.ValueType = StiSeriesLabelsValueType.SeriesTitle;
			}
			if (((ComboBox)cbValueType).SelectedItem.ToString() == StiLocalization.Get("PropertyEnum", "StiSeriesLabelsValueTypeArgument"))
			{
				chartPieLabelsSettings.ValueType = StiSeriesLabelsValueType.Argument;
			}
			if (((ComboBox)cbValueType).SelectedItem.ToString() == StiLocalization.Get("PropertyEnum", "StiSeriesLabelsValueTypeValueArgument"))
			{
				chartPieLabelsSettings.ValueType = StiSeriesLabelsValueType.ValueArgument;
			}
			if (((ComboBox)cbValueType).SelectedItem.ToString() == StiLocalization.Get("PropertyEnum", "StiSeriesLabelsValueTypeArgumentValue"))
			{
				chartPieLabelsSettings.ValueType = StiSeriesLabelsValueType.ArgumentValue;
			}
			if (((ComboBox)cbValueType).SelectedItem.ToString() == StiLocalization.Get("PropertyEnum", "StiSeriesLabelsValueTypeSeriesTitleValue"))
			{
				chartPieLabelsSettings.ValueType = StiSeriesLabelsValueType.SeriesTitleValue;
			}
			if (((ComboBox)cbValueType).SelectedItem.ToString() == StiLocalization.Get("PropertyEnum", "StiSeriesLabelsValueTypeSeriesTitleArgument"))
			{
				chartPieLabelsSettings.ValueType = StiSeriesLabelsValueType.SeriesTitleArgument;
			}
			chartPieLabelsSettings.TextBefore = ((Control)tbTextBefore).Text;
			chartPieLabelsSettings.TextAfter = ((Control)tbTextAfter).Text;
			chartPieLabelsSettings.Angle = (float)udLabelsAngle.Value;
			chartPieLabelsSettings.AutoRotate = cbxAutoRotation.Checked;
			tmpChartSettings.SeriesLabels = chartPieLabelsSettings;
			((Control)scrollableLabels).Invalidate();
		}
	}

	private void scrollableLabels_Paint(object sender, PaintEventArgs e)
	{
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Expected O, but got Unknown
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Expected O, but got Unknown
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Expected O, but got Unknown
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Expected O, but got Unknown
		int num = ((Control)scrollableLabels).Width - ((Control)scrollableLabels.VScrollBar).Width - StiScale.I4;
		int num2 = num / 2;
		Graphics graphics = e.Graphics;
		int num3 = StiScale.I3;
		int num4 = 0;
		int num5 = -1;
		double zoom = tmpChartLabels.Report.Info.Zoom;
		tmpChartLabels.Report.Info.Zoom = 1.0;
		StiService[] array = chartLabelsTypes;
		foreach (StiService stiService in array)
		{
			num5++;
			if (!tmpChartLabels.Area.Core.CheckInLabelsTypes(stiService.GetType()))
			{
				continue;
			}
			tmpChartLabels.SeriesLabels = stiService.Clone() as StiSeriesLabels;
			if (tmpChartLabels.Style is StiCustomStyle)
			{
				StiCustomStyle stiCustomStyle = (StiCustomStyle)tmpChartLabels.Style;
				if (stiCustomStyle.CustomCore.ReportChartStyle != null)
				{
					tmpChartLabels.CustomStyleName = stiCustomStyle.CustomCore.ReportChartStyle.Name;
				}
				else
				{
					tmpChartLabels.CustomStyleName = "Custom Style";
				}
				((StiChartStyle)tmpChartLabels.Style).Core.Chart = tmpChartLabels;
			}
			tmpChartLabels.SeriesLabels.Core.ApplyStyle(tmpChartLabels.Style);
			tmpChartLabels.Core.ApplyStyle(tmpChartLabels.Style);
			Color color;
			if (num5 == chartLabelTypeId)
			{
				DrawSelectRect(graphics, num3, num4 - ((ScrollBar)scrollableLabels.VScrollBar).Value, num2, StiScale.I(170));
				color = Color.FromArgb(200, StiColors.Selected);
			}
			else
			{
				color = Color.FromArgb(200, Color.White);
			}
			Rectangle rectangle = new Rectangle(num3, num4 - ((ScrollBar)scrollableLabels.VScrollBar).Value + StiScale.I(22), num2, StiScale.I(150));
			if (rectangle.Top <= ((Control)scrollableLabels).Height && rectangle.Bottom >= 0)
			{
				tmpChartLabels.DrawChart(graphics, rectangle, useMargins: true, useBackground: false);
			}
			rectangle = new Rectangle(num3, num4 - ((ScrollBar)scrollableLabels.VScrollBar).Value + StiScale.I6, num2, StiScale.I(21));
			rectangle.Inflate(-StiScale.I2, -StiScale.I2);
			if (rectangle.Top <= ((Control)scrollableLabels).Height && rectangle.Bottom >= 0)
			{
				SolidBrush val = new SolidBrush(color);
				try
				{
					graphics.FillRectangle((Brush)(object)val, rectangle);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
				SolidBrush val2 = new SolidBrush(Color.Black);
				try
				{
					Font val3 = new Font("Arial", 8f);
					try
					{
						StringFormat val4 = new StringFormat();
						try
						{
							val4.Alignment = (StringAlignment)1;
							val4.LineAlignment = (StringAlignment)2;
							graphics.DrawString(stiService.ServiceName, val3, (Brush)(object)val2, (RectangleF)rectangle, val4);
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
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			num3 += num2;
			if (num3 > num2 * 2)
			{
				num3 = StiScale.I3;
				num4 += StiScale.I(185);
			}
		}
		tmpChartLabels.Report.Info.Zoom = zoom;
	}

	private void scrollableLabels_MouseDown(object sender, MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)e.Button != 1048576)
		{
			return;
		}
		int num = StiScale.I3;
		int num2 = 0;
		int num3 = -1;
		StiService[] array = chartLabelsTypes;
		foreach (StiService stiService in array)
		{
			num3++;
			if (!tmpChartLabels.Area.Core.CheckInLabelsTypes(stiService.GetType()))
			{
				continue;
			}
			if (new Rectangle(num, num2 - ((ScrollBar)scrollableLabels.VScrollBar).Value, StiScale.I(160), StiScale.I(170)).Contains(e.X, e.Y))
			{
				chartLabelTypeId = num3;
				chartAxisLabelsSettings = stiService.Clone() as StiAxisSeriesLabels;
				chartPieLabelsSettings = stiService.Clone() as StiCenterPieLabels;
				if (chartAxisLabelsSettings != null)
				{
					chartAxisLabelsSettings.Core.ApplyStyle(tmpChartSettings.Style);
				}
				if (chartPieLabelsSettings != null)
				{
					chartPieLabelsSettings.Core.ApplyStyle(tmpChartSettings.Style);
				}
				tmpChartSettings.SeriesLabels = stiService.Clone() as StiSeriesLabels;
				tmpChartSettings.SeriesLabels.Core.ApplyStyle(tmpChartSettings.Style);
				((Control)scrollableLabels).Invalidate();
				((Control)cbxAutoRotation).Enabled = chartPieLabelsSettings != null;
			}
			num += StiScale.I(164);
			if (num > StiScale.I(167))
			{
				num = StiScale.I3;
				num2 += StiScale.I(185);
			}
		}
	}

	private void wpAxes_VisibleChanged(object sender, EventArgs e)
	{
		if (wpAxes.Visible)
		{
			ApplySettings(sender, e);
			tmpChartSettings.Legend.Visible = false;
			tpAxisX.Enabled = chartAxesSettings != null;
			((Control)gbTitleX).Visible = chartAxesSettings != null;
			((Control)gbTicksX).Visible = chartAxesSettings != null;
			((Control)gbGridLinesX).Visible = chartAxesSettings != null;
			((Control)gbLabelsX).Visible = chartAxesSettings != null;
			tpAxisY.Enabled = chartAxesSettings != null;
			((Control)gbTitleY).Visible = chartAxesSettings != null;
			((Control)gbTicksY).Visible = chartAxesSettings != null;
			((Control)gbGridLinesY).Visible = chartAxesSettings != null;
			((Control)gbLabelsY).Visible = chartAxesSettings != null;
		}
	}

	private void scAxes_Paint(object sender, PaintEventArgs e)
	{
		Graphics graphics = e.Graphics;
		double zoom = tmpChartSettings.Report.Info.Zoom;
		tmpChartSettings.Report.Info.Zoom = 1.0;
		if (tmpChartSettings.Style is StiCustomStyle)
		{
			StiCustomStyle stiCustomStyle = (StiCustomStyle)tmpChartSettings.Style;
			if (stiCustomStyle.CustomCore.ReportChartStyle != null)
			{
				tmpChartSettings.CustomStyleName = stiCustomStyle.CustomCore.ReportChartStyle.Name;
			}
			else
			{
				tmpChartSettings.CustomStyleName = "Custom Style";
			}
			((StiChartStyle)tmpChartSettings.Style).Core.Chart = tmpChartSettings;
			tmpChartSettings.Core.ApplyStyle(tmpChartSettings.Style);
		}
		tmpChartSettings.DrawChart(graphics, ((Control)scAxes).ClientRectangle, useMargins: true, useBackground: false);
		tmpChartSettings.Report.Info.Zoom = zoom;
	}

	private void cbxVisibleX_Click(object sender, EventArgs e)
	{
		if (chartAxesSettings != null)
		{
			chartAxesSettings.XAxis.Visible = cbxVisibleX.Checked;
			chartAxesSettings.ReverseHor = cbxReverseX.Checked;
			chartAxesSettings.XAxis.Title.Text = ((Control)tbTitleX).Text;
			if (((ComboBox)cbAligmentX).SelectedItem.ToString() == StiLocalization.Get("PropertyEnum", "StringAlignmentNear"))
			{
				chartAxesSettings.XAxis.Title.Alignment = (StringAlignment)0;
			}
			if (((ComboBox)cbAligmentX).SelectedItem.ToString() == StiLocalization.Get("PropertyEnum", "StringAlignmentCenter"))
			{
				chartAxesSettings.XAxis.Title.Alignment = (StringAlignment)1;
			}
			if (((ComboBox)cbAligmentX).SelectedItem.ToString() == StiLocalization.Get("PropertyEnum", "StringAlignmentFar"))
			{
				chartAxesSettings.XAxis.Title.Alignment = (StringAlignment)2;
			}
			chartAxesSettings.XAxis.Ticks.Visible = ((ComboBox)cbTicksX).SelectedItem.ToString() == StiLocalization.Get("PropertyEnum", "ChartAxesTicksMajor") || ((ComboBox)cbTicksX).SelectedItem.ToString() == StiLocalization.Get("PropertyEnum", "ChartAxesTicksAll");
			chartAxesSettings.XAxis.Ticks.MinorVisible = ((ComboBox)cbTicksX).SelectedItem.ToString() == StiLocalization.Get("PropertyEnum", "ChartAxesTicksAll");
			chartAxesSettings.XAxis.Ticks.MinorCount = (int)udMinorCountX.Value;
			chartAxesSettings.GridLinesHor.Visible = ((ComboBox)cbGridLinesX).SelectedItem.ToString() == StiLocalization.Get("PropertyEnum", "ChartGridLinesMajor") || ((ComboBox)cbGridLinesX).SelectedItem.ToString() == StiLocalization.Get("PropertyEnum", "ChartGridLinesAll");
			chartAxesSettings.GridLinesHor.MinorVisible = ((ComboBox)cbGridLinesX).SelectedItem.ToString() == StiLocalization.Get("PropertyEnum", "ChartGridLinesAll");
			chartAxesSettings.InterlacingHor.Visible = cbxInterlacedX.Checked;
			if (((ComboBox)cbLabelsX).SelectedItem.ToString() == StiLocalization.Get("PropertyEnum", "StiLabelsPlacementNone"))
			{
				chartAxesSettings.XAxis.Labels.Placement = StiLabelsPlacement.None;
			}
			if (((ComboBox)cbLabelsX).SelectedItem.ToString() == StiLocalization.Get("PropertyEnum", "StiLabelsPlacementOneLine"))
			{
				chartAxesSettings.XAxis.Labels.Placement = StiLabelsPlacement.OneLine;
			}
			if (((ComboBox)cbLabelsX).SelectedItem.ToString() == StiLocalization.Get("PropertyEnum", "StiLabelsPlacementTwoLines"))
			{
				chartAxesSettings.XAxis.Labels.Placement = StiLabelsPlacement.TwoLines;
			}
			chartAxesSettings.XAxis.Labels.Angle = (float)udAngleX.Value;
			tmpChartSettings.Area = chartAxesSettings;
			((Control)scAxes).Invalidate();
		}
	}

	private void cbxVisibleY_Click(object sender, EventArgs e)
	{
		if (chartAxesSettings != null)
		{
			chartAxesSettings.YAxis.Visible = cbxVisibleY.Checked;
			chartAxesSettings.ReverseVert = cbxReverseY.Checked;
			chartAxesSettings.YAxis.Title.Text = ((Control)tbTitleY).Text;
			if (((ComboBox)cbAligmentY).SelectedItem.ToString() == StiLocalization.Get("PropertyEnum", "StringAlignmentNear"))
			{
				chartAxesSettings.YAxis.Title.Alignment = (StringAlignment)0;
			}
			if (((ComboBox)cbAligmentY).SelectedItem.ToString() == StiLocalization.Get("PropertyEnum", "StringAlignmentCenter"))
			{
				chartAxesSettings.YAxis.Title.Alignment = (StringAlignment)1;
			}
			if (((ComboBox)cbAligmentY).SelectedItem.ToString() == StiLocalization.Get("PropertyEnum", "StringAlignmentFar"))
			{
				chartAxesSettings.YAxis.Title.Alignment = (StringAlignment)2;
			}
			chartAxesSettings.YAxis.Ticks.Visible = ((ComboBox)cbTicksY).SelectedItem.ToString() == StiLocalization.Get("PropertyEnum", "ChartAxesTicksMajor") || ((ComboBox)cbTicksY).SelectedItem.ToString() == StiLocalization.Get("PropertyEnum", "ChartAxesTicksAll");
			chartAxesSettings.YAxis.Ticks.MinorVisible = ((ComboBox)cbTicksY).SelectedItem.ToString() == StiLocalization.Get("PropertyEnum", "ChartAxesTicksAll");
			chartAxesSettings.YAxis.Ticks.MinorCount = (int)udMinorCountY.Value;
			chartAxesSettings.GridLinesVert.Visible = ((ComboBox)cbGridLinesY).SelectedItem.ToString() == StiLocalization.Get("PropertyEnum", "ChartGridLinesMajor") || ((ComboBox)cbGridLinesY).SelectedItem.ToString() == StiLocalization.Get("PropertyEnum", "ChartGridLinesAll");
			chartAxesSettings.GridLinesVert.MinorVisible = ((ComboBox)cbGridLinesY).SelectedItem.ToString() == StiLocalization.Get("PropertyEnum", "ChartGridLinesAll");
			chartAxesSettings.InterlacingVert.Visible = cbxInterlacedY.Checked;
			if (((ComboBox)cbLabelsY).SelectedItem.ToString() == StiLocalization.Get("PropertyEnum", "StiLabelsPlacementNone"))
			{
				chartAxesSettings.YAxis.Labels.Placement = StiLabelsPlacement.None;
			}
			if (((ComboBox)cbLabelsY).SelectedItem.ToString() == StiLocalization.Get("PropertyEnum", "StiLabelsPlacementOneLine"))
			{
				chartAxesSettings.YAxis.Labels.Placement = StiLabelsPlacement.OneLine;
			}
			if (((ComboBox)cbLabelsY).SelectedItem.ToString() == StiLocalization.Get("PropertyEnum", "StiLabelsPlacementTwoLines"))
			{
				chartAxesSettings.YAxis.Labels.Placement = StiLabelsPlacement.TwoLines;
			}
			chartAxesSettings.YAxis.Labels.Angle = (float)udAngleY.Value;
			tmpChartSettings.Area = chartAxesSettings;
			((Control)scAxes).Invalidate();
		}
	}

	private void wpLegend_VisibleChanged(object sender, EventArgs e)
	{
		if (wpLegend.Visible)
		{
			ApplySettings(sender, e);
			tmpChartSettings.Legend.Visible = true;
		}
	}

	private void scLegend_Paint(object sender, PaintEventArgs e)
	{
		Graphics graphics = e.Graphics;
		double zoom = tmpChartSettings.Report.Info.Zoom;
		tmpChartSettings.Report.Info.Zoom = 1.0;
		if (tmpChartSettings.Style is StiCustomStyle)
		{
			StiCustomStyle stiCustomStyle = (StiCustomStyle)tmpChartSettings.Style;
			if (stiCustomStyle.CustomCore.ReportChartStyle != null)
			{
				tmpChartSettings.CustomStyleName = stiCustomStyle.CustomCore.ReportChartStyle.Name;
			}
			else
			{
				tmpChartSettings.CustomStyleName = "Custom Style";
			}
			((StiChartStyle)tmpChartSettings.Style).Core.Chart = tmpChartSettings;
			tmpChartSettings.Core.ApplyStyle(tmpChartSettings.Style);
		}
		tmpChartSettings.DrawChart(graphics, ((Control)scLegends).ClientRectangle, useMargins: true, useBackground: false);
		tmpChartSettings.Report.Info.Zoom = zoom;
	}

	private void cbxLegendVisible_Click(object sender, EventArgs e)
	{
		if (chartLegendSettings != null)
		{
			chartLegendSettings.Visible = cbxLegendVisible.Checked;
			chartLegendSettings.Title = ((Control)tbLegendTitle).Text;
			if (((ComboBox)cbHorizontalAligment).SelectedItem.ToString() == StiLocalization.Get("PropertyEnum", "StiLegendHorAlignmentLeftOutside"))
			{
				chartLegendSettings.HorAlignment = StiLegendHorAlignment.LeftOutside;
			}
			if (((ComboBox)cbHorizontalAligment).SelectedItem.ToString() == StiLocalization.Get("PropertyEnum", "StiLegendHorAlignmentLeft"))
			{
				chartLegendSettings.HorAlignment = StiLegendHorAlignment.Left;
			}
			if (((ComboBox)cbHorizontalAligment).SelectedItem.ToString() == StiLocalization.Get("PropertyEnum", "StiLegendHorAlignmentCenter"))
			{
				chartLegendSettings.HorAlignment = StiLegendHorAlignment.Center;
			}
			if (((ComboBox)cbHorizontalAligment).SelectedItem.ToString() == StiLocalization.Get("PropertyEnum", "StiLegendHorAlignmentRight"))
			{
				chartLegendSettings.HorAlignment = StiLegendHorAlignment.Right;
			}
			if (((ComboBox)cbHorizontalAligment).SelectedItem.ToString() == StiLocalization.Get("PropertyEnum", "StiLegendHorAlignmentRightOutside"))
			{
				chartLegendSettings.HorAlignment = StiLegendHorAlignment.RightOutside;
			}
			if (((ComboBox)cbVerticalAligment).SelectedItem.ToString() == StiLocalization.Get("PropertyEnum", "StiLegendVertAlignmentTopOutside"))
			{
				chartLegendSettings.VertAlignment = StiLegendVertAlignment.TopOutside;
			}
			if (((ComboBox)cbVerticalAligment).SelectedItem.ToString() == StiLocalization.Get("PropertyEnum", "StiLegendVertAlignmentTop"))
			{
				chartLegendSettings.VertAlignment = StiLegendVertAlignment.Top;
			}
			if (((ComboBox)cbVerticalAligment).SelectedItem.ToString() == StiLocalization.Get("PropertyEnum", "StiLegendVertAlignmentCenter"))
			{
				chartLegendSettings.VertAlignment = StiLegendVertAlignment.Center;
			}
			if (((ComboBox)cbVerticalAligment).SelectedItem.ToString() == StiLocalization.Get("PropertyEnum", "StiLegendVertAlignmentBottom"))
			{
				chartLegendSettings.VertAlignment = StiLegendVertAlignment.Bottom;
			}
			if (((ComboBox)cbVerticalAligment).SelectedItem.ToString() == StiLocalization.Get("PropertyEnum", "StiLegendVertAlignmentBottomOutside"))
			{
				chartLegendSettings.VertAlignment = StiLegendVertAlignment.BottomOutside;
			}
			if (((ComboBox)cbDirection).SelectedItem.ToString() == StiLocalization.Get("PropertyEnum", "StiLegendDirectionLeftToRight"))
			{
				chartLegendSettings.Direction = StiLegendDirection.LeftToRight;
			}
			if (((ComboBox)cbDirection).SelectedItem.ToString() == StiLocalization.Get("PropertyEnum", "StiLegendDirectionRightToLeft"))
			{
				chartLegendSettings.Direction = StiLegendDirection.RightToLeft;
			}
			if (((ComboBox)cbDirection).SelectedItem.ToString() == StiLocalization.Get("PropertyEnum", "StiLegendDirectionTopToBottom"))
			{
				chartLegendSettings.Direction = StiLegendDirection.TopToBottom;
			}
			if (((ComboBox)cbDirection).SelectedItem.ToString() == StiLocalization.Get("PropertyEnum", "StiLegendDirectionBottomToTop"))
			{
				chartLegendSettings.Direction = StiLegendDirection.BottomToTop;
			}
			chartLegendSettings.MarkerVisible = cbxMarkerVisible.Checked;
			chartLegendSettings.MarkerSize = new Size((int)udWidth.Value, (int)udHeight.Value);
			chartLegendSettings.HorSpacing = (int)udHorizontalSpacing.Value;
			chartLegendSettings.VertSpacing = (int)udVerticalSpacing.Value;
			tmpChartSettings.Legend = chartLegendSettings.Clone() as StiLegend;
			((Control)scLegends).Invalidate();
		}
	}

	private void Wizard_FinishClick(object sender, EventArgs e)
	{
		chart.Area = tmpChartSettings.Area;
		chart.Style = tmpChartSettings.Style;
		chart.Series = tmpChartSettings.Series;
		chart.SeriesLabels = tmpChartSettings.SeriesLabels;
		chart.Legend = tmpChartSettings.Legend;
		for (int i = 0; i < chart.Series.Count; i++)
		{
			((StiSeries)chart.Series[i]).Chart = chart;
			if (((StiSeries)chart.Series[i]).ArgumentDataColumn.Length > 0 || ((StiSeries)chart.Series[i]).ValueDataColumn.Length > 0)
			{
				((StiSeries)chart.Series[i]).ListOfArguments.Value = "";
				((StiSeries)chart.Series[i]).ListOfValues.Value = "";
			}
		}
		((Form)this).DialogResult = (DialogResult)1;
	}

	private void Wizard_CancelClick(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void ButtonNewDataSource_Click(object sender, EventArgs e)
	{
		if (chart.Report.Designer != null)
		{
			StiDictionaryPanelService service = StiDictionaryPanelService.GetService(chart.Report.Designer.DesignerControl as StiDesignerControl);
			service.InvokeConnectionNew();
		}
	}

	private void Wizard_ChangeSelected(object sender, EventArgs e)
	{
		if (Wizard.SelectedIndex == 0)
		{
			((Control)scChartType).Focus();
		}
		else if (Wizard.SelectedIndex == 1)
		{
			((Control)scChartStyle).Focus();
		}
	}

	private void ControlChartType_DoubleClick(object sender, EventArgs e)
	{
		if (((Control)Wizard.NextButton).Enabled)
		{
			Wizard.SelectedIndex += 1;
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

	private void This_FormClosing(object sender, FormClosingEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		if ((int)((Form)this).DialogResult != 1)
		{
			for (int i = 0; i < chart.Series.Count; i++)
			{
				((StiSeries)chart.Series[i]).Chart = chart;
			}
			chart.Area = copyArea;
		}
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/reports_designer_creating_reports_in_designer_overview_chart_wizard.htm");
	}
}
