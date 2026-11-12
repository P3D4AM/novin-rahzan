using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Components.Table;
using Stimulsoft.Dashboard.Design.Controls;
using Stimulsoft.Dashboard.Design.Editors.Menu;
using Stimulsoft.Dashboard.Helpers;
using Stimulsoft.Data.Engine;
using Stimulsoft.Data.Helpers;
using Stimulsoft.Report;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Editors;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Help;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Dashboard.Design.Editors;

public class StiDataFiltersEditorForm : StiElementEditorForm
{
	private StiDataFilterListBox selectedListBox;

	private Dictionary<string, object[]> dataPathToItems = new Dictionary<string, object[]>();

	private int filterOperationSelectedIndex = -1;

	private IContainer components;

	private ToolTip toolTip;

	private Panel panelUi;

	private StiDataFilterListBox listBoxFilters;

	public StiDropDownBox comboBoxValue2;

	public StiDropDownBox comboBoxValue;

	public CheckBox checkBoxExpression;

	public StiComboBox comboBoxBoolean;

	public DateTimePicker datePickerValue;

	public DateTimePicker datePickerValue2;

	public StiButtonEdit textBoxValue2;

	public StiButtonEdit textBoxValue;

	public StiComboBox comboBoxCondition;

	private Panel panelExpression;

	private Panel panelTo;

	private Label labelCondition;

	private Label labelValue;

	private Label labelValue2;

	private Panel panelFrom;

	private Panel panelFilterOn;

	private ContextMenuBar contextMenuBar;

	private ButtonItem menuExpression;

	private StiPanelBox panelBoxField;

	private HintTextBox textBoxField;

	private StiOperationButton buttonFieldMenu;

	private Label labelField;

	private Label labelFilterOperation;

	public StiComboBox comboBoxFilterOperation;

	private Panel panelFilterOperation;

	private Label labelFilterOn;

	public StiDropDownBox comboBoxFilterOn;

	private Panel panelOperationDescription;

	private Label labelOperationDescription;

	private string LocFilterOnTrue => Loc.GetEnum("boolTrue");

	private string LocFilterOnFalse => Loc.GetEnum("boolFalse");

	private string LocFilterOperationOr => Loc.GetEnum("StiFilterModeOr");

	private string LocFilterOperationAnd => Loc.GetEnum("StiFilterModeAnd");

	private string LocConditionBetween => Loc.Get("PropertyEnum", "StiFilterConditionBetween");

	private string LocConditionNotBetween => Loc.Get("PropertyEnum", "StiFilterConditionNotBetween");

	private string LocConditionEqualTo => Loc.Get("PropertyEnum", "StiFilterConditionEqualTo");

	private string LocConditionNotEqualTo => Loc.Get("PropertyEnum", "StiFilterConditionNotEqualTo");

	private string LocConditionGreaterThan => Loc.Get("PropertyEnum", "StiFilterConditionGreaterThan");

	private string LocConditionGreaterThanOrEqualTo => Loc.Get("PropertyEnum", "StiFilterConditionGreaterThanOrEqualTo");

	private string LocConditionLessThan => Loc.Get("PropertyEnum", "StiFilterConditionLessThan");

	private string LocConditionLessThanOrEqualTo => Loc.Get("PropertyEnum", "StiFilterConditionLessThanOrEqualTo");

	private string LocConditionContaining => Loc.Get("PropertyEnum", "StiFilterConditionContaining");

	private string LocConditionNotContaining => Loc.Get("PropertyEnum", "StiFilterConditionNotContaining");

	private string LocConditionBeginningWith => Loc.Get("PropertyEnum", "StiFilterConditionBeginningWith");

	private string LocConditionEndingWith => Loc.Get("PropertyEnum", "StiFilterConditionEndingWith");

	private string LocConditionIsBlank => Loc.Get("PropertyEnum", "StiFilterConditionIsBlank");

	private string LocConditionIsNotBlank => Loc.Get("PropertyEnum", "StiFilterConditionIsNotBlank");

	private string LocConditionIsNull => Loc.Get("PropertyEnum", "StiFilterConditionIsNull");

	private string LocConditionIsNotNull => Loc.Get("PropertyEnum", "StiFilterConditionIsNotNull");

	internal StiDataFilterListBox FiltersListBox => listBoxFilters;

	internal IStiDataFilters DataFiltersElement => base.Element as IStiDataFilters;

	internal StiDataFilterRule DataFilter => listBoxFilters.SelectedFilterItem?.Filter;

	internal IStiAppDataColumn DataColumn => StiDataExpressionHelper.GetDataColumnFromExpression(base.Element.Page as StiDashboard, DataFilter?.Path);

	internal Type DataType
	{
		get
		{
			if (DataColumn != null)
			{
				return DataColumn.GetDataType();
			}
			return typeof(object);
		}
	}

	internal bool IsKeysPresent => ((ListBox)listBoxFilters).Items.Count > 0;

	public override bool IsAnyPresent => IsKeysPresent;

	public override bool AllowResize => false;

	public StiDataFiltersEditorForm()
		: this(null)
	{
	}

	public StiDataFiltersEditorForm(StiElement element)
		: base(element)
	{
		InitializeComponent();
		if (StiOptions.Appearance.ShowFormsHelp)
		{
			((Form)this).HelpButton = true;
		}
		selectedListBox = listBoxFilters;
		foreach (StiDataFilterListBox item in (from Control c in (IEnumerable)((Control)this).Controls
			where c is StiDataFilterListBox
			select c).Cast<StiDataFilterListBox>())
		{
			item.Designer = Designer;
			item.Editor = this;
			item.AllowMultipleElements = true;
			item.AllowImage = false;
			contextMenuBar.SetContextMenuEx((Control)(object)item, menuExpression);
		}
		InitializeMenus();
		ReadPropertiesFromElement();
		LocalizeInternal();
		ActivateDictionaryPanel();
		DoScaling();
	}

	public override void Localize()
	{
		LocalizeInternal();
		((Control)listBoxFilters).Invalidate();
	}

	private void LocalizeInternal()
	{
		((Control)this).Text = Loc.Get("PropertyMain", "Filters");
		((Control)labelFilterOn).Text = Loc.Get("PropertyMain", "FilterOn");
		((Control)labelField).Text = Loc.Get("PropertyMain", "Field");
		((Control)labelValue).Text = Loc.Get("PropertyMain", "Value");
		((Control)labelValue2).Text = Loc.Get("PropertyMain", "To");
		((Control)labelCondition).Text = Loc.Get("PropertyMain", "Condition");
		((Control)labelFilterOperation).Text = Loc.GetMain("Operation");
		((Control)labelOperationDescription).Text = Loc.Get("Dashboard", "DataFilterGrouping");
		((Control)checkBoxExpression).Text = Loc.Get("PropertyEnum", "StiFilterDataTypeExpression");
		RefreshFilterOperation();
		RefreshFilterOn();
		RefreshConditions();
	}

	private void RefreshFilterOperation()
	{
		base.LockRefresh = true;
		((ComboBox)comboBoxFilterOperation).BeginUpdate();
		if (((ListControl)comboBoxFilterOperation).SelectedIndex >= 0)
		{
			filterOperationSelectedIndex = ((ListControl)comboBoxFilterOperation).SelectedIndex;
		}
		((ComboBox)comboBoxFilterOperation).Items.Clear();
		((ComboBox)comboBoxFilterOperation).Items.Add((object)LocFilterOperationAnd);
		((ComboBox)comboBoxFilterOperation).Items.Add((object)LocFilterOperationOr);
		((ListControl)comboBoxFilterOperation).SelectedIndex = filterOperationSelectedIndex;
		if (((ListControl)comboBoxFilterOperation).SelectedIndex == -1)
		{
			((ListControl)comboBoxFilterOperation).SelectedIndex = 0;
		}
		((ComboBox)comboBoxFilterOperation).EndUpdate();
		base.LockRefresh = false;
	}

	private void RefreshFilterOn()
	{
		base.LockRefresh = true;
		comboBoxFilterOn.BeginUpdate();
		int selectedIndex = comboBoxFilterOn.SelectedIndex;
		comboBoxFilterOn.Items.Clear();
		comboBoxFilterOn.Items.Add((object)LocFilterOnTrue);
		comboBoxFilterOn.Items.Add((object)LocFilterOnFalse);
		comboBoxFilterOn.SelectedIndex = selectedIndex;
		comboBoxFilterOn.EndUpdate();
		base.LockRefresh = false;
	}

	private void InitializeMenus()
	{
		menuExpression.PopupOpen += delegate
		{
			new StiDataFiltersExpressionMenu(menuExpression, (TextBox)(object)textBoxField, this, selectedListBox).CreateItems();
		};
	}

	private void DoScaling()
	{
		textBoxValue.Image = StiReportDesignImages.Actions.EditSmall();
		textBoxValue2.Image = StiReportDesignImages.Actions.EditSmall();
		StiComboBox stiComboBox = comboBoxFilterOperation;
		StiComboBox stiComboBox2 = comboBoxCondition;
		StiComboBox stiComboBox3 = comboBoxBoolean;
		StiButtonEdit stiButtonEdit = textBoxValue;
		StiButtonEdit stiButtonEdit2 = textBoxValue2;
		DateTimePicker obj = datePickerValue;
		DateTimePicker obj2 = datePickerValue2;
		StiDropDownBox stiDropDownBox = comboBoxValue;
		int num2 = (((Control)comboBoxValue2).Width = ((Control)panelUi).Width - ((Control)comboBoxValue).Left);
		int num4 = (((Control)stiDropDownBox).Width = num2);
		int num6 = (((Control)obj2).Width = num4);
		int num8 = (((Control)obj).Width = num6);
		int num10 = (((Control)stiButtonEdit2).Width = num8);
		int num12 = (((Control)stiButtonEdit).Width = num10);
		int num14 = (((Control)stiComboBox3).Width = num12);
		int width = (((Control)stiComboBox2).Width = num14);
		((Control)stiComboBox).Width = width;
		StiButtonsLocator.ArrangeButtons((Control)(object)panelBoxField);
		((Control)comboBoxCondition).Top = ((Control)panelUi).Height - ((Control)comboBoxCondition).Height;
		((Control)comboBoxFilterOperation).Top = ((Control)panelFilterOperation).Height - ((Control)comboBoxFilterOperation).Height;
	}

	protected override void OnGlobalElementChanged(object sender, EventArgs e)
	{
		if (sender == base.Element)
		{
			ReadPropertiesFromElement();
			listBoxFilters.SelectedFilterItem = listBoxFilters.FirstItem();
		}
	}

	public override void ReadPropertiesFromElement()
	{
		ReadFiltersPropertiesFromElement();
	}

	private void ReadFiltersPropertiesFromElement()
	{
		((ListBox)listBoxFilters).BeginUpdate();
		((ListBox)listBoxFilters).Items.Clear();
		listBoxFilters.AddItems(DataFiltersElement.DataFilters);
		listBoxFilters.SelectFirstItem();
		((ListBox)listBoxFilters).EndUpdate();
	}

	public override void WritePropertiesToElement()
	{
		WriteFiltersPropertiesToElement();
		RefreshElementView();
	}

	private void WriteFiltersPropertiesToElement()
	{
		DataFiltersElement.DataFilters = (from t in listBoxFilters.ToItems()
			select t.Filter).ToList();
	}

	protected override void AttachEvents()
	{
		base.AttachEvents();
		AttachEventsToKeys();
	}

	private void AttachEventsToKeys()
	{
		listBoxFilters.ElementsChanged += delegate
		{
			WritePropertiesToElement();
			RefreshUiPanel();
		};
		listBoxFilters.ElementsDragEnter += delegate
		{
			ResetComponentHighlights();
		};
		listBoxFilters.ElementsDragDrop += delegate
		{
			if (StiDragDropColumnHelper.IsDataColumnDragged(Designer))
			{
				StiDataColumn draggedDataColumn = StiDragDropColumnHelper.GetDraggedDataColumn(Designer);
				if (draggedDataColumn == null)
				{
					return;
				}
				StiDataFilterRule filter = StiMeterHelper.DataFilter.GetFilter(draggedDataColumn);
				StiDataFilterItem stiDataFilterItem = new StiDataFilterItem(filter.ToString(), filter, StiDataFilterItemImage.None);
				StiDataFilterItem itemAtMouse = listBoxFilters.GetItemAtMouse();
				if (itemAtMouse != null)
				{
					int index = ((ListBox)listBoxFilters).Items.IndexOf((object)itemAtMouse);
					listBoxFilters.InsertItem(index, stiDataFilterItem);
				}
				else
				{
					listBoxFilters.AddItem(stiDataFilterItem);
				}
				Designer.PageView.ResetDragging();
				base.Element.Select();
				WritePropertiesToElement();
				listBoxFilters.SelectedFilterItem = stiDataFilterItem;
				((Control)listBoxFilters).Focus();
			}
			else if (StiDragDropColumnHelper.IsTableElementDragged(Designer))
			{
				if (!(StiDragDropColumnHelper.GetDraggedTableElement(Designer) is StiTableElement stiTableElement))
				{
					return;
				}
				int count = ((ListBox)listBoxFilters).Items.Count;
				foreach (StiTableColumn column in stiTableElement.Columns)
				{
					DataFiltersElement.DataFilters.Add(StiMeterHelper.DataFilter.GetFilter(column, base.Element));
				}
				RefreshElementView();
				ReadPropertiesFromElement();
				Designer.PageView.ResetDragging();
				((ListControl)listBoxFilters).SelectedIndex = count;
			}
			ResetComponentHighlights();
			RefreshUiPanel();
		};
		listBoxFilters.ElementsSelectedChanged += delegate
		{
			comboBoxValue.Items.Clear();
			comboBoxValue2.Items.Clear();
			base.Element.Select();
			RefreshUiPanel();
		};
	}

	public override void RefreshUiPanel()
	{
		if (base.LockRefresh)
		{
			return;
		}
		base.LockRefresh = true;
		((Control)this).SuspendLayout();
		try
		{
			base.LockRefresh = true;
			RefreshConditions();
			((ComboBox)comboBoxCondition).SelectedItem = ((DataFilter != null) ? StiDataFilterHelper.ConvertConditionToString(DataFilter.Condition) : "");
			bool flag = ((Control)comboBoxCondition).Text == LocConditionBetween || ((Control)comboBoxCondition).Text == LocConditionNotBetween;
			bool flag2 = DataFilter != null && DataFilter.IsExpression;
			checkBoxExpression.Checked = flag2;
			bool flag3 = DataFilter != null && DataFilter.IsEnabled;
			comboBoxFilterOn.SelectedItem = (flag3 ? LocFilterOnTrue : LocFilterOnFalse);
			((Control)labelValue).Text = (flag ? Loc.Get("PropertyMain", "From") : Loc.Get("PropertyMain", "Value"));
			bool? flag4 = DataFiltersElement?.DataFilters?.Any((StiDataFilterRule f) => f.Operation == StiDataFilterOperation.OR);
			((ComboBox)comboBoxFilterOperation).SelectedItem = (flag4.GetValueOrDefault() ? LocFilterOperationOr : LocFilterOperationAnd);
			bool flag6;
			bool visible;
			if (!IsAnyPresent)
			{
				Panel obj = panelFrom;
				Panel obj2 = panelTo;
				flag6 = (((Control)panelExpression).Visible = false);
				visible = (((Control)obj2).Visible = flag6);
				((Control)obj).Visible = visible;
				Panel obj3 = panelUi;
				Panel obj4 = panelTo;
				flag6 = (((Control)panelExpression).Visible = IsAnyPresent);
				visible = (((Control)obj4).Visible = flag6);
				((Control)obj3).Visible = visible;
			}
			else if (((Control)comboBoxCondition).Text == LocConditionIsNull || ((Control)comboBoxCondition).Text == LocConditionIsNotNull)
			{
				Panel obj5 = panelFrom;
				Panel obj6 = panelTo;
				flag6 = (((Control)panelExpression).Visible = false);
				visible = (((Control)obj6).Visible = flag6);
				((Control)obj5).Visible = visible;
			}
			else if (DataType.IsDateType() && !flag2)
			{
				((Control)panelFrom).Visible = true;
				((Control)panelTo).Visible = flag;
				((Control)panelExpression).Visible = true;
				DateTimePicker obj7 = datePickerValue;
				visible = (((Control)datePickerValue2).Visible = true);
				((Control)obj7).Visible = visible;
				StiButtonEdit stiButtonEdit = textBoxValue;
				visible = (((Control)textBoxValue2).Visible = false);
				((Control)stiButtonEdit).Visible = visible;
				StiDropDownBox stiDropDownBox = comboBoxValue;
				visible = (((Control)comboBoxValue2).Visible = false);
				((Control)stiDropDownBox).Visible = visible;
				((Control)comboBoxBoolean).Visible = false;
				((Control)checkBoxExpression).Visible = true;
				datePickerValue.Value = GetDateValue(DataFilter.Value);
				datePickerValue2.Value = GetDateValue(DataFilter.Value2);
			}
			else if (DataType == typeof(bool) && !flag2)
			{
				((Control)panelFrom).Visible = true;
				((Control)panelTo).Visible = flag;
				((Control)panelExpression).Visible = true;
				StiButtonEdit stiButtonEdit2 = textBoxValue;
				visible = (((Control)textBoxValue2).Visible = false);
				((Control)stiButtonEdit2).Visible = visible;
				StiDropDownBox stiDropDownBox2 = comboBoxValue;
				visible = (((Control)comboBoxValue2).Visible = false);
				((Control)stiDropDownBox2).Visible = visible;
				DateTimePicker obj8 = datePickerValue;
				visible = (((Control)datePickerValue2).Visible = false);
				((Control)obj8).Visible = visible;
				((Control)comboBoxBoolean).Visible = true;
				((Control)checkBoxExpression).Visible = true;
				((ListControl)comboBoxBoolean).SelectedIndex = ((!GetBooleanValue(DataFilter.Value)) ? 1 : 0);
			}
			else
			{
				((Control)panelFrom).Visible = true;
				((Control)panelTo).Visible = flag;
				((Control)panelExpression).Visible = true;
				StiButtonEdit stiButtonEdit3 = textBoxValue;
				visible = (((Control)textBoxValue2).Visible = flag2);
				((Control)stiButtonEdit3).Visible = visible;
				StiDropDownBox stiDropDownBox3 = comboBoxValue;
				visible = (((Control)comboBoxValue2).Visible = !flag2);
				((Control)stiDropDownBox3).Visible = visible;
				DateTimePicker obj9 = datePickerValue;
				visible = (((Control)datePickerValue2).Visible = false);
				((Control)obj9).Visible = visible;
				((Control)comboBoxBoolean).Visible = false;
				((Control)checkBoxExpression).Visible = true;
				if (flag2)
				{
					((Control)textBoxValue).Text = DataFilter.Value;
					((Control)textBoxValue2).Text = DataFilter.Value2;
				}
				else
				{
					((Control)comboBoxValue).Text = DataFilter.Value;
					((Control)comboBoxValue2).Text = DataFilter.Value2;
				}
			}
			SetListHeightFromCount(listBoxFilters, multiple: true);
			StiTableFiltersGroupsType tableFiltersGroupsType = StiDataFilterRuleHelper.GetTableFiltersGroupsType(DataFiltersElement.DataFilters);
			((Control)panelFilterOperation).Visible = tableFiltersGroupsType != StiTableFiltersGroupsType.None;
			((Control)panelOperationDescription).Visible = tableFiltersGroupsType == StiTableFiltersGroupsType.Complex;
			Panel obj10 = panelUi;
			Panel obj11 = panelFilterOn;
			Label obj12 = labelField;
			bool flag20 = (((Control)panelBoxField).Visible = IsAnyPresent);
			flag6 = (((Control)obj12).Visible = flag20);
			visible = (((Control)obj11).Visible = flag6);
			((Control)obj10).Visible = visible;
			((Control)listBoxFilters).Dock = (DockStyle)(IsAnyPresent ? 1 : 5);
			CalculateWindowHeight();
			CalculateWindowPosition();
			base.LockRefresh = false;
		}
		finally
		{
			((Control)this).ResumeLayout();
			base.LockRefresh = false;
		}
	}

	protected virtual void SetListHeightFromCount(StiDataFilterListBox list, bool multiple)
	{
		if (multiple)
		{
			((Control)list).Height = Math.Min(((ListBox)list).Items.Count + 1, 7) * ((ListBox)list).ItemHeight;
		}
		else
		{
			((Control)list).Height = Math.Max(((ListBox)list).Items.Count, 1) * ((ListBox)list).ItemHeight + StiScale.I1;
		}
	}

	protected override void CalculateWindowHeight()
	{
		if (IsAnyPresent)
		{
			base.CalculateWindowHeight();
		}
		else
		{
			((Form)this).ClientSize = new Size(((Form)this).ClientSize.Width, base.PaddingCorrection + ((Control)listBoxFilters).Width);
		}
	}

	internal void RefreshExpression()
	{
		base.LockRefresh = true;
		if (selectedListBox == listBoxFilters && DataFilter != null)
		{
			listBoxFilters.SetSelectedText(DataFilter.GetStringRepresentation());
			((Control)textBoxField).Text = DataFilter.Path;
		}
		else
		{
			((Control)textBoxField).Text = string.Empty;
		}
		base.LockRefresh = false;
	}

	private DateTime GetDateValue(string value)
	{
		if (!DateTime.TryParseExact(value, "MM'/'dd'/'yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
		{
			return DateTime.Now;
		}
		return result;
	}

	private bool GetBooleanValue(string value)
	{
		value = value ?? "";
		return value.ToLowerInvariant() == "true";
	}

	private void RefreshConditions()
	{
		if (!base.LockConditionRefresh)
		{
			base.LockConditionRefresh = true;
			((ComboBox)comboBoxCondition).BeginUpdate();
			object selectedItem = ((ComboBox)comboBoxCondition).SelectedItem;
			((ComboBox)comboBoxCondition).Items.Clear();
			InitConditionValues();
			((ComboBox)comboBoxCondition).SelectedItem = selectedItem;
			if (((ListControl)comboBoxCondition).SelectedIndex == -1)
			{
				((ListControl)comboBoxCondition).SelectedIndex = 0;
			}
			((ComboBox)comboBoxCondition).EndUpdate();
			base.LockConditionRefresh = false;
		}
	}

	internal void ClearLoadedFilterValues()
	{
		comboBoxValue.Items.Clear();
		comboBoxValue2.Items.Clear();
		RefreshUiPanel();
	}

	private void InitConditionValues()
	{
		if (DataType != null)
		{
			((ComboBox)comboBoxCondition).Items.Add((object)LocConditionEqualTo);
			((ComboBox)comboBoxCondition).Items.Add((object)LocConditionNotEqualTo);
			if (DataType.IsStringType())
			{
				((ComboBox)comboBoxCondition).Items.Add((object)LocConditionContaining);
				((ComboBox)comboBoxCondition).Items.Add((object)LocConditionNotContaining);
				((ComboBox)comboBoxCondition).Items.Add((object)LocConditionBeginningWith);
				((ComboBox)comboBoxCondition).Items.Add((object)LocConditionEndingWith);
			}
			if (DataType.IsStringType() || DataType.IsNumericType() || DataType.IsDateType())
			{
				((ComboBox)comboBoxCondition).Items.Add((object)LocConditionBetween);
				((ComboBox)comboBoxCondition).Items.Add((object)LocConditionNotBetween);
				((ComboBox)comboBoxCondition).Items.Add((object)LocConditionGreaterThan);
				((ComboBox)comboBoxCondition).Items.Add((object)LocConditionGreaterThanOrEqualTo);
				((ComboBox)comboBoxCondition).Items.Add((object)LocConditionLessThan);
				((ComboBox)comboBoxCondition).Items.Add((object)LocConditionLessThanOrEqualTo);
			}
			if (DataType.IsStringType())
			{
				((ComboBox)comboBoxCondition).Items.Add((object)LocConditionIsBlank);
				((ComboBox)comboBoxCondition).Items.Add((object)LocConditionIsNotBlank);
			}
		}
		((ComboBox)comboBoxCondition).Items.Add((object)LocConditionIsNull);
		((ComboBox)comboBoxCondition).Items.Add((object)LocConditionIsNotNull);
	}

	private void LoadValues(StiDropDownBox comboBox, string path)
	{
		if (comboBox.Items.Count <= 0)
		{
			object[] dataValuesFromDataPath = GetDataValuesFromDataPath(path);
			if (dataValuesFromDataPath != null)
			{
				comboBox.BeginUpdate();
				comboBox.Items.AddRange(dataValuesFromDataPath);
				comboBox.EndUpdate();
			}
		}
	}

	private object[] GetDataValuesFromDataPath(string path)
	{
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		try
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
			if (string.IsNullOrWhiteSpace(path))
			{
				return null;
			}
			if (dataPathToItems.ContainsKey(path))
			{
				return dataPathToItems[path];
			}
			path = StiExpressionHelper.RemoveFunction(path);
			StiDataSource dataSourceFromDataColumn = StiDataColumn.GetDataSourceFromDataColumn(base.Element.Page.Report.Dictionary, path);
			if (dataSourceFromDataColumn == null)
			{
				return null;
			}
			if (dataSourceFromDataColumn is StiVirtualSource)
			{
				dataSourceFromDataColumn.Dictionary.ConnectVirtualDataSources();
			}
			else if (dataSourceFromDataColumn is StiDataTransformation)
			{
				dataSourceFromDataColumn.Dictionary.ConnectDataTransformations();
			}
			else
			{
				dataSourceFromDataColumn.Dictionary.Connect();
			}
			string[] array = (from v in (from v in StiDataColumn.GetDataListFromDataColumn(base.Element.Page.Report.Dictionary, path, 100)
					where v != null
					select v.ToString()).Distinct()
				orderby v
				select v).Take(100).ToArray();
			StiDataLeader.Connect(dataSourceFromDataColumn, loadData: false);
			Dictionary<string, object[]> dictionary = dataPathToItems;
			string key = path;
			object[] value = array;
			dictionary[key] = value;
			return array;
		}
		finally
		{
			Thread.CurrentThread.CurrentCulture = currentCulture;
		}
	}

	internal StiDataFilterOperation GetSelectedFilterOperation()
	{
		if (!(((ComboBox)comboBoxFilterOperation).SelectedItem?.ToString() == LocFilterOperationAnd))
		{
			return StiDataFilterOperation.OR;
		}
		return StiDataFilterOperation.AND;
	}

	private void ComboBoxFilterMode_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh && !base.LockConditionRefresh && DataFilter != null)
		{
			DataFiltersElement.DataFilters.ForEach(delegate(StiDataFilterRule f)
			{
				f.Operation = GetSelectedFilterOperation();
			});
			RefreshExpression();
			RefreshElementView();
			RefreshUiPanel();
		}
	}

	private void ComboBoxCondition_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh && !base.LockConditionRefresh && DataFilter != null)
		{
			DataFilter.Condition = StiDataFilterHelper.ConvertStringToCondition(((Control)comboBoxCondition).Text);
			RefreshExpression();
			RefreshElementView();
			RefreshUiPanel();
		}
	}

	private void ListBox_Enter(object sender, EventArgs e)
	{
		selectedListBox = sender as StiDataFilterListBox;
		RefreshExpression();
	}

	private void ListBox_AfterSelect(object sender, EventArgs e)
	{
		selectedListBox = sender as StiDataFilterListBox;
		RefreshExpression();
	}

	private void ListBox_MouseDown(object sender, MouseEventArgs e)
	{
		selectedListBox = sender as StiDataFilterListBox;
		RefreshExpression();
	}

	private void ComboBoxFilterOn_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh && DataFilter != null)
		{
			DataFilter.IsEnabled = comboBoxFilterOn.SelectedItem?.ToString() == LocFilterOnTrue;
			RefreshElementView();
			RefreshUiPanel();
		}
	}

	private void CheckBoxExpression_CheckedChanged(object sender, EventArgs e)
	{
		if (base.LockRefresh || DataFilter == null)
		{
			return;
		}
		DataFilter.IsExpression = checkBoxExpression.Checked;
		RefreshElementView();
		RefreshUiPanel();
		if (!checkBoxExpression.Checked)
		{
			if (IsAnyPresent)
			{
				if (DataType.IsDateType())
				{
					((Control)datePickerValue).Focus();
				}
				else if (DataType == typeof(bool))
				{
					((Control)comboBoxBoolean).Focus();
				}
				else
				{
					((Control)comboBoxValue).Focus();
				}
			}
		}
		else if (IsAnyPresent)
		{
			((Control)textBoxValue).Focus();
		}
	}

	private void ComboBoxValue_TextChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh && DataFilter != null)
		{
			DataFilter.Value = ((Control)comboBoxValue).Text;
			DataFilter.Value2 = ((Control)comboBoxValue2).Text;
			RefreshExpression();
			RefreshElementView();
			RefreshUiPanel();
		}
	}

	private void ComboBoxValue_DropDown(object sender, EventArgs e)
	{
		LoadValues(comboBoxValue, DataFilter.Path);
	}

	private void ComboBoxValue2_DropDown(object sender, EventArgs e)
	{
		LoadValues(comboBoxValue2, DataFilter.Path);
	}

	private void DatePickerValue_ValueChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh && DataFilter != null)
		{
			DataFilter.Value = datePickerValue.Value.ToString("MM'/'dd'/'yyyy");
			DataFilter.Value2 = datePickerValue2.Value.ToString("MM'/'dd'/'yyyy");
			RefreshExpression();
			RefreshElementView();
			RefreshUiPanel();
		}
	}

	private void ComboBoxBoolean_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh && DataFilter != null)
		{
			DataFilter.Value = ((((ListControl)comboBoxBoolean).SelectedIndex == 0) ? "True" : "False");
			RefreshExpression();
			RefreshElementView();
			RefreshUiPanel();
		}
	}

	private void TextBoxValue_ButtonClick(object sender, EventArgs e)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Invalid comparison between Unknown and I4
		IStiDesignerBase designer = base.Element.Report.Designer;
		StiButtonEdit stiButtonEdit = sender as StiButtonEdit;
		string text = StiExpressionPacker.PackExpression(((Control)stiButtonEdit).Text, designer, useBraces: false);
		if ((int)StiTextEditor.EditText(designer.Report, ref text, StiCodeEditorMode.EditorDashboardExpression) == 1)
		{
			((Control)stiButtonEdit).Text = StiExpressionPacker.UnPackExpression(text, designer, useBraces: false);
		}
	}

	private void TextBoxValue_TextChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh && DataFilter != null)
		{
			DataFilter.Value = ((Control)textBoxValue).Text;
			DataFilter.Value2 = ((Control)textBoxValue2).Text;
			RefreshExpression();
			RefreshElementView();
			RefreshUiPanel();
		}
	}

	private void ButtonFieldMenu_Click(object sender, EventArgs e)
	{
		new StiDataFiltersExpressionMenu(menuExpression, (TextBox)(object)textBoxField, this, selectedListBox).CreateItems();
		Point point = new Point(((Control)panelBoxField).Width, ((Control)panelBoxField).Height + 1);
		if (StiLeftRightHandedHelper.IsLeftHanded)
		{
			point.X -= menuExpression.PopupSize.Width;
		}
		point = ((Control)panelBoxField).PointToScreen(point);
		menuExpression.Popup(point);
	}

	private void TextBoxField_TextChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh && DataFilter != null)
		{
			DataFilter.Path = ((Control)textBoxField).Text;
			listBoxFilters.SetSelectedText(DataFilter.GetStringRepresentation());
		}
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/dashboards_data_filtering_filters.htm");
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
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Expected O, but got Unknown
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected O, but got Unknown
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Expected O, but got Unknown
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Expected O, but got Unknown
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Expected O, but got Unknown
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Expected O, but got Unknown
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Expected O, but got Unknown
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Expected O, but got Unknown
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_036a: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fe: Expected O, but got Unknown
		//IL_043e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0571: Unknown result type (might be due to invalid IL or missing references)
		//IL_05db: Unknown result type (might be due to invalid IL or missing references)
		//IL_0685: Unknown result type (might be due to invalid IL or missing references)
		//IL_079a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0839: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0942: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a56: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b05: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c78: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d23: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0df8: Expected O, but got Unknown
		//IL_0e09: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e13: Expected O, but got Unknown
		//IL_0e4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fa2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1095: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_11aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_1275: Unknown result type (might be due to invalid IL or missing references)
		//IL_127f: Expected O, but got Unknown
		//IL_1290: Unknown result type (might be due to invalid IL or missing references)
		//IL_129a: Expected O, but got Unknown
		//IL_12d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1377: Unknown result type (might be due to invalid IL or missing references)
		//IL_141c: Unknown result type (might be due to invalid IL or missing references)
		//IL_14a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1543: Unknown result type (might be due to invalid IL or missing references)
		//IL_15bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_15c5: Expected O, but got Unknown
		//IL_1615: Unknown result type (might be due to invalid IL or missing references)
		//IL_168b: Unknown result type (might be due to invalid IL or missing references)
		//IL_17c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_17dd: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiDataFiltersEditorForm));
		toolTip = new ToolTip(components);
		panelFilterOperation = new Panel();
		labelFilterOperation = new Label();
		comboBoxFilterOperation = new StiComboBox();
		contextMenuBar = new ContextMenuBar();
		menuExpression = new ButtonItem();
		panelFilterOn = new Panel();
		labelFilterOn = new Label();
		comboBoxFilterOn = new StiDropDownBox();
		panelUi = new Panel();
		panelBoxField = new StiPanelBox();
		textBoxField = new HintTextBox();
		buttonFieldMenu = new StiOperationButton();
		labelField = new Label();
		labelCondition = new Label();
		comboBoxCondition = new StiComboBox();
		panelFrom = new Panel();
		labelValue = new Label();
		comboBoxValue = new StiDropDownBox();
		textBoxValue = new StiButtonEdit();
		datePickerValue = new DateTimePicker();
		comboBoxBoolean = new StiComboBox();
		panelTo = new Panel();
		labelValue2 = new Label();
		comboBoxValue2 = new StiDropDownBox();
		textBoxValue2 = new StiButtonEdit();
		datePickerValue2 = new DateTimePicker();
		panelExpression = new Panel();
		checkBoxExpression = new CheckBox();
		listBoxFilters = new StiDataFilterListBox();
		panelOperationDescription = new Panel();
		labelOperationDescription = new Label();
		((Control)panelFilterOperation).SuspendLayout();
		((ISupportInitialize)contextMenuBar).BeginInit();
		((Control)panelFilterOn).SuspendLayout();
		((Control)panelUi).SuspendLayout();
		((Control)panelBoxField).SuspendLayout();
		((Control)panelFrom).SuspendLayout();
		((Control)panelTo).SuspendLayout();
		((Control)panelExpression).SuspendLayout();
		((Control)panelOperationDescription).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)panelFilterOperation).BackColor = Color.White;
		((Control)panelFilterOperation).Controls.Add((Control)(object)labelFilterOperation);
		((Control)panelFilterOperation).Controls.Add((Control)(object)comboBoxFilterOperation);
		((Control)panelFilterOperation).Dock = (DockStyle)2;
		((Control)panelFilterOperation).Location = new Point(12, 234);
		((Control)panelFilterOperation).Margin = new Padding(2);
		((Control)panelFilterOperation).Name = "panelFilterOperation";
		((Control)panelFilterOperation).Size = new Size(298, 37);
		((Control)panelFilterOperation).TabIndex = 2;
		((Control)labelFilterOperation).ForeColor = Color.DimGray;
		((Control)labelFilterOperation).Location = new Point(0, 12);
		((Control)labelFilterOperation).Margin = new Padding(2, 0, 2, 0);
		((Control)labelFilterOperation).Name = "labelFilterOperation";
		((Control)labelFilterOperation).Size = new Size(118, 21);
		((Control)labelFilterOperation).TabIndex = 48;
		((Control)labelFilterOperation).Text = "Operation";
		labelFilterOperation.TextAlign = (ContentAlignment)16;
		((Control)comboBoxFilterOperation).Anchor = (AnchorStyles)13;
		((Control)comboBoxFilterOperation).BackColor = SystemColors.Window;
		comboBoxFilterOperation.DropDownStyle = (ComboBoxStyle)2;
		((ComboBox)comboBoxFilterOperation).DropDownWidth = 200;
		((Control)comboBoxFilterOperation).Location = new Point(122, 12);
		((Control)comboBoxFilterOperation).Margin = new Padding(2, 3, 2, 12);
		((ComboBox)comboBoxFilterOperation).MaxDropDownItems = 30;
		((Control)comboBoxFilterOperation).Name = "comboBoxFilterOperation";
		((Control)comboBoxFilterOperation).Size = new Size(174, 21);
		((Control)comboBoxFilterOperation).TabIndex = 0;
		((ComboBox)comboBoxFilterOperation).SelectedIndexChanged += ComboBoxFilterMode_SelectedIndexChanged;
		contextMenuBar.AntiAlias = true;
		contextMenuBar.DockTabStripHeight = 50;
		((Control)contextMenuBar).Font = new Font("Segoe UI", 9f);
		contextMenuBar.Items.AddRange(new BaseItem[1] { menuExpression });
		contextMenuBar.Location = new Point(210, 85);
		((Control)contextMenuBar).Margin = new Padding(2, 1, 2, 1);
		contextMenuBar.Name = "contextMenuBar";
		contextMenuBar.Size = new Size(100, 31);
		contextMenuBar.Stretch = true;
		contextMenuBar.Style = eDotNetBarStyle.StyleManagerControlled;
		contextMenuBar.TabIndex = 14;
		contextMenuBar.TabStop = false;
		((Control)contextMenuBar).Text = "contextMenuBar";
		contextMenuBar.WrapItemsDock = true;
		menuExpression.AutoExpandOnClick = true;
		menuExpression.HorizontalPadding = 0;
		menuExpression.ImagePaddingHorizontal = 16;
		menuExpression.ImagePaddingVertical = 12;
		menuExpression.Name = "menuExpression";
		menuExpression.Text = "menuExpression";
		((Control)panelFilterOn).BackColor = Color.White;
		((Control)panelFilterOn).Controls.Add((Control)(object)labelFilterOn);
		((Control)panelFilterOn).Controls.Add((Control)(object)comboBoxFilterOn);
		((Control)panelFilterOn).Dock = (DockStyle)2;
		((Control)panelFilterOn).Location = new Point(12, 397);
		((Control)panelFilterOn).Margin = new Padding(2);
		((Control)panelFilterOn).Name = "panelFilterOn";
		((Control)panelFilterOn).Size = new Size(298, 37);
		((Control)panelFilterOn).TabIndex = 7;
		((Control)labelFilterOn).ForeColor = Color.DimGray;
		((Control)labelFilterOn).Location = new Point(0, 12);
		((Control)labelFilterOn).Margin = new Padding(2, 0, 2, 0);
		((Control)labelFilterOn).Name = "labelFilterOn";
		((Control)labelFilterOn).Size = new Size(118, 21);
		((Control)labelFilterOn).TabIndex = 47;
		((Control)labelFilterOn).Text = "Filter On";
		labelFilterOn.TextAlign = (ContentAlignment)16;
		((Control)comboBoxFilterOn).BackColor = Color.White;
		comboBoxFilterOn.DrawMode = (DrawMode)2;
		comboBoxFilterOn.DropDownStyle = (ComboBoxStyle)2;
		comboBoxFilterOn.IsEditable = true;
		((Control)comboBoxFilterOn).Location = new Point(122, 12);
		((Control)comboBoxFilterOn).Margin = new Padding(2, 3, 2, 3);
		comboBoxFilterOn.MaxDropDownItems = 15;
		((Control)comboBoxFilterOn).Name = "comboBoxFilterOn";
		comboBoxFilterOn.SelectedIndex = -1;
		comboBoxFilterOn.SelectedItem = null;
		comboBoxFilterOn.SelectedValue = null;
		((Control)comboBoxFilterOn).Size = new Size(92, 21);
		((Control)comboBoxFilterOn).TabIndex = 41;
		comboBoxFilterOn.SelectedIndexChanged += ComboBoxFilterOn_SelectedIndexChanged;
		((Control)panelUi).BackColor = Color.White;
		((Control)panelUi).Controls.Add((Control)(object)panelBoxField);
		((Control)panelUi).Controls.Add((Control)(object)labelField);
		((Control)panelUi).Controls.Add((Control)(object)labelCondition);
		((Control)panelUi).Controls.Add((Control)(object)comboBoxCondition);
		((Control)panelUi).Dock = (DockStyle)2;
		((Control)panelUi).Location = new Point(12, 141);
		((Control)panelUi).Margin = new Padding(2);
		((Control)panelUi).Name = "panelUi";
		((Control)panelUi).Size = new Size(298, 93);
		((Control)panelUi).TabIndex = 1;
		((Control)panelBoxField).BackColor = Color.WhiteSmoke;
		((Control)panelBoxField).Controls.Add((Control)(object)textBoxField);
		((Control)panelBoxField).Controls.Add((Control)(object)buttonFieldMenu);
		((Control)panelBoxField).Dock = (DockStyle)1;
		((Control)panelBoxField).Location = new Point(0, 32);
		((Control)panelBoxField).Margin = new Padding(2);
		((Control)panelBoxField).Name = "panelBoxField";
		((Control)panelBoxField).Size = new Size(298, 25);
		((Control)panelBoxField).TabIndex = 1;
		((Control)textBoxField).Anchor = (AnchorStyles)13;
		((Control)textBoxField).BackColor = Color.WhiteSmoke;
		((TextBoxBase)textBoxField).BorderStyle = (BorderStyle)0;
		((Control)textBoxField).Location = new Point(4, 6);
		((Control)textBoxField).Margin = new Padding(2);
		((Control)textBoxField).Name = "textBoxField";
		((TextBoxBase)textBoxField).ReadOnly = true;
		((Control)textBoxField).Size = new Size(268, 13);
		((Control)textBoxField).TabIndex = 0;
		((Control)textBoxField).TextChanged += TextBoxField_TextChanged;
		((Control)buttonFieldMenu).Anchor = (AnchorStyles)9;
		((Control)buttonFieldMenu).Location = new Point(274, 1);
		((Control)buttonFieldMenu).Margin = new Padding(2);
		((Control)buttonFieldMenu).Name = "buttonFieldMenu";
		((Control)buttonFieldMenu).Size = new Size(23, 23);
		((Control)buttonFieldMenu).TabIndex = 2;
		((Control)buttonFieldMenu).TabStop = false;
		((ButtonBase)buttonFieldMenu).UseVisualStyleBackColor = true;
		((Control)buttonFieldMenu).Click += ButtonFieldMenu_Click;
		((Control)labelField).Dock = (DockStyle)1;
		((Control)labelField).ForeColor = Color.DimGray;
		((Control)labelField).Location = new Point(0, 0);
		((Control)labelField).Name = "labelField";
		((Control)labelField).Size = new Size(298, 32);
		((Control)labelField).TabIndex = 0;
		((Control)labelField).Text = "Field";
		labelField.TextAlign = (ContentAlignment)16;
		((Control)labelCondition).ForeColor = Color.DimGray;
		((Control)labelCondition).Location = new Point(0, 69);
		((Control)labelCondition).Margin = new Padding(2, 0, 2, 0);
		((Control)labelCondition).Name = "labelCondition";
		((Control)labelCondition).Size = new Size(118, 21);
		((Control)labelCondition).TabIndex = 46;
		((Control)labelCondition).Text = "Condition";
		labelCondition.TextAlign = (ContentAlignment)16;
		((Control)comboBoxCondition).Anchor = (AnchorStyles)13;
		((Control)comboBoxCondition).BackColor = SystemColors.Window;
		comboBoxCondition.DropDownStyle = (ComboBoxStyle)2;
		((ComboBox)comboBoxCondition).DropDownWidth = 200;
		((Control)comboBoxCondition).Location = new Point(122, 69);
		((Control)comboBoxCondition).Margin = new Padding(2, 3, 2, 3);
		((ComboBox)comboBoxCondition).MaxDropDownItems = 30;
		((Control)comboBoxCondition).Name = "comboBoxCondition";
		((Control)comboBoxCondition).Size = new Size(174, 21);
		((Control)comboBoxCondition).TabIndex = 1;
		((ComboBox)comboBoxCondition).SelectedIndexChanged += ComboBoxCondition_SelectedIndexChanged;
		((Control)panelFrom).BackColor = Color.White;
		((Control)panelFrom).Controls.Add((Control)(object)labelValue);
		((Control)panelFrom).Controls.Add((Control)(object)comboBoxValue);
		((Control)panelFrom).Controls.Add((Control)(object)textBoxValue);
		((Control)panelFrom).Controls.Add((Control)(object)datePickerValue);
		((Control)panelFrom).Controls.Add((Control)(object)comboBoxBoolean);
		((Control)panelFrom).Dock = (DockStyle)2;
		((Control)panelFrom).Location = new Point(12, 291);
		((Control)panelFrom).Margin = new Padding(2);
		((Control)panelFrom).Name = "panelFrom";
		((Control)panelFrom).Size = new Size(298, 37);
		((Control)panelFrom).TabIndex = 4;
		((Control)labelValue).ForeColor = Color.DimGray;
		((Control)labelValue).Location = new Point(0, 12);
		((Control)labelValue).Margin = new Padding(2, 0, 2, 0);
		((Control)labelValue).Name = "labelValue";
		((Control)labelValue).Size = new Size(118, 21);
		((Control)labelValue).TabIndex = 46;
		((Control)labelValue).Text = "Value";
		labelValue.TextAlign = (ContentAlignment)16;
		((Control)comboBoxValue).Anchor = (AnchorStyles)13;
		((Control)comboBoxValue).BackColor = Color.White;
		comboBoxValue.DrawMode = (DrawMode)2;
		comboBoxValue.IsEditable = true;
		((Control)comboBoxValue).Location = new Point(122, 12);
		((Control)comboBoxValue).Margin = new Padding(2, 3, 2, 3);
		comboBoxValue.MaxDropDownItems = 15;
		((Control)comboBoxValue).Name = "comboBoxValue";
		comboBoxValue.SelectedIndex = -1;
		comboBoxValue.SelectedItem = null;
		comboBoxValue.SelectedValue = null;
		((Control)comboBoxValue).Size = new Size(174, 21);
		((Control)comboBoxValue).TabIndex = 40;
		comboBoxValue.DropDown += ComboBoxValue_DropDown;
		((Control)comboBoxValue).TextChanged += ComboBoxValue_TextChanged;
		((Control)textBoxValue).Anchor = (AnchorStyles)13;
		((Control)textBoxValue).BackColor = SystemColors.Window;
		textBoxValue.ButtonBitmap = (Image)componentResourceManager.GetObject("textBoxValue.ButtonBitmap");
		textBoxValue.ButtonIcon = (Icon)componentResourceManager.GetObject("textBoxValue.ButtonIcon");
		textBoxValue.Flat = false;
		((Control)textBoxValue).ForeColor = SystemColors.ControlText;
		((Control)textBoxValue).Location = new Point(122, 12);
		((Control)textBoxValue).Margin = new Padding(2, 3, 2, 3);
		((Control)textBoxValue).Name = "textBoxValue";
		((Control)textBoxValue).Size = new Size(174, 21);
		((Control)textBoxValue).TabIndex = 45;
		textBoxValue.ButtonClick += TextBoxValue_ButtonClick;
		((Control)textBoxValue).TextChanged += TextBoxValue_TextChanged;
		((Control)datePickerValue).Anchor = (AnchorStyles)13;
		datePickerValue.Format = (DateTimePickerFormat)2;
		((Control)datePickerValue).Location = new Point(122, 12);
		((Control)datePickerValue).Margin = new Padding(2, 3, 2, 3);
		((Control)datePickerValue).Name = "datePickerValue";
		((Control)datePickerValue).Size = new Size(174, 20);
		((Control)datePickerValue).TabIndex = 42;
		datePickerValue.ValueChanged += DatePickerValue_ValueChanged;
		((Control)comboBoxBoolean).Anchor = (AnchorStyles)13;
		comboBoxBoolean.DropDownStyle = (ComboBoxStyle)2;
		((ComboBox)comboBoxBoolean).Items.AddRange(new object[2] { "True", "False" });
		((Control)comboBoxBoolean).Location = new Point(122, 12);
		((Control)comboBoxBoolean).Margin = new Padding(2, 3, 2, 3);
		((Control)comboBoxBoolean).Name = "comboBoxBoolean";
		((Control)comboBoxBoolean).Size = new Size(174, 21);
		((Control)comboBoxBoolean).TabIndex = 44;
		((Control)comboBoxBoolean).Visible = false;
		((ComboBox)comboBoxBoolean).SelectedIndexChanged += ComboBoxBoolean_SelectedIndexChanged;
		((Control)panelTo).BackColor = Color.White;
		((Control)panelTo).Controls.Add((Control)(object)labelValue2);
		((Control)panelTo).Controls.Add((Control)(object)comboBoxValue2);
		((Control)panelTo).Controls.Add((Control)(object)textBoxValue2);
		((Control)panelTo).Controls.Add((Control)(object)datePickerValue2);
		((Control)panelTo).Dock = (DockStyle)2;
		((Control)panelTo).Location = new Point(12, 328);
		((Control)panelTo).Margin = new Padding(2);
		((Control)panelTo).Name = "panelTo";
		((Control)panelTo).Size = new Size(298, 37);
		((Control)panelTo).TabIndex = 5;
		((Control)labelValue2).ForeColor = Color.DimGray;
		((Control)labelValue2).Location = new Point(0, 12);
		((Control)labelValue2).Margin = new Padding(2, 0, 2, 0);
		((Control)labelValue2).Name = "labelValue2";
		((Control)labelValue2).Size = new Size(118, 21);
		((Control)labelValue2).TabIndex = 47;
		((Control)labelValue2).Text = "Value2";
		labelValue2.TextAlign = (ContentAlignment)16;
		((Control)comboBoxValue2).Anchor = (AnchorStyles)13;
		((Control)comboBoxValue2).BackColor = Color.White;
		comboBoxValue2.DrawMode = (DrawMode)2;
		comboBoxValue2.IsEditable = true;
		((Control)comboBoxValue2).Location = new Point(122, 12);
		((Control)comboBoxValue2).Margin = new Padding(2, 3, 2, 3);
		comboBoxValue2.MaxDropDownItems = 15;
		((Control)comboBoxValue2).Name = "comboBoxValue2";
		comboBoxValue2.SelectedIndex = -1;
		comboBoxValue2.SelectedItem = null;
		comboBoxValue2.SelectedValue = null;
		((Control)comboBoxValue2).Size = new Size(174, 21);
		((Control)comboBoxValue2).TabIndex = 41;
		comboBoxValue2.DropDown += ComboBoxValue2_DropDown;
		((Control)comboBoxValue2).TextChanged += ComboBoxValue_TextChanged;
		((Control)textBoxValue2).Anchor = (AnchorStyles)13;
		((Control)textBoxValue2).BackColor = SystemColors.Window;
		textBoxValue2.ButtonBitmap = (Image)componentResourceManager.GetObject("textBoxValue2.ButtonBitmap");
		textBoxValue2.ButtonIcon = (Icon)componentResourceManager.GetObject("textBoxValue2.ButtonIcon");
		textBoxValue2.Flat = false;
		((Control)textBoxValue2).ForeColor = SystemColors.ControlText;
		((Control)textBoxValue2).Location = new Point(122, 12);
		((Control)textBoxValue2).Margin = new Padding(2, 3, 2, 3);
		((Control)textBoxValue2).Name = "textBoxValue2";
		((Control)textBoxValue2).Size = new Size(174, 21);
		((Control)textBoxValue2).TabIndex = 46;
		textBoxValue2.ButtonClick += TextBoxValue_ButtonClick;
		((Control)textBoxValue2).TextChanged += TextBoxValue_TextChanged;
		((Control)datePickerValue2).Anchor = (AnchorStyles)13;
		datePickerValue2.Format = (DateTimePickerFormat)2;
		((Control)datePickerValue2).Location = new Point(122, 12);
		((Control)datePickerValue2).Margin = new Padding(2, 3, 2, 3);
		((Control)datePickerValue2).Name = "datePickerValue2";
		((Control)datePickerValue2).Size = new Size(174, 20);
		((Control)datePickerValue2).TabIndex = 43;
		datePickerValue2.ValueChanged += DatePickerValue_ValueChanged;
		((Control)panelExpression).BackColor = Color.White;
		((Control)panelExpression).Controls.Add((Control)(object)checkBoxExpression);
		((Control)panelExpression).Dock = (DockStyle)2;
		((Control)panelExpression).Location = new Point(12, 365);
		((Control)panelExpression).Margin = new Padding(2);
		((Control)panelExpression).Name = "panelExpression";
		((Control)panelExpression).Size = new Size(298, 32);
		((Control)panelExpression).TabIndex = 6;
		((Control)checkBoxExpression).Anchor = (AnchorStyles)13;
		((Control)checkBoxExpression).BackColor = Color.Transparent;
		((ButtonBase)checkBoxExpression).FlatStyle = (FlatStyle)3;
		((Control)checkBoxExpression).Location = new Point(122, 12);
		((Control)checkBoxExpression).Margin = new Padding(2, 3, 2, 3);
		((Control)checkBoxExpression).Name = "checkBoxExpression";
		((Control)checkBoxExpression).Size = new Size(175, 18);
		((Control)checkBoxExpression).TabIndex = 0;
		((Control)checkBoxExpression).Text = "Expression";
		((ButtonBase)checkBoxExpression).UseVisualStyleBackColor = false;
		checkBoxExpression.CheckedChanged += CheckBoxExpression_CheckedChanged;
		((Control)listBoxFilters).AllowDrop = true;
		((Control)listBoxFilters).Dock = (DockStyle)1;
		((Control)listBoxFilters).Location = new Point(12, 12);
		((Control)listBoxFilters).Margin = new Padding(2);
		((Control)listBoxFilters).Name = "listBoxFilters";
		((Control)listBoxFilters).Size = new Size(298, 50);
		((Control)listBoxFilters).TabIndex = 0;
		((ListBox)listBoxFilters).SelectedIndexChanged += ListBox_AfterSelect;
		((Control)listBoxFilters).Enter += ListBox_Enter;
		((Control)listBoxFilters).MouseDown += new MouseEventHandler(ListBox_MouseDown);
		((Control)panelOperationDescription).BackColor = Color.White;
		((Control)panelOperationDescription).Controls.Add((Control)(object)labelOperationDescription);
		((Control)panelOperationDescription).Dock = (DockStyle)2;
		((Control)panelOperationDescription).Location = new Point(12, 271);
		((Control)panelOperationDescription).Margin = new Padding(2);
		((Control)panelOperationDescription).Name = "panelOperationDescription";
		((Control)panelOperationDescription).Size = new Size(298, 20);
		((Control)panelOperationDescription).TabIndex = 3;
		((Control)labelOperationDescription).AutoSize = true;
		((Control)labelOperationDescription).ForeColor = Color.DimGray;
		((Control)labelOperationDescription).Location = new Point(0, 4);
		((Control)labelOperationDescription).Margin = new Padding(2, 0, 2, 12);
		((Control)labelOperationDescription).MaximumSize = new Size(300, 30);
		((Control)labelOperationDescription).Name = "labelOperationDescription";
		((Control)labelOperationDescription).Size = new Size(84, 13);
		((Control)labelOperationDescription).TabIndex = 2;
		((Control)labelOperationDescription).Text = "Description Text";
		labelOperationDescription.TextAlign = (ContentAlignment)16;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(322, 446);
		((Control)this).Controls.Add((Control)(object)panelUi);
		((Control)this).Controls.Add((Control)(object)panelFilterOperation);
		((Control)this).Controls.Add((Control)(object)panelOperationDescription);
		((Control)this).Controls.Add((Control)(object)contextMenuBar);
		((Control)this).Controls.Add((Control)(object)listBoxFilters);
		((Control)this).Controls.Add((Control)(object)panelFrom);
		((Control)this).Controls.Add((Control)(object)panelTo);
		((Control)this).Controls.Add((Control)(object)panelExpression);
		((Control)this).Controls.Add((Control)(object)panelFilterOn);
		((Form)this).Margin = new Padding(3);
		((Control)this).Name = "StiDataFiltersEditorForm";
		((Control)this).Padding = new Padding(12);
		((Control)this).Text = "Filters";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Control)panelFilterOperation).ResumeLayout(false);
		((ISupportInitialize)contextMenuBar).EndInit();
		((Control)panelFilterOn).ResumeLayout(false);
		((Control)panelUi).ResumeLayout(false);
		((Control)panelBoxField).ResumeLayout(false);
		((Control)panelBoxField).PerformLayout();
		((Control)panelFrom).ResumeLayout(false);
		((Control)panelTo).ResumeLayout(false);
		((Control)panelExpression).ResumeLayout(false);
		((Control)panelOperationDescription).ResumeLayout(false);
		((Control)panelOperationDescription).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
