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
public class StiCloudChartFiltersCollectionControl : UserControl
{
	private StiChartFiltersCollection filters;

	private StiReport report;

	private StiSeries series;

	private IContainer components;

	private StiToolBar toolBar;

	private StiGroupLine headerPanel;

	public RadioButton radioButtonOr;

	public RadioButton radioButtonAnd;

	private StiZoneContainerControl panelFilters;

	private Button tbAddLevel;

	public StiCloudChartFiltersCollectionControl()
	{
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			Localize();
			panelFilters.EmptyImage = StiReportDesignImages.Actions.Filter(StiImageSize.Double, allowCache: false);
			panelFilters.EmptyText = StiLocalization.Get("Chart", "NoFilters");
		}
	}

	public void Localize()
	{
		((Control)tbAddLevel).Text = StiLocalization.Get("Chart", "AddFilter");
		((Control)radioButtonAnd).Text = StiLocalization.Get("Report", "FilterAnd");
		((Control)radioButtonOr).Text = StiLocalization.Get("Report", "FilterOr");
	}

	internal void Init(StiChartFiltersCollection filters, StiSeries series)
	{
		this.series = series;
		report = ((StiChart)series.Chart).Report;
		this.filters = filters;
		Build();
	}

	internal void Clean()
	{
		((Control)panelFilters).Controls.Clear();
	}

	private void AddLevel()
	{
		AddLevel(StiLocalization.Get("PropertyEnum", "StiFilterItemArgument"), StiLocalization.Get("PropertyEnum", "StiFilterConditionEqualTo"), StiLocalization.Get("PropertyEnum", "StiFilterDataTypeString"), string.Empty);
	}

	private void AddLevel(string item, string condition, string dataType, string value)
	{
		StiChartFilterZoneControl stiChartFilterZoneControl = new StiChartFilterZoneControl(report)
		{
			ShowClose = true
		};
		if (series is IStiFinancialSeries)
		{
			stiChartFilterZoneControl.cbItem.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterItemValueOpen"));
			stiChartFilterZoneControl.cbItem.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterItemValueClose"));
			stiChartFilterZoneControl.cbItem.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterItemValueLow"));
			stiChartFilterZoneControl.cbItem.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterItemValueHigh"));
		}
		else
		{
			stiChartFilterZoneControl.cbItem.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterItemValue"));
			if (series is IStiRangeSeries)
			{
				stiChartFilterZoneControl.cbItem.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterItemValueEnd"));
			}
		}
		stiChartFilterZoneControl.cbItem.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterItemExpression"));
		stiChartFilterZoneControl.cbItem.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterItemArgument"));
		((Control)stiChartFilterZoneControl.cbItem).Text = item;
		stiChartFilterZoneControl.cbCondition.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionEqualTo"));
		stiChartFilterZoneControl.cbCondition.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionNotEqualTo"));
		stiChartFilterZoneControl.cbCondition.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionGreaterThan"));
		stiChartFilterZoneControl.cbCondition.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionGreaterThanOrEqualTo"));
		stiChartFilterZoneControl.cbCondition.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionLessThan"));
		stiChartFilterZoneControl.cbCondition.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionLessThanOrEqualTo"));
		stiChartFilterZoneControl.cbCondition.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionContaining"));
		stiChartFilterZoneControl.cbCondition.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionNotContaining"));
		stiChartFilterZoneControl.cbCondition.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionBeginningWith"));
		stiChartFilterZoneControl.cbCondition.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionEndingWith"));
		((Control)stiChartFilterZoneControl.cbCondition).Text = condition;
		stiChartFilterZoneControl.cbDataType.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterDataTypeString"));
		stiChartFilterZoneControl.cbDataType.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterDataTypeNumeric"));
		stiChartFilterZoneControl.cbDataType.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterDataTypeDateTime"));
		stiChartFilterZoneControl.cbDataType.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterDataTypeBoolean"));
		((Control)stiChartFilterZoneControl.cbDataType).Text = dataType;
		if (dataType == StiLocalization.Get("PropertyEnum", "StiFilterDataTypeDateTime"))
		{
			DateTime.TryParse(value, new CultureInfo("en-US", useUserOverride: false), DateTimeStyles.None, out var result);
			stiChartFilterZoneControl.dtpValue.Value = result;
		}
		else
		{
			((Control)stiChartFilterZoneControl.tbValue).Text = value;
		}
		((Control)stiChartFilterZoneControl).Width = 1;
		((Control)panelFilters).Controls.Add((Control)(object)stiChartFilterZoneControl);
	}

	private void Build()
	{
		radioButtonOr.Checked = series.FilterMode == StiFilterMode.Or;
		radioButtonAnd.Checked = series.FilterMode == StiFilterMode.And;
		foreach (StiChartFilter filter in filters)
		{
			string item = null;
			string text = null;
			string text2 = null;
			string value = filter.Value;
			switch (filter.Item)
			{
			case StiFilterItem.Value:
				item = StiLocalization.Get("PropertyEnum", "StiFilterItemValue");
				break;
			case StiFilterItem.ValueEnd:
				item = StiLocalization.Get("PropertyEnum", "StiFilterItemValueEnd");
				break;
			case StiFilterItem.Argument:
				item = StiLocalization.Get("PropertyEnum", "StiFilterItemArgument");
				break;
			case StiFilterItem.Expression:
				item = StiLocalization.Get("PropertyEnum", "StiFilterItemExpression");
				break;
			case StiFilterItem.ValueOpen:
				item = StiLocalization.Get("PropertyEnum", "StiFilterItemValueOpen");
				break;
			case StiFilterItem.ValueClose:
				item = StiLocalization.Get("PropertyEnum", "StiFilterItemValueClose");
				break;
			case StiFilterItem.ValueLow:
				item = StiLocalization.Get("PropertyEnum", "StiFilterItemValueLow");
				break;
			case StiFilterItem.ValueHigh:
				item = StiLocalization.Get("PropertyEnum", "StiFilterItemValueHigh");
				break;
			}
			text2 = StiFilterHelper.ConvertConditionToString(filter.Condition);
			text = StiFilterHelper.ConvertDataTypeToString(filter.DataType);
			AddLevel(item, text2, text, value);
		}
	}

	internal void AddFilters()
	{
		filters.Clear();
		series.FilterMode = (radioButtonOr.Checked ? StiFilterMode.Or : StiFilterMode.And);
		foreach (StiChartFilterZoneControl item2 in (ArrangedElementCollection)((Control)panelFilters).Controls)
		{
			StiFilterItem item = StiFilterItem.Argument;
			StiFilterDataType stiFilterDataType = StiFilterDataType.String;
			StiFilterCondition stiFilterCondition = StiFilterCondition.EqualTo;
			if (((Control)item2.cbItem).Text == StiLocalization.Get("PropertyEnum", "StiFilterItemValue"))
			{
				item = StiFilterItem.Value;
			}
			else if (((Control)item2.cbItem).Text == StiLocalization.Get("PropertyEnum", "StiFilterItemValueEnd"))
			{
				item = StiFilterItem.ValueEnd;
			}
			else if (((Control)item2.cbItem).Text == StiLocalization.Get("PropertyEnum", "StiFilterItemArgument"))
			{
				item = StiFilterItem.Argument;
			}
			else if (((Control)item2.cbItem).Text == StiLocalization.Get("PropertyEnum", "StiFilterItemExpression"))
			{
				item = StiFilterItem.Expression;
			}
			else if (((Control)item2.cbItem).Text == StiLocalization.Get("PropertyEnum", "StiFilterItemValueOpen"))
			{
				item = StiFilterItem.ValueOpen;
			}
			else if (((Control)item2.cbItem).Text == StiLocalization.Get("PropertyEnum", "StiFilterItemValueClose"))
			{
				item = StiFilterItem.ValueClose;
			}
			else if (((Control)item2.cbItem).Text == StiLocalization.Get("PropertyEnum", "StiFilterItemValueLow"))
			{
				item = StiFilterItem.ValueLow;
			}
			else if (((Control)item2.cbItem).Text == StiLocalization.Get("PropertyEnum", "StiFilterItemValueHigh"))
			{
				item = StiFilterItem.ValueHigh;
			}
			stiFilterCondition = StiFilterHelper.ConvertStringToCondition(((Control)item2.cbCondition).Text);
			stiFilterDataType = StiFilterHelper.ConvertStringToDataType(((Control)item2.cbDataType).Text);
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
			filters.Insert(0, new StiChartFilter(item, stiFilterDataType, stiFilterCondition, value));
		}
	}

	private void buttonAddClick(object sender, EventArgs e)
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
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_0376: Unknown result type (might be due to invalid IL or missing references)
		toolBar = new StiToolBar();
		tbAddLevel = new Button();
		headerPanel = new StiGroupLine();
		radioButtonOr = new RadioButton();
		radioButtonAnd = new RadioButton();
		panelFilters = new StiZoneContainerControl();
		((Control)toolBar).SuspendLayout();
		((Control)headerPanel).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)toolBar).Controls.Add((Control)(object)tbAddLevel);
		toolBar.ControlStyle = StiControlStyle.Office2013Blue;
		((Control)toolBar).Dock = (DockStyle)1;
		toolBar.LineStyle = StiToolBar.StiToolBarLineStyle.Bottom;
		((Control)toolBar).Location = new Point(0, 0);
		((Control)toolBar).Name = "toolBar";
		((Control)toolBar).Padding = new Padding(6, 3, 3, 4);
		((Control)toolBar).Size = new Size(500, 47);
		((Control)toolBar).TabIndex = 57;
		((Control)toolBar).Text = "stiToolBar1";
		((Control)tbAddLevel).AutoSize = true;
		((Control)tbAddLevel).Location = new Point(11, 11);
		((Control)tbAddLevel).Name = "tbAddLevel";
		((Control)tbAddLevel).Size = new Size(113, 24);
		((Control)tbAddLevel).TabIndex = 79;
		((Control)tbAddLevel).Text = "Add Filter";
		((ButtonBase)tbAddLevel).UseVisualStyleBackColor = true;
		((Control)tbAddLevel).Click += buttonAddClick;
		((Control)headerPanel).BackColor = Color.White;
		((Control)headerPanel).Controls.Add((Control)(object)radioButtonOr);
		((Control)headerPanel).Controls.Add((Control)(object)radioButtonAnd);
		((Control)headerPanel).Dock = (DockStyle)1;
		headerPanel.LinePosition = StiLinePosition.Bottom;
		headerPanel.LineStyle = StiLineStyle.Dot;
		((Control)headerPanel).Location = new Point(0, 47);
		((Control)headerPanel).Name = "headerPanel";
		((Control)headerPanel).Size = new Size(500, 38);
		((Control)headerPanel).TabIndex = 66;
		((Control)radioButtonOr).Location = new Point(78, 7);
		((Control)radioButtonOr).Name = "radioButtonOr";
		((Control)radioButtonOr).Size = new Size(64, 24);
		((Control)radioButtonOr).TabIndex = 1;
		((Control)radioButtonOr).Text = "Or";
		radioButtonAnd.Checked = true;
		((Control)radioButtonAnd).Location = new Point(14, 7);
		((Control)radioButtonAnd).Name = "radioButtonAnd";
		((Control)radioButtonAnd).Size = new Size(64, 24);
		((Control)radioButtonAnd).TabIndex = 0;
		radioButtonAnd.TabStop = true;
		((Control)radioButtonAnd).Text = "And";
		((Control)panelFilters).AllowDrop = true;
		((Control)panelFilters).Anchor = (AnchorStyles)15;
		((ScrollableControl)panelFilters).AutoScroll = true;
		((Control)panelFilters).BackColor = Color.White;
		panelFilters.EmptyImage = null;
		panelFilters.EmptyText = null;
		((Control)panelFilters).Location = new Point(3, 90);
		((Control)panelFilters).Margin = new Padding(2, 2, 2, 2);
		((Control)panelFilters).Name = "panelFilters";
		((Control)panelFilters).Padding = new Padding(2);
		panelFilters.ShowBorder = false;
		((Control)panelFilters).Size = new Size(494, 307);
		((Control)panelFilters).TabIndex = 67;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Control)this).Controls.Add((Control)(object)panelFilters);
		((Control)this).Controls.Add((Control)(object)headerPanel);
		((Control)this).Controls.Add((Control)(object)toolBar);
		((Control)this).Name = "StiCloudChartFiltersCollectionControl";
		((Control)this).Size = new Size(500, 400);
		((Control)toolBar).ResumeLayout(false);
		((Control)toolBar).PerformLayout();
		((Control)headerPanel).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
