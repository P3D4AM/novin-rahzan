using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Components.DatePicker;
using Stimulsoft.Dashboard.Components.Table;
using Stimulsoft.Dashboard.Design.Controls;
using Stimulsoft.Dashboard.Design.Editors.Helpers;
using Stimulsoft.Dashboard.Design.Editors.Menu;
using Stimulsoft.Dashboard.Helpers;
using Stimulsoft.Data.Engine;
using Stimulsoft.Data.Helpers;
using Stimulsoft.Report;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Help;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Dashboard.Design.Editors;

public class StiDatePickerElementEditorForm : StiElementEditorForm
{
	private StiMeterListBox selectedListBox;

	private IContainer components;

	private StiMeterListBox listBoxValues;

	private Panel panelUi;

	private ToolTip toolTip;

	private StiChartGroupLabel labelValue;

	private Label labelSelectionMode;

	private StiComboBox comboBoxSelectionMode;

	private Label labelField;

	private Panel panelCondition;

	private Label labelCondition;

	private StiComboBox comboBoxCondition;

	private Panel panelInitialRangeSelection;

	private Label labelInitialRangeSelection;

	private StiComboBox comboBoxInitialRangeSelection;

	private ContextMenuBar contextMenuBar;

	private ButtonItem menuExpression;

	private StiPanelBox panelBoxField;

	private HintTextBox textBoxField;

	private StiOperationButton buttonFieldMenu;

	private StiOperationButton buttonFieldEdit;

	private Panel panelInitialRangeSelectionSource;

	private Label labelInitialRangeSelectionSource;

	private StiComboBox comboBoxInitialRangeSelectionSource;

	internal StiDatePickerElement DatePickerElement => base.Element as StiDatePickerElement;

	internal StiValueDatePickerMeter DatePickerValue => DatePickerElement?.ValueMeter;

	internal bool IsValuePresent => ((ListBox)listBoxValues).Items.Count > 0;

	internal StiMeterListBox ValuesListBox => listBoxValues;

	public override bool IsAnyPresent => IsValuePresent;

	public bool IsVariablePresent
	{
		get
		{
			if (IsValuePresent)
			{
				return StiVariableExpressionHelper.IsVariableSpecifiedAsExpression(DatePickerElement, DatePickerValue?.Expression);
			}
			return false;
		}
	}

	public bool IsRangeVariablePresent
	{
		get
		{
			if (IsVariablePresent)
			{
				return Stimulsoft.Report.Range.IsRangeType((StiVariableExpressionHelper.GetVariableSpecifiedAsExpression(DatePickerElement, DatePickerValue?.Expression) as StiVariable)?.Type);
			}
			return false;
		}
	}

	public override bool AllowResize => false;

	private string LocConditionEqualTo => CapitalFirstLetter(Loc.Get("PropertyEnum", "StiFilterConditionEqualTo"));

	private string LocConditionNotEqualTo => CapitalFirstLetter(Loc.Get("PropertyEnum", "StiFilterConditionNotEqualTo"));

	private string LocConditionGreaterThan => CapitalFirstLetter(Loc.Get("PropertyEnum", "StiFilterConditionGreaterThan"));

	private string LocConditionGreaterThanOrEqualTo => CapitalFirstLetter(Loc.Get("PropertyEnum", "StiFilterConditionGreaterThanOrEqualTo"));

	private string LocConditionLessThanOrEqualTo => CapitalFirstLetter(Loc.Get("PropertyEnum", "StiFilterConditionLessThanOrEqualTo"));

	private string LocConditionLessThan => CapitalFirstLetter(Loc.Get("PropertyEnum", "StiFilterConditionLessThan"));

	private string LocSelectionModeSingle => CapitalFirstLetter(Loc.Get("PropertyEnum", "StiDateSelectionModeSingle"));

	private string LocSelectionModeRange => CapitalFirstLetter(Loc.Get("PropertyEnum", "StiDateSelectionModeRange"));

	private string LocSelectionModeAutoRange => CapitalFirstLetter(Loc.Get("PropertyEnum", "StiDateSelectionModeAutoRange"));

	private string LocSourceSelection => CapitalFirstLetter(Loc.GetMain("Selection"));

	private string LocSourceVariable => CapitalFirstLetter(Loc.GetMain("Variable"));

	public static string LocDayTomorrow => Loc.Get("DatePickerRanges", "Tomorrow");

	public static string LocDayToday => Loc.Get("DatePickerRanges", "Today");

	public static string LocDayYesterday => Loc.Get("DatePickerRanges", "Yesterday");

	public static string LocLast7Days => Loc.Get("DatePickerRanges", "Last7Days");

	public static string LocLast14Days => Loc.Get("DatePickerRanges", "Last14Days");

	public static string LocLast30Days => Loc.Get("DatePickerRanges", "Last30Days");

	public static string LocWeekNext => Loc.Get("DatePickerRanges", "NextWeek");

	public static string LocWeekCurrent => Loc.Get("DatePickerRanges", "CurrentWeek");

	public static string LocWeekPrevious => Loc.Get("DatePickerRanges", "PreviousWeek");

	public static string LocMonthNext => Loc.Get("DatePickerRanges", "NextMonth");

	public static string LocMonthCurrent => Loc.Get("DatePickerRanges", "CurrentMonth");

	public static string LocMonthPrevious => Loc.Get("DatePickerRanges", "PreviousMonth");

	public static string LocQuarterNext => Loc.Get("DatePickerRanges", "NextQuarter");

	public static string LocQuarterCurrent => Loc.Get("DatePickerRanges", "CurrentQuarter");

	public static string LocQuarterPrevious => Loc.Get("DatePickerRanges", "PreviousQuarter");

	public static string LocQuarterFirst => Loc.Get("DatePickerRanges", "FirstQuarter");

	public static string LocQuarterSecond => Loc.Get("DatePickerRanges", "SecondQuarter");

	public static string LocQuarterThird => Loc.Get("DatePickerRanges", "ThirdQuarter");

	public static string LocQuarterFourth => Loc.Get("DatePickerRanges", "FourthQuarter");

	public static string LocYearNext => Loc.Get("DatePickerRanges", "NextYear");

	public static string LocYearCurrent => Loc.Get("DatePickerRanges", "CurrentYear");

	public static string LocYearPrevious => Loc.Get("DatePickerRanges", "PreviousYear");

	public static string LocDateToWeek => Loc.Get("DatePickerRanges", "WeekToDate");

	public static string LocDateToMonth => Loc.Get("DatePickerRanges", "MonthToDate");

	public static string LocDateToQuarter => Loc.Get("DatePickerRanges", "QuarterToDate");

	public static string LocDateToYear => Loc.Get("DatePickerRanges", "YearToDate");

	public StiDatePickerElementEditorForm()
		: this(null)
	{
	}

	public StiDatePickerElementEditorForm(StiElement element)
		: base(element)
	{
		InitializeComponent();
		if (StiOptions.Appearance.ShowFormsHelp)
		{
			((Form)this).HelpButton = true;
		}
		selectedListBox = listBoxValues;
		foreach (StiMeterListBox item in (from Control c in (IEnumerable)((Control)this).Controls
			where c is StiMeterListBox
			select c).Cast<StiMeterListBox>())
		{
			item.Designer = Designer;
			item.Editor = this;
			item.AllowMultipleElements = false;
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
		((Control)listBoxValues).Invalidate();
	}

	private void LocalizeInternal()
	{
		((Control)this).Text = Loc.Get("Components", "StiDatePicker");
		((Control)labelValue).Text = Loc.Get("PropertyMain", "Value");
		((Control)labelField).Text = Loc.Get("PropertyMain", "Field");
		((Control)labelSelectionMode).Text = Loc.Get("PropertyMain", "SelectionMode");
		((Control)labelCondition).Text = Loc.Get("PropertyMain", "Condition");
		((Control)labelInitialRangeSelection).Text = Loc.Get("PropertyMain", "InitialSelection");
		((Control)labelInitialRangeSelectionSource).Text = Loc.Get("PropertyMain", "InitialSelectionSource");
		((ComboBox)comboBoxSelectionMode).BeginUpdate();
		int selectedIndex = ((ListControl)comboBoxSelectionMode).SelectedIndex;
		((ComboBox)comboBoxSelectionMode).Items.Clear();
		((ComboBox)comboBoxSelectionMode).Items.Add((object)LocSelectionModeSingle);
		((ComboBox)comboBoxSelectionMode).Items.Add((object)LocSelectionModeRange);
		((ComboBox)comboBoxSelectionMode).Items.Add((object)LocSelectionModeAutoRange);
		((ListControl)comboBoxSelectionMode).SelectedIndex = selectedIndex;
		((ComboBox)comboBoxSelectionMode).EndUpdate();
		((ComboBox)comboBoxInitialRangeSelectionSource).BeginUpdate();
		selectedIndex = ((ListControl)comboBoxInitialRangeSelectionSource).SelectedIndex;
		((ComboBox)comboBoxInitialRangeSelectionSource).Items.Clear();
		((ComboBox)comboBoxInitialRangeSelectionSource).Items.Add((object)LocSourceSelection);
		((ComboBox)comboBoxInitialRangeSelectionSource).Items.Add((object)LocSourceVariable);
		((ListControl)comboBoxInitialRangeSelectionSource).SelectedIndex = selectedIndex;
		((ComboBox)comboBoxInitialRangeSelectionSource).EndUpdate();
		((ComboBox)comboBoxCondition).BeginUpdate();
		selectedIndex = ((ListControl)comboBoxCondition).SelectedIndex;
		((ComboBox)comboBoxCondition).Items.Clear();
		((ComboBox)comboBoxCondition).Items.Add((object)LocConditionEqualTo);
		((ComboBox)comboBoxCondition).Items.Add((object)LocConditionNotEqualTo);
		((ComboBox)comboBoxCondition).Items.Add((object)LocConditionGreaterThan);
		((ComboBox)comboBoxCondition).Items.Add((object)LocConditionGreaterThanOrEqualTo);
		((ComboBox)comboBoxCondition).Items.Add((object)LocConditionLessThan);
		((ComboBox)comboBoxCondition).Items.Add((object)LocConditionLessThanOrEqualTo);
		((ListControl)comboBoxCondition).SelectedIndex = selectedIndex;
		((ComboBox)comboBoxCondition).EndUpdate();
		((ComboBox)comboBoxInitialRangeSelection).BeginUpdate();
		selectedIndex = ((ListControl)comboBoxInitialRangeSelection).SelectedIndex;
		((ComboBox)comboBoxInitialRangeSelection).Items.Clear();
		((ComboBox)comboBoxInitialRangeSelection).Items.Add((object)LocDayTomorrow);
		((ComboBox)comboBoxInitialRangeSelection).Items.Add((object)LocDayToday);
		((ComboBox)comboBoxInitialRangeSelection).Items.Add((object)LocDayYesterday);
		((ComboBox)comboBoxInitialRangeSelection).Items.Add((object)LocLast7Days);
		((ComboBox)comboBoxInitialRangeSelection).Items.Add((object)LocLast14Days);
		((ComboBox)comboBoxInitialRangeSelection).Items.Add((object)LocLast30Days);
		((ComboBox)comboBoxInitialRangeSelection).Items.Add((object)LocWeekNext);
		((ComboBox)comboBoxInitialRangeSelection).Items.Add((object)LocWeekCurrent);
		((ComboBox)comboBoxInitialRangeSelection).Items.Add((object)LocWeekPrevious);
		((ComboBox)comboBoxInitialRangeSelection).Items.Add((object)LocMonthNext);
		((ComboBox)comboBoxInitialRangeSelection).Items.Add((object)LocMonthCurrent);
		((ComboBox)comboBoxInitialRangeSelection).Items.Add((object)LocMonthPrevious);
		((ComboBox)comboBoxInitialRangeSelection).Items.Add((object)LocQuarterNext);
		((ComboBox)comboBoxInitialRangeSelection).Items.Add((object)LocQuarterCurrent);
		((ComboBox)comboBoxInitialRangeSelection).Items.Add((object)LocQuarterPrevious);
		((ComboBox)comboBoxInitialRangeSelection).Items.Add((object)LocYearNext);
		((ComboBox)comboBoxInitialRangeSelection).Items.Add((object)LocYearCurrent);
		((ComboBox)comboBoxInitialRangeSelection).Items.Add((object)LocYearPrevious);
		((ComboBox)comboBoxInitialRangeSelection).Items.Add((object)LocQuarterFirst);
		((ComboBox)comboBoxInitialRangeSelection).Items.Add((object)LocQuarterSecond);
		((ComboBox)comboBoxInitialRangeSelection).Items.Add((object)LocQuarterThird);
		((ComboBox)comboBoxInitialRangeSelection).Items.Add((object)LocQuarterFourth);
		((ComboBox)comboBoxInitialRangeSelection).Items.Add((object)LocDateToWeek);
		((ComboBox)comboBoxInitialRangeSelection).Items.Add((object)LocDateToMonth);
		((ComboBox)comboBoxInitialRangeSelection).Items.Add((object)LocDateToQuarter);
		((ComboBox)comboBoxInitialRangeSelection).Items.Add((object)LocDateToYear);
		((ListControl)comboBoxInitialRangeSelection).SelectedIndex = selectedIndex;
		((ComboBox)comboBoxInitialRangeSelection).EndUpdate();
	}

	private void InitializeMenus()
	{
		menuExpression.PopupOpen += delegate
		{
			new StiDatePickerExpressionMenu(menuExpression, (TextBox)(object)textBoxField, this, selectedListBox).CreateItems();
		};
	}

	private void DoScaling()
	{
		((ButtonBase)buttonFieldEdit).Image = StiReportDesignImages.Actions.Edit();
		StiComboBox stiComboBox = comboBoxCondition;
		StiComboBox stiComboBox2 = comboBoxInitialRangeSelection;
		StiComboBox stiComboBox3 = comboBoxSelectionMode;
		int num2 = (((Control)comboBoxInitialRangeSelectionSource).Width = ((Control)panelUi).Width - ((Control)comboBoxSelectionMode).Left);
		int num4 = (((Control)stiComboBox3).Width = num2);
		int width = (((Control)stiComboBox2).Width = num4);
		((Control)stiComboBox).Width = width;
		StiButtonsLocator.ArrangeButtons((Control)(object)panelBoxField);
	}

	protected override void OnGlobalElementChanged(object sender, EventArgs e)
	{
		if (sender == base.Element)
		{
			ReadPropertiesFromElement();
			listBoxValues.SelectedMeterItem = listBoxValues.FirstItem();
		}
	}

	public override void ReadPropertiesFromElement()
	{
		ReadValuesPropertiesFromElement();
	}

	private void ReadValuesPropertiesFromElement()
	{
		((ListBox)listBoxValues).BeginUpdate();
		((ListBox)listBoxValues).Items.Clear();
		listBoxValues.AddItem(DatePickerValue);
		listBoxValues.SelectFirstItem();
		((ListBox)listBoxValues).EndUpdate();
	}

	public override void WritePropertiesToElement()
	{
		WriteValuesPropertiesToElement();
		RefreshElementView();
	}

	private void WriteValuesPropertiesToElement()
	{
		StiMeterItem stiMeterItem = listBoxValues.FirstItem();
		DatePickerElement.ValueMeter = stiMeterItem?.Meter as StiValueDatePickerMeter;
	}

	protected override void AttachEvents()
	{
		base.AttachEvents();
		AttachEventsToValues();
	}

	private void AttachEventsToValues()
	{
		listBoxValues.ElementsChanged += delegate(object sender, StiElementChangedArgs changedArgs)
		{
			ProcessElementChanging(changedArgs);
			WritePropertiesToElement();
			RefreshUiPanel();
		};
		listBoxValues.ElementsDragEnter += delegate
		{
			ResetComponentHighlights();
		};
		listBoxValues.ElementsDragDrop += delegate
		{
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			if (StiDragDropColumnHelper.IsDataColumnOrVariableDragged(Designer))
			{
				IStiAppDataCell draggedDataColumnOrVariable = StiDragDropColumnHelper.GetDraggedDataColumnOrVariable(Designer);
				if (draggedDataColumnOrVariable == null)
				{
					return;
				}
				Type type = StiMeterHelper.ToDataType(draggedDataColumnOrVariable);
				if (!(type == typeof(DateTime)) && !(type == typeof(DateTime?)) && !(type == typeof(DateTimeRange)))
				{
					MessageBox.Show((IWin32Window)(object)this, string.Format(Loc.Get("Dashboard", "FieldTypeRestrictionHint"), Loc.Get("FormFormatEditor", "Date")), Loc.Get("FormDesigner", "title"));
					return;
				}
				((ListBox)listBoxValues).Items.Clear();
				listBoxValues.AddItem(StiMeterHelper.DatePicker.GetValue(draggedDataColumnOrVariable));
				Designer.PageView.ResetDragging();
				DatePickerElement.Select();
				WritePropertiesToElement();
				listBoxValues.SelectedMeterItem = listBoxValues.LastItem();
				((Control)listBoxValues).Focus();
			}
			else if (StiDragDropColumnHelper.IsTableElementDragged(Designer))
			{
				StiTableElement table = StiDragDropColumnHelper.GetDraggedTableElement(Designer) as StiTableElement;
				if (table == null || table.Columns == null || table.Columns.Count == 0)
				{
					return;
				}
				StiDataColumn stiDataColumn = (from c in table.Columns
					select StiDataExpressionHelper.GetDataColumnFromExpression(table.Dashboard, c.Expression) as StiDataColumn into c
					where c != null
					select c).FirstOrDefault((StiDataColumn c) => c.Type == typeof(DateTime) || c.Type == typeof(DateTime?));
				if (stiDataColumn == null)
				{
					MessageBox.Show((IWin32Window)(object)this, string.Format(Loc.Get("Dashboard", "FieldTypeRestrictionHint"), Loc.Get("FormFormatEditor", "Date")), Loc.Get("FormDesigner", "title"));
					return;
				}
				((ListBox)listBoxValues).Items.Clear();
				listBoxValues.AddItem(StiMeterHelper.DatePicker.GetValue(stiDataColumn));
				Designer.PageView.ResetDragging();
				WritePropertiesToElement();
				listBoxValues.SelectedMeterItem = listBoxValues.LastItem();
				((Control)listBoxValues).Focus();
			}
			ResetComponentHighlights();
			RefreshUiPanel();
		};
		listBoxValues.ElementsSelectedChanged += delegate
		{
			DatePickerElement.Select();
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
			ReadCondition();
			ReadSelectionMode();
			ReadInitialRangeSelection();
			ReadInitialRangeSelectionSource();
			StiChartGroupLabel stiChartGroupLabel = labelValue;
			Panel obj = panelUi;
			Label obj2 = labelField;
			bool flag = (((Control)panelBoxField).Visible = IsAnyPresent);
			bool flag3 = (((Control)obj2).Visible = flag);
			bool visible = (((Control)obj).Visible = flag3);
			((Control)stiChartGroupLabel).Visible = visible;
			((Control)listBoxValues).Dock = (DockStyle)(IsAnyPresent ? 1 : 5);
			((Control)panelCondition).Visible = IsAnyPresent && !IsVariablePresent && DatePickerElement.SelectionMode == StiDateSelectionMode.Single;
			((Control)panelInitialRangeSelectionSource).Visible = IsAnyPresent && IsRangeVariablePresent;
			((Control)panelInitialRangeSelection).Enabled = DatePickerElement.InitialRangeSelectionSource == StiInitialDateRangeSelectionSource.Selection || !IsVariablePresent;
			SetListHeightFromCount(listBoxValues, multiple: false);
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

	private void ReadCondition()
	{
		switch (DatePickerElement.Condition)
		{
		case StiDateCondition.EqualTo:
			((ComboBox)comboBoxCondition).SelectedItem = LocConditionEqualTo;
			break;
		case StiDateCondition.NotEqualTo:
			((ComboBox)comboBoxCondition).SelectedItem = LocConditionNotEqualTo;
			break;
		case StiDateCondition.GreaterThan:
			((ComboBox)comboBoxCondition).SelectedItem = LocConditionGreaterThan;
			break;
		case StiDateCondition.GreaterThanOrEqualTo:
			((ComboBox)comboBoxCondition).SelectedItem = LocConditionGreaterThanOrEqualTo;
			break;
		case StiDateCondition.LessThan:
			((ComboBox)comboBoxCondition).SelectedItem = LocConditionLessThan;
			break;
		case StiDateCondition.LessThanOrEqualTo:
			((ComboBox)comboBoxCondition).SelectedItem = LocConditionLessThanOrEqualTo;
			break;
		}
	}

	private void ReadSelectionMode()
	{
		if (IsVariablePresent)
		{
			if (StiVariableExpressionHelper.GetVariableSpecifiedAsExpression(DatePickerElement, DatePickerValue?.Expression) is StiVariable stiVariable && Stimulsoft.Report.Range.IsRangeType(stiVariable.Type))
			{
				((ComboBox)comboBoxSelectionMode).SelectedItem = LocSelectionModeRange;
				((Control)panelCondition).Visible = false;
				((Control)panelInitialRangeSelection).Visible = IsAnyPresent;
			}
			else
			{
				((ComboBox)comboBoxSelectionMode).SelectedItem = LocSelectionModeSingle;
				((Control)panelCondition).Visible = IsAnyPresent;
				((Control)panelInitialRangeSelection).Visible = false;
			}
			((Control)comboBoxSelectionMode).Enabled = false;
			return;
		}
		switch (DatePickerElement.SelectionMode)
		{
		case StiDateSelectionMode.Single:
			((ComboBox)comboBoxSelectionMode).SelectedItem = LocSelectionModeSingle;
			((Control)panelCondition).Visible = IsAnyPresent;
			((Control)panelInitialRangeSelection).Visible = false;
			break;
		case StiDateSelectionMode.Range:
			((ComboBox)comboBoxSelectionMode).SelectedItem = LocSelectionModeRange;
			((Control)panelCondition).Visible = false;
			((Control)panelInitialRangeSelection).Visible = IsAnyPresent;
			break;
		case StiDateSelectionMode.AutoRange:
			((ComboBox)comboBoxSelectionMode).SelectedItem = LocSelectionModeAutoRange;
			((Control)panelCondition).Visible = false;
			((Control)panelInitialRangeSelection).Visible = false;
			break;
		}
		((Control)comboBoxSelectionMode).Enabled = true;
	}

	private void ReadInitialRangeSelection()
	{
		switch (DatePickerElement.InitialRangeSelection)
		{
		case StiInitialDateRangeSelection.DayTomorrow:
			((ComboBox)comboBoxInitialRangeSelection).SelectedItem = LocDayTomorrow;
			break;
		case StiInitialDateRangeSelection.DayToday:
			((ComboBox)comboBoxInitialRangeSelection).SelectedItem = LocDayToday;
			break;
		case StiInitialDateRangeSelection.DayYesterday:
			((ComboBox)comboBoxInitialRangeSelection).SelectedItem = LocDayYesterday;
			break;
		case StiInitialDateRangeSelection.Last7Days:
			((ComboBox)comboBoxInitialRangeSelection).SelectedItem = LocLast7Days;
			break;
		case StiInitialDateRangeSelection.Last14Days:
			((ComboBox)comboBoxInitialRangeSelection).SelectedItem = LocLast14Days;
			break;
		case StiInitialDateRangeSelection.Last30Days:
			((ComboBox)comboBoxInitialRangeSelection).SelectedItem = LocLast30Days;
			break;
		case StiInitialDateRangeSelection.WeekNext:
			((ComboBox)comboBoxInitialRangeSelection).SelectedItem = LocWeekNext;
			break;
		case StiInitialDateRangeSelection.WeekCurrent:
			((ComboBox)comboBoxInitialRangeSelection).SelectedItem = LocWeekCurrent;
			break;
		case StiInitialDateRangeSelection.WeekPrevious:
			((ComboBox)comboBoxInitialRangeSelection).SelectedItem = LocWeekPrevious;
			break;
		case StiInitialDateRangeSelection.MonthNext:
			((ComboBox)comboBoxInitialRangeSelection).SelectedItem = LocMonthNext;
			break;
		case StiInitialDateRangeSelection.MonthCurrent:
			((ComboBox)comboBoxInitialRangeSelection).SelectedItem = LocMonthCurrent;
			break;
		case StiInitialDateRangeSelection.MonthPrevious:
			((ComboBox)comboBoxInitialRangeSelection).SelectedItem = LocMonthPrevious;
			break;
		case StiInitialDateRangeSelection.QuarterNext:
			((ComboBox)comboBoxInitialRangeSelection).SelectedItem = LocQuarterNext;
			break;
		case StiInitialDateRangeSelection.QuarterCurrent:
			((ComboBox)comboBoxInitialRangeSelection).SelectedItem = LocQuarterCurrent;
			break;
		case StiInitialDateRangeSelection.QuarterPrevious:
			((ComboBox)comboBoxInitialRangeSelection).SelectedItem = LocQuarterPrevious;
			break;
		case StiInitialDateRangeSelection.QuarterFirst:
			((ComboBox)comboBoxInitialRangeSelection).SelectedItem = LocQuarterFirst;
			break;
		case StiInitialDateRangeSelection.QuarterSecond:
			((ComboBox)comboBoxInitialRangeSelection).SelectedItem = LocQuarterSecond;
			break;
		case StiInitialDateRangeSelection.QuarterThird:
			((ComboBox)comboBoxInitialRangeSelection).SelectedItem = LocQuarterThird;
			break;
		case StiInitialDateRangeSelection.QuarterFourth:
			((ComboBox)comboBoxInitialRangeSelection).SelectedItem = LocQuarterFourth;
			break;
		case StiInitialDateRangeSelection.YearNext:
			((ComboBox)comboBoxInitialRangeSelection).SelectedItem = LocYearNext;
			break;
		case StiInitialDateRangeSelection.YearCurrent:
			((ComboBox)comboBoxInitialRangeSelection).SelectedItem = LocYearCurrent;
			break;
		case StiInitialDateRangeSelection.YearPrevious:
			((ComboBox)comboBoxInitialRangeSelection).SelectedItem = LocYearPrevious;
			break;
		case StiInitialDateRangeSelection.DateToWeek:
			((ComboBox)comboBoxInitialRangeSelection).SelectedItem = LocDateToWeek;
			break;
		case StiInitialDateRangeSelection.DateToMonth:
			((ComboBox)comboBoxInitialRangeSelection).SelectedItem = LocDateToMonth;
			break;
		case StiInitialDateRangeSelection.DateToQuarter:
			((ComboBox)comboBoxInitialRangeSelection).SelectedItem = LocDateToQuarter;
			break;
		case StiInitialDateRangeSelection.DateToYear:
			((ComboBox)comboBoxInitialRangeSelection).SelectedItem = LocDateToYear;
			break;
		}
	}

	private void ReadInitialRangeSelectionSource()
	{
		switch (DatePickerElement.InitialRangeSelectionSource)
		{
		case StiInitialDateRangeSelectionSource.Selection:
			((ComboBox)comboBoxInitialRangeSelectionSource).SelectedItem = LocSourceSelection;
			break;
		case StiInitialDateRangeSelectionSource.Variable:
			((ComboBox)comboBoxInitialRangeSelectionSource).SelectedItem = LocSourceVariable;
			break;
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
			((Form)this).ClientSize = new Size(((Form)this).ClientSize.Width, base.PaddingCorrection + ((Control)listBoxValues).Width);
		}
	}

	private string CapitalFirstLetter(string str)
	{
		return str.Substring(0, 1).ToUpperInvariant() + str.Substring(1);
	}

	internal void RefreshExpression()
	{
		base.LockRefresh = true;
		if (selectedListBox == listBoxValues && DatePickerValue != null)
		{
			((Control)textBoxField).Text = DatePickerValue.Expression;
		}
		else
		{
			((Control)textBoxField).Text = string.Empty;
		}
		base.LockRefresh = false;
	}

	private void ButtonFieldEdit_Click(object sender, EventArgs e)
	{
		if (StiExpressionEditorHelper.RunExpressionEditor(base.Element, ref textBoxField))
		{
			RefreshElementView();
		}
	}

	private void ComboBoxCondition_SelectedIndexChanged(object sender, EventArgs e)
	{
		string text = ((ComboBox)comboBoxCondition).SelectedItem.ToString();
		if (text == LocConditionEqualTo)
		{
			DatePickerElement.Condition = StiDateCondition.EqualTo;
		}
		else if (text == LocConditionNotEqualTo)
		{
			DatePickerElement.Condition = StiDateCondition.NotEqualTo;
		}
		else if (text == LocConditionGreaterThan)
		{
			DatePickerElement.Condition = StiDateCondition.GreaterThan;
		}
		else if (text == LocConditionGreaterThanOrEqualTo)
		{
			DatePickerElement.Condition = StiDateCondition.GreaterThanOrEqualTo;
		}
		else if (text == LocConditionLessThan)
		{
			DatePickerElement.Condition = StiDateCondition.LessThan;
		}
		else if (text == LocConditionLessThanOrEqualTo)
		{
			DatePickerElement.Condition = StiDateCondition.LessThanOrEqualTo;
		}
		RefreshElementView();
		RefreshUiPanel();
	}

	private void ComboBoxSelection_SelectedIndexChanged(object sender, EventArgs e)
	{
		string text = ((ComboBox)comboBoxSelectionMode).SelectedItem.ToString();
		if (text == LocSelectionModeSingle)
		{
			DatePickerElement.SelectionMode = StiDateSelectionMode.Single;
		}
		else if (text == LocSelectionModeRange)
		{
			DatePickerElement.SelectionMode = StiDateSelectionMode.Range;
		}
		else if (text == LocSelectionModeAutoRange)
		{
			DatePickerElement.SelectionMode = StiDateSelectionMode.AutoRange;
		}
		RefreshElementView();
		RefreshUiPanel();
	}

	private void ComboBoxInitialRangeSelection_SelectedIndexChanged(object sender, EventArgs e)
	{
		string text = ((ComboBox)comboBoxInitialRangeSelection).SelectedItem.ToString();
		if (text == LocDayTomorrow)
		{
			DatePickerElement.InitialRangeSelection = StiInitialDateRangeSelection.DayTomorrow;
		}
		else if (text == LocDayToday)
		{
			DatePickerElement.InitialRangeSelection = StiInitialDateRangeSelection.DayToday;
		}
		else if (text == LocDayYesterday)
		{
			DatePickerElement.InitialRangeSelection = StiInitialDateRangeSelection.DayYesterday;
		}
		else if (text == LocLast7Days)
		{
			DatePickerElement.InitialRangeSelection = StiInitialDateRangeSelection.Last7Days;
		}
		else if (text == LocLast14Days)
		{
			DatePickerElement.InitialRangeSelection = StiInitialDateRangeSelection.Last14Days;
		}
		else if (text == LocLast30Days)
		{
			DatePickerElement.InitialRangeSelection = StiInitialDateRangeSelection.Last30Days;
		}
		else if (text == LocWeekNext)
		{
			DatePickerElement.InitialRangeSelection = StiInitialDateRangeSelection.WeekNext;
		}
		else if (text == LocWeekCurrent)
		{
			DatePickerElement.InitialRangeSelection = StiInitialDateRangeSelection.WeekCurrent;
		}
		else if (text == LocWeekPrevious)
		{
			DatePickerElement.InitialRangeSelection = StiInitialDateRangeSelection.WeekPrevious;
		}
		else if (text == LocMonthNext)
		{
			DatePickerElement.InitialRangeSelection = StiInitialDateRangeSelection.MonthNext;
		}
		else if (text == LocMonthCurrent)
		{
			DatePickerElement.InitialRangeSelection = StiInitialDateRangeSelection.MonthCurrent;
		}
		else if (text == LocMonthPrevious)
		{
			DatePickerElement.InitialRangeSelection = StiInitialDateRangeSelection.MonthPrevious;
		}
		else if (text == LocQuarterNext)
		{
			DatePickerElement.InitialRangeSelection = StiInitialDateRangeSelection.QuarterNext;
		}
		else if (text == LocQuarterCurrent)
		{
			DatePickerElement.InitialRangeSelection = StiInitialDateRangeSelection.QuarterCurrent;
		}
		else if (text == LocQuarterPrevious)
		{
			DatePickerElement.InitialRangeSelection = StiInitialDateRangeSelection.QuarterPrevious;
		}
		else if (text == LocQuarterFirst)
		{
			DatePickerElement.InitialRangeSelection = StiInitialDateRangeSelection.QuarterFirst;
		}
		else if (text == LocQuarterSecond)
		{
			DatePickerElement.InitialRangeSelection = StiInitialDateRangeSelection.QuarterSecond;
		}
		else if (text == LocQuarterThird)
		{
			DatePickerElement.InitialRangeSelection = StiInitialDateRangeSelection.QuarterThird;
		}
		else if (text == LocQuarterFourth)
		{
			DatePickerElement.InitialRangeSelection = StiInitialDateRangeSelection.QuarterFourth;
		}
		else if (text == LocYearNext)
		{
			DatePickerElement.InitialRangeSelection = StiInitialDateRangeSelection.YearNext;
		}
		else if (text == LocYearCurrent)
		{
			DatePickerElement.InitialRangeSelection = StiInitialDateRangeSelection.YearCurrent;
		}
		else if (text == LocYearPrevious)
		{
			DatePickerElement.InitialRangeSelection = StiInitialDateRangeSelection.YearPrevious;
		}
		else if (text == LocDateToWeek)
		{
			DatePickerElement.InitialRangeSelection = StiInitialDateRangeSelection.DateToWeek;
		}
		else if (text == LocDateToMonth)
		{
			DatePickerElement.InitialRangeSelection = StiInitialDateRangeSelection.DateToMonth;
		}
		else if (text == LocDateToQuarter)
		{
			DatePickerElement.InitialRangeSelection = StiInitialDateRangeSelection.DateToQuarter;
		}
		else if (text == LocDateToYear)
		{
			DatePickerElement.InitialRangeSelection = StiInitialDateRangeSelection.DateToYear;
		}
		RefreshElementView();
		RefreshUiPanel();
	}

	private void ComboBoxInitialRangeSelectionSource_SelectedIndexChanged(object sender, EventArgs e)
	{
		string text = ((ComboBox)comboBoxInitialRangeSelectionSource).SelectedItem.ToString();
		if (text == LocSourceSelection)
		{
			DatePickerElement.InitialRangeSelectionSource = StiInitialDateRangeSelectionSource.Selection;
		}
		if (text == LocSourceVariable)
		{
			DatePickerElement.InitialRangeSelectionSource = StiInitialDateRangeSelectionSource.Variable;
		}
		RefreshElementView();
		RefreshUiPanel();
	}

	private void TextBoxField_TextChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh && selectedListBox == listBoxValues && DatePickerValue != null)
		{
			DatePickerValue.Expression = ((Control)textBoxField).Text;
			listBoxValues.SetSelectedText(StiLabelHelper.GetLabel(DatePickerValue));
		}
	}

	private void ButtonFieldMenu_Click(object sender, EventArgs e)
	{
		new StiDatePickerExpressionMenu(menuExpression, (TextBox)(object)textBoxField, this, selectedListBox).CreateItems();
		Point point = new Point(((Control)panelBoxField).Width, ((Control)panelBoxField).Height + 1);
		if (StiLeftRightHandedHelper.IsLeftHanded)
		{
			point.X -= menuExpression.PopupSize.Width;
		}
		point = ((Control)panelBoxField).PointToScreen(point);
		menuExpression.Popup(point);
	}

	private void ListBox_Enter(object sender, EventArgs e)
	{
		selectedListBox = sender as StiMeterListBox;
		RefreshExpression();
	}

	private void ListBox_AfterSelect(object sender, EventArgs e)
	{
		selectedListBox = sender as StiMeterListBox;
		RefreshExpression();
	}

	private void ListBox_MouseDown(object sender, MouseEventArgs e)
	{
		selectedListBox = sender as StiMeterListBox;
		RefreshExpression();
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/dashboards_data_filtering_date_picker.htm");
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
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Expected O, but got Unknown
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Expected O, but got Unknown
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Expected O, but got Unknown
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0377: Unknown result type (might be due to invalid IL or missing references)
		//IL_0444: Unknown result type (might be due to invalid IL or missing references)
		//IL_049f: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a9: Expected O, but got Unknown
		//IL_04c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0571: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_070e: Unknown result type (might be due to invalid IL or missing references)
		//IL_081a: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae2: Expected O, but got Unknown
		//IL_0b22: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d45: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e66: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiDatePickerElementEditorForm));
		listBoxValues = new StiMeterListBox();
		panelUi = new Panel();
		labelSelectionMode = new Label();
		comboBoxSelectionMode = new StiComboBox();
		panelBoxField = new StiPanelBox();
		buttonFieldEdit = new StiOperationButton();
		textBoxField = new HintTextBox();
		buttonFieldMenu = new StiOperationButton();
		labelField = new Label();
		toolTip = new ToolTip(components);
		labelValue = new StiChartGroupLabel();
		panelCondition = new Panel();
		labelCondition = new Label();
		comboBoxCondition = new StiComboBox();
		panelInitialRangeSelection = new Panel();
		labelInitialRangeSelection = new Label();
		comboBoxInitialRangeSelection = new StiComboBox();
		contextMenuBar = new ContextMenuBar();
		menuExpression = new ButtonItem();
		panelInitialRangeSelectionSource = new Panel();
		labelInitialRangeSelectionSource = new Label();
		comboBoxInitialRangeSelectionSource = new StiComboBox();
		((Control)panelUi).SuspendLayout();
		((Control)panelBoxField).SuspendLayout();
		((Control)panelCondition).SuspendLayout();
		((Control)panelInitialRangeSelection).SuspendLayout();
		((ISupportInitialize)contextMenuBar).BeginInit();
		((Control)panelInitialRangeSelectionSource).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)listBoxValues).AllowDrop = true;
		((Control)listBoxValues).Dock = (DockStyle)1;
		((Control)listBoxValues).Location = new Point(12, 44);
		((Control)listBoxValues).Margin = new Padding(2);
		((Control)listBoxValues).Name = "listBoxValues";
		((Control)listBoxValues).Size = new Size(314, 50);
		((Control)listBoxValues).TabIndex = 1;
		((ListBox)listBoxValues).SelectedIndexChanged += ListBox_AfterSelect;
		((Control)listBoxValues).Enter += ListBox_Enter;
		((Control)listBoxValues).MouseDown += new MouseEventHandler(ListBox_MouseDown);
		((Control)panelUi).BackColor = Color.White;
		((Control)panelUi).Controls.Add((Control)(object)labelSelectionMode);
		((Control)panelUi).Controls.Add((Control)(object)comboBoxSelectionMode);
		((Control)panelUi).Dock = (DockStyle)2;
		((Control)panelUi).Location = new Point(12, 230);
		((Control)panelUi).Margin = new Padding(2);
		((Control)panelUi).Name = "panelUi";
		((Control)panelUi).Size = new Size(314, 38);
		((Control)panelUi).TabIndex = 4;
		((Control)labelSelectionMode).ForeColor = Color.DimGray;
		((Control)labelSelectionMode).Location = new Point(0, 12);
		((Control)labelSelectionMode).Margin = new Padding(2, 0, 2, 0);
		((Control)labelSelectionMode).Name = "labelSelectionMode";
		((Control)labelSelectionMode).Size = new Size(121, 17);
		((Control)labelSelectionMode).TabIndex = 0;
		((Control)labelSelectionMode).Text = "Selection Mode";
		labelSelectionMode.TextAlign = (ContentAlignment)16;
		((Control)comboBoxSelectionMode).Anchor = (AnchorStyles)13;
		comboBoxSelectionMode.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxSelectionMode).FormattingEnabled = true;
		((Control)comboBoxSelectionMode).Location = new Point(126, 12);
		((Control)comboBoxSelectionMode).Margin = new Padding(2);
		((Control)comboBoxSelectionMode).Name = "comboBoxSelectionMode";
		((Control)comboBoxSelectionMode).Size = new Size(188, 21);
		((Control)comboBoxSelectionMode).TabIndex = 0;
		((ComboBox)comboBoxSelectionMode).SelectedIndexChanged += ComboBoxSelection_SelectedIndexChanged;
		((Control)panelBoxField).BackColor = Color.WhiteSmoke;
		((Control)panelBoxField).Controls.Add((Control)(object)buttonFieldEdit);
		((Control)panelBoxField).Controls.Add((Control)(object)textBoxField);
		((Control)panelBoxField).Controls.Add((Control)(object)buttonFieldMenu);
		((Control)panelBoxField).Dock = (DockStyle)1;
		((Control)panelBoxField).Location = new Point(12, 126);
		((Control)panelBoxField).Margin = new Padding(2);
		((Control)panelBoxField).Name = "panelBoxField";
		((Control)panelBoxField).Size = new Size(314, 25);
		((Control)panelBoxField).TabIndex = 3;
		((Control)buttonFieldEdit).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonFieldEdit).Image = (Image)componentResourceManager.GetObject("buttonFieldEdit.Image");
		((Control)buttonFieldEdit).Location = new Point(268, 1);
		((Control)buttonFieldEdit).Margin = new Padding(2);
		((Control)buttonFieldEdit).Name = "buttonFieldEdit";
		((Control)buttonFieldEdit).Size = new Size(23, 23);
		((Control)buttonFieldEdit).TabIndex = 1;
		((Control)buttonFieldEdit).TabStop = false;
		((ButtonBase)buttonFieldEdit).UseVisualStyleBackColor = true;
		((Control)buttonFieldEdit).Click += ButtonFieldEdit_Click;
		((Control)textBoxField).Anchor = (AnchorStyles)13;
		((Control)textBoxField).BackColor = Color.WhiteSmoke;
		((TextBoxBase)textBoxField).BorderStyle = (BorderStyle)0;
		((Control)textBoxField).Location = new Point(4, 6);
		((Control)textBoxField).Margin = new Padding(2);
		((Control)textBoxField).Name = "textBoxField";
		((TextBoxBase)textBoxField).ReadOnly = true;
		((Control)textBoxField).Size = new Size(260, 13);
		((Control)textBoxField).TabIndex = 0;
		((Control)textBoxField).TextChanged += TextBoxField_TextChanged;
		((Control)buttonFieldMenu).Anchor = (AnchorStyles)9;
		((Control)buttonFieldMenu).Location = new Point(290, 1);
		((Control)buttonFieldMenu).Margin = new Padding(2);
		((Control)buttonFieldMenu).Name = "buttonFieldMenu";
		((Control)buttonFieldMenu).Size = new Size(23, 23);
		((Control)buttonFieldMenu).TabIndex = 2;
		((Control)buttonFieldMenu).TabStop = false;
		((ButtonBase)buttonFieldMenu).UseVisualStyleBackColor = true;
		((Control)buttonFieldMenu).Click += ButtonFieldMenu_Click;
		((Control)labelField).Dock = (DockStyle)1;
		((Control)labelField).ForeColor = Color.DimGray;
		((Control)labelField).Location = new Point(12, 94);
		((Control)labelField).Name = "labelField";
		((Control)labelField).Size = new Size(314, 32);
		((Control)labelField).TabIndex = 2;
		((Control)labelField).Text = "Field";
		labelField.TextAlign = (ContentAlignment)16;
		((Control)labelValue).Dock = (DockStyle)1;
		((Control)labelValue).Location = new Point(12, 12);
		((Control)labelValue).Margin = new Padding(2, 0, 2, 0);
		((Control)labelValue).Name = "labelValue";
		((Control)labelValue).Size = new Size(314, 32);
		((Control)labelValue).TabIndex = 0;
		((Control)labelValue).Text = "Value";
		((Control)panelCondition).BackColor = Color.White;
		((Control)panelCondition).Controls.Add((Control)(object)labelCondition);
		((Control)panelCondition).Controls.Add((Control)(object)comboBoxCondition);
		((Control)panelCondition).Dock = (DockStyle)2;
		((Control)panelCondition).Location = new Point(12, 268);
		((Control)panelCondition).Name = "panelCondition";
		((Control)panelCondition).Size = new Size(314, 38);
		((Control)panelCondition).TabIndex = 5;
		((Control)labelCondition).ForeColor = Color.DimGray;
		((Control)labelCondition).Location = new Point(0, 15);
		((Control)labelCondition).Margin = new Padding(2, 0, 2, 0);
		((Control)labelCondition).Name = "labelCondition";
		((Control)labelCondition).Size = new Size(121, 17);
		((Control)labelCondition).TabIndex = 3;
		((Control)labelCondition).Text = "Condition";
		labelCondition.TextAlign = (ContentAlignment)16;
		((Control)comboBoxCondition).Anchor = (AnchorStyles)13;
		comboBoxCondition.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxCondition).FormattingEnabled = true;
		((Control)comboBoxCondition).Location = new Point(126, 12);
		((Control)comboBoxCondition).Margin = new Padding(2);
		((Control)comboBoxCondition).Name = "comboBoxCondition";
		((Control)comboBoxCondition).Size = new Size(188, 21);
		((Control)comboBoxCondition).TabIndex = 0;
		((ComboBox)comboBoxCondition).SelectedIndexChanged += ComboBoxCondition_SelectedIndexChanged;
		((Control)panelInitialRangeSelection).BackColor = Color.White;
		((Control)panelInitialRangeSelection).Controls.Add((Control)(object)labelInitialRangeSelection);
		((Control)panelInitialRangeSelection).Controls.Add((Control)(object)comboBoxInitialRangeSelection);
		((Control)panelInitialRangeSelection).Dock = (DockStyle)2;
		((Control)panelInitialRangeSelection).Location = new Point(12, 344);
		((Control)panelInitialRangeSelection).Name = "panelInitialRangeSelection";
		((Control)panelInitialRangeSelection).Size = new Size(314, 38);
		((Control)panelInitialRangeSelection).TabIndex = 7;
		((Control)labelInitialRangeSelection).ForeColor = Color.DimGray;
		((Control)labelInitialRangeSelection).Location = new Point(0, 15);
		((Control)labelInitialRangeSelection).Margin = new Padding(2, 0, 2, 0);
		((Control)labelInitialRangeSelection).Name = "labelInitialRangeSelection";
		((Control)labelInitialRangeSelection).Size = new Size(121, 17);
		((Control)labelInitialRangeSelection).TabIndex = 3;
		((Control)labelInitialRangeSelection).Text = "Initial Selection";
		labelInitialRangeSelection.TextAlign = (ContentAlignment)16;
		((Control)comboBoxInitialRangeSelection).Anchor = (AnchorStyles)13;
		comboBoxInitialRangeSelection.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxInitialRangeSelection).FormattingEnabled = true;
		((Control)comboBoxInitialRangeSelection).Location = new Point(126, 12);
		((Control)comboBoxInitialRangeSelection).Margin = new Padding(2);
		((Control)comboBoxInitialRangeSelection).Name = "comboBoxInitialRangeSelection";
		((Control)comboBoxInitialRangeSelection).Size = new Size(188, 21);
		((Control)comboBoxInitialRangeSelection).TabIndex = 0;
		((ComboBox)comboBoxInitialRangeSelection).SelectedIndexChanged += ComboBoxInitialRangeSelection_SelectedIndexChanged;
		contextMenuBar.AntiAlias = true;
		contextMenuBar.DockTabStripHeight = 50;
		((Control)contextMenuBar).Font = new Font("Segoe UI", 9f);
		contextMenuBar.Items.AddRange(new BaseItem[1] { menuExpression });
		contextMenuBar.Location = new Point(226, 159);
		((Control)contextMenuBar).Margin = new Padding(1);
		contextMenuBar.Name = "contextMenuBar";
		contextMenuBar.Size = new Size(100, 31);
		contextMenuBar.Stretch = true;
		contextMenuBar.Style = eDotNetBarStyle.StyleManagerControlled;
		contextMenuBar.TabIndex = 13;
		contextMenuBar.TabStop = false;
		((Control)contextMenuBar).Text = "contextMenuBar";
		contextMenuBar.WrapItemsDock = true;
		menuExpression.AutoExpandOnClick = true;
		menuExpression.ImagePaddingHorizontal = 16;
		menuExpression.ImagePaddingVertical = 12;
		menuExpression.LockClosePopup = false;
		menuExpression.Name = "menuExpression";
		menuExpression.Text = "menuExpression";
		((Control)panelInitialRangeSelectionSource).BackColor = Color.White;
		((Control)panelInitialRangeSelectionSource).Controls.Add((Control)(object)labelInitialRangeSelectionSource);
		((Control)panelInitialRangeSelectionSource).Controls.Add((Control)(object)comboBoxInitialRangeSelectionSource);
		((Control)panelInitialRangeSelectionSource).Dock = (DockStyle)2;
		((Control)panelInitialRangeSelectionSource).Location = new Point(12, 306);
		((Control)panelInitialRangeSelectionSource).Name = "panelInitialRangeSelectionSource";
		((Control)panelInitialRangeSelectionSource).Size = new Size(314, 38);
		((Control)panelInitialRangeSelectionSource).TabIndex = 6;
		((Control)labelInitialRangeSelectionSource).ForeColor = Color.DimGray;
		((Control)labelInitialRangeSelectionSource).Location = new Point(0, 15);
		((Control)labelInitialRangeSelectionSource).Margin = new Padding(2, 0, 2, 0);
		((Control)labelInitialRangeSelectionSource).Name = "labelInitialRangeSelectionSource";
		((Control)labelInitialRangeSelectionSource).Size = new Size(121, 17);
		((Control)labelInitialRangeSelectionSource).TabIndex = 3;
		((Control)labelInitialRangeSelectionSource).Text = "Initial Selection Source";
		labelInitialRangeSelectionSource.TextAlign = (ContentAlignment)16;
		((Control)comboBoxInitialRangeSelectionSource).Anchor = (AnchorStyles)13;
		comboBoxInitialRangeSelectionSource.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxInitialRangeSelectionSource).FormattingEnabled = true;
		((Control)comboBoxInitialRangeSelectionSource).Location = new Point(126, 12);
		((Control)comboBoxInitialRangeSelectionSource).Margin = new Padding(2);
		((Control)comboBoxInitialRangeSelectionSource).Name = "comboBoxInitialRangeSelectionSource";
		((Control)comboBoxInitialRangeSelectionSource).Size = new Size(188, 21);
		((Control)comboBoxInitialRangeSelectionSource).TabIndex = 0;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(338, 394);
		((Control)this).Controls.Add((Control)(object)panelBoxField);
		((Control)this).Controls.Add((Control)(object)contextMenuBar);
		((Control)this).Controls.Add((Control)(object)labelField);
		((Control)this).Controls.Add((Control)(object)panelUi);
		((Control)this).Controls.Add((Control)(object)listBoxValues);
		((Control)this).Controls.Add((Control)(object)labelValue);
		((Control)this).Controls.Add((Control)(object)panelCondition);
		((Control)this).Controls.Add((Control)(object)panelInitialRangeSelectionSource);
		((Control)this).Controls.Add((Control)(object)panelInitialRangeSelection);
		((Form)this).Margin = new Padding(0);
		((Control)this).Name = "StiDatePickerElementEditorForm";
		((Control)this).Padding = new Padding(12);
		((Control)this).Text = "Date Picker";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Control)panelUi).ResumeLayout(false);
		((Control)panelBoxField).ResumeLayout(false);
		((Control)panelBoxField).PerformLayout();
		((Control)panelCondition).ResumeLayout(false);
		((Control)panelInitialRangeSelection).ResumeLayout(false);
		((ISupportInitialize)contextMenuBar).EndInit();
		((Control)panelInitialRangeSelectionSource).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
