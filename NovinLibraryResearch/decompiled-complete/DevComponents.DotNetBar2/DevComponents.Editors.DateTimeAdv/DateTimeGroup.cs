using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace DevComponents.Editors.DateTimeAdv;

public class DateTimeGroup : VisualInputGroup
{
	private IDateTimePartInput __2533;

	private IDateTimePartInput __2534;

	private IDateTimePartInput __2535;

	private IDateTimePartInput __2536;

	private IDateTimePartInput __2537;

	private IDateTimePartInput __2538;

	private IDateTimePartInput __2539;

	public static readonly DateTime MinDateTime = new DateTime(1753, 1, 1);

	public static readonly DateTime MaxDateTime = new DateTime(9998, 12, 31);

	private EventHandler ᗲ;

	private bool __253A;

	private DateTime _ᗸ;

	private bool __253B;

	private bool __253C;

	private DateTime __253D = MinDateTime;

	private DateTime __253E = MaxDateTime;

	private bool __253F = true;

	private bool _2532
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
			return _ᗸ;
		}
		set
		{
			bool flag = _ᗸ != value;
			_ᗸ = value;
			base.IsEmpty = false;
			_2548();
			if (flag)
			{
				OnValueChanged();
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public DateTime[] Values
	{
		get
		{
			if (_2532)
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
			if (_2532)
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
			return __253D;
		}
		set
		{
			__253D = value;
			_2549();
		}
	}

	public DateTime MaxDate
	{
		get
		{
			return __253E;
		}
		set
		{
			__253E = value;
			_254A();
		}
	}

	public bool DefaultInputValues
	{
		get
		{
			return __253F;
		}
		set
		{
			__253F = value;
		}
	}

	public event EventHandler ValueChanged
	{
		add
		{
			EventHandler eventHandler = ᗲ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᗲ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ᗲ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᗲ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	protected override void OnItemsCollectionChanged(CollectionChangedInfo collectionChangedInfo)
	{
		if (collectionChangedInfo.ChangeType == eCollectionChangeType.Cleared)
		{
			__2533 = null;
			__2534 = null;
			__2535 = null;
			__2536 = null;
			__2537 = null;
			__2538 = null;
			__2539 = null;
		}
		if (collectionChangedInfo.Removed != null)
		{
			VisualItem[] removed = collectionChangedInfo.Removed;
			foreach (VisualItem visualItem in removed)
			{
				if (visualItem == __2533)
				{
					__2533 = null;
				}
				else if (visualItem == __2534)
				{
					__2534 = null;
				}
				else if (visualItem == __2535)
				{
					__2535 = null;
				}
				else if (visualItem == __2536)
				{
					__2536 = null;
				}
				else if (visualItem == __2537)
				{
					__2537 = null;
				}
				else if (visualItem == __2538)
				{
					__2538 = null;
				}
				else if (visualItem == __2539)
				{
					__2539 = null;
				}
			}
		}
		if (collectionChangedInfo.Added != null)
		{
			new List<VisualItem>();
			VisualItem[] added = collectionChangedInfo.Added;
			foreach (VisualItem visualItem2 in added)
			{
				if (visualItem2 is HourPeriodLabel)
				{
					if (__2536 != null && __2536 is NumericHourInput)
					{
						((HourPeriodLabel)visualItem2).HourInput = (NumericHourInput)__2536;
					}
				}
				else if (visualItem2 is HourPeriodInput && __2536 != null && __2536 is NumericHourInput)
				{
					((HourPeriodInput)visualItem2).HourInput = (NumericHourInput)__2536;
				}
				if (!(visualItem2 is IDateTimePartInput dateTimePartInput))
				{
					continue;
				}
				if (dateTimePartInput.Part == eDateTimePart.Day)
				{
					__2533 = dateTimePartInput;
				}
				else if (dateTimePartInput.Part == eDateTimePart.Hour)
				{
					__2536 = dateTimePartInput;
					if (!(__2536 is NumericHourInput))
					{
						continue;
					}
					foreach (VisualItem item in Items)
					{
						if (item is HourPeriodLabel)
						{
							((HourPeriodLabel)item).HourInput = (NumericHourInput)__2536;
						}
						else if (item is HourPeriodInput)
						{
							((HourPeriodInput)item).HourInput = (NumericHourInput)__2536;
						}
					}
				}
				else if (dateTimePartInput.Part == eDateTimePart.Minute)
				{
					__2537 = dateTimePartInput;
				}
				else if (dateTimePartInput.Part == eDateTimePart.Month)
				{
					__2534 = dateTimePartInput;
				}
				else if (dateTimePartInput.Part == eDateTimePart.Second)
				{
					__2538 = dateTimePartInput;
				}
				else if (dateTimePartInput.Part == eDateTimePart.Year)
				{
					__2535 = dateTimePartInput;
				}
				else if (dateTimePartInput.Part == eDateTimePart.DayOfYear)
				{
					__2539 = dateTimePartInput;
				}
			}
		}
		_254B();
		_2548();
		base.OnItemsCollectionChanged(collectionChangedInfo);
	}

	protected override void OnInputChanged(VisualInputBase input)
	{
		if (!__253A && !__253C)
		{
			try
			{
				__253A = true;
				if ((input == __2534 || input == __2535) && __2533 != null)
				{
					_2541();
				}
				if (input == __2535 && __2539 != null)
				{
					_2540();
				}
				if (input != __2535 && __2535 != null && __2535.IsEmpty && __253F)
				{
					__2535.Value = Math.Min(DateTime.Now.Year, __253E.Year);
				}
				if (input != __2534 && __2534 != null && __2534.IsEmpty)
				{
					if (__253F)
					{
						if (__2535 != null && __2535.Value == __253E.Year)
						{
							__2534.Value = __253E.Month;
						}
						else
						{
							__2534.Value = DateTime.Now.Month;
						}
					}
					_2541();
				}
				if (input != __2533 && __2533 != null && __2533.IsEmpty && __253F)
				{
					if (__2535 != null && __2535.Value == __253E.Year && __2534 != null && __2534.Value == __253E.Month)
					{
						__2533.Value = __253E.Day;
					}
					else
					{
						__2533.Value = DateTime.Now.Day;
					}
				}
				if (input != __2536 && __2536 != null && __2536.IsEmpty && __253F)
				{
					__2536.Value = 0;
				}
				if (input != __2537 && __2537 != null && __2537.IsEmpty && __253F)
				{
					__2537.Value = 0;
				}
				if (input != __2538 && __2538 != null && __2538.IsEmpty && __253F)
				{
					__2538.Value = 0;
				}
				if ((__2535 != null && !__2535.IsEmpty) || (__2534 != null && !__2534.IsEmpty) || (__2533 != null && !__2533.IsEmpty) || (__2536 != null && !__2536.IsEmpty) || (__2537 != null && !__2537.IsEmpty) || (__2538 != null && !__2538.IsEmpty))
				{
					base.IsEmpty = false;
				}
				_2542(input);
			}
			finally
			{
				__253A = false;
			}
			if (base.IsUserInput && _2546())
			{
				OnValueChanged();
			}
		}
		base.OnInputChanged(input);
	}

	private void _2540()
	{
		if (__2535 != null && !__2535.IsEmpty && __2539 != null)
		{
			__2539.MaxValue = DateTimeInput.GetActiveCulture().Calendar.GetDaysInYear(__2535.Value);
		}
	}

	protected override void OnFocusedItemChanged(VisualItem previousFocus)
	{
		if (base.FocusedItem == __2533 && __2533 is NumericDayInput)
		{
			_2541();
		}
		else if (previousFocus == __2535 || previousFocus == __2533)
		{
			_2541();
		}
		else
		{
			_2542(previousFocus);
		}
		base.OnFocusedItemChanged(previousFocus);
	}

	private void _2541()
	{
		if (__2533 != null && (base.FocusedItem == __2533 || base.FocusedItem == __2534) && __2533 is NumericDayInput)
		{
			__2533.MaxValue = 31;
		}
		else if (__2535 != null && __2534 != null && !__2534.IsEmpty && __2533 != null)
		{
			__2533.MaxValue = DateTime.DaysInMonth(__2535.IsEmpty ? DateTime.Now.Year : __2535.Value, __2534.Value);
		}
	}

	protected override void OnInputComplete()
	{
		_2542(base.FocusedItem);
		base.OnInputComplete();
	}

	private void _2542(VisualItem ᡲ)
	{
		if (!(ᡲ is IDateTimePartInput dateTimePartInput))
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
				if (__2535 != null && !__2535.IsEmpty)
				{
					DateTime dateTime = _2543(__2535.Value, dateTimePartInput.Value);
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
				if (__2535 != null && !__2535.IsEmpty && ((__2534 != null && !__2534.IsEmpty && __2533 != null && !__2533.IsEmpty) || (__2539 != null && !__2539.IsEmpty)))
				{
					try
					{
						dateTimePartInput2.Value = (int)((__2539 == null) ? new DateTime(__2535.Value, __2534.Value, __2533.Value) : _2543(__2535.Value, __2539.Value)).DayOfWeek;
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

	internal static DateTime _2543(int _2544, int _2545)
	{
		DateTime result = new DateTime(_2544, 1, 1);
		if (_2545 > 0)
		{
			result.AddDays(_2545 - 1);
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
		if ((__2535 != null && __2535.IsEmpty) || (__2534 != null && __2534.IsEmpty) || (__2533 != null && __2533.IsEmpty) || (__2536 != null && __2536.IsEmpty) || (__2537 != null && __2537.IsEmpty) || (__2538 != null && __2538.IsEmpty))
		{
			if (_2532)
			{
				UpdateIsEmpty();
			}
			else if (!base.IsFocused || __253F)
			{
				base.IsEmpty = true;
			}
			return;
		}
		now = _2547();
		if (now > __253E)
		{
			now = __253E;
		}
		else if (now < __253D)
		{
			now = __253D;
		}
		if (updateDirect)
		{
			_ᗸ = now;
		}
		else
		{
			Value = now;
		}
	}

	private bool _2546()
	{
		DateTime dateTime = _2547();
		if (dateTime < __253D || dateTime > __253E)
		{
			return false;
		}
		return true;
	}

	private DateTime _2547()
	{
		DateTime dateTime = DateTime.Now;
		if (_ᗸ != DateTime.MinValue)
		{
			dateTime = _ᗸ;
		}
		if (dateTime < __253D)
		{
			dateTime = __253D;
		}
		else if (dateTime > __253E)
		{
			dateTime = __253E;
		}
		int num = dateTime.Year;
		int month = dateTime.Month;
		int num2 = dateTime.Day;
		int num3 = dateTime.Hour;
		int minute = dateTime.Minute;
		int second = dateTime.Second;
		int dayOfYear = dateTime.DayOfYear;
		if (__2535 != null)
		{
			num = __2535.Value;
			if (num < 30)
			{
				num += 2000;
			}
			else if (num < 100)
			{
				num += 1900;
			}
		}
		else if (_ᗸ != DateTime.MinValue)
		{
			num = _ᗸ.Year;
		}
		if (__2534 != null)
		{
			month = __2534.Value;
		}
		else if (_ᗸ != DateTime.MinValue)
		{
			month = _ᗸ.Month;
		}
		if (__2533 != null)
		{
			num2 = __2533.Value;
			if (__2535 != null && __2534 != null)
			{
				num2 = Math.Min(num2, DateTime.DaysInMonth(__2535.IsEmpty ? DateTime.Now.Year : __2535.Value, __2534.Value));
			}
		}
		else if (_ᗸ != DateTime.MinValue)
		{
			num2 = _ᗸ.Day;
		}
		if (__2536 != null)
		{
			num3 = __2536.Value;
			if (num3 == 12 && __2536 is NumericHourInput && ((NumericHourInput)__2536).Period == eHourPeriod.AM && !((NumericHourInput)__2536).Is24HourFormat)
			{
				num3 = 0;
			}
			else if (num3 < 12 && __2536 is NumericHourInput && ((NumericHourInput)__2536).Period == eHourPeriod.PM && !((NumericHourInput)__2536).Is24HourFormat)
			{
				num3 += 12;
			}
		}
		else if (_ᗸ != DateTime.MinValue)
		{
			num3 = _ᗸ.Hour;
		}
		if (__2537 != null)
		{
			minute = __2537.Value;
		}
		else if (_ᗸ != DateTime.MinValue)
		{
			minute = _ᗸ.Minute;
		}
		second = ((__2538 != null) ? __2538.Value : ((_ᗸ != DateTime.MinValue) ? _ᗸ.Second : 0));
		if (__2539 != null)
		{
			dayOfYear = __2539.Value;
			if (dayOfYear > 0)
			{
				DateTime dateTime2 = _2543(num, dayOfYear);
				month = dateTime2.Month;
				num2 = dateTime2.Day;
			}
		}
		if (DateTime.DaysInMonth(num, month) < num2)
		{
			num2 = DateTime.DaysInMonth(num, month);
		}
		return (__2536 == null && __2537 == null && __2538 == null && !(_ᗸ != DateTime.MinValue)) ? new DateTime(num, month, num2) : new DateTime(num, month, num2, num3, minute, second);
	}

	protected virtual void OnValueChanged()
	{
		if (__253B)
		{
			return;
		}
		try
		{
			__253B = true;
			if (ᗲ != null)
			{
				ᗲ(this, new EventArgs());
			}
			if (base.Parent is DateTimeGroup)
			{
				((DateTimeGroup)base.Parent).OnValueChanged();
			}
			InvalidateArrange();
		}
		finally
		{
			__253B = false;
		}
	}

	private void _2548()
	{
		DateTime ᗸ = _ᗸ;
		if (__2535 != null)
		{
			if (base.IsEmpty)
			{
				__2535.IsEmpty = true;
			}
			else
			{
				__2535.Value = ᗸ.Year;
			}
		}
		if (__2534 != null)
		{
			if (base.IsEmpty)
			{
				__2534.IsEmpty = true;
			}
			else
			{
				__2534.Value = ᗸ.Month;
			}
		}
		if (__2533 != null)
		{
			if (base.IsEmpty)
			{
				__2533.IsEmpty = true;
			}
			else if (base.FocusedItem != __2533 || !(__2533 is NumericDayInput) || !__253A)
			{
				__2533.Value = ᗸ.Day;
			}
		}
		if (__2536 != null)
		{
			if (base.IsEmpty)
			{
				__2536.IsEmpty = true;
			}
			else
			{
				if (__2536 is NumericHourInput)
				{
					((NumericHourInput)__2536).Period = eHourPeriod.AM;
				}
				__2536.Value = ᗸ.Hour;
				if (__2536 is NumericHourInput && !((NumericHourInput)__2536).Is24HourFormat && ᗸ.Hour >= 12)
				{
					((NumericHourInput)__2536).Period = eHourPeriod.PM;
				}
			}
		}
		if (__2537 != null)
		{
			if (base.IsEmpty)
			{
				__2537.IsEmpty = true;
			}
			else
			{
				__2537.Value = ᗸ.Minute;
			}
		}
		if (__2538 != null)
		{
			if (base.IsEmpty)
			{
				__2538.IsEmpty = true;
			}
			else
			{
				__2538.Value = ᗸ.Second;
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
				dateTimePartInput.Value = (int)(base.IsEmpty ? ((DayOfWeek)(-1)) : _ᗸ.DayOfWeek);
			}
			else if (dateTimePartInput.Part == eDateTimePart.DayOfYear)
			{
				if (base.IsEmpty)
				{
					dateTimePartInput.IsEmpty = true;
				}
				else
				{
					dateTimePartInput.Value = _ᗸ.DayOfYear;
				}
			}
		}
		InvalidateArrange();
	}

	protected override void ResetValue()
	{
		__253C = true;
		try
		{
			if (base.AllowEmptyState)
			{
				if (__2533 != null)
				{
					__2533.IsEmpty = true;
				}
				if (__2534 != null)
				{
					__2534.IsEmpty = true;
				}
				if (__2535 != null)
				{
					__2535.IsEmpty = true;
				}
				if (__2536 != null)
				{
					__2536.IsEmpty = true;
				}
				if (__2537 != null)
				{
					__2537.IsEmpty = true;
				}
				if (__2538 != null)
				{
					__2538.IsEmpty = true;
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
				_ᗸ = DateTime.MinValue;
			}
			else
			{
				_254C();
			}
		}
		finally
		{
			__253C = false;
		}
		OnValueChanged();
	}

	private void _2549()
	{
		_254B();
	}

	private void _254A()
	{
		_254B();
	}

	private void _254B()
	{
		foreach (VisualItem item in Items)
		{
			if (item is DateTimeGroup dateTimeGroup)
			{
				dateTimeGroup.MinDate = __253D;
				dateTimeGroup.MaxDate = __253E;
			}
		}
	}

	protected override void OnAllowEmptyStateChanged()
	{
		if (!base.AllowEmptyState && base.IsEmpty)
		{
			_254C();
		}
		base.OnAllowEmptyStateChanged();
	}

	private void _254C()
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

	internal override void ᡟ(KeyEventArgs ؿ)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Invalid comparison between Unknown and I4
		if ((int)ؿ.KeyCode != 8 && (int)ؿ.KeyCode != 46)
		{
			base.IsUserInput = true;
		}
		base.ᡟ(ؿ);
		base.IsUserInput = false;
	}

	internal override void ᡡ(KeyPressEventArgs ؿ)
	{
		base.IsUserInput = true;
		base.ᡡ(ؿ);
		base.IsUserInput = false;
	}

	internal override bool ᘭ(ref Message ᘮ, Keys ᘯ)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		base.IsUserInput = true;
		bool result = base.ᘭ(ref ᘮ, ᘯ);
		base.IsUserInput = false;
		return result;
	}

	internal override void ᡙ(MouseEventArgs ؿ)
	{
		base.IsUserInput = true;
		base.ᡙ(ؿ);
		base.IsUserInput = false;
	}
}
