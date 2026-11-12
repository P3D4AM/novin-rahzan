using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Design.Editors;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Dictionary.Design;
using Stimulsoft.Report.Help;
using Stimulsoft.Report.Helper;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Design.Dictionary;

public class StiVariableEditForm : StiFormUX, IStiVariableEditForm, IDisposable
{
	private string originalName;

	private StiDataSource selectedDataSource;

	private string baseName;

	private string oldValue;

	private bool lockRemoveItems = true;

	private int prevIndex = -1;

	private StiDataBuilder dataBuilder = new StiDataBuilder();

	private Image variableImage;

	private IContainer components;

	private StiTextBox textBoxVariableName;

	private StiComboBox comboBoxType;

	private StiTextBox textBoxVariableAlias;

	private ImageList imageListTypes;

	private StiCheckBoxUX checkBoxReadOnly;

	private StiLabelUX labelName;

	private StiLabelUX labelType;

	private StiLabelUX labelAlias;

	private ErrorProvider errorProvider;

	private StiButtonEdit buttonEditDefaultValue;

	private StiLabelUX labelDefaultDateTimeValue;

	private ControlContainerItem controlContainerItem2;

	private StiDateTimePickerUX dateTimePickerDefaultValue;

	private StiCheckBoxUX checkBoxNotAssigned;

	private StiTextBox textBoxDescription;

	private StiLabelUX labelDescription;

	private StiCheckBoxUX checkBoxRequestFromUser;

	private StiPanelUX panelInitByValue;

	private StiPanelUX panelInitByDateTime;

	private StiPanelUX panelMain;

	private StiPanelUX panelInitByExpression;

	private StiButtonUX buttonOk;

	private StiButtonUX buttonCancel;

	private StiGroupPanelUX panelSettings;

	private StiButtonPanelUX groupButtons;

	private StiLabelUX labelImage;

	private StiLabelUX labelDefaultValue;

	private StiLabelUX labelDefaultExpression;

	private StiComboBox comboBoxTypeMode;

	private StiButtonEdit textBoxInitByExpression;

	private StiLabelUX labelInitBy;

	private StiComboBox comboBoxInitBy;

	private StiGroupPanelUX panelInitByImage;

	private StiLabelUX labelExpressionSample2;

	private StiLabelUX labelExpressionSample;

	private StiLabelUX labelValueSample;

	private StiLabelUX labelValueSample2;

	private StiGroupPanelUX panelInitByBool;

	private StiCheckBoxUX checkBoxDefaultValue;

	private StiPanelUX panelInitByExpressionRange;

	private StiLabelUX labelExpressionSampleRange;

	private StiLabelUX labelExpressionSampleRange2;

	private StiButtonEdit textBoxInitByExpressionFrom;

	private StiLabelUX labelExpressionFrom;

	private StiPanelUX panelInitByValueRange;

	private StiLabelUX labelValueTo;

	private StiButtonEdit buttonEditDefaultValueTo;

	private StiLabelUX labelValueSampleRange;

	private StiLabelUX labelValueSampleRange2;

	private StiLabelUX labelValueFrom;

	private StiButtonEdit buttonEditDefaultValueFrom;

	private StiPanelUX panelInitByDateTimeRange;

	private StiCheckBoxUX checkBoxNotAssignedTo;

	private StiCheckBoxUX checkBoxNotAssignedFrom;

	private StiLabelUX labelDateTimeValueFrom;

	private StiDateTimePickerUX dateTimePickerDefaultValueTo;

	private StiDateTimePickerUX dateTimePickerDefaultValueFrom;

	private StiLabelUX labelExpressionTo;

	private StiLabelUX labelDateTimeValueTo;

	private ToolTip toolTip1;

	private StiPanelUX panelDialogInfoDateTime;

	private StiLabelUX labelDateTimeFormat;

	private StiComboBox comboBoxDateTimeFormat;

	private StiPanelUX panelDialogInfoMask;

	private StiLabelUX labelMask;

	private StiTextBox textBoxMask;

	private StiGroupPanelUX panelDialogItems;

	private StiGroupPanelUX panelInitBy;

	private StiLabelUX labelItems;

	private StiLabelUX labelDataSource;

	private TextBox textBoxItems;

	private StiComboBox comboBoxDataSource;

	private StiOperationButton buttonEditItems;

	private StiCheckBoxUX checkBoxAllowUserValues;

	private StiPanelUX panelColumns;

	private StiPanelUX panelItems;

	private StiButtonUX buttonSaveCopy;

	private StiPanelUX panelBindingColumns;

	private StiComboBox comboBoxBindingValue;

	private StiTreeViewBox columnBindingColumn;

	private StiLabelUX labelDependentColumn;

	private StiPanelUX panelBindingValue;

	private StiCheckBoxUX checkBoxBindingValue;

	private StiPanelUX panelKeysValues;

	private StiLabelUX labelLabels;

	private StiTreeViewBox columnKeys;

	private StiLabelUX labelKeys;

	private StiTreeViewBox columnValues;

	private StiLabelUX labelVariable;

	private StiCheckBoxUX checkBoxAllowUsingAsSqlParameter;

	private StiGroupPanelUX panelSelection;

	private StiLabelUX labelSelection;

	private StiComboBox comboBoxSelection;

	private StiOperationButton buttonImageRemove;

	private StiOperationButton buttonImageOpen;

	private StiPanelUX panelImage;

	private StiButtonEdit textBoxInitByExpressionTo;

	private StiComboBox comboBoxSortDirection;

	private StiComboBox comboBoxSortField;

	private StiLabelUX labelSortBy;

	private StiLabelUX labelChecked;

	private StiOperationButton buttonEditExpressionChecked;

	private StiTextBox textBoxCheckedExpression;

	private StiTreeViewBox treeViewBoxCheckedDataSource;

	private StiLabelUX labelCheckedSample;

	private StiLabelUX label2;

	private StiPanelUX panelInitByTimeSpanRange;

	private StiCheckBoxUX checkBoxTimeSpanToNotAssigned;

	private StiCheckBoxUX checkBoxTimeSpanFromNotAssigned;

	private StiLabelUX labelTimeSpanValueTo;

	private StiLabelUX labelTimeSpanValueFrom;

	private StiPanelUX panelInitByTimeSpan;

	private StiCheckBoxUX checkBoxTimeSpanNotAssigned;

	private StiLabelUX labelDefaultTimeSpanValue;

	private StiTimeSpanPicker timeSpanPickerDefaultValue;

	private StiTimeSpanPicker timeSpanPickerDefaultValueTo;

	private StiTimeSpanPicker timeSpanPickerDefaultValueFrom;

	private string NoColumn => "[" + Loc.Get("Report", "No") + "]";

	public bool IsVariableDefaultValueIgnored
	{
		get
		{
			if (VariableRequestFromUser)
			{
				return VariableSelectionMode != StiSelectionMode.FromVariable;
			}
			return false;
		}
	}

	public Image VariableImage
	{
		get
		{
			return variableImage;
		}
		set
		{
			variableImage = value;
			CheckStates();
			((Control)panelImage).Invalidate();
		}
	}

	public StiVariableInitBy VariableInitBy
	{
		get
		{
			if (((ListControl)comboBoxInitBy).SelectedIndex != 0)
			{
				return StiVariableInitBy.Expression;
			}
			return StiVariableInitBy.Value;
		}
		set
		{
			((ListControl)comboBoxInitBy).SelectedIndex = ((value != 0) ? 1 : 0);
			CheckStates();
		}
	}

	public Type VariableType
	{
		get
		{
			StiTypeMode typeMode = StiTypeMode.Value;
			switch (((ListControl)comboBoxTypeMode).SelectedIndex)
			{
			case 0:
				typeMode = StiTypeMode.Value;
				break;
			case 1:
				typeMode = StiTypeMode.NullableValue;
				break;
			case 2:
				typeMode = StiTypeMode.List;
				break;
			case 3:
				typeMode = StiTypeMode.Range;
				break;
			}
			if (((ComboBox)comboBoxType).SelectedItem == null)
			{
				return null;
			}
			return StiType.GetTypeFromTypeMode(((StiType)((ComboBox)comboBoxType).SelectedItem).Type, typeMode);
		}
		set
		{
			StiTypeMode typeMode;
			Type typeModeFromType = StiType.GetTypeModeFromType(value, out typeMode);
			switch (typeMode)
			{
			case StiTypeMode.Value:
				((ListControl)comboBoxTypeMode).SelectedIndex = 0;
				break;
			case StiTypeMode.NullableValue:
				((ListControl)comboBoxTypeMode).SelectedIndex = 1;
				break;
			case StiTypeMode.List:
				((ListControl)comboBoxTypeMode).SelectedIndex = 2;
				break;
			case StiTypeMode.Range:
				((ListControl)comboBoxTypeMode).SelectedIndex = 3;
				break;
			}
			foreach (StiType item in ((ComboBox)comboBoxType).Items)
			{
				if (item.Type == typeModeFromType)
				{
					((ComboBox)comboBoxType).SelectedItem = item;
					break;
				}
			}
		}
	}

	public StiItemsInitializationType VariableDataSourceInitializationType
	{
		get
		{
			if (((ListControl)comboBoxDataSource).SelectedIndex != 0)
			{
				return StiItemsInitializationType.Columns;
			}
			return StiItemsInitializationType.Items;
		}
		set
		{
			((ListControl)comboBoxDataSource).SelectedIndex = ((value == StiItemsInitializationType.Columns) ? 1 : 0);
			CheckStates();
		}
	}

	public StiSelectionMode VariableSelectionMode
	{
		get
		{
			return ((ListControl)comboBoxSelection).SelectedIndex switch
			{
				0 => StiSelectionMode.FromVariable, 
				1 => StiSelectionMode.Nothing, 
				_ => StiSelectionMode.First, 
			};
		}
		set
		{
			switch (value)
			{
			case StiSelectionMode.FromVariable:
				((ListControl)comboBoxSelection).SelectedIndex = 0;
				break;
			case StiSelectionMode.Nothing:
				((ListControl)comboBoxSelection).SelectedIndex = 1;
				break;
			case StiSelectionMode.First:
				((ListControl)comboBoxSelection).SelectedIndex = 2;
				break;
			}
			CheckStates();
		}
	}

	public StiDateTimeType VariableDateTimeType
	{
		get
		{
			if (((ListControl)comboBoxDateTimeFormat).SelectedIndex == 0)
			{
				return StiDateTimeType.DateAndTime;
			}
			if (((ListControl)comboBoxDateTimeFormat).SelectedIndex == 1)
			{
				return StiDateTimeType.Date;
			}
			return StiDateTimeType.Time;
		}
		set
		{
			switch (value)
			{
			case StiDateTimeType.DateAndTime:
				((ListControl)comboBoxDateTimeFormat).SelectedIndex = 0;
				break;
			case StiDateTimeType.Date:
				((ListControl)comboBoxDateTimeFormat).SelectedIndex = 1;
				break;
			case StiDateTimeType.Time:
				((ListControl)comboBoxDateTimeFormat).SelectedIndex = 2;
				break;
			}
		}
	}

	public StiVariable VariableBindingVariable
	{
		get
		{
			return ((ComboBox)comboBoxBindingValue).SelectedItem as StiVariable;
		}
		set
		{
			((ComboBox)comboBoxBindingValue).SelectedItem = value;
		}
	}

	public string VariableBindingColumn
	{
		get
		{
			return ((Control)columnBindingColumn).Text;
		}
		set
		{
			((Control)columnBindingColumn).Text = value;
		}
	}

	public string VariableKeysColumn
	{
		get
		{
			if (!(((Control)columnKeys).Text == NoColumn))
			{
				return ((Control)columnKeys).Text;
			}
			return "";
		}
		set
		{
			((Control)columnKeys).Text = (string.IsNullOrWhiteSpace(value) ? NoColumn : value);
		}
	}

	public string VariableValuesColumn
	{
		get
		{
			if (!(((Control)columnValues).Text == NoColumn))
			{
				return ((Control)columnValues).Text;
			}
			return "";
		}
		set
		{
			((Control)columnValues).Text = (string.IsNullOrWhiteSpace(value) ? NoColumn : value);
		}
	}

	public string VariableCheckedColumn
	{
		get
		{
			string text = ((((Control)treeViewBoxCheckedDataSource).Text == NoColumn) ? "" : ((Control)treeViewBoxCheckedDataSource).Text);
			if (string.IsNullOrEmpty(text))
			{
				return "";
			}
			return text + ";" + ((Control)textBoxCheckedExpression).Text;
		}
		set
		{
			if (value.Contains(";"))
			{
				int num = value.IndexOf(";");
				string text = value.Substring(0, num);
				selectedDataSource = Report.Dictionary.DataSources[text];
				((Control)treeViewBoxCheckedDataSource).Text = ((selectedDataSource == null) ? NoColumn : text);
				((Control)textBoxCheckedExpression).Text = value.Substring(num + 1);
			}
			else
			{
				((Control)treeViewBoxCheckedDataSource).Text = NoColumn;
				((Control)textBoxCheckedExpression).Text = value;
				selectedDataSource = null;
			}
		}
	}

	public string VariableName
	{
		get
		{
			return ((Control)textBoxVariableName).Text;
		}
		set
		{
			((Control)textBoxVariableName).Text = value;
		}
	}

	public string VariableAlias
	{
		get
		{
			return ((Control)textBoxVariableAlias).Text;
		}
		set
		{
			((Control)textBoxVariableAlias).Text = value;
		}
	}

	public string VariableDescription
	{
		get
		{
			return ((Control)textBoxDescription).Text;
		}
		set
		{
			((Control)textBoxDescription).Text = value;
		}
	}

	public bool VariableReadOnly
	{
		get
		{
			return ((CheckBox)checkBoxReadOnly).Checked;
		}
		set
		{
			((CheckBox)checkBoxReadOnly).Checked = value;
		}
	}

	public bool VariableRequestFromUser
	{
		get
		{
			return ((CheckBox)checkBoxRequestFromUser).Checked;
		}
		set
		{
			((CheckBox)checkBoxRequestFromUser).Checked = value;
		}
	}

	public bool VariableNotAssigned
	{
		get
		{
			return ((CheckBox)checkBoxNotAssigned).Checked;
		}
		set
		{
			((CheckBox)checkBoxNotAssigned).Checked = value;
		}
	}

	public bool VariableNotAssignedFrom
	{
		get
		{
			return ((CheckBox)checkBoxNotAssignedFrom).Checked;
		}
		set
		{
			((CheckBox)checkBoxNotAssignedFrom).Checked = value;
		}
	}

	public bool VariableNotAssignedTo
	{
		get
		{
			return ((CheckBox)checkBoxNotAssignedTo).Checked;
		}
		set
		{
			((CheckBox)checkBoxNotAssignedTo).Checked = value;
		}
	}

	public bool VariableTimeSpanNotAssigned
	{
		get
		{
			return ((CheckBox)checkBoxTimeSpanNotAssigned).Checked;
		}
		set
		{
			((CheckBox)checkBoxTimeSpanNotAssigned).Checked = value;
		}
	}

	public bool VariableTimeSpanNotAssignedFrom
	{
		get
		{
			return ((CheckBox)checkBoxTimeSpanFromNotAssigned).Checked;
		}
		set
		{
			((CheckBox)checkBoxTimeSpanFromNotAssigned).Checked = value;
		}
	}

	public bool VariableTimeSpanNotAssignedTo
	{
		get
		{
			return ((CheckBox)checkBoxTimeSpanToNotAssigned).Checked;
		}
		set
		{
			((CheckBox)checkBoxTimeSpanToNotAssigned).Checked = value;
		}
	}

	public bool VariableAllowUsingAsSqlParameter
	{
		get
		{
			return ((CheckBox)checkBoxAllowUsingAsSqlParameter).Checked;
		}
		set
		{
			((CheckBox)checkBoxAllowUsingAsSqlParameter).Checked = value;
		}
	}

	public bool VariableAllowUserValues
	{
		get
		{
			return ((CheckBox)checkBoxAllowUserValues).Checked;
		}
		set
		{
			((CheckBox)checkBoxAllowUserValues).Checked = value;
		}
	}

	public bool VariableBindingValue
	{
		get
		{
			return ((CheckBox)checkBoxBindingValue).Checked;
		}
		set
		{
			((CheckBox)checkBoxBindingValue).Checked = value;
		}
	}

	public string VariableMask
	{
		get
		{
			return ((Control)textBoxMask).Text;
		}
		set
		{
			((Control)textBoxMask).Text = value;
		}
	}

	public string VariableInitByExpression
	{
		get
		{
			return ((Control)textBoxInitByExpression).Text;
		}
		set
		{
			((Control)textBoxInitByExpression).Text = value;
		}
	}

	public string VariableInitByExpressionFrom
	{
		get
		{
			return ((Control)textBoxInitByExpressionFrom).Text;
		}
		set
		{
			((Control)textBoxInitByExpressionFrom).Text = value;
		}
	}

	public string VariableInitByExpressionTo
	{
		get
		{
			return ((Control)textBoxInitByExpressionTo).Text;
		}
		set
		{
			((Control)textBoxInitByExpressionTo).Text = value;
		}
	}

	public string VariableDefaultValue
	{
		get
		{
			if (!IsVariableDefaultValueIgnored)
			{
				return ((Control)buttonEditDefaultValue).Text;
			}
			return "";
		}
		set
		{
			((Control)buttonEditDefaultValue).Text = value;
		}
	}

	public string VariableDefaultValueFrom
	{
		get
		{
			if (!IsVariableDefaultValueIgnored)
			{
				return ((Control)buttonEditDefaultValueFrom).Text;
			}
			return "";
		}
		set
		{
			((Control)buttonEditDefaultValueFrom).Text = value;
		}
	}

	public string VariableDefaultValueTo
	{
		get
		{
			if (!IsVariableDefaultValueIgnored)
			{
				return ((Control)buttonEditDefaultValueTo).Text;
			}
			return "";
		}
		set
		{
			((Control)buttonEditDefaultValueTo).Text = value;
		}
	}

	public DateTime VariableDateTimeDefaultValue
	{
		get
		{
			return ((DateTimePicker)dateTimePickerDefaultValue).Value;
		}
		set
		{
			((DateTimePicker)dateTimePickerDefaultValue).Value = value;
		}
	}

	public DateTime VariableDateTimeDefaultValueFrom
	{
		get
		{
			return ((DateTimePicker)dateTimePickerDefaultValueFrom).Value;
		}
		set
		{
			((DateTimePicker)dateTimePickerDefaultValueFrom).Value = value;
		}
	}

	public DateTime VariableDateTimeDefaultValueTo
	{
		get
		{
			return ((DateTimePicker)dateTimePickerDefaultValueTo).Value;
		}
		set
		{
			((DateTimePicker)dateTimePickerDefaultValueTo).Value = value;
		}
	}

	public TimeSpan VariableTimeSpanDefaultValue
	{
		get
		{
			return timeSpanPickerDefaultValue.Value;
		}
		set
		{
			timeSpanPickerDefaultValue.Value = value;
		}
	}

	public TimeSpan VariableTimeSpanDefaultValueFrom
	{
		get
		{
			return timeSpanPickerDefaultValueFrom.Value;
		}
		set
		{
			timeSpanPickerDefaultValueFrom.Value = value;
		}
	}

	public TimeSpan VariableTimeSpanDefaultValueTo
	{
		get
		{
			return timeSpanPickerDefaultValueTo.Value;
		}
		set
		{
			timeSpanPickerDefaultValueTo.Value = value;
		}
	}

	public bool VariableBoolDefaultValue
	{
		get
		{
			return ((CheckBox)checkBoxDefaultValue).Checked;
		}
		set
		{
			((CheckBox)checkBoxDefaultValue).Checked = value;
		}
	}

	public StiVariableSortDirection VariableSortDirection
	{
		get
		{
			if (VariableDataSourceInitializationType == StiItemsInitializationType.Items)
			{
				return StiVariableSortDirection.None;
			}
			return ((ListControl)comboBoxSortDirection).SelectedIndex switch
			{
				0 => StiVariableSortDirection.None, 
				1 => StiVariableSortDirection.Asc, 
				_ => StiVariableSortDirection.Desc, 
			};
		}
		set
		{
			switch (value)
			{
			case StiVariableSortDirection.None:
				((ListControl)comboBoxSortDirection).SelectedIndex = 0;
				break;
			case StiVariableSortDirection.Asc:
				((ListControl)comboBoxSortDirection).SelectedIndex = 1;
				break;
			case StiVariableSortDirection.Desc:
				((ListControl)comboBoxSortDirection).SelectedIndex = 2;
				break;
			}
			CheckStates();
		}
	}

	public StiVariableSortField VariableSortField
	{
		get
		{
			if (((ListControl)comboBoxSortField).SelectedIndex == 0)
			{
				return StiVariableSortField.Key;
			}
			return StiVariableSortField.Label;
		}
		set
		{
			switch (value)
			{
			case StiVariableSortField.Key:
				((ListControl)comboBoxSortField).SelectedIndex = 0;
				break;
			case StiVariableSortField.Label:
				((ListControl)comboBoxSortField).SelectedIndex = 1;
				break;
			}
			CheckStates();
		}
	}

	public List<StiDialogInfoItem> Items { get; set; } = new List<StiDialogInfoItem>();


	public StiReport Report { get; set; }

	public bool CopyModeActivated { get; set; }

	public StiVariableEditForm(StiReport report, StiVariable variable, bool isEditMode)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		DoTheming();
		DoScaling();
		((ScrollableControl)this).AutoScroll = false;
		if (((Component)(object)this).DesignMode)
		{
			return;
		}
		if (StiOptions.Appearance.ShowFormsHelp)
		{
			((Form)this).HelpButton = true;
		}
		Report = report;
		originalName = variable?.Name;
		Localize();
		((Form)this).ShowIcon = false;
		StiTypesCollection baseTypes = StiType.GetBaseTypes();
		ObjectCollection items = ((ComboBox)comboBoxType).Items;
		object[] items2 = baseTypes.Items;
		items.AddRange(items2);
		((ListControl)comboBoxInitBy).SelectedIndex = 0;
		((ListControl)comboBoxDateTimeFormat).SelectedIndex = 0;
		((ListControl)comboBoxDataSource).SelectedIndex = 0;
		((ListControl)comboBoxSelection).SelectedIndex = 0;
		((ListControl)comboBoxSortDirection).SelectedIndex = 0;
		((ListControl)comboBoxSortField).SelectedIndex = 1;
		treeViewBoxCheckedDataSource.DropDownWidth = StiScale.I(300);
		treeViewBoxCheckedDataSource.MaxDropDownHeight = StiScale.I(300);
		foreach (StiVariable variable2 in report.Dictionary.Variables)
		{
			if (!variable2.Equals(variable) && !variable2.IsCategory && (variable2.DialogInfo == null || variable2.DialogInfo.BindingVariable == null || !variable2.DialogInfo.BindingVariable.Equals(variable)))
			{
				((ComboBox)comboBoxBindingValue).Items.Add((object)variable2);
			}
		}
		CheckStates();
		((Control)buttonSaveCopy).Visible = isEditMode;
		VariableImage = null;
	}

	private void Localize()
	{
		((Control)labelDataSource).Text = StiLocalization.Get("PropertyMain", "DataSource");
		((Control)labelItems).Text = StiLocalization.Get("PropertyMain", "Items");
		((Control)labelKeys).Text = StiLocalization.Get("PropertyMain", "Keys");
		((Control)labelLabels).Text = StiLocalization.Get("PropertyMain", "Labels");
		((Control)labelChecked).Text = StiLocalization.Get("PropertyMain", "Checked");
		((Control)labelSortBy).Text = StiLocalization.Get("FormBand", "SortBy");
		((Control)checkBoxAllowUserValues).Text = StiLocalization.Get("PropertyMain", "AllowUserValues");
		((ComboBox)comboBoxSortDirection).BeginUpdate();
		((ComboBox)comboBoxSortDirection).Items.Clear();
		((ComboBox)comboBoxSortDirection).Items.Add((object)StiLocalization.Get("FormBand", "NoSort"));
		((ComboBox)comboBoxSortDirection).Items.Add((object)StiLocalization.Get("FormBand", "Ascending"));
		((ComboBox)comboBoxSortDirection).Items.Add((object)StiLocalization.Get("FormBand", "Descending"));
		((ComboBox)comboBoxSortDirection).EndUpdate();
		((ComboBox)comboBoxSortField).BeginUpdate();
		((ComboBox)comboBoxSortField).Items.Clear();
		((ComboBox)comboBoxSortField).Items.Add((object)StiLocalization.Get("PropertyMain", "Key"));
		((ComboBox)comboBoxSortField).Items.Add((object)StiLocalization.Get("PropertyMain", "Label"));
		((ComboBox)comboBoxSortField).EndUpdate();
		((ComboBox)comboBoxDataSource).BeginUpdate();
		((ComboBox)comboBoxDataSource).Items.Clear();
		((ComboBox)comboBoxDataSource).Items.Add((object)StiLocalization.Get("PropertyMain", "Items"));
		((ComboBox)comboBoxDataSource).Items.Add((object)StiLocalization.Get("Chart", "DataColumns"));
		((ComboBox)comboBoxDataSource).EndUpdate();
		((ComboBox)comboBoxSelection).BeginUpdate();
		((ComboBox)comboBoxSelection).Items.Clear();
		((ComboBox)comboBoxSelection).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiSelectionModeFromVariable"));
		((ComboBox)comboBoxSelection).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiSelectionModeNothing"));
		((ComboBox)comboBoxSelection).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiSelectionModeFirst"));
		((ComboBox)comboBoxSelection).EndUpdate();
		((Control)labelName).Text = Loc.GetCleaned("Report", "LabelName");
		((Control)labelAlias).Text = Loc.GetCleaned("Report", "LabelAlias");
		((Control)labelDescription).Text = Loc.GetCleaned("PropertyMain", "Description");
		((Control)labelType).Text = Loc.GetCleaned("Report", "LabelType");
		((Control)labelImage).Text = Loc.GetCleaned("PropertyMain", "Image");
		((Control)labelDefaultDateTimeValue).Text = Loc.GetCleaned("Report", "LabelValue");
		((Control)labelDateTimeValueFrom).Text = Loc.GetCleaned("PropertyMain", "RangeFrom");
		((Control)labelDateTimeValueTo).Text = Loc.GetCleaned("PropertyMain", "RangeTo");
		((Control)labelDefaultValue).Text = Loc.GetCleaned("Report", "LabelValue");
		((Control)labelTimeSpanValueFrom).Text = Loc.GetCleaned("PropertyMain", "RangeFrom");
		((Control)labelTimeSpanValueTo).Text = Loc.GetCleaned("PropertyMain", "RangeTo");
		((Control)labelDefaultTimeSpanValue).Text = Loc.GetCleaned("Report", "LabelValue");
		((Control)labelValueFrom).Text = Loc.GetCleaned("PropertyMain", "RangeFrom");
		((Control)labelValueTo).Text = Loc.GetCleaned("PropertyMain", "RangeTo");
		((Control)labelDefaultExpression).Text = Loc.GetCleaned("Report", "LabelExpression");
		((Control)labelExpressionFrom).Text = Loc.GetCleaned("PropertyMain", "RangeFrom");
		((Control)labelExpressionTo).Text = Loc.GetCleaned("PropertyMain", "RangeTo");
		((Control)labelInitBy).Text = Loc.GetCleaned("PropertyMain", "InitBy");
		((Control)checkBoxReadOnly).Text = StiLocalization.Get("PropertyMain", "ReadOnly");
		((Control)checkBoxDefaultValue).Text = Loc.GetCleaned("Report", "LabelDefaultValue");
		((Control)checkBoxAllowUsingAsSqlParameter).Text = StiLocalization.Get("PropertyMain", "AllowUsingAsSqlParameter");
		((Control)checkBoxNotAssigned).Text = StiLocalization.Get("Report", "NotAssigned");
		((Control)checkBoxNotAssignedFrom).Text = StiLocalization.Get("Report", "NotAssigned");
		((Control)checkBoxNotAssignedTo).Text = StiLocalization.Get("Report", "NotAssigned");
		((Control)checkBoxTimeSpanNotAssigned).Text = StiLocalization.Get("Report", "NotAssigned");
		((Control)checkBoxTimeSpanFromNotAssigned).Text = StiLocalization.Get("Report", "NotAssigned");
		((Control)checkBoxTimeSpanToNotAssigned).Text = StiLocalization.Get("Report", "NotAssigned");
		((Control)checkBoxRequestFromUser).Text = StiLocalization.Get("PropertyMain", "RequestFromUser");
		((Control)checkBoxBindingValue).Text = StiLocalization.Get("PropertyMain", "DependentValue");
		((Control)labelSelection).Text = Loc.GetCleaned("PropertyMain", "Selection");
		((Control)labelExpressionSample).Text = Loc.Get("FormFormatEditor", "Sample") + ":";
		((Control)labelExpressionSampleRange).Text = Loc.Get("FormFormatEditor", "Sample") + ":";
		((Control)labelValueSample).Text = Loc.Get("FormFormatEditor", "Sample") + ":";
		((Control)labelValueSampleRange).Text = Loc.Get("FormFormatEditor", "Sample") + ":";
		((Control)labelCheckedSample).Text = Loc.Get("FormFormatEditor", "Sample") + ":";
		((Control)labelDateTimeFormat).Text = Loc.GetCleaned("FormFormatEditor", "DateTimeFormat");
		((Control)labelMask).Text = Loc.GetCleaned("FormFormatEditor", "FormatMask");
		int width = TextRenderer.MeasureText(((Control)labelValueSample).Text, ((Control)labelValueSample).Font).Width;
		StiLabelUX stiLabelUX = labelExpressionSample;
		StiLabelUX stiLabelUX2 = labelExpressionSampleRange;
		StiLabelUX stiLabelUX3 = labelValueSample;
		int num2 = (((Control)labelValueSampleRange).Width = width);
		int num4 = (((Control)stiLabelUX3).Width = num2);
		int width2 = (((Control)stiLabelUX2).Width = num4);
		((Control)stiLabelUX).Width = width2;
		((Control)labelExpressionSample2).Left = ((Control)labelExpressionSample).Right + StiScale.I4;
		((Control)labelExpressionSampleRange2).Left = ((Control)labelExpressionSampleRange).Right + StiScale.I4;
		((Control)labelValueSample2).Left = ((Control)labelValueSample).Right + StiScale.I4;
		((Control)labelValueSampleRange2).Left = ((Control)labelValueSampleRange).Right + StiScale.I4;
		((Control)labelVariable).Text = StiLocalization.Get("PropertyMain", "Variable");
		((Control)labelDependentColumn).Text = StiLocalization.Get("PropertyMain", "DependentColumn");
		((ComboBox)comboBoxInitBy).BeginUpdate();
		((ComboBox)comboBoxInitBy).Items.Clear();
		((ComboBox)comboBoxInitBy).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterItemValue"));
		((ComboBox)comboBoxInitBy).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterItemExpression"));
		((ComboBox)comboBoxInitBy).EndUpdate();
		((ComboBox)comboBoxTypeMode).BeginUpdate();
		((ComboBox)comboBoxTypeMode).Items.Clear();
		((ComboBox)comboBoxTypeMode).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiTypeModeValue"));
		((ComboBox)comboBoxTypeMode).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiTypeModeNullableValue"));
		((ComboBox)comboBoxTypeMode).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiTypeModeList"));
		((ComboBox)comboBoxTypeMode).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiTypeModeRange"));
		((ComboBox)comboBoxTypeMode).EndUpdate();
		((ComboBox)comboBoxDateTimeFormat).BeginUpdate();
		((ComboBox)comboBoxDateTimeFormat).Items.Clear();
		((ComboBox)comboBoxDateTimeFormat).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiDateTimeTypeDateAndTime"));
		((ComboBox)comboBoxDateTimeFormat).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiDateTimeTypeDate"));
		((ComboBox)comboBoxDateTimeFormat).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiDateTimeTypeTime"));
		((ComboBox)comboBoxDateTimeFormat).EndUpdate();
		((Control)buttonSaveCopy).Text = StiLocalization.Get("Buttons", "SaveCopy");
		((Control)buttonOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)buttonCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		toolTip1.SetToolTip((Control)(object)buttonEditItems, StiLocalization.Get("FormViewer", "Editor"));
		toolTip1.SetToolTip((Control)(object)buttonEditExpressionChecked, StiLocalization.Get("FormViewer", "Editor"));
		toolTip1.SetToolTip((Control)(object)buttonImageOpen, StiLocalization.Get("Buttons", "Open"));
		toolTip1.SetToolTip((Control)(object)buttonImageRemove, StiLocalization.Get("Buttons", "Remove"));
		StiButtonSizeHelper.CheckSizeOfButton((Button)(object)buttonSaveCopy);
	}

	private void DoTheming()
	{
		if (StiUX.IsDark)
		{
			((Control)textBoxItems).BackColor = StiUX.BackgroundDark;
			((Control)textBoxItems).ForeColor = StiUX.InputForeground;
		}
	}

	private void DoScaling()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		double step = StiScale.Step;
		if (step >= 1.5)
		{
			string stepName = StiScale.StepName;
			imageListTypes = new ImageList();
			imageListTypes.TransparentColor = Color.Transparent;
			Bitmap image = StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.VariableString" + stepName + ".png");
			imageListTypes.ImageSize = ((Image)image).Size;
			imageListTypes.Images.Add("String", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.VariableString" + stepName + ".png"));
			imageListTypes.Images.Add("Binary", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.VariableBinary" + stepName + ".png"));
			imageListTypes.Images.Add("Bool", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.VariableBool" + stepName + ".png"));
			imageListTypes.Images.Add("Char", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.VariableChar" + stepName + ".png"));
			imageListTypes.Images.Add("DateTime", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.VariableDateTime" + stepName + ".png"));
			imageListTypes.Images.Add("Decimal", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.VariableDecimal" + stepName + ".png"));
			imageListTypes.Images.Add("Float", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.VariableFloat" + stepName + ".png"));
			imageListTypes.Images.Add("Image", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.VariableImage" + stepName + ".png"));
			imageListTypes.Images.Add("Int", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.VariableInt" + stepName + ".png"));
		}
		buttonEditDefaultValue.Image = StiReportDesignImages.Actions.EditSmall();
		buttonEditDefaultValueFrom.Image = StiReportDesignImages.Actions.EditSmall();
		buttonEditDefaultValueTo.Image = StiReportDesignImages.Actions.EditSmall();
		textBoxInitByExpression.Image = StiReportDesignImages.Actions.EditSmall();
		textBoxInitByExpressionFrom.Image = StiReportDesignImages.Actions.EditSmall();
		textBoxInitByExpressionTo.Image = StiReportDesignImages.Actions.EditSmall();
		((ButtonBase)buttonEditItems).Image = StiReportDesignImages.Actions.Edit();
		((ButtonBase)buttonEditExpressionChecked).Image = StiReportDesignImages.Actions.Edit();
		((ButtonBase)buttonImageRemove).Image = StiReportDesignImages.Actions.Remove();
		((ButtonBase)buttonImageOpen).Image = StiReportDesignImages.Actions.OpenFile();
		((ComboBox)comboBoxType).ItemHeight = (int)Math.Ceiling(16.0 * StiScale.Y);
		((ComboBox)comboBoxTypeMode).ItemHeight = (int)Math.Ceiling(16.0 * StiScale.Y);
		StiTreeViewBox stiTreeViewBox = columnKeys;
		StiTreeViewBox stiTreeViewBox2 = columnValues;
		bool flag2 = (columnBindingColumn.FixedHeight = false);
		bool fixedHeight = (stiTreeViewBox2.FixedHeight = flag2);
		stiTreeViewBox.FixedHeight = fixedHeight;
		StiTreeViewBox stiTreeViewBox3 = columnKeys;
		StiTreeViewBox stiTreeViewBox4 = columnValues;
		int num = (((Control)columnBindingColumn).Height = ((Control)comboBoxDataSource).Height);
		int height2 = (((Control)stiTreeViewBox4).Height = num);
		((Control)stiTreeViewBox3).Height = height2;
		((Control)comboBoxBindingValue).Width = ((Control)columnBindingColumn).Width;
		((Control)comboBoxTypeMode).Width = ((Control)textBoxDescription).Right - ((Control)comboBoxTypeMode).Left;
		if (Loc.IsRussian)
		{
			((Control)labelDateTimeFormat).Font = StiFontUtils.ChangeFontSize(((Control)labelDateTimeFormat).Font, ((Control)labelDateTimeFormat).Font.Size - 0.5f);
		}
		StiLabelUX stiLabelUX = labelDefaultDateTimeValue;
		height2 = (((Control)checkBoxNotAssigned).Height = ((Control)dateTimePickerDefaultValue).Height);
		((Control)stiLabelUX).Height = height2;
		StiLabelUX stiLabelUX2 = labelDefaultDateTimeValue;
		height2 = (((Control)checkBoxNotAssigned).Top = ((Control)dateTimePickerDefaultValue).Top);
		((Control)stiLabelUX2).Top = height2;
		StiLabelUX stiLabelUX3 = labelDateTimeValueFrom;
		height2 = (((Control)checkBoxNotAssignedFrom).Height = ((Control)dateTimePickerDefaultValueFrom).Height);
		((Control)stiLabelUX3).Height = height2;
		StiLabelUX stiLabelUX4 = labelDateTimeValueFrom;
		height2 = (((Control)checkBoxNotAssignedFrom).Top = ((Control)dateTimePickerDefaultValueFrom).Top);
		((Control)stiLabelUX4).Top = height2;
		StiLabelUX stiLabelUX5 = labelDateTimeValueTo;
		height2 = (((Control)checkBoxNotAssignedTo).Height = ((Control)dateTimePickerDefaultValueTo).Height);
		((Control)stiLabelUX5).Height = height2;
		StiLabelUX stiLabelUX6 = labelDateTimeValueTo;
		height2 = (((Control)checkBoxNotAssignedTo).Top = ((Control)dateTimePickerDefaultValueTo).Top);
		((Control)stiLabelUX6).Top = height2;
		errorProvider.Icon = StiIconUtils.ResizeIcon(errorProvider.Icon, StiScale.I16, StiScale.I16);
		((Control)treeViewBoxCheckedDataSource).Width = ((Control)comboBoxSortDirection).Width;
	}

	public List<StiVariable> GetBindingVariables()
	{
		return ((IEnumerable)((ComboBox)comboBoxBindingValue).Items).Cast<StiVariable>().ToList();
	}

	public void SetBaseName(string name)
	{
		StiTextBox stiTextBox = textBoxVariableName;
		string text2 = (((Control)textBoxVariableAlias).Text = name);
		string text4 = (((Control)stiTextBox).Text = text2);
		baseName = text4;
	}

	private void UpdateNameAndAlias(string name)
	{
		if (baseName == ((Control)textBoxVariableName).Text && baseName == ((Control)textBoxVariableAlias).Text)
		{
			SetBaseName(name);
		}
	}

	private bool SelectNode(string[] strs, int index, TreeNodeCollection nodes, TreeView view)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		if (strs.Length <= index)
		{
			return false;
		}
		string text = strs[index];
		foreach (TreeNode node in nodes)
		{
			TreeNode val = node;
			string str = ((!(val.Tag is StiDataRelation)) ? val.Text : ((StiDataRelation)val.Tag).Name);
			str = StiNameValidator.CorrectName(str, Report);
			if (!(str == text))
			{
				continue;
			}
			if (val.Nodes.Count == 0)
			{
				if (strs.Length - 1 != index)
				{
					return false;
				}
				view.SelectedNode = val;
				return true;
			}
			if (val.Nodes.Count == 1 && val.Nodes[0].Text == "Loading")
			{
				ExpandTreeNode(val);
				val.Expand();
			}
			return SelectNode(strs, index + 1, val.Nodes, view);
		}
		return false;
	}

	private void ExpandTreeNode(TreeNode node)
	{
		if (node is TreeNodeEx node2 && node.Tag != null)
		{
			dataBuilder.ExpandedNode(node2, includeCalcColumn: true);
		}
	}

	private void CheckDefault()
	{
		if (((ListControl)comboBoxInitBy).SelectedIndex == 0 && ((Control)buttonEditDefaultValueFrom).Text.Length == 0 && ((Control)buttonEditDefaultValueTo).Text.Length == 0 && (VariableType == typeof(ByteRange) || VariableType == typeof(ShortRange) || VariableType == typeof(IntRange) || VariableType == typeof(LongRange) || VariableType == typeof(FloatRange) || VariableType == typeof(DoubleRange) || VariableType == typeof(DecimalRange)))
		{
			((Control)buttonEditDefaultValueFrom).Text = "0";
			((Control)buttonEditDefaultValueTo).Text = "0";
		}
	}

	internal void CheckStates()
	{
		StiType stiType = ((ComboBox)comboBoxType).SelectedItem as StiType;
		StiComboBox stiComboBox = comboBoxTypeMode;
		bool visible = (((Control)panelSettings).Visible = stiType == null || (!(stiType.Type == typeof(object)) && !(stiType.Type == typeof(byte[]))));
		((Control)stiComboBox).Visible = visible;
		((Control)buttonImageRemove).Enabled = variableImage != null;
		bool visible2;
		if (stiType == null)
		{
			((Control)panelInitByTimeSpan).Visible = false;
			((Control)panelInitByDateTime).Visible = false;
			((Control)panelInitByValue).Visible = false;
			((Control)panelInitByExpression).Visible = false;
			((Control)panelInitByImage).Visible = false;
			((Control)panelInitByBool).Visible = false;
			((Control)panelInitBy).Visible = false;
			((Control)panelInitByTimeSpanRange).Visible = false;
			((Control)panelInitByValueRange).Visible = false;
			((Control)panelInitByExpressionRange).Visible = false;
			((Control)panelDialogInfoDateTime).Visible = false;
			((Control)panelDialogInfoMask).Visible = false;
			visible2 = false;
			((Control)panelSelection).Visible = false;
		}
		else
		{
			if (((ListControl)comboBoxTypeMode).SelectedIndex == 0 || ((ListControl)comboBoxTypeMode).SelectedIndex == 1)
			{
				if (stiType.Type == typeof(Image))
				{
					((Control)panelInitByTimeSpan).Visible = false;
					((Control)panelInitByDateTime).Visible = false;
					((Control)panelInitByValue).Visible = false;
					((Control)panelInitByExpression).Visible = false;
					((Control)panelInitByImage).Visible = true;
					((Control)panelInitByBool).Visible = false;
					((Control)panelInitBy).Visible = false;
					((Control)panelInitByTimeSpanRange).Visible = false;
					((Control)panelInitByDateTimeRange).Visible = false;
					((Control)panelInitByValueRange).Visible = false;
					((Control)panelInitByExpressionRange).Visible = false;
				}
				else if (VariableInitBy == StiVariableInitBy.Value)
				{
					bool flag2 = stiType.Type == typeof(DateTime);
					bool flag3 = stiType.Type == typeof(TimeSpan);
					if (stiType.Type == typeof(DateTime?))
					{
						flag2 = true;
					}
					if (stiType.Type == typeof(TimeSpan?))
					{
						flag3 = true;
					}
					if (flag2)
					{
						((DateTimePicker)dateTimePickerDefaultValue).Format = (DateTimePickerFormat)8;
						((DateTimePicker)dateTimePickerDefaultValue).CustomFormat = GetVariableDateTimeFormat(VariableDateTimeType);
						((Control)panelInitByTimeSpan).Visible = false;
						((Control)panelInitByDateTime).Visible = true;
						((Control)panelInitByValue).Visible = false;
						((Control)panelInitByExpression).Visible = false;
						((Control)panelInitByImage).Visible = false;
						((Control)panelInitByBool).Visible = false;
						((Control)panelInitBy).Visible = true;
						((Control)panelInitByTimeSpanRange).Visible = false;
						((Control)panelInitByDateTimeRange).Visible = false;
						((Control)panelInitByValueRange).Visible = false;
						((Control)panelInitByExpressionRange).Visible = false;
					}
					else if (flag3)
					{
						((Control)panelInitByTimeSpan).Visible = true;
						((Control)panelInitByDateTime).Visible = false;
						((Control)panelInitByValue).Visible = false;
						((Control)panelInitByExpression).Visible = false;
						((Control)panelInitByImage).Visible = false;
						((Control)panelInitByBool).Visible = false;
						((Control)panelInitBy).Visible = true;
						((Control)panelInitByTimeSpanRange).Visible = false;
						((Control)panelInitByDateTimeRange).Visible = false;
						((Control)panelInitByValueRange).Visible = false;
						((Control)panelInitByExpressionRange).Visible = false;
					}
					else if (stiType.Type == typeof(bool))
					{
						((Control)panelInitByTimeSpan).Visible = false;
						((Control)panelInitByDateTime).Visible = false;
						((Control)panelInitByValue).Visible = false;
						((Control)panelInitByExpression).Visible = false;
						((Control)panelInitByImage).Visible = false;
						((Control)panelInitByBool).Visible = true;
						((Control)panelInitBy).Visible = true;
						((Control)panelInitByTimeSpanRange).Visible = false;
						((Control)panelInitByDateTimeRange).Visible = false;
						((Control)panelInitByValueRange).Visible = false;
						((Control)panelInitByExpressionRange).Visible = false;
					}
					else
					{
						((Control)panelInitByTimeSpan).Visible = false;
						((Control)panelInitByDateTime).Visible = false;
						((Control)panelInitByValue).Visible = true;
						((Control)panelInitByExpression).Visible = false;
						((Control)panelInitByImage).Visible = false;
						((Control)panelInitByBool).Visible = false;
						((Control)panelInitBy).Visible = true;
						((Control)panelInitByTimeSpanRange).Visible = false;
						((Control)panelInitByDateTimeRange).Visible = false;
						((Control)panelInitByValueRange).Visible = false;
						((Control)panelInitByExpressionRange).Visible = false;
					}
				}
				else
				{
					((Control)panelInitByTimeSpan).Visible = false;
					((Control)panelInitByDateTime).Visible = false;
					((Control)panelInitByValue).Visible = false;
					((Control)panelInitByExpression).Visible = true;
					((Control)panelInitByImage).Visible = false;
					((Control)panelInitByBool).Visible = false;
					if (stiType != null && stiType.Type == typeof(Image))
					{
						((Control)panelInitBy).Visible = false;
					}
					else
					{
						((Control)panelInitBy).Visible = true;
					}
					((Control)panelInitByTimeSpanRange).Visible = false;
					((Control)panelInitByDateTimeRange).Visible = false;
					((Control)panelInitByValueRange).Visible = false;
					((Control)panelInitByExpressionRange).Visible = false;
				}
				((Control)checkBoxAllowUsingAsSqlParameter).Enabled = true;
			}
			else if (((ListControl)comboBoxTypeMode).SelectedIndex == 3)
			{
				if (VariableInitBy == StiVariableInitBy.Value)
				{
					bool flag4 = stiType.Type == typeof(DateTime);
					bool flag5 = stiType.Type == typeof(TimeSpan);
					if (stiType.Type == typeof(DateTime?))
					{
						flag4 = true;
					}
					if (stiType.Type == typeof(TimeSpan?))
					{
						flag5 = true;
					}
					if (flag4)
					{
						((DateTimePicker)dateTimePickerDefaultValueFrom).Format = (DateTimePickerFormat)8;
						((DateTimePicker)dateTimePickerDefaultValueTo).Format = (DateTimePickerFormat)8;
						StiDateTimePickerUX stiDateTimePickerUX = dateTimePickerDefaultValueFrom;
						string customFormat = (((DateTimePicker)dateTimePickerDefaultValueTo).CustomFormat = GetVariableDateTimeFormat(VariableDateTimeType));
						((DateTimePicker)stiDateTimePickerUX).CustomFormat = customFormat;
						((Control)panelInitByTimeSpan).Visible = false;
						((Control)panelInitByDateTime).Visible = false;
						((Control)panelInitByValue).Visible = false;
						((Control)panelInitByExpression).Visible = false;
						((Control)panelInitByImage).Visible = false;
						((Control)panelInitByBool).Visible = false;
						((Control)panelInitBy).Visible = true;
						((Control)panelInitByTimeSpanRange).Visible = false;
						((Control)panelInitByDateTimeRange).Visible = true;
						((Control)panelInitByValueRange).Visible = false;
						((Control)panelInitByExpressionRange).Visible = false;
					}
					else if (flag5)
					{
						((Control)panelInitByTimeSpan).Visible = false;
						((Control)panelInitByDateTime).Visible = false;
						((Control)panelInitByValue).Visible = false;
						((Control)panelInitByExpression).Visible = false;
						((Control)panelInitByImage).Visible = false;
						((Control)panelInitByBool).Visible = false;
						((Control)panelInitBy).Visible = true;
						((Control)panelInitByTimeSpanRange).Visible = true;
						((Control)panelInitByDateTimeRange).Visible = false;
						((Control)panelInitByValueRange).Visible = false;
						((Control)panelInitByExpressionRange).Visible = false;
					}
					else
					{
						((Control)panelInitByTimeSpan).Visible = false;
						((Control)panelInitByDateTime).Visible = false;
						((Control)panelInitByValue).Visible = false;
						((Control)panelInitByExpression).Visible = false;
						((Control)panelInitByImage).Visible = false;
						((Control)panelInitByBool).Visible = false;
						((Control)panelInitBy).Visible = true;
						((Control)panelInitByTimeSpanRange).Visible = false;
						((Control)panelInitByDateTimeRange).Visible = false;
						((Control)panelInitByValueRange).Visible = true;
						((Control)panelInitByExpressionRange).Visible = false;
					}
				}
				else
				{
					((Control)panelInitByTimeSpan).Visible = false;
					((Control)panelInitByDateTime).Visible = false;
					((Control)panelInitByValue).Visible = false;
					((Control)panelInitByExpression).Visible = false;
					((Control)panelInitByImage).Visible = false;
					((Control)panelInitByBool).Visible = false;
					((Control)panelInitBy).Visible = true;
					((Control)panelInitByTimeSpanRange).Visible = false;
					((Control)panelInitByDateTimeRange).Visible = false;
					((Control)panelInitByValueRange).Visible = false;
					((Control)panelInitByExpressionRange).Visible = true;
				}
				((Control)checkBoxAllowUsingAsSqlParameter).Enabled = false;
				((CheckBox)checkBoxAllowUsingAsSqlParameter).Checked = false;
			}
			else
			{
				((Control)panelInitByTimeSpan).Visible = false;
				((Control)panelInitByDateTime).Visible = false;
				((Control)panelInitByValue).Visible = false;
				((Control)panelInitByExpression).Visible = false;
				((Control)panelInitByImage).Visible = false;
				((Control)panelInitByBool).Visible = false;
				((Control)panelInitBy).Visible = false;
				((Control)panelInitByTimeSpanRange).Visible = false;
				((Control)panelInitByDateTimeRange).Visible = false;
				((Control)panelInitByValueRange).Visible = false;
				((Control)panelInitByExpressionRange).Visible = false;
				((Control)checkBoxAllowUsingAsSqlParameter).Enabled = false;
				((CheckBox)checkBoxAllowUsingAsSqlParameter).Checked = false;
			}
			((Control)panelBindingValue).Visible = ((ListControl)comboBoxTypeMode).SelectedIndex == 0 || ((ListControl)comboBoxTypeMode).SelectedIndex == 2;
			StiLabelUX stiLabelUX = labelChecked;
			StiTreeViewBox stiTreeViewBox = treeViewBoxCheckedDataSource;
			StiTextBox stiTextBox = textBoxCheckedExpression;
			bool flag7 = (((Control)buttonEditExpressionChecked).Enabled = !((CheckBox)checkBoxAllowUserValues).Checked);
			bool flag9 = (((Control)stiTextBox).Enabled = flag7);
			visible = (((Control)stiTreeViewBox).Enabled = flag9);
			((Control)stiLabelUX).Enabled = visible;
			((Control)panelDialogInfoDateTime).Visible = ((CheckBox)checkBoxRequestFromUser).Checked && !((CheckBox)checkBoxReadOnly).Checked && (VariableType == typeof(DateTime) || VariableType == typeof(DateTime?) || VariableType == typeof(DateTimeRange) || VariableType == typeof(DateTimeList));
			((Control)panelDialogInfoMask).Visible = ((CheckBox)checkBoxRequestFromUser).Checked && !((CheckBox)checkBoxReadOnly).Checked && (VariableType == typeof(string) || VariableType == typeof(StringRange) || VariableType == typeof(StringList));
			visible2 = ((CheckBox)checkBoxRequestFromUser).Checked && !((CheckBox)checkBoxReadOnly).Checked;
		}
		if (((CheckBox)checkBoxRequestFromUser).Checked && !((CheckBox)checkBoxReadOnly).Checked)
		{
			((Control)panelSelection).Visible = ((ListControl)comboBoxTypeMode).SelectedIndex == 0;
			((Control)panelItems).Visible = ((ListControl)comboBoxDataSource).SelectedIndex == 0 && VariableType != typeof(byte[]);
			((Control)panelColumns).Visible = ((ListControl)comboBoxDataSource).SelectedIndex == 1;
			((Control)panelBindingColumns).Visible = ((CheckBox)checkBoxBindingValue).Checked && ((ListControl)comboBoxDataSource).SelectedIndex == 1 && (((ListControl)comboBoxTypeMode).SelectedIndex == 0 || ((ListControl)comboBoxTypeMode).SelectedIndex == 2);
			if (((Control)panelSelection).Visible && ((ListControl)comboBoxSelection).SelectedIndex != 0)
			{
				((Control)panelInitByValue).Visible = false;
				((Control)panelInitByDateTime).Visible = false;
				((Control)panelInitByExpression).Visible = false;
				((Control)panelInitBy).Visible = false;
			}
		}
		else
		{
			((Control)panelSelection).Visible = false;
			((Control)panelItems).Visible = false;
			((Control)panelColumns).Visible = false;
			((Control)panelBindingColumns).Visible = false;
		}
		((Control)panelDialogItems).Visible = visible2;
		((Control)panelColumns).Height = ((Control)panelKeysValues).Height;
		if (((Control)panelBindingValue).Visible)
		{
			StiPanelUX stiPanelUX = panelColumns;
			((Control)stiPanelUX).Height = ((Control)stiPanelUX).Height + ((Control)panelBindingValue).Height;
		}
		int num = ((Control)panelMain).Height + ((Control)groupButtons).Height + SystemInformation.CaptionHeight + SystemInformation.BorderSize.Height * 2;
		if (((Control)panelInitByTimeSpan).Visible)
		{
			num += ((Control)panelInitByTimeSpan).Height;
		}
		if (((Control)panelInitByDateTime).Visible)
		{
			num += ((Control)panelInitByDateTime).Height;
		}
		if (((Control)panelInitByValue).Visible)
		{
			num += ((Control)panelInitByValue).Height;
		}
		if (((Control)panelInitByExpression).Visible)
		{
			num += ((Control)panelInitByExpression).Height;
		}
		if (((Control)panelInitByImage).Visible)
		{
			num += ((Control)panelInitByImage).Height;
		}
		if (((Control)panelInitByBool).Visible)
		{
			num += ((Control)panelInitByBool).Height;
		}
		if (((Control)panelInitBy).Visible)
		{
			num += ((Control)panelInitBy).Height;
		}
		if (((Control)panelInitByTimeSpanRange).Visible)
		{
			num += ((Control)panelInitByTimeSpanRange).Height;
		}
		if (((Control)panelInitByDateTimeRange).Visible)
		{
			num += ((Control)panelInitByDateTimeRange).Height;
		}
		if (((Control)panelInitByValueRange).Visible)
		{
			num += ((Control)panelInitByValueRange).Height;
		}
		if (((Control)panelInitByExpressionRange).Visible)
		{
			num += ((Control)panelInitByExpressionRange).Height;
		}
		if (((Control)panelDialogInfoDateTime).Visible)
		{
			num += ((Control)panelDialogInfoDateTime).Height;
		}
		if (((Control)panelDialogInfoMask).Visible)
		{
			num += ((Control)panelDialogInfoMask).Height;
		}
		if (((Control)panelDialogItems).Visible)
		{
			num += ((Control)panelDialogItems).Height;
		}
		if (((Control)panelItems).Visible)
		{
			num += ((Control)panelItems).Height;
		}
		if (((Control)panelColumns).Visible)
		{
			num += ((Control)panelColumns).Height;
		}
		if (((Control)panelBindingColumns).Visible)
		{
			num += ((Control)panelBindingColumns).Height;
		}
		if (((Control)panelSelection).Visible)
		{
			num += ((Control)panelSelection).Height;
		}
		if (((Control)panelSettings).Visible)
		{
			num += ((Control)panelSettings).Height;
		}
		num += StiScale.I15;
		if (num > Screen.PrimaryScreen.WorkingArea.Height - StiScale.I(50))
		{
			num = Screen.PrimaryScreen.WorkingArea.Height - StiScale.I(50);
			((ScrollableControl)this).AutoScroll = true;
		}
		else
		{
			((ScrollableControl)this).AutoScroll = false;
		}
		int num2 = ((Control)this).Height - num;
		int num3 = ((Form)this).Location.Y + num2 / 2;
		if (num3 < 0)
		{
			num3 = 0;
		}
		((Form)this).Location = new Point(((Form)this).Location.X, num3);
		((Control)this).Height = num;
		if (Items == null || Items.Count == 0)
		{
			((Control)textBoxItems).Text = string.Format("[{0}]", StiLocalization.Get("Report", "NotAssigned"));
		}
		else
		{
			string listSeparator = CultureInfo.CurrentCulture.TextInfo.ListSeparator;
			string text = null;
			foreach (StiDialogInfoItem item in Items)
			{
				text = ((text != null) ? (text + listSeparator + " " + item.ToString(VariableDateTimeType)) : item.ToString(VariableDateTimeType));
			}
			((Control)textBoxItems).Text = text;
		}
		if (((Control)panelColumns).Visible)
		{
			((Control)panelColumns).BringToFront();
		}
		if (((Control)panelBindingColumns).Visible)
		{
			((Control)panelBindingColumns).BringToFront();
		}
		if (((Control)panelItems).Visible)
		{
			((Control)panelItems).BringToFront();
		}
		if (((Control)panelDialogInfoDateTime).Visible)
		{
			((Control)panelDialogInfoDateTime).BringToFront();
		}
		if (((Control)panelDialogInfoMask).Visible)
		{
			((Control)panelDialogInfoMask).BringToFront();
		}
	}

	private string GetVariableDateTimeFormat(StiDateTimeType type)
	{
		if (type != 0)
		{
			return "dd.MM.yyyy, HH:mm:ss";
		}
		return "dd.MM.yyyy";
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/data_data_dictionary_variables.htm");
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		bool flag = true;
		if (((Control)textBoxVariableAlias).Text.Length == 0)
		{
			errorProvider.SetError((Control)(object)textBoxVariableAlias, StiFieldChecker.AliasRequire);
			flag = false;
			((Control)textBoxVariableAlias).Focus();
		}
		else
		{
			errorProvider.SetError((Control)(object)textBoxVariableAlias, string.Empty);
		}
		if (((Control)textBoxVariableName).Text.Length == 0)
		{
			errorProvider.SetError((Control)(object)textBoxVariableName, StiFieldChecker.NameRequire);
			flag = false;
			((Control)textBoxVariableName).Focus();
		}
		else
		{
			if (((Control)textBoxVariableName).Text != originalName && StiNameCreation.Exists(Report, ((Control)textBoxVariableName).Text))
			{
				errorProvider.SetError((Control)(object)textBoxVariableName, StiFieldChecker.NameExists(((Control)textBoxVariableName).Text));
				((Control)textBoxVariableName).Focus();
				return;
			}
			errorProvider.SetError((Control)(object)textBoxVariableName, string.Empty);
		}
		if (((Control)comboBoxType).Text.Length == 0)
		{
			errorProvider.SetError((Control)(object)comboBoxType, string.Format(StiLocalization.Get("Errors", "FieldRequire"), StiLocalization.Get("PropertyMain", "Type")));
			flag = false;
			((Control)comboBoxType).Focus();
		}
		else
		{
			errorProvider.SetError((Control)(object)comboBoxType, string.Empty);
		}
		if (VariableType == typeof(Image))
		{
			((ListControl)comboBoxInitBy).SelectedIndex = 0;
		}
		if (flag)
		{
			((Form)this).DialogResult = (DialogResult)1;
			((Form)this).Close();
		}
	}

	private void This_CancelClick(object sender, EventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)2;
		((Form)this).Close();
	}

	private void This_Load(object sender, EventArgs e)
	{
		CheckStates();
		lockRemoveItems = false;
	}

	private void TextBoxVariableName_Enter(object sender, EventArgs e)
	{
		oldValue = ((Control)textBoxVariableName).Text;
	}

	private void TextBoxVariableName_Leave(object sender, EventArgs e)
	{
		if (oldValue == ((Control)textBoxVariableAlias).Text)
		{
			((Control)textBoxVariableAlias).Text = ((Control)textBoxVariableName).Text;
		}
	}

	private void ComboBoxType_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (((ListControl)comboBoxTypeMode).SelectedIndex == 1 && VariableType == typeof(string))
		{
			((ListControl)comboBoxTypeMode).SelectedIndex = 0;
		}
		if (((ListControl)comboBoxTypeMode).SelectedIndex == 2 || ((ListControl)comboBoxTypeMode).SelectedIndex == 3)
		{
			if (VariableType == typeof(bool) || VariableType == typeof(ulong) || VariableType == typeof(uint) || VariableType == typeof(ushort) || VariableType == typeof(sbyte) || VariableType == typeof(Image) || VariableType == typeof(object))
			{
				((ListControl)comboBoxTypeMode).SelectedIndex = 0;
			}
			CheckDefault();
		}
		if (VariableType == typeof(object) || VariableType == typeof(object[]))
		{
			((CheckBox)checkBoxRequestFromUser).Checked = false;
			((Control)checkBoxRequestFromUser).Enabled = false;
		}
		else
		{
			((Control)checkBoxRequestFromUser).Enabled = true;
		}
		if (!lockRemoveItems && Items != null)
		{
			Items.Clear();
		}
		CheckStates();
	}

	private void PanelImage_DoubleClick(object sender, EventArgs e)
	{
		ButtonImageOpen_Click(sender, e);
	}

	private void ButtonRemoveImage_Click(object sender, EventArgs e)
	{
		VariableImage = null;
	}

	private void ButtonImageOpen_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Invalid comparison between Unknown and I4
		try
		{
			OpenFileDialog val = new OpenFileDialog();
			try
			{
				((FileDialog)val).Filter = StiFileDialogHelper.GetImageFilter();
				((FileDialog)val).RestoreDirectory = true;
				if ((int)((CommonDialog)val).ShowDialog() == 1 && StiMaximumResourceSizeMessageBox.Check((Form)(object)this, ((FileDialog)val).FileName))
				{
					VariableImage = StiImageHelper.FromFile(((FileDialog)val).FileName);
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void CheckBoxNotAssigned_CheckedChanged(object sender, EventArgs e)
	{
		if (((CheckBox)checkBoxNotAssigned).Checked && VariableType == typeof(DateTime))
		{
			((ListControl)comboBoxTypeMode).SelectedIndex = 1;
		}
		((Control)dateTimePickerDefaultValue).Enabled = !((CheckBox)checkBoxNotAssigned).Checked;
	}

	private void CheckBoxReadOnly_CheckedChanged(object sender, EventArgs e)
	{
		((Control)checkBoxRequestFromUser).Enabled = !((CheckBox)checkBoxReadOnly).Checked;
		if (((CheckBox)checkBoxReadOnly).Checked)
		{
			((CheckBox)checkBoxRequestFromUser).Checked = false;
		}
		CheckStates();
	}

	private void ComboBoxInitBy_SelectedIndexChanged(object sender, EventArgs e)
	{
		CheckStates();
	}

	private void ButtonEditInitByExpression_Click(object sender, EventArgs e)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Invalid comparison between Unknown and I4
		string text = ((Control)textBoxInitByExpression).Text;
		if ((int)StiTextEditor.EditText(Report, ref text, StiCodeEditorMode.EditorCode) == 1)
		{
			((Control)textBoxInitByExpression).Text = text;
		}
	}

	private void ComboBoxType_DrawItem(object sender, DrawItemEventArgs e)
	{
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Invalid comparison between Unknown and I4
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_042f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0436: Expected O, but got Unknown
		//IL_0438: Unknown result type (might be due to invalid IL or missing references)
		//IL_043f: Expected O, but got Unknown
		//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Expected O, but got Unknown
		if (e.Index == -1)
		{
			return;
		}
		string text = null;
		StiType stiType = ((ComboBox)comboBoxType).Items[e.Index] as StiType;
		Type type = stiType.Type;
		text = ((type == typeof(float)) ? "Float" : ((type == typeof(double)) ? "Float" : ((type == typeof(decimal)) ? "Decimal" : ((type == typeof(DateTime)) ? "DateTime" : ((type == typeof(TimeSpan)) ? "DateTime" : ((type == typeof(bool)) ? "Bool" : ((type == typeof(sbyte)) ? "Int" : ((type == typeof(byte)) ? "Int" : ((type == typeof(short)) ? "Int" : ((type == typeof(ushort)) ? "Int" : ((type == typeof(int)) ? "Int" : ((type == typeof(uint)) ? "Int" : ((type == typeof(long)) ? "Int" : ((type == typeof(ulong)) ? "Int" : ((type == typeof(Image)) ? "Image" : ((!(type == typeof(char))) ? "String" : "Char"))))))))))))))));
		Graphics graphics = e.Graphics;
		Color color = StiUX.ItemBackground;
		Color color2 = StiUX.ItemForeground;
		if ((e.State & 0x10) > 0)
		{
			color = StiUX.ItemBackgroundSelected;
			color2 = StiUX.ItemForegroundSelected;
		}
		StiDrawing.FillRectangle(graphics, color, e.Bounds);
		if ((e.State & 0x10) == 0 && e.Bounds.Top > 10 && (type == typeof(float) || type == typeof(DateTime) || type == typeof(bool) || type == typeof(sbyte) || type == typeof(Image)))
		{
			Pen val = new Pen(StiUX.IsDark ? StiUX.ItemSeparator : Color.Gray);
			try
			{
				graphics.DrawLine(val, e.Bounds.X, e.Bounds.Y, e.Bounds.Right, e.Bounds.Y);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		double step = StiScale.Step;
		Image val2 = imageListTypes.Images[text];
		if (val2 != null)
		{
			Rectangle rectangle = new Rectangle(e.Bounds.Left + (int)Math.Ceiling(2.0 * step), e.Bounds.Top + (e.Bounds.Height - (int)(16.0 * step)) / 2, (int)Math.Ceiling(16.0 * step), (int)Math.Ceiling(16.0 * step));
			graphics.DrawImage(val2, rectangle);
		}
		Rectangle rectangle2 = new Rectangle(e.Bounds.Left + (int)Math.Ceiling(22.0 * step), e.Bounds.Top, e.Bounds.Width - (int)Math.Ceiling(22.0 * step), e.Bounds.Height - 2);
		StringFormat val3 = new StringFormat();
		try
		{
			SolidBrush val4 = new SolidBrush(color2);
			try
			{
				val3.LineAlignment = (StringAlignment)1;
				val3.Alignment = (StringAlignment)0;
				graphics.DrawString(stiType.ToString(), comboBoxType.DrawingFont, (Brush)(object)val4, (RectangleF)rectangle2, val3);
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

	private void ComboBoxTypeMode_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (((ListControl)comboBoxTypeMode).SelectedIndex == 1 || ((ListControl)comboBoxTypeMode).SelectedIndex == 2 || ((ListControl)comboBoxTypeMode).SelectedIndex == 3)
		{
			if (VariableType == typeof(bool) || VariableType == typeof(ulong) || VariableType == typeof(uint) || VariableType == typeof(ushort) || VariableType == typeof(sbyte) || VariableType == typeof(Image) || VariableType == typeof(object))
			{
				VariableType = typeof(long);
			}
			CheckDefault();
		}
		if (((ListControl)comboBoxTypeMode).SelectedIndex == 1 && VariableType == typeof(string))
		{
			((ListControl)comboBoxTypeMode).SelectedIndex = 0;
		}
		if (VariableType == typeof(object) || VariableType == typeof(object[]))
		{
			((CheckBox)checkBoxRequestFromUser).Checked = false;
		}
		if (!lockRemoveItems && prevIndex != ((ListControl)comboBoxTypeMode).SelectedIndex)
		{
			if (prevIndex != 3 && ((ListControl)comboBoxTypeMode).SelectedIndex == 3)
			{
				Items.Clear();
			}
			if (prevIndex == 3 && ((ListControl)comboBoxTypeMode).SelectedIndex != 3)
			{
				Items.Clear();
			}
		}
		if (((ListControl)comboBoxTypeMode).SelectedIndex == 3)
		{
			((ListControl)comboBoxDataSource).SelectedIndex = 0;
			((Control)comboBoxDataSource).Enabled = false;
		}
		else
		{
			((Control)comboBoxDataSource).Enabled = true;
		}
		CheckStates();
		prevIndex = ((ListControl)comboBoxTypeMode).SelectedIndex;
	}

	private void ButtonEditInitByExpressionFrom_Click(object sender, EventArgs e)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Invalid comparison between Unknown and I4
		string text = ((Control)textBoxInitByExpressionFrom).Text;
		if ((int)StiTextEditor.EditText(Report, ref text, StiCodeEditorMode.EditorCode) == 1)
		{
			((Control)textBoxInitByExpressionFrom).Text = text;
		}
	}

	private void ButtonEditInitByExpressionTo_Click(object sender, EventArgs e)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Invalid comparison between Unknown and I4
		string text = ((Control)textBoxInitByExpressionTo).Text;
		if ((int)StiTextEditor.EditText(Report, ref text, StiCodeEditorMode.EditorCode) == 1)
		{
			((Control)textBoxInitByExpressionTo).Text = text;
		}
	}

	private void CheckBoxNotAssignedFrom_CheckedChanged(object sender, EventArgs e)
	{
		((Control)dateTimePickerDefaultValueFrom).Enabled = !((CheckBox)checkBoxNotAssignedFrom).Checked;
	}

	private void CheckBoxNotAssignedTo_CheckedChanged(object sender, EventArgs e)
	{
		((Control)dateTimePickerDefaultValueTo).Enabled = !((CheckBox)checkBoxNotAssignedTo).Checked;
	}

	private void ButtonEditInitByValue_Click(object sender, EventArgs e)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Invalid comparison between Unknown and I4
		string text = ((Control)buttonEditDefaultValue).Text;
		if ((int)StiTextEditor.EditText(Report, ref text, StiCodeEditorMode.EditorText) == 1)
		{
			((Control)buttonEditDefaultValue).Text = text;
		}
	}

	private void ButtonEditInitByValueFrom_Click(object sender, EventArgs e)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Invalid comparison between Unknown and I4
		string text = ((Control)buttonEditDefaultValueFrom).Text;
		if ((int)StiTextEditor.EditText(Report, ref text, StiCodeEditorMode.EditorText) == 1)
		{
			((Control)buttonEditDefaultValueFrom).Text = text;
		}
	}

	private void ButtonEditInitByValueTo_Click(object sender, EventArgs e)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Invalid comparison between Unknown and I4
		string text = ((Control)buttonEditDefaultValueTo).Text;
		if ((int)StiTextEditor.EditText(Report, ref text, StiCodeEditorMode.EditorText) == 1)
		{
			((Control)buttonEditDefaultValueTo).Text = text;
		}
	}

	private void CheckBoxRequestFromUser_CheckedChanged(object sender, EventArgs e)
	{
		if (VariableType == typeof(object) || VariableType == typeof(object[]))
		{
			VariableType = typeof(string);
		}
		CheckStates();
	}

	private void ComboBoxDataSource_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (((ListControl)comboBoxDataSource).SelectedIndex == 1 && ((ListControl)comboBoxTypeMode).SelectedIndex == 3)
		{
			((ListControl)comboBoxDataSource).SelectedIndex = 0;
		}
		CheckStates();
	}

	private void ButtonEditItems_Click(object sender, EventArgs e)
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Invalid comparison between Unknown and I4
		List<StiDialogInfoItem> items = Items.Select((StiDialogInfoItem i) => i.Clone() as StiDialogInfoItem).Cast<StiDialogInfoItem>().ToList();
		StiDialogInfoItemsEditor stiDialogInfoItemsEditor = new StiDialogInfoItemsEditor(items, VariableType, this);
		try
		{
			if ((int)((Form)stiDialogInfoItemsEditor).ShowDialog((IWin32Window)(object)this) == 1)
			{
				Items = stiDialogInfoItemsEditor.GetItems();
				CheckStates();
			}
		}
		finally
		{
			((IDisposable)stiDialogInfoItemsEditor)?.Dispose();
		}
	}

	private void TreeNode_Expand(object sender, TreeViewCancelEventArgs e)
	{
		if (e.Node is TreeNodeEx node && e.Node.Tag != null)
		{
			dataBuilder.ExpandedNode(node, includeCalcColumn: true);
		}
	}

	private void ColumnKeys_TreeNodesFill(object sender, StiTreeNodesFillEventArgs e)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		((TreeView)e.TreeView).ImageList = StiDataBuilder.Images;
		((TreeView)e.TreeView).BeforeExpand += new TreeViewCancelEventHandler(TreeNode_Expand);
		TreeNode val = new TreeNode(NoColumn);
		string imageKey = (val.SelectedImageKey = "Close");
		val.ImageKey = imageKey;
		e.Nodes.Add(val);
		dataBuilder.Build(e.Nodes, Report.Dictionary, dictionaryDesign: true, includeCalcColumn: true);
		if (e.Nodes.Count <= 1)
		{
			return;
		}
		StiTreeViewBox stiTreeViewBox = sender as StiTreeViewBox;
		if (((Control)stiTreeViewBox).Text == NoColumn)
		{
			((TreeView)e.TreeView).SelectedNode = e.Nodes[0];
			return;
		}
		string[] strs = ((Control)stiTreeViewBox).Text.Split(new char[1] { '.' });
		if (!SelectNode(strs, 0, e.Nodes, (TreeView)(object)e.TreeView))
		{
			((TreeView)e.TreeView).SelectedNode = ((TreeView)e.TreeView).Nodes[0];
		}
	}

	private void ColumnKeys_ValueChanged(object sender, StiValueChangedEventArgs e)
	{
		if (e.Text != NoColumn)
		{
			TreeNode selectedNode = ((TreeView)e.TreeView).SelectedNode;
			if (selectedNode != null)
			{
				e.Text = ((selectedNode.Tag is StiDataColumn) ? StiDataBuilder.GetColumnPathFromNode(selectedNode) : NoColumn);
			}
			else
			{
				e.Text = NoColumn;
			}
		}
	}

	private void ComboBoxDateTimeFormat_SelectedIndexChanged(object sender, EventArgs e)
	{
		CheckStates();
	}

	private void ButtonSaveCopy_Click(object sender, EventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)1;
		CopyModeActivated = true;
		((Form)this).Close();
	}

	private void CheckBoxBindingValue_CheckedChanged(object sender, EventArgs e)
	{
		CheckStates();
	}

	private void ComboBoxSelection_SelectedIndexChanged(object sender, EventArgs e)
	{
		CheckStates();
	}

	private void PanelImage_Paint(object sender, PaintEventArgs e)
	{
		StiImageDialogPainter.Paint((Control)(object)panelImage, e.Graphics, VariableImage, drawDragDrop: true, drawBorder: true, isImage: true);
	}

	private void ComboBoxSortDirection_SelectedIndexChanged(object sender, EventArgs e)
	{
		((Control)comboBoxSortField).Enabled = ((ListControl)comboBoxSortDirection).SelectedIndex != 0;
	}

	private void TreeViewBoxCheckedDataSourceTreeNode_BeforeExpand(object sender, TreeViewCancelEventArgs e)
	{
		if (e.Node is TreeNodeEx node && e.Node.Tag != null)
		{
			dataBuilder.ExpandedNode(node, includeCalcColumn: false);
		}
	}

	private void TreeViewBoxCheckedDataSource_TreeNodesFill(object sender, StiTreeNodesFillEventArgs e)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Expected O, but got Unknown
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Expected O, but got Unknown
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Expected O, but got Unknown
		StiTreeView treeView = e.TreeView;
		((TreeView)treeView).BeforeExpand += new TreeViewCancelEventHandler(TreeViewBoxCheckedDataSourceTreeNode_BeforeExpand);
		((TreeView)treeView).Nodes.Clear();
		((TreeView)treeView).Nodes.Add(new TreeNode(NoColumn)
		{
			ImageKey = "Close",
			SelectedImageKey = "Close"
		});
		((TreeView)treeView).SelectedNode = ((TreeView)treeView).Nodes[0];
		((TreeView)treeView).ImageList = StiDataBuilder.Images;
		TreeNode val = new TreeNode(StiLocalization.Get("PropertyMain", "DataSources"))
		{
			ImageKey = "DataSource",
			SelectedImageKey = "DataSource"
		};
		((TreeView)treeView).Nodes.Add(val);
		StiDataSourcesCollection dataSources = Report.Dictionary.DataSources;
		foreach (StiDataSource item in dataSources)
		{
			TreeNode val2 = new TreeNode(item.ToString())
			{
				Tag = item,
				ImageKey = "DataSource",
				SelectedImageKey = "DataSource"
			};
			StiDataAdapterService dataAdapter = StiDataAdapterService.GetDataAdapter(item);
			string categoryName = item.GetCategoryName();
			TreeNode val3 = null;
			foreach (TreeNode node in val.Nodes)
			{
				TreeNode val4 = node;
				if (val4.Text == categoryName)
				{
					val3 = val4;
					break;
				}
			}
			if (val3 == null)
			{
				val3 = new TreeNode(categoryName)
				{
					ImageKey = "Database",
					SelectedImageKey = "Database"
				};
				val.Nodes.Add(val3);
			}
			val3.Nodes.Add(val2);
			if (item == selectedDataSource)
			{
				((TreeView)treeView).SelectedNode = val2;
			}
		}
		((TreeView)treeView).ExpandAll();
	}

	private void TreeViewBoxCheckedDataSource_ValueChanged(object sender, StiValueChangedEventArgs e)
	{
		((Control)treeViewBoxCheckedDataSource).Text = e.Text;
		if (((TreeView)e.TreeView).SelectedNode == ((TreeView)e.TreeView).Nodes[0])
		{
			selectedDataSource = null;
		}
		else if (((TreeView)e.TreeView).SelectedNode.Tag is StiDataSource)
		{
			selectedDataSource = ((TreeView)e.TreeView).SelectedNode.Tag as StiDataSource;
		}
	}

	private void ButtonEditExpressionChecked_Click(object sender, EventArgs e)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Invalid comparison between Unknown and I4
		string text = ((Control)textBoxCheckedExpression).Text;
		DialogResult val = StiTextEditor.EditText(Report, ref text, StiCodeEditorMode.EditorExpression);
		if ((int)val == 1)
		{
			((Control)textBoxCheckedExpression).Text = text;
		}
	}

	private void CheckBoxAllowUserValues_CheckedChanged(object sender, EventArgs e)
	{
		StiLabelUX stiLabelUX = labelChecked;
		StiTreeViewBox stiTreeViewBox = treeViewBoxCheckedDataSource;
		StiTextBox stiTextBox = textBoxCheckedExpression;
		bool flag2 = (((Control)buttonEditExpressionChecked).Enabled = !((CheckBox)checkBoxAllowUserValues).Checked);
		bool flag4 = (((Control)stiTextBox).Enabled = flag2);
		bool enabled = (((Control)stiTreeViewBox).Enabled = flag4);
		((Control)stiLabelUX).Enabled = enabled;
	}

	private void PanelImage_DragDrop(object sender, DragEventArgs e)
	{
		string[] array = (string[])e.Data.GetData(DataFormats.FileDrop);
		if (array == null)
		{
			return;
		}
		foreach (string item in array.Where((string f) => f != null))
		{
			string ext = Path.GetExtension(item).ToLowerInvariant();
			if (StiResourceTypeHelper.IsImageType(ext))
			{
				UpdateNameAndAlias(Path.GetFileNameWithoutExtension(item).Normalize());
				if (StiMaximumResourceSizeMessageBox.Check((Form)(object)this, item))
				{
					VariableImage = StiImageHelper.TryFromFile(item);
				}
				break;
			}
		}
	}

	private void PanelImage_DragOver(object sender, DragEventArgs e)
	{
		if (!e.Data.GetDataPresent(DataFormats.FileDrop, false))
		{
			return;
		}
		string[] array = (string[])e.Data.GetData(DataFormats.FileDrop);
		if (array != null)
		{
			foreach (string item in array.Where((string f) => f != null))
			{
				string ext = Path.GetExtension(item).ToLowerInvariant();
				if (StiResourceTypeHelper.IsImageType(ext))
				{
					e.Effect = (DragDropEffects)(-2147483645);
					return;
				}
			}
		}
		e.Effect = (DragDropEffects)0;
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
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Expected O, but got Unknown
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Expected O, but got Unknown
		//IL_07c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cc: Expected O, but got Unknown
		//IL_08cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d9: Expected O, but got Unknown
		//IL_0da3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dad: Expected O, but got Unknown
		//IL_0f69: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f73: Expected O, but got Unknown
		//IL_0ffb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1005: Expected O, but got Unknown
		//IL_11fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_1208: Expected O, but got Unknown
		//IL_1290: Unknown result type (might be due to invalid IL or missing references)
		//IL_129a: Expected O, but got Unknown
		//IL_1926: Unknown result type (might be due to invalid IL or missing references)
		//IL_1930: Expected O, but got Unknown
		//IL_19b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_19c2: Expected O, but got Unknown
		//IL_1c54: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c5e: Expected O, but got Unknown
		//IL_1ce6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cf0: Expected O, but got Unknown
		//IL_2624: Unknown result type (might be due to invalid IL or missing references)
		//IL_262e: Expected O, but got Unknown
		//IL_26b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_26c0: Expected O, but got Unknown
		//IL_2f50: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fcc: Unknown result type (might be due to invalid IL or missing references)
		//IL_32c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c15: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c1f: Expected O, but got Unknown
		//IL_3ca8: Unknown result type (might be due to invalid IL or missing references)
		//IL_3cb2: Expected O, but got Unknown
		//IL_3df0: Unknown result type (might be due to invalid IL or missing references)
		//IL_3dfa: Expected O, but got Unknown
		//IL_3e07: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e11: Expected O, but got Unknown
		//IL_3e1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e28: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiVariableEditForm));
		labelName = new StiLabelUX();
		labelType = new StiLabelUX();
		labelAlias = new StiLabelUX();
		controlContainerItem2 = new ControlContainerItem();
		imageListTypes = new ImageList(components);
		errorProvider = new ErrorProvider(components);
		labelDefaultDateTimeValue = new StiLabelUX();
		dateTimePickerDefaultValue = new StiDateTimePickerUX();
		checkBoxNotAssigned = new StiCheckBoxUX();
		labelDescription = new StiLabelUX();
		panelMain = new StiPanelUX();
		comboBoxTypeMode = new StiComboBox();
		textBoxVariableName = new StiTextBox();
		textBoxDescription = new StiTextBox();
		comboBoxType = new StiComboBox();
		textBoxVariableAlias = new StiTextBox();
		panelInitByDateTime = new StiPanelUX();
		panelInitByValue = new StiPanelUX();
		labelValueSample = new StiLabelUX();
		labelValueSample2 = new StiLabelUX();
		labelDefaultValue = new StiLabelUX();
		buttonEditDefaultValue = new StiButtonEdit();
		panelInitByExpression = new StiPanelUX();
		labelExpressionSample = new StiLabelUX();
		labelExpressionSample2 = new StiLabelUX();
		textBoxInitByExpression = new StiButtonEdit();
		labelDefaultExpression = new StiLabelUX();
		panelInitByDateTimeRange = new StiPanelUX();
		checkBoxNotAssignedTo = new StiCheckBoxUX();
		checkBoxNotAssignedFrom = new StiCheckBoxUX();
		labelDateTimeValueTo = new StiLabelUX();
		labelDateTimeValueFrom = new StiLabelUX();
		dateTimePickerDefaultValueTo = new StiDateTimePickerUX();
		dateTimePickerDefaultValueFrom = new StiDateTimePickerUX();
		panelInitByValueRange = new StiPanelUX();
		labelValueTo = new StiLabelUX();
		buttonEditDefaultValueTo = new StiButtonEdit();
		labelValueSampleRange = new StiLabelUX();
		labelValueSampleRange2 = new StiLabelUX();
		labelValueFrom = new StiLabelUX();
		buttonEditDefaultValueFrom = new StiButtonEdit();
		panelInitByExpressionRange = new StiPanelUX();
		textBoxInitByExpressionTo = new StiButtonEdit();
		labelExpressionSampleRange = new StiLabelUX();
		labelExpressionSampleRange2 = new StiLabelUX();
		textBoxInitByExpressionFrom = new StiButtonEdit();
		labelExpressionTo = new StiLabelUX();
		labelExpressionFrom = new StiLabelUX();
		toolTip1 = new ToolTip(components);
		panelDialogInfoDateTime = new StiPanelUX();
		labelDateTimeFormat = new StiLabelUX();
		comboBoxDateTimeFormat = new StiComboBox();
		panelDialogInfoMask = new StiPanelUX();
		textBoxMask = new StiTextBox();
		labelMask = new StiLabelUX();
		labelItems = new StiLabelUX();
		textBoxItems = new TextBox();
		buttonEditItems = new StiOperationButton();
		panelItems = new StiPanelUX();
		panelColumns = new StiPanelUX();
		panelBindingValue = new StiPanelUX();
		checkBoxBindingValue = new StiCheckBoxUX();
		panelKeysValues = new StiPanelUX();
		labelCheckedSample = new StiLabelUX();
		label2 = new StiLabelUX();
		buttonEditExpressionChecked = new StiOperationButton();
		textBoxCheckedExpression = new StiTextBox();
		treeViewBoxCheckedDataSource = new StiTreeViewBox();
		labelChecked = new StiLabelUX();
		comboBoxSortDirection = new StiComboBox();
		comboBoxSortField = new StiComboBox();
		labelSortBy = new StiLabelUX();
		labelLabels = new StiLabelUX();
		columnKeys = new StiTreeViewBox();
		labelKeys = new StiLabelUX();
		columnValues = new StiTreeViewBox();
		panelBindingColumns = new StiPanelUX();
		labelDependentColumn = new StiLabelUX();
		comboBoxBindingValue = new StiComboBox();
		labelVariable = new StiLabelUX();
		columnBindingColumn = new StiTreeViewBox();
		panelInitByTimeSpanRange = new StiPanelUX();
		timeSpanPickerDefaultValueTo = new StiTimeSpanPicker();
		timeSpanPickerDefaultValueFrom = new StiTimeSpanPicker();
		checkBoxTimeSpanToNotAssigned = new StiCheckBoxUX();
		checkBoxTimeSpanFromNotAssigned = new StiCheckBoxUX();
		labelTimeSpanValueTo = new StiLabelUX();
		labelTimeSpanValueFrom = new StiLabelUX();
		panelInitByTimeSpan = new StiPanelUX();
		timeSpanPickerDefaultValue = new StiTimeSpanPicker();
		checkBoxTimeSpanNotAssigned = new StiCheckBoxUX();
		labelDefaultTimeSpanValue = new StiLabelUX();
		buttonOk = new StiButtonUX();
		buttonCancel = new StiButtonUX();
		panelSelection = new StiGroupPanelUX();
		labelSelection = new StiLabelUX();
		comboBoxSelection = new StiComboBox();
		panelDialogItems = new StiGroupPanelUX();
		labelDataSource = new StiLabelUX();
		comboBoxDataSource = new StiComboBox();
		checkBoxAllowUserValues = new StiCheckBoxUX();
		panelSettings = new StiGroupPanelUX();
		checkBoxAllowUsingAsSqlParameter = new StiCheckBoxUX();
		checkBoxReadOnly = new StiCheckBoxUX();
		checkBoxRequestFromUser = new StiCheckBoxUX();
		panelInitByImage = new StiGroupPanelUX();
		buttonImageRemove = new StiOperationButton();
		buttonImageOpen = new StiOperationButton();
		labelImage = new StiLabelUX();
		panelImage = new StiPanelUX();
		panelInitByBool = new StiGroupPanelUX();
		checkBoxDefaultValue = new StiCheckBoxUX();
		groupButtons = new StiButtonPanelUX();
		buttonSaveCopy = new StiButtonUX();
		panelInitBy = new StiGroupPanelUX();
		labelInitBy = new StiLabelUX();
		comboBoxInitBy = new StiComboBox();
		((ISupportInitialize)errorProvider).BeginInit();
		((Control)panelMain).SuspendLayout();
		((Control)panelInitByDateTime).SuspendLayout();
		((Control)panelInitByValue).SuspendLayout();
		((Control)panelInitByExpression).SuspendLayout();
		((Control)panelInitByDateTimeRange).SuspendLayout();
		((Control)panelInitByValueRange).SuspendLayout();
		((Control)panelInitByExpressionRange).SuspendLayout();
		((Control)panelDialogInfoDateTime).SuspendLayout();
		((Control)panelDialogInfoMask).SuspendLayout();
		((Control)panelItems).SuspendLayout();
		((Control)panelColumns).SuspendLayout();
		((Control)panelBindingValue).SuspendLayout();
		((Control)panelKeysValues).SuspendLayout();
		((Control)panelBindingColumns).SuspendLayout();
		((Control)panelInitByTimeSpanRange).SuspendLayout();
		((Control)panelInitByTimeSpan).SuspendLayout();
		((Control)panelSelection).SuspendLayout();
		((Control)panelDialogItems).SuspendLayout();
		((Control)panelSettings).SuspendLayout();
		((Control)panelInitByImage).SuspendLayout();
		((Control)panelInitByBool).SuspendLayout();
		((Control)groupButtons).SuspendLayout();
		((Control)panelInitBy).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)labelName).Location = new Point(12, 12);
		((Control)labelName).Name = "labelName";
		((Control)labelName).Size = new Size(121, 20);
		((Control)labelName).TabIndex = 0;
		((Control)labelName).Text = "Name";
		((Label)labelName).TextAlign = (ContentAlignment)16;
		((Control)labelType).Location = new Point(12, 108);
		((Control)labelType).Name = "labelType";
		((Control)labelType).Size = new Size(121, 21);
		((Control)labelType).TabIndex = 6;
		((Control)labelType).Text = "Type";
		((Label)labelType).TextAlign = (ContentAlignment)16;
		((Label)labelType).UseCompatibleTextRendering = true;
		((Control)labelAlias).Location = new Point(12, 44);
		((Control)labelAlias).Name = "labelAlias";
		((Control)labelAlias).Size = new Size(121, 20);
		((Control)labelAlias).TabIndex = 2;
		((Control)labelAlias).Text = "Alias";
		((Label)labelAlias).TextAlign = (ContentAlignment)16;
		controlContainerItem2.AllowItemResize = false;
		controlContainerItem2.GlobalItem = false;
		controlContainerItem2.MenuVisibility = eMenuVisibility.VisibleAlways;
		controlContainerItem2.Name = "controlContainerItem2";
		controlContainerItem2.ThemeAware = true;
		imageListTypes.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("imageListTypes.ImageStream");
		imageListTypes.TransparentColor = Color.Magenta;
		imageListTypes.Images.SetKeyName(0, "String");
		imageListTypes.Images.SetKeyName(1, "Binary");
		imageListTypes.Images.SetKeyName(2, "Bool");
		imageListTypes.Images.SetKeyName(3, "Char");
		imageListTypes.Images.SetKeyName(4, "DateTime");
		imageListTypes.Images.SetKeyName(5, "Decimal");
		imageListTypes.Images.SetKeyName(6, "Float");
		imageListTypes.Images.SetKeyName(7, "Image");
		imageListTypes.Images.SetKeyName(8, "Int");
		errorProvider.ContainerControl = (ContainerControl)(object)this;
		errorProvider.DataMember = "";
		errorProvider.Icon = (Icon)componentResourceManager.GetObject("errorProvider.Icon");
		((Control)labelDefaultDateTimeValue).Location = new Point(12, 1);
		((Control)labelDefaultDateTimeValue).Name = "labelDefaultDateTimeValue";
		((Control)labelDefaultDateTimeValue).Size = new Size(121, 20);
		((Control)labelDefaultDateTimeValue).TabIndex = 8;
		((Control)labelDefaultDateTimeValue).Text = "Value";
		((Label)labelDefaultDateTimeValue).TextAlign = (ContentAlignment)16;
		((DateTimePicker)dateTimePickerDefaultValue).CustomFormat = "dd.MM.yyyy, HH:mm:ss";
		((DateTimePicker)dateTimePickerDefaultValue).Format = (DateTimePickerFormat)8;
		((Control)dateTimePickerDefaultValue).Location = new Point(139, 1);
		((Control)dateTimePickerDefaultValue).Name = "dateTimePickerDefaultValue";
		((Control)dateTimePickerDefaultValue).Size = new Size(156, 20);
		((Control)dateTimePickerDefaultValue).TabIndex = 0;
		((Control)checkBoxNotAssigned).Anchor = (AnchorStyles)13;
		((Control)checkBoxNotAssigned).BackColor = Color.Transparent;
		((Control)checkBoxNotAssigned).Location = new Point(301, 1);
		((Control)checkBoxNotAssigned).Name = "checkBoxNotAssigned";
		((Control)checkBoxNotAssigned).Size = new Size(171, 20);
		((Control)checkBoxNotAssigned).TabIndex = 0;
		((Control)checkBoxNotAssigned).Text = "Not assigned";
		((ButtonBase)checkBoxNotAssigned).UseVisualStyleBackColor = false;
		((CheckBox)checkBoxNotAssigned).CheckedChanged += CheckBoxNotAssigned_CheckedChanged;
		((Control)labelDescription).Location = new Point(12, 76);
		((Control)labelDescription).Name = "labelDescription";
		((Control)labelDescription).Size = new Size(121, 20);
		((Control)labelDescription).TabIndex = 4;
		((Control)labelDescription).Text = "Description";
		((Label)labelDescription).TextAlign = (ContentAlignment)16;
		((Control)panelMain).Controls.Add((Control)(object)comboBoxTypeMode);
		((Control)panelMain).Controls.Add((Control)(object)textBoxVariableName);
		((Control)panelMain).Controls.Add((Control)(object)labelName);
		((Control)panelMain).Controls.Add((Control)(object)textBoxDescription);
		((Control)panelMain).Controls.Add((Control)(object)comboBoxType);
		((Control)panelMain).Controls.Add((Control)(object)labelDescription);
		((Control)panelMain).Controls.Add((Control)(object)labelType);
		((Control)panelMain).Controls.Add((Control)(object)textBoxVariableAlias);
		((Control)panelMain).Controls.Add((Control)(object)labelAlias);
		((Control)panelMain).Dock = (DockStyle)1;
		((Control)panelMain).Location = new Point(0, 0);
		((Control)panelMain).Name = "panelMain";
		((Control)panelMain).Size = new Size(479, 143);
		((Control)panelMain).TabIndex = 0;
		((Control)comboBoxTypeMode).Anchor = (AnchorStyles)13;
		comboBoxTypeMode.DropDownStyle = (ComboBoxStyle)2;
		((ComboBox)comboBoxTypeMode).Items.AddRange(new object[3] { "Simple", "Array", "Range" });
		((Control)comboBoxTypeMode).Location = new Point(342, 108);
		((ComboBox)comboBoxTypeMode).MaxDropDownItems = 30;
		((Control)comboBoxTypeMode).Name = "comboBoxTypeMode";
		((Control)comboBoxTypeMode).Size = new Size(125, 21);
		((Control)comboBoxTypeMode).TabIndex = 4;
		((ComboBox)comboBoxTypeMode).SelectedIndexChanged += ComboBoxTypeMode_SelectedIndexChanged;
		((Control)textBoxVariableName).Location = new Point(139, 12);
		((Control)textBoxVariableName).Name = "textBoxVariableName";
		((Control)textBoxVariableName).Size = new Size(216, 21);
		((Control)textBoxVariableName).TabIndex = 0;
		((Control)textBoxVariableName).Enter += TextBoxVariableName_Enter;
		((Control)textBoxVariableName).Leave += TextBoxVariableName_Leave;
		((Control)textBoxDescription).Anchor = (AnchorStyles)13;
		((Control)textBoxDescription).Location = new Point(139, 76);
		((Control)textBoxDescription).Name = "textBoxDescription";
		((Control)textBoxDescription).Size = new Size(328, 21);
		((Control)textBoxDescription).TabIndex = 2;
		comboBoxType.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxType).Location = new Point(139, 108);
		((ComboBox)comboBoxType).MaxDropDownItems = 30;
		((Control)comboBoxType).Name = "comboBoxType";
		((Control)comboBoxType).Size = new Size(195, 21);
		((Control)comboBoxType).TabIndex = 3;
		((ComboBox)comboBoxType).DrawItem += new DrawItemEventHandler(ComboBoxType_DrawItem);
		((ComboBox)comboBoxType).SelectedIndexChanged += ComboBoxType_SelectedIndexChanged;
		((Control)textBoxVariableAlias).Location = new Point(139, 44);
		((Control)textBoxVariableAlias).Name = "textBoxVariableAlias";
		((Control)textBoxVariableAlias).Size = new Size(216, 21);
		((Control)textBoxVariableAlias).TabIndex = 1;
		((Control)panelInitByDateTime).Controls.Add((Control)(object)checkBoxNotAssigned);
		((Control)panelInitByDateTime).Controls.Add((Control)(object)labelDefaultDateTimeValue);
		((Control)panelInitByDateTime).Controls.Add((Control)(object)dateTimePickerDefaultValue);
		((Control)panelInitByDateTime).Dock = (DockStyle)1;
		((Control)panelInitByDateTime).Location = new Point(0, 433);
		((Control)panelInitByDateTime).Name = "panelInitByDateTime";
		((Control)panelInitByDateTime).Size = new Size(479, 38);
		((Control)panelInitByDateTime).TabIndex = 4;
		((Control)panelInitByValue).Controls.Add((Control)(object)labelValueSample);
		((Control)panelInitByValue).Controls.Add((Control)(object)labelValueSample2);
		((Control)panelInitByValue).Controls.Add((Control)(object)labelDefaultValue);
		((Control)panelInitByValue).Controls.Add((Control)(object)buttonEditDefaultValue);
		((Control)panelInitByValue).Dock = (DockStyle)1;
		((Control)panelInitByValue).Location = new Point(0, 189);
		((Control)panelInitByValue).Name = "panelInitByValue";
		((Control)panelInitByValue).Size = new Size(479, 51);
		((Control)panelInitByValue).TabIndex = 2;
		((Control)labelValueSample).Font = new Font("Microsoft Sans Serif", 6.75f, (FontStyle)1, (GraphicsUnit)3, (byte)204);
		((Control)labelValueSample).Location = new Point(139, 26);
		((Control)labelValueSample).Name = "labelValueSample";
		((Control)labelValueSample).Size = new Size(45, 13);
		((Control)labelValueSample).TabIndex = 17;
		((Control)labelValueSample).Text = "Sample:";
		((Label)labelValueSample).TextAlign = (ContentAlignment)16;
		((Label)labelValueSample2).AutoEllipsis = true;
		((Control)labelValueSample2).Font = new Font("Microsoft Sans Serif", 6.75f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)labelValueSample2).Location = new Point(190, 26);
		((Control)labelValueSample2).Name = "labelValueSample2";
		((Control)labelValueSample2).Size = new Size(229, 13);
		((Control)labelValueSample2).TabIndex = 16;
		((Control)labelValueSample2).Text = "123; My text; 567f; 456.23f; Test String; A";
		((Label)labelValueSample2).TextAlign = (ContentAlignment)16;
		((Control)labelDefaultValue).Location = new Point(12, 1);
		((Control)labelDefaultValue).Name = "labelDefaultValue";
		((Control)labelDefaultValue).Size = new Size(121, 21);
		((Control)labelDefaultValue).TabIndex = 11;
		((Control)labelDefaultValue).Text = "Value";
		((Label)labelDefaultValue).TextAlign = (ContentAlignment)16;
		buttonEditDefaultValue.AllowEscape = false;
		((Control)buttonEditDefaultValue).Location = new Point(139, 1);
		((Control)buttonEditDefaultValue).Name = "buttonEditDefaultValue";
		((Control)buttonEditDefaultValue).Size = new Size(237, 21);
		((Control)buttonEditDefaultValue).TabIndex = 0;
		buttonEditDefaultValue.ButtonClick += ButtonEditInitByValue_Click;
		((Control)panelInitByExpression).Controls.Add((Control)(object)labelExpressionSample);
		((Control)panelInitByExpression).Controls.Add((Control)(object)labelExpressionSample2);
		((Control)panelInitByExpression).Controls.Add((Control)(object)textBoxInitByExpression);
		((Control)panelInitByExpression).Controls.Add((Control)(object)labelDefaultExpression);
		((Control)panelInitByExpression).Dock = (DockStyle)1;
		((Control)panelInitByExpression).Location = new Point(0, 541);
		((Control)panelInitByExpression).Name = "panelInitByExpression";
		((Control)panelInitByExpression).Size = new Size(479, 51);
		((Control)panelInitByExpression).TabIndex = 6;
		((Control)labelExpressionSample).Font = new Font("Microsoft Sans Serif", 6.75f, (FontStyle)1, (GraphicsUnit)3, (byte)204);
		((Control)labelExpressionSample).Location = new Point(139, 26);
		((Control)labelExpressionSample).Name = "labelExpressionSample";
		((Control)labelExpressionSample).Size = new Size(45, 13);
		((Control)labelExpressionSample).TabIndex = 15;
		((Control)labelExpressionSample).Text = "Sample:";
		((Label)labelExpressionSample).TextAlign = (ContentAlignment)16;
		((Label)labelExpressionSample2).AutoEllipsis = true;
		((Control)labelExpressionSample2).Font = new Font("Microsoft Sans Serif", 6.75f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)labelExpressionSample2).Location = new Point(190, 26);
		((Control)labelExpressionSample2).Name = "labelExpressionSample2";
		((Control)labelExpressionSample2).Size = new Size(229, 13);
		((Control)labelExpressionSample2).TabIndex = 14;
		((Control)labelExpressionSample2).Text = "Data.Field+1; Var+Var2; float.Parse(MyVar); \"My Str\"";
		((Label)labelExpressionSample2).TextAlign = (ContentAlignment)16;
		textBoxInitByExpression.AllowEscape = false;
		((Control)textBoxInitByExpression).Location = new Point(139, 1);
		((Control)textBoxInitByExpression).Name = "textBoxInitByExpression";
		((Control)textBoxInitByExpression).Size = new Size(237, 21);
		((Control)textBoxInitByExpression).TabIndex = 0;
		textBoxInitByExpression.ButtonClick += ButtonEditInitByExpression_Click;
		((Control)labelDefaultExpression).Location = new Point(12, 1);
		((Control)labelDefaultExpression).Name = "labelDefaultExpression";
		((Control)labelDefaultExpression).Size = new Size(121, 20);
		((Control)labelDefaultExpression).TabIndex = 13;
		((Control)labelDefaultExpression).Text = "Expression";
		((Label)labelDefaultExpression).TextAlign = (ContentAlignment)16;
		((Control)panelInitByDateTimeRange).Controls.Add((Control)(object)checkBoxNotAssignedTo);
		((Control)panelInitByDateTimeRange).Controls.Add((Control)(object)checkBoxNotAssignedFrom);
		((Control)panelInitByDateTimeRange).Controls.Add((Control)(object)labelDateTimeValueTo);
		((Control)panelInitByDateTimeRange).Controls.Add((Control)(object)labelDateTimeValueFrom);
		((Control)panelInitByDateTimeRange).Controls.Add((Control)(object)dateTimePickerDefaultValueTo);
		((Control)panelInitByDateTimeRange).Controls.Add((Control)(object)dateTimePickerDefaultValueFrom);
		((Control)panelInitByDateTimeRange).Dock = (DockStyle)1;
		((Control)panelInitByDateTimeRange).Location = new Point(0, 471);
		((Control)panelInitByDateTimeRange).Name = "panelInitByDateTimeRange";
		((Control)panelInitByDateTimeRange).Size = new Size(479, 70);
		((Control)panelInitByDateTimeRange).TabIndex = 5;
		((Control)checkBoxNotAssignedTo).Anchor = (AnchorStyles)13;
		((Control)checkBoxNotAssignedTo).BackColor = Color.Transparent;
		((Control)checkBoxNotAssignedTo).Location = new Point(301, 34);
		((Control)checkBoxNotAssignedTo).Name = "checkBoxNotAssignedTo";
		((Control)checkBoxNotAssignedTo).Size = new Size(171, 20);
		((Control)checkBoxNotAssignedTo).TabIndex = 3;
		((Control)checkBoxNotAssignedTo).Text = "Not assigned";
		((ButtonBase)checkBoxNotAssignedTo).UseVisualStyleBackColor = false;
		((CheckBox)checkBoxNotAssignedTo).CheckedChanged += CheckBoxNotAssignedTo_CheckedChanged;
		((Control)checkBoxNotAssignedFrom).Anchor = (AnchorStyles)13;
		((Control)checkBoxNotAssignedFrom).BackColor = Color.Transparent;
		((Control)checkBoxNotAssignedFrom).Location = new Point(301, 2);
		((Control)checkBoxNotAssignedFrom).Name = "checkBoxNotAssignedFrom";
		((Control)checkBoxNotAssignedFrom).Size = new Size(171, 20);
		((Control)checkBoxNotAssignedFrom).TabIndex = 1;
		((Control)checkBoxNotAssignedFrom).Text = "Not assigned";
		((ButtonBase)checkBoxNotAssignedFrom).UseVisualStyleBackColor = false;
		((CheckBox)checkBoxNotAssignedFrom).CheckedChanged += CheckBoxNotAssignedFrom_CheckedChanged;
		((Control)labelDateTimeValueTo).Location = new Point(12, 33);
		((Control)labelDateTimeValueTo).Name = "labelDateTimeValueTo";
		((Control)labelDateTimeValueTo).Size = new Size(121, 22);
		((Control)labelDateTimeValueTo).TabIndex = 8;
		((Control)labelDateTimeValueTo).Text = "To";
		((Label)labelDateTimeValueTo).TextAlign = (ContentAlignment)16;
		((Control)labelDateTimeValueFrom).Location = new Point(12, 1);
		((Control)labelDateTimeValueFrom).Name = "labelDateTimeValueFrom";
		((Control)labelDateTimeValueFrom).Size = new Size(121, 21);
		((Control)labelDateTimeValueFrom).TabIndex = 8;
		((Control)labelDateTimeValueFrom).Text = "From";
		((Label)labelDateTimeValueFrom).TextAlign = (ContentAlignment)16;
		((DateTimePicker)dateTimePickerDefaultValueTo).CustomFormat = "dd.MM.yyyy, HH:mm:ss";
		((DateTimePicker)dateTimePickerDefaultValueTo).Format = (DateTimePickerFormat)8;
		((Control)dateTimePickerDefaultValueTo).Location = new Point(139, 33);
		((Control)dateTimePickerDefaultValueTo).Name = "dateTimePickerDefaultValueTo";
		((Control)dateTimePickerDefaultValueTo).Size = new Size(156, 20);
		((Control)dateTimePickerDefaultValueTo).TabIndex = 2;
		((DateTimePicker)dateTimePickerDefaultValueFrom).CustomFormat = "dd.MM.yyyy, HH:mm:ss";
		((DateTimePicker)dateTimePickerDefaultValueFrom).Format = (DateTimePickerFormat)8;
		((Control)dateTimePickerDefaultValueFrom).Location = new Point(139, 1);
		((Control)dateTimePickerDefaultValueFrom).Name = "dateTimePickerDefaultValueFrom";
		((Control)dateTimePickerDefaultValueFrom).Size = new Size(156, 20);
		((Control)dateTimePickerDefaultValueFrom).TabIndex = 0;
		((Control)panelInitByValueRange).Controls.Add((Control)(object)labelValueTo);
		((Control)panelInitByValueRange).Controls.Add((Control)(object)buttonEditDefaultValueTo);
		((Control)panelInitByValueRange).Controls.Add((Control)(object)labelValueSampleRange);
		((Control)panelInitByValueRange).Controls.Add((Control)(object)labelValueSampleRange2);
		((Control)panelInitByValueRange).Controls.Add((Control)(object)labelValueFrom);
		((Control)panelInitByValueRange).Controls.Add((Control)(object)buttonEditDefaultValueFrom);
		((Control)panelInitByValueRange).Dock = (DockStyle)1;
		((Control)panelInitByValueRange).Location = new Point(0, 240);
		((Control)panelInitByValueRange).Name = "panelInitByValueRange";
		((Control)panelInitByValueRange).Size = new Size(479, 85);
		((Control)panelInitByValueRange).TabIndex = 3;
		((Control)labelValueTo).Location = new Point(12, 35);
		((Control)labelValueTo).Name = "labelValueTo";
		((Control)labelValueTo).Size = new Size(121, 21);
		((Control)labelValueTo).TabIndex = 19;
		((Control)labelValueTo).Text = "To";
		((Label)labelValueTo).TextAlign = (ContentAlignment)16;
		buttonEditDefaultValueTo.AllowEscape = false;
		((Control)buttonEditDefaultValueTo).Location = new Point(139, 34);
		((Control)buttonEditDefaultValueTo).Name = "buttonEditDefaultValueTo";
		((Control)buttonEditDefaultValueTo).Size = new Size(237, 21);
		((Control)buttonEditDefaultValueTo).TabIndex = 2;
		buttonEditDefaultValueTo.ButtonClick += ButtonEditInitByValueTo_Click;
		((Control)labelValueSampleRange).Font = new Font("Microsoft Sans Serif", 6.75f, (FontStyle)1, (GraphicsUnit)3, (byte)204);
		((Control)labelValueSampleRange).Location = new Point(139, 60);
		((Control)labelValueSampleRange).Name = "labelValueSampleRange";
		((Control)labelValueSampleRange).Size = new Size(45, 13);
		((Control)labelValueSampleRange).TabIndex = 17;
		((Control)labelValueSampleRange).Text = "Sample:";
		((Label)labelValueSampleRange).TextAlign = (ContentAlignment)16;
		((Label)labelValueSampleRange2).AutoEllipsis = true;
		((Control)labelValueSampleRange2).Font = new Font("Microsoft Sans Serif", 6.75f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)labelValueSampleRange2).Location = new Point(190, 60);
		((Control)labelValueSampleRange2).Name = "labelValueSampleRange2";
		((Control)labelValueSampleRange2).Size = new Size(229, 13);
		((Control)labelValueSampleRange2).TabIndex = 16;
		((Control)labelValueSampleRange2).Text = "123; My text; 567f; 456.23f; Test String; A";
		((Label)labelValueSampleRange2).TextAlign = (ContentAlignment)16;
		((Control)labelValueFrom).Location = new Point(12, 1);
		((Control)labelValueFrom).Name = "labelValueFrom";
		((Control)labelValueFrom).Size = new Size(121, 21);
		((Control)labelValueFrom).TabIndex = 11;
		((Control)labelValueFrom).Text = "From";
		((Label)labelValueFrom).TextAlign = (ContentAlignment)16;
		buttonEditDefaultValueFrom.AllowEscape = false;
		((Control)buttonEditDefaultValueFrom).Location = new Point(139, 1);
		((Control)buttonEditDefaultValueFrom).Name = "buttonEditDefaultValueFrom";
		((Control)buttonEditDefaultValueFrom).Size = new Size(237, 21);
		((Control)buttonEditDefaultValueFrom).TabIndex = 0;
		buttonEditDefaultValueFrom.ButtonClick += ButtonEditInitByValueFrom_Click;
		((Control)panelInitByExpressionRange).Controls.Add((Control)(object)textBoxInitByExpressionTo);
		((Control)panelInitByExpressionRange).Controls.Add((Control)(object)labelExpressionSampleRange);
		((Control)panelInitByExpressionRange).Controls.Add((Control)(object)labelExpressionSampleRange2);
		((Control)panelInitByExpressionRange).Controls.Add((Control)(object)textBoxInitByExpressionFrom);
		((Control)panelInitByExpressionRange).Controls.Add((Control)(object)labelExpressionTo);
		((Control)panelInitByExpressionRange).Controls.Add((Control)(object)labelExpressionFrom);
		((Control)panelInitByExpressionRange).Dock = (DockStyle)1;
		((Control)panelInitByExpressionRange).Location = new Point(0, 592);
		((Control)panelInitByExpressionRange).Name = "panelInitByExpressionRange";
		((Control)panelInitByExpressionRange).Size = new Size(479, 85);
		((Control)panelInitByExpressionRange).TabIndex = 7;
		textBoxInitByExpressionTo.AllowEscape = false;
		((Control)textBoxInitByExpressionTo).Location = new Point(139, 34);
		((Control)textBoxInitByExpressionTo).Name = "textBoxInitByExpressionTo";
		((Control)textBoxInitByExpressionTo).Size = new Size(237, 21);
		((Control)textBoxInitByExpressionTo).TabIndex = 2;
		textBoxInitByExpressionTo.ButtonClick += ButtonEditInitByExpressionTo_Click;
		((Control)labelExpressionSampleRange).Font = new Font("Microsoft Sans Serif", 6.75f, (FontStyle)1, (GraphicsUnit)3, (byte)204);
		((Control)labelExpressionSampleRange).Location = new Point(139, 60);
		((Control)labelExpressionSampleRange).Name = "labelExpressionSampleRange";
		((Control)labelExpressionSampleRange).Size = new Size(45, 13);
		((Control)labelExpressionSampleRange).TabIndex = 15;
		((Control)labelExpressionSampleRange).Text = "Sample:";
		((Label)labelExpressionSampleRange).TextAlign = (ContentAlignment)16;
		((Label)labelExpressionSampleRange2).AutoEllipsis = true;
		((Control)labelExpressionSampleRange2).Font = new Font("Microsoft Sans Serif", 6.75f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)labelExpressionSampleRange2).Location = new Point(190, 60);
		((Control)labelExpressionSampleRange2).Name = "labelExpressionSampleRange2";
		((Control)labelExpressionSampleRange2).Size = new Size(229, 13);
		((Control)labelExpressionSampleRange2).TabIndex = 14;
		((Control)labelExpressionSampleRange2).Text = "Data.Field+1; Var+Var2; float.Parse(MyVar); \"My Str\"";
		((Label)labelExpressionSampleRange2).TextAlign = (ContentAlignment)16;
		((Control)textBoxInitByExpressionFrom).Location = new Point(139, 1);
		((Control)textBoxInitByExpressionFrom).Name = "textBoxInitByExpressionFrom";
		((Control)textBoxInitByExpressionFrom).Size = new Size(237, 21);
		((Control)textBoxInitByExpressionFrom).TabIndex = 0;
		textBoxInitByExpressionFrom.ButtonClick += ButtonEditInitByExpressionFrom_Click;
		((Control)labelExpressionTo).Location = new Point(12, 35);
		((Control)labelExpressionTo).Name = "labelExpressionTo";
		((Control)labelExpressionTo).Size = new Size(121, 21);
		((Control)labelExpressionTo).TabIndex = 13;
		((Control)labelExpressionTo).Text = "To";
		((Label)labelExpressionTo).TextAlign = (ContentAlignment)16;
		((Control)labelExpressionFrom).Location = new Point(12, 1);
		((Control)labelExpressionFrom).Name = "labelExpressionFrom";
		((Control)labelExpressionFrom).Size = new Size(121, 21);
		((Control)labelExpressionFrom).TabIndex = 13;
		((Control)labelExpressionFrom).Text = "From";
		((Label)labelExpressionFrom).TextAlign = (ContentAlignment)16;
		((Control)panelDialogInfoDateTime).Controls.Add((Control)(object)labelDateTimeFormat);
		((Control)panelDialogInfoDateTime).Controls.Add((Control)(object)comboBoxDateTimeFormat);
		((Control)panelDialogInfoDateTime).Dock = (DockStyle)1;
		((Control)panelDialogInfoDateTime).Location = new Point(0, 1367);
		((Control)panelDialogInfoDateTime).Name = "panelDialogInfoDateTime";
		((Control)panelDialogInfoDateTime).Size = new Size(479, 36);
		((Control)panelDialogInfoDateTime).TabIndex = 15;
		((Label)labelDateTimeFormat).AutoEllipsis = true;
		((Control)labelDateTimeFormat).Location = new Point(12, 1);
		((Control)labelDateTimeFormat).Name = "labelDateTimeFormat";
		((Control)labelDateTimeFormat).Size = new Size(121, 21);
		((Control)labelDateTimeFormat).TabIndex = 18;
		((Control)labelDateTimeFormat).Text = "Date Time Format";
		((Label)labelDateTimeFormat).TextAlign = (ContentAlignment)16;
		comboBoxDateTimeFormat.DropDownStyle = (ComboBoxStyle)2;
		((ComboBox)comboBoxDateTimeFormat).Items.AddRange(new object[3] { "Simple", "Array", "Range" });
		((Control)comboBoxDateTimeFormat).Location = new Point(139, 1);
		((ComboBox)comboBoxDateTimeFormat).MaxDropDownItems = 30;
		((Control)comboBoxDateTimeFormat).Name = "comboBoxDateTimeFormat";
		((Control)comboBoxDateTimeFormat).Size = new Size(156, 21);
		((Control)comboBoxDateTimeFormat).TabIndex = 0;
		((ComboBox)comboBoxDateTimeFormat).SelectedIndexChanged += ComboBoxDateTimeFormat_SelectedIndexChanged;
		((Control)panelDialogInfoMask).Controls.Add((Control)(object)textBoxMask);
		((Control)panelDialogInfoMask).Controls.Add((Control)(object)labelMask);
		((Control)panelDialogInfoMask).Dock = (DockStyle)1;
		((Control)panelDialogInfoMask).Location = new Point(0, 1403);
		((Control)panelDialogInfoMask).Name = "panelDialogInfoMask";
		((Control)panelDialogInfoMask).Size = new Size(479, 37);
		((Control)panelDialogInfoMask).TabIndex = 16;
		((Control)textBoxMask).Location = new Point(139, 1);
		((Control)textBoxMask).Name = "textBoxMask";
		((Control)textBoxMask).Size = new Size(156, 21);
		((Control)textBoxMask).TabIndex = 0;
		((Control)labelMask).Location = new Point(12, 1);
		((Control)labelMask).Name = "labelMask";
		((Control)labelMask).Size = new Size(122, 20);
		((Control)labelMask).TabIndex = 18;
		((Control)labelMask).Text = "Mask";
		((Label)labelMask).TextAlign = (ContentAlignment)16;
		((Control)labelItems).Location = new Point(12, 1);
		((Control)labelItems).Name = "labelItems";
		((Control)labelItems).Size = new Size(119, 21);
		((Control)labelItems).TabIndex = 40;
		((Control)labelItems).Text = "Items";
		((Label)labelItems).TextAlign = (ContentAlignment)16;
		((Control)textBoxItems).Anchor = (AnchorStyles)13;
		((Control)textBoxItems).Location = new Point(139, 1);
		((TextBoxBase)textBoxItems).Multiline = true;
		((Control)textBoxItems).Name = "textBoxItems";
		((TextBoxBase)textBoxItems).ReadOnly = true;
		((Control)textBoxItems).Size = new Size(298, 57);
		((Control)textBoxItems).TabIndex = 1;
		((Control)buttonEditItems).Anchor = (AnchorStyles)9;
		((Control)buttonEditItems).Location = new Point(440, 0);
		((Control)buttonEditItems).Name = "buttonEditItems";
		((Control)buttonEditItems).Size = new Size(24, 25);
		((Control)buttonEditItems).TabIndex = 2;
		((ButtonBase)buttonEditItems).UseVisualStyleBackColor = true;
		((Control)buttonEditItems).Click += ButtonEditItems_Click;
		((Control)panelItems).Controls.Add((Control)(object)textBoxItems);
		((Control)panelItems).Controls.Add((Control)(object)buttonEditItems);
		((Control)panelItems).Controls.Add((Control)(object)labelItems);
		((Control)panelItems).Dock = (DockStyle)1;
		((Control)panelItems).Location = new Point(0, 1048);
		((Control)panelItems).Name = "panelItems";
		((Control)panelItems).Size = new Size(479, 70);
		((Control)panelItems).TabIndex = 13;
		((Control)panelColumns).Controls.Add((Control)(object)panelBindingValue);
		((Control)panelColumns).Controls.Add((Control)(object)panelKeysValues);
		((Control)panelColumns).Dock = (DockStyle)1;
		((Control)panelColumns).Location = new Point(0, 1118);
		((Control)panelColumns).Name = "panelColumns";
		((Control)panelColumns).Size = new Size(479, 180);
		((Control)panelColumns).TabIndex = 16;
		((Control)panelBindingValue).Controls.Add((Control)(object)checkBoxBindingValue);
		((Control)panelBindingValue).Dock = (DockStyle)2;
		((Control)panelBindingValue).Location = new Point(0, 150);
		((Control)panelBindingValue).Name = "panelBindingValue";
		((Control)panelBindingValue).Size = new Size(479, 30);
		((Control)panelBindingValue).TabIndex = 1;
		((Control)checkBoxBindingValue).AutoSize = true;
		((Control)checkBoxBindingValue).Location = new Point(139, 3);
		((Control)checkBoxBindingValue).Name = "checkBoxBindingValue";
		((Control)checkBoxBindingValue).Size = new Size(91, 17);
		((Control)checkBoxBindingValue).TabIndex = 1;
		((Control)checkBoxBindingValue).Text = "Binding Value";
		((ButtonBase)checkBoxBindingValue).TextAlign = (ContentAlignment)1;
		((ButtonBase)checkBoxBindingValue).UseVisualStyleBackColor = true;
		((CheckBox)checkBoxBindingValue).CheckedChanged += CheckBoxBindingValue_CheckedChanged;
		((Control)panelKeysValues).Controls.Add((Control)(object)labelCheckedSample);
		((Control)panelKeysValues).Controls.Add((Control)(object)label2);
		((Control)panelKeysValues).Controls.Add((Control)(object)buttonEditExpressionChecked);
		((Control)panelKeysValues).Controls.Add((Control)(object)textBoxCheckedExpression);
		((Control)panelKeysValues).Controls.Add((Control)(object)treeViewBoxCheckedDataSource);
		((Control)panelKeysValues).Controls.Add((Control)(object)labelChecked);
		((Control)panelKeysValues).Controls.Add((Control)(object)comboBoxSortDirection);
		((Control)panelKeysValues).Controls.Add((Control)(object)comboBoxSortField);
		((Control)panelKeysValues).Controls.Add((Control)(object)labelSortBy);
		((Control)panelKeysValues).Controls.Add((Control)(object)labelLabels);
		((Control)panelKeysValues).Controls.Add((Control)(object)columnKeys);
		((Control)panelKeysValues).Controls.Add((Control)(object)labelKeys);
		((Control)panelKeysValues).Controls.Add((Control)(object)columnValues);
		((Control)panelKeysValues).Dock = (DockStyle)1;
		((Control)panelKeysValues).Location = new Point(0, 0);
		((Control)panelKeysValues).Name = "panelKeysValues";
		((Control)panelKeysValues).Size = new Size(479, 150);
		((Control)panelKeysValues).TabIndex = 0;
		((Control)labelCheckedSample).Font = new Font("Microsoft Sans Serif", 6.75f, (FontStyle)1, (GraphicsUnit)3, (byte)204);
		((Control)labelCheckedSample).Location = new Point(139, 126);
		((Control)labelCheckedSample).Name = "labelCheckedSample";
		((Control)labelCheckedSample).Size = new Size(45, 13);
		((Control)labelCheckedSample).TabIndex = 45;
		((Control)labelCheckedSample).Text = "Sample:";
		((Label)labelCheckedSample).TextAlign = (ContentAlignment)16;
		((Label)label2).AutoEllipsis = true;
		((Control)label2).Font = new Font("Microsoft Sans Serif", 6.75f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)label2).Location = new Point(190, 126);
		((Control)label2).Name = "label2";
		((Control)label2).Size = new Size(274, 13);
		((Control)label2).TabIndex = 44;
		((Control)label2).Text = "Data.Field > 5; Data.Field == \"ABC\"; Data.BoolField";
		((Label)label2).TextAlign = (ContentAlignment)16;
		((Control)buttonEditExpressionChecked).Anchor = (AnchorStyles)9;
		((Control)buttonEditExpressionChecked).Location = new Point(440, 98);
		((Control)buttonEditExpressionChecked).Name = "buttonEditExpressionChecked";
		((Control)buttonEditExpressionChecked).Size = new Size(24, 25);
		((Control)buttonEditExpressionChecked).TabIndex = 6;
		((ButtonBase)buttonEditExpressionChecked).UseVisualStyleBackColor = true;
		((Control)buttonEditExpressionChecked).Click += ButtonEditExpressionChecked_Click;
		((Control)textBoxCheckedExpression).Location = new Point(281, 100);
		((Control)textBoxCheckedExpression).Name = "textBoxCheckedExpression";
		((Control)textBoxCheckedExpression).Size = new Size(156, 21);
		((Control)textBoxCheckedExpression).TabIndex = 5;
		((Control)treeViewBoxCheckedDataSource).Location = new Point(139, 100);
		((Control)treeViewBoxCheckedDataSource).Name = "treeViewBoxCheckedDataSource";
		((Control)treeViewBoxCheckedDataSource).Size = new Size(132, 21);
		((Control)treeViewBoxCheckedDataSource).TabIndex = 4;
		treeViewBoxCheckedDataSource.ValueChanged += TreeViewBoxCheckedDataSource_ValueChanged;
		treeViewBoxCheckedDataSource.TreeNodesFill += TreeViewBoxCheckedDataSource_TreeNodesFill;
		((Control)labelChecked).Location = new Point(12, 100);
		((Control)labelChecked).Name = "labelChecked";
		((Control)labelChecked).Size = new Size(121, 21);
		((Control)labelChecked).TabIndex = 43;
		((Control)labelChecked).Text = "Checked States";
		((Label)labelChecked).TextAlign = (ContentAlignment)16;
		comboBoxSortDirection.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxSortDirection).Location = new Point(139, 67);
		((ComboBox)comboBoxSortDirection).MaxDropDownItems = 30;
		((Control)comboBoxSortDirection).Name = "comboBoxSortDirection";
		((Control)comboBoxSortDirection).Size = new Size(132, 21);
		((Control)comboBoxSortDirection).TabIndex = 2;
		((ComboBox)comboBoxSortDirection).SelectedIndexChanged += ComboBoxSortDirection_SelectedIndexChanged;
		comboBoxSortField.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxSortField).Location = new Point(281, 67);
		((ComboBox)comboBoxSortField).MaxDropDownItems = 30;
		((Control)comboBoxSortField).Name = "comboBoxSortField";
		((Control)comboBoxSortField).Size = new Size(98, 21);
		((Control)comboBoxSortField).TabIndex = 3;
		((Control)labelSortBy).Location = new Point(12, 67);
		((Control)labelSortBy).Name = "labelSortBy";
		((Control)labelSortBy).Size = new Size(121, 21);
		((Control)labelSortBy).TabIndex = 42;
		((Control)labelSortBy).Text = "Sort by";
		((Label)labelSortBy).TextAlign = (ContentAlignment)16;
		((Control)labelLabels).Location = new Point(12, 37);
		((Control)labelLabels).Name = "labelLabels";
		((Control)labelLabels).Size = new Size(121, 21);
		((Control)labelLabels).TabIndex = 42;
		((Control)labelLabels).Text = "Values";
		((Label)labelLabels).TextAlign = (ContentAlignment)16;
		columnKeys.DropDownWidth = 400;
		((Control)columnKeys).Location = new Point(139, 1);
		columnKeys.MaxDropDownHeight = 300;
		((Control)columnKeys).Name = "columnKeys";
		((Control)columnKeys).Size = new Size(240, 21);
		((Control)columnKeys).TabIndex = 0;
		columnKeys.ValueChanged += ColumnKeys_ValueChanged;
		columnKeys.TreeNodesFill += ColumnKeys_TreeNodesFill;
		((Control)labelKeys).Location = new Point(12, 5);
		((Control)labelKeys).Name = "labelKeys";
		((Control)labelKeys).Size = new Size(121, 21);
		((Control)labelKeys).TabIndex = 41;
		((Control)labelKeys).Text = "Keys";
		((Label)labelKeys).TextAlign = (ContentAlignment)16;
		columnValues.DropDownWidth = 400;
		columnValues.Flat = false;
		((Control)columnValues).Location = new Point(139, 34);
		columnValues.MaxDropDownHeight = 300;
		((Control)columnValues).Name = "columnValues";
		((Control)columnValues).Size = new Size(240, 21);
		((Control)columnValues).TabIndex = 1;
		columnValues.ValueChanged += ColumnKeys_ValueChanged;
		columnValues.TreeNodesFill += ColumnKeys_TreeNodesFill;
		((Control)panelBindingColumns).Controls.Add((Control)(object)labelDependentColumn);
		((Control)panelBindingColumns).Controls.Add((Control)(object)comboBoxBindingValue);
		((Control)panelBindingColumns).Controls.Add((Control)(object)labelVariable);
		((Control)panelBindingColumns).Controls.Add((Control)(object)columnBindingColumn);
		((Control)panelBindingColumns).Dock = (DockStyle)1;
		((Control)panelBindingColumns).Location = new Point(0, 1298);
		((Control)panelBindingColumns).Name = "panelBindingColumns";
		((Control)panelBindingColumns).Size = new Size(479, 69);
		((Control)panelBindingColumns).TabIndex = 14;
		((Control)labelDependentColumn).Location = new Point(12, 36);
		((Control)labelDependentColumn).Name = "labelDependentColumn";
		((Control)labelDependentColumn).Size = new Size(121, 21);
		((Control)labelDependentColumn).TabIndex = 40;
		((Control)labelDependentColumn).Text = "Binding Column";
		((Label)labelDependentColumn).TextAlign = (ContentAlignment)16;
		comboBoxBindingValue.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxBindingValue).Location = new Point(139, 1);
		((ComboBox)comboBoxBindingValue).MaxDropDownItems = 30;
		((Control)comboBoxBindingValue).Name = "comboBoxBindingValue";
		((Control)comboBoxBindingValue).Size = new Size(240, 21);
		((Control)comboBoxBindingValue).TabIndex = 0;
		((Control)labelVariable).Location = new Point(12, 1);
		((Control)labelVariable).Name = "labelVariable";
		((Control)labelVariable).Size = new Size(121, 21);
		((Control)labelVariable).TabIndex = 39;
		((Control)labelVariable).Text = "Variable";
		((Label)labelVariable).TextAlign = (ContentAlignment)16;
		((Control)columnBindingColumn).Location = new Point(139, 34);
		((Control)columnBindingColumn).Name = "columnBindingColumn";
		((Control)columnBindingColumn).Size = new Size(241, 21);
		((Control)columnBindingColumn).TabIndex = 1;
		columnBindingColumn.ValueChanged += ColumnKeys_ValueChanged;
		columnBindingColumn.TreeNodesFill += ColumnKeys_TreeNodesFill;
		((Control)panelInitByTimeSpanRange).Controls.Add((Control)(object)timeSpanPickerDefaultValueTo);
		((Control)panelInitByTimeSpanRange).Controls.Add((Control)(object)timeSpanPickerDefaultValueFrom);
		((Control)panelInitByTimeSpanRange).Controls.Add((Control)(object)checkBoxTimeSpanToNotAssigned);
		((Control)panelInitByTimeSpanRange).Controls.Add((Control)(object)checkBoxTimeSpanFromNotAssigned);
		((Control)panelInitByTimeSpanRange).Controls.Add((Control)(object)labelTimeSpanValueTo);
		((Control)panelInitByTimeSpanRange).Controls.Add((Control)(object)labelTimeSpanValueFrom);
		((Control)panelInitByTimeSpanRange).Dock = (DockStyle)1;
		((Control)panelInitByTimeSpanRange).Location = new Point(0, 363);
		((Control)panelInitByTimeSpanRange).Name = "panelInitByTimeSpanRange";
		((Control)panelInitByTimeSpanRange).Size = new Size(479, 70);
		((Control)panelInitByTimeSpanRange).TabIndex = 19;
		((Control)timeSpanPickerDefaultValueTo).Location = new Point(139, 33);
		((Control)timeSpanPickerDefaultValueTo).Name = "timeSpanPickerDefaultValueTo";
		((Control)timeSpanPickerDefaultValueTo).Padding = new Padding(2);
		timeSpanPickerDefaultValueTo.ReadOnly = false;
		((Control)timeSpanPickerDefaultValueTo).Size = new Size(154, 21);
		((Control)timeSpanPickerDefaultValueTo).TabIndex = 11;
		timeSpanPickerDefaultValueTo.Value = TimeSpan.Parse("00:00:00");
		((Control)timeSpanPickerDefaultValueFrom).Location = new Point(139, 1);
		((Control)timeSpanPickerDefaultValueFrom).Name = "timeSpanPickerDefaultValueFrom";
		((Control)timeSpanPickerDefaultValueFrom).Padding = new Padding(2);
		timeSpanPickerDefaultValueFrom.ReadOnly = false;
		((Control)timeSpanPickerDefaultValueFrom).Size = new Size(154, 21);
		((Control)timeSpanPickerDefaultValueFrom).TabIndex = 10;
		timeSpanPickerDefaultValueFrom.Value = TimeSpan.Parse("00:00:00");
		((Control)checkBoxTimeSpanToNotAssigned).Anchor = (AnchorStyles)13;
		((Control)checkBoxTimeSpanToNotAssigned).BackColor = Color.Transparent;
		((Control)checkBoxTimeSpanToNotAssigned).Location = new Point(301, 34);
		((Control)checkBoxTimeSpanToNotAssigned).Name = "checkBoxTimeSpanToNotAssigned";
		((Control)checkBoxTimeSpanToNotAssigned).Size = new Size(171, 20);
		((Control)checkBoxTimeSpanToNotAssigned).TabIndex = 3;
		((Control)checkBoxTimeSpanToNotAssigned).Text = "Not assigned";
		((ButtonBase)checkBoxTimeSpanToNotAssigned).UseVisualStyleBackColor = false;
		((Control)checkBoxTimeSpanToNotAssigned).Visible = false;
		((Control)checkBoxTimeSpanFromNotAssigned).Anchor = (AnchorStyles)13;
		((Control)checkBoxTimeSpanFromNotAssigned).BackColor = Color.Transparent;
		((Control)checkBoxTimeSpanFromNotAssigned).Location = new Point(301, 2);
		((Control)checkBoxTimeSpanFromNotAssigned).Name = "checkBoxTimeSpanFromNotAssigned";
		((Control)checkBoxTimeSpanFromNotAssigned).Size = new Size(171, 20);
		((Control)checkBoxTimeSpanFromNotAssigned).TabIndex = 1;
		((Control)checkBoxTimeSpanFromNotAssigned).Text = "Not assigned";
		((ButtonBase)checkBoxTimeSpanFromNotAssigned).UseVisualStyleBackColor = false;
		((Control)checkBoxTimeSpanFromNotAssigned).Visible = false;
		((Control)labelTimeSpanValueTo).Location = new Point(12, 33);
		((Control)labelTimeSpanValueTo).Name = "labelTimeSpanValueTo";
		((Control)labelTimeSpanValueTo).Size = new Size(121, 22);
		((Control)labelTimeSpanValueTo).TabIndex = 8;
		((Control)labelTimeSpanValueTo).Text = "To";
		((Label)labelTimeSpanValueTo).TextAlign = (ContentAlignment)16;
		((Control)labelTimeSpanValueFrom).Location = new Point(12, 1);
		((Control)labelTimeSpanValueFrom).Name = "labelTimeSpanValueFrom";
		((Control)labelTimeSpanValueFrom).Size = new Size(121, 21);
		((Control)labelTimeSpanValueFrom).TabIndex = 8;
		((Control)labelTimeSpanValueFrom).Text = "From";
		((Label)labelTimeSpanValueFrom).TextAlign = (ContentAlignment)16;
		((Control)panelInitByTimeSpan).Controls.Add((Control)(object)timeSpanPickerDefaultValue);
		((Control)panelInitByTimeSpan).Controls.Add((Control)(object)checkBoxTimeSpanNotAssigned);
		((Control)panelInitByTimeSpan).Controls.Add((Control)(object)labelDefaultTimeSpanValue);
		((Control)panelInitByTimeSpan).Dock = (DockStyle)1;
		((Control)panelInitByTimeSpan).Location = new Point(0, 325);
		((Control)panelInitByTimeSpan).Name = "panelInitByTimeSpan";
		((Control)panelInitByTimeSpan).Size = new Size(479, 38);
		((Control)panelInitByTimeSpan).TabIndex = 18;
		((Control)timeSpanPickerDefaultValue).Location = new Point(139, 1);
		((Control)timeSpanPickerDefaultValue).Name = "timeSpanPickerDefaultValue";
		((Control)timeSpanPickerDefaultValue).Padding = new Padding(2, 2, 0, 0);
		timeSpanPickerDefaultValue.ReadOnly = false;
		((Control)timeSpanPickerDefaultValue).Size = new Size(154, 21);
		((Control)timeSpanPickerDefaultValue).TabIndex = 9;
		timeSpanPickerDefaultValue.Value = TimeSpan.Parse("00:00:00");
		((Control)checkBoxTimeSpanNotAssigned).Anchor = (AnchorStyles)13;
		((Control)checkBoxTimeSpanNotAssigned).BackColor = Color.Transparent;
		((Control)checkBoxTimeSpanNotAssigned).Location = new Point(301, 1);
		((Control)checkBoxTimeSpanNotAssigned).Name = "checkBoxTimeSpanNotAssigned";
		((Control)checkBoxTimeSpanNotAssigned).Size = new Size(171, 20);
		((Control)checkBoxTimeSpanNotAssigned).TabIndex = 0;
		((Control)checkBoxTimeSpanNotAssigned).Text = "Not assigned";
		((ButtonBase)checkBoxTimeSpanNotAssigned).UseVisualStyleBackColor = false;
		((Control)checkBoxTimeSpanNotAssigned).Visible = false;
		((Control)labelDefaultTimeSpanValue).Location = new Point(12, 1);
		((Control)labelDefaultTimeSpanValue).Name = "labelDefaultTimeSpanValue";
		((Control)labelDefaultTimeSpanValue).Size = new Size(121, 20);
		((Control)labelDefaultTimeSpanValue).TabIndex = 8;
		((Control)labelDefaultTimeSpanValue).Text = "Value";
		((Label)labelDefaultTimeSpanValue).TextAlign = (ContentAlignment)16;
		((Control)buttonOk).Anchor = (AnchorStyles)10;
		((Control)buttonOk).Location = new Point(309, 12);
		((Control)buttonOk).Name = "buttonOk";
		((Control)buttonOk).Size = new Size(75, 24);
		((Control)buttonOk).TabIndex = 1;
		((Control)buttonOk).Text = "Ok";
		((ButtonBase)buttonOk).UseVisualStyleBackColor = true;
		((Control)buttonOk).Click += This_OkClick;
		((Control)buttonCancel).Anchor = (AnchorStyles)10;
		((Button)buttonCancel).DialogResult = (DialogResult)2;
		((Control)buttonCancel).Location = new Point(390, 12);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(75, 24);
		((Control)buttonCancel).TabIndex = 2;
		((Control)buttonCancel).Text = "Cancel";
		((ButtonBase)buttonCancel).UseVisualStyleBackColor = true;
		((Control)buttonCancel).Click += This_CancelClick;
		((Control)panelSelection).Controls.Add((Control)(object)labelSelection);
		((Control)panelSelection).Controls.Add((Control)(object)comboBoxSelection);
		((Control)panelSelection).Dock = (DockStyle)1;
		panelSelection.LinePosition = StiLinePosition.None;
		panelSelection.LineStyle = StiLineStyle.Line;
		((Control)panelSelection).Location = new Point(0, 1014);
		((Control)panelSelection).Name = "panelSelection";
		((Control)panelSelection).Size = new Size(479, 34);
		((Control)panelSelection).TabIndex = 12;
		((Control)labelSelection).Location = new Point(12, 1);
		((Control)labelSelection).Name = "labelSelection";
		((Control)labelSelection).Size = new Size(120, 21);
		((Control)labelSelection).TabIndex = 39;
		((Control)labelSelection).Text = "Selection";
		((Label)labelSelection).TextAlign = (ContentAlignment)16;
		comboBoxSelection.DropDownStyle = (ComboBoxStyle)2;
		((ComboBox)comboBoxSelection).Items.AddRange(new object[2] { "Items", "Columns" });
		((Control)comboBoxSelection).Location = new Point(139, 1);
		((ComboBox)comboBoxSelection).MaxDropDownItems = 30;
		((Control)comboBoxSelection).Name = "comboBoxSelection";
		((Control)comboBoxSelection).Size = new Size(132, 21);
		((Control)comboBoxSelection).TabIndex = 0;
		((ComboBox)comboBoxSelection).SelectedIndexChanged += ComboBoxSelection_SelectedIndexChanged;
		((Control)panelDialogItems).Controls.Add((Control)(object)labelDataSource);
		((Control)panelDialogItems).Controls.Add((Control)(object)comboBoxDataSource);
		((Control)panelDialogItems).Controls.Add((Control)(object)checkBoxAllowUserValues);
		((Control)panelDialogItems).Dock = (DockStyle)1;
		panelDialogItems.LinePosition = StiLinePosition.Top;
		panelDialogItems.LineStyle = StiLineStyle.Dot;
		((Control)panelDialogItems).Location = new Point(0, 936);
		((Control)panelDialogItems).Name = "panelDialogItems";
		((Control)panelDialogItems).Size = new Size(479, 78);
		((Control)panelDialogItems).TabIndex = 11;
		((Control)labelDataSource).Location = new Point(12, 45);
		((Control)labelDataSource).Name = "labelDataSource";
		((Control)labelDataSource).Size = new Size(121, 21);
		((Control)labelDataSource).TabIndex = 39;
		((Control)labelDataSource).Text = "Data Source";
		((Label)labelDataSource).TextAlign = (ContentAlignment)16;
		comboBoxDataSource.DropDownStyle = (ComboBoxStyle)2;
		((ComboBox)comboBoxDataSource).Items.AddRange(new object[2] { "Items", "Columns" });
		((Control)comboBoxDataSource).Location = new Point(139, 45);
		((ComboBox)comboBoxDataSource).MaxDropDownItems = 30;
		((Control)comboBoxDataSource).Name = "comboBoxDataSource";
		((Control)comboBoxDataSource).Size = new Size(132, 21);
		((Control)comboBoxDataSource).TabIndex = 1;
		((ComboBox)comboBoxDataSource).SelectedIndexChanged += ComboBoxDataSource_SelectedIndexChanged;
		((Control)checkBoxAllowUserValues).BackColor = Color.Transparent;
		((Control)checkBoxAllowUserValues).Location = new Point(139, 12);
		((Control)checkBoxAllowUserValues).Name = "checkBoxAllowUserValues";
		((Control)checkBoxAllowUserValues).Size = new Size(335, 20);
		((Control)checkBoxAllowUserValues).TabIndex = 0;
		((Control)checkBoxAllowUserValues).Text = "Allow User Values";
		((ButtonBase)checkBoxAllowUserValues).UseVisualStyleBackColor = false;
		((CheckBox)checkBoxAllowUserValues).CheckedChanged += CheckBoxAllowUserValues_CheckedChanged;
		((Control)panelSettings).Controls.Add((Control)(object)checkBoxAllowUsingAsSqlParameter);
		((Control)panelSettings).Controls.Add((Control)(object)checkBoxReadOnly);
		((Control)panelSettings).Controls.Add((Control)(object)checkBoxRequestFromUser);
		((Control)panelSettings).Dock = (DockStyle)1;
		panelSettings.LinePosition = StiLinePosition.Top;
		panelSettings.LineStyle = StiLineStyle.Dot;
		((Control)panelSettings).Location = new Point(0, 828);
		((Control)panelSettings).Name = "panelSettings";
		((Control)panelSettings).Size = new Size(479, 108);
		((Control)panelSettings).TabIndex = 10;
		((Control)checkBoxAllowUsingAsSqlParameter).BackColor = Color.Transparent;
		((Control)checkBoxAllowUsingAsSqlParameter).Location = new Point(139, 76);
		((Control)checkBoxAllowUsingAsSqlParameter).Name = "checkBoxAllowUsingAsSqlParameter";
		((Control)checkBoxAllowUsingAsSqlParameter).Size = new Size(316, 20);
		((Control)checkBoxAllowUsingAsSqlParameter).TabIndex = 2;
		((Control)checkBoxAllowUsingAsSqlParameter).Text = "Allow using as SQL parameter";
		((ButtonBase)checkBoxAllowUsingAsSqlParameter).UseVisualStyleBackColor = false;
		((Control)checkBoxReadOnly).BackColor = Color.Transparent;
		((Control)checkBoxReadOnly).Location = new Point(139, 12);
		((Control)checkBoxReadOnly).Name = "checkBoxReadOnly";
		((Control)checkBoxReadOnly).Size = new Size(316, 20);
		((Control)checkBoxReadOnly).TabIndex = 0;
		((Control)checkBoxReadOnly).Text = "ReadOnly";
		((ButtonBase)checkBoxReadOnly).UseVisualStyleBackColor = false;
		((CheckBox)checkBoxReadOnly).CheckedChanged += CheckBoxReadOnly_CheckedChanged;
		((Control)checkBoxRequestFromUser).BackColor = Color.Transparent;
		((Control)checkBoxRequestFromUser).Location = new Point(139, 44);
		((Control)checkBoxRequestFromUser).Name = "checkBoxRequestFromUser";
		((Control)checkBoxRequestFromUser).Size = new Size(316, 20);
		((Control)checkBoxRequestFromUser).TabIndex = 1;
		((Control)checkBoxRequestFromUser).Text = "Request From User";
		((ButtonBase)checkBoxRequestFromUser).UseVisualStyleBackColor = false;
		((CheckBox)checkBoxRequestFromUser).CheckedChanged += CheckBoxRequestFromUser_CheckedChanged;
		((Control)panelInitByImage).Controls.Add((Control)(object)buttonImageRemove);
		((Control)panelInitByImage).Controls.Add((Control)(object)buttonImageOpen);
		((Control)panelInitByImage).Controls.Add((Control)(object)labelImage);
		((Control)panelInitByImage).Controls.Add((Control)(object)panelImage);
		((Control)panelInitByImage).Dock = (DockStyle)1;
		panelInitByImage.LinePosition = StiLinePosition.Top;
		panelInitByImage.LineStyle = StiLineStyle.Dot;
		((Control)panelInitByImage).Location = new Point(0, 712);
		((Control)panelInitByImage).Name = "panelInitByImage";
		((Control)panelInitByImage).Size = new Size(479, 116);
		((Control)panelInitByImage).TabIndex = 9;
		((Control)buttonImageRemove).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonImageRemove).Image = (Image)componentResourceManager.GetObject("buttonImageRemove.Image");
		((Control)buttonImageRemove).Location = new Point(437, 47);
		((Control)buttonImageRemove).Name = "buttonImageRemove";
		((Control)buttonImageRemove).Size = new Size(24, 24);
		((Control)buttonImageRemove).TabIndex = 20;
		((ButtonBase)buttonImageRemove).UseVisualStyleBackColor = true;
		((Control)buttonImageRemove).Click += ButtonRemoveImage_Click;
		((Control)buttonImageOpen).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonImageOpen).Image = (Image)componentResourceManager.GetObject("buttonImageOpen.Image");
		((Control)buttonImageOpen).Location = new Point(437, 18);
		((Control)buttonImageOpen).Name = "buttonImageOpen";
		((Control)buttonImageOpen).Size = new Size(24, 24);
		((Control)buttonImageOpen).TabIndex = 19;
		((ButtonBase)buttonImageOpen).UseVisualStyleBackColor = true;
		((Control)buttonImageOpen).Click += ButtonImageOpen_Click;
		((Control)labelImage).Location = new Point(12, 12);
		((Control)labelImage).Name = "labelImage";
		((Control)labelImage).Size = new Size(121, 24);
		((Control)labelImage).TabIndex = 12;
		((Control)labelImage).Text = "Image";
		((Label)labelImage).TextAlign = (ContentAlignment)16;
		((Control)panelImage).AllowDrop = true;
		((Control)panelImage).Anchor = (AnchorStyles)15;
		((Control)panelImage).Location = new Point(139, 12);
		((Control)panelImage).Name = "panelImage";
		((Control)panelImage).Size = new Size(328, 92);
		((Control)panelImage).TabIndex = 26;
		((Control)panelImage).DragDrop += new DragEventHandler(PanelImage_DragDrop);
		((Control)panelImage).DragOver += new DragEventHandler(PanelImage_DragOver);
		((Control)panelImage).Paint += new PaintEventHandler(PanelImage_Paint);
		((Control)panelImage).DoubleClick += PanelImage_DoubleClick;
		((Control)panelInitByBool).Controls.Add((Control)(object)checkBoxDefaultValue);
		((Control)panelInitByBool).Dock = (DockStyle)1;
		panelInitByBool.LinePosition = StiLinePosition.None;
		panelInitByBool.LineStyle = StiLineStyle.Dot;
		((Control)panelInitByBool).Location = new Point(0, 677);
		((Control)panelInitByBool).Name = "panelInitByBool";
		((Control)panelInitByBool).Size = new Size(479, 35);
		((Control)panelInitByBool).TabIndex = 8;
		((Control)checkBoxDefaultValue).Anchor = (AnchorStyles)13;
		((Control)checkBoxDefaultValue).BackColor = Color.Transparent;
		((Control)checkBoxDefaultValue).Location = new Point(139, 1);
		((Control)checkBoxDefaultValue).Name = "checkBoxDefaultValue";
		((Control)checkBoxDefaultValue).Size = new Size(340, 20);
		((Control)checkBoxDefaultValue).TabIndex = 0;
		((Control)checkBoxDefaultValue).Text = "Dependent Value";
		((ButtonBase)checkBoxDefaultValue).UseVisualStyleBackColor = false;
		((Control)groupButtons).BackColor = Color.White;
		((Control)groupButtons).Controls.Add((Control)(object)buttonSaveCopy);
		((Control)groupButtons).Controls.Add((Control)(object)buttonOk);
		((Control)groupButtons).Controls.Add((Control)(object)buttonCancel);
		((Control)groupButtons).Dock = (DockStyle)2;
		groupButtons.LinePosition = StiLinePosition.Top;
		groupButtons.LineStyle = StiLineStyle.Dot;
		((Control)groupButtons).Location = new Point(0, 1015);
		((Control)groupButtons).Name = "groupButtons";
		((Control)groupButtons).Size = new Size(479, 46);
		((Control)groupButtons).TabIndex = 17;
		((Control)buttonSaveCopy).Anchor = (AnchorStyles)6;
		((Control)buttonSaveCopy).Location = new Point(12, 12);
		((Control)buttonSaveCopy).Name = "buttonSaveCopy";
		((Control)buttonSaveCopy).Size = new Size(93, 24);
		((Control)buttonSaveCopy).TabIndex = 0;
		((Control)buttonSaveCopy).Text = "Save a Copy";
		((ButtonBase)buttonSaveCopy).UseVisualStyleBackColor = true;
		((Control)buttonSaveCopy).Visible = false;
		((Control)buttonSaveCopy).Click += ButtonSaveCopy_Click;
		((Control)panelInitBy).Controls.Add((Control)(object)labelInitBy);
		((Control)panelInitBy).Controls.Add((Control)(object)comboBoxInitBy);
		((Control)panelInitBy).Dock = (DockStyle)1;
		panelInitBy.LinePosition = StiLinePosition.Top;
		panelInitBy.LineStyle = StiLineStyle.Dot;
		((Control)panelInitBy).Location = new Point(0, 143);
		((Control)panelInitBy).Name = "panelInitBy";
		((Control)panelInitBy).Size = new Size(479, 46);
		((Control)panelInitBy).TabIndex = 1;
		((Control)labelInitBy).Location = new Point(12, 12);
		((Control)labelInitBy).Name = "labelInitBy";
		((Control)labelInitBy).Size = new Size(121, 21);
		((Control)labelInitBy).TabIndex = 18;
		((Control)labelInitBy).Text = "Init by";
		((Label)labelInitBy).TextAlign = (ContentAlignment)16;
		comboBoxInitBy.DropDownStyle = (ComboBoxStyle)2;
		((ComboBox)comboBoxInitBy).Items.AddRange(new object[3] { "Simple", "Array", "Range" });
		((Control)comboBoxInitBy).Location = new Point(139, 12);
		((ComboBox)comboBoxInitBy).MaxDropDownItems = 30;
		((Control)comboBoxInitBy).Name = "comboBoxInitBy";
		((Control)comboBoxInitBy).Size = new Size(156, 21);
		((Control)comboBoxInitBy).TabIndex = 0;
		((ComboBox)comboBoxInitBy).SelectedIndexChanged += ComboBoxInitBy_SelectedIndexChanged;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(479, 1061);
		((Control)this).Controls.Add((Control)(object)panelDialogInfoMask);
		((Control)this).Controls.Add((Control)(object)panelDialogInfoDateTime);
		((Control)this).Controls.Add((Control)(object)panelBindingColumns);
		((Control)this).Controls.Add((Control)(object)panelColumns);
		((Control)this).Controls.Add((Control)(object)panelItems);
		((Control)this).Controls.Add((Control)(object)panelSelection);
		((Control)this).Controls.Add((Control)(object)panelDialogItems);
		((Control)this).Controls.Add((Control)(object)panelSettings);
		((Control)this).Controls.Add((Control)(object)panelInitByImage);
		((Control)this).Controls.Add((Control)(object)panelInitByBool);
		((Control)this).Controls.Add((Control)(object)panelInitByExpressionRange);
		((Control)this).Controls.Add((Control)(object)panelInitByExpression);
		((Control)this).Controls.Add((Control)(object)groupButtons);
		((Control)this).Controls.Add((Control)(object)panelInitByDateTimeRange);
		((Control)this).Controls.Add((Control)(object)panelInitByDateTime);
		((Control)this).Controls.Add((Control)(object)panelInitByTimeSpanRange);
		((Control)this).Controls.Add((Control)(object)panelInitByTimeSpan);
		((Control)this).Controls.Add((Control)(object)panelInitByValueRange);
		((Control)this).Controls.Add((Control)(object)panelInitByValue);
		((Control)this).Controls.Add((Control)(object)panelInitBy);
		((Control)this).Controls.Add((Control)(object)panelMain);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiVariableEditForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Edit variable";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Form)this).Load += This_Load;
		((ISupportInitialize)errorProvider).EndInit();
		((Control)panelMain).ResumeLayout(false);
		((Control)panelInitByDateTime).ResumeLayout(false);
		((Control)panelInitByValue).ResumeLayout(false);
		((Control)panelInitByExpression).ResumeLayout(false);
		((Control)panelInitByDateTimeRange).ResumeLayout(false);
		((Control)panelInitByValueRange).ResumeLayout(false);
		((Control)panelInitByExpressionRange).ResumeLayout(false);
		((Control)panelDialogInfoDateTime).ResumeLayout(false);
		((Control)panelDialogInfoMask).ResumeLayout(false);
		((Control)panelItems).ResumeLayout(false);
		((Control)panelItems).PerformLayout();
		((Control)panelColumns).ResumeLayout(false);
		((Control)panelBindingValue).ResumeLayout(false);
		((Control)panelBindingValue).PerformLayout();
		((Control)panelKeysValues).ResumeLayout(false);
		((Control)panelBindingColumns).ResumeLayout(false);
		((Control)panelInitByTimeSpanRange).ResumeLayout(false);
		((Control)panelInitByTimeSpan).ResumeLayout(false);
		((Control)panelSelection).ResumeLayout(false);
		((Control)panelDialogItems).ResumeLayout(false);
		((Control)panelSettings).ResumeLayout(false);
		((Control)panelInitByImage).ResumeLayout(false);
		((Control)panelInitByBool).ResumeLayout(false);
		((Control)groupButtons).ResumeLayout(false);
		((Control)panelInitBy).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}

	DialogResult IStiVariableEditForm.get_DialogResult()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return ((Form)this).DialogResult;
	}

	void IStiVariableEditForm.set_DialogResult(DialogResult value)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		((Form)this).DialogResult = value;
	}

	DialogResult IStiVariableEditForm.ShowDialog()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return ((Form)this).ShowDialog();
	}
}
