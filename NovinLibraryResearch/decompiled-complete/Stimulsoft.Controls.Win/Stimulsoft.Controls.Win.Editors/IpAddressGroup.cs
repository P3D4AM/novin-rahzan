using System;

namespace Stimulsoft.Controls.Win.Editors;

public class IpAddressGroup : VisualInputGroup
{
	private string _Value;

	private bool _UpdatingInputItems;

	public string Value
	{
		get
		{
			if (base.IsFocused && IsInputValid())
			{
				return GetCurrentInputValue();
			}
			return _Value;
		}
		set
		{
			bool flag = _Value != value;
			if (!string.IsNullOrEmpty(value) && !IsValueValid(value))
			{
				throw new ArgumentException("Value is not valid IP value");
			}
			_Value = value;
			UpdateInputItems();
			if (flag)
			{
				OnValueChanged(EventArgs.Empty);
			}
		}
	}

	public event EventHandler ValueChanged;

	public IpAddressGroup()
	{
		for (int i = 0; i < 4; i++)
		{
			VisualIntegerInput item = new VisualIntegerInput
			{
				MaxValue = 255,
				MinValue = 0
			};
			Items.Add(item);
			if (i < 3)
			{
				VisualLabel item2 = new VisualLabel
				{
					Text = "."
				};
				Items.Add(item2);
			}
		}
	}

	private void UpdateInputItems()
	{
		if (_UpdatingInputItems)
		{
			return;
		}
		_UpdatingInputItems = true;
		try
		{
			if (string.IsNullOrEmpty(_Value))
			{
				for (int i = 0; i < Items.Count; i++)
				{
					if (Items[i] is VisualIntegerInput)
					{
						VisualIntegerInput visualIntegerInput = Items[i] as VisualIntegerInput;
						visualIntegerInput.IsEmpty = true;
					}
				}
				return;
			}
			int[] array = ParseIPv4Value(_Value);
			int num = 0;
			for (int j = 0; j < Items.Count; j++)
			{
				if (Items[j] is VisualIntegerInput)
				{
					VisualIntegerInput visualIntegerInput2 = Items[j] as VisualIntegerInput;
					visualIntegerInput2.Value = array[num];
					num++;
				}
			}
		}
		finally
		{
			_UpdatingInputItems = false;
		}
	}

	private void OnValueChanged(EventArgs e)
	{
		EventHandler valueChanged = this.ValueChanged;
		if (valueChanged != null)
		{
			this.ValueChanged(this, e);
		}
	}

	internal bool IsValueValid(string ipValue)
	{
		int[] array = ParseIPv4Value(ipValue);
		return array != null;
	}

	private int[] ParseIPv4Value(string ipValue)
	{
		if (string.IsNullOrEmpty(ipValue))
		{
			return null;
		}
		string[] array = ipValue.Split(new char[1] { '.' });
		if (array.Length != 4)
		{
			return null;
		}
		int[] array2 = new int[4];
		for (int i = 0; i < array.Length; i++)
		{
			int result = 0;
			if (int.TryParse(array[i], out result))
			{
				if (result < 0 || result > 255)
				{
					return null;
				}
				array2[i] = result;
				continue;
			}
			return null;
		}
		return array2;
	}

	private bool IsInputValid()
	{
		bool flag = true;
		for (int i = 0; i < Items.Count; i++)
		{
			if (!(Items[i] is VisualIntegerInput visualIntegerInput))
			{
				continue;
			}
			if (visualIntegerInput.IsEmpty)
			{
				if (!flag && base.AllowEmptyState)
				{
					return false;
				}
				continue;
			}
			if (flag && i > 0)
			{
				return false;
			}
			flag = false;
		}
		return true;
	}

	protected override void OnLostFocus()
	{
		UpdateValue();
		base.OnLostFocus();
	}

	protected override void OnInputChanged(VisualInputBase input)
	{
		if (!_UpdatingInputItems && GetCurrentInputValue() != null)
		{
			_UpdatingInputItems = true;
			try
			{
				UpdateValue();
			}
			finally
			{
				_UpdatingInputItems = false;
			}
		}
		base.OnInputChanged(input);
	}

	private string GetCurrentInputValue()
	{
		int[] array = new int[4];
		int num = 0;
		bool flag = false;
		for (int i = 0; i < Items.Count; i++)
		{
			if (!(Items[i] is VisualIntegerInput visualIntegerInput))
			{
				continue;
			}
			if (visualIntegerInput.IsEmpty)
			{
				if (base.AllowEmptyState)
				{
					flag = true;
					break;
				}
				array[num] = 0;
			}
			array[num] = visualIntegerInput.Value;
			num++;
		}
		if (flag)
		{
			return null;
		}
		string text = "";
		for (int j = 0; j < array.Length; j++)
		{
			text += array[j];
			if (j < array.Length - 1)
			{
				text += ".";
			}
		}
		return text;
	}

	private void UpdateValue()
	{
		Value = GetCurrentInputValue();
	}

	protected override void ResetValue()
	{
		Value = null;
	}
}
