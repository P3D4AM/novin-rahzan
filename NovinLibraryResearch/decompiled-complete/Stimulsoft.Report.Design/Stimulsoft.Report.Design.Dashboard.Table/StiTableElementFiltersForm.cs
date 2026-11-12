using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Design.Dashboard.Table;

public class StiTableElementFiltersForm : Form
{
	private IStiDesignerBase designer;

	private List<StiDataFilterRule> filterRules;

	private string columnKey;

	private string columnPath;

	private Type columnType;

	private List<string> items;

	private IContainer components;

	private StiTableZoneContainer zoneFilters;

	public StiToolBar toolbar;

	private StiGroupLine stiGroupLine1;

	private Button buttonOk;

	private Button buttonCancel;

	public CheckBox checkBoxFilterOn;

	private Button tbAddLevel;

	public StiTableElementFiltersForm(IStiDesignerBase designer, List<StiDataFilterRule> filterRules, string columnKey, string columnPath, Type columnType, List<string> items)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		Localize();
		this.designer = designer;
		this.filterRules = filterRules;
		this.columnKey = columnKey;
		this.columnPath = columnPath;
		this.columnType = columnType;
		this.items = items;
		ReadColumnFilters();
		MeasureButtonSize();
	}

	public StiTableElementFiltersForm(IStiDesignerBase designer, List<StiDataFilterRule> filterRules, string columnKey, string columnPath, Type columnType, List<string> items, StiDataFilterCondition condition)
	{
		InitializeComponent();
		Localize();
		this.designer = designer;
		this.filterRules = filterRules;
		this.columnKey = columnKey;
		this.columnPath = columnPath;
		this.columnType = columnType;
		this.items = items;
		AddFilter(condition);
	}

	private void MeasureButtonSize()
	{
		Size size = TextRenderer.MeasureText(((Control)tbAddLevel).Text, ((Control)tbAddLevel).Font);
		((Control)tbAddLevel).Width = size.Width + StiScale.I20;
		((Control)checkBoxFilterOn).Location = new Point(((Control)tbAddLevel).Right + StiScale.I11, ((Control)checkBoxFilterOn).Location.Y);
	}

	private void CheckState()
	{
		((Control)checkBoxFilterOn).Enabled = ((ArrangedElementCollection)((Control)zoneFilters).Controls).Count > 0;
	}

	private void ReadColumnFilters()
	{
		filterRules.Where((StiDataFilterRule r) => r.Key == columnKey || (r.Key == null && r.Path == columnPath)).ToList().ForEach(delegate(StiDataFilterRule r)
		{
			checkBoxFilterOn.Checked = r.IsEnabled;
			AddFilter(r.Condition, r.Value, r.Value2, r.IsExpression);
		});
	}

	public void WriteColumnFilters()
	{
		filterRules.RemoveAll((StiDataFilterRule r) => r.Key == columnKey || (r.Key == null && r.Path == columnPath));
		foreach (StiTableElementFilterZoneControl item2 in (ArrangedElementCollection)((Control)zoneFilters).Controls)
		{
			StiDataFilterCondition stiDataFilterCondition = (StiDataFilterCondition)StiFilterHelper.ConvertStringToCondition(((Control)item2.comboBoxCondition).Text);
			string value = string.Empty;
			string value2 = string.Empty;
			if (stiDataFilterCondition != StiDataFilterCondition.IsNull && stiDataFilterCondition != StiDataFilterCondition.IsNotNull && stiDataFilterCondition != StiDataFilterCondition.IsBlank && stiDataFilterCondition != StiDataFilterCondition.IsNotBlank)
			{
				if (columnType.IsDateType() && !item2.checkBoxExpression.Checked)
				{
					value = item2.datePickerValue.Value.ToString("MM'/'dd'/'yyyy");
					if (stiDataFilterCondition == StiDataFilterCondition.Between || stiDataFilterCondition == StiDataFilterCondition.NotBetween)
					{
						value2 = item2.datePickerValue2.Value.ToString("MM'/'dd'/'yyyy");
					}
				}
				else
				{
					value = (((Control)item2.comboBoxBoolean).Visible ? ((Control)item2.comboBoxBoolean).Text : (item2.checkBoxExpression.Checked ? ((Control)item2.textBoxValue).Text : ((Control)item2.comboBoxValue).Text));
					if (stiDataFilterCondition == StiDataFilterCondition.Between || stiDataFilterCondition == StiDataFilterCondition.NotBetween)
					{
						value2 = (item2.checkBoxExpression.Checked ? ((Control)item2.textBoxValue2).Text : ((Control)item2.comboBoxValue2).Text);
					}
				}
			}
			StiDataFilterRule item = new StiDataFilterRule
			{
				Key = columnKey,
				Path = columnPath,
				Condition = stiDataFilterCondition,
				Value = value,
				Value2 = value2,
				IsEnabled = checkBoxFilterOn.Checked,
				IsExpression = item2.checkBoxExpression.Checked
			};
			filterRules.Insert(0, item);
		}
	}

	private void AddFilter()
	{
		StiTableElementFilterZoneControl stiTableElementFilterZoneControl = ((((ArrangedElementCollection)((Control)zoneFilters).Controls).Count > 0) ? (((Control)zoneFilters).Controls[((ArrangedElementCollection)((Control)zoneFilters).Controls).Count - 1] as StiTableElementFilterZoneControl) : null);
		StiDataFilterCondition condition = (StiDataFilterCondition)((stiTableElementFilterZoneControl != null) ? StiFilterHelper.ConvertStringToCondition(((Control)stiTableElementFilterZoneControl.comboBoxCondition).Text) : StiFilterCondition.EqualTo);
		AddFilter(condition);
	}

	private void AddFilter(StiDataFilterCondition condition)
	{
		AddFilter(condition, string.Empty, string.Empty, isExpression: false);
	}

	private void AddFilter(StiDataFilterCondition condition, string value, string value2, bool isExpression)
	{
		if (condition == StiDataFilterCondition.IsFalse)
		{
			return;
		}
		StiTableElementFilterZoneControl stiTableElementFilterZoneControl = new StiTableElementFilterZoneControl(designer, columnType);
		((Control)stiTableElementFilterZoneControl.comboBoxCondition).Text = Loc.Get("PropertyEnum", $"StiFilterCondition{condition}");
		stiTableElementFilterZoneControl.checkBoxExpression.Checked = isExpression;
		StiTableElementFilterZoneControl control = stiTableElementFilterZoneControl;
		items.ForEach(delegate(string i)
		{
			control.comboBoxValue.Items.Add((object)i);
			control.comboBoxValue2.Items.Add((object)i);
		});
		if (columnType.IsDateType() && !isExpression)
		{
			if (!DateTime.TryParseExact(value, "MM'/'dd'/'yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
			{
				result = DateTime.Now;
			}
			control.datePickerValue.Value = result;
			if (!DateTime.TryParseExact(value2, "MM'/'dd'/'yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var result2))
			{
				result2 = DateTime.Now;
			}
			control.datePickerValue2.Value = result2;
		}
		else if (isExpression)
		{
			((Control)control.textBoxValue).Text = value;
			((Control)control.textBoxValue2).Text = value2;
		}
		else
		{
			((Control)control.comboBoxValue).Text = value;
			((Control)control.comboBoxValue2).Text = value2;
			((Control)control.comboBoxBoolean).Text = ((!string.IsNullOrEmpty(value) && value.ToLower() == "true") ? "True" : "False");
		}
		((Control)zoneFilters).Controls.Add((Control)(object)control);
		((Control)control.textBoxValue).Focus();
		CheckState();
	}

	private void Localize()
	{
		((Control)this).Text = Loc.Get("Wizards", "Filters");
		((Control)tbAddLevel).Text = Loc.Get("FormBand", "AddFilter");
		((Control)checkBoxFilterOn).Text = Loc.Get("PropertyMain", "FilterOn");
		((Control)buttonOk).Text = Loc.Get("Buttons", "Ok");
		((Control)buttonCancel).Text = Loc.Get("Buttons", "Cancel");
	}

	protected override void OnShown(EventArgs e)
	{
		((Form)this).OnShown(e);
		if (((ArrangedElementCollection)((Control)zoneFilters).Controls).Count > 0)
		{
			StiTableElementFilterZoneControl stiTableElementFilterZoneControl = ((Control)zoneFilters).Controls[((ArrangedElementCollection)((Control)zoneFilters).Controls).Count - 1] as StiTableElementFilterZoneControl;
			stiTableElementFilterZoneControl.FocusZoneControl();
		}
	}

	private void ButtonAddFilter_Click(object sender, EventArgs e)
	{
		AddFilter();
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		WriteColumnFilters();
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
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0392: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d1: Unknown result type (might be due to invalid IL or missing references)
		toolbar = new StiToolBar();
		checkBoxFilterOn = new CheckBox();
		stiGroupLine1 = new StiGroupLine();
		buttonOk = new Button();
		buttonCancel = new Button();
		tbAddLevel = new Button();
		zoneFilters = new StiTableZoneContainer();
		((Control)toolbar).SuspendLayout();
		((Control)stiGroupLine1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)toolbar).Controls.Add((Control)(object)tbAddLevel);
		((Control)toolbar).Controls.Add((Control)(object)checkBoxFilterOn);
		toolbar.ControlStyle = StiControlStyle.Office2013Blue;
		((Control)toolbar).Dock = (DockStyle)1;
		toolbar.LineStyle = StiToolBar.StiToolBarLineStyle.Bottom;
		((Control)toolbar).Location = new Point(0, 0);
		((Control)toolbar).Margin = new Padding(0);
		((Control)toolbar).Name = "toolbar";
		((Control)toolbar).Padding = new Padding(6, 3, 3, 4);
		((Control)toolbar).Size = new Size(668, 47);
		((Control)toolbar).TabIndex = 40;
		((Control)toolbar).Text = "stiToolBar1";
		((Control)checkBoxFilterOn).BackColor = Color.Transparent;
		checkBoxFilterOn.Checked = true;
		checkBoxFilterOn.CheckState = (CheckState)1;
		((Control)checkBoxFilterOn).Enabled = false;
		((ButtonBase)checkBoxFilterOn).FlatStyle = (FlatStyle)3;
		((Control)checkBoxFilterOn).ForeColor = SystemColors.ControlText;
		((Control)checkBoxFilterOn).Location = new Point(139, 12);
		((Control)checkBoxFilterOn).Margin = new Padding(2);
		((Control)checkBoxFilterOn).Name = "checkBoxFilterOn";
		((Control)checkBoxFilterOn).Size = new Size(154, 22);
		((Control)checkBoxFilterOn).TabIndex = 12;
		((Control)checkBoxFilterOn).Text = "Filter On";
		((ButtonBase)checkBoxFilterOn).UseVisualStyleBackColor = false;
		((Control)stiGroupLine1).BackColor = Color.White;
		((Control)stiGroupLine1).Controls.Add((Control)(object)buttonOk);
		((Control)stiGroupLine1).Controls.Add((Control)(object)buttonCancel);
		((Control)stiGroupLine1).Dock = (DockStyle)2;
		stiGroupLine1.LinePosition = StiLinePosition.Top;
		stiGroupLine1.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine1).Location = new Point(0, 361);
		((Control)stiGroupLine1).Margin = new Padding(2);
		((Control)stiGroupLine1).Name = "stiGroupLine1";
		((Control)stiGroupLine1).Size = new Size(668, 46);
		((Control)stiGroupLine1).TabIndex = 42;
		((Control)buttonOk).Anchor = (AnchorStyles)10;
		buttonOk.DialogResult = (DialogResult)1;
		((Control)buttonOk).Location = new Point(504, 12);
		((Control)buttonOk).Margin = new Padding(2);
		((Control)buttonOk).Name = "buttonOk";
		((Control)buttonOk).Size = new Size(75, 24);
		((Control)buttonOk).TabIndex = 0;
		((Control)buttonOk).Text = "Ok";
		((ButtonBase)buttonOk).UseVisualStyleBackColor = true;
		((Control)buttonOk).Click += This_OkClick;
		((Control)buttonCancel).Anchor = (AnchorStyles)10;
		buttonCancel.DialogResult = (DialogResult)2;
		((Control)buttonCancel).Location = new Point(584, 12);
		((Control)buttonCancel).Margin = new Padding(2);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(75, 24);
		((Control)buttonCancel).TabIndex = 1;
		((Control)buttonCancel).Text = "Cancel";
		((ButtonBase)buttonCancel).UseVisualStyleBackColor = true;
		((Control)tbAddLevel).Location = new Point(11, 11);
		((Control)tbAddLevel).Name = "tbAddLevel";
		((Control)tbAddLevel).Size = new Size(113, 24);
		((Control)tbAddLevel).TabIndex = 80;
		((Control)tbAddLevel).Text = "Add Filter";
		((ButtonBase)tbAddLevel).UseVisualStyleBackColor = true;
		((Control)tbAddLevel).Click += ButtonAddFilter_Click;
		((Control)zoneFilters).AllowDrop = true;
		((Control)zoneFilters).Anchor = (AnchorStyles)15;
		((ScrollableControl)zoneFilters).AutoScroll = true;
		((Control)zoneFilters).BackColor = Color.White;
		((Control)zoneFilters).Location = new Point(12, 59);
		((Control)zoneFilters).Margin = new Padding(1);
		((Control)zoneFilters).Name = "zoneFilters";
		((Control)zoneFilters).Padding = new Padding(2);
		((Control)zoneFilters).Size = new Size(642, 288);
		((Control)zoneFilters).TabIndex = 5;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(668, 407);
		((Control)this).Controls.Add((Control)(object)stiGroupLine1);
		((Control)this).Controls.Add((Control)(object)toolbar);
		((Control)this).Controls.Add((Control)(object)zoneFilters);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiTableElementFiltersForm";
		((Form)this).ShowIcon = false;
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "StiTableFiltersForm";
		((Control)toolbar).ResumeLayout(false);
		((Control)stiGroupLine1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
