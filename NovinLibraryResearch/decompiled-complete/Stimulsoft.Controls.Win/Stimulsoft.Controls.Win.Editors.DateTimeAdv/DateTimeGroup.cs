using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.Editors.DateTimeAdv;

public class DateTimeGroup : VisualInputGroup
{
	private IDateTimePartInput _DayInput;

	private IDateTimePartInput _MonthInput;

	private IDateTimePartInput _YearInput;

	private IDateTimePartInput _HourInput;

	private IDateTimePartInput _MinuteInput;

	private IDateTimePartInput _SecondInput;

	private IDateTimePartInput _DayOfYearInput;

	public static readonly DateTime MinDateTime = new DateTime(1753, 1, 1);

	public static readonly DateTime MaxDateTime = new DateTime(9998, 12, 31);

	private bool _ProcessingInputChanged;

	private DateTime _Value;

	private bool _InValueChanged;

	private bool _ResettingDateValue;

	private DateTime _MinDate = MinDateTime;

	private DateTime _MaxDate = MaxDateTime;

	private bool _DefaultInputValues = true;

	private bool HasNestedGroups
	{
		get
		{
			foreach (VisualItem item in Items)
			{
				if (item is VisualGroup)
				{
					return true;
				}
			}
			return false;
		}
	}

	public DateTime Value
	{
		get
		{
			if (base.IsFocused)
			{
				UpdateValue(updateDirect: true);
			}
			return _Value;
		}
		set
		{
			bool flag = _Value != value;
			_Value = value;
			base.IsEmpty = false;
			UpdateInputItems();
			if (flag)
			{
				OnValueChanged();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public DateTime[] Values
	{
		get
		{
			if (HasNestedGroups)
			{
				List<DateTime> list = new List<DateTime>();
				foreach (VisualItem item in Items)
				{
					if (item is DateTimeGroup dateTimeGroup)
					{
						list.Add(dateTimeGroup.Value);
					}
				}
				return list.ToArray();
			}
			return new DateTime[1] { Value };
		}
		set
		{
			if (value == null || value.Length == 0)
			{
				return;
			}
			if (HasNestedGroups)
			{
				int num = 0;
				{
					foreach (VisualItem item in Items)
					{
						if (item is DateTimeGroup dateTimeGroup)
						{
							dateTimeGroup.Value = value[num];
							num++;
							if (num >= value.Length)
							{
								break;
							}
						}
					}
					return;
				}
			}
			Value = value[0];
		}
	}

	public DateTime MinDate
	{
		get
		{
			return _MinDate;
		}
		set
		{
			_MinDate = value;
			OnMinDateChanged();
		}
	}

	public DateTime MaxDate
	{
		get
		{
			return _MaxDate;
		}
		set
		{
			_MaxDate = value;
			OnMaxDateChanged();
		}
	}

	public bool DefaultInputValues
	{
		get
		{
			return _DefaultInputValues;
		}
		set
		{
			_DefaultInputValues = value;
		}
	}

	public event EventHandler ValueChanged;

	protected override void OnItemsCollectionChanged(CollectionChangedInfo collectionChangedInfo)
	{
		if (collectionChangedInfo.ChangeType == eCollectionChangeType.Cleared)
		{
			_DayInput = null;
			_MonthInput = null;
			_YearInput = null;
			_HourInput = null;
			_MinuteInput = null;
			_SecondInput = null;
			_DayOfYearInput = null;
		}
		if (collectionChangedInfo.Removed != null)
		{
			VisualItem[] removed = collectionChangedInfo.Removed;
			foreach (VisualItem visualItem in removed)
			{
				if (visualItem == _DayInput)
				{
					_DayInput = null;
				}
				else if (visualItem == _MonthInput)
				{
					_MonthInput = null;
				}
				else if (visualItem == _YearInput)
				{
					_YearInput = null;
				}
				else if (visualItem == _HourInput)
				{
					_HourInput = null;
				}
				else if (visualItem == _MinuteInput)
				{
					_MinuteInput = null;
				}
				else if (visualItem == _SecondInput)
				{
					_SecondInput = null;
				}
				else if (visualItem == _DayOfYearInput)
				{
					_DayOfYearInput = null;
				}
			}
		}
		if (collectionChangedInfo.Added != null)
		{
			List<VisualItem> list = new List<VisualItem>();
			VisualItem[] added = collectionChangedInfo.Added;
			foreach (VisualItem visualItem2 in added)
			{
				if (visualItem2 is HourPeriodLabel)
				{
					if (_HourInput != null && _HourInput is NumericHourInput)
					{
						((HourPeriodLabel)visualItem2).HourInput = (NumericHourInput)_HourInput;
					}
				}
				else if (visualItem2 is HourPeriodInput && _HourInput != null && _HourInput is NumericHourInput)
				{
					((HourPeriodInput)visualItem2).HourInput = (NumericHourInput)_HourInput;
				}
				if (!(visualItem2 is IDateTimePartInput dateTimePartInput))
				{
					continue;
				}
				if (dateTimePartInput.Part == eDateTimePart.Day)
				{
					_DayInput = dateTimePartInput;
				}
				else if (dateTimePartInput.Part == eDateTimePart.Hour)
				{
					_HourInput = dateTimePartInput;
					if (!(_HourInput is NumericHourInput))
					{
						continue;
					}
					foreach (VisualItem item in Items)
					{
						if (item is HourPeriodLabel)
						{
							((HourPeriodLabel)item).HourInput = (NumericHourInput)_HourInput;
						}
						else if (item is HourPeriodInput)
						{
							((HourPeriodInput)item).HourInput = (NumericHourInput)_HourInput;
						}
					}
				}
				else if (dateTimePartInput.Part == eDateTimePart.Minute)
				{
					_MinuteInput = dateTimePartInput;
				}
				else if (dateTimePartInput.Part == eDateTimePart.Month)
				{
					_MonthInput = dateTimePartInput;
				}
				else if (dateTimePartInput.Part == eDateTimePart.Second)
				{
					_SecondInput = dateTimePartInput;
				}
				else if (dateTimePartInput.Part == eDateTimePart.Year)
				{
					_YearInput = dateTimePartInput;
				}
				else if (dateTimePartInput.Part == eDateTimePart.DayOfYear)
				{
					_DayOfYearInput = dateTimePartInput;
				}
			}
		}
		UpdateInnerDateTimeGroupMinMax();
		UpdateInputItems();
		base.OnItemsCollectionChanged(collectionChangedInfo);
	}

	protected override void OnInputChanged(VisualInputBase input)
	{
		if (!_ProcessingInputChanged && !_ResettingDateValue)
		{
			try
			{
				_ProcessingInputChanged = true;
				if ((input == _MonthInput || input == _YearInput) && _DayInput != null)
				{
					UpdateDayMaxValue();
				}
				if (input == _YearInput && _DayOfYearInput != null)
				{
					UpdateDayOfYearMaxValue();
				}
				if (input != _YearInput && _YearInput != null && _YearInput.IsEmpty && _DefaultInputValues)
				{
					_YearInput.Value = Math.Min(DateTime.Now.Year, _MaxDate.Year);
				}
				if (input != _MonthInput && _MonthInput != null && _MonthInput.IsEmpty)
				{
					if (_DefaultInputValues)
					{
						if (_YearInput != null && _YearInput.Value == _MaxDate.Year)
						{
							_MonthInput.Value = _MaxDate.Month;
						}
						else
						{
							_MonthInput.Value = DateTime.Now.Month;
						}
					}
					UpdateDayMaxValue();
				}
				if (input != _DayInput && _DayInput != null && _DayInput.IsEmpty && _DefaultInputValues)
				{
					if (_YearInput != null && _YearInput.Value == _MaxDate.Year && _MonthInput != null && _MonthInput.Value == _MaxDate.Month)
					{
						_DayInput.Value = _MaxDate.Day;
					}
					else
					{
						_DayInput.Value = DateTime.Now.Day;
					}
				}
				if (input != _HourInput && _HourInput != null && _HourInput.IsEmpty && _DefaultInputValues)
				{
					_HourInput.Value = 0;
				}
				if (input != _MinuteInput && _MinuteInput != null && _MinuteInput.IsEmpty && _DefaultInputValues)
				{
					_MinuteInput.Value = 0;
				}
				if (input != _SecondInput && _SecondInput != null && _SecondInput.IsEmpty && _DefaultInputValues)
				{
					_SecondInput.Value = 0;
				}
				if ((_YearInput != null && !_YearInput.IsEmpty) || (_MonthInput != null && !_MonthInput.IsEmpty) || (_DayInput != null && !_DayInput.IsEmpty) || (_HourInput != null && !_HourInput.IsEmpty) || (_MinuteInput != null && !_MinuteInput.IsEmpty) || (_SecondInput != null && !_SecondInput.IsEmpty))
				{
					base.IsEmpty = false;
				}
				SyncValues(input);
			}
			finally
			{
				_ProcessingInputChanged = false;
			}
			if (base.IsUserInput && IsCurrentInputValid())
			{
				OnValueChanged();
			}
		}
		base.OnInputChanged(input);
	}

	private void UpdateDayOfYearMaxValue()
	{
		if (_YearInput != null && !_YearInput.IsEmpty && _DayOfYearInput != null)
		{
			_DayOfYearInput.MaxValue = DateTimeInput.GetActiveCulture().Calendar.GetDaysInYear(_YearInput.Value);
		}
	}

	protected override void OnFocusedItemChanged(VisualItem previousFocus)
	{
		if (base.FocusedItem == _DayInput && _DayInput is NumericDayInput)
		{
			UpdateDayMaxValue();
		}
		else if (previousFocus == _YearInput || previousFocus == _DayInput)
		{
			UpdateDayMaxValue();
		}
		else
		{
			SyncValues(previousFocus);
		}
		base.OnFocusedItemChanged(previousFocus);
	}

	private void UpdateDayMaxValue()
	{
		if (_DayInput != null && (base.FocusedItem == _DayInput || base.FocusedItem == _MonthInput) && _DayInput is NumericDayInput)
		{
			_DayInput.MaxValue = 31;
		}
		else if (_YearInput != null && _MonthInput != null && !_MonthInput.IsEmpty && _DayInput != null)
		{
			_DayInput.MaxValue = DateTime.DaysInMonth(_YearInput.IsEmpty ? DateTime.Now.Year : _YearInput.Value, _MonthInput.Value);
		}
	}

	protected override void OnInputComplete()
	{
		SyncValues(base.FocusedItem);
		base.OnInputComplete();
	}

	private void SyncValues(VisualItem visualItem)
	{
		if (!(visualItem is IDateTimePartInput dateTimePartInput))
		{
			return;
		}
		for (int i = 0; i < Items.Count; i++)
		{
			if (!(Items[i] is IDateTimePartInput dateTimePartInput2))
			{
				continue;
			}
			if (dateTimePartInput2.Part == dateTimePartInput.Part && dateTimePartInput2 != dateTimePartInput)
			{
				dateTimePartInput2.Value = dateTimePartInput.Value;
			}
			else if (dateTimePartInput2 != dateTimePartInput && dateTimePartInput.Part == eDateTimePart.DayOfYear && (dateTimePartInput2.Part == eDateTimePart.Day || dateTimePartInput2.Part == eDateTimePart.Month))
			{
				if (_YearInput != null && !_YearInput.IsEmpty)
				{
					DateTime dateTime = CreateDateTime(_YearInput.Value, dateTimePartInput.Value);
					if (dateTimePartInput2.Part == eDateTimePart.Day)
					{
						dateTimePartInput2.Value = dateTime.Day;
					}
					else if (dateTimePartInput2.Part == eDateTimePart.Month)
					{
						dateTimePartInput2.Value = dateTime.Month;
					}
				}
			}
			else
			{
				if (dateTimePartInput2.Part != eDateTimePart.DayName)
				{
					continue;
				}
				if (_YearInput != null && !_YearInput.IsEmpty && ((_MonthInput != null && !_MonthInput.IsEmpty && _DayInput != null && !_DayInput.IsEmpty) || (_DayOfYearInput != null && !_DayOfYearInput.IsEmpty)))
				{
					try
					{
						dateTimePartInput2.Value = (int)((_DayOfYearInput == null) ? new DateTime(_YearInput.Value, _MonthInput.Value, _DayInput.Value) : CreateDateTime(_YearInput.Value, _DayOfYearInput.Value)).DayOfWeek;
					}
					catch
					{
						dateTimePartInput2.IsEmpty = true;
					}
				}
				else
				{
					dateTimePartInput2.IsEmpty = true;
				}
			}
		}
	}

	internal static DateTime CreateDateTime(int year, int dayOfYear)
	{
		DateTime result = new DateTime(year, 1, 1);
		if (dayOfYear > 0)
		{
			result.AddDays(dayOfYear - 1);
		}
		return result;
	}

	protected override void OnLostFocus()
	{
		UpdateValue(updateDirect: false);
		base.OnLostFocus();
	}

	protected override void OnGroupInputComplete()
	{
		UpdateValue(updateDirect: false);
		base.OnGroupInputComplete();
	}

	protected internal void UpdateValue(bool updateDirect)
	{
		DateTime now = DateTime.Now;
		if ((_YearInput != null && _YearInput.IsEmpty) || (_MonthInput != null && _MonthInput.IsEmpty) || (_DayInput != null && _DayInput.IsEmpty) || (_HourInput != null && _HourInput.IsEmpty) || (_MinuteInput != null && _MinuteInput.IsEmpty) || (_SecondInput != null && _SecondInput.IsEmpty))
		{
			if (HasNestedGroups)
			{
				UpdateIsEmpty();
			}
			else if (!base.IsFocused || _DefaultInputValues)
			{
				base.IsEmpty = true;
			}
			return;
		}
		now = GetCurrentInputValue();
		if (now > _MaxDate)
		{
			now = _MaxDate;
		}
		else if (now < _MinDate)
		{
			now = _MinDate;
		}
		if (updateDirect)
		{
			_Value = now;
		}
		else
		{
			Value = now;
		}
	}

	private bool IsCurrentInputValid()
	{
		DateTime currentInputValue = GetCurrentInputValue();
		if (currentInputValue < _MinDate || currentInputValue > _MaxDate)
		{
			return false;
		}
		return true;
	}

	private DateTime GetCurrentInputValue()
	{
		DateTime dateTime = DateTime.Now;
		if (_Value != DateTime.MinValue)
		{
			dateTime = _Value;
		}
		if (dateTime < _MinDate)
		{
			dateTime = _MinDate;
		}
		else if (dateTime > _MaxDate)
		{
			dateTime = _MaxDate;
		}
		int num = dateTime.Year;
		int month = dateTime.Month;
		int num2 = dateTime.Day;
		int num3 = dateTime.Hour;
		int minute = dateTime.Minute;
		int second = dateTime.Second;
		int dayOfYear = dateTime.DayOfYear;
		if (_YearInput != null)
		{
			num = _YearInput.Value;
			if (num < 30)
			{
				num += 2000;
			}
			else if (num < 100)
			{
				num += 1900;
			}
		}
		else if (_Value != DateTime.MinValue)
		{
			num = _Value.Year;
		}
		if (_MonthInput != null)
		{
			month = _MonthInput.Value;
		}
		else if (_Value != DateTime.MinValue)
		{
			month = _Value.Month;
		}
		if (_DayInput != null)
		{
			num2 = _DayInput.Value;
			if (_YearInput != null && _MonthInput != null)
			{
				num2 = Math.Min(num2, DateTime.DaysInMonth(_YearInput.IsEmpty ? DateTime.Now.Year : _YearInput.Value, _MonthInput.Value));
			}
		}
		else if (_Value != DateTime.MinValue)
		{
			num2 = _Value.Day;
		}
		if (_HourInput != null)
		{
			num3 = _HourInput.Value;
			if (num3 == 12 && _HourInput is NumericHourInput && ((NumericHourInput)_HourInput).Period == eHourPeriod.AM && !((NumericHourInput)_HourInput).Is24HourFormat)
			{
				num3 = 0;
			}
			else if (num3 < 12 && _HourInput is NumericHourInput && ((NumericHourInput)_HourInput).Period == eHourPeriod.PM && !((NumericHourInput)_HourInput).Is24HourFormat)
			{
				num3 += 12;
			}
		}
		else if (_Value != DateTime.MinValue)
		{
			num3 = _Value.Hour;
		}
		if (_MinuteInput != null)
		{
			minute = _MinuteInput.Value;
		}
		else if (_Value != DateTime.MinValue)
		{
			minute = _Value.Minute;
		}
		second = ((_SecondInput != null) ? _SecondInput.Value : ((_Value != DateTime.MinValue) ? _Value.Second : 0));
		if (_DayOfYearInput != null)
		{
			dayOfYear = _DayOfYearInput.Value;
			if (dayOfYear > 0)
			{
				DateTime dateTime2 = CreateDateTime(num, dayOfYear);
				month = dateTime2.Month;
				num2 = dateTime2.Day;
			}
		}
		if (DateTime.DaysInMonth(num, month) < num2)
		{
			num2 = DateTime.DaysInMonth(num, month);
		}
		return (_HourInput == null && _MinuteInput == null && _SecondInput == null && !(_Value != DateTime.MinValue)) ? new DateTime(num, month, num2) : new DateTime(num, month, num2, num3, minute, second);
	}

	protected virtual void OnValueChanged()
	{
		if (_InValueChanged)
		{
			return;
		}
		try
		{
			_InValueChanged = true;
			if (this.ValueChanged != null)
			{
				this.ValueChanged(this, new EventArgs());
			}
			if (base.Parent is DateTimeGroup)
			{
				((DateTimeGroup)base.Parent).OnValueChanged();
			}
			InvalidateArrange();
		}
		finally
		{
			_InValueChanged = false;
		}
	}

	private void UpdateInputItems()
	{
		DateTime value = _Value;
		if (_YearInput != null)
		{
			if (base.IsEmpty)
			{
				_YearInput.IsEmpty = true;
			}
			else
			{
				_YearInput.Value = value.Year;
			}
		}
		if (_MonthInput != null)
		{
			if (base.IsEmpty)
			{
				_MonthInput.IsEmpty = true;
			}
			else
			{
				_MonthInput.Value = value.Month;
			}
		}
		if (_DayInput != null)
		{
			if (base.IsEmpty)
			{
				_DayInput.IsEmpty = true;
			}
			else if (base.FocusedItem != _DayInput || !(_DayInput is NumericDayInput) || !_ProcessingInputChanged)
			{
				_DayInput.Value = value.Day;
			}
		}
		if (_HourInput != null)
		{
			if (base.IsEmpty)
			{
				_HourInput.IsEmpty = true;
			}
			else
			{
				if (_HourInput is NumericHourInput)
				{
					((NumericHourInput)_HourInput).Period = eHourPeriod.AM;
				}
				_HourInput.Value = value.Hour;
				if (_HourInput is NumericHourInput && !((NumericHourInput)_HourInput).Is24HourFormat && value.Hour >= 12)
				{
					((NumericHourInput)_HourInput).Period = eHourPeriod.PM;
				}
			}
		}
		if (_MinuteInput != null)
		{
			if (base.IsEmpty)
			{
				_MinuteInput.IsEmpty = true;
			}
			else
			{
				_MinuteInput.Value = value.Minute;
			}
		}
		if (_SecondInput != null)
		{
			if (base.IsEmpty)
			{
				_SecondInput.IsEmpty = true;
			}
			else
			{
				_SecondInput.Value = value.Second;
			}
		}
		for (int i = 0; i < Items.Count; i++)
		{
			if (!(Items[i] is IDateTimePartInput dateTimePartInput))
			{
				continue;
			}
			if (dateTimePartInput.Part == eDateTimePart.DayName)
			{
				dateTimePartInput.Value = (int)(base.IsEmpty ? ((DayOfWeek)(-1)) : _Value.DayOfWeek);
			}
			else if (dateTimePartInput.Part == eDateTimePart.DayOfYear)
			{
				if (base.IsEmpty)
				{
					dateTimePartInput.IsEmpty = true;
				}
				else
				{
					dateTimePartInput.Value = _Value.DayOfYear;
				}
			}
		}
		InvalidateArrange();
	}

	protected override void ResetValue()
	{
		_ResettingDateValue = true;
		try
		{
			if (base.AllowEmptyState)
			{
				if (_DayInput != null)
				{
					_DayInput.IsEmpty = true;
				}
				if (_MonthInput != null)
				{
					_MonthInput.IsEmpty = true;
				}
				if (_YearInput != null)
				{
					_YearInput.IsEmpty = true;
				}
				if (_HourInput != null)
				{
					_HourInput.IsEmpty = true;
				}
				if (_MinuteInput != null)
				{
					_MinuteInput.IsEmpty = true;
				}
				if (_SecondInput != null)
				{
					_SecondInput.IsEmpty = true;
				}
				foreach (VisualItem item in Items)
				{
					if (item is DateTimeGroup)
					{
						((DateTimeGroup)item).IsEmpty = true;
					}
					else if (item is IDateTimePartInput && !((IDateTimePartInput)item).IsEmpty)
					{
						((IDateTimePartInput)item).IsEmpty = true;
					}
				}
				_Value = DateTime.MinValue;
			}
			else
			{
				SetNonEmptyValue();
			}
		}
		finally
		{
			_ResettingDateValue = false;
		}
		OnValueChanged();
	}

	private void OnMinDateChanged()
	{
		UpdateInnerDateTimeGroupMinMax();
	}

	private void OnMaxDateChanged()
	{
		UpdateInnerDateTimeGroupMinMax();
	}

	private void UpdateInnerDateTimeGroupMinMax()
	{
		foreach (VisualItem item in Items)
		{
			if (item is DateTimeGroup dateTimeGroup)
			{
				dateTimeGroup.MinDate = _MinDate;
				dateTimeGroup.MaxDate = _MaxDate;
			}
		}
	}

	protected override void OnAllowEmptyStateChanged()
	{
		if (!base.AllowEmptyState && base.IsEmpty)
		{
			SetNonEmptyValue();
		}
		base.OnAllowEmptyStateChanged();
	}

	private void SetNonEmptyValue()
	{
		if (MinDate > MinDateTime)
		{
			Value = MinDate;
		}
		else
		{
			Value = DateTime.Now;
		}
	}

	internal override void ProcessKeyDown(KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode != 8 && (int)e.KeyCode != 46)
		{
			base.IsUserInput = true;
		}
		base.ProcessKeyDown(e);
		base.IsUserInput = false;
	}

	internal override void ProcessKeyPress(KeyPressEventArgs e)
	{
		base.IsUserInput = true;
		base.ProcessKeyPress(e);
		base.IsUserInput = false;
	}

	internal override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		base.IsUserInput = true;
		bool result = base.ProcessCmdKey(ref msg, keyData);
		base.IsUserInput = false;
		return result;
	}

	internal override void ProcessMouseWheel(MouseEventArgs e)
	{
		base.IsUserInput = true;
		base.ProcessMouseWheel(e);
		base.IsUserInput = false;
	}
}
