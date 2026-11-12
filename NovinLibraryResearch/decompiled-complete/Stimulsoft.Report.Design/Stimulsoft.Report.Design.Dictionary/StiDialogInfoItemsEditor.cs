using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Design.Helpers;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Help;

namespace Stimulsoft.Report.Design.Dictionary;

public class StiDialogInfoItemsEditor : StiFormUX
{
	private const string ColumnChecked = "Checked";

	private const string ColumnKey = "Key";

	private const string ColumnKeyTo = "KeyTo";

	private const string ColumnLabel = "Label";

	private const string ColumnExpression = "Expression";

	private List<StiDialogInfoItem> items;

	private Type variableType;

	private StiVariableEditForm editForm;

	private IContainer components;

	private StiButtonPanelUX groupButtons;

	private StiButtonUX buttonOk;

	private StiButtonUX buttonCancel;

	private StiExcelGrid gridItems;

	private StiButtonUX buttonSelectColumns;

	private bool IsImageType => variableType == typeof(Image);

	private bool IsRangeType => StiTypeFinder.FindType(variableType, typeof(Range));

	private bool IsCheckable => !editForm.VariableAllowUserValues;

	public StiDialogInfoItemsEditor(List<StiDialogInfoItem> items, Type variableType, StiVariableEditForm editForm)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			if (StiOptions.Appearance.ShowFormsHelp)
			{
				((Form)this).HelpButton = true;
			}
			this.editForm = editForm;
			Localize();
			this.items = items;
			this.variableType = variableType;
			gridItems.EmptyKeys = new string[3] { "Key", "KeyTo", "Label" };
			((DataGridView)gridItems).BorderStyle = (BorderStyle)0;
			StiSettings.Load();
			StiFormSettings.Load((Form)(object)this);
			((Form)this).ShowIcon = false;
			((Control)buttonSelectColumns).Visible = !IsRangeType;
		}
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("PropertyMain", "Items");
		((Control)buttonOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)buttonCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		((Control)buttonSelectColumns).Text = StiHint.CreateHint(StiLocalization.Get("Wizards", "SelectColumns"));
		((Control)buttonSelectColumns).Width = TextRenderer.MeasureText(((Control)buttonSelectColumns).Text, ((Control)buttonSelectColumns).Font).Width + StiScale.I20;
		StiExcelGridLocalizer.Localize(gridItems);
	}

	internal List<StiDialogInfoItem> GetItems()
	{
		DataTable dataTable = gridItems.DataTable;
		if (dataTable == null)
		{
			return null;
		}
		List<StiDialogInfoItem> list = new List<StiDialogInfoItem>();
		foreach (DataRow row in dataTable.Rows)
		{
			bool flag = ((!IsImageType) ? string.IsNullOrWhiteSpace(ReadKey(row)) : (row["Key"] == null || row["Key"] == DBNull.Value));
			bool flag2 = !IsRangeType || string.IsNullOrWhiteSpace(ReadKeyTo(row));
			bool flag3 = string.IsNullOrWhiteSpace(ReadLabel(row));
			if (!(flag && flag2 && flag3))
			{
				bool flag4 = StiValueHelper.TryToBool(row["Expression"]);
				if (flag4 && IsRangeType)
				{
					list.Add(new StiExpressionRangeDialogInfoItem
					{
						Checked = ReadChecked(row),
						KeyObject = ReadKey(row),
						KeyObjectTo = ReadKeyTo(row),
						Label = ReadLabel(row)
					});
				}
				else if (flag4 && !IsRangeType)
				{
					list.Add(new StiExpressionDialogInfoItem
					{
						Checked = ReadChecked(row),
						KeyObject = ReadKey(row),
						Label = ReadLabel(row)
					});
				}
				else
				{
					list.Add(NewDialogInfoItem(row));
				}
			}
		}
		return list;
	}

	private void Build()
	{
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		((DataGridView)gridItems).Columns.Clear();
		DataTable dataTable = new DataTable().AddColumn("Checked", typeof(bool)).AddColumn("Key", IsImageType ? typeof(Image) : typeof(string)).AddColumn("KeyTo", typeof(string))
			.AddColumn("Label", typeof(string))
			.AddColumn("Expression", typeof(bool));
		((DataGridView)gridItems).DataSource = dataTable;
		foreach (StiDialogInfoItem item in items)
		{
			bool flag = item is StiExpressionDialogInfoItem || item is StiExpressionRangeDialogInfoItem;
			DataRow dataRow = dataTable.NewRow();
			dataRow["Checked"] = item.Checked;
			dataRow["Key"] = (IsImageType ? GetImageFromKeyObject(item) : ValueToString(item.KeyObject));
			dataRow["KeyTo"] = (IsImageType ? item.KeyObjectTo : ValueToString(item.KeyObjectTo));
			dataRow["Label"] = item.Label;
			dataRow["Expression"] = flag;
			dataTable.Rows.Add(dataRow);
		}
		gridItems.AddNewEmptyRowsUpTo(15);
		((DataGridView)gridItems).Columns.Clear();
		if (IsCheckable)
		{
			string main = Loc.GetMain("Checked");
			int width = gridItems.MeasureHeader(main) + StiScale.I5;
			gridItems.AddCheckBoxColumn("Checked", main, width, (DataGridViewContentAlignment)32);
		}
		DataGridViewContentAlignment alignment = gridItems.GetAlignment(GetNativeVariableType());
		if (IsRangeType)
		{
			gridItems.AddTextBoxColumn("Key", Loc.GetMain("RangeFrom"), StiScale.I(100), alignment);
			gridItems.AddTextBoxColumn("KeyTo", Loc.GetMain("RangeTo"), StiScale.I(100), alignment);
		}
		else if (IsImageType)
		{
			gridItems.AddImageColumn("Key", Loc.GetMain("Key"), StiScale.I(100), alignment);
		}
		else
		{
			gridItems.AddTextBoxColumn("Key", Loc.GetMain("Key"), StiScale.I(100), alignment);
		}
		gridItems.AddTextBoxColumn("Label", Loc.GetMain("Label"), StiScale.I(150), (DataGridViewContentAlignment)16);
		if (!IsImageType)
		{
			string main2 = Loc.GetMain("Expression");
			int width2 = gridItems.MeasureHeader(main2) + StiScale.I5;
			gridItems.AddCheckBoxColumn("Expression", main2, width2, (DataGridViewContentAlignment)32);
		}
	}

	private object GetImageFromKeyObject(StiDialogInfoItem item)
	{
		if (!(item.KeyObject is Image))
		{
			return DBNull.Value;
		}
		return item.KeyObject;
	}

	private StiDialogInfoItem NewDialogInfoItem(DataRow row = null)
	{
		StiDialogInfoItem stiDialogInfoItem = null;
		if (variableType == typeof(sbyte) || variableType == typeof(ushort) || variableType == typeof(uint) || variableType == typeof(ulong) || variableType == typeof(byte) || variableType == typeof(short) || variableType == typeof(int) || variableType == typeof(long) || variableType == typeof(sbyte?) || variableType == typeof(ushort?) || variableType == typeof(uint?) || variableType == typeof(ulong?) || variableType == typeof(byte?) || variableType == typeof(short?) || variableType == typeof(int?) || variableType == typeof(long?) || variableType == typeof(ByteList) || variableType == typeof(ShortList) || variableType == typeof(IntList) || variableType == typeof(LongList))
		{
			stiDialogInfoItem = new StiLongDialogInfoItem();
			if (row != null)
			{
				stiDialogInfoItem.KeyObject = StiValueHelper.TryToLong(ReadKey(row));
				stiDialogInfoItem.Label = ReadLabel(row);
			}
		}
		else if (variableType == typeof(string) || variableType == typeof(StringList))
		{
			stiDialogInfoItem = new StiStringDialogInfoItem();
			if (row != null)
			{
				stiDialogInfoItem.Checked = ReadChecked(row);
				stiDialogInfoItem.KeyObject = ReadKey(row);
				stiDialogInfoItem.Label = ReadLabel(row);
			}
		}
		else if (variableType == typeof(double) || variableType == typeof(float) || variableType == typeof(double?) || variableType == typeof(float?) || variableType == typeof(FloatList) || variableType == typeof(DoubleList))
		{
			stiDialogInfoItem = new StiDoubleDialogInfoItem();
			if (row != null)
			{
				stiDialogInfoItem.Checked = ReadChecked(row);
				stiDialogInfoItem.KeyObject = StiValueHelper.TryToDouble(ReadKey(row));
				stiDialogInfoItem.Label = ReadLabel(row);
			}
		}
		else if (variableType == typeof(decimal) || variableType == typeof(decimal?) || variableType == typeof(DecimalList))
		{
			stiDialogInfoItem = new StiDecimalDialogInfoItem();
			if (row != null)
			{
				stiDialogInfoItem.Checked = ReadChecked(row);
				stiDialogInfoItem.KeyObject = StiValueHelper.TryToDecimal(ReadKey(row));
				stiDialogInfoItem.Label = ReadLabel(row);
			}
		}
		else if (variableType == typeof(DateTime) || variableType == typeof(DateTimeList) || variableType == typeof(DateTime?))
		{
			stiDialogInfoItem = new StiDateTimeDialogInfoItem();
			if (row != null)
			{
				stiDialogInfoItem.Checked = ReadChecked(row);
				stiDialogInfoItem.KeyObject = StiValueHelper.TryToDateTime(ReadKey(row));
				stiDialogInfoItem.Label = ReadLabel(row);
			}
		}
		else if (variableType == typeof(TimeSpan) || variableType == typeof(TimeSpanList) || variableType == typeof(TimeSpan?))
		{
			stiDialogInfoItem = new StiTimeSpanDialogInfoItem();
			if (row != null)
			{
				stiDialogInfoItem.Checked = ReadChecked(row);
				stiDialogInfoItem.KeyObject = StiValueHelper.TryToTimeSpan(ReadKey(row));
				stiDialogInfoItem.Label = ReadLabel(row);
			}
		}
		else if (variableType == typeof(bool) || variableType == typeof(bool?) || variableType == typeof(BoolList))
		{
			stiDialogInfoItem = new StiBoolDialogInfoItem();
			if (row != null)
			{
				stiDialogInfoItem.Checked = ReadChecked(row);
				stiDialogInfoItem.KeyObject = StiValueHelper.TryToBool(ReadKey(row));
				stiDialogInfoItem.Label = ReadLabel(row);
			}
		}
		else if (variableType == typeof(char) || variableType == typeof(char?) || variableType == typeof(CharList))
		{
			stiDialogInfoItem = new StiCharDialogInfoItem();
			if (row != null)
			{
				stiDialogInfoItem.Checked = ReadChecked(row);
				stiDialogInfoItem.KeyObject = StiValueHelper.TryToChar(ReadKey(row));
				stiDialogInfoItem.Label = ReadLabel(row);
			}
		}
		else if (variableType == typeof(Guid) || variableType == typeof(Guid?) || variableType == typeof(GuidList))
		{
			stiDialogInfoItem = new StiGuidDialogInfoItem();
			if (row != null)
			{
				stiDialogInfoItem.Checked = ReadChecked(row);
				stiDialogInfoItem.KeyObject = StiValueHelper.TryToGuid(ReadKey(row));
				stiDialogInfoItem.Label = ReadLabel(row);
			}
		}
		else if (variableType == typeof(Image) || variableType == typeof(ImageList))
		{
			stiDialogInfoItem = new StiImageDialogInfoItem();
			if (row != null)
			{
				stiDialogInfoItem.Checked = ReadChecked(row);
				StiDialogInfoItem stiDialogInfoItem2 = stiDialogInfoItem;
				object obj = row["Key"];
				stiDialogInfoItem2.KeyObject = ((obj is Image) ? obj : null);
				stiDialogInfoItem.Label = ReadLabel(row);
			}
		}
		if (variableType == typeof(ByteRange) || variableType == typeof(ShortRange) || variableType == typeof(IntRange) || variableType == typeof(LongRange))
		{
			stiDialogInfoItem = new StiLongRangeDialogInfoItem();
			if (row != null)
			{
				StiLongRangeDialogInfoItem stiLongRangeDialogInfoItem = stiDialogInfoItem as StiLongRangeDialogInfoItem;
				stiLongRangeDialogInfoItem.Checked = ReadChecked(row);
				stiLongRangeDialogInfoItem.KeyObject = StiValueHelper.TryToLong(ReadKey(row));
				stiLongRangeDialogInfoItem.KeyObjectTo = StiValueHelper.TryToLong(ReadKeyTo(row));
				stiLongRangeDialogInfoItem.Label = ReadLabel(row);
			}
		}
		else if (variableType == typeof(StringRange))
		{
			stiDialogInfoItem = new StiStringRangeDialogInfoItem();
			if (row != null)
			{
				StiStringRangeDialogInfoItem stiStringRangeDialogInfoItem = stiDialogInfoItem as StiStringRangeDialogInfoItem;
				stiStringRangeDialogInfoItem.Checked = ReadChecked(row);
				stiStringRangeDialogInfoItem.KeyObject = ReadKey(row);
				stiStringRangeDialogInfoItem.KeyObjectTo = ReadKeyTo(row);
				stiStringRangeDialogInfoItem.Label = ReadLabel(row);
			}
		}
		else if (variableType == typeof(DoubleRange) || variableType == typeof(FloatRange))
		{
			stiDialogInfoItem = new StiDoubleRangeDialogInfoItem();
			if (row != null)
			{
				StiDoubleRangeDialogInfoItem stiDoubleRangeDialogInfoItem = stiDialogInfoItem as StiDoubleRangeDialogInfoItem;
				stiDoubleRangeDialogInfoItem.Checked = ReadChecked(row);
				stiDoubleRangeDialogInfoItem.KeyObject = StiValueHelper.TryToDouble(ReadKey(row));
				stiDoubleRangeDialogInfoItem.KeyObjectTo = StiValueHelper.TryToDouble(ReadKeyTo(row));
				stiDoubleRangeDialogInfoItem.Label = ReadLabel(row);
			}
		}
		else if (variableType == typeof(DecimalRange))
		{
			stiDialogInfoItem = new StiDecimalRangeDialogInfoItem();
			if (row != null)
			{
				StiDecimalRangeDialogInfoItem stiDecimalRangeDialogInfoItem = stiDialogInfoItem as StiDecimalRangeDialogInfoItem;
				stiDecimalRangeDialogInfoItem.Checked = ReadChecked(row);
				stiDecimalRangeDialogInfoItem.KeyObject = StiValueHelper.TryToDecimal(ReadKey(row));
				stiDecimalRangeDialogInfoItem.KeyObjectTo = StiValueHelper.TryToDecimal(ReadKeyTo(row));
				stiDecimalRangeDialogInfoItem.Label = ReadLabel(row);
			}
		}
		else if (variableType == typeof(DateTimeRange))
		{
			stiDialogInfoItem = new StiDateTimeRangeDialogInfoItem();
			if (row != null)
			{
				StiDateTimeRangeDialogInfoItem stiDateTimeRangeDialogInfoItem = stiDialogInfoItem as StiDateTimeRangeDialogInfoItem;
				stiDateTimeRangeDialogInfoItem.Checked = ReadChecked(row);
				stiDateTimeRangeDialogInfoItem.KeyObject = StiValueHelper.TryToDateTime(ReadKey(row));
				stiDateTimeRangeDialogInfoItem.KeyObjectTo = StiValueHelper.TryToDateTime(ReadKeyTo(row));
				stiDateTimeRangeDialogInfoItem.Label = ReadLabel(row);
			}
		}
		else if (variableType == typeof(TimeSpanRange))
		{
			stiDialogInfoItem = new StiTimeSpanRangeDialogInfoItem();
			if (row != null)
			{
				StiTimeSpanRangeDialogInfoItem stiTimeSpanRangeDialogInfoItem = stiDialogInfoItem as StiTimeSpanRangeDialogInfoItem;
				stiTimeSpanRangeDialogInfoItem.Checked = ReadChecked(row);
				stiTimeSpanRangeDialogInfoItem.KeyObject = StiValueHelper.TryToTimeSpan(ReadKey(row));
				stiTimeSpanRangeDialogInfoItem.KeyObjectTo = StiValueHelper.TryToTimeSpan(ReadKeyTo(row));
				stiTimeSpanRangeDialogInfoItem.Label = ReadLabel(row);
			}
		}
		else if (variableType == typeof(CharRange))
		{
			stiDialogInfoItem = new StiCharRangeDialogInfoItem();
			if (row != null)
			{
				StiCharRangeDialogInfoItem stiCharRangeDialogInfoItem = stiDialogInfoItem as StiCharRangeDialogInfoItem;
				stiCharRangeDialogInfoItem.Checked = ReadChecked(row);
				stiCharRangeDialogInfoItem.KeyObject = StiValueHelper.TryToChar(ReadKey(row));
				stiCharRangeDialogInfoItem.KeyObjectTo = StiValueHelper.TryToChar(ReadKeyTo(row));
				stiCharRangeDialogInfoItem.Label = ReadLabel(row);
			}
		}
		else if (variableType == typeof(GuidRange))
		{
			stiDialogInfoItem = new StiGuidRangeDialogInfoItem();
			if (row != null)
			{
				StiGuidRangeDialogInfoItem stiGuidRangeDialogInfoItem = stiDialogInfoItem as StiGuidRangeDialogInfoItem;
				stiGuidRangeDialogInfoItem.Checked = ReadChecked(row);
				stiGuidRangeDialogInfoItem.KeyObject = StiValueHelper.TryToGuid(ReadKey(row));
				stiGuidRangeDialogInfoItem.KeyObjectTo = StiValueHelper.TryToGuid(ReadKeyTo(row));
				stiGuidRangeDialogInfoItem.Label = ReadLabel(row);
			}
		}
		else if (variableType == typeof(byte[]))
		{
			stiDialogInfoItem = new StiByteArrayRangeDialogInfoItem();
			if (row != null)
			{
				StiDoubleRangeDialogInfoItem stiDoubleRangeDialogInfoItem2 = stiDialogInfoItem as StiDoubleRangeDialogInfoItem;
				stiDoubleRangeDialogInfoItem2.Checked = ReadChecked(row);
				stiDoubleRangeDialogInfoItem2.Label = ReadLabel(row);
			}
		}
		return stiDialogInfoItem;
	}

	private Type GetNativeVariableType()
	{
		if (variableType == typeof(CharRange) || variableType == typeof(CharList))
		{
			return typeof(char);
		}
		if (variableType == typeof(DateTimeRange) || variableType == typeof(DateTimeList))
		{
			return typeof(DateTime);
		}
		if (variableType == typeof(TimeSpanRange) || variableType == typeof(TimeSpanList))
		{
			return typeof(TimeSpan);
		}
		if (variableType == typeof(DecimalRange) || variableType == typeof(DecimalList))
		{
			return typeof(decimal);
		}
		if (variableType == typeof(FloatRange) || variableType == typeof(FloatList) || variableType == typeof(DoubleRange) || variableType == typeof(DoubleList))
		{
			return typeof(double);
		}
		if (variableType == typeof(ByteRange) || variableType == typeof(ByteList) || variableType == typeof(ShortRange) || variableType == typeof(ShortList) || variableType == typeof(IntRange) || variableType == typeof(IntList) || variableType == typeof(LongRange) || variableType == typeof(LongList))
		{
			return typeof(long);
		}
		if (variableType == typeof(GuidRange) || variableType == typeof(GuidList))
		{
			return typeof(Guid);
		}
		if (variableType == typeof(StringRange) || variableType == typeof(StringList))
		{
			return typeof(string);
		}
		return variableType;
	}

	private bool ReadChecked(DataRow row)
	{
		if (IsCheckable)
		{
			return StiValueHelper.TryToBool(row["Checked"]);
		}
		return true;
	}

	private static string ReadKey(DataRow row)
	{
		return row["Key"] as string;
	}

	private static string ReadKeyTo(DataRow row)
	{
		return row["KeyTo"] as string;
	}

	private static string ReadLabel(DataRow row)
	{
		return row["Label"] as string;
	}

	private string ValueToString(object value)
	{
		if (value is DateTime)
		{
			if (editForm.VariableDateTimeType == StiDateTimeType.DateAndTime)
			{
				return value.ToString();
			}
			if (editForm.VariableDateTimeType == StiDateTimeType.Date)
			{
				return ((DateTime)value).ToShortDateString();
			}
			return ((DateTime)value).ToShortTimeString();
		}
		string text = value?.ToString();
		return text ?? "";
	}

	protected override void OnLoad(EventArgs e)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		((Form)this).OnLoad(e);
		Build();
		Timer timer = new Timer();
		timer.Interval = 300;
		timer.Tick += delegate
		{
			timer.Stop();
			((Component)(object)timer).Dispose();
			((Control)gridItems).Focus();
		};
		timer.Start();
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/data_data_dictionary_variables_panel_request_from_user_items_dialog.htm");
	}

	private void This_Closed(object sender, EventArgs e)
	{
		StiFormSettings.Save((Form)(object)this);
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

	private void ButtonSelectColumns_Click(object sender, EventArgs e)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Invalid comparison between Unknown and I4
		try
		{
			StiDialogInfoGetItemsFromColumnForm stiDialogInfoGetItemsFromColumnForm = new StiDialogInfoGetItemsFromColumnForm(editForm.Report);
			try
			{
				((Form)this).AddOwnedForm((Form)(object)stiDialogInfoGetItemsFromColumnForm);
				if ((int)((Form)stiDialogInfoGetItemsFromColumnForm).ShowDialog() != 1)
				{
					return;
				}
				bool flag = false;
				StiDataSource dataSourceFromDataColumn = StiDataColumn.GetDataSourceFromDataColumn(editForm.Report.Dictionary, ((Control)stiDialogInfoGetItemsFromColumnForm.columnKeys).Text);
				StiDataSource dataSourceFromDataColumn2 = StiDataColumn.GetDataSourceFromDataColumn(editForm.Report.Dictionary, ((Control)stiDialogInfoGetItemsFromColumnForm.columnLabels).Text);
				if ((dataSourceFromDataColumn != null && dataSourceFromDataColumn.IsEmpty) || (dataSourceFromDataColumn2 != null && dataSourceFromDataColumn2.IsEmpty))
				{
					flag = true;
					if (dataSourceFromDataColumn != null)
					{
						dataSourceFromDataColumn.Dictionary.Connect();
					}
					else
					{
						dataSourceFromDataColumn2?.Dictionary.Connect();
					}
				}
				object[] array = StiDataColumn.GetDatasFromDataColumn(editForm.Report.Dictionary, ((Control)stiDialogInfoGetItemsFromColumnForm.columnKeys).Text);
				object[] array2 = StiDataColumn.GetDatasFromDataColumn(editForm.Report.Dictionary, ((Control)stiDialogInfoGetItemsFromColumnForm.columnLabels).Text);
				if (array == null)
				{
					array = new object[0];
				}
				if (array2 == null)
				{
					array2 = new object[0];
				}
				items.Clear();
				int num = Math.Max(array.Length, array2.Length);
				for (int i = 0; i < num; i++)
				{
					object value = ((i < array.Length) ? array[i] : null);
					object value2 = ((i < array2.Length) ? array2[i] : null);
					StiDialogInfoItem stiDialogInfoItem = NewDialogInfoItem();
					stiDialogInfoItem.KeyObject = Stimulsoft.Base.StiConvert.ChangeType(value, variableType);
					stiDialogInfoItem.Value = Stimulsoft.Base.StiConvert.ChangeType(value2, typeof(string)) as string;
					items.Add(stiDialogInfoItem);
				}
				Build();
				if (flag)
				{
					if (dataSourceFromDataColumn != null)
					{
						dataSourceFromDataColumn.Dictionary.Disconnect();
					}
					else
					{
						dataSourceFromDataColumn2?.Dictionary.Disconnect();
					}
				}
			}
			finally
			{
				((IDisposable)stiDialogInfoGetItemsFromColumnForm)?.Dispose();
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void GridItems_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
	{
		string dataPropertyName = ((DataGridView)gridItems).Columns[e.ColumnIndex].DataPropertyName;
		if (dataPropertyName == "Key" || dataPropertyName == "KeyTo")
		{
			Type nativeVariableType = GetNativeVariableType();
			if (nativeVariableType != typeof(string))
			{
				string value = ((ConvertEventArgs)e).Value?.ToString();
				object obj = Stimulsoft.Base.StiConvert.ChangeType(value, nativeVariableType);
				((ConvertEventArgs)e).Value = ValueToString((obj is string) ? Activator.CreateInstance(nativeVariableType) : obj);
			}
		}
		e.ParsingApplied = true;
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
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Expected O, but got Unknown
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_040e: Expected O, but got Unknown
		buttonOk = new StiButtonUX();
		buttonCancel = new StiButtonUX();
		gridItems = new StiExcelGrid();
		groupButtons = new StiButtonPanelUX();
		buttonSelectColumns = new StiButtonUX();
		((ISupportInitialize)gridItems).BeginInit();
		((Control)groupButtons).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)buttonOk).Anchor = (AnchorStyles)10;
		((Button)buttonOk).DialogResult = (DialogResult)1;
		((Control)buttonOk).Location = new Point(349, 12);
		((Control)buttonOk).Name = "buttonOk";
		((Control)buttonOk).Size = new Size(75, 24);
		((Control)buttonOk).TabIndex = 1;
		((Control)buttonOk).Text = "Ok";
		((ButtonBase)buttonOk).UseVisualStyleBackColor = true;
		((Control)buttonCancel).Anchor = (AnchorStyles)10;
		((Button)buttonCancel).DialogResult = (DialogResult)2;
		((Control)buttonCancel).Location = new Point(430, 12);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(75, 24);
		((Control)buttonCancel).TabIndex = 2;
		((Control)buttonCancel).Text = "Cancel";
		((ButtonBase)buttonCancel).UseVisualStyleBackColor = true;
		((Control)gridItems).Anchor = (AnchorStyles)15;
		((Control)gridItems).Location = new Point(9, 10);
		((Control)gridItems).Name = "gridItems";
		gridItems.RowHeadersWidth = 82;
		((DataGridView)gridItems).SelectionMode = (DataGridViewSelectionMode)0;
		((Control)gridItems).Size = new Size(495, 441);
		((Control)gridItems).TabIndex = 1;
		((DataGridView)gridItems).CellParsing += new DataGridViewCellParsingEventHandler(GridItems_CellParsing);
		((Control)groupButtons).BackColor = Color.White;
		((Control)groupButtons).Controls.Add((Control)(object)buttonSelectColumns);
		((Control)groupButtons).Controls.Add((Control)(object)buttonOk);
		((Control)groupButtons).Controls.Add((Control)(object)buttonCancel);
		((Control)groupButtons).Dock = (DockStyle)2;
		groupButtons.LinePosition = StiLinePosition.Top;
		groupButtons.LineStyle = StiLineStyle.Dot;
		((Control)groupButtons).Location = new Point(0, 463);
		((Control)groupButtons).Name = "groupButtons";
		((Control)groupButtons).Size = new Size(514, 46);
		((Control)groupButtons).TabIndex = 2;
		((Control)buttonSelectColumns).Anchor = (AnchorStyles)6;
		((Control)buttonSelectColumns).Location = new Point(9, 12);
		((Control)buttonSelectColumns).Name = "buttonSelectColumns";
		((Control)buttonSelectColumns).Size = new Size(100, 24);
		((Control)buttonSelectColumns).TabIndex = 0;
		((Control)buttonSelectColumns).Text = "Select Columns";
		((ButtonBase)buttonSelectColumns).UseVisualStyleBackColor = true;
		((Control)buttonSelectColumns).Click += ButtonSelectColumns_Click;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(514, 509);
		((Control)this).Controls.Add((Control)(object)gridItems);
		((Control)this).Controls.Add((Control)(object)groupButtons);
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(400, 400);
		((Control)this).Name = "StiDialogInfoItemsEditor";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Items";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Form)this).Closed += This_Closed;
		((Control)this).KeyDown += new KeyEventHandler(This_KeyDown);
		((ISupportInitialize)gridItems).EndInit();
		((Control)groupButtons).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
