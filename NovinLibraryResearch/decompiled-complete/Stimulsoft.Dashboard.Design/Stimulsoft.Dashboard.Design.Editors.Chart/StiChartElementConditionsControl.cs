using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win;
using Stimulsoft.Dashboard.Components.Chart;
using Stimulsoft.Data.Helpers;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Components;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Design.Editors;
using Stimulsoft.Report.Design.Images;

namespace Stimulsoft.Dashboard.Design.Editors.Chart;

[ToolboxItem(false)]
public class StiChartElementConditionsControl : StiZoneControl
{
	private StiChartElement chartElement;

	private StiChartElementCondition condition;

	private IContainer components;

	public DateTimePicker dtpValue;

	private StiHitTestVisibleLabel lbFieldIs;

	public StiComboBox cbDataType;

	public StiComboBox cbItem;

	public StiComboBox cbCondition;

	private StiHitTestVisibleLabel lbCondition;

	private StiHitTestVisibleLabel lbDataType;

	private StiHitTestVisibleLabel lbValue;

	public Stimulsoft.Controls.StiNumericUpDown numericUpDownMarkerAngel;

	public StiComboBox comboBoxMarkerType;

	public StiHitTestVisibleLabel labelMarkerAngel;

	public StiHitTestVisibleLabel labelMarkerType;

	public StiColorBoxXF colorBoxColor;

	private StiHitTestVisibleLabel lbColor;

	public StiGroupLine groupLine;

	private StiPanelBox panelExpression;

	private HintTextBox tbValue;

	private StiOperationButton buttonExpression;

	public StiComboBox comboBoxExpression;

	public bool IsExpression
	{
		get
		{
			return ((ComboBox)comboBoxExpression).SelectedItem.ToString() == StiLocalization.Get("PropertyMain", "Expression");
		}
		set
		{
			((ComboBox)comboBoxExpression).SelectedItem = (value ? StiLocalization.Get("PropertyMain", "Expression") : StiLocalization.Get("PropertyMain", "Value"));
		}
	}

	public StiChartElementConditionsControl(StiChartElement chartElement, StiChartElementCondition condition)
	{
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			base.DrawBorder = false;
			Localize();
			dtpValue.Value = DateTime.Now;
			this.chartElement = chartElement;
			this.condition = condition;
			Build();
			DoScaling();
		}
	}

	private void Localize()
	{
		((Control)lbColor).Text = StiLocalization.Get("PropertyMain", "Color");
		((Control)lbCondition).Text = StiLocalization.Get("PropertyMain", "Condition");
		((Control)lbFieldIs).Text = StiLocalization.Get("PropertyMain", "FieldIs");
		((Control)lbDataType).Text = StiLocalization.Get("PropertyMain", "DataType");
		((Control)lbValue).Text = StiLocalization.Get("PropertyMain", "Value");
		((Control)labelMarkerAngel).Text = StiLocalization.Get("PropertyMain", "MarkerAngle");
		((Control)labelMarkerType).Text = StiLocalization.Get("PropertyMain", "MarkerType");
		((ComboBox)comboBoxExpression).Items.Add((object)StiLocalization.Get("PropertyMain", "Value"));
		((ComboBox)comboBoxExpression).Items.Add((object)StiLocalization.Get("PropertyMain", "Expression"));
		((ListControl)comboBoxExpression).SelectedIndex = 0;
	}

	private void ComboBoxDataType_SelectedIndexChanged(object sender, EventArgs e)
	{
		UpdateState();
	}

	private void ComboBoxItem_SelectedIndexChanged(object sender, EventArgs e)
	{
		UpdateState();
	}

	private void ComboBoxExpression_SelectedIndexChanged(object sender, EventArgs e)
	{
		UpdateState();
	}

	private void ButtonExpression_Click(object sender, EventArgs e)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Invalid comparison between Unknown and I4
		HintTextBox hintTextBox = tbValue;
		string text = ((Control)hintTextBox).Text;
		DialogResult val = StiTextEditor.EditText(chartElement.Report, ref text, StiCodeEditorMode.EditorDashboardExpression, Loc.Get("FormTitles", "ExpressionEditorForm"));
		if ((int)val == 1)
		{
			((Control)hintTextBox).Text = text;
		}
		((Control)hintTextBox).Focus();
		((TextBoxBase)hintTextBox).Select(0, 0);
	}

	private void DoScaling()
	{
		((ButtonBase)buttonExpression).Image = StiReportDesignImages.Actions.EditSmall();
	}

	private void UpdateState()
	{
		if (((ComboBox)cbItem).SelectedItem is StiChartMeterComboBoxItem)
		{
			StiChartMeterComboBoxItem stiChartMeterComboBoxItem = ((ComboBox)cbItem).SelectedItem as StiChartMeterComboBoxItem;
			if (stiChartMeterComboBoxItem.SeriesType == StiChartSeriesType.Line || stiChartMeterComboBoxItem.SeriesType == StiChartSeriesType.StackedLine || stiChartMeterComboBoxItem.SeriesType == StiChartSeriesType.FullStackedLine || stiChartMeterComboBoxItem.SeriesType == StiChartSeriesType.Spline || stiChartMeterComboBoxItem.SeriesType == StiChartSeriesType.StackedSpline || stiChartMeterComboBoxItem.SeriesType == StiChartSeriesType.FullStackedSpline || stiChartMeterComboBoxItem.SeriesType == StiChartSeriesType.SteppedLine || stiChartMeterComboBoxItem.SeriesType == StiChartSeriesType.Area || stiChartMeterComboBoxItem.SeriesType == StiChartSeriesType.StackedArea || stiChartMeterComboBoxItem.SeriesType == StiChartSeriesType.FullStackedArea || stiChartMeterComboBoxItem.SeriesType == StiChartSeriesType.SplineArea || stiChartMeterComboBoxItem.SeriesType == StiChartSeriesType.StackedSplineArea || stiChartMeterComboBoxItem.SeriesType == StiChartSeriesType.FullStackedSplineArea || stiChartMeterComboBoxItem.SeriesType == StiChartSeriesType.SteppedArea || stiChartMeterComboBoxItem.SeriesType == StiChartSeriesType.Scatter || stiChartMeterComboBoxItem.SeriesType == StiChartSeriesType.ScatterSpline || stiChartMeterComboBoxItem.SeriesType == StiChartSeriesType.ScatterLine || stiChartMeterComboBoxItem.SeriesType == StiChartSeriesType.RadarLine || stiChartMeterComboBoxItem.SeriesType == StiChartSeriesType.RadarArea || stiChartMeterComboBoxItem.SeriesType == StiChartSeriesType.RadarPoint)
			{
				StiHitTestVisibleLabel stiHitTestVisibleLabel = labelMarkerAngel;
				StiHitTestVisibleLabel stiHitTestVisibleLabel2 = labelMarkerType;
				StiComboBox stiComboBox = comboBoxMarkerType;
				bool flag2 = (((Control)numericUpDownMarkerAngel).Visible = true);
				bool flag4 = (((Control)stiComboBox).Visible = flag2);
				bool visible = (((Control)stiHitTestVisibleLabel2).Visible = flag4);
				((Control)stiHitTestVisibleLabel).Visible = visible;
				((ComboBox)comboBoxMarkerType).Items.Clear();
				foreach (StiMarkerType value in Enum.GetValues(typeof(StiMarkerType)))
				{
					((ComboBox)comboBoxMarkerType).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiMarkerType" + value));
					if (value == condition.MarkerType)
					{
						((ListControl)comboBoxMarkerType).SelectedIndex = ((ComboBox)comboBoxMarkerType).Items.Count - 1;
					}
				}
				numericUpDownMarkerAngel.Value = (decimal)condition.MarkerAngle;
			}
			else
			{
				StiHitTestVisibleLabel stiHitTestVisibleLabel3 = labelMarkerAngel;
				StiHitTestVisibleLabel stiHitTestVisibleLabel4 = labelMarkerType;
				StiComboBox stiComboBox2 = comboBoxMarkerType;
				bool flag2 = (((Control)numericUpDownMarkerAngel).Visible = false);
				bool flag4 = (((Control)stiComboBox2).Visible = flag2);
				bool visible = (((Control)stiHitTestVisibleLabel4).Visible = flag4);
				((Control)stiHitTestVisibleLabel3).Visible = visible;
			}
		}
		((Control)dtpValue).Visible = ((Control)cbDataType).Text == StiLocalization.Get("PropertyEnum", "StiFilterDataTypeDateTime") && !IsExpression;
		((Control)panelExpression).Visible = ((Control)cbDataType).Text != StiLocalization.Get("PropertyEnum", "StiFilterDataTypeDateTime") || IsExpression;
		string text = ((Control)cbCondition).Text;
		((ComboBox)cbCondition).Items.Clear();
		((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionEqualTo"));
		((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionNotEqualTo"));
		if (((Control)cbDataType).Text != StiLocalization.Get("PropertyEnum", "StiFilterDataTypeString") && ((Control)cbDataType).Text != StiLocalization.Get("PropertyEnum", "StiFilterDataTypeBoolean"))
		{
			((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionGreaterThan"));
			((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionGreaterThanOrEqualTo"));
			((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionLessThan"));
			((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionLessThanOrEqualTo"));
		}
		if (((Control)cbDataType).Text == StiLocalization.Get("PropertyEnum", "StiFilterDataTypeString"))
		{
			((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionContaining"));
			((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionNotContaining"));
			((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionBeginningWith"));
			((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionEndingWith"));
		}
		foreach (string item in ((ComboBox)cbCondition).Items)
		{
			if (item == text)
			{
				((Control)cbCondition).Text = text;
				return;
			}
		}
		((ListControl)cbCondition).SelectedIndex = 0;
	}

	private void Build()
	{
		string text = StiFilterHelper.ConvertConditionToString(condition.Condition);
		string text2 = StiFilterHelper.ConvertDataTypeToString(condition.DataType);
		colorBoxColor.SelectedColor = condition.Color;
		FillFieldsIs();
		((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionEqualTo"));
		((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionNotEqualTo"));
		((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionGreaterThan"));
		((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionGreaterThanOrEqualTo"));
		((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionLessThan"));
		((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionLessThanOrEqualTo"));
		((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionContaining"));
		((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionNotContaining"));
		((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionBeginningWith"));
		((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionEndingWith"));
		((Control)cbCondition).Text = text;
		((ComboBox)cbDataType).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterDataTypeNumeric"));
		((ComboBox)cbDataType).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterDataTypeString"));
		((ComboBox)cbDataType).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterDataTypeDateTime"));
		((ComboBox)cbDataType).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterDataTypeBoolean"));
		((Control)cbDataType).Text = text2;
		if (text2 == StiLocalization.Get("PropertyEnum", "StiFilterDataTypeDateTime") && !condition.IsExpression)
		{
			if (DateTime.TryParse(condition.Value, new CultureInfo("en-US", useUserOverride: false), DateTimeStyles.None, out var result))
			{
				dtpValue.Value = result;
			}
		}
		else
		{
			((Control)tbValue).Text = condition.Value;
		}
		IsExpression = condition.IsExpression;
		((Control)this).Width = 1;
	}

	private void FillFieldsIs()
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		foreach (StiValueChartMeter value in chartElement.Values)
		{
			string label = StiLabelHelper.GetLabel(value);
			if (dictionary.ContainsKey(label))
			{
				dictionary[label] = 2;
			}
			else
			{
				dictionary.Add(label, 1);
			}
		}
		foreach (StiArgumentChartMeter argument in chartElement.Arguments)
		{
			string label2 = StiLabelHelper.GetLabel(argument);
			if (dictionary.ContainsKey(label2))
			{
				dictionary[label2] = 2;
			}
			else
			{
				dictionary.Add(label2, 1);
			}
		}
		string label3 = StiLabelHelper.GetLabel(chartElement.Series);
		if (dictionary.ContainsKey(label3))
		{
			dictionary[label3] = 2;
		}
		else
		{
			dictionary.Add(label3, 1);
		}
		bool flag = true;
		foreach (StiValueChartMeter value2 in chartElement.Values)
		{
			string text = StiLabelHelper.GetLabel(value2);
			if (dictionary[text] != 1)
			{
				text = text + " - " + StiLocalization.Get("PropertyMain", "Value");
			}
			StiChartMeterComboBoxItem stiChartMeterComboBoxItem = new StiChartMeterComboBoxItem
			{
				Text = text,
				Key = value2.Key,
				SeriesType = value2.SeriesType,
				Field = StiChartConditionalField.Value
			};
			((ComboBox)cbItem).Items.Add((object)stiChartMeterComboBoxItem);
			if (value2.Key == condition.KeyValueMeter)
			{
				((ComboBox)cbItem).SelectedItem = stiChartMeterComboBoxItem;
				flag = false;
			}
		}
		foreach (StiArgumentChartMeter argument2 in chartElement.Arguments)
		{
			string text2 = StiLabelHelper.GetLabel(argument2);
			if (dictionary[text2] != 1)
			{
				text2 = text2 + " - " + StiLocalization.Get("PropertyMain", "Argument");
			}
			StiChartMeterComboBoxItem stiChartMeterComboBoxItem2 = new StiChartMeterComboBoxItem
			{
				Text = text2,
				Key = argument2.Key,
				Field = StiChartConditionalField.Argument
			};
			((ComboBox)cbItem).Items.Add((object)stiChartMeterComboBoxItem2);
			if (argument2.Key == condition.KeyValueMeter)
			{
				((ComboBox)cbItem).SelectedItem = stiChartMeterComboBoxItem2;
				flag = false;
			}
		}
		StiSeriesChartMeter series = chartElement.Series;
		if (series != null)
		{
			string text3 = StiLabelHelper.GetLabel(series);
			if (dictionary[text3] != 1)
			{
				text3 = text3 + " - " + StiLocalization.Get("PropertyMain", "Series");
			}
			StiChartMeterComboBoxItem stiChartMeterComboBoxItem3 = new StiChartMeterComboBoxItem
			{
				Text = text3,
				Key = series.Key,
				Field = StiChartConditionalField.Series
			};
			((ComboBox)cbItem).Items.Add((object)stiChartMeterComboBoxItem3);
			if (series.Key == condition.KeyValueMeter)
			{
				((ComboBox)cbItem).SelectedItem = stiChartMeterComboBoxItem3;
				flag = false;
			}
		}
		if (flag && ((ComboBox)cbItem).Items.Count > 0)
		{
			((ListControl)cbItem).SelectedIndex = 0;
		}
	}

	public StiChartElementCondition GetChartElementCondition()
	{
		string keyValueMeter = ((((ComboBox)cbItem).SelectedItem != null) ? ((StiChartMeterComboBoxItem)((ComboBox)cbItem).SelectedItem).Key : "");
		StiChartConditionalField field = ((((ComboBox)cbItem).SelectedItem != null) ? ((StiChartMeterComboBoxItem)((ComboBox)cbItem).SelectedItem).Field : StiChartConditionalField.Value);
		Stimulsoft.Report.Components.StiFilterCondition stiFilterCondition = StiFilterHelper.ConvertStringToCondition(((Control)cbCondition).Text);
		Stimulsoft.Report.Components.StiFilterDataType stiFilterDataType = StiFilterHelper.ConvertStringToDataType(((Control)cbDataType).Text);
		StiMarkerType markerType = StiFilterHelper.ConvertStringToMarkerType(((Control)comboBoxMarkerType).Text);
		string value = null;
		if (stiFilterDataType == Stimulsoft.Report.Components.StiFilterDataType.DateTime && !IsExpression)
		{
			CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
			try
			{
				Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
				value = dtpValue.Value.ToShortDateString();
			}
			finally
			{
				Thread.CurrentThread.CurrentCulture = currentCulture;
			}
		}
		else
		{
			value = ((Control)tbValue).Text;
		}
		return new StiChartElementCondition(keyValueMeter, colorBoxColor.SelectedColor, stiFilterDataType, stiFilterCondition, value, markerType, (float)numericUpDownMarkerAngel.Value, field, IsExpression);
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
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_034f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_043c: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0546: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0643: Unknown result type (might be due to invalid IL or missing references)
		//IL_06db: Unknown result type (might be due to invalid IL or missing references)
		//IL_077c: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0949: Unknown result type (might be due to invalid IL or missing references)
		//IL_096a: Unknown result type (might be due to invalid IL or missing references)
		//IL_09da: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a34: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a3e: Expected O, but got Unknown
		//IL_0a5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b24: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc6: Unknown result type (might be due to invalid IL or missing references)
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiChartElementConditionsControl));
		dtpValue = new DateTimePicker();
		lbFieldIs = new StiHitTestVisibleLabel();
		cbDataType = new StiComboBox();
		cbItem = new StiComboBox();
		cbCondition = new StiComboBox();
		lbCondition = new StiHitTestVisibleLabel();
		lbDataType = new StiHitTestVisibleLabel();
		lbValue = new StiHitTestVisibleLabel();
		numericUpDownMarkerAngel = new Stimulsoft.Controls.StiNumericUpDown();
		comboBoxMarkerType = new StiComboBox();
		labelMarkerAngel = new StiHitTestVisibleLabel();
		labelMarkerType = new StiHitTestVisibleLabel();
		colorBoxColor = new StiColorBoxXF();
		lbColor = new StiHitTestVisibleLabel();
		groupLine = new StiGroupLine();
		panelExpression = new StiPanelBox();
		tbValue = new HintTextBox();
		buttonExpression = new StiOperationButton();
		comboBoxExpression = new StiComboBox();
		((ISupportInitialize)numericUpDownMarkerAngel).BeginInit();
		((Control)panelExpression).SuspendLayout();
		((Control)this).SuspendLayout();
		dtpValue.Format = (DateTimePickerFormat)2;
		((Control)dtpValue).Location = new Point(716, 54);
		((Control)dtpValue).Margin = new Padding(6);
		((Control)dtpValue).Name = "dtpValue";
		((Control)dtpValue).Size = new Size(236, 31);
		((Control)dtpValue).TabIndex = 4;
		((Control)lbFieldIs).BackColor = Color.Transparent;
		((Control)lbFieldIs).Location = new Point(28, 23);
		((Control)lbFieldIs).Margin = new Padding(6, 0, 6, 0);
		((Control)lbFieldIs).Name = "lbFieldIs";
		((Control)lbFieldIs).Size = new Size(208, 29);
		((Control)lbFieldIs).TabIndex = 34;
		((Control)lbFieldIs).Text = "Field Is";
		((Control)cbDataType).BackColor = SystemColors.Window;
		cbDataType.DropDownStyle = (ComboBoxStyle)2;
		((Control)cbDataType).Location = new Point(260, 54);
		((Control)cbDataType).Margin = new Padding(6);
		((Control)cbDataType).Name = "cbDataType";
		((Control)cbDataType).Size = new Size(204, 42);
		((Control)cbDataType).TabIndex = 2;
		((ComboBox)cbDataType).SelectedIndexChanged += ComboBoxDataType_SelectedIndexChanged;
		((Control)cbItem).BackColor = SystemColors.Window;
		cbItem.DropDownStyle = (ComboBoxStyle)2;
		((Control)cbItem).Location = new Point(28, 54);
		((Control)cbItem).Margin = new Padding(6);
		((Control)cbItem).Name = "cbItem";
		((Control)cbItem).Size = new Size(208, 42);
		((Control)cbItem).TabIndex = 1;
		((ComboBox)cbItem).SelectedIndexChanged += ComboBoxItem_SelectedIndexChanged;
		cbCondition.AutoDropDownWidth = false;
		((Control)cbCondition).BackColor = SystemColors.Window;
		cbCondition.DropDownStyle = (ComboBoxStyle)2;
		((ComboBox)cbCondition).DropDownWidth = 300;
		((Control)cbCondition).Location = new Point(488, 54);
		((Control)cbCondition).Margin = new Padding(6);
		((Control)cbCondition).Name = "cbCondition";
		((Control)cbCondition).Size = new Size(204, 42);
		((Control)cbCondition).TabIndex = 3;
		((Control)lbCondition).BackColor = Color.Transparent;
		((Control)lbCondition).Location = new Point(488, 23);
		((Control)lbCondition).Margin = new Padding(6, 0, 6, 0);
		((Control)lbCondition).Name = "lbCondition";
		((Control)lbCondition).Size = new Size(208, 29);
		((Control)lbCondition).TabIndex = 35;
		((Control)lbCondition).Text = "Condition";
		((Control)lbDataType).BackColor = Color.Transparent;
		((Control)lbDataType).Location = new Point(260, 23);
		((Control)lbDataType).Margin = new Padding(6, 0, 6, 0);
		((Control)lbDataType).Name = "lbDataType";
		((Control)lbDataType).Size = new Size(208, 29);
		((Control)lbDataType).TabIndex = 32;
		((Control)lbDataType).Text = "Data Type";
		((Control)lbValue).BackColor = Color.Transparent;
		((Control)lbValue).Location = new Point(718, 23);
		((Control)lbValue).Margin = new Padding(6, 0, 6, 0);
		((Control)lbValue).Name = "lbValue";
		((Control)lbValue).Size = new Size(224, 29);
		((Control)lbValue).TabIndex = 33;
		((Control)lbValue).Text = "Value";
		((Control)numericUpDownMarkerAngel).BackColor = Color.White;
		numericUpDownMarkerAngel.DecimalPlaces = 0;
		((Control)numericUpDownMarkerAngel).Location = new Point(488, 142);
		((Control)numericUpDownMarkerAngel).Margin = new Padding(6);
		((Control)numericUpDownMarkerAngel).Name = "numericUpDownMarkerAngel";
		((Control)numericUpDownMarkerAngel).Size = new Size(204, 42);
		((Control)numericUpDownMarkerAngel).TabIndex = 9;
		((Control)numericUpDownMarkerAngel).Visible = false;
		comboBoxMarkerType.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxMarkerType).FormattingEnabled = true;
		((Control)comboBoxMarkerType).Location = new Point(260, 142);
		((Control)comboBoxMarkerType).Margin = new Padding(6);
		((Control)comboBoxMarkerType).Name = "comboBoxMarkerType";
		((Control)comboBoxMarkerType).Size = new Size(204, 42);
		((Control)comboBoxMarkerType).TabIndex = 8;
		((Control)comboBoxMarkerType).Visible = false;
		((Control)labelMarkerAngel).BackColor = Color.Transparent;
		((Control)labelMarkerAngel).Location = new Point(488, 108);
		((Control)labelMarkerAngel).Margin = new Padding(6, 0, 6, 0);
		((Control)labelMarkerAngel).Name = "labelMarkerAngel";
		((Control)labelMarkerAngel).Size = new Size(224, 29);
		((Control)labelMarkerAngel).TabIndex = 40;
		((Control)labelMarkerAngel).Text = "Marker Angle";
		((Label)labelMarkerAngel).TextAlign = (ContentAlignment)16;
		((Control)labelMarkerAngel).Visible = false;
		((Control)labelMarkerType).BackColor = Color.Transparent;
		((Control)labelMarkerType).Location = new Point(260, 110);
		((Control)labelMarkerType).Margin = new Padding(6, 0, 6, 0);
		((Control)labelMarkerType).Name = "labelMarkerType";
		((Control)labelMarkerType).Size = new Size(208, 29);
		((Control)labelMarkerType).TabIndex = 41;
		((Control)labelMarkerType).Text = "Marker Type";
		((Label)labelMarkerType).TextAlign = (ContentAlignment)16;
		((Control)labelMarkerType).Visible = false;
		((Control)colorBoxColor).BackColor = SystemColors.Window;
		colorBoxColor.Flat = false;
		((Control)colorBoxColor).Location = new Point(28, 142);
		((Control)colorBoxColor).Margin = new Padding(6);
		((Control)colorBoxColor).Name = "colorBoxColor";
		colorBoxColor.SelectedColor = Color.Transparent;
		((Control)colorBoxColor).Size = new Size(208, 40);
		((Control)colorBoxColor).TabIndex = 7;
		((Control)lbColor).BackColor = Color.Transparent;
		((Control)lbColor).Location = new Point(28, 110);
		((Control)lbColor).Margin = new Padding(6, 0, 6, 0);
		((Control)lbColor).Name = "lbColor";
		((Control)lbColor).Size = new Size(320, 29);
		((Control)lbColor).TabIndex = 39;
		((Control)lbColor).Text = "Color";
		((Label)lbColor).TextAlign = (ContentAlignment)16;
		((Control)groupLine).BackColor = Color.FromArgb(250, 250, 250);
		((Control)groupLine).Dock = (DockStyle)2;
		groupLine.LinePosition = StiLinePosition.Bottom;
		groupLine.LineStyle = StiLineStyle.Dot;
		((Control)groupLine).Location = new Point(0, 204);
		((Control)groupLine).Margin = new Padding(6);
		((Control)groupLine).Name = "groupLine";
		((Control)groupLine).Size = new Size(1204, 4);
		((Control)groupLine).TabIndex = 72;
		((Control)panelExpression).BackColor = Color.White;
		((Control)panelExpression).Controls.Add((Control)(object)tbValue);
		((Control)panelExpression).Controls.Add((Control)(object)buttonExpression);
		((Control)panelExpression).Location = new Point(716, 54);
		((Control)panelExpression).Margin = new Padding(6);
		((Control)panelExpression).Name = "panelExpression";
		((Control)panelExpression).Padding = new Padding(2);
		((Control)panelExpression).Size = new Size(240, 40);
		((Control)panelExpression).TabIndex = 74;
		((Control)tbValue).Anchor = (AnchorStyles)15;
		((Control)tbValue).BackColor = SystemColors.Window;
		((TextBoxBase)tbValue).BorderStyle = (BorderStyle)0;
		((Control)tbValue).Location = new Point(8, 8);
		((Control)tbValue).Margin = new Padding(6);
		((Control)tbValue).Name = "tbValue";
		((Control)tbValue).Size = new Size(172, 24);
		((Control)tbValue).TabIndex = 0;
		((Control)buttonExpression).Dock = (DockStyle)4;
		((ButtonBase)buttonExpression).Image = (Image)componentResourceManager.GetObject("buttonExpression.Image");
		((Control)buttonExpression).Location = new Point(194, 2);
		((Control)buttonExpression).Margin = new Padding(6);
		((Control)buttonExpression).Name = "buttonExpression";
		((Control)buttonExpression).Size = new Size(44, 36);
		((Control)buttonExpression).TabIndex = 1;
		((Control)buttonExpression).TabStop = false;
		((ButtonBase)buttonExpression).UseVisualStyleBackColor = true;
		((Control)buttonExpression).Click += ButtonExpression_Click;
		((Control)comboBoxExpression).Anchor = (AnchorStyles)13;
		comboBoxExpression.AutoDropDownWidth = false;
		((Control)comboBoxExpression).BackColor = SystemColors.Window;
		comboBoxExpression.DropDownStyle = (ComboBoxStyle)2;
		((ComboBox)comboBoxExpression).DropDownWidth = 96;
		((Control)comboBoxExpression).Location = new Point(980, 54);
		((Control)comboBoxExpression).Margin = new Padding(6);
		((Control)comboBoxExpression).Name = "comboBoxExpression";
		((Control)comboBoxExpression).Size = new Size(198, 42);
		((Control)comboBoxExpression).TabIndex = 311;
		((ComboBox)comboBoxExpression).SelectedIndexChanged += ComboBoxExpression_SelectedIndexChanged;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(12f, 25f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Control)this).Controls.Add((Control)(object)comboBoxExpression);
		((Control)this).Controls.Add((Control)(object)panelExpression);
		((Control)this).Controls.Add((Control)(object)groupLine);
		((Control)this).Controls.Add((Control)(object)numericUpDownMarkerAngel);
		((Control)this).Controls.Add((Control)(object)comboBoxMarkerType);
		((Control)this).Controls.Add((Control)(object)labelMarkerAngel);
		((Control)this).Controls.Add((Control)(object)labelMarkerType);
		((Control)this).Controls.Add((Control)(object)colorBoxColor);
		((Control)this).Controls.Add((Control)(object)lbColor);
		((Control)this).Controls.Add((Control)(object)lbFieldIs);
		((Control)this).Controls.Add((Control)(object)cbDataType);
		((Control)this).Controls.Add((Control)(object)cbItem);
		((Control)this).Controls.Add((Control)(object)cbCondition);
		((Control)this).Controls.Add((Control)(object)lbCondition);
		((Control)this).Controls.Add((Control)(object)lbDataType);
		((Control)this).Controls.Add((Control)(object)lbValue);
		((Control)this).Controls.Add((Control)(object)dtpValue);
		((Control)this).Margin = new Padding(8);
		((Control)this).Name = "StiChartElementConditionsControl";
		base.ShowClose = true;
		((Control)this).Size = new Size(1204, 208);
		((ISupportInitialize)numericUpDownMarkerAngel).EndInit();
		((Control)panelExpression).ResumeLayout(false);
		((Control)panelExpression).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
