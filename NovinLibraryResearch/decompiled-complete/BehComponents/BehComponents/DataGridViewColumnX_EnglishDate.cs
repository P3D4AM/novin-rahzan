using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BehComponents;

[ToolboxItem(false)]
[Description("Calendar Column In English")]
public class DataGridViewColumnX_EnglishDate : DataGridViewColumn
{
	internal class CalendarCell : DataGridViewTextBoxCell
	{
		public override Type EditType => typeof(CalendarEditingControl);

		public override Type ValueType => typeof(string);

		public override object DefaultNewRowValue => DBNull.Value;

		public CalendarCell()
		{
			((DataGridViewCell)this).Style.Format = "";
		}

		public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
		{
			((DataGridViewTextBoxCell)this).InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
			CalendarEditingControl calendarEditingControl = ((DataGridViewElement)this).DataGridView.EditingControl as CalendarEditingControl;
			if (((DataGridViewCell)this).Value == null || ((DataGridViewCell)this).Value.ToString() == "")
			{
				((Control)calendarEditingControl).Text = DateTime.Today.ToString(_FormatDate);
			}
			else if (((DataGridViewCell)this).Value != DBNull.Value)
			{
				((Control)calendarEditingControl).Text = ((DataGridViewCell)this).Value.ToString();
			}
		}
	}

	internal class CalendarEditingControl : DateTimePickerX, IDataGridViewEditingControl
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
					base.SelectedDate = DateTime.Parse(value.ToString()).ToString(_FormatDate);
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

		public CalendarEditingControl()
		{
			base.Format = FormatDate.Custom;
			base.CustomFormat = _FormatDate;
			base.CalendarType = CalendarTypes.English;
			base.DropDownClosedWhenSelectedDateChanged = _DropDownClosedWhenSelectedDateChanged;
			base.DropDownClosedWhenClickOnDays = _DropDownClosedWhenClickOnDays;
			base.RightToLeftLayout = false;
			base.DockSide = Alignments.Right;
			base.OnCloseDropDown += dateTimePicker_OnCloseDropDown;
			((UserControl)this).Load += DateTimePicker_Load;
		}

		private void DateTimePicker_Load(object sender, EventArgs e)
		{
			base.SelectedDate = valueCell;
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

	private static string _FormatDate = "MM/dd/yyyy";

	private static bool _DropDownClosedWhenSelectedDateChanged = false;

	private static bool _DropDownClosedWhenClickOnDays = true;

	public static string FormatDate
	{
		get
		{
			return _FormatDate;
		}
		set
		{
			_FormatDate = value;
		}
	}

	public static bool DropDownClosedWhenSelectedDateChanged
	{
		get
		{
			return _DropDownClosedWhenSelectedDateChanged;
		}
		set
		{
			_DropDownClosedWhenSelectedDateChanged = value;
		}
	}

	public static bool DropDownClosedWhenClickOnDays
	{
		get
		{
			return _DropDownClosedWhenClickOnDays;
		}
		set
		{
			_DropDownClosedWhenClickOnDays = value;
		}
	}

	public override DataGridViewCell CellTemplate
	{
		get
		{
			return ((DataGridViewColumn)this).CellTemplate;
		}
		set
		{
			if (value != null && !((object)value).GetType().IsAssignableFrom(typeof(CalendarCell)))
			{
				throw new InvalidCastException("Must be a CalendarCell");
			}
			((DataGridViewColumn)this).CellTemplate = value;
		}
	}

	public DataGridViewColumnX_EnglishDate()
		: base((DataGridViewCell)(object)new CalendarCell())
	{
		((DataGridViewColumn)this).SortMode = (DataGridViewColumnSortMode)1;
	}
}
