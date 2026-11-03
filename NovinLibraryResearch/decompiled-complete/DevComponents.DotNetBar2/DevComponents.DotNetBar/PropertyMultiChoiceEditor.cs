using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;

namespace DevComponents.DotNetBar;

public class PropertyMultiChoiceEditor : PropertyValueEditor
{
	private class _2EC2 : ItemContainer, IPropertyValueEditor
	{
		private KeyValuePair<object, string>[] __2EC3;

		private bool __2EC4;

		private Color __0CD1 = Color.Black;

		private bool __2EC5;

		private char ኬ = ',';

		private EventHandler _2EBE;

		public Font EditorFont
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public bool IsEditorFocused => false;

		public object EditValue
		{
			get
			{
				object obj = null;
				foreach (CheckBoxItem subItem in SubItems)
				{
					if (subItem.Checked)
					{
						if (!__2EC4)
						{
							obj = subItem.Tag;
							break;
						}
						if (obj == null)
						{
							obj = subItem.Tag;
						}
						else if (obj is string)
						{
							obj = (string)obj + ኬ + (string)subItem.Tag;
						}
						else if (obj is uint)
						{
							obj = (uint)obj | (uint)subItem.Tag;
						}
						else if (obj is int)
						{
							obj = (int)obj | (int)subItem.Tag;
						}
						else if (obj is long)
						{
							obj = (long)obj | (long)subItem.Tag;
						}
					}
				}
				return obj;
			}
			set
			{
				__2EC5 = true;
				List<string> list = null;
				if (__2EC4)
				{
					if (value is string)
					{
						list = new List<string>(value.ToString().Split(new char[1] { ኬ }));
					}
					else if (value is string[])
					{
						list = new List<string>((string[])value);
					}
				}
				foreach (CheckBoxItem subItem in SubItems)
				{
					if (__2EC4)
					{
						if (list != null && list.Contains(subItem.Tag.ToString()))
						{
							subItem.Checked = true;
						}
						else if (subItem.Checked)
						{
							subItem.Checked = false;
						}
					}
					else
					{
						if (value == subItem.Tag || (value != null && value.Equals(subItem.Tag)))
						{
							subItem.Checked = true;
							break;
						}
						subItem.Checked = false;
					}
				}
				__2EC5 = false;
			}
		}

		public event EventHandler EditValueChanged
		{
			add
			{
				EventHandler eventHandler = _2EBE;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange(ref _2EBE, value2, eventHandler2);
				}
				while ((object)eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler eventHandler = _2EBE;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange(ref _2EBE, value2, eventHandler2);
				}
				while ((object)eventHandler != eventHandler2);
			}
		}

		public _2EC2(KeyValuePair<object, string>[] itemsDefinition, bool isMultiChoice, Color textColor, string initialSelection)
		{
			__2EC3 = itemsDefinition;
			__2EC4 = isMultiChoice;
			__0CD1 = textColor;
			_2EC6(initialSelection);
		}

		private void _2EC6(string _2EC7)
		{
			LayoutOrientation = eOrientation.Vertical;
			SubItems.Clear();
			base.ItemSpacing = 0;
			KeyValuePair<object, string>[] array = __2EC3;
			for (int i = 0; i < array.Length; i++)
			{
				KeyValuePair<object, string> keyValuePair = array[i];
				CheckBoxItem checkBoxItem = new CheckBoxItem();
				checkBoxItem.Text = keyValuePair.Value;
				if (checkBoxItem.Text == _2EC7)
				{
					checkBoxItem.Checked = true;
				}
				checkBoxItem.Tag = keyValuePair.Key;
				checkBoxItem.TextColor = __0CD1;
				if (__2EC4)
				{
					checkBoxItem.CheckBoxStyle = eCheckBoxStyle.CheckBox;
				}
				else
				{
					checkBoxItem.CheckBoxStyle = eCheckBoxStyle.RadioButton;
				}
				checkBoxItem.CheckedChanged += ⅎ;
				SubItems.Add(checkBoxItem);
			}
			Style = eDotNetBarStyle.StyleManagerControlled;
		}

		private void ⅎ(object _٠, CheckBoxChangeEventArgs ؿ)
		{
			if (!__2EC5)
			{
				_2EBF(EventArgs.Empty);
			}
		}

		public void FocusEditor()
		{
		}

		private void _2EBF(EventArgs ؿ)
		{
			_2EBE?.Invoke(this, ؿ);
		}
	}

	private KeyValuePair<object, string>[] _Items;

	private Color _TextColor = Color.Black;

	private bool _MultiSelect;

	public PropertyMultiChoiceEditor()
	{
	}

	public PropertyMultiChoiceEditor(KeyValuePair<object, string>[] items)
	{
		_Items = items;
	}

	public PropertyMultiChoiceEditor(KeyValuePair<object, string>[] items, Color textColor)
	{
		_Items = items;
		_TextColor = textColor;
	}

	public PropertyMultiChoiceEditor(KeyValuePair<object, string>[] items, Color textColor, bool multiSelect)
	{
		_Items = items;
		_TextColor = textColor;
		_MultiSelect = multiSelect;
	}

	public PropertyMultiChoiceEditor(KeyValuePair<object, string>[] items, bool multiSelect)
	{
		_Items = items;
		_MultiSelect = multiSelect;
	}

	public PropertyMultiChoiceEditor(string commaSeparetedItemList)
		: this(commaSeparetedItemList, Color.Black)
	{
	}

	public PropertyMultiChoiceEditor(string commaSeparetedItemList, bool multiSelect)
		: this(commaSeparetedItemList, Color.Black, multiSelect)
	{
	}

	public PropertyMultiChoiceEditor(string commaSeparatedItemList, Color textColor)
		: this(commaSeparatedItemList, textColor, multiSelect: false)
	{
	}

	public PropertyMultiChoiceEditor(string commaSeparatedItemList, Color textColor, bool multiSelect)
	{
		string[] array = commaSeparatedItemList.Split(new char[1] { ',' });
		_Items = new KeyValuePair<object, string>[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			ref KeyValuePair<object, string> reference = ref _Items[i];
			reference = new KeyValuePair<object, string>(array[i], array[i]);
		}
		_TextColor = textColor;
		_MultiSelect = multiSelect;
	}

	public PropertyMultiChoiceEditor(string[] values, object[] keys)
		: this(values, keys, Color.Black)
	{
	}

	public PropertyMultiChoiceEditor(string[] displayValues, object[] keys, Color textColor)
	{
		if (displayValues.Length != keys.Length)
		{
			throw new ArgumentException("values.Length must be same as keys.Length");
		}
		if (displayValues.Length == 0)
		{
			throw new ArgumentException("values.Length must be greater than 0");
		}
		_Items = new KeyValuePair<object, string>[displayValues.Length];
		for (int i = 0; i < displayValues.Length; i++)
		{
			ref KeyValuePair<object, string> reference = ref _Items[i];
			reference = new KeyValuePair<object, string>(keys[i], displayValues[i]);
		}
		_TextColor = textColor;
	}

	public override IPropertyValueEditor CreateEditor(PropertyDescriptor propertyDescriptor, object targetObject)
	{
		KeyValuePair<object, string>[] array = null;
		bool isMultiChoice = _MultiSelect;
		string initialSelection = string.Empty;
		if (_Items == null)
		{
			if (!propertyDescriptor.PropertyType.IsEnum)
			{
				array = new KeyValuePair<object, string>[1]
				{
					new KeyValuePair<object, string>(null, "Values not provided")
				};
			}
			else
			{
				string[] names = Enum.GetNames(propertyDescriptor.PropertyType);
				Array values = Enum.GetValues(propertyDescriptor.PropertyType);
				array = new KeyValuePair<object, string>[names.Length];
				for (int i = 0; i < values.Length; i++)
				{
					ref KeyValuePair<object, string> reference = ref array[i];
					reference = new KeyValuePair<object, string>(values.GetValue(i), names[i]);
				}
				if (propertyDescriptor.Attributes[typeof(FlagsAttribute)] is FlagsAttribute)
				{
					isMultiChoice = true;
				}
			}
		}
		else
		{
			array = _Items;
		}
		if (targetObject != null)
		{
			initialSelection = ((!propertyDescriptor.PropertyType.IsEnum) ? ((string)propertyDescriptor.GetValue(targetObject)) : Enum.GetName(propertyDescriptor.PropertyType, propertyDescriptor.GetValue(targetObject)));
		}
		return new _2EC2(array, isMultiChoice, _TextColor, initialSelection);
	}
}
