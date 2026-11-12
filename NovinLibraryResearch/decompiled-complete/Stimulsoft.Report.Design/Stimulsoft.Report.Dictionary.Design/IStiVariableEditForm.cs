using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Report.Dictionary.Design;

public interface IStiVariableEditForm : IDisposable
{
	bool IsVariableDefaultValueIgnored { get; }

	string Text { get; set; }

	string VariableKeysColumn { get; set; }

	string VariableValuesColumn { get; set; }

	string VariableCheckedColumn { get; set; }

	string VariableName { get; set; }

	string VariableAlias { get; set; }

	string VariableDescription { get; set; }

	Type VariableType { get; set; }

	StiItemsInitializationType VariableDataSourceInitializationType { get; set; }

	Image VariableImage { get; set; }

	StiSelectionMode VariableSelectionMode { get; set; }

	StiVariableInitBy VariableInitBy { get; set; }

	StiDateTimeType VariableDateTimeType { get; set; }

	string VariableInitByExpression { get; set; }

	string VariableInitByExpressionFrom { get; set; }

	string VariableInitByExpressionTo { get; set; }

	string VariableMask { get; set; }

	bool VariableBindingValue { get; set; }

	StiVariable VariableBindingVariable { get; set; }

	bool VariableAllowUsingAsSqlParameter { get; set; }

	bool VariableAllowUserValues { get; set; }

	bool VariableReadOnly { get; set; }

	bool VariableRequestFromUser { get; set; }

	bool VariableNotAssigned { get; set; }

	bool VariableNotAssignedFrom { get; set; }

	bool VariableNotAssignedTo { get; set; }

	bool VariableTimeSpanNotAssigned { get; set; }

	bool VariableTimeSpanNotAssignedFrom { get; set; }

	bool VariableTimeSpanNotAssignedTo { get; set; }

	string VariableDefaultValue { get; set; }

	string VariableDefaultValueFrom { get; set; }

	string VariableDefaultValueTo { get; set; }

	DateTime VariableDateTimeDefaultValue { get; set; }

	DateTime VariableDateTimeDefaultValueFrom { get; set; }

	DateTime VariableDateTimeDefaultValueTo { get; set; }

	TimeSpan VariableTimeSpanDefaultValue { get; set; }

	TimeSpan VariableTimeSpanDefaultValueFrom { get; set; }

	TimeSpan VariableTimeSpanDefaultValueTo { get; set; }

	bool VariableBoolDefaultValue { get; set; }

	string VariableBindingColumn { get; set; }

	StiVariableSortDirection VariableSortDirection { get; set; }

	StiVariableSortField VariableSortField { get; set; }

	List<StiDialogInfoItem> Items { get; set; }

	bool CopyModeActivated { get; set; }

	DialogResult DialogResult { get; set; }

	List<StiVariable> GetBindingVariables();

	DialogResult ShowDialog();

	void SetBaseName(string name);
}
