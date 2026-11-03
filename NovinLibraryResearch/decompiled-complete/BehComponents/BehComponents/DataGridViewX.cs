using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BehComponents;

[Description("DataGridView with auto calendar column. v2")]
[ToolboxItem(true)]
[ToolboxBitmap(typeof(DataGridViewX), "DataGridViewX.bmp")]
public class DataGridViewX : DataGridView
{
	private class DataGridViewColumn_PersianDate : DataGridViewColumn
	{
		public override DataGridViewCell CellTemplate
		{
			get
			{
				return ((DataGridViewColumn)this).CellTemplate;
			}
			set
			{
				if (value != null && !((object)value).GetType().IsAssignableFrom(typeof(CalendarCell_PersianDate)))
				{
					throw new InvalidCastException("Must be a CalendarCell");
				}
				((DataGridViewColumn)this).CellTemplate = value;
			}
		}

		public DataGridViewColumn_PersianDate()
			: base((DataGridViewCell)(object)new CalendarCell_PersianDate())
		{
		}
	}

	private class CalendarCell_PersianDate : DataGridViewTextBoxCell
	{
		public override Type EditType => typeof(CalendarEditingControl_PersianDate);

		public override Type ValueType => typeof(DateTime);

		public override object DefaultNewRowValue => DBNull.Value;

		public CalendarCell_PersianDate()
		{
			((DataGridViewCell)this).Style.Format = "d";
		}

		public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
		{
			((DataGridViewTextBoxCell)this).InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
			CalendarEditingControl_PersianDate calendarEditingControl_PersianDate = ((DataGridViewElement)this).DataGridView.EditingControl as CalendarEditingControl_PersianDate;
			if (((DataGridViewCell)this).Value == null || ((DataGridViewCell)this).Value.ToString() == "")
			{
				((Control)calendarEditingControl_PersianDate).Text = PersianDateTime.Today.ToShortDateString();
			}
			else if (((DataGridViewCell)this).Value != DBNull.Value)
			{
				DateTime value = DateTime.Parse(((DataGridViewCell)this).Value.ToString());
				((Control)calendarEditingControl_PersianDate).Text = new PersianDateTime(value).ToShortDateString();
			}
		}
	}

	private class CalendarEditingControl_PersianDate : DateTimePickerX, IDataGridViewEditingControl
	{
		private DataGridView dataGridView;

		private bool valueChanged;

		private int rowIndex;

		private string valueCell = "";

		public object EditingControlFormattedValue
		{
			get
			{
				string text = "";
				if (text != null && text.ToString() != "")
				{
					base.SelectedDate = ((Control)this).Text;
				}
				return GetSelectedDateInDateTime().ToShortDateString();
			}
			set
			{
				if (value is string)
				{
					if (base.CalendarType == CalendarTypes.Persian)
					{
						base.SelectedDate = PersianDateTime.Parse(value.ToString(), "yyyy/MM/dd").ToShortDateString();
					}
					else
					{
						base.SelectedDate = DateTime.Parse(value.ToString()).ToShortDateString();
					}
				}
			}
		}

		public int EditingControlRowIndex
		{
			get
			{
				return rowIndex;
			}
			set
			{
				rowIndex = value;
			}
		}

		public bool RepositionEditingControlOnValueChange => false;

		public DataGridView EditingControlDataGridView
		{
			get
			{
				return dataGridView;
			}
			set
			{
				dataGridView = value;
			}
		}

		public bool EditingControlValueChanged
		{
			get
			{
				return valueChanged;
			}
			set
			{
				valueChanged = value;
				valueCell = ((Control)this).Text;
				base.SelectedDate = valueCell;
			}
		}

		public Cursor EditingPanelCursor => ((Control)this).Cursor;

		public CalendarEditingControl_PersianDate()
		{
			base.Format = FormatDate.Short;
			base.CalendarType = CalendarTypes.Persian;
			base.DropDownClosedWhenSelectedDateChanged = false;
			base.DropDownClosedWhenClickOnDays = true;
			base.RightToLeftLayout = true;
			base.DockSide = Alignments.Left;
			base.OnCloseDropDown += dateTimePicker_OnCloseDropDown;
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Invalid comparison between Unknown and I4
			base.OnKeyDown(e);
			if ((int)e.KeyCode == 113)
			{
				OpenDropDown();
			}
		}

		private void dateTimePicker_OnCloseDropDown(object sender, EventArgs e)
		{
			EditingControlFormattedValue = base.SelectedDate;
			valueChanged = true;
			EditingControlDataGridView.NotifyCurrentCellDirty(true);
		}

		public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
		{
			return EditingControlFormattedValue;
		}

		public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
		{
		}

		public bool EditingControlWantsInputKey(Keys key, bool dataGridViewWantsInputKey)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Expected I4, but got Unknown
			Keys val = (Keys)(key & 0xFFFF);
			switch (val - 33)
			{
			case 0:
			case 1:
			case 2:
			case 3:
			case 4:
			case 5:
			case 6:
			case 7:
				return true;
			default:
				return false;
			}
		}

		public void PrepareEditingControlForEdit(bool selectAll)
		{
		}
	}

	private class DataGridViewColumn_EnglishDate : DataGridViewColumn
	{
		public override DataGridViewCell CellTemplate
		{
			get
			{
				return ((DataGridViewColumn)this).CellTemplate;
			}
			set
			{
				if (value != null && !((object)value).GetType().IsAssignableFrom(typeof(CalendarCell_EnglishDate)))
				{
					throw new InvalidCastException("Must be a CalendarCell");
				}
				((DataGridViewColumn)this).CellTemplate = value;
			}
		}

		public DataGridViewColumn_EnglishDate()
			: base((DataGridViewCell)(object)new CalendarCell_EnglishDate())
		{
		}
	}

	private class CalendarCell_EnglishDate : DataGridViewTextBoxCell
	{
		public override Type EditType => typeof(CalendarEditingControl_EnglishDate);

		public override Type ValueType => typeof(DateTime);

		public override object DefaultNewRowValue => DBNull.Value;

		public CalendarCell_EnglishDate()
		{
			((DataGridViewCell)this).Style.Format = "d";
		}

		public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
		{
			((DataGridViewTextBoxCell)this).InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
			CalendarEditingControl_EnglishDate calendarEditingControl_EnglishDate = ((DataGridViewElement)this).DataGridView.EditingControl as CalendarEditingControl_EnglishDate;
			if (((DataGridViewCell)this).Value == null || ((DataGridViewCell)this).Value.ToString() == "")
			{
				((Control)calendarEditingControl_EnglishDate).Text = DateTime.Today.ToShortDateString();
			}
			else if (((DataGridViewCell)this).Value != DBNull.Value)
			{
				((Control)calendarEditingControl_EnglishDate).Text = DateTime.Parse(((DataGridViewCell)this).Value.ToString()).ToShortDateString();
			}
		}
	}

	private class CalendarEditingControl_EnglishDate : DateTimePickerX, IDataGridViewEditingControl
	{
		private DataGridView dataGridView;

		private bool valueChanged;

		private int rowIndex;

		private string valueCell = "";

		public object EditingControlFormattedValue
		{
			get
			{
				string text = "";
				text = ((Control)this).Text;
				if (text != null && text.ToString() != "")
				{
					base.SelectedDate = text;
				}
				return text;
			}
			set
			{
				if (value is string)
				{
					if (base.CalendarType == CalendarTypes.Persian)
					{
						base.SelectedDate = PersianDateTime.Parse(value.ToString(), "yyyy/MM/dd").ToShortDateString();
					}
					else
					{
						base.SelectedDate = DateTime.Parse(value.ToString()).ToShortDateString();
					}
				}
			}
		}

		public int EditingControlRowIndex
		{
			get
			{
				return rowIndex;
			}
			set
			{
				rowIndex = value;
			}
		}

		public bool RepositionEditingControlOnValueChange => false;

		public DataGridView EditingControlDataGridView
		{
			get
			{
				return dataGridView;
			}
			set
			{
				dataGridView = value;
			}
		}

		public bool EditingControlValueChanged
		{
			get
			{
				return valueChanged;
			}
			set
			{
				valueChanged = value;
				valueCell = ((Control)this).Text;
				base.SelectedDate = valueCell;
			}
		}

		public Cursor EditingPanelCursor => ((Control)this).Cursor;

		public CalendarEditingControl_EnglishDate()
		{
			base.Format = FormatDate.Short;
			base.CalendarType = CalendarTypes.English;
			base.DropDownClosedWhenSelectedDateChanged = false;
			base.DropDownClosedWhenClickOnDays = true;
			base.RightToLeftLayout = false;
			base.DockSide = Alignments.Right;
			base.OnCloseDropDown += dateTimePicker_OnCloseDropDown;
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Invalid comparison between Unknown and I4
			base.OnKeyDown(e);
			if ((int)e.KeyCode == 113)
			{
				OpenDropDown();
			}
		}

		private void dateTimePicker_OnCloseDropDown(object sender, EventArgs e)
		{
			EditingControlFormattedValue = base.SelectedDate;
			valueChanged = true;
			EditingControlDataGridView.NotifyCurrentCellDirty(true);
		}

		public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
		{
			return EditingControlFormattedValue;
		}

		public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
		{
		}

		public bool EditingControlWantsInputKey(Keys key, bool dataGridViewWantsInputKey)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Expected I4, but got Unknown
			Keys val = (Keys)(key & 0xFFFF);
			switch (val - 33)
			{
			case 0:
			case 1:
			case 2:
			case 3:
			case 4:
			case 5:
			case 6:
			case 7:
				return true;
			default:
				return false;
			}
		}

		public void PrepareEditingControlForEdit(bool selectAll)
		{
		}
	}

	private List<string> _ListColumns_PersianDate = new List<string>();

	private List<string> _ListColumns_EnglishDate = new List<string>();

	private bool blnColumnTypeChanging;

	private IContainer components;

	[Browsable(false)]
	public string Programmer => "Saeed Mohammadi";

	[Browsable(false)]
	public string ProgrammerWebSite => "http://BehSoft.blogfa.com/";

	[Browsable(false)]
	public List<string> Columns_PersianDate
	{
		get
		{
			return _ListColumns_PersianDate;
		}
		set
		{
			_ListColumns_PersianDate = value;
		}
	}

	[Browsable(false)]
	public List<string> Columns_EnglishDate
	{
		get
		{
			return _ListColumns_EnglishDate;
		}
		set
		{
			_ListColumns_EnglishDate = value;
		}
	}

	public DataGridViewX()
	{
		InitializeComponent();
	}

	public DataGridViewX(IContainer container)
	{
		container.Add((IComponent?)this);
		InitializeComponent();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((DataGridView)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		components = new Container();
	}

	protected override void OnCellEndEdit(DataGridViewCellEventArgs e)
	{
		((DataGridView)this).OnCellEndEdit(e);
	}

	protected override void OnColumnAdded(DataGridViewColumnEventArgs e)
	{
		((DataGridView)this).OnColumnAdded(e);
		if (!blnColumnTypeChanging)
		{
			SetCalendarColumns(e.Column.Name);
		}
	}

	private void SetCalendarColumns(string onlyColumn)
	{
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0427: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_058f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0594: Unknown result type (might be due to invalid IL or missing references)
		//IL_0358: Unknown result type (might be due to invalid IL or missing references)
		//IL_0623: Unknown result type (might be due to invalid IL or missing references)
		bool flag = false;
		bool flag2 = false;
		string text = onlyColumn.ToLower();
		foreach (string item in _ListColumns_EnglishDate)
		{
			if (text == item.ToLower())
			{
				flag2 = true;
				break;
			}
		}
		foreach (string item2 in _ListColumns_PersianDate)
		{
			if (text == item2.ToLower())
			{
				flag = true;
				break;
			}
		}
		for (int i = 0; i < ((BaseCollection)((DataGridView)this).Columns).Count; i++)
		{
			if (((DataGridView)this).Columns[i].Name.ToLower() == text && flag && (object)((DataGridView)this).Columns[i].ValueType == typeof(DateTime))
			{
				DataGridViewColumn_PersianDate dataGridViewColumn_PersianDate = new DataGridViewColumn_PersianDate();
				((DataGridViewColumn)dataGridViewColumn_PersianDate).DataPropertyName = ((DataGridView)this).Columns[i].DataPropertyName;
				((DataGridViewColumn)dataGridViewColumn_PersianDate).Name = ((DataGridView)this).Columns[i].Name;
				((DataGridViewColumn)dataGridViewColumn_PersianDate).HeaderText = ((DataGridView)this).Columns[i].HeaderText;
				((DataGridViewColumn)dataGridViewColumn_PersianDate).DisplayIndex = ((DataGridView)this).Columns[i].DisplayIndex;
				((DataGridViewColumn)dataGridViewColumn_PersianDate).AutoSizeMode = ((DataGridView)this).Columns[i].AutoSizeMode;
				((DataGridViewBand)dataGridViewColumn_PersianDate).ContextMenuStrip = ((DataGridViewBand)((DataGridView)this).Columns[i]).ContextMenuStrip;
				((DataGridViewBand)dataGridViewColumn_PersianDate).DefaultCellStyle = ((DataGridViewBand)((DataGridView)this).Columns[i]).DefaultCellStyle;
				((DataGridViewColumn)dataGridViewColumn_PersianDate).DividerWidth = ((DataGridView)this).Columns[i].DividerWidth;
				((DataGridViewBand)dataGridViewColumn_PersianDate).Frozen = ((DataGridViewBand)((DataGridView)this).Columns[i]).Frozen;
				((DataGridViewColumn)dataGridViewColumn_PersianDate).HeaderCell = ((DataGridView)this).Columns[i].HeaderCell;
				((DataGridViewColumn)dataGridViewColumn_PersianDate).MinimumWidth = ((DataGridView)this).Columns[i].MinimumWidth;
				((DataGridViewBand)dataGridViewColumn_PersianDate).ReadOnly = ((DataGridViewBand)((DataGridView)this).Columns[i]).ReadOnly;
				((DataGridViewBand)dataGridViewColumn_PersianDate).Visible = ((DataGridViewBand)((DataGridView)this).Columns[i]).Visible;
				((DataGridViewColumn)dataGridViewColumn_PersianDate).Width = ((DataGridView)this).Columns[i].Width;
				((DataGridViewColumn)dataGridViewColumn_PersianDate).ValueType = typeof(DateTime);
				List<object> list = new List<object>();
				for (int j = 0; j < ((DataGridView)this).Rows.Count; j++)
				{
					if (!((DataGridView)this).Rows[j].IsNewRow)
					{
						list.Add(((DataGridView)this).Rows[j].Cells[((DataGridViewColumn)dataGridViewColumn_PersianDate).Name].Value);
					}
				}
				DataGridViewColumnSortMode sortMode = ((DataGridView)this).Columns[i].SortMode;
				((DataGridView)this).Columns.Remove(((DataGridView)this).Columns[i].Name);
				blnColumnTypeChanging = true;
				((DataGridView)this).Columns.Add((DataGridViewColumn)(object)dataGridViewColumn_PersianDate);
				blnColumnTypeChanging = false;
				for (int k = 0; k < list.Count; k++)
				{
					((DataGridView)this).Rows[k].Cells[((DataGridViewColumn)dataGridViewColumn_PersianDate).Name].Value = list[k];
				}
				((DataGridView)this).Columns[((DataGridViewColumn)dataGridViewColumn_PersianDate).Name].SortMode = sortMode;
			}
			else
			{
				if (!(((DataGridView)this).Columns[i].Name.ToLower() == text) || !flag2 || (object)((DataGridView)this).Columns[i].ValueType != typeof(DateTime))
				{
					continue;
				}
				DataGridViewColumn_EnglishDate dataGridViewColumn_EnglishDate = new DataGridViewColumn_EnglishDate();
				((DataGridViewColumn)dataGridViewColumn_EnglishDate).DataPropertyName = ((DataGridView)this).Columns[i].DataPropertyName;
				((DataGridViewColumn)dataGridViewColumn_EnglishDate).Name = ((DataGridView)this).Columns[i].Name;
				((DataGridViewColumn)dataGridViewColumn_EnglishDate).HeaderText = ((DataGridView)this).Columns[i].HeaderText;
				((DataGridViewColumn)dataGridViewColumn_EnglishDate).DisplayIndex = ((DataGridView)this).Columns[i].DisplayIndex;
				((DataGridViewColumn)dataGridViewColumn_EnglishDate).AutoSizeMode = ((DataGridView)this).Columns[i].AutoSizeMode;
				((DataGridViewBand)dataGridViewColumn_EnglishDate).ContextMenuStrip = ((DataGridViewBand)((DataGridView)this).Columns[i]).ContextMenuStrip;
				((DataGridViewBand)dataGridViewColumn_EnglishDate).DefaultCellStyle = ((DataGridViewBand)((DataGridView)this).Columns[i]).DefaultCellStyle;
				((DataGridViewColumn)dataGridViewColumn_EnglishDate).DividerWidth = ((DataGridView)this).Columns[i].DividerWidth;
				((DataGridViewBand)dataGridViewColumn_EnglishDate).Frozen = ((DataGridViewBand)((DataGridView)this).Columns[i]).Frozen;
				((DataGridViewColumn)dataGridViewColumn_EnglishDate).HeaderCell = ((DataGridView)this).Columns[i].HeaderCell;
				((DataGridViewColumn)dataGridViewColumn_EnglishDate).MinimumWidth = ((DataGridView)this).Columns[i].MinimumWidth;
				((DataGridViewBand)dataGridViewColumn_EnglishDate).ReadOnly = ((DataGridViewBand)((DataGridView)this).Columns[i]).ReadOnly;
				((DataGridViewBand)dataGridViewColumn_EnglishDate).Visible = ((DataGridViewBand)((DataGridView)this).Columns[i]).Visible;
				((DataGridViewColumn)dataGridViewColumn_EnglishDate).Width = ((DataGridView)this).Columns[i].Width;
				((DataGridViewColumn)dataGridViewColumn_EnglishDate).ValueType = typeof(DateTime);
				List<object> list2 = new List<object>();
				for (int l = 0; l < ((DataGridView)this).Rows.Count; l++)
				{
					if (!((DataGridView)this).Rows[l].IsNewRow)
					{
						list2.Add(((DataGridView)this).Rows[l].Cells[((DataGridViewColumn)dataGridViewColumn_EnglishDate).Name].Value);
					}
				}
				DataGridViewColumnSortMode sortMode2 = ((DataGridView)this).Columns[i].SortMode;
				((DataGridView)this).Columns.Remove(((DataGridView)this).Columns[i].Name);
				blnColumnTypeChanging = true;
				((DataGridView)this).Columns.Add((DataGridViewColumn)(object)dataGridViewColumn_EnglishDate);
				blnColumnTypeChanging = false;
				for (int m = 0; m < list2.Count; m++)
				{
					((DataGridView)this).Rows[m].Cells[((DataGridViewColumn)dataGridViewColumn_EnglishDate).Name].Value = list2[m];
				}
				((DataGridView)this).Columns[((DataGridViewColumn)dataGridViewColumn_EnglishDate).Name].SortMode = sortMode2;
			}
		}
	}
}
