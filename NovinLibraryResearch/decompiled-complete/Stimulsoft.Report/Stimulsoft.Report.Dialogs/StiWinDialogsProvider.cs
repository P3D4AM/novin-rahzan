using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Controls;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Dialogs;

public class StiWinDialogsProvider : StiDialogsProvider
{
	private Hashtable controlToReport;

	private Hashtable reportToControl;

	private Hashtable managerToGridControl;

	private StiForm formControl;

	private ToolTip toolTip;

	private bool lockReportControlUpdate;

	protected override StiGuiMode GuiMode => (StiGuiMode)0;

	public Form Form { get; private set; }

	public override StiReport Report { get; set; }

	[DllImport("User32.dll")]
	private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

	private void Manager_PositionChanged(object sender, EventArgs e)
	{
		if (managerToGridControl[sender] is StiGridControl stiGridControl)
		{
			stiGridControl.InvokePositionChanged(e);
		}
		InvokeEventFired(sender, e);
	}

	private void Control_Click(object sender, EventArgs e)
	{
		if (controlToReport == null)
		{
			return;
		}
		StiReportControl stiReportControl = controlToReport[sender] as StiReportControl;
		StiForm stiForm = controlToReport[sender] as StiForm;
		if (stiReportControl != null)
		{
			Control activeControl = null;
			Control val = (Control)((sender is Control) ? sender : null);
			Form val2 = val.FindForm();
			if (val2 != null)
			{
				activeControl = ((ContainerControl)val2).ActiveControl;
				((ContainerControl)val2).ActiveControl = val;
			}
			stiReportControl.InvokeClick(stiReportControl, e);
			if (sender is Button)
			{
				InvokeButtonClick(sender, e);
			}
			if (val2 != null && ((Control)val2).Visible)
			{
				((ContainerControl)val2).ActiveControl = activeControl;
			}
		}
		stiForm?.InvokeClick(stiReportControl, e);
	}

	private void Control_DoubleClick(object sender, EventArgs e)
	{
		if (controlToReport != null)
		{
			StiReportControl stiReportControl = controlToReport[sender] as StiReportControl;
			stiReportControl.InvokeDoubleClick(stiReportControl, e);
			if (controlToReport[sender] is StiForm stiForm)
			{
				stiForm.InvokeDoubleClick(stiReportControl, e);
			}
		}
	}

	private void Control_Enter(object sender, EventArgs e)
	{
		if (controlToReport != null && controlToReport[sender] is StiReportControl stiReportControl)
		{
			stiReportControl.InvokeEnter(e);
		}
	}

	private void Control_Leave(object sender, EventArgs e)
	{
		lockReportControlUpdate = true;
		FormToReportControl(formControl);
		lockReportControlUpdate = false;
		if (controlToReport != null && controlToReport[sender] is StiReportControl stiReportControl)
		{
			stiReportControl.InvokeLeave(e);
		}
	}

	private void Control_MouseDown(object sender, MouseEventArgs e)
	{
		if (controlToReport != null && controlToReport[sender] is StiReportControl stiReportControl)
		{
			stiReportControl.InvokeMouseDown(e);
		}
	}

	private void Control_MouseUp(object sender, MouseEventArgs e)
	{
		if (controlToReport != null && controlToReport[sender] is StiReportControl stiReportControl)
		{
			stiReportControl.InvokeMouseUp(e);
		}
	}

	private void Control_MouseMove(object sender, MouseEventArgs e)
	{
		if (controlToReport != null && controlToReport[sender] is StiReportControl stiReportControl)
		{
			stiReportControl.InvokeMouseMove(e);
		}
	}

	private void Form_Closed(object sender, EventArgs e)
	{
		if (controlToReport != null && controlToReport[sender] is StiForm stiForm)
		{
			stiForm.InvokeClosedForm(e);
		}
	}

	private void Form_Closing(object sender, CancelEventArgs e)
	{
		if (controlToReport != null && controlToReport[sender] is StiForm stiForm)
		{
			stiForm.InvokeClosingForm(e);
		}
	}

	private void Form_Load(object sender, EventArgs e)
	{
		if (controlToReport != null && controlToReport[sender] is StiForm stiForm)
		{
			stiForm.InvokeLoadForm(e);
		}
	}

	private void Control_CheckedChanged(object sender, EventArgs e)
	{
		if (controlToReport != null)
		{
			lockReportControlUpdate = true;
			FormToReportControl(formControl);
			lockReportControlUpdate = false;
			if (controlToReport[sender] is StiCheckBoxControl stiCheckBoxControl)
			{
				stiCheckBoxControl.InvokeCheckedChanged(e);
			}
			if (controlToReport[sender] is StiRadioButtonControl stiRadioButtonControl)
			{
				stiRadioButtonControl.InvokeCheckedChanged(e);
			}
			InvokeEventFired(sender, e);
		}
	}

	private void Control_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (controlToReport != null)
		{
			lockReportControlUpdate = true;
			FormToReportControl(formControl);
			lockReportControlUpdate = false;
			if (controlToReport[sender] is StiListBoxControl stiListBoxControl)
			{
				stiListBoxControl.InvokeSelectedIndexChanged(e);
			}
			if (controlToReport[sender] is StiCheckedListBoxControl stiCheckedListBoxControl)
			{
				stiCheckedListBoxControl.InvokeSelectedIndexChanged(e);
			}
			if (controlToReport[sender] is StiListViewControl stiListViewControl)
			{
				stiListViewControl.InvokeSelectedIndexChanged(e);
			}
			if (controlToReport[sender] is StiComboBoxControl stiComboBoxControl)
			{
				stiComboBoxControl.InvokeSelectedIndexChanged(e);
			}
			InvokeEventFired(sender, e);
		}
	}

	private void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
	{
		if (controlToReport != null)
		{
			lockReportControlUpdate = true;
			FormToReportControl(formControl);
			lockReportControlUpdate = false;
			if (controlToReport[sender] is StiTreeViewControl stiTreeViewControl)
			{
				stiTreeViewControl.InvokeAfterSelect(e);
			}
			InvokeEventFired(sender, (EventArgs)(object)e);
		}
	}

	private void OnValueChanged(object sender, EventArgs e)
	{
		if (controlToReport != null)
		{
			lockReportControlUpdate = true;
			FormToReportControl(formControl);
			lockReportControlUpdate = false;
			if (controlToReport[sender] is StiDateTimePickerControl stiDateTimePickerControl)
			{
				stiDateTimePickerControl.InvokeValueChanged(e);
			}
			if (controlToReport[sender] is StiNumericUpDownControl stiNumericUpDownControl)
			{
				stiNumericUpDownControl.InvokeValueChanged(e);
			}
			InvokeEventFired(sender, e);
		}
	}

	protected virtual void ConvertReportControlToControl(Control control, StiReportControl reportControl)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Expected O, but got Unknown
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Expected O, but got Unknown
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Expected O, but got Unknown
		if (reportControl.TagValueBinding.Length > 0)
		{
			reportControl.TagValue = StiDataColumn.GetDataFromDataColumn(Report.Dictionary, reportControl.TagValueBinding);
		}
		control.Dock = StiComponent.ConvertDockStyle(reportControl.DockStyle);
		control.Location = StiScale.I(reportControl.Location);
		control.Size = StiScale.I(reportControl.Size);
		if (control.BackColor != reportControl.BackColor)
		{
			control.BackColor = reportControl.BackColor;
		}
		control.Font = reportControl.Font;
		control.RightToLeft = reportControl.RightToLeft;
		control.Tag = reportControl.TagValue;
		if (control.ForeColor != reportControl.ForeColor)
		{
			control.ForeColor = reportControl.ForeColor;
		}
		control.Enabled = reportControl.Enabled;
		control.Visible = reportControl.Visible;
		reportControl.InvokeEvents();
		toolTip.SetToolTip(control, reportControl.ToolTipValue as string);
		control.Click += Control_Click;
		control.DoubleClick += Control_DoubleClick;
		control.Enter += Control_Enter;
		control.Leave += Control_Leave;
		control.MouseDown += new MouseEventHandler(Control_MouseDown);
		control.MouseUp += new MouseEventHandler(Control_MouseUp);
		control.MouseMove += new MouseEventHandler(Control_MouseMove);
	}

	protected virtual Control ConvertReportControlToPanel(StiPanelControl panelControl)
	{
		Stimulsoft.Report.Controls.StiPanel stiPanel2 = (panelControl.Control = new Stimulsoft.Report.Controls.StiPanel());
		ConvertReportControlToControl((Control)(object)stiPanel2, panelControl);
		stiPanel2.BorderStyle = panelControl.BorderStyle;
		return (Control)(object)stiPanel2;
	}

	protected virtual Control ConvertReportControlToTextBox(StiTextBoxControl textBoxControl)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		TextBox val2 = (textBoxControl.Control = new TextBox());
		ConvertReportControlToControl((Control)(object)val2, textBoxControl);
		if (textBoxControl.TextBinding.Length > 0)
		{
			object dataFromDataColumn = StiDataColumn.GetDataFromDataColumn(Report.Dictionary, textBoxControl.TextBinding);
			if (dataFromDataColumn != null)
			{
				textBoxControl.Text = dataFromDataColumn.ToString();
			}
		}
		((TextBoxBase)val2).MaxLength = textBoxControl.MaxLength;
		val2.PasswordChar = textBoxControl.PasswordChar;
		((Control)val2).Text = textBoxControl.Text;
		((TextBoxBase)val2).Multiline = textBoxControl.Multiline;
		((TextBoxBase)val2).WordWrap = textBoxControl.WordWrap;
		val2.AcceptsReturn = textBoxControl.AcceptsReturn;
		((TextBoxBase)val2).AcceptsTab = textBoxControl.AcceptsTab;
		return (Control)(object)val2;
	}

	protected virtual Control ConvertReportControlToRichTextBox(StiRichTextBoxControl textBoxControl)
	{
		StiRichTextBox stiRichTextBox = (StiRichTextBox)(object)(textBoxControl.Control = (RichTextBox)(object)new StiRichTextBox(textBoxControl.BackColor == Color.Transparent));
		ConvertReportControlToControl((Control)(object)stiRichTextBox, textBoxControl);
		((RichTextBox)stiRichTextBox).Rtf = textBoxControl.RtfText;
		return (Control)(object)stiRichTextBox;
	}

	protected virtual Control ConvertReportControlToButton(StiButtonControl buttonControl)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		Button val2 = (buttonControl.Control = new Button
		{
			TextAlign = buttonControl.TextAlign,
			ImageAlign = buttonControl.ImageAlign,
			Image = buttonControl.Image
		});
		ConvertReportControlToControl((Control)(object)val2, buttonControl);
		val2.DialogResult = buttonControl.DialogResult;
		((Control)val2).Text = buttonControl.Text;
		if (buttonControl.Default)
		{
			Form.AcceptButton = (IButtonControl)(object)val2;
		}
		if (buttonControl.Cancel)
		{
			Form.CancelButton = (IButtonControl)(object)val2;
		}
		return (Control)(object)val2;
	}

	protected virtual Control ConvertReportControlToCheckBox(StiCheckBoxControl checkBoxControl)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		CheckBox val2 = (checkBoxControl.Control = new CheckBox());
		ConvertReportControlToControl((Control)(object)val2, checkBoxControl);
		if (checkBoxControl.CheckedBinding.Length > 0)
		{
			object dataFromDataColumn = StiDataColumn.GetDataFromDataColumn(Report.Dictionary, checkBoxControl.CheckedBinding);
			if (dataFromDataColumn is bool)
			{
				checkBoxControl.Checked = (bool)dataFromDataColumn;
			}
		}
		if (checkBoxControl.TextBinding.Length > 0)
		{
			object dataFromDataColumn2 = StiDataColumn.GetDataFromDataColumn(Report.Dictionary, checkBoxControl.TextBinding);
			if (dataFromDataColumn2 != null)
			{
				checkBoxControl.Text = dataFromDataColumn2.ToString();
			}
		}
		val2.Checked = checkBoxControl.Checked;
		((Control)val2).Text = checkBoxControl.Text;
		val2.CheckedChanged += Control_CheckedChanged;
		return (Control)(object)val2;
	}

	protected virtual Control ConvertReportControlToRadioButton(StiRadioButtonControl radioButtonControl)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		RadioButton val2 = (radioButtonControl.Control = new RadioButton());
		ConvertReportControlToControl((Control)(object)val2, radioButtonControl);
		if (radioButtonControl.CheckedBinding.Length > 0)
		{
			object dataFromDataColumn = StiDataColumn.GetDataFromDataColumn(Report.Dictionary, radioButtonControl.CheckedBinding);
			if (dataFromDataColumn is bool)
			{
				radioButtonControl.Checked = (bool)dataFromDataColumn;
			}
		}
		if (radioButtonControl.TextBinding.Length > 0)
		{
			object dataFromDataColumn2 = StiDataColumn.GetDataFromDataColumn(Report.Dictionary, radioButtonControl.TextBinding);
			if (dataFromDataColumn2 != null)
			{
				radioButtonControl.Text = dataFromDataColumn2.ToString();
			}
		}
		val2.Checked = radioButtonControl.Checked;
		((Control)val2).Text = radioButtonControl.Text;
		val2.CheckedChanged += Control_CheckedChanged;
		return (Control)(object)val2;
	}

	protected virtual Control ConvertReportControlToLabel(StiLabelControl labelControl)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		Label val2 = (labelControl.Control = new Label());
		ConvertReportControlToControl((Control)(object)val2, labelControl);
		if (labelControl.TextBinding.Length > 0)
		{
			object dataFromDataColumn = StiDataColumn.GetDataFromDataColumn(Report.Dictionary, labelControl.TextBinding);
			if (dataFromDataColumn != null)
			{
				labelControl.Text = dataFromDataColumn.ToString();
			}
		}
		val2.TextAlign = labelControl.TextAlign;
		((Control)val2).Text = labelControl.Text;
		return (Control)(object)val2;
	}

	protected virtual Control ConvertReportControlToGrid(StiGridControl gridControl)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Expected O, but got Unknown
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		DataGridView val2 = (gridControl.Control = new DataGridView());
		ConvertReportControlToControl((Control)(object)val2, gridControl);
		val2.ColumnHeadersVisible = gridControl.ColumnHeadersVisible;
		val2.RowHeadersVisible = gridControl.RowHeadersVisible;
		val2.ColumnHeadersDefaultCellStyle.Font = gridControl.HeaderFont;
		val2.AutoResizeColumns();
		val2.ColumnHeadersHeight = StiScale.YYI(20.0);
		val2.RowTemplate.Height = gridControl.PreferredRowHeight;
		if (gridControl.DataSource != null)
		{
			StiDataSource dataSource = gridControl.DataSource;
			if (gridControl.Columns.Count == 0)
			{
				val2.RowsDefaultCellStyle.BackColor = gridControl.BackgroundColor;
				val2.AlternatingRowsDefaultCellStyle.BackColor = gridControl.AlternatingBackColor;
				((Control)val2).ForeColor = gridControl.ForeColor;
				((Control)val2).BackColor = gridControl.BackColor;
			}
			DataSet dataSet = new DataSet();
			DataTable dataTable = new DataTable(dataSource.Name);
			dataSet.Tables.Add(dataTable);
			if (gridControl.Columns.Count > 0)
			{
				foreach (StiGridColumn column in gridControl.Columns)
				{
					if (!column.Visible || column.DataTextField.Length <= 0)
					{
						continue;
					}
					StiDataColumn dataColumnFromColumnName = StiDataColumn.GetDataColumnFromColumnName(dataSource.Dictionary, column.DataTextField);
					if (dataColumnFromColumnName != null)
					{
						string columnNameFromDataColumn = StiDataColumn.GetColumnNameFromDataColumn(dataSource.Dictionary, column.DataTextField);
						dataTable.Columns.Add(new DataColumn(columnNameFromDataColumn, dataColumnFromColumnName.Type)
						{
							Caption = dataColumnFromColumnName.Alias,
							AllowDBNull = true
						});
						DataGridViewTextBoxColumn val3 = new DataGridViewTextBoxColumn
						{
							DataPropertyName = columnNameFromDataColumn,
							HeaderText = ((column.HeaderText.Length == 0) ? dataColumnFromColumnName.Alias : column.HeaderText)
						};
						((DataGridViewBand)val3).DefaultCellStyle.Alignment = ToDataGridViewContentAlignment(column.Alignment);
						if (column.Width != 0)
						{
							((DataGridViewColumn)val3).Width = column.Width;
						}
						val2.Columns.Add((DataGridViewColumn)(object)val3);
					}
				}
			}
			else
			{
				foreach (StiDataColumn column2 in dataSource.Columns)
				{
					dataTable.Columns.Add(new DataColumn(column2.Name, column2.Type)
					{
						Caption = column2.Alias,
						AllowDBNull = true
					});
				}
			}
			while (!dataSource.IsEof)
			{
				DataRow dataRow = dataTable.NewRow();
				if (gridControl.Columns.Count > 0)
				{
					foreach (StiGridColumn column3 in gridControl.Columns)
					{
						if (column3.Visible && column3.DataTextField.Length > 0)
						{
							string columnNameFromDataColumn2 = StiDataColumn.GetColumnNameFromDataColumn(dataSource.Dictionary, column3.DataTextField);
							dataRow[columnNameFromDataColumn2] = StiDataColumn.GetDataFromDataColumn(dataSource.Dictionary, column3.DataTextField);
						}
					}
				}
				else
				{
					foreach (StiDataColumn column4 in dataSource.Columns)
					{
						dataRow[column4.Name] = dataSource[column4.Name];
					}
				}
				dataTable.Rows.Add(dataRow);
				dataSource.Next();
			}
			dataSource.First();
			DataView dataView = new DataView(dataTable);
			dataView.RowFilter = gridControl.Filter;
			val2.DataSource = dataView;
			val2.ReadOnly = true;
			val2.RowHeadersVisible = false;
			if (gridControl.Columns.Count > 0)
			{
				val2.BackgroundColor = gridControl.BackgroundColor;
				val2.RowsDefaultCellStyle.BackColor = gridControl.BackgroundColor;
				val2.AlternatingRowsDefaultCellStyle.BackColor = gridControl.AlternatingBackColor;
				((Control)val2).ForeColor = gridControl.ForeColor;
				((Control)val2).BackColor = gridControl.BackColor;
			}
			BindingManagerBase val4 = ((Control)Form).BindingContext[val2.DataSource, val2.DataMember];
			managerToGridControl[val4] = gridControl;
			val4.PositionChanged += Manager_PositionChanged;
		}
		return (Control)(object)val2;
	}

	private DataGridViewContentAlignment ToDataGridViewContentAlignment(HorizontalAlignment alignment)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected I4, but got Unknown
		return (DataGridViewContentAlignment)((int)alignment switch
		{
			2 => 32, 
			1 => 64, 
			0 => 16, 
			_ => 32, 
		});
	}

	protected virtual Control ConvertReportControlToPictureBox(StiPictureBoxControl pictureBoxControl)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		PictureBox val2 = (pictureBoxControl.Control = new PictureBox());
		ConvertReportControlToControl((Control)(object)val2, pictureBoxControl);
		val2.SizeMode = pictureBoxControl.SizeMode;
		val2.BorderStyle = pictureBoxControl.BorderStyle;
		val2.Image = pictureBoxControl.Image;
		return (Control)(object)val2;
	}

	protected virtual Control ConvertReportControlToGroupBox(StiGroupBoxControl groupBoxControl)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GroupBox val2 = (groupBoxControl.Control = new GroupBox());
		ConvertReportControlToControl((Control)(object)val2, groupBoxControl);
		if (groupBoxControl.TextBinding.Length > 0)
		{
			object dataFromDataColumn = StiDataColumn.GetDataFromDataColumn(Report.Dictionary, groupBoxControl.TextBinding);
			if (dataFromDataColumn != null)
			{
				groupBoxControl.Text = dataFromDataColumn.ToString();
			}
		}
		((Control)val2).Text = groupBoxControl.Text;
		return (Control)(object)val2;
	}

	protected virtual Control ConvertReportControlToListBox(StiListBoxControl listBoxControl)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		ListBox val2 = (listBoxControl.Control = new ListBox());
		ConvertReportControlToControl((Control)(object)val2, listBoxControl);
		if (listBoxControl.ItemsBinding.Length > 0)
		{
			object[] datasFromDataColumn = StiDataColumn.GetDatasFromDataColumn(Report.Dictionary, listBoxControl.ItemsBinding);
			if (listBoxControl.Sorted)
			{
				Array.Sort(datasFromDataColumn);
			}
			listBoxControl.Items.ClearCore();
			listBoxControl.Items.AddRangeCore(datasFromDataColumn);
		}
		val2.Items.AddRange(listBoxControl.Items.ToArray());
		if (val2.Items.Count > 0)
		{
			object selectedItem = (listBoxControl.SelectedItem = val2.Items[0]);
			val2.SelectedItem = selectedItem;
		}
		if (listBoxControl.SelectedItemBinding.Length > 0)
		{
			object dataFromDataColumn = StiDataColumn.GetDataFromDataColumn(Report.Dictionary, listBoxControl.SelectedItemBinding);
			object selectedItem = (listBoxControl.SelectedItem = dataFromDataColumn);
			val2.SelectedItem = selectedItem;
		}
		if (listBoxControl.SelectedValueBinding.Length > 0)
		{
			object dataFromDataColumn2 = StiDataColumn.GetDataFromDataColumn(Report.Dictionary, listBoxControl.SelectedValueBinding);
			object selectedItem = (listBoxControl.SelectedValue = dataFromDataColumn2);
			((ListControl)val2).SelectedValue = selectedItem;
		}
		val2.SelectionMode = listBoxControl.SelectionMode;
		val2.Sorted = listBoxControl.Sorted;
		val2.ItemHeight = StiScale.I(listBoxControl.ItemHeight);
		val2.HorizontalScrollbar = true;
		val2.SelectedIndexChanged += Control_SelectedIndexChanged;
		return (Control)(object)val2;
	}

	protected virtual Control ConvertReportControlToTreeView(StiTreeViewControl treeViewControl)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		TreeView val = new TreeView();
		val.Scrollable = true;
		treeViewControl.Control = val;
		ConvertReportControlToControl((Control)(object)val, treeViewControl);
		val.AfterSelect += new TreeViewEventHandler(TreeView_AfterSelect);
		return (Control)(object)val;
	}

	protected virtual Control ConvertReportControlToListView(StiListViewControl listViewControl)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		ListView val = new ListView();
		val.Scrollable = true;
		listViewControl.Control = val;
		ConvertReportControlToControl((Control)(object)val, listViewControl);
		val.SelectedIndexChanged += Control_SelectedIndexChanged;
		return (Control)(object)val;
	}

	protected virtual Control ConvertReportControlToCheckedListBox(StiCheckedListBoxControl checkedListBoxControl)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		CheckedListBox val2 = (checkedListBoxControl.Control = new CheckedListBox());
		ConvertReportControlToControl((Control)(object)val2, checkedListBoxControl);
		((ListBox)val2).SelectionMode = checkedListBoxControl.SelectionMode;
		((ListBox)val2).Sorted = checkedListBoxControl.Sorted;
		((ListBox)val2).ItemHeight = StiScale.I(checkedListBoxControl.ItemHeight);
		val2.CheckOnClick = checkedListBoxControl.CheckOnClick;
		((ListBox)val2).HorizontalScrollbar = true;
		if (checkedListBoxControl.ItemsBinding.Length > 0)
		{
			object[] datasFromDataColumn = StiDataColumn.GetDatasFromDataColumn(Report.Dictionary, checkedListBoxControl.ItemsBinding);
			if (checkedListBoxControl.Sorted)
			{
				Array.Sort(datasFromDataColumn);
			}
			checkedListBoxControl.Items.ClearCore();
			checkedListBoxControl.Items.AddRangeCore(datasFromDataColumn);
		}
		((ObjectCollection)val2.Items).AddRange(checkedListBoxControl.Items.ToArray());
		if (((ObjectCollection)val2.Items).Count > 0)
		{
			object selectedItem = (checkedListBoxControl.SelectedItem = ((ObjectCollection)val2.Items)[0]);
			((ListBox)val2).SelectedItem = selectedItem;
		}
		if (checkedListBoxControl.SelectedItemBinding.Length > 0)
		{
			object dataFromDataColumn = StiDataColumn.GetDataFromDataColumn(Report.Dictionary, checkedListBoxControl.SelectedItemBinding);
			object selectedItem = (checkedListBoxControl.SelectedItem = dataFromDataColumn);
			((ListBox)val2).SelectedItem = selectedItem;
		}
		if (checkedListBoxControl.SelectedValueBinding.Length > 0)
		{
			object dataFromDataColumn2 = StiDataColumn.GetDataFromDataColumn(Report.Dictionary, checkedListBoxControl.SelectedValueBinding);
			object selectedItem = (checkedListBoxControl.SelectedValue = dataFromDataColumn2);
			((ListControl)val2).SelectedValue = selectedItem;
		}
		((ListBox)val2).SelectedIndexChanged += Control_SelectedIndexChanged;
		return (Control)(object)val2;
	}

	protected virtual Control ConvertReportControlToComboBox(StiComboBoxControl comboBoxControl)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		ComboBox val2 = (comboBoxControl.Control = new ComboBox());
		ConvertReportControlToControl((Control)(object)val2, comboBoxControl);
		if (comboBoxControl.ItemsBinding.Length > 0)
		{
			object[] datasFromDataColumn = StiDataColumn.GetDatasFromDataColumn(Report.Dictionary, comboBoxControl.ItemsBinding);
			if (comboBoxControl.Sorted)
			{
				Array.Sort(datasFromDataColumn);
			}
			comboBoxControl.Items.ClearCore();
			comboBoxControl.Items.AddRangeCore(datasFromDataColumn);
		}
		val2.Items.AddRange(comboBoxControl.Items.ToArray());
		if (val2.Items.Count > 0)
		{
			object selectedItem = (comboBoxControl.SelectedItem = val2.Items[0]);
			val2.SelectedItem = selectedItem;
		}
		if (comboBoxControl.TextBinding.Length > 0)
		{
			object dataFromDataColumn = StiDataColumn.GetDataFromDataColumn(Report.Dictionary, comboBoxControl.TextBinding);
			if (dataFromDataColumn != null)
			{
				comboBoxControl.Text = dataFromDataColumn.ToString();
			}
		}
		if (comboBoxControl.SelectedItemBinding.Length > 0)
		{
			object dataFromDataColumn2 = StiDataColumn.GetDataFromDataColumn(Report.Dictionary, comboBoxControl.SelectedItemBinding);
			object selectedItem = (comboBoxControl.SelectedItem = dataFromDataColumn2);
			val2.SelectedItem = selectedItem;
		}
		if (comboBoxControl.SelectedValueBinding.Length > 0)
		{
			object dataFromDataColumn3 = StiDataColumn.GetDataFromDataColumn(Report.Dictionary, comboBoxControl.SelectedValueBinding);
			object selectedItem = (comboBoxControl.SelectedValue = dataFromDataColumn3);
			((ListControl)val2).SelectedValue = selectedItem;
		}
		val2.DropDownStyle = comboBoxControl.DropDownStyle;
		val2.MaxLength = comboBoxControl.MaxLength;
		val2.Sorted = comboBoxControl.Sorted;
		val2.ItemHeight = StiScale.I(comboBoxControl.ItemHeight);
		val2.MaxDropDownItems = comboBoxControl.MaxDropDownItems;
		val2.DropDownWidth = StiScale.I(comboBoxControl.DropDownWidth);
		val2.SelectedIndexChanged += Control_SelectedIndexChanged;
		return (Control)(object)val2;
	}

	protected virtual Control ConvertReportControlToLookUpBox(StiLookUpBoxControl lookUpBoxControl)
	{
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		StiLookUpBox stiLookUpBox = (StiLookUpBox)(object)(lookUpBoxControl.Control = (ComboBox)(object)new StiLookUpBox());
		ConvertReportControlToControl((Control)(object)stiLookUpBox, lookUpBoxControl);
		if (lookUpBoxControl.TextBinding.Length > 0)
		{
			object dataFromDataColumn = StiDataColumn.GetDataFromDataColumn(Report.Dictionary, lookUpBoxControl.TextBinding);
			if (dataFromDataColumn != null)
			{
				lookUpBoxControl.Text = dataFromDataColumn.ToString();
			}
		}
		((ComboBox)stiLookUpBox).Items.Clear();
		if (lookUpBoxControl.ItemsBinding.Length > 0)
		{
			((ComboBox)stiLookUpBox).Items.AddRange(StiDataColumn.GetDatasFromDataColumn(Report.Dictionary, lookUpBoxControl.ItemsBinding));
			if (((ComboBox)stiLookUpBox).Items.Count > 0)
			{
				object selectedItem = (lookUpBoxControl.SelectedItem = ((ComboBox)stiLookUpBox).Items[0]);
				((ComboBox)stiLookUpBox).SelectedItem = selectedItem;
			}
		}
		else
		{
			((ComboBox)stiLookUpBox).Items.AddRange(lookUpBoxControl.Items.ToArray());
		}
		stiLookUpBox.Keys.Clear();
		if (lookUpBoxControl.KeysBinding.Length > 0)
		{
			stiLookUpBox.Keys.AddRange(StiDataColumn.GetDatasFromDataColumn(Report.Dictionary, lookUpBoxControl.KeysBinding));
		}
		else
		{
			stiLookUpBox.Keys.AddRange(lookUpBoxControl.Keys.ToArray());
		}
		if (lookUpBoxControl.SelectedItemBinding.Length > 0)
		{
			object dataFromDataColumn2 = StiDataColumn.GetDataFromDataColumn(Report.Dictionary, lookUpBoxControl.SelectedItemBinding);
			object selectedItem = (lookUpBoxControl.SelectedItem = dataFromDataColumn2);
			((ComboBox)stiLookUpBox).SelectedItem = selectedItem;
		}
		if (lookUpBoxControl.SelectedKeyBinding.Length > 0)
		{
			object dataFromDataColumn3 = StiDataColumn.GetDataFromDataColumn(Report.Dictionary, lookUpBoxControl.SelectedKeyBinding);
			object selectedItem = (lookUpBoxControl.SelectedKey = dataFromDataColumn3);
			stiLookUpBox.SelectedKey = selectedItem;
		}
		if (lookUpBoxControl.SelectedValueBinding.Length > 0)
		{
			object dataFromDataColumn4 = StiDataColumn.GetDataFromDataColumn(Report.Dictionary, lookUpBoxControl.SelectedValueBinding);
			object selectedItem = (lookUpBoxControl.SelectedValue = dataFromDataColumn4);
			((ListControl)stiLookUpBox).SelectedValue = selectedItem;
		}
		((ComboBox)stiLookUpBox).DropDownStyle = lookUpBoxControl.DropDownStyle;
		((ComboBox)stiLookUpBox).MaxLength = lookUpBoxControl.MaxLength;
		((ComboBox)stiLookUpBox).Sorted = lookUpBoxControl.Sorted;
		((ComboBox)stiLookUpBox).ItemHeight = StiScale.I(lookUpBoxControl.ItemHeight);
		((ComboBox)stiLookUpBox).MaxDropDownItems = lookUpBoxControl.MaxDropDownItems;
		((ComboBox)stiLookUpBox).DropDownWidth = StiScale.I(lookUpBoxControl.DropDownWidth);
		stiLookUpBox.Flat = false;
		((ComboBox)stiLookUpBox).SelectedIndexChanged += Control_SelectedIndexChanged;
		return (Control)(object)stiLookUpBox;
	}

	protected virtual Control ConvertReportControlToNumericUpDown(StiNumericUpDownControl numericUpDownControl)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		NumericUpDown val2 = (numericUpDownControl.Control = new NumericUpDown());
		ConvertReportControlToControl((Control)(object)val2, numericUpDownControl);
		if (numericUpDownControl.ValueBinding.Length > 0)
		{
			object dataFromDataColumn = StiDataColumn.GetDataFromDataColumn(Report.Dictionary, numericUpDownControl.ValueBinding);
			if (dataFromDataColumn is int)
			{
				numericUpDownControl.Value = (int)dataFromDataColumn;
			}
		}
		if (numericUpDownControl.MaximumBinding.Length > 0)
		{
			object dataFromDataColumn2 = StiDataColumn.GetDataFromDataColumn(Report.Dictionary, numericUpDownControl.MaximumBinding);
			if (dataFromDataColumn2 is int)
			{
				numericUpDownControl.Maximum = (int)dataFromDataColumn2;
			}
		}
		if (numericUpDownControl.MinimumBinding.Length > 0)
		{
			object dataFromDataColumn3 = StiDataColumn.GetDataFromDataColumn(Report.Dictionary, numericUpDownControl.MinimumBinding);
			if (dataFromDataColumn3 is int)
			{
				numericUpDownControl.Minimum = (int)dataFromDataColumn3;
			}
		}
		val2.Minimum = numericUpDownControl.Minimum;
		val2.Maximum = numericUpDownControl.Maximum;
		val2.Increment = numericUpDownControl.Increment;
		val2.Value = numericUpDownControl.Value;
		val2.ValueChanged += OnValueChanged;
		return (Control)(object)val2;
	}

	protected virtual Control ConvertReportControlToDateTimePicker(StiDateTimePickerControl dateTimePickerControl)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		DateTimePicker val2 = (dateTimePickerControl.Control = new DateTimePicker());
		ConvertReportControlToControl((Control)(object)val2, dateTimePickerControl);
		if (dateTimePickerControl.TagValueBinding.Length > 0)
		{
			object dataFromDataColumn = StiDataColumn.GetDataFromDataColumn(Report.Dictionary, dateTimePickerControl.TagValueBinding);
			if (dataFromDataColumn is DateTime)
			{
				dateTimePickerControl.Value = (DateTime)dataFromDataColumn;
			}
		}
		if (dateTimePickerControl.MaxDateBinding.Length > 0)
		{
			object dataFromDataColumn2 = StiDataColumn.GetDataFromDataColumn(Report.Dictionary, dateTimePickerControl.MaxDateBinding);
			if (dataFromDataColumn2 is DateTime)
			{
				dateTimePickerControl.MaxDate = (DateTime)dataFromDataColumn2;
			}
		}
		if (dateTimePickerControl.MinDateBinding.Length > 0)
		{
			object dataFromDataColumn3 = StiDataColumn.GetDataFromDataColumn(Report.Dictionary, dateTimePickerControl.MinDateBinding);
			if (dataFromDataColumn3 is DateTime)
			{
				dateTimePickerControl.MinDate = (DateTime)dataFromDataColumn3;
			}
		}
		if (dateTimePickerControl.Today)
		{
			dateTimePickerControl.Value = DateTime.Now;
		}
		val2.CalendarFont = dateTimePickerControl.Font;
		val2.CalendarForeColor = dateTimePickerControl.ForeColor;
		val2.CustomFormat = dateTimePickerControl.CustomFormat;
		val2.DropDownAlign = dateTimePickerControl.DropDownAlign;
		val2.ShowUpDown = dateTimePickerControl.ShowUpDown;
		val2.MaxDate = dateTimePickerControl.MaxDate;
		val2.MinDate = dateTimePickerControl.MinDate;
		val2.Value = dateTimePickerControl.Value;
		val2.Format = dateTimePickerControl.Format;
		val2.ValueChanged += OnValueChanged;
		return (Control)(object)val2;
	}

	protected virtual Control ConvertReportControlToCustomControl(StiCustomControl customControl)
	{
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Expected O, but got Unknown
		object control = customControl.Control;
		Control val = (Control)((control is Control) ? control : null);
		val.Location = StiScale.I(customControl.Location);
		val.Size = StiScale.I(customControl.Size);
		val.Click += Control_Click;
		val.DoubleClick += Control_DoubleClick;
		val.Enter += Control_Enter;
		val.Leave += Control_Leave;
		val.MouseDown += new MouseEventHandler(Control_MouseDown);
		val.MouseUp += new MouseEventHandler(Control_MouseUp);
		val.MouseMove += new MouseEventHandler(Control_MouseMove);
		return val;
	}

	protected virtual void ConvertDialogsToControls(Control parentControl, StiReportControl control, int tabIndex)
	{
		Control val = null;
		if (control is StiPanelControl)
		{
			val = ConvertReportControlToPanel(control as StiPanelControl);
		}
		if (control is StiTextBoxControl)
		{
			val = ConvertReportControlToTextBox(control as StiTextBoxControl);
		}
		if (control is StiRichTextBoxControl)
		{
			val = ConvertReportControlToRichTextBox(control as StiRichTextBoxControl);
		}
		if (control is StiButtonControl)
		{
			val = ConvertReportControlToButton(control as StiButtonControl);
		}
		if (control is StiCheckBoxControl)
		{
			val = ConvertReportControlToCheckBox(control as StiCheckBoxControl);
		}
		if (control is StiRadioButtonControl)
		{
			val = ConvertReportControlToRadioButton(control as StiRadioButtonControl);
		}
		if (control is StiLabelControl)
		{
			val = ConvertReportControlToLabel(control as StiLabelControl);
		}
		if (control is StiGridControl)
		{
			val = ConvertReportControlToGrid(control as StiGridControl);
		}
		if (control is StiPictureBoxControl)
		{
			val = ConvertReportControlToPictureBox(control as StiPictureBoxControl);
		}
		if (control is StiGroupBoxControl)
		{
			val = ConvertReportControlToGroupBox(control as StiGroupBoxControl);
		}
		if (control is StiListBoxControl)
		{
			val = ConvertReportControlToListBox(control as StiListBoxControl);
		}
		if (control is StiTreeViewControl)
		{
			val = ConvertReportControlToTreeView(control as StiTreeViewControl);
		}
		if (control is StiListViewControl)
		{
			val = ConvertReportControlToListView(control as StiListViewControl);
		}
		if (control is StiCheckedListBoxControl)
		{
			val = ConvertReportControlToCheckedListBox(control as StiCheckedListBoxControl);
		}
		if (control is StiLookUpBoxControl)
		{
			val = ConvertReportControlToLookUpBox(control as StiLookUpBoxControl);
		}
		else if (control is StiComboBoxControl)
		{
			val = ConvertReportControlToComboBox(control as StiComboBoxControl);
		}
		if (control is StiNumericUpDownControl)
		{
			val = ConvertReportControlToNumericUpDown(control as StiNumericUpDownControl);
		}
		if (control is StiDateTimePickerControl)
		{
			val = ConvertReportControlToDateTimePicker(control as StiDateTimePickerControl);
		}
		if (control is StiCustomControl)
		{
			if (((StiCustomControl)control).Control == null)
			{
				return;
			}
			val = ConvertReportControlToCustomControl(control as StiCustomControl);
		}
		val.TabIndex = tabIndex;
		parentControl.Controls.Add(val);
		reportToControl[control] = val;
		controlToReport[val] = control;
		if (control.IsReportContainer)
		{
			for (int num = control.Components.Count - 1; num >= 0; num--)
			{
				StiReportControl control2 = control.Components[num] as StiReportControl;
				ConvertDialogsToControls(val, control2, num);
			}
		}
	}

	protected virtual void ConvertReportControlToForm(StiForm formControl)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		Form = new Form();
		Form activeForm = Form.ActiveForm;
		if (activeForm != null)
		{
			try
			{
				uint lpdwProcessId = 0u;
				uint lpdwProcessId2 = 0u;
				uint windowThreadProcessId = GetWindowThreadProcessId(((Control)Form).Handle, out lpdwProcessId);
				uint windowThreadProcessId2 = GetWindowThreadProcessId(((Control)activeForm).Handle, out lpdwProcessId2);
				if (windowThreadProcessId == windowThreadProcessId2)
				{
					Form.Owner = activeForm;
				}
			}
			catch
			{
			}
		}
		formControl.Control = Form;
		((Control)Form).Handle.ToString();
		Form.FormBorderStyle = (FormBorderStyle)3;
		Form.MaximizeBox = false;
		Form.MinimizeBox = false;
		Form.ShowInTaskbar = false;
		Form.Location = StiScale.I(formControl.Location);
		Form.Size = new Size(StiScale.I(formControl.Size.Width), StiScale.I(formControl.Size.Height - 7) + SystemInformation.CaptionHeight);
		Form.WindowState = formControl.WindowState;
		Form.StartPosition = formControl.StartPosition;
		((Control)Form).BackColor = formControl.BackColor;
		((Control)Form).Font = formControl.Font;
		((Control)Form).Text = formControl.Text;
		((Control)Form).RightToLeft = formControl.RightToLeft;
		Form.DialogResult = formControl.DialogResult;
		Form.Closed += Form_Closed;
		Form.Closing += Form_Closing;
		((Control)Form).Click += Control_Click;
		((Control)Form).DoubleClick += Control_DoubleClick;
		reportToControl[formControl] = Form;
		controlToReport[Form] = formControl;
		for (int num = formControl.Components.Count - 1; num >= 0; num--)
		{
			if (formControl.Components[num] is StiReportControl control)
			{
				ConvertDialogsToControls((Control)(object)Form, control, num);
			}
		}
	}

	protected virtual void ConvertControlToReportControl(StiReportControl reportControl, Control control)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		reportControl.DockStyle = StiComponent.ConvertDock(control.Dock);
		reportControl.Location = StiScale.I(control.Location);
		reportControl.Size = StiScale.I(control.Size);
		reportControl.BackColor = control.BackColor;
		reportControl.ForeColor = control.ForeColor;
		reportControl.Font = control.Font;
		reportControl.RightToLeft = control.RightToLeft;
		reportControl.Enabled = control.Enabled;
		reportControl.TagValue = control.Tag;
	}

	protected virtual void ConvertPanelToReportControl(StiPanelControl panelControl, Control panel)
	{
		ConvertControlToReportControl(panelControl, panel);
		panelControl.BorderStyle = ((Stimulsoft.Report.Controls.StiPanel)(object)panel).BorderStyle;
	}

	protected virtual void ConvertTextBoxToReportControl(StiTextBoxControl textBoxControl, Control textBox)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		ConvertControlToReportControl(textBoxControl, textBox);
		textBoxControl.MaxLength = ((TextBoxBase)(TextBox)textBox).MaxLength;
		textBoxControl.PasswordChar = ((TextBox)textBox).PasswordChar;
		textBoxControl.Text = ((Control)(TextBox)textBox).Text;
		textBoxControl.Multiline = ((TextBoxBase)(TextBox)textBox).Multiline;
		textBoxControl.WordWrap = ((TextBoxBase)(TextBox)textBox).WordWrap;
		textBoxControl.AcceptsReturn = ((TextBox)textBox).AcceptsReturn;
		textBoxControl.AcceptsTab = ((TextBoxBase)(TextBox)textBox).AcceptsTab;
	}

	protected virtual void ConvertRichTextBoxToReportControl(StiRichTextBoxControl textBoxControl, Control textBox)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		ConvertControlToReportControl(textBoxControl, textBox);
		textBoxControl.RtfText = ((RichTextBox)textBox).Rtf;
	}

	protected virtual void ConvertButtonToReportControl(StiButtonControl buttonControl, Control button)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		ConvertControlToReportControl(buttonControl, button);
		buttonControl.DialogResult = ((Button)button).DialogResult;
		buttonControl.Text = button.Text;
	}

	protected virtual void ConvertCheckBoxToReportControl(StiCheckBoxControl checkBoxControl, Control checkBox)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		ConvertControlToReportControl(checkBoxControl, checkBox);
		checkBoxControl.Checked = ((CheckBox)checkBox).Checked;
		checkBoxControl.Text = checkBox.Text;
	}

	protected virtual void ConvertRadioButtonToReportControl(StiRadioButtonControl radioButtonControl, Control radioButton)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		ConvertControlToReportControl(radioButtonControl, radioButton);
		radioButtonControl.Checked = ((RadioButton)radioButton).Checked;
		radioButtonControl.Text = radioButton.Text;
	}

	protected virtual void ConvertLabelToReportControl(StiLabelControl labelControl, Control label)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		ConvertControlToReportControl(labelControl, label);
		labelControl.TextAlign = ((Label)label).TextAlign;
		labelControl.Text = label.Text;
	}

	protected virtual void ConvertGridToReportControl(StiGridControl gridControl, Control grid)
	{
		ConvertControlToReportControl(gridControl, grid);
		DataGridView val = (DataGridView)(object)((grid is DataGridView) ? grid : null);
		gridControl.ColumnHeadersVisible = val.ColumnHeadersVisible;
		gridControl.RowHeadersVisible = val.RowHeadersVisible;
		gridControl.HeaderFont = val.ColumnHeadersDefaultCellStyle.Font;
		gridControl.PreferredRowHeight = val.RowTemplate.Height;
		gridControl.RowHeaderWidth = val.ColumnHeadersHeight;
		gridControl.AlternatingBackColor = val.AlternatingRowsDefaultCellStyle.BackColor;
		gridControl.BackgroundColor = val.BackgroundColor;
		gridControl.ForeColor = ((Control)val).ForeColor;
		gridControl.BackColor = ((Control)val).BackColor;
	}

	protected virtual void ConvertPictureBoxToReportControl(StiPictureBoxControl pictureBoxControl, Control pictureBox)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		ConvertControlToReportControl(pictureBoxControl, pictureBox);
		pictureBoxControl.SizeMode = ((PictureBox)pictureBox).SizeMode;
		pictureBoxControl.BorderStyle = ((PictureBox)pictureBox).BorderStyle;
		pictureBoxControl.Image = ((PictureBox)pictureBox).Image;
	}

	protected virtual void ConvertGroupBoxToReportControl(StiGroupBoxControl groupBoxControl, Control groupBox)
	{
		ConvertControlToReportControl(groupBoxControl, groupBox);
		groupBoxControl.Text = groupBox.Text;
	}

	protected virtual void ConvertListBoxToReportControl(StiListBoxControl listBoxControl, ListBox listBox)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		ConvertControlToReportControl(listBoxControl, (Control)(object)listBox);
		listBoxControl.SelectionMode = listBox.SelectionMode;
		listBoxControl.Sorted = listBox.Sorted;
		listBoxControl.SelectedIndex = ((ListControl)listBox).SelectedIndex;
		listBoxControl.SelectedItem = listBox.SelectedItem;
		listBoxControl.SelectedValue = ((ListControl)listBox).SelectedValue;
		listBoxControl.ItemHeight = (int)((double)listBox.ItemHeight / StiScale.Factor);
		listBoxControl.Items.ClearCore();
		foreach (object item in listBox.Items)
		{
			listBoxControl.Items.AddCore(item);
		}
	}

	protected virtual void ConvertTreeViewToReportControl(StiTreeViewControl treeViewControl, TreeView treeView)
	{
		ConvertControlToReportControl(treeViewControl, (Control)(object)treeView);
	}

	protected virtual void ConvertListViewToReportControl(StiListViewControl listViewControl, ListView listView)
	{
		ConvertControlToReportControl(listViewControl, (Control)(object)listView);
	}

	protected virtual void ConvertCheckedListBoxToReportControl(StiCheckedListBoxControl checkedListBoxControl, CheckedListBox checkedListBox)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		ConvertControlToReportControl(checkedListBoxControl, (Control)(object)checkedListBox);
		checkedListBoxControl.SelectionMode = ((ListBox)checkedListBox).SelectionMode;
		checkedListBoxControl.Sorted = ((ListBox)checkedListBox).Sorted;
		checkedListBoxControl.SelectedIndex = ((ListControl)checkedListBox).SelectedIndex;
		checkedListBoxControl.SelectedItem = ((ListBox)checkedListBox).SelectedItem;
		checkedListBoxControl.SelectedValue = ((ListControl)checkedListBox).SelectedValue;
		checkedListBoxControl.ItemHeight = (int)((double)((ListBox)checkedListBox).ItemHeight / StiScale.Factor);
		checkedListBoxControl.CheckOnClick = checkedListBox.CheckOnClick;
		object[] array = new object[checkedListBox.CheckedItems.Count];
		int num = 0;
		foreach (object checkedItem in checkedListBox.CheckedItems)
		{
			array[num++] = checkedItem;
		}
		checkedListBoxControl.CheckedItems = array;
		checkedListBoxControl.Items.ClearCore();
		foreach (object item in (ObjectCollection)checkedListBox.Items)
		{
			checkedListBoxControl.Items.AddCore(item);
		}
	}

	protected virtual void ConvertComboBoxToReportControl(StiComboBoxControl comboBoxControl, ComboBox comboBox)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		ConvertControlToReportControl(comboBoxControl, (Control)(object)comboBox);
		comboBoxControl.DropDownStyle = comboBox.DropDownStyle;
		comboBoxControl.MaxLength = comboBox.MaxLength;
		comboBoxControl.Sorted = comboBox.Sorted;
		comboBoxControl.SelectedIndex = ((ListControl)comboBox).SelectedIndex;
		comboBoxControl.SelectedItem = comboBox.SelectedItem;
		comboBoxControl.SelectedValue = ((ListControl)comboBox).SelectedValue;
		comboBoxControl.ItemHeight = (int)((double)comboBox.ItemHeight / StiScale.Factor);
		comboBoxControl.MaxDropDownItems = comboBox.MaxDropDownItems;
		comboBoxControl.DropDownWidth = (int)((double)comboBox.DropDownWidth / StiScale.Factor);
		comboBoxControl.Text = ((Control)comboBox).Text;
		comboBoxControl.Items.ClearCore();
		foreach (object item in comboBox.Items)
		{
			comboBoxControl.Items.AddCore(item);
		}
	}

	protected virtual void ConvertLookUpBoxToReportControl(StiLookUpBoxControl lookUpBoxControl, StiLookUpBox lookUpBox)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		ConvertControlToReportControl(lookUpBoxControl, (Control)(object)lookUpBox);
		lookUpBoxControl.DropDownStyle = ((ComboBox)lookUpBox).DropDownStyle;
		lookUpBoxControl.MaxLength = ((ComboBox)lookUpBox).MaxLength;
		lookUpBoxControl.Sorted = ((ComboBox)lookUpBox).Sorted;
		lookUpBoxControl.SelectedIndex = ((ListControl)lookUpBox).SelectedIndex;
		lookUpBoxControl.SelectedItem = ((ComboBox)lookUpBox).SelectedItem;
		lookUpBoxControl.SelectedValue = ((ListControl)lookUpBox).SelectedValue;
		lookUpBoxControl.SelectedKey = lookUpBox.SelectedKey;
		lookUpBoxControl.ItemHeight = (int)((double)((ComboBox)lookUpBox).ItemHeight / StiScale.Factor);
		lookUpBoxControl.MaxDropDownItems = ((ComboBox)lookUpBox).MaxDropDownItems;
		lookUpBoxControl.DropDownWidth = (int)((double)((ComboBox)lookUpBox).DropDownWidth / StiScale.Factor);
		lookUpBoxControl.Text = ((Control)lookUpBox).Text;
		lookUpBoxControl.Items.Clear();
		foreach (object item in ((ComboBox)lookUpBox).Items)
		{
			lookUpBoxControl.Items.Add(item);
		}
		lookUpBoxControl.Keys.Clear();
		foreach (object key in lookUpBox.Keys)
		{
			lookUpBoxControl.Keys.Add(key);
		}
	}

	protected virtual void ConvertNumericUpDownToReportControl(StiNumericUpDownControl numericUpDownControl, NumericUpDown numericUpDown)
	{
		ConvertControlToReportControl(numericUpDownControl, (Control)(object)numericUpDown);
		numericUpDownControl.Minimum = (int)numericUpDown.Minimum;
		numericUpDownControl.Maximum = (int)numericUpDown.Maximum;
		numericUpDownControl.Increment = (int)numericUpDown.Increment;
		numericUpDownControl.Value = (int)numericUpDown.Value;
	}

	protected virtual void ConvertDateTimePickerToReportControl(StiDateTimePickerControl dateTimePickerControl, DateTimePicker dateTimePicker)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		ConvertControlToReportControl(dateTimePickerControl, (Control)(object)dateTimePicker);
		dateTimePickerControl.CustomFormat = dateTimePicker.CustomFormat;
		dateTimePickerControl.DropDownAlign = dateTimePicker.DropDownAlign;
		dateTimePickerControl.ShowUpDown = dateTimePicker.ShowUpDown;
		dateTimePickerControl.MaxDate = dateTimePicker.MaxDate;
		dateTimePickerControl.MinDate = dateTimePicker.MinDate;
		dateTimePickerControl.Value = dateTimePicker.Value;
		dateTimePickerControl.Format = dateTimePicker.Format;
	}

	protected virtual void ConvertControlsToDialogs(Control parentControl, StiReportControl control)
	{
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Expected O, but got Unknown
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Expected O, but got Unknown
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Expected O, but got Unknown
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Expected O, but got Unknown
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Expected O, but got Unknown
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Expected O, but got Unknown
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Expected O, but got Unknown
		object? obj = reportToControl[control];
		Control val = (Control)((obj is Control) ? obj : null);
		if (val == null)
		{
			return;
		}
		if (control is StiPanelControl)
		{
			ConvertPanelToReportControl(control as StiPanelControl, val);
		}
		if (control is StiTextBoxControl)
		{
			ConvertTextBoxToReportControl(control as StiTextBoxControl, val);
		}
		if (control is StiRichTextBoxControl)
		{
			ConvertRichTextBoxToReportControl(control as StiRichTextBoxControl, val);
		}
		if (control is StiButtonControl)
		{
			ConvertButtonToReportControl(control as StiButtonControl, val);
		}
		if (control is StiCheckBoxControl)
		{
			ConvertCheckBoxToReportControl(control as StiCheckBoxControl, val);
		}
		if (control is StiRadioButtonControl)
		{
			ConvertRadioButtonToReportControl(control as StiRadioButtonControl, val);
		}
		if (control is StiLabelControl)
		{
			ConvertLabelToReportControl(control as StiLabelControl, val);
		}
		if (control is StiGridControl)
		{
			ConvertGridToReportControl(control as StiGridControl, val);
		}
		if (control is StiPictureBoxControl)
		{
			ConvertPictureBoxToReportControl(control as StiPictureBoxControl, val);
		}
		if (control is StiGroupBoxControl)
		{
			ConvertGroupBoxToReportControl(control as StiGroupBoxControl, val);
		}
		if (control is StiListBoxControl)
		{
			ConvertListBoxToReportControl(control as StiListBoxControl, (ListBox)val);
		}
		if (control is StiTreeViewControl)
		{
			ConvertTreeViewToReportControl(control as StiTreeViewControl, (TreeView)val);
		}
		if (control is StiListViewControl)
		{
			ConvertListViewToReportControl(control as StiListViewControl, (ListView)val);
		}
		if (control is StiCheckedListBoxControl)
		{
			ConvertCheckedListBoxToReportControl(control as StiCheckedListBoxControl, (CheckedListBox)val);
		}
		if (control is StiLookUpBoxControl)
		{
			ConvertLookUpBoxToReportControl(control as StiLookUpBoxControl, (StiLookUpBox)(object)val);
		}
		else if (control is StiComboBoxControl)
		{
			ConvertComboBoxToReportControl(control as StiComboBoxControl, (ComboBox)val);
		}
		if (control is StiNumericUpDownControl)
		{
			ConvertNumericUpDownToReportControl(control as StiNumericUpDownControl, (NumericUpDown)val);
		}
		if (control is StiDateTimePickerControl)
		{
			ConvertDateTimePickerToReportControl(control as StiDateTimePickerControl, (DateTimePicker)val);
		}
		if (!control.IsReportContainer)
		{
			return;
		}
		foreach (StiReportControl component in control.Components)
		{
			ConvertControlsToDialogs(val, component);
		}
	}

	protected virtual void FormToReportControl(StiForm formControl)
	{
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		if (formControl == null)
		{
			return;
		}
		formControl.Location = new Point((int)((double)Form.Location.X / StiScale.Factor), (int)((double)Form.Location.Y / StiScale.Factor));
		formControl.Size = new Size((int)((double)Form.Size.Width / StiScale.Factor), (int)((double)Form.Size.Height / StiScale.Factor));
		formControl.WindowState = Form.WindowState;
		formControl.StartPosition = Form.StartPosition;
		formControl.BackColor = ((Control)Form).BackColor;
		formControl.Font = ((Control)Form).Font;
		formControl.Text = ((Control)Form).Text;
		formControl.RightToLeft = ((Control)Form).RightToLeft;
		formControl.DialogResult = Form.DialogResult;
		foreach (StiComponent component in formControl.Components)
		{
			if (component is StiReportControl control)
			{
				ConvertControlsToDialogs((Control)(object)Form, control);
			}
		}
	}

	private void OnFormClose(object sender, EventArgs e)
	{
		Form.Close();
	}

	private void OnReportControlUpdate(object sender, StiReportControlUpdateEventArgs e)
	{
		if (lockReportControlUpdate)
		{
			return;
		}
		string propertyName = e.PropertyName;
		object? obj = reportToControl[sender];
		Control val = (Control)((obj is Control) ? obj : null);
		StiComponent stiComponent = sender as StiComponent;
		PropertyInfo propertyInfo = ((propertyName == "RightToLeft") ? ((object)stiComponent).GetType().GetProperty(propertyName, typeof(RightToLeft)) : ((!(propertyName == "BorderStyle")) ? ((object)stiComponent).GetType().GetProperty(propertyName) : ((object)stiComponent).GetType().GetProperty(propertyName, typeof(BorderStyle))));
		PropertyInfo propertyInfo2 = null;
		try
		{
			if (propertyName == "BorderStyle")
			{
				propertyInfo2 = ((object)val).GetType().GetProperty(propertyName, typeof(BorderStyle));
			}
			else if (propertyName == "Value" && stiComponent is StiDateTimePickerControl)
			{
				propertyInfo2 = ((object)val).GetType().GetProperty(propertyName, typeof(DateTime));
			}
			else if (propertyName == "Value")
			{
				propertyInfo2 = ((object)val).GetType().GetProperty(propertyName, typeof(decimal));
			}
			else if (propertyName == "Filter")
			{
				DataGridView val2 = (DataGridView)(object)((val is DataGridView) ? val : null);
				if (val2 != null)
				{
					object value = propertyInfo.GetValue(stiComponent, null);
					if (val2.DataSource is DataView dataView)
					{
						dataView.RowFilter = value as string;
					}
				}
			}
			else
			{
				PropertyInfo[] properties = ((object)val).GetType().GetProperties();
				PropertyInfo[] array = properties;
				foreach (PropertyInfo propertyInfo3 in array)
				{
					if (propertyName == propertyInfo3.Name && (propertyInfo3.PropertyType == propertyInfo.PropertyType || propertyName == "Items" || propertyName == "Keys"))
					{
						propertyInfo2 = propertyInfo3;
						break;
					}
				}
			}
		}
		catch
		{
		}
		if (propertyInfo == null || propertyInfo2 == null)
		{
			return;
		}
		object value2 = propertyInfo.GetValue(stiComponent, null);
		if (value2 is StiArrayList && propertyName == "Items" && val is ListBox)
		{
			ListBox val3 = (ListBox)(object)((val is ListBox) ? val : null);
			val3.Items.Clear();
			if (stiComponent is StiCheckedListBoxControl)
			{
				val3.Items.AddRange(((StiCheckedListBoxControl)stiComponent).Items.ToArray());
			}
			else
			{
				val3.Items.AddRange(((StiListBoxControl)stiComponent).Items.ToArray());
			}
			return;
		}
		if (value2 is StiArrayList && propertyName == "Items" && val is ComboBox)
		{
			ComboBox val4 = (ComboBox)(object)((val is ComboBox) ? val : null);
			val4.Items.Clear();
			val4.Items.AddRange(((StiComboBoxControl)stiComponent).Items.ToArray());
			return;
		}
		if (value2 is StiArrayList && propertyName == "Keys" && val is StiLookUpBox)
		{
			StiLookUpBox stiLookUpBox = val as StiLookUpBox;
			stiLookUpBox.Keys.Clear();
			stiLookUpBox.Keys.AddRange(((StiLookUpBoxControl)stiComponent).Keys.ToArray());
			return;
		}
		if (value2 is StiArrayList && propertyName == "Items" && val is CheckedListBox)
		{
			CheckedListBox val5 = (CheckedListBox)(object)((val is CheckedListBox) ? val : null);
			((ObjectCollection)val5.Items).Clear();
			((ObjectCollection)val5.Items).AddRange(((StiCheckedListBoxControl)stiComponent).Items.ToArray());
			return;
		}
		if (propertyName == "Filter")
		{
			DataGridView val6 = (DataGridView)(object)((val is DataGridView) ? val : null);
			if (val6 != null && val6.DataSource is DataView dataView2)
			{
				dataView2.RowFilter = value2 as string;
			}
			return;
		}
		try
		{
			if (value2 is IConvertible)
			{
				value2 = Convert.ChangeType(value2, propertyInfo2.PropertyType);
				propertyInfo2.SetValue(val, value2, null);
			}
		}
		catch
		{
		}
	}

	public override void PrepareForm()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		controlToReport = new Hashtable();
		reportToControl = new Hashtable();
		managerToGridControl = new Hashtable();
		toolTip = new ToolTip();
	}

	public override void DisposeForm()
	{
		if (controlToReport != null)
		{
			controlToReport.Clear();
		}
		if (reportToControl != null)
		{
			reportToControl.Clear();
		}
		if (managerToGridControl != null)
		{
			managerToGridControl.Clear();
		}
		controlToReport = null;
		reportToControl = null;
		managerToGridControl = null;
		if (Form != null)
		{
			((Component)(object)Form).Dispose();
		}
		Form = null;
		if (toolTip != null)
		{
			((Component)(object)toolTip).Dispose();
		}
		toolTip = null;
		formControl = null;
	}

	public override void LoadForm(IStiForm formControl)
	{
		this.formControl = formControl as StiForm;
		ConvertReportControlToForm(formControl as StiForm);
		this.formControl.ReportControlUpdate += OnReportControlUpdate;
		this.formControl.FormClose += OnFormClose;
		this.formControl.InvokeLoadForm(EventArgs.Empty);
	}

	public override void CloseForm()
	{
		if (formControl != null && Form != null)
		{
			formControl.ReportControlUpdate -= OnReportControlUpdate;
			formControl.FormClose -= OnFormClose;
			FormToReportControl(formControl);
		}
	}

	public override bool RenderForm(IStiForm formControl)
	{
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Invalid comparison between Unknown and I4
		try
		{
			this.formControl = formControl as StiForm;
			PrepareForm();
			if (formControl.Visible)
			{
				if (Report.Progress != null && Report.Progress.IsVisible)
				{
					Report.Progress.Hide();
				}
				try
				{
					LoadForm(formControl);
					if (formControl.Visible)
					{
						DialogResult val = Form.ShowDialog();
						CloseForm();
						if ((int)val == 2)
						{
							return false;
						}
					}
				}
				catch (Exception ex)
				{
					StiExceptionProvider.Show(ex, false, (Form)null);
					return false;
				}
			}
		}
		finally
		{
			CloseForm();
		}
		return true;
	}

	public override bool Render(StiReport report, StiFormStartMode startMode)
	{
		try
		{
			Report = report;
			foreach (StiPage page in report.Pages)
			{
				if (page is StiForm stiForm && stiForm.StartMode == startMode && !RenderForm(stiForm))
				{
					return false;
				}
			}
			return true;
		}
		finally
		{
			Report = null;
		}
	}

	public override IStiForm CreateForm(StiReport report)
	{
		return new StiForm(report);
	}

	public override IStiTextBoxControl CreateTextBoxControl()
	{
		return new StiTextBoxControl();
	}

	public override IStiLabelControl CreateLabelControl()
	{
		return new StiLabelControl();
	}

	public override IStiCheckBoxControl CreateCheckBoxControl()
	{
		return new StiCheckBoxControl();
	}

	public override IStiPictureBoxControl CreatePictureBoxControl()
	{
		return new StiPictureBoxControl();
	}

	public StiWinDialogsProvider(StiReport report)
	{
		Report = report;
	}

	public StiWinDialogsProvider()
		: this(null)
	{
	}
}
