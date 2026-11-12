using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Design.Images;

namespace Stimulsoft.Report.Chart.Design;

[ToolboxItem(false)]
public class StiCloudChartConditionsCollectionControl : UserControl
{
	private StiChartConditionsCollection conditions;

	private StiSeries series;

	private IContainer components;

	private StiZoneContainerControl panelConditions;

	private Button buttonAdd;

	private StiToolBar toolBar;

	public StiCloudChartConditionsCollectionControl()
	{
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			Localize();
			panelConditions.EmptyImage = StiReportDesignImages.Actions.Conditions(StiImageSize.Double, allowCache: false);
			panelConditions.EmptyText = StiLocalization.Get("Chart", "NoConditions");
		}
	}

	public void Localize()
	{
		((Control)buttonAdd).Text = StiLocalization.Get("Chart", "AddCondition");
		foreach (StiChartConditionZoneControl item in (ArrangedElementCollection)((Control)panelConditions).Controls)
		{
			item.Localize();
		}
	}

	internal void Init(StiChartConditionsCollection conditions, StiSeries series)
	{
		this.conditions = conditions;
		this.series = series;
		Build();
	}

	internal void Clean()
	{
		((Control)panelConditions).Controls.Clear();
	}

	private void AddLevel()
	{
		AddLevel(Color.White, StiLocalization.Get("PropertyEnum", "StiFilterItemArgument"), StiLocalization.Get("PropertyEnum", "StiFilterConditionEqualTo"), StiLocalization.Get("PropertyEnum", "StiFilterDataTypeString"), string.Empty);
	}

	private void AddLevel(Color color, string item, string condition, string dataType, string value, StiMarkerType markerType = StiMarkerType.Circle, float angle = 0f)
	{
		StiChartConditionZoneControl stiChartConditionZoneControl = new StiChartConditionZoneControl();
		stiChartConditionZoneControl.colorBoxColor.SelectedColor = color;
		if (series is IStiLineSeries)
		{
			Label labelMarkerAngel = stiChartConditionZoneControl.labelMarkerAngel;
			Label labelMarkerType = stiChartConditionZoneControl.labelMarkerType;
			ComboBox comboBoxMarkerType = stiChartConditionZoneControl.comboBoxMarkerType;
			bool flag2 = (((Control)stiChartConditionZoneControl.numericUpDownMarkerAngel).Visible = true);
			bool flag4 = (((Control)comboBoxMarkerType).Visible = flag2);
			bool visible = (((Control)labelMarkerType).Visible = flag4);
			((Control)labelMarkerAngel).Visible = visible;
			foreach (StiMarkerType value2 in Enum.GetValues(typeof(StiMarkerType)))
			{
				stiChartConditionZoneControl.comboBoxMarkerType.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiMarkerType" + value2));
				if (value2 == markerType)
				{
					((ListControl)stiChartConditionZoneControl.comboBoxMarkerType).SelectedIndex = stiChartConditionZoneControl.comboBoxMarkerType.Items.Count - 1;
				}
			}
			stiChartConditionZoneControl.numericUpDownMarkerAngel.Value = (decimal)angle;
		}
		if (series is IStiFinancialSeries)
		{
			stiChartConditionZoneControl.cbItem.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterItemValueOpen"));
			stiChartConditionZoneControl.cbItem.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterItemValueClose"));
			stiChartConditionZoneControl.cbItem.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterItemValueLow"));
			stiChartConditionZoneControl.cbItem.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterItemValueHigh"));
		}
		else
		{
			stiChartConditionZoneControl.cbItem.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterItemValue"));
			if (series is IStiRangeSeries)
			{
				stiChartConditionZoneControl.cbItem.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterItemValueEnd"));
			}
		}
		stiChartConditionZoneControl.cbItem.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterItemArgument"));
		((Control)stiChartConditionZoneControl.cbItem).Text = item;
		stiChartConditionZoneControl.cbCondition.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionEqualTo"));
		stiChartConditionZoneControl.cbCondition.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionNotEqualTo"));
		stiChartConditionZoneControl.cbCondition.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionGreaterThan"));
		stiChartConditionZoneControl.cbCondition.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionGreaterThanOrEqualTo"));
		stiChartConditionZoneControl.cbCondition.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionLessThan"));
		stiChartConditionZoneControl.cbCondition.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionLessThanOrEqualTo"));
		stiChartConditionZoneControl.cbCondition.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionContaining"));
		stiChartConditionZoneControl.cbCondition.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionNotContaining"));
		stiChartConditionZoneControl.cbCondition.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionBeginningWith"));
		stiChartConditionZoneControl.cbCondition.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionEndingWith"));
		((Control)stiChartConditionZoneControl.cbCondition).Text = condition;
		stiChartConditionZoneControl.cbDataType.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterDataTypeString"));
		stiChartConditionZoneControl.cbDataType.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterDataTypeNumeric"));
		stiChartConditionZoneControl.cbDataType.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterDataTypeDateTime"));
		stiChartConditionZoneControl.cbDataType.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterDataTypeBoolean"));
		((Control)stiChartConditionZoneControl.cbDataType).Text = dataType;
		if (dataType == StiLocalization.Get("PropertyEnum", "StiFilterDataTypeDateTime"))
		{
			DateTime.TryParse(value, new CultureInfo("en-US", useUserOverride: false), DateTimeStyles.None, out var result);
			stiChartConditionZoneControl.dtpValue.Value = result;
		}
		else
		{
			((Control)stiChartConditionZoneControl.tbValue).Text = value;
		}
		((Control)stiChartConditionZoneControl).Width = 1;
		((Control)panelConditions).Controls.Add((Control)(object)stiChartConditionZoneControl);
	}

	private void Build()
	{
		((Control)panelConditions).Controls.Clear();
		foreach (StiChartCondition condition2 in conditions)
		{
			string item = StiFilterHelper.ConvertItemToString(condition2.Item);
			string condition = StiFilterHelper.ConvertConditionToString(condition2.Condition);
			string dataType = StiFilterHelper.ConvertDataTypeToString(condition2.DataType);
			AddLevel(condition2.Color, item, condition, dataType, condition2.Value, condition2.MarkerType, condition2.MarkerAngle);
		}
	}

	internal StiChartConditionsCollection GetConditions()
	{
		AddConditions();
		return conditions;
	}

	internal void AddConditions()
	{
		conditions.Clear();
		foreach (StiChartConditionZoneControl item2 in (ArrangedElementCollection)((Control)panelConditions).Controls)
		{
			StiFilterItem item = StiFilterHelper.ConvertStringToItem(((Control)item2.cbItem).Text);
			StiFilterCondition condition = StiFilterHelper.ConvertStringToCondition(((Control)item2.cbCondition).Text);
			StiFilterDataType stiFilterDataType = StiFilterHelper.ConvertStringToDataType(((Control)item2.cbDataType).Text);
			StiMarkerType markerType = StiFilterHelper.ConvertStringToMarkerType(((Control)item2.comboBoxMarkerType).Text);
			string value = null;
			if (stiFilterDataType == StiFilterDataType.DateTime)
			{
				CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
				try
				{
					Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
					value = item2.dtpValue.Value.ToShortDateString();
				}
				finally
				{
					Thread.CurrentThread.CurrentCulture = currentCulture;
				}
			}
			else
			{
				value = ((Control)item2.tbValue).Text;
			}
			conditions.Insert(0, new StiChartCondition(item2.colorBoxColor.SelectedColor, item, stiFilterDataType, condition, value, markerType, (float)item2.numericUpDownMarkerAngel.Value));
		}
	}

	private void buttonAdd_Click(object sender, EventArgs e)
	{
		AddLevel();
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
		panelConditions = new StiZoneContainerControl();
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
		((Control)panelConditions).AllowDrop = true;
		((Control)panelConditions).Anchor = (AnchorStyles)15;
		((ScrollableControl)panelConditions).AutoScroll = true;
		((Control)panelConditions).BackColor = Color.White;
		panelConditions.EmptyImage = null;
		panelConditions.EmptyText = null;
		((Control)panelConditions).Location = new Point(4, 52);
		((Control)panelConditions).Margin = new Padding(2, 2, 2, 2);
		((Control)panelConditions).Name = "panelConditions";
		((Control)panelConditions).Padding = new Padding(2);
		panelConditions.ShowBorder = false;
		((Control)panelConditions).Size = new Size(494, 346);
		((Control)panelConditions).TabIndex = 62;
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
		((Control)this).Controls.Add((Control)(object)panelConditions);
		((Control)this).Name = "StiCloudChartConditionsCollectionControl";
		((Control)this).Size = new Size(500, 400);
		((Control)toolBar).ResumeLayout(false);
		((Control)toolBar).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
